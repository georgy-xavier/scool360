using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WinEr
{
    public partial class ClassRedirect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int ClassId = 0;
            string page = "";
            if (Request.QueryString["PageName"] != null)
            {
                page = Request.QueryString["PageName"].ToString();
            }
            else
            {  //Back
                page = "LoadClassDetails.aspx";
            }

            if (Request.QueryString["ClassId"] != null)
            {
                if (int.TryParse(Request.QueryString["ClassId"].ToString(), out ClassId))
                {
                    Session["ClassId"] = ClassId;

                }

            }

            Response.Redirect(page);

 
        }
    }
}
