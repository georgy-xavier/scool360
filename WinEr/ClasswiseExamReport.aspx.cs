using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;
using System.IO;
using WinBase;


namespace WinEr
{
    public partial class ClasswiseExamReport : System.Web.UI.Page
    {
        private OdbcDataReader m_MyReader = null;
        private ClassOrganiser MyClassMang;
        private KnowinUser MyUser;
        private ExamManage MyExamMang;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet;
        private DataSet MyExamDataSet = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            //if (Session["ClassId"] == null)
            //{
            //    Response.Redirect("LoadClassDetails.aspx");
            //}
            MyUser = (KnowinUser)Session["UserObj"];
            MyClassMang = MyUser.GetClassObj();
            MyExamMang = MyUser.GetExamObj();
            if (MyClassMang == null)
            {
                Response.Redirect("sectionerr.htm");
                //no rights for this user.
            }
            else if (MyExamMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(77))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (!IsPostBack)
                {
                    Drp_Exam.Enabled = false;
                    Btn_Generate.Enabled = false;
                    Drp_Period.Enabled = false;
                    //some initlization
                    LoadExamType();
                    Img_Export.Enabled = false;


                    if (Session["ClassId"] != null)
                    {
                        LoadAllClassDetailsToDropDown(int.Parse(Session["ClassId"].ToString()));
                    }
                    else
                    {
                        LoadAllClassDetailsToDropDown(0);
                    }
                }
            }

        }

        private void LoadAllClassDetailsToDropDown(int _ClassId)
        {
            Drp_ClassSelect.Items.Clear();

            string sql = "SELECT tblclass.Id,tblclass.ClassName FROM tblclass order by tblclass.Standard, tblclass.ClassName"; 
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_ClassSelect.Items.Add(li);

                }
                Drp_ClassSelect.SelectedValue = _ClassId.ToString();
            }
            else
            {
                ListItem li = new ListItem("No Standard Found", "-1");
                Drp_ClassSelect.Items.Add(li);
            }

        }

        private void LoadExamType()
        {

            Drp_ExamType.Items.Clear();
            string sql = "select tblsubject_type.Id, tblsubject_type.sbject_type from tblsubject_type";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_Exam.Enabled = true;
                Btn_Generate.Enabled = false;
                Drp_ExamType.Items.Add(new ListItem("Select any exam type", "0"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_ExamType.Items.Add(li);
                }

            }
            else
            {
                Drp_ExamType.Items.Add(new ListItem("No exam type found", "0"));
                Drp_Exam.Enabled = false;
                Btn_Generate.Enabled = false;
                Drp_Period.Enabled = false;
            }
        }

        protected void Drp_ExamType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Img_Export.Enabled = false;
            EportGrid.DataSource = null;
            EportGrid.DataBind();
            if (int.Parse(Drp_ExamType.SelectedValue) != 0)
            {

                LoadExamToDrpList();
            }
            else
            {
                Drp_Exam.Items.Clear();
            }
        }

        private void LoadExamToDrpList()
        {

            Drp_Exam.Items.Clear();
            string sql = "select distinct tblexammaster.Id, tblexammaster.ExamName from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster ON tblexammaster.Id= tblclassexam.ExamId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id  where tblclassexam.ClassId = " + int.Parse(Drp_ClassSelect.SelectedValue) + " and tblexamschedule.BatchId = " + MyUser.CurrentBatchId + " and tblexammaster.ExamTypeId= " + int.Parse(Drp_ExamType.SelectedValue) + "";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_Period.Enabled = true;
                Btn_Generate.Enabled = false;
                Drp_Exam.Items.Add(new ListItem("Select any exam", "0"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Exam.Items.Add(li);
                }

            }
            else
            {
                Drp_Exam.Items.Add(new ListItem("No exam found", "0"));
                Btn_Generate.Enabled = false;
            }
        }

        protected void Drp_Exam_SelectedIndexChanged(object sender, EventArgs e)
        {
            Img_Export.Enabled = false;
            EportGrid.DataSource = null;
            EportGrid.DataBind();
            LoadExamPeriods();
        }

        private void LoadExamPeriods()
        {
            Drp_Period.Items.Clear();
            string sql = "";// "select distinct tblperiod.Id , tblperiod.Period from tblexammaster inner join tblperiod on tblperiod.Id = tblexammaster.PeriodTypeId inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id and tblclassexam.ClassId =" + int.Parse(Session["ClassId"].ToString()) + " inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.Id and tblexamschedule.BatchId= " + MyUser.CurrentBatchId + " inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblexammaster.Id = " + int.Parse(Drp_Exam.SelectedValue) + "";
            sql = "select distinct tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId and tblclassexam.ClassId =" + int.Parse(Drp_ClassSelect.SelectedValue) + " and tblclassexam.ExamId=" + int.Parse(Drp_Exam.SelectedValue) + " inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblexamschedule.BatchId=" + MyUser.CurrentBatchId;
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_Generate.Enabled = false;
                Drp_Period.Items.Add(new ListItem("Select any period", "0"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Period.Items.Add(li);
                }

            }
            else
            {
                Drp_Period.Items.Add(new ListItem("No period found", "0"));
                Btn_Generate.Enabled = false;
            }
        }

        protected void Drp_Period_SelectedIndexChanged(object sender, EventArgs e)
        {
            Img_Export.Enabled = false;
            EportGrid.DataSource = null;
            EportGrid.DataBind();
            if (Drp_Period.SelectedValue != "0")
            {
                Btn_Generate.Enabled = true;
            }
            else
            {
                Btn_Generate.Enabled = false;
            }
        }

        protected void Btn_Generate_Click(object sender, EventArgs e)
        {
            int i_ExamId = int.Parse(Drp_Exam.SelectedValue);
            int Periodid = int.Parse(Drp_Period.SelectedValue);
            DataSet MyExamData = GetExamData(Periodid, i_ExamId, MyUser.CurrentBatchId);
            EportGrid.DataSource = MyExamData;
            EportGrid.DataBind();
            Img_Export.Enabled = true;
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Generate Marksheet", ""+Drp_ClassSelect.SelectedItem.Text+" Mark sheet generated", 1);
        }

        private DataSet GetExamData(int _Periodid, int i_ExamId, int _BatchId)
        {
            DataSet ExamDataSet = new DataSet();
            DataTable dt;
            DataRow dr;
            int i, j;

            try
            {
                ExamDataSet.Tables.Add(new DataTable("Exam"));
                dt = ExamDataSet.Tables["Exam"];

                DataSet Subjects = MyClassMang.GetSubjectList(int.Parse(Drp_ClassSelect.SelectedValue), i_ExamId, _BatchId);
                string[] name = new string[100];
                if (Subjects != null && Subjects.Tables != null && Subjects.Tables[0].Rows.Count > 0)
                {

                    dt.Columns.Add("StudentName");
                    name[0] = "StudentName";
                    i = 1;

                    foreach (DataRow subject in Subjects.Tables[0].Rows)
                    {
                        dt.Columns.Add(subject[1].ToString());
                        name[i] = subject[1].ToString();
                        i++;
                    }

                    dt.Columns.Add("TotalMark");
                    dt.Columns.Add("ObtainedMark");
                    dt.Columns.Add("Avg");
                    dt.Columns.Add("Grade");
                    dt.Columns.Add("Result");
                    dt.Columns.Add("Rank");

                    dr = ExamDataSet.Tables["Exam"].NewRow();
                    dr["StudentName"] = " ";

                    j = 0;
                    foreach (DataRow subject in Subjects.Tables[0].Rows)
                    {
                        j++;
                        dr[name[j]] = " ";
                    }
                    dr["TotalMark"] = " ";
                    dr["ObtainedMark"] = " ";
                    dr["Avg"] = " ";
                    dr["Grade"] = " ";
                    dr["Result"] = " ";
                    dr["Rank"] = " ";

                    ExamDataSet.Tables["Exam"].Rows.Add(dr);


                    DataSet Students = MyClassMang.GetStudentMarkDetails(int.Parse(Drp_ClassSelect.SelectedValue), i_ExamId, _BatchId, _Periodid);
                    if (Students != null && Students.Tables != null && Students.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr_Student in Students.Tables[0].Rows)
                        {
                            double _SubMark = 0, _TotalMark = 0, Avg = 0;
                            dr = ExamDataSet.Tables["Exam"].NewRow();
                            j = 0;
                            dr[name[j]] = dr_Student[0].ToString();
                            foreach (DataRow subject in Subjects.Tables[0].Rows)
                            {
                                j++;
                                double.TryParse(dr_Student[j].ToString(), out _SubMark);
                                if (_SubMark == -1)
                                {
                                    dr[name[j]] = "A";
                                }
                                else
                                {
                                    dr[name[j]] = (Math.Round(_SubMark, 2)).ToString();
                                }
                            }
                            double.TryParse(dr_Student[j + 2].ToString(), out _TotalMark);
                            double.TryParse(dr_Student[j + 3].ToString(), out Avg);
                            dr["TotalMark"] = dr_Student[j + 1].ToString();
                            dr["ObtainedMark"] = (Math.Round(_TotalMark, 2)).ToString();
                            dr["Avg"] = (Math.Round(Avg, 2)).ToString();
                            dr["Grade"] = dr_Student[j + 4].ToString();
                            dr["Result"] = dr_Student[j + 5].ToString();
                            dr["Rank"] = dr_Student[j + 6].ToString();
                            ExamDataSet.Tables["Exam"].Rows.Add(dr);
                        }
                    }
                }
            }
            catch
            {

            }
            return ExamDataSet;
        }

        protected void Img_Export_Click(object sender, ImageClickEventArgs e)
        {
            int i_ExamId = int.Parse(Drp_Exam.SelectedValue);
            int Periodid = int.Parse(Drp_Period.SelectedValue);
            DataSet Subjects = MyClassMang.GetSubjectList(int.Parse(Drp_ClassSelect.SelectedValue), i_ExamId, MyUser.CurrentBatchId);
            string[] name = new string[100];
            DataSet ExportDataSet = new DataSet();
            DataTable dt;
            DataRow dr;
            int i = 0;
            int j = 0;
            ExportDataSet.Tables.Add(new DataTable("Exam"));
            dt = ExportDataSet.Tables["Exam"];
            dt.Columns.Add("StudentName");
            name[0] = "StudentName";
            i = 1;
            foreach (DataRow subject in Subjects.Tables[0].Rows)
            {
                dt.Columns.Add(subject[1].ToString());
                name[i] = subject[1].ToString();
                i++;
            }
            dt.Columns.Add("TotalMark");
            dt.Columns.Add("ObtainedMark");
            dt.Columns.Add("Avg");
            dt.Columns.Add("Grade");
            dt.Columns.Add("Result");
            dt.Columns.Add("Rank");

            name[i] = "TotalMark";
            name[i + 1] = "ObtainedMark";
            name[i + 2] = "Avg";
            name[i + 3] = "Grade";
            name[i + 4] = "Result";
            name[i + 5] = "Rank";
            foreach (GridViewRow gvr in EportGrid.Rows)
            {
                i = 0;
                dr = ExportDataSet.Tables["Exam"].NewRow();
                for (j = 0; j < gvr.Cells.Count; ++j)
                {
                    dr[name[i]] = gvr.Cells[j].Text;
                    i++;
                    
                }
                ExportDataSet.Tables["Exam"].Rows.Add(dr);
            }
            string FileName = "Class-wiseExamReport";
            string _ReportName = "Class-wiseExamReport";
            if (!WinEr.ExcelUtility.ExportDataToExcel(ExportDataSet, _ReportName, FileName, MyUser.ExcelHeader))
            {
                Lbl_Message.Text = "This function need Ms office";
            }
        }

        protected void GrdExam_Sort(object sender, GridViewSortEventArgs e)
        {
            int i_ExamId = int.Parse(Drp_Exam.SelectedValue);
            int Periodid = int.Parse(Drp_Period.SelectedValue);
            string[] name = new string[100];
            DataSet Subjects = MyClassMang.GetSubjectList(int.Parse(Drp_ClassSelect.SelectedValue), i_ExamId, MyUser.CurrentBatchId);
           
            DataTable dt = new DataTable();
            dt.Columns.Add("StudentName");
            name[0] = "StudentName";
            int j = 1;
            foreach (DataRow subject in Subjects.Tables[0].Rows)
            {
                name[j] = subject[1].ToString();
                dt.Columns.Add(subject[1].ToString());
                j++;
            }

            dt.Columns.Add("TotalMark");
            dt.Columns.Add("ObtainedMark");
            dt.Columns.Add("Avg");
            dt.Columns.Add("Grade");
            dt.Columns.Add("Result");
            dt.Columns.Add("Rank");

            name[j] = "TotalMark";
            name[j + 1] = "ObtainedMark";
            name[j + 2] = "Avg";
            name[j + 3] = "Grade";
            name[j + 4] = "Result";
            name[j + 5] = "Rank";
           
            foreach (GridViewRow gvr in EportGrid.Rows)
            {
                j = 0;
                DataRow dr = dt.NewRow();
                for (int i = 0; i < gvr.Cells.Count; ++i)
                {
                    dr[name[j]] = gvr.Cells[i].Text;
                    j++;
                }
                dt.Rows.Add(dr);
            }
            if (dt != null)
            {

                DataView dataView = new DataView(dt);
                dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);

                EportGrid.DataSource = dataView;
                EportGrid.DataBind();
            }
        }

        private string ConvertSortDirectionToSql(SortDirection sortDirection)
        {
            string newSortDirection = String.Empty;

            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    newSortDirection = "ASC";
                    break;
                
                case SortDirection.Descending:
                    newSortDirection = "DESC";
                    break;
            }
            return newSortDirection;
        }

        protected void Drp_ClassSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadExamType();
        }

    }
}
