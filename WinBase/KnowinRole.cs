using System;
using System.Web;
//using System.Web.Services;
//using System.Web.Services.Protocols;
using System.Data.Odbc;
using System.Data;
public class KnowinRole:KnowinGen
{
    public MysqlClass m_MysqlDb;
    private OdbcDataReader m_MyReader = null;
    public int m_RoleId;
    public string m_RoleName;
    public string m_RoleDisc;

    public KnowinRole(KnowinGen _Prntobj)
    {
        m_Parent = _Prntobj;
        //m_MyReader = m_Parent.ODBCconnection   ;
        m_MyODBCConn = m_Parent.ODBCconnection;
        m_MysqlDb = new MysqlClass(this);
        m_RoleId = 0;
        m_RoleName = "";
        m_RoleDisc = "";

    }

    ~KnowinRole()
    {
        if (m_MysqlDb != null)
        {
            m_MysqlDb = null;

        } if (m_MyReader != null)
        {
            m_MyReader = null;

        }
    }


    public void CreateNewRole(string _RoleName, string _RoleDisc,string _RoleType)
    {
        string sql = "INSERT INTO tblrole(RoleName,Discription,Type) VALUES ('" + _RoleName + "', '" + _RoleDisc + "', '" + _RoleType + "')";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        m_MyReader.Close();   
    }

    public bool ValidateRoleName(string _RoleName)
    {
        bool _valide;
        string sql = "SELECT * FROM tblrole where RoleName='" + _RoleName + "'";
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



    public bool IsRoleAction(DataSet _dataSet, int _ActionId)
    {
        bool _valide;
        _valide = false;
        if (_dataSet != null && _dataSet.Tables != null && _dataSet.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr in _dataSet.Tables[0].Rows)
            {
                if (int.Parse(dr[0].ToString()) == _ActionId)
                {
                    _valide = true;

                }

            }

        }
        return _valide;
    }

    public bool HasUser(int _RoleId)
    {
         bool _valide;
        string sql = "SELECT Id FROM tbluser where RoleId=" + _RoleId;
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
        


    public void DeleteRole(int _RoleId)
    {
        string sql = "DELETE FROM tblrole WHERE Id=" + _RoleId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        sql = "DELETE FROM tblroleactionmap WHERE RoleId=" + _RoleId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        m_MyReader.Close();
    }
}
