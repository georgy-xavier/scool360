using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Data;

namespace WinBase
{
    public class EmailManager: KnowinGen
    {
        private KnowinUser MyUser;
        public MysqlClass m_MysqlDb;
        private OdbcDataReader m_MyReader = null;
        public MysqlClass m_TransationDb = null;

         public EmailManager(KnowinGen _Prntobj)
        {
            m_Parent = _Prntobj;
            m_MyODBCConn = m_Parent.ODBCconnection;
            m_UserName = m_Parent.LoginUserName;
            m_MysqlDb = new MysqlClass(this);
        }

         ~EmailManager()
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

         public Boolean InitClass()
         {
             //1,"SMTP Server Name"," mail.winceron.com"
             //2,"Email From",NULL
             //3,"Email Id","Jisha@winceron.com"
             //4,"Password",""
             //5,"Port Number","25"

             //mSMTP = GetCheckURL_FromDatabase();
             //mSendURL = GetSendURL_FromDatabase();
             //mSymbol = GetSMS_NumberSeperator_FromDatabase();
             //mUsername = GetSMS_username_FromDatabase();
             //mPassword = GetSMS_password_FromDatabase();
             //mMaxCount = GetSMS_Max_Count_FromDatabase();
             return true;
         }


         public DataSet LoadClassDetails()
         {
             DataSet ClassDs = new DataSet();
             string _sql = "";
             _sql = "select tblclass.ClassName, tblclass.Id from tblclass where tblclass.Status=1 order by tblclass.Standard,tblclass.ClassName";
             ClassDs = m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
             return ClassDs;
         }

         public OdbcDataReader GetEmailStaffId(string staffId)
         {
             OdbcDataReader EmailReader = null;
             string sql = "";
             string tempSql="";
             if (staffId != "")
             {
                 tempSql = " and tbl_emailstafflist.Id="+staffId+"";
             }
             sql = "select  tbl_emailstafflist.EmailId,tbl_emailstafflist.Id from tbl_emailstafflist inner join  tbluser ON tbluser.Id=tbl_emailstafflist.Id where  tbl_emailstafflist.Enabled=1" + tempSql + "";
             EmailReader = m_TransationDb.ExecuteQuery(sql);
             return EmailReader;
         }

         public void InsertDataToAutoEmailList(string EmailAddress,string SenderId, string EmailSubject, string EmailBody, int Type)
         {
             string sql = "";
             if (SenderId == "")
             {
                 SenderId = "0";
             }
             sql = "Insert into tbl_autoemail(EmailAddress,EmailSubject,EmailBody,TimeTosend,`Type`,`Status`,SenderId) values('" + EmailAddress + "','" + EmailSubject + "','" + EmailBody + "','" + System.DateTime.Now.Date.ToString("s") + "'," + Type + ",0,'" + SenderId + "')";
             if (m_TransationDb != null)
             {
                 m_TransationDb.ExecuteQuery(sql);
             }
             else
             {
                 m_MysqlDb.ExecuteQuery(sql);
             }
         }
         public void InsertDataToAutoEmailListwithattachment(string EmailAddress, string SenderId, string EmailSubject, string EmailBody, int Type, string attach1, string attach2, string attach3)
         {
             string sql = "";
             if (SenderId == "")
             {
                 SenderId = "0";
             }
             sql = "Insert into tbl_autoemail(EmailAddress,EmailSubject,EmailBody,TimeTosend,`Type`,`Status`,SenderId,Attachment1,Attachment2,Attachment3) values('" + EmailAddress + "','" + EmailSubject + "','" + EmailBody + "','" + System.DateTime.Now.Date.ToString("s") + "'," + Type + ",0,'" + SenderId + "','" + attach1 + "','" + attach2 + "','" + attach3 + "')";
             m_TransationDb.ExecuteQuery(sql);
         }

         public OdbcDataReader GetEmailParentId(int ClassId,int studentId)
         {
             OdbcDataReader EmailReader = null;
             string sql = "";
             string tempSQl = "";
             if (ClassId > 0 && ClassId!=-1)
             {
                 tempSQl = " AND tblstudentclassmap.ClassId=" + ClassId + "";

             }
             if (studentId > 0 && studentId != -1)
             {
                 tempSQl = " AND tblstudentclassmap.studentId=" + studentId + "";
             }

             sql = "select  tbl_emailparentlist.EmailId,tbl_emailparentlist.Id from tbl_emailparentlist INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tbl_emailparentlist.Id where  tbl_emailparentlist.Enabled=1" + tempSQl;
             EmailReader = m_TransationDb.ExecuteQuery(sql);
             return EmailReader;
         }

