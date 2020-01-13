using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Data;
using System.Configuration;

namespace WinBase
{

     public class ParentInfoClass
     {
          private int m_ParentId;
          private string m_ParentName;
          private int m_StudentId;
          private string m_StudentName;
          private string m_StudentImage;
          private string m_SchoolLogo;
          private int m_BatchId;
          private string m_BatchName;
          private string m_SchoolName;
          private string m_ClassName;
          private int m_ClassId;
          private string m_RollN0;
          private int m_Age;
          private string m_AdmissionN0;
          private int m_Point;
          private int m_rating;
          private bool m_Login = false;
          private FeeManage m_FeeMng = null;
          public SchoolClass SchoolObject = null;

          public ParentInfoClass(int Parentid, string _ParentName, string _StudentName, int _StudentId, string _SchoolName, string _SchoolLogo)
          {
              m_ParentId = Parentid;
              m_ParentName = _ParentName;
              m_StudentName = _StudentName;
              m_StudentId = _StudentId;
              m_SchoolName = _SchoolName;
              m_SchoolLogo = _SchoolLogo;
           
          }
          public void ChangeStudent(string _StudentName, int _StudentId)
          {
              m_StudentName = _StudentName;
              m_StudentId = _StudentId;
          }
          public void SetBatchDetails(int _BatchId , string _BatchName)
          {
              m_BatchId = _BatchId;
              m_BatchName = _BatchName;
          }
          public void SetStudentDetails(string _ClassName, string _RollN0, int _Age, string _AdmissionN0, int _Point, int _rating, string _studentImg,string ClassId)
          {
              m_ClassId =int.Parse( ClassId);
              m_ClassName = _ClassName;
              m_RollN0 = _RollN0;
              m_Age = _Age;
              m_AdmissionN0 = _AdmissionN0;
              m_Point = _Point;
              m_rating = _rating;
              m_StudentImage = _studentImg;
              
          }
          public string ParentName
          {
            get
            {
               return m_ParentName;
            }
          }
          public string CLASSNAME
          {
              get
              {
                  return m_ClassName;
              }
          }

          public int CLASSID
          {
              get
              {
                  return m_ClassId;
              }
          }

          public string RollNO
          {
              get
              {
                  return m_RollN0;
              }
          }
          public string ADMISSIONNO
          {
              get
              {
                  return m_AdmissionN0;
              }
          }
          public int AGE
          {
              get
              {
                  return m_Age;
              }
          }
          public int POINT
          {
              get
              {
                  return m_Point;
              }
          }
          public int RATING
          {
              get
              {
                  return m_rating;
              }
          }
        public int ParentId
        {
            get
            {
                return m_ParentId;
            }
        }
        public string SCHOOLNAME
        {
            get
            {
                return m_SchoolName;
            }
        }
        public string SCHOOLLOGO
        {
            get
            {
                return m_SchoolLogo;
            }
        }
        public string StudentName
        {
            get
            {
                return m_StudentName;
            }
        }
          public string StudentImage
        {
            get
            {
                return m_StudentImage;
            }
        }
         
        public int StudentId
        {
            get
            {
                return m_StudentId;
            }
        }

        public string CurrentBatchName
        {
            get
            {
                return m_BatchName;
            }
        }

        public int CurrentBatchId
        {
            get
            {
                return m_BatchId;
            }
        }
     }

    public class ParentLogin
    {
        private KnowinEncryption m_MyEncrypt = null;
        public MysqlClass m_MysqlDb;
        private OdbcDataReader m_MyReader = null;
        private string m_ExcelHeader = "";
        string sql = "";
        private DataSet m_Dataset = null;
        private SchoolClass m_SchoolObject = null;
        public MysqlClass m_TransationDb = null;
        public ParentLogin(MysqlClass _Msqlobj,SchoolClass _SchoolObject)
        {
            m_MysqlDb = _Msqlobj;
            m_SchoolObject = _SchoolObject;
        }
        ~ParentLogin()
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
            //CLogging logger = CLogging.GetLogObject();
            //logger.LogToFile("CreateTansationDb", "Starting New Fee Transaction", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
            if (m_TransationDb != null)
            {

                m_TransationDb.TransactionRollback();
                m_TransationDb = null;
            }

            m_TransationDb = new MysqlClass(WinerUtlity.GetConnectionString(m_SchoolObject));
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

