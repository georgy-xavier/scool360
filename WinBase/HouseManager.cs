using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Data;

namespace WinBase
{
    public class HouseManager : KnowinGen
    {
        private KnowinUser MyUser;
        public MysqlClass m_MysqlDb;
        private OdbcDataReader m_MyReader = null;
        public MysqlClass m_TransationDb = null;

        public HouseManager(KnowinGen _Prntobj)
        {
            m_Parent = _Prntobj;
            m_MyODBCConn = m_Parent.ODBCconnection;
            m_UserName = m_Parent.LoginUserName;
            m_MysqlDb = new MysqlClass(this);
        }

        ~HouseManager()
        {
            if (m_MysqlDb != null)
            {
                m_MysqlDb = null;

            } if (m_MyReader != null)
            {
                m_MyReader = null;

            }

        }


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


        public DataSet GetAllActiveHouse()
        {
            string sql = "";
            DataSet HouseDs = new DataSet();
            sql = "SELECT tblhousemaster.Id,tblhousemaster.HouseName FROM tblhousemaster WHERE Status=1 ORDER BY Id";
            HouseDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return HouseDs;

        }

        public void AddNewHouse(string Housename,bool exist,int Id)
        {
            string sql = "";
            if (exist)
            {
                sql = "UPDATE tblhousemaster SET HouseName='"+Housename+"',Status=1  WHERE Id="+Id+"";
            }
            else
            {
                sql = "INSERT INTO tblhousemaster(HouseName,Status) VALUES('" + Housename + "',1)";
            }
          
            m_MysqlDb.ExecuteQuery(sql);
        }

        public bool HouseExist(string House, out bool existdel,out int Id)
        {
            string sql = "";
            Id = 0;
             existdel = false;
             bool exist = false;           
            OdbcDataReader HouseIdreader = null;
            sql = "SELECT tblhousemaster.Id,Status FROM tblhousemaster WHERE tblhousemaster.HouseName='"+House+"'";
            HouseIdreader = m_MysqlDb.ExecuteQuery(sql);
            if (HouseIdreader.HasRows)
            {
                if (HouseIdreader.GetValue(1).ToString() == "0")
                {
                    existdel = true;
                    int.TryParse(HouseIdreader.GetValue(0).ToString(), out Id);
                }
                else
                {
                    exist = true;

                }

            }
            return exist;
        }

        public void DeleteLocation(int Id)
        {
            string sql = "";
            sql = "UPDATE tblhousemaster SET Status=0  WHERE Id=" + Id + "";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public DataSet getStudentsFromClass(int classid)
        {
            string sql = "select tblstudent.Id, tblstudent.StudentName, tblstudent.RollNo, tblstudent.Sex, tblstudent.Address from tblstudent where tblstudent.LastClassId = " + classid + " and tblstudent.status = 1 and Id NOT IN (SELECT StudId from tblhousestudentmap) order by tblstudent.RollNo";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }

        public void SaveToDataBase(int houseid, int studid)
        {
            string sql = "";
            sql = "INSERT INTO tblhousestudentmap(HouseId,StudId) VALUES(" + houseid + "," + studid + ")";
            m_TransationDb.ExecuteQuery(sql);
        }


        public DataSet GetAllStudDataSet()
        {
            DataSet StudDs = new DataSet();
            string sql = "";
            sql = "select tblstudent.StudentName, tblstudent.Sex, tblstudent.RollNo, tblstudent.Address, tblstudent.Id , tblstudent.LastClassId from tblstudent where  tblstudent.Id not in( select StudId from tblhousestudentmap) and tblstudent.status=1 order by  tblstudent.LastClassId, tblstudent.Sex";
            StudDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return StudDs;
        }

        public DataSet GetStudentHouseMapReport(int classid, int houseid,int Gender)
        {
            string sql = "",tempsql="";
            DataSet ReportDs = new DataSet();

            //if(classid>0 & houseid>0)
            //{
            //    tempsql=" where tblstudent.LastClassId="+classid+" and tblhousestudentmap.HouseId="+houseid+"";
            //}
            //else 
            if (classid > 0)
            {
              
                tempsql = " where tblstudent.LastClassId=" + classid + " ";
            }
            if (houseid > 0)
            {
                if (tempsql == "")
                {
                    tempsql = " where tblhousestudentmap.HouseId=" + houseid + "";
                }
                else
                {
                    tempsql = tempsql+" and tblhousestudentmap.HouseId=" + houseid + "";

                }
            }
            if (Gender == 1)
            {
                if (tempsql == "")
                {
                    tempsql = " where tblstudent.Sex='Male'";
                }
                else
                {
                    tempsql = tempsql+" and tblstudent.Sex='Male'";

                }
            }
            if (Gender == 2)
            {
                if (tempsql == "")
                {
                    tempsql = " where tblstudent.Sex='Female'";
                }
                else
                {
                    tempsql =tempsql+ " and tblstudent.Sex='Female'";

                }
            }


            
            sql = "SELECT tblstudent.StudentName, tblstudent.Id as studid, tblstudent.Sex, tblclass.ClassName, tblhousemaster.HouseName, tblhousemaster.Id as houseid,  tblstudent.Address FROM  tblhousemaster inner join tblhousestudentmap on tblhousestudentmap.HouseId= tblhousemaster.Id inner join tblstudent  on  tblstudent.Id= tblhousestudentmap.StudId   inner join tblclass on tblclass.Id= tblstudent.LastClassId " + tempsql + " ";
            ReportDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return ReportDs;
        }
        public void updatehousestudentmap(int houseid, int studid)
        {
            DataSet ds = new DataSet();
            string sql = "";
            sql = "UPDATE tblhousestudentmap SET HouseId=" + houseid + " WHERE StudId=" + studid + ";";
            ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }
    }
}
