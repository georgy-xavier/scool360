using System;
using System.Web;
//using System.Web.Services;
//using System.Web.Services.Protocols;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.Text;
using WinBase;


using System.Collections;

using System.Linq;



using System.Xml.Linq;


using System.IO;




public class KnowinUser:KnowinGen
{
    private string m_stConnection = "";
    private KnowinGroup m_KnowinGrp = null;
    private KnowinRole m_KnowinRole = null;
    private KnowinUserAction m_knowinUserAction = null;
    public MysqlClass m_TransationDb = null;
    private SMSManager m_smsMng = null;
    private FeeManage m_FeeMng = null;
    private ExamManage m_ExamMng = null;
    private ClassOrganiser m_ClassMng = null;
    private StudentManagerClass m_StudMang = null;
    private StaffManager m_StaffMng = null;
    private EmailManager m_EmailMng = null;    
    private ConfigManager m_ConfigMng = null;
    private WinErSearch m_SearchMng = null;
    private Incident m_IncidentMng = null;
    private Attendance m_AttendanceMng = null;
    private LibraryManagClass m_LibMng = null;
    private Reports m_Report = null;
    private TimeTable m_TimeTable = null;
    private Payroll m_Payroll = null;
    private Inventory m_Inventory = null;
    private HouseManager m_House = null;
    private GroupManager m_Group = null;
    private TransportationClass m_TransMng = null;
    public MysqlClass m_MysqlDb = null;
    private KnowinEncryption m_MyEncrypt = null;
    public DBLogClass m_DbLog = null;
    private OdbcDataReader m_MyReader = null;
    private String m_MenuString = "";
    private String m_SchoolMenuString = "";
    private DataSet m_myAssoGroups=null;
    private DataSet m_myAssoClass = null;
    private string m_ExcelHeader = "";
    private bool m_HaveStudRight=false;
    private bool m_HaveSchoolRight = false;
    private string m_myAssoGroupString = "";
    private bool m_Login = false;
    private string m_Name = null;
    private int m_Role = 0;
    private int m_myId = 0;
    private int m_CurrtentBatchId = 0;
    private string m_CurrtentBatchName = null;
    private string m_HomeMenu = "";
    private string m_ParentLoginUrl = "";
    private string m_SchoolName = "";
    public LicenceDetails m_LicenseObject;
    private int m_selectedModule = 1;
    public string m_LeavMenuStr = "";
    public string m_RelativePath = "";
    private int m_ParentId;
    private string m_ParentName;
    public int m_StudentId;
    private string m_StudentName;
    private string m_StudentImage;
    private string m_SchoolLogo;
    private int m_BatchId;
    private string m_BatchName;
   // private string m_SchoolName;
    private string m_ClassName;
    private int m_ClassId;
   
    private string m_RollN0;
    private int m_Age;
    private string m_AdmissionN0;
    private int m_Point;
    private int m_rating;
    public int m_StudType;

   
    public SchoolClass SchoolObject = null;
    public KnowinUser(string Connection,string _FilePath,string _RelativePath)
    {

        m_FilePath = _FilePath;
        m_RelativePath =_RelativePath;
        if (Connection != null)
        {
            m_stConnection = Connection;
        }
        CLogging logger = CLogging.GetLogObject();
        if (m_MyODBCConn == null)
        {
            m_MyODBCConn = new OdbcConnection(m_stConnection);
            m_MyODBCConn.Open();
        }
        if (m_MysqlDb == null)
        {
            m_MysqlDb = new MysqlClass(this);
        }
       
        if (m_DbLog == null)
        {
            m_DbLog = new DBLogClass(m_MysqlDb);
        }
    }
   
    //public ParentInfoClass(int Parentid, string _ParentName, string _StudentName, int _StudentId, string _SchoolName, string _SchoolLogo)
    //{
    //    m_ParentId = Parentid;
    //    m_ParentName = _ParentName;
    //    m_StudentName = _StudentName;
    //    m_StudentId = _StudentId;
    //    m_SchoolName = _SchoolName;
    //    m_SchoolLogo = _SchoolLogo;

    //}
    ~KnowinUser()
    {
        if (m_DbLog != null)
        {
            m_DbLog = null;
        }
        if (m_MyEncrypt != null)
        {
            m_MyEncrypt = null;
        }
        if (m_MysqlDb != null)
        {
            m_MysqlDb = null;
        }
        if (m_SearchMng != null)
        {
            m_SearchMng = null;
        }
        if (m_IncidentMng != null)
        {
            m_IncidentMng = null;
        }
        if (m_AttendanceMng !=null) 
        {
            m_AttendanceMng = null;
        }
        if (m_LibMng != null)
        {
            m_LibMng = null;
        }
        if (m_ConfigMng != null)
        {
            m_ConfigMng = null;
        }
        if (m_StudMang != null)
        {
            m_StudMang = null;
        }
        if (m_Group != null)
        {
            m_Group = null;
        }
        
        if (m_StaffMng != null)
        {
            m_StaffMng = null;
        }
        if (m_Payroll != null)
        {
            m_Payroll = null;
        }
 
        if (m_ClassMng != null)
        {
            m_ClassMng = null;
        }
        if (m_Report != null)
        {
            m_Report = null;
        }
        if (m_TimeTable != null)
        {
            m_TimeTable = null;
        }
        if (m_TransMng != null)
        {
            m_TransMng = null;
        }
        
        if (m_KnowinRole != null)
        {
            m_KnowinRole = null;
        }
        if (m_KnowinGrp != null)
        {
            m_KnowinGrp = null;
        }
        if (m_FeeMng != null)
        {
            m_FeeMng = null;
        }
        if (m_ExamMng != null)
        {
            m_ExamMng = null;
        }
        if (m_smsMng != null)
        {
            m_smsMng = null;
        }
        if (m_MyODBCConn != null)
        {
           /* if (m_MyODBCConn.State == System.Data.ConnectionState.Open)
            {
                m_MyODBCConn.Close();
            }*/
        }
        m_MyODBCConn = null;
        if (m_MyReader != null)
        {
            m_MyReader = null;

        }
        if (m_Inventory != null)
        {
            m_Inventory = null;
        }
        if (m_House != null)
        {
            m_House = null;
        }

    }

    public bool IsLogedIn
    {
        get
        {
            return m_Login;
        }
       
    }
    public int SELECTEDMODE
    {
        get
        {
            return m_selectedModule;
        }
        set
        {
            m_selectedModule = value;
        }

    }
    public string UserName
    {
        get
        {
            return m_Name; 
        }
        
    }
    public int UserRoleId
    {
        get
        {
            return m_Role;
        }

    }
    public bool HaveSchoolRight
    {
        get
        {
            return m_HaveSchoolRight;
        }

    }
    public bool HaveStudentRight
    {
        get
        {
            return m_HaveStudRight;
        }

    }
    public string SCHOOLNAME
    {
        get
        {
            return m_SchoolName;
        }
    }
    public string MyGroupString
    {
        get
        {
            if (m_myAssoGroupString == "")
            {
                m_myAssoGroupString = GetMyAssociatedGroupString();
            }
            return m_myAssoGroupString;
        }

    }

   


     private string GetMyAssociatedGroupString()
     {
         string _Groupstring = "0";
         DataSet _mygroup = MyAssociatedGroups();
         if (_mygroup != null && _mygroup.Tables != null && _mygroup.Tables[0].Rows.Count > 0)
         {

             foreach (DataRow dr_group in _mygroup.Tables[0].Rows)
             {
                 if (_Groupstring == "0")
                 {
                     _Groupstring = dr_group[0].ToString();
                 }
                 else
                 {
                     _Groupstring = _Groupstring + "," + dr_group[0].ToString();
                 }

                    
             }
             
         }

         return _Groupstring;
     }

    public string CurrentBatchName
    {
        get
        {
            if (m_CurrtentBatchName == null)
            {
                string sql = "SELECT BatchName FROM tblbatch where Status=1";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {

                    m_CurrtentBatchName = m_MyReader.GetValue(0).ToString();
                }
                m_MyReader.Close();
            }

            
            return m_CurrtentBatchName;
        }

    }


    public string ExcelHeader
    {
        get
        {
            if (m_ExcelHeader == "")
            {
                string ColumnCount = "10";
                string sql = "SELECT SchoolName,Address FROM tblschooldetails";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {

                    m_ExcelHeader += "<table width=\"100%\"><tr>";
                    m_ExcelHeader += "<td colspan=\"" + ColumnCount + "\" style=\"font-size:24px;text-align:center;height:40px;font-weight:bold\">" + m_MyReader.GetValue(0).ToString() + "";
                    m_ExcelHeader += "</td></tr>";
                    m_ExcelHeader += "<tr><td colspan=\"" + ColumnCount + "\" style=\"font-size:20px;text-align:center;height:35px;font-weight:bold\">" + m_MyReader.GetValue(1).ToString() + "";
                    m_ExcelHeader += "</td></tr>";
                    m_ExcelHeader += "</table>";
                }
                m_MyReader.Close();
            }
            return m_ExcelHeader;
        }
    }


