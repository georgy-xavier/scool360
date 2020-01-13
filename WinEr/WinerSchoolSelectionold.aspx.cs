using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using WinBase;

namespace WinEr
{
    public partial class WinerSchoolSelection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ScriptManager.RegisterClientScriptBlock(this.Updatepanel, this.Updatepanel.GetType(), "AnyScript", "clearcookies();", true);
                LoadSchoolList();
            }
        }

        private void LoadSchoolList()
        {
            Grd_School.Columns[0].Visible = true;
            MysqlClass objDB = new MysqlClass(WinerUtlity.CentralConnectionString);
            string sql;
            sql = "select tblschool_list.Id, tblschool_list.SchoolName from tblschool_list where tblschool_list.IsActice=1;";
                 DataSet dt = objDB.ExecuteQueryReturnDataSet(sql);

            if (dt != null && dt.Tables[0] != null && dt.Tables[0].Rows.Count > 0)
            {
                Grd_School.DataSource = dt;
                Grd_School.DataBind();
            }
            else
            {
                Lbl_select.Text = "No school found";
            }
            Grd_School.Columns[0].Visible = false;
            objDB.CloseConnection();
            objDB = null;
        }

        protected void Grd_School_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i_SelectedId = int.Parse(Grd_School.SelectedRow.Cells[0].Text.ToString());
            
            //fetch school details from the database.

            SchoolClass objSchool = WinerUtlity.GetSchoolObject(i_SelectedId);
            if (objSchool != null)
            {
                Session[WinerConstants.SessionSchool] = objSchool;
                //Response.Redirect("WinerCampusHome.aspx");
                Response.Redirect("Default.aspx");
            }
        }
    }
}
