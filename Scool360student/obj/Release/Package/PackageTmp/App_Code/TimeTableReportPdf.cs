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
    public class TimeTableReportPdf
    {
        private Font[] MyFonts = new Font[24];
        public MysqlClass m_MysqlDb;
        private OdbcDataReader MyReader = null;
        private DataSet m_MyDataSet = null;
        private SchoolClass m_objSchool = null;
        private string sql = "";
        private string m_NullStr = "NULL";
        private string _YourRef = "Asper your enquiry";
        private string m_AmtType = "RS";
        private string m_PdfType = "";

        public TimeTableReportPdf(MysqlClass _Mysqlobj, SchoolClass objSchool)
        {
            m_objSchool = objSchool;
            m_MysqlDb = _Mysqlobj;
            LoadMyFont();
        }

        ~TimeTableReportPdf()
        {
            if (m_MysqlDb != null)
            {
                m_MysqlDb = null;
            }
            if (MyReader != null)
            {
                MyReader = null;
            }
        }





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

        #region CLASS WISE TIME TABLE REPORT PDF FUNCTIONS

        internal bool CreateTimeTableReportForSelectedClasses(int _ClassId,string _BatchName, string _physicalpath, out string _PdfName, out string _ErrorMsg)
        {
            _PdfName = "";
            _ErrorMsg = "";
            int _PeriodCount = 0;

            try
            {
                string _ClassName = "";
                sql = "SELECT tblclass.ClassName from tblclass where tblclass.Id=" + _ClassId;
                MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    _ClassName = MyReader.GetValue(0).ToString();
                }
                _PdfName = "TimeTable_" + _ClassName + ".pdf";

                if (Delete_Existing_ExamReport( _physicalpath, out _ErrorMsg))
                {
                    Document DocTT = new Document(PageSize.A4, 50, 40, 20, 10);
                    PdfWriter writerTT = PdfWriter.GetInstance(DocTT, new FileStream(_physicalpath + "\\PDF_Files\\TimeTable_" + _ClassName + ".pdf", FileMode.Create));
                    
                    DocTT.Open();
                   _PeriodCount = GetPeriodCount();
                    iTextSharp.text.Table Tbl_CollageInformation = TT_LoadCollageDetilsToTable(_BatchName,_ClassName, _physicalpath);
                    iTextSharp.text.Table Tbl_TimeTable = TT_LoadClassTimetableDetails(_ClassId, _PeriodCount);

                    DocTT.Add(Tbl_CollageInformation);
                    DocTT.Add(Tbl_TimeTable);
                    DocTT.Close();

                    sql = "Delete from tbltime_createdreport ";
                    m_MysqlDb.ExecuteQuery(sql);
                    sql = "INSERT into tbltime_createdreport(tbltime_createdreport.TimeTable) VALUES ('" + _PdfName + "')";
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

        private int GetPeriodCount()
        {
            OdbcDataReader m_MyReader2 = null;
            int _PeriodCount = 0;
            sql = "SELECT COUNT(tblattendanceperiod.PeriodId) from tblattendanceperiod where tblattendanceperiod.ModeId=3";
            m_MyReader2 = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader2.HasRows)
            {
                int.TryParse(m_MyReader2.GetValue(0).ToString(), out _PeriodCount);
            }
            return _PeriodCount;
        }

        private iTextSharp.text.Table TT_LoadClassTimetableDetails(int _ClassId, int _PeriodCount)
        {
            iTextSharp.text.Table Tbl_TimeTableDetails = new iTextSharp.text.Table(_PeriodCount+1);
            OdbcDataReader MyReader_TT = null;
            OdbcDataReader MyReader_Days = null;
            OdbcDataReader MyReader_Period = null;
            Tbl_TimeTableDetails.Width = 100;
            Tbl_TimeTableDetails.Padding = 1;
            Tbl_TimeTableDetails.AutoFillEmptyCells = true;

            Tbl_TimeTableDetails.DefaultCell.HorizontalAlignment = 1;
            Tbl_TimeTableDetails.AddCell(new Phrase("DAY", MyFonts[4]));
            int _PeriodLast = 4 + _PeriodCount - 1;
            sql = "SELECT tblattendanceperiod.FrequencyName from tblattendanceperiod where tblattendanceperiod.PeriodId BETWEEN 4 and " + _PeriodLast;
            MyReader_TT = m_MysqlDb.ExecuteQuery(sql);
            if (MyReader_TT.HasRows)
            {
                while (MyReader_TT.Read())
                {
                    Tbl_TimeTableDetails.AddCell(new Phrase(MyReader_TT.GetValue(0).ToString(), MyFonts[4]));
                }
            }

            string _DayName = "";

            string sql1 = "SELECT tbltime_week.Name from tbltime_week where tbltime_week.Name not in (SELECT tblholidayconfig.`Day` FROM tblholidayconfig WHERE tblholidayconfig.`Status`=1) order by tbltime_week.Id";
            MyReader_Days = m_MysqlDb.ExecuteQuery(sql1);
            if (MyReader_Days.HasRows)
            {
                while (MyReader_Days.Read())
                {
                    _DayName = MyReader_Days.GetValue(0).ToString();
                    Tbl_TimeTableDetails.AddCell(new Phrase(_DayName.ToUpper(), MyFonts[4]));

                    for (int i = 4; i <= _PeriodLast; i++)
                    {
                        sql = "SELECT tbltime_week.Name, tblsubjects.subject_name from tbltime_classperiod inner join tbltime_master on tbltime_master.ClassPeriodId= tbltime_classperiod.Id inner join tbltime_week on tbltime_week.Id= tbltime_classperiod.DayId inner join tblsubjects on tblsubjects.Id= tbltime_master.SubjectId where tbltime_classperiod.ClassId=" + _ClassId + " and tbltime_classperiod.PeriodId =" + i + " and tbltime_week.Name='" + _DayName + "'";
                        MyReader_Period = m_MysqlDb.ExecuteQuery(sql);
                        if (MyReader_Period.HasRows)
                        {
                            Tbl_TimeTableDetails.AddCell(new Phrase(MyReader_Period.GetValue(1).ToString(), MyFonts[3]));
                        }
                        else
                        {
                            Tbl_TimeTableDetails.AddCell(new Phrase("\n", MyFonts[3]));
                        }
                    }
                }
            }
            return Tbl_TimeTableDetails;
        }

        private iTextSharp.text.Table TT_LoadCollageDetilsToTable(string _BatchName, string _ClassName, string _physicalpath)
        {
            OdbcDataReader m_MyReader1 =null;
            iTextSharp.text.Table _Tbl_CollageDetails = new iTextSharp.text.Table(2);
            _Tbl_CollageDetails.Width = 100;
            float[] headerwidths = { 20, 80 };
            _Tbl_CollageDetails.Widths = headerwidths;
            _Tbl_CollageDetails.Padding = 1;
            _Tbl_CollageDetails.Border = 0;
            _Tbl_CollageDetails.AutoFillEmptyCells = true;
            _Tbl_CollageDetails.DefaultCell.Border = 0;
            ImageUploaderClass imgobj = new ImageUploaderClass(m_objSchool);
           string sql1 = "select tblschooldetails.SchoolName, tblschooldetails.Address from tblschooldetails";
            //GroupName(0),Address(1)
            m_MyReader1 = m_MysqlDb.ExecuteQuery(sql1);
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
                        _Tbl_CollageDetails.DefaultCell.Rowspan = 3;

                        Cell cell = new Cell();
                        cell.Rowspan = 3;
                        cell.Add(_ImgHeader);
                        cell.Border = 0;
                        _Tbl_CollageDetails.AddCell(cell);
                        _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                        _Tbl_CollageDetails.DefaultCell.Rowspan = 1;
                        _Tbl_CollageDetails.DefaultCell.VerticalAlignment = 1;
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(0).ToString(), MyFonts[1])); //Collage Name
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(1).ToString(), MyFonts[2]));//Collage Address
                        _Tbl_CollageDetails.AddCell(new Phrase("TIME TABLE ("+_BatchName+" - "+_ClassName.ToUpper()+" )", MyFonts[4]));//batch and class name
                    }
                    else
                    {
                        _Tbl_CollageDetails.DefaultCell.Colspan = 2;
                        _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                        _Tbl_CollageDetails.DefaultCell.Rowspan = 1;
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(0).ToString(), MyFonts[1])); //Collage Name
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(1).ToString(), MyFonts[2]));//Collage Address
                        _Tbl_CollageDetails.AddCell(new Phrase("TIME TABLE (" + _BatchName + " - " + _ClassName.ToUpper() + " )", MyFonts[4]));//batch and class name
                    }


                }
                else
                {
                    _Tbl_CollageDetails.DefaultCell.Colspan = 2;
                    _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                    _Tbl_CollageDetails.DefaultCell.Rowspan = 1;
                    _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(0).ToString(), MyFonts[1])); //Collage Name
                    _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(1).ToString(), MyFonts[2]));//Collage Address
                    _Tbl_CollageDetails.AddCell(new Phrase("TIME TABLE (" + _BatchName + " - " + _ClassName.ToUpper() + " )", MyFonts[4]));//batch and class name
                }

            }
            _Tbl_CollageDetails.DefaultCell.Colspan = 2;
            _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
            _Tbl_CollageDetails.AddCell(new Phrase("\n", MyFonts[3])); //empty

            return _Tbl_CollageDetails;
        }

        private bool Delete_Existing_ExamReport(string _physicalpath, out string _ErrorMsg)
        {
            _ErrorMsg = "";
            bool _valid = false;
            try
            {
                sql = "SELECT tbltime_createdreport.TimeTable from tbltime_createdreport";
                MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        string _pdf = MyReader.GetValue(0).ToString();
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

        internal bool CreateTimeTableReportForAllClasses(string _BatchName,int _UserId, string _physicalpath, out string _PdfName, out string _ErrorMsg)
        {
            _PdfName = "";
            _ErrorMsg = "";
            int _PeriodCount = 0;

            try
            {
                if (Delete_Existing_ExamReport(_physicalpath, out _ErrorMsg))
                {
                _PdfName = "TimeTable_AllClass.pdf";
                Document DocTT = new Document(PageSize.A4, 50, 40, 20, 10);
                PdfWriter writerTT = PdfWriter.GetInstance(DocTT, new FileStream(_physicalpath + "\\PDF_Files\\TimeTable_AllClass.pdf", FileMode.Create));

                DocTT.Open();
                _PeriodCount = GetPeriodCount();

                string _ClassName = "";
                int _ClassId = 0;
                sql = "SELECT tblclass.Id,tblclass.ClassName FROM tblclass where tblclass.Id IN (SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _UserId + "))";
                MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        _ClassId = int.Parse(MyReader.GetValue(0).ToString());
                        _ClassName = MyReader.GetValue(1).ToString();

                        iTextSharp.text.Table Tbl_CollageInformation = TT_LoadCollageDetilsToTable(_BatchName, _ClassName, _physicalpath);
                        iTextSharp.text.Table Tbl_TimeTable = TT_LoadClassTimetableDetails(_ClassId, _PeriodCount);

                        DocTT.Add(Tbl_CollageInformation);
                        DocTT.Add(Tbl_TimeTable);

                        DocTT.NewPage();
                    }
                }
                DocTT.Close();

                    sql = "Delete from tbltime_createdreport ";
                    m_MysqlDb.ExecuteQuery(sql);
                    sql = "INSERT into tbltime_createdreport(tbltime_createdreport.TimeTable) VALUES ('" + _PdfName + "')";
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




        #region STAFF WISE TIME TABLE REPORT PDF FUNCTIONS

        internal bool CreateTimeTableReportForSelectedStaff(int _StaffId, string _BatchName, string _physicalpath, out string _PdfName, out string _ErrorMsg)
        {
            _PdfName = "";
            _ErrorMsg = "";
            int _PeriodCount = 0;

            try
            {
                string _StaffName = "";
                sql = "SELECT tbluser.SurName from tbluser where tbluser.Id=" + _StaffId;
                MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    _StaffName = MyReader.GetValue(0).ToString();
                }
                _PdfName = "TimeTable_" + _StaffName + ".pdf";

                if (Delete_Existing_ExamReport(_physicalpath, out _ErrorMsg))
                {
                    Document DocTT = new Document(PageSize.A4, 50, 40, 20, 10);
                    PdfWriter writerTT = PdfWriter.GetInstance(DocTT, new FileStream(_physicalpath + "\\PDF_Files\\TimeTable_" + _StaffName + ".pdf", FileMode.Create));

                    DocTT.Open();
                    _PeriodCount = GetPeriodCount();
                    iTextSharp.text.Table Tbl_CollageInformation = TT_LoadCollageDetilsToTable(_BatchName, _StaffName, _physicalpath);
                    iTextSharp.text.Table Tbl_TimeTable = TT_LoadStaffTimetableDetails(_StaffId, _PeriodCount);

                    DocTT.Add(Tbl_CollageInformation);
                    DocTT.Add(Tbl_TimeTable);
                    DocTT.Close();

                    sql = "Delete from tbltime_createdreport ";
                    m_MysqlDb.ExecuteQuery(sql);
                    sql = "INSERT into tbltime_createdreport(tbltime_createdreport.TimeTable) VALUES ('" + _PdfName + "')";
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

        private iTextSharp.text.Table TT_LoadStaffTimetableDetails(int _StaffId, int _PeriodCount)
        {
            iTextSharp.text.Table Tbl_TimeTableDetails = new iTextSharp.text.Table(_PeriodCount + 1);
            OdbcDataReader MyReader_TT = null;
            OdbcDataReader MyReader_Days = null;
            OdbcDataReader MyReader_Period = null;
            Tbl_TimeTableDetails.Width = 100;
            Tbl_TimeTableDetails.Padding = 1;
            Tbl_TimeTableDetails.AutoFillEmptyCells = true;

            Tbl_TimeTableDetails.DefaultCell.HorizontalAlignment = 1;
            Tbl_TimeTableDetails.AddCell(new Phrase("DAY", MyFonts[4]));
            int _PeriodLast = 4 + _PeriodCount - 1;
            sql = "SELECT tblattendanceperiod.FrequencyName from tblattendanceperiod where tblattendanceperiod.PeriodId BETWEEN 4 and " + _PeriodLast;
            MyReader_TT = m_MysqlDb.ExecuteQuery(sql);
            if (MyReader_TT.HasRows)
            {
                while (MyReader_TT.Read())
                {
                    Tbl_TimeTableDetails.AddCell(new Phrase(MyReader_TT.GetValue(0).ToString(), MyFonts[4]));
                }
            }

            string _DayName = "";

            string sql1 = "SELECT tbltime_week.Name from tbltime_week where tbltime_week.Name not in (SELECT tblholidayconfig.`Day` FROM tblholidayconfig WHERE tblholidayconfig.`Status`=1) order by tbltime_week.Id";
            MyReader_Days = m_MysqlDb.ExecuteQuery(sql1);
            if (MyReader_Days.HasRows)
            {
                while (MyReader_Days.Read())
                {
                    _DayName = MyReader_Days.GetValue(0).ToString();
                    Tbl_TimeTableDetails.AddCell(new Phrase(_DayName.ToUpper(), MyFonts[4]));

                    for (int i = 4; i <= _PeriodLast; i++)
                    {
                        sql = "SELECT tbltime_master.StaffId, tbltime_week.Name, tblclass.ClassName, tblsubjects.subject_name,tbltime_classperiod.PeriodId from tbltime_classperiod inner join tbltime_master on tbltime_master.ClassPeriodId= tbltime_classperiod.Id inner join tbltime_week on tbltime_week.Id= tbltime_classperiod.DayId inner join tblclass on tblclass.Id=tbltime_classperiod.ClassId inner join tblsubjects on tblsubjects.Id= tbltime_master.SubjectId where tbltime_week.Name='" + _DayName + "' and tbltime_master.StaffId=" + _StaffId + " and tbltime_classperiod.PeriodId=" + i;
                        MyReader_Period = m_MysqlDb.ExecuteQuery(sql);
                        if (MyReader_Period.HasRows)
                        {
                            Tbl_TimeTableDetails.AddCell(new Phrase(MyReader_Period.GetValue(2).ToString() + "\n" + "(" + MyReader_Period.GetValue(3).ToString() + ")", MyFonts[3]));
                        }
                        else
                        {
                            Tbl_TimeTableDetails.AddCell(new Phrase("\n" + "\n", MyFonts[3]));
                        }
                    }
                }
            }
            return Tbl_TimeTableDetails;
        }

        internal bool CreateTimeTableReportForAllStaffs(string _BatchName, int _UserId, string _physicalpath, out string _PdfName, out string _ErrorMsg)
        {
            _PdfName = "";
            _ErrorMsg = "";
            int _PeriodCount = 0;

            try
            {
                if (Delete_Existing_ExamReport(_physicalpath, out _ErrorMsg))
                {
                    _PdfName = "TimeTable_AllStaffs.pdf";
                    Document DocTT = new Document(PageSize.A4, 50, 40, 20, 10);
                    PdfWriter writerTT = PdfWriter.GetInstance(DocTT, new FileStream(_physicalpath + "\\PDF_Files\\TimeTable_AllStaffs.pdf", FileMode.Create));

                    DocTT.Open();
                    _PeriodCount = GetPeriodCount();

                    string _StaffName = "";
                    int _StaffId = 0;
                    sql = "select distinct tbluser.Id,tbluser.SurName from tbluser inner join tblrole on tblrole.Id = tbluser.RoleId where tbluser.`Status`=1 and  tblrole.Type='staff'";
                    MyReader = m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        while (MyReader.Read())
                        {
                            _StaffId = int.Parse(MyReader.GetValue(0).ToString());
                            _StaffName = MyReader.GetValue(1).ToString();

                            iTextSharp.text.Table Tbl_CollageInformation = TT_LoadCollageDetilsToTable(_BatchName, _StaffName, _physicalpath);
                            iTextSharp.text.Table Tbl_TimeTable = TT_LoadStaffTimetableDetails(_StaffId, _PeriodCount);

                            DocTT.Add(Tbl_CollageInformation);
                            DocTT.Add(Tbl_TimeTable);

                            DocTT.NewPage();
                        }
                    }
                    DocTT.Close();

                    sql = "Delete from tbltime_createdreport ";
                    m_MysqlDb.ExecuteQuery(sql);
                    sql = "INSERT into tbltime_createdreport(tbltime_createdreport.TimeTable) VALUES ('" + _PdfName + "')";
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
                    _ErrorMsg = "This file is used by another person.Try again later";
                return false;
            }
        }

        #endregion

        
    }
}
