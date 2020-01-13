using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Odbc;
using System.Data;
using System.Text;
using System.Collections;
using System.Globalization;
using System.Configuration;
using System.Net.Mail;

namespace WinBase
{

    public class AttendenceTableManager
    {
        public MysqlClass m_MysqlDb;
        private OdbcDataReader m_MyReader = null;
        public AttendenceTableManager(MysqlClass _MysqlDb)
        {
            m_MysqlDb = _MysqlDb;
        }

        public bool AttendanceTables_Exits(string StdId, int YearId)
        {
            bool valid = false;
            string _tablename = "";
            string End_Region = "std" + StdId + "yr" + YearId;
            string Sql = "show tables like 'tblattdcls_" + End_Region + "'";

            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);

            if (m_MyReader.HasRows)
            {
                _tablename = m_MyReader.GetValue(0).ToString();
                if (_tablename != "")
                {
                    valid = true;
                }
            }
            if (valid)
            {
                _tablename = "";
                valid = false;
                Sql = "show tables like 'tblattdstud_" + End_Region + "'";

                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
               
                if (m_MyReader.HasRows)
                {
                    _tablename = m_MyReader.GetValue(0).ToString();
                    if (_tablename != "")
                    {
                        valid = true;
                    }
                }
            }
            return valid;
        }

