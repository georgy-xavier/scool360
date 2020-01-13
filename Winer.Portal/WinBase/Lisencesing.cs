using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.IO;
using System.Data.Odbc;
using System.Configuration;

namespace WinBase
{
    public struct LicenceDetails
    {
        public string m_LisenceKey;
        public bool m_IsLisenced;
        public string m_Msg;
        public string m_SoftWare;
        public string m_Version;
        public int m_usercount;
        public DateTime m_Expiredate_dt;
        public int m_Awailabledays;
    }
     public class Lisencesing
    {
         private MysqlClass m_MysqlDb = null;


         private bool m_IsLisenced;
         private string m_ServerPath = "";
         private string m_MasterSoftWare = "Winer";
         private string m_MasterVersion = "V3";
         private string m_Masterusercount = "5";
         private int m_MastertrilePeriod_days = 30;
         private string[] m_OrginalMac;
         private string m_LisenceMac = "";
         private string m_SoftWare = "";
         private string m_Version = "";
         private string m_usercount = "";
         private string m_Expiredate_str = "";
         private DateTime m_Expiredate_dt ;
         private string m_Installdate_str = "";
         private string m_Awailabledays = "";
         private OdbcDataReader m_MyReader = null;
         private string m_ConnectionString = "";
         bool m_invalidFile = false;
         private string m_Msg = "";
         KnowinEncryption _MyEncrption = null;
         public Lisencesing(string _ServerPath, string strConnectionString)
          {
              
              m_ServerPath=_ServerPath;
              m_ConnectionString = strConnectionString;
              _MyEncrption = new KnowinEncryption();
              //InItDb();
              LoadLisence();
          }

          public bool IsLisenced
          {
              get
              {
                  return m_IsLisenced;
              }

          }
          public string ErrorMessage
          {
              get
              {
                  return m_Msg;
              }

          }
          private void LoadLisence()
          {
              string _isfirstRun;
              m_OrginalMac = GetMACAddress();
              GetDbLisencedetails(out _isfirstRun, out m_Installdate_str);
              if (_isfirstRun == "true")
              {
                  initialLicence();
              }
              else
              {
                  GetDetails();
              }
              AssignLisenceFlag();
          }

          private void AssignLisenceFlag()
          {
              if(m_invalidFile)
              {
                  m_Msg="Invalid License";
                  m_IsLisenced = false;
              }
              else if (!IsAcceptedMac(m_OrginalMac,m_LisenceMac))
              {
                  m_Msg = "Invalid License for the system";
                  m_IsLisenced = false;
              }
              else if (m_SoftWare != "All" && m_SoftWare!=m_MasterSoftWare)
              {
                  m_Msg = "Invalid License for the Software";
                  m_IsLisenced = false;
              }
              else if (m_Version != "All" && m_Version != m_MasterVersion)
              {
                  m_Msg = "Invalid License for the Version";
                  m_IsLisenced = false;
              }
              else if (m_Expiredate_dt < DateTime.Now && m_Awailabledays != "Unlimited")
              {
                  m_Msg = "License Expired";
                  m_IsLisenced = false;
              }
              else
              {
                  m_IsLisenced = true;
                  m_Msg = "Success";
              }
              
          }

          private bool IsAcceptedMac(string[] m_OrginalMac, string m_LisenceMac)
          {
              bool _valid = false;
              foreach (string macAddress in m_OrginalMac)
              {
                  if (macAddress == m_LisenceMac)
                  {
                      _valid = true;
                  }
                  
              }
              return _valid;
          }

