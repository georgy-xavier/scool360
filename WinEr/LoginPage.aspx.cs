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
using WinEr;


public partial class _LoginPage : System.Web.UI.Page 
{
    private SchoolClass objSchool = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        string FooterCompanyName = ConfigurationSettings.AppSettings["FooterCompanyName"];
        string FooterWebsite = ConfigurationSettings.AppSettings["FooterWebsite"];
        string Footerstr = " <a href=\"" + FooterWebsite + "\">" + FooterCompanyName + "</a>";
     //   footerName.InnerHtml = Footerstr;
        if (WinerUtlity.NeedCentrelDB())
        {
            if (Session[WinerConstants.SessionSchool] == null)
            {
                Response.Redirect("WinerSchoolSelection.aspx");
            }
            objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
        }
        if (!IsPostBack)
        {
           
        }
     
    }

 
  
    protected void Client_Login_Authenticate(object sender, AuthenticateEventArgs e)
    {
        string SchoolDetails1 = "";
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


      
        if (UserObj.LoginUser(Client_Login.UserName.ToString(), Client_Login.Password.ToString(), 0,out _meassageError))
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
           List <UserInfoClass> _UaerList = new List<UserInfoClass>();
            if (Application["UserList"] == null)
            {
                Application.Lock();
                Application["UserList"] = _UaerList;
                Application.UnLock();
            }
             int SchoolId= WinerUtlity.GetSchoolId(objSchool);
            _UaerList = (List<UserInfoClass>)Application["UserList"];
            int _UserIndex = WinerUtlity.GetUserIndex(UserObj.UserId,SchoolId, _UaerList);
            if (_UserIndex != -1)
            {
                _UaerList[_UserIndex].UserName=UserObj.UserName;
                _UaerList[_UserIndex].Allowlogin=true;
                _UaerList[_UserIndex].LoginTime=DateTime.Now;
                _UaerList[_UserIndex].NeedSessionOut=false;
                _UaerList[_UserIndex].IP=Request.UserHostAddress;
                _UaerList[_UserIndex].MachineName=Request.UserHostName;
            }
            else
            {
                UserInfoClass UserInfoClassObj = new UserInfoClass();
                UserInfoClassObj.SchoolId = SchoolId;
                UserInfoClassObj.UserId = UserObj.UserId;
                UserInfoClassObj.UserName=UserObj.UserName;
                UserInfoClassObj.Allowlogin=true;
                UserInfoClassObj.LoginTime=DateTime.Now;
                UserInfoClassObj.NeedSessionOut=false;
                UserInfoClassObj.IP=Request.UserHostAddress;
                UserInfoClassObj.MachineName=Request.UserHostName;
                _UaerList.Add(UserInfoClassObj);
            
            }
            Application.Lock();
            Application["UserList"] = _UaerList;
            Application.UnLock();
    }

   
    # endregion
    protected void Lnk_Registeration_Click(object sender, EventArgs e)
    {
        //Txt_EnterKey.Text = "";

        //Lisencesing _MyLicense = new Lisencesing(MyUser.FilePath + "\\UpImage\\");
        //Lbl_getKey.Text = _MyLicense.GetSystemKey();
        //_MyLicense = null;
        //MPE_Registeration.Show();
        MPE_AddressLicense.Show();
    }

    protected void Lnk_Lisencedetails_Click(object sender, EventArgs e)
    {
        //Lisencesing _MyLicense = new Lisencesing(MyUser.FilePath + "\\UpImage\\");
        //string _software, _Version, _installionDate, _usercount, _expiredate, _Dayesleft;
        //_MyLicense.GetLisenceDetails(out _software, out _Version, out _installionDate, out _usercount, out _expiredate, out _Dayesleft);
        //LBl_software.Text = _software;
        //Lbl_Version.Text = _Version;
        //Lbl_installionDate.Text = _installionDate;
        //Lbl_usercount.Text = _usercount;
        //Lbl_expiredate.Text = _expiredate;
        //Lbl_Dayesleft.Text = _Dayesleft;
        //_MyLicense = null;
        //MPE_viewlicence.Show();
        string FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));
            FilePath += "\\UpImage\\";

            AddressLisencesing _MyLicense = new AddressLisencesing(FilePath, WinerUtlity.GetConnectionString(objSchool));
        string _software, _Version, _usercount, _expiredate, _Dayesleft;
        _MyLicense.GetLisenceDetails(out _software, out _Version, out _usercount, out _expiredate, out _Dayesleft);
        LBl_software.Text = _software;
        Lbl_Version.Text = _Version;
        //Lbl_installionDate.Text = _installionDate;
        Lbl_usercount.Text = _usercount;
        Lbl_expiredate.Text = _expiredate;
        Lbl_Dayesleft.Text = _Dayesleft;
        _MyLicense = null;
        MPE_viewlicence.Show();
        
    }

    protected void Btn_Register_Click(object sender, EventArgs e)
    {
        string _message;
        if (Txt_EnterKey.Text.Trim() != "")
        {
            string FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));
            FilePath += "\\UpImage\\";
            Lisencesing _MyLicense = new Lisencesing(FilePath, WinerUtlity.GetConnectionString(objSchool));
            _MyLicense.UpdateLicense(Txt_EnterKey.Text, out _message);
            _MyLicense = null;
        }
        else
        {
            _message="Please Enter a License key";
        }
        Lbl_msg.Text = _message;
        MPE_MessageBox.Show();
    }

    protected void Btn_UpRegister_Click(object sender, EventArgs e)
    {
        string SchoolDetails1 = "";
        string FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));
            FilePath = FilePath + "\\UpImage\\";
        string _msg="";
        if (FileUpload_License.PostedFile == null)
        {
            Lbl_msg.Text = "Please select a file.";
            this.MPE_MessageBox.Show();
        }
        else if  (FileUpload_License.PostedFile != null && !ValidImageFile())
        {
            Lbl_msg.Text = "File type cannot be uploaded";
            this.MPE_MessageBox.Show();

        }
        else
        {
            string _extension = Path.GetExtension(FileUpload_License.FileName).ToLower();
            string _FileName = "TMP" + DateTime.UtcNow.ToString("s").Replace(":", "").Replace("-", "").Replace(" ", "") + _extension;
            FileUpload_License.SaveAs(FilePath + _FileName);
            AddressLisencesing _MyLicense = new AddressLisencesing(FilePath, WinerUtlity.GetConnectionString(objSchool));
            _MyLicense.RegisterLicense(_FileName, ref _msg);
            _MyLicense = null;
            Lbl_msg.Text = _msg;
            this.MPE_MessageBox.Show();
        }
    }

    private bool ValidImageFile()
    {
        bool _ValidFile = true;

        return _ValidFile;
    }

    protected void Lnk_DownloadKey_Click(object sender, EventArgs e)
    {
        DownloadLicence();
        //ClientScript.RegisterClientScriptBlock(typeof(Page), "Download", "<script>window.open(\"UpImage/sysinfo.dat\");</script>"); 
    }

    private void DownloadLicence()
    {
        string SchoolDetails1 = "";
        string FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));
        MPE_AddressLicense.Show();
        FilePath = FilePath + "\\UpImage\\";

        AddressLisencesing _MyLicense = new AddressLisencesing(FilePath, WinerUtlity.GetConnectionString(objSchool));
        
        _MyLicense.GenerateSystemFile();
        _MyLicense = null;

        Response.ContentType = "text/plain";

        Response.AppendHeader("Content-Disposition", "attachment; filename=sysinfo.dat");
        string FileName = FilePath + "sysinfo.dat";

        //Response.TransmitFile(Server.MapPath("~/UpImage/sysinfo.dat"));
        Response.TransmitFile(FileName);
        Response.End();
    }

    protected void Img_DownloadLicenseKey_Click(object sender, ImageClickEventArgs e)
    {
        DownloadLicence();
        //ClientScript.RegisterClientScriptBlock(typeof(Page), "Download", "<script>window.open(\"UpImage/sysinfo.dat\");</script>"); 
        //ClientScript.RegisterClientScriptBlock(typeof(Page), "Download", "<script>window.open('downloadsyskey.aspx', 'popupwindow', 'width=400,height=300,scrollbars,resizable');return false;</script>"); 
    }

    protected void Lnk_Adminlogin_Click(object sender, EventArgs e)
    {
        Response.Redirect("AdminLogin.aspx?height=155&&width=330&&modal=true");
    }
    
}
