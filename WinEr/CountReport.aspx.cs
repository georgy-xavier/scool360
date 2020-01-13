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
    public partial class CountReport : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet = null;

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
            else if (!MyUser.HaveActionRignt(502))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    Img_Excel.Visible = false;
                    Img_Excel.Enabled = false;
                   // LoadAllStudentCountInAllClasses(0);
                    Load_Batches();
                    Drplist_batch.Visible = false;
                    lbl_batch.Visible = false;
                    
                }
            }
        }

        private void LoadAllStudentCountInAllClasses(int student_mode,int Batch_Id)
        {
            int _Count = 0, _Male = 0, _Female = 0;
            DataSet CountDataSet = new DataSet();
            DataTable dt;
            DataRow dr;

            CountDataSet.Tables.Add(new DataTable("CountList"));
            dt = CountDataSet.Tables["CountList"];
            dt.Columns.Add("ClassName");
            dt.Columns.Add("TotalStudents");
            dt.Columns.Add("MaleStudents");
            dt.Columns.Add("FemaleStudents");

            MydataSet = MyUser.MyAssociatedClass();

            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Dr_Class in MydataSet.Tables[0].Rows)
                {
                    dr = dt.NewRow();
                    dr["ClassName"] = Dr_Class[1].ToString();
                    GetTotalAndGenderBasedCountForClass(student_mode,Dr_Class[0].ToString(),Batch_Id, out _Count, out _Male, out _Female);
                    dr["TotalStudents"] = _Count.ToString();
                    dr["MaleStudents"] = _Male.ToString();
                    dr["FemaleStudents"] = _Female.ToString();
                    dt.Rows.Add(dr);
                }

                Grd_CountList.Columns[2].Visible = true;
                Grd_CountList.Columns[3].Visible = true;
                Grd_CountList.DataSource = CountDataSet;
                Grd_CountList.DataBind();
                Grd_CountList.Columns[2].Visible = false;
                Grd_CountList.Columns[3].Visible = false;
                ViewState["CountList"] = CountDataSet;
                Session["mode"] = student_mode;
                Session["Batch_Name"] = Drplist_batch.SelectedItem.Text.ToString();
                Img_Excel.Enabled = true;
                Img_Excel.Visible = true;
            }
            else
            {
                Img_Excel.Visible = false;
                Grd_CountList.DataSource = null;
                Grd_CountList.DataBind();
                Img_Excel.Enabled = false;
                Lbl_msg.Text = "No Class Found";
                this.MPE_MessageBox.Show();
            }
        }

        private void GetTotalAndGenderBasedCountForClass(int student_mode,string _ClassId,int Batch_id, out int _Count, out int _Male, out int _Female)
        {
            _Count = 0; _Male = 0; _Female = 0;
            string sql = "";
            if (student_mode == 0)
            {
               sql = "select (select count(tblstudentclassmap.StudentId) from tblstudentclassmap inner join tblstudent on tblstudentclassmap.StudentId= tblstudent.Id WHERE tblstudentclassmap.ClassId=" + _ClassId + " AND tblstudent.`Status`=1) as TotalCount,  (select count(tblstudentclassmap.StudentId) from tblstudentclassmap inner join tblstudent on tblstudentclassmap.StudentId= tblstudent.Id  WHERE tblstudentclassmap.ClassId=" + _ClassId + " and tblstudent.Sex='Male' and tblstudent.`Status`=1) as MaleCount, (select count(tblstudentclassmap.StudentId) from tblstudentclassmap inner join tblstudent on tblstudentclassmap.StudentId= tblstudent.Id  WHERE tblstudentclassmap.ClassId=" + _ClassId + " and tblstudent.Sex='Female' and tblstudent.`Status`=1) as FemaleCount";
            }
            else if (student_mode == 1)
            {
               //sql="select (select count(tblstudentclassmap_history.StudentId) from tblstudentclassmap_history inner join tblstudent_history on tblstudentclassmap_history.StudentId= tblstudent_history.Id WHERE tblstudentclassmap_history.ClassId=" + _ClassId + ") as TotalCount,  (select count(tblstudentclassmap_history.StudentId) from tblstudentclassmap_history inner join tblstudent_history on tblstudentclassmap_history.StudentId= tblstudent_history.Id  WHERE tblstudentclassmap_history.ClassId=" + _ClassId + " and tblstudent_history.Sex='Male') as MaleCount, (select count(tblstudentclassmap_history.StudentId) from tblstudentclassmap_history inner join tblstudent_history on tblstudentclassmap_history.StudentId= tblstudent_history.Id  WHERE tblstudentclassmap_history.ClassId=" + _ClassId + " and tblstudent_history.Sex='Female') as FemaleCount";
                if (Batch_id == 0 || Batch_id == -1)
                {
                    sql = "select (select count(Id) from tblstudent_history WHERE ClassId=" + _ClassId + ") as TotalCount,  (select count(Id) from tblstudent_history  WHERE ClassId=" + _ClassId + " and Sex='Male') as MaleCount, (select count(Id) from tblstudent_history  WHERE ClassId=" + _ClassId + " and Sex='Female') as FemaleCount";
                }
                else
                {
                    sql = "select (select count(Id) from tblstudent_history WHERE ClassId=" + _ClassId + " and BatchId=" + Batch_id + ") as TotalCount,  (select count(Id) from tblstudent_history  WHERE ClassId=" + _ClassId + " and BatchId=" + Batch_id + " and Sex='Male') as MaleCount, (select count(Id) from tblstudent_history  WHERE ClassId=" + _ClassId + " and BatchId=" + Batch_id + " and Sex='Female') as FemaleCount";
                }
            }
            else
            {
                sql = "select (select count(Id) from tbltempstdent WHERE Class=" + _ClassId + ") as TotalCount,(select count(Id) from tbltempstdent WHERE Class=" + _ClassId + " and Gender='Male') as MaleCount, (select count(Id) from tbltempstdent WHERE Class=" + _ClassId + " and Gender='Female') as FemaleCount";
            }
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _Count = int.Parse(MyReader.GetValue(0).ToString());
                _Male = int.Parse(MyReader.GetValue(1).ToString());
                _Female = int.Parse(MyReader.GetValue(2).ToString());
            }
        }
        //sai added for history and register students
        protected void Rdb_student_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(Rdb_student_type.SelectedValue.ToString()) == 1)
            {
                Drplist_batch.Visible = true ;
                lbl_batch.Visible = true;
            }
            else
            {
                Drplist_batch.Visible = false;
                lbl_batch.Visible =false;
            }
          

        }
        //sai modified for history students batch wise
        protected void Btn_show_Click(object sender,EventArgs e)
        {
            try
            {
                LoadAllStudentCountInAllClasses(int.Parse(Rdb_student_type.SelectedValue), int.Parse(Drplist_batch.SelectedValue.ToString()));
            }
            catch (Exception ew)
            {
                Lbl_msg.Text =ew.Message;
            }
            
        }

        //
        protected void Img_Excel_Click(object sender, ImageClickEventArgs e)
        {
            DataSet _CountReportList = new DataSet();
            _CountReportList = (DataSet)ViewState["CountList"];
            string _ReportName="";
            if (int.Parse(Session["mode"].ToString()) == 0)
            {
                _ReportName = "Current Batch Count Report";
            }
            else if (int.Parse(Session["mode"].ToString()) == 1)
            {
                _ReportName = "History("+  Session["Batch_Name"].ToString()+")  Count Report";
            }
            else
            {
                _ReportName = "Resister Count Report";
            }
            //if (!WinEr.ExcelUtility.ExportDataSetToExcel(_CountReportList, "CountReport.xls"))
            //{
            //    Lbl_msg.Text = "This function need Ms office";
            //    this.MPE_MessageBox.Show();
            //}
            string FileName = "CountReport";
            
            if (!WinEr.ExcelUtility.ExportDataToExcel(_CountReportList, _ReportName, FileName, MyUser.ExcelHeader))
            {

                Lbl_msg.Text = "This function need Ms office";
                this.MPE_MessageBox.Show();
            }
        }
        //load batches to drop down list
        private void Load_Batches()
        {
            Drplist_batch.Items.Clear();
            string sql = " select tblbatch.Id , tblbatch.BatchName from tblbatch where Created=1 and Status<>1";

            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drplist_batch.Items.Add(new ListItem("ALL", "0"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drplist_batch.Items.Add(li);
                }
            }
            else
            {
                Drplist_batch.Items.Add(new ListItem("No Batch exists", "-1"));
            }
        }

    }
}