          private void GetDetails()
          {
              General _GenObj = new General(m_MysqlDb);
              string _licencetext="";
              try
              {
                 
                 StreamReader fp = null;
                 fp = File.OpenText(m_ServerPath + "info");
                 _licencetext=fp.ReadLine();
                 fp.Close();
                 _licencetext = _MyEncrption.Decrypt(_licencetext);
                 _licencetext = _MyEncrption.Decrypt(_licencetext);
                 string[] _temp = _licencetext.Split('$');

                 m_LisenceMac = _temp[0];
                 m_Expiredate_str = _temp[1];
                 m_usercount = _temp[2];
                 m_SoftWare = _temp[3];
                 m_Version = _temp[4];
                 if (m_Expiredate_str == "None")
                 {
                     m_Awailabledays = "Unlimited";
                 }
                 else
                 {
                     //m_Expiredate_dt= DateTime.Parse(m_Expiredate_str);
                     //m_Expiredate_dt = DateTime.ParseExact(m_Expiredate_str, "M/d/yyyy h:mm:ss tt", null);
                     //m_Expiredate_dt = _GenObj.GetDareFromText(m_Expiredate_str);
                     string[] _DateParts = m_Expiredate_str.Split('/');
                     m_Expiredate_dt = new DateTime(int.Parse(_DateParts[2]), int.Parse(_DateParts[1]), int.Parse(_DateParts[0]), int.Parse(_DateParts[3]), int.Parse(_DateParts[4]), int.Parse(_DateParts[5]));
                     m_Expiredate_dt = m_Expiredate_dt.Date;
                     TimeSpan _ts= m_Expiredate_dt.Subtract(DateTime.Now.Date);
                     int _days=(int)_ts.TotalDays;
                     if (_days < 0)
                     {
                         m_Awailabledays = "None";
                     }
                     else
                     {
                         m_Awailabledays = _days.ToString();
                     }
                     
                     m_Expiredate_str = m_Expiredate_dt.Date.ToString("dd-MM-yyyy");
                 }
                 
             }
             catch
             {
                 m_LisenceMac = "Unknown";
                 m_Expiredate_str = "Unknown";
                 m_usercount = "Unknown";
                 m_SoftWare = m_MasterSoftWare;
                 m_Version = m_MasterSoftWare;
                 m_invalidFile = true;
             }

             
          }


          private void GetDbLisencedetails(out string _isfirstRun, out string m_Installdate_str)
          {
              _isfirstRun = "None";
              m_Installdate_str = "Unknown";
              OdbcConnection _MyODBCConn = new OdbcConnection(m_ConnectionString);
              _MyODBCConn.Open();
              MysqlClass _MysqlDb = new MysqlClass(_MyODBCConn);
              string sql = "select Name,Value from tblconfiguration WHERE Id =9";
              m_MyReader = _MysqlDb.ExecuteQuery(sql);
              if (m_MyReader.HasRows)
              {
                  m_Installdate_str = m_MyReader.GetValue(0).ToString();
                  _isfirstRun = m_MyReader.GetValue(1).ToString();

                  _isfirstRun = _MyEncrption.Decrypt(_isfirstRun);
                  m_Installdate_str = _MyEncrption.Decrypt(m_Installdate_str);
                  
              }
           
              m_MyReader.Close();
              _MyODBCConn.Close();
              
          }
          ~Lisencesing()
          {
             
              _MyEncrption =null;
          }
         public string GetSystemKey()
        {
           
            string _encript, _mcaddress;
            _mcaddress = m_OrginalMac[0];
            _encript = _MyEncrption.Encrypt(_mcaddress);
            return _encript;

        }
         public void initialLicence()
         {
             string keytext ,_licencetext;

             m_Expiredate_dt = DateTime.Now.AddDays(m_MastertrilePeriod_days);
             m_Expiredate_str = m_Expiredate_dt.Date.ToString();
             m_LisenceMac = m_OrginalMac[0];
             m_usercount = m_Masterusercount;
             m_SoftWare = m_MasterSoftWare;
             m_Version = m_MasterVersion;
             m_Awailabledays = m_MastertrilePeriod_days.ToString();
             keytext = m_LisenceMac + "$" + m_Expiredate_str + "$" + m_Masterusercount + "$" + m_MasterSoftWare + "$" + m_MasterVersion;
             _licencetext=_MyEncrption.Encrypt(keytext);
             keytext = _MyEncrption.Encrypt(_licencetext);
             CreateLisenceFile(keytext);
             UpdateDb();

         }

