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
using PdfSharp.Pdf;
using System.Data;
using MigraDoc.DocumentObjectModel.Shapes;
using System.IO;

namespace WinEr
{

    public class ReportcardClass
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

        public ExamNode[] m_CC_Eaxams;
        public ExamNode[] m_CC_Subjects;
        public MarkNode[,] m_CC_ExamMarks;

        public MarkNode[] m_CC_TotalMarks;
        public MarkNode[] m_CC_Ranks;


        public string[] m_CC_Grade;
        public double[] m_CC_Average;
        public double[] m_CC_MaxTotal;



        public ReportcardClass(MysqlClass _Mysqlobj, int _StudentId)
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
                m_Remark = new string[_Eaxams.Length];

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
                m_StudDetails.Sex = _StudDetails[12].ToString();
                m_StudDetails.State = _StudDetails[11].ToString();
                m_StudDetails.Year = _StudDetails[10].ToString();


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

                    sqlMark = sqlMark + "tblstudentmark.TotalMark, tblstudentmark.TotalMax, tblstudentmark.Avg, tblstudentmark.Rank, tblstudentmark.Grade,tblstudentmark.Remark  from  tblstudentmark where  tblstudentmark.ExamSchId=" + _Eaxams[i].Id + " and  tblstudentmark.StudId=" + m_StudentId;
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
                                        double _Mark = 0;
                                        double.TryParse(Mark, out _Mark);
                                        m_ExamMarks[i, j].MaxMark = _Mark;

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

    public partial class ReportcardS2 : System.Web.UI.Page
    {
        private ExamManage MyExamMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private MysqlClass _Mysqlobj;
        private ClassOrganiser MyClassMngr;
        private SchoolClass objSchool = null;
        private string M_Logo = "";




        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
                Response.Redirect("Default.aspx");

            MyUser = (KnowinUser)Session["UserObj"];
            MyExamMang = MyUser.GetExamObj();
            MyClassMngr = MyUser.GetClassObj();

            if (MyExamMang == null)
                Response.Redirect("RoleErr.htm");
            if (!MyUser.HaveActionRignt(3041))
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
            LoadStudentsDetailsToDropDown();
        }

        protected void Btn_ExamReport_Click(object sender, EventArgs e)
        {
            Lbl_Err.Text = "";
            try
            {
                if (ValidEntries())
                {
                    
                   
                    if ("PRIMARY_SCHOOL_REPORT" == TypofReport())
                    {
                        CreatePrimarySchoolReport();
                    }
                    else
                    {
                        Lbl_Err.Text = "Select class";
                    }

                }

            }
            catch (Exception err)
            {
                Lbl_Err.Text = err.Message;
            }

        }



        #region Methods



        #endregion
        


        #region PRIMARY AREA

        private void CreatePrimarySchoolReport()
        {
            DataSet TotalStudList;
            DataSet totalMarkRatios = null;
            ExamNode[] _ExamDetails;
            ExamNode[] _SubDetails;

            SchoolDetails _SchoolDetails;
            ExamNode[] CC_ExamDetails = null;
            ExamNode[] CC_SubDetails = null;



            List<ReportcardClass> ClsObj = new List<ReportcardClass>();
            ReportcardClass _ReportObj;


            GetSchoolDetails(out _SchoolDetails);

            GetExamDetails(int.Parse(Drp_SelectClass.SelectedValue), "MAIN REPORT", out _ExamDetails);
            GetSubDetails(int.Parse(Drp_SelectClass.SelectedValue), "MAIN REPORT", out _SubDetails);

            if (_ExamDetails != null && _SubDetails != null && _ExamDetails.Length > 0 && _SubDetails.Length > 0)
            {
                TotalStudList = GetAllStudentListFromClass(int.Parse(Drp_SelectStudent.SelectedValue), int.Parse(Drp_SelectClass.SelectedValue));


                totalMarkRatios = GetMarkRatio(Drp_SelectClass.SelectedValue);


                foreach (DataRow Dr in TotalStudList.Tables[0].Rows)
                {
                    _ReportObj = new ReportcardClass(_Mysqlobj, int.Parse(Dr[0].ToString()));
                    _ReportObj.ExamDetails(_ExamDetails, _SubDetails, Dr, "Main");
                    //    _ReportObj.ExamDetails(CC_ExamDetails, CC_SubDetails, Dr, "CC");

                    ClsObj.Add(_ReportObj);
                }


                //  CreateReport(ClsObj, _ExamDetails, _SubDetails, CC_ExamDetails, CC_SubDetails, _SchoolDetails, int.Parse(Drp_SelectStudent.SelectedValue), totalMarkRatios);
                CreatePrimaryReport(ClsObj, _ExamDetails, _SubDetails, _SchoolDetails, int.Parse(Drp_SelectStudent.SelectedValue), totalMarkRatios);
            }
            else
            {
                Lbl_Err.Text = "Exam details not found";
            }
        }

