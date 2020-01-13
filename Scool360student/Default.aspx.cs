using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WinBase;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Data.Odbc;
using WinEr;
using System.Web.Services;


public partial class _Default : System.Web.UI.Page
{

    private SchoolClass objSchool = null;

    protected void Page_Load(object sender, EventArgs e)
    {
       

        if (WinerUtlity.NeedCentrelDB())
        {
            if (Request.QueryString[WinerConstants.SchoolId] != null)
            {
                int intSchoolId;
                if (int.TryParse(Request.QueryString[WinerConstants.SchoolId], out intSchoolId))
                {
                    objSchool = WinerUtlity.GetSchoolObject(intSchoolId);
                    if (objSchool != null)
                        Session[WinerConstants.SessionSchool] = objSchool;
                }

            }
            if (Session[WinerConstants.SessionSchool] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];

        }
        if (!IsPostBack)
        {
            LoadDocumentCookie(objSchool.SchoolId);
            Page.Title = objSchool.SchoolName.ToString();
         
        }

    }


    private void CheckExpiryDate()
    {
        string Expiremsg = "";
       
        int days = 0;
        bool Unlimited = false;
        OdbcConnection _myOdbcConn = null;
        string m_stConnection = WinerUtlity.GetConnectionString(objSchool);
        _myOdbcConn = new OdbcConnection(m_stConnection);
        _myOdbcConn.Open();
        string sql = "SELECT tblschooldetails.ExpireDate from tblschooldetails";
        OdbcCommand m_cmd = new OdbcCommand(sql, _myOdbcConn);
        OdbcDataReader _myReader = m_cmd.ExecuteReader();
        if (_myReader.HasRows)
        {
            if (_myReader.GetValue(0).ToString() != "")
            {
                DateTime ExpireDate = Convert.ToDateTime(_myReader.GetValue(0).ToString());
                if (ExpireDate.Date.Equals(DateTime.MaxValue.Date))
                    Unlimited = true;
                else
                {
                    ExpireDate = ExpireDate.Date;
                    DateTime CurrentDate = DateTime.Now.Date;
                    TimeSpan span = ExpireDate.Subtract(CurrentDate);
                    days = (int)span.TotalDays;
                }


                if (days <= 30 && days > 0)
                    Expiremsg = "Your licence will expire within " + days + " days.";
                else if (days > 30 || Unlimited == true)
                    Expiremsg = "";
                else if (days == 0)
                    Expiremsg = "Your licence will expire on today.";
                else
                    Expiremsg = "Your licence is expired.Please update your licence.";
            }
            else
                Expiremsg = "Your license was not updated .";




            if (Expiremsg != "")
            {
                //warningarea.Style.Add("display", "block");
                //createSnackbar(shortMessage, 'Dismiss');
                //  Lbl_waringmsg.Text = Expiremsg;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "createSnackbar(" + Expiremsg + ", 'Dismiss')", true);
            }

        }
        _myReader.Close();


    }

    private void LoadDocumentCookie(int intSchoolId)
    {
        //string _InnerHtml = " <script type=\"text/javascript\"> document.cookie = \"0$#$" + intSchoolId + "\";    </script>";
        string _InnerHtml = " <script type=\"text/javascript\">document.cookie = \"0$#$" + intSchoolId.ToString() + "\";document.cookie = \"WIN#SD=" + intSchoolId.ToString() + ";\";document.cookie = \"WIN#SD#$APP=" + (intSchoolId * 2048).ToString() + ";\"; </script>";//firstone for session and second for mobile app(*by 2048)
        this.javascriptId.InnerHtml = _InnerHtml;
    }
    

    //private void LoadDetails()
    //{
    //     string SchoolDetails1="";
    //     string FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));

    //    try
    //    {

    //        OdbcConnection _myOdbcConn = null;
    //        string m_stConnection = WinerUtlity.GetConnectionString(objSchool);
    //        _myOdbcConn = new OdbcConnection(m_stConnection);
    //        _myOdbcConn.Open();
    //        string sql = "select SchoolName,SchoolImage from tblschooldetails";
    //        OdbcCommand m_cmd = new OdbcCommand(sql, _myOdbcConn);
    //        OdbcDataReader _myReader = m_cmd.ExecuteReader();

    //        if(_myReader.HasRows)
    //        {
    //            if (_myReader.GetValue(0).ToString() != "")
    //            {


