using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Configuration;
using System.Data.Odbc;
using System.Collections;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WebChart;
using System.Drawing;
using WinEr;
using System.IO;
using System.Text;

namespace WinEr
{
    public partial class WinErStudentMaster : System.Web.UI.MasterPage
    {
        private KnowinUser MyUser;
        private ConfigManager MyConfig;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            {
                MyUser = (KnowinUser)Session["UserObj"];
                MyConfig = MyUser.GetConfigObj();

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
                    if (MyUser.IsLogedIn && MyUser.HaveStudentRight)
                    {
                        if (!IsPostBack)
                        {
                            MyUser.SELECTEDMODE = 1;
                            string userId = MyUser.User_Id.ToString();
                            string _InnerHtml = " <script type=\"text/javascript\">sessionStorage.setItem(\"$SMD$\",\"1\");document.cookie = \"$CUR$USR$ID$=" + userId.ToString() + "\";</script>";
                            this.javascriptId.InnerHtml = _InnerHtml;
                            if (MyUser.HaveSchoolRight)
                            {
                                this.divAreaSep.InnerHtml = "<a href=\"SchoolHome.aspx?SM=2\" class=\"roundBorder\" onmouseover=\"roll_over('but1','images/schoolmanagement.png')\" onmouseout=\"roll_over('but1', 'images/schoolmanagementover.png')\"><img alt=\"\" src=\"images/schoolmanagementover.png\" name=\"but1\" /></a>";
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
                        Response.Redirect("LoginErr.htm");
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
                        //if (!_UaerList[_UserIndex].Allowlogin || _UaerList[_UserIndex].NeedSessionOut || _UaerList[_UserIndex].IP != Request.UserHostAddress)
                        if (!_UaerList[_UserIndex].Allowlogin || _UaerList[_UserIndex].NeedSessionOut)
                        {
                            Session["UserObj"] = null;
                            Response.Redirect("LoginErr.htm?OldIp=" + _UaerList[_UserIndex].IP + "&NewIp=" + Request.UserHostAddress);

                        }
                        else
                        {
                            _Valid = true;
                        }
                    }
                    else
                    {
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

        //private void loadschoolinfo()
        //{
        //    //String ImageUrl = "";
        //    //imgmaster_school.ImageUrl = "Handler/ImageReturnHandler.ashx?id=1&type=Logo";
        //    String Sql = "SELECT SchoolName FROM tblschooldetails WHERE Id=1";
        //    MyReader = MyConfig.m_MysqlDb.ExecuteQuery(Sql);
        //    if (MyReader.HasRows)
        //    {
        //        if (MyReader.GetValue(0).ToString().Length > 25)
        //        {
        //            lblmaster_schoolname.Text = MyReader.GetValue(0).ToString().Substring(0, 24) + "...";
        //        }
        //        else
        //        {
        //            lblmaster_schoolname.Text = MyReader.GetValue(0).ToString();
        //        }
        //        lblmaster_schoolname.ToolTip = MyReader.GetValue(0).ToString();

        //    }
        //}
        //protected void Drp_Dashboard_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string page = MyUser.SelectDashBoard(Drp_Dashboard.SelectedValue, MyUser.SELECTEDMODE);
        //    if (page != "")
        //    {
        //        Response.Redirect(page);
        //    }
        //}
        //protected void Btn_Default_Click(object sender, EventArgs e)
        //{
        //    MyUser.SetDefaultDashboard(1);
        //}
        //private void Load_DrpDashboard()
        //{
        //    Drp_Dashboard.Items.Clear();
        //    MyReader = MyUser.GetDashboardPages_hasRights();
        //    if (MyReader.HasRows)
        //    {
        //        Drp_Dashboard.Items.Add(new ListItem("Select Dashboard", "-1"));
        //        while (MyReader.Read())
        //        {

        //            Drp_Dashboard.Items.Add(new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString()));

        //        }
        //    }

        //    //PanelDashborad.Visible = false;
        //    //if (Drp_Dashboard.Items.Count > 2)
        //    //{
        //    //    PanelDashborad.Visible = true;
        //    //}
        //}


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
