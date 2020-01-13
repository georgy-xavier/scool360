using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WinEr
{
    public partial class ErrorRedirect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Error"] != null)
            {
                Label4.Text = Request.QueryString["Error"].ToString();
            }
            if (Request.QueryString["NextPage"] != null)
            {
                LinkButton1.Text = Request.QueryString["NextPage"].ToString();
            }
            if (Request.QueryString["URL"] != null)
            {
                Hd_URL.Value = Request.QueryString["URL"].ToString();
            }
        }


        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect(Hd_URL.Value);
        }
    }
}
