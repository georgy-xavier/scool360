using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;

namespace WinErParentLogin
{
    public class General
    {
        public MysqlClass m_MysqlDb;
        private OdbcDataReader m_MyReader5 = null;
        public General(MysqlClass _MysqlDb)
        {
            m_MysqlDb = _MysqlDb;
        }
        public static DateTime GetDateFromText(string _StrDate)
        {
            DateTime _outDate;
            string[] _DateArrayfirst = _StrDate.Split(' ');
            string[] _DateArray = _DateArrayfirst[0].Split('/');// store DD MM YYYY
            int _Day, _Month, _Year;
            _Day = int.Parse(_DateArray[0]);// day
            _Month = int.Parse(_DateArray[1]);// Month
            _Year = int.Parse(_DateArray[2]);// Year
            _outDate = new DateTime(_Year, _Month, _Day, 0, 0, 0);
            return _outDate;
        }

        public static string GerFormatedDatVal(DateTime dt)
        {

            return dt.Date.Day + "/" + dt.Date.Month + "/" + dt.Date.Year;


        }
        public int GetTableMaxIdWithCondition(string _TableName, string _Field, int _BatchId, string _Condition)
        {
            int Id = 0;
            string sql = "select max(" + _TableName + "." + _Field + ") from " + _TableName + " where " + _TableName + "." + _Condition + "=" + _BatchId;
            m_MyReader5 = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader5.HasRows)
            {
                bool valid = int.TryParse(m_MyReader5.GetValue(0).ToString(), out Id);
            }
            Id = Id + 1;
            return Id;
        }

        public int GetTableMaxId(string _TableName, string _Field)
        {
            int Id = 0;
            string sql = "select max(" + _TableName + "." + _Field + ") from " + _TableName + "";

            m_MyReader5 = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader5.HasRows)
            {
                bool valid = int.TryParse(m_MyReader5.GetValue(0).ToString(), out Id);
            }
            Id = Id + 1;
            return Id;
        }
    }
}
