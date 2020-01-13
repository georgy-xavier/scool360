using System;
using System.Web;
using System.ComponentModel;
using System.Data.Odbc;
using System.Data;
using WinBase;
using System.Text;
public class ConfigManager : KnowinGen
{
    public MysqlClass m_MysqlDb;
    DataSet _MydataSet;
    private StudentManagerClass MyStudMang;
    private OdbcDataReader m_MyReader = null, m_MyReader1=null, m_MyReader2=null;
    private string m_CinfigMenuStr;   
    public MysqlClass m_TransationDb = null;
    
    public ConfigManager(KnowinGen _Prntobj)
    {
        m_Parent = _Prntobj;
        m_MyODBCConn = m_Parent.ODBCconnection;
        m_UserName = m_Parent.LoginUserName;
        m_MysqlDb = new MysqlClass(this);
        m_CinfigMenuStr = "";
    }
    public ConfigManager(MysqlClass _Msqlobj)
    {
        m_MysqlDb = _Msqlobj;

    }

    ~ConfigManager()
    {
        if (m_MysqlDb != null)
        {
            m_MysqlDb = null;

        } if (m_MyReader != null)
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
    public string GetConfigMangMenuString(int _roleid)
    {
        CLogging logger = CLogging.GetLogObject();
        string _MenuStr;
        if (m_CinfigMenuStr == "")
        {
            _MenuStr = "<ul><li><a href=\"ConfigurationHome.aspx\">Home</a></li>";
            logger.LogToFile("GetConfigMangMenuString", "user is sending Request to select menu tblaction ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            string sql = "SELECT DISTINCT tblaction.MenuName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND ((tblroleactionmap.ModuleId=12 AND tblaction.ActionType='Link') OR tblaction.ActionType='ConfiLink')";
            logger.LogToFile("GetConfigMangMenuString", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                logger.LogToFile("GetConfigMangMenuString", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                while (m_MyReader.Read())
                {
                    _MenuStr = _MenuStr + "<li><a href=\"" + m_MyReader.GetValue(1).ToString() + "\">" + m_MyReader.GetValue(0).ToString() + "</a></li>";
                }
            }
            _MenuStr = _MenuStr + "</ul>";
            logger.LogToFile("GetConfigMangMenuString", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Close();
            m_CinfigMenuStr = _MenuStr;
            logger.LogToFile("GetConfigMangMenuString", " exits from module  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        }
        else
        {
            _MenuStr = m_CinfigMenuStr;
        }
        logger.LogToFile("GetConfigMangMenuString", "Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _MenuStr;
    }

    public void CreateSubject(string subName, string SubCode,int _SubType)
    {
        CLogging logger = CLogging.GetLogObject();
        try
        {
            logger.LogToFile("CreateSubjet", "user is sending Request to select all from tblsubjects ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            logger.LogToFile("CreateSubjet", "user is sending Request to insert into tblsubjects ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            string sql = "INSERT INTO tblsubjects(subject_name,SubjectCode,sub_Catagory) VALUES ('" + subName + "','" + SubCode + "',"+_SubType+")";
            logger.LogToFile("CraeteSubject", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            logger.LogToFile("CraeteSubject", "Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Close();
        }
        catch (Exception e)
        {
            string _message;
            _message = e.Message;
            logger.LogToFile("GetConfigMangMenuString", "throws Error" + _message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
        }
    }

    public void CreateCombSubject(string _SubName, string _SubCode, int _SubType, string _SubjectId)
    {
        int SubId = 0;
        string[] Subjects = _SubjectId.Split('\\');
        string sql = "INSERT INTO tblsubjects(subject_name,SubjectCode,sub_Catagory,Combined) VALUES ('" + _SubName + "','" + _SubCode + "'," + _SubType + ",1)";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        sql = "select Id from tblsubjects where subject_name='" + _SubName + "' and SubjectCode='"+_SubCode+"'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            SubId = int.Parse(m_MyReader.GetValue(0).ToString()); 
        }
        if (SubId != 0)
        {
            foreach(string Sub in Subjects)
            {
                sql = "insert into tblcombinedsubjects (CombSubId,SubId) values(" + SubId + "," + int.Parse(Sub) + ")";
                m_MysqlDb.ExecuteQuery(sql);
            }
        }
    }
    //
    public void creategroup(string groupname)
    {
        string sqlcreategroup = "insert into tbltime_subgroup(Name) values('" + groupname + "')";
        m_MysqlDb.ExecuteQuery(sqlcreategroup);
    }
    public void removegroup(int grpid)
    {
        string sqlcreategroup = "delete from tbltime_subgroup where Id=" + grpid + " ";
        m_MysqlDb.ExecuteQuery(sqlcreategroup);
    }
    public bool DeleteSubjectById(int _subID, out string _message)
    {
        CLogging logger = CLogging.GetLogObject();
        logger.LogToFile("DeleteSubjectById", "user is sending Request to select SubjectId from tblclasssubmap ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "select SubjectId from tblclasssubmap where SubjectId=" + _subID;
        logger.LogToFile("DeleteSubjectById", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        m_MyReader.Read();
        if (m_MyReader.HasRows)
        {
            logger.LogToFile("DeleteSubjectById", "Reading Success ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            _message = "Subject is assigned to some Class";
            logger.LogToFile("DeleteSubjectById", "returning false ", 'I', CLogging.PriorityEnum.LEVEL_IMPORTANT, LoginUserName);
            return false;
        }
        else
        {
            sql = "select SubId from tblcombinedsubjects where SubId=" + _subID;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _message = "Subject is combined with other. Delete the combined subject first";
                return false;
            }
            else
            {
                logger.LogToFile("DeleteSubjectById", "user is sending Request to delete a tblsubject", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                sql = "Delete From tblsubjects where Id=" + _subID;
                logger.LogToFile("DeleteSubjectById", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                _message = "Subject is deleted";
                logger.LogToFile("DeleteSubjectById", "returning true and exists from module ", 'I', CLogging.PriorityEnum.LEVEL_IMPORTANT, LoginUserName);
                return true;
            }
          
        }
       
    }

    public bool CreateBatch(int _NewBatchId, string _startdate, string _enddate, int _curntbatchid, out string _message)
    {
      
        bool _valid;

        int id = 0;
        string sql = "";
        sql = "select tblstudentclassmap_history.StudentId FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id inner join tblbatch on tblbatch.Status=1  where tblstudent.Status=1 AND tblstudentclassmap_history.BatchId=tblbatch.LastbatchId AND tblstudent.Id NOt IN (Select tblstudentclassmap.StudentId from tblstudentclassmap where tblstudentclassmap.BatchId=" + _curntbatchid + " )";

        _MydataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        MyStudMang = new StudentManagerClass(this);

        if (_MydataSet.Tables[0].Rows.Count != 0)
        {
            

            foreach (DataRow dr in _MydataSet.Tables[0].Rows)
            {
                id = int.Parse(dr[0].ToString());
                MyStudMang.MoveStudentToHistory(id);
            }
            _valid = false;
            _message = "Students moved to history";
            return _valid;
        }
        else
        {


            try
            {

                DateTime _stDate = General.GetDateTimeFromText(_startdate);
                DateTime _endDate = General.GetDateTimeFromText(_enddate);
                if (_stDate < _endDate)
                {

                    CreateTansationDb();

                    StoreIncidentBatchCalculations(_curntbatchid, _NewBatchId);
                    StoreAttendanceDetails(_curntbatchid);
                    StoreCCEReportDetails(_curntbatchid);

                    RemoveInactiveStudent(_curntbatchid);
                    //RemoveInactiveBillData();
                    ExecuteUpdateHistory(_curntbatchid, _NewBatchId, _stDate, _endDate);

                    AttendenceTableManager MyAttdTables = new AttendenceTableManager(m_TransationDb);
                    MyAttdTables.RemoveOldStudentAttendanceTables(_curntbatchid);
                    MyAttdTables.CreateStudentAttendanceTables(_NewBatchId);

                    sql = "UPDATE tblbatch SET StartDate= '" + _stDate.Date.ToString("s") + "', EndDate = '" + _endDate.Date.ToString("s") + "', Created =1, Status =1  WHERE Id =" + _NewBatchId;

                    m_TransationDb.TransExecuteQuery(sql);

                    sql = "UPDATE tblbatch SET Status=0  WHERE Id =" + _curntbatchid;

                    m_TransationDb.TransExecuteQuery(sql);

                    EndSucessTansationDb();
                    _valid = true;
                    _message = "";
                }
                else
                {
                    _valid = false;
                    _message = "Start Date should be less than end date";
                }

            }
            catch (Exception e)
            {
                EndFailTansationDb();
                _valid = false;
                _message = e.Message;

            }
            return _valid;
        }
            
        
    }

    private void StoreCCEReportDetails(int _CurrentBatchid)
    {   
        // Moving tblcce_mark to history table
        string sql = "INSERT INTO tblcce_mark_history (BatchId,StudentId,SubjectId,Col1,Col2,Col3,Col4,Col5,Col6,Col7,Col8,Col9,Col10) SELECT " + _CurrentBatchid + ",StudentId,SubjectId,Col1,Col2,Col3,Col4,Col5,Col6,Col7,Col8,Col9,Col10 FROM tblcce_mark";
        m_TransationDb.ExecuteQuery(sql);

        // Moving tblcce_result to history table
        sql = "INSERT INTO tblcce_result_history (BatchId,StudentId,SubjectId,RCol1,RCol2,RCol3,RCol4,RCol5,RCol6,RCol7,RCol8,RCol9,RCol10,RCol11,RCol12,RCol13,RCol14,RCol15,RCol16,RCol17,RCol18,RCol19,RCol20,RCol21,RCol22,RCol23,RCol24,RCol25) SELECT " + _CurrentBatchid + ",StudentId,SubjectId,RCol1,RCol2,RCol3,RCol4,RCol5,RCol6,RCol7,RCol8,RCol9,RCol10,RCol11,RCol12,RCol13,RCol14,RCol15,RCol16,RCol17,RCol18,RCol19,RCol20,RCol21,RCol22,RCol23,RCol24,RCol25 FROM tblcce_result";
        m_TransationDb.ExecuteQuery(sql);

        //moving tblcce_descriptive to tblcce_descriptive_history mani updated on 21.08.2014
        sql = "INSERT INTO tblcce_descriptive_history (StudentId,SubjectId,BatchId,SkillId,PartId,DescriptiveIndicator,Term1,Term2,Term3) select StudentId,SubjectId," + _CurrentBatchid + ",SkillId,PartId,DescriptiveIndicator,Term1,Term2,Term3 from tblcce_descriptive";
        m_TransationDb.ExecuteQuery(sql);

        sql = "TRUNCATE TABLE tblcce_mark";
        m_TransationDb.ExecuteQuery(sql);

        sql = "TRUNCATE TABLE tblcce_result";
        m_TransationDb.ExecuteQuery(sql);

        sql = "TRUNCATE TABLE tblcce_descriptive";
        m_TransationDb.ExecuteQuery(sql);
    }

    public void StoreAttendanceDetails(int _CurrentBatchid)
    {
        int Holidays = 0, Presentdays = 0, Absentdays = 0, Halfdays = 0, WorkingDays = 0; 
        int StudentId = 0;
        string ClassId="",StandardId="";
        string sql = "SELECT tblclass.Id,tblclass.Standard from tblclass where tblclass.Status=1 ORDER BY tblclass.Standard,tblclass.ClassName";
        DataSet MydataSet = m_TransationDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr in MydataSet.Tables[0].Rows)
            {
                Holidays = 0;
                ClassId = dr[0].ToString();
                StandardId= dr[1].ToString();
                Holidays = GetNumberOfHolidaysForClass(ClassId);

                sql = "SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudent.Status=1 AND tblstudentclassmap.ClassId=" + ClassId + " AND tblstudentclassmap.BatchId=" + _CurrentBatchid;
                m_MyReader1 = m_TransationDb.ExecuteQuery(sql);
                if (m_MyReader1.HasRows)
                {
                    while (m_MyReader1.Read())
                    {
                        StudentId = 0;
                        Presentdays = 0; Absentdays = 0; Halfdays = 0; WorkingDays = 0; 
                        int.TryParse(m_MyReader1.GetValue(0).ToString(), out StudentId);
                        if (StudentId > 0 && ClassId!="" && StandardId!="")
                        {
                            if (AttendanceTables_Exits(StandardId, _CurrentBatchid))
                            {
                                WorkingDays = GetNumberOfworkingDayForClassFromNewAttdance(StudentId, _CurrentBatchid, StandardId);
                                Presentdays = GetNumberOfFullDayDayForTheSutdentFromNewAttdance(StudentId, _CurrentBatchid, StandardId);
                                Absentdays = GetNumberOfAbsentDayForTheSutdentFromNewAttdance(StudentId, _CurrentBatchid, StandardId);
                                Halfdays = GetNumberOfHalfDayDayForTheSutdentFromNewAttdance(StudentId, _CurrentBatchid, StandardId); 
                            }
                            InsertPreviousBtach_AttdanceDetails(StudentId, ClassId, _CurrentBatchid, WorkingDays, Presentdays, Absentdays, Halfdays, Holidays);
                        }
                    }
                }
            }

        }
    }

    private void InsertPreviousBtach_AttdanceDetails(int StudentId, string ClassId, int _CurrentBatchid, int WorkingDays, int Presentdays, int Absentdays, int Halfdays, int Holidays)
    {
        string sql = "insert into tblattdperivousbatch(StudentId,ClassId,BatchId,WorkingDays,PresentDays,AbsentDays,HalfDays,Holidays) values("+StudentId+","+ClassId+","+_CurrentBatchid+","+WorkingDays+","+Presentdays+","+Absentdays+","+Halfdays+","+Holidays+")";
        m_TransationDb.ExecuteQuery(sql);
    }


    public bool AttendanceTables_Exits(string StdId, int YearId)
    {
        bool valid = false;
        string _tablename = "";
        string End_Region = "std" + StdId + "yr" + YearId;
        string Sql = "show tables like 'tblattdcls_" + End_Region + "'";
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


    private int GetNumberOfHolidaysForClass(string _ClsId)
    {
        int _TotalHolidaysDay = 0;
        //Sql = "select count(tblholiday.Id) from tblholiday where tblholiday.Id in (select  tblholiday.Id from tblholiday WHERE tblholiday.`Type`='class' AND tblholiday.Class_Id=" + _ClsId + ") OR  tblholiday.Id in (select  tblholiday.Id from tblholiday inner join tblclass on tblclass.ParentGroupID = tblholiday.GroupId AND tblholiday.`Type`='all'  WHERE  tblclass.Id=" + _ClsId + ")";
        string Sql = "select count(tblholiday.Id) from tblholiday where tblholiday.Id in (select  tblholiday.Id from tblholiday  WHERE tblholiday.`Type`='class' AND tblholiday.Class_Id=" + _ClsId + " AND tblholiday.Class_Id<>0) OR  tblholiday.Id in (select  tblholiday.Id from tblholiday WHERE (( tblholiday.`Type`='all' OR tblholiday.`Type`='class') AND tblholiday.Class_Id=0 ))";
        m_MyReader = m_TransationDb.ExecuteQuery(Sql);
        if (m_MyReader.HasRows)
        {
            _TotalHolidaysDay = int.Parse(m_MyReader.GetValue(0).ToString());

        }
        return _TotalHolidaysDay;
    }


    private int GetNumberOfHalfDayDayForTheSutdentFromNewAttdance(int _studid, int _batchid, string _StandardId)
    {
        int _TotalhalfDay = 0;
        string Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + _StandardId + "yr" + _batchid + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + _batchid + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studid + " AND (t2.PresentStatus=1 OR t2.PresentStatus=2)";
        m_MyReader = m_TransationDb.ExecuteQuery(Sql);
        if (m_MyReader.HasRows)
        {
            _TotalhalfDay = int.Parse(m_MyReader.GetValue(0).ToString());

        }
        return _TotalhalfDay;
    }

    private int GetNumberOfFullDayDayForTheSutdentFromNewAttdance(int _studid, int _batchid, string _StandardId)
    {
        int _TotalFullDay = 0;
        string Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + _StandardId + "yr" + _batchid + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + _batchid + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studid + " AND t2.PresentStatus=3";
        m_MyReader = m_TransationDb.ExecuteQuery(Sql);
        if (m_MyReader.HasRows)
        {
            _TotalFullDay = int.Parse(m_MyReader.GetValue(0).ToString());

        }
        return _TotalFullDay;
    }

    private int GetNumberOfAbsentDayForTheSutdentFromNewAttdance(int _studid, int _batchid, string _StandardId)
    {
        int _TotalAbsentDay = 0;
        string Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + _StandardId + "yr" + _batchid + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + _batchid + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studid + " AND t2.PresentStatus=0";
        m_MyReader = m_TransationDb.ExecuteQuery(Sql);
        if (m_MyReader.HasRows)
        {
            _TotalAbsentDay = int.Parse(m_MyReader.GetValue(0).ToString());

        }
        return _TotalAbsentDay;
    }

    private int GetNumberOfworkingDayForClassFromNewAttdance(int _studid, int _batchid, string _StandardId)
    {
        int _TotalworkingDay = 0;
        string Sql = "SELECT Count(t1.Id) FROM tblattdcls_std" + _StandardId + "yr" + _batchid + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + _batchid + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.StudentId=" + _studid;
        m_MyReader = m_TransationDb.ExecuteQuery(Sql);
        if (m_MyReader.HasRows)
        {
            _TotalworkingDay = int.Parse(m_MyReader.GetValue(0).ToString());

        }
        return _TotalworkingDay;
    }



    public void StoreIncidentBatchCalculations(int _CurrentBatchid, int _NewBatchId)
    {
        StoreStudent_IncedenceCalculation(_CurrentBatchid, _NewBatchId);
        StoreStaff_IncedenceCalculation(_CurrentBatchid, _NewBatchId);
    }

    private void StoreStaff_IncedenceCalculation(int _CurrentBatchid, int _NewBatchId)
    {
        string StaffId = "";
        int CurrentBatch_StaffAVG = 0, TotalStaff_Count = 0,TotalStaffPoints=0;
        int CurrentBatch_StaffRating = 0, Current_Batch_StaffPoints = 0;
        TotalStaff_Count = GetStaffCount_CurrentBatch(_CurrentBatchid);
        TotalStaffPoints = GetTotalCurrentBatchStaffPoints(_CurrentBatchid);
        if (TotalStaff_Count > 0)
        {
            CurrentBatch_StaffAVG = TotalStaffPoints / TotalStaff_Count;
            string sql = "SELECT t.`Id` FROM tbluser t  inner join tblrole r on t.`RoleId`=r.`Id`  where t.`Status`=1 AND r.`Type`='Staff'";
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    StaffId = "";
                    CurrentBatch_StaffRating = 0;
                    StaffId = m_MyReader.GetValue(0).ToString();
                    Current_Batch_StaffPoints = GetTotal_StaffIncidentPoints(StaffId, _CurrentBatchid);
                    CurrentBatch_StaffRating = Current_Batch_StaffPoints - CurrentBatch_StaffAVG;
                    Insert_StaffIncidentCalculations(StaffId, _CurrentBatchid, Current_Batch_StaffPoints, CurrentBatch_StaffRating);
                }
            }
        }
    }

    private void Insert_StaffIncidentCalculations(string StaffId, int _CurrentBatchid, int Current_BatchPoints, int CurrentBatchRating)
    {
        string sql = "insert into tblincidentstaffcalculation(StaffId,PBP,PBR,BatchId) values(" + StaffId + "," + Current_BatchPoints + "," + CurrentBatchRating + "," + _CurrentBatchid + ")";
        m_TransationDb.ExecuteQuery(sql);
    }

    private int GetTotal_StaffIncidentPoints(string StaffId, int _CurrentBatchid)
    {
        int Totalpoints = 0;
        string sql = "SELECT SUM(tblincedent.`Point`) FROM tblincedent WHERE tblincedent.UserType='staff' AND tblincedent.Status='Approved' AND tblincedent.BatchId=" + _CurrentBatchid + " AND tblincedent.AssoUser=" + StaffId;
        m_MyReader1 = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader1.HasRows)
        {
            int.TryParse(m_MyReader1.GetValue(0).ToString(), out Totalpoints);
        }
        return Totalpoints;
    }

    private int GetTotalCurrentBatchStaffPoints(int _CurrentBatchid)
    {
        int Totalpoints = 0;
        string sql = "SELECT SUM(tblincedent.`Point`) FROM tblincedent WHERE LOWER(tblincedent.UserType)='staff'  AND tblincedent.Status='Approved' AND tblincedent.BatchId=" + _CurrentBatchid;
        m_MyReader1 = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader1.HasRows)
        {
            int.TryParse(m_MyReader1.GetValue(0).ToString(), out Totalpoints);
        }
        return Totalpoints;
    }

    private int GetStaffCount_CurrentBatch(int _CurrentBatchid)
    {
        int count = 0;
        string sql = "SELECT COUNT( t.`Id`) FROM tbluser t  inner join tblrole r on t.`RoleId`=r.`Id`  where t.`Status`=1 AND r.`Type`='Staff'";
        m_MyReader1 = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader1.HasRows)
        {
            int.TryParse(m_MyReader1.GetValue(0).ToString(), out count);
        }
        return count;
    }

    private void StoreStudent_IncedenceCalculation(int _CurrentBatchid, int _NewBatchId)
    {
        string ClassId = "", StudentId = "";
        int Current_Batch_Class_AVG = 0, Total_ClassPoints = 0, Total_No_Students = 0;
        int Current_Batch_StudentPoints = 0, CurrentBatch_StudentRating = 0;
        string sql = "SELECT tblclass.Id,tblclass.ClassName from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 ORDER BY tblstandard.Id,tblclass.ClassName";
        DataSet MydataSet = m_TransationDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr in MydataSet.Tables[0].Rows)
            {
                ClassId = "";
                StudentId = "";
                Current_Batch_Class_AVG = 0;
                CurrentBatch_StudentRating = 0;
                Total_ClassPoints = 0;
                ClassId = dr[0].ToString();
                Total_ClassPoints = GetTotal_ClassIncidentPoints(ClassId, _CurrentBatchid);
                Total_No_Students = GetTotal_StudentsInClass(ClassId, _CurrentBatchid);
                if (Total_No_Students > 0)
                {
                    Current_Batch_Class_AVG = Total_ClassPoints / Total_No_Students;
                    sql = "SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudent.Status=1 AND tblstudentclassmap.ClassId=" + ClassId + " AND tblstudentclassmap.BatchId=" + _CurrentBatchid;
                    m_MyReader = m_TransationDb.ExecuteQuery(sql);
                    if (m_MyReader.HasRows)
                    {
                        while (m_MyReader.Read())
                        {
                            StudentId = "";
                            CurrentBatch_StudentRating = 0;
                            StudentId = m_MyReader.GetValue(0).ToString();
                            Current_Batch_StudentPoints = GetTotal_StudentIncidentPoints(StudentId, ClassId, _CurrentBatchid);
                            CurrentBatch_StudentRating = Current_Batch_StudentPoints - Current_Batch_Class_AVG;
                            Insert_IncidentCalculations(StudentId, _CurrentBatchid, Current_Batch_StudentPoints, CurrentBatch_StudentRating, ClassId);
                        }
                    }
                }
            }

        }
    }

    private void Insert_IncidentCalculations(string StudentId, int _CurrentBatchid, int Current_Batch_StudentPoints, int CurrentBatch_StudentRating, string ClassId)
    {
        string sql = "insert into tblincidentcalculation(StudentId,PBP,PBR,BatchId,OldClassId) values(" + StudentId + "," + Current_Batch_StudentPoints + "," + CurrentBatch_StudentRating + "," + _CurrentBatchid + "," + ClassId + ")";
        m_TransationDb.ExecuteQuery(sql);
    }



    private int GetTotal_StudentIncidentPoints(string StudentId, string ClassId, int _CurrentBatchid)
    {
        int Totalpoints = 0;
        string sql = "SELECT SUM(tblincedent.`Point`) FROM tblincedent WHERE tblincedent.UserType='student'  AND tblincedent.Status='Approved' AND tblincedent.ClassId=" + ClassId + " AND tblincedent.BatchId=" + _CurrentBatchid + " AND tblincedent.AssoUser=" + StudentId;
        m_MyReader1 = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader1.HasRows)
        {
            int.TryParse(m_MyReader1.GetValue(0).ToString(), out Totalpoints);
        }
        return Totalpoints;
    }

    private int GetTotal_StudentsInClass(string ClassId,int CurrentBatchId)
    {
        int count = 0;
        string sql = "SELECT Count(tblstudentclassmap.StudentId) FROM tblstudentclassmap INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudent.Status=1 AND tblstudentclassmap.ClassId=" + ClassId + " AND tblstudentclassmap.BatchId=" + CurrentBatchId;
        m_MyReader1 = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader1.HasRows)
        {
            int.TryParse(m_MyReader1.GetValue(0).ToString(), out count);
        }
        return count;
    }

    private int GetTotal_ClassIncidentPoints(string ClassId, int _CurrentBatchid)
    {
        int Totalpoints = 0;
        string sql = "SELECT SUM(tblincedent.`Point`) FROM tblincedent WHERE tblincedent.UserType='student' AND tblincedent.Status='Approved' AND tblincedent.ClassId=" + ClassId + " AND tblincedent.BatchId=" + _CurrentBatchid;
        m_MyReader1 = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader1.HasRows)
        {
            int.TryParse(m_MyReader1.GetValue(0).ToString(), out Totalpoints);
        }
        return Totalpoints;
    }

    //private void RemoveInactiveBillData()
    //{
    //    string sql = "Delete from tblbillcancel";
    //    m_TransationDb.TransExecuteQuery(sql);
    //    //sql = "Delete from tblbillclearence";
    //    //m_TransationDb.TransExecuteQuery(sql);
    //}

    private void RemoveInactiveStudent(int _curntbatchid)
    {
        //string sql = "Delete from tbltransaction where tbltransaction.UserId in(select tblstudent.Id FROM tblstudent where tblstudent.`Status` IN (0,3))";
        //m_TransationDb.TransExecuteQuery(sql);
        //sql = "Delete from tblfeebill where  tblfeebill.StudentID IN (select tblstudent.Id FROM tblstudent where tblstudent.`Status` in (0,3))";
        //m_TransationDb.TransExecuteQuery(sql);
        string sql = "Delete from tblstudent where tblstudent.`Status` in (0,3)";
        m_TransationDb.TransExecuteQuery(sql);

    }

    private void ExecuteUpdateHistory(int _curntbatchid, int _NewBatchId, DateTime _new_Batch_sdate, DateTime _new_Batch_edate)
    {
        
        if(HaveModule(1))
        {
            ArchiveFeeData(_curntbatchid, _NewBatchId);
        }
        if (HaveModule(3))
        {
            ArchiveExamData();
        }
        if (HaveModule(21))
        {
            ArchieveAttendanceDatas(_curntbatchid, _new_Batch_sdate, _new_Batch_edate);
        }
        if (HaveModule(20))
        {
            ArchiveIncidentData();
        }
        ExecuteBasicArchive();

        ChangeAllNewAdmision();
        RemoveStudents();
    }

    private void ArchiveIncidentData()
    {
        string sql = "Insert into tblincedent_history select * from tblincedent where tblincedent.Status='Approved'";
        
        m_TransationDb.TransExecuteQuery(sql);
        sql = " Delete from tblincedent";
        m_TransationDb.TransExecuteQuery(sql);
        sql = "Delete from tblincidentqueue";
        m_TransationDb.TransExecuteQuery(sql);



        
    }

    private void RemoveStudents()
    {
        string sql = "DELETE FROM  tblstudent WHERE tblstudent.`Status`=0";
        m_TransationDb.ExecuteQuery(sql);
    }

    private void ChangeAllNewAdmision()
    {
        string sql = "update tblstudent set tblstudent.AdmissionTypeId=1";
        m_TransationDb.ExecuteQuery(sql);
    }

    private void ArchieveAttendanceDatas(int _curntbatchid, DateTime _new_Batch_sdate, DateTime _new_Batch_edate)
    {
        deleteAll_CurrentBatchAttendanceDetails();

        Insert_Holiday_DatasForNewBatch(_new_Batch_sdate, _new_Batch_edate);
    }

    //private void BackupStaffAttendanceDatas(int _batchId, DateTime _startDate, DateTime _endDate)
    //{
        
    //     String sql;
    //     int _workingdays, _staffid, _absentdays, _presentdays,  _holidays;
    //     double _percentage;
    //     DataSet staff;
             
    //             _workingdays = get_NoOf_WorkingDaysForThePeriod_staff(_startDate, _endDate);
    //             _holidays = get_NoOf_HoliDaysForThePeriod_satff(_startDate, _endDate);

              
    //             sql = "select tbluser.Id,  tbluser.SurName  from tbluser  inner join tblrole on tblrole.Id= tbluser.RoleId where tblrole.Type='staff' and tbluser.Status=1";
    //             staff = m_TransationDb.ExecuteQueryReturnDataSet(sql);
    //             if (staff.Tables != null && staff.Tables[0].Rows.Count > 0)
    //             {
    //                 foreach (DataRow dr in staff.Tables[0].Rows)
    //                 {
    //                     _staffid = int.Parse(dr[0].ToString());
    //                     _absentdays = get_NoOf_AbsentDayForTheperiod_staff(_staffid, _startDate, _endDate);
    //                     _presentdays = _workingdays - _absentdays;
    //                     _percentage = (double)_presentdays * 100;
    //                     if (_percentage != 0)
    //                         _percentage = _percentage / (double)_workingdays;

    //                     insert_ToattendanceHistory_Staff(_staffid, _workingdays, _holidays, _presentdays, _absentdays, _percentage, _batchId);               
    //                  }
    //             } 
        
    //}

    private void insert_ToattendanceHistory_Staff(int _staffid, int _workingdays, int _holidays, int _presentdays, int _absentdays, double _percentage, int _BatchId)
    {
        string sql = "insert into tblattendancehistorystaff(tblattendancehistorystaff.staffId, tblattendancehistorystaff.workingDays, tblattendancehistorystaff.presentDays, tblattendancehistorystaff.absentDays, tblattendancehistorystaff.holidays, tblattendancehistorystaff.percentage,tblattendancehistorystaff.BatchId) values(" + _staffid + "," + _workingdays + "," + _presentdays + "," + _absentdays + "," + _holidays + "," + _percentage + ","+_BatchId+")";
        m_TransationDb.ExecuteQuery(sql);
    }

    private int get_NoOf_AbsentDayForTheperiod_staff(int _staffid, DateTime _startDate, DateTime _endDate)
    {
        int _absentdays = 0;
        string sql = "select  count(tblstaffattendance.Id) from tblstaffattendance inner join  tbldate on tblstaffattendance.DayId= tbldate.Id  inner join tblmasterdate on  tblmasterdate.Id= tbldate.DateId  WHERE tbldate.`Status`='staff'  and  tblstaffattendance.StaffId=" + _staffid + "  AND   date(tblmasterdate.date) BETWEEN '" + _startDate.ToString("yyyy-MM-dd") + "' and '" + _endDate.ToString("yyyy-MM-dd") + "'";
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _absentdays = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        return _absentdays;
    }

    private int get_NoOf_WorkingDaysForThePeriod_staff(DateTime _startDate, DateTime _endDate)
    {

        int _workdays = 0;
        string sql = "select  count(DISTINCT(tbldate.Id)) from tbldate inner join tblmasterdate on  tblmasterdate.Id= tbldate.DateId  WHERE  date(tblmasterdate.date) BETWEEN '" + _startDate.ToString("yyyy-MM-dd") + "' and '" + _endDate.ToString("yyyy-MM-dd") + "' and  tbldate.`Status`='staff' and tbldate.classId=0";
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _workdays = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        return _workdays;
    }

    private int get_NoOf_HoliDaysForThePeriod_satff(DateTime _startDate, DateTime _endDate)
    {
        
        int holidays = 0;
        string sql = "select count(distinct tblmasterdate.Id) from tblmasterdate inner join tblholiday on tblmasterdate.Id = tblholiday.dateId where tblholiday.Type='all' and tblholiday.Class_Id=0 and date(tblmasterdate.date) BETWEEN '" + _startDate.ToString("yyyy-MM-dd") + "' and '" + _endDate.ToString("yyyy-MM-dd") + "'";
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            holidays = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        return holidays;
    }



    //private void BackupStudentsAttendanceDatas(int _batchId, DateTime _startDate, DateTime _endDate)
    //{
       
    //    String sql;
    //    int _workingdays, _studentid, _absentdays, _presentdays,_classid,_holidays,_masterId=0;
    //    double _percentage;
    //    DataSet classes;
    //    DataSet students_forclass;
    //    sql = "SELECT tblclass.Id from tblclass WHERE tblclass.Status=1";
    //    classes = m_TransationDb.ExecuteQueryReturnDataSet(sql);
    //    if (classes.Tables != null && classes.Tables[0].Rows.Count > 0)
    //    {
    //        foreach (DataRow dr in classes.Tables[0].Rows)
    //        {
    //            _masterId = GetMaxId();
    //            _classid = int.Parse(dr[0].ToString());
    //            _workingdays = get_NoOf_WorkingDaysForThePeriod(_classid, _startDate, _endDate);
    //            _holidays = get_NoOf_HoliDaysForThePeriod(_classid, _startDate, _endDate);

    //            insert_ToattendanceHistory_Master(_masterId, _batchId, _classid, _workingdays, _holidays);
    //            sql = "SELECT tblstudent.Id from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + _batchId + " AND tblstudentclassmap.ClassId=" + _classid + " Order by tblstudentclassmap.RollNo ASC";
    //            students_forclass = m_TransationDb.ExecuteQueryReturnDataSet(sql);
    //            if (students_forclass.Tables != null && students_forclass.Tables[0].Rows.Count > 0)
    //            {
    //                foreach (DataRow drsid in students_forclass.Tables[0].Rows)
    //                {
    //                    _studentid = int.Parse(drsid[0].ToString());
    //                    _absentdays = get_NoOf_AbsentDayForTheperiod(_studentid, _classid, _startDate, _endDate);
    //                    _presentdays = _workingdays - _absentdays;
    //                    _percentage = (double)_presentdays * 100;
    //                    if (_percentage != 0)
    //                        _percentage = _percentage / (double)_workingdays;

    //                    insert_ToattendanceHistory(_masterId, _studentid, _presentdays, _absentdays, _percentage);
    //                }
    //            }
    //        }
    //    }

       
       
    //}

    private int GetMaxId()
    {
        int Id = 0;
        string sql = "select max(tblattendancehistory_master.Id) from tblattendancehistory_master";
        m_MyReader2 = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader2.HasRows)
        {
            bool valid = int.TryParse(m_MyReader2.GetValue(0).ToString(), out Id);
            Id = Id + 1;
        }
        return Id;
    }
  
    private void insert_ToattendanceHistory(int _masterId, int _studentid, int _presentdays, int _absentdays, double _percentage)
    {
        string sql = "insert into tblattendancehistory(tblattendancehistory.masterId, tblattendancehistory.studentId, tblattendancehistory.presentDays, tblattendancehistory.absentDays, tblattendancehistory.percentage) values(" + _masterId + "," + _studentid + "," + _presentdays + "," + _absentdays + "," + _percentage + ")";
        m_TransationDb.ExecuteQuery(sql);
      
    }

    private int get_NoOf_AbsentDayForTheperiod(int _studentid, int _classid, DateTime _startDate, DateTime _endDate)
    {
        int __AbsentDays = 0;
        string sql = "select count(DISTINCT(tblstudentattendance.Id)) from tblstudentattendance inner join tbldate on tblstudentattendance.DayId= tbldate.Id  inner join tblmasterdate on  tblmasterdate.Id= tbldate.DateId  WHERE  date(tblmasterdate.date) BETWEEN '" + _startDate.ToString("yyyy-MM-dd") + "' and '" + _endDate.ToString("yyyy-MM-dd") + "' and  tbldate.`Status`='class' and tbldate.classId=" + _classid + " and tblstudentattendance.StudentId=" + _studentid;
        m_MyReader2 = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader2.HasRows)
        {
            __AbsentDays = int.Parse(m_MyReader2.GetValue(0).ToString());
        }
        return __AbsentDays;
    }

    private void insert_ToattendanceHistory_Master(int _masterId,int _batchId, int _classid, int _workingdays, int _holidays)
    {                                                                                                                                                                                                                                                                                                    
        string sql = "insert into tblattendancehistory_master(tblattendancehistory_master.Id, tblattendancehistory_master.batchId, tblattendancehistory_master.classId, tblattendancehistory_master.totalWorkingDays, tblattendancehistory_master.totalHolidays) values("+_masterId+","+_batchId+","+_classid+","+_workingdays+","+_holidays+")";
        m_TransationDb.ExecuteQuery(sql);
    }

    private int get_NoOf_HoliDaysForThePeriod(int _classid, DateTime _startDate, DateTime _endDate)
    {
        int holidays = 0;
        string sql = "select count(tblholiday.Id) from tblholiday inner join tblmasterdate on  tblmasterdate.Id= tblholiday.dateId  WHERE  date(tblmasterdate.date) BETWEEN '" + _startDate.ToString("yyyy-MM-dd") + "' and '" + _endDate.ToString("yyyy-MM-dd") + "' and  tblholiday.Id in (select  tblholiday.Id from tblholiday WHERE tblholiday.`Type`='class' AND tblholiday.Class_Id=" + _classid + " AND tblholiday.Class_Id<>0) OR  tblholiday.Id in (select  tblholiday.Id from tblholiday WHERE (( tblholiday.`Type`='all' OR tblholiday.`Type`='class') AND tblholiday.Class_Id=0 ) )";
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            holidays = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        return holidays;
    }

    private int get_NoOf_WorkingDaysForThePeriod(int _classid, DateTime _startDate, DateTime _endDate)
    {
        int _WorkingDays = 0;
        string sql = "select  count(DISTINCT(tbldate.Id)) from tbldate inner join tblmasterdate on  tblmasterdate.Id= tbldate.DateId  WHERE  date(tblmasterdate.date) BETWEEN '" + _startDate.ToString("yyyy-MM-dd") + "' and '" + _endDate.ToString("yyyy-MM-dd") + "' and  tbldate.`Status`='class' and tbldate.classId=" + _classid;
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _WorkingDays = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        return _WorkingDays;
    }
  


    private void deleteAll_CurrentBatchAttendanceDetails()
    {
        String sql;
        sql = "delete from tblholiday";
        m_TransationDb.TransExecuteQuery(sql);
        sql = "delete from tblmasterdate";
        m_TransationDb.TransExecuteQuery(sql);
        sql = "delete from tbldate";
        m_TransationDb.TransExecuteQuery(sql);

    }
 
    private void Insert_Holiday_DatasForNewBatch(DateTime _startDate, DateTime _endDate)
    {

        int _monthid, _dayid, _year;
        string _description;
        int _startyear = _startDate.Year, _startmonth = _startDate.Month,_startday=_startDate.Day;

        int _endyear = _endDate.Year, _endmonth = _endDate.Month, _endday = _endDate.Day; 
       
        int _diff = _endyear - _startyear;
        int _count = 0;
        _year = _startyear;
        string sql;
        while (_count <= _diff)
        {
                DataSet def_holidays;
                sql = "select tbldefaultholidays.monthId, tbldefaultholidays.dayId, tbldefaultholidays.Description from tbldefaultholidays where tbldefaultholidays.Status=1";              
                def_holidays = m_TransationDb.ExecuteQueryReturnDataSet(sql);
                if (def_holidays.Tables != null && def_holidays.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in def_holidays.Tables[0].Rows)
                    {
                        _description = dr[2].ToString();
                        _monthid = int.Parse(dr[0].ToString());
                        _dayid = int.Parse(dr[1].ToString());
                        string _date = _dayid + "/" + _monthid + "/" + _year;                        
                        DateTime _holidaydate = General.GetDateTimeFromText(_date);
                        if (validdate_ToSave(_holidaydate, _startDate,_endDate))
                        {
                            Insert_Holiday_Data(_holidaydate, _description);
                        }
                    }
                }
                _count = _count + 1;
                _year = _year + 1;
        }            
       
    }

