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
using WinEr;
using WebChart;
using System.Drawing;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Collections.Generic;
using WinBase;

namespace WinEr
{
    public struct ExamNode
    {
        public int      Id;
        public string   Name;
        public double   MaxMark;
        public int      Group;
        public string   GroupName;
        public DateTime Date;
        public int      GradeMasterId;
        public string   Desc;
        public string   SubCode;
        public int      GradeSubject;

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

    public class ComprehensiveExamClass
    {
        public MysqlClass   m_MysqlDb;
        public ExamNode[]   m_Eaxams;
        public ExamNode[]   m_Subjects;
        public MarkNode[,]  m_ExamMarks;
        public MarkNode[]   m_TotalMarks;
        public MarkNode[]   m_Ranks;
        public StudDetails  m_StudDetails;

        public int          m_StudentId;
        public string[]     m_Grade;
        public double[]     m_Average;
        public double[]     m_MaxTotal;

        public ComprehensiveExamClass(MysqlClass _Mysqlobj, int _StudentId)
        {
            m_MysqlDb = _Mysqlobj;
            m_StudentId = _StudentId;

        }

        public bool ExamDetails(ExamNode[] _Eaxams, ExamNode[] m_Subjects, DataRow _StudDetails)
        {
            m_ExamMarks     = new MarkNode[_Eaxams.Length, m_Subjects.Length];
            m_TotalMarks    = new MarkNode[_Eaxams.Length];
            m_Ranks         = new MarkNode[_Eaxams.Length];
            m_Grade         = new string[_Eaxams.Length];
            m_Average       = new double[_Eaxams.Length];
            m_MaxTotal      = new double[_Eaxams.Length];

            m_StudDetails.Id = int.Parse(_StudDetails[0].ToString());
            m_StudDetails.Name = _StudDetails[1].ToString();
            m_StudDetails.RollNum = int.Parse(_StudDetails[2].ToString());
            m_StudDetails.Class = _StudDetails[3].ToString();

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

                    sqlMark = sqlMark + "tblstudentmark.TotalMark, tblstudentmark.TotalMax, tblstudentmark.Avg, tblstudentmark.Rank, tblstudentmark.Grade from  tblstudentmark where  tblstudentmark.ExamSchId=" + _Eaxams[i].Id + " and  tblstudentmark.StudId=" + m_StudentId;
                }

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

            return true;
        }
    }

    public partial class DisplyComprehensiveReport : System.Web.UI.Page
    {
        private iTextSharp.text.Font[] MyFonts = new iTextSharp.text.Font[24];

        private ClassOrganiser  MyClassMang;
        private Attendance      MyAttendence;
        private Incident        MyIncidentMang;
        private KnowinUser      MyUser;
        private OdbcDataReader  MyReader        = null;
        private MysqlClass _Mysqlobj;
        private PdfWriter pdfcls ;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }

            MyUser          = (KnowinUser)Session["UserObj"];
            MyClassMang     = MyUser.GetClassObj();
            MyIncidentMang  = MyUser.GetIncedentObj();
            MyAttendence    = MyUser.GetAttendancetObj();
            //_Mysqlobj       = new MysqlClass(MyUser.ConnectionString);
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

