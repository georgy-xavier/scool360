using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WinBase;
using System.Data.Odbc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System.Diagnostics;
using PdfSharp.Pdf;
using MigraDoc;
using MigraDoc.DocumentObjectModel.Tables;
using System.Collections;

namespace WinEr
{

    public class ExamICSEGridReportClass
    {

        public MysqlClass m_MysqlDb;
        public ExamNode[] m_Eaxams;
        public ExamNode[] m_Subjects;
        public MarkNode[,] m_ExamMarks;

        public MarkNode[] m_TotalMarks;
        public MarkNode[] m_Ranks;
        public StudentDetails m_StudDetails;

        public int m_StudentId;
        public string[] m_Grade;
        public double[] m_Average;
        public double[] m_MaxTotal;
        public string[] m_Remark;



        public ExamNode[] m_T_Eaxams;
        public ExamNode[] m_T_Subjects;
        public MarkNode[,] m_T_ExamMarks;

        public MarkNode[] m_T_TotalMarks;
        public MarkNode[] m_T_Ranks;
        public StudentDetails m_T_StudDetails;

        public string[] m_T_Grade;
        public double[] m_T_Average;
        public double[] m_T_MaxTotal;




        public ExamICSEGridReportClass(MysqlClass _Mysqlobj, int _StudentId)
        {
            m_MysqlDb = _Mysqlobj;
            m_StudentId = _StudentId;

        }

        public bool ExamDetails(ExamNode[] _Eaxams, ExamNode[] m_Subjects, DataRow _StudDetails, string _ExamType, int StudentId)
        {
            if (_ExamType == "Main")
            {


                m_ExamMarks = new MarkNode[_Eaxams.Length, m_Subjects.Length];

                m_TotalMarks = new MarkNode[_Eaxams.Length];
                m_Ranks = new MarkNode[_Eaxams.Length];
                m_Grade = new string[_Eaxams.Length];
                m_Average = new double[_Eaxams.Length];
                m_MaxTotal = new double[_Eaxams.Length];
                m_Remark = new string[_Eaxams.Length];

                m_StudDetails.Id = int.Parse(_StudDetails[0].ToString());
                m_StudDetails.Name = _StudDetails[1].ToString();
                m_StudDetails.RollNum = int.Parse(_StudDetails[2].ToString());
                m_StudDetails.Class = _StudDetails[5].ToString();
                m_StudDetails.Strength = double.Parse(_StudDetails[7].ToString());
                m_StudDetails.DOB = _StudDetails[4].ToString();
                m_StudDetails.AdmissionNum = _StudDetails[6].ToString();
                m_StudDetails.Attendance = _StudDetails[3].ToString();

            }
          
            else if (_ExamType == "Total")
            {
                m_T_ExamMarks = new MarkNode[_Eaxams.Length, m_Subjects.Length];

                m_T_TotalMarks = new MarkNode[_Eaxams.Length];
                m_T_Ranks = new MarkNode[_Eaxams.Length];
                m_T_Grade = new string[_Eaxams.Length];
                m_T_Average = new double[_Eaxams.Length];
                m_T_MaxTotal = new double[_Eaxams.Length];
                m_T_StudDetails.Id = int.Parse(_StudDetails[0].ToString());
                m_T_StudDetails.Name = _StudDetails[1].ToString();
                m_T_StudDetails.RollNum = int.Parse(_StudDetails[2].ToString());
                m_T_StudDetails.Class = _StudDetails[3].ToString();
            }


            for (int i = 0; i < _Eaxams.Length; i++)
            {

                string _sqlMarksColum = "select tblexammark.MarkColumn, tblexammark.SubjectId  from tblexammark where tblexammark.ExamSchId=" + _Eaxams[i].Id + " ORDER BY tblexammark.SubjectOrder";
                DataSet ColumnDetails = m_MysqlDb.ExecuteQueryReturnDataSet(_sqlMarksColum);
                string sqlMark = "";

                if (ColumnDetails != null && ColumnDetails.Tables[0].Rows.Count > 0)
                {
                    sqlMark = "Select ";

                    foreach (DataRow dr in ColumnDetails.Tables[0].Rows)
                    {
                        sqlMark = sqlMark + dr["MarkColumn"].ToString() + " , ";
                    }

                    sqlMark = sqlMark + "tblstudentmark.TotalMark, tblstudentmark.TotalMax, tblstudentmark.Avg, tblstudentmark.Rank, tblstudentmark.Grade, tblstudentmark.Remark  from  tblstudentmark where  tblstudentmark.ExamSchId=" + _Eaxams[i].Id + " and  tblstudentmark.StudId=" + m_StudentId;
                }
                if (_ExamType == "Main")
                {
                    if (sqlMark != "")
                    {
                        DataSet MarkSet = m_MysqlDb.ExecuteQueryReturnDataSet(sqlMark);

                        if (MarkSet != null && MarkSet.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < m_Subjects.Length; j++)
                            {
                                foreach (DataRow dr in ColumnDetails.Tables[0].Rows)
                                {
                                    if (m_Subjects[j].Id == int.Parse(dr["SubjectId"].ToString()))
                                    {
                                        m_ExamMarks[i, j].Id = _Eaxams[i].Id;
                                        m_ExamMarks[i, j].Name = _Eaxams[i].Name;

                                        string _DbVal = dr["MarkColumn"].ToString();
                                        string Mark = MarkSet.Tables[0].Rows[0][_DbVal].ToString();

                                        m_ExamMarks[i, j].MaxMark = double.Parse(Mark);

                                        break;
                                    }
                                }
                            }

                            m_TotalMarks[i].MaxMark = double.Parse(MarkSet.Tables[0].Rows[0]["TotalMark"].ToString());
                            m_Ranks[i].MaxMark = Math.Round(double.Parse(MarkSet.Tables[0].Rows[0]["Rank"].ToString()));
                            m_Grade[i] = MarkSet.Tables[0].Rows[0]["Grade"].ToString();
                            m_Average[i] = double.Parse(MarkSet.Tables[0].Rows[0]["Avg"].ToString());
                            m_MaxTotal[i] = double.Parse(MarkSet.Tables[0].Rows[0]["TotalMax"].ToString());
                            m_Remark[i] = MarkSet.Tables[0].Rows[0]["Remark"].ToString();
                        }
                        else
                        {
                            m_TotalMarks[i].MaxMark = 0;
                            m_Ranks[i].MaxMark = 0;
                            m_Grade[i] = "";
                            m_Average[i] = 0;
                            m_MaxTotal[i] = 0;
                            m_Remark[i] = "";
                        }
                    }
                }
               
                
                else if (_ExamType == "Total")
                {

                    if (sqlMark != "")
                    {
                        int temp = 0;
                        DataSet MarkSet = m_MysqlDb.ExecuteQueryReturnDataSet(sqlMark);

                        if (MarkSet != null && MarkSet.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < m_Subjects.Length; j++)
                            {
                                temp = 0;
                                foreach (DataRow dr in ColumnDetails.Tables[0].Rows)
                                {
                                    if (m_Subjects[j].Id == int.Parse(dr["SubjectId"].ToString()))
                                    {
                                        m_T_ExamMarks[i, j].Id = _Eaxams[i].Id;
                                        m_T_ExamMarks[i, j].Name = _Eaxams[i].Name;

                                        string _DbVal = dr["MarkColumn"].ToString();
                                        string Mark = MarkSet.Tables[0].Rows[0][_DbVal].ToString();

                                        m_T_ExamMarks[i, j].MaxMark = double.Parse(Mark);
                                        temp = 1;
                                        break;
                                    }
                                }
                                if(temp==0){
                                    m_T_ExamMarks[i, j].MaxMark = -1;
                                }
                            }

                            m_T_TotalMarks[i].MaxMark = double.Parse(MarkSet.Tables[0].Rows[0]["TotalMark"].ToString());
                            m_T_Ranks[i].MaxMark = Math.Round(double.Parse(MarkSet.Tables[0].Rows[0]["Rank"].ToString()));
                            m_T_Grade[i] = MarkSet.Tables[0].Rows[0]["Grade"].ToString();
                            m_T_Average[i] = double.Parse(MarkSet.Tables[0].Rows[0]["Avg"].ToString());
                            m_T_MaxTotal[i] = double.Parse(MarkSet.Tables[0].Rows[0]["TotalMax"].ToString());
                        }
                        else
                        {
                            m_T_TotalMarks[i].MaxMark = 0;
                            m_T_Ranks[i].MaxMark = 0;
                            m_T_Grade[i] = "";
                            m_T_Average[i] = 0;
                            m_T_MaxTotal[i] = 0;
                        }
                    }



                }

            }