        public void CreateStudentAttendanceTables(int BatchId)
        {
            int StandardId = 0;
            string clstable = "";
            string sql = "SELECT Id,Name FROM  tblstandard";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                   
                    StandardId = 0;
                    int.TryParse(m_MyReader.GetValue(0).ToString(), out StandardId);
                    if (StandardId > 0)
                    {
                       
                        clstable = "DROP TABLE IF EXISTS `tblattdcls_std" + StandardId + "yr" + BatchId + "`";
                        m_MysqlDb.ExecuteQuery(clstable);
                        clstable = "CREATE TABLE `tblattdcls_std" + StandardId + "yr" + BatchId + "` (`Id` int(15) NOT NULL AUTO_INCREMENT,`ClassId` int(11) DEFAULT NULL,`Date` date DEFAULT NULL,`Status` tinyint(4) DEFAULT NULL COMMENT '1:ForeNoon , 2:AfterNoon , 3:FullDay',`LastModifiedDateTime` datetime DEFAULT NULL,`LastModifiedUserId` int(11) DEFAULT NULL,`CreatedDateTime` datetime DEFAULT NULL,`CreatedUserId` int(11) DEFAULT NULL,PRIMARY KEY (`Id`),UNIQUE KEY `UniqueIndex` (`ClassId`,`Date`)) ";
                        m_MysqlDb.ExecuteQuery(clstable);
                        clstable = "DROP TABLE IF EXISTS `tblattdstud_std" + StandardId + "yr" + BatchId + "`";
                        m_MysqlDb.ExecuteQuery(clstable);
                        clstable = "CREATE TABLE `tblattdstud_std" + StandardId + "yr" + BatchId + "` (`Id` int(11) NOT NULL AUTO_INCREMENT,`ClassAttendanceId` int(15) DEFAULT NULL,`StudentId` bigint(20) DEFAULT NULL,`PresentStatus` tinyint(4) DEFAULT '0' COMMENT '0:Absent , 1:ForNoon , 2:AfterNoon , 3:FullDay',`ApproveStatus` tinyint(3) DEFAULT '0',`ApproveId` int(11) DEFAULT NULL, `InTime` varchar(11) DEFAULT NULL, `OutTime` varchar(11) DEFAULT NULL, `IsLate` tinyint(3) DEFAULT '0' COMMENT '1 means student is Late', `LateValue` varchar(11) DEFAULT NULL, PRIMARY KEY (`Id`))";
                        m_MysqlDb.ExecuteQuery(clstable);
                    }
                }

            }
        }

        public void RemoveOldStudentAttendanceTables(int BatchId)
        {
            int StandardId = 0;
            string clstable = "";
            string sql = "SELECT Id,Name FROM  tblstandard";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    StandardId = 0;
                    int.TryParse(m_MyReader.GetValue(0).ToString(), out StandardId);
                    if (StandardId > 0)
                    {
                        clstable = "DROP TABLE IF EXISTS `tblattdcls_std" + StandardId + "yr" + BatchId + "`";
                        m_MysqlDb.ExecuteQuery(clstable);
                        clstable = "DROP TABLE IF EXISTS `tblattdstud_std" + StandardId + "yr" + BatchId + "`";
                        m_MysqlDb.ExecuteQuery(clstable);
                    }
                }
            }
        }




        public void CreateSingle_StudentAttendanceTables(int StandardId, int BatchId)
        {
            if (StandardId > 0)
            {
               string clstable = "DROP TABLE IF EXISTS `tblattdcls_std" + StandardId + "yr" + BatchId + "`";
                m_MysqlDb.ExecuteQuery(clstable);
                clstable = "CREATE TABLE `tblattdcls_std" + StandardId + "yr" + BatchId + "` (`Id` int(15) NOT NULL AUTO_INCREMENT,`ClassId` int(11) DEFAULT NULL,`Date` date DEFAULT NULL,`Status` tinyint(4) DEFAULT NULL COMMENT '1:ForeNoon , 2:AfterNoon , 3:FullDay',`LastModifiedDateTime` datetime DEFAULT NULL,`LastModifiedUserId` int(11) DEFAULT NULL,`CreatedDateTime` datetime DEFAULT NULL,`CreatedUserId` int(11) DEFAULT NULL,PRIMARY KEY (`Id`),UNIQUE KEY `UniqueIndex` (`ClassId`,`Date`)) ";
                m_MysqlDb.ExecuteQuery(clstable);
                clstable = "DROP TABLE IF EXISTS `tblattdstud_std" + StandardId + "yr" + BatchId + "`";
                m_MysqlDb.ExecuteQuery(clstable);
                clstable = "CREATE TABLE `tblattdstud_std" + StandardId + "yr" + BatchId + "` (`Id` int(11) NOT NULL AUTO_INCREMENT,`ClassAttendanceId` int(15) DEFAULT NULL,`StudentId` bigint(20) DEFAULT NULL,`PresentStatus` tinyint(4) DEFAULT '0' COMMENT '0:Absent , 1:ForNoon , 2:AfterNoon , 3:FullDay',`ApproveStatus` tinyint(3) DEFAULT '0',`ApproveId` int(11) DEFAULT NULL, `InTime` varchar(11) DEFAULT NULL, `OutTime` varchar(11) DEFAULT NULL,`IsLate` tinyint(3) DEFAULT '0' COMMENT '1 means student is Late', `LateValue` varchar(11) DEFAULT NULL, PRIMARY KEY (`Id`))";
                m_MysqlDb.ExecuteQuery(clstable);
            }
        }

        
    }

    public class Attendance : KnowinGen
    {
        public MysqlClass m_MysqlDb;
        public MysqlClass m_TransationDb = null;
        private OdbcDataReader m_MyReader = null;
        private DataSet m_HolidayDataset = null;
        private DataSet m_HolidayDataset_Staffs=null;
        private DataSet m_groups = null;
        string Sql;
        public Attendance(KnowinGen _Prntobj)
        {
            m_Parent = _Prntobj;
            m_MyODBCConn = m_Parent.ODBCconnection;
            m_UserName = m_Parent.LoginUserName;
            m_MysqlDb = new MysqlClass(this);
 
        }
        public Attendance(MysqlClass _Msqlobj)
        {
            m_ConnectionStr = _Msqlobj.ConnectionStr;
            m_MysqlDb = _Msqlobj;
        }
        ~Attendance()
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

        public void CreateTansationDb()
        {

            if (m_TransationDb != null)
            {

                m_TransationDb.TransactionRollback();
                m_TransationDb = null;
            }

            m_TransationDb = new MysqlClass(this);
            m_TransationDb.MyBeginTransaction();

        }

        public void CreateExternalTansationDb()
        {

            if (m_TransationDb != null)
            {

                m_TransationDb.TransactionRollback();
                m_TransationDb = null;
            }

            m_TransationDb = new MysqlClass(m_ConnectionStr);
            m_TransationDb.MyBeginTransaction();

        }

        public void EndSucessTansationDb()
        {
            if (m_TransationDb != null)
            {
                m_TransationDb.TransactionCommit();
                m_TransationDb = null;
            }


        }
        public void EndFailTansationDb()
        {
            if (m_TransationDb != null)
            {
                m_TransationDb.TransactionRollback();
                m_TransationDb = null;
            }

        }


        public void EndSucessExternalTansationDb()
        {
            if (m_TransationDb != null)
            {
                m_TransationDb.TransactionCommit();
                m_TransationDb.CloseConnection();
                m_TransationDb = null;
            }


        }
        public void EndFailExternalTansationDb()
        {
            if (m_TransationDb != null)
            {
                m_TransationDb.TransactionRollback();
                m_TransationDb.CloseConnection();
                m_TransationDb = null;
            }

        }


        public string gettodaydate()
        {
            CLogging logger = CLogging.GetLogObject();
            string _DATEToday;
            DateTime _curerntdateTime = DateTime.Today;
            logger.LogToFile("gettodaydate", "geting Todays Date", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
            _DATEToday = _curerntdateTime.ToString("dddd");
            logger.LogToFile("gettodaydate", "Exiting from gettodaydate funtion", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
            return _DATEToday;

        }
        // funtion is used for tbldate, and for selecting tbldate.Id from tbldate //
        public int insertToDateTable(int _redateID, string _typ, int _ClsId)
        {
           
            int _dateId = -1;
            Sql = "INSERT INTO tbldate(DateId,Status,classId) VALUES(" + _redateID + ",'" + _typ + "'," + _ClsId + ")";
            m_MyReader = m_TransationDb.ExecuteQuery(Sql);
          
            Sql = "select tbldate.Id from tbldate where tbldate.DateId =" + _redateID + " and tbldate.Status='" + _typ + "'and tbldate.classId=" + _ClsId;
            m_MyReader = m_TransationDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                m_MyReader.Read();
                _dateId = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            return _dateId;




        }

        private bool CheckDateInDatemast(DateTime _date, out int _reDateId)
        {
            bool flag = false;
            _reDateId = 0;
            
            Sql = " select tblmasterdate.Id from tblmasterdate where tblmasterdate.date='" + _date.ToString("yyyy-MM-dd") + "'";
            if (m_TransationDb == null)

                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            else
                m_MyReader = m_TransationDb.ExecuteQuery(Sql);  

            if (m_MyReader.HasRows)
            {
                flag = true;
                _reDateId = int.Parse(m_MyReader.GetValue(0).ToString());
            }

            return flag;
        }

        public void Insert_theStaff_AttendanceTblDetails(int _StaffId, int _dateId, int _PeriodId)
        {      
             
            Sql = "insert into tblstaffattendancedetails(StaffId,DayId,PeriodId)VALUES(" + _StaffId + "," + _dateId + "," + _PeriodId + ")";
            m_MyReader = m_TransationDb.ExecuteQuery(Sql);           
           
        }
        public void Insert_theStaff_AttendanceTbl(int _StaffId, int _dateId, int Status)
        {
            Sql = "insert into tblstaffattendance(StaffId,DayId,LeaveRequest,Status)VALUES(" + _StaffId + "," + _dateId + ",0," + Status + ")";
            m_MyReader = m_TransationDb.ExecuteQuery(Sql);
        }
        public void insertToDateTableFrmMonToSat(DateTime _getdate, int _clsId, string _typ)
        {
            try
            {

                CLogging logger = CLogging.GetLogObject();
                int i = 0;
                logger.LogToFile("insertToDateTableFrmMonToSat", " querry to insert into masterdate Table 6 date ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                while (i < 6)
                {
                    int _redateID = getDateIdFrmMastTbl(_getdate);
                    Sql = "INSERT INTO tbldate(DateId,Status,classId) VALUES('" + _redateID + "','" + _typ + "'," + _clsId + " )";
                    m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
                    i++;
                    _getdate = _getdate.AddDays(1);
                }
                logger.LogToFile("insertToDateTableFrmMonToSat", " Inserted into the master date table  ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            }
            catch (Exception e)
            {
                CLogging logger = CLogging.GetLogObject();
                logger.LogToFile("insertToDateTableFrmMonToSat", " Error during the insertion into the master tbl " + e.Message + "", 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
            }
        }

       
        public int selectDateIdFrmDay(DateTime _getdate, string _typ)
        {
            int _dateid = -1;
            CLogging logger = CLogging.GetLogObject();
            //  Sql = "select tbldate.DateId from tbldate inner join tblmasterdate on tblmasterdate.Id= tbldate.DateId WHERE tblmasterdate.`date`='"2009-08-10' and tbldate.Status='staff' ";
            logger.LogToFile("selectDateIdFrmDay", " Querry to select the date id from date tbl", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            Sql = "select tbldate.Id from tbldate inner join tblmasterdate on tblmasterdate.Id= tbldate.DateId WHERE tblmasterdate.`date`='" + _getdate.ToString("yyyy-MM-dd") + "'and tbldate.Status='" + _typ + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                m_MyReader.Read();
                _dateid = int.Parse(m_MyReader.GetValue(0).ToString());
            }

            logger.LogToFile("selectDateIdFrmDay", " select the Staff Id from Tbldate table", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            return _dateid;

        }

        public int getDateIdFrmMastTbl(DateTime _date)
        {
            int _retunDateId = -1;
            int _reDateId = -1;           
            if (CheckDateInDatemast(_date, out _reDateId) == true)
            {

                _retunDateId = int.Parse(_reDateId.ToString());
            }
            else
            {
                    Sql = "INSERT INTO tblmasterdate(Date) VALUES('" + _date.ToString("s") + "')";
                    if(  m_TransationDb ==null)

                        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
                    else
                        m_MyReader = m_TransationDb.ExecuteQuery(Sql);  
                   
                    Sql = " select tblmasterdate.Id from tblmasterdate where date ='" + _date.ToString("yyyy-MM-dd") + "'";
                    if (m_TransationDb == null)

                        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
                    else
                        m_MyReader = m_TransationDb.ExecuteQuery(Sql);  
                  
                    if (m_MyReader.HasRows)
                    {
                        _retunDateId = int.Parse(m_MyReader.GetValue(0).ToString());

                    }
               
            }
            return _retunDateId;


        }
       
        public bool CheckAttadanceStatusOfClass(DateTime _todayDate, int _ClassId)
        {
           
            bool _checkStatus = false;
            int dateid;
            if (CheckDateInDatemast(_todayDate, out dateid) == true)
            {

                dateid = int.Parse(dateid.ToString());
                Sql = "select tbldate.Id from tbldate where tbldate.DateId=" + dateid + " and tbldate.classId="+_ClassId+" and tbldate.Status='class'";
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
                if (m_MyReader.HasRows)
                {
                    _checkStatus = true;
                }
            }                       
           
            return _checkStatus;

        }

        public bool CheckAttadanceStatusOf_Staff(DateTime _todayDate)
        {
            bool _checkStatus = false;
            int dateid;
            if (CheckDateInDatemast(_todayDate, out dateid) == true)
            {

                dateid = int.Parse(dateid.ToString());
                Sql = "select tbldate.Id from tbldate where tbldate.DateId=" + dateid + " and tbldate.classId=0 and tbldate.Status='staff'";
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
                if (m_MyReader.HasRows)
                {
                    _checkStatus = true;
                }
            }

            return _checkStatus;
        }

        public void Insert_theStundent_AttendanceTblDetails(int _StutendId, int _dateId, int _PeriodId)
        {

            Sql = "insert into tblstudentattendancedetails(StudentId,DayId,PeriodId)VALUES(" + _StutendId + "," + _dateId + "," + _PeriodId + ")";
            m_TransationDb.TransExecuteQuery(Sql);


        }
        public void Insert_theStundent_AttendanceTbl(int _StutendId, int _dateId, int Status)
        {          
                //Sql = "insert into tblstudentattendance(StudentId,DayId)VALUES(" + _StutendId + "," + _dateId + ")";
                //m_TransationDb.TransExecuteQuery(Sql);
            Sql = "insert into tblstudentattendance(StudentId,DayId,LeaveRequest,Status)VALUES(" + _StutendId + "," + _dateId + ",0," + Status + ")";
            m_TransationDb.TransExecuteQuery(Sql);  
            
           
        }

        public bool CheckAttadanceStatus(DateTime _getdate, string _typ, int _clsId)
        {
           
            bool _checkStatus = false;
            Sql = "select tbldate.Id from  tbldate  inner join tblmasterdate on tblmasterdate.id=tbldate.dateId where tblmasterdate.Date ='" + _getdate.ToString("yyyy-MM-dd") + "'and tbldate.Status='" + _typ + "' and tbldate.classId= " + _clsId;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
           
            if (m_MyReader.HasRows)
            {
                _checkStatus = true;
            }
            return _checkStatus;
        }

        //this fution is used to get the dateId from the tblDate ::used in:: Btn_EditAttandance_Click fuction ater geting the date id from tblmasterdate//
        public int getDateIdFrmDateTable(int _redateID, string _typ, int _clsId)
        {
           
            int _dateId = 0;
            Sql = "select tbldate.Id from tbldate where tbldate.DateId =" + _redateID + " and tbldate.Status='" + _typ + "'and tbldate.classId=" + _clsId;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            
            if (m_MyReader.HasRows)
            {
                m_MyReader.Read();
                _dateId = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            return _dateId;
        }

        //This Fuction is used to find whether  the Staff is Present In Table StaffAttandance 
        public bool Find_IfTheStaff_IsPresent_Inthe_tblstaffAttandence(int _StaffId, int _dateId, int _PeriodId)
        {
            
            bool _present = false;
            
            //Sql = " select tblstaffattendance.StaffId from tblstaffattendance where tblstaffattendance.StaffId=" + _StaffId + " and tblstaffattendance.DayId=" + _dateId;
            //m_MyReader = m_TransationDb.ExecuteQuery(Sql);
           
            Sql = " select tblstaffattendance.StaffId from tblstaffattendance where tblstaffattendance.StaffId=" + _StaffId + " and tblstaffattendance.DayId=" + _dateId+" AND tblstaffattendance.PeriodId="+_PeriodId;
            m_MyReader = m_TransationDb.ExecuteQuery(Sql);
           
            if (m_MyReader.HasRows)
            {

                _present = true;
            }
            return _present;
        }
        //this fuction is used to delete the entry from the tblstaffattendance //
        public void DeletetheStfAtceTbl(int _StaffId, int _dateId)
        {
            Sql = "delete from tblstaffattendance where tblstaffattendance.StaffId=" + _StaffId + "  and tblstaffattendance.DayId=" + _dateId;
            m_TransationDb.TransExecuteQuery(Sql);
            Sql = "delete from tblstaffattendancedetails where tblstaffattendancedetails.StaffId=" + _StaffId + "  and tblstaffattendancedetails.DayId=" + _dateId;
            m_TransationDb.TransExecuteQuery(Sql);
              
        }
        //This fuction is used to Edit Staff Weekly Attendance ::USed in SelectDifferMode Fution//

        public bool checkOfattendance(DateTime _ToDaysDate, int _StaffId)
        {
            bool _flag = false;
            CLogging logger = CLogging.GetLogObject();

            Sql = "select  tblstaffattendance.StaffId from   tblstaffattendance inner join tbldate on tbldate.Id = tblstaffattendance.DayId inner join tblmasterdate on tblmasterdate.Id= tbldate.DateId where tblmasterdate.`date`='" + _ToDaysDate.ToString("yyyy-MM-dd") + "' and tblstaffattendance.StaffId=" + _StaffId;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            logger.LogToFile("checkOfattendance", " Select the staff id From date attendance table", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
            if (m_MyReader.HasRows)
            {
                _flag = true;
            }
            return _flag;



        }


        public bool Find_IfTheStundt_IsPrnt_Inthe_tblStuAttandence(int _StutendId, int _dateId, int _PeriodId)
        {
           
            bool _present = false;
            Sql = " select tblstudentattendance.DayId from tblstudentattendance  where tblstudentattendance.DayId=" + _dateId + " and tblstudentattendance.StudentId=" + _StutendId + " AND tblstudentattendance.PeriodId=" + _PeriodId;
            m_MyReader = m_TransationDb.ExecuteQuery(Sql);
            //Sql = " select tblstudentattendance.DayId from tblstudentattendance  where tblstudentattendance.DayId=" + _dateId + " and tblstudentattendance.StudentId=" + _StutendId;
            //m_MyReader = m_TransationDb.ExecuteQuery(Sql);
             if (m_MyReader.HasRows)
            {

                _present = true;
            }
            return _present;
        }

        public void deleteStudentAtt(int _StutendId, int _dateId)
        {

            Sql = "delete from tblstudentattendance where tblstudentattendance.StudentId=" + _StutendId + "  and tblstudentattendance.DayId=" + _dateId;
            m_TransationDb.TransExecuteQuery(Sql);


            Sql = "delete from tblstudentattendancedetails where tblstudentattendancedetails.StudentId=" + _StutendId + "  and tblstudentattendancedetails.DayId=" + _dateId;
            m_TransationDb.TransExecuteQuery(Sql);
               
           
        }

        public bool checkOfstudentattendance(DateTime _ToDaysDate, int _StaffIdFrmGrid)
        {
            bool _flag = false;
            CLogging logger = CLogging.GetLogObject();
            Sql = " select tblstudentattendance.StudentId from tblstudentattendance inner join tbldate on tbldate.id= tblstudentattendance.DayId inner join tblmasterdate on tblmasterdate.Id= tbldate.DateId  where tblmasterdate.`date`='" + _ToDaysDate.ToString("yyyy-MM-dd") + "' and tblstudentattendance.StudentId=" + _StaffIdFrmGrid;
            // "select tblstudentattendance.StudentId from tblstudentattendance INNER join tbldate on  tbldate.Id = tblstudentattendance.DayId   inner join tblmasterdate on tblmasterdate.Id= tbldate.DateId where tblmasterdate.`date`='" + _ToDaysDate.ToString("yyyy-MM-dd ") + "'and tblstudentattendance.StudentId=" + _StaffIdFrmGrid; 
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            logger.LogToFile("checkOfstudentattendance", " Select the staff id From date attendance table", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
            if (m_MyReader.HasRows)
            {
                _flag = true;
            }
            return _flag;
        }

        public bool checkTheBatchDate(DateTime _getDate, int _batchId)
        {
            bool _flag = false;
            DateTime _startDate;
            Sql = "select tblbatch.StartDate , tblbatch.EndDate from tblbatch where tblbatch.Id=" + _batchId;
            if (m_TransationDb == null)
            {
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            }
            else
            {
                m_MyReader = m_TransationDb.ExecuteQuery(Sql);
            }
            if (m_MyReader.HasRows)
            {
              _startDate = DateTime.Parse(m_MyReader.GetValue(0).ToString());
              //  _startDate = Myuser.GetDareFromText(m_MyReader.GetValue(0).ToString());
                if (_getDate >= _startDate)
                {
                    _flag = true;
                    //DateTime _today;
                    //_today = DateTime.Now;
                    
                    //if (_getDate <= _today)
                    //{
                    //    _flag = true;
                    //}
                }
            }
            return _flag;

        }
        public void updateIncedentStd(int _StutendId, int _dateId, int _ClsId, DateTime _date)
        {
            string _firstDate = "";
            string _secondDate = "";

            string[] date = new string[10];
            int i = 0;
            string _tempdate;
        xy: _date = _date.AddDays(-1);
            _tempdate = _date.ToString("dddd");
            if (!presentInDefault_HolidayList(_tempdate))
            {

                if ((!holiday(_date, _ClsId)))
                {
                    int _IdOfdate = getDateIdFrmMastTbl(_date);
                    // Sql = "select tblmasterdate.Id  from tblmasterdate where tblmasterdate.`date`='" + _date.ToString("yyyy-MM-dd") + "'";
                    Sql = "SELECT tbldate.Id from tbldate where tbldate.classId=" + _ClsId + " and  tbldate.DateId=" + _IdOfdate + " and tbldate.Status='class'";
                    m_MyReader = m_TransationDb.ExecuteQuery(Sql);
                    if (m_MyReader.HasRows)
                    {
                        int _idof_tbldate = int.Parse(m_MyReader.GetValue(0).ToString());

                        if (CheckPresentInTheAttenTbl(_StutendId, _idof_tbldate) == true)
                        {

                            date[i] = _date.ToString("dd-MMM-yyyy");
                            i++;
                            goto xy;
                        }
                    }
                }
                else
                {
                    goto xy;
                }
            }
            else
            {
                goto xy;
            }

            if (i > 0)
            {
                // int i = ReturnValueFrmIncedentRecurssion;
                int j = 0;
                while (j != i)
                {
                    _firstDate = date[j];
                    _secondDate = _firstDate + "," + _secondDate;
                    j++;
                }
                General _GenObj = new General(m_MysqlDb);
                int _Incedentid = _GenObj.GetTableMaxId("tblview_incident", "Id");
                DateTime _Now = System.DateTime.Now;
                string _Desc = "  Absent For " + i + "  Days and the Days Are :" + _secondDate;
                string sql = " insert into tblincedent (Id,Title,Description,IncedentDate,CreatedDate,TypeId,ApproverId,CreatedUserId,UserType,Status,AssoUser)values (" + _Incedentid + ",'Attendance','" + _Desc + "','" + _Now.ToString("s") + "','" + _Now.ToString("s") + "',1,0,1,'student','Approved'," + _StutendId + " )";
                m_TransationDb.TransExecuteQuery(sql);
            }
        }

        public void updateIncedentStudent(int _StutendId, int _dateId, int _ClsId, DateTime _date, int _UserId, int _CurrentBatchId)
        {
            string _firstDate = "";
            string _secondDate = "";

                string[] date = new string[800];
                int i = 0;
                string _tempdate;
            xy: _date = _date.AddDays(-1);
                _tempdate = _date.ToString("dddd");
                if (checkTheBatchDate(_date, _CurrentBatchId))
                {
                    if ((!presentInDefault_HolidayList(_tempdate)) && (!holiday(_date, _ClsId)))
                    {
                        int _IdOfdate = getDateIdFrmMastTbl(_date);
                        // Sql = "select tblmasterdate.Id  from tblmasterdate where tblmasterdate.`date`='" + _date.ToString("yyyy-MM-dd") + "'";
                        Sql = "SELECT tbldate.Id from tbldate where tbldate.classId=" + _ClsId + " and  tbldate.DateId=" + _IdOfdate + " and tbldate.Status='class'";
                        m_MyReader = m_TransationDb.ExecuteQuery(Sql);
                        if (m_MyReader.HasRows)
                        {
                            int _idof_tbldate = int.Parse(m_MyReader.GetValue(0).ToString());

                            if (CheckPresentInTheAttenTbl(_StutendId, _idof_tbldate) == true)
                            {

                                date[i] = _date.ToString("dd-MMM-yyyy");
                                i++;
                                goto xy;
                            }
                        }
                    }
                    else
                    {
                        goto xy;
                    }
                }
                
                if (i > 0)
                {
                    // int i = ReturnValueFrmIncedentRecurssion;
                    int j = 0;
                    while (j != i)
                    {
                        _firstDate = date[j];
                        _secondDate = _firstDate + "," + _secondDate;
                        j++;
                    }

                    General _GenObj = new General(m_MysqlDb);
                    int _Incedentid = _GenObj.GetTableMaxId("tblview_incident", "Id");
                    DateTime _Now = System.DateTime.Now;
                    string _Desc = "  Absent For " + i + "  Days and the Days Are :" + _secondDate;
                    string sql = " insert into tblincedent (Id,Title,Description,IncedentDate,CreatedDate,TypeId,ApproverId,CreatedUserId,UserType,Status,AssoUser)values ("+_Incedentid+",'Attendance','" + _Desc + "','" + _Now.ToString("s") + "','" + _Now.ToString("s") + "',1,0," + _UserId + ",'student','Approved'," + _StutendId + " )";
                    m_TransationDb.TransExecuteQuery(sql);
                }
        }

        public void updateIncedentStaff(int _StaffId, int _dateId, DateTime _date, int _UserId, int _CurrentBatchId)
        {

            string _firstDate = "";
            string _secondDate = "";
            int i = 0;
            string[] date = new string[800];
            
            string _tempdate;
        xy: _date = _date.AddDays(-1);
            _tempdate = _date.ToString("dddd");
            if (checkTheBatchDate(_date, _CurrentBatchId))
            {
                if((!presentInDefault_HolidayListForStaff(_tempdate))&&(!holidaystaff(_date)))
                {
                        int _IdOfdate = getDateIdFrmMastTbl(_date);
                        // Sql = "select tblmasterdate.Id  from tblmasterdate where tblmasterdate.`date`='" + _date.ToString("yyyy-MM-dd") + "'";
                        Sql = "SELECT tbldate.Id from tbldate where tbldate.classId=0 and  tbldate.DateId=" + _IdOfdate + " and tbldate.Status='staff'";
                        m_MyReader = m_TransationDb.ExecuteQuery(Sql);
                        if (m_MyReader.HasRows)
                        {
                            int _idof_tbldate = int.Parse(m_MyReader.GetValue(0).ToString());

                            if (CheckPresentInTheAttenTblStaff(_StaffId, _idof_tbldate) == true)
                            {

                                date[i] = _date.ToString("dd-MMM-yyyy");
                                i++;
                                goto xy;
                            }

                        }

                }                
                else
                {
                  goto xy;
                }
            }
            

            if (i > 0)
            {
                int j = 0;
                while (j != i)
                {
                    _firstDate = date[j];
                    _secondDate = _firstDate + "," + _secondDate;
                    j++;
                }

                General _GenObj = new General(m_MysqlDb);
                int _Incedentid = _GenObj.GetTableMaxId("tblview_incident", "Id");
                DateTime _Now = System.DateTime.Now;
                string _Desc = " Absent For " + i + "  Days and the Days Are :" + _secondDate;
                string sql = " insert into tblincedent (Id,Title,Description,IncedentDate,CreatedDate,TypeId,ApproverId,CreatedUserId,UserType,Status,AssoUser)values ("+_Incedentid+",'Attendance','" + _Desc + "','" + _Now.ToString("s") + "','" + _Now.ToString("s") + "',1,0," + _UserId + ",'staff','Approved'," + _StaffId + " )";
                m_TransationDb.TransExecuteQuery(sql);              
            }


        }
       
        
        public bool holidaystaff(DateTime _date)
        {
            bool _flag = false;
            int dateid = getDateIdFrmMastTbl(_date);   
            //select tblholiday.Id from tblholiday where tblholiday.Type='all' and tblholiday.Class_Id=0 and tblholiday.GroupId 
            //Sql = "select tblHoliday.Id from tblHoliday inner join tblmasterdate on tblmasterdate.Id = tblHoliday.DateId where tblmasterdate.`date`='" + _date.ToString("yyyy-MM-dd") + "' and (tblHoliday.`Type`='staff' or tblHoliday.`Type`='all')";
            Sql="select  tblholiday.Id from tblholiday WHERE tblholiday.`Type`='all' AND tblholiday.Class_Id=0 AND tblholiday.dateId="+ dateid;
         
            if(m_TransationDb!=null)
            m_MyReader = m_TransationDb.ExecuteQuery(Sql);
          else
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
           
            if (m_MyReader.HasRows)
            {
                _flag = true;
            }
            return _flag;
        }

        public bool holiday(DateTime _date, int clsId)
        {
            bool _flag = false;
            int dateid = getDateIdFrmMastTbl(_date);

            Sql = "select tblholiday.Id from tblholiday where tblholiday.Id in   (select  tblholiday.Id from tblholiday WHERE tblholiday.`Type`='class' AND tblholiday.Class_Id=" + clsId + " AND tblholiday.dateId=" + dateid + ") OR  tblholiday.Id in (select  tblholiday.Id from tblholiday  WHERE  tblholiday.`Type`='all' AND tblholiday.dateId=" + dateid + ")";
            
            if(m_TransationDb!=null)
                m_MyReader = m_TransationDb.ExecuteQuery(Sql);
            else
                m_MyReader= m_MysqlDb.ExecuteQuery(Sql);
            
            if (m_MyReader.HasRows)
            {
                _flag = true;
            }
            return _flag;
        }

        private bool CheckPresentInTheAttenTbl(int _StutendId, int _idofdate_table)
        {
            bool _flag = false;
          //  Sql = "select tblstudentattendance.DayId from tblstudentattendance INNER join tbldate on tbldate.Id = tblstudentattendance.DayId inner join tblmasterdate on tblmasterdate.Id = tbldate.DateId where tblmasterdate.Id=" + _idofdate + " and tblstudentattendance.StudentId=" + _StutendId;
            Sql = "SELECT tblstudentattendance.Id from tblstudentattendance where tblstudentattendance.DayId=" + _idofdate_table + " AND tblstudentattendance.StudentId=" + _StutendId;

            if (m_TransationDb != null)
                m_MyReader = m_TransationDb.ExecuteQuery(Sql);
            else
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);

            if (m_MyReader.HasRows)
            {
                _flag = true;
            }
            return _flag;

        }

      

        public void UpdateIncedentTbl(int _StaffId, int _dateId, DateTime _date, int _UserID)
        {
            try
            {
                string _firstDate = "";
                string _secondDate = "";
                string[] date = new string[10];
                int i = 0;
                string _tempdate;
            xy: _date = _date.AddDays(-1);
                _tempdate = _date.ToString("dddd");
                if (presentInDefault_HolidayList(_tempdate))
                {
                    if (!holidaystaff(_date))
                    {
                        Sql = "select tblmasterdate.Id  from tblmasterdate where tblmasterdate.`date`='" + _date.ToString("yyyy-MM-dd") + "'";
                        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
                        if (m_MyReader.HasRows)
                        {
                            int _id = int.Parse(m_MyReader.GetValue(0).ToString());
                            if (CheckPresentInTheAttenTblStaff(_StaffId, _id) == true)
                            {

                                date[i] = _date.ToString("dd-MMM-yyyy");
                                i++;
                                goto xy;
                            }

                        }

                    }
                    else
                    {
                        goto xy;
                    }

                }
                else
                {
                    goto xy;
                }





                if (i > 0)
                {
                    int j = 0;
                    while (j != i)
                    {
                        _firstDate = date[j];
                        _secondDate = _firstDate + "," + _secondDate;
                        j++;
                    }

                    General _GenObj = new General(m_MysqlDb);
                    int _Incedentid = _GenObj.GetTableMaxId("tblview_incident", "Id");
                    DateTime _Now = System.DateTime.Now;
                    string _Desc = " Absent For " + i + "  Days and the Days Are :" + _secondDate;
                    string sql = " insert into tblincedent (Id,Title,Description,IncedentDate,CreatedDate,TypeId,ApproverId,CreatedUserId,UserType,Status,AssoUser)values ("+_Incedentid+",'Attendance','" + _Desc + "','" + _Now.ToString("s") + "','" + _Now.ToString("s") + "',1,0,1,'staff','Approved'," + _StaffId + " )";
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                }
            }
            catch (Exception e)
            {
                CLogging logger = CLogging.GetLogObject();
                logger.LogToFile("UpdateIncedentTbl", " Error during the writing incident to the tbl " + e.Message + "", 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
            }

        }



        private bool CheckPresentInTheAttenTblStaff(int _StaffId, int _idofdate_table)
        {
            bool _flag = false;
            Sql = "SELECT tblstaffattendance.Id from tblstaffattendance where tblstaffattendance.DayId=" + _idofdate_table + " AND tblstaffattendance.StaffId="+ _StaffId;
            m_MyReader = m_TransationDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _flag = true;
            }
            return _flag;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_StaffId"></param>
        /// <param name="_dateId"></param>
        /// <param name="_date"></param>
        /// <param name="_UserID"></param>
        /// < Funtion used by this funtion CheckHolidayAndsunday,CheckForPreviousDayAttendanceHasMasker,CheckForFirstValueinDatetbl</param>
        /// <returns></returns> 
        /// 
        public bool CheckForPrevioudDay(int _batchId, DateTime _getdate, int _clsId, string _typ, out string message)
        {
            message = ""; 
            bool _CheckForPreviousDayFlag = false;
            DateTime _ReturnDate;
            if (selected_dateis_batch_StartingDate(_batchId, _getdate)) //check its the batch starting date if yes no need to check anything
            {
                _CheckForPreviousDayFlag = true;
            }
            else
            {
                if (CheckForFirstValueinDatetbl(_typ, _clsId)) //check in any date attendance is marked return true if yes 
                {
                    _getdate = _getdate.AddDays(-1);

                    CheckHolidayAndsunday(_getdate, out _ReturnDate, _typ, _clsId);  // _ReturnDate = previous working day



                    if (CheckForPreviousDayAttendanceHasMarked(_ReturnDate, _typ, _clsId)) //check attendance marked for the _ReturnDate
                    {
                        _CheckForPreviousDayFlag = true;
                    }

                    else //attendance not marked for the _ReturnDate find the The Last Valid DayFor Attendance
                    {
                        string _message = "";
                        getTheLastValiedDayForAttendance(_typ, _clsId, out _message);

                        message = "Mark Attendance on " + _message;
                     
                        _CheckForPreviousDayFlag = false;
                    }
                }

                else if (batchStartingday(_batchId, _getdate, out message))  //check the given date is batch starting date
                {
                    _CheckForPreviousDayFlag = true;


                }
            }
            return _CheckForPreviousDayFlag;

        }
        
        public bool CheckForPreviousDay(int _batchId, DateTime _getdate, int _clsId, string _typ, out string message,out DateTime _correctdate)
        {
            message = ""; _correctdate = _getdate;
            bool _CheckForPreviousDayFlag = false;
            DateTime _ReturnDate;
            if (selected_dateis_batch_StartingDate(_batchId, _getdate)) //check its the batch starting date if yes no need to check anything
            {
                _CheckForPreviousDayFlag = true;
            }
            else
            {
                if (CheckForFirstValueinDatetbl(_typ, _clsId)) //check in any date attendance is marked return true if yes 
                {
                    _getdate = _getdate.AddDays(-1);

                    CheckHolidayAndsunday(_getdate, out _ReturnDate, _typ, _clsId);  // _ReturnDate = previous working day



                    if (CheckForPreviousDayAttendanceHasMarked(_ReturnDate, _typ, _clsId)) //check attendance marked for the _ReturnDate
                    {
                        _CheckForPreviousDayFlag = true;
                    }

                    else //attendance not marked for the _ReturnDate find the The Last Valid DayFor Attendance
                    {
                        string _message = "";
                        getTheLastValiedDayForAttendance(_typ, _clsId, out _message);

                        message = "Mark Attendance on " + _message;
                        _correctdate = General.GetDateTimeFromText(_message);
                        _CheckForPreviousDayFlag = false;
                    }
                }

                else if (_Is_batchStartingday(_batchId, _getdate, out message, out _correctdate))  //check the given date is batch starting date
                {
                    _CheckForPreviousDayFlag = true;


                }
            }
            return _CheckForPreviousDayFlag;
           
        }
        public bool CheckForPrevioudDay_Staff(int _batchId, DateTime _getdate, string _typ, out string message,out DateTime _correctdate)
        {
            message = ""; _correctdate = _getdate;
            bool _CheckForPreviousDayFlag = false;
            DateTime _ReturnDate;        
            if (selected_dateis_batch_StartingDate(_batchId, _getdate)) //check its the batch starting date if yes no need to check anything
            {
                _CheckForPreviousDayFlag = true;
            }
            else
            {
                if (CheckForFirstValueinDatetbl(_typ, 0)) //0 is class_id   //check in any date attendance is marked return true if yes 
                {
                    _getdate = _getdate.AddDays(-1);

                    CheckHolidayAndsunday_ForStaff(_getdate, out _ReturnDate, _typ);  // _ReturnDate = previous working day

                    if (CheckForPreviousDayAttendanceHasMarked(_ReturnDate, _typ, 0)) // 0 is class id .check attendance marked for the _ReturnDate
                    {
                        _CheckForPreviousDayFlag = true;
                    }

                    else //attendance not marked for the _ReturnDate find the The Last Valid DayFor Attendance
                    {
                        string _message = "";
                        getTheLastValiedDayForAttendance_ForStaff(_typ, out _message);

                        message = "Mark Attendance on " + _message;
                        _correctdate = General.GetDateTimeFromText(_message);
                        _CheckForPreviousDayFlag = false;
                    }
                }

                else if (_Is_batchStartingday(_batchId, _getdate, out message,out _correctdate))  //check the given date is batch starting date
                {
                    _CheckForPreviousDayFlag = true;


                }
            }

            return _CheckForPreviousDayFlag;
        }

        private bool selected_dateis_batch_StartingDate(int _batchId,DateTime _getdate)
        {
            bool startingdate = false;
            DateTime _startDate;
            Sql = "select tblbatch.StartDate , tblbatch.EndDate from tblbatch where tblbatch.Id=" + _batchId;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _startDate = DateTime.Parse(m_MyReader.GetValue(0).ToString());

                if (_getdate == _startDate)
                {
                    startingdate = true;
                }
            }
            return startingdate;
        }
        private void getTheLastValiedDayForAttendance_ForStaff(string _typ,out string _message)
        {
            _message = "";
            DateTime _tempValue;
            string _tempStringValue;
            Sql = "select tblmasterdate.`date` from tblmasterdate inner join tbldate on tbldate.DateId = tblmasterdate.Id  where (tbldate.Status='" + _typ + "' or tbldate.Status='all') and tbldate.classId=0 ORDER by tblmasterdate.`date` DESC ";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);

            if (m_MyReader.HasRows)
            {
                _tempValue = DateTime.Parse(m_MyReader.GetValue(0).ToString());
                _tempValue = _tempValue.AddDays(1);
            xy: _tempStringValue = _tempValue.ToString("dddd");
                if (!presentInDefault_HolidayListForStaff(_tempStringValue))
                {
                        if (!holidaystaff(_tempValue))
                        {
                            _message = General.GerFormatedDatVal( _tempValue);
                        }
                        else
                        {
                            _tempValue = _tempValue.AddDays(1);
                            goto xy;
                        }                  

                }
                else
                {
                    _tempValue = _tempValue.AddDays(1);
                    goto xy;
                }
            }

        }

        private void CheckHolidayAndsunday_ForStaff(DateTime _date, out DateTime _ReturnDate, string _typ)
        {
            string _tempdate = _date.ToString("dddd");
           
            if (!presentInDefault_HolidayListForStaff(_tempdate))
            {
                
                    if (holidaystaff(_date))
                    {
                        _date = _date.AddDays(-1);
                        CheckHolidayAndsunday_ForStaff(_date, out  _ReturnDate, _typ);
                    }
                    else
                    {
                        _ReturnDate = _date;
                    }
                
               
            }
            else
            {
               
                    _date = _date.AddDays(-1);
                    if (holidaystaff(_date))
                    {
                        _date = _date.AddDays(-1);
                        CheckHolidayAndsunday_ForStaff(_date, out  _ReturnDate, _typ);

                    }
                    else
                    {
                        _ReturnDate = _date;
                    }
                  
            }
        }
      
        private void getTheLastValiedDayForAttendance(string _typ, int _clsId, out string _message)
        {
            _message = "";
            DateTime _tempValue;
            string _tempStringValue;
            Sql = "select tblmasterdate.`date` from tblmasterdate inner join tbldate on tbldate.DateId = tblmasterdate.Id  where (tbldate.Status='" + _typ + "' or tbldate.Status='all') and tbldate.classId= " + _clsId + " ORDER by tblmasterdate.`date` DESC ";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);

            if (m_MyReader.HasRows)
            {
                _tempValue = DateTime.Parse(m_MyReader.GetValue(0).ToString());
                _tempValue = _tempValue.AddDays(1);
            xy: _tempStringValue = _tempValue.ToString("dddd");
                if (!presentInDefault_HolidayList(_tempStringValue))
                {
                    //if (_typ == "staff")
                    //{
                    //    if (!holidaystaff(_tempValue))
                    //    {
                    //        _message = _tempValue.ToString("dd-MMM-yyyy");
                    //    }
                    //    else
                    //    {
                    //        _tempValue = _tempValue.AddDays(1);
                    //        goto xy;
                    //    }
                    //}
                    //else
                    //{
                        if (!holiday(_tempValue, _clsId))
                        {
                            _message = General.GerFormatedDatVal(_tempValue);
                        }
                        else
                        {
                            _tempValue = _tempValue.AddDays(1);
                            goto xy;
                        }
                    //}

                }
                else
                {
                    _tempValue = _tempValue.AddDays(1);
                    goto xy;
                }
            }


        }

        private bool CheckForFirstValueinDatetblForStudent()
        {
            throw new NotImplementedException();
        }
        private bool _Is_batchStartingday(int _batchId, DateTime _date, out string message, out DateTime _correctdate)
        {
            message = ""; _correctdate = _date;
            bool _flag = false;
            DateTime _startDate;
            // _date = _date.AddDays(1);          
            Sql = "select tblbatch.StartDate  from tblbatch where tblbatch.Id=" + _batchId;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);

            if (m_MyReader.HasRows)
            {
                _startDate = DateTime.Parse(m_MyReader.GetValue(0).ToString());
                if (_date == _startDate)
                {
                    _flag = true;

                }
                else
                {
                    message = "Mark the Attendance on " + _startDate.ToString("dd-MMM-yyyy ");
                    _correctdate = _startDate;
                }

            }
            else
            {
                message = "Batch Is not Created Please Create the Batch to get the Starting date";
                _flag = false;

            }
            return _flag;




        }

        private bool batchStartingday(int _batchId, DateTime _date, out string message)
        {
            message = ""; 
            bool _flag = false;
            DateTime _startDate;
            // _date = _date.AddDays(1);          
            Sql = "select tblbatch.StartDate  from tblbatch where tblbatch.Id=" + _batchId;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
         
            if (m_MyReader.HasRows)
            {
                _startDate = DateTime.Parse(m_MyReader.GetValue(0).ToString());
                if (_date == _startDate)
                {
                    _flag = true;

                }
                else
                {
                    message = "Mark the Attendance on " + _startDate.ToString("dd-MMM-yyyy ");
                  
                }

            }
            else
            {
                message = "Batch Is not Created Please Create the Batch to get the Starting date";
                _flag = false;

            }
            return _flag;




        }


        /// <summary>
        /// This funtion check holiday and sunday and return the previous working day
        /// </summary>
        /// <param name="_date"></param>
        /// <param name="_ReturnDate"></param>
        public void CheckHolidayAndsunday(DateTime _date, out DateTime _ReturnDate, string _typ, int clsId)
        {

            string _tempdate = _date.ToString("dddd");
            if (!presentInDefault_HolidayList(_tempdate))
            {
               
                if (holiday(_date, clsId))
                    {
                        _date = _date.AddDays(-1);
                        CheckHolidayAndsunday(_date, out  _ReturnDate, _typ, clsId);
                    }
                    else
                    {
                        _ReturnDate = _date;
                    }

               
            }
            else
            {
                
                    _date = _date.AddDays(-1);
                    if (holiday(_date, clsId))
                    {
                        _date = _date.AddDays(-1);
                        CheckHolidayAndsunday(_date, out  _ReturnDate, _typ, clsId);

                    }
                    else
                    {
                        _ReturnDate = _date;
                    }
            }

        }
        /// <summary>
        /// This Funtion is used to Check the  Whether the attendance have been marked for previous day
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>


        private bool CheckForPreviousDayAttendanceHasMarked(DateTime _date, string _typ, int clsId)
        {
            bool _flag = false;
            Sql = "select tbldate.Id from tbldate inner join tblmasterdate on tbldate.DateId= tblmasterdate.Id where tbldate.`Status`='" + _typ + " ' and tblmasterdate.`date`='" + _date.ToString("yyyy-MM-dd ") + "' and tbldate.classId=" + clsId;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _flag = true;
            }
            return _flag;


        }
        /// <summary>
        /// This Funtion is used to Check Whethar the the Marked Attendance is the first Attendance::
        /// </summary>
        /// <returns></returns>

        private bool CheckForFirstValueinDatetbl(string _typ, int clasId)
        {
            bool _flag = false;
            Sql = "select tbldate.Id from tbldate where (tbldate.`Status`='" + _typ + "') and tbldate.classId=" + clasId;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _flag = true;
            }
            return _flag;
        }

     
        // This Funtion is used to Display Total workin day of a perticular student ;this funtion ids used int StudentAttendanceReport.aspx page


        private int getNoOfAbsentDays(string _StringMonth,  int _studentId)
        {
            int _TotalAbsentdays = 0;
            Sql = "select count(tblstudentattendance.Id) from tblstudentattendance inner join tbldate on tbldate.Id= tblstudentattendance.DayId inner join tblmasterdate on tbldate.DateId= tblmasterdate.Id where tblstudentattendance.StudentId=" + _studentId + " and ( monthname ( tblmasterdate.`date`) ='" + _StringMonth + "')";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalAbsentdays = int.Parse(m_MyReader.GetValue(0).ToString());
                
            }
            return _TotalAbsentdays;
        }

        public int getTotalWorkingDaysFortheMonth(int _MonthIdFromDrp_month, int _studentId, int _batchId, out string message, out int _TotalAbsentdays, out int _TotalHoliday, out string _StringMonth, out int _ClsId)
        {
           _StringMonth="";
           _TotalAbsentdays = 0;
           _TotalHoliday = 0;
           message = "";
           int _totalday = 0;
           _ClsId=0;
           Sql = "select tblstudentclassmap.ClassId from tblstudentclassmap where tblstudentclassmap.BatchId=" + _batchId + " and tblstudentclassmap.StudentId=" + _studentId;
           m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
           if (m_MyReader.HasRows)
            {
                _ClsId = int.Parse(m_MyReader.GetValue(0).ToString());

                Sql = " select tblmonth.`Month` from tblmonth where tblmonth.Id=" + _MonthIdFromDrp_month;
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
                if (m_MyReader.HasRows)
                {
                    _StringMonth = (m_MyReader.GetValue(0).ToString());

                    Sql = " select count(tbldate.Id) from tbldate inner join tblmasterdate on tblmasterdate.Id= tbldate.DateId  where (monthname (tblmasterdate.`date`)='" + _StringMonth + "') or tbldate.`Status`='class' and tbldate.classId=" + _ClsId;
                    m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
                    if (m_MyReader.HasRows)
                    {
                        _totalday = int.Parse(m_MyReader.GetValue(0).ToString());


                        if (_totalday != 0)
                        {
                            _TotalAbsentdays = getNoOfAbsentDays(_StringMonth,_studentId);
                            _TotalHoliday =getNoOfHolidays(_StringMonth, _ClsId);

                        }
                        else
                        {
                            message = " Select Another Month No Class is  Reported on this month";

                        }
                    }
                    else
                    {
                        message = " Select Another Month No Class is  Reported on this month";

                    }
                }
                else
                {
                    message = "Month is not Valied";

                }
            }
            else
            {
                message = "This User is Not Valied";

            }

            return _totalday;


        }

        private int getNoOfHolidays(string _StringMonth, int _ClsId)
        {
            int _TotalHoliday=0;
            Sql = "select count( tblHoliday.Id) from tblHoliday inner join tblmasterdate on tblmasterdate.Id= tblHoliday.dateId where (monthname( tblmasterdate.`date`)='" + _StringMonth + "') and (tblHoliday.`Type`='class' or tblHoliday.Class_Id=" + _ClsId + " or   tblHoliday.`Type`='all')";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
              {
                  _TotalHoliday = int.Parse(m_MyReader.GetValue(0).ToString());

              }
             return _TotalHoliday;

        }

        public int getTotalWorkingDaysFortheMonthForStaff(int _MonthIdFromDrp_month, int _StaffId, int _batchId, out string message, out int _TotalAbsentdays, out int _TotalHoliday, out string _StringMonth, out int _ClsId)
        {



            {
                _StringMonth = "";
                _TotalAbsentdays = 0;
                _TotalHoliday = 0;
                message = "";
                int _totalday = 0;
                _ClsId = 0;
                
                    Sql = " select tblmonth.`Month` from tblmonth where tblmonth.Id=" + _MonthIdFromDrp_month;
                    m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
                    if (m_MyReader.HasRows)
                    {
                        _StringMonth = (m_MyReader.GetValue(0).ToString());

                        Sql = " select count(tbldate.Id) from tbldate inner join tblmasterdate on tblmasterdate.Id= tbldate.DateId  where (monthname (tblmasterdate.`date`)='" + _StringMonth + "') and tbldate.`Status`='staff'";
                        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
                        if (m_MyReader.HasRows)
                        {
                            _totalday = int.Parse(m_MyReader.GetValue(0).ToString());


                            if (_totalday != 0)
                            {
                                _TotalAbsentdays = getNoOfAbsentDaysForStaff(_StringMonth, _StaffId);
                                _TotalHoliday = getNoOfHolidaysForStaff(_StringMonth);

                            }
                            else
                            {
                                message = " Select Another Month No Class is  Reported on this month";

                            }
                        }
                        else
                        {
                            message = " Select Another Month No Class is  Reported on this month";

                        }
                    }
                    else
                    {
                        message = "Month is not Valied";

                    }
             //   }
              //  else
              //  {
                //    message = "This User is Not Valied";

//}
//
                return _totalday;


            }

        }

        private int getNoOfHolidaysForStaff(string _StringMonth)
        {

            {
                int _TotalHoliday = 0;
                Sql = "select count( tblHoliday.Id) from tblHoliday inner join tblmasterdate on tblmasterdate.Id= tblHoliday.dateId where (monthname( tblmasterdate.`date`)='" + _StringMonth + "') and (tblHoliday.`Type`='staff' or tblHoliday.`Type`='all')";
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
                if (m_MyReader.HasRows)
                {
                    _TotalHoliday = int.Parse(m_MyReader.GetValue(0).ToString());

                }
                return _TotalHoliday;

            }
        }

        private int getNoOfAbsentDaysForStaff(string _StringMonth, int _StaffId)
        {
            int _TotalAbsentdays = 0;
            Sql = "select count( tblstaffattendance.Id) from tblstaffattendance inner join tbldate on tbldate.Id= tblstaffattendance.DayId inner join tblmasterdate on tbldate.DateId= tblmasterdate.Id where tblstaffattendance.StaffId=" + _StaffId + " and ( monthname ( tblmasterdate.`date`) ='" + _StringMonth + "')";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalAbsentdays = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalAbsentdays;
        }
     

        public DataSet MyAssociatedHolidays(string _type, int _classId,int _userid)
        {
            DataSet holidays = null;
           
          string sql = "select distinct tblmasterdate.date, tblholiday.Type, tblholiday.Desc from tblmasterdate inner join tblholiday on tblholiday.dateId= tblmasterdate.Id  where";
            if (_type == "class")
            {
                if (_classId != 0)
                    sql += " (tblholiday.Type='class' and tblholiday.Class_Id=0) or ( tblholiday.Class_Id=" + _classId + ") or(tblholiday.Type='all')";

                else
                    sql += " (tblholiday.Type='class' and tblholiday.Class_Id=0) or(tblholiday.Type='all')"; //means all class
            }
            else
                sql += " tblholiday.Type='all'";
            
            
           

            holidays = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return holidays;
        }

        private DataSet associated_groups(int _userid)
        {
            if (m_groups == null)
            {
                Sql = "SELECT DISTINCT tblgroup.Id, tblgroup.GroupName FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" +_userid + " UNION SELECT DISTINCT tblgroup.Id, tblgroup.GroupName FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId="+_userid;
             
                if (m_TransationDb != null)
                    m_groups = m_TransationDb.ExecuteQueryReturnDataSet(Sql);
                else
                    m_groups = m_MysqlDb.ExecuteQueryReturnDataSet(Sql);
            }
            return m_groups;          
        }




        public void GetCurrentBatchattendanceDetails(int _studid, out int _no_workingdays, out int _no_presentdays, out int _no_absentdays, out int _no_holidays, out double _attendencepersent,int _batchid)
        {
            _no_workingdays = 0;
            _no_presentdays = 0;
            _no_absentdays = 0;
            _no_holidays = 0;
            _attendencepersent = 0;
            int _ClsId = 0;
            try
            {
                Sql = "select tblstudentclassmap.ClassId from tblstudentclassmap where tblstudentclassmap.BatchId=" + _batchid + " and tblstudentclassmap.StudentId=" + _studid;
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
                if (m_MyReader.HasRows)
                {
                    _ClsId = int.Parse(m_MyReader.GetValue(0).ToString());
                    _no_workingdays = GetNumberOfworkingDayForClass(_ClsId);
                    _no_absentdays = GetNumberOfAbsentDayForTheSutdent(_studid, _ClsId);
                    _no_presentdays = _no_workingdays - _no_absentdays;
                    _attendencepersent = (double)_no_presentdays * 100 ;
                    if (_attendencepersent!=0)
                    _attendencepersent = _attendencepersent / (double)_no_workingdays;
                    _no_holidays = GetNumberOfHolidaysForClass(_ClsId);
                }

            }
            catch
            {
                _no_workingdays = 0;
                _no_presentdays = 0;
                _no_absentdays = 0;
                _attendencepersent = 0;
                _no_holidays = 0;
            }
           
        }
        public void GetCurrentBatchattendanceDetails_Staff(int _staffid, out int _no_workingdays, out int _no_presentdays, out int _no_absentdays, out int _no_holidays, out double _attendencepersent, int _batchid)
        {
            _no_workingdays = 0;
            _no_presentdays = 0;
            _no_absentdays = 0;
            _no_holidays = 0;
            _attendencepersent = 0;        
            try
            {
                    _no_workingdays = GetNumberOfworkingDayForStaff();
                    _no_absentdays = GetNumberOfAbsentDayForTheStaff(_staffid);
                    _no_presentdays = _no_workingdays - _no_absentdays;
                    _attendencepersent = (double)_no_presentdays * 100;
                    if (_attendencepersent != 0)
                        _attendencepersent = _attendencepersent / (double)_no_workingdays;
                    _no_holidays = GetNumberOfHolidaysForStaff(_staffid);
            }
            catch
            {
                _no_workingdays = 0;
                _no_presentdays = 0;
                _no_absentdays = 0;
                _attendencepersent = 0;
                _no_holidays = 0;
            }
        }

        private int GetNumberOfHolidaysForStaff(int _staffid)
        {
            int _TotalHolidaysDay = 0;
            Sql = "select count(distinct tblmasterdate.Id) from tblmasterdate inner join tblholiday on tblmasterdate.Id = tblholiday.dateId where tblholiday.Type='all' and tblholiday.Class_Id=0";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalHolidaysDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalHolidaysDay;
        }

        private int GetNumberOfHolidaysForClass(int _ClsId)
        {
            int _TotalHolidaysDay = 0;
            //Sql = "select count(tblholiday.Id) from tblholiday where tblholiday.Id in (select  tblholiday.Id from tblholiday WHERE tblholiday.`Type`='class' AND tblholiday.Class_Id=" + _ClsId + ") OR  tblholiday.Id in (select  tblholiday.Id from tblholiday inner join tblclass on tblclass.ParentGroupID = tblholiday.GroupId AND tblholiday.`Type`='all'  WHERE  tblclass.Id=" + _ClsId + ")";
            Sql = "select count(tblholiday.Id) from tblholiday where tblholiday.Id in (select  tblholiday.Id from tblholiday  WHERE tblholiday.`Type`='class' AND tblholiday.Class_Id=" + _ClsId + " AND tblholiday.Class_Id<>0) OR  tblholiday.Id in (select  tblholiday.Id from tblholiday WHERE (( tblholiday.`Type`='all' OR tblholiday.`Type`='class') AND tblholiday.Class_Id=0 ))";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalHolidaysDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalHolidaysDay;
        }
        private int GetNumberOfAbsentDayForTheStaff(int _staffid)
        {
            int _TotalabsentDay = 0;
            // Sql = "select count(tblstudentattendance.Id) from tblstudentattendance inner join  tbldate on tblstudentattendance.DayId= tbldate.Id  WHERE tbldate.`Status`='class' AND tbldate.classId=" + _ClsId + " And tblstudentattendance.StudentId=" + _staffid;
            Sql = "select  count(tblstaffattendance.Id) from tblstaffattendance inner join  tbldate on tblstaffattendance.DayId= tbldate.Id   WHERE tbldate.`Status`='staff'  and  tblstaffattendance.StaffId=" + _staffid;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalabsentDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalabsentDay;
        }
        private int GetNumberOfAbsentDayForTheSutdent(int _studid, int _ClsId)
        {
            int _TotalabsentDay = 0;
            Sql = "select count(tblstudentattendance.Id) from tblstudentattendance inner join  tbldate on tblstudentattendance.DayId= tbldate.Id  WHERE tbldate.`Status`='class' AND tbldate.classId=" + _ClsId + " And tblstudentattendance.StudentId=" + _studid;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalabsentDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalabsentDay;
        }

        private int GetNumberOfworkingDayForClass(int _ClsId)
        {
            int _TotalworkingDay = 0;
            Sql = "select count(tbldate.Id) from tbldate   WHERE tbldate.`Status`='class' AND tbldate.classId=" + _ClsId;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalworkingDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalworkingDay;
           
        }
        private int GetNumberOfworkingDayForStaff()
        {
            int _TotalworkingDay = 0;
            Sql = "select count(tbldate.Id) from tbldate WHERE tbldate.`Status`='staff' AND tbldate.classId=0";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalworkingDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalworkingDay;

        }

        public DataSet defaultholidays()
        {            
            if (m_HolidayDataset == null)
            {                
                Sql = "select tblholidayconfig.Id, tblholidayconfig.Day from tblholidayconfig where tblholidayconfig.Status=1";

                if (m_TransationDb!=null)
                  m_HolidayDataset = m_TransationDb.ExecuteQueryReturnDataSet(Sql);
                else
                    m_HolidayDataset = m_MysqlDb.ExecuteQueryReturnDataSet(Sql);

            }
            return m_HolidayDataset;
        }

        public bool presentInDefault_HolidayList(string day)
        {
            bool present=false;
            if (m_HolidayDataset == null)

                m_HolidayDataset = defaultholidays();

            if (m_HolidayDataset.Tables != null && m_HolidayDataset.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in m_HolidayDataset.Tables[0].Rows)
                {
                  
                    if (day.ToLower() == (dr[1].ToString().ToLower()))
                    {
                        present = true;
                        break;
                    }
                }
            }            
            return present;
        }

        public bool presentInDefault_HolidayListForStaff(string day)
        {
            bool present = false;
            if (m_HolidayDataset_Staffs == null)

                m_HolidayDataset_Staffs = defaultholidaysForStaff();

            if (m_HolidayDataset_Staffs.Tables != null && m_HolidayDataset_Staffs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in m_HolidayDataset_Staffs.Tables[0].Rows)
                {
                    if (day.ToLower() == (dr[1].ToString().ToLower()))
                    {
                        present = true;
                        break;
                    }
                }
            }
            return present;
        }

        private DataSet defaultholidaysForStaff()
        {
            if (m_HolidayDataset_Staffs == null)
            {
                Sql = "select tblholidayconfig.Id, tblholidayconfig.Day from tblholidayconfig where tblholidayconfig.StaffStatus=1";

                if (m_TransationDb != null)
                    m_HolidayDataset_Staffs = m_TransationDb.ExecuteQueryReturnDataSet(Sql);
                else
                    m_HolidayDataset_Staffs = m_MysqlDb.ExecuteQueryReturnDataSet(Sql);

            }
            return m_HolidayDataset_Staffs;
        }
        //following 2 methods are renamed and used
        public void UpadetheStundentfAtceTbl(int _StutendId, int _dateId)
        {
            try
            {
                CLogging logger = CLogging.GetLogObject();
                logger.LogToFile("UpadetheStundentfAtceTbl", " Querry To Insert into the student attendance table", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                Sql = "insert into tblstudentattendance(StudentId,DayId)VALUES(" + _StutendId + "," + _dateId + ")";
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
                logger.LogToFile("UpadetheStundentfAtceTbl", " Inserted into the staff attance table", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            }
            catch (Exception e)
            {
                CLogging logger = CLogging.GetLogObject();
                logger.LogToFile("UpadetheStundentfAtceTbl", " Error during the inertion into the student attdendance tbl " + e.Message + "", 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
            }
        }
        public bool FindIfTheStundtIsPrntInthetblStuAttandence(int _StutendId, int _dateId)
        {
            CLogging logger = CLogging.GetLogObject();
            bool _present = false;
            Sql = " select tblstudentattendance.DayId from tblstudentattendance  where tblstudentattendance.DayId=" + _dateId + " and tblstudentattendance.StudentId=" + _StutendId;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            logger.LogToFile("FindIfTheStundtIsPrntInthetblStuAttandence", " to check if the staff in present in the attendance table", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
            if (m_MyReader.HasRows)
            {

                _present = true;
            }
            return _present;
        }
     


        public void GetMonthattendanceDetails(int _studid, int _month, out int _no_workingdays, out int _no_presentdays, out int _no_absentdays, out int _no_holidays, out double _attendencepersent, int _batchid)
        {
            _no_workingdays = 0;
            _no_presentdays = 0;
            _no_absentdays = 0;
            _no_holidays = 0;
            _attendencepersent = 0;
            int _ClsId = 0;
            try
            {
                Sql = "select tblstudentclassmap.ClassId from tblstudentclassmap where tblstudentclassmap.BatchId=" + _batchid + " and tblstudentclassmap.StudentId=" + _studid;
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
                if (m_MyReader.HasRows)
                {
                    _ClsId = int.Parse(m_MyReader.GetValue(0).ToString());
                    _no_workingdays = GetNumberOfworkingDayForClassForMonth(_ClsId, _month);
                    _no_absentdays = GetNumberOfAbsentDayForTheSutdentForMonth(_studid, _ClsId, _month);
                    _no_presentdays = _no_workingdays - _no_absentdays;
                    _attendencepersent = (double)_no_presentdays * 100;
                    if (_attendencepersent != 0)
                        _attendencepersent = _attendencepersent / (double)_no_workingdays;
                    _no_holidays = GetNumberOfHolidaysForClassForMonth(_ClsId, _month);
                }

            }
            catch
            {
                _no_workingdays = 0;
                _no_presentdays = 0;
                _no_absentdays = 0;
                _attendencepersent = 0;
                _no_holidays = 0;
            }
        }
          public void GetMonthattendanceDetails_Staff(int _staffid, int _month, out int _no_workingdays, out int _no_presentdays, out int _no_absentdays, out int _no_holidays, out double _attendencepersent)
        {
            _no_workingdays = 0;
            _no_presentdays = 0;
            _no_absentdays = 0;
            _no_holidays = 0;
            _attendencepersent = 0;
           
            try
            {
               
                   
                    _no_workingdays = GetNumberOfworkingDayForStaffForMonth(_month);
                    _no_absentdays = GetNumberOfAbsentDayForTheStaffForMonth(_staffid, _month);
                    _no_presentdays = _no_workingdays - _no_absentdays;
                    _attendencepersent = (double)_no_presentdays * 100;
                    if (_attendencepersent != 0)
                        _attendencepersent = _attendencepersent / (double)_no_workingdays;
                    _no_holidays = GetNumberOfHolidaysForStaffForMonth( _month);
                
            }
            catch
            {
                _no_workingdays = 0;
                _no_presentdays = 0;
                _no_absentdays = 0;
                _attendencepersent = 0;
                _no_holidays = 0;
            }
        }
       
        private int GetNumberOfHolidaysForClassForMonth(int _ClsId, int _month)
        {
            int _TotalHolidaysDay = 0;
            Sql = "select count(tblholiday.Id) from tblholiday inner join tblmasterdate on tblmasterdate.Id= tblholiday.dateId  where MONTH( tblmasterdate.`date`)=" + _month + "  AND ( tblholiday.Id in (select  tblholiday.Id from tblholiday WHERE tblholiday.`Type`='class' AND tblholiday.Class_Id=" + _ClsId + ") OR  tblholiday.Id in (select  tblholiday.Id from tblholiday WHERE tblholiday.`Type`='all'))";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalHolidaysDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalHolidaysDay;
        }
        private int GetNumberOfHolidaysForStaffForMonth(int _month)
        {
            int _TotalHolidaysDay = 0;
            Sql = "select count(distinct tblmasterdate.Id) from tblmasterdate inner join tblholiday on tblmasterdate.Id = tblholiday.dateId  where MONTH( tblmasterdate.`date`)=" + _month + "  AND tblholiday.`Type`='all' AND tblholiday.Class_Id=0";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalHolidaysDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalHolidaysDay;
        }
        private int GetNumberOfAbsentDayForTheSutdentForMonth(int _studid, int _ClsId, int _month)
        {
            int _TotalabsentDay = 0;
            Sql = "select count(tblstudentattendance.Id) from tblstudentattendance inner join  tbldate on tblstudentattendance.DayId= tbldate.Id inner join tblmasterdate on tblmasterdate.Id= tbldate.DateId WHERE tbldate.`Status`='class' AND tbldate.classId="+_ClsId+" And tblstudentattendance.StudentId="+_studid+" AND  MONTH( tblmasterdate.`date`) =" + _month;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalabsentDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalabsentDay;
        }
        private int GetNumberOfAbsentDayForTheStaffForMonth(int _staffid, int _monthid)
        {
            int _TotalabsentDay = 0;
            Sql = "select count(tblstaffattendance.Id) from tblstaffattendance inner join  tbldate on tblstaffattendance.DayId= tbldate.Id inner join tblmasterdate on tblmasterdate.Id= tbldate.DateId WHERE tbldate.`Status`='staff' AND tbldate.classId=0 And tblstaffattendance.StaffId=" + _staffid + " AND  MONTH( tblmasterdate.`date`) =" + _monthid;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalabsentDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalabsentDay;
        }

        private int GetNumberOfworkingDayForStaffForMonth(int _monthid)
        {
            int _TotalworkingDay = 0;
            Sql = "select count(tbldate.Id) from tbldate inner join tblmasterdate on tblmasterdate.Id= tbldate.DateId  WHERE tbldate.`Status`='staff' AND tbldate.classId=0 AND MONTH( tblmasterdate.`date`) =" + _monthid;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalworkingDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalworkingDay;
        }
        private int GetNumberOfworkingDayForClassForMonth(int _ClsId, int _month)
        {
            int _TotalworkingDay = 0;
            Sql = "select count(tbldate.Id) from tbldate inner join tblmasterdate on tblmasterdate.Id= tbldate.DateId  WHERE tbldate.`Status`='class' AND tbldate.classId=" + _ClsId + " AND MONTH( tblmasterdate.`date`) =" + _month;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalworkingDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalworkingDay;
        }

    
        public int GetAttendanceAvgForStud(int _studid, int _classid, int _monthid,out int _no_workingdays,out int _no_presentdays,out int _no_absentdays)
        {
            _no_workingdays=0;
            _no_absentdays = 0; 
            _no_presentdays = 0;
            int _attendencepersent = 0;
            _no_workingdays = GetNumberOfworkingDayForClassForMonth(_classid, _monthid);
            _no_absentdays = GetNumberOfAbsentDayForTheSutdentForMonth(_studid, _classid, _monthid);
            _no_presentdays = _no_workingdays - _no_absentdays;
            _attendencepersent = _no_presentdays * 100;
            if (_attendencepersent != 0)
                _attendencepersent = _attendencepersent /_no_workingdays;
            return _attendencepersent;
        }

        public int GetAttendanceAvgForStaff(int _staffid, int _monthid, out int _no_workingdays, out int _no_presentdays, out int _no_absentdays)
        {
            _no_workingdays = 0;
            _no_absentdays = 0;
            _no_presentdays = 0;
            int _attendencepersent = 0;
            _no_workingdays = GetNumberOfworkingDayForStaffForMonth(_monthid);
            _no_absentdays = GetNumberOfAbsentDayForTheStaffForMonth(_staffid, _monthid);
            _no_presentdays = _no_workingdays - _no_absentdays;
            _attendencepersent = _no_presentdays * 100;
            if (_attendencepersent != 0)
                _attendencepersent = _attendencepersent / _no_workingdays;
            return _attendencepersent;
        }

       

        public void UpadetheStfAtceTbl(int _StaffId, int _dateId)
        {
            Sql = "insert into tblstaffattendance(StaffId,DayId)VALUES(" + _StaffId + "," + _dateId + ")";
            m_MyReader = m_TransationDb.ExecuteQuery(Sql);
           
        }



        public bool FindIfTheStaffIsPresentInthetblstaffAttandence(int _StaffId, int _dateId)
        {

            bool _present = false;

            Sql = " select tblstaffattendance.StaffId from tblstaffattendance where tblstaffattendance.StaffId=" + _StaffId + " and tblstaffattendance.DayId=" + _dateId;
            m_MyReader = m_TransationDb.ExecuteQuery(Sql);

            if (m_MyReader.HasRows)
            {

                _present = true;
            }
            return _present;
        }
        
        public void SaveDate_In_HolidayTbl(DateTime _currentday, string description, string _type, int _ClassId)
        {
             
        string sql = "";
        int _GetId = getDateIdFrmMastTbl(_currentday);
        sql = "insert into tblholiday (`dateId`,`Desc`,`Type`,Class_Id) values(" + _GetId + ",'" + description + "','" + _type + "'," + _ClassId + ")";
        m_TransationDb.TransExecuteQuery(sql);

   
        }

        public int get_NoOf_WorkingDaysForThePeriod(int _classid, string _sdate, string _enddate)
        {
            int _WorkingDays = 0;
            string sql = "select  count(DISTINCT(tbldate.Id)) from tbldate inner join tblmasterdate on  tblmasterdate.Id= tbldate.DateId  WHERE  date(tblmasterdate.date) BETWEEN '"+_sdate+"' and '"+_enddate+"' and  tbldate.`Status`='class' and tbldate.classId=" + _classid;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _WorkingDays = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            return _WorkingDays;
        }

        public int get_NoOf_AbsentDayForTheperiod(int _studentid, int _classid, string _sdate, string _enddate)
        {
            int __AbsentDays = 0;
            string sql = "select count(DISTINCT(tblstudentattendance.Id)) from tblstudentattendance inner join tbldate on tblstudentattendance.DayId= tbldate.Id  inner join tblmasterdate on  tblmasterdate.Id= tbldate.DateId  WHERE  date(tblmasterdate.date) BETWEEN '" + _sdate + "' and '" + _enddate + "' and  tbldate.`Status`='class' and tbldate.classId=" + _classid + " and tblstudentattendance.StudentId=" + _studentid;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                __AbsentDays = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            return __AbsentDays;
           
        }
        public DateTime GetFirstedDayFromMonth(int selected_month)
        {
            DateTime dtFrom = new DateTime(DateTime.Now.Year, selected_month, 1);

            dtFrom = dtFrom.AddDays(-(dtFrom.Day - 1));

            return dtFrom;
        }

        public DateTime GetLastDateFromMonth(int selected_month)
        {

            DateTime dtTo = new DateTime(DateTime.Now.Year, selected_month, 1);
            dtTo = dtTo.AddMonths(1);

            dtTo = dtTo.AddDays(-(dtTo.Day));
            return dtTo;

        }

        public bool CheckSelectedMonthIsCurrentMonthorNot(int selected_month)
        {
            bool _currentMonth = false;
            if (selected_month == System.DateTime.Now.Month)
            {
                _currentMonth = true;
            }
            return _currentMonth;
        }

        public bool ReadAllAttendenceAndReturndataset(int _studId, int _classId, int _currentBatchId, string _module, string _startDate, string _endDate, out DataSet _attendenceReport, out string _msg)
        {
            _attendenceReport = new DataSet();
            _msg = "";

            bool _valid = false;

            OdbcDataReader StudList = null;             // to store name and id of all student
            OdbcDataReader _StudAttendence = null;      //used to save attendence of a student

            ArrayList arr = new ArrayList();            // used to save all dates in a specified format-"dd/mm/yyy"
            ArrayList _dateArr = new ArrayList();       //used to save all dates in date format
            ArrayList dateId = new ArrayList();         //used to save all date Id 
         
            DataTable dt;
            DataRow dr;

            try
            {
                /*Console.WriteLine(date1.ToString("d", 
                  CultureInfo.CreateSpecificCulture("en-NZ")));
                // Displays 10/04/2008  */
                General _GenObj = new General(m_MysqlDb);

                DateTime _stDate = _GenObj.GetDateFromText(_startDate);
                DateTime _eDate = _GenObj.GetDateFromText(_endDate);
                DateTime tmpDate = _stDate;

                _attendenceReport.Tables.Add(new DataTable("Attedance"));
                dt = _attendenceReport.Tables["Attedance"];
                dt.Columns.Add("Student Name");

                // Enter all dates
                do
                {
                    arr.Add(tmpDate.ToString("d" , CultureInfo.CreateSpecificCulture("en-NZ")));
                    _dateArr.Add(tmpDate.ToString());
                    tmpDate = tmpDate.AddDays(1);

                } while (tmpDate <= _eDate);

                int arraaCount = arr.Count;
                
                for (int i = 0; i <arraaCount; i++)
                {
                    dt.Columns.Add(arr[i].ToString());
                }

                for (int k = 0; k < arraaCount; k++)
                {
                    // save date id. if Id is not exist for a date then it store -1
                    dateId.Add(IdExistInDb(DateTime.Parse(_dateArr[k].ToString())));
                }

                if (_module == "Stud")
                {
                    string sql = "Select tblstudent.Id, tblstudent.StudentName from tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudent.Id where tblstudentclassmap.BatchId= " + _currentBatchId + " and tblstudentclassmap.ClassId=" + _classId + " and tblstudent.Status=1 and tblstudent.Id=" + _studId + "   ";
                    StudList = m_MysqlDb.ExecuteQuery(sql);
                }
                else
                {
                    string sql = "Select tblstudent.Id, tblstudent.StudentName from tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudent.Id where tblstudentclassmap.BatchId= " + _currentBatchId + " and tblstudentclassmap.ClassId=" + _classId + " and tblstudent.Status=1 order by  tblstudent.StudentName ASC ";
                    StudList = m_MysqlDb.ExecuteQuery(sql);
                }

                dr = _attendenceReport.Tables["Attedance"].NewRow();
                dr["Student Name"] = " ";

                for (int m = 0; m< arraaCount; m++)
                {
                    dr[arr[m].ToString()] = " ";
                }

                _attendenceReport.Tables["Attedance"].Rows.Add(dr);

                // studnet is exist or not
                if (StudList.HasRows)
                {
                    //Read student Name
                    while (StudList.Read())
                    {
                        dr = _attendenceReport.Tables["Attedance"].NewRow();
                        dr["Student Name"] = StudList.GetValue(1).ToString();
                        int _dateCount = dateId.Count;
                        for (int j = 0; j < _dateCount; j++)
                        {
                            _msg="";
                            // Date is holiday or not
                            if (!dateisHoliday(_classId, int.Parse(dateId[j].ToString())))
                            {
                                //Day is sunday or not 
                                if (findDaysOfWeek(DateTime.Parse(_dateArr[j].ToString()),  out _msg))
                                {
                                    //Attendance is entered or not in selected day
                                    if (_attendanceReportedOnSelectedDate(_classId, int.Parse(dateId[j].ToString())))
                                    {
                                        // student is present or not .If valus of '_StudAttendence' is null sturndet is present 
                                        Sql = "select tblstudentattendance.StudentId,tblstudentattendance.`Status` from tblstudentattendance inner join tbldate on  tbldate.Id = tblstudentattendance.DayId where tbldate.DateId=" + int.Parse(dateId[j].ToString()) + " and tbldate.Status='class' and tbldate.classId=" + _classId + " and tblstudentattendance.StudentId=" + int.Parse(StudList.GetValue(0).ToString()) + "";
                                        _StudAttendence = m_MysqlDb.ExecuteQuery(Sql);

                                        if (_StudAttendence.HasRows)
                                        {
                                            if (_StudAttendence.GetValue(1).ToString()=="1")
                                            dr[arr[j].ToString()] = "Absent";
                                            else
                                                dr[arr[j].ToString()] = "Absent";
                                             
                                        }
                                        else
                                        {
                                            dr[arr[j].ToString()] = "Present";
                                        }
                                    }
                                    else
                                    {
                                        dr[arr[j].ToString()] = "-";
                                    }
                                    //End if loop "Attendance is entered or not in selected day"

                                }
                                else
                                {
                                    dr[arr[j].ToString()] = _msg;
                                    _msg="";
                                }
                                //End if loop "Day is sunday or not"
                              }
                            
                            else
                            {
                                dr[arr[j].ToString()] = "Holiday";
                            }
                            //End if loop " Date is holiday or not"
                        }
                        _attendenceReport.Tables["Attedance"].Rows.Add(dr);


                    }
                    //End while loop "Read student Name"
                    _valid = true;
                }
                else
                {
                    _msg = " students not  Found";
                    _attendenceReport = null;
                    _valid = false;
                }
                //End if loop "studnet is exist or not"
            }
            catch
            {
                _valid = false;
                _msg = "Report Creation Fail";
            }
            return _valid;

        }

        private bool findDaysOfWeek(DateTime dateTime, out string _msg)
        {
            _msg="";
            bool day = true;
           DayOfWeek dw=  dateTime.DayOfWeek;
           if (dw == DayOfWeek.Sunday)
           {
               _msg = "Sunday";
               day = false;
           }
           else
           {
               day = true;
           }
           return day;
        }

       
       

        private int IdExistInDb(DateTime date)
        {
            // if date is exist in tblmasterdate then it returns date id otherwise return -1 
            int Id = -1;
            OdbcDataReader _DateId = null;
            string sql = "Select tblmasterdate.Id from tblmasterdate where tblmasterdate.`date`='" + date.ToString("d", CultureInfo.CreateSpecificCulture("zh-CN")) + "'";
            _DateId = m_MysqlDb.ExecuteQuery(sql);
            if (_DateId.HasRows)
            {
                string ds = _DateId.GetValue(0).ToString();

                Id = int.Parse(_DateId.GetValue(0).ToString());
            }

            return Id;
        }

        private bool _attendanceReportedOnSelectedDate(int _classId, int _dateId)
        {
            bool exist = false;
            string sql = "Select tbldate.Id from tbldate where tbldate.DateId=" + _dateId + " and tbldate.classId=" + _classId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                exist = true;
            }
            else
            {
                exist = false;
            }
            return exist;

        }

        private bool dateisHoliday(int _ClassId, int _dateid)
        {
            bool isholiday = false;

            Sql = "select tblholiday.Id from tblholiday where tblholiday.Id in (select  tblholiday.Id from tblholiday WHERE tblholiday.`Type`='class' AND tblholiday.Class_Id=" + _ClassId + " AND tblholiday.dateId=" + _dateid + ") OR  tblholiday.Id in (select  tblholiday.Id from tblholiday  WHERE tblholiday.`Type`='all' AND tblholiday.dateId=" + _dateid + ")";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);

            if (m_MyReader.HasRows)
            {
                isholiday = true;
            }
            return isholiday;

        }





        # region new attendance module

        public string GetStandard_Class(int ClassId)
        {
            string StandardId = "";
            string sql = "SELECT tblclass.Standard FROM tblclass WHERE tblclass.Id=" + ClassId;
            if (m_TransationDb != null)
            {
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            }
            if (m_MyReader.HasRows)
            {
                StandardId = m_MyReader.GetValue(0).ToString();
            }

            return StandardId;
        }

        public bool IsDefaultHoliday(DayOfWeek dayOfWeek)
        {
            bool DefaultHoliday = false;
            int TempStatus = 0;
            string sql = "SELECT tblholidayconfig.`Status` FROM tblholidayconfig WHERE tblholidayconfig.`Day`='" + dayOfWeek.ToString().ToUpperInvariant() + "'";
            if (m_TransationDb != null)
            {
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            }
            if (m_MyReader.HasRows)
            {

                if (int.TryParse(m_MyReader.GetValue(0).ToString(), out TempStatus))
                {
                    if (TempStatus == 1)
                    {
                        DefaultHoliday = true;
                    }
                }

            }
            return DefaultHoliday;
        }


        public bool IsDateHoliday(int ClassId, DateTime Days)
        {
            bool IsHoilday = false;
            if (IsDefaultHoliday(Days.Date.DayOfWeek))
            {
                IsHoilday = true;
            }
            else
            {
                int TempCount = 0;
                string sql = "SELECT COUNT(tblholiday.Id) FROM tblholiday INNER JOIN tblmasterdate ON tblmasterdate.Id=tblholiday.dateId  WHERE tblmasterdate.`date`='" + Days.Date.ToString("s") + "' AND (tblholiday.`Type`='all' or (tblholiday.Class_Id=" + ClassId + " AND tblholiday.`Type`='class'))";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {

                    if (int.TryParse(m_MyReader.GetValue(0).ToString(), out TempCount))
                    {
                        if (TempCount > 0)
                        {
                            IsHoilday = true;
                        }
                    }

                }

            }
            return IsHoilday;
        }

        public bool AttendanceTables_Exits(string StdId, int YearId)
        {
            bool valid = false;
            string _tablename = "";
            string End_Region = "std" + StdId + "yr" + YearId;
            Sql = "show tables like 'tblattdcls_" + End_Region + "'";
            if (m_TransationDb != null)
            {
                m_MyReader = m_TransationDb.ExecuteQuery(Sql);
            }
            else
            {
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            }
            if (m_MyReader.HasRows)
            {
                _tablename = m_MyReader.GetValue(0).ToString();
                if (_tablename != "")
                {
                    valid = true;
                }
            }
            if (valid)
            {
                _tablename = "";
                valid = false;
                Sql = "show tables like 'tblattdstud_" + End_Region + "'";
                if (m_TransationDb != null)
                {
                    m_MyReader = m_TransationDb.ExecuteQuery(Sql);
                }
                else
                {
                    m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
                }
                if (m_MyReader.HasRows)
                {
                    _tablename = m_MyReader.GetValue(0).ToString();
                    if (_tablename != "")
                    {
                        valid = true;
                    }
                }
            }
            return valid;
        }



        public void SaveClassAttendanceDetails(int ClassId, DateTime Date, int CreatedUserId, string StandardId, int YearId, string Status, out int ClassAttendanceId)
        {
            ClassAttendanceId = 0;
            Sql = "INSERT INTO tblattdcls_std" + StandardId + "yr" + YearId + " (ClassId,Date,Status,LastModifiedDateTime,LastModifiedUserId,CreatedDateTime,CreatedUserId) VALUES(" + ClassId + ",'" + Date.Date.ToString("s") + "'," + Status + ",'" + DateTime.Now.ToString("s") + "'," + CreatedUserId + ",'" + DateTime.Now.ToString("s") + "'," + CreatedUserId + ")";
            m_MyReader = m_TransationDb.ExecuteQuery(Sql);
            ClassAttendanceId = GetClassAttendanceId(ClassId, Date, StandardId, YearId);
            if (ClassAttendanceId > 0)
            {
                Sql = "SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudent.Status=1 AND tblstudentclassmap.ClassId=" + ClassId + " AND tblstudentclassmap.BatchId=" + YearId + " AND tblstudentclassmap.Standard=" + StandardId;
                m_MyReader = m_TransationDb.ExecuteQuery(Sql);
                if (m_MyReader.HasRows)
                {
                    while (m_MyReader.Read())
                    {
                        SaveStudentAttendanceDetails(ClassAttendanceId, m_MyReader.GetValue(0).ToString(), Status, ClassId, StandardId, YearId);
                    }
                }
            }
        }
        private int GetClassAttendanceId(int ClassId, DateTime Date, string StandardId, int YearId)
        {
            int _Id = 0;
            Sql = "SELECT Id FROM tblattdcls_std" + StandardId + "yr" + YearId + " WHERE ClassId=" + ClassId + " AND Date='" + Date.ToString("s") + "'";
            m_MyReader = m_TransationDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out _Id);
            }
            return _Id;
        }
        public void SaveStudentAttendanceDetails(int ClassAttendanceId, string StudentId, string AttendanceStatus, int ClassId, string StandardId, int YearId)
        {
            Sql = "INSERT INTO tblattdstud_std" + StandardId + "yr" + YearId + " (ClassAttendanceId,StudentId,PresentStatus,ApproveStatus) VALUES(" + ClassAttendanceId + "," + StudentId + "," + AttendanceStatus + ",0)";
            m_TransationDb.ExecuteQuery(Sql);
        }


        public void UpdateStudentAttendanceDetails(int ClassAttendanceId, string StudentId, int AttendanceStatus, int ClassId, string StandardId, int YearId)
        {
            Sql = "UPDATE tblattdstud_std" + StandardId + "yr" + YearId + " SET PresentStatus=" + AttendanceStatus + " WHERE classAttendanceId=" + ClassAttendanceId + " AND StudentId=" + StudentId;
            m_MyReader = m_TransationDb.ExecuteQuery(Sql);
        }

        private void UpdateStudentAttendanceTimeDetails(int ClassAttendanceId, string StudentId, int ClassId, string StandardId, int YearId, string RFReaderType,TimeSpan _time,int GroupId)
        {
            string FieldName = "";
            string sqlsub = "";
            TimeSpan _LateIntime = TimeSpan.Parse(GetStudentAttend_ConfigValue("Late InTime", GroupId));
            if (RFReaderType == "INTIME")
            {
                FieldName = "InTime";
                
                if (_time > _LateIntime)
                {
                    TimeSpan LateValue = _time - _LateIntime;
                    sqlsub = ",IsLate=1,LateValue='" + LateValue.ToString() + "'";
                }
            }

            if (RFReaderType == "OUTTIME")
            {
                FieldName = "OutTime";

                if (!IsStudentInTimeMarked(ClassAttendanceId, StudentId, StandardId, YearId))
                {
                    if (_time > _LateIntime)
                    {
                        TimeSpan LateValue = _time - _LateIntime;
                        sqlsub = ",IsLate=1,LateValue='" + LateValue.ToString() + "',InTime='" + _time.ToString() + "'";
                    }
                }

            }


            if (FieldName != "")
            {

                Sql = "UPDATE tblattdstud_std" + StandardId + "yr" + YearId + " SET " + FieldName + "='" + _time.ToString() + "'" + sqlsub + " WHERE classAttendanceId=" + ClassAttendanceId + " AND StudentId=" + StudentId;
                m_MyReader = m_TransationDb.ExecuteQuery(Sql);
            }
        }



        private bool IsStudentInTimeMarked(int ClassAttendanceId, string StudentId, string StandardId, int YearId)
        {
            bool _valid = false;
            string sql = "SELECT InTime FROM tblattdstud_std" + StandardId + "yr" + YearId + "  WHERE classAttendanceId=" + ClassAttendanceId + " AND StudentId=" + StudentId;
            m_MyReader = m_TransationDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                TimeSpan _test = new TimeSpan();
                if (TimeSpan.TryParse(m_MyReader.GetValue(0).ToString(), out _test))
                {
                    _valid = true;
                }
            }
            return _valid;
        }




        public bool IsAttendanceAlreadyMarked(int ClassId, DateTime M_StartTime, string StandardId, int YearId, out string Status, out int ClassAttendanceId)
        {
            Status = ""; 
            bool valid = false;
            ClassAttendanceId = 0;
            Sql = "SELECT Id,Status FROM tblattdcls_std" + StandardId + "yr" + YearId + " WHERE ClassId=" + ClassId + " AND Date='" + M_StartTime.Date.ToString("s") + "'";
            if (m_TransationDb != null)
            {
                m_MyReader = m_TransationDb.ExecuteQuery(Sql);
            }
            else
            {
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            }
            if (m_MyReader.HasRows)
            {

                if (int.TryParse(m_MyReader.GetValue(0).ToString(), out ClassAttendanceId))
                {
                    if (ClassAttendanceId > 0)
                    {
                        valid = true;
                    }
                }
                Status = m_MyReader.GetValue(1).ToString();

            }
            return valid;
        }


        public void UpdateClassAttendanceDetails(int ClassId,string ClassAttandanceId, string Status, int UserId, string StandardId, int YearId)
        {
            int Id = 0;
            int.TryParse(ClassAttandanceId, out Id);
            if (Id > 0)
            {

                Sql = "UPDATE tblattdcls_std" + StandardId + "yr" + YearId + " SET Status=Status+" + Status + ",LastModifiedDateTime='" + DateTime.Now.ToString("s") + "',LastModifiedUserId=" + User_Id + " WHERE Id=" + Id;
                m_MyReader = m_TransationDb.ExecuteQuery(Sql);


                Sql = "SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudent.Status=1 AND tblstudentclassmap.ClassId=" + ClassId + " AND tblstudentclassmap.BatchId=" + YearId + " AND tblstudentclassmap.Standard=" + StandardId;
                m_MyReader = m_TransationDb.ExecuteQuery(Sql);
                if (m_MyReader.HasRows)
                {
                    while (m_MyReader.Read())
                    {
                        UpdateStudentAttendanceDetailsAgain(Id, m_MyReader.GetValue(0).ToString(), Status, StandardId, YearId);
                    }
                }
            }
        }

        public void UpdateStudentAttendanceDetailsAgain(int ClassAttendanceId, string StudentId, string Status, string StandardId, int YearId)
        {
            Sql = "UPDATE tblattdstud_std" + StandardId + "yr" + YearId + " SET PresentStatus=PresentStatus+" + Status + " WHERE classAttendanceId=" + ClassAttendanceId + " AND StudentId=" + StudentId;
            m_TransationDb.ExecuteQuery(Sql);
        }

        public int GetAttendanceStatus(DateTime SelectedDate, int ClassId, string StandardId, int YearId)
        {
            int status = 0;
            Sql = "SELECT `Status` FROM  tblattdcls_std" + StandardId + "yr" + YearId + " WHERE ClassId=" + ClassId + " AND `Date`='" + SelectedDate.Date.ToString("s")+ "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out status);
            }

            return status;
        }
        public int GetAttendanceStatus_Student(DateTime SelectedDate, int ClassId, string StandardId, int YearId, string Studentid)
        {
            int status = -1;
            Sql = "SELECT std.PresentStatus FROM  tblattdcls_std" + StandardId + "yr" + YearId + " cls INNER JOIN tblattdstud_std" + StandardId + "yr" + YearId + " std ON std.ClassAttendanceId=cls.Id WHERE cls.ClassId=" + ClassId + " AND cls.`Date`='" + SelectedDate.Date.ToString("s") + "' AND std.StudentId=" + Studentid;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out status);
            }

            return status;
        }

        public bool GetCurrentBatchNewattendanceDetails(int _studid, out int _no_workingdays, out int _no_fulldays, out int _no_absentdays, out int _no_holidays, out int _no_halfdays,out double _attendencepersent, int _batchid)
        {
            bool valid = false;
            _no_workingdays = 0;
            _no_fulldays = 0;
            _no_absentdays = 0;
            _no_holidays = 0;
            _no_halfdays = 0;
            _attendencepersent = 0;
            int _ClsId = 0;
            int _StandardId = 0;
            try
            {

                Sql = "select tblview_studentclassmap.ClassId,tblview_studentclassmap.Standard from tblview_studentclassmap where tblview_studentclassmap.BatchId=" + _batchid + " and tblview_studentclassmap.StudentId=" + _studid;
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
                if (m_MyReader.HasRows)
                {
                     
                    if (int.TryParse(m_MyReader.GetValue(0).ToString(), out _ClsId) && int.TryParse(m_MyReader.GetValue(1).ToString(), out _StandardId))
                    {
                        if (AttendanceTables_Exits(_StandardId.ToString(), _batchid))
                        {
                            _no_workingdays = GetNumberOfworkingDayForClassFromNewAttdance(_studid, _batchid, _StandardId);
                            _no_absentdays = GetNumberOfAbsentDayForTheSutdentFromNewAttdance(_studid, _batchid, _StandardId);
                            _no_fulldays = GetNumberOfFullDayDayForTheSutdentFromNewAttdance(_studid, _batchid, _StandardId);
                            _no_halfdays = GetNumberOfHalfDayDayForTheSutdentFromNewAttdance(_studid,  _batchid, _StandardId);

                            if (_no_halfdays != 0)
                            {
                                _attendencepersent = (double)(_no_fulldays + (double)(_no_halfdays / 2)) * 100;
                            }
                            else
                            {
                                _attendencepersent = (double)(_no_fulldays) * 100;
                            }
                            if (_no_workingdays != 0)
                                _attendencepersent = _attendencepersent / (double)_no_workingdays;
                            
                        }
                    }
                    _no_holidays = GetNumberOfHolidaysForClass(_ClsId);
                    valid = true;
                }

            }
            catch
            {
                _no_workingdays = 0;
                _no_fulldays = 0;
                _no_absentdays = 0;
                _attendencepersent = 0;
                _no_holidays = 0;
                _no_halfdays = 0;
            }
            return valid;
        }

        public int GetNumberOfHalfDayDayForTheSutdentFromNewAttdance(int _studid, int _batchid, int _StandardId)
        {
            int _TotalhalfDay = 0;
            Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + _StandardId + "yr" + _batchid + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + _batchid + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studid + " AND (t2.PresentStatus=1 OR t2.PresentStatus=2)";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalhalfDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalhalfDay;
        }

        public int GetNumberOfFullDayDayForTheSutdentFromNewAttdance(int _studid, int _batchid, int _StandardId)
        {
            int _TotalFullDay = 0;
            Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + _StandardId + "yr" + _batchid + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + _batchid + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studid + " AND t2.PresentStatus=3";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalFullDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalFullDay;
        }

        public int GetNumberOfAbsentDayForTheSutdentFromNewAttdance(int _studid, int _batchid, int _StandardId)
        {
            int _TotalAbsentDay = 0;
            Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + _StandardId + "yr" + _batchid + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + _batchid + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId="+_studid+" AND t2.PresentStatus=0";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalAbsentDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalAbsentDay;
        }

        public int GetNumberOfworkingDayForClassFromNewAttdance(int _studid, int _batchid, int _StandardId)
        {
            int _TotalworkingDay = 0;
            Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + _StandardId + "yr" + _batchid + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + _batchid + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studid;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalworkingDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalworkingDay;
        }
        /*Dominic 
         *  Attendance with date
         */
        public bool GetCurrentBatchNewattendanceDetailsWithDate(int _studid, out int _no_workingdays, out int _no_fulldays, out int _no_absentdays, out int _no_holidays, out int _no_halfdays, out double _attendencepersent, int _batchid, DateTime _StartDate, DateTime _EndDate)
        {
            bool valid = false;
            _no_workingdays = 0;
            _no_fulldays = 0;
            _no_absentdays = 0;
            _no_holidays = 0;
            _no_halfdays = 0;
            _attendencepersent = 0;
            int _ClsId = 0;
            int _StandardId = 0;
            try
            {

                Sql = "select tblstudentclassmap.ClassId,tblstudentclassmap.Standard from tblstudentclassmap where tblstudentclassmap.BatchId=" + _batchid + " and tblstudentclassmap.StudentId=" + _studid;
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
                if (m_MyReader.HasRows)
                {

                    if (int.TryParse(m_MyReader.GetValue(0).ToString(), out _ClsId) && int.TryParse(m_MyReader.GetValue(1).ToString(), out _StandardId))
                    {
                        if (AttendanceTables_Exits(_StandardId.ToString(), _batchid))
                        {
                            _no_workingdays = GetNumberOfworkingDayForClassFromNewAttdanceWithDate(_studid, _batchid, _StandardId , _StartDate,  _EndDate);
                            _no_absentdays = GetNumberOfAbsentDayForTheSutdentFromNewAttdanceWithDate(_studid, _batchid, _StandardId, _StartDate, _EndDate);
                            _no_fulldays = GetNumberOfFullDayDayForTheSutdentFromNewAttdanceWithDate(_studid, _batchid, _StandardId, _StartDate, _EndDate);
                            _no_halfdays = GetNumberOfHalfDayDayForTheSutdentFromNewAttdanceWithDate(_studid, _batchid, _StandardId, _StartDate,  _EndDate);

                            if (_no_halfdays != 0)
                            {
                                _attendencepersent = (double)(_no_fulldays + (double)(_no_halfdays / 2)) * 100;
                            }
                            else
                            {
                                _attendencepersent = (double)(_no_fulldays) * 100;
                            }
                            if (_no_workingdays != 0)
                                _attendencepersent = _attendencepersent / (double)_no_workingdays;

                        }
                    }
                    _no_holidays = GetNumberOfHolidaysForClass(_ClsId);
                    valid = true;
                }

            }
            catch
            {
                _no_workingdays = 0;
                _no_fulldays = 0;
                _no_absentdays = 0;
                _attendencepersent = 0;
                _no_holidays = 0;
                _no_halfdays = 0;
            }
            return valid;
        }

        public int GetNumberOfHalfDayDayForTheSutdentFromNewAttdanceWithDate(int _studid, int _batchid, int _StandardId, DateTime _StartDate, DateTime _EndDate)
        {
            int _TotalhalfDay = 0;
            if (_StartDate == _EndDate)
                Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + _StandardId + "yr" + _batchid + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + _batchid + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studid + " AND (t2.PresentStatus=1 OR t2.PresentStatus=2) AND t1.Date < '" + _StartDate.ToString("s") + "' ";
            else
                Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + _StandardId + "yr" + _batchid + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + _batchid + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studid + " AND (t2.PresentStatus=1 OR t2.PresentStatus=2) AND t1.Date BETWEEN '" + _StartDate.ToString("s") + "' AND '" + _EndDate.ToString("s") + "' ";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalhalfDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalhalfDay;
        }

        public int GetNumberOfFullDayDayForTheSutdentFromNewAttdanceWithDate(int _studid, int _batchid, int _StandardId, DateTime _StartDate, DateTime _EndDate)
        {
            int _TotalFullDay = 0;
            if (_StartDate == _EndDate)
                Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + _StandardId + "yr" + _batchid + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + _batchid + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studid + " AND t2.PresentStatus=3 AND t1.Date < '" + _StartDate.ToString("s") + "' ";
            else
                Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + _StandardId + "yr" + _batchid + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + _batchid + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studid + " AND t2.PresentStatus=3 AND t1.Date BETWEEN '" + _StartDate.ToString("s") + "' AND '" + _EndDate.ToString("s") + "' ";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalFullDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalFullDay;
        }

        public int GetNumberOfAbsentDayForTheSutdentFromNewAttdanceWithDate(int _studid, int _batchid, int _StandardId, DateTime _StartDate, DateTime _EndDate)
        {
            int _TotalAbsentDay = 0;
            if (_StartDate == _EndDate)
                Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + _StandardId + "yr" + _batchid + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + _batchid + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studid + " AND t2.PresentStatus=0 AND t1.Date < '" + _StartDate.ToString("s") + "' ";
            else
                Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + _StandardId + "yr" + _batchid + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + _batchid + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studid + " AND t2.PresentStatus=0 AND t1.Date BETWEEN '" + _StartDate.ToString("s") + "' AND '" + _EndDate.ToString("s") + "' ";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalAbsentDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalAbsentDay;
        }

        public int GetNumberOfworkingDayForClassFromNewAttdanceWithDate(int _studid, int _batchid, int _StandardId, DateTime _StartDate, DateTime _EndDate)
        {
            int _TotalworkingDay = 0;
            if(_StartDate==_EndDate)
                Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + _StandardId + "yr" + _batchid + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + _batchid + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studid + " AND t1.Date < '" + _StartDate.ToString("s") + "' ";
            else
                Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + _StandardId + "yr" + _batchid + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + _batchid + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studid + " AND t1.Date BETWEEN '" + _StartDate.ToString("s") + "' AND '" + _EndDate.ToString("s") + "' ";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalworkingDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalworkingDay;
        }

        
        /*Dominic */
        


        public double GetStudentMonthly_NewAttendanceDatas(int _studid, int _classid, int _standard, int batchid, int _monthid, out int _no_workingdays, out int _no_presentdays, out int _no_absentdays, out int _no_halfdays, out int _no_Holidays)
        {
            _no_workingdays = 0;
            _no_absentdays = 0;
            _no_presentdays = 0;
            _no_halfdays = 0;
            _no_Holidays = 0;
            double _attendencepersent = 0;
            if (AttendanceTables_Exits(_standard.ToString(), batchid))
            {
                _no_workingdays = GetNumberOfworkingDayForClassForMonth_NewAttendance(_studid, _standard, batchid, _monthid);
                _no_absentdays = GetNumberOfAbsentDayForTheSutdentForMonth_NewAttendance(_studid, _standard, batchid, _monthid);
                _no_presentdays = GetNumberOfFullDayForTheSutdentForMonth_NewAttendance(_studid, _standard, batchid, _monthid);
                _no_halfdays = GetNumberOfHalfDayForTheSutdentForMonth_NewAttendance(_studid, _standard, batchid, _monthid);
                if (_no_halfdays != 0)
                {
                    _attendencepersent = (double)(_no_presentdays + (double)(_no_halfdays / 2)) * 100;
                }
                else
                {
                    _attendencepersent = (double)(_no_presentdays) * 100;
                }
                if (_no_workingdays != 0)
                    _attendencepersent = _attendencepersent / _no_workingdays;
            }
            _no_Holidays = GetNumberOfHolidaysForClassForMonth(_classid, _monthid);
            return _attendencepersent;
        }

        private int GetNumberOfHalfDayForTheSutdentForMonth_NewAttendance(int _studid, int _standard, int batchid, int _monthid)
        {
            int _TotalHalfDay = 0;
            Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + _standard + "yr" + batchid + " t1 INNER JOIN tblattdstud_std" + _standard + "yr" + batchid + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studid + " AND (t2.PresentStatus=1 OR t2.PresentStatus=2) AND MONTH(t1.`Date`)=" + _monthid;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalHalfDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalHalfDay;
        }

        private int GetNumberOfFullDayForTheSutdentForMonth_NewAttendance(int _studid, int _standard, int batchid, int _monthid)
        {
            int _TotalFullDay = 0;
            Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + _standard + "yr" + batchid + " t1 INNER JOIN tblattdstud_std" + _standard + "yr" + batchid + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studid + " AND t2.PresentStatus=3 AND MONTH(t1.`Date`)=" + _monthid;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalFullDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalFullDay;
        }

        private int GetNumberOfAbsentDayForTheSutdentForMonth_NewAttendance(int _studid, int _standard, int batchid, int _monthid)
        {
            int _TotalAbsentDay = 0;
            Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + _standard + "yr" + batchid + " t1 INNER JOIN tblattdstud_std" + _standard + "yr" + batchid + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studid + " AND t2.PresentStatus=0 AND MONTH(t1.`Date`)=" + _monthid;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalAbsentDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalAbsentDay;
        }

        private int GetNumberOfworkingDayForClassForMonth_NewAttendance(int _studid, int _standard, int batchid, int _monthid)
        {
            int _TotalworkingDay = 0;
            Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + _standard + "yr" + batchid + " t1 INNER JOIN tblattdstud_std" + _standard + "yr" + batchid + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studid + " AND MONTH(t1.`Date`)=" + _monthid;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalworkingDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalworkingDay;
        }



        public bool StudentAttendanceReport(int standard,  int _studId, int _classId, int BacthId, string _startDate, string _endDate, out string _msg, out string[] Dates, out string[] AttData)
        {
            Dates = new string[40];
            AttData = new string[40];
            _msg = "";
            bool _valid = false;
            int Status = 0;
            ArrayList dateId = new ArrayList();         //used to save all date Id 


            try
            {
                /*Console.WriteLine(date1.ToString("d", 
                  CultureInfo.CreateSpecificCulture("en-NZ")));
                // Displays 10/04/2008  */
                General _GenObj = new General(m_MysqlDb);

                DateTime _stDate = _GenObj.GetDateFromText(_startDate);
                DateTime _eDate = _GenObj.GetDateFromText(_endDate);
                DateTime tmpDate = _stDate;

                int j = 0;
                do
                {
                    Dates[j]=tmpDate.ToString("d", CultureInfo.CreateSpecificCulture("en-NZ"));
                    dateId.Add(IdExistInDb(_GenObj.GetDateFromText(Dates[j].ToString())));
                    Status = 0;
                    _msg = "";
                    // Date is holiday or not
                    if (!dateisHoliday(_classId, int.Parse(dateId[j].ToString())) && !IsDefaultHoliday(_GenObj.GetDateFromText(Dates[j].ToString()).Date.DayOfWeek))
                    {

                        if (AttendanceTables_Exits(standard.ToString(), BacthId))
                        {
                            if (IsNew_attendanceReportedOnSelectedDate(_studId, standard, BacthId, _GenObj.GetDateFromText(Dates[j].ToString()), out Status))
                            {

                                if (Status == 3)
                                {
                                    AttData[j] = "Full day";
                                }
                                else if (Status == 2)
                                {
                                    AttData[j] = "Afternoon";
                                }
                                else if (Status == 1)
                                {
                                    AttData[j] = "Forenoon";
                                }
                                else
                                {
                                    AttData[j] = "Absent";
                                }


                            }
                            else
                            {
                                AttData[j] = "Not marked";
                            }
                            //End if loop "Attendance is entered or not in selected day"

                        }
                        else
                        {
                            AttData[j] = "Not marked";
                        }
                    }

                    else
                    {
                        AttData[j] = "Holiday";
                    }
                    //End if loop " Date is holiday or not"
                    tmpDate = tmpDate.AddDays(1);
                    j++;
                 } while (tmpDate <= _eDate);
                _valid = true;

                //End if loop "studnet is exist or not"
            }
            catch
            {
                _valid = false;
                _msg = "Report Creation Failed";
            }
            return _valid;

        }

        public bool IsNew_attendanceReportedOnSelectedDate(int _studId, int standard, int BacthId, DateTime Day, out int Status)
        {
            Status = -1;
            bool exist = false;
            string sql = "SELECT t2.PresentStatus FROM tblattdcls_std" + standard + "yr" + BacthId + " t1 INNER JOIN tblattdstud_std" + standard + "yr" + BacthId + " t2 on t1.Id= t2.ClassAttendanceId  where t1.`date`='" + Day.Date.ToString("s") + "' AND t2.StudentId=" + _studId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out Status);
                if (Status >= 0)
                {
                    exist = true;
                }
            }


            return exist;
        }




        public int GetWorkingDaysForThePeriod_New(int _studentid, string standard, int BtchId, string _sdate, string _enddate)
        {
            int _TotalworkingDay = 0;
            Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + standard + "yr" + BtchId + " t1 INNER JOIN tblattdstud_std" + standard + "yr" + BtchId + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studentid + " AND t1.`Date` BETWEEN '" + _sdate + "' and '" + _enddate + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalworkingDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalworkingDay;
        }

        public int GetNoOf_AbsentDayForTheperiod_New(int _studentid, string standard, int BtchId, string _sdate, string _enddate)
        {
            int _TotalAbsentDay = 0;
            Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + standard + "yr" + BtchId + " t1 INNER JOIN tblattdstud_std" + standard + "yr" + BtchId + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studentid + " AND t2.PresentStatus=0 AND t1.`Date` BETWEEN '" + _sdate + "' and '" + _enddate + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalAbsentDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalAbsentDay;
        }


        public int GetNoOf_FullDayForTheperiod_New(int _studentid, string standard, int BtchId, string _sdate, string _enddate)
        {
            int _TotalFullDay = 0;
            Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + standard + "yr" + BtchId + " t1 INNER JOIN tblattdstud_std" + standard + "yr" + BtchId + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studentid + " AND t2.PresentStatus=3 AND t1.`Date` BETWEEN '" + _sdate + "' and '" + _enddate + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalFullDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalFullDay;
        }


        public int GetNoOf_HalfDayForTheperiod_New(int _studentid,string standard, int BtchId, string _sdate, string _enddate)
        {
            int _TotalFullDay = 0;
            Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + standard + "yr" + BtchId + " t1 INNER JOIN tblattdstud_std" + standard + "yr" + BtchId + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studentid + " AND (t2.PresentStatus=1 OR t2.PresentStatus=2) AND t1.`Date` BETWEEN '" + _sdate + "' and '" + _enddate + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _TotalFullDay = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            return _TotalFullDay;
        }

        public void GetbatchDates(int BatchId, out DateTime _Start, out DateTime _End)
        {
            _Start = new DateTime();
            _End = new DateTime();
            string Sql = "SELECT StartDate,EndDate FROM tblbatch WHERE Id=" + BatchId;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                DateTime.TryParse(m_MyReader.GetValue(0).ToString(), out _Start);
                DateTime.TryParse(m_MyReader.GetValue(1).ToString(), out _End);
            }

        }

        public bool IsNewAttendance()
        {
            string Page = "MarkClassAttendanceMaster.aspx";
            bool NewAttd = false;
            Sql = "SELECT Link FROM tblaction WHERE Id=80";
            if (m_TransationDb != null)
            {
                m_MyReader = m_TransationDb.ExecuteQuery(Sql);
            }
            else
            {
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            }
            if (m_MyReader.HasRows)
            {
                if (m_MyReader.GetValue(0).ToString() == Page)
                {
                    NewAttd = true;
                }

            }
            return NewAttd;
        }

        # endregion





        # region Events Manager

        public DataSet MyAssociatedEventsDataSet(int Classid, int UserId)
        {
            DataSet Events = null;
            string sql = "select distinct tblmasterdate.`date`, tblcalender_event.ClassId, tbleventmaster.EventName,tbleventmaster.Description from tblmasterdate inner join tblcalender_event on tblcalender_event.DateId= tblmasterdate.Id INNER JOIN tbleventmaster ON tbleventmaster.Id=tblcalender_event.EventId where  (tblcalender_event.ClassId=0) or ( tblcalender_event.ClassId=" + Classid + ")";
            Events = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return Events;
        }

        public bool IsEventPresent(DateTime Day, int ClassId, out string EventName,out string Id,out string Desc,out string publish)
        {
            bool valid = false;
            EventName = "";
            Id = "";
            Desc = "";
            publish = "";

            string sql = "select tbleventmaster.Id,tbleventmaster.EventName,tbleventmaster.Description,tbleventmaster.IsPublish from tblmasterdate inner join tblcalender_event on tblcalender_event.DateId= tblmasterdate.Id INNER JOIN tbleventmaster ON tbleventmaster.Id=tblcalender_event.EventId where (tblcalender_event.ClassId=0 or tblcalender_event.ClassId=" + ClassId + ") and tblmasterdate.`date`='" + Day.Date.ToString("s") + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                EventName = m_MyReader.GetValue(1).ToString();
                Id = m_MyReader.GetValue(0).ToString();
                Desc = m_MyReader.GetValue(2).ToString();
                publish = m_MyReader.GetValue(3).ToString();
                valid = true;
            }
            return valid;
        }



        public void InsertEvents(int EventId, int ClassId, DateTime _currentday)
        {
            int _GetId = getDateIdFrmMastTbl(_currentday);
            string sql = "insert into tblcalender_event (DateId,ClassId,EventId) values(" + _GetId + "," + ClassId + "," + EventId + ")";
            m_TransationDb.TransExecuteQuery(sql);
        }

        public void InsertMasterEvent(string EventName, string description, bool Publish,string usename, out int EventId)
        {
            EventId = 0;
            int ispublish = 0;
            if (Publish)
            {
                ispublish = 1;
            }
            string date = DateTime.Now.ToString("s");
            string sql = "insert into tbleventmaster (EventName,Description,IsPublish,CreatedBy,CreatedDateTime) values('" + EventName + "','" + description + "'," + ispublish + ",'" + usename + "','"+date+"')";
            m_TransationDb.TransExecuteQuery(sql);
            EventId = GetEventMasterId(EventName, description, ispublish, date, usename);
        }

        private int GetEventMasterId(string EventName, string description, int ispublish, string date, string usename)
        {
            int Id = 0;
            Sql = "SELECT Id FROM tbleventmaster WHERE EventName='" + EventName + "' AND Description='" + description + "' AND IsPublish=" + ispublish + " AND CreatedBy='" + usename + "' AND CreatedDateTime='" + date + "'";
            m_MyReader = m_TransationDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out Id);

            }
            return Id;
        }


        public string GetFormatedDateString_EventView(string DateSr)
        {
            string[] DateArray = DateSr.Split(',');
            string DateNewString = "", TempStr = "";

            if (DateArray.Length > 1)
            {
                string startDate = General.GetDateTimeFromText(DateArray[0]).ToString("MMM d,yyyy");
                string LastDate = General.GetDateTimeFromText(DateArray[DateArray.Length - 1]).ToString("MMM d,yyyy");
                TempStr = startDate + " to " + LastDate;
            }
            else
            {
                TempStr = General.GetDateTimeFromText(DateArray[0]).ToString("MMM d yyyy");
            }

            DateNewString = "<table width=\"100%\"><tr> <td align=\"left\"> No. of event days : " + DateArray.Length + " </td> </tr> <tr> <td align=\"left\">" + TempStr + "</td> </tr> </table>";

            return DateNewString;
        }

        # endregion


        
        //public int getWrkingBatch(string _session)
        //{
        //    int batch = 0;
        //    string sql = " select max(tblstudentclassmap.BatchId) from tblstudentclassmap inner join tblstudent on tblstudent.LastClassId = tblstudentclassmap.ClassId where tblstudentclassmap.StudentId = " + int.Parse(_session);//+ int.Parse(_session)+";
        //    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        //    if (m_MyReader.HasRows)
        //    {
        //        while (m_MyReader.Read())
        //        {

        //            batch = int.Parse(m_MyReader.GetValue(0).ToString());
        //        }
        //    }
        //    else
        //    {
        //        sql = "select max(tblstudentclassmap_history.BatchId) from tblstudentclassmap_history inner join tblstudent on tblstudent.LastClassId = tblstudentclassmap_history.ClassId where tblstudentclassmap_history.StudentId = " + _session;
        //        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        //        if (m_MyReader.HasRows)
        //        {
        //            while (m_MyReader.Read())
        //            {
        //                batch = int.Parse(m_MyReader.GetValue(0).ToString());
        //            }
        //        }
        //    }
        //    return batch;
        //}

        public int getWrkingBatch(string _session,int std)
        {
            int batch = 0;
            //string sql = " select max(tblview_studentclassmap.BatchId) from tblview_studentclassmap inner join tblstudent on tblstudent.LastClassId = tblview_studentclassmap.ClassId where tblview_studentclassmap.StudentId = " + int.Parse(_session);//+ int.Parse(_session)+";
            string sql = "     select max(tblview_studentclassmap.BatchId) from tblview_studentclassmap where tblview_studentclassmap.StudentId = "+_session+" and       tblview_studentclassmap.Standard = "+std+"";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    
                        batch = int.Parse(m_MyReader.GetValue(0).ToString());
                    
                }
            }
            return batch;
        }


        public DateTime GetLastAttendDay(string _session, int _ClassId, int std, int batch)
        {
            DateTime _LastPresentDate = new DateTime();
            string sql = "SELECT Max(t1.Date) FROM tblattdcls_std" + std + "yr" + batch + " t1 INNER JOIN tblattdstud_std" + std + "yr" + batch + " t2 ON t2.ClassAttendanceId = t1.Id  WHERE t2.StudentId=" + _session + " AND t2.PresentStatus!=0"; 
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    if (m_MyReader.GetValue(0).ToString() != "")
                        _LastPresentDate = DateTime.Parse(m_MyReader.GetValue(0).ToString());
                }
            }
            return _LastPresentDate;
        }



        public void GetPrevBatchNewattendanceDetails(int studentId, out int _no_workingdays, out int _no_presentdays,int classid, int batch)
        {
            _no_workingdays = 0;
            _no_presentdays = 0;
            string sql = "select tblattdperivousbatch.WorkingDays, tblattdperivousbatch.PresentDays  + tblattdperivousbatch.HalfDays from tblattdperivousbatch where tblattdperivousbatch.StudentId=" + studentId + " and tblattdperivousbatch.ClassId=" + classid + " and tblattdperivousbatch.BatchId=" + batch;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    if (m_MyReader.GetValue(0).ToString() != "")
                    {
                        _no_workingdays = int.Parse(m_MyReader.GetValue(0).ToString());
                        _no_presentdays = int.Parse(m_MyReader.GetValue(1).ToString());
                    }
                }
            }
        }

        public int LastClassId(string _session)
        {

            int lastclass = 0;
            string sql = "select LastClassId from tblstudent where Id=" + _session + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    if (m_MyReader.GetValue(0).ToString() != "")
                    {
                        lastclass = int.Parse(m_MyReader.GetValue(0).ToString()); 
                    }
                }
            }
            return lastclass;
        }

        public bool IsRollNoCofigEnabled()
        {
            bool _valid = false;
            string Status;
            string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='RoleNoEnabled'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                Status = m_MyReader.GetValue(0).ToString();
                if (Status == "1")
                {
                    _valid = true;
                }
            }
            return _valid;
        }

        private string GetStaffDetails(int StaffId)
        {
            string StaffName = "";
            string sql = "select SurName from tbluser where Id=" + StaffId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {

                StaffName = m_MyReader.GetValue(0).ToString();

            }
            return StaffName;
        }

        public void MarkAttandance_External(string _UniqueId, DateTime _DateTime, string RFReaderType)
        {
            try
            {
                CreateExternalTansationDb();
                int UserId = 0;
                int GroupId = 0;
                bool IsStudent = false;
                if (IsUniqueIdIdentified(_UniqueId, out IsStudent, out UserId, out GroupId)) // Student Attendance Code
                {
                    if (IsStudent)
                    {
                        MarkAttendanceForstudent(UserId, _DateTime, RFReaderType, GroupId);
                    }
                    else // Staff Attendance Code
                    {
                        MarkAttendanceForStaff(UserId, _DateTime, RFReaderType);

                    }

                }
                else // Unknown _UniqueId
                {
                   
                  
                }

                EndSucessExternalTansationDb();
            }
            catch
            {
                EndFailExternalTansationDb();
            }
        }

        private void MarkAttendanceForStaff(int StaffId, DateTime _DateTime, string RFReaderType)
        {
            DateTime _Date = _DateTime.Date;
            TimeSpan _Time = _DateTime.TimeOfDay;
            string OldInTime="",OldOutTime="",OldPresentHours="";

            if (_StaffAttendance_MarkingPossible(RFReaderType, _Time))
            {
                int oldStatus = GetStaffAttedanceStatus_OneStaff(StaffId.ToString(), _Date, out OldInTime, out OldOutTime, out OldPresentHours);

                string _LateValue = "00:00:00";
                string _IsLate = "0";
                if (RFReaderType == "INTIME")
                {
                   
                    TimeSpan _LateIntime = TimeSpan.Parse(GetStaffAttend_ConfigValue("Late InTime"));
                    if (_Time > _LateIntime)
                    {
                        TimeSpan LateValue = _Time - _LateIntime;
                        _IsLate = "1";
                        _LateValue = LateValue.ToString();
                    }
                   
                    if (oldStatus == 0)
                    {
                        SaveStaffAttendance(StaffId.ToString(), _Date, "3", _Time.ToString(), OldOutTime, _IsLate, _LateValue);
                    }
                    else if (OldInTime == "" || OldInTime == "00:00:00")
                    {
                        SaveStaffAttendance(StaffId.ToString(), _Date, "3", _Time.ToString(), OldOutTime, _IsLate, _LateValue);
                    }
                    Load_InTimeSms_Staff(StaffId, _Time, _Date);
                }
                else if (RFReaderType == "OUTTIME")
                {

                    if (oldStatus == 0)
                    {
                        SaveStaffAttendance(StaffId.ToString(), _Date, "3", _Time.ToString(), _Time.ToString(), _IsLate, _LateValue);
                    }
                    else if (OldInTime == "" || OldInTime == "00:00:00")
                    {
                        SaveStaffAttendance(StaffId.ToString(), _Date, "3", _Time.ToString(), _Time.ToString(), _IsLate, _LateValue);
                    }
                    else
                    {
                        SaveStaffAttendance(StaffId.ToString(), _Date, "3", OldInTime, _Time.ToString(), _IsLate, _LateValue);
                    }
                    Load_OutTimeSms_Staff(StaffId, _Time, _Date);
                }
            }
            
        }

        private void Load_OutTimeSms_Staff(int StaffId, TimeSpan _Time, DateTime _Date)
        {
            string SMSMessage = "";
            string ScheduleTime = "0";
            if (_Date.Date == DateTime.Now.Date)
            {
                if (SMS_Enabled("Onstaffexit", out SMSMessage, out ScheduleTime))
                {
                    TimeSpan _ScheduleTime = TimeSpan.Parse(ScheduleTime);
                    if (_ScheduleTime <= DateTime.Now.TimeOfDay)
                    {
                        string PhoneNumbers = GetConfigValue("Staff Attendance PhoneNumber");
                        string[] Phones = PhoneNumbers.Split(',');
                        string StaffName = GetStaffDetails(StaffId);
                        SMSMessage = SMSMessage.Replace("($StaffName$)", StaffName);
                        SMSMessage = SMSMessage.Replace("($Date$)", General.GerFormatedDatVal(_Date));
                        SMSMessage = SMSMessage.Replace("($Time$)", _Time.ToString());

                        for (int i = 0; i < Phones.Length; i++)
                        {

                            AddtoAutoSMS(Phones[i], SMSMessage, "0");
                        }

                    }

                }
            }
        }


        private void Load_InTimeSms_Staff(int StaffId, TimeSpan _Time, DateTime _Date)
        {
            string SMSMessage = "";
            string ScheduleTime = "0";
            if (_Date.Date == DateTime.Now.Date)
            {
                if (SMS_Enabled("Onstaffpresent", out SMSMessage, out ScheduleTime))
                {
                    TimeSpan _ScheduleTime = TimeSpan.Parse(ScheduleTime);
                    if (_ScheduleTime <= DateTime.Now.TimeOfDay)
                    {
                        string PhoneNumbers = GetConfigValue("Staff Attendance PhoneNumber");
                        string[] Phones = PhoneNumbers.Split(',');
                        string StaffName = GetStaffDetails(StaffId);
                        SMSMessage = SMSMessage.Replace("($StaffName$)", StaffName);
                        SMSMessage = SMSMessage.Replace("($Date$)", General.GerFormatedDatVal(_Date));
                        SMSMessage = SMSMessage.Replace("($Time$)", _Time.ToString());

                        for (int i = 0; i < Phones.Length; i++)
                        {

                            AddtoAutoSMS(Phones[i], SMSMessage, "0");
                        }

                    }

                }
            }
        }



        private void MarkAttendanceForstudent(int StudentId, DateTime _DateTime, string RFReaderType, int GroupId)
        {
            DateTime _Date = _DateTime.Date;
            TimeSpan _Time = _DateTime.TimeOfDay;
            int ClassAttendanceId = 0;
            int UserId = 1;
            string StandardId = "";
            int YearId = 0;
            string Status = "3",MarkedStatus="0";
            int ClassId = 0;

            if (_StudentAttendance_MarkingPossible(RFReaderType, _Time, GroupId))
            {
                GetStudentDetails(StudentId, out ClassId, out StandardId, out YearId);
                if (!AttendanceTables_Exits(StandardId, YearId))
                {
                    AttendenceTableManager myAttManagerObj = new AttendenceTableManager(m_TransationDb);
                    myAttManagerObj.CreateSingle_StudentAttendanceTables(int.Parse(StandardId), YearId);
                    myAttManagerObj = null;
                }

                if (!IsAttendanceAlreadyMarked(ClassId, _Date, StandardId, YearId, out MarkedStatus, out ClassAttendanceId))
                {
                    SaveClassAttendanceDetails(ClassId, _Date, UserId, StandardId, YearId, Status, out ClassAttendanceId);

                    if (ClassAttendanceId > 0)
                    {
                        Sql = "SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudent.Status=1 AND tblstudentclassmap.ClassId=" + ClassId + " AND tblstudentclassmap.BatchId=" + YearId + " AND tblstudentclassmap.Standard=" + StandardId;
                        OdbcDataReader m_MyReader1 = m_TransationDb.ExecuteQuery(Sql);
                        if (m_MyReader1.HasRows)
                        {
                            while (m_MyReader1.Read())
                            {
                                UpdateStudentAttendanceDetails(ClassAttendanceId, m_MyReader1.GetValue(0).ToString(), 0, ClassId, StandardId, YearId);
                            }
                        }
                    }
                    UpdateStudentAttendanceDetails(ClassAttendanceId, StudentId.ToString(), 3, ClassId, StandardId, YearId);
                }
                else
                {
                    if (ClassAttendanceId > 0)
                    {
                        UpdateStudentAttendanceDetails(ClassAttendanceId, StudentId.ToString(), 3, ClassId, StandardId, YearId);
                    }
                }

                if (RFReaderType == "INTIME")
                {
                    if (!IsInTimeAlreadyExists(ClassAttendanceId, StudentId, StandardId, YearId))
                    {
                        UpdateStudentAttendanceTimeDetails(ClassAttendanceId, StudentId.ToString(), ClassId, StandardId, YearId, RFReaderType, _Time,GroupId);
                        Load_InTimeSms(StudentId, _Time, _Date);
                    }
                }

                if (RFReaderType == "OUTTIME")
                {
                    UpdateStudentAttendanceTimeDetails(ClassAttendanceId, StudentId.ToString(), ClassId, StandardId, YearId, RFReaderType, _Time,GroupId);
                    Load_OutTimeSms(StudentId, _Time, _Date);
                }
            }
        }

        private void Load_InTimeSms(int StudentId, TimeSpan _Time, DateTime _Date)
        {
            string SMSMessage = "";
            string ScheduleTime = "0";
            if (_Date.Date == DateTime.Now.Date)
            {
                if (SMS_Enabled("OnPesent", out SMSMessage, out ScheduleTime))
                {
                    TimeSpan _ScheduleTime = TimeSpan.Parse(ScheduleTime);
                    if (_ScheduleTime <= DateTime.Now.TimeOfDay)
                    {
                        string StudentName = "";
                        string ParentName = "";
                        string PhoneNumber = "";
                        bool ParentEnabled = false;

                        GetParentDetails(StudentId, out StudentName, out ParentName, out PhoneNumber, out ParentEnabled);
                        if (ParentEnabled)
                        {

                            SMSMessage = SMSMessage.Replace("($Student$)", StudentName);
                            SMSMessage = SMSMessage.Replace("($Parent$)", ParentName);
                            SMSMessage = SMSMessage.Replace("($Date$)", General.GerFormatedDatVal(_Date));
                            SMSMessage = SMSMessage.Replace("($Time$)", _Time.ToString());

                            AddtoAutoSMS(PhoneNumber, SMSMessage, "0");
                            AddReportCount("OnPesent", _Date);
                        }

                    }

                }
            }
        }



        private void Load_OutTimeSms(int StudentId, TimeSpan _Time, DateTime _Date)
        {
            string SMSMessage = "";
            string ScheduleTime = "0";
            if (_Date.Date == DateTime.Now.Date)
            {
                if (SMS_Enabled("OnExit", out SMSMessage, out ScheduleTime))
                {
                    TimeSpan _ScheduleTime = TimeSpan.Parse(ScheduleTime);
                    if (_ScheduleTime <= DateTime.Now.TimeOfDay)
                    {
                        string StudentName = "";
                        string ParentName = "";
                        string PhoneNumber = "";
                        bool ParentEnabled = false;

                        GetParentDetails(StudentId, out StudentName, out ParentName, out PhoneNumber, out ParentEnabled);
                        if (ParentEnabled)
                        {

                            SMSMessage = SMSMessage.Replace("($Student$)", StudentName);
                            SMSMessage = SMSMessage.Replace("($Parent$)", ParentName);
                            SMSMessage = SMSMessage.Replace("($Date$)", General.GerFormatedDatVal(_Date));
                            SMSMessage = SMSMessage.Replace("($Time$)", _Time.ToString());

                            AddtoAutoSMS(PhoneNumber, SMSMessage, "0");
                            AddReportCount("OnExit", _Date);
                        }

                    }

                }
            }
        }

        private void  AddReportCount(string _type, DateTime _Date)
        {
            int _count = 0;
            string sql = "SELECT tblsmsoptionreport.SendCount FROM tblsmsoptionreport INNER JOIN tblsmsoptionconfig ON tblsmsoptionconfig.Id=tblsmsoptionreport.OptionId WHERE tblsmsoptionconfig.ShortName='" + _type + "' AND tblsmsoptionreport.DoneDate='" + _Date.Date.ToString("s") + "'";
            OdbcDataReader _myreader = m_TransationDb.ExecuteQuery(sql);
            if (_myreader.HasRows)
            {
                int.TryParse(_myreader.GetValue(0).ToString(), out _count);
            }

            _count++;

            sql = " UPDATE tblsmsoptionreport INNER JOIN tblsmsoptionconfig ON tblsmsoptionconfig.Id=tblsmsoptionreport.OptionId  SET tblsmsoptionreport.SendCount=" + _count + ",tblsmsoptionreport.DoneDate='" + _Date.Date.ToString("s") + "' WHERE tblsmsoptionconfig.ShortName='" + _type + "'";
            m_TransationDb.ExecuteQuery(sql);
        }



        private bool SMSAlreadySend(string type,DateTime _date)
        {
            bool valid = false;
            int _count = 0;
            string sql = "SELECT COUNT(tblsmsoptionreport.OptionId) FROM tblsmsoptionreport INNER JOIN tblsmsoptionconfig ON tblsmsoptionconfig.Id=tblsmsoptionreport.OptionId WHERE tblsmsoptionconfig.ShortName='" + type + "' AND tblsmsoptionreport.DoneDate='" + _date.ToString("s") + "'";
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out _count);
                if (_count > 0)
                {
                    valid = true;
                }

            }
            return valid;
        }



        public bool SMS_Enabled(string Type, out string SmsMessage, out string ScheduleTime)
        {
            bool valid = false;
            SmsMessage = "";
            ScheduleTime = "0";
            string Enable = "";
            string sql = "SELECT `Enable`,`Format`,ScheduledTime FROM tblsmsoptionconfig WHERE tblsmsoptionconfig.SetVisible=1 AND tblsmsoptionconfig.ShortName='" + Type + "' ";
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                Enable = m_MyReader.GetValue(0).ToString();
                if (Enable == "1")
                {
                    valid = true;
                    SmsMessage = m_MyReader.GetValue(1).ToString();
                    ScheduleTime = m_MyReader.GetValue(2).ToString();
                }

            }
            return valid;
        }


        public void GetParentDetails(int StudentId, out string StudentName, out string ParentName, out string PhoneNumber, out bool ParentEnabled)
        {
            StudentName = "";
            ParentName = "";
            PhoneNumber = "";
            ParentEnabled = false;
            int _status = 0;

            string sql = "SELECT StudentName,GardianName FROM tblview_student WHERE Id=" + StudentId;
            OdbcDataReader mReader = m_TransationDb.ExecuteQuery(sql);
            if (mReader.HasRows)
            {
                StudentName = mReader.GetValue(0).ToString();
                ParentName = mReader.GetValue(1).ToString();
            }

            sql = "SELECT PhoneNo,Enabled FROM tblsmsparentlist WHERE Id=" + StudentId;
            OdbcDataReader mReader1 = m_TransationDb.ExecuteQuery(sql);
            if (mReader1.HasRows)
            {
                PhoneNumber = mReader1.GetValue(0).ToString();
                int.TryParse(mReader1.GetValue(1).ToString(), out _status);
                if (_status == 1)
                {
                    ParentEnabled = true;
                }
            }

        }



        public void AddtoAutoSMS(string PhoneNumber, string SmsMessage, string ScheduleTime)
        {
            string sql = "INSERT INTO tblautosms (PhoneNumber,Message,TimeToSend,Status) VALUES('" + PhoneNumber + "','" + SmsMessage + "','" + ScheduleTime + "',0)";
            if (m_TransationDb != null)
            {
                m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MysqlDb.ExecuteQuery(sql);
            }
        }

        private bool IsInTimeAlreadyExists(int ClassAttendanceId, int StudentId, string StandardId, int YearId)
        {
            bool valid=false;
            string INtimeStr = "";
            string sql = "SELECT InTime FROM tblattdstud_std" + StandardId + "yr" + YearId + " WHERE StudentId=" + StudentId + " AND ClassAttendanceId=" + ClassAttendanceId;
            OdbcDataReader _reader = m_TransationDb.ExecuteQuery(sql);
            if (_reader.HasRows)
            {
                INtimeStr = _reader.GetValue(0).ToString();
            }

            if (INtimeStr != "")
            {
                valid = true;
            }
            return valid;
        }




        private void GetStudentDetails(int StudentId, out int ClassId, out string StandardId, out int YearId)
        {
            ClassId = 0;
            StandardId = "";
            YearId = 0;
            string sql = "SELECT tblview_student.ClassId,tblclass.Standard FROM tblview_student INNER JOIN tblclass ON tblview_student.ClassId=tblclass.Id WHERE tblview_student.Id=" + StudentId;
            OdbcDataReader _reader = m_TransationDb.ExecuteQuery(sql);
            if (_reader.HasRows)
            {
                int.TryParse(_reader.GetValue(0).ToString(), out ClassId);
                StandardId=_reader.GetValue(1).ToString();
            }


            sql = "SELECT tblbatch.Id FROM tblbatch WHERE tblbatch.`Status`=1";
            OdbcDataReader _reader1 = m_TransationDb.ExecuteQuery(sql);
            if (_reader1.HasRows)
            {
                int.TryParse(_reader1.GetValue(0).ToString(), out YearId);
            }
        }

        private bool IsUniqueIdIdentified(string _UniqueId, out bool IsStudent, out int UserId, out int AssociatedGroupId)
        {
            OdbcDataReader m_NewReader;
            bool valid = false;
            IsStudent = false;
            UserId = 0;
            AssociatedGroupId = 0;
            string _Type = "";
            string sql = "SELECT tblexternalreff.UserId,tblexternalreff.UserType FROM tblexternalreff WHERE tblexternalreff.ISActive=1 AND tblexternalreff.Id='" + _UniqueId + "'";
            if (m_TransationDb != null)
            {
                m_NewReader = m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_NewReader = m_MysqlDb.ExecuteQuery(sql);
            }
            if (m_NewReader.HasRows)
            {
                int.TryParse(m_NewReader.GetValue(0).ToString(), out UserId);

                _Type = m_NewReader.GetValue(1).ToString();
            }

            if (UserId > 0)
            {
                
                if (_Type == "STUDENT")
                {
                    IsStudent = true;
                }

                AssociatedGroupId = GetUserGroupId(UserId, IsStudent);
                if (AssociatedGroupId > 0)
                {
                    valid = true;
                }
            }
            return valid;
        }

        private int GetUserGroupId(int UserId, bool IsStudent)
        {
            int GroupId = 0;
            OdbcDataReader m_NewReader;
            string sql = "";
            if (IsStudent)
            {
                sql = "SELECT tblclass.ParentGroupID FROM tblclass INNER JOIN tblstudentclassmap ON tblstudentclassmap.ClassId=tblclass.Id WHERE tblstudentclassmap.StudentId=" + UserId;   
            }
            else
            {
                sql = "SELECT tblgroupusermap.GroupId FROM tblgroupusermap WHERE tblgroupusermap.UserId=" + UserId;
            }

            if (m_TransationDb != null)
            {
                m_NewReader = m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_NewReader = m_MysqlDb.ExecuteQuery(sql);
            }
            if (m_NewReader.HasRows)
            {
                int.TryParse(m_NewReader.GetValue(0).ToString(), out GroupId);
            }
            return GroupId;
        }


        public int GetStaffAttendanceStatus(DateTime TempDate)
        {
            int StatusCount = 0;
            string sql = "SELECT COUNT(tblstaffattendance.MarkStatus) FROM tblstaffattendance WHERE tblstaffattendance.MarkDate='" + TempDate.ToString("s")+ "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out StatusCount);
            }
            return StatusCount;
        }

        public int GetStaffAttedanceStatus_OneStaff(string StaffId, DateTime _NewDate, out string Intime, out string OutTime, out string PresentHours)
        {
            int StatusCount = 0;
            Intime = "";
            OutTime = "";
            PresentHours = "";
            string sql = "SELECT tblstaffattendance.MarkStatus,tblstaffattendance.InTime,tblstaffattendance.OutTime,tblstaffattendance.PresentHours FROM tblstaffattendance WHERE tblstaffattendance.StaffId=" + StaffId + " AND tblstaffattendance.MarkDate='" + _NewDate.Date.ToString("s") + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out StatusCount);
                Intime = m_MyReader.GetValue(1).ToString();
                OutTime = m_MyReader.GetValue(2).ToString();
                PresentHours = m_MyReader.GetValue(3).ToString();
            }
            return StatusCount;
        }



        public void SaveStaffAttendance(string StaffId, DateTime dateTime, string MarkStatus, string InTime, string OutTime,string _isLate,string LateVale)
        {
           
            if (StaffAttendanceEnteredOnDay(StaffId, dateTime))
            {
                UpdateStaffAttendance(StaffId, dateTime, MarkStatus, InTime, OutTime);
            }
            else
            {
                InsertStaffAttendance(StaffId, dateTime, MarkStatus, InTime, OutTime, _isLate, LateVale);
            }

          
        }

        private void InsertStaffAttendance(string StaffId, DateTime dateTime, string MarkStatus, string InTime, string OutTime, string _isLate, string LateVale)
        {
            TimeSpan _Intime;
            TimeSpan _Outtime;
            TimeSpan.TryParse(InTime, out _Intime);
            TimeSpan.TryParse(OutTime, out _Outtime);
            TimeSpan WorkHours = _Outtime - _Intime;

            string sql = "INSERT INTO tblstaffattendance (StaffId,MarkDate,MarkStatus,InTime,OutTime,PresentHours,IsLate,LateValue) VALUES (" + StaffId + ",'" + dateTime.ToString("s") + "'," + MarkStatus + ",'" + InTime + "','" + OutTime + "','" + WorkHours.ToString() + "','" + _isLate + "','"+LateVale+"')";
            m_TransationDb.ExecuteQuery(sql);
        }

        private void UpdateStaffAttendance(string StaffId, DateTime dateTime, string MarkStatus, string InTime, string OutTime)
        {
            TimeSpan _Intime;
            TimeSpan _Outtime;
            TimeSpan.TryParse(InTime,out _Intime);
            TimeSpan.TryParse(OutTime, out _Outtime);
            TimeSpan WorkHours = _Outtime - _Intime;
            string sql = "UPDATE tblstaffattendance SET MarkStatus=" + MarkStatus + ",InTime='" + InTime + "',OutTime='" + OutTime + "',PresentHours='" + WorkHours.ToString() + "' WHERE StaffId=" + StaffId + " AND MarkDate='" + dateTime.ToString("s") + "'";
            m_TransationDb.ExecuteQuery(sql);
        }

        public bool StaffAttendanceEnteredOnDay(string StaffId, DateTime dateTime)
        {
            bool valid = false;
            int count = 0;
            string sql = "SELECT COUNT(tblstaffattendance.Id) FROM tblstaffattendance WHERE tblstaffattendance.StaffId=" + StaffId + " AND tblstaffattendance.MarkDate='" + dateTime.ToString("s") + "'";
            if (m_TransationDb != null)
            {
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            }
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(),out count);
                if (count > 0)
                {
                    valid = true;
                }
            }
            return valid;
        }

        public int GetNo_StaffAttendanceMarkedInMonth(string MonthId, string YearId)
        {
            int count = 0;
            string sql = "SELECT COUNT(DISTINCT tblstaffattendance.MarkDate) FROM tblstaffattendance WHERE YEAR(tblstaffattendance.MarkDate)=" + YearId + " AND MONTH(tblstaffattendance.MarkDate)=" + MonthId;
            OdbcDataReader newreader = m_MysqlDb.ExecuteQuery(sql);
            if (newreader.HasRows)
            {
                int.TryParse(newreader.GetValue(0).ToString(), out count);
            }
            return count;
        }

        public int GetNo_FullDayAttendance_Month_Staff(int StaffId, string MonthId, string YearId)
        {
            int count = 0;
            string sql = "SELECT COUNT(tblstaffattendance.Id) FROM tblstaffattendance WHERE YEAR(tblstaffattendance.MarkDate)=" + YearId + " AND MONTH(tblstaffattendance.MarkDate)=" + MonthId + " AND tblstaffattendance.MarkStatus=3 AND tblstaffattendance.StaffId=" + StaffId;
            OdbcDataReader newreader = m_MysqlDb.ExecuteQuery(sql);
            if (newreader.HasRows)
            {
                int.TryParse(newreader.GetValue(0).ToString(), out count);
            }
            return count;
        }

        public int GetNo_HalfDayAttendance_Month_Staff(int StaffId, string MonthId, string YearId)
        {
            int count = 0;
            string sql = "SELECT COUNT(tblstaffattendance.Id) FROM tblstaffattendance WHERE YEAR(tblstaffattendance.MarkDate)=" + YearId + " AND MONTH(tblstaffattendance.MarkDate)=" + MonthId + " AND (tblstaffattendance.MarkStatus=1 OR tblstaffattendance.MarkStatus=2) AND tblstaffattendance.StaffId=" + StaffId;
            OdbcDataReader newreader = m_MysqlDb.ExecuteQuery(sql);
            if (newreader.HasRows)
            {
                int.TryParse(newreader.GetValue(0).ToString(), out count);
            }
            return count;
        }


        public int GetNo_AbsentAttendance_Month_Staff(int StaffId, string MonthId, string YearId)
        {
            int count = 0;
            string sql = "SELECT COUNT(tblstaffattendance.Id) FROM tblstaffattendance WHERE YEAR(tblstaffattendance.MarkDate)=" + YearId + " AND MONTH(tblstaffattendance.MarkDate)=" + MonthId + " AND tblstaffattendance.MarkStatus=0 AND tblstaffattendance.StaffId=" + StaffId;
            OdbcDataReader newreader = m_MysqlDb.ExecuteQuery(sql);
            if (newreader.HasRows)
            {
                int.TryParse(newreader.GetValue(0).ToString(), out count);
            }
            return count;
        }

        public bool isRFReaderRegistered(string _RFReaderID)
        {
            bool valid = false;
            int count = 0;
            string sql = "SELECT COUNT(tblexternalrfid.EquipmentId) FROM tblexternalrfid WHERE tblexternalrfid.EquipmentId='" + _RFReaderID + "'";
            OdbcDataReader newreader = m_MysqlDb.ExecuteQuery(sql);
            if (newreader.HasRows)
            {
                int.TryParse(newreader.GetValue(0).ToString(), out count);
                if (count > 0)
                {
                    valid = true;
                }
            }
            return valid;
        }

        private bool _StaffAttendance_MarkingPossible(string RFReaderType, TimeSpan _Time)
        {
            bool valid = false;
            TimeSpan _startIntime = TimeSpan.Parse(GetStaffAttend_ConfigValue("Start InTime"));
            TimeSpan _endIntime = TimeSpan.Parse(GetStaffAttend_ConfigValue("End InTime"));
            TimeSpan _startOuttime = TimeSpan.Parse(GetStaffAttend_ConfigValue("Start OutTime"));
            TimeSpan _endOuttime = TimeSpan.Parse(GetStaffAttend_ConfigValue("End OutTime"));
            if (RFReaderType == "INTIME")
            {
                if (_Time > _startIntime && _Time < _endIntime)
                {
                    valid = true;
                }
            }

            if (RFReaderType == "OUTTIME")
            {

                if (_Time > _startOuttime && _Time < _endOuttime)
                {
                    valid = true;
                }

            }
            return valid;
        }

        private bool _StudentAttendance_MarkingPossible(string RFReaderType, TimeSpan _Time, int GroupId)
        {
            bool valid = false;
            TimeSpan _startIntime = TimeSpan.Parse(GetStudentAttend_ConfigValue("Start InTime", GroupId));
            TimeSpan _endIntime = TimeSpan.Parse(GetStudentAttend_ConfigValue("End InTime", GroupId));
            TimeSpan _startOuttime = TimeSpan.Parse(GetStudentAttend_ConfigValue("Start OutTime", GroupId));
            TimeSpan _endOuttime = TimeSpan.Parse(GetStudentAttend_ConfigValue("End OutTime", GroupId));
            string _needOutTime = GetStudentAttend_ConfigValue("Need OutTime", GroupId);
            if (RFReaderType == "INTIME")
            {
                if (_Time > _startIntime && _Time < _endIntime)
                {
                    valid = true;
                }
            }

            if(RFReaderType == "OUTTIME")
            {
                if (_needOutTime == "YES")
                {
                    if (_Time > _startOuttime && _Time < _endOuttime)
                    {
                        valid = true;
                    }

                }
            }
            return valid;
        }

        public void CheckRFDeviceStatus_SendWarning(string _RFReaderID, DateTime _DateTime, string laststatus)
        {
            TimeSpan _ntime = _DateTime.TimeOfDay;

            TimeSpan _startIntime = TimeSpan.Parse(GetStudentAttend_GeneralConfigValue("Start InTime"));
            TimeSpan _endIntime = TimeSpan.Parse(GetStudentAttend_GeneralConfigValue("End InTime"));
            TimeSpan _LateIntime = TimeSpan.Parse(GetStudentAttend_GeneralConfigValue("Late InTime"));
            TimeSpan _endOuttime = TimeSpan.Parse(GetStudentAttend_GeneralConfigValue("End OutTime"));

            TimeSpan _StartInTimeVariation = new TimeSpan();
            TimeSpan.TryParse(GetConfigValue("StartInTimeVariation_Reporting"), out _StartInTimeVariation);
            TimeSpan _ReportingVariation = new TimeSpan();
            TimeSpan.TryParse(GetConfigValue("ReportingVariation"), out _ReportingVariation);

            string _needOutTime = GetStudentAttend_GeneralConfigValue("Need OutTime");

            if (_needOutTime == "YES")
            {
                _endIntime = _endOuttime;
            }

            if (_ntime > _startIntime && _ntime < _endIntime)  // failure should not be reported
            {
                if (!IsDateHoliday(0, _DateTime.Date))
                {
                    if (laststatus == "Working")
                    {
                        SendSMS_Email_RF_Report(_RFReaderID, _DateTime);
                    }
                    else
                    {
                        TimeSpan _CurrentTimespan=_DateTime.TimeOfDay;
                        TimeSpan _StartIntime_AfterVariation = _startIntime.Add(_StartInTimeVariation);
                        if (_CurrentTimespan > (_StartIntime_AfterVariation - _ReportingVariation) && _CurrentTimespan < (_StartIntime_AfterVariation + _ReportingVariation))
                        {
                            SendSMS_Email_RF_Report(_RFReaderID, _DateTime);
                        }
                        else if (_CurrentTimespan > (_LateIntime - _ReportingVariation) && _CurrentTimespan < (_LateIntime + _ReportingVariation))
                        {
                            SendSMS_Email_RF_Report(_RFReaderID, _DateTime);
                        }

                    }
                
                }

            }

        }

        private string GetConfigValue(string _Type)
        {
            OdbcDataReader newreader1;
            string _value = "";
            string sql = "SELECT tblconfiguration.Value FROM tblconfiguration WHERE tblconfiguration.Name='" + _Type + "'";
            if (m_TransationDb != null)
            {
                newreader1 = m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                newreader1 = m_MysqlDb.ExecuteQuery(sql);
            }
            if (newreader1.HasRows)
            {
                _value = newreader1.GetValue(0).ToString();
            }
            return _value;
        }

        private void SendSMS_Email_RF_Report(string _RFReaderID, DateTime _DateTime)
        {
            string _ReportMessage = "";
            string RfReaderName = GetRFReaderName(_RFReaderID);
            string SchoolName = GetSchoolName();
            if (RfReaderName != "" && SchoolName!="")
            {

                _ReportMessage = "School name: " + SchoolName + ". RF device placed in location: " + RfReaderName + " is found not working on " + General.GerFormatedDatVal(_DateTime) + " " + _DateTime.TimeOfDay + ". Please verify the situation.";
                string reportingnumbers = GetConfigValue("Report_Via_SMS");
                string reportingemailids = GetConfigValue("Report_Via_Email");
                string EmailFromId = GetConfigValue("EmailFromId");
                if (reportingnumbers != "")
                {
                    string []numbers = reportingnumbers.Split(',');
                    for (int i = 0; i < numbers.Length; i++)
                    {
                        AddtoAutoSMS(numbers[i], _ReportMessage,"0");
                    }
                }

                if (reportingemailids != "")
                {
                    string[] emailid = reportingemailids.Split(',');
                    for (int i = 0; i < emailid.Length; i++)
                    {

                        Send_Email(emailid[i],"0",  "Rf Reader Failure in " + SchoolName, _ReportMessage,0);
                    }

                   
                }
               
            
            }
        }

        public void Send_Email(string EmailAddress, string SenderId, string EmailSubject, string EmailBody, int Type)
        {
            string sql = "";
            sql = "Insert into tbl_autoemail(EmailAddress,EmailSubject,EmailBody,TimeTosend,`Type`,`Status`,SenderId) values('" + EmailAddress + "','" + EmailSubject + "','" + EmailBody + "','" + System.DateTime.Now.Date.ToString("s") + "'," + Type + ",0,'" + SenderId + "')";
            if (m_TransationDb != null)
            {
                m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MysqlDb.ExecuteQuery(sql);
            }
        }

        //public void Send_Email(string To, string CC, string Subject, string Body, string Attachment, string _From, out string Message)
        //{

           
        //    Message = "";
        //    try
        //    {
        //        string _Host = GetConfigValue("smtp.Host");
        //        string _Id = GetConfigValue("smtp.Credentials.Id");
        //        string _Password = GetConfigValue("smtp.Credentials.Password");
        //        if (To == "")
        //        {
        //            To = "arun@winceron.com";
        //        }
        //        MailMessage mailMsg = new MailMessage(_From, To);
        //        if (CC != "")
        //        {
        //            string[] str_cc = CC.Split(',');
        //            for (int i = 0; i < str_cc.Length; i++)
        //            {
        //                mailMsg.CC.Add(new MailAddress(str_cc[i]));
        //            }

        //            //mailMsg.CC.Add(new MailAddress("arun@winceron.com"));
        //        }

        //        mailMsg.Subject = Subject;
        //        mailMsg.Body = Body;
        //        //mailMsg.Body = "<html><body>Testing <b>123</b/>....</body></html>";
        //        mailMsg.IsBodyHtml = true;

        //        if (Attachment != "")
        //        {
        //            mailMsg.Attachments.Add(new Attachment(Attachment));
        //        }

        //        //mailMsg.IsBodyHtml = true;

        //        SmtpClient smtp = new SmtpClient();
        //        smtp.Host = _Host;

        //        smtp.Credentials = new System.Net.NetworkCredential(_Id, _Password);

        //        smtp.Send(mailMsg);
        //        Message = "Email has been send successfully";
        //        mailMsg.Attachments.Clear();
        //    }

        //    catch (Exception ex)
        //    {
        //        Message = ex.Message;
        //    }

        //}

        private string GetSchoolName()
        {
            OdbcDataReader newreader1;
            string _value = "";
            string sql = "SELECT SchoolName FROM tblschooldetails";
            if (m_TransationDb != null)
            {
                newreader1 = m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                newreader1 = m_MysqlDb.ExecuteQuery(sql);
            }
            if (newreader1.HasRows)
            {
                _value = newreader1.GetValue(0).ToString();
            }
            return _value;
        }

        private string GetRFReaderName(string _RFReaderID)
        {
            OdbcDataReader newreader1;
            string _value = "";
            string sql = "SELECT tblexternalrfid.Location FROM tblexternalrfid WHERE tblexternalrfid.EquipmentId='" + _RFReaderID + "'";
            if (m_TransationDb != null)
            {
                newreader1 = m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                newreader1 = m_MysqlDb.ExecuteQuery(sql);
            }
            if (newreader1.HasRows)
            {
                _value = newreader1.GetValue(0).ToString();
            }
            return _value;
        }

        public string GetRFReaderType(string _RFReaderID, string _UniqueId, DateTime _DateTime)
        {
            string _returnValue = "";
            string RFIDType = "",RFUser="";
            string sql = "SELECT tblexternalrfid.UseType,tblexternalrfid.User FROM tblexternalrfid WHERE tblexternalrfid.EquipmentId='" + _RFReaderID + "'";
            OdbcDataReader newreader = m_MysqlDb.ExecuteQuery(sql);
            if (newreader.HasRows)
            {
                RFIDType = newreader.GetValue(0).ToString();
                RFUser = newreader.GetValue(1).ToString();
            }

            int UserId = 0;
            bool IsStudent = false;
            int GroupId = 0;

            if (IsUniqueIdIdentified(_UniqueId, out IsStudent, out UserId, out GroupId))
            {
                if (IsStudent && RFUser != "STAFF")
                {
                    string NeedDiffEquipment_INandOut = GetStudentAttend_GeneralConfigValue("Need Diff Equipment for In and Out Time of student");
                    if (NeedDiffEquipment_INandOut == "YES")
                    {
                        if (RFIDType == "0")
                        {
                            _returnValue = "INTIME";
                        }
                        else if (RFIDType == "1")
                        {
                            _returnValue = "OUTTIME";
                        }

                    }
                    else
                    {

                        TimeSpan _ntime = _DateTime.TimeOfDay;

                        TimeSpan _startIntime = TimeSpan.Parse(GetStudentAttend_ConfigValue("Start InTime",GroupId));
                        TimeSpan _endIntime = TimeSpan.Parse(GetStudentAttend_ConfigValue("End InTime", GroupId));
                        TimeSpan _startOuttime = TimeSpan.Parse(GetStudentAttend_ConfigValue("Start OutTime", GroupId));
                        TimeSpan _endOuttime = TimeSpan.Parse(GetStudentAttend_ConfigValue("End OutTime", GroupId));
                        string _needOutTime = GetStudentAttend_ConfigValue("Need OutTime", GroupId);
                        if (_ntime > _startIntime && _ntime < _endIntime)
                        {
                            _returnValue = "INTIME";
                        }
                        else
                        {
                            if (_needOutTime == "YES")
                            {
                                if (_ntime > _startOuttime && _ntime < _endOuttime)
                                {
                                    _returnValue = "OUTTIME";
                                }

                            }
                        }



                    }
                }
                else if( RFUser != "STUDENT")
                {
                    TimeSpan _ntime = _DateTime.TimeOfDay;
                    TimeSpan _startIntime = TimeSpan.Parse(GetStaffAttend_ConfigValue("Start InTime"));
                    TimeSpan _endIntime = TimeSpan.Parse(GetStaffAttend_ConfigValue("End InTime"));
                    TimeSpan _startOuttime = TimeSpan.Parse(GetStaffAttend_ConfigValue("Start OutTime"));
                    TimeSpan _endOuttime = TimeSpan.Parse(GetStaffAttend_ConfigValue("End OutTime"));

                    if (_ntime > _startIntime && _ntime < _endIntime)
                    {
                        _returnValue = "INTIME";
                    }
                    else if (_ntime > _startOuttime && _ntime < _endOuttime)
                    {
                        _returnValue = "OUTTIME";
                    }

                }
            }
            else // Not Identified _niqueid
            {
                if (RFIDType == "0")
                {
                    _returnValue = "INTIME";
                }
                else if (RFIDType == "1")
                {
                    _returnValue = "OUTTIME";
                }

            }

            if (_returnValue == "")
            {
                _returnValue = "UNKNOWN";
            }

            return _returnValue;
        }


        public string GetStaffAttend_ConfigValue(string _Type)
        {
            OdbcDataReader newreader1;
            string _value = "";
            string sql = "SELECT tblstaffattdconfig.Value FROM tblstaffattdconfig WHERE tblstaffattdconfig.Name='" + _Type + "'";
            if (m_TransationDb != null)
            {
                newreader1 = m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                newreader1 = m_MysqlDb.ExecuteQuery(sql);
            }
           
            if (newreader1.HasRows)
            {
                _value = newreader1.GetValue(0).ToString();
            }
            return _value;
        }

        public string GetStudentAttend_ConfigValue(string _Type,int GroupId)
        {
            OdbcDataReader newreader1;
            string _value = "";
            string sql = "SELECT tblstudentattdconfig.Value FROM tblstudentattdconfig WHERE tblstudentattdconfig.Name='" + _Type + "' AND GroupId=" + GroupId;
            if (m_TransationDb != null)
            {
                newreader1 = m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                newreader1 = m_MysqlDb.ExecuteQuery(sql);
            }
            if (newreader1.HasRows)
            {
                _value = newreader1.GetValue(0).ToString();
            }
            return _value;
        }


        public string GetStudentAttend_GeneralConfigValue(string _Type)
        {
            OdbcDataReader newreader1;
            string _value = "";
            string sql = "SELECT tblstudentattdconfig.Value,tblstudentattdconfig.Id FROM tblstudentattdconfig WHERE tblstudentattdconfig.Name='" + _Type + "' ORDER BY tblstudentattdconfig.Id";
            if (m_TransationDb != null)
            {
                newreader1 = m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                newreader1 = m_MysqlDb.ExecuteQuery(sql);
            }
            if (newreader1.HasRows)
            {
                _value = newreader1.GetValue(0).ToString();
            }
            return _value;
        }


        public void RemoveAttendance(string _UniqueId, DateTime _DateTime, string RFReaderType)
        {
            try
            {
                CreateExternalTansationDb();
                int UserId = 0;
                int GroupId = 0;
                bool IsStudent = false;
                if (IsUniqueIdIdentified(_UniqueId, out IsStudent, out UserId, out GroupId)) // Student Attendance Code
                {
                    if (IsStudent)
                    {
                        RemoveAttendanceForstudent(UserId, _DateTime, RFReaderType, GroupId);
                    }
                    else // Staff Attendance Code
                    {
                       // MarkAttendanceForStaff(UserId, _DateTime, RFReaderType);

                    }

                }
                else // Unknown _UniqueId
                {


                }

                EndSucessExternalTansationDb();
            }
            catch
            {
                EndFailExternalTansationDb();
            }
        }

        private void RemoveAttendanceForstudent(int StudentId, DateTime _DateTime, string RFReaderType, int GroupId)
        {
            //if (_DateTime.Date == DateTime.Now.Date)
            if (true)
            {
                DateTime _Date = _DateTime.Date;
                TimeSpan _Time = _DateTime.TimeOfDay;
                int ClassAttendanceId = 0;
                int UserId = 1;
                string StandardId = "";
                int YearId = 0;
                string Status = "3", MarkedStatus = "0";
                int ClassId = 0;

                GetStudentDetails(StudentId, out ClassId, out StandardId, out YearId);
                if (AttendanceTables_Exits(StandardId, YearId))
                {
                    //No needed
                    //AttendenceTableManager myAttManagerObj = new AttendenceTableManager(m_TransationDb); 
                    //myAttManagerObj.CreateSingle_StudentAttendanceTables(int.Parse(StandardId), YearId);
                    //myAttManagerObj = null;
                    if (IsAttendanceAlreadyMarked(ClassId, _Date, StandardId, YearId, out MarkedStatus, out ClassAttendanceId))
                    {
                        if (ClassAttendanceId > 0)
                        {
                            UpdateStudentAttendanceDetails(ClassAttendanceId, StudentId.ToString(), 0, ClassId, StandardId, YearId);
                        }
                    }
                    else
                    {
                        
                    }
                }



              

            }
        }





        public string getStudentEnrollNo(string Studentid)
        {
            string _EnrollNo = "";
            string sql = "SELECT tblexternalreff.Id FROM tblexternalreff WHERE tblexternalreff.UserType='STUDENT' AND tblexternalreff.UserId=" + Studentid;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _EnrollNo = m_MyReader.GetValue(0).ToString();
            }
            return _EnrollNo;
        }
    }
}


