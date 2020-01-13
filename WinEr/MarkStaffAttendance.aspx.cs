using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using WinBase;
using System.Data.Odbc;
using System.Drawing;

namespace WinEr
{
    public partial class MarkStaffAttendance : System.Web.UI.Page
    {
       
        private Attendance MyAttendance;
        private StaffManager MyStaffMang;
        private KnowinUser MyUser;      
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet, holidaydataset;

        protected void Page_PreInit(Object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            else
            {
                MyUser = (KnowinUser)Session["UserObj"];

                if (MyUser.SELECTEDMODE == 1)
                {
                    this.MasterPageFile = "~/WinerStudentMaster.master";

                }
                else if (MyUser.SELECTEDMODE == 2)
                {

                    this.MasterPageFile = "~/WinerSchoolMaster.master";
                }
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStaffMang = MyUser.GetStaffObj();
            MyAttendance = MyUser.GetAttendancetObj();
            if (MyStaffMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(81))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (!IsPostBack)
                {
                    if (Session["StaffDate"] == null)
                    {
                        Session["StaffDate"] = DateTime.Today;
                    }
                    Load_Calender();
                }
            }

        }

        private void Load_Calender()
        {
            Calendar1.VisibleDate = DateTime.Parse(Session["StaffDate"].ToString());
            string type = "all";
            int Classid = 0;
            holidaydataset = MyAttendance.MyAssociatedHolidays(type, Classid, MyUser.User_Id);
            Load_unMarked();
            Lnk_Today.Visible = true;
            if (Calendar1.VisibleDate.Month == DateTime.Now.Month)
            {
                Lnk_Today.Visible = false;
            }
        }

        private void Load_unMarked()
        {
            DateTime StartingDate, EndingDate, TempDate = new DateTime(), nextholidayDate;
            int AttendanceStatus = 0;
            bool _Continue = true, next = true;
            if (GetBatchDates(MyUser.CurrentBatchId, out StartingDate, out EndingDate))
            {
                TempDate = StartingDate;
                while (_Continue && TempDate <= EndingDate && TempDate <= DateTime.Now.Date)
                {
                    next = true;
                    AttendanceStatus = 0;
                    if (MyAttendance.IsDefaultHoliday(TempDate.Date.DayOfWeek))
                    {
                        next = false;
                    }
                    if (next)
                    {
                        if (holidaydataset != null && holidaydataset.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in holidaydataset.Tables[0].Rows)
                            {
                                nextholidayDate = (DateTime)dr[0];
                                if (TempDate == nextholidayDate)
                                {
                                    next = false;
                                    break;
                                }
                            }
                        }
                    }
                    if (next)
                    {
                        AttendanceStatus = MyAttendance.GetStaffAttendanceStatus(TempDate);
                        if (AttendanceStatus == 0)
                        {
                            _Continue = false;
                            break;
                        }
                        else
                        {
                            TempDate = TempDate.AddDays(1);
                        }
                    }
                    else
                    {
                        TempDate = TempDate.AddDays(1);
                    }
                }
                if (_Continue)
                {
                    lbldate.Text = "";
                    Lnk_Unmarked.Text = "";
                }
                else
                {
                    lbldate.Text = MyUser.GerFormatedDatVal(TempDate);
                    Lnk_Unmarked.Text = "Attendance Not Marked On";

                }
            }

        }

