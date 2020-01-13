using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Configuration;

namespace WinBase
{
    public class CentralDataBase
    {
        private OdbcConnection _myOdbcConn = null;
        public string ConnectionStr = null;

        public CentralDataBase()
        {
            ConnectionStr = ConfigurationManager.AppSettings["CentralConnectionInfo"];
            _myOdbcConn = new OdbcConnection(ConnectionStr);
            _myOdbcConn.Open();
        }

        public bool ExquteQuery(string m_Str_sql)
        {
            bool valid = true;
            try
            {
                OdbcCommand m_cmd = new OdbcCommand(m_Str_sql, _myOdbcConn);
                m_cmd.ExecuteReader();
            }
            catch
            {
                valid = false;
            }
            return valid;
        }

        public DataSet ExecuteReturnDataset(string m_Str_sql, string str_DataSetName)
        {
            DataSet _myDataset = new DataSet();
            OdbcDataAdapter _myAdapter = new OdbcDataAdapter();
            OdbcCommand m_cmd = new OdbcCommand(m_Str_sql, _myOdbcConn);
            _myAdapter.SelectCommand = m_cmd;
            _myAdapter.Fill(_myDataset, str_DataSetName);
            return _myDataset;

        }
    }
}
