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
using WinBase;
using System.Data.Odbc;

namespace WinEr
{
    public partial class MarkMonthlyAttd_stud : System.Web.UI.Page
    {
        private ClassOrganiser MyClassMang;
        private TimeTable MyTimeTable;
        private Attendance MyAttendance;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyTimeTable = MyUser.GetTimeTableObj();
            MyClassMang = MyUser.GetClassObj();
            MyAttendance = MyUser.GetAttendancetObj();
            if (MyTimeTable == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MyAttendance == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(926))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    if (Session["StudDate"] == null)
                    {
                        Session["StudDate"] = DateTime.Today;
                    }


                    AddClassNameToDrpList();
                    if (Session["ClassId"] == null)
                    {
                        Session["ClassId"] = Drp_ClassName.SelectedValue;
                    }
                }
                
                Load_Calander();

            }
        }
        private void Load_Calander()
        {
            Hd_ClassId.Value = Session["ClassId"].ToString();
            setWeek();

            LoadPlot_Resourses();
            DayPlot_Monthly.DataSource = getData();
            DayPlot_Monthly.DataBind();

        }


        protected DataTable getData()
        {
            DataTable dt;
            dt = new DataTable();
            dt.Columns.Add("start", typeof(DateTime));
            dt.Columns.Add("end", typeof(DateTime));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("Period", typeof(string));
            dt.Columns.Add("barColor", typeof(string));
            DataRow dr;
            string BarColor = "", Comment = "";
            int PeriodId = 0,_PresentStatus=0;
            string Id = "";

            DateTime CalenderDays = DateTime.Now.Date;
            if (Hd_SelectedDate.Value != "")
            {
                CalenderDays = General.GetDateTimeFromText(Hd_SelectedDate.Value);
            }
            //int days = 1;
            string sql = "SELECT std.StudentId,std.PresentStatus,cls.`Date` FROM tblattdcls_std" + MyClassMang.GetStandardIdfromClassId(Hd_ClassId.Value) + "yr" + MyUser.CurrentBatchId + " cls INNER JOIN tblattdstud_std" + MyClassMang.GetStandardIdfromClassId(Hd_ClassId.Value) + "yr" + MyUser.CurrentBatchId + " std ON std.ClassAttendanceId=cls.Id WHERE cls.ClassId=" + Hd_ClassId.Value + " AND MONTH(cls.`Date`)=" + CalenderDays.Month;
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    BarColor = "";
                    CalenderDays = DateTime.Parse(MyReader.GetValue(2).ToString());
                    PeriodId = 0;
                    int.TryParse(MyReader.GetValue(0).ToString(), out PeriodId);
                    _PresentStatus = 0;
                    int.TryParse(MyReader.GetValue(1).ToString(), out _PresentStatus);
                    BarColor = "";
                    Comment = "";
                    Id = "";

                    if (_PresentStatus == 3)
                    {
                        BarColor = "#a4d805";
                    }
                    else if (_PresentStatus != 0)
                    {
                        BarColor = "#66ccff";
                    }
                    else
                    {
                        BarColor = "Red";
                    }


                    if (IsDateInBath(CalenderDays) && !MyAttendance.IsDateHoliday(int.Parse(Hd_ClassId.Value), CalenderDays))
                    {

                        Id = General.GerFormatedDatVal(CalenderDays) + "$" + Hd_ClassId.Value + "$" + PeriodId.ToString() + "$1";
                        dr = dt.NewRow();
                        dr["id"] = Id;
                        dr["start"] = CalenderDays;
                        dr["end"] = CalenderDays.AddDays(1);
                        dr["name"] = Comment;
                        dr["Period"] = PeriodId;
                        dr["barColor"] = BarColor;
                        dt.Rows.Add(dr);
                    }
                }
            }


            CalenderDays = DateTime.Now.Date;
            if (Hd_SelectedDate.Value != "")
            {
                CalenderDays = General.GetDateTimeFromText(Hd_SelectedDate.Value);
                CalenderDays = new DateTime(CalenderDays.Year, CalenderDays.Month, 1);

                
            }

            int _daysinMonth = DateTime.DaysInMonth(CalenderDays.Year, CalenderDays.Month);
            int days = 1;
            sql = "SELECT tblstudent.Id,tblstudent.StudentName from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + int.Parse(Hd_ClassId.Value) + " Order by tblstudentclassmap.RollNo ASC";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    BarColor = "";
                    CalenderDays = DateTime.Now.Date;
                    if (Hd_SelectedDate.Value != "")
                    {
                        CalenderDays = General.GetDateTimeFromText(Hd_SelectedDate.Value);
                        CalenderDays = new DateTime(CalenderDays.Year, CalenderDays.Month, 1);


                    }
                    PeriodId = 0;
                    int.TryParse(MyReader.GetValue(0).ToString(), out PeriodId);
                    days = 1;
                    while (days <= _daysinMonth)
                    {
                        BarColor = "";
                        Comment = "";
                        Id = "";
                        if (IsDateInBath(CalenderDays))
                        {
                            if (MyAttendance.IsDateHoliday(int.Parse(Hd_ClassId.Value), CalenderDays))
                            {

                                // Id = General.GerFormatedDatVal(CalenderDays) + "$" + Drp_ClassName.SelectedValue + "$" + PeriodId.ToString() + "$1";
                                Id = "-1";
                                dr = dt.NewRow();
                                dr["id"] = Id;
                                dr["start"] = CalenderDays;
                                dr["end"] = CalenderDays.AddDays(1);
                                dr["name"] = "";
                                dr["Period"] = PeriodId;
                                dr["barColor"] = "#ffcc00";
                                dt.Rows.Add(dr);

                            }


                        }
                        else
                        {
                            //Id = General.GerFormatedDatVal(CalenderDays) + "$" + Drp_ClassName.SelectedValue + "$" + PeriodId.ToString() + "$1";
                            Id = "-2";
                            dr = dt.NewRow();
                            dr["id"] = Id;
                            dr["start"] = CalenderDays;
                            dr["end"] = CalenderDays.AddDays(1);
                            dr["name"] = "";
                            dr["Period"] = PeriodId;
                            dr["barColor"] = "#ffc1c1";
                            dt.Rows.Add(dr);

                        }

                        CalenderDays = CalenderDays.AddDays(1);
                        days++;
                    }

                }
            }



            return dt;

        }






        private bool IsDateInBath(DateTime SELECTEDDATE)
        {
            int M_Id = 0;
            bool valid = false;
            string sql = "SELECT tblbatch.Id FROM tblbatch WHERE tblbatch.Id=" + MyUser.CurrentBatchId + " AND '" + SELECTEDDATE.Date.ToString("s") + "' BETWEEN tblbatch.StartDate AND tblbatch.EndDate";
            OdbcDataReader MyReader1 = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader1.HasRows)
            {
                int.TryParse(MyReader1.GetValue(0).ToString(), out M_Id);
                if (M_Id > 0)
                {
                    valid = true;
                }
            }
            return valid;
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            setWeek();
            DayPlot_Monthly.DataSource = getData();
            DayPlot_Monthly.DataBind();
        }

        private void setWeek()
        {
            //Session["StartDate"] = Calendar1.SelectedDate;

            DateTime _SelectedDate = DateTime.Parse(Session["StudDate"].ToString());
            if (Hd_SelectedDate.Value != "")
            {
                _SelectedDate = General.GetDateTimeFromText(Hd_SelectedDate.Value);
            }
            DateTime firstDay = new DateTime(_SelectedDate.Year, _SelectedDate.Month, 1);
            //Calendar1.VisibleDate = firstDay;
            //for (int i = 0; i < 7; i++)
            //    Calendar1.SelectedDates.Add(firstDay.AddDays(i));

            int _daysinMonth = DateTime.DaysInMonth(firstDay.Year, firstDay.Month);
            DayPlot_Monthly.Days = _daysinMonth;
            DayPlot_Monthly.StartDate = firstDay;
            //DateTime lastDay = firstDay.AddDays(6);
             Lbl_SchedulerHeader.Text = firstDay.ToString("MMMMMM , yyyy");

        }



        private void LoadPlot_Resourses()
        {
            //Pnl_calanderview.Visible = true;
            //Panel_Msg.Visible = false;
            DayPlot_Monthly.Resources.Clear();
            DataSet _ItemResources = LoadAwailableResources();
            if (_ItemResources != null && _ItemResources.Tables != null && _ItemResources.Tables[0].Rows.Count > 0)
            {
                DayPlot_Monthly.Resources.Clear();
                //DayPlot_Monthly.DataSource = null;
                //DataBind();
                foreach (DataRow dr in _ItemResources.Tables[0].Rows)
                {

                    DayPlot_Monthly.Resources.Add(dr[1].ToString(), dr[0].ToString());

                }
            }
            else
            {
                //Lbl_note.Text = "No Item Available";
                //Pnl_calanderview.Visible = false;
                //Panel_Msg.Visible = true;
            }
        }

        private DataSet LoadAwailableResources()
        {

            string sql = "SELECT tblstudent.Id,tblstudent.StudentName from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + int.Parse(Hd_ClassId.Value) + " Order by tblstudentclassmap.RollNo ASC";
            MydataSet = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return MydataSet;
        }

        private void AddClassNameToDrpList()
        {
            Drp_ClassName.Items.Clear();

            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {

                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_ClassName.Items.Add(li);

                }

            }
            else
            {
                ListItem li = new ListItem("No Class present", "-1");
                Drp_ClassName.Items.Add(li);
            }
            Drp_ClassName.SelectedIndex = 0;
        }

        protected void Drp_ClassName_SelectedIndexChanged(object sender, EventArgs e)
        {

            Session["ClassId"] = Drp_ClassName.SelectedValue;

            Load_Calander();
        }

        protected void ImgBtn_Left_Click(object sender, ImageClickEventArgs e)
        {
            DateTime _SelectedDate = DateTime.Now.Date;
            if (Hd_SelectedDate.Value != "")
            {
                _SelectedDate = General.GetDateTimeFromText(Hd_SelectedDate.Value);
            }
            Hd_SelectedDate.Value = General.GerFormatedDatVal(_SelectedDate.AddMonths(-1));
            Load_Calander();
        }

        protected void ImgBtn_Right_Click(object sender, ImageClickEventArgs e)
        {
            DateTime _SelectedDate = DateTime.Now.Date;
            if (Hd_SelectedDate.Value != "")
            {
                _SelectedDate = General.GetDateTimeFromText(Hd_SelectedDate.Value);
            }
            Hd_SelectedDate.Value = General.GerFormatedDatVal(_SelectedDate.AddMonths(1));
            Load_Calander();
        }

  
    }
}
