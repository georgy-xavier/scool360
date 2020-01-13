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

namespace WinEr
{
    public class TCpdf
    {
        private Font[] MyFonts = new Font[24];
        public MysqlClass m_MysqlDb;
        private OdbcDataReader m_MyReader = null;

        private SchoolClass m_objSchool = null;
        private DataSet m_MyDataSet = null;

        private string sql = "";
        private string m_NullStr = "NULL";
        private string _YourRef = "Asper your enquiry";
        private string m_AmtType = "RS";
        private string m_PdfType = "";

        public TCpdf(MysqlClass _Mysqlobj, SchoolClass objSchool)
        {
            m_objSchool = objSchool;
            m_MysqlDb = _Mysqlobj;
            LoadMyFont();
        }

        ~TCpdf()
        {
            if (m_MysqlDb != null)
            {
                m_MysqlDb = null;
            }
            if (m_MyReader != null)
            {
                m_MyReader = null;

            }
        }


        #region STUDENT TC

        internal bool GenerateStudentTCPdf(int _StudentId, string _physicalpath, out string _PdfName, out string _ErrorMsg)
        {
            _PdfName = "";
            _ErrorMsg = "";

            try
            {
                 StringBuilder _TC = new StringBuilder();
            string sql = "select tbltcformat.TCFormat from tbltcformat ";
            OdbcDataReader MyReader = m_MysqlDb.ExecuteQuery(sql);

            if (!MyReader.HasRows)
            {
                //dominic show error mseeage
               
                _ErrorMsg = "Default TC format does not found";
                return false;
                
            }
            else
            {
                _TC.Append(MyReader.GetValue(0).ToString());

                string _StudentName = "";
                sql = "SELECT tblview_student.StudentName from tblview_student where tblview_student.Id=" + _StudentId;
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _StudentName = m_MyReader.GetValue(0).ToString();
                }
                _PdfName = "TC_" + _StudentName + ".pdf";


                if (Delete_Existing_ExamReport(_PdfName, _physicalpath, out _ErrorMsg))
                {
                    Document DocTC = new Document(PageSize.A4, 50, 40, 20, 10);
                    PdfWriter writerTC = PdfWriter.GetInstance(DocTC, new FileStream(_physicalpath + "\\PDF_Files\\TC_" + _StudentName + ".pdf", FileMode.Create));

                    Document DocTCTemp = new Document(PageSize.A4, 50, 40, 20, 10);
                    PdfWriter writerTCTemp = PdfWriter.GetInstance(DocTCTemp, new FileStream(_physicalpath + "\\PDF_Files\\TCTemp_" + _StudentName + ".pdf", FileMode.Create));

                    DocTC.Open();
                    DocTCTemp.Open();

                    iTextSharp.text.Table Tbl_CollageInformation = TC_LoadCollageDetilsToTable(_StudentId, _physicalpath);
                    iTextSharp.text.Table Tbl_StudentTCDetails = TCTemp_LoadStudentDetailsToTable(_StudentId);
                    iTextSharp.text.Table Tbl_SignAndSeal = TC_SignandSealArea();
                    int _EmptyRowCount = 1;
                    DocTCTemp.Add(Tbl_CollageInformation);
                    DocTCTemp.Add(Tbl_StudentTCDetails);
                    DocTCTemp.Add(Tbl_SignAndSeal);

                    DocTCTemp = TCTemp_LoadStudentDetails(ref DocTCTemp, ref writerTCTemp, _StudentId, out _EmptyRowCount);

                    iTextSharp.text.Table Tbl_StudentTCDetailsFull = TC_LoadStudentDetailsToTableFull(_StudentId, _EmptyRowCount);

                    DocTC.Add(Tbl_CollageInformation);
                    DocTC.Add(Tbl_StudentTCDetailsFull);
                    DocTC.Add(Tbl_SignAndSeal);

                    DocTC.Close();
                    DocTCTemp.Close();

                    File.Delete(_physicalpath + "\\PDF_Files\\TCTemp_" + _StudentName + ".pdf");
                    sql = "Delete from tblcreatedtc ";
                    m_MysqlDb.ExecuteQuery(sql);
                    sql = "INSERT into tblcreatedtc(tblcreatedtc.TcName) VALUES ('" + _PdfName + "')";
                    m_MysqlDb.ExecuteQuery(sql);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            }
            catch (Exception e)
            {
                if (e.Message == "The process cannot access the file")
                    _ErrorMsg = "This Bill is used by another person.Try again later";
                return false;
            }

        }

        private iTextSharp.text.Table TC_LoadStudentDetailsToTableFull(int _StudentId, int _EmptyRowCount)
        {
            iTextSharp.text.Table Tbl_Details = new iTextSharp.text.Table(2);
            Tbl_Details.Width = 100;
            float[] headerwidths = { 50, 50 };
            Tbl_Details.Widths = headerwidths;
            Tbl_Details.Padding = 1;
            Tbl_Details.DefaultCell.Border = 0;

            DateTime Date_Dob, Date_LastAttendance, Date_TcRcvedDate, Date_TCIssue;//, Date_DteOFAdmission;

            String Sql = "SELECT * FROM tbltc WHERE StudentId=" + _StudentId;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {

                Tbl_Details.AddCell(new Phrase("  Name of the School", MyFonts[3])); //School Name
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(4).ToString(), MyFonts[4])); //School Name

                Tbl_Details.AddCell(new Phrase("  Admission No.", MyFonts[3])); //Admission No
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(5).ToString(), MyFonts[4])); //Admission No

