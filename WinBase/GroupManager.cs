using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;

namespace WinBase
{
    public class GroupManager : KnowinGen
    {

        private KnowinUser MyUser;
        public MysqlClass m_MysqlDb;
        private OdbcDataReader m_MyReader = null;
        public MysqlClass m_TransationDb = null;

        public GroupManager(KnowinGen _Prntobj)
        {
            m_Parent = _Prntobj;
            m_MyODBCConn = m_Parent.ODBCconnection;
            m_UserName = m_Parent.LoginUserName;
            m_MysqlDb = new MysqlClass(this);
        }

        ~GroupManager()
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

        public DataSet GetAllGroup()
        {
            string sql = "";
            sql = "Select tbl_gr_master.Id,tbl_gr_master.GroupName,Description from tbl_gr_master";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        }

        public bool GroupExist(string groupname, string Id)
        {
            bool exist = false;
            string sql = "";
            OdbcDataReader GroupIdReader = null;
            if (Id != "")
            {
                sql = "select id from tbl_gr_master where tbl_gr_master.GroupName='" + groupname + "' and Id<>" + Id + "";
            }
            else
            {
                sql = "select id from tbl_gr_master where tbl_gr_master.GroupName='" + groupname + "'";
            }
          
            GroupIdReader = m_MysqlDb.ExecuteQuery(sql);
            if (GroupIdReader.HasRows)
            {
                exist = true;
            }
            return exist;
        }

