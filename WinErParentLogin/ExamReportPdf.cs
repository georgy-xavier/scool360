using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using System.Diagnostics;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Web.Security;
using WinBase;
using WinEr;
namespace WinErParentLogin
{
    public class ExamReportPdf
    {
        private Font[] MyFonts = new Font[24];
        public MysqlClass m_MysqlDb;
        private SchoolClass m_objSchool = null;
        private OdbcDataReader m_MyReader = null;
        private OdbcDataReader m_MyReader1 = null;
        private OdbcDataReader m_MyReader2 = null;
        private OdbcDataReader m_MyReader3 = null;
        private OdbcDataReader m_MyReader4 = null;
        private OdbcDataReader m_MyReader5 = null;
        private OdbcDataReader m_MyReader6 = null;
        private OdbcDataReader m_MyReader7 = null;
        private OdbcDataReader MyReader1 = null;
        private OdbcDataReader MyReader2 = null;
        private OdbcDataReader MyReader3 = null;
        private OdbcDataReader MyReader4 = null;
        private OdbcDataReader MyReader5 = null;
        private OdbcDataReader MyReader6 = null;


        //private DataSet m_MyDataSet = null;

        private string sql = "";
        //private string m_NullStr = "NULL";
        //private string _YourRef = "Asper your enquiry";
        //private string m_AmtType = "RS";
        //private string m_PdfType = "";

        private Attendance MyAttendence;
        private ExamManage MyExamMang;


        public ExamReportPdf(MysqlClass _Mysqlobj, SchoolClass objSchool)
        {
            m_objSchool = objSchool;
            m_MysqlDb = _Mysqlobj;
            LoadMyFont();
            MyAttendence = new Attendance(_Mysqlobj);
            MyExamMang = new ExamManage(_Mysqlobj);
           
        }

        ~ExamReportPdf()
        {
            if (m_MysqlDb != null)
            {
                m_MysqlDb = null;
            }
            if (m_MyReader != null)
            {
                m_MyReader = null;

            }
            if (m_MyReader1 != null)
            {
                m_MyReader1 = null;

            }
            if (m_MyReader2 != null)
            {
                m_MyReader2 = null;

            }
            if (m_MyReader3 != null)
            {
                m_MyReader3 = null;

            }
            if (m_MyReader4 != null)
            {
                m_MyReader4 = null;

            }
            if (m_MyReader5 != null)
            {
                m_MyReader5 = null;

            }
            if (m_MyReader6 != null)
            {
                m_MyReader6 = null;

            }
            if (m_MyReader7 != null)
            {
                m_MyReader7 = null;

            }
            if (MyReader1 != null)
            {
                MyReader1 = null;

            }
            if (MyReader2 != null)
            {
                MyReader2 = null;

            }
            if (MyReader3 != null)
            {
                MyReader3 = null;

            }
            if (MyReader4 != null)
            {
                MyReader4 = null;

            }
            if (MyReader5 != null)
            {
                MyReader5 = null;

            }
            if (MyReader6 != null)
            {
                MyReader6 = null;

            }


        }

        #region EXAM REPORT

        internal bool CreateExamReportPdf(int _ClassId, int _ExamId, int _CurrentBatchId, string _BatchName, string _physicalpath, out string _PdfName, out string _ErrorMsg)
        {
            _PdfName = "";
            _ErrorMsg = "";
            int _EmptyRowCount = 0;

            try
            {
                string _ClassName = "";
                sql = "SELECT tblclass.ClassName from tblclass where tblclass.Id=" + _ClassId;
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _ClassName = m_MyReader.GetValue(0).ToString();
                }
                _PdfName = "ER_" + _ClassName + ".pdf";


                if (Delete_Existing_ExamReport(_PdfName, _physicalpath, out _ErrorMsg))
                {
                    Document DocER = new Document(PageSize.A4, 50, 40, 20, 10);
                    PdfWriter writerER = PdfWriter.GetInstance(DocER, new FileStream(_physicalpath + "\\PDF_Files\\ER_" + _ClassName + ".pdf", FileMode.Create));

                    Document DocERTemp = new Document(PageSize.A4, 50, 40, 20, 10);
                    PdfWriter writerERTemp = PdfWriter.GetInstance(DocERTemp, new FileStream(_physicalpath + "\\PDF_Files\\ERTemp_" + _ClassName + ".pdf", FileMode.Create));

                    DocERTemp.Open();
                    DocER.Open();

                    string sql1 = "SELECT tblstudentclassmap.StudentId, tblstudentclassmap.RollNo, tblstudent.StudentName,tblstudent.AdmitionNo from tblstudentclassmap inner JOIN tblstudent on tblstudentclassmap.StudentId= tblstudent.Id  where tblstudentclassmap.ClassId=" + _ClassId;
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql1);
                    if (m_MyReader.HasRows)
                    {
                        while (m_MyReader.Read())
                        {
                            int _StudentId = int.Parse(m_MyReader.GetValue(0).ToString());
                            int _UniversityNo = 0;
                            int _RollNo = int.Parse(m_MyReader.GetValue(1).ToString());
                            string _StudentName = m_MyReader.GetValue(2).ToString();
                            string _AdmissionNo = m_MyReader.GetValue(3).ToString();// _Batch = m_MyReader.GetValue(4).ToString();
                            iTextSharp.text.Table Tbl_CollageInformation = ER_LoadCollageDetilsToTable(_BatchName, _physicalpath);
                            iTextSharp.text.Table Tbl_StudentInformation = ER_LoadStudentDetailsToTable(_StudentId, _StudentName, _AdmissionNo, _BatchName, _ClassName, _ClassId, _RollNo, _UniversityNo, _ExamId);

                            iTextSharp.text.Table Tbl_SubjectWiseTotalandMaxMarks = ER_LoadSubWiseTotalandMaxMarkDetails(_ClassId, _CurrentBatchId, _ExamId, _StudentId);
                            iTextSharp.text.Table Tbl_TotalandMaxMark = ER_LoadTotalandMaxMarkDetailsToTable(_ClassId, _CurrentBatchId, _ExamId, _StudentId);
                            iTextSharp.text.Table Tbl_SignAreaDetails = ER_SignAreaToTable();

                            DocERTemp.Add(Tbl_CollageInformation);
                            DocERTemp.Add(Tbl_StudentInformation);
                            DocERTemp.Add(Tbl_SubjectWiseTotalandMaxMarks);
                            DocERTemp.Add(Tbl_TotalandMaxMark);
                            DocERTemp.Add(Tbl_SignAreaDetails);
                            DocERTemp = ERTemp_LoadExamReportDetails(DocERTemp, writerERTemp, _ClassId, _CurrentBatchId, _ExamId, _StudentId, out _EmptyRowCount);

                            DocERTemp.NewPage();


                            DocER.Add(Tbl_CollageInformation);
                            DocER.Add(Tbl_StudentInformation);
                            //iTextSharp.text.Table Tbl_ExamReportDetails = ER_LoadExamReportDetails(_ClassId, _CurrentBatchId, _ExamId, _StudentId, _EmptyRowCount);
                            //DocER.Add(Tbl_ExamReportDetails);
                            DocER = ERDoc_LoadExamReportDetails(DocER, writerER, _ClassId, _CurrentBatchId, _ExamId, _StudentId, _EmptyRowCount);
                            DocER.Add(Tbl_SubjectWiseTotalandMaxMarks);
                            DocER.Add(Tbl_TotalandMaxMark);
                            DocER.Add(Tbl_SignAreaDetails);

                            DocER.NewPage();
                        }
                    }
                    DocER.Close();

                    DocERTemp.Close();
                    File.Delete(_physicalpath + "\\PDF_Files\\ERTemp_" + _ClassName + ".pdf");

                    sql = "Delete from tblexamreport ";
                    m_MysqlDb.ExecuteQuery(sql);
                    sql = "INSERT into tblexamreport(tblexamreport.ReportName) VALUES ('" + _PdfName + "')";
                    m_MysqlDb.ExecuteQuery(sql);

                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception e)
            {
                if (e.Message == "The process cannot access the file")
                    _ErrorMsg = "This Bill is used by another person.Try again later";
                return false;
            }

        }

        private Document ERDoc_LoadExamReportDetails(Document DocER, PdfWriter writerER, int _ClassId, int _CurrentBatchId, int _ExamId, int _StudentId, int _EmptyRowCount)
        {
            int _SubjectCount = 0;

            string sql2 = "select DISTINCT tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblclassexam.ExamId = " + _ExamId + " and tblexamschedule.BatchId =" + _CurrentBatchId + " and tblstudentclassmap.ClassId=" + _ClassId;
            m_MyReader2 = m_MysqlDb.ExecuteQuery(sql2);
            int _ColumCount = m_MyReader2.RecordsAffected + 2;
            iTextSharp.text.Table Tbl_ExamReportDetails = new iTextSharp.text.Table(_ColumCount);
            Tbl_ExamReportDetails.Width = 100;
            Tbl_ExamReportDetails.Padding = 1;
            Tbl_ExamReportDetails.DefaultCell.Border = 0;
            Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
            Tbl_ExamReportDetails.DefaultCell.BorderWidthRight = 1;
            Tbl_ExamReportDetails.DefaultCell.BorderWidthBottom = 1;
            Tbl_ExamReportDetails.AddCell(new Phrase("Subject", MyFonts[4])); // Sub Name

            if (m_MyReader2.HasRows)
            {
                while (m_MyReader2.Read())
                {
                    Tbl_ExamReportDetails.AddCell(new Phrase(m_MyReader2.GetValue(1).ToString(), MyFonts[4]));

                }
            }
            Tbl_ExamReportDetails.AddCell(new Phrase("Total", MyFonts[4])); // Sub Name


            string sql = "select distinct tblsubjects.subject_name, tblexammark.SubjectId, tblexammark.MarkColumn from tblexamschedule inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblexamschedule.ClassExamId inner JOIN tblexammark on tblexammark.ExamSchId= tblexamschedule.Id AND tblclassexamsubmap.SubId= tblexammark.SubjectId inner join tblsubjects on tblsubjects.Id= tblexammark.SubjectId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId and tblstudentclassmap.ClassId=" + _ClassId + "  and tblstudentclassmap.BatchId =" + _CurrentBatchId + " where tblclassexam.ExamId=" + _ExamId + " and tblexamschedule.BatchId=" + _CurrentBatchId + " order by tblexammark.SubjectOrder";
            m_MyReader3 = m_MysqlDb.ExecuteQuery(sql);
            _SubjectCount = m_MyReader3.RecordsAffected;
            if (m_MyReader3.HasRows)
            {
                while (m_MyReader3.Read())
                {
                    Tbl_ExamReportDetails.DefaultCell.BorderWidthBottom = 0;
                    Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 0;
                    Tbl_ExamReportDetails.DefaultCell.BorderWidthRight = 1;

                    double _SubjectTotalMark = 0;

                    Tbl_ExamReportDetails.AddCell(new Phrase(" " + m_MyReader3.GetValue(0).ToString(), MyFonts[3]));
                    sql2 = "select DISTINCT tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblclassexam.ExamId = " + _ExamId + " and tblexamschedule.BatchId =" + _CurrentBatchId + " and tblstudentclassmap.ClassId=" + _ClassId;
                    m_MyReader2 = m_MysqlDb.ExecuteQuery(sql2);
                    if (m_MyReader2.HasRows)
                    {
                        while (m_MyReader2.Read())
                        {
                            int _PeriodId = int.Parse(m_MyReader2.GetValue(0).ToString());
                            int _examschid = GetExamSchid(_ClassId, _ExamId, _CurrentBatchId, _PeriodId);
                            double _ExamMark = 0;
                            double.TryParse(GetMarkfromColumn(_StudentId, _examschid, m_MyReader3.GetValue(2).ToString(), _PeriodId), out _ExamMark);
                            if (_ExamMark == -1)
                            {
                                //_ExamMark = "A";
                                _SubjectTotalMark = _SubjectTotalMark + 0;
                            }
                            else
                            {
                                _SubjectTotalMark = _SubjectTotalMark + _ExamMark;
                            }
                            if (_ExamMark == -1)
                            {
                                Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                                Tbl_ExamReportDetails.AddCell(new Phrase("A", MyFonts[3]));
                            }
                            else
                            {
                                Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                                Tbl_ExamReportDetails.AddCell(new Phrase(Math.Round(_ExamMark, 2).ToString(), MyFonts[3]));
                            }
                        }
                        Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_ExamReportDetails.AddCell(new Phrase(Math.Round(_SubjectTotalMark, 2).ToString(), MyFonts[3]));
                        Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 0;
                    }

                    if (writerER.FitsPage(Tbl_ExamReportDetails))
                    {
                    }
                    else
                    {
                        Tbl_ExamReportDetails.DeleteLastRow();
                        DocER.Add(Tbl_ExamReportDetails);

                        Tbl_ExamReportDetails.DeleteAllRows();
                        sql2 = "select DISTINCT tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblclassexam.ExamId = " + _ExamId + " and tblexamschedule.BatchId =" + _CurrentBatchId + " and tblstudentclassmap.ClassId=" + _ClassId;
                        m_MyReader2 = m_MysqlDb.ExecuteQuery(sql2);

                        Tbl_ExamReportDetails.DefaultCell.BorderWidthBottom = 1;
                        Tbl_ExamReportDetails.DefaultCell.BorderWidthTop = 1;
                        Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_ExamReportDetails.AddCell(new Phrase("Subject", MyFonts[4])); // Sub Name
                        DocER.NewPage();

                        if (m_MyReader2.HasRows)
                        {
                            while (m_MyReader2.Read())
                            {
                                Tbl_ExamReportDetails.AddCell(new Phrase(m_MyReader2.GetValue(1).ToString(), MyFonts[4]));

                            }
                        }
                        Tbl_ExamReportDetails.AddCell(new Phrase("Total", MyFonts[4])); // Sub Name

                        Tbl_ExamReportDetails.DefaultCell.BorderWidthBottom = 0;
                        Tbl_ExamReportDetails.DefaultCell.BorderWidthTop = 0;
                        Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 0;

                        Tbl_ExamReportDetails.AddCell(new Phrase(m_MyReader3.GetValue(0).ToString(), MyFonts[3]));
                        sql2 = "select DISTINCT tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblclassexam.ExamId = " + _ExamId + " and tblexamschedule.BatchId =" + _CurrentBatchId + " and tblstudentclassmap.ClassId=" + _ClassId;
                        m_MyReader2 = m_MysqlDb.ExecuteQuery(sql2);
                        if (m_MyReader2.HasRows)
                        {
                            while (m_MyReader2.Read())
                            {
                                int _PeriodId = int.Parse(m_MyReader2.GetValue(0).ToString());
                                int _examschid = GetExamSchid(_ClassId, _ExamId, _CurrentBatchId, _PeriodId);
                                double _ExamMark = 0;
                                double.TryParse(GetMarkfromColumn(_StudentId, _examschid, m_MyReader3.GetValue(2).ToString(), _PeriodId), out _ExamMark);
                                if (_ExamMark == -1)
                                {
                                    //_ExamMark = "A";
                                    _SubjectTotalMark = _SubjectTotalMark + 0;
                                }
                                else
                                {
                                    _SubjectTotalMark = _SubjectTotalMark + _ExamMark;
                                }
                                if (_ExamMark == -1)
                                {
                                    Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                                    Tbl_ExamReportDetails.AddCell(new Phrase("A", MyFonts[3]));
                                }
                                else
                                {
                                    Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                                    Tbl_ExamReportDetails.AddCell(new Phrase(Math.Round(_ExamMark, 2).ToString(), MyFonts[3]));
                                }
                            }
                            Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                            Tbl_ExamReportDetails.AddCell(new Phrase(Math.Round(_SubjectTotalMark, 2).ToString(), MyFonts[3]));
                            Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 0;
                        }
                    }
                }
            }

            for (int k = 0; k < _EmptyRowCount - 2; k++)
            {
                for (int a = 0; a < _ColumCount; a++)
                {
                    Tbl_ExamReportDetails.AddCell(new Phrase("\n", MyFonts[3]));
                }
            }
            DocER.Add(Tbl_ExamReportDetails);
            return DocER;
        }

