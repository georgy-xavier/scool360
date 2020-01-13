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

namespace WinEr
{

    public struct StudentDetails
    {
        public int    Id;
        public string Name;
        public int    RollNum;
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



    public class ExamGridReportClass
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
     





        public ExamNode[] m_CT_Eaxams;
        public ExamNode[] m_CT_Subjects;
        public MarkNode[,] m_CT_ExamMarks;

        public MarkNode[] m_CT_TotalMarks;
        public MarkNode[] m_CT_Ranks;

        public string[] m_CT_Grade;
        public double[] m_CT_Average;
        public double[] m_CT_MaxTotal;



        public ExamNode[] m_T_Eaxams;
        public ExamNode[] m_T_Subjects;
        public MarkNode[,] m_T_ExamMarks;

        public MarkNode[] m_T_TotalMarks;
        public MarkNode[] m_T_Ranks;
        public StudentDetails m_T_StudDetails;

        public string[] m_T_Grade;
        public double[] m_T_Average;
        public double[] m_T_MaxTotal;




        public ExamGridReportClass(MysqlClass _Mysqlobj, int _StudentId)
        {
            m_MysqlDb = _Mysqlobj;
            m_StudentId = _StudentId;

        }

        public bool ExamDetails(ExamNode[] _Eaxams, ExamNode[] m_Subjects, DataRow _StudDetails,string _ExamType,int StudentId)
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
                m_StudDetails.Strength =double.Parse( _StudDetails[7].ToString());
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

            else if (_ExamType == "CT")
            {
                m_CT_ExamMarks = new MarkNode[_Eaxams.Length, m_Subjects.Length];

                m_CT_TotalMarks = new MarkNode[_Eaxams.Length];
                m_CT_Ranks = new MarkNode[_Eaxams.Length];
                m_CT_Grade = new string[_Eaxams.Length];
                m_CT_Average = new double[_Eaxams.Length];
                m_CT_MaxTotal = new double[_Eaxams.Length];

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

                    sqlMark = sqlMark + "tblstudentmark.TotalMark, tblstudentmark.TotalMax, tblstudentmark.Avg, tblstudentmark.Rank, tblstudentmark.Grade, tblstudentmark.Remark  from  tblstudentmark where  tblstudentmark.ExamSchId=" + _Eaxams[i].Id + " and  tblstudentmark.StudId=" + m_StudentId;
                }
                if( _ExamType == "Main")
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
                else if (_ExamType == "CT")
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
                                        m_CT_ExamMarks[i, j].Id = _Eaxams[i].Id;
                                        m_CT_ExamMarks[i, j].Name = _Eaxams[i].Name;

                                        string _DbVal = dr["MarkColumn"].ToString();
                                        string Mark = MarkSet.Tables[0].Rows[0][_DbVal].ToString();

                                        m_CT_ExamMarks[i, j].MaxMark = double.Parse(Mark);

