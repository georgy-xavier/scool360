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
    public partial class ExamDashboard : System.Web.UI.Page
    {
        private ExamManage MyExamMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
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
                    Load_DropDown();
                    load_Grid();
                }
            }
        }



        private void Load_DropDown()
        {
            Load_DrpClass();
            Load_DrpExam();
        }



        private void Load_DrpExam()
        {
            Drp_Exam.Items.Clear();
            string sql = "SELECT tblexammaster.ExamName,tblexammaster.Id FROM tblexammaster WHERE tblexammaster.`Status`=1 ORDER BY tblexammaster.ExamName";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_Exam.Items.Add(new ListItem("Any Exam", "-1"));
                while (MyReader.Read())
                {

                    ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_Exam.Items.Add(li);
                }
            }
            else
            {
                Drp_Exam.Items.Add(new ListItem("No Exam Found", "-1"));
            }
            Drp_Exam.SelectedIndex = 0;
        }



        private void Load_DrpClass()
        {
            Drp_Class.Items.Clear();
            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Drp_Class.Items.Add(new ListItem("Any Class", "-1"));
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Class Found", "-1");
                Drp_Class.Items.Add(li);
            }
            Drp_Class.SelectedIndex = 0;
        }



        protected void Btn_Load_Click(object sender, EventArgs e)
        {
            load_Grid();
           
        }

        private void load_Grid()
        {
            Lbl_msg.Text = "";
            MydataSet = GetExamDataSet();
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_ExamDashboard.Columns[0].Visible = true;
                Grd_ExamDashboard.Columns[1].Visible = true;
                Grd_ExamDashboard.Columns[2].Visible = true;
                Grd_ExamDashboard.Columns[3].Visible = true;
                Grd_ExamDashboard.Columns[4].Visible = true;

                Grd_ExamDashboard.DataSource = MydataSet;
                Grd_ExamDashboard.DataBind();

                Grd_ExamDashboard.Columns[0].Visible = false;
                Grd_ExamDashboard.Columns[1].Visible = false;
                Grd_ExamDashboard.Columns[2].Visible = false;
                Grd_ExamDashboard.Columns[3].Visible = false;
                Grd_ExamDashboard.Columns[4].Visible = false;
            }
            else
            {
                Lbl_msg.Text = "No Exam Details Found";
                Grd_ExamDashboard.DataSource = null;
                Grd_ExamDashboard.DataBind();
            }
        }

        private DataSet GetExamDataSet()
        {
            string sql_sub = "";
            if (Drp_Class.SelectedValue != "-1")
            {
                sql_sub = sql_sub + " AND tblclass.Id=" + Drp_Class.SelectedValue;
            }

            if (Drp_Exam.SelectedValue != "-1")
            {
                sql_sub = sql_sub + " AND tblexammaster.Id=" + Drp_Exam.SelectedValue;
            }

            if (Drp_Status.SelectedValue != "0")
            {
                sql_sub = sql_sub + " AND tblexamschedule.Published=0";
            }

            DataSet ExamDataSet = new DataSet();
            ExamDataSet.Tables.Add(new DataTable("EXAM"));
            DataTable dt;
            DataRow dr;
            dt = ExamDataSet.Tables["EXAM"];
            dt.Columns.Add("ExamScheduleId");
            dt.Columns.Add("ExamId");
            dt.Columns.Add("ClassId");
            dt.Columns.Add("PeriodId");
            dt.Columns.Add("Page");

            dt.Columns.Add("ExamName");
            dt.Columns.Add("ClassName");
            dt.Columns.Add("Status");
            dt.Columns.Add("Description");
            string sql = "SELECT tblexamschedule.Id,tblexammaster.ExamName,tblclass.ClassName,tblexamschedule.`Status`,tblclassexam.ExamId,tblclassexam.ClassId,tblexamschedule.PeriodId  FROM tblexamschedule INNER JOIN tblclassexam ON tblexamschedule.ClassExamId=tblclassexam.Id INNER JOIN tblexammaster ON tblclassexam.ExamId=tblexammaster.Id INNER JOIN tblclass ON tblclassexam.ClassId=tblclass.Id WHERE tblclassexam.`Status`=1 AND tblexammaster.`Status`=1  AND tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " " + sql_sub;
            OdbcDataReader MyPayReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyPayReader.HasRows)
            {
                while (MyPayReader.Read())
                {
                    string ExamScheduleId = MyPayReader.GetValue(0).ToString();
                    string ExamId = MyPayReader.GetValue(4).ToString();
                    string ClassId = MyPayReader.GetValue(5).ToString();
                    string PeriodId = MyPayReader.GetValue(6).ToString();
                    string _page = "";

                    string _Description = GetExamDescription(ExamScheduleId, ExamId, ClassId, PeriodId, MyPayReader.GetValue(3).ToString());
                    string _Status = GetExamStatus(MyPayReader.GetValue(3).ToString(),out _page);

                    dr = ExamDataSet.Tables["EXAM"].NewRow();

                    dr["ExamScheduleId"] = ExamScheduleId;
                    dr["ExamId"] = ExamId;
                    dr["ClassId"] = ClassId;
                    dr["PeriodId"] = PeriodId;
                    dr["Page"] = _page;

                    dr["ExamName"] = MyPayReader.GetValue(1).ToString();
                    dr["ClassName"] = MyPayReader.GetValue(2).ToString();
                    dr["Status"] = _Status;
                    dr["Description"] = _Description;
                    ExamDataSet.Tables["EXAM"].Rows.Add(dr);
                }
               
            }

            return ExamDataSet;
        }

        private string GetExamStatus(string _type, out string Page)
        {
            string _Status = "";
            Page = "ExamDetails.aspx";

            if (_type == "Updated")
            {
                _Status = "Mark Updated";
                Page = "ExamReport.aspx";
            }
            else if (_type == "Scheduled")
            {
                _Status = "Exam Scheduled";
                Page = "EnterMark.aspx";
            }
            else if (_type == "Completed")
            {
                _Status = "Report Generated";
                Page = "ExamDetails.aspx";
            }

            return _Status;
        }

        private string GetExamDescription(string ExamScheduleId, string ExamId, string ClassId, string PeriodId,string Status)
        {
            string Description = "";
            string _Subjects="";
            if (!CheckValidMarks(ExamId, ClassId, PeriodId, out _Subjects))
            {
                Description = "All marks are not entered for subjects: " + _Subjects.Trim().TrimEnd(',');
            }
            else if (Status == "Updated")
            {
                Description = "Exam report not generated.";
            }
            return Description;
        }




        private bool CheckValidMarks(string ExamId, string ClassId, string PeriodId, out string _Subjects)
        {
            _Subjects = "";
            bool _valid = true;
            string sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudentclassmap.RollNo from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND tblstudentclassmap.ClassId=" + ClassId + " Order by tblstudentclassmap.RollNo ASC";
            MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    sql = " SELECT tblexammark.MarkColumn,tblsubjects.subject_name FROM tblexammark inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblsubjects on tblsubjects.Id=tblexammark.SubjectId inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId where tblclassexam.ExamId=" + ExamId + " AND tblclassexam.ClassId=" + ClassId + " AND tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " AND tblexamschedule.PeriodId=" + PeriodId+" order by tblexammark.SubjectOrder";
                    MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        string _Str_Mark;
                        while (MyReader.Read())
                        {
                            _Str_Mark = MyExamMang.GetMarks(int.Parse(dr[0].ToString()), int.Parse(ExamId), MyUser.CurrentBatchId, MyReader.GetValue(0).ToString(), ClassId, PeriodId);
                            if (_Str_Mark == "")
                            {
                                _Subjects = _Subjects + MyReader.GetValue(1).ToString() + ", ";
                                _valid = false;
                            }
                        }
                    }
                    if (_valid == false)
                    {
                        break;
                    }
                }
            }
            return _valid;
        }

        protected void Grd_ExamDashboard_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _page = Grd_ExamDashboard.SelectedRow.Cells[4].Text.ToString();
            Session["ExamId"] = Grd_ExamDashboard.SelectedRow.Cells[1].Text.ToString();
            Session["ClassId"] = Grd_ExamDashboard.SelectedRow.Cells[2].Text.ToString();
            Session["PeriodId"] = Grd_ExamDashboard.SelectedRow.Cells[3].Text.ToString();

            Response.Redirect(_page);
        }

        protected void Grd_ExamDashboard_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_ExamDashboard.PageIndex = e.NewPageIndex;
            load_Grid();
        }
    }
}
