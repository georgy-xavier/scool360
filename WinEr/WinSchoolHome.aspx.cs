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
using WebChart;
using System.Drawing; 
using WinBase;
using WinEr;
using System.Web.Services;
using System.Web.Script.Serialization;
public partial class WinSchoolHome : System.Web.UI.Page
{
    private StudentManagerClass MyStudMang;
    private ConfigManager MyConfig;
    private Attendance MyAttendance;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    private DataSet holidaydataset, EventDataset;
    private SchoolClass objSchool = null;
    protected void Page_Load(object sender, EventArgs e)
    {
 
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyStudMang = MyUser.GetStudentObj();
        MyConfig = MyUser.GetConfigObj();
        MyAttendance = MyUser.GetAttendancetObj();
        if (MyAttendance == null)
        {
            Response.Redirect("LoginErr.htm");
            //no rights for this user.
        }
        else if (MyStudMang == null)
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
                //some initlization
                //Load_DrpDashboard();
                //LoadLogo();
                //LoadSchoolDetails();
               // LoadGeneralDetails();
             //   this.HomeInfo.InnerHtml = MyConfig.getHomeInfo(1, MyUser.UserRoleId, MyUser.CurrentBatchId, MyUser.UserId);
                LoadCalenderDetails(); 
                checkConfig();
                if (!MyUser.HaveActionRignt(922) && !MyUser.HaveActionRignt(923))
                {
                    //Panel_Birthday.Visible = false;
                }
                else if (!MyUser.HaveActionRignt(922))
                {
                    TabPanel1.Visible = false;//View Student Birthday
                }
                else if (!MyUser.HaveActionRignt(923))
                {
                    TabPanel2.Visible = false;//View Staff Birthday
                }  
              //  LoadContentSlide();
               
                MyUser.SetDefaultDashboard(1);

            }
            // DreawGenterChart();
            ChartControl.PerformCleanUp();
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

    //protected void Page_init(object sender, EventArgs e)
    //{
      
    //}

    //protected void principaldashboard_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("PrincipalDashboard.aspx");
    //   // MyUser.SetDefaultDashboard(1);
       
    //}
    protected void Btn_Default_Click(object sender, EventArgs e)
    {
        MyUser.SetDefaultDashboard(1);
    }

    public class MyGenClass
    {
        int _Mode = 1;
        KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
        OdbcDataReader MyReader = null;

        public int[] GetActionId()
        {
            int i = 0,x=1;
            
            string sql = "select distinct(tblaction.id), tblaction.MenuName from tblaction  inner join tblroleactionmap on tblaction.Id = tblroleactionmap.ActionId  inner join tblmoduleactionmap on tblaction.Id = tblmoduleactionmap.ActionId where tblaction.ActionType = 'HomeInfoLink' and tblmoduleactionmap.ModuleId in  ( select tblmodule.Id from tblmodule where tblmodule.ModuleType in(" + _Mode + ",3)) and tblaction.Id in (select tblroleactionmap.ActionId from tblroleactionmap where tblroleactionmap.RoleId =" + MyUser.UserRoleId + ") order by tblaction.Order";
             Attendance MyAttendance = MyUser.GetAttendancetObj();
             MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
             x = MyReader.RecordsAffected;
             int[] actionId = new int[x]; 
             if (MyReader.HasRows)
             {
                 while (MyReader.Read())
                 {
                     actionId[i] = int.Parse(MyReader.GetValue(0).ToString());
                     i++;
                 }
             }
             return actionId;
        }
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
            string BirthdayStr = "";
            BirthdayCount = 0;
            DateTime CurrentDate = DateTime.Now.Date;
            int Month = CurrentDate.Month;
            int Day = CurrentDate.Day;
            int Year = CurrentDate.Year;
            string StudentPageName = "StudentSessionMaker.aspx?StudentId=", StaffPageName = "StaffSessionMaker.aspx?StaffId=";
            //Student Birthday

