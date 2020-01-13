using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WinBase;
using System.Data;
using System.Data.Odbc;

namespace WinEr.WinerService
{
    /// <summary>
    /// Summary description for WinerService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WinerService : System.Web.Services.WebService
    {
        private SchoolClass objSchool = null;

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod(EnableSession = true)]
        public int Autenticate(string _UserName, string _PassWord)
        {
            int _reffid = 0;
            if (_PassWord == "a")
            {
                Random _Rnd = new Random(DateTime.Now.Millisecond);
                _reffid = _Rnd.Next(1, 500);


            }
            return _reffid;

        }


        [WebMethod(EnableSession = true)]
        public string GetTotalPendingFees(string _UniqueId,int schoolId)
        {
            string _return = "";
            objSchool = WinerUtlity.GetSchoolObject(schoolId);
            if (objSchool!=null && _UniqueId != "-1")
            {

                MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(objSchool));
                FeeManage MyFeeManage = new FeeManage(_mysqlObj);

                _return = MyFeeManage.GetTotalFeeAmount_Student(_UniqueId).ToString();

                _mysqlObj.CloseConnection();
                _mysqlObj = null;
                MyFeeManage = null;
            }
            return _return;
        }


        [WebMethod(EnableSession = true)]
        public bool RegisterInputPicData( string _UniqueId, DateTime _DateTime, string _RFReaderID, int Action,string DimensionParameters,int schoolId)
        {
            bool _return = true;
            string RFReaderType = "";
            int _AutreffId = 1;
            objSchool = WinerUtlity.GetSchoolObject(schoolId);
            _return = RegisterAttencenceToDb_PicData(_AutreffId, _UniqueId, _DateTime, _RFReaderID, Action, DimensionParameters, out RFReaderType);


            if (_return && _UniqueId != "0" && _UniqueId != "-1")
            {

                MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(objSchool));
                Attendance MyAttendence = new Attendance(_mysqlObj);
                if (Action == 1 || Action == 3)
                {
                    MyAttendence.MarkAttandance_External(_UniqueId, _DateTime, RFReaderType);
                }
                else if (Action == 2)
                {
                    MyAttendence.RemoveAttendance(_UniqueId, _DateTime, RFReaderType);
                }
                _mysqlObj.CloseConnection();
                _mysqlObj = null;
                MyAttendence = null;
            }
            return _return;
        }

        private bool RegisterAttencenceToDb_PicData(int _AutreffId, string _UniqueId, DateTime _DateTime, string _RFReaderID, int Action, string DimensionParameters, out string RFReaderType)
        {
            bool _valid = true;
            int _uid = 0;
            RFReaderType = "";
            try
            {
                int.TryParse(_UniqueId, out _uid);
                MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(objSchool));
                Attendance MyAttendence = new Attendance(_mysqlObj);

                if (MyAttendence.isRFReaderRegistered(_RFReaderID))
                {
                    if (_uid > 0)
                    {
                        RFReaderType = MyAttendence.GetRFReaderType(_RFReaderID, _UniqueId, _DateTime);
                        string sql = "INSERT INTO tblexternalattencence(ExternalReffid,SessionId,ActionDate,RFReaderID,RFReaderType,ActionStatus,DimensionString) VALUES('" + _UniqueId + "'," + _AutreffId + ",'" + _DateTime.ToString("s") + "','" + _RFReaderID + "','" + RFReaderType + "'," + Action + ",'" + DimensionParameters + "')";
                        _mysqlObj.ExecuteQuery(sql);


                        sql = "UPDATE tblexternalrfid SET LastTransaction='" + _DateTime.ToString("s") + "' WHERE EquipmentId='" + _RFReaderID + "'";
                        _mysqlObj.ExecuteQuery(sql);
                    }
                    else
                    {
                        string _msg = "Failure", laststatus = "";
                        string sql = "";
                        if (_uid == 0)
                        {
                            _msg = "Working";
                        }

                        if (_msg == "Failure")
                        {
                            sql = "SELECT `Status` FROM tblexternalrfid WHERE EquipmentId='" + _RFReaderID + "'";
                            OdbcDataReader myreader = _mysqlObj.ExecuteQuery(sql);
                            if (myreader.HasRows)
                            {
                                laststatus = myreader.GetValue(0).ToString();
                            }
                        }

                        if (_DateTime.Date == DateTime.Now.Date)
                        {
                            sql = "UPDATE tblexternalrfid SET `Status`='" + _msg + "',LastTransaction='" + _DateTime.ToString("s") + "' WHERE EquipmentId='" + _RFReaderID + "'";
                            _mysqlObj.ExecuteQuery(sql);

                            //try
                            //{

                            //    if (_msg == "Failure")
                            //    {
                            //        MyAttendence.CheckRFDeviceStatus_SendWarning(_RFReaderID, _DateTime, laststatus);
                            //    }

                            //}
                            //catch
                            //{
                            //    _valid = false;
                            //}
                        }
                    }

                }
                else
                {
                    string sql = "INSERT INTO tblexternalunregisteredrfid(RFReaderID,ExternalReffid,SessionId,ActionDate) VALUES('" + _RFReaderID + "','" + _UniqueId + "'," + _AutreffId + ",'" + _DateTime.ToString("s") + "')";
                    _mysqlObj.ExecuteQuery(sql);
                    _valid = false;
                }


                _mysqlObj.CloseConnection();
                _mysqlObj = null;
                MyAttendence = null;
            }
            catch
            {
                _valid = false;
            }

            return _valid;
        }



        [WebMethod]
        public bool RegisterInput(string rfparameterm, out string message)
        {

            bool call_return = false;
            message = "";
            try
            {
                string MainDiv = "", SubDiv = "$Subspliter$";
                string[] Maindiv = rfparameterm.Split(new string[] { "$Mainspliter$" }, StringSplitOptions.RemoveEmptyEntries);
               
                for (int i = 0; i < Maindiv.Length; i++)
                {

                    bool _return = true;
                    string[] Subdiv = Maindiv[i].Split(new string[] { "$Subspliter$" }, StringSplitOptions.RemoveEmptyEntries);
                    string Id = "";
                    int _AutreffId = 0;
                    string _UniqueId = "";
                    DateTime _DateTime = new DateTime();
                    string _RFReaderID = "";
                    int schoolId = 0;

                    Id = Subdiv[0];
                    int.TryParse(Subdiv[1], out _AutreffId);
                    _UniqueId = Subdiv[2];
                    DateTime.TryParse(Subdiv[3], out _DateTime);
                    _RFReaderID = Subdiv[4];
                    if (Subdiv.Length==6)
                    {
                        int.TryParse(Subdiv[5], out schoolId);
                    }
                    string RFReaderType = "";
                    if (_AutreffId == 0)
                    {
                        _return = false;
                    }
                    else
                    {
                        objSchool = WinerUtlity.GetSchoolObject(schoolId);
                        _return = RegisterAttencenceToDb(_AutreffId, _UniqueId, _DateTime, _RFReaderID, out RFReaderType);

                    }
                    if (_return && _UniqueId != "0" && _UniqueId != "-1")
                    {

                        MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(objSchool));
                        Attendance MyAttendence = new Attendance(_mysqlObj);

                        MyAttendence.MarkAttandance_External(_UniqueId, _DateTime, RFReaderType);

                        _mysqlObj.CloseConnection();
                        _mysqlObj = null;
                        MyAttendence = null;
                    }

                    message = message + MainDiv + Id + SubDiv + _return;
                    MainDiv = "$Mainspliter$";
                    call_return = true;
                }
            }
            catch
            {
                call_return = false;
            }
   
            return call_return;
        }


        private bool RegisterAttencenceToDb(int _AutreffId, string _UniqueId, DateTime _DateTime, string _RFReaderID, out string RFReaderType)
        {
            bool _valid = true;
            int _uid = 0;
            RFReaderType = "";
            try
            {
                int.TryParse(_UniqueId, out _uid);
                MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(objSchool));
                Attendance MyAttendence = new Attendance(_mysqlObj);

                if (MyAttendence.isRFReaderRegistered(_RFReaderID))
                {
                    if (_uid > 0)
                    {
                        RFReaderType = MyAttendence.GetRFReaderType(_RFReaderID, _UniqueId, _DateTime);
                        string sql = "INSERT INTO tblexternalattencence(ExternalReffid,SessionId,ActionDate,RFReaderID,RFReaderType) VALUES('" + _UniqueId + "'," + _AutreffId + ",'" + _DateTime.ToString("s") + "','" + _RFReaderID + "','" + RFReaderType + "')";
                        _mysqlObj.ExecuteQuery(sql);


                        sql = "UPDATE tblexternalrfid SET LastTransaction='" + _DateTime.ToString("s") + "' WHERE EquipmentId='" + _RFReaderID + "'";
                        _mysqlObj.ExecuteQuery(sql);
                    }
                    else
                    {
                        string _msg = "Failure",laststatus="";
                        string sql = "";
                        if (_uid == 0)
                        {
                            _msg = "Working";
                        }

                        if (_msg == "Failure")
                        {
                            sql = "SELECT `Status` FROM tblexternalrfid WHERE EquipmentId='" + _RFReaderID + "'";
                            OdbcDataReader myreader = _mysqlObj.ExecuteQuery(sql);
                            if (myreader.HasRows)
                            {
                                laststatus = myreader.GetValue(0).ToString();
                            }
                        }

                        if (_DateTime.Date == DateTime.Now.Date)
                        {
                            sql = "UPDATE tblexternalrfid SET `Status`='" + _msg + "',LastTransaction='" + _DateTime.ToString("s") + "' WHERE EquipmentId='" + _RFReaderID + "'";
                            _mysqlObj.ExecuteQuery(sql);

                            try
                            {

                                if (_msg == "Failure")
                                {
                                    MyAttendence.CheckRFDeviceStatus_SendWarning(_RFReaderID, _DateTime, laststatus);
                                }

                            }
                            catch
                            {
                                _valid = false;
                            }
                        }
                    }

                }
                else
                {
                    string sql = "INSERT INTO tblexternalunregisteredrfid(RFReaderID,ExternalReffid,SessionId,ActionDate) VALUES('" + _RFReaderID + "','" + _UniqueId + "'," + _AutreffId + ",'" + _DateTime.ToString("s") + "')";
                    _mysqlObj.ExecuteQuery(sql);
                    _valid = false;
                }


                _mysqlObj.CloseConnection();
                _mysqlObj = null;
                MyAttendence = null;
            }
            catch
            {
                _valid = false;
            }

            return _valid;
        }


        [WebMethod]
        public bool GetRF_userDataSet(string SchoolId, out DataSet UserDataSet)
        {
            bool valid = false;
            int schoolId = 0;
            int.TryParse(SchoolId, out schoolId);
            objSchool = WinerUtlity.GetSchoolObject(schoolId);
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(objSchool));
            Attendance MyAttendence = new Attendance(_mysqlObj);

            UserDataSet = new DataSet();
            UserDataSet.Tables.Add(new DataTable("USER"));
            DataTable dt;
            DataRow dr;
            dt = UserDataSet.Tables["USER"];
            dt.Columns.Add("ID");
            dt.Columns.Add("UserName");
            dt.Columns.Add("Password");
            dt.Columns.Add("UserType");
            dt.Columns.Add("RefferenceId");
            dt.Columns.Add("IsActive");
            dt.Columns.Add("SystemId");
            dt.Columns.Add("SystemUserType");
            dt.Columns.Add("NeedUpload");
            string sql = "SELECT ExternalReffId,UserId,UserName,UserType,ISActive,Id FROM tblexternalreff";
            OdbcDataReader MyUserReader = MyAttendence.m_MysqlDb.ExecuteQuery(sql);
            if (MyUserReader.HasRows)
            {
                while (MyUserReader.Read())
                {
                    dr = UserDataSet.Tables["USER"].NewRow();

                    dr["ID"] = MyUserReader.GetValue(5).ToString();
                    dr["UserName"] = MyUserReader.GetValue(2).ToString();
                    dr["Password"] = "123";
                    dr["UserType"] = "0";
                    dr["RefferenceId"] = MyUserReader.GetValue(0).ToString();
                    dr["IsActive"] = MyUserReader.GetValue(4).ToString();
                    dr["SystemId"] = MyUserReader.GetValue(1).ToString();
                    dr["SystemUserType"] = MyUserReader.GetValue(3).ToString();
                    dr["NeedUpload"] = "1";
                    UserDataSet.Tables["USER"].Rows.Add(dr);
                }
            }

            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyAttendence = null;
            return valid;
        }


        [WebMethod]
        public bool GetRF_ReaderDataSet(string SchoolId,out DataSet ReaderDataSet)
        {
            bool valid = false;

            int schoolId = 0;
            int.TryParse(SchoolId, out schoolId);
            objSchool = WinerUtlity.GetSchoolObject(schoolId);
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(objSchool));
            Attendance MyAttendence = new Attendance(_mysqlObj);

            ReaderDataSet = new DataSet();
            ReaderDataSet.Tables.Add(new DataTable("Reader"));
            DataTable dt;
            DataRow dr;
            dt = ReaderDataSet.Tables["Reader"];
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            dt.Columns.Add("IP");
            dt.Columns.Add("Port");
            dt.Columns.Add("LastTransaction");
            dt.Columns.Add("IsActive");
            dt.Columns.Add("Status");
            dt.Columns.Add("Device");

            string sql = "SELECT EquipmentId,Location,IP,Port,LastTransaction,IsActive,Status,Device FROM tblexternalrfid";
            OdbcDataReader MyReader = MyAttendence.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    dr = ReaderDataSet.Tables["Reader"].NewRow();

                    dr["ID"] = MyReader.GetValue(0).ToString();
                    dr["Name"] = MyReader.GetValue(1).ToString();
                    dr["IP"] = MyReader.GetValue(2).ToString();
                    dr["Port"] = MyReader.GetValue(3).ToString();
                    dr["LastTransaction"] = MyReader.GetValue(4).ToString();
                    dr["IsActive"] = MyReader.GetValue(5).ToString();
                    dr["Status"] = MyReader.GetValue(6).ToString();
                    dr["Device"] = MyReader.GetValue(7).ToString();
                    ReaderDataSet.Tables["Reader"].Rows.Add(dr);
                }
            }

            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyAttendence = null;
            return valid;
        }

        #region mani ivr integration work

        [WebMethod(EnableSession = true)]
        public bool CheckParentPhoneNumber(string key, string phoneNumber, int SchoolId, out string ErrorMessage, out string[] studentdetails)
        {
            DateTime Intime = DateTime.Now;
            bool valid = true;
            studentdetails=null;
            ErrorMessage = "";
            objSchool = WinerUtlity.GetSchoolObject(SchoolId);
            if (objSchool != null)
            {
                MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(objSchool));
                StudentManagerClass _objmanager = new StudentManagerClass(_mysqlObj);
                if (!_objmanager.GetstudentIdfromPhonenumber(phoneNumber, out ErrorMessage, out studentdetails))
                {
                    valid = false;
                }

                #region log updation

                DateTime OutTime = DateTime.Now;
                string studentinformation="";
                if (studentdetails!=null && ErrorMessage == "")
                {
                    foreach (string info in studentdetails)
                    {
                        studentinformation += studentinformation + " ";
                    }
                }
                else
                    studentinformation = ErrorMessage;

                string m_stConnection = WinerUtlity.CentralConnectionString;
                MysqlClass m_MysqlDb = new MysqlClass(m_stConnection);
                string sql = "INSERT into tblivrlog_details(SchoolId,Userinfo,Mobilno,Logintime,Logouttime,StudentDetails) VALUES (" + SchoolId + ",'" + key + "','" + phoneNumber + "','" + Intime.ToString("s") + "','" + OutTime.ToString("s") + "','" + studentinformation + "')";
                m_MysqlDb.ExecuteQuery(sql);
                
                #endregion



            }
            return valid;
           
        }


        #endregion

    }
}
