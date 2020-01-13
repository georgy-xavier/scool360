using System;
using System.Web;
//using System.Web.Services;
//using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Data.Odbc;
using System.Data;
using System.Text;
using System.Collections;
using System.Linq;
using WinBase;


public class ExamManage:KnowinGen
{
    public MysqlClass m_MysqlDb;
    public MysqlClass m_TransationDb = null;
    private OdbcDataReader m_MyReader = null;
    private string m_ExamMenuStr;
    private string m_SubExamMenuStr;
    public bool NeedRank = true;
    public ExamManage(KnowinGen _Prntobj)
    {
        m_Parent = _Prntobj;
        m_MyODBCConn = m_Parent.ODBCconnection;
        m_UserName = m_Parent.LoginUserName;
        m_MysqlDb = new MysqlClass(this);
        m_ExamMenuStr = "";
        m_SubExamMenuStr = "";
        NeedRank = IsRankNeeded();
    }

    

    public ExamManage(MysqlClass _Msqlobj)
    {
        m_MysqlDb = _Msqlobj;
        m_ExamMenuStr = "";
        m_SubExamMenuStr = "";
    }
    ~ExamManage()
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
        CLogging logger = CLogging.GetLogObject();
        logger.LogToFile("CreateTansationDb", "Starting New Fee Transaction", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
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


    private bool IsRankNeeded()
    {
        bool _NeedRank = true;
        string sql = "SELECT tblconfiguration.Value FROM tblconfiguration WHERE tblconfiguration.Name='NeedRank'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int _value = 1;
            int.TryParse(m_MyReader.GetValue(0).ToString(), out _value);
            if (_value == 0)
            {
                _NeedRank = false;
            }
        }
        return _NeedRank;
    }

