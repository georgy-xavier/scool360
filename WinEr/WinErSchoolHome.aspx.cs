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
using System.Drawing;
using System.Web.Services;
using System.Collections.Generic;

namespace WinEr
{
    public partial class WinErSchoolHome : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private ConfigManager MyConfig;
        private Attendance MyAttendance;
        private DataSet holidaydataset, EventDataset;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyConfig = MyUser.GetConfigObj();
            MyAttendance = MyUser.GetAttendancetObj();
            if (MyAttendance == null)
            {
                Response.Redirect("LoginErr.htm");
                //no rights for this user.
            }
            else
            {
                if (WinerUtlity.NeedCentrelDB())
                {
                    if (Session[WinerConstants.SessionSchool] == null)
                    {
                        Response.Redirect("Logout.aspx");
                    }
                    objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                }
                if (!IsPostBack)
                {
                   // Load_DrpDashboard();
                   // LoadLogo();
                 //   LoadSchoolDetails();
                  //  this.HomeInfo.InnerHtml = MyConfig.getHomeInfo(2, MyUser.UserRoleId, MyUser.CurrentBatchId, MyUser.UserId);
                    LoadCalenderDetails();
                //    LoadBirthdays();
                   // LoadContentSlide();
                    if (!MyUser.HaveActionRignt(923))
                    {

                        TabStaffBirthDay.Visible = false;//View Staff Birthday
                    }
                   // if (!MyUser.HaveActionRignt(92))
                  //  {
                      //  Lmg_AddEvent.Visible = false;
                       // Lnk_AddEvent.Visible = false;
                   // }
                }
            }
        }

        //private void Load_DrpDashboard()
        //{
        //    Drp_Dashboard.Items.Clear();
        //    MyReader = MyUser.GetDashboardPages_hasRights();
        //    if (MyReader.HasRows)
        //    {
        //        Drp_Dashboard.Items.Add(new ListItem("Select Dashboard", "-1"));
        //        while (MyReader.Read())
        //        {

        //            Drp_Dashboard.Items.Add(new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString()));

        //        }
        //    }


        //    PanelDashborad.Visible = false;
        //    if (Drp_Dashboard.Items.Count > 2)
        //    {
        //        PanelDashborad.Visible = true;
        //    }

        //}

        public class MyGenClass
        {
            KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
            OdbcDataReader MyReader = null;

            public string GetSchoolData()
            {
                SchoolClass objSchool = (SchoolClass)HttpContext.Current.Session[WinerConstants.SessionSchool];
               // MysqlClass dd = new MysqlClass(objSchool.ConnectionString);
               
                string schoolName = objSchool.SchoolName.ToString();
                //string Sql = "SELECT SchoolName,Address,Disc FROM tblschooldetails WHERE Id=1";
               // MyReader = MysqlDb.ExecuteQuery(Sql);
                //if (MyReader.HasRows)
                //{
                //   // Lbl_SchoolName.Text = MyReader.GetValue(0).ToString();
                //   // Lbl_Subhead.Text = MyReader.GetValue(1).ToString();
                //    schoolName = MyReader.GetValue(0).ToString();
                //}
                return schoolName;
            }
            public string GetBirthdaySlide(out int BirthdayCount)
            {
                Attendance MyAttendance = MyUser.GetAttendancetObj();
                string StaffPageName = "StaffSessionMaker.aspx?StaffId=";
                string BirthdayStr = "";
                BirthdayCount = 0;
                DateTime CurrentDate = DateTime.Now.Date;
                int Month = CurrentDate.Month;
                int Day = CurrentDate.Day;
                int Year = CurrentDate.Year;
                //Staff Birthday
                string sql = "SELECT DISTINCT t.`Id`,t.`SurName`, date_format( tblstaffdetails.Dob , '%d/%m/%Y'),r.RoleName  FROM tbluser t  inner join tblstaffdetails on tblstaffdetails.UserId = t.Id  inner join tblrole r on t.`RoleId`=r.`Id` where t.`Status`=1 AND r.`Type`='Staff' AND Month(tblstaffdetails.Dob)=" + Month + " AND Day(tblstaffdetails.Dob)=" + Day + " ORDER BY Day(tblstaffdetails.Dob),t.`SurName`";
                MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        string StId = MyReader.GetValue(0).ToString();
                        string Name = MyReader.GetValue(1).ToString();
                        DateTime DOB = General.GetDateTimeFromText(MyReader.GetValue(2).ToString());
                        string Role = MyReader.GetValue(3).ToString();
                        int Age = Year - DOB.Year;
                        string ImgUrl = "Handler/ImageReturnHandler.ashx?id=" + int.Parse(StId) + "&type=StaffImage";
                        // MyUser.GetImageUrl("StaffImage", int.Parse(StId));
                        BirthdayStr = BirthdayStr + " <div class=\"BirthdaySlide scrollStyle scrollbar force-overflow\">  <table width=\"100%\" cellspacing=\"0\">  <tr> <td style=\"padding-left:10px;width:20%\"> <img src=\"images/cake.png\" alt=\"\" width=\"40px\" />  </td> <td align=\"center\" valign=\"middle\" style=\"color:#00586f;font-weight:bold\" >   <h2> Just Turned " + Age + "</h2>     </td>        <td style=\"padding-right:10px;\" >   <img src=\"images/birthdayCelebration.jpg\" alt=\"\" width=\"60px\" />    </td>    </tr><tr > <td valign=\"middle\" align=\"center\" style=\"border-top:solid 4px #ff359a;padding-top:10px;\">     <img src=\"" + ImgUrl + "\" alt=\"\" width=\"120px\" style=\"ridge 1px #c7c7c7;\" />     </td>    <td colspan=\"2\" style=\"border-top:solid 4px #ff359a;padding-top:10px;\">    <table width=\"100%\"><tr><td style=\"width:50%;text-align:-webkit-center;\">  Name &nsbp;&nsbp;:  </td>  <td  style=\"font-weight:500;text-align: -webkit-left;\"> " + Name + "    </td>   </tr> <tr>  <td style=\"width:50%;\" align=\"right\">  Role:   </td>  <td align=\"left\" style=\"font-weight:500;\">  " + Role + "   </td>   </tr>  <tr>   <td >     </td>   <td align=\"left\">     <a style=\"color:Transparent;\" href=\"" + StaffPageName + StId + "\"> <img src=\"Pics/info.png\" alt=\"\" width=\"30px\" /></a>   </td>  </tr>   </table>   </td>  </tr>  </table>    </div> ";
                        BirthdayCount++;
                    }
                }



                return BirthdayStr;
            }
            public string UpcomingEventPresent(out int EventCount)
            {
                Attendance MyAttendance = MyUser.GetAttendancetObj();
                EventCount = 0;
                string EventStr = "";
                string sql = "SELECT DISTINCT( tblcalender_event.EventId) FROM tblcalender_event INNER JOIN tblmasterdate ON tblmasterdate.Id=tblcalender_event.DateId WHERE tblmasterdate.`date`>='" + DateTime.Now.Date.ToString("s") + "' ORDER BY tblmasterdate.`date`";
                MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {

                        string EventId = MyReader.GetValue(0).ToString();
                        string EventName = "", Description = "", DateSr = "", Seperator = "";
                        sql = "SELECT DISTINCT tbleventmaster.EventName,tbleventmaster.Description,date_format( tblmasterdate.date , '%d/%m/%Y')  FROM tblcalender_event INNER JOIN tblmasterdate ON tblmasterdate.Id=tblcalender_event.DateId INNER JOIN tbleventmaster ON tbleventmaster.Id=tblcalender_event.EventId WHERE tblcalender_event.EventId=" + EventId;
                        OdbcDataReader EventReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
                        if (EventReader.HasRows)
                        {
                            while (EventReader.Read())
                            {

                                EventName = EventReader.GetValue(0).ToString();
                                Description = EventReader.GetValue(1).ToString();
                                DateSr = DateSr + Seperator + EventReader.GetValue(2).ToString();
                                Seperator = " , ";
                            }
                        }

                        string DateNewString = MyAttendance.GetFormatedDateString_EventView(DateSr);
                        EventStr = EventStr + "<div class=\"EventSlide scrollStyle scrollbar force-overflow\" >     <table width=\"100%\">  <tr>  <td style=\"width:20%;padding-left:10px;\">  <img src=\"Pics/calendar_empty.png\" alt=\"\" width=\"40px\" />  </td>  <td align=\"center\" valign=\"middle\" style=\"color:#00586f;font-weight:bold\" >    <h2> " + EventName + " </h2>   </td>   <td style=\"width:35%;font-size:11px;font-weight:bold\">   " + DateNewString + "   </td> </tr>  <tr>  <td colspan=\"3\"  align=\"left\" style=\"border-top:solid 4px #0094b9;padding-top:10px;\">   " + Description + "   </td>    </tr>  </table>     </div>  ";
                        EventCount++;
                    }
                }
                else if (EventCount == 0)
                {
                    EventStr = "   <div class=\"EventSlide scrollStyle scrollbar force-overflow\" >  <table width=\"100%\">  <tr>  <td style=\"width:20%;padding-left:10px;\">  <img src=\"Pics/calendar_empty.png\" alt=\"\" width=\"40px\" /> </td>  <td align=\"center\" valign=\"middle\" style=\"color:#00586f;font-weight:bold\" >   </td> <td style=\"width:30%;font-size:11px;font-weight:bold\">    </td>  </tr> <tr>  <td colspan=\"3\" align=\"center\" style=\"border-top:solid 4px #0094b9;padding-top:10px;\">  <h2> No Upcoming Events Found </h2>   </td>    </tr>  </table>   </div>";
                    EventCount++;
                }

                return EventStr;

            }
        }


        [WebMethod (EnableSession=true)]
        public static string[] LoadContentSlide()
        {
            MyGenClass genCls = new MyGenClass();
            string ImageUrl = "Handler/ImageReturnHandler.ashx?id=1&type=Logo";
            // string Sql = "SELECT SchoolName,Address,Disc FROM tblschooldetails WHERE Id=1";
            string schoolName = genCls.GetSchoolData();
            int SlideCount = 1;
            int BirthdayCount = 0, EventCount = 0;
            string ImageStr = "<div class=\"images\"> </div>";
            string HomeStr = "<!-- first slide --><div style=\"background-color:White;width:100%;\"><img src=\"" + ImageUrl + "\" alt=\"School\" style=\"max-width:80px;margin:auto\"/><h4>" + schoolName + "</h4><p>Play To View Upcoming Events </p> <center><i class=\"fa fa-play-circle\" style=\"font-size:48px;color:#673AB7\" onclick='$(\".slidetabs\").data(\"slideshow\").play();'/></center></div>";
            string BirthdayStr = "   <!-- second slide -->    ";
            string EventStr = "   <!-- third slide --> ";
            BirthdayStr = genCls.GetBirthdaySlide(out BirthdayCount);
            EventStr = genCls.UpcomingEventPresent(out EventCount);
            ImageStr = "<div class=\"images\">   " + HomeStr + BirthdayStr + "    " + EventStr + "    </div>";
           // this.ImagesDiv.InnerHtml = ImageStr;
            SlideCount = SlideCount + BirthdayCount + EventCount;

            string SlideTabStr = " <div class=\"slidetabs\">   ";

            for (int i = 1; i <= SlideCount; i++)
            {
                SlideTabStr = SlideTabStr + " <a href=\"#\">" + i + "</a>";
            }

            SlideTabStr = SlideTabStr + " </div> ";
            string[] slide = new String[2];
            slide[0] = SlideTabStr.ToString();
            slide[1] = ImageStr.ToString();
        //    this.SlideTabsDiv.InnerHtml = SlideTabStr;
            return slide;

        }
        [WebMethod(EnableSession = true)]
        public static string LoadHomeInfoData()
        {

            KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
            ConfigManager MyConfig = MyUser.GetConfigObj();
            string data = MyConfig.getHomeInfo(2, MyUser.UserRoleId, MyUser.CurrentBatchId, MyUser.UserId);
            return data;

        }
        [WebMethod(EnableSession = true)]
        public static string[] LoadBirthdays()
        {
            MyGenClass GenCls = new MyGenClass();
            KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
            Attendance MyAttendance = MyUser.GetAttendancetObj();
            OdbcDataReader MyReader = null;
            int ChekCount = 0;
            bool StaffToday = false, AddEvent = true, SendSms = true;
            DateTime CurrentDate = DateTime.Now.Date;
            int Month = CurrentDate.Month;
            int Day = CurrentDate.Day;
            int StaffCount = 0, StudentLimit = 6;
            string Name = "", StId = "",sql="";
            string StaffStr = "<div class=\"BirthdayData scrollStyle\"> <table style=\"width:100%;\">           ";
            string StrRows = "", StaffPageName = "StaffSessionMaker.aspx?StaffId=";
            CurrentDate = DateTime.Now.Date;
            Month = CurrentDate.Month;
            Day = CurrentDate.Day;

           sql = "SELECT DISTINCT t.`Id`,t.`SurName`, tblstaffdetails.Dob  FROM tbluser t  inner join tblstaffdetails on tblstaffdetails.UserId = t.Id  inner join tblrole r on t.`RoleId`=r.`Id` where t.`Status`=1 AND r.`Type`='Staff' AND Month(tblstaffdetails.Dob)=" + Month + " AND Day(tblstaffdetails.Dob)=" + Day + " ORDER BY Day(tblstaffdetails.Dob),t.`SurName`";
            MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    StId = MyReader.GetValue(0).ToString();
                    Name = MyReader.GetValue(1).ToString();
                    StrRows = StrRows + "<tr>  <td class=\"StudentName\" align=\"left\">   <a href=\"" + StaffPageName + StId + "\" style=\"color:Red;\" title=\"Click\" >" + Name + "</a>    </td>   <td class=\"Day\">  <a href=\"" + StaffPageName + StId + "\" style=\"color:Red;font-weight:bold\" title=\"Click\"  > Today</a>   </td> </tr>";
                    StaffToday = true;
                    StaffCount++;
                }
            }

            if (StaffCount < StudentLimit)
            {
                CurrentDate = CurrentDate.AddDays(1);
                Month = CurrentDate.Month;
                Day = CurrentDate.Day;
                sql = "SELECT DISTINCT t.`Id`,t.`SurName`,tblstaffdetails.Dob  FROM tbluser t  inner join tblstaffdetails on tblstaffdetails.UserId = t.Id  inner join tblrole r on t.`RoleId`=r.`Id` where t.`Status`=1 AND r.`Type`='Staff' AND Month(tblstaffdetails.Dob)=" + Month + " AND Day(tblstaffdetails.Dob)=" + Day + " ORDER BY Day(tblstaffdetails.Dob),t.`SurName`";
                MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        StId = MyReader.GetValue(0).ToString();
                        Name = MyReader.GetValue(1).ToString();
                        StrRows = StrRows + "<tr>  <td class=\"StudentName\" align=\"left\">   <a href=\"" + StaffPageName + StId + "\" style=\"color:Black;\" title=\"Click\" >" + Name + "</a>    </td>   <td class=\"Day\">  <a href=\"" + StaffPageName + StId + "\" style=\"color:#ff8000;font-weight:bold\" title=\"Click\"  > Tomorrow</a>   </td> </tr>";
                        StaffCount++;
                    }
                }
            }

            if (StaffCount < StudentLimit)
            {
                ChekCount = 0;
                while (StaffCount < StudentLimit)
                {
                    DateTime OtherDays;
                    sql = "SELECT DISTINCT t.`Id`,t.`SurName`,date_format( tblstaffdetails.Dob, '%d/%m/%Y')  FROM tbluser t  inner join tblstaffdetails on tblstaffdetails.UserId = t.Id  inner join tblrole r on t.`RoleId`=r.`Id` where t.`Status`=1 AND r.`Type`='Staff' AND Month(tblstaffdetails.Dob)=" + Month + " AND Day(tblstaffdetails.Dob)>" + Day + " ORDER BY Day(tblstaffdetails.Dob),t.`SurName`";
                    MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        while (MyReader.Read())
                        {
                            if (StaffCount < StudentLimit)
                            {
                                StId = MyReader.GetValue(0).ToString();
                                Name = MyReader.GetValue(1).ToString();
                                OtherDays = General.GetDateTimeFromText(MyReader.GetValue(2).ToString());
                                StrRows = StrRows + "<tr>  <td class=\"StudentName\" align=\"left\">   <a href=\"" + StaffPageName + StId + "\" style=\"color:Black;\" title=\"Click\" >" + Name + "</a>    </td>   <td class=\"Day\">  <a href=\"" + StaffPageName + StId + "\" style=\"color:Black;\" title=\"Click\"  > " + OtherDays.ToString("dd , MMM") + " </a>   </td> </tr>";
                                StaffCount++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    ChekCount++;
                    if (ChekCount < 11)
                    {
                        CurrentDate = CurrentDate.AddMonths(1);
                        Month = CurrentDate.Month;
                        Day = 0;
                    }
                    else
                    {
                        break;
                    }
                }
            }


            if (StaffCount == 0)
            {
                StaffStr = "<div id=\"BirthdayData scrollStyle\"> No Staff Found </div>";
            }
            else
            {
                StaffStr = StaffStr + StrRows + "    </table> </div>";
            }


            string[] BirthayData = new String[6];

            if (!MyUser.HaveActionRignt(92))
            {
                AddEvent = false;
            }
            if (!MyUser.HaveActionRignt(893))
            {
                SendSms = false;
            }

            BirthayData[0] = StaffStr.ToString();
            BirthayData[1] = StaffToday.ToString();
            BirthayData[2] = AddEvent.ToString();
            BirthayData[3] = SendSms.ToString();
            return BirthayData;
        }
       
        protected void Lnk_AddEvent_Click(object sender, EventArgs e)
        {
            Response.Redirect("HolidayManager.aspx");
        }

        protected void Lmg_AddEvent_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("HolidayManager.aspx");
        }
        # region Calender Manager
        private void LoadCalenderDetails()
        {
            Calendar1.SelectedDate = new DateTime().Date;
            string type = "all";
            int Classid = 0;
            holidaydataset = MyAttendance.MyAssociatedHolidays(type, Classid, MyUser.User_Id);
            EventDataset = MyAttendance.MyAssociatedEventsDataSet(Classid, MyUser.User_Id);
        }

        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            bool done = false;
            if (IsDateInBath(e.Day.Date))
            {

                DateTime nextholidayDate;
                if (holidaydataset != null && holidaydataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in holidaydataset.Tables[0].Rows)
                    {
                        nextholidayDate = (DateTime)dr[0];
                        if (MyAttendance.IsDefaultHoliday(e.Day.Date.DayOfWeek))
                        {
                            //e.Cell.BackColor = HexStringToColor("#ffcc00");//Holiday
                            e.Cell.BackColor = HexStringToColor("#e98074");

                            AddTextToDayCell(e, "Holiday", "Default Holiday");
                            done = true;
                            break;
                        }
                        else
                        {
                            if (nextholidayDate == e.Day.Date)
                            {

                                //e.Cell.BackColor = HexStringToColor("#ffcc00");//Holiday
                                e.Cell.BackColor = HexStringToColor("#e98074");
                                AddTextToDayCell(e, "Holiday", dr[2].ToString());
                                done = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (MyAttendance.IsDefaultHoliday(e.Day.Date.DayOfWeek))
                    {
                        // e.Cell.BackColor = HexStringToColor("#ffcc00"); //Holiday
                        e.Cell.BackColor = HexStringToColor("#e98074");
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

                            e.Cell.BackColor = HexStringToColor("#afd275");
                            AddTextToDayCell(e, "Event", dr[2].ToString() + "$" + dr[3].ToString());
                            done = true;
                            break;
                        }

                    }
                }

            }
            else
            {
                e.Cell.BackColor = HexStringToColor("#ffc1c1"); // Not batch
                AddTextToDayCell(e, "NotBatch", "NoBatch");
                done = true;
            }
            if (!done)
            {
                AddTextToDayCell(e, "Other", "Other");
            }
        }

        void AddTextToDayCell(DayRenderEventArgs e, string Type, string Name)
        {

            string TextColor = "#646464", HoliTextColor = "#dadbdc", EventTextColor = "#ffffff";
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
                e.Cell.Text = "<a href=\"javascript:alert('" + HolidayMessage + "')\" style=\"color:" + HoliTextColor + "\">" + e.Day.DayNumberText;
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
                e.Cell.Text = "<a href=\"javascript:LoadPopup('" + str[0] + "')\" style=\"color:" + EventTextColor + "\">" + e.Day.DayNumberText;
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

        protected void Calendar1_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            Calendar1.SelectedDate = new DateTime().Date;
            LoadCalenderDetails();
        }

        # endregion
        //private void LoadBirthdays()
        //{
        //    int ChekCount = 0;
        //    bool StaffToday = false;
        //    DateTime CurrentDate = DateTime.Now.Date;
        //    int Month = CurrentDate.Month;
        //    int Day = CurrentDate.Day;
        //    int  StaffCount = 0, StudentLimit = 6;
        //    string Name = "", StId = "";
        //    string StaffStr = "<div class=\"BirthdayData\"> <table width=\"210px\" cellspacing=\"0\">           ";
        //    string StrRows = "", StaffPageName = "StaffSessionMaker.aspx?StaffId=";
           

        //    StrRows = "";
        //    CurrentDate = DateTime.Now.Date;
        //    Month = CurrentDate.Month;
        //    Day = CurrentDate.Day;

        //    string sql = "SELECT DISTINCT t.`Id`,t.`SurName`, tblstaffdetails.Dob  FROM tbluser t  inner join tblstaffdetails on tblstaffdetails.UserId = t.Id  inner join tblrole r on t.`RoleId`=r.`Id` where t.`Status`=1 AND r.`Type`='Staff' AND Month(tblstaffdetails.Dob)=" + Month + " AND Day(tblstaffdetails.Dob)=" + Day + " ORDER BY Day(tblstaffdetails.Dob),t.`SurName`";
        //    MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
        //    if (MyReader.HasRows)
        //    {
        //        while (MyReader.Read())
        //        {
        //            StId = MyReader.GetValue(0).ToString();
        //            Name = MyReader.GetValue(1).ToString();
        //            StrRows = StrRows + "<tr>  <td class=\"StudentName\" align=\"left\">   <a href=\"" + StaffPageName + StId + "\" style=\"color:Red;\" title=\"Click\" >" + Name + "</a>    </td>   <td class=\"Day\">  <a href=\"" + StaffPageName + StId + "\" style=\"color:Red;font-weight:bold\" title=\"Click\"  > Today</a>   </td> </tr>";
        //            StaffToday = true;
        //            StaffCount++;
        //        }
        //    }

        //    if (StaffCount < StudentLimit)
        //    {
        //        CurrentDate = CurrentDate.AddDays(1);
        //        Month = CurrentDate.Month;
        //        Day = CurrentDate.Day;
        //        sql = "SELECT DISTINCT t.`Id`,t.`SurName`,tblstaffdetails.Dob  FROM tbluser t  inner join tblstaffdetails on tblstaffdetails.UserId = t.Id  inner join tblrole r on t.`RoleId`=r.`Id` where t.`Status`=1 AND r.`Type`='Staff' AND Month(tblstaffdetails.Dob)=" + Month + " AND Day(tblstaffdetails.Dob)=" + Day + " ORDER BY Day(tblstaffdetails.Dob),t.`SurName`";
        //        MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
        //        if (MyReader.HasRows)
        //        {
        //            while (MyReader.Read())
        //            {
        //                StId = MyReader.GetValue(0).ToString();
        //                Name = MyReader.GetValue(1).ToString();
        //                StrRows = StrRows + "<tr>  <td class=\"StudentName\" align=\"left\">   <a href=\"" + StaffPageName + StId + "\" style=\"color:Black;\" title=\"Click\" >" + Name + "</a>    </td>   <td class=\"Day\">  <a href=\"" + StaffPageName + StId + "\" style=\"color:#ff8000;font-weight:bold\" title=\"Click\"  > Tomorrow</a>   </td> </tr>";
        //                StaffCount++;
        //            }
        //        }
        //    }

        //    if (StaffCount < StudentLimit)
        //    {
        //        ChekCount = 0;
        //        while (StaffCount < StudentLimit)
        //        {
        //            DateTime OtherDays;
        //            sql = "SELECT DISTINCT t.`Id`,t.`SurName`,date_format( tblstaffdetails.Dob, '%d/%m/%Y')  FROM tbluser t  inner join tblstaffdetails on tblstaffdetails.UserId = t.Id  inner join tblrole r on t.`RoleId`=r.`Id` where t.`Status`=1 AND r.`Type`='Staff' AND Month(tblstaffdetails.Dob)=" + Month + " AND Day(tblstaffdetails.Dob)>" + Day + " ORDER BY Day(tblstaffdetails.Dob),t.`SurName`";
        //            MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
        //            if (MyReader.HasRows)
        //            {
        //                while (MyReader.Read())
        //                {
        //                    if (StaffCount < StudentLimit)
        //                    {
        //                        StId = MyReader.GetValue(0).ToString();
        //                        Name = MyReader.GetValue(1).ToString();
        //                        OtherDays = General.GetDateTimeFromText(MyReader.GetValue(2).ToString());
        //                        StrRows = StrRows + "<tr>  <td class=\"StudentName\" align=\"left\">   <a href=\"" + StaffPageName + StId + "\" style=\"color:Black;\" title=\"Click\" >" + Name + "</a>    </td>   <td class=\"Day\">  <a href=\"" + StaffPageName + StId + "\" style=\"color:Black;\" title=\"Click\"  > " + OtherDays.ToString("dd , MMM") + " </a>   </td> </tr>";
        //                        StaffCount++;
        //                    }
        //                    else
        //                    {
        //                        break;
        //                    }
        //                }
        //            }
        //            ChekCount++;
        //            if (ChekCount < 11)
        //            {
        //                CurrentDate = CurrentDate.AddMonths(1);
        //                Month = CurrentDate.Month;
        //                Day = 0;
        //            }
        //            else
        //            {
        //                break;
        //            }
        //        }
        //    }


        //    if (StaffCount == 0)
        //    {
        //        StaffStr = "<div id=\"BirthdayData\"> No Staff Found </div>";
        //    }
        //    else
        //    {
        //        StaffStr = StaffStr + StrRows + "    </table> </div>";
        //    }


        //    if (StaffToday)
        //    {
        //        ImageStaff.ImageUrl = "~/images/user_r.png";
        //        Label2.ForeColor = Color.Red;
        //    }

        //    this.StaffDiv.InnerHtml = StaffStr;


        //    if (!MyUser.HaveActionRignt(923))
        //    {

        //        Panel_Birthday.Visible = false;
        //    }
           
        //}



   

       // private void LoadLogo()
       // {
            //Img_Logo.ImageUrl = "Handler/ImageReturnHandler.ashx?id=1&type=Logo";
            //String ImageUrl = "";
            //String Sql = "SELECT LogoUrl FROM tblschooldetails WHERE Id=1";
            //MyReader = MyConfig.m_MysqlDb.ExecuteQuery(Sql);
            //if (MyReader.HasRows)
            //{
            //    ImageUrl = MyReader.GetValue(0).ToString();
            //}
            //else
            //{
            //    ImageUrl = "img.png";
            //}

            //Img_Logo.ImageUrl =  WinerUtlity.GetRelativeFilePath(objSchool)+"ThumbnailImages/" + ImageUrl;

       // }

        //private void LoadSchoolDetails()
        //{
        //    string Sql = "SELECT SchoolName,Address,Disc FROM tblschooldetails WHERE Id=1";
        //    MyReader = MyConfig.m_MysqlDb.ExecuteQuery(Sql);
        //    if (MyReader.HasRows)
        //    {
        //        Lbl_SchoolName.Text = MyReader.GetValue(0).ToString();
        //        Lbl_Subhead.Text = MyReader.GetValue(1).ToString();
        //        string[] aa = new string[2];
        //        //if (MyReader.GetValue(2).ToString().Trim() == "")
        //        //{
        //        //    Pnl_aboutUs.Visible = false;
        //        //}
        //        //this.Description.InnerHtml = MyReader.GetValue(2).ToString();

        //    }
        //}




       

        //protected void Drp_Dashboard_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string page = MyUser.SelectDashBoard(Drp_Dashboard.SelectedValue, MyUser.SELECTEDMODE);
        //    if (page != "")
        //    {
        //        Response.Redirect(page);
        //    }
        //}

        //protected void Btn_Default_Click(object sender, EventArgs e)
        //{
        //    MyUser.SetDefaultDashboard(1);
        //}



    }
}