        private void CreatePrimaryReport(List<ReportcardClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, SchoolDetails _SchoolDetails, int _StudId, DataSet totalMarkRatios)
        {
            string _PhysicalPath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));
            //int _StudentId = 0;
            Document document = LoadPrimaryClassPDFPage(ClsObj, _ExamDetails, _SubDetails, _SchoolDetails, totalMarkRatios);

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

            filename = _PhysicalPath + "\\PDF_Files\\ECP_" + MainName + ".pdf";

            pdfRenderer.PdfDocument.Save(filename);
            // ...and start a viewer.
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "null", "window.open(\"OpenPdfPage.aspx?PdfName=ECP_" + MainName + ".pdf\",'Info','status=1, width=1000, height=700,,resizable=1,scrollbars=1');", true);

            //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=PRC_" + MainName + ".pdf\");", true);
            //Response.Redirect("OpenPdfPage.aspx?PdfName=PRC_" + MainName + ".pdf", false);

        }

        private Document LoadPrimaryClassPDFPage(List<ReportcardClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, SchoolDetails _SchoolDetails, DataSet totalMarkRatios)

        {
            Document document = new Document();
            string path = GtSchoolLogo();
            //Dominic In bolosing school , they are using different remarks .
            int examFailCount = 0;
            int usingDifferentRemarks = 0;
            string mainRemark = "";
            double _doubleMinimumPersentage = 0;
            DataSet DifferentRemarks = null;

            if (UseDifferentRemarks(out _doubleMinimumPersentage))
            {
                usingDifferentRemarks = 1;
                DifferentRemarks = GetRemarks();
            }
            else
            {

                mainRemark = "Promoted To    ";
            }
            for (int i = 0; i < ClsObj.Count; i++)
            {
                PageSetup PgSt = document.DefaultPageSetup;
                PgSt.PageHeight = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(240);
                PgSt.PageWidth = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(297);

                PgSt.LeftMargin = 0;
                PgSt.RightMargin = 0;
                PgSt.TopMargin = 5;
                PgSt.BottomMargin = 0;

                PgSt.HeaderDistance = 0;
                PgSt.FooterDistance = 0;



                // Add a section to the document
                Section section = document.AddSection();

                MigraDoc.DocumentObjectModel.Tables.Table table = section.AddTable();



                table.Borders.Width = 1;
                //table.Borders.Left.Width = 0.5;
                //table.Borders.Right.Width = 0.5;
                table.Rows.LeftIndent = 25;
                MigraDoc.DocumentObjectModel.Tables.Column Col = table.AddColumn(PgSt.PageWidth - 50);
                MigraDoc.DocumentObjectModel.Tables.Row row = table.AddRow();
                row.Height = PgSt.PageHeight - 100;
                Col.Borders.Visible = true;

                MigraDoc.DocumentObjectModel.Tables.Cell cel = row.Cells[0];
                // Add a paragraph to the section
                Paragraph paragraph = section.AddParagraph();


                paragraph.Format.Alignment = ParagraphAlignment.Center;


                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;

                MigraDoc.DocumentObjectModel.Tables.Table tb = new MigraDoc.DocumentObjectModel.Tables.Table();


                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                tb.AddColumn((PgSt.PageWidth - 80) / 7);
                tb.AddColumn((PgSt.PageWidth - 80) - ((PgSt.PageWidth - 80) / 7));

                row = tb.AddRow();
                row.Cells[0].AddImage(path);
                row.Cells[0].MergeDown =3;

                row.Cells[1].Format.Font.Size = 12;
                row.Cells[1].Format.Font.Bold = true;
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[1].AddParagraph(MyUser.SchoolName.ToUpperInvariant());
                row = tb.AddRow();


                string _smallAddress = MyUser.GetSchoolSmallAdress();
                string[] Address = _smallAddress.Split('|');
                string _Address1 = "", _Address2 = "";
                if (Address.Length > 0)
                {
                    _Address1 = Address[0];
                }

                if (Address.Length > 1)
                {
                    _Address2 = Address[1];
                }


                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].AddParagraph(_Address1);
                row = tb.AddRow();
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].AddParagraph(_Address2);
                row = tb.AddRow();
                row.Cells[1].Format.Font.Size = 12;
                row.Cells[1].Format.Font.Bold = true;
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].AddParagraph("REPORT CARD S2");




                cel.Elements.Add(tb);

                
                tb = new MigraDoc.DocumentObjectModel.Tables.Table();


                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                tb.AddColumn((PgSt.PageWidth - 80) / 2);
                tb.AddColumn((PgSt.PageWidth - 80) / 4);
                tb.AddColumn((PgSt.PageWidth - 80) / 2);


                row = tb.AddRow();
                row.Cells[0].AddParagraph("Name: " + ClsObj[i].m_StudDetails.Name);
                row.Cells[1].AddParagraph("Class & Sec :" + ClsObj[i].m_StudDetails.Class);
                // row.Cells[2].AddParagraph("Roll No  :" + ClsObj[i].m_StudDetails.RollNum);
                row = tb.AddRow();

                tb.Borders.Visible = false;

                cel.Elements.Add(tb);

               

                tb = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                tb.AddColumn(((PgSt.PageWidth - 80) / 2)-90);
                tb.AddColumn(((PgSt.PageWidth - 80) / 3)-10);
                tb.AddColumn(((PgSt.PageWidth - 80) / 3));



                
                #region tbl

                MigraDoc.DocumentObjectModel.Tables.Table tb1 = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb1.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                tb1.AddColumn(((PgSt.PageWidth - 80) * 2 / 12)+15 );

                tb1.AddColumn(((PgSt.PageWidth - 80) / 12) - 20);
                tb1.AddColumn(((PgSt.PageWidth - 80) / 12) - 20);
             
                tb1.AddColumn(((PgSt.PageWidth - 80) / 12) - 15);

                row = tb1.AddRow();

                row.Cells[0].AddParagraph().AddFormattedText("Subjects");
                row.Cells[1].AddParagraph().AddFormattedText("Semester 1");
                row.Cells[2].AddParagraph().AddFormattedText("Semester 2");
                
                row.Cells[3].AddParagraph().AddFormattedText("Grand Total");

                row = tb1.AddRow();

                // dominic  Marks




                double _TotalMaxMark = 0;

                double[] _ExamWiseTotalMark = new double[_ExamDetails.Length];
                double[] _ExamWiseSubMark = new double[_SubDetails.Length];

                double[] _ExamWiseTotaMax = new double[_ExamDetails.Length];
                double[] _ExamWiseSubMax = new double[_SubDetails.Length];
                double[] _SubjectWiseMark = new double[_ExamDetails.Length];


                double MarksIn100 = 0;
                double MaxMarkForRemark = 0;
                examFailCount = 0;
                for (int Subjct = 0; Subjct < _SubDetails.Length; Subjct++)
                {
                    MaxMarkForRemark = 0;
                    row = tb1.AddRow();
                    row.Cells[0].AddParagraph().AddFormattedText(_SubDetails[Subjct].Name); //Subject Name

                    for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                    {

                        double max_Mark = MyExamMang.GetMaxMarkFromExamScheduleIdandSubjectId(_ExamDetails[ExamCount].Id, _SubDetails[Subjct].Id);


                        if (_ExamDetails[ExamCount].Name.Contains("semester 1"))
                        {
                            if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString() != "-1" && ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString() != "-2")
                            {
                               
                                row.Cells[1].AddParagraph().AddFormattedText(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                                _ExamWiseTotalMark[ExamCount] = _ExamWiseTotalMark[ExamCount] + double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                                if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark == 0)
                                    MarksIn100 = 0;
                                else
                                    MarksIn100 = ((double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString())) / max_Mark) * 100;
                                _ExamWiseSubMark[Subjct] = _ExamWiseSubMark[Subjct] + (double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString()));
                                _SubjectWiseMark[ExamCount] = _SubjectWiseMark[ExamCount] + MarksIn100;
                                MaxMarkForRemark = MaxMarkForRemark + MarksIn100;
                            }
                            else
                                row.Cells[1].AddParagraph().AddFormattedText("-");
                        }
                        if (_ExamDetails[ExamCount].Name.Contains("semester 2"))
                        {
                            
                            if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString() != "-1" && ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString() != "-2")
                            {
                                row.Cells[2].AddParagraph().AddFormattedText(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                                _ExamWiseTotalMark[ExamCount] = _ExamWiseTotalMark[ExamCount] + double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                                if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark == 0)
                                    MarksIn100 = 0;
                                else
                                    MarksIn100 = ((double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString())) / max_Mark) * 100;
                                if (MarksIn100 == -2)
                                {
                                    MarksIn100 = 0;
                                }


                                _ExamWiseSubMark[Subjct] = _ExamWiseSubMark[Subjct] + (double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString()));
                                _SubjectWiseMark[ExamCount] = _SubjectWiseMark[ExamCount] + MarksIn100;
                                MaxMarkForRemark = MaxMarkForRemark + MarksIn100;
                            }
                            else
                                row.Cells[2].AddParagraph().AddFormattedText("-");
                        }






                        _ExamWiseTotaMax[ExamCount] = _ExamWiseTotaMax[ExamCount] + max_Mark;
                        _ExamWiseSubMax[Subjct] = _ExamWiseSubMax[Subjct] + max_Mark;



                    }
                    double MaxMark = 100 * _ExamDetails.Length;
                    if (((MaxMarkForRemark / MaxMark) * 100) < _doubleMinimumPersentage)
                    {
                        examFailCount = examFailCount + 1;
                    }

                    MaxMarkForRemark = MaxMarkForRemark + MarksIn100;
                    row.Cells[3].AddParagraph().AddFormattedText(_ExamWiseSubMark[Subjct].ToString());


                }
              



 //Subject Name
                row = tb1.AddRow();
                row.Height = 15;
                row.Cells[0].AddParagraph();
                // row = tb1.AddRow();
                //  row = tb1.AddRow();


                row = tb1.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("JUMLAH MARKAH");
                row.Cells[0].AddParagraph().AddFormattedText("TOTAL SCORE");
                double Total_Mark = 0;
                for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                {

                    if (_ExamDetails[ExamCount].Name.Contains("semester 1"))
                    {
                        row.Cells[1].AddParagraph().AddFormattedText(_ExamWiseTotalMark[ExamCount].ToString());

                    }
                    else if (_ExamDetails[ExamCount].Name.Contains("semester 2"))
                    {
                        row.Cells[2].AddParagraph().AddFormattedText(_ExamWiseTotalMark[ExamCount].ToString());

                    }

                    Total_Mark = Total_Mark + _ExamWiseTotalMark[ExamCount];


                }

                row.Cells[3].AddParagraph().AddFormattedText(Total_Mark.ToString());
              
                row.Height = 10;
                row = tb1.AddRow();
                row.Height = 10;
                row.Cells[0].AddParagraph().AddFormattedText("TOLAK MARKAH KETIDKHADIRAN");
                row.Cells[0].AddParagraph().AddFormattedText("DEDUCTION FOR ABSENTEEISM");
                row = tb1.AddRow();
                row.Height = 10;
                row.Cells[0].AddParagraph().AddFormattedText("JUMLAH MARKAH SEBENAR");
                row.Cells[0].AddParagraph().AddFormattedText("ACTUAL TOTAL SCORE");
                double Total_Marks = 0;
                for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                {

                    if (_ExamDetails[ExamCount].Name.Contains("semester 1"))
                    {
                        row.Cells[1].AddParagraph().AddFormattedText(_ExamWiseTotalMark[ExamCount].ToString());

                    }
                    else if (_ExamDetails[ExamCount].Name.Contains("semester 2"))
                    {
                        row.Cells[2].AddParagraph().AddFormattedText(_ExamWiseTotalMark[ExamCount].ToString());

                    }

                    Total_Marks = Total_Mark + _ExamWiseTotalMark[ExamCount];


                }
                row = tb1.AddRow();


                row.Height = 10;
                row.Cells[0].AddParagraph().AddFormattedText("MARKAH PURATA");
                row.Cells[0].AddParagraph().AddFormattedText("AVERAGE SCORE");
                double _Per = 0, ToatlMark = 0, ToatalMax = 0;

                for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                {
                    //  _Per =( _SubjectWiseMark[ExamCount] / (_SubDetails.Length* 100))*100;
                    _Per = (_ExamWiseTotalMark[ExamCount] / (_ExamWiseTotaMax[ExamCount])) * 100;

                    if (_ExamDetails[ExamCount].Name.Contains("semester 1"))
                    {
                        row.Cells[1].AddParagraph().AddFormattedText(_Per.ToString("00.00"));

                    }
                    else if (_ExamDetails[ExamCount].Name.Contains("semester 2"))
                    {

                        row.Cells[2].AddParagraph().AddFormattedText(_Per.ToString("00.00"));

                    }
                   
                  

                    ToatlMark = ToatlMark + _ExamWiseTotalMark[ExamCount];
                    ToatalMax = ToatalMax + _ExamWiseTotaMax[ExamCount];

                }

                _Per = (ToatlMark / (ToatalMax)) * 100;

                row.Cells[3].AddParagraph().AddFormattedText(_Per.ToString("00.00"));

                row = tb1.AddRow();
                row.Height = 10;
                row.Cells[0].AddParagraph().AddFormattedText("KEDUDUKAN DALAM KELAS");
                row.Cells[0].AddParagraph().AddFormattedText("POSITION IN CLASS");
               // string sqls = "";
                for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                {
                    if (Drp_SelectStudent.SelectedIndex == 0)
                    {
                        string sqls = "Select tblstudentmark.Rank from  tblstudentmark where  tblstudentmark.ExamSchId =" + _ExamDetails[ExamCount].Id.ToString() + " and tblstudentmark.StudId =" + ClsObj[ExamCount].m_StudentId;
                        OdbcDataReader MyReaders = MyExamMang.m_MysqlDb.ExecuteQuery(sqls);

                        if (MyReaders.HasRows)
                        {
                            if (_ExamDetails[ExamCount].Name.Contains("semester 1"))
                            {
                                row.Cells[1].AddParagraph().AddFormattedText(MyReaders.GetValue(0).ToString());
                            }
                            if (_ExamDetails[ExamCount].Name.Contains("semester 1"))
                            {
                                row.Cells[2].AddParagraph().AddFormattedText(MyReaders.GetValue(0).ToString());
                            }



                        }
                    }
                    else
                    {
                        string sqls = "Select tblstudentmark.Rank from  tblstudentmark where  tblstudentmark.ExamSchId =" + _ExamDetails[ExamCount].Id.ToString() + " and tblstudentmark.StudId =" + Drp_SelectStudent.SelectedValue;



                        OdbcDataReader MyReaders = MyExamMang.m_MysqlDb.ExecuteQuery(sqls);

                        if (MyReaders.HasRows)
                        {
                            if (_ExamDetails[ExamCount].Name.Contains("semester 1"))
                            {
                                row.Cells[1].AddParagraph().AddFormattedText(MyReaders.GetValue(0).ToString());
                            }
                            if (_ExamDetails[ExamCount].Name.Contains("semester 1"))
                            {
                                row.Cells[2].AddParagraph().AddFormattedText(MyReaders.GetValue(0).ToString());
                            }



                        }
                    }
                }
                    row = tb1.AddRow();
                row.Height = 10;
                row.Cells[0].AddParagraph().AddFormattedText("BILANGAN PELAJAR DALAM KELAS");
                row.Cells[0].AddParagraph().AddFormattedText("MUMBER OF STUDENTS IN CLASS");
                string sql = "select DISTINCT count(tblstudent.id) from tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId = tblstudent.Id join tblbatch where tblbatch.status = 1  and tblstudent.LastClassId = "+Drp_SelectClass.SelectedValue;
                OdbcDataReader MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    //return _Reader.GetValue(0).ToString();
                    row.Cells[1].AddParagraph().AddFormattedText(MyReader.GetValue(0).ToString());
                    row.Cells[2].AddParagraph().AddFormattedText(MyReader.GetValue(0).ToString());
                }

               

                tb.Borders.Visible = false;
                cel.Elements.Add(tb);

                #endregion

                #region tbl2

                MigraDoc.DocumentObjectModel.Tables.Table tb2 = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb2.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);
                tb2.AddColumn(((PgSt.PageWidth - 80) / 12) + 10);
                tb2.AddColumn(((PgSt.PageWidth - 80) / 12) + 10);
                tb2.AddColumn(((PgSt.PageWidth - 80) / 12) + 10);
            
                row = tb2.AddRow();

                row.Cells[0].AddParagraph().AddFormattedText("KELAKUAN");

                row.Cells[0].AddParagraph().AddFormattedText("CONDUCT");
                row.Cells[1].AddParagraph().AddFormattedText("SEMESTER 1");
                row.Cells[2].AddParagraph().AddFormattedText("SEMESTER 2");
           //     row.Cells[0].AddParagraph().AddLineBreak();
                row = tb2.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("PUJIAN");
                row.Cells[0].AddParagraph().AddFormattedText("COMENDATIONS");
               row.Cells[0].AddParagraph().AddLineBreak();
                row = tb2.AddRow();

                row.Cells[0].AddParagraph().AddFormattedText("MERIT");
                row.Cells[0].AddParagraph().AddFormattedText("MERIT");
               row.Cells[0].AddParagraph().AddLineBreak();
                row = tb2.AddRow();

                row.Cells[0].AddParagraph().AddFormattedText("MERIT MULIA");
                row.Cells[0].AddParagraph().AddFormattedText("GREAT MERIT");
               row.Cells[0].AddParagraph().AddLineBreak();
                row = tb2.AddRow();

                row.Cells[0].AddParagraph().AddFormattedText("AMARAN");
                row.Cells[0].AddParagraph().AddFormattedText("WARNINGS");
             //   row.Cells[0].AddParagraph().AddLineBreak();
                row = tb2.AddRow();

                row.Cells[0].AddParagraph().AddFormattedText("DEMERIT");
                row.Cells[0].AddParagraph().AddFormattedText("DEMERIT");
               row.Cells[0].AddParagraph().AddLineBreak();
                row = tb2.AddRow();

                row.Cells[0].AddParagraph().AddFormattedText("DEMERIT SERIUS");
                row.Cells[0].AddParagraph().AddFormattedText("SERIOUS DEMERIT");
           //     row.Cells[0].AddParagraph().AddLineBreak();
                row = tb2.AddRow();

                row.Cells[0].AddParagraph().AddFormattedText("GRED KELAKUAN");
                row.Cells[0].AddParagraph().AddFormattedText("CONDUCT GRADE");
             //   row.Cells[0].AddParagraph().AddLineBreak();
               // row.Cells[0].AddParagraph().AddLineBreak();





                #endregion
                /////////////////////////////////
                #region tbl3

                MigraDoc.DocumentObjectModel.Tables.Table tb3 = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb3.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                
                tb3.AddColumn(((PgSt.PageWidth - 80) / 12) + 30);
                tb3.AddColumn(((PgSt.PageWidth - 80) / 12) + 10);
                tb3.AddColumn(((PgSt.PageWidth - 80) / 12) + 10);

              

                row = tb3.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("KEHADIRAN SEKOLAH");
                row.Cells[0].AddParagraph().AddFormattedText("SCHOOL ATTENDANCE");
                row.Cells[1].AddParagraph().AddFormattedText("SEMESTER 1");
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[2].AddParagraph().AddFormattedText("SEMESTER 2");
                row.Cells[2].Format.Alignment = ParagraphAlignment.Center;
               // row.Cells[0].AddParagraph().AddLineBreak();

                row = tb3.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("LEWAT");
                row.Cells[0].AddParagraph().AddFormattedText("LATE");
               // row.Cells[0].AddParagraph().AddLineBreak();
                row = tb3.AddRow();

                row.Cells[0].AddParagraph().AddFormattedText("PONTENG");
                row.Cells[0].AddParagraph().AddFormattedText("TRUANCY");
             //   row.Cells[0].AddParagraph().AddLineBreak();
                row = tb3.AddRow();

                row.Cells[0].AddParagraph().AddFormattedText("CUTI SAKIT");
                row.Cells[0].AddParagraph().AddFormattedText("SICK LEAVE");
             //   row.Cells[0].AddParagraph().AddLineBreak();
                row = tb3.AddRow();

                row.Cells[0].AddParagraph().AddFormattedText("CUTI URUSAN PERIBADI");
                row.Cells[0].AddParagraph().AddFormattedText("PERSONAL LEAVE");
                row.Cells[0].AddParagraph().AddLineBreak();
                row = tb3.AddRow();

                row.Cells[0].AddParagraph().AddFormattedText("CUTI URUSAN SEKOLAH");
                row.Cells[0].AddParagraph().AddFormattedText("SCHOOL OFFICIAL LEAVE");
            //    row.Cells[0].AddParagraph().AddLineBreak();
                row = tb3.AddRow();

                row.Cells[0].AddParagraph().AddFormattedText("CUTI URUSAN KHAS");

                row.Cells[0].AddParagraph().AddFormattedText("SPECIAL LEAVE");
               // row.Cells[0].AddParagraph().AddLineBreak();
                row = tb3.AddRow();

                row.Cells[0].AddParagraph().AddFormattedText("JUMLAH WAKTU KETIDAKHADIRAN");

                row.Cells[0].AddParagraph().AddFormattedText("TOTAL ABSENT PERIODS");
           //     row.Cells[0].AddParagraph().AddLineBreak();
               





                #endregion



                row = tb1.AddRow();
                row.Borders.Visible = false;
             





                MigraDoc.DocumentObjectModel.Tables.Table tb4 = new MigraDoc.DocumentObjectModel.Tables.Table();
                row.Cells[0].AddParagraph().AddLineBreak();
                tb4.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);


                tb4.AddColumn(((PgSt.PageWidth - 80)/2)-95);

          


                row = tb4.AddRow();
                row.Height = 40;
               
                row.Cells[0].AddParagraph().AddFormattedText("ULASAN GURU TINGKATAN");
                row.Cells[0].AddParagraph().AddFormattedText("FORM TEACHER'S COMMENTS");
                row.Cells[0].AddParagraph().AddLineBreak();


                tb.Borders.Visible = false;

                row = tb2.AddRow();
                row.Borders.Visible = false;
              

                MigraDoc.DocumentObjectModel.Tables.Table tb5 = new MigraDoc.DocumentObjectModel.Tables.Table();
                row.Cells[0].AddParagraph().AddLineBreak();
                tb5.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

              
                tb5.AddColumn(((PgSt.PageWidth - 80) / 3)+100);
                tb5.AddColumn((PgSt.PageWidth - 80) / 12);
                tb5.AddColumn((PgSt.PageWidth - 80) / 12);
               



                row = tb5.AddRow();
                row.Height = 25;

                row.Cells[0].AddParagraph().AddFormattedText("AKTIVITI-AKTIVITI KO-KURIKULUM");
                row.Cells[0].AddParagraph().AddFormattedText("CO-CURRICULAR ACTIVITIES");
                row.Cells[1].AddParagraph().AddFormattedText("SEMESTER 1");
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[2].AddParagraph().AddFormattedText("SEMESTER 2");
                row.Cells[2].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[0].AddParagraph().AddLineBreak();
                row = tb5.AddRow();
                row.Height = 30;

               // row.Cells[0].AddParagraph().AddFormattedText("0");
              //  row.Cells[0].AddParagraph().AddFormattedText("REMARKS");
                row.Cells[0].AddParagraph().AddLineBreak();



                tb.Borders.Visible = false;
                row = tb4.AddRow();
                row.Borders.Visible = false;
             

                MigraDoc.DocumentObjectModel.Tables.Table tb6 = new MigraDoc.DocumentObjectModel.Tables.Table();
                row.Cells[0].AddParagraph().AddLineBreak();
                tb6.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

               
                tb6.AddColumn(((PgSt.PageWidth - 80) / 12)-10 );
                tb6.AddColumn(((PgSt.PageWidth - 80) / 12) + 7);
                tb6.AddColumn(((PgSt.PageWidth - 80) / 12)-10);
                tb6.AddColumn(((PgSt.PageWidth - 80) / 12)-10);
                tb6.AddColumn(((PgSt.PageWidth - 80) / 12)-10);
              



                row = tb6.AddRow();
                row.Height = 70;

                row.Cells[0].AddParagraph().AddFormattedText("GURU TINGKATAN");
                row.Cells[0].AddParagraph().AddFormattedText("FORM TEACHER");
                row.Cells[1].AddParagraph().AddFormattedText("KOKURIKULUM");
                row.Cells[1].AddParagraph().AddFormattedText("CO-CURRICULUM");
                row.Cells[2].AddParagraph().AddFormattedText("HAL EHWAL PELAJAR");
                row.Cells[2].AddParagraph().AddFormattedText("DISCIPLINE MASTER");
                row.Cells[3].AddParagraph().AddFormattedText("HAL EHWAL AKADEMIK");
                row.Cells[3].AddParagraph().AddFormattedText("ACADEMIC");
                row.Cells[4].AddParagraph().AddFormattedText("PENGETUA");
                row.Cells[4].AddParagraph().AddFormattedText("PRINCIPAL");
                row.Cells[0].AddParagraph().AddLineBreak();
                row = tb6.AddRow();
                row.Height = 40;

               
                row.Cells[0].AddParagraph().AddLineBreak();



                tb.Borders.Visible = false;
                row = tb5.AddRow();
                row.Borders.Visible = false;


                MigraDoc.DocumentObjectModel.Tables.Table tb7 = new MigraDoc.DocumentObjectModel.Tables.Table();
                row.Cells[0].AddParagraph().AddLineBreak();
                tb7.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);


                tb7.AddColumn(((PgSt.PageWidth - 80) / 2)+100);
               




                row = tb7.AddRow();
                row.Height = 40;

                row.Cells[0].AddParagraph().AddFormattedText("ULASAN GURU TINGKATAN");
                row.Cells[0].AddParagraph().AddFormattedText("FORM TEACHER'S COMMENTS");
                row.Cells[0].AddParagraph().AddLineBreak();
               



                tb.Borders.Visible = false;




                tb.Borders.Visible = false;
                row = tb7.AddRow();
                row.Borders.Visible = false;
              

                MigraDoc.DocumentObjectModel.Tables.Table tb9 = new MigraDoc.DocumentObjectModel.Tables.Table();
                row.Cells[0].AddParagraph().AddLineBreak();
                tb9.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);


                tb9.AddColumn(((PgSt.PageWidth - 80) / 4));
                tb9.AddColumn(((PgSt.PageWidth - 80) / 2)-280);
                tb9.AddColumn(((PgSt.PageWidth - 80) / 10));
                tb9.AddColumn(((PgSt.PageWidth - 80) / 10));






                row = tb9.AddRow();
               

                row.Cells[0].AddParagraph().AddFormattedText("BIL. HARI BERSEKOLAH");
                row.Cells[0].AddParagraph().AddFormattedText("TOTAL SCHOOL DAYS");
               // row.Cells[0].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[1].AddParagraph().AddFormattedText("SEMESTER 1");
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;


                row.Cells[0].AddParagraph().AddLineBreak();
                row.Cells[0].Borders.Bottom.Visible = false;
                row = tb9.AddRow();
            

                row.Cells[0].AddParagraph().AddFormattedText("");
                row.Cells[0].AddParagraph().AddFormattedText("");
                row.Cells[1].AddParagraph().AddFormattedText("SEMESTER 2");
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[0].AddParagraph().AddLineBreak();


                row = tb6.AddRow();
                row.Borders.Visible = false;
               
                //MigraDoc.DocumentObjectModel.Tables.Table tb8 = new MigraDoc.DocumentObjectModel.Tables.Table();
                //row.Cells[0].AddParagraph().AddLineBreak();
                //tb8.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);


                //tb8.AddColumn(((PgSt.PageWidth - 80)));
               





                //row = tb8.AddRow();


                //row.Cells[0].AddParagraph().AddFormattedText("TANDATANGAN IBUBAPA");
                //row.Cells[0].AddParagraph().AddFormattedText("PARENT'S SIGNATURE _____________________________");
                //row.Cells[0].AddParagraph().AddLineBreak();


                //row = tb9.AddRow();
                //row.Borders.Visible = false;




                MigraDoc.DocumentObjectModel.Tables.Table tb10 = new MigraDoc.DocumentObjectModel.Tables.Table();
                row.Cells[0].AddParagraph().AddLineBreak();
                tb10.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);


                tb10.AddColumn(((PgSt.PageWidth - 80)));

                row = tb10.AddRow();


                row.Cells[0].AddParagraph().AddFormattedText("TANDATANGAN IBUBAPA");
                row.Cells[0].AddParagraph().AddFormattedText("PARENT'S SIGNATURE _____________________________");
                row.Cells[0].AddParagraph().AddLineBreak();


                /////////////////////////////////////
                row = tb.AddRow();
                tb1.Borders.Visible = true;
                row.Cells[0].Elements.Add(tb1);

                tb2.Borders.Visible = true;
                row.Cells[1].Elements.Add(tb2);

                tb3.Borders.Visible = true;
                row.Cells[2].Elements.Add(tb3);


                tb4.Borders.Visible = true;
                row.Cells[0].Elements.Add(tb4);
                tb5.Borders.Visible = true;
                row.Cells[1].Elements.Add(tb5);

                tb6.Borders.Visible = true;
                row.Cells[0].Elements.Add(tb6);
                tb7.Borders.Visible = true;
                row.Cells[1].Elements.Add(tb7);

                tb9.Borders.Visible = true;
                row.Cells[1].Elements.Add(tb9);

                //tb8.Borders.Visible = false;
                //row.Cells[0].Elements.Add(tb8);
                tb10.Borders.Visible = false;
                row.Cells[1].Elements.Add(tb10);



            }









            return document;
        }



       






        #endregion PRIMARY AREA


        #region CommonMethods

        private string GetClassDivision(double _Per)
        {
            string sql = "select tblexamdivisionpersentage.Division, tblexamdivisionpersentage.MinPersentage  from tblexamdivisionpersentage ";
            DataSet Division = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (Division != null && Division.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Division.Tables[0].Rows)
                {
                    if (_Per >= double.Parse(dr[1].ToString()))
                    {
                        return dr[0].ToString();
                    }
                }
            }
            return "";

        }

        private string GetNextClass()
        {
            int _ClassId = 0;
            int.TryParse(Drp_SelectClass.SelectedValue, out _ClassId);

            string sql = " select tblstandard.Name from tblclass inner join tblstandard on  tblstandard.Id= tblclass.Standard+1 where tblclass.Id=" + _ClassId;
            //   string sql = "select tblstandard.name from tblstandard where tblstandard.Id=" + _ClassId;
            OdbcDataReader _Reader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (_Reader.HasRows)
            {
                return _Reader.GetValue(0).ToString();
            }
            else return "";

        }

        private DataSet GetRemarks()
        {
            string sql = "select tblexamremark.ExamCount, tblexamremark.Remarks from tblexamremark where tblexamremark.Active=1";
            DataSet _RemarksDetails = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return _RemarksDetails;
        }


        private bool UseDifferentRemarks(out double _doubleMinimumPersentage)
        {
            _doubleMinimumPersentage = 0;
            bool differentRemarks = false; ;
            int Val = 0;
            string sql = "select tblconfiguration.Value, tblconfiguration.SubValue from tblconfiguration where tblconfiguration.Name='Remarks'  and Module='Exam Report'";

            OdbcDataReader _Reader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (_Reader.HasRows)
            {
                if (int.TryParse(_Reader.GetValue(0).ToString(), out Val))
                {
                    if (Val == 1)
                    {
                        differentRemarks = true;
                        double.TryParse(_Reader.GetValue(1).ToString(), out _doubleMinimumPersentage);
                    }
                }
            }
            return differentRemarks;
        }

        private string GtSchoolLogo()
        {
            String ImageUrl = "";


            ImageUploaderClass imgobj = new ImageUploaderClass(objSchool);
            byte[] img_bytes = imgobj.getImageBytes(objSchool.SchoolId, "Logo");
            M_Logo = MyUser.FilePath + "/ThumbnailImages/" + objSchool.SchoolId + "_" + System.DateTime.Now.Millisecond + ".jpg";


            File.WriteAllBytes(M_Logo, img_bytes);
            ImageUrl = M_Logo;
            return ImageUrl;



        }

        private double GetMarkRations(DataSet totalMarkRatios, string ExamName, double Mark, out double Ratio)
        {
            double _Mark = 0;
            Ratio = 0;
            string sql = "SELECT tblcbseexamratiomap.RatioColumName from tblcbseexamratiomap where tblcbseexamratiomap.ExamName='" + ExamName + "'";
            OdbcDataReader MyReader = MyClassMngr.m_MysqlDb.ExecuteQuery(sql);

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


        private string TypofReport()
        {
            int standard = 0;
            int _ClassId = 0;
            int fromclassconfi = 0;
            int toclassconfi = 0;
            string sqlstandard = " select tblconfiguration.Value,tblconfiguration.SubValue from tblconfiguration where tblconfiguration.Name='StandardId' and tblconfiguration.Module='Report'";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sqlstandard);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out fromclassconfi);
                int.TryParse(MyReader.GetValue(1).ToString(), out toclassconfi);

            }
            string sql = "select tblclass.Standard from tblclass WHERE tblclass.id=" + Drp_SelectClass.SelectedValue;
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);



            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out standard);
                if (standard > fromclassconfi && standard < toclassconfi)
                {
                    return "NEW_REPORT";
                }
               
                else
                {
                    return "PRIMARY_SCHOOL_REPORT";
                }
            }
            return "";
        }

        private DataSet GetMarkRatio(string _selectedClassId)
        {
            string stdId = MyClassMngr.GetStandardIdfromClassId(_selectedClassId);
            string sqlRation = "select FA1,FA2,SA1,FA3,FA4,SA2 from tblcbsegraderatio where tblcbsegraderatio.StandardId=" + stdId;
            DataSet Dt = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sqlRation);
            return Dt;


        }

        private void GetSubDetailsCC(int ClassId, int ExamId, out ExamNode[] _SubDetails)
        {
            string sql = " SELECT  distinct  tblsubjects.Id,tblsubjects.subject_name, tblsubjects.sub_Catagory, tbltime_subgroup.Name ,tblsubjects.sub_description from tblclassexamsubmap inner JOIN tblsubjects on tblclassexamsubmap.SubId= tblsubjects.Id inner JOIN tblclassexam on tblclassexam.id= tblclassexamsubmap.ClassExamId   inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.id  inner join   tblexammark on tblexammark.ExamSchId=tblexamschedule.Id  inner join tbltime_subgroup on tbltime_subgroup.Id= tblsubjects.sub_Catagory  where  tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "  and tblclassexam.ClassId=" + ClassId + " and tblclassexam.ExamId=" + ExamId + " order by tblexammark.SubjectOrder ";

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
            dt.Columns.Add("Year");
            dt.Columns.Add("State");
            dt.Columns.Add("Sex");

            string sql_Student = "select tblview_student.Id, tblview_student.StudentName, tblview_student.RollNo, tblclass.ClassName,date_Format(tblview_student.DOB,'%d/%m/%Y') as DOB , tblview_student.AdmitionNo,tblview_student.GardianName,tblview_student.MothersName, tblview_student.ResidencePhNo ,tblview_student.Address ,tblview_student.sex ,tblbatch.BatchName ,tblview_student.state  from tblview_student inner join tblclass on tblclass.Id= tblview_student.ClassId inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblview_student.Id  inner join tblbatch on tblbatch.id= tblview_student.JoinBatch  where tblview_student.ClassId=" + ClassId + " and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId;

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

                dr["Year"] = dr_values["BatchName"];
                dr["State"] = dr_values["state"];
                dr["Sex"] = dr_values["sex"];
                _StdentSet.Tables["Student"].Rows.Add(dr);

            }
            return _StdentSet;


        }

        private void GetCC_SubDetails(int ClassId, string ExamType, out ExamNode[] CC_SubDetails)
        {
            //string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=tblexamschedule.ClassExamId and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId  where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblclassexam.ClassId=" + ClassId + " and tblexammaster.Id=" + ExamId;
            string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark ,tblsubjects.sub_description , tblsubjects.sub_Catagory, tbltime_subgroup.Name from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id  inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId   inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId   inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=tblexamschedule.ClassExamId   and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId   inner join tbltime_subgroup on tbltime_subgroup.Id= tblsubjects.sub_Catagory where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "   and tblexammaster.Id     in (select tblexammaster.id from tblsubject_type  inner join tblexammaster on tblexammaster.ExamTypeId=tblsubject_type.Id  inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id   inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id    where tblclassexam.ClassId=" + ClassId + "  and   tblsubject_type.TypeDisc='" + ExamType + "' and tblexamschedule.Status='Completed'  order by tblexammark.SubjectOrder )";
            OdbcDataReader MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int i = 0;
                int Count = MyReader.RecordsAffected;
                CC_SubDetails = new ExamNode[Count];

                while (MyReader.Read())
                {
                    CC_SubDetails[i].Id = int.Parse(MyReader.GetValue(0).ToString());
                    CC_SubDetails[i].Name = MyReader.GetValue(1).ToString();
                    CC_SubDetails[i].MaxMark = double.Parse(MyReader.GetValue(2).ToString());
                    CC_SubDetails[i].Desc = MyReader.GetValue(3).ToString();
                    CC_SubDetails[i].Group = int.Parse(MyReader.GetValue(4).ToString());
                    CC_SubDetails[i].GroupName = MyReader.GetValue(5).ToString();

                    i++;
                }
            }
            else
            {
                CC_SubDetails = new ExamNode[0];
            }

        }

        private void GetExamDetails(int ClassId, string ExamType, out ExamNode[] _ExamDetails)
        {
            //string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period from tblexammaster inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id     where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblexammaster.Id=" + ExamId + " and tblexamschedule.Status='Completed'  order by tblexamschedule.id desc";
            string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period, tblexamschedule.GradeMasterId from tblsubject_type  inner join tblexammaster on tblexammaster.ExamTypeId=tblsubject_type.Id   inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id     where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and  tblsubject_type.TypeDisc='MAIN REPORT' and tblexamschedule.Status='Completed'  order by  tblexammaster.ExamOrder ";
            OdbcDataReader MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

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

        private void GetSubDetails(int ClassId, string ExamType, out ExamNode[] _SubDetails)
        {
            //string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=tblexamschedule.ClassExamId and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId  where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblclassexam.ClassId=" + ClassId + " and tblexammaster.Id=" + ExamId;
            string sql = " SELECT distinct tblsubjects.Id,tblsubjects.subject_name,tblsubjects.sub_description from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id  inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId   inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId   inner join tblclassexamsubmap on tblclassexamsubmap. ClassExamId=tblexamschedule.ClassExamId   and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id=  .tblclassexam.ExamId Inner join tblsubject_type on  tblexammaster.ExamTypeId=tblsubject_type.Id   where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "  and tblclassexam.ClassId=" + ClassId + "  and     tblsubject_type.TypeDisc='MAIN REPORT' and tblexamschedule.Status='Completed'  order by tblexammark.SubjectOrder  ";

            OdbcDataReader MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int i = 0;
                int Count = MyReader.RecordsAffected;
                _SubDetails = new ExamNode[Count];

                while (MyReader.Read())
                {
                    _SubDetails[i].Id = int.Parse(MyReader.GetValue(0).ToString());
                    _SubDetails[i].Name = MyReader.GetValue(1).ToString();

                    _SubDetails[i].Desc = MyReader.GetValue(2).ToString();
                    i++;
                }
            }
            else
            {
                _SubDetails = new ExamNode[0];
            }

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
            LoadStudentsDetailsToDropDown();
        }

        private bool ValidEntries()
        {
            if (Drp_SelectClass.SelectedValue == "-1")
            {
                Lbl_Err.Text = "No class found.";
                return false;
            }
            else if (Txt_ReportName.Text == "")
            {
                Lbl_Err.Text = "Enter Report Name.";
                return false;
            }

            return true;
        }

        private string GetExamTypeName(string _ExamDescription, out int _SubTypeId)
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

        private void LoadStudentsDetailsToDropDown()
        {
            if (Drp_SelectClass.SelectedValue != "-1")
            {
                int ClassId = 0;
                int.TryParse(Drp_SelectClass.SelectedValue.ToString(), out ClassId);

                Drp_SelectStudent.Items.Clear();
                string sql = " SELECT stud.Id, stud.StudentName FROM tblstudentclassmap map inner join tblstudent stud on stud.id= map.Studentid where stud.status=1 and  map.ClassId=" + ClassId + " and map.RollNo<>-1 and map.BatchId=" + MyUser.CurrentBatchId + " order by  stud.StudentName ASC";
                MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

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
        #endregion

        
    }

    }