        private Document ERTemp_LoadExamReportDetails(Document DocERTemp, PdfWriter writerERTemp, int _ClassId, int _CurrentBatchId, int _ExamId, int _StudentId, out int _EmptyRowCount)
        {
            int _SubjectCount = 0;

            string sql2 = "select DISTINCT tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblclassexam.ExamId = " + _ExamId + " and tblexamschedule.BatchId =" + _CurrentBatchId + " and tblstudentclassmap.ClassId=" + _ClassId;
            m_MyReader2 = m_MysqlDb.ExecuteQuery(sql2);
            int _ColumCount = m_MyReader2.RecordsAffected + 2;
            iTextSharp.text.Table Tbl_ExamReportDetails = new iTextSharp.text.Table(_ColumCount);
            Tbl_ExamReportDetails.Width = 100;
            Tbl_ExamReportDetails.Padding = 1;
            Tbl_ExamReportDetails.DefaultCell.Border = 0;
            Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
            Tbl_ExamReportDetails.DefaultCell.BorderWidthRight = 1;
            Tbl_ExamReportDetails.DefaultCell.BorderWidthBottom = 1;
            Tbl_ExamReportDetails.AddCell(new Phrase("Subject", MyFonts[4])); // Sub Name

            if (m_MyReader2.HasRows)
            {
                while (m_MyReader2.Read())
                {
                    Tbl_ExamReportDetails.AddCell(new Phrase(m_MyReader2.GetValue(1).ToString(), MyFonts[4]));

                }
            }
            Tbl_ExamReportDetails.AddCell(new Phrase("Total", MyFonts[4])); // Sub Name


            string sql = "select distinct tblsubjects.subject_name, tblexammark.SubjectId, tblexammark.MarkColumn from tblexamschedule inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblexamschedule.ClassExamId inner JOIN tblexammark on tblexammark.ExamSchId= tblexamschedule.Id AND tblclassexamsubmap.SubId= tblexammark.SubjectId inner join tblsubjects on tblsubjects.Id= tblexammark.SubjectId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId and tblstudentclassmap.ClassId=" + _ClassId + "  and tblstudentclassmap.BatchId =" + _CurrentBatchId + " where tblclassexam.ExamId=" + _ExamId + " and tblexamschedule.BatchId=" + _CurrentBatchId + " order by tblexammark.SubjectOrder";
            m_MyReader3 = m_MysqlDb.ExecuteQuery(sql);
            _SubjectCount = m_MyReader3.RecordsAffected;
            if (m_MyReader3.HasRows)
            {
                while (m_MyReader3.Read())
                {
                    Tbl_ExamReportDetails.DefaultCell.BorderWidthBottom = 0;
                    Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 0;
                    Tbl_ExamReportDetails.DefaultCell.BorderWidthRight = 1;

                    double _SubjectTotalMark = 0;

                    Tbl_ExamReportDetails.AddCell(new Phrase(" " + m_MyReader3.GetValue(0).ToString(), MyFonts[3]));
                    sql2 = "select DISTINCT tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblclassexam.ExamId = " + _ExamId + " and tblexamschedule.BatchId =" + _CurrentBatchId + " and tblstudentclassmap.ClassId=" + _ClassId;
                    m_MyReader2 = m_MysqlDb.ExecuteQuery(sql2);
                    if (m_MyReader2.HasRows)
                    {
                        while (m_MyReader2.Read())
                        {
                            int _PeriodId = int.Parse(m_MyReader2.GetValue(0).ToString());
                            int _examschid = GetExamSchid(_ClassId, _ExamId, _CurrentBatchId, _PeriodId);
                            string _ExamMark = GetMarkfromColumn(_StudentId, _examschid, m_MyReader3.GetValue(2).ToString(), _PeriodId);
                            if (_ExamMark == "-1")
                            {
                                _ExamMark = "A";
                                _SubjectTotalMark = _SubjectTotalMark + 0;
                            }
                            else
                            {
                                _SubjectTotalMark = _SubjectTotalMark + double.Parse(_ExamMark);
                            }

                            Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                            Tbl_ExamReportDetails.AddCell(new Phrase(_ExamMark, MyFonts[3]));
                        }
                        Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_ExamReportDetails.AddCell(new Phrase(_SubjectTotalMark.ToString(), MyFonts[3]));
                        Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 0;
                    }
                    if (writerERTemp.FitsPage(Tbl_ExamReportDetails))
                    {
                    }

                    else
                    {
                        Tbl_ExamReportDetails.DeleteLastRow();
                        DocERTemp.Add(Tbl_ExamReportDetails);

                        Tbl_ExamReportDetails.DeleteAllRows();
                        sql2 = "select DISTINCT tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblclassexam.ExamId = " + _ExamId + " and tblexamschedule.BatchId =" + _CurrentBatchId + " and tblstudentclassmap.ClassId=" + _ClassId;
                        m_MyReader2 = m_MysqlDb.ExecuteQuery(sql2);

                        Tbl_ExamReportDetails.DefaultCell.BorderWidthBottom = 1;
                        Tbl_ExamReportDetails.DefaultCell.BorderWidthTop = 1;
                        Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_ExamReportDetails.AddCell(new Phrase("Subject", MyFonts[4])); // Sub Name
                        DocERTemp.NewPage();

                        if (m_MyReader2.HasRows)
                        {
                            while (m_MyReader2.Read())
                            {
                                Tbl_ExamReportDetails.AddCell(new Phrase(m_MyReader2.GetValue(1).ToString(), MyFonts[4]));

                            }
                        }
                        Tbl_ExamReportDetails.AddCell(new Phrase("Total", MyFonts[4])); // Sub Name

                        Tbl_ExamReportDetails.DefaultCell.BorderWidthBottom = 0;
                        Tbl_ExamReportDetails.DefaultCell.BorderWidthTop = 0;

                        Tbl_ExamReportDetails.AddCell(new Phrase(m_MyReader3.GetValue(0).ToString(), MyFonts[3]));
                        sql2 = "select DISTINCT tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblclassexam.ExamId = " + _ExamId + " and tblexamschedule.BatchId =" + _CurrentBatchId + " and tblstudentclassmap.ClassId=" + _ClassId;
                        m_MyReader2 = m_MysqlDb.ExecuteQuery(sql2);
                        if (m_MyReader2.HasRows)
                        {
                            while (m_MyReader2.Read())
                            {
                                int _PeriodId = int.Parse(m_MyReader2.GetValue(0).ToString());
                                int _examschid = GetExamSchid(_ClassId, _ExamId, _CurrentBatchId, _PeriodId);
                                string _ExamMark = GetMarkfromColumn(_StudentId, _examschid, m_MyReader3.GetValue(2).ToString(), _PeriodId);
                                _SubjectTotalMark = _SubjectTotalMark + double.Parse(_ExamMark);
                                Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                                Tbl_ExamReportDetails.AddCell(new Phrase(_ExamMark, MyFonts[3]));
                            }
                            Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                            Tbl_ExamReportDetails.AddCell(new Phrase(_SubjectTotalMark.ToString(), MyFonts[3]));
                            Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 0;
                        }
                    }
                }
            }

            _EmptyRowCount = 0;
            while (writerERTemp.FitsPage(Tbl_ExamReportDetails))
            {
                for (int a = 0; a < _ColumCount; a++)
                {
                    Tbl_ExamReportDetails.AddCell(new Phrase("\n", MyFonts[3]));
                }

                _EmptyRowCount++;
            }
            DocERTemp.Add(Tbl_ExamReportDetails);
            return DocERTemp;
        }

        private iTextSharp.text.Table ER_LoadSubWiseTotalandMaxMarkDetails(int _ClassId, int _CurrentBatchId, int _ExamId, int _StudentId)
        {
            string sql2 = "select DISTINCT tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblclassexam.ExamId = " + _ExamId + " and tblexamschedule.BatchId =" + _CurrentBatchId + " and tblstudentclassmap.ClassId=" + _ClassId;
            MyReader5 = m_MysqlDb.ExecuteQuery(sql2);
            int _ColumCount = MyReader5.RecordsAffected + 2;
            iTextSharp.text.Table Tbl_ExamReportSubjectwiseDetails = new iTextSharp.text.Table(_ColumCount);
            Tbl_ExamReportSubjectwiseDetails.Width = 100;
            Tbl_ExamReportSubjectwiseDetails.Padding = 1;
            Tbl_ExamReportSubjectwiseDetails.DefaultCell.Border = 0;
            Tbl_ExamReportSubjectwiseDetails.DefaultCell.BorderWidthRight = 1;
            Tbl_ExamReportSubjectwiseDetails.DefaultCell.BorderWidthBottom = 1;

            string sql = "select distinct tblsubjects.subject_name, tblexammark.SubjectId, tblexammark.MarkColumn from tblexamschedule inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblexamschedule.ClassExamId inner JOIN tblexammark on tblexammark.ExamSchId= tblexamschedule.Id AND tblclassexamsubmap.SubId= tblexammark.SubjectId inner join tblsubjects on tblsubjects.Id= tblexammark.SubjectId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId and tblstudentclassmap.ClassId=" + _ClassId + "  and tblstudentclassmap.BatchId =" + _CurrentBatchId + " where tblclassexam.ExamId=" + _ExamId + " and tblexamschedule.BatchId=" + _CurrentBatchId + " order by tblexammark.SubjectOrder";
            MyReader6 = m_MysqlDb.ExecuteQuery(sql);
            if (MyReader6.HasRows)
            {

                //Total Mark for Each Schedule

                sql2 = "select DISTINCT tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblclassexam.ExamId = " + _ExamId + " and tblexamschedule.BatchId =" + _CurrentBatchId + " and tblstudentclassmap.ClassId=" + _ClassId;
                MyReader4 = m_MysqlDb.ExecuteQuery(sql2);
                Tbl_ExamReportSubjectwiseDetails.DefaultCell.BorderWidthTop = 1;

                if (MyReader4.HasRows)
                {
                    Tbl_ExamReportSubjectwiseDetails.DefaultCell.HorizontalAlignment = 0;
                    Tbl_ExamReportSubjectwiseDetails.AddCell(new Phrase(" Total Mark", MyFonts[4]));
                    while (MyReader4.Read())
                    {
                        int _PeriodId = int.Parse(MyReader4.GetValue(0).ToString());
                        int _examschid = GetExamSchid(_ClassId, _ExamId, _CurrentBatchId, _PeriodId);
                        double _TotalMark = 0;
                        double.TryParse(GetTotalMarkForaPeriod(_examschid, _StudentId), out _TotalMark);
                        Tbl_ExamReportSubjectwiseDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_ExamReportSubjectwiseDetails.AddCell(new Phrase(Math.Round(_TotalMark, 2).ToString(), MyFonts[4]));

                    }
                    Tbl_ExamReportSubjectwiseDetails.AddCell(new Phrase("\n", MyFonts[4]));
                    Tbl_ExamReportSubjectwiseDetails.DefaultCell.HorizontalAlignment = 0;
                }

                //Avg Mark for Each Schedule

                sql2 = "select DISTINCT tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblclassexam.ExamId = " + _ExamId + " and tblexamschedule.BatchId =" + _CurrentBatchId + " and tblstudentclassmap.ClassId=" + _ClassId;
                MyReader4 = m_MysqlDb.ExecuteQuery(sql2);
                Tbl_ExamReportSubjectwiseDetails.DefaultCell.BorderWidthTop = 1;

                if (MyReader4.HasRows)
                {
                    Tbl_ExamReportSubjectwiseDetails.DefaultCell.HorizontalAlignment = 0;
                    Tbl_ExamReportSubjectwiseDetails.AddCell(new Phrase(" Average Mark", MyFonts[4]));
                    while (MyReader4.Read())
                    {
                        int _PeriodId = int.Parse(MyReader4.GetValue(0).ToString());
                        int _examschid = GetExamSchid(_ClassId, _ExamId, _CurrentBatchId, _PeriodId);
                        double _AverageMark = 0;
                        double.TryParse(GetAverageMarkForaPeriod(_examschid, _StudentId), out _AverageMark);
                        Tbl_ExamReportSubjectwiseDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_ExamReportSubjectwiseDetails.AddCell(new Phrase(Math.Round(_AverageMark, 2) + " %", MyFonts[4]));

                    }
                    Tbl_ExamReportSubjectwiseDetails.AddCell(new Phrase("\n", MyFonts[4]));
                    Tbl_ExamReportSubjectwiseDetails.DefaultCell.HorizontalAlignment = 0;
                }

                //Maximum Mark for Each Schedule

                sql2 = "select DISTINCT tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblclassexam.ExamId = " + _ExamId + " and tblexamschedule.BatchId =" + _CurrentBatchId + " and tblstudentclassmap.ClassId=" + _ClassId;
                MyReader4 = m_MysqlDb.ExecuteQuery(sql2);
                Tbl_ExamReportSubjectwiseDetails.DefaultCell.BorderWidthTop = 1;

                if (MyReader4.HasRows)
                {
                    Tbl_ExamReportSubjectwiseDetails.DefaultCell.HorizontalAlignment = 0;
                    Tbl_ExamReportSubjectwiseDetails.AddCell(new Phrase(" Maximum Mark", MyFonts[4]));
                    while (MyReader4.Read())
                    {
                        int _PeriodId = int.Parse(MyReader4.GetValue(0).ToString());
                        int _examschid = GetExamSchid(_ClassId, _ExamId, _CurrentBatchId, _PeriodId);
                        string _MaxMark = GetMaximumMarkForaPeriod(_examschid, _StudentId);
                        Tbl_ExamReportSubjectwiseDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_ExamReportSubjectwiseDetails.AddCell(new Phrase(_MaxMark, MyFonts[4]));

                    }
                    Tbl_ExamReportSubjectwiseDetails.AddCell(new Phrase("\n", MyFonts[4]));
                    Tbl_ExamReportSubjectwiseDetails.DefaultCell.HorizontalAlignment = 0;
                }

                //Result for Each Schedule

                sql2 = "select DISTINCT tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblclassexam.ExamId = " + _ExamId + " and tblexamschedule.BatchId =" + _CurrentBatchId + " and tblstudentclassmap.ClassId=" + _ClassId;
                MyReader4 = m_MysqlDb.ExecuteQuery(sql2);
                Tbl_ExamReportSubjectwiseDetails.DefaultCell.BorderWidthTop = 1;

                if (MyReader4.HasRows)
                {
                    Tbl_ExamReportSubjectwiseDetails.DefaultCell.HorizontalAlignment = 0;
                    Tbl_ExamReportSubjectwiseDetails.AddCell(new Phrase(" Result", MyFonts[4]));
                    while (MyReader4.Read())
                    {
                        int _PeriodId = int.Parse(MyReader4.GetValue(0).ToString());
                        int _examschid = GetExamSchid(_ClassId, _ExamId, _CurrentBatchId, _PeriodId);
                        string _Result = GetResultForaPeriod(_examschid, _StudentId);
                        Tbl_ExamReportSubjectwiseDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_ExamReportSubjectwiseDetails.AddCell(new Phrase(_Result, MyFonts[4]));

                    }
                    Tbl_ExamReportSubjectwiseDetails.AddCell(new Phrase("\n", MyFonts[4]));
                    Tbl_ExamReportSubjectwiseDetails.DefaultCell.HorizontalAlignment = 0;
                }

                //Grade for Each Schedule

                sql2 = "select DISTINCT tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblclassexam.ExamId = " + _ExamId + " and tblexamschedule.BatchId =" + _CurrentBatchId + " and tblstudentclassmap.ClassId=" + _ClassId;
                MyReader4 = m_MysqlDb.ExecuteQuery(sql2);
                Tbl_ExamReportSubjectwiseDetails.DefaultCell.BorderWidthTop = 1;

                if (MyReader4.HasRows)
                {
                    Tbl_ExamReportSubjectwiseDetails.DefaultCell.HorizontalAlignment = 0;
                    Tbl_ExamReportSubjectwiseDetails.AddCell(new Phrase(" Grade", MyFonts[4]));
                    while (MyReader4.Read())
                    {
                        int _PeriodId = int.Parse(MyReader4.GetValue(0).ToString());
                        int _examschid = GetExamSchid(_ClassId, _ExamId, _CurrentBatchId, _PeriodId);
                        string _Grade = GetGradeForaPeriod(_examschid, _StudentId);
                        Tbl_ExamReportSubjectwiseDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_ExamReportSubjectwiseDetails.AddCell(new Phrase(_Grade, MyFonts[4]));

                    }
                    Tbl_ExamReportSubjectwiseDetails.AddCell(new Phrase("\n", MyFonts[4]));
                    Tbl_ExamReportSubjectwiseDetails.DefaultCell.HorizontalAlignment = 0;
                }

                //Rank for Each Schedule

                sql2 = "select DISTINCT tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblclassexam.ExamId = " + _ExamId + " and tblexamschedule.BatchId =" + _CurrentBatchId + " and tblstudentclassmap.ClassId=" + _ClassId;
                MyReader4 = m_MysqlDb.ExecuteQuery(sql2);
                Tbl_ExamReportSubjectwiseDetails.DefaultCell.BorderWidthTop = 1;

                if (MyReader4.HasRows)
                {
                    Tbl_ExamReportSubjectwiseDetails.DefaultCell.HorizontalAlignment = 0;
                    Tbl_ExamReportSubjectwiseDetails.AddCell(new Phrase(" Rank", MyFonts[4]));
                    while (MyReader4.Read())
                    {
                        int _PeriodId = int.Parse(MyReader4.GetValue(0).ToString());
                        int _examschid = GetExamSchid(_ClassId, _ExamId, _CurrentBatchId, _PeriodId);
                        string _Rank = GetRankForaPeriod(_examschid, _StudentId);
                        Tbl_ExamReportSubjectwiseDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_ExamReportSubjectwiseDetails.AddCell(new Phrase(_Rank, MyFonts[4]));

                    }
                    Tbl_ExamReportSubjectwiseDetails.AddCell(new Phrase("\n", MyFonts[4]));
                    Tbl_ExamReportSubjectwiseDetails.DefaultCell.HorizontalAlignment = 0;
                }
            }

            return Tbl_ExamReportSubjectwiseDetails;

        }