    public string GetExamMangMenuString(int _roleid)
    {
        CLogging logger = CLogging.GetLogObject();
        string _MenuStr;
        if (m_ExamMenuStr == "")
        {


            _MenuStr = "<ul><li><a href=\"ViewExams.aspx\">View Exams</a></li>";
            logger.LogToFile("GetExamMangMenuString", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            string sql = "SELECT DISTINCT tblaction.MenuName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND tblroleactionmap.ModuleId=3 AND tblaction.ActionType='Link' order by tblaction.`Order` Asc";
            logger.LogToFile("GetExamMangMenuString", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                logger.LogToFile("GetExamMangMenuString", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                while (m_MyReader.Read())
                {
                    	

                    _MenuStr = _MenuStr + "<li><a href=\""+m_MyReader.GetValue(1).ToString() +"\">"+m_MyReader.GetValue(0).ToString()+"</a></li>";
                }

            }
            _MenuStr = _MenuStr + "</ul>";
            logger.LogToFile("GetExamMangMenuString", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Close();
            m_ExamMenuStr = _MenuStr;
            
         
        }
        else
        {
            _MenuStr = m_ExamMenuStr;
        }
        logger.LogToFile("GetExamMangMenuString", " exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _MenuStr;
 
    }

    public int CreateExam(string _ExamName, int _ClassId,int _examtypeid)
    {
        CLogging logger = CLogging.GetLogObject();
        int _Examid = -1;
        string _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        try
        {
            logger.LogToFile("CreateExam", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            string sql = "INSERT INTO tblexam(ExamName,ClassId,CreationDate,TypeId) VALUES ('" + _ExamName + "'," + _ClassId + ",'" + _strdtNow + "'," + _examtypeid + ")";
            logger.LogToFile("CreateExam", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            logger.LogToFile("CreateExam", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            sql = "SELECT Id FROM tblexam where ExamName='" + _ExamName + "' AND CreationDate ='" + _strdtNow + "'";
            logger.LogToFile("CreateExam", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                logger.LogToFile("CreateExam", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader.Read();

                _Examid = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            else
            {
                logger.LogToFile("CreateExam", " Reading Failure " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                _Examid = -1;
            }
            logger.LogToFile("CreateExam", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Close();
        }
        catch (Exception exc)
        {
            logger.LogToFile("CreateExam", "throws Error" + exc.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);       
        }
        logger.LogToFile("CreateExam", " exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _Examid;
    }

    public int CreateExamMaster(string _ExamName, int _periodTypeId, int _examtypeid, string _createdBy)
    {
        CLogging logger = CLogging.GetLogObject();
        int _Examid = -1;
        int _status = 1;
        string _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        try
        {
            logger.LogToFile("CreateExam", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            string sql = "INSERT INTO tblexammaster(ExamName,ExamTypeId,PeriodTypeId,CreatedBy,CreatedDatetime,Status) VALUES ('" + _ExamName + "'," + _examtypeid + ", " + _periodTypeId + ",'" + _createdBy + "', '" + _strdtNow + "', " + _status + ")";
            logger.LogToFile("CreateExam", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            logger.LogToFile("CreateExam", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            sql = "SELECT Id FROM tblexammaster where ExamName='" + _ExamName + "' AND CreatedDatetime ='" + _strdtNow + "'";
            logger.LogToFile("CreateExam", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                logger.LogToFile("CreateExam", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader.Read();

                _Examid = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            else
            {
                logger.LogToFile("CreateExam", " Reading Failure " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                _Examid = -1;
            }
            logger.LogToFile("CreateExam", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Close();
        }
        catch (Exception exc)
        {
            logger.LogToFile("CreateExam", "throws Error" + exc.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
        }
        logger.LogToFile("CreateExam", " exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _Examid;
    }

    public string GetSubExamMangMenuString(int _roleid)
    {
        CLogging logger = CLogging.GetLogObject();
        string _MenuStr;
        if (m_SubExamMenuStr == "")
        {
            _MenuStr = "<ul class=\"block\"><li><a href=\"ExamDetails.aspx\">Exam Details</a></li>";
            logger.LogToFile("GetSubExamMangMenuString", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            string sql = "SELECT DISTINCT tblaction.MenuName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND tblroleactionmap.ModuleId=3 AND tblaction.ActionType='SubLink' order by tblaction.`Order` Asc";
            logger.LogToFile("GetSubExamMangMenuString", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                logger.LogToFile("GetSubExamMangMenuString", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                while (m_MyReader.Read())
                {
                    _MenuStr = _MenuStr + "<li><a href=\"" + m_MyReader.GetValue(1).ToString() + "\">" + m_MyReader.GetValue(0).ToString() + "</a></li>";
                }

            }
            _MenuStr = _MenuStr + "</ul>";
            logger.LogToFile("GetSubExamMangMenuString", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Close();
            m_SubExamMenuStr = _MenuStr;

        }
        else
        {
            _MenuStr = m_SubExamMenuStr;
        }
        logger.LogToFile("GetSubExamMangMenuString", " exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _MenuStr; 
    }

    public bool ExamSchduled(int _examid,int _batchId)
    {
        CLogging logger = CLogging.GetLogObject();
        bool _valide;
        logger.LogToFile("ExamSchduled", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "SELECT * FROM tblexamschedule where BatchId=" + _batchId + " AND ClassExamId=" + _examid;
        logger.LogToFile("ExamSchduled", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            logger.LogToFile("ExamSchduled", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            _valide = true;
        }
        else
        {
            logger.LogToFile("ExamSchduled", " Reading Failure " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            _valide = false;
        }
        logger.LogToFile("ExamSchduled", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader.Close();
        logger.LogToFile("ExamSchduled", " exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _valide;
        
    }

    public int ScheduleExam(int _examid, int _BatchId)
    {
        CLogging logger = CLogging.GetLogObject();
        int _ExamSchid = -1;
        string _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        try
        {
            logger.LogToFile("ScheduleExam", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            string sql = "INSERT INTO tblexamschedule(BatchId,ExamId,Status,ScheduledDate) VALUES (" + _BatchId + "," + _examid + ",'Scheduled','" + _strdtNow + "')";
            logger.LogToFile("ScheduleExam", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            logger.LogToFile("ScheduleExam", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            sql = "SELECT Id FROM tblexamschedule where BatchId=" + _BatchId + " AND ExamId=" + _examid + " AND ScheduledDate ='" + _strdtNow + "'";
            logger.LogToFile("ScheduleExam", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                logger.LogToFile("ScheduleExam", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader.Read();

                _ExamSchid = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            else
            {
                logger.LogToFile("ScheduleExam", " Reading Failure " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                _ExamSchid = -1;
            }
            logger.LogToFile("ExamSchduled", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Close();
        }
        catch (Exception exc)
        {
            logger.LogToFile("ExamSchduled", "throws Error" + exc.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);       
        }
        logger.LogToFile("ExamSchduled", " exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _ExamSchid;
    }

    public int ScheduleExamMaster(int _clsexamid, int _BatchId)
    {
        CLogging logger = CLogging.GetLogObject();
        int _ExamSchid = -1;
        string _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        try
        {
            logger.LogToFile("ScheduleExam", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            string sql = "INSERT INTO tblexamschedule(ClassExamId,BatchId,PeriodId,Status,ScheduledDate) VALUES (" + _clsexamid + ", " + _BatchId + ", 1, 'Scheduled', '" + _strdtNow + "')";
            logger.LogToFile("ScheduleExam", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            logger.LogToFile("ScheduleExam", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            sql = "SELECT Id FROM tblexamschedule where BatchId=" + _BatchId + " AND ClassExamId=" + _clsexamid + " AND ScheduledDate ='" + _strdtNow + "'";
            logger.LogToFile("ScheduleExam", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                logger.LogToFile("ScheduleExam", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader.Read();

                _ExamSchid = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            else
            {
                logger.LogToFile("ScheduleExam", " Reading Failure " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                _ExamSchid = -1;
            }
            logger.LogToFile("ExamSchduled", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Close();
        }
        catch (Exception exc)
        {
            logger.LogToFile("ExamSchduled", "throws Error" + exc.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
        }
        logger.LogToFile("ExamSchduled", " exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _ExamSchid;
    }

    

    public void ScheduleSubjects(int _ExSchId, int _subjectId, int _timeid, DateTime _examdate)
    {
        CLogging logger = CLogging.GetLogObject();        
        logger.LogToFile("ScheduleSubjects", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "INSERT INTO tblexammark(ScheduleId,SubId,TimeId,ExamDate) VALUES (" + _ExSchId + "," + _subjectId + "," + _timeid + ", '" + _examdate.ToString("s") + "')";
        logger.LogToFile("ScheduleSubjects", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        logger.LogToFile("ScheduleSubjects", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader.Close();
        logger.LogToFile("ScheduleSubjects", " Exiting from module  " , 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    }

    public int GetExamScheduleId(int _examid, int _BatchId, string _classid, string _periodid)
    {
        
        CLogging logger = CLogging.GetLogObject();
        int _ExamSchid;
        logger.LogToFile("GetExamScheduleId", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "select  tblexamschedule.Id,tblexamschedule.AggregateMark from tblexamschedule inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId where tblclassexam.ExamId=" + _examid + " AND tblclassexam.ClassId=" + _classid + " AND tblexamschedule.BatchId=" + _BatchId + " AND tblexamschedule.PeriodId=" + _periodid;
        logger.LogToFile("GetExamScheduleId", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            logger.LogToFile("GetExamScheduleId", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Read();
            _ExamSchid = int.Parse(m_MyReader.GetValue(0).ToString());
            //AggregateMark =Math.Round(double.Parse(m_MyReader.GetValue(1).ToString()),2);
        }
        else
        {
            logger.LogToFile("GetExamScheduleId", " Reading Failure " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            _ExamSchid = -1;
        }
        logger.LogToFile("GetExamScheduleId", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader.Close();
        logger.LogToFile("GetExamScheduleId", " Exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _ExamSchid;
    }

    public void UpdateScheduleSubjects(int _ExSchId, int _subjectId, string _ExamDate, string _StartTime, string _EndTime, double _Passmark, double _maxmark)
    {
        
        CLogging logger = CLogging.GetLogObject();
        DateTime Exam_Date = General.GetDateTimeFromText(_ExamDate);
        logger.LogToFile("UpdateScheduleSubjects", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "UPDATE tblexammark SET PassMark= " + _Passmark + ", MaxMark = " + _maxmark + ", ExamDate = '" + Exam_Date.Date.ToString("s") + "', StartTime ='" + _StartTime + "', EndTime = '" + _EndTime + "' WHERE ExamSchId =" + _ExSchId + " AND SubjectId=" + _subjectId;
        logger.LogToFile("UpdateScheduleSubjects", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        logger.LogToFile("UpdateScheduleSubjects", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader.Close();
        logger.LogToFile("UpdateScheduleSubjects", " Exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    }

    public bool AllStudHaveRollNo(int _examid ,int _batchid)
    {
        CLogging logger = CLogging.GetLogObject();
        bool _valide;
        logger.LogToFile("AllStudHaveRollNo", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "select tblstudentclassmap.StudentId from  tblstudentclassmap inner JOIN tblexam on tblexam.ClassId=tblstudentclassmap.ClassId inner join tblstudent on tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudentclassmap.BatchId=" + _batchid + " and tblstudentclassmap.RollNo=-1 AND tblstudent.Status=1 AND tblexam.Id=" + _examid;
        logger.LogToFile("AllStudHaveRollNo", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            logger.LogToFile("AllStudHaveRollNo", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            _valide = false;
        }
        else
        {
            logger.LogToFile("AllStudHaveRollNo", " Reading Failure " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            _valide = true;
        }
        logger.LogToFile("AllStudHaveRollNo", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader.Close();
        logger.LogToFile("AllStudHaveRollNo", " Exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _valide;
    }

    public string GetExamName(int _examId)
    {
        CLogging logger = CLogging.GetLogObject();
        string _ExamName;
        logger.LogToFile("GetExamName", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "SELECT ExamName FROM tblexammaster where Id=" + _examId;
        logger.LogToFile("GetExamName", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            logger.LogToFile("GetExamName", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Read();

            _ExamName = m_MyReader.GetValue(0).ToString();

        }
        else
        {
            logger.LogToFile("GetExamName", " Reading Failure " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            _ExamName = "";
        }
        logger.LogToFile("GetExamName", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader.Close();
        logger.LogToFile("GetExamName", " Exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _ExamName;
       
    }

    public string GetMarks(int _studId, int ExamId, int _batchId, string _column, string _classid,string _periodid)
    {
        CLogging logger = CLogging.GetLogObject();
        string _ExamMark;
        logger.LogToFile("GetMarks", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "Select tblstudentmark." + _column + " From tblstudentmark inner join tblexamschedule on tblexamschedule.Id=tblstudentmark.ExamSchId inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId  where tblexamschedule.PeriodId=" + _periodid + " AND tblexamschedule.BatchId=" + _batchId + " And tblclassexam.ClassId=" + _classid + " And tblclassexam.ExamId=" + ExamId + " AND tblstudentmark.StudId=" + _studId;
        logger.LogToFile("GetMarks", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            logger.LogToFile("GetMarks", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Read();
            _ExamMark = m_MyReader.GetValue(0).ToString();
        }
        else
        {
            logger.LogToFile("GetMarks", " Reading Failure " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            _ExamMark = "";
        }
        logger.LogToFile("GetMarks", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader.Close();
        logger.LogToFile("GetMarks", " Exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _ExamMark;

    }

    //public string GetMarks(int _studId, int ExamId, int _batchId, string _column)
    //{
    //    CLogging logger = CLogging.GetLogObject();
    //    string _ExamMark;
    //    logger.LogToFile("GetMarks", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
    //    string sql = "Select tblstudentmark." + _column + " From tblstudentmark inner join tblexamschedule on tblexamschedule.Id=tblstudentmark.ExamSchId where tblexamschedule.BatchId=" + _batchId + " And tblexamschedule.ExamId=" + ExamId + " AND tblstudentmark.StudId=" + _studId;
    //    logger.LogToFile("GetMarks", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    //    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    //    if (m_MyReader.HasRows)
    //    {
    //        logger.LogToFile("GetMarks", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    //        m_MyReader.Read();

    //        _ExamMark = m_MyReader.GetValue(0).ToString();

    //    }
    //    else
    //    {
    //        logger.LogToFile("GetMarks", " Reading Failure " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    //        _ExamMark = "";
    //    }
    //    logger.LogToFile("GetMarks", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    //    m_MyReader.Close();
    //    logger.LogToFile("GetMarks", " Exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    //    return _ExamMark;

    //}

    public void RemoveEnteredMarks(string _classid, string periodid, int _BatchId, int _Exam_id)
    {
        CLogging logger = CLogging.GetLogObject();
        int Scexam_id = -1;
        try
        {
            logger.LogToFile("RemoveEnteredMarks", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);


            string sql = "select  tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId where tblclassexam.ExamId=" + _Exam_id + " AND tblclassexam.ClassId=" + _classid + " AND tblexamschedule.BatchId=" + _BatchId + " AND tblexamschedule.PeriodId=" + periodid;
            logger.LogToFile("RemoveEnteredMarks", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            if (m_TransationDb == null)
            {
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            }
            else
            {
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            }
            if (m_MyReader.HasRows)
            {
                logger.LogToFile("FillMarks", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader.Read();
                Scexam_id = int.Parse(m_MyReader.GetValue(0).ToString());
                logger.LogToFile("FillMarks", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                sql = "DELETE from tblstudentmark where ExamSchId=" + Scexam_id;
                logger.LogToFile("RemoveEnteredMarks", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                if (m_TransationDb == null)
                {
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                }
                else
                {
                    m_MyReader = m_TransationDb.ExecuteQuery(sql);
                }
            }
        }
        catch (Exception exc)
        {
            logger.LogToFile("RemoveEnteredMarks", "throws Error" + exc.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
        }
        logger.LogToFile("RemoveEnteredMarks", " Exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName); 
             
    }



    public void FillMarks(string _classid,string periodid, int _BatchId,int _Stud_id, int _Exam_id, string _Mark_column, Double _mark)
    {
        CLogging logger = CLogging.GetLogObject();
        int Scexam_id=-1;
        try
        {
            logger.LogToFile("FillMarks", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);


            string sql = "select  tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId where tblclassexam.ExamId=" + _Exam_id + " AND tblclassexam.ClassId=" + _classid + " AND tblexamschedule.BatchId=" + _BatchId + " AND tblexamschedule.PeriodId=" + periodid;
            logger.LogToFile("FillMarks", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            if (m_TransationDb == null)
            {
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            }
            else
            {
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            }
            if (m_MyReader.HasRows)
            {
                logger.LogToFile("FillMarks", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader.Read();
                Scexam_id = int.Parse(m_MyReader.GetValue(0).ToString());
                logger.LogToFile("FillMarks", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                sql = "SELECT Id from tblstudentmark where ExamSchId=" + Scexam_id + " and StudId=" + _Stud_id;
                logger.LogToFile("FillMarks", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                if (m_TransationDb == null)
                {
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                }
                else
                {
                    m_MyReader = m_TransationDb.ExecuteQuery(sql);
                }

                if (m_MyReader.HasRows)
                {
                    logger.LogToFile("FillMarks", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                    logger.LogToFile("FillMarks", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                    sql = "UPDATE tblstudentmark   SET " + _Mark_column + "=" + _mark + " WHERE ExamSchId =" + Scexam_id + " AND Studid=" + _Stud_id;
                    logger.LogToFile("FillMarks", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                    if (m_TransationDb == null)
                    {
                        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                    }
                    else
                    {
                        m_MyReader = m_TransationDb.ExecuteQuery(sql);
                    }
                }
                else
                {
                    logger.LogToFile("FillMarks", " Reading Failure " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                    logger.LogToFile("FillMarks", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                    sql = "insert into tblstudentmark (ExamSchId,StudId," + _Mark_column + ") VALUES (" + Scexam_id + "," + _Stud_id + "," + _mark + ")";
                    logger.LogToFile("FillMarks", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                    if (m_TransationDb == null)
                    {
                        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                    }
                    else
                    {
                        m_MyReader = m_TransationDb.ExecuteQuery(sql);
                    }
                }
            }
        }
        catch (Exception exc)
        {
            logger.LogToFile("FillMarks", "throws Error" + exc.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);       
        }
        logger.LogToFile("FillMarks", " Exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName); 
             
    }
    public void FillMarksAB(string _classid, string periodid, int _BatchId, int _Stud_id, int _Exam_id, string _Mark_column, string _mark)
    {
        CLogging logger = CLogging.GetLogObject();
        int Scexam_id = -1;
        try
        {
            logger.LogToFile("FillMarks", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);


            string sql = "select  tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId where tblclassexam.ExamId=" + _Exam_id + " AND tblclassexam.ClassId=" + _classid + " AND tblexamschedule.BatchId=" + _BatchId + " AND tblexamschedule.PeriodId=" + periodid;
            logger.LogToFile("FillMarks", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            if (m_TransationDb == null)
            {
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            }
            else
            {
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            }
            if (m_MyReader.HasRows)
            {
                logger.LogToFile("FillMarks", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader.Read();
                Scexam_id = int.Parse(m_MyReader.GetValue(0).ToString());
                logger.LogToFile("FillMarks", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                sql = "SELECT Id from tblstudentmark where ExamSchId=" + Scexam_id + " and StudId=" + _Stud_id;
                logger.LogToFile("FillMarks", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                if (m_TransationDb == null)
                {
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                }
                else
                {
                    m_MyReader = m_TransationDb.ExecuteQuery(sql);
                }

                if (m_MyReader.HasRows)
                {
                    logger.LogToFile("FillMarks", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                    logger.LogToFile("FillMarks", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                    sql = "UPDATE tblstudentmark   SET " + _Mark_column + "='" + _mark + "' WHERE ExamSchId =" + Scexam_id + " AND Studid=" + _Stud_id;
                    logger.LogToFile("FillMarks", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                    if (m_TransationDb == null)
                    {
                        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                    }
                    else
                    {
                        m_MyReader = m_TransationDb.ExecuteQuery(sql);
                    }
                }
                else
                {
                    logger.LogToFile("FillMarks", " Reading Failure " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                    logger.LogToFile("FillMarks", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                    sql = "insert into tblstudentmark (ExamSchId,StudId," + _Mark_column + ") VALUES (" + Scexam_id + "," + _Stud_id + ",'" + _mark + "')";
                    logger.LogToFile("FillMarks", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                    if (m_TransationDb == null)
                    {
                        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                    }
                    else
                    {
                        m_MyReader = m_TransationDb.ExecuteQuery(sql);
                    }
                }
            }
        }
        catch (Exception exc)
        {
            logger.LogToFile("FillMarks", "throws Error" + exc.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
        }
        logger.LogToFile("FillMarks", " Exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);

    }
    public string GetConfigValue(string _Name,string _Module,out string _SubValue)
    {
        _SubValue = "";
        OdbcDataReader newreader1;
        string _value = "";
        string sql = "SELECT tblconfiguration.Value,tblconfiguration.SubValue FROM tblconfiguration WHERE tblconfiguration.Module='" + _Module + "' AND tblconfiguration.Name='" + _Name + "'";
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
            _SubValue = newreader1.GetValue(1).ToString();
        }
        return _value;
    }

    public void UpdateResult(int _studId, int _examscid, double _Max_Total, double _Total, int _SubCount, string _Result, DataSet _aggregatepassDs, int _scheduledid, int _Examid, int _classid,int _batchid,string _examperiod)
    {
        CLogging logger = CLogging.GetLogObject();
        double _Avg;
        string _StudGrade = "";
        string Currentresult = "";
        logger.LogToFile("UpdateResult", "Calculating Average", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        _Avg = (_Total * 100) / _Max_Total;
        logger.LogToFile("UpdateResult", "Calculating grade and result", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);

        int _GradeMasterId = GetGradeMaster(_examscid);

        string sqlstr = "SELECT Grade,LowerLimit,Result FROM tblgrade WHERE `Status`=1 AND GradeMasterId=" + _GradeMasterId + " ORDER BY LowerLimit DESC";
        m_MyReader = m_MysqlDb.ExecuteQuery(sqlstr);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                string _Grade = m_MyReader.GetValue(0).ToString();
                double _Lowerlimit = 0;
                double.TryParse(m_MyReader.GetValue(1).ToString(), out _Lowerlimit);
                string t_Result = m_MyReader.GetValue(2).ToString();
                if (_Avg >= _Lowerlimit)
                {
                    logger.LogToFile("UpdateResult", "Calculating grade and result", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                    _StudGrade = _Grade;
                    Currentresult = t_Result;
                    break;
                }
            }
        }

        if (_StudGrade == "")
        {
            logger.LogToFile("UpdateResult", "Setting Failed", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            _StudGrade = "F";
            Currentresult = "Not Found";
        }

        //if (_Result != "Failed") Arun modified on 05-12-11
        //{
        //    _Result = Currentresult;
        //}
        //else
        //{
        //    _Result = "Failed";
        //}

        if (_Result == "Passed")
        {
            _Result = Currentresult;
        }
        //if (_Result != "Failed")
        //    if (aggregatemark > 0)
        //    {
        //        if (_Total < aggregatemark)
        //            _Result = "Failed";

        //    }

        #region Check aggregate wise pass mark

        double minmark = 0, maxmark = 0;
        bool aggregateresult = true;
        double studentAggregateMark = 0, Aggregatemark = 0;
        if (_aggregatepassDs.Tables[0].Rows.Count > 0)
        {
            string m_sql = "SELECT tblexammark.SubjectId,tblexammark.MarkColumn,tblsubjects.subject_name FROM tblexammark inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblsubjects on tblsubjects.Id=tblexammark.SubjectId  WHERE tblexamschedule.Id=" + _scheduledid + " order by tblexammark.SubjectOrder";
            DataSet Aggregateds = m_MysqlDb.ExecuteQueryReturnDataSet(m_sql);
            if (Aggregateds.Tables[0].Rows.Count > 0)
            {

                #region firsh check all subject mark is above of minpass marks or not

                string _Str_Mark = "";
                int subjectid = 0;
                double studentmark = 0, minpassmark = 0;

                //configuration subject
                foreach (DataRow dr1 in Aggregateds.Tables[0].Rows)
                {
                    subjectid = int.Parse(dr1[0].ToString());
                    _Str_Mark = GetMarks(_studId, _Examid, _batchid, dr1[1].ToString(), _classid.ToString(), _examperiod);
                    double.TryParse(_Str_Mark, out studentmark);

                    foreach (DataRow dr2 in _aggregatepassDs.Tables[0].Rows)
                    {
                        double.TryParse(dr2[1].ToString(), out minpassmark);
                        if (subjectid == int.Parse(dr2[0].ToString()))
                        {
                            studentAggregateMark += studentmark;
                            if (minpassmark > studentmark)
                                aggregateresult = false;
                        }
                    }
                }

                //un configuration subject

               
                int[] configsub = new int[_aggregatepassDs.Tables[0].Rows.Count];
                int i = 0;
                foreach (DataRow dr2 in _aggregatepassDs.Tables[0].Rows)
                {
                    configsub[i] = int.Parse(dr2[0].ToString());
                    i++;

                }
                foreach (DataRow dr1 in Aggregateds.Tables[0].Rows)
                {
                    subjectid = int.Parse(dr1[0].ToString());
                    _Str_Mark = GetMarks(_studId, _Examid, _batchid, dr1[1].ToString(), _classid.ToString(), _examperiod);
                    double.TryParse(_Str_Mark, out studentmark);
                    if (!configsub.Contains(subjectid))
                    {
                        GetMaxAndMinMark(Convert.ToString(_classid), Convert.ToString(_Examid), dr1[0].ToString(), out maxmark, out minmark);
                        if (minmark > studentmark)
                            aggregateresult = false;
                    }


                }
                #endregion

                #region calculate aggregate result

                if (aggregateresult)
                {
                    double MaxaggregateMark = 0;
                    foreach (DataRow dr1 in Aggregateds.Tables[0].Rows)
                    {
                        foreach (DataRow dr2 in _aggregatepassDs.Tables[0].Rows)
                        {
                            if (int.Parse(dr1[0].ToString()) == int.Parse(dr2[0].ToString()))
                            {
                                GetMaxAndMinMark(Convert.ToString(_classid), Convert.ToString(_Examid), dr1[0].ToString(), out maxmark, out minmark);
                                MaxaggregateMark += minmark;
                            }
                        }
                    }
                    if (MaxaggregateMark <= studentAggregateMark)
                        _Result = "Passed";
                    else
                        _Result = "Failed";

                }

                #endregion

            }

        }

        #endregion

        logger.LogToFile("UpdateResult", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "UPDATE tblstudentmark   SET TotalMark=" + _Total + ",TotalMax=" + _Max_Total + ",Avg=" + _Avg + ",Grade='" + _StudGrade + "',Result='" + _Result + "' WHERE ExamSchId =" + _examscid + "  AND Studid=" + _studId;
        logger.LogToFile("UpdateResult", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        logger.LogToFile("UpdateResult", " Exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);

    }

    public int GetGradeMaster(int _examscid)
    {
        int _GradeMasterId = 0;

        string sqlstr = "SELECT GradeMasterId FROM tblexamschedule WHERE Id=" + _examscid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sqlstr);
        if (m_MyReader.HasRows)
        {
            int.TryParse(m_MyReader.GetValue(0).ToString(), out _GradeMasterId);
        }

        return _GradeMasterId;
    }

    private string GetGradeResult(string _grade)
    {
        string _Result;
        string sql = "SELECT tblgrade.Result FROM tblgrade where tblgrade.Grade='" + _grade + "'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            _Result = m_MyReader.GetValue(0).ToString();
        }
        else
        {
            _Result = "";
        }
        m_MyReader.Close();
        return _Result;
    }

    private bool GradeExist(string _grade)
    {
        bool _valide;
        string sql = "SELECT tblgrade.LowerLimit FROM tblgrade where tblgrade.Status=1 And tblgrade.Grade='" + _grade + "'";
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

    private double GetLowerLimitOfGrade(string _grade)
    {
        double _limit ;
        string sql = "SELECT tblgrade.LowerLimit FROM tblgrade where tblgrade.Grade='" + _grade+"'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            _limit = double.Parse(m_MyReader.GetValue(0).ToString());
        }
        else
        {
            _limit = 100;
        }
        m_MyReader.Close();
        return _limit;
    }

    public void UpdateRemark(string _Remark, int _scheduledid, int _StudId)
    {
        CLogging logger = CLogging.GetLogObject();
        logger.LogToFile("UpdateRemark", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql ="UPDATE tblstudentmark   SET Remark='"+_Remark + "' WHERE ExamSchId =" + _scheduledid + " AND Studid=" + _StudId;
        logger.LogToFile("UpdateRemark", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        logger.LogToFile("UpdateRemark", " Exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName); 
    }

    public void UpdateRank(int i, int ExScID, int _studid)
    {
        CLogging logger = CLogging.GetLogObject();
        string sql;
        logger.LogToFile("UpdateRank", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        sql = "UPDATE tblstudentmark SET Rank=" + i + " WHERE ExamSchId =" + ExScID + "  AND Studid=" + _studid;
        logger.LogToFile("UpdateRank", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        logger.LogToFile("UpdateRank", " Exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName); 
    }

    public void UpdateExamSchedule(string _classid, string _periodid, int ExamID, int _BatchId,string status)
    {
        CLogging logger = CLogging.GetLogObject();
        string sql;
        logger.LogToFile("UpdateExamSchedule", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        sql = "select  tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId where tblclassexam.ExamId=" + ExamID + " AND tblclassexam.ClassId=" + _classid + " AND tblexamschedule.BatchId=" + _BatchId + " AND tblexamschedule.PeriodId=" + _periodid ;
        if (m_TransationDb == null)
        {
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }
        else
        {
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
        }
        if (m_MyReader.HasRows)
        {

           string _examschdulId = m_MyReader.GetValue(0).ToString();
           sql = "UPDATE tblexamschedule SET Status='" + status + "' WHERE Id =" + _examschdulId;
           logger.LogToFile("UpdateExamSchedule", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
           if (m_TransationDb == null)
           {
               m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           }
           else
           {
               m_MyReader = m_TransationDb.ExecuteQuery(sql);
           }
           logger.LogToFile("UpdateExamSchedule", " Exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName); 
        }
       
    }

    public void UpdateExamStatus(int _ExamId)
    {
        CLogging logger = CLogging.GetLogObject();
        string sql;
        logger.LogToFile("UpdateExamStatus", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        sql = "UPDATE tblexammaster SET Status=0 WHERE Id =" + _ExamId;
        logger.LogToFile("UpdateExamStatus", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        logger.LogToFile("UpdateExamStatus", " Exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName); 
    }

    public void DeleteExam(int _ExamId)
    {
        CLogging logger = CLogging.GetLogObject();
        MysqlClass MysqlTranDb;
        MysqlTranDb = new MysqlClass(this);
        string sql;
        try
        {
            sql = "select tblclassexam.Id from tblclassexam where tblclassexam.ExamId=" + _ExamId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            logger.LogToFile("DeleteExam", "transaction begins", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
            MysqlTranDb.MyBeginTransaction();
            logger.LogToFile("DeleteExam", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            while (m_MyReader.Read())
            {
                sql = "Delete From tblclassexamsubmap where ClassExamId=" + m_MyReader.GetValue(0).ToString();
                logger.LogToFile("DeleteExam", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                MysqlTranDb.TransExecuteQuery(sql);
              
            }

            sql = "Delete From tblclassexam where ExamId=" + _ExamId;
            MysqlTranDb.TransExecuteQuery(sql);
            logger.LogToFile("DeleteExam", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            sql = "Delete From tblexammaster where Id=" + _ExamId;
            logger.LogToFile("DeleteExam", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            MysqlTranDb.TransExecuteQuery(sql);
            logger.LogToFile("DeleteExam", "transaction commits", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
            MysqlTranDb.TransactionCommit();
        }
        catch (Exception exc)
        {
            MysqlTranDb.TransactionRollback();
            logger.LogToFile("DeleteExam", "throws Error " + exc.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);       
        }
        logger.LogToFile("UpdateExamStatus", " Exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);       
    }

    public void CreateExamType(string _subTypeName, string _subTypeDesc)
    {
        CLogging logger = CLogging.GetLogObject();
        logger.LogToFile("CreateSubType", "user is sending Request to INSERT INTO tblsubject_type  ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "INSERT INTO tblsubject_type(sbject_type,TypeDisc) VALUES ('" + _subTypeName + "','" + _subTypeDesc + "')";
        logger.LogToFile("CreateSubType", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        logger.LogToFile("CreateSubType", "Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader.Close();

    }

    public bool DeleteExamTypeById(int _examTypeID, out string _message)
    {
        CLogging logger = CLogging.GetLogObject();
        logger.LogToFile("DeleteSubjectTypeById", "user is sending Request to select sub_Catagory from tblsubjects  ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "select ExamTypeId from tblexammaster where ExamTypeId=" + _examTypeID;
        logger.LogToFile("DeleteSubjectTypeById", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        m_MyReader.Read();
        if (m_MyReader.HasRows)
        {
            logger.LogToFile("DeleteSubjectTypeById", " Reading Success ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            _message = "Exam type is assigned to some Exams";
            logger.LogToFile("DeleteSubjectTypeById", " returning false ", 'I', CLogging.PriorityEnum.LEVEL_IMPORTANT, LoginUserName);
            return false;
        }
        else
        {
            logger.LogToFile("DeleteSubjectTypeById", "user is sending Request to Delete From tblsubject_type  ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            sql = "Delete From tblsubject_type where Id=" + _examTypeID;
            logger.LogToFile("DeleteSubjectTypeById", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            _message = "Exam type is deleted";
            logger.LogToFile("DeleteSubjectTypeById", "returning true and exists from module ", 'I', CLogging.PriorityEnum.LEVEL_IMPORTANT, LoginUserName);
            return true;
        }
    }

    public bool ExamExist(string _examname, int _Examtyp, int _exampiodid)
    {
        bool _valide;
        string sql = "SELECT * FROM tblexammaster where ExamName='" + _examname + "' AND Status=1 AND ExamTypeId=" + _Examtyp + " AND PeriodTypeId=" + _exampiodid;
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

    public string ExamType(int _examId)
    {
        string _examtype = "";
        string sql = "SELECT tblsubject_type.sbject_type FROM tblexam inner join tblsubject_type on tblsubject_type.Id=tblexam.TypeId where tblexam.Id=" + _examId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            _examtype = m_MyReader.GetValue(0).ToString(); ;
        }
       
        m_MyReader.Close();
        return _examtype;

    }

    public void ReportIncident(int _StudentId, string _Rank, int _ExamId, double _Total, string _MaxMark, string _Grade, string _Result)
    {
        string _ExamName = GetExamName(_ExamId);
        string _Desc = " Secured " + _Total + " marks out of " + _MaxMark + " for the " + _ExamName + " Exam.The result is " + _Result + " and the grade is " + _Grade + " .Rank obtained is " + _Rank + ".";
        //string _Desc = "Seccued " + _Rank + " Rank for the " + _ExamName + " Exam";

        General _GenObj = new General(m_MysqlDb);
        int _Incedentid = _GenObj.GetTableMaxId("tblview_incident", "Id");
        DateTime _Now = System.DateTime.Now;
        string sql = " insert into tblincedent (Id,Title,Description,IncedentDate,CreatedDate,TypeId,ApproverId,CreatedUserId,UserType,Status,AssoUser)values ("+_Incedentid+",'Class Rank','" + _Desc + "','" + _Now.ToString("s") + "','" + _Now.ToString("s") + "',1,0,1,'student','Approved'," + _StudentId + " )";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    }

    public bool IsAnyReportGenerated()
    {
        bool _valid = false;
        string sql = "Select Id from tblstudentmark";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _valid = true;
        }
        return _valid;
    }

    public int GetExamCount(int _ExamId)
    {
        int Count = 0;
        string sql = "select count(tblexammark.ExamSchId) from tblexammark inner join tblexamschedule on tblexamschedule.Id = tblexammark.ExamSchId inner join tblexam on tblexam.Id = tblexamschedule.ExamId where tblexam.Id = " + _ExamId + " ";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Count = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        return Count;
    }

    public string GetMarkfromColumn(int _studId, int ExamId, int _batchId, string _column)
    {
        CLogging logger = CLogging.GetLogObject();
        string _ExamMark;
        logger.LogToFile("GetMarkfromColumn", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "Select tblstudentmark." + _column + " From tblstudentmark inner join tblexamschedule on tblexamschedule.Id=tblstudentmark.ExamSchId where tblexamschedule.BatchId=" + _batchId + " And tblexamschedule.Id=" + ExamId + " AND tblstudentmark.StudId=" + _studId;
        logger.LogToFile("GetMarkfromColumn", "Executing Query", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            logger.LogToFile("GetMarkfromColumn", "Reading Success", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            m_MyReader.Read();
            _ExamMark = m_MyReader.GetValue(0).ToString();

        }
        else
        {
            logger.LogToFile("GetMarkfromColumn", "Reading Failure", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            _ExamMark = "-1";
        }
        logger.LogToFile("GetMark", "closing myReader and  returning exammark and Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader.Close();
        return _ExamMark;

    }

    public int getScheduleId(int _ExamID)
    {
        string sql = "select Id from tblexamschedule where ClassExamId=" + _ExamID + "";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        int ExamID=-1;
        if (m_MyReader.HasRows)
        {
            ExamID = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        return ExamID;
    }

    public int CreateClassExamMaster(int _ClassId, int _examID, string _createdBy)
    {
        int classexamid = -1;
        string _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        CLogging logger = CLogging.GetLogObject();
        try
        {
            logger.LogToFile("CreateClassExamMaster", "user is sending Request to INSERT INTO tblclassexam  ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            string sql = "INSERT INTO tblclassexam(ExamId,ClassId,CreatedDatetime,CreatedBy,Status) VALUES (" + _examID + "," + _ClassId + ", '" + _strdtNow + "', '" + _createdBy + "', 1)";
            logger.LogToFile("CreateClassExamMaster", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            logger.LogToFile("CreateClassExamMaster", "Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Close();
            sql = "SELECT Id FROM tblclassexam WHERE ExamId=" + _examID + " AND CreatedDatetime ='" + _strdtNow + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                logger.LogToFile("CreateClassExamMaster", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader.Read();
                classexamid = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            else
            {
                logger.LogToFile("CreateClassExamMaster", " Reading Failure " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                classexamid = -1;
            }
            logger.LogToFile("CreateClassExamMaster", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Close();
            //return classexamid;
        }
        catch (Exception exc)
        {
            logger.LogToFile("CreateClassExamMaster", "throws Error" + exc.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
        }
        logger.LogToFile("CreateClassExamMaster", " exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return classexamid;
    }

    public void CreateClassExam(int _classexamId, int _SubjectId, double _Max, double _Min)
    {
        CLogging logger = CLogging.GetLogObject();
        try
        {
            logger.LogToFile("CreateClassExamSubMap", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            string sql = "INSERT INTO tblclassexamsubmap(ClassExamId,SubId,MinMark,MaxMark) VALUES (" + _classexamId + "," + _SubjectId + ", " + _Min + "," + _Max + ")";
            logger.LogToFile("CreateClassExamSubMap", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }
        catch (Exception exc)
        {
            logger.LogToFile("CreateClassExamSubMap", "throws Error" + exc.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
        }        
    }    

    public void DeleteClassExam(int classexamId)
    {
        string sql = "DELETE FROM tblclassexamsubmap WHERE ClassExamId=" + classexamId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    }

    public void DeleteClassExamMaster(int classexamId)
    {
        string sql = "DELETE FROM tblclassexam WHERE Id=" + classexamId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    }

    public void UpdateClassExam(int classexamId, int _SubjectId, float _Max, float _Min)
    {
        string sql = "UPDATE tblclassexamsubmap SET MaxMark=" + _Max + ", MinMark=" + _Min + " WHERE ClassExamId=" + classexamId + " AND SubId=" + _SubjectId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    }

    //public string LoadExamDetails(int p)
    //{
    //   string name="";
    //   string sql="select ExamName from tblexammaster where Id=" + p +"";
    //   m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    //   name = m_MyReader.GetValue(0).ToString();
    //   return name;
    //}

    public void GetExamDetail(int _examid, out string ExamName, out string Examtypes, out string FreqType)
    {
        ExamName = "";
        Examtypes = "";
        FreqType = "";

        string sql = "select tblexammaster.ExamName , tblsubject_type.sbject_type , tblfrequency.Name from tblexammaster inner join tblsubject_type on tblsubject_type.Id= tblexammaster.ExamTypeId inner join tblfrequency on tblfrequency.Id = tblexammaster.PeriodTypeId where tblexammaster.Id=" + _examid + "";

        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            ExamName = m_MyReader.GetValue(0).ToString() + " Examination";
            Examtypes = m_MyReader.GetValue(1).ToString();
            FreqType = m_MyReader.GetValue(2).ToString();
        }
    }

    public DataSet MyAssociatedExamClass(int _userid, int examid)
    {
        DataSet _myAssoFeeClass;
        string sql = "SELECT tblclass.Id, tblclass.ClassName FROM tblclass INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id  WHERE tblclass.Id IN (SELECT tblclassexam.ClassId FROM tblclassexam WHERE tblclassexam.ExamId=" + examid + ") AND tblclass.Id IN  (SELECT tblclass.Id from tblclass  where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _userid + ")) ORDER BY tblstandard.Id, tblclass.Id";
        _myAssoFeeClass = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return _myAssoFeeClass;
    }

    public bool ExamSchduled(int _examid, int _batchid, string _classid, string _periodid)
    {
        CLogging logger = CLogging.GetLogObject();
        bool _valide;
        logger.LogToFile("ExamSchduled", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "select  tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId where tblclassexam.ExamId=" + _examid + " AND tblclassexam.ClassId=" + _classid + " AND tblexamschedule.BatchId=" + _batchid + " AND tblexamschedule.PeriodId=" + _periodid;
        logger.LogToFile("ExamSchduled", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            logger.LogToFile("ExamSchduled", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            _valide = true;
        }
        else
        {
            logger.LogToFile("ExamSchduled", " Reading Failure " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            _valide = false;
        }
        logger.LogToFile("ExamSchduled", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader.Close();
        logger.LogToFile("ExamSchduled", " exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _valide;
    }

    public void GetMaxAndMinMark(string _classid, string _examid, string _subjectid, out double _maxmark, out double _minmark)
    {
        string sql = "SELECT tblclassexamsubmap.MaxMark, tblclassexamsubmap.MinMark from tblclassexam inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblclassexam.Id where tblclassexamsubmap.SubId=" + _subjectid + " AND tblclassexam.ClassId=" + _classid + " AND tblclassexam.ExamId=" + _examid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _maxmark = double.Parse(m_MyReader.GetValue(0).ToString());
            _minmark = double.Parse(m_MyReader.GetValue(1).ToString());

        }
        else
        {
            _maxmark = 0;
            _minmark = 0;
        }
    }

    public string ExamSchduledState(int _examid, int _batchid, string _classid, string _periodid)
    {
        
        string _status;
      
        string sql = "select  tblexamschedule.Status from tblexamschedule inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId where tblclassexam.ExamId=" + _examid + " AND tblclassexam.ClassId=" + _classid + " AND tblexamschedule.BatchId=" + _batchid + " AND tblexamschedule.PeriodId=" + _periodid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            _status = m_MyReader.GetValue(0).ToString();
        }
        else
        {

            _status = "Exam Not Scheduled";
        }
        
        m_MyReader.Close();

        return _status;
    }

    public int ScheduleExamMaster(int _clsexamid, int _BatchId, int _periodId, int GradeId, DateTime _ScheduledDate)
    {
        CLogging logger = CLogging.GetLogObject();
        int _ExamSchid = -1;
       // string _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string _strdtNow=_ScheduledDate.ToString("yyyy-MM-dd HH:mm:ss");
        try
        {
            General MyGenObj = new General(m_MysqlDb);
            int _MaxId = MyGenObj.GetTableMaxId("tblview_examschedule", "ScheduleId");
           // string sql = "INSERT INTO tblexamschedule(Id,ClassExamId,BatchId,PeriodId,Status,ScheduledDate,GradeMasterId) VALUES (" + _MaxId + "," + _clsexamid + ", " + _BatchId + ", " + _periodId + ", 'Scheduled', '" + _strdtNow + "'," + GradeId + ")";
            string sql = "INSERT INTO tblexamschedule(Id,ClassExamId,BatchId,PeriodId,Status,ScheduledDate,GradeMasterId) VALUES (" + _MaxId + "," + _clsexamid + ", " + _BatchId + ", " + _periodId + ", 'Scheduled', '" + _strdtNow + "'," + GradeId + ")";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            _ExamSchid = _MaxId;
            logger.LogToFile("ExamSchduled", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Close();
        }
        catch (Exception exc)
        {
            logger.LogToFile("ExamSchduled", "throws Error" + exc.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
        }
        logger.LogToFile("ExamSchduled", " exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _ExamSchid;
    }

    public void ScheduleSubjects(int _ExSchId, string _columnname, int _subjectId, int _timeid, DateTime _examdate, int SubOrder)
    {
        CLogging logger = CLogging.GetLogObject();        
        logger.LogToFile("ScheduleSubjects", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "INSERT INTO tblexammark(ExamSchId,MarkColumn,SubjectId,TimeSlotId,ExamDate,SubjectOrder) VALUES (" + _ExSchId + ",'" + _columnname + "', " + _subjectId + "," + _timeid + ", '" + _examdate.Date.ToString("s") + "'," + SubOrder + ")";
        logger.LogToFile("ScheduleSubjects", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        logger.LogToFile("ScheduleSubjects", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader.Close();
        logger.LogToFile("ScheduleSubjects", " Exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    }
   
    public void UpdateScheduleSubjects(int _ExSchId, int _subjectId, int _timeid, DateTime _examdate, int SubOrder)
    {
        CLogging logger = CLogging.GetLogObject();
        logger.LogToFile("UpdateScheduleSubjects", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "UPDATE tblexammark SET ExamDate = '" + _examdate.Date.ToString("s") + "', TimeSlotId =" + _timeid + ",SubjectOrder=" + SubOrder + " WHERE ExamSchId =" + _ExSchId + " AND SubjectId=" + _subjectId + " order by tblexammark.SubjectOrder";
        logger.LogToFile("UpdateScheduleSubjects", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        logger.LogToFile("UpdateScheduleSubjects", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader.Close();
        logger.LogToFile("UpdateScheduleSubjects", " Exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    }

    public bool ExamSchduled(int _examid, int _batchid, string _classid,out int scheduledID)
    {
        scheduledID = 0;
        CLogging logger = CLogging.GetLogObject();
        bool _valide;
        logger.LogToFile("ExamSchduled", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "select tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId where tblclassexam.ExamId=" + _examid + " AND tblclassexam.ClassId=" + _classid + " AND tblexamschedule.BatchId=" + _batchid;
        logger.LogToFile("ExamSchduled", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            int.TryParse(m_MyReader["Id"].ToString(), out scheduledID);
            logger.LogToFile("ExamSchduled", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            _valide = true;
        }
        else
        {
            logger.LogToFile("ExamSchduled", " Reading Failure " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            _valide = false;
        }
        logger.LogToFile("ExamSchduled", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader.Close();
        logger.LogToFile("ExamSchduled", " exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _valide;
    }

    # region Combined Exams

    public OdbcDataReader getclass()
    {
        string sql = "select id, classname from tblclass order by tblclass.Standard,tblclass.ClassName";
        return m_MysqlDb.ExecuteQuery(sql);
    }

    public OdbcDataReader getMainExams(int _classId)
    {
        string sql = "select id, examname from tblexamcombmaster where ClassId = "+_classId+"";
        return m_MysqlDb.ExecuteQuery(sql);
    }

    public OdbcDataReader getExams()
    {
        string sql = "select tblexammaster.Id, tblexammaster.ExamName  from tblexammaster   where tblexammaster.Status = 1";
        return m_MysqlDb.ExecuteQuery(sql);
    }


    public OdbcDataReader GetExamsWithPeriod(int _ExmId)
    {
        //string sql = "select tblexamschedule.id,  tblperiod.Period from tblperiod inner join tblexamschedule on tblperiod.Id = tblexamschedule.PeriodId where tblexamschedule.ClassExamId  = " + _ExmId + "";
        string sql = "select tblperiod.Id,tblperiod.Period from tblperiod inner join tblexammaster on tblperiod.FrequencyId = tblexammaster.PeriodTypeId where tblexammaster.id = " + _ExmId + "";
        return m_MysqlDb.ExecuteQuery(sql);
    }

    public void AddNewCombination(int classId, string combination,string crUser, DateTime crDate, out string _msg)
    {
        string sql = "select id from tblexamcombmaster where ExamName='" + combination + "' and ClassId = " + classId + "";
        _msg = "";
        if (m_MysqlDb.ExecuteQuery(sql).HasRows)
        {
            _msg = "Already Exists..!";
        }
        else
        {
            sql = "insert into tblexamcombmaster (ExamName,ClassId,CreatedUser,CreatedDate) values ('" + combination + "'," + classId + ",'" + crUser + "','" + crDate.ToString("s") + "') ";
            m_MysqlDb.ExecuteQuery(sql);
            _msg = "Added Successfully";

        }
    }

    public void InsertCombinedExams(int _classId,int _combExmId,int _ExmId,DataSet _exmDataset)
    {
        string _sql = "", abbrev = "",ScheduleId="";
        int _PeriodId=0;
        foreach (DataRow dr in _exmDataset.Tables[0].Rows)
        {
            _PeriodId = int.Parse(dr[0].ToString());
            abbrev = dr[1].ToString();
            ScheduleId = dr[2].ToString();
            //_sql = "insert into tblexamcombmap (CombinedId,ClassId,SchExamId,Abbreviation) values (" + _CombExmId + "," + _classId + "," + _SchExmId + ",'" + abbrev + "')";
            _sql = "insert into tblexamcombmap (CombinedId,ClassId,ExamId,PeriodId,Abbreviation,SchExamId) values (" + _combExmId + "," + _classId + "," + _ExmId + "," + _PeriodId + ",'" + abbrev + "','" + ScheduleId + "')";
            m_MysqlDb.ExecuteQuery(_sql);

        }
    }

    public OdbcDataReader CombinedExamDtls(int classId, int cmbExmID)
    {
        // string sql = "select tblexamschedule.Id, tblexammaster.ExamName, tblperiod.Period,tblexamcombmap.Abbreviation  from tblexamcombmap inner join tblexamschedule on tblexamcombmap.SchExamId = tblexamschedule.Id  inner join tblclassexam on tblclassexam.id = tblexamschedule.ClassExamId  inner join tblclass on tblclassexam.ClassId = tblclass.Id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id inner join tblexammaster on tblexammaster.Id = tblclassexam.ExamId   where( tblclass.id = " + classId + " ) and (tblexamcombmap.CombinedId = " + cmbExmID + ") ";
        string sql="select tblexamcombmap.Id, tblexammaster.ExamName, tblperiod.Period , tblexamcombmap.Abbreviation from tblexamcombmap inner join tblexammaster on tblexammaster.Id = tblexamcombmap.ExamId inner join tblperiod on tblperiod.Id = tblexamcombmap.PeriodId where tblexamcombmap.CombinedId = "+cmbExmID+"";
        return m_MysqlDb.ExecuteQuery(sql);
    }

    public void DeleteCombinedResult(int id)
    {
        string sql = "Delete from tblexamcombmap where id=" + id + " ";
        m_MysqlDb.ExecuteQuery(sql);
    }

    public void DeleteCombinedExam(int CombExmId)
    {
        string sql = "Delete from tblexamcombmap where CombinedId=" + CombExmId + " ";
        m_MysqlDb.ExecuteQuery(sql);
        
        sql = "Delete from tblexamcombmaster where Id=" + CombExmId + " ";
        m_MysqlDb.ExecuteQuery(sql);
    }

    # endregion

    # region Classteacherreport

    public string GetClassTeacherReport(int _ClassId, int _ExamId)
    {
        StringBuilder CTR = new StringBuilder();
        DataSet Subjects = GetSubjects(_ClassId, _ExamId);
        DataSet Students = GetStudents(_ClassId);
        DataSet CombineExams = null;
        double Total = 0;
        double Maxtotal = 0;
        string Mark = "0";
        int _IndExamId = 0;
       
        if (Subjects != null && Subjects.Tables != null && (Subjects.Tables[0].Rows.Count > 0) && Students != null && Students.Tables != null && (Students.Tables[0].Rows.Count > 0))
        {

  
            CTR.Append("<table id=\"MyReport\" runat=\"server\" width=\"100%\" style=\"border: thin solid #000000\">");
            CTR.Append("<tr>");
            CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">NAME</td>");
            foreach (DataRow Dr_Subject in Subjects.Tables[0].Rows)
            {
                CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Dr_Subject[1].ToString() + "");
                CTR.Append("</td>");
            }
            CTR.Append("</tr>");

            CTR.Append("<tr>");
            // Name Td
            CTR.Append("<td>");
            CTR.Append("</td>");
            //Foreach Subject Making Table

            foreach (DataRow Dr_Subject in Subjects.Tables[0].Rows)
            {
                CTR.Append("<td>");
                    CTR.Append("<table width=\"100%\">");
                     CTR.Append("<tr>");
                     CombineExams = GetCombinedExams(_ClassId, _ExamId);
                         if (CombineExams != null && CombineExams.Tables != null && CombineExams.Tables[0].Rows.Count > 0)
                         {
                             foreach (DataRow Dr_Exams in CombineExams.Tables[0].Rows)
                             {
                                 CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">" + Dr_Exams[0].ToString() + "");
                                 CTR.Append("</td>");
                             }
                         }
                         CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">TOTAL");
                         CTR.Append("</td>");
                         CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">GRADE");
                         CTR.Append("</td>");
                    CTR.Append("</tr>");
                  CTR.Append("</table>");
                CTR.Append("</td>");
            }


            CTR.Append("</tr>");
            // StudentName Generation
           
            foreach (DataRow Dr_Student in Students.Tables[0].Rows)
            {
                CTR.Append("<tr>");
                CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Dr_Student[1].ToString() + "");
                CTR.Append("</td>");
                 // Marks For Subject
                foreach (DataRow Dr_Subject in Subjects.Tables[0].Rows)
                {
                    Total = 0;
                    Maxtotal = 0;
                    CTR.Append("<td>");
                        CTR.Append("<table width=\"100%\">");
                            CTR.Append("<tr>");
                            // CombineExams = GetCombinedExams(_ClassId, _ExamId);
                            ArrayList ExamSchdID = new ArrayList();
                            CombineExams = GetCombinedExamSchedId(_ClassId, _ExamId, out ExamSchdID);
                             int[] DiscAmt = (int[])ExamSchdID.ToArray(typeof(int));

                             foreach (int Dr_Exams in DiscAmt)
                             {
                                 _IndExamId = GetExamId(Dr_Exams.ToString(), _ClassId);
                                 Mark = GetSubjectMarks(Dr_Exams.ToString(), Dr_Subject[0].ToString(), Dr_Student[0].ToString());
                                 CTR.Append("<td style=\"text-align:center; border: thin solid #000000\">" + Mark + "");
                                 CTR.Append("</td>");
                                 if (Mark != "a")
                                     Total = Total + double.Parse(Mark);

                                 Maxtotal = Maxtotal + GetMaxToTal(_ClassId, _IndExamId, Dr_Subject[0].ToString());
                             }
                                     CTR.Append("<td style=\"text-align:center; border: thin solid #000000\">" + Total.ToString() + "");
                                     CTR.Append("</td>");
                                     CTR.Append("<td style=\"text-align:center; border: thin solid #000000\">" + GetGrade(Total, Maxtotal,0) + "");
                                 CTR.Append("</td>");
                             CTR.Append("</tr>");
                     CTR.Append("</table>");
                    CTR.Append("</td>");
                }


                CTR.Append("</tr>");
            }
           

            CTR.Append("</table>");
        }
        return CTR.ToString();
    }

    private DataSet GetCombinedExamSchedId(int _ClassId, int _ExamId, out ArrayList ExamSchdID)
    {
        DataSet MyDataSet = null;
        OdbcDataReader MyExamReader = null;
        int SchId = 0;
        ExamSchdID = new ArrayList();
        string sql = "select tblexamcombmap.ExamId , tblexamcombmap.PeriodId from tblexamcombmap where tblexamcombmap.CombinedId=" + _ExamId + " and tblexamcombmap.ClassId=" + _ClassId;
        MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow Dr_Exam in MyDataSet.Tables[0].Rows)
            {
                sql = "select tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId where tblexamschedule.PeriodId=" + Dr_Exam[1].ToString() + " and tblclassexam.ExamId=" + Dr_Exam[0].ToString() + " and tblclassexam.ClassId=" + _ClassId;
                MyExamReader = m_MysqlDb.ExecuteQuery(sql);
                if (MyExamReader.HasRows)
                {
                    int.TryParse(MyExamReader.GetValue(0).ToString(), out SchId);
                    ExamSchdID.Add(SchId);
                }
            }
        }
       
        return MyDataSet;
    }

    private int GetExamId(string _ExmScheduleId, int _ClassId)
    {
        int GetExamId = 0;
        OdbcDataReader MyTempReader = null;
        string sql = "select distinct ExamId FROM tblclassexam where tblclassexam.Id = (select distinct tblexamschedule.ClassExamId from tblexamschedule where Id="+_ExmScheduleId+" ) and tblclassexam.ClassId =" + _ClassId;
        MyTempReader = m_MysqlDb.ExecuteQuery(sql);
        if (MyTempReader.HasRows)
        {
            int.TryParse(MyTempReader.GetValue(0).ToString(), out GetExamId);    
        }
        return GetExamId;            
    }

    public string GetGrade(double _Total, double _Maxtotal, int _GradeMasterId)
    {
        string _StudGrade="";
        double _Avg = 0;
        if (_Maxtotal > 0)
        {
            _Avg = (_Total * 100) / _Maxtotal;
        }

        string sqlstr = "SELECT Grade,LowerLimit,Result FROM tblgrade WHERE `Status`=1 AND GradeMasterId=" + _GradeMasterId + " ORDER BY LowerLimit DESC";
        OdbcDataReader m_MyReader1 = m_MysqlDb.ExecuteQuery(sqlstr);
        if (m_MyReader1.HasRows)
        {
            while (m_MyReader1.Read())
            {
                string _Grade = m_MyReader1.GetValue(0).ToString();
                double _Lowerlimit = 0;
                double.TryParse(m_MyReader1.GetValue(1).ToString(), out _Lowerlimit);
                string t_Result = m_MyReader1.GetValue(2).ToString();
                if (_Avg >= _Lowerlimit)
                {
                    _StudGrade = _Grade;
                    break;
                }
            }
        }
        return _StudGrade;
    }

    private double GetMaxToTal(int _ClassId, int _ExamId, string _SubjectId)
    {
        double _MaxToTal = 0;
        string sql = "select tblclassexamsubmap.MaxMark from tblclassexamsubmap inner join tblclassexam on tblclassexam.Id = tblclassexamsubmap.ClassExamId where tblclassexamsubmap.SubId=" + _SubjectId + " and tblclassexam.ClassId=" + _ClassId + " and tblclassexam.ExamId="+ _ExamId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if(m_MyReader.HasRows)
        {
            double.TryParse(m_MyReader.GetValue(0).ToString(),out _MaxToTal);
        }
        return _MaxToTal;
    }

    private string GetSubjectMarks(string ExmSchId, string _SubId, string _StudentId)
    {
        string _MarkColumn = GetMarkColumn(ExmSchId, _SubId);
        string _Mark = GetMark(_MarkColumn, ExmSchId, _StudentId);
        return _Mark;
    }

    private string GetMark(string _MarkColumn, string _ExmSchId, string _StudentId)
    {
        string _Mark = "0";
        string sql = "select " + _MarkColumn + "  from tblstudentmark where ExamSchId=" + _ExmSchId + " and StudId="+_StudentId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _Mark = m_MyReader.GetValue(0).ToString();
            if (_Mark == "-1")
                _Mark = "a";
            if (_Mark == "")
                _Mark = "0";
            
        }
        return _Mark;      
    }

    private string GetMarkColumn(string _ExmSchId, string _SubId)
    {
        string Column = "Mark1";
        string sql = "select tblexammark.MarkColumn from tblexammark where tblexammark.ExamSchId=" + _ExmSchId + " and tblexammark.SubjectId=" + _SubId + " order by tblexammark.SubjectOrder";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Column = m_MyReader.GetValue(0).ToString();
        }
        return Column;      
    }

  

    private DataSet GetCombinedExams(int _ClassId, int _ExamId)
    {
        DataSet MyDataSet = null;
        string sql = "select tblexamcombmap.Abbreviation,tblexamcombmap.SchExamId from tblexamcombmap where tblexamcombmap.CombinedId=" + _ExamId + " and tblexamcombmap.ClassId=" + _ClassId;
        MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return MyDataSet;
    }

    private DataSet GetStudents(int _ClassId)
    {
        DataSet MyDataSet = null;
        string sql = "select tblstudent.Id , tblstudent.StudentName from tblstudentclassmap inner join tblstudent on tblstudent.Id = tblstudentclassmap.StudentId where tblstudent.`Status`=1 and tblstudentclassmap.ClassId=" + _ClassId;
        MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return MyDataSet;
    }

    private DataSet GetSubjects(int _ClassId, int _ExamId)
    {
        DataSet MyDataSet= null;
       // string sql = "select tblsubjects.Id , tblsubjects.subject_name from tblclasssubmap inner join  tblsubjects on tblsubjects.Id = tblclasssubmap.SubjectId where tblclasssubmap.ClassId="+_ClassId;
        string sql = "select distinct tblsubjects.Id , tblsubjects.subject_name from tblclassexamsubmap INNER join   tblsubjects on tblsubjects.Id = tblclassexamsubmap.SubId inner join tblclassexam on tblclassexam.Id =  tblclassexamsubmap.ClassExamId where tblclassexam.ClassId=" + _ClassId + " and tblclassexam.ExamId in (select distinct ExamId from tblexamcombmap where CombinedId=" + _ExamId + ")";
        MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return MyDataSet;
    }

    # endregion

    public DateTime ExamScheduleDateFromScheduleId(string _ExamSchdId)
    {
        DateTime _ScheduleDate=new DateTime();
        string sql = "SELECT tblexamschedule.ScheduledDate from tblexamschedule where tblexamschedule.Id=" + _ExamSchdId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            DateTime.TryParse(m_MyReader.GetValue(0).ToString(), out _ScheduleDate);
        }
        return _ScheduleDate;    
    }

    public DateTime ExamScheduleDateFromScheduleIdForCombinedExam(string _ExamSchdId,int _ClassId)
    {
        DateTime _ScheduleDate = new DateTime();
        string sql = "SELECT MAX(tblexamschedule.ScheduledDate) from tblexamschedule where tblexamschedule.Id in(SELECT tblexamcombmap.SchExamId from tblexamcombmap where tblexamcombmap.CombinedId="+_ExamSchdId+" AND tblexamcombmap.ClassId="+_ClassId+")";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            DateTime.TryParse(m_MyReader.GetValue(0).ToString(), out _ScheduleDate);
        }
        return _ScheduleDate;    
    }

    public bool IsReadyForPublish(int _ExamSchId)
    {
        bool _Valid = false;
        string sql = "select Published from tblexamschedule where Id=" + _ExamSchId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            if (m_MyReader.GetValue(0).ToString() == "0")
                _Valid = true;
        }
        return _Valid;
    }

    public string getsubjectid(string SubjectName)
    {
        int SubjectId = 0;
        string sql = "select id from tblsubjects where subject_name LIKE '%" + SubjectName + "'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int.TryParse(m_MyReader.GetValue(0).ToString(), out SubjectId);
        }
        return SubjectId.ToString();
    }

    public int getstudentid(string StudentName,string RollNo,string ClassId,int BatchId)
    {
        int StudentId = 0;
        string sql = "select tblview_student.id from tblview_student inner join tblview_studentclassmap ON tblview_studentclassmap.StudentId=tblview_student.id where tblview_studentclassmap.BatchId=" + BatchId + " AND  tblview_studentclassmap.ClassId=" + ClassId + " AND tblview_studentclassmap.RollNo=" + RollNo + " AND tblview_student.StudentName LIKE '%" + StudentName + "%'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int.TryParse(m_MyReader.GetValue(0).ToString(), out StudentId);
        }
        return StudentId;
    }
  
    public int IfExamAssignedToAnyClass(int _ExamId)
    {
        int cnt = 0;
        string sql = "select count(id) from tblclassexam where examId = " + _ExamId + " and status = 1";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int.TryParse(m_MyReader.GetValue(0).ToString(), out cnt);
        }
        return cnt;
    }

    public bool WhetherMarksEntered(int _ExamId, int _BatchId, string _ClassId, string _PeriodId)
    {
        bool marks = true;
        string sql = "";
        string MarkCol="";
        int exmschId = 0;
        OdbcDataReader myreader = null;
        sql = "select tblexammark.MarkColumn, tblexammark.ExamSchId from tblexammark  where tblexammark.ExamSchId =( select tblexamschedule.id from tblexamschedule where tblexamschedule.ClassExamId = (SELECT tblclassexam.id from tblclassexam where tblclassexam.ClassId = "+_ClassId+" and tblclassexam.ExamId = "+_ExamId+" ) and tblexamschedule.BatchId = "+_BatchId+" and tblexamschedule.PeriodId = "+_PeriodId+" ) order by tblexammark.SubjectOrder";
        m_MyReader = null;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                MarkCol = m_MyReader.GetValue(0).ToString();
                int.TryParse(m_MyReader.GetValue(1).ToString(),out exmschId);
                sql = "select tblstudentmark." + MarkCol + " from tblstudentmark where tblstudentmark.StudId = (select max(tblstudentmark.StudId) from tblstudentmark where tblstudentmark.ExamSchId = " + exmschId + " ) and tblstudentmark.ExamSchId = " + exmschId + " ";
                myreader = null;
                myreader = m_MysqlDb.ExecuteQuery(sql);
                if (myreader.HasRows)
                {
                    while (myreader.Read())
                    {
                        if (myreader.GetValue(0).ToString() == "")
                        {
                        }
                    }
                }
                else
                {
                    marks = false;
                }
            }
        }
        m_MyReader.Close();
        myreader.Close();
        return marks;
    }

    public bool WhetherReportGenerated(int _ExamId, int _BatchId, string _ClassId, string _PeriodId)
    {
        bool report = true;
          OdbcDataReader myreader = null;

        string sql = "select tblexamschedule.id from tblexamschedule where tblexamschedule.ClassExamId = (SELECT tblclassexam.id from tblclassexam where tblclassexam.ClassId =  "+_ClassId+" and tblclassexam.ExamId = "+_ExamId+" ) and tblexamschedule.BatchId = "+_BatchId+" and tblexamschedule.PeriodId = "+_PeriodId+" ";
        int exmschId=0;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(),out exmschId);
                sql = " select tblstudentmark.TotalMark from tblstudentmark where tblstudentmark.StudId = (select max(tblstudentmark.StudId) from tblstudentmark where tblstudentmark.ExamSchId = " + exmschId + " ) and tblstudentmark.ExamSchId = " + exmschId + " ";
                myreader = null;
                myreader = m_MysqlDb.ExecuteQuery(sql);
                if (myreader.HasRows)
                    {
                        while (myreader.Read())
                        {
                            if (myreader.GetValue(0).ToString() == "")
                            {
                                report = false;
                            }
                        }
                    }

                else
                {
                    report = false;
                }
            }
        }
        
        return report;
    }

    public string GetsubjectColumn(int ExamId, int BatchId, string _subjectId, string ClassId)
    {

        string MarkColumn = "";
        string sql1 = "SELECT tblexammark.MarkColumn FROM tblexammark inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId WHERE tblclassexam.ExamId=" + ExamId + " AND tblexamschedule.BatchId=" + BatchId + " AND tblexammark.SubjectId=" + _subjectId + " AND tblclassexam.ClassId=" + ClassId + " order by tblexammark.SubjectOrder";
        if (m_TransationDb != null)
        {
            m_MyReader = m_TransationDb.ExecuteQuery(sql1);
        }
        else
        {
            m_MyReader = m_MysqlDb.ExecuteQuery(sql1);
        }
        if (m_MyReader.HasRows)
        {
            MarkColumn = m_MyReader.GetValue(0).ToString();

        }
        return MarkColumn;
    }

    public int GetExamIdFromSubType(string _SubjectType, int _ClassId)
    {
        // this fuction returns exam id by using Subject type and ClassId.
        int _ExamId = 0;
        string sql = "select tblclassexam.ExamId from tblclassexam  inner join tblexammaster on tblexammaster.Id = tblclassexam.ExamId inner join tblsubject_type on tblsubject_type.Id= tblexammaster.ExamTypeId  where tblsubject_type.sbject_type='" + _SubjectType + "' and tblclassexam.ClassId=" + _ClassId;
        OdbcDataReader ExamReader = m_MysqlDb.ExecuteQuery(sql);

        if (ExamReader.HasRows)
            _ExamId = int.Parse(ExamReader.GetValue(0).ToString());

        return _ExamId;

    }

    public string GetFont()
    {
        return "Times New Roman";
    }

    public DataSet GetGradeDataSet(int _GradeMasterId)
    {
        string _sql = "select tblgrade.Grade, tblgrade.LowerLimit,NumericalGrade,Result,Award from tblgrade where tblgrade.GradeMasterId=" + _GradeMasterId + " and   tblgrade.`Status`=1   order by tblgrade.id asc";
        DataSet Dt = m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
        return Dt;
    }

    public string[] GetSubectGroupd()
    {
        string[] _SubGrp;
        string sql = "select tbltime_subgroup.Id, tbltime_subgroup.Name from tbltime_subgroup";
        OdbcDataReader SubGrpReader = m_MysqlDb.ExecuteQuery(sql);

        if (SubGrpReader.HasRows)
        {
            int Count = SubGrpReader.RecordsAffected;
            _SubGrp = new string[Count];
            int i = 0;
            while (SubGrpReader.Read())
            {
                _SubGrp[i] = SubGrpReader.GetValue(0).ToString();
                i++;
            }
        }
        else
        {
            _SubGrp = null;
        }
        return _SubGrp;

    }

    public double GetMaxMarkFromExamScheduleIdandSubjectId(int ExamSchduleId, int SubjectId)
    {
        double MaxMArk = 0;
        string sql = "select tblclassexamsubmap.MaxMark from tblclassexamsubmap inner join tblexamschedule on  tblexamschedule.ClassExamId= tblclassexamsubmap.ClassExamId where tblclassexamsubmap.SubId=" + SubjectId + " and tblexamschedule.Id=" + ExamSchduleId + "";
        DataSet _Dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (_Dt != null && _Dt.Tables[0].Rows.Count > 0)
        {
            double.TryParse(_Dt.Tables[0].Rows[0]["MaxMark"].ToString(), out  MaxMArk);
        }
        return MaxMArk;
    }

    public double GetMaxMarkFromExamScheduleIdandSubjectName(int ExamSchduleId, string SubjectName)
    {
        double MaxMArk = 0;
        string sql = "select tblclassexamsubmap.MaxMark from tblclassexamsubmap inner join tblexamschedule on  tblexamschedule.ClassExamId= tblclassexamsubmap.ClassExamId inner join tblsubjects on tblsubjects.Id = tblclassexamsubmap.SubId where  tblsubjects.subject_name='" + SubjectName + "' and tblexamschedule.Id=" + ExamSchduleId + "";
        DataSet _Dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (_Dt != null && _Dt.Tables[0].Rows.Count > 0)
        {
            double.TryParse(_Dt.Tables[0].Rows[0]["MaxMark"].ToString(), out  MaxMArk);
        }
        return MaxMArk;
    }




    public DataSet GetExamResultRemarks()
    {

        string sql = "SELECT ExamCount,Remarks FROM tblexamremark WHERE Active=1 ORDER BY ExamCount";
        DataSet _Dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return _Dt;
    }

    public DataSet GetFeeDueStudent(string ClassId)
    {
        string sql = "select DISTINCT tblfeestudent.StudId from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tblfeestudent.StudId where tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion' and tblfeeschedule.DueDate <= CURRENT_DATE() AND tblstudentclassmap.ClassId=" + ClassId;
        DataSet _Dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return _Dt;
    }

    public DataSet GetExamPersentageRatio(string ClassId)
    {
        string sql = "select  tblcbseexamratiomap.RatioColumName,tblcbseexamratiomap.ExamName  from tblcbseexamratiomap where tblcbseexamratiomap.ExamName in ('UT','Half','Annual')";
        DataSet Dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        
        string _colName = "";

        if (Dt.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in Dt.Tables[0].Rows)
            {
                if (_colName != "")
                    _colName = _colName + " , ";
                _colName = _colName + dr["RatioColumName"] + "  as " + dr["ExamName"];
            }

            sql = "select  Distinct " + _colName + " from tblcbsegraderatio inner join tblstandard on tblstandard.Id= tblcbsegraderatio.StandardId inner join tblstudentclassmap on tblstudentclassmap.Standard=tblstandard.Id where tblstudentclassmap.ClassId=" + ClassId;
          
            Dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            return Dt;
        }


        return null;
    }

    public void DeleteSubjectsFromscheduledExam(string subjectId, int scheduledId,int CurrentBatchId)
    {
        // delete exam mark details from tblexammark
     
        string sql = "select tblexammark.MarkColumn as  MarkColumn from tblexammark where tblexammark.ExamSchId=" + scheduledId + " and tblexammark.SubjectId=" + subjectId;
        DataSet Dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        if (Dt != null && Dt.Tables != null && Dt.Tables[0].Rows.Count > 0)
        {
            sql = "update tblstudentmark set " + Dt.Tables[0].Rows[0]["MarkColumn"].ToString() + " =null  where tblstudentmark.ExamSchId= " + scheduledId;
            m_MysqlDb.ExecuteQuery(sql);
        }

        sql = "delete from tblexammark where tblexammark.ExamSchId=" + scheduledId + " and tblexammark.SubjectId=" + subjectId + "";
        m_MysqlDb.ExecuteQuery(sql);
        
        // update status of examschedule  if status is 'Completed'
        /* this qry is not working
         * update tblexamschedule a set a.`Status` = (select if( b.`Status`<>'Scheduled','Updated','Scheduled') as Updated from tblexamschedule  b where b.id=266)  where a.id=266
         */


        sql = " select tblexamschedule.Status from tblexamschedule where tblexamschedule.Id=" + scheduledId+" and BatchId="+CurrentBatchId;
         Dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        if (Dt != null && Dt.Tables != null && Dt.Tables[0].Rows.Count > 0)
        {
            if (Dt.Tables[0].Rows[0]["Status"].ToString() != "Scheduled")
            {

                sql = "update tblexamschedule set `Status` = 'Updated' where   tblexamschedule.id=" + scheduledId + " and BatchId=" + CurrentBatchId; 
                m_MysqlDb.ExecuteQuery(sql);
            }

        }

        

    }

    public void DeleteScheduledExam(string i_SelectedSubId)
    {
        string sql = "Delete from tblexamschedule where   tblexamschedule.id=" + i_SelectedSubId;
        m_MysqlDb.ExecuteQuery(sql);
    }

    public void UpdateExamScheduleStatusfromClassExamId(int classexamId, string Status, int BatchId)
    {
        string sql = "update tblexamschedule set tblexamschedule.Status='" + Status + "' where tblexamschedule.BatchId=" + BatchId + " and tblexamschedule.ClassExamId=" + classexamId + " and tblexamschedule.Status<>'Scheduled'";
        m_MysqlDb.ExecuteQuery(sql);
    }

    public void UpdatedEnterdMark(int classexamId, int _SubjectId, int BatchId)
    {
        string sql = "select  tblexamschedule.Id,tblexamschedule.Status from tblexamschedule where  tblexamschedule.BatchId=" + BatchId + " and tblexamschedule.ClassExamId=" + classexamId;
        DataSet Dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

         if (Dt != null && Dt.Tables != null && Dt.Tables[0].Rows.Count > 0)
         {
             if (Dt.Tables[0].Rows[0]["Status"].ToString() != "Scheduled")
             {
                 sql = "select tblexammark.ExamSchId as Id, tblexammark.MarkColumn as  MarkColumn from tblexammark where tblexammark.ExamSchId=" + Dt.Tables[0].Rows[0]["Id"].ToString() + " and tblexammark.SubjectId=" + _SubjectId;
                 
                 Dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
               
                 if (Dt != null && Dt.Tables != null && Dt.Tables[0].Rows.Count > 0)
                 {
                     sql = "update tblstudentmark set " + Dt.Tables[0].Rows[0]["MarkColumn"].ToString() + " =null  where tblstudentmark.ExamSchId= " + Dt.Tables[0].Rows[0]["Id"].ToString();
                     m_MysqlDb.ExecuteQuery(sql);
                 }


             }
         }
    }

    public void UpdateInexammarkcolumn(int classexamId, int _SubjectId, int BatchId)
    {
         string sql = "select  tblexamschedule.Id,tblexamschedule.Status from tblexamschedule where  tblexamschedule.BatchId=" + BatchId + " and tblexamschedule.ClassExamId=" + classexamId;
        DataSet Dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        int _ExSchId = 0;
        string _columnname = "Mark"; int _timeid = 1; int SubOrder = 0;
        if (Dt != null && Dt.Tables != null && Dt.Tables[0].Rows.Count > 0)
        {

            int.TryParse(Dt.Tables[0].Rows[0]["Id"].ToString(), out  _ExSchId);

            sql = "select tblexammark.MarkColumn as  MarkColumn, Date_Format(ExamDate,'%d/%m/%Y') as ExamDate ,SubjectOrder from tblexammark where tblexammark.ExamSchId=" + _ExSchId + "  order by SubjectOrder Desc";
            Dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            int count =Dt.Tables[0].Rows.Count + 1;
            _columnname = _columnname + count;

            DateTime _examdate = General.GetDateTimeFromText(Dt.Tables[0].Rows[0]["ExamDate"].ToString());
            int.TryParse(Dt.Tables[0].Rows[0]["SubjectOrder"].ToString(), out SubOrder);
            SubOrder = SubOrder + 1;
            ScheduleSubjects(_ExSchId, _columnname, _SubjectId, _timeid, _examdate, SubOrder);

        }

        
    }

    public double GetAggregatemark(int _examid, int _BatchId, string _classid, string _periodid)
    {
        CLogging logger = CLogging.GetLogObject();
        double Aggregatemark;
        logger.LogToFile("GetAggregatemark", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "select  tblexamschedule.Id,tblexamschedule.AggregateMark from tblexamschedule inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId where tblclassexam.ExamId=" + _examid + " AND tblclassexam.ClassId=" + _classid + " AND tblexamschedule.BatchId=" + _BatchId + " AND tblexamschedule.PeriodId=" + _periodid;
        logger.LogToFile("GetAggregatemark", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            logger.LogToFile("GetAggregatemark", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Read();
            //Aggregatemark = int.Parse(m_MyReader.GetValue(0).ToString());
            Aggregatemark = Math.Round(double.Parse(m_MyReader.GetValue(1).ToString()), 2);
        }
        else
        {
            logger.LogToFile("GetAggregatemark", " Reading Failure " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            Aggregatemark = 0;
        }
        logger.LogToFile("GetAggregatemark", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader.Close();
        logger.LogToFile("GetAggregatemark", " Exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return Aggregatemark;
    }
}

class AggregateConfig
{
    public int SubId;
    public double MinPassMArk;
}
class StudentAggregateConfig
{
    public int SubId;
    public string MarkCloumn;
}

