using System;
using System.Web;
//using System.Web.Services;
//using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Data.Odbc;
using System.Data;
public class ClassOrganiser:KnowinGen
{
    public MysqlClass m_MysqlDb;
    public MysqlClass m_TransationDb = null;
    private OdbcDataReader m_MyReader = null;

    private string m_ClassMenuStr;
    private string m_ClassSubMenuStr;
    public ClassOrganiser(MysqlClass _Msqlobj)
    {
        m_MysqlDb = _Msqlobj;
    }
    public ClassOrganiser(KnowinGen _Prntobj)
    {
        m_Parent = _Prntobj;
        m_MyODBCConn = m_Parent.ODBCconnection;
        m_MysqlDb = new MysqlClass(this);
        m_ClassMenuStr = "";
        m_ClassSubMenuStr = "";
    }

    ~ClassOrganiser()
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


    public string GetClassMangMenuString(int _roleid)
    {
        string _MenuStr;
        if (m_ClassMenuStr == "")
        {


            _MenuStr = "<ul><li><a href=\"LoadClassDetails.aspx\">View Class</a></li>";
            string sql = "SELECT DISTINCT tblaction.MenuName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND tblroleactionmap.ModuleId=10 AND tblaction.ActionType='Link'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {


                    _MenuStr = _MenuStr + "<li><a href=\"" + m_MyReader.GetValue(1).ToString() + "\">" + m_MyReader.GetValue(0).ToString() + "</a></li>";
                }

            }
            _MenuStr = _MenuStr + "</ul>";
            m_MyReader.Close();
            m_ClassMenuStr = _MenuStr;

        }
        else
        {
            _MenuStr = m_ClassMenuStr;
        }
        return _MenuStr;

    }

    public int CreateClass(string _Name, int _Standrd, int _parentgroup,int TotalSeats)
    {
        int _classid;
        string sql = "INSERT INTO tblclass(ClassName,Standard,ParentGroupID,TotalSeats) VALUES ('" + _Name + "','" + _Standrd + "'," + _parentgroup + ","+TotalSeats+")";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        sql = "SELECT Id FROM tblclass where ClassName='" + _Name + "'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            m_MyReader.Read();

            _classid = int.Parse(m_MyReader.GetValue(0).ToString());

        }
        else
        {
            _classid = -1;
        }
        m_MyReader.Close();
        return _classid;
    }

    public void UpdateClass(int _ClassId,  string _name,string _Standrd, int _parentgroup, int _TotalSeats ,int _ClassTeacher)
    {
        string sql = "UPDATE tblclass SET ClassName='" + _name + "', Standard = '" + _Standrd + "', ParentGroupID= " + _parentgroup + ",TotalSeats=" + _TotalSeats + " ,ClassTeacher=" + _ClassTeacher + "  WHERE Status=1 AND Id =" + _ClassId + "";
        m_MysqlDb.ExecuteQuery(sql);
        //DeletClassSubject(_ClassId);
    }

    public bool AvailableClassName(string _name)
    {
        bool _valide;
        string sql = "SELECT ClassName FROM tblclass where Status=1 AND ClassName='" + _name + "'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            _valide = false;
        }
        else
        {
            _valide = true;
        }
        m_MyReader.Close();
        return _valide;
    }

    public bool IsClassSubject(int _ClassId, int _subId)
    {
        bool _valide;
        string sql = "SELECT * FROM tblclasssubmap where ClassId=" + _ClassId + " And SubjectId=" + _subId;
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

    public void AddSujectToClass(int _SubjectId, int _classid)
    {
        string sql = "INSERT INTO tblclasssubmap(ClassId,SubjectId) VALUES (" + _classid + ", " + _SubjectId + ")";
        m_MysqlDb.ExecuteQuery(sql);
    }

    private void DeletClassSubject(int _classid)
    {
        string sql = "DELETE FROM tblclasssubmap WHERE ClassId=" + _classid;
        m_MysqlDb.ExecuteQuery(sql);

    }

    public void DeletClass(int _classid,int _mode)

    {
        if (_mode == 2)
        {
            DeletClassSubject(_classid);
            string sql = "DELETE FROM tblclass WHERE Id=" + _classid;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            m_MyReader.Close();
        }
        else
        {
            string sql = "UPDATE tblclass SET  Status=0  WHERE Id =" + _classid;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            m_MyReader.Close();
        }
        
    }

    public bool CanDelete(int _ClassId, int _batchId, out int _mode, out bool IsRegular)
    {
        bool _valide;
        IsRegular = false;
        if (!ClassHasStudent(_ClassId, _batchId, out IsRegular))
        {
            if (ClassHasExam(_ClassId) || ClassHasFeeScheduled(_ClassId) || ClassHasSchduled(_ClassId))
            {
                _mode = 1;
                _valide = true;
            }
            else
            {
                _mode = 2;
                _valide = true;
            }

        }
        else
        {
            _valide = false;
            _mode = 0;
        }
        return _valide;
    }

    private bool ClassHasFeeScheduled(int _ClassId)
    {
        bool _valide;
        string sql = "SELECT * FROM tblfeeschedule where ClassId=" + _ClassId;
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

    private bool ClassHasSchduled(int _ClassId)
    {
        bool _valide;
        string sql = "SELECT * FROM tblclassschedule where ClassId=" + _ClassId ;
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

 

    private bool ClassHasExam(int _ClassId)
    {

        bool _valide;
        string sql = "SELECT * FROM tblclassexam where ClassId=" + _ClassId;
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

  
    private bool ClassHasStudent(int _ClassId, int _batchId,out bool IsRegular)
    {
        IsRegular = false;
        bool _valide=false;
        string sql = "SELECT * FROM tblstudentclassmap where BatchId=" + _batchId + " AND ClassId=" + _ClassId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            _valide = true;
            IsRegular = true;
        }

        if (!_valide)
        {
            sql = "SELECT COUNT(tbltempstdent.Id) FROM tbltempstdent WHERE tbltempstdent.Status=1 AND tbltempstdent.Class="+_ClassId+" AND tbltempstdent.JoiningBatch=" + _batchId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int _count = 0;
                int.TryParse(m_MyReader.GetValue(0).ToString(), out _count);
                if (_count > 0)
                {
                    _valide = true;
                    IsRegular = false;
                }
            }
            else
            {
                _valide = false;
            }
        }

        m_MyReader.Close();
        return _valide;
    }

    public string GetClassMangSubMenuString(int _roleid,int _ModuleType)
    {
        string _MenuStr;
        //if (m_ClassSubMenuStr == "")
        //{

        _MenuStr = "<ul class=\"block\"><li><a href=\"ClassDetails.aspx\">Class Details</a></li>";
        string sql = "SELECT DISTINCT tblaction.MenuName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId inner join tblmodule  on tblmodule.Id= tblroleactionmap.ModuleId  WHERE  tblroleactionmap.RoleId=" + _roleid + " AND ((tblroleactionmap.ModuleId=10 AND tblaction.ActionType='SubLink') OR (tblaction.ActionType='ClassSubLink')) AND tblmodule.ModuleType IN (3," + _ModuleType + ")";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {


                _MenuStr = _MenuStr + "<li><a href=\"" + m_MyReader.GetValue(1).ToString() + "\">" + m_MyReader.GetValue(0).ToString() + "</a></li>";
            }

        }
        _MenuStr = _MenuStr + "</ul>";
        m_MyReader.Close();
        //m_ClassSubMenuStr = _MenuStr;

           
        //}
        //else
        //{
        //    _MenuStr = m_ClassSubMenuStr;
        //}
        return _MenuStr;
    }

    //public bool IsSchiduled(int _classId,int _batchId, out int _SchId)
    //{
    //    bool _valide;
    //    string sql = "SELECT Id FROM tblclassschedule where ClassId=" + _classId + " And BatchId=" + _batchId;
    //    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    //    if (m_MyReader.HasRows)
    //    {

    //        _valide = true;
    //        _SchId = int.Parse(m_MyReader.GetValue(0).ToString());

    //    }
    //    else
    //    {
    //        _SchId = -1;
    //        _valide = false;
    //    }
    //    m_MyReader.Close();
    //    return _valide;
    //}

    public void SchduleClass(int _classid, int _batchid, int _classteacherid, string _room)
    {
        string sql = "INSERT INTO tblclassschedule (ClassId,BatchId,ClassTeacherId,ClassRoom) VALUES (" + _classid + ", " + _batchid + ", " + _classteacherid + ", '" + _room + "')";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        m_MyReader.Close();
        
    }

    public void UpdateSchdule(int _classid, int _batchid, int _classteacherid, string _room)
    {
        string sql = "UPDATE tblclassschedule SET  ClassTeacherId = " + _classteacherid + ", ClassRoom= '" + _room + "'  WHERE ClassId=" + _classid + " AND BatchId=" + _batchid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        m_MyReader.Close();
        
    }

   

    public string GetClassname(int _classid)
    {

        string _classname = "";
        string sql = "SELECT ClassName FROM tblclass where Id=" + _classid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {


            _classname = m_MyReader.GetValue(0).ToString();

        }
       
        m_MyReader.Close();
        return _classname;

    }

    public void UpdateRollNo(int _ClassId, int _BatchId, int _RollNo,int _studId)
    {
        string sql = "UPDATE tblstudentclassmap SET  RollNo = " + _RollNo + "  WHERE ClassId=" + _ClassId + " AND BatchId =" + _BatchId + " AND StudentId =" + _studId;
        if (m_TransationDb != null)
        {
            m_TransationDb.ExecuteQuery(sql);
        }
        else
        {
            m_MysqlDb.ExecuteQuery(sql);
        }
        sql = "update tblstudent set RollNo=" + _RollNo + " where Id=" + _studId;
        if (m_TransationDb != null)
        {
            m_TransationDb.ExecuteQuery(sql);
        }
        else
        {
            m_MysqlDb.ExecuteQuery(sql);
        }
        
    }

    public string GetRollnumber(int _StudId, int _ClassId, int _BatchId)
    {
        string _rollNo="";
        string sql = "SELECT tblstudentclassmap.RollNo from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + _BatchId + " AND tblstudentclassmap.ClassId=" + _ClassId + "  AND tblstudentclassmap.StudentId=" + _StudId + " Order by tblstudentclassmap.RollNo ASC";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _rollNo = m_MyReader.GetValue(0).ToString();
            if (_rollNo == "-1")
            {
                _rollNo = "No Roll No.";
            }
        }
        return _rollNo;
    }


    # region Exam Report

    public DataSet GetStudentlist_AdmissionNo(int _ClassID, int _BatchId)
    {
        DataSet Students = null;
        string sql = "select tblstudent.AdmitionNo , tblstudent.StudentName from tblstudentclassmap inner join tblstudent on tblstudent.Id = tblstudentclassmap.StudentId where tblstudentclassmap.ClassId = " + _ClassID + " and tblstudentclassmap.BatchId = " + _BatchId + " and tblstudent.`Status` = 1 ORDER BY tblstudentclassmap.RollNo";
        Students = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return Students;
    }
    public DataSet GetStudentlist(int _ClassID, int _BatchId)
    {
        DataSet Students = null;
        string sql = "select tblstudentclassmap.RollNo , tblstudent.StudentName from tblstudentclassmap inner join tblstudent on tblstudent.Id = tblstudentclassmap.StudentId where tblstudentclassmap.ClassId = " + _ClassID + " and tblstudentclassmap.BatchId = " + _BatchId + " and tblstudent.`Status` = 1 ORDER BY tblstudentclassmap.RollNo";
        Students = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return Students;
    }
    public DataSet GetStudentlistWithStudentIdAndName(int _ClassID, int _BatchId)
    {
        DataSet Students = null;
        string sql = "select tblstudent.Id , tblstudent.StudentName from tblstudentclassmap inner join tblstudent on tblstudent.Id = tblstudentclassmap.StudentId where tblstudentclassmap.ClassId = " + _ClassID + " and tblstudentclassmap.BatchId = " + _BatchId + " and tblstudent.`Status` = 1 ORDER BY tblstudentclassmap.RollNo";
        Students = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return Students;
    }

    public DataSet GetSubjectList(int _ClassID, int i_ExamId, int _BatchId)
    {
        DataSet Subjects = null;
        string sql = " SELECT distinct tblsubjects.Id,tblsubjects.subject_name from tblsubjects inner JOIN tblexammark on  tblexammark.SubjectId=tblsubjects.Id  inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on  tblclassexam.Id = tblexamschedule.ClassExamId        where tblexamschedule.BatchId="+_BatchId+"    and   tblclassexam.ClassId="+_ClassID+" and tblexamschedule.Status='Completed'  and tblclassexam.ExamId ="+i_ExamId+"  order by tblexammark.SubjectOrder ";
       // string sql = "select tblsubjects.Id , tblsubjects.subject_name from tblsubjects where tblsubjects.Id in (select DISTINCT tblclassexamsubmap.SubId from tblclassexamsubmap inner join tblclassexam on tblclassexam.Id = tblclassexamsubmap.ClassExamId  and tblclassexam.ClassId =" + _ClassID + "  and tblclassexam.ExamId =" + i_ExamId + " inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.Id and tblexamschedule.BatchId = " + _BatchId + ")  ";
        Subjects = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return Subjects;
    }

    public DataSet GetScheduledSubjectList(int _ClassID, int i_ExamId, int _BatchId)
    {
        DataSet Subjects = null;
        string sql = " SELECT distinct tblsubjects.Id,tblsubjects.subject_name from tblsubjects inner JOIN tblexammark on  tblexammark.SubjectId=tblsubjects.Id  inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on  tblclassexam.Id = tblexamschedule.ClassExamId        where tblexamschedule.BatchId=" + _BatchId + "    and   tblclassexam.ClassId=" + _ClassID + "   and tblclassexam.ExamId =" + i_ExamId + "  order by tblexammark.SubjectOrder ";
        // string sql = "select tblsubjects.Id , tblsubjects.subject_name from tblsubjects where tblsubjects.Id in (select DISTINCT tblclassexamsubmap.SubId from tblclassexamsubmap inner join tblclassexam on tblclassexam.Id = tblclassexamsubmap.ClassExamId  and tblclassexam.ClassId =" + _ClassID + "  and tblclassexam.ExamId =" + i_ExamId + " inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.Id and tblexamschedule.BatchId = " + _BatchId + ")  ";
        Subjects = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return Subjects;
    }

    public DataSet GetStudentMarkDetails(int _ClassID, int i_ExamId, int _BatchId, int _Periodid)
    {
        DataSet Students = null;
        string Query = "";
        int i = 0;
        int ScheduleId = GetExamSchid(_ClassID, i_ExamId, _BatchId, _Periodid);
        string sql = "select tblexammark.MarkColumn from tblexammark where tblexammark.ExamSchId=" + ScheduleId + " ORDER BY tblexammark.SubjectOrder";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                i++;
                if (i > 1)
                {
                    Query = Query + ",";
                }
                Query = Query + m_MyReader.GetValue(0).ToString();
            }
        }

        sql = "select DISTINCT tblstudent.StudentName, " + Query + ", tblstudentmark.TotalMax , tblstudentmark.TotalMark , tblstudentmark.`Avg`, Grade, Result, Rank  from tblstudentclassmap inner join tblstudent on tblstudentclassmap.StudentId = tblstudent.Id and tblstudentclassmap.BatchId=" + _BatchId + " and tblstudentclassmap.ClassId = " + _ClassID + " inner join tblstudentmark on tblstudentmark.StudId = tblstudentclassmap.StudentId where tblstudentmark.ExamSchId in (select tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblclassexam.Id =  tblexamschedule.ClassExamId  where tblexamschedule.PeriodId =" + _Periodid + " and tblexamschedule.BatchId = " + _BatchId + " and tblclassexam.ExamId=" + i_ExamId + " and tblclassexam.ClassId =" + _ClassID + ") and tblstudent.`Status`=1 order by tblstudent.StudentName asc";
        Students = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return Students;
    }

    private int GetExamSchid(int _ClassID, int i_ExamId, int _BatchId, int _Period)
    {
        int SchedId = -2;
        string sql = "select tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId and tblclassexam.ClassId =" + _ClassID + " and tblclassexam.ExamId = " + i_ExamId + "  where tblexamschedule.BatchId =" + _BatchId + " and tblexamschedule.PeriodId =" + _Period + "";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            SchedId = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        return SchedId;
    }
    # endregion


    #region ExamReport - SubjectWise

    public OdbcDataReader getCombinedExams(int classId)
    {
        string sql = "select distinct (tblexamcombmaster.id), tblexamcombmaster.ExamName from  tblexamcombmaster inner join tblexamcombmap on tblexamcombmaster.Id = tblexamcombmap.CombinedId where tblexamcombmap.ClassId = "+classId+" order by tblexamcombmaster.ExamName";
        return m_MysqlDb.ExecuteQuery(sql);
    }

    public OdbcDataReader getIndividualExams(int classId)
    {
        string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period from tblexammaster inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id  where tblclassexam.ClassId="+classId+"  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) order by tblexamschedule.id";
        return  m_MysqlDb.ExecuteQuery(sql);
    }

    public OdbcDataReader getSubjects(int _ClassId)
    {
        string sql = "select distinct(tblsubjects.Id), tblsubjects.subject_name from tblsubjects  inner join tblexammark on tblsubjects.id =  tblexammark.SubjectId inner join tblstudentmark on tblexammark.ExamSchId = tblstudentmark.ExamSchId inner join tblexamschedule on tblstudentmark.ExamSchId = tblexamschedule.Id inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId where tblclassexam.ClassId = " + _ClassId + " order by tblexammark.SubjectOrder";
        return m_MysqlDb.ExecuteQuery(sql);
    }

    public bool IsSubjectExistsInExam(int curBatch,int examtype, int examId, int subId, int ClassId, out string msg)
    {
        msg = "";
        string sql = "",markColumn="";
        //m_MyReader = null;
        OdbcDataReader m_MyReader1 = null;
        OdbcDataReader m_MyReader2 = null;
        int combflg = 0, exmflg = 0;
        bool exists = false;
        if (examtype == 1)
        {

            //Combined Exam - Find All Individual Exam and Check Whether Subject is in it
            //sql = "select distinct(tblexamcombmap.SchExamId) from tblexamcombmap where tblexamcombmap.CombinedId = " + examId + " and tblexamcombmap.ClassId =" + ClassId + "";
            sql = "select tblexamschedule.Id from tblexamcombmap inner join tblclassexam on tblclassexam.ClassId= tblexamcombmap.ClassId AND tblexamcombmap.ExamId= tblclassexam.ExamId inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.Id AND tblexamschedule.PeriodId= tblexamcombmap.PeriodId AND tblexamschedule.BatchId="+curBatch+" where  tblexamcombmap.CombinedId="+examId+"";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                combflg = 0;
                while (m_MyReader.Read())
                {
                    examId =int.Parse( m_MyReader.GetValue(0).ToString());
                    if (IsSubjectExistsInExam(curBatch,0, examId, subId, ClassId, out msg))
                    {
                        combflg = 1;
                    }
                   
                }
            }
            if (combflg == 1)
                exists = true;
            else
            {
                msg = "";
            }
        }
        else
        {
            int ind_flg = 0;
            exists = false;
            sql = "select distinct(tblexammark.MarkColumn) from tblexammark WHERE tblexammark.SubjectId =" + subId + " and  tblexammark.ExamSchId  = " + examId + " order by tblexammark.SubjectOrder";
            m_MyReader1 = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader1.HasRows)
            {
                while (m_MyReader1.Read())
                {
                    markColumn = m_MyReader1.GetValue(0).ToString();
                    sql = "select "+ markColumn +" from tblstudentmark where tblstudentmark.ExamSchId="+examId+"";
                    m_MyReader2 = m_MysqlDb.ExecuteQuery(sql);
                    if (m_MyReader2.HasRows)
                    {
                        while (m_MyReader2.Read())
                        {
                            if (m_MyReader2.GetValue(0).ToString() != "")
                            {

                            }
                            else ind_flg = 1;
                        }
                    }
                    if (ind_flg == 0)
                    {
                        //Subject - Mark - is in tblStudentMark
                        exists = true;
                    }
                    else
                    {

                    }
                }
            }
        }

        return exists;
    }

    public DataSet getStudents(int _classId)
    {
        string sql = "select tblstudent.id, tblstudent.StudentName from tblstudent inner join tblstudentclassmap on tblstudent.id = tblstudentclassmap.StudentId where tblstudent.`Status`=1 and tblstudentclassmap.ClassId = " + _classId + "";
        return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
    }

    public double getExamMark(int curBatch,int examtype, int examId, int studid, int subId, int ClassId)
    {
        OdbcDataReader m_MyReader1 = null;
        OdbcDataReader m_MyReader2 = null;
        string sql = "", markColumn="";
        double mark = 0,ind_mark=0;
        if (examtype == 1) //Combined
        {
            //sql = "select distinct(tblexamcombmap.SchExamId) from tblexamcombmap where tblexamcombmap.CombinedId = " + examId + " and tblexamcombmap.ClassId =" + ClassId + "";
            sql = "select tblexamschedule.Id from tblexamcombmap inner join tblclassexam on tblclassexam.ClassId= tblexamcombmap.ClassId AND tblexamcombmap.ExamId= tblclassexam.ExamId inner join tblexamschedule on tblexamschedule.ClassExamId= tblclassexam.Id AND tblexamschedule.PeriodId= tblexamcombmap.PeriodId AND tblexamschedule.BatchId=" + curBatch + " where  tblexamcombmap.CombinedId=" + examId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    examId = int.Parse(m_MyReader.GetValue(0).ToString());
                    ind_mark = getExamMark(curBatch,0, examId, studid, subId, ClassId);
                    if (ind_mark == -1)
                    {
                        ind_mark = 0;
                    }
                    mark = mark + ind_mark;
                }
            }
        }
        else
        {
            sql = "select distinct(tblexammark.MarkColumn) from tblexammark WHERE tblexammark.SubjectId =" + subId + " and  tblexammark.ExamSchId  = " + examId + " order by tblexammark.SubjectOrder";
            m_MyReader1 = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader1.HasRows)
            {
                while (m_MyReader1.Read())
                {
                    markColumn = m_MyReader1.GetValue(0).ToString();
                    sql = "select " + markColumn + " from tblstudentmark where tblstudentmark.ExamSchId=" + examId + "  and  tblstudentmark.StudId = "+ studid +"";
                    m_MyReader2 = m_MysqlDb.ExecuteQuery(sql);
                    if (m_MyReader2.HasRows)
                    {
                        while (m_MyReader2.Read())
                        {
                            if (m_MyReader2.GetValue(0).ToString() != "")
                            {
                                mark = double.Parse(m_MyReader2.GetValue(0).ToString());
                            }
                            else
                            {
                                mark = 0;
                            }
                        }
                    }
                    else
                    {
                    }
                }
            }
        }

        return mark;

    }


    #endregion





    public void RemoveSubject(int _SubjectId, int _classid)
    {
        string sql = " DELETE from tblclasssubmap where tblclasssubmap.ClassId=" + _classid + " and tblclasssubmap.SubjectId= " + _SubjectId;
        m_MysqlDb.ExecuteQuery(sql);
    }

    public bool IsClassStaff(int _ClassId, int _StaffId,int _SubjectId)
    {
        bool _valide = false;
        string sql = "SELECT * FROM tblclassstaffmap where ClassId=" + _ClassId + " And StaffId=" + _StaffId + " and SubjectId=" + _SubjectId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _valide = true;
        }
        m_MyReader.Close();
        return _valide;
    }

    public void AddStaffToClassStaffMapWithSubject(int _StaffId, int _ClassId, int _SubjectId)
    {
        string sql = "";
        //sql = "Delete From tblclassstaffmap where subjectId=" + _SubjectId + " and ClassId=" + _ClassId + "";
        //m_MysqlDb.ExecuteQuery(sql);
        sql = "INSERT INTO tblclassstaffmap(ClassId,SubjectId,StaffId) VALUES (" + _ClassId + "," + _SubjectId + ", " + _StaffId + ")";
        m_MysqlDb.ExecuteQuery(sql);
    }
    public void UpdateStaffToClassStaffMapWithSubject(int StaffId, int ClassId, int SubjectId)
    {
        string sql = "";
        sql = "Update tblclassstaffmap set StaffId=" + StaffId + " where ClassId=" + ClassId + " and SubjectId=" + SubjectId + " and StaffId=-1";
        m_MysqlDb.ExecuteQuery(sql);
    }
    


    public void RemoveStaffFromClassStaffMapWithSubject(int _StaffId, int _ClassId, int _SubjectId)
    {
        string sql = "DELETE FROM tblclassstaffmap WHERE ClassId=" + _ClassId + " and SubjectId=" + _SubjectId + " and StaffId=" + _StaffId;
        m_MysqlDb.ExecuteQuery(sql);
    }

    //public void AddStaffToClassStaffMapForTimeTable(int _StaffId, int _ClassId)
    //{
    //    string sql = "insert into tbltime_classstaffmap(StaffId,ClassId) VALUES (" +_StaffId+","+ _ClassId + ")";
    //    m_MysqlDb.ExecuteQuery(sql);
    //}

    //public void RemoveFromClassStaffMapForTimeTable(int _StaffId, int _ClassId)
    //{
    //    string sql = "DELETE FROM tbltime_classstaffmap WHERE ClassId=" + _ClassId + " and StaffId=" + _StaffId;
    //    m_MysqlDb.ExecuteQuery(sql);
    //}

    public void AddStaffToStaffSubjectMapTable(int _StaffId, int _SubjectId)
    {
        string sql = "insert into tblstaffsubjectmap(StaffId,SubjectId) VALUES (" + _StaffId + "," + _SubjectId + ")";
        m_MysqlDb.ExecuteQuery(sql);
    }

    public void RemoveStaffFromStaffSubjectMapTable(int _StaffId, int _SubjectId)
    {
        string sql = "DELETE FROM tblstaffsubjectmap WHERE SubjectId=" + _SubjectId + " and StaffId=" + _StaffId;
        m_MysqlDb.ExecuteQuery(sql);
    }

    public int GetTotalNumberOfStudentsInClass(int _ClassId,int _BatchId)
    {
        int _TotalSeats = 0;
        string sql = "select count(tblstudentclassmap.StudentId) from tblstudentclassmap INNER JOIN tblview_student ON tblstudentclassmap.StudentId=tblview_student.Id where tblview_student.`Status`=1 AND tblstudentclassmap.ClassId=" + _ClassId + " and tblstudentclassmap.BatchId=" + _BatchId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int.TryParse(m_MyReader.GetValue(0).ToString(), out _TotalSeats);
        }
        return _TotalSeats;
    }

    public void getClassStuds(int classid, int batchid, out int students, out int boys, out int girls)
    {
        students = 0; boys = 0;   girls = 0;
        string sql = "select COUNT(tblstudentclassmap.StudentId) from tblstudentclassmap INNER JOIN tblstudent on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId= "+batchid+" AND tblstudentclassmap.ClassId= "+classid+" Order by tblstudentclassmap.RollNo ASC";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                students =int.Parse( m_MyReader.GetValue(0).ToString());
            }
        }
        sql = "select COUNT(tblstudentclassmap.StudentId) from tblstudentclassmap INNER JOIN tblstudent on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 and tblstudent.Sex = 'Male' AND tblstudentclassmap.BatchId= " + batchid + " AND tblstudentclassmap.ClassId= " + classid + " Order by tblstudentclassmap.RollNo ASC";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                boys = int.Parse(m_MyReader.GetValue(0).ToString());
            }
        }
        sql = "select COUNT(tblstudentclassmap.StudentId) from tblstudentclassmap INNER JOIN tblstudent on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 and tblstudent.Sex = 'FeMale' AND tblstudentclassmap.BatchId= " + batchid + " AND tblstudentclassmap.ClassId= " + classid + " Order by tblstudentclassmap.RollNo ASC";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                girls = int.Parse(m_MyReader.GetValue(0).ToString());
            }
        }
    }

    public string  ReligionName(string  religId)
    {
        string religion = "Unknown";
        string sql = "select Religion from tblreligion where id = " + religId + "";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                if (m_MyReader.GetValue(0).ToString() != "")
                {
                    religion = m_MyReader.GetValue(0).ToString();
                }
            }
        }
        return religion;
    }

    public string  CastName(string  castId)
    {
        string cast = "Unknown";
        string sql = "select CastName from tblcast where id = " + castId + "";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                if (m_MyReader.GetValue(0).ToString() != "")
                {
                    cast = m_MyReader.GetValue(0).ToString();
                }
            }
        }
        return cast;
    }



    public void DeleteStaffIfAlreadyAssigened(int ClassId, int SbjectId)
    {
        string sql = "";
        sql = "Delete From tblclassstaffmap where subjectId=" + SbjectId + " and ClassId=" + ClassId + "";
        m_MysqlDb.ExecuteQuery(sql);
    }



    public void UpdateRemovedStaff(int staffId, int ClassId, int SubjectId)
    {
        string sql = "";
        sql = "Update tblclassstaffmap set staffId=-1 where ClassId=" + ClassId + " and SubjectId="+SubjectId+" and StaffId="+staffId+"";
        m_MysqlDb.ExecuteQuery(sql);
    }



    public double GetMaximumMarkOfSubject(int _ClassID, int i_ExamId, int _BatchId, int subjectID)
    {
        double MaxMark = 0;
        string sql="";
        sql = "SELECT tblclassexamsubmap.MaxMark from tblclassexam inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblclassexam.Id where tblclassexamsubmap.SubId=" + subjectID + " AND tblclassexam.ClassId=" + _ClassID + " AND tblclassexam.ExamId=" + i_ExamId + "";
 
       // sql = " select tblclassexamsubmap.MaxMark from tblclassexamsubmap inner join tblclassexam on tblclassexam.Id = tblclassexamsubmap.ClassExamId and tblclassexam.ClassId =" + _ClassID + "  and tblclassexam.ExamId =" + i_ExamId + "        inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.Id and tblexamschedule.BatchId= " + _BatchId + " where tblclassexamsubmap.SubId=" + subjectID + "";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            MaxMark = double.Parse(m_MyReader.GetValue(0).ToString());
        }
        return MaxMark;
       //inner join tblclassexam on tblclassexam.Id = tblclassexamsubmap.ClassExamId  
        //and tblclassexam.ClassId =" + _ClassID + "  and tblclassexam.ExamId =" + i_ExamId + "
        //inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.Id and tblexamschedule.BatchId
        //    = " + _BatchId + "
    }

    public string GetStandardIdfromClassId(string _selectedClassId)
    {
        string sql = "select tblclass.Standard from tblclass where tblclass.Id=" + _selectedClassId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
              return m_MyReader.GetValue(0).ToString();
           }
           return "0";
    }

    public DataSet getStudentExcelFiledMap()
    {
        string sql = "select SoftwareField,ExcelFiled,IsDynamic,IsMandatory from tblexcelfieldmap where IsMandatory = 0";
        return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
    }
    public DataSet getStudentDetailExcelFiledMap()
    {
        string sql = "select SoftwareField,ExcelFiled,IsDynamic,IsMandatory from tblexcelfieldmap ";
        return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
    }
}