        public bool SecureLogin(string Id, string Type, out string _Message, out ParentInfoClass MyParentObj,out int  ACtivationStatus)
        {  
            MyParentObj = null;
            string sql = "";
            string BatchName;
            bool _Valid = false;
            _Message = "";
            ACtivationStatus = 0;
            if (Type == "Gmail")
            {
                sql = "SELECT Id,Name ,IsActiveSecure FROM tblparent_parentdetails where GmailAuthId='" + Id + "' And CanLogin=1";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            }
            else
            {
                sql = "SELECT Id,Name ,IsActiveSecure FROM tblparent_parentdetails where FBAuthId='" + Id + "' And CanLogin=1";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            }

            if (m_MyReader.HasRows)
            {
                int ParentId = int.Parse(m_MyReader.GetValue(0).ToString());
                string ParentName = m_MyReader.GetValue(1).ToString();
                m_Dataset = GetMyStudent(ParentId);
                int.TryParse(m_MyReader["IsActiveSecure"].ToString(), out  ACtivationStatus);
                if (m_Dataset != null && m_Dataset.Tables != null && m_Dataset.Tables[0].Rows.Count > 0)
                {
                    DataRow Dr_Student = m_Dataset.Tables[0].Rows[0];
                    string _SchoolName = GetSchoolName();
                    string _SchoolLogo = GetSchoolLogo();
                    MyParentObj = new ParentInfoClass(ParentId, ParentName, Dr_Student[0].ToString(), int.Parse(Dr_Student[1].ToString()), _SchoolName, _SchoolLogo);
                    string _ClassName, _RollN0, _AdmissionN0, _studentImg, ClassId;
                    int _Age, _Point, _rating;
                    GetStudentDetails(int.Parse(Dr_Student[1].ToString()), out _ClassName, out _RollN0, out _Age, out _AdmissionN0, out _studentImg, out ClassId);
                    MyParentObj.SetBatchDetails(GetBatchId(out BatchName), BatchName);
                    Incident MyIncident = new Incident(m_MysqlDb);
                    MyIncident.GetPointRating(MyParentObj.StudentId, MyParentObj.CurrentBatchId, out _Point, out _rating);
                    MyParentObj.SetStudentDetails(_ClassName, _RollN0, _Age, _AdmissionN0, _Point, _rating, _studentImg, ClassId);

                    MyIncident = null;
                    _Valid = true;

                }
                else
                {
                    _Message = "No child found";
                }


            }
            else
                _Message = "Login Failed. Please register your login credential";

            return _Valid;

        }
        public bool ParentLoginSuccess(int StudentId, out string _Message, out ParentInfoClass MyParentObj)
        {
            bool _Valid = false;
            string BatchName;
            _Message = ""; MyParentObj = null;
            string sql = "SELECT Password,Id,Name FROM tblparent_parentdetails Inner Join tblparent_parentstudentmap ON tblparent_parentstudentmap.ParentId=tblparent_parentdetails.Id where tblparent_parentstudentmap.StudentId=" + StudentId + " And tblparent_parentdetails.CanLogin=1";
                MyParentObj = null;
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);

                if (m_MyReader.HasRows)
                {
                    m_MyReader.Read();
                    int ParentId = int.Parse(m_MyReader.GetValue(1).ToString());
                    string ParentName = m_MyReader.GetValue(2).ToString();
                    m_Dataset = GetMyStudent(ParentId);
                    if (m_Dataset != null && m_Dataset.Tables != null && m_Dataset.Tables[0].Rows.Count > 0)
                    {
                        DataRow Dr_Student = m_Dataset.Tables[0].Rows[0];
                        string _SchoolName = GetSchoolName();
                        string _SchoolLogo = GetSchoolLogo();
                        MyParentObj = new ParentInfoClass(ParentId, ParentName, Dr_Student[0].ToString(), int.Parse(Dr_Student[1].ToString()), _SchoolName, _SchoolLogo);
                        string _ClassName, _RollN0, _AdmissionN0, _studentImg, ClassId;
                        int _Age, _Point, _rating;
                        GetStudentDetails(int.Parse(Dr_Student[1].ToString()), out _ClassName, out _RollN0, out _Age, out _AdmissionN0, out _studentImg, out ClassId);
                        MyParentObj.SetBatchDetails(GetBatchId(out BatchName), BatchName);
                        Incident MyIncident = new Incident(m_MysqlDb);
                        MyIncident.GetPointRating(MyParentObj.StudentId, MyParentObj.CurrentBatchId, out _Point, out _rating);
                        MyParentObj.SetStudentDetails(_ClassName, _RollN0, _Age, _AdmissionN0, _Point, _rating, _studentImg, ClassId);

                        MyIncident = null;
                        _Valid = true;

                    }
                    else
                        _Message = "No child found";
                }
            else
                    _Message = "Parent login not enabled for this parent";
            return _Valid;
        }

