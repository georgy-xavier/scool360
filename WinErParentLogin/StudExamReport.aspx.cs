using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;
namespace WinErParentLogin
{
    public partial class StudExamReport : System.Web.UI.Page
    {
        private ParentInfoClass MyParentInfo;
        private OdbcDataReader MyReader = null;
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
                Pnl_errormessage.Visible = false;

                LoadSchoolDetails();
                LoadLogo();
                LoadStudentDetails();
                LoadExamNameAndType();
                LoadExamReport();

                MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
                DBLogClass _DblogObj = new DBLogClass(_mysqlObj);

                _DblogObj.LogToDb(MyParentInfo.ParentName, "ParentLogin_Exam reportpage", " Visited exam report page", 4, 2);
                _DblogObj = null;
                _mysqlObj = null;

                //Label MyHeader = (Label)this.Master.FindControl("Lbl_PageHeader");
                //MyHeader.Text = "Exam Details";
            }
        }
        private void LoadExamReport()
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            StudentManagerClass MyStudMang = new StudentManagerClass(_mysqlObj);
            this.ExmReportDiv.InnerHtml = MyStudMang.GetStudentExamReportTable(MyParentInfo.StudentId, int.Parse(Request.QueryString["SchId"].ToString()), int.Parse(Txt_examid.Text.ToString()), MyParentInfo.CurrentBatchId);
            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyStudMang = null;
        }

        private void LoadExamNameAndType()
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            string Sql = "select tblexammaster.Id, tblexammaster.ExamName, tblperiod.Period from tblexamschedule inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId INNER join tblperiod on tblperiod.Id= tblexamschedule.PeriodId where tblexamschedule.Id=" + Request.QueryString["SchId"].ToString();
            MyReader = MyParent.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Txt_examid.Text = MyReader.GetValue(0).ToString();
                Lbl_exam.Text = MyReader.GetValue(1).ToString();
                Lbl_Type.Text = MyReader.GetValue(2).ToString();
                MyReader.Close();
            }
            ReleaseResourse(_mysqlObj, MyParent);
        }
        private void ReleaseResourse(MysqlClass _mysqlObj, ParentLogin MyParent)
        {
            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyParent = null;
        }

        private void LoadStudentDetails()
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            string Sql = "SELECT StudentName FROM tblstudent WHERE Id=" + MyParentInfo.StudentId;
            MyReader = MyParent.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                Lbl_StudName.Text = MyReader.GetValue(0).ToString();
            }
            Sql = "SELECT tblstandard.Name,tblclass.ClassName from tblstudentclassmap INNER join tblclass on tblstudentclassmap.ClassId=tblclass.Id inner join tblstandard on tblstandard.Id = tblclass.Standard where tblstudentclassmap.BatchId=" + MyParentInfo.CurrentBatchId + " And tblstudentclassmap.StudentId= " + MyParentInfo.StudentId + "";
            MyReader = MyParent.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Lbl_Standard.Text = MyReader.GetValue(0).ToString();
                Lbl_Class.Text = MyReader.GetValue(1).ToString();
                MyReader.Close();
            }
            Lbl_CurrBatch.Text = MyParentInfo.CurrentBatchName;
            ReleaseResourse(_mysqlObj, MyParent);
        }

        private void LoadLogo()
        {
            //MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            //ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            //string ImageUrl = "";
            //string Sql = "SELECT LogoUrl FROM tblschooldetails WHERE Id=1";
            //MyReader = MyParent.m_MysqlDb.ExecuteQuery(Sql);
            //if (MyReader.HasRows)
            //{
            //    ImageUrl = MyReader.GetValue(0).ToString();
            //}
            //else
            //{
            //    ImageUrl = "img.png";
            //}

            Img_logo.ImageUrl = MyParentInfo.SCHOOLLOGO;
            //ReleaseResourse(_mysqlObj, MyParent);
        }

        private void LoadSchoolDetails()
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            string Sql = "SELECT SchoolName,Address FROM tblschooldetails WHERE Id=1";
            MyReader = MyParent.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                Lbl_schoolname.Text = MyReader.GetValue(0).ToString();
                Lbi_subHead.Text = MyReader.GetValue(1).ToString();
            }
            ReleaseResourse(_mysqlObj, MyParent);
        }
    }
}
