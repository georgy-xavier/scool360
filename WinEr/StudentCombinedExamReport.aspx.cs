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
using System.IO;
using WinBase;

namespace WinEr
{
    public partial class StudentCombinedExamReport : System.Web.UI.Page
    {
        private OdbcDataReader m_MyReader = null;
        private ClassOrganiser MyClassMang;
        private KnowinUser MyUser;
        private ExamManage MyExamMang;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet;
        private DataSet MyExamDataSet = null;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
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
            else if (!MyUser.HaveActionRignt(302))
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

                    if (Session["ClassId"] != null)
                    {
                        LoadAllClassDetailsToDropDown(int.Parse(Session["ClassId"].ToString()));
                    }
                    else
                    {
                        LoadAllClassDetailsToDropDown(0);
                    }

                    LoadInitialCondition();
                    LoadAllExamsToGrid();                   
                }
            }
        }

        private void LoadInitialCondition()
        {
            Lbl_Message.Text = "";
        }

        private void LoadAllExamsToGrid()
        {
            DataSet ExamDataSet = new DataSet();
            DataTable dt;
            DataRow dr;
            ExamDataSet.Tables.Add(new DataTable("Exam"));
            dt = ExamDataSet.Tables["Exam"];

            dt.Columns.Add("ExamSchdId");
            dt.Columns.Add("IsCombined");
            dt.Columns.Add("ExamName");
            dt.Columns.Add("ExamId");
            dt.Columns.Add("PeriodId");

            string sql = "SELECT DISTINCT(tblexamschedule.Id), tblexammaster.ExamName, tblclassexam.ExamId, tblperiod.Period,tblexamschedule.PeriodId from tblexammaster inner join tblclassexam on tblclassexam.ExamId= tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblperiod on tblexamschedule.PeriodId= tblperiod.Id inner join tblstudentmark on tblexamschedule.Id= tblstudentmark.ExamSchId where tblclassexam.ClassId=" + int.Parse(Drp_ClassSelect.SelectedValue) + " order by tblexamschedule.Id asc";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    dr = ExamDataSet.Tables["Exam"].NewRow();

                    dr["ExamSchdId"] = MyReader.GetValue(0).ToString();
                    dr["IsCombined"] = "0";
                    dr["ExamName"] = MyReader.GetValue(1).ToString() +" ("+ MyReader.GetValue(3).ToString()+")";
                    dr["ExamId"] = MyReader.GetValue(2).ToString();
                    dr["PeriodId"] = MyReader.GetValue(4).ToString();
                    ExamDataSet.Tables["Exam"].Rows.Add(dr);
                }
            }


            sql = "SELECT DISTINCT(tblexamcombmaster.Id), tblexamcombmaster.ExamName from tblexamcombmaster inner join tblexamcombmap on tblexamcombmaster.Id= tblexamcombmap.CombinedId where tblexamcombmap.ClassId=" + int.Parse(Drp_ClassSelect.SelectedValue)+" order by tblexamcombmaster.Id asc";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    dr = ExamDataSet.Tables["Exam"].NewRow();

                    dr["ExamSchdId"] = MyReader.GetValue(0).ToString();
                    dr["IsCombined"] = "1";
                    dr["ExamName"] = MyReader.GetValue(1).ToString();
                    dr["ExamId"] = "0";
                    dr["PeriodId"] = "0";
                    ExamDataSet.Tables["Exam"].Rows.Add(dr);
                }
            }


            if (ExamDataSet.Tables[0].Rows.Count > 0)
            {                
                Grd_CreateReport.Columns[1].Visible = true;
                Grd_CreateReport.Columns[2].Visible = true;
                Grd_CreateReport.Columns[5].Visible = true;
                Grd_CreateReport.Columns[6].Visible = true;

                Grd_CreateReport.DataSource = ExamDataSet;
                Grd_CreateReport.DataBind();

                Grd_CreateReport.Columns[1].Visible = false;
                Grd_CreateReport.Columns[2].Visible = false;
                Grd_CreateReport.Columns[5].Visible = false;
                Grd_CreateReport.Columns[6].Visible = false;

                if (CheckValidMarks())
                {
                    Lbl_Message.Text = "";
                    Btn_Generate.Enabled = true;
                }
                else
                {
                    Lbl_Message.Text = "All marks are not entered....";
                    Btn_Generate.Enabled = false;
                }
                
            }
            else
            {
                Grd_CreateReport.DataSource = null;
                Grd_CreateReport.DataBind();
                Lbl_Message.Text = "No Exam for Report";
                Btn_Generate.Enabled = false;
            }

        }

        private bool CheckValidMarks()
        {
            bool _valid = true;
            foreach (GridViewRow gv in Grd_CreateReport.Rows)
            {
                if (gv.Cells[5].Text != "0")
                {
                    string sql = " SELECT tblexammark.MarkColumn FROM tblexammark inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblsubjects on tblsubjects.Id=tblexammark.SubjectId inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId where tblclassexam.ExamId=" + gv.Cells[5].Text + " AND tblclassexam.ClassId=" + Drp_ClassSelect.SelectedValue + " AND tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " AND tblexamschedule.PeriodId=" + gv.Cells[6].Text+" order by tblexammark.SubjectOrder";
                    MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        string _Str_Mark;
                        while (MyReader.Read())
                        {
                            string sql1 = "SELECT tblstudentclassmap.StudentId from tblstudentclassmap inner JOIN tblstudent on tblstudentclassmap.StudentId= tblstudent.Id inner join tblstudentmark on tblstudent.Id= tblstudentmark.StudId where tblstudentclassmap.ClassId=" + Drp_ClassSelect.SelectedValue+" and tblstudentmark.ExamSchId in ( select tblexamschedule.Id from tblexamschedule where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblexamschedule.PeriodId=" + gv.Cells[6].Text+" and tblexamschedule.ClassExamId in (select tblclassexam.Id from tblclassexam where tblclassexam.ClassId="+Drp_ClassSelect.SelectedValue+" and tblclassexam.ExamId=" + gv.Cells[5].Text + "))";
                            m_MyReader =MyExamMang.m_MysqlDb.ExecuteQuery(sql1);
                            if (m_MyReader.HasRows)
                            {
                                while (m_MyReader.Read())
                                {
                                    _Str_Mark = MyExamMang.GetMarks(int.Parse(m_MyReader.GetValue(0).ToString()), int.Parse(gv.Cells[5].Text), MyUser.CurrentBatchId, MyReader.GetValue(0).ToString(), Drp_ClassSelect.SelectedValue, gv.Cells[6].Text);
                                    if (_Str_Mark == "")
                                    {
                                        _valid = false;
                                        break;
                                    }
                                }
                            }
                            if (_valid == false)
                            {
                                break;
                            }                            
                        }
                    }
                }                
            }
            return _valid;
        }

        private void LoadAllClassDetailsToDropDown(int _ClassId)
        {
            Drp_ClassSelect.Items.Clear();

            string sql = "SELECT tblclass.Id,tblclass.ClassName FROM tblclass order by tblclass.Standard,tblclass.ClassName";
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

        protected void Drp_ClassSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAllExamsToGrid();
        }

        protected void Btn_Generate_Click(object sender, EventArgs e)
        {
            int _CombinedCount = 0, _IndCount = 0;
            string[,] _ExamScheduleId = new string[30, 4];

            
            if(IsValidData(out _ExamScheduleId,out _CombinedCount,out _IndCount))
            {
                //if (AllSubjectMarkColumnsAreValid())
                //{
                    if ((_CombinedCount + _IndCount) > 0)
                    {
                        ExamReportPdf MyPdf = new ExamReportPdf(MyClassMang.m_MysqlDb, MyUser, objSchool);
                        string _ErrorMsg = "";
                        string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath(""));
                        string _PdfName = "";
                        int _ClassId = int.Parse(Drp_ClassSelect.SelectedValue);

                        if (MyPdf.CreateCombinedExamReportPdf(_ExamScheduleId, _CombinedCount, _IndCount, _ClassId, MyUser.CurrentBatchId, MyUser.CurrentBatchName, _physicalpath, out _PdfName, out _ErrorMsg))
                        {
                            _ErrorMsg = "";
                            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                            Lbl_Message.Text = "";
                        }
                        else
                        {
                            _ErrorMsg = "Failed To Create";
                            Lbl_Message.Text = _ErrorMsg;
                        }
                    }
                //}
                //else
                //{
                //    Lbl_Message.Text = "Exams are not applied for All Subjects..";
                //}
            }
        }

        private bool AllSubjectMarkColumnsAreValid()
        {
            bool _Valid = true;
            OdbcDataReader MyReaderSchedule = null;
            OdbcDataReader MyReaderNextSchedule = null;
            OdbcDataReader MyReaderSchedulId= null;

            string sql = "SELECT tblexamschedule.Id from tblexamschedule where tblexamschedule.ClassExamId in (select tblclassexam.Id from tblclassexam WHERE tblclassexam.ClassId="+Drp_ClassSelect.SelectedValue+")";
            MyReaderSchedulId = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReaderSchedulId.HasRows)
            {
                while (MyReaderSchedulId.Read())
                {
                    string sql1 = "select DISTINCT(tblexammark.MarkColumn),tblexammark.SubjectId from tblexammark where tblexammark.ExamSchId in( SELECT tblexamschedule.Id from tblexamschedule where tblexamschedule.ClassExamId in (select tblclassexam.Id from tblclassexam WHERE tblclassexam.ClassId=" + Drp_ClassSelect.SelectedValue + ")) order by tblexammark.SubjectOrder";
                    MyReaderSchedule = MyExamMang.m_MysqlDb.ExecuteQuery(sql1);
                    if (MyReaderSchedule.HasRows)
                    {
                        while (MyReaderSchedule.Read())
                        {
                            string sql2 = "select tblexammark.SubjectId from tblexammark where tblexammark.ExamSchId =" + MyReaderSchedulId.GetValue(0).ToString() + " and tblexammark.MarkColumn='" + MyReaderSchedule.GetValue(0).ToString() + "' and tblexammark.SubjectId=" + MyReaderSchedule.GetValue(1).ToString() + " order by tblexammark.SubjectOrder";
                            MyReaderNextSchedule = MyExamMang.m_MysqlDb.ExecuteQuery(sql2);
                            if (MyReaderNextSchedule.HasRows)
                            {
                                if (MyReaderNextSchedule.GetValue(0).ToString() != MyReaderSchedule.GetValue(1).ToString())
                                {
                                    _Valid = false;
                                    break;
                                }
                            }
                            else
                            {
                                _Valid = false;
                                break;
                            }
                            if (_Valid == false)
                            {
                                break;
                            }
                        }
                    }
                    if (_Valid == false)
                    {
                        break;
                    }
                }
            }

            

            return _Valid;
        }

        private bool IsValidData(out string[,] _ExamScheduleId, out int _CombinedCount, out int _IndCount)
        {
             bool _Valid = false;
            _ExamScheduleId = new string[0, 4];
             string[,] _ExamData= new string[Grd_CreateReport.Rows.Count, 4];
            _IndCount = 0; _CombinedCount = 0;
            int _TotalCount = 0;
            double _CumulativeTotal = 0;
            try
            {
                foreach (GridViewRow gv in Grd_CreateReport.Rows)
                {
                    CheckBox Chk_Exam = (CheckBox)gv.FindControl("Chk_Exam");
                    TextBox txt_Cumulative = (TextBox)gv.FindControl("Txt_Cumulative");
                    if (Chk_Exam.Checked == true)
                    {
                        if (txt_Cumulative.Text != "")
                        {
                            if (gv.Cells[2].Text.ToString() == "0")
                            {
                                _IndCount++;
                                _ExamData[_TotalCount, 0] = gv.Cells[1].Text.ToString();
                                _ExamData[_TotalCount, 1] = gv.Cells[2].Text.ToString();
                                _ExamData[_TotalCount, 2] = gv.Cells[3].Text.ToString();
                                _ExamData[_TotalCount, 3] = (double.Parse(txt_Cumulative.Text)).ToString();
                            }
                            else
                            {
                                _CombinedCount++;
                                _ExamData[_TotalCount, 0] = gv.Cells[1].Text.ToString();
                                _ExamData[_TotalCount, 1] = gv.Cells[2].Text.ToString();
                                _ExamData[_TotalCount, 2] = gv.Cells[3].Text.ToString();
                                _ExamData[_TotalCount, 3] = (double.Parse(txt_Cumulative.Text)).ToString();
                            }
                            _CumulativeTotal = _CumulativeTotal + double.Parse(txt_Cumulative.Text);
                            _TotalCount++;
                        }
                        else
                        {
                            Lbl_Message.Text = "Enter Cumulative Values";
                            _Valid = false;
                            break;
                        }
                    }
                }
                
                    if (_TotalCount > 0)
                    {
                        if (_CumulativeTotal == 100)
                        {
                            _ExamScheduleId = new string[_TotalCount, 4];
                            for (int i = 0; i < _TotalCount; i++)
                            {
                                _ExamScheduleId[i, 0] = _ExamData[i, 0].ToString();
                                _ExamScheduleId[i, 1] = _ExamData[i, 1].ToString();
                                _ExamScheduleId[i, 2] = _ExamData[i, 2].ToString();
                                _ExamScheduleId[i, 3] = _ExamData[i, 3].ToString();
                            }
                            _Valid = true;
                        }
                        else
                        {
                            Lbl_Message.Text = "Cumulative Sum Should be 100";
                            _Valid = false;
                        }                        
                    }
                    else
                    {
                        Lbl_Message.Text = "Please Select atleast one Exam";
                        _Valid = false;
                    }
            }
            catch
            {
                Lbl_Message.Text = "Enter Valid Datas";
            }
            
            return _Valid;
        }        

    }
}
