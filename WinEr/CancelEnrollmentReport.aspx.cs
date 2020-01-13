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
    public partial class CancelEnrollmentReport : System.Web.UI.Page
    {
        private OdbcDataReader m_MyReader = null;
        private KnowinUser MyUser;
        private StudentManagerClass MyStudMang;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(500))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    Img_Export.Enabled = false;
                    Txt_from.Text = MyUser.GerFormatedDatVal(System.DateTime.Now);
                    Txt_To.Text = MyUser.GerFormatedDatVal(System.DateTime.Now);
                    LoadAllCanceledUsersToDropDown();
                }
            }
        }

        private void LoadAllCanceledUsersToDropDown()
        {
            Drp_CanceledUser.Items.Clear();

            string sql = "SELECT tbluser.Id, tbluser.SurName from tbluser where tbluser.Status=1 and tbluser.Id in (select DISTINCT(tblview_student.CanceledUser) from tblview_student) order by tbluser.Id asc";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("ALL", "0");
                Drp_CanceledUser.Items.Add(li);
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_CanceledUser.Items.Add(li);
                }
            }
            else
            {
                Btn_Generate.Enabled = false;
                ListItem li = new ListItem("No User Found", "-1");
                Drp_CanceledUser.Items.Add(li);
            }
        }

        protected void Btn_Generate_Click(object sender, EventArgs e)
        {
            Grd_CanceledEnrollment.Columns[0].Visible = true;
            DateTime _FromDate = MyUser.GetDareFromText(Txt_from.Text);
            DateTime _ToDate = MyUser.GetDareFromText(Txt_To.Text);
            string sql = "SELECT tblview_student.Id, tblview_student.StudentName, tblview_student.Address, tblview_student.`Comment` as Reason, tbluser.SurName as CanceledUser from tblview_student inner join tbluser on tblview_student.CanceledUser= tbluser.Id where tblview_student.`Status`=3 and tblview_student.DateOfLeaving BETWEEN '" + _FromDate.ToString("s") + "' and '" + _ToDate.ToString("s") + "'";
            if(int.Parse(Drp_CanceledUser.SelectedValue)>0)
            {
                sql = sql + " and tblview_student.CanceledUser=" + int.Parse(Drp_CanceledUser.SelectedValue);
            }
            //sql = sql + " order by tblview_student.StudentName";
            //sai changed for order based on roll nos
            sql = sql + " order by tblview_student.ClassId ASC ,tblview_student.RollNo ASC ";
            MyDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (MyDataSet.Tables[0].Rows.Count > 0)
            {
                ViewState["StudentList"] = MyDataSet;
                Grd_CanceledEnrollment.DataSource = MyDataSet;
                Grd_CanceledEnrollment.DataBind();
                Img_Export.Enabled = true;
                Lbl_Message.Text = "";
            }
            else
            {
                Grd_CanceledEnrollment.DataSource = null;
                Grd_CanceledEnrollment.DataBind();
                Img_Export.Enabled = false;
                Lbl_Message.Text = "No Canceled Enrollments";
            }
            Grd_CanceledEnrollment.Columns[0].Visible = false;
        }

        

        protected void Grd_Student_SelectedIndexChanged(object sender, EventArgs e)
        {

            int _studId = int.Parse(Grd_CanceledEnrollment.SelectedRow.Cells[0].Text);
            Session["StudId"] = _studId;
            Session["StudType"] = 2;
            Response.Redirect("StudentDetails.aspx");
            //Grd_Student.Columns[0].Visible = true;

            //this.studdtls.InnerHtml = "";
            //MPE_MessageBox.Show();
            //this.studdtls.InnerHtml = getStudDetails(_studId);          
            //Grd_Student.Columns[0].Visible = false;
        }

        protected void Img_Export_Click(object sender, ImageClickEventArgs e)
        {
            DataSet ExportDataSet = newDataSet((DataSet)ViewState["StudentList"]);
            string FileName = "CanceledEnrollmentReport";
            string _ReportName = "Canceled Enrollment Report";
            if (!WinEr.ExcelUtility.ExportDataToExcel(ExportDataSet, _ReportName, FileName, MyUser.ExcelHeader))
            {
                Lbl_Message.Text = "This function need Ms office";
            }
        }

        private DataSet newDataSet(DataSet dataSet)
        {
            DataSet newDataset = new DataSet();
            DataTable dt;
            DataRow dr;

            newDataset.Tables.Add(new DataTable("CanclData"));
            dt = newDataset.Tables["CanclData"];
            dt.Columns.Add("Student Name");
            dt.Columns.Add("Address");
            dt.Columns.Add("Reason");
            dt.Columns.Add("Cancelled User");
            foreach (GridViewRow gv in Grd_CanceledEnrollment.Rows)
            {
                dr = dt.NewRow();
                dr["Student Name"] = gv.Cells[1].Text;
                dr["Address"] = gv.Cells[2].Text;
                dr["Reason"] = gv.Cells[3].Text;
                dr["Cancelled User"] = gv.Cells[4].Text;
                newDataset.Tables["CanclData"].Rows.Add(dr);
            }
            return newDataset;
        }
    }
}
