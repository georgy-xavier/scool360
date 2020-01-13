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
using System.util;

namespace WinEr
{
    public partial class ClassTeacherExamReport : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        private ExamManage MyExamMang;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet; 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyExamMang = MyUser.GetExamObj();
            if (MyExamMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(251))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {

                    AddClassNameToDrpList();

                }
            }
        }

        private void AddClassNameToDrpList()
        {
            Drp_Class.Items.Clear();
            MyDataSet = MyUser.MyAssociatedClass();
            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                ListItem li = new ListItem("Select any class", "-1");
                Drp_Class.Items.Add(li);
                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {
                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Class present", "-1");
                Drp_Class.Items.Add(li);
            }
            Drp_Class.SelectedIndex = 0;
        }


        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {

            string ExmType ="";
            LoadExams(int.Parse(Drp_Class.SelectedValue), ExmType);
        }


        protected void Rdo_ExmType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ExmType = "";
            LoadExams(int.Parse(Drp_Class.SelectedValue), ExmType);
        }
        private void LoadExams(int _ClassId, string _ExmType)
        {
            Drp_Exam.Items.Clear();
            string sql ="";
            //if (Rdo_ExmType.SelectedValue == "1")
            //{
            //sql = "select distinct tblexamcombmaster.Id , tblexamcombmaster.ExamName from tblexamcombmaster inner join tblexamcombmap on tblexamcombmap.CombinedId = tblexamcombmaster.Id where tblexamcombmap.ClassId=" + _ClassId;
            //}
            //else
            //{
                sql = "select distinct tblexammaster.Id, tblexammaster.ExamName from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster ON tblexammaster.Id= tblclassexam.ExamId inner join tblstudentmark on tblstudentmark.ExamSchId= tblexamschedule.Id where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "  AND tblexammaster.`Status`=1 and tblclassexam.ClassId=" + _ClassId + "";
            //}
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_GenReport.Enabled = true;
                ListItem li = new ListItem("Select any exam", "-1");
                Drp_Exam.Items.Add(li);
                while(MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(),MyReader.GetValue(0).ToString());
                    Drp_Exam.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No exam found", "-1");
                Drp_Exam.Items.Add(li);
                Btn_GenReport.Enabled = false;
            }
        }

        protected void Btn_GenReport_Click(object sender, EventArgs e)
        {
            if ((Drp_Exam.SelectedValue != "-1") && (Drp_Class.SelectedValue !="-1"))
            {
                int ExamId = int.Parse(Drp_Exam.SelectedValue);
                string FileName = Drp_Class.SelectedItem.ToString();
                // FileName = FileName + "Exam Report.xls";
                // ClassTeacherReport.InnerHtml = MyExamMang.GetClassTeacherReport(int.Parse(Drp_Class.SelectedValue), ExamId);
                Response.ContentType = "application/force-download";
                Response.AddHeader("content-disposition", "attachment; filename=" + FileName + ".xls");
                Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
                Response.Write("<head>");
                Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                Response.Write("<!--[if gte mso 9]><xml>");
                Response.Write("<x:ExcelWorkbook>");
                Response.Write("<x:ExcelWorksheets>");
                Response.Write("<x:ExcelWorksheet>");
                Response.Write("<x:Name>Exam Report</x:Name>");
                Response.Write("<x:WorksheetOptions>");
                Response.Write("<x:Print>");
                Response.Write("<x:ValidPrinterInfo/>");
                Response.Write("</x:Print>");
                Response.Write("</x:WorksheetOptions>");
                Response.Write("</x:ExcelWorksheet>");
                Response.Write("</x:ExcelWorksheets>");
                Response.Write("</x:ExcelWorkbook>");
                Response.Write("</xml>");
                Response.Write("<![endif]--> ");
                Response.Write(MyExamMang.GetClassTeacherReport(int.Parse(Drp_Class.SelectedValue), ExamId));
                Response.Write("</head>");
                Response.Flush();
                Response.End();
            }
        }
    }
}
