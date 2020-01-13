using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WinEr
{
    public partial class StaffSessionMaker : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["StaffId"] != null)
            {
                Session["StaffId"] = Request.QueryString["StaffId"];
                if (Request.QueryString["Type"] == null)
                {
                    Response.Redirect("StaffDetails.aspx");
                }
                else
                    if (Request.QueryString["Type"] == "1")
                    {
                        Response.Redirect("ViewStaffIncidenceByType.aspx");
                    }
            }
        }
    }
}
