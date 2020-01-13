using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WinEr
{
    public partial class SchoolHome : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyUser.SELECTEDMODE = 2;
            DirectDashboard();
           
        }

        private void DirectDashboard()
        {
            string _LoginPage = "WinErSchoolHome.aspx";
            int MasterId = 2;
            int _DashboardId = 0;
            if (MyUser.IsDefaultDashBoardExists(MyUser.UserId, out _DashboardId))
            {
                _LoginPage = MyUser.SelectDashBoard(_DashboardId.ToString(), MasterId);
            }
            Response.Redirect(_LoginPage);
        }
    }
}
