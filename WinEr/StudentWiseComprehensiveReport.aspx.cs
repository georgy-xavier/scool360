using System;
namespace WinEr
{
    public partial class StudentWiseComprehensiveReport : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        protected void Page_Load(object sender, EventArgs e)
        {

            WC_ComprehensiveReport.EVENTCreateReport += new EventHandler(Wc_ComprehensiveReport_Create);
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (Session["StudId"] == null)
            {
                Response.Redirect("SearchStudent.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(834))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {


                if (!IsPostBack)
                {
                 //   string _MenuStr;
                 //   _MenuStr = MyStudMang.GetSubStudentMangMenuString(MyUser.UserRoleId, int.Parse(Session["StudType"].ToString()));
                 //   this.SubStudentMenu.InnerHtml = _MenuStr;
                 //   LoadpupilTopData();

                    WC_ComprehensiveReport.STUDENTID = Session["StudId"].ToString();
                    //some initlization

                }
            }
        }
        protected void Wc_ComprehensiveReport_Create(object sender, EventArgs e)
        {

        }

    }
}  