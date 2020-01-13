using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using WinBase;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
//using MigraDoc.RtfRendering;
using PdfSharp.Pdf;
using System.Data;
namespace WinEr
{
    public class CBSCReportCreationClass
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

        public ExamNode[] m_CC_Eaxams;
        public ExamNode[] m_CC_Subjects;
        public MarkNode[,] m_CC_ExamMarks;

        public MarkNode[] m_CC_TotalMarks;
        public MarkNode[] m_CC_Ranks;


        public string[] m_CC_Grade;
        public double[] m_CC_Average;
        public double[] m_CC_MaxTotal;




        public CBSCReportCreationClass(MysqlClass _Mysqlobj, int _StudentId)
        {
            m_MysqlDb = _Mysqlobj;
            m_StudentId = _StudentId;

        }

        public bool ExamDetails(ExamNode[] _Eaxams, ExamNode[] m_Subjects, DataRow _StudDetails, string _ExamType)
        {
            if (_ExamType == "Main")
            {

                m_ExamMarks = new MarkNode[_Eaxams.Length, m_Subjects.Length];
                m_TotalMarks = new MarkNode[_Eaxams.Length];
                m_Ranks = new MarkNode[_Eaxams.Length];
                m_Grade = new string[_Eaxams.Length];
                m_Average = new double[_Eaxams.Length];
                m_MaxTotal = new double[_Eaxams.Length];

                m_StudDetails.Id = int.Parse(_StudDetails[0].ToString());
                m_StudDetails.Name = _StudDetails[1].ToString();
                m_StudDetails.RollNum = int.Parse(_StudDetails[2].ToString());
                m_StudDetails.Class = _StudDetails[4].ToString();

                m_StudDetails.DOB = _StudDetails[3].ToString();
                m_StudDetails.AdmissionNum = _StudDetails[5].ToString();
                m_StudDetails.FatherName = _StudDetails[6].ToString();
                m_StudDetails.MotherName = _StudDetails[7].ToString();
                m_StudDetails.Tel = _StudDetails[8].ToString();
                m_StudDetails.Add = _StudDetails[9].ToString();

               
            }
            else if (_ExamType == "CC")
            {
                m_CC_ExamMarks = new MarkNode[_Eaxams.Length, m_Subjects.Length];

                m_CC_TotalMarks = new MarkNode[_Eaxams.Length];
                m_CC_Ranks = new MarkNode[_Eaxams.Length];
                m_CC_Grade = new string[_Eaxams.Length];
                m_CC_Average = new double[_Eaxams.Length];
                m_CC_MaxTotal = new double[_Eaxams.Length];

            }

            for (int i = 0; i < _Eaxams.Length; i++)
            {

                string _sqlMarksColum = "select tblexammark.MarkColumn, tblexammark.SubjectId  from tblexammark where tblexammark.ExamSchId=" + _Eaxams[i].Id + " order by tblexammark.SubjectOrder";
                DataSet ColumnDetails = m_MysqlDb.ExecuteQueryReturnDataSet(_sqlMarksColum);
                string sqlMark = "";

                if (ColumnDetails != null && ColumnDetails.Tables[0].Rows.Count > 0)
                {
                    sqlMark = "Select ";

                    foreach (DataRow dr in ColumnDetails.Tables[0].Rows)
                    {
                        sqlMark = sqlMark + dr["MarkColumn"].ToString() + " , ";
                    }

                    sqlMark = sqlMark + "tblstudentmark.TotalMark, tblstudentmark.TotalMax, tblstudentmark.Avg, tblstudentmark.Rank, tblstudentmark.Grade from  tblstudentmark where  tblstudentmark.ExamSchId=" + _Eaxams[i].Id + " and  tblstudentmark.StudId=" + m_StudentId;
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
                        }
                        else
                        {
                            m_TotalMarks[i].MaxMark = 0;
                            m_Ranks[i].MaxMark = 0;
                            m_Grade[i] = "";
                            m_Average[i] = 0;
                            m_MaxTotal[i] = 0;
                        }
                    }
                }
                else if (_ExamType == "CC")
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
                                        m_CC_ExamMarks[i, j].Id = _Eaxams[i].Id;
                                        m_CC_ExamMarks[i, j].Name = _Eaxams[i].Name;

                                        string _DbVal = dr["MarkColumn"].ToString();
                                        string Mark = MarkSet.Tables[0].Rows[0][_DbVal].ToString();

                                        m_CC_ExamMarks[i, j].MaxMark = double.Parse(Mark);

