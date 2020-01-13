using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using WinBase;
using MySql.Data.MySqlClient;
using WinEr;
public partial class ManageStaff : System.Web.UI.Page
{
    private StaffManager MyStaffMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
      private SchoolClass objSchool = null;
    protected void Page_PreInit(Object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        MyUser = (KnowinUser)Session["UserObj"];

        if (MyUser.SELECTEDMODE == 1)
        {
            this.MasterPageFile = "~/WinerStudentMaster.master";

        }
        else if (MyUser.SELECTEDMODE == 2)
        {

            this.MasterPageFile = "~/WinerSchoolMaster.master";
        }

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
        

    }
    protected void Page_init(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        if (Session["StaffId"] == null)
        {
            Response.Redirect("ViewStaffs.aspx");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyStaffMang = MyUser.GetStaffObj();
        if (MyStaffMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }
        else if (!MyUser.HaveActionRignt(19))
        {
            Response.Redirect("RoleErr.htm");
        }
        else
        {


            if (!IsPostBack)
            {
                if (MyUser.HaveModule(29))
                {
                    PnlPayrollYesNo.Visible = true;
                    PnlPayrollDetails.Visible = false;
                    RdbNo.Checked = true;
                    RdbYes.Checked = false;
                    LoadPayrollActive();
                }
                else
                {
                    PnlPayrollYesNo.Visible = false;
                }

                string _SubMenuStr;
                _SubMenuStr = MyStaffMang.GetSubStaffMangMenuString(MyUser.UserRoleId);
                this.SubStaffMenu.InnerHtml = _SubMenuStr;
                AddRoleToDrpList();
                AddAllGroupsToDropDown();
                
                AddStaffDetails();
                AddSubjectToChkList();
                setpanel();
                SetResetRight();
                LoaduserTopData();
                if (MyUser.HaveModule(3))
                {
                    Pnl_Subjects.Visible = true;
                }
                else
                {
                    Pnl_Subjects.Visible = false;
                }
                //some initlization
            }
        }
    }
   
    private void LoadPayrollActive()
    {
        TxtEmpId.Enabled = true;
        PnlPayrollYesNo.Visible = true;
        int status;
        if (MyUser.HaveModule(29))
        {
            string sql = "SELECT Id,EmpId,PAN,BankName,AccNo,`status` FROM tblpay_employee where StaffId = " + Session["StaffId"].ToString() + "";
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                PnlPayrollYesNo.Visible = true;
                PnlPayrollDetails.Visible = true;                
                status = int.Parse(MyReader.GetValue(5).ToString());
                if (status == 1)
                {
                    RdbYes.Checked = true;
                    RdbNo.Checked = false;
                }
                else
                {
                    RdbYes.Checked = false;
                    RdbNo.Checked = true;
                }
                TxtEmpId.Text = MyReader.GetValue(1).ToString();
                TxtPan.Text = MyReader.GetValue(2).ToString();
                TxtBankName.Text = MyReader.GetValue(3).ToString();
                TxtAcc.Text = MyReader.GetValue(4).ToString();
                FillDrpPayroll();
                TxtEmpId.Enabled = false;

            }
            else
            {
                RdbYes.Checked = false; ;
                RdbNo.Checked = true;
            }

        }

    }

    private void AddAllGroupsToDropDown()
    {
        DataSet GroupDataset = new DataSet();
        Drp_Group.Items.Clear();
        GroupDataset = MyUser.MyAssociatedGroups();
        if (GroupDataset.Tables[0].Rows.Count>0)
        {
            foreach(DataRow Group in GroupDataset.Tables[0].Rows)            
            {
                ListItem li = new ListItem(Group[1].ToString(),Group[0].ToString());
                Drp_Group.Items.Add(li);
            }
        }
    }