            return true;
        }
    }



    public partial class GradeExamReport : System.Web.UI.Page
    {
        private ExamManage MyExamDetails;
        private KnowinUser MyUser;
        private ClassOrganiser MyClassMang;
        private StudentManagerClass MystudentObj;
        private Attendance MyAttendence;
        private MysqlClass _Mysqlobj;
        private SchoolClass objSchool = null;
        private OdbcDataReader MyReader = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }

            MyUser = (KnowinUser)Session["UserObj"];
            MyExamDetails = MyUser.GetExamObj();
            MyClassMang = MyUser.GetClassObj();
            MystudentObj = MyUser.GetStudentObj();
            MyAttendence = MyUser.GetAttendancetObj();

            if (MyExamDetails == null)
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
                    if (objSchool != null)
                        _Mysqlobj = new MysqlClass(objSchool.ConnectionString);
                }
                else
                {
                    _Mysqlobj = new MysqlClass(WinerUtlity.SingleSchoolConnectionString);
                }


                if (!IsPostBack)
                {
                    LoadInitialValue();
                }

            }
        }

        private void LoadInitialValue()
        {
            LoadClassDetails();
            LoadExamType();
        }

        private void LoadClassDetails()
        {
            DataSet dt_class = MyUser.MyAssociatedClass();

            if (dt_class != null && dt_class.Tables != null && dt_class.Tables[0].Rows.Count > 0)
            {
                drpClass.DataTextField = "ClassName";
                drpClass.DataValueField = "Id";
                drpClass.DataSource = dt_class;
                drpClass.DataBind();

                LoadStudent();
            }
            else
            {
                drpClass.Items.Add(new ListItem("No class Present", "0"));
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
                Drp_ExamType.Items.Add(new ListItem("No exam type found", "-1"));
                Drp_Exam.Enabled = false;
            }
        }

        private void loadExam()
        {
            Drp_Exam.Items.Clear();
            string sql = "select distinct tblexammaster.Id, tblexammaster.ExamName from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster ON tblexammaster.Id= tblclassexam.ExamId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id  where tblclassexam.ClassId = " + int.Parse(drpClass.SelectedValue) + " and tblexamschedule.BatchId = " + MyUser.CurrentBatchId + " and tblexammaster.ExamTypeId= " + int.Parse(Drp_ExamType.SelectedValue) + " and tblexamschedule.status ='Completed'";
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
                Drp_Exam.Items.Add(new ListItem("No exam found", "-1"));
            }
        }

        private void LoadStudent()
        {
            drp_Student.Items.Clear();

            DataSet dt_Student = MyClassMang.GetStudentlistWithStudentIdAndName(int.Parse(drpClass.SelectedValue), MyUser.CurrentBatchId);

            if (dt_Student != null && dt_Student.Tables != null && dt_Student.Tables[0].Rows.Count > 0)
            {
                drp_Student.Items.Add(new ListItem("All", "0"));
                foreach (DataRow dr in dt_Student.Tables[0].Rows)
                {
                    drp_Student.Items.Add(new ListItem(dr["StudentName"].ToString(),dr["Id"].ToString()));
                }
               
                btnSelect.Enabled = true;
            }
            else
            {
                btnSelect.Enabled = false;
                drp_Student.Items.Add(new ListItem("No Students Present", "0"));

            }
        }
        protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudent();
            loadExam();
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            string Msg = "";
            if (ValidData(out Msg))
            {
                LoadReport(int.Parse(drpClass.SelectedValue), int.Parse(Drp_Exam.SelectedValue), int.Parse(drp_Student.SelectedValue));
            }
            else
            {
                lblMessage.Text = Msg;
            }
        }

        protected void Drp_ExamType_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadExam();
        }

        private bool ValidData(out string _msg)
        {
            bool _Valid = true;
            _msg = "";

            if (drpClass.SelectedValue == "-1")
            {
                _msg = "Class does not found";
                _Valid = false;
            }
            else if (Drp_ExamType.SelectedValue == "-1")
            {
                _msg = "Exam type does not found";
                _Valid = false;
            }
            else if (Drp_ExamType.SelectedValue == "0")
            {
                _msg = "Select exam type";
                _Valid = false;
            }
            else if (Drp_Exam.SelectedValue == "-1")
            {
                _msg = "Exam  does not found";
                _Valid = false;
            }
            else if (Drp_Exam.SelectedValue == "0")
            {
                _msg = "Select one exam ";
                _Valid = false;
            }
            else if (drp_Student.SelectedValue == "-1")
            {
                _msg = "Student does not found ";
                _Valid = false;
            }
            else if (Drp_Exam.SelectedValue != "-1" && drp_Student.SelectedValue != "-1" && drp_Student.SelectedValue != "0")
            {
                string sql = "select tblstudentmark.StudId from tblstudentmark inner join tblexamschedule on tblexamschedule.Id = tblstudentmark.ExamSchId inner join  tblclassexam on tblclassexam.Id=  tblexamschedule.ClassExamId where tblclassexam.ClassId=" + drpClass.SelectedValue + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblclassexam.ExamId= " + Drp_Exam.SelectedValue + " and tblexamschedule.Status='Completed' and tblstudentmark.StudId=" + drp_Student.SelectedValue;
                MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
                if (!MyReader.HasRows)
                {
                    _msg = "Selected student does not attend this exam. Please select another studnet.";
                    _Valid = false;
                }
            }



            return _Valid;
        }

        private void LoadReport(int ClassId, int ExamId, int StudentId)
        {
            DataSet StudList = new DataSet();
            ExamNode[] _ExamDetails;
            ExamNode[] _SubDetails;
            ExamNode[] Total_SubDetails;
            ExamNode[] _SubDetailsPassMark;
            ExamNode[] _SubjectWiseClassPersetage;
            ExamNode[] _ClassHeighest;
            ExamNode[,] _ClassMarks;


            ExamNode[] Total_ExamDetails = null;


            DataSet TopStudentList = new DataSet();
            List<ExamICSEGridReportClass> ClsObj = new List<ExamICSEGridReportClass>();
            ExamICSEGridReportClass _ReportObj;

            try
            {


                StudList = GetStudDetails(StudentId, ClassId, ExamId);
                GetExamDetails(ClassId, ExamId, out _ExamDetails);
                GetSubDetails(ClassId, _ExamDetails[_ExamDetails.Length-1].Id, out _SubDetails);

                GetTotalExamDetails(ClassId, ExamId, out Total_ExamDetails);
                
                Total_SubDetails = null;

                if (Total_ExamDetails != null)
                {
                    getMaximumSubject(ClassId, Total_ExamDetails, out Total_SubDetails);

                }

                _ClassMarks = new ExamNode[StudList.Tables[0].Rows.Count, _SubDetails.Length];

                foreach (DataRow Dr in StudList.Tables[0].Rows)
                {

                    _ReportObj = new ExamICSEGridReportClass(_Mysqlobj, int.Parse(Dr[0].ToString()));
                    _ReportObj.ExamDetails(_ExamDetails, _SubDetails, Dr, "Main", 0);

                    if (Total_ExamDetails != null)
                        _ReportObj.ExamDetails(Total_ExamDetails, Total_SubDetails, Dr, "Total", 0);
                  


                    ClsObj.Add(_ReportObj);
                }

                CreateReport(ClsObj, _ExamDetails, _SubDetails, Total_ExamDetails, Total_SubDetails, StudentId);

            }

            catch (Exception _error)
            {

                lblMessage.Text ="Error in report generation; Error details: "+ _error.Message;
            
            }
        }



        private void CreateReport(List<ExamICSEGridReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, ExamNode[] Total_ExamDetails, ExamNode[] Total_SubDetails, int StudentId)
        {
            string _PhysicalPath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));
            Document document = LoadPDFPage(ClsObj, _ExamDetails, _SubDetails, _PhysicalPath,  Total_ExamDetails,   Total_SubDetails, StudentId);
            //    {CreateDocument();
            const PdfFontEmbedding embedding = PdfFontEmbedding.Always;

            // ----------------------------------------------------------------------------------------

            const bool unicode = false;

            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF


            pdfRenderer.RenderDocument();

            // Save the document...
            string filename = "", MainName = "";

            if (StudentId == 0)
                MainName = ClsObj[0].m_StudDetails.Class;
            else
                MainName = "Stud_Id_" + StudentId.ToString();

            filename = _PhysicalPath + "\\PDF_Files\\ICSEGrade_Class_" + MainName + ".pdf";

            pdfRenderer.PdfDocument.Save(filename);
            // ...and start a viewer.

            //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=Grade_Class_" + MainName + ".pdf\");", true);
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "null", "window.open(\"OpenPdfPage.aspx?PdfName=ICSEGrade_Class_" + MainName + ".pdf\",'Info','status=1, width=1000, height=700,,resizable=1,scrollbars=1');", true);

              
        }

        private Document LoadPDFPage(List<ExamICSEGridReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, string _PhysicalPath, ExamNode[] Total_ExamDetails, ExamNode[] Total_SubDetails, int StudentId)
        {
            Document document = new Document();

            string _ExamName = _ExamDetails[_ExamDetails.Length - 1].Name;

            for (int i = 0; i < ClsObj.Count; i++)
            {

                if (StudentId != 0 && ClsObj[i].m_StudentId == StudentId || StudentId == 0)
                {
                    PageSetup PgSt = document.DefaultPageSetup;

                    PgSt.LeftMargin = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2);
                    PgSt.RightMargin = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2); 
                    PgSt.TopMargin = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(4); 
                    PgSt.BottomMargin = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1);

                    PgSt.HeaderDistance = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(0);
                    PgSt.FooterDistance = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(0); 
                    PgSt.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.A4;

                    // Add a section to the document
                    Section section = document.AddSection();

                    MigraDoc.DocumentObjectModel.Font font = new MigraDoc.DocumentObjectModel.Font("Times New Roman", 16);
                    MigraDoc.DocumentObjectModel.Font subfont = new MigraDoc.DocumentObjectModel.Font("Times New Roman", 11);

                    MigraDoc.DocumentObjectModel.Font fontMain = new MigraDoc.DocumentObjectModel.Font("Times New Roman", 11);
                    MigraDoc.DocumentObjectModel.Font fontMain2 = new MigraDoc.DocumentObjectModel.Font("Times New Roman", 11);
                    MigraDoc.DocumentObjectModel.Font fontMain3 = new MigraDoc.DocumentObjectModel.Font("Times New Roman", 11);
                    MigraDoc.DocumentObjectModel.Font font2 = new MigraDoc.DocumentObjectModel.Font("Times New Roman", 11);


                    Paragraph Top = section.AddParagraph();

                    Top.Format.Alignment = ParagraphAlignment.Left;

                    //if (_Header != 0)
                    //    Top.AddImage("head.JPG");
                    //document.LastSection.AddParagraph("St. FRANCIS");
                    //else
                  //  Top.AddFormattedText("\n\n\n\n\n\n\n\n");

                    Paragraph paragraph = section.AddParagraph();


                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                    paragraph.Format.Font = font;
                    paragraph.Format.Font.Bold = true;
                    paragraph.AddFormattedText(_ExamName);


                    MigraDoc.DocumentObjectModel.Tables.Table table = new MigraDoc.DocumentObjectModel.Tables.Table();

                    table.Format.Font = fontMain;

                    Column column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));


                    column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(5));
                    column.Format.Font.Bold = true;
                    column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1));

                    column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(4));
                    column.Format.Font.Bold = false;
                    column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(5));
                    column.Format.Font.Bold = true;


                    Row row = table.AddRow();
                    row.Height = 15;
                    //row.Format.Font = fontMain;
                    Cell cell = row.Cells[0];
                    // cell.Format.Font.Bold = false;
                    cell.AddParagraph("Name");
                    cell = row.Cells[1];
                    //cell.Format.Font.Bold = true;
                    cell.AddParagraph(": " + ClsObj[i].m_StudDetails.Name + "");
                    cell = row.Cells[2];
                    cell.AddParagraph("");
                    cell = row.Cells[3];
                    // cell.Format.Font.Bold = false;
                    cell.AddParagraph("Roll Number");
                    cell = row.Cells[4];
                    // cell.Format.Font.Bold = true;
                    cell.AddParagraph(": " + ClsObj[i].m_StudDetails.RollNum + "");

                    row = table.AddRow();
                    row.Height = 20;
                    cell = row.Cells[0];
                    //cell.Format.Font.Bold = false;
                    cell.AddParagraph("Class and Section");
                    cell = row.Cells[1];
                    // cell.Format.Font.Bold = true;
                    cell.AddParagraph(": " + ClsObj[i].m_StudDetails.Class + "");
                    cell = row.Cells[2];
                    cell.AddParagraph("");
                    cell = row.Cells[3];
                    // cell.Format.Font.Bold = false;
                    cell.AddParagraph("Admission Number");
                    cell = row.Cells[4];
                    // cell.Format.Font.Bold = true;
                    cell.AddParagraph(": " + ClsObj[i].m_StudDetails.AdmissionNum + "");

                    row = table.AddRow();
                    row.Height = 20;
                    cell = row.Cells[0];
                    // cell.Format.Font.Bold = false;
                    cell.AddParagraph("Date of Birth");
                    cell = row.Cells[1];
                    //  cell.Format.Font.Bold = true;
                    cell.AddParagraph(": " + ClsObj[i].m_StudDetails.DOB + "");
                    cell = row.Cells[2];
                    cell.AddParagraph("");
                    cell = row.Cells[3];
                    //  cell.Format.Font.Bold = false;
                    cell.AddParagraph("Attendance");
                    cell = row.Cells[4];
                    //  cell.Format.Font.Bold = true;
                    cell.AddParagraph(": " + ClsObj[i].m_StudDetails.Attendance + "");



                    table.SetEdge(0, 0, 5, 3, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.5, Colors.Black);


                    document.LastSection.Add(table);

                    Paragraph space = section.AddParagraph();


                    space.AddFormattedText("\n");

                    MigraDoc.DocumentObjectModel.Tables.Table _BaseTable = GetTableFormant();
                    MigraDoc.DocumentObjectModel.Tables.Table TableMain = new MigraDoc.DocumentObjectModel.Tables.Table();


                    TableMain = GetMainReportDetails(ClsObj[i].m_ExamMarks, _ExamDetails, _SubDetails, ClsObj[i].m_Average, ClsObj[i].m_TotalMarks, ClsObj[i].m_Ranks, ClsObj[i].m_Grade, ClsObj[i].m_MaxTotal, ClsObj, ClsObj[i].m_StudentId, _BaseTable);
                    document.LastSection.Add(TableMain);

                    MigraDoc.DocumentObjectModel.Tables.Table TotalExam = GetTotalExam(ClsObj[i].m_T_ExamMarks, Total_ExamDetails,  Total_SubDetails);
                    document.LastSection.Add(TotalExam);

                    Paragraph bottom = section.AddParagraph();
                    bottom.Format.Font = fontMain3;

                    bottom.Format.Alignment = ParagraphAlignment.Left;
                    bottom.AddFormattedText("Teacher's Remark");
                    bottom.AddFormattedText("\n\n");
                    bottom.AddFormattedText("\n\n");
                    if (ClsObj[i].m_Remark[_ExamDetails.Length - 1].ToString() != null)
                    {
                        bottom.AddFormattedText(ClsObj[i].m_Remark[_ExamDetails.Length - 1].ToString());
                    }
                    else
                    {
                        bottom.AddFormattedText("\n");
                    }
                    bottom.AddFormattedText("\n\n");
                    bottom.AddFormattedText("\n\n");

                    bottom.AddFormattedText("Signature: Class Teacher\t\t\tPrincipal\t\t\tParent");
                }


            }
            return document;
        }

   
        private MigraDoc.DocumentObjectModel.Tables.Table GetMainReportDetails(MarkNode[,] markNode, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, double[] _Average, MarkNode[] _TotalMarks, MarkNode[] _Ranks, string[] _Grade, double[] _MaxTotal, List<ExamICSEGridReportClass> ClsObj, int _StudentId, MigraDoc.DocumentObjectModel.Tables.Table StartTable)
        {

            MigraDoc.DocumentObjectModel.Tables.Table _MarkDetails = StartTable;

            Row row; Cell cell;

            row = _MarkDetails.AddRow();

            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            cell = row.Cells[0];

            cell.AddParagraph("Sl. No");
            cell = row.Cells[1];
            cell.AddParagraph("Subject");
            cell = row.Cells[2];
        
            cell.AddParagraph("Grade Obtained");
          



            row = _MarkDetails.AddRow();
            row.Height = 150;
            row.Format.Alignment = ParagraphAlignment.Left;
            cell = row.Cells[0];
            string subName = "";
            int temp;
            DataSet Grade = new DataSet();
            int gradeId = 0;
            double per = 0;
            string grade = "";

            gradeId = MyExamDetails.GetGradeMaster(_ExamDetails[_ExamDetails.Length - 1].Id);
            Grade = MyExamDetails.GetGradeDataSet(gradeId);

            for (int Subjct = 0; Subjct < _SubDetails.Length; Subjct++)
            {
                 temp = 0;
                cell = row.Cells[0];
                cell.Format.Alignment = ParagraphAlignment.Center;
                int i = Subjct + 1;
                cell.AddParagraph(i.ToString() + "\n");
                cell = row.Cells[1];
              
                if (_SubDetails[Subjct].Name.Length > 24)
                    subName = _SubDetails[Subjct].Name.Substring(0, 21) + "..";
                else
                    subName = _SubDetails[Subjct].Name;
                cell.AddParagraph(subName + "\n");
             
                cell = row.Cells[2];
                cell.Format.Alignment = ParagraphAlignment.Center;
                
            
                if (markNode[_ExamDetails.Length - 1, Subjct].MaxMark.ToString() == "0" || markNode[_ExamDetails.Length - 1, Subjct].MaxMark.ToString() == "-1")
                {
                    cell.AddParagraph("Absent" + "\n");
                    temp = 1;
                }

                else
                {
                    if (Grade != null && Grade.Tables != null && Grade.Tables[0].Rows.Count > 0)
                    {
                        cell.AddParagraph(GetGradeFromMarks(Grade, double.Parse(markNode[_ExamDetails.Length - 1, Subjct].MaxMark.ToString()),_SubDetails[Subjct].MaxMark)).ToString();
                    }
                }

              

            }
            //double _TotalPassMark = 0;
            //for (int j = 0; j < _SubDetailsPassMark.Length; j++)
            //    _TotalPassMark = _TotalPassMark + _SubDetailsPassMark[j].MaxMark;

            //row = _MarkDetails.AddRow();
            //row.Height = 25;
            //cell = row.Cells[0];
            //cell.AddParagraph("");
            //cell = row.Cells[1];
            //cell.Format.Alignment = ParagraphAlignment.Center;
            //cell.VerticalAlignment = VerticalAlignment.Center;
            //cell.AddParagraph("Total");

            //cell = row.Cells[2];
            //cell.Format.Alignment = ParagraphAlignment.Center;
            //cell.AddParagraph(_MaxTotal[_ExamDetails.Length - 1].ToString());
            //cell = row.Cells[3];
            //cell.Format.Alignment = ParagraphAlignment.Center;
            //cell.AddParagraph(_TotalPassMark.ToString());
            //cell = row.Cells[4];
            //cell.Format.Alignment = ParagraphAlignment.Center;
            //cell.AddParagraph(_TotalMarks[_ExamDetails.Length - 1].MaxMark.ToString());
            //cell = row.Cells[5];
            //cell.Format.Alignment = ParagraphAlignment.Center;
            //cell.MergeRight = 2;

            //cell.VerticalAlignment = VerticalAlignment.Center;
            //cell.AddParagraph("Percentage :" + Math.Round(_Average[_ExamDetails.Length - 1]).ToString());

            return _MarkDetails;
        }

        private MigraDoc.DocumentObjectModel.Tables.Table GetTotalExam(MarkNode[,] markNode, ExamNode[] Total_ExamDetails, ExamNode[] _SubDetails)
        {

            int totalSubCOunt = _SubDetails.Length;

            MigraDoc.DocumentObjectModel.Font fontMain2 = new MigraDoc.DocumentObjectModel.Font("Times New Roman", 10);
            MigraDoc.DocumentObjectModel.Tables.Table table3 = new MigraDoc.DocumentObjectModel.Tables.Table();
            table3.Format.Font = fontMain2;
            double[] avgMarks, MaxMark;
            int[] TotalExam;

            table3.Borders.Width = 0.5;
            Column column3;

            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(.8));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2.7));
           double temp_totalSubCOunt=11;
           if (totalSubCOunt > 11)
               temp_totalSubCOunt = totalSubCOunt;
           double totalColumnsize = (14.5 / (temp_totalSubCOunt));

           for (int c = 0; c < temp_totalSubCOunt ; c++)
            {
                column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(totalColumnsize));
               
            }



            Row row1 = table3.AddRow();



            Cell cell1 = row1.Cells[0];


            cell1.MergeRight = 12;
            row1.Height = 30;
            cell1.Format.Alignment = ParagraphAlignment.Center;
            cell1.VerticalAlignment = VerticalAlignment.Center;
            cell1.Format.Font.Bold = true;
            cell1.AddParagraph("PERFORMANCE");

            row1 = table3.AddRow();
            row1.Format.Alignment = ParagraphAlignment.Center;
            row1.Format.Font.Bold = true;
            cell1 = row1.Cells[0];
            cell1.Format.Alignment = ParagraphAlignment.Center;
            cell1.AddParagraph("Sl. No");
            cell1 = row1.Cells[1];
            cell1.AddParagraph("Test");

            for (int j = 0; j < _SubDetails.Length; j++)
            {
                cell1 = row1.Cells[2 + j];
                cell1.Format.Alignment = ParagraphAlignment.Center;
                if (_SubDetails[j].SubCode.Length > 4)
                    cell1.AddParagraph(_SubDetails[j].SubCode.Substring(0,4));
                else
                    cell1.AddParagraph(_SubDetails[j].SubCode);
            }


            row1 = table3.AddRow();
            row1.Height = 110;

            avgMarks = new double[_SubDetails.Length];
            TotalExam = new int[_SubDetails.Length];
            MaxMark = new double[_SubDetails.Length];
              
            int gradeId = 0;
            double per = 0;
            string grade = "";
            DataSet Grade;
            

            for (int i = 0; i < Total_ExamDetails.Length; i++)
            {
                gradeId = MyExamDetails.GetGradeMaster(Total_ExamDetails[i].Id);
                Grade = MyExamDetails.GetGradeDataSet(gradeId);
                GetSubDetails(int.Parse(drpClass.SelectedValue.ToString()), Total_ExamDetails[i].Id, out _SubDetails);

                if (i == Total_ExamDetails.Length)
                {
                    //if (Total_ExamDetails.Length != 1 && _AverageReport == 1)
                    //{
                    //    cell1 = row1.Cells[0];

                    //    cell1.AddParagraph("\n\n");
                    //    cell1 = row1.Cells[1];

                    //    cell1.AddParagraph("\n\nAverage");

                    //    for (int j = 0; j < _SubDetails.Length; j++)
                    //    {
                    //        cell1 = row1.Cells[2 + j];
                    //        cell1.Format.Alignment = ParagraphAlignment.Center;
                    //        if (avgMarks[j] == 0 || avgMarks[j] == -1)
                    //            cell1.AddParagraph("\n");
                    //        else
                    //        {
                    //            if (MaxMark[j] != 0)
                    //            {
                    //                avgMarks[j] = (avgMarks[j] / (Total_ExamDetails.Length * 100)) * 100;
                    //            }
                    //            else
                    //            {
                    //                avgMarks[j] = 0;
                    //            }

                    //            string[] _Mrak = avgMarks[j].ToString("0.00").Split('.');

                    //            if (_Mrak[1].ToString() == "00")

                    //                cell1.AddParagraph("\n\n" + Math.Round(avgMarks[j]).ToString());
                    //            else
                    //                cell1.AddParagraph("\n\n" + avgMarks[j].ToString("0.00"));


                    //        }

                    //    }
                    //}
                }
                else
                {
                    cell1 = row1.Cells[0];

                    cell1.AddParagraph((i + 1) + "\n");
                    cell1 = row1.Cells[1];

                    if (Total_ExamDetails[i].Name.Length > 13)
                        cell1.AddParagraph(Total_ExamDetails[i].Name.Substring(0, 13) + "\n");
                    //else if (Total_ExamDetails[i].Name.Length > 11)
                    //    cell1.AddParagraph(Total_ExamDetails[i].Name.Substring(0, 11) + "\n");
                    else
                        cell1.AddParagraph(Total_ExamDetails[i].Name + "\n");

                    int j=0;
                    for (j = 0; j < _SubDetails.Length; j++)
                    {
                        cell1 = row1.Cells[2 + j];
                        //dominic 17-10-2011
                        // Changed the code  if the exam is not found then showing "-" and if the masrk is 0 or -1 then showing "a" means absent
                        if (markNode[i, j].MaxMark == 0 || markNode[i, j].MaxMark == -1)
                        {
                            if (markNode[i, j].Name != null && markNode[i, j].Name != "")
                            {
                                cell1.AddParagraph("a");
                                markNode[i, j].MaxMark = 0;
                            }
                            else
                            {
                                cell1.AddParagraph("-");
                                markNode[i, j].MaxMark = 0;
                            }
                        }
                        else
                        {

                           
                            cell1.AddParagraph(GetGradeFromMarks(Grade, double.Parse(markNode[i, j].MaxMark.ToString()), _SubDetails[j].MaxMark)).ToString();
               
                            //double max_Mark = MyExamDetails.GetMaxMarkFromExamScheduleIdandSubjectId(Total_ExamDetails[i].Id, _SubDetails[j].Id);

                            //double mark = (markNode[i, j].MaxMark / max_Mark) * 100;
                            //if (i != 0)
                            //    MaxMark[j] = MaxMark[j] + max_Mark;
                            //else
                            //    MaxMark[j] = max_Mark;

                            //if (i != 0)
                            //    avgMarks[j] = avgMarks[j] + mark;
                            //else
                            //    avgMarks[j] = mark;
                        }
                    }

                    if (_SubDetails.Length < totalSubCOunt)
                    {
                        for (; j < totalSubCOunt; j++)
                        {
                            cell1 = row1.Cells[2 + j];
                            cell1.AddParagraph("-");

                        }
                    }
                }



            }

            return table3;
        }

        private string GetGradeFromMarks(DataSet Grade, double _Mark, double MaxMark)
        {
            double avg = (_Mark / MaxMark) * 100;

            foreach (DataRow Dr in Grade.Tables[0].Rows)
            {
                if (avg >= double.Parse(Dr[1].ToString()))
                {
                    return Dr[0].ToString();
                }
            }
            return "";
        }

        private MigraDoc.DocumentObjectModel.Tables.Table GetTableFormant()
        {
            MigraDoc.DocumentObjectModel.Font font1 = new MigraDoc.DocumentObjectModel.Font("Times New Roman", 11);
            Column column;
            MigraDoc.DocumentObjectModel.Tables.Table _MarkDetails = new MigraDoc.DocumentObjectModel.Tables.Table();
            _MarkDetails.Rows.Height = 15;

            _MarkDetails.Format.Font = font1;
            _MarkDetails.Borders.Width = 0.5;

            column = _MarkDetails.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1));
            column = _MarkDetails.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(15));
            column = _MarkDetails.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2));
          
            return _MarkDetails;
        }

        private void GetTotalExamDetails(int ClassId, int ExamId, out ExamNode[] _ExamDetails)
        {

            string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period from tblexammaster inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id     where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark)  and tblexamschedule.Status='Completed' and tblexammaster.ExamTypeId=1  order by tblexamschedule.id ASC";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                int i = 0;
                int Count = MyReader.RecordsAffected;
                _ExamDetails = new ExamNode[Count];

                while (MyReader.Read())
                {
                    _ExamDetails[i].Id = int.Parse(MyReader.GetValue(0).ToString());
                    _ExamDetails[i].Name = MyReader.GetValue(1).ToString();
                    string sql1 = "select Max( tblexammark.ExamDate) from tblexammark where tblexammark.ExamSchId=" + int.Parse(MyReader.GetValue(0).ToString());
                    OdbcDataReader dr = MyClassMang.m_MysqlDb.ExecuteQuery(sql1);
                    if (dr.HasRows)
                    {

                        _ExamDetails[i].Date = DateTime.Parse(dr.GetValue(0).ToString());
                    }

                    i++;
                }
            }
            else
            {
                _ExamDetails = new ExamNode[0];
            }
        }

        private void GetSubDetails(int ClassId, int ExamId, out ExamNode[] _SubDetails)
        {
            //string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=tblexamschedule.ClassExamId and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId  where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblclassexam.ClassId=" + ClassId + " and tblexammaster.Id=" + ExamId;
            string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark ,tblsubjects.SubjectCode,tblexammark.SubjectOrder  from tblclassexamsubmap inner JOIN tblsubjects on tblclassexamsubmap.SubId= tblsubjects.Id inner JOIN tblclassexam on tblclassexam.id= tblclassexamsubmap.ClassExamId    inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.id  inner JOIN tblexammark on  tblexammark.SubjectId=tblsubjects.Id and tblexammark.ExamSchId =tblexamschedule.id   where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "  and tblclassexam.ClassId=" + ClassId + " and tblexamschedule.Id=" + ExamId + " order by tblexammark.SubjectOrder";

            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                int i = 0;
                int Count = MyReader.RecordsAffected;
                _SubDetails = new ExamNode[Count];

                while (MyReader.Read())
                {
                    _SubDetails[i].Id = int.Parse(MyReader.GetValue(0).ToString());
                    _SubDetails[i].Name = MyReader.GetValue(1).ToString();
                    _SubDetails[i].MaxMark = double.Parse(MyReader.GetValue(2).ToString());
                    _SubDetails[i].SubCode = MyReader.GetValue(3).ToString();
                    i++;
                }
            }
            else
            {
                _SubDetails = new ExamNode[0];
            }

        }

        private void getMaximumSubject(int ClassId, ExamNode[] Total_ExamDetails, out  ExamNode[] Total_SubDetails)
        {
            Total_SubDetails = new ExamNode[0];
            int temp = 0;
            for (int j = 0; j < Total_ExamDetails.Length; j++)
            {
                //string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=tblexamschedule.ClassExamId and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId  where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblclassexam.ClassId=" + ClassId + " and tblexammaster.Id=" + ExamId;
                string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark ,tblsubjects.SubjectCode,tblexammark.SubjectOrder  from tblclassexamsubmap inner JOIN tblsubjects on tblclassexamsubmap.SubId= tblsubjects.Id inner JOIN tblclassexam on tblclassexam.id= tblclassexamsubmap.ClassExamId    inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.id  inner JOIN tblexammark on  tblexammark.SubjectId=tblsubjects.Id and tblexammark.ExamSchId =tblexamschedule.id   where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "  and tblclassexam.ClassId=" + ClassId + " and tblexamschedule.Id=" + Total_ExamDetails[j].Id + " order by tblexammark.SubjectOrder";

                MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);

                if (MyReader.HasRows)
                {
                    int i = 0;
                    int Count = MyReader.RecordsAffected;

                    if (temp == 0 || (temp > 0 && temp < Count))
                    {
                        Total_SubDetails = new ExamNode[Count];

                        while (MyReader.Read())
                        {
                            Total_SubDetails[i].Id = int.Parse(MyReader.GetValue(0).ToString());
                            Total_SubDetails[i].Name = MyReader.GetValue(1).ToString();
                            Total_SubDetails[i].MaxMark = double.Parse(MyReader.GetValue(2).ToString());
                            Total_SubDetails[i].SubCode = MyReader.GetValue(3).ToString();
                            i++;
                        }
                        temp = Count;
                    }
                }
                else
                {
                    Total_SubDetails = new ExamNode[0];
                }
            }
        }


        private void GetExamDetails(int ClassId, int ExamId, out ExamNode[] _ExamDetails)
        {

            string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period, tblexamschedule.GradeMasterId from tblexammaster inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id     where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblexammaster.Id=" + ExamId + " and tblexamschedule.Status='Completed'  order by tblexamschedule.id ASC";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                int i = 0;
                int Count = MyReader.RecordsAffected;
                _ExamDetails = new ExamNode[Count];

                while (MyReader.Read())
                {
                    _ExamDetails[i].Id = int.Parse(MyReader.GetValue(0).ToString());
                    _ExamDetails[i].Name = MyReader.GetValue(1).ToString();
                    _ExamDetails[i].GradeMasterId = int.Parse(MyReader.GetValue(3).ToString());
                    string sql1 = "select Max( tblexammark.ExamDate) from tblexammark where tblexammark.ExamSchId=" + int.Parse(MyReader.GetValue(0).ToString());
                    OdbcDataReader dr = MyClassMang.m_MysqlDb.ExecuteQuery(sql1);

                    if (dr.HasRows)
                    {

                        _ExamDetails[i].Date = DateTime.Parse(dr.GetValue(0).ToString());
                    }

                    i++;
                }
            }
            else
            {
                _ExamDetails = new ExamNode[0];
            }
        }


        private DataSet GetStudDetails(int StudentId, int ClassId, int ExamId)
        {

            MigraDoc.DocumentObjectModel.Font subfont = new MigraDoc.DocumentObjectModel.Font("Times New Roman", 11);

            int count = 0;
            DataSet _StdentSet = new DataSet();
            DataTable dt;
            DataRow dr;

            _StdentSet.Tables.Add(new DataTable("Student"));

            dt = _StdentSet.Tables["Student"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("RollNum");
            dt.Columns.Add("Attendance");
            dt.Columns.Add("DOB");
            dt.Columns.Add("Class");
            dt.Columns.Add("AdmissionNum");
            dt.Columns.Add("Strength");

            string sql_Student = "select tblview_student.Id, tblview_student.StudentName, tblview_student.RollNo, tblclass.ClassName,date_Format(tblview_student.DOB,'%d/%m/%Y') as DOB , tblview_student.AdmitionNo from tblview_student inner join tblclass on tblclass.Id= tblview_student.ClassId inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblview_student.Id  where tblview_student.ClassId=" + ClassId + " and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblview_student.Id IN( select tblstudentmark.StudId from tblstudentmark inner join tblexamschedule on tblexamschedule.Id = tblstudentmark.ExamSchId inner join  tblclassexam on tblclassexam.Id=  tblexamschedule.ClassExamId where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblclassexam.ExamId= " + ExamId + " and tblexamschedule.Status='Completed'  ) order by tblview_student.RollNo";

            if (StudentId != 0)
                sql_Student = sql_Student + " and tblview_student.Id=" + StudentId;



            DataSet _studList = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql_Student);

            DataSet mydataset = MyClassMang.getStudents(ClassId);
            if (mydataset != null && mydataset.Tables[0].Rows.Count > 0)
                count = mydataset.Tables[0].Rows.Count;

            foreach (DataRow dr_values in _studList.Tables[0].Rows)
            {

                dr = _StdentSet.Tables["Student"].NewRow();


                dr["Id"] = dr_values["Id"];
                dr["Name"] = dr_values["StudentName"];
                dr["RollNum"] = dr_values["RollNo"];
                dr["DOB"] = dr_values["DOB"];
                dr["Class"] = dr_values["ClassName"];
                dr["AdmissionNum"] = dr_values["AdmitionNo"];
                dr["Strength"] = count;
                dr["Attendance"] = 0;


                _StdentSet.Tables["Student"].Rows.Add(dr);

            }
            foreach (DataRow dr_values in _StdentSet.Tables[0].Rows)
            {
                int _no_workingdays = 0;
                int _no_presentdays = 0;
                int _no_absentdays = 0;
                int _no_holidays = 0;
                int _no_halfdays = 0;
                double _attendencepersent = 0;


                MyAttendence.GetCurrentBatchNewattendanceDetails(int.Parse(dr_values["Id"].ToString()), out  _no_workingdays, out  _no_presentdays, out _no_absentdays, out _no_holidays, out _no_halfdays, out _attendencepersent, MyUser.CurrentBatchId);

                dr_values["Attendance"] = _no_presentdays + "/" + _no_workingdays;
            }
            return _StdentSet;

        }
    }



}

