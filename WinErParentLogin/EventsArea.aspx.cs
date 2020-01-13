using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using WinBase;
using System.Data;
using System.Drawing;

namespace WinErParentLogin
{
    public partial class EventsArea : System.Web.UI.Page
    {
        private OdbcDataReader MyReader = null;
        private ParentInfoClass MyParentInfo;
        private Attendance MyAttendence;
        private DataSet holidaydataset, EventDataset;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MyParentObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyParentInfo = (ParentInfoClass)Session["MyParentObj"];
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            MyAttendence = new Attendance(_mysqlObj);
            if (MyParentInfo == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            else if (MyAttendence == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            else if (!IsPostBack)
            {

               // LoadSchoolDetails();

                     LoadCalenderDetails();
               // LoadContentSlide();
                Label MyHeader = (Label)this.Master.FindControl("Lbl_PageHeader");
                MyHeader.Text = "Home";

                DBLogClass _DblogObj = new DBLogClass(_mysqlObj);

                _DblogObj.LogToDb(MyParentInfo.ParentName, "ParentLogin_Visit Event Page", "Event page visited", 4, 2);
                _DblogObj = null;

            }
        }
        # region Calender Manager

        private void LoadCalenderDetails()
        {
            Calendar1.SelectedDate = new DateTime().Date;
            string type = "all";
           
            holidaydataset = MyAttendence.MyAssociatedHolidays(type, MyParentInfo.CLASSID, MyParentInfo.ParentId);
            EventDataset = MyAssociatedEventsDataSet(MyParentInfo.CLASSID);
        }

        private DataSet MyAssociatedEventsDataSet(int Classid)
        {
            DataSet Events = null;
            string sql = "select distinct tblmasterdate.`date`, tblcalender_event.ClassId, tbleventmaster.EventName,tbleventmaster.Description from tblmasterdate inner join tblcalender_event on tblcalender_event.DateId= tblmasterdate.Id INNER JOIN tbleventmaster ON tbleventmaster.Id=tblcalender_event.EventId where  ((tblcalender_event.ClassId=0) or ( tblcalender_event.ClassId=" + Classid + "))";
            Events = MyAttendence.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return Events;
        }

        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            bool done = false;


            DateTime nextholidayDate;
            if (holidaydataset != null && holidaydataset.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in holidaydataset.Tables[0].Rows)
                {
                    nextholidayDate = (DateTime)dr[0];
                    if (MyAttendence.IsDefaultHoliday(e.Day.Date.DayOfWeek))
                    {
                        e.Cell.BackColor = HexStringToColor("#ffcc00");//Holiday
                        e.Cell.BackColor = Color.Gold;
                        AddTextToDayCell(e, "Holiday", "Default Holiday");
                        done = true;
                        break;
                    }
                    else
                    {
                        if (nextholidayDate == e.Day.Date)
                        {

                            e.Cell.BackColor = HexStringToColor("#ffcc00");//Holiday
                            e.Cell.BackColor = Color.Gold;
                            AddTextToDayCell(e, "Holiday", dr[2].ToString());
                            done = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                if (MyAttendence.IsDefaultHoliday(e.Day.Date.DayOfWeek))
                {
                    e.Cell.BackColor = HexStringToColor("#ffcc00"); //Holiday
                    e.Cell.BackColor = Color.Gold;
                    AddTextToDayCell(e, "Holiday", "Default Holiday");
                    done = true;
                }
            }

            if (EventDataset != null && EventDataset.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in EventDataset.Tables[0].Rows)
                {
                    nextholidayDate = (DateTime)dr[0];

                    if (nextholidayDate == e.Day.Date)
                    {

                        e.Cell.BackColor = System.Drawing.Color.Turquoise;
                        AddTextToDayCell(e, "Event", dr[2].ToString() + "$" + dr[3].ToString());
                        done = true;
                        break;
                    }

                }
            }



            if (!done)
            {
                AddTextToDayCell(e, "Other", "Other");
            }
        }

        void AddTextToDayCell(DayRenderEventArgs e, string Type, string Name)
        {

            string TextColor = "#000000";
            if (e.Day.IsOtherMonth)
            {
                TextColor = "#999999";
            }
            if (Type == "Holiday")
            {
                string HolidayMessage = "Selected day is holiday due to " + Name;
                if (Name == "Default Holiday")
                {
                    HolidayMessage = e.Day.Date.DayOfWeek + " is holiday";
                }
                e.Cell.Text = "<a href=\"javascript:alert('" + HolidayMessage + "')\" style=\"color:" + TextColor + "\">" + e.Day.DayNumberText;
            }
            else if (Type == "NotBatch")
            {
                e.Cell.Text = "<a href=\"javascript:alert('Selected day is not within batch')\" style=\"color:" + TextColor + "\">" + e.Day.DayNumberText;
            }
            else if (Type == "Event")
            {
                string HdValue = CalenderDataHide.InnerHtml;
                string[] str = Name.Split('$');

                if (HdValue == "")
                {
                    HdValue = str[0] + "*-*" + str[1];
                }
                else
                {
                    CalenderDataHide.InnerHtml = HdValue + "$%$" + str[0] + "*-*" + str[1];
                }
                e.Cell.Text = "<a href=\"javascript:LoadPopup('" + str[0] + "','" + str[1] + "')\" style=\"color:" + TextColor + "\">" + e.Day.DayNumberText;
            }
            else
            {
                e.Cell.Text = "<a href=\"javascript:alert('" + General.GerFormatedDatVal(e.Day.Date) + "')\" style=\"color:" + TextColor + "\">" + e.Day.DayNumberText;
            }

        }

        private System.Drawing.Color HexStringToColor(string hex)
        {
            hex = hex.Replace("#", "");

            if (hex.Length != 6)
                throw new Exception(hex +
                    " is not a valid 6-place hexadecimal color code.");

            string r, g, b;

            r = hex.Substring(0, 2);
            g = hex.Substring(2, 2);
            b = hex.Substring(4, 2);

            return System.Drawing.Color.FromArgb(HexStringToBase10Int(r), HexStringToBase10Int(g),
                                                HexStringToBase10Int(b));
        }

        private int HexStringToBase10Int(string hex)
        {
            int base10value = 0;

            try { base10value = System.Convert.ToInt32(hex, 16); }
            catch { base10value = 0; }

            return base10value;

        }





        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {

        }

        protected void Calendar1_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            Calendar1.SelectedDate = new DateTime().Date;
            LoadCalenderDetails();
        }

        # endregion
    }
}
