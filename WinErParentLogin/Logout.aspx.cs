using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Web.Security;

namespace WinErParentLogin
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SchoolClass objSchool = null;
            if (Session["MyParentObj"] != null)
            {
                ParentInfoClass MyParentObj = (ParentInfoClass)Session["MyParentObj"];
                objSchool = MyParentObj.SchoolObject;
                Session["MyParentObj"] = null;
            }

            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            
            if (objSchool != null)
                Response.Redirect("Default.aspx?" + WinerConstants.SchoolId + "=" + objSchool.SchoolId);
            else
                Response.Redirect("Default.aspx");

        }
    }
}
