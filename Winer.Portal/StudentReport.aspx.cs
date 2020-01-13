using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Winer.Portal
{
    public partial class StudentReport : System.Web.UI.Page
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
            HiddenField1.Value = MyUser.m_orgId.ToString();
        }
        private void loadgrid()
        {
            

        }
        protected void Drp_School_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
        protected void Btn_Show_Click(object sender, EventArgs e)
        {

        }
        protected void Grd_StudentReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
               
        }

        protected void Grd_StudentReport_SelectedIndexChanged(object sender, EventArgs e)
        {
                
        }

        protected void img_export_Excel_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}