                                        break;
                                    }
                                }
                            }

                            m_CT_TotalMarks[i].MaxMark = double.Parse(MarkSet.Tables[0].Rows[0]["TotalMark"].ToString());
                            m_CT_Ranks[i].MaxMark = Math.Round(double.Parse(MarkSet.Tables[0].Rows[0]["Rank"].ToString()));
                            m_CT_Grade[i] = MarkSet.Tables[0].Rows[0]["Grade"].ToString();
                            m_CT_Average[i] = double.Parse(MarkSet.Tables[0].Rows[0]["Avg"].ToString());
                            m_CT_MaxTotal[i] = double.Parse(MarkSet.Tables[0].Rows[0]["TotalMax"].ToString());
                        }
                        else
                        {
                            m_CT_TotalMarks[i].MaxMark = 0;
                            m_CT_Ranks[i].MaxMark = 0;
                            m_CT_Grade[i] = "";
                            m_CT_Average[i] = 0;
                            m_CT_MaxTotal[i] = 0;
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


    public partial class DisplayMarkGradeReport : System.Web.UI.Page
    {
        private ClassOrganiser MyClassMang;
        private ExamManage MyExamMgnr;
        private Attendance MyAttendence;
        private Incident MyIncidentMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private MysqlClass _Mysqlobj;
        private SchoolClass objSchool = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }

            MyUser = (KnowinUser)Session["UserObj"];
            MyClassMang = MyUser.GetClassObj();
            MyExamMgnr = MyUser.GetExamObj();
            MyIncidentMang = MyUser.GetIncedentObj();
            MyAttendence = MyUser.GetAttendancetObj();
            //_Mysqlobj = new MysqlClass(MyUser.ConnectionString);
            if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(848))
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
                    if (objSchool != null)
                        _Mysqlobj = new MysqlClass(objSchool.ConnectionString);
                }
                else
                {
                    _Mysqlobj = new MysqlClass(WinerUtlity.SingleSchoolConnectionString);
                }
                if (!IsPostBack)
                {
                    if (Request.QueryString["ClassId"] != null && Request.QueryString["ExamId"] != null && Request.QueryString["studentId"] != null && Request.QueryString["ClassId"] != "0" && Request.QueryString["ExamId"] != "0" && Request.QueryString["cocurriculatexam"] != null && Request.QueryString["charecterTraitsExamId"] != null)
                    {
                        /*ClassId* ExamId* studentId* MainReport* GraphReport* TopStudList* Disciplinary* MedicalReport* Academic* Attendance* FeeDue* Summary*/

                        LoadReport(int.Parse(Request.QueryString["ClassId"].ToString()), int.Parse(Request.QueryString["ExamId"].ToString()), int.Parse(Request.QueryString["studentId"].ToString()), int.Parse(Request.QueryString["cocurriculatexam"].ToString()), int.Parse(Request.QueryString["charecterTraitsExamId"].ToString()));
                    }

                }
            }
        }
        private void LoadReport(int ClassId, int ExamId, int StudentId,int CCID,int CTID)
        {            

            
            double _ClassPersentage;
            double CT_ClassPersentage;
            double CC_ClassPersentage;
            int _CCID=0; int _CTID=0;

            DataSet StudList = new DataSet();
            ExamNode[] _ExamDetails;
            ExamNode[] _SubDetails;
            ExamNode[] _SubDetailsPassMark;
            ExamNode[] _SubjectWiseClassPersetage;
            ExamNode[] _ClassHeighest;
            ExamNode[,] _ClassMarks;



            ExamNode[] CC_ExamDetails = null;
            ExamNode[] CC_SubDetails = null; 
            ExamNode[] CT_ExamDetails=null;
            ExamNode[] CT_SubDetails = null;
            ExamNode[] CC_SubjectWiseClassPersetage=null;
            ExamNode[] CT_SubjectWiseClassPersetage=null;
           
            ExamNode[] Total_ExamDetails=null;
            

            DataSet TopStudentList = new DataSet();
            List<ExamGridReportClass> ClsObj = new List<ExamGridReportClass>();
            ExamGridReportClass _ReportObj;
           
            try
            {

            StudList = GetStudDetails(0, ClassId, ExamId);

            _CCID = CCID;
            _CTID = CTID;
            
            GetExamDetails(ClassId, ExamId, out _ExamDetails);
            GetSubDetails(ClassId, ExamId, out _SubDetails);
           
            GetSubDetailsWithPassmark(ClassId, ExamId, out _SubDetailsPassMark);
                //dominic
            if ( _CCID > 0)
            {
                GetExamDetails(ClassId, _CCID, out CC_ExamDetails);
                GetSubDetails(ClassId, _CCID, out CC_SubDetails);
               
                GetExamWiseAverage(CC_ExamDetails, CC_SubDetails, StudList.Tables[0].Rows.Count, out CC_SubjectWiseClassPersetage, out CC_ClassPersentage);
                
            }

            if ( _CTID > 0 )
            {
                GetExamDetails(ClassId, _CTID, out CT_ExamDetails);
                GetSubDetails(ClassId, _CTID, out CT_SubDetails);
                
                GetExamWiseAverage(CT_ExamDetails, CT_SubDetails, StudList.Tables[0].Rows.Count, out CT_SubjectWiseClassPersetage, out CT_ClassPersentage);
                
            }

            GetTotalExamDetails(ClassId, ExamId, out Total_ExamDetails);


            GetExamWiseAverage(_ExamDetails, _SubDetails, StudList.Tables[0].Rows.Count, out _SubjectWiseClassPersetage, out _ClassPersentage);
            GetClassHighestandPositon(_ExamDetails, _SubDetails, out _ClassHeighest);
            
            _ClassMarks = new ExamNode[StudList.Tables[0].Rows.Count, _SubDetails.Length];
           
            foreach (DataRow Dr in StudList.Tables[0].Rows)
            {
                
                _ReportObj = new ExamGridReportClass(_Mysqlobj, int.Parse(Dr[0].ToString()));
                _ReportObj.ExamDetails(_ExamDetails, _SubDetails, Dr,"Main",0);
                if (_CCID > 0 )
                {
                    _ReportObj.ExamDetails(CC_ExamDetails, CC_SubDetails, Dr, "CC", 0);
                }
                if (_CTID > 0 )
                {
                    _ReportObj.ExamDetails(CT_ExamDetails, CT_SubDetails, Dr, "CT", 0);
                }
                if(Total_ExamDetails!=null)
                 _ReportObj.ExamDetails(Total_ExamDetails, _SubDetails, Dr, "Total", 0);
                

                ClsObj.Add(_ReportObj);
            }

            CreateReport(ClsObj, _ExamDetails, _SubDetails, _ClassHeighest, _SubjectWiseClassPersetage, _ClassPersentage, CC_ExamDetails, CC_SubDetails, CC_SubjectWiseClassPersetage, CT_ExamDetails, CT_SubDetails, CT_SubjectWiseClassPersetage, Total_ExamDetails, _SubDetailsPassMark, StudentId,_CTID,_CCID);

            }

            catch (Exception _error)
            {

                Response.Write("<script>alert(\"Please make sure that all exams reports are generated correctly in exam dash board. (Co-Curricular and Character Traits also)\");</script>");
                Response.Write("<script>window.close()</script>");
            }
           
        }

        private void CreateReport(List<ExamGridReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, ExamNode[] _ClassHeighest, ExamNode[] _SubjectWiseClassPersetage, double _ClassPersentage, ExamNode[] CC_ExamDetails, ExamNode[] CC_SubDetails, ExamNode[] CC_SubjectWiseClassPersetage, ExamNode[] CT_ExamDetails, ExamNode[] CT_SubDetails, ExamNode[] CT_SubjectWiseClassPersetage, ExamNode[] Total_ExamDetails, ExamNode[] _SubDetailsPassMark, int StudentId,int _CTID,int _CCID)
        {
       
              string _PhysicalPath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath(""));
                Document document = LoadPDFPage(ClsObj, _ExamDetails, _SubDetails, _PhysicalPath, _ClassHeighest, _SubjectWiseClassPersetage, _ClassPersentage, CC_ExamDetails, CC_SubDetails, CC_SubjectWiseClassPersetage, CT_ExamDetails, CT_SubDetails, CT_SubjectWiseClassPersetage, Total_ExamDetails, _SubDetailsPassMark, StudentId, _CTID, _CCID);
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

            if(StudentId==0)
                MainName =  ClsObj[0].m_StudDetails.Class ;
            else
                MainName = "Stud_Id_"+StudentId.ToString();

            filename = _PhysicalPath + "\\PDF_Files\\Grade_Class_" + MainName + ".pdf";

                pdfRenderer.PdfDocument.Save(filename);
                // ...and start a viewer.

                //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=Grade_Class_" + MainName + ".pdf\");", true);
                Response.Redirect("OpenPdfPage.aspx?PdfName=Grade_Class_" + MainName + ".pdf", false);

              

        }

        private Document LoadPDFPage(List<ExamGridReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, string _PhysicalPath, ExamNode[] _ClassHeighest, ExamNode[] _SubjectWiseClassPersetage, double _ClassPersentage, ExamNode[] CC_ExamDetails, ExamNode[] CC_SubDetails, ExamNode[] CC_SubjectWiseClassPersetage, ExamNode[] CT_ExamDetails, ExamNode[] CT_SubDetails, ExamNode[] CT_SubjectWiseClassPersetage, ExamNode[] Total_ExamDetails, ExamNode[] _SubDetailsPassMark, int StudentId ,int _CTID,int _CCID)
        {
            
            Document document = new Document();
          
            int _Header = 0;
            int _MainReport = 0;
            int _Performance = 0;
            int  _CTCC = 0;
            int _TeacherRemark = 0;
            int _AverageReport = 0;
            int _Summary = 0;

            string _ExamName = _ExamDetails[_ExamDetails.Length - 1].Name;


            if (Request.QueryString["Header"] != null)
            {
                int.TryParse(Request.QueryString["Header"].ToString(), out _Header);
            }
            if (Request.QueryString["Main Report"] != null)
            {
                int.TryParse(Request.QueryString["Main Report"].ToString(), out _MainReport);
            }

            if (Request.QueryString["Performance"] != null)
            {
                int.TryParse(Request.QueryString["Performance"].ToString(), out _Performance);
            }

            if (Request.QueryString["Teacher's Remark"] != null)
            {
                int.TryParse(Request.QueryString["Teacher's Remark"].ToString(), out _TeacherRemark);
            }
            if (Request.QueryString["Co-Curricular Activities and Character Traits"] != null)
            {
                int.TryParse(Request.QueryString["Co-Curricular Activities and Character Traits"].ToString(), out _CTCC);

            }
            if (Request.QueryString["Average"] != null)
            {
                int.TryParse(Request.QueryString["Average"].ToString(), out _AverageReport);

            }
            
            ExamNode[] _PositionDetails = new ExamNode[ _SubDetails.Length];

            
            


            for (int i = 0; i < ClsObj.Count; i++)
            {

                if (StudentId != 0 && ClsObj[i].m_StudentId == StudentId || StudentId == 0)
                {
                    PageSetup PgSt = document.DefaultPageSetup;

                    PgSt.LeftMargin = 75;
                    PgSt.RightMargin = 15;
                    PgSt.TopMargin = 0;
                    PgSt.BottomMargin = 5;

                    PgSt.HeaderDistance = 0;
                    PgSt.FooterDistance = 0;
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

                    if(_Header!=0)
                        Top.AddImage("head.JPG");
                    //document.LastSection.AddParagraph("St. FRANCIS");
                    else
                        Top.AddFormattedText("\n\n\n\n\n\n\n\n\n\n\n\n");


                    Paragraph paragraph = section.AddParagraph();


                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                    paragraph.Format.Font = font;
                    paragraph.Format.Font.Bold = true;
                    paragraph.AddFormattedText(_ExamName);
                    

                    Table table = new Table();

                    table.Format.Font = fontMain;

                    Column column = table.AddColumn(Unit.FromCentimeter(3));


                    column = table.AddColumn(Unit.FromCentimeter(5));
                    column.Format.Font.Bold = true;
                    column = table.AddColumn(Unit.FromCentimeter(1));

                    column = table.AddColumn(Unit.FromCentimeter(4));
                    column.Format.Font.Bold = false;
                    column = table.AddColumn(Unit.FromCentimeter(5));
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

                    Table _BaseTable = GetTableFormant();
                    Table TableMain = new Table();
                     

                     if (_MainReport > 0)
                    {
                         TableMain = GetMainReportDetails(ClsObj[i].m_ExamMarks, _ExamDetails, _SubDetails, ClsObj[i].m_Average, ClsObj[i].m_TotalMarks, ClsObj[i].m_Ranks, ClsObj[i].m_Grade, ClsObj[i].m_MaxTotal, _SubDetailsPassMark, _ClassHeighest, ClsObj, ClsObj[i].m_StudentId, _BaseTable);
                        if (_CTCC == 0 ||( _CCID==0 && _CTID==0))
                        {
                            document.LastSection.Add(TableMain);
                        }
                        else if (_CTCC == 1 ||( _CCID<=0 && _CTID<=0))
                        {
                            document.LastSection.Add(TableMain);
                        }

                    }

                     if (_CTCC > 0 && (_CCID > 0 || _CTID > 0))
                     {
                         Table TableCC;
                        if (_MainReport == 0)
                        {
                             TableCC = GetExtraDEtails(ClsObj[i].m_CC_ExamMarks, CC_ExamDetails, CC_SubDetails, ClsObj[i].m_CT_ExamMarks, CT_ExamDetails, CT_SubDetails, _BaseTable,_CCID, _CTID );

                        }
                        else
                        {
                            TableCC = GetExtraDEtails(ClsObj[i].m_CC_ExamMarks, CC_ExamDetails, CC_SubDetails, ClsObj[i].m_CT_ExamMarks, CT_ExamDetails, CT_SubDetails, TableMain,_CCID, _CTID);
                        }
                      //  Table TempTbl = TableCC.Clone();
                        document.LastSection.Add(TableCC);
                    }
                    if (_Performance != 0)
                    {
                        Table TotalExam = GetTotalExam(ClsObj[i].m_T_ExamMarks, Total_ExamDetails, _SubDetails, _AverageReport);
                        document.LastSection.Add(TotalExam);
                    }
                    Paragraph bottom = section.AddParagraph();
                    bottom.Format.Font = fontMain3;
                    if (_TeacherRemark != 0)
                    {
                        bottom.Format.Alignment = ParagraphAlignment.Left;
                        bottom.AddFormattedText("Teacher's Remark");
                        bottom.AddFormattedText("\n\n");
                        bottom.AddFormattedText(ClsObj[i].m_Remark[_ExamDetails.Length - 1].ToString());
                        bottom.AddFormattedText("\n\n");

                    }
                    else
                    {
                        bottom.AddFormattedText("\n\n\n\n\n\n\n\n");
                    }

                    bottom.AddFormattedText("Signature: Class Teacher\t\t\t\tPrincipal\t\t\t\tParent");
                }
           
            }
               
            
            return document;

        }


        private Table GetTotalExam(MarkNode[,] markNode, ExamNode[] Total_ExamDetails, ExamNode[] _SubDetails, int _AverageReport)
        {
            MigraDoc.DocumentObjectModel.Font fontMain2 = new MigraDoc.DocumentObjectModel.Font("Times New Roman", 10);
            Table table3 = new Table();
            table3.Format.Font = fontMain2;
            double[] avgMarks, MaxMark;
            int[] TotalExam;
      
            table3.Borders.Width = 0.5;
            Column column3;

            double totalColumnsize = 14.5 / _SubDetails.Length;

            column3 = table3.AddColumn(Unit.FromCentimeter(1));
            column3 = table3.AddColumn(Unit.FromCentimeter(2.7));

            for (int c = 0; c < _SubDetails.Length; c++)
            {
                column3 = table3.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(totalColumnsize));
            }


            //column3 = table3.AddColumn(Unit.FromCentimeter(1));
            //column3 = table3.AddColumn(Unit.FromCentimeter(2.7));
            //column3 = table3.AddColumn(Unit.FromCentimeter(1.3));
            //column3 = table3.AddColumn(Unit.FromCentimeter(1.3));
            //column3 = table3.AddColumn(Unit.FromCentimeter(1.3));
            //column3 = table3.AddColumn(Unit.FromCentimeter(1.3));
            //column3 = table3.AddColumn(Unit.FromCentimeter(1.3));
            //column3 = table3.AddColumn(Unit.FromCentimeter(1.3));
            //column3 = table3.AddColumn(Unit.FromCentimeter(1.3));
            //column3 = table3.AddColumn(Unit.FromCentimeter(1.3));
            //column3 = table3.AddColumn(Unit.FromCentimeter(1.3));
            //column3 = table3.AddColumn(Unit.FromCentimeter(1.3));
            //column3 = table3.AddColumn(Unit.FromCentimeter(1.3));



            Row row1 = table3.AddRow();



            Cell cell1 = row1.Cells[0];


            cell1.MergeRight = _SubDetails.Length+1;
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
                                    avgMarks[j] =( avgMarks[j] / (Total_ExamDetails.Length*100))*100;
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

                    if(Total_ExamDetails[i].Name.Length>11)
                        cell1.AddParagraph(Total_ExamDetails[i].Name.Substring(0,12) + "\n");
                    else
                        cell1.AddParagraph(Total_ExamDetails[i].Name+ "\n");


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

                            double max_Mark = MyExamMgnr.GetMaxMarkFromExamScheduleIdandSubjectId(Total_ExamDetails[i].Id, _SubDetails[j].Id);

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

    
        private Table GetExtraDEtails(MarkNode[,] CC_markNode, ExamNode[] CC_ExamDetails, ExamNode[] CC_SubDetails, MarkNode[,] CT_markNode, ExamNode[] CT_ExamDetails, ExamNode[] CT_SubDetails, Table TableMain_Original ,int _CCID,int  _CTID)
        {
       
           //dominic calculatie grade
            Table TableMain = GetTableFormant();
            Row row = TableMain.AddRow();
            row.Height = 30;
            Cell cell = row.Cells[0];
            cell.MergeRight = 7;
         
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.Format.Alignment = ParagraphAlignment.Center;
            cell.Format.Font.Bold = true;
            string HeadName = "";
            if(_CCID>0)
                HeadName="CO - CURRICULAR ACTIVITIES";
            if (_CTID > 0 && _CCID > 0)
                HeadName = HeadName + " AND CHARACTER TRAITS ";
            else if (_CTID > 0 )
                HeadName = HeadName + "CHARACTER TRAITS ";
            cell.AddParagraph(HeadName);

            row = TableMain.AddRow();
       
            MigraDoc.DocumentObjectModel.Font font1 = new MigraDoc.DocumentObjectModel.Font("Times New Roman", 11);
            MigraDoc.DocumentObjectModel.Font font2 = new MigraDoc.DocumentObjectModel.Font("Times New Roman", 11);

            row.Height = 70;


     
            Table _Extradetails = new Table();
            
            Column columncc;
          
            _Extradetails.Borders.Width = 0;

            columncc = _Extradetails.AddColumn(Unit.FromCentimeter(6));
            columncc = _Extradetails.AddColumn(Unit.FromCentimeter(1));
            Row row1 = _Extradetails.AddRow();
            row1.Format.Font = font1;
            Cell cell1;
          
           

            if (_CCID != 0)
            {
                DataSet Grade = MyExamMgnr.GetGradeDataSet(CC_ExamDetails[CC_ExamDetails.Length - 1].GradeMasterId);
                for (int i = 0; i < CC_SubDetails.Length; i++)
                {

                    cell1 = row1.Cells[0];
                    cell1.AddParagraph(CC_SubDetails[i].Name);
                    cell1 = row1.Cells[1];
                    cell1.AddParagraph(": " + GetGradeFromMarks(Grade,CC_markNode[CC_ExamDetails.Length - 1, i].MaxMark, CC_SubDetails[i].MaxMark));
                    //cell.AddParagraph(CC_SubDetails[i].Name + "-" + GetGradeFromManrks(CC_markNode[CC_ExamDetails.Length - 1, i].MaxMark, CC_SubDetails[i].MaxMark) + "\n");
                }
                
                cell = row.Cells[0];
                if (_CTID > 0)
                {
                    cell.MergeRight = 2;
                }
                else
                {
                    cell.MergeRight = 5;
                }

                cell.Elements.Add(_Extradetails);
            }
            else
            {
                cell = row.Cells[0];
                if (_CTID > 0)
                {
                    cell.MergeRight = 2;
                }
                else
                {
                    cell.MergeRight = 5;
                }

                cell.AddParagraph("");
            }

          // cell.Document.LastSection.Add(_Extradetails);
            if (_CTID > 0)
            {

                Table _ExtradetailsCT = new Table();

                DataSet Grade = MyExamMgnr.GetGradeDataSet(CT_ExamDetails[CT_ExamDetails.Length - 1].GradeMasterId);
               
                _ExtradetailsCT.Borders.Width = 0;

                _ExtradetailsCT.Borders.Width = 0;

                Column columnct = _ExtradetailsCT.AddColumn(Unit.FromCentimeter(4));
                columnct = _ExtradetailsCT.AddColumn(Unit.FromCentimeter(1));
                Row rowct = _ExtradetailsCT.AddRow();
                rowct.Format.Font = font2;
                Cell cellct;
                for (int i = 0; i < CT_SubDetails.Length; i++)
                {
                    cellct = rowct.Cells[0];
                    cellct.AddParagraph(CT_SubDetails[i].Name);
                    cellct = rowct.Cells[1];
                    cellct.AddParagraph(": " + GetGradeFromMarks(Grade, CT_markNode[CT_ExamDetails.Length - 1, i].MaxMark, CT_SubDetails[i].MaxMark));



                }
                if (_CCID > 0)
                {
                    cell = row.Cells[3];
                    cell.MergeRight = 2;
                }
                else
                {
                    cell = row.Cells[0];
                    cell.MergeRight = 5;
                }
                cell.Elements.Add(_ExtradetailsCT);

            }
            else
            {
                if (_CCID > 0)
                {
                    cell = row.Cells[3];
                    cell.MergeRight = 2;
                }
                else
                {
                    cell = row.Cells[0];
                    cell.MergeRight = 5;
                }
                cell.AddParagraph("");
            }


            cell = row.Cells[6];
            cell.MergeRight = 1;
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.AddParagraph("A-Excellent \nB-Good \nC-Fair \nD-Satisfactory \nE- Unsatisfactory");
            
            

            return TableMain;
            
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

        private Table GetTableFormant()
        {
            MigraDoc.DocumentObjectModel.Font font1 = new MigraDoc.DocumentObjectModel.Font("Times New Roman", 11);
            Column column;
            Table _MarkDetails = new Table();
            _MarkDetails.Rows.Height = 15;

            _MarkDetails.Format.Font = font1;
            _MarkDetails.Borders.Width = 0.5;
            
            column = _MarkDetails.AddColumn(Unit.FromCentimeter(1));
            column = _MarkDetails.AddColumn(Unit.FromCentimeter(5));
            column = _MarkDetails.AddColumn(Unit.FromCentimeter(2));
            column = _MarkDetails.AddColumn(Unit.FromCentimeter(2));
            column = _MarkDetails.AddColumn(Unit.FromCentimeter(2));
            column = _MarkDetails.AddColumn(Unit.FromCentimeter(2));
            column = _MarkDetails.AddColumn(Unit.FromCentimeter(2));
            column = _MarkDetails.AddColumn(Unit.FromCentimeter(2));
            return _MarkDetails;
        }


        private Table GetMainReportDetails(MarkNode[,] markNode, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, double[] _Average, MarkNode[] _TotalMarks, MarkNode[] _Ranks, string[] _Grade, double[] _MaxTotal, ExamNode[] _SubDetailsPassMark, ExamNode[] _ClassHeighest, List<ExamGridReportClass> ClsObj,int _StudentId,Table StartTable)
        {

            MarkNode[] ClassPosition = new MarkNode[_SubDetails.Length];
            ClassPosition = GetPositions(ClsObj, _StudentId, _SubDetails, markNode,_ExamDetails);
            Table _MarkDetails = StartTable;
          
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
            cell = row.Cells[6];
            cell.AddParagraph("Class Highest");
            cell = row.Cells[7];
            cell.AddParagraph("Class Position");

           
           
            row = _MarkDetails.AddRow();
            row.Height = 150;
            row.Format.Alignment = ParagraphAlignment.Left;
            cell = row.Cells[0];
                       
            for (int Subjct = 0; Subjct < _SubDetails.Length; Subjct++)
            {
                int temp = 0;
                   cell = row.Cells[0];
                   cell.Format.Alignment = ParagraphAlignment.Center;
                   int i=Subjct+1;
                    cell.AddParagraph(i.ToString()+"\n");
                cell = row.Cells[1];
                string subName="";
                if (_SubDetails[Subjct].Name.Length > 24)
                    subName = _SubDetails[Subjct].Name.Substring(0, 21)+"..";
                else
                    subName = _SubDetails[Subjct].Name;
                cell.AddParagraph(subName+"\n");
                cell = row.Cells[2];
                cell.Format.Alignment = ParagraphAlignment.Center;
                cell.AddParagraph(_SubDetails[Subjct].MaxMark.ToString()+ "\n");
                cell = row.Cells[3];
                cell.Format.Alignment = ParagraphAlignment.Center;
                cell.AddParagraph(_SubDetailsPassMark[Subjct].MaxMark.ToString("0")+"\n");
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
                 cell = row.Cells[6];
                 cell.Format.Alignment = ParagraphAlignment.Center;
                
                     cell.AddParagraph(_ClassHeighest[Subjct].MaxMark.ToString() + "\n");
                
                 cell = row.Cells[7];
                 cell.Format.Alignment = ParagraphAlignment.Center;
                 if (temp == 1)
                 {
                     cell.AddParagraph("\n");
                 }
                 else
                 {
                     cell.AddParagraph(int.Parse(ClassPosition[Subjct].MaxMark.ToString()) + "\n");
                 }
                
               
            }
            double _TotalPassMark=0;
            for (int j=0;j<_SubDetailsPassMark.Length;j++)
                _TotalPassMark=_TotalPassMark+_SubDetailsPassMark[j].MaxMark;

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
            cell.AddParagraph(_MaxTotal[_ExamDetails.Length-1].ToString());
            cell = row.Cells[3];
            cell.Format.Alignment = ParagraphAlignment.Center;
            cell.AddParagraph(_TotalPassMark.ToString());
            cell = row.Cells[4];
            cell.Format.Alignment = ParagraphAlignment.Center;
            cell.AddParagraph(_TotalMarks[_ExamDetails.Length-1].MaxMark.ToString()); 
            cell = row.Cells[5];
            cell.Format.Alignment = ParagraphAlignment.Center;  
            cell.MergeRight = 2;

            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.AddParagraph("Percentage :" + Math.Round(_Average[_ExamDetails.Length-1]).ToString()); 

            return _MarkDetails;
        }

        private MarkNode[] GetPositions(List<ExamGridReportClass> ClsObj, int _StudentId, ExamNode[] _SubDetails, MarkNode[,] markNode, ExamNode[] _ExamDetails)
        {
            MarkNode[] _Position = new MarkNode[_SubDetails.Length];
            for (int Subjct = 0; Subjct < _SubDetails.Length; Subjct++)
            {
                _Position[Subjct].Id = _SubDetails[Subjct].Id;
                _Position[Subjct].MaxMark = 1;
            }
            //for (int i = 0; i < ClsObj.Count; i++)
            //{
                
            //    if (ClsObj[i].m_StudentId != _StudentId)
            //    {
            //        for (int Subjct = 0; Subjct < _SubDetails.Length; Subjct++)
            //        {
                       
            //            if (ClsObj[i].m_ExamMarks[_ExamDetails.Length - 1, Subjct].MaxMark > markNode[_ExamDetails.Length - 1, Subjct].MaxMark)
            //            {
            //                _Position[Subjct].MaxMark = _Position[Subjct].MaxMark + 1;
            //            }
            //        }
            //    }
            
            //}
            
            
            int temp = 0;
            for (int Subjct = 0; Subjct < _SubDetails.Length; Subjct++)
            {
                ArrayList _markarray = new ArrayList();
                for (int i = 0; i < ClsObj.Count; i++)
                {
                    if (ClsObj[i].m_StudentId != _StudentId)
                    {
                        for (int j = 0; j < _markarray.Count; j++)
                        {

                            //int l = markarray.GetUpperBound(markarray.Rank-1);
                            if (double.Parse(_markarray[j].ToString()) == ClsObj[i].m_ExamMarks[_ExamDetails.Length - 1, Subjct].MaxMark)
                            {
                                temp = 1;
                                break;
                            }

                        }
                        if (temp != 1)
                            _markarray.Add(ClsObj[i].m_ExamMarks[_ExamDetails.Length - 1, Subjct].MaxMark);
                        else
                            temp = 0;
                    }
                }

                for (int k = 0; k < _markarray.Count; k++)
                {
                    if (double.Parse(_markarray[k].ToString()) > markNode[_ExamDetails.Length - 1, Subjct].MaxMark)
                    {
                        _Position[Subjct].MaxMark = _Position[Subjct].MaxMark + 1;
                    }
                }


            }

            return _Position;
        }




        /*private int GetExamId(string _ExamName)
        {
            int _Id = 0;
            string sql = "select tblexammaster.id from tblexammaster where lower(tblexammaster.ExamName)=lower('" + _ExamName + "')";
            OdbcDataReader Myreader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (Myreader.HasRows)
            {
                int.TryParse(Myreader.GetValue(0).ToString(), out  _Id);
            }
            return _Id;
        }*/
        private void GetClassHighestandPositon(ExamNode[] _ExamDetails, ExamNode[] _SubDetails, out ExamNode[] _ClassHeighest)
        {
            _ClassHeighest = new ExamNode[_SubDetails.Length];


            string _sqlMarksColum = "select tblexammark.MarkColumn, tblexammark.SubjectId  from tblexammark where tblexammark.ExamSchId=" + _ExamDetails[_ExamDetails.Length - 1].Id + " ORDER BY tblexammark.SubjectOrder";
            DataSet ColumnDetails = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(_sqlMarksColum);

            string sqlMark = "";
            if (ColumnDetails != null && ColumnDetails.Tables[0].Rows.Count > 0)
            {
                string _Marks = "";

                foreach (DataRow dr in ColumnDetails.Tables[0].Rows)
                {
                    if (_Marks != "")
                        _Marks = _Marks + ",";
                    _Marks = _Marks + " max(" + dr["MarkColumn"].ToString() + ") as " + dr["MarkColumn"].ToString() + " ";
                }

                sqlMark = "Select " + _Marks + " from  tblstudentmark where  tblstudentmark.ExamSchId=" + _ExamDetails[_ExamDetails.Length - 1].Id;
            }

            if (sqlMark != "")
            {
                DataSet _Marks = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sqlMark);

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
                                    double.TryParse (dr1[dr["MarkColumn"].ToString()].ToString(),out   _ClassHeighest[j].MaxMark);
                                 
                                }

                            }
                        }
                    }

                }

            }

           
            
            
        }

        private void GetExamWiseAverage(ExamNode[] _ExamDetails, ExamNode[] _SubDetails, int count, out ExamNode[] _SubjectWiseClassPer, out double _ClassPersentage)
        {
            _SubjectWiseClassPer = new ExamNode[_SubDetails.Length];
            
            _ClassPersentage= 0;
            string _sqlMarksColum = "select tblexammark.MarkColumn, tblexammark.SubjectId  from tblexammark where tblexammark.ExamSchId=" + _ExamDetails[_ExamDetails.Length - 1].Id + " ORDER BY tblexammark.SubjectOrder";
            DataSet ColumnDetails = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(_sqlMarksColum);

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
                DataSet _Marks = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sqlMark);

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
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sqlMark);
            if (MyReader.HasRows)
            {
                _ClassPersentage = (double.Parse(MyReader.GetValue(0).ToString()) / double.Parse(MyReader.GetValue(1).ToString()) * 100.0);
            }


        }
    
        private void GetSubDetails(int ClassId, int ExamId, out ExamNode[] _SubDetails)
        {
            //string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=tblexamschedule.ClassExamId and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId  where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblclassexam.ClassId=" + ClassId + " and tblexammaster.Id=" + ExamId;
            string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark ,tblsubjects.SubjectCode,tblexammark.SubjectOrder  from tblclassexamsubmap inner JOIN tblsubjects on tblclassexamsubmap.SubId= tblsubjects.Id inner JOIN tblclassexam on tblclassexam.id= tblclassexamsubmap.ClassExamId    inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.id  inner JOIN tblexammark on  tblexammark.SubjectId=tblsubjects.Id and tblexammark.ExamSchId =tblexamschedule.id   where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "  and tblclassexam.ClassId=" + ClassId + " and tblclassexam.ExamId=" + ExamId+" order by tblexammark.SubjectOrder";

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
      
        private void GetSubDetailsWithPassmark(int ClassId, int ExamId, out ExamNode[] _SubDetails) 
        {
            // Dominic modified on 18/10/2011
            //string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MinMark from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=tblexamschedule.ClassExamId and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId  where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblclassexam.ClassId=" + ClassId + " and tblexammaster.Id=" + ExamId;
            string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MinMark from tblclassexamsubmap inner JOIN tblsubjects on tblclassexamsubmap.SubId= tblsubjects.Id  inner JOIN tblclassexam on tblclassexam.id= tblclassexamsubmap.ClassExamId inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.id  inner JOIN tblexammark on    tblexammark.SubjectId=tblsubjects.Id  and tblexammark.ExamSchId =tblexamschedule.id  where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblclassexam.ClassId=" + ClassId + " and tblclassexam.ExamId=" + ExamId + "  order by tblexammark.SubjectOrder";
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
                    _SubDetails[i] .MaxMark = double.Parse(MyReader.GetValue(2).ToString());
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
                    _ExamDetails[i].GradeMasterId =int.Parse(MyReader.GetValue(3).ToString());

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
                    _ExamDetails[i].Name = MyReader.GetValue(1).ToString() ;
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
               count =mydataset.Tables[0].Rows.Count;

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

                dr_values["Attendance"] =_no_presentdays+"/"+_no_workingdays;
            }
            return _StdentSet;

        }
    }
}

