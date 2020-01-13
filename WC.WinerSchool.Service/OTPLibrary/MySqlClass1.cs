using System;
using System.Web;
//using System.Web.Services;
//using System.Web.Services.Protocols;
using System.Data.Odbc;
using System.Data;
using System.Configuration;
public class MysqlClasslib
{
    private string m_checkconn = ConfigurationSettings.AppSettings["CheckConnection"];
    //private KnowinGen m_PntObj = null;
    private OdbcConnection _myOdbcConn = null;
    private OdbcDataReader _myReader = null;
    private OdbcCommand m_myCommand = null;
    private OdbcTransaction m_myTransaction = null;
    private string m_stConnection = ConfigurationSettings.AppSettings["ConnectionInfo"];
    public MysqlClasslib()
    {


        _myOdbcConn = new OdbcConnection(m_stConnection);
        _myOdbcConn.Open();

    }
    public void CloseConnection()
    {


        _myOdbcConn.Close();
        _myOdbcConn = null;

    }

    //public MysqlClass(KnowinGen _PntObj)
    //{
    //    if (m_PntObj == null)
    //    {
    //        m_PntObj = _PntObj;
    //    }
    //    _myOdbcConn = m_PntObj.ODBCconnection;
    //}
    public MysqlClasslib(OdbcConnection _ODBCconnection)
    {
        if (_ODBCconnection != null)
        {
            _myOdbcConn = _ODBCconnection;
        }
    }
    ~MysqlClasslib()
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
            CheckConnection();
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
        CheckConnection();
        m_myCommand = _myOdbcConn.CreateCommand();
        m_myTransaction = _myOdbcConn.BeginTransaction();
        m_myCommand.Connection = _myOdbcConn;
        m_myCommand.Transaction = m_myTransaction;

    }
    public DataSet ExecuteQueryReturnDataSet(string m_Str_sql)
    {
        DataSet _myDataset = new DataSet();
        OdbcDataAdapter _myAdapter = new OdbcDataAdapter();
        OdbcCommand m_cmd = new OdbcCommand(m_Str_sql, _myOdbcConn);

        if (m_myTransaction == null)
        {
            CheckConnection();
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
        CheckConnection();
        DataSet _myDataset = new DataSet();
        OdbcDataAdapter _myAdapter = new OdbcDataAdapter();
        OdbcCommand m_cmd = new OdbcCommand(m_Str_sql, _myOdbcConn);
        _myAdapter.SelectCommand = m_cmd;
        _myAdapter.Fill(_myDataset, str_DataSetName);
        return _myDataset;
    }
    private void CheckConnection()
    {
        if (m_checkconn == "1")
        {
            try
            {


                string _testSql = "select tblconfig.Id from tblconfig where tblconfig.Id=1";

                if (m_myTransaction == null)
                {
                    OdbcCommand m_cmd = new OdbcCommand(_testSql, _myOdbcConn);
                    _myReader = m_cmd.ExecuteReader();
                }


            }
            catch (InvalidOperationException)
            {
                _myOdbcConn.Open();
            }
            //catch (Exception exc)
            //{

            //    _myOdbcConn = null;
            //    _myOdbcConn = new OdbcConnection(m_stConnection);
            //    _myOdbcConn.Open();

            //}
        }
    }



}
