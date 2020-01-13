using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WinBase;

namespace WinEr
{
    public partial class supportlink : System.Web.UI.Page
    {
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WinerUtlity.NeedCentrelDB())
            {
                if (Session[WinerConstants.SessionSchool] == null)
                {
                    Response.Redirect("Logout.aspx");
                }
                objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                Response.Redirect("http://itonsky.com/supporttracker/entertoken.aspx?CustId=" + objSchool.CustId);
            }
            else
            {
                Response.Redirect("http://support.winceron.com/");
            }
        }
    }
}