    //                SchoolDetails1 = _myReader.GetValue(0).ToString();
    //                ImgName.InnerHtml = SchoolDetails1;
    //                Page.Title = objSchool.SchoolName.ToString();
    //            }
    //            else
    //            {

    //                SchoolDetails1 = ConfigurationSettings.AppSettings["SchoolName"];

    //                ImgName.InnerHtml = SchoolDetails1;

    //            }

    //                ImageUploaderClass imgobj = new ImageUploaderClass(objSchool);


    //                //string Mimag1 = "<img src=\"" + "Handler/ImageReturnHandler.ashx?id=" + objSchool.SchoolId + "&type=SchoolImage" + "\" width=\"90%\" class=\"img-thumbnail\" alt=\"\" />";
    //               // string Mimag1 = "<div><img src=\"" + "Handler/ImageReturnHandler.ashx?id=" + objSchool.SchoolId + "&type=SchoolImage" + "\" class=\"img-responsive\" alt=\"\" /></div>";
    //             //   Mimage.InnerHtml = Mimag1;

    //                string Mimag = "<img src=\"" + "Handler/ImageReturnHandler.ashx?id=" + objSchool.SchoolId + "&type=Logo" + "\" width=\"90%\" alt=\"\" />";

    //                mlogo.InnerHtml = Mimag;


    //        }
    //        else
    //        {
    //             SchoolDetails1= ConfigurationSettings.AppSettings["SchoolName"];

    //             ImgName.InnerHtml = SchoolDetails1;
    //        }
    //    }
    //    catch 
    //    {  
    //        SchoolDetails1 = ConfigurationSettings.AppSettings["SchoolName"];

    //        ImgName.InnerHtml = SchoolDetails1;
    //    }
    //}

