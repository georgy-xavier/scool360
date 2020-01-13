using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using WinBase;
using System.Data.Odbc;
using System.IO;
using System.Text;

namespace WinEr
{
    public partial class WinerSchoolMaster : System.Web.UI.MasterPage
    {
        private KnowinUser MyUser;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            {
                MyUser = (KnowinUser)Session["UserObj"];

                if (WinerUtlity.NeedCentrelDB())
                {
                    if (Session[WinerConstants.SessionSchool] == null)
                    {
                        Response.Redirect("Logout.aspx");
                    }
                    objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                }

                if (AppicationObjectCheck())
                {
                    if (MyUser.IsLogedIn && MyUser.HaveSchoolRight)
                    {
                        if (!IsPostBack)
                        {
                            MyUser = (KnowinUser)Session["UserObj"];
                            MyUser.SELECTEDMODE = 2;
                            string userId = MyUser.User_Id.ToString();
                            string _InnerHtml = " <script type=\"text/javascript\">sessionStorage.setItem(\"$SMD$\",\"2\");document.cookie = \"$CUR$USR$ID$=" + userId.ToString() + "\";</script>";
                            this.javascriptId.InnerHtml = _InnerHtml;
                            if (MyUser.HaveStudentRight)
                            {
                                //this.divAreaSep.InnerHtml = "<a href=\"StudentHome.aspx\" onmouseover=\"roll_over('but2','images/studentmanagement.png')\" onmouseout=\"roll_over('but2', 'images/studentmanagementover.png')\"><img  alt=\"\" src=\"images/studentmanagementover.png\" name=\"but2\" border=\"0\"  /></a>";
                                this.divAreaSep.InnerHtml = "<a href=\"StudentHome.aspx\"  class=\"roundBorder\" onmouseover=\"roll_over('but2','images/studentmanagement.png')\" onmouseout=\"roll_over('but2', 'images/studentmanagementover.png')\"><img  alt=\"\" src=\"images/studentmanagementover.png\" name=\"but2\"/></a>";
                            }
                            else
                            {
                                this.divAreaSep.InnerHtml = "";
                            }
                        }
                    }

                    else
                    {
                        //no rights for this user.
                        Response.Redirect("RoleErr.htm");

                    }
                }
            }
        }
        # region Application Object Area Fee
        private bool AppicationObjectCheck()
        {
            bool _Valid = false;
            if (ConfigurationSettings.AppSettings["NeedAppicationObject"] == "1")
            {
                if (Application["UserList"] != null)
                {
                    List<UserInfoClass> _UaerList = new List<UserInfoClass>();
                    _UaerList = (List<UserInfoClass>)Application["UserList"];
                    int SchoolId = WinerUtlity.GetSchoolId(objSchool);
                    int _UserIndex = WinerUtlity.GetUserIndex(MyUser.UserId, SchoolId, _UaerList);
                    if (_UserIndex != -1)
                    {
                        if (!_UaerList[_UserIndex].Allowlogin || _UaerList[_UserIndex].NeedSessionOut || _UaerList[_UserIndex].IP != Request.UserHostAddress)
                        //if (!_UaerList[_UserIndex].Allowlogin || _UaerList[_UserIndex].NeedSessionOut)
                        {
                            if (Session["UserObj"] != null)
                            {
                                KnowinUser User = (KnowinUser)Session["UserObj"];
                                User.m_DbLog.LogToDb(User.UserName, "User Logout", "User " + User.UserName + " Loged Out", 1);
                                User.Dispose();
                                Session["UserObj"] = null;
                            }

                            Session.Clear();
                            Session.Abandon();
                            Session.RemoveAll();
                            Session["UserObj"] = null;
                            Response.Redirect("LoginErr.htm?OldIp="+_UaerList[_UserIndex].IP+"&NewIp="+Request.UserHostAddress);

                        }
                        else
                        {
                            _Valid = true;
                        }
                    }
                    else
                    {
                        if (Session["UserObj"] != null)
                        {
                            KnowinUser User = (KnowinUser)Session["UserObj"];
                            User.m_DbLog.LogToDb(User.UserName, "User Logout", "User " + User.UserName + " Loged Out", 1);
                            User.Dispose();
                            Session["UserObj"] = null;
                        }
                        Session.Clear();
                        Session.Abandon();
                        Session.RemoveAll();
                        Session["UserObj"] = null;
                        Response.Redirect("LoginErr.htm?OldIp=" + _UaerList[_UserIndex].IP + "&NewIp=" + Request.UserHostAddress);
                    }
                }
            }
            else
            {
                _Valid = true;
            }
            return _Valid;

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

        //protected void Btn_Go_Click(object sender, EventArgs e)
        //{
        //    if (Txt_Search.Text.Trim() != "")
        //    {
        //        Response.Redirect("SearchIncident.aspx?Keyword=" + Txt_Search.Text);
        //    }
        //    Txt_Search.Text = Txt_Search.Text.Trim();
        //}
        //protected void Btn_link_Click(object sender, EventArgs e)
        //{
        //    string pageName = Path.GetFileName(Request.Path);

        //    string sql = "select Videolink  from tblvideourl where Pagename = '" + pageName + "' and Enabled = 1"; 
        //    MyReader = MyUser.m_MysqlDb.ExecuteQuery(sql);
        //    if (MyReader.HasRows)
        //    {
        //        string videourl = MyReader.GetValue(0).ToString();
        //        Response.Redirect(videourl); 

        //    }
        //    else
        //    {
        //        Response.Redirect("http://winceron.com/#/contact");
        //    }



        //}
    }
}
