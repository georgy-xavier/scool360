using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;
using System.Text;
using WinEr;

namespace WinErParentLogin
{
    public partial class StudTimeTable : System.Web.UI.Page
    {
        private ParentInfoClass MyParentInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MyParentObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyParentInfo = (ParentInfoClass)Session["MyParentObj"];
            if (MyParentInfo == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (!IsPostBack)
            {
                Label MyHeader = (Label)this.Master.FindControl("Lbl_PageHeader");
                MyHeader.Text = "Time Table";
                Hd_Date.Value = General.GerFormatedDatVal(DateTime.Now.Date);
                LoadTimetable();
            }

        }

        private void LoadTimetable()
        {
            DateTime SelectedDate = General.GetDateFromText(Hd_Date.Value);
            DateTime Date = firstDayOfWeek(SelectedDate, DayOfWeek.Monday);
            
            TimetableDiv.InnerHtml = GetTimeTableView(MyParentInfo.StudentId, Date, MyParentInfo.CLASSID);
        }


        private string GetTimeTableView(int StudentId, DateTime Date, int ClassId)
        {
            Lnk_Previous.Visible = true;
            Lnk_Next.Visible=true;
            Img_Export.Visible = true;
            DivBottom_Color.Visible = true;
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            TimeTable MyTimeTable = new TimeTable(_mysqlObj);
            Attendance MyAttendance = new Attendance(_mysqlObj);
            DateTime WorkingDate = Date;
            StringBuilder _TimeTable = new StringBuilder();
            string PeriodData = "";
            OdbcDataReader MyPeriod = null;
            string sql = "select distinct tbltime_week.Id,tbltime_week.Name from tbltime_week order by tbltime_week.Id";
            DataSet MyDays = MyTimeTable.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            sql = " select DISTINCT tbltime_classperiod.PeriodId,tblattendanceperiod.FrequencyName from tbltime_classperiod INNER JOIN tblattendanceperiod ON tblattendanceperiod.PeriodId=tbltime_classperiod.PeriodId where tbltime_classperiod.ClassId=" + ClassId + " ORDER BY tbltime_classperiod.PeriodId";
            DataSet MyPeriods = MyTimeTable.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyPeriods != null && MyPeriods.Tables != null && MyPeriods.Tables[0].Rows.Count > 0 && MyDays != null && MyDays.Tables != null && MyDays.Tables[0].Rows.Count > 0)
            {

                _TimeTable.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"5\" class=\"MyTable\">");
                //*********************HEADER *********************************
                _TimeTable.Append("<tr>");
                _TimeTable.Append("<th>DAYS/PERIODS</th>");
                foreach (DataRow Dr_Period in MyPeriods.Tables[0].Rows)
                {
                    _TimeTable.Append("<th>" + Dr_Period[1].ToString().ToUpper() + "</th>");
                }
                _TimeTable.Append("</tr>");

                //*********************ROWS********************************

