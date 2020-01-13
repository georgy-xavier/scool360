using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Data;
using System.Collections;
namespace WinBase
{
  
   public struct Node
    {
        public int SubjectId;
        public int StaffId;
        public bool IsFixed;
        public bool IsAllocated;
        public bool IsEnabled;
        public bool IsNextPeordBreak;
        public int StaffSubjectNodeIndex;
    }

   public struct StaffSubjectNode
   {
       public int ClassId;
       public int SubjectId;
       public int StaffId;
       public int NPerod;
       public int NAllocated;
       public bool IsCompleted;
       public int StaffNodeIndex;
       public bool IsFreePerod;
       public int PerodType;
       public int ContinousPerodCount;
   }
   public struct StaffNode
   {
       public int StaffId;
       public int MaxPerodPerWeak;
       public int MaxPerodPerDay;
       public int AllocatedPerods;
       
       
   }


   public class TimeTable : KnowinGen
   {
       enum PerodType { Normal = 0,ClassTeacher,Continous,Combined};
       enum  AllocationStates { Failed = 0, Success = 1,Completed =2 };
       public MysqlClass m_MysqlDb;
       public MysqlClass m_TransationDb = null;
       public DBLogClass m_DbLog = null;
       private OdbcDataReader m_MyReader = null;
       private DataSet m_MyDataSet = null;
       private bool[, ,] m_StaffCube;
       private Node[, ,] m_TimeTableCube;
       private StaffSubjectNode[] m_StaffSubjectList;
       private StaffNode[] m_StaffList;
       private int[] M_ClassId;
       private int[] M_DayId;
       private int[] M_PeriodId;
       private int M_MaxClassCount;
       private int M_MaxDayCount;
       private int M_MaxPeriodCount;
       private int M_MaxStaffCount;
       private int M_MaxStaffSubCount;


       private string m_TimeTableMenuString;
       public TimeTable(KnowinGen _Prntobj)
       {
           m_Parent = _Prntobj;
           m_MyODBCConn = m_Parent.ODBCconnection;
           m_UserName = m_Parent.LoginUserName;
           m_MysqlDb = new MysqlClass(this);
           m_TimeTableMenuString = "";
       }
       public TimeTable(MysqlClass _Msqlobj)
       {
           m_MysqlDb = _Msqlobj;
       }
       ~TimeTable()
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
       public string GetTimeTMangMenuString(int _roleid)
       {
           CLogging logger = CLogging.GetLogObject();
           string _MenuStr;
           if (m_TimeTableMenuString == "")
           {


               _MenuStr = "<ul><li><a href=\"TimeTableHome.aspx\">TimeTable Home</a></li>";
               logger.LogToFile("GetTimeTMangMenuString", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
               string sql = "SELECT DISTINCT tblaction.MenuName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND (( tblroleactionmap.ModuleId=25 AND tblaction.ActionType='Link') Or tblaction.ActionType='TimeLink') ";
               logger.LogToFile("GetTimeTMangMenuString", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
               m_MyReader = m_MysqlDb.ExecuteQuery(sql);
               if (m_MyReader.HasRows)
               {
                   logger.LogToFile("GetTimeTMangMenuString", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                   while (m_MyReader.Read())
                   {
                       _MenuStr = _MenuStr + "<li><a href=\"" + m_MyReader.GetValue(1).ToString() + "\">" + m_MyReader.GetValue(0).ToString() + "</a></li>";
                   }

               }
               _MenuStr = _MenuStr + "</ul>";
               logger.LogToFile("GetTimeTMangMenuString", " Closing MyReader ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
               m_MyReader.Close();
               m_TimeTableMenuString = _MenuStr;

           }
           else
           {
               _MenuStr = m_TimeTableMenuString;
           }
           logger.LogToFile("GetTimeTMangMenuString", "Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
           return _MenuStr;

       }

       public void SaveGenRules(string _PeridDay, int _ClassTeacher, string _MaxConsecutive, string _MaxteacherSub, string _MaxStaffWeekperiod, string _MaxStaffDayperiod)
       {
           string sql = "update tbltime_config set value1="+_PeridDay+" where Id=1";
           m_MysqlDb.ExecuteQuery(sql);
           sql = "update tbltime_config set value1=" + _ClassTeacher + " where Id=2";
           m_MysqlDb.ExecuteQuery(sql);
           sql = "update tbltime_config set value1=" + _MaxConsecutive + " where Id=3";
           m_MysqlDb.ExecuteQuery(sql);
           sql = "update tbltime_config set value1=" + _MaxteacherSub + " where Id=4";
           m_MysqlDb.ExecuteQuery(sql);
           sql = "update tbltime_config set value1=" + _MaxStaffWeekperiod + " where Id=5";
           m_MysqlDb.ExecuteQuery(sql);
           sql = "update tbltime_config set value1=" + _MaxStaffDayperiod + " where Id=6";
           m_MysqlDb.ExecuteQuery(sql);
       }

       public DataSet GetDayIds(int _PeriodId, int _ClassId, string _Table)
       {
           string sql = "select distinct DayId from " + _Table + " where PeriodId=" + _PeriodId;
           if (_ClassId != -1)
           {
               sql = sql + " and ClassId=" + _ClassId;
           }
           m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           return m_MyDataSet;
       }

       public bool GetCheckSatusFor(int _PeriodId, int _ClassId, string _Table)
       {
           bool _valid = false;
           string sql = "select distinct NextPeriodBreak from " + _Table + " where PeriodId=" + _PeriodId;
           if (_ClassId != -1)
           {
               sql = sql + " and ClassId=" + _ClassId;
           }
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int BreakNext = int.Parse(m_MyReader.GetValue(0).ToString());
               if (BreakNext == 1)
               {
                   _valid = true;
               }
           }
           return _valid;
       }

       public void UpdateSchoolConfig(bool _MonDay, bool _TuesDay, bool _WednesDay, bool _ThursDay, bool _FriDay, bool _SaturDay, bool _Sunday, bool _NxtBrk, int _PeriodId, int IndexStatus)
       {
           int _NxtBreak = 0;
           if (_NxtBrk)
           {
               _NxtBreak = 1;
           }
           UpdateNextPeriod(_PeriodId, _NxtBreak);
           if (_MonDay)
           {

               if (!EntryExists(1, _PeriodId))
               {
                   InsertCLassEntry(1, _PeriodId, _NxtBreak);
               }

           }
           else
           {
               if (IndexStatus != 1)
                   DeleteClassEntry(1, _PeriodId);
           }

           //For TuesDay

           if (_TuesDay)
           {

               if (!EntryExists(2, _PeriodId))
               {
                   InsertCLassEntry(2, _PeriodId, _NxtBreak);
               }

           }
           else
           {
               if (IndexStatus != 1)
                   DeleteClassEntry(2, _PeriodId);
           }

           //For WednesDay

           if (_WednesDay)
           {

               if (!EntryExists(3, _PeriodId))
               {
                   InsertCLassEntry(3, _PeriodId, _NxtBreak);
               }

           }
           else
           {
               if (IndexStatus != 1)
                   DeleteClassEntry(3, _PeriodId);
           }

           //For ThursDay

           if (_ThursDay)
           {

               if (!EntryExists(4, _PeriodId))
               {
                   InsertCLassEntry(4, _PeriodId, _NxtBreak);
               }

           }
           else
           {
               if (IndexStatus != 1)
                   DeleteClassEntry(4, _PeriodId);
           }

           //For FriDay

           if (_FriDay)
           {

               if (!EntryExists(5, _PeriodId))
               {
                   InsertCLassEntry(5, _PeriodId, _NxtBreak);
               }

           }
           else
           {
               if (IndexStatus != 1)
                   DeleteClassEntry(5, _PeriodId);
           }

           //For SaturDay

           if (_SaturDay)
           {

               if (!EntryExists(6, _PeriodId))
               {
                   InsertCLassEntry(6, _PeriodId, _NxtBreak);
               }

           }
           else
           {
               if (IndexStatus != 1)

                   DeleteClassEntry(6, _PeriodId);
           }


           //For SunDay

           if (_Sunday)
           {

               if (!EntryExists(7, _PeriodId))
               {
                   InsertCLassEntry(7, _PeriodId, _NxtBreak);
               }

           }
           else
           {
               if (IndexStatus != 1)

                   DeleteClassEntry(7, _PeriodId);
           }
       }

       private void DeleteClassEntry(int _DayId, int _PeriodId)
       {
           string sql = "delete from tbltime_generalperiod where  DayId=" + _DayId + " and PeriodId = " + _PeriodId;
           m_MysqlDb.ExecuteQuery(sql);
       }

       private void InsertCLassEntry(int _DayId, int _PeriodId, int _NxtBreak)
       {
           string sql = "insert into tbltime_generalperiod (DayId,PeriodId,NextPeriodBreak) values (" + _DayId + ", " + _PeriodId + " ," + _NxtBreak + ")";
           m_MysqlDb.ExecuteQuery(sql);
       }

       private bool EntryExists(int _DayId, int _PeriodId)
       {
           bool _Valid = false;
           string sql = "select DayId from tbltime_generalperiod where DayId=" + _DayId + " and PeriodId=" + _PeriodId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               _Valid = true;
           }
           return _Valid;
       }

       private void UpdateNextPeriod(int _PeriodId, int _NxtBreak)
       {
           string sql = "update tbltime_generalperiod set NextPeriodBreak=" + _NxtBreak + " where  PeriodId=" + _PeriodId;
           m_MysqlDb.ExecuteQuery(sql);

       }

       public void DeleteClassEnrty(int _ClassId)
       {
           string sql = "delete from tbltime_classperiod where ClassId=" + _ClassId;
           m_MysqlDb.ExecuteQuery(sql);
       }

       public void UpdateClassConfig(bool _MonDay, bool _TuesDay, bool _WednesDay, bool _ThursDay, bool _FriDay, bool _SaturDay,bool _SunDay ,bool _NxtBrk, int _PeriodId, int _ClassId)
       {
           int _NxtBreak = 0;
           if (_NxtBrk)
           {
               _NxtBreak = 1;
           }
           UpdateNextPeriod(_PeriodId, _ClassId, _NxtBreak);
           //For MonDay
           if (_MonDay)
           {

               if (!EntryExists(1, _PeriodId, _ClassId))
               {
                   InsertCLassEntry(1, _PeriodId, _ClassId, _NxtBreak);
               }

           }
           else
           {
               DeleteClassEntry(1, _PeriodId, _ClassId);
           }

           //For TuesDay

           if (_TuesDay)
           {

               if (!EntryExists(2, _PeriodId, _ClassId))
               {
                   InsertCLassEntry(2, _PeriodId, _ClassId, _NxtBreak);
               }

           }
           else
           {
               DeleteClassEntry(2, _PeriodId, _ClassId);
           }

           //For WednesDay

           if (_WednesDay)
           {

               if (!EntryExists(3, _PeriodId, _ClassId))
               {
                   InsertCLassEntry(3, _PeriodId, _ClassId, _NxtBreak);
               }

           }
           else
           {
               DeleteClassEntry(3, _PeriodId, _ClassId);
           }

           //For ThursDay

           if (_ThursDay)
           {

               if (!EntryExists(4, _PeriodId, _ClassId))
               {
                   InsertCLassEntry(4, _PeriodId, _ClassId, _NxtBreak);
               }

           }
           else
           {
               DeleteClassEntry(4, _PeriodId, _ClassId);
           }

           //For FriDay

           if (_FriDay)
           {

               if (!EntryExists(5, _PeriodId, _ClassId))
               {
                   InsertCLassEntry(5, _PeriodId, _ClassId, _NxtBreak);
               }

           }
           else
           {
               DeleteClassEntry(5, _PeriodId, _ClassId);
           }

           //For SaturDay

           if (_SaturDay)
           {

               if (!EntryExists(6, _PeriodId, _ClassId))
               {
                   InsertCLassEntry(6, _PeriodId, _ClassId, _NxtBreak);
               }

           }
           else
           {
               DeleteClassEntry(6, _PeriodId, _ClassId);
           }

           //For SunDay

           if (_SunDay)
           {

               if (!EntryExists(7, _PeriodId, _ClassId))
               {
                   InsertCLassEntry(7, _PeriodId, _ClassId, _NxtBreak);
               }

           }
           else
           {
               DeleteClassEntry(7, _PeriodId, _ClassId);
           }
       }

       private void DeleteClassEntry(int _DayId, int _PeriodId, int _ClassId)
       {
           string sql = "delete from tbltime_classperiod where  DayId=" + _DayId + " and PeriodId = " + _PeriodId + " and ClassId=" + _ClassId + "";
           m_MysqlDb.ExecuteQuery(sql);
       }

       private void InsertCLassEntry(int _DayId, int _PeriodId, int _ClassId, int _NxtBreak)
       {
           string sql = "insert into tbltime_classperiod (DayId,PeriodId,NextPeriodBreak,ClassId) values (" + _DayId + ", " + _PeriodId + " ," + _NxtBreak + " , " + _ClassId + ")";
           m_MysqlDb.ExecuteQuery(sql);
       }

       private void UpdateNextPeriod(int _PeriodId, int _ClassId, int _NxtBrk)
       {

           string sql = "update tbltime_classperiod set NextPeriodBreak=" + _NxtBrk + " where  PeriodId=" + _PeriodId + " and ClassId=" + _ClassId + "";
           m_MysqlDb.ExecuteQuery(sql);
       }

       private bool EntryExists(int _DayId, int _PeriodId, int _ClassId)
       {
           bool _Valid = false;
           string sql = "select Id from tbltime_classperiod where DayId=" + _DayId + " and PeriodId=" + _PeriodId + " and ClassId=" + _ClassId + "";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               _Valid = true;
           }
           return _Valid;
       }

       public void DeleteAllClassEntries()
       {
           string sql = "delete from tbltime_classperiod";
           m_MysqlDb.ExecuteQuery(sql);
       }

       public void REstoreDefaultsToClass(int _ClassId)
       {
           string sql = "delete from tbltime_classperiod where ClassId="+_ClassId;
           m_MysqlDb.ExecuteQuery(sql);
            sql = "insert into tbltime_classperiod (DayId,PeriodId,NextPeriodBreak,ClassId) select *," + _ClassId + " from tbltime_generalperiod";
           m_MysqlDb.ExecuteQuery(sql);
           
       }



       public bool ValidSubjects(string _SubName, string _SubCode, out string Message)
       {
           Message = "";
           bool valid = true;
           if ((_SubName == "") && (_SubCode==""))
           {
               Message = "Please fill both subject name and subject code";
               return false;
           }
           string sql = "Select subject_name from tblsubjects where subject_name='" + _SubName + "'";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               Message = "Subject Already Exist";
               valid = false;
           }

           sql = "Select SubjectCode from tblsubjects where SubjectCode='" + _SubCode + "'";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               Message = "Subject Code Already Exist";
               valid = false;
           }
           return valid;
       }

       public int SaveSubGroup(string _Grpname, string _SubGrpAdjPeriods, string _SubGrpMaxPrd, string _SubGrpMinPrd)
       {
           int _SubGrpId = -1;
           string sql = "insert into tbltime_subgroup (Name,AdjPeriods,MaxPeriodWeek,MinPeriodWeek) values ('" + _Grpname + "'," + _SubGrpAdjPeriods + "," + _SubGrpMaxPrd + "," + _SubGrpMinPrd + ")";
           m_MysqlDb.ExecuteQuery(sql);
           sql = "select Id from tbltime_subgroup where Name='" + _Grpname + "' and AdjPeriods=" + _SubGrpAdjPeriods + "";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               _SubGrpId = int.Parse(m_MyReader.GetValue(0).ToString());
           }
           return _SubGrpId;
       }

