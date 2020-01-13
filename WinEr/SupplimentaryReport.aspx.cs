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
using System.IO;

namespace WinEr
{

    public class SupplimentaryReportClass
    {

        public MysqlClass m_MysqlDb;
        public ExamNode[] m_Eaxams;
        public ExamNode[] m_Subjects;
        public MarkNode[,] m_ExamMarks;
        public MarkNode[,] m_ExamMarksFailed;
        public MarkNode[,] m_ExamMarksWithoutPractical;
        public MarkNode[,] m_PracticalExamMarks;
        public MarkNode[] m_TotalMarks;
        public MarkNode[] m_TotalMarksWithoutPractical;
        public MarkNode[] m_PracticalTotalMarks;
        public MarkNode[] m_Ranks;
        public MarkNode[] m_RanksWithoutPractical;
        public MarkNode[] m_PracticalRanks;
        public StudentDetails m_StudDetails;

        public int m_StudentId;
        public string[] m_Grade;
        public double[] m_Average;
        public double[] m_MaxTotal;
        public string[] m_GradeWithoutPractical;
        public double[] m_AverageWithoutPractical;
        public double[] m_MaxTotalWithoutPractical;
        public string[] m_PracticalGrade;
        public double[] m_PracticalAverage;
        public double[] m_PracticalMaxTotal;

        public ExamNode[] m_CC_Eaxams;
        public ExamNode[] m_CC_Subjects;
        public MarkNode[,] m_CC_ExamMarks;

        public MarkNode[] m_CC_TotalMarks;
        public MarkNode[] m_CC_Ranks;


        public string[] m_CC_Grade;
        public double[] m_CC_Average;
        public double[] m_CC_MaxTotal;


       

        public SupplimentaryReportClass(MysqlClass _Mysqlobj, int _StudentId)
        {
            m_MysqlDb = _Mysqlobj;
            m_StudentId = _StudentId;

        }