                foreach (DataRow Dr_Day in MyDays.Tables[0].Rows)
                {
                    bool IsHolliday = false;
                    bool IsCurrentBatch = true;
                    _TimeTable.Append("<tr");
                    if (WorkingDate == DateTime.Now.Date)
                    {
                        _TimeTable.Append(" style=\"background-color: #97FFB1;\" ");
                    }
                    else if (!IsDateInBath(WorkingDate))
                    {
                        _TimeTable.Append(" style=\"background-color: #ffc1c1;\" ");
                        IsCurrentBatch = false;
                    }
                    else if (MyAttendance.IsDateHoliday(0, WorkingDate))
                    {
                        _TimeTable.Append(" style=\"background-color: #ffcc00;\" ");
                        IsHolliday = true;
                    }
                    _TimeTable.Append(">");
                    _TimeTable.Append("<td><b>" + Dr_Day[1].ToString().Substring(0, 3).ToUpperInvariant() + " , " + General.GerFormatedDatVal(WorkingDate) + "</b></td>");

                    foreach (DataRow Dr_Period in MyPeriods.Tables[0].Rows)
                    {
                        _TimeTable.Append("<td class=\"DataCell\"><b>");

                        bool subjectAssaingned = false;
                        sql = "SELECT tbluser.SurName,tbltime_submaster.SubjectId FROM tbltime_submaster INNER JOIN tbluser ON tbluser.Id=tbltime_submaster.StaffId WHERE tbltime_submaster.PeriodId=" + Dr_Period[0].ToString() + " AND tbltime_submaster.`Day`='" + WorkingDate.Date.ToString("s") + "' AND tbltime_submaster.ClassId=" + ClassId;
                        MyPeriod = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
                        if (MyPeriod.HasRows)
                        {
                            PeriodData = "Subject : " + MyTimeTable.GetSubjectName(MyPeriod.GetValue(1).ToString()) + "<br>Staff : " + MyPeriod.GetValue(0).ToString();
                            _TimeTable.Append(PeriodData);
                            subjectAssaingned = true;
                        }


                        if (!subjectAssaingned && !IsHolliday && IsCurrentBatch)
                        {
                            sql = "SELECT tbluser.SurName,tbltime_master.SubjectId FROM tbltime_master INNER JOIN tbltime_classperiod ON tbltime_classperiod.Id=tbltime_master.ClassPeriodId INNER JOIN tbluser ON tbluser.Id=tbltime_master.StaffId INNER JOIN tblattendanceperiod ON tblattendanceperiod.PeriodId=tbltime_classperiod.PeriodId WHERE tbltime_classperiod.PeriodId=" + Dr_Period[0].ToString() + " AND tbltime_classperiod.DayId=" + Dr_Day[0].ToString() + " AND tbltime_classperiod.ClassId=" + ClassId;
                            MyPeriod = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
                            if (MyPeriod.HasRows)
                            {
                                PeriodData = "Subject : " + MyTimeTable.GetSubjectName(MyPeriod.GetValue(1).ToString()) + "<br>Staff : " + MyPeriod.GetValue(0).ToString();
                                _TimeTable.Append(PeriodData);
                                subjectAssaingned = true;
                            }
                        }

                        if (!subjectAssaingned)
                        {

                            _TimeTable.Append("FREE");
                        }


                        _TimeTable.Append("</b></td>");
                    }
                    _TimeTable.Append("</tr>");
                    WorkingDate = WorkingDate.AddDays(1);
                }

                _TimeTable.Append("</table>");
            }
            else
            {
                _TimeTable.Append("<br/><br/><br/><h3>Time table is not generated</h3>");
                Img_Export.Visible = false;
                Lnk_Previous.Visible = false;
                Lnk_Next.Visible = false;
                DivBottom_Color.Visible = false;
            }

            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyTimeTable = null;
            MyAttendance = null;

            string _Table = _TimeTable.ToString();
            return _Table;
        }

        private bool IsDateInBath(DateTime SELECTEDDATE)
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            TimeTable MyTimeTable = new TimeTable(_mysqlObj);
            int M_Id = 0;
            bool valid = false;
            string sql = "SELECT tblbatch.Id FROM tblbatch WHERE tblbatch.Id=" + MyParentInfo.CurrentBatchId + " AND '" + SELECTEDDATE.Date.ToString("s") + "' BETWEEN tblbatch.StartDate AND tblbatch.EndDate";
            OdbcDataReader MyReader1 = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader1.HasRows)
            {
                int.TryParse(MyReader1.GetValue(0).ToString(), out M_Id);
                if (M_Id > 0)
                {
                    valid = true;
                }
            }
            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyTimeTable = null;
            return valid;
        }

        private static DateTime firstDayOfWeek(DateTime day, DayOfWeek weekStarts)
        {
            DateTime d = day;
            while (d.DayOfWeek != weekStarts)
            {
                d = d.AddDays(-1);
            }

            return d;
        }

        protected void Lnk_Next_Click(object sender, EventArgs e)
        {
            DateTime SelectedDate = General.GetDateFromText(Hd_Date.Value);
            SelectedDate = SelectedDate.AddDays(7);
            Hd_Date.Value = General.GerFormatedDatVal(SelectedDate);
            LoadTimetable();
        }

        protected void Lnk_Previous_Click(object sender, EventArgs e)
        {
            DateTime SelectedDate = General.GetDateFromText(Hd_Date.Value);
            SelectedDate = SelectedDate.AddDays(-7);
            Hd_Date.Value = General.GerFormatedDatVal(SelectedDate);
            LoadTimetable();
        }

        protected void Img_Export_Click(object sender, ImageClickEventArgs e)
        {
            ExcelUtility.ExportBuiltStringToExcel(MyParentInfo.StudentName + "TimeTable.xsl", TimetableDiv.InnerHtml, "TimeTable");
        }




    }
}
