using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
namespace WinEr
{
    public partial class ViewClassStaffMap : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        private TimeTable MyTimeTable;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }

            MyUser = (KnowinUser)Session["UserObj"];
            MyTimeTable = MyUser.GetTimeTableObj();
            if (MyTimeTable == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }
            else
            {
                if (!IsPostBack)
                {
                    //some initlization
                    LoadReport();
                }
            }
        }

        private void LoadReport()
        {
            Div_Report.InnerHtml = MyTimeTable.GetTimeClassReport(MyUser.UserId);

        }
    }
}
