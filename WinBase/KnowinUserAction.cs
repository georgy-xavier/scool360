using System;
using System.Web;
//using System.Web.Services;
//using System.Web.Services.Protocols;
using System.Data.Odbc;
using System.Data;
using System.Text.RegularExpressions;
public class KnowinUserAction:KnowinGen
{
    public MysqlClass m_MysqlDb;
    private OdbcDataReader m_MyReader = null;
    private KnowinEncryption m_MyEncrypt = null;
    public int m_UserId;
    public string m_Name;
    public int m_UserRoleId;
    private string m_EmailPattern;
    public KnowinUserAction(KnowinGen _Prntobj)
    {
        m_Parent = _Prntobj;
        m_MyODBCConn = m_Parent.ODBCconnection;
        m_UserName = m_Parent.LoginUserName;
        m_MysqlDb = new MysqlClass(this);
        m_UserId = 0;
        m_Name = "";
        m_UserRoleId = 0;
        m_EmailPattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
    }
    
    ~KnowinUserAction()
    {
        if (m_MysqlDb != null)
        {
            m_MysqlDb = null;

        } if (m_MyReader != null)
        {
            m_MyReader = null;

        }
        if (m_MyEncrypt != null)
        {
            m_MyEncrypt =null;
        }
    }

    public int CreateNewUser(string _Loginname, string _Password, string _email,string _SurName, int _UserRole, int _Loginright)
    {
        if (m_MyEncrypt == null)
        {
            m_MyEncrypt = new KnowinEncryption();
        }
        string _cipherText;
        
        _cipherText = m_MyEncrypt.Encrypt(_Password);
        string _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string sql = "INSERT INTO tbluser(UserName,Password,EmailId,SurName,CreationTime,RoleId,CanLogin) VALUES ('" + _Loginname + "', '" + _cipherText + "', '" + _email + "', '" + _SurName + "', '" + _strdtNow + "', " + _UserRole + ", " + _Loginright + ")";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        sql = "SELECT Id FROM tbluser where UserName='" + _Loginname + "'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            m_UserId =int.Parse( m_MyReader.GetValue(0).ToString());
        }
        m_MyReader.Close();
        return m_UserId;
    }

    public bool ValidadLoginName(string _LoginName)
    {
        bool _valide;
        string sql = "SELECT * FROM tbluser where UserName='" + _LoginName + "'";
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

    public void AddDetails(string _Address, string _dob, string _Phone)
    {
        string sql = "INSERT INTO tbluserdetails(UserId,Address,DOB,Phone) VALUES(" + m_UserId + ", '" + _Address + "', '" + _dob + "', '" + _Phone + "')";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        
    }

    public bool VaidateEmail(string _EmailId)
    {
        bool _valide;
        System.Text.RegularExpressions.Match match = Regex.Match(_EmailId.Trim(), m_EmailPattern, RegexOptions.IgnoreCase);
        if (match.Success)
        {

            _valide = true;
        }
        else
        {
            _valide = false;
        }
        //m_MyReader.Close();
        return _valide; 
    }

    public void UpdateUser(string _Password, string _email, string _SurName, int UserRole, int Loginright,int _Userid)
    {
        if (m_MyEncrypt == null)
        {
            m_MyEncrypt = new KnowinEncryption();
        }
        string _cipherText;
        _cipherText = m_MyEncrypt.Encrypt(_Password);
        string _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        String Sql = "UPDATE tbluser SET SurName='" + _SurName + "',Password='" + _cipherText + "',EmailId='" + _email + "',CreationTime='" + _strdtNow + "',RoleId='" + UserRole + "',CanLogin='" + Loginright + "' WHERE Id=" + _Userid;
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
    }

    public bool HasData(int _UserId)
    {
        bool Data = false;
        String Sql = "SELECT * FROM tbluserdetails WHERE UserId=" + _UserId;
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
        if (m_MyReader.HasRows)
        {
            Data = true;
        }
        return Data;
    }



    public void UpdateUser1(string _email, string _SurName, int _UserRole, int _Loginright, int _UserId)
    {
        string _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        String Sql = "UPDATE tbluser SET SurName='" + _SurName + "',EmailId='" + _email + "',CreationTime='" + _strdtNow + "',RoleId='" + _UserRole + "',CanLogin='" + _Loginright + "' WHERE Id=" + _UserId;
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
    }


    public bool HasImage(int _UserID,out string preimage)
    {
        preimage = "";
        bool ImageFlag = false;
        String Sql = "SELECT FilePath FROM tblfileurl WHERE Type='UserImage' AND UserId=" + _UserID;
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
        if (m_MyReader.HasRows)
        {
            preimage = m_MyReader.GetValue(0).ToString();
            ImageFlag = true;
        }
        return ImageFlag;
    }

    public bool TblUserdetailsHasData(int _UserId)
    {
        bool HasData = false;
        String Sql = "SELECT * FROM tbluserdetails WHERE UserId=" + _UserId;
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
        if (m_MyReader.HasRows)
        {

            HasData = true;
        }
        return HasData;
    }

    public bool NotaManagerOrAdmin(int _UserId)
    {
        
        String Sql;
        bool NtmanOradmin = true;
        if (_UserId == 1)
        {
            NtmanOradmin = false;
        }
        Sql = "select tblgroup.ManagerId from tblgroup where tblgroup.ManagerId="+ _UserId;
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
        if(m_MyReader.HasRows)
        {         
           NtmanOradmin = false;
        }
        return NtmanOradmin;
    }

    public bool UseHasImage(int _UserId, out string Imagename)
    {
        String Sql;
        Imagename = "";
        bool HasImage = false;
        Sql = "SELECT FilePath FROM tblfileurl WHERE Type='UserImage' AND UserId=" + _UserId;
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
        if (m_MyReader.HasRows)
        {
            HasImage = true;
            Imagename = m_MyReader.GetValue(0).ToString();
        }
        return HasImage;
    }
  
}
