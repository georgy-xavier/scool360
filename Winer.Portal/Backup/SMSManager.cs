using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Net;
using WinBase;
using System.Data;

public class SMSManager:KnowinGen
{
    public MysqlClass m_MysqlDb;
    private OdbcDataReader m_MyReader = null;
    private string m_SMSMenuStr;
    private string m_SubSMSMenuStr;
    private string mCheckURL = "http://122.166.5.17/desk2web/CreditCheck.aspx?Username=($UN$)&Password=($PS$)";
    private string mSendURL = "http://122.166.5.17/desk2web/SendSMS.aspx?UserName=($UN$)&password=($PS$)&MobileNo=($MN$)&SenderID=($SID$)&CDMAHeader=($CID$)&Message=($SMS$)&isFlash=($FLS$)";
    private string mCreaditURL = "http://122.166.5.17/desk2web/CreditCheck.aspx?Username=($UN$)&Password=($PS$)";
    private string mUsername = "";
    private string mPassword = "";
    private string mNeedCheckBalance = "0";
    private string mDeliveryStatus = "Delivered";
    private string mNoAddl = "";
    private int mMaxCount = 0;
    private char mSymbol = new char();
    private string mError = "";
    private string mSMSReply = "";
    public string mLimit = "";
    public string mSendCount = "";

    public SMSManager(KnowinGen _Prntobj)
    {
        m_Parent = _Prntobj;
        m_MyODBCConn = m_Parent.ODBCconnection;
        m_UserName = m_Parent.LoginUserName;
        m_MysqlDb = new MysqlClass(this);
        m_SMSMenuStr = "";
        m_SubSMSMenuStr = "";
    }

    ~SMSManager()
    {
        if (m_MysqlDb != null)
        {
            m_MysqlDb = null;

        } if (m_MyReader != null)
        {
            m_MyReader = null;

        }
       
    }

    public string GetSMSMangMenuString(int _roleid)
    {
        CLogging logger = CLogging.GetLogObject();
        string _MenuStr;
        if (m_SMSMenuStr == "")
        {


            _MenuStr = "<ul>";
            logger.LogToFile("GetSMSMangMenuString", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
            string sql = "SELECT DISTINCT tblaction.MenuName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND tblroleactionmap.ModuleId=23 AND tblaction.ActionType='Link' order by tblaction.`Order` Asc";
            logger.LogToFile("GetSMSMangMenuString", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                logger.LogToFile("GetSMSMangMenuString", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                while (m_MyReader.Read())
                {


                    _MenuStr = _MenuStr + "<li><a href=\"" + m_MyReader.GetValue(1).ToString() + "\">" + m_MyReader.GetValue(0).ToString() + "</a></li>";
                }

            }
            _MenuStr = _MenuStr + "</ul>";
            logger.LogToFile("GetSMSMangMenuString", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Close();
            m_SMSMenuStr = _MenuStr;


        }
        else
        {
            _MenuStr = m_SMSMenuStr;
        }
        logger.LogToFile("GetSMSMangMenuString", " exiting from module  ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        return _MenuStr;

    }

    public Boolean InitClass()
    {
       
        mCheckURL = GetCheckURL_FromDatabase();
        mSendURL = GetSendURL_FromDatabase();
        mSymbol = GetSMS_NumberSeperator_FromDatabase();
        mUsername = GetSMS_username_FromDatabase();
        mPassword = GetSMS_password_FromDatabase();
        mMaxCount = GetSMS_Max_Count_FromDatabase();
        mNoAddl = GetSMS_NoAddl_FromDatabase();
        mNeedCheckBalance = GetSMS_mNeedCheckBalance_FromDatabase();
        mDeliveryStatus = GetDeliveyStatus_String();
        return true;
    }

    private string GetDeliveyStatus_String()
    {
        string str = "";
        string sql = "SELECT Value FROM tblsmsconfig WHERE `Type`='Delivery Status'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            str = m_MyReader.GetValue(0).ToString();
        }
        return str;
    }

    private string GetSMS_mNeedCheckBalance_FromDatabase()
    {
        string str = "";
        string sql = "SELECT Value FROM tblsmsconfig WHERE `Type`='Need Check Balance'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            str = m_MyReader.GetValue(0).ToString();
        }
        return str;
    }



# region SMS Constant Area



    private string GetSMS_NoAddl_FromDatabase()
    {
        string str = "";
        string sql = "SELECT Value FROM tblsmsconfig WHERE `Type`='Need Additional Number'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            str = m_MyReader.GetValue(0).ToString();
        }
        return str;
    }
    private string GetSMS_password_FromDatabase()
    {
        string str = "";
        string sql = "SELECT Value FROM tblsmsconfig WHERE `Type`='Password'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            str = m_MyReader.GetValue(0).ToString();
        }
        return str;
    }
    public char GetSMS_NumberSeperator_FromDatabase()
    {
        char str=new char();
        string sql = "SELECT Value FROM tblsmsconfig WHERE `Type`='Seperator'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            str = char.Parse(m_MyReader.GetValue(0).ToString());
        }
        return str;
    }
    private int GetSMS_Max_Count_FromDatabase()
    {
        int str = 0;
        string sql = "SELECT Value FROM tblsmsconfig WHERE `Type`='MaxCount'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
           int.TryParse( m_MyReader.GetValue(0).ToString(),out str);
        }
        return str;
    }

    private string GetSMS_username_FromDatabase()
    {
        string str = "";
        string sql = "SELECT Value FROM tblsmsconfig WHERE `Type`='UserName'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            str = m_MyReader.GetValue(0).ToString();
        }
        return str;
    }

    private string GetSendURL_FromDatabase()
    {
        string str = "";
        string sql = "SELECT Value FROM tblsmsconfig WHERE `Type`='SendURL'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            str = m_MyReader.GetValue(0).ToString();
        }
        return str;
    }

    private string GetCheckURL_FromDatabase()
    {
        string str = "";
        string sql = "SELECT Value FROM tblsmsconfig WHERE `Type`='CheckURL'";
         m_MyReader = m_MysqlDb.ExecuteQuery(sql);
         if (m_MyReader.HasRows)
         {
             str = m_MyReader.GetValue(0).ToString();
         }
         return str;
    }