        public bool LoginSuccess(string _UserName, string _PassWord, out string _Message, out ParentInfoClass MyParentObj)
        {
            bool _Valid = false;
            _Valid=notEnabledSecureLogin(_UserName, _PassWord, out _Message) ;
            MyParentObj = null;

            if (_Valid)
            {
                _Valid = false;
                string BatchName;
                _Message = "";
                string sql = "SELECT Password,Id,Name FROM tblparent_parentdetails where UserName='" + _UserName + "' And CanLogin=1";
                MyParentObj = null;
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);

                if (m_MyReader.HasRows)
                {
                    m_MyReader.Read();
                    if (m_MyEncrypt == null)
                    {
                        m_MyEncrypt = new KnowinEncryption();
                    }
                    if (_PassWord == m_MyEncrypt.Decrypt(m_MyReader.GetValue(0).ToString()))
                    {
                        int ParentId = int.Parse(m_MyReader.GetValue(1).ToString());
                        string ParentName = m_MyReader.GetValue(2).ToString();
                        m_Dataset = GetMyStudent(ParentId);
                        if (m_Dataset != null && m_Dataset.Tables != null && m_Dataset.Tables[0].Rows.Count > 0)
                        {
                            DataRow Dr_Student = m_Dataset.Tables[0].Rows[0];
                            string _SchoolName = GetSchoolName();
                            string _SchoolLogo = GetSchoolLogo();
                            MyParentObj = new ParentInfoClass(ParentId, ParentName, Dr_Student[0].ToString(), int.Parse(Dr_Student[1].ToString()), _SchoolName, _SchoolLogo);
                            string _ClassName, _RollN0, _AdmissionN0, _studentImg, ClassId;
                            int _Age, _Point, _rating;
                            GetStudentDetails(int.Parse(Dr_Student[1].ToString()), out _ClassName, out _RollN0, out _Age, out _AdmissionN0, out _studentImg, out ClassId);
                            MyParentObj.SetBatchDetails(GetBatchId(out BatchName), BatchName);
                            Incident MyIncident = new Incident(m_MysqlDb);
                            MyIncident.GetPointRating(MyParentObj.StudentId, MyParentObj.CurrentBatchId, out _Point, out _rating);
                            MyParentObj.SetStudentDetails(_ClassName, _RollN0, _Age, _AdmissionN0, _Point, _rating, _studentImg, ClassId);

                            MyIncident = null;
                            _Valid = true;

                        }
                        else
                            _Message = "No child found";

                    }
                    else
                        _Message = "Invalid password";
                }
                else
                    _Message = "Invalid Login Credentials";
            }
            return _Valid;
        }

        private bool notEnabledSecureLogin(string _UserName, string _PassWord, out string _Message)
        {
            _Message = "";
            string sql = "SELECT IsActiveSecure FROM tblparent_parentdetails where UserName='" + _UserName + "' And CanLogin=1";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);

            int activated = 0;
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader["IsActiveSecure"].ToString(), out activated);
                if (activated == 0)
                    return true;
                else
                {
                    _Message="Secure login is already activated. Please login via Gmail  ";
                    return false;
                }
            }
            return true;

        }

        public void GetStudentDetails(int _StudentId, out string _ClassName, out string _RollN0, out int _Age, out string _AdmissionN0, out string _StudendImg, out string _ClassId)
        {
            _ClassName = "";
            _RollN0 = "NIL";
            _Age = 0;
            _AdmissionN0 = "";
            _ClassId = "";
            DateTime _Dob;

            sql = "select tblclass.ClassName , tblstudent.RollNo, tblstudent.DOB, tblstudent.AdmitionNo, tblclass.Id from tblstudent inner join tblclass on tblstudent.LastClassId= tblclass.Id where tblstudent.Id=" + _StudentId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _ClassName = m_MyReader.GetValue(0).ToString();
                _RollN0 = m_MyReader.GetValue(1).ToString();
                _Dob = DateTime.Parse(m_MyReader.GetValue(2).ToString());
                _AdmissionN0 = m_MyReader.GetValue(3).ToString();
                _ClassId = m_MyReader.GetValue(4).ToString();
                if (_RollN0.Trim() == "-1")
                    _RollN0 = "NIL";
                _Age = DateTime.Now.Year - _Dob.Year;
               
            }
            _StudendImg = "Handler/ImageReturnHandler.ashx?id=" + _StudentId + "&type=StudentImage"; 
        }

        //private string GetStudentImage(int _StudentId)
        //{
        //    string ImageUrl = "images/" + "img.png";
        //    string Sql = "SELECT FilePath FROM tblfileurl WHERE Type='StudentImage' AND UserId=" + _StudentId;
        //    m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
        //    if (m_MyReader.HasRows)
        //    {
                
        //        ImageUrl =WinerUtlity.GetRelativeFilePath(m_SchoolObject)+"ThumbnailImages/" + m_MyReader.GetValue(0).ToString();
        //        //ImageUrl = FilePath+"\\ThumbnailImages\\" + m_MyReader.GetValue(0).ToString();
        //    }
        //    else
        //    {
        //        Sql = "SELECT FilePath FROM tblfileurl_history WHERE Type='StudentImage' AND UserId=" + _StudentId;
        //        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
        //        if (m_MyReader.HasRows)
        //        {
                    
        //            ImageUrl = WinerUtlity.GetRelativeFilePath(m_SchoolObject)+"ThumbnailImages/" + m_MyReader.GetValue(0).ToString();
        //        }
        //        else
        //        {

        //            ImageUrl = "images/" + "stdnt.png";
        //        }
        //    }

        //    return ImageUrl;
        //}

        
        private string GetSchoolName()
        {
            string _SchoolName = "";
            sql = "SELECT SchoolName FROM tblschooldetails WHERE Id=1";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _SchoolName = m_MyReader.GetValue(0).ToString();
             
            }
            return _SchoolName;
        }

        private string GetSchoolLogo()
        {
            //string ImageUrl = ConfigurationSettings.AppSettings["ParentLoginLogo"];
            //string HasVirtualFolder = ConfigurationSettings.AppSettings["HasVirtualFolder"];
            //if (HasVirtualFolder == "1")
            //{
            //    string Sql = "SELECT LogoUrl FROM tblschooldetails WHERE Id=1";
            //    m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            //    if (m_MyReader.HasRows)
            //    {


            //        if (m_MyReader.GetValue(0).ToString() != "")
            //            ImageUrl = WinerUtlity.GetRelativeFilePath(m_SchoolObject) + "ThumbnailImages/" + m_MyReader.GetValue(0).ToString();
            //        else
            //            ImageUrl = WinerUtlity.GetRelativeFilePath(m_SchoolObject) + "ThumbnailImages/" + "img.png";
            //    }
            //}
            return "Handler/ImageReturnHandler.ashx?id=1&type=Logo";
        }

        public DataSet GetMyStudent(int _ParentId)
        {
            sql = "select tblstudent.StudentName,tblstudent.Id from tblparent_parentdetails INNER JOIN tbl_siblingsmap ON tblparent_parentdetails.SiblingId=tbl_siblingsmap.Id inner join tblstudent on tblstudent.Id = tbl_siblingsmap.StudId where tblparent_parentdetails.Id=" + _ParentId + " UNION select tblstudent.StudentName,tblstudent.Id from tblparent_parentstudentmap inner join tblstudent on tblstudent.Id = tblparent_parentstudentmap.StudentId where tblparent_parentstudentmap.ParentId=" + _ParentId;
            m_Dataset =m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return m_Dataset;

        }


    

        private string GetClassName(int _StudentId, out string _RollNo)
        {
            string _ClassName = "None";
            _RollNo = "Not assigned";
            sql = "select tblclass.ClassName , tblstudent.RollNo from tblstudent inner join tblclass on tblclass.Id = tblstudent.LastClassId where tblstudent.Id=" + _StudentId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _ClassName = m_MyReader.GetValue(0).ToString();
                _RollNo = m_MyReader.GetValue(1).ToString();
                if (_RollNo == "-1")
                    _RollNo = "Not assigned";
            }
            return _ClassName;
        }

        public int GetBatchId(out string BatchName)
        {
            int BatchId = 0;
            BatchName = "";
            sql = "select Id , BatchName from tblbatch where Status=1";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out BatchId);
                BatchName = m_MyReader.GetValue(1).ToString();
            }
            return BatchId;
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


        public bool ChangePassWord(string _oldPassword, string _newPassword, out string _message , int _ParentId)
        {
            string sql;
            bool _valide;
            _valide = false;
            KnowinEncryption m_MyEncrypt = new KnowinEncryption();
            sql = "SELECT Password FROM tblparent_parentdetails where Id='" + _ParentId + "' And CanLogin=1";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                m_MyReader.Read();
                if (_oldPassword == m_MyEncrypt.Decrypt(m_MyReader.GetValue(0).ToString()))
                {
                    _valide = true;
                    string _newpass = m_MyEncrypt.Encrypt(_newPassword);
                    sql = "UPDATE tblparent_parentdetails SET Password= '" + _newpass + "' WHERE Id ='" + _ParentId + "'";
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
            m_MyEncrypt = null;
            return _valide;
        }


        public bool LoginEnabled(out string _Message)
        {
            bool _Valid = false;
            _Message = "";
            string sql = "select Value from tblparent_config where Id=2";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                if (m_MyReader.GetValue(0).ToString() == "1")
                    _Valid = true;
            }
            if (!_Valid)
                _Message = "Your login is disabled temporarily. Please try again after some time";
            return _Valid;
        }


        public void RecordLogin(string _UserName)
        {
            //CLogging logger = CLogging.GetLogObject();
            //logger.LogToFile("GetMenuString", "TryingBuildMainMenu", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, m_Name);

           

            try
            {
                DateTime _dtNow = System.DateTime.Now;
                string sql = "UPDATE tblparent_parentdetails SET LastLogin= '" + _dtNow.ToString("s") + "' WHERE UserName ='" + _UserName + "'";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                m_MyReader.Close();
             
               // logger.LogToFile("RecordLogin", "Login Recorded", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, m_Name);
            }
            catch (Exception _ex)
            {
                //logger.LogToFile("GetMenuString", "Record Login Failed:" + _ex.Message, 'E', CLogging.PriorityEnum.LEVEL_IMPORTANT, m_Name);
            }
        }

        public void GetSchoolDetails(out string SchoolName, out string address)
        {
            SchoolName = "";
            address = "";

            string Sql = "SELECT SchoolName,Address FROM tblschooldetails WHERE Id=1";
            m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                SchoolName = m_MyReader.GetValue(0).ToString();
                address = m_MyReader.GetValue(1).ToString();
            }
        }
        public int GetThreadId(int frmUserId, int toUsrId, int fromUsrType, int ToUsrType, string subj)
        {
            int threadId = 0;
            string sql = "select max(threadId) from tblmail_thread";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    int.TryParse(m_MyReader.GetValue(0).ToString(), out threadId);
                    threadId = threadId + 1;
                }
            }
            sql = "insert into tblmail_thread (ThreadId,MsgType,FromUserId,ToUserId,FromUserType,ToUserType,Subject,Status,OpenDate,CloseDate) values (" + threadId + " ,0," + frmUserId + "," + toUsrId + "," + fromUsrType + "," + ToUsrType + ",'" + subj + "',1,'" + DateTime.Now.ToString("s") + "','" + DateTime.Now.ToString("s") + "' )";
            m_MysqlDb.ExecuteQuery(sql);

            return threadId;
        }

        public void ComposeMessage(int frmUserId, int toUsrId, int FromUserType, int ToUserType, string subj, string desc, int status, int priority,  int threadId)
        {
            string sql = "";
            sql = "insert into tblparent_mail (FromUserId,ToUserId,FromUserType,ToUserType,Subject,Description,Status,Priority,Date,ThreadId) values (" + frmUserId + "," + toUsrId + "," + FromUserType + "," + ToUserType + ",'" + subj + "','" + desc + "'," + status + "," + priority + ",'" + DateTime.Now.ToString("s") + "'," + threadId + ") ";
            m_MysqlDb.ExecuteQuery(sql);
        }
        public DataSet GetMessagesForParent(int ToUserId, int ReadStatus, int ToUserType)
        {
            string sql = "", ReadCondition = "";
            if (ReadStatus != -1)
            {
                ReadCondition = " and  tblparent_mail.Status=" + ReadStatus + "";
            }
            //sql = "select tblparent_mail.Id, tblparent_mail.FromUserId, tblparent_mail.FromUserType, tbluser.Surname,tblparent_mail.Subject, tblparent_mail.Description,tblparent_mail.Date,tblparent_mail.Priority,tblparent_mail.Status ,tblparent_mail.ToUserType from tblparent_mail inner join tbluser on tbluser.Id =  tblparent_mail.FromUserId  where tblparent_mail.ToUserType = " + ToUserType + " and tblparent_mail.ToUserId =" + ToUserId + ReadCondition;
            //sql = "select max(tblparent_mail.Id) as Id, tblparent_mail.FromUserId, tblparent_mail.FromUserType,  tbluser.Surname,tblparent_mail.Subject, tblparent_mail.Description,tblparent_mail.Date,tblparent_mail.Priority,tblparent_mail.Status,tblparent_mail.ThreadId  from tblparent_mail inner join tbluser on tbluser.Id =  tblparent_mail.FromUserId where tblparent_mail.ToUserType = " + ToUserType + " and tblparent_mail.ToUserId =" + ToUserId + ReadCondition + " group by threadid";
            sql = "select tblmail_thread.ThreadId ,  tbluser.Surname,tblmail_thread.Subject,(SELECT Description FROM tblparent_mail WHERE  Id IN  (SELECT MAX(Id) FROM tblparent_mail WHERE tblparent_mail.ThreadId=tblmail_thread.ThreadId )) as Description,(SELECT Date FROM tblparent_mail WHERE  Id IN  (SELECT MAX(Id) FROM tblparent_mail WHERE tblparent_mail.ThreadId=tblmail_thread.ThreadId )) as 'Date', tblmail_thread.Status from tblmail_thread  inner join tbluser on (tblmail_thread.FromUserType=1 and  tbluser.Id = tblmail_thread.ToUserId ) or (tblmail_thread.FromUserType=2 and  tbluser.Id =  tblmail_thread.FromUserId) where (tblmail_thread.ToUserType = 1 and tblmail_thread.ToUserId =" + ToUserId + ") or (tblmail_thread.FromUserType = 1 and tblmail_thread.FromUserId=" + ToUserId + ") ";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }

        public DataSet GetMessageThrds(int thrdId, int UserType)
        {
             // tblparent_mail  inner join tblview_student on tblparent_mail.FromUserId =
            //tblview_student.id  where tblparent_mail.ThreadId = " + thrdId + " and tblparent_mail.FromUserType = 1 
            string sql = "select tblparent_mail.Id, tblparent_mail.FromUserId, tblparent_mail.FromUserType, tbluser.Surname as Name,tblparent_mail.Subject, tblparent_mail.Description,tblparent_mail.Date,tblparent_mail.Priority,tblparent_mail.Status ,tblparent_mail.ToUserType,  tblparent_mail.ToUserId  from tblparent_mail inner join tbluser on (tbluser.Id =  tblparent_mail.FromUserId  and tblparent_mail.ToUserType = " + UserType + ") where tblparent_mail.ThreadId = " + thrdId + " union select tblparent_mail.Id, tblparent_mail.FromUserId, tblparent_mail.FromUserType, tblview_student.StudentName as Name,tblparent_mail.Subject, tblparent_mail.Description,tblparent_mail.Date,tblparent_mail.Priority,tblparent_mail.Status ,tblparent_mail.ToUserType,  tblparent_mail.ToUserId from tblparent_mail  inner join tblview_student on tblparent_mail.FromUserId =tblview_student.id  where tblparent_mail.ThreadId = " + thrdId + " and tblparent_mail.FromUserType = 1 ";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }

        public void SendThrdMessage(int FromUserId, int TmpFromUserType, int thrdId, string subj, string descrption)
        {
            int id = 0, ToUserId = 0, FromUserType = 0, ToUserType = 0, DeptId = 0;
            string sql = "";
            //sql = "select id,FromUserId,ToUserId,FromUserType,ToUserType,DeptId from tblparent_mail where ThreadId = " + thrdId + " and id in (select min(id) from tblparent_mail)";
            sql = "select ToUserId,FromUserType,ToUserType from tblmail_thread where ThreadId = " + thrdId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    int.TryParse(m_MyReader.GetValue(1).ToString(), out FromUserType);
                    if (TmpFromUserType == FromUserType)
                    {
                        int.TryParse(m_MyReader.GetValue(0).ToString(), out ToUserId);
                        int.TryParse(m_MyReader.GetValue(2).ToString(), out ToUserType);
                    }
                    else
                    {
                        ToUserId = FromUserId;
                        ToUserType = FromUserType;
                        int.TryParse(m_MyReader.GetValue(0).ToString(), out FromUserId);
                        int.TryParse(m_MyReader.GetValue(2).ToString(), out FromUserType);
                    }
                   // int.TryParse(m_MyReader.GetValue(3).ToString(), out DeptId);
                }
            }
            sql = "insert into tblparent_mail (FromUserId,ToUserId,FromUserType,ToUserType,Subject,Description,Status,Priority,Date,ThreadId,MessageReadStatus) values (" + FromUserId + "," + ToUserId + "," + FromUserType + "," + ToUserType + ",'" + subj + "','" + descrption + "',0,3,'" + DateTime.Now.ToString("s") + "'," + thrdId + ",'UNREAD')";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public DataSet GetMessages(int ToUserId, int ToUserType)
        {
            string sql = "", ReadCondition = "";

            //  sql = "select max(tblparent_mail.Id) as Id, tblparent_mail.FromUserId, tblparent_mail.FromUserType, tblparent_parentdetails.Name,tblparent_mail.Subject, tblparent_mail.Description,tblparent_mail.Date,tblparent_mail.Priority,tblparent_mail.Status,tblparent_mail.ThreadId  from tblparent_mail inner join tblparent_parentdetails on tblparent_parentdetails.Id =  tblparent_mail.FromUserId where tblparent_mail.ToUserType = " + ToUserType + " and tblparent_mail.ToUserId =" + ToUserId + ReadCondition + " group by threadid";

            sql = "select tblmail_thread.ThreadId ,  tblstudent.StudentName as 'Name',tblmail_thread.Subject,(SELECT Description FROM tblparent_mail WHERE  Id IN  (SELECT MAX(Id) FROM tblparent_mail WHERE tblparent_mail.ThreadId=tblmail_thread.ThreadId )) as Description,(SELECT Date FROM tblparent_mail WHERE  Id IN  (SELECT MAX(Id) FROM tblparent_mail WHERE tblparent_mail.ThreadId=tblmail_thread.ThreadId )) as 'Date', tblmail_thread.Status from tblmail_thread  INNER JOIN tblstudent  ON tblstudent.Id= tblmail_thread.FromUserId  where (tblmail_thread.FromUserType = 2 and tblmail_thread.FromUserId =" + ToUserId + ") or (tblmail_thread.ToUserType = 2 and tblmail_thread.ToUserId=" + ToUserId + ") ";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }





        public DataSet getactivemodules()
        {
            string sql = " select tblmodule.id , tblmodule.ModuleName from tblmodule where IsActive=1";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }

        public DataSet GetUnreadmailcount(int studentid)
        {
            string sql = " select count(tblparent_mail.id) as mailcount from tblparent_mail  where tblparent_mail.MessageReadStatus='UNREAD' and ToUserId=" + studentid;
               return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }
        public DataSet UpdateReadStatus(int studentid,int threadid)
        {
            string sql = " update tblparent_mail set MessageReadStatus='READ' where   ThreadId=" + threadid + "  and ToUserId=" + studentid;
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }

        public DataSet getRequestType(int ServiceType)
        {
            string sql = "select Id,ServiceName,ServiceType from tblservicerequesttype where ServiceType=" + ServiceType;
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }


        public void createNewServiceLog(string serviceId, int serviceType, string serviceHeading, string serviceDescription, int fromUserType, int toUserType, int fromUserId, int toUserId)
        {
           DateTime dt= System.DateTime.Now;
           string sql = "insert into tblservicerequestthread(ServiceId,ServiceType,ServiceHeading,FromUserId,FromUserType,ToUserType,ToUserId,CreatedDate) values(" + serviceId + "," + serviceType + ",'" + serviceHeading + "'," + fromUserId + "," + fromUserType + "," + toUserType + "," + toUserId + ",'" + dt.ToString("s") + "')";
            m_MysqlDb.ExecuteQuery(sql);

        
            
            int threadId = 0;
             sql = "select max(Id) from tblservicerequestthread";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    int.TryParse(m_MyReader.GetValue(0).ToString(), out threadId);
                 
                }
            }
            else
            {
                threadId = 1;
            }

            sql = "insert into tblservicerequestthreaddetails (ThreadId,Heading,Description,FromUserType,FromUserId,ToUserType,ToUserId,CreatedDate) values (" + threadId + " ,'" + serviceHeading + "','" + serviceDescription + "'," + fromUserType + "," + fromUserId + "," + toUserType + "," + toUserId + ",'"+dt.ToString("s") + "' )";
            m_MysqlDb.ExecuteQuery(sql);

            
        }

        public DataSet GetAllServiceRequests(int studetId, int ReadStatus, int toUserType, int SERVICE_TYPE)
        {
            string sql = @"select DISTINCT tblservicerequestthread.Id as ThreadId ,tblservicerequestthread.ServiceHeading, (SELECT tblservicerequestthreaddetails.Description FROM tblservicerequestthreaddetails WHERE  Id IN  (SELECT MAX(Id) FROM tblservicerequestthreaddetails WHERE tblservicerequestthreaddetails.ThreadId=tblservicerequestthread.Id )) as Description,(SELECT tblservicerequestthreaddetails.CreatedDate FROM tblservicerequestthreaddetails WHERE  Id IN  (SELECT MAX(Id) FROM tblservicerequestthreaddetails  WHERE tblservicerequestthreaddetails.ThreadId=tblservicerequestthread.Id )) as 'Date'  from tblservicerequestthread inner join tblservicerequestthreaddetails on tblservicerequestthreaddetails.ThreadId=tblservicerequestthread.Id where ( tblservicerequestthreaddetails.ToUserType = " + toUserType + " and tblservicerequestthreaddetails.ToUserId =" + studetId + ") or  (tblservicerequestthreaddetails.FromUserType  = " + toUserType + " and tblservicerequestthreaddetails.FromUserId =" + studetId + ") and tblservicerequestthread.ServiceType="+ SERVICE_TYPE;
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }
       
        public DataSet GetServiceThrds(int thrdId, int UserType, int SERVICE_TYPE)
        {
            // tblparent_mail  inner join tblview_student on tblparent_mail.FromUserId =
            //tblview_student.id  where tblparent_mail.ThreadId = " + thrdId + " and tblparent_mail.FromUserType = 1 
            string sql = "select tblservicerequestthreaddetails.Id, tblservicerequestthreaddetails.FromUserId, tblservicerequestthreaddetails.FromUserType, tbluser.Surname as Name,tblservicerequestthreaddetails.Heading, tblservicerequestthreaddetails.Description,tblservicerequestthreaddetails.CreatedDate as Date,tblservicerequestthreaddetails.ToUserType,  tblservicerequestthreaddetails.ToUserId  from tblservicerequestthread  inner join tblservicerequestthreaddetails on tblservicerequestthreaddetails.ThreadId = tblservicerequestthread.id inner join tbluser on (tbluser.Id =  tblservicerequestthreaddetails.FromUserId  and tblservicerequestthreaddetails.ToUserType = " + UserType + ") where tblservicerequestthreaddetails.ThreadId = " + thrdId + " and tblservicerequestthread.ServiceType=" + SERVICE_TYPE + " union select tblservicerequestthreaddetails.Id, tblservicerequestthreaddetails.FromUserId, tblservicerequestthreaddetails.FromUserType, tblview_student.StudentName as Name,tblservicerequestthreaddetails.Heading, tblservicerequestthreaddetails.Description,tblservicerequestthreaddetails.CreatedDate,tblservicerequestthreaddetails.ToUserType,  tblservicerequestthreaddetails.ToUserId from tblservicerequestthread  inner join tblservicerequestthreaddetails on tblservicerequestthreaddetails.ThreadId = tblservicerequestthread.id  inner join tblview_student on tblservicerequestthreaddetails.FromUserId =tblview_student.id  where tblservicerequestthreaddetails.ThreadId = " + thrdId + " and tblservicerequestthreaddetails.FromUserType = " + UserType + " and tblservicerequestthread.ServiceType=" + SERVICE_TYPE;
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }

        public void SendServicesThrd(int FromUserId, int TmpFromUserType, int thrdId, string subj, string descrption)
        {
            int ToUserId = 0, FromUserType = 0, ToUserType = 0;
            string sql = "";
            //sql = "select id,FromUserId,ToUserId,FromUserType,ToUserType,DeptId from tblparent_mail where ThreadId = " + thrdId + " and id in (select min(id) from tblparent_mail)";
            sql = "select ToUserId,FromUserType,ToUserType from tblservicerequestthread where Id = " + thrdId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    int.TryParse(m_MyReader.GetValue(1).ToString(), out FromUserType);
                    if (TmpFromUserType == FromUserType)
                    {
                        int.TryParse(m_MyReader.GetValue(0).ToString(), out ToUserId);
                        int.TryParse(m_MyReader.GetValue(2).ToString(), out ToUserType);
                    }
                    else
                    {
                        ToUserId = FromUserId;
                        ToUserType = FromUserType;
                        int.TryParse(m_MyReader.GetValue(0).ToString(), out FromUserId);
                        int.TryParse(m_MyReader.GetValue(2).ToString(), out FromUserType);
                    }
                    // int.TryParse(m_MyReader.GetValue(3).ToString(), out DeptId);
                }
            }
            sql = "insert into tblservicerequestthreaddetails (ThreadId,Heading,Description,FromUserType,FromUserId,ToUserType,ToUserId,CreatedDate) values (" + thrdId + ",'" + subj + "','" + descrption + "'," + FromUserType + "," + FromUserId + "," + ToUserType + "," + ToUserId + ",'" + DateTime.Now.ToString("s") + "')";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public DataSet GetService_request_complaint_feedback(int user_type, int service_Type)
        {
            string sql = "select tblservicerequestthread.id as  ThreadId,tblstudent.StudentName as 'Name' , (select  tblservicerequestthreaddetails.Heading  from tblservicerequestthreaddetails where tblservicerequestthreaddetails.Id in  (select max(tblservicerequestthreaddetails.id) from tblservicerequestthreaddetails where tblservicerequestthreaddetails.ThreadId= tblservicerequestthread.id  ) )as  Subject,  (select  tblservicerequestthreaddetails.CreatedDate  from tblservicerequestthreaddetails where tblservicerequestthreaddetails.Id in  (select max(tblservicerequestthreaddetails.id) from tblservicerequestthreaddetails where tblservicerequestthreaddetails.ThreadId= tblservicerequestthread.id  ) )as  Date, (select  tblservicerequestthreaddetails.Description  from tblservicerequestthreaddetails where tblservicerequestthreaddetails.Id in  (select max(tblservicerequestthreaddetails.id) from tblservicerequestthreaddetails where tblservicerequestthreaddetails.ThreadId= tblservicerequestthread.id  ) )as  Description from tblservicerequestthread INNER JOIN tblstudent  ON tblstudent.Id= tblservicerequestthread.FromUserId  where (tblservicerequestthread.FromUserType = 2) or (tblservicerequestthread.ToUserType = 2) and  tblservicerequestthread.ServiceType=" + service_Type;
             return m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        }



        public bool IsUpdatedFeedback(int studentId, int classId, out int IncidentId)
        {
            IncidentId = 0;
            string sql = "select tblincident_teachersfeedback.id from tblincident_teachersfeedback where tblincident_teachersfeedback.StudentId=" + studentId + " and tblincident_teachersfeedback.ClassID=" + classId;
            DataSet dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
            {
                int.TryParse(dt.Tables[0].Rows[0]["id"].ToString(), out IncidentId);

                return true;


            }
            else
                return false;
        }


        public bool isFeedbackActivated(int StudentId, int CLASSID)
        {

            string sql = "select tblincident_teachersfeedback.id from tblincident_teachersfeedback where tblincident_teachersfeedback.StudentId=" + StudentId + " and tblincident_teachersfeedback.ClassID=" + CLASSID;
            DataSet dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
            {

                return true;


            }
            else
                return false;
        }

        public void UpdategmailIdintable(string gmail_fb, string email, int IsActiveSecure, int ParentId)
        {
            string sql = "update tblparent_parentdetails set " + gmail_fb + "='" + email + "' ,IsActiveSecure=" + IsActiveSecure + "  where tblparent_parentdetails.Id= " + ParentId;
            m_MysqlDb.ExecuteQuery(sql);

        }

        public bool EnabledSecureLogin(int p_id)
        {
            bool active =false;
            string sql = "select IsActiveSecure from tblparent_parentdetails where Id="+p_id;
            DataSet dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
            {
                if (int.Parse(dt.Tables[0].Rows[0]["IsActiveSecure"].ToString()) == 0)
                {
                     active =false;
                }
                else active = true;


            }
             return active;
        }

        public string getReportConfiguration()
        {
            string sql = "select Name,Value from tblparent_config where Name='ExamReport'";
             DataSet dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

             if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
             {
                 return (dt.Tables[0].Rows[0]["Value"].ToString());

             }
             else
                return "Grade";

        }
    }
    
}