        private iTextSharp.text.Table ER_SignAreaToTable()
        {
            iTextSharp.text.Table Tbl_SignArea = new iTextSharp.text.Table(4);
            Tbl_SignArea.Width = 100;
            float[] headerwidths = { 25, 25, 25, 25 };
            Tbl_SignArea.Widths = headerwidths;
            Tbl_SignArea.Padding = 1;
            Tbl_SignArea.DefaultCell.Border = 0;

            Tbl_SignArea.DefaultCell.Colspan = 4;
            Tbl_SignArea.AddCell(new Phrase(" \n", MyFonts[3]));

            Tbl_SignArea.DefaultCell.Colspan = 1;
            Tbl_SignArea.DefaultCell.HorizontalAlignment = 1;
            Tbl_SignArea.DefaultCell.VerticalAlignment = 1;
            Tbl_SignArea.AddCell(new Phrase("     Signature of Principal ", MyFonts[3]));
            Tbl_SignArea.AddCell(new Phrase("     Signature of HOD ", MyFonts[3]));
            Tbl_SignArea.AddCell(new Phrase("     Signature of Class Teacher ", MyFonts[3]));
            Tbl_SignArea.AddCell(new Phrase("     Signature of Guardian ", MyFonts[3]));

            Tbl_SignArea.DefaultCell.Colspan = 4;
            Tbl_SignArea.AddCell(new Phrase(" \n", MyFonts[3]));

            return Tbl_SignArea;
        }

        private iTextSharp.text.Table ER_LoadTotalandMaxMarkDetailsToTable(int _ClassId, int _CurrentBatchId, int _ExamId, int _StudentId)
        {
            iTextSharp.text.Table Tbl_TotalandMaxMarkDetails = new iTextSharp.text.Table(3);

            Tbl_TotalandMaxMarkDetails.Width = 100;
            Tbl_TotalandMaxMarkDetails.Padding = 1;
            Tbl_TotalandMaxMarkDetails.DefaultCell.Border = 0;
            float[] headerwidths = { 35, 35, 30 };
            Tbl_TotalandMaxMarkDetails.Widths = headerwidths;
            Tbl_TotalandMaxMarkDetails.AutoFillEmptyCells = true;

            Tbl_TotalandMaxMarkDetails.DefaultCell.HorizontalAlignment = 1;
            Tbl_TotalandMaxMarkDetails.DefaultCell.VerticalAlignment = 1;
            Tbl_TotalandMaxMarkDetails.DefaultCell.Colspan = 1;

            double _MaximumMark = 0, _TotMark = 0, _AvgMark = 0;

            sql = "select DISTINCT tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblclassexam.ExamId = " + _ExamId + " and tblexamschedule.BatchId =" + _CurrentBatchId + " and tblstudentclassmap.ClassId=" + _ClassId;
            m_MyReader7 = m_MysqlDb.ExecuteQuery(sql);
            int _TotalPeriod = m_MyReader7.RecordsAffected;

            if (m_MyReader7.HasRows)
            {
                while (m_MyReader7.Read())
                {
                    int _PeriodId = int.Parse(m_MyReader7.GetValue(0).ToString());
                    int _examschid = GetExamSchid(_ClassId, _ExamId, _CurrentBatchId, _PeriodId);
                    string _MaxMark = GetMaximumMarkForaPeriod(_examschid, _StudentId);
                    double _TotalMark = 0;
                    double.TryParse(GetTotalMarkForaPeriod(_examschid, _StudentId), out _TotalMark);
                    double _AverageMark = 0;
                    double.TryParse(GetAverageMarkForaPeriod(_examschid, _StudentId), out _AverageMark);

                    if (_MaxMark != "")
                    {
                        _MaximumMark = _MaximumMark + double.Parse(_MaxMark);
                        _TotMark = _TotMark + _TotalMark;
                    }

                }
                _AvgMark = (_TotMark / _MaximumMark) * 100;

                Tbl_TotalandMaxMarkDetails.AddCell(new Phrase("  Grand Total: " + Math.Round(_TotMark, 2), MyFonts[4]));
                Tbl_TotalandMaxMarkDetails.AddCell(new Phrase("  Average Mark: " + _AvgMark.ToString("#0.00") + " %", MyFonts[4]));
                Tbl_TotalandMaxMarkDetails.AddCell(new Phrase("  Maximum Mark: " + _MaximumMark, MyFonts[4]));

                Tbl_TotalandMaxMarkDetails.DefaultCell.Colspan = 3;
                Tbl_TotalandMaxMarkDetails.AddCell(new Phrase(" \n", MyFonts[3]));

            }

            return Tbl_TotalandMaxMarkDetails;
        }

        private iTextSharp.text.Table ER_LoadExamReportDetails(int _ClassId, int _CurrentBatchId, int _ExamId, int _StudentId, int _EmptyRowCount)
        {
            int _SubjectCount = 0;

            string sql2 = "select DISTINCT tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblclassexam.ExamId = " + _ExamId + " and tblexamschedule.BatchId =" + _CurrentBatchId + " and tblstudentclassmap.ClassId=" + _ClassId;
            m_MyReader2 = m_MysqlDb.ExecuteQuery(sql2);
            int _ColumCount = m_MyReader2.RecordsAffected + 2;
            iTextSharp.text.Table Tbl_ExamReportDetails = new iTextSharp.text.Table(_ColumCount);
            Tbl_ExamReportDetails.Width = 100;
            Tbl_ExamReportDetails.Padding = 1;
            Tbl_ExamReportDetails.DefaultCell.Border = 0;
            Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
            Tbl_ExamReportDetails.DefaultCell.BorderWidthRight = 1;
            Tbl_ExamReportDetails.DefaultCell.BorderWidthBottom = 1;
            Tbl_ExamReportDetails.AddCell(new Phrase("Subject", MyFonts[4])); // Sub Name

            if (m_MyReader2.HasRows)
            {
                while (m_MyReader2.Read())
                {
                    Tbl_ExamReportDetails.AddCell(new Phrase(m_MyReader2.GetValue(1).ToString(), MyFonts[4]));

                }
            }
            Tbl_ExamReportDetails.AddCell(new Phrase("Total", MyFonts[4])); // Sub Name


            string sql = "select distinct tblsubjects.subject_name, tblexammark.SubjectId, tblexammark.MarkColumn from tblexamschedule inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblexamschedule.ClassExamId inner JOIN tblexammark on tblexammark.ExamSchId= tblexamschedule.Id AND tblclassexamsubmap.SubId= tblexammark.SubjectId inner join tblsubjects on tblsubjects.Id= tblexammark.SubjectId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId and tblstudentclassmap.ClassId=" + _ClassId + "  and tblstudentclassmap.BatchId =" + _CurrentBatchId + " where tblclassexam.ExamId=" + _ExamId + " and tblexamschedule.BatchId=" + _CurrentBatchId + " order by tblexammark.SubjectOrder";
            m_MyReader3 = m_MysqlDb.ExecuteQuery(sql);
            _SubjectCount = m_MyReader3.RecordsAffected;
            if (m_MyReader3.HasRows)
            {
                while (m_MyReader3.Read())
                {
                    Tbl_ExamReportDetails.DefaultCell.BorderWidthBottom = 0;
                    Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 0;
                    Tbl_ExamReportDetails.DefaultCell.BorderWidthRight = 1;

                    double _SubjectTotalMark = 0;

                    Tbl_ExamReportDetails.AddCell(new Phrase(" " + m_MyReader3.GetValue(0).ToString(), MyFonts[3]));
                    sql2 = "select DISTINCT tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblclassexam.ExamId = " + _ExamId + " and tblexamschedule.BatchId =" + _CurrentBatchId + " and tblstudentclassmap.ClassId=" + _ClassId;
                    m_MyReader2 = m_MysqlDb.ExecuteQuery(sql2);
                    if (m_MyReader2.HasRows)
                    {
                        while (m_MyReader2.Read())
                        {
                            int _PeriodId = int.Parse(m_MyReader2.GetValue(0).ToString());
                            int _examschid = GetExamSchid(_ClassId, _ExamId, _CurrentBatchId, _PeriodId);
                            string _ExamMark = GetMarkfromColumn(_StudentId, _examschid, m_MyReader3.GetValue(2).ToString(), _PeriodId);
                            _SubjectTotalMark = _SubjectTotalMark + double.Parse(_ExamMark);
                            Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                            Tbl_ExamReportDetails.AddCell(new Phrase(_ExamMark, MyFonts[3]));
                        }
                        Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_ExamReportDetails.AddCell(new Phrase(_SubjectTotalMark.ToString(), MyFonts[3]));
                        Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 0;
                    }
                }
            }

            //_EmptyRowCount = 32 - _SubjectCount;
            for (int b = 0; b < _EmptyRowCount - 1; b++)
            {
                for (int a = 0; a < _ColumCount; a++)
                {
                    Tbl_ExamReportDetails.AddCell(new Phrase("\n", MyFonts[3]));
                }
            }
            return Tbl_ExamReportDetails;
        }