    private bool validdate_ToSave(DateTime _holidaydate,DateTime _startdate,DateTime _enddate)
    {
        bool _valid = false;
       
        if ((_holidaydate >= _startdate) && (_holidaydate <= _enddate))
            _valid = true;
        
        return _valid;
    }

    private void Insert_Holiday_Data(DateTime _holidaydate,string _description)
    {
        string sql = "SELECT DISTINCT tblgroup.Id FROM tblgroup";
        DataSet m_groups = m_TransationDb.ExecuteQueryReturnDataSet(sql);

        string Sql;
        int _dateid = GetIdFromMasterdate(_holidaydate);

        if (m_groups.Tables != null && m_groups.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in m_groups.Tables[0].Rows)
            {
                Sql = "insert into tblholiday (`dateId`,`Desc`,`Type`,Class_Id,GroupId) values(" + _dateid + ",'" + _description + "','all',0," + int.Parse(dr[0].ToString()) + ")";
                m_TransationDb.TransExecuteQuery(Sql);
            }
        }

    }
    private void ArchiveExamData()
    {
        string sql = "insert into tblexamschedule_history (tblexamschedule_history.Id, tblexamschedule_history.ExamName, tblexamschedule_history.ExamId, tblexamschedule_history.ClassId, tblexamschedule_history.BatchId, tblexamschedule_history.PeriodId, tblexamschedule_history.`Status`, tblexamschedule_history.ScheduledDate ) Select tblexamschedule.Id, tblexammaster.ExamName, tblexammaster.Id, tblclassexam.ClassId, tblexamschedule.BatchId, tblexamschedule.PeriodId, tblexamschedule.`Status`, tblexamschedule.ScheduledDate from tblexamschedule inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId";
        m_TransationDb.TransExecuteQuery(sql);

        sql = "insert into tblexammark_history (tblexammark_history.ExamSchId, tblexammark_history.MarkColumn, tblexammark_history.SubjectId, tblexammark_history.SubjectName, tblexammark_history.SubjectCode, tblexammark_history.ExamDate, tblexammark_history.TimeSlotId, tblexammark_history.MinMark, tblexammark_history.MaxMark ,tblexammark_history.SubjectOrder) select tblexammark.ExamSchId, tblexammark.MarkColumn, tblexammark.SubjectId, tblsubjects.subject_name, tblsubjects.SubjectCode, tblexammark.ExamDate, tblexammark.TimeSlotId, tblclassexamsubmap.MinMark, tblclassexamsubmap.MaxMark,tblexammark.SubjectOrder from tblexammark inner join tblsubjects on tblsubjects.Id= tblexammark.SubjectId inner join tblexamschedule on tblexamschedule.Id= tblexammark.ExamSchId inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblexamschedule.ClassExamId AND tblexammark.SubjectId= tblclassexamsubmap.SubId ";
        m_TransationDb.TransExecuteQuery(sql);

        sql = "insert into tblstudentmark_history (tblstudentmark_history.ExamSchId, tblstudentmark_history.StudId, tblstudentmark_history.Mark1, tblstudentmark_history.Mark2, tblstudentmark_history.Mark3,tblstudentmark_history.Mark4,tblstudentmark_history.Mark5,tblstudentmark_history.Mark6,tblstudentmark_history.Mark7,tblstudentmark_history.Mark8,tblstudentmark_history.Mark9,tblstudentmark_history.Mark10,tblstudentmark_history.Mark11,tblstudentmark_history.Mark12,tblstudentmark_history.Mark13,tblstudentmark_history.Mark14,tblstudentmark_history.Mark15,tblstudentmark_history.Mark16,tblstudentmark_history.Mark17,tblstudentmark_history.Mark18,tblstudentmark_history.Mark19,tblstudentmark_history.Mark20, tblstudentmark_history.TotalMark, tblstudentmark_history.TotalMax, tblstudentmark_history.`Avg`, tblstudentmark_history.Grade, tblstudentmark_history.Result, tblstudentmark_history.Rank, tblstudentmark_history.Remark) select tblstudentmark.ExamSchId, tblstudentmark.StudId, tblstudentmark.Mark1, tblstudentmark.Mark2, tblstudentmark.Mark3,tblstudentmark.Mark4,tblstudentmark.Mark5,tblstudentmark.Mark6,tblstudentmark.Mark7,tblstudentmark.Mark8,tblstudentmark.Mark9,tblstudentmark.Mark10,tblstudentmark.Mark11,tblstudentmark.Mark12,tblstudentmark.Mark13,tblstudentmark.Mark14,tblstudentmark.Mark15,tblstudentmark.Mark16,tblstudentmark.Mark17,tblstudentmark.Mark18,tblstudentmark.Mark19,tblstudentmark.Mark20, tblstudentmark.TotalMark, tblstudentmark.TotalMax, tblstudentmark.`Avg`, tblstudentmark.Grade, tblstudentmark.Result, tblstudentmark.Rank, tblstudentmark.Remark  from tblstudentmark";
        m_TransationDb.TransExecuteQuery(sql);

        sql = "DELETE FROM tblexamschedule";
        m_TransationDb.TransExecuteQuery(sql);

        sql = "DELETE FROM tblexammark";
        m_TransationDb.TransExecuteQuery(sql);

        sql = "DELETE FROM tblstudentmark";
        m_TransationDb.TransExecuteQuery(sql);
    }

    private void ArchiveFeeData(int _Batchid, int _NewBatchId)
    {
        string sql = "DELETE FROM tbltempstdent WHERE tbltempstdent.JoiningBatch=" + _Batchid;
        m_TransationDb.TransExecuteQuery(sql);

        sql = "insert into tblfeebillhistory (tblfeebillhistory.Id,tblfeebillhistory.StudentID,tblfeebillhistory.TotalAmount, tblfeebillhistory.`Date`, tblfeebillhistory.PaymentMode, tblfeebillhistory.PaymentModeId, tblfeebillhistory.BankName, tblfeebillhistory.BillNo,tblfeebillhistory.UserId, tblfeebillhistory.BatchId, tblfeebillhistory.ClassId, tblfeebillhistory.Canceled , tblfeebillhistory.StudentName , tblfeebillhistory.Counter ,tblfeebillhistory.RegularFee,tblfeebillhistory.TempId,tblfeebillhistory.CreatedDateTime,tblfeebillhistory.OtherReference ) select  tblfeebill.Id ,tblfeebill.StudentID, tblfeebill.TotalAmount, tblfeebill.`Date`, tblfeebill.PaymentMode, tblfeebill.PaymentModeId, tblfeebill.BankName, tblfeebill.BillNo,tblfeebill.UserId, tblfeebill.AccYear, tblfeebill.ClassID , tblfeebill.Canceled , tblfeebill.StudentName , tblfeebill.Counter,tblfeebill.RegularFee,tblfeebill.TempId,tblfeebill.CreatedDateTime,tblfeebill.OtherReference from tblfeebill where tblfeebill.TempId NOT IN (select tbltempstdent.TempId from tbltempstdent)";
        m_TransationDb.TransExecuteQuery(sql);

        sql = "insert into tbltransactionhistory (tbltransactionhistory.TransationId, tbltransactionhistory.UserId,tbltransactionhistory.BillNo, tbltransactionhistory.PaidDate, tbltransactionhistory.Amount, tbltransactionhistory.AccountTo, tbltransactionhistory.AccountFrom, tbltransactionhistory.`Type`, tbltransactionhistory.FeeName, tbltransactionhistory.FeeId,  tbltransactionhistory.PeriodId, tbltransactionhistory.ClassId, tbltransactionhistory.BatchId, tbltransactionhistory.TransType, tbltransactionhistory.Canceled,tbltransactionhistory.StudentName,tbltransactionhistory.CollectedUser , tbltransactionhistory.PeriodName ,tbltransactionhistory.TempId, tbltransactionhistory.RegularFee,tbltransactionhistory.CollectionType) select tbltransaction.TransationId,tbltransaction.UserId, tbltransaction.BillNo, tbltransaction.PaidDate, tbltransaction.Amount, tbltransaction.AccountTo,  tbltransaction.AccountFrom, tbltransaction.`Type`, tbltransaction.FeeName  , tbltransaction.FeeId , tbltransaction.PeriodId, tbltransaction.ClassId , tbltransaction.BatchId, tbltransaction.TransType , tbltransaction.Canceled , tbltransaction.StudentName , tbltransaction.CollectedUser , tbltransaction.PeriodName , tbltransaction.TempId, tbltransaction.RegularFee,tbltransaction.CollectionType from tbltransaction where tbltransaction.TempId NOT IN (select tbltempstdent.TempId from tbltempstdent)";
        m_TransationDb.TransExecuteQuery(sql);


        //string sql = "insert into tblfeebillhistory (tblfeebillhistory.Id ,tblfeebillhistory.StudentID,tblfeebillhistory.TotalAmount, tblfeebillhistory.`Date`, tblfeebillhistory.PaymentMode, tblfeebillhistory.PaymentModeId, tblfeebillhistory.BankName, tblfeebillhistory.BillNo,tblfeebillhistory.UserId, tblfeebillhistory.BatchId, tblfeebillhistory.ClassId, tblfeebillhistory.Canceled , tblfeebillhistory.StudentName ,tblfeebillhistory.Counter ,tblfeebillhistory.RegularFee , tblfeebillhistory.TempId ) select  tblfeebill.Id, tblfeebill.StudentID, tblfeebill.TotalAmount, tblfeebill.`Date`, tblfeebill.PaymentMode, tblfeebill.PaymentModeId, tblfeebill.BankName, tblfeebill.BillNo,tblfeebill.UserId, tblfeebill.AccYear, tblfeebill.ClassID , tblfeebill.Canceled , tblfeebill.StudentName , tblfeebill.Counter , tblfeebill.RegularFee ,tblfeebill.TempId  from tblfeebill";
        //m_TransationDb.TransExecuteQuery(sql);

        //sql = "insert into tbltransactionhistory (tbltransactionhistory.TransationId, tbltransactionhistory.UserId,tbltransactionhistory.BillNo, tbltransactionhistory.PaidDate, tbltransactionhistory.Amount, tbltransactionhistory.AccountTo, tbltransactionhistory.AccountFrom, tbltransactionhistory.`Type`, tbltransactionhistory.FeeName, tbltransactionhistory.FeeId,  tbltransactionhistory.PeriodId, tbltransactionhistory.ClassId, tbltransactionhistory.BatchId, tbltransactionhistory.TransType, tbltransactionhistory.Canceled , tbltransactionhistory.RegularFee , tbltransactionhistory.StudentName ,tbltransactionhistory.CollectedUser,tbltransactionhistory.CollectionType , tbltransactionhistory.PeriodName , tbltransactionhistory.TempId  ) select tbltransaction.TransationId,tbltransaction.UserId, tbltransaction.BillNo, tbltransaction.PaidDate, tbltransaction.Amount, tbltransaction.AccountTo,  tbltransaction.AccountFrom, tbltransaction.`Type`, tbltransaction.FeeName, tblfeeaccount.Id, tbltransaction.PeriodId, tbltransaction.ClassId ,tbltransaction.BatchId, tbltransaction.TransType , tbltransaction.Canceled , tbltransaction.RegularFee , tbltransaction.StudentName , tbltransaction.CollectedUser, tbltransaction.CollectionType , tbltransaction.PeriodName , tbltransaction.TempId from tbltransaction inner join tblfeeschedule on tbltransaction.PaymentElementId= tblfeeschedule.Id inner join tblfeeaccount on tblfeeaccount.Id= tblfeeschedule.FeeId where tbltransaction.TransType=0";
        //m_TransationDb.TransExecuteQuery(sql);

        sql = " delete from tblfeebill where tblfeebill.TempId NOT IN (select tbltempstdent.TempId from tbltempstdent)";
        m_TransationDb.TransExecuteQuery(sql);

        
        sql = " delete from tbltransaction where tbltransaction.TempId NOT IN (select tbltempstdent.TempId from tbltempstdent)";
        m_TransationDb.TransExecuteQuery(sql);


         sql = "DELETE FROM  tblfeestudent where tblfeestudent.SchId IN (select tblfeeschedule.Id from tblfeeschedule where tblfeeschedule.BatchId<>" + _NewBatchId + ") AND (tblfeestudent.`Status`='Paid' OR tblfeestudent.`Status`='fee Exemtion')";
        m_TransationDb.TransExecuteQuery(sql);

        //sql = "DELETE FROM  tblfeeschedule WHERE tblfeeschedule.Id NOT IN (select DISTINCT(tblfeestudent.SchId) from tblfeestudent UNION select distinct tbltransaction.PaymentElementId as SchId from tbltransaction)";
        //m_TransationDb.TransExecuteQuery(sql);

        //sql = "DELETE FROM  tblfeeschedule WHERE tblfeeschedule.Id NOT IN (select distinct tbltransaction.PaymentElementId as SchId from tbltransaction)";
        //m_TransationDb.TransExecuteQuery(sql);
        StringBuilder strActiveScdIds = new StringBuilder("-1");
        sql = "select DISTINCT(tblfeestudent.SchId) from tblfeestudent";
        DataSet MydataSet = m_TransationDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr in MydataSet.Tables[0].Rows)
            {
                strActiveScdIds.Append("," + dr[0].ToString());
            }
        }

        sql = "DELETE FROM  tblfeeschedule WHERE tblfeeschedule.Id NOT IN (" + strActiveScdIds.ToString() + ")";
        m_TransationDb.TransExecuteQuery(sql); 



        //sql = "DELETE FROM tbljoining_transaction WHERE tbljoining_transaction.BillNo IN( SELECT tbljoining_feebill.BillNo FROM tbljoining_feebill WHERE tbljoining_feebill.Batch="+_Batchid+")";
        //m_TransationDb.TransExecuteQuery(sql);

        //sql = "DELETE FROM tbljoining_feebill WHERE tbljoining_feebill.Batch=" + _Batchid + "";
        //m_TransationDb.TransExecuteQuery(sql);

    


    }

    private void ExecuteBasicArchive()
    {
        ClearClassSchedule();
        ArchiveLog();
        ArchiveStudentClassMap();
    }

    private void ArchiveStudentClassMap()
    {
        string sql = "insert into tblstudentclassmap_history (tblstudentclassmap_history.ClassId, tblstudentclassmap_history.RollNo, tblstudentclassmap_history.BatchId, tblstudentclassmap_history.Standard, tblstudentclassmap_history.StudentId) select tblstudentclassmap.ClassId, tblstudentclassmap.RollNo, tblstudentclassmap.BatchId, tblstudentclassmap.Standard, tblstudentclassmap.StudentId from tblstudentclassmap";
        m_TransationDb.TransExecuteQuery(sql);
        sql = "DELETE FROM tblstudentclassmap";
        m_TransationDb.TransExecuteQuery(sql);
    }

    private void ArchiveLog()
    {
        DateTime _Dateperiod = DateTime.Now.AddMonths(-2);
        string sql = "insert into tblloghistory (tblloghistory.`Action`, tblloghistory.Description, tblloghistory.`Level`, tblloghistory.`Time`, tblloghistory.UserName, tblloghistory.UserTypeId) select tbllog.`Action`, tbllog.Description, tbllog.`Level`, tbllog.`Time`, tbllog.UserName, tbllog.UserTypeId from tbllog where tbllog.`Time` < '" + _Dateperiod.Date.ToString("s") + "'";
        m_TransationDb.TransExecuteQuery(sql);
        sql = "DELETE FROM tbllog where tbllog.`Time` < '" + _Dateperiod.Date.ToString("s") + "'";
        m_TransationDb.TransExecuteQuery(sql);
    }

    private void ClearClassSchedule()
    {
        string sql = "DELETE FROM tblclassschedule";
        m_TransationDb.TransExecuteQuery(sql);
    }

    private bool HaveModule(int _moduleid)
    {
        bool _valide;
        string sql = "select Id from tblmodule where Id=" + _moduleid;
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            _valide = true;
        }
        else
        {
            _valide = false;
        }
        m_MyReader.Close();
        return _valide;
    }

    public void AddTONewBatch(int _studid, int _batchid, int _classid,int _OldClass)
    {
        CLogging logger = CLogging.GetLogObject();
        string _std = GetStandatd(_classid);
        logger.LogToFile("AddTONewBatch", "user is sending Request to INSERT INTO tblstudentclassmap", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "INSERT INTO tblstudentclassmap(StudentId,ClassId,Standard,BatchId) VALUES (" + _studid + "," + _classid + ",'" + _std + "'," + _batchid + ")";
        logger.LogToFile("AddTONewBatch", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_TransationDb.ExecuteQuery(sql);
        sql = "update tblstudent set LastClassId=" + _classid + " where Id=" + _studid;
        m_TransationDb.ExecuteQuery(sql);
        logger.LogToFile("AddTONewBatch", " Closing reader and exists ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        UpdateStudentAdmissionType(_studid);
       // ScheduleRollNumber(_classid, _batchid);
        m_MyReader.Close();
        if (_OldClass != 0)// Update scheduleId for Fees
            ReArrangeFeeSchedule(_OldClass.ToString(), _classid.ToString(), _batchid, _studid);

        ScheduleClassWisePendingFees( _studid,  _batchid,  _classid, _OldClass);

    }

    private void ScheduleClassWisePendingFees(int _studid, int _batchid, int _classid, int _OldClass)
    {
        OdbcDataReader MyTempReader = null;
        int NextBatch = _batchid + 1;
        string sql = "select tblfeeschedule.Id,tblfeeschedule.FeeId, tblfeeaccount.AccountName, tblfeeschedule.Amount, date_format(tblfeeschedule.Duedate, '%d-%m-%Y') AS 'Duedate' , date_format(tblfeeschedule.LastDate, '%d-%m-%Y') AS 'LastDate' from tblfeeschedule inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblfeeasso on tblfeeasso.Id = tblfeeaccount.AssociatedId  where (tblfeeschedule.BatchId=" + _batchid + " or tblfeeschedule.BatchId=" + NextBatch + ") and tblfeeschedule.ClassId=" + _classid + " and tblfeeasso.Name='Class' and tblfeeschedule.Id not in(select distinct tblfeestudent.SchId from tblfeestudent where tblfeestudent.StudId=" + _studid + ")";
        MyTempReader = m_TransationDb.ExecuteQuery(sql);
        if (MyTempReader.HasRows)
        {
            while (MyTempReader.Read())
            {
                if (CheckForRuleApplicableToClassAndFee1(_classid.ToString(), int.Parse(MyTempReader.GetValue(1).ToString())))
                {
                    if (CheckRuleIsApplicabletoThisStudent(int.Parse(MyTempReader.GetValue(1).ToString()), _classid, double.Parse(MyTempReader.GetValue(3).ToString()), _studid, _batchid, int.Parse(MyTempReader.GetValue(0).ToString())))
                    {

                    }
                }
                else
                {
                    ScheduleStudFee(_studid, int.Parse(MyTempReader.GetValue(0).ToString()), double.Parse(MyTempReader.GetValue(3).ToString()), "Scheduled");
                }
            }
        }
    }

    public void ScheduleRollNumber(int _Classid, int _Batchid)
    {
        int Rollno = 0;
        string sql = "SELECT tblstudent.Id  from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + _Batchid + " AND tblstudentclassmap.ClassId=" + _Classid + " Order by tblstudent.StudentName ASC";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                Rollno++;
                UpdateRollNumber(_Classid, _Batchid, int.Parse(m_MyReader.GetValue(0).ToString()), Rollno);
            }
        }
    }

    private void UpdateRollNumber(int _classid, int _BatchId, int _StudentId, int _Rollno)
    {
        string sql = "update tblstudentclassmap set RollNo=" + _Rollno + " where StudentId=" + _StudentId + " and ClassId=" + _classid + " and BatchId=" + _BatchId + "";
        m_MysqlDb.ExecuteQuery(sql);
        sql = "update tblstudent set RollNo=" + _Rollno + " where Id=" + _StudentId;
        m_MysqlDb.ExecuteQuery(sql);
    }

    private void UpdateStudentAdmissionType(int _studentid)
    {
        string sql = "update tblstudent set tblstudent.AdmissionTypeId =1 where tblstudent.Id = " + _studentid;
        m_TransationDb.ExecuteQuery(sql);
    }

    public void ReportIncident(int _studid, int _classid, int _batchid, string _status)
    {
        string _Desc = "";
        string ClassName = GetClass(_classid);
        if (_status == "fail")
        {
            _Desc = "The student is failed from " + ClassName + "";
        }
        else
        {
            _Desc = "The student is promoted to " + ClassName + "";
        }

        General _GenObj = new General(m_MysqlDb);
        int _Incedentid = _GenObj.GetTableMaxId("tblview_incident", "Id");
        DateTime _Now = System.DateTime.Now;
        string sql = " insert into tblincedent (Id,Title,Description,IncedentDate,CreatedDate,TypeId,ApproverId,CreatedUserId,UserType,Status,AssoUser)values ("+_Incedentid+",'Promotion','" + _Desc + "','" + _Now.ToString("s") + "','" + _Now.ToString("s") + "',1,0,1,'student','Approved'," + _studid + " )";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    }

    private string GetClass(int _classid)
    {
        string _Class = "";
        string sql = "select tblclass.ClassName from tblclass where tblclass.Id=" + _classid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _Class = m_MyReader.GetValue(0).ToString();
        }
        return _Class;
    }

    private string GetStandatd(int _classid)
    {
        CLogging logger = CLogging.GetLogObject();
        string _std = "";
        string sql = "SELECT Standard FROM tblclass where Id=" + _classid;
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _std = m_MyReader.GetValue(0).ToString();
        }
        m_MyReader.Close();
        return _std;
    }

    public bool HasImage()
    {
        CLogging logger = CLogging.GetLogObject();
       
        bool ImageFlag = false;
        logger.LogToFile("HasImage", "user is sending Request ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        String Sql = "SELECT Logo FROM tblschooldetails WHERE Id=1";
        logger.LogToFile("HasImage", " Executing Query " + Sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
        if (m_MyReader.HasRows)
        {
            logger.LogToFile("HasImage", " Reading Success ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
          
            ImageFlag = true;
        }
        logger.LogToFile("HasImage", " returning ImageFlag and exists ", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
        return ImageFlag;
    }

    public void AddSchoolDetails(string _SchoolName, string _address, string _Syllabus, string _MediumOfIns, string _Desc,string language)
    {
        CLogging logger = CLogging.GetLogObject();
        string Sql;
        logger.LogToFile("AddSchoolDetails", " function call to SchoolDetailsExists ", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
        try
        {
            if (SchoolDetailsExists())
            {
                logger.LogToFile("AddSchoolDetails", "user is sending Request ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                Sql = "UPDATE tblschooldetails SET SchoolName='" + _SchoolName + "',Address='" + _address + "',Syllabus='" + _Syllabus + "',MediumofInstruction='" + _MediumOfIns + "',Disc='" + _Desc + "' WHERE Id=1";
                logger.LogToFile("AddSchoolDetails", " Executing Query " + Sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);


            }
            else
            {
                logger.LogToFile("AddSchoolDetails", "user is sending Request ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                Sql = "INSERT INTO tblschooldetails(SchoolName,Address,Syllabus,MediumofInstruction,Disc) VALUES('" + _SchoolName + "','" + _address + "','" + _Syllabus + "','" + _MediumOfIns + "','" + _Desc + "')";
                logger.LogToFile("AddSchoolDetails", " Executing Query " + Sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            }
            if (NativeLanguageexist())
            {
                logger.LogToFile("UpdateNativeLanguage", "user is sending Request ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                Sql = "UPDATE tblconfiguration SET Value='" + language + "' where Module='SMS' and Name='Native Language'";
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            }
            else
            {
                logger.LogToFile("AddedNativeLanguage", "user is sending Request ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                Sql = "Insert into tblconfiguration(Module, Name, Value) values('SMS','Native Language','" + language + "')";
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            }

        }
        catch (Exception e)
        {
            logger.LogToFile("CreateBatch", "throws Error" + e, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
        }
    }

    private bool NativeLanguageexist()
    {
        string Sql = "select Value from  tblconfiguration where Module='SMS' and Name='Native Language'";
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
        
        if (m_MyReader.HasRows)
        {
            if (String.IsNullOrEmpty(m_MyReader["Value"].ToString())) return false;
            else return true; 
        }
        else return false;
    }

    public bool GetCustomeCCEReportType(ref string type, ref string xmlstr)
    {
        string Sql = "SELECT * from tblconfiguration where Name='CCE_Custome_Report'";
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);

        if (m_MyReader.HasRows)
        {
            if (m_MyReader["Value"] == null || string.IsNullOrEmpty(m_MyReader["Value"].ToString()) || m_MyReader["Value"].ToString().Equals("0")) return false;
            else if (m_MyReader["SubValue"] == null || string.IsNullOrEmpty(m_MyReader["SubValue"].ToString())) return false;
            else
            {
                type = m_MyReader["Value"].ToString();
                xmlstr = m_MyReader["SubValue"].ToString();
                return true;
            }
        }
        else return false;
    }

    private bool SchoolDetailsExists()
    {
        CLogging logger = CLogging.GetLogObject();
        bool HasDetails = false;
        logger.LogToFile("SchoolDetailsExists", "user is sending Request ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string Sql = "SELECT SchoolName FROM tblschooldetails WHERE Id=1";
        logger.LogToFile("SchoolDetailsExists", " Executing Query " + Sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
        if (m_MyReader.HasRows)
        {
            logger.LogToFile("SchoolDetailsExists", " Reading Success ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            HasDetails = true;
        }
        logger.LogToFile("SchoolDetailsExists", " return HasDetails and exists ", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
        return HasDetails;
    }

    public bool HaveStudentsTopromote(int _batchid)
    {
        bool _valide;
        string sql = "select tblstudentclassmap_history.StudentId FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id inner join tblbatch on tblbatch.Status=1  where tblstudent.Status=1 AND tblstudentclassmap_history.BatchId=tblbatch.LastbatchId AND tblstudent.Id NOt IN (Select tblstudentclassmap.StudentId from tblstudentclassmap where tblstudentclassmap.BatchId=" + _batchid + " )";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _valide = true;
        }
        else
        {
            _valide = false;
        }
        m_MyReader.Close();
        return _valide;
    }

  

    public int GetIdFromMasterdate(DateTime _day)
    {
        int _retunDateId = -1;
        int _reDateId = -1;
        string Sql;
        CLogging logger = CLogging.GetLogObject();
        if (CheckDateInDatemast(_day, out _reDateId) == true)
        {
            _retunDateId = int.Parse(_reDateId.ToString());
        }
        else
        {
            try
            {
                Sql = "INSERT INTO tblmasterdate(Date) VALUES('" + _day.ToString("s") + "')";
                if (m_TransationDb == null)

                   m_MysqlDb.ExecuteQuery(Sql);
                else
                    m_TransationDb.TransExecuteQuery(Sql);

                logger.LogToFile("getDateIdFrmMastTbl", " INSERT into date into master table ", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
                Sql = " select tblmasterdate.Id from tblmasterdate where date ='" + _day.ToString("yyyy-MM-dd") + "'";
                if (m_TransationDb == null)
                    m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
                else
                    m_MyReader = m_TransationDb.ExecuteQuery(Sql);

                logger.LogToFile("getDateIdFrmMastTbl", " selecting Id from date master table ", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
                if (m_MyReader.HasRows)
                {
                    _retunDateId = int.Parse(m_MyReader.GetValue(0).ToString());
                }
            }
            catch (Exception e)
            {
                logger.LogToFile("getDateIdFrmMastTbl", " Exception During the Insertion into the masterdate " + e.Message + " ", 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
            }
        }
        return _retunDateId;
    }

    private bool CheckDateInDatemast(DateTime _day, out int _reDateId)
    {
        bool flag = false;
        string Sql;
        _reDateId = 0;
        CLogging logger = CLogging.GetLogObject();
        Sql = " select tblmasterdate.Id from tblmasterdate where tblmasterdate.date='" + _day.ToString("yyyy-MM-dd") + "'";
        if(m_TransationDb==null)
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
        else
            m_MyReader = m_TransationDb.ExecuteQuery(Sql);
        logger.LogToFile("CheckDateInDatemast", " select date from date master ", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
        if (m_MyReader.HasRows)
        {
            flag = true;
            _reDateId = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        return flag;
    }

    public void updateDate(DateTime _day, string _Desc)
    {
        string sql = "";
        int _GetId = GetIdFromMasterdate(_day);
        sql = "update tblholiday set `Desc`='" + _Desc + "' where tblholiday.dateId=" + _GetId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    }

    public int ApproveRegistration(int _RegId)
    {
        string sql, Sql;
        int ParentId = 0, success = 0;
        MysqlClass MysqlTranDb;
        MysqlTranDb = new MysqlClass(this);
        try
        {
            MysqlTranDb.MyBeginTransaction();
            Sql = "SELECT tblregistration.Name, tblregistration.pswd, tblregistration.`E-mail`, tblregistration.SurName FROM tblregistration WHERE tblregistration.Id=" + _RegId + "";
            m_MyReader = MysqlTranDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                m_MyReader.Read();
                sql = "INSERT INTO tblparent(LoginName,Password,Email,Name) VALUES('" + m_MyReader.GetValue(0).ToString() + "', '" + m_MyReader.GetValue(1).ToString() + "', '" + m_MyReader.GetValue(2).ToString() + "', '" + m_MyReader.GetValue(3).ToString() + "')";
                MysqlTranDb.TransExecuteQuery(sql);                
                sql = "SELECT tblparent.Id FROM tblparent WHERE LoginName='" + m_MyReader.GetValue(0).ToString() + "' AND Email='" + m_MyReader.GetValue(2).ToString() + "' AND Name='" + m_MyReader.GetValue(3).ToString() + "'";
                m_MyReader.Close();
                m_MyReader = MysqlTranDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    m_MyReader.Read();
                    ParentId = int.Parse(m_MyReader.GetValue(0).ToString());
                }
                m_MyReader.Close();
            }            
            if (ParentId != 0)
            {
                Sql = "SELECT tblregdparentstud.StudentId FROM tblregdparentstud WHERE tblregdparentstud.ParentId=" + _RegId + "";
                m_MyReader = MysqlTranDb.ExecuteQuery(Sql);
                if (m_MyReader.HasRows)
                {
                    while (m_MyReader.Read())
                    {
                        sql = "INSERT INTO tblparentstudmap(ParentId,StudId) VALUES(" + ParentId + "," + int.Parse(m_MyReader.GetValue(0).ToString()) + ")";
                        MysqlTranDb.TransExecuteQuery(sql);
                    }
                    m_MyReader.Close();
                    sql = "DELETE FROM tblregdparentstud WHERE tblregdparentstud.ParentId=" + _RegId + "";
                    MysqlTranDb.TransExecuteQuery(sql);
                    sql = "DELETE FROM tblregistration WHERE tblregistration.Id=" + _RegId + "";
                    MysqlTranDb.TransExecuteQuery(sql);
                    MysqlTranDb.TransactionCommit();
                    success = 1;
                }
                else
                {
                    MysqlTranDb.TransactionRollback();
                }
            }
            else
            {
                MysqlTranDb.TransactionRollback();
            }
        }
        catch 
        {
            MysqlTranDb.TransactionRollback();
        }
        return success;
    }

    public int RejectRegistration(int _RegId)
    {
        int success = 0;
        MysqlClass MysqlTranDb;
        MysqlTranDb = new MysqlClass(this);
        try
        {
            MysqlTranDb.MyBeginTransaction();
            string sql = "DELETE FROM tblregdparentstud WHERE tblregdparentstud.ParentId=" + _RegId + "";
            MysqlTranDb.TransExecuteQuery(sql);
            sql = "DELETE FROM tblregistration WHERE tblregistration.Id=" + _RegId + "";
            MysqlTranDb.TransExecuteQuery(sql);
            MysqlTranDb.TransactionCommit();
            success = 2;
        }
        catch 
        {
            MysqlTranDb.TransactionRollback();
            success = 0;
        }
        return success;
    }



    public void DeleteCombinedSubject(int _subID, out string _message)
    {
        _message = "";
        try
        {
            string sql = "select SubjectId from tblclasssubmap where SubjectId=" + _subID;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            m_MyReader.Read();
            if (m_MyReader.HasRows)
            {
                _message = "Cannot delete.The subject is assigned to some class.";
            }
            else
            {
                sql = "delete from tblcombinedsubjects where CombSubId=" + _subID;
                m_MysqlDb.ExecuteQuery(sql);
                sql = "delete from tblsubjects where Id=" + _subID;
                m_MysqlDb.ExecuteQuery(sql);
                _message = "The subject is deleted";
            }
        }
        catch
        {
            _message = "Action failed. Please try again";
        }

    }

    # region PromotionRule

    public int SavePromotionMaster(string _RuleName, string _RuleType)
    {
        int _RuleMaxId = -1;
        if (m_TransationDb != null)
        {
            General _GenObj = new General(m_TransationDb);
             _RuleMaxId = _GenObj.GetTableMaxId("tblpromotionrule_master", "RuleId");
        }
        else
        {
            General _GenObj = new General(m_MysqlDb);
             _RuleMaxId = _GenObj.GetTableMaxId("tblpromotionrule_master", "RuleId");
        }
       
        string sql = "insert into tblpromotionrule_master(RuleId,Name,`Type`,`Table`) values(" + _RuleMaxId + ",'" + _RuleName + "','" + _RuleType + "','')";
        if (m_TransationDb != null)
        {
            m_TransationDb.ExecuteQuery(sql);
        }
        else
        {
            m_MysqlDb.ExecuteQuery(sql);
        }
        return _RuleMaxId;
    }

    public void SaveExamPromotionRule(int _MasterId, string _ExamId, string _ExamName, string _Percentage,int _Combined , string _Period)
    {
        double _Per =0;
        if (_Percentage != "")
            double.TryParse(_Percentage, out _Per);
        string sql = "insert into tblpromotionrule_exam(MasterId,ExamId,ExamName,Period,Percentage,Combined) values(" + _MasterId + "," + _ExamId + ",'" + _ExamName + "'," +_Period +"," + _Per + "," + _Combined + ")";
        m_TransationDb.ExecuteQuery(sql);
    }

    public void SaveAttendancePromotionRule(int _MasterId, string _AttPer)
    {
        double _Per = 0;
        if (_AttPer != "")
            double.TryParse(_AttPer, out _Per);
        string sql = "insert into tblpromotionrule_attendance(MasterId,Percentage) values(" + _MasterId + "," + _Per + ")";
        m_MysqlDb.ExecuteQuery(sql);
    }


    public bool RuleNotMappedToClass(string _RuleId)
    {
        bool _Valid = true;
        string sql = "select tblclasspromotionrulemap.RuleId from tblpromotionrule_master inner join  tblclasspromotionrulemap on tblclasspromotionrulemap.RuleId = tblpromotionrule_master.RuleId where tblpromotionrule_master.RuleId =" + _RuleId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _Valid = false;
        }
        return _Valid;
    }

    public void DeleteRule(string _RuleId)
    {
        string sql = "delete from tblpromotionrule_exam where MasterId=" + _RuleId;
        m_MysqlDb.ExecuteQuery(sql);
        sql = "delete from tblpromotionrule_attendance where MasterId=" + _RuleId;
        m_MysqlDb.ExecuteQuery(sql);
        sql = "delete from tblpromotionrule_master where RuleId=" + _RuleId;
        m_MysqlDb.ExecuteQuery(sql);
    }

    # endregion

    #region HOME INFO - Student Manager, School Manager
 
    //public string getHomeInfo(int _Mode)
    //{
    //    string HomeInfo;
    //    if (_Mode == 1)
    //    {
    //        HomeInfo = GetTableHomeInfo(1);
    //    }
    //    else
    //    {
    //        HomeInfo = GetTableHomeInfo(2);
    //    }


       
    //    return HomeInfo;
    //}


    public string getHomeInfo(int _Mode, int _RoleId, int _BatchId, int UserId)
    {
        OdbcDataReader m_MyReaderHead = null;
        string sql = "", content="", heading="", style="";
        int ActionId = 0, mode = 0,cnt=1;
         StringBuilder _HomeDetailString = new StringBuilder("");

         sql = "select distinct(tblaction.id), tblaction.MenuName from tblaction  inner join tblroleactionmap on tblaction.Id = tblroleactionmap.ActionId  inner join tblmoduleactionmap on tblaction.Id = tblmoduleactionmap.ActionId where tblaction.ActionType = 'HomeInfoLink' and tblmoduleactionmap.ModuleId in  ( select tblmodule.Id from tblmodule where tblmodule.ModuleType in(" + _Mode + ",3)) and tblaction.Id in (select tblroleactionmap.ActionId from tblroleactionmap where tblroleactionmap.RoleId =" + _RoleId + ") order by tblaction.Order";
         m_MyReaderHead = m_MysqlDb.ExecuteQuery(sql);
         if (m_MyReaderHead.HasRows)
         {
             _HomeDetailString.Append("<table cellspacing=\"10px\"  class=\"tablelist\"><tr>");

             while (m_MyReaderHead.Read())
             {
                 ActionId = int.Parse(m_MyReaderHead.GetValue(0).ToString());
                 content = GetInfo(ActionId, mode, _BatchId, UserId);
                 heading = m_MyReaderHead.GetValue(1).ToString();
                 _HomeDetailString.Append("<td align=\"center\">" + UseBox(content, heading, style) + "</td>");
                 if (cnt % 3 == 0)
                 {
                     _HomeDetailString.Append("</tr><tr>");
                 }
                 cnt++;
             }
             cnt--;
             if (cnt % 3 != 0)
             {
                
                 string RemaingTds = "";
                 for (int i = 1; i <=3-( cnt % 3); i++)
                 {
                     RemaingTds = RemaingTds + "<td><div style=\"width:260px\"></div></td>";
                 }
                 _HomeDetailString.Append(RemaingTds);
             }
             _HomeDetailString.Append("</tr></table>");
         }
         m_MyReaderHead.Close();
         return _HomeDetailString.ToString();
    }

    private string GetInfo(int ActionId, int mode, int _BatchId, int UserId)
    {
        string content = "";
        if (ActionId == 420)
        {
            content = getStudTableInfo(_BatchId);
        }
        else if (ActionId == 421)
        {
            content = getExamTableInfo(_BatchId);
        }
        else if (ActionId == 422)
        {
            content = getFeesTableInfo(_BatchId);
        }
        else if (ActionId == 423)
        {
            content = getSMSTableInfo();
        }
        else if (ActionId == 424)
        {
            content = getLibraryTableInfo();
        }
        else if (ActionId == 425)
        {
            content = getTimeTableInfo(UserId);
        }
        else if (ActionId == 426)
        {
            content = getTransportTableInfo();
        }
        else if (ActionId == 427)
        {
            content = getStaffTableInfo(ActionId);
        }
        else if (ActionId == 428)
        {
            content = getClassTableInfo(ActionId, _BatchId);
        }
        return content;
    }

    private string getStudTableInfo( int _BatchId)
    {

        StringBuilder _StudentTable = new StringBuilder("");
        int total = 0, boys = 0, girls = 0, temp = 0;
        string sql = "";
        m_MyReader = null;
        _StudentTable.Append("   <div  class=\"divcontent\" > <table class=\"tablelist\" style=\"color:#0a0550\"");
        sql = "SELECT count(tblstudentclassmap.StudentId) FROM tblstudentclassmap inner join tblstudent on tblstudent.Id = tblstudentclassmap.StudentId WHERE tblstudent.Status=1 and tblstudentclassmap.BatchId = "+_BatchId+"";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                total = int.Parse( m_MyReader.GetValue(0).ToString());
            }
        }
        _StudentTable.Append("<tr><td class=\"leftside\" >Total</td><td class=\"rightside\">: "+total+"</td></tr>");



        m_MyReader = null;
        sql = "SELECT count(tblstudentclassmap.StudentId) FROM tblstudentclassmap inner join tblstudent on tblstudent.Id = tblstudentclassmap.StudentId WHERE tblstudent.Status=1 and tblstudentclassmap.BatchId = " + _BatchId + " AND tblstudent.Sex='Male'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                boys = int.Parse(m_MyReader.GetValue(0).ToString());
            }
        }
        _StudentTable.Append("<td class=\"leftside\" >Boys</td><td class=\"rightside\">: " + boys + "</td>");

        m_MyReader = null;
        sql = "SELECT count(tblstudentclassmap.StudentId) FROM tblstudentclassmap inner join tblstudent on tblstudent.Id = tblstudentclassmap.StudentId WHERE tblstudent.Status=1 and tblstudentclassmap.BatchId = " + _BatchId + "  AND tblstudent.Sex='Female'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                girls = int.Parse(m_MyReader.GetValue(0).ToString());
            }
        }
        _StudentTable.Append("<tr><td class=\"leftside\" >Girls</td><td class=\"rightside\">: " + girls + "</td></tr>");

        m_MyReader = null;
        sql = "select count(id) from tbltempstdent where status=1";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                temp = int.Parse(m_MyReader.GetValue(0).ToString());
            }
        }
        _StudentTable.Append("<tr><td class=\"leftside\" >Registered Students</td><td class=\"rightside\">: " + temp + "</td></tr>");


        int pr_studs = 0;
        m_MyReader = null;
        sql = "select count(tblstudent.Id) FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id inner join tblbatch on tblbatch.Status=1   where tblstudent.Status=1      AND tblstudentclassmap_history.BatchId=tblbatch.LastbatchId    AND tblstudent.Id NOt IN (Select tblstudentclassmap.StudentId    from tblstudentclassmap where tblstudentclassmap.BatchId=" + _BatchId + " ) Order by tblstudentclassmap_history.RollNo ASC";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                pr_studs = int.Parse(m_MyReader.GetValue(0).ToString());
            }
        }
        _StudentTable.Append("<tr><td class=\"leftside\" >Students in Promotion List </td><td class=\"rightside\">: " + pr_studs + "</td></tr>");



        _StudentTable.Append("</table></div>");
        return _StudentTable.ToString();
    }

    private string getExamTableInfo(int _BatchId)
    {
        StringBuilder _ExamTable = new StringBuilder("");
        int created = 0, scheduled = 0, not_schedule = 0, Marks_entered = 0;
        string sql = "";
        _ExamTable.Append("   <div  class=\"divcontent\" > <table class=\"tablelist\" style=\"color:#0a0550\">");
        m_MyReader = null;
        sql = "select count(id) from tblexammaster where status=1";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                created = int.Parse(m_MyReader.GetValue(0).ToString());
            }
        }
        _ExamTable.Append("<tr><td class=\"leftside\" >Master Exams Created</td><td class=\"rightside\">: " + created + "</td></tr>");


        m_MyReader = null;
        sql = "select count(distinct(tblclassexam.examid)) from tblclassexam inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id where tblclassexam.status=1 and tblexamschedule.BatchId = " + _BatchId + "";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                scheduled = int.Parse(m_MyReader.GetValue(0).ToString());
            }
        }
        _ExamTable.Append("<tr><td class=\"leftside\" >Master Exams Scheduled</td><td class=\"rightside\">: " + scheduled + "</td></tr>");

        not_schedule = created - scheduled;
        //m_MyReader = null;
        //sql = "select count(id) from tblexammaster where status=1";
        //m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        //if (m_MyReader.HasRows)
        //{
        //    while (m_MyReader.Read())
        //    {
        //        not_schedule = int.Parse(m_MyReader.GetValue(0).ToString());
        //    }
        //}
      //  _ExamTable.Append("<tr><td class=\"leftside\" >Not Scheduled</td><td class=\"rightside\">: " + not_schedule + "</td></tr>");
        int examsscheduled = 0;
        sql = "select count(tblexamschedule.id) from tblexamschedule  inner join tblclassexam on tblexamschedule.ClassExamId = tblclassexam.id where tblclassexam.status=1 and tblexamschedule.BatchId = " + _BatchId + "";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                examsscheduled = int.Parse(m_MyReader.GetValue(0).ToString());
            }
        }


        _ExamTable.Append("<tr><td class=\"leftside\" >Exams Scheduled</td><td class=\"rightside\">: " + examsscheduled + "</td></tr>");



        m_MyReader = null;
        sql = " select  count(distinct(tblstudentmark.ExamSchId)) from tblstudentmark inner join tblexamschedule  on tblstudentmark.ExamSchId = tblexamschedule.Id inner join tblclassexam on tblexamschedule.ClassExamId = tblclassexam.id where tblclassexam.status=1 and tblexamschedule.BatchId = " + _BatchId + "";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                Marks_entered = int.Parse(m_MyReader.GetValue(0).ToString());
            }
        }
        _ExamTable.Append("<tr><td class=\"leftside\" >Marks Entered</td><td class=\"rightside\">: " + Marks_entered + "</td></tr>");

        _ExamTable.Append("</table></div>");
        return _ExamTable.ToString();

    }

    private string getFeesTableInfo(int _BatchId)
    {

        StringBuilder _FeeTable = new StringBuilder("");
        double created = 0, Not_remitted = 0,_ActualTotal=0;
        double remitted = 0;
        _FeeTable.Append("   <div  class=\"divcontent\" > <table class=\"tablelist\" style=\"color:#0a0550\">");
      
        m_MyReader = null;
        string sql = "SELECT count( Distinct(FeeId)) FROM tblfeeschedule WHERE BatchId=" + _BatchId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
                double.TryParse(m_MyReader.GetValue(0).ToString(),out created);
        }
        _FeeTable.Append("<tr><td class=\"leftside\" >No of Scheduled Fee</td><td class=\"rightside\">: " + created + "</td></tr>");


        m_MyReader = null;
        sql = "select sum(Amount) from tblfeestudent where tblfeestudent.StudId in (select tblstudent.Id from tblstudent where tblstudent.Status=1)";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            double.TryParse(m_MyReader.GetValue(0).ToString(), out _ActualTotal);
        }
        _FeeTable.Append("<tr><td class=\"leftside\" >Actual Amount to be collected</td><td class=\"rightside\">: " + _ActualTotal + "</td></tr>");

     
        m_MyReader = null;
        sql = "select sum(TotalAmount) from tblfeebill where tblfeebill.Canceled=0";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            double.TryParse(m_MyReader.GetValue(0).ToString(),out remitted);
        }
        _FeeTable.Append("<tr><td class=\"leftside\" >Total Amount Collected</td><td class=\"rightside\">: " + remitted + "</td></tr>");


        m_MyReader = null;
        sql = "select sum(BalanceAmount) from tblfeestudent where tblfeestudent.StudId in (select tblstudent.Id from tblstudent where tblstudent.Status=1)";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            double.TryParse(m_MyReader.GetValue(0).ToString(), out Not_remitted);
        }
        _FeeTable.Append("<tr><td class=\"leftside\" >Amount to be Collected</td><td class=\"rightside\">: " + Not_remitted + "</td></tr>");


        _FeeTable.Append("</table></div>");
        return _FeeTable.ToString();
    }

    private string getSMSTableInfo() 
    {
        StringBuilder _SMSTable = new StringBuilder("");
        int stud_config = 0, staff_config = 0, parent_config = 0;
        string sql = "";
        _SMSTable.Append("   <div  class=\"divcontent\" > <table class=\"tablelist\" style=\"color:#0a0550\">");
        m_MyReader = null;
        sql = "select count(tblsmsstudentlist.Id) from tblsmsstudentlist inner join tblstudent on tblstudent.Id=tblsmsstudentlist.Id where tblsmsstudentlist.phoneno!=0 and tblsmsstudentlist.enabled=1 and tblstudent.Status=1";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
                stud_config = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        _SMSTable.Append("<tr><td class=\"leftside\" >Students Configured</td><td class=\"rightside\">: " + stud_config + "</td></tr>");
  
       
        

        m_MyReader = null;
        sql = "select count(tblsmsstafflist.Id) from tblsmsstafflist inner join tbluser ON tbluser.Id=tblsmsstafflist.Id where tblsmsstafflist.phoneno!=0 and tblsmsstafflist.enabled=1 and tbluser.Status=1 and tbluser.RoleId!=1";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
               staff_config = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        _SMSTable.Append("<tr><td class=\"leftside\" >Staffs Configured</td><td class=\"rightside\">: " + staff_config + "</td></tr>");


        m_MyReader = null;
        sql = "      select count(tblsmsparentlist.Id) from tblsmsparentlist  inner join tblstudent on tblstudent.Id=tblsmsparentlist.Id where phoneno!=0 and enabled=1 and tblstudent.Status=1";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
                parent_config = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        _SMSTable.Append("<tr><td class=\"leftside\" >Parents Configured</td><td class=\"rightside\">: " + parent_config + "</td></tr>");
        _SMSTable.Append("</table></div>");
        return _SMSTable.ToString();
    }

    private string getLibraryTableInfo()
    {

        StringBuilder _LibTable = new StringBuilder("");
        int total = 0, student_issued = 0, staff_issued = 0, stud_pending = 0, staff_pending=0;
        string sql = "";
        _LibTable.Append("<div  class=\"divcontent\" > <table class=\"tablelist\">");

        m_MyReader = null;
        sql = "select count(Id) from tblbookmaster";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
                total = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        _LibTable.Append("<tr><td class=\"leftside\" >Total Books</td><td class=\"rightside\">: " + total + "</td></tr>");

        OdbcDataReader MyReader=null;

        //sql = "select count(tblbooks.Id) from tblbooks";
        //MyReader =m_MysqlDb.ExecuteQuery(sql);
        //if (MyReader.HasRows)
        //{
        //    _LibTable.Append("<tr><td class=\"leftside\" >Total Books</td><td class=\"rightside\">: " + MyReader.GetValue(0).ToString() + "</td></tr>");
        //}

        sql = "select count( tblbookissue.BookNo ) from tblbooks inner join tblbookissue on tblbookissue.BookNo = tblbooks.BookNo";
        MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            _LibTable.Append("<tr><td class=\"leftside\" >Issued</td><td class=\"rightside\">: " + MyReader.GetValue(0).ToString() + "</td></tr>");
        }
        sql = "select count( tblbookbooking.bookId) from tblbooks inner join tblbookbooking on tblbookbooking.bookId = tblbooks.BookNo";
        MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            _LibTable.Append("<tr><td class=\"leftside\" >Booked</td><td class=\"rightside\">: " + MyReader.GetValue(0).ToString() + "</td></tr>");
        }
        sql = "select count(tblbooks.BookNo) from tblbooks where tblbooks.BookNo not in (select tblbookissue.BookNo from tblbooks inner join tblbookissue on tblbookissue.BookNo = tblbooks.BookNo)";
        MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            _LibTable.Append("<tr><td class=\"leftside\" >Available Books</td><td class=\"rightside\">: " + MyReader.GetValue(0).ToString() + "</td></tr>");
        }
        sql = "select count(tblbookissue.BookNo) from tblbookissue where tblbookissue.UserType=1";
        MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            _LibTable.Append("<tr><td class=\"leftside\" >Issued to Students</td><td class=\"rightside\">: " + MyReader.GetValue(0).ToString() + "</td></tr>");
        }
        sql = "select count(tblbookissue.BookNo) from tblbookissue where tblbookissue.UserType=2";
        MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            _LibTable.Append("<tr><td class=\"leftside\" >Issued to Staffs</td><td class=\"rightside\">: " + MyReader.GetValue(0).ToString() + "</td></tr>");
        }



        _LibTable.Append("</table></div>");
        return _LibTable.ToString();
    }

    private string getTimeTableInfo(int UserId)
    {
        StringBuilder _TimeTable = new StringBuilder("");
        int Created = 0, not_Created = 0;
        string sql = "";
        _TimeTable.Append("<div  class=\"divcontent\" > <table class=\"tablelist\">");
        m_MyReader = null;
        sql = "select count(DISTINCT(ClassId)) from tbltime_classperiod";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
                Created = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        _TimeTable.Append("<tr><td class=\"leftside\" >Created For</td><td class=\"rightside\">: " + Created + " Classes</td></tr>");


        sql = "SELECT count(tblclass.Id) from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + UserId + ") and tblclass.Id not in (select DISTINCT(ClassId) from tbltime_classperiod)";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
                not_Created = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        _TimeTable.Append("<tr><td class=\"leftside\" >Not Created For</td><td class=\"rightside\">: " + not_Created + "Classes</td></tr>");

        _TimeTable.Append("</table></div>");
        return _TimeTable.ToString();
    }

    private string getTransportTableInfo()
    {
        StringBuilder _TransportTable = new StringBuilder("");
        int vehicles = 0, trips = 0, routes=0;
        string sql = "";
        _TransportTable.Append("<div  class=\"divcontent\" > <table class=\"tablelist\">");

        sql = "SELECT count(Id) FROM tblstudent WHERE Status=1";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                vehicles = int.Parse(m_MyReader.GetValue(0).ToString());
            }
        }
        _TransportTable.Append("<tr><td class=\"leftside\" >Vehicles</td><td class=\"rightside\">: " + vehicles + "</td></tr>");

        sql = "SELECT count(Id) FROM tblstudent WHERE Status=1";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                trips = int.Parse(m_MyReader.GetValue(0).ToString());
            }
        }
        _TransportTable.Append("<tr><td class=\"leftside\" >Trips</td><td class=\"rightside\">: " + trips + "</td></tr>");


        sql = "SELECT count(Id) FROM tblstudent WHERE Status=1";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                routes = int.Parse(m_MyReader.GetValue(0).ToString());
            }
        }
        _TransportTable.Append("<tr><td class=\"leftside\" >Routes</td><td class=\"rightside\">: " + routes + "</td></tr>");


        _TransportTable.Append("</table></div>");
        return _TransportTable.ToString();
    }

    private string getStaffTableInfo(int ActionId)
    {
        StringBuilder _StaffTable = new StringBuilder("");
        int staffs = 0, allotted = 0 ;
        string sql = "";

        _StaffTable.Append("<div  class=\"divcontent\" > <table class=\"tablelist\" style=\"color:#0a0550\">");
        sql = "select count(tbluser.id) from tbluser inner join tblrole on tbluser.RoleId = tblrole.Id and tblrole.Type = 'Staff' and tbluser.Status =1";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                staffs = int.Parse(m_MyReader.GetValue(0).ToString());
            }
        }
        _StaffTable.Append("<tr><td class=\"leftside\" >Staffs</td><td class=\"rightside\">: " + staffs + "</td></tr>");
        OdbcDataReader myreader1=null;
        m_MyReader=null;
        sql = "select distinct(tbluser.RoleId), tblrole.RoleName from tbluser inner join tblrole on tbluser.RoleId = tblrole.Id and tblrole.Type = 'Staff' and tbluser.Status =1 order by tbluser.RoleId";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            while (m_MyReader.Read())
            {
                sql="select count(tbluser.Id) from tbluser where tbluser.RoleId="+int.Parse(m_MyReader.GetValue(0).ToString())+"";
                        myreader1 = m_MysqlDb.ExecuteQuery(sql);

                  if (myreader1.HasRows)

                        {
                        while (myreader1.Read())
                        {
                          _StaffTable.Append("<tr><td class=\"leftside\" >"+m_MyReader.GetValue(1).ToString()+"</td><td class=\"rightside\">: "+myreader1.GetValue(0).ToString()+"</td></tr>");

                        }
                        }

            }
        }



        //sql = "select count(distinct(staffid)) from tblstaffsubjectmap";
        //m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        //if (m_MyReader.HasRows)
        //{
        //    while (m_MyReader.Read())
        //    {
        //        allotted = int.Parse(m_MyReader.GetValue(0).ToString());
        //    }
        //}
        //_StaffTable.Append("<tr><td class=\"leftside\" >Periods Allotted</td><td class=\"rightside\">: " + allotted + "</td></tr>");

        _StaffTable.Append("</table></div>");
        return _StaffTable.ToString();
    
    }

    private string getClassTableInfo(int ActionId, int _BatchId)
    {
        StringBuilder _ClassTable = new StringBuilder("");
        int classes = 0, students = 0;
        string sql = "";
        _ClassTable.Append("<div  class=\"divcontent\" > <table class=\"tablelist\" style=\"color:#0a0550\">");
        sql = "select count(id) from tblclass where tblclass.Status=1";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
                classes = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        _ClassTable.Append("<tr><td class=\"leftside\" >Classes</td><td class=\"rightside\">: " + classes + "</td></tr>");


        OdbcDataReader myReader1 = null;
        sql = "select DISTINCT(tblclass.Id),tblclass.ClassName from tblclass INNER JOIN tblstandard ON tblstandard.Id=tblclass.Standard where tblclass.Status=1 ORDER BY tblstandard.Id,tblclass.ClassName";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _ClassTable.Append("<tr><td class=\"leftside\" > </td><td class=\"rightside\"> </td></tr>");
            _ClassTable.Append("<tr><td class=\"leftside\" ><b><u>CLASS</u></b> </td><td class=\"rightside\"><u>STUDENTS</u></td></tr>");
            while (m_MyReader.Read())
            {
                sql = "select count(tblstudentclassmap.studentId) from tblstudentclassmap inner join tblstudent on tblstudentclassmap.StudentId= tblstudent.Id where tblstudent.Status=1 and tblstudentclassmap.ClassId = " + int.Parse(m_MyReader.GetValue(0).ToString()) + " and tblstudentclassmap.BatchId =" + _BatchId;
                myReader1 = m_MysqlDb.ExecuteQuery(sql);
                if (myReader1.HasRows)
                {
                        _ClassTable.Append("<tr><td class=\"leftside\" >" + m_MyReader.GetValue(1).ToString() + "</td><td class=\"rightside\">:  " + myReader1.GetValue(0).ToString()+ "</td></tr>");
                }
            }
        }

        _ClassTable.Append("</table></div>");
        return _ClassTable.ToString();

    }

    private string UseBox(string _Content, string _Heading, string _Style)
    {
        StringBuilder _RoundBoxDetailString = new StringBuilder("");
        _RoundBoxDetailString.Append("   <div style=\"width:260px;\" class=\"panel panel-primary\">  <div class=\"panel-heading\">  " + _Heading + " </div> <div class=\"panel-body\"> <div style=\"min-height:150px; max-height:150px;\"> " + _Content + " </div>	</div> </div>  ");
        //_RoundBoxDetailString.Append("<div class=\"" + _Style + "\"><table ><tr><td class=\"topleft\"></td><td class=\"topmiddle\" ></td><td class=\"topright\"></td></tr><tr><td class=\"centerleft\"></td><td class=\"centermiddle\" >");
        //_RoundBoxDetailString.Append("<span class=\"heading\">" + _Heading + "</span><div class=\"line\"></div><br />");
        //_RoundBoxDetailString.Append("<div  class=\"divcontent\">" + _Content + " </div>");
        //_RoundBoxDetailString.Append("</td><td class=\"centerright\"></td></tr><tr><td class=\"bottomleft\"></td><td class=\"bottommiddile\" ></td><td class=\"bottomright\"></td></tr></table></div>");

    return _RoundBoxDetailString.ToString();

    }

    #endregion



    public void ReArrangeFeeSchedule(string _OldClassId, string _NewClassID, int _CurrentBatchId,int _StudentId )
    {
        int SchId = 0;
        DateTime _DueDate = System.DateTime.Now;
        DateTime _LastDate = System.DateTime.Now;
        //DATE_FORMAT(Duedate,'%d/%m/%Y') as     DATE_FORMAT(LastDate,'%d/%m/%Y') as
        string sql;// = "select FeeId,BatchId,  Duedate, LastDate,Status,PeriodId,Amount,Id from tblfeeschedule where ClassId=" + _OldClassId + " and BatchId=" + _CurrentBatchId;
        sql = "select tblfeeschedule.FeeId,tblfeeschedule.BatchId, tblfeeschedule.Duedate, tblfeeschedule.LastDate,tblfeeschedule.`Status`,tblfeeschedule.PeriodId,tblfeeschedule.Amount,tblfeeschedule.Id from tblfeestudent inner join  tblfeeschedule on tblfeeschedule.Id = tblfeestudent.SchId where ClassId=" + _OldClassId + " and BatchId=" + _CurrentBatchId + " and tblfeestudent.StudId=" + _StudentId;
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                DateTime.TryParse(m_MyReader.GetValue(2).ToString(), out _DueDate);
                DateTime.TryParse(m_MyReader.GetValue(3).ToString(), out _LastDate);
                if (ScheduleNotExists(m_MyReader.GetValue(0).ToString(), m_MyReader.GetValue(1).ToString(), m_MyReader.GetValue(5).ToString(), _NewClassID, out SchId))
                {
                    int _MaxId = GetMaxId("tblfeeschedule", "Id");
                    
                    sql = "insert into  tblfeeschedule(Id,FeeId,BatchId,Duedate,LastDate,Status,ClassId,PeriodId,Amount) values(" + _MaxId + "," + m_MyReader.GetValue(0).ToString() + "," + _CurrentBatchId + ",'" + _DueDate.Date.ToString("s") + "','" + _LastDate.Date.ToString("s") + "','" + m_MyReader.GetValue(4).ToString() + "'," + _NewClassID + "," + m_MyReader.GetValue(5).ToString() + "," + m_MyReader.GetValue(6).ToString() + ")";
                    m_TransationDb.ExecuteQuery(sql);
                    SchId = _MaxId;
                }
               
                sql = "update tblfeestudent set SchId=" + SchId + " where SchId=" + m_MyReader.GetValue(7).ToString() + " and StudId=" + _StudentId;
                m_TransationDb.ExecuteQuery(sql);
                //sql = "update tbltransactionhistory set PaymentElementId=" + SchId + " where PaymentElementId=" + m_MyReader.GetValue(7).ToString() + " and UserId=" + _StudentId;
                //m_TransationDb.ExecuteQuery(sql);
            }
            //sql = "delete from tblfeeschedule where tblfeeschedule.Id not in (select distinct SchId from tblfeestudent)";
            //m_TransationDb.ExecuteQuery(sql);
        }
    }

    private int GetMaxId(string _Table, string _Id)
    {
        int Id = 0;
        OdbcDataReader MyTempReader = null;
        string sql = "select MAX(" + _Id + ") from " + _Table + "";
        MyTempReader = m_TransationDb.ExecuteQuery(sql);
        if (MyTempReader.HasRows)
        {
            int.TryParse(MyTempReader.GetValue(0).ToString(), out Id);
        }
        Id = Id + 1;
        MyTempReader.Close();
        return Id;
    }

    private bool ScheduleNotExists(string _FeeID, string _BatchId, string _PeriodId, string _NewClassID, out int _ScheduledId)
    {
        bool _Valid = true;
        _ScheduledId = 0;
        OdbcDataReader MyTempReader = null;
        string sql = "select Id from tblfeeschedule where ClassId=" + _NewClassID + " and BatchId=" + _BatchId + " and PeriodId=" + _PeriodId + " and FeeId=" + _FeeID;
        MyTempReader = m_TransationDb.ExecuteQuery(sql);
        if (MyTempReader.HasRows)
        {
            int.TryParse(MyTempReader.GetValue(0).ToString(), out _ScheduledId);
            _Valid = false;
        }
        MyTempReader.Close();
        return _Valid;
    }

    public void ScheduleFeesWhilePromotion(string _FromClass, string _ToClass, int _Batch)
    {

        DataSet MyStudent = null;
       
        string sql = "select tblstudentclassmap.StudentId from tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudent.Id where tblstudent.`Status`=1 and tblstudentclassmap.ClassId=" + _ToClass;
        MyStudent = m_TransationDb.ExecuteQueryReturnDataSet(sql);
        if (MyStudent != null && MyStudent.Tables != null && MyStudent.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow Dr_Student in MyStudent.Tables[0].Rows)
            {
                
            }
        }
    }


    public bool CheckForRuleApplicableToClassAndFee1(string _ClassId, int _FeeId)
    {
        bool _flag = false;
        CLogging logger = CLogging.GetLogObject();
        string sql = "select tblruleclassmap.RuleId from tblruleclassmap where tblruleclassmap.classId=" + _ClassId + " and tblruleclassmap.feeTypeId=" + _FeeId;
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _flag = true;
        }
        return _flag;
    }

    public void ScheduleStudFee(int _StudId, int _FeeSchId, double _Amount, string _Status)
    {
        double BalanceAmount = _Amount;
        AutoAdvanceCancellion(_FeeSchId, _StudId, ref _Amount, ref BalanceAmount, ref _Status);
        string sql = "INSERT INTO tblfeestudent(SchId,StudId,Amount,BalanceAmount,Status) VALUES(" + _FeeSchId + "," + _StudId + "," + _Amount + "," + BalanceAmount + ",'" + _Status + "')";
        if (m_TransationDb == null)
            m_MysqlDb.ExecuteQuery(sql);
        else
            m_TransationDb.ExecuteQuery(sql);
    }

    # region Auto Advance Cancellion Region

    public String ISAdvanceAutoCancel
    {
        get
        {
            string m_AdvanceAutoCancel = "-1";
            if (m_AdvanceAutoCancel == "-1")
            {
                OdbcDataReader _MyReaderNew = null;
                string sql = "SELECT Value FROM tblconfiguration WHERE id =23";
                if (m_TransationDb != null)
                    _MyReaderNew = m_TransationDb.ExecuteQuery(sql);
                else
                    _MyReaderNew = m_MysqlDb.ExecuteQuery(sql);
                if (_MyReaderNew.HasRows)
                {

                    m_AdvanceAutoCancel = _MyReaderNew.GetValue(0).ToString();
                }
            }
            return m_AdvanceAutoCancel;
        }


    }

    private void InserAdvanceTransaction(int _StudId, string _StudentName, string _FeeName, string _Period, int _BatchId, double _Amount, int _FeeId, int _PeriodId, string _TempId, int _Type, string _BillId, double _TotalAdvAmount)
    {
        string sql = "insert into tblfeeadvancetransaction(StudentId,StudentName,FeeName,PeriodName,BatchId,Amount,FeeId,PeriodId,TempId,Type,BillNo,CreatedUser,CreatedDate,AdvanceBalance) values (" + _StudId + ",'" + _StudentName + "','" + _FeeName + "','" + _Period + "'," + _BatchId + "," + _Amount + "," + _FeeId + "," + _PeriodId + ",'" + _TempId + "'," + _Type + ",'" + _BillId + "','" + m_UserName + "','" + DateTime.Now.ToString("s") + "'," + _TotalAdvAmount + ")";
        if (m_TransationDb == null)
            m_MysqlDb.ExecuteQuery(sql);
        else
            m_TransationDb.ExecuteQuery(sql);
    }


    private bool AutoAdvanceCancellion(int _FeeSchId, int _StudId, ref double _Amount, ref double BalanceAmount, ref string _Status)
    {
        bool _valid = true;
        double _advanceAmt = 0;
        double _CanceladvanceAmt = 0;
        int _advId = 0;
        if (ISAdvanceAutoCancel == "1")
        {
            if (GetAdvanceAmount(_FeeSchId, _StudId, ref _advanceAmt, ref _advId))
            {
                RecalculateSchDetails(ref _advanceAmt, ref _CanceladvanceAmt, ref BalanceAmount, ref _Status);
                CancelAdvance(_advId, _advanceAmt, _CanceladvanceAmt);
            }
        }
        return _valid;
    }

    private void CancelAdvance(int _advId, double _advanceAmt, double _CanceladvanceAmt)
    {
        string sql = "";
        int StudentId, BatchId, FeeId, PeriodId;
        string StudentName, FeeName, PeriodName, TempId;
        double AdvanceBalance;
        GetFeeAdvanceDetails(_advId, out StudentId, out StudentName, out FeeName, out PeriodName, out BatchId, out FeeId, out PeriodId, out TempId, out AdvanceBalance);
        InserAdvanceTransaction(StudentId, StudentName, FeeName, PeriodName, BatchId, _CanceladvanceAmt, FeeId, PeriodId, TempId, 0, "NIL", AdvanceBalance - _CanceladvanceAmt);
        if (_advanceAmt > 0)
            sql = "UPDATE tblstudentfeeadvance SET tblstudentfeeadvance.Amount=" + _advanceAmt + "  WHERE tblstudentfeeadvance.Id =" + _advId;
        else
            sql = "delete from tblstudentfeeadvance where Id=" + _advId;

        if (m_TransationDb == null)
            m_MysqlDb.ExecuteQuery(sql);
        else
            m_TransationDb.ExecuteQuery(sql);

    }

    private void GetFeeAdvanceDetails(int _advId, out int StudentId, out string StudentName, out string FeeName, out string PeriodName, out int BatchId, out int FeeId, out int PeriodId, out string TempId, out double AdvanceBalance)
    {
        StudentId = 0;
        StudentName = "";
        FeeName = "";
        PeriodName = "";
        BatchId = 0;
        FeeId = 0;
        PeriodId = 0;
        TempId = "";
        AdvanceBalance = 0;
        OdbcDataReader _MyReader = null;
        string sql = "select StudentId,StudentName,FeeName,PeriodName,BatchId,FeeId,PeriodId,TempId from tblstudentfeeadvance where tblstudentfeeadvance.Id=" + _advId;
        if (m_TransationDb == null)
            _MyReader = m_MysqlDb.ExecuteQuery(sql);
        else
            _MyReader = m_TransationDb.ExecuteQuery(sql);

        if (_MyReader.HasRows)
        {
            StudentId = int.Parse(_MyReader.GetValue(0).ToString());
            StudentName = _MyReader.GetValue(1).ToString();
            FeeName = _MyReader.GetValue(2).ToString();
            PeriodName = _MyReader.GetValue(3).ToString();
            BatchId = int.Parse(_MyReader.GetValue(4).ToString());
            FeeId = int.Parse(_MyReader.GetValue(5).ToString());
            PeriodId = int.Parse(_MyReader.GetValue(6).ToString());
            TempId = _MyReader.GetValue(7).ToString();
            AdvanceBalance = GetAdvanceBalance();

        }

    }

    private double GetAdvanceBalance()
    {
        double _advanceAmt = 0;
        OdbcDataReader _MyReader = null;
        string sql = "select tblfeeadvancetransaction.AdvanceBalance from tblfeeadvancetransaction order by tblfeeadvancetransaction.Id desc LIMIT 0,1";
        if (m_TransationDb == null)
            _MyReader = m_MysqlDb.ExecuteQuery(sql);
        else
            _MyReader = m_TransationDb.ExecuteQuery(sql);

        if (_MyReader.HasRows)
        {
            if (!double.TryParse(_MyReader.GetValue(0).ToString(), out _advanceAmt))
            {
                _advanceAmt = 0;

            }
        }
        return _advanceAmt;
    }



   private void RecalculateSchDetails(ref double _advanceAmt, ref double _CanceladvanceAmt, ref double BalanceAmount, ref string _Status)
    {
        if (_advanceAmt == 0)
        {
            //do nothing
        }
        else if (_advanceAmt == BalanceAmount)
        {
            _CanceladvanceAmt = _advanceAmt;
            _advanceAmt=0;
            BalanceAmount = 0;
            _Status = "Paid";
        }
        else if (_advanceAmt > BalanceAmount)
        {
            _CanceladvanceAmt = BalanceAmount;
            _advanceAmt = _advanceAmt - BalanceAmount;
            BalanceAmount = 0;
            _Status = "Paid";
        }
        else if (_advanceAmt < BalanceAmount)
        {
            _CanceladvanceAmt = _advanceAmt;
            BalanceAmount = BalanceAmount - _advanceAmt;
            _advanceAmt = 0;
            _Status = "Arrear";
        }
      
    }


    private bool GetAdvanceAmount(int _FeeSchId, int _StudId, ref double _advanceAmt, ref int _advId)
    {
        bool _valid = false;
        OdbcDataReader _MyReader = null;
        string sql = "select tblstudentfeeadvance.Amount,tblstudentfeeadvance.Id from tblstudentfeeadvance inner join tblfeeschedule on tblfeeschedule.PeriodId= tblstudentfeeadvance.PeriodId AND tblfeeschedule.BatchId= tblstudentfeeadvance.BatchId AND tblfeeschedule.FeeId= tblstudentfeeadvance.FeeId where tblfeeschedule.Id=" + _FeeSchId + "  AND tblstudentfeeadvance.StudentId=" + _StudId;
        if (m_TransationDb == null)
            _MyReader = m_MysqlDb.ExecuteQuery(sql);
        else
            _MyReader = m_TransationDb.ExecuteQuery(sql);

        if (_MyReader.HasRows)
        {
            if (!double.TryParse(_MyReader.GetValue(0).ToString(), out _advanceAmt))
            {
                _advanceAmt = 0;

            }
            else
            {
                if (!int.TryParse(_MyReader.GetValue(1).ToString(), out _advId))
                {
                    _advId = 0;
                }
                _valid = true;
            }

        }
        return _valid;
    }


    # endregion

    
    public bool CheckRuleIsApplicabletoThisStudent(int _feeId, int _ClassId, double _BaseAmount, int _userId, int _batchId, int _SteduleId)
    {
        bool _flag = false;
        DataSet _ruleset;
        double _ReturnAmt = _BaseAmount;
        string _tblname, _colname, _ruleassigmode, _ruleFieldvalue, _fieldtype;
        int _rulesAmounttype;
        float _rulesAmount;
        double _AmountAfterRuleCal;

            string sql = "select tblrules.Amounttype, tblrules.Amount , tblrules.AssigMode , tblrules.FieldValue,tblruleitem.tblname , tblruleitem.Colname , tblruleitem.fieldType from tblruleclassmap inner join tblrules on tblruleclassmap.RuleId = tblrules.Id  inner join tblruleitem on tblrules.tblruleitemId = tblruleitem.Id where tblruleclassmap.classId=" + _ClassId + " and tblruleclassmap.feeTypeId=" + _feeId;
            _ruleset = m_TransationDb.ExecuteQueryReturnDataSet(sql);

            foreach (DataRow dr_rule in _ruleset.Tables[0].Rows)
            {
                _tblname = dr_rule[4].ToString();
                _colname = dr_rule[5].ToString();
                _ruleassigmode = dr_rule[2].ToString();
                _ruleFieldvalue = dr_rule[3].ToString();
                _fieldtype = dr_rule[6].ToString();
                _rulesAmounttype = int.Parse(dr_rule[0].ToString());
                _rulesAmount = float.Parse(dr_rule[1].ToString());
                if (CheckIfTheRuleIsApllicableTothisStudent(_userId, _tblname, _colname, _ruleassigmode, _ruleFieldvalue, _fieldtype) == true)
                {
                    calculateRuleAmount(_rulesAmounttype, _rulesAmount, _BaseAmount, out _AmountAfterRuleCal);
                    if (_ReturnAmt > _AmountAfterRuleCal)
                    {
                        _ReturnAmt = _AmountAfterRuleCal;
                        _flag = true;
                    }

                    if (_rulesAmount < 0)
                    {
                        _ReturnAmt = _AmountAfterRuleCal;
                        _flag = true;
                    }

                }
            }

            AddStudDataToTblFeeStud(_SteduleId, _userId, _ReturnAmt);

        return _flag;

    }
    private bool CheckIfTheRuleIsApllicableTothisStudent(int StudentId, string _tblname, string _colname, string _ruleassigmode, string _ruleFieldvalue, string _fieldtype)
    {
        bool _flag = false;
        int _intcolname;
        string _stringcolname;
        try
        {
            CLogging logger = CLogging.GetLogObject();
            string sql = "select " + _tblname + " ." + _colname + " from " + _tblname + " where " + _tblname + ".id=" + StudentId;
            m_MyReader1 = m_TransationDb.ExecuteQuery(sql);
            logger.LogToFile("CheckIfTheRuleIsApllicableTothisStudent", "selecting tblname and corresponting col ", 'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
            if (m_MyReader1.HasRows)
            {
                _stringcolname = m_MyReader1.GetValue(0).ToString();

                if (_fieldtype == "Varchar" && _ruleassigmode == "Equal " && _stringcolname == _ruleFieldvalue)
                {

                    _flag = true;
                }
                else if (_fieldtype != "Varchar")
                {
                    _intcolname = int.Parse(_stringcolname.ToString());
                    if (_ruleassigmode == "Less than ")
                    {
                        if (_intcolname < (int.Parse(_ruleFieldvalue.ToString())))
                        {
                            _flag = true;
                        }

                    }
                    else if (_ruleassigmode == "Greater than ")
                    {

                        if (_intcolname > (int.Parse(_ruleFieldvalue.ToString())))
                        {
                            _flag = true;
                        }

                    }

                }
            }
        }
        catch
        {

        }

        return _flag;
    }


    private void calculateRuleAmount(int _rulesAmounttype, float _rulesAmount, double _BaseAmount, out double _AmountAfterRuleCal)
    {
        if (_rulesAmounttype == 1)
        {
            _AmountAfterRuleCal = _BaseAmount - _rulesAmount;
        }
        else
        {
            _AmountAfterRuleCal = ((_BaseAmount * _rulesAmount) / 100);
            _AmountAfterRuleCal = _BaseAmount - _AmountAfterRuleCal;
        }
    }

    private void AddStudDataToTblFeeStud(int schedId, int _studId, double _studamt)
    {
        string sql;
        double BalanceAmount = _studamt;
        string _Status = "";
        if (_studamt > 0)
        {
            _Status = "Scheduled";
        }
        else
        {
            _Status = "fee Exemtion";
        }
        AutoAdvanceCancellion(schedId, _studId, ref _studamt, ref BalanceAmount, ref _Status);
        sql = "INSERT INTO tblfeestudent(SchId,StudId,Amount,BalanceAmount,Status) VALUES(" + schedId + "," + _studId + "," + _studamt + "," + BalanceAmount + ",'" + _Status + "')";
        m_TransationDb.TransExecuteQuery(sql);
   
    }


    public void ScheduleRollNumberNew(int _Classid, int _Batchid)
    {
        int Rollno = 0;
        string sql = "SELECT tblstudent.Id  from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + _Batchid + " AND tblstudentclassmap.ClassId=" + _Classid + " Order by tblstudent.StudentName ASC";
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                Rollno++;
                UpdateRollNumberNew(_Classid, _Batchid, int.Parse(m_MyReader.GetValue(0).ToString()), Rollno);
            }
        }
    }

    private void UpdateRollNumberNew(int _classid, int _BatchId, int _StudentId, int _Rollno)
    {
        string sql = "update tblstudentclassmap set RollNo=" + _Rollno + " where StudentId=" + _StudentId + " and ClassId=" + _classid + " and BatchId=" + _BatchId + "";
        m_TransationDb.ExecuteQuery(sql);
        sql = "update tblstudent set RollNo=" + _Rollno + " where Id=" + _StudentId;
        m_TransationDb.ExecuteQuery(sql);
    }

    # region ParentLogin

    public DataSet GetParentsList(string _ClassId)
    {
        string CanLogin = "0";
        string ParentId = "";
        string UserId = "0";
        string _lastlogin = "",date="";
        string emailId = "";
        DataSet MyParentList = new DataSet();
        DataTable dt;
        DataRow dr;
        MyParentList.Tables.Add(new DataTable("Parent"));
        dt = MyParentList.Tables["Parent"];
        dt.Columns.Add("StudId");
        dt.Columns.Add("StudentName");
        dt.Columns.Add("GardianName");
        dt.Columns.Add("OfficePhNo");
        dt.Columns.Add("SentCredentials");
        dt.Columns.Add("CanLogin");
        dt.Columns.Add("ParentId");
        dt.Columns.Add("UserId");
        dt.Columns.Add("Status");
        dt.Columns.Add("LastLogin");
        dt.Columns.Add("EmailId");

       // string sql = "select tblstudent.Id , tblstudent.StudentName ,  tblstudent.GardianName , tblstudent.OfficePhNo  from tblstudent  inner join tblsmsparentlist on tblsmsparentlist.Id=  tblstudent.Id where tblstudent.LastClassId=" + _ClassId + " and tblstudent.`Status`=1 and   tblsmsparentlist.Enabled=1 order by tblstudent.RollNo asc";
        string sql = "select tblstudent.Id , tblstudent.StudentName ,  tblstudent.GardianName , tblstudent.OfficePhNo,Email  from tblstudent  where tblstudent.LastClassId=" + _ClassId + " and tblstudent.`Status`=1  order by tblstudent.RollNo asc";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                dr = MyParentList.Tables["Parent"].NewRow();
                dr["StudId"] = m_MyReader.GetValue(0).ToString();
                dr["StudentName"] = m_MyReader.GetValue(1).ToString();
                dr["GardianName"] = m_MyReader.GetValue(2).ToString();
                dr["EmailId"] = m_MyReader["Email"].ToString();

                int SiblingId = GetSiblingId(dr["StudId"].ToString());
                dr["SentCredentials"] = GetSendStatus(SiblingId, dr["StudId"].ToString(), out CanLogin, out ParentId, out UserId, out _lastlogin, out date, out emailId);

                if (UserId == "0")
                {
                    if (m_MyReader.GetValue(3).ToString() != "")
                        dr["OfficePhNo"] = m_MyReader.GetValue(3).ToString();
                    else
                        dr["OfficePhNo"] = "0";
                }
                else
                {
                    dr["OfficePhNo"] = UserId;
                }

                dr["EmailId"] = emailId;                   
                dr["CanLogin"] = CanLogin;
                dr["ParentId"] = ParentId;
                dr["UserId"] = UserId;
                dr["UserId"] = UserId;
                dr["LastLogin"] = _lastlogin;
                OdbcDataReader configreader = null;
                int duration = 0;
                string configsql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='Duration Of Parent Log In Inactivation'";
                configreader = m_MysqlDb.ExecuteQuery(configsql);
                if (configreader.HasRows)
                {
                    int.TryParse(configreader.GetValue(0).ToString(), out duration);

                }
                configreader.Close();
                if (date != "")
                {
                    DateTime tdydate = System.DateTime.Now.Date;
                   string strdate = date.Replace("-", "/");
                   DateTime lastdate = General.GetDateTimeFromText(strdate);
                    int diff = 0;
                    double daydiff = (tdydate - lastdate).TotalDays;
                    diff = (int)daydiff;
                    if (diff > duration)
                    {
                        dr["Status"] = "In Active";
                    }
                    else
                    {
                        dr["Status"] = "Active";
                    }
                }
                else
                {
                    dr["Status"] = "";
                }
                
                MyParentList.Tables["Parent"].Rows.Add(dr);
            }
        }
        return MyParentList;
    }

    private string GetSendStatus(int SiblingId, string StudentId, out string _CanLogin, out string _ParentId, out string _UserName, out string _lastlogin, out string date, out string emailId)
    {
        string Status = "0";
        _CanLogin = "0";
        _ParentId = "0";
        _UserName = "0";
        _lastlogin = "";
        date = "";
        emailId = ""; ;
        OdbcDataReader MyTempreader = null;
        string Subsql = " INNER JOIN tblparent_parentstudentmap ON tblparent_parentstudentmap.ParentId = tblparent_parentdetails.Id WHERE tblparent_parentstudentmap.StudentId=" + StudentId;
        if (SiblingId > 0)
        {
            Subsql = " WHERE tblparent_parentdetails.SiblingId=" + SiblingId;
        }
        string sql = "select tblparent_parentdetails.SentCredentials , tblparent_parentdetails.CanLogin , tblparent_parentdetails.Id, tblparent_parentdetails.UserName, tblparent_parentdetails.LastLogin, date_format(tblparent_parentdetails.LastLogin, '%d-%m-%Y') AS date ,GmailAuthId from tblparent_parentdetails " + Subsql;
        MyTempreader = m_MysqlDb.ExecuteQuery(sql);
        if (MyTempreader.HasRows)
        {
            Status = MyTempreader.GetValue(0).ToString();
            _CanLogin = MyTempreader.GetValue(1).ToString();
            _ParentId = MyTempreader.GetValue(2).ToString();
            _UserName = MyTempreader.GetValue(3).ToString();
            _lastlogin = MyTempreader.GetValue(4).ToString();
            emailId = MyTempreader["GmailAuthId"].ToString();
            date = MyTempreader.GetValue(5).ToString();
        }
        return Status;
    }

    public int GetSiblingId(string StudentId)
    {
        int SiblingId = 0;
        OdbcDataReader Siblingsreder = null;
        string sql = "select Id from tbl_siblingsmap where tbl_siblingsmap.StudId=" + StudentId;
        Siblingsreder = m_MysqlDb.ExecuteQuery(sql);
        if (Siblingsreder.HasRows)
        {
            int.TryParse(Siblingsreder.GetValue(0).ToString(), out SiblingId);
        }
        return SiblingId;
    }

    # endregion

    /*Dominic Dynmic */

    public OdbcDataReader GetReportFormat(string _ReportName)
    {
        string sql = "select tblreportformats.DefaultFormat from tblreportformats WHERE tblreportformats.Status=1 and tblreportformats.ReportName='" + _ReportName + "'";
        OdbcDataReader MyReader_Formates = m_MysqlDb.ExecuteQuery(sql);
        return MyReader_Formates;
    }
}
