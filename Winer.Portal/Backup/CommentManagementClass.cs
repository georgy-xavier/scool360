using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
namespace WinBase
{
    public class CommentManagementClass
    {
         public MysqlClass m_MysqlDb;
        //private OdbcDataReader m_MyReader = null;
        public CommentManagementClass(MysqlClass _MysqlDb)
        {
            m_MysqlDb = _MysqlDb;
        }

        public int CreateNewCommentReturnThredId(string _ThreadName, string _Comment, string m_UserName)
        {
            int _threadId = 0;
            _threadId=CreateThread(_ThreadName);
            AddComment(_threadId, _Comment, m_UserName);
            return _threadId;
            
        }

        public void AddComment(int _threadId, string _Comment, string m_UserName)
        {
            string sql = "INSERT INTO tblcomment(ThreadId,Comment,CreatedDate,CreatedUser) VALUES(" + _threadId + ",'" + _Comment + "','" + DateTime.Now.ToString("s") + "','" + m_UserName + "')";
            m_MysqlDb.ExecuteQuery(sql);
        }

        private int CreateThread(string _ThreadName)
        {
            int _threadId = 0;
            General _GeneralObj = new General(m_MysqlDb);
            _threadId = _GeneralObj.GetTableMaxId("tblcommentthread", "Id");

            string sql = "INSERT INTO tblcommentthread(Id,ThreadName) VALUES(" + _threadId + ",'" + _ThreadName + "')";
            m_MysqlDb.ExecuteQuery(sql);

            return _threadId;
        }

        public System.Data.Odbc.OdbcDataReader ReadHeading(int CommentThredId)
        {
            string sql = " select tblcommentthread.Id, tblcommentthread.ThreadName from tblcommentthread where tblcommentthread.Id=" + CommentThredId;
            OdbcDataReader Myreader = m_MysqlDb.ExecuteQuery(sql);
            return Myreader;
        }

        public bool IsExist(int CommentThredId)
        {
            string sql = " select tblcommentthread.Id from tblcommentthread where tblcommentthread.Id=" + CommentThredId;
            OdbcDataReader Myreader = m_MysqlDb.ExecuteQuery(sql);
            if (Myreader.HasRows)
                return true;
            else
                return false;
        }

        public StringBuilder GetComments(int CommentThredId, KnowinUser MyUser)
        {
            StringBuilder _Comments = new StringBuilder();
            int _TempId=0;
            string sql = "select tblcomment.`Comment`, DATE_FORMAT(tblcomment.CreatedDate, '%d-%m-%Y %h:%i %p'), tblcomment.CreatedUser from tblcomment where tblcomment.ThreadId=" + CommentThredId;
            OdbcDataReader Myreader = m_MysqlDb.ExecuteQuery(sql);
           
            if (Myreader.HasRows)
            {
                _Comments.AppendLine(" <table  width=\"100%\" >");
                while (Myreader.Read())
                {
                    int id = MyUser.GetUserIDFromName(Myreader.GetValue(2).ToString());
                    string url = "Handler/ImageReturnHandler.ashx?id=" + id + "&type=UserImage";
                        // MyUser.GetImageUrl("UserImage", id);
                    if (_TempId == 0)
                    {
                        _Comments.AppendLine("  <tr class=\"firstraw\"><td  class=\"userId\" ><img alt=\"\" src=\"" + url + "\" width=\"38px\" height=\"38px\" /><br /><b>" + Myreader.GetValue(2).ToString() + " </b> </td><td class=\"description\"  align=\"left\" > <div class=\"time\">" + Myreader.GetValue(1).ToString() + "</div> <div class=\"content\" align=\"left\">" + Myreader.GetValue(0).ToString() + "</div></td></tr>");
                        _TempId = 1;
                    }
                    else
                    {
                        _Comments.AppendLine("  <tr class=\"secondraw\"><td  class=\"userId\" ><img alt=\"\" src=\"" + url + "\" width=\"38px\" height=\"38px\" /><br /><b>" + Myreader.GetValue(2).ToString() + " </b> </td><td class=\"description\"  align=\"left\"> <div class=\"time\">" + Myreader.GetValue(1).ToString() + "</div>  <div class=\"content\" align=\"left\">" + Myreader.GetValue(0).ToString() + "</div></td></tr>");
                   
                        _TempId = 0;
                    }
                }
                _Comments.AppendLine(" </table >");
            }
            else
            {
                _Comments.AppendLine("No comments exist");
            }

            return _Comments;
        }



        public int GetCommentId(string _Table, string _CommentIdField, string _TablereffField, string _ReffValue)
        {
            int _CommentId = 0;
            string sql = " select " + _CommentIdField + " from " + _Table + " where " + _TablereffField + "='" + _ReffValue+"'";
            OdbcDataReader Myreader = m_MysqlDb.ExecuteQuery(sql);
            if (Myreader.HasRows)
                _CommentId =int.Parse(Myreader.GetValue(0).ToString());

            return _CommentId;
        }
    }
}