        private iTextSharp.text.Table ER_LoadStudentDetailsToTable(int _StudentId, string _StudentName, string _AdmissionNo, string _Batch, string _ClassName, int _ClassId, int _RollNo, int _UniversityNo, int _ExamId)
        {
            iTextSharp.text.Table Tbl_Details = new iTextSharp.text.Table(5);
            Tbl_Details.Width = 100;
            float[] headerwidths = { 15, 20, 30, 15, 20 };
            Tbl_Details.Widths = headerwidths;
            Tbl_Details.Padding = 1;
            Tbl_Details.DefaultCell.Border = 0;


            string _ExamName = "";
            string sql = "select tblexammaster.ExamName from tblexammaster where tblexammaster.Id=" + _ExamId;
            MyReader2 = m_MysqlDb.ExecuteQuery(sql);
            if (MyReader2.HasRows)
            {
                _ExamName = MyReader2.GetValue(0).ToString();
            }

            // fill data to table
            // first row
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("     Student Name  : ", MyFonts[3])); // Name
            Tbl_Details.DefaultCell.Colspan = 2;
            Tbl_Details.AddCell(new Phrase(_StudentName, MyFonts[4]));// bold
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("  Batch  : ", MyFonts[3])); //Bil; date
            Tbl_Details.AddCell(new Phrase(_Batch, MyFonts[4]));

            // Second row
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("     Class  : ", MyFonts[3])); //Addmission No
            Tbl_Details.DefaultCell.Colspan = 2;
            Tbl_Details.AddCell(new Phrase(_ClassName, MyFonts[4]));
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("  RollNo  : ", MyFonts[3])); //Class name
            Tbl_Details.AddCell(new Phrase(_RollNo.ToString(), MyFonts[4]));

            // Third row
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("     Exam Name  : ", MyFonts[3])); //Addmission No
            Tbl_Details.DefaultCell.Colspan = 2;
            Tbl_Details.AddCell(new Phrase(_ExamName, MyFonts[4]));
            Tbl_Details.DefaultCell.Colspan = 2;
            Tbl_Details.AddCell(new Phrase(" \n", MyFonts[3])); //Class name

            Tbl_Details.DefaultCell.Colspan = 5;
            Tbl_Details.AddCell(new Phrase("\n", MyFonts[3]));//empty

            return Tbl_Details;
        }

        private iTextSharp.text.Table ER_LoadCollageDetilsToTable(string _BatchName, string _physicalpath)
        {
            iTextSharp.text.Table _Tbl_CollageDetails = new iTextSharp.text.Table(2);
            _Tbl_CollageDetails.Width = 100;
            float[] headerwidths = { 20, 80 };
            _Tbl_CollageDetails.Widths = headerwidths;
            _Tbl_CollageDetails.Padding = 1;
            _Tbl_CollageDetails.Border = 0;
            _Tbl_CollageDetails.AutoFillEmptyCells = true;
            _Tbl_CollageDetails.DefaultCell.Border = 0;
            ImageUploaderClass imgobj = new ImageUploaderClass(m_objSchool);
            sql = "select tblschooldetails.SchoolName, tblschooldetails.Address from tblschooldetails";
            //GroupName(0),Address(1)
            m_MyReader1 = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader1.HasRows)
            {
                string _HeaderImgName = m_MyReader1.GetValue(2).ToString();
                if (true)
                {
                   if (true)
                    {
                        iTextSharp.text.Image _ImgHeader = iTextSharp.text.Image.GetInstance(imgobj.getImageBytes(1, "Logo"));
                        _ImgHeader.ScaleAbsolute(50, 50);
                        _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 0;
                        _Tbl_CollageDetails.DefaultCell.Colspan = 1;
                        _Tbl_CollageDetails.DefaultCell.Rowspan = 2;

                        Cell cell = new Cell();
                        cell.Rowspan = 2;
                        cell.Add(_ImgHeader);
                        cell.Border = 0;
                        _Tbl_CollageDetails.AddCell(cell);
                        _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                        _Tbl_CollageDetails.DefaultCell.Rowspan = 1;
                        _Tbl_CollageDetails.DefaultCell.VerticalAlignment = 1;
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(0).ToString(), MyFonts[1])); //Collage Name
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(1).ToString(), MyFonts[2]));//Collage Address
                    }
                    else
                    {
                        _Tbl_CollageDetails.DefaultCell.Colspan = 2;
                        _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                        _Tbl_CollageDetails.DefaultCell.Rowspan = 1;
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(0).ToString(), MyFonts[1])); //Collage Name
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(1).ToString(), MyFonts[2]));//Collage Address

                    }


                }
                else
                {
                    _Tbl_CollageDetails.DefaultCell.Colspan = 2;
                    _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                    _Tbl_CollageDetails.DefaultCell.Rowspan = 1;
                    _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(0).ToString(), MyFonts[1])); //Collage Name
                    _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(1).ToString(), MyFonts[2]));//Collage Address

                }

            }
            _Tbl_CollageDetails.DefaultCell.Colspan = 2;
            _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
            _Tbl_CollageDetails.AddCell(new Phrase("\n", MyFonts[3])); //empty

            return _Tbl_CollageDetails;

        }



        #endregion


        #region COMMON EXAM REPORT FUNCTIONS

        private string GetAverageMarkForaPeriod(int _examschid, int _StudentId)
        {
            string _Avg = "";
            double _AvgMark = 0;
            string sql = "select tblstudentmark.Avg from tblstudentmark where  tblstudentmark.ExamSchId=" + _examschid + " and tblstudentmark.StudId =" + _StudentId + "";
            MyReader3 = m_MysqlDb.ExecuteQuery(sql);
            if (MyReader3.HasRows)
            {
                _AvgMark = double.Parse(MyReader3.GetValue(0).ToString());
                _Avg = _AvgMark.ToString("#0.00");
            }
            return _Avg;
        }

        private string GetTotalMarkForaPeriod(int _examschid, int _StudentId)
        {
            string _Total = "";
            double _TotalMark = 0;
            string sql = "select tblstudentmark.TotalMark from tblstudentmark where  tblstudentmark.ExamSchId=" + _examschid + " and tblstudentmark.StudId =" + _StudentId + "";
            MyReader2 = m_MysqlDb.ExecuteQuery(sql);
            if (MyReader2.HasRows)
            {
                _TotalMark = double.Parse(MyReader2.GetValue(0).ToString());
                _Total = _TotalMark.ToString();
            }
            return _Total;
        }

        private string GetMaximumMarkForaPeriod(int _examschid, int _StudentId)
        {
            string _Max = "";
            string sql = "select tblstudentmark.TotalMax from tblstudentmark where  tblstudentmark.ExamSchId=" + _examschid + " and tblstudentmark.StudId =" + _StudentId + "";
            MyReader1 = m_MysqlDb.ExecuteQuery(sql);
            if (MyReader1.HasRows)
            {
                _Max = MyReader1.GetValue(0).ToString();
            }
            return _Max;
        }

        private string GetRankForaPeriod(int _examschid, int _StudentId)
        {
            string _Rank = "";
            string sql = "select tblstudentmark.Rank from tblstudentmark where  tblstudentmark.ExamSchId=" + _examschid + " and tblstudentmark.StudId =" + _StudentId + "";
            MyReader3 = m_MysqlDb.ExecuteQuery(sql);
            if (MyReader3.HasRows)
            {
                _Rank = MyReader3.GetValue(0).ToString();
            }
            return _Rank;
        }

        private string GetGradeForaPeriod(int _examschid, int _StudentId)
        {
            string _Grade = "";
            string sql = "select tblstudentmark.Grade from tblstudentmark where  tblstudentmark.ExamSchId=" + _examschid + " and tblstudentmark.StudId =" + _StudentId + "";
            MyReader3 = m_MysqlDb.ExecuteQuery(sql);
            if (MyReader3.HasRows)
            {
                _Grade = MyReader3.GetValue(0).ToString();
            }
            return _Grade;
        }

        private string GetResultForaPeriod(int _examschid, int _StudentId)
        {
            string _Result = "";
            string sql = "select tblstudentmark.Result from tblstudentmark where  tblstudentmark.ExamSchId=" + _examschid + " and tblstudentmark.StudId =" + _StudentId + "";
            MyReader3 = m_MysqlDb.ExecuteQuery(sql);
            if (MyReader3.HasRows)
            {
                _Result = MyReader3.GetValue(0).ToString();
            }
            return _Result;
        }

        private string GetMarkfromColumn(int _StudentId, int _examschid, string _Column, int _PeriodId)
        {
            string _ExamMark;

            string sql = "Select tblstudentmark." + _Column + " From  tblstudentmark inner join tblexamschedule on tblexamschedule.Id = tblstudentmark.ExamSchId where tblstudentmark.ExamSchId=" + _examschid + " AND tblstudentmark.StudId=" + _StudentId + " and tblexamschedule.PeriodId =" + _PeriodId;
            m_MyReader5 = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader5.HasRows)
            {
                _ExamMark = m_MyReader5.GetValue(0).ToString();

            }
            else
            {
                _ExamMark = "-1";
            }

            return _ExamMark;
        }

        private int GetExamSchid(int _ClassId, int _ExamId, int _CurrentBatchId, int _PeriodId)
        {
            int PeriodId = -1;
            string sql = "select DISTINCT tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId  and tblclassexam.ExamId=" + _ExamId + "  inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId where tblstudentclassmap.ClassId= " + _ClassId + " and tblstudentclassmap.BatchId=" + _CurrentBatchId + " and tblexamschedule.PeriodId=" + _PeriodId + " and tblexamschedule.BatchId=" + _CurrentBatchId;
            m_MyReader4 = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader4.HasRows)
            {
                PeriodId = int.Parse(m_MyReader4.GetValue(0).ToString());
            }
            return PeriodId;
        }

        private bool Delete_Existing_ExamReport(string _PdfName, string _physicalpath, out string _ErrorMsg)
        {
            _ErrorMsg = "";
            bool _valid = false;
            try
            {
                sql = "SELECT tblexamreport.ReportName from tblexamreport";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    while (m_MyReader.Read())
                    {
                        string _pdf = m_MyReader.GetValue(0).ToString();
                        File.Delete(_physicalpath + "\\PDF_Files\\" + _pdf);
                        _valid = true;
                    }

                }
                else
                {
                    _valid = true;
                }

            }
            catch
            {
                _ErrorMsg = "This File is Allready Used by another Person.Please try again later.";
                _valid = false;
            }
            return _valid;
        }

        #endregion


        #region PERIOD EXAM REPORT

        internal bool CreatePeriodExamReportPdf(int _ClassId, int _ExamId, int _PeriodId, int _CurrentBatchId, string _BatchName, string _physicalpath, out string _PdfName, out string _ErrorMsg)
        {
            _PdfName = "";
            _ErrorMsg = "";

            try
            {
                string _ClassName = "";
                sql = "SELECT tblclass.ClassName from tblclass where tblclass.Id=" + _ClassId;
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _ClassName = m_MyReader.GetValue(0).ToString();
                }
                string _Period = "";
                sql = "select tblperiod.Period from tblperiod where tblperiod.Id=" + _PeriodId;
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _Period = m_MyReader.GetValue(0).ToString();
                }
                _PdfName = "ER_" + _Period + "_" + _ClassName + ".pdf";


                if (Delete_Existing_ExamReport(_PdfName, _physicalpath, out _ErrorMsg))
                {
                    Document DocPER = new Document(PageSize.A4, 50, 40, 20, 10);
                    PdfWriter writerPFR = PdfWriter.GetInstance(DocPER, new FileStream(_physicalpath + "\\PDF_Files\\ER_" + _Period + "_" + _ClassName + ".pdf", FileMode.Create));

                    DocPER.Open();

                    string sql1 = "SELECT tblstudentclassmap.StudentId, tblstudentclassmap.RollNo, tblstudent.StudentName, tblstudent.AdmitionNo from tblstudentclassmap inner JOIN tblstudent on tblstudentclassmap.StudentId= tblstudent.Id  where tblstudentclassmap.ClassId=" + _ClassId;
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql1);
                    if (m_MyReader.HasRows)
                    {
                        while (m_MyReader.Read())
                        {
                            int _StudentId = int.Parse(m_MyReader.GetValue(0).ToString());
                            int _UniversityNo = 0;
                            int _RollNo = int.Parse(m_MyReader.GetValue(1).ToString());
                            string _StudentName = m_MyReader.GetValue(2).ToString();
                            string _AdmissionNo = m_MyReader.GetValue(3).ToString();// _Batch = m_MyReader.GetValue(4).ToString();
                            iTextSharp.text.Table Tbl_CollageInformation = PER_LoadCollageDetilsToTable(_BatchName, _physicalpath);
                            iTextSharp.text.Table Tbl_StudentInformation = PER_LoadStudentDetailsToTable(_StudentId, _StudentName, _AdmissionNo, _BatchName, _ClassName, _ClassId, _RollNo, _UniversityNo, _ExamId, _Period);
                            iTextSharp.text.Table Tbl_ExamReportDetails = PER_LoadExamReportDetails(_ClassId, _CurrentBatchId, _ExamId, _StudentId, _PeriodId);
                            iTextSharp.text.Table Tbl_TotalandMaxMark = PER_LoadTotalandMaxMarkDetailsToTable(_ClassId, _CurrentBatchId, _ExamId, _StudentId, _PeriodId);
                            iTextSharp.text.Table Tbl_SignAreaDetails = PER_SignAreaToTable();

                            DocPER.Add(Tbl_CollageInformation);
                            DocPER.Add(Tbl_StudentInformation);
                            DocPER.Add(Tbl_ExamReportDetails);
                            DocPER.Add(Tbl_TotalandMaxMark);
                            DocPER.Add(Tbl_SignAreaDetails);
                            DocPER.NewPage();

                        }
                    }
                    DocPER.Close();

                    sql = "Delete from tblexamreport ";
                    m_MysqlDb.ExecuteQuery(sql);
                    sql = "INSERT into tblexamreport(tblexamreport.ReportName) VALUES ('" + _PdfName + "')";
                    m_MysqlDb.ExecuteQuery(sql);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                if (e.Message == "The process cannot access the file")
                    _ErrorMsg = "This Bill is used by another person.Try again later";
                return false;
            }
        }

        private iTextSharp.text.Table PER_LoadTotalandMaxMarkDetailsToTable(int _ClassId, int _CurrentBatchId, int _ExamId, int _StudentId, int _PeriodId)
        {
            iTextSharp.text.Table Tbl_TotalandMaxMarkDetails = new iTextSharp.text.Table(4);

            Tbl_TotalandMaxMarkDetails.Width = 100;
            Tbl_TotalandMaxMarkDetails.Padding = 1;
            Tbl_TotalandMaxMarkDetails.DefaultCell.Border = 0;
            float[] headerwidths = { 25, 25, 25, 25 };
            Tbl_TotalandMaxMarkDetails.Widths = headerwidths;
            Tbl_TotalandMaxMarkDetails.AutoFillEmptyCells = true;

            int _examschid = GetExamSchid(_ClassId, _ExamId, _CurrentBatchId, _PeriodId);//exam schedule id
            double _TotalMark = 0;
            double.TryParse(GetTotalMarkForaPeriod(_examschid, _StudentId), out _TotalMark);//total mark that a student obtained

            string _MaxMark = GetMaximumMarkForaPeriod(_examschid, _StudentId);//Max Mark for that Period

            double _AverageMark = 0;
            double.TryParse(GetAverageMarkForaPeriod(_examschid, _StudentId), out _AverageMark);//Avg mark for the period

            string _Result = GetResultForaPeriod(_examschid, _StudentId);

            string _Grade = GetGradeForaPeriod(_examschid, _StudentId);

            string _Rank = GetRankForaPeriod(_examschid, _StudentId);

            Tbl_TotalandMaxMarkDetails.DefaultCell.Colspan = 1;
            Tbl_TotalandMaxMarkDetails.DefaultCell.BorderWidthBottom = 1;
            Tbl_TotalandMaxMarkDetails.DefaultCell.BorderWidthRight = 1;
            Tbl_TotalandMaxMarkDetails.DefaultCell.HorizontalAlignment = 1;
            Tbl_TotalandMaxMarkDetails.AddCell(new Phrase("Grand Total ", MyFonts[4])); // Total Obtained
            Tbl_TotalandMaxMarkDetails.DefaultCell.HorizontalAlignment = 0;
            Tbl_TotalandMaxMarkDetails.AddCell(new Phrase(Math.Round(_TotalMark, 2).ToString(), MyFonts[4])); // Total Obtained
            Tbl_TotalandMaxMarkDetails.DefaultCell.HorizontalAlignment = 1;
            Tbl_TotalandMaxMarkDetails.AddCell(new Phrase("Maximum Total ", MyFonts[4])); //Max Total
            Tbl_TotalandMaxMarkDetails.DefaultCell.HorizontalAlignment = 0;
            Tbl_TotalandMaxMarkDetails.AddCell(new Phrase(_MaxMark, MyFonts[4])); //Max Total

            Tbl_TotalandMaxMarkDetails.DefaultCell.HorizontalAlignment = 1;
            Tbl_TotalandMaxMarkDetails.AddCell(new Phrase("Average Mark", MyFonts[4])); //Avg mark
            Tbl_TotalandMaxMarkDetails.DefaultCell.HorizontalAlignment = 0;
            Tbl_TotalandMaxMarkDetails.AddCell(new Phrase(Math.Round(_AverageMark, 2) + " %", MyFonts[4])); //Avg Mark
            Tbl_TotalandMaxMarkDetails.DefaultCell.HorizontalAlignment = 1;
            Tbl_TotalandMaxMarkDetails.AddCell(new Phrase("Result ", MyFonts[4])); // Result
            Tbl_TotalandMaxMarkDetails.DefaultCell.HorizontalAlignment = 0;
            Tbl_TotalandMaxMarkDetails.AddCell(new Phrase(_Result, MyFonts[4])); // Result

            Tbl_TotalandMaxMarkDetails.DefaultCell.HorizontalAlignment = 1;
            Tbl_TotalandMaxMarkDetails.AddCell(new Phrase("Grade", MyFonts[4])); // Grade
            Tbl_TotalandMaxMarkDetails.DefaultCell.HorizontalAlignment = 0;
            Tbl_TotalandMaxMarkDetails.AddCell(new Phrase(_Grade, MyFonts[4])); // Grade
            Tbl_TotalandMaxMarkDetails.DefaultCell.HorizontalAlignment = 1;
            Tbl_TotalandMaxMarkDetails.AddCell(new Phrase("Rank", MyFonts[4])); // Rank
            Tbl_TotalandMaxMarkDetails.DefaultCell.HorizontalAlignment = 0;
            Tbl_TotalandMaxMarkDetails.AddCell(new Phrase(_Rank, MyFonts[4])); // Rank


            return Tbl_TotalandMaxMarkDetails;
        }

        private iTextSharp.text.Table PER_SignAreaToTable()
        {
            iTextSharp.text.Table Tbl_SignArea = new iTextSharp.text.Table(4);
            Tbl_SignArea.Width = 100;
            float[] headerwidths = { 25, 25, 25, 25 };
            Tbl_SignArea.Widths = headerwidths;
            Tbl_SignArea.Padding = 1;
            Tbl_SignArea.DefaultCell.Border = 0;

            Tbl_SignArea.DefaultCell.Colspan = 4;
            Tbl_SignArea.AddCell(new Phrase(" \n", MyFonts[3]));

            Tbl_SignArea.DefaultCell.Colspan = 1;
            Tbl_SignArea.DefaultCell.HorizontalAlignment = 1;
            Tbl_SignArea.DefaultCell.VerticalAlignment = 1;
            Tbl_SignArea.AddCell(new Phrase("     Signature of Principal ", MyFonts[3]));
            Tbl_SignArea.AddCell(new Phrase("     Signature of HOD ", MyFonts[3]));
            Tbl_SignArea.AddCell(new Phrase("     Signature of Class Teacher ", MyFonts[3]));
            Tbl_SignArea.AddCell(new Phrase("     Signature of Guardian ", MyFonts[3]));

            Tbl_SignArea.DefaultCell.Colspan = 4;
            Tbl_SignArea.AddCell(new Phrase(" \n", MyFonts[3]));

            return Tbl_SignArea;
        }

        private iTextSharp.text.Table PER_LoadExamReportDetails(int _ClassId, int _CurrentBatchId, int _ExamId, int _StudentId, int _PeriodId)
        {
            int _EmptyRowCount = 0, _SubjectCount = 0;
            iTextSharp.text.Table Tbl_ExamReportDetails = new iTextSharp.text.Table(5);

            float[] headerwidths = { 40, 15, 15, 15, 15 };
            Tbl_ExamReportDetails.Widths = headerwidths;

            Tbl_ExamReportDetails.Width = 100;
            Tbl_ExamReportDetails.Padding = 1;
            Tbl_ExamReportDetails.DefaultCell.Border = 0;
            Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
            Tbl_ExamReportDetails.DefaultCell.BorderWidthRight = 1;
            Tbl_ExamReportDetails.DefaultCell.BorderWidthBottom = 1;

            Tbl_ExamReportDetails.AddCell(new Phrase("Subject", MyFonts[4])); // Sub Name
            Tbl_ExamReportDetails.AddCell(new Phrase("Obtained Mark", MyFonts[4])); // Obt mark
            Tbl_ExamReportDetails.AddCell(new Phrase("Pass Mark", MyFonts[4])); // pass mark
            Tbl_ExamReportDetails.AddCell(new Phrase("Max.Mark", MyFonts[4])); // Max.mark
            Tbl_ExamReportDetails.AddCell(new Phrase("Result", MyFonts[4])); // Result




            string sql = "select distinct tblsubjects.subject_name, tblexammark.SubjectId, tblexammark.MarkColumn from tblexamschedule inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblexamschedule.ClassExamId inner JOIN tblexammark on tblexammark.ExamSchId= tblexamschedule.Id AND tblclassexamsubmap.SubId= tblexammark.SubjectId inner join tblsubjects on tblsubjects.Id= tblexammark.SubjectId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId and tblstudentclassmap.ClassId=" + _ClassId + "  and tblstudentclassmap.BatchId =" + _CurrentBatchId + " where tblclassexam.ExamId=" + _ExamId + " and tblexamschedule.BatchId=" + _CurrentBatchId + " order by tblexammark.SubjectOrder";
            m_MyReader3 = m_MysqlDb.ExecuteQuery(sql);
            _SubjectCount = m_MyReader3.RecordsAffected;
            if (m_MyReader3.HasRows)
            {

                while (m_MyReader3.Read())
                {
                    Tbl_ExamReportDetails.DefaultCell.BorderWidthBottom = 0;
                    Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 0;
                    Tbl_ExamReportDetails.DefaultCell.BorderWidthRight = 1;

                    Tbl_ExamReportDetails.AddCell(new Phrase(" " + m_MyReader3.GetValue(0).ToString(), MyFonts[3]));//Sub Name column

                    Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                    int _examschid = GetExamSchid(_ClassId, _ExamId, _CurrentBatchId, _PeriodId);//get exam schedule id
                    double _ExamMark = 0;
                    double.TryParse(GetMarkfromColumn(_StudentId, _examschid, m_MyReader3.GetValue(2).ToString(), _PeriodId), out _ExamMark);//get sub mark
                    if (_ExamMark == -1)
                    {
                        Tbl_ExamReportDetails.AddCell(new Phrase("A", MyFonts[3]));//Absent
                    }
                    else
                    {
                        Tbl_ExamReportDetails.AddCell(new Phrase(Math.Round(_ExamMark, 2).ToString(), MyFonts[3]));//Sub.Mark column
                    }

                    string sql1 = "SELECT tblclassexamsubmap.MaxMark, tblclassexamsubmap.MinMark from tblclassexamsubmap inner join tblclassexam on tblclassexamsubmap.ClassExamId= tblclassexam.Id where tblclassexam.ExamId=" + _ExamId + " and tblclassexamsubmap.SubId=" + int.Parse(m_MyReader3.GetValue(1).ToString()) + "  and tblclassexam.ClassId=" + _ClassId;
                    MyReader1 = m_MysqlDb.ExecuteQuery(sql1);
                    float _SubjectMaxMark = 0;
                    float _SubjectPassMark = 0;
                    if (MyReader1.HasRows)
                    {
                        _SubjectMaxMark = float.Parse(MyReader1.GetValue(0).ToString());
                        _SubjectPassMark = float.Parse(MyReader1.GetValue(1).ToString());

                    }
                    Tbl_ExamReportDetails.AddCell(new Phrase(_SubjectPassMark.ToString(), MyFonts[3]));//add pass mark column 
                    Tbl_ExamReportDetails.AddCell(new Phrase(_SubjectMaxMark.ToString(), MyFonts[3]));//add max mark column

                    string _Result = "";

                    if (float.Parse(_ExamMark.ToString()) < _SubjectPassMark)
                    {
                        _Result = "Failed";
                    }
                    else
                    {
                        _Result = "Pass";
                    }
                    Tbl_ExamReportDetails.AddCell(new Phrase(_Result, MyFonts[3]));//add Result column
                }
            }
            _EmptyRowCount = 32 - _SubjectCount;

            for (int b = 0; b < _EmptyRowCount; b++)
            {
                for (int a = 0; a < 5; a++)
                {
                    Tbl_ExamReportDetails.AddCell(new Phrase("\n", MyFonts[4]));
                }
            }

            return Tbl_ExamReportDetails;
        }

        private iTextSharp.text.Table PER_LoadStudentDetailsToTable(int _StudentId, string _StudentName, string _AdmissionNo, string _Batch, string _ClassName, int _ClassId, int _RollNo, int _UniversityNo, int _ExamId, string _PeriodName)
        {
            iTextSharp.text.Table Tbl_Details = new iTextSharp.text.Table(5);
            Tbl_Details.Width = 100;
            float[] headerwidths = { 15, 20, 30, 15, 20 };
            Tbl_Details.Widths = headerwidths;
            Tbl_Details.Padding = 1;
            Tbl_Details.DefaultCell.Border = 0;

            string _ExamName = "";
            string sql = "select tblexammaster.ExamName from tblexammaster where tblexammaster.Id=" + _ExamId;
            MyReader2 = m_MysqlDb.ExecuteQuery(sql);
            if (MyReader2.HasRows)
            {
                _ExamName = MyReader2.GetValue(0).ToString();
            }


            // fill data to table
            // first row
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("     Student Name  : ", MyFonts[3])); // Name
            Tbl_Details.DefaultCell.Colspan = 2;
            Tbl_Details.AddCell(new Phrase(_StudentName, MyFonts[4]));// bold
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("  Batch  : ", MyFonts[3])); //Batch
            Tbl_Details.AddCell(new Phrase(_Batch, MyFonts[4]));

            // Second row
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("     Class  : ", MyFonts[3])); //Class
            Tbl_Details.DefaultCell.Colspan = 2;
            Tbl_Details.AddCell(new Phrase(_ClassName, MyFonts[4]));
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("  Exam Period  : ", MyFonts[3])); //Exam Period
            Tbl_Details.AddCell(new Phrase(_PeriodName, MyFonts[4]));

            // Third row
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("     RollNo  : ", MyFonts[3])); //Roll No
            Tbl_Details.DefaultCell.Colspan = 2;
            Tbl_Details.AddCell(new Phrase(_RollNo.ToString(), MyFonts[4]));
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("  Exam Name  : ", MyFonts[3])); //Exam name
            Tbl_Details.AddCell(new Phrase(_ExamName, MyFonts[4]));

            Tbl_Details.DefaultCell.Colspan = 5;
            Tbl_Details.AddCell(new Phrase("\n", MyFonts[3])); //empty

            return Tbl_Details;
        }

        private iTextSharp.text.Table PER_LoadCollageDetilsToTable(string _BatchName, string _physicalpath)
        {
            iTextSharp.text.Table _Tbl_CollageDetails = new iTextSharp.text.Table(2);
            _Tbl_CollageDetails.Width = 100;
            float[] headerwidths = { 20, 80 };
            _Tbl_CollageDetails.Widths = headerwidths;
            _Tbl_CollageDetails.Padding = 1;
            _Tbl_CollageDetails.Border = 0;
            _Tbl_CollageDetails.AutoFillEmptyCells = true;
            _Tbl_CollageDetails.DefaultCell.Border = 0;
            ImageUploaderClass imgobj = new ImageUploaderClass(m_objSchool);
            sql = "select tblschooldetails.SchoolName, tblschooldetails.Address from tblschooldetails";
            //GroupName(0),Address(1)
            m_MyReader1 = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader1.HasRows)
            {
                
                if (true)
                {

                    if (true)
                    {
                        iTextSharp.text.Image _ImgHeader = iTextSharp.text.Image.GetInstance(imgobj.getImageBytes(1, "Logo"));
                        _ImgHeader.ScaleAbsolute(50, 50);
                        _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 0;
                        _Tbl_CollageDetails.DefaultCell.Colspan = 1;
                        _Tbl_CollageDetails.DefaultCell.Rowspan = 2;

                        Cell cell = new Cell();
                        cell.Rowspan = 2;
                        cell.Add(_ImgHeader);
                        cell.Border = 0;
                        _Tbl_CollageDetails.AddCell(cell);
                        _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                        _Tbl_CollageDetails.DefaultCell.Rowspan = 1;
                        _Tbl_CollageDetails.DefaultCell.VerticalAlignment = 1;
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(0).ToString(), MyFonts[1])); //Collage Name
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(1).ToString(), MyFonts[2]));//Collage Address
                    }
                    else
                    {
                        _Tbl_CollageDetails.DefaultCell.Colspan = 2;
                        _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                        _Tbl_CollageDetails.DefaultCell.Rowspan = 1;
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(0).ToString(), MyFonts[1])); //Collage Name
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(1).ToString(), MyFonts[2]));//Collage Address

                    }


                }
                else
                {
                    _Tbl_CollageDetails.DefaultCell.Colspan = 2;
                    _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                    _Tbl_CollageDetails.DefaultCell.Rowspan = 1;
                    _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(0).ToString(), MyFonts[1])); //Collage Name
                    _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(1).ToString(), MyFonts[2]));//Collage Address

                }

            }
            _Tbl_CollageDetails.DefaultCell.Colspan = 2;
            _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
            _Tbl_CollageDetails.AddCell(new Phrase("\n", MyFonts[3])); //empty

            return _Tbl_CollageDetails;
        }

        #endregion


        #region Individual Student Performance Report

        internal bool CreateIndividualStudentExamReportPdf(int _StudentId, int _ExamId, int _CurrentBatchId, string _BatchName, string _physicalpath, out string _PdfName, out string _ErrorMsg)
        {
            _PdfName = "";
            _ErrorMsg = "";
            int _EmptyRowCount = 0;

            try
            {
                string _StudName = "";
                sql = "SELECT tblstudent.StudentName from tblstudent where tblstudent.Id=" + _StudentId;
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _StudName = m_MyReader.GetValue(0).ToString();
                }
                _PdfName = "ER_" + _StudName + ".pdf";


                if (Delete_Existing_ExamReport(_PdfName, _physicalpath, out _ErrorMsg))
                {
                    Document DocER = new Document(PageSize.A4, 50, 40, 20, 10);
                    PdfWriter writerER = PdfWriter.GetInstance(DocER, new FileStream(_physicalpath + "\\PDF_Files\\ER_" + _StudName + ".pdf", FileMode.Create));

                    Document DocERTemp = new Document(PageSize.A4, 50, 40, 20, 10);
                    PdfWriter writerERTemp = PdfWriter.GetInstance(DocERTemp, new FileStream(_physicalpath + "\\PDF_Files\\ERTemp_" + _StudName + ".pdf", FileMode.Create));

                    DocERTemp.Open();
                    DocER.Open();

                    string sql1 = "SELECT tblstudentclassmap.ClassId, tblstudentclassmap.RollNo, tblstudent.StudentName,tblstudent.AdmitionNo, tblclass.ClassName from tblstudentclassmap inner JOIN tblstudent on tblstudentclassmap.StudentId= tblstudent.Id inner join tblclass on tblstudentclassmap.ClassId= tblclass.Id  where tblstudentclassmap.StudentId=" + _StudentId;
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql1);
                    if (m_MyReader.HasRows)
                    {
                        while (m_MyReader.Read())
                        {
                            int _ClassId = int.Parse(m_MyReader.GetValue(0).ToString());
                            int _UniversityNo = 0;
                            int _RollNo = int.Parse(m_MyReader.GetValue(1).ToString());
                            string _StudentName = m_MyReader.GetValue(2).ToString();
                            string _AdmissionNo = m_MyReader.GetValue(3).ToString();
                            string _ClassName = m_MyReader.GetValue(4).ToString();
                            iTextSharp.text.Table Tbl_CollageInformation = ER_LoadCollageDetilsToTable(_BatchName, _physicalpath);
                            iTextSharp.text.Table Tbl_StudentInformation = ER_LoadStudentDetailsToTable(_StudentId, _StudentName, _AdmissionNo, _BatchName, _ClassName, _ClassId, _RollNo, _UniversityNo, _ExamId);

                            iTextSharp.text.Table Tbl_SubjectWiseTotalandMaxMarks = ER_LoadSubWiseTotalandMaxMarkDetails(_ClassId, _CurrentBatchId, _ExamId, _StudentId);
                            iTextSharp.text.Table Tbl_TotalandMaxMark = ER_LoadTotalandMaxMarkDetailsToTable(_ClassId, _CurrentBatchId, _ExamId, _StudentId);
                            iTextSharp.text.Table Tbl_SignAreaDetails = ER_SignAreaToTable();

                            DocERTemp.Add(Tbl_CollageInformation);
                            DocERTemp.Add(Tbl_StudentInformation);

                            DocERTemp.Add(Tbl_SubjectWiseTotalandMaxMarks);
                            DocERTemp.Add(Tbl_TotalandMaxMark);
                            DocERTemp.Add(Tbl_SignAreaDetails);
                            DocERTemp = ERTemp_LoadExamReportDetails(DocERTemp, writerERTemp, _ClassId, _CurrentBatchId, _ExamId, _StudentId, out _EmptyRowCount);

                            DocERTemp.NewPage();


                            DocER.Add(Tbl_CollageInformation);
                            DocER.Add(Tbl_StudentInformation);
                            //iTextSharp.text.Table Tbl_ExamReportDetails = ER_LoadExamReportDetails(_ClassId, _CurrentBatchId, _ExamId, _StudentId, _EmptyRowCount);
                            //DocER.Add(Tbl_ExamReportDetails);
                            DocER = ERDoc_LoadExamReportDetails(DocER, writerER, _ClassId, _CurrentBatchId, _ExamId, _StudentId, _EmptyRowCount);
                            DocER.Add(Tbl_SubjectWiseTotalandMaxMarks);
                            DocER.Add(Tbl_TotalandMaxMark);
                            DocER.Add(Tbl_SignAreaDetails);




                            // DocER.NewPage();
                        }
                    }
                    DocER.Close();

                    DocERTemp.Close();
                    File.Delete(_physicalpath + "\\PDF_Files\\ERTemp_" + _StudName + ".pdf");

                    sql = "Delete from tblexamreport ";
                    m_MysqlDb.ExecuteQuery(sql);
                    sql = "INSERT into tblexamreport(tblexamreport.ReportName) VALUES ('" + _PdfName + "')";
                    m_MysqlDb.ExecuteQuery(sql);

                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception e)
            {
                if (e.Message == "The process cannot access the file")
                    _ErrorMsg = "This Bill is used by another person.Try again later";
                return false;
            }

        }

        #endregion


        #region Individual Student Period Performance Report

        internal bool CreateIndividualStudentPeriodExamReportPdf(int _StudentId, int _ExamId, int _PeriodId, int _CurrentBatchId, string _BatchName, int ClassId_Schduled, string _physicalpath, out string _PdfName, out string _ErrorMsg)
        {
            _PdfName = "";
            _ErrorMsg = "";

            try
            {
                string _StudName = "";
                sql = "SELECT tblstudent.StudentName from tblstudent where tblstudent.Id=" + _StudentId;
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _StudName = m_MyReader.GetValue(0).ToString();
                }
                string _Period = "";
                sql = "select tblperiod.Period from tblperiod where tblperiod.Id=" + _PeriodId;
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _Period = m_MyReader.GetValue(0).ToString();
                }
                _PdfName = "ER_" + _Period + "_" + _StudName + ".pdf";


                if (Delete_Existing_ExamReport(_PdfName, _physicalpath, out _ErrorMsg))
                {
                    Document DocPER = new Document(PageSize.A4, 50, 40, 20, 10);
                    PdfWriter writerPFR = PdfWriter.GetInstance(DocPER, new FileStream(_physicalpath + "\\PDF_Files\\ER_" + _Period + "_" + _StudName + ".pdf", FileMode.Create));

                    DocPER.Open();

                    string sql1 = "SELECT tblstudentclassmap.ClassId, tblstudentclassmap.RollNo, tblstudent.StudentName,tblstudent.AdmitionNo, tblclass.ClassName from tblstudentclassmap inner JOIN tblstudent on tblstudentclassmap.StudentId= tblstudent.Id inner join tblclass on tblstudentclassmap.ClassId= tblclass.Id  where tblstudentclassmap.StudentId=" + _StudentId;
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql1);
                    if (m_MyReader.HasRows)
                    {
                        while (m_MyReader.Read())
                        {
                            int _ClassId = int.Parse(m_MyReader.GetValue(0).ToString());
                            int _UniversityNo = 0;
                            int _RollNo = int.Parse(m_MyReader.GetValue(1).ToString());
                            string _StudentName = m_MyReader.GetValue(2).ToString();
                            string _AdmissionNo = m_MyReader.GetValue(3).ToString();
                            string _ClassName = m_MyReader.GetValue(4).ToString();
                            iTextSharp.text.Table Tbl_CollageInformation = PER_LoadCollageDetilsToTable(_BatchName, _physicalpath);
                            iTextSharp.text.Table Tbl_StudentInformation = PER_LoadStudentDetailsToTable(_StudentId, _StudentName, _AdmissionNo, _BatchName, _ClassName, _ClassId, _RollNo, _UniversityNo, _ExamId, _Period);
                            iTextSharp.text.Table Tbl_ExamReportDetails = PER_LoadExamReportDetails(ClassId_Schduled, _CurrentBatchId, _ExamId, _StudentId, _PeriodId);
                            iTextSharp.text.Table Tbl_TotalandMaxMark = PER_LoadTotalandMaxMarkDetailsToTable(ClassId_Schduled, _CurrentBatchId, _ExamId, _StudentId, _PeriodId);
                            iTextSharp.text.Table Tbl_SignAreaDetails = PER_SignAreaToTable();

                            DocPER.Add(Tbl_CollageInformation);
                            DocPER.Add(Tbl_StudentInformation);
                            DocPER.Add(Tbl_ExamReportDetails);
                            DocPER.Add(Tbl_TotalandMaxMark);
                            DocPER.Add(Tbl_SignAreaDetails);
                            // DocPER.NewPage();

                        }
                    }
                    DocPER.Close();

                    sql = "Delete from tblexamreport ";
                    m_MysqlDb.ExecuteQuery(sql);
                    sql = "INSERT into tblexamreport(tblexamreport.ReportName) VALUES ('" + _PdfName + "')";
                    m_MysqlDb.ExecuteQuery(sql);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                if (e.Message == "The process cannot access the file")
                    _ErrorMsg = "This Bill is used by another person.Try again later";
                return false;
            }
        }


        #endregion


        #region COMMON FUNCTIONS

        private void LoadMyFont()
        {

            //NEW
            MyFonts[0] = FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD);//Title(FR

            //Collage Details
            MyFonts[1] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 15, iTextSharp.text.Font.BOLD);// CollageName  (RF
            MyFonts[2] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.NORMAL);// Collage ADDRESS (RF
            MyFonts[3] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL);// Bill and Student etails              
            MyFonts[4] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.BOLD);// Bill and Student etails    
            MyFonts[5] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD);// for the Message (QTN ,DC  ,OA ,PI,TAX INVOICE       
            //message

            //m_FntFeeItem = MyFonts[3];//empty table and Addone item functions
            //m_FntFeeItemDesc = MyFonts[4];//
            //

            //// contents

            //MyFonts[6] = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD);//Items List Heading(QTN ,DC  ,OA ,PI,TAX INVOICE  PACK LIST     
            //MyFonts[7] = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL);//Main Items List Content( QTN ,OA ,PI, TAX INVOICE         
            //MyFonts[8] = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL);//Accessory Items List Content( )

            //// contents FOR DC
            //MyFonts[9] = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD);//MAIN PRODUCT  (DC  PacKLList      
            //MyFonts[10] = FontFactory.GetFont(FontFactory.HELVETICA, 7, iTextSharp.text.Font.NORMAL);//Accessory Items ( DC, PacKLList

            //// condition
            //MyFonts[11] = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD);// Condition Heading( Qtn ,OA  ,PI,TAX INVOICE      
            //MyFonts[12] = FontFactory.GetFont(FontFactory.HELVETICA, 7, iTextSharp.text.Font.NORMAL);//Condition Content (Qtn ,OA ,PI,TAX INVOICE


            ////sign

            //MyFonts[15] = FontFactory.GetFont(FontFactory.HELVETICA, 10); // Sign AREA AREA QTN ,DC ,OA ,PI,TAX INVOICE        
            //MyFonts[16] = FontFactory.GetFont(FontFactory.HELVETICA, 8); // Sign AREA AREA QTN ,DC  ,OA ,PI,TAX INVOICE
            //MyFonts[17] = FontFactory.GetFont(FontFactory.HELVETICA, 8); // Sign AREA AREA QTN ,DC  ,OA ,PI,TAX INVOICE

            ////total amount
            //MyFonts[18] = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD);//Total Amount In Words( Qtn,OA  ,PI, TAX INVOICE            

            ////heaer and footer
            //MyFonts[19] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL, Color.BLUE);//header logo
            //MyFonts[20] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10);//header
            //MyFonts[21] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL);// for the footer 

        }

        #endregion




        #region COMBINED EXAM REPORT FOR STUDENTS

        internal bool CreateCombinedExamReportPdf(string[,] _ExamScheduleId, int _CombinedCount, int _IndCount, int _ClassId, int _BatchId, string _BatchName, string _physicalpath, out string _PdfName, out string _ErrorMsg)
        {
            _PdfName = "";
            _ErrorMsg = "";
            int _EmptyRowCount = 0;

            try
            {
                string _ClassName = "";
                sql = "SELECT tblclass.ClassName from tblclass where tblclass.Id=" + _ClassId;
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _ClassName = m_MyReader.GetValue(0).ToString();
                }
                _PdfName = "CombER_" + _ClassName + ".pdf";


                if (Delete_Existing_ExamReport(_PdfName, _physicalpath, out _ErrorMsg))
                {
                    Document DocCER = new Document(PageSize.A4, 50, 40, 20, 10);
                    PdfWriter writerCER = PdfWriter.GetInstance(DocCER, new FileStream(_physicalpath + "\\PDF_Files\\CombER_" + _ClassName + ".pdf", FileMode.Create));

                    Document DocCERTemp = new Document(PageSize.A4, 50, 40, 20, 10);
                    PdfWriter writerCERTemp = PdfWriter.GetInstance(DocCERTemp, new FileStream(_physicalpath + "\\PDF_Files\\CombERTemp_" + _ClassName + ".pdf", FileMode.Create));

                    DocCERTemp.Open();
                    DocCER.Open();

                    string sql1 = "SELECT tblstudentclassmap.StudentId, tblstudentclassmap.RollNo, tblstudent.StudentName,tblstudent.AdmitionNo from tblstudentclassmap inner JOIN tblstudent on tblstudentclassmap.StudentId= tblstudent.Id  where tblstudent.Status=1 and tblstudentclassmap.ClassId=" + _ClassId + " order by  tblstudentclassmap.RollNo";
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql1);
                    if (m_MyReader.HasRows)
                    {
                        while (m_MyReader.Read())
                        {
                            int _StudentId = int.Parse(m_MyReader.GetValue(0).ToString());
                            //int _UniversityNo = 0;
                            int _RollNo = int.Parse(m_MyReader.GetValue(1).ToString());
                            string _StudentName = m_MyReader.GetValue(2).ToString();
                            string _AdmissionNo = m_MyReader.GetValue(3).ToString();// _Batch = m_MyReader.GetValue(4).ToString();
                            iTextSharp.text.Table Tbl_CollageInformation = ER_LoadCollageDetilsToTable(_BatchName, _physicalpath);
                            iTextSharp.text.Table Tbl_StudentInformation = CER_LoadStudentDetailsToTable(_StudentName, _AdmissionNo, _BatchName, _ClassName, _RollNo);
                            iTextSharp.text.Table Tbl_SignArea = CER_SignAreaToTable(_CombinedCount, _IndCount, _physicalpath, _BatchId, _ExamScheduleId, _ClassId, _StudentId);

                            DocCERTemp.Add(Tbl_CollageInformation);
                            DocCERTemp.Add(Tbl_StudentInformation);
                            DocCERTemp.Add(Tbl_SignArea);
                            DocCERTemp = CERTemp_LoadExamReportDetails(DocCERTemp, writerCERTemp, _ExamScheduleId, _ClassId, _CombinedCount, _IndCount, _BatchId, _StudentId, out _EmptyRowCount);
                            DocCERTemp.NewPage();

                            iTextSharp.text.Table Tbl_EmptyRows = CER_EmptyRows(_EmptyRowCount);

                            DocCER.Add(Tbl_CollageInformation);
                            DocCER.Add(Tbl_StudentInformation);
                            DocCER = CER_LoadExamReportDetails(DocCER, writerCER, _ExamScheduleId, _ClassId, _CombinedCount, _IndCount, _BatchId, _StudentId, _EmptyRowCount);
                            //DocCER.Add(Tbl_EmptyRows);
                            DocCER.Add(Tbl_SignArea);
                            DocCER.NewPage();
                        }
                    }
                    DocCER.Close();

                    DocCERTemp.Close();
                    File.Delete(_physicalpath + "\\PDF_Files\\CombERTemp_" + _ClassName + ".pdf");

                    sql = "Delete from tblexamreport ";
                    m_MysqlDb.ExecuteQuery(sql);
                    sql = "INSERT into tblexamreport(tblexamreport.ReportName) VALUES ('" + _PdfName + "')";
                    m_MysqlDb.ExecuteQuery(sql);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                if (e.Message == "The process cannot access the file")
                    _ErrorMsg = "This Report is used by another person.Try again later";
                return false;
            }
        }

        private iTextSharp.text.Table CER_EmptyRows(int _EmptyRowCount)
        {
            iTextSharp.text.Table Tbl_Empty = new iTextSharp.text.Table(1);
            Tbl_Empty.Width = 100;
            Tbl_Empty.Padding = 1;
            Tbl_Empty.DefaultCell.Border = 0;

            // fill empty data to table
            for (int i = 0; i < _EmptyRowCount - 2; i++)
            {
                Tbl_Empty.AddCell(new Phrase("\n", MyFonts[4])); // Empty Row
            }
            return Tbl_Empty;
        }

        private iTextSharp.text.Table CER_SignAreaToTable(int _CombinedCount, int _IndCount, string _physicalpath, int _BatchId, string[,] _ExamScheduleId, int _ClassId, int _StudentId)
        {
            int _ColCount = _CombinedCount + _IndCount + 2;
            iTextSharp.text.Table Tbl_SignArea = new iTextSharp.text.Table(_ColCount);
            Tbl_SignArea.Width = 100;
            Tbl_SignArea.Padding = 1;
            Tbl_SignArea.DefaultCell.Border = 1;
            Tbl_SignArea.DefaultCell.BorderWidthRight = 1;

            Tbl_SignArea.DefaultCell.HorizontalAlignment = 0;
            string _StartDate = "", _EndDate = "";
            int _PResentDays = 0, _WorkingDays = 0, _AbsentDays = 0;
            string _StandardId = MyAttendence.GetStandard_Class(_ClassId);
            GetStartandEndDatesFromBatchId(out _StartDate, out _EndDate, _BatchId);

            //*********** PRESENT Days********//

            Tbl_SignArea.AddCell(new Phrase(" Present Days", MyFonts[5]));

            for (int j = 0; j < _IndCount; j++)
            {
                DateTime _ExamSchdDate = MyExamMang.ExamScheduleDateFromScheduleId(_ExamScheduleId[j, 0]);

                //_AbsentDays = MyAttendence.get_NoOf_AbsentDayForTheperiod(_StudentId, _ClassId, _StartDate,_ExamSchdDate.ToString("s"));
                //_WorkingDays = MyAttendence.get_NoOf_WorkingDaysForThePeriod(_ClassId, _StartDate,_ExamSchdDate.ToString("s"));

                _AbsentDays = MyAttendence.GetNoOf_AbsentDayForTheperiod_New(_StudentId, _StandardId, _BatchId, _StartDate, _ExamSchdDate.ToString("s"));
                _WorkingDays = MyAttendence.GetWorkingDaysForThePeriod_New(_StudentId, _StandardId, _BatchId, _StartDate, _ExamSchdDate.ToString("s"));

                _PResentDays = _WorkingDays - _AbsentDays;
                Tbl_SignArea.DefaultCell.HorizontalAlignment = 1;
                Tbl_SignArea.AddCell(new Phrase(_PResentDays.ToString(), MyFonts[5]));
            }
            for (int k = _IndCount; k < _IndCount + _CombinedCount; k++)
            {
                DateTime _ExamSchdDate = MyExamMang.ExamScheduleDateFromScheduleIdForCombinedExam(_ExamScheduleId[k, 0], _ClassId);
                _AbsentDays = MyAttendence.GetNoOf_AbsentDayForTheperiod_New(_StudentId, _StandardId, _BatchId, _StartDate, _ExamSchdDate.ToString("s"));
                _WorkingDays = MyAttendence.GetWorkingDaysForThePeriod_New(_StudentId, _StandardId, _BatchId, _StartDate, _ExamSchdDate.ToString("s"));
                _PResentDays = _WorkingDays - _AbsentDays;
                Tbl_SignArea.DefaultCell.HorizontalAlignment = 1;
                Tbl_SignArea.AddCell(new Phrase(_PResentDays.ToString(), MyFonts[5]));
            }
            _AbsentDays = MyAttendence.GetNoOf_AbsentDayForTheperiod_New(_StudentId, _StandardId, _BatchId, _StartDate, _EndDate);
            _WorkingDays = MyAttendence.GetWorkingDaysForThePeriod_New(_StudentId, _StandardId, _BatchId, _StartDate, _EndDate);
            _PResentDays = _WorkingDays - _AbsentDays;
            Tbl_SignArea.DefaultCell.HorizontalAlignment = 1;
            Tbl_SignArea.AddCell(new Phrase(_PResentDays.ToString(), MyFonts[5]));


            //********** WORKING Days*********//
            Tbl_SignArea.DefaultCell.HorizontalAlignment = 0;
            Tbl_SignArea.AddCell(new Phrase(" Working Days", MyFonts[5]));

            for (int j = 0; j < _IndCount; j++)
            {
                DateTime _ExamSchdDate = MyExamMang.ExamScheduleDateFromScheduleId(_ExamScheduleId[j, 0]);
                _WorkingDays = MyAttendence.GetWorkingDaysForThePeriod_New(_StudentId, _StandardId, _BatchId, _StartDate, _ExamSchdDate.ToString("s"));
                Tbl_SignArea.DefaultCell.HorizontalAlignment = 1;
                Tbl_SignArea.AddCell(new Phrase(_WorkingDays.ToString(), MyFonts[5]));
            }
            for (int k = _IndCount; k < _IndCount + _CombinedCount; k++)
            {
                DateTime _ExamSchdDate = MyExamMang.ExamScheduleDateFromScheduleIdForCombinedExam(_ExamScheduleId[k, 0], _ClassId);
                _WorkingDays = MyAttendence.GetWorkingDaysForThePeriod_New(_StudentId, _StandardId, _BatchId, _StartDate, _ExamSchdDate.ToString("s"));
                Tbl_SignArea.DefaultCell.HorizontalAlignment = 1;
                Tbl_SignArea.AddCell(new Phrase(_WorkingDays.ToString(), MyFonts[5]));
            }
            _WorkingDays = MyAttendence.GetWorkingDaysForThePeriod_New(_StudentId, _StandardId, _BatchId, _StartDate, _EndDate);
            Tbl_SignArea.DefaultCell.HorizontalAlignment = 1;
            Tbl_SignArea.AddCell(new Phrase(_WorkingDays.ToString(), MyFonts[5]));


            //****REMARKS********//   
            Tbl_SignArea.DefaultCell.HorizontalAlignment = 0;
            Tbl_SignArea.AddCell(new Phrase(" Remarks", MyFonts[5]));

            for (int j = 0; j < _IndCount + _CombinedCount + 1; j++)
            {
                Tbl_SignArea.AddCell(new Phrase("\n\n\n", MyFonts[5]));
            }

            //******Principal*******//

            string _SignUrl = "";
            Tbl_SignArea.DefaultCell.HorizontalAlignment = 0;
            Tbl_SignArea.AddCell(new Phrase(" Principal's Sign", MyFonts[5]));

            if (IsAutoPrincipalSign(out _SignUrl))
            {
                if (_SignUrl != "")
                {
                    string _ImagePathAndName = _physicalpath + _SignUrl;

                    if (File.Exists(_ImagePathAndName))
                    {
                        iTextSharp.text.Image _ImgHeader = iTextSharp.text.Image.GetInstance(_ImagePathAndName);
                        _ImgHeader.ScaleAbsolute(40, 35);
                        Tbl_SignArea.DefaultCell.HorizontalAlignment = 0;

                        Cell cell = new Cell();
                        cell.Add(_ImgHeader);

                        for (int j = 0; j < _IndCount + _CombinedCount + 1; j++)
                        {
                            Tbl_SignArea.DefaultCell.HorizontalAlignment = 1;
                            Tbl_SignArea.AddCell(cell);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < _IndCount + _CombinedCount + 1; j++)
                        {
                            Tbl_SignArea.AddCell(new Phrase("\n\n\n", MyFonts[5]));
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < _IndCount + _CombinedCount + 1; j++)
                    {
                        Tbl_SignArea.AddCell(new Phrase("\n\n\n", MyFonts[5]));
                    }
                }
            }
            else
            {
                for (int j = 0; j < _IndCount + _CombinedCount + 1; j++)
                {
                    Tbl_SignArea.AddCell(new Phrase("\n\n\n", MyFonts[5]));
                }
            }

            //******Class Teacher*******//
            Tbl_SignArea.DefaultCell.HorizontalAlignment = 0;
            Tbl_SignArea.AddCell(new Phrase(" Class Teacher's Sign", MyFonts[5]));
            for (int j = 0; j < _IndCount + _CombinedCount + 1; j++)
            {
                Tbl_SignArea.AddCell(new Phrase("\n\n\n", MyFonts[5]));
            }

            //******Parent*******//
            Tbl_SignArea.DefaultCell.HorizontalAlignment = 0;
            Tbl_SignArea.AddCell(new Phrase(" Parent's Sign", MyFonts[5]));
            for (int j = 0; j < _IndCount + _CombinedCount + 1; j++)
            {
                Tbl_SignArea.AddCell(new Phrase("\n\n\n", MyFonts[5]));
            }

            //******Result*******//
            Tbl_SignArea.DefaultCell.HorizontalAlignment = 0;
            Tbl_SignArea.AddCell(new Phrase(" RESULT", MyFonts[5]));
            for (int j = 0; j < _IndCount + _CombinedCount + 1; j++)
            {
                Tbl_SignArea.AddCell(new Phrase("\n\n\n", MyFonts[5]));
            }

            return Tbl_SignArea;
        }

        private Document CER_LoadExamReportDetails(Document DocCER, PdfWriter writerCER, string[,] _ExamScheduleId, int _ClassId, int _CombinedCount, int _IndCount, int _BatchId, int _StudentId, int _EmptyRowCount)
        {
            int _SubjectCount = 0;
            int _ColumCount = _CombinedCount + _IndCount + 2;
            iTextSharp.text.Table Tbl_ExamReportDetails = new iTextSharp.text.Table(_ColumCount);
            Tbl_ExamReportDetails.Width = 100;
            Tbl_ExamReportDetails.Padding = 1;
            Tbl_ExamReportDetails.DefaultCell.Border = 1;
            Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
            Tbl_ExamReportDetails.DefaultCell.BorderWidthRight = 1;
            Tbl_ExamReportDetails.DefaultCell.BorderWidthBottom = 1;
            Tbl_ExamReportDetails.AddCell(new Phrase("Subject", MyFonts[4])); // Sub Name

            for (int i = 0; i < _CombinedCount + _IndCount; i++)
            {
                Tbl_ExamReportDetails.AddCell(new Phrase(_ExamScheduleId[i, 2], MyFonts[4]));
            }
            Tbl_ExamReportDetails.AddCell(new Phrase("Cumulative Performance", MyFonts[4])); // Cumulative

            string _Grade = "";
            string sql = "select distinct tblsubjects.subject_name, tblexammark.SubjectId, tblexammark.MarkColumn from tblexamschedule inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblexamschedule.ClassExamId inner JOIN tblexammark on tblexammark.ExamSchId= tblexamschedule.Id AND tblclassexamsubmap.SubId= tblexammark.SubjectId inner join tblsubjects on tblsubjects.Id= tblexammark.SubjectId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId and tblstudentclassmap.ClassId=" + _ClassId + " and tblstudentclassmap.BatchId =" + _BatchId + " where tblexamschedule.BatchId=" + _BatchId + " order by tblexammark.SubjectOrder";
            m_MyReader3 = m_MysqlDb.ExecuteQuery(sql);
            _SubjectCount = m_MyReader3.RecordsAffected;

            if (m_MyReader3.HasRows)
            {
                while (m_MyReader3.Read())
                {
                    double _CumulativeTotal = 0, _CumulativeTotalMax = 0, _IndTotal = 0, _IndTotalMax = 0, _CombineToal = 0, _CombineTotalMax = 0;
                    Tbl_ExamReportDetails.DefaultCell.BorderWidthBottom = 0;
                    Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 0;
                    Tbl_ExamReportDetails.DefaultCell.BorderWidthRight = 1;

                    Tbl_ExamReportDetails.AddCell(new Phrase(" " + m_MyReader3.GetValue(0).ToString(), MyFonts[5]));

                    for (int j = 0; j < _IndCount; j++)
                    {
                        _Grade = GetGradeForIndividualExam(_StudentId, _ExamScheduleId[j, 0], int.Parse(m_MyReader3.GetValue(1).ToString()), _BatchId, out _IndTotal, out _IndTotalMax);
                        Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_ExamReportDetails.AddCell(new Phrase(_Grade, MyFonts[5]));
                        if (_IndTotal > -1)
                        {
                            _CumulativeTotal = _CumulativeTotal + _IndTotal * (double.Parse(_ExamScheduleId[j, 3]) / 100);
                        }
                        _CumulativeTotalMax = _CumulativeTotalMax + _IndTotalMax * (double.Parse(_ExamScheduleId[j, 3]) / 100);
                    }
                    for (int k = _IndCount; k < _IndCount + _CombinedCount; k++)
                    {
                        _Grade = GetGradeForCombinedExam(_StudentId, _ExamScheduleId[k, 0], _ClassId, int.Parse(m_MyReader3.GetValue(1).ToString()), _BatchId, out _CombineToal, out _CombineTotalMax);
                        Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_ExamReportDetails.AddCell(new Phrase(_Grade, MyFonts[5]));
                        if (_CombineToal > -1)
                        {
                            _CumulativeTotal = _CumulativeTotal + _CombineToal * (double.Parse(_ExamScheduleId[k, 3]) / 100);
                        }
                        _CumulativeTotalMax = _CumulativeTotalMax + _CombineTotalMax * (double.Parse(_ExamScheduleId[k, 3]) / 100);
                    }
                    if (_CumulativeTotalMax > 0)
                    {
                        _Grade = MyExamMang.GetGrade(_CumulativeTotal, _CumulativeTotalMax,0);
                    }
                    else
                    {
                        _Grade = "--";
                    }
                    Tbl_ExamReportDetails.AddCell(new Phrase(_Grade, MyFonts[5])); // CP
                    if (writerCER.FitsPage(Tbl_ExamReportDetails))
                    {
                    }

                    else
                    {
                        Tbl_ExamReportDetails.DeleteLastRow();
                        DocCER.Add(Tbl_ExamReportDetails);
                        Tbl_ExamReportDetails.DeleteAllRows();

                        Tbl_ExamReportDetails.DefaultCell.BorderWidthBottom = 1;
                        Tbl_ExamReportDetails.DefaultCell.BorderWidthTop = 1;
                        Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_ExamReportDetails.AddCell(new Phrase("Subject", MyFonts[4])); // Sub Name
                        DocCER.NewPage();

                        for (int i = 0; i < _CombinedCount + _IndCount; i++)
                        {
                            Tbl_ExamReportDetails.AddCell(new Phrase(_ExamScheduleId[i, 2], MyFonts[4]));
                        }
                        Tbl_ExamReportDetails.AddCell(new Phrase("Cumulative Performance", MyFonts[4])); // Cumulative
                        _CumulativeTotal = 0; _CumulativeTotalMax = 0; _IndTotal = 0; _IndTotalMax = 0; _CombineToal = 0; _CombineTotalMax = 0;
                        Tbl_ExamReportDetails.DefaultCell.Border = 1;
                        Tbl_ExamReportDetails.DefaultCell.BorderWidthRight = 1;
                        Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 0;
                        Tbl_ExamReportDetails.AddCell(new Phrase(" " + m_MyReader3.GetValue(0).ToString(), MyFonts[5]));

                        for (int j = 0; j < _IndCount; j++)
                        {
                            _Grade = GetGradeForIndividualExam(_StudentId, _ExamScheduleId[j, 0], int.Parse(m_MyReader3.GetValue(1).ToString()), _BatchId, out _IndTotal, out _IndTotalMax);
                            Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                            Tbl_ExamReportDetails.AddCell(new Phrase(_Grade, MyFonts[5]));
                            if (_IndTotal > -1)
                            {
                                _CumulativeTotal = _CumulativeTotal + _IndTotal * (double.Parse(_ExamScheduleId[j, 3]) / 100);
                            }
                            _CumulativeTotalMax = _CumulativeTotalMax + _IndTotalMax * (double.Parse(_ExamScheduleId[j, 3]) / 100);
                        }
                        for (int k = _IndCount; k < _IndCount + _CombinedCount; k++)
                        {
                            _Grade = GetGradeForCombinedExam(_StudentId, _ExamScheduleId[k, 0], _ClassId, int.Parse(m_MyReader3.GetValue(1).ToString()), _BatchId, out _CombineToal, out _CombineTotalMax);
                            Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                            Tbl_ExamReportDetails.AddCell(new Phrase(_Grade, MyFonts[5]));
                            if (_CombineToal > -1)
                            {
                                _CumulativeTotal = _CumulativeTotal + _CombineToal * (double.Parse(_ExamScheduleId[k, 3]) / 100);
                            }
                            _CumulativeTotalMax = _CumulativeTotalMax + _CombineTotalMax * (double.Parse(_ExamScheduleId[k, 3]) / 100);
                        }
                        if (_CumulativeTotalMax > 0)
                        {
                            _Grade = MyExamMang.GetGrade(_CumulativeTotal, _CumulativeTotalMax,0);
                        }
                        else
                        {
                            _Grade = "--";
                        }
                        Tbl_ExamReportDetails.AddCell(new Phrase(_Grade, MyFonts[5])); // CP
                    }
                }
            }
            //for (int i = 0; i < _EmptyRowCount - 3; i++)
            //{
            //    for (int a = 0; a < _ColumCount; a++)
            //    {
            //        Tbl_ExamReportDetails.DefaultCell.BorderWidthBottom = 0;
            //        Tbl_ExamReportDetails.DefaultCell.BorderWidthRight = 0;
            //        Tbl_ExamReportDetails.AddCell(new Phrase("\n", MyFonts[3]));
            //    }
            //} 
            DocCER.Add(Tbl_ExamReportDetails);
            return DocCER;
        }

        private void GetStartandEndDatesFromBatchId(out string _StartDate, out string _EndDate, int _BatchId)
        {
            _StartDate = "";
            _EndDate = "";
            string sql = "SELECT tblbatch.StartDate, tblbatch.EndDate from tblbatch where tblbatch.Id=" + _BatchId;
            m_MyReader6 = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader6.HasRows)
            {
                _StartDate = (DateTime.Parse(m_MyReader6.GetValue(0).ToString()).ToString("s"));
                _EndDate = (DateTime.Parse(m_MyReader6.GetValue(1).ToString()).ToString("s"));
            }
        }

        private bool IsAutoPrincipalSign(out string _SignUrl)
        {
            bool _AutoSign = false;
            _SignUrl = "";
            string sql = "SELECT tblconfiguration.Value,tblconfiguration.SubValue from tblconfiguration where tblconfiguration.Name='AutoPrincipalSign'";
            m_MyReader6 = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader6.HasRows)
            {
                if (m_MyReader6.GetValue(0).ToString() == "1")
                {
                    _AutoSign = true;
                }
                _SignUrl = m_MyReader6.GetValue(1).ToString();
            }
            return _AutoSign;
        }

        private Document CERTemp_LoadExamReportDetails(Document DocCERTemp, PdfWriter writerCERTemp, string[,] _ExamScheduleId, int _ClassId, int _CombinedCount, int _IndCount, int _BatchId, int _StudentId, out int _EmptyRowCount)
        {
            int _SubjectCount = 0;
            int _ColumCount = _CombinedCount + _IndCount + 2;
            iTextSharp.text.Table Tbl_ExamReportDetails = new iTextSharp.text.Table(_ColumCount);
            Tbl_ExamReportDetails.Width = 100;
            Tbl_ExamReportDetails.Padding = 1;
            Tbl_ExamReportDetails.DefaultCell.Border = 0;
            Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
            Tbl_ExamReportDetails.DefaultCell.BorderWidthRight = 1;
            Tbl_ExamReportDetails.DefaultCell.BorderWidthBottom = 1;
            Tbl_ExamReportDetails.AddCell(new Phrase("Subject", MyFonts[4])); // Sub Name

            for (int i = 0; i < _CombinedCount + _IndCount; i++)
            {
                Tbl_ExamReportDetails.AddCell(new Phrase(_ExamScheduleId[i, 2], MyFonts[4]));
            }
            Tbl_ExamReportDetails.AddCell(new Phrase("Cumulative Performance", MyFonts[4])); // Sub Name

            string _Grade = "";
            string sql = "select distinct tblsubjects.subject_name, tblexammark.SubjectId, tblexammark.MarkColumn from  tblexammark inner join tblsubjects on tblexammark.SubjectId= tblsubjects.Id where tblexammark.ExamSchId in(SELECT tblexamschedule.Id from tblexamschedule where tblexamschedule.ClassExamId in (select tblclassexam.Id from tblclassexam WHERE tblclassexam.ClassId=" + _ClassId + ")) order by tblexammark.SubjectOrder";
            m_MyReader3 = m_MysqlDb.ExecuteQuery(sql);
            _SubjectCount = m_MyReader3.RecordsAffected;
            if (m_MyReader3.HasRows)
            {
                while (m_MyReader3.Read())
                {
                    double _IndTotal = 0, _IndTotalMax = 0, _CombineTotal = 0, _CombineTotalMax = 0;
                    Tbl_ExamReportDetails.DefaultCell.BorderWidthBottom = 0;
                    Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 0;
                    Tbl_ExamReportDetails.DefaultCell.BorderWidthRight = 1;

                    Tbl_ExamReportDetails.AddCell(new Phrase(" " + m_MyReader3.GetValue(0).ToString(), MyFonts[5]));

                    for (int j = 0; j < _IndCount; j++)
                    {
                        _Grade = GetGradeForIndividualExam(_StudentId, _ExamScheduleId[j, 0], int.Parse(m_MyReader3.GetValue(1).ToString()), _BatchId, out _IndTotal, out _IndTotalMax);
                        Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_ExamReportDetails.AddCell(new Phrase(_Grade, MyFonts[5]));
                    }
                    for (int k = _IndCount; k < _IndCount + _CombinedCount; k++)
                    {
                        _Grade = GetGradeForCombinedExam(_StudentId, _ExamScheduleId[k, 0], _ClassId, int.Parse(m_MyReader3.GetValue(1).ToString()), _BatchId, out _CombineTotal, out _CombineTotalMax);
                        Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_ExamReportDetails.AddCell(new Phrase(_Grade, MyFonts[5]));
                    }
                    Tbl_ExamReportDetails.AddCell(new Phrase("CP", MyFonts[5]));
                    if (writerCERTemp.FitsPage(Tbl_ExamReportDetails))
                    {
                    }

                    else
                    {
                        Tbl_ExamReportDetails.DeleteLastRow();
                        DocCERTemp.Add(Tbl_ExamReportDetails);
                        Tbl_ExamReportDetails.DeleteAllRows();

                        Tbl_ExamReportDetails.DefaultCell.BorderWidthBottom = 1;
                        Tbl_ExamReportDetails.DefaultCell.BorderWidthTop = 1;
                        Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_ExamReportDetails.AddCell(new Phrase("Subject", MyFonts[4])); // Sub Name
                        DocCERTemp.NewPage();

                        for (int i = 0; i < _CombinedCount + _IndCount; i++)
                        {
                            Tbl_ExamReportDetails.AddCell(new Phrase(_ExamScheduleId[i, 2], MyFonts[4]));
                        }
                        Tbl_ExamReportDetails.AddCell(new Phrase("Cumulative Performance", MyFonts[4])); // Sub Name

                        Tbl_ExamReportDetails.DefaultCell.BorderWidthBottom = 0;
                        Tbl_ExamReportDetails.DefaultCell.BorderWidthTop = 0;

                        Tbl_ExamReportDetails.AddCell(new Phrase(" " + m_MyReader3.GetValue(0).ToString(), MyFonts[5]));

                        for (int j = 0; j < _IndCount; j++)
                        {
                            _Grade = GetGradeForIndividualExam(_StudentId, _ExamScheduleId[j, 0], int.Parse(m_MyReader3.GetValue(1).ToString()), _BatchId, out _IndTotal, out _IndTotalMax);
                            Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                            Tbl_ExamReportDetails.AddCell(new Phrase(_Grade, MyFonts[5]));
                        }
                        for (int k = _IndCount; k < _IndCount + _CombinedCount; k++)
                        {
                            _Grade = GetGradeForCombinedExam(_StudentId, _ExamScheduleId[k, 0], _ClassId, int.Parse(m_MyReader3.GetValue(1).ToString()), _BatchId, out _CombineTotal, out _CombineTotalMax);
                            Tbl_ExamReportDetails.DefaultCell.HorizontalAlignment = 1;
                            Tbl_ExamReportDetails.AddCell(new Phrase(_Grade, MyFonts[5]));
                        }
                        Tbl_ExamReportDetails.AddCell(new Phrase("CP", MyFonts[5]));
                    }
                }
            }
            _EmptyRowCount = 0;
            while (writerCERTemp.FitsPage(Tbl_ExamReportDetails))
            {
                for (int a = 0; a < _ColumCount; a++)
                {
                    Tbl_ExamReportDetails.AddCell(new Phrase("\n", MyFonts[5]));
                }

                _EmptyRowCount++;
            }
            DocCERTemp.Add(Tbl_ExamReportDetails);
            return DocCERTemp;
        }

        private string GetGradeForCombinedExam(int _StudentId, string _ExamId, int _ClassId, int _SubjectId, int _BatchId, out double _CombineToal, out double _CombineTotalMax)
        {
            OdbcDataReader m_MyReaderGrade = null;
            string _Grade = "", _MarkColumn = "";
            _CombineToal = 0; _CombineTotalMax = 0;
            double _Total = 0, _TotalMax = 0, _TotalAll = 0, _TotalMaxAll = 0;
            string sql = "SELECT tblexammark.MarkColumn, tblexammark.ExamSchId from tblexammark where tblexammark.SubjectId=" + _SubjectId + " and tblexammark.ExamSchId in(select tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexamcombmap on tblexamcombmap.ExamId= tblclassexam.ExamId where tblexamschedule.PeriodId= tblexamcombmap.PeriodId and tblclassexam.ClassId= tblexamcombmap.ClassId AND tblexamcombmap.ClassId=" + _ClassId + " and  tblexamcombmap.CombinedId=" + _ExamId + ") order by tblexammark.SubjectOrder" ;
            m_MyReaderGrade = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReaderGrade.HasRows)
            {
                while (m_MyReaderGrade.Read())
                {
                    _MarkColumn = m_MyReaderGrade.GetValue(0).ToString();
                    double.TryParse(MyExamMang.GetMarkfromColumn(_StudentId, int.Parse(m_MyReaderGrade.GetValue(1).ToString()), _BatchId, _MarkColumn), out _Total);
                    if (_Total > -1)
                    {
                        _TotalAll = _TotalAll + _Total;
                    }
                }

                sql = " SELECT tblclassexamsubmap.MaxMark from tblclassexamsubmap inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexamsubmap.ClassExamId where tblclassexamsubmap.SubId=" + _SubjectId + " and tblexamschedule.Id in(select tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexamcombmap on tblexamcombmap.ExamId= tblclassexam.ExamId where tblexamschedule.PeriodId= tblexamcombmap.PeriodId and tblclassexam.ClassId= tblexamcombmap.ClassId AND tblexamcombmap.ClassId=" + _ClassId + " and  tblexamcombmap.CombinedId=" + _ExamId + ")";
                m_MyReaderGrade = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReaderGrade.HasRows)
                {
                    while (m_MyReaderGrade.Read())
                    {
                        double.TryParse(m_MyReaderGrade.GetValue(0).ToString(), out _TotalMax);
                        _TotalMaxAll = _TotalMaxAll + _TotalMax;
                    }
                }
                _CombineToal = _TotalAll;
                _CombineTotalMax = _TotalMaxAll;
                _Grade = MyExamMang.GetGrade(_TotalAll, _TotalMaxAll,0);

            }
            else
            {
                _Grade = "--";
            }

            return _Grade;
        }

        private string GetGradeForIndividualExam(int _StudentId, string _ExamId, int _SubjectId, int _BatchId, out double _IndTotal, out double _IndTotalMax)
        {
            OdbcDataReader m_MyReaderGrade = null;
            _IndTotal = 0; _IndTotalMax = 0;
            string _Grade = "", _MarkColumn = "";
            double _Total = 0, _TotalMax = 0;
            string sql = "SELECT tblexammark.MarkColumn from tblexammark where tblexammark.ExamSchId=" + _ExamId + " and tblexammark.SubjectId=" + _SubjectId+" order by tblexammark.SubjectOrder";
            m_MyReaderGrade = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReaderGrade.HasRows)
            {
                _MarkColumn = m_MyReaderGrade.GetValue(0).ToString();
                double.TryParse(MyExamMang.GetMarkfromColumn(_StudentId, int.Parse(_ExamId), _BatchId, _MarkColumn), out _Total);

                sql = " SELECT tblclassexamsubmap.MaxMark from tblclassexamsubmap inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexamsubmap.ClassExamId where tblclassexamsubmap.SubId=" + _SubjectId + " and tblexamschedule.Id=" + _ExamId;
                m_MyReaderGrade = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReaderGrade.HasRows)
                {
                    double.TryParse(m_MyReaderGrade.GetValue(0).ToString(), out _TotalMax);
                }
                if (_Total > -1)
                {
                    _IndTotal = _Total;
                }
                _IndTotalMax = _TotalMax;
                _Grade = MyExamMang.GetGrade(_Total, _TotalMax,0);
            }
            else
            {
                _Grade = "--";
            }

            return _Grade;
        }

        private iTextSharp.text.Table CER_LoadStudentDetailsToTable(string _StudentName, string _AdmissionNo, string _BatchName, string _ClassName, int _RollNo)
        {
            iTextSharp.text.Table Tbl_Details = new iTextSharp.text.Table(5);
            Tbl_Details.Width = 100;
            float[] headerwidths = { 15, 20, 30, 15, 20 };
            Tbl_Details.Widths = headerwidths;
            Tbl_Details.Padding = 1;
            Tbl_Details.DefaultCell.Border = 0;

            // fill data to table
            // first row
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("     Student Name  : ", MyFonts[3])); // Name
            Tbl_Details.DefaultCell.Colspan = 2;
            Tbl_Details.AddCell(new Phrase(_StudentName, MyFonts[4]));// bold
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("  Roll No  : ", MyFonts[3])); //Bil; date
            Tbl_Details.AddCell(new Phrase(_RollNo.ToString(), MyFonts[4]));

            // Second row
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("     Batch  : ", MyFonts[3])); //Addmission No
            Tbl_Details.DefaultCell.Colspan = 2;
            Tbl_Details.AddCell(new Phrase(_BatchName, MyFonts[4]));
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("  Class  : ", MyFonts[3])); //Class name
            Tbl_Details.AddCell(new Phrase(_ClassName.ToString(), MyFonts[4]));

            //empty row
            Tbl_Details.DefaultCell.Colspan = 5;
            Tbl_Details.AddCell(new Phrase("\n", MyFonts[3]));//empty

            return Tbl_Details;
        }

        #endregion
    }
}
