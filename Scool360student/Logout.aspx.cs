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
using System.Collections.Generic;
using WinBase;
using WinEr;

public partial class _Logout : System.Web.UI.Page
{
    SchoolClass objSchool = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session[WinerConstants.SessionSchool] != null)
        {
            objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
        }
        if (Session["UserObj"] != null)
        {
            KnowinUser User = (KnowinUser)Session["UserObj"];
            User.m_DbLog.LogToDb(User.UserName, "User Logout", "User " + User.UserName + " Loged Out", 1);
            if (ConfigurationSettings.AppSettings["NeedAppicationObject"] == "1")
            {
                AppicationObjectRemovel(User);
            }
            User.Dispose();
            Session["UserObj"] = null;
        }
      
        Session.Clear();
        Session.Abandon();
        Session.RemoveAll();
        if (objSchool != null)
            Response.Redirect("Default.aspx?" + WinerConstants.SchoolId + "=" + objSchool.SchoolId);
        else
            Response.Redirect("Default.aspx");


    }
    # region Application Object Area Fee
    private void AppicationObjectRemovel(KnowinUser _Userobj)
    {
        if (Application["UserList"] != null)
        {
            List <UserInfoClass> _UaerList = new List<UserInfoClass>();
            _UaerList = (List<UserInfoClass>)Application["UserList"];
            int SchoolId = WinerUtlity.GetSchoolId(objSchool);
            int _UserIndex = WinerUtlity.GetUserIndex(_Userobj.UserId, SchoolId, _UaerList);
            if (_UserIndex != -1)
            {
                _UaerList.RemoveAt(_UserIndex);
                Application.Lock();
                Application["UserList"] = _UaerList;
                Application.UnLock();

            }
           
        }
    }

    private int GetUserIndex(int _UserId, List<UserInfoClass> list)
    {
        int _ReturnIndex = -1;
        for (int _i = 0; _i < list.Count; _i++) // Loop through List with foreach
        {

            if (list[_i].UserId == _UserId)
            {
                return _i;
            }
        }
        return _ReturnIndex;
    }
    # endregion
}
