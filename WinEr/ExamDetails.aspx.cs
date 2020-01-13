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
using WinBase;
public partial class ExamDetails : System.Web.UI.Page
{
    private ExamManage MyExamMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    private OdbcDataReader MyReader1 = null;
    private DataSet MydataSet;
    private DataSet examdataset;
    private StudentManagerClass MyStudMang;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        if (Session["ExamId"] == null)
        {
            Response.Redirect("ViewExams.aspx");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyExamMang = MyUser.GetExamObj();
        MyStudMang = MyUser.GetStudentObj();
        if (MyExamMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }
        else if (MyStudMang == null)
        {
            Response.Redirect("RoleErr.htm");
        }
        else
        {
            if (!IsPostBack)
            {
                string _ExamMenuStr;            
                _ExamMenuStr = MyExamMang.GetSubExamMangMenuString(MyUser.UserRoleId);
                this.SubExammngMenu.InnerHtml = _ExamMenuStr;
                LoadExamGeneralDetails();
                LoadClassAndPerdiod();
                SelectClassAndPeriod();
                LoadExamReports();
                LoadSuppliReportConfig();
              
            }
        }
    }

    private void LoadExamReports()
    {
        lnk_manageExam.Visible=false;
        lnk_ScheduleExam.Visible = false;
        lnk_mark.Visible = false;
        lnk_report.Visible = false;
        int ifAssignToClass = 0;
        string _status =MyExamMang.ExamSchduledState(int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue);
        ifAssignToClass = MyExamMang.IfExamAssignedToAnyClass(int.Parse(Session["ExamId"].ToString()));
        if (ifAssignToClass == 0)
        {
            Lbl_Not.Text = "This exam is not mapped to any Class..!";
            if (MyUser.HaveActionRignt(76))
            {
               lnk_manageExam.Visible=true;
            }
            else
            {
                lnk_manageExam.Visible = false;
            }
            EnableNote();
            DisableExamReport();
            DisableExamSchedule();
            Btn_Publish.Visible = false;
        }
        else if (_status == "Exam Not Scheduled")
        {
            Lbl_Not.Text = "This exam is not scheduled for the selected class and period";
            if (MyUser.HaveActionRignt(6))
            {
                lnk_ScheduleExam.Visible = true;
            }
            else
            {
                lnk_ScheduleExam.Visible = false;
            }
            EnableNote();
            DisableExamReport();
            DisableExamSchedule();
            Btn_Publish.Visible = false;
        }

        else if (!MarksNotEntered())
        {
            Lbl_Not.Text = "Marks not Entered for this Exam";
            if (MyUser.HaveActionRignt(7))
            {
                lnk_mark.Visible = true;
            }
            else
            {
                lnk_mark.Visible = false;
            }
            LoadExamSchedule();
            DisableExamReport();
            Btn_Publish.Visible = false;
        }
        else if (!ResultNotGenerated())
        {
            Lbl_Not.Text = "Exam Report is not Generated";
            if (MyUser.HaveActionRignt(28))
            {
                lnk_report.Visible = true;
            }
            else
            {
                lnk_report.Visible = false;
            }
            LoadExamSchedule();
            DisableExamReport();
            Btn_Publish.Visible = false;
        }
        else if (_status == "Completed")
        {
            DisableNote();
            LoadExamSchedule();
            LoadStudReports();
            int ExamSchId = MyExamMang.GetExamScheduleId(int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue);
            if ((MyUser.HaveActionRignt(510)) && (MyExamMang.IsReadyForPublish(ExamSchId)))
            {
                Btn_Publish.Visible = true;
                Lbl_PublishMessage.Text = "The Exam result is not published";
            }
            else
            {
                Lbl_PublishMessage.Text = "The Exam result is published";
                Btn_Publish.Visible = false;
            }
        }
        else
        {
            DisableNote();
            LoadExamSchedule();
            DisableExamReport();
            Btn_Publish.Visible = false;
        }
    }

    private bool MarksNotEntered()
    {
        bool mark = false;
        mark = MyExamMang.WhetherMarksEntered(int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue);

        return mark;
    }