                Tbl_Details.AddCell(new Phrase("  Cumulative Record No.", MyFonts[3])); //Record No
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(6).ToString(), MyFonts[4])); //Record No

                Tbl_Details.AddCell(new Phrase("  Name of the Pupil", MyFonts[3])); //Pupil Name
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(3).ToString(), MyFonts[4])); //Pupil Name

                Tbl_Details.AddCell(new Phrase("  Sex", MyFonts[3])); //Sex
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(7).ToString(), MyFonts[4])); //Sex

                Tbl_Details.AddCell(new Phrase("  Name of Father", MyFonts[3])); //Father Name
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(8).ToString(), MyFonts[4])); //Father Name

                Tbl_Details.AddCell(new Phrase("  Name of Mother", MyFonts[3])); //Father Name
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(33).ToString(), MyFonts[4])); //Father Name




                string DOB = "";
              
                if (DateTime.TryParse(m_MyReader.GetValue(13).ToString(), out Date_Dob))
                {
                    DOB = General.GerFormatedDatVal(Date_Dob);
                }


                Date_Dob = DateTime.Parse(m_MyReader.GetValue(13).ToString());
                Tbl_Details.AddCell(new Phrase("  Date of Birth", MyFonts[3]));//Date of Birth
                Tbl_Details.AddCell(new Phrase(DOB, MyFonts[4]));//Date of Birth

                bool includeTime = false, UK = true;

                string DOBinWords = General.DateToText(Date_Dob, includeTime, UK);
                Tbl_Details.AddCell(new Phrase("  In Words", MyFonts[3]));//Date of Birth
                Tbl_Details.AddCell(new Phrase(DOBinWords, MyFonts[4]));//Date of Birth

                Tbl_Details.AddCell(new Phrase("  Address", MyFonts[3])); //Father Name
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(34).ToString(), MyFonts[4])); //Father Name

                Tbl_Details.AddCell(new Phrase("  Nationality", MyFonts[3]));//Nationality
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(9).ToString(), MyFonts[4]));//Nationality

                Tbl_Details.AddCell(new Phrase("  Religion", MyFonts[3]));//Religion
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(10).ToString(), MyFonts[4]));//Religion

                Tbl_Details.AddCell(new Phrase("  Caste", MyFonts[3]));//cast
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(11).ToString(), MyFonts[4]));//cast

                Tbl_Details.AddCell(new Phrase("  SC or ST", MyFonts[3]));//sc\st
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(12).ToString(), MyFonts[4]));//sc\st

                Tbl_Details.AddCell(new Phrase("  Standard at the time of leaving the school", MyFonts[3]));//std
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(14).ToString(), MyFonts[4]));//std
               
                DateTime LastClassDate = DateTime.Now;

                string ADate1 = "";
                if (DateTime.TryParse(m_MyReader.GetValue(39).ToString(), out LastClassDate))
                {
                    ADate1 = General.GerFormatedDatVal(LastClassDate);

                    Tbl_Details.AddCell(new Phrase("  Date of admission / promotion to that class ", MyFonts[3]));//std
                    Tbl_Details.AddCell(new Phrase(ADate1, MyFonts[4]));//std
                }





                string languages = m_MyReader.GetValue(15).ToString().Replace("\r\n", ",");
                  
                Tbl_Details.AddCell(new Phrase("  For  higher standard Pupil :Language studied", MyFonts[3]));//language
                Tbl_Details.AddCell(new Phrase(languages, MyFonts[4]));//language
               
                string subjects = m_MyReader.GetValue(36).ToString().Replace("\r\n", ",");
                Tbl_Details.AddCell(new Phrase("  Subject offered ", MyFonts[3]));//language
                Tbl_Details.AddCell(new Phrase(subjects, MyFonts[4]));//language
                

                Tbl_Details.AddCell(new Phrase("  Medium of Instruction", MyFonts[3]));//medium
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(16).ToString(), MyFonts[4]));//medium

                Tbl_Details.AddCell(new Phrase("  Syllabus", MyFonts[3]));//Syllabus
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(17).ToString(), MyFonts[4]));//Syllabus

                DateTime AdmissionDate = DateTime.Now;

                string ADate = "";
                if (DateTime.TryParse(m_MyReader.GetValue(18).ToString(), out AdmissionDate))
                {
                    ADate = General.GerFormatedDatVal(AdmissionDate);
                }


                Tbl_Details.AddCell(new Phrase("  Date of Admission", MyFonts[3]));//Date of Admission 
                Tbl_Details.AddCell(new Phrase(ADate, MyFonts[4]));//Date of Admission 

                Tbl_Details.AddCell(new Phrase("  Whether qualified for Promotion to a higher standard", MyFonts[3]));//promotion
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(19).ToString(), MyFonts[4]));//promotion

                Tbl_Details.AddCell(new Phrase("  Whether the pupil has paid all the fees due to the School", MyFonts[3]));//fee paid
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(20).ToString(), MyFonts[4]));//fee paid

                Tbl_Details.AddCell(new Phrase("  Fee concessions if any (Nature & period to be specified)", MyFonts[3]));//fee concession
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(21).ToString(), MyFonts[4]));//fee concession

                Tbl_Details.AddCell(new Phrase("  Scholarships, if any  (Nature & period to be specified)", MyFonts[3]));//scholarship
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(22).ToString(), MyFonts[4]));//scholarship

                Tbl_Details.AddCell(new Phrase("  Whether medically Examined or not", MyFonts[3]));//medical
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(23).ToString(), MyFonts[4]));//medical


                string LDate = "";
               
                if (DateTime.TryParse(m_MyReader.GetValue(24).ToString(), out Date_LastAttendance))
                {
                    LDate = General.GerFormatedDatVal(Date_LastAttendance);
                }



                Date_LastAttendance = DateTime.Parse(m_MyReader.GetValue(24).ToString());

                Tbl_Details.AddCell(new Phrase("  Date of pupil's last attendance at school", MyFonts[3]));//last attendance
                Tbl_Details.AddCell(new Phrase(LDate, MyFonts[4]));//last attendance

                string RDate = "";

                if (DateTime.TryParse(m_MyReader.GetValue(25).ToString(), out Date_TcRcvedDate))
                {
                    RDate = General.GerFormatedDatVal(Date_TcRcvedDate);
                }

                Date_TcRcvedDate = DateTime.Parse(m_MyReader.GetValue(25).ToString());
                Tbl_Details.AddCell(new Phrase("  Date on which the application for the T C was received", MyFonts[3]));//tc date
                Tbl_Details.AddCell(new Phrase(RDate, MyFonts[4]));//tc date


                string IDate = "";

                if (DateTime.TryParse(m_MyReader.GetValue(26).ToString(), out Date_TCIssue))
                {
                    IDate = General.GerFormatedDatVal(Date_TCIssue);
                }


                
                Date_TCIssue = DateTime.Parse(m_MyReader.GetValue(26).ToString());
                Tbl_Details.AddCell(new Phrase("  Date of issue of Transfer certificate", MyFonts[3]));//tc issue date
                Tbl_Details.AddCell(new Phrase(IDate, MyFonts[4]));//tc issue date

                Tbl_Details.AddCell(new Phrase("  No of School days up to the date of leaving", MyFonts[3]));//working days
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(27).ToString(), MyFonts[4]));//working days

                Tbl_Details.AddCell(new Phrase("  No of school days the pupil attended", MyFonts[3]));//present days
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(28).ToString(), MyFonts[4]));//present days

                Tbl_Details.AddCell(new Phrase("  Reason for leaving", MyFonts[3]));//present days
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(35).ToString(), MyFonts[4]));//present days
                
                Tbl_Details.AddCell(new Phrase("  Public Examination appeared with Reg No,Month And Year", MyFonts[3]));//present days
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(37).ToString(), MyFonts[4]));//present days


                Tbl_Details.AddCell(new Phrase("  School to which the student is transferring", MyFonts[3]));//present days
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(38).ToString(), MyFonts[4]));//present days


                 Tbl_Details.AddCell(new Phrase("  Character and Conduct", MyFonts[3]));//conduct
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(29).ToString(), MyFonts[4]));//conduct

                //for (int a = 0; a < _EmptyRowCount-5; a++)//empty rows
                //{
                   
                    Tbl_Details.AddCell(new Phrase("\n", MyFonts[3]));
                    Tbl_Details.AddCell(new Phrase("\n", MyFonts[4]));
                //}
            }
            return Tbl_Details;
        }

        private Document TCTemp_LoadStudentDetails(ref Document DocTCTemp, ref PdfWriter writerTCTemp, int _StudentId, out int _EmptyRowCount)
        {
            _EmptyRowCount = 0;
            iTextSharp.text.Table Tbl_Details = new iTextSharp.text.Table(2);
            Tbl_Details.Width = 100;
            float[] headerwidths = { 50, 50 };
            Tbl_Details.Widths = headerwidths;
            Tbl_Details.Padding = 1;
            Tbl_Details.DefaultCell.Border = 0;

            while (writerTCTemp.FitsPage(Tbl_Details))
            {
                Tbl_Details.AddCell(new Phrase("\n", MyFonts[3]));
                Tbl_Details.AddCell(new Phrase("\n", MyFonts[4]));
                _EmptyRowCount++;
                //DocTCTemp.Add(Tbl_Details);
            }
            DocTCTemp.Add(Tbl_Details);
            return DocTCTemp;
        }

        private iTextSharp.text.Table TC_SignandSealArea()
        {
            iTextSharp.text.Table Tbl_SignDetails = new iTextSharp.text.Table(2);
            Tbl_SignDetails.Width = 100;
            float[] headerwidths = { 50, 50 };
            Tbl_SignDetails.Widths = headerwidths;
            Tbl_SignDetails.Padding = 1;
            Tbl_SignDetails.DefaultCell.Border = 0;

            Tbl_SignDetails.DefaultCell.HorizontalAlignment = 1;
            Tbl_SignDetails.DefaultCell.BorderWidthRight = 1;

            Tbl_SignDetails.AddCell(new Phrase("\n", MyFonts[4])); //Sign
            Tbl_SignDetails.AddCell(new Phrase("\n", MyFonts[4])); //School Seal
            Tbl_SignDetails.AddCell(new Phrase("\n", MyFonts[4])); //Sign
            Tbl_SignDetails.AddCell(new Phrase("\n", MyFonts[4])); //School Seal

            Tbl_SignDetails.AddCell(new Phrase("Signature of the Head of the Institution", MyFonts[4])); //Sign
            Tbl_SignDetails.AddCell(new Phrase("SCHOOL SEAL", MyFonts[4])); //School Seal
            
            return Tbl_SignDetails;
        }

        private iTextSharp.text.Table TCTemp_LoadStudentDetailsToTable(int _StudentId)
        {
            iTextSharp.text.Table Tbl_Details = new iTextSharp.text.Table(2);
            Tbl_Details.Width = 100;
            float[] headerwidths = {50,50 };
            Tbl_Details.Widths = headerwidths;
            Tbl_Details.Padding = 1;
            Tbl_Details.DefaultCell.Border = 0;

            DateTime Date_Dob, Date_LastAttendance, Date_TcRcvedDate, Date_TCIssue;//, Date_DteOFAdmission;
            int _EmptyRowCount = 10;

            String Sql = "SELECT * FROM tbltc WHERE StudentId=" + _StudentId;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {

                Tbl_Details.AddCell(new Phrase("  Name of the School", MyFonts[3])); //School Name
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(4).ToString(), MyFonts[4])); //School Name

                Tbl_Details.AddCell(new Phrase("  Admission No.", MyFonts[3])); //Admission No
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(5).ToString(), MyFonts[4])); //Admission No

                Tbl_Details.AddCell(new Phrase("  Cumulative Record No.", MyFonts[3])); //Record No
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(6).ToString(), MyFonts[4])); //Record No

                Tbl_Details.AddCell(new Phrase("  Name of the Pupil", MyFonts[3])); //Pupil Name
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(3).ToString(), MyFonts[4])); //Pupil Name

                Tbl_Details.AddCell(new Phrase("  Sex", MyFonts[3])); //Sex
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(7).ToString(), MyFonts[4])); //Sex

                Tbl_Details.AddCell(new Phrase("  Name of Father", MyFonts[3])); //Father Name
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(8).ToString(), MyFonts[4])); //Father Name

                Date_Dob = DateTime.Parse(m_MyReader.GetValue(13).ToString());
                Tbl_Details.AddCell(new Phrase("  Date of Birth", MyFonts[3]));//Date of Birth
                Tbl_Details.AddCell(new Phrase(Date_Dob.Date.ToString("dd-MM-yyyy"), MyFonts[4]));//Date of Birth

                bool includeTime = false, UK = true;
                string DOBinWords = General.DateToText(Date_Dob, includeTime, UK);
                Tbl_Details.AddCell(new Phrase("  In Words", MyFonts[3]));//Date of Birth
                Tbl_Details.AddCell(new Phrase(DOBinWords, MyFonts[4]));//Date of Birth


              

                Tbl_Details.AddCell(new Phrase("  Nationality", MyFonts[3]));//Nationality
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(9).ToString(), MyFonts[4]));//Nationality

                Tbl_Details.AddCell(new Phrase("  Religion", MyFonts[3]));//Religion
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(10).ToString(), MyFonts[4]));//Religion

                Tbl_Details.AddCell(new Phrase("  Caste", MyFonts[3]));//cast
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(11).ToString(), MyFonts[4]));//cast

                Tbl_Details.AddCell(new Phrase("  SC or ST", MyFonts[3]));//sc\st
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(12).ToString(), MyFonts[4]));//sc\st

                Tbl_Details.AddCell(new Phrase("  Standard at the time of leaving the school", MyFonts[3]));//std
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(14).ToString(), MyFonts[4]));//std
                Tbl_Details.AddCell(new Phrase("  Date of Admission or promotion to that standard", MyFonts[3]));//std
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(39).ToString(), MyFonts[4]));//std
             
                Tbl_Details.AddCell(new Phrase("  For  higher standard Pupil :Language studied", MyFonts[3]));//language
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(15).ToString(), MyFonts[4]));//language

                Tbl_Details.AddCell(new Phrase("  Medium of Instruction", MyFonts[3]));//medium
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(16).ToString(), MyFonts[4]));//medium

                Tbl_Details.AddCell(new Phrase("  Syllabus", MyFonts[3]));//Syllabus
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(17).ToString(), MyFonts[4]));//Syllabus

                Tbl_Details.AddCell(new Phrase("  Date of Admission", MyFonts[3]));//Date of Admission 
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(18).ToString(), MyFonts[4]));//Date of Admission 

                Tbl_Details.AddCell(new Phrase("  Whether qualified for Promotion to a higher standard", MyFonts[3]));//promotion
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(19).ToString(), MyFonts[4]));//promotion

                Tbl_Details.AddCell(new Phrase("  Whether the pupil has paid all the fees due to the School", MyFonts[3]));//fee paid
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(20).ToString(), MyFonts[4]));//fee paid

                Tbl_Details.AddCell(new Phrase("  Fee concessions if any (Nature & period to be specified)", MyFonts[3]));//fee concession
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(21).ToString(), MyFonts[4]));//fee concession

                Tbl_Details.AddCell(new Phrase("  Scholarships, if any  (Nature & period to be specified)", MyFonts[3]));//scholarship
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(22).ToString(), MyFonts[4]));//scholarship

                Tbl_Details.AddCell(new Phrase("  Whether medically Examined or not", MyFonts[3]));//medical
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(23).ToString(), MyFonts[4]));//medical

                Date_LastAttendance = DateTime.Parse(m_MyReader.GetValue(24).ToString());
                Tbl_Details.AddCell(new Phrase("  Date of pupil's last attendance at school", MyFonts[3]));//last attendance
                Tbl_Details.AddCell(new Phrase(Date_LastAttendance.Date.ToString("dd-MM-yyyy"), MyFonts[4]));//last attendance

                Date_TcRcvedDate = DateTime.Parse(m_MyReader.GetValue(25).ToString());
                Tbl_Details.AddCell(new Phrase("  Date on which the application for the T C was received", MyFonts[3]));//tc date
                Tbl_Details.AddCell(new Phrase(Date_TcRcvedDate.Date.ToString("dd-MM-yyyy"), MyFonts[4]));//tc date

                Date_TCIssue = DateTime.Parse(m_MyReader.GetValue(26).ToString());
                Tbl_Details.AddCell(new Phrase("  Date of issue of Transfer certificate", MyFonts[3]));//tc issue date
                Tbl_Details.AddCell(new Phrase(Date_TCIssue.Date.ToString("dd-MM-yyyy"), MyFonts[4]));//tc issue date

                Tbl_Details.AddCell(new Phrase("  No of School days up to the date of leaving", MyFonts[3]));//working days
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(27).ToString(), MyFonts[4]));//working days

                Tbl_Details.AddCell(new Phrase("  No of school days the pupil attended", MyFonts[3]));//present days
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(28).ToString(), MyFonts[4]));//present days

                Tbl_Details.AddCell(new Phrase("  Character and Conduct", MyFonts[3]));//conduct
                Tbl_Details.AddCell(new Phrase(m_MyReader.GetValue(29).ToString(), MyFonts[4]));//conduct

                //for(int a=0;a<_EmptyRowCount; a++)//empty rows
                //{
                //   Tbl_Details.AddCell(new Phrase("\n", MyFonts[3]));
                //   Tbl_Details.AddCell(new Phrase("\n", MyFonts[4]));
                //}
            }
            return Tbl_Details;
        }

        private iTextSharp.text.Table TC_LoadCollageDetilsToTable(int _StudentId, string _physicalpath)
        {
            iTextSharp.text.Table _Tbl_CollageDetails = new iTextSharp.text.Table(2);
            _Tbl_CollageDetails.Width = 100;
            float[] headerwidths = { 20, 80 };
            _Tbl_CollageDetails.Widths = headerwidths;
            _Tbl_CollageDetails.Padding = 1;
            _Tbl_CollageDetails.Border = 0;
            _Tbl_CollageDetails.AutoFillEmptyCells = true;
            _Tbl_CollageDetails.DefaultCell.Border = 0;

            string _TCNo = "";

             sql = "SELECT tbltc.TcNumber from tbltc where tbltc.StudentId=" + _StudentId;
             m_MyReader = m_MysqlDb.ExecuteQuery(sql);
             if (m_MyReader.HasRows)
             {
                 _TCNo = m_MyReader.GetValue(0).ToString();
             }
           
             _Tbl_CollageDetails.DefaultCell.Colspan = 2;
             _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
             _Tbl_CollageDetails.AddCell(new Phrase("TC No: "+_TCNo, MyFonts[4])); //TC No.

             ImageUploaderClass imgobj = new ImageUploaderClass(m_objSchool);
            sql = "select tblschooldetails.SchoolName, tblschooldetails.Address from tblschooldetails";
            //GroupName(0),Address(1)
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {

                if (true)
                {
                if (true)
                    {
                        iTextSharp.text.Image _ImgHeader = iTextSharp.text.Image.GetInstance(imgobj.getImageBytes(1, "Logo"));
                        _ImgHeader.ScaleAbsolute(75, 75);
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
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader.GetValue(0).ToString(), MyFonts[1])); //Collage Name
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader.GetValue(1).ToString(), MyFonts[2]));//Collage Address
                    }
                    else
                    {
                        _Tbl_CollageDetails.DefaultCell.Colspan = 2;
                        _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                        _Tbl_CollageDetails.DefaultCell.Rowspan = 1;
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader.GetValue(0).ToString(), MyFonts[1])); //Collage Name
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader.GetValue(1).ToString(), MyFonts[2]));//Collage Address

                    }


                }
                else
                {
                    _Tbl_CollageDetails.DefaultCell.Colspan = 2;
                    _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                    _Tbl_CollageDetails.DefaultCell.Rowspan = 1;
                    _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader.GetValue(0).ToString(), MyFonts[1])); //Collage Name
                    _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader.GetValue(1).ToString(), MyFonts[2]));//Collage Address

                }

            }
            return _Tbl_CollageDetails;

        }

        private bool Delete_Existing_ExamReport(string _PdfName, string _physicalpath, out string _ErrorMsg)
        {
            _ErrorMsg = "";
            bool _valid = false;
            try
            {
                sql = "SELECT tblcreatedtc.TcName from tblcreatedtc";
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










        #region COMMON FUNCTIONS
        private void LoadMyFont()
        {

            //NEW
            MyFonts[0] = FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD);//Title(FR

            //Collage Details
            MyFonts[1] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 15, iTextSharp.text.Font.BOLD);// CollageName  (RF
            MyFonts[2] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.NORMAL);// Collage ADDRESS (RF
            MyFonts[3] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL);// Bill and Student etails              
            MyFonts[4] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD);// Bill and Student etails       
            //message

            //m_FntFeeItem = MyFonts[3];//empty table and Addone item functions
            //m_FntFeeItemDesc = MyFonts[4];//
            //MyFonts[5] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL);// for the Message (QTN ,DC  ,OA ,PI,TAX INVOICE       

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
    }
}