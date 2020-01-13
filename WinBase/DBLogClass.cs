using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Configuration;

namespace WinBase
{
    public class DBLogClass
    {
        public MysqlClass m_MysqlDb = null;
        private OdbcDataReader m_MyReader = null;
        public MysqlClass m_TransationDb = null;
        string DbLogingStatus=ConfigurationSettings.AppSettings["NeedDbLoging"];
  
        public DBLogClass(MysqlClass _MysqlCldObj)
        {
            m_MysqlDb = _MysqlCldObj;
        }
       
        public void LogToDb(string _UserName, string _Action, string _Discription, int _level, MysqlClass _MysqlTransCldObj)
        {
            if (DbLogingStatus == "1")
            {
                string sql = "INSERT INTO tbllog(UserName,Action,Time,Description,Level) VALUES('" + _UserName + "','" + _Action + "','" + DateTime.Now.ToString("s") + "','" + _Discription + "'," + _level + ")";

                _MysqlTransCldObj.TransExecuteQuery(sql);
                
            }
        }
        public void LogToDbNoti(string _UserName, string _Action, string _Discription, int _level,int _notiLevel, MysqlClass _MysqlTransCldObj)
        {
            if (DbLogingStatus == "1")
            {
                string sql = "INSERT INTO tbllog(UserName,Action,Time,Description,Level,NotificationLevel) VALUES('" + _UserName + "','" + _Action + "','" + DateTime.Now.ToString("s") + "','" + _Discription + "'," + _level + "," + _notiLevel + ")";

                _MysqlTransCldObj.TransExecuteQuery(sql);

            }
        }
        public void LogToDb(string _UserName,string _Action,string _Discription,int _level)
        {
            if (DbLogingStatus == "1")
            {

                string sql = "INSERT INTO tbllog(UserName,Action,Time,Description,Level) VALUES('" + _UserName + "','" + _Action + "','" + DateTime.Now.ToString("s") + "','" + _Discription + "'," + _level + ")";
                if (m_TransationDb != null)
                {
                    m_MyReader = m_TransationDb.ExecuteQuery(sql);
                }
                else
                {
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                }
            }
        }
        public void LogToDbNoti(string _UserName, string _Action, string _Discription, int _level,int _notiLevel)
        {
            if (DbLogingStatus == "1")
            {

                string sql = "INSERT INTO tbllog(UserName,Action,Time,Description,Level,NotificationLevel) VALUES('" + _UserName + "','" + _Action + "','" + DateTime.Now.ToString("s") + "','" + _Discription + "'," + _level + "," + _notiLevel + ")";
                if (m_TransationDb != null)
                {
                    m_MyReader = m_TransationDb.ExecuteQuery(sql);
                }
                else
                {
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                }
            }
        }
        public void LogToDb(string _UserName, string _Action, string _Discription, int _level,int UserType)
        {
            if (DbLogingStatus == "1")
            {

                string sql = "INSERT INTO tbllog(UserName,Action,Time,Description,Level,UserTypeId) VALUES('" + _UserName + "','" + _Action + "','" + DateTime.Now.ToString("s") + "','" + _Discription + "'," + _level + "," + UserType + ")";
                if (m_TransationDb != null)
                {
                    m_MyReader = m_TransationDb.ExecuteQuery(sql);
                }
                else
                {
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                }
            }
        }
        public string GetStudentName(int _id)
        {
            string name = null;
            string sql = "select distinct tblstudent.StudentName from tblstudent where tblstudent.Id=" + _id;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
                name = m_MyReader.GetValue(0).ToString();
            return name;
        }
        public string GetStudentName(int _id, MysqlClass _MysqlTransCldObj)
        {
            string name = null;
            string sql = "select distinct tblstudent.StudentName from tblstudent where tblstudent.Id=" + _id;
            m_MyReader = _MysqlTransCldObj.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
                name = m_MyReader.GetValue(0).ToString();
            return name;
        }
    
    }
}
