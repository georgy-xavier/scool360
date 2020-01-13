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
using WinEr;
public partial class CreateStaff : System.Web.UI.Page
{
    private Incident MyIncedent; 
    private StaffManager MyStaffMang;
    private Payroll Mypay;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    private DataSet MydataSet = null;
    private SchoolClass objSchool = null;
    protected void Page_PreInit(Object sender, EventArgs e)
    {
        
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        else
        {
            if (WinerUtlity.NeedCentrelDB())
            {
                if (Session[WinerConstants.SessionSchool] == null)
                {
                    Response.Redirect("Logout.aspx");
                }
                objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
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
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {

        /*PostBackOptions optionsSubmitAsync = new PostBackOptions(Btn_Create);

        Btn_Create.OnClientClick = "disableButtonOnClick(this, 'Please wait...', 'disabled_button'); ";

        Btn_Create.OnClientClick += ClientScript.GetPostBackEventReference(optionsSubmitAsync);*/

    }
    protected void Page_init(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyStaffMang = MyUser.GetStaffObj();
        MyIncedent = MyUser.GetIncedentObj();
        if (MyStaffMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }
        else if (!MyUser.HaveActionRignt(18))
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
                }
                else
                {
                    PnlPayrollYesNo.Visible = false;
                }

               // Panel3.Visible = false;
                AddSubjectToChkList();
                AddRoleToDrpList(0);
                AddpParentGrpToDrpList();
                if (MyUser.HaveModule(3))
                {
                    Pnl_Subjects.Visible = true;
                }
                else
                {
                    Pnl_Subjects.Visible = false;
                }
                Txt_StaffName.Focus();
                //some initlization
            }
        }
    }
    private void AddpParentGrpToDrpList()
    {
        Drp_ParentGroup.Items.Clear();
        DataSet myDataset;
        myDataset = MyUser.MyAssociatedGroups();
        if (myDataset != null && myDataset.Tables != null && myDataset.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr in myDataset.Tables[0].Rows)
            {

                ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                Drp_ParentGroup.Items.Add(li);

            }

        }
        else
        {

            ListItem li = new ListItem("No Groups", "-1");
            Drp_ParentGroup.Items.Add(li);

        }
        Drp_ParentGroup.SelectedIndex = 0;
    }
    private void AddRoleToDrpList(int _intex)
    {
        Drp_SelectRole.Items.Clear();
            string sql = "SELECT Id,RoleName FROM tblrole WHERE Type='Staff'";
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {

                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_SelectRole.Items.Add(li);

                }
                Drp_SelectRole.SelectedIndex = _intex;
            }
            else
            {
                Drp_SelectRole.Items.Add(new ListItem("No role found","-1"));
                Btn_Create.Enabled = false;
                WC_MessageBox.ShowMssage("Please create role before entering staff");
               

            }

    }
    private void AddSubjectToChkList()
    {

        ChkBox_AllsSub.Items.Clear();

        string sql = "SELECT Id,subject_name FROM tblsubjects";
        MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                ChkBox_AllsSub.Items.Add(li);
            }        
        }
    }
    protected void Rdb_Login_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Rdb_Login.SelectedIndex == 0)
        {
            Panel2.Visible = false;
            Drp_SelectRole.Focus();
        }
        else
        {
            Panel2.Visible = true;
            Txt_PassWord.Focus();
        }
    }
    protected void Btn_Add_Click(object sender, EventArgs e)
    {
        Lbl_Failue.Text = "";
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
        Lbl_Failue.Text = "";
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
    protected void Btn_Create_Click(object sender, EventArgs e)
    {
        double _BasicPay = 0;
        double _Gross = 0;
        double _NetPay = 0;
        bool IsModulePresent = false,IsPayrollModulePresent=false;
        if (MyUser.HaveModule(3))
        {
            IsModulePresent = true;
        }

        if (MyUser.HaveModule(29))
        {
            IsPayrollModulePresent = true;
        }
           


       // Btn_Create.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(Btn_Create, "").ToString());
        try
        {
           
            Btn_Create.Enabled = false;
            string _message;
            float _ExperienceYears = 0;
            if (Txt_Experience.Text != "")
            {
                _ExperienceYears = float.Parse(Txt_Experience.Text.Trim().ToString());
            }
            if (Rdb_Login.SelectedIndex == 1&& MyUser.m_LicenseObject.m_usercount != -1 && MyUser.UserCountExceeds())
            {
                WC_MessageBox.ShowMssage("Cannot create more login users.The number of users for your license is only : " + MyUser.m_LicenseObject.m_usercount);
                
                Btn_Create.Enabled = true;
            }
            else if (Txt_StaffName.Text.Trim() == "" || Txt_JoiningDate.Text.Trim() == "" || Txt_Address.Text.Trim() == ""  || Txt_Dob.Text.Trim() == "" || Txt_StaffId.Text.Trim() == "")//|| (Rdb_Login.SelectedIndex == 1 && Txt_StaffId.Text.Trim() == ""))
            {
                WC_MessageBox.ShowMssage("One Or more fields are empty");
               
                Btn_Create.Enabled = true;
            }

            else if (!MyStaffMang.roleExists(int.Parse(Drp_SelectRole.SelectedValue.ToString())))
            {
                WC_MessageBox.ShowMssage("The selected role no longer exists. Please refresh your page");
               
                Btn_Create.Enabled = true;
            }
            else if (Rdb_Login.SelectedIndex == 1 && (Txt_PassWord.Text.Trim() == "" || Txt_PassWord.Text != Txt_Confirm.Text))
            {
                WC_MessageBox.ShowMssage("Re-enter password...");
                
                Btn_Create.Enabled = true;
            }
            else if (Rdb_Login.SelectedIndex == 1 && !MyStaffMang.IsValidStaffId(Txt_StaffId.Text))
            {
                WC_MessageBox.ShowMssage("Login Name Allready exists");
                
                Btn_Create.Enabled = true;
            }
            else if (Rdb_Login.SelectedIndex == 0 && Txt_StaffId.Text != "" && !MyStaffMang.IsValidStaffId(Txt_StaffId.Text))
            {
                WC_MessageBox.ShowMssage("StaffId Already exists");
               
                Btn_Create.Enabled = true;
            }
            else if (Drp_ParentGroup.SelectedValue == "-1")
            {
                WC_MessageBox.ShowMssage("Staff must need a Group");
             
                Btn_Create.Enabled = true;
            }
            else if (!CheckDate(out _message))
            {
                WC_MessageBox.ShowMssage(_message);
              
                Btn_Create.Enabled = true;
            }
            
            else
            {
                MyStaffMang.CreateTansationDb();
                int _StaffId;
                _StaffId = MyStaffMang.CreateStaff(Txt_StaffName.Text, Txt_PassWord.Text, Txt_JoiningDate.Text, Txt_Address.Text, RbLst_Sex.SelectedValue.ToString(), _ExperienceYears, Txt_Desig.Text, Txt_PhNo.Text, Txt_Email.Text, Txt_StaffId.Text, Txt_EduQuli.Text, int.Parse(Rdb_Login.SelectedValue.ToString()), int.Parse(Drp_SelectRole.SelectedValue.ToString()), Txt_Dob.Text, Drp_ParentGroup.SelectedValue.ToString(), Txt_Aadhar.Text, Txt_Pan.Text, out _message);
                if (_StaffId != -1)
                {
                    if (IsModulePresent)
                    {
                        AddStaffSubjects(_StaffId);
                    }
                    if (Txt_Email.Text != "")
                    {
                        MyStaffMang.InsertStaffEmailIdIntoEmailStaffList(_StaffId, Txt_Email.Text.Trim());
                    }
                 
                    if (IsPayrollModulePresent)
                    {
                        if (RdbYes.Checked)
                        {

                            string _Name = Txt_StaffName.Text.Trim();
                            string _address = Txt_Address.Text.Trim();
                            string _EmpId = TxtEmpId.Text.ToString();
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
                                    Btn_Create.Enabled = true;

                                }
                                else if (!MyStaffMang.BankNameValid(_BankName))
                                {
                                    WC_MessageBox.ShowMssage("Enter Bank Name");
                                    MyStaffMang.EndFailTansationDb();
                                    Btn_Create.Enabled = true;
                                }
                                else if (!MyStaffMang.BankAccountNumValid(_Acc))
                                {
                                    WC_MessageBox.ShowMssage("Enter Account number");
                                    MyStaffMang.EndFailTansationDb();
                                    Btn_Create.Enabled = true;
                                }
                                else
                                {
                                    MyReader = MyStaffMang.GetBasicPay(_PayrollId);
                                    if (MyReader.HasRows)
                                    {
                                        _BasicPay = double.Parse(MyReader.GetValue(0).ToString());
                                        _Gross = _BasicPay;
                                        _NetPay = _Gross;
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

                                    if (NotEmpIDPresent(_EmpId))
                                    {
                                        if (_Acc == "")
                                        {
                                            _Acc = "0";
                                        }
                                        MyStaffMang.AddPayrollEmp(_Name, _address, _EmpId, _Pan, _BankName, _Acc, _PayrollId, _Designation, _BasicPay, _Gross, _NetPay, _StaffId);

                                        MyStaffMang.EndSucessTansationDb();
                                        Txt_UserId.Text = _StaffId.ToString();
                                        this.MPE_LastMessage.Show();
                                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create Staff", " Create Staff " + Txt_StaffName.Text, 1);
                                        MyIncedent.CreateApprovedIncedent("Staff Created", "Staff " + _Name + " is added  on " + General.GerFormatedDatVal(DateTime.Now), General.GerFormatedDatVal(DateTime.Now), 4, MyUser.UserId, "staff", _StaffId, 34, 0, MyUser.CurrentBatchId, 0);
                                        AddSubjectToChkList();
                                    }
                                    else
                                    {
                                        WC_MessageBox.ShowMssage("Employee ID already present, Please enter another ID");
                                        MyStaffMang.EndFailTansationDb();
                                        Btn_Create.Enabled = true;

                                    }
                                }
                            }
                            else
                            {
                                WC_MessageBox.ShowMssage("Please select a payroll type");
                                MyStaffMang.EndFailTansationDb();
                                Btn_Create.Enabled = true;
                            }
                        }
                        else
                        {
                            MyStaffMang.EndSucessTansationDb();
                            Txt_UserId.Text = _StaffId.ToString();
                            this.MPE_LastMessage.Show();
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create Staff", " Create Staff " + Txt_StaffName.Text, 1);
                            MyIncedent.CreateApprovedIncedent("Staff Created", "Staff " + Txt_StaffName.Text + " is added  on " + General.GerFormatedDatVal(DateTime.Now), General.GerFormatedDatVal(DateTime.Now), 4, MyUser.UserId, "staff", _StaffId, 34, 0, MyUser.CurrentBatchId, 0);
                            AddSubjectToChkList();
                        }
                    }
                    else
                    {
                        MyStaffMang.EndSucessTansationDb();
                        Txt_UserId.Text = _StaffId.ToString();
                        this.MPE_LastMessage.Show();
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create Staff", " Create Staff " + Txt_StaffName.Text, 1);
                        MyIncedent.CreateApprovedIncedent("Staff Created", "Staff " + Txt_StaffName.Text + " is added  on " + General.GerFormatedDatVal(DateTime.Now), General.GerFormatedDatVal(DateTime.Now), 4, MyUser.UserId, "staff", _StaffId, 34, 0, MyUser.CurrentBatchId, 0);
                        AddSubjectToChkList();
                    }
                    
                  
                }
                else
                {
                    WC_MessageBox.ShowMssage("Staff is not created. Please try again");
                    Btn_Create.Enabled = true;
                    MyStaffMang.EndFailTansationDb();
                }

            }
           // Btn_Create.Enabled = true;
            
        }
        catch
        {
            
            WC_MessageBox.ShowMssage("Please refresh the page and try again...");
            Btn_Create.Enabled = true;
            MyStaffMang.EndFailTansationDb();
        }
    }

   
    private bool NotEmpIDPresent(string _EmpId)
    {
        bool NotPresent = true;
        string sql = "select * from tblpay_employee where tblpay_employee.EmpId ='" + _EmpId + "'";
        MyReader = MyStaffMang.m_TransationDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            NotPresent = false;
        }
        return NotPresent;
        
    }

    

    //#region Attendance

    //private void Mark_AbsentFor_PreviousDays(int _StaffId)
    //{

    //    DataSet _attendancedays = new DataSet();
    //    _attendancedays = FindAttendanceDays();
    //    if (_attendancedays.Tables[0].Rows.Count > 0)
    //    {
    //        string Sql;
    //        foreach (DataRow dr in _attendancedays.Tables[0].Rows)
    //        {
    //            Sql = "insert into tblstaffattendance(StaffId,DayId)VALUES("+_StaffId+","+int.Parse(dr[0].ToString())+")";
    //            MyStaffMang.m_TransationDb.ExecuteQuery(Sql);
    //            //MyStaffMang.m_MysqlDb.ExecuteQuery(Sql);

    //        }
    //    }
    //}

    //private DataSet FindAttendanceDays()
    //{
    //    DataSet _attendancedays = new DataSet();
    //    string Sql = "select distinct tbldate.Id FROM tbldate where tbldate.Status='staff' and tbldate.classId=0";
    //    _attendancedays = MyStaffMang.m_MysqlDb.ExecuteQueryReturnDataSet(Sql);
    //    return _attendancedays;
    //}
    //#endregion 

    protected void Btn_Yes_Click(object sender, EventArgs e)
    {
        MPE_UploadPhoto.Show();
    }
    protected void Btn_No_Click(object sender, EventArgs e)
    {
        Response.Redirect("CreateStaff.aspx");
    }

    protected void Btn_GoToStaff_Click(object sender, EventArgs e)
    {
        Session["StudId"] = int.Parse(Txt_UserId.Text.ToString());
        Response.Redirect("StaffDetails.aspx");
    }
    private bool CheckDate(out string _message)
    {
        bool _valid = true;
        _message = "";
        string _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        DateTime Today = DateTime.Now;
        //DateTime Date_Birth = DateTime.Parse(Txt_Dob.Text);
        //DateTime JoiningDate = DateTime.Parse(Txt_JoiningDate.Text);

        DateTime Date_Birth = MyUser.GetDareFromText(Txt_Dob.Text);
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
        else if (JoiningDate >Today)
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
        for (int i = 0; i < ChkBox_AssSub.Items.Count; i++)
        {
            MyStaffMang.AddSubjectsToStaff(int.Parse(ChkBox_AssSub.Items[i].Value.ToString()), _StaffId);
        }
    }
    protected void Btn_UploadStaff_Click(object sender, EventArgs e)
    {
        if (Txt_UserId.Text.Trim() == "")
        {
            WC_MessageBox.ShowMssage("Please try again later");
        }
        else if (FileUp_User.PostedFile == null)
        {
            //Lbl_msg.Text = "Select an image to upload";
           // this.MPE_MessageBox.Show();
            Lbl_UpMessage.Text = "Select an image to upload";
            MPE_UploadPhoto.Show();
        }
        else if (FileUp_User.PostedFile != null && !ValidImageFile())
        {

           // Lbl_msg.Text = "File type cannot be uploaded";
            //this.MPE_MessageBox.Show();
            Lbl_UpMessage.Text = "File type cannot be uploaded";
            MPE_UploadPhoto.Show();
        }

        else
        {
            AddPhoto(int.Parse(Txt_UserId.Text.ToString()));
            WC_MessageBox.ShowMssage("Photo Uploaded");
         
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Staff Photo is Uploaded", "Photo of staff is uploaded ", 1);
            
            Panel1.Visible = true;
            //Panel4.Visible = true;
            //Panel3.Visible = false;
            ClearStaff();
            ClearSubjects();
            Btn_Create.Enabled = true;
            //Response.Redirect("");
        }
    }
   private void ClearSubjects()
    {
        for (int i = 0; i < ChkBox_AssSub.Items.Count; i++)
        {
           
                ChkBox_AssSub.Items.Remove(ChkBox_AssSub.Items[i]);
                i--;


        }
    }

    private void ClearStaff()
    {
        Txt_StaffName.Text = "";
        Txt_Dob.Text = "";
        Txt_Address.Text = "";
        Txt_JoiningDate.Text = "";
        Txt_Experience.Text = "";
       
        Txt_Desig.Text = "";
        Txt_PhNo.Text = "";
        Txt_Email.Text = "";
        Txt_StaffId.Text = "";
        Txt_EduQuli.Text = "";
        Txt_UserId.Text = "";
        

    }
    

    private void AddPhoto(int _StaffId)
    {
        string ImageUrl = "";
       
        string ImageName = FileUp_User.FileName.ToString();
        string path = Server.MapPath("TemporaryFileManager");
        FileUp_User.SaveAs(path + "\\" + ImageName);

        string ThumbnailPath = (path + "\\" + "Staff" + _StaffId.ToString() + ImageName);
        using (System.Drawing.Image Img = System.Drawing.Image.FromFile(path + "\\" + ImageName))
        {
            Size ThumbNailSize = NewImageSize(Img.Height, Img.Width, 150);
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



       

        try
        {
            if (ImageName != "")
            {
                byte[] imagebytes = General.getImageinBytefromImage(ThumbnailPath);

                ImageUploaderClass imgUpload = new ImageUploaderClass(objSchool);

                imgUpload.InsertImageFile(imagebytes, _StaffId, "StaffImage");

                File.Delete(path + "\\" + ImageName);
                File.Delete(path + "\\" + "Staff" + _StaffId.ToString() + ImageName);
            }
        }
        catch
        {

        }

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
        string fileExtension = System.IO.Path.GetExtension(FileUp_User.FileName).ToLower();
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
    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {

       
        Response.Redirect("CreateStaff.aspx");

    }
    protected void Buttn_Cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewStaffs.aspx");
    }
    protected void Btn_Reset_Click(object sender, EventArgs e)
    {
        Response.Redirect("CreateStaff.aspx");
    }

    protected void RdbYes_CheckedChanged(object sender, EventArgs e)
    {
        
        FillDrpPayroll();
        PnlPayrollDetails.Visible = true;
    }

    private void FillDrpPayroll()
    {
        
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
        
       
    }

    protected void RdbNo_CheckedChanged(object sender, EventArgs e)
    {
        PnlPayrollDetails.Visible = false;

    }
}
