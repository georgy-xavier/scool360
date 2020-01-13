using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WinEr
{
    public partial class StudentHome : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
                MyUser = (KnowinUser)Session["UserObj"];
                MyUser.SELECTEDMODE = 1;
                DirectDashboard();
                
        }

        private void DirectDashboard()
        {
            string _LoginPage = "WinSchoolHome.aspx";
            int MasterId = 1;
            int _DashboardId = 0;
            if (MyUser.IsDefaultDashBoardExists(MyUser.UserId, out _DashboardId))
            {
                _LoginPage = MyUser.SelectDashBoard(_DashboardId.ToString(), MasterId);
            }
            Response.Redirect(_LoginPage);
        }
    }
}
