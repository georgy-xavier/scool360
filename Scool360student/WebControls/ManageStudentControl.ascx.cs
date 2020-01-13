using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using AjaxControlToolkit;
using System.Data;
using System.Drawing;
using System.IO;
using WinEr;
using WinBase;
using System.Configuration;

namespace Scool360student.WebControls
{
    public partial class ManageStudentControl : System.Web.UI.UserControl
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private TextBox[] dynamicTextBoxes;
        public event EventHandler EVNTSave;
        private int[] Mandatoryflag;
        private string[] FealdName;
        private SchoolClass objSchool = null;
        public string STUDENTID
        {
            get
            {
                return Hdn_StudentID.Value;
            }
            set
            {
                Hdn_StudentID.Value = value;
            }

        }
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            if (WinerUtlity.NeedCentrelDB())
            {
                if (Session[WinerConstants.SessionSchool] == null)
                {
                    Response.Redirect("Logout.aspx");
                }
                objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
           
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(4))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                LoadCoustomFields();
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

                   

                    LoadDetails();

                    LoadSiblingDetails();
                    LoadStudentIdMandatory();
                   // LoadDocType();
                    
                    //some initlization

                }
                Lbl_Err.Text = "";
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
                Hdn_StudMandatory.Value = value.ToString();
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
        //private void LoadDocType()
        //{
        //    MysqlClass _MysqlDb = new MysqlClass(ConfigurationSettings.AppSettings["DMSConnectionInfo"]);
        //    using (OdbcConnection _MyODBCConn = new OdbcConnection(ConfigurationSettings.AppSettings["DMSConnectionInfo"]))
        //    {
                
        //        _MyODBCConn.Open();
        //        {
        //            int SchoolId = WinerUtlity.GetSchoolId(objSchool);
        //            Drp_doctype.Items.Clear();
        //            string sql = "SELECT Id,DocumentType FROM uploaddoc where SchoolId=" + SchoolId + "";

        //            MyReader = _MysqlDb.ExecuteQuery(sql);
        //            if (MyReader.HasRows)
        //            {
        //                while (MyReader.Read())
        //                {
        //                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
        //                    Drp_doctype.Items.Add(li);


        //                }
        //            }
        //        }
        //    }


        //}
        private void LoadSiblingDetails()
        {
            string sql = "";
            DataSet SiblingsDs = new DataSet();
            OdbcDataReader Siblingsreder = null;
            sql = "select Id from tbl_siblingsmap where tbl_siblingsmap.StudId=" + int.Parse(Hdn_StudentID.Value) + "";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                sql = "select tbl_siblingsmap.StudId from tbl_siblingsmap where tbl_siblingsmap.Id=" + int.Parse(MyReader.GetValue(0).ToString()) + " and StudId<>" + int.Parse(Hdn_StudentID.Value) + "";
                Siblingsreder = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                if (Siblingsreder.HasRows)
                {
                    string StudId = "";
                    int count = 0;
                    while (Siblingsreder.Read())
                    {
                        if (count == 0)
                        {
                            StudId = Siblingsreder.GetValue(0).ToString();
                            count = 1;
                        }
                        else
                        {
                            StudId = StudId + "," + Siblingsreder.GetValue(0).ToString();

                        }


                    }
                    string _sql = "select Id,StudentName,GardianName from tblstudent where tblstudent.Id in(" + StudId + ")";
                    SiblingsDs = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
                    if (SiblingsDs != null && SiblingsDs.Tables[0].Rows.Count > 0)
                    {
                        Pnl_SibDisplay.Visible = true;
                        GrdSiblings.Columns[0].Visible = true;
                        GrdSiblings.DataSource = SiblingsDs;
                        GrdSiblings.DataBind();
                        GrdSiblings.Columns[0].Visible = false;
                        Lbl_Sib.Text = "Siblings:";
                    }
                    else
                    {
                        GrdSiblings.DataSource = null;
                        GrdSiblings.DataBind();
                        Pnl_SibDisplay.Visible = false;
                        Lbl_Sib.Text = "";

                    }

                }

            }
            else
            {
                GrdSiblings.DataSource = null;
                GrdSiblings.DataBind();
                Pnl_SibDisplay.Visible = false;
                Lbl_Sib.Text = "";
            }

        }

        private void LoadClassDivisionToDrp()
        {
            int StandardId = MyStudMang.GetStandard(Hdn_StudentID.Value);
            Drp_Class.Items.Clear();
            string sql = "SELECT Id,ClassName FROM tblclass where Standard=" + StandardId + " order by ClassName asc";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Class.Items.Add(li);


                }
            }
        }

        private void LoadCoustomFields()
        {
            int CustfieldCount = MyStudMang.CoustomFieldCount;
            //CustfieldCount = 0;
            if (CustfieldCount == 0)
            {

                Pnl_custumarea.Visible = false;
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
                    myPlaceHolder.Controls.Add(tbl);
                    tbl.CssClass = "tablelist";

                    foreach (DataRow dr_fieldData in _CustomFields.Tables[0].Rows)
                    {

                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();
                        TableCell tc2 = new TableCell();
                        TableCell tc3 = new TableCell();
                        tc1.CssClass = "leftside";
                        if (dr_fieldData[4].ToString() == "1")
                        {

                            tc1.Text = dr_fieldData[1].ToString() + "<span class=\"redcol\">*</span>";
                            RequiredFieldValidator ReqfldvalExt = new RequiredFieldValidator();
                            ReqfldvalExt.ID = "ReqfldvalExt" + i.ToString();
                            ReqfldvalExt.ControlToValidate = "myTextBox" + i.ToString();
                            ReqfldvalExt.ValidationGroup = "SaveOtherData";
                            ReqfldvalExt.ErrorMessage = "You Must enter " + dr_fieldData[1].ToString();
                            tc3.Controls.Add(ReqfldvalExt);
                        }
                        else
                        {
                            tc1.Text = dr_fieldData[1].ToString();
                        }
                        TextBox textBox = new TextBox();
                        textBox.Text = MyStudMang.GetCuestomField(dr_fieldData[0].ToString(), Hdn_StudentID.Value);
                        textBox.MaxLength = int.Parse(dr_fieldData[3].ToString());
                        textBox.ID = "myTextBox" + i.ToString();
                        tc2.Controls.Add(textBox);
                        tc2.CssClass = "rightside";
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

        private void LoadDetails()
        {
            LoadClassDivisionToDrp();
            AddReligionToDrpList();
            AddBloodGrpToDrpList();
            AddMotherTongueToDrpList();
            AddJoiningBatchToDrpList();
            AddJoiningStandardToDrpList();
            AddFirstLanguage();
            AddStudentTypeToDrpList();
            LoadStudentData();
            AddStdntClsDrpList();
        }

       

        private void LoadStudentData()
        {
            int AdmissionType = 1;
            string Sql = "select tblstudent.Nationality , tblstudent.MothersName, tblstudent.FatherEduQuali, tblstudent.MotherEduQuali, tblstudent.FatherOccupation,tblstudent.MotherOccupation, tblstudent.AnnualIncome, tblstudent.Addresspresent, tblstudent.Location, tblstudent.State, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo, tblstudent.Email, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters,tblstudent.BloodGroup,tblstudent.MotherTongue,tblstudent.1stLanguage,tblstudent.StudTypeId,tblstudent.StudentName,tblstudent.Sex,tblstudent.DOB,tblstudent.GardianName,tblstudent.Address,tblstudent.JoinBatch,tblstudent.DateofJoining,tblstudent.Religion,tblstudent.Cast,tblstudent.AdmissionTypeId,tblstudent.AdmitionNo,tblstudentclassmap.ClassId, tblstudent.UseBus, tblstudent.UseHostel, tblstudent.JoinStandard,tblstudent.StudentId, tblstudent.AadharNumber from  tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudent.Id where tblstudent.Id=" + int.Parse(Hdn_StudentID.Value) + " and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId;
            // Up to Nationality Field in tblstudent
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                Rdb_NeedBus.SelectedValue = MyReader.GetValue(32).ToString();
                Rdb_NeedHostel.SelectedValue = MyReader.GetValue(33).ToString();
                Drp_JoinStandard.SelectedValue = MyReader.GetValue(34).ToString();

                Txt_Nationality.Text = MyReader.GetValue(0).ToString();
                Txt_aadharno.Text = MyReader.GetValue(36).ToString();
                Txt_MotherName.Text = MyReader.GetValue(1).ToString();

                Txt_FatherEduQuali.Text = MyReader.GetValue(2).ToString();
                Txt_MotherEduQuali.Text = MyReader.GetValue(3).ToString();
                Txt_FatherOccupation.Text = MyReader.GetValue(4).ToString();
                Txt_MotherOccupation.Text = MyReader.GetValue(5).ToString();
                if (MyReader.GetValue(5).ToString() != "0")
                    Txt_AnualIncome.Text = MyReader.GetValue(6).ToString();
                Txt_Address_Present.Text = MyReader.GetValue(7).ToString();
                Txt_Location.Text = MyReader.GetValue(8).ToString();
                Txt_State.Text = MyReader.GetValue(9).ToString();
                if (MyReader.GetValue(9).ToString() != "0")
                    Txt_pin.Text = MyReader.GetValue(10).ToString();
                if (MyReader.GetValue(10).ToString() != "0")
                    Txt_ResidencePh.Text = MyReader.GetValue(11).ToString();
                if (MyReader.GetValue(11).ToString() != "0")
                    Txt_OfficePh.Text = MyReader.GetValue(12).ToString();
                Txt_Email.Text = MyReader.GetValue(13).ToString();
                if (MyReader.GetValue(13).ToString() != "0")
                    Txt_NoBro.Text = MyReader.GetValue(14).ToString();
                if (MyReader.GetValue(14).ToString() != "0")
                    Txt_NoSys.Text = MyReader.GetValue(15).ToString();
                Drp_BloodGrp.SelectedValue = MyReader.GetValue(16).ToString();
                Drp_MotherTongue.SelectedValue = MyReader.GetValue(17).ToString();
                Drp_FirstLanguage.SelectedValue = MyReader.GetValue(18).ToString();
                Drp_StudentType.SelectedValue = MyReader.GetValue(19).ToString();
                Txt_Name.Text = MyReader.GetValue(20).ToString();
                RadioBtn_Sex.SelectedValue = MyReader.GetValue(21).ToString();
                DateTime Dob = DateTime.Parse(MyReader.GetValue(22).ToString());
                //DateTime Dob = MyUser.GetDareFromText(MyReader.GetValue(21).ToString());

                //Txt_Dob.Text = Dob.Date.ToString("dd/MM/yyyy");
                Txt_Dob.Text = Dob.Date.Date.Day + "/" + Dob.Date.Month + "/" + Dob.Date.Year;

                Txt_FGName.Text = MyReader.GetValue(23).ToString();
                Txt_Address.Text = MyReader.GetValue(24).ToString();
                Drp_JoinBatch.SelectedValue = MyReader.GetValue(25).ToString();
                DateTime Doj = DateTime.Parse(MyReader.GetValue(26).ToString());
                //DateTime Doj = MyUser.GetDareFromText(MyReader.GetValue(25).ToString());

                //Txt_JoiningDate.Text = Doj.Date.ToString("dd/MM/yyyy");
                Txt_JoiningDate.Text = Doj.Date.Day + "/" + Doj.Date.Month + "/" + Doj.Date.Year;
                Drp_Religion.SelectedValue = MyReader.GetValue(27).ToString();
                string _castid = MyReader.GetValue(28).ToString();
                bool valid = int.TryParse(MyReader.GetValue(29).ToString(), out AdmissionType);
                if (AdmissionType == 1)
                {
                    Chk_NewAdmin.Checked = false;
                }
                else
                {
                    Chk_NewAdmin.Checked = true;
                }
                Txt_AdmissionNo.Text = MyReader.GetValue(30).ToString();
                Hdn_AdmissionNo.Value = MyReader.GetValue(30).ToString();
                Hdn_ClassId.Value = MyReader.GetValue(31).ToString();
                Drp_Class.SelectedValue = MyReader.GetValue(31).ToString();
                Txt_StudentId.Text = MyReader.GetValue(35).ToString();
               
                
                MyReader.Close();
                //Reading SecondaryNo:- Edited by mobin
                string SqlQuery = "SELECT tblsmsparentlist.SecondaryNo from tblsmsparentlist where tblsmsparentlist.id=" + int.Parse(Hdn_StudentID.Value);
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(SqlQuery);
                if (MyReader.HasRows)
                    if (MyReader.GetValue(0).ToString() != "0")
                        Txt_SecondaryPh.Text = MyReader.GetValue(0).ToString();

                LoadCast();
                Drp_Caste.SelectedValue = _castid;

                MyReader.Close();
            }
        }

        private void LoadCast()
        {
            Drp_Caste.Items.Clear();
            string sql = "select tblcast.Id, tblcast.castname from tblcast where tblcast.castname <>'Other' ";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {


                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Caste.Items.Add(li);

                }


            }

            //Drp_Caste.Items.Add(new ListItem("OTHERS", "-1"));

            //if (Drp_Caste.SelectedItem.Text == "OTHERS")
            //{
            //    Lbl_newCaste.Visible = true;
            //    Txt_NewCaste.Visible = true;

            //}
            //else
            //{
            //    Lbl_newCaste.Visible = false;
            //    Txt_NewCaste.Visible = false;
            //}
        }

        private void AddStudentTypeToDrpList()
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

            }
        }

        private void AddReligionToDrpList()
        {
            Drp_Religion.Items.Clear();
            string sql = "SELECT Id,Religion FROM tblreligion where Religion <>'Other' ORDER BY Religion ";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Religion.Items.Add(li);

                }


            }

        }

        private void AddFirstLanguage()
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
            }
        }

        private void AddJoiningStandardToDrpList()
        {
            Drp_JoinStandard.Items.Clear();
            string sql = "SELECT DISTINCT tblstandard.Id, tblstandard.Name from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_JoinStandard.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Standard Found", "-1");
                Drp_JoinStandard.Items.Add(li);
            }
        }

        private void AddJoiningBatchToDrpList()
        {
            Drp_JoinBatch.Items.Clear();
            string sql = "SELECT Id,BatchName FROM tblbatch WHERE Created=1";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_JoinBatch.Items.Add(li);
                }
            }
        }

        private void AddMotherTongueToDrpList()
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
            }
        }
        private void AddStdntClsDrpList()
        {
            Drp_selectClass.Items.Clear();
            string sql = "";
            sql += " SELECT tblclass.ClassName AS Class,tblview_studentclassmap.ClassId,tblbatch.BatchName";
            sql += " FROM  tblstudent";
            sql += " INNER JOIN tblview_studentclassmap ON tblview_studentclassmap.StudentId = tblstudent.Id INNER JOIN tblbatch ON  tblbatch.Id =  tblview_studentclassmap.BatchId INNER JOIN tblclass on tblclass.Id =  tblview_studentclassmap.ClassId INNER JOIN tblstandard ON tblstandard.Id = tblview_studentclassmap.Standard ";
            sql += " WHERE tblstudent.Id =" + int.Parse(Hdn_StudentID.Value) + "  ORDER BY  tblview_studentclassmap.BatchId DESC ";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_selectClass.Items.Add(li);
                }
            }
        }
        private void AddBloodGrpToDrpList()
        {
            Drp_BloodGrp.Items.Clear();

            string sql = "SELECT Id,GroupName FROM tblbloodgrp";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_BloodGrp.Items.Add(li);
                }
            }
        }

        protected void Btn_UpdateGeneraldetails_Click(object sender, EventArgs e)
        {
            string _message, _message1 = "";
            try
            {
                if (ValidData(out _message1))
                {
                    int ClassId = 0;
                    string _studname = Txt_Name.Text.ToUpper();
                    string _sex = RadioBtn_Sex.SelectedValue.ToString();
                    //            DateTime _Dob = DateTime.Parse(Txt_Dob.Text);
                    DateTime _Dob = MyUser.GetDareFromText(Txt_Dob.Text);

                    string _Fathername = Txt_FGName.Text.ToUpper();
                    string _address = Txt_Address.Text;
                    int _joiningbatchid = int.Parse(Drp_JoinBatch.SelectedValue.ToString());
                    string _joiningStandard = Drp_JoinStandard.SelectedValue;
                    //            DateTime _Joindate = DateTime.Parse(Txt_JoiningDate.Text);
                    DateTime _Joindate = MyUser.GetDareFromText(Txt_JoiningDate.Text);

                    string studentId = Txt_StudentId.Text;
                    //string aadharno = Txt_aadharno.Text;
                    int ReligionId = int.Parse(Drp_Religion.SelectedValue);
                    int CastId = int.Parse(Drp_Caste.SelectedValue);
                    //if (Drp_Caste.SelectedItem.Text.ToString().ToUpper() == "OTHERS")
                       //CastId = MyStudMang.IncertNewCaste(ReligionId, Txt_NewCaste.Text.Trim());
                    bool valid = int.TryParse(Drp_Class.SelectedValue, out ClassId);
                    if (_Joindate > DateTime.Now)
                    {
                        _message = "Date of admission should not be greater than current date";
                    }
                    else
                    {
                        MyStudMang.UpdateStudentGeneralDetails(_studname, _sex, _Dob, _Fathername, _address, _joiningbatchid, _Joindate, ReligionId, CastId, int.Parse(Hdn_StudentID.Value), Txt_AdmissionNo.Text.Trim(), _joiningStandard, studentId);
                        MyStudMang.UpdateStudentDetailsInTempStudentAndFeeTables(int.Parse(Hdn_StudentID.Value), _studname, _address, _Fathername);
                        if (Drp_Class.SelectedValue != Hdn_ClassId.Value)
                        {
                            if (MyStudMang.UpDateStudentDivision(Hdn_StudentID.Value, Drp_Class.SelectedValue, MyUser.CurrentBatchId))
                            {

                                MyStudMang.ScheduleRollNumber(int.Parse(Drp_Class.SelectedValue.ToString()), MyUser.CurrentBatchId, int.Parse(Hdn_ClassId.Value));//Roll Number for new Class
                               // MyStudMang.ScheduleRollNumber(int.Parse(Hdn_ClassId.Value), MyUser.CurrentBatchId);//Roll number for old class
                                MyStudMang.MoveStudentWiseFeescheduleToNewClass(Drp_Class.SelectedValue, Hdn_ClassId.Value, Hdn_StudentID.Value, MyUser.CurrentBatchId);
                                MyStudMang.DeleteOldClassWiseFees(Drp_Class.SelectedValue, Hdn_ClassId.Value, MyUser.CurrentBatchId, Hdn_StudentID.Value);
                                ScheduleNewClassWiseFees(Drp_Class.SelectedValue, MyUser.CurrentBatchId, Hdn_StudentID.Value);
                                MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Division Changes", "The student " + Txt_Name.Text + "  is moved from " + MyStudMang.GetClassName(int.Parse(Hdn_ClassId.Value)) + " to " + MyStudMang.GetClassName(int.Parse(Drp_Class.SelectedValue)) + "", 1,1);
                            }
                        }
                        if (EVNTSave != null)
                        {
                            EVNTSave(this, e);
                        }
                        _message = "Details are updated";
                        //LoadpupilTopData();
                        MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Updated Student General details", "General details Of Student " + MyUser.m_DbLog.GetStudentName(int.Parse(Hdn_StudentID.Value)) + " is Updated", 1, 1);
                    }

                }
                else
                {
                    _message = _message1;
                }

            }
            catch
            {
                _message = "Unable to Update General Details";
            }

            Lbl_msg.Text = _message;
            MPE_MessageBox.Show();
        }

        private void ScheduleNewClassWiseFees(string _NewClass, int _BatchId, string _StudedntId)
        {
            DataSet MyFees = new DataSet();
            int _NextBatchId = _BatchId + 1;
            string sql = "select tblfeeschedule.Id,tblfeeschedule.FeeId, tblfeeaccount.AccountName, tblfeeschedule.Amount, date_format(tblfeeschedule.Duedate, '%d-%m-%Y') AS 'Duedate' , date_format(tblfeeschedule.LastDate, '%d-%m-%Y') AS 'LastDate' from tblfeeschedule inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblfeeasso on tblfeeasso.Id = tblfeeaccount.AssociatedId  where (tblfeeschedule.BatchId=" + _BatchId + " || tblfeeschedule.BatchId=" + _NextBatchId + ") and tblfeeschedule.ClassId=" + _NewClass + " and tblfeeasso.Name='Class'";
            MyFees = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyFees != null && MyFees.Tables != null && MyFees.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Dr_Fees in MyFees.Tables[0].Rows)
                {

                    if (MyStudMang.CheckForRuleApplicableToClassAndFee1(int.Parse(_NewClass), int.Parse(Dr_Fees[1].ToString())))
                    {

                        if (MyStudMang.CheckRuleIsApplicabletoThisStudent(int.Parse(Dr_Fees[1].ToString()), int.Parse(_NewClass), double.Parse(Dr_Fees[3].ToString()), int.Parse(_StudedntId), MyUser.CurrentBatchId, int.Parse(Dr_Fees[0].ToString())))
                        {
                            MyStudMang.m_DbLog.LogToDbNoti(MyUser.UserName, "Scheduled Fee", "Scheduled " + Dr_Fees[2].ToString() + " Fee to Student Named " + Txt_Name.Text.ToUpper(), 1,1);
                        }

                    }
                    else
                    {
                        MyStudMang.ScheduleStudFee(int.Parse(_StudedntId), int.Parse(Dr_Fees[0].ToString()), double.Parse(Dr_Fees[3].ToString()), "Scheduled");
                        MyStudMang.m_DbLog.LogToDbNoti(MyUser.UserName, "Scheduled Fee", "Scheduled " + Dr_Fees[2].ToString() + " Fee to Student Named " + Txt_Name.Text.ToUpper(), 1, 1);
                    }
                }
            }
        }

        //private bool SheduleFeeForTheStudent(int _StudentId, int _classid)
        //{
        //    bool _valid = true;
        //    try
        //    {

        //        foreach (GridViewRow gv in Grd_Fees.Rows)
        //        {
        //            CheckBox cb = (CheckBox)gv.FindControl("Chk_Fee");
        //            if (cb.Checked)
        //            {
        //                if (MyFeeMang.CheckForRuleApplicableToClassAndFee1(_classid, int.Parse(gv.Cells[2].Text)))
        //                {

        //                    if (MyFeeMang.CheckRuleIsApplicabletoThisStudent(int.Parse(gv.Cells[2].Text), _classid, double.Parse(gv.Cells[4].Text), _StudentId, MyUser.CurrentBatchId, int.Parse(gv.Cells[1].Text)))
        //                    {
        //                        MyFeeMang.m_DbLog.LogToDb(MyUser.UserName, "Scheduled Fee", "Scheduled " + gv.Cells[3].Text + " Fee to Student Named " + Txt_Name.Text.ToUpper(), 1);
        //                    }

        //                }
        //                else
        //                {
        //                    MyFeeMang.ScheduleStudFee(_StudentId, int.Parse(gv.Cells[1].Text), double.Parse(gv.Cells[4].Text), "Scheduled");
        //                    MyFeeMang.m_DbLog.LogToDb(MyUser.UserName, "Scheduled Fee", "Scheduled " + gv.Cells[3].Text + " Fee to Student Named " + Txt_Name.Text.ToUpper(), 1);
        //                }

        //            }
        //        }

        //    }
        //    catch
        //    {
        //        _valid = false;
        //    }
        //    return _valid;
        //}

        private bool ValidData(out string _message)
        {
            bool valid = true;
            _message = "";
            if (Txt_AdmissionNo.Text != Hdn_AdmissionNo.Value)
            {
                if (!MyStudMang.AvailableAdminNo(Txt_AdmissionNo.Text))
                {
                    valid = false;
                    _message = "Admission number already exists";
                }
            }
            else if (Drp_Class.SelectedValue != Hdn_ClassId.Value)
            {
                //if (MyStudMang.AnyFeePaid(Hdn_StudentID.Value, MyUser.CurrentBatchId, Hdn_ClassId.Value))
                //{
                //    valid = false;
                //    _message = "Cannot change class.Student has paid some fees";
                //}
                ////if ((MyUser.HaveModule(21))&&(MyStudMang.AttendanceMarked(Hdn_StudentID.Value, MyUser.CurrentBatchId, Hdn_ClassId.Value)))
                ////{
                ////    valid = false;
                ////    _message = "Cannot change class.Attendance is marked for the class";
                ////}
                //if ((MyUser.HaveModule(3)) && (MyStudMang.ExamConducted(Hdn_StudentID.Value, MyUser.CurrentBatchId, Hdn_ClassId.Value)))
                //{
                //    valid = false;
                //    _message = "Cannot change class.Exam is conducted";
                //}
                valid = false;
                _message = "Use change class for changing student class";
            }
            else if (MyUser.GetDareFromText(Txt_Dob.Text) >= DateTime.Now.Date)
            {
                valid = false;
                _message = "Enter valid Date of Birth";
            }
            else if (!MyStudMang.IsStudentIdUnique(Txt_StudentId.Text, Hdn_StudentID.Value))
            {
                valid = false;
                _message = "Student id exist,Please enter another one";
            }

            if (valid)
            {
                if (int.Parse(Drp_JoinStandard.SelectedValue) > MyStudMang.GetStandard(Hdn_StudentID.Value))
                {
                    _message = "Joining standard should not be greater than current standard";
                    valid = false;
                }

            }

            return valid;
        }

        protected void Btn_updateotherdetails_Click(object sender, EventArgs e)
        {
            string _message;
            try
            {
                int _Bloodgroupid = int.Parse(Drp_BloodGrp.SelectedValue.ToString());
                string _nationality = Txt_Nationality.Text.ToUpper();
                int mothertoungid = int.Parse(Drp_MotherTongue.SelectedValue.ToString());
                string _mothername = Txt_MotherName.Text.ToUpper();
                string _Fathedu = Txt_FatherEduQuali.Text.ToUpper();
                string _mothedu = Txt_MotherEduQuali.Text.ToUpper();
                string _fatherOcc = Txt_FatherOccupation.Text.ToUpper();
                string _motherOcc = Txt_MotherOccupation.Text.ToUpper();
                string aadharno = Txt_aadharno.Text;

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
                string _SecondaryPhno= Txt_SecondaryPh.Text;
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
                    studcatogory = 0;
                }
                int AdmissionType = 1;
                if (Chk_NewAdmin.Checked)
                {
                    AdmissionType = 2;
                }

              //MyStudMang.UpdateStudentOtherDetails(_Bloodgroupid, _nationality, mothertoungid, _mothername, _Fathedu, _mothedu, _fatherOcc, _motherOcc, _Anualincom, _addrspresent, _location, _State, _pin, _resedphon, _officephon, _email, nofbrother, nofsis, firstlng, studcatogory, int.Parse(Hdn_StudentID.Value), AdmissionType, int.Parse(Rdb_NeedBus.SelectedValue), int.Parse(Rdb_NeedHostel.SelectedValue));
               MyStudMang.UpdateStudentOtherDetails(_Bloodgroupid, _nationality, mothertoungid, _mothername, _Fathedu, _mothedu, _fatherOcc, _Anualincom, _addrspresent, _location, _State, _pin, _resedphon, _officephon, _email, nofbrother, nofsis, firstlng, studcatogory, int.Parse(Hdn_StudentID.Value), AdmissionType, int.Parse(Rdb_NeedBus.SelectedValue), int.Parse(Rdb_NeedHostel.SelectedValue), _motherOcc,aadharno);

                //MyStudMang.UpdateParentSecondaryPhNo(int.Parse(Hdn_StudentID.Value),_SecondaryPhno);
                UpdateStudentTripMap(Hdn_StudentID.Value);
                if (_officephon.Trim() != "")
                {
                    MyStudMang.InsertParentMobileNumberIntoSMSParenstsList(int.Parse(Hdn_StudentID.Value), _officephon.Trim(), _SecondaryPhno);
                    //InsertParentMobileNumberIntoSMSParenstsList();
                }
                UpdateCoustomFields();
                if (Txt_Email.Text != "")
                {
                    MyStudMang.CreateTansationDb();
                    MyStudMang.InsertParentEmailIdIntoEmailParenstsList(int.Parse(Hdn_StudentID.Value), Txt_Email.Text.Trim());
                    MyStudMang.EndSucessTansationDb();
                }
                if (EVNTSave != null)
                {
                    EVNTSave(this, e);
                }
                _message = "Details are Updated";

                MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Updated Student Other details", "Other details Of Student " + MyUser.m_DbLog.GetStudentName(int.Parse(Hdn_StudentID.Value)) + " is Updated", 1,1);
            }
            catch
            {
                _message = "Unable to Update Other Details";
            }

            Lbl_msg1.Text = _message;
            MPE_MessageBox1.Show();
        }

        private void UpdateStudentTripMap(string studid)
        {
            int studentid = 0, fromtripid = 0, totripid = 0;
            OdbcDataReader tripreader = null;
            try
            {
                if (Rdb_NeedBus.SelectedValue == "0")
                {
                    int.TryParse(studid, out studentid);
                    string sql = "select tbl_tr_studtripmap.FromTripId, tbl_tr_studtripmap.ToTripId from tbl_tr_studtripmap where tbl_tr_studtripmap.StudId=" + studid + "";
                    tripreader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    if (tripreader.HasRows)
                    {

                        string tripsql = "";
                        int.TryParse(tripreader.GetValue(0).ToString(), out fromtripid);
                        int.TryParse(tripreader.GetValue(1).ToString(), out totripid);
                        tripsql = "delete from tbl_tr_studtripmap where tbl_tr_studtripmap.StudId=" + studid + "";
                        MyStudMang.m_MysqlDb.ExecuteQuery(tripsql);
                        tripsql = "update tbl_tr_trips SET tbl_tr_trips.Occupied=tbl_tr_trips.Occupied-1 where tbl_tr_trips.id=" + fromtripid + "";
                        MyStudMang.m_MysqlDb.ExecuteQuery(tripsql);
                        tripsql = "update tbl_tr_trips SET tbl_tr_trips.Occupied=tbl_tr_trips.Occupied-1 where tbl_tr_trips.id=" + totripid + "";
                        MyStudMang.m_MysqlDb.ExecuteQuery(tripsql);
                    }
                }
            }
            catch (Exception)
            {
            }

        }

        private void UpdateCoustomFields()
        {
            int CustfieldCount = MyStudMang.CoustomFieldCount;

            if (CustfieldCount > 0)
            {
                if (MyStudMang.HaveCoustomStudentDetails(int.Parse(Hdn_StudentID.Value)))
                {
                    UpdateDetails(CustfieldCount);
                }
                else
                {
                    InsertCoustomDetails(CustfieldCount, int.Parse(Hdn_StudentID.Value));
                }
            }


        }

        private bool InsertCoustomDetails(int CustfieldCount, int _userId)
        {
            bool valid = false;

            if (CustfieldCount > 0)
            {
                int i;
                string Fields, Values;
                Fields = "StudentId";
                Values = "'" + _userId.ToString() + "'";
                for (i = 0; i < CustfieldCount; i++)
                {
                    Fields = Fields + "," + FealdName[i];
                    Values = Values + "," + "'" + dynamicTextBoxes[i].Text + "'";

                }
                valid = MyStudMang.InsertStudentDetails(Fields, Values);

            }
            else
            {
                valid = true;
            }
            return valid;
        }

        private void UpdateDetails(int CustfieldCount)
        {
            int i;
            string Values = "";

            for (i = 0; i < CustfieldCount; i++)
            {
                if (i == 0)
                {
                    Values = FealdName[i] + "='" + dynamicTextBoxes[i].Text + "'";
                }
                else
                {
                    Values = Values + "," + FealdName[i] + "='" + dynamicTextBoxes[i].Text + "'";

                }


            }

            string sql = "UPDATE tblstudentdetails SET " + Values + " WHERE StudentId=" + Hdn_StudentID.Value;
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
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
        //private bool ValidImageFileFath()
        //{
        //    bool fileOK = false;
        //    string fileExtension = System.IO.Path.GetExtension(FileUp_Father.FileName).ToLower();
        //    string[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg" };
        //    for (int i = 0; i < allowedExtensions.Length; i++)
        //    {
        //        if (fileExtension == allowedExtensions[i])
        //        {
        //            fileOK = true;
        //        }

        //    }
        //    return fileOK;
        //}
        private Size NewImageSize(int OriginalHeight, int OriginalWidth, double FormatSize)
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


        private void AddPhotoPrevbtch(int _StudentId, int _classId)
        {
            string ImageUrl = "";
            String Sql = "";
            string preimage = "";
            int UsrId;
            string ImageName = FileUp_Student.FileName.ToString();
            FileUp_Student.SaveAs(MyUser.FilePath + "\\UpImage\\" + ImageName);
            // string strVirtualPath="http://localhost:1334/WinSchool/UpImage/" + ImageName;
            string ThumbnailPath = (MyUser.FilePath + "\\ThumbnailImages\\" + "Student" + _StudentId.ToString() + "Class" + _classId + ImageName);
            using (System.Drawing.Image Img = System.Drawing.Image.FromFile(MyUser.FilePath + "\\UpImage\\" + ImageName))
            {
                Size ThumbNailSize = NewImageSize(Img.Height, Img.Width, 100);
                using (System.Drawing.Image ImgThnail =
                new Bitmap(Img, ThumbNailSize.Width, ThumbNailSize.Height))
                {
                    ImgThnail.Save(ThumbnailPath, Img.RawFormat);
                    ImgThnail.Dispose();
                }
                Img.Dispose();

            }
            ImageUrl = "Student" + _StudentId + ImageName;

            UsrId = _StudentId;




            bool success = false;
            //if (MyStudMang.HasImageWitCls(UsrId, _classId, out preimage))
            //{
            //    // Sql = "UPDATE tblfileurl SET FilePath='" + ImageUrl + "' WHERE Type='StudentImage' AND UserId=" + _StudentId;

            //    byte[] imagebytes = General.getImageinBytefromImage(ThumbnailPath);

            //    ImageUploaderClass imgUpload = new ImageUploaderClass(objSchool);

            //    success = imgUpload.UpdateImageFilePrevbtch(imagebytes, _StudentId, "StudentImage", _classId, ImageName);
            //}
            //else
            //{
            //    //  Sql = "INSERT INTO tblfileurl(UserId,FilePath,Type) VALUES(" + _StudentId + ", '" + ImageUrl + "','StudentImage')";

            //    byte[] imagebytes = General.getImageinBytefromImage(ThumbnailPath);

            //    ImageUploaderClass imgUpload = new ImageUploaderClass(objSchool);

            //    success = imgUpload.InsertImageFilePrevBtch(imagebytes, _StudentId, "StudentImage", _classId, ImageName);
            //}


            if (ImageName != "")
            {
                File.Delete(MyUser.FilePath + "\\UpImage\\" + ImageName);
            }
            if ((preimage != "") && (preimage != ImageUrl))
            {
                File.Delete(MyUser.FilePath + "\\ThumbnailImages\\" + preimage);
            }

        }



        //private void AddPhotoFather(int _StudentId)
        //{
        //    string ImageUrl = "";
        //    String Sql = "";
        //    string preimage = "";
        //    int UsrId;
        //    string ImageName = FileUp_Father.FileName.ToString();
        //    FileUp_Father.SaveAs(MyUser.FilePath + "\\UpImage\\" + ImageName);
        //    // string strVirtualPath="http://localhost:1334/WinSchool/UpImage/" + ImageName;
        //    string ThumbnailPath = (MyUser.FilePath + "\\ThumbnailImages\\" + "Parent" + _StudentId.ToString() + ImageName);
        //    using (System.Drawing.Image Img = System.Drawing.Image.FromFile(MyUser.FilePath + "\\UpImage\\" + ImageName))
        //    {
        //        Size ThumbNailSize = NewImageSize(Img.Height, Img.Width, 200);
        //        using (System.Drawing.Image ImgThnail =
        //        new Bitmap(Img, ThumbNailSize.Width, ThumbNailSize.Height))
        //        {
        //            ImgThnail.Save(ThumbnailPath, Img.RawFormat);
        //            ImgThnail.Dispose();
        //        }
        //        Img.Dispose();

        //    }
        //    ImageUrl = "Parent" + _StudentId + ImageName;

        //    UsrId = _StudentId;




        //    bool success = false;
        //    if (MyStudMang.HasImage(UsrId, out preimage))
        //    {
        //        // Sql = "UPDATE tblfileurl SET FilePath='" + ImageUrl + "' WHERE Type='StudentImage' AND UserId=" + _StudentId;

        //        byte[] imagebytes = General.getImageinBytefromImage(ThumbnailPath);

        //        ImageUploaderClass imgUpload = new ImageUploaderClass(objSchool);

        //        success = imgUpload.UpdateImageFile(imagebytes, _StudentId, "ParentImage");
        //    }
        //    else
        //    {
        //        //  Sql = "INSERT INTO tblfileurl(UserId,FilePath,Type) VALUES(" + _StudentId + ", '" + ImageUrl + "','StudentImage')";

        //        byte[] imagebytes = General.getImageinBytefromImage(ThumbnailPath);

        //        ImageUploaderClass imgUpload = new ImageUploaderClass(objSchool);

        //        success = imgUpload.InsertImageFile(imagebytes, _StudentId, "ParentImage");
        //    }


        //    if (ImageName != "")
        //    {
        //        File.Delete(MyUser.FilePath + "\\UpImage\\" + ImageName);
        //    }
        //    if ((preimage != "") && (preimage != ImageUrl))
        //    {
        //        File.Delete(MyUser.FilePath + "\\ThumbnailImages\\" + preimage);
        //    }

        //}



      

        protected void Btn_Upload_Click(object sender, EventArgs e)
        {
            if (FileUp_Student.PostedFile != null && !ValidImageFile())
            {
                Lbl_msg2.Text = "File type cannot be uploaded";
                this.MPE_MessageBox2.Show();

            }
            else if (FileUp_Student.PostedFile == null)
            {
                Lbl_msg2.Text = "Please Select an image to upload";
                this.MPE_MessageBox2.Show();
            }
            else
            {
                int _StudentId = int.Parse(Hdn_StudentID.Value);
                int _classId = int.Parse(Drp_selectClass.SelectedValue);
                int classId = int.Parse(Hdn_ClassId.Value);

                if(_classId == classId)
                {
                    AddPhoto(_StudentId);
                }
                else
                {
                    AddPhotoPrevbtch(_StudentId, _classId);
                }
                
                MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Upload Photo", "Photo Of Student " + MyUser.m_DbLog.GetStudentName(_StudentId) + " is changed", 1,1);
                if (EVNTSave != null)
                {
                    EVNTSave(this, e);
                }
                Lbl_msg2.Text = "Image Uploaded.";
                this.MPE_MessageBox2.Show();
            }

        }

        //protected void Btn_UploadFather_Click(object sender, EventArgs e)
        //{
        //    if (FileUp_Father.PostedFile != null && !ValidImageFileFath())
        //    {
        //        Lbl_msg2.Text = "File type cannot be uploaded";
        //        this.MPE_MessageBox2.Show();

        //    }
        //    else if (FileUp_Father.PostedFile == null)
        //    {
        //        Lbl_msg2.Text = "Please Select an image to upload";
        //        this.MPE_MessageBox2.Show();
        //    }
        //    else
        //    {
        //        int _StudentId = int.Parse(Hdn_StudentID.Value);
        //        AddPhotoFather(_StudentId);
        //        MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Upload Photo", "Photo Of Parent " + MyUser.m_DbLog.GetStudentName(_StudentId) + " is changed", 1, 1);
        //        if (EVNTSave != null)
        //        {
        //            EVNTSave(this, e);
        //        }
        //        Lbl_msg2.Text = "Image Uploaded.";
        //        this.MPE_MessageBox2.Show();
        //    }

        //}

        protected void Btn_UploadMother_Click(object sender,EventArgs e)
        {

        }
        protected void Lnk_AddSiblings_Click(object sender, EventArgs e)
        {
            Txt_StudentName.Text = "";
            Txt_ParentName.Text = "";
            Txt_PhoneNum.Text = "";
            MPE_ADDSIBLINGS.Show();
            Pnl_SearchSiblingsDisplay.Visible = false;
        }

        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            LoadStudents();
           
        }

        private void LoadStudents()
        {
           // throw new NotImplementedException();
            string sql = "";
            Lbl_Err.Text = "";
            DataSet StudentDs = new DataSet();
            string Tempsql = "";
            if (Txt_StudentName.Text != "")
            {
                Tempsql = " and StudentName='" + Txt_StudentName.Text + "'";
                
            }
            if (Txt_ParentName.Text != "")
            {

                    Tempsql =Tempsql+ " and GardianName='" + Txt_ParentName.Text + "'";

                
               

            }
            if (Txt_PhoneNum.Text != "")
            {

                Tempsql = Tempsql + " and OfficePhNo='" + Txt_PhoneNum.Text + "'";
 

            }
            sql = "select Id,StudentName,GardianName from tblstudent WHERE Id<>" + Hdn_StudentID.Value + Tempsql + "";
            StudentDs = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            
            if (StudentDs != null && StudentDs.Tables[0].Rows.Count > 0)
            {           
                    Pnl_SearchSiblingsDisplay.Visible = true;
                    Grd_SearchSiblings.Columns[1].Visible = true;
                    Grd_SearchSiblings.DataSource = StudentDs;
                    Grd_SearchSiblings.DataBind();
                    Grd_SearchSiblings.Columns[1].Visible = false; ;
               
            }
            else
            {
                Lbl_Err.Text = "No students found";
                Pnl_SearchSiblingsDisplay.Visible = false;
                Grd_SearchSiblings.DataSource = null;
                Grd_SearchSiblings.DataBind();
            }
            MPE_ADDSIBLINGS.Show();
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            CheckBox chk = new CheckBox();
            int count = 0;
            OdbcDataReader studReader = null;
            string Sql1 = "";
            OdbcDataReader Idreader = null;                     
            OdbcDataReader StudIdreader = null;
            Grd_SearchSiblings.Columns[1].Visible = true;
            int SiblingId = 0;
            foreach (GridViewRow gv in Grd_SearchSiblings.Rows)
            {
                chk = (CheckBox)gv.FindControl("CheckBoxUpdate");
                if (chk.Checked)
                {
                    try
                    {
                      
                        count = 1;
                        string sql = "";
                        sql = "select tbl_siblingsmap.Id from tbl_siblingsmap where tbl_siblingsmap.StudId=" + int.Parse(Hdn_StudentID.Value) + "";
                        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                        if (MyReader.HasRows)
                        {
                            OdbcDataReader Reader = null;
                            Sql1 = "select Id from tbl_siblingsmap where tbl_siblingsmap.StudId=" + int.Parse(gv.Cells[1].Text) + "";
                            Reader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql1);
                            if (!Reader.HasRows)
                            {
                                SiblingId = int.Parse(MyReader.GetValue(0).ToString());
                                sql = "insert into tbl_siblingsmap(Id,StudId) values(" + int.Parse(MyReader.GetValue(0).ToString()) + "," + int.Parse(gv.Cells[1].Text) + ")";
                                MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                            }
                        }
                        else
                        {

                            sql = "select tbl_siblingsmap.Id from tbl_siblingsmap where tbl_siblingsmap.StudId=" + int.Parse(gv.Cells[1].Text) + "";
                            studReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                            if (studReader.HasRows)
                            {
                                SiblingId = int.Parse(studReader.GetValue(0).ToString());
                                sql = "insert into tbl_siblingsmap(Id,StudId) values(" + int.Parse(studReader.GetValue(0).ToString()) + "," + int.Parse(Hdn_StudentID.Value) + ")";
                                MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                            }
                            else
                            {

                                //if (count == 0)
                                //{
                                string _sql = "select max(Id) from tbl_siblingsmap";
                                StudIdreader = MyStudMang.m_MysqlDb.ExecuteQuery(_sql);
                                if (StudIdreader.HasRows)
                                {
                                    int id = 0;
                                    int.TryParse(StudIdreader.GetValue(0).ToString(), out id);
                                    id = id + 1;
                                    SiblingId = id;
                                    sql = "insert into tbl_siblingsmap(Id,StudId) values(" + id + "," + int.Parse(Hdn_StudentID.Value) + ")";
                                    MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                                    if (int.Parse(Hdn_StudentID.Value) != int.Parse(gv.Cells[1].Text))
                                    {
                                        Sql1 = "select Id from tbl_siblingsmap where tbl_siblingsmap.StudId=" + int.Parse(gv.Cells[1].Text) + "";
                                        Idreader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql1);
                                        if (!Idreader.HasRows)
                                        {
                                            sql = "insert into tbl_siblingsmap(Id,StudId) values(" + id + "," + int.Parse(gv.Cells[1].Text) + ")";
                                            MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                                        }
                                        // MyUser.m_DbLog.LogToDb(MyUser.UserName, "Add siblings", " "+int.Parse(gv.Cells[1].Text)+"  ", 1);
                                        //count = 1;
                                    }
                                }
                                // }
                                //else
                                //{
                                //    sql = "insert into tbl_siblingsmap(Id,StudId) values(" + int.Parse(StudIdreader.GetValue(0).ToString()) + "," + int.Parse(gv.Cells[1].Text) + ")";
                                //    MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                                //}
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                      
                        Lbl_Err.Text = "Cannot add,Please try again. Error reason : "+ex.Message;
                    }
                }

            }
            Grd_SearchSiblings.Columns[1].Visible = false;
            MPE_ADDSIBLINGS.Show();
            if (count == 1)
            {
                UpdateSibling_In_ParentLoginCredentials(SiblingId);
                LoadSiblingDetails();
                Lbl_Err.Text = "Saved successfully";
                Txt_StudentName.Text = "";
                Txt_ParentName.Text = "";
                Txt_PhoneNum.Text = "";
                
            }
            else
            {
                Lbl_Err.Text = "Select student";
            }
           
        }

        private void UpdateSibling_In_ParentLoginCredentials(int SiblingId)
        {
            if (!IsSiblingInParentLogin(SiblingId))
            {
                string _sql = "SELECT tblparent_parentstudentmap.ParentId FROM tbl_siblingsmap INNER JOIN tblparent_parentstudentmap ON tblparent_parentstudentmap.StudentId=tbl_siblingsmap.StudId WHERE tbl_siblingsmap.Id=" + SiblingId;
                OdbcDataReader _Reader = MyStudMang.m_MysqlDb.ExecuteQuery(_sql);
                if (_Reader.HasRows)
                {
                    while (_Reader.Read())
                    {
                        int ParentId = 0;
                        int.TryParse(_Reader.GetValue(0).ToString(), out ParentId);
                        if (ParentId > 0)
                        {
                            UpdateSibling_ParentLogin(SiblingId, ParentId);
                            break;
                        }
                    }
                }
            }

        }

        private void UpdateSibling_ParentLogin(int SiblingId, int ParentId)
        {
            string sql = "UPDATE tblparent_parentdetails SET tblparent_parentdetails.SiblingId=" + SiblingId + " WHERE tblparent_parentdetails.Id=" + ParentId;
            MyStudMang.m_MysqlDb.ExecuteQuery(sql);
        }

        private bool IsSiblingInParentLogin(int SiblingId)
        {
            bool _valid = false;
            string sql = "SELECT COUNT(tblparent_parentdetails.SiblingId) FROM tblparent_parentdetails WHERE tblparent_parentdetails.SiblingId="+SiblingId;
            OdbcDataReader CountReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (CountReader.HasRows)
            {
                int _count = 0;
                int.TryParse(CountReader.GetValue(0).ToString(), out _count);
                if (_count > 0)
                {
                    _valid = true;
                }
            }
            return _valid;
        }

        protected void Grd_SearchSiblings_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_SearchSiblings.PageIndex = e.NewPageIndex;
            LoadStudents();
        }

        protected void GrdSiblings_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "",_sql="";
            int count = 0;
            OdbcDataReader CountReader = null;
            sql = "select count(Id),Id from tbl_siblingsmap where tbl_siblingsmap.Id in(select Id from tbl_siblingsmap where   tbl_siblingsmap.StudId="+int.Parse(GrdSiblings.SelectedRow.Cells[0].Text)+")";
            CountReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (CountReader.HasRows)
            {
                int.TryParse(CountReader.GetValue(0).ToString(), out count);
                if (count > 0)
                {
                    if (count == 2)
                    {
                        _sql = "delete from tbl_siblingsmap where Id=" + int.Parse(CountReader.GetValue(1).ToString()) + "";
                        MyStudMang.m_MysqlDb.ExecuteQuery(_sql);
                        _sql = "update tblparent_parentdetails set tblparent_parentdetails.SiblingId=0 where tblparent_parentdetails.SiblingId=" + int.Parse(CountReader.GetValue(1).ToString()) + "";
                        MyStudMang.m_MysqlDb.ExecuteQuery(_sql);

                    }
                    else
                    {
                        _sql = "delete from tbl_siblingsmap where Id=" + int.Parse(CountReader.GetValue(1).ToString()) + " and tbl_siblingsmap.StudId=" + int.Parse(GrdSiblings.SelectedRow.Cells[0].Text) + "";
                        MyStudMang.m_MysqlDb.ExecuteQuery(_sql);
                    }
                }
            }
            LoadSiblingDetails();
        }

        protected void Lnk_Cam_Click(object sender, EventArgs e)
        {
            Session["SaveType"] = "StudId";
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "window.open('silverlitepicturecapture.aspx','','width=700,height=500')", true);
        }

        protected void Rdb_NeedBus_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckIfStudentHasToPaidTransportationFee();
            
        }

        protected void Grd_TransFee_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {           
            Grd_TransFee.PageIndex = e.NewPageIndex;
            CheckIfStudentHasToPaidTransportationFee();
            
        }
        protected void Btn_TransOk_Click(object sender, EventArgs e)
        {
            int feeid=0;
            CheckBox chk=new CheckBox();
            string sql="";
            try
            {
                foreach (GridViewRow gr in Grd_TransFee.Rows)
                {
                    chk = (CheckBox)gr.FindControl("ChkFee");
                    if (chk.Checked)
                    {
                        int.TryParse(gr.Cells[0].Text, out feeid);
                        sql = "Update tblfeestudent set  tblfeestudent.Status='fee Exemtion' where Id=" + feeid + "";
                        MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                        CheckIfStudentHasToPaidTransportationFee();
                        Lbl_TransErrMsg.Text = "Fee removed successfully!";
                    }
                }
            }
            catch
            {
                Lbl_TransErrMsg.Text = "Error,Can't remove fee!";               
            }
            MPE_TRANSFEE.Show();
        }

        private void CheckIfStudentHasToPaidTransportationFee()
        {

            int studid = 0, classid = 0;
            DataSet FeeDs = new DataSet();
            try
            {
                if (MyUser.HaveModule(26))
                {
                    if (Rdb_NeedBus.SelectedValue == "0")
                    {
                        int.TryParse(Hdn_ClassId.Value, out classid);
                        int.TryParse(Hdn_StudentID.Value, out studid);
                        string sql = "";
                        sql = "SELECT DISTINCT  tblfeestudent.Id,tblfeestudent.BalanceAmount , tblperiod.Period FROM tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId inner join tblperiod on  tblperiod.Id=tblfeeschedule.PeriodId   inner join tblstudentclassmap on tblfeeschedule.ClassId= tblstudentclassmap.ClassId inner join tblstudent on tblstudent.Id= tblfeestudent.StudId  where  tblstudentclassmap.BatchId= 44 AND tblstudent.`Status`=1 AND  tblfeestudent.Status<>'Paid' and tblfeestudent.`Status`<> 'Fee Exemtion' AND tblstudentclassmap.ClassId=" + classid + "   AND tblfeeschedule.FeeId=100     and tblfeestudent.StudId=" + studid + " ";
                        FeeDs = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                        if (FeeDs != null && FeeDs.Tables[0].Rows.Count > 0)
                        {
                            Grd_TransFee.Columns[0].Visible = true;
                            Grd_TransFee.DataSource = FeeDs;
                            Grd_TransFee.DataBind();
                            Grd_TransFee.Columns[0].Visible = false;
                            Lbl_TransMsg.Text = "Student has to pay Transportation fee of following months.If you want to remove, select fee and click on Ok,else click on cancel!";
                            MPE_TRANSFEE.Show();
                        }
                        else
                        {
                            Grd_TransFee.DataSource = null;
                            Grd_TransFee.DataBind();
                            Lbl_TransMsg.Text = "";

                        }
                    }
                }
            }
            catch (Exception)
            {

            }

        }
    }
}