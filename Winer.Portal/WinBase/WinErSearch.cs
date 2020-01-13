using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
namespace WinBase
{
    public class WinErSearch : KnowinGen
    {  
    public MysqlClass m_MysqlDb;
    private OdbcDataReader m_MyReader = null;
    private string m_SearchMenuStr;
    public WinErSearch(KnowinGen _Prntobj)
    {
        m_Parent = _Prntobj;
        m_MyODBCConn = m_Parent.ODBCconnection;
        m_UserName = m_Parent.LoginUserName;
        m_MysqlDb = new MysqlClass(this);
        m_SearchMenuStr = "";
    }

    ~WinErSearch()
    {
        if (m_MysqlDb != null)
        {
            m_MysqlDb = null;

        } if (m_MyReader != null)
        {
            m_MyReader = null;

        }
       
    }

    public string GetSearchMangMenuString(int _roleid)
    {
        CLogging logger = CLogging.GetLogObject();
        string _MenuStr;
        if (m_SearchMenuStr == "")
        {


            _MenuStr = "<ul><li><a href=\"StudentSearch.aspx\">Search Student</a></li>";
            logger.LogToFile("GetSearchMangMenuString", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            string sql = "SELECT DISTINCT tblaction.MenuName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND tblroleactionmap.ModuleId=4 AND tblaction.ActionType='Link'";
            logger.LogToFile("GetSearchMangMenuString", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                logger.LogToFile("GetSearchMangMenuString", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                while (m_MyReader.Read())
                {
                    	

                    _MenuStr = _MenuStr + "<li><a href=\""+m_MyReader.GetValue(1).ToString() +"\">"+m_MyReader.GetValue(0).ToString()+"</a></li>";
                }

            }
            _MenuStr = _MenuStr + "</ul>";
            logger.LogToFile("GetSearchMangMenuString", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Close();
            m_SearchMenuStr = _MenuStr;
            
         
        }
        else
        {
            _MenuStr = m_SearchMenuStr;
        }
        logger.LogToFile("GetExamMangMenuString", " exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _MenuStr;
 
    }



    public string GetStudStatus(int _StudId)
    {
        string status="";
        int ActualStatus;
        string sql = "SELECT tblstudent.Status FROM tblstudent WHERE tblstudent.Id=" + _StudId;
         m_MyReader = m_MysqlDb.ExecuteQuery(sql);
         if (m_MyReader.HasRows)
         {
             ActualStatus = int.Parse(m_MyReader.GetValue(0).ToString());
             if (ActualStatus == 1)
             {
                 status = "Studying";
             }
             else
             {
                 status = "Passed out";
             }
         }
         return status;
    }

    public string GetLastBatch(int _studentId)
    {
        string Lastbatch = "";
        string sql = "";
        if (Passedout(_studentId))
        {
            sql = "SELECT DISTINCT tblbatch.BatchName FROM tblstudentclassmap INNER JOIN tblbatch ON tblbatch.Id = tblstudentclassmap.BatchId INNER JOIN tbltc ON tblstudentclassmap.BatchId = tbltc.LastBatchId WHERE tbltc.StudentId=" + _studentId;
        }
        else
        {
         // sql = "SELECT DISTINCT tblbatch.BatchName FROM tblstudentclassmap INNER JOIN tblbatch ON (select max(tblstudentclassmap.BatchId) from tblstudentclassmap)= tblbatch.Id WHERE tblstudentclassmap.StudentId="+ _studentId;
            sql = "SELECT tblbatch.BatchName FROM tblstudentclassmap INNER JOIN tblbatch on tblstudentclassmap.BatchId = tblbatch.Id AND tblbatch.Status=1 WHERE tblstudentclassmap.StudentId=" + _studentId;
        }
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Lastbatch = m_MyReader.GetValue(0).ToString();
        }
        return Lastbatch;
    }

    private bool Passedout(int _studentId)
    {

        bool status = true;
        int ActualStatus;
        string sql = "SELECT tblstudent.Status FROM tblstudent WHERE tblstudent.Id=" + _studentId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            ActualStatus = int.Parse(m_MyReader.GetValue(0).ToString());
            if (ActualStatus == 1)
            {
                status =false;
            }
           
        }
        return status;
    }

    public string GetLastClass(int _studentId)
    {
        string Lastclass = "";
        string sql = "";
        if (Passedout(_studentId))
        {
            sql = "SELECT DISTINCT tblclass.ClassName  FROM tblstudentclassmap INNER JOIN tblclass ON tblstudentclassmap.ClassId = tblclass.Id INNER JOIN tbltc on tbltc.LastBatchId = tblstudentclassmap.BatchId WHERE tblstudentclassmap.StudentId=" + _studentId;
        }
        else
        {
            sql = "SELECT DISTINCT tblclass.ClassName FROM tblstudentclassmap INNER JOIN tblclass ON tblstudentclassmap.ClassId = tblclass.Id INNER JOIN tblbatch ON tblstudentclassmap.BatchId = tblbatch.Id WHERE tblbatch.Status=1 AND tblstudentclassmap.StudentId=" + _studentId;
        }
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Lastclass = m_MyReader.GetValue(0).ToString();
        }
        return Lastclass;
    }



    public string GetDesignation(int _staffId)
    {
        string designation = ""; ;
        string sql = " SELECT Designation FROM tblstaffdetails WHERE UserId=" + _staffId;
         m_MyReader = m_MysqlDb.ExecuteQuery(sql);
         if (m_MyReader.HasRows)
         {
             designation = m_MyReader.GetValue(0).ToString();
         }
        return designation;
    }

    public string GetStaffStatus(int _staffId)
    {
        string status = "";
        int ActualStatus;
        string sql = "SELECT tbluser.Status FROM tbluser WHERE tbluser.Id=" + _staffId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            ActualStatus = int.Parse(m_MyReader.GetValue(0).ToString());
            if (ActualStatus == 1)
            {
                status = "Existing";
            }
            else
            {
                status = "Resigned";
            }
        }
        return status;
    }

    public string GetExamType(int _ExamID)
    {
        string ExamType = "",sql;
        sql = "SELECT tblsubject_type.sbject_type FROM tblexam INNER JOIN tblsubject_type ON tblexam.TypeId = tblsubject_type.Id AND tblexam.Id=" + _ExamID;
         m_MyReader = m_MysqlDb.ExecuteQuery(sql);
         if (m_MyReader.HasRows)
         {
             ExamType = m_MyReader.GetValue(0).ToString();
         }
        return ExamType;
    }



    public int GetBatchID(string BatchName)
    {
        int BatchId=-1;
        string sql = "SELECT tblbatch.Id FROM tblbatch WHERE tblbatch.BatchName='" + BatchName + "'";
         m_MyReader = m_MysqlDb.ExecuteQuery(sql);
         if (m_MyReader.HasRows)
         {
             BatchId = int.Parse(m_MyReader.GetValue(0).ToString());
         }
         return BatchId;
    }

    public bool IsStaffSubject(int _StaffId, int _subId)
    {
        bool _valide;
        string sql = "SELECT * FROM tblstaffsubjectmap where StaffId=" + _StaffId + " And SubjectId=" + _subId;
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


    public bool ExamSchduled(int _examid, int _batchId)
    {
        CLogging logger = CLogging.GetLogObject();
        bool _valide;
        logger.LogToFile("ExamSchduled", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "SELECT * FROM tblexamschedule where BatchId=" + _batchId + " AND ExamId=" + _examid;
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

    }
}
