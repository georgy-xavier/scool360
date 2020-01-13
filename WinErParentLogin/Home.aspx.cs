using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;
using System.Drawing;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using System.Text;

namespace WinErParentLogin
{
    public partial class Home : System.Web.UI.Page
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
            else if (MyAttendence==null)
            {
                Response.Redirect("sectionerr.htm");
            }
            else if (!IsPostBack)
            {
                vdo.Visible = false;
                LoadSchoolDetails();
                LoadAnnouncemnets();
                
           //     LoadCalenderDetails();
               LoadContentSlide();
                Label MyHeader = (Label)this.Master.FindControl("Lbl_PageHeader");
                MyHeader.Text = "Home";
                loadincidents();
                Load_Home_Works();
              //  hdnactivatedauth.Value = Session["ACtivationStatus"].ToString();

            }

            //#region gmail
            //OpenIdRelyingParty rp = new OpenIdRelyingParty();
            //var r = rp.GetResponse();
            //if (r != null)
            //{
            //    switch (r.Status)
            //    {
            //        case AuthenticationStatus.Authenticated:
            //            //  NotLoggedIn.Visible = false;
            //            Session["GoogleIdentifier"] = r.ClaimedIdentifier.ToString();
            //            var fetch = r.GetExtension<FetchResponse>();
            //            string email = string.Empty;

            //            if (fetch != null)
            //            {
            //                email = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);
            //                //  Student studObj = new Student();
            //                //   studObj.Email = email;
            //                //  studObj.Name = objBl.GetStudentName(email);
            //                //  studObj.LoginTime = System.DateTime.Now;
            //                //   Session["student"] = studObj;
            //                UpdateGmailIDinTable(email, "GmailAuthId"); //redirect to main page of your website


            //            }
            //            else
            //            {
            //                Response.Redirect("<script>alert('Login Failed')</script>");
            //            }

