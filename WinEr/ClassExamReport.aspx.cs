using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using WinBase;

namespace WinEr
{
    public partial class ClassExamReport : System.Web.UI.Page
    {

        private OdbcDataReader m_MyReader = null;
        private ClassOrganiser MyClassMang;
        private KnowinUser MyUser;
        private ExamManage MyExamMang;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet;
        private DataSet MyExamDataSet = null;
        private MysqlClass m_TransationDb=null;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            //if (Session["ClassId"] == null)
            //{
            //    Response.Redirect("LoadClassDetails.aspx");
            //}
            MyUser = (KnowinUser)Session["UserObj"];
            MyClassMang = MyUser.GetClassObj();
            MyExamMang = MyUser.GetExamObj();
            if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MyExamMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(122))
            {
                Response.Redirect("RoleErr.htm");
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
                    Btn_Generate.Enabled = false;
                    LoadExamType();

                    LoadExamTypeToPeriodExam();

                    if (Session["ClassId"] != null)
                    {
                        LoadAllClassDetailsToDropDown(int.Parse(Session["ClassId"].ToString()));
                        LoadAllClassDetailsToDropDownPeriodReport(int.Parse(Session["ClassId"].ToString()));
                    }
                    else
                    {
                        LoadAllClassDetailsToDropDown(0);
                        LoadAllClassDetailsToDropDownPeriodReport(0);
                    }
                   
                }
            }
        }

       

        #region CONSOLIDATE EXAM REPORT

        private void LoadAllClassDetailsToDropDown(int _ClassId)
        {
            Drp_ClassSelect.Items.Clear();

            string sql = "SELECT tblclass.Id,tblclass.ClassName FROM tblclass order by tblclass.Standard, tblclass.ClassName";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_ClassSelect.Items.Add(li);

                }
                Drp_ClassSelect.SelectedValue = _ClassId.ToString();
            }
            else
            {
                ListItem li = new ListItem("No Standard Found", "-1");
                Drp_ClassSelect.Items.Add(li);
            }

        }       

        private void LoadExamType()
        {
            Drp_ExamType.Items.Clear();
            string sql = "select tblsubject_type.Id, tblsubject_type.sbject_type from tblsubject_type";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_Exam.Enabled = true;
                Drp_ExamType.Items.Add(new ListItem("Select any exam type", "0"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_ExamType.Items.Add(li);
                }

            }
            else
            {
                Drp_ExamType.Items.Add(new ListItem("No exam type found", "0"));
                Drp_Exam.Enabled = false;
                Btn_Generate.Enabled = false;
            }
        }

        protected void Drp_ExamType_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (int.Parse(Drp_ExamType.SelectedValue) != 0)
            {
                LoadExamToDrpList();
            }
            else
            {
                Btn_Generate.Enabled = false;
                Drp_Exam.Items.Clear();
            }
        }

        private void LoadExamToDrpList()
        {

            Drp_Exam.Items.Clear();
            string sql = "select distinct tblexammaster.Id, tblexammaster.ExamName from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster ON tblexammaster.Id= tblclassexam.ExamId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id  where tblclassexam.ClassId = " + int.Parse(Drp_ClassSelect.SelectedValue) + " and tblexamschedule.BatchId = " + MyUser.CurrentBatchId + " and tblexammaster.ExamTypeId= " + int.Parse(Drp_ExamType.SelectedValue) + "";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            { 
                Drp_Exam.Items.Add(new ListItem("Select any exam", "0"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Exam.Items.Add(li);
                }

            }
            else
            {
                Drp_Exam.Items.Add(new ListItem("No exam found", "0"));
                Btn_Generate.Enabled = false;
            }
        }

        protected void Drp_ClassSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadExamToDrpList();
            LoadExamType();
        }

        protected void Drp_Exam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(Drp_Exam.SelectedValue) != 0)
            {
                Btn_Generate.Enabled = true;
            }
            else
            {
                Btn_Generate.Enabled = false;
            }

        }

        protected void Btn_Generate_Click(object sender, EventArgs e)
        {
            ExamReportPdf MyPdf = new ExamReportPdf(MyClassMang.m_MysqlDb, MyUser, objSchool);
           
            string  _ErrorMsg = "";
            string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
            string _PdfName = "";
            int _ClassId = int.Parse(Drp_ClassSelect.SelectedValue);
            int _ExamId = int.Parse(Drp_Exam.SelectedValue);

            if (MyPdf.CreateExamReportPdf(_ClassId,_ExamId, MyUser.CurrentBatchId,MyUser.CurrentBatchName, _physicalpath, out _PdfName, out _ErrorMsg))
            {
                _ErrorMsg = "";
                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                Lbl_msg.Text = _ErrorMsg;
                //MPE_MessageBox.Show();
            }
            else
            {
               // _ErrorMsg = "Faild To Create";
                Lbl_msg.Text = _ErrorMsg;
                MPE_MessageBox.Show();
            }

        }


        #endregion

        #region INDIVIDUAL EXAM REPORT

        private void LoadAllClassDetailsToDropDownPeriodReport(int _ClassId)
        {
            Drp_ClassSelectIndi.Items.Clear();

            string sql = "SELECT tblclass.Id,tblclass.ClassName FROM tblclass order by tblclass.Standard, tblclass.ClassName";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_ClassSelectIndi.Items.Add(li);

                }
                Drp_ClassSelectIndi.SelectedValue = _ClassId.ToString();
            }
            else
            {
                ListItem li = new ListItem("No Standard Found", "-1");
                Drp_ClassSelectIndi.Items.Add(li);
            }
        }

        private void LoadExamTypeToPeriodExam()
        {
            Drp_ExamTypeIndividual.Items.Clear();
            string sql = "select tblsubject_type.Id, tblsubject_type.sbject_type from tblsubject_type";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_ExamIndi.Enabled = true;
                Drp_ExamTypeIndividual.Items.Add(new ListItem("Select any exam type", "0"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_ExamTypeIndividual.Items.Add(li);
                }

            }
            else
            {
                Drp_ExamTypeIndividual.Items.Add(new ListItem("No exam type found", "0"));
                Drp_ExamIndi.Enabled = false;
                Btn_IndiGenerate.Enabled = false;
            }
        }

        protected void Drp_ExamIndi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(Drp_ExamIndi.SelectedValue) != 0)
            {
                LoadPeriodToDropExamPeriod();
            }
            else
            {
                Btn_IndiGenerate.Enabled = false;
                Drp_ExamPeriod.Items.Clear();
            }
        }

        private void LoadPeriodToDropExamPeriod()
        {
            Drp_ExamPeriod.Items.Clear();
            string sql = "";
            sql = "select distinct tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId and tblclassexam.ClassId =" + int.Parse(Drp_ClassSelectIndi.SelectedValue) + " and tblclassexam.ExamId=" + int.Parse(Drp_ExamIndi.SelectedValue) + " inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblexamschedule.BatchId=" + MyUser.CurrentBatchId;
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_IndiGenerate.Enabled = false;
                Drp_ExamPeriod.Items.Add(new ListItem("Select any period", "0"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_ExamPeriod.Items.Add(li);
                }

            }
            else
            {
                Drp_ExamPeriod.Items.Add(new ListItem("No period found", "0"));
                Btn_IndiGenerate.Enabled = false;
            }
        }

        protected void Drp_ExamPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(Drp_ExamPeriod.SelectedValue) != 0)
            {
                Btn_IndiGenerate.Enabled = true;
            }
            else
            {
                Btn_IndiGenerate.Enabled = false;
            }
        }

        protected void Btn_IndiGenerate_Click(object sender, EventArgs e)
        {
            ExamReportPdf MyPdf = new ExamReportPdf(MyClassMang.m_MysqlDb, MyUser, objSchool);
            string _ErrorMsg = "";
            string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
            string _PdfName = "";
            int _ClassId = int.Parse(Drp_ClassSelectIndi.SelectedValue);
            int _ExamId = int.Parse(Drp_ExamIndi.SelectedValue);
            int _PeriodId = int.Parse(Drp_ExamPeriod.SelectedValue);

            if (MyPdf.CreatePeriodExamReportPdf(_ClassId, _ExamId, _PeriodId, MyUser.CurrentBatchId, MyUser.CurrentBatchName, _physicalpath, out _PdfName, out _ErrorMsg))
            {
                _ErrorMsg = "";
                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                Lbl_msg.Text = _ErrorMsg;
                //MPE_MessageBox.Show();
            }
            else
            {
                // _ErrorMsg = "Faild To Create";
                Lbl_msg.Text = _ErrorMsg;
                MPE_MessageBox.Show();
            }
        }

        protected void Drp_ExamTypeIndividual_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(Drp_ExamTypeIndividual.SelectedValue) != 0)
            {
                LoadExamToDropIndividual();
            }
            else
            {
                Btn_IndiGenerate.Enabled = false;
                Drp_ExamIndi.Items.Clear();
                Drp_ExamPeriod.Items.Clear();
            }
        }

        private void LoadExamToDropIndividual()
        {

            Drp_ExamIndi.Items.Clear();
            string sql = "select distinct tblexammaster.Id, tblexammaster.ExamName from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster ON tblexammaster.Id= tblclassexam.ExamId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id  where tblclassexam.ClassId = " + int.Parse(Drp_ClassSelectIndi.SelectedValue) + " and tblexamschedule.BatchId = " + MyUser.CurrentBatchId + " and tblexammaster.ExamTypeId= " + int.Parse(Drp_ExamTypeIndividual.SelectedValue) + "";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_ExamIndi.Items.Add(new ListItem("Select any exam", "0"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_ExamIndi.Items.Add(li);
                }

            }
            else
            {
                Drp_ExamIndi.Items.Add(new ListItem("No exam found", "0"));
                Btn_IndiGenerate.Enabled = false;
            }
        }

        protected void Drp_ClassSelectIndi_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadExamTypeToPeriodExam();
            //LoadPeriodToDropExamPeriod();
            //LoadExamToDropIndividual();
        }

        #endregion


    }
}
