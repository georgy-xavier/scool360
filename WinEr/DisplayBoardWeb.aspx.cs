using System;
using System.Collections;
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

namespace WinEr
{
    public partial class DisplayBoardWeb : System.Web.UI.Page
    {
        private OdbcDataReader MyReader;
        private Attendance MyAttendance;
        private MysqlClass _mysqlObj=null;
        private KnowinUser MyUser;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            string _msg = "";
            
            if (Request.QueryString["UserName"] != null && Request.QueryString["Pasw"] != null)
            {
                if (WinerUtlity.NeedCentrelDB())
                {

                    if (Request.QueryString[WinerConstants.SchoolId] != null)
                    {
                        int intSchoolId;
                        if (int.TryParse(Request.QueryString[WinerConstants.SchoolId], out intSchoolId))
                        {
                            objSchool = WinerUtlity.GetSchoolObject(intSchoolId);
                            if (objSchool != null)
                                _mysqlObj = new MysqlClass(objSchool.ConnectionString);
                            Session[WinerConstants.SessionSchool] = objSchool;
                        }
                    }
                    if (Session[WinerConstants.SessionSchool] != null)
                    {
                        objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                    }

                }
                else
                {
                    _mysqlObj = new MysqlClass(WinerUtlity.SingleSchoolConnectionString);
                }

                if (_mysqlObj != null)
                {
                    MyAttendance = new Attendance(_mysqlObj);
                    int Heightpix = 0, SlideTime = 0;
                    string UserName = Request.QueryString["UserName"].ToString();
                    string Passward = Request.QueryString["Pasw"].ToString();
                    if (Request.QueryString["Height"] != null)
                    {
                        int.TryParse(Request.QueryString["Height"].ToString(), out Heightpix);
                    }
                    if (Request.QueryString["SlideTime"] != null)
                    {
                        int.TryParse(Request.QueryString["SlideTime"].ToString(), out SlideTime);
                    }
                    MyUser = CheckUserCredentials(UserName, Passward, Heightpix, SlideTime, out _msg);
                }
                else
                {
                    _msg = "Invalid Login Parameters";
                }
            }
            else
            {
                _msg = "Invalid Login Parameters";
            }

            if (_msg != "")
            {
                LoadErrorMessage();
            }

        }

        private void LoadErrorMessage()
        {

        }



        private KnowinUser CheckUserCredentials(string UserName, string Passward, int Heightpix, int SlideTime, out string _msg)
        {
            KnowinUser UserObj;
            
            string FilePath = 

            _msg = "";
            try
            {
                
                if (FilePath == "")
                    FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));

                string _meassageError;
                Session["RsltQry"] = null;
                if (Session["UserObj"] == null)
                {


                    UserObj = new KnowinUser(WinerUtlity.GetConnectionString(objSchool), FilePath, WinerUtlity.GetRelativeFilePath(objSchool));
      
                    Session["UserObj"] = UserObj;
                }
                else
                {

                    UserObj = (KnowinUser)Session["UserObj"];
                    if (UserObj.UserName != UserName.Trim())
                    {

                        UserObj = new KnowinUser(WinerUtlity.GetConnectionString(objSchool), FilePath, WinerUtlity.GetRelativeFilePath(objSchool));
                        
                        Session["UserObj"] = UserObj;
                    }
                }