         private void UpdateDb()
         {
             OdbcConnection _MyODBCConn = new OdbcConnection(m_ConnectionString);
             _MyODBCConn.Open();
             MysqlClass _MysqlDb = new MysqlClass(_MyODBCConn);
             string _isfirstRun = "false";
             string m_installedDate = DateTime.Now.ToString();
             _isfirstRun = _MyEncrption.Encrypt(_isfirstRun);
             m_installedDate = _MyEncrption.Encrypt(m_installedDate);
             string sql = "UPDATE tblconfiguration SET Name= '" + m_installedDate + "',Value='" + _isfirstRun + "' WHERE Id =9";
             m_MyReader = _MysqlDb.ExecuteQuery(sql);

             m_MyReader.Close();
             _MyODBCConn.Close();
         }
         private bool CreateLisenceFile(string _licencetext)
         {
             bool _valid = false;
             try
             {
                 
                 StreamWriter fp = null;
                 fp = File.CreateText(m_ServerPath + "info");
                 fp.WriteLine(_licencetext);
                 fp.Close();
                 _valid = true;
             }
             catch
             {
                 _valid = false;
             }

             return _valid;
         }
        public string[] GetMACAddress()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            int PhysicalAddressCount = 0;
            string[] Tenpaddress;
            string[] MACAddress;
            Tenpaddress = new string[moc.Count];
            foreach (ManagementObject mo in moc)
            {
               
                //if (MACAddress == String.Empty) // only return MAC Address from first card
                //{
                   
                //}
                //if ((bool)mo["IPEnabled"] == true)

                if (mo["MacAddress"]!=null)
                {

                    Tenpaddress[PhysicalAddressCount] = mo["MacAddress"].ToString();
                    Tenpaddress[PhysicalAddressCount]=Tenpaddress[PhysicalAddressCount].Replace(":", "");
                    PhysicalAddressCount++;
                }
                mo.Dispose();
            }
            MACAddress = new string[PhysicalAddressCount];
            for (int i = 0; i < PhysicalAddressCount; i++)
                MACAddress[i] = Tenpaddress[i];
                //MACAddress = MACAddress.Replace(":", "");
                return MACAddress;
        }
        public void InItDb()
        {
            OdbcConnection _MyODBCConn = new OdbcConnection(m_ConnectionString);
            _MyODBCConn.Open();
             MysqlClass _MysqlDb  = new MysqlClass(_MyODBCConn);
             string _isfirstRun = "true";
             string m_installedDate = "None";
             _isfirstRun = _MyEncrption.Encrypt(_isfirstRun);
             m_installedDate = _MyEncrption.Encrypt(m_installedDate);
             string sql = "UPDATE tblconfiguration SET Name= '" + m_installedDate + "',Value='" + _isfirstRun + "' WHERE Id =9";
             m_MyReader = _MysqlDb.ExecuteQuery(sql);
             
             m_MyReader.Close();
             _MyODBCConn.Close();
            
        }


        public void GetLisenceDetails(out string _software, out string _Version, out string _installionDate, out string _usercount, out string _expiredate, out string _Dayesleft)
        {
            _software = m_SoftWare;
            _Version = m_Version;
            _installionDate = m_Installdate_str;
            _usercount = m_usercount;
            _expiredate = m_Expiredate_str;
            _Dayesleft = m_Awailabledays;
        }

