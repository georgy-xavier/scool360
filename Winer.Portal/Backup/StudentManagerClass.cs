using System;
using System.Web;
//using System.Web.Services;
//using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Data.Odbc;
using System.Data;
using System.Text;
using WinBase;
using System.Collections.Generic;
using System.Globalization;
public class StudentManagerClass:KnowinGen
{
   
    public MysqlClass m_MysqlDb;
    public MysqlClass m_TransationDb = null;
    public DBLogClass m_DbLog = null;
    private OdbcDataReader m_MyReader = null;
    private OdbcDataReader m_MyReader1 = null;
    private DataSet m_MyDataSet = null;
    private string m_StudentMenuStr;
    private string m_SubStudentMenuStr;
    private int m_Type=-1;
    private int m_CoustomFieldCount=-1;
    private string m_AdmitionNoPrefix = "-1";
    public StudentManagerClass(KnowinGen _Prntobj)
    {
        m_Parent = _Prntobj;
        m_MyODBCConn = m_Parent.ODBCconnection;
        m_UserName = m_Parent.LoginUserName;
        m_MysqlDb = new MysqlClass(this);
        m_DbLog = new DBLogClass(m_MysqlDb);
        m_StudentMenuStr = "";
        m_SubStudentMenuStr = "";
    
    }
    public StudentManagerClass(MysqlClass _Msqlobj)
    {
        m_MysqlDb = _Msqlobj;
    }
    ~StudentManagerClass()
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
    public int CoustomFieldCount
    {
        get
        {
            if (m_CoustomFieldCount == -1)
            {
                m_CoustomFieldCount=GetCustStudFieldCount();
            }
            return m_CoustomFieldCount;
        }

    }
    public string GetStudentMangMenuString(int _roleid)
    {
        CLogging logger = CLogging.GetLogObject();
        string _MenuStr;
        if (m_StudentMenuStr == "")
        {


            _MenuStr = "<ul><li><a href=\"SearchStudent.aspx\">Search Student</a></li>";
            logger.LogToFile("GetStudentMangMenuString", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            string sql = "SELECT DISTINCT tblaction.MenuName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND (( tblroleactionmap.ModuleId=2 AND tblaction.ActionType='Link') Or tblaction.ActionType='StudLink') ";
            logger.LogToFile("GetStudentMangMenuString", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                logger.LogToFile("GetStudentMangMenuString", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                while (m_MyReader.Read())
                {
                    _MenuStr = _MenuStr + "<li><a href=\"" + m_MyReader.GetValue(1).ToString() + "\">" + m_MyReader.GetValue(0).ToString() + "</a></li>";
                }

            }
            _MenuStr = _MenuStr + "</ul>";
            logger.LogToFile("GetStudentMangMenuString", " Closing MyReader ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Close();
            m_StudentMenuStr = _MenuStr;

        }
        else
        {
            _MenuStr = m_StudentMenuStr;
        }
        logger.LogToFile("GetStudentMangMenuString", "Exiting from module" , 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _MenuStr;

    }


    public bool AvailableAdminNo(string _AdmisionNo)
    {
        CLogging logger = CLogging.GetLogObject();
        bool _valide;
        logger.LogToFile("AvailableAdminNo", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "SELECT AdmitionNo FROM tblstudent where AdmitionNo='" + _AdmisionNo + "'";
        logger.LogToFile("AvailableAdminNo", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            logger.LogToFile("AvailableAdminNo", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            _valide = false;
        }
        else
        {
            logger.LogToFile("AvailableAdminNo", "Reading Failure" + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            _valide = true;
        }
        logger.LogToFile("AvailableAdminNo", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader.Close();
        logger.LogToFile("AvailableAdminNo", "Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _valide;
    }

    
   

    public string GetSubStudentMangMenuString(int _roleid,int _Type)
    {
        string _Field="Live";
        if (_Type == 1)
            _Field = "Live";
        if (_Type == 2)
            _Field = "History";
        if(_Type == 3)
            _Field = "WaitingForPromotion";
        string _MenuStr;
        if (m_SubStudentMenuStr == "" || m_Type != _Type)
        {
            _MenuStr = "<ul class=\"block\"><li><a href=\"StudentDetails.aspx\">Student Details</a></li>";
            string sql = "SELECT DISTINCT tblaction.MenuName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId INNER JOIN tblstudentsubmenu on tblstudentsubmenu.ActionId = tblaction.Id and tblstudentsubmenu." + _Field + "=1 WHERE  tblroleactionmap.RoleId=" + _roleid + " AND ((tblroleactionmap.ModuleId=2 AND tblaction.ActionType='SubLink') or tblaction.ActionType='StudSubLink') order by tblaction.`Order` ";
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
            m_SubStudentMenuStr = _MenuStr;
            m_Type = _Type;
        }
        else
        {
            _MenuStr = m_SubStudentMenuStr;
        }
        return _MenuStr;

    }



    //public int CreateStudent(string _Name, string _Sex, string _Dob, string _BloodGrp, string _FGName, string _FatherEduQuali, string _MotherName, string _MotherEduQuali, string _FatherOccupation, double _AnualIncome, string _Nationality, string _Religion, string _Cast, string _MotherTongue, string _OtherLanguage, string _Email, string _Address, string _Location, int _pin, string _State, string _ResidencePh, string _OfficePh, string _AdminNo, string _JoiningDate, int _Std, int _Class, string _JoinBatch, string _Wish2Take, string _FeeReceiptNo, int _CurrentBatchId, string _StayingWith, string _PlaceOfBirth, string _Village, string _Town, string _Taluk, string _District, int _NoBro, int _NoSys, string _Addresspresent, int _StudentType, int _AdmissionType, string _ReligionCaste)
    //{
    //    CheckRegilionExitsInTheTbl(_Religion);


    //    string _RCaste;
    //    if (_Cast == "")
    //    {
    //        _RCaste = _ReligionCaste;
    //    }
    //    else
    //    {

    //        _RCaste = CreateCateIfExitGetCastId(_Cast, _Religion);

    //    }
    //    CLogging logger = CLogging.GetLogObject();
    //    MysqlClass MysqlTranDb;
    //    MysqlTranDb = new MysqlClass(this);
    //    int _studentid = -1, flag = -1;
    //    DateTime Date_Dob = DateTime.Parse(_Dob);
    //    DateTime Date_JoiningDate = DateTime.Parse(_JoiningDate);
    //    string _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    //    DateTime Today = DateTime.Parse(_strdtNow);
    //    if (_AdminNo == "")
    //    {
    //        _AdminNo = "A" + Today.Year + "-";
    //        flag = 1;
    //    }
    //    _AdminNo = Adno(_AdminNo, _studentid);
    //    DateTime Date_LeavingDate = DateTime.Parse(_DateOfLeaving);
    //    try
    //    {

    //        logger.LogToFile("CreateStudent", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
    //        MysqlTranDb.MyBeginTransaction();
    //        string sql = "INSERT INTO tblstudent(StudentName,GardianName,AdmitionNo,DOB,Sex,Address,BloodGroup,Religion,Cast,DateofJoining,Status,Email,Location,Pin,State,Nationality,FatherEduQuali,MothersName,MotherEduQuali,FatherOccupation,AnnualIncome,ScheduledcasteType,MotherTongue,OtherLanguages,ResidencePhNo,OfficePhNo,FeeReceiptNo,1stLanguage,StayingWith,PlaceOfBirth,Village,Town,Taluk,District,NumberofBrothers,NumberOfSysters,JoinBatch,CreationTime,Addresspresent,StudTypeId,AdmissionTypeId ) VALUES ('" + _Name + "', '" + _FGName + "','" + _AdminNo + "','" + Date_Dob.Date.ToString("s") + "','" + _Sex + "','" + _Address + "','" + _BloodGrp + "','" + _Religion + "','" + _RCaste + "','" + Date_JoiningDate.Date.ToString("s") + "',1,'" + _Email + "','" + _Location + "'," + _pin + ",'" + _State + "','" + _Nationality + "','" + _FatherEduQuali + "','" + _MotherName + "','" + _MotherEduQuali + "','" + _FatherOccupation + "'," + _AnualIncome + ",'','" + _MotherTongue + "','" + _OtherLanguage + "','" + _ResidencePh + "','" + _OfficePh + "','" + _FeeReceiptNo + "','" + _Wish2Take + "','" + _StayingWith + "','" + _PlaceOfBirth + "','" + _Village + "','" + _Town + "','" + _Taluk + "','" + _District + "'," + _NoBro + ", " + _NoSys + ",'" + _JoinBatch + "','" + _strdtNow + "','" + _Addresspresent + "', " + _StudentType + " , " + _AdmissionType + ")";
    //        logger.LogToFile("CreateStudent", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    //        MysqlTranDb.TransExecuteQuery(sql);

    //        logger.LogToFile("CreateStudent", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
    //        sql = "SELECT Id FROM tblstudent where CreationTime='" + _strdtNow + "' AND StudentName='" + _Name + "' AND GardianName= '" + _FGName + "' order by Id desc";
    //        logger.LogToFile("CreateStudent", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    //        m_MyReader = MysqlTranDb.ExecuteQuery(sql);
    //        if (m_MyReader.HasRows)
    //        {
    //            logger.LogToFile("CreateStudent", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    //            m_MyReader.Read();
    //            _studentid = int.Parse(m_MyReader.GetValue(0).ToString());
    //        }
    //        else
    //        {
    //            logger.LogToFile("CreateStudent", " Reading Failure " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    //            _studentid = -1;
    //        }
    //        if (_studentid != -1)
    //        {

    //            if (flag == 1)
    //            {
    //                _AdminNo = _AdminNo + _studentid;
    //            }
    //            logger.LogToFile("CreateStudent", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
    //            sql = "UPDATE tblstudent SET AdmitionNo= '" + _AdminNo + "' WHERE Id='" + _studentid + "'";
    //            logger.LogToFile("CreateStudent", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    //            MysqlTranDb.TransExecuteQuery(sql);
    //            logger.LogToFile("CreateStudent", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
    //            sql = "INSERT INTO tblstudentclassmap (StudentId,ClassId,Standard,BatchId) VALUES (" + _studentid + ", " + _Class + ", '" + _Std + "', " + _CurrentBatchId + ")";
    //            logger.LogToFile("CreateStudent", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    //            MysqlTranDb.TransExecuteQuery(sql);
    //            logger.LogToFile("CreateStudent", "transaction commiting", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
    //            MysqlTranDb.TransactionCommit();
    //        }
    //        else
    //        {
    //            MysqlTranDb.TransactionRollback();
    //        }
    //    }
    //    catch (Exception exc)
    //    {
    //        MysqlTranDb.TransactionRollback();
    //        logger.LogToFile("CreateStudent", "throws Error " + exc.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
    //    }
    //    logger.LogToFile("CreateStudent", "Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    //    return _studentid;
    //}

   

    public void CheckRegilionExitsInTheTbl(string _Religion)
    {
        string sql = "select tblreligion.Id  from tblreligion where tblreligion.Religion='" + _Religion + "' ";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (!m_MyReader.HasRows)
        {
            sql = "insert into tblreligion (Religion) values ('" +_Religion +"')";
                 m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }
    }

    //public string CreateCateIfExitGetCastId(string _Cast, string _Religion)
    //{
    //    string _castId;
    //    if (!checkCastExistInTheTbl(_Cast))
    //    {
    //        insertIntoCastTBlAndReligioncastMapTBl(_Cast, _Religion, out _castId);
    //    }
    //    else
    //    {
    //        _castId = getcastID(_Cast);
    //    }

    //    return _castId;
    //}

    //private void insertIntoCastTBlAndReligioncastMapTBl(string _Cast, string _Religion, out string _castId)
    //{
    //    CLogging logger = CLogging.GetLogObject();
    //    _castId = "";
    //    string sql = "INSERT into tblcast (castname) values ('" + _Cast + "')";        
    //    logger.LogToFile("insertIntoCastTBlAndReligioncastMapTBl", "Inserting into cast tbl", 'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
    //    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    //    _castId = getcastID(_Cast);
        
    //    sql = "select tblreligion.Id from tblreligion where tblreligion.Religion='" +_Religion + "'";
    //    logger.LogToFile("insertIntoCastTBlAndReligioncastMapTBl", "Selecting Religion Id frm TblReligion", 'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
    //    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    //    if (m_MyReader.HasRows)
    //    {
    //        sql = "insert into tblreligioncastmap (ReligionId,CasteId) values (" + int.Parse(m_MyReader.GetValue(0).ToString()) + "," + _castId + ")";
    //        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    //    }

    //}

    //private string getcastID(string _Cast)
    //{
    //    OdbcDataReader m_MyReaderTemp = null;
    //    string _castId = "-1";
    //    CLogging logger = CLogging.GetLogObject();
    //    string sql = "select tblcast.Id from tblcast where tblcast.castname='" + _Cast + "'";
    //    logger.LogToFile("getcastID", "Selecting cast iD frm Tblcast", 'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
    //    m_MyReaderTemp = m_MysqlDb.ExecuteQuery(sql);
    //    if (m_MyReaderTemp.HasRows)
    //    {
    //        _castId = m_MyReaderTemp.GetValue(0).ToString();
    //    }
    //    return _castId;
    //}

    private bool checkCastExistInTheTbl(string _Cast)
    {
        bool _flag = false;
        string sql = "Select tblcast.castname from tblcast where tblcast.castname='" + _Cast + "'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if(m_MyReader.HasRows)
            {
                _flag = true;
            }
        return _flag;
    }
  


    public void CreatePreSchool(int _UserId, string _PreviousSchool, string _Scholarship, string _StadardsCoverd, string _DateOfLeaving, string _TCNumber, string _Reason, string _Innoculated, string _Pre1stLan, string _LangStuded, string _MediumOfInstruct)
    {
        CLogging logger = CLogging.GetLogObject();
        String sql;
        DateTime Date_PreviousSchoolLeaving = General.GetDateTimeFromText(_DateOfLeaving);
        logger.LogToFile("CreatePreSchool", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        sql = "INSERT INTO tblpreviousschooldata(StudentId,NameofSchool,ConcessionScholarshipDetails,StandarsCoveredDetails,DateofLeaving,Reason,TCNumber,Innoculated,1stLanguage,LanguagesStudied,MediumOfInstruction) VALUES (" + _UserId + ",'" + _PreviousSchool + "','" + _Scholarship + "','" + _StadardsCoverd + "','" + Date_PreviousSchoolLeaving.Date.ToString("s") + "','" + _Reason + "','" + _TCNumber + "','" + _Innoculated + "','" + _Pre1stLan + "','" + _LangStuded + "','" + _MediumOfInstruct + "')";
        logger.LogToFile("CreatePreSchool", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        logger.LogToFile("CreatePreSchool", "Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    }

   


    public bool HasImage(int _UsrId, out string preimage)
    {
        CLogging logger = CLogging.GetLogObject();
        preimage = "";
        bool ImageFlag = false;
        logger.LogToFile("HasImage", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        String Sql = "SELECT FilePath FROM tblfileurl WHERE UserId=" + _UsrId + " AND  Type='StudentImage'";
        logger.LogToFile("HasImage", " Executing Query " + Sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
        if (m_MyReader.HasRows)
        {
            logger.LogToFile("HasImage", " Reading Success " + Sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            preimage = m_MyReader.GetValue(0).ToString();
            logger.LogToFile("HasImage", " Setting ImageFlag to true " + Sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            ImageFlag = true;
        }
        logger.LogToFile("HasImage", " returning imageflag and Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return ImageFlag;
    }

    public string GetMark(int _SubId, int _ExamId, int _BatchId, int _StudId)
    {
        CLogging logger = CLogging.GetLogObject();
        string _Mark;
        logger.LogToFile("GetMark", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "select tblexammark.MarkColumn from tblexammark inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId where tblexammark.SubjectId=" + _SubId + " and tblexamschedule.BatchId=" + _BatchId + " and tblexamschedule.ExamId=" + _ExamId + " order by tblexammark.SubjectOrder";
        logger.LogToFile("GetMark", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            logger.LogToFile("GetMark", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Read();
            //_Mark = GetMarkfromColumn(_StudId, _ExamId, _BatchId, m_MyReader.GetValue(0).ToString());
            _Mark = "-1";
        }
        else
        {
            logger.LogToFile("GetMark", " Reading Failure " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            _Mark = "-1";
        }
        logger.LogToFile("GetMark", " returning mark and Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _Mark;

    }

    public string GetMarkfromColumn(int _studId, int _examschid, string _column)
    {
        CLogging logger = CLogging.GetLogObject();
        string _ExamMark;
        logger.LogToFile("GetMarkfromColumn", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string sql = "Select tblstudentmark." + _column + " From  tblstudentmark where tblstudentmark.ExamSchId=" + _examschid + " AND tblstudentmark.StudId=" + _studId;
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

    public void GenerateTC(string _PupilName, string _SchoolName, string _AdmissionNo, string _Cumulative, string _Sex, string _NameOfFather, string _Nationality, string _Religion, string _Cast, string _CastType, string _Dob, string _CurrentStd, string _LangStd, string _MediumOfIns, string _Syllabus, string _DateOfAdmission, string _Quali_Promo, string _Feesdue, string _FeeCon, string _Scholarship, string _MedicalyExmnd, string _LastAttendance, string _AppTcDate, string _TcDate, string _TotalSchoolDays, string _DaysAttended, string _CC,int _StudentId,int _LastBatch,string NameOfMother,string ResAddress,string ResonForLeaving,string SubjectStudied,string LastExamDetails,string TCNumber,string newschool,string LastClsAdmissionDate)
    {

        CLogging logger = CLogging.GetLogObject();
        string m_TcNo="1"; 
        DateTime Dob, LastAttendance, AppTcDate, TcDate,LastAdmissionDate=System.DateTime.Now.Date;
        Dob = General.GetDateTimeFromText(_Dob);
       // DateOfAdmission = DateTime.Parse(_DateOfAdmission);
        LastAttendance = General.GetDateTimeFromText(_LastAttendance);
        AppTcDate = General.GetDateTimeFromText(_AppTcDate);
        TcDate = General.GetDateTimeFromText(_TcDate);
        if (LastClsAdmissionDate!="")
        LastAdmissionDate = General.GetDateTimeFromText(LastClsAdmissionDate);


        
        string _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        try
        {
            logger.LogToFile("GenerateTC", "Checking student has TC table or not", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            if (!StudentHasTcTable(_StudentId))
            {
                logger.LogToFile("GenerateTC", "does not have tc", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                logger.LogToFile("GenerateTC", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                string Sql = "INSERT INTO tbltc (StudentId,Status,TcNumber,StudentName,NameOFSchool,AdmissionNo,CumulativeRecNo,Sex,NameOfFather,Nationality,Religion,Cast,CasteType,Dob,CurrStandard,LangStudied,MediumOfIns,Syllabus,DateOfAdmission,WhetherQualiForPromo,FeesDue,FeeConcessions,SCholarship,MedicalyExmnd,LastAttendanceDate,AppForTcRecvedDate,DateOfIssueOfTC,NoOfSchoolDays,SchoolDaysAttended,CharacterNConduct,CreationDate,LastBatchId,NameOfMother,ResAddress,ResonForLeaving,SubjectStudied,LastExamDetails,NextClass,LastClassDate)VALUES(" + _StudentId + ",1,'" + m_TcNo + "','" + _PupilName + "','" + _SchoolName + "','" + _AdmissionNo + "','" + _Cumulative + "','" + _Sex + "','" + _NameOfFather + "','" + _Nationality + "','" + _Religion + "','" + _Cast + "','" + _CastType + "','" + Dob.Date.ToString("s") + "','" + _CurrentStd + "','" + _LangStd + "','" + _MediumOfIns + "','" + _Syllabus + "','" + _DateOfAdmission + "','" + _Quali_Promo + "','" + _Feesdue + "','" + _FeeCon + "','" + _Scholarship + "','" + _MedicalyExmnd + "','" + LastAttendance.Date.ToString("s") + "','" + AppTcDate.Date.ToString("s") + "','" + TcDate.Date.ToString("s") + "','" + _TotalSchoolDays + "','" + _DaysAttended + "','" + _CC + "','" + _strdtNow + "'," + _LastBatch + ",'" + NameOfMother + "','" + ResAddress + "','" + ResonForLeaving + "','" + SubjectStudied + "','" + LastExamDetails + "','" + newschool + "','" + LastAdmissionDate.Date.ToString("s") + "')";
                logger.LogToFile("GenerateTC", "Executing Query", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
                logger.LogToFile("GenerateTC", "GENERATING TC NUMBER", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                
                if (TCNumber == "")


                    m_TcNo = GenerateTcNo(_StudentId);//GENERATING TC NUMBER

                else

                    m_TcNo = TCNumber;

                logger.LogToFile("GenerateTC", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                Sql = "UPDATE tbltc SET TcNumber='" + m_TcNo + "' WHERE StudentId=" + _StudentId;
                logger.LogToFile("GenerateTC", "Executing Query", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            }
            else
            {
                logger.LogToFile("GenerateTC", "student has tc", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                logger.LogToFile("GenerateTC", "GENERATING TC NUMBER", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                m_TcNo = GenerateTcNo(_StudentId);
                logger.LogToFile("GenerateTC", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                string Sql = "UPDATE tbltc SET TcNumber='" + m_TcNo + "',StudentName='" + _PupilName + "',NameOFSchool='" + _SchoolName + "',AdmissionNo='" + _AdmissionNo + "',CumulativeRecNo='" + _Cumulative + "',Sex='" + _Sex + "',NameOfFather='" + _NameOfFather + "',Nationality='" + _Nationality + "',Religion='" + _Religion + "',Cast='" + _Cast + "',CasteType='" + _CastType + "',Dob='" + Dob.Date.ToString("s") + "',CurrStandard='" + _CurrentStd + "',LangStudied='" + _LangStd + "',MediumOfIns='" + _MediumOfIns + "',Syllabus='" + _Syllabus + "',DateOfAdmission='" + _DateOfAdmission + "',WhetherQualiForPromo='" + _Quali_Promo + "',FeesDue='" + _Feesdue + "',FeeConcessions='" + _FeeCon + "',SCholarship='" + _Scholarship + "',MedicalyExmnd='" + _MedicalyExmnd + "',LastAttendanceDate='" + LastAttendance.Date.ToString("s") + "',AppForTcRecvedDate='" + AppTcDate.Date.ToString("s") + "',DateOfIssueOfTC='" + TcDate.Date.ToString("s") + "',NoOfSchoolDays='" + _TotalSchoolDays + "',SchoolDaysAttended='" + _DaysAttended + "',CharacterNConduct='" + _CC + "',CreationDate='" + _strdtNow + "',LastBatchId=" + _LastBatch + " WHERE StudentId=" + _StudentId;
                logger.LogToFile("GenerateTC", "Executing Query", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            }
        }
        catch(Exception exc)
        {
            logger.LogToFile("GenerateTC", "throws Error " + exc.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);       
        }

        logger.LogToFile("GenerateTC", " Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    }
    private string GenerateTcNo(int _StudentId)
    {
        CLogging logger = CLogging.GetLogObject();
        string _strdtNow, Year, Id = "", TcNumber;
        _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        DateTime _StandardNow = DateTime.Now;
        Year = _StandardNow.Year.ToString();
        logger.LogToFile("GenerateTcNo", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        String Sql = "SELECT Id FROM tbltc WHERE StudentId=" + _StudentId;//GETTING THE 'Id' OF THE USER
        logger.LogToFile("GenerateTcNo", "Executing Query", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
        if (m_MyReader.HasRows)
        {
            logger.LogToFile("GenerateTcNo", "Reading Success", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            Id = m_MyReader.GetValue(0).ToString();
        }
        logger.LogToFile("GenerateTcNo", "Combinig current year with tctable id", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        TcNumber = Year + "-" + Id;//COMBINING WITH CURRENT YEAR
        logger.LogToFile("GenerateTcNo", " returning TcNumber and Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return TcNumber;
    }
    private bool StudentHasTcTable(int _StudentId)
    {
        CLogging logger = CLogging.GetLogObject();
        bool HasTC = false;
        logger.LogToFile("StudentHasTcTable", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string Sql = "SELECT TcNumber FROM tbltc WHERE StudentId=" + _StudentId;
        logger.LogToFile("StudentHasTcTable", "Executing Query", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
        if (m_MyReader.HasRows)
        {
            logger.LogToFile("StudentHasTcTable", "Reading Success and setting HasTc to true", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            HasTC = true;
        }
        logger.LogToFile("StudentHasTcTable", " returning HasTc and Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return HasTC;
    }

    public bool HasFeesDue(int _studId)
    {
        CLogging logger = CLogging.GetLogObject();
        bool _valid = false;
        logger.LogToFile("HasFeesDue", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        string Sql = "Select tblfeestudent.Id from tblfeestudent inner join tblfeeschedule on tblfeestudent.SchId=tblfeeschedule.Id where tblfeestudent.StudId=" + _studId + " AND tblfeeschedule.DueDate <= CURRENT_DATE() AND  tblfeestudent.Status  NOT in ('Paid','fee Exemtion')";
        logger.LogToFile("HasFeesDue", "Executing Query", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
        if (m_MyReader.HasRows)
        {
            logger.LogToFile("HasFeesDue", "Reading Success and setting _valid to true", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            _valid = true;
        }
        logger.LogToFile("HasFeesDue", "closing myReader and  returning _valid and Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader.Close();
        return _valid;
    }

    public void MoveStudentToHistory(int _StudentId)
    {
        CLogging logger = CLogging.GetLogObject();
        MysqlClass MysqlTranDb;
        MysqlTranDb = new MysqlClass(this);
        string _strdtNow;
        _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        DateTime _StandardNow = DateTime.Now;
        try
        {
            MysqlTranDb.MyBeginTransaction();
            logger.LogToFile("MoveStudentToHistory", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
            string Sql = "UPDATE tblstudent SET DateOfLeaving='" + _StandardNow.Date.ToString("s") + "',Status=0 WHERE Id=" + _StudentId;
            logger.LogToFile("MoveStudentToHistory", "Executing Query", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            MysqlTranDb.TransExecuteQuery(Sql);
            logger.LogToFile("MoveStudentToHistory", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
            Sql = "UPDATE tbltc SET Status=0 WHERE StudentId=" + _StudentId;
            logger.LogToFile("MoveStudentToHistory", "Executing Query", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            MysqlTranDb.TransExecuteQuery(Sql);
            Sql = "delete from tblbookissue where tblbookissue.UserId=" + _StudentId + " and tblbookissue.UserType=1";
            MysqlTranDb.TransExecuteQuery(Sql);
            Sql = "delete from tblbookbooking where tblbookbooking.UserId=" + _StudentId + " and tblbookbooking.UserType=1";
            MysqlTranDb.TransExecuteQuery(Sql);
            MysqlTranDb.TransactionCommit();
        }
        catch (Exception exc)
        {
            MysqlTranDb.TransactionRollback();
            logger.LogToFile("MoveStudentToHistory", "throws Error " + exc.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);       
        }
        logger.LogToFile("MoveStudentToHistory", "Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    }
   
   
    public bool AutoAdmissionNoTrue()
    {
        bool AdmissionNo=false;
        int Value;
        string sql = "SELECT Value FROM tblconfiguration WHERE Id=1 AND Name='AdmisionNo'";
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
            Value = int.Parse(m_MyReader.GetValue(0).ToString());
            if (Value == 1)
            {
                AdmissionNo = true;
            }
        }
        return AdmissionNo;
    }




    public bool FeeOrExmScheduled(out string _message1, int _BatchId, int _StudId, string _StdImage, string _Std, string _ClassImage, string _Class)
    {
        bool _valid = true;
        _message1 = "";
        if ((_Std != _StdImage)||(_Class !=_ClassImage))
        {
            string Sql = "SELECT festud.Id FROM tblfeestudent festud INNER JOIN tblfeeschedule fesched ON fesched.Id=festud.SchId WHERE fesched.BatchId=" + _BatchId + " AND festud.StudId=" + _StudId;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _message1 = "class or standard cannot be changed. Fee has been scheduled";
                _valid = false;                
            }
            Sql = "SELECT studm.Id FROM tblstudentmark studm INNER JOIN tblexamschedule exmsched ON exmsched .Id=studm.ExamSchId WHERE exmsched.BatchId=" + _BatchId + " AND studm.StudId="+ _StudId;
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _message1 = "class or standard cannot be changed. Exam has been scheduled";
                _valid = false;
            }
        }
        return _valid;
    }

    public bool NameExistsInClass(string _studname, int classID)
    {
        bool name = false;
        string sql = "SELECT tblstudent.StudentName FROM tblstudentclassmap INNER JOIN tblstudent ON tblstudentclassmap.StudentId = tblstudent.Id AND tblstudentclassmap.ClassId =" + classID + " AND tblstudent.StudentName='" + _studname + "' And tblstudent.Status=1";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            name = true;
        }
        return name;
    }


    public string GetClassName(int classID)
    {
        string name = "";
        string sql = " SELECT tblclass.ClassName FROM tblclass WHERE tblclass.Id=" + classID;
         m_MyReader = m_MysqlDb.ExecuteQuery(sql);
         if (m_MyReader.HasRows)
         {
             name=m_MyReader.GetValue(0).ToString();
         }
        return name;
    }

    public bool ClassExists(int _ClassId)
    {
        bool classExists = false;
        string sql = "SELECT tblclass.ClassName from tblclass WHERE tblclass.Id=" + _ClassId;
         m_MyReader = m_MysqlDb.ExecuteQuery(sql);
         if (m_MyReader.HasRows)
         {
             classExists = true;
         }
         return classExists;
    }

    public bool PrinterTypeisDesk()
    {
        bool DeskJet = false;
        string printer = "";
        string sql = "SELECT Value FROM tblconfiguration WHERE Name='Printer'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            printer = m_MyReader.GetValue(0).ToString();
            if (printer == "Desk Jet")
            {
                DeskJet = true;
            }
            else
            {
                DeskJet = false;
            }
        }
        return DeskJet;
    }
     public string GetStudentExamReportTable(int _studid, int _examschid, int _examid, int _batchid)
    {
        StringBuilder _Table = new StringBuilder("<table class=\"style1\"> <tr><td style=\"width:10%\">&nbsp;</td> <td ><table class=\"style1\"  style=\"border: thin solid #808080;\"><tr class=\"rowhead\" ><td class=\"style9\">Subject Name</td><td class=\"style9\">Obtained Mark</td><td class=\"style9\">Max Mark</td><td class=\"style9\">Pass Mark</td><td class=\"style9\">Result</td></tr>");
        _Table.Append("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>");
        DataSet _Mydata_Subjects;
        double _obmark=0;
        double _psmark=0;
        string _temp;
        string _result;
        double _avrage=0, _GrandTotal=0;
        string sql = "select tblsubjects.subject_name, tblexammark.SubjectId, tblexammark.MarkColumn, tblclassexamsubmap.MaxMark, tblclassexamsubmap.MinMark from tblexamschedule inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblexamschedule.ClassExamId inner JOIN tblexammark on tblexammark.ExamSchId= tblexamschedule.Id AND tblclassexamsubmap.SubId= tblexammark.SubjectId inner join tblsubjects on tblsubjects.Id= tblexammark.SubjectId where tblexamschedule.Id=" + _examschid + " order by tblexammark.SubjectOrder";
        _Mydata_Subjects = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (_Mydata_Subjects != null && _Mydata_Subjects.Tables != null && _Mydata_Subjects.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr_subject in _Mydata_Subjects.Tables[0].Rows)
            {
                _temp = GetMarkfromColumn(_studid, _examschid, dr_subject[2].ToString());
                if (double.TryParse(_temp, out _obmark) && double.TryParse(dr_subject[4].ToString(), out _psmark))
                {
                    if (_obmark < _psmark)
                    {
                        _result = "Fail";
                    }
                    else
                    {
                        _result = "Pass";
                    }
                    if (_temp == "-1")
                    {
                        _temp = "A";
                    }
                    else
                    {
                        _temp = Math.Round(_obmark, 2).ToString();
                    }
                    _Table.Append("<tr><td>" + dr_subject[0].ToString() + "</td><td>" + _temp + "</td><td>" + dr_subject[3].ToString() + "</td><td>" + dr_subject[4].ToString() + "</td><td>" + _result + "</td></tr>");
                }
            }
        }


        _Table.Append("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>");
        _Table.Append("</table> <table class=\"style1\" style=\"border-style: groove; border-color: #C0C0C0\">");
        _Table.Append("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr>");
        _temp = GetMarkfromColumn(_studid, _examschid, "Avg");
        if (double.TryParse(_temp, out _avrage))
        {
            double.TryParse(GetMarkfromColumn(_studid, _examschid, "TotalMark"), out _GrandTotal);
            _Table.Append("<td class=\"style19\">Grand Total</td><td class=\"style19\">" + Math.Round(_GrandTotal,2).ToString() + "</td><td class=\"style19\">Max Total</td><td class=\"style19\">" + GetMarkfromColumn(_studid, _examschid, "TotalMax") + "</td></tr><tr><td class=\"style19\">Average</td><td class=\"style19\">" + _avrage.ToString("0.00") + "%</td><td class=\"style19\">Result</td><td class=\"style19\">" + GetMarkfromColumn(_studid, _examschid, "Result") + "</td></tr><tr><td class=\"style19\">Grade</td><td class=\"style19\">" + GetMarkfromColumn(_studid, _examschid, "Grade") + "</td><td class=\"style19\">Rank</td><td class=\"style19\">" + GetMarkfromColumn(_studid, _examschid, "Rank") + "</td></tr>");
        }
        _Table.Append("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr>");
        _Table.Append("</table></td><td style=\"width:10%\">&nbsp;</td></tr></table>");

        return _Table.ToString();
    }



    




    public string FillStudentTopData(int _StudentId, string Type) 
    {
        DataSet _Mydata_PupilData = GetPupilData(_StudentId, Type);
        StringBuilder _pupilTopData = new StringBuilder("");
        DateTime _DOB;
        int Year;
        int Today = DateTime.Now.Year;

        if (_Mydata_PupilData != null && _Mydata_PupilData.Tables != null && _Mydata_PupilData.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr_PupilData in _Mydata_PupilData.Tables[0].Rows)
            {

                _DOB = DateTime.Parse(dr_PupilData[2].ToString());

                Year = Today - _DOB.Year;
                _pupilTopData = new StringBuilder("<div class=\"container skin1 \" > <table cellpadding=\"0\" cellspacing=\"0\" class=\"containerTable\"> <tr class=\"top\"><td class=\"no\"> </td> <td class=\"n\"> Pupil Details </td> <td class=\"ne\"> </td></tr><tr class=\"middle\"> <td class=\"o\"> </td> 	<td class=\"c\"><table width=\"100%\">");
                _pupilTopData.Append(" <tr> <td style=\"background-color: #C2D5FC\">  <b>Name</b> </td> <td style=\"background-color: #C2D5FC\"> " + dr_PupilData[0].ToString() + "  </td> </tr><tr><td> &nbsp;</td> <td> &nbsp;</td>  </tr><tr> <td style=\"background-color: #C2D5FC\">  <b>Sex</b> </td> <td style=\"background-color: #C2D5FC\"> " + dr_PupilData[1].ToString() + "  </td> </tr><tr><td> &nbsp;</td> <td> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>  </tr>");
                _pupilTopData.Append("<tr> <td style=\"background-color: #C2D5FC\">  <b>Age</b> </td> <td style=\"background-color: #C2D5FC\"> " + Year + "  </td> </tr><tr><td> &nbsp;</td> <td> &nbsp;</td>  </tr><tr> <td style=\"background-color: #C2D5FC\">  <b>DOB</b> </td> <td style=\"background-color: #C2D5FC\"> " + _DOB.ToString("dd-MM-yyyy") + "  </td> </tr>");
                _pupilTopData.Append("</table></td> <td class=\"e\"> </td> </tr><tr class=\"bottom\"> <td class=\"so\"> </td> <td class=\"s\"></td> <td class=\"se\"> </td> </tr> 	</table> </div>");
            }
        }
        return _pupilTopData.ToString();
    }

    private DataSet GetPupilData(int _userId, string _Type)
    {
        string sql;
        DataSet _Mydata_PupilData = null;
        if (_Type == "student")
        {
            sql = "select tblstudent.StudentName , tblstudent.Sex, tblstudent.DOB  from tblstudent where tblstudent.Id=" + _userId + " and tblstudent.`Status`=1";
            _Mydata_PupilData = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }
        return _Mydata_PupilData;
    }

    public bool FeeScheduledForTheClass(int _ClassId, int _BatchId)
    {
        bool _valid = false;
        try
        {
            int NextBatch = _BatchId + 1;
            int _Batch = _BatchId - 2;
           // string sql = "select tblfeeschedule.Id from tblfeeschedule inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblfeeasso on tblfeeasso.Id = tblfeeaccount.AssociatedId  where tblfeeschedule.BatchId=" + _BatchId + " and tblfeeschedule.ClassId=" + _ClassId + " and tblfeeasso.Name='Class'";
            string sql = "select tblfeeschedule.Id from tblfeeschedule inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblfeeasso on tblfeeasso.Id = tblfeeaccount.AssociatedId  where tblfeeschedule.ClassId=" + _ClassId + " and tblfeeasso.Name='Class' and tblfeeschedule.BatchId in (select tblbatch.Id from tblbatch where tblbatch.`Status` = 1 or tblbatch.LastbatchId = " + _Batch + " or tblbatch.Id=" + NextBatch + ")";
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
                _valid = true;
            }
        }
        catch
        {
            _valid = false;
        }
        return _valid;
    }

  


    public bool IsQuickSchedule()
    {
        bool _valid = false;
        try
        {
            string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='QuickSchedule'";
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
                if (m_MyReader.GetValue(0).ToString() == "1")
                {
                    _valid = true;
                }
            }
        }
        catch
        {
            _valid = false;
        }
        return _valid;
    }

    private int GetCustStudFieldCount()
    {
        int count = 0; ;
        string sql = "select count(tblstudentfieldconfi.Id) from tblstudentfieldconfi";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            count =int.Parse(m_MyReader.GetValue(0).ToString());
        }
        return count;
    }

    public DataSet GetCuestomFields()
    {
        string sql;
        DataSet _Mydata_Customfields = null;

        sql = "select tblstudentfieldconfi.DbColumanName, tblstudentfieldconfi.FieldName, tblstudentfieldconfi.DataTypeId, tblstudentfieldconfi.MaxLength, tblstudentfieldconfi.Ismandatory FROM tblstudentfieldconfi";
        _Mydata_Customfields = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
      
        return _Mydata_Customfields;
    }

    //public int CreateStudent(string _studname, string _sex, DateTime _Dob, string _Fathername, string _address, int _joiningbatchid, int _joiningStandard, DateTime _Joindate, int _StandardId, int _classid, string _admisionNo, int _Bloodgroupid, string _nationality, int mothertoungid, string _mothername, string _Fathedu, string _mothedu, string _fatherOcc, double _Anualincom, string _addrspresent, string _location, string _State, int _pin, string _resedphon, string _officephon, string _email, int nofbrother, int nofsis, int firstlng, int studcatogory, int _relegionId, int CastId, int _CurrentBatchId, int _AdmissionType, string _TempStudId, int _UseBus, int _UseHostel,string StudentId)
    public int CreateStudent(string _studname, string _sex, DateTime _Dob, string _Fathername, string _address, int _joiningbatchid, int _joiningStandard, DateTime _Joindate, int _StandardId, int _classid, string _admisionNo, int _Bloodgroupid, string _nationality, int mothertoungid, string _mothername, string _Fathedu, string _mothedu, string _fatherOcc, string _motherOcc, double _Anualincom, string _addrspresent, string _location, string _State, int _pin, string _resedphon, string _officephon, string _email, int nofbrother, int nofsis, int firstlng, int studcatogory, int _relegionId, int CastId, int _CurrentBatchId, int _AdmissionType, string _TempStudId, int _UseBus, int _UseHostel, string StudentId, string aadharno)
    {


        DateTime Today = DateTime.Now;
        General _GenObj = new General(m_TransationDb);
        int _studentid = _GenObj.GetTableMaxId("tblview_student", "Id");
        while (Is_StudentAlreadyRejected(_studentid))
        {
            _studentid++;
        }
        int _status = 1;// status of student 1 means active and approved students
        try
        {
            if (NeedStudentApprovel())
            {
                _status = 2; //status 2 means seperatly approved the created students
            }
            if (_joiningStandard == -1)
            {
                _joiningStandard = _StandardId;
            }

            string sql = "INSERT INTO tblstudent(Id,StudentName,GardianName,AdmitionNo,DOB,Sex,Address,BloodGroup,Religion,Cast,DateofJoining,Status,Email,Location,Pin,State,Nationality,FatherEduQuali,MothersName,MotherEduQuali,FatherOccupation,MotherOccupation,AnnualIncome,MotherTongue,ResidencePhNo,OfficePhNo,1stLanguage,NumberofBrothers,NumberOfSysters,JoinBatch,CreationTime,Addresspresent,StudTypeId,CreatedUserName,AdmissionTypeId,TempStudentId,UseBus,UseHostel,LastClassId,JoinStandard,StudentId,AadharNumber) VALUES (" + _studentid + ",'" + _studname + "','" + _Fathername + "','" + _admisionNo + "','" + _Dob.ToString("s") + "','" + _sex + "','" + _address + "'," + _Bloodgroupid + "," + _relegionId + "," + CastId + ",'" + _Joindate.ToString("s") + "'," + _status + ",'" + _email + "','" + _location + "'," + _pin + ",'" + _State + "','" + _nationality + "','" + _Fathedu + "','" + _mothername + "','" + _mothedu + "','" + _fatherOcc + "','" + _motherOcc + "'," + _Anualincom + "," + mothertoungid + ",'" + _resedphon + "','" + _officephon + "'," + firstlng + "," + nofbrother + "," + nofsis + "," + _joiningbatchid + ",'" + Today.ToString("s") + "','" + _addrspresent + "'," + studcatogory + ",'" + LoginUserName + "'," + _AdmissionType + ",'" + _TempStudId + "'," + _UseBus + "," + _UseHostel + "," + _classid + "," + _joiningStandard + ",'" + StudentId + "','" + aadharno + "')";
            m_TransationDb.TransExecuteQuery(sql);
            if (_studentid != -1)
            {

                if (AutoAdmissionNoTrue())
                {
                    string _admitionno = GenerateAdmitionNo(_studentid);
                    sql = "UPDATE tblstudent SET AdmitionNo= '" + _admitionno + "' WHERE Id=" + _studentid;
                    m_TransationDb.TransExecuteQuery(sql);
                }


                sql = "INSERT INTO tblstudentclassmap (StudentId,ClassId,Standard,BatchId) VALUES (" + _studentid + ", " + _classid + ", '" + _StandardId + "', " + _CurrentBatchId + ")";
                m_TransationDb.TransExecuteQuery(sql);
                if (_status == 2)
                {
                    m_DbLog.LogToDb(m_UserName, "Created Student", "Student Named " + _studname + " is created and send for approval", 1, m_TransationDb);
                }
                else
                {
                    m_DbLog.LogToDb(m_UserName, "Created Student", "Student Named " + _studname + " is created and approved", 1, m_TransationDb);

                }
            }
        }
        catch
        {
            _studentid = -1;
        }

        return _studentid;
    }


    private bool Is_StudentAlreadyRejected(int _studentid)
    {
        bool _valid = false;
        string sql = "select count(tblrejectedstudent.Id) from tblrejectedstudent WHERE tblrejectedstudent.Id=" + _studentid;
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int _count = 0;
            int.TryParse(m_MyReader.GetValue(0).ToString(),out _count);
            if (_count > 0)
            {
                _valid = true;
            }

        }
        return _valid;
    }

    private int GetStudentId()
    {
        int Id = 0;
        string sql = "select max(tblview_student.Id) from tblview_student";
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
            bool valid = int.TryParse(m_MyReader.GetValue(0).ToString(),out Id);
        }
        Id = Id + 1;
        return Id;
    }

    public bool NeedStudentApprovel()
    {
        bool _Approvelneeded = false;
        int _value;
        string sql = "SELECT Value FROM tblconfiguration WHERE Id=8";
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
            _value = int.Parse(m_MyReader.GetValue(0).ToString());
            if (_value == 1)
            {
                _Approvelneeded = true;
            }
           
        }
        return _Approvelneeded;
    }

    private string GenerateAdmitionNo(int _studentid)
    {
        if (m_AdmitionNoPrefix == "-1")
        {
            m_AdmitionNoPrefix = GetAdmisionNoprefix();
        }

        return m_AdmitionNoPrefix + _studentid;
    }

    private string GetAdmisionNoprefix()
    {
        string _prefix="";
        string sql = "SELECT Value FROM tblconfiguration WHERE Id=7";
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
            _prefix = m_MyReader.GetValue(0).ToString();
           
        }
        return _prefix;
    }

    public int GetRelionId(string _Religion)
    {
        int _regid;
        string sql = "select tblreligion.Id  from tblreligion where tblreligion.Religion='" + _Religion + "' ";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _regid = int.Parse(m_MyReader.GetValue(0).ToString());
           
        }
        else
        {
            General MyGenObj = new General(m_MysqlDb);
            int MaxId = MyGenObj.GetTableMaxId("tblreligion", "Id");
            sql = "insert into tblreligion (Id,Religion) values (" + MaxId + ",'" + _Religion + "')";
            m_MysqlDb.ExecuteQuery(sql);
            _regid = MaxId;
            //sql = "select tblreligion.Id  from tblreligion where tblreligion.Religion='" + _Religion + "' ";
            //m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            //if (m_MyReader.HasRows)
            //{
            //    _regid = int.Parse(m_MyReader.GetValue(0).ToString());

            //}
            //else
            //{
            //    _regid = 0;
            //}
        }
        return _regid;
    }

    //public int GetCastId(string _Cast, int ReligionId)
    //{
    //    int _castId;
    //    _castId =int.Parse(getcastID(_Cast));
    //    if (_castId == -1)
    //    {
            
    //        _castId=GetCastAfterInsert(_Cast);
    //    }

    //    string sql = "insert into tblreligioncastmap (ReligionId,CasteId) values (" + ReligionId + "," + _castId + ")";
    //    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    //    return _castId;
    //}

    private int GetCastAfterInsert(string _Cast)
    {
        int _castId;
        General MyGenObj = new General(m_MysqlDb);
        int MaxId = MyGenObj.GetTableMaxId("tblcast", "Id");
        string sql = "INSERT into tblcast (Id,castname) values (" + MaxId + ",'" + _Cast + "')";
        m_MysqlDb.ExecuteQuery(sql);
        _castId = MaxId;
        //_castId = int.Parse (getcastID(_Cast));
        return _castId;
    }



    public bool InsertStudentDetails(string Fields, string Values)
    {
        bool valid = false;
        try
        {
            string sql = "INSERT into tblstudentdetails (" + Fields + ") values (" + Values + ")";
            if (m_TransationDb != null)
            {
                
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
               
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            }
            valid = true;
        }
        catch
        {
            valid = false;
        }
        return valid;
    }

    public bool InsertTempStudentDetails(string Fields, string Values)
    {
        bool valid = false;
        try
        {
            string sql = "INSERT into tbltempstudentdetails (" + Fields + ") values (" + Values + ")";
            if (m_TransationDb != null)
            {

                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            }
            else
            {

                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            }
            valid = true;
        }
        catch
        {
            valid = false;
        }
        return valid;
    }



    public string GetAdmossionNo(int _studentId)
    {
        string _adno="";
        string sql = "SELECT AdmitionNo FROM tblstudent where Id=" + _studentId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _adno = m_MyReader.GetValue(0).ToString();

            }
            
            return _adno;
    }


    

    public string GetCuestomField(string _dbfield, string _studentid)
    {

        string _valu = "";
        string sql = "SELECT " + _dbfield + " FROM tblstudentdetails where StudentId=" + _studentid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _valu = m_MyReader.GetValue(0).ToString();

        }

        return _valu;
    }


    public string GetCustomFieldRegStdnt(string _dbfield, string _studentid)
    {

        string _valu = "";
        string sql = "SELECT " + _dbfield + " FROM tbltempstudentdetails where StudentId='" + _studentid + "'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _valu = m_MyReader.GetValue(0).ToString();

        }

        return _valu;
    }




    private bool IsStudentIdMandatory()
    {
        string sql = "";
        OdbcDataReader Configvaluereader = null;
        bool mandatory = false;
        int value = 0;
        sql = "Select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='IsStudentIdMandatory'";
        Configvaluereader = m_MysqlDb.ExecuteQuery(sql);
        if (Configvaluereader.HasRows)
        {
            int.TryParse(Configvaluereader.GetValue(0).ToString(), out value);
            if (value == 1)
            {
                mandatory = true;
            }
        }
        return mandatory;
    }

    public string ToStripString(int _studid,string _imgurl,int _StudType)
    {

        
        DateTime _DOB;
        string _AdmissionStr_Left = "Admission No", _AdmissionStr_Right = "";
        string _studentname="", _Admissionno="", _class="", _age="", _rollno="Not Assigned",_StudentId="";
        int Year;
        DateTime Today = DateTime.Now.Date;
        string Sql = "select tblview_student.StudentName, tblview_student.AdmitionNo, tblview_student.DOB,tblclass.ClassName,tblview_student.RollNo,tblview_student.StudentId from tblview_student INNER join tblclass on tblview_student.ClassId=tblclass.Id where tblview_student.Id=" + _studid;
        // Up to Nationality Field in tblstudent
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
        if (m_MyReader.HasRows)
        {
            m_MyReader.Read();
            _studentname = m_MyReader.GetValue(0).ToString();
            _Admissionno = m_MyReader.GetValue(1).ToString();
            _DOB = DateTime.Parse(m_MyReader.GetValue(2).ToString());
            Year = GetAge(_DOB, Today);
            _age = Year.ToString(); 
            _class = m_MyReader.GetValue(3).ToString();
            if (m_MyReader.GetValue(4).ToString() != "-1")
                _rollno = m_MyReader.GetValue(4).ToString();

            _StudentId = m_MyReader.GetValue(5).ToString();
            m_MyReader.Close();
        }

        _AdmissionStr_Right = _Admissionno;
        if (IsStudentIdMandatory())
        {
            _AdmissionStr_Left = "Student Id";
            _AdmissionStr_Right = _StudentId;
        }


        //if (_StudType == 1)
        //    Sql = "select tblclass.ClassName ,tblstudentclassmap.RollNo from tblstudentclassmap inner join tblbatch on tblbatch.Id= tblstudentclassmap.BatchId AND tblbatch.`Status`=1 INNER join tblclass on tblstudentclassmap.ClassId=tblclass.Id  where tblstudentclassmap.StudentId=" + _studid;
        //else if (_StudType == 3 || _StudType == 2)
        //    Sql = "select tblclass.ClassName ,tblview_studentclassmap.RollNo from tblview_studentclassmap  INNER join tblclass on tblview_studentclassmap.ClassId=tblclass.Id  where tblview_studentclassmap.StudentId=" + _studid + " order by tblview_studentclassmap.BatchId desc";
        //    //Sql = "select tblclass.ClassName ,tblstudentclassmap_history.RollNo from tblstudentclassmap_history inner join tblbatch on tblbatch.Id= tblstudentclassmap_history.BatchId AND tblbatch.`Status`<>1 INNER join tblclass on tblstudentclassmap_history.ClassId=tblclass.Id  where tblstudentclassmap_history.StudentId=" + _studid + " order by tblstudentclassmap_history.BatchId desc";
      
        //m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
        //if (m_MyReader.HasRows)
        //{
        //    m_MyReader.Read();
        //    _class = m_MyReader.GetValue(0).ToString();
        //    if (m_MyReader.GetValue(1).ToString()!="-1")
        //    _rollno = m_MyReader.GetValue(1).ToString();
        //    m_MyReader.Close();
        //}

        StringBuilder _pupilTopData = new StringBuilder("<div id=\"winschoolStudentStrip\"><table class=\"NewStudentStrip\" width=\"100%\"><tr><td class=\"left1\"></td><td class=\"middle1\" ><table><tr><td><img alt=\"\" src=\"" + _imgurl + "\" width=\"75px\" height=\"80px\" /></td><td> </td><td><table width=\"500\"><tr><td class=\"attributeValue\">Name</td><td></td><td>:</td><td></td><td class=\"DBvalue\">" + _studentname + "</td></tr><tr><td class=\"attributeValue\">Final Class</td><td></td><td>:</td><td></td><td class=\"DBvalue\">" + _class + "</td><td class=\"attributeValue\">" + _AdmissionStr_Left + "</td><td></td><td>:</td> <td></td> <td class=\"DBvalue\"> " + _AdmissionStr_Right + "</td> <td></td> </tr><tr><td class=\"attributeValue\">Roll No</td> <td></td><td>:</td> <td></td> <td class=\"DBvalue\">" + _rollno + "</td> <td class=\"attributeValue\">Age</td> <td></td><td>:</td> <td></td><td class=\"DBvalue\">" + _age + "</td></tr> </table></td>  </tr></table>  </td>  <td class=\"right1\"> </td> </tr></table></div>");               
       
         
        return _pupilTopData.ToString();
    }

    public int GetAge(DateTime dateOfBirth, DateTime dateAsAt)
    {
        return dateAsAt.Year - dateOfBirth.Year - (GetDayOfYear(dateOfBirth) <= dateAsAt.DayOfYear ? 0 : 1);
      
    }

    private int GetDayOfYear(DateTime dateOfBirth)
    {
        int _dayOfyear = dateOfBirth.DayOfYear;
        if(dateOfBirth.Month>2)
        {
            if (IsLeapYear(dateOfBirth.Year))
            {
                _dayOfyear--;
            }
        }
        return _dayOfyear;
    }


    public static bool IsLeapYear(int year)
    {

        if (year % 4 != 0)
        {

            return false;

        }

        if (year % 100 == 0)
        {

            return (year % 400 == 0);

        }

        return true;
    }



    //public int UpdateStudent(string _StudentName, string _Sex, string _Dob, string _BloodGrp, string _FatherName, string _FatherEdu, string _MotherName, string _MotherEdu, string _FatherOccu, double _AnualIncome, string _Nationality, string _Religion, string _Cast, string _MotherTongue, string _OtherLan, string _Email, string _Address, string _Location, int _Pin, string _State, string _ResiPh, string _OfficePh, string _DteOfAdmission, int _Std, int _Class, string _1stLanguage, int CurrentBatchId, string _StayingWith, string _BirthPlace, string _Village, string _Town, string _Taluk, string _District, int _NoBro, int _NoSys, int _ClassID, int _StudID, string _FeeReceipt, string _PresentAddress, int _JoiningBatch, int _StudentType, int AdmissionType, string _AdmissionNo, int _CasteId)
    //{
    //    CLogging logger = CLogging.GetLogObject();
    //    CheckRegilionExitsInTheTbl(_Religion);

    //    string _CstId;
    //    if (_Cast == "")
    //    {
    //        _CstId = _CasteId.ToString();
    //    }
    //    else
    //    {

    //        _CstId = CreateCateIfExitGetCastId(_Cast, _Religion);

    //    }



    //    DateTime Date_Dob = General.GetDateTimeFromText(_Dob);
    //    DateTime Date__DteOfAdmission = General.GetDateTimeFromText(_DteOfAdmission);
    //    //DateTime Date_LeavingDate = DateTime.Parse(_DateOfLeaving);
    //    logger.LogToFile("UpdateStudent", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
    //    string sql = "UPDATE tblstudent SET StudentName='" + _StudentName + "',GardianName='" + _FatherName + "',DOB='" + Date_Dob.Date.ToString("s") + "',Sex='" + _Sex + "',Address='" + _Address + "',BloodGroup='" + _BloodGrp + "',Religion= '" + _Religion + "',Cast= '" + _CstId + "',DateofJoining= '" + Date__DteOfAdmission.Date.ToString("s") + "',Email= '" + _Email + "',Location= '" + _Location + "',Pin= " + _Pin + ",State= '" + _State + "',Nationality= '" + _Nationality + "',FatherEduQuali= '" + _FatherEdu + "',MothersName= '" + _MotherName + "',MotherEduQuali= '" + _MotherEdu + "',FatherOccupation= '" + _FatherOccu + "',AnnualIncome= " + _AnualIncome + ",MotherTongue= '" + _MotherTongue + "',OtherLanguages= '" + _OtherLan + "',ResidencePhNo= '" + _ResiPh + "',OfficePhNo= '" + _OfficePh + "',1stLanguage= '" + _1stLanguage + "',StayingWith= '" + _StayingWith + "',PlaceOfBirth= '" + _BirthPlace + "',Village= '" + _Village + "',Town= '" + _Town + "',Taluk= '" + _Taluk + "',District= '" + _District + "',NumberofBrothers= " + _NoBro + ",NumberOfSysters= " + _NoSys + ",FeeReceiptNo= '" + _FeeReceipt + "',Addresspresent= '" + _PresentAddress + "',JoinBatch=" + _JoiningBatch + " , AdmitionNo ='" + _AdmissionNo + "', StudTypeId= " + _StudentType + ", AdmissionTypeId = " + AdmissionType + "  WHERE Id=" + _StudID + "";
    //    logger.LogToFile("UpdateStudent", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    //    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    //    logger.LogToFile("UpdateStudent", "Checking whether class is changed or not", 'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
    //    if (_Class != _ClassID)
    //    {
    //        logger.LogToFile("UpdateStudent", " if class is changed then user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
    //        sql = "UPDATE tblstudentclassmap SET ClassId= " + _Class + ",Standard= '" + _Std + "',RollNo=-1 WHERE StudentId=" + _StudID + " AND ClassId=" + _ClassID + "";
    //        logger.LogToFile("UpdateStudent", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    //        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    //    }
    //    logger.LogToFile("UpdateStudent", "Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
    //    return _StudID;

    //}

    public void UpdateStudentGeneralDetails(string _studname, string _sex, DateTime _Dob, string _Fathername, string _address, int _joiningbatchid, DateTime _Joindate, int ReligionId, int CastId, int _studentId, string _AdmissionNo, string _JoiningStandard, string studentId)
    {
        string sql = "UPDATE tblstudent SET StudentName='" + _studname + "',Sex='" + _sex + "',DOB= '" + _Dob.Date.ToString("s") + "',GardianName= '" + _Fathername + "',Address= '" + _address + "',JoinBatch= " + _joiningbatchid + ",DateofJoining= '" + _Joindate.Date.ToString("s") + "',Religion= " + ReligionId + ",Cast=" + CastId + " , AdmitionNo = '" + _AdmissionNo + "',JoinStandard=" + _JoiningStandard + ",StudentId='"+studentId+"' WHERE Id=" + _studentId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    }

   public void UpdateStudentOtherDetails(int _Bloodgroupid, string _nationality, int mothertoungid, string _mothername, string _Fathedu, string _mothedu, string _fatherOcc, double _Anualincom, string _addrspresent, string _location, string _State, int _pin, string _resedphon, string _officephon, string _email, int nofbrother, int nofsis, int firstlng, int studcatogory, int _studentId, int _AdmissionType, int _UseBus, int _UseHostel, string _motherOcc, string _aadharno)
        // public void UpdateStudentOtherDetails(int _Bloodgroupid, string _nationality, int mothertoungid, string _mothername, string _Fathedu, string _mothedu, string _fatherOcc, string _motherOcc, double _Anualincom, string _addrspresent, string _location, string _State, int _pin, string _resedphon, string _officephon, string _email, int nofbrother, int nofsis, int firstlng, int studcatogory, int _studentId, int _AdmissionType,int _UseBus,int _UseHostel)
    {
        string sql = "UPDATE tblstudent SET BloodGroup=" + _Bloodgroupid + ",Nationality='" + _nationality + "',MotherTongue= " + mothertoungid + ",MothersName= '" + _mothername + "',FatherEduQuali= '" + _Fathedu + "',MotherEduQuali= '" + _mothedu + "',FatherOccupation= '" + _fatherOcc + "',MotherOccupation= '"+ _motherOcc +"',AnnualIncome=" + _Anualincom + ",Addresspresent='" + _addrspresent + "',Location='" + _location + "',State='" + _State + "',Pin=" + _pin + ",ResidencePhNo='" + _resedphon + "',OfficePhNo='" + _officephon + "',Email='" + _email + "',NumberofBrothers=" + nofbrother + ",NumberOfSysters=" + nofsis + ",1stLanguage=" + firstlng + ",StudTypeId=" + studcatogory + ", AdmissionTypeId =" + _AdmissionType + ",UseBus=" + _UseBus + ",UseHostel=" + _UseHostel + ",AadharNumber='" + _aadharno + "'  WHERE Id=" + _studentId;
        //string sql = "UPDATE tblstudent SET BloodGroup=" + _Bloodgroupid + ",Nationality='" + _nationality + "',MotherTongue= " + mothertoungid + ",MothersName= '" + _mothername + "',FatherEduQuali= '" + _Fathedu + "',MotherEduQuali= '" + _mothedu + "',FatherOccupation= '" + _fatherOcc + "',MotherOccupation= '" + _motherOcc + "',AnnualIncome=" + _Anualincom + ",Addresspresent='" + _addrspresent + "',Location='" + _location + "',State='" + _State + "',Pin=" + _pin + ",ResidencePhNo='" + _resedphon + "',OfficePhNo='" + _officephon + "',Email='" + _email + "',NumberofBrothers=" + nofbrother + ",NumberOfSysters=" + nofsis + ",1stLanguage=" + firstlng + ",StudTypeId=" + studcatogory + ", AdmissionTypeId =" + _AdmissionType + ",UseBus=" + _UseBus + ",UseHostel=" + _UseHostel + "  WHERE Id=" + _studentId;
   
        
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    }

    public void UpdateParentSecondaryPhNo(int _studentId, string _SecondaryNo)
    {
        string sql = "UPDATE tblsmsparentlist SET SecondaryNo='" + _SecondaryNo + "' WHERE Id=" + _studentId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    }

    public bool HaveCoustomStudentDetails(int _studentid)
    {
        bool _valid = false;
        string sql = "SELECT StudentId FROM tblstudentdetails where StudentId=" + _studentid;
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
            _valid = true;

        }

        return _valid;
    }

    public bool HaveCoustomTempStudentDetails(string _studentid)
    {
        bool _valid = false;
        string sql = "SELECT StudentId FROM tbltempstudentdetails where StudentId='" + _studentid + "'";
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
            _valid = true;

        }

        return _valid;
    }



    public int GetClassIdHistory(int _studid)
    {
        int _ClsId=-1;
        string Sql = "SELECT tblstudentclassmap_history.ClassId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.StudentId=" + _studid + " ORDER BY  tblstudentclassmap_history.BatchId DESC";
       
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
            _ClsId = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        return _ClsId;
    }


    public int GetClassId(int _studid, int _batchid)
    {
        int _ClsId=-1;
        string Sql = "select tblview_studentclassmap.ClassId from tblview_studentclassmap where tblview_studentclassmap.BatchId=" + _batchid + " and tblview_studentclassmap.StudentId=" + _studid;
       
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
            _ClsId = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        return _ClsId;
    }

    public int GetClassId(int _studid)
    {
        int _ClsId = -1;
        string Sql = "SELECT tblview_student.ClassId from tblview_student where tblview_student.Id=" + _studid;

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
            _ClsId = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        return _ClsId;
    }

    public void MoveStudentToHistory(int _studid, int _BatchID, int _Status)
    {
        string sql = "";

        sql = "INSERT INTO tblstudent_history(Id,StudentName,GardianName,AdmitionNo,DOB,Sex,Address,BloodGroup,Religion,Cast,DateofJoining,DateOfLeaving,Status,Email,Location,Pin,State,Nationality,FatherEduQuali,MothersName,MotherEduQuali,FatherOccupation,AnnualIncome,MotherTongue,ResidencePhNo,OfficePhNo,1stLanguage,NumberofBrothers,NumberOfSysters,JoinBatch,CreationTime,Addresspresent,StudTypeId,AdmissionTypeId,CreatedUserName,Comment,CanceledUser,TempStudentId,UseBus,UseHostel,ClassId,RollNo,JoinStandard,StudentId,SyncDate,MotherOccupation,AadharNumber) SELECT * FROM tblstudent WHERE tblstudent.Id=" + _studid;
        m_TransationDb.ExecuteQuery(sql);

        sql = "update tblstudent_history set tblstudent_history.`Status`=" + _Status + " , tblstudent_history.DateOfLeaving= '" + DateTime.Now.Date.ToString("s") + "' where tblstudent_history.Id=" + _studid;
        m_TransationDb.ExecuteQuery(sql);

        sql = "INSERT INTO tblstudentdetails_history SELECT * FROM tblstudentdetails WHERE tblstudentdetails.StudentId=" + _studid;
        m_TransationDb.ExecuteQuery(sql);

        sql = "INSERT INTO tblstudentclassmap_history SELECT * FROM tblstudentclassmap WHERE tblstudentclassmap.StudentId=" + _studid;
        m_TransationDb.ExecuteQuery(sql);

        int ClassId = GetClassId(_studid, _BatchID);
        sql = "UPDATE tblstudent_history SET BatchId=" + _BatchID + " WHERE Id=" + _studid;
        m_TransationDb.ExecuteQuery(sql);

        sql = "Insert into tblfileurl_history (id,FilePath,Type,UserId,FileBytes) (select Id,FilePath,Type,UserId,FileBytes from tblfileurl where tblfileurl.UserId=" + _studid + " and tblfileurl.`Type`='StudentImage')";
        m_TransationDb.TransExecuteQuery(sql);

        sql = "Delete from tblfileurl where tblfileurl.UserId=" + _studid + " and tblfileurl.`Type`='StudentImage'";
        m_TransationDb.TransExecuteQuery(sql);

        sql = "delete from tblbookissue where tblbookissue.UserId=" + _studid + " and tblbookissue.UserType=1";
        m_TransationDb.TransExecuteQuery(sql);

        sql = "delete from tblbookbooking where tblbookbooking.UserId=" + _studid + " and tblbookbooking.UserType=1";
        m_TransationDb.TransExecuteQuery(sql);

        sql = "delete from tblbookhistory where tblbookhistory.UserId=" + _studid + "  and tblbookhistory.UserTypeId=1";
        m_TransationDb.TransExecuteQuery(sql);

        sql = "insert into tblfeebillhistory (tblfeebillhistory.Id,tblfeebillhistory.StudentID,tblfeebillhistory.TotalAmount, tblfeebillhistory.`Date`, tblfeebillhistory.PaymentMode, tblfeebillhistory.PaymentModeId, tblfeebillhistory.BankName, tblfeebillhistory.BillNo,tblfeebillhistory.UserId, tblfeebillhistory.BatchId, tblfeebillhistory.ClassId, tblfeebillhistory.Canceled , tblfeebillhistory.StudentName , tblfeebillhistory.Counter ,tblfeebillhistory.RegularFee,tblfeebillhistory.TempId ,tblfeebillhistory.CreatedDateTime,tblfeebillhistory.OtherReference) select  tblfeebill.Id ,tblfeebill.StudentID, tblfeebill.TotalAmount, tblfeebill.`Date`, tblfeebill.PaymentMode, tblfeebill.PaymentModeId, tblfeebill.BankName, tblfeebill.BillNo,tblfeebill.UserId, tblfeebill.AccYear, tblfeebill.ClassID , tblfeebill.Canceled , tblfeebill.StudentName , tblfeebill.Counter,tblfeebill.RegularFee,tblfeebill.TempId,tblfeebill.CreatedDateTime,tblfeebill.OtherReference from tblfeebill where tblfeebill.StudentID =" + _studid;
        m_TransationDb.TransExecuteQuery(sql);

        sql = "insert into tbltransactionhistory (tbltransactionhistory.TransationId, tbltransactionhistory.UserId,tbltransactionhistory.BillNo, tbltransactionhistory.PaidDate, tbltransactionhistory.Amount, tbltransactionhistory.AccountTo, tbltransactionhistory.AccountFrom, tbltransactionhistory.`Type`, tbltransactionhistory.FeeName, tbltransactionhistory.FeeId,  tbltransactionhistory.PeriodId, tbltransactionhistory.ClassId, tbltransactionhistory.BatchId, tbltransactionhistory.TransType, tbltransactionhistory.Canceled,tbltransactionhistory.StudentName,tbltransactionhistory.CollectedUser , tbltransactionhistory.PeriodName ,tbltransactionhistory.TempId, tbltransactionhistory.RegularFee,tbltransactionhistory.CollectionType) select tbltransaction.TransationId,tbltransaction.UserId, tbltransaction.BillNo, tbltransaction.PaidDate, tbltransaction.Amount, tbltransaction.AccountTo,  tbltransaction.AccountFrom, tbltransaction.`Type`, tbltransaction.FeeName  , tbltransaction.FeeId , tbltransaction.PeriodId, tbltransaction.ClassId , tbltransaction.BatchId, tbltransaction.TransType , tbltransaction.Canceled , tbltransaction.StudentName , tbltransaction.CollectedUser , tbltransaction.PeriodName , tbltransaction.TempId, tbltransaction.RegularFee,tbltransaction.CollectionType from tbltransaction where tbltransaction.UserId=" + _studid;
        m_TransationDb.TransExecuteQuery(sql);

        sql = "delete from tblfeebill where StudentID=" + _studid;
        m_TransationDb.TransExecuteQuery(sql);
        sql = "delete from tblfeestudent where StudId=" + _studid;
        m_TransationDb.TransExecuteQuery(sql);

        sql = "delete from tbltransaction where UserId=" + _studid;
        m_TransationDb.TransExecuteQuery(sql);

        sql = "DELETE FROM tblstudentclassmap WHERE StudentId=" + _studid;
        m_TransationDb.ExecuteQuery(sql);

        sql = "DELETE FROM tblstudent WHERE Id=" + _studid;
        m_TransationDb.ExecuteQuery(sql);

        sql = "Delete from tblstudentdetails Where StudentId=" + _studid;
        m_TransationDb.ExecuteQuery(sql);


        // Remove RF ID 
        sql = "DELETE FROM tblexternalattencence WHERE tblexternalattencence.ExternalReffid in( SELECT tblexternalreff.Id FROM tblexternalreff  WHERE tblexternalreff.UserType='STUDENT' AND tblexternalreff.UserId="+_studid+")" ;
        m_TransationDb.ExecuteQuery(sql);

        sql = "Delete from tblexternalreff Where UserType='STUDENT' AND UserId=" + _studid;
        m_TransationDb.ExecuteQuery(sql);

        sql = "select tbltc.Id, tbltc.AdmissionNo, tbltc.TcNumber from tbltc where tbltc.StudentId=" + _studid;
        OdbcDataReader Reader_Datas= m_TransationDb.ExecuteQuery(sql);

        string sql1 = "select tblstudent_history.Id from tblstudent_history where Id=" + _studid;
        OdbcDataReader StudDetail = m_TransationDb.ExecuteQuery(sql1);

        if (Reader_Datas.HasRows && StudDetail.HasRows)
        {
            sql = "insert into tblstudentleavinghistory(tblstudentleavinghistory.AdmissionNumber, tblstudentleavinghistory.TC_Number ,tblstudentleavinghistory.ReasonForLeaving, tblstudentleavinghistory.Student_History_Id) values('" + Reader_Datas.GetValue(1).ToString() + "','" + Reader_Datas.GetValue(1).ToString() + "',''," + int.Parse(StudDetail.GetValue(0).ToString()) + ")";
            m_TransationDb.ExecuteQuery(sql);
        }

        CancelParentLogin(_studid);
    }

    private void CancelParentLogin(int _studid)
    {
        try
        {
            int _parentId = GetParentId(_studid);
            string sql = "Delete from tblparent_parentstudentmap where tblparent_parentstudentmap.StudentId=" + _studid ;
            m_TransationDb.TransExecuteQuery(sql);
            if (!HasChild(_parentId))
            {
                sql = "Delete from tblparent_parentdetails where tblparent_parentdetails.Id=" + _parentId;
                m_TransationDb.TransExecuteQuery(sql);
            }
        }
        catch
        {
        }

    }

    private bool HasChild(int _parentId)
    {
        bool _HasChild=false;
        string sql = "SELECT tblparent_parentstudentmap.ParentId FROM tblparent_parentstudentmap WHERE tblparent_parentstudentmap.ParentId=" + _parentId;
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _HasChild = true;
        }
        return _HasChild;
    }

    private int GetParentId(int _studid)
    {
        int _parentId = 0;
        string sql = "SELECT tblparent_parentstudentmap.ParentId FROM tblparent_parentstudentmap WHERE tblparent_parentstudentmap.StudentId=" + _studid;
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int.TryParse(m_MyReader.GetValue(0).ToString(), out _parentId);
        }
        return _parentId;
    }

    //public void ChangeStudentToHistory(int _studid, int _BatchID)
    //{

    //    string sql = "";
    //    sql = "INSERT INTO tblstudent_history(Id,StudentName,GardianName,AdmitionNo,DOB,Sex,Address,BloodGroup,Religion,Cast,DateofJoining,DateOfLeaving,Status,Email,Location,Pin,State,Nationality,FatherEduQuali,MothersName,MotherEduQuali,FatherOccupation,AnnualIncome,MotherTongue,ResidencePhNo,OfficePhNo,1stLanguage,NumberofBrothers,NumberOfSysters,JoinBatch,CreationTime,Addresspresent,StudTypeId,AdmissionTypeId,CreatedUserName,Comment,CanceledUser,TempStudentId,UseBus,UseHostel,ClassId,RollNo) SELECT * FROM tblstudent WHERE tblstudent.Id=" + _studid;
    //    m_TransationDb.ExecuteQuery(sql);

    //    sql = "update tblstudent_history set tblstudent_history.`Status`=0 , tblstudent_history.DateOfLeaving= '" + DateTime.Now.Date.ToString("s") + "' where tblstudent_history.Id=" + _studid;
    //    m_TransationDb.ExecuteQuery(sql);

    //    sql = "INSERT INTO tblstudentdetails_history SELECT * FROM tblstudentdetails WHERE tblstudentdetails.StudentId=" + _studid;
    //    m_TransationDb.ExecuteQuery(sql);

    //    sql = "INSERT INTO tblstudentclassmap_history SELECT * FROM tblstudentclassmap WHERE tblstudentclassmap.StudentId=" + _studid;
    //    m_TransationDb.ExecuteQuery(sql);

    //    //int ClassId = GetClassId(_studid, _BatchID);
    //    sql = "UPDATE tblstudent_history SET BatchId=" + _BatchID + " WHERE Id=" + _studid;
    //    m_TransationDb.ExecuteQuery(sql);

    //    sql = "Insert into tblfileurl_history select * from tblfileurl where tblfileurl.UserId=" + _studid + " and tblfileurl.`Type`='StudentImage'";
    //    m_TransationDb.TransExecuteQuery(sql);

    //    sql = "Delete from tblfileurl where tblfileurl.UserId=" + _studid + " and tblfileurl.`Type`='StudentImage'";
    //    m_TransationDb.TransExecuteQuery(sql);

    //     sql = "delete from tblbookissue where tblbookissue.UserId=" + _studid + " and tblbookissue.UserType=1";
    //    m_TransationDb.TransExecuteQuery(sql);

    //    sql = "delete from tblbookbooking where tblbookbooking.UserId=" + _studid + " and tblbookbooking.UserType=1";
    //    m_TransationDb.TransExecuteQuery(sql);


    //    sql = "insert into tblfeebillhistory (tblfeebillhistory.Id,tblfeebillhistory.StudentID,tblfeebillhistory.TotalAmount, tblfeebillhistory.`Date`, tblfeebillhistory.PaymentMode, tblfeebillhistory.PaymentModeId, tblfeebillhistory.BankName, tblfeebillhistory.BillNo,tblfeebillhistory.UserId, tblfeebillhistory.BatchId, tblfeebillhistory.ClassId, tblfeebillhistory.Canceled , tblfeebillhistory.StudentName , tblfeebillhistory.Counter,tblfeebillhistory.CreatedDateTime ) select  tblfeebill.Id ,tblfeebill.StudentID, tblfeebill.TotalAmount, tblfeebill.`Date`, tblfeebill.PaymentMode, tblfeebill.PaymentModeId, tblfeebill.BankName, tblfeebill.BillNo,tblfeebill.UserId, tblfeebill.AccYear, tblfeebill.ClassID , tblfeebill.Canceled , tblfeebill.StudentName , tblfeebill.Counter,tblfeebill.CreatedDateTime from tblfeebill where tblfeebill.StudentID =" + _studid;
    //    m_TransationDb.TransExecuteQuery(sql);

    //    sql = "insert into tbltransactionhistory (tbltransactionhistory.TransationId, tbltransactionhistory.UserId,tbltransactionhistory.BillNo, tbltransactionhistory.PaidDate, tbltransactionhistory.Amount, tbltransactionhistory.AccountTo, tbltransactionhistory.AccountFrom, tbltransactionhistory.`Type`, tbltransactionhistory.FeeName, tbltransactionhistory.FeeId,  tbltransactionhistory.PeriodId, tbltransactionhistory.ClassId, tbltransactionhistory.BatchId, tbltransactionhistory.TransType, tbltransactionhistory.Canceled ) select tbltransaction.TransationId,tbltransaction.UserId, tbltransaction.BillNo, tbltransaction.PaidDate, tbltransaction.Amount, tbltransaction.AccountTo,  tbltransaction.AccountFrom, tbltransaction.`Type`, tblfeeaccount.AccountName, tblfeeaccount.Id, tblfeeschedule.PeriodId, tblfeeschedule.ClassId ,tblfeeschedule.BatchId, tbltransaction.TransType , tbltransaction.Canceled from tbltransaction inner join tblfeeschedule on tbltransaction.PaymentElementId= tblfeeschedule.Id inner join tblfeeaccount on tblfeeaccount.Id= tblfeeschedule.FeeId  where tbltransaction.TransType=0 and tbltransaction.UserId=" + _studid;
    //    m_TransationDb.TransExecuteQuery(sql);

    //    sql = "delete from tblfeebill where StudentID=" + _studid;
    //    m_TransationDb.TransExecuteQuery(sql);

    //    sql = "delete from tbltransaction where UserId=" + _studid;
    //    m_TransationDb.TransExecuteQuery(sql);

    //    sql = "DELETE FROM tblstudentclassmap WHERE StudentId=" + _studid;
    //    m_TransationDb.ExecuteQuery(sql);

    //    sql = "DELETE FROM tblstudent WHERE Id=" + _studid;
    //    m_TransationDb.ExecuteQuery(sql);

    //    sql = "Delete from tblstudentdetails Where StudentId=" + _studid;
    //    m_TransationDb.ExecuteQuery(sql);

    //}
 
    public string GetHistoryCuestomField(string _dbfield, string _studentid)
    {
        string _valu = "";
        string sql = "SELECT " + _dbfield + " FROM tblstudentdetails_history where StudentId=" + _studentid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _valu = m_MyReader.GetValue(0).ToString();
        }
        return _valu;
    }

    public string ToHistoryStripString(int _studid, string _imgurl)
    {
        DateTime _DOB;
        string _studentname = "", _Admissionno = "", _age = "";
        int Year;
        int Today = DateTime.Now.Year;
        string Sql = "select tblview_student.StudentName, tblview_student.AdmitionNo, tblview_student.DOB from tblview_student where tblview_student.Id=" + _studid;
        //"select tblstudent_history.StudentName, tblstudent_history.AdmitionNo, tblstudent_history.DOB from tblstudent_history where tblstudent_history.Id=" + _studid;
        // Up to Nationality Field in tblstudent
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
        if (m_MyReader.HasRows)
        {
            m_MyReader.Read();
            _studentname = m_MyReader.GetValue(0).ToString();
            _Admissionno = m_MyReader.GetValue(1).ToString();
            _DOB = DateTime.Parse(m_MyReader.GetValue(2).ToString());
            Year = Today - _DOB.Year;
            _age = Year.ToString();
            m_MyReader.Close();
        }




        StringBuilder _pupilTopData = new StringBuilder("<div id=\"winschoolStudentStrip\"><table class=\"NewStudentStrip\" width=\"100%\"><tr><td class=\"left1\"></td><td class=\"middle1\" ><table><tr><td><img alt=\"\" src=\"" + _imgurl + "\" width=\"75px\" height=\"80px\" /></td><td> </td><td><table width=\"500\"><tr><td class=\"attributeValue\">Name</td><td></td><td>:</td><td></td><td class=\"DBvalue\">" + _studentname + "</td></tr><tr><td colspan=\"11\"><hr /></td></tr><tr><td class=\"attributeValue\">Admission No</td><td></td><td>:</td> <td></td> <td class=\"DBvalue\"> " + _Admissionno + "</td><td class=\"attributeValue\">Age</td> <td></td><td>:</td> <td></td><td class=\"DBvalue\">" + _age + "</td> <td></td> </tr> </table></td>  </tr></table>  </td>  <td class=\"right1\"> </td> </tr></table></div>");


        return _pupilTopData.ToString();
    }

    public void CreateApproved_History_Incedent(string _Title, string _Dese, string _Date, int _InceType, int _UserId, string _Type, int _AssoUser, int _HeadId, int _Point, int _BatchId, int _ClassId)
    {
        General _GenObj = new General(m_MysqlDb);
        int _Incedentid = _GenObj.GetTableMaxId("tblview_incident", "Id");
        DateTime _InceDate = _GenObj.GetDateFromText(_Date);
        //string _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        DateTime Today = DateTime.Now;
        string sql = "insert into tblincedent_history (Id,Title,Description,IncedentDate,CreatedDate,TypeId,ApproverId,CreatedUserId,UserType,Status,AssoUser,HeadId,Point,BatchId,ClassId)values (" + _Incedentid + ",'" + _Title + "','" + _Dese + "','" + _InceDate.ToString("s") + "','" + Today.ToString("s") + "'," + _InceType + "," + _UserId + "," + _UserId + ",'" + _Type.ToLower() + "','Approved'," + _AssoUser + "," + _HeadId + "," + _Point + "," + _BatchId + "," + _ClassId + " )";
        m_MysqlDb.ExecuteQuery(sql);

    }

    public void MoveStudentIncidentToHistory(int _studid)
    {
        string sql = "Insert into tblincedent_history select * from tblincedent where tblincedent.AssoUser=" + _studid + " and tblincedent.UserType='student' and tblincedent.Status='Approved'";
        m_TransationDb.TransExecuteQuery(sql);
        sql = " Delete  from tblincedent where tblincedent.AssoUser=" + _studid + " and tblincedent.UserType='student'";
        m_TransationDb.TransExecuteQuery(sql);

    }

    public bool HasPendingbooks(int _StudentId)
    {
        bool _valid = false;
        string sql = "select tblbookissue.BookNo from tblbookissue inner join tblstudent on tblstudent.Id = tblbookissue.UserId where tblstudent.Id=" + _StudentId + "";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _valid = true;
        }

        return _valid;
    }


    # region IncidentHistory

    public void StoreIncedentCalcualtion(int StudentId, int _CurrentBatchid)
    {
        int ClassId = 0;
      
        int Current_Batch_Class_AVG = 0, Total_ClassPoints = 0, Total_No_Students = 0;
        int Current_Batch_StudentPoints = 0, CurrentBatch_StudentRating = 0;
        ClassId = GetClassId(StudentId, _CurrentBatchid);
        Total_ClassPoints = GetTotal_ClassIncidentPoints(ClassId, _CurrentBatchid);
        Total_No_Students = GetTotal_StudentsInClass(ClassId, _CurrentBatchid);
        if (Total_No_Students > 0)
        {
            Current_Batch_Class_AVG = Total_ClassPoints / Total_No_Students;
            CurrentBatch_StudentRating = 0;
            Current_Batch_StudentPoints = GetTotal_StudentIncidentPoints(StudentId, ClassId, _CurrentBatchid);
            CurrentBatch_StudentRating = Current_Batch_StudentPoints - Current_Batch_Class_AVG;
            Insert_IncidentCalculations(StudentId, _CurrentBatchid, Current_Batch_StudentPoints, CurrentBatch_StudentRating, ClassId);

        }
    }

    private void Insert_IncidentCalculations(int StudentId, int _CurrentBatchid, int Current_Batch_StudentPoints, int CurrentBatch_StudentRating, int ClassId)
    {
        string sql = "insert into tblincidentcalculation(StudentId,PBP,PBR,BatchId,OldClassId) values(" + StudentId + "," + Current_Batch_StudentPoints + "," + CurrentBatch_StudentRating + "," + _CurrentBatchid + "," + ClassId + ")";
        m_TransationDb.ExecuteQuery(sql);
    }

    private int GetTotal_StudentIncidentPoints(int StudentId, int ClassId, int _CurrentBatchid)
    {
        int Totalpoints = 0;
        string sql = "SELECT SUM(tblincedent.`Point`) FROM tblincedent WHERE tblincedent.UserType='student'  AND tblincedent.Status='Approved' AND tblincedent.ClassId=" + ClassId + " AND tblincedent.BatchId=" + _CurrentBatchid + " AND tblincedent.AssoUser=" + StudentId;
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int.TryParse(m_MyReader.GetValue(0).ToString(), out Totalpoints);
        }
        return Totalpoints;
    }


    private int GetTotal_StudentsInClass(int ClassId, int _CurrentBatchid)
    {
        int count = 0;
        string sql = "SELECT Count(tblstudentclassmap.StudentId) FROM tblstudentclassmap INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudent.Status=1 AND tblstudentclassmap.ClassId=" + ClassId + " AND tblstudentclassmap.BatchId=" + _CurrentBatchid;
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int.TryParse(m_MyReader.GetValue(0).ToString(), out count);
        }
        return count;
    }

    private int GetTotal_ClassIncidentPoints(int ClassId, int _CurrentBatchid)
    {
        int Totalpoints = 0;
        string sql = "SELECT SUM(tblincedent.`Point`) FROM tblincedent WHERE tblincedent.UserType='student' AND tblincedent.Status='Approved' AND tblincedent.ClassId=" + ClassId + " AND tblincedent.BatchId=" + _CurrentBatchid;
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int.TryParse(m_MyReader.GetValue(0).ToString(), out Totalpoints);
        }
        return Totalpoints;
    }

    # endregion


    # region Consolidate Student Exam Report
    public string GetStudentConsolidateReport(int _Studid, int _ExamId, int _Batchid)
    {
        int ExamSchId = GetExamSchid(_Studid, _ExamId, _Batchid);
        return "";

    }

    public int GetExamSchid(int _Studid, int _ExamId, int _Batchid)
    {
        string sql = "select DISTINCT tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId  and tblclassexam.ExamId=" + _ExamId + "  inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId where tblstudentclassmap.StudentId= " + _Studid + " and tblstudentclassmap.BatchId=" + _Batchid + "";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

        }
        return 1;
    }

    public DataSet GetExamPeriods(int _StudentId, int i_ExamId, int _BatchId)
    {
        DataSet _Period = null;
        string sql = "select DISTINCT tblperiod.Id , tblperiod.Period from tblexamschedule inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblclassexam.ExamId = " + i_ExamId + " and tblexamschedule.BatchId =" + _BatchId + " and tblstudentclassmap.StudentId=" + _StudentId + "";
        _Period = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return _Period;
    }

    public DataSet GetSubjects(int _StudentId, int i_ExamId, int _BatchId)
    {

        DataSet _Subjects = null;

        // string sql = "select tblsubjects.subject_name, tblexammark.SubjectId, tblexammark.MarkColumn, tblclassexamsubmap.MaxMark, tblclassexamsubmap.MinMark from tblexamschedule inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblexamschedule.ClassExamId inner JOIN tblexammark on tblexammark.ExamSchId= tblexamschedule.Id AND tblclassexamsubmap.SubId= tblexammark.SubjectId inner join tblsubjects on tblsubjects.Id= tblexammark.SubjectId where tblexamschedule.Id=" + _examschid;
        string sql = "select distinct tblsubjects.subject_name, tblexammark.SubjectId, tblexammark.MarkColumn from tblexamschedule inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblexamschedule.ClassExamId inner JOIN tblexammark on tblexammark.ExamSchId= tblexamschedule.Id AND tblclassexamsubmap.SubId= tblexammark.SubjectId inner join tblsubjects on tblsubjects.Id= tblexammark.SubjectId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId and tblstudentclassmap.StudentId=" + _StudentId + "  and tblstudentclassmap.BatchId =" + _BatchId + " where tblclassexam.ExamId=" + i_ExamId + " and tblexamschedule.BatchId=" + _BatchId + " order by tblexammark.SubjectOrder";
        _Subjects = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return _Subjects;
    }

    public int GetExamSchid(int _StudentId, int _ExamId, int _BatchId, int _period)
    {
        int PeriodId = -1;
        string sql = "select DISTINCT tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId  and tblclassexam.ExamId=" + _ExamId + "  inner join tblstudentclassmap on tblstudentclassmap.ClassId = tblclassexam.ClassId where tblstudentclassmap.StudentId= " + _StudentId + " and tblstudentclassmap.BatchId=" + _BatchId + " and tblexamschedule.PeriodId=" + _period + " and tblexamschedule.BatchId="+_BatchId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            PeriodId = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        return PeriodId;
    }

    public string GetTotalMarkForaPeriod(int _Examschid, int _StudentId)
    {
        string _Total = "";
        string sql = "select tblstudentmark.TotalMark from tblstudentmark where  tblstudentmark.ExamSchId=" + _Examschid + " and tblstudentmark.StudId =" + _StudentId + "";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _Total = m_MyReader.GetValue(0).ToString();
        }
        return _Total;
    }

    public string GetMaximumMarkForaPeriod(int _Examschid, int _StudentId)
    {
        string _Max = "";
        string sql = "select tblstudentmark.TotalMax from tblstudentmark where  tblstudentmark.ExamSchId=" + _Examschid + " and tblstudentmark.StudId =" + _StudentId + "";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _Max = m_MyReader.GetValue(0).ToString();
        }
        return _Max;
    }

    public string GetAvgMarkForaPeriod(int _Examschid, int _StudentId)
    {
        double _Avg = 0.0;
        string Average = "";
        string sql = "select tblstudentmark.Avg from tblstudentmark where  tblstudentmark.ExamSchId=" + _Examschid + " and tblstudentmark.StudId =" + _StudentId + "";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            double.TryParse(m_MyReader.GetValue(0).ToString(), out _Avg);            
        }
        Average = _Avg.ToString("0.0");
        return Average;
    }

    public string GetGradeForaPeriod(int _Examschid, int _StudentId)
    {
       
        string Grade = "";
        string sql = "select tblstudentmark.Grade from tblstudentmark where  tblstudentmark.ExamSchId=" + _Examschid + " and tblstudentmark.StudId =" + _StudentId + "";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Grade = m_MyReader.GetValue(0).ToString();
        }
        return Grade;
    }

    public string GetResultForaPeriod(int _Examschid, int _StudentId)
    {
        string Grade = "";
        string sql = "select tblstudentmark.Result from tblstudentmark where  tblstudentmark.ExamSchId=" + _Examschid + " and tblstudentmark.StudId =" + _StudentId + "";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Grade = m_MyReader.GetValue(0).ToString();
        }
        return Grade;
    }

    public string GetRankForaPeriod(int _Examschid, int _StudentId)
    {
        string Grade = "";
        string sql = "select tblstudentmark.Rank from tblstudentmark where  tblstudentmark.ExamSchId=" + _Examschid + " and tblstudentmark.StudId =" + _StudentId + "";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Grade = m_MyReader.GetValue(0).ToString();
        }
        return Grade;
    }

    # endregion

    public int GetWizardStep()
    {

        int  Step = 0;
        bool valid = false;
        string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='RemoveWizardStep'";
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
            valid = int.TryParse(m_MyReader.GetValue(0).ToString(),out Step);
        }

        return Step;
    }

    # region tempStudent

    public string CreatetempStudent(string _Studentname, string _FatherName, string _Gender, int _Standard, int _Class, int _JoiningBatch, string _Address, string _ph, int _Rank, string UserName, string _Location, string _State, int _Pin, int _blood, string _Nationality, int _MotherTongue, string _FatherEducQual, string _MotherEducQual, string _FatherOuptn, double _Annualincum, string _EmailId, string MotherName, DateTime DOB, string Remark, string previousBoard, string personalInterview, string DOI, string teacherRemark, string HMRemark, string principalRemark, string Result, int admissionStatusId, int status, string _MotherOcptn, out bool _Cont, out int _StudentId)
    {
        _Cont = false;
        _StudentId = 0;
        int Id = GetMaxidfromtable();
        int MaxId = Id + 1;
        DateTime _Now = DateTime.Now;
        string _TempId = "T" + _Now.Year + "-" + MaxId;
        string sql = "insert into tbltempstdent(Name,TempId,Fathername,Gender,Standard,Class,JoiningBatch,Address,PhoneNumber,CreatedDate,Rank,CreatedUserName,AdmissionStatusId,DOB,MotherName,Location,Pin,State,Nationality,BloodGroup,MotherTongue,FatherEduQualification,MotherEduQualification,FatherOccupation,MotherOccupation,AnnualIncome,Remark,PersionalInterview,DateOfInterView,TeacherRemark,HMRemark,PrincipalRemark,Result,PreviousBoard,Email,`Status`)  values('" + _Studentname + "','" + _TempId + "','" + _FatherName + "','" + _Gender + "'," + _Standard + "," + _Class + "," + _JoiningBatch + ",'" + _Address + "','" + _ph + "','" + _Now.Date.ToString("s") + "'," + _Rank + ",'" + UserName + "'," + admissionStatusId + ",'" + DOB.ToString("s") + "','" + MotherName + "','" + _Location + "'," + _Pin + ",'" + _State + "','" + _Nationality + "'," + _blood + "," + _MotherTongue + ",'" + _FatherEducQual + "','" + _MotherEducQual + "','" + _FatherOuptn + "','" + _MotherOcptn + "'," + _Annualincum + ",'" + Remark + "','" + personalInterview + "','" + DOI + "','" + teacherRemark + "','" + HMRemark + "','" + principalRemark + "','" + Result + "','" + previousBoard + "','" + _EmailId + "'," + status + ")";

        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        sql = "select Id from tbltempstdent where Name='" + _Studentname + "' and Class=" + _Class + " and CreatedDate='" + _Now.Date.ToString("s") + "'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _StudentId = int.Parse(m_MyReader.GetValue(0).ToString());
            _Cont = true;
        }
        _Cont = true;
        return _TempId;
    }


   
    private int GetMaxidfromtable()
    {
        int MaxId = 0;
        string sql = "select max(Id) from tbltempstdent";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            bool valid = int.TryParse(m_MyReader.GetValue(0).ToString(), out MaxId);
        }
        return MaxId;
    }

    public bool UpdateTempStudentStatus(int _TempStudentId)
    {
        bool valid = false;
        string sql = "update tbltempstdent set `Status`=0 where Id="+ _TempStudentId;
        m_TransationDb.TransExecuteQuery(sql);
        valid = true;
        return valid;
    }

   

    # endregion





    public bool PrinterTypeisDesk(out string BillType)
    {
        BillType = "";
        bool DeskJet = false;
        string printer = "";
        string sql = "SELECT Value,SubValue FROM tblconfiguration WHERE Name='Printer'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            printer = m_MyReader.GetValue(0).ToString();
            if (printer == "Desk Jet")
            {
                DeskJet = true;
            }
            else
            {
                DeskJet = false;
            }
            BillType = m_MyReader.GetValue(1).ToString();
        }
        return DeskJet;
    }

    public string ScheduleRollNumber(int _classid, int _BatchId , int _Studentid)
    {
        #region old code
        //int Rollno = 0;
        //string RollNumber = "";
        //string sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblstudent.Sex from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + _BatchId + " AND tblstudentclassmap.ClassId=" + _classid + " Order by tblstudent.StudentName ASC";
        //if (m_TransationDb != null)
        //{
        //    m_MyReader = m_TransationDb.ExecuteQuery(sql);
        //}
        //else
        //{
        //    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        //}
        //if (m_MyReader.HasRows)
        //{
        //    while (m_MyReader.Read())
        //    {
        //        Rollno++;
        //        UpdateRollNumber(_classid, _BatchId, int.Parse(m_MyReader.GetValue(0).ToString()), Rollno);
        //    }
        //}
        //sql = "SELECT RollNo from tblstudentclassmap WHERE StudentId=" + _Studentid + " AND ClassId=" + _classid + " AND BatchId=" + _BatchId + "";
        //if (m_TransationDb != null)
        //{
        //    m_MyReader = m_TransationDb.ExecuteQuery(sql);
        //}
        //else
        //{
        //    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        //}
        //if (m_MyReader.HasRows)
        //{
        //    RollNumber = m_MyReader.GetValue(0).ToString();
        //}
        #endregion
        int GetLastRollNo_Class = GetLastRollNo(_BatchId, _classid);
        int RollNumber = GetLastRollNo_Class + 1;
        UpdateRollNumber(_classid, _BatchId, _Studentid, RollNumber);
        return RollNumber.ToString();
    }

    private int GetLastRollNo(int _BatchId, int _classid)
    {
        int _LastRollId = 0;
        string sql = "SELECT MAX(tblstudentclassmap.RollNo) from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + _BatchId + " AND tblstudentclassmap.ClassId=" + _classid;
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
            int.TryParse(m_MyReader.GetValue(0).ToString(),out _LastRollId);
            if (_LastRollId < 0)
            {
                _LastRollId = 0;
            }
        }
        return _LastRollId;

    }

    private void UpdateRollNumber(int _classid, int _BatchId, int _StudentId, int _Rollno)
    {
        string sql = "update tblstudentclassmap set RollNo=" + _Rollno + " where StudentId=" + _StudentId + " and ClassId=" + _classid + " and BatchId="+_BatchId+"";
        if (m_TransationDb != null)
        {
           m_TransationDb.ExecuteQuery(sql);
        }
        else
        {
           m_MysqlDb.ExecuteQuery(sql);
        }
        sql = "update tblstudent set RollNo=" + _Rollno + " where Id=" + _StudentId;
        if (m_TransationDb != null)
        {
            m_TransationDb.ExecuteQuery(sql);
        }
        else
        {
            m_MysqlDb.ExecuteQuery(sql);
        }
    }

    public int GetStandard(string _StudentId)
    {
        int Standard = 0;
        string sql = "select Standard from tblstudentclassmap where StudentId="+_StudentId;
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
            Standard = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        return Standard;
    }

    public bool AnyFeePaid(string _StudentId, int _BatchId, string _ClassId)
    {
        bool _valid = false;
        string sql = "select tbltransaction.UserId from tblstudentclassmap inner join tbltransaction on tbltransaction.UserId = tblstudentclassmap.StudentId where tblstudentclassmap.ClassId=" + _ClassId + " and tblstudentclassmap.BatchId=" + _BatchId + " and tbltransaction.Canceled=0 and tblstudentclassmap.StudentId=" + _StudentId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _valid = true; 
            }
        return _valid;
    }

    public bool AttendanceMarked(string _StudentId, int _BatchId, string _ClassId)
    {
        bool _valid = false;
        string sql = "";
        if (IsNewAttendance())
        {
            int _count = 0;
            int standardId = GetStandard(_StudentId);
            AttendenceTableManager MyAttendance = new AttendenceTableManager(m_MysqlDb);
            if (MyAttendance.AttendanceTables_Exits(standardId.ToString(), _BatchId))
            {
                sql = "SELECT Count(Id) FROM tblattdcls_std"+standardId+"yr"+_BatchId+" WHERE ClassId=" + _ClassId;
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);

                if (m_MyReader.HasRows) //means attendance is marked on the date
                {
                    int.TryParse(m_MyReader.GetValue(0).ToString(), out _count);
                    if (_count > 0)
                    {
                        _valid = true;
                    }
                }
            }
        }
        else
        {
             sql = "select Id from tbldate where classId=" + _ClassId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _valid = true;
            }
        }
        return _valid;
    }

    public bool IsNewAttendance()
    {
        string Page = "MarkClassAttendanceMaster.aspx";
        bool NewAttd = false;
        string Sql = "SELECT Link FROM tblaction WHERE Id=80";
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

    public bool UpDateStudentDivision(string _StudentId,string _ClassId, int _BatchId)
    {
        bool _valid = false;
        try
        {
            string sql = "update tblstudentclassmap set ClassId =" + _ClassId + " where StudentId=" + _StudentId + " and BatchId=" + _BatchId + "";
            m_MysqlDb.ExecuteQuery(sql);
            sql = "update tblstudent set LastClassId =" + _ClassId + " where Id=" + _StudentId;
            m_MysqlDb.ExecuteQuery(sql);
            _valid = true;
        }
        catch
        {
            _valid = false;
        }
        return _valid;
    }


    public void MoveStudentWiseFeescheduleToNewClass(string _NewClass, string _OldClass, string _StudentId, int _BatchId)
    {
        string NewClassFeeSchId="0";
        DataSet FeeScheduleId = GetFeeScheduleId(_OldClass, _StudentId, _BatchId);
        if (FeeScheduleId != null && FeeScheduleId.Tables != null && FeeScheduleId.Tables.Count>0)
        {
            foreach(DataRow Dr_Fee in FeeScheduleId.Tables[0].Rows)
            {
                if (FeeScheduledForNesClass(_NewClass, Dr_Fee[1].ToString(), Dr_Fee[2].ToString(), Dr_Fee[3].ToString(), out NewClassFeeSchId))
                {
                    if (NewClassFeeSchId != "0")
                    {
                        UpdateStudFeeScheduleEntry(NewClassFeeSchId, Dr_Fee[0].ToString(), _StudentId);
                    }
                }
                else//Not scheduled for new class
                {
                    NewClassFeeSchId = InsertFeeScheduleData(Dr_Fee[1].ToString(), Dr_Fee[2].ToString(), Dr_Fee[3].ToString(), Dr_Fee[4].ToString(), Dr_Fee[4].ToString(), _NewClass);
                    UpdateStudFeeScheduleEntry(NewClassFeeSchId, Dr_Fee[0].ToString(), _StudentId);
                }
            }
        }
    }

    private string InsertFeeScheduleData(string _FeeId, string _BatchId, string _Periodid, string _DueDate, string _LastDate, string _NewClass)
    {
        string _FeeschdId = "0";
        DateTime DueDate = DateTime.Parse(_DueDate);
        DateTime LastDate = DateTime.Parse(_LastDate);
        string sql = "insert into tblfeeschedule(FeeId,BatchId,Duedate,LastDate,ClassId,PeriodId) values(" + _FeeId + " , " + _BatchId + " ,'" + DueDate.Date.ToString("s") + "' , '" + LastDate.Date.ToString("s") + "' , "+_NewClass+" , "+_Periodid+")";
        m_MysqlDb.ExecuteQuery(sql);
        sql = "select Id from tblfeeschedule where FeeId=" + _FeeId + " and BatchId=" + _BatchId + " and ClassId=" + _NewClass + " and PeriodId =" + _Periodid + " ";
         m_MyReader = m_MysqlDb.ExecuteQuery(sql);
         if (m_MyReader.HasRows)
         {
             _FeeschdId = m_MyReader.GetValue(0).ToString();
         }
         return _FeeschdId;
    }

    private void UpdateStudFeeScheduleEntry(string _NewClassFeeSchId, string _OldSchId, string _StudentId)
    {
        string sql = "update tblfeestudent set SchId=" + _NewClassFeeSchId + " where SchId=" + _OldSchId + " and StudId="+_StudentId;
        m_MysqlDb.ExecuteQuery(sql);
    }

    private bool FeeScheduledForNesClass(string _NewClass, string _FeeId, string _BatchId, string _Periodid, out string _FeeSchdid)
    {
        bool _valid = false;
        _FeeSchdid = "0";
        string sql = "select Id from tblfeeschedule where FeeId=" + _FeeId + " and BatchId=" + _BatchId + " and PeriodId=" + _Periodid + " and ClassId="+_NewClass;
         m_MyReader = m_MysqlDb.ExecuteQuery(sql);
         if (m_MyReader.HasRows)
         {
             _valid = true;
             _FeeSchdid = m_MyReader.GetValue(0).ToString();
         }
         return _valid;
    }

    private DataSet GetFeeScheduleId(string _OldClass, string _StudentId, int _BatchId)
    {
        string sql = "select tblfeestudent.SchId ,tblfeeschedule.FeeId,tblfeeschedule.BatchId, tblfeeschedule.PeriodId,tblfeeschedule.Duedate , tblfeeschedule.LastDate from tblfeeschedule inner join tblfeestudent on tblfeeschedule.Id = tblfeestudent.SchId inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId and tblfeeaccount.AssociatedId=2 where tblfeestudent.StudId=" + _StudentId + " and tblfeeschedule.ClassId=" + _OldClass + " and tblfeeschedule.BatchId=" + _BatchId + "";
        m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return m_MyDataSet;
    }

    public void DeleteOldClassWiseFees(string _NewClass, string _OldClass, int _BatchId, string _StudentId)
    {
        DeleteClassWiseFeeSchedules(_OldClass, _BatchId, _StudentId);        
    }

    private void DeleteClassWiseFeeSchedules(string _OldClass, int _BatchId , string _StudentId)
    {
        string sql = "delete from tblfeestudent where tblfeestudent.SchId in (select tblfeeschedule.Id from tblfeeschedule inner join tblfeeaccount on tblfeeaccount.Id=tblfeeschedule.FeeId where tblfeeaccount.AssociatedId=1 and tblfeeschedule.ClassId="+_OldClass+") and StudId=" + _StudentId;
        m_MysqlDb.ExecuteQuery(sql);
    }

    public bool CheckForRuleApplicableToClassAndFee1(int _ClassId, int _FeeId)
    {
        bool _flag = false;

        CLogging logger = CLogging.GetLogObject();
        string sql = "select tblruleclassmap.RuleId from tblruleclassmap where tblruleclassmap.classId=" + _ClassId + " and tblruleclassmap.feeTypeId=" + _FeeId;
        if (m_TransationDb == null)
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        else
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
        logger.LogToFile("CheckForRuleApplicableToClassAndFee1", "select tblruleclassmap.RuleId from tblruleclassmap  ", 'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
        if (m_MyReader.HasRows)
        {
            _flag = true;
        }
        return _flag;
    }

    public bool CheckRuleIsApplicabletoThisStudent(int _feeId, int _ClassId, double _BaseAmount, int _userId, int _batchId, int _SteduleId)
    {
        bool _flag = false;
        DataSet _ruleset;
        double _ReturnAmt = _BaseAmount;
        string _tblname, _colname, _ruleassigmode, _ruleFieldvalue, _fieldtype;
        int _rulesAmounttype;
        float _rulesAmount;
        double _AmountAfterRuleCal;
        try
        {
            //CreateTansationDb();
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
            //if (_SteduleId != -1)
            //{
            //    EndSucessTansationDb();
            //}
            //else
            //{
            //    EndFailTansationDb();
            //}
        }
        catch (Exception)
        {
            //EndFailTansationDb();
            _flag = false;
        }


        return _flag;

    }

    public void ScheduleStudFee(int _StudId, int _FeeSchId, double _Amount, string _Status)
    {
        string sql = "INSERT INTO tblfeestudent(SchId,StudId,Amount,BalanceAmount,Status) VALUES(" + _FeeSchId + "," + _StudId + "," + _Amount + "," + _Amount + ",'" + _Status + "')";
        if (m_TransationDb == null)
            m_MysqlDb.ExecuteQuery(sql);
        else
           m_TransationDb.ExecuteQuery(sql);
    }

    private void AddStudDataToTblFeeStud(int schedId, int _studId, double _studamt)
    {
        string sql;
        CLogging logger = CLogging.GetLogObject();
        if (_studamt > 0)
        {
            sql = "INSERT INTO tblfeestudent(SchId,StudId,Amount,BalanceAmount,Status) VALUES(" + schedId + "," + _studId + "," + _studamt + "," + _studamt + ",'Scheduled')";

        }
        else
        {
            sql = "INSERT INTO tblfeestudent(SchId,StudId,Amount,BalanceAmount,Status) VALUES(" + schedId + "," + _studId + "," + _studamt + "," + _studamt + ",'fee Exemtion')";
        }
        m_TransationDb.TransExecuteQuery(sql);
        logger.LogToFile("AddStudDataToTblFeeStud", "Insering into feestudent tbl ", 'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);

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
                    else
                    {
                        if (_intcolname == (int.Parse(_ruleFieldvalue.ToString())))
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

    public bool ExamConducted(string _Student, int _Batch, string _Class)
    {
        bool _valid = false;
        string sql = "select tblstudentmark.Id from tblstudentmark inner join tblexamschedule on tblexamschedule.Id = tblstudentmark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId where tblclassexam.ClassId="+_Class+" and tblexamschedule.BatchId = "+_Batch+" and tblstudentmark.StudId=" + _Student;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _valid = true;
        }
        return _valid;
    }

    public void RemoveStudentFromCurrentDBandComment(int _studid, string _Reason, int UserId, string AdmissionN0)
    {
        string _PreFix = "CE", NewAdmissionNo = "";
        GenNewAdmissionNo(_PreFix, _studid, AdmissionN0, out NewAdmissionNo, 0,_Reason, UserId); 
    }

    private void GenNewAdmissionNo(string _PreFix, int _studId, string AdmissionN0, out string NewAdmissionNo, int i,string _Reason, int UserId)
    {
        i++;
        try
        {
            NewAdmissionNo = _PreFix + i + "-" + AdmissionN0;
            Update(NewAdmissionNo, _studId,  _Reason, UserId);
        }
        catch
        {
            GenNewAdmissionNo(_PreFix, _studId, AdmissionN0, out NewAdmissionNo, i, _Reason, UserId);
        }
    }

    private void Update(string NewAdmissionNo, int _studid, string _Reason, int UserId)
    {
        string sql = "update tblstudent set `Status` =3 , DateOfLeaving= '" + DateTime.Now.Date.ToString("s") + "',Comment='" + _Reason + "',AdmitionNo='"+NewAdmissionNo+"',CanceledUser=" + UserId + " Where Id=" + _studid;
        m_TransationDb.ExecuteQuery(sql);
    }

    public string GetStudentSatus(string _StudentId)
    {
        string Status = "1";
        string sql = "select tblstudent.Status from tblstudent where tblstudent.Id="+_StudentId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Status = m_MyReader.GetValue(0).ToString();
        }
        return Status;
    }

    public int GetHistoryClassId(int _StudentId, int _BatchId)
    {
        int _ClsId = -1;
        string Sql = "select tblstudentclassmap_history.ClassId from tblstudentclassmap_history where tblstudentclassmap_history.BatchId=" + _BatchId + " and tblstudentclassmap_history.StudentId=" + _StudentId;

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
            _ClsId = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        return _ClsId;
    }
   
    public OdbcDataReader GetPrevClassBatchs(int studid, int batchId, int classid)
    {
        string condition = "";
        if (batchId != 0)
        {
            condition = " and tblstudentclassmap_history.Batchid = " + batchId + " and tblstudentclassmap_history.ClassId = " + classid + "";
        }

        string sql = "select tblstudentclassmap_history.ClassId, CONCAT( (select tblclass.ClassName from tblclass where tblclass.Id=tblstudentclassmap_history.ClassId), '@', (select tblbatch.BatchName from tblbatch where tblbatch.Id= tblstudentclassmap_history.BatchId)) as ClassName from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + studid + condition +" order by tblstudentclassmap_history.ClassId";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        return m_MyReader;
    }

    public int getBatchIdFromName(string batch)
    {
        string sql = "select id from tblbatch where batchname= '" + batch + "'";
        int batchId = 0;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out batchId);
            }
        }
        return batchId;
    }

    public int getClassIdFromName(string classname)
    {
        int classId = 0;
        string sql = "select id from tblclass where classname = '" + classname + "'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out classId);
            }
        }
        return classId;
    }
    
    public OdbcDataReader getPrevClassExamNames(int classid, int batchId)
    {
        string sql = "select tblexamschedule_history.Id, tblexamschedule_history.ExamName, tblperiod.Period from tblexamschedule_history inner join tblperiod on tblexamschedule_history.PeriodId = tblperiod.Id where tblexamschedule_history.BatchId = "+batchId+" and tblexamschedule_history.ClassId = "+classid+"";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);

        return m_MyReader;
    }

    public OdbcDataReader getPrevClassExamSubjNames(int exmschId)
    {
        string sql = "select markcolumn, subjectname from tblexammark_history where examschID =" + exmschId + " order by SubjectOrder";
        return (m_MysqlDb.ExecuteQuery(sql));
    }

    public OdbcDataReader getPrevClassExamSubjMarks(string markcolumn, int exmschId, int studId)
    {
        string sql = "select " + markcolumn + " from tblstudentmark_history where examschID =" + exmschId + " and studId = " + studId + "";
        return (m_MysqlDb.ExecuteQuery(sql));
    }
    
    public OdbcDataReader getPrevClassExamResult(int studId, int exmschId)
    {
        string sql ="select round(TotalMark,2), round(TotalMax,2), round(Avg,2), Grade, Result,  Rank from tblstudentmark_history where StudId = "+studId+" and ExamSchId = "+exmschId+"";

        return (m_MysqlDb.ExecuteQuery(sql));
    }


    # region scheduleClasswis fees

    public bool ScheduleClassWisePendingFees(int _StudentId, int _CurrentBatchId, int _ClassId,bool _CheckStudentApproval)
    {
        bool _Valid = true;
        try
        {
            if (!_CheckStudentApproval || !NeedStudentApprovel())
            {

                OdbcDataReader MyTempReader = null;
                int NextBatch = _CurrentBatchId + 1;
                string sql = "select tblfeeschedule.Id,tblfeeschedule.FeeId, tblfeeaccount.AccountName, tblfeeschedule.Amount, date_format(tblfeeschedule.Duedate, '%d-%m-%Y') AS 'Duedate' , date_format(tblfeeschedule.LastDate, '%d-%m-%Y') AS 'LastDate' from tblfeeschedule inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblfeeasso on tblfeeasso.Id = tblfeeaccount.AssociatedId  where (tblfeeschedule.BatchId=" + _CurrentBatchId + " or tblfeeschedule.BatchId=" + NextBatch + " ) and tblfeeschedule.ClassId=" + _ClassId + " and tblfeeasso.Name='Class' and tblfeeschedule.Id not in(select distinct tblfeestudent.SchId from tblfeestudent where tblfeestudent.StudId=" + _StudentId + ")";
                MyTempReader = m_TransationDb.ExecuteQuery(sql);
                if (MyTempReader.HasRows)
                {
                    while (MyTempReader.Read())
                    {
                        if (CheckForRuleApplicableToClassAndFee1(_ClassId, int.Parse(MyTempReader.GetValue(1).ToString())))
                        {
                            if (CheckRuleIsApplicabletoThisStudent(int.Parse(MyTempReader.GetValue(1).ToString()), _ClassId, double.Parse(MyTempReader.GetValue(3).ToString()), _StudentId, _CurrentBatchId, int.Parse(MyTempReader.GetValue(0).ToString())))
                            {

                            }
                        }
                        else
                        {
                            ScheduleStudFee(_StudentId, int.Parse(MyTempReader.GetValue(0).ToString()), double.Parse(MyTempReader.GetValue(3).ToString()), "Scheduled");
                        }
                    }
                }
            }
        }
        catch
        {
            _Valid = false;
        }
        return _Valid;
    }

    # endregion


    public int GetClassNroll(int StudentId, int _BatchId, out int RollNumber)
    {

        int _ClsId = -1;
        RollNumber = -1;
        string Sql = "select tblstudentclassmap.ClassId , tblstudentclassmap.BatchId from tblstudentclassmap where tblstudentclassmap.BatchId=" + _BatchId + " and tblstudentclassmap.StudentId=" + StudentId;

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
            _ClsId = int.Parse(m_MyReader.GetValue(0).ToString());
            RollNumber = int.Parse(m_MyReader.GetValue(1).ToString());
        }
        return _ClsId;
    }

    public int getStudType(int studid, int currentbatch)
    {
        int StudType = 1;
        string sql = "select tblstudent.Id from  tblstudentclassmap inner join tblstudent on tblstudent.Id = tblstudentclassmap.StudentId  where tblstudentclassmap.StudentId = " + studid + " and tblstudentclassmap.BatchId = "+currentbatch+" and tblstudent.Status = 1";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            StudType = 1;
        }
        else
        {
            m_MyReader = null;
            sql = "select Id from  tblstudent where id= "+studid+" and Status = 1";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                StudType = 3;
            }
            else
            {
                m_MyReader = null;
                sql = "select tblstudent_history.Id  from tblstudent_history where tblstudent_history.Id = "+studid+" ";//and tblstudent_history.Status = 1";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    StudType = 2;
                }
            }
        }
        return StudType;
    }

    public void UpdateStudentDetailsInTempStudentAndFeeTables(int _StudentId, string _studname, string _address, string _Fathername)
    {
        string sql="";
        sql = "update tbltempstdent inner join tblstudent on  tbltempstdent.TempId= tblstudent.TempStudentId set tbltempstdent.Name='" + _studname + "', tbltempstdent.Address='"+_address+"', tbltempstdent.Fathername='"+_Fathername+"' where  tblstudent.Id=" + _StudentId;
        m_MysqlDb.ExecuteQuery(sql);

        sql = "update tblfeebill set tblfeebill.StudentName='" + _studname + "' where tblfeebill.StudentID=" + _StudentId;
        m_MysqlDb.ExecuteQuery(sql);

        sql = "update tbltransaction set tbltransaction.StudentName='" + _studname + "' where tbltransaction.UserId<>0 AND tbltransaction.UserId=" + _StudentId;
        m_MysqlDb.ExecuteQuery(sql);
        sql = "update tblstudentfeeadvance set tblstudentfeeadvance.StudentName='" + _studname + "' where tblstudentfeeadvance.StudentId<>0 AND tblstudentfeeadvance.StudentId=" + _StudentId;
        m_MysqlDb.ExecuteQuery(sql);
        sql = "update tblfeeadvancetransaction set tblfeeadvancetransaction.StudentName='" + _studname + "' where tblfeeadvancetransaction.StudentId<>0 AND tblfeeadvancetransaction.StudentId=" + _StudentId;
        m_MysqlDb.ExecuteQuery(sql);  
    }



    //public int IncertNewCaste(int _ReligionId, string _CasteName)
    //{
    //    if (_CasteName == "")
    //        _CasteName = "UNKNOWN";
    //    General _GenObj = new General(m_MysqlDb);
    //    int _Id = _GenObj.GetTableMaxId("tblcast", "Id");
    //    string sql = "insert into tblcast (Id, castname) values (" + _Id + ",'" + _CasteName + "')";
    //    m_MysqlDb.ExecuteQuery(sql);
    //    sql = "insert into tblreligioncastmap (ReligionId , CasteId) values (" + _ReligionId + " ," + _Id + ")";
    //    m_MysqlDb.ExecuteQuery(sql);
    //    return _Id;
    //}

    #region CURRICULUM DETAILS

    public OdbcDataReader GetAdmissionDtls(int studId)
    {
        string sql = " select DATE_FORMAT(tblview_student.DateofJoining,'%d/%m/%Y'), tblbatch.BatchName,  tblstandard.Name, tblclass.ClassName, tblview_student.CreatedUserName, DATE_FORMAT(tblview_student.CreationTime,'%d/%m/%Y'),  tblview_student.TempStudentId from tblview_student inner join tblbatch on  tblbatch.Id = tblview_student.JoinBatch  inner join tblview_studentclassmap on tblview_studentclassmap.StudentId = tblview_student.Id  inner join tblclass on tblclass.Id =  tblview_studentclassmap.ClassId  inner join tblstandard on tblstandard.Id = tblview_studentclassmap.Standard    where tblview_student.Id = "+studId+" and tblview_studentclassmap.ClassId =    (select min(tblview_studentclassmap.ClassId) from tblview_studentclassmap where tblview_studentclassmap.StudentId = " + studId + ") ";
        return m_MysqlDb.ExecuteQuery(sql);
    }
    #endregion

    public DataSet GetCarrierData(int studId)
    {
        string sql = " select tblview_studentclassmap.ClassId, tblbatch.BatchName as Batch,   tblstandard.Name as Standard,   tblclass.ClassName as Class from tblview_student        inner join tblview_studentclassmap on tblview_studentclassmap.StudentId = tblview_student.Id       inner join tblbatch on  tblbatch.Id =  tblview_studentclassmap.BatchId   inner join tblclass on tblclass.Id =  tblview_studentclassmap.ClassId     inner join tblstandard on tblstandard.Id = tblview_studentclassmap.Standard           where tblview_student.Id = " + studId + " order by  tblview_studentclassmap.BatchId";
        return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
    }

    public OdbcDataReader GetTCDetails(int studId)
    {
        string sql = "   select tbltc.TcNumber, DATE_FORMAT(tbltc.DateOfIssueOfTC,'%d/%m/%Y'), tblbatch.BatchName, tblclass.ClassName   from tbltc inner join tblbatch on tbltc.LastBatchId = tblbatch.Id       inner join tblview_studentclassmap on tblview_studentclassmap.StudentId = tbltc.StudentId inner join tblclass on tblclass.Id =  tblview_studentclassmap.ClassId  inner join tblstandard on tblstandard.Id = tblview_studentclassmap.Standard    where tbltc.Status = 1 and tbltc.StudentId = " + studId + " and tblview_studentclassmap.ClassId =    (select max(tblview_studentclassmap.ClassId) from tblview_studentclassmap where tblview_studentclassmap.StudentId = " + studId + ")";
        return m_MysqlDb.ExecuteQuery(sql);
    }

    public string getStatus(int studId)
    {
        string status = "Current";
        string sql="select tblstudent.id from tblstudent inner join tblview_student on tblview_student.Id = tblstudent.Id where tblview_student.id = "+studId+"";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                status = "Ongoing";
            }
        }
        else
        {
            sql = "select tblstudent_history.id from tblstudent_history inner join tblview_student on tblview_student.Id = tblstudent_history.Id where tblview_student.id = " + studId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);

            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    sql = "select tbltc.StudentId from tbltc inner join tblview_student on tblview_student.Id = tbltc.StudentId where tblview_student.id = " + studId + "";
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                    if (m_MyReader.HasRows)
                    {
                        while (m_MyReader.Read())
                        {
                            status = "Issued TC";
                        }
                    }
                    else
                    {
                        status = "Cancelled";
                    }

                }
            }
        }
        

       return status;
    }

    public OdbcDataReader GetTcDetails(int studId)
    {
        string sql = "select StudentName,NameOFSchool,AdmissionNo, CumulativeRecNo, Sex, NameOfFather,Nationality, Religion, Cast,     CasteType, Dob,CurrStandard,  LangStudied,MediumOfIns,  Syllabus,  DateOfAdmission, WhetherQualiForPromo, FeesDue,  FeeConcessions, SCholarship, MedicalyExmnd, LastAttendanceDate,  AppForTcRecvedDate, DateOfIssueOfTC,  NoOfSchoolDays, SchoolDaysAttended,  CharacterNConduct, StudentId, LastBatchId from tbltc where StudentId = "+studId+"";
        return m_MysqlDb.ExecuteQuery(sql);
    }
   
    public int GetLastStandard(string _StudentId)
    {
        int Standard = 0;
        string sql = "select tblclass.Standard from tblclass INNER JOIN tblstudent on tblstudent.LastClassId = tblclass.Id where tblstudent.Id = "+_StudentId+"";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Standard = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        return Standard;
    }

    public void UpdateJoiningFeeId(string _TempStudentId, int _userId)
    {
        string sql = "update tblfeebill set StudentID=" + _userId + " where TempId='" + _TempStudentId + "'";
        m_TransationDb.ExecuteQuery(sql);
        sql = "update tbltransaction set UserId=" + _userId + " where TempId='" + _TempStudentId + "'";
        m_TransationDb.ExecuteQuery(sql);

        sql = "update tblfeebillclearence set StudentID=" + _userId + " where TempId='" + _TempStudentId + "'";
        m_TransationDb.ExecuteQuery(sql);
        sql = "update tbltransactionclearence set UserId=" + _userId + " where TempId='" + _TempStudentId + "'";
        m_TransationDb.ExecuteQuery(sql);


        sql = "update tblstudentfeeadvance set tblstudentfeeadvance.StudentId=" + _userId + " where tblstudentfeeadvance.TempId='" + _TempStudentId + "'";
        m_TransationDb.ExecuteQuery(sql);
        sql = "update tblfeeadvancetransaction set tblfeeadvancetransaction.StudentId=" + _userId + " where tblfeeadvancetransaction.TempId='" + _TempStudentId + "'";
        m_TransationDb.ExecuteQuery(sql);

    }

    public void InsertParentEmailIdIntoEmailParenstsList(int _StudentId, string EmailId)
    {
        string sql = "delete from tbl_emailparentlist where Id=" + _StudentId;
        m_TransationDb.ExecuteQuery(sql);

        sql = "insert into tbl_emailparentlist(Id,EmailId,Enabled) values (" + _StudentId + ",'" + EmailId + "'," + "1)";
        m_TransationDb.ExecuteQuery(sql);
    }

    public void InsertParentMobileNumberIntoSMSParenstsList(int _StudentId, string _MobileNumber, string _SecodaryNumber)
    {
        string sql = "delete from tblsmsparentlist where Id=" + _StudentId;
        if (m_TransationDb != null)
        {
            m_TransationDb.ExecuteQuery(sql);
        }
        else
        {
            m_MysqlDb.ExecuteQuery(sql);
        }

        sql = "insert into tblsmsparentlist(Id,PhoneNo,SecondaryNo,Enabled) values (" + _StudentId + ",'" + _MobileNumber + "','" +_SecodaryNumber+"',1)";
        if (m_TransationDb != null)
        {
            m_TransationDb.ExecuteQuery(sql);
        }
        else
        {
            m_MysqlDb.ExecuteQuery(sql);
        }
    }

    public int GetTempStudentIdFromTempIDString(string _TempId)
    {
        int _StudentTempID = 0;
        string sql = "select tbltempstdent.Id from tbltempstdent where tbltempstdent.TempId='" + _TempId + "'";
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
            int.TryParse(m_MyReader.GetValue(0).ToString(), out _StudentTempID);
        }
        return _StudentTempID;
    }




    public int UpdateHistory(string StudentName, string GardianName, string AdmitionNo, DateTime DOB, string Sex, string Address, int BloodGroup, int Religion, int Cast, DateTime DateofJoining, DateTime DateOfLeaving, int Status, string Email, string Location, int Pin, string State, string Nationality, string FatherEduQuali, string MothersName, string MotherEduQuali, string FatherOccupation, int AnnualIncome, int MotherTongue, string ResidencePhNo, string OfficePhNo, string FirstLanguage, int NumberofBrothers, int NumberOfSysters, int JoinBatch, string CreationTime, string Addresspresent, int StudTypeId, int AdmissionTypeId, string CreatedUserName, int ClassId, int BatchId, string Comment, int CanceledUser, string TempStudentId, int UseBus, int UseHostel, int RollNo, string ReasonForLeaving, string TCNumber, out string message)
    {
        int _studentid = -1,standardId=0;

        message="";
        try
        {
            General _GenObj = new General(m_TransationDb);
            string sql_Standard = "select tblclass.Standard   from tblclass where tblclass.id=" +ClassId;
            m_MyReader = m_TransationDb.ExecuteQuery(sql_Standard);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out standardId);
            }

            _studentid = _GenObj.GetTableMaxId("tblview_student", "Id");
            string sql = "insert into tblstudent_history(Id,StudentName,GardianName,AdmitionNo,DOB,Sex,Address,BloodGroup,Religion,Cast,DateofJoining,DateOfLeaving,Status,Email,Location,Pin,State,Nationality,FatherEduQuali,MothersName,MotherEduQuali,FatherOccupation,AnnualIncome,MotherTongue,ResidencePhNo,OfficePhNo,1stLanguage,NumberofBrothers,NumberOfSysters,JoinBatch,CreationTime,Addresspresent,StudTypeId,AdmissionTypeId,CreatedUserName,ClassId,BatchId,Comment,CanceledUser,TempStudentId,UseBus,UseHostel,RollNo)VALUES (" + _studentid + ",'" + StudentName + "','" + GardianName + "','" + AdmitionNo + "','" + DOB.Date.ToString("s") + "','" + Sex + "','" + Address + "'," + BloodGroup + "," + Religion + "," + Cast + ",'" + DateofJoining.Date.ToString("s") + "','" + DateOfLeaving.Date.ToString("s") + "'," + Status + ",'" + Email + "','" + Location + "'," + Pin + ",'" + State + "','" + Nationality + "','" + FatherEduQuali + "','" + MothersName + "','" + MotherEduQuali + "','" + FatherOccupation + "'," + AnnualIncome + "," + MotherTongue + ",'" + ResidencePhNo + "','" + OfficePhNo + "','" + FirstLanguage + "'," + NumberofBrothers + "," + NumberOfSysters + "," + JoinBatch + ",'" + CreationTime + "','" + Addresspresent + "'," + StudTypeId + "," + AdmissionTypeId + ",'" + CreatedUserName + "'," + ClassId + "," + BatchId + ",'" + Comment + "'," + CanceledUser + "," + TempStudentId + "," + UseBus + "," + UseHostel + "," + RollNo + ")";
            m_TransationDb.ExecuteQuery(sql);
            sql = "insert into tblstudentleavinghistory(Student_History_Id,AdmissionNumber,TC_Number,ReasonForLeaving) Values(" + _studentid + ",'" + AdmitionNo + "','" + TCNumber + "','" + ReasonForLeaving + "')";
            m_TransationDb.ExecuteQuery(sql);

            sql = "insert into tblstudentclassmap_history(StudentId,ClassId,Standard,BatchId,RollNo) values (" + _studentid + "," + ClassId + "," + standardId + "," + JoinBatch + ",0)";
            m_TransationDb.ExecuteQuery(sql);
            message = "Student records successfully updated";


        }
        catch
        {
            _studentid = -1;
        }
        return _studentid;
    }

    public bool BookScheduledToClass(int ClassId, int BatchId)
    {
        bool Schedule = false;
        string sql = "";
        sql = "select tblinv_bookscheduledetails.BookId from tblinv_bookscheduledetails where tblinv_bookscheduledetails.ClassId="+ClassId+" and tblinv_bookscheduledetails.BatchId="+BatchId+"";
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
            Schedule = true;
        }
        return Schedule;
    }


    public void ScheduleBookToNewStudent(int _classId, int _studentid, int BatchId)
    {
        string sql = "";
        string _sql = "";
        OdbcDataReader Schedulereader = null;
        sql = "select tblinv_bookscheduledetails.BookId,`count`,Id from tblinv_bookscheduledetails where tblinv_bookscheduledetails.ClassId=" + _classId + " and tblinv_bookscheduledetails.BatchId=" + BatchId + "";
        if (m_TransationDb != null)
        {
            Schedulereader = m_TransationDb.ExecuteQuery(sql);
        }
        else
        {
            Schedulereader = m_MysqlDb.ExecuteQuery(sql);
        }
        if (Schedulereader.HasRows)
        {
            while (Schedulereader.Read())
            {
                _sql = "select tblinv_bookschedule.Id from  tblinv_bookschedule where tblinv_bookschedule.StudId=" + _studentid + " and tblinv_bookschedule.ScheduleId=" + int.Parse(Schedulereader.GetValue(2).ToString()) + "";
                if (m_TransationDb != null)
                {
                    m_MyReader = m_TransationDb.ExecuteQuery(_sql);
                }
                else
                {
                    m_MyReader = m_MysqlDb.ExecuteQuery(_sql);
                }
                if (!m_MyReader.HasRows)
                {
                    _sql = "insert into tblinv_bookschedule(BookId,StudId,ScheduleDate,Count,Status,ScheduleId) values (" + int.Parse(Schedulereader.GetValue(0).ToString()) + "," + _studentid + ",'" + System.DateTime.Now.ToString("s") + "'," + int.Parse(Schedulereader.GetValue(1).ToString()) + ",1," + int.Parse(Schedulereader.GetValue(2).ToString()) + ")";
                    if (m_TransationDb != null)
                    {
                        m_TransationDb.ExecuteQuery(_sql);
                    }
                    else
                    {
                        m_MysqlDb.ExecuteQuery(_sql);
                    } 
                }
            }
        }
    }

    public DataSet GetAllStatus()
    {
        DataSet StatusDs = new DataSet();
        string sql = "";
        sql = "select `status`,ID from tbl_admissionstatus where Id<>7";
        StatusDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return StatusDs;
    }

    public DataSet GetAllTemporaryStudents(int status)
    {

        DataSet TempstudentDs = new DataSet();
        string sql = "";

        sql = "select tbltempstdent.Id,tbltempstdent.Name,TempId,tbl_admissionstatus.Id as admId,Gender, DATE_FORMAT( tbltempstdent.`DOB`, '%d/%m/%Y') as DOB , tblstandard.Name as Standard, tblbatch.BatchName,PhoneNumber,Email,DATE_FORMAT( tbltempstdent.`CreatedDate`, '%d/%m/%Y') as CreatedDate , tbl_admissionstatus.Status,Rank from tbltempstdent inner join tblbatch on tblbatch.Id= tbltempstdent.JoiningBatch inner join tbl_admissionstatus on tbl_admissionstatus.Id=  tbltempstdent.AdmissionStatusId inner join tblstandard on tblstandard.Id= tbltempstdent.Standard";
        if (status > 0)
        {
            sql = sql + " where tbltempstdent.AdmissionStatusId="+status+"";
        }
        sql = sql + " order by CreatedDate";
        TempstudentDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return TempstudentDs;
    }

    public int UpdateStatus(DataSet TempDS, int status, out string Msg)
    {
        Msg = "";
        string sql = "";
        int Fail = 0;
        try
        {
            if (status != -1)
            {
                if (TempDS != null && TempDS.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in TempDS.Tables[0].Rows)
                    {
                        sql = "Update tbltempstdent set Rank=" + int.Parse(dr["Rank"].ToString()) + ",`AdmissionStatusId`=" + status + "  where ID=" + int.Parse(dr["Id"].ToString()) + " and TempId='" + dr["TempId"].ToString() + "'";
                        m_MysqlDb.ExecuteQuery(sql);
                    }
                }               
            }
            else
            {
                if (TempDS != null && TempDS.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in TempDS.Tables[0].Rows)
                    {
                        sql = "Update tbltempstdent set Rank=" + int.Parse(dr["Rank"].ToString()) + "  where ID=" + int.Parse(dr["Id"].ToString()) + " and TempId='" + dr["TempId"].ToString() + "'";
                        m_MysqlDb.ExecuteQuery(sql);
                    }
                }
            }

        }
        catch(Exception ex)
        {

            Fail = -1;
            Msg = ex.ToString();
        }
        return Fail;
    }

    public void InsertDataToAutoEmailList(string EmailAddress, string SenderId, string EmailSubject, string EmailBody, int Type)
    {
        string sql = "";
        sql = "Insert into tbl_autoemail(EmailAddress,EmailSubject,EmailBody,TimeTosend,`Type`,`Status`,SenderId) values('" + EmailAddress + "','" + EmailSubject + "','" + EmailBody + "','" + System.DateTime.Now.Date.ToString("s") + "'," + Type + ",0,'" + SenderId + "')";
        m_MysqlDb.ExecuteQuery(sql);
    }

    public int UpdateStatusinViewStudents(string TempId, int status, out string Msg)
    {
        string sql = "";
        Msg = "";
        int success = 0;
        try
        {

            sql = "Update tbltempstdent set AdmissionStatusId=" + status + "  where  TempId='" + TempId + "'";
            m_MysqlDb.ExecuteQuery(sql);
        }
        catch (Exception ex)
        {
            Msg = ex.ToString();
            success = 1;
        }
        return success;
    }



    public DataSet GetGeneralEmailTemplate()
    {
        string sql = "";
        DataSet TemplateDs = new DataSet();
        sql = "select tbl_generalemailtemplate.Type,Id from tbl_generalemailtemplate where tbl_generalemailtemplate.SetVisible=1 and tbl_generalemailtemplate.ShortName<>''";
        TemplateDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return TemplateDs;
    }


    #region Student Switch Class


    public bool SwitchStudentData(int StudentId, int OldClassId, int NewClassId, int BatchId, bool _RemoveAttendance, out string ExamStr, out string FeeStr, out string AttendanceStr, out string ErrorMsg)
    {
        bool _valid = false;
        ExamStr = "";
        FeeStr = "";
        AttendanceStr = "";
        ErrorMsg = "";


        ValidateExams(StudentId, OldClassId, NewClassId, out ExamStr, out ErrorMsg);

        ValidateFees(StudentId, OldClassId, NewClassId, BatchId, out FeeStr, out ErrorMsg);

        ValidateAttenadance(StudentId, OldClassId, BatchId,_RemoveAttendance,out AttendanceStr, out ErrorMsg);


        _valid = false;

        return _valid;
    }

    private void ValidateAttenadance(int StudentId, int OldClassId, int BatchId, bool _RemoveAttendance, out string AttendanceStr, out string ErrorMsg)
    {
        AttendanceStr = "";
        ErrorMsg = "";
        if (_RemoveAttendance)
        {
            RemoveAttendance(StudentId, GetStandard(StudentId.ToString()), OldClassId, BatchId);
            AttendanceStr = "Attendance marked for the student for this year has been removed";
        }
    }

    private void RemoveAttendance(int StudentId, int _StandardId, int OldClassId, int _batchid)
    {
        if (AttendanceTables_Exits(_StandardId.ToString(), _batchid))
        {
            string sql = "DELETE FROM tblattdstud_std" + _StandardId + "yr" + _batchid + "  WHERE StudentId=" + StudentId;
            m_TransationDb.ExecuteQuery(sql);
        }

    }

    public bool AttendanceTables_Exits(string StdId, int YearId)
    {
        bool valid = false;
        string _tablename = "";
        string End_Region = "std" + StdId + "yr" + YearId;
        string Sql = "show tables like 'tblattdcls_" + End_Region + "'";

        m_MyReader = m_TransationDb.ExecuteQuery(Sql);

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

            m_MyReader = m_TransationDb.ExecuteQuery(Sql);

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
 
    private void ValidateFees(int StudentId, int OldClassId, int NewClassId,int BatchId, out string FeeStr, out string ErrorMsg)
    {
        FeeStr = "";
        ErrorMsg = "";
        string _tempstr, feestr1 = "";

        CheckLiveTransactions(StudentId, OldClassId, NewClassId, BatchId, out FeeStr);
        CheckClearingTransactions(StudentId, OldClassId, NewClassId, BatchId);
        ConvertSettledAdvanceTo_NewAdvance(StudentId, OldClassId, NewClassId, BatchId, out _tempstr);
        if (FeeStr == "")
        {
            FeeStr = _tempstr;
        }
        ConvertStudentFeeSchedules(StudentId, OldClassId, NewClassId, BatchId, out feestr1);
        if (feestr1 != "" && FeeStr!="")
        {
            FeeStr = FeeStr + "<br/>" + feestr1;
        }


        RemoveFees_SCheduledtoClass(StudentId, OldClassId, BatchId);
        UpdateClassIdinFee(StudentId, OldClassId, BatchId,NewClassId);
        
    }

    private void ConvertSettledAdvanceTo_NewAdvance(int StudentId, int OldClassId, int NewClassId, int BatchId, out string _tempstr)
    {
        _tempstr = "";
       // string sql = "SELECT tblfeeadvancetransaction.StudentName,tblfeeadvancetransaction.FeeName,tblfeeadvancetransaction.PeriodName,tblfeeadvancetransaction.Amount,tblfeeadvancetransaction.FeeId,tblfeeadvancetransaction.PeriodId,tblfeeadvancetransaction.TempId FROM tblfeeadvancetransaction INNER JOIN tblfeeaccount ON tblfeeaccount.Id=tblfeeadvancetransaction.FeeId  WHERE tblfeeadvancetransaction.BillNo='NIL' AND tblfeeadvancetransaction.`Type`=0 AND tblfeeaccount.AssociatedId=1 AND tblfeeadvancetransaction.StudentId=" + StudentId + " AND tblfeeadvancetransaction.BatchId="+BatchId;
         string sql = "SELECT tblfeeadvancetransaction.StudentName,tblfeeadvancetransaction.FeeName,tblfeeadvancetransaction.PeriodName,tblfeeadvancetransaction.Amount,tblfeeadvancetransaction.FeeId,tblfeeadvancetransaction.PeriodId,tblfeeadvancetransaction.TempId FROM tblfeeadvancetransaction WHERE tblfeeadvancetransaction.BillNo='NIL' AND tblfeeadvancetransaction.`Type`=0 AND tblfeeadvancetransaction.StudentId="+StudentId+" AND tblfeeadvancetransaction.BatchId="+BatchId;
        DataSet MyDataSet = m_TransationDb.ExecuteQueryReturnDataSet(sql);
        if (MyDataSet != null && MyDataSet.Tables[0].Rows.Count > 0)
        {
            string StudentName = "", FeeName = "", PeriodName = "", Amount = "", FeeId = "", PeriodId = "", TempId = "";
            foreach (DataRow dr in MyDataSet.Tables[0].Rows)
            {
                StudentName = dr[0].ToString();
                FeeName = dr[1].ToString();
                PeriodName = dr[2].ToString();
                Amount = dr[3].ToString();
                FeeId = dr[4].ToString();
                PeriodId = dr[5].ToString();
                TempId = dr[6].ToString();

                string BillNo = "";
                BillNo = GetBillNo_Transaction(StudentId, BatchId, FeeId, PeriodId);
                if (BillNo.Trim() != "")
                {
                    FeeManage MyFeemanager = new FeeManage(m_TransationDb);
                    MyFeemanager.SaveAdvanceEntry(StudentId, TempId, int.Parse(PeriodId),int.Parse(FeeId), BatchId, FeeName, PeriodName, double.Parse(Amount), StudentName, BillNo);
                    MyFeemanager = null;
                    _tempstr = "Amount collected from the student has been converted as advance.";

                    #region mani update on 26.06.2014
                    sql = "delete from tblfeeadvancetransaction where tblfeeadvancetransaction.BillNo='NIL' AND tblfeeadvancetransaction.Type=0  AND tblfeeadvancetransaction.StudentId="+StudentId+" and  tblfeeadvancetransaction.BatchId="+BatchId;
                    m_TransationDb.ExecuteQuery(sql);
                    #endregion
                }
               
                
            }
        }
    }

    private string GetBillNo_Transaction(int StudentId, int BatchId, string FeeId, string PeriodId)
    {
        string _BillNo = "";
        string sql = "SELECT BillNo FROM  tbltransaction WHERE Canceled=0 AND PaymentElementId=-1 AND BatchId="+BatchId+" AND CollectionType=3 AND PeriodId="+PeriodId+" AND FeeId="+FeeId+" AND UserId=" + StudentId;
        OdbcDataReader _myreader = m_TransationDb.ExecuteQuery(sql);
        if (_myreader.HasRows)
        {
            _BillNo=_myreader.GetValue(0).ToString();
        }
        return _BillNo;
    }

    private void ConvertStudentFeeSchedules(int StudentId, int OldClassId, int NewClassId, int BatchId, out string Feestr1)
    {
        Feestr1 = "";
        string sql = "SELECT tblfeeschedule.Id,tblfeeschedule.FeeId,tblfeeschedule.Duedate,tblfeeschedule.LastDate,tblfeeschedule.PeriodId FROM tblfeestudent INNER JOIN tblfeeschedule ON tblfeestudent.SchId=tblfeeschedule.Id INNER JOIN tblfeeaccount ON tblfeeschedule.FeeId=tblfeeaccount.Id WHERE tblfeeaccount.AssociatedId=2 AND tblfeeschedule.ClassId="+OldClassId+" AND tblfeeschedule.BatchId="+BatchId+" AND tblfeestudent.StudId=" + StudentId;
        DataSet MyDataSet = m_TransationDb.ExecuteQueryReturnDataSet(sql);
        if (MyDataSet != null && MyDataSet.Tables[0].Rows.Count > 0)
        {
            string ScheduleId = "", PeriodId = "", FeeId = "";
            string _Duedate = "", _LastDate = "";
            foreach (DataRow dr in MyDataSet.Tables[0].Rows)
            {
                ScheduleId = dr[0].ToString();
                PeriodId = dr[4].ToString();
                FeeId = dr[1].ToString();
                _Duedate = dr[2].ToString();
                _LastDate = dr[3].ToString();
                ConvertPaid_StudentAssociatedFees(StudentId, ScheduleId, PeriodId, FeeId, BatchId, NewClassId, _Duedate, _LastDate);
                Feestr1 = "Fees scheduled to student has been saved to new class";
            }
        }
    }





    private void CheckLiveTransactions(int StudentId, int OldClassId, int NewClassId, int BatchId, out string FeeStr)
    {
        FeeStr = "";
        string sql = "SELECT tbltransaction.PaymentElementId,tblfeeaccount.AssociatedId,tblfeeschedule.PeriodId,tblfeeschedule.FeeId,tbltransaction.Amount,tbltransaction.StudentName,tbltransaction.FeeName,tbltransaction.PeriodName,tblfeeschedule.Duedate,tblfeeschedule.LastDate,tbltransaction.TransationId,tbltransaction.BillNo,tbltransaction.AccountTo FROM tbltransaction INNER JOIN tblfeebill ON tbltransaction.BillNo=tblfeebill.BillNo INNER JOIN tblfeeschedule ON tblfeeschedule.Id=tbltransaction.PaymentElementId INNER JOIN tblfeeaccount ON tblfeeaccount.Id=tblfeeschedule.FeeId WHERE tblfeeaccount.AssociatedId=1 AND tbltransaction.Canceled=0 AND tbltransaction.CollectionType=1 AND tbltransaction.PaymentElementId <> -1 AND tblfeeschedule.BatchId=" + BatchId + " AND tblfeebill.StudentID=" + StudentId;
        DataSet MyDataSet = m_TransationDb.ExecuteQueryReturnDataSet(sql);
        if (MyDataSet != null && MyDataSet.Tables[0].Rows.Count > 0)
        {
            string ScheduleId = "", PeriodId = "", FeeId = "", AssociatedType = "", TransId = "", BillNo = "";
            string StudentName = "", FeeName = "", PeriodName = "", AccountTo = "";
            string _Amount = "";
            string _Duedate = "", _LastDate = "";
            foreach (DataRow dr in MyDataSet.Tables[0].Rows)
            {
                ScheduleId = dr[0].ToString();
                AssociatedType = dr[1].ToString();
                PeriodId = dr[2].ToString();
                FeeId = dr[3].ToString();
                _Amount = dr[4].ToString();
                StudentName = dr[5].ToString();
                FeeName = dr[6].ToString();
                PeriodName = dr[7].ToString();
                _Duedate = dr[8].ToString();
                _LastDate = dr[9].ToString();
                TransId = dr[10].ToString();
                BillNo = dr[11].ToString();
                AccountTo = dr[12].ToString();

                if (AccountTo == "3")
                {
                    Removededuction(StudentId, ScheduleId, "tbltransaction");
                }
                else
                {
                    ConvertPaid_ScheduledClassFees_to_Advance(StudentId, ScheduleId, int.Parse(PeriodId), int.Parse(FeeId), BatchId, NewClassId, StudentName, FeeName, PeriodName, _Amount, BillNo, true, "tbltransaction");
                    FeeStr = "Amount collected from the student has been converted as advance.";
                }

            }

        }
    }

    private void Removededuction(int StudentId, string ScheduleId, string transactiontable)
    {
        string sql = "DELETE FROM " + transactiontable + " WHERE UserId=" + StudentId + " AND PaymentElementId=" + ScheduleId;
        m_TransationDb.ExecuteQuery(sql);
    }

    private void CheckClearingTransactions(int StudentId, int OldClassId, int NewClassId, int BatchId)
    {
        string sql = "SELECT tbltransactionclearence.PaymentElementId,tblfeeaccount.AssociatedId,tblfeeschedule.PeriodId,tblfeeschedule.FeeId,tbltransactionclearence.Amount,tbltransactionclearence.StudentName,tbltransactionclearence.FeeName,tbltransactionclearence.PeriodName,tblfeeschedule.Duedate,tblfeeschedule.LastDate,tbltransactionclearence.TransationId,tbltransactionclearence.BillNo,tbltransactionclearence.AccountTo FROM tbltransactionclearence INNER JOIN tblfeebillclearence ON tbltransactionclearence.BillNo=tblfeebillclearence.BillNo INNER JOIN tblfeeschedule ON tblfeeschedule.Id=tbltransactionclearence.PaymentElementId INNER JOIN tblfeeaccount ON tblfeeaccount.Id=tblfeeschedule.FeeId WHERE tblfeeaccount.AssociatedId=1 AND tbltransactionclearence.Canceled=0 AND tbltransactionclearence.CollectionType=1 AND tbltransactionclearence.PaymentElementId <> -1 AND tblfeeschedule.BatchId=" + BatchId + " AND tblfeebillclearence.StudentID=" + StudentId;
        DataSet MyDataSet = m_TransationDb.ExecuteQueryReturnDataSet(sql);
        if (MyDataSet != null && MyDataSet.Tables[0].Rows.Count > 0)
        {
            string ScheduleId = "", PeriodId = "", FeeId = "", AssociatedType = "", TransId = "", BillNo = "";
            string StudentName = "", FeeName = "", PeriodName = "", AccountTo = "";
            string _Amount = "";
            string _Duedate = "", _LastDate = "";
            foreach (DataRow dr in MyDataSet.Tables[0].Rows)
            {
                ScheduleId = dr[0].ToString();
                AssociatedType = dr[1].ToString();
                PeriodId = dr[2].ToString();
                FeeId = dr[3].ToString();
                _Amount = dr[4].ToString();
                StudentName = dr[5].ToString();
                FeeName = dr[6].ToString();
                PeriodName = dr[7].ToString();
                _Duedate = dr[8].ToString();
                _LastDate = dr[9].ToString();
                TransId = dr[10].ToString();
                BillNo = dr[11].ToString();
                AccountTo = dr[12].ToString();

                if (AccountTo == "3")
                {
                    Removededuction(StudentId, ScheduleId, "tbltransaction");
                }
                else
                {
                    ConvertPaid_ScheduledClassFees_to_Advance(StudentId, ScheduleId, int.Parse(PeriodId), int.Parse(FeeId), BatchId, NewClassId, StudentName, FeeName, PeriodName, _Amount, BillNo, false, "tbltransactionclearence");
                }

            }
        }
    }

    private void UpdateClassIdinFee(int StudentId, int OldClassId, int BatchId, int NewClassId)
    {
        string sql = "UPDATE tblfeebill SET ClassID=" + NewClassId + " WHERE StudentID=" + StudentId + " AND ClassID=" + OldClassId;
        m_TransationDb.ExecuteQuery(sql);

        sql = "UPDATE tbltransaction SET ClassId=" + NewClassId + " WHERE UserId=" + StudentId + " AND ClassId=" + OldClassId;
        m_TransationDb.ExecuteQuery(sql);


        sql = "UPDATE tblfeebillclearence SET ClassID=" + NewClassId + " WHERE StudentID=" + StudentId + " AND ClassID=" + OldClassId;
        m_TransationDb.ExecuteQuery(sql);

        sql = "UPDATE tbltransactionclearence SET ClassId=" + NewClassId + " WHERE UserId=" + StudentId + " AND ClassId=" + OldClassId;
        m_TransationDb.ExecuteQuery(sql);
    }

    private void RemoveFees_SCheduledtoClass(int StudentId, int OldClassId, int BatchId)
    {
        string sql = "DELETE FROM tblfeestudent WHERE tblfeestudent.StudId=" + StudentId + " AND tblfeestudent.SchId  IN (SELECT tblfeeschedule.Id FROM tblfeeschedule INNER JOIN tblfeeaccount ON tblfeeaccount.Id=tblfeeschedule.FeeId WHERE tblfeeaccount.AssociatedId=1 AND tblfeeschedule.BatchId=" + BatchId + " AND tblfeeschedule.ClassId=" + OldClassId + ")";
        m_TransationDb.ExecuteQuery(sql);
    }

    private void ConvertPaid_ScheduledClassFees_to_Advance(int _StudId, string ScheduleId, int _PeriodId, int _FeeId, int _BatchId, int NewClassId, string _StudentName, string _FeeName, string _Period, string _Amount, string BillNo,bool NeedStoreAdvance,string transactiontable)
    {
        if (NeedStoreAdvance)
        {
            FeeManage MyFeemanager = new FeeManage(m_TransationDb);
            MyFeemanager.SaveAdvanceEntry(_StudId, "", _PeriodId, _FeeId, _BatchId, _FeeName, _Period, double.Parse(_Amount), _StudentName, BillNo);
            MyFeemanager = null;
        }
        string sql = "UPDATE " + transactiontable + " SET PaymentElementId=-1,CollectionType=3 WHERE UserId=" + _StudId + " AND PaymentElementId=" + ScheduleId;
        m_TransationDb.ExecuteQuery(sql);
    }

    



    private void ConvertPaid_StudentAssociatedFees(int StudentId, string ScheduleId, string PeriodId, string FeeId, int BatchId, int NewClassId, string _Duedatestr, string _LastDatestr )
    {
        DateTime _DueDate = new DateTime();
        DateTime _LastDate = new DateTime();
        DateTime.TryParse(_Duedatestr, out _DueDate);
        DateTime.TryParse(_LastDatestr, out _LastDate);
        int NextScheduleId = 0;

        if (SameFeeSchedule_InNextClassExist(FeeId, PeriodId, BatchId, NewClassId, out NextScheduleId))
        {

            UpdateOldFeestudentScheduleId(NextScheduleId, ScheduleId, StudentId);
           

        }
        else
        {
            if (CreateFeeSchedule(BatchId, FeeId, PeriodId, NewClassId, _DueDate, _LastDate, out NextScheduleId))
            {
                UpdateOldFeestudentScheduleId(NextScheduleId, ScheduleId, StudentId);
            }

        }
    }

    private void UpdateOldFeestudentScheduleId(int NextScheduleId, string OldScheduleId, int StudentId)
    {
        string sql = "UPDATE tblfeestudent SET SchId=" + NextScheduleId + " WHERE StudId=" + StudentId + " AND SchId=" + OldScheduleId;
        m_TransationDb.ExecuteQuery(sql);

        UpdateScheduleId_transation(NextScheduleId, OldScheduleId, StudentId);


 
    }

    private void UpdateScheduleId_transation(int NextScheduleId, string OldScheduleId, int StudentId)
    {
        string sql = "UPDATE tbltransaction SET PaymentElementId=" + NextScheduleId + " WHERE UserId=" + StudentId + " AND PaymentElementId=" + OldScheduleId;
        m_TransationDb.ExecuteQuery(sql);
    }

    private bool CreateFeeSchedule(int BatchId, string _feeid, string PeriodId, int ClassId, DateTime _duedate, DateTime _lastdate, out int _SchId)
    {
        bool _valid = false;
        _SchId = 0;
        string sql = "SELECT tblfeeschedule.Id FROM tblfeeschedule WHERE tblfeeschedule.ClassId=" + ClassId + " AND tblfeeschedule.PeriodId=" + PeriodId + " AND tblfeeschedule.BatchId=" + BatchId + " AND tblfeeschedule.FeeId=" + _feeid ;
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _SchId = int.Parse(m_MyReader.GetValue(0).ToString());
            sql = "UPDATE tblfeeschedule SET Duedate= '" + _duedate.Date.ToString("s") + "', LastDate = '" + _lastdate.Date.ToString("s") + "' WHERE Id =" + _SchId;
            m_TransationDb.TransExecuteQuery(sql);
        }
        else
        {
            sql = "INSERT INTO tblfeeschedule(ClassId,PeriodId,FeeId,BatchId,Duedate,LastDate) VALUES(" + ClassId + "," + PeriodId + "," + _feeid + "," + BatchId + ",'" + _duedate.Date.ToString("s") + "','" + _lastdate.Date.ToString("s") + "')";
            m_TransationDb.TransExecuteQuery(sql);
            sql = "SELECT tblfeeschedule.Id FROM tblfeeschedule WHERE tblfeeschedule.ClassId=" + ClassId + " AND tblfeeschedule.PeriodId=" + PeriodId + " AND tblfeeschedule.BatchId=" + BatchId + " AND tblfeeschedule.FeeId=" + _feeid + "";
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _SchId = int.Parse(m_MyReader.GetValue(0).ToString()); 
            }

        }

        if (_SchId > 0)
        {
            _valid = true;
        }


        return _valid;
    }

    private bool SameFeeSchedule_InNextClassExist(string FeeId, string PeriodId, int BatchId, int NewClassId, out int NextScheduleId)
    {
        bool _valid = false;
        NextScheduleId = 0;
        string sql = "SELECT Id FROM tblfeeschedule WHERE FeeId=" + FeeId + " AND BatchId=" + BatchId + " AND ClassId=" + NewClassId + " AND PeriodId=" + PeriodId;
        OdbcDataReader _myreader= m_TransationDb.ExecuteQuery(sql);
        if (_myreader.HasRows)
        {
            int.TryParse(_myreader.GetValue(0).ToString(), out NextScheduleId);
            if (NextScheduleId > 0)
            {
                _valid = true;
            }
        }
        return _valid;
    }

    private void ValidateExams(int StudentId, int OldClassId, int NewClassId, out string ExamStr, out string ErrorMsg)
    {
        ExamStr = "";
        ErrorMsg = "";
        int _Examcount = 0;
        string sql = "SELECT COUNT(tblstudentmark.ExamSchId) FROM tblstudentmark INNER JOIN tblexamschedule ON tblexamschedule.Id=tblstudentmark.ExamSchId WHERE tblexamschedule.Status<>'Completed' AND tblstudentmark.StudId=" + StudentId;
        OdbcDataReader _myreader = m_TransationDb.ExecuteQuery(sql);
        if (_myreader.HasRows)
        {
            int.TryParse(_myreader.GetValue(0).ToString(), out _Examcount);
        }

        sql = "DELETE FROM tblstudentmark WHERE tblstudentmark.StudId=" + StudentId + " AND tblstudentmark.ExamSchId IN (SELECT tblexamschedule.Id FROM tblexamschedule INNER JOIN tblclassexam ON tblexamschedule.ClassExamId=tblclassexam.Id WHERE tblexamschedule.`Status`<>'Completed' AND tblclassexam.ClassId=" + OldClassId + ")";
        m_TransationDb.ExecuteQuery(sql);

        if (_Examcount > 0)
        {
            ExamStr = "Student marks removed from "+_Examcount+" exam.";
        }
    }

    public void UpdateClassInStudentMap(int StudentId, string newStandard, string NewClassId)
    {
        string sql = "UPDATE tblstudentclassmap SET ClassId=" + NewClassId + ",Standard=" + newStandard + " WHERE StudentId =" + StudentId;
        m_TransationDb.TransExecuteQuery(sql);
    }

    public void UpdateLastClassId(int StudentId, int ClassId)
    {
        string sql = "update tblstudent set LastClassId=" + ClassId + " where Id=" + StudentId;
        m_TransationDb.ExecuteQuery(sql);
    }

    public void InsertClassSwichinghistory(int StudentId, int OldClassId, string OldStandard, int BatchId, string OldRollNo, string Remark)
    {
        string sql = "Insert into tblstudentclassmap_switch(StudentId,ClassId,Standard,BatchId,RollNo,Remark) values(" + StudentId + ","+OldClassId+",'"+OldStandard+"',"+BatchId+","+OldRollNo+",'"+Remark+"')";
        m_TransationDb.ExecuteQuery(sql);
    }


    #endregion





    public void UpdateInventoryDetails(int _StudentId)
    {
        string sql = "";
        string _sql = "";
        OdbcDataReader IdReader = null;
        sql = "select Id from tblinv_bookschedule where StudId=" + _StudentId + "";
        if (m_TransationDb != null)
        {

            IdReader = m_TransationDb.ExecuteQuery(sql);
        }
        else
        {
            IdReader = m_MysqlDb.ExecuteQuery(sql);
        }

        if (IdReader.HasRows)
        {
            int Id = 0;
            while (IdReader.Read())
            {
                int.TryParse(IdReader.GetValue(0).ToString(), out Id);
                sql = "DELETE FROM tblinv_bookschedule  WHERE Id=" + Id + "";
                if (m_TransationDb != null)
                {

                    m_TransationDb.ExecuteQuery(sql);
                }
                else
                {
                    m_MysqlDb.ExecuteQuery(sql);
                }

            }
        }

        //sql = "select Id from tblinv_bookissue where StudId=" + _StudentId + "";
        //if (m_TransationDb != null)
        //{

        //    IdReader = m_TransationDb.ExecuteQuery(sql);
        //}
        //else
        //{
        //    IdReader = m_MysqlDb.ExecuteQuery(sql);
        //}


        //if (IdReader.HasRows)
        //{
        //    int IssueId = 0;
        //    while (IdReader.Read())
        //    {
        //        int.TryParse(IdReader.GetValue(0).ToString(), out IssueId);
        //        _sql = "insert into tblinv_bookissue_history(Id,ScheduleId,BookId,StudId,BatchId,ClassId,IssueDate,Count,Cost,IsSpcBook) select * from tblinv_bookissue where ID="+IssueId+"";
        //        if (m_TransationDb != null)
        //        {

        //            m_TransationDb.ExecuteQuery(_sql);
        //        }
        //        else
        //        {
        //            m_MysqlDb.ExecuteQuery(_sql);
        //        }

        //        _sql = "DELETE FROM tblinv_bookissue WHERE Id=" + IssueId + "";
        //        if (m_TransationDb != null)
        //        {

        //            m_TransationDb.ExecuteQuery(_sql);
        //        }
        //        else
        //        {
        //            m_MysqlDb.ExecuteQuery(_sql);
        //        }

        // }

        //}

    }



    public StringBuilder GenerateDynamicTC(int StudentId, SchoolClass objSchool, out string Err)
    {
        Err = "";
        StringBuilder _TC = new StringBuilder();
        string sql = "select tbltcformat.TCFormat from tbltcformat ";
        OdbcDataReader MyReader = m_MysqlDb.ExecuteQuery(sql);

        if (!MyReader.HasRows)
        {
            //dominic show error mseeage
            Err = "Tc Format is not available.Create a TC format for the institution before create TC";
           
        }
        else
        {
            _TC.Append(MyReader.GetValue(0).ToString());
            _TC = LoadLogo(_TC, objSchool);
            _TC = LoadSchoolDetails(_TC);
            _TC = LoadStudentDetails(StudentId, _TC);
        }
        return _TC;

    }



    private StringBuilder LoadStudentDetails(int StudId, StringBuilder _TC)
    {
        DateTime Date_Dob, Date_LastAttendance, Date_TcRcvedDate, Date_TCIssue;//, Date_DteOFAdmission;

        String Sql = "SELECT * FROM tbltc WHERE StudentId=" + StudId;
        OdbcDataReader MyReader = m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
           
            _TC.Replace("[$TCNo$]", MyReader.GetValue(2).ToString());
            _TC.Replace("[$StudName$]", MyReader.GetValue(3).ToString());
            _TC.Replace("[$SchoolName$]", MyReader.GetValue(4).ToString());
            _TC.Replace("[$AdmissionNo$]", MyReader.GetValue(5).ToString());
            _TC.Replace("[$CumulativeNo$]", MyReader.GetValue(6).ToString());
            _TC.Replace("[$Sex$]", MyReader.GetValue(7).ToString());
            _TC.Replace("[$FatherName$]", MyReader.GetValue(8).ToString());
            _TC.Replace("[$MotherName$]", MyReader.GetValue(33).ToString());
            string DOB = "";
            string DOBinWords = "";
            bool includeTime = false, UK = true; 
            if(DateTime.TryParse(MyReader.GetValue(13).ToString(),out Date_Dob))
            {
                DOB=General.GerFormatedDatVal(Date_Dob);
                DOBinWords = General.DateToText(Date_Dob, includeTime, UK);

            }
         
            _TC.Replace("[$DOB$]", DOB);
            _TC.Replace("[$DOBinword$]", DOBinWords);
            _TC.Replace("[$ResAddress$]", MyReader.GetValue(34).ToString());
            _TC.Replace("[$Nationality$]", MyReader.GetValue(9).ToString());
            _TC.Replace("[$Religion$]", MyReader.GetValue(10).ToString());
            _TC.Replace("[$Caste$]", MyReader.GetValue(11).ToString());
            _TC.Replace("[$SCST$]", MyReader.GetValue(12).ToString());
            _TC.Replace("[$LastStandard$]", MyReader.GetValue(14).ToString());
            _TC.Replace("[$LanguageStudied$]", MyReader.GetValue(15).ToString());
            _TC.Replace("[$SubjectStudied$]", MyReader.GetValue(36).ToString());
            _TC.Replace("[$Medium$]", MyReader.GetValue(16).ToString());

            _TC.Replace("[$Syllabus$]", MyReader.GetValue(17).ToString());
            DateTime AdmissionDate = DateTime.Now;
            // Dominic
            //Here admission date is saved as text format in db so we can display it as direct string 
          
            _TC.Replace("[$DoA$]", MyReader.GetValue(18).ToString());

            _TC.Replace("[$Result$]", MyReader.GetValue(19).ToString());
            _TC.Replace("[$Fee$]", MyReader.GetValue(20).ToString());
            _TC.Replace("[$FeeCon$]", MyReader.GetValue(21).ToString());
            _TC.Replace("[$Scholarship$]", MyReader.GetValue(22).ToString());
            _TC.Replace("[$Medical$]", MyReader.GetValue(23).ToString());

            string LDate = "";
            
            if (DateTime.TryParse(MyReader.GetValue(24).ToString(), out Date_LastAttendance))
            {
                LDate = General.GerFormatedDatVal(Date_LastAttendance);
            }

            _TC.Replace("[$LastDate$]", LDate);


            string RDate = "";

            if (DateTime.TryParse(MyReader.GetValue(25).ToString(), out Date_TcRcvedDate))
            {
                RDate = General.GerFormatedDatVal(Date_TcRcvedDate);
            }


            _TC.Replace("[$DateOfTCApp$]", RDate);


            string IDate = "";

            if (DateTime.TryParse(MyReader.GetValue(26).ToString(), out Date_TCIssue))
            {
                IDate = General.GerFormatedDatVal(Date_TCIssue);
            }


            _TC.Replace("[$DateOfTCIssue$]", IDate);

            _TC.Replace("[$TotalDays$]", MyReader.GetValue(27).ToString());
            _TC.Replace("[$NoOfAttendance$]", MyReader.GetValue(28).ToString());
            _TC.Replace("[$Reason$]", MyReader.GetValue(35).ToString());
            _TC.Replace("[$LastExamDetails$]", MyReader.GetValue(37).ToString());
            _TC.Replace("[$Character$]", MyReader.GetValue(29).ToString());
            _TC.Replace("[$NextSchool$]", MyReader.GetValue(38).ToString());
            DateTime Date_LastClassDateIssue;
             if (DateTime.TryParse(MyReader.GetValue(39).ToString(), out Date_LastClassDateIssue))
            {
                 _TC.Replace("[$LastClassPromotionDate$]", General.GerFormatedDatVal(Date_LastClassDateIssue)); 

            }


          
        }
        return _TC;
    }

    private StringBuilder LoadLogo(StringBuilder _TC, SchoolClass objSchool)
    {//dominic test the image name 
        string _image = " <img src=\"Handler/ImageReturnHandler.ashx?id=1&type=Logo\" alt=\" \" height=\"120px\" width=\"125px\" />";
        _TC.Replace("LOGO : [$Logo$]", _image);
        return _TC;
    }

    private StringBuilder LoadSchoolDetails(StringBuilder _TC)
    {
        String Sql = "SELECT SchoolName,Address FROM tblschooldetails WHERE Id=1";

       OdbcDataReader  MyReader = m_MysqlDb.ExecuteQuery(Sql);

        if (MyReader.HasRows)
        {
            _TC.Replace("[$InstitutionName$]", MyReader.GetValue(0).ToString());
            _TC.Replace("[$Address$]", MyReader.GetValue(1).ToString());

        }
        return _TC;
    }

    public bool IsDynamicTCNumber()
    {
        bool value = false;
         string sql = "SELECT Value FROM tblconfiguration WHERE Module='TC' and  Name='AutomaticTCNum'";
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
            if (Convert.ToInt32(m_MyReader["Value"]) == 1)
            {
                value= true;
            }
            else
            {
                value= false;
            }
        }
        return value;

    }

    public void CreateStudentDetails(string  _dynamicfieldname,string  dynamicdatas,  int studentId)
    {
        string sql = "INSERT INTO tblstudentdetails(StudentId," + _dynamicfieldname + ") VALUES (" + studentId + "," + dynamicdatas + ")";
        m_TransationDb.TransExecuteQuery(sql);
    }

    public bool IsStudentIdUnique(string studentid,string value)
    {
        bool valid = true;
        string editsql = "";
        int id = 0;
        OdbcDataReader StudentIdReader = null;
        if (value !="0")
        {
            editsql = " and tblstudent.Id<>"+value+"";
        }
        if (studentid != "")
        {
          string sql= "";
            sql = "select tblstudent.Id from tblstudent where tblstudent.StudentId='" + studentid + "' " + editsql + "";
            StudentIdReader = m_MysqlDb.ExecuteQuery(sql);
            if (StudentIdReader.HasRows)
            {
                int.TryParse(StudentIdReader.GetValue(0).ToString(), out id);
                if (id > 0)
                {
                    valid = false;
                }
            }
        }

        return valid;
    }

    public DataSet GetStudenttableFieldWithoutManadatoryField()
    {
       string sql= "Select SoftwareField,ExcelFiled from tblexcelfieldmap  where tblexcelfieldmap.IsMandatory=0 and IsDynamic=0";
       return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
    }

    public DataSet GetParentLogInReport(DateTime fromdate, DateTime Todate,int type)
    {
        string sql="";
        sql = "Select  tblparent_parentdetails.Id, tblparent_parentdetails.Name, tblparent_parentdetails.UserName, Date_Format(tblparent_parentdetails.LastLogin,'%d-%M-%Y') as `Last Login` from tblparent_parentdetails where CanLogin=1";
        
       

        return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
    }

    public string GetStudentName(int StudentId)
    {
        string StudentName = "";
        string sql = "SELECT StudentName FROM tblview_student where Id=" + StudentId;
        this.m_MyReader = this.m_MysqlDb.ExecuteQuery(sql);
        if (this.m_MyReader.HasRows)
        {
            StudentName = this.m_MyReader.GetValue(0).ToString();
        }
        this.m_MyReader.Close();
        return StudentName;
    }

    #region MIS_Rrport_Area

    public DataSet getMISGroups()
    {
        string sql = "select tblmisgroup.Id as Value, tblmisgroup.GroupName as Text from tblmisgroup";
        return  m_MysqlDb.ExecuteQueryReturnDataSet(sql);
    }

   public void CreateMISGroup(string GroupName)
    {
        string sql = "insert into tblmisgroup(GroupName ) values('" + GroupName + "')";
        m_MysqlDb.ExecuteQuery(sql);
    }

    public DataSet getMISStaffDetails(string groupId)
    {
         string sql = @"select tbluser.id, tbluser.UserName, tblrole.RoleName , tblmisgroup.GroupName from tbluser 
                    inner join tblrole on tblrole.id=tbluser.id inner join
                    tblmisgroupusermap on tblmisgroupusermap.UserId= tbluser.id
                    inner join tblmisgroup on tblmisgroup.Id= tblmisgroupusermap.GroupId
                    where  tblmisgroupusermap.GroupId=" + groupId;
     
        return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
    }
   
    public DataSet getMISUnAssignedStaffDetails()
    {
        string sql = @"select tbluser.id, tbluser.UserName, tblrole.RoleName from tbluser 
                    inner join tblrole on tblrole.id=tbluser.id where tbluser.id not in (SELECT   tblmisgroupusermap.UserId from  tblmisgroupusermap)";
        
        return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
    }

    public void MapUserWithGroup(string StaffId, string groupId)
    {
        string sql=" insert into tblmisgroupusermap (GroupId,UserId) values("+groupId+","+StaffId+")";
        
        m_MysqlDb.ExecuteQuery(sql);
    }

    public bool isMISGroupExists(string GroupName)
    {
        string sql = "select tblmisgroup.Id as Value, tblmisgroup.GroupName as Text from tblmisgroup";
        
        DataSet dt=  m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }    

    public void RemoveUserFromSelectedGroup(string StaffId, string groupId)
    {
        string sql = " delete from tblmisgroupusermap where GroupId ="+ groupId+" and UserId="+ StaffId ;

        m_MysqlDb.ExecuteQuery(sql);
    }

    #endregion

    public DataSet GetParentLogIn_UserLoginlogs(DateTime fromdate, DateTime Todate)
    {

        string sql = "select Id,UserName,`Action`,`Time`,`Description` from tbllog where UserTypeId=2 and `Action`='ParentLogin_UserLoggin' and date(`Time`) between '" + fromdate.ToString("s") + "' and '" + Todate.ToString("s") + "'";
        return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
    }

    #region mani ivr integration work

    public bool GetstudentIdfromPhonenumber(string phoneNumber, out string ErrorMessage, out string[] return_studentDetails)
    {
        string Stu_details = "";
        return_studentDetails = null;
        int _StudentId = 0, _batchid = getBatchId();
        int _no_workingdays = 0, _no_fulldays = 0, _no_absentdays = 0, _no_holidays = 0, _no_halfdays = 0;
        double _attendencepersent = 0.0;
        string _Examname = "", _Grad = "";
        double _Avg = 0.0;

        bool valid = true;
        ErrorMessage = "";
        try
        {
            string sql = "SELECT tblstudent.StudentId,tblstudent.StudentName,tblstudent.DOB,tblstudent.Id from tblsmsparentlist INNER JOIN tblstudent on tblstudent.Id=tblsmsparentlist.Id where tblsmsparentlist.PhoneNo='" + phoneNumber + "' AND tblstudent.StudentId!=''";
            m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (m_MyDataSet.Tables[0].Rows.Count >0)
            {
                return_studentDetails = new string[m_MyDataSet.Tables[0].Rows.Count];
                int Index = 0;
                foreach (DataRow dr in m_MyDataSet.Tables[0].Rows)
                {
                    FeeManage MyFeeManage = new FeeManage(m_MysqlDb);
                    Attendance MyAttendance = new Attendance(m_MysqlDb);
                    Stu_details =CheckValueNotQualtoZero(dr[0].ToString())+",";
                    Stu_details += CheckValueNotQualtoZero(dr[1].ToString()) + ",";

                    #region DOB
                    string Date = "";
                    string[] datestr = dr[2].ToString().Split('/');
                    for (int i = 0; i < datestr.Length; i++)
                    {
                        if (i == datestr.Length - 1)
                        {
                            string[] _str = datestr[i].ToString().Split(' ');
                            Date += _str[0];
                        }
                        else
                            Date += datestr[i].ToString() + "/";
                    }
                    #endregion

                    Stu_details += CheckValueNotQualtoZero(Date) + ",";
                    int.TryParse(dr[3].ToString(), out _StudentId);
                    Stu_details += CheckValueNotQualtoZero(MyFeeManage.GetTotalFeeAmount_Student(_StudentId.ToString()).ToString())+ ",";

                    int Persentdays = 0;
                    if (MyAttendance.GetCurrentBatchNewattendanceDetails(_StudentId, out  _no_workingdays, out  _no_fulldays, out  _no_absentdays, out  _no_holidays, out  _no_halfdays, out  _attendencepersent, _batchid))
                    {
                        Persentdays = _no_fulldays + (_no_halfdays / 2);
                        Stu_details +=CheckValueNotQualtoZero(_no_workingdays.ToString()) + "," + CheckValueNotQualtoZero(Persentdays.ToString()) + ",";
                    }
                    else
                        Stu_details += "Null,Null,";

                    int _avground = 0;
                    if (!CheckExamConfiguration())
                    {
                        if (GetStudentIvrExamResults(_StudentId, _batchid, out _Examname, out _Avg, out _Grad))
                        {
                            _Avg = Math.Round(_Avg, 0);
                            _avground = Convert.ToInt32(_Avg);
                            Stu_details += CheckValueNotQualtoZero(_Examname) + "," + CheckValueNotQualtoZero(_avground.ToString()) + "," + CheckValueNotQualtoZero(_Grad);
                        }
                        else
                            Stu_details += "Null,Null,Null";
                    }
                    else
                        Stu_details += "Null,Null,Null";

                    return_studentDetails[Index] = Stu_details;
                    Index++;
                }
            }
            else
            {
                valid = false;
                ErrorMessage = "Unregistered number";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Error in commission!";
            valid = false;

        }
        return valid;
    }

    private bool CheckExamConfiguration()
    {
        bool valid = false;
        string sql = "SELECT tblconfiguration.value  from tblconfiguration where tblconfiguration.Module='Exam' AND tblconfiguration.Name='Exam_Type'";
        OdbcDataReader MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                if (MyReader.GetValue(0).ToString() == "1")
                    valid = true;
            }
        }
        return valid;
    }

    private string CheckValueNotQualtoZero(string Value)
    {
        if (Value == "" || Value == "0")
            Value = "Null";
        return Value;
    }

    private bool GetStudentIvrExamResults(int _StudentId, int _batchid, out string _Examname, out double _Avg, out string _Grad)
    {
        bool valid = true;
        _Examname = "";
        _Avg = 0.0;
        _Grad = "";

        try
        {
            string sql = "SELECT DISTINCT tblexammaster.ExamName,tblstudentmark.Avg,tblstudentmark.Grade,tblstudentmark.Rank from tblexammaster inner JOIN tblclassexam on tblclassexam.ExamId=tblexammaster.Id inner JOIN tblexamschedule on tblexamschedule.ClassExamId=tblclassexam.Id inner JOIN tblstudentclassmap on tblstudentclassmap.ClassId=tblclassexam.ClassId inner join tblstudentmark on tblstudentmark.ExamSchId=tblexamschedule.Id where tblexamschedule.Status='Completed' AND tblexamschedule.BatchId=" + _batchid + " and tblstudentclassmap.StudentId=" + _StudentId + " AND tblstudentmark.StudId=" + _StudentId;
            DataSet ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _Examname = dr[0].ToString();
                    if (dr[1].ToString() != "")
                        double.TryParse(dr[1].ToString(), out _Avg);
                    _Grad = dr[2].ToString();

                }
            }
            else
                valid = false;
        }
        catch (Exception ex)
        {
            valid = false;
        }
        return valid;
    }

    private int getBatchId()
    {
        int ID = 0;
        string sql = "SELECT tblbatch.Id from tblbatch where tblbatch.Status=1";
        DataSet ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0][0].ToString() != "")
                int.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ID);
        }
        return ID;
    }

    #endregion

    
}





  
                