    public int CurrentBatchId
    {
        get
        {
            if (m_CurrtentBatchId == 0)
            {
                LoadCurrentbatchId();
            }
            return m_CurrtentBatchId;
        }
        
    }
    public void LoadCurrentbatchId()
    {
        string sql = "SELECT Id,BatchName FROM tblbatch where Status=1";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            m_CurrtentBatchId = int.Parse(m_MyReader.GetValue(0).ToString());
            m_CurrtentBatchName = m_MyReader.GetValue(1).ToString();
        }
        m_MyReader.Close();
    }

    public int UserId
    {
        get
        {
            return m_myId;
        }

    }


    public int StudId
    {
        get
        {
            return m_StudentId;
        }

    }

    public KnowinGroup GetGroupObj()
    {
        if (m_Login && m_Role==1)
        {
            if (m_KnowinGrp == null)
            {
                m_KnowinGrp = new KnowinGroup(this);
            }
        }

        return m_KnowinGrp;
    }
    public FeeManage GetFeeObj()
    {
        if (m_Login)
        {
            if (m_FeeMng == null)
            {
                m_FeeMng = new FeeManage(this);
            }
        }

        return m_FeeMng;
    }
    public WinErSearch GetSearchObj()
    {
        if (m_Login)
        {
            if (m_SearchMng == null)
            {
                m_SearchMng = new WinErSearch(this);
            }
        }

        return m_SearchMng;
    }
    public SMSManager GetSMSMngObj()
    {
        if (m_Login)
        {
            if (m_smsMng == null)
            {
                m_smsMng = new SMSManager(this);
            }
        }

        return m_smsMng;
    }

    public Payroll GetPayrollObj()
    {
        if (m_Login)
        {
            if (m_Payroll == null)
            {
                m_Payroll = new Payroll(this);
            }
        }

        return m_Payroll;
    }

    public Inventory GetInventoryObj()
    {
        if (m_Login)
        {
            if (m_Inventory == null)
            {
                m_Inventory = new Inventory(this);
            }
        }

        return m_Inventory;
    }
    public HouseManager GetHouseObj()
    {
        if (m_Login)
        {
            if (m_House == null)
            {
                m_House = new HouseManager(this);
            }
        }

        return m_House;
    }
    


    public Incident GetIncedentObj()
    {
        if (m_Login)
        {
            if (m_IncidentMng == null)
            {
                m_IncidentMng = new Incident(this);
            }
        }

        return m_IncidentMng;
    }

    public Attendance GetAttendancetObj()
    {
        if (m_Login)
        {
            if (m_AttendanceMng == null)
            {
                m_AttendanceMng = new Attendance(this);
            }
        }

        return m_AttendanceMng;
    }

    public LibraryManagClass GetLibObj()
    {
        if (m_Login)
        {
            if (m_LibMng == null)
            {
                m_LibMng = new LibraryManagClass(this);
            }
        }

        return m_LibMng;
    }

    public StudentManagerClass GetStudentObj()
    {
        if (m_Login)
        {
            if (m_StudMang == null)
            {
                m_StudMang = new StudentManagerClass(this);
            }
        }

        return m_StudMang;
    }
    public GroupManager GetGroupManagerObj()
    {
        if (m_Login)
        {
            if (m_Group == null)
            {
                m_Group = new GroupManager(this);
            }
        }

        return m_Group;
    }

    public ConfigManager GetConfigObj()
    {
        if (m_Login)
        {
            if (m_ConfigMng == null)
            {
                m_ConfigMng = new ConfigManager(this);
            }
        }

        return m_ConfigMng;
    }

    public EmailManager GetEmailObj()
    {
        if (m_Login)
        {
            if (m_EmailMng == null)
            {
                m_EmailMng = new EmailManager(this);
            }
        }

        return m_EmailMng;
    }


    public StaffManager GetStaffObj()
    {
        if (m_Login)
        {
            if (m_StaffMng == null)
            {
                m_StaffMng = new StaffManager(this);
            }
        }

        return m_StaffMng;
    }
    public ExamManage GetExamObj()
    {
        if (m_Login)
        {
            if (m_ExamMng == null)
            {
                m_ExamMng = new ExamManage(this);
            }
        }

        return m_ExamMng;
    }
    public TransportationClass GetTransObj()
    {
        if (m_Login)
        {
            if (m_TransMng == null)
            {
                m_TransMng = new TransportationClass(this);
            }
        }

        return m_TransMng;
    }

    public ClassOrganiser GetClassObj()
    {
        if (m_Login)
        {
            if (m_ClassMng == null)
            {
                m_ClassMng = new ClassOrganiser(this);
            }
        }

        return m_ClassMng;
    }


    public Reports GetReportObj()
    {
        if (m_Login)
        {
            if (m_Report == null)
            {
                m_Report = new Reports(this);
            }
        }

        return m_Report;
    }

    public TimeTable GetTimeTableObj()
    {
        if (m_Login)
        {
            if (m_TimeTable == null)
            {
                m_TimeTable = new TimeTable(this);
            }
        }

        return m_TimeTable;
    }



    public KnowinUserAction GetUserActionObj()
    {
        if (m_Login && m_Role == 1)
        {
            if (m_knowinUserAction == null)
            {
                m_knowinUserAction = new KnowinUserAction(this);
            }
        }

        return m_knowinUserAction;
    }
    public KnowinRole GetRoleObj()
    {
        if (m_Login && m_Role == 1)
        {
            if (m_KnowinRole == null)
            {
                m_KnowinRole = new KnowinRole(this);
            }
        }

        return m_KnowinRole;
    }
    public void Dispose()
    {
        CLogging logger = CLogging.GetLogObject();
        logger.LogToFile("Dispose", "Loging Out", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, m_Name);
       // Finalize();
        System.GC.SuppressFinalize(this);
    }

    
    public Boolean LoginUser(string sUsername,string sPassword,int _Role , out string _message)
    {
    
        string sql;
        bool _valide;
        CLogging logger = CLogging.GetLogObject();
        try
        {
            
            logger.LogToFile("LoginUser", "user is send Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, sUsername);
            //Lisencesing _MyLicense = new Lisencesing(m_ServerPath+"\\UpImage\\");
            AddressLisencesing _MyLicense = new AddressLisencesing(FilePath + "\\UpImage\\", m_stConnection);
            if (_MyLicense.IsLisenced)
            {
                m_LicenseObject = _MyLicense.LicencseObj;
               
                //_MyLicense.InItDb();
                _valide = false;
                if (_Role == 1)
                    sql = "SELECT Password,RoleId,Id FROM tbluser where UserName='" + sUsername + "' AND RoleId=" + _Role + " And CanLogin=1";
                else
                    sql = "SELECT Password,RoleId,Id  FROM tbluser where UserName='" + sUsername + "' And CanLogin=1";

                logger.LogToFile("LoginUser", " Exicuting Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, sUsername);
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    logger.LogToFile("LoginUser", "Read Success ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, sUsername);
                    m_MyReader.Read();
                    if (m_MyEncrypt == null)
                    {
                        m_MyEncrypt = new KnowinEncryption();
                    }
            
                   
                    if (sPassword == m_MyEncrypt.Decrypt(m_MyReader.GetValue(0).ToString()))
                    {
                        logger.LogToFile("LoginUser", "Login Success ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, sUsername);
                        _valide = true;
                        _message = "sucess";
                        m_Name = sUsername;
                        m_UserName= sUsername;
                        m_Role = int.Parse(m_MyReader.GetValue(1).ToString());
                        m_myId = int.Parse(m_MyReader.GetValue(2).ToString());
                        m_userid = m_myId;
                        _message=GetLoginPage();
                        m_DbLog.LogToDb(m_UserName, "User Login", "User " + m_UserName + " Loged In", 1);
                    }
                    else
                    {
                        _message = "Invalid password";
                        m_DbLog.LogToDb(sUsername, "User Login", "User " + sUsername + " Login Failed Due to Wrong Password", 4);
                    }

                }
                else
                {
                    _message = "Invalid username";
                    m_DbLog.LogToDb(sUsername, "User Login", "User " + sUsername + " Login Failed Due to Invalid username", 5);
                }
                logger.LogToFile("LoginUser", "Exit module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, sUsername);

                m_MyReader.Close();

            }
            else
            {
                logger.LogToFile("LoginUser", "Login Failed : License Expired", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, sUsername);

                _valide = false;
                _message = _MyLicense.ErrorMessage;
                //_MyLicense.InItDb();
                m_DbLog.LogToDb(sUsername, "User Login", "User " + sUsername + " Login Failed Due to License is Expired", 5);
            }

            _MyLicense = null;
        }
        catch(Exception _ex)
        {
            logger.LogToFile("LoginUser", "Login Failed :"+_ex.Message, 'E', CLogging.PriorityEnum.LEVEL_IMPORTANT, sUsername);
            _message = "Unable To Login";
            m_DbLog.LogToDb(sUsername, "User Login", "User " + sUsername + " Login Failed", 5);
            _valide = false;
        }
        m_Login = _valide;
        return _valide;
    }


    public Boolean LoginUserStudent(string sUsername, string sPassword, int _Role, out string _message)
    {

        string sql;
        bool _valide;
        CLogging logger = CLogging.GetLogObject();
        try
        {

            logger.LogToFile("LoginUser", "user is send Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, sUsername);
            
                _valide = false;
                if (_Role == 1)
                    sql = "SELECT Pwd,Id,Studentid FROM tblstudentlogin where Loginname='" + sUsername + "' And Status=0";
                else
                    sql = "SELECT Pwd,Id,Studentid  FROM tblstudentlogin where Loginname='" + sUsername + "' And Status=0";

                logger.LogToFile("LoginUser", " Exicuting Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, sUsername);
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    logger.LogToFile("LoginUser", "Read Success ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, sUsername);
                    m_MyReader.Read();
                    if (m_MyEncrypt == null)
                    {
                        m_MyEncrypt = new KnowinEncryption();
                    }


                    if (sPassword == m_MyEncrypt.Decrypt(m_MyReader.GetValue(0).ToString()))
                    {
                        logger.LogToFile("LoginUser", "Login Success ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, sUsername);
                        _valide = true;
                        _message = "sucess";
                        m_Name = sUsername;
                        m_UserName = sUsername;
                        m_Role = 1;
                        m_myId = int.Parse(m_MyReader.GetValue(1).ToString());
                        m_userid = m_myId;
                    m_StudentId = int.Parse(m_MyReader.GetValue(2).ToString());

                    HttpContext.Current.Session["StudId"] = int.Parse(m_MyReader.GetValue(2).ToString());
                    HttpContext.Current.Session["StudType"] = 1;
                    //_message = GetStudentLoginPage();
                    m_DbLog.LogToDb(m_UserName, "User Login", "User " + m_UserName + " Loged In", 1);
                    }
                    else
                    {
                        _message = "Invalid password";
                        m_DbLog.LogToDb(sUsername, "User Login", "User " + sUsername + " Login Failed Due to Wrong Password", 4);
                    }

                }
                else
                {
                    _message = "Invalid username";
                    m_DbLog.LogToDb(sUsername, "User Login", "User " + sUsername + " Login Failed Due to Invalid username", 5);
                }
                logger.LogToFile("LoginUser", "Exit module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, sUsername);

                m_MyReader.Close();

           

            
        }
        catch (Exception _ex)
        {
            logger.LogToFile("LoginUser", "Login Failed :" + _ex.Message, 'E', CLogging.PriorityEnum.LEVEL_IMPORTANT, sUsername);
            _message = "Unable To Login";
            m_DbLog.LogToDb(sUsername, "User Login", "User " + sUsername + " Login Failed", 5);
            _valide = false;
        }
        m_Login = _valide;
        return _valide;
    }




    private string GetLoginPage()
    {
        string _LoginPage="#";
        if (HaveAreaRight(1))
        {
            m_HaveStudRight = true;
        }
        else
        {
            m_HaveStudRight = false;
        }
        if (HaveAreaRight(2))
        {
            m_HaveSchoolRight = true;
        }
        else
        {
            m_HaveSchoolRight = false;
        }
        if (m_HaveStudRight)
        {
            _LoginPage = "StudentHome.aspx";
        }
        else if (m_HaveSchoolRight)
        {
            _LoginPage = "StudentHome.aspx";
        }
        else
        {
            _LoginPage = "StudentHome.aspx";
        }



        m_MenuString = "";
        m_SchoolMenuString="";
        return _LoginPage;
    }

    private string GetStudentLoginPage()
    {
        string _LoginPage = "#";
        if (HaveAreaRight(1))
        {
            m_HaveStudRight = true;
        }
        else
        {
            m_HaveStudRight = false;
        }
        if (HaveAreaRight(2))
        {
            m_HaveSchoolRight = true;
        }
        else
        {
            m_HaveSchoolRight = false;
        }
        if (m_HaveStudRight)
        {
            _LoginPage = "Studentdetails.aspx";
        }
        else if (m_HaveSchoolRight)
        {
            _LoginPage = "SchoolHome.aspx";
        }
        else
        {
            _LoginPage = "Studentdetails.aspx";
        }



        m_MenuString = "";
        m_SchoolMenuString = "";
        return _LoginPage;
    }


    public bool IsDefaultDashBoardExists(int m_userid, out int _DashboardId)
    {
        bool _valid = false;
        _DashboardId = 0;
        string sql = "SELECT tbluserdashboardmap.DashBoardId FROM tbluserdashboardmap WHERE tbluserdashboardmap.UserId=" + m_userid;
        OdbcDataReader  m_MyReader1 = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader1.HasRows)
        {
            int.TryParse(m_MyReader1.GetValue(0).ToString(), out _DashboardId);
            if (_DashboardId > 0)
            {
                _valid = true;
            }
        }
        return _valid;
    }

    private bool HaveAreaRight(int _type)
    {
        bool _valide;
        string sql = "select tblroleactionmap.ActionId from tblroleactionmap  inner join tblmodule on tblmodule.Id= tblroleactionmap.ModuleId where tblmodule.IsActive=1 AND tblmodule.ModuleType="+_type+" AND tblroleactionmap.RoleId=" + m_Role;
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

    private bool IsLicensed()
    {
        bool _valide;
        DateTime _dtNow = System.DateTime.Now;
        DateTime _DtLicEndDate = DateTime.Parse("10/10/2011 9:22:00 AM");
        if ( _dtNow < _DtLicEndDate )
        {

            _valide = true;
        }
        else
        {
            _valide = false;
        }
        return _valide;
        
    }

    public void RecordLogin(string _UserName)
    {
        CLogging logger = CLogging.GetLogObject();
        logger.LogToFile("GetMenuString", "TryingBuildMainMenu", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, m_Name); 
        

        try
        {
            DateTime _dtNow = System.DateTime.Now;
            string sql = "UPDATE tbluser SET LastLogin= '" + _dtNow.ToString("s") + "' WHERE UserName ='" + _UserName + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            m_MyReader.Close();
            logger.LogToFile("RecordLogin", "Login Recorded", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, m_Name); 
        }
        catch (Exception _ex)
        {
            logger.LogToFile("GetMenuString", "Record Login Failed:" + _ex.Message, 'E', CLogging.PriorityEnum.LEVEL_IMPORTANT, m_Name); 
        }
    }
   
    public string GetMenuString(int _Type)
    {
        CLogging logger = CLogging.GetLogObject();
        logger.LogToFile("GetMenuString", "TryingBuildMainMenu", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, m_Name);
        string _home;
        string _MenuStr, _SubMenuString;
        if(_Type==1)
        {
            _MenuStr=m_MenuString;
            _home = "StudentHome.aspx";
        }
        else
        {
            _MenuStr=m_SchoolMenuString;
            _home = "SchoolHome.aspx";
        }
        
        if (_MenuStr == "")
        {
            _MenuStr = "<ul class=\"nav navbar-nav main-menu\"><li class=\"active\"><a class=\"dropdown-toggle dr-menu\" href=\"" + _home + "\">HOME</a></li>";
            //_MenuStr = "<ul id=\"qm0\" class=\"qmmc\"><li><a class=\"qmparent\" href=\"" + _home + "\">HOME</a></li>";

            string sql = "SELECT DISTINCT tblmodule.MenuName, tblmodule.Link,tblmodule.Id FROM tblmodule  WHERE tblmodule.ISActive=1 AND tblmodule.Type='MainMenu' AND tblmodule.ModuleType IN (" + _Type + ",3) order by tblmodule.Order";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    _SubMenuString = GetSubMenu(m_MyReader.GetValue(0).ToString(), m_MyReader.GetValue(1).ToString(), m_MyReader.GetValue(2).ToString(), _Type);

                    _MenuStr = _MenuStr + _SubMenuString;
                }

            }
            _MenuStr = _MenuStr + "</ul>";
            m_MyReader.Close();

            if (_Type == 1)
            {
                 m_MenuString=_MenuStr;
            }
            else
            {
                m_SchoolMenuString=_MenuStr;
            }

        }
        
        return _MenuStr;
    }


    public string GetStudentMenuString(int _Type)
    {
        CLogging logger = CLogging.GetLogObject();
        logger.LogToFile("GetMenuString", "TryingBuildMainMenu", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, m_Name);
        string _home;
        string _MenuStr, _SubMenuString;
        if (_Type == 1)
        {
            _MenuStr = m_MenuString;
            _home = "Studentdetails.aspx";
        }
        else
        {
            _MenuStr = m_SchoolMenuString;
            _home = "Studentdetails.aspx";
        }

        if (_MenuStr == "")
        {
            _MenuStr = "<ul class=\"nav navbar-nav main-menu\"><li class=\"active\"><a class=\"dropdown-toggle dr-menu\" href=\"" + _home + "\">HOME</a></li>";
            //_MenuStr = "<ul id=\"qm0\" class=\"qmmc\"><li><a class=\"qmparent\" href=\"" + _home + "\">HOME</a></li>";

            string sql = "SELECT DISTINCT tblmodule.MenuName, tblmodule.Link,tblmodule.Id FROM tblmodule  WHERE tblmodule.ISActive=1 AND tblmodule.Type='MainMenu' AND tblmodule.ModuleType IN (4) order by tblmodule.Order";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    _SubMenuString = GetSubMenu(m_MyReader.GetValue(0).ToString(), m_MyReader.GetValue(1).ToString(), m_MyReader.GetValue(2).ToString(), _Type);

                    _MenuStr = _MenuStr + _SubMenuString;
                }

            }
            _MenuStr = _MenuStr + "</ul>";
            m_MyReader.Close();

            if (_Type == 1)
            {
                m_MenuString = _MenuStr;
            }
            else
            {
                m_SchoolMenuString = _MenuStr;
            }

        }

        return _MenuStr;
    }




    private string GetSubMenu(string _Menuname, string _Menulink, string _ModuleId,int _Type)
    {
        StringBuilder _menustr = new StringBuilder("");
        string _Sublinkstr = "";
        string _Masterlinkstr = "";
        string _CCE_Masterlinkstr = "";
        bool _HasSubLink = false;
        
        if (_ModuleId == "24")
        {
            return GetReportMenu(_Menuname, _Menulink, _Type);
        }
        else if (_ModuleId == "27")
        {
            return GetUserMenu(_Menuname, _Menulink, _Type, _ModuleId);
        }
        else if (_ModuleId=="3")//mani updated code for 20.12.2013
        {
            return GetExamMenu(_Menuname, _Menulink, _Type, _ModuleId);
        }
        else
        {
            OdbcDataReader MyReader1;
            string _extraLink = GetExtraLink(_ModuleId, _Type);
            
            string sql = "SELECT DISTINCT tblaction.MenuName,tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId inner join tblmodule on tblmodule.Id= tblroleactionmap.ModuleId  WHERE  tblroleactionmap.RoleId=" + m_Role + " AND ((tblroleactionmap.ModuleId=" + _ModuleId + " AND tblaction.ActionType='Link') " + _extraLink + ")  order by tblaction.`Order` Asc";
            //string sql = "SELECT DISTINCT tblaction.ActionName,tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + m_Role + " AND tblroleactionmap.ModuleId=" + _ModuleId + " AND tblaction.ActionType IN ('Link','TypeSpecLink','TypeCustomerSpecLink') order by tblaction.`Order` Asc";
            MyReader1 = m_MysqlDb.ExecuteQuery(sql);
            if (MyReader1.HasRows)
            {
               
                _HasSubLink = true;
                while (MyReader1.Read())
                {
                    _Sublinkstr = _Sublinkstr + "<li><a href=\"" + MyReader1.GetValue(1).ToString() + "\">" + MyReader1.GetValue(0).ToString() + "</a></li>";
                  
                }
               
            }

            //GetCCEMasetrLinks(_ModuleId, out _CCE_Masterlinkstr);

            if (GetMasetrLinks(_ModuleId, _Type, out _Masterlinkstr) || _HasSubLink )
            {

                _menustr.Append("<li class=\"dropdown\"><a class=\"dropdown-toggle dr-menu\" data-toggle=\"dropdown\" href= \"" + _Menulink + "\">" + _Menuname + "<span class=\"caret\"></span></a> <ul class=\"dropdown-menu\">");
                     
                //_menustr.Append("<li><a class=\"qmparent\" href=\"" + _Menulink + "\">" + _Menuname + "</a><ul>");
                _menustr.Append(_Sublinkstr);
                _menustr.Append(_CCE_Masterlinkstr);
                _menustr.Append(_Masterlinkstr);
                
                _menustr.Append("</ul></li>");
            }
            MyReader1.Close();

            return _menustr.ToString();
        }
       
    }

    private string GetExamMenu(string _Menuname, string _Menulink, int _Type, string _ModuleId)
    {
        string _menustr = "";
        string Actiontype = "Link";
        if (CheckExamConfiguration())
            Actiontype = "CCE_Link";

      
        string _extraLink = GetExtraLink(_ModuleId, _Type);
        OdbcDataReader MyReader1;
        string sql = "SELECT DISTINCT tblaction.MenuName,tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId inner join tblmodule on tblmodule.Id= tblroleactionmap.ModuleId WHERE  tblroleactionmap.RoleId=" + m_Role + " AND ((tblroleactionmap.ModuleId=" + _ModuleId + " AND tblaction.ActionType='" + Actiontype + "') " + _extraLink + ")  order by tblaction.`Order` Asc";
        MyReader1 = m_MysqlDb.ExecuteQuery(sql);
        if (MyReader1.HasRows)
        {
 
            while (MyReader1.Read())
            {
                _menustr = _menustr + "<li><a href=\"" + MyReader1.GetValue(1).ToString() + "\">" + MyReader1.GetValue(0).ToString() + "</a></li>";

            }

        }

        string _Masterlinkstr = "";
        StringBuilder menustr = new StringBuilder("");
        menustr.Append("<li class=\"dropdown\"><a class=\"dropdown-toggle dr-menu\" data-toggle=\"dropdown\" href= \"" + _Menulink + "\">" + _Menuname + "<span class=\"caret\"></span></a> <ul class=\"dropdown-menu\">");
        //menustr.Append("<li><a class=\"qmparent\" href=\"" + _Menulink + "\">" + _Menuname + "</a><ul>");
        menustr.Append(_menustr);
        if (GetMasetrLinks(_ModuleId, _Type, out _Masterlinkstr) || true)
        {
            menustr.Append(_Masterlinkstr);
        }
        menustr.Append("</ul></li>");

        MyReader1.Close();
        return menustr.ToString();
    }

    private bool CheckExamConfiguration()
    {
        bool valid=false;
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

    private bool GetCCEMasetrLinks(string _ModuleId, out string _Masterlinkstr)
    {
        bool _Valid = false;
        _Masterlinkstr = "";
        OdbcDataReader MyReaderNew;
        string sql = "SELECT DISTINCT tblaction.MenuName,tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId inner join tblmodule on tblmodule.Id= tblroleactionmap.ModuleId  WHERE  tblroleactionmap.RoleId=" + m_Role + " AND ((tblroleactionmap.ModuleId=" + _ModuleId + " AND tblaction.ActionType='CCE_MasterLink'))  order by tblaction.`Order` Asc";
        //string sql = "SELECT DISTINCT tblaction.ActionName,tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + m_Role + " AND tblroleactionmap.ModuleId=" + _ModuleId + " AND tblaction.ActionType IN ('Link','TypeSpecLink','TypeCustomerSpecLink') order by tblaction.`Order` Asc";
        MyReaderNew = m_MysqlDb.ExecuteQuery(sql);
        if (MyReaderNew.HasRows)
        {

            _Valid = true;
            _Masterlinkstr = "<li><a class=\"qmparent\" href=\"javascript:void(0)\">CCE Masters</a><ul>";

            while (MyReaderNew.Read())
            {
                _Masterlinkstr = _Masterlinkstr + "<li><a href=\"" + MyReaderNew.GetValue(1).ToString() + "\">" + MyReaderNew.GetValue(0).ToString() + "</a></li>";

            }
            _Masterlinkstr = _Masterlinkstr + "</ul></li>";

        }

        return _Valid;

    }

    private bool GetMasetrLinks(string _ModuleId, int _Type, out string _Masterlinkstr)
    {
        string MasterLink="MasterLink";
        if (_ModuleId == "3")
            if (CheckExamConfiguration())
                MasterLink = "CCE_MasterLink";

        string _ExtraMasterLink=GetMasterExtraLink(_ModuleId,_Type);
        bool _Valid = false;
        _Masterlinkstr = "";
        OdbcDataReader MyReaderNew;
        string sql = "SELECT DISTINCT tblaction.MenuName,tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId inner join tblmodule on tblmodule.Id= tblroleactionmap.ModuleId  WHERE  tblroleactionmap.RoleId=" + m_Role + " AND ((tblroleactionmap.ModuleId=" + _ModuleId + " AND tblaction.ActionType='" + MasterLink + "')" + _ExtraMasterLink + ")  order by tblaction.`Order` Asc";
        MyReaderNew = m_MysqlDb.ExecuteQuery(sql);
        if (MyReaderNew.HasRows)
        {

            _Valid = true;
            _Masterlinkstr = "<li class=\"dropdown dropdown-submenu\"><a href=\"javascript:void(0)\" class=\"dropdown-toggle\" data-toggle=\"dropdown\">Masters</a><ul class=\"dropdown-menu\">";
              
            while (MyReaderNew.Read())
            {
                _Masterlinkstr = _Masterlinkstr + "<li><a href=\"" + MyReaderNew.GetValue(1).ToString() + "\">" + MyReaderNew.GetValue(0).ToString() + "</a></li>";

            }
            _Masterlinkstr = _Masterlinkstr + "</ul></li>";

        }
        return _Valid;
    }

    private string GetMasterExtraLink(string _ModuleId, int _Type)
    {
        string _Link="";
       if (_ModuleId=="12")
       {
           _Link = "OR (tblaction.ActionType='ConfigMasterLink' AND tblmodule.ModuleType IN (3," + _Type + "))";
       }
        return _Link;
    }

    private string GetUserMenu(string _Menuname, string _Menulink, int _Type, string _ModuleId)
    {
        string _menustr = "";
        OdbcDataReader MyReader1;
        OdbcDataReader MyReader2;

        string _extraLink = GetExtraLink(_ModuleId, _Type);
        
        string sql = "SELECT DISTINCT tblaction.MenuName,tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId inner join tblmodule on tblmodule.Id= tblroleactionmap.ModuleId WHERE  tblroleactionmap.RoleId=" + m_Role + " AND ((tblroleactionmap.ModuleId=" + _ModuleId + " AND tblaction.ActionType='Link') " + _extraLink + ")  order by tblaction.`Order` Asc";
        MyReader1 = m_MysqlDb.ExecuteQuery(sql);
        //sql = "SELECT DISTINCT tblmodule.MenuName,tblmodule.Id FROM tblmodule inner JOIN tblroleactionmap on tblroleactionmap.ModuleId= tblmodule.Id inner join  tblaction on tblaction.Id= tblroleactionmap.ActionId WHERE tblmodule.ISActive=1 AND  tblaction.ActionType='ApproveLink'  AND tblmodule.ModuleType IN (" + _Type + ",3) AND tblroleactionmap.RoleId=" + m_Role + " order by tblmodule.Order";
        sql = "SELECT DISTINCT tblaction.MenuName,tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId inner join tblmodule on tblroleactionmap.ModuleId= tblmodule.Id WHERE tblmodule.ISActive=1 AND  tblaction.ActionType='ApproveLink'  AND tblmodule.ModuleType IN (" + _Type + ",3) AND tblroleactionmap.RoleId=" + m_Role + " order by tblmodule.Order";
        MyReader2 = m_MysqlDb.ExecuteQuery(sql);
        if (MyReader1.HasRows || MyReader2.HasRows)
        {
            _menustr = "<li class=\"dropdown\"><a class=\"dropdown-toggle dr-menu\" data-toggle=\"dropdown\" href= \"javascript:void(0)\">" + _Menuname + "<span class=\"caret\"></span></a> <ul class=\"dropdown-menu\">";
            //_menustr = "<li><a class=\"qmparent\" href=\"javascript:void(0)\">" + _Menuname + "</a><ul>";


            while (MyReader1.Read())
            {
                _menustr = _menustr + "<li><a href=\"" + MyReader1.GetValue(1).ToString() + "\">" + MyReader1.GetValue(0).ToString() + "</a></li>";
                

            }
            if (MyReader2.HasRows)
            {
                _menustr = _menustr + "<li class=\"dropdown dropdown-submenu\"><a href=\"javascript:void(0)\" class=\"dropdown-toggle\" data-toggle=\"dropdown\">Approval List</a><ul class=\"dropdown-menu\">";
                //_menustr = _menustr + "<li><a class=\"qmparent\" href=\"javascript:void(0)\">Approval List</a><ul>";
                while (MyReader2.Read())
                {
                    _menustr = _menustr + "<li><a href=\"" + MyReader2.GetValue(1).ToString() + "\">" + MyReader2.GetValue(0).ToString() + "</a></li>";

                }
                _menustr = _menustr + "</ul></li>";
            }

            _menustr = _menustr + "</ul></li>";

        }


        MyReader1.Close();
        MyReader2.Close();



        return _menustr;
    }

    private string GetReportMenu(string _Menuname, string _Menulink, int _Type)
    {
        string _menustr = "";
        OdbcDataReader MyReader1;

        string sql = "SELECT DISTINCT tblmodule.MenuName,tblmodule.Id FROM tblmodule inner JOIN tblroleactionmap on tblroleactionmap.ModuleId= tblmodule.Id inner join  tblaction on tblaction.Id= tblroleactionmap.ActionId WHERE tblmodule.ISActive=1 AND tblmodule.Id !=24 AND tblaction.ActionType='ReportLink'  AND tblmodule.ModuleType IN (" + _Type + ",3) AND tblroleactionmap.RoleId=" + m_Role + " order by tblmodule.Order";
            //string sql = "SELECT DISTINCT tblaction.ActionName,tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + m_Role + " AND tblroleactionmap.ModuleId=" + _ModuleId + " AND tblaction.ActionType IN ('Link','TypeSpecLink','TypeCustomerSpecLink') order by tblaction.`Order` Asc";
            MyReader1 = m_MysqlDb.ExecuteQuery(sql);
            if (MyReader1.HasRows)
            {
                _menustr = "<li class=\"dropdown\"><a class=\"dropdown-toggle dr-menu\" data-toggle=\"dropdown\" href= \"javascript:void(0)\">" + _Menuname + "<span class=\"caret\"></span></a> <ul class=\"dropdown-menu\">";
                //_menustr = "<li><a class=\"qmparent\" href=\"javascript:void(0)\">" + _Menuname + "</a><ul>";

                while (MyReader1.Read())
                {
                    _menustr = _menustr+GetReportSubLink(MyReader1.GetValue(1).ToString(),MyReader1.GetValue(0).ToString());

                }
                _menustr = _menustr + "</ul></li>";

            }


            MyReader1.Close();

          

        return _menustr;
    }

    private string GetReportSubLink(string _ModuleId, string _ModuleName)
    {

        string _menustr = "";
        OdbcDataReader MyReader1;
        //mani update code for 20.12.2013
        string ActionType = "ReportLink";
        if (_ModuleId == "3" && _ModuleName == "EXAM")
        {
            if (CheckExamConfiguration())
                ActionType = "CCE_ReportLink";
        }

        string sql = "SELECT DISTINCT tblaction.MenuName,tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + m_Role + " AND tblroleactionmap.ModuleId=" + _ModuleId + " AND tblaction.ActionType='"+ActionType+"'   order by tblaction.`Order` Asc";
            
            MyReader1 = m_MysqlDb.ExecuteQuery(sql);
            if (MyReader1.HasRows)
            {
                //_menustr = "<li><a class=\"qmparent\" href=\"javascript:void(0)\">" + _ModuleName + "</a><ul>";
                _menustr = "<li class=\"dropdown dropdown-submenu\"><a href=\"javascript:void(0)\" class=\"dropdown-toggle\" data-toggle=\"dropdown\">" + _ModuleName + "</a><ul class=\"dropdown-menu\">";

                while (MyReader1.Read())
                {
                    _menustr = _menustr + "<li><a href=\"" + MyReader1.GetValue(1).ToString() + "\">" + MyReader1.GetValue(0).ToString() + "</a></li>";
                    
                }
                _menustr = _menustr + "</ul></li>";

            }


            MyReader1.Close();

          

        return _menustr;

    
    }

    private string GetExtraLink(string _ModuleId, int _Type)
    {
        string _ExtraLink = "";
        if (_ModuleId=="2")
        _ExtraLink = "OR ( tblaction.ActionType='StudLink'";
        else if (_ModuleId == "11")
            _ExtraLink = "OR ( tblaction.ActionType='StaffLink'";
        else if (_ModuleId == "12")
            _ExtraLink = "OR ( tblaction.ActionType='ConfiLink'";
        else if (_ModuleId == "27")
            _ExtraLink = "OR (tblaction.ActionType='UserLink'";
        if (_ExtraLink != "")
        {
            _ExtraLink = _ExtraLink + " AND tblmodule.ModuleType IN (" + _Type + ",3))";
        }
            return _ExtraLink;
       
    }

    public DataSet MyAssociatedGroups()
    {
        if (m_myAssoGroups == null)
        {
            string sql = "SELECT DISTINCT tblgroup.Id, tblgroup.GroupName FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + m_myId + " UNION SELECT DISTINCT tblgroup.Id, tblgroup.GroupName FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + m_myId;
            m_myAssoGroups = m_MysqlDb.ExecuteQuery(sql, "RoleAction");
        }
        return m_myAssoGroups;
    }

    public DataSet MyAssociatedClass()
    {
        if (m_myAssoClass == null)
        {
            string sql = "SELECT tblclass.Id,tblclass.ClassName from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + m_myId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + m_myId + ") ORDER BY tblstandard.Id,tblclass.ClassName";
            m_myAssoClass = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }
            return m_myAssoClass;
    }

    public void ClearAssociatedClass()
    {
        m_myAssoClass = null;
    }

    public bool MeIsManager(int _GrpId)
    {
        bool _valide;
        string sql = "SELECT * FROM tblgroup where Id=" + _GrpId + " And ManagerId="+m_myId;
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

    public bool IsMyGroup(int _groupID)
    {
        bool _valide;
        string sql = "SELECT * FROM tblgroupusermap where UserId=" + m_myId + " And GroupId =" + _groupID;
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

    public int GetUserIDFromName(string _username)
    {
        int _UsId;
        string sql = "SELECT Id FROM tbluser where UserName='" + _username + "'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            _UsId = int.Parse(m_MyReader.GetValue(0).ToString());
        }
        else
        {
            _UsId = 0;
        }
        m_MyReader.Close();
        return _UsId;
        
    }   

    public string GetDetailsString(int _ActionId)
    {
        string _strdetails="";

        string sql = "SELECT `tblaction`.`ActionName`, `tblaction`.`Description` FROM `tblaction` WHERE `tblaction`.`Id` =" + _ActionId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                m_MyReader.Read();
              
                _strdetails = "<br/><h2>" + m_MyReader.GetValue(0).ToString() + "</h2><br /><p>" + m_MyReader.GetValue(1).ToString() + "</p><br/>";

            }
            
            m_MyReader.Close();
          
            return _strdetails;
 
    }

    public bool ChangePassWord(string _oldPassword, string _newPassword,out string _message)
    {
        string sql;
        bool _valide;
       // CLogging logger = CLogging.GetLogObject();


       // logger.LogToFile("Try Password change", "user is send Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, sUsername);

        _valide = false;

        sql = "SELECT Password,RoleId,Id  FROM tbluser where UserName='" + m_Name + "' And CanLogin=1";

       // logger.LogToFile("LoginUser", " Exicuting Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, sUsername);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            // logger.LogToFile("LoginUser", "Read Success ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, sUsername);
            m_MyReader.Read();
            if (m_MyEncrypt == null)
            {
                m_MyEncrypt = new KnowinEncryption();
            }
            if (_oldPassword == m_MyEncrypt.Decrypt(m_MyReader.GetValue(0).ToString()))
            {
                _valide = true;
                string _newpass = m_MyEncrypt.Encrypt(_newPassword);
                sql = "UPDATE tbluser SET Password= '" + _newpass + "' WHERE UserName ='" + m_Name + "'";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                _message = "Password Changed";
            }
            else
            {
                _message = "Invalid Password";
                _valide = false;
            }

        }
        else
        {
            _message = "Invalid User";
            _valide = false;
        }
        
        //logger.LogToFile("LoginUser", "Exit module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, sUsername);

        m_MyReader.Close();
        m_Login = _valide;
        return _valide;
    }



    public bool ChangeStudentPassWord(int studentid,string _oldPassword, string _newPassword, out string _message)
    {
        string sql;
        bool _valide;
        // CLogging logger = CLogging.GetLogObject();


        // logger.LogToFile("Try Password change", "user is send Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, sUsername);

        _valide = false;

        sql = "SELECT Pwd,Id  FROM tblstudentlogin where Studentid='" + studentid + "' And Status=0";

        // logger.LogToFile("LoginUser", " Exicuting Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, sUsername);
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            // logger.LogToFile("LoginUser", "Read Success ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, sUsername);
            m_MyReader.Read();
            if (m_MyEncrypt == null)
            {
                m_MyEncrypt = new KnowinEncryption();
            }
            if (_oldPassword == m_MyEncrypt.Decrypt(m_MyReader.GetValue(0).ToString()))
            {
                _valide = true;
                string _newpass = m_MyEncrypt.Encrypt(_newPassword);
                sql = "UPDATE tblstudentlogin SET Pwd= '" + _newpass + "' WHERE Studentid ='" + studentid + "'";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                _message = "Password Changed";
            }
            else
            {
                _message = "Invalid Password";
                _valide = false;
            }

        }
        else
        {
            _message = "Invalid User";
            _valide = false;
        }

        //logger.LogToFile("LoginUser", "Exit module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, sUsername);

        m_MyReader.Close();
        m_Login = _valide;
        return _valide;
    }

    public bool HaveActionRignt(int _ActionId)
    {
        bool _valide;
        string sql = "select tblroleactionmap.ActionId from tblroleactionmap where tblroleactionmap.RoleId="+m_Role+" AND tblroleactionmap.ActionId=" + _ActionId;
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

    public bool HaveModule(int _moduleid)
    {
        bool _valide;
        string sql = "select Id from tblmodule where Id=" + _moduleid + " and IsActive=1";
        if (m_TransationDb != null)
        {
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
        }
        else
        {
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }
        //m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            _valide = true;
        }
        else
        {
            _valide = false;
        }
       
        return _valide;
    }

    //public string GetImageUrl(string Type, int UserId)
    //{
      
    //    string ImageUrl = "images/" + "img.png";
    //    string Sql = "SELECT FilePath FROM tblfileurl WHERE Type='" + Type + "' AND UserId=" + UserId;
    //    m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
    //    if (m_MyReader.HasRows)
    //    {
    //        ImageUrl = m_RelativePath;
    //        ImageUrl += "ThumbnailImages/" + m_MyReader.GetValue(0).ToString();
    //        //ImageUrl = FilePath+"\\ThumbnailImages\\" + m_MyReader.GetValue(0).ToString();
    //    }
    //    else
    //    { 
    //        if (Type == "StaffImage")
    //        {
    //            Sql = "SELECT FilePath FROM tblfileurl_history WHERE Type='" + Type + "' AND UserId=" + UserId;
    //                    m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
    //                    if (m_MyReader.HasRows)
    //                    {
    //                        ImageUrl = m_RelativePath;
    //                        ImageUrl += "ThumbnailImages/" + m_MyReader.GetValue(0).ToString();
    //                    }
    //                    else
    //                    {
    //                        ImageUrl = "images/" + "user.png";
    //                    }
    //        }
    //        else
    //        {
    //            Sql = "SELECT FilePath FROM tblfileurl_history WHERE Type='" + Type + "' AND UserId=" + UserId;
    //                    m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
    //                    if (m_MyReader.HasRows)
    //                    {
    //                        ImageUrl = m_RelativePath;
    //                        ImageUrl += "ThumbnailImages/" + m_MyReader.GetValue(0).ToString();
    //                    }
    //                    else
    //                    {

    //                        ImageUrl = "images/" + "stdnt.png";
    //                    }
    //        }
    //    }
      
    //    return ImageUrl;
    //}

    public string GetHomeMenuString(int _RoleId)
    {
        
        string _MenuStr;
        if (m_HomeMenu == "")
        {

            _MenuStr = "<ul><li><a href=\"WinSchoolHome.aspx\">WinEr Home</a></li>";
            string sql = "SELECT DISTINCT tblaction.MenuName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _RoleId + " AND tblaction.ActionType='HomeLink'";
            
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {              
                while (m_MyReader.Read())
                {
                    _MenuStr = _MenuStr + "<li><a href=\"" + m_MyReader.GetValue(1).ToString() + "\">" + m_MyReader.GetValue(0).ToString() + "</a></li>";
                }
            }
            _MenuStr = _MenuStr + "<li><a href=\"UserArea.aspx\">User Area</a></li>";
            _MenuStr = _MenuStr + "</ul>";
            
            m_MyReader.Close();
            m_HomeMenu = _MenuStr;

        }
        else
        {
            _MenuStr = m_HomeMenu;
        }
        return _MenuStr;
    }
    public string FillStaffTopData(int _UserId, string Type)
    {
        DataSet _Mydata_PupilData = GetPupilData(_UserId, Type);
        StringBuilder _pupilTopData = new StringBuilder("");
        //DateTime _DOB;
        // int Year;
        int Today = DateTime.Now.Year;

        if (_Mydata_PupilData != null && _Mydata_PupilData.Tables != null && _Mydata_PupilData.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr_PupilData in _Mydata_PupilData.Tables[0].Rows)
            {
                // _pupilTopData = new StringBuilder("<div class=\"container skin1 \"  style=\"min-height:200px\"> <table cellpadding=\"0\" cellspacing=\"0\" class=\"containerTable\"> <tr class=\"top\"><td class=\"no\"> </td> <td class=\"n\"> User Details </td> <td class=\"ne\"> </td></tr><tr class=\"middle\"> <td class=\"o\"> </td> 	<td class=\"c\"><table width=\"100%\">");
                // _pupilTopData.Append(" <tr> <td style=\"background-color: #C2D5FC\">  <b>Name</b> </td> <td style=\"background-color: #C2D5FC\"> " + dr_PupilData[0].ToString() + "  </td> </tr><tr><td> &nbsp;</td> <td> &nbsp;</td>  </tr><tr> <td style=\"background-color: #C2D5FC\">  <b>Role</b> </td> <td style=\"background-color: #C2D5FC\"> " + dr_PupilData[1].ToString() + "  </td> </tr><tr><td> &nbsp;</td> <td> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>  </tr>");

                //// _pupilTopData.Append("<tr> <td style=\"background-color: #C2D5FC\">  <b>Experience</b> </td> <td style=\"background-color: #C2D5FC\"> " + dr_PupilData[3].ToString() + "  </td> </tr><tr><td> &nbsp;</td> <td> &nbsp;</td>  </tr><tr> <td style=\"background-color: #C2D5FC\">  <b>DOB</b> </td> <td style=\"background-color: #C2D5FC\"> " + _DOB.ToString("dd-MM-yyyy") + "  </td> </tr><tr><td> &nbsp;</td> <td> &nbsp;</td>  </tr>");
                //// _pupilTopData.Append("<tr> <td style=\"background-color: #C2D5FC\">  <b>Age</b> </td> <td style=\"background-color: #C2D5FC\"> " + Year + "  </td> </tr><tr><td> &nbsp;</td> <td> &nbsp;</td>  </tr><tr> <td style=\"background-color: #C2D5FC\">  <b>Designation</b> </td> <td style=\"background-color: #C2D5FC\"> " + dr_PupilData[4].ToString() + "  </td> </tr>");
                // _pupilTopData.Append("</table></td> <td class=\"e\"> </td> </tr><tr class=\"bottom\"> <td class=\"so\"> </td> <td class=\"s\"></td> <td class=\"se\"> </td> </tr> 	</table> </div>");


                //  _pupilTopData = new StringBuilder(" <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"contain1erTable\"> <tr class=\"top\"><td class=\"no\"> </td> <td class=\"n\"> User Details </td> <td class=\"ne\"> </td></tr><tr class=\"middle\"> <td class=\"o\"> </td> 	<td class=\"c\"><table width=\"100%\">");
                _pupilTopData = new StringBuilder(" <table width=\"100%\">");
                _pupilTopData.Append(" <tr> <td align=\"right\">  <b>Name : </b> </td> <td align=\"left\"> " + dr_PupilData[0].ToString() + "  </td> </tr><tr><td> &nbsp;</td> <td> &nbsp;</td>   </tr>");
                _pupilTopData.Append(" <tr> <td align=\"right\">  <b>Role : </b> </td> <td align=\"left\"> " + dr_PupilData[1].ToString() + "  </td> </tr><tr><td> &nbsp;</td> <td> &nbsp;</td>   </tr>");
                _pupilTopData.Append(" <tr> <td align=\"right\">  <b>E-mail : </b> </td> <td align=\"left\"> " + dr_PupilData[2].ToString() + "  </td> </tr><tr><td> &nbsp;</td> <td> &nbsp;</td>  </tr>");
                _pupilTopData.Append(" <tr> <td align=\"right\">  <b>Last Login : </b> </td> <td align=\"left\"> " + dr_PupilData[3].ToString() + "  </td> </tr><tr><td> &nbsp;</td> <td> &nbsp;</td> </tr> ");
                _pupilTopData.Append("</table>");
                // _pupilTopData.Append("</table></td> <td class=\"e\"> </td> </tr><tr class=\"bottom\"> <td class=\"so\"> </td> <td class=\"s\"></td> <td class=\"se\"> </td> </tr> 	</table> ");
            }
        }
        return _pupilTopData.ToString();
    }

    public string FillStudentTopData(int _UserId, string Type)
    {
        string grp = "";
        DataSet _StudData = GetStudData(_UserId, Type);
        DataSet _Mydata_PupilData = GetStudLoginData(_UserId, Type);
        if(_Mydata_PupilData.Tables[0].Rows.Count>0)
        {
            grp = _Mydata_PupilData.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            grp = "No group is assigned";
        }
       
        StringBuilder _pupilTopData = new StringBuilder("");    
        int Today = DateTime.Now.Year;

        if (_StudData != null && _StudData.Tables != null && _StudData.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr_PupilData in _StudData.Tables[0].Rows)
            {
               
                _pupilTopData = new StringBuilder(" <table width=\"100%\">");
                _pupilTopData.Append(" <tr> <td align=\"right\">  <b>Name : </b> </td> <td align=\"left\"> " + dr_PupilData[0].ToString() + "  </td> </tr><tr><td> &nbsp;</td> <td> &nbsp;</td>   </tr>");
                _pupilTopData.Append(" <tr> <td align=\"right\">  <b>Group Name : </b> </td> <td align=\"left\"> " + grp + "  </td> </tr><tr><td> &nbsp;</td> <td> &nbsp;</td>   </tr>");
             
                _pupilTopData.Append("</table>");
             
            }
        }
        return _pupilTopData.ToString();
    }
    public string FillUserProfileData(int _UserId, string Type)
    {
        DataSet _Mydata_PupilData = GetPupilData(_UserId, Type);
        StringBuilder _pupilTopData = new StringBuilder("");
        int Today = DateTime.Now.Year;
        if (_Mydata_PupilData != null && _Mydata_PupilData.Tables != null && _Mydata_PupilData.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr_PupilData in _Mydata_PupilData.Tables[0].Rows)
            {
                _pupilTopData = new StringBuilder("<div class=\"well usrProfStyle\">");
                _pupilTopData.Append("<div class=\"Prfname\"> " + dr_PupilData[0].ToString() + "</div>");
                _pupilTopData.Append("<p class=\"Prfrole\"> " + dr_PupilData[1].ToString() + "</p>");
                _pupilTopData.Append("<p class=\"Prfemail\">" + dr_PupilData[2].ToString() + "</p>");
                _pupilTopData.Append("<p class=\"PrfLstLgn\">Last Login :" + dr_PupilData[3].ToString() + "</p>");
                _pupilTopData.Append("</div>");
            }
        }
        return _pupilTopData.ToString();
    }


    public string FillstudProfileData(int _UserId, string Type)
    {
        DataSet _Mydata_PupilData = GetStudData(_UserId, Type);
        StringBuilder _pupilTopData = new StringBuilder("");
        int Today = DateTime.Now.Year;
        if (_Mydata_PupilData != null && _Mydata_PupilData.Tables != null && _Mydata_PupilData.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr_PupilData in _Mydata_PupilData.Tables[0].Rows)
            {
                _pupilTopData = new StringBuilder("<div class=\"well usrProfStyle\">");
                _pupilTopData.Append("<div class=\"Prfname\"> " + dr_PupilData[0].ToString() + "</div>");
                _pupilTopData.Append("<p class=\"Prfrole\"> Student</p>");
              
                _pupilTopData.Append("</div>");
            }
        }
        return _pupilTopData.ToString();
    }





    private DataSet GetPupilData(int _userId, string _Type)
    {
        string sql;
        DataSet _Mydata_PupilData = null;
        if (_Type == "User")
        {
            sql = "select tbluser.SurName , tblrole.RoleName, tbluser.EmailId, tbluser.LastLogin from tbluser inner join tblrole on tblrole.Id = tbluser.RoleId where tbluser.Id =" + _userId + " and tbluser.`Status`=1";
            _Mydata_PupilData = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }
        return _Mydata_PupilData;
    }
    private DataSet GetStudData(int _userId, string _Type)
    {
        string sql;
        DataSet _Mydata_PupilData = null;
        if (_Type == "User")
        {
            sql = "select tblstudent.StudentName from tblstudent where tblstudent.Id =" + _userId + " and tblstudent.`Status`=1";
            _Mydata_PupilData = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }
        return _Mydata_PupilData;
    }
    private DataSet GetStudLoginData(int _userId, string _Type)
    {
        string sql;
        DataSet _Mydata_PupilData = null;
        if (_Type == "User")
        {
            sql = "select tbl_gr_master.GroupName from tbl_gr_groupusermap INNER JOIN tbl_gr_master on tbl_gr_master.Id = tbl_gr_groupusermap.GroupId  where tbl_gr_groupusermap.UserId =" + _userId + "";
            _Mydata_PupilData = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }
        return _Mydata_PupilData;
    }

    public DateTime GetDareFromText(string _StrDate)
    {
        DateTime _outDate;
        string[] _DateArray = _StrDate.Split('/');// store DD MM YYYY
        int _Day, _Month, _Year;
        _Day = int.Parse(_DateArray[0]);// day
        _Month = int.Parse(_DateArray[1]);// Month
        _Year = int.Parse(_DateArray[2]);// Year
        _outDate = new DateTime(_Year, _Month, _Day, 0, 0, 0);
        return _outDate;
    }

    public DateTime GetDaTeFromText(string _StrDate)
    {
        DateTime _outDate;
        string[] _DateArray = _StrDate.Split('-');// store DD MM YYYY
        int _Day, _Month, _Year;
        _Day = int.Parse(_DateArray[0]);// day
        _Month = int.Parse(_DateArray[1]);// Month
        _Year = int.Parse(_DateArray[2]);// Year
        _outDate = new DateTime(_Year, _Month, _Day, 0, 0, 0);
        return _outDate;
    }



    //public DateTime GetDareFromText_1(string _StrDate)
    //{
    //    DateTime _outDate;
    //    string[] _DateArray = _StrDate.Split('/');// store DD MM YYYY
    //    string[] _yraray = _DateArray[2].Split(' ');
    //    int _Day, _Month, _Year;
    //    _Day = int.Parse(_DateArray[0]);// day
    //    _Month = int.Parse(_DateArray[1]);// Month
    //    _Year = int.Parse(_yraray[0]);// Year
    //    _outDate = new DateTime(_Year, _Month, _Day, 0, 0, 0);
    //    return _outDate;
    //}

    public bool TryGetDareFromText(string _StrDate, out DateTime _outDate)
    {
        bool _Valid = false;
        try
        {
            string[] _DateArray = _StrDate.Split('/');// store DD MM YYYY
            int _Day, _Month, _Year;
            _Day = int.Parse(_DateArray[0]);// day
            _Month = int.Parse(_DateArray[1]);// Month
            _Year = int.Parse(_DateArray[2]);// Year
            _outDate = new DateTime(_Year, _Month, _Day, 0, 0, 0);
            _Valid = true;
        }
        catch
        {
            _Valid = false;
            _outDate = DateTime.Now;
        }
        return _Valid;
    }

    public bool TryGetDateFromText_Hiphen(string _StrDate, out DateTime _outDate)
    {
        bool _Valid = false;
        try
        {
            string[] _DateArray = _StrDate.Split(' ');
            _DateArray = _DateArray[0].Split('-');// store DD MM YYYY
            int _Day, _Month, _Year;
            _Day = int.Parse(_DateArray[0]);// day
            _Month = int.Parse(_DateArray[1]);// Month
            _Year = int.Parse(_DateArray[2]);// Year
            _outDate = new DateTime(_Year, _Month, _Day, 0, 0, 0);
            _Valid = true;
        }
        catch
        {
            _Valid = false;
            _outDate = DateTime.UtcNow;
        }
        return _Valid;
    }

    //public bool TryGetDareFromText_1(string _StrDate, out DateTime _outDate)
    //{
    //    bool _Valid = false;
    //    try
    //    {
    //        string[] _DateArray = _StrDate.Split('/');// store DD MM YYYY
    //        string[] _yraray = _DateArray[2].Split(' ');
    //        int _Day, _Month, _Year;
    //        _Day = int.Parse(_DateArray[0]);// day
    //        _Month = int.Parse(_DateArray[1]);// Month
    //        _Year = int.Parse(_yraray[0]);// Year
    //        _outDate = new DateTime(_Year, _Month, _Day, 0, 0, 0);
    //        _Valid = true;
    //    }
    //    catch
    //    {
    //        _Valid = false;
    //        _outDate = DateTime.Now;
    //    }
    //    return _Valid;
    //}
 
    public string GerFormatedDatVal(DateTime dt)
    {
        string d = "", m = "";
        if (dt.Date.Day < 10)
        {
            d = "0";
        }
        if (dt.Date.Month < 10)
        {
            m = "0";
        }


        return d+dt.Date.Day + "/" + m+dt.Date.Month + "/" + dt.Date.Year;
        

    }

    public bool UserCountExceeds()
    {
        bool Valid = true;
        int _UserCount = GetUserCount();
        if (_UserCount >= m_LicenseObject.m_usercount)
        {
            Valid = true;
        }
        else
        {
            Valid = false;
        }
        return Valid;
    }

    public int GetUserCount()
    {
        int _Count=0;
        string sql = "select count(tbluser.Id) from tbluser where  tbluser.RoleId<>1 and tbluser.`status`=1 and tbluser.CanLogin=1";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int.TryParse(m_MyReader.GetValue(0).ToString(),out  _Count);
        }
        return _Count;
    }

    public string getStudName(int userId)
    {
        string Uname = "";
        string sql = "select StudentName from tblview_student where Id=" + userId + " ";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Uname = m_MyReader.GetValue(0).ToString();
        }
        return Uname;
    }

    public string getStaffName(int userId)
    {
        string Uname = "";
        string sql = "select SurName from tblview_user where Id = " + userId + " ";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Uname = m_MyReader.GetValue(0).ToString();
        }
        return Uname;
    }

    public bool IsActive(int _ModuleId)
    {
        m_MyReader = null;
        bool _IsActive = false;
        string sql = "SELECT tblmodule.ModuleName from tblmodule where tblmodule.Id=" + _ModuleId + " and tblmodule.IsActive=1";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
                _IsActive = true;
        }
        return _IsActive;
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

    public bool HistoryUser(int userId)
    {
        bool inHistory = false;
        string sql = "select id from tbluser_history where id = " + userId + "";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            inHistory = true;
        }
        return inHistory;

    }

    public bool AnyClassExists()
    {
        bool ispresent= false;
        int Count = 0;
        string sql = "SELECT COUNT(tblclass.Id) from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + m_myId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + m_myId + ")";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            int.TryParse(m_MyReader.GetValue(0).ToString(), out Count);
            if (Count > 0)
            {
                ispresent = true;
            }
        }
        return ispresent;
    }

    # region ParentLogin

    public string ParentLoginUrl
    {
        get
        {
            if (m_ParentLoginUrl == "")
            {
                m_ParentLoginUrl = GetParentLoginUrl();
            }
            return m_ParentLoginUrl;
        }
    }

    public string SchoolName
    {
        get
        {
            if (m_SchoolName == "")
            {
                m_SchoolName = GetSchoolName();
            }
            return m_SchoolName;
        }
    }

    private string GetSchoolName()
    {
        string _Name = "";
        string sql = "select `SchoolName` from tblschooldetails";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _Name = m_MyReader.GetValue(0).ToString();
        }
        return _Name;
    }


    private string GetParentLoginUrl()
    {
        string _Path = "";
        string sql = "select `Value` from tblparent_config where Id=1";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _Path = m_MyReader.GetValue(0).ToString();
        }
        return _Path;
    }

    # endregion

    public string SelectDashBoard(string SelectedValue, int MasterId)
    {
        string page = "";
        if (SelectedValue != "-1")
        {
            if (SelectedValue == "1")
            {
                if (MasterId == 1)
                {
                    page = "WinSchoolHome.aspx";
                }
                else
                {
                    page = "WinErSchoolHome.aspx";
                }
            }
            else
            {
                page = "PrincipalDashboard.aspx";
            }
        }
        return page;
    }

    public void SetDefaultDashboard(int DashBoardId)
    {
        string sql = "REPLACE INTO tbluserdashboardmap (UserId,DashBoardId) VALUES (" + UserId + "," + DashBoardId + ")";
        m_MysqlDb.ExecuteQuery(sql);
    }

    public OdbcDataReader GetDashboardPages_hasRights()
    {
        string sql = "SELECT tbldashboardpages.Id,tbldashboardpages.`Page` FROM tbldashboardpages WHERE  tbldashboardpages.ActionId=0 OR tbldashboardpages.ActionId in (SELECT tblroleactionmap.ActionId FROM tblroleactionmap where tblroleactionmap.RoleId=" + m_Role + ")";
        OdbcDataReader _myreader= m_MysqlDb.ExecuteQuery(sql);
        return _myreader;
    }

    public string GetAcademicMenuString(int _roleid)
    {

        CLogging logger = CLogging.GetLogObject();

        string _MenuStr;
        if (m_LeavMenuStr == "")
        {
            _MenuStr = "<div class=\"module\"><img src=\"images/stdnt.png\" alt=\"\" width=\"20px\" height=\"20\"  align=\"left\"/></div>";

            _MenuStr = _MenuStr + "<div class=\"topmenu\"><ul>";

            logger.LogToFile("GetAttendanceMenuString", "user is sending Request to select menu tblaction ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            //string sql = "SELECT DISTINCT tblactionsystemmap.DisplayName, tblactionsystemmap.Link FROM tblactionsystemmap INNER JOIN  tblsystemrolemap ON tblactionsystemmap.Id = tblsystemrolemap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND tblsystemrolemap.ModuleId=40 AND tblactionsystemmap.ActionType='Link' order by tblactionsystemmap.`Order`";
            string sql = "SELECT DISTINCT tblactionsystemmap.DisplayName, tblactionsystemmap.Link FROM tblactionsystemmap INNER JOIN  tblroleactionmap ON tblactionsystemmap.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND tblroleactionmap.ModuleId=12 AND tblactionsystemmap.ActionType='HomeUserLink' order by tblactionsystemmap.`Order`";
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
            _MenuStr = _MenuStr + "</ul></div>";
            logger.LogToFile("GetConfigMangMenuString", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Close();
            m_LeavMenuStr = _MenuStr;
            logger.LogToFile("GetConfigMangMenuString", " exits from module  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        }
        else
        {
            _MenuStr = m_LeavMenuStr;
        }
        logger.LogToFile("GetConfigMangMenuString", "Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _MenuStr;
    }

    public void UpdateGroupMapDetails(int _StudentId, int type)
    {

        string sql = "";
        string _sql = "";
        OdbcDataReader IdReader = null;
        sql = "select Id from tbl_gr_groupusermap where UserId=" + _StudentId + " and Type=" + type + "";
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
                sql = "DELETE FROM tbl_gr_groupusermap  WHERE Id=" + Id + "";
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
    }

    public string GetSchoolSmallAdress()
    {
        string _Address = "";
        string sql = "";
        OdbcDataReader IdReader = null;
       // sql = "SELECT tblconfiguration.SubValue FROM tblconfiguration WHERE tblconfiguration.Module='Report' AND tblconfiguration.Id=47";
        sql = "SELECT tblschooldetails.Address FROM tblschooldetails WHERE tblschooldetails.Id=1";
        IdReader = m_MysqlDb.ExecuteQuery(sql);
        if (IdReader.HasRows)
        {
            if (IdReader.Read())
            {
                _Address = IdReader.GetValue(0).ToString();
            }
        }
        return _Address;
    }
}
