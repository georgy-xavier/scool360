<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;

public class Handler : IHttpHandler {
    string connectionString = "";
    public void ProcessRequest (HttpContext context) {
        string _returnstring = "";
        try
        {
            KnowinUser User = (KnowinUser)context.Session["UserObj"];
            connectionString = User.ConnectionString;
            context.Response.ContentType = "text/plain";
            string _UniqueId = context.Request["UniqueId"];
            string _DateTime = context.Request["DateTime"];
            string _Action = context.Request["Action"];
            DateTime _NewDate = new DateTime(2222, 2, 22);
            if(!DateTime.TryParse(_DateTime, out _NewDate))
            {
                 
                _NewDate = GetDateTimeFromStringFormat(_DateTime);
            
            }
            int _NewActionStatus = 0;
            int.TryParse(_Action, out _NewActionStatus);
           
            string Dateee = DateTime.Now.ToString();
            string _DimensionParameters = context.Request["DimensionParameters"];
            bool _returnvalue = RegisterInputPicData(_UniqueId, _NewDate, "1", _NewActionStatus, _DimensionParameters, _DateTime);
            if (_returnvalue)
            {
                _returnstring = "Requested action completed successfully";
            }
            else
            {
                _returnstring = "Requested action failed to complete";
            }
        }
        catch(Exception Ex)
        {
            _returnstring = "Exception Error: " + Ex.Message;
        }
        context.Response.Write(_returnstring);
    }

    public DateTime GetDateTimeFromStringFormat(string _DateTimestr)
    {
        DateTime _outDate = new DateTime();

        // 25/07/2012%2011:37:56%20AM
        _DateTimestr = _DateTimestr.Replace("%20", "$");
        string[] strarray = _DateTimestr.Split('$');
        if (strarray.Length > 0)
        {
            string[] _DateArray = strarray[0].Split('/');// store DD MM YYYY
            int _Day=0, _Month=0, _Year=0;
            int _h=0, _m=0, _S=0;
            if (_DateArray.Length == 3)
            {
                _Day = int.Parse(_DateArray[0]);// day
                _Month = int.Parse(_DateArray[1]);// Month
                _Year = int.Parse(_DateArray[2]);// Year
            }
            if (strarray.Length > 1)
            {
                string[] _TimeArray = strarray[1].Split(':');
                if (_TimeArray.Length == 3)
                {
                    _h = int.Parse(_TimeArray[0]);// HH
                    _m = int.Parse(_TimeArray[1]);// MM
                    _S = int.Parse(_TimeArray[2]);// SS
                }
            }
            _outDate = new DateTime(_Year, _Month, _Day, _h, _m, _S);
        }

       

        
        
        return _outDate;
    }

    public bool RegisterInputPicData(string _UniqueId, DateTime _DateTime, string _RFReaderID, int Action, string DimensionParameters, string _DateTimestr)
    {
        bool _return = true;
        string RFReaderType = "";
        int _AutreffId = 1;

        _return = RegisterAttencenceToDb_PicData(_AutreffId, _UniqueId, _DateTime, _RFReaderID, Action, DimensionParameters,_DateTimestr, out RFReaderType);


        if (_return && _UniqueId != "0" && _UniqueId != "-1")
        {

            MysqlClass _mysqlObj = new MysqlClass(connectionString);
            WinBase.Attendance MyAttendence = new WinBase.Attendance(_mysqlObj);
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

    private bool RegisterAttencenceToDb_PicData(int _AutreffId, string _UniqueId, DateTime _DateTime, string _RFReaderID, int Action, string DimensionParameters,string _DateTimestr, out string RFReaderType)
    {
        bool _valid = true;
        int _uid = 0;
        RFReaderType = "";
        try
        {
            int.TryParse(_UniqueId, out _uid);
            MysqlClass _mysqlObj = new MysqlClass(connectionString);
            WinBase.Attendance MyAttendence = new WinBase.Attendance(_mysqlObj);

            if (MyAttendence.isRFReaderRegistered(_RFReaderID))
            {
                if (_uid > 0)
                {
                    RFReaderType = MyAttendence.GetRFReaderType(_RFReaderID, _UniqueId, _DateTime);
                    string sql = "INSERT INTO tblexternalattencence(ExternalReffid,SessionId,ActionDate,RFReaderID,RFReaderType,ActionStatus,DimensionString,_DateTimestr) VALUES('" + _UniqueId + "'," + _AutreffId + ",'" + _DateTime.ToString("s") + "','" + _RFReaderID + "','" + RFReaderType + "'," + Action + ",'" + DimensionParameters + "','" + _DateTimestr + "')";
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
                        System.Data.Odbc.OdbcDataReader myreader = _mysqlObj.ExecuteQuery(sql);
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
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}