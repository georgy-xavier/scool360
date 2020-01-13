using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
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
using PdfSharp.Drawing;
using MigraDoc.DocumentObjectModel.Shapes;
using System.IO;

namespace WinEr
{
   
    public class ProgessReportClass
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




        public ProgessReportClass(MysqlClass _Mysqlobj, int _StudentId)
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
                m_StudDetails.Class = _StudDetails[5].ToString();

                m_StudDetails.DOB = _StudDetails[4].ToString();
                m_StudDetails.AdmissionNum = _StudDetails[6].ToString();
                m_StudDetails.Attendance = _StudDetails[3].ToString();
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


    public partial class ProgressReport : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private ExamManage          MyExamMang;
        private ClassOrganiser      MyClassMngr;
        private KnowinUser          MyUser;
        private OdbcDataReader      MyReader = null;
        private Attendance          MyAttendence;
        private int                 _ClassId = 0;
        private int                 _ExamId = 0;
        private MysqlClass          _Mysqlobj;
        public StudentDetails       m_StudDetails;
         MigraDoc.DocumentObjectModel.Font Font_All;
         MigraDoc.DocumentObjectModel.Font Font_ReportName;
         MigraDoc.DocumentObjectModel.Font Font_StudentDetails;
         MigraDoc.DocumentObjectModel.Font Font_Heading;
         MigraDoc.DocumentObjectModel.Font Font_ItemDetails;
         MigraDoc.DocumentObjectModel.Font Font_Total;
         MigraDoc.DocumentObjectModel.Font Font_BottomDetails;
         MigraDoc.DocumentObjectModel.Font Font_BottomDetails1;
         MigraDoc.DocumentObjectModel.Font Font_BottomDetails2;
         private SchoolClass objSchool = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)            
                Response.Redirect("Default.aspx");            

            MyUser          = (KnowinUser)Session["UserObj"];
            MyClassMngr     = MyUser.GetClassObj();
            MyStudMang      = MyUser.GetStudentObj();
            MyExamMang      = MyUser.GetExamObj();
            MyAttendence    = MyUser.GetAttendancetObj();
           
            if (MyClassMngr == null)           
                Response.Redirect("RoleErr.htm");
            if (MyStudMang == null)
                Response.Redirect("RoleErr.htm");
            if (MyExamMang == null)
                Response.Redirect("RoleErr.htm");

            string _conStr = WinerUtlity.SingleSchoolConnectionString;
            if (WinerUtlity.NeedCentrelDB())
            {
                if (Session[WinerConstants.SessionSchool] == null)
                {
                    Response.Redirect("Logout.aspx");
                }
                objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                _conStr = objSchool.ConnectionString;
            }
            _Mysqlobj = new MysqlClass(_conStr);

            if (!IsPostBack)
            {
                LoadDetails();
                //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:handleChange(); ", true);

            }
            else
            {

                //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:handleChange(); ", true);
            }
        }

        protected void Drp_SelectClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadExamDetailsToDropDown();
            LoadStudentsDetailsToDropDown();
       
        }

        protected void Btn_Exam_Click(object sender, EventArgs e)
        {
             string _ErrMessage = "";
             Lbl_Err.Text = "";
             if (ValidateExams(out _ErrMessage))
             {
                 CreateReort(out _ErrMessage);
             }
             else
             {
                 Lbl_Err.Text = _ErrMessage;
             }
        }

        private void CreateReort(out string  _ErrMessage)
        {
            _ErrMessage = "";
            int             _CCExamId       = 0;
            DataSet         StudList        = new DataSet();
            ExamNode[]      _ExamDetails;
            ExamNode[]      _SubDetails;          
            ExamNode[]      CC_ExamDetails  = null;
            ExamNode[]      CC_SubDetails   = null; 

            SchoolDetails   _SchoolDetails;
            DataSet         TopStudentList  = new DataSet();

            List<ProgessReportClass> ClsObj = new List<ProgessReportClass>();
            ProgessReportClass _ReportObj;

            try
            {
                int StudentId = 0; 
                int.TryParse(Drp_SelectStudent.SelectedValue, out StudentId);
                int.TryParse(Drp_SelectClass.SelectedValue, out _ClassId);
                int.TryParse(Drp_SelectExam.SelectedValue, out _ExamId);
                _CCExamId = MyExamMang.GetExamIdFromSubType("Co-curricular Activities", _ClassId);

                GetExamDetails(_ClassId, _ExamId, out _ExamDetails);
                GetSubDetails(_ClassId, _ExamId, out _SubDetails);
                GetSchoolDetails(out _SchoolDetails);
                StudList = GetStudDetails(StudentId, _ClassId, _ExamId);
            
                GetExamDetails(_ClassId, _CCExamId, out CC_ExamDetails);
                GetSubDetails(_ClassId, _CCExamId, out CC_SubDetails);

                foreach (DataRow Dr in StudList.Tables[0].Rows)
                {
                    _ReportObj = new ProgessReportClass(_Mysqlobj, int.Parse(Dr[0].ToString()));
                    _ReportObj.ExamDetails(_ExamDetails, _SubDetails, Dr,"Main");
                    _ReportObj.ExamDetails(CC_ExamDetails, CC_SubDetails, Dr, "CC");
                    
                    ClsObj.Add(_ReportObj);
                }


                CreateReport(ClsObj, _ExamDetails, _SubDetails, _SchoolDetails, StudentId, CC_ExamDetails, CC_SubDetails);
            }
            catch(Exception e)
            {

                _ErrMessage = e.Message;
                //Response.Write("<script>alert(\""+e.Message+"\");</script>");
                //Response.Write("<script>window.close()</script>");
            }

        }

        private void CreateReport(List<ProgessReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, SchoolDetails _SchoolDetails, int StudentId, ExamNode[] CC_ExamDetails, ExamNode[] CC_SubDetails)
        {
            //LoadFont();
            string _PhysicalPath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath(""));
            if (Directory.Exists(_PhysicalPath))
                Directory.CreateDirectory(_PhysicalPath);


            Document document = LoadPDFPage(ClsObj, _ExamDetails, _SubDetails,CC_ExamDetails,CC_SubDetails);
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
                MainName = ClsObj[0].m_StudDetails.Name;

            filename = _PhysicalPath + "\\PDF_Files\\PRC_" + MainName + ".pdf";

            if (File.Exists(filename))
                File.Delete(filename);

            pdfRenderer.PdfDocument.Save(filename);
            // ...and start a viewer.
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "null", "window.open(\"OpenPdfPage.aspx?PdfName=PRC_" + MainName + ".pdf\",'Info','status=1, width=1000, height=700,,resizable=1,scrollbars=1');", true);
                           
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=PRC_" + MainName + ".pdf\");", true);
          // Response.Redirect("OpenPdfPage.aspx?PdfName=PRC_" + MainName + ".pdf", false);

        }

        private Document LoadPDFPage(List<ProgessReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, ExamNode[] CC_ExamDetails, ExamNode[] CC_SubDetails)
        {
            Document ReportCard = new Document();
            
            string _ExamName = _ExamDetails[_ExamDetails.Length - 1].Name;
            string _ReportHeading = "REPORT CARD : " + Drp_SelectExam.SelectedItem +" : "+ MyUser.CurrentBatchName;
            // Start  Create Details
            for (int Count = 0; Count < ClsObj.Count; Count++)
            {
                PageSetup PgSt = ReportCard.DefaultPageSetup;
              
                PgSt.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.A4;
                PgSt.LeftMargin = 70;
                PgSt.RightMargin =60;
                PgSt.TopMargin = 80;
                PgSt.BottomMargin = 70;

                PgSt.HeaderDistance = 0;
                PgSt.FooterDistance = 0;

                // Add a section to the document
                Section Report_Section = ReportCard.AddSection();
                
                Paragraph Top = Report_Section.AddParagraph();

                Top.Format.Alignment = ParagraphAlignment.Center;
                Top.Format.Font = Font_ReportName;
                Top.Format.Font.Bold = true;
                Top.Format.Font.Underline =Underline.Single;
                Top.AddFormattedText(_ReportHeading);

                Paragraph _space = Report_Section.AddParagraph();
                _space.AddFormattedText("\n");
              
                // student details


                MigraDoc.DocumentObjectModel.Tables.Table Stud_Table = new  MigraDoc.DocumentObjectModel.Tables.Table();

                Stud_Table.Format.Font = Font_StudentDetails;
                Stud_Table.Format.Font.Bold = true;

                Column S_column = Stud_Table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2));

                S_column = Stud_Table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(6));
                S_column.Format.Font.Bold = true;
                S_column = Stud_Table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1));

                S_column = Stud_Table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));
                S_column.Format.Font.Bold = false;
                S_column = Stud_Table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(6));
                S_column.Format.Font.Bold = true;

                Row row = Stud_Table.AddRow();
                row.Height = 20;
                row.Format.Font = Font_StudentDetails;
                

                Cell cell = row.Cells[0];
                cell.Format.Font.Bold = true; 
                cell.AddParagraph("Name");

                cell = row.Cells[1];
                cell.Format.Font.Bold = false; 
                cell.AddParagraph(": " + ClsObj[Count].m_StudDetails.Name + "");

                cell = row.Cells[2];
                cell.AddParagraph("");

                cell = row.Cells[3];
                cell.Format.Font.Bold = true; 

                if(Chk_regno.Checked)
                    cell.AddParagraph("Reg No");
                else
                    cell.AddParagraph("Roll No");
                

                cell = row.Cells[4];
                cell.Format.Font.Bold = false; 
                cell.AddParagraph(": " + ClsObj[Count].m_StudDetails.RollNum + "");

                row = Stud_Table.AddRow();
                row.Format.Font = Font_StudentDetails;
                row.Format.Font.Bold = true; 

                row.Height = 20;

                cell = row.Cells[0];
                cell.Format.Font.Bold = true; 
                cell.AddParagraph("Class ");

                cell = row.Cells[1];
                cell.Format.Font.Bold = false; 
                cell.AddParagraph(": " + ClsObj[Count].m_StudDetails.Class + "");

                cell = row.Cells[2];
                cell.Format.Font.Bold = false; 
                cell.AddParagraph("");

                cell = row.Cells[3];
                cell.Format.Font.Bold = true; 
                cell.AddParagraph("Admn No");

                cell = row.Cells[4];
                cell.Format.Font.Bold = false; 
                cell.AddParagraph(": " + ClsObj[Count].m_StudDetails.AdmissionNum + "");

                ReportCard.LastSection.Add(Stud_Table);


                Table TableMain = GetMainReportDetails(ClsObj[Count].m_ExamMarks, _ExamDetails, _SubDetails, ClsObj[Count].m_Average, ClsObj[Count].m_TotalMarks, ClsObj[Count], ClsObj[Count].m_StudentId, ClsObj[Count].m_MaxTotal, ClsObj[Count].m_CC_ExamMarks, CC_ExamDetails, CC_SubDetails, ClsObj[Count].m_CC_Average);
                ReportCard.LastSection.Add(TableMain);

                Paragraph space1 = Report_Section.AddParagraph();
                space1.AddFormattedText("\n");
                space1.AddFormattedText("\n");

                Table TotalRepot = new MigraDoc.DocumentObjectModel.Tables.Table();
                
                Column M_column = TotalRepot.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(4));
                M_column = TotalRepot.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(4));
                M_column = TotalRepot.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(8));
              
                Row row1 = TotalRepot.AddRow();
                row1.Height = 15;
                row1.Format.Font = Font_BottomDetails;
                row1.Format.Font.Bold = true;

                Cell cell1;
                if (Chk_needrank.Checked)//mani 29.8.2013
                {
                    cell1 = row1.Cells[0];
                    cell.Format.Font.Bold = true;
                    cell1.Format.Alignment = ParagraphAlignment.Left;
                    cell1.AddParagraph("Rank  ");


                    cell1 = row1.Cells[1];
                    cell.Format.Font.Bold = false;
                    cell1.Format.Alignment = ParagraphAlignment.Left;
                    cell1.AddParagraph(": " + ClsObj[Count].m_Ranks[_ExamDetails.Length - 1].MaxMark);
                }

                //dominic  if need to show the attendance commend this 3 lines and uncommend below three lines
                if (Chk_needattendance.Checked)//mani 29.8.2013
                {
                    cell1 = row1.Cells[2];
                    cell1.Format.Alignment = ParagraphAlignment.Left;
                    cell1.AddParagraph("Attendance : " + ClsObj[Count].m_StudDetails.Attendance);
                }
                
                //cell1 = row1.Cells[2];
                //cell1.Format.Alignment = ParagraphAlignment.Left;
                //cell1.AddParagraph("Attendance : " + ClsObj[Count].m_StudDetails.Attendance);
              


                ReportCard.LastSection.Add(TotalRepot);

                Paragraph space2= Report_Section.AddParagraph();
                space2.AddFormattedText("\n");
                space2.AddFormattedText("\n");
               

                Paragraph Remarks = Report_Section.AddParagraph();
                
                Remarks.Format.Alignment = ParagraphAlignment.Left;
                Remarks.Format.Font = Font_ReportName;
                Remarks.Format.Font.Bold = true;
                Remarks.AddFormattedText("Remarks: ");

                Paragraph space3 = Report_Section.AddParagraph();
                space3.AddFormattedText("\n");
                space3.AddFormattedText("\n");
          
                Table Bottom = new MigraDoc.DocumentObjectModel.Tables.Table();
                Bottom.Format.Font = Font_BottomDetails1;
                Bottom.Format.Font.Bold = true;

                Column B_column = Bottom.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(5));
                B_column = Bottom.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(5));
                B_column = Bottom.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(6));

                Row row2 = Bottom.AddRow();
                row2.Height = 15;
                if (Chk_signature.Checked)
                {
                    string[] _string = Txt.Text.Trim().Split('$');
                    int i = 0;
                    Cell cell2=new Cell(); 
                    foreach (string str in _string)
                    {
                        cell2 = row2.Cells[i];
                        if(i==0)
                            cell2.Format.Alignment = ParagraphAlignment.Left;
                        else if(i==1)
                            cell2.Format.Alignment = ParagraphAlignment.Center;
                        else
                            cell2.Format.Alignment = ParagraphAlignment.Right;


                        cell2.AddParagraph(str);
                        i++;
                    }
                }
                else
                {
                    Cell cell2 = row2.Cells[0];
                    cell2.Format.Alignment = ParagraphAlignment.Left;
                    cell2.AddParagraph("Teacher's Sign");

                    cell2 = row2.Cells[1];
                    cell2.Format.Alignment = ParagraphAlignment.Center;
                    cell2.AddParagraph("Parent's Sign");

                    cell2 = row2.Cells[2];
                    cell2.Format.Alignment = ParagraphAlignment.Right;
                    cell2.AddParagraph("H.M.Sign");
                }

                ReportCard.LastSection.Add(Bottom);   
            }

            return ReportCard;
        }

        private Table GetMainReportDetails(MarkNode[,] markNode, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, double[] m_Average, MarkNode[] m_TotalMarks, ProgessReportClass progessReportClass, int m_StudentId, double[] _MaxMark, MarkNode[,] m_CC_ExamMarks, ExamNode[] CC_ExamDetails, ExamNode[] CC_SubDetails, double[] m_CC_Average)
        {
            int _Rowheight=25;
        
            Table MarkRepot = new MigraDoc.DocumentObjectModel.Tables.Table();
            MarkRepot.Borders.Visible = true;          
            MarkRepot.Format.Font = Font_ItemDetails;
            MarkRepot.Format.Font.Bold = true;

            Column M_column = MarkRepot.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.5));
            if (Chk_needpercentage.Checked)//mani 29.8.2013
                M_column = MarkRepot.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(9));
            else
                M_column = MarkRepot.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(11));
           
            M_column = MarkRepot.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2));
            M_column = MarkRepot.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2));
            if (Chk_needpercentage.Checked)//mani 29.8.2013
                M_column = MarkRepot.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2));
           

            Row row = MarkRepot.AddRow();
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Height = 20;
            row.Format.Font.Bold = true;
            //row.Format.Font = fontMain;

            Cell cell = row.Cells[0];
            cell.MergeRight = 1;
            cell.Format.Alignment = ParagraphAlignment.Center;                
            cell.AddParagraph("Subject");

            cell = row.Cells[2];
            cell.Format.Alignment = ParagraphAlignment.Center;
            cell.AddParagraph("Max. Marks");

            cell = row.Cells[3];
            cell.Format.Alignment = ParagraphAlignment.Center;
            cell.AddParagraph("Marks Obtd");

            if (Chk_needpercentage.Checked)//mani 29.8.2013
            {
                cell = row.Cells[4];
                cell.Format.Alignment = ParagraphAlignment.Center;
                cell.AddParagraph("% of Marks");
            }

            int _GroupId = 0;

            string[] SubGroup = MyExamMang.GetSubectGroupd();

            int groupId = 0;
            int _GroupCount = 0;
            int _MeargeCount = 0;
            int _SubGrpCount = 0;

          


            // MAIN EXAM
            for (int Subjct = 0; Subjct < _SubDetails.Length; Subjct++)
            {
                int _Temp = 0;

                row = MarkRepot.AddRow();
                row.Format.Alignment = ParagraphAlignment.Left;
                row.VerticalAlignment = VerticalAlignment.Center;
                row.Height = _Rowheight;

                if (Chk_SubGroup.Checked)
                {
                    _GroupCount = GetTotalCountWithSameGroup(_SubDetails, _SubDetails[Subjct].Group);


                    if (CC_SubDetails.Length>0 && CC_SubDetails[0].Group == _SubDetails[Subjct].Group)
                    {
                        _SubGrpCount = GetTotalCountWithSameGroup(CC_SubDetails, CC_SubDetails[0].Group);
                        _GroupCount = _GroupCount + _SubGrpCount;
                    }


                
                    if (_GroupId != _SubDetails[Subjct].Group)
                    {

                        cell = row.Cells[0];
                      
                        if (_GroupCount > 1)
                            cell.MergeDown = _GroupCount - 1;
                     
                        
                        //cell.Format.Alignment = ParagraphAlignment.Left;
                        //cell.VerticalAlignment = VerticalAlignment.Center;
                     
                        
                        TextFrame Frame = cell.AddTextFrame();
                        Frame.Orientation = TextOrientation.Upward;
                        Frame.Width = Unit.FromCentimeter(1);
                        
                        //Frame.Height = _GroupCount * _Rowheight;
                        //Frame.MarginLeft=
                        Paragraph para = Frame.AddParagraph(_SubDetails[Subjct].GroupName);
                        //para.Format.SpaceBefore = Unit.FromCm(0.4);
                        para.Format.Font.Bold = true;
                        //para.Format.ClearAll();
                        para.Format.Alignment = ParagraphAlignment.Center;
                        
                        cell = row.Cells[1];
                        cell.AddParagraph(_SubDetails[Subjct].Name);


                    }                
                    else
                    {
                        cell = row.Cells[1];
                        cell.Format.Font.Bold = true;
                        cell.AddParagraph(_SubDetails[Subjct].Name);
                    }

                    _GroupId = _SubDetails[Subjct].Group;
                    //cell = row.Cells[1];
                   // cell.AddParagraph(_SubDetails[Subjct].Name);
                }
                else
                {
                    cell = row.Cells[0];
                    cell.MergeRight = 1;
                    cell.Format.Font.Bold = true;
                    cell.AddParagraph(_SubDetails[Subjct].Name);
                }

                cell = row.Cells[2];
                cell.Format.Font.Bold = true;
                cell.Format.Alignment = ParagraphAlignment.Center;
                cell.VerticalAlignment = VerticalAlignment.Center;
                cell.AddParagraph(_SubDetails[Subjct].MaxMark.ToString());

                cell = row.Cells[3];
                cell.Format.Font.Bold = false;
                cell.Format.Alignment = ParagraphAlignment.Center;
                cell.VerticalAlignment = VerticalAlignment.Center;
                if (markNode[_ExamDetails.Length - 1, Subjct].MaxMark.ToString() == "0" || markNode[_ExamDetails.Length - 1, Subjct].MaxMark.ToString() == "-1")
                {
                    cell.AddParagraph("-");
                   
                    _Temp = 1;
                }
                else
                    cell.AddParagraph(markNode[_ExamDetails.Length - 1, Subjct].MaxMark.ToString());


                if (Chk_needpercentage.Checked)//mani 29.8.2013
                {
                    cell = row.Cells[4];
                    cell.Format.Font.Bold = false;
                    cell.Format.Alignment = ParagraphAlignment.Center;
                    cell.VerticalAlignment = VerticalAlignment.Center;

                    if (_Temp == 1)
                    {
                        cell.AddParagraph("-");
                    }
                    else
                    {
                        double persentage = (markNode[_ExamDetails.Length - 1, Subjct].MaxMark / _SubDetails[Subjct].MaxMark) * 100;
                        if (persentage == -1)
                            cell.AddParagraph("-");
                        else
                            cell.AddParagraph(persentage.ToString("0"));
                    }
                }

            }
            //MAIN EXAM END

            // CC EXAMS
            int _Mearged = 0;
            for (int Subjct = 0; Subjct < CC_SubDetails.Length; Subjct++)
            {
                int _Temp = 0;

                row = MarkRepot.AddRow(); 
                row.Height = _Rowheight;
                row.Format.Alignment = ParagraphAlignment.Left;
                row.VerticalAlignment = VerticalAlignment.Center;
               
               
                if (Chk_SubGroup.Checked)
                {

                    cell = row.Cells[0];
                    cell.Format.Font.Bold = true;
                    _GroupCount = GetTotalCountWithSameGroup(_SubDetails, CC_SubDetails[Subjct].Group);

                    if (_GroupId != CC_SubDetails[Subjct].Group)
                    {
                        cell.Format.Font.Bold = true;
                        cell = row.Cells[0];
                        cell.Format.Alignment = ParagraphAlignment.Left;
                        cell.VerticalAlignment = VerticalAlignment.Center;
                        if (_GroupCount > 1)
                            cell.MergeDown = _GroupCount - 1;
                      
                        TextFrame Frame = cell.AddTextFrame();
                        Frame.Orientation = TextOrientation.Upward;                        
                        
                        Frame.Width = 12;
                        if (_GroupCount <= 0)
                            Frame.Height = _Rowheight;
                        else
                            Frame.Height = _GroupCount * _Rowheight;
                        
                        Paragraph para = Frame.AddParagraph(CC_SubDetails[Subjct].GroupName);
                        cell = row.Cells[1];
                        cell.Format.Font.Bold = true;
                        cell.VerticalAlignment = VerticalAlignment.Bottom;
                        cell.AddParagraph(CC_SubDetails[Subjct].Name);


                    }
                    else
                    {
                        cell = row.Cells[1];
                        cell.Format.Font.Bold = true;
                        cell.AddParagraph(CC_SubDetails[Subjct].Name);
                    }

                   
                }
                else
                {
                    cell = row.Cells[0];
                    cell.MergeRight = 2;
                    cell.Format.Font.Bold = true;
                    cell.AddParagraph(CC_SubDetails[Subjct].Name);
                }

                cell = row.Cells[2];
                cell.Format.Font.Bold = true;
                cell.Format.Alignment = ParagraphAlignment.Center;
                cell.AddParagraph("G");

                cell = row.Cells[3];
                cell.Format.Font.Bold = false;
                cell.Format.Alignment = ParagraphAlignment.Center;

                if (markNode[_ExamDetails.Length - 1, Subjct].MaxMark.ToString() == "0" || markNode[_ExamDetails.Length - 1, Subjct].MaxMark.ToString() == "-1")
                {
                    cell.AddParagraph("-");
                    _Temp = 1;
                }
                else
                {

                    cell.AddParagraph(GetGradeFromManrks(CC_ExamDetails[CC_ExamDetails.Length - 1].GradeMasterId ,m_CC_ExamMarks[CC_ExamDetails.Length - 1, Subjct].MaxMark, CC_SubDetails[Subjct].MaxMark));
                }

                if (Chk_needpercentage.Checked)//mani 29.8.2013
                {
                    cell = row.Cells[4];
                    cell.Format.Font.Bold = false;
                    cell.Format.Alignment = ParagraphAlignment.Center;

                    if (_Temp == 1)
                    {
                        cell.AddParagraph("-");
                    }
                    else
                    {
                        double persentage = (m_CC_ExamMarks[CC_ExamDetails.Length - 1, Subjct].MaxMark / CC_SubDetails[Subjct].MaxMark) * 100;
                        if (persentage == -1)
                            cell.AddParagraph("-");
                        else
                            cell.AddParagraph(persentage.ToString("0"));
                    }
                }

            }
            //CC EXAM END


            
            row = MarkRepot.AddRow();
            row.Format.Alignment = ParagraphAlignment.Center;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Format.Font = Font_Total;
            row.Format.Font.Bold = true;
            row.Height = 30;

            cell = row.Cells[0];
            cell.MergeRight = 1;
            cell.AddParagraph("TOTAL");

            cell = row.Cells[2];
            cell.AddParagraph(_MaxMark[_ExamDetails.Length - 1].ToString());

            cell = row.Cells[3];
            cell.AddParagraph(m_TotalMarks[_ExamDetails.Length - 1].MaxMark.ToString());

            if (Chk_needpercentage.Checked)//mani 29.8.2013
            {
                cell = row.Cells[4];
                cell.AddParagraph(Math.Round(m_Average[_ExamDetails.Length - 1]).ToString());
            }

            return MarkRepot;
          
        }

        private int GetTotalCountWithSameGroup(ExamNode[] _SubDetails, int _Id)
        {
            int idcount = 0;

            for(int _sub=0; _sub<_SubDetails.Length;_sub++)
            {
                if (_SubDetails[_sub].Group == _Id)
                    idcount++;
            }

            return idcount;
        }

        private string GetGradeFromManrks(int GradeId, double Mark, double MaxMark)
        {
            DataSet Grade = MyExamMang.GetGradeDataSet(GradeId);
            string _Grade = "";

            if (Grade != null && Grade.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Grade.Tables[0].Rows)
                {
                    double LowerLimit = (Mark / MaxMark) * 100;

                    if (LowerLimit >= double.Parse(dr["LowerLimit"].ToString()))
                    {
                        return _Grade= dr["Grade"].ToString();
                    }     
                }

                return _Grade;
            }
            return _Grade;
        }

        private DataSet GetStudDetails(int StudentId, int ClassId, int ExamId)
        {           
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
           

            string sql_Student = "select tblview_student.Id, tblview_student.StudentName, tblview_student.RollNo, tblclass.ClassName,date_Format(tblview_student.DOB,'%d/%m/%Y') as DOB , tblview_student.AdmitionNo from tblview_student inner join tblclass on tblclass.Id= tblview_student.ClassId inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblview_student.Id  where tblview_student.ClassId=" + ClassId + " and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblview_student.Id IN( select tblstudentmark.StudId from tblstudentmark inner join tblexamschedule on tblexamschedule.Id = tblstudentmark.ExamSchId inner join  tblclassexam on tblclassexam.Id=  tblexamschedule.ClassExamId where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblclassexam.ExamId= " + ExamId + " and tblexamschedule.Status='Completed'  )";
         
            if (StudentId != 0)
                sql_Student = sql_Student + " and tblview_student.Id=" + StudentId;



            DataSet _studList = MyExamMang .m_MysqlDb.ExecuteQueryReturnDataSet(sql_Student);

         
            foreach (DataRow dr_values in _studList.Tables[0].Rows)
            {

                dr = _StdentSet.Tables["Student"].NewRow();


                dr["Id"] = dr_values["Id"];
                dr["Name"] = dr_values["StudentName"];
                dr["RollNum"] = dr_values["RollNo"];
                dr["DOB"] = dr_values["DOB"];
                dr["Class"] = dr_values["ClassName"];
                dr["AdmissionNum"] = dr_values["AdmitionNo"];            
                dr["Attendance"] = "";


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
                if (_no_presentdays > 0 && _no_workingdays > 0)

                    dr_values["Attendance"] = _no_presentdays + "/" + _no_workingdays;
                else
                    dr_values["Attendance"] = "";
            }
            return _StdentSet;

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

        private void GetSubDetails(int ClassId, int ExamId, out ExamNode[] _SubDetails)
        {
            string sql = " SELECT  distinct  tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark,tblsubjects.sub_Catagory, tbltime_subgroup.Name  from tblclassexamsubmap inner JOIN tblsubjects on tblclassexamsubmap.SubId= tblsubjects.Id  inner JOIN tblexammark on  tblexammark.SubjectId=tblsubjects.Id  inner JOIN tblclassexam on tblclassexam.id= tblclassexamsubmap.ClassExamId   inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.id inner join tbltime_subgroup on tbltime_subgroup.Id= tblsubjects.sub_Catagory  where  tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "  and tblclassexam.ClassId=" + ClassId + " and tblclassexam.ExamId=" + ExamId + " Order By  tblsubjects.sub_Catagory , tblexammark.SubjectOrder ";  
           
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                int i = 0;
                int Count = MyReader.RecordsAffected;
                _SubDetails = new ExamNode[Count];

                while (MyReader.Read())
                {
                    _SubDetails[i].Id           = int.Parse(MyReader.GetValue(0).ToString());
                    _SubDetails[i].Name         = MyReader.GetValue(1).ToString();
                    _SubDetails[i].MaxMark      = double.Parse(MyReader.GetValue(2).ToString());
                    _SubDetails[i].Group        = int.Parse(MyReader.GetValue(3).ToString());
                    _SubDetails[i].GroupName    = MyReader.GetValue(4).ToString();
                    i++;
                }
            }
            else
            {
                _SubDetails = new ExamNode[0];
            }

        }

        private void GetExamDetails(int ClassId, int ExamId, out ExamNode[] _ExamDetails)
        {

            string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period,tblexamschedule.GradeMasterId from tblexammaster inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id     where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblexammaster.Id=" + ExamId + " and tblexamschedule.Status='Completed'  order by tblexamschedule.id desc limit 1";
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
                    _ExamDetails[i].GradeMasterId = int.Parse(MyReader["GradeMasterId"].ToString());
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

        private bool ValidateExams(out string _ErrMessage)
        {
            _ErrMessage = "";
            string sql = "";

            if (Drp_SelectClass.SelectedValue == "-1")
            {
                _ErrMessage = "No class found.";
                return false;
            }
            else if (Drp_SelectExam.SelectedValue == "-1")
            {
                _ErrMessage = "No exams found.";
                return false;
            }
            else if (Drp_SelectExam.SelectedValue == "0")
            {
                _ErrMessage = "Select one exam.";
                return false;
            }
            else if (Drp_SelectClass.SelectedValue != "-1" && Drp_SelectExam.SelectedValue != "-1" && Drp_SelectExam.SelectedValue != "0")
            {
                sql = " select tblexamschedule.Id from tblclassexam inner join  tblexamschedule on  tblexamschedule.ClassExamId= tblclassexam.id where tblclassexam.ClassId=" + Drp_SelectClass.SelectedValue + "  and tblclassexam.ExamId= " + Drp_SelectExam.SelectedValue + " and tblexamschedule.Status='Completed'";
                MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                if (!MyReader.HasRows)
                {
                    _ErrMessage = "Please complete the exam before creating report.";
                    return false;
                }
            }


            else if (Drp_SelectExam.SelectedValue != "-1" && Drp_SelectStudent.SelectedValue != "-1" && Drp_SelectStudent.SelectedValue != "0")
            {
                sql = "select tblstudentmark.StudId from tblstudentmark inner join tblexamschedule on tblexamschedule.Id = tblstudentmark.ExamSchId inner join  tblclassexam on tblclassexam.Id=  tblexamschedule.ClassExamId where tblclassexam.ClassId=" + Drp_SelectClass.SelectedValue + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblclassexam.ExamId= " + Drp_SelectExam.SelectedValue + " and tblexamschedule.Status='Completed' and tblstudentmark.StudId=" + Drp_SelectStudent.SelectedValue;
                MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                if (!MyReader.HasRows)
                {
                    _ErrMessage = "Selected student did not attend this exam.";
                    return false;
                }
            }
            return true;
        }

        private void LoadDetails()
        {
            LoadFont();
            LoadClassDetailsToDropDown();
            LoadExamDetailsToDropDown();
            LoadStudentsDetailsToDropDown();
            LoadSignaturesetting();
        }

        private void LoadFont()
        {
            string FontFormat = MyExamMang.GetFont();

            Font_All = new MigraDoc.DocumentObjectModel.Font(FontFormat, 14);
            Font_ReportName = new MigraDoc.DocumentObjectModel.Font(FontFormat, 16);
            Font_StudentDetails = new MigraDoc.DocumentObjectModel.Font(FontFormat, 14);
            Font_Heading = new MigraDoc.DocumentObjectModel.Font(FontFormat, 13);
            Font_ItemDetails = new MigraDoc.DocumentObjectModel.Font(FontFormat, 13);
            Font_Total = new MigraDoc.DocumentObjectModel.Font(FontFormat, 14);
            Font_BottomDetails = new MigraDoc.DocumentObjectModel.Font(FontFormat, 13);
            Font_BottomDetails1 = new MigraDoc.DocumentObjectModel.Font(FontFormat, 13);
            Font_BottomDetails2 = new MigraDoc.DocumentObjectModel.Font(FontFormat, 13);
        }

        private void LoadExamDetailsToDropDown()
        {

            if (Drp_SelectClass.SelectedValue != "-1")
            {
                Drp_SelectExam.Items.Clear();
                string sql = "select distinct tblexammaster.Id, tblexammaster.ExamName from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster ON tblexammaster.Id= tblclassexam.ExamId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id  where tblclassexam.ClassId = " + int.Parse(Drp_SelectClass.SelectedValue) + " and tblexamschedule.BatchId = " + MyUser.CurrentBatchId + " and tblexammaster.ExamTypeId=1 and tblexamschedule.status ='Completed'";
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
                // Up to Nationality Field in tblstudent
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
                    System.Web.UI.WebControls.ListItem li = new  System.Web.UI.WebControls.ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_SelectClass.Items.Add(li);
                }             
            }
            else
            {
                 System.Web.UI.WebControls.ListItem  li = new  System.Web.UI.WebControls.ListItem("No Class Found", "-1");
                Drp_SelectClass.Items.Add(li);
            }
        }

        protected void ChckedChanged(object sender, EventArgs e)
        {
            LoadSignaturesetting();
          
        }

        private void LoadSignaturesetting()
        {
            if (Chk_signature.Checked)
            {
                Label1.Visible = true;
                Txt.Visible = true;
            }
            else
            {
                Label1.Visible = false;
                Txt.Visible = false;
            }
        }

       

        
    }
}
