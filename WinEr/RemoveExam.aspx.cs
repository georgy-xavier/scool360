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
public partial class RemoveExam : System.Web.UI.Page
{
    private ExamManage MyExamMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    //private DataSet MydataSet;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_init(object sender, EventArgs e)
    {

        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        if (Session["ExamId"] == null)
        {
            Response.Redirect("ViewExams.aspx");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyExamMang = MyUser.GetExamObj();
        if (MyExamMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }
        else if (!MyUser.HaveActionRignt(30))
        {
            Response.Redirect("RoleErr.htm");
        }
        else
        {


            if (!IsPostBack)
            {
                string _ExamMenuStr;
                _ExamMenuStr = MyExamMang.GetSubExamMangMenuString(MyUser.UserRoleId);
                this.SubExammngMenu.InnerHtml = _ExamMenuStr;

                LoadExamGeneralDetails();
               

                //some initlization

            }

        }
    }

    private void LoadExamGeneralDetails()
    {
        string ExamNme, ExamType, examfreqncy;
        MyExamMang.GetExamDetail(int.Parse(Session["ExamId"].ToString()), out ExamNme, out ExamType, out examfreqncy);
        Lbl_ExamName.Text = ExamNme.ToString();
        Lbl_ExamType.Text = ExamType.ToString();
        Lbl_Frequency.Text = examfreqncy.ToString();
    }
 
    protected void btnremove_Click(object sender, EventArgs e)
    {
        int _ExamId= int.Parse(Session["ExamId"].ToString());
        Delete(_ExamId);
        
    }
    protected void Btn_Finish_Click(object sender, EventArgs e)
    {

        Response.Redirect("ViewExams.aspx");
    }

    private void Delete(int _ExamId)
    {

        string sql = "select tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId where tblclassexam.ExamId=" + _ExamId + " AND tblexamschedule.BatchId=" + MyUser.CurrentBatchId;
        MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            Lbl_altmessage.Text = "Cannot remove exam, because this exam is scheduled for this batch .";
            MPE_ExamMessage.Show();
        }
        else
        {
            sql = "select tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId where tblclassexam.ExamId=" + _ExamId + " AND tblexamschedule.BatchId<>" + MyUser.CurrentBatchId; 
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                MyExamMang.UpdateExamStatus(_ExamId);
                Session["ExamId"] = null;
               
                Lbl_msg.Text = "Exam is deleted";
                MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Delete Exam", "One Exam " + Lbl_ExamName.Text.ToString() + " is  deleted", 1,1);

            }
            else
            {
                MyExamMang.DeleteExam(_ExamId);
                Session["ExamId"] = null;                
                Lbl_msg.Text = "Exam Deleted";
                MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Delete Exam", "One Exam " + Lbl_ExamName.Text.ToString() + " is deleted", 1,1);
            }
            MPE_MessageBox.Show();
        }
        
    } 
    
}
