using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
namespace WinBase
{
    public class Reports:KnowinGen
    {
        public MysqlClass m_MysqlDb;
        private OdbcDataReader m_MyReader = null;
        private string m_ReportMenuString;
        public Reports(KnowinGen _Prntobj)
        {
            m_Parent = _Prntobj;
            m_MyODBCConn = m_Parent.ODBCconnection;
            m_UserName = m_Parent.LoginUserName;
            m_MysqlDb = new MysqlClass(this);
            m_ReportMenuString = "";
        }
        ~Reports()
         {
             if (m_MysqlDb != null)
                 {
                 m_MysqlDb = null;

                 } 
            if (m_MyReader != null)
                {
                 m_MyReader = null;

                }
       
        }


        public string GetReportMenuString(int _RoleId)
        {

            string _MenuStr;
            if (m_ReportMenuString == "")
            {


                _MenuStr = "<ul><li><a href=\"ReportHome.aspx\">Reports Home</a></li>";

                //string sql = "SELECT DISTINCT tblaction.ActionName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _RoleId + " AND tblroleactionmap.ModuleId=24 AND tblaction.ActionType='Link' order by tblaction.Order";

                //m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                //if (m_MyReader.HasRows)
                //{

                //    while (m_MyReader.Read())
                //    {


                //        _MenuStr = _MenuStr + "<li><a href=\"" + m_MyReader.GetValue(1).ToString() + "\">" + m_MyReader.GetValue(0).ToString() + "</a></li>";
                //    }

                //}
                //_MenuStr = _MenuStr + "</ul>";
                //m_MyReader.Close();
                m_ReportMenuString = _MenuStr;


            }
            else
            {
                _MenuStr = m_ReportMenuString;
            }

            return _MenuStr;
        }

       
    }
}