# endregion

#region SMS Sending
    public string GetSMSReply()
    {
        return mSMSReply;
    }
    public string GetLastError()
    {
        return mError;
    }

    public bool CheckConnection(out string msg)
    {
        msg = "";
        string URL = GetCheckURL(out msg);

        if (URL != "")
        {
            if (CheckSMSBalance(URL, out msg))
            {
                if (mNeedCheckBalance == "0")
                {
                    msg = "Successfully Connected";
                }
                return true;
            }
            else
            {
                msg = "Connection Failed";
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private bool CheckSMSBalance(string SMSURL,out string msg)
    {
        msg = "";
        mSMSReply = "";
        mError = "";
        try
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(SMSURL);
            WebResponse resp = req.GetResponse();
            System.IO.Stream stream = resp.GetResponseStream();
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            mSMSReply = reader.ReadToEnd();
            msg = "Successfully Connected <br/><br/> " + mSMSReply;
            return true;
        }
        catch
        {
            return false;
        }
        //Response.Write(reader.ReadToEnd);
    }

    public bool GetSMSCreaditDetails()
    {
        string URL = GetCreditCheckURL();

        if (SendSMS(URL))
        {
            if (mSMSReply.Length > 0)
            {
                string[] sArray = mSMSReply.Split(' ');
                string[] contArray;
                string Name = "";
                for (int i = 0; i < sArray.Length; i++)
                {
                    contArray = (sArray[i].ToString()).Split('=');
                    Name = contArray[0].ToString();
                    if (string.Compare(Name, "Limit") == 0)
                    {
                        mLimit = contArray[1].Substring(1, (contArray[1].ToString()).Length - 2);

                    }
                    if (string.Compare(Name, "Used") == 0)
                    {
                        mSendCount = contArray[1].Substring(1, (contArray[1].ToString()).Length - 2);
                    }

                }

            }

            return true;
        }
        else
        {
            return false;
        }
    }
    /* Numbers - Comma Seperated Numbers
     * SenderId- Sender Id-This field will hold the Header that will be displayed on the GSM mobile phones
      CDMAHeader- CDMAHeader, This field will hold the value that will be shown as the header on the CDMA mobile no CDMA
                header, should be a 12 digit mobile no with the country code The CDMA header should be a
                valid GSM mobile no, the SMS packets will be rejected if this field is not a valid 12 digit GSM
                number and the user account will be charged for the messages rejected
      SMS- SMS to send maximum 160 Char
      Flash- (True/False). if true message will be send as Flash messsage.
     */
    public bool SendBULKSms(string Numbers, string SMS, string CDMAHeader, string SenderId, bool Flash, out string failedNumberString)
    {
        
        failedNumberString="";
        //byte[] urlBytes = Encoding.ASCII.GetBytes(SMS);
        // = Convert.ToBase64String(urlBytes);
        try
        {
            if (mUsername != "" && mPassword != "")
            {
                string urlB64 = SMS.Replace("=", "%3D");
                string SMSURL = "";
                Numbers = Numbers.Replace(',', mSymbol);
                string[] Numstr = Numbers.Split(mSymbol);
                if (mNoAddl != "")
                {
                    string symbol = "";
                    Numbers = "";
                    for (int x = 0; x < Numstr.Length; x++)
                    {
                        if (!String.IsNullOrEmpty(Numstr[x]))
                        {
                            Numstr[x] = mNoAddl + Numstr[x].Trim();
                            Numbers = Numbers + symbol + Numstr[x];
                            symbol = mSymbol.ToString();
                        }
                    }
                }
                if (Numstr.Length > mMaxCount)
                {
                    int count = Numstr.Length / mMaxCount;
                    if (Numstr.Length % mMaxCount != 0)
                    {
                        count++;
                    }
                    bool _continue = true;
                    string number_str = "";
                    int i;

                    for (i = 0; i < count; i++)
                    {
                        number_str = "";
                        SMSURL = "";
                        bool _IsLast = false;
                        if (i == count - 1)
                        {
                            _IsLast = true;
                        }
                        number_str = GetSMS_NumberList(Numstr, i, _IsLast);
                        SMSURL = SendSMSURL(number_str, urlB64, CDMAHeader, SenderId, Flash);

                        if (SMSURL.Length > 0)
                        {
                            _continue = SendSMS(SMSURL);

                            if (!_continue)
                            {
                                if (String.IsNullOrEmpty(failedNumberString))
                                    failedNumberString = number_str;
                                else
                                    failedNumberString = failedNumberString + mSymbol.ToString() + number_str;
                            }
                        }



                    }
                    if (_continue)
                    {
                        string symbol = "";
                        number_str = "";
                        SMSURL = "";
                        int maxcount = mMaxCount;
                        for (int j = i * maxcount; j <= (Numstr.Length) - 1; j++)
                        {
                            number_str = number_str + symbol + Numstr[j];
                            symbol = mSymbol.ToString();
                        }
                        SMSURL = SendSMSURL(number_str, urlB64, CDMAHeader, SenderId, Flash);
                        if (SMSURL.Length > 0)
                        {
                            _continue = SendSMS(SMSURL);
                            if (!_continue)
                            {
                                if (String.IsNullOrEmpty(failedNumberString))
                                    failedNumberString = number_str;
                                else
                                    failedNumberString = failedNumberString + mSymbol.ToString() + number_str;
                            }

                        }

                    }
                    return _continue;
                }
                else
                {
                    SMSURL = SendSMSURL(Numbers, urlB64, CDMAHeader, SenderId, Flash);
                    if (SMSURL.Length > 0)
                    {
                        if (SendSMS(SMSURL))
                        {
                            return true;
                        }
                        else
                        {

                            if (String.IsNullOrEmpty(failedNumberString))
                                failedNumberString = Numbers;
                            else
                                failedNumberString = failedNumberString + mSymbol.ToString() + Numbers;

                        }
                    }
                }

                return false;
            }
            else
            {
                return false;
            }

          
        }
        catch
        {
            return false;
        }
        finally
        {
            //code remove prefix 91 for failed sms number
            if (!String.IsNullOrEmpty(failedNumberString))
            {

                string[] fail_Numstr = failedNumberString.Split(mSymbol);
                failedNumberString = "";
                string fail_Number = "";
                for (int x = 0; x < fail_Numstr.Length; x++)
                {
                    if (!String.IsNullOrEmpty(fail_Numstr[x]))
                    {
                        fail_Number = fail_Numstr[x].Trim().Substring(2);
                        if (String.IsNullOrEmpty(failedNumberString))
                        {
                            failedNumberString = fail_Number;
                        }
                        else
                        {
                            failedNumberString = failedNumberString + mSymbol.ToString() + fail_Number;
                        }
                    }
                }
            }

            //
        }
    }

    private string GetSMS_NumberList(string[] Numstr, int i,bool _lastList)
    {
        int MaxCount = mMaxCount;
        string phone_NUM = "",symbol="";
        if (_lastList)
        {
            for (int j = i * MaxCount; j < Numstr.Length; j++)
            {
                phone_NUM = phone_NUM + symbol + Numstr[j];
                symbol = mSymbol.ToString();
            }
        }
        else
        {
            for (int j = i * MaxCount; j <= ((i + 1) * MaxCount) - 1; j++)
            {
                phone_NUM = phone_NUM + symbol + Numstr[j];
                symbol = mSymbol.ToString();
            }
        }
        return phone_NUM;
    }


    public String GetCheckURL(out string msg)
    {
        msg = "";
        string URL = "";
        if (mUsername != "" && mPassword != "")
        {
            URL = mCheckURL.Replace("($UN$)", mUsername);
            URL = URL.Replace("($PS$)", mPassword);
        }
        else
        {
            msg = "SMS Configuration is not complete";
        }
        return URL;
    }

    private String GetCreditCheckURL()
    {
        string URL = mCreaditURL.Replace("($UN$)", mUsername);
        URL = URL.Replace("($PS$)", mPassword);
        return URL;
    }

    private string SendSMSURL(string Numbers, string SMS, string CDMAHeader, string SenderId, bool Flash)
    {
        string SMSURL = "";

        string URL = mSendURL.Replace("($UN$)", mUsername);
        URL = URL.Replace("($PS$)", mPassword);
        URL = URL.Replace("($CID$)", CDMAHeader);
        URL = URL.Replace("($SID$)", SenderId);
        if (Flash == true)
            URL = URL.Replace("($FLS$)", "true");
        else
            URL = URL.Replace("($FLS$)", "false");

        URL = URL.Replace("($SMS$)", SMS);

        string[] Numstr = Numbers.Split(mSymbol);
        if (Numstr.Length > mMaxCount)
        {
            mError = "Maximum of " + mMaxCount + " Numbers";
            SMSURL = "";
            return SMSURL;
        }

        SMSURL = URL.Replace("($MN$)", Numbers);

        return SMSURL;
    }
    private bool SendSMS(string SMSURL)
    {
        mSMSReply = "";
        mError = "";
        try
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(SMSURL);
            WebResponse resp = req.GetResponse();
            System.IO.Stream stream = resp.GetResponseStream();
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            mSMSReply = reader.ReadToEnd();
            if (mSMSReply.ToUpperInvariant().Contains(mDeliveryStatus.ToUpperInvariant()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
        //Response.Write(reader.ReadToEnd);
    }

    #endregion

#region SMS Manging


    // ARUN FOR SMS TEMPLATE
    //public string Get_StudentPhoneNo_List(int classId)
    //{
    //    string phno = "", tempSQl = "", seperator = "";
    //    if (classId != -1)
    //    {
    //        tempSQl = " AND tblstudentclassmap.ClassId=" + classId;
    //    }
    //    string sql = "SELECT DISTINCT tblsmsstudentlist.PhoneNo,tblsmsstudentlist.Id FROM tblsmsstudentlist INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tblsmsstudentlist.Id WHERE tblsmsstudentlist.Enabled=1 " + tempSQl;
    //    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    //    if (m_MyReader.HasRows)
    //    {
    //        while (m_MyReader.Read())
    //        {
    //            phno = phno + seperator + m_MyReader.GetValue(0).ToString() + "##" + m_MyReader.GetValue(1).ToString() + "##2";
    //            seperator = "$$";
    //        }
    //    }
    //    return phno;
    //}

    //public string Get_ParentPhoneNo_List(int classId)
    //{
    //    string phno = "", tempSQl = "", seperator = "";
    //    if (classId != -1)
    //    {
    //        tempSQl = " AND tblstudentclassmap.ClassId=" + classId;
    //    }
    //    string sql = "SELECT DISTINCT tblsmsparentlist.PhoneNo,tblsmsparentlist.Id FROM tblsmsparentlist INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tblsmsparentlist.Id  WHERE tblsmsparentlist.Enabled=1" + tempSQl;
    //    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    //    if (m_MyReader.HasRows)
    //    {
    //        while (m_MyReader.Read())
    //        {
    //            phno = phno + seperator + m_MyReader.GetValue(0).ToString() + "##" + m_MyReader.GetValue(1).ToString() + "##1";
    //            seperator = "$$";
    //        }
    //    }
    //    return phno;
    //}

    //public string Get_StaffPhoneNo_List(string _StaffId)
    //{
    //    string phno = "", tempSQl = "";
    //    string symbol = "";
    //    if (_StaffId != "")
    //        tempSQl = " AND tblsmsstafflist.Id in(" + _StaffId + ")";
    //    string sql = "SELECT DISTINCT tblsmsstafflist.PhoneNo,tbluser.Id FROM tblsmsstafflist INNER JOIN tbluser ON tbluser.Id=tblsmsstafflist.Id WHERE tblsmsstafflist.Enabled=1" + tempSQl;
    //    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
    //    if (m_MyReader.HasRows)
    //    {
    //        while (m_MyReader.Read())
    //        {
    //            phno = phno + symbol.ToString() + m_MyReader.GetValue(0).ToString() + "##" + m_MyReader.GetValue(1).ToString()+"##0";
    //            symbol = "$$";
    //        }
    //    }
    //    return phno;
    //}



    public string Get_StudentPhoneNo_List(string Class_Ids)
    {
        string phno = "", tempSQl = "", seperator = "";
        if (Class_Ids !="")
        {
            tempSQl = " AND tblstudentclassmap.ClassId in ("+ Class_Ids +")";
        }
        string sql = "SELECT DISTINCT tblsmsstudentlist.PhoneNo FROM tblsmsstudentlist INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tblsmsstudentlist.Id WHERE tblsmsstudentlist.Enabled=1 " + tempSQl;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                phno = phno + seperator + m_MyReader.GetValue(0).ToString() ;
                seperator = ",";
            }
        }
        return phno;
    }

    public string Get_ParentPhoneNo_List(string temp_classIds)
    {
        string phno = "", tempSQl = "", seperator = "";
        if (temp_classIds !="")
        {
            tempSQl = " AND tblstudentclassmap.ClassId in (" + temp_classIds + ")";
        }
        string sql = "SELECT DISTINCT tblsmsparentlist.PhoneNo,tblsmsparentlist.SecondaryNo FROM tblsmsparentlist INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tblsmsparentlist.Id  WHERE tblsmsparentlist.Enabled=1 and IsActiveNativeLanguage=0" + tempSQl;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                phno = phno + seperator + m_MyReader.GetValue(0).ToString();
                seperator = ",";
                if (m_MyReader.GetValue(1).ToString() != "")
                    phno = phno + seperator + m_MyReader.GetValue(1).ToString();
            }
        }
        return phno;
    }

    public string Get_StaffPhoneNo_List(string _StaffId)
    {
        string phno = "", tempSQl = "";
        string symbol = "";
        if (_StaffId != "")
            tempSQl = " AND tblsmsstafflist.Id in(" + _StaffId + ")";
        string sql = "SELECT DISTINCT tblsmsstafflist.PhoneNo,tbluser.Id FROM tblsmsstafflist INNER JOIN tbluser ON tbluser.Id=tblsmsstafflist.Id WHERE tblsmsstafflist.Enabled=1" + tempSQl;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                phno = phno + symbol.ToString() + m_MyReader.GetValue(0).ToString();
                symbol = ",";
            }
        }
        return phno;
    }

    public string Get_SelectedParentPhoneNo_List(string _StudentId)
    {
        string phno = "", seperator = "", tempSQl="";
        char symbol = mSymbol;
        if(_StudentId!="")
            tempSQl = " AND tblstudentclassmap.StudentId in(" + _StudentId+")";
        string sql = "SELECT DISTINCT tblsmsparentlist.PhoneNo,tblsmsparentlist.SecondaryNo FROM tblsmsparentlist INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tblsmsparentlist.Id  WHERE tblsmsparentlist.Enabled=1" + tempSQl;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                phno = phno + seperator + m_MyReader.GetValue(0).ToString();
                seperator = symbol.ToString();
                if(m_MyReader.GetValue(1).ToString()!="")
                    phno = phno + seperator + m_MyReader.GetValue(1).ToString();
            }
        }
        return phno;
    }

    // sai added for sms group
    public string Get_PhoneNo_List(string _StudentId, out bool Is_Nativesmsneed)
    {
        Is_Nativesmsneed = false;
        string phno = "", seperator = "", tempSQl = "";
        char symbol = mSymbol;
        if (_StudentId != "")
            tempSQl = " AND tblstudentclassmap.StudentId in(" + _StudentId + ")";
        string sql = "SELECT DISTINCT tblsmsparentlist.PhoneNo,tblsmsparentlist.IsActiveNativeLanguage FROM tblsmsparentlist INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tblsmsparentlist.Id  WHERE tblsmsparentlist.Enabled=1" + tempSQl;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                phno = phno + seperator + m_MyReader.GetValue(0).ToString();
                seperator = symbol.ToString();
                if (int.Parse(m_MyReader.GetValue(1).ToString())==1)
                {
                    Is_Nativesmsneed = true;
                }
            }
        }
        return phno;
    }

    # region unpaid SMS

    public string GenerateSMSstring(string _PlaneText, string _FatherName, string _StudentName, string Amnt, string _Batch)
    {
        DateTime _Now = System.DateTime.Now;
        string _Today = (_Now.Date).ToString("dd/MM/yyyy");
        _PlaneText = _PlaneText.Replace("($Parent$)", _FatherName);
        _PlaneText = _PlaneText.Replace("($Student$)", _StudentName);
        _PlaneText = _PlaneText.Replace("($Amt$)", Amnt);
        _PlaneText = _PlaneText.Replace("($Today$)", _Today);
        _PlaneText = _PlaneText.Replace("($Date$)", _Today);
        _PlaneText = _PlaneText.Replace("($Batch$)", _Batch);
        return _PlaneText;
    }

    # endregion