            if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(835))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["ClassId"] != null && Request.QueryString["ExamId"] != null && Request.QueryString["studentId"] != null && Request.QueryString["ClassId"] != "0" && Request.QueryString["ExamId"] != "0")
                    {
                        /*ClassId* ExamId* studentId* MainReport* GraphReport* TopStudList* Disciplinary* MedicalReport* Academic* Attendance* FeeDue* Summary*/

                        LoadReport(int.Parse(Request.QueryString["ClassId"].ToString()), int.Parse(Request.QueryString["ExamId"].ToString()), int.Parse(Request.QueryString["studentId"].ToString()));
                    }

                }
            }
           
        }

        private void LoadReport(int ClassId, int ExamId, int StudentId)
        {
            double                          _ClassAvg;
            DataSet                         StudList                = new DataSet();
            ExamNode[]                      _ExamDetails;
            ExamNode[]                      _SubDetails;
            ExamNode[]                      _SubjectWiseClassAvg;
            SchoolDetails                   _SchoolDetails;
            DataSet                         TopStudentList          = new DataSet();
            List<ComprehensiveExamClass>    ClsObj                  = new List<ComprehensiveExamClass>();
            ComprehensiveExamClass          _ReportObj;

            GetExamDetails(ClassId, ExamId, out _ExamDetails);
            GetSubDetails(ClassId, ExamId, out _SubDetails);
            GetSchoolDetails(out _SchoolDetails);
            CreateTopList(_ExamDetails, _SubDetails, out TopStudentList);
            StudList = GetStudDetails(StudentId, ClassId, ExamId);
            GetExamWiseAverage(_ExamDetails,_SubDetails,StudList.Tables[0].Rows.Count, out _SubjectWiseClassAvg, out _ClassAvg);
           

            foreach (DataRow Dr in StudList.Tables[0].Rows)
            {
                _ReportObj = new ComprehensiveExamClass(_Mysqlobj, int.Parse(Dr[0].ToString()));
                _ReportObj.ExamDetails(_ExamDetails, _SubDetails, Dr);

                ClsObj.Add(_ReportObj);
            }

            CreateReport(ClsObj, _ExamDetails, _SubDetails, _SchoolDetails, TopStudentList, _SubjectWiseClassAvg, _ClassAvg);
        }
        
       private void GetExamWiseAverage(ExamNode[] _ExamDetails, ExamNode[] _SubDetails,int count, out ExamNode[] _SubjectWiseClassAvg, out double _ClassAvg)
       {
           _SubjectWiseClassAvg = new ExamNode[_SubDetails.Length];
           _ClassAvg = 0;
            string _sqlMarksColum = "select tblexammark.MarkColumn, tblexammark.SubjectId  from tblexammark where tblexammark.ExamSchId=" + _ExamDetails[_ExamDetails.Length-1].Id+" ORDER BY tblexammark.SubjectOrder";
            DataSet ColumnDetails = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(_sqlMarksColum);
            string sqlMark = "";

            if (ColumnDetails != null && ColumnDetails.Tables[0].Rows.Count > 0)
            {
                string _Marks = "";

                foreach (DataRow dr in ColumnDetails.Tables[0].Rows)
                {
                    if (_Marks != "")
                        _Marks = _Marks + ",";
                    _Marks = _Marks+"Sum(" + dr["MarkColumn"].ToString() + ") as "+dr["MarkColumn"].ToString()+" ";
                }

                sqlMark = "Select " + _Marks + " from  tblstudentmark where  tblstudentmark.ExamSchId=" + _ExamDetails[_ExamDetails.Length-1].Id;
            }

            if (sqlMark != "")
            {
                DataSet _Marks = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sqlMark);
                
                if (_Marks != null && _Marks.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0;  j< _SubDetails.Length; j++)
                    {
                        foreach (DataRow dr in ColumnDetails.Tables[0].Rows)
                        {
                            foreach (DataRow dr1 in _Marks.Tables[0].Rows)
                            {
                                if (int.Parse(dr["SubjectId"].ToString()) == _SubDetails[j].Id)
                                {
                                    string _Mark=dr["MarkColumn"].ToString();
                                    double Avg_Marks=((double.Parse(dr1[_Mark].ToString()) / (_SubDetails[j].MaxMark * count)) * 100.0);
                                    _SubjectWiseClassAvg[j].MaxMark = Avg_Marks;
                                }

                            }
                        }
                    }
                    
                }
                
            }

            sqlMark = "select  sum(tblstudentmark.TotalMark),sum( tblstudentmark.TotalMax) from  tblstudentmark where  tblstudentmark.ExamSchId=" + _ExamDetails[_ExamDetails.Length - 1].Id;
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sqlMark);
            if (MyReader.HasRows)
            {
                _ClassAvg = (double.Parse(MyReader.GetValue(0).ToString()) / double.Parse(MyReader.GetValue(1).ToString()) * 100.0);
            }

            
        }

       private void CreateReport(List<ComprehensiveExamClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, SchoolDetails _SchoolDetails, DataSet TopStudentList, ExamNode[] _SubjectWiseClassAvg, double _ClassAvg)
       {
            Document DocER = new Document(PageSize.A4, 50, 40, 20, 10);
            try
            {
                string _PhysicalPath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));

                PdfWriter writerER = PdfWriter.GetInstance(DocER, new FileStream(_PhysicalPath + "\\PDF_Files\\ExamReport_Class_" + ClsObj[0].m_StudDetails.Class + ".pdf", FileMode.Create));

                LoadMyFont();
                if (LoadPDFPage(ClsObj, DocER, _ExamDetails, _SubDetails, _PhysicalPath, _SchoolDetails, TopStudentList, writerER,  _SubjectWiseClassAvg,  _ClassAvg))
                { 
                    
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=ExamReport_Class_" + ClsObj[0].m_StudDetails.Class + ".pdf\");", true);
                    Response.Redirect("OpenPdfPage.aspx?PdfName=ExamReport_Class_" + ClsObj[0].m_StudDetails.Class + ".pdf", false);
                    
                }
            }
            catch (Exception _error)
            {

                Response.Write("<script>alert(\"Error in Creation\");</script>");
                Response.Write("<script>window.close()</script>");
            }
            finally
            {
                if (DocER.IsOpen())
                    DocER.Close();
                
            }
        }

       private bool LoadPDFPage(List<ComprehensiveExamClass> ClsObj, Document DocER, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, string _PhysicalPath, SchoolDetails _SchoolDetails, DataSet TopStudentList, PdfWriter writerER, ExamNode[] _SubjectWiseClassAvg, double _ClassAvg)
        {


            int _MainReport = 0;
            int _GraphReport = 0;
            int _TopStudList = 0;
            int _Attendance = 0;
            int _FeeDue = 0;

            int _Summary = 0;

            string _ExamName = _ExamDetails[_ExamDetails.Length - 1].Name;


            if (Request.QueryString["Main Report"] != null)
            {
                int.TryParse(Request.QueryString["Main Report"].ToString(), out _MainReport);
            }
            if (Request.QueryString["Graph Report"] != null)
            {
                int.TryParse(Request.QueryString["Graph Report"].ToString(), out _GraphReport);
            }

            if (Request.QueryString["Top Student Report"] != null)
            {
                int.TryParse(Request.QueryString["Top Student Report"].ToString(), out _TopStudList);
            }

            if (Request.QueryString["Attendance Report"] != null)
            {
                int.TryParse(Request.QueryString["Attendance Report"].ToString(), out _Attendance);
            }
            if (Request.QueryString["Fee Due Report"] != null)
            {
                int.TryParse(Request.QueryString["Fee Due Report"].ToString(), out _FeeDue);

            }
            if (Request.QueryString["Summary"] != null)
            {
                int.TryParse(Request.QueryString["Summary"].ToString(), out _Summary);

            }

            if (DocER.IsOpen())
            {
                DocER.Close();
            }
            DocER.Open();

            iTextSharp.text.Table Tbl_CollegeDetails = ER_LoadCollegeDetailsToTable(_PhysicalPath, _SchoolDetails);

            for (int i = 0; i < ClsObj.Count; i++)
            {

                DocER.NewPage();
                DocER.Add(Tbl_CollegeDetails);
                iTextSharp.text.Table Tbl_StudentDetails = ER_LoadStudentDetailsToTable(ClsObj[i].m_StudDetails, _ExamName);
                DocER.Add(Tbl_StudentDetails);

                if (_MainReport == 1)
                {
                    iTextSharp.text.Table Tbl_MarkDetails = ER_MarkDetailsToTable(ClsObj[i].m_ExamMarks, _ExamDetails, _SubDetails, ClsObj[i].m_Average, ClsObj[i].m_TotalMarks, ClsObj[i].m_Ranks, ClsObj[i].m_Grade, ClsObj[i].m_MaxTotal);
                    if (writerER.FitsPage(Tbl_MarkDetails))
                    {
                        DocER.Add(Tbl_MarkDetails);
                    }
                    else
                    {
                        DocER.NewPage();
                        DocER.Add(Tbl_MarkDetails);
                    }

                }
                int _GraphCount = 0;
                string[] _GraphName;
                iTextSharp.text.Table _Tbl_GraphDetails = new iTextSharp.text.Table(2);
               

                if (_GraphReport == 1 || _TopStudList == 1)
                {
                    iTextSharp.text.Table Tbl_Header = new iTextSharp.text.Table(1);
                    Tbl_Header.Width = 100;
                    float[] headerwidths = { 100 };
                    Tbl_Header.Widths = headerwidths;
                    Tbl_Header.Padding = 1;
                    Tbl_Header.Border = 0;
                    Tbl_Header.AutoFillEmptyCells = true;
                    Tbl_Header.DefaultCell.Border = 0;
                    Tbl_Header.DefaultCell.HorizontalAlignment = 1;
                    Tbl_Header.DefaultCell.VerticalAlignment = 1;
                    Tbl_Header.AddCell(new Phrase("Subject-wise Analysis", MyFonts[5]));

                    DocER.Add(Tbl_Header);
                       
                    CreateSubjectGraph(ClsObj[i].m_ExamMarks, _ExamDetails, _SubDetails, out _GraphName, out _GraphCount);
                    for (int tempId = 0; tempId < _SubDetails.Length; tempId++)
                    {
                        GetPerformanceGraphReport(_GraphName, tempId, _SubDetails, _PhysicalPath, _TopStudList, _GraphReport, TopStudentList, out _Tbl_GraphDetails);
                        //Tbl_Graph
                        if (writerER.FitsPage(_Tbl_GraphDetails))
                        {
                            DocER.Add(_Tbl_GraphDetails);
                        }
                        else
                        {
                            DocER.NewPage();
                            DocER.Add(_Tbl_GraphDetails);
                        }
                    }      
                    
                    
                   
                }

                if (MyUser.HaveModule(20))
                {
                    string sql = "select tblincedenttype.`Type` from tblincedenttype where tblincedenttype.Visibility=1  and  tblincedenttype.IncidentType='NORMAL'  Order By tblincedenttype.Order ASC";
                    DataSet Dt = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

                    if (Dt != null && Dt.Tables[0].Rows.Count>0)
                    {

                        foreach(DataRow Dr in Dt.Tables[0].Rows)
                        {
                            if (Request.QueryString[Dr[0].ToString()] != null && Request.QueryString[Dr[0].ToString()]=="1")
                            {
                                string _Incident = Dr[0].ToString();

                                iTextSharp.text.Table Tbl_IncidentDetails = ER_GetIncidentReport(_Incident, ClsObj[i], _ExamDetails);

                                if (writerER.FitsPage(Tbl_IncidentDetails))
                                {
                                    DocER.Add(Tbl_IncidentDetails);
                                }
                                else
                                {
                                    DocER.NewPage();
                                    DocER.Add(Tbl_IncidentDetails);
                                }
                            }
                        }
                    }
                }

                if (MyUser.HaveModule(21) && _Attendance == 1)
                {
                    DateTime _StartDate;
                    DateTime _EndDate;
                    if (_ExamDetails.Length >= 2)
                    {
                         _StartDate = _ExamDetails[_ExamDetails.Length - 2].Date;
                         _EndDate = _ExamDetails[_ExamDetails.Length - 1].Date.AddDays(-1);

                    }
                    else
                    {
                        _StartDate = _EndDate = _ExamDetails[_ExamDetails.Length - 1].Date;
                    }
                    iTextSharp.text.Table Tbl_AttendenceReport = ER_GetAttendanceReport(ClsObj[i], _StartDate, _EndDate);
                    if (writerER.FitsPage(Tbl_AttendenceReport))
                    {
                        DocER.Add(Tbl_AttendenceReport);
                    }
                    else
                    {
                        DocER.NewPage();
                        DocER.Add(Tbl_AttendenceReport);
                    }
                }

                if (MyUser.HaveModule(1) && _FeeDue == 1)
                {
                    iTextSharp.text.Table Tbl_FeeDetails = ER_GetFeeReport(ClsObj[i].m_StudentId);
                    if (writerER.FitsPage(Tbl_FeeDetails))
                    {
                        DocER.Add(Tbl_FeeDetails);
                    }
                    else
                    {
                        DocER.NewPage();
                        DocER.Add(Tbl_FeeDetails);
                    }
                    
                }

                if (_Summary == 1)
                {
                    string ReturnDate = "";
                     if (Request.QueryString["ReturnDate"] != null )
                     {
                            ReturnDate = Request.QueryString["ReturnDate"];
                     }

                     iTextSharp.text.Table Tbl_SummaryReport = ER_SummaryReport(ClsObj[i], ReturnDate, _ExamDetails, _SubDetails, ClsObj[i].m_StudDetails, _SubjectWiseClassAvg, _ClassAvg);
                     if (writerER.FitsPage(Tbl_SummaryReport))
                     {
                         DocER.Add(Tbl_SummaryReport);
                     }
                     else
                     {
                         DocER.NewPage();
                         DocER.Add(Tbl_SummaryReport);
                     }

                     
                }

                iTextSharp.text.Table Tbl_Footer = ER_footer();
                DocER.Add(Tbl_Footer);

            }

            DocER.Close();
            return true;

        }
        
        private iTextSharp.text.Table ER_footer()
        {
            iTextSharp.text.Table _Tblfooter = new iTextSharp.text.Table(1);
            _Tblfooter.Width = 100;
            float[] headerwidths = { 100 };
            _Tblfooter.Widths = headerwidths;
            _Tblfooter.Padding = 1;
            _Tblfooter.Border = 0;
            _Tblfooter.AutoFillEmptyCells = true;
            _Tblfooter.DefaultCell.Border = 0;
            _Tblfooter.DefaultCell.HorizontalAlignment = 1;
            _Tblfooter.DefaultCell.VerticalAlignment = 1;
            _Tblfooter.AddCell(new Phrase("Computer generated report by WinEr.Powered by Winceron Software Technologies", MyFonts[6]));

            return _Tblfooter;
        }

        private iTextSharp.text.Table ER_SummaryReport(ComprehensiveExamClass comprehensiveExamClass, string ReturnDate, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, StudDetails studDetails, ExamNode[] _SubjectWiseClassAvg, double _ClassAvg)
         {



            double _Exam1  = 0,_Maxmark1=0;
            double _Exam2  = 0,_MaxMark2=0;
          
            int    Point1, Point2, Result;
            string _StudnetSex = GetStudentDetails(comprehensiveExamClass.m_StudDetails);
            string _Report = "",Overall ;
            string SubRate1 = "", SubRate2 = "", SubRate3 = "", SubRate4 = "", SubRate5 = "", SubRate6 = "", TSubRate1 = "", TSubRate2 = "", TSubRate3 = "", TSubRate4 = "", TSubRate5 = "", TSubRate6 = "";

            iTextSharp.text.Table _TblSummary = new iTextSharp.text.Table(1);
            _TblSummary.Width = 100;
            float[] headerwidths = { 100 };
            _TblSummary.Widths = headerwidths;
            _TblSummary.Padding = 1;
            _TblSummary.Border = 0;
            _TblSummary.AutoFillEmptyCells = true;
            _TblSummary.DefaultCell.Border = 0;
            _TblSummary.DefaultCell.HorizontalAlignment = 0;
            _TblSummary.DefaultCell.VerticalAlignment = 1;
            _TblSummary.AddCell(new Phrase("Summary", MyFonts[4]));
            if (MyUser.HaveModule(22))
            {
                DateTime _StartDate, _EndDate;

                if (_ExamDetails.Length >= 2)
                {
                    _StartDate = _ExamDetails[_ExamDetails.Length - 2].Date;
                    _EndDate = _ExamDetails[_ExamDetails.Length - 1].Date;

                }
                else
                {
                    _StartDate = _EndDate = _ExamDetails[_ExamDetails.Length - 1].Date;
                }
                int _no_presentdays=0;
                int _no_absentdays=0;
                int _no_holidays=0;
                int _no_halfdays=0;
                int _no_workingdays = 0;
                double _attendencepersent = 0;

                 

                bool attendance = MyAttendence.GetCurrentBatchNewattendanceDetailsWithDate(comprehensiveExamClass.m_StudentId, out  _no_workingdays, out  _no_presentdays, out _no_absentdays, out _no_holidays, out _no_halfdays, out _attendencepersent, MyUser.CurrentBatchId, _StartDate, _EndDate);
                if(_attendencepersent<65)

                    _Report = _Report + "  Dear Parent,  your ward is irregular  in the class";
                else
                    _Report = _Report + " Dear Parent,   your ward is regular in the class";

                _Report = _Report + " and ";
            }
            if(_Report!="")
                _Report = _Report + " " + _StudnetSex.ToLower() + " performance in the " + _ExamDetails[_ExamDetails.Length - 1].Name + "   was ";
            else
                _Report = _Report + " Dear Parent , your ward's performance in the " + _ExamDetails[_ExamDetails.Length - 1].Name + "  was ";
            if(_ExamDetails.Length>1)
            {
            _Maxmark1 = comprehensiveExamClass.m_MaxTotal[_ExamDetails.Length - 1];//Max Mark
            _Exam1 = comprehensiveExamClass.m_TotalMarks[_ExamDetails.Length - 1].MaxMark;// Total Mark
             _MaxMark2 = comprehensiveExamClass.m_MaxTotal[_ExamDetails.Length - 2];//Max Mark
            _Exam2 = comprehensiveExamClass.m_TotalMarks[_ExamDetails.Length - 2].MaxMark;// Total Mark
            }
            else
            {
                _Maxmark1 = comprehensiveExamClass.m_MaxTotal[_ExamDetails.Length - 1];//Max Mark
                _Exam1 = comprehensiveExamClass.m_TotalMarks[_ExamDetails.Length - 1].MaxMark;// Total Mark
            }
            
            Point1 = GetExamPerformaceWithPreviousExam(_Exam1, _Maxmark1, _Exam2, _MaxMark2, _ExamDetails.Length);
            Point2 = GetAverageInClass(_ClassAvg, _Exam1, _Maxmark1);
            Result = Point1 + Point2;
            string _Rate;
            GetRating(Result, out _Rate);
            _Report = _Report +_Rate+".";
            Overall = _Rate;
            for (int i = 0; i < _SubDetails.Length; i++)
            {
                if (_ExamDetails.Length >= 2)
                {
                    _Exam1 = comprehensiveExamClass.m_ExamMarks[_ExamDetails.Length - 1, i].MaxMark;
                    _Exam2 = comprehensiveExamClass.m_ExamMarks[_ExamDetails.Length - 2, i].MaxMark;
                    _MaxMark2= _Maxmark1 = _SubDetails[i].MaxMark;
                    
                }
                else
                {
                    _Exam1 = comprehensiveExamClass.m_ExamMarks[_ExamDetails.Length - 1, i].MaxMark;
                    _Maxmark1 = _SubDetails[i].MaxMark;
                }

                Point1 = GetExamPerformaceWithPreviousExam(_Exam1, _Maxmark1, _Exam2, _MaxMark2, _ExamDetails.Length);
                Point2 = GetAverageInClass(_ClassAvg, _Exam1, _Maxmark1);
                Result = Point1 + Point2;
                switch (Result)
                {
                    case 1:
                        if (SubRate1 != "")
                            SubRate1 = SubRate1 + " , " ;
                        SubRate1 = SubRate1 + TSubRate1;
                        TSubRate1 = _SubDetails[i].Name;
                        
                        break;
                    case 2:
                    case 3:
                        if (SubRate2 != "")
                            SubRate2 = SubRate2 + " , ";
                        SubRate2 = SubRate2 + TSubRate2;
                        TSubRate2 = _SubDetails[i].Name;
                       
                        break;
                    case 4:
                    case 5:
                    case 6:
                        if (SubRate3 != "")
                            SubRate3 = SubRate3 + " , ";
                        SubRate3 = SubRate3 + TSubRate3;
                        TSubRate3 = _SubDetails[i].Name;
                        
                        break;
                    case 7:
                    case 8:

                        if (SubRate4 != "")
                            SubRate4 = SubRate4 + " , ";
                        SubRate4 = SubRate4 + TSubRate4;
                        TSubRate4 = _SubDetails[i].Name;
                        
                        break;
                    case 9:
                    case 10:
                        if (SubRate5 != "")
                            SubRate5 = SubRate5 + " , ";
                        SubRate5 = SubRate5 + TSubRate5;
                        TSubRate5 = _SubDetails[i].Name;
                        
                        break;
                    case 11:
                    case 12:
                        if (SubRate6 != "")
                            SubRate6 = SubRate6 + " , ";
                        SubRate6 = SubRate6 + TSubRate6;
                        TSubRate6 = _SubDetails[i].Name;
                        
                        break;
                }
            
               
            }
            string report="";

            if (SubRate6 != "" || TSubRate6 != "")
            {
                _Report = _Report + _StudnetSex + " performance in ";
                if (SubRate6 != "")
                    _Report = _Report + SubRate6 + " and " + TSubRate6 + " are";
                else
                    _Report = _Report + TSubRate6 + " is ";

                GetRating(12, out report);
                _Report = _Report + report + ".";
            }

            if (SubRate5 != "" || TSubRate5 != "")
            {
                _Report = _Report + _StudnetSex + " performance in ";
                if (SubRate5 != "")
                    _Report = _Report + SubRate5 + " and " + TSubRate5 + " are";
                else
                    _Report = _Report + TSubRate5 + " is ";

                GetRating(9, out report);
                _Report = _Report + report + ".";
            }

            if (SubRate4 != "" || TSubRate4 != "")
            {
                _Report = _Report + _StudnetSex + " performance in ";
                if (SubRate4 != "")
                    _Report = _Report + SubRate4 + " and " + TSubRate4 + " are";
                else
                    _Report = _Report + TSubRate4 + " is ";

                GetRating(7, out report);
                _Report = _Report + report + ".";
            }

            if (SubRate3 != "" || TSubRate3 != "")
            {
                _Report = _Report + _StudnetSex + " performance in ";
                if (SubRate3 != "")
                    _Report = _Report + SubRate3 + " and " + TSubRate3 + " are";
                else
                    _Report = _Report + TSubRate3 + " is ";

                GetRating(4, out report);
                _Report = _Report + report + ".";
            }

            if (SubRate2 != "" || TSubRate2 != "")
            {
                _Report = _Report + _StudnetSex + " performance in ";
                if (SubRate2 != "")
                    _Report = _Report + SubRate2 + " and " + TSubRate2 + " are";
                else
                    _Report = _Report + TSubRate2 + " is ";

                GetRating(2, out report);
                _Report = _Report + report + ".";
            }

            if (SubRate1 != "" || TSubRate1 != "")
            {
                _Report = _Report + _StudnetSex + " performance in ";
                if (SubRate1 != "")
                    _Report =  _Report + SubRate1+" and " + TSubRate1 + " are" ;
                else
                    _Report =  _Report +  TSubRate1 + " is ";
                
                GetRating(1, out report);
                _Report = _Report +report+".";
            }


            _Report = _Report + " His/Her over all performance is " +Overall+". ";

            _Report = _Report + " Please go through the complete reports ";
            if (ReturnDate != "")
            {
                _Report = _Report + " and revert the signed on or before " + ReturnDate + ".";
            }

            _TblSummary.AddCell(new Phrase(_Report, MyFonts[5]));

            return _TblSummary;
        }

        private int GetAverageInClass(double ClassAvg, double SubjectMark, double MaxMark)
        {
            double avg1 = 0;

            avg1 = (SubjectMark / MaxMark) * 100;


            if (ClassAvg < avg1 && avg1 >= 85 || ClassAvg > avg1 && avg1 >=85)
            {
                return 6;
            }
            else if (ClassAvg < avg1 && avg1 >= 60 || ClassAvg > avg1 && avg1 >= 60)
            {
                return 5;
            }
            else if (ClassAvg < avg1 && avg1 > 50 || ClassAvg > avg1 && avg1 > 50)
            {
                return 4;
            }
            else if (ClassAvg < avg1 && avg1 > 45||ClassAvg > avg1 && avg1 > 50)
            {
                return 3;
            }
            else if (ClassAvg < avg1 && avg1 > 40 || ClassAvg > avg1 && avg1 > 40)
            {
                return 2;
            }
            else if (ClassAvg < avg1 && avg1 > 30 || ClassAvg > avg1 && avg1 > 30)
            {
                return 1;
            }
            else
            return 1;

        }

        private int GetExamPerformaceWithPreviousExam(double _Exam1, double _Maxmark1, double _Exam2, double _MaxMakr2, int No_of_Exam)
        {
            double avg1 = 0, avg2 = 0;
            if (No_of_Exam > 1)
            {
                avg1 = (_Exam1 / _Maxmark1) * 100;
                avg2 = (_Exam2 / _MaxMakr2) * 100;

                if (avg1 > avg2 && avg1 > 85)
                {
                    return 3;
                }
                else if(avg1 > avg2 && avg1 > 60)
                {
                    return 2;
                }
                else if(avg1 > avg2 && avg1 < 50)
                {
                    return 1;
                }
                else if (avg1 > avg2 && avg1 > 45)
                {
                    return 3;
                }
                else if (avg1 > avg2 && avg1 > 40)
                {
                    return 2;
                }
                else if (avg1 > avg2 && avg1 < 40)
                {
                    return 1;
                }
            }
            else
            {
                avg1 = (_Exam1 / _Maxmark1) * 100;
             
                if (avg1 > 85)
                {
                    return 6;
                }
                else if (avg1 > 60)
                {
                    return 5;
                }
                else if (avg1 < 50)
                {
                    return 4;
                }
                else if (avg1 > 45)
                {
                    return 3;
                }
                else if (avg1 > 40)
                {
                    return 2;
                }
                else if (avg1 < 40)
                {
                    return 1;
                }
            }
            return 1;

        }

        private void GetRating(int Result, out string _Rate)
        {
            _Rate = "";
            switch (Result)
            {
                case 1:
                    _Rate = " bad ";
                    break;
                case 2:
                case 3:
                    _Rate = " poor ";
                    break;
                case 4:
                case 5:
                case 6:
                    _Rate = " average ";
                    break;
                case 7:
                case 8:

                    _Rate = " good ";
                    break;
                case 9:
                case 10:
                    _Rate = " very good ";
                    break;
                case 11:
                case 12:
                    _Rate = " excelent ";
                    break;
            }
            
        }

        private string GetStudentDetails(StudDetails studDetails)
        {

            int _studentId = studDetails.Id;
            string _Sex="";
            string sql="select tblstudent.Sex from tblstudent where tblstudent.Id="+_studentId+" and tblstudent.StudentName='"+studDetails.Name+"'";
            MyReader=MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if(MyReader.HasRows)
            {
                if(MyReader.GetValue(0).ToString().ToLower()=="male")
                    _Sex="His";
                else
                    _Sex="Her";

            }
            return _Sex;
        }

        private iTextSharp.text.Table ER_GetAttendanceReport(ComprehensiveExamClass comprehensiveExamClass, DateTime _StartDate, DateTime _EndDate)
        {
            int  _no_workingdays=0;
            int _no_presentdays=0;
            int _no_absentdays=0;
            int _no_holidays=0;
            int _no_halfdays=0;
            double _attendencepersent = 0;


            MyAttendence.GetCurrentBatchNewattendanceDetailsWithDate(comprehensiveExamClass.m_StudentId, out  _no_workingdays, out  _no_presentdays, out _no_absentdays, out _no_holidays, out _no_halfdays, out _attendencepersent, MyUser.CurrentBatchId, _StartDate,_EndDate);
            iTextSharp.text.Table _TblAttendanceDetails = new iTextSharp.text.Table(1);
            _TblAttendanceDetails.Width = 100;
            float[] headerwidths = { 100 };
            _TblAttendanceDetails.Widths = headerwidths;
            _TblAttendanceDetails.Padding = 1;
            _TblAttendanceDetails.Border = 0;
            _TblAttendanceDetails.AutoFillEmptyCells = true;
            _TblAttendanceDetails.DefaultCell.Border = 0;
            _TblAttendanceDetails.DefaultCell.HorizontalAlignment = 0;
            _TblAttendanceDetails.DefaultCell.VerticalAlignment = 1;

            string _Report = "";
            if (_no_workingdays != 0 && _no_presentdays != 0 || _no_absentdays != 0 || _no_holidays != 0 || _no_halfdays != 0)
            {
                _Report = " Total Working days : " + _no_workingdays + ", Present Days : " + _no_presentdays + ", Absent Days : " + _no_absentdays + " , Half Days :" + _no_halfdays + " and  Attendance percentage : " + _attendencepersent.ToString("0.0") + ".";
            }
            else
            {
                _Report = "Attendance report doest not found";
            }
            _TblAttendanceDetails.AddCell(new Phrase("Attendance Report", MyFonts[4]));

            _TblAttendanceDetails.AddCell(new Phrase(_Report, MyFonts[5]));
            return _TblAttendanceDetails;
        }
       
        private iTextSharp.text.Table ER_GetFeeReport(int StudId)
        {
            iTextSharp.text.Table _TblIncidentDetails = new iTextSharp.text.Table(1);
            _TblIncidentDetails.Width = 100;
            float[] headerwidths = { 100 };
            _TblIncidentDetails.Widths = headerwidths;
            _TblIncidentDetails.Padding = 1;
            _TblIncidentDetails.Border = 0;
            _TblIncidentDetails.AutoFillEmptyCells = true;
            _TblIncidentDetails.DefaultCell.Border = 0;
            _TblIncidentDetails.DefaultCell.HorizontalAlignment = 0;
            _TblIncidentDetails.DefaultCell.VerticalAlignment = 1;
            string desc = "";
            string sql = "select  tblfeeaccount.AccountName, tblfeestudent.BalanceAmount,tblperiod.Period from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblstudent on tblstudent.Id = tblfeestudent.StudId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId where tblfeestudent.StudId=" + StudId + " and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion' and tblfeeschedule.DueDate <= CURRENT_DATE() and tblstudent.Status=1  ORDER BY tblperiod.Order";
            DataSet Dt = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Dt != null & Dt.Tables[0].Rows.Count > 0)
            {
                desc = "The student has to pay ";
                int Temp = 1;
                string _comma = "";
                int Count = Dt.Tables[0].Rows.Count;
                foreach (DataRow Dr in Dt.Tables[0].Rows)
                {

                    desc = desc + _comma + " " + Dr[0].ToString() + "-" + Dr[2].ToString() + "(" + Dr[1].ToString() + ")";

                    _comma = ",";
                    if (Temp == Count - 1 && Count != 1)
                    {
                        desc = desc + " and ";
                        _comma = "";
                    }


                    
                    Temp++;
                }
                desc = desc + ".";
            }
            _TblIncidentDetails.AddCell(new Phrase("Fee Due Report", MyFonts[4]));
             
            if (desc != "")
            {
                _TblIncidentDetails.AddCell(new Phrase(desc, MyFonts[5]));
            }
            else
            {
                _TblIncidentDetails.AddCell(new Phrase("No fee to pay ", MyFonts[5]));
            }
            return _TblIncidentDetails;
        
        }
             
        private iTextSharp.text.Table ER_GetIncidentReport(string _Incident, ComprehensiveExamClass comprehensiveExamClass,  ExamNode[] _ExamDetails)
       {
            iTextSharp.text.Table _TblIncidentDetails = new iTextSharp.text.Table(1);
            _TblIncidentDetails.Width = 100;
            float[] headerwidths = { 100 };
            _TblIncidentDetails.Widths = headerwidths;
           
            _TblIncidentDetails.Border = 0;
            _TblIncidentDetails.AutoFillEmptyCells = true;
            _TblIncidentDetails.DefaultCell.Border = 0;
            _TblIncidentDetails.DefaultCell.HorizontalAlignment = 0;
            _TblIncidentDetails.DefaultCell.VerticalAlignment = 1;

            int count = _ExamDetails.Length;

            string sql = "select tblincedenttype.`Type`, tblincedent.Title, tblincedent.Description, DATE_FORMAT( tblincedent.IncedentDate,\"%d-%m-%y\"), tblview_user.UserName from tblincedent inner join tblincedenttype on tblincedenttype.Id= tblincedent.TypeId inner join tblview_user on tblview_user.Id= tblincedent.CreatedUserId where tblincedenttype.Type='" + _Incident + "' and tblincedent.AssoUser=" + comprehensiveExamClass.m_StudDetails.Id + " and tblincedent.UserType='student' and tblincedent.Status='Approved' and    tblincedenttype.IncidentType='NORMAL'  and tblincedent.BatchId=" + MyUser.CurrentBatchId;
            if (count > 1)
            {
                sql = sql + " and tblincedent.IncedentDate between '" + _ExamDetails[(_ExamDetails.Length) - 2].Date.ToString("s") + "' and '" + _ExamDetails[(_ExamDetails.Length) - 1].Date.ToString("s") + "'";
            }
          
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _TblIncidentDetails.AddCell(new Phrase(_Incident, MyFonts[4]));
                while (MyReader.Read())
                {
                   
                    string _incident = MyReader.GetValue(1).ToString() + " : " + MyReader.GetValue(2).ToString() + ". Reported By " + MyReader.GetValue(4).ToString() + ". Incident Date : " + MyReader.GetValue(3).ToString();
                    _TblIncidentDetails.AddCell(new Phrase(_incident, MyFonts[3]));

                }
            }
            

            return _TblIncidentDetails;
        }

        private void GetPerformanceGraphReport(string[] _GraphName, int subId, ExamNode[] _SubDetails, string _PhysicalPath, int _TopStudList, int _GraphReport, DataSet StudentList, out iTextSharp.text.Table _Tbl_GraphDetails)
        {
            int i=subId;
             _Tbl_GraphDetails = new iTextSharp.text.Table(2);
           
            _Tbl_GraphDetails.Width = 100;
            float[] headerwidths = { 50, 50 };
            _Tbl_GraphDetails.Widths = headerwidths;
            _Tbl_GraphDetails.Padding = 1;
            _Tbl_GraphDetails.Border = 0;
            _Tbl_GraphDetails.AutoFillEmptyCells = true;
            _Tbl_GraphDetails.DefaultCell.Border = 0;
            iTextSharp.text.Image _ImgHeader = iTextSharp.text.Image.GetInstance(_GraphName[i]);
            _ImgHeader.ScaleAbsolute(230, 135);
          

            Cell cell0 = new Cell();
            Cell cell1 = new Cell();
            Cell cellHead1 = new Cell();
            Cell cellHead2 = new Cell();


            if (_GraphReport == 1 && _TopStudList == 1)
            {
                cell0.Add(_ImgHeader);
                cell0.Border = 0;
                iTextSharp.text.Table _Tbl_Toppers = new iTextSharp.text.Table(2);
                _Tbl_Toppers.Width = 100;
                float[] headerwidths1 = { 30, 70 };
                _Tbl_Toppers.Widths = headerwidths1;
            
                
                _Tbl_Toppers.DeleteAllRows();
                
                _Tbl_Toppers.AddCell(new Phrase("Marks", MyFonts[5]));
                _Tbl_Toppers.AddCell(new Phrase("Student Name", MyFonts[5]));

                DataTable dt = StudentList.Tables[_SubDetails[i].Id.ToString()];

                foreach (DataRow dataRow in dt.Rows)
                {
                    _Tbl_Toppers.AddCell(new Phrase(dataRow[0].ToString(), MyFonts[4]));
                    _Tbl_Toppers.AddCell(new Phrase(dataRow[1].ToString(), MyFonts[4]));
                }

                
                cell1.Add(_Tbl_Toppers);
                cellHead1.Add(new Phrase(_SubDetails[i].Name + " - Performance", MyFonts[4]));
                cellHead2.Add(new Phrase(_SubDetails[i].Name + " - Toppers", MyFonts[4]));

            }
            else if (_GraphReport == 0 && _TopStudList == 1)
            {
                iTextSharp.text.Table _Tbl_Toppers = new iTextSharp.text.Table(2);
                _Tbl_Toppers.Width = 100;
                float[] headerwidths1 = { 30, 70 };
                _Tbl_Toppers.Widths = headerwidths1;
                _Tbl_Toppers.Padding = 1;
                _Tbl_Toppers.DeleteAllRows();
                _Tbl_Toppers.AddCell(new Phrase("Marks", MyFonts[5]));
                _Tbl_Toppers.AddCell(new Phrase("Student Name", MyFonts[5]));

                DataTable dt = StudentList.Tables[_SubDetails[i].Id.ToString()];

                foreach (DataRow dataRow in dt.Rows)
                {
                    _Tbl_Toppers.AddCell(new Phrase(dataRow[0].ToString(), MyFonts[4]));
                    _Tbl_Toppers.AddCell(new Phrase(dataRow[1].ToString(), MyFonts[4]));
                }

                cell0.Add(_Tbl_Toppers);
                cell0.Border = 0;
                cellHead1.Add(new Phrase(_SubDetails[i].Name + " - Toppers", MyFonts[4]));
            }
            else if (_GraphReport == 1 && _TopStudList == 0)
            {
                cell0.Add(_ImgHeader);
                cell0.Border = 0;
                cellHead1.Add(new Phrase(_SubDetails[i].Name + " - Performance", MyFonts[4]));
            }

            _Tbl_GraphDetails.DefaultCell.Colspan = 1;

            _Tbl_GraphDetails.AddCell(cellHead1);
            _Tbl_GraphDetails.AddCell(cellHead2);

            _Tbl_GraphDetails.AddCell(cell0);
            _Tbl_GraphDetails.AddCell(cell1);


        }     
       
        private iTextSharp.text.Table ER_LoadCollegeDetailsToTable(string _PhysicalPath, SchoolDetails _SchoolDetails)
        {
            iTextSharp.text.Table _Tbl_CollageDetails = new iTextSharp.text.Table(3);
            _Tbl_CollageDetails.Width = 100;
            float[] headerwidths = { 20, 60, 20 };
            _Tbl_CollageDetails.Widths = headerwidths;
            _Tbl_CollageDetails.Padding = 1;
            _Tbl_CollageDetails.Border = 0;
            _Tbl_CollageDetails.AutoFillEmptyCells = true;
            _Tbl_CollageDetails.DefaultCell.Border = 0;
            string _HeaderImgName = _SchoolDetails.LogoURL;
            if (_HeaderImgName != "")
            {
                string _ImagePathAndName = _PhysicalPath + "ThumbnailImages\\" + _HeaderImgName;

                if (File.Exists(_ImagePathAndName))
                {
                    iTextSharp.text.Image _ImgHeader = iTextSharp.text.Image.GetInstance(_ImagePathAndName);
                    _ImgHeader.ScaleAbsolute(40, 40);
                    _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 0;
                    _Tbl_CollageDetails.DefaultCell.Colspan =2;
                   // _Tbl_CollageDetails.DefaultCell.Rowspan = 2;

                    Cell cell = new Cell();
                    cell.Rowspan = 2;
                    cell.Add(_ImgHeader);
                    cell.Border = 0;
                    //cell.BorderWidthBottom = 1;
                    _Tbl_CollageDetails.AddCell(cell);

                    _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                    _Tbl_CollageDetails.DefaultCell.VerticalAlignment = 1;
                    _Tbl_CollageDetails.AddCell(new Phrase(_SchoolDetails.SchoolName, MyFonts[1])); //Collage Name
                    _Tbl_CollageDetails.AddCell(new Phrase(_SchoolDetails.Address, MyFonts[2]));//Collage Address
                }
                else
                {
                    _Tbl_CollageDetails.DefaultCell.Colspan = 3;
                    _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                    //_Tbl_CollageDetails.DefaultCell.Rowspan = 1;
                    _Tbl_CollageDetails.AddCell(new Phrase(_SchoolDetails.SchoolName, MyFonts[1])); //Collage Name
                    _Tbl_CollageDetails.AddCell(new Phrase(_SchoolDetails.Address, MyFonts[2]));//Collage Address

                }


            }
            else
            {
                _Tbl_CollageDetails.DefaultCell.Colspan = 3;
                _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                // _Tbl_CollageDetails.DefaultCell.Rowspan = 1;
                _Tbl_CollageDetails.AddCell(new Phrase(_SchoolDetails.SchoolName, MyFonts[1])); //Collage Name
                _Tbl_CollageDetails.AddCell(new Phrase(_SchoolDetails.Address.ToString(), MyFonts[2]));//Collage Address

            }


            return _Tbl_CollageDetails;

        }

        private iTextSharp.text.Table ER_LoadStudentDetailsToTable(StudDetails studDetails, string _ExamName)
        {
            iTextSharp.text.Table Tbl_Details = new iTextSharp.text.Table(5);
            Tbl_Details.Width = 100;
            float[] headerwidths = { 20, 25, 20, 15, 20 };
            Tbl_Details.Widths = headerwidths;
            Tbl_Details.Padding = 1;
            Tbl_Details.Border = 0;
            Tbl_Details.DefaultCell.Border = 0;
            Tbl_Details.AutoFillEmptyCells = true;

            Tbl_Details.DefaultCell.BorderWidthTop = 1;
            Tbl_Details.DefaultCell.Colspan = 5;
            Tbl_Details.DefaultCell.HorizontalAlignment = 1;
            Tbl_Details.AddCell(new Phrase(_ExamName, MyFonts[5]));

            Tbl_Details.DefaultCell.BorderWidthTop = 0;
            Tbl_Details.DefaultCell.HorizontalAlignment = 0;
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("Student Name : ", MyFonts[3])); // Name
            Tbl_Details.DefaultCell.Colspan = 2;
            Tbl_Details.AddCell(new Phrase(studDetails.Name, MyFonts[7]));// bold
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("Roll No : ", MyFonts[3])); //Bil; date
            Tbl_Details.AddCell(new Phrase(studDetails.RollNum.ToString(), MyFonts[7]));

            // Second row
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("Class : ", MyFonts[3])); //Addmission No
            Tbl_Details.DefaultCell.Colspan = 2;
            Tbl_Details.AddCell(new Phrase(studDetails.Class, MyFonts[7]));
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("Date : ", MyFonts[3])); //Class name
            Tbl_Details.AddCell(new Phrase(General.GerFormatedDatVal(System.DateTime.Now), MyFonts[7]));

            Tbl_Details.DefaultCell.Colspan = 5;
            Tbl_Details.AddCell(new Phrase("\n", MyFonts[3]));//empty

            return Tbl_Details;
        }

        private iTextSharp.text.Table ER_MarkDetailsToTable(MarkNode[,] markNode, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, double[] Avg, MarkNode[] TotalMark, MarkNode[] Rank, string[] Grade, double[] MaxTotal)
        {

            iTextSharp.text.Table _Tbl_MarkDetails = new iTextSharp.text.Table(_ExamDetails.Length + 1);
            _Tbl_MarkDetails.Width = 100;
            //float[] headerwidths = { 40, 30, 30 };
            //_Tbl_MarkDetails.Widths = headerwidths;
            _Tbl_MarkDetails.Padding = 1;
            //_Tbl_MarkDetails.Border = 0;
            _Tbl_MarkDetails.AutoFillEmptyCells = true;
            _Tbl_MarkDetails.DefaultCell.Border = 0;

            _Tbl_MarkDetails.DefaultCell.HorizontalAlignment = 0;

            _Tbl_MarkDetails.DefaultCell.Colspan = _ExamDetails.Length + 1;
            _Tbl_MarkDetails.AddCell(new Phrase("COMPREHENSIVE PROGRESS REPORT", MyFonts[5]));

            _Tbl_MarkDetails.DefaultCell.Colspan = 1;
            _Tbl_MarkDetails.AddCell(new Phrase("SUBJECTS", MyFonts[5]));
            for (int i = 0; i < _ExamDetails.Length; i++)
            {
                _Tbl_MarkDetails.AddCell(new Phrase(_ExamDetails[i].Name, MyFonts[5]));

            }

            for (int Subjct = 0; Subjct < _SubDetails.Length; Subjct++)
            {
                _Tbl_MarkDetails.AddCell(new Phrase(_SubDetails[Subjct].Name, MyFonts[4]));
                for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                {
                 
                  

                        _Tbl_MarkDetails.AddCell(new Phrase(markNode[ExamCount, Subjct].MaxMark.ToString() + "/" + _SubDetails[Subjct].MaxMark.ToString("0"), MyFonts[4]));
                   
                }
            }
            _Tbl_MarkDetails.AddCell(new Phrase("TOTAL MARK", MyFonts[4]));
            for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
            {
                _Tbl_MarkDetails.AddCell(new Phrase(TotalMark[ExamCount].MaxMark.ToString() + "/" + MaxTotal[ExamCount].ToString("0"), MyFonts[4]));
            }

            _Tbl_MarkDetails.AddCell(new Phrase("PERCENTAGE", MyFonts[4]));

            for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
            {
                _Tbl_MarkDetails.AddCell(new Phrase(Avg[ExamCount].ToString("0.00"), MyFonts[4]));
            }
            _Tbl_MarkDetails.AddCell(new Phrase("RANK", MyFonts[4]));

            for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
            {
                _Tbl_MarkDetails.AddCell(new Phrase(Rank[ExamCount].MaxMark.ToString("0"), MyFonts[4]));
            }
            _Tbl_MarkDetails.AddCell(new Phrase("GRADE", MyFonts[4]));
            for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
            {
                _Tbl_MarkDetails.AddCell(new Phrase(Grade[ExamCount].ToString(), MyFonts[4]));
            }


            return _Tbl_MarkDetails;
        }

        private void CreateSubjectGraph(MarkNode[,] markNode, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, out string[] _GraphName, out int _GraphCount)
        {
            _GraphName = new string[_SubDetails.Length];
            _GraphCount = 0;

            float MaxVal = 100;
            float _val = 0;

            for (int a = 0; a < _SubDetails.Length; a++)
            {
                ChartControl chartcontrol_ExamChart = new ChartControl();
                //chartcontrol_ExamChart.ID = chartcontrol_ExamChart0.ID;
                chartcontrol_ExamChart.HasChartLegend = false;

                ColumnChart chart_Bar = new ColumnChart();

                Chart chart_Line = new SmoothLineChart();

                ChartPointCollection chart_Line_data = chart_Line.Data;

                for (int i = 0; i < _ExamDetails.Length; i++)
                {

                    double Mark = ((double.Parse(markNode[i, a].MaxMark.ToString("0.00")) / _SubDetails[a].MaxMark) * 100.0);
                    if (float.TryParse(Mark.ToString("0.00"), out _val))
                    {
                        //DataColumn dc = _ExamArray[_SelectedCondition, i].ExamName;
                        string ColumnName = "Exam " + (i + 1).ToString();// _ExamArray[_SelectedCondition, i].ExamName;
                        if (MaxVal < _val)
                        {
                            MaxVal = _val;
                        }
                        chart_Line_data.Add(new ChartPoint(ColumnName, _val));
                        chart_Bar.Data.Add(new ChartPoint(ColumnName, _val));
                    }
                }

                chart_Line.Line.Width = 2;
                chart_Line.Line.Color = System.Drawing.Color.RoyalBlue;
                chart_Line.Legend = "1";// Drp_SelectList.SelectedValue;
                chartcontrol_ExamChart.Charts.Clear();
                chartcontrol_ExamChart.Charts.Add(chart_Line);

                //chart_Line.DataLabels.Visible = true;

                chartcontrol_ExamChart.YTitle.StringFormat.FormatFlags = StringFormatFlags.DirectionVertical;
                chartcontrol_ExamChart.YTitle.StringFormat.Alignment = StringAlignment.Near;


                if (MaxVal != 100)
                {
                    MaxVal = MaxVal + 10;
                }
                chart_Bar.Shadow.Visible = true;
                chart_Bar.DataLabels.Visible = true;
                chart_Bar.MaxColumnWidth = 20;
                chartcontrol_ExamChart.YCustomEnd = MaxVal;
                chartcontrol_ExamChart.Charts.Add(chart_Bar);
                chart_Bar.Fill.Color = System.Drawing.Color.RoyalBlue;
                chartcontrol_ExamChart.Background.Color = System.Drawing.Color.White;
                chartcontrol_ExamChart.RedrawChart();
                _GraphCount++;
                _GraphName[a] = chartcontrol_ExamChart.FileName;
            }
        }

        private void CreateTopList(ExamNode[] _ExamDetails, ExamNode[] _SubDetails, out DataSet TopStudentList)
        {
            TopStudentList = new DataSet();
            int LastExamId = _ExamDetails[_ExamDetails.Length - 1].Id;
            string TopMarksColum = "select tblexammark.MarkColumn, tblexammark.SubjectId  from tblexammark where tblexammark.ExamSchId=" + LastExamId+ "  ORDER BY tblexammark.SubjectOrder";
            DataSet MarksColum = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(TopMarksColum);
            if (MarksColum != null && MarksColum.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in MarksColum.Tables[0].Rows)
                {

                    DataTable DataTbl = new DataTable(dr["SubjectId"].ToString());

                    DataColumn DataClmn;
                    DataRow DtRow;


                    DataClmn = new DataColumn();
                    DataClmn.DataType = System.Type.GetType("System.Double");
                    DataClmn.ColumnName = "Mark";
                    DataClmn.AutoIncrement = false;
                    DataClmn.Caption = "Mark";
                    DataClmn.ReadOnly = false;
                    DataClmn.Unique = false;
                    DataTbl.Columns.Add(DataClmn);

                    // Create second column.
                    DataClmn = new DataColumn();
                    DataClmn.DataType = System.Type.GetType("System.String");
                    DataClmn.ColumnName = "Name";
                    DataClmn.AutoIncrement = false;
                    DataClmn.Caption = "Name";
                    DataClmn.ReadOnly = false;
                    DataClmn.Unique = false;
                    DataTbl.Columns.Add(DataClmn);


                    DataSet _TopStudList = GetTopstudentList(LastExamId, dr[0].ToString());
                    if (_TopStudList != null && _TopStudList.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow StudList in _TopStudList.Tables[0].Rows)
                        {
                            DtRow = DataTbl.NewRow();
                            double _mark = 0;
                            double.TryParse(StudList[1].ToString(), out _mark);
                            DtRow["Mark"] = _mark.ToString();
                            DtRow["Name"] = StudList["StudentName"].ToString();
                            DataTbl.Rows.Add(DtRow);

                        }
                    }

                    TopStudentList.Tables.Add(DataTbl);
                }
            }
        }

        private DataSet GetTopstudentList(int _examId, string MarkCol)
        {
            DataSet _TopList = null;
            string sqlMark = "select tblstudent.StudentName ," + MarkCol + " from tblstudent INNER join tblstudentmark on tblstudentmark.StudId= tblstudent.Id where tblstudentmark.ExamSchId= " + _examId + " Order By " + MarkCol + " DESC LIMIT 3 ";

            _TopList = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sqlMark);

            return _TopList;
        }

        private void GetSchoolDetails(out SchoolDetails _SchoolDetails)
        {
            _SchoolDetails.SchoolName   = "";
            _SchoolDetails.Address      = "";
            _SchoolDetails.LogoURL      = "";

            string sql = "select tblschooldetails.SchoolName, tblschooldetails.Address, tblschooldetails.LogoUrl from tblschooldetails";
            OdbcDataReader MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                _SchoolDetails.SchoolName   = MyReader.GetValue(0).ToString();
                _SchoolDetails.Address      = MyReader.GetValue(1).ToString();
                _SchoolDetails.LogoURL      = MyReader.GetValue(2).ToString();
            }

        }




        private void GetSubDetails(int ClassId, int ExamId, out ExamNode[] _SubDetails)
        {
            string sql  = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=tblexamschedule.ClassExamId and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId  where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblclassexam.ClassId=" + ClassId + " and tblexammaster.Id=" + ExamId+"  order by tblexammark.SubjectOrder ";
            MyReader    = MyClassMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                int i       = 0;
                int Count   = MyReader.RecordsAffected;
                _SubDetails = new ExamNode[Count];

                while (MyReader.Read())
                {
                    _SubDetails[i].Id       = int.Parse(MyReader.GetValue(0).ToString());
                    _SubDetails[i].Name     = MyReader.GetValue(1).ToString();
                    _SubDetails[i].MaxMark  = double.Parse(MyReader.GetValue(2).ToString());
                    i++;
                }
            }
            else
            {
                _SubDetails = new ExamNode[0];
            }

        }
        private DataSet GetStudDetails(int StudentId, int ClassId, int ExamId)
        {
            string sql_Student = "select tblview_student.Id, tblview_student.StudentName, tblview_student.RollNo, tblclass.ClassName from tblview_student inner join tblclass on tblclass.Id= tblview_student.ClassId inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblview_student.Id  where tblview_student.ClassId=" + ClassId + " and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblview_student.Id IN( select tblstudentmark.StudId from tblstudentmark inner join tblexamschedule on tblexamschedule.Id = tblstudentmark.ExamSchId inner join  tblclassexam on tblclassexam.Id=  tblexamschedule.ClassExamId where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblclassexam.ExamId= " + ExamId + " and tblexamschedule.Status='Completed'  )";

            if (StudentId != 0)
                sql_Student = sql_Student + " and tblview_student.Id=" + StudentId;

            sql_Student = sql_Student + "  order by tblview_student.RollNo";

            DataSet _studList = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql_Student);

            return _studList;

        }
        private void GetExamDetails(int ClassId, int ExamId, out ExamNode[] _ExamDetails)
        {

        //    string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period,tblexamschedule.ScheduledDate from tblexammaster inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id     where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblexammaster.Id=" + ExamId + " and tblexamschedule.Status='Completed'  order by tblexamschedule.id ";
            string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period, max( tblexammark.ExamDate)  from tblexammaster inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id  inner join tblexammark on tblexammark.ExamSchId =tblexamschedule.id     where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblexammaster.Id=" + ExamId + " and tblexamschedule.Status='Completed'  order by tblexamschedule.id ";
            MyReader    = MyClassMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                int i       = 0;
                int Count   = MyReader.RecordsAffected;
                _ExamDetails = new ExamNode[Count];

                while (MyReader.Read())
                {
                    _ExamDetails[i].Id      = int.Parse(MyReader.GetValue(0).ToString());
                    _ExamDetails[i].Name    = MyReader.GetValue(2).ToString();
                    _ExamDetails[i].Date = DateTime.Parse(MyReader.GetValue(3).ToString());
                    //string sql1 = "select Max( tblexammark.ExamDate) from tblexammark where tblexammark.ExamSchId=" + int.Parse(MyReader.GetValue(0).ToString());
                    //OdbcDataReader dr = MyClassMang.m_MysqlDb.ExecuteQuery(sql1);
                    //if (dr.HasRows)
                    //{

                    //    _ExamDetails[i].Date = DateTime.Parse(dr.GetValue(0).ToString());
                    //}
                 
                    i++;
                }
            }
            else
            {
                _ExamDetails = new ExamNode[0];
            }
        }

        private void LoadMyFont()
        {
            MyFonts[0] = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD);//Title(FR
            MyFonts[1] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 15, iTextSharp.text.Font.BOLD);// CollageName  (RF
            MyFonts[2] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.NORMAL);// Collage ADDRESS (RF
            MyFonts[3] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL);// Bill and Student etails              
            MyFonts[4] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD);// Bill and Student etails    
            MyFonts[5] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL);// for the Message (QTN ,DC  ,OA ,PI,TAX INVOICE       
            MyFonts[6] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);// used for Winceron adv
            MyFonts[7] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD);// Bill and Student etails              
           
        }

    }
}

