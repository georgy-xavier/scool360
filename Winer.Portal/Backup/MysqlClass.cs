using System;
using System.Web;
//using System.Web.Services;
//using System.Web.Services.Protocols;
using System.Data.Odbc;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
public class MysqlClass
{
    private KnowinGen m_PntObj = null;
    private OdbcConnection _myOdbcConn=null;
    private OdbcDataReader _myReader = null;
    private OdbcCommand m_myCommand = null;
    private OdbcTransaction m_myTransaction = null;
    public string ConnectionStr = null;

    //private string m_stConnection = ConfigurationSettings.AppSettings["ConnectionInfo"];
    private MysqlClass()
    {

    }
    public MysqlClass(string m_stConnection)
    {
        ConnectionStr = m_stConnection;
        _myOdbcConn = new OdbcConnection(m_stConnection);
        _myOdbcConn.Open();

    }
    public void CloseConnection()
    {


        _myOdbcConn.Close();
        _myOdbcConn = null;

    }
    public MysqlClass(KnowinGen _PntObj)
    {
        if (m_PntObj == null)
        {
            m_PntObj = _PntObj;
        }
        _myOdbcConn = m_PntObj.ODBCconnection;
    }
    public MysqlClass(OdbcConnection _ODBCconnection)
    {
        if (_ODBCconnection != null)
        {
            _myOdbcConn = _ODBCconnection;
        }
        
    }
    ~MysqlClass()
    {
        if (_myReader != null)
        {
            _myReader = null;
        }
        /*if (m_PntObj != null)
        {
            m_PntObj = null;
        }*/
    }

    public OdbcDataReader ExecuteQuery(string m_Str_sql)
    {
        if (m_myTransaction == null)
        {
            OdbcCommand m_cmd = new OdbcCommand(m_Str_sql, _myOdbcConn);
             _myReader = m_cmd.ExecuteReader();
        }
        else
        {
            OdbcCommand m_cmd = new OdbcCommand(m_Str_sql, _myOdbcConn, m_myTransaction);
            _myReader = m_cmd.ExecuteReader();
        }

        return _myReader;
    }

    public void TransExecuteQuery(string m_Str_sql)
    {

        m_myCommand.CommandText = m_Str_sql;
        m_myCommand.ExecuteNonQuery();

    }
    public OdbcDataReader SelectTansExecuteQuery(string m_Str_sql)
    {

        m_myCommand.CommandText = m_Str_sql;
        _myReader = m_myCommand.ExecuteReader();
        return _myReader;
    }
    public void TransactionCommit()
    {
        m_myTransaction.Commit();
        m_myTransaction = null;

    }
    public void TransactionRollback()
    {
        m_myTransaction.Rollback();
        m_myTransaction = null;

    }
    public void MyBeginTransaction()
    {
        m_myCommand = _myOdbcConn.CreateCommand();
        m_myTransaction = _myOdbcConn.BeginTransaction();
        m_myCommand.Connection = _myOdbcConn;
        m_myCommand.Transaction = m_myTransaction;

    }
    public DataTable ExecuteQueryReturnDataTable(string m_Str_sql, List<OdbcParameter> lstOdbcParameter)
    {
        DataTable _myDt = new DataTable();
        OdbcDataAdapter _myAdapter = new OdbcDataAdapter();
        OdbcCommand m_cmd;
        if (m_myTransaction == null)
        {
            m_cmd = new OdbcCommand(m_Str_sql, _myOdbcConn);
            
        }
        else
        {
            m_cmd = new OdbcCommand(m_Str_sql, _myOdbcConn, m_myTransaction);

        }
        foreach (OdbcParameter para in lstOdbcParameter)
            m_cmd.Parameters.Add(para);

        _myAdapter.SelectCommand = m_cmd;
        _myAdapter.Fill(_myDt);
        return _myDt;
    }



    public DataSet ExecuteQueryReturnDataSet(string m_Str_sql)
    {
        DataSet _myDataset = new DataSet();
        OdbcDataAdapter _myAdapter = new OdbcDataAdapter();
        OdbcCommand m_cmd ;
        if (m_myTransaction == null)
        {
            m_cmd = new OdbcCommand(m_Str_sql, _myOdbcConn);          
           
        }
        else
        {
            m_cmd = new OdbcCommand(m_Str_sql, _myOdbcConn, m_myTransaction);
            
        }
        _myAdapter.SelectCommand = m_cmd;
        _myAdapter.Fill(_myDataset);
        return _myDataset;
    }

    public DataSet ExecuteQuery(string m_Str_sql, string str_DataSetName)
    {
        DataSet _myDataset = new DataSet();
        OdbcDataAdapter _myAdapter = new OdbcDataAdapter();
        OdbcCommand m_cmd = new OdbcCommand(m_Str_sql, _myOdbcConn);
        _myAdapter.SelectCommand = m_cmd;
        _myAdapter.Fill(_myDataset, str_DataSetName);
        return _myDataset;
    }

    public OdbcConnection returnOdbcConnection()
    {
        return _myOdbcConn;
    }

   


}
