using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Drawing;
using System.Data;
using AjaxControlToolkit;
using System.IO;

namespace WinEr
{
    public partial class ManageStudentBulk : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private int QrystudId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            WC_ManageStudent.EVNTSave += new EventHandler(Wc_ManageStudent_Saved);
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (Request.QueryString["StudId"] == null)
            {
                Response.Redirect("SearchStudent.aspx");
            }
            int.TryParse(Request.QueryString["StudId"].ToString(), out QrystudId);
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            } 
            else if (!MyUser.HaveActionRignt(4))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    WC_ManageStudent.STUDENTID = QrystudId.ToString();
                }
            }
        }
        
        protected void Wc_ManageStudent_Saved(object sender, EventArgs e)
        {
            ClientScript.RegisterClientScriptBlock(typeof(Page), "AnyScript", "<script>PageRelorad();</script>"); 
            //ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaetBookAllot, this.pnlAjaxUpdaetBookAllot.GetType(), "AnyScript", "PageRelorad();", true);
        }

    }
}

