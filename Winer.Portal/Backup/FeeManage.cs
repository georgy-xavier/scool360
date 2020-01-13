using System;
using System.Web;
//using System.Web.Services;
//using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Data.Odbc;
using System.Text;
using System.Data;
using WinBase;

public class FeeManage : KnowinGen
{
    public MysqlClass m_MysqlDb;
    public MysqlClass m_TransationDb = null;
    public DBLogClass m_DbLog = null;
    private OdbcDataReader m_MyReader = null;
    private OdbcDataReader m_MyReader1 = null;
    //private OdbcDataReader m_MyReader2 = null;
    //private OdbcDataReader m_MyReader3 = null;
    //private OdbcDataReader m_MyReader5 = null;
    private string m_FeeMenuStr;
    private string m_FeeSubMenuStr;
    private int m_temp_feeid = 0;
    private int m_temp_ClassId = 0;
    private int m_temp_PeriodId = 0;
    private int m_temp_Batchid = 0;
    private int m_temp_feeSchid = 0;
    private string m_BillPrefix;
    private string m_AdvanceAutoCancel="-1";
    private DataSet MyDataSet = null;
    
    public FeeManage(KnowinGen _Prntobj)
    {
        m_Parent = _Prntobj;
        m_MyODBCConn = m_Parent.ODBCconnection;
        m_UserName = m_Parent.LoginUserName;
        m_MysqlDb = new MysqlClass(this);
        m_DbLog = new DBLogClass(m_MysqlDb);
        m_userid = m_Parent.User_Id;
        m_FeeMenuStr = "";
        m_FeeSubMenuStr = "";
    }

    public FeeManage(MysqlClass _Msqlobj)
    {
        m_MysqlDb = _Msqlobj;
    }

    public FeeManage(MysqlClass _Msqlobj,OdbcConnection _odbcConnection)
    {
        m_MysqlDb = _Msqlobj;
        m_MyODBCConn = _odbcConnection;
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

    ~FeeManage()
    {
        if (m_MysqlDb != null)
        {
            m_MysqlDb = null;

        }
        if (m_TransationDb != null)
        {
            m_TransationDb = null;

        }
        if (m_MyReader != null)
        {
            m_MyReader = null;

        }

    }

   

    public string GetFeeMangMenuString(int _roleid)
    {
        string _MenuStr;

        if (m_FeeMenuStr == "")
        {

            _MenuStr = "<ul><li><a href=\"ManageFeeAccount.aspx\">Manage Fee</a></li>";
            string sql = "SELECT DISTINCT tblaction.MenuName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND tblroleactionmap.ModuleId=1 AND tblaction.ActionType='Link' order by tblaction.`Order` ";
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
            m_FeeMenuStr = _MenuStr;

        }
        else
        {
            _MenuStr = m_FeeMenuStr;
        }
        return _MenuStr;

    }



    public string GetSubFeeMangMenuString(int _roleid, int _feeid)
    {
        string _MenuStr;
        int associatedId = GetFeeSchduleType(_feeid);

        // if (m_FeeSubMenuStr == "")
        //  {
        if (associatedId == 1)
        {
            _MenuStr = "<ul class=\"block\"><li><a href=\"FeeDetails.aspx\">Fee Details</a></li>";
            string sql = "SELECT DISTINCT tblaction.MenuName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND tblroleactionmap.ModuleId=1 AND tblaction.ActionType='SubLink' or tblaction.ActionType='AssoSubLink' order by tblaction.`Order` ";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }
        else
        {
            _MenuStr = "<ul class=\"block\"><li><a href=\"FeeDetails.aspx\">Fee Details</a></li>";
            string sql = "SELECT DISTINCT tblaction.ActionName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND tblroleactionmap.ModuleId=1 AND tblaction.ActionType='SubLink' order by tblaction.`Order` ";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }


        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                _MenuStr = _MenuStr + "<li><a href=\"" + m_MyReader.GetValue(1).ToString() + "\">" + m_MyReader.GetValue(0).ToString() + "</a></li>";
            }
        }

        _MenuStr = _MenuStr + "</ul>";
        m_MyReader.Close();
        m_FeeSubMenuStr = _MenuStr;