# endregion

   


    #region Trip SMS

    public string Get_DriverAndTripStudentParentPhoneNo_List(int tripId, int Recivr)
    {
        string phno = "", seperator = "", sql = "", driversql = "";
        char symbol = mSymbol;
        string parentsql = "SELECT DISTINCT tblsmsparentlist.PhoneNo as ContactNo FROM tblsmsparentlist INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tblsmsparentlist.Id INNER JOIN tbl_tr_studtripmap ON tbl_tr_studtripmap.StudId =tblstudentclassmap.StudentId  WHERE tblsmsparentlist.Enabled=1 and( tbl_tr_studtripmap.FromTripId = " + tripId + " or tbl_tr_studtripmap.ToTripId = " + tripId + " )";
        driversql = " select tbl_tr_trips.ContactNo from tbl_tr_trips where tbl_tr_trips.id =" + tripId + "";
        if (Recivr == 1)
        {
            sql = parentsql;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    phno = phno + seperator + m_MyReader.GetValue(0).ToString();
                    seperator = symbol.ToString();
                }
            }
        }
        else if (Recivr == 2)
        {
            sql = driversql;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    phno = phno + seperator + m_MyReader.GetValue(0).ToString();
                    seperator = symbol.ToString();
                }
            }
        }
        else
        {
            sql = parentsql;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    phno = phno + seperator + m_MyReader.GetValue(0).ToString();
                    seperator = symbol.ToString();
                }
            }


            sql = driversql;
            m_MyReader = null;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    phno = phno + seperator + m_MyReader.GetValue(0).ToString();
                    seperator = symbol.ToString();
                }
            }
        }

        return phno;
    }

    #endregion

    # region ParentLoginSMS

    public string ParentLoginSMSstring(string _PlaneText, string _FatherName, string _Url , string _SchoolName , string _UserName , string _PassWord)
    {
        _PlaneText = _PlaneText.Replace("($Parent$)", _FatherName);
        _PlaneText = _PlaneText.Replace("($Url$)", _Url);
        _PlaneText = _PlaneText.Replace("($School$)", _SchoolName);
        _PlaneText = _PlaneText.Replace("($UserName$)", _UserName);
        _PlaneText = _PlaneText.Replace("($Password$)", _PassWord);
        return _PlaneText;
    }
    public string GenerateParentLoginPassword()
    {
        string _Random = RandomString(6, false);
        return _Random;
    }
    private string RandomString(int size, bool lowerCase)
    {
        StringBuilder builder = new StringBuilder();
        Random random = new Random();
        char ch;
        for (int i = 0; i < size; i++)
        {
            ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
            builder.Append(ch);
        }
        if (lowerCase)
            return builder.ToString().ToLower();
        return builder.ToString();
    }

    public bool SaveParentCredentials(string _PassWord, string _UserName, string _ParentName, string _CanLogin, string _SentFlag, string _StudentId, int SiblingId,string EmailID,int AciveSecureAuth)
    {
        bool _valid = false;
        try
        {
             General myGenobj = new General(m_MysqlDb);
             string sql="";
             int MaxId = myGenobj.GetTableMaxId("tblparent_parentdetails", "Id");
             string Parentid = "0";
             if (MobNoNotexists(_UserName, out Parentid))
             {
                 sql = "insert into tblparent_parentdetails(Id,Name,UserName,Password,CanLogin,SentCredentials,SiblingId,GmailAuthId,IsActiveSecure) values(" + MaxId + ",'" + _ParentName + "','" + _UserName + "','" + _PassWord + "' , " + _CanLogin + " , " + _SentFlag + "," + SiblingId + ",'" + EmailID + "'," + AciveSecureAuth + ")";
                 m_MysqlDb.ExecuteQuery(sql);
                 sql = "insert into tblparent_parentstudentmap(ParentId,StudentId) values (" + MaxId + "," + _StudentId + ")";
                 m_MysqlDb.ExecuteQuery(sql);
                 _valid = true;
             }
             else
             {
                 if (!ParentStudentMapExists(Parentid, _StudentId))
                 {
                     sql = "insert into tblparent_parentstudentmap(ParentId,StudentId) values (" + Parentid + "," + _StudentId + ")";
                     m_MysqlDb.ExecuteQuery(sql);
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

    private bool ParentStudentMapExists(string _Parentid, string _StudentId)
    {
        bool _valid = false;
        string sql = "select ParentId from tblparent_parentstudentmap where ParentId=" + _Parentid + " and StudentId="+_StudentId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _valid = true;
        }
        return _valid;
    }

    private bool MobNoNotexists(string _UserName, out string Parentid)
    {
         bool _valid = true;
         Parentid = "0";
         string sql = "select Id from tblparent_parentdetails where UserName='" + _UserName + "'";
         m_MyReader = m_MysqlDb.ExecuteQuery(sql);
         if (m_MyReader.HasRows)
         {
             Parentid = m_MyReader.GetValue(0).ToString();
             _valid = false;
         }
         return _valid;
    }

    public bool UpdateParentCredentials(string _PassWord, string _UserName, string _ParentName, string _CanLogin, string _SentFlag, string _ParentId, string EmailId, int ActiveSecureAuth)
    {
        bool _valid = false;
        try
        {

            string sql = "update tblparent_parentdetails set UserName = '" + _UserName + "' ,Password = '" + _PassWord + "', CanLogin =  " + _CanLogin + ",SentCredentials=" + _SentFlag + ", GmailAuthId=" + EmailId + " , IsActiveSecure=" + ActiveSecureAuth + " where Id=" + _ParentId;
            m_MysqlDb.ExecuteQuery(sql);
            _valid = true;
        }
        catch
        {
            _valid = false;
        }
        return _valid;
    }

    public void UpdateParentLoginStatus(string _ParentId, string _CanLogin)
    {
        string sql = "update tblparent_parentdetails set CanLogin=" + _CanLogin + " where Id=" + _ParentId;
        m_MysqlDb.ExecuteQuery(sql);
    }

    public string ReadPassword(string _ParentId)
    {
        KnowinEncryption MyEncription = new KnowinEncryption();
        string Passord = "";
        string sql = "select tblparent_parentdetails.Password from tblparent_parentdetails where tblparent_parentdetails.Id="+_ParentId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Passord = MyEncription.Decrypt(m_MyReader.GetValue(0).ToString());
        }
        MyEncription = null;
        return Passord;
    }


    public bool GetParentData(out string ParentName, out string parentId, out string _UserName,out string  EmailId, string _StudentId,int Sibling)
    {
        ParentName = "";
        _UserName = "";
        parentId = "";
        EmailId="";
        bool _Valid = false;
        string subsql = "";
        if (Sibling != 0)
        {
            subsql = " OR SiblingId=" + Sibling + "";
        }
        string sql = "select tblparent_parentstudentmap.ParentId,tblparent_parentdetails.Name,tblparent_parentdetails.UserName,tblparent_parentdetails.GmailAuthId from tblparent_parentstudentmap INNER JOIN tblparent_parentdetails ON tblparent_parentdetails.Id= tblparent_parentstudentmap.ParentId where  tblparent_parentstudentmap.StudentId=" + _StudentId + " " + subsql;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            parentId = m_MyReader.GetValue(0).ToString();
            ParentName = m_MyReader.GetValue(1).ToString();
            _UserName = m_MyReader.GetValue(2).ToString();
            EmailId = m_MyReader.GetValue(3).ToString();
            _Valid = true;
        }

        return _Valid;
    }

    public string GetPassword(string _ParentName, string _MobNo, string _StudentId)
    {
        KnowinEncryption MyEncription = new KnowinEncryption();
        string Password = "";
        string sql = "select Id,Password from tblparent_parentdetails where  UserName='" + _MobNo + "'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Password = m_MyReader.GetValue(1).ToString();
            if (Password != "")
                Password = MyEncription.Decrypt(Password);
           
        }
        else
        {
            Password = GenerateParentLoginPassword();
            
        }
        return Password;
    }

    public bool ParentStudentExists(string _ParentId, string _StudentId)
    {
        bool Valid = false;
        string sql = "select ParentId from tblparent_parentstudentmap where ParentId=" + _ParentId + " and StudentId=" + _StudentId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Valid = true;
        }
        return Valid;
    }

    public void UpdateSMSStatus(string _ParentId)
    {
        string sql = "update tblparent_parentdetails set SentCredentials=1 where Id=" + _ParentId;
        m_MysqlDb.ExecuteQuery(sql);
    }

    public System.Data.DataSet GetAllAPrentData()
    {
        string sql = "select tblstudent.GardianName , tblstudent.OfficePhNo,tblstudent.Id ,Email from tblstudent where tblstudent.Status=1 and OfficePhNo <>'' and OfficePhNo<>0";
        return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
    }

    public System.Data.DataSet GetSelectedParents()
    {
        string sql = "select Id,Name,UserName,Password from tblparent_parentdetails";
        return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
    }

    public System.Data.DataSet GetParentStudentMap()
    {
        string sql = "select ParentId,StudentId from tblparent_parentstudentmap";
        return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
    }

    public void AddStudentToParent(string _ParentId, string _StudentId)
    {
        try
        {
            string sql = "insert into tblparent_parentstudentmap(ParentId,StudentId) values(" + _ParentId + "," + _StudentId + ")";
            m_MysqlDb.ExecuteQuery(sql);
        }
        catch
        { 
        }
    }


    public void UpdateParentLoginStatus(string _Status)
    {
        string sql = "update tblparent_config set Value=" + _Status + " where Id=2";
        m_MysqlDb.ExecuteQuery(sql);
    }

    # endregion




    public bool SMS_Enabled(string Type, out string SmsMessage, out string ScheduleTime)
    {
        bool valid = false;
        SmsMessage = "";
        ScheduleTime = "0";
        string Enable = "";
        string sql = "SELECT `Enable`,`Format`,ScheduledTime FROM tblsmsoptionconfig WHERE tblsmsoptionconfig.SetVisible=1 AND tblsmsoptionconfig.ShortName='" + Type + "' ";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Enable = m_MyReader.GetValue(0).ToString();
            if (Enable == "1")
            {
                valid = true;
                SmsMessage = m_MyReader.GetValue(1).ToString();
                ScheduleTime = m_MyReader.GetValue(2).ToString();
            }

        }
        return valid;
    }


    public void GetParentDetails(int StudentId, out string StudentName, out string ParentName, out string PhoneNumber, out bool ParentEnabled)
    {
        StudentName = "";
        ParentName = "";
        PhoneNumber = "";
        ParentEnabled = false;
        int _status = 0;

        string sql = "SELECT StudentName,GardianName FROM tblview_student WHERE Id=" + StudentId;
        OdbcDataReader mReader = m_MysqlDb.ExecuteQuery(sql);
        if (mReader.HasRows)
        {
            StudentName = mReader.GetValue(0).ToString();
            ParentName = mReader.GetValue(1).ToString();
        }

        sql = "SELECT PhoneNo,Enabled FROM tblsmsparentlist WHERE Id=" + StudentId;
        OdbcDataReader mReader1 = m_MysqlDb.ExecuteQuery(sql);
        if (mReader1.HasRows)
        {
            PhoneNumber = mReader1.GetValue(0).ToString();
            int.TryParse(mReader1.GetValue(1).ToString(), out _status);
            if (_status == 1)
            {
                ParentEnabled = true;
            }
        }

    }



    public void AddtoAutoSMS(string PhoneNumber, string SmsMessage, string ScheduleTime)
    {
        string sql = "INSERT INTO tblautosms (PhoneNumber,Message,TimeToSend,Status) VALUES('" + PhoneNumber + "','" + SmsMessage + "','" + ScheduleTime + "',0)";
        m_MysqlDb.ExecuteQuery(sql);
    }

    public string GetSMSTemplate(string Id)
    {
        string Template = "";
        string sql = "SELECT tblsmstemplate.TemplateValue FROM tblsmstemplate WHERE Id="+Id;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            Template = m_MyReader.GetValue(0).ToString();

        }
        return Template;
    }

    public string GetGeneralReplaceSMSMessage(string _Template,string UserId, string UserType)
    {
        string ReturnMessage = _Template;
        if (UserType == "0") //staff
        {
            string _Staffname="";
            GetstaffDetails(UserId, out _Staffname);
            ReturnMessage = _Template.Replace("($StaffName$)", _Staffname);
        }
        else if (UserType == "1")// parent
        {
            string _Studentname = "", ParentName = "", ClassName = "";
            GetStudentDetails(UserId, out _Studentname, out ParentName, out ClassName);
            ReturnMessage = _Template.Replace("($Student$)", _Studentname);
            ReturnMessage = _Template.Replace("($Parent$)", ParentName);
            ReturnMessage = _Template.Replace("($ClassName$)", ClassName);
        }
        else if (UserType == "2")// Student
        {
            string _Studentname = "", ParentName = "", ClassName = "";
            GetStudentDetails(UserId, out _Studentname, out ParentName, out ClassName);
            ReturnMessage = _Template.Replace("($Student$)", _Studentname);
            ReturnMessage = _Template.Replace("($Parent$)", ParentName);
            ReturnMessage = _Template.Replace("($ClassName$)", ClassName);
        }
        return ReturnMessage;
    }

    private void GetStudentDetails(string UserId, out string _Studentname, out string ParentName, out string ClassName)
    {
        _Studentname = "";
        ParentName = "";
        ClassName = "";
        string sql = "SELECT tblstudent.StudentName,tblstudent.GardianName,tblclass.ClassName FROM tblstudent INNER JOIN tblclass ON tblclass.Id=tblstudent.LastClassId WHERE tblstudent.Id=" + UserId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            _Studentname = m_MyReader.GetValue(0).ToString();
            ParentName = m_MyReader.GetValue(1).ToString();
            ClassName = m_MyReader.GetValue(2).ToString();
        }
    }

    private void GetstaffDetails(string UserId, out string _Staffname)
    {
        _Staffname = "";
        string sql = "SELECT SurName FROM tbluser WHERE Id=" + UserId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            _Staffname = m_MyReader.GetValue(0).ToString();

        }
    }

    public void InsertParentMobileNumberIntoSMSParenstsList(int _StudentId, string _MobileNumber)
    {
        string sql = "delete from tblsmsparentlist where Id=" + _StudentId;
        m_MysqlDb.ExecuteQuery(sql);


        sql = "insert into tblsmsparentlist(Id,PhoneNo,Enabled) values (" + _StudentId + ",'" + _MobileNumber + "'," + "1)";

        m_MysqlDb.ExecuteQuery(sql);

    }

    public DataSet GetStudentExceptionList(int configid)
    {
        string sql = "select tblstudent.StudentName, tblclass.ClassName, tblstudent.Id, tblsmsexceptionlist.PhoneNumber,tblsmsexceptionlist.Id as excId  from tblstudent inner join tblclass on tblclass.Id=  tblstudent.LastClassId inner join tblsmsexceptionlist on tblsmsexceptionlist.ParentId=  tblstudent.Id where tblsmsexceptionlist.ConfigId=" + configid + "";
        return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
    }

    public void InsertInToExceptionList(string phonenumber, int parentid, int configid)
    {
        string sql = "Insert into tblsmsexceptionlist(ParentId,PhoneNumber,ConfigId) values(" + parentid + ",'" + phonenumber + "'," + configid + ")";
        m_MysqlDb.ExecuteQuery(sql);
    }

    public void RemoveStudentfromExceptionList(string Id)
    {
        string sql = "";
        sql = "Delete from tblsmsexceptionlist where Id=" + Id + "";
        m_MysqlDb.ExecuteQuery(sql);
    }

    public void UpdateSMSLogtotable(string phonelist, char symbol, string message, int smstype)
    {
        if (!String.IsNullOrEmpty(phonelist))
        {
            string[] phonearray=phonelist.Split(symbol);
            DateTime dt= System.DateTime.Now.Date;
            if (phonearray.Length > 0)
            {
                string sql = "insert into tblautosms (PhoneNumber,Message,TimeToSend,Status,UserType,SentDate)values";
                string sql1 = "";

                for (int i = 0; i < phonearray.Length; i++)
                {
                    if (!String.IsNullOrEmpty(sql1)) sql1 = sql1 + ",";

                    sql1 = sql1 + "('" + phonearray[i] + "','" + message + "',0, 1," + smstype + ",'" + dt.ToString("s") + "')";
                }
                sql = sql + sql1;

            }
            else
            {
                string sql = "insert into tblautosms (PhoneNumber,Message,TimeToSend,Status,UserType,SentDate)values ('" + phonelist + "','" + message + "',0, 1," + smstype + ",'" + dt.ToString("s") + "')";               
                m_MysqlDb.ExecuteQuery(sql);
            }

        }
        
    }

    public string Get_SelectedParentPhoneNo_ListwithNativeLanguage(string _StudentId)
    {
        string phno = "", seperator = "", tempSQl = "";
        char symbol = mSymbol;
        if (_StudentId != "")
            tempSQl = " AND tblstudentclassmap.StudentId in(" + _StudentId + ")";
        string sql = "SELECT DISTINCT tblsmsparentlist.PhoneNo,tblsmsparentlist.SecondaryNo FROM tblsmsparentlist INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tblsmsparentlist.Id  WHERE tblsmsparentlist.Enabled=1 and IsActiveNativeLanguage=1" + tempSQl;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                phno = phno + seperator + m_MyReader.GetValue(0).ToString();
                seperator = symbol.ToString();
                if(m_MyReader.GetValue(1).ToString()!="")
                    phno = phno + seperator + m_MyReader.GetValue(1).ToString();                
            }
        }
        return phno;
    }

    public string Get_ParentPhoneNo_ListwithNativeLanguage(string temp_classIds)
    {
        string phno = "", tempSQl = "", seperator = "";
        if (temp_classIds !="")
        {
            tempSQl = " AND tblstudentclassmap.ClassId in (" + temp_classIds + ")";
        }
        string sql = "SELECT DISTINCT tblsmsparentlist.PhoneNo,tblsmsparentlist.SecondaryNo FROM tblsmsparentlist INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tblsmsparentlist.Id  WHERE tblsmsparentlist.Enabled=1 and IsActiveNativeLanguage=1" + tempSQl;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                phno = phno + seperator + m_MyReader.GetValue(0).ToString();
                seperator = ",";
                if (m_MyReader.GetValue(1).ToString() != "")
                    phno = phno + seperator + m_MyReader.GetValue(1).ToString();
            }
        }
        return phno;
    }

    public string Get_SelectedParentPhoneNo_ListwioutNativelanguage(string _StudentId)
    {

        string phno = "", seperator = "", tempSQl = "";
        char symbol = mSymbol;
        if (_StudentId != "")
            tempSQl = " AND tblstudentclassmap.StudentId in(" + _StudentId + ")";
        string sql = "SELECT DISTINCT tblsmsparentlist.PhoneNo,tblsmsparentlist.SecondaryNo FROM tblsmsparentlist INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tblsmsparentlist.Id  WHERE tblsmsparentlist.Enabled=1 and IsActiveNativeLanguage=0" + tempSQl;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                phno = phno + seperator + m_MyReader.GetValue(0).ToString();
                seperator = symbol.ToString();
                if (m_MyReader.GetValue(1).ToString() != "")
                    phno = phno + seperator + m_MyReader.GetValue(1).ToString();
            }
        }
        return phno;
    }


    public string GetNativeLanguage()
    {
        string language = "";
        string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Module='SMS' and  Name='Native Language'";

        DataSet dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
        {

            if (!String.IsNullOrEmpty(dt.Tables[0].Rows[0]["Value"].ToString()))
            {
                language = dt.Tables[0].Rows[0]["Value"].ToString();
            }
        }

        return language;
    }

    public bool HaveNetConnection()
    {
        HttpWebRequest objReq;
        HttpWebResponse objRes;
        try
        {
            objReq = (HttpWebRequest)HttpWebRequest.Create("http://www.google.com");
            objRes = (HttpWebResponse)objReq.GetResponse();
            if (objRes.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }


        }
        catch (WebException ex)
        {
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
  
}