        public void UpdateLicense(string _lisencekey, out string _message)
        {
            string _licencetext = _MyEncrption.Decrypt(_lisencekey);
            string[] _temp = _licencetext.Split('$');

            string _LisenceMac = _temp[0];
             string _SoftWare = _temp[3];
             string _Version = _temp[4];

            if (!IsAcceptedMac(m_OrginalMac,_LisenceMac))
            {
                _message = "Invalid License.";
                
            }
            else if (_SoftWare != "All" && _SoftWare!= m_MasterSoftWare)
            {
                _message = "Invalid License.";
            }
            else if (_Version != "All" && _Version != m_MasterVersion)
            {
                _message = "Invalid License.";
            }
            else
            {

                try
                {
                    _licencetext = _MyEncrption.Encrypt(_lisencekey);
                    if (CreateLisenceFile(_licencetext))
                    {
                        _message = "Your License is Updated";
                    }
                    else
                    {
                        _message = "Unable to update License";
                    }

                }
                catch
                {
                    _message = "Invalid License.";
                }
               
            }
        }
    }

     public class AddressLisencesing
     {
         private MysqlClass m_MysqlDb = null;

         private string m_ServerPath = "";
         private string m_MasterSoftWare = "Winer";
         private string m_MasterVersion = "V3";
         private string m_Masterusercount = "5";
         private int m_MastertrilePeriod_days = 30;
         private LicenceDetails M_LicenceObj;
         private string m_ConnectionString;
     
         private OdbcDataReader m_MyReader = null;
        
         
         KnowinEncryption _MyEncrption = null;

         public AddressLisencesing(string _ServerPath,string strConnectionString)
         {

             m_ServerPath = _ServerPath;
             m_ConnectionString = strConnectionString;
             _MyEncrption = new KnowinEncryption();
             //InItDb();
             if (LoadLisenceObj())
             {
                 AssignLisenceFlag();
             }
             
         }

         private bool LoadLisenceObj()
         {
             bool _Valid=false;
             M_LicenceObj = new LicenceDetails();
             string _licencetext = "";
             try
             {

                 StreamReader fp = null;
                 fp = File.OpenText(m_ServerPath + "addressinfo.dat");
                 _licencetext = fp.ReadLine();
                 fp.Close();
                 _licencetext = _MyEncrption.Decrypt(_licencetext);
                 _licencetext = _MyEncrption.Decrypt(_licencetext);
                 string[] _temp = _licencetext.Split('$');

                 M_LicenceObj.m_LisenceKey = _temp[0];
                 string m_Expiredate_str = _temp[1];
                 string m_usercount = _temp[2];
                 M_LicenceObj.m_SoftWare= _temp[3];
                 M_LicenceObj.m_Version = _temp[4];
                 
                 if (m_Expiredate_str == "None")
                 {
                     M_LicenceObj.m_Awailabledays=-1;
                 }
                 else
                 {
                     //m_Expiredate_dt= DateTime.Parse(m_Expiredate_str);
                     //m_Expiredate_dt = DateTime.ParseExact(m_Expiredate_str, "M/d/yyyy h:mm:ss tt", null);
                     //m_Expiredate_dt = _GenObj.GetDareFromText(m_Expiredate_str);
                     string[] _DateParts = m_Expiredate_str.Split('/');
                     DateTime m_Expiredate_dt = new DateTime(int.Parse(_DateParts[2]), int.Parse(_DateParts[1]), int.Parse(_DateParts[0]), int.Parse(_DateParts[3]), int.Parse(_DateParts[4]), int.Parse(_DateParts[5]));

                     TimeSpan _ts = m_Expiredate_dt.Date.Subtract(DateTime.Now.Date);//mani updation
                     int _days = (int)_ts.TotalDays;
                     if (_days < 0)
                     {
                         M_LicenceObj.m_Awailabledays = 0;
                         //m_Awailabledays = "None";
                     }
                     else
                     {
                         M_LicenceObj.m_Awailabledays  = _days;
                     }
                     M_LicenceObj.m_Expiredate_dt = m_Expiredate_dt;
                    
                 }
                 if (m_usercount == "Unlimited")
                 {
                     M_LicenceObj.m_usercount = -1;

                 }
                 else
                 {
                     M_LicenceObj.m_usercount = int.Parse(m_usercount);
                 }
                 _Valid = true;
             }
             catch
             {
                 M_LicenceObj.m_LisenceKey = "Unknown";
                 M_LicenceObj.m_Expiredate_dt = DateTime.Now.AddDays(-1);
                 M_LicenceObj.m_usercount = 0;
                 M_LicenceObj.m_SoftWare = "Unknown";
                 M_LicenceObj.m_Version = "Unknown";
                 _Valid = false;
                 M_LicenceObj.m_IsLisenced = false;
                 M_LicenceObj.m_Msg = "Invalid License";
             }

             return _Valid;
             
         }

         private void AssignLisenceFlag()
         {
             if (!IsAcceptedKey(M_LicenceObj.m_LisenceKey))
             {
                 M_LicenceObj.m_Msg = "Invalid License for the School";
                 M_LicenceObj.m_IsLisenced = false;
             }
             else if (M_LicenceObj.m_SoftWare != "All" && M_LicenceObj.m_SoftWare != m_MasterSoftWare)
             {
                 M_LicenceObj.m_Msg = "Invalid License for the Software";
                 M_LicenceObj.m_IsLisenced = false;
             }
             else if (M_LicenceObj.m_Version != "All" && M_LicenceObj.m_Version != m_MasterVersion)
             {
                 M_LicenceObj.m_Msg = "Invalid License for the Version";
                 M_LicenceObj.m_IsLisenced = false;
             }
             else if (M_LicenceObj.m_Expiredate_dt < DateTime.Now && M_LicenceObj.m_Awailabledays != -1)
             {
                 M_LicenceObj.m_Msg = "License Expired";
                 M_LicenceObj.m_IsLisenced = false;
             }
             else
             {
                 M_LicenceObj.m_IsLisenced = true;
                 M_LicenceObj.m_Msg = "Success";
             }

         }

         private bool IsAcceptedKey(string m_LisenceKey)
         {
             if (m_LisenceKey.Trim() == GetSystemKey().Trim())
             {
                 return true;
             }
             else 
             {
                 return false;
             }
         
         }

         public bool IsLisenced
         {
             get
             {
                 return M_LicenceObj.m_IsLisenced;
             }

         }
         public LicenceDetails LicencseObj
         {
             get
             {
                 return M_LicenceObj;
             }

         }
         public string ErrorMessage
         {
             get
             {
                 return M_LicenceObj.m_Msg;
             }

         }


         public void GetLisenceDetails(out string _software, out string _Version, out string _usercount, out string _expiredate, out string _Dayesleft)
         {
             _software =M_LicenceObj.m_SoftWare;
             _Version =M_LicenceObj. m_Version;
             if (M_LicenceObj.m_usercount == -1)
             {
                 _usercount = "Unlimited";
             }
             else
             {
                 _usercount = M_LicenceObj.m_usercount.ToString();
             }
             if (M_LicenceObj.m_Expiredate_dt.Date == new DateTime(1, 1, 1).Date)
             {
                 _expiredate = "NONE";
             }
             else
             {
                 _expiredate = M_LicenceObj.m_Expiredate_dt.ToString("dd-MM-yyyy"); 
             }
             if (M_LicenceObj.m_Expiredate_dt.Date == new DateTime(1, 1, 1).Date)
             {
                 _Dayesleft = "Unlimited";
             }
             else if (M_LicenceObj.m_Awailabledays < 1)
             {
                 _Dayesleft = "Expired";
             }
             else
             {
                 _Dayesleft = M_LicenceObj.m_Awailabledays.ToString();
                 
             }
            
         }

         

         public bool RegisterLicense(string _FileName, ref string _msg)
         {
             bool _Valid = false;
           
             string _licencetext = "";
             string C_licencetext = "";
             try
             {

                 StreamReader fp = null;
                 fp = File.OpenText(m_ServerPath + _FileName);
                 C_licencetext = fp.ReadLine();
                 fp.Close();

                 File.Delete(m_ServerPath + _FileName);
                 _licencetext = _MyEncrption.Decrypt(C_licencetext);
                 string[] _temp = _licencetext.Split('$');

                 string _LisenceKey = _temp[0];
                 string _SoftWare = _temp[3];
                 string _Version = _temp[4];

                 

                 if (!IsAcceptedKey(_LisenceKey))
                 {
                     _msg = "Invalid License.";

                 }
                 else if (_SoftWare != "All" && _SoftWare != m_MasterSoftWare)
                 {
                     _msg = "Invalid License.";
                 }
                 else if (_Version != "All" && _Version != m_MasterVersion)
                 {
                     _msg = "Invalid License.";
                 }
                 else
                 {

                     try
                     {
                         _licencetext = _MyEncrption.Encrypt(C_licencetext);
                         if (CreateLisenceFile(_licencetext))
                         {
                             //my function
                             string _Expirydate = _temp[1];
                             string[] _SplitEx_Date = _Expirydate.Split('/');
                             string sql="";
                             OdbcConnection _MyODBCConn = new OdbcConnection(m_ConnectionString);
                             _MyODBCConn.Open();
                             MysqlClass _MysqlDb = new MysqlClass(_MyODBCConn);
                             if (_SplitEx_Date.Count() > 1)
                             {
                                 int _day = int.Parse(_SplitEx_Date[0]), _month = int.Parse(_SplitEx_Date[1]), _year = int.Parse(_SplitEx_Date[2]), _HH = int.Parse(_SplitEx_Date[3]), _M = int.Parse(_SplitEx_Date[4]), _S = int.Parse(_SplitEx_Date[5]);
                                 DateTime LicanceDate = new DateTime(_year, _month, _day, _HH, _M, _S);
                                 _Expirydate = LicanceDate.ToString("yyyy-MM-dd HH:m:s");
                                 sql = "UPDATE tblschooldetails SET ExpireDate='" + _Expirydate + "'";
                             }
                             else
                             {
                                 DateTime LicanceDate = DateTime.MaxValue;
                                 _Expirydate = LicanceDate.ToString("yyyy-MM-dd HH:m:s");
                                 sql = "UPDATE tblschooldetails SET ExpireDate ='" + _Expirydate + "'";
                             }
                            
                             _MysqlDb.ExecuteQuery(sql);
                             _msg = "Your License is Updated";
                             _Valid = true;
                         }
                         else
                         {
                             _msg = "Unable to update License";
                         }

                     }
                     catch
                     {
                         _licencetext = "Unable to register License.";
                         _msg = _licencetext;
                     }


                 }
             }

             catch
             {

                 _licencetext = "Invalid License file.";
                 _msg = _licencetext;
             }
             
             return _Valid;
         }


     

         private bool CreateLisenceFile(string _licencetext)
         {
             bool _valid = false;
             try
             {

                 StreamWriter fp = null;
                 fp = File.CreateText(m_ServerPath + "addressinfo.dat");
                 fp.WriteLine(_licencetext);
                 fp.Close();
                 _valid = true;
             }
             catch
             {
                 _valid = false;
             }

             return _valid;
         }

         public bool GenerateSystemFile()
         {
             bool _valid = false;
             try
             {
                 string _SystemKey = GetSystemKey();
                 string C_SystemKey = _MyEncrption.Encrypt(_SystemKey);
                 StreamWriter fp = null;
                 fp = File.CreateText(m_ServerPath + "sysinfo.dat");
                 fp.WriteLine(C_SystemKey);
                 fp.Close();
                 _valid = true;
             }
             catch
             {
                 _valid = false;
             }
             return _valid;
         }

         private string GetSystemKey()
         {
             string _systemKey = "None";
             OdbcConnection _MyODBCConn = new OdbcConnection(m_ConnectionString);
             _MyODBCConn.Open();
             MysqlClass _MysqlDb = new MysqlClass(_MyODBCConn);
             string sql = "select tblschooldetails.SchoolName, tblschooldetails.Address from tblschooldetails";
             m_MyReader = _MysqlDb.ExecuteQuery(sql);
             if (m_MyReader.HasRows)
             {
                 _systemKey = m_MyReader.GetValue(0).ToString() + m_MyReader.GetValue(1).ToString();
               

             }

             m_MyReader.Close();
             _MyODBCConn.Close();
             return _systemKey;
         }
     }
}
