using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using WinBase;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using System.IO;


namespace WinEr
{
    public class AssessemntReportClass
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




        public AssessemntReportClass(MysqlClass _Mysqlobj, int _StudentId)
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
                m_StudDetails.Attendance = _StudDetails[10].ToString();


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

                string _sqlMarksColum = "select tblexammark.MarkColumn, tblexammark.SubjectId  from tblexammark where tblexammark.ExamSchId=" + _Eaxams[i].Id +" ORDER BY tblexammark.SubjectOrder";
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


    public partial class AssesmentReport : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private ExamManage MyExamMang;
        private MysqlClass _Mysqlobj;
        private Attendance MyAttendence;
        private SchoolClass objSchool = null;
        private string M_Logo = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MyExamMang = MyUser.GetExamObj();
            MyAttendence = MyUser.GetAttendancetObj();
           // _Mysqlobj = new MysqlClass(MyUser.ConnectionString);
            if (MyExamMang == null)
                Response.Redirect("RoleErr.htm");


            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
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
                    LoadDefaultValues();
                }
            }
        }

        private void LoadDefaultValues()
        {
            LoadClassDetails();
            LoadStudentDetails();
        }

        private void LoadStudentDetails()
        {
            //drp_Student
            int ClassId = 0;
            int.TryParse(Drp_ClassSelect.SelectedValue.ToString(), out ClassId);
            drp_Student.Items.Clear();
            string Sql = "select tblview_student.Id, tblview_student.StudentName from tblview_student INNER join tblclass on tblview_student.ClassId=tblclass.Id where tblview_student.ClassId=" + ClassId + " and  tblview_student.LIve=1  and tblview_student.RollNo<>-1  order by  tblview_student.StudentName ASC";
            // Up to Nationality Field in tblstudent
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("All Student", "0");
                drp_Student.Items.Add(li);

                while (MyReader.Read())
                {
                    ListItem Li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    drp_Student.Items.Add(Li);
                }

                drp_Student.SelectedValue = "0";

            }
            else
            {
                ListItem li = new ListItem("No student found", "-1");
                drp_Student.Items.Add(li);
            }
        }

        private void LoadClassDetails()
        {

            //Load all class and select one class if _ClassId!=0
            Drp_ClassSelect.Items.Clear();
            string sql = "SELECT tblclass.Id,tblclass.ClassName FROM tblclass where tblclass.Status =1 AND tblclass.Id IN (SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ")) ORDER BY tblclass.Standard,tblclass.ClassName";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {  
                ListItem li;
                li = new ListItem("Select Class", "0");
                Drp_ClassSelect.Items.Add(li);
              
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_ClassSelect.Items.Add(li);
                }

                
                Drp_ClassSelect.SelectedValue = "0";
            }
            else
            {
                ListItem li = new ListItem("No Class Found", "-1");
                Drp_ClassSelect.Items.Add(li);
            }
            LoadExams(Drp_ClassSelect.SelectedValue);
        }


        private void LoadExams(string ClassId)
        {
            int Classid = Convert.ToInt32(ClassId);
            Drp_ExamType.Items.Clear();
            if (Classid > -1)
            {
                Drp_ExamType.Items.Clear();

                string sql = "select tblsubject_type.Id, tblsubject_type.sbject_type from tblsubject_type where TypeDisc='QUARTERLY'";
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);

                if (MyReader.HasRows)
                {
                   
                    Drp_ExamType.Items.Add(new ListItem("Select any exam type", "0"));

                    while (MyReader.Read())
                    {
                        ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_ExamType.Items.Add(li);
                    }
                }
            }
            else
            {
                ListItem li = new ListItem("No exam type found", "-1");
                Drp_ExamType.Items.Add(li);
            }
            
        }

        protected void Btn_Generate_Click(object sender, EventArgs e)
        {
            try
            {
                string Msg = "";

                if (ValidInputs(out Msg))
                {
                    if (!PrepareReport(out  Msg))
                    {
                        Lbl_Message.Text = Msg;
                    }
                }
                Lbl_Message.Text = Msg;

            }
            catch (Exception e1)
            {
                Lbl_Message.Text = e1.Message;
            }

        }

        private bool PrepareReport(out string Msg)
        {
            DataSet dt_exams=null;
            Msg="";
            if(ExamsExist(out dt_exams,out Msg))
            {
                if (dt_exams != null && dt_exams.Tables != null && dt_exams.Tables[0].Rows.Count > 0)
                {
                    DataSet ExamDisplayMap = GetExamDisplayMap();

                    if (ExamDisplayMap != null && ExamDisplayMap.Tables != null && ExamDisplayMap.Tables[0].Rows.Count > 0)
                    {

                        if (CreateExamReports(ExamDisplayMap, out Msg))
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        Msg = "Exam report configuration missing.";
                        return false;
                    }

                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        private bool CreateExamReports(DataSet ExamDisplayMap, out string Msg)            
        {

            DataSet TotalStudList;
            
            ExamNode[] _ExamDetails;
            ExamNode[] _SubDetails;
            
            SchoolDetails _SchoolDetails;
            ExamNode[] CC_ExamDetails = null;
            ExamNode[] CC_SubDetails = null;
            


            List<AssessemntReportClass> ClsObj = new List<AssessemntReportClass>();
            AssessemntReportClass _ReportObj;


            GetSchoolDetails(out _SchoolDetails);

            GetExamDetails(int.Parse(Drp_ClassSelect.SelectedValue), int.Parse(Drp_ExamType.SelectedValue), out _ExamDetails);
            GetSubDetails(int.Parse(Drp_ClassSelect.SelectedValue), int.Parse(Drp_ExamType.SelectedValue), out _SubDetails);
            if (_ExamDetails != null && _SubDetails != null && _ExamDetails.Length > 0 && _SubDetails.Length > 0)
            {
                TotalStudList = GetAllStudentListFromClass(int.Parse(drp_Student.SelectedValue.ToString()), int.Parse(Drp_ClassSelect.SelectedValue));

                GetExamDetailsCC(int.Parse(Drp_ClassSelect.SelectedValue), int.Parse(Drp_ExamType.SelectedValue), out CC_ExamDetails);
                GetSubDetailsCC(int.Parse(Drp_ClassSelect.SelectedValue), int.Parse(Drp_ExamType.SelectedValue), out CC_SubDetails);


               
                foreach (DataRow Dr in TotalStudList.Tables[0].Rows)
                {
                    _ReportObj = new AssessemntReportClass(_Mysqlobj, int.Parse(Dr[0].ToString()));
                    _ReportObj.ExamDetails(_ExamDetails, _SubDetails, Dr, "Main");
                    _ReportObj.ExamDetails(CC_ExamDetails, CC_SubDetails, Dr, "CC");


                    ClsObj.Add(_ReportObj);
                }


                CreateReport(ClsObj, _ExamDetails, _SubDetails, CC_ExamDetails, CC_SubDetails, _SchoolDetails, 0, ExamDisplayMap);
                Msg = "";
                return true;
               
            }
            else
            {
                Msg = "Exam details not found";
                return false;
            }
           
        }

        private void CreateReport(List<AssessemntReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, ExamNode[] CC_ExamDetails, ExamNode[] CC_SubDetails, SchoolDetails _SchoolDetails, int _StudId, DataSet ExamDisplayDetails)
        {
           
                string _PhysicalPath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath(""));
                //int _StudentId = 0;
                Document document = LoadPDFPage(ClsObj, _ExamDetails, _SubDetails, _SchoolDetails, CC_ExamDetails, CC_SubDetails, ExamDisplayDetails);

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
                    MainName = Drp_ClassSelect.SelectedItem.ToString();
                else
                    MainName = drp_Student.SelectedItem.Text.ToString()+drp_Student.SelectedValue.ToString();


                filename = _PhysicalPath + "\\PDF_Files\\ASR_" + MainName + ".pdf";

                pdfRenderer.PdfDocument.Save(filename);
                // ...and start a viewer.
                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "null", "window.open(\"OpenPdfPage.aspx?PdfName=ASR_" + MainName + ".pdf\",'Info','status=1, width=1000, height=700,,resizable=1,scrollbars=1');", true);

                //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=PRC_" + MainName + ".pdf\");", true);
                // Response.Redirect("OpenPdfPage.aspx?PdfName=PRC_" + MainName + ".pdf", false);
                if (!String.IsNullOrEmpty(M_Logo))
                {
                    File.Delete(M_Logo);
                }
        }

        private Document LoadPDFPage(List<AssessemntReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, SchoolDetails _SchoolDetails, ExamNode[] CC_ExamDetails, ExamNode[] CC_SubDetails, DataSet ExamDisplayDetails)
        {

            Document document = new Document();

            for (int i = 0; i < ClsObj.Count; i++)
            {

                PageSetup PgSt = document.DefaultPageSetup;
                //PgSt.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.A5;
                PgSt.PageWidth = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(148);
                PgSt.PageHeight = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(210);
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
                paragraph.AddLineBreak();

                paragraph.AddLineBreak();


                MigraDoc.DocumentObjectModel.Tables.Table table = section.AddTable();


                table.Borders.Width = 0;

                table.Rows.LeftIndent = 25;

                MigraDoc.DocumentObjectModel.Tables.Column Col = table.AddColumn(PgSt.PageWidth - 50);
                MigraDoc.DocumentObjectModel.Tables.Row row = table.AddRow();
                row.Height = PgSt.PageHeight -100;
               

                MigraDoc.DocumentObjectModel.Tables.Cell cel = row.Cells[0];

                //paragraph = section.AddParagraph();

                MigraDoc.DocumentObjectModel.Tables.Table tb = new MigraDoc.DocumentObjectModel.Tables.Table();
                tb.Rows.LeftIndent = 10;

                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);


                tb.AddColumn((PgSt.PageWidth - 65) / 5);
                tb.AddColumn((PgSt.PageWidth - 65) * 4 / 5);
             

                row = tb.AddRow();
             

               //  row.Cells[0]
              //  row.Cells[0].AddImage(image);

                // Arun Need to check code
                row.Cells[0].MergeDown = 2;

                ImageUploaderClass imgobj  = new ImageUploaderClass(objSchool);                
                byte[] img_bytes= imgobj.getImageBytes(objSchool.SchoolId,"Logo");
                M_Logo = MyUser.FilePath + "/ThumbnailImages/" + objSchool.SchoolId + "_" + System.DateTime.Now.Millisecond + ".jpg";


                File.WriteAllBytes(M_Logo, img_bytes);


                //_space.AddImage(M_Logo).Width = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(50).Millimeter;





                row.Cells[0].AddImage(M_Logo).Width = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(50).Millimeter;
                

                paragraph = row.Cells[1].AddParagraph();

                MigraDoc.DocumentObjectModel.Font font = new MigraDoc.DocumentObjectModel.Font("Benguiat Bk BT", 14);
                paragraph.Format.Font = font;
                paragraph.Format.Alignment = ParagraphAlignment.Center;
                paragraph.AddFormattedText(MyUser.SchoolName, TextFormat.Bold);



                row = tb.AddRow();

                paragraph = row.Cells[1].AddParagraph();

                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);
                paragraph.Format.Alignment = ParagraphAlignment.Center;
                paragraph.AddFormattedText(_SchoolDetails.Address, TextFormat.NotBold);
                paragraph.AddFormattedText(" ");

                row = tb.AddRow();
                paragraph.AddLineBreak();
                paragraph = row.Cells[1].AddParagraph();

                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 11);
                paragraph.Format.Alignment = ParagraphAlignment.Center;
                string[] Array = MyUser.CurrentBatchName.ToString().Split('-');
                paragraph.AddFormattedText(GetReportName() + "  " + Array[0] , TextFormat.Bold | TextFormat.Underline);
                paragraph.AddLineBreak();
                tb.Borders.Visible = false;  
             
                cel.Elements.Add(tb);

                tb = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                tb.AddColumn((PgSt.PageWidth - 65) * 2 / 3);
                tb.AddColumn((PgSt.PageWidth - 65) / 5);
                tb.AddColumn((PgSt.PageWidth - 65) / 5);

                row = tb.AddRow();

                row.Cells[0].AddParagraph("Name     : " + ClsObj[i].m_StudDetails.Name);
                row.Cells[1].AddParagraph("Roll No  : " + ClsObj[i].m_StudDetails.RollNum);
                row = tb.AddRow();

                row.Cells[0].AddParagraph("Attendance : " + ClsObj[i].m_StudDetails.Attendance);
                row.Cells[1].AddParagraph("Class      : " + ClsObj[i].m_StudDetails.Class);
                tb.Borders.Visible = false;
                cel.Elements.Add(tb);

                // main exam area


                
                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.AddLineBreak();
              
                tb = new MigraDoc.DocumentObjectModel.Tables.Table();
             
                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 11);

                tb.AddColumn(((PgSt.PageWidth - 60) / 12)-10);
                tb.AddColumn((PgSt.PageWidth - 60) * 3 / 12);
                tb.AddColumn((PgSt.PageWidth - 60) / 12);
                tb.AddColumn((PgSt.PageWidth - 60) / 12);
                tb.AddColumn((PgSt.PageWidth - 60) / 12);
                tb.AddColumn((PgSt.PageWidth - 60) / 12);
                tb.AddColumn((PgSt.PageWidth - 60) / 12);
                tb.AddColumn((PgSt.PageWidth - 60) / 12);
                tb.AddColumn((PgSt.PageWidth - 60) / 12);
                tb.AddColumn((PgSt.PageWidth - 60) / 12);

                row = tb.AddRow();
               
                row.Cells[0].AddParagraph("Sl. No.");
                row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[0].Format.Font.Bold = false;
                row.Cells[0].MergeDown = 1;
                
                row.Cells[1].AddParagraph("Subjects");
                row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].Format.Font.Bold = false;
                row.Cells[1].MergeDown = 1;
                
                int k = 2;
                int TempColumn = 0;
               
                if (_ExamDetails.Length > 0)
                {


                    for (int j = 0; j < _ExamDetails.Length; j++)
                    {

                        if (_ExamDetails[j].GroupName != "MINER")
                        {
                          
                            if (_ExamDetails[j].GroupName == "EXTERNAL")
                            {
                                row.Cells[k].Format.Alignment = ParagraphAlignment.Right;
                                TextFrame Frame = row.Cells[k].AddTextFrame();
                               

                                row.Cells[k].Format.Font.Bold = false;
                                Frame.Orientation = TextOrientation.Upward;
                                Frame.Width = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1);
                          

                            
                                Paragraph para = Frame.AddParagraph(GetExamName(_ExamDetails[j].Name, ExamDisplayDetails));
                                para.Format.Alignment = ParagraphAlignment.Center;
                                para.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 11);
                                para.Format.Font.Bold = false;
                               
                                para.Format.Alignment = ParagraphAlignment.Center;
                            }
                            else if (_ExamDetails[j].GroupName.Contains("INTERNAL"))
                            {
                                TextFrame Frame = row.Cells[k].AddTextFrame();
                              
                                row.Cells[k].Format.Alignment = ParagraphAlignment.Center;
                                row.Cells[k].Format.Font.Bold = false;

                                Frame.Orientation = TextOrientation.Upward;
                                Frame.Width = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1);

                          
                                Paragraph para = Frame.AddParagraph(GetExamName(_ExamDetails[j].Name, ExamDisplayDetails));
                                para.Format.Alignment = ParagraphAlignment.Center;
                                para.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 11);
                                para.Format.Font.Bold = false;
                             
                                para.Format.Alignment = ParagraphAlignment.Center;

                               
                               
                            }

                           
                            k = k + 1;
                        }
                    }

                    TextFrame Frame1 = row.Cells[6].AddTextFrame();
                    row.Cells[6].Format.Alignment = ParagraphAlignment.Center;
                    Frame1.Orientation = TextOrientation.Upward;
                    Frame1.Width = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1);
                                       
                    Paragraph para1 = Frame1.AddParagraph("Total");
           
                    para1.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 11);
                    para1.Format.Font.Bold = false;
                 
                    para1.Format.Alignment = ParagraphAlignment.Center;





                    Frame1 = row.Cells[7].AddTextFrame();
                    row.Cells[7].Format.Alignment = ParagraphAlignment.Center;
                    Frame1.Orientation = TextOrientation.Upward;
                    Frame1.Width = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1);

                    //Frame.Height = _GroupCount * _Rowheight;
                    //Frame.MarginLeft=
                 
                    para1 = Frame1.AddParagraph("Grade");
                    //para.Format.SpaceBefore = Unit.FromCm(0.4);
                    para1.Format.Alignment = ParagraphAlignment.Center;
                    para1.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 11);
                    para1.Format.Font.Bold = false;
                    
                    para1.Format.Alignment = ParagraphAlignment.Center;


                    //row.Cells[5].AddParagraph().AddFormattedText("Grade", TextFormat.Bold);
                    row.Cells[7].MergeDown = 1;
                    row.Cells[8].Format.Alignment = ParagraphAlignment.Center;
                    Frame1 = row.Cells[8].AddTextFrame();
                    Frame1.Orientation = TextOrientation.Upward;
                    Frame1.Width = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1);

                    //Frame.Height = _GroupCount * _Rowheight;
                    //Frame.MarginLeft=
                    para1 = Frame1.AddParagraph("Numerical Grade");
                    para1.Format.Alignment = ParagraphAlignment.Center;
                    //para.Format.SpaceBefore = Unit.FromCm(0.4);
                    para1.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 11);
                    para1.Format.Font.Bold = false;
                   
                    //para.Format.ClearAll();
                    para1.Format.Alignment = ParagraphAlignment.Center;


                   // row.Cells[6].AddParagraph().AddFormattedText("Numerical Grade", TextFormat.Bold);
                    row.Cells[8].MergeDown = 1;

                    row.Cells[9].Format.Alignment = ParagraphAlignment.Center;
                    Frame1 = row.Cells[9].AddTextFrame();
                    Frame1.Orientation = TextOrientation.Upward;
                    Frame1.Width = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1);

                    //Frame.Height = _GroupCount * _Rowheight;
                    //Frame.MarginLeft=
                    para1.Format.LeftIndent = 5;
                    para1 = Frame1.AddParagraph("Award");
                    para1.Format.Alignment = ParagraphAlignment.Center;
                    //para.Format.SpaceBefore = Unit.FromCm(0.4);
                    para1.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 11);
                    para1.Format.Font.Bold = false;
                  
                    //para.Format.ClearAll();
                    para1.Format.Alignment = ParagraphAlignment.Center;


                    // row.Cells[6].AddParagraph().AddFormattedText("Numerical Grade", TextFormat.Bold);
                    row.Cells[9].MergeDown = 1;






                    row = tb.AddRow();
                    row.Format.Font.Size = 9;
                    k = 2;

                    for (int j = 0; j < _ExamDetails.Length; j++)
                    {
                        if (_ExamDetails[j].GroupName == "INTERNAL1")
                        {
                            row.Cells[k].Format.Alignment = ParagraphAlignment.Center;
                            row.Cells[k].AddParagraph().AddFormattedText ("8/20", TextFormat.Bold);
                        }
                        else if (_ExamDetails[j].GroupName == "INTERNAL2")
                        {
                            row.Cells[k].Format.Alignment = ParagraphAlignment.Center;
                            row.Cells[k].AddParagraph().AddFormattedText("4/10", TextFormat.Bold);
                        }
                        else if (_ExamDetails[j].GroupName == "INTERNAL3")
                        {
                            row.Cells[k].Format.Alignment = ParagraphAlignment.Center;
                            row.Cells[k].AddParagraph().AddFormattedText("4/10", TextFormat.Bold);
                        }

                        else if (_ExamDetails[j].GroupName == "EXTERNAL")
                        {
                            row.Cells[k].Format.Alignment = ParagraphAlignment.Center;
                            row.Cells[k].AddParagraph().AddFormattedText("24/60", TextFormat.Bold);
                        }
                        k = k + 1;
                    }
                   
                 

                    row.Cells[6].Column.LeftPadding = 1;
                    row.Cells[6].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[6].AddParagraph().AddFormattedText().AddFormattedText(" 40/100", TextFormat.Bold);

                }

                DataSet MainGrade = MyExamMang.GetGradeDataSet(_ExamDetails[0].GradeMasterId);
                DataSet Grade = MainGrade;

                int k1 = 2;
                double TotalMark;
                int count = 0;
          
                int tempmain = 1;
                int maincount = 0;
                int tempsub = 0;
                for (int Subjct = 0; Subjct < _SubDetails.Length; Subjct++)
                {
                                      
                    count++;
                    row = tb.AddRow();
                    row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[0].AddParagraph(count.ToString());
                    if (_SubDetails[Subjct].Name == "General Knowledge")
                    {
                        row.Cells[1].AddParagraph().AddFormattedText("G.K.");
                    }
                    else if (_SubDetails[Subjct].Name == "Alternative English")
                    {
                        row.Cells[1].AddParagraph().AddFormattedText("Alt. English");
                    }
                    else
                    {
                        row.Cells[1].AddParagraph().AddFormattedText(_SubDetails[Subjct].Name); //Subject Name
                    }

                    k1 = 2;

                    TotalMark = 0;
               
                    for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                    {
                       
                        if (_ExamDetails[ExamCount].GroupName != "MINER" && tempmain == 1 )
                        {
                            if (tempsub == 0 &&( double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString()) > 0 || ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString() == "-1"))
                            {
                                maincount = maincount + 1;
                                tempsub = 1;
                            }
                           

                            Grade = MyExamMang.GetGradeDataSet(_ExamDetails[ExamCount].GradeMasterId);


                            if (double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString()) > 0)
                            {
                                row.Cells[k1].Format.Alignment =ParagraphAlignment.Center;
                                row.Cells[k1].AddParagraph().AddFormattedText(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString(), TextFormat.Bold);
                                TotalMark = TotalMark + double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                            }
                            else if(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString() == "-1")
                            {
                                row.Cells[k1].Format.Alignment =ParagraphAlignment.Center;
                                row.Cells[k1].AddParagraph().AddFormattedText("-");
                            }

                           
                        }
                        else if (_ExamDetails[ExamCount].GroupName == "MINER" && TotalMark==0)
                        {
                            tempmain = 0;
                            
                            Grade = MyExamMang.GetGradeDataSet(_ExamDetails[ExamCount].GradeMasterId);

                            if( double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString())>0)
                            {
                                row.Cells[6].Format.Alignment =ParagraphAlignment.Center;
                                row.Cells[6].AddParagraph().AddFormattedText(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString(), TextFormat.Bold);
                                TotalMark = TotalMark +    double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                            }
                            else if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString() != "-1")
                            {
                                TotalMark = 0;
                                row.Cells[8].Format.Alignment =ParagraphAlignment.Center;
                                row.Cells[8].AddParagraph().AddFormattedText("-");
                            }
                        }
                       
                       

                        k1++;
                    }
                    tempsub = 0;
                    if (tempmain == 1)
                    {
                        row.Cells[6].Format.Alignment =ParagraphAlignment.Center;
                        row.Cells[6].AddParagraph().AddFormattedText(TotalMark.ToString(), TextFormat.Bold);
                    }
                    string GradeVal, NG;
                    GetGradeAndNG(TotalMark, Grade, out  GradeVal, out  NG);
                    row.Cells[7].Format.Alignment =ParagraphAlignment.Center;
                    row.Cells[7].AddParagraph().AddFormattedText(GradeVal, TextFormat.Bold);

                    row.Cells[8].Format.Alignment =ParagraphAlignment.Center;
                    row.Cells[8].AddParagraph().AddFormattedText(NG, TextFormat.NotBold);
                    string Award = "";
                    GetAwards(TotalMark, Grade, out Award);
                    row.Cells[9].Format.Alignment =ParagraphAlignment.Center;
                    row.Cells[9].Format.Font.Size = 12;
                    row.Cells[9].AddParagraph().AddFormattedText(Award, TextFormat.Bold);
                    TotalMark = 0;
                    GradeVal = "";                                     
                }
               

                tb.Borders.Visible = true;
                cel.Elements.Add(tb);

            

                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.AddLineBreak();
                paragraph.AddLineBreak();
                paragraph.AddLineBreak();
                 // dominic
                // Starting second  table here 
                if (CC_ExamDetails.Length > 0)
                {
                    tb = new MigraDoc.DocumentObjectModel.Tables.Table();
                    tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 8);
                    tb.AddColumn((PgSt.PageWidth - 60) / 3);
                    tb.AddColumn((PgSt.PageWidth - 60) / 3);
                    tb.AddColumn((PgSt.PageWidth - 60) / 3);
                    row = tb.AddRow();
                    tb.Rows.LeftIndent = 0;
                    tb.LeftPadding = 0;
                    tb.RightPadding = 0;
                    

                    MigraDoc.DocumentObjectModel.Tables.Cell cel1 = row.Cells[0];
                    //dominic : Main Table 
                    /*Co curricular*/
                    MigraDoc.DocumentObjectModel.Tables.Table tbc = new MigraDoc.DocumentObjectModel.Tables.Table();
                    tbc.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri",8);


                    tbc.AddColumn((((PgSt.PageWidth - 65) / 3) / 5 )- 3);
                    tbc.AddColumn(((PgSt.PageWidth - 65) / 3) * 3 / 5);
                    tbc.AddColumn(((PgSt.PageWidth - 65) / 3) / 5);

                    MigraDoc.DocumentObjectModel.Tables.Row rowc = tbc.AddRow();
                    rowc.Format.Font.Size = 8;
                  
                    rowc.Format.Alignment =ParagraphAlignment.Center;
                    rowc.Cells[0].AddParagraph().AddFormattedText("Sl.No", TextFormat.Bold);
                    rowc.Cells[1].AddParagraph().AddFormattedText("Subject", TextFormat.Bold);
                    rowc.Cells[2].AddParagraph().AddFormattedText("Grade", TextFormat.Bold);

                    DataSet CCGrade = MyExamMang.GetGradeDataSet(CC_ExamDetails[CC_ExamDetails.Length - 1].GradeMasterId);

                    string GradeVal, NG;

                    for (int Subjct = 0; Subjct < CC_SubDetails.Length; Subjct++)
                    {
                        count++;
                        rowc = tbc.AddRow();
                        rowc.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                        rowc.Cells[0].AddParagraph(count.ToString());
                          rowc.Cells[1].Format.Alignment = ParagraphAlignment.Left ;
                       
                        rowc.Cells[1].AddParagraph().AddFormattedText(CC_SubDetails[Subjct].Name); //Subject Name
                       
                        for (int CCExamCount = 0; CCExamCount < CC_ExamDetails.Length; CCExamCount++)
                        {
                            if (ClsObj[i].m_CC_ExamMarks[CCExamCount, Subjct].MaxMark.ToString() != "-1")
                            {
                                double ratio = (double.Parse(ClsObj[i].m_CC_ExamMarks[CCExamCount, Subjct].MaxMark.ToString()) / double.Parse(CC_SubDetails[Subjct].MaxMark.ToString())) * 100;
                                GetGradeAndNG(ratio, CCGrade, out  GradeVal, out  NG);
                                 rowc.Cells[2].Format.Alignment = ParagraphAlignment.Center;
                                rowc.Cells[2].AddParagraph().AddFormattedText(GradeVal);
                            }
                            else
                            {
                                rowc.Cells[2].AddParagraph().AddFormattedText("-");
                            }

                        }
                        TotalMark = 0;
                        GradeVal = "";
                    }

                  
                    tbc.Borders.Visible = true;

                    cel1.Elements.Add(tbc);

                    /*Grade Ratios*/

                    cel1 = row.Cells[1];

                    MigraDoc.DocumentObjectModel.Tables.Table tbr = new MigraDoc.DocumentObjectModel.Tables.Table();
                    tbr.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri",8);
                 
                    tbr.AddColumn(((PgSt.PageWidth - 65) / 3)/3 );
                    tbr.AddColumn(((PgSt.PageWidth - 65) / 3) /1.5);

                    MigraDoc.DocumentObjectModel.Tables.Row rowr = tbr.AddRow();

                    rowr.Format.Font.Name = "Calibri";
                    rowr.Cells[0].MergeRight = 1;
                    rowr.Cells[0].Format.Alignment =ParagraphAlignment.Center;
                    rowr.Cells[0].AddParagraph().AddFormattedText("Grade Details", TextFormat.Bold);
                 

                    rowr = tbr.AddRow();
                    rowr.Format.Alignment =ParagraphAlignment.Center;
                    rowr.Format.Font.Size = 8;
                    rowr.Format.Font.Name = "Calibri";
                    rowr.Cells[0].AddParagraph().AddFormattedText("Mark[%]", TextFormat.Bold);
                    rowr.Cells[1].AddParagraph().AddFormattedText("Performance", TextFormat.Bold);


                    int lastVal = 100;
                    int firstVal = 0;
                    string _MarksCondition = "";

                    string result = "";
                    int mergerow = 0;
                    foreach (DataRow dr in MainGrade.Tables[0].Rows)
                    {

                        rowr = tbr.AddRow();
                      
                        rowr.Format.Alignment =ParagraphAlignment.Center;
                        firstVal = int.Parse(dr["LowerLimit"].ToString());
                        _MarksCondition = firstVal.ToString() + " - " + lastVal.ToString();

                        rowr.Cells[0].AddParagraph().AddFormattedText(_MarksCondition);

                        rowr.Cells[1].Format.Font.Name = "Calibri";
                        rowr.Cells[1].Format.Font.Size = 7;
                         rowr.Cells[1].AddParagraph().AddFormattedText(dr["Result"].ToString());
                         mergerow = mergerow + 1;
                        
                        

                        lastVal = firstVal - 1;
                      
                         result = dr["Result"].ToString();
                    }


                   
                    tbr.Borders.Visible = true;
                    cel1.Elements.Add(tbr);






                    //last

                    cel1 = row.Cells[2];
                    // dominic : grade /numberical table for first 7 subjet started here


                    MigraDoc.DocumentObjectModel.Tables.Table tbf = new MigraDoc.DocumentObjectModel.Tables.Table();
                    tbf.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 8);
            
                    tbf.AddColumn(((PgSt.PageWidth - 65) / 3) / 2);
                    tbf.AddColumn(((PgSt.PageWidth - 65) / 3) / 2);
                    MigraDoc.DocumentObjectModel.Tables.Row rowf = tbf.AddRow();

                    MigraDoc.DocumentObjectModel.Tables.Cell celn = rowf.Cells[0];

                    MigraDoc.DocumentObjectModel.Tables.Table tblf = new MigraDoc.DocumentObjectModel.Tables.Table();
                    tbf.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri",8);

                    tblf.AddColumn(((PgSt.PageWidth - 65) / 3) / 4.2);
                    tblf.AddColumn(((PgSt.PageWidth - 65) / 3) / 4.2);
                    MigraDoc.DocumentObjectModel.Tables.Row rowlf = tblf.AddRow();
                    rowlf.Format.Alignment =ParagraphAlignment.Center;
                    rowlf.Format.Font.Size = 8;
                    rowlf.Format.Font.Name = "Calibri";
                    rowlf.Cells[0].MergeRight = 1;
                    rowlf.Cells[0].AddParagraph().AddFormattedText("Sl No :1 - " + maincount + "", TextFormat.Bold);
                   
                    rowlf = tblf.AddRow();
                    rowlf.Format.Alignment =ParagraphAlignment.Center;
                    rowlf.Format.Font.Name = "Calibri";
                    rowlf.Format.Font.Size = 8;
                    rowlf.Cells[0].AddParagraph().AddFormattedText("GRADE", TextFormat.Bold);
                    rowlf.Cells[1].AddParagraph().AddFormattedText("NG", TextFormat.Bold);



                    foreach (DataRow dr in MainGrade.Tables[0].Rows)
                    {
                        rowlf = tblf.AddRow();
                        rowlf.Format.Alignment =ParagraphAlignment.Center;
                        rowlf.Format.Font.Size = 8;
                        rowlf.Cells[0].Format.Font.Name = "Calibri";
                        rowlf.Cells[0].AddParagraph().AddFormattedText(dr["Grade"].ToString());
                        rowlf.Cells[1].Format.Font.Name = "Calibri";
                        rowlf.Cells[1].AddParagraph(dr["NumericalGrade"].ToString()).AddFormattedText( dr["Award"].ToString(),TextFormat.Bold);


                    }

                    tblf.Borders.Visible = true;
                    celn.Elements.Add(tblf);





                    celn = rowf.Cells[1];
                    // dominic : grade /numberical table for  subjet 8 to 20 started here
                    MigraDoc.DocumentObjectModel.Tables.Table tblt = new MigraDoc.DocumentObjectModel.Tables.Table();
                    tblt.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri",8);

                    tblt.AddColumn(((PgSt.PageWidth - 65) / 3) / 4.2);
                    tblt.AddColumn(((PgSt.PageWidth - 65) / 3) / 4.2);
                    MigraDoc.DocumentObjectModel.Tables.Row rowlt = tblt.AddRow();

                    rowlt.Format.Alignment = ParagraphAlignment.Center;
                    rowlt.Format.Font.Name = "Calibri";
                    rowlt.Cells[0].MergeRight = 1;
                    rowlt.Cells[0].AddParagraph().AddFormattedText("Sl No : " + ++maincount + " - " + count + "", TextFormat.Bold);

                    rowlt = tblt.AddRow();
                    rowlt.Format.Alignment = ParagraphAlignment.Center;
                    rowlt.Format.Font.Size = 8;
                    rowlt.Format.Font.Name = "Calibri";
                    rowlt.Format.Alignment =ParagraphAlignment.Center;
                    rowlt.Cells[0].AddParagraph().AddFormattedText("GRADE", TextFormat.Bold);
                    rowlt.Cells[1].AddParagraph().AddFormattedText("NG", TextFormat.Bold);


                    Grade = MyExamMang.GetGradeDataSet(_ExamDetails[_ExamDetails.Length-1].GradeMasterId);
                    foreach (DataRow dr in Grade.Tables[0].Rows)
                    {
                        rowlt = tblt.AddRow();
                        rowlt.Format.Alignment = ParagraphAlignment.Center;
                        rowlt.Format.Font.Size =8;
                        if (dr["NumericalGrade"].ToString() != "1")
                            rowlt.Height = 20;
                        rowlt.VerticalAlignment = VerticalAlignment.Center;
                        rowlt.Cells[0].Format.Font.Name = "Calibri";
                         
                        rowlt.Cells[0].AddParagraph().AddFormattedText(dr["Grade"].ToString());
                         rowlt.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                        rowlt.Cells[1].Format.Font.Name = "Calibri";
                          rowlt.Cells[1].VerticalAlignment = VerticalAlignment.Center;
                        rowlt.Cells[1].AddParagraph(dr["NumericalGrade"].ToString()).AddFormattedText(dr["Award"].ToString()).Font.Size = 12;
                        //rowlt.Cells[1].AddParagraph().AddFormattedText(dr["Award"].ToString()).Font.Size=12;
                       
                     

                    }

                    tblt.Borders.Visible = true;
                    celn.Elements.Add(tblt);

                    tbf.Borders.Visible = false;
                  
                    cel1.Elements.Add(tbf);


                    tb.Borders.Visible = false;
                   
                    cel.Elements.Add(tb);



                }
                //last grade 
                


                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.AddLineBreak();
                paragraph.AddLineBreak();
                paragraph.AddLineBreak(); paragraph.AddLineBreak();
                tb = new MigraDoc.DocumentObjectModel.Tables.Table();               
                tb.AddColumn((PgSt.PageWidth - 65) / 3);
                tb.AddColumn((PgSt.PageWidth - 65) / 3);
                tb.AddColumn((PgSt.PageWidth - 65) / 3);


                row = tb.AddRow();
                row.Format.Font.Size = 8;
                row.Cells[0].AddParagraph().AddFormattedText("Class Teacher", TextFormat.Italic);
                row.Cells[1].AddParagraph().AddFormattedText("Headmaster", TextFormat.Italic);         
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[2].AddParagraph().AddFormattedText("Parent/Guardian", TextFormat.Italic); 
                row.Cells[2].Format.Alignment =ParagraphAlignment.Right;


                tb.Borders.Visible = false;
                cel.Elements.Add(tb);
            }

            

            return document;
        }

        private bool GetAwards(double TotalMark, DataSet Grade, out string Award)
        {
          
            Award = "";
           

            foreach (DataRow dr in Grade.Tables[0].Rows)
            {
                if (double.Parse(dr["LowerLimit"].ToString()) <= TotalMark)
                {

                    Award = dr["Award"].ToString();
                    return true;
                }
            }
            return false;
        }
       
        private bool GetGradeAndNG(double TotalMark, DataSet Grade,out  string GradeVal,out  string NG)
        {
            GradeVal = "";
            NG = "";
            foreach (DataRow dr in Grade.Tables[0].Rows)
            {
                if (double.Parse(dr["LowerLimit"].ToString()) <= TotalMark)
                {
                    GradeVal=dr["Grade"].ToString();
                    NG = dr["NumericalGrade"].ToString();
                    return true;
                }
            }
            return false;
        }
          
        private string GetExamName(string ExamName, DataSet ExamDisplayDetails)
        {
            foreach (DataRow dr in ExamDisplayDetails.Tables[0].Rows)
            {
                if (dr["ExamName"].ToString() == ExamName)
                    return dr["DisplayName"].ToString();
            }
            return "";
        }

        private string  GetReportName()
        {
            if (Drp_ExamType.SelectedItem.ToString() == "First Quarterly")
            {
                return "1st QUARTERLY ASSESSMENT";
            }
            else if (Drp_ExamType.SelectedItem.ToString() == "Second Quarterly")
            {
                return "2nd QUARTERLY ASSESSMENT";
            }
            else if (Drp_ExamType.SelectedItem.ToString() == "Third Quarterly")
            {
                return "3rd QUARTERLY ASSESSMENT";
            }
            else if (Drp_ExamType.SelectedItem.ToString() == "Fourth Quarterly")
            {
                return "4th QUARTERLY ASSESSMENT";
            }
            else
                return "";
        }

        private void GetSubDetailsCC(int ClassId, int ExamTypeId, out ExamNode[] _SubDetails)
        {
          // string sql = " SELECT distinct  tblsubjects.Id,tblsubjects.subject_name,tblsubjects.sub_Catagory, tbltime_subgroup.Name ,tblsubjects.sub_description from tblclassexamsubmap inner JOIN tblsubjects on tblclassexamsubmap.SubId= tblsubjects.Id inner JOIN tblclassexam on tblclassexam.id= tblclassexamsubmap.ClassExamId   inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.id inner join tbltime_subgroup on tbltime_subgroup.Id= tblsubjects.sub_Catagory  where  tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "  and tblclassexam.ClassId=" + ClassId + " and tblclassexam.ExamId=" + ExamId;
            string sql = "SELECT  distinct  tblsubjects.Id,tblsubjects.subject_name, tblsubjects.sub_description,tblclassexamsubmap.MaxMark from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id  inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId   inner join tblexammaster on  tblexammaster.Id=  .tblclassexam.ExamId Inner join tblsubject_type on  tblexammaster.ExamTypeId=tblsubject_type.Id     inner join tblexamdisplaymap on tblexamdisplaymap.ExamName=tblexammaster.ExamName   inner join tblclassexamsubmap on tblclassexamsubmap.SubId=  tblsubjects.Id and  tblclassexamsubmap.ClassExamId= tblclassexam.Id   where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "    and   tblclassexam.ClassId=" + ClassId + " and tblsubject_type.Id=" + ExamTypeId + "   and tblexamschedule.Status='Completed' and tblexamdisplaymap.HardcodedName='COCURRICULAR'   order by tblexammark.SubjectOrder  ";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                int i = 0;
                int Count = MyReader.RecordsAffected;
                _SubDetails = new ExamNode[Count];

                while (MyReader.Read())
                {
                    //_SubDetails[i].Id = int.Parse(MyReader.GetValue(0).ToString());
                    //_SubDetails[i].Name = MyReader.GetValue(1).ToString();

                    //_SubDetails[i].Group = int.Parse(MyReader.GetValue(2).ToString());
                    //_SubDetails[i].GroupName = MyReader.GetValue(3).ToString();
                    //_SubDetails[i].Desc = MyReader.GetValue(4).ToString();
                    _SubDetails[i].Id = int.Parse(MyReader.GetValue(0).ToString());
                    _SubDetails[i].Name = MyReader.GetValue(1).ToString();
                     _SubDetails[i].MaxMark = double.Parse(MyReader["MaxMark"].ToString());
                    _SubDetails[i].Desc = MyReader.GetValue(2).ToString();
          

                    i++;
                }
            }
            else
            {
                _SubDetails = new ExamNode[0];
            }

        }

        private void GetExamDetailsCC(int ClassId, int ExamTypeId, out ExamNode[] _ExamDetails)
        {
            string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period, tblexamschedule.GradeMasterId, tblexamdisplaymap.HardcodedName  from tblsubject_type  inner join tblexammaster on tblexammaster.ExamTypeId=tblsubject_type.Id   inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id   inner join tblexamdisplaymap on tblexamdisplaymap.ExamName=tblexammaster.ExamName   where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblsubject_type.Id=" + ExamTypeId + " and tblexamschedule.Status='Completed' and tblexamdisplaymap.HardcodedName='COCURRICULAR'  order by tblexamschedule.id ";
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
                    _ExamDetails[i].GroupName = MyReader["HardcodedName"].ToString();
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
            dt.Columns.Add("Attendance");
            string sql_Student = "select tblview_student.Id, tblview_student.StudentName, tblview_student.RollNo, tblclass.ClassName,date_Format(tblview_student.DOB,'%d/%m/%Y') as DOB , tblview_student.AdmitionNo,tblview_student.GardianName,tblview_student.MothersName, tblview_student.ResidencePhNo ,tblview_student.Address from tblview_student inner join tblclass on tblclass.Id= tblview_student.ClassId inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblview_student.Id  where tblview_student.ClassId=" + ClassId + " and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId;

            if (StudentId != 0)
                sql_Student = sql_Student + " and tblview_student.Id=" + StudentId;

            sql_Student = sql_Student + " Order By tblview_student.RollNo ASC";

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
                dr["Attendance"] = 0;
                _StdentSet.Tables["Student"].Rows.Add(dr);

            }

            DateTime StartDate = System.DateTime.Now;
            DateTime EndDate = System.DateTime.Now;
            DateTime.TryParse(Txt_EndDate.Text.ToString().Trim(), out EndDate); 
            DateTime.TryParse(Txt_startdate.Text.ToString().Trim(), out StartDate); 

            foreach (DataRow dr_values in _StdentSet.Tables[0].Rows)
            {
                int _no_workingdays = 0;
                int _no_presentdays = 0;
                int _no_absentdays = 0;
                int _no_holidays = 0;
                int _no_halfdays = 0;
                double _attendencepersent = 0;


                MyAttendence.GetCurrentBatchNewattendanceDetailsWithDate(int.Parse(dr_values["Id"].ToString()), out  _no_workingdays, out  _no_presentdays, out _no_absentdays, out _no_holidays, out _no_halfdays, out _attendencepersent, MyUser.CurrentBatchId, StartDate, EndDate);
                dr_values["Attendance"] = _no_presentdays + "/" + _no_workingdays;
            }

       
            return _StdentSet;


        }

  
        
        private void GetSubDetails(int ClassId, int ExamTypeId, out ExamNode[] _SubDetails)
        {
            //string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=tblexamschedule.ClassExamId and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId  where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblclassexam.ClassId=" + ClassId + " and tblexammaster.Id=" + ExamId;
            // string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark ,tblsubjects.sub_description from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id  inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId   inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId   inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=tblexamschedule.ClassExamId   and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId     where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "   and tblexammaster.Id     in (select tblexammaster.id from tblsubject_type  inner join tblexammaster on tblexammaster.ExamTypeId=tblsubject_type.Id  inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id   inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id    where tblclassexam.ClassId=" + ClassId + "  and    tblsubject_type.Id=" + ExamTypeId + " and tblexamschedule.Status='Completed'  order by tblexamschedule.id )";
            // no need to take max mark here 

            string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblsubjects.sub_description from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id  inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId   inner join tblexammaster on  tblexammaster.Id=  .tblclassexam.ExamId Inner join tblsubject_type on  tblexammaster.ExamTypeId=tblsubject_type.Id     inner join tblexamdisplaymap on tblexamdisplaymap.ExamName=tblexammaster.ExamName   where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "    and   tblclassexam.ClassId=" + ClassId + " and tblsubject_type.Id=" + ExamTypeId + "   and tblexamschedule.Status='Completed' and tblexamdisplaymap.HardcodedName<>'COCURRICULAR'  order by tblexammark.SubjectOrder   ";
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
            string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period, tblexamschedule.GradeMasterId, tblexamdisplaymap.HardcodedName from tblsubject_type  inner join tblexammaster on tblexammaster.ExamTypeId=tblsubject_type.Id   inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id   inner join tblexamdisplaymap on tblexamdisplaymap.ExamName=tblexammaster.ExamName   where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblsubject_type.Id=" + ExamTypeId + " and tblexamschedule.Status='Completed' and tblexamdisplaymap.HardcodedName<>'COCURRICULAR'  order by tblexamdisplaymap.HardcodedName  ";
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
                    _ExamDetails[i].GradeMasterId = int.Parse(MyReader.GetValue(3).ToString());
                     _ExamDetails[i].GroupName=MyReader["HardcodedName"].ToString();
                   
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
                
        private void GetSchoolDetails(out SchoolDetails _SchoolDetails)
        {
            _SchoolDetails.SchoolName = "";
            _SchoolDetails.Address = "";
            _SchoolDetails.LogoURL = "";

            string sql = "select tblschooldetails.SchoolName, tblschooldetails.Address, tblschooldetails.LogoUrl from tblschooldetails";
            OdbcDataReader MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                _SchoolDetails.SchoolName = MyReader.GetValue(0).ToString();
                _SchoolDetails.Address = MyReader.GetValue(1).ToString();
                _SchoolDetails.LogoURL = MyReader.GetValue(2).ToString();
            }

        }

        private DataSet GetExamDisplayMap()
        {
            string sql = "select Id,ExamName,DisplayName,HardcodedName,ExamMasterId from tblexamdisplaymap";
            DataSet dt_exams = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return dt_exams;
            
        }

        private bool ExamsExist(out DataSet dt_exams, out string Msg)
        {

            string sql = "select distinct tblexammaster.Id, tblexammaster.ExamName from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster ON tblexammaster.Id= tblclassexam.ExamId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id  where tblclassexam.ClassId = " + int.Parse(Drp_ClassSelect.SelectedValue) + " and tblexamschedule.BatchId = " + MyUser.CurrentBatchId + " and tblexammaster.ExamTypeId= " + int.Parse(Drp_ExamType.SelectedValue) + " and tblexamschedule.status ='Completed'";
            dt_exams = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (dt_exams != null && dt_exams.Tables != null && dt_exams.Tables[0].Rows.Count > 0)
            {
                Msg = "";
                return true;
            }
            Msg = "Exams does not found.";
            return true;

        }

        private bool ValidInputs(out string Msg)
        {
            int Classid = Convert.ToInt32(Drp_ClassSelect.SelectedValue);
            int examTypeId=Convert.ToInt32(Drp_ExamType.SelectedValue);
            if (Classid <= 0)
            {
                Msg = "Select class";
                return false;
            }

            else if (examTypeId <= 0)
            {
                Msg = "Select exam type";
                return false;
            }
            else if (drp_Student.SelectedValue == "-1")
            {
                Msg = "Select  a student";
                return false;
            }
            else
            {
                Msg = "";
                return true;
            }

        }

        protected void Drp_ClassSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentDetails();
        }
    }
}