            //            break;
            //        case AuthenticationStatus.Canceled:
            //            Response.Redirect("<script>alert('Login Cancelled')</script>");
            //            break;
            //        case AuthenticationStatus.Failed:
            //            Response.Redirect("<script>alert('Login Failed')</script>");
            //            break;
            //    }
            //}
            //#endregion

        }

        private void LoadAnnouncemnets()
        {
            StringBuilder Inner_Html = new StringBuilder();
            string Content = "";
            try
            {
                MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
                Incident MyIncident = new Incident(_mysqlObj);
                DataSet MyDataSet = new DataSet();
                Content = Get_Video_Length();
                if (Content != "")
                {
                    string sql = "(select tbl_announcemnts.Id,tbl_announcemnts.Body,tbl_announcemnts.Title,tbl_announcemnts.ExternalLink from tbl_announcemnts INNER  JOIN tbl_annoucement_studentmap on tbl_announcemnts.Id=tbl_annoucement_studentmap.AnnId where tbl_annoucement_studentmap.StudentID=" + MyParentInfo.StudentId.ToString() + " and tbl_announcemnts.Type=1 and tbl_announcemnts.RefType=1 and tbl_announcemnts.ExternalType=1 and date(tbl_announcemnts.ExpiryDatetime)>= '" + DateTime.Now.Date.ToString("s") + "') union (SELECT tbl_announcemnts.Id,tbl_announcemnts.Body,tbl_announcemnts.Title,tbl_announcemnts.ExternalLink from tbl_announcemnts where tbl_announcemnts.Type=1 and tbl_announcemnts.ExternalType=1 and date(tbl_announcemnts.ExpiryDatetime)>= '" + DateTime.Now.Date.ToString("s") + "') order by Id DESC ";
                    MyDataSet = MyIncident.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
                    {
                        Inner_Html.Append(" <ul class=\"bxslider\" >");
                        foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                        {
                            Content=Content.Replace("src", "src=" + dr["ExternalLink"].ToString() + "");
                            Content=Content.Replace("title", "title=" + dr["Title"].ToString() + "");
                            Inner_Html.Append("<li>");
                            Inner_Html.Append("<p></p> <center><h3 style=\"color:#FFFFFF;background-color:#151515 ;opacity: 0.5;\">" + dr["Title"].ToString() + " </h3></center>");
                            Inner_Html.Append(Content);
                            Inner_Html.Append("<p></p> <center><h3 style=\"color:#FFFFFF;background-color:#151515 ;opacity: 0.5;\">" + dr["Body"].ToString() + " </h3></center>");
                            Inner_Html.Append("</li>");
                        
                            this.Videos.InnerHtml = Inner_Html.ToString();
                        }
                        Inner_Html.Append(" </ul>");
                    }
                    else
                    {
                        Inner_Html.Append("<div><hr COLOR=\"Orange\"><h3 style=\"color:Orange;text-align:center;\">No Annoucements Exist </h3> <hr COLOR=\"Orange\"></div> ");
                        this.Videos.InnerHtml = Inner_Html.ToString();
                    }
                }
                else
                {
                    Inner_Html.Append("<div><hr COLOR=\"Orange\"><h3 style=\"color:Orange;text-align:center;\">Annoucements Configuration Content Missed </h3> <hr COLOR=\"Orange\"></div> ");
                    this.Videos.InnerHtml = Inner_Html.ToString();
                }
            }
            catch
            {
                Inner_Html.Append("<div><hr COLOR=\"Orange\"><h3 style=\"color:Orange;text-align:center;\"> Error Ocurred </h3> <hr COLOR=\"Orange\"></div> ");
                this.Videos.InnerHtml = Inner_Html.ToString();
            }
        }
  
        private string Get_Video_Length()
        {
            string Content = "";
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            Incident MyIncident = new Incident(_mysqlObj);
            DataSet MyDataSet = new DataSet();
            string sql = "select Content from tbl_annoucement_type where Type='YouTube'";
            MyDataSet = MyIncident.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {
                    Content = dr["Content"].ToString();
                }
            }
            return Content;
        }
 
        private void loadincidents()
        {

            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            Incident MyIncident = new Incident(_mysqlObj);
            string sql = "";
            DataSet MyDataSet = new DataSet();
            StringBuilder incident = new StringBuilder();

            // academic area

            sql = "select tblincedent.Id , tblincedent.Title , tblincedenttype.`Type` , tblincedenttype.Description,date_format(IncedentDate,'%d/%m/%Y' ) as insdate, (select tblview_user.SurName from  tblview_user where tblview_user.Id= tblincedent.CreatedUserId)  as SurName, tblincedent.`Point`, DATE_FORMAT(tblincedent.IncedentDate,'%d/%m/%Y') as IncidentDate from tblincedent INNER join tblincedenttype on tblincedenttype.Id = tblincedent.TypeId inner join tblview_student on tblview_student.Id = tblincedent.AssoUser where tblincedent.`Status` = 'Approved' AND tblincedent.UserType='student'  and  tblincedenttype.IncidentType='NORMAL'  and tblview_student.Id = " + MyParentInfo.StudentId + " and tblincedent.BatchId=" + MyParentInfo.CurrentBatchId;

            sql = sql + " and tblincedenttype.Id =1  order by tblincedent.CreatedDate desc";
 
            MyDataSet = MyIncident.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {
                    incident.Append("<b>" + dr["Title"].ToString() + "</b><br />");
                    incident.Append(dr["Description"].ToString() + "<br />");
                    incident.Append(dr["insdate"].ToString() + "<br /> <hr><br /> ");

                }


            }

            else
            {
                incident.Append("No achivements reported");
            }
            academicarea.InnerHtml=incident.ToString();
            incident.Length = 0;
            incident.Capacity = 0;


            // medical area

            sql = "select tblincedent.Id , tblincedent.Title , tblincedenttype.`Type` , tblincedenttype.Description,date_format(IncedentDate,'%d/%m/%Y' ) as insdate, (select tblview_user.SurName from  tblview_user where tblview_user.Id= tblincedent.CreatedUserId)  as SurName, tblincedent.`Point`, DATE_FORMAT(tblincedent.IncedentDate,'%d/%m/%Y') as IncidentDate from tblincedent INNER join tblincedenttype on tblincedenttype.Id = tblincedent.TypeId inner join tblview_student on tblview_student.Id = tblincedent.AssoUser where tblincedent.`Status` = 'Approved' AND tblincedent.UserType='student' and tblview_student.Id = " + MyParentInfo.StudentId + " and    tblincedenttype.IncidentType='NORMAL' and  tblincedent.BatchId=" + MyParentInfo.CurrentBatchId;

            sql = sql + " and tblincedenttype.Id =2 order by tblincedent.CreatedDate desc" ;

            MyDataSet = MyIncident.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {
                    incident.Append("<b>" + dr["Title"].ToString() + "</b><br />");
                    incident.Append(dr["Description"].ToString() + "<br />");
                    incident.Append(dr["insdate"].ToString() + "<br /> <hr><br /> ");

                }


            }

            else
            {
                incident.Append("No medical details reported");
            }


            medicalarea.InnerHtml = incident.ToString();
            incident.Length = 0;
            incident.Capacity = 0;

            //Disciplinary actions

            sql = "select tblincedent.Id , tblincedent.Title , tblincedenttype.`Type` , tblincedenttype.Description,date_format(IncedentDate,'%d/%m/%Y' ) as insdate, (select tblview_user.SurName from  tblview_user where tblview_user.Id= tblincedent.CreatedUserId)  as SurName, tblincedent.`Point`, DATE_FORMAT(tblincedent.IncedentDate,'%d/%m/%Y') as IncidentDate from tblincedent INNER join tblincedenttype on tblincedenttype.Id = tblincedent.TypeId inner join tblview_student on tblview_student.Id = tblincedent.AssoUser where tblincedent.`Status` = 'Approved' AND tblincedent.UserType='student' and tblview_student.Id = " + MyParentInfo.StudentId + " and    tblincedenttype.IncidentType='NORMAL' and tblincedent.BatchId=" + MyParentInfo.CurrentBatchId;

            sql = sql + " and tblincedenttype.Id =3 order by tblincedent.CreatedDate desc";

           

            MyDataSet = MyIncident.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {
                    incident.Append("<b>" + dr["Title"].ToString() + "</b><br />");
                    incident.Append(dr["Description"].ToString() + "<br />");
                    incident.Append(dr["insdate"].ToString() + "<br /> <hr><br /> ");

                }


            }

            else
            {
                incident.Append("No disciplinary actions reported");
            }


            displinaryarea.InnerHtml = incident.ToString();
            incident.Length = 0;
            incident.Capacity = 0;


            // other actions

            sql = "select tblincedent.Id , tblincedent.Title , tblincedenttype.`Type` , tblincedenttype.Description,date_format(IncedentDate,'%d/%m/%Y' ) as insdate,  (select tblview_user.SurName from  tblview_user where tblview_user.Id= tblincedent.CreatedUserId)  as SurName, tblincedent.`Point`, DATE_FORMAT(tblincedent.IncedentDate,'%d/%m/%Y') as IncidentDate from tblincedent INNER join tblincedenttype on tblincedenttype.Id = tblincedent.TypeId inner join tblview_student on tblview_student.Id = tblincedent.AssoUser where tblincedent.`Status` = 'Approved' AND tblincedent.UserType='student' and tblview_student.Id = " + MyParentInfo.StudentId + " and  tblincedenttype.IncidentType='NORMAL' and tblincedent.BatchId=" + MyParentInfo.CurrentBatchId;

            sql = sql + " and tblincedenttype.Id <>1 and tblincedenttype.Id <>2 and tblincedenttype.Id <>3  order by tblincedent.CreatedDate desc";



            MyDataSet = MyIncident.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {
                    incident.Append("<b>" + dr["Title"].ToString() + "</b><br />");
                    incident.Append(dr["Description"].ToString() + "<br />");
                    incident.Append(dr["insdate"].ToString() + "<br /> <hr><br /> ");

                }


            }

            else
            {
                incident.Append("Any other activements reported");
            }


            otherarea.InnerHtml = incident.ToString();
            incident.Length = 0;
            incident.Capacity = 0;


        }

        private void UpdateGmailIDinTable(string email, string gmail_fb)
        {
            int IsActiveSecure = 0;
            if (gmail_fb == "GmailAuthId")
                IsActiveSecure = 1;
            else IsActiveSecure = 2;


            string sql = "update tblparent_parentdetails set " + gmail_fb + "='" + email + "' ,IsActiveSecure=" + IsActiveSecure + "  where tblparent_parentdetails.Id= " + MyParentInfo.ParentId;
            MyReader = MyAttendence.m_MysqlDb.ExecuteQuery(sql);
        }

        private void LoadContentSlide()
        {
            System.Configuration.AppSettingsReader MyAppReader = new System.Configuration.AppSettingsReader();

            //string Img_Logo = MyAppReader.GetValue("ParentLoginLogo", typeof(string)).ToString();
            string Img_Logo = MyParentInfo.SCHOOLLOGO;
            string SchoolName = MyParentInfo.SCHOOLNAME;

            int SlideCount = 1;
            int BirthdayCount = 0, EventCount = 0, ImageCount=0;
            string ImageStr = "<div class=\"images\">           </div>";
            string HomeStr = "<!-- first slide -->	 <div style=\"background-color:White;width:97%;height:235px;\">        <img src=\"" + Img_Logo + "\" alt=\"School\" style=\"float:left;margin:0 30px 20px 0;width:100px;\" />     <br />     <h2 style=\"color:blueviolet;\">" + SchoolName + "</h2>     <h5> Click Play To View Upcoming Events </h5>          <br />	    <br />	    <center>      <img src=\"Pics/play buttons/blue_play.png\" alt=\"School\" width=\"45\" height=\"45\" onclick='$(\".slidetabs\").data(\"slideshow\").play();'/>        </center>      </div>	  ";
            string BirthdayStr = "   <!-- second slide -->    ";
            string EventStr = "   <!-- third slide --> ";
            string AnnouncementStr = "   <!-- Fourth slide --> ";

            EventStr = UpcomingEventPresent(out EventCount);
            AnnouncementStr = AnnouncementImageLoader(out ImageCount);
            ImageStr = "<div class=\"images\">   " + HomeStr + BirthdayStr + "    " + EventStr + AnnouncementStr + "    </div>";
            this.ImagesDiv.InnerHtml = ImageStr;
            SlideCount = SlideCount + BirthdayCount + EventCount + ImageCount;

            string SlideTabStr = " <div class=\"slidetabs\">   ";

            for (int i = 1; i <= SlideCount; i++)
            {
                SlideTabStr = SlideTabStr + " <a href=\"#\">" + i + "</a>";
            }

            SlideTabStr = SlideTabStr + " </div> ";

            this.SlideTabsDiv.InnerHtml = SlideTabStr;

        }
 
        private void Load_Home_Works()
        {
            DateTime Cur_Dt=new DateTime();
            DataSet ds_home = new DataSet();
            Cur_Dt = DateTime.Now.AddDays(-7);
            int Student_Id = 0;
            int Class_Id = 0;
            string Inner_HTML = "";
            try
            {
                Student_Id = MyParentInfo.StudentId;
                Class_Id = MyParentInfo.CLASSID;
                string sql = "(SELECT Id,Title,Body,CreatedDatetime,ExpiryDatetime FROM tbl_announcemnts WHERE ExpiryDatetime >='" + Cur_Dt.ToString("yyyy-MM-dd") + "' and RedId=" + Class_Id + " and  Type=2 )union (SELECT t1.Id,t1.Title,t1.Body,t1.CreatedDatetime,t1.ExpiryDatetime FROM tbl_announcemnts t1 INNER join tbl_annoucement_studentmap t2 on t2.AnnId=t1.Id WHERE t1.ExpiryDatetime>='" + Cur_Dt.ToString("yyyy-MM-dd") + "' and t2.StudentId=" + Student_Id + " and t1.Type=2)order by ExpiryDatetime ASC";
                ds_home = MyAttendence.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (ds_home != null && ds_home.Tables[0] != null && ds_home.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds_home.Tables[0].Rows)
                    {
                        DateTime Tomorrow_Dt = DateTime.Now.AddDays(1);
                        DateTime Expire_Dt = new DateTime();
                        DateTime.TryParse(dr["ExpiryDatetime"].ToString(), out Expire_Dt);
                        if (Expire_Dt.Date == Tomorrow_Dt.Date)
                        {
                            Inner_HTML = Inner_HTML + "<table width=\"100%\"><tr><td align=\"left\" style=\"width:50%;\"><h5 style=\"color:#138DC6;\">" + dr["Title"].ToString() + "</h5></td><td  align=\"center\" style=\"width:50%;\"><h5 style=\"color:Red;\">Tomorrow</h5></td></tr></table> <hr COLOR=\"purple\"><div class=\"Div_Content\" ><p>" + dr["Body"].ToString() + "</p> </div><br /> ";
                        }
                        else if (Expire_Dt.Date == Cur_Dt.Date)
                        {
                            Inner_HTML = Inner_HTML + "<table width=\"100%\"><tr><td align=\"left\" style=\"width:50%;\"><h5 style=\"color:#138DC6;\">" + dr["Title"].ToString() + "</h5></td><td  align=\"center\" style=\"width:50%;\"><h5 style=\"color:Orange;\">Today</h5></td></tr></table> <hr COLOR=\"purple\"><div class=\"Div_Content\" ><p>" + dr["Body"].ToString() + "</p> </div><br /> ";
                        }
                        else
                        {
                            Inner_HTML = Inner_HTML + "<table width=\"100%\"><tr><td align=\"left\" style=\"width:50%;\"><h5 style=\"color:#138DC6;\">" + dr["Title"].ToString() + "</h5></td><td  align=\"center\" style=\"width:50%;\"><h5 style=\"color:Green;\">" + DateTime.Parse(dr["ExpiryDatetime"].ToString()).ToString("dd/MM/yyyy") + "</h5></td></tr></table> <hr COLOR=\"purple\"><div class=\"Div_Content\" ><p>" + dr["Body"].ToString() + "</p> </div><br /> ";

                        }
                    }
                    this.InnerHtml.InnerHtml = Inner_HTML;
                    Pnl_Home_More.Visible = true;

                }
                else
                {
                    Inner_HTML = "<div><hr COLOR=\"Orange\"><h3 style=\"color:Orange;text-align:center;\">No Home Works Assigned For Your Kid </h3> <hr COLOR=\"Orange\"></div> ";
                    this.InnerHtml.InnerHtml = Inner_HTML;
                }
            }
            catch
            {
                Inner_HTML = "<div><hr COLOR=\"purple\"><br/><h3 style=\"color:Orange;text-align:center;\"> Error Occured </h3><br/> <hr COLOR=\"purple\"></div> ";
                this.InnerHtml.InnerHtml = Inner_HTML;

            }

        }

        protected void Lnk_Home_Click(object sender, EventArgs e)
        {
            Response.Redirect("HomeWork.aspx");
        }

        private string UpcomingEventPresent(out int EventCount)
        {
            EventCount = 0;
            string EventStr = "";
            string sql = "SELECT DISTINCT( tblcalender_event.EventId) FROM tblcalender_event INNER JOIN tblmasterdate ON tblmasterdate.Id=tblcalender_event.DateId INNER JOIN tbleventmaster ON tbleventmaster.Id=tblcalender_event.EventId WHERE tblmasterdate.`date`>='" + DateTime.Now.Date.ToString("s") + "' ORDER BY tblmasterdate.`date`";
            MyReader = MyAttendence.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {

                    string EventId = MyReader.GetValue(0).ToString();
                    string EventName = "", Description = "", DateSr = "", Seperator = "";
                    sql = "SELECT DISTINCT tbleventmaster.EventName,tbleventmaster.Description,date_format( tblmasterdate.date , '%d/%m/%Y')  FROM tblcalender_event INNER JOIN tblmasterdate ON tblmasterdate.Id=tblcalender_event.DateId INNER JOIN tbleventmaster ON tbleventmaster.Id=tblcalender_event.EventId WHERE tblcalender_event.EventId=" + EventId;
                    OdbcDataReader EventReader = MyAttendence.m_MysqlDb.ExecuteQuery(sql);
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
                    string DateNewString = MyAttendence.GetFormatedDateString_EventView(DateSr);
                    EventStr = EventStr + "<div class=\"EventSlide\" >     <table width=\"100%\">  <tr>  <td style=\"width:20%;padding-left:10px;\">  <img src=\"Pics/calendar_empty.png\" alt=\"\" width=\"40px\" />  </td>  <td align=\"center\" valign=\"middle\" style=\"color:#00586f;font-weight:bold\" >    <h2> " + EventName + " </h2>   </td>   <td style=\"width:35%;font-size:11px;font-weight:bold\">   " + DateNewString + "   </td> </tr>  <tr>  <td colspan=\"3\"  align=\"left\" style=\"border-top:solid 4px #0094b9;padding-top:10px;\">   " + Description + "   </td>    </tr>  </table>     </div>  ";
                    EventCount++;
                }
            }
            else if (EventCount == 0)
            {
                EventStr = "   <div class=\"EventSlide\" >  <table width=\"100%\">  <tr>  <td style=\"width:20%;padding-left:10px;\">  <img src=\"Pics/calendar_empty.png\" alt=\"\" width=\"40px\" /> </td>  <td align=\"center\" valign=\"middle\" style=\"color:#00586f;font-weight:bold\" >   </td> <td style=\"width:30%;font-size:11px;font-weight:bold\">    </td>  </tr> <tr>  <td colspan=\"3\" align=\"center\" style=\"border-top:solid 4px #0094b9;padding-top:10px;\">  <h2> No Upcoming Events Found </h2>   </td>    </tr>  </table>   </div>";
                EventCount++;
            }

            return EventStr;

        }

        private string AnnouncementImageLoader(out int EventCount)
        {
            EventCount = 0;
            SchoolClass objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
            string FilePath = "FileRepository/" + objSchool.FilePath + "/UpImage/";
            string EventStr = "";
            string sql = "(select tbl_announcemnts.Id,tbl_announcemnts.Body,tbl_announcemnts.Title,tbl_announcemnts.ExternalLink from tbl_announcemnts INNER  JOIN tbl_annoucement_studentmap on tbl_announcemnts.Id=tbl_annoucement_studentmap.AnnId where tbl_annoucement_studentmap.StudentID=" + MyParentInfo.StudentId.ToString() + " and tbl_announcemnts.Type=1 and tbl_announcemnts.RefType=1 and tbl_announcemnts.ExternalType=2 and date(tbl_announcemnts.ExpiryDatetime)>= '" + DateTime.Now.Date.ToString("s") + "') union (SELECT tbl_announcemnts.Id,tbl_announcemnts.Body,tbl_announcemnts.Title,tbl_announcemnts.ExternalLink from tbl_announcemnts where tbl_announcemnts.Type=1 and tbl_announcemnts.ExternalType=2 and date(tbl_announcemnts.ExpiryDatetime)>= '" + DateTime.Now.Date.ToString("s") + "') order by Id DESC ";
            MyReader = MyAttendence.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    EventStr = EventStr + "<div class=\"EventSlide\" > <table width=\"100%\"><tr>  <td style=\"padding-left:10px;\"> <h2> " + MyReader.GetValue(2).ToString() + " </h2>  </td> </tr>  <tr>  <td align=\"center\" style=\"border-top:solid 4px #0094b9;padding-top:10px;\">   <a class=\"Announcement-Image\" onClick=\"ViewAnnouncementImage(this)\" href=\"#\"> <img src=\"" + FilePath + MyReader.GetValue(3).ToString() + "\" alt=\"\" height=\"186px\" /> </a>  </td>    </tr>  <tr> <td> <marquee> " + MyReader.GetValue(1).ToString() + " </marquee> </td></tr> </table> </div>";
                    EventCount++;
                }
            }
            return EventStr;
        }

        protected void imggmail_Click(object sender, EventArgs e)
        {
            string discoveryUri = "https://www.google.com/accounts/o8/id".ToString();
            using (OpenIdRelyingParty openid = new OpenIdRelyingParty())
            {
                var URIbuilder = new UriBuilder(Request.Url) { Query = "" };
                var req = openid.CreateRequest(discoveryUri, URIbuilder.Uri, URIbuilder.Uri);
                IAuthenticationRequest request = openid.CreateRequest(discoveryUri);

                var fetch = new FetchRequest();
                fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
                request.AddExtension(fetch);

                // Send your visitor to their Provider for authentication.
                request.RedirectToProvider();
            }
        }
     
        # region Calender Manager Comented

        //private void LoadCalenderDetails()
        //{
        //    Calendar1.SelectedDate = new DateTime().Date;
        //    string type = "all";
        //    int Classid = 0;
        //    holidaydataset = MyAttendence.MyAssociatedHolidays(type, Classid, MyParentInfo.ParentId);
        //    EventDataset = MyAssociatedEventsDataSet(Classid);
        //}

        //private DataSet MyAssociatedEventsDataSet(int Classid)
        //{
        //    DataSet Events = null;
        //    string sql = "select distinct tblmasterdate.`date`, tblcalender_event.ClassId, tbleventmaster.EventName,tbleventmaster.Description from tblmasterdate inner join tblcalender_event on tblcalender_event.DateId= tblmasterdate.Id INNER JOIN tbleventmaster ON tbleventmaster.Id=tblcalender_event.EventId where tbleventmaster.IsPublish=1 and  ((tblcalender_event.ClassId=0) or ( tblcalender_event.ClassId=" + Classid + "))";
        //    Events =MyAttendence.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        //    return Events;
        //}

        //protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        //{
        //    bool done = false;


        //        DateTime nextholidayDate;
        //        if (holidaydataset != null && holidaydataset.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in holidaydataset.Tables[0].Rows)
        //            {
        //                nextholidayDate = (DateTime)dr[0];
        //                if (MyAttendence.IsDefaultHoliday(e.Day.Date.DayOfWeek))
        //                {
        //                    //e.Cell.BackColor = HexStringToColor("#ffcc00");//Holiday
        //                    e.Cell.BackColor = Color.Gold;
        //                    AddTextToDayCell(e, "Holiday", "Default Holiday");
        //                    done = true;
        //                    break;
        //                }
        //                else
        //                {
        //                    if (nextholidayDate == e.Day.Date)
        //                    {

        //                        //e.Cell.BackColor = HexStringToColor("#ffcc00");//Holiday
        //                        e.Cell.BackColor = Color.Gold;
        //                        AddTextToDayCell(e, "Holiday", dr[2].ToString());
        //                        done = true;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (MyAttendence.IsDefaultHoliday(e.Day.Date.DayOfWeek))
        //            {
        //                // e.Cell.BackColor = HexStringToColor("#ffcc00"); //Holiday
        //                e.Cell.BackColor = Color.Gold;
        //                AddTextToDayCell(e, "Holiday", "Default Holiday");
        //                done = true;
        //            }
        //        }

        //        if (EventDataset != null && EventDataset.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in EventDataset.Tables[0].Rows)
        //            {
        //                nextholidayDate = (DateTime)dr[0];

        //                if (nextholidayDate == e.Day.Date)
        //                {

        //                    e.Cell.BackColor = System.Drawing.Color.Turquoise;
        //                    AddTextToDayCell(e, "Event", dr[2].ToString() + "$" + dr[3].ToString());
        //                    done = true;
        //                    break;
        //                }

        //            }
        //        }

         
            
        //    if (!done)
        //    {
        //        AddTextToDayCell(e, "Other", "Other");
        //    }
        //}

        //void AddTextToDayCell(DayRenderEventArgs e, string Type, string Name)
        //{

        //    string TextColor = "#000000";
        //    if (e.Day.IsOtherMonth)
        //    {
        //        TextColor = "#999999";
        //    }
        //    if (Type == "Holiday")
        //    {
        //        string HolidayMessage = "Selected day is holiday due to " + Name;
        //        if (Name == "Default Holiday")
        //        {
        //            HolidayMessage = e.Day.Date.DayOfWeek + " is holiday";
        //        }
        //        e.Cell.Text = "<a href=\"javascript:alert('" + HolidayMessage + "')\" style=\"color:" + TextColor + "\">" + e.Day.DayNumberText;
        //    }
        //    else if (Type == "NotBatch")
        //    {
        //        e.Cell.Text = "<a href=\"javascript:alert('Selected day is not within batch')\" style=\"color:" + TextColor + "\">" + e.Day.DayNumberText;
        //    }
        //    else if (Type == "Event")
        //    {
        //        string HdValue = CalenderDataHide.InnerHtml;
        //        string[] str = Name.Split('$');

        //        if (HdValue == "")
        //        {
        //            HdValue = str[0] + "*-*" + str[1];
        //        }
        //        else
        //        {
        //            CalenderDataHide.InnerHtml = HdValue + "$%$" + str[0] + "*-*" + str[1];
        //        }
        //        e.Cell.Text = "<a href=\"javascript:LoadPopup('" + str[0] + "')\" style=\"color:" + TextColor + "\">" + e.Day.DayNumberText;
        //    }
        //    else
        //    {
        //        e.Cell.Text = "<a href=\"javascript:alert('" + General.GerFormatedDatVal(e.Day.Date) + "')\" style=\"color:" + TextColor + "\">" + e.Day.DayNumberText;
        //    }

        //}

        //private System.Drawing.Color HexStringToColor(string hex)
        //{
        //    hex = hex.Replace("#", "");

        //    if (hex.Length != 6)
        //        throw new Exception(hex +
        //            " is not a valid 6-place hexadecimal color code.");

        //    string r, g, b;

        //    r = hex.Substring(0, 2);
        //    g = hex.Substring(2, 2);
        //    b = hex.Substring(4, 2);

        //    return System.Drawing.Color.FromArgb(HexStringToBase10Int(r), HexStringToBase10Int(g),
        //                                        HexStringToBase10Int(b));
        //}

        //private int HexStringToBase10Int(string hex)
        //{
        //    int base10value = 0;

        //    try { base10value = System.Convert.ToInt32(hex, 16); }
        //    catch { base10value = 0; }

        //    return base10value;

        //}

  



        //protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        //{

        //}

        //protected void Calendar1_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        //{
        //    Calendar1.SelectedDate = new DateTime().Date;
        //    LoadCalenderDetails();
        //}

        # endregion

        private void LoadSchoolDetails()
        {
            string Sql = "SELECT SchoolName,Address,Disc FROM tblschooldetails WHERE Id=1";
            MyReader = MyAttendence.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
              
                //if (MyReader.GetValue(2).ToString().Trim() == "")
                //{
                //    Pnl_aboutUs.Visible = false;
                //}
                //this.Description.InnerHtml = MyReader.GetValue(2).ToString();

            }
        }

        protected void Btn_Send_Click(object sender, EventArgs e)
        {
            int toDept = 0, toUsrId = 0, frmUserId = 0, threadId;
            string subj = "", desc = "";
            //  frmUserId = MyParentInfo.ParentId;
            frmUserId = MyParentInfo.StudentId;
            toUsrId = 1;
            if (toUsrId > 0)
            {
                subj = txt_subject.Text;
                desc = txt_descrpn.Text;


                MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
                ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
                threadId = MyParent.GetThreadId(frmUserId, toUsrId, 1, 2, subj);
                MyParent.ComposeMessage(frmUserId, toUsrId, 1, 2, subj, desc, 0, 3, threadId);
                _mysqlObj.CloseConnection();
                LblFailureNotice.Text = "Message has been sent";
                txt_subject.Text = "";
                txt_descrpn.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hi", "showstatus();", true);
            }

        }

        protected void btnsendmsg_Click(object sender, EventArgs e)
        {
            LblFailureNotice.Text = "";
        }
    }
}
