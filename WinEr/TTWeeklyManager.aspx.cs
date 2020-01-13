using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using WinBase;

namespace WinEr
{
    public partial class TTWeeklyManager : System.Web.UI.Page
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
            else if (!MyUser.HaveActionRignt(632))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    if (Session["StartDate"] == null)
                    {
                        Session["StartDate"] = DateTime.Now.Date;
                    }
                    AddClassNameToDrpList();
                    Load_Calander();
                }


            }
        }



        private void LoadPlot_Resourses()
        {
            Pnl_calanderview.Visible = true;
            Panel_Msg.Visible = false;
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
                Pnl_calanderview.Visible = false;
                Panel_Msg.Visible = true;
            }
        }

        private DataSet LoadAwailableResources()
        {
            string sql = " select DISTINCT tbltime_classperiod.PeriodId,tblattendanceperiod.FrequencyName from tbltime_classperiod INNER JOIN tblattendanceperiod ON tblattendanceperiod.PeriodId=tbltime_classperiod.PeriodId where tbltime_classperiod.ClassId="+Hd_ClassId.Value+" ORDER BY tbltime_classperiod.PeriodId";
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
            int i_SelectedClassId = int.Parse(Drp_ClassName.SelectedValue.ToString());

            Load_Calander();

        }



        private void Load_Calander()
        {
            Hd_ClassId.Value = Drp_ClassName.SelectedValue;

            DateTime SelctedDate = DateTime.Parse(Session["StartDate"].ToString());
            Calendar1.SelectedDate = SelctedDate;
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
            string  BarColor = "", Comment = "" ;
            int  PeriodId= 0;
            string Id = "";
            DateTime CalenderDays = firstDayOfWeek(Calendar1.SelectedDate, DayOfWeek.Monday);
            int days = 1;
            string sql = " select DISTINCT tbltime_classperiod.PeriodId,tblattendanceperiod.FrequencyName from tbltime_classperiod INNER JOIN tblattendanceperiod ON tblattendanceperiod.PeriodId=tbltime_classperiod.PeriodId where tbltime_classperiod.ClassId=" + Hd_ClassId.Value + " ORDER BY tbltime_classperiod.PeriodId";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    BarColor = "";
                    CalenderDays = firstDayOfWeek(Calendar1.SelectedDate, DayOfWeek.Monday);
                    PeriodId=0;
                    int.TryParse(MyReader.GetValue(0).ToString(),out PeriodId);
                    days = 1;
                    while (days <= 7)
                    {
                        BarColor = "";
                        Comment = "";
                        Id = "";
                        if (IsDateInBath(CalenderDays))
                        {
                            if (MyAttendance.IsDateHoliday(int.Parse(Drp_ClassName.SelectedValue), CalenderDays))
                            {
                                bool Generalbool = true;
                                if (MyTimeTable.TemperoryTimeTableEntryExists(CalenderDays, Drp_ClassName.SelectedValue, PeriodId.ToString()))
                                {
                                    Comment = "";
                                    BarColor = "#c6ffc6";
                                    int _staffId = 0, _SubjectId = 0;
                                    MyTimeTable.GetClassPeriodDetails_Weekly(int.Parse(Drp_ClassName.SelectedValue), PeriodId, CalenderDays, out _staffId, out _SubjectId, out Generalbool);
                                    if (_staffId > 0 || _SubjectId > 0)
                                    {
                                        Comment = MyTimeTable.GetSubjectName(_SubjectId.ToString());
                                        BarColor = "#c6ffc6";
                                    }
                                }

                                if (Generalbool)
                                {
                                    Id = General.GerFormatedDatVal(CalenderDays) + "$" + Drp_ClassName.SelectedValue + "$" + PeriodId.ToString()+"$1";
                                    dr = dt.NewRow();
                                    dr["id"] = Id;
                                    dr["start"] = CalenderDays;
                                    dr["end"] = CalenderDays.AddDays(1);
                                    dr["name"] = "";
                                    dr["Period"] = PeriodId;
                                    dr["barColor"] = "#ffcc00";
                                    dt.Rows.Add(dr);
                                }
                                else
                                {
                                    Id = General.GerFormatedDatVal(CalenderDays) + "$" + Drp_ClassName.SelectedValue + "$" + PeriodId.ToString() + "$1";
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
                            else if (GetPeriodStatus(CalenderDays, PeriodId, int.Parse(Drp_ClassName.SelectedValue), out BarColor, out Comment))
                            {

                                Id = General.GerFormatedDatVal(CalenderDays) + "$" + Drp_ClassName.SelectedValue + "$" + PeriodId.ToString() + "$0";
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
                        else
                        {
                            bool Generalbool = true;
                            if (MyTimeTable.TemperoryTimeTableEntryExists(CalenderDays, Drp_ClassName.SelectedValue, PeriodId.ToString()))
                            {
                                Comment = "";
                                BarColor = "#c6ffc6";
                                int _staffId = 0, _SubjectId = 0;
                                MyTimeTable.GetClassPeriodDetails_Weekly(int.Parse(Drp_ClassName.SelectedValue), PeriodId, CalenderDays, out _staffId, out _SubjectId, out Generalbool);
                                if (_staffId > 0 || _SubjectId > 0)
                                {
                                    Comment = MyTimeTable.GetSubjectName(_SubjectId.ToString());
                                    BarColor = "#c6ffc6";
                                }
                            }

                            if (Generalbool)
                            {
                                Id = General.GerFormatedDatVal(CalenderDays) + "$" + Drp_ClassName.SelectedValue + "$" + PeriodId.ToString() + "$1";
                                dr = dt.NewRow();
                                dr["id"] = Id;
                                dr["start"] = CalenderDays;
                                dr["end"] = CalenderDays.AddDays(1);
                                dr["name"] = "";
                                dr["Period"] = PeriodId;
                                dr["barColor"] = "#ffc1c1";
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                Id = General.GerFormatedDatVal(CalenderDays) + "$" + Drp_ClassName.SelectedValue + "$" + PeriodId.ToString() + "$1";
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

                        CalenderDays = CalenderDays.AddDays(1);
                        days++;
                    }

                }
            }


            return dt;

        }

        private bool GetPeriodStatus(DateTime CalenderDays, int PeriodId, int ClassId, out string BarColor,out string Comment)
        {
            bool Generalbool;
            bool valid=true;
            Comment = "";
            BarColor = "White";
            int staffId=0,SubjectId=0;
            MyTimeTable.GetClassPeriodDetails_Weekly(ClassId, PeriodId, CalenderDays, out staffId, out SubjectId, out Generalbool);
            if(staffId>0 || SubjectId>0)
            {
                Comment = MyTimeTable.GetSubjectName(SubjectId.ToString());
                BarColor = "#c6ffc6";
            }

            return valid;
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
            Session["StartDate"] = Calendar1.SelectedDate;
            DateTime firstDay = firstDayOfWeek(Calendar1.SelectedDate, DayOfWeek.Monday);
            Calendar1.VisibleDate = firstDay;
            for (int i = 0; i < 7; i++)
                Calendar1.SelectedDates.Add(firstDay.AddDays(i));

            DayPlot_Monthly.StartDate = firstDay;
            DateTime lastDay = firstDay.AddDays(6);
            Lbl_SchedulerHeader.Text = firstDay.ToString("D") + " To " + lastDay.ToString("D");

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


        protected void Img_Calender_Click(object sender, ImageClickEventArgs e)
        {
            Calendar1.Visible = !Calendar1.Visible;
        }

        protected void LinkButtonCalender_Click(object sender, EventArgs e)
        {
            Calendar1.Visible = !Calendar1.Visible;
        }

        protected void ImgBtn_Left_Click(object sender, ImageClickEventArgs e)
        {
            Calendar1.SelectedDate = Calendar1.SelectedDate.AddDays(-7);
            setWeek();
            DayPlot_Monthly.DataSource = getData();
            DayPlot_Monthly.DataBind();
        }

        protected void ImgBtn_Right_Click(object sender, ImageClickEventArgs e)
        {
            Calendar1.SelectedDate = Calendar1.SelectedDate.AddDays(7);
            setWeek();
            DayPlot_Monthly.DataSource = getData();
            DayPlot_Monthly.DataBind();
        }
    }
}
