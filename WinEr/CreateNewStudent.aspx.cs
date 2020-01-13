using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using AjaxControlToolkit;
using System.Drawing;
using System.IO;
using WinBase;
using System.Xml;
using WinEr;

namespace WinEr
{
    public partial class CreateNewStudent : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private FeeManage MyFeeMang;
        private Incident MyIncedent;   
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet = null;
        private TextBox[] dynamicTextBoxes;
        private int[] Mandatoryflag;
        private string[] FealdName;
        private int CustfieldCount;
        private SchoolClass objSchool = null;
        public event EventHandler EVNTSave;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (WinerUtlity.NeedCentrelDB())
            {
                if (Session[WinerConstants.SessionSchool] == null)
                {
                    Response.Redirect("Logout.aspx");
                }
                objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
            }

            if (!IsPostBack)
            {
                bool Tempvalid = false;
                string TempStudId ="";
               
               
                LoadInitialValues();
                
                Txt_Name.Focus();
                if (MyUser.HaveActionRignt(126))
                {
                    Img_AddTempStd.Visible = true;
                    Lnk_AddTemp.Visible = true;
                }

                else
                {
                    Img_AddTempStd.Visible = false;
                    Lnk_AddTemp.Visible = false;
                }
                if (MyUser.HaveActionRignt(2))
                {
                    Img_CollectFee.Visible = true;
                }
                else
                {
                    Img_CollectFee.Visible = false;
                }
                if (Session["TempId"] != null && Session["ViewDs"] != null)
                {

                    DataSet StudentRegistrationDS = new DataSet();
                    StudentRegistrationDS = (DataSet)Session["ViewDs"];
                    string TempId = Session["TempId"].ToString();
                    Session["TempId"] = null;

                    LoadTempStudentDetails(StudentRegistrationDS, TempId);
                }

                if (Request.QueryString["TempStudId"] != null)
                {
                    //int.TryParse(Request.QueryString["TempStudId"].ToString(), out TempStudId);
                     TempStudId = Request.QueryString["TempStudId"];
                    if (TempStudId !="")
                    {
                        Tempvalid = true;
                    }
                    if (Tempvalid)
                    {
                        LoadAllDetails(TempStudId.ToString());
                        

                    }
                }
                
              
                LoadStudentLimitWarningMessage();
                LoadStudentIdMandatory();
                //pnlAjaxUpdaet.Triggers.Add(new PostBackTrigger()
                //{
                //    ControlID = Btn_Upload
                //});

                //ScriptManager.GetCurrent(this).RegisterPostBackControl(FileUp_Student);

                //PostBackTrigger trigger = new PostBackTrigger();
                //trigger.ControlID = btn[q].UniqueID;
                //trigger.EventName = "Click";
                //pnlAjaxUpdaet.Triggers.Add(trigger);
                Page.Form.Enctype = "multipart/form-data";


            }

        }

        private void LoadStudentIdMandatory()
        {
            string sql = "";
            OdbcDataReader Configvaluereader = null;
            int value = 0;
            sql = "Select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='IsStudentIdMandatory'";
            Configvaluereader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (Configvaluereader.HasRows)
            {
                int.TryParse(Configvaluereader.GetValue(0).ToString(), out value);
                Hdn_ManStudId.Value = value.ToString();
                if (value == 1)
                {
                    Lbl_Manstudid.Text = "*";
                    RequiredFieldValidator6.ErrorMessage = "Enter Student ID";
                    RequiredFieldValidator6.Enabled = true;
                }
                else
                {
                    Lbl_Manstudid.Text = "";
                    RequiredFieldValidator6.ErrorMessage = "";
                    RequiredFieldValidator6.Enabled = false;
                }
            }

        }

        private void LoadTempStudentDetails(DataSet StudentRegistrationDS, string TempId)
        {
            if (StudentRegistrationDS != null && StudentRegistrationDS.Tables[0].Rows.Count > 0)
            {
                Hdn_TempId.Value = TempId;
                string _sql = "";
                OdbcDataReader Idreader = null;
                _sql = "select ID from tbltempstdent where tbltempstdent.TempID='" + TempId + "'";
                Idreader = MyStudMang.m_MysqlDb.ExecuteQuery(_sql);
                if (Idreader.HasRows)
                {
                    Hdn_Studid.Value = Idreader.GetValue(0).ToString();
                }
                Hdn_ststus.Value = "1";
                foreach (DataRow dr in StudentRegistrationDS.Tables[0].Rows)
                {
                    if (dr["ColumnName"].ToString() == "StudentName")
                    {
                        Txt_Name.Text = dr["Name"].ToString();
                    }
                    if (dr["ColumnName"].ToString() == "Dob")
                    {
                        Txt_Dob.Text = dr["Name"].ToString();
                    }                   
                    if (dr["ColumnName"].ToString() == "PemanentAddress")
                    {
                        Txt_Address.Text = dr["Name"].ToString();
                    }
                    if (dr["ColumnName"].ToString() == "MobileNum")
                    {
                        Txt_OfficePh.Text = dr["Name"].ToString();
                    }
                    if (dr["ColumnName"].ToString() == "EmaidID")
                    {
                        Txt_Email.Text= dr["Name"].ToString();
                    }
                    if (dr["ColumnName"].ToString() == "Standard")
                    {
                        Drp_Std.SelectedValue = dr["Name"].ToString();
                        OdbcDataReader ClassReader = null;
                        AddClassToDrpList(0);
                        string sql = "";
                        sql = "select tbltempstdent.Class from tbltempstdent where tbltempstdent.TempId='" + Hdn_TempId.Value + "'";                       
                        ClassReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                        if (ClassReader.HasRows)
                        {
                            int ClassId;
                            int.TryParse(ClassReader.GetValue(0).ToString(), out ClassId);
                            if (ClassId != 0)
                            {
                                Drp_Class.SelectedValue = ClassId.ToString();
                            }
                        }
                    }
                    if (dr["ColumnName"].ToString() == "BatchId")
                    {
                        Drp_JoinBatch.SelectedValue = dr["Name"].ToString();
                    }
                    if (dr["ColumnName"].ToString() == "Gender")
                    {
                       // RadioBtn_Sex.SelectedValue = dr["Name"].ToString();
                        if (dr["Name"].ToString() == "0")
                        {
                            RadioBtn_Sex.Items[0].Selected = true;
                        }
                        else
                        {
                            RadioBtn_Sex.Items[1].Selected = true;
                        }

                    }
                    if (dr["ColumnName"].ToString() == "BloodGroup")
                    {
                        Drp_BloodGrp.SelectedValue = dr["Name"].ToString();

                    }
                    if (dr["ColumnName"].ToString() == "Nationality")
                    {
                        Txt_Nationality.Text= dr["Name"].ToString();
                    }
                    if (dr["ColumnName"].ToString() == "MotherTongue")
                    {
                        Drp_MotherTongue.SelectedValue= dr["Name"].ToString();
                    }
                    if (dr["ColumnName"].ToString() == "DrpReligion")
                    {
                        Drp_Religion.SelectedValue = dr["Name"].ToString();
                    }
                    //if (dr["ColumnName"].ToString() == "OthrRelgn")
                    //{
                    //    Txt_Religion.Text = dr["Name"].ToString();
                    //}
                    if (dr["ColumnName"].ToString() == "Caste")
                    {
                        Drp_Caste.SelectedValue = dr["Name"].ToString();
                    }
                    if (dr["ColumnName"].ToString() == "GuardianName")
                    {
                        Txt_FGName.Text = dr["Name"].ToString();
                    }
                    if (dr["ColumnName"].ToString() == "MothrName")
                    {
                        Txt_MotherName.Text = dr["Name"].ToString();
                    }
                    if (dr["ColumnName"].ToString() == "Presentaddress")
                    {
                        Txt_Address_Present.Text = dr["Name"].ToString();

                    }
                    if (dr["ColumnName"].ToString() == "State")
                    {
                       Txt_State.Text = dr["Name"].ToString();
                    }
                    if (dr["ColumnName"].ToString() == "Location")
                    {
                        Txt_Location.Text = dr["Name"].ToString();
                    }
                    if (dr["ColumnName"].ToString() == "Pin")
                    {
                        Txt_pin.Text= dr["Name"].ToString();
                    }
                    if (dr["ColumnName"].ToString() == "ResidencePhno")
                    {
                        Txt_ResidencePh.Text= dr["Name"].ToString();
                    }
                    if (dr["ColumnName"].ToString() == "FthrQlfn")
                    {
                        Txt_FatherEduQuali.Text = dr["Name"].ToString();
                    }
                    if (dr["ColumnName"].ToString() == "MthrQlfn")
                    {
                        Txt_MotherEduQuali.Text= dr["Name"].ToString();
                    }
                    if (dr["ColumnName"].ToString() == "FthrOccupn")
                    {
                       Txt_FatherOccupation.Text = dr["Name"].ToString();

                    }
                    if (dr["ColumnName"].ToString() == "FthrAnnualIncome")
                    {
                        Txt_AnualIncome.Text= dr["Name"].ToString();
                    }
                    if (dr["ColumnName"].ToString() == "BrotherNum")
                    {
                        Txt_NoBro.Text = dr["Name"].ToString();
                    }
                    if (dr["ColumnName"].ToString() == "SisterNum")
                    {
                       Txt_NoSys.Text= dr["Name"].ToString();
                    }
                    Session["ViewDs"] = null;
                    FreezeFields();
                }
            }
            
        }

        private void LoadStudentLimitWarningMessage()
        {
            int _TotalSeats = 0, _TotalStudents = 0;

            string sql = "select (select count(tblstudentclassmap.StudentId) from tblstudentclassmap where tblstudentclassmap.ClassId="+Drp_Class.SelectedValue+") as StudentCount, (select tblclass.TotalSeats from tblclass where tblclass.Id="+Drp_Class.SelectedValue+") as TotalSeats";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out _TotalStudents);
                int.TryParse(MyReader.GetValue(1).ToString(), out _TotalSeats);
                if (_TotalStudents >= _TotalSeats)
                {
                    lbl_ClassMsg.Text = "All seats are filled..";
                }
                else
                {
                    lbl_ClassMsg.Text = "";
                }
            }
        }

        private void GetWizardConfig()
        {
            int WizardStep = MyStudMang.GetWizardStep();
            if (WizardStep != 0)
            {
                Wzd_StudCreation.WizardSteps.Remove(Wzd_StudCreation.WizardSteps[WizardStep]);
            }

            if (MyStudMang.NeedStudentApprovel())
            {
                Wzd_StudCreation.WizardSteps.Remove(Wzd_StudCreation.WizardSteps[2]);
            }

        }

        private void LoadInitialValues()
        {
            AddStandardToDrpList(0);
            AddClassToDrpList(0);
            AddBloodGrpToDrpList(16);
            AddMotherTongueToDrpList(0);
            AddJoiningBatchToDrpList(0);
            AddJoiningStandardToDrpList(0);
            AddFirstLanguage(0);
            AddReligionToDrpList(0);
           // CheckCaste();
            LoadCast();
            CheckAutoAdmissionNo();
            LoadFeeSchedule();
            AddStudentTypeToDrpList(0);
            SetAdmissionDate();
        }

        private void SetAdmissionDate()
        {
            string sql = "SELECT tblconfiguration.Value FROM tblconfiguration WHERE tblconfiguration.Name='AdmissionDate'";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int _value = 0;
                int.TryParse(MyReader.GetValue(0).ToString(), out _value);
                if (_value == 1)
                {
                    Txt_JoiningDate.Text = General.GerFormatedDatVal(DateTime.Now.Date);
                }
            }
        }
       
        private void AddStudentTypeToDrpList(int _intex)
        {
            Drp_StudentType.Items.Clear();
            string sql = "SELECT Id,TypeName FROM tblstudtype";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_StudentType.Items.Add(li);

                }
                Drp_StudentType.SelectedIndex = _intex;
            }
        }

        private void LoadFeeSchedule()
        {
            if ((MyStudMang.IsQuickSchedule()) && (MyStudMang.FeeScheduledForTheClass(int.Parse(Drp_Class.SelectedValue), MyUser.CurrentBatchId)))
            {
              
                LoadFeeGrid(int.Parse(Drp_Class.SelectedValue));
                
            }
            else
            {
                Grd_Fees.Columns[1].Visible = true;
                Grd_Fees.Columns[2].Visible = true;
                Grd_Fees.DataSource = null;
                Grd_Fees.DataBind();
                Grd_Fees.Columns[1].Visible = false;
                Grd_Fees.Columns[2].Visible = false;
                Lbl_Fee.Text = "No fee to schedule";
                Lnk_Select.Text = "";
                Pnl_Feesched.Visible = false;

            }
        }

        private void LoadFeeGrid(int ClassId)
        {
            int NextBatch = MyUser.CurrentBatchId + 1;
            string sql = "select tblfeeschedule.Id,tblfeeschedule.FeeId, tblfeeaccount.AccountName, tblfeeschedule.Amount, date_format(tblfeeschedule.Duedate, '%d-%m-%Y') AS 'Duedate' , date_format(tblfeeschedule.LastDate, '%d-%m-%Y') AS 'LastDate',tblbatch.BatchName as Batch from tblfeeschedule inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblfeeasso on tblfeeasso.Id = tblfeeaccount.AssociatedId inner join tblbatch on tblbatch.Id = tblfeeschedule.BatchId where tblfeeaccount.`Status`=1 and (tblfeeschedule.BatchId=" + MyUser.CurrentBatchId + " or tblfeeschedule.BatchId=" + NextBatch + " ) and tblfeeschedule.ClassId=" + ClassId + " and tblfeeasso.Name='Class'";
            MyDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyDataSet.Tables[0].Rows.Count > 0)
            {
                Grd_Fees.Columns[1].Visible = true;
                Grd_Fees.Columns[2].Visible = true;
                Grd_Fees.DataSource = MyDataSet;
                Grd_Fees.DataBind();
                Grd_Fees.Columns[1].Visible = false;
                Grd_Fees.Columns[2].Visible = false;
                Lbl_Fee.Text = "";
                Lnk_Select.Text = "None";
                foreach (GridViewRow gv in Grd_Fees.Rows)
                {
                    CheckBox cb = (CheckBox)gv.FindControl("Chk_Fee");
                    cb.Checked = true;
                }
                Pnl_Feesched.Visible = true;
            }
            else
            {
                Grd_Fees.Columns[1].Visible = true;
                Grd_Fees.Columns[2].Visible = true;
                Grd_Fees.DataSource = null;
                Grd_Fees.DataBind();
                Grd_Fees.Columns[1].Visible = false;
                Grd_Fees.Columns[2].Visible = false;
                Lbl_Fee.Text = "No fee to schedule";
                Lnk_Select.Text = "";
                Pnl_Feesched.Visible = false;
                
            }

        }

        private void CheckCaste()
        {

            if (Drp_Religion.SelectedValue == "-1")
            {
                //Lbl_Religion.Visible = true;
                //Txt_Religion.Visible = true;
                //Lbltmp1.Visible = true;
                //Txt_ReligionRequiredFieldValidator6.Enabled = true;
                Lbl_Caste.Visible = true;
                Drp_Caste.Visible = true;
               
            }
            else
            {
                //LoadRelogionCast(int.Parse(Drp_Religion.SelectedValue));
                //Lbl_Religion.Visible = false;
                //Txt_Religion.Visible = false;
                //Lbltmp1.Visible = false;
                //Txt_ReligionRequiredFieldValidator6.Enabled = false;

            }


        }

        private void LoadCast()
        {
            Drp_Caste.Items.Clear();
            string sql = "select tblcast.Id, tblcast.castname from tblcast  where tblcast.castname <>'Other' order by tblcast.castname ASC";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            
            if (MyReader.HasRows)
            {
                
                
                //Txt_Cast.Visible = false;
                //Label_Caste.Visible = false;
                //Lbltmp3.Visible = false;
                //Txt_CastRequiredFieldValidator6.Enabled = false;
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Caste.Items.Add(li);

                }
                
                
            }
            
            //Drp_Caste.Items.Add(new ListItem("UNKNOWN", "0"));
            //Drp_Caste.Items.Add(new ListItem("Others", "-1"));
        }

        //protected void Drp_Religion_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //   // CheckCaste();
        //    Drp_Caste.Focus();
        //}

        //protected void Drp_Cast_SelecteIndexchang(object sender, EventArgs e)
        //{
        //    if (Drp_Caste.SelectedValue != "-1")
        //    {

        //        Txt_Cast.Visible = false;
        //        Label_Caste.Visible = false;
        //        Lbltmp3.Visible = false;
        //        Txt_CastRequiredFieldValidator6.Enabled = false;
        //        Txt_Address.Focus();
        //    }
        //    else
        //    {
        //        Txt_Cast.Visible = true;
        //        Label_Caste.Visible = true;
        //        Lbltmp3.Visible = true;
        //        Txt_CastRequiredFieldValidator6.Enabled = true;
        //        Txt_Cast.Focus();
        //    }
           
        //}

        private void AddClassToDrpList(int _intex)
        {

            Drp_Class.Items.Clear();
            if (Drp_Std.SelectedValue != "")
            {
                string sql = "SELECT tblclass.Id,tblclass.ClassName FROM tblclass where tblclass.Status AND tblclass.Standard='" + int.Parse(Drp_Std.SelectedValue.ToString()) + "' AND tblclass.Id IN (SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + "))";
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_Class.Items.Add(li);

                    }
                    Drp_Class.SelectedIndex = _intex;
                }
                else
                {
                    ListItem li = new ListItem("No Class Found", "-1");
                    Drp_Class.Items.Add(li);
                }
            }


        }
  
        private void AddStandardToDrpList(int _intex)
        {

            Drp_Std.Items.Clear();

            string sql = "SELECT DISTINCT tblstandard.Id, tblstandard.Name from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Std.Items.Add(li);

                }
                Drp_Std.SelectedIndex = _intex;
            }
            else
            {
                ListItem li = new ListItem("No Standard Found", "-1");
                Drp_Std.Items.Add(li);
            }


        }

        private void AddReligionToDrpList(int _intex)
        {
            Drp_Religion.Items.Clear();
            string sql = "SELECT Id,Religion FROM tblreligion where Religion <>'Other' ";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Religion.Items.Add(li);

                }

                Drp_Religion.SelectedIndex = _intex;
            }
            //Drp_Religion.Items.Add(new ListItem("Others", "-1"));
        }

        private void CheckAutoAdmissionNo()
        {
            if (MyStudMang.AutoAdmissionNoTrue())
            {
                Txt_AdminNo.Visible = false;
                Lbltemp4.Visible = false;
                LblAdmission.Visible = false;
                Txt_AdminNoRequiredFieldValidator6.Enabled = false;
            }
            else
            {
                Txt_AdminNo.Visible = true;
                Lbltemp4.Visible = true;
                LblAdmission.Visible = true;
            }
        }
     
        private void AddFirstLanguage(int _intex)
        {
            Drp_FirstLanguage.Items.Clear();
            string sql = "SELECT Id,Language FROM tbllanguage";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_FirstLanguage.Items.Add(li);

                }
                Drp_FirstLanguage.SelectedIndex = _intex;
            }
        }


        private void AddJoiningStandardToDrpList(int _intex)
        {
            Drp_JoinStandard.Items.Clear();
            Drp_JoinStandard.Enabled = true;
            string sql;
            if (Chk_newadminsion.Checked)
            {

                Drp_JoinStandard.Enabled = false;
                ListItem li = new ListItem("", "-1");
                Drp_JoinStandard.Items.Add(li);
            }
            else
            {
                 sql = "SELECT DISTINCT tblstandard.Id, tblstandard.Name from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id";
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_JoinStandard.Items.Add(li);

                    }
                    Drp_JoinStandard.SelectedIndex = _intex;
                }
                else
                {
                    ListItem li = new ListItem("No Standard Found", "-1");
                    Drp_JoinStandard.Items.Add(li);
                }
            }

        }


        private void AddJoiningBatchToDrpList(int _intex)
        {
            Drp_JoinBatch.Items.Clear();
            string sql;
            MyUser.LoadCurrentbatchId();
            if (Chk_newadminsion.Checked)
            {
               
                ListItem li = new ListItem(MyUser.CurrentBatchName, MyUser.CurrentBatchId.ToString());
                Drp_JoinBatch.Items.Add(li);
            }
            else
            {
                sql = "SELECT Id,BatchName FROM tblbatch WHERE Created=1 AND Status<>1";
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_JoinBatch.Items.Add(li);

                    }
                    Drp_JoinBatch.SelectedIndex = _intex;
                }
            }
           
        }

        private void AddMotherTongueToDrpList(int _intex)
        {
            Drp_MotherTongue.Items.Clear();
            string sql = "SELECT Id,Language FROM tbllanguage";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_MotherTongue.Items.Add(li);

                }
                Drp_MotherTongue.SelectedIndex = _intex;
            }
        }

        private void AddBloodGrpToDrpList(int _intex)
        {
            Drp_BloodGrp.Items.Clear();
            if (Drp_Std.SelectedValue != "")
            {
                string sql = "SELECT Id,GroupName FROM tblbloodgrp";
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_BloodGrp.Items.Add(li);

                    }
                    Drp_BloodGrp.SelectedIndex = _intex;
                }
            }

        }

        protected void Lnk_Cam_Click(object sender, EventArgs e)
        {
            Session["SaveType"] = "StudId";
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "window.open('silverlitepicturecapture.aspx','','width=700,height=500')", true);
        }
        protected void Btn_Upload_Click(object sender, EventArgs e)
        {
            if (FileUp_Student.PostedFile != null && !ValidImageFile())
            {
                Lbl_msg.Text = "File type cannot be uploaded";
                this.MPE_MessageBox.Show();

            }
            else if (FileUp_Student.PostedFile == null)
            {
                Lbl_msg.Text = "Please Select an image to upload";
                this.MPE_MessageBox.Show();
            }
            else
            {
                int _StudentId = int.Parse(Hdn_StudentId.Value);
                AddPhoto(_StudentId);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Upload Photo", "Photo Of Student " + MyUser.m_DbLog.GetStudentName(_StudentId) + " is changed", 1);
                if (EVNTSave != null)
                {
                    EVNTSave(this, e);
                }
                Label_text.Text = "Image Uploaded.";
                //this.MPE_MessageBox.Show();
                //Lbl_msg.Text = "Please Enter the Correct OLD password";
                // MPE_MessageBox.Show();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "modalpopup();", true);
            }

        }
        protected void Btn_okclick(object sender, EventArgs e)
        {
            Response.Redirect("createnewstudent.aspx");



        }
        private bool ValidImageFile()
        {
            bool fileOK = false;
            string fileExtension = System.IO.Path.GetExtension(FileUp_Student.FileName).ToLower();
            string[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg" };
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    fileOK = true;
                }

            }
            return fileOK;
        }
        private void AddPhoto(int _StudentId)
        {
            string ImageUrl = "";
            String Sql = "";
            string preimage = "";
            int UsrId;
            string ImageName = FileUp_Student.FileName.ToString();
            FileUp_Student.SaveAs(MyUser.FilePath + "\\UpImage\\" + ImageName);
            // string strVirtualPath="http://localhost:1334/WinSchool/UpImage/" + ImageName;
            string ThumbnailPath = (MyUser.FilePath + "\\ThumbnailImages\\" + "Student" + _StudentId.ToString() + ImageName);
            using (System.Drawing.Image Img = System.Drawing.Image.FromFile(MyUser.FilePath + "\\UpImage\\" + ImageName))
            {
                Size ThumbNailSize = NewImageSize(Img.Height, Img.Width, 200);
                using (System.Drawing.Image ImgThnail =
                new Bitmap(Img, ThumbNailSize.Width, ThumbNailSize.Height))
                {
                    ImgThnail.Save(ThumbnailPath, Img.RawFormat);
                    ImgThnail.Dispose();
                }
                Img.Dispose();
                // File.Delete(Server.MapPath("../UpImage/" + ImageName));
                //DeleteFile(strVirtualPath);
            }
            ImageUrl = "Student" + _StudentId + ImageName;

            UsrId = _StudentId;




            bool success = false;
            if (MyStudMang.HasImage(UsrId, out preimage))
            {
                // Sql = "UPDATE tblfileurl SET FilePath='" + ImageUrl + "' WHERE Type='StudentImage' AND UserId=" + _StudentId;

                byte[] imagebytes = General.getImageinBytefromImage(ThumbnailPath);

                ImageUploaderClass imgUpload = new ImageUploaderClass(objSchool);

                success = imgUpload.UpdateImageFile(imagebytes, _StudentId, "StudentImage");
            }
            else
            {
                //  Sql = "INSERT INTO tblfileurl(UserId,FilePath,Type) VALUES(" + _StudentId + ", '" + ImageUrl + "','StudentImage')";

                byte[] imagebytes = General.getImageinBytefromImage(ThumbnailPath);

                ImageUploaderClass imgUpload = new ImageUploaderClass(objSchool);

                success = imgUpload.InsertImageFile(imagebytes, _StudentId, "StudentImage");
            }


            if (ImageName != "")
            {
                File.Delete(MyUser.FilePath + "\\UpImage\\" + ImageName);
            }
            if ((preimage != "") && (preimage != ImageUrl))
            {
                File.Delete(MyUser.FilePath + "\\ThumbnailImages\\" + preimage);
            }

        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MyFeeMang = MyUser.GetFeeObj();
            MyIncedent = MyUser.GetIncedentObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else
            {
                if (Request.QueryString["TempStudId"] == null)
                {
                    AddCoustomControls();

                }

                else if(Request.QueryString["TempStudId"] != null)
                {


                    if (Session["TempId"] != null)
                    {
                        LoadCoustomFields();

                    }

                    

                }



                //if (Session["TempId"] == null )
                //{
                //  //  AddCoustomControls();
                //}
  
                GetWizardConfig();               
            }           
        }

        private void AddCoustomControls()
        {
            CustfieldCount = MyStudMang.CoustomFieldCount;
            if (CustfieldCount == 0)
            {
                Label lbicusnote = new Label();
                lbicusnote.ID = "lbicusnote";
                lbicusnote.Text = "No Coustom Fields Present.";
                myPlaceHolder.Controls.Add(lbicusnote);
                Wzd_StudCreation.WizardSteps.Remove(Wzd_StudCreation.WizardSteps[2]);
            }
            else
            {

                dynamicTextBoxes = new TextBox[CustfieldCount];
                Mandatoryflag = new int[CustfieldCount];
                FealdName = new string[CustfieldCount];
                DataSet _CustomFields = MyStudMang.GetCuestomFields();
                if (_CustomFields != null && _CustomFields.Tables != null && _CustomFields.Tables[0].Rows.Count > 0)
                {

                    int i = 0;
                    Table tbl = new Table();
                    tbl.Width = 500;
                    myPlaceHolder.Controls.Add(tbl);


                    foreach (DataRow dr_fieldData in _CustomFields.Tables[0].Rows)
                    {

                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();
                        TableCell tc2 = new TableCell();
                        TableCell tc3 = new TableCell();
                        if (dr_fieldData[4].ToString() == "1")
                        {
                            tc1.Text = dr_fieldData[1].ToString() + "<span class=\"redcol\">*</span>";
                            RequiredFieldValidator ReqfldvalExt = new RequiredFieldValidator();
                            ReqfldvalExt.ID = "ReqfldvalExt" + i.ToString();
                            ReqfldvalExt.ControlToValidate = "myTextBox" + i.ToString();
                            ReqfldvalExt.ErrorMessage = "You Must enter " + dr_fieldData[1].ToString();
                            tc3.Controls.Add(ReqfldvalExt);
                        }
                        else
                        {
                            tc1.Text = dr_fieldData[1].ToString();
                        }
                        TextBox textBox = new TextBox();
                        textBox.MaxLength = int.Parse(dr_fieldData[3].ToString());
                        textBox.ID = "myTextBox" + i.ToString();
                        tc2.Controls.Add(textBox);
                        Mandatoryflag[i] = int.Parse(dr_fieldData[4].ToString());
                        FealdName[i] = dr_fieldData[0].ToString();
                        dynamicTextBoxes[i] = textBox;

                        FilteredTextBoxExtender FiltTxtbxExt = new FilteredTextBoxExtender();
                        FiltTxtbxExt.ID = "FiltTxtbxExt" + i.ToString();
                        if (dr_fieldData[2].ToString() == "1")
                        {

                            FiltTxtbxExt.FilterType = FilterTypes.Numbers;

                        }
                        else if (dr_fieldData[2].ToString() == "2")
                        {

                            FiltTxtbxExt.FilterType = FilterTypes.Custom;
                            FiltTxtbxExt.FilterMode = FilterModes.InvalidChars;
                            FiltTxtbxExt.InvalidChars = "'/\\";
                        }
                        else
                        {
                            FiltTxtbxExt.ValidChars = ".";
                            FiltTxtbxExt.FilterType = FilterTypes.Numbers | FilterTypes.Custom;

                        }
                        FiltTxtbxExt.TargetControlID = "myTextBox" + i.ToString();
                        myPlaceHolder.Controls.Add(FiltTxtbxExt);
                        
                        

                        tr.Cells.Add(tc1);
                        tr.Cells.Add(tc2);
                        tr.Cells.Add(tc3);
                        tbl.Rows.Add(tr);
                        i++;
                    }
                }

            }
        }

        protected void SideBarList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            try
            {
                WizardStep dataitem = (WizardStep)e.Item.DataItem;
                LinkButton lnkBtn = (LinkButton)e.Item.FindControl("SideBarButton");
                if (dataitem == null)
                {
                    lnkBtn.Enabled = (e.Item.ItemIndex <= Wzd_StudCreation.ActiveStepIndex);
                }
            }
            catch (Exception ex)
            {
                int a = 1;
            }
       
        }
  
        protected void Drp_Std_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddClassToDrpList(0);
            GetMessage();
            LoadFeeSchedule();
            Drp_Class.Focus();
            LoadStudentLimitWarningMessage();
        }

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetMessage();
            LoadFeeSchedule();
            LoadStudentLimitWarningMessage();
        }

        protected void Txt_Name_TextChanged(object sender, EventArgs e)
        {
            GetMessage();
        }

        private void GetMessage()
        {
            int classID = -1;
            string classname = "";
            string _Name = Txt_Name.Text.Trim();
            if (Drp_Class.SelectedValue != "")
            {
                classID = int.Parse(Drp_Class.SelectedValue.ToString());
                classname = MyStudMang.GetClassName(classID);
            }

            if ((classID != -1) && (_Name != "") && (MyStudMang.NameExistsInClass(_Name, classID)))
            {
                Lbl_Sudentname.Text = "A student with name '" + _Name + "' is already present in '" + classname + "'.";
                this.MPE_MessageBox3.Show();
            }
        }

        protected void Chk_newadminsion_CheckedChanged(object sender, EventArgs e)
        {
            AddJoiningBatchToDrpList(0);
            AddJoiningStandardToDrpList(0);
        }
        //protected void btn_addReligion(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "modalpopup();", true);
        //}
        protected void LnkSelBtn_Click(object sender, EventArgs e)
        {

            if (Lnk_Select.Text == "None")
            {
                Lnk_Select.Text = "Select All";
                foreach (GridViewRow gv in Grd_Fees.Rows)
                {
                    CheckBox cb = (CheckBox)gv.FindControl("Chk_Fee");
                    cb.Checked = false;
                }
            }
            else
            {
                Lnk_Select.Text = "None";
                foreach (GridViewRow gv in Grd_Fees.Rows)
                {
                    CheckBox cb = (CheckBox)gv.FindControl("Chk_Fee");
                    cb.Checked = true;
                }
            }
        }
        protected void btn_photoUpload(object sender, EventArgs e)
        {
            Pnl_StudentDtails.Visible = false;
            Pnl_studentPhoto.Visible = true;
            string studentidd = Hdn_StudentId.Value;

        }
        protected void Wzd_StudCreation_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            try
            {
               

                bool _continue = true;
                bool _isattendancemodule = MyUser.HaveModule(21);
                string studentid = Txt_StudentId.Text;
                string _message;
                int _userId;
                string _TempStudentId = "0";
                string _studname = Txt_Name.Text.ToUpper();
                string _sex = RadioBtn_Sex.SelectedValue.ToString();
                DateTime _Dob = MyUser.GetDareFromText(Txt_Dob.Text);
                string _Fathername = Txt_FGName.Text.ToUpper();
                string _address = Txt_Address.Text;
                int _joiningbatchid = int.Parse(Drp_JoinBatch.SelectedValue.ToString());
                int _joiningStandard = int.Parse(Drp_JoinStandard.SelectedValue.ToString());
                DateTime _Joindate = MyUser.GetDaTeFromText(Txt_JoiningDate.Text);

                int _StandardId = int.Parse(Drp_Std.SelectedValue.ToString());
                int _classid = int.Parse(Drp_Class.SelectedValue.ToString());
                string _ClassName = Drp_Class.SelectedItem.Text.Trim();
                string _admisionNo = Txt_AdminNo.Text;
                int _Bloodgroupid = 17;
                if (!int.TryParse(Drp_BloodGrp.SelectedValue.ToString(), out _Bloodgroupid))
                {
                    _Bloodgroupid = 17;
                }
                string _nationality = Txt_Nationality.Text.ToUpper();
                int mothertoungid = 8;
                if (!int.TryParse(Drp_MotherTongue.SelectedValue.ToString(), out mothertoungid))
                {
                    mothertoungid = 8;
                }
                string _mothername = Txt_MotherName.Text.ToUpper();
                string _Fathedu = Txt_FatherEduQuali.Text.ToUpper();
                string _mothedu = Txt_MotherEduQuali.Text.ToUpper();
                string _fatherOcc = Txt_FatherOccupation.Text.ToUpper();
                string _motherOcc = Txt_MotherOccupation.Text.ToUpper();
                string _aadharno = Txt_AadharNo.Text;
                double _Anualincom;
                if (!double.TryParse(Txt_AnualIncome.Text, out _Anualincom))
                {
                    _Anualincom = 0;
                }
                string _addrspresent = Txt_Address_Present.Text;
                string _location = Txt_Location.Text.ToUpper();
                string _State = Txt_State.Text.ToUpper();

                int _pin;
                if (!int.TryParse(Txt_pin.Text, out _pin))
                {
                    _pin = 0;
                }
                string _resedphon = Txt_ResidencePh.Text;
                string _officephon = Txt_OfficePh.Text;
                string _email = Txt_Email.Text;

                int nofbrother;
                if (!int.TryParse(Txt_NoBro.Text, out nofbrother))
                {
                    nofbrother = 0;
                }
                int nofsis;
                if (!int.TryParse(Txt_NoSys.Text, out nofsis))
                {
                    nofsis = 0;
                }

                int firstlng;
                if (!int.TryParse(Drp_FirstLanguage.SelectedValue, out firstlng))
                {
                    firstlng = 0;
                }

                int studcatogory;
                if (!int.TryParse(Drp_StudentType.SelectedValue, out studcatogory))
                {
                    studcatogory = 2;
                }

                int AdmissionType = 1;
                if (Chk_newadminsion.Checked)
                {
                    AdmissionType = 2;
                }


                _TempStudentId = Hdn_TempId.Value;
                if (ValidData(out _message))
                {

                    int RegionId, CastId;
                    if (GetRearionAndCast(out RegionId, out CastId) && (RegionId != -1 && CastId != -1))
                    {
                        MyStudMang.CreateTansationDb();
                        _userId = MyStudMang.CreateStudent(_studname, _sex, _Dob, _Fathername, _address, _joiningbatchid, _joiningStandard, _Joindate, _StandardId, _classid, _admisionNo, _Bloodgroupid, _nationality, mothertoungid, _mothername, _Fathedu, _mothedu, _fatherOcc, _motherOcc, _Anualincom, _addrspresent, _location, _State, _pin, _resedphon, _officephon, _email, nofbrother, nofsis, firstlng, studcatogory, RegionId, CastId, MyUser.CurrentBatchId, AdmissionType, _TempStudentId, int.Parse(Rdb_NeedBus.SelectedValue), int.Parse(Rdb_NeedHostel.SelectedValue), studentid,_aadharno);
                        //_userId = MyStudMang.CreateStudent(_studname, _sex, _Dob, _Fathername, _address, _joiningbatchid, _joiningStandard, _Joindate, _StandardId, _classid, _admisionNo, _Bloodgroupid, _nationality, mothertoungid, _mothername, _Fathedu, _mothedu, _fatherOcc, _Anualincom, _addrspresent, _location, _State, _pin, _resedphon, _officephon, _email, nofbrother, nofsis, firstlng, studcatogory, RegionId, CastId, MyUser.CurrentBatchId, AdmissionType, _TempStudentId, int.Parse(Rdb_NeedBus.SelectedValue), int.Parse(Rdb_NeedHostel.SelectedValue), studentid);

                        if (_userId != -1)
                        {
                          
                            Hdn_StudentId.Value = _userId.ToString();
                            //Hdn_RollNumber.Value = MyStudMang.ScheduleRollNumber(_classid, MyUser.CurrentBatchId, _userId);
                            if ((Hdn_ststus.Value == "1") && (Hdn_Studid.Value != ""))
                            {
                                //string sql = "";
                                //OdbcDataReader IDreader = null;
                                //sql = "select tbltempstdent.Id from tbltempstdent where tbltempstdent.TempId='" + Hdn_Studid.Value + "'";
                                //IDreader = MyStudMang.m_TransationDb.ExecuteQuery(sql);
                                //if (IDreader.HasRows)
                                //{
                                _continue = MyStudMang.UpdateTempStudentStatus(int.Parse(Hdn_Studid.Value.ToString()));
                                //}


                            }
                            //_continue = UpDateCoustomFields(_userId);

                            //if (_continue && Txt_OfficePh.Text != "")
                            //{
                            //    InsertParentMobileNumberIntoSMSParenstsList();
                            //}
                            //if (_continue && _TempStudentId!="0")
                            //    UpdateJoiningFeeId(_TempStudentId, _userId);

                            Hdn_TempId.Value = "0";
                            if (!MyStudMang.NeedStudentApprovel())
                            {
                                Hdn_RollNumber.Value = MyStudMang.ScheduleRollNumber(_classid, MyUser.CurrentBatchId, _userId);

                                //if ((Hdn_ststus.Value == "1") && (Hdn_Studid.Value != ""))
                                //{
                                //    _continue = MyStudMang.UpdateTempStudentStatus(int.Parse(Hdn_Studid.Value));
                                //}
                                _continue = UpDateCoustomFields(_userId);

                                if (_continue && Txt_OfficePh.Text != "")
                                {
                                    MyStudMang.InsertParentMobileNumberIntoSMSParenstsList(int.Parse(Hdn_StudentId.Value), Txt_OfficePh.Text.Trim(),"");
                                    //InsertParentMobileNumberIntoSMSParenstsList();
                                }

                                if (_continue && Txt_Email.Text != "")
                                {
                                    MyStudMang.InsertParentEmailIdIntoEmailParenstsList(int.Parse(Hdn_StudentId.Value), Txt_Email.Text.Trim());
                                    //InsertParentMobileNumberIntoSMSParenstsList();
                                }
                                if (_continue && _TempStudentId != "0")
                                    MyStudMang.UpdateJoiningFeeId(_TempStudentId, _userId);
                                //UpdateJoiningFeeId(_TempStudentId, _userId);
                                //if (_continue)
                                //{
                                //    MyStudMang.EndSucessTansationDb();

                                //    MyIncedent.CreateApprovedIncedent("Student Created", "Student " + _studname + " is admitted to " + _ClassName + " on " + General.GerFormatedDatVal(DateTime.Now), General.GerFormatedDatVal(DateTime.Now), 1, MyUser.UserId, "student", _userId, 1, 0, MyUser.CurrentBatchId, _classid);
                                //    if ((MyStudMang.IsQuickSchedule()) && (MyStudMang.FeeScheduledForTheClass(int.Parse(Drp_Class.SelectedValue), MyUser.CurrentBatchId)))
                                //    {
                                //        SheduleFeeForTheStudent(_userId, _classid);
                                //    }
                                //}
                                //else
                                //{
                                //    MyStudMang.EndFailTansationDb();
                                //    _message = "Unable to upload student custom data";
                                //    Lbl_msg.Text = _message;
                                //    this.MPE_MessageBox.Show();
                                //} 

                                if (_continue)
                                {
                                    MyStudMang.EndSucessTansationDb();
                                    MyIncedent.CreateApprovedIncedent("Student Created", "Student " + _studname + " is admitted to " + _ClassName + " on " + General.GerFormatedDatVal(DateTime.Now), General.GerFormatedDatVal(DateTime.Now), 1, MyUser.UserId, "student", _userId, 1, 0, MyUser.CurrentBatchId, _classid);
                                    if ((MyStudMang.IsQuickSchedule()) && (MyStudMang.FeeScheduledForTheClass(int.Parse(Drp_Class.SelectedValue), MyUser.CurrentBatchId)))
                                    {
                                        SheduleFeeForTheStudent(_userId, _classid);
                                    }
                                    if (MyStudMang.BookScheduledToClass(int.Parse(Drp_Class.SelectedValue), MyUser.CurrentBatchId))
                                    {
                                        MyStudMang.ScheduleBookToNewStudent(_classid, _userId, MyUser.CurrentBatchId);
                                    }
                                }
                                else
                                {
                                    MyStudMang.EndFailTansationDb();
                                    _message = "Unable to upload student custom data";
                                    Lbl_msg.Text = _message;
                                    this.MPE_MessageBox.Show();
                                }
                            }
                            else
                            {

                                if (_continue)
                                    MyStudMang.EndSucessTansationDb();
                            }

                            Pnl_mainarea.Visible = false;
                            LoadDetailsFordisplay(_userId);
                            Pnl_StudentDtails.Visible = true;
                        }
                        else
                        {
                            MyStudMang.EndFailTansationDb();
                            if (_message == "")
                            {
                                _message = "Error while adding student. Try later";
                            }
                            Lbl_msg.Text = _message;
                            this.MPE_MessageBox.Show();
                        }
                    }
                }
                else
                {
                    Lbl_msg.Text = _message;
                    this.MPE_MessageBox.Show();
                }
               

            }
            catch (Exception exc)
            {
                Lbl_msg.Text = exc.Message;
                this.MPE_MessageBox.Show();
            }
        
        }

      

        private void InsertParentMobileNumberIntoSMSParenstsList()
        {
            string sql = "insert into tblsmsparentlist(Id,PhoneNo,Enabled) values (" + int.Parse(Hdn_StudentId.Value) + "," + Txt_OfficePh.Text.Trim() + "," + "1)";
            MyStudMang.m_TransationDb.ExecuteQuery(sql);
        }

        #region Attendance

        //private void Mark_AbsentFor_PreviousDays(int _ClassId, int _studId)
        //{
          
        //    DataSet _attendancedays = new DataSet();
        //    _attendancedays = FindAttendanceDays(_ClassId);
        //    if (_attendancedays.Tables[0].Rows.Count > 0)
        //    {
        //        string Sql;
        //        foreach (DataRow dr in _attendancedays.Tables[0].Rows)
        //        {
        //           Sql = "insert into tblstudentattendance(StudentId,DayId)VALUES("+ _studId + "," + int.Parse(dr[0].ToString())+")";
        //           MyStudMang.m_MysqlDb.ExecuteQuery(Sql);    
        //        }
        //    }
        //}

        private DataSet FindAttendanceDays(int _ClassId)
        {
            DataSet _attendancedays = new DataSet();
            string Sql = "select distinct tbldate.Id FROM tbldate where tbldate.Status='class' and tbldate.classId=" + _ClassId;
            _attendancedays = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(Sql);
            return _attendancedays;
        }

        #endregion 

        private void LoadDetailsFordisplay(int _studentId)
        {
            Session["StudId"] = _studentId;
            Lbl_name_ds.Text = Txt_Name.Text;
            Lbl_Sex_ds.Text = RadioBtn_Sex.SelectedValue;
            Lbl_Admission_ds.Text = MyStudMang.GetAdmossionNo(_studentId);
            Lbl_DOB_ds.Text=Txt_Dob.Text;
            Lbl_Father_ds.Text = Txt_FGName.Text;
            Lbl_Standard_ds.Text = Drp_Std.SelectedItem.ToString();
            Lbl_Class_ds.Text = Drp_Class.SelectedItem.ToString();
            Lbl_Joining_ds.Text = Drp_JoinBatch.SelectedItem.Text;

        }
    
        public Size NewImageSize(int OriginalHeight, int OriginalWidth, double FormatSize)
        {
            Size NewSize;
            double tempval;

            if (OriginalHeight > FormatSize && OriginalWidth > FormatSize)
            {
                if (OriginalHeight > OriginalWidth)
                    tempval = FormatSize / Convert.ToDouble(OriginalHeight);
                else
                    tempval = FormatSize / Convert.ToDouble(OriginalWidth);

                NewSize = new Size(Convert.ToInt32(tempval * OriginalWidth), Convert.ToInt32(tempval * OriginalHeight));
            }
            else
                NewSize = new Size(OriginalWidth, OriginalHeight); return NewSize;
        }
  
        private bool UpDateCoustomFields(int _userId)
        {
            bool valid = false;
        
            if (CustfieldCount > 0)
            {
                int i;
                string Fields, Values;
                Fields="StudentId";
                Values = "'"+_userId.ToString()+"'";
                for (i = 0; i < CustfieldCount; i++)
                {
                    Fields = Fields + "," + FealdName[i];
                    Values = Values + "," + "'" + dynamicTextBoxes[i].Text + "'";
                }
                valid= MyStudMang.InsertStudentDetails(Fields, Values);
                
            }
            else
            {
                valid = true;
            }
            Session["TempId"] = null;
            return valid;
        }

        private bool SheduleFeeForTheStudent(int _StudentId, int _classid)
        {
            bool _valid = true;
            try
            {
                foreach (GridViewRow gv in Grd_Fees.Rows)
                {
                    CheckBox cb = (CheckBox)gv.FindControl("Chk_Fee");
                    if (cb.Checked)
                    {
                        if (MyFeeMang.CheckForRuleApplicableToClassAndFee1(_classid, int.Parse(gv.Cells[2].Text)))
                        {
                            if (MyFeeMang.CheckRuleIsApplicabletoThisStudent(int.Parse(gv.Cells[2].Text), _classid, double.Parse(gv.Cells[5].Text), _StudentId, MyUser.CurrentBatchId, int.Parse(gv.Cells[1].Text)))
                            {
                                MyFeeMang.m_DbLog.LogToDbNoti(MyUser.UserName, "Scheduled Fee", "Scheduled " + gv.Cells[3].Text + " Fee to Student Named " + Txt_Name.Text.ToUpper(), 1,1);
                            }
                        }
                        else
                        {
                            MyFeeMang.ScheduleStudFee(_StudentId, int.Parse(gv.Cells[1].Text), double.Parse(gv.Cells[5].Text), "Scheduled");
                            MyFeeMang.m_DbLog.LogToDbNoti(MyUser.UserName, "Scheduled Fee", "Scheduled " + gv.Cells[3].Text + " Fee to Student Named " + Txt_Name.Text.ToUpper(), 1, 1);
                        }                        
                    }
                }               
            }
            catch
            {
                _valid = false;
            }
            return _valid;
        }

        private bool GetRearionAndCast(out int ReligionId, out int CastId)
        {
            bool valid = false;
            ReligionId = -1;
            CastId = -1;
            try
            {
                if (Drp_Religion.SelectedValue != "-1")
                {
                    ReligionId = int.Parse(Drp_Religion.SelectedValue);
                   // ReligionId = MyStudMang.GetRelionId(Txt_Religion.Text);
                }
                //else
                //{
                //    ReligionId = int.Parse(Drp_Religion.SelectedValue);

                //}
                
                
                    CastId = int.Parse(Drp_Caste.SelectedValue);

                
                valid = true;
            }
            catch
            {
                valid = false;
            }
            return valid;
        }

        private bool ValidData(out string _message)
        {
            
            DateTime Today = DateTime.Now;
            bool _valid = true;
            _message = "";
            DateTime Dob= new DateTime();
            if (!MyStudMang.AutoAdmissionNoTrue() && !MyStudMang.AvailableAdminNo(Txt_AdminNo.Text.ToString()))
            {
                _message = "Admission No already exists";
                _valid = false;
               
            }
            else if (Drp_Class.SelectedItem == null)
            {
                _message = "No Class is Selected";
                _valid = false;
            }
            else if (lbl_ClassMsg.Text!="")
            {
                _message = "All Seats are filled for the selected class.";
                _valid = false;
            }
            else if (!MyStudMang.IsStudentIdUnique(Txt_StudentId.Text,"0"))
            {
                _message = "Student id exist,Please enter another one";
                _valid = false;
            } 

            else
            {
                DateTime Join_Date = MyUser.GetDaTeFromText(Txt_JoiningDate.Text);
                Dob = MyUser.GetDareFromText(Txt_Dob.Text);
                if(Join_Date > Today)
                {
                    _message = "Date of admission cannot be greater than today";
                    _valid = false;                    
                }
                else if ( (Dob > Today) || (Dob > Join_Date) || (Dob==Join_Date))
                {
                    _message = "Invalid Date of Birth / Date of ADmission";
                    _valid = false; 
                }             
            }

            if (_valid)
            {
                if (Chk_newadminsion.Checked == false)
                {
                    if (int.Parse(Drp_JoinStandard.SelectedValue) > int.Parse(Drp_Std.SelectedValue))
                    {
                        _message = "Joining standard should not be greater than current standard";
                        _valid = false; 
                    }
                }
            }

            return _valid;
        }

        protected void Btn_reload_Click(object sender, EventArgs e)
        {
            
        }

        protected void Img_addnewuser_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("CreateNewStudent.aspx");
        }

        protected void Img_viewuser_Click(object sender, ImageClickEventArgs e)
        {
            string status = MyStudMang.GetStudentSatus(Hdn_StudentId.Value);
            if (status == "1")
            {
                Session["StudType"] = status;
                Response.Redirect("StudentDetails.aspx");

            }
            else
            {
                Lbl_msg.Text = "The student is waiting for approval";
                this.MPE_MessageBox.Show();
            }
        }

        protected void Img_CollectFee_Click(object sender, ImageClickEventArgs e)
        {
            string Status= MyStudMang.GetStudentSatus(Hdn_StudentId.Value);
            if (Status != "1")
            {
                Lbl_msg.Text = "The student is waiting for approval";
                this.MPE_MessageBox.Show();
            }
            else if (MyUser.HaveActionRignt(2) && Status=="1")
            {
                Img_CollectFee.Visible = true;
                Response.Redirect("CollectFeeAccount.aspx?ClassId=" + Drp_Class.SelectedValue + "&RollNumber=" + Hdn_RollNumber.Value + "&StudentId=" + Hdn_StudentId.Value + "");
            }

        }
 
        # region tempstudent
       
        protected void Img_AddTempStd_Click(object sender, ImageClickEventArgs e)
        {
            LoadRegisteredDetails();           
        }

        protected void Lnk_AddTemp_Click(object sender, EventArgs e)
        {
            LoadRegisteredDetails();
        }

        private void LoadRegisteredDetails()
        {
            this.ModalPopupExtender_tempstudents.Show();
            // LoadTempStudents("",0,0);
            AddStandardToPopUpDrp();
            AddClassTopopupDrp();
            LoadTempStudents("", int.Parse(Drp_PopUp_Standardlist.SelectedValue), int.Parse(Drp_PopUp_Class.SelectedValue), 0);
        }

        private void AddClassTopopupDrp()
        {

            Drp_PopUp_Class.Items.Clear();
            if (Drp_PopUp_Standardlist.SelectedValue != "")
            {
                string sql = "SELECT tblclass.Id,tblclass.ClassName FROM tblclass where tblclass.Status AND tblclass.Standard='" + int.Parse(Drp_PopUp_Standardlist.SelectedValue.ToString()) + "' AND tblclass.Id IN (SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + "))";
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    //Drp_PopUp_Class.Items.Add(new ListItem("All", "0"));
                    while (MyReader.Read())
                    {
                        ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_PopUp_Class.Items.Add(li);

                    }
                  
                }
                else
                {
                    ListItem li = new ListItem("No Class Found", "-1");
                    Drp_PopUp_Class.Items.Add(li);
                }
            }

        }

        private void AddStandardToPopUpDrp()
        {
            Drp_PopUp_Standardlist.Items.Clear();
            string sql = "SELECT DISTINCT tblstandard.Id, tblstandard.Name from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                //Drp_PopUp_Standardlist.Items.Add(new ListItem("All","0"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_PopUp_Standardlist.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Standard Found", "-1");
                Drp_PopUp_Standardlist.Items.Add(li);
            }
        }

        private void LoadTempStudents(string _Name, int _StdId, int _ClassId, int _ListType)
        {
            lbl_StudentName1.Visible = false;
            Lbl_TempMessage.Text = "";
            int _TotalSeats = GetTotalSeats(_ClassId);
            //int _ListType = int.Parse(RdBtn_StdListType.SelectedIndex.ToString());
            Grd_TempStudents.Columns[0].Visible = true;
                string sql = "";
                sql = "select tbltempstdent.Id,tbltempstdent.TempId,tbltempstdent.Name,tbltempstdent.Gender,tblclass.ClassName,tbltempstdent.Rank from tbltempstdent inner join tblstandard on tblstandard.Id = tbltempstdent.Standard inner join tblclass on tblclass.Id =tbltempstdent.Class   where JoiningBatch=" + MyUser.CurrentBatchId;
                if (_Name != "")
                {
                    sql = sql + " and tbltempstdent.Name Like'" + _Name + "%'";
                }
                if ((_StdId != 0) && (_StdId!=-1))
                {
                    sql = sql + " and tbltempstdent.Standard=" + _StdId;
                }
                if (_ListType == 0)
                {
                    if ((_ClassId != 0) && (_ClassId != -1))
                    {
                        sql = sql + " and tbltempstdent.Class=" + _ClassId + " and tbltempstdent.AdmissionStatusId=4";
                    }
                }
                else
                {
                    if ((_ClassId != 0) && (_ClassId != -1))
                    {
                        sql = sql + " and tbltempstdent.Class=" + _ClassId + " and tbltempstdent.AdmissionStatusId=6";
                    }
                }
                sql = sql + " and tbltempstdent.`Status`=1";
            MyDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyDataSet != null && MyDataSet.Tables!=null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                Grd_TempStudents.DataSource = MyDataSet;
                Grd_TempStudents.DataBind();
                Grd_TempStudents.Columns[0].Visible = false;
            }
            else
            {
               // lbl_StudentName1.Visible = true;
                Grd_TempStudents.DataSource = null;
                Grd_TempStudents.DataBind();
                Lbl_TempMessage.Text = "No student found";
               
            }
        }

        private int GetTotalSeats(int _ClassId)
        {
            int _TotalSeats = 0;
            string sql1 = "SELECT tblclass.TotalSeats from tblclass where tblclass.Id="+_ClassId;
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql1);
            if (MyReader.HasRows)
            {
                _TotalSeats =int.Parse( MyReader.GetValue(0).ToString());
            }
            return _TotalSeats;
        }

        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            Grd_TempStudents.DataSource = null;
            Grd_TempStudents.DataBind();

            string _StudName = txt_StudentName.Text.Trim();
            int _ListType = int.Parse(RdBtn_StdListType.SelectedIndex.ToString());
            LoadTempStudents(_StudName, int.Parse(Drp_PopUp_Standardlist.SelectedValue), int.Parse(Drp_PopUp_Class.SelectedValue), _ListType);                
            
            ModalPopupExtender_tempstudents.Show();
        }

        protected void Btn_AllList_Click(object sender, EventArgs e)
        {
            txt_StudentName.Text = "";
            
           // LoadTempStudents("", 0, 0);
            int _ListType = int.Parse(RdBtn_StdListType.SelectedIndex.ToString());
            LoadTempStudents("", int.Parse(Drp_PopUp_Standardlist.SelectedValue), int.Parse(Drp_PopUp_Class.SelectedValue),_ListType);
            ModalPopupExtender_tempstudents.Show();
        }

        protected void Grd_TempStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _StudId = Grd_TempStudents.SelectedRow.Cells[0].Text.ToString();
            string TempId = Grd_TempStudents.SelectedRow.Cells[1].Text.ToString();
            string sql = "";
            OdbcDataReader XmlReader = null;
            sql = "select tbl_xmlstring.Id, tbl_xmlstring.XMLString from tbl_xmlstring  where tbl_xmlstring.TempId='" + TempId + "'";
            XmlReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (XmlReader.HasRows)
            {
                DataSet StudentRegistrationDS = new DataSet();
                XmlDocument doc = new XmlDocument();
                string StrXml = XmlReader.GetValue(1).ToString();
                DataSet ReaderDs = new DataSet();
                doc.LoadXml(StrXml);
                XmlNodeReader Nodereader = new XmlNodeReader(doc);
                Nodereader.MoveToContent();
                while (Nodereader.Read())
                {
                    ReaderDs.ReadXml(Nodereader);

                }
                LoadTempStudentDetails(ReaderDs, TempId);

            }
            else
            {
                LoadAllDetails(_StudId);
            }
            LoadStudentLimitWarningMessage();
        }

        private void LoadAllDetails(string _StudId)
        {
            OdbcDataReader MyTempReader = null;
            string sql = "select Name,Fathername,Gender,Standard,Class,Address,TempId,PhoneNumber,Email,MotherName,Location,Pin,State,Nationality,BloodGroup,MotherTongue,FatherEduQualification,MotherEduQualification,FatherOccupation,AnnualIncome,Date_Format(DOB,'%d/%m/%Y') as DOB from tbltempstdent where Id=" + _StudId + " and `Status`=1";
            MyTempReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyTempReader.HasRows)
            {
                Txt_Name.Text = MyTempReader.GetValue(0).ToString();
                Txt_FGName.Text = MyTempReader.GetValue(1).ToString();
                RadioBtn_Sex.SelectedValue = MyTempReader.GetValue(2).ToString();
                Drp_Std.SelectedValue = MyTempReader.GetValue(3).ToString();
                AddClassToDrpList(0);
                Drp_Class.SelectedValue = MyTempReader.GetValue(4).ToString();
                Txt_Address.Text = MyTempReader.GetValue(5).ToString();
                Hdn_TempId.Value = MyTempReader.GetValue(6).ToString();
                if (MyTempReader.GetValue(7).ToString() != "0")
                    Txt_OfficePh.Text = MyTempReader.GetValue(7).ToString();

                Txt_Email.Text = MyTempReader["Email"].ToString();
                Txt_MotherName.Text = MyTempReader["MotherName"].ToString();
                Txt_Location.Text = MyTempReader["Location"].ToString();
                Txt_pin.Text = MyTempReader["Pin"].ToString();
                Txt_State.Text = MyTempReader["State"].ToString();
                Txt_Nationality.Text = MyTempReader["Nationality"].ToString();

                Drp_BloodGrp.SelectedValue = MyTempReader["BloodGroup"].ToString();
                Drp_MotherTongue.SelectedValue = MyTempReader["MotherTongue"].ToString();
                Txt_FatherEduQuali.Text = MyTempReader["FatherEduQualification"].ToString();
                Txt_MotherEduQuali.Text = MyTempReader["MotherEduQualification"].ToString();
                Txt_FatherOccupation.Text = MyTempReader["FatherOccupation"].ToString();
                Txt_AnualIncome.Text = MyTempReader["AnnualIncome"].ToString();
                Txt_Dob.Text = MyTempReader["DOB"].ToString();
                Txt_JoiningDate.Text = MyUser.GerFormatedDatVal(System.DateTime.Now.Date);
                
                MyTempReader.Close();
                Hdn_ststus.Value = "1";
                Hdn_Studid.Value = _StudId.ToString();
                //LoadCoustomFields();
                FreezeFields();
                LoadFeeSchedule();
            }
        }


        private void LoadCoustomFields()
        {
            //int CustfieldCount = MyStudMang.CoustomFieldCount;

            //dynamicTextBoxes = new TextBox[CustfieldCount];
            //Mandatoryflag = new int[CustfieldCount];
            //FealdName = new string[CustfieldCount];
           
            //if (CustfieldCount == 0)
            //{
            //    WizardCustom.Visible = false;
            //}
            //else
            //{

            //    DataSet _CustomFields = MyStudMang.GetCuestomFields();
            //    if (_CustomFields != null && _CustomFields.Tables != null && _CustomFields.Tables[0].Rows.Count > 0)
            //    {

            //        int i = 0;
            //        Table tbl = new Table();

            //        myPlaceHolder.Controls.Add(tbl);
            //        tbl.CssClass = "tablelist";

            //        foreach (DataRow dr_fieldData in _CustomFields.Tables[0].Rows)
            //        {

            //            TableRow tr = new TableRow();
            //            TableCell tc1 = new TableCell();
            //            TableCell tc2 = new TableCell();

            //            tc1.Text = dr_fieldData[1].ToString() + ":";
            //            tc1.CssClass = "leftside";

            //            //Label Lblcostom = new Label();
            //            //Lblcostom.Text = MyStudMang.GetCustomFieldRegStdnt(dr_fieldData[0].ToString(), Session["TempId"].ToString());
            //            //Lblcostom.ForeColor = System.Drawing.Color.Black;
            //            //Lblcostom.Font.Bold = true;
            //            //Lblcostom.ID = "myLbl" + i.ToString();
            //            //tc2.Controls.Add(Lblcostom);
            //            //tc2.CssClass = "rightside";




            //            TextBox textBox = new TextBox();
            //            textBox.MaxLength = int.Parse(dr_fieldData[3].ToString());
            //            textBox.ID = "myTextBox" + i.ToString();
            //            textBox.Text = MyStudMang.GetCustomFieldRegStdnt(dr_fieldData[0].ToString(), Session["TempId"].ToString());
            //            tc2.Controls.Add(textBox);
            //            Mandatoryflag[i] = int.Parse(dr_fieldData[4].ToString());
            //            FealdName[i] = dr_fieldData[0].ToString();
            //            dynamicTextBoxes[i] = textBox;





            //            tr.Cells.Add(tc1);
            //            tr.Cells.Add(tc2);

            //            tbl.Rows.Add(tr);
            //            i++;
            //        }
            //    }

            //}





            CustfieldCount = MyStudMang.CoustomFieldCount;
            if (CustfieldCount == 0)
            {
                Label lbicusnote = new Label();
                lbicusnote.ID = "lbicusnote";
                lbicusnote.Text = "No Coustom Fields Present.";
                myPlaceHolder.Controls.Add(lbicusnote);
                Wzd_StudCreation.WizardSteps.Remove(Wzd_StudCreation.WizardSteps[2]);
            }
            else
            {

                dynamicTextBoxes = new TextBox[CustfieldCount];
                Mandatoryflag = new int[CustfieldCount];
                FealdName = new string[CustfieldCount];
                DataSet _CustomFields = MyStudMang.GetCuestomFields();
                if (_CustomFields != null && _CustomFields.Tables != null && _CustomFields.Tables[0].Rows.Count > 0)
                {

                    int i = 0;
                    Table tbl = new Table();
                    tbl.Width = 500;
                    myPlaceHolder.Controls.Add(tbl);


                    foreach (DataRow dr_fieldData in _CustomFields.Tables[0].Rows)
                    {

                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();
                        TableCell tc2 = new TableCell();
                        TableCell tc3 = new TableCell();
                        if (dr_fieldData[4].ToString() == "1")
                        {
                            tc1.Text = dr_fieldData[1].ToString() + "<span class=\"redcol\">*</span>";
                            RequiredFieldValidator ReqfldvalExt = new RequiredFieldValidator();
                            ReqfldvalExt.ID = "ReqfldvalExt" + i.ToString();
                            ReqfldvalExt.ControlToValidate = "myTextBox" + i.ToString();
                            ReqfldvalExt.ErrorMessage = "You Must enter " + dr_fieldData[1].ToString();
                            tc3.Controls.Add(ReqfldvalExt);
                        }
                        else
                        {
                            tc1.Text = dr_fieldData[1].ToString();
                        }
                        TextBox textBox = new TextBox(); 
                        textBox.MaxLength = int.Parse(dr_fieldData[3].ToString());
                        textBox.ID = "myTextBox" + i.ToString();
                        textBox.Text = MyStudMang.GetCustomFieldRegStdnt(dr_fieldData[0].ToString(), Session["TempId"].ToString());
                        tc2.Controls.Add(textBox);
                        Mandatoryflag[i] = int.Parse(dr_fieldData[4].ToString());
                        FealdName[i] = dr_fieldData[0].ToString();
                        dynamicTextBoxes[i] = textBox;

                        FilteredTextBoxExtender FiltTxtbxExt = new FilteredTextBoxExtender();
                        FiltTxtbxExt.ID = "FiltTxtbxExt" + i.ToString();
                        if (dr_fieldData[2].ToString() == "1")
                        {

                            FiltTxtbxExt.FilterType = FilterTypes.Numbers;

                        }
                        else if (dr_fieldData[2].ToString() == "2")
                        {

                            FiltTxtbxExt.FilterType = FilterTypes.Custom;
                            FiltTxtbxExt.FilterMode = FilterModes.InvalidChars;
                            FiltTxtbxExt.InvalidChars = "'/\\";
                        }
                        else
                        {
                            FiltTxtbxExt.ValidChars = ".";
                            FiltTxtbxExt.FilterType = FilterTypes.Numbers | FilterTypes.Custom;

                        }
                        FiltTxtbxExt.TargetControlID = "myTextBox" + i.ToString();
                        myPlaceHolder.Controls.Add(FiltTxtbxExt);



                        tr.Cells.Add(tc1);
                        tr.Cells.Add(tc2);
                        tr.Cells.Add(tc3);
                        tbl.Rows.Add(tr);
                        i++;
                    }
                }

            }


        }





        private void FreezeFields()
        {
            Txt_Name.Enabled = false;
            RadioBtn_Sex.Enabled = false;
            Drp_Std.Enabled = false;
          
        }

        protected void Drp_PopUpStd_SelectedIndex(object sender, EventArgs e)
        {
            AddClassTopopupDrp();
            int _ListType = int.Parse(RdBtn_StdListType.SelectedIndex.ToString());
            LoadTempStudents("", int.Parse(Drp_PopUp_Standardlist.SelectedValue), int.Parse(Drp_PopUp_Class.SelectedValue),_ListType);
            this.ModalPopupExtender_tempstudents.Show();
        }

         protected void Drp_PopUp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _ListType = int.Parse(RdBtn_StdListType.SelectedIndex.ToString());
            LoadTempStudents("", int.Parse(Drp_PopUp_Standardlist.SelectedValue), int.Parse(Drp_PopUp_Class.SelectedValue),_ListType);
            this.ModalPopupExtender_tempstudents.Show();
        }

         protected void RdBtn_StdListType_SelectedIndexChanged(object sender, EventArgs e)
         {
             int _ListType = int.Parse(RdBtn_StdListType.SelectedIndex.ToString());
             LoadTempStudents("", int.Parse(Drp_PopUp_Standardlist.SelectedValue), int.Parse(Drp_PopUp_Class.SelectedValue),_ListType);
             this.ModalPopupExtender_tempstudents.Show();
         }

        # endregion


        
    }
}
