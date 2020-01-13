using System;
using System.Web;
using System.Data.Odbc;
public class KnowinGen
{
    protected OdbcConnection m_MyODBCConn=null;
    protected KnowinGen m_Parent=null;
    protected string m_UserName = null;
    protected int m_userid;
    protected string m_ConnectionStr = "";
    protected string m_FilePath = "";

    ~KnowinGen()
    {
        if (m_MyODBCConn != null)
        {
           /* if (m_MyODBCConn.State == System.Data.ConnectionState.Open)
            {
                m_MyODBCConn.Close();
            }*/
            m_MyODBCConn = null;
        }

    }

    public OdbcConnection ODBCconnection
    {
        get
        {
            return m_MyODBCConn;
        }
        set
        {
            m_MyODBCConn = ODBCconnection;

        }
    }
    public string LoginUserName
    {
        get
        {
            return m_UserName;
        }
        
    }
    //public string ConnectionString
    //{
    //    get
    //    {
    //        return m_ConnectionStr;
    //    }

    //}
    public string FilePath
    {
        get
        {
            return m_FilePath;
        }

    }
    public int User_Id
    {
        get
        {
            return m_userid;
        }

    }
}
