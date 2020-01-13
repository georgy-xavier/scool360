using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;

using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

using PdfSharp.Pdf;
using MigraDoc;
using MigraDoc.DocumentObjectModel.Tables;
using System.Configuration;


namespace WinErParentLogin
{

    public struct StudentDetails
    {
        public int Id;
        public string Name;
        public int RollNum;
        public string Attendance;
        public string DOB;
        public string Class;
        public string AdmissionNum;
        public double Strength;
        public string FatherName;
        public string MotherName;
        public string Tel;
        public string Add;
        public string Sex;
        public string State;
        public string Year;
    }

    public struct ExamNode
    {
        public int Id;
        public string Name;
        public double MaxMark;
        public int Group;
        public string GroupName;
        public DateTime Date;
        public int GradeMasterId;
        public string Desc;
        public string SubCode;

    }

    public struct MarkNode
    {
        public double MaxMark;
        public int Id;
        public string Name;
        public bool IsActive;
    }

    public struct StudDetails
    {
        public int Id;
        public string Name;
        public int RollNum;
        public string Class;
    }

    public struct SchoolDetails
    {
        public string LogoURL;
        public string SchoolName;
        public string Address;
    }

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
                        DataSet MarkSet = m_MysqlDb.ExecuteQueryReturnDataSet(sqlMark);