        private bool GetBatchDates(int BatchId, out DateTime StartingDate, out DateTime EndingDate)
        {
            bool valid = false;
            StartingDate = new DateTime();
            EndingDate = new DateTime();
            string sql = "SELECT StartDate,EndDate FROM tblbatch WHERE tblbatch.Id=" + BatchId;
            MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {

                if (DateTime.TryParse(MyReader.GetValue(0).ToString(), out StartingDate) && DateTime.TryParse(MyReader.GetValue(1).ToString(), out EndingDate))
                {
                    valid = true;
                }
            }
            return valid;
        }

        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            if (IsDateInBath(e.Day.Date))
            {


                bool IsHoliday = false;
                DateTime nextholidayDate;
                if (holidaydataset != null && holidaydataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in holidaydataset.Tables[0].Rows)
                    {
                        nextholidayDate = (DateTime)dr[0];
                        if (MyAttendance.IsDefaultHoliday(e.Day.Date.DayOfWeek))
                        {
                            e.Cell.BackColor = HexStringToColor("#ffcc00");//Holiday
                            AddTextToDayCell(e, "Holiday");
                            IsHoliday = true;
                            break;
                        }
                        else
                        {
                            if (nextholidayDate == e.Day.Date)
                            {

                                e.Cell.BackColor = HexStringToColor("#ffcc00");//Holiday
                                AddTextToDayCell(e, "Holiday");
                                IsHoliday = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (MyAttendance.IsDefaultHoliday(e.Day.Date.DayOfWeek))
                    {
                        e.Cell.BackColor = HexStringToColor("#ffcc00"); //Holiday
                        AddTextToDayCell(e, "Holiday");
                        IsHoliday = true;
                    }
                }

                if (!IsHoliday)
                {
                    int AttendanceStatus = MyAttendance.GetStaffAttendanceStatus(e.Day.Date);
                    if (AttendanceStatus > 0)
                    {
                        e.Cell.BackColor = HexStringToColor("#a4d805"); // Full day
                    }

                    AddTextToDayCell(e, "Other");

                }
            }
            else
            {
                e.Cell.BackColor = HexStringToColor("#ffc1c1"); // Not batch
                AddTextToDayCell(e, "NotBatch");
            }

        }

        private bool IsDateInBath(DateTime SELECTEDDATE)
        {
            int M_Id = 0;
            bool valid = false;
            string sql = "SELECT tblbatch.Id FROM tblbatch WHERE tblbatch.Id=" + MyUser.CurrentBatchId + " AND '" + SELECTEDDATE.Date.ToString("s") + "' BETWEEN tblbatch.StartDate AND tblbatch.EndDate";
            MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out M_Id);
                if (M_Id > 0)
                {
                    valid = true;
                }
            }
            return valid;
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


        void AddTextToDayCell(DayRenderEventArgs e, string Type)
        {

            string TextColor = "#000000";
            if (e.Day.IsOtherMonth)
            {
                TextColor = "#999999";
            }
            if (Type == "Holiday")
            {
                e.Cell.Text = "<a href=\"javascript:LoadPopup()\" style=\"color:" + TextColor + "\">" + e.Day.DayNumberText;
            }
            else if (Type == "NotBatch")
            {
                e.Cell.Text = "<a href=\"javascript:LoadnotBatchPopup()\" style=\"color:" + TextColor + "\">" + e.Day.DayNumberText;
            }
            else if (e.Day.Date > DateTime.Now.Date)
            {

                e.Cell.Text = "<a href=\"javascript:LoadFuturePopup()\" style=\"color:" + TextColor + "\">" + e.Day.DayNumberText;
            }
            else
            {
                e.Cell.Text = "<a href=\"MarkStaffAttendancePage.aspx?Date=" + e.Day.Date + "\" style=\"color:" + TextColor + "\">" + e.Day.DayNumberText;
            }
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {

            Calendar1.SelectedDate = new DateTime().Date;
            Load_Calender();
        }

        protected void Calendar1_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            Session["StaffDate"] = Calendar1.VisibleDate;
            Load_Calender();
        }

        protected void Lnk_Unmarked_Click(object sender, EventArgs e)
        {
            Session["StaffDate"] = MyUser.GetDareFromText(lbldate.Text);
            Load_Calender();
        }

        protected void Lnk_Today_Click(object sender, EventArgs e)
        {
            Session["StaffDate"] = DateTime.Now.Date;
            Load_Calender();
        }
      
    }
}