        public bool PracticalExamDetails(ExamNode[] _Eaxams, ExamNode[] m_Subjects, DataRow _StudDetails, string _ExamType)
        {
            if (_ExamType == "Main")
            {

                m_PracticalExamMarks = new MarkNode[_Eaxams.Length, m_Subjects.Length];
                m_PracticalTotalMarks = new MarkNode[_Eaxams.Length];
                m_PracticalRanks = new MarkNode[_Eaxams.Length];
                m_PracticalGrade = new string[_Eaxams.Length];
                m_PracticalAverage = new double[_Eaxams.Length];
                m_PracticalMaxTotal = new double[_Eaxams.Length];

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

                string _sqlMarksColum = "select tblexammark.FailedMarkColumn, tblexammark.SubjectId  from tblexammark where tblexammark.ExamSchId=" + _Eaxams[i].Id + " order by tblexammark.SubjectOrder";
                DataSet ColumnDetails = m_MysqlDb.ExecuteQueryReturnDataSet(_sqlMarksColum);
                string sqlMark = "";

                if (ColumnDetails != null && ColumnDetails.Tables[0].Rows.Count > 0)
                {
                    sqlMark = "Select ";

                    foreach (DataRow dr in ColumnDetails.Tables[0].Rows)
                    {
                        sqlMark = sqlMark + dr["FailedMarkColumn"].ToString() + " , ";
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
                                        m_PracticalExamMarks[i, j].Id = _Eaxams[i].Id;
                                        m_PracticalExamMarks[i, j].Name = _Eaxams[i].Name;

                                        string _DbVal = dr["FailedMarkColumn"].ToString();
                                        string Mark = MarkSet.Tables[0].Rows[0][_DbVal].ToString();

                                        m_PracticalExamMarks[i, j].MaxMark = double.Parse(Mark);

                                        break;
                                    }
                                }
                            }

                            m_PracticalTotalMarks[i].MaxMark = double.Parse(MarkSet.Tables[0].Rows[0]["TotalMark"].ToString());
                            m_PracticalRanks[i].MaxMark = Math.Round(double.Parse(MarkSet.Tables[0].Rows[0]["Rank"].ToString()));
                            m_PracticalGrade[i] = MarkSet.Tables[0].Rows[0]["Grade"].ToString();
                            m_PracticalAverage[i] = double.Parse(MarkSet.Tables[0].Rows[0]["Avg"].ToString());
                            m_PracticalMaxTotal[i] = double.Parse(MarkSet.Tables[0].Rows[0]["TotalMax"].ToString());
                        }
                        else
                        {
                            m_PracticalTotalMarks[i].MaxMark = 0;
                            m_PracticalRanks[i].MaxMark = 0;
                            m_PracticalGrade[i] = "";
                            m_PracticalAverage[i] = 0;
                            m_PracticalMaxTotal[i] = 0;
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

                                        string _DbVal = dr["FailedMarkColumn"].ToString();
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

                string _sqlMarksColum = "select tblexammark.FailedMarkColumn, tblexammark.SubjectId  from tblexammark where tblexammark.ExamSchId=" + _Eaxams[i].Id + " order by tblexammark.SubjectOrder";
                DataSet ColumnDetails = m_MysqlDb.ExecuteQueryReturnDataSet(_sqlMarksColum);
                string sqlMark = "";

                if (ColumnDetails != null && ColumnDetails.Tables[0].Rows.Count > 0)
                {
                    sqlMark = "Select ";

                    foreach (DataRow dr in ColumnDetails.Tables[0].Rows)
                    {
                        sqlMark = sqlMark + dr["FailedMarkColumn"].ToString() + " , ";
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
                                        //string Mark = "0";
                                        m_ExamMarks[i, j].Id = _Eaxams[i].Id;
                                        m_ExamMarks[i, j].Name = _Eaxams[i].Name;

                                        string _DbVal = dr["FailedMarkColumn"].ToString();
                                        string Mark = MarkSet.Tables[0].Rows[0][_DbVal].ToString();

                                       // m_ExamMarks[i, j].MaxMark = _Eaxams[i].MaxMark;
                                        m_ExamMarks[i, j].MaxMark = double.Parse(Mark);
                                        //if (m_ExamMarks[i, j].MaxMark.ToString() != "-1")

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

                                        string _DbVal = dr["FailedMarkColumn"].ToString();
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

        public bool ExamDetailsFailed(ExamNode[] _Eaxams, ExamNode[] m_Subjects, DataRow _StudDetails, string _ExamType)
        {
            if (_ExamType == "Main")
            {

                m_ExamMarksFailed = new MarkNode[_Eaxams.Length, m_Subjects.Length];
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
                                        //string Mark = "0";
                                        m_ExamMarksFailed[i, j].Id = _Eaxams[i].Id;
                                        m_ExamMarksFailed[i, j].Name = _Eaxams[i].Name;

                                        string _DbVal = dr["MarkColumn"].ToString();
                                        string Mark = MarkSet.Tables[0].Rows[0][_DbVal].ToString();

                                        // m_ExamMarks[i, j].MaxMark = _Eaxams[i].MaxMark;
                                        m_ExamMarksFailed[i, j].MaxMark = double.Parse(Mark);
                                        //if (m_ExamMarks[i, j].MaxMark.ToString() != "-1")

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


        public bool ExamDetailsWithoutPractical(ExamNode[] _Eaxams, ExamNode[] m_Subjects, DataRow _StudDetails, string _ExamType)
        {
            if (_ExamType == "Main")
            {

                m_ExamMarksWithoutPractical = new MarkNode[_Eaxams.Length, m_Subjects.Length];
                m_TotalMarksWithoutPractical = new MarkNode[_Eaxams.Length];
                m_RanksWithoutPractical = new MarkNode[_Eaxams.Length];
                m_GradeWithoutPractical = new string[_Eaxams.Length];
                m_AverageWithoutPractical = new double[_Eaxams.Length];
                m_MaxTotalWithoutPractical = new double[_Eaxams.Length];

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

                string _sqlMarksColum = "select tblexammark.FailedMarkColumn, tblexammark.SubjectId  from tblexammark where tblexammark.ExamSchId=" + _Eaxams[i].Id + " order by tblexammark.SubjectOrder";
                DataSet ColumnDetails = m_MysqlDb.ExecuteQueryReturnDataSet(_sqlMarksColum);
                string sqlMark = "";

                if (ColumnDetails != null && ColumnDetails.Tables[0].Rows.Count > 0)
                {
                    sqlMark = "Select ";

                    foreach (DataRow dr in ColumnDetails.Tables[0].Rows)
                    {
                        sqlMark = sqlMark + dr["FailedMarkColumn"].ToString() + " , ";
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
                                        m_ExamMarksWithoutPractical[i, j].Id = _Eaxams[i].Id;
                                        m_ExamMarksWithoutPractical[i, j].Name = _Eaxams[i].Name;

                                        string _DbVal = dr["FailedMarkColumn"].ToString();
                                        string Mark = MarkSet.Tables[0].Rows[0][_DbVal].ToString();

                                        m_ExamMarksWithoutPractical[i, j].MaxMark = double.Parse(Mark);

                                        break;
                                    }
                                }
                            }

                            m_TotalMarksWithoutPractical[i].MaxMark = double.Parse(MarkSet.Tables[0].Rows[0]["TotalMark"].ToString());
                            m_RanksWithoutPractical[i].MaxMark = Math.Round(double.Parse(MarkSet.Tables[0].Rows[0]["Rank"].ToString()));
                            m_GradeWithoutPractical[i] = MarkSet.Tables[0].Rows[0]["Grade"].ToString();
                            m_AverageWithoutPractical[i] = double.Parse(MarkSet.Tables[0].Rows[0]["Avg"].ToString());
                            m_MaxTotalWithoutPractical[i] = double.Parse(MarkSet.Tables[0].Rows[0]["TotalMax"].ToString());
                        }
                        else
                        {
                            m_TotalMarksWithoutPractical[i].MaxMark = 0;
                            m_RanksWithoutPractical[i].MaxMark = 0;
                            m_GradeWithoutPractical[i] = "";
                            m_AverageWithoutPractical[i] = 0;
                            m_MaxTotalWithoutPractical[i] = 0;
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

                                        string _DbVal = dr["FailedMarkColumn"].ToString();
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


    public partial class SupplimentaryReport : System.Web.UI.Page
    {
        private ExamManage MyExamMang;
        private KnowinUser MyUser;
        private MysqlClass _Mysqlobj;
        private ClassOrganiser MyClassMngr;
        private SchoolClass objSchool = null;
        private string M_Logo = "";

        #region Events

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
            //LoadStudentsDetailsToDropDown();
        }

        protected void Btn_ExamReport_Click(object sender, EventArgs e)
        {
            string Msg = "";
            if (ValidData(out Msg))
            {
                LoadReport(int.Parse(Drp_SelectClass.SelectedValue), int.Parse(Drp_Exam.SelectedValue), int.Parse(Drp_SelectStudent.SelectedValue));
            }
            else
            {
                Lbl_Err.Text = Msg;
            }
        }

        #endregion

        #region Report Area

        private void LoadReport(int ClassId, int ExamId, int StudentId)
        {
            int classId = 0;
            int studentId = 0;

            if (GatBaseValuese(out classId, out studentId))
            {
                DataSet TotalStudList;
                DataSet totalMarkRatios = null;

                ExamNode[] _ExamDetails;
                ExamNode[] _SubDetails;
                ExamNode[] _PracticalSubDetails;
                ExamNode[] _SubDetailsWithoutPractical;
                ExamNode[] CC_ExamDetails = null;
                ExamNode[] CC_SubDetails = null;

                SchoolDetails _SchoolDetails;
                List<SupplimentaryReportClass> ClsObj = new List<SupplimentaryReportClass>();
                SupplimentaryReportClass _ReportObj;

                GetSchoolDetails(out _SchoolDetails);
                GetExamDetails(ClassId, ExamId, out _ExamDetails);
                GetSubDetails(ClassId, _ExamDetails[_ExamDetails.Length - 1].Id, out _SubDetails);
                GetPracticalSubDetails(ClassId, _ExamDetails[_ExamDetails.Length - 1].Id, out _PracticalSubDetails);
                GetSubDetailsWithoutPractical(ClassId, _ExamDetails[_ExamDetails.Length - 1].Id, out _SubDetailsWithoutPractical);

                if (_ExamDetails != null && _SubDetails != null && _ExamDetails.Length > 0 && _SubDetails.Length > 0)
                {
                    TotalStudList = GetAllStudentListFromClass(studentId, classId,_ExamDetails);

                    GetExamDetails(ClassId, ExamId, out CC_ExamDetails);
                    GetCC_SubDetails(int.Parse(Drp_SelectClass.SelectedValue), "CBSE ACTIVITY REPORT", out CC_SubDetails);

                    totalMarkRatios = GetMarkRatios(Drp_SelectClass.SelectedValue);


                    foreach (DataRow Dr in TotalStudList.Tables[0].Rows)
                    {
                        _ReportObj = new SupplimentaryReportClass(_Mysqlobj, int.Parse(Dr[0].ToString()));
                        _ReportObj.ExamDetails(_ExamDetails, _SubDetails, Dr, "Main");
                        _ReportObj.ExamDetailsFailed(_ExamDetails, _SubDetails, Dr, "Main");
                        _ReportObj.PracticalExamDetails(_ExamDetails, _PracticalSubDetails, Dr, "Main");
                        _ReportObj.ExamDetailsWithoutPractical(_ExamDetails, _SubDetailsWithoutPractical, Dr, "Main");


                        _ReportObj.ExamDetails(CC_ExamDetails, CC_SubDetails, Dr, "CC");

                        ClsObj.Add(_ReportObj);
                    }


                    CreateReport(ClsObj, _ExamDetails, _SubDetails, CC_ExamDetails, CC_SubDetails, _SchoolDetails, studentId, totalMarkRatios, _PracticalSubDetails);
                }
                else
                {
                    Lbl_Err.Text = "Exam details not found";
                }



            }

        }
        private string GtSchoolCode()
        {

            String SchoolCode = "";

            string sql = "select tblschooldetails.SchoolCode from tblschooldetails";
            OdbcDataReader MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                SchoolCode = MyReader.GetValue(0).ToString();

            }

            return SchoolCode;



        }

        private string GtdiesCode()
        {

            String DIESCode = "";

            string sql = "select tblschooldetails.DIESCode from tblschooldetails";
            OdbcDataReader MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                DIESCode = MyReader.GetValue(0).ToString();

            }

            return DIESCode;



        }


        #region Create Records and Write to File

        private Document LoadPDFPage(List<SupplimentaryReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, SchoolDetails _SchoolDetails, ExamNode[] CC_ExamDetails, ExamNode[] CC_SubDetails, DataSet totalMarkRatios, ExamNode[] _PracticalSubDetails)
        {
            string path = GtSchoolLogo();
            string schoolcode = GtSchoolCode();
            string diescode = GtdiesCode();
            Document document = new Document();

            for (int i = 0; i < ClsObj.Count; i++)
            {

                PageSetup PgSt = document.DefaultPageSetup;
                PgSt.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.A4;
                PgSt.LeftMargin = 0;
                PgSt.RightMargin = 0;
                PgSt.TopMargin = 30;
                PgSt.BottomMargin = 0;

                PgSt.HeaderDistance = 0;
                PgSt.FooterDistance = 0;
                Section section = document.AddSection();
                MigraDoc.DocumentObjectModel.Tables.Table table = section.AddTable();
                table.Borders.Width = 2;
                table.Rows.LeftIndent = 25;
                MigraDoc.DocumentObjectModel.Tables.Column Col = table.AddColumn(PgSt.PageWidth - 50);
                MigraDoc.DocumentObjectModel.Tables.Row row = table.AddRow();
                row.Height = PgSt.PageHeight - 100;
                Col.Borders.Visible = true;
                MigraDoc.DocumentObjectModel.Tables.Cell cel = row.Cells[0];
                Paragraph paragraph = section.AddParagraph();
                paragraph = cel.AddParagraph();
                paragraph.AddLineBreak();
                MigraDoc.DocumentObjectModel.Tables.Table tb = new MigraDoc.DocumentObjectModel.Tables.Table();
                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);
                tb.AddColumn((PgSt.PageWidth - 80) / 7);
                tb.AddColumn((PgSt.PageWidth - 80) - ((PgSt.PageWidth - 80) / 7));
                row = tb.AddRow();
                row.Cells[0].AddImage(path);
                row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[0].MergeDown = 1;
                row.Cells[1].Format.Font.Size = 20;
                row.Cells[1].Format.Font.Bold = true;
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].AddParagraph(MyUser.SchoolName.ToUpperInvariant());
                row = tb.AddRow();
                row.Cells[1].Format.Font.Size = 14;
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].AddParagraph(_SchoolDetails.Address);
                if (schoolcode != "" && diescode != "")
                {
                    row = tb.AddRow();
                    row.Cells[1].Format.Font.Size = 14;
                    row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[1].AddParagraph("School Code:" + schoolcode + ",DISE Code:" + diescode);
                }
                row = tb.AddRow();
                row.Cells[1].Format.Font.Size = 18;
                row.Cells[1].Format.Font.Bold = true;
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].AddParagraph(Txt_ReportName.Text.ToUpper());
                row = tb.AddRow();
                row.Cells[1].Format.Font.Size = 16;
                row.Cells[1].Format.Font.Bold = true;
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].AddParagraph("SESSION:" + MyUser.CurrentBatchName);
                cel.Elements.Add(tb);
                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 14);
                paragraph.AddFormattedText("STUDENT INFORMATIONS", TextFormat.Underline | TextFormat.Bold);
                paragraph.AddLineBreak();
                tb = new MigraDoc.DocumentObjectModel.Tables.Table();
                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 12);
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
                row.Cells[0].AddParagraph("");
                row.Cells[1].AddParagraph("");
                row.Cells[2].AddParagraph("");
                row.Cells[3].AddParagraph("");
                row = tb.AddRow();
                row.Cells[0].AddParagraph("Date of Birth");
                row.Cells[1].AddParagraph(":" + ClsObj[i].m_StudDetails.DOB);
                row.Cells[2].AddParagraph("Mother's Name");
                row.Cells[3].AddParagraph(":" + ClsObj[i].m_StudDetails.MotherName);
                row = tb.AddRow();
                row.Cells[0].AddParagraph("");
                row.Cells[1].AddParagraph("");
                row.Cells[2].AddParagraph("");
                row.Cells[3].AddParagraph("");
                row = tb.AddRow();
                row.Cells[0].AddParagraph("Admission No");
                row.Cells[1].AddParagraph(":" + ClsObj[i].m_StudDetails.AdmissionNum);
                row.Cells[2].AddParagraph("Telephone No");
                row.Cells[3].AddParagraph(":" + ClsObj[i].m_StudDetails.Tel);
                row = tb.AddRow();
                row.Cells[0].AddParagraph("");
                row.Cells[1].AddParagraph("");
                row.Cells[2].AddParagraph("");
                row.Cells[3].AddParagraph("");
                row = tb.AddRow();
                row.Cells[0].AddParagraph("Class");
                row.Cells[1].AddParagraph(":" + ClsObj[i].m_StudDetails.Class);
                row.Cells[2].AddParagraph("Address");
                row.Cells[3].AddParagraph(":" + ClsObj[i].m_StudDetails.Add);
                row = tb.AddRow();
                row.Cells[0].AddParagraph("");
                row.Cells[1].AddParagraph("");
                row.Cells[2].AddParagraph("");
                row.Cells[3].AddParagraph("");
                row = tb.AddRow();
                row.Cells[0].AddParagraph("Roll No");
                row.Cells[1].AddParagraph(":" + ClsObj[i].m_StudDetails.RollNum);
                row.Cells[2].AddParagraph("");
                row.Cells[3].AddParagraph("");
                tb.Borders.Visible = false;
                cel.Elements.Add(tb);
                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 14);
                paragraph.AddLineBreak();
                paragraph.AddFormattedText("", TextFormat.Underline | TextFormat.Bold);
                paragraph.AddLineBreak();
                tb = new MigraDoc.DocumentObjectModel.Tables.Table();
                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 12);
                tb.AddColumn((PgSt.PageWidth - 80) * 4 / 12.5);
                tb.AddColumn((PgSt.PageWidth - 80) / 12);
                tb.AddColumn((PgSt.PageWidth - 80) / 12);
                tb.AddColumn((PgSt.PageWidth - 80) / 12);
                tb.AddColumn((PgSt.PageWidth - 80) / 12);
                tb.AddColumn((PgSt.PageWidth - 80) / 12);
                tb.AddColumn((PgSt.PageWidth - 80) / 12);
                tb.AddColumn((PgSt.PageWidth - 80) / 10);
                tb.AddColumn((PgSt.PageWidth - 80) / 9);
                row = tb.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("SUBJECT", TextFormat.Bold);
                if (_ExamDetails.Length > 0)
                {
                    int TempVal = 0, k = 0;
                    for (int j = 0; j < _ExamDetails.Length; j++)
                    {
                        if (TempVal != 0 && j == 3)
                        {
                            TempVal = 0;
                            k = k + 4;
                        }
                        string TypeName = "";
                        if (j == 0)
                        {
                            TypeName = "TERM I";
                            row.Cells[k + 1].AddParagraph().AddFormattedText(_ExamDetails[j].Name, TextFormat.Bold); //Exam Name
                            row.Cells[k + 1].MergeRight = 5;
                            row.Cells[k + 1].Format.Alignment = ParagraphAlignment.Center;
                        }
                        else if (j == 3)
                        {
                            TypeName = "TERM II";
                            row.Cells[k + 1].AddParagraph().AddFormattedText(_ExamDetails[j].Name, TextFormat.Bold); //Exam Name
                            row.Cells[k + 1].MergeRight = 3;
                            row.Cells[k + 1].Format.Alignment = ParagraphAlignment.Center;
                        }
                        TempVal = 1;
                    }
                    row.Cells[7].AddParagraph().AddFormattedText("TOTAL", TextFormat.Bold);
                    row[7].MergeDown = 2;
                    row.Cells[8].AddParagraph().AddFormattedText("REMARKS", TextFormat.Bold);
                    row[8].MergeDown = 2;
                    row = tb.AddRow();
                    row.Cells[1].AddParagraph().AddFormattedText("THEORY", TextFormat.Bold);
                    row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[1].MergeRight = 2;
                    row.Cells[4].AddParagraph().AddFormattedText("PRACTICAL", TextFormat.Bold);
                    row.Cells[4].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[4].MergeRight = 2;
                    TempVal = 0;
                    k = 0;
                    string tempName = "";
                    row[0].Column[0].MergeDown = 2;
                    row = tb.AddRow();
                    row.Cells[1].AddParagraph().AddFormattedText("M.M", TextFormat.Bold);
                    row.Cells[2].AddParagraph().AddFormattedText("P.M", TextFormat.Bold);
                    row.Cells[3].AddParagraph().AddFormattedText("M.O", TextFormat.Bold);
                    row.Cells[4].AddParagraph().AddFormattedText("M.M", TextFormat.Bold);
                    row.Cells[5].AddParagraph().AddFormattedText("P.M", TextFormat.Bold);
                    row.Cells[6].AddParagraph().AddFormattedText("M.O", TextFormat.Bold);
                }
                DataSet Grade = GetGradeDataSet(_ExamDetails[_ExamDetails.Length - 1].GradeMasterId);
                double[] colWiseTotalMarkSum = new double[_ExamDetails.Length];//  this is used for save subject wise mark
                double[] colWiseTotalMarkSumPractical = new double[_ExamDetails.Length];
                double[] colWiseTotalMaxSum = new double[_ExamDetails.Length];
                double[] colWiseTotalMarkSum1 = new double[_ExamDetails.Length];//  this is used for save subject wise mark
                double[] colWiseTotalMaxSum1 = new double[_ExamDetails.Length];
                double rowWiseSubjectMark = 0;//this two variable using  for FA1+FA2+SA1 and FA3+FA4+SA2
                double rowWiseSubjectMax = 0;
                double rowWiseFAexamMark = 0;// this two variable using for FA1+FA2+FA3+FA4
                double rowWiseFAexamMax = 0;
                double rowWiseSAExamMark = 0;//this two variable using for SA1+SA2
                double rowWiseSAExamMax = 0;
                double Ratio = 0;
                double MainRatio = 0;
                double subRatio = 0;
                double FARatio = 0;
                double SARatio = 0;
                double Term1Ratio = 0;
                double Term2Ratio = 0;
                double _maximummark = 0;
                double _maxpassmark = 0;
                double _maximummarkpractical = 0;
                double _maxpassmarkpractical = 0;
                double practicalmark = 0;
                double subjmark = 0;
                string word = "";
                double MinimumMark = 0;
                double failmark = 0;
                double totalpassmark = 0;
                double totalmarkobt = 0;
                double totalSubMark = 0;
                int columnCount = 0;
                for (int Subjct = 0; Subjct < _SubDetails.Length; Subjct++)
                {
                    string subjectname = _SubDetails[Subjct].Name;
                    if (!subjectname.Contains("Practical"))
                    {
                        for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                        {                         
                          failmark = double.Parse(ClsObj[i].m_ExamMarksFailed[ExamCount, Subjct].MaxMark.ToString());
                          MinimumMark = GetMinMarkFromExamScheduleIdandSubjectId(_ExamDetails[ExamCount].Id, _SubDetails[Subjct].Id);
                          if (failmark < MinimumMark)
                           {
                               row.Cells[columnCount].AddParagraph().AddFormattedText(totalSubMark.ToString());
                               columnCount = 0;
                               totalSubMark = 0;
                               row = tb.AddRow();
                               row.Cells[0].AddParagraph().AddFormattedText(_SubDetails[Subjct].Name + "*"); //Subject Name
                           }
                            else
                           {
                               row.Cells[columnCount].AddParagraph().AddFormattedText(totalSubMark.ToString());
                               columnCount = 0;
                               totalSubMark = 0;
                               row = tb.AddRow();
                               row.Cells[0].AddParagraph().AddFormattedText(_SubDetails[Subjct].Name); //Subject Name
                           }
                        }                        
                    }
                    int k = 0;
                    for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                    {
                        double r_Mark = 0;
                        double r_Max = 0;
                        k++;
                        string _Grade = "";
                        double examMark = 0;
                        double examMarkPractical = 0;
                        double MaxMark = 1;
                        double PracticalMaxMark = 1;
                        double MinMark = 1;
                        double PracticalMinMark = 1;
                        string DefaultName = "";
                        if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString() != "-1")
                        {
                            string sql = "SELECT tblcbseexamratiomap.RatioColumName from tblcbseexamratiomap where tblcbseexamratiomap.ExamName='" + _ExamDetails[ExamCount].Name + "'";
                            OdbcDataReader MyReader = MyClassMngr.m_MysqlDb.ExecuteQuery(sql);
                            if (MyReader.HasRows)
                                DefaultName = MyReader.GetValue(0).ToString();
                            _Grade = GetGradeFromMarks(Grade, totalMarkRatios, DefaultName, double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString()), MaxMark, out r_Mark, out  r_Max);
                        }
                        MinMark = GetMinMarkFromExamScheduleIdandSubjectId(_ExamDetails[ExamCount].Id, _SubDetails[Subjct].Id);
                        MaxMark = MyExamMang.GetMaxMarkFromExamScheduleIdandSubjectId(_ExamDetails[ExamCount].Id, _SubDetails[Subjct].Id);
                        if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark == -1)
                        {
                            examMark =  0;
                        }
                        else
                        {
                            examMark = ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark;
                        }
                        colWiseTotalMarkSum[ExamCount] = colWiseTotalMarkSum[ExamCount] + examMark;
                        colWiseTotalMaxSum[ExamCount] = colWiseTotalMaxSum[ExamCount] + MaxMark;
                        if (!subjectname.Contains("Practical"))
                        {                                                       
                            subjmark = (double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString()));
                            if (subjmark == -1)
                            {
                                subjmark = 0;
                            }
                            _maximummark = _maximummark + MaxMark;
                            _maxpassmark = _maxpassmark + MinMark;
                            row.Cells[k].AddParagraph().AddFormattedText(MaxMark.ToString());
                            row.Cells[k + 1].AddParagraph().AddFormattedText(MinMark.ToString());
                            row.Cells[k + 2].AddParagraph().AddFormattedText(subjmark.ToString());
                        }
                        if (subjectname.Contains("Practical"))
                        {
                            int PracticalSubjct = 0;
                            PracticalMaxMark = MyExamMang.GetMaxMarkFromExamScheduleIdandSubjectName(_ExamDetails[ExamCount].Id, subjectname);
                            string sql = "SELECT tblcbseexamratiomap.RatioColumName from tblcbseexamratiomap where tblcbseexamratiomap.ExamName='" + _ExamDetails[ExamCount].Name + "'";
                            OdbcDataReader MyReader = MyClassMngr.m_MysqlDb.ExecuteQuery(sql);
                            if (MyReader.HasRows)
                                DefaultName = MyReader.GetValue(0).ToString();
                            PracticalMinMark = GetMinMarkFromExamScheduleIdandSubjectName(_ExamDetails[ExamCount].Id, subjectname);
                            practicalmark = ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark;
                            if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark == -1)
                            {
                                examMarkPractical  = 0;
                            }
                            else
                            {
                                examMarkPractical = ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark;
                            }
                            colWiseTotalMarkSumPractical[ExamCount] = colWiseTotalMarkSumPractical[ExamCount] + examMarkPractical;
                            _maximummarkpractical = _maximummarkpractical + PracticalMaxMark;

                            _maxpassmarkpractical = _maxpassmarkpractical + PracticalMinMark;

                            if (practicalmark == -1)
                            {
                                practicalmark = 0;
                            }
                            PracticalSubjct++;

                            row.Cells[k + 3].AddParagraph().AddFormattedText(MaxMark.ToString());

                            row.Cells[k + 4].AddParagraph().AddFormattedText(MinMark.ToString());

                            row.Cells[k + 5].AddParagraph().AddFormattedText(practicalmark.ToString());
                        }
                        if (subjectname.Contains("Practical"))
                        {
                            totalSubMark = subjmark + practicalmark;
                            columnCount = k + 6;
                            practicalmark = 0;
                        }
                        if (!subjectname.Contains("Practical"))
                        {
                            totalSubMark = subjmark;
                            columnCount = k + 6;
                            practicalmark = 0;
                        }
                        rowWiseSubjectMark = rowWiseSubjectMark + GetMarkRations(totalMarkRatios, DefaultName, subjmark, out Ratio);
                        rowWiseSubjectMax = rowWiseSubjectMax + GetMarkRations(totalMarkRatios, _ExamDetails[ExamCount].Name, 100, out Ratio);
                        if (Subjct == 0)
                        {
                            MainRatio = MainRatio + Ratio;
                        }
                        subRatio = subRatio + Ratio;
                        if (DefaultName != "SA1" && DefaultName != "SA2")
                        {
                            rowWiseFAexamMark = rowWiseFAexamMark + GetMarkRations(totalMarkRatios, DefaultName, subjmark, out Ratio);
                            rowWiseFAexamMax = rowWiseFAexamMax + GetMarkRations(totalMarkRatios, _ExamDetails[ExamCount].Name, 100, out Ratio);
                            if (Subjct == 0)
                            {
                                FARatio = FARatio + Ratio;
                            }
                        }
                        else if (DefaultName == "SA1" || DefaultName == "SA2")
                        {
                            rowWiseSAExamMark = rowWiseSAExamMark + GetMarkRations(totalMarkRatios, DefaultName, subjmark, out Ratio);
                            rowWiseSAExamMax = rowWiseSAExamMax + GetMarkRations(totalMarkRatios, _ExamDetails[ExamCount].Name, 100, out Ratio);
                            if (Subjct == 0)
                            {
                                SARatio = SARatio + Ratio;
                            }
                        }
                        colWiseTotalMarkSum1[ExamCount] = colWiseTotalMarkSum1[ExamCount] + GetMarkRations(totalMarkRatios, DefaultName, subjmark, out Ratio);
                        colWiseTotalMaxSum1[ExamCount] = colWiseTotalMaxSum1[ExamCount] + GetMarkRations(totalMarkRatios, _ExamDetails[ExamCount].Name, 100, out Ratio);
                        if (ExamCount == 2 || ExamCount == 5)
                        {

                            if (ExamCount == 2 && Subjct == 0)
                            {
                                Term1Ratio = subRatio;
                            }
                            else if (ExamCount == 5 && Subjct == 0)
                            {
                                Term2Ratio = subRatio;
                            }

                            double tempMark = 0;

                            if (subRatio > 1)
                            {
                                tempMark = (rowWiseSubjectMark / 100);
                                tempMark = (tempMark / subRatio) * 100;
                            }
                            else
                                tempMark = (rowWiseSubjectMark / rowWiseSubjectMax) * 100;

                            _Grade = GetGradeFromAvg(Grade, tempMark);
                            k = k + 1;

                            row.Cells[k].AddParagraph().AddFormattedText(_Grade);

                            rowWiseSubjectMark = 0;
                            rowWiseSubjectMax = 0;
                            subRatio = 0;
                        }
                    }
                    string last_Grade = "";
                    double t_mark = 0, t_Max = 0, Marks = 0;
                    if (_ExamDetails.Length == 6)
                    {
                        if (FARatio > 1)
                        {
                            Marks = (rowWiseFAexamMark / 100);
                            Marks = (Marks / FARatio) * 100;
                        }
                        else
                            Marks = (rowWiseFAexamMark / rowWiseFAexamMax) * 100;
                        last_Grade = GetGradeFromAvg(Grade, Marks);
                        row.Cells[7].AddParagraph().AddFormattedText(last_Grade);
                        if (SARatio > 1)
                        {
                            Marks = (rowWiseSAExamMark / 100);
                            Marks = (Marks / SARatio) * 100;
                        }
                        else
                            Marks = (rowWiseSAExamMark / rowWiseSAExamMax) * (100);

                        last_Grade = GetGradeFromAvg(Grade, Marks);
                        row.Cells[8].AddParagraph().AddFormattedText(last_Grade);
                        double _totalMark = rowWiseFAexamMark + rowWiseSAExamMark;
                        if (MainRatio > 1)

                            Marks = (_totalMark / MainRatio);
                        else
                        {
                            Marks = (_totalMark / _ExamDetails.Length * 100) * 100;
                        }

                        last_Grade = GetGradeFromAvg(Grade, Marks);
                    }
                    rowWiseFAexamMark = 0;
                    rowWiseSAExamMark = 0;
                    rowWiseFAexamMax = 0;
                    rowWiseSAExamMax = 0;
                }
                row.Cells[columnCount].AddParagraph().AddFormattedText(totalSubMark.ToString());
                row = tb.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("TOTAL", TextFormat.Bold); //Subject Name
                int m = 0;
                double TempMark = 0;
                for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                {
                    m++;
                    string last_Grade = "";
                    double t_mark = 0, t_Max = 0;
                    string _ExamDefaultName = "";
                    string sql = "SELECT tblcbseexamratiomap.RatioColumName from tblcbseexamratiomap where tblcbseexamratiomap.ExamName='" + _ExamDetails[ExamCount].Name + "'";
                    OdbcDataReader MyReader = MyClassMngr.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                        _ExamDefaultName = MyReader.GetValue(0).ToString();
                    last_Grade = GetGradeFromMarks(Grade, null, "", colWiseTotalMarkSum[ExamCount], colWiseTotalMaxSum[ExamCount], out t_mark, out  t_Max);
                    row.Cells[m].AddParagraph().AddFormattedText(_maximummark.ToString());                  
                    row.Cells[m + 1].AddParagraph().AddFormattedText(_maxpassmark.ToString());
                    row.Cells[m + 2].AddParagraph().AddFormattedText((colWiseTotalMarkSum[ExamCount] - colWiseTotalMarkSumPractical[ExamCount]).ToString());
                    row.Cells[m + 3].AddParagraph().AddFormattedText(_maximummarkpractical.ToString());
                    row.Cells[m + 4].AddParagraph().AddFormattedText(_maxpassmarkpractical.ToString());
                    row.Cells[m + 5].AddParagraph().AddFormattedText(colWiseTotalMarkSumPractical[ExamCount].ToString());
                    double totalexammark = (colWiseTotalMarkSum[ExamCount] - colWiseTotalMarkSumPractical[ExamCount]);
                    double totalpracticalmark = colWiseTotalMarkSumPractical[ExamCount];
                    double totalmark = totalexammark + totalpracticalmark;
                    row.Cells[m + 6].AddParagraph().AddFormattedText(totalmark.ToString());
                    word = ConvertNumbertoWords(Convert.ToInt32(colWiseTotalMarkSum[ExamCount].ToString()));
                    totalmarkobt = colWiseTotalMarkSum[ExamCount];
                    totalpassmark = _maximummark + _maximummarkpractical;
                    TempMark = (colWiseTotalMarkSum[ExamCount] / colWiseTotalMaxSum[ExamCount]) * 100;
                    rowWiseSubjectMark = rowWiseSubjectMark + GetMarkRations(totalMarkRatios, _ExamDefaultName, TempMark, out Ratio);
                    rowWiseSubjectMax = rowWiseSubjectMax + colWiseTotalMaxSum1[ExamCount];
                    if (_ExamDefaultName != "SA1" && _ExamDefaultName != "SA2")
                    {
                        rowWiseFAexamMark = rowWiseFAexamMark + GetMarkRations(totalMarkRatios, _ExamDefaultName, TempMark, out Ratio);
                    }
                    else if (_ExamDefaultName == "SA1" || _ExamDefaultName == "SA2")
                    {
                        rowWiseSAExamMark = rowWiseSAExamMark + GetMarkRations(totalMarkRatios, _ExamDefaultName, TempMark, out Ratio);
                    }
                    if (ExamCount == 2 || ExamCount == 5)
                    {
                        double Ratio1 = 0;
                        if (ExamCount == 2)
                            Ratio1 = Term1Ratio;
                        else if (ExamCount == 5)
                            Ratio1 = Term2Ratio;
                        double Mark = 0;
                        if (Ratio1 > 1)
                        {
                            Mark = rowWiseSubjectMark / 100;
                            Mark = (Mark / Ratio1) * 100;
                        }
                        else
                            Mark = (rowWiseSubjectMark / 300) * 100;
                        last_Grade = GetGradeFromAvg(Grade, Mark);
                        m = m + 1;
                        row.Cells[m].AddParagraph().AddFormattedText(last_Grade);
                        rowWiseSubjectMark = 0; rowWiseSubjectMax = 0;
                    }
                }
                string total = "";
                DateTime today = DateTime.Today;
                string date = today.ToString("dd/MM/yyyy");
                double percentage = 0;
                string sql1 = "";
                string Result = "";
                int rank = 0;
                string perc = ((totalmarkobt / totalpassmark) * 100).ToString();
                percentage=double.Parse(Math.Round(decimal.Parse(perc), 2).ToString());
                sql1 = "select tblstudentmark.Result from tblstudentmark inner join tblexamschedule on tblexamschedule.Id=tblstudentmark.ExamSchId where tblstudentmark.ExamSchId = " + _ExamDetails[0].Id + " and tblstudentmark.StudId=" + ClsObj[i].m_StudDetails.Id +"";
                OdbcDataReader MyReader1 = MyExamMang.m_MysqlDb.ExecuteQuery(sql1);
                if(MyReader1.HasRows)
                {
                    Result = MyReader1.GetValue(0).ToString();
                }
                rank = int.Parse(ClsObj[i].m_Ranks[0].MaxMark.ToString());              
                tb.Borders.Visible = true;
                cel.Elements.Add(tb);
                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 12);
                paragraph.AddLineBreak();
                tb = new MigraDoc.DocumentObjectModel.Tables.Table();
                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 12);
                tb.AddColumn((PgSt.PageWidth - 80) / 6);
                tb.AddColumn((PgSt.PageWidth - 80) / 3);
                tb.AddColumn((PgSt.PageWidth - 80) / 6);
                tb.AddColumn((PgSt.PageWidth - 80) / 3);
                row = tb.AddRow();
                row.Cells[0].AddParagraph("Mark Obt.(In Words)");
                row.Cells[1].AddParagraph(": " + word);
                row.Cells[2].AddParagraph("Result");
                row.Cells[3].AddParagraph(":" + Result);
                row = tb.AddRow();
                row.Cells[0].AddParagraph("");
                row.Cells[1].AddParagraph("");
                row.Cells[2].AddParagraph("");
                row.Cells[3].AddParagraph("");
                row = tb.AddRow();
                row.Cells[0].AddParagraph("Percentage");
                row.Cells[1].AddParagraph(": " + percentage);
                row.Cells[2].AddParagraph("Division");
                row.Cells[3].AddParagraph(":");
                row = tb.AddRow();
                row.Cells[0].AddParagraph("");
                row.Cells[1].AddParagraph("");
                row.Cells[2].AddParagraph("");
                row.Cells[3].AddParagraph("");
                row = tb.AddRow();
                row.Cells[0].AddParagraph("Rank In Class");
                row.Cells[1].AddParagraph(": ");
                row.Cells[2].AddParagraph("Date");
                row.Cells[3].AddParagraph(": " + date);
                row = tb.AddRow();
                row.Cells[0].AddParagraph("");
                row.Cells[1].AddParagraph("");
                row.Cells[2].AddParagraph("");
                row.Cells[3].AddParagraph("");
                tb.Borders.Visible = false;
                cel.Elements.Add(tb);

                // dominic new 

                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.AddLineBreak();
                paragraph.AddLineBreak();
                paragraph.AddLineBreak();
                paragraph.AddLineBreak();


                tb = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 12);

                tb.AddColumn((PgSt.PageWidth - 80) * 5 / 15);
                tb.AddColumn((PgSt.PageWidth - 80) * 5 / 15);
                tb.AddColumn((PgSt.PageWidth - 80) * 5 / 15);

                row = tb.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("");
                row.Cells[1].AddParagraph().AddFormattedText("", TextFormat.Bold); //Exam Name            
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[2].AddParagraph().AddFormattedText(""); //Exam Name
                row.Cells[2].Format.Alignment = ParagraphAlignment.Center;

                row = tb.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("CLASS TEACHER", TextFormat.Bold);
                row.Cells[1].AddParagraph().AddFormattedText("", TextFormat.Bold); //Exam Name            
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[2].AddParagraph().AddFormattedText("PRINCIPAL", TextFormat.Bold); //Exam Name
                row.Cells[2].Format.Alignment = ParagraphAlignment.Center;

                row = tb.AddRow();
                tb.Borders.Visible = false;
                cel.Elements.Add(tb);
            }
            return document;
        }


        #endregion

        #region Functions used for Create report

        private double GetMarkRatios(DataSet totalMarkRatios, string ExamName, double Mark)
        {
            double _Mark = 0;

            if (totalMarkRatios.Tables[0].Rows.Count > 0)
            {
                _Mark = Mark * double.Parse(totalMarkRatios.Tables[0].Rows[0][ExamName].ToString());
            }
            return _Mark;
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

        public double GetMinMarkFromExamScheduleIdandSubjectId(int ExamSchduleId, int SubjectId)
        {
            double MinMark = 0;
            string sql = "select tblclassexamsubmap.MinMark from tblclassexamsubmap inner join tblexamschedule on  tblexamschedule.ClassExamId= tblclassexamsubmap.ClassExamId where tblclassexamsubmap.SubId=" + SubjectId + " and tblexamschedule.Id=" + ExamSchduleId + "";
            DataSet Dt = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Dt != null && Dt.Tables[0].Rows.Count > 0)
            {
                double.TryParse(Dt.Tables[0].Rows[0]["MinMark"].ToString(), out  MinMark);
            }
            return MinMark;
        }

        public double GetMinMarkFromExamScheduleIdandSubjectName(int ExamSchduleId, string SubjectName)
        {
            double MinMark = 0;
            string sql = "select tblclassexamsubmap.MinMark from tblclassexamsubmap inner join tblexamschedule on  tblexamschedule.ClassExamId= tblclassexamsubmap.ClassExamId inner join tblsubjects on tblsubjects.Id = tblclassexamsubmap.SubId where tblsubjects.subject_name='" + SubjectName + "' and tblexamschedule.Id=" + ExamSchduleId + "";
            DataSet Dt = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Dt != null && Dt.Tables[0].Rows.Count > 0)
            {
                double.TryParse(Dt.Tables[0].Rows[0]["MinMark"].ToString(), out  MinMark);
            }
            return MinMark;
        }

        private string GetGradeFromAvg(DataSet Grade, double _Mark)
        {
            foreach (DataRow Dr in Grade.Tables[0].Rows)
            {
                if (_Mark >= double.Parse(Dr[1].ToString()))
                {
                    return Dr[0].ToString();
                }
            }
            return "";
        }


        private string GetGradeFromMarks(DataSet Grade, DataSet totalMarkRatios, string ExamName, double _Mark, double MaxMark, out double Ratio_Mark, out double Ratio_Max_Mark)
        {
            Ratio_Max_Mark = 0;
            Ratio_Mark = 0;
            double ratio = 0;
            if (ExamName != "")
            {
                if (totalMarkRatios.Tables[0].Rows.Count > 0)
                {
                    if (double.Parse(totalMarkRatios.Tables[0].Rows[0][ExamName].ToString()) < 1)
                        ratio = 1;
                    else
                        ratio = double.Parse(totalMarkRatios.Tables[0].Rows[0][ExamName].ToString());

                    _Mark = _Mark * ratio;
                    MaxMark = MaxMark * ratio;
                }
                Ratio_Max_Mark = MaxMark;
                Ratio_Mark = _Mark;
            }
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

        private DataSet GetGradeDataSet(int _GradeMasterId)
        {
            string _sql = "select tblgrade.Grade, tblgrade.LowerLimit from tblgrade where";
            if (_GradeMasterId > 0)
                _sql = _sql + " tblgrade.GradeMasterId=" + _GradeMasterId + " and ";
            _sql = _sql + "  tblgrade.`Status`=1   order by tblgrade.id asc";
            DataSet Dt = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
            return Dt;
        }

        #endregion

        #region Create and Open PDF File

        private void CreateReport(List<SupplimentaryReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, ExamNode[] CC_ExamDetails, ExamNode[] CC_SubDetails, SchoolDetails _SchoolDetails, int _StudId, DataSet totalMarkRatios, ExamNode[] _PracticalSubDetails)
        {
            string _PhysicalPath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));
            //int _StudentId = 0;
            Document document = LoadPDFPage(ClsObj, _ExamDetails, _SubDetails, _SchoolDetails, CC_ExamDetails, CC_SubDetails, totalMarkRatios, _PracticalSubDetails);

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

            filename = _PhysicalPath + "\\PDF_Files\\CBSC_Final_" + MainName + ".pdf";

            pdfRenderer.PdfDocument.Save(filename);
            // ...and start a viewer.
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "null", "window.open(\"OpenPdfPage.aspx?PdfName=CBSC_Final_" + MainName + ".pdf\",'Info','status=1, width=1000, height=700,,resizable=1,scrollbars=1');", true);
         
        }


        #endregion

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


        #region Read Base Details From DB

        private DataSet GetMarkRatios(string _selectedClassId)
        {
            string stdId = MyClassMngr.GetStandardIdfromClassId(_selectedClassId);
            string sqlRation = "select FA1,FA2,SA1,FA3,FA4,SA2 from tblcbsegraderatio where tblcbsegraderatio.StandardId=" + stdId;
            DataSet Dt = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sqlRation);
            return Dt;
        }

        private DataSet GetAllStudentListFromClass(int StudentId, int ClassId, ExamNode[] _exam)
        {
            int examSchdlId;
            int failsubconfig = Getfailsubcountconfig();
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
            for (int i = 0; i < _exam.Length; i++)
            {
                examSchdlId = _exam[i].Id;

                string sql_Student = "select tblview_student.Id, tblview_student.StudentName, tblview_student.RollNo, tblclass.ClassName,date_Format(tblview_student.DOB,'%d/%m/%Y') as DOB , tblview_student.AdmitionNo,tblview_student.GardianName,tblview_student.MothersName, tblview_student.ResidencePhNo ,tblview_student.Address from tblview_student inner join tblclass on tblclass.Id= tblview_student.ClassId inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblview_student.Id inner join tblstudentmark on tblstudentmark.StudId=tblview_student.Id where tblview_student.ClassId=" + ClassId + " and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " and tblstudentmark.FailSubCount>" + failsubconfig + " and tblstudentmark.ExamSchId =" + examSchdlId;
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
            }
            return _StdentSet;
        }
        private int Getfailsubcountconfig()
        {
            int failcount = 0;
            string sql = "SELECT Value FROM  tblconfiguration WHERE Name='Failed Subjects' AND Module = 'Exam Report'";
            OdbcDataReader MyReader = MyClassMngr.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                failcount = int.Parse(MyReader.GetValue(0).ToString());
            }
            return failcount;
        }


        private void GetSubDetails(int ClassId, int ExamId, out ExamNode[] _SubDetails)
        {            
            string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark ,tblsubjects.SubjectCode,tblexammark.SubjectOrder  from tblclassexamsubmap inner JOIN tblsubjects on tblclassexamsubmap.SubId= tblsubjects.Id inner JOIN tblclassexam on tblclassexam.id= tblclassexamsubmap.ClassExamId    inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.id  inner JOIN tblexammark on  tblexammark.SubjectId=tblsubjects.Id and tblexammark.ExamSchId =tblexamschedule.id   where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "  and tblclassexam.ClassId=" + ClassId + " and tblexamschedule.Id=" + ExamId + " order by tblexammark.SubjectOrder";            
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
                    _SubDetails[i].MaxMark = double.Parse(MyReader.GetValue(2).ToString());
                    _SubDetails[i].SubCode = MyReader.GetValue(3).ToString();
                    //_SubDetails[i].Desc = MyReader.GetValue(2).ToString();
                    i++;
                }
            }
            else
            {
                _SubDetails = new ExamNode[0];
            }

        }
        private void GetSubDetailsWithoutPractical(int ClassId, int ExamId, out ExamNode[] _SubDetailsWithoutPractical)
        {
            string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark ,tblsubjects.SubjectCode,tblexammark.SubjectOrder  from tblclassexamsubmap inner JOIN tblsubjects on tblclassexamsubmap.SubId= tblsubjects.Id inner JOIN tblclassexam on tblclassexam.id= tblclassexamsubmap.ClassExamId    inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.id  inner JOIN tblexammark on  tblexammark.SubjectId=tblsubjects.Id and tblexammark.ExamSchId =tblexamschedule.id   where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "  and tblclassexam.ClassId=" + ClassId + " and tblexamschedule.Id=" + ExamId + " and tblsubjects.sub_Catagory=1 order by tblexammark.SubjectOrder";
            OdbcDataReader MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int i = 0;
                int Count = MyReader.RecordsAffected;
                _SubDetailsWithoutPractical = new ExamNode[Count];

                while (MyReader.Read())
                {
                    _SubDetailsWithoutPractical[i].Id = int.Parse(MyReader.GetValue(0).ToString());
                    _SubDetailsWithoutPractical[i].Name = MyReader.GetValue(1).ToString();
                    _SubDetailsWithoutPractical[i].MaxMark = double.Parse(MyReader.GetValue(2).ToString());
                    _SubDetailsWithoutPractical[i].SubCode = MyReader.GetValue(3).ToString();
                    //_SubDetails[i].Desc = MyReader.GetValue(2).ToString();
                    i++;
                }
            }
            else
            {
                _SubDetailsWithoutPractical = new ExamNode[0];
            }

        }


        private void GetPracticalSubDetails(int ClassId, int ExamId, out ExamNode[] _PracticalSubDetails)
        {
            string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark ,tblsubjects.SubjectCode,tblexammark.SubjectOrder  from tblclassexamsubmap inner JOIN tblsubjects on tblclassexamsubmap.SubId= tblsubjects.Id inner JOIN tblclassexam on tblclassexam.id= tblclassexamsubmap.ClassExamId    inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.id  inner JOIN tblexammark on  tblexammark.SubjectId=tblsubjects.Id and tblexammark.ExamSchId =tblexamschedule.id   where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "  and tblclassexam.ClassId=" + ClassId + " and tblexamschedule.Id=" + ExamId + " and tblsubjects.sub_Catagory=2 order by tblexammark.SubjectOrder";
            OdbcDataReader MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int i = 0;
                int Count = MyReader.RecordsAffected;
                _PracticalSubDetails = new ExamNode[Count];

                while (MyReader.Read())
                {
                    _PracticalSubDetails[i].Id = int.Parse(MyReader.GetValue(0).ToString());
                    _PracticalSubDetails[i].Name = MyReader.GetValue(1).ToString();
                    _PracticalSubDetails[i].MaxMark = double.Parse(MyReader.GetValue(2).ToString());
                    _PracticalSubDetails[i].SubCode = MyReader.GetValue(3).ToString();
                    //_SubDetails[i].Desc = MyReader.GetValue(2).ToString();
                    i++;
                }
            }
            else
            {
                _PracticalSubDetails = new ExamNode[0];
            }

        }

        private void GetCC_SubDetails(int ClassId, string ExamType, out ExamNode[] CC_SubDetails)
        {           
            string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblsubjects.sub_description , tblsubjects.sub_Catagory, tbltime_subgroup.Name from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id  inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId   inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId   inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=tblexamschedule.ClassExamId   and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId   inner join tbltime_subgroup on tbltime_subgroup.Id= tblsubjects.sub_Catagory where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "   and tblexammaster.Id     in (select tblexammaster.id from tblsubject_type  inner join tblexammaster on tblexammaster.ExamTypeId=tblsubject_type.Id  inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id   inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id    where tblclassexam.ClassId=" + ClassId + "  and   tblsubject_type.TypeDisc='" + ExamType + "' and tblexamschedule.Status='Completed'   order by tblexammark.SubjectOrder  )";
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
                    CC_SubDetails[i].Desc = MyReader.GetValue(2).ToString();
                    CC_SubDetails[i].Group = int.Parse(MyReader.GetValue(3).ToString());
                    CC_SubDetails[i].GroupName = MyReader.GetValue(4).ToString();
                    i++;
                }
            }
            else
            {
                CC_SubDetails = new ExamNode[0];
            }

        }

        private void GetExamDetails(int ClassId, int ExamId, out ExamNode[] _ExamDetails)
        {
            string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period, tblexamschedule.GradeMasterId from tblexammaster inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id     where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblexammaster.Id=" + ExamId + " and tblexamschedule.Status='Completed'  order by tblexamschedule.id ASC";
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

        public static string ConvertNumbertoWords(int number)
        {
            if (number == 0)
                return "ZERO";
            if (number < 0)
                return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";
            if ((number / 1000000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000000) + " MILLION ";
                number %= 1000000;
            }
            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                number %= 100;
            }
            if (number > 0)
            {
                if (words != "")
                    words += "AND ";
                var unitsMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
                var tensMap = new[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }
            return words;
        }
        #endregion

        #region  Read User Entries

        private bool GatBaseValuese(out int classId, out int studentId)
        {
            classId = 0;
            studentId = 0;
            if (ValidEntries())
            {
                int.TryParse(Drp_SelectClass.SelectedValue, out classId);
                int.TryParse(Drp_SelectStudent.SelectedValue, out studentId);
                return true;
            }
            else
                return false;
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
            else if (Drp_SelectStudent.SelectedValue == "-1")
            {
                Lbl_Err.Text = "No students exist in the class.";
                return false;
            }
            return true;

        }

        #endregion

        #endregion

        #region LoadMethos

        private void LoadDetails()
        {
            LoadClassDetailsToDropDown();
           // LoadStudentsDetailsToDropDown();
            LoadExamType();
        }

        
        private void LoadStudentsDetailsToDropDown(int schdlId)
        {
            if (Drp_SelectClass.SelectedValue != "-1")
            {
                int ClassId = 0;
                int.TryParse(Drp_SelectClass.SelectedValue.ToString(), out ClassId);
                int failsubconfig = Getfailsubcountconfig();
                Drp_SelectStudent.Items.Clear();
                string Sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudentclassmap.RollNo,tblstudentmark.Failsubcount from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id INNER JOIN tblstudentmark on tblstudentmark.StudId=tblstudent.Id WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND tblstudentclassmap.ClassId=" + Drp_SelectClass.SelectedValue + " AND tblstudentmark.ExamSchId =" + schdlId + " AND tblstudentmark.FailSubCount>" + failsubconfig + "  Order by tblstudentclassmap.RollNo ASC";
                OdbcDataReader MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(Sql);

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

        protected void Drp_ExamType_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadExam();
        }

        protected void Drp_Exam_SelectedIndexChanged(object sender,EventArgs e)
        {
            loadfailstud();
            //LoadStudentsDetailsToDropDown();
        }

        private void LoadExamType()
        {
            Drp_ExamType.Items.Clear();
            string sql = "select tblsubject_type.Id, tblsubject_type.sbject_type from tblsubject_type";
            OdbcDataReader MyReader = MyClassMngr.m_MysqlDb.ExecuteQuery(sql);
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
            string sql = "select distinct tblexammaster.Id, tblexammaster.ExamName,tblexamschedule.Id as ExamScdlId from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster ON tblexammaster.Id= tblclassexam.ExamId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id  where tblclassexam.ClassId = " + int.Parse(Drp_SelectClass.SelectedValue) + " and tblexamschedule.BatchId = " + MyUser.CurrentBatchId + " and tblexammaster.ExamTypeId= " + int.Parse(Drp_ExamType.SelectedValue) + " and tblexamschedule.status ='Completed'";
            OdbcDataReader MyReader = MyClassMngr.m_MysqlDb.ExecuteQuery(sql);
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

        private void loadfailstud()
        {
            int schdlId=0;
            string sql = "select distinct tblexammaster.Id, tblexammaster.ExamName,tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster ON tblexammaster.Id= tblclassexam.ExamId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id  where tblclassexam.ClassId = " + int.Parse(Drp_SelectClass.SelectedValue) + " and tblexamschedule.BatchId = " + MyUser.CurrentBatchId + " and tblexammaster.ExamTypeId= " + int.Parse(Drp_ExamType.SelectedValue) + " and tblexamschedule.status ='Completed' and tblexammaster.Id=" + int.Parse(Drp_Exam.SelectedValue);
            OdbcDataReader MyReader = MyClassMngr.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_SelectStudent.Items.Clear();
                while (MyReader.Read())
                {
                    schdlId = int.Parse(MyReader.GetValue(2).ToString());
                    LoadStudentsDetailsToDropDown(schdlId);
                }
            }
            else
            {
                Drp_SelectStudent.Items.Add(new ListItem("No Student Found", "-1"));
            }          
        }

        private void LoadClassDetailsToDropDown()
        {
            Drp_SelectClass.Items.Clear();
            string sql = "SELECT tblclass.Id,tblclass.ClassName FROM tblclass where tblclass.Status =1 AND tblclass.Id IN (SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ")) ORDER BY tblclass.Standard,tblclass.ClassName";
            OdbcDataReader MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
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
        private bool ValidData(out string _msg)
        {
            bool _Valid = true;
            _msg = "";

            if (Drp_SelectClass.SelectedValue == "-1")
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
            else if (Drp_SelectStudent.SelectedValue == "-1")
            {
                _msg = "Student does not found ";
                _Valid = false;
            }
            else if (Drp_Exam.SelectedValue != "-1" && Drp_SelectStudent.SelectedValue != "-1" && Drp_SelectStudent.SelectedValue != "0")
            {
                string sql = "select tblstudentmark.StudId from tblstudentmark inner join tblexamschedule on tblexamschedule.Id = tblstudentmark.ExamSchId inner join  tblclassexam on tblclassexam.Id=  tblexamschedule.ClassExamId where tblclassexam.ClassId=" + Drp_SelectClass.SelectedValue + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblclassexam.ExamId= " + Drp_Exam.SelectedValue + " and tblexamschedule.Status='Completed' and tblstudentmark.StudId=" + Drp_SelectStudent.SelectedValue;
                OdbcDataReader MyReader = MyClassMngr.m_MysqlDb.ExecuteQuery(sql);
                if (!MyReader.HasRows)
                {
                    _msg = "Selected student does not attend this exam. Please select another studnet.";
                    _Valid = false;
                }
            }



            return _Valid;
        }

        #endregion
    }
}
