using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WinEr
{
    public partial class ShowDynamicTc : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
     
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_init(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            if (MyStudMang == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }
            else
            {
                if (!IsPostBack)
                {
                    //some initlization
                    if (Request.QueryString["StudId"].ToString() != "")
                    {
                        try
                        {
                            int StudId = int.Parse(Request.QueryString["StudId"]);

                        }
                        catch
                        {

                        }
                        if (Session["StudTc"] == null)
                        {
                            Response.Redirect("IssueTC.aspx");
                        }
                        else
                        {
                            tc.InnerHtml = Session["StudTc"].ToString();
                            Session["StudTc"] = null;
                        }
                    }

                }
            }
        }
    }
}