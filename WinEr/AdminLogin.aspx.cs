using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WinBase;
using System.Collections.Generic;
using WinEr;

public partial class AdminLogin : System.Web.UI.Page
{
    private SchoolClass objSchool = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (WinerUtlity.NeedCentrelDB())
        {
            if (Session[WinerConstants.SessionSchool] == null)
            {
                Response.Redirect("WinerSchoolSelection.aspx");
            }
            objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
        }
    }
    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {

    }
  
    protected void Cmd_Cancel_Click1(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
    protected void LoginButton_Click(object sender, EventArgs e)
    {
        string FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));

        KnowinUser UserObj;
        string _meassageError;
        if (Session["UserObj"] == null)
        {
            UserObj = new KnowinUser(WinerUtlity.GetConnectionString(objSchool), FilePath, WinerUtlity.GetRelativeFilePath(objSchool));
            Session["UserObj"] = UserObj;

        }
        else
        {
            UserObj = (KnowinUser)Session["UserObj"];
        }

        if (UserObj.LoginUser(Admin_Login.UserName.ToString(), Admin_Login.Password.ToString(), 1, out _meassageError))
        {
            UserObj.RecordLogin(Admin_Login.UserName.ToString());
            if (ConfigurationSettings.AppSettings["NeedAppicationObject"] == "1")
            {
                SetApplicationUserObject(UserObj);
            }
            Response.Redirect("AdminHome.aspx");
        }
        else
        {
            //Response.Redirect("Default.aspx");
            Admin_Login.FailureText = _meassageError;
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
           int SchoolId= WinerUtlity.GetSchoolId(objSchool);
            _UaerList = (List<UserInfoClass>)Application["UserList"];
            int _UserIndex = WinerUtlity.GetUserIndex(UserObj.UserId,SchoolId, _UaerList);
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
}