    private void LoadSuppliReportConfig()
    {
        int failcount = 0;
        string sql = "SELECT Value FROM  tblconfiguration WHERE Name='Supplimentary Report' AND Module = 'Exam Report'";
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            failcount = int.Parse(MyReader.GetValue(0).ToString());
        }
          if(failcount == 1)
          {
              Btn_supplimentaryexam.Visible = true;
          }
    }

    private bool ResultNotGenerated()
    {
        bool report = false;
        report = MyExamMang.WhetherReportGenerated(int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue);

        return report;
    }

    private void DisableExamSchedule()
    {
        Pnl_schedul.Visible = false;
    }

    protected void Grd_CreateReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string value = e.Row.Cells[2].Text;
            if (value == "-1")
            {
                e.Row.Cells[2].Text = "No Roll No.";
            }
        }
    }
   
    private void LoadExamSchedule()
    {
        string sql = "select tblsubjects.subject_name, tblsubjects.SubjectCode,date_format(tblexammark.ExamDate,'%d/%m/%Y') AS `ExamDate` , tbltimeslot.StartTime, tbltimeslot.EndTime, tblclassexamsubmap.MinMark , tblclassexamsubmap.MaxMark from tblclassexam inner join tblexamschedule on tblclassexam.Id= tblexamschedule.ClassExamId AND tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " AND tblexamschedule.PeriodId=" + DrpExamPeriod.SelectedValue + " inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblclassexam.Id inner join tblexammark on tblexammark.ExamSchId= tblexamschedule.Id AND tblexammark.SubjectId= tblclassexamsubmap.SubId inner join tbltimeslot on tbltimeslot.Id= tblexammark.TimeSlotId inner join tblsubjects on tblsubjects.Id= tblexammark.SubjectId where tblclassexam.ClassId=" + DrpClassName.SelectedValue + " AND tblclassexam.ExamId=" + Session["ExamId"].ToString() + " order by tblexammark.ExamDate";
        MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
            Grd_ExamSchdule.DataSource = MydataSet;
            Grd_ExamSchdule.DataBind();
            Pnl_schedul.Visible = true;
        }
    }
    private void LoadStudReports()
    {
        string sql = "SELECT tblstudent.Id, tblstudent.StudentName, tblstudentclassmap.RollNo, tblstudentmark.TotalMark , tblstudentmark.TotalMax,ROUND(tblstudentmark.`Avg`, 2) AS `Avg`, tblstudentmark.Grade, tblstudentmark.Result, tblstudentmark.Rank, MID(tblstudentmark.Remark,1,10)AS Remark from tblexamschedule inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId inner join tblstudentmark on tblstudentmark.ExamSchId= tblexamschedule.Id inner join tblstudent on tblstudent.Id= tblstudentmark.StudId inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudentmark.StudId AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " And tblexamschedule.PeriodId=" + DrpExamPeriod.SelectedValue + " AND tblclassexam.ClassId=" + DrpClassName.SelectedValue + " AND tblclassexam.ExamId=" + Session["ExamId"].ToString() + " order by tblstudentclassmap.RollNo ASC";
        MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
            Grd_CreateReport.Columns[0].Visible = true;
            Grd_CreateReport.Columns[8].Visible = true;
            Grd_CreateReport.DataSource = MydataSet;
            Grd_CreateReport.DataBind();
           
            Grd_CreateReport.Columns[0].Visible = false;
            if (!MyExamMang.NeedRank)
            {
                Grd_CreateReport.Columns[8].Visible = false;
            }
            PanelExamStudaDetails.Visible = true;
        }
    }

    private void DisableExamReport()
    {
        PanelExamStudaDetails.Visible = false;
    }

    private void DisableNote()
    {
        Pnl_note.Visible = false;
        Lbl_Not.Text = "";
    }

    private void EnableNote()
    {
        Pnl_note.Visible = true;       
    }

    private void SelectClassAndPeriod()
    {
        if (Session["ClassId"] != null)
        {
            DrpClassName.SelectedValue = Session["ClassId"].ToString();
        }
        if (Session["PeriodId"] != null)
        {
            DrpExamPeriod.SelectedValue = Session["PeriodId"].ToString();
        }
    }
   
    private void LoadClassAndPerdiod()
    {
        LoadClass();
        LoadPeriod();
    }

    private void LoadPeriod()
    {
        DrpExamPeriod.Items.Clear();
        string sql = "SELECT tblperiod.Id, tblperiod.Period FROM tblexammaster inner join tblperiod  on tblexammaster.PeriodTypeId = tblperiod.FrequencyId WHERE tblexammaster.Id=" + int.Parse(Session["ExamId"].ToString());
        MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                DrpExamPeriod.Items.Add(li);
            }
        }
    }

    private void LoadClass()
    {
        DrpClassName.Items.Clear();
        MydataSet = MyExamMang.MyAssociatedExamClass(MyUser.UserId, int.Parse(Session["ExamId"].ToString()));
        if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in MydataSet.Tables[0].Rows)
            {
                ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                DrpClassName.Items.Add(li);
            }
        }
        else
        {
            ListItem li = new ListItem("No Class present", "-1");
            DrpClassName.Items.Add(li);
        }
        DrpClassName.SelectedIndex = 0;
    }

    private void LoadExamGeneralDetails()
    {
        string ExamNme, ExamType, examfreqncy;
        MyExamMang.GetExamDetail(int.Parse(Session["ExamId"].ToString()), out ExamNme, out ExamType, out examfreqncy);
        Lbl_ExamName.Text = ExamNme.ToString();
        Lbl_ExamType.Text = ExamType.ToString();
        Lbl_Frequency.Text = examfreqncy.ToString();
    }

    protected void DrpClassName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["ClassId"] = DrpClassName.SelectedValue;
        LoadExamReports();
    }

    protected void DrpExamPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["PeriodId"] = DrpExamPeriod.SelectedValue;
        LoadExamReports();
    }

    protected void Btn_exporttoexel_Click(object sender, ImageClickEventArgs e)
    {
        string sql = "select tblsubjects.subject_name AS `Subject Name`, tblsubjects.SubjectCode AS `Subject Code`, date_format(tblexammark.ExamDate,'%d/%m/%Y') AS `Exam Date`, tbltimeslot.StartTime AS `Start Time`, tbltimeslot.EndTime AS `End Time`, tblclassexamsubmap.MinMark AS `Pass Mark`, tblclassexamsubmap.MaxMark AS `Maximum Mark` from tblclassexam inner join tblexamschedule on tblclassexam.Id= tblexamschedule.ClassExamId AND tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " AND tblexamschedule.PeriodId=" + DrpExamPeriod.SelectedValue + " inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblclassexam.Id inner join tblexammark on tblexammark.ExamSchId= tblexamschedule.Id AND tblexammark.SubjectId= tblclassexamsubmap.SubId inner join tbltimeslot on tbltimeslot.Id= tblexammark.TimeSlotId inner join tblsubjects on tblsubjects.Id= tblexammark.SubjectId where tblclassexam.ClassId=" + DrpClassName.SelectedValue + " AND tblclassexam.ExamId=" + Session["ExamId"].ToString()+" order by tblexammark.SubjectOrder";
        MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
          
            string FileName = "TimeTable";
            string _ReportName = "TimeTable";
            if (!WinEr.ExcelUtility.ExportDataToExcel(MydataSet, _ReportName, FileName, MyUser.ExcelHeader))
            {

                Lbl_Not.Text = "This function need Ms office";
            }
        }
    }

    protected void Btn_Exp2_Click(object sender, ImageClickEventArgs e)
    {
        string sql = "SELECT  tblstudent.StudentName AS `Student Name`, tblstudentclassmap.RollNo, tblstudentmark.TotalMark AS `Mark Obtained` , tblstudentmark.TotalMax AS `Maximum Mark`, ROUND(tblstudentmark.`Avg`, 2) AS `Average`, tblstudentmark.Grade, tblstudentmark.Result, tblstudentmark.Rank, tblstudentmark.Remark from tblexamschedule inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId inner join tblstudentmark on tblstudentmark.ExamSchId= tblexamschedule.Id inner join tblstudent on tblstudent.Id= tblstudentmark.StudId inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudentmark.StudId AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " And tblexamschedule.PeriodId=" + DrpExamPeriod.SelectedValue + " AND tblclassexam.ClassId=" + DrpClassName.SelectedValue + " AND tblclassexam.ExamId=" + Session["ExamId"].ToString() + " order by tblstudentclassmap.RollNo ASC";
        MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
     
            if (!MyExamMang.NeedRank)
            {
                MydataSet.Tables[0].Columns.Remove(MydataSet.Tables[0].Columns[7]);
            }

            string FileName = "ExamResult";
            string _ReportName = "ExamResult";
            if (!WinEr.ExcelUtility.ExportDataToExcel(MydataSet, _ReportName, FileName, MyUser.ExcelHeader))
            {

                Lbl_Not.Text = "This function need Ms office";
            }
        }
    }
 



    protected void Grd_CreateReport_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sql = "SELECT tblstudent.Id, tblstudent.StudentName, tblstudentclassmap.RollNo, tblstudentmark.TotalMark , tblstudentmark.TotalMax, tblstudentmark.`Avg`, tblstudentmark.Grade, tblstudentmark.Result, tblstudentmark.Rank, MID(tblstudentmark.Remark,1,10) AS Remark from tblexamschedule inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId inner join tblstudentmark on tblstudentmark.ExamSchId= tblexamschedule.Id inner join tblstudent on tblstudent.Id= tblstudentmark.StudId inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudentmark.StudId AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " And tblexamschedule.PeriodId=" + DrpExamPeriod.SelectedValue + " AND tblclassexam.ClassId=" + DrpClassName.SelectedValue + " AND tblclassexam.ExamId=" + Session["ExamId"].ToString() + " order by tblstudentclassmap.RollNo ASC";
        MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
            DataTable dtAccountData = MydataSet.Tables[0];
            DataView dataView = new DataView(dtAccountData);
            dataView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            Grd_CreateReport.Columns[0].Visible = true;
            Grd_CreateReport.Columns[8].Visible = true;
            Grd_CreateReport.DataSource = dataView;
            Grd_CreateReport.DataBind();
            PanelExamStudaDetails.Visible = true;
            if (!MyExamMang.NeedRank)
            {
                Grd_CreateReport.Columns[8].Visible = false;
            }
            Grd_CreateReport.Columns[0].Visible = false;
        }
    }

    private string GetSortDirection(string column)
    {
        // By default, set the sort direction to ascending.
        string sortDirection = "ASC";
        // Retrieve the last column that was sorted.
        string sortExpression = Session["SortExpression"] as string;
        if (sortExpression != null)
        {
            // Check if the same column is being sorted.
            // Otherwise, the default value can be returned.
            if (sortExpression == column)
            {
                string lastDirection = Session["SortDirection"] as string;
                if ((lastDirection != null) && (lastDirection == "ASC"))
                {
                    sortDirection = "DESC";
                }
            }
        }
        // Save new values in ViewState.
        Session["SortDirection"] = sortDirection;
        Session["SortExpression"] = column;
        return sortDirection;
    }

    # region Publish

    protected void Btn_Publish_Click(object sender, EventArgs e)
    {
        int ExamSchId = MyExamMang.GetExamScheduleId(int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue);

        lbl_examschuleId.Text = ExamSchId.ToString();
        if (MyUser.HaveModule(20) || MyUser.HaveModule(23))
        {
            Chk_PublishIncident.Visible = true;
            Chk_PublishSMS.Visible = true;
            if (!MyUser.HaveModule(20))
            {
                Chk_PublishIncident.Visible = false;
            }
            if (!MyUser.HaveModule(23))
            {
                Chk_PublishSMS.Visible = false;
            }
            MPE_PublishExam.Show();
        }
       
    }


    protected void Btn_ok_Click(object sender, EventArgs e)
    {
        bool Done = false;
        if (Chk_PublishIncident.Checked)
        {
                StoreExamIncident(lbl_examschuleId.Text);
                Done = true;
        }
        if (Chk_PublishSMS.Checked)
        {
                StoreSMSMessage(lbl_examschuleId.Text);
                Done = true;
        }
        if (Done)
        {
            string sql = "update tblexamschedule set Published=1 where Id=" + lbl_examschuleId.Text;
            MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            Lbl_PublishMessage.Text = "The Exam result is published";
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Exam Result Published","Result of " +Lbl_ExamName.Text + " is published for " + DrpClassName.SelectedItem.Text, 1);

            Btn_Publish.Visible = false;
        }
        else
        {
            Message.Show();
        }
    }


    private void StoreExamIncident(string _scheduledid)
    {
        string sql = "";
        int i = 1, StudentId = 0, ExamTypeId = 0, StudentsCount = 0, Rank = 0, classId=0;
        string subjectStr = "", ExamName = "", Grade = "",Result="";
 
        bool _continue = true;

        sql = "SELECT StudId,Mark1,Mark2,Mark3,Mark4,Mark5,Mark6,Mark7,Mark8,Mark9,Mark10,Mark11,Mark12,Mark13,Mark14,Mark15,Mark16,Mark17,Mark18,Mark19,Mark20,TotalMark,TotalMax,Avg,Grade,Result,Rank,Remark FROM tblstudentmark WHERE ExamSchId=" + _scheduledid;
        MyReader1 = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader1.HasRows)
        {
            while (MyReader1.Read())
            {
                _continue = true;
                i = 1;
                StudentId = 0; ExamTypeId = 0; StudentsCount = 0; Rank = 0;
                subjectStr = ""; ExamName = "";
                Grade = MyReader1.GetValue(24).ToString();
                Result = MyReader1.GetValue(25).ToString();
                if (int.TryParse(MyReader1.GetValue(0).ToString(), out StudentId))
                {
                    if (!(StudentId > 0))
                    {
                        _continue = false;
                    }
                }
                int.TryParse(MyReader1.GetValue(26).ToString(), out Rank);
                if (_continue)
                {
                    classId = MyStudMang.GetClassId(StudentId, MyUser.CurrentBatchId);
                    sql = "SELECT COUNT(tblstudentclassmap.StudentId) FROM tblstudentclassmap INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudentclassmap.ClassId="+classId+" AND tblstudent.Status=1";
                    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        int.TryParse(MyReader.GetValue(0).ToString(), out StudentsCount);
                        if (StudentsCount == 0)
                        {
                            _continue = false;
                        }
                    }
                    else
                    {
                        _continue = false;
                    }
                }
              

                if (_continue)
                {
                    sql = "SELECT DISTINCT tblexammaster.ExamName,tblexammaster.ExamTypeId FROM tblstudentmark INNER JOIN tblexamschedule ON tblexamschedule.Id=tblstudentmark.ExamSchId INNER JOIN tblclassexam ON tblexamschedule.ClassExamId=tblclassexam.Id INNER JOIN tblexammaster ON tblexammaster.Id=tblclassexam.ExamId WHERE tblstudentmark.ExamSchId=" + _scheduledid;
                    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        ExamName = MyReader.GetValue(0).ToString();
                        int.TryParse(MyReader.GetValue(1).ToString(), out ExamTypeId);
                    }
                    else
                    {
                        _continue = false;
                    }
                }

                if (_continue)
                {
                    sql = "SELECT tblsubjects.SubjectCode FROM tblstudentmark INNER JOIN tblexammark ON tblexammark.ExamSchId=tblstudentmark.ExamSchId INNER JOIN tblsubjects ON tblsubjects.Id=tblexammark.SubjectId WHERE tblstudentmark.ExamSchId=" + _scheduledid + "  AND tblstudentmark.StudId=" + MyReader1.GetValue(0).ToString() + "  ORDER BY SubjectOrder";
                    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        while (MyReader.Read())
                        {
                            subjectStr = subjectStr + " " + MyReader.GetValue(0).ToString() + ':' + MyReader1.GetValue(i).ToString();
                            i = i + 1;
                        }
                    }
                }
                if (_continue)
                {
                    Incident Myincidence = MyUser.GetIncedentObj();
                    if (Rank > 0)
                    {
                        Myincidence.Reportincident("RankEquation", StudentsCount, Rank, subjectStr, StudentId, "student", classId, MyUser.CurrentBatchId, MyUser.UserId, ExamTypeId, "ExamRank" + StudentId.ToString() + classId.ToString() + MyUser.CurrentBatchId.ToString() +  _scheduledid);

                    }
                    else
                    {
                        Myincidence.Reportincident("FailedInExam", 0, Rank, "Fixed", StudentId, "student", classId, MyUser.CurrentBatchId, MyUser.UserId, ExamTypeId, "ExamRank" + StudentId.ToString() + classId.ToString() + MyUser.CurrentBatchId.ToString() +  _scheduledid);
                    }
                    sql = "SELECT tblgrade.Grade FROM tblgrade WHERE tblgrade.Status=1";
                    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        while (MyReader.Read())
                        {
                            if (Grade == MyReader.GetValue(0).ToString())
                            {
                                Myincidence.Reportincident("Grade" + Grade, 0, Rank, "Fixed", StudentId, "student", classId, MyUser.CurrentBatchId, MyUser.UserId, ExamTypeId, "ExamRank" + StudentId.ToString() + classId.ToString() + MyUser.CurrentBatchId.ToString() + _scheduledid);
                            }
                        }
                    }
                }
            }
        }
    }


    // code for AUTOMATIC SMS DATA TO TABLE
    //private void StoreSMSMessage(string _scheduledid)
    //{
    //    string sql = ""; int Next_Id = 0, i = 1;
    //    string StudentName = "", ParentName = "", PhoneNumber = "", subjectStr = "", ExamName = "";
    //    bool _continue = true;
    //    try
    //    {
    //        MyStudMang.CreateTansationDb();
    //        sql = "SELECT StudId,Mark1,Mark2,Mark3,Mark4,Mark5,Mark6,Mark7,Mark8,Mark9,Mark10,Mark11,Mark12,Mark13,Mark14,Mark15,Mark16,Mark17,Mark18,Mark19,Mark20,TotalMark,TotalMax,Avg,Grade,Result,Rank,Remark FROM tblstudentmark WHERE ExamSchId=" + _scheduledid;
    //        MyReader1 = MyStudMang.m_TransationDb.ExecuteQuery(sql);
    //        if (MyReader1.HasRows)
    //        {
    //            while (MyReader1.Read())
    //            {
    //                _continue = true;
    //                Next_Id = 0; i = 1;
    //                StudentName = ""; ParentName = ""; PhoneNumber = ""; subjectStr = ""; ExamName = "";
    //                if (_continue)
    //                {
    //                    sql = "SELECT tblsmsparentlist.PhoneNo FROM tblsmsparentlist WHERE tblsmsparentlist.Enabled=1 AND tblsmsparentlist.Id=" + MyReader1.GetValue(0).ToString();
    //                    MyReader = MyStudMang.m_TransationDb.ExecuteQuery(sql);
    //                    if (MyReader.HasRows)
    //                    {

    //                        PhoneNumber = MyReader.GetValue(0).ToString();

    //                    }
    //                    else
    //                    {
    //                        _continue = false;
    //                    }
    //                }


    //                if (_continue)
    //                {
    //                    sql = "SELECT tblstudent.StudentName,tblstudent.GardianName FROM tblstudent WHERE tblstudent.Id=" + MyReader1.GetValue(0).ToString();
    //                    MyReader = MyStudMang.m_TransationDb.ExecuteQuery(sql);
    //                    if (MyReader.HasRows)
    //                    {

    //                        StudentName = MyReader.GetValue(0).ToString();
    //                        ParentName = MyReader.GetValue(1).ToString();
    //                    }
    //                    else
    //                    {
    //                        _continue = false;
    //                    }
    //                }

    //                if (_continue)
    //                {
    //                    sql = "SELECT Max(Id) FROM tblautosms";
    //                    MyReader = MyStudMang.m_TransationDb.ExecuteQuery(sql);
    //                    if (MyReader.HasRows)
    //                    {
    //                        int.TryParse(MyReader.GetValue(0).ToString(), out Next_Id);
    //                        Next_Id++;
    //                    }
    //                    else
    //                    {
    //                        _continue = false;
    //                    }
    //                }
    //                if (_continue)
    //                {
    //                    sql = "SELECT DISTINCT tblexammaster.ExamName FROM tblstudentmark INNER JOIN tblexamschedule ON tblexamschedule.Id=tblstudentmark.ExamSchId INNER JOIN tblclassexam ON tblexamschedule.ClassExamId=tblclassexam.Id INNER JOIN tblexammaster ON tblexammaster.Id=tblclassexam.ExamId WHERE tblstudentmark.ExamSchId=" + _scheduledid;
    //                    MyReader = MyStudMang.m_TransationDb.ExecuteQuery(sql);
    //                    if (MyReader.HasRows)
    //                    {
    //                        ExamName = MyReader.GetValue(0).ToString();

    //                    }
    //                    else
    //                    {
    //                        _continue = false;
    //                    }
    //                }

    //                if (_continue)
    //                {
    //                    sql = "INSERT INTO tblautosms SET Id=" + Next_Id + ",PhoneNumber=" + PhoneNumber + ",Message=(SELECT `Format` FROM tblsmsoptionconfig WHERE `Enable`=1 AND ShortName='Onexamreport'),TimeToSend=(SELECT ScheduledTime FROM tblsmsoptionconfig WHERE `Enable`=1 AND ShortName='Onexamreport')";
    //                    MyStudMang.m_TransationDb.ExecuteQuery(sql);

    //                }
    //                if (_continue)
    //                {
    //                    sql = "SELECT tblsubjects.SubjectCode FROM tblstudentmark INNER JOIN tblexammark ON tblexammark.ExamSchId=tblstudentmark.ExamSchId INNER JOIN tblsubjects ON tblsubjects.Id=tblexammark.SubjectId WHERE tblstudentmark.ExamSchId=" + _scheduledid + "  AND tblstudentmark.StudId=" + MyReader1.GetValue(0).ToString() + "  ORDER BY MarkColumn";
    //                    MyReader = MyStudMang.m_TransationDb.ExecuteQuery(sql);
    //                    if (MyReader.HasRows)
    //                    {
    //                        while (MyReader.Read())
    //                        {
    //                            subjectStr = subjectStr + " " + MyReader.GetValue(0).ToString() + ':' + MyReader1.GetValue(i).ToString();
    //                            i = i + 1;
    //                        }
    //                    }


    //                }
    //                if (_continue)
    //                {
    //                    sql = "UPDATE tblautosms set Message=replace(Message, '($Student$)', '" + StudentName + "') where Id=" + Next_Id;
    //                    MyStudMang.m_TransationDb.ExecuteQuery(sql);
    //                    sql = "UPDATE tblautosms set Message=replace(Message, '($Parent$)', '" + ParentName + "') where Id=" + Next_Id;
    //                    MyStudMang.m_TransationDb.ExecuteQuery(sql);
    //                    sql = "UPDATE tblautosms set Message=replace(Message, '($S_M$)', '" + subjectStr + "') where Id=" + Next_Id;
    //                    MyStudMang.m_TransationDb.ExecuteQuery(sql);
    //                    sql = "UPDATE tblautosms set Message=replace(Message, '($TotalMk$)', '" + MyReader1.GetValue(21).ToString() + "') where Id=" + Next_Id;
    //                    MyStudMang.m_TransationDb.ExecuteQuery(sql);
    //                    sql = "UPDATE tblautosms set Message=replace(Message, '($MaxMk$)', '" + MyReader1.GetValue(22).ToString() + "') where Id=" + Next_Id;
    //                    MyStudMang.m_TransationDb.ExecuteQuery(sql);
    //                    sql = "UPDATE tblautosms set Message=replace(Message, '($AvgMk$)', '" + MyReader1.GetValue(23).ToString() + "') where Id=" + Next_Id;
    //                    MyStudMang.m_TransationDb.ExecuteQuery(sql);
    //                    sql = "UPDATE tblautosms set Message=replace(Message, '($Grade$)', '" + MyReader1.GetValue(24).ToString() + "') where Id=" + Next_Id;
    //                    MyStudMang.m_TransationDb.ExecuteQuery(sql);
    //                    sql = "UPDATE tblautosms set Message=replace(Message, '($Result$)', '" + MyReader1.GetValue(25).ToString() + "') where Id=" + Next_Id;
    //                    MyStudMang.m_TransationDb.ExecuteQuery(sql);
    //                    sql = "UPDATE tblautosms set Message=replace(Message, '($Rank$)', '" + MyReader1.GetValue(26).ToString() + "') where Id=" + Next_Id;
    //                    MyStudMang.m_TransationDb.ExecuteQuery(sql);
    //                    sql = "UPDATE tblautosms set Message=replace(Message, '($Remark$)', '" + MyReader1.GetValue(27).ToString() + "') where Id=" + Next_Id;
    //                    MyStudMang.m_TransationDb.ExecuteQuery(sql);
    //                    sql = "UPDATE tblautosms set Message=replace(Message, '($Exam$)', '" + ExamName + "') where Id=" + Next_Id;
    //                    MyStudMang.m_TransationDb.ExecuteQuery(sql);
    //                    //  21 TotalMark,TotalMax,Avg,Grade,Result,Rank,Remark
    //                }
    //            }
    //        }
    //        if (_continue)
    //        {
    //            MyStudMang.EndSucessTansationDb();
    //        }
    //        else
    //        {
    //            MyStudMang.EndFailTansationDb();
    //        }
    //    }
    //    catch
    //    {
    //        MyStudMang.EndFailTansationDb();
    //    }
    //}

    private void StoreSMSMessage(string _scheduledid)
    {
        string sql = ""; int i = 1;
        string StudentName = "", ParentName = "", PhoneNumber = "", subjectStr = "", ExamName = "",Message="";
        bool _continue = true;
        sql = "SELECT StudId,Mark1,Mark2,Mark3,Mark4,Mark5,Mark6,Mark7,Mark8,Mark9,Mark10,Mark11,Mark12,Mark13,Mark14,Mark15,Mark16,Mark17,Mark18,Mark19,Mark20,TotalMark,TotalMax,Avg,Grade,Result,Rank,Remark FROM tblstudentmark WHERE ExamSchId=" + _scheduledid;
        MyReader1 = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader1.HasRows)
        {
            SMSManager mysms = MyUser.GetSMSMngObj();
            mysms.InitClass();
            while (MyReader1.Read())
            {
                _continue = true;
                i = 1;
                StudentName = ""; ParentName = ""; PhoneNumber = ""; subjectStr = ""; ExamName = "";
                if (_continue)
                {
                    sql = "SELECT tblsmsparentlist.PhoneNo FROM tblsmsparentlist WHERE tblsmsparentlist.Enabled=1 AND tblsmsparentlist.Id=" + MyReader1.GetValue(0).ToString();
                    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {

                        PhoneNumber = MyReader.GetValue(0).ToString();

                    }
                    else
                    {
                        _continue = false;
                    }
                }


                if (_continue)
                {
                    sql = "SELECT tblstudent.StudentName,tblstudent.GardianName FROM tblstudent WHERE tblstudent.Id=" + MyReader1.GetValue(0).ToString();
                    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {

                        StudentName = MyReader.GetValue(0).ToString();
                        ParentName = MyReader.GetValue(1).ToString();
                    }
                    else
                    {
                        _continue = false;
                    }
                }


                if (_continue)
                {
                    sql = "SELECT DISTINCT tblexammaster.ExamName FROM tblstudentmark INNER JOIN tblexamschedule ON tblexamschedule.Id=tblstudentmark.ExamSchId INNER JOIN tblclassexam ON tblexamschedule.ClassExamId=tblclassexam.Id INNER JOIN tblexammaster ON tblexammaster.Id=tblclassexam.ExamId WHERE tblstudentmark.ExamSchId=" + _scheduledid;
                    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        ExamName = MyReader.GetValue(0).ToString();

                    }
                    else
                    {
                        _continue = false;
                    }
                }

                if (_continue)
                {
                    string _ShortName = "Onexamreport";
                    string remark = MyReader1.GetValue(27).ToString();
                    string _FeeDueResult_WithheldSubvalue = "";
                    string Need_FeeDueResult_Withheld = MyExamMang.GetConfigValue("FeeDueResult_Withheld", "Exam Report", out _FeeDueResult_WithheldSubvalue);
                    if (Need_FeeDueResult_Withheld.Trim() == "1")
                    {
                        if (_FeeDueResult_WithheldSubvalue.ToLowerInvariant().Trim() == remark.ToLowerInvariant().Trim())
                        {
                            _ShortName = "OnexamreportWithheld";
                        }
                    
                    }


                    sql = "SELECT `Format` FROM tblsmsoptionconfig WHERE `Enable`=1 AND ShortName='" + _ShortName + "'";
                    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        Message = MyReader.GetValue(0).ToString();

                    }
                    else
                    {
                        _continue = false;
                    }

                }
                if (_continue)
                {
                    sql = "SELECT tblsubjects.SubjectCode FROM tblstudentmark INNER JOIN tblexammark ON tblexammark.ExamSchId=tblstudentmark.ExamSchId INNER JOIN tblsubjects ON tblsubjects.Id=tblexammark.SubjectId WHERE tblstudentmark.ExamSchId=" + _scheduledid + "  AND tblstudentmark.StudId=" + MyReader1.GetValue(0).ToString() + "  ORDER BY MarkColumn";
                    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        while (MyReader.Read())
                        {
                            string mark = "";
                            int submark = int.Parse(MyReader1.GetValue(i).ToString());
                            if(submark==-1)
                            {
                                mark = "AB";
                            }
                            else
                            {
                                mark = MyReader1.GetValue(i).ToString();
                            }
                            //subjectStr = subjectStr + " " + MyReader.GetValue(0).ToString() + ':' + MyReader1.GetValue(i).ToString();
                            subjectStr = subjectStr + " " + MyReader.GetValue(0).ToString() + ':' + mark;
                            i = i + 2;
                        }
                    }


                }
                if (_continue)
                {
                    double avg = 0;
                    if (double.TryParse(MyReader1.GetValue(23).ToString(), out avg))
                    {
                        avg = Math.Round(avg, 2);
                    }
                    Message = Message.Replace("($Student$)", StudentName);
                    Message = Message.Replace("($Parent$)", ParentName);
                    Message = Message.Replace("($S_M$)", subjectStr);
                    Message = Message.Replace("($TotalMk$)", MyReader1.GetValue(21).ToString());
                    Message = Message.Replace("($MaxMk$)", MyReader1.GetValue(22).ToString());
                    Message = Message.Replace("($AvgMk$)", avg.ToString());
                    Message = Message.Replace("($Grade$)", MyReader1.GetValue(24).ToString());
                    Message = Message.Replace("($Result$)", MyReader1.GetValue(25).ToString());
                    Message = Message.Replace("($Rank$)", MyReader1.GetValue(26).ToString());
                    Message = Message.Replace("($Remark$)", MyReader1.GetValue(27).ToString());
                    Message = Message.Replace("($Exam$)", ExamName);
                }
                
                if (_continue && PhoneNumber != "" && Message!="")
                {
                    string failedList = "";
                    if (mysms.SendBULKSms(PhoneNumber, Message, "90366450445", "WINER", true, out  failedList))
                    {
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Exam SMS Report", "Message : " + Message, 1);
                    }
                }

            }
        }

    }


    # endregion

    protected void lnk_manageExam_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageClassExam.aspx");
    }

    protected void lnk_supplimentaryexam_Click(object sender, EventArgs e)
    {
        Response.Redirect("FailedStudMark.aspx");
    }

    protected void lnk_Schedule_Click(object sender, EventArgs e)
    {
        Response.Redirect("ScheduleExam.aspx");
    }
    protected void lnk_mark_Click(object sender, EventArgs e)
    {
        Response.Redirect("EnterMark.aspx");
    }

    protected void lnk_report_Click(object sender, EventArgs e)
    {
        Response.Redirect("ExamReport.aspx");
    }
    protected void Btn_generatadmitcard(object sender, EventArgs e)
    {

        string sql = "SELECT map.StudentId,stud.StudentName,stud.GardianName,stud.DOB,stud.Sex,stud.Address,tblclass.ClassName,stud.RollNo FROM tblstudentclassmap map inner join tblstudent stud on stud.id= map.Studentid inner join tblclass on tblclass.id = stud.LastClassId where stud.status=1 and  map.ClassId=" + int.Parse(DrpClassName.SelectedValue.ToString()) + " and map.RollNo<>-1 and map.BatchId=" + MyUser.CurrentBatchId + " order by map.RollNo";
        MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        sql = "select tblsubjects.subject_name, tblsubjects.SubjectCode,date_format(tblexammark.ExamDate,'%d/%m/%Y') AS `ExamDate` , tbltimeslot.StartTime, tbltimeslot.EndTime, tblclassexamsubmap.MinMark , tblclassexamsubmap.MaxMark from tblclassexam inner join tblexamschedule on tblclassexam.Id= tblexamschedule.ClassExamId AND tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " AND tblexamschedule.PeriodId=" + DrpExamPeriod.SelectedValue + " inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblclassexam.Id inner join tblexammark on tblexammark.ExamSchId= tblexamschedule.Id AND tblexammark.SubjectId= tblclassexamsubmap.SubId inner join tbltimeslot on tbltimeslot.Id= tblexammark.TimeSlotId inner join tblsubjects on tblsubjects.Id= tblexammark.SubjectId where tblclassexam.ClassId=" + DrpClassName.SelectedValue + " AND tblclassexam.ExamId=" + Session["ExamId"].ToString() + " order by tblexammark.ExamDate";
        examdataset = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        string selected_class = DrpClassName.SelectedValue;
        string selected_exam = Session["ExamId"].ToString();
        string selected_period = DrpExamPeriod.SelectedValue;
        string ExamName, Examtype, examfrequency;
        MyExamMang.GetExamDetail(int.Parse(Session["ExamId"].ToString()), out ExamName, out Examtype, out examfrequency);
        string exam_name = ExamName.ToString();

        ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"admitcard.aspx?ClassId=" + selected_class + "&ExamId="+ selected_exam +"&PeriodId=" + selected_period +"&ExamName="+ exam_name +"\");", true);


    }




   





}        