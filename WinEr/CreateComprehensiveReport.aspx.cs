using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;

namespace WinEr
{
    public partial class CreateComprehensiveReport : System.Web.UI.Page
    {
        private ClassOrganiser  MyClassMang;
        private KnowinUser      MyUser;
        private OdbcDataReader  MyReader    = null;
       
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }

            MyUser      = (KnowinUser)Session["UserObj"];
            MyClassMang = MyUser.GetClassObj();
          
            if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(835))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                  

                }
            }
        }


        

       

    }
}