         public string GetEmailParentId( int studentId)
         {
             string Emailid = "";
             OdbcDataReader EmailReader = null;
             string sql = "";
             string tempSQl = "";
             if (studentId > 0 && studentId != -1)
             {
                 tempSQl = " AND tblstudentclassmap.studentId=" + studentId + "";
             }

             sql = "select  tbl_emailparentlist.EmailId,tbl_emailparentlist.Id from tbl_emailparentlist INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tbl_emailparentlist.Id where  tbl_emailparentlist.Enabled=1" + tempSQl;
             if (m_TransationDb != null)
                 EmailReader = m_TransationDb.ExecuteQuery(sql);
             else
                 EmailReader = m_MysqlDb.ExecuteQuery(sql);
             if (EmailReader.HasRows)
             {
                 Emailid = EmailReader.GetValue(0).ToString();
             }

             return Emailid;
         }

         public OdbcDataReader GetEmailStudentId(int ClassId)
         {
             OdbcDataReader EmailReader = null;
             string sql = "";
             string tempSQl = "";
             if (ClassId > 0)
             {
                 tempSQl = " AND tblstudentclassmap.ClassId=" + ClassId + "";
             }
             sql = "select  tbl_emailstudentlist.EmailId,tbl_emailstudentlist.Id from tbl_emailstudentlist INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tbl_emailstudentlist.Id where  tbl_emailstudentlist.Enabled=1" + tempSQl;
             EmailReader = m_TransationDb.ExecuteQuery(sql);
             return EmailReader;
         }

         public DataSet GetStaffEmailDs()
         {
             string sql = "";
             DataSet StaffDs = new DataSet();
             sql = "SELECT tbluser.Id,tbluser.UserName as `Staff Name`, tbl_emailstafflist.EmailId,tbl_emailstafflist.Enabled FROM tbluser LEFT OUTER JOIN tbl_emailstafflist ON tbluser.Id=tbl_emailstafflist.Id WHERE tbluser.`Status`=1 AND tbluser.RoleId!=1 ORDER BY tbluser.UserName";
             StaffDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
             return StaffDs;
         }

         public DataSet GetAllEmailConfigType()
         {
             DataSet TypeDs = new DataSet();
             string sql = "";
             sql = "Select tbl_emailoptionconfig.Id, tbl_emailoptionconfig.`Type` from tbl_emailoptionconfig where tbl_emailoptionconfig.SetVisible=1 and ShortName<>''";
             TypeDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
             return TypeDs;
         }

         public void UpdateEmailConfig(string emailSubject, string emailBody, int enabeld, int Id, bool SchedulTime, string TimeID)
         {
             string sql = "";
             int  ScheduleTime = 0;
             if (SchedulTime)
             {
                 ScheduleTime = 1;

             }
             else
             {
                 TimeID = "0";
             }
             sql = "Update tbl_emailoptionconfig set Enabled=" + enabeld + ",Subject='" + emailSubject + "',Body='" + emailBody + "',ScheduledTime='" + TimeID + "',ScheduledTimeEnabled=" + ScheduleTime + " where Id=" + Id + "";
             m_MysqlDb.ExecuteQuery(sql);
         }

         public DataSet GetStaffLogDetails(int staffId)
         {
             DataSet StaffLog = new DataSet();
             string sql = "";
             string tempsql = "";
             if (staffId > 0)
             {
                 tempsql = " and tbl_emaillog.SenderId='" + staffId + "'";
             }

             sql = "select Id,EmailAddress,EmailSubject, DATE_FORMAT( tbl_emaillog.SendDate, '%d/%m/%Y') as TimeTosend ,`SendStatus` from tbl_emaillog where `Type`=1 " + tempsql + "";
             StaffLog = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
             return StaffLog;
         }

         public DataSet GetParentLogDetails(string StudentId)
         {
             DataSet EmailLogDs = new DataSet();
             string sql = "";
             sql = "";
             string tempsql = "";
             if (StudentId !="0")
             {
                 tempsql = " and tbl_emaillog.SenderId='" + StudentId + "'";
             }

             sql = "select Id,EmailAddress,EmailSubject, DATE_FORMAT( tbl_emaillog.SendDate, '%d/%m/%Y') as TimeTosend ,`SendStatus` from tbl_emaillog where (`Type` =2 or `Type` =4)  " + tempsql + "";
             EmailLogDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
             return EmailLogDs;
         }

