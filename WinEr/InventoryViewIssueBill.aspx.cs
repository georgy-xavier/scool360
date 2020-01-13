using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace WinEr
{
    public partial class InventoryViewIssueBill : System.Web.UI.Page
    {
        int studId;
        protected void Page_Load(object sender, EventArgs e)
        {
            studId = int.Parse(Request.QueryString["studid"].ToString());
            StringBuilder ReportText = new StringBuilder();
            if (!IsPostBack)
            {
                if (Session["ReportText"] != null)
                {

                    ReportText.Append(Session["ReportText"]);
                    ViewBill.InnerHtml = ReportText.ToString();
                }
                else
                {
                    ViewBill.InnerText = "No Receipt found";
                }
            }
        }
    }
}