        public void DeleteGroup(int Id)
        {

            string sql = "";
            sql = "Delete from tbl_gr_master  WHERE Id=" + Id + "";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public DataSet GetStaffDetails(int userid)
        {
            string sql = "";
            sql = "SELECT DISTINCT tbluser.`Id`,tbluser.`SurName`, tbluser.`UserName`,tblrole.`RoleName` FROM tbluser   inner join tblstaffdetails on tblstaffdetails.UserId = tbluser.Id  inner join tblrole on tbluser.`RoleId`=tblrole.`Id`   inner join tblgroupusermap on tbluser.`Id`=tblgroupusermap.`UserId`  where tbluser.`Status`=1 AND tblrole.`Type`='Staff'  AND tblgroupusermap.`GroupId`  IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId   INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE     tblgroupusermap.UserId=" + userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + userid + ")";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            
        }

        public void MapUser(int usertype, int userid, string username, int groupid)
        {
            string sql = "";
            sql = "Insert Into tbl_gr_groupusermap(UserName,Type,GroupId,UserId) Values('" + username + "'," + usertype + "," + groupid + "," + userid + ")";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public DataSet GetAllStaffInGroup(int GroupId, int type)
        {
            string sql = "", tempsql = "";
            if (GroupId > 0)
            {
                tempsql = " and tbl_gr_groupusermap.GroupId=" + GroupId + "";
            }
            sql = "select tbluser.SurName, tbluser.UserName , tbl_gr_master.GroupName, tbluser.Id,tbl_gr_groupusermap.Id as GroupId from tbluser inner join tbl_gr_groupusermap on   tbl_gr_groupusermap.UserId = tbluser.Id inner join tbl_gr_master on tbl_gr_master.Id =   tbl_gr_groupusermap.GroupId Where tbl_gr_groupusermap.Type=" + type + " " + tempsql + "";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }

        public DataSet GetAllStudentInGroup(int GroupId,int classid, int type)
        {
            string sql = "", tempsql = "";
            if (GroupId > 0)
            {
                tempsql = " and tbl_gr_groupusermap.GroupId=" + GroupId + "";
            }
            if (classid > 0)
            {
                tempsql = tempsql + " and tblstudent.lastClassId=" + classid + ""; ;
            }
            sql = "select tblstudent.Id, tblstudent.StudentName, tblstudent.RollNo, tblstudent.Sex, tbl_gr_groupusermap.Id as GroupId ,tbl_gr_master.GroupName from tblstudent inner join tbl_gr_groupusermap on tbl_gr_groupusermap.UserId=  tblstudent.Id inner join tbl_gr_master on tbl_gr_master.Id= tbl_gr_groupusermap.GroupId   where  tblstudent.status = 1 and tbl_gr_groupusermap.Type=" + type + " " + tempsql + "";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }

        public void UpdateGroupUserMap(int usertype, int userid, string username, int groupid,int grouptableid)
        {
            string sql = "";
            sql = "Update tbl_gr_groupusermap Set UserName='" + username + "',Type=" + usertype + ",GroupId=" + groupid + ",UserId=" + userid + " where tbl_gr_groupusermap.Id=" + grouptableid + "";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public bool UserMappedToGroup(int Id)
        {
            string sql = "";
            bool mapped = false;
            OdbcDataReader GroupMapIdReader = null;
            sql = "select tbl_gr_groupuser.Id from tbl_gr_groupusermap where Groupid=" + Id + "";
            GroupMapIdReader = m_MysqlDb.ExecuteQuery(sql);
            if (GroupMapIdReader.HasRows)
            {
                mapped = true;
            }
            return mapped;
        }

        public bool UserMappedToSurvey(int Id)
        {
            string sql = "";
            bool mapped = false;
            OdbcDataReader GroupMapIdReader = null;
            sql = "select tbl_survey.Id from tbl_survey where Survey_Id=" + Id + "";
            GroupMapIdReader = m_MysqlDb.ExecuteQuery(sql);
            if (GroupMapIdReader.HasRows)
            {
                mapped = true;
            }
            return mapped;
        }

        public DataSet getStudentsFromClass(int classid)
        {
            string tempsql = "";
            if (classid > 0)
            {
                tempsql = " and tblstudent.LastClassId = " + classid + "";
            }
            string sql = "select tblstudent.Id, tblstudent.StudentName, tblstudent.RollNo, tblstudent.Sex, tblstudent.Address from tblstudent where  tblstudent.status = 1 " + tempsql + "";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }

        public DataSet GetUserMapDetails(int groupid, int type)
        {
            string sql = "", tempsql = "";
            if (groupid > 0)
            {
                tempsql = " where tbl_gr_groupusermap.GroupId = " + groupid + "";
            }
            if(type!=2 )
            {
                if (tempsql == "")
                {
                    tempsql = " where tbl_gr_groupusermap.`type`=" + type + "";
                }
                else
                {
                    tempsql = tempsql + " and tbl_gr_groupusermap.`type`=" + type + "";
                }
                
            }
            sql = "  SELECT tbl_gr_groupusermap.UserName, tbl_gr_master.GroupName ,  (CASE    WHEN (`type` = 0) THEN 'Student'    WHEN (`type` = 1) THEN 'Staff'  END) as `Type` FROM tbl_gr_groupusermap  inner join  tbl_gr_master on tbl_gr_master.Id= tbl_gr_groupusermap.GroupId " + tempsql + "";

            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            
        }

        public void DeleteGroupUserMap(int GroupTableid)
        {
            string sql = "";
            sql = "delete from  tbl_gr_groupusermap  where tbl_gr_groupusermap.Id=" + GroupTableid + "";
            m_TransationDb.ExecuteQuery(sql);
        }

        public DataSet GetAllUsers(int groupid, int type)
        {
            string sql = "", tempsql = "";
           
            if (type !=-1)
            {
                tempsql = " where tbl_gr_groupusermap.Type=" + type + "";
            }
            if (groupid > 0)
            {
                if (tempsql == "")
                {
                    tempsql = " where tbl_gr_groupusermap.GroupId=" + groupid + "";
                }
                else
                {
                    tempsql = tempsql + " and tbl_gr_groupusermap.GroupId=" + groupid + "";
                }
            }

           // 
            sql = "select DISTINCT tbl_gr_groupusermap.UserName, tbl_gr_groupusermap.UserId, tbl_gr_groupusermap.`Type` as typeid, tbl_gr_master.GroupName ,(CASE    WHEN (`type` = 0) THEN 'Student'    WHEN (`type` = 1) THEN 'Staff'  END) as `Type` from tbl_gr_groupusermap  inner join tbl_gr_master on tbl_gr_master.Id= tbl_gr_groupusermap.GroupId " + tempsql + "";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        }
        public DataSet get_studentgroups(int student_id)
        {
            string sql = "select tbl_gr_master.GroupName from tbl_gr_master inner join tbl_gr_groupusermap on tbl_gr_groupusermap.UserId=" + student_id + " and tbl_gr_master.Id= tbl_gr_groupusermap.GroupId and tbl_gr_groupusermap.Type=0";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        }
        public DataSet get_staffgroups(int staff_id)
        {
            string sql = "select tbl_gr_master.GroupName from tbl_gr_master inner join tbl_gr_groupusermap on tbl_gr_groupusermap.UserId=" + staff_id + " and tbl_gr_master.Id= tbl_gr_groupusermap.GroupId and tbl_gr_groupusermap.Type=1";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        }
        public DataSet Is_usergroupexist(int stud_id, int Grp_Id)
        {
            string sql = "select Id from tbl_gr_groupusermap where UserId=" + stud_id + " and GroupId=" + Grp_Id + " and Type=0";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        }
        public DataSet Is_staffgroupexist(int staff_id, int Group_Id)
        {
            string sql = "select Id from tbl_gr_groupusermap where UserId=" + staff_id + " and GroupId=" + Group_Id + " and Type=1";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        }
     
      
       
        public DataSet Get_Survey_all()
        {
            string sql = "select * from tbl_survey";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }
        public DataSet Get_Selected_Survey(int Group_Id)
        {
            string sql = "select * from tbl_survey where Group_id = '" + Group_Id + "'";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }

        public void Map_ques(int Groupid, int survey_id, string surveyname, string Groupname, string Ques_type, string Question, string Answer)
        {
            string sql = "Insert Into tbl_survey(Group_id,Survey_Id,Survey_name,Group_name,Question,Ques_type,Answer) Values(" + Groupid + "," + survey_id + ",'" + surveyname + "','" + Groupname + "','" + Question + "','" + Ques_type + "','" + Answer + "')";
            m_MysqlDb.ExecuteQuery(sql);
        }
        public void Map_Update(int id, int Groupid, int survey_id, string surveyname, string Groupname, string Question, string Ques_type, string ans)
        {
            string sql = "";
            sql = "Update tbl_survey Set Group_id=" + Groupid + ",Survey_Id=" + survey_id + ",Survey_name='" + surveyname + "',Group_name='" + Groupname + "',Question='" + Question + "',Ques_type='" + Ques_type + "',Answer='" + ans + "' where tbl_survey.Id=" + id + "";
            m_TransationDb.ExecuteQuery(sql);
        }

        public void Del_Survey(int id)
        {
            string sql = "";
            sql = "delete from  tbl_survey  where tbl_survey.Id=" + id + "";
            m_TransationDb.ExecuteQuery(sql);
        }

        public DataSet Get_SurveyCreation()
        {
            string sql = "select * from tbl_surveycreation";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }

        public void Create_Survey(string surveyname, string lastdate)
        {
            string sql = "Insert Into tbl_surveycreation(Survey_name,Last_Date) Values('" + surveyname + "','" + lastdate + "')";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public void Delete_Survey(int Id)
        {
            string sql = "";
            sql = "Delete from tbl_survey WHERE Id=" + Id + "";
            m_MysqlDb.ExecuteQuery(sql);
        }
       
        //public void Map_ques(int Groupid, int survey_id, string surveyname, string Groupname, string Ques_type, string Question, string Answer)
        //{
        //    string sql = "Insert Into tbl_survey(Group_id,Survey_Id,Survey_name,Group_name,Question,Ques_type,Answer) Values(" + Groupid + "," + survey_id + ",'" + surveyname + "','" + Groupname + "','" + Question + "','" + Ques_type + "','" + Answer + "')";
        //    m_MysqlDb.ExecuteQuery(sql);
        //}
        //public void Map_Update(int id, int Groupid, int survey_id, string surveyname, string Groupname, string Question, string Ques_type, string ans)
        //{
        //    string sql = "";
        //    sql = "Update tbl_survey Set Group_id=" + Groupid + ",Survey_Id=" + survey_id + ",Survey_name='" + surveyname + "',Group_name='" + Groupname + "',Question='" + Question + "',Ques_type='" + Ques_type + "',Answer='" + ans + "' where tbl_survey.Id=" + id + "";
        //    m_TransationDb.ExecuteQuery(sql);
        //}
        //public DataSet GetAllSurvey()
        //{
        //    string sql = "";
        //    sql = "Select tbl_surveycreation.Id,tbl_surveycreation.Survey_name,Last_Date from tbl_surveycreation";
        //    return m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        //}
      
        public void DeleteSurvey(int Id)
        {

            string sql = "";
            sql = "Delete from tbl_surveycreation WHERE Id=" + Id + "";
            m_MysqlDb.ExecuteQuery(sql);
        }
        public bool SurveynameExist(int Id)
        {
            bool exist = false;
            string sql = "";
            OdbcDataReader GroupIdReader = null;
            if (Id != 0)
            {
                sql = "select Id from tbl_survey where Survey_Id=" + Id + "";
            }

            GroupIdReader = m_MysqlDb.ExecuteQuery(sql);
            if (GroupIdReader.HasRows)
            {
                exist = true;
            }
            return exist;
        }
        public DataSet GetAllSurvey()
        {
            string sql = "";
            sql = "Select tbl_surveycreation.Id,tbl_surveycreation.Survey_name,Last_Date from tbl_surveycreation";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        }
    }
}
