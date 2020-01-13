using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Winer.Portal
{
    public partial class Schoollist : System.Web.UI.Page
    {
        private KnowinEncryption Myencryption;
        private KnowinUser MyUser;
        public MysqlClass _MysqlObj;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PortalUserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["PortalUserObj"];
        }
        protected void LinkButton1_Click(object sender, EventArgs e)
        {

        }
    }
}