         public void UpdateGeneralTemplate(string emailSubject, string emailBody, int enabeld, int Id)
         {
             string sql = "";
             sql = "Update tbl_generalemailtemplate set Enabled=" + enabeld + ",Subject='" + emailSubject + "',Body='" + emailBody + "' where Id=" + Id + "";
             m_MysqlDb.ExecuteQuery(sql);
           
         }

         public void SaveNewTemplate(string templatesubject, string templateBody)
         {
             string sql = "";
             sql = "insert into tbl_generalemailtemplate(`Type`,Enabled,Subject,Body,ShortName,SetVisible) values('" + templatesubject + "',1,'" + templatesubject + "','" + templateBody + "','" + templatesubject + "',1)";
             m_MysqlDb.ExecuteQuery(sql);
             sql = "Select Max(Id) from tbl_generalemailtemplate";
             m_MyReader = m_MysqlDb.ExecuteQuery(sql);
             if (m_MyReader.HasRows)
             {
                 string _sql = " select tbl_generalemailseperators.Id from tbl_generalemailseperators";
                 OdbcDataReader SepReader = m_MysqlDb.ExecuteQuery(_sql);
                 if (SepReader.HasRows)
                 {
                     while (SepReader.Read())
                     {
                         sql = "insert into tbl_generaltemplateseperatormap(TemplateId,SeperatorId) values(" + int.Parse(m_MyReader.GetValue(0).ToString()) + "," + int.Parse(SepReader.GetValue(0).ToString()) + ") ";
                         m_MysqlDb.ExecuteQuery(sql);
                     }
                 }
             }
         }

         public bool Email_Enabled(string ShortName, out string Subject,out string body)
         {
             bool valid = false;
             Subject = "";
             body = "";
             string Enable = "";
             //Type,Enabled,Subject,Body,ShortName,SetVisible
             string sql = "select Enabled,Subject,Body from tbl_emailoptionconfig WHERE SetVisible=1 AND ShortName='" + ShortName + "' ";
             m_MyReader = m_MysqlDb.ExecuteQuery(sql);
             if (m_MyReader.HasRows)
             {
                 Enable = m_MyReader.GetValue(0).ToString();
                 if (Enable == "1")
                 {
                     valid = true;
                     Subject = m_MyReader.GetValue(1).ToString();
                     body = m_MyReader.GetValue(2).ToString();
                 }

             }
             return valid;
         }

         public void GetParentDetails(int StudentId, out string Email, out bool ParentEnabled, out string StudentName, out string ParentName)
         {
             string sql = "";
             Email = "";
             int _status = 0;
             ParentEnabled = false;
             StudentName = "";
             ParentName = "";

             string _sql = "SELECT StudentName,GardianName FROM tblview_student WHERE Id=" + StudentId;
             OdbcDataReader mReader = m_MysqlDb.ExecuteQuery(_sql);
             if (mReader.HasRows)
             {
                 StudentName = mReader.GetValue(0).ToString();
                 ParentName = mReader.GetValue(1).ToString();
             }
             sql = "SELECT EmailId,Enabled FROM tbl_emailparentlist WHERE Id=" + StudentId;
             OdbcDataReader mReader1 = m_MysqlDb.ExecuteQuery(sql);
             if (mReader1.HasRows)
             {
                 Email = mReader1.GetValue(0).ToString();
                 int.TryParse(mReader1.GetValue(1).ToString(), out _status);
                 if (_status == 1)
                 {
                     ParentEnabled = true;
                 }
             }
         }



         public string SeperatorReplace(int StaffId, int StudentId, string EmailBody)
         {
             string sql = "", _sql = "";
             OdbcDataReader staffReader = null, StudentReader = null;
             if (StaffId > 0)
             {
                 sql = "select tbluser.SurName from tbluser where tbluser.Id=" + StaffId + "";
                 staffReader = m_TransationDb.ExecuteQuery(sql);
                 if (staffReader.HasRows)
                 {
                     EmailBody = EmailBody.Replace("($StaffName$)", staffReader.GetValue(0).ToString());
                 }

             }
             if (StudentId > 0)
             {
                 //StudentName,GardianName
                 _sql = "select tblview_student.StudentName, GardianName from tblview_student where tblview_student.Id=" + StudentId + "";
                 StudentReader = m_TransationDb.ExecuteQuery(_sql);
                 if (StudentReader.HasRows)
                 {
                     EmailBody = EmailBody.Replace("($Parent$)", StudentReader.GetValue(1).ToString());
                     EmailBody = EmailBody.Replace("($Student$)", StudentReader.GetValue(0).ToString());
                 }
             }

            return EmailBody;
             
         }
      
    }
}
