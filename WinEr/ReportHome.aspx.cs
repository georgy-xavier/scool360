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
using System.Text;
namespace WinEr
{
    public partial class ReportHome : System.Web.UI.Page
    {
      
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private Reports MyReport;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyReport = MyUser.GetReportObj();
            if (MyReport == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else
            {
                if (!IsPostBack)
                {
                    Pnl_General.Visible = false;
                    Pnl_Fee.Visible = false;
                    Pnl_Exam.Visible = false;
                    Pnl_Attendance.Visible = false;
                    ShowReports();
                }
            }
        }

        private void ShowReports()
        {
            Div_General.InnerHtml = GetReports();
            Pnl_General.Visible = true;
            if (MyUser.HaveModule(1))//Fee
            {
                Div_Fee.InnerHtml = GetFeeReports();
               
            }
            if (MyUser.HaveModule(3))//Exam
            {
                Div_Exam.InnerHtml = GetExamReports();
            }
            if (MyUser.HaveModule(21))//Exam
            {
                Div_Attendance.InnerHtml = GetAttendanceReports();
            }
        }

        private string GetAttendanceReports()
        {
            bool flag = false;
            StringBuilder GenReport = new StringBuilder("<br/>");
            GenReport.Append("<ul>");
            if (MyUser.HaveActionRignt(95))
            {
                flag = true;
                GenReport.Append("<li><a href=\"ClassAttendanceReport.aspx\">");

                GenReport.Append("Class Wise Report</a></li>");
            }
            
            if (flag)
                Pnl_Attendance.Visible = true;
            GenReport.Append("</ul><br/>");
            return GenReport.ToString();
        }

        private string GetExamReports()
        {
            bool flag = false;
            StringBuilder GenReport = new StringBuilder("<br/>");
            GenReport.Append("<ul>");
            if (MyUser.HaveActionRignt(77))
            {
                flag = true;
                GenReport.Append("<li><a href=\"ClasswiseExamReport.aspx\">");

                GenReport.Append("Class Wise Report</a></li>");
            }
            if (MyUser.HaveActionRignt(122))
            {
                flag = true;
                GenReport.Append("<li><a href=\"ClassExamReport.aspx\">");

                GenReport.Append("Consolidate Report</a></li>");
            }
            if (MyUser.HaveActionRignt(251))
            {
                flag = true;
                GenReport.Append("<li><a href=\"ClassTeacherExamReport.aspx\">");

                GenReport.Append("ClassTeacher Report</a></li>");
            }
            if (MyUser.HaveActionRignt(300))
            {
                flag = true;
                GenReport.Append("<li><a href=\"ExamStatusReport.aspx\">");

                GenReport.Append("Exam Status Report</a></li>");
            }
            if (MyUser.HaveActionRignt(302))
            {
                flag = true;
                GenReport.Append("<li><a href=\"StudentCombinedExamReport.aspx\">");

                GenReport.Append("Progress Report</a></li>");
            }
            if (flag)
                Pnl_Exam.Visible = true;
            GenReport.Append("</ul><br/>");
            return GenReport.ToString();
        }

        private string GetFeeReports()
        {
            bool flag = false;
            StringBuilder GenReport = new StringBuilder("<br/>");
            GenReport.Append("<ul>");
            if (MyUser.HaveActionRignt(21))
            {
                flag = true;
                GenReport.Append("<li><a href=\"ViewFeeReport.aspx\">");

                GenReport.Append("BatchWiseReport</a></li>");
            }
            if (MyUser.HaveActionRignt(8))
            {
                flag = true;
                GenReport.Append("<li><a href=\"SearchFeeAccount.aspx\">");

                GenReport.Append("Daily Report</a></li>");
            }
            if (MyUser.HaveActionRignt(253))
            {
                flag = true;
                GenReport.Append("<li><a href=\"AbsDlyFeeReport.aspx\">");

                GenReport.Append("Abstract Fee Report</a></li>");
            }
            if(flag)
                Pnl_Fee.Visible = true;
            GenReport.Append("</ul><br/>");
            return GenReport.ToString();
        }

        public string GetReports()
        {
            StringBuilder GenReport = new StringBuilder("<br/>");
            GenReport.Append("<ul>");
                if(MyUser.HaveActionRignt(129))
                {
                    GenReport.Append("<li><a href=\"TcIssueReport.aspx\">");
              
                    GenReport.Append("Tc/resignation Reports</a></li>");
                }
                GenReport.Append("</ul><br/>");
            return GenReport.ToString();
        }
    }
}