       public bool SubGrpExists(string _Grpname)
       {
           bool valid = false;
           string sql = "select Id from tbltime_subgroup where Name='" + _Grpname + "'";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               valid = true;
           }
           return valid;
       }
       public void CreateSubject(string subName, string SubCode, int _SubGrp)
       {
           string sql = "INSERT INTO tblsubjects(subject_name,SubjectCode,sub_Catagory) VALUES ('" + subName + "','" + SubCode + "'," + _SubGrp + ")";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
       }

       public void UpdateSubGroup(string _GrpName, string _SubGrpAdjPeriods, string _SubGrpMaxPrd, string _SubGrpMinPrd, string _GrpId)
       {
           string sql = "update tbltime_subgroup set Name='" + _GrpName + "' , AdjPeriods=" + _SubGrpAdjPeriods + " , MaxPeriodWeek=" + _SubGrpMaxPrd + " , MinPeriodWeek= " + _SubGrpMinPrd + " where Id=" + _GrpId;
           m_MysqlDb.ExecuteQuery(sql);
       }

       public bool NewSubject(string _Subject)
       {
           bool valid = true;
           string sql = "select Id from tblsubjects where subject_name='" + _Subject + "'";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               valid = false;
           }
           return valid;
       }

       public void ChangeGroupForSubject(string _Subject, int _SubGrp)
       {
           string sql = "update tblsubjects set sub_Catagory=" + _SubGrp + " where subject_name='" + _Subject + "'";
           m_MysqlDb.ExecuteQuery(sql);
       }

       public void ChangeGroupForSubjectId(string _SubjectId, int _SubGrp)
       {
           string sql = "update tblsubjects set sub_Catagory=" + _SubGrp + " where Id='" + _SubjectId + "'";
           m_MysqlDb.ExecuteQuery(sql);
       }

       public bool SubjectmapedForClass(int _ClassId, string _SubjectId)
       {
           bool valid = false;
           string sql = "select SubjectId from tblclasssubmap where ClassId=" + _ClassId + " and SubjectId=" + _SubjectId + "";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               valid = true;
           }
           return valid;
       }

       public void AddSubjectstoClass(int _Classid, int _Subjectid)
       {
           string sql = "INSERT INTO tblclasssubmap(ClassId,SubjectId) VALUES (" + _Classid + "," + _Subjectid + ")";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
       }

       public void DeleteAllClassSubjects(int _Classid)
       {
           string sql = "delete from tblclasssubmap where ClassId="+_Classid;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
       }

       public string GetSubjectStringForStaff(int _StaffId)
       {
           string Subjects = "";
           string sql = "select tblsubjects.subject_name from tblstaffsubjectmap inner join tblsubjects on tblsubjects.Id = tblstaffsubjectmap.SubjectId where tblstaffsubjectmap.StaffId=" + _StaffId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               while (m_MyReader.Read())
               {
                   if (Subjects == "")
                   {
                       Subjects = Subjects + m_MyReader.GetValue(0).ToString();
                   }
                   else
                   {
                       Subjects = Subjects + " / " + m_MyReader.GetValue(0).ToString();
                   }
               }
           }
           else
           {
               Subjects = "None";
           }
           return Subjects;
       }

       public string GetClassStringForStaff(int _StaffId)
       {
           string _Class = "";
           string sql = "select tblclass.ClassName from tblclassstaffmap inner join tblclass on tblclass.Id = tblclassstaffmap.ClassId where tblclassstaffmap.StaffId=" + _StaffId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               while (m_MyReader.Read())
               {
                   if (_Class == "")
                   {
                       _Class = _Class + m_MyReader.GetValue(0).ToString();
                   }
                   else
                   {
                       _Class = _Class + " / " + m_MyReader.GetValue(0).ToString();
                   }
               }
           }
           else
           {
               _Class = "Any";
           }
           return _Class;
       }

       public bool IsStaffAvailabilityEntryExists(int StaffId)
       {
           bool valid = false;
           int Id=0;
           string sql = "select distinct DayId from tbltime_staffavailability where StaffId=" + StaffId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               if (int.TryParse(m_MyReader.GetValue(0).ToString(), out Id))
               {
                   if (Id > 0)
                   {
                       valid = true;
                   }
               }
           }
           return valid;
       }

       public DataSet GetStaffAvlDayIds_PeriodId(int _PeriodId, int _Staff)
       {
           string sql = "select distinct DayId from tbltime_staffavailability where PeriodId = " + _PeriodId + " and StaffId=" + _Staff + "";
           m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

 

           return m_MyDataSet;
       }

       public void UpdateStaffAvail(bool _MonDay, bool _TuesDay, bool _WednesDay, bool _ThursDay, bool _FriDay, bool _SaturDay, bool _SunDay, int _PeriodId, int _Staffid)
       {

           if (_MonDay)
           {

               if (!StaffEntryExists(1, _PeriodId, _Staffid))
               {
                   InsertStaffAvl(1, _PeriodId, _Staffid);
               }

           }
           else
           {
               DeleteStaffAvl(1, _PeriodId, _Staffid);
           }

           //For TuesDay

           if (_TuesDay)
           {

               if (!StaffEntryExists(2, _PeriodId, _Staffid))
               {
                   InsertStaffAvl(2, _PeriodId, _Staffid);
               }

           }
           else
           {
               DeleteStaffAvl(2, _PeriodId, _Staffid);
           }

           //For WednesDay

           if (_WednesDay)
           {

               if (!StaffEntryExists(3, _PeriodId, _Staffid))
               {
                   InsertStaffAvl(3, _PeriodId, _Staffid);
               }

           }
           else
           {
               DeleteStaffAvl(3, _PeriodId, _Staffid);
           }

           //For ThursDay

           if (_ThursDay)
           {

               if (!StaffEntryExists(4, _PeriodId, _Staffid))
               {
                   InsertStaffAvl(4, _PeriodId, _Staffid);
               }

           }
           else
           {
               DeleteStaffAvl(4, _PeriodId, _Staffid);
           }

           //For FriDay

           if (_FriDay)
           {

               if (!StaffEntryExists(5, _PeriodId, _Staffid))
               {
                   InsertStaffAvl(5, _PeriodId, _Staffid);
               }

           }
           else
           {
               DeleteStaffAvl(5, _PeriodId, _Staffid);
           }

           //For SaturDay

           if (_SaturDay)
           {

               if (!StaffEntryExists(6, _PeriodId, _Staffid))
               {
                   InsertStaffAvl(6, _PeriodId, _Staffid);
               }

           }
           else
           {
               DeleteStaffAvl(6, _PeriodId, _Staffid);
           }

           //For SunDay

           if (_SunDay)
           {

               if (!StaffEntryExists(7, _PeriodId, _Staffid))
               {
                   InsertStaffAvl(7, _PeriodId, _Staffid);
               }

           }
           else
           {
               DeleteStaffAvl(7, _PeriodId, _Staffid);
           }
       }

       private void DeleteStaffAvl(int _DayId, int _PeriodId, int _StaffId)
       {
           string sql = "delete from tbltime_staffavailability where DayId=" + _DayId + " and PeriodId=" + _PeriodId + " and StaffId=" + _StaffId + "";
           m_MysqlDb.ExecuteQuery(sql);
       }

       private void InsertStaffAvl(int _DayId, int _PeriodId, int _StaffId)
       {
           string sql = "insert into tbltime_staffavailability (DayId,PeriodId,StaffId) values (" + _DayId + " , " + _PeriodId + " ," + _StaffId + ")";
           m_MysqlDb.ExecuteQuery(sql);
       }

       private bool StaffEntryExists(int _DayId, int _PeriodId, int _StaffId)
       {
           bool _Valid = false;
           string sql = "select DayId from tbltime_staffavailability where DayId=" + _DayId + " and PeriodId=" + _PeriodId + " and StaffId=" + _StaffId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               _Valid = true;
           }
           return _Valid;
       }

       public bool ClassAssignedToStaff(int _StaffId, string _ClassId)
       {

           bool _Valid = false;
           string sql = "select StaffId from tblclassstaffmap where StaffId=" + _StaffId + " and ClassId=" + _ClassId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               _Valid = true;
           }
           return _Valid;
       }


       public string GetTotalClassAllocationPeriods(string _ClassId)
       {
           string TotPeriods = "0";
           string sql = "select count(tbltime_classperiod.PeriodId) from tbltime_classperiod where tbltime_classperiod.ClassId=" + _ClassId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               TotPeriods = m_MyReader.GetValue(0).ToString();
           }
           return TotPeriods;
       }

       public int GetTotalPeriodsForWeek()
       {
           int TotPeriods = 0;
           string sql = "select count(tbltime_generalperiod.PeriodId) from tbltime_generalperiod";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int.TryParse( m_MyReader.GetValue(0).ToString(),out TotPeriods);
           }
           return TotPeriods;
       }

       public void AssignClassToStaff(string _StaffId, string _ClassId)
       {
           string sql = "insert into tblclassstaffmap (StaffId,ClassId) values (" + _StaffId + " , " + _ClassId + ")";
           if (m_TransationDb != null)
           {
               m_TransationDb.ExecuteQuery(sql);
           }
           else
           {
               m_MysqlDb.ExecuteQuery(sql);
           }
       }



       public bool SubjectsAssignedToStaff(int _StaffId, string _SubjectId)
       {

           bool _Valid = false;
           string sql = "select StaffId from tblstaffsubjectmap where StaffId=" + _StaffId + " and SubjectId=" + _SubjectId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               _Valid = true;
           }
           return _Valid;
       }

       public void AssignSubjectsToStaff(string _StaffId, string _SubjectId)
       {
           string sql = "insert into tblstaffsubjectmap (StaffId,SubjectId) values (" + _StaffId + " , " + _SubjectId + ") ";
           m_MysqlDb.ExecuteQuery(sql);
       }

       public void RemoveSubjectFromStaff(string _StaffId, string _SubjectId)
       {
           string sql = "delete from tblstaffsubjectmap where StaffId=" + _StaffId + " and SubjectId =" + _SubjectId;
           if (m_TransationDb != null)
           {
               m_TransationDb.ExecuteQuery(sql);
           }
           else
           {
               m_MysqlDb.ExecuteQuery(sql);
           }
       }

       public DataSet GetGenStaffAvlDetails(int _PeriodId)
       {
           string sql = "select distinct DayId from tbltime_generalperiod where PeriodId = " + _PeriodId;
           m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           return m_MyDataSet;
       }

       public void UpdategenStaffConfig(bool _MonDay, bool _TuesDay, bool _WednesDay, bool _ThursDay, bool _FriDay, bool _SaturDay, bool _SunDay, int _PeriodId)
       {
           if (_MonDay)
           {

               if (!EntryGenStaffExists(1, _PeriodId))
               {
                   InsertGenStaffEntry(1, _PeriodId);
               }

           }
           else
           {
               DeletegenStaffEntry(1, _PeriodId);
           }

           //For TuesDay

           if (_TuesDay)
           {

               if (!EntryGenStaffExists(2, _PeriodId))
               {
                   InsertGenStaffEntry(2, _PeriodId);
               }

           }
           else
           {
               DeletegenStaffEntry(2, _PeriodId);
           }

           //For WednesDay

           if (_WednesDay)
           {

               if (!EntryGenStaffExists(3, _PeriodId))
               {
                   InsertGenStaffEntry(3, _PeriodId);
               }

           }
           else
           {
               DeletegenStaffEntry(3, _PeriodId);
           }

           //For ThursDay

           if (_ThursDay)
           {

               if (!EntryGenStaffExists(4, _PeriodId))
               {
                   InsertGenStaffEntry(4, _PeriodId);
               }

           }
           else
           {
               DeletegenStaffEntry(4, _PeriodId);
           }

           //For FriDay

           if (_FriDay)
           {
               if (!EntryGenStaffExists(5, _PeriodId))
               {
                   InsertGenStaffEntry(5, _PeriodId);
               }

           }
           else
           {
               DeletegenStaffEntry(5, _PeriodId);
           }

           //For SaturDay

           if (_SaturDay)
           {

               if (!EntryGenStaffExists(6, _PeriodId))
               {
                   InsertGenStaffEntry(6, _PeriodId);
               }

           }
           else
           {
               DeletegenStaffEntry(6, _PeriodId);
           }


           if (_SunDay)
           {

               if (!EntryGenStaffExists(7, _PeriodId))
               {
                   InsertGenStaffEntry(7, _PeriodId);
               }

           }
           else
           {
               DeletegenStaffEntry(7, _PeriodId);
           }


       }

       private void DeletegenStaffEntry(int _DayId, int _PeriodId)
       {
           string sql = "delete from tbltime_genstaffavlconfig where  DayId=" + _DayId + " and PeriodId = " + _PeriodId;
           m_MysqlDb.ExecuteQuery(sql);
       }

       private void InsertGenStaffEntry(int _DayId, int _PeriodId)
       {
           string sql = "insert into tbltime_genstaffavlconfig (DayId,PeriodId) values (" + _DayId + ", " + _PeriodId + ")";
           m_MysqlDb.ExecuteQuery(sql);
       }

       private bool EntryGenStaffExists(int _DayId, int _PeriodId)
       {
           bool _Valid = false;
           string sql = "select DayId from tbltime_generalperiod where DayId=" + _DayId + " and PeriodId=" + _PeriodId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               _Valid = true;
           }
           return _Valid;
       }

       public void DeleteStaffAvlDetails(int _StaffId)
       {
           string sql = "delete from tbltime_staffavailability ";
           if (_StaffId != -1)
           {
               sql = sql + "where StaffId=" + _StaffId;
           }
           m_MysqlDb.ExecuteQuery(sql);
       }

       public void ApplyDefltAvlToStaff(int _StaffId)
       {
           string sql = "insert into  tbltime_staffavailability (DayId,PeriodId,StaffId) select DayId,PeriodId," + _StaffId + " from tbltime_generalperiod";
           m_MysqlDb.ExecuteQuery(sql);
       }

       public bool ClassAllocatedToStaff(int _Classid, int _StaffId)
       {
           bool _Valid = false;
           string sql = "select StaffId from tblclassstaffmap where StaffId=" + _StaffId + " and ClassId=" + _Classid;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               _Valid = true;
           }
           return _Valid;
       }

       public void FillClassSubStaffEntry(int _ClassId, int _SatffId, int _SubjectId, int _MaxPeriod)
       {
           int ClassSubMapId = GetClassSubMapId(_ClassId, _SubjectId);
           InsertClassSubjectStaff(_MaxPeriod, ClassSubMapId, _SatffId);
       }

       private int GetClassSubMapId(int _ClassId, int _SubjectId)
       {
           int _ClassSubId = 0;
           string sql = "select Id from tblclasssubmap where ClassId=" + _ClassId + " and SubjectId=" + _SubjectId + "";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               _ClassSubId = int.Parse(m_MyReader.GetValue(0).ToString());
           }
           return _ClassSubId;
       }

       # region Specific Subject Staff

       public DataSet LoadTeacherForClassSub(int _Subject, int _ClassId)
       {
           DataSet Staff1 = new DataSet();
           string sql = "select tbluser.Id , tbluser.SurName from  tblclassstaffmap inner join tbluser on tbluser.Id =  tblclassstaffmap.StaffId inner join tblstaffsubjectmap on tblstaffsubjectmap.StaffId = tblclassstaffmap.StaffId where tblclassstaffmap.ClassId=" + _ClassId + " and tbluser.`status`=1 and tblstaffsubjectmap.SubjectId=" + _Subject;
           Staff1 = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           return Staff1;
       }

       public void InsertClassSubjectStaff(int _MaxPeriod, int _ClassSubMapId, int _Staff)
       {
           if (!ClasssSubStaffEntryExists(_ClassSubMapId, _Staff))
           {
               string sql = "insert into  tbltime_classsubjectstaff (ClassSubjectId,StaffId,PeriodCount) values(" + _ClassSubMapId + " , " + _Staff + " ," + _MaxPeriod + ")";
               m_MysqlDb.ExecuteQuery(sql);
           }
       }

       public int GetMinPeriod(int _SubjectId)
       {
           int Subgroup = GetSubGroup(_SubjectId);
           int MinPeriod = GetGroupMinPeriod(Subgroup);
           return MinPeriod;
       }

       private int GetGroupMinPeriod(int Subgroup)
       {
           int MaxPeriod = 0;
           string sql = "select MinPeriodWeek from tbltime_subgroup where Id=" + Subgroup;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               MaxPeriod = int.Parse(m_MyReader.GetValue(0).ToString());
           }
           return MaxPeriod;
       }


       public int GetMaxPeriod(int _SubjectId)
       {
           int Subgroup = GetSubGroup(_SubjectId);
           int MaxPeriod = GetGroupMaxPeriod(Subgroup);
           return MaxPeriod;
       }

       private int GetGroupMaxPeriod(int _Subgroup)
       {
           int MaxPeriod = 0;
           string sql = "select MaxPeriodWeek from tbltime_subgroup where Id=" + _Subgroup;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               MaxPeriod = int.Parse(m_MyReader.GetValue(0).ToString());
           }
           return MaxPeriod;
       }

       private int GetSubGroup(int _SubjectId)
       {
           int _Group = 0;
           OdbcDataReader MySubCatReader = null;
           string sql = "select sub_Catagory from tblsubjects where Id=" + _SubjectId;
           MySubCatReader = m_MysqlDb.ExecuteQuery(sql);
           if (MySubCatReader.HasRows)
           {
               _Group = int.Parse(MySubCatReader.GetValue(0).ToString());
           }
           MySubCatReader.Close();
           return _Group;
       }

       public bool ClasssSubStaffEntryExists(int _ClassSubMapId, int _StaffId)
       {
           bool _Valid = false;
           string sql = "select StaffId from tbltime_classsubjectstaff where StaffId=" + _StaffId + " and ClassSubjectId=" + _ClassSubMapId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               _Valid = true;
           }
           return _Valid;
       }

       public void RemoveClassSubjectMap(int ClassSubId,int _SubjectId, int _classid)
       {
           string sql = " DELETE from tblclasssubmap where Id=" + ClassSubId + " and tblclasssubmap.ClassId=" + _classid + " and tblclasssubmap.SubjectId= " + _SubjectId;
           m_MysqlDb.ExecuteQuery(sql);
       }


       public void DeleteClassSubjectStaff(int _ClassSubMapId)
       {
           string sql = "delete from tbltime_classsubjectstaff where ClassSubjectId=" + _ClassSubMapId;
           m_MysqlDb.ExecuteQuery(sql);
       }


       public void DeleteClassSubjectStaff_ParticularStaff(int _ClassSubMapId,int StaffID)
       {
           string sql = "delete from tbltime_classsubjectstaff where ClassSubjectId=" + _ClassSubMapId + " and StaffId=" + StaffID;
           if (m_TransationDb != null)
           {
               m_TransationDb.ExecuteQuery(sql);
           }
           else
           {
               m_MysqlDb.ExecuteQuery(sql);
           }
       }



       public void GetSelectedValues(int _ClassSubId, out int _Val1, out int _Val2)
       {

           _Val1 = 0;
           _Val2 = 0;
           int[] Staffs = new int[2];
           int i = 0;
           try
           {
               string sql = "select StaffId from tbltime_classsubjectstaff where  ClassSubjectId=" + _ClassSubId;
               m_MyReader = m_MysqlDb.ExecuteQuery(sql);
               if (m_MyReader.HasRows)
               {
                   while (m_MyReader.Read())
                   {
                       Staffs[i] = int.Parse(m_MyReader.GetValue(0).ToString());
                       i++;
                   }
               }
               _Val1 = Staffs[0];
               _Val2 = Staffs[1];
           }
           catch
           {

           }
       }

       public DataSet GetAllSubjects()
       {
           DataSet _TempData = new DataSet();
           DataTable Dt;
           DataRow Dr;
           DataSet SubjectsList = new DataSet();
         
           _TempData.Tables.Add(new DataTable("Subjects1"));
           Dt = _TempData.Tables["Subjects1"];
           Dt.Columns.Add("SubId");
           Dt.Columns.Add("Count");

           string sql = "select distinct tblsubjects.Id from tblstaffsubjectmap  inner join tblsubjects on tblstaffsubjectmap.SubjectId = tblsubjects.Id";
           SubjectsList = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           if (SubjectsList != null && SubjectsList.Tables != null && SubjectsList.Tables[0].Rows.Count > 0)
           {
               foreach (DataRow Dr_Sub in SubjectsList.Tables[0].Rows)
               {
                   Dr = _TempData.Tables["Subjects1"].NewRow();
                   Dr["SubId"] = Dr_Sub[0].ToString();
                   Dr["Count"] = GetStaffCountForSub(Dr_Sub[0].ToString());
                   _TempData.Tables["Subjects1"].Rows.Add(Dr);
               }
               // _TempData.Tables["Subjects1"].DefaultView.Sort = "Count DESC";
               DataView dataView = new DataView(_TempData.Tables[0]);
               dataView.Sort = "Count" + " " + "ASC";
              dataView.ToTable("Dt");
              
           }

           //m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           return _TempData;
       }

       private string GetStaffCountForSub(string _Subject)
       {
           string Count = "0";
           string sql = "select count(tblstaffsubjectmap.StaffId) from tblstaffsubjectmap where tblstaffsubjectmap.SubjectId=" + _Subject;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               Count = m_MyReader.GetValue(0).ToString();
               if ((Count == "") || (Count == null))
               {
                   Count = "0";
               }
           }
           return Count;
       }

       public DataSet GetAllClassForSubject(string _Subjects)
       {
           string sql = "select distinct ClassId from tblclasssubmap where SubjectId=" + _Subjects + " AND ClassId in (select DISTINCT(ClassId) from tbltime_classperiod) order by ClassId asc";
           m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           return m_MyDataSet;
       }

       public string CLassSubjectStaff(int _ClassSubjectID, out string _Staff2)
       {
           _Staff2 = "";
           string _Staff1 = "";
           string[] Staff = new string[2];
           Staff[0] = "";
           Staff[1] = "";
           int i = 0;
           string sql = "select distinct tbluser.Id from tbltime_classsubjectstaff inner join tbluser on tbluser.Id = tbltime_classsubjectstaff.StaffId where tbltime_classsubjectstaff.ClassSubjectId=" + _ClassSubjectID;
           //string sql = "select distinct tbluser.Id , tbluser.SurName from tbltime_classstaffmap inner join tbluser on tbluser.Id = tbltime_classstaffmap.StaffId where tbltime_classstaffmap.ClassId in (select tblclasssubmap.ClassId from tblclasssubmap where tblclasssubmap.SubjectId=" + _Subject + ") and tbltime_classstaffmap.ClassId=" + _Class + " and  tbluser.`status`=1";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               while (m_MyReader.Read())
               {
                   if (i == 2)
                   {
                       break;
                   }
                   else
                   {
                       Staff[i] = m_MyReader.GetValue(0).ToString();
                       i++;
                   }
               }
               _Staff1 = Staff[0];
               if ((Staff[1] != "") || (Staff[1] != null))
               {
                   _Staff2 = Staff[1];
               }
           }
           return _Staff1;
       }

       public DataSet GetStaffs()
       {
           string sql = "select distinct tbluser.Id,tbluser.SurName from tbluser inner join tblrole on tblrole.Id = tbluser.RoleId where tbluser.`Status`=1 and tblrole.Type='staff' ORDER BY tbluser.SurName";
           m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           return m_MyDataSet;
       }

       public string GetAllocationCount(string _StaffId)
       {
           string count = "0";
           string sql = "select count(ClassSubjectId) from tbltime_classsubjectstaff where StaffId=" + _StaffId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               count = m_MyReader.GetValue(0).ToString();
           }
           return count;
       }

       public string GetTeacherMaxPeriod()
       {
           string MaxPeriod = "0";
           string sql = "select value1 from tbltime_config where Id=5";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               MaxPeriod = m_MyReader.GetValue(0).ToString();
           }
           return MaxPeriod;
       }

       public string GetAllocatedPeriods(string _StaffId)
       {
           int AllotedPeriod = 0;
           string sql = "select PeriodCount from tbltime_classsubjectstaff where StaffId=" + _StaffId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               while (m_MyReader.Read())
               {
                   AllotedPeriod = AllotedPeriod + int.Parse(m_MyReader.GetValue(0).ToString());
               }
           }
           return AllotedPeriod.ToString();
       }

       public string GetAllotedStatus(string _Subject, string _Class, out int _ClassSubId)
       {
           string _Status = "0";
           _ClassSubId = 0;
           int ClassSubId = GetClassSubMapId(int.Parse(_Class), int.Parse(_Subject));
           _ClassSubId = ClassSubId;
           bool Valid = ClassSubAllocated(ClassSubId);
           if (Valid)
           {
               _Status = "1";

           }
           return _Status;
       }

       private bool ClassSubAllocated(int _ClassSubId)
       {
           bool valid = false;
           string sql = "select ClassSubjectId from tbltime_classsubjectstaff where ClassSubjectId=" + _ClassSubId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               valid = true;
           }
           return valid;
       }

       public string GetAllotedPeriodForStaff(int _ClassSubId, string _StaffId)
       {
           string _PeiodCount = "0";
           if (_StaffId != "")
           {
               string sql = "select PeriodCount from tbltime_classsubjectstaff where ClassSubjectId=" + _ClassSubId + " and StaffId= " + _StaffId + "";
               m_MyReader = m_MysqlDb.ExecuteQuery(sql);
               if (m_MyReader.HasRows)
               {
                   _PeiodCount = m_MyReader.GetValue(0).ToString();
               }
           }
           return _PeiodCount;
       }

       public bool SubjectHandlesbyStaff(string _SubjectId, string _StaffId)
       {
           bool _valid = false;
           string sql = "select StaffId from tblstaffsubjectmap where StaffId=" + _StaffId + " and SubjectId=" + _SubjectId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               _valid = true;
           }
           return _valid;
       }


       public int GetClassSubjectMap(string _SubjectId, string _Classid)
       {
           int Id = 0;
           string sql = "select Id from tblclasssubmap where ClassId=" + _Classid + " and SubjectId=" + _SubjectId;
           if (m_TransationDb != null)
           {
               m_MyReader = m_TransationDb.ExecuteQuery(sql);
           }
           else
           {
               m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           }
           if (m_MyReader.HasRows)
           {
               Id = int.Parse(m_MyReader.GetValue(0).ToString());
           }
           return Id;
       }

       public void UpdateMainTable(ref DataRow Dr_SpecStaff, ref DataSet StaffAllocation, ref DataRow Dr_Subject)
       {
           int Period1 = 0;
           int SubPeriods = 0;
           string staffId = Dr_SpecStaff[0].ToString();
           string SubjectPeriod = Dr_Subject[3].ToString();
           Dr_Subject["Staff1"] = staffId;
           string StaffPeriod = Dr_Subject[6].ToString();
           if ((StaffPeriod != "") && (SubjectPeriod != ""))
           {
               SubPeriods = int.Parse(SubjectPeriod);
               Period1 = int.Parse(StaffPeriod) + SubPeriods;
           }
           Dr_Subject["Period1"] = Period1.ToString();

       }

       public void UpdateStaffAndSpecificStaff(ref DataRow Dr_SpecStaff, ref DataSet StaffAllocation, string SubjectPeriod)
       {
           int Period1 = 0;
           int AllotedCount = 0;
           string AltCount = "";
           string AllocatedPeriods = "0";
           int AltPeriod = 0;
           int SubPeriods = 0;
           string staffId = Dr_SpecStaff[0].ToString();
           SubPeriods = int.Parse(SubjectPeriod);

           AltCount = Dr_SpecStaff[1].ToString();
           if (AltCount != "")
               AllotedCount = int.Parse(AltCount);
           AllotedCount = AllotedCount + 1;
           Dr_SpecStaff["AllocationCount"] = AllotedCount;

           AllocatedPeriods = Dr_SpecStaff[3].ToString();
           if (AllocatedPeriods != "")
               AltPeriod = int.Parse(AllocatedPeriods);
           AltPeriod = AltPeriod + SubPeriods;
           Dr_SpecStaff["AllocatedPeriods"] = AltPeriod;


           FillAllStaffTable(AllotedCount, AltPeriod, staffId, ref StaffAllocation);
       }

       private void FillAllStaffTable(int AllotedCount, int AltPeriod, string staffId, ref DataSet StaffAllocation)
       {

           foreach (DataRow Dr_Staff in StaffAllocation.Tables[1].Rows)
           {
               if (Dr_Staff[0].ToString() == staffId)
               {
                   Dr_Staff["AllocationCount"] = AllotedCount;
                   Dr_Staff["AllocatedPeriods"] = AltPeriod;
               }
           }
       }

       public void UpdateMainTable(DataRow Dr_Subject, string _StaffId, int RemainingStaffPeriods, int Staff)
       {
           if (Staff == 1)
           {
               Dr_Subject["Staff1"] = _StaffId;
               Dr_Subject["Period1"] = RemainingStaffPeriods.ToString();
           }
           else
           {
               Dr_Subject["Staff2"] = _StaffId;
               Dr_Subject["Period2"] = RemainingStaffPeriods.ToString();
           }
       }

       public string GetTimeClassReport(int _UserId)
       {
           DataSet MyclassSub = new DataSet();
           DataSet StaffDetails = new DataSet();
           StringBuilder Report = new StringBuilder();
           Report.Append("<table width=\"60%\"><th>Name</th> <th>Staff</th> <th>AllottedPeriods</th> <th>Remaining</th>");
           m_MyDataSet = GetAllClassList(_UserId);
           if (m_MyDataSet != null && m_MyDataSet.Tables != null && m_MyDataSet.Tables[0].Rows.Count > 0)
           {
               foreach (DataRow Dr_Class in m_MyDataSet.Tables[0].Rows)
               {
                   MyclassSub = GetClassSubjects(Dr_Class[0].ToString());
                   Report.Append("<tr><td colspan=\"4\" style=\"border-bottom-color: Black; border-style: solid; border-width: thin; border-color: inherit;\">" + Dr_Class[1].ToString() + "</td></tr>");
                   if (MyclassSub != null && MyclassSub.Tables != null && MyclassSub.Tables[0].Rows.Count > 0)
                   {
                       foreach (DataRow Dr_Subject in MyclassSub.Tables[0].Rows)
                       {
                           Report.Append("<tr>");
                           StaffDetails = GetStaffDetails(Dr_Subject[2].ToString());
                           if (StaffDetails != null && StaffDetails.Tables != null && StaffDetails.Tables[0].Rows.Count > 0)
                           {
                               foreach (DataRow Dr_Staff in StaffDetails.Tables[0].Rows)
                               {

                                   Report.Append("<td align=\"center\"> " + Dr_Subject[1].ToString() + " </td> <td align=\"center\">" + Dr_Staff[0].ToString() + "</td> <td align=\"center\">" + Dr_Staff[1].ToString() + "</td>");

                               }
                           }
                           Report.Append("</tr>");

                       }
                   }
                   Report.Append("<tr><td colspan=\"4\"> &nbsp;</td></tr>");
               }
           }
           Report.Append("</table>");
           return Report.ToString();
       }

       private DataSet GetStaffDetails(string _ClassSubMapId)
       {
           string sql = "select tbluser.SurName , tbltime_classsubjectstaff.PeriodCount from tbltime_classsubjectstaff inner join tbluser on tbluser.Id = tbltime_classsubjectstaff.StaffId where tbltime_classsubjectstaff.ClassSubjectId=" + _ClassSubMapId;
           m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           return m_MyDataSet;
       }

       private DataSet GetClassSubjects(string _ClassId)
       {
           string sql = "select distinct tblsubjects.Id, tblsubjects.subject_name , tblclasssubmap.Id from tblclasssubmap inner join tblsubjects on tblsubjects.Id = tblclasssubmap.SubjectId where tblclasssubmap.ClassId=" + _ClassId;
           m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           return m_MyDataSet;
       }

       public DataSet GetAllClassList(int _UserId)
       {
           string sql = "SELECT tblclass.Id,tblclass.ClassName from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Id in (SELECT DISTINCT tbltime_classperiod.ClassId FROM tbltime_classperiod) AND tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _UserId + ") ORDER BY tblstandard.Id,tblclass.ClassName";
           m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           return m_MyDataSet;
       }

       # endregion




       # region Verify

       public void SetClassTeacherPeriod()
       {
           DataSet ClassId = new DataSet();
           DataSet DayId = new DataSet();
           int DayClassPeriodid = 0;
           int ClassTeacherid = 0;
           int SubjectId = 0;
           if (FirstPeriodClassteacher())
           {
               ClassId = GetClassIds();
               if (ClassId != null && ClassId.Tables != null && ClassId.Tables[0].Rows.Count > 0)
               {
                   foreach (DataRow Dr_Classid in ClassId.Tables[0].Rows)
                   {
                       ClassTeacherid = GetClassTeacher(Dr_Classid[0].ToString());
                       SubjectId = GetClassTeacherSubjectId(ClassTeacherid, Dr_Classid[0].ToString());
                       DayId = GetDayIds(Dr_Classid[0].ToString());
                       if (DayId != null && DayId.Tables != null && DayId.Tables[0].Rows.Count > 0)
                       {
                           foreach (DataRow Dr_DayId in DayId.Tables[0].Rows)
                           {
                               DayClassPeriodid = GetDayClassPeriodId(Dr_Classid[0].ToString(), Dr_DayId[0].ToString());
                               InsertTimeTableMasterData(DayClassPeriodid, ClassTeacherid, SubjectId);
                           }
                       }
                   }
               }
           }
       }

       private void InsertTimeTableMasterData(int _DayClassPeriodid, int _ClassTeacherid, int _SubjectId)
       {
           string sql = "insert into tbltime_master (ClassPeriodId,StaffId,SubjectId) values(" + _DayClassPeriodid + " , " + _ClassTeacherid + " , " + _SubjectId + ")";
           m_TransationDb.ExecuteQuery(sql);
       }

       private int GetClassTeacherSubjectId(int _ClassTeacherid, string _ClassId)
       {
           int ClsTSubId = 0;
           bool valid;
           string sql = "select SubjectId from tblstaffsubjectmap where StaffId = (select distinct StaffId from tblclassstaffmap where tblclassstaffmap.StaffId=" + _ClassTeacherid + " and tblclassstaffmap.ClassId=" + _ClassId + ")";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               valid = int.TryParse(m_MyReader.GetValue(0).ToString(), out ClsTSubId);
           }
           return ClsTSubId;
       }

       private int GetClassTeacher(string _Classid)
       {
           int DayClsPId = 0;
           bool valid;
           string sql = "select tblclass.ClassTeacher from tblclass where tblclass.Id =" + _Classid;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               valid = int.TryParse(m_MyReader.GetValue(0).ToString(), out DayClsPId);
           }
           return DayClsPId;
       }

       private int GetDayClassPeriodId(string _ClassId, string _DayId)
       {
           int DayClsPId = 0;
           string sql = "select tbltime_classperiod.PeriodId from tbltime_classperiod inner join tblattendanceperiod on tblattendanceperiod.PeriodId = tbltime_classperiod.PeriodId where tblattendanceperiod.ModeId=3 and  tblattendanceperiod.count=1 and tbltime_classperiod.ClassId=" + _ClassId + " and tbltime_classperiod.DayId=" + _DayId;
           m_MyReader = m_TransationDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               DayClsPId = int.Parse(m_MyReader.GetValue(0).ToString());
           }
           return DayClsPId;
       }

       private DataSet GetDayIds(string _ClassId)
       {
           string sql = "select distinct DayId from tbltime_classperiod where ClassId=" + _ClassId;
           m_MyDataSet = m_TransationDb.ExecuteQueryReturnDataSet(sql);
           return m_MyDataSet;
       }

       private DataSet GetClassIds()
       {
           string sql = "select distinct ClassId from tbltime_classperiod";
           m_MyDataSet = m_TransationDb.ExecuteQueryReturnDataSet(sql);
           return m_MyDataSet;
       }




       //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

       public void FillTimeTableGenEntry(ref DataSet TimeTable)
       {
           DataRow dr;
           string sql = "Select tbltime_classperiod.PeriodId , tbltime_classperiod.DayId , tbltime_classperiod.ClassId , NextPeriodBreak , Id from tbltime_classperiod order by tbltime_classperiod.PeriodId asc , tbltime_classperiod.ClassId ASC";
           m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           if (m_MyDataSet != null && m_MyDataSet.Tables != null && m_MyDataSet.Tables[0].Rows.Count > 0)
           {
               foreach (DataRow Dr_Period in m_MyDataSet.Tables[0].Rows)
               {
                   dr = TimeTable.Tables["TimeMaster"].NewRow();
                   dr["Alloted"] = "0";
                   dr["ClassPeriodId"] = Dr_Period[4].ToString();
                   dr["Period"] = Dr_Period[0].ToString();
                   dr["Day"] = Dr_Period[1].ToString();
                   dr["Class"] = Dr_Period[2].ToString();
                   dr["NxtBrk"] = Dr_Period[3].ToString();
                   dr["Staff"] = "0";
                   dr["Subject"] = "0";
                   TimeTable.Tables["TimeMaster"].Rows.Add(dr);
               }
           }
       }


       public void FillClassTeacher(ref DataSet TimeTable, DataSet MyClass)
       {
           int ClassTeacher = 0;
           int ClassTeacherSubject = 0;
           if (MyClass != null && MyClass.Tables != null && MyClass.Tables[0].Rows.Count > 0)
           {

               foreach (DataRow Dr_Class in MyClass.Tables[0].Rows)
               {
                   foreach (DataRow Dr_Period in TimeTable.Tables[0].Rows)
                   {
                       if ((Dr_Period["Class"].ToString() == Dr_Class[0].ToString()) && (Dr_Period["Period"].ToString() == "4")) // Get 4 from DataBase
                       {

                           ClassTeacher = GetClassTeacher(Dr_Class[0].ToString());
                           ClassTeacherSubject = GetClassTeacherSubjectId(ClassTeacher, Dr_Class[0].ToString());
                           Dr_Period["Staff"] = ClassTeacher.ToString();
                           Dr_Period["Subject"] = ClassTeacherSubject.ToString();
                           Dr_Period["Alloted"] = "1";
                       }
                   }
               }
           }
       }

       public void FillAdjacentPeriodMoreSubjects(ref DataSet TimeTable, DataSet MyClass)
       {
           string _PreClass = "0";
           int ClassMaxPEriod = 0;
           int PeriodPerWeek = 0;
           int i;
           int j, k, p = 0, q;
           int AdjPeriod = 0;
           DataSet Subject = new DataSet();
           DataSet AdjSubStaff = new DataSet();
           DataRow Dr_Staff;
           if (TimeTable != null && TimeTable.Tables != null && TimeTable.Tables[0].Rows.Count > 0)
           {
               if (MyClass != null && MyClass.Tables != null && MyClass.Tables[0].Rows.Count > 0)
               {
                   foreach (DataRow Dr_Claaa in MyClass.Tables[0].Rows)
                   {
                       int[] Day = new int[GetMaximumCountOfDays(Dr_Claaa[0].ToString())];
                       foreach (DataRow Dr_Period in TimeTable.Tables[0].Rows)
                       {
                           if ((Dr_Claaa[0].ToString() == Dr_Period["Class"].ToString()) && (_PreClass != Dr_Period["Class"].ToString()))
                           {
                               _PreClass = Dr_Period["Class"].ToString();
                               Subject = GetAdjacentPeriodMoreSub(Dr_Claaa[0].ToString());
                               if (Subject != null && Subject.Tables != null && Subject.Tables[0].Rows.Count > 0)
                               {
                                   foreach (DataRow Dr_Subject in Subject.Tables[0].Rows)
                                   {
                                       AdjPeriod = int.Parse(Dr_Subject[1].ToString());
                                       PeriodPerWeek = int.Parse(Dr_Subject[2].ToString());
                                       AdjSubStaff = GetStaffForSubject(Dr_Subject[0].ToString(), Dr_Claaa[0].ToString()); // sort by staff having more periods
                                       if (AdjSubStaff != null && AdjSubStaff.Tables != null && AdjSubStaff.Tables[0].Rows.Count > 0)
                                       {
                                           Dr_Staff = AdjSubStaff.Tables[0].Rows[0];
                                           BuildDayArray(Dr_Claaa[0].ToString(), ref Day);
                                           for (i = 0, p = 0; i < Day.Length && p < PeriodPerWeek; i++, p++) // Execute loop for maximum subject period per week
                                           {
                                               ClassMaxPEriod = GetPeriodForClassDay(Dr_Claaa[0].ToString(), Day[i]);
                                               int[,] Period = new int[ClassMaxPEriod - 1, 3];
                                               BuildPeriodArray(ref TimeTable, Dr_Claaa[0].ToString(), Day[i], ref Period, Day.Length);
                                               for (j = 0; j < ClassMaxPEriod; j++)
                                               {
                                                   if (ValidPeriod(ref Period, j, AdjPeriod))
                                                   {
                                                       q = j; // Call the function till the adjperiod count
                                                       k = 0;
                                                       do
                                                       {
                                                           UpDateMasterTimeTable(Dr_Subject[0].ToString(), Dr_Staff[0].ToString(), Period[q, 0], ref TimeTable);
                                                           q++;//inseting nxt period
                                                           k++;// counting to adjperiod
                                                       }
                                                       while ((k < AdjPeriod) && ((q + AdjPeriod) < ClassMaxPEriod));

                                                       break;

                                                   }
                                               }
                                           }
                                       }
                                   }
                               }
                           }
                       }
                   }
               }
           }
       }

       private int GetPeriodForClassDay(string _Class, int _Day)
       {
           //
           int Count = 0;
           bool valid;
           string sql = "select  count(distinct tbltime_classperiod.Id) from tbltime_classperiod where tbltime_classperiod.ClassId=" + _Class + " and tbltime_classperiod.DayId=" + _Day;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               valid = int.TryParse(m_MyReader.GetValue(0).ToString(), out Count);
           }
           return Count;
       }

       private int GetMaximumCountOfDays(string _ClassId)
       {
           int Count = 0;
           bool valid;
           string sql = "select count(distinct tbltime_classperiod.DayId) from tbltime_classperiod where tbltime_classperiod.ClassId=" + _ClassId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               valid = int.TryParse(m_MyReader.GetValue(0).ToString(), out Count);
           }
           return Count;
       }

       private void UpDateMasterTimeTable(string _Subject, string Staff, int _ClassPeriodId, ref DataSet TimeTable)
       {
           foreach (DataRow Dr_Period in TimeTable.Tables[0].Rows)
           {
               if ((Dr_Period["ClassPeriodId"].ToString() == _ClassPeriodId.ToString()) && (Dr_Period["Alloted"].ToString() != "1"))
               {
                   Dr_Period["Staff"] = Staff;
                   Dr_Period["Subject"] = _Subject;
                   Dr_Period["Alloted"] = "1";
               }
           }
       }


       private bool ValidPeriod(ref int[,] Period, int Index, int _NoAdjprd)
       {
           bool valid = false;
           int j = 2, k = 1, count = 0;

           for (int a = Index; a < Period.Length; a++)
           {
               do
               {
                   if (valid)
                   {
                       a++;
                   }
                   if (Period[a, j] == 0)
                   {
                       valid = true;
                       count++;
                       if ((_NoAdjprd == 2) && (count == 1))
                           return valid;
                       if ((_NoAdjprd == 3) && (count == 2))
                           return valid;
                   }
                   else
                   {
                       valid = false;
                       return valid;
                   }
                   k++;
               }
               while (k < _NoAdjprd);
           }
           return valid;
       }

       private void BuildPeriodArray(ref DataSet TimeTable, string _Class, int _DayId, ref int[,] Period, int _Length)
       {
           DataSet ClassPeriod = new DataSet();
           DataTable dt;
           DataRow dr;
           ClassPeriod.Tables.Add(new DataTable("Periods"));
           dt = ClassPeriod.Tables["Periods"];
           dt.Columns.Add("Alloted");
           dt.Columns.Add("ClassPeriodId");
           dt.Columns.Add("Period");
           dt.Columns.Add("NxtBrk");
           DataRow Dr_ClassPeriod;

           int i = 0, j = 0, k = 0;
           string sql = "select distinct tbltime_classperiod.Id, tbltime_classperiod.PeriodId, tbltime_classperiod.NextPeriodBreak   from tbltime_classperiod where tbltime_classperiod.ClassId=" + _Class + " and tbltime_classperiod.DayId=" + _DayId;
           // ClassPeriod = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           foreach (DataRow Dr_Period in TimeTable.Tables[0].Rows)
           {
               if ((Dr_Period["Class"].ToString() == _Class) && (Dr_Period["Day"].ToString() == _DayId.ToString()) && (Dr_Period["Alloted"].ToString() != "1"))
               {
                   dr = ClassPeriod.Tables["Periods"].NewRow();

                   dr["Alloted"] = Dr_Period[0].ToString();
                   dr["ClassPeriodId"] = Dr_Period[1].ToString();
                   dr["Period"] = Dr_Period[2].ToString();
                   dr["NxtBrk"] = Dr_Period[7].ToString();
                   ClassPeriod.Tables["Periods"].Rows.Add(dr);
               }
           }
           if (ClassPeriod != null && ClassPeriod.Tables != null && ClassPeriod.Tables[0].Rows.Count > 1)
           {
               for (k = 0, i = 0; k < ClassPeriod.Tables[0].Rows.Count && i < _Length; k++, i++)
               {
                   Dr_ClassPeriod = ClassPeriod.Tables[0].Rows[k];
                   for (j = 0; j < 3; j++)
                   {
                       if (j == 0)
                       {
                           Period[i, j] = int.Parse(Dr_ClassPeriod[1].ToString());
                       }
                       else if (j == 1)
                       {
                           Period[i, j] = int.Parse(Dr_ClassPeriod[2].ToString());
                       }
                       else
                       {
                           Period[i, j] = int.Parse(Dr_ClassPeriod[3].ToString());
                       }
                   }
               }
           }

       }


       private void BuildDayArray(string _Class, ref int[] Day)
       {
           int i = 0;
           string sql = "select distinct tbltime_classperiod.DayId from tbltime_classperiod where tbltime_classperiod.ClassId=" + _Class;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               while (m_MyReader.Read())
               {

                   Day[i] = int.Parse(m_MyReader.GetValue(0).ToString());
                   i++;

               }
           }
       }

       //public void FillAdjacentPeriodMoreSubjects(ref DataSet TimeTable, DataSet MyClass)
       //{
       //    TimeTable.Tables.Add(new DataTable ("ClassPeriod"));
       //    DataTable dt;

       //    dt = TimeTable.Tables["ClassPeriod"];
       //    dt.Columns.Add("Alloted");
       //    dt.Columns.Add("ClassPeriodId");
       //    dt.Columns.Add("Class");
       //    dt.Columns.Add("Day");
       //    dt.Columns.Add("Period");
       //    dt.Columns.Add("Staff");
       //    dt.Columns.Add("Subject");
       //    dt.Columns.Add("NxtBrk");
       //    string _PreviousClass = "0";
       //        foreach (DataRow Dr_Period in TimeTable.Tables[0].Rows)
       //        {
       //            if ((Dr_Period["Alloted"].ToString() != "1") && (_PreviousClass != Dr_Period["Class"].ToString()))
       //            {
       //                _PreviousClass = Dr_Period["Class"].ToString();
       //                GenerateClassWiseTable(ref TimeTable, _PreviousClass);
       //                AllocateStaffsForAdjSubjects(ref TimeTable, _PreviousClass);
       //                break;
       //            }

       //        }
       // }

       //private void AllocateStaffsForAdjSubjects(ref DataSet TimeTable , string _Class)
       //{
       //    DataSet SubjectStaff = new DataSet();
       //    int MaxPeriod = 0;
       //    int AdjPeriod = 0;
       //    int i=0;
       //    int j = 0;
       //    string PreDay = "";
       //    string AltFlag = "0";
       //    string sql = "select tblsubjects.Id , tbltime_subgroup.AdjPeriods , tbltime_subgroup.MaxPeriodWeek, tblstaffsubjectmap.StaffId from tblclasssubmap inner join tblsubjects on tblsubjects.Id = tblclasssubmap.SubjectId inner join tbltime_subgroup on tbltime_subgroup.Id = tblsubjects.sub_Catagory inner join tblstaffsubjectmap on tblstaffsubjectmap.SubjectId= tblclasssubmap.SubjectId where tblclasssubmap.ClassId="+_Class+" and tbltime_subgroup.AdjPeriods>1";
       //    SubjectStaff = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
       //    if (SubjectStaff != null && SubjectStaff.Tables != null && SubjectStaff.Tables[0].Rows.Count>0)
       //    {   
       //        foreach (DataRow Dr_SubStaff in SubjectStaff.Tables[0].Rows)
       //        {
       //            MaxPeriod = int.Parse(Dr_SubStaff[2].ToString());
       //            AdjPeriod = int.Parse(Dr_SubStaff[1].ToString());
       //            if(TimeTable.Tables[1] !=null && TimeTable.Tables[1].Rows.Count>0)
       //            {
       //                i = 0; j = 0;
       //                foreach (DataRow Dr_ClassPeriod in TimeTable.Tables[1].Rows)
       //                {

       //                    if ((i < MaxPeriod) && (j < AdjPeriod))
       //                    {

       //                        // if not alloted , not nxt prd brk or previously alloted  , not same day
       //                        if ((Dr_ClassPeriod[0].ToString() != "1") && ((Dr_ClassPeriod[5].ToString() != "1") || (AltFlag == "1")) && (PreDay != Dr_ClassPeriod[3].ToString()))
       //                        {
       //                            PreDay = Dr_ClassPeriod[3].ToString();
       //                            Dr_ClassPeriod["Staff"] = Dr_SubStaff[3].ToString();
       //                            Dr_ClassPeriod["Subject"] = Dr_SubStaff[1].ToString();
       //                            Dr_ClassPeriod["Alloted"] = "1";
       //                            j++;
       //                            if()
       //                            {

       //                            }
       //                            AltFlag = "1";
       //                        }
       //                        else
       //                        {
       //                            AltFlag = "0";
       //                        }
       //                    }
       //                }
       //            }
       //        }
       //    }
       //}

       //private void GenerateClassWiseTable(ref DataSet TimeTable , string _PerviousClass)
       //{
       //     DataRow dr;
       //    foreach (DataRow Dr_Period in TimeTable.Tables["TimeMaster"].Rows)
       //    {
       //        if (_PerviousClass == Dr_Period["Class"].ToString())
       //         {
       //              dr = TimeTable.Tables["ClassPeriod"].NewRow();

       //              dr["Alloted"] = Dr_Period[0].ToString();
       //              dr["ClassPeriodId"] = Dr_Period[1].ToString();
       //              dr["Period"] = Dr_Period[2].ToString();
       //              dr["Day"] = Dr_Period[3].ToString();
       //              dr["Class"] = Dr_Period[4].ToString();
       //              dr["NxtBrk"] = Dr_Period[7].ToString();
       //              dr["Staff"] = "0";
       //              dr["Subject"] = "0";

       //              TimeTable.Tables["ClassPeriod"].Rows.Add(dr);
       //         }


       //    }
       //}




       private DataSet GetStaffForSubject(string _SubjectId, string _ClassId)
       {
           string sql = "select tbltime_classsubjectstaff.StaffId , tbltime_classsubjectstaff.PeriodCount from tbltime_classsubjectstaff inner join tblclasssubmap on tblclasssubmap.Id = tbltime_classsubjectstaff.ClassSubjectId where tblclasssubmap.ClassId=" + _ClassId + " and tblclasssubmap.SubjectId=" + _SubjectId;
           m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           return m_MyDataSet;
       }


       private DataSet GetAdjacentPeriodMoreSub(string _ClassId)
       {
           string sql = "select tblsubjects.Id , tbltime_subgroup.AdjPeriods , tbltime_subgroup.MaxPeriodWeek from tblclasssubmap inner join tblsubjects on tblsubjects.Id = tblclasssubmap.SubjectId inner join tbltime_subgroup on tbltime_subgroup.Id = tblsubjects.sub_Catagory  where tblclasssubmap.ClassId=" + _ClassId + " and tbltime_subgroup.AdjPeriods>1";
           m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           return m_MyDataSet;
       }

       public bool FirstPeriodClassteacher()
       {

           bool _valid = false;
           string sql = "select value1 from tbltime_config where Id=2";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               if (m_MyReader.GetValue(0).ToString() == "1")
               {
                   _valid = true;
               }
           }
           return _valid;
       }

       public int SetNumberofPeriods(int _RemainingStaffPeriods, int _MaxSubPeriod)
       {
           int _Periods = _RemainingStaffPeriods;
           for (int i = 0; i < _RemainingStaffPeriods;i++ )
           {
               if (_Periods > _MaxSubPeriod)
               {
                   _Periods = _RemainingStaffPeriods - 1;
               }
               else
               {
                   _Periods = _RemainingStaffPeriods;
               }
           }
           return _Periods;
       }

       public DataSet GetMyClass()
       {
           string sql = "select distinct ClassId from tbltime_classperiod order by ClassId";
           m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           return m_MyDataSet;
       }


       public void FillCombinedSubjects(ref DataSet TimeTable, DataSet MyClass)
       {
           DataSet Standard = new DataSet();
           DataSet CombinedSubjects = new DataSet();
           DataSet CombSubClass = new DataSet();
           string sql = "select tblstandard.Id from tblclass inner join tblstandard on tblclass.Standard = tblstandard.Id";
           Standard = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

           if (Standard != null && Standard.Tables != null && Standard.Tables[0].Rows.Count > 0)
           {
               foreach (DataRow Dr_Standard in Standard.Tables[0].Rows)
               {
                   sql = "select distinct tblsubjects.Id from tblclasssubmap inner join  tblsubjects on tblsubjects.Id = tblclasssubmap.SubjectId inner join tblclass on tblclass.Id = tblclasssubmap.ClassId  where tblsubjects.Combined=1 AND tblclass.Standard=" + Dr_Standard[0].ToString();
                   CombinedSubjects = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                   if (CombinedSubjects != null && CombinedSubjects.Tables != null && CombinedSubjects.Tables[0].Rows.Count > 0)
                   {
                       foreach (DataRow Dr_CombSub in CombinedSubjects.Tables[0].Rows)
                       {
                           CombSubClass = GetCombSubClasses(Dr_CombSub[0].ToString(), Dr_Standard[0].ToString());
                           if (CombSubClass != null && CombSubClass.Tables != null && CombSubClass.Tables[0].Rows.Count > 1)
                           {

                           }
                       }
                   }
               }
           }
       }

       private DataSet GetCombSubClasses(string _Subject, string _Standard)
       {
           string sql = "select distinct tblclasssubmap.ClassId from tblsubjects inner join  tblclasssubmap on tblsubjects.Id = tblclasssubmap.SubjectId inner join tblclass on tblclass.Id = tblclasssubmap.ClassId where tblsubjects.Id=" + _Subject + " and tblclass.Standard=" + _Standard;
           m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           return m_MyDataSet;
       }

       

       # endregion


       # region Timtable Alocation

       public bool GenerateTimtable(out string _Msg)
       {
           bool _Valid = false;
           _Msg = "";
           try
           {
              
               InitiliseMasterDatas();
               DiableInvalidPeriods();
               if (DoAllocation())
               {
                   UpDateDataBase();
                   _Valid = true;
                   _Msg = "Time table generated successfully";
               }
               else
               {
                   _Valid = false;
                   _Msg = "Cannot generate timetable. Please verify the data provided";
               }
           }
           catch
           {
               _Valid= false;
               _Msg = "Unable to generate timetable. Please try again";
           }
           return _Valid;

       }

       private void UpDateDataBase()
       {
           int ClassPeroidId = 0;
          Empty_UnFixedTimeMasterTable();
           int _ClassIndex, _DayIndex, _PeriodIndex;
            for (_ClassIndex = 0; _ClassIndex < M_MaxClassCount; _ClassIndex++)
               for (_DayIndex = 0; _DayIndex < M_MaxDayCount; _DayIndex++)
                   for (_PeriodIndex = 0; _PeriodIndex < M_MaxPeriodCount; _PeriodIndex++)
                   {
                       if (m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].IsAllocated == true && m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].IsFixed==false)
                       {
                           if (GetClassPeriodId(M_ClassId[_ClassIndex], M_DayId[_DayIndex], M_PeriodId[_PeriodIndex], out ClassPeroidId))
                           {
                               InserTimeMasterData(ClassPeroidId, m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].StaffId, m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].SubjectId,0);
                           }
                       }
                   }
       
       }

       private void InserTimeMasterData(int _ClassPeroidId, int _StaffId, int _SubjectId , int _Fixed)
       {
           string sql = "insert into tbltime_master (ClassPeriodId,StaffId,SubjectId,Fixed) values (" + _ClassPeroidId + " , " + _StaffId + " , " + _SubjectId + "," + _Fixed + ")";
           m_MysqlDb.ExecuteQuery(sql);
       }

       private bool GetClassPeriodId(int _ClassId, int _DayId, int _PeriodId, out int ClassPeroidId)
       {
           bool _valid = false;
           ClassPeroidId = 0;
           string sql = "select Id from tbltime_classperiod where ClassId=" + _ClassId + " and DayId=" + _DayId + " and PeriodId=" + _PeriodId + "";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               ClassPeroidId = int.Parse(m_MyReader.GetValue(0).ToString());
               _valid = true;
           }
           return _valid;               
       }

       private void Empty_UnFixedTimeMasterTable()
       {
           string sql = "delete from tbltime_master where Fixed=0";
           m_MysqlDb.ExecuteQuery(sql);
       }

    
       private bool DoAllocation()
       {
           bool _Valid = false;
           int _Return = RecusivePeriodAllocation(0, 0, 0);
           if (_Return == (int)AllocationStates.Completed)
           {
               _Valid = true;
           }
           return _Valid;
       }

       private int RecusivePeriodAllocation(int _ClassIndex, int _DayIndex, int _PeriodIndex)
       {
           bool IsCompleted = false;
           int _ReturnState;
           int _CurrentSubjectListIndex,_SubjectListCount;
           int[] _PosableSubjectList;
           int N_ClassIndex, N_DayIndex,N_PeriodIndex;
           if (!m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].IsEnabled || m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].IsFixed)
           {
               //NodeIs Eigther Disenabled or Fixed then go to next Node
               IsCompleted = GetNextNode(_ClassIndex, _DayIndex, _PeriodIndex, out N_ClassIndex, out N_DayIndex, out N_PeriodIndex);
               if (!IsCompleted)
               {
                   _ReturnState = RecusivePeriodAllocation(N_ClassIndex, N_DayIndex, N_PeriodIndex);

               }
               else
               {
                   _ReturnState = (int)AllocationStates.Completed;
               }
           }
           else
           {
               //String str = "Class Index =" + _ClassIndex + "; Day Index =" +_DayIndex +"; _PeriodIndex =" + _PeriodIndex;

               //System.Diagnostics.Debug.WriteLine(str); 
               //Node Is Need to be filled
               if (GetPossableSubjectList(_ClassIndex, _DayIndex, _PeriodIndex, out _PosableSubjectList, out _SubjectListCount))
               {
                   //Possable subjectList Receved. Now setting first node as _CurrentSubjectListIndex
                   
                   _CurrentSubjectListIndex=0;
                 
                   //simon before going to the next class make sure that the current class is completed correctly . 

                  // if ((_DayIndex + 1 == M_MaxDayCount) && (_PeriodIndex + 1 == M_MaxPeriodCount))
                  // {
                       // Thuis means going to next class. 


                   //}

                   IsCompleted = GetNextNode(_ClassIndex, _DayIndex, _PeriodIndex, out N_ClassIndex, out N_DayIndex, out N_PeriodIndex);

                   //if (_ClassIndex !=N_ClassIndex)
                   //if (!CompleteSubjectAllotment(_ClassIndex))
                   //    return (int)AllocationStates.Failed;


                   //str = "New  Class Index =" + N_ClassIndex + "; Day Index =" + N_DayIndex + "; _PeriodIndex =" + N_PeriodIndex;
                   //System.Diagnostics.Debug.WriteLine(str); 
                   if (!IsCompleted)
                   {

                       _ReturnState = (int)AllocationStates.Failed;
                       while (_ReturnState == (int)AllocationStates.Failed && _CurrentSubjectListIndex < _SubjectListCount)
                       {
                           SetNode(_ClassIndex, _DayIndex, _PeriodIndex, _PosableSubjectList[_CurrentSubjectListIndex]);
                          
                           _ReturnState = RecusivePeriodAllocation(N_ClassIndex, N_DayIndex, N_PeriodIndex);

                           //if the current option is not possable
                           if (_ReturnState == (int)AllocationStates.Failed)
                           {
                               //release the rersourse

                               UnSetNode(_ClassIndex, _DayIndex, _PeriodIndex, _PosableSubjectList[_CurrentSubjectListIndex]);


                               // then go with next option.
                               _CurrentSubjectListIndex++;
                           }

                       }


                   }
                   else
                   {
                       SetNode(_ClassIndex, _DayIndex, _PeriodIndex, _PosableSubjectList[_CurrentSubjectListIndex]);
                       _ReturnState = (int)AllocationStates.Completed;
                   }


               }
               else
               {
                   _ReturnState = (int)AllocationStates.Failed;
               }

           }
           return _ReturnState;
       }

       private bool CompleteSubjectAllotment(int _ClassIndex)
       {
           bool SubjectAllotCompleted = true;
           int _StaffSubjectIndex;


           for (_StaffSubjectIndex = 0; _StaffSubjectIndex < M_MaxStaffSubCount; _StaffSubjectIndex++)
           {
               // if the staff subject is for the selected class and is not fully allocated
               if (m_StaffSubjectList[_StaffSubjectIndex].ClassId == M_ClassId[_ClassIndex] && !m_StaffSubjectList[_StaffSubjectIndex].IsCompleted)
               {
                   if (! (m_StaffSubjectList[_StaffSubjectIndex].IsFreePerod))
                   if (m_StaffSubjectList[_StaffSubjectIndex].NAllocated != m_StaffSubjectList[_StaffSubjectIndex].NPerod)
                       return false;
               }
           }


           return SubjectAllotCompleted;
       }

       private void UnSetNode(int _ClassIndex, int _DayIndex, int _PeriodIndex, int _SelectedStaffSbjectIndex)
       {
           //decrement Staff Alloted Perods
           m_StaffList[m_StaffSubjectList[_SelectedStaffSbjectIndex].StaffNodeIndex].AllocatedPerods--;

           //decrement The staffSubject count in the  Node
           m_StaffSubjectList[_SelectedStaffSbjectIndex].NAllocated--;

           m_StaffSubjectList[_SelectedStaffSbjectIndex].IsCompleted = false;
           m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].StaffSubjectNodeIndex = -1;

           //cancel the allocation
           m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].IsAllocated = false;
       }

       private void SetNode(int _ClassIndex, int _DayIndex, int _PeriodIndex, int _SelectedStaffSbjectIndex)
       {
           m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].StaffId = m_StaffSubjectList[_SelectedStaffSbjectIndex].StaffId;
           m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].SubjectId = m_StaffSubjectList[_SelectedStaffSbjectIndex].SubjectId;
           m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].IsAllocated = true;
           m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].StaffSubjectNodeIndex = _SelectedStaffSbjectIndex;

           //Increment Staff Alloted Perods
           m_StaffList[m_StaffSubjectList[_SelectedStaffSbjectIndex].StaffNodeIndex].AllocatedPerods++;

           //Increment The staffSubject count in the  Node
           m_StaffSubjectList[_SelectedStaffSbjectIndex].NAllocated++;

           //Set completed is the alloterd equal to number of perods
           if (m_StaffSubjectList[_SelectedStaffSbjectIndex].NAllocated == m_StaffSubjectList[_SelectedStaffSbjectIndex].NPerod)
           {
               m_StaffSubjectList[_SelectedStaffSbjectIndex].IsCompleted = true;
           }
       }

       private bool GetPossableSubjectList(int _ClassIndex, int _DayIndex, int _PeriodIndex,out int[] _PosableSubjectList, out int _SubjectListCount)
       {
           bool valid = false;
           //Gat all the possable options in unprioritised order
           //if (_ClassIndex == 13)
           //{
           //    if (GetSubjectList(_ClassIndex, _DayIndex, _PeriodIndex, out  _PosableSubjectList, out _SubjectListCount))
           //    {
           //        valid = true;
           //        //order the possable options base on ranks.
           //        OrderSubjectList(_ClassIndex, _DayIndex, _PeriodIndex, _PosableSubjectList, _SubjectListCount);
           //    }
           //    else
           //    {
           //        valid = false;
           //    }
           //}
           //else
           //{
               if (GetSubjectList(_ClassIndex, _DayIndex, _PeriodIndex, out  _PosableSubjectList, out _SubjectListCount))
               {
                   valid = true;
                   //order the possable options base on ranks.
                   OrderSubjectList(_ClassIndex, _DayIndex, _PeriodIndex, _PosableSubjectList, _SubjectListCount);
               }
               else
               {
                   valid = false;
               }
           //}
           return valid;
       }

       private void OrderSubjectList(int _ClassIndex, int _DayIndex, int _PeriodIndex, int[] _PosableSubjectList, int _SubjectListCount)
       {
           double[] _RankList = new double[_SubjectListCount];
           int _ListIndex;
           for (_ListIndex = 0; _ListIndex < _SubjectListCount; _ListIndex++)
           {
               //if the perod is first perod and the staff is ClassTeacher
               if (m_StaffSubjectList[_PosableSubjectList[_ListIndex]].PerodType == (int)PerodType.ClassTeacher && _PeriodIndex == 0)
               {

                   _RankList[_ListIndex] = 1000000; //simon increased 
               }
               else
               {

                   _RankList[_ListIndex] = GetRankOftheSubject(_PosableSubjectList[_ListIndex], _ClassIndex, _DayIndex, _PeriodIndex);
                   
               }
           }

           ReArrangeBaseOnRank(ref _PosableSubjectList, _RankList, _SubjectListCount);
               
       }

       private void ReArrangeBaseOnRank(ref int[] _PosableSubjectList, double[] _RankList, int _SubjectListCount)
       {
           long right_border = _SubjectListCount - 1;

           do
           {
               long last_exchange = 0;

               for (long i = 0; i < right_border; i++)
               {
                   if (_RankList[i] < _RankList[i + 1])
                   {
                       double temp = _RankList[i];
                       _RankList[i] = _RankList[i + 1];
                       _RankList[i + 1] = temp;


                       int temp1 = _PosableSubjectList[i];
                       _PosableSubjectList[i] = _PosableSubjectList[i + 1];
                       _PosableSubjectList[i + 1] = temp1;

                       last_exchange = i;
                   }
               }

               right_border = last_exchange;
           }
           while (right_border > 0);

       }

       private double GetRankOftheSubject(int _SubjectListindex, int _ClassIndex, int _DayIndex, int _PeriodIndex)
       {
           double _rank = 0;
           double AllocatonMark = 0;
           if (m_StaffSubjectList[_SubjectListindex].NAllocated != 0)
           {
               AllocatonMark = (double)m_StaffSubjectList[_SubjectListindex].NPerod-m_StaffSubjectList[_SubjectListindex].NAllocated;
               AllocatonMark = AllocatonMark * 100;
               //AllocatonMark = 100 - AllocatonMark;
           }
           else
           {
               AllocatonMark = 1000;
           }
           _rank = AllocatonMark + 2 * m_StaffSubjectList[_SubjectListindex].NPerod - m_StaffSubjectList[_SubjectListindex].NAllocated;
           return _rank;
       }

       private bool GetSubjectList(int _ClassIndex, int _DayIndex, int _PeriodIndex, out int[] _PosableSubjectList, out int _SubjectListCount)
       {
           bool valid = false;
          
           //if the perod need to continus the previous perod.
           if (NeedToContinueWithPreviousSubject(_ClassIndex, _DayIndex, _PeriodIndex))
           {
               
               //if the staff is not prealocated in  same perod, same day for different class.
               if (!StaffPreAllocated(_ClassIndex, _DayIndex, _PeriodIndex, m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex - 1].StaffId) && m_StaffCube[m_StaffSubjectList[ m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex - 1].StaffSubjectNodeIndex].StaffNodeIndex, _DayIndex, _PeriodIndex])
               {

                   //add the previous perod alone as posiblity and return true.
                   _SubjectListCount = 1;
                   _PosableSubjectList = new int[_SubjectListCount];
                   _PosableSubjectList[0] = m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex - 1].StaffSubjectNodeIndex;
                   valid = true;
               }
               else
               {
                   //continous perod allocation failed
                   valid = false;
                   _PosableSubjectList = null;
                   _SubjectListCount = 0;
               }
           }
           else
           {
               //if not need to continue the previous subject generate posiablity list

               valid = GeneratePossablityList(_ClassIndex, _DayIndex, _PeriodIndex, out _PosableSubjectList, out _SubjectListCount);
               
           }
         

           return valid;
       }

       private bool GeneratePossablityList(int _ClassIndex, int _DayIndex, int _PeriodIndex, out int[] _PosableSubjectList, out int _SubjectListCount)
       {
           bool valid = false;
           _PosableSubjectList = null;
           _SubjectListCount = 0;
           int _StaffSubjectIndex;

           ArrayList AL_SelectedIndex = new ArrayList();


           // go through all the m_StaffSubjectList item
           for (_StaffSubjectIndex = 0; _StaffSubjectIndex < M_MaxStaffSubCount; _StaffSubjectIndex++)
           {
               // if the staff subject is for the selected class and is not fully allocated
               if (m_StaffSubjectList[_StaffSubjectIndex].ClassId == M_ClassId[_ClassIndex] && !m_StaffSubjectList[_StaffSubjectIndex].IsCompleted)
               {
                   //if the staff is not prealocated in  same perod, same day for different class and if staff is available that perod.
                   if (!StaffPreAllocated(_ClassIndex, _DayIndex, _PeriodIndex, m_StaffSubjectList[_StaffSubjectIndex].StaffId) && m_StaffCube[m_StaffSubjectList[_StaffSubjectIndex].StaffNodeIndex, _DayIndex, _PeriodIndex])

 
                       //if the select staff subject is a continues type subject
                       if (m_StaffSubjectList[_StaffSubjectIndex].PerodType == (int)PerodType.Continous)
                       {
                           //check if continous perod possable
                           if (HaveContinousPerod(m_StaffSubjectList[_StaffSubjectIndex].ContinousPerodCount, _ClassIndex, _DayIndex, _PeriodIndex))
                           {
                               // add index to list
                               valid = true;
                               AL_SelectedIndex.Add(_StaffSubjectIndex);
                           }

                       }
                       else
                       {

                           // add index to list
                           AL_SelectedIndex.Add(_StaffSubjectIndex);
                           valid = true;

                           //simon added for removing continues periods 
                            //if ((_PeriodIndex > 0))
                            //   if (m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex - 1].IsAllocated)
                            //   {
                            //       if (m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex - 1].StaffSubjectNodeIndex != _StaffSubjectIndex)
                            //       {
                            //           // add index to list
                            //           AL_SelectedIndex.Add(_StaffSubjectIndex);
                            //           valid = true;
                            //       }
                            //       else
                            //       {
                            //           valid = valid;

                            //       }

                            //   }


                       }


               }
           }


           if (valid)
           {
               _SubjectListCount=AL_SelectedIndex.Count;
               _PosableSubjectList=new int[_SubjectListCount];
               for (_StaffSubjectIndex = 0; _StaffSubjectIndex < _SubjectListCount; _StaffSubjectIndex++)
                   _PosableSubjectList[_StaffSubjectIndex] = (int)AL_SelectedIndex[_StaffSubjectIndex];

           }


           return valid;
       }

       private bool HaveContinousPerod(int _PerodCount, int _ClassIndex, int _DayIndex, int _PeriodIndex)
       {
           int _ConPCount = 0;
           bool _Valid = true;
           int i = _PeriodIndex;
           while (_ConPCount < _PerodCount)
           {
               if (i >= M_MaxPeriodCount)
                   return false;
               else if (m_TimeTableCube[_ClassIndex, _DayIndex, i].IsAllocated == true)
                   return false;
               else if (m_TimeTableCube[_ClassIndex, _DayIndex, i].IsFixed == true)
                   return false;
               else if (m_TimeTableCube[_ClassIndex, _DayIndex, i].IsEnabled == false)
                   return false;
               else if (m_TimeTableCube[_ClassIndex, _DayIndex, i].IsNextPeordBreak == true)
                   if (_ConPCount + 1 < _PerodCount)
                   {
                       return false;
                   }
                   else
                   {
                       return true;
                   }

                   i++;
                   _ConPCount++;
              
           }
           return _Valid;
       }

       private bool StaffPreAllocated(int _ClassIndex, int _DayIndex, int _PeriodIndex, int StaffId)
       {
           bool _Valid = false;
           if (StaffId == 0)
           {
               _Valid = false;
           }
           else
           {
               for (int i = 0; i < M_MaxClassCount; i++)
               {
                   if ((m_TimeTableCube[i, _DayIndex, _PeriodIndex].StaffId == StaffId) && (m_TimeTableCube[i, _DayIndex, _PeriodIndex].IsAllocated))
                   {
                       _Valid = true;
                       break;
                   }
               }
           }
           return _Valid;
       }

       private bool NeedToContinueWithPreviousSubject(int _ClassIndex, int _DayIndex, int _PeriodIndex)
       {
           bool _valid = false;
           //if the previous perod is not zero and IsAllocated and Continous
           if ((_PeriodIndex > 0))
               if (m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex - 1].IsAllocated)
                   if (m_StaffSubjectList[m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex - 1].StaffSubjectNodeIndex].PerodType == (int)PerodType.Continous)
                   {
                       // Get Continous Previous Subject Allocation count
                       int _ContinousPreviousAllocations = GetContinousPreviousSubjectAllocations(_ClassIndex, _DayIndex, _PeriodIndex);

                       //if the _ContinousPreviousAllocations less the actual needed continous perod then return true

                       if (_ContinousPreviousAllocations < m_StaffSubjectList[m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex - 1].StaffSubjectNodeIndex].ContinousPerodCount)
                       {
                           _valid = true;
                       }

                   }
       
           return _valid;
       }

       private int GetContinousPreviousSubjectAllocations(int _ClassIndex, int _DayIndex, int _PeriodIndex)
       {
           int _count = 1;
           int _reffISubIndex = m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex - 1].StaffSubjectNodeIndex;
           for (int i = _PeriodIndex - 2; i >= 0 && m_TimeTableCube[_ClassIndex, _DayIndex, i].StaffSubjectNodeIndex == _reffISubIndex; i--)
               _count++;
           return _count;
       }

       private bool GetNextNode(int _ClassIndex, int _DayIndex, int _PeriodIndex, out int N_ClassIndex, out int N_DayIndex, out int N_PeriodIndex)
       {
           bool _valid = false;
           N_ClassIndex = _ClassIndex;
           N_DayIndex = _DayIndex;
           N_PeriodIndex = _PeriodIndex + 1;

           if (N_PeriodIndex == M_MaxPeriodCount)
           {
               N_PeriodIndex = 0;
               N_DayIndex = _DayIndex + 1;
               if (N_DayIndex == M_MaxDayCount)
               {
                   N_DayIndex = 0;
                   N_ClassIndex = _ClassIndex + 1;
                   if (N_ClassIndex == M_MaxClassCount)
                   {
                       N_ClassIndex = 0;
                       _valid = true;
                   }
               }
           }

           return _valid;
       }

    
       private void DiableInvalidPeriods()
       {
           int _ClassIndex, _DayIndex, _PeriodIndex;
           bool _Fixed = false;
           bool _IsNextPerodBreak;
           bool _AvailableClassPeriod;
           for (_ClassIndex = 0; _ClassIndex < M_MaxClassCount; _ClassIndex++)
               for (_DayIndex = 0; _DayIndex < M_MaxDayCount; _DayIndex++)
                   for (_PeriodIndex = 0; _PeriodIndex < M_MaxPeriodCount; _PeriodIndex++)
                   {
                       if (ValidClassPeriod(M_ClassId[_ClassIndex], M_DayId[_DayIndex], M_PeriodId[_PeriodIndex], out _IsNextPerodBreak, out _AvailableClassPeriod, out _Fixed))
                       {
                           m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].IsEnabled = true;
                           m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].IsNextPeordBreak = _IsNextPerodBreak;
                           
                       }
                       else
                       {
                           m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].IsEnabled = false;
                           m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].IsNextPeordBreak = false;
                           if (!_AvailableClassPeriod)
                           {
                               m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].IsEnabled = false;
                           }
                       }
                       if (!_Fixed)
                       {
                           m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].IsAllocated = false;
                           m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].IsFixed = false;
                       }
                       else
                       {
                           m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].IsAllocated = true;
                           m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].IsFixed = true;
                           m_TimeTableCube[_ClassIndex, _DayIndex, _PeriodIndex].IsEnabled = false;
                       }

                   }

       }

       private bool ValidClassPeriod(int _ClassId, int _DayId, int _PeriodId, out bool _IsNextPerodBreak , out bool _AvailablePeriod , out bool _Fixed)
       {
           _IsNextPerodBreak = false;
           _AvailablePeriod = false;
           bool _Valid = false;
           string Period="";
           _Fixed = false;
           OdbcDataReader  Mytempreader = null;
           if (ClassPeriodisAvailable(_ClassId, _DayId, _PeriodId))
           {
               _AvailablePeriod = true;
               string sql = "select Id,NextPeriodBreak from tbltime_classperiod where DayId=" + _DayId + " and PeriodId=" + _PeriodId + " and ClassId=" + _ClassId;
               m_MyReader = m_MysqlDb.ExecuteQuery(sql);
               if (m_MyReader.HasRows)
               {
                   Period = m_MyReader.GetValue(0).ToString();
                   _Valid = true;
                   if (m_MyReader.GetValue(1).ToString() == "1")
                   {
                       _IsNextPerodBreak = true;
                   }
                   sql = "select ClassPeriodId from tbltime_master where ClassPeriodId=" + Period + " and Fixed=1";
                   Mytempreader =  m_MysqlDb.ExecuteQuery(sql);
                   if (Mytempreader.HasRows)
                   {
                       _Fixed = true;
                   }
               }
           }
           else
           {
               _AvailablePeriod = false;
               return false;
           }
           Mytempreader.Close();
           return _Valid;
       }

       private bool ClassPeriodisAvailable(int _ClassId, int _DayId, int _PeriodId)
       {
           bool _Valid = false;
           string sql = "select Id from tbltime_classperiod where ClassId=" + _ClassId + " and DayId=" + _DayId + " and PeriodId=" + _PeriodId + "";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               _Valid = true;
           }
           return _Valid; ;
       }

       private void InitiliseMasterDatas()
       {

           DataSet _ClassList = GetAllClassList(out M_MaxClassCount);
           DataSet _DaysList = GetAllDaysList(out M_MaxDayCount);
           DataSet _PerodsList = GetAllPerodsList(out M_MaxPeriodCount);
           LoadDataToArray(_ClassList, M_MaxClassCount, out M_ClassId, true);
           LoadDataToArray(_DaysList, M_MaxDayCount, out M_DayId, false);
           LoadDataToArray(_PerodsList, M_MaxPeriodCount, out M_PeriodId, false);
           m_TimeTableCube = new Node[M_MaxClassCount, M_MaxDayCount, M_MaxPeriodCount];
           LoadStaffList();
           LoadStaffSubjectList();
           LoadStaffAvailablity();
       }

       private void LoadStaffAvailablity()
       {
           m_StaffCube = new bool[M_MaxStaffCount, M_MaxDayCount, M_MaxPeriodCount];
           int _StaffIndex, _DayIndex, _PeriodIndex;

           for (_StaffIndex = 0; _StaffIndex < M_MaxStaffCount; _StaffIndex++)
               for (_DayIndex = 0; _DayIndex < M_MaxDayCount; _DayIndex++)
                   for (_PeriodIndex = 0; _PeriodIndex < M_MaxPeriodCount; _PeriodIndex++)
                   {
                       if (IsStaffAvailabile(m_StaffList[_StaffIndex].StaffId, M_DayId[_DayIndex], M_PeriodId[_PeriodIndex]))
                           m_StaffCube[_StaffIndex, _DayIndex, _PeriodIndex] = true;
                       else
                           m_StaffCube[_StaffIndex, _DayIndex, _PeriodIndex] = false;
                   }
       }

       private bool IsStaffAvailabile(int _StaffId, int _DayId, int _PeriodId)
       {
           
           bool _valid = false;
           if (_StaffId == 0)
               return true;

           if (IsStaffAvailabilityStored(_StaffId))
           {
               string sql = "select Id from tbltime_staffavailability where StaffId=" + _StaffId + " and DayId=" + _DayId + " and PeriodId=" + _PeriodId + "";
               m_MyReader = m_MysqlDb.ExecuteQuery(sql);
               if (m_MyReader.HasRows)
               {
                   _valid = true;
               }
           }
           else
           {
               string sql = "select DayId from tbltime_generalperiod where DayId=" + _DayId + " and PeriodId=" + _PeriodId + "";
               m_MyReader = m_MysqlDb.ExecuteQuery(sql);
               if (m_MyReader.HasRows)
               {
                   _valid = true;
               }
           }
          
           return _valid;
       }

       public bool IsStaffAvailabilityStored(int _StaffId)
       {
           bool _valid = false;
           int count=0;
           string sql = "select COUNT(Id) from tbltime_staffavailability where StaffId="+_StaffId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int.TryParse(m_MyReader.GetValue(0).ToString(), out count);
               if (count > 0)
               {
                   _valid = true;
               }
           }
           return _valid;
       }

       private void LoadStaffList()
       {
           M_MaxStaffCount = GetMaxStaffCount()+1;
           m_StaffList = new StaffNode[M_MaxStaffCount];
           int _StaffIndex = 0;
           DataSet StaffList = GetStaffList();
           if (StaffList != null && StaffList.Tables != null && StaffList.Tables[0].Rows.Count > 0)
           {
               m_StaffList[_StaffIndex].StaffId = 0;
               m_StaffList[_StaffIndex].MaxPerodPerDay = 0;
               m_StaffList[_StaffIndex].MaxPerodPerWeak = 0;
               m_StaffList[_StaffIndex].AllocatedPerods = 0;
               _StaffIndex++;
               foreach (DataRow Dr_Data in StaffList.Tables[0].Rows)
               {
                   m_StaffList[_StaffIndex].StaffId=int.Parse(Dr_Data[0].ToString());
                   m_StaffList[_StaffIndex].MaxPerodPerDay=int.Parse(Dr_Data[1].ToString());
                   m_StaffList[_StaffIndex].MaxPerodPerWeak=int.Parse(Dr_Data[2].ToString());
                   m_StaffList[_StaffIndex].AllocatedPerods=int.Parse(Dr_Data[3].ToString());
                   _StaffIndex++;
               }
           }
           
       }

       private DataSet GetStaffList()
       {
           DataSet Staff = new DataSet();
           DataTable dt;
           DataRow dr;
           Staff.Tables.Add(new DataTable("StaffList"));
           dt = Staff.Tables["StaffList"];
           dt.Columns.Add("StaffId");
           dt.Columns.Add("MaxDayPeriod");
           dt.Columns.Add("MaxWeekPeriod");
           dt.Columns.Add("Allocated");
           int MaxDayPeriod = GetStaffMaxDayPeriod();
           int MaxWeekPeriod = GetStaffMaxWeekPeriod();
           string sql = "select distinct StaffId from tblstaffsubjectmap";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               while (m_MyReader.Read())
               {
                   dr = Staff.Tables["StaffList"].NewRow();
                   dr["StaffId"] = int.Parse(m_MyReader.GetValue(0).ToString());
                   dr["MaxDayPeriod"] = MaxDayPeriod;
                   dr["MaxWeekPeriod"] = MaxWeekPeriod;
                   dr["Allocated"] = 0;
                   Staff.Tables["StaffList"].Rows.Add(dr);
               }
           }
           return Staff;
       }

       private int GetStaffMaxWeekPeriod()
       {
           OdbcDataReader MyStaffReader = null;
           int _Value=0;
           string sql = "select value1 from tbltime_config where Id=5";
           MyStaffReader = m_MysqlDb.ExecuteQuery(sql);
           if (MyStaffReader.HasRows)
           {
               int.TryParse(MyStaffReader.GetValue(0).ToString(), out _Value);
           }
           return _Value;
       }

       private int GetStaffMaxDayPeriod()
       {
           OdbcDataReader MyStaffReader = null;
           int _Value = 4;
           string sql = "select value1 from tbltime_config where Id=6";
           MyStaffReader = m_MysqlDb.ExecuteQuery(sql);
           if (MyStaffReader.HasRows)
           {
               _Value = int.Parse(MyStaffReader.GetValue(0).ToString());
           }
           return _Value;
       }

       private int GetMaxStaffCount()
       {
           int _Count = 0;
           string sql = "select count( distinct tblstaffsubjectmap.StaffId) from tblstaffsubjectmap";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int.TryParse(m_MyReader.GetValue(0).ToString(), out _Count);
           }
           return _Count;
          
       }

     
       private void LoadStaffSubjectList()
       {
           int NPeriodCount = 0;
           int MaxClassPeriod = 0;
           M_MaxStaffSubCount = GetMaxStaffSubCount() + M_MaxClassCount;
           m_StaffSubjectList = new StaffSubjectNode[M_MaxStaffSubCount];
           
           int _StaffSubIndex = 0;
           int _ClassIndex = 0;
           int _FreePerods = 0;

           for (_ClassIndex = 0; _ClassIndex < M_MaxClassCount; _ClassIndex++)
           {
               NPeriodCount = 0;
               _FreePerods = 0;
               DataSet _StaffSubList = GerStaffSubDataList(M_ClassId[_ClassIndex]);
               if (_StaffSubList != null && _StaffSubList.Tables != null && _StaffSubList.Tables[0].Rows.Count > 0)
               {
                   foreach (DataRow Dr_Data in _StaffSubList.Tables[0].Rows)
                   {

                       m_StaffSubjectList[_StaffSubIndex].ClassId = int.Parse(Dr_Data[0].ToString());
                       m_StaffSubjectList[_StaffSubIndex].SubjectId = int.Parse(Dr_Data[1].ToString());
                       m_StaffSubjectList[_StaffSubIndex].StaffId = int.Parse(Dr_Data[2].ToString());
                       m_StaffSubjectList[_StaffSubIndex].NPerod = int.Parse(Dr_Data[3].ToString());
                       m_StaffSubjectList[_StaffSubIndex].NAllocated = GetAllotedPeriodCount(Dr_Data[0].ToString(),Dr_Data[1].ToString(),int.Parse(Dr_Data[2].ToString()));  //simon 
                       if (m_StaffSubjectList[_StaffSubIndex].NPerod == 0)
                           m_StaffSubjectList[_StaffSubIndex].IsCompleted = true;
                       else
                           m_StaffSubjectList[_StaffSubIndex].IsCompleted = false;
                       if (m_StaffSubjectList[_StaffSubIndex].NPerod == m_StaffSubjectList[_StaffSubIndex].NAllocated)
                           m_StaffSubjectList[_StaffSubIndex].IsCompleted = true;

                       m_StaffSubjectList[_StaffSubIndex].StaffNodeIndex = GetStaffIndex(m_StaffSubjectList[_StaffSubIndex].StaffId, m_StaffList);
                       m_StaffSubjectList[_StaffSubIndex].IsFreePerod = false;
                       NPeriodCount = NPeriodCount + m_StaffSubjectList[_StaffSubIndex].NPerod;
                       m_StaffSubjectList[_StaffSubIndex].PerodType = (int)GetPerodType(m_StaffSubjectList[_StaffSubIndex].SubjectId, m_StaffSubjectList[_StaffSubIndex].StaffId, m_StaffSubjectList[_StaffSubIndex].ClassId);
                       if (m_StaffSubjectList[_StaffSubIndex].PerodType == (int)PerodType.Continous)
                           m_StaffSubjectList[_StaffSubIndex].ContinousPerodCount = GetCountinousPeriodCount(m_StaffSubjectList[_StaffSubIndex].SubjectId);
                       else
                           m_StaffSubjectList[_StaffSubIndex].ContinousPerodCount = 1;
                       _StaffSubIndex++;
                   }

               }
               MaxClassPeriod = GetMaximumPeridForClass(M_ClassId[_ClassIndex]);
               _FreePerods = MaxClassPeriod - NPeriodCount;
               if (_FreePerods < 0)
                   _FreePerods = 0;
               
               m_StaffSubjectList[_StaffSubIndex].ClassId = M_ClassId[_ClassIndex];
               m_StaffSubjectList[_StaffSubIndex].SubjectId = 0;
               m_StaffSubjectList[_StaffSubIndex].StaffId = 0;
               m_StaffSubjectList[_StaffSubIndex].NPerod = _FreePerods;
               m_StaffSubjectList[_StaffSubIndex].NAllocated = 0;  // simon we need to read this from data base 
               if (m_StaffSubjectList[_StaffSubIndex].NPerod == 0)
               m_StaffSubjectList[_StaffSubIndex].IsCompleted = true;
               else
                   m_StaffSubjectList[_StaffSubIndex].IsCompleted = false;
               m_StaffSubjectList[_StaffSubIndex].StaffNodeIndex = 0;
               m_StaffSubjectList[_StaffSubIndex].IsFreePerod = true;
               m_StaffSubjectList[_StaffSubIndex].ContinousPerodCount = 1;
               m_StaffSubjectList[_StaffSubIndex].PerodType = (int)PerodType.Normal;

               _StaffSubIndex++;

           }
  

       }

       private int GetPerodType(int _SubjectId, int _StaffId, int _ClassId)
       {
           int Type = (int)PerodType.Normal;
           if (CountinousPeriod(_SubjectId))
           {
               Type = (int)PerodType.Continous;
           }
           else if(ClassTeacherSubject(_ClassId,_StaffId,_SubjectId))
           {
               Type = (int)PerodType.ClassTeacher;
           }
           else if (CombinedSubject(_SubjectId))
           {
               Type = (int)PerodType.Combined;
           }
           else
           {
               Type = (int)PerodType.Normal;
           }
           return Type;
       }

       private bool CombinedSubject(int _SubjectId)
       {
           bool _Valid = false;
           string sql = "select tblsubjects.Combined from tblsubjects where tblsubjects.Id=" + _SubjectId;
           m_MyReader=  m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               if (m_MyReader.GetValue(0).ToString()=="1")
               {
                   _Valid = true;
               }
           }
           return _Valid;
       }

       private bool ClassTeacherSubject(int _ClassId, int _StaffId, int _SubjectId)
       {
           bool _Valid = false;
           string sql = "select tbltime_classsubjectstaff.StaffId from tblclasssubmap inner join tbltime_classsubjectstaff on tbltime_classsubjectstaff.ClassSubjectId = tblclasssubmap.Id where tblclasssubmap.ClassId=" + _ClassId + " and tblclasssubmap.SubjectId=" + _SubjectId + " and tbltime_classsubjectstaff.StaffId = (select tblclass.ClassTeacher from tblclass where tblclass.Id=" + _ClassId + " and tblclass.ClassTeacher="+_StaffId+")";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               _Valid = true;
           }
           return _Valid;
       }

       private bool CountinousPeriod(int _SubjectId)
       {
           bool _Valid = false;
           string sql = "select tblsubjects.Id from tblsubjects inner join tbltime_subgroup on tbltime_subgroup.Id = tblsubjects.sub_Catagory  where tblsubjects.Id=" + _SubjectId + " and tbltime_subgroup.AdjPeriods>1";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               _Valid = true;
           }
           return _Valid;
       }

     

       private DataSet GerStaffSubDataList(int _ClassId)
       {
           string sql = "select tblclasssubmap.ClassId , tblclasssubmap.SubjectId , tbltime_classsubjectstaff.StaffId , tbltime_classsubjectstaff.PeriodCount from tblclasssubmap  inner join tbltime_classsubjectstaff on tblclasssubmap.Id = tbltime_classsubjectstaff.ClassSubjectId where tblclasssubmap.ClassId=" + _ClassId + " order by tblclasssubmap.ClassId asc, tblclasssubmap.SubjectId asc, tbltime_classsubjectstaff.StaffId asc";
           m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           return m_MyDataSet;
       }

       private int GetMaxStaffSubCount()
       {
           int _Count = 0;
           string sql = "select count(tbltime_classsubjectstaff.StaffId) from tblclasssubmap  inner join tbltime_classsubjectstaff on tblclasssubmap.Id = tbltime_classsubjectstaff.ClassSubjectId WHERE tblclasssubmap.ClassId in (SELECT DISTINCT tbltime_classperiod.ClassId FROM tbltime_classperiod)";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int.TryParse(m_MyReader.GetValue(0).ToString(), out _Count);
           }
           return _Count;
       }

       private int GetMaximumPeridForClass(int _ClassID)
       {
           int _Count = 0;
           string sql = "select count(tbltime_classperiod.ClassId) from tbltime_classperiod where tbltime_classperiod.ClassId="+_ClassID;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int.TryParse(m_MyReader.GetValue(0).ToString(), out _Count);
           }
           return _Count;
       }

       private int GetCountinousPeriodCount(int _SubjectId)
       {
           int _Count=1;
           string sql = "select tbltime_subgroup.AdjPeriods from tblsubjects inner join tbltime_subgroup on tbltime_subgroup.Id = tblsubjects.sub_Catagory where tblsubjects.Id=(select distinct tblclasssubmap.SubjectId from tblclasssubmap where tblclasssubmap.SubjectId="+_SubjectId+")";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int.TryParse(m_MyReader.GetValue(0).ToString(), out _Count);
           }
           return _Count;
       }

       private int GetStaffIndex(int _StaffId, StaffNode[] m_StaffList)
       {
           int Value = 0;
           for (int i = 0; i < m_StaffList.Length;i++ )
           {
               if (_StaffId == m_StaffList[i].StaffId)
               {
                   Value = i;
                   return Value;
               }
           }
           return Value;
       }

    

       //private int GetMaxStaffSubCount()
       //{
       //    int _Count = 0;
       //   // string sql = "select count(StaffId) from tblstaffsubjectmap where tblstaffsubjectmap.StaffId in (select tbltime_classstaffmap.StaffId from tbltime_classstaffmap) and tblstaffsubjectmap.SubjectId in (select tblclasssubmap.SubjectId from tblclasssubmap)";
       //    //string sql = "select count(tblclasssubmap.ClassId) from tbltime_classsubjectstaff inner join tblclasssubmap on tblclasssubmap.Id = tbltime_classsubjectstaff.ClassSubjectId ";
       //    string sql = "select count()";
       //    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
       //    if (m_MyReader.HasRows)
       //    {
       //        int.TryParse(m_MyReader.GetValue(0).ToString(),out _Count);
       //    }
       //    return _Count;
       //}

       private DataSet GetAllPerodsList(out int M_MaxPeriodCount)
       {
           M_MaxPeriodCount = 0;
           string sql = "select distinct PeriodId from tbltime_classperiod order by PeriodId";
           m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           M_MaxPeriodCount = m_MyDataSet.Tables[0].Rows.Count;
           return m_MyDataSet;
       }

       private DataSet GetAllDaysList(out int M_MaxDayCount)
       {
           M_MaxDayCount = 0;
           string sql = "select distinct DayId from tbltime_classperiod order by DayId";
           m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           M_MaxDayCount = m_MyDataSet.Tables[0].Rows.Count;
           return m_MyDataSet;
       }

       private DataSet GetAllClassList(out int M_MaxClassCount)
       {
           M_MaxClassCount = 0;  // selecting the classes with perids available and subjects available
           string sql = "select distinct tbltime_classperiod.ClassId from tbltime_classperiod inner join tblclasssubmap on tblclasssubmap.ClassId = tbltime_classperiod.ClassId inner join tblclassstaffmap on tblclassstaffmap.ClassId = tbltime_classperiod.ClassId";
           m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           M_MaxClassCount = m_MyDataSet.Tables[0].Rows.Count;
           return m_MyDataSet;
       }

       private void LoadDataToArray(DataSet DS_DataList, int ArraySize, out int[] M_Array, bool _IsRandom)
       {
           M_Array = new int[ArraySize];
           int i = 0;
           if (DS_DataList != null && DS_DataList.Tables != null && DS_DataList.Tables[0].Rows.Count > 0)
           {
               foreach (DataRow Dr_Data in DS_DataList.Tables[0].Rows)
               {
                   M_Array[i] = int.Parse(Dr_Data[0].ToString());
                   i++;
               }
           }
           if (_IsRandom)
           {
               shuffle(ref M_Array);
           }
       }

       public static void shuffle(ref int[] array)
       {

           Random rng = new Random();
           int n = array.Length;
           int m=n;
           int[] TempData = new int[n];
           
          
           InitializeArray(ref TempData, -1);

           int i = 0;
           while (m > 1)
           {
               int k = rng.Next(n);
               if (NotRepeted(k, ref TempData))
               {
                   TempData[i] = k;
                   m--;
                   int temp = array[m];
                   array[m] = array[k];
                   array[k] = temp;
                   i++;
               }
           }
       }

       private static void InitializeArray(ref int[] TempData, int value)
       {
           for (int i = 0; i < TempData.Length; i++)
           {
               TempData[i] = value;
           }
       }

       private static bool NotRepeted(int k, ref int[] TempData)
       {
           bool _Valid = true;

           for (int i = 0; i < TempData.Length; i++)
           {
               if (TempData[i] == k)
               {
                   _Valid = false;
               }
           }
           return _Valid;
       }

       # endregion

       public DataSet GetDays()
       {
           string sql = "select distinct tbltime_week.Name , tbltime_week.Id  from tbltime_classperiod inner join tbltime_week on tbltime_week.Id = tbltime_classperiod.DayId  order by tbltime_week.Id";
           m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           return m_MyDataSet;
       }

       public string GetSubjectName(string _SubjectId)
       {
           string _SubName = "";
           if (_SubjectId == "0")
           {
               return "Free";
           }
           else
           {
               string sql = "select subject_name from tblsubjects where Id="+_SubjectId;
               m_MyReader = m_MysqlDb.ExecuteQuery(sql);
               if (m_MyReader.HasRows)
               {
                   _SubName = m_MyReader.GetValue(0).ToString();
               }
           }
           return _SubName;
       }



       public string GetSubjectCode(string _SubjectId)
       {
           string _SubName = "";
           if (_SubjectId == "0")
           {
               return "Free";
           }
           else
           {
               string sql = "select SubjectCode from tblsubjects where Id=" + _SubjectId;
               m_MyReader = m_MysqlDb.ExecuteQuery(sql);
               if (m_MyReader.HasRows)
               {
                   _SubName = m_MyReader.GetValue(0).ToString();
               }
           }
           return _SubName;
       }

       public string GetStaffName(string _ClassPeriodId)
       {
           string Name = "";
           string sql = "select tbluser.SurName , tbluser.Id from tbltime_master inner join tbluser on tbluser.Id = tbltime_master.StaffId where tbltime_master.ClassPeriodId="+_ClassPeriodId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {

               Name = m_MyReader.GetValue(0).ToString();
           }
           return Name;
       }


       public string GetStaffName_Id(string Id)
       {
           string Name = "";
           string sql = "select tbluser.SurName from tbluser where tbluser.Id=" + Id;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {

               Name = m_MyReader.GetValue(0).ToString();
           }
           return Name;
       }

       public void DeleteTimeTableEntry(string _ClassPeriodId)
       {
           string sql = "update tbltime_master set StaffId=0 , SubjectId=0 where ClassPeriodId="+_ClassPeriodId;
           m_MysqlDb.ExecuteQuery(sql);
       }

       public void UpdateTimetableentry(string _ClassPeriodId, string _Subject, string _Staff, int _Fixed)
       {
            
            string sql = "update tbltime_master set StaffId=" + _Staff + " , SubjectId=" + _Subject + " , Fixed="+_Fixed+" where ClassPeriodId=" + _ClassPeriodId;
            if (m_TransationDb != null)
            {
                m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MysqlDb.ExecuteQuery(sql);
            }
       }

       public bool IsTimeTableEntryPossible(string _ClassPeriodId, string _Staff,out string msg)
       {
           msg = "";
           int Day = 0;
           int PeriodId = GetPeriodid(_ClassPeriodId , out Day);
           bool _Valid = true;
           string sql = "";
           if ((PeriodId != 0) && (Day!=0))
           {
               if (IsStaffAvailabile(int.Parse(_Staff), Day, PeriodId))
               {

                   sql = "select tbltime_master.ClassPeriodId from tbltime_master inner join tbltime_classperiod on tbltime_classperiod.Id = tbltime_master.ClassPeriodId where tbltime_master.StaffId=" + _Staff + " and tbltime_classperiod.PeriodId=" + PeriodId + " and  tbltime_classperiod.DayId=" + Day;
                   m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                   if (m_MyReader.HasRows)
                   {
                       _Valid = false;
                       msg = "Staff is already engaged for same period in some other class";
                   }
               }
               else
               {
                   _Valid = false;
                   msg = "Staff is not available for selected period";
               }
           }
           return _Valid;
       }

       private int GetPeriodid(string _ClassPeriodId , out int _Day)
       {
           _Day = 0;
           int Periodid = 0;
           string sql = "select PeriodId,DayId from tbltime_classperiod where Id=" + _ClassPeriodId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               Periodid = int.Parse(m_MyReader.GetValue(0).ToString());
               _Day = int.Parse(m_MyReader.GetValue(1).ToString());
           }
           return
                Periodid;
       }



       public bool IsTimeTableGenerated()
       {
           bool _Valid = false;
           string sql = "select ClassPeriodId from tbltime_master" ;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               _Valid = true;
           }
           return _Valid;
       }

       public void DeleteTimeTableMaster()
       {
           string sql = "delete from tbltime_master";
           m_MysqlDb.ExecuteQuery(sql);
       }

       public void DeleteTimeTableMaster_Class(string ClassId)
       {
           string sql = "delete from tbltime_master where tbltime_master.ClassPeriodId in (SELECT tbltime_classperiod.Id FROM tbltime_classperiod WHERE tbltime_classperiod.ClassId="+ClassId+" )";
           m_MysqlDb.ExecuteQuery(sql);
       }

       public void DeleteAllClassSubjectStaff()
       {
           string sql = "delete from tbltime_classsubjectstaff";
           m_MysqlDb.ExecuteQuery(sql);
       }

       public void DeleteAllClassSubjectStaff_Class(string ClassId)
       {
           string sql = "DELETE FROM tbltime_classsubjectstaff WHERE tbltime_classsubjectstaff.ClassSubjectId in (SELECT tblclasssubmap.Id FROM tblclasssubmap WHERE tblclasssubmap.ClassId=" + ClassId + " )";
           m_MysqlDb.ExecuteQuery(sql);
       }


       public void RemoveSubjectFromClass(int _Classid, int _SubjectId)
       {
           string sql = "delete from tblclasssubmap where ClassId="+_Classid +" and SubjectId="+_SubjectId;
           m_MysqlDb.ExecuteQuery(sql);
       }

       public bool AllSubjectFilled(int _Classid, string _SubjectId , out string _Message)
       {
           bool _Valid = false;
           _Message = "";
           int MaxClassPeriods = GetMaximumPeridForClass(_Classid);
           int AllotedPerid = GetAllotedPeriodForClass(_Classid);
           int MinPeriod = GetSubjectMinimunPeriod(_SubjectId);
           if (AllotedPerid >= MaxClassPeriods)
           {
               _Message = "Cannot allot more subjects. Please increase the period count";
               return true ;
           }
           if (AllotedPerid > (MaxClassPeriods + MinPeriod))
           {
               _Message = "Cannot allot more subjects. Please reduce the minimum period of the subject";
               return true;
           }
           return _Valid;
       }

       private int GetSubjectMinimunPeriod(string _SubjectId)
       {
           int Min = 0;
           int Subgroup = GetSubGroup(int.Parse(_SubjectId));
           Min = GetMin(Subgroup);
           return Min;
       }

       private int GetMin(int _Subgroup)
       {
           int MinPeriod = 0;
           string sql = "select MinPeriodWeek from tbltime_subgroup where Id=" + _Subgroup;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               MinPeriod = int.Parse(m_MyReader.GetValue(0).ToString());
           }
           return MinPeriod;
       }

       private int GetAllotedPeriodForClass(int _Classid)
       {
           int MaxPeriod = 0;
           OdbcDataReader MySubReader = null;
           string sql = "select distinct SubjectId from tblclasssubmap where ClassId="+_Classid;
           MySubReader = m_MysqlDb.ExecuteQuery(sql);
           if (MySubReader.HasRows)
           {
               while (MySubReader.Read())
               {
                   MaxPeriod = MaxPeriod + GetMinPeriod(int.Parse(MySubReader.GetValue(0).ToString()));
               }
           }
           MySubReader.Close();
           return MaxPeriod;
       }

       public bool ValidateInputData(out string _Msg)
       {
           bool _Valid = true;
           _Msg = "";

           if (!TeacherPeriodMatched(out _Msg))
               return false;
           if (!CLassSubjectStaffPeriodMatched(out _Msg))
               return false;
           return _Valid;
       }

       private bool CLassSubjectStaffPeriodMatched(out string _Msg)
       {
           DataSet MyClassData = GetAllClassList(1);
           bool _Valid = true;
           int ClassMaxPerod = 0,StaffAllocationCount=0;
           _Msg = "Staff-subject ratio and Availabity  do not match in the class ";
           if (MyClassData != null && MyClassData.Tables != null && MyClassData.Tables[0].Rows.Count > 0)
           {
               foreach (DataRow Dr_Class in MyClassData.Tables[0].Rows)
               {
                   ClassMaxPerod = GetMaximumPeridForClass(int.Parse(Dr_Class[0].ToString()));
                   StaffAllocationCount = GetStaffAllocationCount(int.Parse(Dr_Class[0].ToString()));
                   //if ((StaffAllocationCount > (ClassMaxPerod+5))||(StaffAllocationCount < (ClassMaxPerod-10)))
                   if (StaffAllocationCount < (ClassMaxPerod - 20))
                   {
                       _Valid = false;
                       _Msg = _Msg + Dr_Class[1].ToString()+",";
                   }
                   
               }
           }
           return _Valid;
       }

       private int GetStaffAllocationCount(int _ClassId)
       {
           int Periods = 0;
           string sql = "SELECT sum(tbltime_classsubjectstaff.PeriodCount)  from tbltime_classsubjectstaff  inner join tblclasssubmap on tblclasssubmap.Id = tbltime_classsubjectstaff.ClassSubjectId where tblclasssubmap.ClassId="+_ClassId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int.TryParse(m_MyReader.GetValue(0).ToString(), out Periods);
           }
           return Periods;
       }

       private bool TeacherPeriodMatched(out string _Msg)
       {
           bool _Valid = true;
           _Msg = "";
           int StaffGenaralAvailability = GetTotalPeriodsForWeek(); ;
           int StaffPeriodCount = 0;
           int StaffAvlCount = 0;
           string StaffID = "";
           DataSet StaffAvailability = GetAllStaffAvailability();
           string sql = "select DISTINCT tbltime_classsubjectstaff.StaffId , tbltime_classsubjectstaff.PeriodCount from tbltime_classsubjectstaff INNER JOIN tblclasssubmap ON tbltime_classsubjectstaff.ClassSubjectId=tblclasssubmap.Id WHERE tblclasssubmap.ClassId in (SELECT DISTINCT tbltime_classperiod.ClassId FROM tbltime_classperiod)";
           DataSet MyStaffs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           if (StaffAvailability == null || StaffAvailability.Tables == null || StaffAvailability.Tables[0].Rows.Count == 0)
           {
               StaffAvailability = GetGeneralDatasetFor_SelectedStaff();
           }
           if (StaffAvailability != null  && StaffAvailability.Tables != null && StaffAvailability.Tables[0].Rows.Count > 0)
           {
               if (MyStaffs != null && MyStaffs.Tables != null && MyStaffs.Tables[0].Rows.Count > 0)
               {
                   foreach (DataRow Dr_Staff in MyStaffs.Tables[0].Rows)
                   {
                       if (StaffID != Dr_Staff[0].ToString())
                       {
                           StaffID = Dr_Staff[0].ToString();
                           StaffPeriodCount = GetStaffTotalPeriodCountFromDataset(ref MyStaffs, StaffID);
                           StaffAvlCount = GetStaffAvlCountFromDataset(StaffID, ref StaffAvailability);
                           if (StaffAvlCount <= 0)
                           {
                               StaffAvlCount= StaffGenaralAvailability;
                           }
                           if (StaffAvlCount < StaffPeriodCount)
                           {
                               _Msg = "Availability and period count doesnt match for staff " + GetStaffName_Id(StaffID); 
                               return false;
                           }

                       }

                   }
               }
           }
           else
           {
               _Msg = "Staff availability is not setted properly";
               return false;
           }
          

           return _Valid;
       }

       public DataSet GetGeneralDatasetFor_SelectedStaff()
       {
          
           DataSet GeneralStaffDataset = new DataSet();
           DataTable dt;
           DataRow dr;
           GeneralStaffDataset.Tables.Add(new DataTable("Staff"));
           dt = GeneralStaffDataset.Tables["Staff"];
           dt.Columns.Add("Id");
           dt.Columns.Add("DayId");
           dt.Columns.Add("PeriodId");
           dt.Columns.Add("StaffId");
           DataSet GeneralAvailabiliyt = GetGeneralAvailibilityDataSet();
           string sql = "select  DISTINCT StaffId from tbltime_classsubjectstaff";
           DataSet MyStaffs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           if (MyStaffs != null && MyStaffs.Tables != null && MyStaffs.Tables[0].Rows.Count > 0)
           {
               foreach (DataRow DRowStaff in MyStaffs.Tables[0].Rows)
               {
                   foreach (DataRow DRow in GeneralAvailabiliyt.Tables[0].Rows)
                   {
                       dr = GeneralStaffDataset.Tables["Staff"].NewRow();
                       dr["Id"] = DRowStaff[0].ToString();
                       dr["DayId"] = DRow[0].ToString();
                       dr["PeriodId"] = DRow[1].ToString();
                       dr["StaffId"] = DRowStaff[0].ToString();
                       GeneralStaffDataset.Tables["Staff"].Rows.Add(dr);
                   }
               }
           }
           return GeneralStaffDataset;
       }

       public DataSet GetGeneralAvailibilityDataSet()
       {
           string sql = "select DayId,PeriodId from tbltime_generalperiod";
           m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           return m_MyDataSet;
       }

      

       private int GetStaffAvlCountFromDataset(string _StaffID, ref DataSet StaffAvailability)
       {
           int Count = 0;
           foreach (DataRow Dr_Staff in StaffAvailability.Tables[0].Rows)
           {
               if (_StaffID == Dr_Staff[3].ToString())
               {
                   Count++;
               }
           }
           return Count;
       }

       private int GetStaffTotalPeriodCountFromDataset(ref DataSet _MyStaffs, string _StaffID)
       {
           int Count = 0;
           foreach (DataRow Dr_Staff in _MyStaffs.Tables[0].Rows)
           {
               if (_StaffID == Dr_Staff[0].ToString())
               {
                   Count = Count + int.Parse(Dr_Staff[1].ToString());
               }
           }
           return Count;
       }

       
       private DataSet GetAllStaffAvailability()
       {
           string sql = "select * from tbltime_staffavailability";
           m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           return m_MyDataSet;
       }


       public string GetMyTimeTable(int _UserId, DateTime firstDay, int BatchId, out bool _Isgenerated, out string _Table, MysqlClass _mysqlObj)
       {
           //MysqlClass _mysqlObj = new MysqlClass(m_ConnectionStr);
           Attendance MyAttendance = new Attendance(_mysqlObj);
           _Isgenerated = true;
           _Table = "";
           string sql = "";
           DateTime WorkingDate = firstDay;
           StringBuilder TimeTable = new StringBuilder();
           DataSet MyDays = null;
           DataSet MyPeriods = null;
           OdbcDataReader MyPeriod = null;
           string PeriodData = "";
           sql = "select distinct tbltime_week.Id,tbltime_week.Name from tbltime_week order by tbltime_week.Id";
           MyDays = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           MyPeriods = GetStaffAvlPeriods(_UserId);

           if (MyPeriods != null && MyPeriods.Tables != null && MyPeriods.Tables[0].Rows.Count > 0 && MyDays != null && MyDays.Tables != null && MyDays.Tables[0].Rows.Count > 0)
           {
               TimeTable.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"5\" class=\"MyTable\">");
               //*********************HEADER *********************************
               TimeTable.Append("<tr>");
               TimeTable.Append("<th>DAYS/PERIODS</th>");
               foreach (DataRow Dr_Period in MyPeriods.Tables[0].Rows)
               {
                   TimeTable.Append("<th>" + Dr_Period[1].ToString().ToUpper() + "</th>");
               }
               TimeTable.Append("</tr>");

               //*********************ROWS********************************

               foreach (DataRow Dr_Day in MyDays.Tables[0].Rows)
               
               {
                   bool IsHolliday = false;
                   bool IsCurrentBatch = true;
                   TimeTable.Append("<tr");
                   if (WorkingDate == DateTime.Now.Date)
                   {
                       TimeTable.Append(" style=\"background-color: #97FFB1;\" ");
                   }
                   else if (!IsDateInBath(WorkingDate, BatchId))
                   {
                       TimeTable.Append(" style=\"background-color: #ffc1c1;\" ");
                       IsCurrentBatch = false;
                   }
                   else if (MyAttendance.IsDateHoliday(0, WorkingDate))
                   {
                       TimeTable.Append(" style=\"background-color: #ffcc00;\" ");
                       IsHolliday = true;
                   }
                   TimeTable.Append(">");
                   TimeTable.Append("<td><b>" + Dr_Day[1].ToString().Substring(0,3).ToUpperInvariant() +" , " + General.GerFormatedDatVal(WorkingDate) + "</b></td>");

                   foreach (DataRow Dr_Period in MyPeriods.Tables[0].Rows)
                   {
                       TimeTable.Append("<td class=\"DataCell\"><b>");

                       bool ClassAssaingned = false;
                       sql = "SELECT tblclass.ClassName,tbltime_submaster.SubjectId FROM tbltime_submaster INNER JOIN tblclass ON tblclass.Id=tbltime_submaster.ClassId WHERE tbltime_submaster.PeriodId=" + Dr_Period[0].ToString() + " AND tbltime_submaster.`Day`='" + WorkingDate.Date.ToString("s") + "' AND tbltime_submaster.StaffId=" + _UserId;
                       MyPeriod = m_MysqlDb.ExecuteQuery(sql);
                       if (MyPeriod.HasRows)
                       {
                           PeriodData = "Class : " + MyPeriod.GetValue(0).ToString() + "<br>Subject : " +GetSubjectName( MyPeriod.GetValue(1).ToString());
                           TimeTable.Append(PeriodData);
                           ClassAssaingned = true;
                       }


                       if (!ClassAssaingned && !IsHolliday && IsCurrentBatch)
                       {
                           sql = "SELECT tblclass.ClassName,tbltime_master.SubjectId FROM tbltime_master INNER JOIN tbltime_classperiod ON tbltime_classperiod.Id=tbltime_master.ClassPeriodId INNER JOIN tblclass ON tblclass.Id=tbltime_classperiod.ClassId WHERE tbltime_classperiod.PeriodId=" + Dr_Period[0].ToString() + " AND tbltime_classperiod.DayId=" + Dr_Day[0].ToString() + " AND tbltime_master.StaffId=" + _UserId;
                           MyPeriod = m_MysqlDb.ExecuteQuery(sql);
                           if (MyPeriod.HasRows)
                           {
                               PeriodData = "Class : " + MyPeriod.GetValue(0).ToString() + "<br>Subject : " + GetSubjectName(MyPeriod.GetValue(1).ToString());
                               TimeTable.Append(PeriodData);
                               ClassAssaingned = true;
                           }
                       }

                       if (!ClassAssaingned)
                       {

                           TimeTable.Append("FREE");
                       }


                       // OLD CODE COmmented
                       //sql = "SELECT tbltime_week.Name, tblclass.ClassName, tblsubjects.subject_name from tbltime_classperiod inner join tbltime_master on tbltime_master.ClassPeriodId= tbltime_classperiod.Id inner join tbltime_week on tbltime_week.Id= tbltime_classperiod.DayId inner join tblclass on tblclass.Id=tbltime_classperiod.ClassId inner join tblsubjects on tblsubjects.Id= tbltime_master.SubjectId where tbltime_week.Name='" + Dr_Day[1].ToString() + "' and tbltime_master.StaffId=" + _UserId + " and tbltime_classperiod.PeriodId=" + Dr_Period[0].ToString();
                       //MyPeriod = m_MysqlDb.ExecuteQuery(sql);
                       //if (MyPeriod.HasRows)
                       //{
                       //    PeriodData = "Class : " + MyPeriod.GetValue(1).ToString() + "<br>Subject : " + MyPeriod.GetValue(2).ToString();
                       //    TimeTable.Append(PeriodData);
                       //}
                       //else
                       //    TimeTable.Append("FREE");

                       TimeTable.Append("</b></td>");
                   }
                   TimeTable.Append("</tr>");
                   WorkingDate = WorkingDate.AddDays(1);
               }

               TimeTable.Append("</table>");
           }
           else
           {
               TimeTable.Append("<br/><br/><br/><h4>Time table is not generated</h4>");
               _Isgenerated = false;
           }
           _Table = TimeTable.ToString();
           //_mysqlObj.CloseConnection();
           //_mysqlObj = null;
           MyAttendance = null;
           return _Table;
       }

       private DataSet GetStaffAvlPeriods(int StaffId)
       {
           bool Presence = false;
           DataSet NewDataSet = new DataSet();
           string sql = "select distinct tbltime_staffavailability.PeriodId,tblattendanceperiod.FrequencyName from tbltime_staffavailability inner join tblattendanceperiod On tblattendanceperiod.PeriodId=tbltime_staffavailability.PeriodId where tbltime_staffavailability.StaffId="+StaffId+" ORDER BY tbltime_staffavailability.PeriodId ";
           NewDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           if (NewDataSet != null && NewDataSet.Tables[0].Rows.Count > 0)
           {
               Presence = true;
           }
           if (!Presence)
           {
               sql = "select distinct tbltime_generalperiod.PeriodId,tblattendanceperiod.FrequencyName from tbltime_generalperiod inner join tblattendanceperiod On tblattendanceperiod.PeriodId=tbltime_generalperiod.PeriodId  ORDER BY tbltime_generalperiod.PeriodId ";
               NewDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           }

           return NewDataSet;
       }

       public DataSet GetStaffAvlDayIds(int StaffId)
       {
           bool Presence = false;
           DataSet NewDataSet = new DataSet();
           string sql = "select distinct tbltime_staffavailability.DayId,tbltime_week.Name from tbltime_staffavailability inner join tbltime_week On tbltime_week.Id=tbltime_staffavailability.DayId where tbltime_staffavailability.StaffId=" + StaffId + " ORDER BY tbltime_staffavailability.DayId ";
           NewDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           if (NewDataSet != null && NewDataSet.Tables[0].Rows.Count > 0)
           {
               Presence = true;
           }
           if (!Presence)
           {
               sql = "select distinct tbltime_generalperiod.DayId,tbltime_week.Name from tbltime_generalperiod inner join tbltime_week On tbltime_week.Id=tbltime_generalperiod.DayId  ORDER BY tbltime_generalperiod.DayId ";
               NewDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           }

           return NewDataSet;
       }

       public void InsertMasterEntry(string _PeriodId, string _SubjectId, string _StaffId, string _ClassId)
       {
           if (MasterExists(_PeriodId, _SubjectId, _StaffId))
           {
               UpdateTimetableentry(_PeriodId, _SubjectId, _StaffId, 1);
           }
           else
           {
               InserTimeMasterData(int.Parse(_PeriodId),int.Parse( _StaffId), int.Parse(_SubjectId),1);
           }
           IncreaseStaffAllocationCount(_StaffId, _SubjectId, _ClassId);  
       }

       private bool ZeroEntryExists(string _PeriodId, string _SubjectId, string _StaffId)
       {
           bool Valid=false;
           string sql = "select StaffId from tbltime_master where StaffId=0 AND SubjectId=0 AND ClassPeriodId=" + _PeriodId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               Valid = true;
           }
           return Valid;
       }

       private void IncreaseStaffAllocationCount(string _StaffId, string _SubjectId, string _ClassId)
       {
           int _Count = 0;
           OdbcDataReader CountReader = null;
           string ClassSubmapId = "0";
           string sql = "select tbltime_classsubjectstaff.PeriodCount ,tblclasssubmap.Id  from tbltime_classsubjectstaff inner join tblclasssubmap on tblclasssubmap.Id = tbltime_classsubjectstaff.ClassSubjectId  where tblclasssubmap.ClassId = " + _ClassId + " and tblclasssubmap.SubjectId=" + _SubjectId + " and tbltime_classsubjectstaff.StaffId=" + _StaffId;
           CountReader = m_MysqlDb.ExecuteQuery(sql);
           if (CountReader.HasRows)
           {
               int.TryParse(CountReader.GetValue(0).ToString(), out _Count);
               int AllotedPeriodCount = GetAllotedPeriodCount(_ClassId, _SubjectId, int.Parse(_StaffId));
               if (AllotedPeriodCount > _Count)
               {
                   _Count = AllotedPeriodCount;
                   ClassSubmapId = CountReader.GetValue(1).ToString();
                   sql = "update tbltime_classsubjectstaff set PeriodCount = " + _Count + " where  ClassSubjectId =" + ClassSubmapId + " and StaffId = " + _StaffId + "";
                   m_MysqlDb.ExecuteQuery(sql);
               }
           }
           else
           {
               int AllotedPeriodCount = GetAllotedPeriodCount(_ClassId, _SubjectId, int.Parse(_StaffId));
               if (AllotedPeriodCount > _Count)
               {
                   _Count = AllotedPeriodCount;
                   sql = "insert into tbltime_classsubjectstaff (ClassSubjectId,StaffId,PeriodCount) values ((select tblclasssubmap.Id  from tblclasssubmap  where tblclasssubmap.ClassId = " + _ClassId + " and tblclasssubmap.SubjectId=" + _SubjectId + ")," + _StaffId + "," + _Count + ")";
                   m_MysqlDb.ExecuteQuery(sql);
               }
              
           }
           CountReader.Close();
           
       }

       public int GetAllotedPeriodCount(string _ClassId, string SubjectsId, int StaffId)
       {
           int Count = 0;
           string subsql = "";
           if (StaffId > 0)
           {
               subsql = " AND tbltime_master.StaffId=" + StaffId;
           }
           string sql = "SELECT COUNT(tbltime_master.ClassPeriodId) FROM tbltime_master INNER JOIN tbltime_classperiod ON tbltime_classperiod.Id=tbltime_master.ClassPeriodId WHERE tbltime_classperiod.ClassId=" + _ClassId + " AND tbltime_master.SubjectId=" + SubjectsId + subsql;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int.TryParse(m_MyReader.GetValue(0).ToString(), out Count);
           }
           return Count;
       }


       private bool MasterExists(string _PeriodId, string _SubjectId, string _StaffId)
       {
           bool Valid = false;
           string sql = "select StaffId from tbltime_master where ClassPeriodId=" + _PeriodId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               Valid = true;
           }
           return Valid;
       }



       public int TotalClassCount_Subject(string SubjectId)
       {
           int Count = 0;
           string sql = "SELECT COUNT( DISTINCT tblclasssubmap.ClassId) FROM tblclasssubmap WHERE tblclasssubmap.SubjectId=" + SubjectId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int.TryParse(m_MyReader.GetValue(0).ToString(), out Count);
           }
           return Count;
       }

       public int GetEstimatedPeriods_Subject(string SubjectID)
       {
           bool PeriodExists = false;
           string ClassSubMapId = "",ClassId="";
           int StaffId = 0, PeriodCount = 0, TotalPeriodCount = 0;
           string sql = "SELECT tblclasssubmap.ClassId,tblclasssubmap.Id,tbltime_subgroup.MinPeriodWeek FROM tblclasssubmap INNER JOIN tblsubjects ON tblsubjects.Id=tblclasssubmap.SubjectId INNER JOIN tbltime_subgroup ON tbltime_subgroup.Id=tblsubjects.sub_Catagory WHERE tblclasssubmap.SubjectId=" + SubjectID;
           OdbcDataReader OuterReader = m_MysqlDb.ExecuteQuery(sql);
           if (OuterReader.HasRows)
           {
               while (OuterReader.Read())
               {
                   StaffId = 0;
                   PeriodCount = 0;
                   PeriodExists = false;
                   ClassId = OuterReader.GetValue(0).ToString();
                   ClassSubMapId = OuterReader.GetValue(1).ToString();
                   int.TryParse(OuterReader.GetValue(2).ToString(), out PeriodCount);
                   sql = "SELECT tbltime_classsubjectstaff.StaffId,tbltime_classsubjectstaff.PeriodCount FROM tbltime_classsubjectstaff WHERE tbltime_classsubjectstaff.ClassSubjectId=" + ClassSubMapId;
                   OdbcDataReader InnerReader = m_MysqlDb.ExecuteQuery(sql);
                   if (InnerReader.HasRows)
                   {
                       while (InnerReader.Read())
                       {
                           StaffId = 0;

                           if (int.TryParse(InnerReader.GetValue(0).ToString(), out StaffId) && int.TryParse(InnerReader.GetValue(1).ToString(), out PeriodCount))
                           {
                               PeriodExists = true;
                               TotalPeriodCount = TotalPeriodCount + PeriodCount;
                           }
                       }
                   }


                   if (!PeriodExists)
                   {
                       TotalPeriodCount = TotalPeriodCount + PeriodCount;

                   }

               }
           }



           return TotalPeriodCount;
       }

       public int GetTotalStaff_Subject(string SubjectID)
       {
           int Count = 0;
           string sql = "SELECT COUNT(tblstaffsubjectmap.StaffId) FROM tblstaffsubjectmap WHERE tblstaffsubjectmap.SubjectId=" + SubjectID;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int.TryParse(m_MyReader.GetValue(0).ToString(), out Count);
           }
           return Count;
       }

       public int getTotalSubject_Class(string ClassId)
       {
           int Count = 0;
           string sql = "SELECT COUNT( DISTINCT tblclasssubmap.SubjectId) FROM tblclasssubmap WHERE tblclasssubmap.ClassId=" + ClassId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int.TryParse(m_MyReader.GetValue(0).ToString(), out Count);
           }
           return Count;
       }
       public int getTotalStaff_Class(string ClassId)
       {

           int Count = 0;
           string sql = "SELECT COUNT( DISTINCT tblclassstaffmap.StaffId) FROM tblclassstaffmap WHERE tblclassstaffmap.ClassId=" + ClassId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int.TryParse(m_MyReader.GetValue(0).ToString(), out Count);
           }
           return Count;
       }


       public int getAllotedCount_Class(string ClassId)
       {
           int Count = 0;
           string sql = "SELECT COUNT(tbltime_master.ClassPeriodId) FROM tbltime_master INNER JOIN tbltime_classperiod ON tbltime_classperiod.Id=tbltime_master.ClassPeriodId WHERE tbltime_master.StaffId!=0 AND tbltime_master.SubjectId!=0 AND tbltime_classperiod.ClassId=" + ClassId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int.TryParse(m_MyReader.GetValue(0).ToString(), out Count);
           }
           return Count;
       }

       public void AddSubjectsToStaff(int _SubjectId, int _StaffId)
       {

           string sql = "INSERT INTO tblstaffsubjectmap(StaffId,SubjectId) VALUES (" + _StaffId + ", " + _SubjectId + ")";
           if (m_TransationDb != null)
           {
               m_MyReader = m_TransationDb.ExecuteQuery(sql);
           }
           else
           {
               m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           }

           m_MyReader.Close();

       }

       public bool IsClassAvailibilityStored(string ClassId)
       {
           bool valid = false;
           int Count = 0;
           string sql = "SELECT COUNT(tbltime_classperiod.Id) FROM tbltime_classperiod WHERE tbltime_classperiod.ClassId=" + ClassId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int.TryParse(m_MyReader.GetValue(0).ToString(), out Count);
               if (Count > 0)
               {
                   valid = true;
               }
           }
           return valid;
       }




       public int GetDayId_Date(DateTime CalenderDays)
       {
           int Id = 0;
           string Day = CalenderDays.Date.DayOfWeek.ToString();
           string sql = "SELECT tbltime_week.Id FROM tbltime_week WHERE lower(tbltime_week.Name)='"+Day.ToLowerInvariant()+"'";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int.TryParse(m_MyReader.GetValue(0).ToString(),out Id);
           }
           return Id;
       }



       public int GetClassPeriodId(int ClassId, int DayId, int PeriodId)
       {
           int ClassPeriodId = 0;
           string sql = "SELECT tbltime_classperiod.Id FROM tbltime_classperiod WHERE tbltime_classperiod.ClassId=" + ClassId + " AND tbltime_classperiod.DayId="+DayId+" AND tbltime_classperiod.PeriodId="+PeriodId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int.TryParse(m_MyReader.GetValue(0).ToString(), out ClassPeriodId);
           }
           return ClassPeriodId;
       }

       public bool GetClassPeriodDetails_Weekly(int ClassId, int PeriodId, DateTime Days, out int staffId, out int SubjectId, out bool Generalbool)
       {
           staffId = -1; SubjectId = -1;
           bool Done = false;
           Generalbool = false;

           string sql = "SELECT tbltime_submaster.StaffId,tbltime_submaster.SubjectId FROM tbltime_submaster WHERE tbltime_submaster.`Day`='" + Days.Date.ToString("s") + "' AND tbltime_submaster.ClassId="+ClassId+" AND tbltime_submaster.PeriodId="+PeriodId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int.TryParse(m_MyReader.GetValue(0).ToString(), out staffId);
               int.TryParse(m_MyReader.GetValue(1).ToString(), out SubjectId);
               Done = true;
           }
           if (!Done)
           {
               sql = "SELECT tbltime_master.StaffId,tbltime_master.SubjectId FROM tbltime_master INNER JOIN tbltime_classperiod ON tbltime_classperiod.Id=tbltime_master.ClassPeriodId INNER JOIN tbltime_week ON tbltime_classperiod.DayId=tbltime_week.Id WHERE tbltime_classperiod.ClassId=" + ClassId + " AND tbltime_classperiod.PeriodId=" + PeriodId + " AND lower(tbltime_week.Name)='" + Days.Date.DayOfWeek.ToString().ToLowerInvariant() +"'";
               m_MyReader = m_MysqlDb.ExecuteQuery(sql);
               if (m_MyReader.HasRows)
               {
                   int.TryParse(m_MyReader.GetValue(0).ToString(), out staffId);
                   int.TryParse(m_MyReader.GetValue(1).ToString(), out SubjectId);
                   Done = true;
                   Generalbool = true;
               }
           }
           return Done;
       }

       public string getPeriodName(int Id)
       {
           string PeriodName = "";
           string sql = "SELECT tblattendanceperiod.FrequencyName FROM tblattendanceperiod WHERE tblattendanceperiod.PeriodId=" +Id;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               PeriodName = m_MyReader.GetValue(0).ToString();
           }
           return PeriodName;
       }

       public bool AddTemperoryTTEntry(DateTime Day, string ClassId, string PeriodId, string StaffId, string SubjectId)
       {
           bool valid = true;
           try
           {

               if (TemperoryTimeTableEntryExists(Day, ClassId, PeriodId))
               {
                   string sql = "update tbltime_submaster set StaffId=" + StaffId + ",SubjectId=" + SubjectId + " where `Day`='" + Day.Date.ToString("s") + "' AND ClassId=" + ClassId + " AND PeriodId=" + PeriodId;
                   m_MysqlDb.ExecuteQuery(sql);
               }
               else
               {
                   string sql = "insert into tbltime_submaster (Day,ClassId,PeriodId,StaffId,SubjectId) values ('" + Day.Date.ToString("s") + "'," + ClassId + "," + PeriodId + "," + StaffId + "," + SubjectId + ")";
                   m_MysqlDb.ExecuteQuery(sql);
               }
           }
           catch
           {
               valid = false;
           }

           return valid;
       }

       public bool TemperoryTimeTableEntryExists(DateTime Day, string ClassId, string PeriodId)
       {
           int Count = 0;
           bool valid = false;
           string sql = "SELECT COUNT(tbltime_submaster.SubjectId) FROM tbltime_submaster WHERE tbltime_submaster.`Day`='" + Day.Date.ToString("s") + "' AND tbltime_submaster.PeriodId=" + PeriodId + " AND tbltime_submaster.ClassId=" + ClassId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int.TryParse(m_MyReader.GetValue(0).ToString(), out Count);
               if (Count > 0)
               {
                   valid = true;
               }
           }
           return valid;
       }



       public bool DeleteTemperoryTTEntry(DateTime Day, string ClassId, string PeriodId)
       {
           bool valid = true;
           try
           {
               string sql = "delete from tbltime_submaster where `Day`='" + Day.Date.ToString("s") + "' AND ClassId=" + ClassId + " AND PeriodId=" + PeriodId;
               m_MysqlDb.ExecuteQuery(sql);
           }
           catch
           {
               valid = false;
           }

           return valid;
       }



       public bool IsperiodAvailable_Class(string periodId, int ClassId, string DayId,out int classperiodID)
       {
           bool valid = false;
           classperiodID = 0;
           string sql = "select Id from tbltime_classperiod where DayId="+DayId+" and ClassId="+ClassId+" and PeriodId="+periodId+" order by PeriodId";
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int.TryParse(m_MyReader.GetValue(0).ToString(), out classperiodID);
               if (classperiodID > 0)
               {
                   valid = true;
               }
           }
           return valid;
       }

       private bool IsDateInBath(DateTime SELECTEDDATE,int BatchId)
       {
           int M_Id = 0;
           bool valid = false;
           string sql = "SELECT tblbatch.Id FROM tblbatch WHERE tblbatch.Id=" + BatchId + " AND '" + SELECTEDDATE.Date.ToString("s") + "' BETWEEN tblbatch.StartDate AND tblbatch.EndDate";
           OdbcDataReader MyReader1 = m_MysqlDb.ExecuteQuery(sql);
           if (MyReader1.HasRows)
           {
               int.TryParse(MyReader1.GetValue(0).ToString(), out M_Id);
               if (M_Id > 0)
               {
                   valid = true;
               }
           }
           return valid;
       }

       public int GetStaff_PeriodCount(string StaffId)
       {
           int Count = 0;
           string sql = "SELECT SUM(tbltime_classsubjectstaff.PeriodCount) FROM tbltime_classsubjectstaff WHERE tbltime_classsubjectstaff.StaffId=" + StaffId;
           m_MyReader = m_MysqlDb.ExecuteQuery(sql);
           if (m_MyReader.HasRows)
           {
               int.TryParse(m_MyReader.GetValue(0).ToString(), out Count);
           }
           return Count;
       }


   }
}