    protected void Client_Login_Authenticate(object sender, AuthenticateEventArgs e)
    {

        string FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));

        KnowinUser UserObj;
        string _meassageError;
        Session["RsltQry"] = null;
        if (Session["UserObj"] == null)
        {

            UserObj = new KnowinUser(WinerUtlity.GetConnectionString(objSchool), FilePath, WinerUtlity.GetRelativeFilePath(objSchool));
            Session["UserObj"] = UserObj;
        }
        else
        {

            UserObj = (KnowinUser)Session["UserObj"];
            if (UserObj.UserName != Client_Login.UserName.Trim())
            {

                UserObj = new KnowinUser(WinerUtlity.GetConnectionString(objSchool), FilePath, WinerUtlity.GetRelativeFilePath(objSchool));
                Session["UserObj"] = UserObj;
            }
        }



        if (UserObj.LoginUserStudent(Client_Login.UserName.ToString(), Client_Login.Password.ToString(), 0, out _meassageError))
        {
            UserObj.RecordLogin(Client_Login.UserName.ToString());
            //UserObj.RecordLogin(Client_Login.UserName.ToString());
            if (ConfigurationSettings.AppSettings["NeedAppicationObject"] == "1")
            {
                SetApplicationUserObject(UserObj);
            }
            Response.Redirect(_meassageError);
        }
        else
        {
            Client_Login.FailureText = _meassageError;


        }
    }

    # region Application Object Area Fee
    private void SetApplicationUserObject(KnowinUser UserObj)
    {
        List<UserInfoClass> _UaerList = new List<UserInfoClass>();
        if (Application["UserList"] == null)
        {
            Application.Lock();
            Application["UserList"] = _UaerList;
            Application.UnLock();
        }
        int SchoolId = WinerUtlity.GetSchoolId(objSchool);
        _UaerList = (List<UserInfoClass>)Application["UserList"];
        int _UserIndex = WinerUtlity.GetUserIndex(UserObj.UserId, SchoolId, _UaerList);
        if (_UserIndex != -1)
        {
            _UaerList[_UserIndex].UserName = UserObj.UserName;
            _UaerList[_UserIndex].Allowlogin = true;
            _UaerList[_UserIndex].LoginTime = DateTime.Now;
            _UaerList[_UserIndex].NeedSessionOut = false;
            _UaerList[_UserIndex].IP = Request.UserHostAddress;
            _UaerList[_UserIndex].MachineName = Request.UserHostName;
        }
        else
        {
            UserInfoClass UserInfoClassObj = new UserInfoClass();
            UserInfoClassObj.SchoolId = SchoolId;
            UserInfoClassObj.UserId = UserObj.UserId;
            UserInfoClassObj.UserName = UserObj.UserName;
            UserInfoClassObj.Allowlogin = true;
            UserInfoClassObj.LoginTime = DateTime.Now;
            UserInfoClassObj.NeedSessionOut = false;
            UserInfoClassObj.IP = Request.UserHostAddress;
            UserInfoClassObj.MachineName = Request.UserHostName;
            _UaerList.Add(UserInfoClassObj);

        }
        Application.Lock();
        Application["UserList"] = _UaerList;
        Application.UnLock();
    }

    # endregion

    //[WebMethod(EnableSession = true)]
    //public static string CheckAppExpiry()
    //{
    //    string Expiremsg = "";
    //    SchoolClass objSchool = (SchoolClass)HttpContext.Current.Session[WinerConstants.SessionSchool];
    //    if (objSchool != null)
    //    {
    //        int days = 0;
    //        bool Unlimited = false;
    //        OdbcConnection _myOdbcConn = null;
    //        string m_stConnection = WinerUtlity.GetConnectionString(objSchool);
    //        _myOdbcConn = new OdbcConnection(m_stConnection);
    //        _myOdbcConn.Open();
    //        string sql = "SELECT tblschooldetails.ExpireDate from tblschooldetails";
    //        OdbcCommand m_cmd = new OdbcCommand(sql, _myOdbcConn);
    //        OdbcDataReader _myReader = m_cmd.ExecuteReader();
    //        if (_myReader.HasRows)
    //        {
    //            if (_myReader.GetValue(0).ToString() != "")
    //            {
    //                DateTime ExpireDate = Convert.ToDateTime(_myReader.GetValue(0).ToString());
    //                if (ExpireDate.Date.Equals(DateTime.MaxValue.Date))
    //                    Unlimited = true;
    //                else
    //                {
    //                    ExpireDate = ExpireDate.Date;
    //                    DateTime CurrentDate = DateTime.Now.Date;
    //                    TimeSpan span = ExpireDate.Subtract(CurrentDate);
    //                    days = (int)span.TotalDays;
    //                }
    //                if (days <= 45 && days > 0)
    //                    Expiremsg = "Your Application will expire within " + days + " days.";
    //                else if (days <= 30 && days > 0)
    //                    Expiremsg = "Your Application will expire within " + days + " days.";
    //                else if (days > 30 || Unlimited == true)
    //                    Expiremsg = "";
    //                else if (days == 0)
    //                    Expiremsg = "Your Application will expire on today.";
    //                else
    //                    Expiremsg = "Your Application is expired.Please update licence.";
    //            }
    //            else
    //                Expiremsg = "Your Application license not updated .";

    //        }
    //    }
    //    else
    //    {
    //        HttpContext.Current.Response.Redirect("sectionerr.htm");

    //    }
    //    return Expiremsg;
    //}
    [WebMethod(EnableSession = true)]
    public static string UpdateLis(string key)
    {
        string _message = "";
        if (key.Trim() != "")
        {
            SchoolClass objSchool = (SchoolClass)HttpContext.Current.Session[WinerConstants.SessionSchool];
            if (objSchool != null)
            {
                string FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, HttpContext.Current.Server.MapPath(""));
                FilePath += "\\UpImage\\";
                AddressLisencesing _MyLicense = new AddressLisencesing(FilePath, WinerUtlity.GetConnectionString(objSchool));
                _MyLicense.RegisterLicenseNew(key, out _message);
                _MyLicense = null;
            }
            else
            {
                HttpContext.Current.Response.Redirect("sectionerr.htm");
            }
        }
        else
        {
            _message = "Please Enter a License key";
        }
        return _message;
    }
    [WebMethod(EnableSession = true)]
    public static string GetLisRegData()
    {
        string SK = "";
        SchoolClass objSchool = (SchoolClass)HttpContext.Current.Session[WinerConstants.SessionSchool];
        if (objSchool != null)
        {
            string FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, HttpContext.Current.Server.MapPath(""));
            FilePath = FilePath + "\\UpImage\\";
            AddressLisencesing _MyLicense = new AddressLisencesing(FilePath, WinerUtlity.GetConnectionString(objSchool));
            SK = _MyLicense.GetKeyAsChar();
        }
        return SK;
        ;
    }
    [WebMethod(EnableSession = true)]
    public static string[] LoadLisenceData()
    {
        string[] dt = new string[5];
        SchoolClass objSchool = (SchoolClass)HttpContext.Current.Session[WinerConstants.SessionSchool];
        if (objSchool != null)
        {
            string FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, HttpContext.Current.Server.MapPath(""));

            FilePath += "\\UpImage\\";

            AddressLisencesing _MyLicense = new AddressLisencesing(FilePath, WinerUtlity.GetConnectionString(objSchool));
            string _software, _Version, _usercount, _expiredate, _Dayesleft;
            _MyLicense.GetLisenceDetails(out _software, out _Version, out _usercount, out _expiredate, out _Dayesleft);

            dt[0] = _software;
            dt[1] = _Version;
            dt[2] = _usercount;
            dt[3] = _expiredate;
            dt[4] = _Dayesleft;
        }
        else
        {
            HttpContext.Current.Response.Redirect("sectionerr.htm");

        }
        return dt;
    }
    [WebMethod(EnableSession = true)]
    public static string[] LoadInstituteData()
    {
        string[] SchoolDetails = new string[2];
        SchoolClass objSchool = (SchoolClass)HttpContext.Current.Session[WinerConstants.SessionSchool];
        if (objSchool != null)
        {
            string FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, HttpContext.Current.Server.MapPath(""));
            try
            {

                OdbcConnection _myOdbcConn = null;
                string m_stConnection = WinerUtlity.GetConnectionString(objSchool);
                _myOdbcConn = new OdbcConnection(m_stConnection);
                _myOdbcConn.Open();
                string sql = "select SchoolName,SchoolImage from tblschooldetails";
                OdbcCommand m_cmd = new OdbcCommand(sql, _myOdbcConn);
                OdbcDataReader _myReader = m_cmd.ExecuteReader();
                ImageUploaderClass imgobj = new ImageUploaderClass(objSchool);
                SchoolDetails[1] = "<img src=\"" + "Handler/ImageReturnHandler.ashx?id=" + objSchool.SchoolId + "&type=Logo" + "\" width=\"90%\" alt=\"\" />";
                if (_myReader.HasRows)
                {
                    if (_myReader.GetValue(0).ToString() != "")
                    {
                        SchoolDetails[0] = _myReader.GetValue(0).ToString();
                        // ImgName.InnerHtml = SchoolDetails1;
                        // Page.Title = objSchool.SchoolName.ToString();
                    }
                    else
                    {
                        SchoolDetails[0] = ConfigurationSettings.AppSettings["SchoolName"];
                    }
                }
                else
                {
                    SchoolDetails[0] = ConfigurationSettings.AppSettings["SchoolName"];
                }
            }
            catch
            {
                SchoolDetails[0] = ConfigurationSettings.AppSettings["SchoolName"];
            }
        }
        else
        {
            HttpContext.Current.Response.Redirect("sectionerr.htm");

        }
        return SchoolDetails;
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(objSchool));
        string FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));

      
        
            //ParentLogin MyParent = new ParentLogin(_mysqlObj, objSchool);
          //  ParentLogin MyParent = new ParentLogin(_mysqlObj, objSchool);
        string _UserName = Client_Login.UserName.Trim();
        string _PassWord = Client_Login.Password.Trim();
        //string _UserName = Client_Login.UserName.Trim();
        //string _PassWord = Client_Login.Password.Trim();
        //ParentInfoClass MyParentObj;
        KnowinUser MyParentObj = new KnowinUser(WinerUtlity.GetConnectionString(objSchool), FilePath, WinerUtlity.GetRelativeFilePath(objSchool)); ;
        string _Message = "";
        int role = 0;
        string message = "";
        //string BatchName = "";
        if (MyParentObj.LoginUserStudent(_UserName, _PassWord, role, out message))
        {

           // MyParent.RecordLogin(_UserName);
            MyParentObj.SchoolObject = objSchool;
           
            Session["UserObj"] = MyParentObj;
            DBLogClass _DblogObj = new DBLogClass(_mysqlObj);

            _mysqlObj = null;
            Session["ACtivationStatus"] = 0;
         
            Response.Redirect("StudentDetails.aspx");
        }
        else
        {

            Client_Login.FailureText = _Message;
        }
        _mysqlObj.CloseConnection();
        _mysqlObj = null;
        MyParentObj = null;

    }

    //protected void Lnk_Registeration_Click(object sender, EventArgs e)
    //{

    //    MPE_AddressLicense.Show();
    //}
    //protected void Lnk_Lisencedetails_Click(object sender, EventArgs e)
    //{

    //    string FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));

    //    FilePath += "\\UpImage\\";

    //    AddressLisencesing _MyLicense = new AddressLisencesing(FilePath, WinerUtlity.GetConnectionString(objSchool));
    //    string _software, _Version, _usercount, _expiredate, _Dayesleft;
    //    _MyLicense.GetLisenceDetails(out _software, out _Version, out _usercount, out _expiredate, out _Dayesleft);
    //    LBl_software.Text = _software;
    //    Lbl_Version.Text = _Version;
    //    //Lbl_installionDate.Text = _installionDate;
    //    Lbl_usercount.Text = _usercount;
    //    Lbl_expiredate.Text = _expiredate;
    //    Lbl_Dayesleft.Text = _Dayesleft;
    //    _MyLicense = null;
    //   // MPE_viewlicence.Show();

    //}
    //protected void Btn_Register_Click(object sender, EventArgs e)
    //{
    //    string _message;
    //    if (Txt_EnterKey.Text.Trim() != "")
    //    {
    //        string FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));
    //        FilePath += "\\UpImage\\";
    //        Lisencesing _MyLicense = new Lisencesing(FilePath, WinerUtlity.GetConnectionString(objSchool));
    //        _MyLicense.UpdateLicense(Txt_EnterKey.Text, out _message);
    //        _MyLicense = null;
    //    }
    //    else
    //    {
    //        _message="Please Enter a License key";
    //    }
    //    Lbl_msg.Text = _message;
    //    MPE_MessageBox.Show();
    //}

    //protected void Btn_UpRegister_Click(object sender, EventArgs e)
    //{
    //    string FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));
    //        FilePath = FilePath + "\\UpImage\\";

    //    string _msg="";
    //    if (FileUpload_License.PostedFile == null)
    //    {
    //        Lbl_msg.Text = "Please select a file.";
    //        this.MPE_MessageBox.Show();
    //    }
    //    else if  (FileUpload_License.PostedFile != null && !ValidImageFile())
    //    {
    //        Lbl_msg.Text = "File type cannot be uploaded";
    //        this.MPE_MessageBox.Show();

    //    }
    //    else
    //    {
    //        string _extension = Path.GetExtension(FileUpload_License.FileName).ToLower();
    //        string _FileName = "TMP" + DateTime.UtcNow.ToString("s").Replace(":", "").Replace("-", "").Replace(" ", "") + _extension;
    //        FileUpload_License.SaveAs(FilePath + _FileName);
    //        AddressLisencesing _MyLicense = new AddressLisencesing(FilePath, WinerUtlity.GetConnectionString(objSchool));
    //        _MyLicense.RegisterLicense(_FileName, ref _msg);
    //        _MyLicense = null;
    //        Lbl_msg.Text = _msg;
    //        this.MPE_MessageBox.Show();
    //        CheckExpiryDate();
    //    }
    //}

    //private bool ValidImageFile()
    //{
    //    bool _ValidFile = true;

    //    return _ValidFile;
    //}

    //protected void Lnk_DownloadKey_Click(object sender, EventArgs e)
    //{
    //    DownloadLicence();
    //    //ClientScript.RegisterClientScriptBlock(typeof(Page), "Download", "<script>window.open(\"UpImage/sysinfo.dat\");</script>"); 
    //}

    //private void DownloadLicence()
    //{
    //    string FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));

    //    MPE_AddressLicense.Show();
    //    FilePath = FilePath + "\\UpImage\\";

    //    AddressLisencesing _MyLicense = new AddressLisencesing(FilePath, WinerUtlity.GetConnectionString(objSchool));

    //    _MyLicense.GenerateSystemFile();
    //    _MyLicense = null;

    //    Response.ContentType = "text/plain";

    //    Response.AppendHeader("Content-Disposition", "attachment; filename=sysinfo.dat");
    //    string FileName = FilePath + "sysinfo.dat";

    //    //Response.TransmitFile(Server.MapPath("~/UpImage/sysinfo.dat"));
    //    Response.TransmitFile(FileName);
    //    Response.End();
    //}

    //protected void Img_DownloadLicenseKey_Click(object sender, ImageClickEventArgs e)
    //{
    //    DownloadLicence();
    //    //ClientScript.RegisterClientScriptBlock(typeof(Page), "Download", "<script>window.open(\"UpImage/sysinfo.dat\");</script>"); 
    //    //ClientScript.RegisterClientScriptBlock(typeof(Page), "Download", "<script>window.open('downloadsyskey.aspx', 'popupwindow', 'width=400,height=300,scrollbars,resizable');return false;</script>"); 
    //}

    //protected void Lnk_Adminlogin_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("AdminLogin.aspx?height=155&&width=330&&modal=true");
    //}

}