                KnowinEncryption m_MyEncrypt = new KnowinEncryption();
                MyUser = UserObj;
                if (UserObj.LoginUser(UserName,m_MyEncrypt.Decrypt(Passward), 0, out _meassageError))
                {

                    string SlideJS = "<script type=\"text/javascript\"> $(function() { $(\".slidetabs\").tabs(\".images > div\", { effect: 'fade', fadeOutSpeed: \"slow\", rotate: true }).slideshow({autoplay: true, interval:10000}); });</script>";
                    if (Heightpix == 0)
                    {
                        Heightpix = 450;
                    }
                    if (SlideTime > 0)
                    {
                        SlideJS = "<script type=\"text/javascript\"> $(function() { $(\".slidetabs\").tabs(\".images > div\", { effect: 'fade', fadeOutSpeed: \"slow\", rotate: true }).slideshow({autoplay: true, interval:" + SlideTime + "}); });</script>";
                    }
                    this.JSDIV.InnerHtml = SlideJS;
                    LoadContentSlide(Heightpix);
                   
                }
                else
                {
                    _msg = _meassageError;
                }
            }
            catch(Exception Ex)
            {
                UserObj = new KnowinUser(WinerUtlity.GetConnectionString(objSchool), WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath("")), WinerUtlity.GetRelativeFilePath(objSchool));
                _msg = "Error while connecting to website";
            }
            return UserObj;
        }



        private void LoadContentSlide(int Heightpix)
        {
            int SlideCount = 0;
            int BirthdayCount = 0, EventCount = 0,AttendanceCount=0;
            string ImageStr = "<div class=\"images\">           </div>";
            string HomeStr = "<!-- first slide -->	 <div style=\"background-color:White;width:97%;height:235px;\">        <img src=\"#\" alt=\"School\" style=\"float:left;margin:0 30px 20px 0\" />     <br />     <h2>Arun School</h2>     <h4> Click Play To View Upcoming Events </h4>          <br />	    <br />	    <center>      <img src=\"Pics/play buttons/blue_play.png\" alt=\"School\" width=\"45\" height=\"45\" onclick='$(\".slidetabs\").data(\"slideshow\").play();'/>        </center>      </div>	  ";
            string BirthdayStr = "   <!-- second slide -->    ";
            string EventStr = "   <!-- third slide --> ";
            string AttendanceStr = " <!-- fourth slide -->";

            BirthdayStr = GetBirthdaySlide(out BirthdayCount);
            EventStr = UpcomingEventPresent(out EventCount);
            AttendanceStr = GetAttendanceDetails(out AttendanceCount, Heightpix);
            ImageStr = "<div class=\"images\" style=\"height:" + Heightpix + "px;\">   " + BirthdayStr + "    " + EventStr + "  " + AttendanceStr + "   </div>";
            this.ImagesDiv.InnerHtml = ImageStr;
            SlideCount = SlideCount + BirthdayCount + EventCount + AttendanceCount;

            string SlideTabStr = " <div class=\"slidetabs\">   ";

            for (int i = 1; i <= SlideCount; i++)
            {
                SlideTabStr = SlideTabStr + " <a href=\"#\">" + i + "</a>";
            }

            SlideTabStr = SlideTabStr + " </div> ";

            this.SlideTabsDiv.InnerHtml = SlideTabStr;

        }

        private string GetAttendanceDetails(out int AttendanceCount, int Heightpix)
        {
            AttendanceCount=0;
            string str = "";
            string _classstr = "";
            string sql = "SELECT tblclass.Id,tblclass.ClassName,tblstandard.Id from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id,tblclass.ClassName";
            OdbcDataReader my_Reader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (my_Reader.HasRows)
            {
                while (my_Reader.Read())
                {
                    string _ClassId = my_Reader.GetValue(0).ToString();
                    string _ClassName = my_Reader.GetValue(1).ToString();
                    string _StandardId = my_Reader.GetValue(2).ToString();
                    _classstr = _classstr + Get_EachClassString(_ClassId, _ClassName, _StandardId, Heightpix);
                    AttendanceCount++;
                }
            }
            return _classstr;
        }

        private string Get_EachClassString(string _ClassId, string _ClassName, string _StandardId, int Heightpix)
        {
            string _classstr = "";


            int _StudentsCount = GetTotalNumberOfStudentsInClass(int.Parse(_ClassId), MyUser.CurrentBatchId);
            int Present_StudentsCount = GetAttendanceCount(_ClassId, _StandardId);
            string Absent_studentStr = "";
            if (_StudentsCount == 0)
            {
                Absent_studentStr = "<h2> No students in the class </h2>";
            }
            else if (_StudentsCount <= Present_StudentsCount)
            {

                Absent_studentStr = "<table width=\"100%\">  <tr> <td colspan=\"2\" align=\"left\" style=\"font-size:17px;padding-left:10px\"> All Students are present</h2></td></tr></table> ";
            }
            else
            {
                if (MyAttendance.AttendanceTables_Exits(_StandardId, MyUser.CurrentBatchId))
                {

                    string Sql = "SELECT DISTINCT t2.StudentId,tblstudentclassmap.RollNo FROM tblattdcls_std" + _StandardId + "yr" + MyUser.CurrentBatchId + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + MyUser.CurrentBatchId + " t2 ON t2.ClassAttendanceId=t1.Id INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=t2.StudentId WHERE t2.PresentStatus=0 AND t1.ClassId=" + _ClassId + " AND t1.Date='" + DateTime.Now.Date.ToString("s") + "' ORDER BY tblstudentclassmap.RollNo";
                    OdbcDataReader m_MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(Sql);
                    if (m_MyReader.HasRows)
                    {
                        int _count = 0;
                        string substr="";
                        while (m_MyReader.Read())
                        {
                            string RollNo = m_MyReader.GetValue(1).ToString();
                            string _studName = MyUser.getStudName(int.Parse(m_MyReader.GetValue(0).ToString()));
                            substr = substr + "<tr> <td align=\"left\" style=\"font-size:14px;padding-left:10px\"> " + RollNo + " </td> <td align=\"left\" style=\"font-size:14px;padding-left:10px\">  " + _studName + "  </td> </tr>";
                            _count++;
                        }
                        string leftMarguee = "", rightMarquee = "";

                        if (_count > 15)
                        {
                            leftMarguee = "<marquee height=\"" + (Heightpix - 200).ToString() + "px\" behavior=\"scroll\" direction=\"up\" scrollamount=\"3\">";
                            rightMarquee = " </marquee> ";
                        }
                        Absent_studentStr = " <h2> <u>  ABSENT STUDENTS </u> </h2>   " + leftMarguee + "   <table width=\"100%\">  <tr> <td align=\"left\" style=\"width:20%;font-size:15px;font-weight:bold;padding-left:10px\">  <u> ROLL NO</u> </td>  <td  align=\"left\" style=\"width:80%;font-size:15px;font-weight:bold;padding-left:10px\"> <u>STUDENT NAME</u> </td> </tr>" + substr + "  </table>  " + rightMarquee;
                    }
                    else
                    {
                        Absent_studentStr = "<h2>Attendance not marked</h2>";
                    }


                }
                else
                {
                    Absent_studentStr = "<h2>Attendance not marked</h2>";
                }
            }


            string LateComers_studentStr = "&nbsp;";
            if (_StudentsCount == 0)
            {
                //LateComers_studentStr = "<h2> No students in the class </h2>";
            }
            else
            {
                if (MyAttendance.AttendanceTables_Exits(_StandardId, MyUser.CurrentBatchId))
                {
                    int count=0;
                    string Sql = "SELECT DISTINCT t2.StudentId,tblstudentclassmap.RollNo FROM tblattdcls_std" + _StandardId + "yr" + MyUser.CurrentBatchId + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + MyUser.CurrentBatchId + " t2 ON t2.ClassAttendanceId=t1.Id INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=t2.StudentId WHERE t2.PresentStatus<>0 AND t2.IsLate=1 AND t1.ClassId=" + _ClassId + " AND t1.Date='" + DateTime.Now.Date.ToString("s") + "' ORDER BY tblstudentclassmap.RollNo";
                    OdbcDataReader m_MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(Sql);
                    if (m_MyReader.HasRows)
                    {
                        
                        string substr="";
                        while (m_MyReader.Read())
                        {
                            count++;
                            string RollNo = m_MyReader.GetValue(1).ToString();
                            string _studName = MyUser.getStudName(int.Parse(m_MyReader.GetValue(0).ToString()));
                            substr = substr + "<tr> <td align=\"left\" style=\"font-size:14px;padding-left:10px\"> " + RollNo + " </td> <td align=\"left\" style=\"font-size:14px;padding-left:10px\">  " + _studName + "  </td> </tr>";
                        }

                        string leftMarguee = "", rightMarquee = "";
                        if (count > 15)
                        {
                            leftMarguee = "<marquee height=\"" + (Heightpix - 200).ToString() + "px\" behavior=\"scroll\" direction=\"up\" scrollamount=\"3\">";
                            rightMarquee = " </marquee> ";
                        }

                        LateComers_studentStr = "<h2> <u>  LATE COMERS </u> </h2>" + leftMarguee + "<table width=\"100%\"> <tr> <td align=\"left\" style=\"width:20%;font-size:15px;font-weight:bold;padding-left:10px\">  <u> ROLL NO</u> </td>  <td  align=\"left\" style=\"width:80%;font-size:15px;font-weight:bold;padding-left:10px\"> <u>STUDENT NAME</u> </td> </tr>" + substr + "  </table>    " + rightMarquee;
                    }
                    else
                    {
                        //LateComers_studentStr = "<h2>Attendance not marked</h2>";
                    }


                }
                else
                {
                    //LateComers_studentStr = "<h2>Attendance not marked</h2>";
                }
            }



            if (Absent_studentStr != "")
            {
                _classstr = " <div class=\"AttendanceSlide\">  <table width=\"100%\" cellspacing=\"0\"> <tr>  <td style=\"width:50%;padding-left:10px;\">     <table>   <tr> <td>    <img src=\"Pics/evolution-tasks.png\" alt=\"\" width=\"60px\" height=\"60px\" />  </td> <td valign=\"middle\" style=\"padding-left:10px\">  <h1> ATTENDANCE DETAILS </h1> </td>  </tr> </table>    </td> <td align=\"center\" valign=\"middle\" style=\"width:50%;color:#00586f;font-weight:bold;font-size:22px\" > <table>  <tr> <td>  Class : " + _ClassName + "  </td>  <td style=\"padding-left:40px\"> STRENGTH (" + Present_StudentsCount + "/" + _StudentsCount + ")</td>  </tr></table>    </td>  </tr>  <tr >  <td valign=\"top\" align=\"center\" style=\"border-top:solid 8px #f1111a;padding-top:10px;\">   " + Absent_studentStr + "            </td>   <td valign=\"top\" align=\"center\" style=\"border-top:solid 8px #f1111a;padding-top:10px;\">    "+LateComers_studentStr+"         </td> </tr> </table> </div> ";
            }
            return _classstr;
        }

        public int GetTotalNumberOfStudentsInClass(int _ClassId, int _BatchId)
        {
            int _TotalSeats = 0;
            string sql = "select count(tblstudentclassmap.StudentId) from tblstudentclassmap INNER JOIN tblview_student ON tblstudentclassmap.StudentId=tblview_student.Id where tblview_student.`Status`=1 AND tblstudentclassmap.ClassId=" + _ClassId + " and tblstudentclassmap.BatchId=" + _BatchId;
            OdbcDataReader m_MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out _TotalSeats);
            }
            return _TotalSeats;
        }

        private int GetAttendanceCount(string _ClassId, string _StandardId)
        {
            int _count = 0;
            if (MyAttendance.AttendanceTables_Exits(_StandardId, MyUser.CurrentBatchId))
            {
                string Sql = "SELECT Count(DISTINCT t2.StudentId) FROM tblattdcls_std" + _StandardId + "yr" + MyUser.CurrentBatchId + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + MyUser.CurrentBatchId + " t2 ON t2.ClassAttendanceId=t1.Id INNER JOIN tblview_student ON tblview_student.Id=t2.StudentId WHERE t2.PresentStatus<>0 AND tblview_student.Status=1 AND t1.ClassId=" + _ClassId + " AND t1.Date='" + DateTime.Now.Date.ToString("s") + "'";
                OdbcDataReader m_MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(Sql);
                if (m_MyReader.HasRows)
                {
                    _count = int.Parse(m_MyReader.GetValue(0).ToString());

                }
            }
            return _count;
        }


        private string UpcomingEventPresent(out int EventCount)
        {
            EventCount = 0;
            string EventStr = "";
            string sql = "SELECT DISTINCT( tblcalender_event.EventId) FROM tblcalender_event INNER JOIN tblmasterdate ON tblmasterdate.Id=tblcalender_event.DateId WHERE tblmasterdate.`date`>='" + DateTime.Now.Date.ToString("s") + "' ORDER BY tblmasterdate.`date`";
            MyReader = _mysqlObj.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {

                    string EventId = MyReader.GetValue(0).ToString();
                    string EventName = "", Description = "", DateSr = "", Seperator = "";
                    sql = "SELECT DISTINCT tbleventmaster.EventName,tbleventmaster.Description,date_format( tblmasterdate.date , '%d/%m/%Y')  FROM tblcalender_event INNER JOIN tblmasterdate ON tblmasterdate.Id=tblcalender_event.DateId INNER JOIN tbleventmaster ON tbleventmaster.Id=tblcalender_event.EventId WHERE tblcalender_event.EventId=" + EventId;
                    OdbcDataReader EventReader = _mysqlObj.ExecuteQuery(sql);
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
                    EventStr = EventStr + "<div class=\"EventSlide\" >     <table width=\"100%\">  <tr>  <td style=\"width:20%;padding-left:10px;\">  <img src=\"Pics/calendar_empty.png\" alt=\"\" width=\"70px\" />  </td>  <td align=\"center\" valign=\"middle\" style=\"color:#00586f;font-weight:bold;font-size:22px\" >     " + EventName + "    </td>   <td style=\"width:35%;font-size:17px;font-weight:bold\">   " + DateNewString + "   </td> </tr>  <tr>  <td colspan=\"3\"  align=\"left\" style=\"border-top:solid 8px #0094b9;padding-top:10px;\">   " + Description + "   </td>    </tr>  </table>     </div>  ";
                    EventCount++;
                }
            }
            //else if (EventCount == 0)  No Upcoming Events Found  is commented
            //{
            //    EventStr = "  <div class=\"EventSlide\" >  <table width=\"100%\">  <tr>  <td style=\"width:20%;padding-left:10px;\">  <img src=\"Pics/calendar_empty.png\" alt=\"\" width=\"40px\" /> </td>  <td align=\"center\" valign=\"middle\" style=\"color:#00586f;font-weight:bold\" >   </td> <td style=\"width:30%;font-size:11px;font-weight:bold\">    </td>  </tr> <tr>  <td colspan=\"3\" align=\"center\" style=\"border-top:solid 4px #0094b9;padding-top:10px;\">  <h2> No Upcoming Events Found </h2>   </td>    </tr>  </table>   </div>";
            //    EventCount++;
            //}

            return EventStr;

        }

        private string GetBirthdaySlide(out int BirthdayCount)
        {
            //string StudentPageName = "StudentSessionMaker.aspx?StudentId=", StaffPageName = "StaffSessionMaker.aspx?StaffId=";
            string BirthdayStr = "";
            BirthdayCount = 0;
            DateTime CurrentDate = DateTime.Now.Date;
            int Month = CurrentDate.Month;
            int Day = CurrentDate.Day;
            int Year = CurrentDate.Year;

            //Student Birthday

            string sql = "SELECT tblstudent.Id,tblstudent.StudentName,date_format( tblstudent.DOB , '%d/%m/%Y'),tblclass.ClassName FROM tblstudent INNER JOIN tblclass ON tblclass.Id=tblstudent.LastClassId WHERE tblstudent.`Status`=1 AND Month(tblstudent.DOB)=" + Month + " AND Day(tblstudent.DOB)=" + Day + " ORDER BY Day(tblstudent.DOB),tblstudent.StudentName";
            MyReader = _mysqlObj.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    string StId = MyReader.GetValue(0).ToString();
                    string Name = MyReader.GetValue(1).ToString();
                    DateTime DOB = General.GetDateTimeFromText(MyReader.GetValue(2).ToString());
                    string ClassName = MyReader.GetValue(3).ToString();
                    int Age = Year - DOB.Year;
                   // string ImgUrl = MyUser.GetImageUrl("StudentImage", int.Parse(StId));
                    string ImgUrl = "Handler/ImageReturnHandler.ashx?id=" + int.Parse(StId) + "&type=StudentImage";
                  
                    BirthdayStr = BirthdayStr + " <div class=\"BirthdaySlide\">  <table width=\"100%\" cellspacing=\"0\">  <tr> <td style=\"padding-left:10px;width:20%\"> <img src=\"images/cake.png\" alt=\"\" width=\"70px\" />  </td> <td align=\"center\" valign=\"middle\" style=\"color:#00586f;font-weight:bold;font-size:22px\" >    Just Turned " + Age + "     </td>        <td style=\"padding-right:10px;\" >   <img src=\"images/birthdayCelebration.jpg\" alt=\"\" width=\"90px\" />    </td>    </tr><tr > <td valign=\"middle\" align=\"center\" style=\"border-top:solid 8px #ff359a;padding-top:10px;\">     <img src=\"" + ImgUrl + "\" alt=\"\" width=\"150px\" style=\"border:ridge 4px silver;\" />     </td>    <td colspan=\"2\" style=\"border-top:solid 8px #ff359a;padding-top:10px;font-size:16px\">    <table width=\"100%\">    <tr><td style=\"width:50%;\" align=\"right\">  Name:  </td>  <td align=\"left\" style=\"font-weight:bold\"> " + Name + "    </td>   </tr> <tr>  <td style=\"width:50%;\" align=\"right\">  Class Name:   </td>  <td align=\"left\" style=\"font-weight:bold\">   " + ClassName + "   </td>   </tr>  <tr>   <td >     </td>   <td align=\"left\">   </td>  </tr>   </table>   </td>  </tr>  </table>    </div> ";
                    BirthdayCount++;
                }
            }


            //Staff Birthday

            sql = "SELECT DISTINCT t.`Id`,t.`SurName`, date_format( tblstaffdetails.Dob , '%d/%m/%Y'),r.RoleName  FROM tbluser t  inner join tblstaffdetails on tblstaffdetails.UserId = t.Id  inner join tblrole r on t.`RoleId`=r.`Id` where t.`Status`=1 AND r.`Type`='Staff' AND Month(tblstaffdetails.Dob)=" + Month + " AND Day(tblstaffdetails.Dob)=" + Day + " ORDER BY Day(tblstaffdetails.Dob),t.`SurName`";
            MyReader = _mysqlObj.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    string StId = MyReader.GetValue(0).ToString();
                    string Name = MyReader.GetValue(1).ToString();
                    DateTime DOB = General.GetDateTimeFromText(MyReader.GetValue(2).ToString());
                    string Role = MyReader.GetValue(3).ToString();
                    int Age = Year - DOB.Year;
                    //string ImgUrl = MyUser.GetImageUrl("StaffImage", int.Parse(StId));
                    string ImgUrl = "Handler/ImageReturnHandler.ashx?id=" +int.Parse(StId) + "&type=StaffImage";
                    
                    BirthdayStr = BirthdayStr + " <div class=\"BirthdaySlide\">  <table width=\"100%\" cellspacing=\"0\">  <tr> <td style=\"padding-left:10px;width:20%\"> <img src=\"images/cake.png\" alt=\"\" width=\"70px\" />  </td> <td align=\"center\" valign=\"middle\" style=\"color:#00586f;font-weight:bold;font-size:22px\" >    Just Turned " + Age + "    </td>        <td style=\"padding-right:10px;\" >   <img src=\"images/birthdayCelebration.jpg\" alt=\"\" width=\"90px\" />    </td>    </tr><tr > <td valign=\"middle\" align=\"center\" style=\"border-top:solid 8px #ff359a;padding-top:10px;\">     <img src=\"" + ImgUrl + "\" alt=\"\" width=\"150px\" style=\"border:ridge 4px silver;\" />     </td>    <td colspan=\"2\" style=\"border-top:solid 8px #ff359a;padding-top:10px;font-size:16px\">    <table width=\"100%\">    <tr><td style=\"width:50%;\" align=\"right\">  Name:  </td>  <td align=\"left\" style=\"font-weight:bold\"> " + Name + "    </td>   </tr> <tr>  <td style=\"width:50%;\" align=\"right\">  Role:   </td>  <td align=\"left\" style=\"font-weight:bold\">  " + Role + "   </td>   </tr>  <tr>   <td >     </td>   <td align=\"left\">     </td>  </tr>   </table>   </td>  </tr>  </table>    </div> ";
                    BirthdayCount++;
                }
            }



            return BirthdayStr;
        }

      
    }
}
