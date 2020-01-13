using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WinBase;

namespace WinEr
{
    public partial class GenerateExamResult : System.Web.UI.Page
    {
        private ExamManage MyExamMang;
        private KnowinUser MyUser;
        private DataSet MydataSet;
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
            }

            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyExamMang = MyUser.GetExamObj();
            if (MyExamMang == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }
            else
            {
                if (!IsPostBack)
                {
                    //some initlization
                    Load_TermDropdown();
               
                }
            }
        }

        private void Load_TermDropdown()
        {
            Drp_TermSelect.Items.Clear();
            string sql = "SELECT tblcce_term.Id as Id,tblcce_term.TermName as TermName FROM tblcce_term";
            DataSet Ds_term = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (Ds_term != null && Ds_term.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drcls in Ds_term.Tables[0].Rows)
                {
                    li = new ListItem(drcls["TermName"].ToString(), drcls["Id"].ToString());
                    Drp_TermSelect.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("NO Data Found", "0");
                Drp_TermSelect.Items.Add(li);
                Load_TermDropdown();
            }
        }

        protected void Btn_Generate_Click(object sender, EventArgs e)
        {
            string sql = "";
            try
            {
                CentralDataBase obj_Central = new CentralDataBase();
                sql = "insert into tbleventshedscheduler(exeName,SchoolId,Status) values('WinEr.CCE.ReportConverter'," + objSchool.SchoolId + ",0)";
                obj_Central.ExquteQuery(sql);
                WC_MessageBox.ShowMssage(Drp_TermSelect.SelectedItem.Text+" is create sucessfully");

            }
            catch(Exception ex)
            {
                WC_MessageBox.ShowMssage(ex.Message);
            }
            

        }


    }
}
