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
    public partial class ExamTimeTable : System.Web.UI.Page
    {
        private ParentInfoClass MyParentInfo;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet = null;


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
                LoadExams();


            }
        }

        private void LoadExams()
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);

            string sql = "SELECT tblexamschedule.Id,tblexammaster.ExamName,tblclass.ClassName,tblexamschedule.`Status`,tblclassexam.ExamId,tblclassexam.ClassId,tblexamschedule.PeriodId  FROM tblexamschedule INNER JOIN tblclassexam ON tblexamschedule.ClassExamId=tblclassexam.Id INNER JOIN tblexammaster ON tblclassexam.ExamId=tblexammaster.Id INNER JOIN tblclass ON tblclassexam.ClassId=tblclass.Id WHERE tblclassexam.`Status`=1 AND tblexammaster.`Status`=1  AND tblexamschedule.BatchId=" + MyParentInfo.CurrentBatchId + "  AND tblclass.Id=" + MyParentInfo.CLASSID ;
            MydataSet = _mysqlObj.ExecuteQueryReturnDataSet(sql);
           
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                drpExam.DataTextField = "ExamName";
                drpExam.DataValueField = "Id";
                drpExam.DataSource = MydataSet.Tables[0];
                drpExam.DataBind();
                
            }
            else
            {
                drpExam.DataSource = null;
                drpExam.DataBind();
                drpExam.Items.Add(new ListItem("Exam details doesnot found", "0"));

            }
            _mysqlObj = null;
            MyParent = null;
        }

        protected void btn_Show_Click(object sender, EventArgs e)
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);

            if (drpExam.SelectedValue != "0")
            {
                string sql = "select tblsubjects.subject_name, tblsubjects.SubjectCode,date_format(tblexammark.ExamDate,'%d/%m/%Y') AS `ExamDate` , tbltimeslot.StartTime, tbltimeslot.EndTime, tblclassexamsubmap.MinMark , tblclassexamsubmap.MaxMark from tblclassexam   inner join tblexamschedule on tblclassexam.Id= tblexamschedule.ClassExamId AND tblexamschedule.BatchId=" + MyParentInfo.CurrentBatchId + "  inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblclassexam.Id   inner join tblexammark on tblexammark.ExamSchId= tblexamschedule.Id AND tblexammark.SubjectId= tblclassexamsubmap.SubId inner join tbltimeslot   on tbltimeslot.Id= tblexammark.TimeSlotId inner join tblsubjects on tblsubjects.Id= tblexammark.SubjectId where tblclassexam.ClassId=" + MyParentInfo.CLASSID + "   AND tblexamschedule.Id=" + drpExam.SelectedValue + " order by tblexammark.SubjectOrder  ";

                MydataSet = _mysqlObj.ExecuteQueryReturnDataSet(sql);
                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    Grd_ExamSchdule.DataSource = MydataSet;
                    Grd_ExamSchdule.DataBind();

                }
                else
                {
                    Grd_ExamSchdule.DataSource = null;
                    Grd_ExamSchdule.DataBind();
                }
            }
            _mysqlObj = null;
            MyParent = null;
    

        }

    }

}
