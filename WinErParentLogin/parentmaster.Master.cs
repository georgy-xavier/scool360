using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;
using System.Text;
using System.Drawing;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using System.Data.Odbc;

namespace WinErParentLogin
{
    public partial class parentmaster : System.Web.UI.MasterPage
    {
        DataSet MyDataSet = null;
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
                FillStudentNameToDrpList();
                FillStudentDetails();
                FillMasterDetails();
                loadmenu();
                //isgmailloginactivated();

            }
            #region gmail
            OpenIdRelyingParty rp = new OpenIdRelyingParty();
            var r = rp.GetResponse();
            if (r != null)
            {
                switch (r.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        //  NotLoggedIn.Visible = false;
                        Session["GoogleIdentifier"] = r.ClaimedIdentifier.ToString();
                        var fetch = r.GetExtension<FetchResponse>();
                        string email = string.Empty;

                        if (fetch != null)
                        {
                            email = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);
                            //  Student studObj = new Student();
                            //   studObj.Email = email;
                            //  studObj.Name = objBl.GetStudentName(email);
                            //  studObj.LoginTime = System.DateTime.Now;
                            //   Session["student"] = studObj;
                            UpdateGmailIDinTable(email, "GmailAuthId"); //redirect to main page of your website
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AnyScriptNameYouLike", "showmsg(' Your Gmail authentication is activated ');", true);
                            //activategmail.Visible = false;
                        }
                        else
                        {
   
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AnyScriptNameYouLike", "showmsg(' Login Failed ');", true);
        
                        }

                        break;
                    case AuthenticationStatus.Canceled:
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AnyScriptNameYouLike", "showmsg('Login Cancelled');", true);
        
                         break;
                    case AuthenticationStatus.Failed:
                         ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AnyScriptNameYouLike", "showmsg('Login Failed');", true);
        
                          break;
                }
            }
            #endregion
        }

        //private void isgmailloginactivated()
        //{
        //    MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
        //    ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);

        //    if (MyParent.EnabledSecureLogin(MyParentInfo.ParentId))
        //    {
        //        activategmail.Visible = false;
        //    }
        //    else
        //        activategmail.Visible = true;

        //    _mysqlObj = null;
        //    MyParent = null;
        //}

        private void UpdateGmailIDinTable(string email, string gmail_fb)
        {
            int IsActiveSecure = 0;
            if (gmail_fb == "GmailAuthId")
                IsActiveSecure = 1;
            else IsActiveSecure = 2;


            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);

             MyParent.UpdategmailIdintable(gmail_fb,email ,IsActiveSecure, MyParentInfo.ParentId);

            _mysqlObj.CloseConnection();

            _mysqlObj = null;
            MyParent = null;

        }
        private void loadmenu()
        {
            StringBuilder sb= new StringBuilder();

          
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);

            DataSet menus=MyParent.getactivemodules();
            MyParent = null;
            _mysqlObj.CloseConnection();
            sb.Append("<ul class=\"nav\" id=\"side-menu\">  <li><a href=\"Home.aspx\"><i class=\"fa fa-home fa-fw\"></i> HOME </a> </li>");
           
            if (menus != null && menus.Tables != null && menus.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in menus.Tables[0].Rows)
                {
                    if (dr["ModuleName"].ToString() == "FEES") { sb.Append("<li><a href=\"FeeDetails.aspx\"><i class=\"fa fa-money fa-fw\"></i> FEE </a> </li>"); }
                    else if (dr["ModuleName"].ToString() == "EXAM")
                    {
                        if (!CheckExamConfiguration())
                        {
                            sb.Append("<li><a href=\"Examreports.aspx\" ><i class=\"fa fa-file fa-fw\"></i> EXAM REPORT</a> </li>");
                            sb.Append("<li><a href=\"ExamTimeTable.aspx\" ><i class=\"fa fa-calendar fa-fw\"></i> EXAM TIMETABLE</a> </li>");
                        }
                        else
                        {
                            sb.Append("<li><a href=\"StudentConsolidate.aspx\" ><i class=\"fa fa-file fa-fw\"></i> EXAM REPORT</a> </li>");                
                        }
                        
                    }
                    else if (dr["ModuleName"].ToString() == "ATTENDANCE") 
                    {
                        sb.Append("<li><a href=\"AttendanceReport.aspx\" ><i class=\"fa fa-group fa-fw\"></i> ATTENDANCE </a> </li>");
                    }
                    else if (dr["ModuleName"].ToString() == "TIME TABLE") 
                    {
                        sb.Append("<li><a href=\"StudTimeTable.aspx\" ><i class=\"fa fa-calendar fa-fw\"></i> TIME TABLE </a> </li>"); 
                    }
                    else if (dr["ModuleName"].ToString() == "INCIDENT") 
                    {
                        sb.Append("<li><a href=\"Viewincidents.aspx\" ><i class=\"fa fa-star fa-fw\"></i> INCIDENTS </a>  </li>");
                       
                    }
                    else if (dr["ModuleName"].ToString() == "ANNOUNCEMENTS")
                    {
                        sb.Append("<li><a href=\"HomeWork.aspx\" ><i class=\"fa fa-bullhorn fa-fw\"></i> ANNOUNCEMENTS </a>  </li>"); 
                    }
                   
                }
            }

            _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
             MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            if ( MyParent.isFeedbackActivated(MyParentInfo.StudentId, MyParentInfo.CLASSID))
            {
                

                sb.Append("<li><a href=\"TeachersFeedback.aspx\" ><i class=\"fa fa-comments fa-fw\"></i> FEEDBACK</a></li>");
                
            }
            


            int unreadmail = getunreadmailcount();
         
            sb.Append("<li><a href=\"MessageHome.aspx\" ><i class=\"fa fa-envelope fa-fw\"></i> MESSAGES <span id=\"smscount\"  style=\"font-weight:bold;\" runat=\"server\"> </span>  </a></li>");
          
            sb.Append("<li><a href=\"ServiceReq.aspx\" ><i class=\"fa fa-envelope fa-fw\"></i> SUBMIT REQUEST </a></li>");
            sb.Append("<li><a href=\"Servicecomplaints.aspx\" ><i class=\"fa fa-envelope fa-fw\"></i> SUBMIT COMPLAINT </a></li>");
            sb.Append("<li><a href=\"ServiceFeedBack.aspx\" ><i class=\"fa fa-envelope fa-fw\"></i> SUBMIT FEED BACK </a></li>");
            sb.Append(" <li><a href=\"ChangePassword.aspx\" ><i class=\"fa fa-cogs fa-fw\"></i> CHANGE PASSWORD </a> </li>");
            sb.Append(" </ul>");


            _mysqlObj = null;
            MyParent = null;
            menuarea.InnerHtml = sb.ToString();
        
        }

        private bool CheckExamConfiguration()
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
             
            bool valid = false;
            string sql = "SELECT tblconfiguration.value  from tblconfiguration where tblconfiguration.Module='Exam' AND tblconfiguration.Name='Exam_Type'";
            OdbcDataReader MyReader = _mysqlObj.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    if (MyReader.GetValue(0).ToString() == "1")
                        valid = true;
                }
            }
            return valid;
        }

        private int getunreadmailcount()
        {
            int count = 0;
            
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            
            MyDataSet = MyParent.GetUnreadmailcount(MyParentInfo.StudentId);
           
            _mysqlObj.CloseConnection();
           
            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
               
                int.TryParse(MyDataSet.Tables[0].Rows[0]["mailcount"].ToString() ,out count );
  
            _mysqlObj = null;
            MyParent = null;
            
            return count;
        }

        private void FillMasterDetails()
        {
            Lbl_LoginUser.Text = "Hi " + MyParentInfo.ParentName;

            SchoolDetails.InnerHtml = GetSchoolInfo(MyParentInfo.SCHOOLNAME);
            var page = (Page)HttpContext.Current.Handler;
            page.Title = MyParentInfo.SCHOOLNAME.ToUpperInvariant() + " Parent Login";
        }
        public string GetSchoolInfo(string _schoolName)
        {
            StringBuilder School = new StringBuilder("");
            //System.Configuration.AppSettingsReader MyAppReader = new System.Configuration.AppSettingsReader();
            School.Append("<table><tr><td id=\"SchoolLogoStyle\" ><img height=\"30\"  alt=\"\" src=\"" + MyParentInfo.SCHOOLLOGO + "\" /> </td><td style=\"font-size:18px;\">" + _schoolName + "</td> </tr></table>");
            //MyAppReader = null;
            return School.ToString();
        }

        private void FillStudentDetails()
        {
            string HasVirtualFolder = System.Configuration.ConfigurationSettings.AppSettings["HasVirtualFolder"];
            if (HasVirtualFolder == "1")
            {

                Image_StudentIMG.ImageUrl = MyParentInfo.StudentImage;
                TRImgArea.Visible = true;
            }
            else
            {
                TRImgArea.Visible = false;
            }
            lblStudName.Text = MyParentInfo.StudentName;
            
            Lbl_Class.Text = MyParentInfo.CLASSNAME;
            Lbl_RollNo.Text = MyParentInfo.RollNO;
            Lbl_Age.Text = MyParentInfo.AGE.ToString();
            Lbl_AdmissionNo.Text = MyParentInfo.ADMISSIONNO;
            //Lbl_TotalPoints.Text = MyParentInfo.POINT.ToString();
            //Lbl_TotalRating.Text = MyParentInfo.RATING.ToString();
            //if (MyParentInfo.POINT < 0)
            //{
            //    Lbl_TotalPoints.ForeColor = Color.Red;
            //    Img_TotalPoints.ImageUrl = "Pics/Points red.png";
            //}
            //else
            //{
            //    Lbl_TotalPoints.ForeColor = Color.FromName("#ddb104");
            //    Img_TotalPoints.ImageUrl = "Pics/Points.png";
            //}
            //if (MyParentInfo.RATING < 0)
            //{
            //    Lbl_TotalRating.ForeColor = Color.Red;
            //    Img_TotalRating.ImageUrl = "Pics/Rating red.png";

            //}
            //else
            //{
            //    Lbl_TotalRating.ForeColor = Color.FromName("#ddb104");
            //    Img_TotalRating.ImageUrl = "Pics/Rating.png";
            //}
        }

        private void FillStudentNameToDrpList()
        {
            Drp_StudentName.Items.Clear();
            ListItem li;
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            MyDataSet = MyParent.GetMyStudent(MyParentInfo.ParentId);
            _mysqlObj.CloseConnection();
            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Dr_Students in MyDataSet.Tables[0].Rows)
                {
                    li = new ListItem(Dr_Students[0].ToString(), Dr_Students[1].ToString());
                    Drp_StudentName.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No student found", "-1");
                Drp_StudentName.Items.Add(li);
            }
            Drp_StudentName.SelectedValue = MyParentInfo.StudentId.ToString();
            _mysqlObj = null;
            MyParent = null;
            Divselectkid.Visible = false;
            if (Drp_StudentName.Items.Count > 1)
            {
                Divselectkid.Visible = true;
            }
        }

        protected void Drp_StudentName_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyParentInfo.ChangeStudent(Drp_StudentName.SelectedItem.Text, int.Parse(Drp_StudentName.SelectedValue));
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            Incident MyIncident = new Incident(_mysqlObj);
            string _ClassName, _RollN0, _AdmissionN0, _studentImg, ClassId;
            int _Age, _Point, _rating;
            MyParent.GetStudentDetails(MyParentInfo.StudentId, out _ClassName, out _RollN0, out _Age, out _AdmissionN0, out _studentImg, out ClassId);
            //MyIncident.Get
            MyIncident.GetPointRating(MyParentInfo.StudentId, MyParentInfo.CurrentBatchId, out _Point, out _rating);
            MyParentInfo.SetStudentDetails(_ClassName, _RollN0, _Age, _AdmissionN0, _Point, _rating, _studentImg, ClassId);
            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyParent = null;
            MyIncident = null;
            Session["MyParentObj"] = MyParentInfo;
            Response.Redirect(GetCurrentPageName());
        }

        public string GetCurrentPageName()
        {
            string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
            string sRet = oInfo.Name;
            return sRet;
        }

        //protected void imggmail_Click(object sender, EventArgs e)
        //{
        //    string discoveryUri = "https://www.google.com/accounts/o8/id".ToString();
        //    using (OpenIdRelyingParty openid = new OpenIdRelyingParty())
        //    {
        //        var URIbuilder = new UriBuilder(Request.Url) { Query = "" };
        //        var req = openid.CreateRequest(discoveryUri, URIbuilder.Uri, URIbuilder.Uri);
        //        IAuthenticationRequest request = openid.CreateRequest(discoveryUri);

        //        var fetch = new FetchRequest();
        //        fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
        //        request.AddExtension(fetch);

        //        // Send your visitor to their Provider for authentication.
        //        request.RedirectToProvider();
        //    }
        //}
    }
}
