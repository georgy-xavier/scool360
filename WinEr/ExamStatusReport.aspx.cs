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
    public partial class ExamStatusReport : System.Web.UI.Page
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
            MyUser = (KnowinUser)Session["UserObj"];
            MyClassMang = MyUser.GetClassObj();
            MyExamMang = MyUser.GetExamObj();
            if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MyExamMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(300))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (!IsPostBack)
                {
                    Btn_Generate.Enabled = false;

                    if (Session["ClassId"] != null)
                    {
                        LoadAllClassDetailsToDropDown(int.Parse(Session["ClassId"].ToString()));
                    }
                    else
                    {
                        LoadAllClassDetailsToDropDown(0);
                    }

                    LoadExamType();
                    Img_Export.Enabled = false;
                    lbl_Label.Visible = false;
                  //  LoadSubjectsToDropDown();
                    //LoadSearchTypeToDropDown();
                    LoadExamToDrpList();
                    LoadExamPeriods();
                }
            }
        }

        //private void LoadSubjectsToDropDown()
        //{
        //    Drp_Subject.Items.Clear();
        //    string sql = "SELECT tblsubjects.subject_name, tblsubjects.Id from tblsubjects INNER JOIN tblclasssubmap on  tblclasssubmap.SubjectId= tblsubjects.Id where tblclasssubmap.ClassId=" + int.Parse(Drp_ClassSelect.SelectedValue);
        //    MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
        //    if (MyReader.HasRows)
        //    {
        //        Drp_Subject.Items.Add(new ListItem("ALL", "0"));
        //        while (MyReader.Read())
        //        {
        //            ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
        //            Drp_Subject.Items.Add(li);
        //        }

        //    }
        //    else
        //    {
        //        Drp_Subject.Items.Add(new ListItem("No Subject found", "0"));
        //        Btn_Generate.Enabled = false;
        //    }
        //}

        //private void LoadSearchTypeToDropDown()
        //{
        //    if (Drp_Mode.SelectedItem.Text == "ALL")
        //    {
        //        Drp_Type.Items.Clear();
        //        Drp_Type.Items.Add(new ListItem("ALL", "0"));
        //        Drp_Type.Items.Add(new ListItem("Passed", "1"));
        //        Drp_Type.Items.Add(new ListItem("Failed", "2"));
        //        Drp_Type.Items.Add(new ListItem("Present", "3"));
        //        Drp_Type.Items.Add(new ListItem("Absent", "4"));
        //    }
        //    else if (Drp_Mode.SelectedItem.Text == "Mark")
        //    {
        //        Drp_Type.Items.Clear();
        //        Drp_Type.Items.Add(new ListItem("ALL", "0"));
        //        Drp_Type.Items.Add(new ListItem("Passed", "1"));
        //        Drp_Type.Items.Add(new ListItem("Failed", "2"));
        //    }
        //    else
        //    {
        //        Drp_Type.Items.Clear();
        //        Drp_Type.Items.Add(new ListItem("ALL", "0"));
        //        Drp_Type.Items.Add(new ListItem("Present", "1"));
        //        Drp_Type.Items.Add(new ListItem("Absent", "2"));
        //    }
        //}

        private void LoadAllClassDetailsToDropDown(int _ClassId)
        {
            Drp_ClassSelect.Items.Clear();

            string sql = "SELECT tblclass.Id,tblclass.ClassName FROM tblclass order by tblclass.Standard,tblclass.ClassName";
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
                Btn_Generate.Enabled = false;
               // Drp_ExamType.Items.Add(new ListItem("Select any exam type", "0"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_ExamType.Items.Add(li);
                }

            }
            else
            {
                Drp_ExamType.Items.Add(new ListItem("No exam type found", "0"));
                Btn_Generate.Enabled = false;
            }
        }

        protected void Drp_ClassSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadExamToDrpList();
            LoadExamPeriods();
        }

        protected void Drp_ExamType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Img_Export.Enabled = false;
            LoadExamToDrpList();
            LoadExamPeriods();
        }

        private void LoadExamToDrpList()
        {
            Drp_Exam.Items.Clear();
            string sql = "select distinct tblexammaster.Id, tblexammaster.ExamName from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster ON tblexammaster.Id= tblclassexam.ExamId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id  where tblclassexam.ClassId = " + int.Parse(Drp_ClassSelect.SelectedValue) + " and tblexamschedule.BatchId = " + MyUser.CurrentBatchId + " and tblexammaster.ExamTypeId= " + int.Parse(Drp_ExamType.SelectedValue) + "";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_Generate.Enabled = true;
               // Drp_Exam.Items.Add(new ListItem("Select any exam", "0"));
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
            //EportGrid.DataSource = null;
            //EportGrid.DataBind();
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
                Btn_Generate.Enabled = true;
                //Drp_Period.Items.Add(new ListItem("Select any period", "0"));
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
            //EportGrid.DataSource = null;
            //EportGrid.DataBind();
            if (Drp_Period.SelectedValue != "0")
            {
                Btn_Generate.Enabled = true;
            }
            else
            {
                Btn_Generate.Enabled = false;
            }
        }

        //protected void Drp_Mode_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    LoadSearchTypeToDropDown();
        //}

       

        protected void Btn_Generate_Click(object sender, EventArgs e)
        {
            int i_ExamId = int.Parse(Drp_Exam.SelectedValue);
            int Periodid = int.Parse(Drp_Period.SelectedValue);
            DataSet MyExamData = GetExamData(Periodid, i_ExamId, MyUser.CurrentBatchId);

            if (MyExamData!= null)
            {
                Lbl_Message.Text = "";
                ViewState["ClassStatusReport"] = MyExamData;
                EportGrid.DataSource = MyExamData;
                EportGrid.DataBind();
                Img_Export.Enabled = true;
            }
            else
            {
                EportGrid.DataSource = null;
                EportGrid.DataBind();
                Img_Export.Enabled = false;
                Lbl_Message.Text = "No Result found";
            }
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
                        // dt.Columns.Add(Periods[0].ToString());
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
                    //int k = i;
                    //for (j = i; j < k + 6; j++)
                    //{
                    //    dt.Columns.Add(i.ToString());
                    //    name[i] = i.ToString();
                    //    i++;
                    //}

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


                    DataSet Students =GetStudentMarkDetails(int.Parse(Drp_ClassSelect.SelectedValue), i_ExamId, _BatchId, _Periodid);
                    if (Students != null && Students.Tables != null && Students.Tables[0].Rows.Count > 0)
                    {
                        lbl_Label.Visible = true;
                        lbl_StudentsNumber.Text = Students.Tables[0].Rows.Count.ToString();
                        foreach (DataRow dr_Student in Students.Tables[0].Rows)
                        {
                            double _SubMark = 0, _TotalMark = 0, Avg = 0;
                            dr = ExamDataSet.Tables["Exam"].NewRow();
                            j = 0;
                            dr[name[j]] = dr_Student[0].ToString();
                            foreach (DataRow subject in Subjects.Tables[0].Rows)
                            {
                                j++;
                                double.TryParse(dr_Student[j].ToString(),out _SubMark); 
                                //string a = dr_Student[j].ToString();
                                if (_SubMark == -1)
                                {
                                    dr[name[j]] = "A";
                                }
                                else
                                {
                                    dr[name[j]] = (Math.Round(_SubMark,2)).ToString();
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
                    else
                    {
                        ExamDataSet = null;
                        lbl_Label.Visible = false;
                        lbl_StudentsNumber.Text = "";
                    }
                }
            }
            catch
            {

            }
            return ExamDataSet;
        }

        private DataSet GetStudentMarkDetails(int _ClassID, int i_ExamId, int _BatchId, int _Periodid)
        {
            DataSet Students = null;
            string Query = "", Query1 = "";
            int i = 0;
            int ScheduleId =GetExamSchid(_ClassID, i_ExamId, _BatchId, _Periodid);
            string sql = "select tblexammark.MarkColumn from tblexammark where tblexammark.ExamSchId=" + ScheduleId + " order by tblexammark.SubjectOrder";
            m_MyReader =MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    i++;
                    if (i > 1)
                    {
                        Query = Query + ",";
                        Query1 = Query1 + ",";
                    }
                    Query = Query + m_MyReader.GetValue(0).ToString();
                    Query1 = Query1 + "-1";
                }
            }

            string[] Present = null;
            Present = Query.Split(',');
            string _PresentCondn = "";
            string _AbsentCondition = "";
            int _TotalColumns = Present.Length;
            for (int j = 0; j < _TotalColumns; j++)
            {
                if (j > 0)
                {
                    _PresentCondn = _PresentCondn + " and ";
                    _AbsentCondition = _AbsentCondition + " or ";
                }
                _PresentCondn = _PresentCondn + Present[j] + ">0";
                _AbsentCondition = _AbsentCondition + Present[j] + "<0";
            }

            sql = "select DISTINCT tblstudent.StudentName, " + Query + ", tblstudentmark.TotalMax , tblstudentmark.TotalMark , tblstudentmark.`Avg`, Grade, Result, Rank  from tblstudentclassmap inner join tblstudent on tblstudentclassmap.StudentId = tblstudent.Id and tblstudentclassmap.BatchId=" + _BatchId + " and tblstudentclassmap.ClassId = " + _ClassID + " inner join tblstudentmark on tblstudentmark.StudId = tblstudentclassmap.StudentId where tblstudentmark.ExamSchId in (select tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblclassexam.Id =  tblexamschedule.ClassExamId  where tblexamschedule.PeriodId =" + _Periodid + " and tblexamschedule.BatchId = " + _BatchId + " and tblclassexam.ExamId=" + i_ExamId + " and tblclassexam.ClassId =" + _ClassID + ") and tblstudent.`Status`=1";

            if (Drp_Type.SelectedItem.Text == "ALL")
            {
                sql = sql + "";
            }
            else if (Drp_Type.SelectedItem.Text == "Passed")
            {
                sql = sql + " and tblstudentmark.Result!='Failed'";
            }
            else if (Drp_Type.SelectedItem.Text == "Failed")
            {
                sql = sql + " and tblstudentmark.Result='Failed'";
            }
            else if (Drp_Type.SelectedItem.Text == "Present")
            {
                sql = sql + " and (" + _PresentCondn + ")";
            }
            else if (Drp_Type.SelectedItem.Text == "Absent")
            {
                sql = sql + " and (" + _AbsentCondition + ")";
            }
           // sql = sql + " order by tblstudent.StudentName asc";

            if (Drp_Type.SelectedItem.Text == "ALL" || Drp_Type.SelectedItem.Text == "Failed" || Drp_Type.SelectedItem.Text == "Absent")
            {
                sql = sql + " union select DISTINCT tblstudent.StudentName, " + Query1 + ", 0 , 0 ,0, 'F', 'Failed', 0 from tblstudentclassmap inner join tblstudent on tblstudentclassmap.StudentId = tblstudent.Id and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " and tblstudentclassmap.ClassId =" + Drp_ClassSelect.SelectedValue + " and  tblstudentclassmap.StudentId not in ( select tblstudentmark.StudId from tblstudentmark  where tblstudentmark.ExamSchId in (select tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblclassexam.Id =  tblexamschedule.ClassExamId  where tblexamschedule.PeriodId =" + Drp_Period.SelectedValue + " and tblexamschedule.BatchId = " + MyUser.CurrentBatchId + " and tblclassexam.ExamId=" + Drp_Exam.SelectedValue + " and tblclassexam.ClassId =" + Drp_ClassSelect.SelectedValue + ")) and tblstudent.`Status`=1 ";
            }

            Students =MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return Students;
        }

        private int GetExamSchid(int _ClassID, int i_ExamId, int _BatchId, int _Period)
        {
            int SchedId = -2;
            string sql = "select tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId and tblclassexam.ClassId =" + _ClassID + " and tblclassexam.ExamId = " + i_ExamId + "  where tblexamschedule.BatchId =" + _BatchId + " and tblexamschedule.PeriodId =" + _Period + "";
            m_MyReader =MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                SchedId = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            return SchedId;
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

        protected void Img_Export_Click(object sender, ImageClickEventArgs e)
        {
            DataSet ExportDataSet = (DataSet)ViewState["ClassStatusReport"];
            //if (!WinEr.ExcelUtility.ExportDataSetToExcel(ExportDataSet, "ExamResult.xls"))
            //{
            //    Lbl_Message.Text = "This function need Ms office";
            //}
            string FileName = "ExamResult" ;
            string _ReportName = "ExamResult" ;
            if (!WinEr.ExcelUtility.ExportDataToExcel(ExportDataSet, _ReportName, FileName, MyUser.ExcelHeader))
            {
                Lbl_Message.Text = "This function need Ms office";
            }
        }

        
    }
}