            string sql = "SELECT tblstudent.Id,tblstudent.StudentName,date_format( tblstudent.DOB , '%d/%m/%Y'),tblclass.ClassName FROM tblstudent INNER JOIN tblclass ON tblclass.Id=tblstudent.LastClassId WHERE tblstudent.`Status`=1 AND Month(tblstudent.DOB)=" + Month + " AND Day(tblstudent.DOB)=" + Day + " ORDER BY Day(tblstudent.DOB),tblstudent.StudentName";
            MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    string StId = MyReader.GetValue(0).ToString();
                    string Name = MyReader.GetValue(1).ToString();
                    DateTime DOB = General.GetDateTimeFromText(MyReader.GetValue(2).ToString());
                    string ClassName = MyReader.GetValue(3).ToString();
                    int Age = Year - DOB.Year;
                    string ImgUrl = "Handler/ImageReturnHandler.ashx?id=" + int.Parse(StId) + "&type=StudentImage";
                    //  MyUser.GetImageUrl("StudentImage", int.Parse(StId));
                    BirthdayStr = BirthdayStr + " <div class=\"BirthdaySlide scrollStyle scrollbar force-overflow\">  <table width=\"100%\" cellspacing=\"0\">  <tr> <td style=\"padding-left:10px;width:20%\"> <img src=\"pics/bday.png\" alt=\"\" width=\"40px\" />  </td> <td align=\"center\" valign=\"middle\" style=\"color:#00586f;font-weight:bold\" >   <h3> Just Turned " + Age + "</h3>     </td>        <td style=\"padding-right:10px;\" >   <img src=\"images/birthdayCelebration.jpg\" alt=\"\" width=\"60px\" />    </td>    </tr><tr > <td valign=\"middle\" align=\"center\" style=\"border-top:solid 4px #ff359a;padding-top:10px;\">     <img src=\"" + ImgUrl + "\" alt=\"\" width=\"110px\" style=\"border: ridge 1px #c7c7c7;\" />     </td>    <td colspan=\"2\" style=\"border-top:solid 4px #ff359a;padding-top:10px;\">    <table width=\"100%\">    <tr><td style=\"width:50%;\" align=\"right\">  Name:  </td>  <td align=\"left\" style=\"font-weight:bold\"> " + Name + "    </td>   </tr> <tr>  <td style=\"width:50%;\" align=\"right\">  Class Name:   </td>  <td align=\"left\" style=\"font-weight:bold\">   " + ClassName + "   </td>   </tr>  <tr>   <td >     </td>   <td align=\"left\">     <a  style=\"color:Transparent;\" href=\"" + StudentPageName + StId + "\"> <img src=\"Pics/information.png\" alt=\"\" width=\"25px\" /></a>   </td>  </tr>   </table>   </td>  </tr>  </table>    </div> ";
                    BirthdayCount++;
                }
            }


            //Staff Birthday

            sql = "SELECT DISTINCT t.`Id`,t.`SurName`, date_format( tblstaffdetails.Dob , '%d/%m/%Y'),r.RoleName  FROM tbluser t  inner join tblstaffdetails on tblstaffdetails.UserId = t.Id  inner join tblrole r on t.`RoleId`=r.`Id` where t.`Status`=1 AND r.`Type`='Staff' AND Month(tblstaffdetails.Dob)=" + Month + " AND Day(tblstaffdetails.Dob)=" + Day + " ORDER BY Day(tblstaffdetails.Dob),t.`SurName`";
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
                    //MyUser.GetImageUrl("StaffImage", int.Parse(StId));
                    BirthdayStr = BirthdayStr + " <div class=\"BirthdaySlide scrollStyle scrollbar force-overflow\">  <table width=\"100%\" cellspacing=\"0\">  <tr> <td style=\"padding-left:10px;width:20%\"> <img src=\"images/cake.png\" alt=\"\" width=\"40px\" />  </td> <td align=\"center\" valign=\"middle\" style=\"color:#00586f;font-weight:bold\" >   <h2> Just Turned " + Age + "</h2>     </td>        <td style=\"padding-right:10px;\" >   <img src=\"images/birthdayCelebration.jpg\" alt=\"\" width=\"60px\" />    </td>    </tr><tr > <td valign=\"middle\" align=\"center\" style=\"border-top:solid 4px #ff359a;padding-top:10px;\">     <img src=\"" + ImgUrl + "\" alt=\"\" width=\"120px\" style=\"border:ridge 4px silver;\" />     </td>    <td colspan=\"2\" style=\"border-top:solid 4px #ff359a;padding-top:10px;\">    <table width=\"100%\">    <tr><td style=\"width:50%;\" align=\"right\">  Name:  </td>  <td align=\"left\" style=\"font-weight:bold\"> " + Name + "    </td>   </tr> <tr>  <td style=\"width:50%;\" align=\"right\">  Role:   </td>  <td align=\"left\" style=\"font-weight:bold\">  " + Role + "   </td>   </tr>  <tr>   <td >     </td>   <td align=\"left\">     <a style=\"color:Transparent;\" href=\"" + StaffPageName + StId + "\"> <img src=\"Pics/info.png\" alt=\"\" width=\"30px\" /></a>   </td>  </tr>   </table>   </td>  </tr>  </table>    </div> ";
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
                    EventStr = EventStr + "<div class=\"EventSlide scrollStyle scrollbar force-overflow\" >     <table width=\"100%\">  <tr>  <td style=\"width:20%;padding-left:10px;\">  <img src=\"Pics/calendar_empty.png\" alt=\"\" width=\"40px\" />  </td>  <td align=\"center\" valign=\"middle\" style=\"color:#00586f;font-weight:bold\" >    <h5> " + EventName + " </h5>   </td>   <td style=\"width:35%;font-size: 15px;font-weight: 200;\">   " + DateNewString + "   </td> </tr>  <tr>  <td colspan=\"3\"  align=\"left\" style=\"border-top:solid 4px #0094b9;padding-top:10px;\">   " + Description + "   </td>    </tr>  </table>     </div>  ";
                    EventCount++;
                }
            }
            else if (EventCount == 0)
            {
                EventStr = "  <div class=\"EventSlide scrollStyle scrollbar force-overflow\" >  <table width=\"100%\">  <tr>  <td style=\"width:20%;padding-left:10px;\">  <img src=\"Pics/calendar_empty.png\" alt=\"\" width=\"40px\" /> </td>  <td align=\"center\" valign=\"middle\" style=\"color:#00586f;font-weight:bold\" >   </td> <td style=\"width:35%;font-size: 15px;font-weight: 200;\">    </td>  </tr> <tr>  <td colspan=\"3\" align=\"center\" style=\"border-top:solid 4px #0094b9;padding-top:10px;\">  <h2> No Upcoming Events Found </h2>   </td>    </tr>  </table>   </div>";
                EventCount++;
            }

            return EventStr;

        }
    }
    [WebMethod(EnableSession = true)]
    public static string LoadHomeInfoData()
    {
      
        KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
        ConfigManager MyConfig = MyUser.GetConfigObj();
        string data = MyConfig.getHomeInfo(1, MyUser.UserRoleId, MyUser.CurrentBatchId, MyUser.UserId);
        return data;
    
    }
    [WebMethod(EnableSession = true)]
    public static string[] LoadSchoolData()
    {
        OdbcDataReader MyReader = null;
        KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
        ConfigManager MyConfig = MyUser.GetConfigObj();
        string[] Dt = new string[3];
        string Sql = "SELECT SchoolName,Address,Disc FROM tblschooldetails WHERE Id=1";
        MyReader = MyConfig.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
            Dt[0] = MyReader.GetValue(0).ToString();//school name
            Dt[1] = MyReader.GetValue(1).ToString();//shool addres sub
            Dt[2] = MyReader.GetValue(2).ToString();
        }
        return Dt;
    }
    [WebMethod(EnableSession = true)]
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
    public static string[] LoadBirthdays()
    {
        MyGenClass GenCls = new MyGenClass();
        KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
        Attendance MyAttendance = MyUser.GetAttendancetObj();
        OdbcDataReader MyReader = null;
        int ChekCount = 0;
        bool StudToday = false, StaffToday = false,AddEvent=true,SendSms=true;
        DateTime CurrentDate = DateTime.Now.Date;
        int Month = CurrentDate.Month;
        int Day = CurrentDate.Day;
        int StudentCount = 0, StaffCount = 0, StudentLimit = 6;
        string Name = "", StId = "";
        string StudentStr = "<div class=\"BirthdayData scrollStyle\"> <table width=\"100%\" cellspacing=\"0\">           ";
        string StaffStr = "<div class=\"BirthdayData scrollStyle\"> <table width=\"100%\" cellspacing=\"0\">           ";
        string StrRows = "", StudentPageName = "StudentSessionMaker.aspx?StudentId=", StaffPageName = "StaffSessionMaker.aspx?StaffId=";
        string sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.DOB FROM tblstudent WHERE tblstudent.`Status`=1 AND Month(tblstudent.DOB)=" + Month + " AND Day(tblstudent.DOB)=" + Day + " ORDER BY Day(tblstudent.DOB),tblstudent.StudentName";
        MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                StId = MyReader.GetValue(0).ToString();
                Name = MyReader.GetValue(1).ToString();
                StrRows = StrRows + "<tr>  <td class=\"StudentName\" align=\"left\">   <a href=\"" + StudentPageName + StId + "\" style=\"color:Red;\" title=\"Click\" >" + Name + "</a>    </td>   <td class=\"Day\">  <a href=\"" + StudentPageName + StId + "\" style=\"color:Red;font-weight:bold\" title=\"Click\"  > Today</a>   </td> </tr>";
                StudToday = true;
                StudentCount++;
            }
        }

        if (StudentCount < StudentLimit)
        {
            CurrentDate = CurrentDate.AddDays(1);
            Month = CurrentDate.Month;
            Day = CurrentDate.Day;
            sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.DOB FROM tblstudent WHERE tblstudent.`Status`=1 AND Month(tblstudent.DOB)=" + Month + " AND Day(tblstudent.DOB)=" + Day + " ORDER BY Day(tblstudent.DOB),tblstudent.StudentName";
            MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    StId = MyReader.GetValue(0).ToString();
                    Name = MyReader.GetValue(1).ToString();
                    StrRows = StrRows + "<tr>  <td class=\"StudentName\" align=\"left\">   <a href=\"" + StudentPageName + StId + "\" style=\"color:Black;\" title=\"Click\" >" + Name + "</a>    </td>   <td class=\"Day\">  <a href=\"" + StudentPageName + StId + "\" style=\"color:#ff8000;font-weight:bold\" title=\"Click\"  > Tomorrow</a>   </td> </tr>";
                    StudentCount++;
                }
            }
        }

        if (StudentCount < StudentLimit)
        {
            ChekCount = 0;
            while (StudentCount < StudentLimit)
            {

                DateTime OtherDays;
                sql = "SELECT tblstudent.Id,tblstudent.StudentName,date_format( tblstudent.DOB , '%d/%m/%Y') FROM tblstudent WHERE tblstudent.`Status`=1 AND Month(tblstudent.DOB)=" + Month + " AND Day(tblstudent.DOB)>" + Day + " ORDER BY Day(tblstudent.DOB),tblstudent.StudentName";
                MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        if (StudentCount < StudentLimit)
                        {
                            StId = MyReader.GetValue(0).ToString();
                            Name = MyReader.GetValue(1).ToString();
                            OtherDays = General.GetDateTimeFromText(MyReader.GetValue(2).ToString());
                            StrRows = StrRows + "<tr>  <td class=\"StudentName\" align=\"left\">   <a href=\"" + StudentPageName + StId + "\" style=\"color:Black;\" title=\"Click\" >" + Name + "</a>    </td>   <td class=\"Day\">  <a href=\"" + StudentPageName + StId + "\" style=\"color:Black;\" title=\"Click\"  > " + OtherDays.ToString("dd , MMM") + " </a>   </td> </tr>";
                            StudentCount++;
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


        if (StudentCount == 0)
        {
            StudentStr = "<div id=\"BirthdayData scrollStyle\"> No Students Found </div>";
        }
        else
        {
            StudentStr = StudentStr + StrRows + "    </table> </div>";
        }

        StrRows = "";
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
       
      
        BirthayData[0] = StudentStr.ToString();
        BirthayData[1] = StaffStr.ToString();
        BirthayData[2] = StudToday.ToString();
        BirthayData[3] = StaffToday.ToString();
        BirthayData[4] = AddEvent.ToString();
        BirthayData[5] = SendSms.ToString();
        //    this.SlideTabsDiv.InnerHtml = SlideTabStr;
        return BirthayData;
        //if (StudToday)
        //{
        //    ImageStud.ImageUrl = "~/images/graduate-student-avatar.png";
        //    Label1.ForeColor = Color.Red;
        //}

        //if (StaffToday)
        //{
        //    ImageStaff.ImageUrl = "~/images/user_r.png";
        //    Label2.ForeColor = Color.Red;
        //}
      //  this.StudentDiv.InnerHtml = StudentStr;
       // this.StaffDiv.InnerHtml = StaffStr;
    }
    //private void LoadContentSlide()
    //{
    //    int SlideCount = 1;
    //    int BirthdayCount=0,EventCount=0;
    //    string ImageStr="<div class=\"images\">           </div>";
    //    string HomeStr = "<!-- first slide -->	 <div style=\"background-color:White;width:97%;height:235px;\"><img src=\"" + Img_Logo.ImageUrl + "\" alt=\"School\" style=\"float:left;max-width:70px;max-height:100px;margin:0 30px 20px 0\" />     <br />     <h3>" + Lbl_SchoolName.Text + "</h3><h6> Click Play To View Upcoming Events </h6><br /><br /><center><img src=\"Pics/play buttons/black-play.png\" alt=\"School\" width=\"35\" height=\"35\" onclick='$(\".slidetabs\").data(\"slideshow\").play();'/></center></div>";
    //    string BirthdayStr = "   <!-- second slide -->    ";
    //    string EventStr = "   <!-- third slide --> ";
      
    //    BirthdayStr = GetBirthdaySlide(out BirthdayCount);
    //    EventStr = UpcomingEventPresent(out EventCount);
    //    ImageStr = "<div class=\"images\">   " + HomeStr + BirthdayStr + "    " + EventStr + "    </div>";
    //    this.ImagesDiv.InnerHtml = ImageStr;
    //    SlideCount = SlideCount + BirthdayCount + EventCount;

    //    string SlideTabStr = " <div class=\"slidetabs\">   ";

    //    for(int i=1;i<=SlideCount;i++)
    //    {
    //        SlideTabStr=SlideTabStr+" <a href=\"#\">"+i+"</a>";
    //    }

    //    SlideTabStr = SlideTabStr + " </div> ";

    //    this.SlideTabsDiv.InnerHtml = SlideTabStr;

    //}

    //private string UpcomingEventPresent(out int EventCount)
    //{
    //    EventCount = 0;
    //    string EventStr = "";
    //    string sql = "SELECT DISTINCT( tblcalender_event.EventId) FROM tblcalender_event INNER JOIN tblmasterdate ON tblmasterdate.Id=tblcalender_event.DateId WHERE tblmasterdate.`date`>='" + DateTime.Now.Date.ToString("s") + "' ORDER BY tblmasterdate.`date`";
    //    MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
    //    if (MyReader.HasRows)
    //    {
    //        while (MyReader.Read())
    //        {

    //            string EventId = MyReader.GetValue(0).ToString();
    //            string EventName = "", Description = "", DateSr = "", Seperator = "";
    //            sql = "SELECT DISTINCT tbleventmaster.EventName,tbleventmaster.Description,date_format( tblmasterdate.date , '%d/%m/%Y')  FROM tblcalender_event INNER JOIN tblmasterdate ON tblmasterdate.Id=tblcalender_event.DateId INNER JOIN tbleventmaster ON tbleventmaster.Id=tblcalender_event.EventId WHERE tblcalender_event.EventId=" + EventId;
    //            OdbcDataReader EventReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
    //            if (EventReader.HasRows)
    //            {
    //                while (EventReader.Read())
    //                {

    //                    EventName = EventReader.GetValue(0).ToString();
    //                    Description = EventReader.GetValue(1).ToString();
    //                    DateSr = DateSr + Seperator + EventReader.GetValue(2).ToString();
    //                    Seperator = " , ";
    //                }
    //            }
    //            string DateNewString = MyAttendance.GetFormatedDateString_EventView(DateSr);
    //            EventStr = EventStr + "<div class=\"EventSlide\" >     <table width=\"100%\">  <tr>  <td style=\"width:20%;padding-left:10px;\">  <img src=\"Pics/calendar_empty.png\" alt=\"\" width=\"40px\" />  </td>  <td align=\"center\" valign=\"middle\" style=\"color:#00586f;font-weight:bold\" >    <h5> " + EventName + " </h5>   </td>   <td style=\"width:35%;font-size:11px;font-weight:bold\">   " + DateNewString + "   </td> </tr>  <tr>  <td colspan=\"3\"  align=\"left\" style=\"border-top:solid 4px #0094b9;padding-top:10px;\">   " + Description + "   </td>    </tr>  </table>     </div>  ";
    //            EventCount++;
    //        }
    //    }
    //    else if(EventCount==0)
    //    {
    //        EventStr = "  <div class=\"EventSlide\" >  <table width=\"100%\">  <tr>  <td style=\"width:20%;padding-left:10px;\">  <img src=\"Pics/calendar_empty.png\" alt=\"\" width=\"40px\" /> </td>  <td align=\"center\" valign=\"middle\" style=\"color:#00586f;font-weight:bold\" >   </td> <td style=\"width:30%;font-size:11px;font-weight:bold\">    </td>  </tr> <tr>  <td colspan=\"3\" align=\"center\" style=\"border-top:solid 4px #0094b9;padding-top:10px;\">  <h2> No Upcoming Events Found </h2>   </td>    </tr>  </table>   </div>";
    //        EventCount++;
    //    }

    //    return EventStr;

    //}

    //private string GetBirthdaySlide(out int BirthdayCount)
    //{
    //    string StudentPageName = "StudentSessionMaker.aspx?StudentId=", StaffPageName = "StaffSessionMaker.aspx?StaffId=";
    //    string BirthdayStr = "";
    //    BirthdayCount = 0;
    //    DateTime CurrentDate = DateTime.Now.Date;
    //    int Month = CurrentDate.Month;
    //    int Day = CurrentDate.Day;
    //    int Year = CurrentDate.Year;

    //    //Student Birthday

    //    string sql = "SELECT tblstudent.Id,tblstudent.StudentName,date_format( tblstudent.DOB , '%d/%m/%Y'),tblclass.ClassName FROM tblstudent INNER JOIN tblclass ON tblclass.Id=tblstudent.LastClassId WHERE tblstudent.`Status`=1 AND Month(tblstudent.DOB)=" + Month + " AND Day(tblstudent.DOB)=" + Day + " ORDER BY Day(tblstudent.DOB),tblstudent.StudentName";
    //    MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
    //    if (MyReader.HasRows)
    //    {
    //        while (MyReader.Read())
    //        {
    //            string StId = MyReader.GetValue(0).ToString();
    //            string Name = MyReader.GetValue(1).ToString();
    //            DateTime DOB = General.GetDateTimeFromText(MyReader.GetValue(2).ToString());
    //            string ClassName = MyReader.GetValue(3).ToString();
    //            int Age = Year - DOB.Year;
    //            string ImgUrl = "Handler/ImageReturnHandler.ashx?id=" + int.Parse(StId) + "&type=StudentImage";
    //              //  MyUser.GetImageUrl("StudentImage", int.Parse(StId));
    //            BirthdayStr = BirthdayStr + " <div class=\"BirthdaySlide\">  <table width=\"100%\" cellspacing=\"0\">  <tr> <td style=\"padding-left:10px;width:20%\"> <img src=\"pics/bday.png\" alt=\"\" width=\"40px\" />  </td> <td align=\"center\" valign=\"middle\" style=\"color:#00586f;font-weight:bold\" >   <h3> Just Turned " + Age + "</h3>     </td>        <td style=\"padding-right:10px;\" >   <img src=\"images/birthdayCelebration.jpg\" alt=\"\" width=\"60px\" />    </td>    </tr><tr > <td valign=\"middle\" align=\"center\" style=\"border-top:solid 4px #ff359a;padding-top:10px;\">     <img src=\"" + ImgUrl + "\" alt=\"\" width=\"110px\" style=\"border: ridge 1px #c7c7c7;\" />     </td>    <td colspan=\"2\" style=\"border-top:solid 4px #ff359a;padding-top:10px;\">    <table width=\"100%\">    <tr><td style=\"width:50%;\" align=\"right\">  Name:  </td>  <td align=\"left\" style=\"font-weight:bold\"> " + Name + "    </td>   </tr> <tr>  <td style=\"width:50%;\" align=\"right\">  Class Name:   </td>  <td align=\"left\" style=\"font-weight:bold\">   " + ClassName + "   </td>   </tr>  <tr>   <td >     </td>   <td align=\"left\">     <a  style=\"color:Transparent;\" href=\"" + StudentPageName + StId + "\"> <img src=\"Pics/information.png\" alt=\"\" width=\"25px\" /></a>   </td>  </tr>   </table>   </td>  </tr>  </table>    </div> ";
    //            BirthdayCount++;
    //        }
    //    }


    //    //Staff Birthday

    //    sql = "SELECT DISTINCT t.`Id`,t.`SurName`, date_format( tblstaffdetails.Dob , '%d/%m/%Y'),r.RoleName  FROM tbluser t  inner join tblstaffdetails on tblstaffdetails.UserId = t.Id  inner join tblrole r on t.`RoleId`=r.`Id` where t.`Status`=1 AND r.`Type`='Staff' AND Month(tblstaffdetails.Dob)=" + Month + " AND Day(tblstaffdetails.Dob)=" + Day + " ORDER BY Day(tblstaffdetails.Dob),t.`SurName`";
    //    MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
    //    if (MyReader.HasRows)
    //    {
    //        while (MyReader.Read())
    //        {
    //            string StId = MyReader.GetValue(0).ToString();
    //            string Name = MyReader.GetValue(1).ToString();
    //            DateTime DOB = General.GetDateTimeFromText(MyReader.GetValue(2).ToString());
    //            string Role= MyReader.GetValue(3).ToString();
    //            int Age = Year - DOB.Year;
    //            string ImgUrl = "Handler/ImageReturnHandler.ashx?id=" + int.Parse(StId) + "&type=StaffImage";
    //                //MyUser.GetImageUrl("StaffImage", int.Parse(StId));
    //            BirthdayStr = BirthdayStr + " <div class=\"BirthdaySlide\">  <table width=\"100%\" cellspacing=\"0\">  <tr> <td style=\"padding-left:10px;width:20%\"> <img src=\"images/cake.png\" alt=\"\" width=\"40px\" />  </td> <td align=\"center\" valign=\"middle\" style=\"color:#00586f;font-weight:bold\" >   <h2> Just Turned " + Age + "</h2>     </td>        <td style=\"padding-right:10px;\" >   <img src=\"images/birthdayCelebration.jpg\" alt=\"\" width=\"60px\" />    </td>    </tr><tr > <td valign=\"middle\" align=\"center\" style=\"border-top:solid 4px #ff359a;padding-top:10px;\">     <img src=\"" + ImgUrl + "\" alt=\"\" width=\"120px\" style=\"border:ridge 4px silver;\" />     </td>    <td colspan=\"2\" style=\"border-top:solid 4px #ff359a;padding-top:10px;\">    <table width=\"100%\">    <tr><td style=\"width:50%;\" align=\"right\">  Name:  </td>  <td align=\"left\" style=\"font-weight:bold\"> " + Name + "    </td>   </tr> <tr>  <td style=\"width:50%;\" align=\"right\">  Role:   </td>  <td align=\"left\" style=\"font-weight:bold\">  " + Role + "   </td>   </tr>  <tr>   <td >     </td>   <td align=\"left\">     <a style=\"color:Transparent;\" href=\"" + StaffPageName + StId + "\"> <img src=\"Pics/info.png\" alt=\"\" width=\"30px\" /></a>   </td>  </tr>   </table>   </td>  </tr>  </table>    </div> ";
    //            BirthdayCount++;
    //        }
    //    }



    //    return BirthdayStr;
    //}





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
            AddTextToDayCell(e, "NotBatch","NoBatch");
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
    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {

    }
    protected void Calendar1_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        Calendar1.SelectedDate = new DateTime().Date;
        LoadCalenderDetails();
    }

    # endregion

    //private void DreawGenterChart()
    //{
    //    PieChart GenterPiechart = (PieChart)chartcontrol_yearly.Charts.FindChart("Genter_Chart");
    //    GenterPiechart.Colors = new Color[] { Color.LightGreen, Color.Yellow, Color.AntiqueWhite, Color.OrangeRed, Color.Cyan, Color.RosyBrown };

    //    int _NofBoys, _NoGerls;

    //    _NofBoys = 200;
    //    _NoGerls = 50;
    //    GenterPiechart.Data.Clear();
    //    GenterPiechart.Data.Add(new ChartPoint("Boys", _NofBoys));
    //    GenterPiechart.Data.Add(new ChartPoint("Girls", _NoGerls));

    //    if ((_NofBoys + _NoGerls) == 0)
    //    {
    //        GenterPiechart.Data.Add(new ChartPoint("NO DATA", 100));

    //        GenterPiechart.DataLabels.Visible = false;
    //    }
    //    chartcontrol_yearly.Background.Color = Color.White;
    //    chartcontrol_yearly.RedrawChart();
    //}

    private void checkConfig()
    {
        //if (OtherDetailsOn())
        //{
        //    Panel_Details.Visible = true;
        //    LoasOtherDetails();
        //}
        //else
        //{
        //    Panel_Details.Visible = false;
        //}
    }

    private void LoasOtherDetails()
    {   // Christian students
        string sql = "SELECT COUNT(tblstudent.Id) FROM tblstudent WHERE tblstudent.Religion = 'Christian' AND tblstudent.Status=1";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
             //Txt_Christ.Text = MyReader.GetValue(0).ToString();
         }
        //C_boys
         sql = "SELECT COUNT(tblstudent.Id) FROM tblstudent WHERE tblstudent.Religion = 'Christian' AND tblstudent.Sex = 'Male' AND tblstudent.Status=1";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
             //Txt_christ_boys.Text = MyReader.GetValue(0).ToString();
         }
        //C_girls
         sql = "SELECT COUNT(tblstudent.Id) FROM tblstudent WHERE tblstudent.Religion = 'Christian' AND tblstudent.Sex = 'Female' AND tblstudent.Status=1";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
            // Txt_christ_girls.Text = MyReader.GetValue(0).ToString();
         }
        //No of Hindu students
         sql = "SELECT COUNT(tblstudent.Id) FROM tblstudent WHERE tblstudent.Religion = 'Hindu' AND tblstudent.Status=1";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
             //Txt_hindu.Text = MyReader.GetValue(0).ToString();
         }
        //H_Boys
         sql = "SELECT COUNT(tblstudent.Id) FROM tblstudent WHERE tblstudent.Religion = 'Hindu'  AND tblstudent.Sex = 'Male' AND tblstudent.Status=1";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
            // Txt_H_Boys.Text = MyReader.GetValue(0).ToString();
         }
        //H_Girls
         sql = "SELECT COUNT(tblstudent.Id) FROM tblstudent WHERE tblstudent.Religion = 'Hindu' AND tblstudent.Sex = 'Female' AND tblstudent.Status=1 AND tblstudent.Status=1";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
           //  Txt_H_Girls.Text = MyReader.GetValue(0).ToString();
         }
        //No of Muslims
         sql = "SELECT COUNT(tblstudent.Id) FROM tblstudent WHERE tblstudent.Religion = 'Muslim' AND tblstudent.Status=1";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
            // Txt_Muslims.Text = MyReader.GetValue(0).ToString();
         }
        //M_Boys
         sql = "SELECT COUNT(tblstudent.Id) FROM tblstudent WHERE tblstudent.Religion = 'Muslim' AND tblstudent.Sex = 'Male' AND tblstudent.Status=1";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
            // Txt_M_Boys.Text = MyReader.GetValue(0).ToString();
         }
        //M_Girls
         sql = "SELECT COUNT(tblstudent.Id) FROM tblstudent WHERE tblstudent.Religion = 'Muslim' AND tblstudent.Sex = 'Female' AND tblstudent.Status=1";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
             //Txt_M_Girls.Text = MyReader.GetValue(0).ToString();
         }
         // No of Others (ALl)
         sql = "SELECT COUNT(tblstudent.Id) FROM tblstudent WHERE tblstudent.Religion <> 'Christian' AND tblstudent.Religion <> 'Hindu' AND tblstudent.Religion <> 'Muslim' AND tblstudent.Status=1";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
             //Txt_Other.Text = MyReader.GetValue(0).ToString();
         }
         //o_boys
         sql = "SELECT COUNT(tblstudent.Id) FROM tblstudent WHERE tblstudent.Religion <> 'Christian' AND tblstudent.Religion <> 'Hindu' AND tblstudent.Religion <> 'Muslim' AND tblstudent.Sex = 'Male' AND tblstudent.Status=1";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
             //Txt_O_Boys.Text = MyReader.GetValue(0).ToString();
         }
        //o_girls
         sql = "SELECT COUNT(tblstudent.Id) FROM tblstudent WHERE tblstudent.Religion <> 'Christian' AND tblstudent.Religion <> 'Hindu' AND tblstudent.Religion <> 'Muslim' AND tblstudent.Sex = 'Female' AND tblstudent.Status=1";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
             //Txt_O_Girls.Text = MyReader.GetValue(0).ToString();
         }
        //tottal RC
         sql = "select count(tblstudent.Id) from tblstudent inner join tblreligioncastmap on tblreligioncastmap.CasteId = tblstudent.`Cast` inner join tblcast on tblcast.Id= tblreligioncastmap.CasteId  inner join tblreligion on tblreligion.Id = tblreligioncastmap.ReligionId  where tblreligion.Religion='Christian' and tblcast.castname='Rc' and tblstudent.`Cast` <>'-1' and tblstudent.`Status` <>-1 ";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
             //Txt_RCTotal.Text = MyReader.GetValue(0).ToString();
         }
        //RC Boys
         sql = "select count(tblstudent.Id) from tblstudent inner join tblreligioncastmap on tblreligioncastmap.CasteId = tblstudent.`Cast` inner join tblcast on tblcast.Id= tblreligioncastmap.CasteId  inner join tblreligion on tblreligion.Id = tblreligioncastmap.ReligionId  where tblreligion.Religion='Christian' and tblcast.castname='Rc' and tblstudent.`Cast` <>'-1' and  tblstudent.Sex='Male' ";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
            // Txt_RCBoys.Text = MyReader.GetValue(0).ToString();
         }
        //RC Girls
         sql = "select count(tblstudent.Id) from tblstudent inner join tblreligioncastmap on tblreligioncastmap.CasteId = tblstudent.`Cast` inner join tblcast on tblcast.Id= tblreligioncastmap.CasteId  inner join tblreligion on tblreligion.Id = tblreligioncastmap.ReligionId  where tblreligion.Religion='Christian' and tblcast.castname='Rc' and tblstudent.`Cast` <>'-1' and   tblstudent.Sex='Female' ";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
            // Txt_RCGirls.Text = MyReader.GetValue(0).ToString();
         }
        //totalSc
         sql = "select count(tblstudent.Id) from tblstudent inner join tblreligioncastmap on tblreligioncastmap.CasteId = tblstudent.`Cast` inner join tblcast on tblcast.Id= tblreligioncastmap.CasteId  inner join tblreligion on tblreligion.Id = tblreligioncastmap.ReligionId where tblreligion.Religion='Hindu' and tblstudent.`Cast` <>'-1' and tblstudent.`Status` <>-1 and (tblcast.castname = 'SC' || tblstudent.ScheduledcasteType ='Scheduled Caste')";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
             //Txt_totalSC.Text = MyReader.GetValue(0).ToString();
         }
        //scBoys
         sql = "select count(tblstudent.Id) from tblstudent inner join tblreligioncastmap on tblreligioncastmap.CasteId= tblstudent.`Cast` inner join tblcast on tblcast.Id= tblreligioncastmap.CasteId  inner join tblreligion on tblreligion.Id = tblreligioncastmap.ReligionId where tblreligion.Religion='Hindu' and tblstudent.`Cast` <>'-1' and tblstudent.`Status` <>-1 and (tblcast.castname = 'SC' || tblstudent.ScheduledcasteType ='Scheduled Caste') and tblstudent.Sex='Male'";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
           //  Txt_SCBoys.Text = MyReader.GetValue(0).ToString();
         }
        //SCGirls
         sql = "select count(tblstudent.Id) from tblstudent inner join tblreligioncastmap on tblreligioncastmap.CasteId = tblstudent.`Cast`  inner join tblcast on tblcast.Id= tblreligioncastmap.CasteId inner join tblreligion on tblreligion.Id = tblreligioncastmap.ReligionId where tblreligion.Religion='Hindu' and tblstudent.`Cast` <>'-1' and tblstudent.`Status` <>-1 and (tblcast.castname = 'SC' || tblstudent.ScheduledcasteType ='Scheduled Caste') and tblstudent.Sex='Female'";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
            // Txt_SCGirls.Text = MyReader.GetValue(0).ToString();
         }
        //StTotal
         sql = "select count(tblstudent.Id) from tblstudent inner join tblreligioncastmap on tblreligioncastmap.CasteId = tblstudent.`Cast`  inner join tblcast on tblcast.Id= tblreligioncastmap.CasteId inner join tblreligion on tblreligion.Id = tblreligioncastmap.ReligionId where tblreligion.Religion='Hindu' and tblstudent.`Cast` <>'-1' and tblstudent.`Status` <>-1 and (tblcast.castname = 'ST' || tblstudent.ScheduledcasteType ='Scheduled Tribe')";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
             //Txt_STTotal.Text = MyReader.GetValue(0).ToString();
         }
        //stBoys
         sql = "select count(tblstudent.Id) from tblstudent inner join tblreligioncastmap on tblreligioncastmap.CasteId = tblstudent.`Cast`inner join tblcast on tblcast.Id= tblreligioncastmap.CasteId inner join tblreligion on tblreligion.Id = tblreligioncastmap.ReligionId where tblreligion.Religion='Hindu' and tblstudent.`Cast` <>'-1' and tblstudent.`Status` <>-1 and (tblcast.castname = 'ST' || tblstudent.ScheduledcasteType ='Scheduled Tribe') and tblstudent.Sex='Male'";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
            // Txt_STBoys.Text = MyReader.GetValue(0).ToString();
         }
        //stGirls
         sql = "select count(tblstudent.Id) from tblstudent inner join tblreligioncastmap on tblreligioncastmap.CasteId = tblstudent.`Cast`  inner join tblcast on tblcast.Id= tblreligioncastmap.CasteId inner join tblreligion on tblreligion.Id = tblreligioncastmap.ReligionId where tblreligion.Religion='Hindu' and tblstudent.`Cast` <>'-1' and tblstudent.`Status` <>-1 and (tblcast.castname = 'ST' || tblstudent.ScheduledcasteType ='Scheduled Tribe') and tblstudent.Sex='Female'";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
            // Txt_STGirls.Text = MyReader.GetValue(0).ToString();
         }
        //Minority Tottal
         sql = "select count(tblstudent.Id) from tblstudent  inner join tblreligion on tblreligion.Religion = tblstudent.Religion where tblreligion.Religion <> 'Hindu'  and tblstudent.`Status` <>-1 ";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
          //   Txt_MinTotal.Text = MyReader.GetValue(0).ToString();
         }
        //MinBoys
         sql = "select count(tblstudent.Id) from tblstudent  inner join tblreligion on tblreligion.Religion = tblstudent.Religion where tblreligion.Religion <> 'Hindu'  and tblstudent.`Status` <>-1 and tblstudent.Sex = 'Male'";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
           //  Txt_MinBoys.Text = MyReader.GetValue(0).ToString();
         }
        //minGirls
         sql = "select count(tblstudent.Id) from tblstudent  inner join tblreligion on tblreligion.Religion = tblstudent.Religion where tblreligion.Religion <> 'Hindu'  and tblstudent.`Status` <>-1 and tblstudent.Sex = 'Female'";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
         if (MyReader.HasRows)
         {
           //  Txt_MinGirls.Text = MyReader.GetValue(0).ToString();
         }
    }

    private bool OtherDetailsOn()
    {
        bool DetailsOn = false;
        int value;
        string sql = "SELECT Value FROM tblconfiguration WHERE Name='OtherDetails'";
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            value = int.Parse(MyReader.GetValue(0).ToString());
            if (value==1)
            {
                DetailsOn = true;
            }
        }
        return DetailsOn;
    }

    private void LoadGeneralDetails()
    {
        string Sql = "SELECT count(Id) FROM tblstudent WHERE Status=1";
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
            //Txt_NoOfStudents.Text = MyReader.GetValue(0).ToString();
        }
        Sql = "SELECT COUNT(tbluser.Id) FROM tbluser INNER JOIN tblrole on tbluser.RoleId=tblrole.Id AND tblrole.Type='staff' AND tbluser.Status=1";
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
           // Txt_NoOfStaff.Text = MyReader.GetValue(0).ToString();
        }
        Sql = "SELECT count(Id) FROM tblstudent WHERE Status=1 AND Sex='Male'";
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
           // Txt_Malestud.Text = MyReader.GetValue(0).ToString();
        }
        Sql = "SELECT count(Id) FROM tblstudent WHERE Status=1 AND Sex='Female'";
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
            //Txt_FemaleStud.Text = MyReader.GetValue(0).ToString();
        }
        Sql = "SELECT COUNT(DISTINCT Standard)FROM tblclass WHERE Status=1";
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
          //  Txt_Standard.Text = MyReader.GetValue(0).ToString();
        }
        Sql = "SELECT COUNT(ClassName) FROM tblclass WHERE Status=1";
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
           // Txt_Class.Text = MyReader.GetValue(0).ToString();
        }

    }

    //private void LoadLogo()
    //{
    //    Img_Logo.ImageUrl = "Handler/ImageReturnHandler.ashx?id=1&type=Logo";
    //    //String ImageUrl = "";
    //    //String Sql = "SELECT LogoUrl FROM tblschooldetails WHERE Id=1";
    //    //MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
    //    //if (MyReader.HasRows)
    //    //{
    //    //    ImageUrl = MyReader.GetValue(0).ToString();
    //    //}
    //    //else
    //    //{
    //    //    ImageUrl = "img.png";
    //    //}

    //    //Img_Logo.ImageUrl =  WinerUtlity.GetRelativeFilePath(objSchool)+"ThumbnailImages/" + ImageUrl;
      
    //}

    //private void LoadSchoolDetails()
    //{
    //    string Sql = "SELECT SchoolName,Address,Disc FROM tblschooldetails WHERE Id=1";
    //    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
    //    if (MyReader.HasRows)
    //    {
    //        Lbl_SchoolName.Text = MyReader.GetValue(0).ToString();
    //        Lbl_Subhead.Text = MyReader.GetValue(1).ToString();
    //        //if (MyReader.GetValue(2).ToString().Trim() == "")
    //        //{
    //        //    Pnl_aboutUs.Visible = false;
    //        //}
    //        //this.Description.InnerHtml = MyReader.GetValue(2).ToString();
           
    //    }
    //}

    //protected void Txt_FemaleStud_TextChanged(object sender, EventArgs e)
    //{
    //}

    //protected void Lnk_AddEvent_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("HolidayManager.aspx");
    //}



    //protected void Lmg_AddEvent_Click(object sender, ImageClickEventArgs e)
    //{
    //    Response.Redirect("HolidayManager.aspx");
    //}

    //protected void Drp_Dashboard_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //   string page= MyUser.SelectDashBoard(Drp_Dashboard.SelectedValue, MyUser.SELECTEDMODE);
    //   if (page != "")
    //   {
    //       Response.Redirect(page);
    //   }
    //}

  

    //protected void Img_SMS_Click(object sender, ImageClickEventArgs e)
    //{
    //    Response.Redirect("BirthdayList.aspx");
    //}
}
