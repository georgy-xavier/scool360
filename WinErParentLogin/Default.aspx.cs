using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;

using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using System.Text;

namespace WinErParentLogin
{
    public partial class defaultlogin : System.Web.UI.Page
    {
        private SchoolClass objSchool = null;

        protected void Page_Load(object sender, EventArgs e)
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
                            Session[WinerConstants.SessionSchool] = objSchool;
                    }

                }
                if (Session[WinerConstants.SessionSchool] == null)
                {
                    Response.Redirect("sectionerr.htm");
                }
                objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
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
                            //redirect to main page of your website
                            UpdateSessionValues(email);

                          
                        }
                        else
                        {
                            Response.Redirect("<script>alert('Login Failed')</script>");
                        }

                        break;
                    case AuthenticationStatus.Canceled:
                        Response.Redirect("<script>alert('Login Cancelled')</script>");
                        break;
                    case AuthenticationStatus.Failed:
                        Response.Redirect("<script>alert('Login Failed')</script>");
                        break;
                }
            }
            #endregion
            if (!IsPostBack)
            {

                LoadDocumentCookie(objSchool.SchoolId);
                
                string SchoolName, address;
                StringBuilder School = new StringBuilder("");
                MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(objSchool));
                ParentLogin MyParent = new ParentLogin(_mysqlObj, objSchool);
                MyParent.GetSchoolDetails(out SchoolName,  out address);

                schoollogo.InnerHtml = " <img src=\"" + "Handler/ImageReturnHandler.ashx?id=" + objSchool.SchoolId + "&type=Logo" + "\" class=\"schoollogo\" height=\"120\" alt=\"school logo\"/>";
                schoolname.InnerHtml = SchoolName;
                schooladd.InnerHtml = address;
            //  bodysection.Style["background-images"] =Page.ResolveUrl("pslogindesign/psloginhomeimage.jpg"); 

                //bdy.Attributes.Add("style", " background:White url('pslogindesign/" + psimage + "')no-repeat center center fixed;width:100%;         font-size:12px; font-family:@Arial Unicode MS; -webkit-background-size: cover;-moz-background-size: cover; -o-background-size: cover; background-size: cover;");
                bdy.Attributes.Add("style", " background:White url('Handler/ImageReturnHandler.ashx?id=" + objSchool.SchoolId + "&type=SchoolImage')no-repeat center fixed;width:100%;         font-size:12px; font-family:@Arial Unicode MS; -webkit-background-size:  100% 100%;-moz-background-size:  100% 100%; -o-background-size:  100% 100%; background-size:  100% 100%;");
                
                
            }
            var page = (Page)HttpContext.Current.Handler;
            page.Title = objSchool.SchoolName.ToUpperInvariant() + " Parent Login";
           
        }

        private void LoadDocumentCookie(int intSchoolId)
        {
            string _InnerHtml = " <script type=\"text/javascript\"> document.cookie = \"0$#$" + intSchoolId + "\";    </script>";
            this.javascriptId.InnerHtml = _InnerHtml;
        }

        private void UpdateSessionValues(string email)
        {
             MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(objSchool));
             DBLogClass _DblogObj = new DBLogClass(_mysqlObj);
             ParentInfoClass MyParentObj;
            ParentLogin MyParent = new ParentLogin(_mysqlObj, objSchool);
            string message="";
            int ACtivationStatus = 0;
            if (MyParent.SecureLogin(email, "Gmail", out message, out MyParentObj, out ACtivationStatus))
            {

               
                MyParent.RecordLogin(txtUsertName.Text.ToString());
                MyParentObj.SchoolObject = objSchool;

                //MyParentObj.SetBatchDetails(MyParent.GetBatchId(out BatchName), BatchName);
                Session["MyParentObj"] = MyParentObj;
                Session["ACtivationStatus"] = ACtivationStatus;
                _DblogObj.LogToDb(MyParentObj.ParentName, "ParentLogin_UserLoggin ", "Parents of " + MyParentObj.StudentName + " is logged in parentstudentlogin", 1, 2);
                Response.Redirect("Home.aspx");
            }
            else
            {
                lblErr.Text = message;

            }
            _mysqlObj.CloseConnection();
            _DblogObj = null;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(objSchool));
           
            ParentLogin MyParent = new ParentLogin(_mysqlObj, objSchool);
            string _UserName = txtUsertName.Text.Trim();
            string _PassWord = txtPassword.Text.Trim();
            //string _UserName = Client_Login.UserName.Trim();
            //string _PassWord = Client_Login.Password.Trim();
            ParentInfoClass MyParentObj;
            string _Message = "";
            //string BatchName = "";
            if (MyParent.LoginEnabled(out _Message) && MyParent.LoginSuccess(_UserName, _PassWord, out _Message, out MyParentObj))
            {
                
                MyParent.RecordLogin(txtUsertName.Text.ToString());
                MyParentObj.SchoolObject = objSchool;
                //MyParentObj.SetBatchDetails(MyParent.GetBatchId(out BatchName), BatchName);
                Session["MyParentObj"] = MyParentObj;
                DBLogClass _DblogObj = new DBLogClass(_mysqlObj);

                _DblogObj.LogToDb(MyParentObj.ParentName, "ParentLogin_UserLoggin ", "Parents of " + MyParentObj.StudentName + " is logged in parentstudentlogin", 1, 2);
                _mysqlObj = null;
                Session["ACtivationStatus"] = 0;
                Response.Redirect("Home.aspx");
            }
            else
            {
               
                lblErr.Text = _Message;
            }
            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyParentObj = null;
            
        }
 
       

    }
}
