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
namespace Scool360student

{
    public partial class StudExamReport : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;

        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Pnl_content.Visible = false;
            }
            else if (Session["StudId"] == null)
            {
                Pnl_content.Visible = false;
            }
            else if (Request.QueryString["SchId"] == null)
            {
                Pnl_content.Visible = false;
            }
            else
            {
                
                MyUser = (KnowinUser)Session["UserObj"];
                MyStudMang = MyUser.GetStudentObj();
                if (MyStudMang == null)
                {
                    Response.Redirect("Default.aspx");
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

                        Pnl_errormessage.Visible = false;
  
                        LoadSchoolDetails();
                        LoadLogo();
                        LoadStudentDetails();
                        LoadExamNameAndType();
                        LoadExamReport();
                        //some initlization

                    }
                }
            }
        }

        private void LoadExamReport()
        {
            this.ExmReportDiv.InnerHtml = MyStudMang.GetStudentExamReportTable(int.Parse(MyUser.StudId.ToString()), int.Parse(Request.QueryString["SchId"].ToString()),int.Parse(Txt_examid.Text.ToString()),MyUser.CurrentBatchId);
        }

        private void LoadExamNameAndType()
        {
            string Sql = "select tblexammaster.Id, tblexammaster.ExamName, tblperiod.Period from tblexamschedule inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId INNER join tblperiod on tblperiod.Id= tblexamschedule.PeriodId where tblexamschedule.Id=" + Request.QueryString["SchId"].ToString();
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Txt_examid.Text = MyReader.GetValue(0).ToString();
                Lbl_exam.Text = MyReader.GetValue(1).ToString();
                Lbl_Type.Text = MyReader.GetValue(2).ToString();
                MyReader.Close();
            }
        }


        private void LoadStudentDetails()
        {
            string Sql = "SELECT StudentName FROM tblstudent WHERE Id=" + int.Parse(MyUser.StudId.ToString());
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                Lbl_StudName.Text = MyReader.GetValue(0).ToString();
            }
            Sql = "SELECT tblstandard.Name,tblclass.ClassName from tblstudentclassmap INNER join tblclass on tblstudentclassmap.ClassId=tblclass.Id inner join tblstandard on tblstandard.Id = tblclass.Standard where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " And tblstudentclassmap.StudentId= " + int.Parse(MyUser.StudId.ToString()) + "";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Lbl_Standard.Text = MyReader.GetValue(0).ToString();
                Lbl_Class.Text = MyReader.GetValue(1).ToString();
                MyReader.Close();
            }
            Lbl_CurrBatch.Text = MyUser.CurrentBatchName.ToString();
        }

        private void LoadLogo()
        {

            Img_logo.ImageUrl = "Handler/ImageReturnHandler.ashx?id=1&type=Logo";

        }

        private void LoadSchoolDetails()
        {
            string Sql = "SELECT SchoolName,Address FROM tblschooldetails WHERE Id=1";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                Lbl_schoolname.Text = MyReader.GetValue(0).ToString();
                Lbi_subHead.Text = MyReader.GetValue(1).ToString();
            }
        }

    }

}