                                        break;
                                    }
                                }
                            }

                            m_CC_TotalMarks[i].MaxMark = double.Parse(MarkSet.Tables[0].Rows[0]["TotalMark"].ToString());
                            m_CC_Ranks[i].MaxMark = Math.Round(double.Parse(MarkSet.Tables[0].Rows[0]["Rank"].ToString()));
                            m_CC_Grade[i] = MarkSet.Tables[0].Rows[0]["Grade"].ToString();
                            m_CC_Average[i] = double.Parse(MarkSet.Tables[0].Rows[0]["Avg"].ToString());
                            m_CC_MaxTotal[i] = double.Parse(MarkSet.Tables[0].Rows[0]["TotalMax"].ToString());
                        }
                        else
                        {
                            m_CC_TotalMarks[i].MaxMark = 0;
                            m_CC_Ranks[i].MaxMark = 0;
                            m_CC_Grade[i] = "";
                            m_CC_Average[i] = 0;
                            m_CC_MaxTotal[i] = 0;
                        }

                    }
                }
            }

            return true;
        }
    }



    public partial class TermReports : System.Web.UI.Page
    {      
        private ExamManage MyExamMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;        
        private MysqlClass _Mysqlobj ;
        private ClassOrganiser MyClassMngr;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
                Response.Redirect("Default.aspx");

            MyUser = (KnowinUser)Session["UserObj"];
            MyExamMang = MyUser.GetExamObj();
            MyClassMngr = MyUser.GetClassObj();
            
            if (MyExamMang == null)
                Response.Redirect("RoleErr.htm");
            string _ConnectionString = WinerUtlity.SingleSchoolConnectionString;
            if (WinerUtlity.NeedCentrelDB())
            {
                if (Session[WinerConstants.SessionSchool] == null)
                {
                    Response.Redirect("Logout.aspx");
                }
                objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                _ConnectionString = objSchool.ConnectionString;
            }
            _Mysqlobj = new MysqlClass(_ConnectionString);
            if (!IsPostBack)
                LoadDetails();
        }


        protected void Drp_SelectClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadExamDetailsToDropDown();
            LoadStudentsDetailsToDropDown();
            LoadScholasticArea();
        }


        protected void Drp_SelectExam_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ExamName = "PROGRESS REPORT -";

            if (Drp_SelectExam.SelectedValue != "-1" || Drp_SelectExam.SelectedValue != "0")
            {
                ExamName = ExamName + Drp_SelectExam.SelectedItem.ToString().Replace("CBSE", "");

            }
            else
                ExamName = "";

            Txt_ReportName.Text = ExamName;
        }

        protected void Btn_ExamReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidEntries())
                {

                    CreateReport();
                }

            }
            catch (Exception err)
            {
                Lbl_Err.Text = err.Message;
            }

        }

        private bool ValidEntries()
        {
           
            
            string sql = "";

            if (Drp_SelectClass.SelectedValue == "-1")
            {
                Lbl_Err.Text = "No class found.";
                return false;
            }
            else if (Drp_SelectExam.SelectedValue == "-1")
            {
                Lbl_Err.Text = "No exams found.";
                return false;
            }
            else if (Drp_SelectExam.SelectedValue == "0")
            {
                Lbl_Err.Text = "Select one exam.";
                return false;
            }
          
            else if (Txt_ReportName.Text == "")
            {
                
                    Lbl_Err.Text = "Enter Report Name.";
                    return false;
                
            }
            return true;
        
        }

        private void CreateReport()
        {

            DataSet TotalStudList;
            DataSet totalMarkRatios = null;
            ExamNode[] _ExamDetails;
            ExamNode[] _SubDetails;
            
            SchoolDetails _SchoolDetails;
            ExamNode[] CC_ExamDetails = null;
            ExamNode[] CC_SubDetails = null;
            


            List<CBSCReportCreationClass> ClsObj = new List<CBSCReportCreationClass>();
            CBSCReportCreationClass _ReportObj;


            GetSchoolDetails(out _SchoolDetails);

            GetExamDetails(int.Parse(Drp_SelectClass.SelectedValue), int.Parse(Drp_SelectExam.SelectedValue), out _ExamDetails);
            GetSubDetails(int.Parse(Drp_SelectClass.SelectedValue), int.Parse(Drp_SelectExam.SelectedValue), out _SubDetails);
            if (_ExamDetails != null && _SubDetails != null && _ExamDetails.Length > 0 && _SubDetails.Length > 0)
            {
                TotalStudList = GetAllStudentListFromClass(int.Parse(Drp_SelectStudent.SelectedValue), int.Parse(Drp_SelectClass.SelectedValue));

                GetExamDetailsCC(int.Parse(Drp_SelectClass.SelectedValue), int.Parse(Drp_Scholastic.SelectedValue), out CC_ExamDetails);
                GetSubDetailsCC(int.Parse(Drp_SelectClass.SelectedValue), int.Parse(Drp_Scholastic.SelectedValue), out CC_SubDetails);

                totalMarkRatios = GetMarkRatio(Drp_SelectClass.SelectedValue);


                foreach (DataRow Dr in TotalStudList.Tables[0].Rows)
                {
                    _ReportObj = new CBSCReportCreationClass(_Mysqlobj, int.Parse(Dr[0].ToString()));
                    _ReportObj.ExamDetails(_ExamDetails, _SubDetails, Dr, "Main");
                    _ReportObj.ExamDetails(CC_ExamDetails, CC_SubDetails, Dr, "CC");

                    ClsObj.Add(_ReportObj);
                }


                CreateReport(ClsObj, _ExamDetails, _SubDetails, CC_ExamDetails, CC_SubDetails, _SchoolDetails, int.Parse(Drp_SelectStudent.SelectedValue), totalMarkRatios);
            }
            else
            {
                Lbl_Err.Text = "Exam details not found";
            }
           
        }

        private DataSet GetMarkRatio(string _selectedClassId)
        {
            string stdId = MyClassMngr.GetStandardIdfromClassId(_selectedClassId);
            string sqlRation = "select FA1,FA2,SA1,FA3,FA4,SA2 from tblcbsegraderatio where tblcbsegraderatio.StandardId=" + stdId;
            DataSet Dt = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sqlRation);
            return Dt;

             
        }

        private void CreateReport(List<CBSCReportCreationClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, ExamNode[] CC_ExamDetails, ExamNode[] CC_SubDetails, SchoolDetails _SchoolDetails, int _StudId, DataSet totalMarkRatios)
        {
            string _PhysicalPath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath(""));
            //int _StudentId = 0;
            Document document = LoadPDFPage(ClsObj, _ExamDetails, _SubDetails, _SchoolDetails, CC_ExamDetails,CC_SubDetails,totalMarkRatios);

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

            if (_StudId == 0)
                MainName = Drp_SelectClass.SelectedItem.ToString();
            else
                MainName = Drp_SelectStudent.SelectedItem.ToString();

            filename = _PhysicalPath + "\\PDF_Files\\CBSC_" + MainName + ".pdf";

            pdfRenderer.PdfDocument.Save(filename);
            // ...and start a viewer.
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "null", "window.open(\"OpenPdfPage.aspx?PdfName=CBSC_" + MainName + ".pdf\",'Info','status=1, width=1000, height=700,,resizable=1,scrollbars=1');", true);

            //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=PRC_" + MainName + ".pdf\");", true);
            // Response.Redirect("OpenPdfPage.aspx?PdfName=PRC_" + MainName + ".pdf", false);

        }

        private Document  LoadPDFPage(List<CBSCReportCreationClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, SchoolDetails _SchoolDetails, ExamNode[] CC_ExamDetails, ExamNode[] CC_SubDetails, DataSet totalMarkRatios)
        {
            
            Document document = new Document();

            for (int i = 0; i < ClsObj.Count; i++)
            {

                PageSetup PgSt = document.DefaultPageSetup;
                PgSt.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.A4;
                PgSt.LeftMargin = 0;
                PgSt.RightMargin = 0;
                PgSt.TopMargin = 0;
                PgSt.BottomMargin = 0;

                PgSt.HeaderDistance = 0;
                PgSt.FooterDistance = 0;
                // Add a section to the document

                Section section = document.AddSection();

                // Add a paragraph to the section
                Paragraph paragraph = section.AddParagraph();

                paragraph.AddLineBreak();
                MigraDoc.DocumentObjectModel.Font font = new MigraDoc.DocumentObjectModel.Font("Calibri", 16);
                paragraph.Format.Font = font;
                paragraph.Format.Alignment = ParagraphAlignment.Center;
                paragraph.AddFormattedText(MyUser.SchoolName, TextFormat.Bold);

                paragraph = section.AddParagraph();
                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);
                paragraph.Format.Alignment = ParagraphAlignment.Center;
                paragraph.AddFormattedText(_SchoolDetails.Address, TextFormat.NotBold);
                paragraph.AddFormattedText(" ");

                paragraph = section.AddParagraph();
                //paragraph.AddLineBreak();
                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 14);
                paragraph.Format.Alignment = ParagraphAlignment.Center;
                paragraph.AddFormattedText(Txt_ReportName.Text.ToUpper(), TextFormat.Bold);


                MigraDoc.DocumentObjectModel.Tables.Table table = section.AddTable();

                //PgSt.PageHeight = 100;
                //PgSt.PageWidth = 100; 

                table.Borders.Width = 2;
                //table.Borders.Left.Width = 0.5;
                //table.Borders.Right.Width = 0.5;
                table.Rows.LeftIndent = 25;

                MigraDoc.DocumentObjectModel.Tables.Column Col = table.AddColumn(PgSt.PageWidth - 50);
                MigraDoc.DocumentObjectModel.Tables.Row row = table.AddRow();
                row.Height = PgSt.PageHeight - 100;
                Col.Borders.Visible = true;

                MigraDoc.DocumentObjectModel.Tables.Cell cel = row.Cells[0];

                //paragraph = section.AddParagraph();

                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);
                paragraph.AddFormattedText("STUDENT INFORMATIONS", TextFormat.Underline | TextFormat.Bold);
                paragraph.AddLineBreak();

                MigraDoc.DocumentObjectModel.Tables.Table tb = new MigraDoc.DocumentObjectModel.Tables.Table();


                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                tb.AddColumn((PgSt.PageWidth - 80) / 6);
                tb.AddColumn((PgSt.PageWidth - 80) / 3);
                tb.AddColumn((PgSt.PageWidth - 80) / 6);
                tb.AddColumn((PgSt.PageWidth - 80) / 3);

                row = tb.AddRow();

                row.Cells[0].AddParagraph("Name");
                row.Cells[1].AddParagraph(":" + ClsObj[i].m_StudDetails.Name);
                row.Cells[2].AddParagraph("Father's Name");
                row.Cells[3].AddParagraph(":" + ClsObj[i].m_StudDetails.FatherName);

                row = tb.AddRow();

                row.Cells[0].AddParagraph("Date of Birth");
                row.Cells[1].AddParagraph(":" + ClsObj[i].m_StudDetails.DOB);
                row.Cells[2].AddParagraph("Mother's Name");
                row.Cells[3].AddParagraph(":" + ClsObj[i].m_StudDetails.MotherName );

                row = tb.AddRow();

                row.Cells[0].AddParagraph("Admission No");
                row.Cells[1].AddParagraph(":" + ClsObj[i].m_StudDetails.AdmissionNum);
                row.Cells[2].AddParagraph("Telephone No");
                row.Cells[3].AddParagraph(":" + ClsObj[i].m_StudDetails.Tel);

                row = tb.AddRow();

                row.Cells[0].AddParagraph("Class & Roll No");
                row.Cells[1].AddParagraph(":" + ClsObj[i].m_StudDetails.Class+ ", " + ClsObj[i].m_StudDetails.RollNum);
                row.Cells[2].AddParagraph("Address");
                row.Cells[3].AddParagraph(":" + ClsObj[i].m_StudDetails.Add);

                tb.Borders.Visible = false;
                cel.Elements.Add(tb);

                if (Chk_HealthInfo.Checked)
                {
                    paragraph = cel.AddParagraph();
                    paragraph.Format.Alignment = ParagraphAlignment.Left;
                    paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);
                    paragraph.AddLineBreak();
                    paragraph.AddFormattedText("HEALTH INFORMATIONS", TextFormat.Underline | TextFormat.Bold);
                    paragraph.AddLineBreak();

                    tb = new MigraDoc.DocumentObjectModel.Tables.Table();

                    tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                    tb.AddColumn((PgSt.PageWidth - 80) / 6);
                    tb.AddColumn((PgSt.PageWidth - 80) / 3);
                    tb.AddColumn((PgSt.PageWidth - 80) / 6);
                    tb.AddColumn((PgSt.PageWidth - 80) / 3);

                    row = tb.AddRow();
                    row.Cells[0].AddParagraph("Height");
                    row.Cells[1].AddParagraph(":");
                    row.Cells[2].AddParagraph("Weight");
                    row.Cells[3].AddParagraph(":");
                    row = tb.AddRow();
                    row.Cells[0].AddParagraph("Blood Group");
                    row.Cells[1].AddParagraph(":");
                    row.Cells[2].AddParagraph("Vision");
                    row.Cells[3].AddParagraph(":(L)                    (R)");
                    row = tb.AddRow();
                    row.Cells[0].AddParagraph("Dental Hygiene");
                    row.Cells[1].AddParagraph(":");


                    tb.Borders.Visible = false;
                    cel.Elements.Add(tb);

                   
                }

                if (Chk_Self_Awareness.Checked)
                {


                    // Self awarness
                    paragraph = cel.AddParagraph();

                    paragraph.Format.Alignment = ParagraphAlignment.Left;
                    paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                    paragraph.AddLineBreak();
                    paragraph.AddFormattedText("SELF AWARENESS", TextFormat.Underline | TextFormat.Bold);
                    paragraph.AddLineBreak();

                    tb = new MigraDoc.DocumentObjectModel.Tables.Table();

                    tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                    tb.AddColumn((PgSt.PageWidth - 80) / 6);
                    tb.AddColumn((PgSt.PageWidth - 80) / 3);
                    tb.AddColumn((PgSt.PageWidth - 80) / 6);
                    tb.AddColumn((PgSt.PageWidth - 80) / 3);

                    row = tb.AddRow();

                    row.Cells[0].AddParagraph("Goals");
                    row.Cells[1].AddParagraph(":");
                    row.Cells[2].AddParagraph("Interests/Hobbies");
                    row.Cells[3].AddParagraph(":");

                    row = tb.AddRow();

                    row.Cells[0].AddParagraph("Strengths");
                    row.Cells[1].AddParagraph(":");
                    row.Cells[2].AddParagraph("Responsibilities Discharged / Achievements ");
                    row.Cells[3].AddParagraph(":");

                    tb.Borders.Visible = false;
                    cel.Elements.Add(tb);
                }

                // main exam area

                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);
                paragraph.AddLineBreak();
                paragraph.AddFormattedText("Part 1: Scholastic Area", TextFormat.Underline | TextFormat.Bold);
                paragraph.AddLineBreak();
                tb = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                tb.AddColumn((PgSt.PageWidth - 80) * 4 / 15);
                tb.AddColumn((PgSt.PageWidth - 80) / 15);
                tb.AddColumn((PgSt.PageWidth - 80) / 15);
                tb.AddColumn((PgSt.PageWidth - 80) / 15);
                tb.AddColumn((PgSt.PageWidth - 80) / 15);
                tb.AddColumn((PgSt.PageWidth - 80) / 15);
                tb.AddColumn((PgSt.PageWidth - 80) / 15);
                tb.AddColumn((PgSt.PageWidth - 80) / 15);
                tb.AddColumn((PgSt.PageWidth - 80) / 15);
                tb.AddColumn((PgSt.PageWidth - 80) / 15);
                tb.AddColumn((PgSt.PageWidth - 80) * 2 / 15);

                row = tb.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("1A: Academic Performance", TextFormat.Bold);


                if (_ExamDetails.Length > 0)
                {
                    int TempVal = 0,k=0;
                    for (int j = 0; j < _ExamDetails.Length; j++)
                    { 
                        if (TempVal != 0)
                        {
                            TempVal = 0;
                            k = k + 3;
                        }

                        row.Cells[k + 1].AddParagraph().AddFormattedText(_ExamDetails[j].Name, TextFormat.Bold); //Exam Name
                        row.Cells[k + 1].MergeRight = 2;
                        row.Cells[k + 1].Format.Alignment = ParagraphAlignment.Center;
                        TempVal = 1;

                    }
                   
                    row.Cells[10].AddParagraph().AddFormattedText("Total", TextFormat.Bold);
                    row.Cells[10].MergeDown = 1;
                    row = tb.AddRow();                  

                    TempVal = 0;
                    k = 0;
                    for (int j = 0; j < _ExamDetails.Length; j++)
                    {
                        if (TempVal != 0)
                        {
                            
                            k = k + 3;
                        }
                        row[0].Column[0].MergeDown = 1;

                        row.Cells[k+1].AddParagraph().AddFormattedText("Mark", TextFormat.Bold);
                        row.Cells[k+2].AddParagraph().AddFormattedText("Max", TextFormat.Bold);
                        row.Cells[k+3].AddParagraph().AddFormattedText("Grade", TextFormat.Bold);
                        TempVal = 1;
                    }
                   
                }


                double _TotalMaxMark = 0, _TotalMark = 0, _Sub_Mark = 0, _Sub_Max = 0; 

                double[] _ExamWiseTotalMark= new double[_ExamDetails.Length];
                double[] _ExamWiseSubMark = new double[_ExamDetails.Length];

                DataSet Grade = MyExamMang.GetGradeDataSet(_ExamDetails[_ExamDetails.Length - 1].GradeMasterId);

                // dominic new modificatios
                /* In this report we are using some extra calculations for getting TOTAL
                 * For that i  declared some extra variables like rowSum and columnSum etc
                 */
                double[] rowWiseMarkSum=  new double[_ExamDetails.Length];

                double[] rowWiseMaxSum = new double[_ExamDetails.Length];

                double[] colWiseTotalMarkSum = new double[_ExamDetails.Length];

                double[] colWiseTotalMaxSum = new double[_ExamDetails.Length];

                double TotalRatio = 0;
                double _Ratio = 0;
                for (int Subjct = 0; Subjct < _SubDetails.Length; Subjct++)
                {
                    row = tb.AddRow();
                    row.Cells[0].AddParagraph().AddFormattedText(_SubDetails[Subjct].Name); //Subject Name
                    int TempVal = 0;
                    int k = 0;
                    
                    for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                    {
                        if (TempVal != 0)
                        {
                            k = k + 3;
                        }

                        if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString() != "-1")
                            row.Cells[k + 1].AddParagraph().AddFormattedText(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                        else
                            row.Cells[k + 1].AddParagraph().AddFormattedText("-");
                        double max_Mark = MyExamMang.GetMaxMarkFromExamScheduleIdandSubjectId(_ExamDetails[ExamCount].Id, _SubDetails[Subjct].Id);

                          row.Cells[k + 2].AddParagraph().AddFormattedText(max_Mark .ToString("0"));

                        //grade
                        string _Grade = "";

                        if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString() != "-1")
                            _Grade= GetGradeFromMarks(Grade, double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString()),max_Mark);
                        
                        row.Cells[k + 3].AddParagraph().AddFormattedText(_Grade);

                        // Calculate Examwise total mark with ratios


                        if (_SubDetails[Subjct].MaxMark.ToString("0") != "-1" && ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString() != "-1")
                        {
                           
                            double mark = (double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString()) / max_Mark) * 100;
                            rowWiseMarkSum[ExamCount] = rowWiseMarkSum[ExamCount] + GetMarkRations(totalMarkRatios, _ExamDetails[ExamCount].Name, mark, out _Ratio);


                            if (Subjct == 0)
                            {
                                TotalRatio = TotalRatio + _Ratio;
                            }

                            colWiseTotalMarkSum[ExamCount] = colWiseTotalMarkSum[ExamCount] + double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                            colWiseTotalMaxSum[ExamCount] = colWiseTotalMaxSum[ExamCount] +  max_Mark;


                        }
                        TempVal = 1;
                    }
                    if (_ExamDetails.Length == 3)
                    {
                        for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                        {
                            _Sub_Mark = _Sub_Mark + rowWiseMarkSum[ExamCount];
                            _Sub_Max = _Sub_Max + 100;
                            rowWiseMarkSum[ExamCount] = 0;
                            rowWiseMaxSum[ExamCount] = 0;
                        }


                        double LastMark = (_Sub_Mark / 100);
                        if (TotalRatio > 1)
                        {
                            LastMark = (LastMark / TotalRatio) * 100;
                        }
                        else
                        {
                            LastMark = (_Sub_Mark / _Sub_Max) * 100;
                        }
                        string Grade3 = GetGradeFromRatio(Grade, LastMark);
                        //string Grade3 = GetGradeFromMarks(Grade, _Sub_Mark, _Sub_Max);
                        row.Cells[10].AddParagraph().AddFormattedText(Grade3, TextFormat.Bold);
                    }
                     _Sub_Mark = 0;
                     _Sub_Max = 0;
                }

                row = tb.AddRow();

                row.Cells[0].AddParagraph().AddFormattedText("TOTAL", TextFormat.Bold); //Subject Name
                int T = 0, TVal = 0;
                double _LstSubMark = 0, Lst_TotalMark = 0;
                double rowWiseMark = 0, rowWiseMax = 0;

                double TRatio = 0;
                double Ratio = 0;

                for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                {
                    //  colWiseTotalMarkSum[ExamCount] 
                    //colWiseTotalMaxSum[ExamCount] 

                    if (TVal != 0)
                    {

                        T = T + 3;
                    }

                    row.Cells[T + 1].AddParagraph().AddFormattedText(colWiseTotalMarkSum[ExamCount].ToString(), TextFormat.Bold);
                    _LstSubMark = _LstSubMark + colWiseTotalMarkSum[ExamCount];
                    row.Cells[T + 2].AddParagraph().AddFormattedText(colWiseTotalMaxSum[ExamCount].ToString(), TextFormat.Bold);
                    Lst_TotalMark = Lst_TotalMark + colWiseTotalMaxSum[ExamCount];

                    string Grade1 = GetGradeFromMarks(Grade, colWiseTotalMarkSum[ExamCount], colWiseTotalMaxSum[ExamCount]);
                    row.Cells[T + 3].AddParagraph().AddFormattedText(Grade1, TextFormat.Bold);
                    TVal = 1;

                    double mark = (colWiseTotalMarkSum[ExamCount] / colWiseTotalMaxSum[ExamCount] * 100);

                    rowWiseMark = rowWiseMark + GetMarkRations(totalMarkRatios, _ExamDetails[ExamCount].Name, mark,out  Ratio);
                    rowWiseMax = rowWiseMax + 100;
                    TRatio = TRatio + Ratio;


                }
                if (_ExamDetails.Length == 3)
                {

                     
                    double Mark = (rowWiseMark / 100);
                    if (TRatio >1)
                        
                    Mark = (Mark / TRatio) * 100;
                    else
                        Mark = (rowWiseMark / rowWiseMax) * 100;
                    string Grade2 = GetGradeFromRatio(Grade, Mark);
                    row.Cells[10].AddParagraph().AddFormattedText(Grade2);

                }
                tb.Borders.Visible = true;

                // Co-curricular activiteis

                cel.Elements.Add(tb);

                if (CC_ExamDetails.Length > 0)
                {

                    paragraph = cel.AddParagraph();
                    paragraph.Format.Alignment = ParagraphAlignment.Left;
                    paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);
                    paragraph.AddLineBreak();
                    paragraph.AddFormattedText("Part 2: Co-Scholastic Area", TextFormat.Underline | TextFormat.Bold);
                    paragraph.AddLineBreak();

                    tb = new MigraDoc.DocumentObjectModel.Tables.Table();

                    tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                    tb.AddColumn((PgSt.PageWidth - 80) * 5 / 15);
                    tb.AddColumn((PgSt.PageWidth - 80) / 15);
                    tb.AddColumn((PgSt.PageWidth - 80) * 9 / 15);
                    
                    

                    string _TgroupName = "";
                    int Temp = 0;
                    DataSet CC_Grade = MyExamMang.GetGradeDataSet(CC_ExamDetails[CC_ExamDetails.Length - 1].GradeMasterId);
               
                    for (int Subjct = 0; Subjct < CC_SubDetails.Length; Subjct++)
                    {

                       
                        if (_TgroupName != CC_SubDetails[Subjct].GroupName)
                        {
                            if (Temp > 0)
                            {

                                tb.Borders.Visible = true;
                                cel.Elements.Add(tb);
                                paragraph = cel.AddParagraph();
                                paragraph.Format.Alignment = ParagraphAlignment.Left;
                                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);
                                paragraph.AddLineBreak();
                                if (CC_SubDetails[Subjct].GroupName.StartsWith("3"))
                                {
                                    paragraph.AddFormattedText("Part 3: Co-Scholastic Activities", TextFormat.Underline | TextFormat.Bold);
                                    paragraph.AddLineBreak();
                                }


                                tb = new MigraDoc.DocumentObjectModel.Tables.Table();

                                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                                tb.AddColumn((PgSt.PageWidth - 80) * 5 / 15);
                                tb.AddColumn((PgSt.PageWidth - 80) / 15);
                                tb.AddColumn((PgSt.PageWidth - 80) * 9 / 15);

                               
                            }

                            row = tb.AddRow();

                            _TgroupName = CC_SubDetails[Subjct].GroupName;

                            row.Cells[0].AddParagraph().AddFormattedText(CC_SubDetails[Subjct].GroupName, TextFormat.Bold);
                            
                            row.Cells[1].AddParagraph().AddFormattedText("Grade", TextFormat.Bold); //Exam Name            
                            row.Cells[1].Format.Alignment = ParagraphAlignment.Center;

                            row.Cells[2].AddParagraph().AddFormattedText("Descriptive Indicators", TextFormat.Bold); //Exam Name
                            row.Cells[2].Format.Alignment = ParagraphAlignment.Center;

                           
                        }

                        row = tb.AddRow();

                        row.Cells[0].AddParagraph().AddFormattedText(CC_SubDetails[Subjct].Name); //Subject Name
                        
                        for (int ExamCount = 0; ExamCount < CC_ExamDetails.Length; ExamCount++)
                        {
                            if (ClsObj[i].m_CC_ExamMarks[ExamCount, Subjct].MaxMark.ToString() != "-1")
                            {

                                double max_Mark = MyExamMang.GetMaxMarkFromExamScheduleIdandSubjectId(CC_ExamDetails[ExamCount].Id, CC_SubDetails[Subjct].Id);
                                
                               
                                

                                string _Grade = GetGradeFromMarks(CC_Grade, double.Parse(ClsObj[i].m_CC_ExamMarks[ExamCount, Subjct].MaxMark.ToString()), max_Mark);
                            
                                row.Cells[1].AddParagraph().AddFormattedText(_Grade);
                            }
                            else
                                row.Cells[1].AddParagraph().AddFormattedText("-");

                           
                         
                        }

                        row.Cells[2].AddParagraph().AddFormattedText(CC_SubDetails[Subjct].Desc);//sub description
                        //row = tb.AddRow();
                        Temp++;
                       
                    }
                    tb.Borders.Visible = true;
                    cel.Elements.Add(tb);

                }


                // dominic new 

                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.AddLineBreak();
                paragraph.AddLineBreak();


                tb = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                tb.AddColumn((PgSt.PageWidth - 80) * 5 / 15);
                tb.AddColumn((PgSt.PageWidth - 80) * 5 / 15);
                tb.AddColumn((PgSt.PageWidth - 80) * 5 / 15);



                row = tb.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("CLASS TEACHER", TextFormat.Bold);
                row.Cells[1].AddParagraph().AddFormattedText("PRINCIPAL", TextFormat.Bold); //Exam Name            
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[2].AddParagraph().AddFormattedText("PARENT", TextFormat.Bold); //Exam Name
                row.Cells[2].Format.Alignment = ParagraphAlignment.Center;

                row = tb.AddRow();
                tb.Borders.Visible = false;
                cel.Elements.Add(tb);
                
            }
            return document;
        }

        private string GetGradeFromRatio(DataSet Grade, double LastMark)
        {
            

            foreach (DataRow Dr in Grade.Tables[0].Rows)
            {
                if (LastMark >= double.Parse(Dr[1].ToString()))
                {
                    return Dr[0].ToString();
                }
            }
            return "";
        }

        private double GetMarkRations(DataSet totalMarkRatios, string ExamName, double Mark,out double Ratio)
        {
            Ratio = 0;
            double _Mark = 0;
            string sql = "SELECT tblcbseexamratiomap.RatioColumName from tblcbseexamratiomap where tblcbseexamratiomap.ExamName='" + ExamName + "'";
            OdbcDataReader  MyReader = MyClassMngr.m_MysqlDb.ExecuteQuery(sql);
            
            if (MyReader.HasRows)
            {
                if (totalMarkRatios.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow Dr in totalMarkRatios.Tables[0].Rows)
                    {
                         double.TryParse(Dr[MyReader.GetValue(0).ToString()].ToString(), out Ratio);

                         if (Ratio > 0)
                             _Mark = Mark * double.Parse(Dr[MyReader.GetValue(0).ToString()].ToString());
                         else
                             _Mark = Mark;
                    }
                }
            }
            else
            {
                return 1; 
            }

            return _Mark;            
        }

        private string GetGradeFromMarks(DataSet Grade, double _Mark, double MaxMark)
        {
            double avg=(_Mark/MaxMark)*100;
            
            foreach (DataRow Dr in Grade.Tables[0].Rows)
            {
                if (avg >= double.Parse(Dr[1].ToString()))
                {
                    return Dr[0].ToString();
                }
            }
            return "";
        }

       
        //Dominic : URGENT   -Need to change all the below code
     
        private void GetSubDetailsCC(int ClassId, int ExamId, out ExamNode[] _SubDetails)
        {
            string sql = " SELECT  distinct  tblsubjects.Id,tblsubjects.subject_name,tblsubjects.sub_Catagory, tbltime_subgroup.Name ,tblsubjects.sub_description from tblclassexamsubmap inner JOIN tblsubjects on tblclassexamsubmap.SubId= tblsubjects.Id inner JOIN tblclassexam on tblclassexam.id= tblclassexamsubmap.ClassExamId   inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.id inner join   tblexammark on tblexammark.ExamSchId=tblexamschedule.Id  inner join tbltime_subgroup on tbltime_subgroup.Id= tblsubjects.sub_Catagory  where  tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "  and tblclassexam.ClassId=" + ClassId + " and tblclassexam.ExamId=" + ExamId + " order by tblexammark.SubjectOrder ";

            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                int i = 0;
                int Count = MyReader.RecordsAffected;
                _SubDetails = new ExamNode[Count];

                while (MyReader.Read())
                {
                    _SubDetails[i].Id = int.Parse(MyReader.GetValue(0).ToString());
                    _SubDetails[i].Name = MyReader.GetValue(1).ToString();
                    
                    _SubDetails[i].Group = int.Parse(MyReader.GetValue(2).ToString());
                    _SubDetails[i].GroupName = MyReader.GetValue(3).ToString();
                    _SubDetails[i].Desc = MyReader.GetValue(4).ToString();

                    i++;
                }
            }
            else
            {
                _SubDetails = new ExamNode[0];
            }

        }

        private void GetExamDetailsCC(int ClassId, int ExamId, out ExamNode[] _ExamDetails)
        {

            string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period, tblexamschedule.GradeMasterId from tblexammaster inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id     where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblexammaster.Id=" + ExamId + " and tblexamschedule.Status='Completed'  order by tblexamschedule.id desc limit 1";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                int i = 0;
                int Count = MyReader.RecordsAffected;
                _ExamDetails = new ExamNode[Count];

                while (MyReader.Read())
                {
                    _ExamDetails[i].Id = int.Parse(MyReader.GetValue(0).ToString());
                    _ExamDetails[i].Name = MyReader.GetValue(2).ToString();
                    _ExamDetails[i].GradeMasterId = int.Parse(MyReader.GetValue(3).ToString());
                    string sql1 = "select Max( tblexammark.ExamDate) from tblexammark where tblexammark.ExamSchId=" + int.Parse(MyReader.GetValue(0).ToString());
                    OdbcDataReader dr = MyExamMang.m_MysqlDb.ExecuteQuery(sql1);
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


        private string GetExamTypeName(string _ExamDescription ,out int _SubTypeId )
        {
            _SubTypeId = 0;
            string _ExamTypeName = "";
            string sql = "select tblsubject_type.Id, tblsubject_type.sbject_type from tblsubject_type where tblsubject_type.TypeDisc='" + _ExamDescription + "' ";

            OdbcDataReader Reader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

            if (Reader.HasRows)
            {
                _ExamTypeName = Reader.GetValue(1).ToString();
                _SubTypeId = int.Parse(Reader.GetValue(0).ToString());
            }

            return _ExamTypeName;

        }

        private void GetSubDetails(int ClassId, int ExamTypeId, out ExamNode[] _SubDetails)
        {
            //string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=tblexamschedule.ClassExamId and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId  where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblclassexam.ClassId=" + ClassId + " and tblexammaster.Id=" + ExamId;
           // string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark ,tblsubjects.sub_description from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id  inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId   inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId   inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=tblexamschedule.ClassExamId   and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId     where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "   and tblexammaster.Id     in (select tblexammaster.id from tblsubject_type  inner join tblexammaster on tblexammaster.ExamTypeId=tblsubject_type.Id  inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id   inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id    where tblclassexam.ClassId=" + ClassId + "  and    tblsubject_type.Id=" + ExamTypeId + " and tblexamschedule.Status='Completed'  order by tblexamschedule.id )";
            // no need to take max mark here 

            string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblsubjects.sub_description from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id  inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId   inner join tblexammaster on  tblexammaster.Id=  .tblclassexam.ExamId Inner join tblsubject_type on  tblexammaster.ExamTypeId=tblsubject_type.Id    where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "    and   tblclassexam.ClassId=" + ClassId + " and tblsubject_type.Id=" + ExamTypeId + "   and tblexamschedule.Status='Completed'   order by tblexammark.SubjectOrder  ";           
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);         
            
            if (MyReader.HasRows)           
            {
                int i = 0;
                int Count = MyReader.RecordsAffected;
                _SubDetails = new ExamNode[Count];

                while (MyReader.Read())
                {
                    _SubDetails[i].Id = int.Parse(MyReader.GetValue(0).ToString());
                    _SubDetails[i].Name = MyReader.GetValue(1).ToString();
                   // _SubDetails[i].MaxMark = double.Parse(MyReader.GetValue(2).ToString());
                    _SubDetails[i].Desc = MyReader.GetValue(2).ToString();
                    i++;
                }
            }
            else
            {
                _SubDetails = new ExamNode[0];
            }

        }

        private void GetExamDetails(int ClassId, int ExamTypeId, out ExamNode[] _ExamDetails)
        {

           // string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period from tblexammaster inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id     where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblexammaster.Id=" + ExamId + " and tblexamschedule.Status='Completed'  order by tblexamschedule.id desc";
            string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period, tblexamschedule.GradeMasterId from tblsubject_type  inner join tblexammaster on tblexammaster.ExamTypeId=tblsubject_type.Id   inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id     where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblsubject_type.Id=" + ExamTypeId + " and tblexamschedule.Status='Completed'  order by tblexamschedule.id ";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                int i = 0;
                int Count = MyReader.RecordsAffected;
                _ExamDetails = new ExamNode[Count];

                while (MyReader.Read())
                {
                    _ExamDetails[i].Id = int.Parse(MyReader.GetValue(0).ToString());
                    _ExamDetails[i].Name = MyReader.GetValue(1).ToString();
                    _ExamDetails[i].GradeMasterId =  int.Parse(MyReader.GetValue(3).ToString());
                    string sql1 = "select Max( tblexammark.ExamDate) from tblexammark where tblexammark.ExamSchId=" + int.Parse(MyReader.GetValue(0).ToString());
                    OdbcDataReader dr = MyExamMang.m_MysqlDb.ExecuteQuery(sql1);
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

        private DataSet GetAllStudentListFromClass(int StudentId, int ClassId)
        {
           
            DataSet _StdentSet = new DataSet();
            DataTable dt;
            DataRow dr;

            _StdentSet.Tables.Add(new DataTable("Student"));

           dt = _StdentSet.Tables["Student"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("RollNum");
          
            dt.Columns.Add("DOB");
            dt.Columns.Add("Class");
            dt.Columns.Add("AdmissionNum");
            dt.Columns.Add("FatherName");
            dt.Columns.Add("MotherName");
            dt.Columns.Add("Tel");
            dt.Columns.Add("Add");

            string sql_Student = "select tblview_student.Id, tblview_student.StudentName, tblview_student.RollNo, tblclass.ClassName,date_Format(tblview_student.DOB,'%d/%m/%Y') as DOB , tblview_student.AdmitionNo,tblview_student.GardianName,tblview_student.MothersName, tblview_student.ResidencePhNo ,tblview_student.Address from tblview_student inner join tblclass on tblclass.Id= tblview_student.ClassId inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblview_student.Id  where tblview_student.ClassId=" + ClassId + " and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId;

            if (StudentId != 0)
                sql_Student = sql_Student + " and tblview_student.Id=" + StudentId;



            DataSet _studList = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql_Student);


            foreach (DataRow dr_values in _studList.Tables[0].Rows)
            {

                dr = _StdentSet.Tables["Student"].NewRow();

                dr["Id"] = dr_values["Id"];
                dr["Name"] = dr_values["StudentName"];
                dr["RollNum"] = dr_values["RollNo"];
                dr["DOB"] = dr_values["DOB"];
                dr["Class"] = dr_values["ClassName"];
                dr["AdmissionNum"] = dr_values["AdmitionNo"];
                dr["FatherName"] = dr_values["GardianName"];
                dr["MotherName"] = dr_values["MothersName"];
                dr["Tel"] = dr_values["ResidencePhNo"];
                dr["Add"] = dr_values["Address"];
                
                _StdentSet.Tables["Student"].Rows.Add(dr);

            }       
            return _StdentSet;


        }

        private DataSet GetStudDetails(int StudentId, int ClassId, int ExamId)
        {

            MigraDoc.DocumentObjectModel.Font subfont = new MigraDoc.DocumentObjectModel.Font("Times New Roman", 11);

            DataSet _StdentSet = new DataSet();
            DataTable dt;
            DataRow dr;

            _StdentSet.Tables.Add(new DataTable("Student"));

            dt = _StdentSet.Tables["Student"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("RollNum");
          
            dt.Columns.Add("DOB");
            dt.Columns.Add("Class");
            dt.Columns.Add("AdmissionNum");
            dt.Columns.Add("FatherName");
            dt.Columns.Add("MotherName");
            dt.Columns.Add("Tel");

            string sql_Student = "select tblview_student.Id, tblview_student.StudentName, tblview_student.RollNo, tblclass.ClassName,date_Format(tblview_student.DOB,'%d/%m/%Y') as DOB , tblview_student.AdmitionNo,tblview_student.GardianName,tblview_student.MothersName, tblview_student.ResidencePhNo    from tblview_student inner join tblclass on tblclass.Id= tblview_student.ClassId inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblview_student.Id  where tblview_student.ClassId=" + ClassId + " and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblview_student.Id IN( select tblstudentmark.StudId from tblstudentmark inner join tblexamschedule on tblexamschedule.Id = tblstudentmark.ExamSchId inner join  tblclassexam on tblclassexam.Id=  tblexamschedule.ClassExamId where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblclassexam.ExamId= " + ExamId + " and tblexamschedule.Status='Completed'  )";

            if (StudentId != 0)
                sql_Student = sql_Student + " and tblview_student.Id=" + StudentId;

            DataSet _studList = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql_Student);

         
            foreach (DataRow dr_values in _studList.Tables[0].Rows)
            {

                dr = _StdentSet.Tables["Student"].NewRow();

                dr["Id"] = dr_values["Id"];
                dr["Name"] = dr_values["StudentName"];
                dr["RollNum"] = dr_values["RollNo"];
                dr["DOB"] = dr_values["DOB"];
                dr["Class"] = dr_values["ClassName"];
                dr["AdmissionNum"] = dr_values["AdmitionNo"];
                dr["FatherName"] = dr_values["GardianName"];
                dr["MotherName"] = dr_values["MothersName"];
                dr["Tel"] = dr_values["ResidencePhNo"];

                _StdentSet.Tables["Student"].Rows.Add(dr);

            }           
            return _StdentSet;
        }

        private MigraDoc.DocumentObjectModel.Tables.Table GetStudentInformations(PageSetup PgSt)
        {
            // generate student information table and  return that table;
            MigraDoc.DocumentObjectModel.Tables.Table tb = new MigraDoc.DocumentObjectModel.Tables.Table();

            MigraDoc.DocumentObjectModel.Tables.Row row = tb.AddRow();
            tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

            tb.AddColumn((PgSt.PageWidth - 80) / 6);
            tb.AddColumn((PgSt.PageWidth - 80) / 3);
            tb.AddColumn((PgSt.PageWidth - 80) / 6);
            tb.AddColumn((PgSt.PageWidth - 80) / 3);



            row = tb.AddRow();
            row.Cells[0].AddParagraph("Name");
            row.Cells[1].AddParagraph(":");
            row.Cells[2].AddParagraph("Father's Name");
            row.Cells[3].AddParagraph(":");
            row = tb.AddRow();
            row.Cells[0].AddParagraph("Date of Birth");
            row.Cells[1].AddParagraph(":");
            row.Cells[2].AddParagraph("Mother's Name");
            row.Cells[3].AddParagraph(":");
            row = tb.AddRow();
            row.Cells[0].AddParagraph("Admission No");
            row.Cells[1].AddParagraph(":");
            row.Cells[2].AddParagraph("Telephone No");
            row.Cells[3].AddParagraph(":");
            row = tb.AddRow();
            row.Cells[0].AddParagraph("Class & Roll No");
            row.Cells[1].AddParagraph(":");
            row.Cells[2].AddParagraph("Address");
            row.Cells[3].AddParagraph(":");

            tb.Borders.Visible = false;

            return tb;
        }

        private void GetSchoolDetails(out SchoolDetails _SchoolDetails)
        {
            _SchoolDetails.SchoolName = "";
            _SchoolDetails.Address = "";
            _SchoolDetails.LogoURL = "";

            string sql = "select tblschooldetails.SchoolName, tblschooldetails.Address, tblschooldetails.LogoUrl from tblschooldetails";
            OdbcDataReader MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                _SchoolDetails.SchoolName = MyReader.GetValue(0).ToString();
                _SchoolDetails.Address = MyReader.GetValue(1).ToString();
                _SchoolDetails.LogoURL = MyReader.GetValue(2).ToString();
            }

        }

        private void LoadDetails()
        {
            LoadClassDetailsToDropDown();
            LoadExamDetailsToDropDown();
            LoadStudentsDetailsToDropDown();
            LoadScholasticArea();
        }

        private void LoadScholasticArea()
        {
            int _SubTypeId=0;
            string _Co_CurricularExam = GetExamTypeName("CBSE ACTIVITY REPORT", out _SubTypeId);
            if (Drp_SelectClass.SelectedValue != "-1")
            {
                Drp_Scholastic.Items.Clear();
                //string sql = "select tblexammaster.Id, tblexammaster.ExamName from tblsubject_type  inner join tblexammaster on tblexammaster.ExamTypeId= tblsubject_type.Id where   tblsubject_type.TypeDisc='CBSE ACTIVITY REPORT';";
                string sql = "select distinct tblexammaster.Id, tblexammaster.ExamName from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster ON tblexammaster.Id= tblclassexam.ExamId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id  where tblclassexam.ClassId = " + int.Parse(Drp_SelectClass.SelectedValue) + " and tblexamschedule.BatchId = " + MyUser.CurrentBatchId + " and tblexammaster.ExamTypeId= " + _SubTypeId + " and tblexamschedule.status ='Completed'";
         
                MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

                if (MyReader.HasRows)
                {
                    Drp_Scholastic.Items.Add(new System.Web.UI.WebControls.ListItem("Select Co-Scholastic area", "0"));

                    while (MyReader.Read())
                    {
                        System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_Scholastic.Items.Add(li);
                    }
                }
                else
                {
                    Drp_Scholastic.Items.Add(new System.Web.UI.WebControls.ListItem("No Details Found", "-1"));
                }
            }
            else
            {
                Drp_Scholastic.Items.Add(new System.Web.UI.WebControls.ListItem("Select Co-Scholastic area", "0"));
            }
        }

        private void LoadExamDetailsToDropDown()
        {
            if (Drp_SelectClass.SelectedValue != "-1")
            {
                Drp_SelectExam.Items.Clear();

                string sql = "select tblsubject_type.Id, tblsubject_type.sbject_type from tblsubject_type where tblsubject_type.TypeDisc='CBSE MAIN REPORT'";
                MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

                if (MyReader.HasRows)
                {
                    Drp_SelectExam.Items.Add(new System.Web.UI.WebControls.ListItem("Select One Exam", "0"));

                    while (MyReader.Read())
                    {
                        System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_SelectExam.Items.Add(li);
                    }
                }
                else
                {
                    Drp_SelectExam.Items.Add(new System.Web.UI.WebControls.ListItem("No Exam Found", "-1"));
                }
            }
            else
            {
                Drp_SelectExam.Items.Add(new System.Web.UI.WebControls.ListItem("Select One Exam", "0"));
            }
        }

        private void LoadStudentsDetailsToDropDown()
        {
            if (Drp_SelectClass.SelectedValue != "-1")
            {
                int ClassId = 0;
                int.TryParse(Drp_SelectClass.SelectedValue.ToString(), out ClassId);

                Drp_SelectStudent.Items.Clear();
                string Sql = "select tblview_student.Id, tblview_student.StudentName from tblview_student INNER join tblclass on tblview_student.ClassId=tblclass.Id where tblview_student.ClassId=" + ClassId + " and  tblview_student.LIve=1  and tblview_student.RollNo<>-1  order by  tblview_student.StudentName ASC";
                MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(Sql);

                if (MyReader.HasRows)
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("All Student", "0");
                    Drp_SelectStudent.Items.Add(li);

                    while (MyReader.Read())
                    {
                        System.Web.UI.WebControls.ListItem Li = new System.Web.UI.WebControls.ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_SelectStudent.Items.Add(Li);
                    }
                }
                else
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Student Not Found", "-1");
                    Drp_SelectStudent.Items.Add(li);
                }
            }
            else
            {

                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("All Student", "0");
                Drp_SelectStudent.Items.Add(li);
            }
        }

        private void LoadClassDetailsToDropDown()
        {
            Drp_SelectClass.Items.Clear();
            string sql = "SELECT tblclass.Id,tblclass.ClassName FROM tblclass where tblclass.Status =1 AND tblclass.Id IN (SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ")) ORDER BY tblclass.Standard,tblclass.ClassName";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_SelectClass.Items.Add(li);
                }
            }
            else
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("No Class Found", "-1");
                Drp_SelectClass.Items.Add(li);
            }
        }
        
    }
}
