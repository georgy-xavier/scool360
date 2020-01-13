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
    public class JoinedProgessReportClass
    {
        //public MysqlClass m_MysqlDb;
        //public ExamNode[] m_Eaxams;
        //public ExamNode[] m_Subjects;
        //public MarkNode[,] m_ExamMarks;
        //public MarkNode[] m_TotalMarks;
        //public MarkNode[] m_Ranks;
        //public StudentDetails m_StudDetails;

        //public int m_StudentId;
        //public string[] m_Grade;
        //public double[] m_Average;
        //public double[] m_MaxTotal;

        //public ExamNode[] m_CC_Eaxams;
        //public ExamNode[] m_CC_Subjects;
        //public MarkNode[,] m_CC_ExamMarks;

        //public MarkNode[] m_CC_TotalMarks;
        //public MarkNode[] m_CC_Ranks;


        //public string[] m_CC_Grade;
        //public double[] m_CC_Average;
        //public double[] m_CC_MaxTotal;

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



        public JoinedProgessReportClass(MysqlClass _Mysqlobj, int _StudentId)
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
                m_StudDetails.Class = _StudDetails[5].ToString();
              
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
                                if (temp == 0)
                                {
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




    public partial class JoinedProgressReport : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private ExamManage MyExamMang;
        private ClassOrganiser MyClassMngr;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private Attendance MyAttendence;
        private int _ClassId = 0;
        private int _ExamId = 0;
        private MysqlClass _Mysqlobj;
        public StudentDetails m_StudDetails;
        private SchoolClass objSchool = null;
        private string M_Logo = "";
        MigraDoc.DocumentObjectModel.Font Font_All;
        MigraDoc.DocumentObjectModel.Font Font_ReportName;
        MigraDoc.DocumentObjectModel.Font Font_StudentDetails;
        MigraDoc.DocumentObjectModel.Font Font_Heading;
        MigraDoc.DocumentObjectModel.Font Font_ItemDetails;
        MigraDoc.DocumentObjectModel.Font Font_Total;
        MigraDoc.DocumentObjectModel.Font Font_BottomDetails;
        MigraDoc.DocumentObjectModel.Font Font_BottomDetails1;
        MigraDoc.DocumentObjectModel.Font Font_BottomDetails2;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
                Response.Redirect("Default.aspx");

            MyUser = (KnowinUser)Session["UserObj"];
            MyClassMngr = MyUser.GetClassObj();
            MyStudMang = MyUser.GetStudentObj();
            MyExamMang = MyUser.GetExamObj();
            MyAttendence = MyUser.GetAttendancetObj();

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
                LoadDetails();
        }

        private void LoadDetails()
        {
            LoadFont();
            LoadClassDetailsToDropDown();
            LoadExamDetailsToDropDown();
            LoadStudentsDetailsToDropDown();
        }
        protected void Drp_SelectClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadExamDetailsToDropDown();
            LoadStudentsDetailsToDropDown();
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

        private void CreateReort(out string _ErrMessage)
        {
            _ErrMessage = "";
         //   int _CCExamId = 0;
            DataSet StudList = new DataSet();
            ExamNode[] _ExamDetails;
            ExamNode[] _SubDetails;
            //ExamNode[] CC_ExamDetails = null;
            //ExamNode[] CC_SubDetails = null;

            ExamNode[] Total_SubDetails;
            ExamNode[] Total_ExamDetails = null;

            SchoolDetails _SchoolDetails;
            DataSet TopStudentList = new DataSet();

            List<JoinedProgessReportClass> ClsObj = new List<JoinedProgessReportClass>();
            JoinedProgessReportClass _ReportObj;

            try
            {
                int StudentId = 0;
                int.TryParse(Drp_SelectStudent.SelectedValue, out StudentId);
                int.TryParse(Drp_SelectClass.SelectedValue, out _ClassId);
                int.TryParse(Drp_SelectExam.SelectedValue, out _ExamId);
              //  _CCExamId = MyExamMang.GetExamIdFromSubType("Co-curricular Activities", _ClassId);
                
                StudList = GetStudDetails(StudentId, _ClassId, _ExamId);
                if (StudList != null && StudList.Tables != null && StudList.Tables[0].Rows.Count > 0)
                {
                    GetExamDetails(_ClassId, _ExamId, out _ExamDetails);
                    GetSubDetails(_ClassId, _ExamId, out _SubDetails);
                    GetSchoolDetails(out _SchoolDetails);



                    GetTotalExamDetails(_ClassId, _ExamId, out Total_ExamDetails);

                    Total_SubDetails = null;

                    if (Total_ExamDetails != null)
                    {
                        getMaximumSubject(_ClassId, Total_ExamDetails, out Total_SubDetails);

                    }

                    //GetExamDetails(_ClassId, _CCExamId, out CC_ExamDetails);
                    //GetSubDetails(_ClassId, _CCExamId, out CC_SubDetails);

                    foreach (DataRow Dr in StudList.Tables[0].Rows)
                    {
                        _ReportObj = new JoinedProgessReportClass(_Mysqlobj, int.Parse(Dr[0].ToString()));
                        _ReportObj.ExamDetails(_ExamDetails, _SubDetails, Dr, "Main");
                        // _ReportObj.ExamDetails(CC_ExamDetails, CC_SubDetails, Dr, "CC");

                        if (Total_ExamDetails != null)
                            _ReportObj.ExamDetails(Total_ExamDetails, Total_SubDetails, Dr, "Total");



                        ClsObj.Add(_ReportObj);
                    }


                    //CreateReport(ClsObj, _ExamDetails, _SubDetails, _SchoolDetails, StudentId, CC_ExamDetails, CC_SubDetails);
                    CreateReport(ClsObj, _ExamDetails, _SubDetails, _SchoolDetails, Total_ExamDetails, Total_SubDetails, StudentId);
                }
                else
                {
                    Lbl_Err.Text = "Exam details does not contain under the selected student. Please make sure the student attended the exams";

                }
            }
            catch (Exception e)
            {

                
                Lbl_Err.Text = e.Message;
              
            }

        }



        private void CreateReport(List<JoinedProgessReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, SchoolDetails _SchoolDetails, ExamNode[] Total_ExamDetails, ExamNode[] Total_SubDetails, int StudentId)
        {
            //LoadFont();
            string _PhysicalPath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));
            Document document = LoadPDFPage(ClsObj, _ExamDetails, _SubDetails, Total_ExamDetails,  Total_SubDetails);
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

            pdfRenderer.PdfDocument.Save(filename);
            // ...and start a viewer.
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "null", "window.open(\"OpenPdfPage.aspx?PdfName=PRC_" + MainName + ".pdf\",'Info','status=1, width=1000, height=700,,resizable=1,scrollbars=1');", true);

            //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=PRC_" + MainName + ".pdf\");", true);
            // Response.Redirect("OpenPdfPage.aspx?PdfName=PRC_" + MainName + ".pdf", false);
            if (!String.IsNullOrEmpty(M_Logo))
            {
                File.Delete(M_Logo);
            }
        }
    
        private Document LoadPDFPage(List<JoinedProgessReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails,ExamNode[] Total_ExamDetails, ExamNode[] Total_SubDetails)
        {
            Document ReportCard = new Document();

            string _ExamName = _ExamDetails[_ExamDetails.Length - 1].Name;
            string _ReportHeading = "REPORT CARD : " + Drp_SelectExam.SelectedItem + " : " + MyUser.CurrentBatchName;
            // Start  Create Details
            for (int Count = 0; Count < ClsObj.Count; Count++)
            {
                PageSetup PgSt = ReportCard.DefaultPageSetup;

                PgSt.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.A4;
                PgSt.LeftMargin = 70;
                PgSt.RightMargin = 60;
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
                Top.Format.Font.Underline = Underline.Single;
                Top.AddFormattedText(_ReportHeading);

                Paragraph _space = Report_Section.AddParagraph();
                _space.AddFormattedText("\n");

                //ImageUploaderClass imgobj  = new ImageUploaderClass(objSchool);                
                //byte[] img_bytes= imgobj.getImageBytes(objSchool.SchoolId,"Logo");
                //M_Logo = MyUser.FilePath + "/ThumbnailImages/" + objSchool.SchoolId + "_" + System.DateTime.Now.Millisecond + ".jpg";


                //File.WriteAllBytes(M_Logo, img_bytes);


                //_space.AddImage(M_Logo).Width = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(50).Millimeter;
               
               

                // student details


                MigraDoc.DocumentObjectModel.Tables.Table Stud_Table = new MigraDoc.DocumentObjectModel.Tables.Table();

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
                MigraDoc.DocumentObjectModel.Tables.Table _BaseTable = GetTableFormant();

                Table TableMain = GetMainReportDetails(ClsObj[Count].m_ExamMarks, _ExamDetails, _SubDetails, ClsObj[Count].m_Average, ClsObj[Count].m_TotalMarks, ClsObj[Count], ClsObj[Count].m_StudentId, ClsObj[Count].m_MaxTotal, _BaseTable);
                ReportCard.LastSection.Add(TableMain);

                MigraDoc.DocumentObjectModel.Tables.Table TotalExam = GetTotalExam(ClsObj[Count].m_T_ExamMarks, Total_ExamDetails, Total_SubDetails);
                ReportCard.LastSection.Add(TotalExam);











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

                //if (Chk_needrank.Checked)//mani 29.8.2013
                //{
                //    cell1 = row1.Cells[0];
                //    cell.Format.Font.Bold = true;
                //    cell1.Format.Alignment = ParagraphAlignment.Left;
                //    cell1.AddParagraph("Rank  ");


                //    cell1 = row1.Cells[1];
                //    cell.Format.Font.Bold = false;
                //    cell1.Format.Alignment = ParagraphAlignment.Left;
                //    cell1.AddParagraph(": " + ClsObj[Count].m_Ranks[_ExamDetails.Length - 1].MaxMark);
                //}

                ////dominic  if need to show the attendance commend this 3 lines and uncommend below three lines
                //if (Chk_needattendance.Checked)//mani 29.8.2013
                //{
                //    cell1 = row1.Cells[2];
                //    cell1.Format.Alignment = ParagraphAlignment.Left;
                //    cell1.AddParagraph("Attendance : " + ClsObj[Count].m_StudDetails.Attendance);
                //}

                //cell1 = row1.Cells[2];
                //cell1.Format.Alignment = ParagraphAlignment.Left;
                //cell1.AddParagraph("Attendance : " + ClsObj[Count].m_StudDetails.Attendance);



                ReportCard.LastSection.Add(TotalRepot);

                Paragraph space2 = Report_Section.AddParagraph();
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

                Cell cell2 = row2.Cells[0];
                cell2.Format.Alignment = ParagraphAlignment.Left;
                cell2.AddParagraph("Teacher's Sign");

                cell2 = row2.Cells[1];
                cell2.Format.Alignment = ParagraphAlignment.Center;
                cell2.AddParagraph("Parent's Sign");

                cell2 = row2.Cells[2];
                cell2.Format.Alignment = ParagraphAlignment.Right;
                cell2.AddParagraph("H.M.Sign");

                ReportCard.LastSection.Add(Bottom);
            }

            return ReportCard;
        }

        private Table GetTotalExam(MarkNode[,] markNode, ExamNode[] Total_ExamDetails, ExamNode[] _SubDetails)
        {
            int totalSubCOunt = _SubDetails.Length;

            MigraDoc.DocumentObjectModel.Font fontMain2 = new MigraDoc.DocumentObjectModel.Font("Times New Roman", 10);
            MigraDoc.DocumentObjectModel.Tables.Table table3 = new MigraDoc.DocumentObjectModel.Tables.Table();
            table3.Format.Font = fontMain2;
            double[] avgMarks, MaxMark;
            int[] TotalExam;

            table3.Borders.Width = 0.5;
            Column column3;

            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2.5));

            double totalColumnsize = 14.5 / totalSubCOunt;

     
            for (int c = 0; c < totalSubCOunt; c++)
            {
                column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(totalColumnsize));
            }

            Row row1 = table3.AddRow();
            Cell cell1 = row1.Cells[0];


            cell1.MergeRight = totalSubCOunt+1;
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
                    cell1.AddParagraph(_SubDetails[j].SubCode.Substring(0, 5));
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
                gradeId = MyExamMang.GetGradeMaster(Total_ExamDetails[i].Id);
                Grade = MyExamMang.GetGradeDataSet(gradeId);
                GetTotalSubDetails(int.Parse(Drp_SelectClass.SelectedValue.ToString()), Total_ExamDetails[i].Id, out _SubDetails);

                cell1 = row1.Cells[0];

                cell1.AddParagraph((i + 1) + "\n");
                cell1 = row1.Cells[1];

                if (Total_ExamDetails[i].Name.Length > 11)
                    cell1.AddParagraph(Total_ExamDetails[i].Name.Substring(0, 12) + "\n");
                else
                    cell1.AddParagraph(Total_ExamDetails[i].Name + "\n");

                int j = 0;
                for (j = 0; j < _SubDetails.Length; j++)
                {
                    cell1 = row1.Cells[2 + j];
                    
                    //dominic 17-10-2011
                    //Changed the code  if the exam is not found then showing "-" and if the masrk is 0 or -1 then showing "a" means absent

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
                    else if (_SubDetails[j].GradeSubject == 0)
                    {
                        cell1.AddParagraph(markNode[i, j].MaxMark.ToString());
                    }
                    else
                    {

                        cell1.AddParagraph(GetGradeFromMarks(Grade, double.Parse(markNode[i, j].MaxMark.ToString()), _SubDetails[j].MaxMark)).ToString();
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
            column = _MarkDetails.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(13));
            column = _MarkDetails.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(4));

            return _MarkDetails;
        }

        private Table GetMainReportDetails(MarkNode[,] markNode, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, double[] m_Average, MarkNode[] m_TotalMarks, JoinedProgessReportClass joinedProgessReportClass, int m_StudentId, double[] _MaxMark,MigraDoc.DocumentObjectModel.Tables.Table tblbase)
       
        {
          


            MigraDoc.DocumentObjectModel.Tables.Table _MarkDetails = tblbase;

            Row row; Cell cell;

            row = _MarkDetails.AddRow();

            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            cell = row.Cells[0];

            cell.AddParagraph("Sl. No");
            cell = row.Cells[1];
            cell.AddParagraph("Subject");
            cell = row.Cells[2];

            cell.AddParagraph("Marks/Grade\n Obtained [125/100]");
    



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

            gradeId = MyExamMang.GetGradeMaster(_ExamDetails[_ExamDetails.Length - 1].Id);
            Grade = MyExamMang.GetGradeDataSet(gradeId);

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
                    if (_SubDetails[Subjct].GradeSubject == 0)
                    {
                        cell.AddParagraph(markNode[_ExamDetails.Length - 1, Subjct].MaxMark.ToString());
                    }
                    else
                    {
                        cell.AddParagraph(GetGradeFromMarks(Grade, double.Parse(markNode[_ExamDetails.Length - 1, Subjct].MaxMark.ToString()), _SubDetails[Subjct].MaxMark)).ToString();
                    }


                }



            }


            return _MarkDetails;

        }

        private string GetGrade(double mark, ExamNode SubjectDetails)
        {
            DataSet Grade = MyExamMang.GetGradeDataSet(SubjectDetails.GradeMasterId);

            double avg = (mark / SubjectDetails.MaxMark) * 100;

            foreach (DataRow Dr in Grade.Tables[0].Rows)
            {
                if (avg >= double.Parse(Dr[1].ToString()))
                {
                    return Dr[0].ToString();
                }
            }
            return "";

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
            string sql = " SELECT  distinct  tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark,tblsubjects.sub_Catagory, tbltime_subgroup.Name,tbltime_subgroup.IsGradeSubjects  from tblclassexamsubmap inner JOIN tblsubjects on tblclassexamsubmap.SubId= tblsubjects.Id  inner JOIN tblexammark on  tblexammark.SubjectId=tblsubjects.Id  inner JOIN tblclassexam on tblclassexam.id= tblclassexamsubmap.ClassExamId   inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.id inner join tbltime_subgroup on tbltime_subgroup.Id= tblsubjects.sub_Catagory  where  tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "  and tblclassexam.ClassId=" + ClassId + " and tblclassexam.ExamId=" + ExamId + " Order By  tblsubjects.sub_Catagory , tblexammark.SubjectOrder ";

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
                    _SubDetails[i].MaxMark = double.Parse(MyReader.GetValue(2).ToString());
                    _SubDetails[i].Group = int.Parse(MyReader.GetValue(3).ToString());
                    _SubDetails[i].GroupName = MyReader.GetValue(4).ToString();
                    _SubDetails[i].GradeSubject = int.Parse(MyReader.GetValue(5).ToString());
                  
                    i++;
                }
            }
            else
            {
                _SubDetails = new ExamNode[0];
            }

        }
        private void GetTotalSubDetails(int ClassId, int ExamScheduledId, out ExamNode[] _SubDetails)
        {
            //string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=tblexamschedule.ClassExamId and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId  where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblclassexam.ClassId=" + ClassId + " and tblexammaster.Id=" + ExamId;
            string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark ,tblsubjects.SubjectCode,tblexammark.SubjectOrder , tblsubjects.sub_Catagory,  tbltime_subgroup.Name,tbltime_subgroup.IsGradeSubjects from tblclassexamsubmap inner JOIN tblsubjects on tblclassexamsubmap.SubId= tblsubjects.Id inner JOIN tblclassexam on tblclassexam.id= tblclassexamsubmap.ClassExamId    inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.id  inner JOIN tblexammark on  tblexammark.SubjectId=tblsubjects.Id and tblexammark.ExamSchId =tblexamschedule.id      inner join tbltime_subgroup on tbltime_subgroup.Id= tblsubjects.sub_Catagory   where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "  and tblclassexam.ClassId=" + ClassId + " and tblexamschedule.Id=" + ExamScheduledId + " order by tblexammark.SubjectOrder";

            MyReader = MyClassMngr.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                int i = 0;
                int Count = MyReader.RecordsAffected;
                _SubDetails = new ExamNode[Count];

                while (MyReader.Read())
                {
                    _SubDetails[i].Id = int.Parse(MyReader["Id"].ToString());
                    _SubDetails[i].Name = MyReader["subject_name"].ToString();
                    _SubDetails[i].MaxMark = double.Parse(MyReader["MaxMark"].ToString());
                    _SubDetails[i].SubCode = MyReader["SubjectCode"].ToString();

      
                    _SubDetails[i].Group = int.Parse(MyReader["sub_Catagory"].ToString());
                    _SubDetails[i].GroupName = MyReader["Name"].ToString();
                    _SubDetails[i].GradeSubject = int.Parse(MyReader["IsGradeSubjects"].ToString());
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
        private void GetTotalExamDetails(int ClassId, int ExamId, out ExamNode[] _ExamDetails)
        {

            string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period from tblexammaster inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id     where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark)  and tblexamschedule.Status='Completed' and tblexammaster.ExamTypeId=1  order by tblexamschedule.id ASC";
            MyReader = MyClassMngr.m_MysqlDb.ExecuteQuery(sql);

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
                    OdbcDataReader dr = MyClassMngr.m_MysqlDb.ExecuteQuery(sql1);
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

        private void getMaximumSubject(int ClassId, ExamNode[] Total_ExamDetails, out  ExamNode[] Total_SubDetails)
        {
            Total_SubDetails = new ExamNode[0];
            int temp = 0;
            for (int j = 0; j < Total_ExamDetails.Length; j++)
            {
                //string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=tblexamschedule.ClassExamId and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId  where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblclassexam.ClassId=" + ClassId + " and tblexammaster.Id=" + ExamId;
                string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark ,tblsubjects.SubjectCode,tblexammark.SubjectOrder  from tblclassexamsubmap inner JOIN tblsubjects on tblclassexamsubmap.SubId= tblsubjects.Id inner JOIN tblclassexam on tblclassexam.id= tblclassexamsubmap.ClassExamId    inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.id  inner JOIN tblexammark on  tblexammark.SubjectId=tblsubjects.Id and tblexammark.ExamSchId =tblexamschedule.id   where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "  and tblclassexam.ClassId=" + ClassId + " and tblexamschedule.Id=" + Total_ExamDetails[j].Id + " order by tblexammark.SubjectOrder";

                MyReader = MyClassMngr.m_MysqlDb.ExecuteQuery(sql);

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

    }
}