    private void LoaduserTopData()
    {
        ImageUploaderClass imgobj = new ImageUploaderClass(objSchool);
        int id = int.Parse(Session["StaffId"].ToString());
        string type = "StaffImage";
        string _Studstrip = MyStaffMang.ToStripString(int.Parse(Session["StaffId"].ToString()),"Handler/ImageReturnHandler.ashx?id=" + id + "&type=" + type);
        this.StudentTopStrip.InnerHtml = _Studstrip;
    }

    private void SetResetRight()
    {
        if (!MyUser.HaveActionRignt(91))
        {
            Rdb_Login.Enabled = false;
           
        }
    }

    private void AddRoleToDrpList()
    {        
        Drp_SelectRole.Items.Clear();
        String Sql = "SELECT Id,RoleName FROM tblrole WHERE Type='Staff'";
        MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                Drp_SelectRole.Items.Add(li);
            }            
        }
        else
        {
            Drp_SelectRole.Items.Add(new ListItem("No role found", "-1"));
            Btn_Update.Enabled = false;
            Lbl_Failue.Text = "Cannot update. The roles have been deleted"; 
        }
    }

    private void AddSubjectToChkList()
    {
        ChkBox_AllsSub.Items.Clear();
        ChkBox_AssSub.Items.Clear();
        string sql = "SELECT Id,subject_name FROM tblsubjects";
        MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                if (MyStaffMang.IsStaffSubject(int.Parse(Session["StaffId"].ToString()), int.Parse(MyReader.GetValue(0).ToString())))
                {
                    ChkBox_AssSub.Items.Add(li);
                }
                else
                {
                    ChkBox_AllsSub.Items.Add(li);
                }
            }
        }
    }



    private void AddStaffDetails()
    {
        DateTime JoiningDate;
        DateTime Dob;
        String Sql = "SELECT tbluser.SurName,tbluser.EmailId,tbluser.UserName,tbluser.RoleId,tbluser.CanLogin,tblgroupusermap.GroupId FROM tbluser inner join tblgroupusermap on tbluser.Id= tblgroupusermap.UserId  WHERE tbluser.Id=" + int.Parse(Session["StaffId"].ToString());
        MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
            MyReader.Read();
            Txt_StaffName.Text = MyReader.GetValue(0).ToString();
            Txt_Email.Text = MyReader.GetValue(1).ToString();
            Txt_StaffId.Text = MyReader.GetValue(2).ToString();
            Drp_SelectRole.SelectedValue = MyReader.GetValue(3).ToString();
            Rdb_Login.SelectedValue = MyReader.GetValue(4).ToString();
            Drp_Group.SelectedValue = MyReader.GetValue(5).ToString();
        }
       
        Sql = "SELECT Experience,Designation,JoiningDate,Address,Sex,Dob,PhoneNumber,EduQualifications,AadharNo,PanNo FROM tblstaffdetails WHERE UserId=" + int.Parse(Session["StaffId"].ToString());
        MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
            MyReader.Read();
            Txt_Experience.Text = MyReader.GetValue(0).ToString();
            Txt_Desig.Text = MyReader.GetValue(1).ToString();
            JoiningDate = DateTime.Parse(MyReader.GetValue(2).ToString());
            //JoiningDate = MyUser.GetDareFromText(MyReader.GetValue(2).ToString());

            //Txt_JoiningDate.Text = JoiningDate.Date.ToString("dd/MM/yyyy");
            Txt_JoiningDate.Text = JoiningDate.Date.Day + "/" + JoiningDate.Date.Month + "/" + JoiningDate.Date.Year;

            Txt_Address.Text = MyReader.GetValue(3).ToString();
            RbLst_Sex.SelectedValue = MyReader.GetValue(4).ToString();
            Dob = DateTime.Parse(MyReader.GetValue(5).ToString());
            //Dob =MyUser.GetDareFromText(MyReader.GetValue(5).ToString());

            //Txt_Dob.Text = Dob.Date.ToString("dd/MM/yyyy");
            Txt_Dob.Text = Dob.Date.Day + "/" + Dob.Date.Month + "/" + Dob.Date.Year;
          
            Txt_PhNo.Text = MyReader.GetValue(6).ToString();
            Txt_EduQuli.Text = MyReader.GetValue(7).ToString();
            Txt_Aadhar.Text = MyReader.GetValue(8).ToString();
            Txt_PAN.Text = MyReader.GetValue(9).ToString();
        }

    }

    protected void Btn_Add_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < ChkBox_AllsSub.Items.Count; i++)
        {
            if (ChkBox_AllsSub.Items[i].Selected)
            {
                ChkBox_AllsSub.Items[i].Selected = false;
                ChkBox_AssSub.Items.Add(ChkBox_AllsSub.Items[i]);
                ChkBox_AllsSub.Items.Remove(ChkBox_AllsSub.Items[i]);
                i--;
            }
        }
    }
  
    protected void Btn_Remove_Click(object sender, EventArgs e)
    {       
        for (int i = 0; i < ChkBox_AssSub.Items.Count; i++)
        {
            if (ChkBox_AssSub.Items[i].Selected)
            {
                ChkBox_AssSub.Items[i].Selected = false;
                ChkBox_AllsSub.Items.Add(ChkBox_AssSub.Items[i]);
                ChkBox_AssSub.Items.Remove(ChkBox_AssSub.Items[i]);
                i--;
            }
        }
    }

    protected void Rdb_Login_SelectedIndexChanged(object sender, EventArgs e)
    {
        setpanel();       
    }

    private void setpanel()   
    {
       if (Rdb_Login.SelectedIndex == 0)
       {
           Panel2.Visible = false;
           Lnk_Reset.Visible = false;
       }
       else
       {           
           Lnk_Reset.Visible = true;
       } 
    }
  
    protected void Lnk_reset_Click(object sender, EventArgs e) 
    {
       if (Lnk_Reset.Text == "Reset Password")
       {
           Panel2.Visible = true;
           Lnk_Reset.Text = "Hide";
           Txt_Password.Text = "";
           Txt_RePassword.Text = "";
       }
       else
       {
           Panel2.Visible = false;
           Lnk_Reset.Text = "Reset Password";
           Txt_Password.Text = "";
           Txt_RePassword.Text = "";
       }
    }

    protected void Btn_Update_Click(object sender, EventArgs e)
    {
        OdbcDataReader m_empheadreader = null;
        double _BasicPay = 0;
        double _Gross = 0;
        double _NetPay = 0;
        string _message;
        if (Txt_StaffName.Text.Trim() == "" || Txt_JoiningDate.Text.Trim() == "" || Txt_Address.Text.Trim() == "" || Txt_Experience.Text.Trim() == "" || Txt_Dob.Text.Trim() == "" || Txt_StaffId.Text.Trim() == "")//(Rdb_Login.SelectedIndex == 1 && Txt_StaffId.Text.Trim() == ""))
        {
            Lbl_Failue.Text = "One Or more fields are empty";

        }
        else if (!MyStaffMang.roleExists(int.Parse(Drp_SelectRole.SelectedValue.ToString())))
        {
            Lbl_Failue.Text = "The selected role no longer exists. Please refresh your page";

        }
        /* else if (LnkBtn_Reset.Text =="Hide" && Rdb_Login.SelectedIndex == 1 && (Txt_Password.Text.Trim() == "" || Txt_Password.Text != Txt_RePassword.Text))
         {
             Lbl_Failue.Text = "Re-enter password...";
         }*/

        else if (Rdb_Login.SelectedIndex == 1 && MyUser.m_LicenseObject.m_usercount != -1 && !LoginAlreadyAllowed() && MyUser.UserCountExceeds())
        {
            Lbl_Failue.Text = "Cannot create more login users.The number of users for your license is only : " + MyUser.m_LicenseObject.m_usercount;
        }
        else if (Rdb_Login.SelectedIndex == 1 && Txt_Password.Text.Trim() == "" && Lnk_Reset.Text == "Hide")
        {
            Lbl_Failue.Text = "Please Enter the Passwrod";
        }
        else if (Rdb_Login.SelectedIndex == 1 && (Txt_Password.Text != Txt_RePassword.Text) && !(Txt_Password.Text.Trim() == ""))
        {
            Lbl_Failue.Text = "Password doesn't match";

        }
        else if (Rdb_Login.SelectedIndex == 1 && (!MyStaffMang.IsValidStaffId(Txt_StaffId.Text) && CompareStaffId()))
        {
            Lbl_Failue.Text = "Login Name Allready exists";

        }
        /*else if (Rdb_Login.SelectedIndex == 0 && Txt_StaffId.Text != "" && !MyStaffMang.IsValidStaffId(Txt_StaffId.Text))
        {
            Lbl_Failue.Text = "StaffId Already exists";
        }*/
        else if (!CheckDate(out _message))
        {
            Lbl_Failue.Text = _message;

        }
        else
        {
            int _StaffId;
            _StaffId = MyStaffMang.UpdateStaff(Txt_StaffName.Text, Txt_Password.Text, Txt_JoiningDate.Text, Txt_Address.Text, RbLst_Sex.SelectedValue.ToString(), float.Parse(Txt_Experience.Text.ToString()), Txt_Desig.Text, Txt_PhNo.Text, Txt_Email.Text, Txt_StaffId.Text, Txt_EduQuli.Text, int.Parse(Rdb_Login.SelectedValue.ToString()), int.Parse(Drp_SelectRole.SelectedValue.ToString()), Txt_Dob.Text, int.Parse(Session["StaffId"].ToString()), int.Parse(Drp_Group.SelectedValue), Txt_Aadhar.Text, Txt_PAN.Text);
            if (Txt_Email.Text != "")
            {
                MyStaffMang.CreateTansationDb();
                MyStaffMang.InsertStaffEmailIdIntoEmailStaffList(_StaffId, Txt_Email.Text.Trim());
                MyStaffMang.EndSucessTansationDb();
            }
            if (MyUser.HaveModule(29))
            {
                string _EmpId = TxtEmpId.Text.ToString();
                string _Name = Txt_StaffName.Text.Trim();
                string _address = Txt_Address.Text.Trim();

                try
                {
                    MyStaffMang.CreateTansationDb();
                    if (RdbYes.Checked)
                    {
                        string _Pan = TxtPan.Text.ToString();
                        string _BankName = TxtBankName.Text.Trim();
                        string _Acc = TxtAcc.Text.ToString();
                        string _Designation = Drp_SelectRole.SelectedItem.Text;
                        int _PayrollId = int.Parse(DrpPayroll.SelectedValue.ToString());
                        if (_PayrollId > 0)
                        {
                            if (!MyStaffMang.PanNumberValid(_Pan))
                            {
                                WC_MessageBox.ShowMssage("Enter Pan number");
                                MyStaffMang.EndFailTansationDb();
                                _StaffId = -1;
                            }
                            else if (!MyStaffMang.BankNameValid(_BankName))
                            {
                                WC_MessageBox.ShowMssage("Enter Bank Name");
                                MyStaffMang.EndFailTansationDb();
                                _StaffId = -1;
                            }
                            else if (!MyStaffMang.BankAccountNumValid(_Acc))
                            {
                                WC_MessageBox.ShowMssage("Enter Account number");
                                MyStaffMang.EndFailTansationDb();
                                _StaffId = -1;
                            }
                            else
                            {
                                if (_Acc == "")
                                {
                                    _Acc = "0";
                                }
                                MyReader = MyStaffMang.GetBasicPay(_PayrollId);
                                if (MyReader.HasRows)
                                {
                                    _BasicPay = double.Parse(MyReader.GetValue(0).ToString());
                                    _Gross = _BasicPay;
                                    _NetPay = _Gross;
                                }


                                if (NotEmpIDPresent(_EmpId))
                                {
                                    MyReader = MyStaffMang.GetHeadId(_PayrollId);
                                    if (MyReader.HasRows)
                                    {
                                        while (MyReader.Read())
                                        {
                                            int _HeadId = int.Parse(MyReader.GetValue(0).ToString());
                                            MyStaffMang.AddEmpHeadMap(_HeadId, _EmpId, _PayrollId);
                                        }
                                    }

                                    MyStaffMang.AddPayrollEmp(_Name, _address, _EmpId, _Pan, _BankName, _Acc, _PayrollId, _Designation, _BasicPay, _Gross, _NetPay, _StaffId);

                                    MyStaffMang.EndSucessTansationDb();

                                }
                                else
                                {
                                    if (TxtEmpId.Enabled == false)
                                    {

                                        int payrolltype = 0;
                                        payrolltype = MyStaffMang.GetpayrollType(_EmpId);
                                        if (payrolltype > 0)
                                        {
                                            if (payrolltype != int.Parse(DrpPayroll.SelectedValue))
                                            {
                                                m_empheadreader = MyStaffMang.GetHeadIdOfEmp(_EmpId);
                                                if (m_empheadreader.HasRows)
                                                {
                                                    while (m_empheadreader.Read())
                                                    {
                                                        int headId = int.Parse(m_empheadreader.GetValue(0).ToString());
                                                        MyStaffMang.DeleteEmpHeadMap(_EmpId, headId);
                                                    }
                                                }

                                                MyReader = MyStaffMang.GetHeadId(_PayrollId);
                                                if (MyReader.HasRows)
                                                {
                                                    while (MyReader.Read())
                                                    {
                                                        int _HeadId = int.Parse(MyReader.GetValue(0).ToString());
                                                        MyStaffMang.AddEmpHeadMap(_HeadId, _EmpId, _PayrollId);
                                                    }
                                                }
                                            }
                                        }
                                        MyStaffMang.UpdatePayrollEmp(_Name, _address, _EmpId, _Pan, _BankName, _Acc, _PayrollId, _Designation, _BasicPay, _Gross, _NetPay, _StaffId);
                                        MyStaffMang.EndSucessTansationDb();
                                    }

                                    else
                                    {


                                        WC_MessageBox.ShowMssage("Employee ID already present, Please enter another ID");
                                        MyStaffMang.EndFailTansationDb();
                                        _StaffId = -1;
                                    }

                                }
                            }
                        }
                        else
                        {
                            WC_MessageBox.ShowMssage("Please select a payroll type");
                            MyStaffMang.EndFailTansationDb();
                            _StaffId = -1;
                        }
                    }
                    else if (RdbNo.Checked)
                    {

                        if (!NotEmpIDPresent(_EmpId))
                        {

                            MyStaffMang.UpdateInactivePayrollEmp(_EmpId, _StaffId, _Name, _address);

                        }
                        MyStaffMang.EndSucessTansationDb();
                        //else
                        //{
                        //    MyStaffMang.EndFailTansationDb();
                        //    _StaffId = -1;
                        //}
                    }
                }
                catch
                {
                    WC_MessageBox.ShowMssage("Please refresh the page and try again...");
                    MyStaffMang.EndFailTansationDb();
                    _StaffId = -1;

                }
                //}





            }
            if (_StaffId != -1)
            {
                Lbl_Failue.Text = "Staff is Updated";
                MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Update Staff Details", "The details Of Staff " + Txt_StaffName.Text + " is updated", 1,1);

                if (MyUser.HaveModule(3))
                {
                    AddStaffSubjects(_StaffId);
                }
            }
            else
            {
                Lbl_Failue.Text = "Staff is not updated. Please try again";

            }
        }


    }

    public bool LoginAlreadyAllowed()
    {
        bool _valid = false;
        string sql = "select count(tbluser.Id) from tbluser where  tbluser.RoleId<>1 and tbluser.`status`=1 and tbluser.CanLogin=1 and tbluser.Id=" + Session["StaffId"].ToString();
         MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            int _Count = 0;
            int.TryParse(MyReader.GetValue(0).ToString(), out  _Count);
            if (_Count > 0)
            {
                _valid = true;
            }
        }
        return _valid;
    }


    private bool notpresent()
    {

        bool NotPresent = true;
        string sql = "select * from tblpay_employee where tblpay_employee.StaffId = " + Session["StaffId"].ToString() + "";
        MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            NotPresent = false;
        }
        return NotPresent;
    }

    private bool NotEmpIDPresent(string _EmpId)
    {
        bool NotPresent = true;
        string sql = "select * from tblpay_employee where tblpay_employee.EmpId = '" + _EmpId + "'";
        MyReader = MyStaffMang.m_TransationDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            NotPresent = false;
        } 
        return NotPresent;

    }


    protected void UpdateOk_Click(object sender, EventArgs e)
    {
        Response.Redirect("StaffDetails.aspx");
    }
  
    private bool CheckDate(out string _message)
    {
        bool _valid = true;
        _message = "";
        //string _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        DateTime Today = DateTime.Now;
        //DateTime Today =MyUser.GetDareFromText(_strdtNow);

        //DateTime Date_Birth = DateTime.Parse(Txt_Dob.Text);
        DateTime Date_Birth = MyUser.GetDareFromText(Txt_Dob.Text);

//        DateTime JoiningDate = DateTime.Parse(Txt_JoiningDate.Text);
        DateTime JoiningDate = MyUser.GetDareFromText(Txt_JoiningDate.Text);

        int Year = Today.Year;
        int Dob_Year = Date_Birth.Year;
        if (Date_Birth > Today)
        {
            _message = "Date of birth is invalid";
            _valid = false;
        }
        else if (Dob_Year >= Year)
        {
            _message = "Date of birth is invalid";
            _valid = false;
        }
        else if (JoiningDate > Today)
        {
            _message = "Date of joining cannot be greater than today";
            _valid = false;
        }
        else if (Date_Birth >= JoiningDate)
        {
            _message = "Date of joining cannot be Less than Date of birth";
            _valid = false;
        }
        return _valid;
    }
  
    private void AddStaffSubjects(int _StaffId)
    {
        MyStaffMang.DeleteCurrentSubjects(_StaffId);
        for (int i = 0; i < ChkBox_AssSub.Items.Count; i++)
        {
            MyStaffMang.AddSubjectsToStaff(int.Parse(ChkBox_AssSub.Items[i].Value.ToString()), _StaffId);
        }
    }

    private bool CompareStaffId()
    {
        bool StId = false;
        if(Txt_StaffId.Text==Session["StaffId"].ToString())
        {
            StId = true;
        }
        return StId;
    }

    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("StaffDetails.aspx");
    }

    # region Upload
    protected void Btn_Upload_Click(object sender, EventArgs e)
    {
        if (FileUp_Photo.PostedFile != null && !ValidImageFile())
        {
            lbl_Upmessage.Text = "File type cannot be uploaded";
           
        }
        else if (FileUp_Photo.PostedFile == null)
        {
            lbl_Upmessage.Text = "Select an image to upload";
           
        }
        else
        {
            int _StaffId = int.Parse(Session["StaffId"].ToString());
            if (AddPhoto(_StaffId))
            {
                lbl_Upmessage.Text = "Image Uploaded";
                LoaduserTopData();
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Change Staff Photo", "The Photo  Of Staff is changed", 1);
            }
            else
            {
                lbl_Upmessage.Text = "Unable to upload  files please try again later";
                LoaduserTopData();
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Change Staff Photo", "Changing photo uploading is failed", 1);
            }
        }
    }

    protected void Btn_UPLoadCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("StaffDetails.aspx");
    }
    private bool AddPhoto(int _StaffId)
    {
        bool success = false;
        int UsrId;

        string ImageUrl = "";                     
        string ImageName = FileUp_Photo.FileName.ToString();

        string path=Server.MapPath("TemporaryFileManager");
        FileUp_Photo.SaveAs(path+"\\" + ImageName);
        // string strVirtualPath="http://localhost:1334/WinSchool/UpImage/" + ImageName;

        string ThumbnailPath = (path + "\\" + "Staff" + _StaffId.ToString() + ImageName);
        using (System.Drawing.Image Img = System.Drawing.Image.FromFile(path + "\\" + ImageName))
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
        ImageUrl = "Staff" + _StaffId + ImageName;

        UsrId = _StaffId;

       
      

          if (MyStaffMang.HasImage(UsrId))
          {
              
              byte[] imagebytes  = General.getImageinBytefromImage(ThumbnailPath);

              ImageUploaderClass imgUpload = new ImageUploaderClass(objSchool);

              success= imgUpload.UpdateImageFile(imagebytes, _StaffId, "StaffImage");
             
          }
          else
          {
              byte[] imagebytes = General.getImageinBytefromImage(ThumbnailPath);

              ImageUploaderClass imgUpload = new ImageUploaderClass(objSchool);

              success= imgUpload.InsertImageFile(imagebytes, _StaffId, "StaffImage");            
          }
         
      
        
        if (ImageName != "")
        {
            File.Delete(path + "\\" + ImageName);
            File.Delete(path+"\\"+ "Staff" + _StaffId.ToString() + ImageName);
        }
        return success;



    }
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



    private bool ValidImageFile()
    {
        bool fileOK = false;
        string fileExtension = System.IO.Path.GetExtension(FileUp_Photo.FileName).ToLower();
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
    
    # endregion

    protected void RdbYes_CheckedChanged(object sender, EventArgs e)
    {

        FillDrpPayroll();
        PnlPayrollDetails.Visible = true;
    }

    private void FillDrpPayroll()
    {
        DataSet MydataSet = new DataSet();
        int payrolltype=0;
        MydataSet = null;
        DrpPayroll.Items.Clear();
        MydataSet = MyStaffMang.FillDrp();

        if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
        {
            ListItem li;
            li = new ListItem("Select Category", "0");
            DrpPayroll.Items.Add(li);
            foreach (DataRow Dr_Cat in MydataSet.Tables[0].Rows)
            {
                li = new ListItem(Dr_Cat[1].ToString(), Dr_Cat[0].ToString());
                DrpPayroll.Items.Add(li);
            }
        }
        else
        {
            ListItem li = new ListItem("No Category Found", "-1");
            DrpPayroll.Items.Add(li);
        }
        if(payrollTypeExist(out payrolltype))
        {
            DrpPayroll.SelectedValue = payrolltype.ToString();
        }

    }

    private bool payrollTypeExist(out int payrolltype)
    {
        string  empid = TxtEmpId.Text;
        payrolltype = 0;
        bool exist = false;
        string sql = "select PayrollType from tblpay_employee where tblpay_employee.EmpId = '" + empid + "'";
        MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            exist = true;
            payrolltype = int.Parse(MyReader.GetValue(0).ToString());
        } 
        return exist;
      
    }

    protected void RdbNo_CheckedChanged(object sender, EventArgs e)
    {
        PnlPayrollDetails.Visible = false;

    }

    protected void Lnk_Cam_Click(object sender, EventArgs e)
    {
        Session["SaveType"] = "StaffId";
        ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "window.open('silverlitepicturecapture.aspx','','width=700,height=500')", true);
    }


}