                        if (MarkSet != null && MarkSet.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < m_Subjects.Length; j++)
                            {
                                foreach (DataRow dr in ColumnDetails.Tables[0].Rows)
                                {
                                    if (m_Subjects[j].Id == int.Parse(dr["SubjectId"].ToString()))
                                    {
                                        m_T_ExamMarks[i, j].Id = _Eaxams[i].Id;
                                        m_T_ExamMarks[i, j].Name = _Eaxams[i].Name;

                                        string _DbVal = dr["MarkColumn"].ToString();
                                        string Mark = MarkSet.Tables[0].Rows[0][_DbVal].ToString();

                                        m_T_ExamMarks[i, j].MaxMark = double.Parse(Mark);

                                        break;
                                    }
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

    
    public partial class Examreports : System.Web.UI.Page
    {
        private ParentInfoClass MyParentInfo;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet = null;
        private KnowinUser MyUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MyParentObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyParentInfo = (ParentInfoClass)Session["MyParentObj"];

            if (MyParentInfo == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (!IsPostBack)
            {
                // GenerateTable();
                LoadExams(); 
            }
        }
        
        protected void LoadExams()
        {

            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            string sql = "select tblstudentmark.ExamSchId, tblexammaster.ExamName , tblperiod.Period from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster ON tblexammaster.Id= tblclassexam.ExamId inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblstudentmark on tblstudentmark.ExamSchId= tblexamschedule.Id where tblexamschedule.BatchId=" + MyParentInfo.CurrentBatchId + " AND tblstudentmark.StudId=" + MyParentInfo.StudentId + " AND tblexammaster.`Status`=1 and tblexamschedule.Status='Completed'";
            MydataSet = MyParent.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_ExamList.Columns[1].Visible = true;
                Grd_ExamList.Columns[0].Visible = true;
                Grd_ExamList.DataSource = MydataSet;
                Grd_ExamList.DataBind();
                Grd_ExamList.Columns[1].Visible = false;
                Grd_ExamList.Columns[0].Visible = false;

                string ReportType = MyParent.getReportConfiguration();
               
                if (String.Compare(ReportType, "Mark") == 0)
                {
                    Grd_ExamList.Columns[4].Visible = false;
                    Grd_ExamList.Columns[5].Visible = true;
                }
                else
                {
                    Grd_ExamList.Columns[4].Visible = true;
                    Grd_ExamList.Columns[5].Visible = false;
                }
            }
            else
            {
                Grd_ExamList.DataSource = null;
                Grd_ExamList.DataBind();
                Lbl_indexammsg.Text = "No Exams found";

            }
            ReleaseResourse(_mysqlObj, MyParent);
        }

        protected void Lnk_PreviousPerformance_Click(object sender, EventArgs e)
        {
           
                Response.Redirect("ExamReportYearly.aspx");
            
        }

        private void ReleaseResourse(MysqlClass _mysqlObj, ParentLogin MyParent)
        {
            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyParent = null;
        }

        protected void Grd_ExamList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_ExamList.PageIndex = e.NewPageIndex;
            LoadExams();
        }

        protected void Grd_ExamList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowState == DataControlRowState.Alternate)
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='gray';this.style.cursor='hand'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
                }
                else
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='gray';this.style.cursor='hand'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#F7F7DE';");
                }
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Grd_ExamList, "Select$" + e.Row.RowIndex);
            }

        }

        #region mark area
        
        protected void Grd_ExamList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //mark 
            int _ExamScheduleId = int.Parse(Grd_ExamList.SelectedRow.Cells[1].Text.ToString());
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
          
            LoadReport(MyParentInfo.CLASSID, _ExamScheduleId, MyParentInfo.StudentId, _mysqlObj, MyParent,"Mark");
           
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "<script language=JavaScript>window.open(\"StudExamReport.aspx?SchId=" + _ExamScheduleId + "\")</script>");
            //ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "openIncpopup('StudExamReport.aspx?SchId=" + _ExamScheduleId + "');", true);
        }


        private Document LoadPDFPageMark(List<ExamICSEGridReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, string _PhysicalPath, ExamNode[] Total_ExamDetails, ExamNode[] _SubDetailsPassMark, int StudentId, MysqlClass _Mysqlobj, ParentLogin MyParent)
        {

            Document document = new Document();

            int _Header = 0;
            int _MainReport = 0;
            int _Performance = 0;
         
            int _TeacherRemark = 0;
            int _AverageReport = 0;
            int _Summary = 0;

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

                   // if (_Header != 0)
                     //   Top.AddImage("head.JPG");
                    //document.LastSection.AddParagraph("St. FRANCIS");
                    //else
                     //   Top.AddFormattedText("\n\n\n\n\n\n\n\n\n\n\n\n");


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
                    cell.AddParagraph("Class Strength");
                    cell = row.Cells[4];
                    // cell.Format.Font.Bold = true;
                    cell.AddParagraph(": " + ClsObj[i].m_StudDetails.Strength + "");

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

                    MigraDoc.DocumentObjectModel.Tables.Table _BaseTable = GetTableFormat();
                    MigraDoc.DocumentObjectModel.Tables.Table TableMain = new MigraDoc.DocumentObjectModel.Tables.Table();


                  
                    TableMain = GetMainReportDetails(ClsObj[i].m_ExamMarks, _ExamDetails, _SubDetails, ClsObj[i].m_Average, ClsObj[i].m_TotalMarks, ClsObj[i].m_Ranks, ClsObj[i].m_Grade, ClsObj[i].m_MaxTotal, _SubDetailsPassMark,  ClsObj, ClsObj[i].m_StudentId, _BaseTable);
                    document.LastSection.Add(TableMain);
                   
                    MigraDoc.DocumentObjectModel.Tables.Table TotalExam = GetTotalExam(ClsObj[i].m_T_ExamMarks, Total_ExamDetails, _SubDetails, _AverageReport,_Mysqlobj,  MyParent);
                    document.LastSection.Add(TotalExam);
                    
                    Paragraph bottom = section.AddParagraph();
                    bottom.Format.Font = fontMain3;
                    if (_TeacherRemark != 0)
                    {
                        bottom.Format.Alignment = ParagraphAlignment.Left;
                        bottom.AddFormattedText("Teacher's Remark");
                        bottom.AddFormattedText("\n\n");
                        bottom.AddFormattedText(ClsObj[i].m_Remark[_ExamDetails.Length - 1].ToString());
                        bottom.AddFormattedText("\n\n");
                        bottom.AddFormattedText("\n\n");

                    }
                    else
                    {
                        bottom.AddFormattedText("\n\n\n\n\n\n\n\n\n\n");
                    }

                    bottom.AddFormattedText("Signature: Class Teacher\t\t\tPrincipal\t\t\tParent");
                }

            }


            return document;

        }


    


        private MigraDoc.DocumentObjectModel.Tables.Table GetTotalExam(MarkNode[,] markNode, ExamNode[] Total_ExamDetails, ExamNode[] _SubDetails, int _AverageReport, MysqlClass _Mysqlobj, ParentLogin MyParent )
        {
            MigraDoc.DocumentObjectModel.Font fontMain2 = new MigraDoc.DocumentObjectModel.Font("Times New Roman", 10);
            MigraDoc.DocumentObjectModel.Tables.Table table3 = new MigraDoc.DocumentObjectModel.Tables.Table();
            table3.Format.Font = fontMain2;
            double[] avgMarks, MaxMark;
            int[] TotalExam;

            table3.Borders.Width = 0.5;
            Column column3;

            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2.7));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            //column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            //column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            //column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            //column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(0.86));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(0.86));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(0.86));

            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(0.86));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(0.86));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(0.86));


            Row row1 = table3.AddRow();



            Cell cell1 = row1.Cells[0];


            //cell1.MergeRight = 12;
            cell1.MergeRight = 14;
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

            for (int i = 0; i <= Total_ExamDetails.Length; i++)
            {

                if (i == Total_ExamDetails.Length)
                {
                    if (Total_ExamDetails.Length != 1 && _AverageReport == 1)
                    {
                        cell1 = row1.Cells[0];

                        cell1.AddParagraph("\n\n");
                        cell1 = row1.Cells[1];

                        cell1.AddParagraph("\n\nAverage");

                        for (int j = 0; j < _SubDetails.Length; j++)
                        {
                            cell1 = row1.Cells[2 + j];
                            cell1.Format.Alignment = ParagraphAlignment.Center;
                            if (avgMarks[j] == 0 || avgMarks[j] == -1)
                                cell1.AddParagraph("\n");
                            else
                            {
                                if (MaxMark[j] != 0)
                                {
                                    avgMarks[j] = (avgMarks[j] / (Total_ExamDetails.Length * 100)) * 100;
                                }
                                else
                                {
                                    avgMarks[j] = 0;
                                }

                                string[] _Mrak = avgMarks[j].ToString("0.00").Split('.');

                                if (_Mrak[1].ToString() == "00")

                                    cell1.AddParagraph("\n\n" + Math.Round(avgMarks[j]).ToString());
                                else
                                    cell1.AddParagraph("\n\n" + avgMarks[j].ToString("0.00"));


                            }

                        }
                    }
                }
                else
                {
                    cell1 = row1.Cells[0];

                    cell1.AddParagraph((i + 1) + "\n");
                    cell1 = row1.Cells[1];

                    if (Total_ExamDetails[i].Name.Length > 11)
                        cell1.AddParagraph(Total_ExamDetails[i].Name.Substring(0, 12) + "\n");
                    else
                        cell1.AddParagraph(Total_ExamDetails[i].Name + "\n");


                    for (int j = 0; j < _SubDetails.Length; j++)
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

                            cell1.AddParagraph(markNode[i, j].MaxMark.ToString());

                            double max_Mark = GetMaxMarkFromExamScheduleIdandSubjectId(Total_ExamDetails[i].Id, _SubDetails[j].Id, _Mysqlobj,  MyParent);

                            double mark = (markNode[i, j].MaxMark / max_Mark) * 100;
                            if (i != 0)
                                MaxMark[j] = MaxMark[j] + max_Mark;
                            else
                                MaxMark[j] = max_Mark;

                            if (i != 0)
                                avgMarks[j] = avgMarks[j] + mark;
                            else
                                avgMarks[j] = mark;
                        }
                    }
                }



            }

            return table3;
        }

        public double GetMaxMarkFromExamScheduleIdandSubjectId(int ExamSchduleId, int SubjectId, MysqlClass _Mysqlobj, ParentLogin MyParent)
        {
            double MaxMArk = 0;
            string sql = "select tblclassexamsubmap.MaxMark from tblclassexamsubmap inner join tblexamschedule on  tblexamschedule.ClassExamId= tblclassexamsubmap.ClassExamId where tblclassexamsubmap.SubId=" + SubjectId + " and tblexamschedule.Id=" + ExamSchduleId + "";
            DataSet _Dt = _Mysqlobj.ExecuteQueryReturnDataSet(sql);
            if (_Dt != null && _Dt.Tables[0].Rows.Count > 0)
            {
                double.TryParse(_Dt.Tables[0].Rows[0]["MaxMark"].ToString(), out  MaxMArk);
            }
            return MaxMArk;
        }






        private MigraDoc.DocumentObjectModel.Tables.Table GetTableFormat()
        {
            MigraDoc.DocumentObjectModel.Font font1 = new MigraDoc.DocumentObjectModel.Font("Times New Roman", 11);
            Column column;
            MigraDoc.DocumentObjectModel.Tables.Table _MarkDetails = new MigraDoc.DocumentObjectModel.Tables.Table();
            _MarkDetails.Rows.Height = 15;

            _MarkDetails.Format.Font = font1;
            _MarkDetails.Borders.Width = 0.5;

            column = _MarkDetails.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1));
            column = _MarkDetails.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(7));
            column = _MarkDetails.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2.5));
            column = _MarkDetails.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2.5));
            column = _MarkDetails.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2.5));
            column = _MarkDetails.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2.5));


            return _MarkDetails;
        }


        private MigraDoc.DocumentObjectModel.Tables.Table GetMainReportDetails(MarkNode[,] markNode, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, double[] _Average, MarkNode[] _TotalMarks, MarkNode[] _Ranks, string[] _Grade, double[] _MaxTotal, ExamNode[] _SubDetailsPassMark,  List<ExamICSEGridReportClass> ClsObj, int _StudentId, MigraDoc.DocumentObjectModel.Tables.Table StartTable)
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
            cell.AddParagraph("Max Mark");
            cell = row.Cells[3];
            cell.AddParagraph("Pass Mark");
            cell = row.Cells[4];
            cell.AddParagraph("Marks Obtained");
            cell = row.Cells[5];
            cell.AddParagraph("Percentage");
         
            row = _MarkDetails.AddRow();
            row.Height = 150;
            row.Format.Alignment = ParagraphAlignment.Left;
            cell = row.Cells[0];

            for (int Subjct = 0; Subjct < _SubDetails.Length; Subjct++)
            {
                int temp = 0;
                cell = row.Cells[0];
                cell.Format.Alignment = ParagraphAlignment.Center;
                int i = Subjct + 1;
                cell.AddParagraph(i.ToString() + "\n");
                cell = row.Cells[1];
                string subName = "";
                if (_SubDetails[Subjct].Name.Length > 24)
                    subName = _SubDetails[Subjct].Name.Substring(0, 21) + "..";
                else
                    subName = _SubDetails[Subjct].Name;
                cell.AddParagraph(subName + "\n");
                cell = row.Cells[2];
                cell.Format.Alignment = ParagraphAlignment.Center;
                cell.AddParagraph(_SubDetails[Subjct].MaxMark.ToString() + "\n");
                cell = row.Cells[3];
                cell.Format.Alignment = ParagraphAlignment.Center;
                cell.AddParagraph(_SubDetailsPassMark[Subjct].MaxMark.ToString("0") + "\n");
                cell = row.Cells[4];
                cell.Format.Alignment = ParagraphAlignment.Center;
                if (markNode[_ExamDetails.Length - 1, Subjct].MaxMark.ToString() == "0" || markNode[_ExamDetails.Length - 1, Subjct].MaxMark.ToString() == "-1")
                {
                    cell.AddParagraph("A" + "\n");
                    temp = 1;
                }

                else
                {
                    cell.AddParagraph(markNode[_ExamDetails.Length - 1, Subjct].MaxMark.ToString() + "\n");
                }

                cell = row.Cells[5];
                cell.Format.Alignment = ParagraphAlignment.Center;
                if (temp == 1)
                {
                    cell.AddParagraph("\n");
                }
                else
                {
                    double persentage = (markNode[_ExamDetails.Length - 1, Subjct].MaxMark / _SubDetails[Subjct].MaxMark) * 100;
                    if (persentage == -1)
                        cell.AddParagraph("\n");
                    else
                        cell.AddParagraph(persentage.ToString("0") + "\n");
                }
                

            }
            double _TotalPassMark = 0;
            for (int j = 0; j < _SubDetailsPassMark.Length; j++)
                _TotalPassMark = _TotalPassMark + _SubDetailsPassMark[j].MaxMark;

            row = _MarkDetails.AddRow();
            row.Height = 25;
            cell = row.Cells[0];
            cell.AddParagraph("");
            cell = row.Cells[1];
            cell.Format.Alignment = ParagraphAlignment.Center;
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.AddParagraph("Total");

            cell = row.Cells[2];
            cell.Format.Alignment = ParagraphAlignment.Center;
            cell.AddParagraph(_MaxTotal[_ExamDetails.Length - 1].ToString());
            cell = row.Cells[3];
            cell.Format.Alignment = ParagraphAlignment.Center;
            cell.AddParagraph(_TotalPassMark.ToString());
            cell = row.Cells[4];
            cell.Format.Alignment = ParagraphAlignment.Center;
            cell.AddParagraph(_TotalMarks[_ExamDetails.Length - 1].MaxMark.ToString());
            cell = row.Cells[5];
            cell.Format.Alignment = ParagraphAlignment.Center;
           

            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.AddParagraph("Per :" + Math.Round(_Average[_ExamDetails.Length - 1]).ToString());

            return _MarkDetails;
        }

       



          private void GetExamWiseAverage(ExamNode[] _ExamDetails, ExamNode[] _SubDetails, int count, out ExamNode[] _SubjectWiseClassPer, out double _ClassPersentage, MysqlClass _Mysqlobj, ParentLogin MyParent)
        {
            _SubjectWiseClassPer = new ExamNode[_SubDetails.Length];

            _ClassPersentage = 0;
            string _sqlMarksColum = "select tblexammark.MarkColumn, tblexammark.SubjectId  from tblexammark where tblexammark.ExamSchId=" + _ExamDetails[_ExamDetails.Length - 1].Id + " ORDER BY tblexammark.SubjectOrder";
            DataSet ColumnDetails = _Mysqlobj.ExecuteQueryReturnDataSet(_sqlMarksColum);

            string sqlMark = "";
            if (ColumnDetails != null && ColumnDetails.Tables[0].Rows.Count > 0)
            {
                string _Marks = "";

                foreach (DataRow dr in ColumnDetails.Tables[0].Rows)
                {
                    if (_Marks != "")
                        _Marks = _Marks + ",";
                    _Marks = _Marks + "Sum(" + dr["MarkColumn"].ToString() + ") as " + dr["MarkColumn"].ToString() + " ";
                }

                sqlMark = "Select " + _Marks + " from  tblstudentmark where  tblstudentmark.ExamSchId=" + _ExamDetails[_ExamDetails.Length - 1].Id;
            }

            if (sqlMark != "")
            {
                DataSet _Marks = _Mysqlobj.ExecuteQueryReturnDataSet(sqlMark);

                if (_Marks != null && _Marks.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < _SubDetails.Length; j++)
                    {
                        foreach (DataRow dr in ColumnDetails.Tables[0].Rows)
                        {
                            foreach (DataRow dr1 in _Marks.Tables[0].Rows)
                            {
                                if (int.Parse(dr["SubjectId"].ToString()) == _SubDetails[j].Id)
                                {
                                    string _Mark = dr["MarkColumn"].ToString();
                                    double Avg_Marks = ((double.Parse(dr1[_Mark].ToString()) / (_SubDetails[j].MaxMark * count)) * 100.0);
                                    _SubjectWiseClassPer[j].MaxMark = Avg_Marks;
                                }

                            }
                        }
                    }

                }

            }

            sqlMark = "select  sum(tblstudentmark.TotalMark),sum( tblstudentmark.TotalMax) from  tblstudentmark where  tblstudentmark.ExamSchId=" + _ExamDetails[_ExamDetails.Length - 1].Id;
            MyReader = _Mysqlobj.ExecuteQuery(sqlMark);
            if (MyReader.HasRows)
            {
                _ClassPersentage = (double.Parse(MyReader.GetValue(0).ToString()) / double.Parse(MyReader.GetValue(1).ToString()) * 100.0);
            }


        }

        //private void GetSubDetails(int ClassId, int ExamId, out ExamNode[] _SubDetails)
        //{
        //    //string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=tblexamschedule.ClassExamId and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId  where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblclassexam.ClassId=" + ClassId + " and tblexammaster.Id=" + ExamId;
        //    string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark ,tblsubjects.SubjectCode,tblexammark.SubjectOrder  from tblclassexamsubmap inner JOIN tblsubjects on tblclassexamsubmap.SubId= tblsubjects.Id inner JOIN tblclassexam on tblclassexam.id= tblclassexamsubmap.ClassExamId    inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.id  inner JOIN tblexammark on  tblexammark.SubjectId=tblsubjects.Id and tblexammark.ExamSchId =tblexamschedule.id   where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "  and tblclassexam.ClassId=" + ClassId + " and tblclassexam.ExamId=" + ExamId + " order by tblexammark.SubjectOrder";

        //    MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);

        //    if (MyReader.HasRows)
        //    {
        //        int i = 0;
        //        int Count = MyReader.RecordsAffected;
        //        _SubDetails = new ExamNode[Count];

        //        while (MyReader.Read())
        //        {
        //            _SubDetails[i].Id = int.Parse(MyReader.GetValue(0).ToString());
        //            _SubDetails[i].Name = MyReader.GetValue(1).ToString();
        //            _SubDetails[i].MaxMark = double.Parse(MyReader.GetValue(2).ToString());
        //            _SubDetails[i].SubCode = MyReader.GetValue(3).ToString();
        //            i++;
        //        }
        //    }
        //    else
        //    {
        //        _SubDetails = new ExamNode[0];
        //    }

        //}

        private void GetSubDetailsWithPassmark(int ClassId, int ExamId, out ExamNode[] _SubDetails , MysqlClass _Mysqlobj, ParentLogin MyParent)
        {
            // Dominic modified on 18/10/2011
            //string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MinMark from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=tblexamschedule.ClassExamId and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId  where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblclassexam.ClassId=" + ClassId + " and tblexammaster.Id=" + ExamId;
            string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MinMark from tblclassexamsubmap inner JOIN tblsubjects on tblclassexamsubmap.SubId= tblsubjects.Id  inner JOIN tblclassexam on tblclassexam.id= tblclassexamsubmap.ClassExamId inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.id where tblexamschedule.BatchId=" + MyParentInfo.CurrentBatchId + " and tblclassexam.ClassId=" + ClassId + " and  tblexamschedule.Id=" + ExamId ;
            MyReader = _Mysqlobj.ExecuteQuery(sql);

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
                    i++;
                }
            }
            else
            {
                _SubDetails = new ExamNode[0];
            }

        }

        //private void GetExamDetails(int ClassId, int ExamId, out ExamNode[] _ExamDetails)
        //{

        //    string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period, tblexamschedule.GradeMasterId from tblexammaster inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id     where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblexammaster.Id=" + ExamId + " and tblexamschedule.Status='Completed'  order by tblexamschedule.id ASC";
        //    MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);

        //    if (MyReader.HasRows)
        //    {
        //        int i = 0;
        //        int Count = MyReader.RecordsAffected;
        //        _ExamDetails = new ExamNode[Count];

        //        while (MyReader.Read())
        //        {
        //            _ExamDetails[i].Id = int.Parse(MyReader.GetValue(0).ToString());
        //            _ExamDetails[i].Name = MyReader.GetValue(1).ToString();
        //            _ExamDetails[i].GradeMasterId = int.Parse(MyReader.GetValue(3).ToString());

        //            string sql1 = "select Max( tblexammark.ExamDate) from tblexammark where tblexammark.ExamSchId=" + int.Parse(MyReader.GetValue(0).ToString());
        //            OdbcDataReader dr = MyClassMang.m_MysqlDb.ExecuteQuery(sql1);

        //            if (dr.HasRows)
        //            {

        //                _ExamDetails[i].Date = DateTime.Parse(dr.GetValue(0).ToString());
        //            }

        //            i++;
        //        }
        //    }
        //    else
        //    {
        //        _ExamDetails = new ExamNode[0];
        //    }
        //}

        ////private void GetTotalExamDetails(int ClassId, int ExamId, out ExamNode[] _ExamDetails)
        //{

        //    string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period from tblexammaster inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id     where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark)  and tblexamschedule.Status='Completed' and tblexammaster.ExamTypeId=1  order by tblexamschedule.id ASC";
        //    MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);

        //    if (MyReader.HasRows)
        //    {
        //        int i = 0;
        //        int Count = MyReader.RecordsAffected;
        //        _ExamDetails = new ExamNode[Count];

        //        while (MyReader.Read())
        //        {
        //            _ExamDetails[i].Id = int.Parse(MyReader.GetValue(0).ToString());
        //            _ExamDetails[i].Name = MyReader.GetValue(1).ToString();
        //            string sql1 = "select Max( tblexammark.ExamDate) from tblexammark where tblexammark.ExamSchId=" + int.Parse(MyReader.GetValue(0).ToString());
        //            OdbcDataReader dr = MyClassMang.m_MysqlDb.ExecuteQuery(sql1);
        //            if (dr.HasRows)
        //            {

        //                _ExamDetails[i].Date = DateTime.Parse(dr.GetValue(0).ToString());
        //            }

        //            i++;
        //        }
        //    }
        //    else
        //    {
        //        _ExamDetails = new ExamNode[0];
        //    }
        //}

    

        #endregion


        #region grade report : pdf

        protected void Grd_ExamList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //grade 
            
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            int _ExamScheduleId = int.Parse(Grd_ExamList.Rows[e.RowIndex].Cells[1].Text.ToString());
              LoadReport(MyParentInfo.CLASSID, _ExamScheduleId, MyParentInfo.StudentId, _mysqlObj, MyParent,"Grade");
           
            ReleaseResourse(_mysqlObj, MyParent);

        }

        private void LoadReport(int ClassId, int ExamId, int StudentId, MysqlClass _Mysqlobj, ParentLogin MyParent,string examType)
        {
            DataSet StudList = new DataSet();
            ExamNode[] _ExamDetails;
            ExamNode[] _SubDetails;
            ExamNode[] _SubDetailsPassMark;
           
            ExamNode[,] _ClassMarks;


            ExamNode[] Total_ExamDetails = null;

            GetSubDetailsWithPassmark(ClassId, ExamId, out _SubDetailsPassMark, _Mysqlobj);
        
            DataSet TopStudentList = new DataSet();
            List<ExamICSEGridReportClass> ClsObj = new List<ExamICSEGridReportClass>();
            ExamICSEGridReportClass _ReportObj;

            try
            {

                StudList = GetStudDetails(StudentId, ClassId, ExamId,  _Mysqlobj,  MyParent);
                GetExamDetails(ClassId, ExamId, out _ExamDetails,  _Mysqlobj,  MyParent);
                GetSubDetails(ClassId, _ExamDetails[_ExamDetails.Length-1].Id, out _SubDetails, _Mysqlobj, MyParent);

                GetTotalExamDetails(ClassId, ExamId, out Total_ExamDetails,  _Mysqlobj,  MyParent);

                _ClassMarks = new ExamNode[StudList.Tables[0].Rows.Count, _SubDetails.Length];

                foreach (DataRow Dr in StudList.Tables[0].Rows)
                {

                    _ReportObj = new ExamICSEGridReportClass(_Mysqlobj, int.Parse(Dr[0].ToString()));
                    _ReportObj.ExamDetails(_ExamDetails, _SubDetails, Dr, "Main", 0);

                    if (Total_ExamDetails != null)
                        _ReportObj.ExamDetails(Total_ExamDetails, _SubDetails, Dr, "Total", 0);


                    ClsObj.Add(_ReportObj);
                }

                CreateReport(ClsObj, _ExamDetails, _SubDetails, Total_ExamDetails, StudentId, _Mysqlobj, MyParent, examType, _SubDetailsPassMark);

            }

            catch (Exception _error)
            {
                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "alert(\" Error while loading. Message: " + _error.Message + " \");", true);
                //Response.Write("<script>alert(\"Please make sure that all exams reports are generated correctly in exam dash board\");</script>");
                //Response.Write("<script>window.close()</script>");
            }
        }
        private void GetSubDetailsWithPassmark(int ClassId, int ExamId, out ExamNode[] _SubDetails, MysqlClass _Mysqlobj)
        {
            // Dominic modified on 18/10/2011
            //string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MinMark from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=tblexamschedule.ClassExamId and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId  where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblclassexam.ClassId=" + ClassId + " and tblexammaster.Id=" + ExamId;
            string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MinMark, tblclassexamsubmap.MaxMark, tblexammark.SubjectOrder  from tblclassexamsubmap inner JOIN tblsubjects on tblclassexamsubmap.SubId= tblsubjects.Id  inner JOIN tblclassexam on tblclassexam.id= tblclassexamsubmap.ClassExamId inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.id inner JOIN tblexammark on  tblexammark.SubjectId=tblsubjects.Id and tblexammark.ExamSchId =tblexamschedule.id where tblexamschedule.BatchId=" + MyParentInfo.CurrentBatchId + " and tblclassexam.ClassId=" + ClassId + " and tblexamschedule.Id=" + ExamId + " order by tblexammark.SubjectOrder"; 
            MyReader = _Mysqlobj.ExecuteQuery(sql);

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
                    i++;
                }
            }
            else
            {
                _SubDetails = new ExamNode[0];
            }

        }

        private void CreateReport(List<ExamICSEGridReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, ExamNode[] Total_ExamDetails, int StudentId, MysqlClass _Mysqlobj, ParentLogin MyParent, string examType, ExamNode[] _SubDetailsPassMark)
        {
            string _physicalpath = WinerUtlity.GetParentLoginAbsoluteFilePath(MyParentInfo.SchoolObject, Server.MapPath("")) + "\\PDF_Files\\" ;
            Document document;
            if(examType=="Grade")

             document = LoadPDFPage(ClsObj, _ExamDetails, _SubDetails, _physicalpath, Total_ExamDetails, StudentId, _Mysqlobj, MyParent);
            else

                document = LoadPDFPageMark(ClsObj, _ExamDetails, _SubDetails, _physicalpath, Total_ExamDetails, _SubDetailsPassMark, StudentId, _Mysqlobj, MyParent);
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

            filename = _physicalpath + "\\ICSEGrade_Class_" + MainName + ".pdf";

            pdfRenderer.PdfDocument.Save(filename);
            // ...and start a viewer.

            //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=Grade_Class_" + MainName + ".pdf\");", true);
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "null", "window.open(\"OpenPdfPage.aspx?PdfName=ICSEGrade_Class_" + MainName + ".pdf\",'Info','status=1, width=1000, height=700,,resizable=1,scrollbars=1');", true);


        }

      

        private Document LoadPDFPage(List<ExamICSEGridReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, string _PhysicalPath, ExamNode[] Total_ExamDetails, int StudentId, MysqlClass _Mysqlobj, ParentLogin MyParent)
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
                 //   Top.AddFormattedText("\n\n\n\n\n\n\n\n\n\n\n\n");

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

                    MigraDoc.DocumentObjectModel.Tables.Table _BaseTable = GetTableFormatGrade();
                    MigraDoc.DocumentObjectModel.Tables.Table TableMain = new MigraDoc.DocumentObjectModel.Tables.Table();


                    TableMain = GetMainReportDetails(ClsObj[i].m_ExamMarks, _ExamDetails, _SubDetails, ClsObj[i].m_Average, ClsObj[i].m_TotalMarks, ClsObj[i].m_Ranks, ClsObj[i].m_Grade, ClsObj[i].m_MaxTotal, ClsObj, ClsObj[i].m_StudentId, _BaseTable,_Mysqlobj, MyParent);
                    document.LastSection.Add(TableMain);

                    MigraDoc.DocumentObjectModel.Tables.Table TotalExam = GetTotalExam(ClsObj[i].m_T_ExamMarks, Total_ExamDetails, _SubDetails,_Mysqlobj, MyParent);
                    document.LastSection.Add(TotalExam);

                    Paragraph bottom = section.AddParagraph();
                    bottom.Format.Font = fontMain3;

                    bottom.Format.Alignment = ParagraphAlignment.Left;
                    bottom.AddFormattedText("Teacher's Remark");
                    bottom.AddFormattedText("\n\n");
                    bottom.AddFormattedText(ClsObj[i].m_Remark[_ExamDetails.Length - 1].ToString());
                    bottom.AddFormattedText("\n\n");


                    bottom.AddFormattedText("Signature: Class Teacher\t\t\tPrincipal\t\t\tParent");
                }


            }
            return document;
        }


        private MigraDoc.DocumentObjectModel.Tables.Table GetMainReportDetails(MarkNode[,] markNode, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, double[] _Average, MarkNode[] _TotalMarks, MarkNode[] _Ranks, string[] _Grade, double[] _MaxTotal, List<ExamICSEGridReportClass> ClsObj, int _StudentId, MigraDoc.DocumentObjectModel.Tables.Table StartTable,MysqlClass _Mysqlobj, ParentLogin MyParent)
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

                gradeId = GetGradeMaster(_ExamDetails[_ExamDetails.Length - 1].Id, _Mysqlobj,  MyParent);
                Grade = GetGradeDataSet(gradeId, _Mysqlobj,  MyParent);

                if (markNode[_ExamDetails.Length - 1, Subjct].MaxMark.ToString() == "0" || markNode[_ExamDetails.Length - 1, Subjct].MaxMark.ToString() == "-1")
                {
                    cell.AddParagraph("Absent" + "\n");
                    temp = 1;
                }

                else
                {
                    if (Grade != null && Grade.Tables != null && Grade.Tables[0].Rows.Count > 0)
                    {
                        cell.AddParagraph(GetGradeFromMarks(Grade, double.Parse(markNode[_ExamDetails.Length - 1, Subjct].MaxMark.ToString()), _SubDetails[Subjct].MaxMark)).ToString();
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

        private MigraDoc.DocumentObjectModel.Tables.Table GetTotalExam(MarkNode[,] markNode, ExamNode[] Total_ExamDetails, ExamNode[] _SubDetails, MysqlClass _Mysqlobj, ParentLogin MyParent)
        {
            MigraDoc.DocumentObjectModel.Font fontMain2 = new MigraDoc.DocumentObjectModel.Font("Times New Roman", 10);
            MigraDoc.DocumentObjectModel.Tables.Table table3 = new MigraDoc.DocumentObjectModel.Tables.Table();
            table3.Format.Font = fontMain2;
            double[] avgMarks, MaxMark;
            int[] TotalExam;

            table3.Borders.Width = 0.5;
            Column column3;

            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2.7));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));



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

                GetSubDetails(MyParentInfo.CLASSID, Total_ExamDetails[i].Id, out _SubDetails, _Mysqlobj, MyParent);
                gradeId = GetGradeMaster(Total_ExamDetails[i].Id, _Mysqlobj, MyParent);
                Grade = GetGradeDataSet(gradeId, _Mysqlobj, MyParent);


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

                    if (Total_ExamDetails[i].Name.Length > 11)
                        cell1.AddParagraph(Total_ExamDetails[i].Name.Substring(0, 12) + "\n");
                    else
                        cell1.AddParagraph(Total_ExamDetails[i].Name + "\n");

                  
                    for (int j = 0; j < _SubDetails.Length; j++)
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
                }



            }

            return table3;
        }

      private DataSet GetGradeDataSet(int _GradeMasterId, MysqlClass _Mysqlobj, ParentLogin MyParent)
        {
            string _sql = "select tblgrade.Grade, tblgrade.LowerLimit,NumericalGrade,Result,Award from tblgrade where tblgrade.GradeMasterId=" + _GradeMasterId + " and   tblgrade.`Status`=1   order by tblgrade.id asc";
            DataSet Dt = _Mysqlobj.ExecuteQueryReturnDataSet(_sql);
            return Dt;
        }

      private int GetGradeMaster(int _examscid , MysqlClass _Mysqlobj, ParentLogin MyParent)
        {
            int _GradeMasterId = 0;

            string sqlstr = "SELECT GradeMasterId FROM tblexamschedule WHERE Id=" + _examscid;
            OdbcDataReader m_MyReader = _Mysqlobj.ExecuteQuery(sqlstr);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out _GradeMasterId);
            }

            return _GradeMasterId;
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

        private MigraDoc.DocumentObjectModel.Tables.Table GetTableFormatGrade()
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

        private void GetTotalExamDetails(int ClassId, int ExamId, out ExamNode[] _ExamDetails, MysqlClass _Mysqlobj, ParentLogin MyParent)
        {

            string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period from tblexammaster inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id     where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark)  and tblexamschedule.Status='Completed' and tblexammaster.ExamTypeId=1  order by tblexamschedule.id ASC";
            MyReader = _Mysqlobj.ExecuteQuery(sql);

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
                    OdbcDataReader dr = _Mysqlobj.ExecuteQuery(sql1);
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
        
        private void GetSubDetails(int ClassId, int ExamSchId, out ExamNode[] _SubDetails, MysqlClass _Mysqlobj, ParentLogin MyParent)
        {
            //string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=tblexamschedule.ClassExamId and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId  where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblclassexam.ClassId=" + ClassId + " and tblexammaster.Id=" + ExamId;
            string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark ,tblsubjects.SubjectCode,tblexammark.SubjectOrder  from tblclassexamsubmap inner JOIN tblsubjects on tblclassexamsubmap.SubId= tblsubjects.Id inner JOIN tblclassexam on tblclassexam.id= tblclassexamsubmap.ClassExamId    inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.id  inner JOIN tblexammark on  tblexammark.SubjectId=tblsubjects.Id and tblexammark.ExamSchId =tblexamschedule.id   where tblexamschedule.BatchId=" + MyParentInfo.CurrentBatchId + "  and tblclassexam.ClassId=" + ClassId + " and tblexamschedule.id=" + ExamSchId + " order by tblexammark.SubjectOrder";

            MyReader =_Mysqlobj.ExecuteQuery(sql);

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


        private void GetExamDetails(int ClassId, int ExamId, out ExamNode[] _ExamDetails , MysqlClass _Mysqlobj, ParentLogin MyParent)
        {

            string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period, tblexamschedule.GradeMasterId from tblexammaster inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id     where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id=" + ExamId + " and tblexamschedule.Status='Completed'  order by tblexamschedule.id ASC";
            MyReader = _Mysqlobj.ExecuteQuery(sql);

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
                    OdbcDataReader dr = _Mysqlobj.ExecuteQuery(sql1);

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


        private DataSet GetStudDetails(int StudentId, int ClassId, int ExamId, MysqlClass _Mysqlobj, ParentLogin MyParent)
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

            string sql_Student = "select tblview_student.Id, tblview_student.StudentName, tblview_student.RollNo, tblclass.ClassName,date_Format(tblview_student.DOB,'%d/%m/%Y') as DOB , tblview_student.AdmitionNo,tblclass.Id as ClassId from tblview_student inner join tblclass on tblclass.Id= tblview_student.ClassId inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblview_student.Id  where tblview_student.ClassId=" + ClassId + " and tblstudentclassmap.BatchId=" + MyParentInfo.CurrentBatchId + " AND tblview_student.Id IN( select tblstudentmark.StudId from tblstudentmark inner join tblexamschedule on tblexamschedule.Id = tblstudentmark.ExamSchId inner join  tblclassexam on tblclassexam.Id=  tblexamschedule.ClassExamId where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id = " + ExamId + " and tblexamschedule.Status='Completed'   and tblview_student.Id=" + StudentId + ") ";

            if (StudentId != 0)
                sql_Student = sql_Student + " and tblview_student.Id=" + StudentId;
            sql_Student = sql_Student + "  order by tblview_student.RollNo";


            DataSet _studList = _Mysqlobj.ExecuteQueryReturnDataSet(sql_Student);

        

            foreach (DataRow dr_values in _studList.Tables[0].Rows)
            {

                dr = _StdentSet.Tables["Student"].NewRow();
                count = GetClassStrength(dr_values["ClassId"].ToString(), _Mysqlobj);

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

                Attendance MyAttendence = new Attendance(_Mysqlobj);
                MyAttendence.GetCurrentBatchNewattendanceDetails(int.Parse(dr_values["Id"].ToString()), out  _no_workingdays, out  _no_presentdays, out _no_absentdays, out _no_holidays, out _no_halfdays, out _attendencepersent, MyParentInfo.CurrentBatchId);

               dr_values["Attendance"] = _no_presentdays + "/" + _no_workingdays;
            }
            return _StdentSet;

        }


        private int GetClassStrength(string ClassId, MysqlClass _Mysqlobj)
        {
            int Count = 0;
            string sql = "select count(tblstudentclassmap.StudentId) as totalStud from  tblclass  inner join tblstudentclassmap  on tblstudentclassmap.ClassId=tblclass.Id  inner join tblstudent on tblstudent.Id=tblstudentclassmap.StudentId inner join tblstandard on tblstandard.Id = tblclass.Standard where tblclass.Id=" + ClassId + " and tblstudent.Status=1 AND tblstudentclassmap.batchId=" + MyParentInfo.CurrentBatchId;
            MyReader = _Mysqlobj.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out Count);
            }
            return Count;
        }

        #endregion
    
    }
    
}