        //}
        // else
        //{
        _MenuStr = m_FeeSubMenuStr;
        //}
        return _MenuStr;

    }


    public int GenBill(double _Total, int StudentId)
    {
        int _Id = 0;
        if (m_TransationDb != null)
        {

            string sql = "insert into tblfeebill (StudentID,TotalAmount,Date) values(" + StudentId + "," + _Total + ",'" + System.DateTime.Now.ToString("s") + "')";
            m_TransationDb.TransExecuteQuery(sql);
            sql = "select Id from tblfeebill where StudentID=" + StudentId + " and TotalAmount=" + _Total + " and Date='" + System.DateTime.Now.ToString("s") + "'";
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                m_MyReader.Read();
                _Id = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            else
            {
                _Id = 0;
            }
        }
        else
        {
            _Id = 0;
        }
        return _Id;

    }

    //kiran new
    private bool IsStudentFeeSchduled(int _studid, int _SchId)
    {

        bool _valid;
        string sql = "SELECT tblfeestudent.Amount,tblfeestudent.Status from tblfeestudent WHERE tblfeestudent.StudId=" + _studid + " AND tblfeestudent.SchId=" + _SchId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _valid = true;
        }
        else
        {


            _valid = false;
        }
        m_MyReader.Close();
        return _valid;
    }
    public bool IsValidFeeName(string _feename)
    {
        bool _valid;

        string sql = "select Id from tblfeeaccount where Status=1 AND AccountName='" + _feename + "'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _valid = false;
        }
        else
        {

            _valid = true;
        }

        m_MyReader.Close();
        return _valid;

    }



    public void CreateFee(string _feename, string _disc, int _Freqid, int _Assoid, int _Userid , int _FeeType)
    {
        General myGenObj = new General(m_MysqlDb);
        int _MaxID = myGenObj.GetTableMaxId("tblfeeaccount", "Id");
        if (_MaxID == 0)
            _MaxID = 1;
        DateTime _dtNow = System.DateTime.Now;
        string sql = "INSERT INTO tblfeeaccount(Id,AccountName,Desciptrion,FrequencyId,AssociatedId,CreatedUserId,CreatedTime,Status,`Type`) VALUES(" + _MaxID + ",'" + _feename + "','" + _disc + "'," + _Freqid + "," + _Assoid + "," + _Userid + ",'" + _dtNow.ToString("s") + "',1," + _FeeType + ")";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        m_MyReader.Close();
    }

    public bool HaveStudentInClass(int _ClassId, int _Batchid)
    {
        bool _valid;
        string sql = "SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap inner join tblstudent on tblstudentclassmap.StudentId=tblstudent.Id where tblstudent.Status=1  AND tblstudentclassmap.BatchId=" + _Batchid + " and tblstudentclassmap.ClassId=" + _ClassId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _valid = true;
        }
        else
        {

            _valid = false;
        }
        m_MyReader.Close();
        return _valid;
    }

    public bool ScheduleClassFee(int _classid, int _Periodid, DateTime _duedate, DateTime _lastdate, double _amount, int _BatchId, int _FeeId,int _CurrentBatchId)
    {
        bool _valid = false;
        CLogging logger = CLogging.GetLogObject();
        logger.LogToFile("ScheduleClassFee", "Request to schdule fee", 'I', CLogging.PriorityEnum.LEVEL_IMPORTANT, LoginUserName);
       
        int _schid;
        try
        {
            CreateTansationDb();
            string sql = "INSERT INTO tblfeeschedule(PeriodId,ClassId,FeeId,BatchId,Duedate,LastDate,Amount) VALUES(" + _Periodid + "," + _classid + "," + _FeeId + "," + _BatchId + ",'" + _duedate.Date.ToString("s") + "','" + _lastdate.Date.ToString("s") + "'," + _amount + " )";
            logger.LogToFile("ScheduleClassFee", "Executing Query" + sql, 'I', CLogging.PriorityEnum.LEVEL_IMPORTANT, LoginUserName);
            m_TransationDb.TransExecuteQuery(sql);
            sql = "select Id from tblfeeschedule where FeeId=" + _FeeId + " and ClassId=" + _classid + " and PeriodId=" + _Periodid + " and BatchId=" + _BatchId;
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
            m_MyReader.Read();
            _schid = int.Parse(m_MyReader.GetValue(0).ToString());
            logger.LogToFile("ScheduleClassFee", "Returning SchId" + _schid.ToString(), 'I', CLogging.PriorityEnum.LEVEL_IMPORTANT, LoginUserName);
            sql = "SELECT tblstudentclassmap.StudentId from tblstudentclassmap inner JOIN tblstudent on tblstudent.Id=tblstudentclassmap.StudentId where tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + _CurrentBatchId + " AND tblstudentclassmap.ClassId=" + _classid;

            m_MyReader = m_TransationDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    ScheduleStudFee(int.Parse(m_MyReader.GetValue(0).ToString()), _schid, _amount, "Scheduled");
                    logger.LogToFile("ScheduleClassFee", "Schduling Fee To Student of class" + sql, 'I', CLogging.PriorityEnum.LEVEL_IMPORTANT, LoginUserName);
                    m_TransationDb.TransExecuteQuery(sql);
                }
            }
            // m_MyReader.Close();
            EndSucessTansationDb();
            _valid = true;
        }
        catch (Exception exc)
        {
            logger.LogToFile("ScheduleClassFee", "Error in Schduling" + exc.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
            EndFailTansationDb();

        }
        return _valid;

    }


    public bool HaveStudWithOutRollNoInClass(int _ClassId, int _Batchid)
    {
        bool _valid;
        string sql = "SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap inner join tblstudent on tblstudentclassmap.StudentId=tblstudent.Id where tblstudent.Status=1  AND tblstudentclassmap.RollNo=-1 AND tblstudentclassmap.BatchId=" + _Batchid + " and tblstudentclassmap.ClassId=" + _ClassId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _valid = true;
        }
        else
        {

            _valid = false;
        }
        m_MyReader.Close();
        return _valid;
    }

    public int ScheduleStudFee(int _classId, int _PeriodId, DateTime _duedate, DateTime _lastdate, int _BatchId, int _FeeId, double _amount)
    {

        int _schid = -1; ;
        if (m_TransationDb != null)
        {
            string sql = "INSERT INTO tblfeeschedule(ClassId,PeriodId,FeeId,BatchId,Duedate,LastDate,Amount) VALUES(" + _classId + "," + _PeriodId + "," + _FeeId + "," + _BatchId + ",'" + _duedate.Date.ToString("s") + "','" + _lastdate.Date.ToString("s") + "' ," + _amount + ")";
            m_TransationDb.TransExecuteQuery(sql);
            sql = "select Id from tblfeeschedule where FeeId=" + _FeeId + " and ClassId=" + _classId + " and PeriodId=" + _PeriodId + " and BatchId=" + _BatchId;
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                m_MyReader.Read();
                _schid = int.Parse(m_MyReader.GetValue(0).ToString());

            }

        }
        return _schid;
    }
    public void ScheduleStudFeeAmount(double _amount, int _studid, int _SchId)
    {
        string sql = "INSERT INTO tblfeestudent(SchId,StudId,Amount,BalanceAmount,Status) VALUES(" + _SchId + "," + _studid + "," + _amount + "," + _amount + ",'Scheduled')";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);

        m_MyReader.Close();
    }
    public bool UpdateStudFeeSchedule(DateTime _duedate, DateTime _lastdate, int _Schid)
    {
        bool _valide;
        try
        {
            string sql = "UPDATE tblfeeschedule SET Duedate= '" + _duedate.Date.ToString("s") + "', LastDate = '" + _lastdate.Date.ToString("s") + "' WHERE Id =" + _Schid;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            m_MyReader.Close();
            _valide = true;
        }
        catch
        {
            _valide = false;
        }
        return _valide;
    }
    public void UpdateScheduleStudFeeAmount(double _amount, int _studid, int _SchId)
    {

        if (IsStudentFeeSchduled(_studid, _SchId))
        {
            string sql = "UPDATE tblfeestudent SET Amount= " + _amount + ", BalanceAmount = " + _amount + " WHERE StudId=" + _studid + " AND SchId =" + _SchId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);

            m_MyReader.Close();
        }
        else
        {
            ScheduleStudFeeAmount(_amount, _studid, _SchId);
        }



    }

    public void DeleteScheduleStudFeeAmount(int _studid, int _SchId)
    {
        string sql;
        sql = "Delete From tblfeestudent WHERE StudId=" + _studid + " AND SchId =" + _SchId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        sql = "SELECT tblfeestudent.SchId from tblfeestudent WHERE tblfeestudent.SchId=" + _SchId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (!m_MyReader.HasRows)
        {
            sql = "Delete From tblfeeschedule WHERE Id=" + _SchId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql); ;
        }

    }
    public bool AnyOnePaid(string _feeSchId)
    {
        bool _valid;
        string sql = "SELECT tblfeestudent.Amount from tblfeestudent WHERE tblfeestudent.SchId=" + _feeSchId + " AND  NOT tblfeestudent.Status='Scheduled'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _valid = true;
        }
        else
        {


            _valid = false;
        }
        m_MyReader.Close();
        return _valid;
    }
    public bool HaveNewStudWithOutFeeSchduled(int _schid, int _ClassId, int _batchId)
    {
        bool _valid;
        string sql = "SELECT tblstudentclassmap.StudentId from tblstudentclassmap inner JOIN tblstudent on tblstudentclassmap.StudentId=tblstudent.Id where tblstudentclassmap.BatchId=" + _batchId + " AND tblstudentclassmap.ClassId=" + _ClassId + " AND tblstudent.Status=1 AND tblstudentclassmap.StudentId NOT IN (Select tblfeestudent.StudId from tblfeestudent where tblfeestudent.SchId=" + _schid + ")";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);

        if (m_MyReader.HasRows)
        {
            _valid = true;
        }
        else
        {


            _valid = false;
        }
        m_MyReader.Close();
        return _valid;
    }
    public bool UpdateClassFeeSehedule(DateTime _duedate, DateTime _lastdate, double _amount, int _batchId, int _schId, int _amount_flag, int _feeId, int _ClassId, int _periodId)
    {
        bool _Vaild;
        CLogging logger = CLogging.GetLogObject();
        DataSet _ruleset;
        double _studamt;
        try
        {
            CreateTansationDb();
            if (_amount_flag == 1)
            {



                logger.LogToFile("UpdateClassFeeSehedule", "Request to schdule fee", 'I', CLogging.PriorityEnum.LEVEL_IMPORTANT, LoginUserName);
                string sql = "UPDATE tblfeeschedule SET Duedate= '" + _duedate.Date.ToString("s") + "', LastDate = '" + _lastdate.Date.ToString("s") + "', PeriodId=" + _periodId + " WHERE Id =" + _schId;
                m_TransationDb.TransExecuteQuery(sql);
                logger.LogToFile("UpdateClassFeeSehedule", "Fee Date Updated" + sql, 'I', CLogging.PriorityEnum.LEVEL_IMPORTANT, LoginUserName);
                sql = "select tblrules.Amounttype, tblrules.Amount , tblrules.AssigMode , tblrules.FieldValue,tblruleitem.tblname , tblruleitem.Colname , tblruleitem.fieldType from tblruleclassmap inner join tblrules on tblruleclassmap.RuleId = tblrules.Id  inner join tblruleitem on tblrules.tblruleitemId = tblruleitem.Id where tblruleclassmap.classId=" + _ClassId + " and tblruleclassmap.feeTypeId=" + _feeId;
                _ruleset = m_TransationDb.ExecuteQueryReturnDataSet(sql);
                if (_ruleset.Tables[0].Rows.Count > 0)
                {

                    sql = "SELECT tblfeestudent.StudId from tblfeestudent where tblfeestudent.SchId=" + _schId + " AND tblfeestudent.Status='Scheduled'";
                    m_MyReader = m_TransationDb.ExecuteQuery(sql);
                    if (m_MyReader.HasRows)
                    {
                        while (m_MyReader.Read())
                        {
                            _studamt = GetStudCalcuAmt(_ruleset, int.Parse(m_MyReader.GetValue(0).ToString()), _amount);
                            sql = "UPDATE tblfeestudent SET Amount= " + _studamt + ", BalanceAmount = " + _studamt + " WHERE SchId =" + _schId + " and tblfeestudent.StudId=" + int.Parse(m_MyReader.GetValue(0).ToString());
                            logger.LogToFile("UpdateClassFeeSehedule", "Fee Amound for the Student Updated" + sql, 'I', CLogging.PriorityEnum.LEVEL_IMPORTANT, LoginUserName);
                            m_TransationDb.TransExecuteQuery(sql);
                        }
                    }
                    sql = "UPDATE tblfeeschedule SET Amount= " + _amount + " WHERE Id =" + _schId + " ";
                    m_TransationDb.TransExecuteQuery(sql);
                    _Vaild = true;
                    // m_MyReader.Close();
                    // MysqlTranDb.TransactionCommit();
                }
                else
                {
                    sql = "SELECT tblfeestudent.StudId from tblfeestudent where tblfeestudent.SchId=" + _schId + " AND tblfeestudent.Status='Scheduled'";
                    m_MyReader = m_TransationDb.ExecuteQuery(sql);
                    if (m_MyReader.HasRows)
                    {
                        while (m_MyReader.Read())
                        {
                            sql = "UPDATE tblfeestudent SET Amount= " + _amount + ", BalanceAmount = " + _amount + " WHERE SchId =" + _schId;
                            logger.LogToFile("UpdateClassFeeSehedule", "Fee Amound for the Student Updated" + sql, 'I', CLogging.PriorityEnum.LEVEL_IMPORTANT, LoginUserName);
                            m_TransationDb.TransExecuteQuery(sql);
                        }
                    }
                    sql = "UPDATE tblfeeschedule SET Amount= " + _amount + " WHERE Id =" + _schId + " ";
                    m_TransationDb.TransExecuteQuery(sql);
                    _Vaild = true;
                }

            }
            else
            {
                string sql = "UPDATE tblfeeschedule SET Duedate= '" + _duedate.Date.ToString("s") + "', LastDate = '" + _lastdate.Date.ToString("s") + "', PeriodId=" + _periodId + " WHERE Id =" + _schId;
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
                logger.LogToFile("UpdateClassFeeSehedule", "Fee Date Updated" + sql, 'I', CLogging.PriorityEnum.LEVEL_IMPORTANT, LoginUserName);
                _Vaild = true;
            }
        }
        catch
        {
            EndFailTansationDb();
            _Vaild = false;
        }
        if (_Vaild == true)
        {
            EndSucessTansationDb();
        }
        m_MyReader.Close();
        return _Vaild;
    }

    public string GenBill(double _Total, int StudentId, string _PayMode, string _payId, string _bank, string _Date, int _UserId,int _BatchId , string _ClassId , string _StudentName,int _Regular , string _TempId,string _OtherReferance,string _BillTable)
    {
        string _billno = "0";
        if (m_BillPrefix == null)
        {
            m_BillPrefix = GetBillPrefix();
        }
        if (m_TransationDb != null)
        {
            General _GenObj = new General(m_TransationDb);
            int _FeeMaxId = _GenObj.GetTableMaxIdWithCondition("tblview_feebill", "Counter", _BatchId, "BatchId");
            int _FeeId = _GenObj.GetTableMaxId("tblview_feebill", "TransationId");
            if (_BillTable == "tblfeebillclearence")
              _FeeMaxId=  _FeeId = _GenObj.GetTableMaxId("tblfeebillclearence", "Id");
            DateTime _BillDate = General.GetDateTimeFromText(_Date);
            DateTime _CreatedDate = System.DateTime.Now;
            string Year = GetStartYear(_BatchId);
            if (NeedOnlyNumber_InBill())
            {
                _billno = _FeeMaxId.ToString();
            }
            else
            {
                _billno = m_BillPrefix + Year.ToString() + "-" + _FeeMaxId.ToString();
            }
            if(_TempId!="")//Setting student id to zero for registered students
                StudentId=0;
            string sql = "insert into " + _BillTable + " (Id,StudentID,TotalAmount,Date,PaymentMode,PaymentModeId,BankName,UserId,CreatedDateTime,Counter,AccYear,ClassID,StudentName,BillNo,RegularFee,TempId,OtherReference) values(" + _FeeId + "," + StudentId + "," + _Total + ",'" + _BillDate.ToString("s") + "','" + _PayMode + "','" + _payId + "','" + _bank + "'," + _UserId + ",'" + _CreatedDate.ToString("s") + "'," + _FeeMaxId + "," + _BatchId + "," + _ClassId + ",'" + _StudentName + "','" + _billno + "'," + _Regular + ",'" + _TempId + "','" + _OtherReferance + "')";
            m_TransationDb.TransExecuteQuery(sql);
           
            //sql = "UPDATE tblfeebill SET BillNo= '" + _billno + "' WHERE Id =" + _FeeId;
            //m_TransationDb.TransExecuteQuery(sql);
        }

        return _billno;
    }

    private bool NeedOnlyNumber_InBill()
    {
        bool _valid=false;
        int _Prefix = 0;
        string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name = 'BillPrefix Only Numbers'";
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            int.TryParse(m_MyReader.GetValue(0).ToString(), out _Prefix);
            if (_Prefix == 1)
            {
                _valid = true;
            }
        }
        return _valid;
    }
    private string GetStartYear(int _BatchId)
    {
        OdbcDataReader m_MyReader = null;
        string Year = "2000";

        string sql = "select tblbatch.BatchName from tblbatch where Id=" + _BatchId;
        m_MyReader = m_TransationDb.SelectTansExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            string []Array = m_MyReader.GetValue(0).ToString().Split('-');
            Year = Array[0];
        }
        m_MyReader.Close();
        return Year;
    }


    private string GetBillPrefix()
    {
        string _Prefix = "";
        string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name = 'BillPrefix'";
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            m_MyReader.Read();
            _Prefix = m_MyReader.GetValue(0).ToString();
        }
        return _Prefix;
    }

    public void FillTransaction(int _SfeeId, int _StudId, double _Amount, double _Deduction, double _Fine, double Total, double _Arrier, string _BillId, string _ClassId, string _StudentName, string _UserName, int _BatchId, string _FeeName, string _Period, int _CollectionType, int _RegularFee, string _TempId, string _TransactionTable, int _PeriodId, int _FeeId, string _Date)
    {
        CLogging logger = CLogging.GetLogObject();
        logger.LogToFile("FillTransaction", "start Transation", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
        if (m_TransationDb != null)
        {
            if (_TempId != "")//Setting student id to zero for registered students
                _StudId = 0;
            DateTime _BillDate = General.GetDateTimeFromText(_Date);
            General _GenObj = new General(m_TransationDb);
            int _TranMaxId = _GenObj.GetTableMaxId("tblview_transaction", "TransationId");
            if (_TransactionTable == "tbltransactionclearence")
                _TranMaxId =  _GenObj.GetTableMaxId("tbltransactionclearence", "TransationId");
            string sql = "INSERT INTO " + _TransactionTable + "(TransationId,PaymentElementId,UserId,Amount,AccountTo,AccountFrom,Type,PaidDate,BillNo,ClassId,StudentName,CollectedUser,BatchId,FeeName,PeriodName,CollectionType,RegularFee,TempId,PeriodId,FeeId) VALUES(" + _TranMaxId + "," + _SfeeId + "," + _StudId + "," + Total + ",1 , 2 ,'C','" + _BillDate.Date.ToString("s") + "','" + _BillId + "'," + _ClassId + ",'" + _StudentName + "','" + _UserName + "'," + _BatchId + ",'" + _FeeName + "','" + _Period + "','" + _CollectionType + "'," + _RegularFee + ",'" + _TempId + "'," + _PeriodId + "," + _FeeId + ")";
            logger.LogToFile("FillTransaction", "Execute Query" + sql, 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
            m_TransationDb.TransExecuteQuery(sql);
            sql = "UPDATE " + _TransactionTable + " SET " + _TransactionTable + ".BalanceAmount=" + _Arrier + " where " + _TransactionTable + ".BillNo='" + _BillId + "' AND " + _TransactionTable + ".TransationId=" + _TranMaxId;
            m_TransationDb.TransExecuteQuery(sql);
            if (_SfeeId != -1)// -1 for other fee
            {
                if (_Deduction > 0)
                {
                    _TranMaxId = _GenObj.GetTableMaxId("tblview_transaction", "TransationId");
                    if (_TransactionTable == "tbltransactionclearence")
                        _TranMaxId = _GenObj.GetTableMaxId("tbltransactionclearence", "TransationId");
                    sql = "INSERT INTO " + _TransactionTable + "(TransationId,PaymentElementId,UserId,Amount,AccountTo,AccountFrom,Type,PaidDate,BillNo,ClassId,StudentName,CollectedUser,BatchId,FeeName,PeriodName,CollectionType,RegularFee,TempId,PeriodId,FeeId) VALUES(" + _TranMaxId + "," + _SfeeId + "," + _StudId + "," + _Deduction + ",3 , 2 ,'C','" + _BillDate.Date.ToString("s") + "','" + _BillId + "'," + _ClassId + ",'" + _StudentName + "','" + _UserName + "'," + _BatchId + ",'" + _FeeName + "','" + _Period + "','" + _CollectionType + "'," + _RegularFee + ",'" + _TempId + "'," + _PeriodId + "," + _FeeId + ")";
                    logger.LogToFile("FillTransaction", "Execute Query" + sql, 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
                    m_TransationDb.TransExecuteQuery(sql);
                    sql = "UPDATE " + _TransactionTable + " SET " + _TransactionTable + ".BalanceAmount=" + _Arrier + " where " + _TransactionTable + ".BillNo='" + _BillId + "' AND " + _TransactionTable + ".TransationId=" + _TranMaxId;
                    m_TransationDb.TransExecuteQuery(sql);
                }

                if (_Fine > 0)
                {
                    _TranMaxId = _GenObj.GetTableMaxId("tblview_transaction", "TransationId");
                    if (_TransactionTable == "tbltransactionclearence")
                        _TranMaxId = _GenObj.GetTableMaxId("tbltransactionclearence", "TransationId");
                    sql = "INSERT INTO " + _TransactionTable + "(TransationId,PaymentElementId,UserId,Amount,AccountTo,AccountFrom,Type,PaidDate,BillNo,ClassId,StudentName,CollectedUser,BatchId,FeeName,PeriodName,CollectionType,RegularFee,TempId,PeriodId,FeeId) VALUES(" + _TranMaxId + "," + _SfeeId + "," + _StudId + "," + _Fine + ",4 , 2 ,'C','" + _BillDate.Date.ToString("s") + "','" + _BillId + "'," + _ClassId + ",'" + _StudentName + "','" + _UserName + "'," + _BatchId + ",'" + _FeeName + "','" + _Period + "','" + _CollectionType + "'," + _RegularFee + ",'" + _TempId + "'," + _PeriodId + "," + _FeeId + ")";
                    logger.LogToFile("FillTransaction", "Execute Query" + sql, 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
                    m_TransationDb.TransExecuteQuery(sql);
                    sql = "UPDATE " + _TransactionTable + " SET " + _TransactionTable + ".BalanceAmount=" + _Arrier + " where " + _TransactionTable + ".BillNo='" + _BillId + "' AND " + _TransactionTable + ".TransationId=" + _TranMaxId;
                    m_TransationDb.TransExecuteQuery(sql);
                }

                if (_Arrier > 0)
                {
                    sql = "UPDATE tblfeestudent SET BalanceAmount = " + _Arrier + ", Status='Arrear' WHERE StudId=" + _StudId + " and SchId =" + _SfeeId;
                    logger.LogToFile("FillTransaction", "Execute Query" + sql, 'I', CLogging.PriorityEnum.LEVEL_IMPORTANT, LoginUserName);
                    m_TransationDb.TransExecuteQuery(sql);
                    sql = "UPDATE " + _TransactionTable + " SET " + _TransactionTable + ".BalanceAmount=" + _Arrier + " where " + _TransactionTable + ".BillNo='" + _BillId + "' AND " + _TransactionTable + ".TransationId=" + _TranMaxId;
                    m_TransationDb.TransExecuteQuery(sql);
                }
                else
                {
                    sql = "UPDATE tblfeestudent SET BalanceAmount = " + _Arrier + ", Status='Paid' WHERE StudId=" + _StudId + " and SchId =" + _SfeeId;
                    logger.LogToFile("FillTransaction", "Execute Query" + sql, 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
                    m_TransationDb.TransExecuteQuery(sql);
                }
            }

            if (_CollectionType == 3 && _TransactionTable == "tbltransaction") // If Advance payment then 
            {
                SaveAdvanceEntry(_StudId, _TempId, _PeriodId, _FeeId, _BatchId, _FeeName, _Period, _Amount, _StudentName, _BillId);
                             
            }
        }

    }

    public void SaveAdvanceEntry(int _StudId, string _TempId, int _PeriodId, int _FeeId, int _BatchId, string _FeeName, string _Period, double _Amount, string _StudentName, string _BillId)
    {
        string sql = "";
        int _AdviD = 0;
        if (_StudId == 0)
        {
            if (!ISAdvanceAlreadyPresent(_TempId, _PeriodId, _FeeId, _BatchId, out _AdviD))
            {
                sql = "insert into tblstudentfeeadvance(StudentId,FeeName,PeriodName,BatchId,Amount,StudentName,FeeId,PeriodId,TempId) values (" + _StudId + ",'" + _FeeName + "','" + _Period + "'," + _BatchId + "," + _Amount + ",'" + _StudentName + "'," + _FeeId + "," + _PeriodId + ",'" + _TempId + "')";
                if (m_TransationDb != null)
                {
                    m_TransationDb.TransExecuteQuery(sql);
                }
                else
                {
                    m_MysqlDb.TransExecuteQuery(sql);
                }
            }
            else
            {
                sql = "UPDATE tblstudentfeeadvance SET Amount=Amount+" + _Amount + " WHERE Id=" + _AdviD;
                if (m_TransationDb != null)
                {
                    m_TransationDb.TransExecuteQuery(sql);
                }
                else
                {
                    m_MysqlDb.TransExecuteQuery(sql);
                }
            }
        }
        else
        {
            if (!ISAdvanceAlreadyPresent(_StudId, _PeriodId, _FeeId, _BatchId, out _AdviD))
            {
                sql = "insert into tblstudentfeeadvance(StudentId,FeeName,PeriodName,BatchId,Amount,StudentName,FeeId,PeriodId,TempId) values (" + _StudId + ",'" + _FeeName + "','" + _Period + "'," + _BatchId + "," + _Amount + ",'" + _StudentName + "'," + _FeeId + "," + _PeriodId + ",'" + _TempId + "')";
                if (m_TransationDb != null)
                {
                    m_TransationDb.TransExecuteQuery(sql);
                }
                else
                {
                    m_MysqlDb.TransExecuteQuery(sql);
                }
            }
            else
            {
                sql = "UPDATE tblstudentfeeadvance SET Amount=Amount+" + _Amount + " WHERE Id=" + _AdviD;
                if (m_TransationDb != null)
                {
                    m_TransationDb.TransExecuteQuery(sql);
                }
                else
                {
                    m_MysqlDb.TransExecuteQuery(sql);
                }
            }
        }
        double _TotalAdv = GetTotalAdvanceBalance();
        InserAdvanceTransaction(_StudId, _StudentName, _FeeName, _Period, _BatchId, _Amount, _FeeId, _PeriodId, _TempId, 1, _BillId, _TotalAdv + _Amount);

    }

    private bool ISAdvanceAlreadyPresent(string _TempId, int _PeriodId, int _FeeId, int _BatchId, out int _AdviD)
    {
        bool _valid = false;
        string sql;
        _AdviD = 0;
        sql = "select tblstudentfeeadvance.Id from tblstudentfeeadvance where tblstudentfeeadvance.FeeId=" + _FeeId + " AND tblstudentfeeadvance.BatchId=" + _BatchId + " AND tblstudentfeeadvance.PeriodId=" + _PeriodId + " AND tblstudentfeeadvance.TempId='" + _TempId+"'";
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
            _AdviD = int.Parse(m_MyReader.GetValue(0).ToString());
            _valid = true;
        }

        return _valid;
    }
  
    private bool ISAdvanceAlreadyPresent(int _StudId, int _PeriodId, int _FeeId, int _BatchId, out int _AdviD)
    {
        bool _valid = false;
        string sql;
        OdbcDataReader m_MyReader1;
        _AdviD = 0;
        sql = "select tblstudentfeeadvance.Id from tblstudentfeeadvance where tblstudentfeeadvance.FeeId=" + _FeeId + " AND tblstudentfeeadvance.BatchId=" + _BatchId + " AND tblstudentfeeadvance.PeriodId=" + _PeriodId + " AND tblstudentfeeadvance.StudentId=" + _StudId;
        if (m_TransationDb != null)
        {
            m_MyReader1 = m_TransationDb.ExecuteQuery(sql);
        }
        else
        {
            m_MyReader1 = m_MysqlDb.ExecuteQuery(sql);
        }
        if (m_MyReader1.HasRows)
        {
            _AdviD = int.Parse(m_MyReader1.GetValue(0).ToString());
            _valid = true;
        }

        return _valid;
    }
    public bool RemoveFeeAccount(int _FeeID, int _batchid,out string _msg)
    {
        bool _valid = false;
       
        _msg = "";
        if (IsDeletionPossible(_FeeID, _batchid, out _msg))
        {
            _valid = true;
        }
        
        return _valid;
    }

    private bool IsDeletionPossible(int _FeeID, int _batchid, out string _msg)
    {
        _msg = "";
        bool _valid = true;
        try
        {
            CreateTansationDb();
            if (IsFeeScheduledInBatch(_FeeID, _batchid))
            {
                if (IscollectionDone(_FeeID, _batchid))
                {
                    _msg = "Fee that is scheduled, has already been paid by some students. You cannot remove this fees untill the collected fees has been cancelled";
                    _valid = false;
                }
                else
                {
                    if (IsCancelledFeesCollectionArePresent(_FeeID, _batchid))
                    {
                        DeleteFeeStudentEntries(_FeeID, _batchid);
                        UpdateFees(_FeeID);

                    }
                    else
                    {
                        DeleteFeeRuleMap(_FeeID);
                        DeleteFeeStudentEntries(_FeeID, _batchid);
                        DeleteFeeSchedule(_FeeID, _batchid);
                        DeleteFee(_FeeID);
                    }

                }

            }
            else if (IsFeeScheduled(_FeeID))
            {
                UpdateFees(_FeeID);
            }
            else
            {

                DeleteFee(_FeeID);
            }
            EndSucessTansationDb();
        }
        catch (Exception Ex)
        {
            EndFailTansationDb();
            _msg = "Error while deleting. Message : " + Ex.Message;
            _valid = false;
        }

        return _valid;
    }

    private void DeleteFeeRuleMap(int _FeeID)
    {
        string sql = "DELETE FROM tblruleclassmap WHERE feeTypeId=" + _FeeID;
        m_TransationDb.ExecuteQuery(sql);
    }

    private void DeleteFeeSchedule(int _FeeID, int _batchid)
    {
        string sql = "DELETE FROM tblfeeschedule WHERE tblfeeschedule.FeeId=" + _FeeID + " AND tblfeeschedule.BatchId=" + _batchid;
        m_TransationDb.ExecuteQuery(sql);
    }

    private void DeleteFeeStudentEntries(int _FeeID, int _batchid)
    {
        string sql = "DELETE FROM tblfeestudent WHERE tblfeestudent.SchId IN (SELECT tblfeeschedule.Id FROM tblfeeschedule WHERE tblfeeschedule.FeeId="+_FeeID+" AND tblfeeschedule.BatchId="+_batchid+")";
        m_TransationDb.ExecuteQuery(sql);
    }

    private bool IsCancelledFeesCollectionArePresent(int _FeeID, int _batchid)
    {
        bool _valid = false;
        string sql = "SELECT COUNT(tblview_transaction.TransationId) FROM tblview_transaction WHERE tblview_transaction.Canceled=1 AND tblview_transaction.FeeId=" + _FeeID + " AND tblview_transaction.BatchId=" + _batchid;
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int _count = 0;
            int.TryParse(m_MyReader.GetValue(0).ToString(), out _count);
            if (_count > 0)
            {
                _valid = true;
            }
        }
        return _valid;
    }

    private bool IscollectionDone(int _FeeID, int _batchid)
    {
        bool _valid = false;
        string sql = "SELECT COUNT(tblview_transaction.TransationId) FROM tblview_transaction WHERE tblview_transaction.Canceled=0 AND tblview_transaction.FeeId=" + _FeeID + " AND tblview_transaction.BatchId=" + _batchid;
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int _count = 0;
            int.TryParse(m_MyReader.GetValue(0).ToString(), out _count);
            if (_count > 0)
            {
                _valid = true;
            }
        }
        return _valid;
    }

    private bool IsFeeScheduledInBatch(int _FeeID, int _batchid)
    {
        bool _valid = false;
        string sql;
        sql = "select COUNT(tblfeeschedule.Id) from tblfeeschedule where tblfeeschedule.FeeId=" + _FeeID + " AND tblfeeschedule.BatchId=" + _batchid;
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int _count = 0;
            int.TryParse(m_MyReader.GetValue(0).ToString(), out _count);
            if (_count > 0)
            {
                _valid = true;
            }
        }
        return _valid;
    }

    private bool IsFeeScheduled(int _FeeID)
    {
        bool _valid = false;
        string sql;
        sql = "select COUNT(tblfeeschedule.Id) from tblfeeschedule where tblfeeschedule.FeeId=" + _FeeID ;
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int _count = 0;
            int.TryParse(m_MyReader.GetValue(0).ToString(), out _count);
            if (_count > 0)
            {
                _valid = true;
            }
        }
        return _valid;
    }

    private void DeleteFee(int _FeeID)
    {
        string sql;
        sql = "Delete From tblfeeaccount where Id=" + _FeeID;
        m_TransationDb.ExecuteQuery(sql);


    }

    private void UpdateFees(int _FeeID)
    {
        string sql = "UPDATE tblfeeaccount SET Status=0 WHERE Id=" + _FeeID;
        m_TransationDb.ExecuteQuery(sql);
        m_MyReader.Close();
    }


    public bool BillExists(string BillId)
    {
        bool _HasBill = false;
        string sql = "SELECT TransationId FROM tbltransaction WHERE BillNo='" + BillId + "'";
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _HasBill = true;
        }
        return _HasBill;
    }
    public bool PrinterTypeisDesk(out string _BillType)
    {
        _BillType = "";
        bool DeskJet = false;
        string printer = "";
        string sql = "SELECT Value,SubValue FROM tblconfiguration WHERE Name='Printer'";
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
            printer = m_MyReader.GetValue(0).ToString();
            if (printer == "Desk Jet")
            {
                DeskJet = true;
            }
            else
            {
                DeskJet = false;
            }
            _BillType = m_MyReader.GetValue(1).ToString();
        }
        return DeskJet;
    }

    public bool ValidTransaction(int ScheduledFeeId, int StudentId, double Amount)
    {
        bool valid = false;
        CLogging logger = CLogging.GetLogObject();
        logger.LogToFile("ValidTransaction", "start validation", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
        if (m_TransationDb != null)
        {
            string sql = "select Id from tblfeestudent where SchId=" + ScheduledFeeId + " and StudId=" + StudentId + " and BalanceAmount=" + Amount;
            logger.LogToFile("ValidTransaction", "Execute Query" + sql, 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                valid = true;
            }
        }
        return valid;
    }

    public bool IsFeeScheduled(int _Class, int _Perod1, int BatchId, int FeeId)
    {
        bool FeeSchd = false;
        string sql = "SELECT tblfeeschedule.Id FROM tblfeeschedule WHERE tblfeeschedule.ClassId=" + _Class + " AND tblfeeschedule.PeriodId=" + _Perod1 + " AND tblfeeschedule.BatchId=" + BatchId + " AND tblfeeschedule.FeeId=" + FeeId + "";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            FeeSchd = true;
        }
        return FeeSchd;
    }

    public string GetFeeScheduleDetailsTableString(int _classId, int _Feeid, int _CurrentBatchid,int _BatchId)
    {

        StringBuilder _Table = new StringBuilder("<table class=\"style1\"><tr class=\"tablehead\"><td >Period</td><td>Status</td><td>Avg Amount</td> <td>No of Stud</td><td>No of Stud<br /> Fee scheduled</td><td> Due Date</td><td> Last Date</td><td> Total Amount</td><td>No of UnPaid<br />Students</td> <td>Balance<br /> Amount</td></tr>");
        DataSet _MydataSet;
        string _Rowcolor;
        string _image;

        double amount;
        int _feetype;
        int n_student;
        int fee_n_student;
        string due_dt;
        string last_dt;
        double _total;
        int n_stud_Unpaid;
        //double ttl_dudition;
        //double ttl_fine;
        //double coll_amount;
        double balance_amount;
        int _schid;

        string sql = "select tblperiod.Id, tblperiod.Period from tblfeeaccount inner join tblperiod on tblfeeaccount.FrequencyId = tblperiod.FrequencyId  where tblfeeaccount.Id=" + _Feeid.ToString();
        _MydataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (_MydataSet != null && _MydataSet.Tables != null && _MydataSet.Tables[0].Rows.Count > 0)
        {
            n_student = GetNoofstudinclass(_classId, _CurrentBatchid);

            foreach (DataRow dr in _MydataSet.Tables[0].Rows)
            {
                if (IsFeeScheduled(_classId, int.Parse(dr[0].ToString()), _BatchId, _Feeid))
                {

                    _schid = GetscheId(_classId, int.Parse(dr[0].ToString()), _BatchId, _Feeid);
                    if (HaveNewStudWithOutFeeSchduled(_schid, _classId, _CurrentBatchid))
                    {

                        _image = "warning.png";
                    }
                    else
                    {

                        _image = "accept.png";

                    }
                    _feetype = GetFeeSchduleType(_Feeid);
                    GetFeescheduledDetails(_schid, _feetype, out amount, out fee_n_student, out due_dt, out last_dt, out _total,out n_stud_Unpaid,out balance_amount);
                    if (_feetype == 2)
                    {
                        _image = "accept.png";
                    }
                    //GetFeePaidDetails(_schid, out , out ttl_dudition, out ttl_fine, out coll_amount, );
                    if (balance_amount == 0)
                    {
                        _Rowcolor = "class=\"rowgreen\"";
                    }
                    else
                    {
                        DateTime _lst_dt;
                        DateTime _due_dt;
                        _Rowcolor = "";
                        if (DateTime.TryParse(due_dt, out _due_dt))
                        {
                            if (_due_dt <= System.DateTime.Now)
                            {
                                _Rowcolor = "class=\"rowyellow\"";
                            }

                        }
                        if (DateTime.TryParse(last_dt, out _lst_dt))
                        {
                            if (_lst_dt < System.DateTime.Now)
                            {
                                _Rowcolor = "class=\"rowred\"";
                            }

                        }

                    }

                }
                else
                {

                    _image = "cross.png";
                    _Rowcolor = "";
                    amount = 0;
                    fee_n_student = 0;
                    due_dt = "nil";
                    last_dt = "nil";
                    _total = 0;
                    n_stud_Unpaid = 0;
                    //ttl_dudition = 0;
                    //ttl_fine = 0;
                    //coll_amount = 0;
                    balance_amount = 0;
                }


                _Table.Append("<tr " + _Rowcolor + "><td>" + dr[1].ToString() + "</td><td><img alt=\"\" src=\"images/" + _image + "\" style=\"height: 30px; width: 30px\" /></td><td> " + amount.ToString("0.00") + "</td><td>" + n_student.ToString() + "</td><td>" + fee_n_student.ToString() + "</td><td>" + due_dt + "</td><td>" + last_dt + "</td><td>" + _total.ToString("0.00") + "</td><td> " + n_stud_Unpaid.ToString() + "</td><td>" + balance_amount.ToString("0.00") + "</td></tr>");

                //ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());


            }


        }
        _Table.Append("</table>");
        return _Table.ToString();

    }

    private void GetFeePaidDetails(int _schid, out int n_stud_paid, out double ttl_dudition, out double ttl_fine, out double coll_amount, out double balance_amount)
    {
        string sql = "select count(tblfeestudent.Id) from tblfeestudent where (tblfeestudent.Status='Paid' OR tblfeestudent.Status='fee Exemtion') AND tblfeestudent.SchId=" + _schid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            m_MyReader.Read();
            n_stud_paid = int.Parse(m_MyReader.GetValue(0).ToString());

        }
        else
        {
            n_stud_paid = 0;
        }
        sql = "select sum(tbltransaction.Amount) from tbltransaction where tbltransaction.AccountTo=3 and tbltransaction.PaymentElementId=" + _schid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out ttl_dudition))
            {
                ttl_dudition = 0;
            }

        }
        else
        {
            ttl_dudition = 0;
        }
        sql = "select sum(tbltransaction.Amount) from tbltransaction where tbltransaction.AccountTo=4 and tbltransaction.PaymentElementId=" + _schid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out ttl_fine))
            {
                ttl_fine = 0;
            }

        }
        else
        {
            ttl_fine = 0;
        }
        sql = "select sum(tbltransaction.Amount) from tbltransaction where (tbltransaction.AccountTo=1 OR tbltransaction.AccountTo=4) and tbltransaction.PaymentElementId=" + _schid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out coll_amount))
            {
                coll_amount = 0;
            }

        }
        else
        {
            coll_amount = 0;
        }
        sql = "select sum(tblfeestudent.BalanceAmount) from tblfeestudent where tblfeestudent.Status<>'Paid' AND tblfeestudent.SchId=" + _schid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out balance_amount))
            {
                balance_amount = 0;
            }


        }
        else
        {
            balance_amount = 0;
        }
    }

    private void GetFeescheduledDetails(int _schid, int _feetype, out double amount, out int fee_n_student, out string due_dt, out string last_dt, out double _total, out int n_stud_Unpaid, out double balance_amount)
    {
        string sql = "select tblfeeschedule.Amount, date_format(tblfeeschedule.Duedate, '%m-%d-%Y'), date_format(tblfeeschedule.LastDate, '%m-%d-%Y') from tblfeeschedule where tblfeeschedule.Id=" + _schid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {



            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out amount))
            {
                amount = 0;
            }
            due_dt = m_MyReader.GetValue(1).ToString();
            last_dt = m_MyReader.GetValue(2).ToString();

            sql = "select sum(tblfeestudent.Amount) from tblfeeschedule inner join tblfeestudent on tblfeestudent.SchId= tblfeeschedule.Id where tblfeeschedule.Id=" + _schid;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out _total))
                {
                    _total = 0;
                }
            }
            else
            {
                _total = 0;
            }

            sql = "select count(tblfeestudent.Id) from tblfeeschedule inner join tblfeestudent on tblfeestudent.SchId= tblfeeschedule.Id where tblfeeschedule.Id=" + _schid;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                fee_n_student = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            else
            {
                fee_n_student = 0;
            }
            sql = "select count(tblfeestudent.Id) from tblfeeschedule inner join tblfeestudent on tblfeestudent.SchId= tblfeeschedule.Id where tblfeestudent.BalanceAmount>0 AND tblfeeschedule.Id=" + _schid;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                n_stud_Unpaid = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            else
            {
                n_stud_Unpaid = 0;
            }
            sql = "select sum(tblfeestudent.BalanceAmount) from tblfeeschedule inner join tblfeestudent on tblfeestudent.SchId= tblfeeschedule.Id where tblfeeschedule.Id=" + _schid;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out balance_amount))
                {
                    balance_amount = 0;
                }
            }
            else
            {
                balance_amount = 0;
            }
        }
        else
        {
            amount = 0;
            fee_n_student = 0;
            due_dt = "nil";
            last_dt = "nil";
            _total = 0;
            n_stud_Unpaid = 0;
            balance_amount = 0;
        }
        if (fee_n_student != 0)
        {
            amount = _total / fee_n_student;
        }


    }
    private int GetFeeSchduleType(int _feeid)
    {
        int _feetype = 0;
        string sql = "SELECT tblfeeaccount.AssociatedId from tblfeeaccount where tblfeeaccount.Id=" + _feeid;

        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            m_MyReader.Read();

            _feetype = int.Parse(m_MyReader.GetValue(0).ToString());


        }
        return _feetype;

    }
    private int GetNoofstudinclass(int _classId, int _batchid)
    {
        int studno = 0;
        string sql = "select count(sc.StudentId) from  tblstudentclassmap sc  inner join tblstudent stud on stud.Id=sc.StudentId where sc.ClassId=" + _classId + " and stud.Status=1 AND sc.batchId=" + _batchid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            studno = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        return studno;
    }

    private int GetscheId(int _classId, int _periodid, int _batchid, int _Feeid)
    {
        int FeeSchid = 0;
        string sql = "SELECT tblfeeschedule.Id FROM tblfeeschedule WHERE tblfeeschedule.ClassId=" + _classId + " AND tblfeeschedule.PeriodId=" + _periodid + " AND tblfeeschedule.BatchId=" + _batchid + " AND tblfeeschedule.FeeId=" + _Feeid + "";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            FeeSchid = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        return FeeSchid;
    }

    public string GetFeeReportTableString(int _batchid)
    {
        StringBuilder _Table = new StringBuilder("<table width=\"100%\"><tr class=\"tablehead\"><td rowspan=\"2\" >Fee Name</td><td rowspan=\"2\">Period</td><td rowspan=\"2\">No of Unpaid<br />Students</td><td rowspan=\"2\">Total<br />Deduction</td><td rowspan=\"2\">Total Fine</td><td colspan=\"4\" style=\"text-align:center;font-weight:bold;\">Amount Collected</td><td colspan=\"4\" style=\"text-align:center;font-weight:bold;\">Balance to be collected</td></tr><tr class=\"tablehead\"><td>Previous<br />Arrear</td><td>Current<br />Batch</td><td>Future<br />Advance</td><td><b >Total</b></td><td>Previous<br />Arrier</td><td>Current<br />Batch</td><td>Future<br />Advance</td><td><b >Total</b></td></tr>");
        DataSet _Mydata_fee;
        DataSet _Mydata_period;
        string _feeid = "";
        string _feeTypeid = "";
        string _tempfeename;
        string _Rowcolor;

        double _ArrerBalance, _CurBalance, _AdvBalance, _TotalBalance;
        double _ArrerColl, _CurColl, _AdvColl, _TotalColl;

        int n_stud_unpaid;
        double ttl_dudition;
        double ttl_fine;
       ;

        //n_student = GetNoofstudinBatch(_batchid);
       
        string sql = "select tblfeeaccount.Id, tblfeeaccount.AccountName, tblfeeaccount.AssociatedId, tblfeeaccount.FrequencyId, tblfeeaccount.`Type` FROM tblfeeaccount where tblfeeaccount.`Status`=1 and tblfeeaccount.Id IN (select tblfeeschedule.FeeId AS `ID` from tblfeeschedule  Union  select tblview_transaction.FeeId AS `ID` from tblview_transaction inner join  tblview_feebill on tblview_feebill.BillNo = tblview_transaction.BillNo  where tblview_feebill.BatchId=" + _batchid + " AND tblview_feebill.Canceled=0 ) order by tblfeeaccount.`Type` DESC, tblfeeaccount.AccountName";
       //string sql = "select tblfeeaccount.Id, tblfeeaccount.AccountName, tblfeeaccount.AssociatedId, tblfeeaccount.FrequencyId, tblfeeaccount.`Type` FROM tblfeeaccount where tblfeeaccount.`Status`=1 and tblfeeaccount.Id IN (select tblfeeschedule.FeeId AS `ID` from tblfeeschedule where  tblfeeschedule.BatchId=" + _batchid + "  Union  select tblview_transaction.FeeId AS `ID` from tblview_transaction inner join  tblview_feebill on tblview_feebill.BillNo = tblview_transaction.BillNo  where tblview_feebill.BatchId=" + _batchid + " AND tblview_feebill.Canceled=0 ) order by tblfeeaccount.`Type` DESC, tblfeeaccount.AccountName";
        _Mydata_fee = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (_Mydata_fee != null && _Mydata_fee.Tables != null && _Mydata_fee.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr_fee in _Mydata_fee.Tables[0].Rows)
            {
                _feeTypeid = dr_fee[4].ToString();
                if (_feeTypeid == "1")
                {
                    //sql = "select tblperiod.Id, tblperiod.Period from tblperiod where  tblperiod.FrequencyId=" + dr_fee[3].ToString() + " AND tblperiod.Id IN ( select tblfeeschedule.PeriodId AS `ID` from tblfeeschedule where tblfeeschedule.FeeId=" + dr_fee[0].ToString() + " AND tblfeeschedule.BatchId=" + _batchid + " union select tblview_transaction.PeriodId AS `ID` from tblview_transaction inner join  tblview_feebill on tblview_feebill.BillNo = tblview_transaction.BillNo AND tblview_transaction.FeeId=" + dr_fee[0].ToString() + " where tblview_feebill.BatchId=" + _batchid + "  AND tblview_feebill.Canceled=0 )   order by tblperiod.`Order`";
                    sql = "select tblperiod.Id, tblperiod.Period from tblperiod where  tblperiod.FrequencyId=" + dr_fee[3].ToString() + " AND tblperiod.Id IN ( select tblfeeschedule.PeriodId AS `ID` from tblfeeschedule where tblfeeschedule.FeeId=" + dr_fee[0].ToString() + "  union select tblview_transaction.PeriodId AS `ID` from tblview_transaction inner join  tblview_feebill on tblview_feebill.BillNo = tblview_transaction.BillNo AND tblview_transaction.FeeId=" + dr_fee[0].ToString() + " where tblview_feebill.BatchId=" + _batchid + "  AND tblview_feebill.Canceled=0 )   order by tblperiod.`Order`";
                    _Mydata_period = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (_Mydata_period != null && _Mydata_period.Tables != null && _Mydata_period.Tables[0].Rows.Count > 0)
                    {


                        foreach (DataRow dr_period in _Mydata_period.Tables[0].Rows)
                        {

                            GetFullFeescheDetailsForPeroid(int.Parse(dr_fee[0].ToString()), int.Parse(dr_period[0].ToString()), _batchid, out n_stud_unpaid, out _ArrerBalance, out _CurBalance, out _AdvBalance, out _TotalBalance);
                            GetFullFeepaidDetailsForPeroid(int.Parse(dr_fee[0].ToString()), int.Parse(dr_period[0].ToString()), _batchid, out ttl_dudition, out ttl_fine, out _ArrerColl, out _CurColl, out _AdvColl, out _TotalColl);
                            if (_TotalBalance == 0)
                            {
                                _Rowcolor = "class=\"rowgreen\"";

                            }
                            else if (_CurBalance == 0)
                            {

                                _Rowcolor = "";
                            }
                            else
                            {
                                _Rowcolor = "class=\"rowyellow\"";
                            }
                            if (_feeid != dr_fee[0].ToString())
                            {
                                _feeid = dr_fee[0].ToString();
                                _tempfeename = dr_fee[1].ToString();
                            }
                            else
                            {
                                _tempfeename = "&nbsp;";
                            }

                            //_Table.Append("<tr " + _Rowcolor + "><td class=\"rowgray\">" + _tempfeename + "</td><td>" + dr_period[1].ToString() + "</td><td><img src=\"images/" + _image + "\" style=\"height: 30px; width: 30px\" /></td><td>" + n_student.ToString() + "</td><td>" + fee_n_student.ToString() + "</td><td>" + _total.ToString("0.00") + "</td><td>" + n_stud_unpaid.ToString() + "</td><td>" + ttl_dudition.ToString("0.00") + "</td><td>" + ttl_fine.ToString("0.00") + "</td><td>" + coll_amount.ToString("0.00") + "</td><td>" + balance_amount.ToString("0.00") + "</td></tr>");

                            _Table.Append("<tr " + _Rowcolor + "><td>" + _tempfeename + "</td><td>" + dr_period[1].ToString() + "</td><td>" + n_stud_unpaid.ToString() + "</td><td>" + ttl_dudition.ToString("0.00") + "</td><td>" + ttl_fine.ToString("0.00") + "</td><td>" + _ArrerColl.ToString("0.00") + "</td><td>" + _CurColl.ToString("0.00") + "</td><td>" + _AdvColl.ToString("0.00") + "</td><td>" + _TotalColl.ToString("0.00") + "</td><td>" + _ArrerBalance.ToString("0.00") + "</td><td>" + _CurBalance.ToString("0.00") + "</td> <td>" + _AdvBalance.ToString("0.00") + "</td><td>" + _TotalBalance.ToString("0.00") + "</td></tr>");
                            //ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());


                        }



                    }
                    GetFullFeescheDetailsForFee(int.Parse(dr_fee[0].ToString()), _batchid, out n_stud_unpaid, out _ArrerBalance, out _CurBalance, out _AdvBalance, out _TotalBalance);
                    GetFullFeepaidDetailsForFee(int.Parse(dr_fee[0].ToString()), _batchid, out ttl_dudition, out ttl_fine, out _ArrerColl, out _CurColl, out _AdvColl, out _TotalColl);
                    _Rowcolor = "class=\"rowtotal\"";
                    _Table.Append("<tr " + _Rowcolor + "><td >Sub Total</td><td> &nbsp;</td><td>" + n_stud_unpaid.ToString() + "</td><td>" + ttl_dudition.ToString("0.00") + "</td><td>" + ttl_fine.ToString("0.00") + "</td><td>" + _ArrerColl.ToString("0.00") + "</td><td>" + _CurColl.ToString("0.00") + "</td><td>" + _AdvColl.ToString("0.00") + "</td><td>" + _TotalColl.ToString("0.00") + "</td><td>" + _ArrerBalance.ToString("0.00") + "</td><td>" + _CurBalance.ToString("0.00") + "</td> <td>" + _AdvBalance.ToString("0.00") + "</td><td>" + _TotalBalance.ToString("0.00") + "</td></tr>");
                          
                    

                }

                else
                {

                    GetFullFeepaidDetailsForFee(int.Parse(dr_fee[0].ToString()), _batchid, out ttl_dudition, out ttl_fine, out _ArrerColl, out _CurColl, out _AdvColl, out _TotalColl);
                    _Rowcolor = "class=\"rowtotal\"";
                    _ArrerBalance = _CurBalance = _AdvBalance = _TotalBalance = 0;
                    _Table.Append("<tr " + _Rowcolor + "><td >" + dr_fee[1].ToString() + "</td><td>-</td><td>-</td><td>" + ttl_dudition.ToString("0.00") + "</td><td>" + ttl_fine.ToString("0.00") + "</td><td>" + _ArrerColl.ToString("0.00") + "</td><td>" + _CurColl.ToString("0.00") + "</td><td>" + _AdvColl.ToString("0.00") + "</td><td>" + _TotalColl.ToString("0.00") + "</td><td>" + _ArrerBalance.ToString("0.00") + "</td><td>" + _CurBalance.ToString("0.00") + "</td> <td>" + _AdvBalance.ToString("0.00") + "</td><td>" + _TotalBalance.ToString("0.00") + "</td></tr>");
                       
                }
              
            }

        }

        GetFullFeescheDetails(_batchid, out n_stud_unpaid, out _ArrerBalance, out _CurBalance, out _AdvBalance, out _TotalBalance);
        GetFullFeepaidDetails(_batchid, out ttl_dudition, out ttl_fine, out _ArrerColl, out _CurColl, out _AdvColl, out _TotalColl);
        _Rowcolor = "class=\"rowred\"";
   

        _Table.Append("<tr " + _Rowcolor + "><td >Grand Total</td><td> &nbsp;</td><td>" + n_stud_unpaid.ToString() + "</td><td>" + ttl_dudition.ToString("0.00") + "</td><td>" + ttl_fine.ToString("0.00") + "</td><td>" + _ArrerColl.ToString("0.00") + "</td><td>" + _CurColl.ToString("0.00") + "</td><td>" + _AdvColl.ToString("0.00") + "</td><td>" + _TotalColl.ToString("0.00") + "</td><td>" + _ArrerBalance.ToString("0.00") + "</td><td>" + _CurBalance.ToString("0.00") + "</td> <td>" + _AdvBalance.ToString("0.00") + "</td><td>" + _TotalBalance.ToString("0.00") + "</td></tr>");
                             

        _Table.Append("</table>");
        return _Table.ToString();


    }

    private void GetFullFeepaidDetails(int _batchid, out double ttl_dudition, out double ttl_fine, out double _ArrerColl, out double _CurColl, out double _AdvColl, out double _TotalColl)
    {
        string sql = "select sum(tblview_transaction.Amount) from tblview_transaction inner join tblview_feebill on tblview_feebill.BillNo=tblview_transaction.BillNo  where tblview_transaction.AccountTo=3  AND tblview_feebill.Canceled=0 AND tblview_feebill.BatchId=" + _batchid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out ttl_dudition))
            {
                ttl_dudition = 0;
            }

        }
        else
        {
            ttl_dudition = 0;
        }
        sql = "select sum(tblview_transaction.Amount) from tblview_transaction inner join tblview_feebill on tblview_feebill.BillNo=tblview_transaction.BillNo  where tblview_transaction.AccountTo=4  AND tblview_feebill.Canceled=0 AND tblview_feebill.BatchId=" + _batchid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out ttl_fine))
            {
                ttl_fine = 0;
            }

        }
        else
        {
            ttl_fine = 0;
        }

        sql = "select sum(tblview_transaction.Amount) from tblview_transaction inner join tblview_feebill on tblview_feebill.BillNo=tblview_transaction.BillNo  where (tblview_transaction.AccountTo=1 OR tblview_transaction.AccountTo=4)  AND tblview_feebill.Canceled=0 AND tblview_feebill.BatchId=" + _batchid + " AND tblview_transaction.BatchId<" + _batchid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out _ArrerColl))
            {
                _ArrerColl = 0;
            }

        }
        else
        {
            _ArrerColl = 0;
        }
        sql = "select sum(tblview_transaction.Amount) from tblview_transaction inner join tblview_feebill on tblview_feebill.BillNo=tblview_transaction.BillNo  where (tblview_transaction.AccountTo=1 OR tblview_transaction.AccountTo=4)  AND tblview_feebill.Canceled=0 AND tblview_feebill.BatchId=" + _batchid + " AND tblview_transaction.BatchId=" + _batchid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out _CurColl))
            {
                _CurColl = 0;
            }

        }
        else
        {
            _CurColl = 0;
        }
        sql = "select sum(tblview_transaction.Amount) from tblview_transaction inner join tblview_feebill on tblview_feebill.BillNo=tblview_transaction.BillNo  where (tblview_transaction.AccountTo=1 OR tblview_transaction.AccountTo=4)  AND tblview_feebill.Canceled=0 AND tblview_feebill.BatchId=" + _batchid + " AND tblview_transaction.BatchId>" + _batchid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out _AdvColl))
            {
                _AdvColl = 0;
            }

        }
        else
        {
            _AdvColl = 0;
        }
        _TotalColl = _ArrerColl + _CurColl + _AdvColl;
    }

    private void GetFullFeescheDetails(int _batchid, out int fee_n_student, out double _ArrerBalance, out double _CurBalance, out double _AdvBalance, out double _TotalBalance)
    {
        string sql = "select COUNT(DISTINCT tblfeestudent.StudId)  from tblfeeschedule inner join tblfeestudent on tblfeestudent.SchId= tblfeeschedule.Id where tblfeestudent.BalanceAmount>0";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            fee_n_student = int.Parse(m_MyReader.GetValue(0).ToString());

        }
        else
        {

            fee_n_student = 0;

        }
        sql = "select SUM(tblfeestudent.BalanceAmount)  from tblfeeschedule inner join tblfeestudent on tblfeestudent.SchId= tblfeeschedule.Id inner join tblfeeaccount on tblfeeaccount.Id=tblfeeschedule.FeeId where tblfeeaccount.`Status`=1 and tblfeeschedule.BatchId<" + _batchid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out _ArrerBalance))
                _ArrerBalance = 0;

        }
        else
        {

            _ArrerBalance = 0;

        }
        sql = "select SUM(tblfeestudent.BalanceAmount)  from tblfeeschedule inner join tblfeestudent on tblfeestudent.SchId= tblfeeschedule.Id  inner join tblfeeaccount on tblfeeaccount.Id=tblfeeschedule.FeeId where tblfeeaccount.`Status`=1 and tblfeeschedule.BatchId=" + _batchid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out _CurBalance))
                _CurBalance = 0;

        }
        else
        {

            _CurBalance = 0;

        }
        sql = "select SUM(tblfeestudent.BalanceAmount)  from tblfeeschedule inner join tblfeestudent on tblfeestudent.SchId= tblfeeschedule.Id  inner join tblfeeaccount on tblfeeaccount.Id=tblfeeschedule.FeeId where tblfeeaccount.`Status`=1 and tblfeeschedule.BatchId>" + _batchid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out _AdvBalance))
                _AdvBalance = 0;

        }
        else
        {

            _AdvBalance = 0;

        }
        _TotalBalance = _ArrerBalance + _CurBalance + _AdvBalance;
    }

    private void GetFullFeepaidDetailsForFee(int _feeid, int _batchid, out double ttl_dudition, out double ttl_fine, out double _ArrerColl, out double _CurColl, out double _AdvColl, out double _TotalColl)
    {
        string sql = "select sum(tblview_transaction.Amount) from tblview_transaction inner join tblview_feebill on tblview_feebill.BillNo=tblview_transaction.BillNo  where tblview_transaction.AccountTo=3  AND tblview_feebill.Canceled=0 AND tblview_feebill.BatchId=" + _batchid + " and tblview_transaction.FeeId=" + _feeid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out ttl_dudition))
            {
                ttl_dudition = 0;
            }

        }
        else
        {
            ttl_dudition = 0;
        }
        sql = "select sum(tblview_transaction.Amount) from tblview_transaction inner join tblview_feebill on tblview_feebill.BillNo=tblview_transaction.BillNo  where tblview_transaction.AccountTo=4  AND tblview_feebill.Canceled=0 AND tblview_feebill.BatchId=" + _batchid + " and tblview_transaction.FeeId=" + _feeid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out ttl_fine))
            {
                ttl_fine = 0;
            }

        }
        else
        {
            ttl_fine = 0;
        }

        sql = "select sum(tblview_transaction.Amount) from tblview_transaction inner join tblview_feebill on tblview_feebill.BillNo=tblview_transaction.BillNo  where (tblview_transaction.AccountTo=1 OR tblview_transaction.AccountTo=4)  AND tblview_feebill.Canceled=0 AND tblview_feebill.BatchId=" + _batchid + " AND tblview_transaction.BatchId<" + _batchid + "  and tblview_transaction.FeeId=" + _feeid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out _ArrerColl))
            {
                _ArrerColl = 0;
            }

        }
        else
        {
            _ArrerColl = 0;
        }
        sql = "select sum(tblview_transaction.Amount) from tblview_transaction inner join tblview_feebill on tblview_feebill.BillNo=tblview_transaction.BillNo  where (tblview_transaction.AccountTo=1 OR tblview_transaction.AccountTo=4)  AND tblview_feebill.Canceled=0 AND tblview_feebill.BatchId=" + _batchid + " AND tblview_transaction.BatchId=" + _batchid + "  and tblview_transaction.FeeId=" + _feeid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out _CurColl))
            {
                _CurColl = 0;
            }

        }
        else
        {
            _CurColl = 0;
        }
        sql = "select sum(tblview_transaction.Amount) from tblview_transaction inner join tblview_feebill on tblview_feebill.BillNo=tblview_transaction.BillNo  where (tblview_transaction.AccountTo=1 OR tblview_transaction.AccountTo=4)  AND tblview_feebill.Canceled=0 AND tblview_feebill.BatchId=" + _batchid + " AND tblview_transaction.BatchId>" + _batchid + "  and tblview_transaction.FeeId=" + _feeid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out _AdvColl))
            {
                _AdvColl = 0;
            }

        }
        else
        {
            _AdvColl = 0;
        }
        _TotalColl = _ArrerColl + _CurColl + _AdvColl;
    }

    private void GetFullFeescheDetailsForFee(int _feeid, int _batchid, out int fee_n_student, out double _ArrerBalance, out double _CurBalance, out double _AdvBalance, out double _TotalBalance)
    {
        string sql = "select COUNT(DISTINCT tblfeestudent.StudId)  from tblfeeschedule inner join tblfeestudent on tblfeestudent.SchId= tblfeeschedule.Id where tblfeestudent.BalanceAmount>0 AND tblfeeschedule.FeeId=" + _feeid ;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            fee_n_student = int.Parse(m_MyReader.GetValue(0).ToString());

        }
        else
        {

            fee_n_student = 0;

        }
        sql = "select SUM(tblfeestudent.BalanceAmount)  from tblfeeschedule inner join tblfeestudent on tblfeestudent.SchId= tblfeeschedule.Id where tblfeeschedule.BatchId<" + _batchid + "  AND  tblfeeschedule.FeeId=" + _feeid ;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out _ArrerBalance))
                _ArrerBalance = 0;

        }
        else
        {

            _ArrerBalance = 0;

        }
        sql = "select SUM(tblfeestudent.BalanceAmount)  from tblfeeschedule inner join tblfeestudent on tblfeestudent.SchId= tblfeeschedule.Id where tblfeeschedule.BatchId=" + _batchid + "  AND  tblfeeschedule.FeeId=" + _feeid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out _CurBalance))
                _CurBalance = 0;

        }
        else
        {

            _CurBalance = 0;

        }
        sql = "select SUM(tblfeestudent.BalanceAmount)  from tblfeeschedule inner join tblfeestudent on tblfeestudent.SchId= tblfeeschedule.Id where tblfeeschedule.BatchId>" + _batchid + "  AND  tblfeeschedule.FeeId=" + _feeid ;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out _AdvBalance))
                _AdvBalance = 0;

        }
        else
        {

            _AdvBalance = 0;

        }
        _TotalBalance = _ArrerBalance + _CurBalance + _AdvBalance;
    }

    private void GetFullFeepaidDetailsForPeroid(int _feeid, int _periodid, int _batchid, out double ttl_dudition, out double ttl_fine, out double _ArrerColl, out double _CurColl, out double _AdvColl, out double _TotalColl)
    {
        string sql = "select sum(tblview_transaction.Amount) from tblview_transaction inner join tblview_feebill on tblview_feebill.BillNo=tblview_transaction.BillNo  where tblview_transaction.AccountTo=3  AND tblview_feebill.Canceled=0 AND tblview_feebill.BatchId=" + _batchid + " and tblview_transaction.FeeId=" + _feeid + " and tblview_transaction.PeriodId=" + _periodid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out ttl_dudition))
            {
                ttl_dudition = 0;
            }

        }
        else
        {
            ttl_dudition = 0;
        }
        sql = "select sum(tblview_transaction.Amount) from tblview_transaction inner join tblview_feebill on tblview_feebill.BillNo=tblview_transaction.BillNo  where tblview_transaction.AccountTo=4  AND tblview_feebill.Canceled=0 AND tblview_feebill.BatchId=" + _batchid + " and tblview_transaction.FeeId=" + _feeid + " and tblview_transaction.PeriodId=" + _periodid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out ttl_fine))
            {
                ttl_fine = 0;
            }

        }
        else
        {
            ttl_fine = 0;
        }

        sql = "select sum(tblview_transaction.Amount) from tblview_transaction inner join tblview_feebill on tblview_feebill.BillNo=tblview_transaction.BillNo  where (tblview_transaction.AccountTo=1 OR tblview_transaction.AccountTo=4)  AND tblview_feebill.Canceled=0 AND tblview_feebill.BatchId=" + _batchid + " AND tblview_transaction.BatchId<" + _batchid + "  and tblview_transaction.FeeId=" + _feeid + " and tblview_transaction.PeriodId=" + _periodid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out _ArrerColl))
            {
                _ArrerColl = 0;
            }

        }
        else
        {
            _ArrerColl = 0;
        }
        sql = "select sum(tblview_transaction.Amount) from tblview_transaction inner join tblview_feebill on tblview_feebill.BillNo=tblview_transaction.BillNo  where (tblview_transaction.AccountTo=1 OR tblview_transaction.AccountTo=4)  AND tblview_feebill.Canceled=0 AND tblview_feebill.BatchId=" + _batchid + " AND tblview_transaction.BatchId=" + _batchid + "  and tblview_transaction.FeeId=" + _feeid + " and tblview_transaction.PeriodId=" + _periodid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out _CurColl))
            {
                _CurColl = 0;
            }

        }
        else
        {
            _CurColl = 0;
        }
        sql = "select sum(tblview_transaction.Amount) from tblview_transaction inner join tblview_feebill on tblview_feebill.BillNo=tblview_transaction.BillNo  where (tblview_transaction.AccountTo=1 OR tblview_transaction.AccountTo=4)  AND tblview_feebill.Canceled=0 AND tblview_feebill.BatchId=" + _batchid + " AND tblview_transaction.BatchId>" + _batchid + "  and tblview_transaction.FeeId=" + _feeid + " and tblview_transaction.PeriodId=" + _periodid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out _AdvColl))
            {
                _AdvColl = 0;
            }

        }
        else
        {
            _AdvColl = 0;
        }
        _TotalColl = _ArrerColl + _CurColl + _AdvColl;
    }

    private void GetFullFeescheDetailsForPeroid(int _feeid, int _periodid, int _batchid, out int fee_n_student, out double _ArrerBalance, out double _CurBalance, out double _AdvBalance,out double _TotalBalance)
    {
        string sql = "select COUNT(DISTINCT tblfeestudent.StudId)  from tblfeeschedule inner join tblfeestudent on tblfeestudent.SchId= tblfeeschedule.Id where tblfeestudent.BalanceAmount>0 AND tblfeeschedule.FeeId=" + _feeid + " and tblfeeschedule.PeriodId=" + _periodid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            fee_n_student = int.Parse(m_MyReader.GetValue(0).ToString());
       
        }
        else
        {

            fee_n_student = 0;
           
        }
        sql = "select SUM(tblfeestudent.BalanceAmount)  from tblfeeschedule inner join tblfeestudent on tblfeestudent.SchId= tblfeeschedule.Id where tblfeeschedule.BatchId<" + _batchid + "  AND  tblfeeschedule.FeeId=" + _feeid + " and tblfeeschedule.PeriodId=" + _periodid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out _ArrerBalance))
                _ArrerBalance = 0;
       
        }
        else
        {

            _ArrerBalance = 0;
           
        }
        sql = "select SUM(tblfeestudent.BalanceAmount)  from tblfeeschedule inner join tblfeestudent on tblfeestudent.SchId= tblfeeschedule.Id where tblfeeschedule.BatchId=" + _batchid + "  AND  tblfeeschedule.FeeId=" + _feeid + " and tblfeeschedule.PeriodId=" + _periodid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out _CurBalance))
                _CurBalance = 0;

        }
        else
        {

            _CurBalance = 0;

        }
        sql = "select SUM(tblfeestudent.BalanceAmount)  from tblfeeschedule inner join tblfeestudent on tblfeestudent.SchId= tblfeeschedule.Id where tblfeeschedule.BatchId>" + _batchid + "  AND  tblfeeschedule.FeeId=" + _feeid + " and tblfeeschedule.PeriodId=" + _periodid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            if (!double.TryParse(m_MyReader.GetValue(0).ToString(), out _AdvBalance))
                _AdvBalance = 0;

        }
        else
        {

            _AdvBalance = 0;

        }
        _TotalBalance = _ArrerBalance + _CurBalance + _AdvBalance;
    }

    private int GetNoofstudinBatch(int _batchid)
    {
        int studno = 0;
        string sql = "select count(sc.StudentId) from  tblstudentclassmap sc  inner join tblstudent stud on stud.Id=sc.StudentId where sc.ClassId IN (SELECT tblclass.Id from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + m_userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + m_userid + ") ) AND stud.Status=1 AND sc.batchId=" + _batchid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            studno = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        return studno;
    }

    public void UpdateFeeDetails(int _feeid, string _feename, string _Desc)
    {
        string sql = "UPDATE tblfeeaccount SET AccountName='" + _feename + "', Desciptrion='" + _Desc + "' WHERE Id=" + _feeid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        CLogging logger = CLogging.GetLogObject();
        logger.LogToFile("UpdateFeeDetails", "Request to update fee name Sql:" + sql, 'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
        m_MyReader.Close();
    }

    public bool ScheduleFeeToStudent(int _feeid, int StudentId, int ClassId, int PeriodId, int BatchId, DateTime _duedate, DateTime _lastdate, double _amount)
    {
        bool _valid = true;
        string sql;
        int _SchId = 0;
        if (m_TransationDb != null)
        {
            if (_feeid == m_temp_feeid && ClassId == m_temp_ClassId && m_temp_PeriodId == PeriodId && BatchId == m_temp_Batchid)
            {
                _SchId = m_temp_feeSchid;
            }
            else
            {
                sql = "SELECT tblfeeschedule.Id FROM tblfeeschedule WHERE tblfeeschedule.ClassId=" + ClassId + " AND tblfeeschedule.PeriodId=" + PeriodId + " AND tblfeeschedule.BatchId=" + BatchId + " AND tblfeeschedule.FeeId=" + _feeid + "";
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
                        _SchId = int.Parse(m_MyReader.GetValue(0).ToString()); ;
                    }

                }
                m_temp_feeid = _feeid;
                m_temp_ClassId = ClassId;
                m_temp_PeriodId = PeriodId;
                m_temp_Batchid = BatchId;
                m_temp_feeSchid = _SchId;

            }

            sql = "select tblfeestudent.Id from tblfeestudent where tblfeestudent.SchId=" + _SchId + " AND tblfeestudent.StudId=" + StudentId;
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _valid = false;
            }
            else
            {
                ScheduleStudFee(StudentId, _SchId, _amount, "Scheduled");
                //sql = "INSERT INTO tblfeestudent(SchId,StudId,Amount,BalanceAmount,Status) VALUES(" + _SchId + "," + StudentId + "," + _amount + "," + _amount + ",'Scheduled')";
                //m_TransationDb.TransExecuteQuery(sql);
            }

        }

        return _valid;
    }

    public void ClearTempVariables()
    {

        m_temp_feeid = 0;
        m_temp_ClassId = 0;
        m_temp_PeriodId = 0;
        m_temp_Batchid = 0;
        m_temp_feeSchid = 0;
    }

    public bool QuickBillEnabled()
    {
        bool _valid = false;
        string QuickBill;
        string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='QuickBill'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            QuickBill = m_MyReader.GetValue(0).ToString();
            if (QuickBill == "1")
            {
                _valid = true;
            }
        }
        return _valid;
    }

    public void loaddrp_category()
    {
        throw new NotImplementedException();
    }

    public bool Ckeck4AssgedValueIsCorrect(string _assgmode, string _category)
    {
        string sql;
        bool _flag = true;
        string _temp;
        sql = "select tblruleitem.fieldType from tblruleitem WHERE tblruleitem.name='" + _category + "'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _temp = m_MyReader.GetValue(0).ToString();
            if (_temp == "Varchar")
            {
                if (_assgmode == "Equal")
                {
                    _flag = true;
                }
                else
                {
                    _flag = false;
                }
            }
            else
            {
                _flag = true;
            }
        }
        return _flag;
    }

    public int GetRuleItemId(string _category, out int _havemaster)
    {
        int m_RuleitemId = 0;
        _havemaster = 0;
        string sql;
        sql = "select tblruleitem.Id , tblruleitem.havemaster from tblruleitem where tblruleitem.name='" + _category + "'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            m_RuleitemId = int.Parse(m_MyReader.GetValue(0).ToString());
            _havemaster = int.Parse(m_MyReader.GetValue(1).ToString());

        }
        return m_RuleitemId;
    }

    public void AddToRuleTbl(string _rulename, int _TypeId, float _amount, int _ruleitemId, string _assgmode, string _FieldValue)
    {

        string sql = " insert into tblrules (RuleName,Amounttype,Amount,tblruleitemId,AssigMode,FieldValue) values ('" + _rulename + "'," + _TypeId + " ," + _amount + " ," + _ruleitemId + ",'" + _assgmode + " ','" + _FieldValue + "')";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);

    }



    public bool CheckPresenceOfRuleName(string _rulename)
    {
        bool _flag = false;
        CLogging logger = CLogging.GetLogObject();
        string sql = "select tblrules.RuleName from tblrules where tblrules.RuleName='" + _rulename + " '";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        logger.LogToFile("CheckPresenceOfRuleName", "Select rule name", 'I', CLogging.PriorityEnum.LEVEL_IMPORTANT, LoginUserName);
        if (m_MyReader.HasRows)
        {
            _flag = true;
        }
        return _flag;
    }



    public void AddDateToTblRuleClassMap(int _RuleId, int _ClassId, double _Amount, int _FeeId)
    {

        CLogging logger = CLogging.GetLogObject();
        string sql = "insert into tblruleclassmap (RuleId,classId,`Values`,feeTypeId) VALUES(" + _RuleId + "," + _ClassId + "," + _Amount + " ," + _FeeId + ")";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        logger.LogToFile("AddDateToTblRuleClassMap", "inserting into rule class map", 'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);


    }

    public bool CheckRuleAndClassEntryToTblRuleclassmap(int _RuleId, int _ClassId, int _FeedId)
    {
        bool _flag = false;
        CLogging logger = CLogging.GetLogObject();
        string sql = "select tblruleclassmap.Id  from tblruleclassmap where tblruleclassmap.RuleId=" + _RuleId + " and tblruleclassmap.classId=" + _ClassId + " and tblruleclassmap.feeTypeId= " + _FeedId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        logger.LogToFile("CheckRuleAndClassEntryToTblRuleclassmap", "Checking whether ruleuclassmap entry have made", 'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
        if (m_MyReader.HasRows)
        {
            _flag = true;
        }
        return _flag;
    }




    public void ScheduleClassFeeAccordingTotheRule(double _BaseAmount, int _feeid, int _classId, int _BatchId, int _Periodid, DateTime _duedate, DateTime _lastdate,int _CurrentBatchId)
    {

        OdbcDataReader m_MyReadernew;
        double _studamt;
        DataSet _ruleset;
        int _studId;
        bool _continue = true;
        try
        {
            CLogging logger = CLogging.GetLogObject();
            CreateTansationDb();
            int schedId = ScheduleStudFee(_classId, _Periodid, _duedate, _lastdate, _BatchId, _feeid, _BaseAmount);
            if (schedId == -1)
            {
                _continue = false;
            }
            if (_continue)
            { //select tblrules.Amounttype, tblrules.Amount , tblrules.AssigMode , tblrules.FieldValue,tblruleitem.tblname , tblruleitem.Colname , tblruleitem.fieldType from tblruleclassmap inner join tblrules on tblruleclassmap.RuleId = tblrules.Id  inner join tblruleitem on tblrules.tblruleitemId = tblruleitem.Id   where tblruleclassmap.classId=8 and tblruleclassmap.feeTypeId=11  
                string sql = "select tblrules.Amounttype, tblrules.Amount , tblrules.AssigMode , tblrules.FieldValue,tblruleitem.tblname , tblruleitem.Colname , tblruleitem.fieldType from tblruleclassmap inner join tblrules on tblruleclassmap.RuleId = tblrules.Id  inner join tblruleitem on tblrules.tblruleitemId = tblruleitem.Id where tblruleclassmap.classId=" + _classId + " and tblruleclassmap.feeTypeId=" + _feeid;
                _ruleset = m_TransationDb.ExecuteQueryReturnDataSet(sql);
                logger.LogToFile("ScheduleClassFeeAccordingTotheRule", "Select rule amount ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                sql = "SELECT tblstudentclassmap.StudentId from tblstudentclassmap inner JOIN tblstudent on tblstudent.Id=tblstudentclassmap.StudentId where tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + _CurrentBatchId + " AND tblstudentclassmap.ClassId=" + _classId;
                m_MyReadernew = m_TransationDb.ExecuteQuery(sql);
                logger.LogToFile("ScheduleClassFeeAccordingTotheRule", "Selecting Student id  from stuentclassmap ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                if (m_MyReadernew.HasRows)
                {
                    while (m_MyReadernew.Read())
                    {
                        _studId = int.Parse(m_MyReadernew.GetValue(0).ToString());
                        _studamt = GetStudCalcuAmt(_ruleset, _studId, _BaseAmount);

                        AddStudDataToTblFeeStud(schedId, _studId, _studamt);
                        //addFeeTotheStudentAfterRules(_AmountAfterRuleCal, _ruleassigmode, _ruleFieldvalue, _feeid, _classId, _BatchId, _BaseAmount, schedId, _Periodid, _duedate, _lastdate);
                    }
                }
            }
            if (_continue)
            {
                EndSucessTansationDb();

            }
            else
            {
                EndFailTansationDb();
            }
        }
        catch (Exception)
        {
            EndFailTansationDb();
        }


    }

    private void AddStudDataToTblFeeStud(int schedId, int _studId, double _studamt)
    {
        string sql;
        CLogging logger = CLogging.GetLogObject();
        double BalanceAmount = _studamt;
        string _Status = "";
        if (_studamt > 0)
        {
            _Status="Scheduled";
        }
        else
        {
              _Status="fee Exemtion";
        }
        AutoAdvanceCancellion(schedId, _studId, ref _studamt, ref BalanceAmount, ref _Status);
        sql = "INSERT INTO tblfeestudent(SchId,StudId,Amount,BalanceAmount,Status) VALUES(" + schedId + "," + _studId + "," + _studamt + "," + BalanceAmount + ",'"+_Status+"')";
        m_TransationDb.TransExecuteQuery(sql);
        logger.LogToFile("AddStudDataToTblFeeStud", "Insering into feestudent tbl ", 'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);

    }

    private double GetStudCalcuAmt(DataSet _ruleset, int _studId, double _BaseAmount)
    {
        double _ReturnAmt = _BaseAmount;
        string _tblname, _colname, _ruleassigmode, _ruleFieldvalue, _fieldtype;
        int _rulesAmounttype;
        float _rulesAmount;
        double _AmountAfterRuleCal;
        foreach (DataRow dr_rule in _ruleset.Tables[0].Rows)
        {
            _tblname = dr_rule[4].ToString();
            _colname = dr_rule[5].ToString();
            _ruleassigmode = dr_rule[2].ToString();
            _ruleFieldvalue = dr_rule[3].ToString();
            _fieldtype = dr_rule[6].ToString();
            _rulesAmounttype = int.Parse(dr_rule[0].ToString());
            _rulesAmount = float.Parse(dr_rule[1].ToString());
            if (CheckIfTheRuleIsApllicableTothisStudent(_studId, _tblname, _colname, _ruleassigmode, _ruleFieldvalue, _fieldtype) == true)
            {
                calculateRuleAmount(_rulesAmounttype, _rulesAmount, _BaseAmount, out _AmountAfterRuleCal);
                if (_rulesAmount > 0)
                {
                    if (_ReturnAmt > _AmountAfterRuleCal)
                    {
                        _ReturnAmt = _AmountAfterRuleCal;
                    }
                }
                if (_rulesAmount < 0)
                {
                    if (_ReturnAmt < _AmountAfterRuleCal)
                    {
                        _ReturnAmt = _AmountAfterRuleCal;
                    }
                }

            }
        }

        if (_ReturnAmt < 0)
        {
            _ReturnAmt = 0;
        }
        return _ReturnAmt;

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


    ///importance code....

    /*private void addFeeTotheStudentAfterRules(double _tembaseamount, string _ruleassigmode, string _ruleFieldvalue, int _feeid, int _classId, int _BatchId, double _BaseAmount, int schedId, int _Periodid, DateTime _duedate, DateTime _lastdate)
    {
        string _tblname="", _colname="",_fieldtype="";
        double _SmallAmountTostudent;
        int _studentId;
        string sql = "select tblruleitem.tblname , tblruleitem.Colname , tblruleitem.fieldType   from tblruleitem inner join tblrules on tblruleitem.Id = tblrules.tblruleitemId inner join tblruleclassmap on tblrules.Id = tblruleclassmap.RuleId   where tblruleclassmap.classId=" + _classId + " and tblruleclassmap.feeTypeId=" + _feeid;
        m_MyReader1 = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader1.HasRows)
        {
            while (m_MyReader1.Read())
            {
                _tblname = m_MyReader1.GetValue(0).ToString();
                _colname = m_MyReader1.GetValue(1).ToString();
                _fieldtype = m_MyReader1.GetValue(2).ToString();

                sql = "SELECT tblstudentclassmap.StudentId from tblstudentclassmap inner JOIN tblstudent on tblstudent.Id=tblstudentclassmap.StudentId where tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + _BatchId + " AND tblstudentclassmap.ClassId=" + _classId;
                m_MyReader2 = m_TransationDb.ExecuteQuery(sql);
                if (m_MyReader2.HasRows)
                {
                    while (m_MyReader2.Read())
                    {
                        _studentId = int.Parse(m_MyReader2.GetValue(0).ToString());
                       
                        if (CheckIfTheRuleIsApllicableTothisStudent(_studentId, _tblname, _colname, _ruleassigmode, _ruleFieldvalue, _fieldtype) == true)
                        {
                            if (!CheckIfEnterTotblfeestudent(schedId, _studentId) )
                            {
                                _SmallAmountTostudent = checkFromTheTblfeestudententrySmallAmountApllicabletoThatStudent(schedId, _studentId, _BaseAmount);
                                sql = "UPDATE tblfeestudent SET tblfeestudent.Amount = " + _SmallAmountTostudent + " , tblfeestudent.BalanceAmount=" + _SmallAmountTostudent + "   WHERE tblfeestudent.SchId =" + schedId + "and tblfeestudent.StudId=" + _studentId + " and tblfeestudent.`Status`='Scheduled '";
                                m_TransationDb.TransExecuteQuery(sql);
                            }
                            else if (CheckIfanyEntryIsMadeintblfeestudent(schedId, _studentId) == false)
                            {
                                sql = "INSERT INTO tblfeestudent(SchId,StudId,Amount,BalanceAmount,Status) VALUES(" + schedId + "," + _studentId + "," + _BaseAmount + "," + _BaseAmount + ",'Scheduled')";
                                m_TransationDb.TransExecuteQuery(sql);

                            }

                        }
                        else
                        {
                            if (CheckIfanyEntryIsMadeintblfeestudent(schedId, _studentId) == false)
                            {
                                sql = "INSERT INTO tblfeestudent(SchId,StudId,Amount,BalanceAmount,Status) VALUES(" + schedId + "," + _studentId + "," + _SmallAmountTostudent + "," + _SmallAmountTostudent + ",'Scheduled')";
                                m_TransationDb.TransExecuteQuery(sql);
                            }
                        }

                    }
                }
            }
        }


    }*/

    private double checkFromTheTblfeestudententrySmallAmountApllicabletoThatStudent(int schedId, int p, double _BaseAmount)
    {
        double _retamount = 0;
        string sql = "select tblfeestudent.Amount from tblfeestudent where tblfeestudent.SchId=1 and tblfeestudent.StudId=8 and tblfeestudent.`Status`='Scheduled'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            if (_BaseAmount < double.Parse(m_MyReader.GetValue(0).ToString()))
            {
                _retamount = _BaseAmount;
            }
            else
            {
                _retamount = double.Parse(m_MyReader.GetValue(0).ToString());
            }
        }

        return _retamount;
    }

    private bool CheckIfanyEntryIsMadeintblfeestudent(int schedId, int _studentId)
    {
        bool _flag = false;
        CLogging logger = CLogging.GetLogObject();
        string sql = "SELECT tblfeestudent.Id from tblfeestudent where tblfeestudent.SchId=" + schedId + " and tblfeestudent.StudId=" + _studentId + " and tblfeestudent.`Status`='Scheduled' ";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        logger.LogToFile("CheckIfanyEntryIsMadeintblfeestudent", "SELECT tblfeestudent.Id from tblfeestudent ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        if (m_MyReader.HasRows)
        {
            _flag = true;
        }
        return _flag;
    }

    private bool CheckIfEnterTotblfeestudent(int schedId, int _studentId)
    {
        bool _flag = false;
        CLogging logger = CLogging.GetLogObject();
        string sql = "SELECT tblfeestudent.Id from tblfeestudent where tblfeestudent.SchId=" + schedId + " and tblfeestudent.StudId=" + _studentId + " and( tblfeestudent.`Status`='Paid' or tblfeestudent.`Status`='Arrear' OR tblfeestudent.`Status`='Scheduled' )";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        logger.LogToFile("CheckIfEnterTotblfeestudent", "SELECT tblfeestudent.Id from tblfeestudent ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
        if (m_MyReader.HasRows)
        {
            _flag = true;
        }

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











    //  public void ScheduleClassFeeAccordingTotheRule1(double _BaseAmount, int _feeid, int _classId, int _BatchId, int schedId, int p_6, DateTime _duedate, DateTime _lastdate)
    // yFeeMang.ScheduleClassFeeAccordingTotheRule(double.Parse(Txt_amount.Text.ToString()), int.Parse(Session["FeeId"].ToString()), int.Parse(ChkBox_Class.Items[i].Value.ToString()), MyUser.CurrentBatchId, schedId, int.Parse(Drp_Perod1.SelectedValue.ToString()), _duedate, _lastdate);



    public bool CheckForRuleApplicableToClassAndFee1(int _ClassId, int _FeeId)
    {
        bool _flag = false;

        CLogging logger = CLogging.GetLogObject();
        string sql = "select tblruleclassmap.RuleId from tblruleclassmap where tblruleclassmap.classId=" + _ClassId + " and tblruleclassmap.feeTypeId=" + _FeeId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        logger.LogToFile("CheckForRuleApplicableToClassAndFee1", "select tblruleclassmap.RuleId from tblruleclassmap  ", 'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
        if (m_MyReader.HasRows)
        {
            _flag = true;
        }
        return _flag;
    }

    public void insertintoscheduletbl(double _BaseAmount, int _feeid, int _classId, int _BatchId, int _Periodid, DateTime _duedate, DateTime _lastdate)
    {

        CLogging logger = CLogging.GetLogObject();
        string sql = "INSERT INTO tblfeeschedule(ClassId,PeriodId,FeeId,BatchId,Duedate,LastDate,Amount) VALUES(" + _classId + "," + _Periodid + "," + _feeid + "," + _BatchId + ",'" + _duedate.Date.ToString("s") + "','" + _lastdate.Date.ToString("s") + "' ," + _BaseAmount + ")";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        logger.LogToFile("insertintoscheduletbl", "inserting into feeschedule tbl ", 'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);

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
            CreateTansationDb();
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
            if (_SteduleId != -1)
            {
                EndSucessTansationDb();
            }
            else
            {
                EndFailTansationDb();
            }
        }
        catch (Exception)
        {
            EndFailTansationDb();
        }


        return _flag;

    }
    
    public bool IsValidFeeName1(string _feename, string _LblFeename)
    {
        bool _valid;
        if (_feename != _LblFeename)
        {
            string sql = "select Id from tblfeeaccount where Status=1 AND AccountName='" + _feename + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _valid = false;
            }
            else
            {

                _valid = true;
            }
        }
        else
        {
            _valid = true;
        }
        m_MyReader.Close();
        return _valid;

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
        set
        {
            m_AdvanceAutoCancel = value;
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

    public void SettleAdvance_WithDueFees(string StudentId)
    {

        CheckForFeeSimilarity(StudentId);
        DataSet _AdvanceDataSet = GetAdvanceDataSet(StudentId);
        DataSet _FeeDataSet = GetFeeDataSet(StudentId);
        

        bool _continue=true;
        while (_continue)
        {
            _continue = false;
            if (_FeeDataSet != null && _AdvanceDataSet != null && _FeeDataSet.Tables[0].Rows.Count > 0 && _AdvanceDataSet.Tables[0].Rows.Count > 0)
            {
                int AdanceId = 0, FeeStudId = 0;
                int.TryParse(_AdvanceDataSet.Tables[0].Rows[0][0].ToString(), out AdanceId);
                int.TryParse(_FeeDataSet.Tables[0].Rows[0][0].ToString(), out FeeStudId);
                if (AdanceId > 0 && FeeStudId > 0)
                {
                    if (AdvanceSettelment(AdanceId, FeeStudId))
                    {
                        _AdvanceDataSet = GetAdvanceDataSet(StudentId);
                        _FeeDataSet = GetFeeDataSet(StudentId);
                        _continue = true;
                    }
                }
            }

        }

    }

    private void CheckForFeeSimilarity(string StudentId)
    {
        DataSet _AdvanceDataSet = GetAdvanceDataSet(StudentId);
        DataSet _FeeDataSet = GetFeeDataSet(StudentId);
        if (_FeeDataSet != null && _AdvanceDataSet != null && _FeeDataSet.Tables[0].Rows.Count > 0 && _AdvanceDataSet.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow DrAdvance in _AdvanceDataSet.Tables[0].Rows)
            {
                int AdvId = 0, AdvFeeId = 0,AdvPeriodId=0;
                int.TryParse(DrAdvance[0].ToString(), out AdvId);
                int.TryParse(DrAdvance[2].ToString(), out AdvFeeId);
                int.TryParse(DrAdvance[3].ToString(), out AdvPeriodId);

                foreach (DataRow DrFee in _FeeDataSet.Tables[0].Rows)
                {
                    int FeeStudId = 0, F_FeeId = 0, FeePeriodId = 0;
                    int.TryParse(DrFee[0].ToString(), out FeeStudId);
                    int.TryParse(DrFee[2].ToString(), out F_FeeId);
                    int.TryParse(DrFee[3].ToString(), out FeePeriodId);

                    if (AdvId > 0 && FeeStudId > 0)
                    {
                        if (AdvFeeId == F_FeeId && AdvPeriodId == FeePeriodId)
                        {
                            AdvanceSettelment(AdvId, FeeStudId);
                        }
                    }

                }
            }
        }
    }

    private DataSet GetFeeDataSet(string StudentId)
    {
        string sql1 = "select tblfeestudent.Id,tblfeestudent.BalanceAmount,tblfeeschedule.FeeId,tblfeeschedule.PeriodId from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId where tblfeestudent.StudId=" + StudentId + " and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion' and tblfeeschedule.DueDate <= CURRENT_DATE() ORDER BY tblfeestudent.BalanceAmount DESC";
        return m_TransationDb.ExecuteQueryReturnDataSet(sql1);
    }

    private DataSet GetAdvanceDataSet(string StudentId)
    {
        string sql = "select Id,Amount,FeeId,PeriodId from tblstudentfeeadvance where StudentId=" + StudentId + " ORDER BY Amount DESC";
        return m_TransationDb.ExecuteQueryReturnDataSet(sql);
       
    }


    public bool AdvanceSettelment(int _advId, int _FeeStudentId)
    {
        bool _valid = true;
        double _advanceAmt=GetAdvanceAmount(_advId);
        double _CanceladvanceAmt = 0;
        double _BalanceAmount = 0;
        string _Status = "";
        if (_advanceAmt != 0 && GetStudFeeScheduleDetails(_FeeStudentId,out _BalanceAmount,out _Status))
        {
            RecalculateSchDetails(ref _advanceAmt, ref _CanceladvanceAmt, ref _BalanceAmount, ref _Status);
            CancelAdvance(_advId, _advanceAmt, _CanceladvanceAmt);
            UpdateStudentFeeDue(_FeeStudentId,_BalanceAmount,_Status);
            
        }
        else
        {
            _valid = false;
        }

        return _valid;

    }

    private void UpdateStudentFeeDue(int _FeeStudentId, double _BalanceAmount, string _Status)
    {
        string sql = "UPDATE tblfeestudent SET tblfeestudent.BalanceAmount=" + _BalanceAmount + ", tblfeestudent.`Status`='" + _Status + "'  WHERE tblfeestudent.Id =" + _FeeStudentId;
        if (m_TransationDb == null)
            m_MysqlDb.ExecuteQuery(sql);
        else
            m_TransationDb.ExecuteQuery(sql);
    }

    private bool GetStudFeeScheduleDetails(int _FeeStudentId, out double _BalanceAmount, out string _Status)
    {
        bool _valid = false;

        _BalanceAmount = 0;
        _Status = "";
        OdbcDataReader _MyReader = null;
        string sql = "select tblfeestudent.BalanceAmount, tblfeestudent.`Status` from  tblfeestudent where tblfeestudent.Id=" + _FeeStudentId;
        if (m_TransationDb == null)
            _MyReader = m_MysqlDb.ExecuteQuery(sql);
        else
            _MyReader = m_TransationDb.ExecuteQuery(sql);

        if (_MyReader.HasRows)
        {
            if (!double.TryParse(_MyReader.GetValue(0).ToString(), out _BalanceAmount))
            {
                _BalanceAmount = 0;

            }
            _Status = _MyReader.GetValue(1).ToString();

        }
        if (_BalanceAmount != 0)
            _valid = true;
        return _valid;
    }

    private double GetAdvanceAmount(int _advId)
    {
        double _advanceAmt = 0;
        OdbcDataReader _MyReader = null;
        string sql = "select tblstudentfeeadvance.Amount from tblstudentfeeadvance  where tblstudentfeeadvance.Id=" + _advId;
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
        string sql="";
        int StudentId,BatchId,FeeId,PeriodId;
        string StudentName,FeeName,PeriodName,TempId;
        double AdvanceBalance;
        GetFeeAdvanceDetails(_advId, out StudentId, out StudentName, out FeeName, out PeriodName, out BatchId, out FeeId, out PeriodId, out TempId, out AdvanceBalance);
        InserAdvanceTransaction(StudentId, StudentName, FeeName, PeriodName, BatchId, _CanceladvanceAmt, FeeId, PeriodId, TempId, 0, "NIL",AdvanceBalance - _CanceladvanceAmt);
        if(_advanceAmt>0)
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
        BatchId =0;
        FeeId = 0;
        PeriodId =0;
        TempId = "";
        AdvanceBalance =0;
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
            PeriodName =_MyReader.GetValue(3).ToString();
            BatchId = int.Parse(_MyReader.GetValue(4).ToString());
            FeeId = int.Parse(_MyReader.GetValue(5).ToString());
            PeriodId = int.Parse(_MyReader.GetValue(6).ToString());
            TempId = _MyReader.GetValue(7).ToString();
            AdvanceBalance = GetTotalAdvanceBalance();
       
        }
            
    }

    private double GetTotalAdvanceBalance()
    {
        double _advanceAmt=0;
        OdbcDataReader _MyReader1 = null;
        string sql = "select tblfeeadvancetransaction.AdvanceBalance from tblfeeadvancetransaction order by tblfeeadvancetransaction.Id desc LIMIT 0,1";
        if (m_TransationDb == null)
            _MyReader1 = m_MysqlDb.ExecuteQuery(sql);
        else
            _MyReader1 = m_TransationDb.ExecuteQuery(sql);

        if (_MyReader1.HasRows)
        {
            if (!double.TryParse(_MyReader1.GetValue(0).ToString(), out _advanceAmt))
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

    public string LoadFeeCollectedStaffName(string BillNo, string BillType)
    {
        string Name = "";
        string sql = "";
        if (BillType == "0")
        {
            sql = "select DISTINCT tblview_user.SurName from tblview_feebill inner join tblview_user on tblview_user.Id = tblview_feebill.CollectedUser  where tblview_feebill.BillNo='" + BillNo + "'";
        }
        else
        {
            sql = "select DISTINCT tblview_user.SurName from tblview_feebill inner join tblview_user on tblview_user.Id = tblview_feebill.CollectedUser  where tblview_feebill.BillNo='" + BillNo + "'";
           // sql = "select DISTINCT tblview_user.SurName from tbljoining_feebill inner join tblview_user on tblview_user.Id = tbljoining_feebill.UserId  where tbljoining_feebill.BillNo='" + BillNo + "'";
        }
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Name = m_MyReader.GetValue(0).ToString();
        }
        return Name;
    }
    public string LoadFeeCollectedStaffNameWithDesignation(string BillNo, string BillType)
    {
        string Name = "";
        string sql = "";
        if (BillType == "0")
        {
            sql = "select DISTINCT tblview_user.SurName,tblrole.RoleName from tblview_feebill inner join tblview_user on tblview_user.Id = tblview_feebill.CollectedUser inner join tblrole on tblrole.Id=RoleId where tblview_feebill.BillNo='" + BillNo + "'";
        }
        else
        {
            sql = "select DISTINCT tblview_user.SurName,tblrole.RoleName from tblview_feebill inner join tblview_user on tblview_user.Id = tblview_feebill.CollectedUser  inner join tblrole on tblrole.Id=RoleId  where tblview_feebill.BillNo='" + BillNo + "'";
            // sql = "select DISTINCT tblview_user.SurName from tbljoining_feebill inner join tblview_user on tblview_user.Id = tbljoining_feebill.UserId  where tbljoining_feebill.BillNo='" + BillNo + "'";
        }
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Name = m_MyReader.GetValue(0).ToString() + " (" + m_MyReader["RoleName"] + ")";
        }
        return Name;
    }


    # region Clearence

    public bool ClearenceEnabledForDD()
    {
        bool _flag = false;
        string sql = "SELECT tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='DDClearence'";
        m_MyReader = m_TransationDb.ExecuteQuery(sql);

        if (m_MyReader.HasRows)
        {
            string value = m_MyReader.GetValue(0).ToString();
            if (value == "1")
            {
                _flag = true;
            }
        }

        return _flag;
    }

    public bool ClearenceEnabledForCheck()
    {
        bool _flag = false;

        string sql = "SELECT tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='CheckClearence'";
        m_MyReader = m_TransationDb.ExecuteQuery(sql);

        if (m_MyReader.HasRows)
        {
            string value = m_MyReader.GetValue(0).ToString();
            if (value == "1")
            {
                _flag = true;
            }
        }

        return _flag;
    }




    //public string ClearBill(string _BillNo, int _UserId, string _Studid, int _BatchId)
    //{
        
    //    CreateTansationDb();
    //    string _billno = "0";
    //    try
    //    {
    //        if (m_BillPrefix == null)
    //        {
    //            m_BillPrefix = GetBillPrefix();
    //        }
    //        General _GenObj = new General(m_TransationDb);
    //        int _FeeMaxId = _GenObj.GetTableMaxIdWithCondition("tblfeebill", "Counter", _BatchId, "AccYear");
    //        int _FeeId = _GenObj.GetTableMaxId("tblview_feebill", "TransationId");
    //        string _ClassId = GetClassID(int.Parse(_Studid));
    //        DateTime _Now = DateTime.Now;
    //        string sql = "insert into tblfeebill(Id,StudentID,TotalAmount,Date,PaymentMode,PaymentModeId,BankName,UserId,Counter,AccYear,ClassId) select " + _FeeMaxId + ",StudentID,TotalAmount,Date,PaymentMode,PaymentModeId,BankName,UserId," + _FeeId + "," + _BatchId + "," + _ClassId + " from tblfeebillclearence where BillNo='" + _BillNo + "'";
    //        m_TransationDb.ExecuteQuery(sql);
    //        _billno = m_BillPrefix + _Now.Year.ToString() + "-" + _FeeMaxId.ToString();
    //        sql = "UPDATE tblfeebill SET BillNo= '" + _billno + "', CreatedDateTime='" + _Now.Date.ToString("s") + "' WHERE Id =" + _FeeMaxId;
    //        m_TransationDb.ExecuteQuery(sql);
    //           int _TransMaxId = 0;
    //           sql = "select UserId,PaymentElementId,'" + _billno + "','" + _Now.Date.ToString("s") + "', Amount , AccountTo, AccountFrom , Type , TransType from tbltransactionclearence where BillNo='" + _BillNo + "'";
    //           m_MyReader = m_TransationDb.ExecuteQuery(sql);
    //           if (m_MyReader.HasRows)
    //           {
    //               while (m_MyReader.Read())
    //               {
    //                   _TransMaxId = _GenObj.GetTableMaxId("tblview_transaction", "TransationId");
    //                   sql = " insert into tbltransaction(TransationId,UserId,PaymentElementId,BillNo,PaidDate,Amount,AccountTo,AccountFrom,Type,TransType) values(" + _TransMaxId + "," + m_MyReader.GetValue(0).ToString() + "," + m_MyReader.GetValue(1).ToString() + ",'" + _billno + "','" + _Now.Date.ToString("s") + "'," + m_MyReader.GetValue(4).ToString() + "," + m_MyReader.GetValue(5).ToString() + "," + m_MyReader.GetValue(6).ToString() + ",'" + m_MyReader.GetValue(7).ToString() + "'," + m_MyReader.GetValue(8).ToString() + ")";
    //                   m_TransationDb.ExecuteQuery(sql);
    //               }
    //           }
    //            sql = "insert into tblbillclearence (StudentId,BillNo,Userid,ClearedDate) values (" + int.Parse(_Studid.ToString()) + ",'" + _billno + "'," + _UserId + ",'" + _Now.Date.ToString("s") + "')";
    //            m_TransationDb.ExecuteQuery(sql);

    //            sql = "delete from tblfeebillclearence where BillNo='" + _BillNo + "'";
    //            m_TransationDb.ExecuteQuery(sql);

    //            sql = "delete from tbltransactionclearence where BillNo='" + _BillNo + "'";
    //            m_TransationDb.ExecuteQuery(sql);
    //            EndSucessTansationDb(); 
    //    }
    //    catch
    //    {
    //        EndFailTansationDb();
    //        _billno = "0";
    //    }
    //    return _billno;
    //}

    public string ClearBill(string _OldBillNo, string _Studid, int _BatchId)
    {
        //**********************Bill variables***************
        double _Total = 0;
        int _StudentId=0;
        string _PaymentMode = "";
        string _PaymentId = "";
        string _Bank = "";
        string _PayDate = "";
        int _UserId=1;
        int _CurrentBatchId;
        string _ClassId = "0";
        string _StudentName = "";
        int _Regular = 1;
        string _TempId = "";
        string _Table = "tblfeebill";
        string _BillId = "";

        //***************************Transaction variables*************
        General _GenObj = new General(m_TransationDb);
        int _TransMaxId = 0;
        string Trn_PaymentElementId = "";
        string Trn_Amount = "";
        string Trn_AccountTo = "";
        string Trn_AccountFrom = "";
        string Trn_Type = "";
        DateTime Trn_PaidDate = new DateTime();
        string Trn_ClassId = "";
        string Trn_CollectedUser = "";
        string Trn_BatchId = "";
        string Trn_FeeName = "";
        string Trn_PeriodName = "";
        string Trn_CollectionType = "";
        string Trn_RegularFee = "";
        string Trn_PeriodId = "";
        string Trn_FeeId = "-1";
        string TransType="0",Canceled="0",BalanceAmount="0";
        string OtherReference = "";
        //*******************************


        string sql = "select StudentID,TotalAmount,DATE_FORMAT(`Date`,'%d/%m/%Y') as `Date`,PaymentMode,PaymentModeId,BankName,UserId,AccYear,ClassID,StudentName,RegularFee,TempId,OtherReference from tblfeebillclearence where BillNo='" + _OldBillNo + "'";
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int.TryParse(m_MyReader.GetValue(0).ToString(), out _StudentId);
            double.TryParse(m_MyReader.GetValue(1).ToString(), out _Total);
            _PayDate = m_MyReader.GetValue(2).ToString();
            _PaymentMode = m_MyReader.GetValue(3).ToString();
            _PaymentId = m_MyReader.GetValue(4).ToString();
            _Bank = m_MyReader.GetValue(5).ToString();
            int.TryParse(m_MyReader.GetValue(6).ToString(), out _UserId);
            int.TryParse(m_MyReader.GetValue(7).ToString(), out _CurrentBatchId);
            _ClassId = m_MyReader.GetValue(8).ToString();
            _StudentName = m_MyReader.GetValue(9).ToString();
            int.TryParse(m_MyReader.GetValue(10).ToString(), out _Regular);
            _TempId = m_MyReader.GetValue(11).ToString();
            OtherReference = m_MyReader.GetValue(12).ToString();
            _BillId = GenBill(_Total, _StudentId, _PaymentMode, _PaymentId, _Bank, _PayDate, _UserId, _CurrentBatchId, _ClassId, _StudentName, _Regular, _TempId, OtherReference, _Table);
        }
        sql = "select PaymentElementId,Amount,AccountTo,AccountFrom,Type,DATE_FORMAT(`PaidDate`,'%d/%m/%Y') as `PaidDate`,ClassId,CollectedUser,BatchId,FeeName,PeriodName,CollectionType,RegularFee,PeriodId,FeeId,TransType,Canceled,BalanceAmount from tbltransactionclearence where BillNo='" + _OldBillNo + "'";
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                _TransMaxId = _GenObj.GetTableMaxId("tblview_transaction", "TransationId");
                Trn_PaymentElementId = m_MyReader.GetValue(0).ToString();
                Trn_Amount = m_MyReader.GetValue(1).ToString();
                Trn_AccountTo = m_MyReader.GetValue(2).ToString();
                Trn_AccountFrom = m_MyReader.GetValue(3).ToString();
                Trn_Type = m_MyReader.GetValue(4).ToString();
                Trn_PaidDate = _GenObj.GetDateFromText(m_MyReader.GetValue(5).ToString());
                Trn_ClassId = m_MyReader.GetValue(6).ToString();
                Trn_CollectedUser = m_MyReader.GetValue(7).ToString();
                Trn_BatchId = m_MyReader.GetValue(8).ToString();
                Trn_FeeName = m_MyReader.GetValue(9).ToString();
                Trn_PeriodName = m_MyReader.GetValue(10).ToString();
                Trn_CollectionType = m_MyReader.GetValue(11).ToString();
                Trn_RegularFee = m_MyReader.GetValue(12).ToString();
                Trn_PeriodId = m_MyReader.GetValue(13).ToString();
                Trn_FeeId = m_MyReader.GetValue(14).ToString();
                TransType=m_MyReader.GetValue(15).ToString();
                Canceled = m_MyReader.GetValue(16).ToString();
                BalanceAmount = m_MyReader.GetValue(17).ToString();

                //.................................TransationId,PaymentElementId,UserId,Amount,AccountTo,AccountFrom,Type,PaidDate,BillNo,ClassId,StudentName,CollectedUser,BatchId,FeeName,PeriodName,CollectionType,RegularFeeTempId,PeriodId,FeeId,TransType,Canceled,BalanceAmount,
                sql = "INSERT INTO tbltransaction (TransationId,PaymentElementId,UserId,Amount,AccountTo,AccountFrom,Type,PaidDate,BillNo,ClassId,StudentName,CollectedUser,BatchId,FeeName,PeriodName,CollectionType,RegularFee,TempId,PeriodId,FeeId,TransType,Canceled,BalanceAmount) VALUES(" + _TransMaxId + "," + Trn_PaymentElementId + "," + _StudentId + "," + Trn_Amount + "," + Trn_AccountTo + " , " + Trn_AccountFrom + " ,'" + Trn_Type + "','" + Trn_PaidDate.Date.ToString("s") + "','" + _BillId + "'," + Trn_ClassId + ",'" + _StudentName + "','" + Trn_CollectedUser + "'," + Trn_BatchId + ",'" + Trn_FeeName + "','" + Trn_PeriodName + "','" + Trn_CollectionType + "'," + Trn_RegularFee + ",'" + _TempId + "'," + Trn_PeriodId + "," + Trn_FeeId + ","+TransType + ","+Canceled + ","+BalanceAmount+")";
                m_TransationDb.ExecuteQuery(sql);
                //if (Trn_CollectionType == "3") // If advance then insert into student advance table
                //{
                //    sql = "insert into tblstudentfeeadvance(StudentId,FeeName,PeriodName,BatchId,Amount,StudentName,FeeId,PeriodId,TempId) values (" + _Studid + ",'" + Trn_FeeName + "','" + Trn_PeriodName + "'," + _BatchId + "," + Trn_Amount + ",'" + _StudentName + "'," + Trn_FeeId + "," + Trn_PeriodId + ",'" + _TempId + "')";
                //    m_TransationDb.TransExecuteQuery(sql);
                //}

                int _StudId = int.Parse(_Studid);
                if (Trn_CollectionType == "3") // If Advance payment then 
                {
                    SaveAdvanceEntry(_StudId, _TempId, int.Parse(Trn_PeriodId), int.Parse(Trn_FeeId), _BatchId, Trn_FeeName, Trn_PeriodName, double.Parse(Trn_Amount), _StudentName, _BillId);
                }
            }
        }
        sql = "insert into tblbillclearence (StudentId,BillNo,Userid,ClearedDate) values (" + int.Parse(_Studid.ToString()) + ",'" + _BillId + "'," + _UserId + ",'" + System.DateTime.Now.Date.ToString("s") + "')";
        m_TransationDb.ExecuteQuery(sql);

        sql = "delete from tblfeebillclearence where BillNo='" + _OldBillNo + "'";
        m_TransationDb.ExecuteQuery(sql);

        sql = "delete from tbltransactionclearence where BillNo='" + _OldBillNo + "'";
        m_TransationDb.ExecuteQuery(sql);
        

        return _BillId;
    }

    public bool ClearenceEnabled(string _Mode)
    {
        bool valid = false;
        if (_Mode == "Cash")
        {
            valid = false;
        }
        else if (_Mode == "Cheque")
        {
            valid = ClearenceEnabledForCheck();
        }
        else if (_Mode == "Demand Draft")
        {
            valid = ClearenceEnabledForDD();
        }
        else
        {
            valid = false;
        }
        return valid;
    }

    public string GenPendingBill(double _Total, int StudentId, string _PayMode, string _payId, string _bank, string _Date, int _UserId)
    {

        string _billno = "0";
        int _Id = 0;
        if (m_BillPrefix == null)
        {
            m_BillPrefix = GetBillPrefix();
        }
        if (m_TransationDb != null)
        {
            DateTime _BillDate= General.GetDateTimeFromText(_Date);
            DateTime _CreatedDate = DateTime.Now;

            string sql = "insert into tblfeebillclearence (StudentID,TotalAmount,Date,PaymentMode,PaymentModeId,BankName,UserId,CreatedDateTime) values(" + StudentId + "," + _Total + ",'" + _BillDate.ToString("s") + "','" + _PayMode + "','" + _payId + "','" + _bank + "'," + _UserId + ",'" + _CreatedDate.ToString("s") + "')";
            m_TransationDb.TransExecuteQuery(sql);

            sql = "select Id from tblfeebillclearence where StudentID=" + StudentId + " and TotalAmount=" + _Total + " and PaymentModeId='" + _payId + "' and Date='" + _BillDate.ToString("s") + "' order by Id Desc";
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                m_MyReader.Read();
                _Id = int.Parse(m_MyReader.GetValue(0).ToString());
                _billno = m_BillPrefix + _BillDate.Year.ToString() + "-" + _Id.ToString();
                sql = "UPDATE tblfeebillclearence SET BillNo= '" + _billno + "' WHERE Id =" + _Id;
                m_TransationDb.TransExecuteQuery(sql);

            }
            else
            {
                m_TransationDb.TransactionRollback();
            }

        }

        return _billno;
    }

    private void UpdateClearenceStatus(string _PayMode, int _Id, out int _status)
    {
        _status = 0;
        string sql = "";
        if (_PayMode == "Cash")
        {
            sql = "UPDATE tblfeebill SET Clearence= 1 WHERE Id =" + _Id;
            m_TransationDb.TransExecuteQuery(sql);
            _status = 1;
        }
        if (ClearenceEnabledForCheck())
        {

            if (_PayMode == "Cheque")
            {
                sql = "UPDATE tblfeebill SET Clearence= 0 WHERE Id =" + _Id;
                m_TransationDb.TransExecuteQuery(sql);
                _status = 0;
            }

        }
        else
        {
            if (_PayMode == "Cheque")
            {
                sql = "UPDATE tblfeebill SET Clearence= 1 WHERE Id =" + _Id;
                m_TransationDb.TransExecuteQuery(sql);
                _status = 1;
            }
        }
        if (ClearenceEnabledForDD())
        {

            if (_PayMode == "Demand Draft")
            {
                sql = "UPDATE tblfeebill SET Clearence= 0 WHERE Id =" + _Id;
                m_TransationDb.TransExecuteQuery(sql);
                _status = 0;
            }

        }
        else
        {
            if (_PayMode == "Demand Draft")
            {
                sql = "UPDATE tblfeebill SET Clearence= 1 WHERE Id =" + _Id;
                m_TransationDb.TransExecuteQuery(sql);
                _status = 1;
            }
        }
    }

    public void FillPendingTransaction(int _SfeeId, int _StudId, double _Amount, double _Deduction, double _Fine, double Total, double _Arrier, string _BillId)
    {
        CLogging logger = CLogging.GetLogObject();
        logger.LogToFile("FillPendingTransaction", "start Transation", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
        if (m_TransationDb != null)
        {
            string sql = "INSERT INTO tbltransactionclearence(PaymentElementId,UserId,Amount,AccountTo,AccountFrom,Type,PaidDate,BillNo) VALUES(" + _SfeeId + "," + _StudId + "," + Total + ",1 , 2 ,'C','" + System.DateTime.Now.Date.ToString("s") + "','" + _BillId + "')";
            logger.LogToFile("FillPendingTransaction", "Execute Query" + sql, 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
            m_TransationDb.TransExecuteQuery(sql);
            if (_Deduction > 0)
            {
                sql = "INSERT INTO tbltransactionclearence(PaymentElementId,UserId,Amount,AccountTo,AccountFrom,Type,PaidDate,BillNo) VALUES(" + _SfeeId + "," + _StudId + "," + _Deduction + ",3 , 2 ,'C','" + System.DateTime.Now.Date.ToString("s") + "','" + _BillId + "')";
                logger.LogToFile("FillPendingTransaction", "Execute Query" + sql, 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
                m_TransationDb.TransExecuteQuery(sql);

            }
            if (_Fine > 0)
            {
                sql = "INSERT INTO tbltransactionclearence(PaymentElementId,UserId,Amount,AccountTo,AccountFrom,Type,PaidDate,BillNo) VALUES(" + _SfeeId + "," + _StudId + "," + _Fine + ",4 , 2 ,'C','" + System.DateTime.Now.Date.ToString("s") + "','" + _BillId + "')";
                logger.LogToFile("FillPendingTransaction", "Execute Query" + sql, 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
                m_TransationDb.TransExecuteQuery(sql);

            }
            if (_Arrier > 0)
            {
                sql = "UPDATE tblfeestudent SET BalanceAmount = " + _Arrier + ", Status='Arrear' WHERE StudId=" + _StudId + " and SchId =" + _SfeeId;
                logger.LogToFile("FillPendingTransaction", "Execute Query" + sql, 'I', CLogging.PriorityEnum.LEVEL_IMPORTANT, LoginUserName);
                m_TransationDb.TransExecuteQuery(sql);

            }
            else
            {
                sql = "UPDATE tblfeestudent SET BalanceAmount = " + _Arrier + ", Status='Paid' WHERE StudId=" + _StudId + " and SchId =" + _SfeeId;
                logger.LogToFile("FillPendingTransaction", "Execute Query" + sql, 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
                m_TransationDb.TransExecuteQuery(sql);
            }

        }

    }

    public bool CancelPayment(string _BillNo, string _StudentId,string _TempId)
    {
        
         OdbcDataReader MyTempTeader = null;
         bool valid = false;
         int Paymentelementid = -1;
         double Feeamount = 0;
         double Deduction = 0;
         double BalanceAmont = 0;
         double Newbalance = 0;
         string sql = "select DISTINCT PaymentElementId from tbltransactionclearence where (UserId='" + _StudentId + "' or TempId='" + _TempId + "') and BillNo='" + _BillNo + "'";
         MyTempTeader = m_TransationDb.ExecuteQuery(sql);
         if (MyTempTeader.HasRows)
         {
             while (MyTempTeader.Read())
             {
                 Paymentelementid = int.Parse(MyTempTeader.GetValue(0).ToString());
                 if (Paymentelementid != -1)
                 {
                     Feeamount = GetFeeAmount(_BillNo, _StudentId, Paymentelementid, "tbltransactionclearence");
                     Deduction = Getdeduction(_BillNo, _StudentId, Paymentelementid, "tbltransactionclearence");
                     BalanceAmont = GetBalamce(Paymentelementid, _StudentId, 1, _BillNo);//Regular fee
                     Newbalance = (BalanceAmont + Deduction) + Feeamount;
                     NewbalanceUpdated(Newbalance, Paymentelementid, _StudentId);
                 }
             }
             sql = "delete from tblfeebillclearence where BillNo='" + _BillNo + "'";
             m_TransationDb.TransExecuteQuery(sql);

             sql = "delete from tbltransactionclearence where BillNo='" + _BillNo + "'";
             m_TransationDb.TransExecuteQuery(sql);

             valid = true;
         }
        return valid;
    }

    private bool NewbalanceUpdated(double _Newbalance, int _Paymentelementid, string _StudentId)
    {
        bool _valid = false;
        string sql = "update tblfeestudent set BalanceAmount =" + _Newbalance + " , Status='Arrear' where StudId=" + int.Parse(_StudentId) + " and SchId= " + _Paymentelementid + "";
        m_TransationDb.ExecuteQuery(sql);
        _valid = true;
        return _valid;
    }

    private double GetBalamce(int _Paymentelementid, string _StudentId , int _FeeType,string _BillNo)
    {
        double Balance = 0;
        string sql = "";
        if (_FeeType == 1)
        {
            sql = "select BalanceAmount from tblfeestudent where  SchId= " + _Paymentelementid + "";
            if (_StudentId != "-1")
            {
                sql = sql + " and StudId=" + int.Parse(_StudentId);
            }
        }
        else if (_FeeType==2)
        {
            sql = GetSqlStringForJoiningStudent(_StudentId, _BillNo, _Paymentelementid);
        }
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Balance = double.Parse(m_MyReader.GetValue(0).ToString());
        }
        return Balance;
    }


    private string GetSqlStringForJoiningStudent(string _Studentid , string _BillNo,int _PaymentElementId)
    {
        string sql = "select Amount from tbljoining_transaction where UserId=" + _Studentid + " and BillNo='" + _BillNo + "' and PaymentElementId="+_PaymentElementId+"";
        return sql;
    }

    private double Getdeduction(string _BillNo, string _StudentId, int _Paymentelementid, string _Transaction)
    {
        double deduction = 0;
        string sql = "";
        sql = "select Amount from " + _Transaction + " where  BillNo='" + _BillNo + "' and PaymentElementId= " + _Paymentelementid + " and AccountTo=3";
        
        if (_StudentId != "-1")
        {
            sql = sql + " and UserId=" + int.Parse(_StudentId);
        }
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            deduction = double.Parse(m_MyReader.GetValue(0).ToString());
        }
        return deduction;
    }

    private double GetFeeAmount(string _BillNo, string _StudentId, int _Paymentelementid,string _Transaction)
    {
        double Amount = 0;
        string sql = "";
        sql = "select Amount from " + _Transaction + " where BillNo='" + _BillNo + "' and PaymentElementId= " + _Paymentelementid + " and AccountTo=1";
       
        if (_StudentId != "-1")
        {
            sql = sql + " and UserId=" + int.Parse(_StudentId) + "";
        }
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Amount = double.Parse(m_MyReader.GetValue(0).ToString());
        }
        return Amount;
    }

    # endregion 


    # region Joing Fee

    public bool JoiningFee(int i_SelectedFeeId)
    {
        bool valid = false;
        string sql = "select `Type` from tblfeeaccount where Id="+i_SelectedFeeId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            if (m_MyReader.GetValue(0).ToString()=="2")
            {
                valid= true;
            }
        }
        return valid;
    }

    public string GetAmountForClass(int _Standardid, int _FeeId)
    {
        string Amount="0";
        string sql = "select Amount from tbljoining_feeschedule where StandardId=" + _Standardid + " and tbljoining_feeschedule.FeeId="+_FeeId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Amount = m_MyReader.GetValue(0).ToString();  
        }
        return Amount;
    }

    public bool GetSaveAmount(double _Amount, int _StandrdId , int _FeeId)
    {
        bool valid = false;
        string sql = "";
        try
        {
            if (DataExists(_StandrdId, _FeeId))
            {
                sql = "update tbljoining_feeschedule set Amount=" + _Amount + " where StandardId=" + _StandrdId + " and FeeId= " + _FeeId;
            }
            else
            {
                sql = "insert into tbljoining_feeschedule(FeeId,StandardId,Amount) values (" + _FeeId + "," + _StandrdId + ", " + _Amount + ")";
            }
            m_MysqlDb.ExecuteQuery(sql);
            valid = true;
        }
        catch
        {
            valid = false;
        }
        return valid;
    }

    private bool DataExists(int _StandrdId , int _FeeId)
    {
        bool valid = false;
        string sql = "select Amount from tbljoining_feeschedule where StandardId=" + _StandrdId + " and FeeId=" + _FeeId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
           valid = true; 
        }
        return valid;
    }


    public string GenJoinigFeeBill(double _Total, int StudentId, string _PaymentMode, string _Paymentid, string _Bank, string _Pay_Date, int _Userid, int _ClassId,int _Batchid , string _StudentName)
    {
        string _billno = "0";
        if (m_BillPrefix == null)
        {
            m_BillPrefix = GetBillPrefix();
        }
        if (m_TransationDb != null)
        {
            General _GenObj = new General(m_TransationDb);
            DateTime _now = _GenObj.GetDateFromText(_Pay_Date);
            DateTime _CreatedDate = DateTime.Now;
            //int _FeeMaxId = _GenObj.GetTableMaxIdWithCondition("tbljoining_feebill", "Counter", _Batchid, "Batch");
            //int _FeeId = _GenObj.GetTableMaxId("tbljoining_feebill", "Id");
            int _FeeMaxId = _GenObj.GetTableMaxIdWithCondition("tblfeebill", "Counter", _Batchid, "AccYear");
            int _FeeId = _GenObj.GetTableMaxId("tblview_feebill", "TransationId");
            _billno = m_BillPrefix + GetStartYear(_Batchid) + "-" + _FeeMaxId.ToString();
            //_billno = "T" + m_BillPrefix + GetStartYear(_Batchid) + "-" + _FeeMaxId.ToString();
            //string sql = "insert into tbljoining_feebill (Id,StudentID,TotalAmount,Date,PaymentMode,PaymentModeId,BankName,UserId,CreatedDateTime,Class,Batch,Counter,StudentName) values(" + _FeeId + "," + StudentId + "," + _Total + ",'" + _now.ToString("s") + "','" + _PaymentMode + "','" + _Paymentid + "','" + _Bank + "'," + _Userid + ",'" + _CreatedDate.ToString("s") + "'," + _ClassId + "," + _Batchid + "," + _FeeMaxId + ",'" + _StudentName + "')";
            //m_TransationDb.TransExecuteQuery(sql);
            string sql = "insert into tblfeebill (Id,StudentID,TotalAmount,Date,PaymentMode,PaymentModeId,BankName,UserId,CreatedDateTime,Counter,AccYear,ClassID,StudentName,BillNo,RegularFee) values(" + _FeeId + "," + StudentId + "," + _Total + ",'" + _now.ToString("s") + "','" + _PaymentMode + "','" + _Paymentid + "','" + _Bank + "'," + _Userid + ",'" + _CreatedDate.ToString("s") + "'," + _FeeMaxId + "," + _Batchid + "," + _ClassId + ",'" + _StudentName + "','" + _billno + "',1)";
            m_TransationDb.TransExecuteQuery(sql);    
        }
        return _billno;
    }


    public bool ValidJoingTransaction(int _ScheduledFeeId, int _StudentId, double _Amount, int _StandrdId)
    {
        bool valid = false;
        if (m_TransationDb != null)
        {
            string sql = "select Id from tbljoining_feeschedule where Id=" + _ScheduledFeeId + " and StandardId=" + _StandrdId + " and Amount=" + _Amount;
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                valid = true; 
            }
        }
        return valid;
    }

    //public void FillJoinigFeeTransaction(int _ScheduledFeeId, int _StudentId, double Amount, double Total, string _BillId, int _Batchid, int _ClassId, string _StudentName , string _ColectedUser)
    //{
    //    if (m_TransationDb != null)
    //    {
    //        General _GenObj = new General(m_TransationDb);
    //       // int _TransMaxId = _GenObj.GetTableMaxId("tbljoining_transaction", "TransationId");
    //        int _TransMaxId = _GenObj.GetTableMaxId("tblview_transaction", "TransationId");
    //       // string sql = "INSERT INTO tbljoining_transaction(TransationId,PaymentElementId,UserId,Amount,AccountTo,AccountFrom,Type,PaidDate,BillNo,BatchId) VALUES(" + _TransMaxId + "," + _ScheduledFeeId + "," + _StudentId + "," + Total + ",1 , 2 ,'C','" + System.DateTime.Now.Date.ToString("s") + "','" + _BillId + "'," + _Batchid + ")";
    //        string sql = "INSERT INTO tbltransaction(TransationId,PaymentElementId,UserId,Amount,AccountTo,AccountFrom,Type,PaidDate,BillNo,RegularFee,BatchId,ClassId,StudentName,CollectedUser) VALUES(" + _TransMaxId + "," + _ScheduledFeeId + "," + _StudentId + "," + Total + ",1 , 2 ,'C','" + System.DateTime.Now.Date.ToString("s") + "','" + _BillId + "',1," + _Batchid + "," + _ClassId + ",'" + _StudentName + "','" + _ColectedUser + "')";
    //        m_TransationDb.TransExecuteQuery(sql);
    //    }
    //}

    public bool DeleteJoiningFee(int _FeeId, out string _FeeMessage)
    {
        string _message;
        bool _valid = false;
        if (!JoiningFeePaid(_FeeId, out _message))
        {
            try
            {
                CreateTansationDb();
                string sql = "delete from tblfeeaccount where Id=" + _FeeId;
                m_TransationDb.ExecuteQuery(sql);


                sql = "delete from tbljoining_feeschedule where FeeId=" + _FeeId;
                m_TransationDb.ExecuteQuery(sql);


                _message = "Fee is deleted";
                _valid = true;
                EndSucessTansationDb();
            }
            catch
            {
                EndFailTansationDb();
                _message = "Error while deleting. Try later";
                _valid = false;
            }
        }
        _FeeMessage = _message;
        return _valid;
    }

    private bool JoiningFeePaid(int _FeeId, out string _Message)
    {
        bool valid = false;
        _Message = "";
        string sql = "select tbltransaction.TransationId from tbltransaction where tbltransaction.RegularFee=0 AND tbltransaction.PaymentElementId in (select tbljoining_feeschedule.Id from tbljoining_feeschedule where tbljoining_feeschedule.FeeId=" + _FeeId + ")";
        m_MyReader= m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            valid = true;
            _Message = "Cannot delete. Transactions are done for this fee";
        }
        return valid;
    }

    public bool HasJoiningFee()
    {
        bool valid = false;
        string sql = " select tblmoduleactionmap.ActionId from tblmoduleactionmap where tblmoduleactionmap.ActionId=124";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            valid = true;
        }
        return valid;
    }



 
    # endregion


    public DataSet LoadCollectedUser()
    {
        string sql = "SELECT distinct tblview_user.UserName,tblview_user.Id from tblview_feebill inner join tblview_user on tblview_user.Id = tblview_feebill.CollectedUser";
        MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return MyDataSet;
    }




    public bool JoinBillExists(string BillId)
    {
        bool _HasBill = false;
        string sql = "SELECT TransationId FROM tbljoining_transaction WHERE BillNo='" + BillId + "'";
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _HasBill = true;
        }
        return _HasBill;
    }

    public bool CancelBill(string _BillNo, string _StudentId, string _FeeBill, string _transaction, int _FeeType, out int _StudId, out string _PayedDate, out string _Message)
    {
        _PayedDate = "";
        _Message = "";
        OdbcDataReader MyTempTeader = null;
        bool valid = true;
        _StudId = -1;
        CreateTansationDb();
        try
        {
            if (_StudentId == "-1")
                _StudentId = GetStudentId(_BillNo, _transaction);
            _StudId = int.Parse(_StudentId);
            int Paymentelementid = -1;
            double Feeamount = 0;
            double Deduction = 0;
            double BalanceAmont = 0;
            double Newbalance = 0;
            string sql = "";
            if (_FeeType != 2) // if not joining Fee
            {
                sql = "select DISTINCT PaymentElementId,PaidDate from " + _transaction + " where BillNo='" + _BillNo + "'";
                 //and PaymentElementId<>-1
                if (_StudentId != "-1")
                    sql = sql + " and UserId=" + int.Parse(_StudentId);
                MyTempTeader = m_TransationDb.ExecuteQuery(sql);
                if (MyTempTeader.HasRows)
                {
                    _PayedDate = MyTempTeader.GetValue(1).ToString();
                    while (MyTempTeader.Read())
                    {
                        if (MyTempTeader.GetValue(0).ToString() != "-1")
                        {
                            Paymentelementid = int.Parse(MyTempTeader.GetValue(0).ToString());

                            Feeamount = GetFeeAmount(_BillNo, _StudentId, Paymentelementid, _transaction);
                            Deduction = Getdeduction(_BillNo, _StudentId, Paymentelementid, _transaction);
                            BalanceAmont = GetBalamce(Paymentelementid, _StudentId, _FeeType, _BillNo);
                            Newbalance = (BalanceAmont + Deduction) + Feeamount;
                            NewbalanceUpdated(Newbalance, Paymentelementid, _StudentId);
                        }
                    }
                    valid = true;
                }
                else
                {
                    valid = false;
                    _Message = "Cannot cancel the bill " + _BillNo + " . It is not paid in the current academic year";
                }

            }
            if (valid)
            {
                sql = "select DISTINCT PaidDate from " + _transaction + " where BillNo='" + _BillNo + "'";
                //if (_StudentId != "-1")
                //    sql = sql + " and UserId=" + int.Parse(_StudentId);
                MyTempTeader = m_TransationDb.ExecuteQuery(sql);
                if (MyTempTeader.HasRows)
                    _PayedDate = MyTempTeader.GetValue(0).ToString();
                sql = "update  " + _FeeBill + " set Canceled=1 where BillNo='" + _BillNo + "'";
                m_TransationDb.TransExecuteQuery(sql);
                sql = "update  " + _transaction + " set Canceled=1 where BillNo='" + _BillNo + "'";
                m_TransationDb.TransExecuteQuery(sql);
                valid = CancelBillAdvance(_BillNo, out _Message);
            }
          
            if (valid)
            {
                EndSucessTansationDb();
            }
            else
            {
                
                EndFailTansationDb();
            }
        }
        catch(Exception Ex)
        {
            _Message="Unable to Cancel the Bill. Error:"+Ex.Message;
            valid = false;
            EndFailTansationDb();
        }
        return valid;
    }

    private bool CancelBillAdvance(string _BillNo, out string _Message)
    {
        OdbcDataReader _MyReader;
        bool _Valid = true;
        double _Amount, AdvanceBalance;
        int _FeeId, _BatchId, _PeriodId,_StudentId;
        string _FeeName, _PeriodName, _StudentName, _TempId;
        _Message="";
        string sql = "select tblfeeadvancetransaction.Amount,tblfeeadvancetransaction.FeeId, tblfeeadvancetransaction.BatchId, tblfeeadvancetransaction.PeriodId, tblfeeadvancetransaction.FeeName, tblfeeadvancetransaction.PeriodName, tblfeeadvancetransaction.StudentId, tblfeeadvancetransaction.StudentName, tblfeeadvancetransaction.TempId from tblfeeadvancetransaction where tblfeeadvancetransaction.BillNo='" + _BillNo + "' AND tblfeeadvancetransaction.Type=1";
        _MyReader = m_TransationDb.ExecuteQuery(sql);
        if (_MyReader.HasRows)
        {
            while (_MyReader.Read())
            {
                _Amount = double.Parse(_MyReader.GetValue(0).ToString());
                _FeeId = int.Parse(_MyReader.GetValue(1).ToString());
                _BatchId = int.Parse(_MyReader.GetValue(2).ToString());
                _PeriodId = int.Parse(_MyReader.GetValue(3).ToString());
                _FeeName = _MyReader.GetValue(4).ToString();
                _PeriodName = _MyReader.GetValue(5).ToString();
                _StudentId = int.Parse(_MyReader.GetValue(6).ToString()); ;
                _StudentName = _MyReader.GetValue(7).ToString();
                _TempId = _MyReader.GetValue(8).ToString();
                if (CanCancelAdvance(_StudentId,_Amount, _FeeId, _PeriodId, _BatchId, _TempId, ref _Message))
                {
                    AdvanceBalance = GetTotalAdvanceBalance();
                    InserAdvanceTransaction(_StudentId, _StudentName, _FeeName, _PeriodName, _BatchId, _Amount, _FeeId, _PeriodId, _TempId, 2, _BillNo, AdvanceBalance - _Amount);
                }
                else
                {
                    _Valid = false;
                    _Message = "Fee bill cannot be cancelled.Since the advance balance is less than "+_Amount+" for the fee ("+_FeeName+"-"+_PeriodName+").";
                    break;
                }

            }
        }
        return _Valid;
    }

    private bool CanCancelAdvance(int _StudentId, double _Amount, int _FeeId, int _PeriodId, int _BatchId, string _TempId, ref string _Message)
    {
        bool _valid = false;
        OdbcDataReader _MyReader = null;
        double _AdvAmt=0;
        int _advId = 0;
        string sql = "select tblstudentfeeadvance.Amount,tblstudentfeeadvance.Id from tblstudentfeeadvance where tblstudentfeeadvance.TempId='" + _TempId + "' AND tblstudentfeeadvance.PeriodId=" + _PeriodId + " AND tblstudentfeeadvance.FeeId=" + _FeeId + " AND tblstudentfeeadvance.BatchId=" + _BatchId + "  AND tblstudentfeeadvance.StudentId=" + _StudentId;
        if (m_TransationDb == null)
            _MyReader = m_MysqlDb.ExecuteQuery(sql);
        else
            _MyReader = m_TransationDb.ExecuteQuery(sql);

        if (_MyReader.HasRows)
        {
            if (!double.TryParse(_MyReader.GetValue(0).ToString(), out _AdvAmt))
            {
                _AdvAmt = 0;

            }
            else
            {
               _advId=int.Parse(_MyReader.GetValue(1).ToString());
               if (_AdvAmt >= _Amount)
               {
                   if (_AdvAmt > _Amount)
                       sql = "UPDATE tblstudentfeeadvance SET tblstudentfeeadvance.Amount=tblstudentfeeadvance.Amount-" + _Amount + "  WHERE tblstudentfeeadvance.Id =" + _advId;
                   else
                       sql = "delete from tblstudentfeeadvance where Id=" + _advId;

                   if (m_TransationDb == null)
                       m_MysqlDb.ExecuteQuery(sql);
                   else
                       m_TransationDb.ExecuteQuery(sql);
                   _valid = true;

               }
               else
               {
                 
                   _valid = false;
               }
            }

        }
        return _valid;
    }

    public string GetTotalAmount(string _BillNo,string _FeeType)
    {
        string Amount = "0";
        string sql = "select sum(Amount) from tblview_transaction where BillNo='" + _BillNo + "' and AccountTo<>3";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Amount = m_MyReader.GetValue(0).ToString();
        }
        return Amount;
    }

    private string GetStudentId(string _BillNo, string _Transaction)
    {
        string Studentid = "-1";
        string sql = "select UserId from " + _Transaction + " where BillNo='" + _BillNo + "'";
        m_MyReader = m_TransationDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Studentid = m_MyReader.GetValue(0).ToString();
        }
        return Studentid;
    }


    public void WriteCancelLog(string _BillNo, int _StudentId, int _Billtype, int _UserId, string _PayedDate, string _Reason)
    {
       
        DateTime _Date = DateTime.Now;
        if (_PayedDate != "")
        {
            DateTime _PaymentDate = DateTime.Parse(_PayedDate);
            string sql = "insert into tblbillcancel(BillNo,StudentId,FeeType,CancelDate,PaymentDate,CanceledBy,Reason) values('" + _BillNo + "'," + _StudentId + "," + _Billtype + ",'" + _Date.Date.ToString("s") + "','" + _PaymentDate.Date.ToString("s") + "'," + _UserId + ",'" + _Reason + "')";
            m_MysqlDb.ExecuteQuery(sql);
        }
      
    }


    public DataSet GetDailyAbsFeeReport(string _From, string _To, DataSet _MyClass)
    {
        DataSet AbsReport = new DataSet();
        DataTable dt;
        DataRow dr;
        int i = 1;
        double TotalAmt = 0;
        double Amt_ExcludingOtherFee = 0;
        int TotalStudents = 0;
        AbsReport.Tables.Add(new DataTable("FeeReport"));
        dt = AbsReport.Tables["FeeReport"];
        dt.Columns.Add("Sl.No");
        dt.Columns.Add("Class");
        dt.Columns.Add("No.of students", typeof(int));
        //dt.Columns.Add("Amount(Excluding Fine)", typeof(double));
        dt.Columns.Add("Amount(Excluding Other Fee)", typeof(double));
        dt.Columns.Add("Total Amount", typeof(double));
        foreach (DataRow Dr_Class in _MyClass.Tables[0].Rows)
        {
            dr = AbsReport.Tables[0].NewRow();
            dr["Sl.No"] = i;
            dr["Class"] = Dr_Class[1].ToString();
            dr["No.of students"] = GetCountOfPaidStudents(Dr_Class[0].ToString(), _From, _To);
            TotalStudents = TotalStudents + int.Parse(dr["No.of students"].ToString());

            //dr["Amount(Excluding Fine)"] = GetAmtExcludingFineForDay(Dr_Class[0].ToString(), _From, _To);
            //Amt_ExcludingFine = Amt_ExcludingFine + double.Parse(dr["Amount(Excluding Fine)"].ToString());


            dr["Amount(Excluding Other Fee)"] = GetAmtExcludingOtherFeeForDay(Dr_Class[0].ToString(), _From, _To);
            Amt_ExcludingOtherFee = Amt_ExcludingOtherFee + double.Parse(dr["Amount(Excluding Other Fee)"].ToString());

            dr["Total Amount"] = GetTotalAmtForDay(Dr_Class[0].ToString(), _From, _To);
            TotalAmt = TotalAmt + double.Parse(dr["Total Amount"].ToString());
            AbsReport.Tables[0].Rows.Add(dr);
            i++;
        }
        dr = AbsReport.Tables[0].NewRow();
        dr["Sl.No"] = "";
        dr["Class"] = "Total";
        dr["No.of students"] = TotalStudents;
        //dr["Amount(Excluding Fine)"] = Amt_ExcludingFine;
        dr["Amount(Excluding Other Fee)"] = Amt_ExcludingOtherFee;
        dr["Total Amount"] = TotalAmt;
        AbsReport.Tables[0].Rows.Add(dr);
        return AbsReport;
    }

    private string GetAmtExcludingOtherFeeForDay(string _ClassID, string _From, string _To)
    {
        double Day = 0;
        DateTime _FDate = General.GetDateTimeFromText(_From);
        DateTime _TDate = General.GetDateTimeFromText(_To);
        string sql = "select  sum(tblview_transaction.Amount) from tblview_transaction where tblview_transaction.FeeId<>-1 and AccountTo<>3 and tblview_transaction.ClassId=" + _ClassID + " and tblview_transaction.PaidDate>='" + _FDate.Date.ToString("s") + "' and tblview_transaction.PaidDate<='" + _TDate.Date.ToString("s") + "' and tblview_transaction.Canceled=0 ";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            double.TryParse(m_MyReader.GetValue(0).ToString(), out Day);
        }
        return Day.ToString();
    }

    private string GetTotalAmtForDay(string _ClassID, string _From , string _To)
    {
        double Day = 0;
        DateTime _FDate = General.GetDateTimeFromText(_From);
        DateTime _TDate = General.GetDateTimeFromText(_To);
        string sql = "select  sum(tblview_transaction.Amount) from tblview_transaction where AccountTo<>3 and tblview_transaction.ClassId=" + _ClassID + " and tblview_transaction.PaidDate>='" + _FDate.Date.ToString("s") + "' and tblview_transaction.PaidDate<='" + _TDate.Date.ToString("s") + "' and tblview_transaction.Canceled=0 ";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            double.TryParse(m_MyReader.GetValue(0).ToString(), out Day);
        }
        return Day.ToString();
    }


    private string GetAmtExcludingFineForDay(string _ClassID, string _From, string _To)
    {
        double Day = 0;
        DateTime _FDate = General.GetDateTimeFromText(_From);
        DateTime _TDate = General.GetDateTimeFromText(_To);
        string sql = "select  sum(tblview_transaction.Amount) from tblview_transaction where tblview_transaction.AccountTo=1 and tblview_transaction.ClassId=" + _ClassID + " and tblview_transaction.PaidDate>='" + _FDate.Date.ToString("s") + "' and tblview_transaction.PaidDate<='" + _TDate.Date.ToString("s") + "' and tblview_transaction.Canceled=0 ";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            double.TryParse(m_MyReader.GetValue(0).ToString(), out Day);
        }
        return Day.ToString();
    }

    private string GetCountOfPaidStudents(string _ClassID, string _From, string _To)
    {
        int Day = 0;
        DateTime _FDate = General.GetDateTimeFromText(_From);
        DateTime _TDate = General.GetDateTimeFromText(_To);
        //string sql = "select  count( distinct tblview_studentclassmap.StudentId) from tblview_feebill inner join  tblview_studentclassmap on tblview_studentclassmap.StudentId = tblview_feebill.StudentID where tblview_studentclassmap.ClassId=" + _ClassID + " and tblview_feebill.`Date`>='" + _FDate.Date.ToString("s") + "' and tblview_feebill.`Date`<='" + _TDate.Date.ToString("s") + "'";
        string sql = "select  count( distinct tblview_feebill.StudentID) from tblview_feebill where tblview_feebill.ClassId=" + _ClassID + " and tblview_feebill.`Date`>='" + _FDate.Date.ToString("s") + "' and tblview_feebill.`Date`<='" + _TDate.Date.ToString("s") + "' and tblview_feebill.Canceled=0 ";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int.TryParse(m_MyReader.GetValue(0).ToString(), out Day);
        }
        return Day.ToString();
    }

    # region Edit All Class Fees
    public void UpdateAllClassFee(int _StudentId, int _SchedId, double _NewAmt, double _NewBalance)
    {
        string _Status = "";
        string sql="";
        if (_NewBalance == 0)
            _Status = "Paid";
        else if (_NewBalance > 0)
            _Status = "Scheduled";
        if (Feescheduled(_SchedId, _StudentId))
        {
            sql = "update tblfeestudent set Amount=" + _NewAmt + " , BalanceAmount=" + _NewBalance + " , `Status`='" + _Status + "' where SchId=" + _SchedId + " and StudId=" + _StudentId;
            m_TransationDb.ExecuteQuery(sql);
        }
        else
        {
            ScheduleStudFee(_StudentId, _SchedId, _NewAmt, _Status);
            //sql = "insert into tblfeestudent (SchId,StudId,Amount,BalanceAmount,Status) values (" + _SchedId + "," + _StudentId + "," + _NewAmt + "," + _NewBalance + ",'" + _Status + "')";
            //m_TransationDb.ExecuteQuery(sql);
        }

    }

    private bool Feescheduled(int _SchedId, int _StudentId)
    {
        bool _Valid = false;
        string sql = "select tblfeestudent.Id from tblfeestudent where tblfeestudent.SchId=" + _SchedId + " and tblfeestudent.StudId=" + _StudentId;
        m_MyReader = m_TransationDb.SelectTansExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _Valid = true;
        }
        m_MyReader.Close();
        return _Valid;
    }

    public int GetFeeScheduleId(int _FeeId, string _ClassId, string _PeriodId, int _BatchId)
    {
        int SchId = 0;
        string sql = "select Id  from tblfeeschedule where FeeId = " + _FeeId + " and BatchId = " + _BatchId + " and ClassId = " + _ClassId + " and PeriodId ="+_PeriodId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int.TryParse(m_MyReader.GetValue(0).ToString(), out SchId);
        }
        return SchId;
    }

    # endregion

    public int GetClassID(int _StudentId)
    {
        int Class = 0;
        string sql = "SELECT tblstudent.LastClassId from tblstudent where tblstudent.Id=" + _StudentId ;
        if (m_TransationDb != null)
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
        else
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Class =int.Parse( m_MyReader.GetValue(0).ToString());
        }
        m_MyReader.Close();
        return Class;
    }

    public bool GetBillType(ref int _Value, ref string _PageName ,out bool _Pdf)
    {
        bool _value= false;
        _Pdf = false;
        string sql = "select tblfeebilltype.Id , tblfeebilltype.PageName,IsPDF from tblconfiguration inner join tblfeebilltype on tblconfiguration.Value = tblfeebilltype.Id where tblconfiguration.Id=21";
        if (m_TransationDb != null)
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
        else
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        
        if (m_MyReader.HasRows)
        {
           _value = true;
           _Value = int.Parse(m_MyReader.GetValue(0).ToString());
           _PageName = m_MyReader.GetValue(1).ToString();
           if (m_MyReader.GetValue(2).ToString() == "0")
               _Pdf = false;
           else
               _Pdf = true;

        }
        return _value;
    }

    public bool ValidAmount(int _SchdId, double _BalanceAmount , int _StudentId)
    {
        bool _value = false;
        double Bal = 0;
        string sql = "select tblfeestudent.BalanceAmount from tblfeestudent where tblfeestudent.SchId=" + _SchdId + " and tblfeestudent.StudId=" + _StudentId + "";
         if (m_TransationDb != null)
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
        else
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
         if (m_MyReader.HasRows)
         {
             double.TryParse(m_MyReader.GetValue(0).ToString(), out Bal);
             if (_BalanceAmount == Bal)
                 return true;
         }
         return _value;
    }

    # region OtherFee

    public void SaveOtherFee(string _FeeName, string _Description, int _Refundable)
    {
        string sql = "insert into tblfeeothermaster(Name,Description,Refundable) values ('" + _FeeName + "','" + _Description + "'," + _Refundable + ") ";
        m_MysqlDb.ExecuteQuery(sql);
    }

    public bool OtherFeeExists(string _FeeName)
    {
        bool _Valid = false;
        string sql = "select Name from tblfeeothermaster where Name='" + _FeeName + "'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _Valid = true;
        }
        return _Valid;
    }

    public DataSet GetOtherfeeList()
    {
        DataSet Fees = new DataSet();
        string sql = "select Id,Name,Description from tblfeeothermaster";
        Fees = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return Fees;
    }

    # endregion


    # region FeeBillGenFunctions

    public void GetStudentDetails(string _BillNo, out string _StudentName, out string _AdmissionNo, out  string _RollNumber , out string _ClassName)
    {
        bool _RegularStudent= false;
        _StudentName="";
        _AdmissionNo = "";
        string _StudentId = "-1";
        _RollNumber = "Not allotted";
        _ClassName = "";
        string _ClassId = "0";
        //SELECT   tblview_studentclassmap.RollNo, tblclass.ClassName from tblview_student inner join tblview_feebill on tblview_feebill.StudentID= tblview_student.Id inner join tblview_studentclassmap on tblview_studentclassmap.StudentId= tblview_student.Id and tblview_studentclassmap.BatchId=tblview_feebill.BatchId inner join tblclass on tblclass.Id = tblview_studentclassmap.ClassId where  tblview_feebill.BillNo= '" + BillNo + "'
        string sql = "select tblview_feebill.StudentID,tblview_feebill.StudentName,tblview_feebill.TempId from tblview_feebill where tblview_feebill.BillNo='" + _BillNo + "'";
        if(m_TransationDb!=null)
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
        else
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);

        if (m_MyReader.HasRows)
        {
            _StudentId = m_MyReader.GetValue(0).ToString();
            if (_StudentId == "0")
                _RegularStudent = false;
            _StudentName = m_MyReader.GetValue(1).ToString();
            _AdmissionNo = m_MyReader.GetValue(2).ToString();
        }
        if (!_RegularStudent)
        {
            sql = "SELECT tblview_student.AdmitionNo from tblview_student where  tblview_student.Id= " + _StudentId + "";
            if (m_TransationDb != null)
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            else
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);


            if (m_MyReader.HasRows)
            {
                _AdmissionNo = m_MyReader.GetValue(0).ToString();
            }
        }
        sql = "select Id,ClassName from tblview_feebill inner join  tblclass on tblclass.Id = tblview_feebill.ClassId where tblview_feebill.BillNo='" + _BillNo + "'";
 
        if (m_TransationDb != null)
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
        else
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);


        if (m_MyReader.HasRows)
        {
            _ClassId = m_MyReader.GetValue(0).ToString();
            _ClassName = m_MyReader.GetValue(1).ToString();
        }

        sql = "select RollNo from tblview_studentclassmap where ClassId=" + _ClassId + " and StudentId=" + _StudentId;
    
        if (m_TransationDb != null)
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
        else
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);


        if (m_MyReader.HasRows)
        {
            _RollNumber = m_MyReader.GetValue(0).ToString();
        }
    }

    # endregion 


   

    public bool HasNextBatchSchedule()
    {
        bool _valid = false;
        string _Value;
        string sql = "SELECT Value FROM tblconfiguration WHERE id =22";
        if (m_TransationDb != null)
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
        else
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _Value = m_MyReader.GetValue(0).ToString();
            if (_Value == "1")
                _valid= true;
        }
        return _valid;
        
    }

    public int GetBillType(string _BillNo)
    {
        int BillType = 0;
        string sql = "select RegularFee from tblview_feebill where BillNo='" + _BillNo + "'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int.TryParse(m_MyReader.GetValue(0).ToString(), out BillType);
        }
        if (BillType == 0) // If Joining Fee then asssigning 2
            BillType = 2;
        return BillType;
    }

    public bool IsRegularBill(string BillNo)
    {
        bool _valid = false;
        int BillType = 0;
        string sql = "select RegularFee from tblview_feebill where BillNo='" + BillNo + "'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int.TryParse(m_MyReader.GetValue(0).ToString(), out BillType);
            if (BillType == 1)
                _valid=true;
        }
       
        return _valid;
    }

    #region  COMMON
    public string Convert_Number_To_Words(int l)
    {
        int _temp = l;
        if (l < 0)
        {
            l = l * -1;
        }
        int r = 0, i = 0;
        string Words = "";
        string[] a = { " One ", " Two ", " Three ", " Four ", " Five ", " Six ", " Seven ", " Eight ", " Nine ", " Ten " };
        string[] b = { " Eleven ", " Twelve ", " Thirteen ", " Fourteen ", " Fifteen ", " Sixteen ", " Seventeen ", " Eighteen ", " Nineteen " };
        string[] c = { "Ten", " Twenty ", " Thirty ", " Fourty ", " Fifty ", " Sixty ", " Seventy ", " Eighty ", " Ninety ", " Hundred " };
        try
        {
            if (l > 99999)
            {
                r = l / 100000;
                if (r == 10 || r == 20 || r == 30 || r == 40 || r == 50 || r == 60 || r == 70 || r == 80 || r == 90 || r == 100)
                {
                    r = r / 10;
                    Words = Words + c[r - 1] + " Lakh ";
                }
                else if (r > 0 && r < 10)
                {
                    Words += a[r - 1] + " Lakh ";
                }
                else if (r > 10 && r < 20)
                {
                    r = r % 10;
                    Words = b[r - 1] + " Lakh ";
                }
                else
                {
                    i = r / 10;
                    r = r % 10;
                    Words = Words + c[i - 1] + a[r - 1] + " Lakh ";
                }
                l = l % 100000;
            }
            if (l > 9999)
            {
                r = l / 1000;
                if (r == 10 || r == 20 || r == 30 || r == 40 || r == 50 || r == 60 || r == 70 || r == 80 || r == 90 || r == 100)
                {
                    r = r / 10;
                    Words = Words + c[r - 1] + " Thousand ";
                }
                else if (r > 10 && r < 20)
                {
                    r = r % 10;
                    Words = Words + b[r - 1] + "Thousand ";
                }
                else
                {
                    i = r / 10;
                    r = r % 10;
                    Words = Words + c[i - 1] + a[r - 1] + " Thousand ";
                }
                l = l % 1000;
            }
            if (l > 999)
            {
                if (l == 1000)
                {
                    Words += " Thousand ";
                    l = 0;
                }
                else
                {
                    r = l / 1000;
                    Words += a[r - 1] + " Thousand ";
                    l = l % 1000;
                }
            }

            if (l > 99)
            {
                if (l == 100)
                {
                    Words += " Hundred ";
                    l = 0;
                }
                else
                {
                    r = l / 100;
                    Words += a[r - 1] + " Hundred ";
                    l = l % 100;
                }
            }
            if (l > 10 && l < 20)
            {
                r = l % 10;
                if (Words == "")
                    Words += b[r - 1];
                else
                    Words += " And " + b[r - 1];
            }
            if (l > 19 && l <= 100)
            {
                r = l / 10;
                i = l % 10;
                if (Words == "")
                {
                    if (i != 0)
                        Words += c[r - 1] + a[i - 1];
                    else
                        Words += c[r - 1];
                }
                else
                {
                    if (i != 0)
                        Words += " And " + c[r - 1] + a[i - 1];
                    else
                        Words += " And " + c[r - 1];
                }
            }
            if (l > 0 && l <= 10)
            {
                if (Words == "")
                    Words += a[l - 1];
                else
                    Words += " And " + a[l - 1];
            }
            if (_temp == 0)
            {
                Words = "Zero";
            }
            else if (_temp < 0)
            {
                Words = "(-ve) " + Words;
            }
            if (Words != "")
                Words = Words + " Only.";

            return Words;
        }
        catch
        {
            return "Error in Conversion";
        }
    }
    #endregion COMMON



    public DataSet GetAllFeeAmount()
    {
        string sql = "";      
        DataSet AmountDs = new DataSet();
        sql = "select tblfeeaccount.id,AccountName,tblfeeaccount.FrequencyId, tblfeeaccount.`Type` from tblfeeaccount where `status`=1 and tblfeeaccount.id<>-1";
      AmountDs= m_MysqlDb.ExecuteQueryReturnDataSet(sql);     
      return AmountDs;

    }

    public void SaveFineAmountToDatabase(double fineamount, int Feeid, int Typeid, int freqncyId,int finetype,int finedate,int fineduration)
    {
        string sql1 = "",sql="";
        OdbcDataReader feeidreader = null;
        sql1 = "select Id from tblfine where tblfine.FeeId="+Feeid+"";
        feeidreader = m_MysqlDb.ExecuteQuery(sql1);
        if (feeidreader.HasRows)
        {
            sql = "UPDATE  tblfine SET Amount=" + fineamount + ",FineAmounttype=" + finetype + ",Finedate=" + finedate + ",FineDuration="+fineduration+" WHERE Id=" + feeidreader.GetValue(0).ToString() + "";
            m_MysqlDb.ExecuteQuery(sql);
        }
        else
        {
            sql = "INSERT INTO tblfine(Amount,Frequency,Type,FeeId,FineAmounttype,Finedate,FineDuration) values(" + fineamount + "," + freqncyId + "," + Typeid + "," + Feeid + "," + finetype + "," + finedate + "," + fineduration + ")";
            m_MysqlDb.ExecuteQuery(sql);
        }
    }

    public DataSet GetBankStatementReport(DateTime _fromdate, DateTime _Todate, int _classid, int _feetype, int _userid,int _paymentmode)
    {
        string sql = "";
        DataSet ReportDS = CreateDynamicDS();
        DataRow dr;
        OdbcDataReader ReportReader = null;
        //sql = "select tblfeebill.Id, tblfeebill.BillNo as `Bill No`, tblfeebill.BankName as `Bank Name`, Date_Format(tblfeebill.`Date`,'%d-%m-%Y') as `Paid Date`, tblfeebill.PaymentMode as `Mode Of Payment`, tblfeebill.StudentName as `Student Name`, tblfeebill.TotalAmount as `Amount`, tblfeebill.PaymentModeId as `DD/Cheque No`,tblfeebill.StudentID, tblclass.ClassName as `Class`, tblstandard.Name as `Standard`, tblbatch.BatchName as `Batch`, tblview_student.AdmitionNo as `Admission No`,tblfeebill.OtherReference as `Other Reference` from tblfeebill inner join tblclass on tblclass.Id= tblfeebill.ClassID inner join tblstandard on    tblstandard.Id= tblclass.Standard inner join  tblview_student on tblview_student.Id= tblfeebill.StudentID inner join tblbatch on tblfeebill.AccYear= tblbatch.Id   WHERE  tblfeebill.`Date` BETWEEN '" + _fromdate.ToString("s") + "' and '" + _Todate.ToString("s") + "' and tblfeebill.Canceled=0 ";
        sql = "select tblfeebill.Id, tblfeebill.BillNo as `Bill No`, tblfeebill.BankName as `Bank Name`, Date_Format(tblfeebill.`Date`,'%d-%m-%Y') as `Paid Date`, tblfeebill.PaymentMode as `Mode Of Payment`, tblfeebill.StudentName as `Student Name`, tblfeebill.TotalAmount as `Amount`, tblfeebill.PaymentModeId as `DD/Cheque No`,tblfeebill.StudentID, tblclass.ClassName as `Class`, tblbatch.BatchName as `Batch`,tblfeebill.OtherReference as `Other Reference` from tblfeebill inner join tblclass on tblclass.Id= tblfeebill.ClassID inner join tblbatch on tblfeebill.AccYear= tblbatch.Id  WHERE  tblfeebill.`Date` BETWEEN '" + _fromdate.ToString("s") + "' and '" + _Todate.ToString("s") + "' and tblfeebill.Canceled=0";
        if (_classid > 0)
        {
            sql = sql + " and tblfeebill.ClassID=" + _classid + "";
        }
        if (_feetype == 1)
        {
            sql = sql + " and tblfeebill.RegularFee=1";
        }
        if (_feetype == 2)
        {
            sql = sql + " and tblfeebill.RegularFee=0";
        }
        if (_userid > 0)
        {
            sql = sql + " and tblfeebill.UserId=" + _userid + "";
        }
        if (_paymentmode == 1)
        {
            sql = sql + " and tblfeebill.PaymentMode='Cash'";
        }
        if (_paymentmode == 2)
        {
            sql = sql + " and tblfeebill.PaymentMode='Demand Draft'";
        }
        if (_paymentmode == 3)
        {
            sql = sql + " and tblfeebill.PaymentMode='Cheque'";
        }
        if (_paymentmode == 4)
        {
            sql = sql + " and tblfeebill.PaymentMode='NEFT'";
        }

        //sql = sql + "  order by `date` desc";
        //sai changed for based on student roll no 
        //sql = sql + "  order by tblview_student.ClassId ASC ,tblview_student.RollNo ASC,`date` desc ";
        ReportReader = m_MysqlDb.ExecuteQuery(sql);
        int slno = 1;
        if (ReportReader.HasRows)
        {
            while (ReportReader.Read())
            {
                dr = ReportDS.Tables["DynamicDt"].NewRow();
                dr["S.No"] = slno.ToString();
                foreach (DataColumn dc in ReportDS.Tables["DynamicDt"].Columns)
                {
                    for (int i = 0; i < ReportReader.FieldCount; i++)
                    {
                        if (dc.ToString() == ReportReader.GetName(i).ToString())
                        {
                            dr[dc] = ReportReader.GetValue(i).ToString();
                        }
                    }

                }

                ReportDS.Tables["DynamicDt"].Rows.Add(dr);
                slno++;              
            }
        }

        ReportReader.Close();
        return ReportDS;

    }

    private DataSet CreateDynamicDS()
    {
        string sql = "";
        OdbcDataReader receiptcolumnreader = null;
        DataSet Dynamicds = new DataSet();
        DataTable dt;
        Dynamicds.Tables.Add("DynamicDt");
        dt = Dynamicds.Tables["DynamicDt"];
        sql = "Select DisplayName from tblbankstatement  where IsNeeded=1";
        receiptcolumnreader = m_MysqlDb.ExecuteQuery(sql);
        if (receiptcolumnreader.HasRows)
        {
            while (receiptcolumnreader.Read())
            {
                dt.Columns.Add(receiptcolumnreader.GetValue(0).ToString());
            }
        }

        return Dynamicds;

    }

    public int getstudentid(string StudentName, string AdmitionNo, string ClassId, int BatchId)
    {
        int StudentId = 0;
        string sql = string.Concat(new object[]
		{
			"select tblview_student.id from tblview_student inner join tblview_studentclassmap ON tblview_studentclassmap.StudentId=tblview_student.id where tblview_studentclassmap.BatchId=",
			BatchId,
			" AND  tblview_studentclassmap.ClassId=",
			ClassId,
			" AND tblview_student.AdmitionNo='",
			AdmitionNo,
			"' AND tblview_student.StudentName LIKE '%",
			StudentName,
			"%'"
		});
        this.m_MyReader = this.m_MysqlDb.ExecuteQuery(sql);
        if (this.m_MyReader.HasRows)
        {
            int.TryParse(this.m_MyReader.GetValue(0).ToString(), out StudentId);
        }
        return StudentId;
    }
    public double GetTotalFeeAmount_Student(string _StudentId)
    {
        double _amt = 0.0;
        string sql = "select SUM(tblfeestudent.BalanceAmount) from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId where tblfeestudent.StudId=" + _StudentId + " and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion' ";
        this.m_MyReader = this.m_MysqlDb.ExecuteQuery(sql);
        if (this.m_MyReader.HasRows)
        {
            double.TryParse(this.m_MyReader.GetValue(0).ToString(), out _amt);
        }
        return _amt;
    }
    public int GetFeeImport_ColumnCount()
    {
        int _count = 0;
        string sql = "SELECT Value FROM tblconfiguration WHERE Name ='Fee import Column Count'";
        if (this.m_TransationDb != null)
        {
            this.m_MyReader = this.m_TransationDb.ExecuteQuery(sql);
        }
        else
        {
            this.m_MyReader = this.m_MysqlDb.ExecuteQuery(sql);
        }
        if (this.m_MyReader.HasRows)
        {
            int.TryParse(this.m_MyReader.GetValue(0).ToString(), out _count);
        }
        return _count;
    }


    public void StoreFeeTransactions(string StudentId, string StudentName, double _amount, int UserId, int CurrentBatchId, int ClassId, string UserName)
    {
        bool _BillDone = false;
        string PaidDate = General.GerFormatedDatVal(DateTime.Now.Date);
        string _BillId = "";
        double _RemainingSum = _amount;
        string sql = "select tblfeestudent.SchId FeeScheduleId, tblfeestudent.Id FeeStudentId, tblfeeaccount.AccountName,tblbatch.BatchName, tblperiod.Period, tblfeestudent.`Status`, tblfeestudent.BalanceAmount, tblfeeschedule.LastDate  AS 'LastDate' , tblperiod.Id as PeriodId , tblfeeaccount.Id as FeeId , tblbatch.Id as BatchId,tblfeeschedule.Duedate AS 'Duedate' from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblstudent on tblstudent.Id = tblfeestudent.StudId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId where tblfeestudent.StudId=" + StudentId + " and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion' and tblstudent.Status=1 ORDER BY tblfeeschedule.Duedate,tblperiod.`Order`";
        this.MyDataSet = this.m_TransationDb.ExecuteQueryReturnDataSet(sql);
        if (this.MyDataSet != null && this.MyDataSet.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in this.MyDataSet.Tables[0].Rows)
            {
                int ScheduleId = int.Parse(dr[0].ToString());
                int FeeStudentId = int.Parse(dr[1].ToString());
                string FeeName = dr[2].ToString();
                string BatchName = dr[3].ToString();
                string PeriodName = dr[4].ToString();
                string Status = dr[5].ToString();
                double BalanceAmount = double.Parse(dr[6].ToString());
                DateTime LastDate = DateTime.Parse(dr[7].ToString());
                int PeriodId = int.Parse(dr[8].ToString());
                int FeeId = int.Parse(dr[9].ToString());
                int TrnsBatchId = int.Parse(dr[10].ToString());
                DateTime Duedate = DateTime.Parse(dr[11].ToString());
                double Amount = BalanceAmount;
                double Deduction = 0.0;
                double Fine = 0.0;
                if (!_BillDone)
                {
                    _BillId = this.GenBill(_amount, int.Parse(StudentId), "Cash", "", "", PaidDate, UserId, CurrentBatchId, ClassId.ToString(), StudentName, 1, "", "", "tblfeebill");
                    _BillDone = true;
                }
                double Total;
                double ArrierAmount;
                if (_RemainingSum < BalanceAmount)
                {
                    Total = _RemainingSum;
                    ArrierAmount = BalanceAmount - _RemainingSum;
                }
                else
                {
                    Total = BalanceAmount;
                    ArrierAmount = 0.0;
                }
                this.FillTransaction(ScheduleId, int.Parse(StudentId), Amount, Deduction, Fine, Total, ArrierAmount, _BillId, ClassId.ToString(), StudentName, UserName, TrnsBatchId, FeeName, PeriodName, 1, 1, "", "tbltransaction", PeriodId, FeeId, PaidDate);
                _RemainingSum -= BalanceAmount;
                if (_RemainingSum <= 0.0)
                {
                    break;
                }
            }
        }
    }




    public bool CanChangeFeeBillDate()
    {
        bool _CanChange = false;
        int _value1 = 0;
        string sql = "SELECT tblconfiguration.Value FROM tblconfiguration WHERE tblconfiguration.Module='Fee Collection' AND tblconfiguration.Name='FeebillDateChanging'";
        this.m_MyReader = this.m_MysqlDb.ExecuteQuery(sql);
        if (this.m_MyReader.HasRows)
        {
            int.TryParse(this.m_MyReader.GetValue(0).ToString(), out _value1);
            if (_value1 == 1)
            {
                _CanChange = true;
            }
        }
        return _CanChange;
    }
}