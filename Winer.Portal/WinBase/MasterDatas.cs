using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;

namespace WinBase
{
    public class MasterDatas:KnowinGen
    {
        public MysqlClass m_MysqlDb;
        private OdbcDataReader m_MyReader = null;
        public MysqlClass m_TransationDb = null;
        private CLogging logger = null;
        public MasterDatas(KnowinGen _Prntobj)
        {
            m_Parent = _Prntobj;
            m_MyODBCConn = m_Parent.ODBCconnection;
            m_UserName = _Prntobj.LoginUserName;
            m_userid = _Prntobj.User_Id;
            m_MysqlDb = new MysqlClass(this);
            logger = CLogging.GetLogObject();
        }
        ~MasterDatas()
        {

            if (m_MysqlDb != null)
            {
                m_MysqlDb = null;

            } if (m_MyReader != null)
            {
                m_MyReader = null;

            }
            if (m_TransationDb != null)
            {
                m_TransationDb = null;

            }
            if (logger != null)
            {
                logger = null;

            } 

        }

        #region Transaction Area

        public void CreateTansationDb()
        {
            CLogging logger = CLogging.GetLogObject();
            logger.LogToFile("CreateTansationDb", "Starting New Fee Transaction", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
            if (m_TransationDb != null)
            {

                m_TransationDb.TransactionRollback();
                m_TransationDb = null;
            }

            m_TransationDb = new MysqlClass(this);
            m_TransationDb.MyBeginTransaction();

        }
        public void EndSucessTansationDb()
        {
            if (m_TransationDb != null)
            {
                m_TransationDb.TransactionCommit();
                m_TransationDb = null;
            }


        }
        public void EndFailTansationDb()
        {
            if (m_TransationDb != null)
            {
                m_TransationDb.TransactionRollback();
                m_TransationDb = null;
            }

        }
       
        #endregion




    }
}
