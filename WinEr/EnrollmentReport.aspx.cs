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
    public partial class EnrollmentReport : System.Web.UI.Page
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
            else if (!MyUser.HaveActionRignt(501))
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
                    LoadAllCreatedUsersToDropDown();

                    LoadAllClassToDropDown();
                }
            }
        }

        private void LoadAllClassToDropDown()
        {
            Drp_ClassName.Items.Clear();

            MyDataSet = MyUser.MyAssociatedClass();
            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                Btn_Generate.Enabled = true;
                ListItem li = new ListItem("ALL", "0");
                Drp_ClassName.Items.Add(li);
               
                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {
                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_ClassName.Items.Add(li);
                }
            }
            else
            {
                Btn_Generate.Enabled = false;
                ListItem li = new ListItem("No Class present", "-1");
                Drp_ClassName.Items.Add(li);
            }
        }

        private void LoadAllCreatedUsersToDropDown()
        {
            Drp_CreatedUser.Items.Clear();

            string sql = "select DISTINCT(tblstudent.CreatedUserName) from tblstudent order by tblstudent.CreatedUserName asc";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("ALL");
                Drp_CreatedUser.Items.Add(li);
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(0).ToString());
                    Drp_CreatedUser.Items.Add(li);
                }
            }
            else
            {
                Btn_Generate.Enabled = false;
                ListItem li = new ListItem("No User Found");
                Drp_CreatedUser.Items.Add(li);
            }
        }

        protected void Btn_Generate_Click(object sender, EventArgs e)
        {
            DateTime _FromDate = MyUser.GetDareFromText(Txt_from.Text);
            DateTime _ToDate = MyUser.GetDareFromText(Txt_To.Text);
            string sql = "SELECT tblstudent.StudentName, tblstudent.AdmitionNo, tblclass.ClassName, tblstudent.GardianName, DATE_FORMAT( tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.Address,tblreligion.Religion, tblcast.castname,DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining ,tblstudent.CreatedUserName from tblstudent inner join tblstudentclassmap on tblstudent.Id= tblstudentclassmap.StudentId inner join tblclass on tblstudentclassmap.ClassId= tblclass.Id inner join tblreligion on tblreligion.Id= tblstudent.Religion inner join tblcast on tblcast.Id= tblstudent.`Cast` where tblstudent.`Status`=1 and date(tblstudent.CreationTime) BETWEEN '" + _FromDate.ToString("s") + "' and '" + _ToDate.ToString("s") + "'";
            if (Drp_CreatedUser.SelectedItem.Text !="ALL")
            {
                sql = sql + " and tblstudent.CreatedUserName='" + Drp_CreatedUser.SelectedItem.Text + "'";
            }
            if (int.Parse(Drp_ClassName.SelectedValue) > 0)
            {
                sql = sql + " and tblstudentclassmap.ClassId="+Drp_ClassName.SelectedValue;
            }
            //sql = sql + " order by tblstudentclassmap.ClassId";//tblstudent.StudentName";
            //sai changed for roll no order
            sql = sql + " Order by tblstudentclassmap.ClassId ASC ,tblstudentclassmap.RollNo ASC  ";
            MyDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ViewState["StudentList"] = MyDataSet;
            if (MyDataSet.Tables[0].Rows.Count > 0)
            {
                Grd_Enrollment.PageIndex = 0;
                
                Grd_Enrollment.DataSource = MyDataSet;
                Grd_Enrollment.DataBind();
                Img_Export.Enabled = true;
                Lbl_Message.Text = "";
                Pnl_Enrollment.Visible = true;
            }
            else
            {
                Grd_Enrollment.DataSource = null;
                Grd_Enrollment.DataBind();
                Img_Export.Enabled = false;
                Lbl_Message.Text = "No Enrollments Exist";
                Pnl_Enrollment.Visible = false;
            }
        }

        protected void Img_Export_Click(object sender, ImageClickEventArgs e)
        {
            DataSet ExportDataSet = newDataSet((DataSet)ViewState["StudentList"]);
            //if (!WinEr.ExcelUtility.ExportDataSetToExcel(ExportDataSet, "EnrollmentReport.xls"))
            //{
            //    Lbl_Message.Text = "This function need Ms office";
            //}
            string FileName = "EnrollmentReport";
            string _ReportName = "Enrollment Report";
            if (!WinEr.ExcelUtility.ExportDataToExcel(ExportDataSet, _ReportName, FileName, MyUser.ExcelHeader))
            {

                Lbl_Message.Text = "This function need Ms office";
            }
        }

        protected void Grd_CanceledEnrollment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {            
            Grd_Enrollment.PageIndex = e.NewPageIndex;
            DataSet MyDataSetNew = (DataSet)ViewState["StudentList"];
            Grd_Enrollment.DataSource = MyDataSetNew;
            Grd_Enrollment.DataBind();
        }

        private DataSet newDataSet(DataSet dataSet)
        {
            DataSet newDataset = new DataSet();
            DataTable dt;
            DataRow dr;

            newDataset.Tables.Add(new DataTable("TcData"));
            dt = newDataset.Tables["TcData"];
            dt.Columns.Add("Student Name");
            dt.Columns.Add("Admission No");
            dt.Columns.Add("Guardian Name");
            dt.Columns.Add("Class");
            dt.Columns.Add("DOB");
            dt.Columns.Add("Sex");
            dt.Columns.Add("Address");
            dt.Columns.Add("Religion");
            dt.Columns.Add("Caste");
            dt.Columns.Add("Date of Joining");
            dt.Columns.Add("Created User");
            foreach (DataRow dtr in dataSet.Tables[0].Rows)
            {
                dr = dt.NewRow();
                dr["Student Name"] = dtr[0].ToString();
                dr["Guardian Name"] = dtr[3].ToString();
                dr["Class"] = dtr[2].ToString();
                dr["Admission No"] = dtr[1].ToString();
                dr["Sex"] = dtr[5].ToString();
                dr["DOB"] = dtr[4].ToString();
                dr["Address"] = dtr[6].ToString();
                dr["Religion"] = dtr[7].ToString();
                dr["Caste"] = dtr[8].ToString();
                dr["Date of Joining"] = dtr[9].ToString();
                dr["Created User"] = dtr[10].ToString();
                newDataset.Tables["TcData"].Rows.Add(dr);
            }
            return newDataset;
        }

    }
}
