using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Data;
using System.Text.RegularExpressions;

namespace WinBase
{

    public struct IncidentList
    {
        public int      id;
        public string   Values;
    }

    public struct IncidentHeder
    {
        public int      id;
        public string   Values;
    }
    public struct FeedbackClass
    {
        public IncidentHeder        IncidentType;
        public List<IncidentList>   Incidentheadings;
    }

     public class Incident : KnowinGen
    {
        public MysqlClass m_MysqlDb;
        private OdbcDataReader m_MyReader = null;
     
        private string m_IncedentMenuStr;
        private string m_SubIncidentMenuStr;
        public Incident(KnowinGen _Prntobj)
        {
            m_Parent = _Prntobj;
            m_MyODBCConn = m_Parent.ODBCconnection;
            m_UserName = m_Parent.LoginUserName;
            m_MysqlDb = new MysqlClass(this);
            m_IncedentMenuStr = "";
            m_SubIncidentMenuStr = "";
        }

        public Incident(MysqlClass _Msqlobj)
        {
            m_MysqlDb = _Msqlobj;
           
        }



        ~Incident()
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

        public void CreateIncedent(string _Title, string _Dese, string _Date, int _InceType, int _Approver, int _UserId, string _Type, int _AssoUserId, int _Head, int _Point, int _BatchId, int _ClassId)
        {
            General _GenObj = new General(m_MysqlDb);
            int _Incedentid = _GenObj.GetTableMaxId("tblview_incident", "Id");

            DateTime _InceDate = _GenObj.GetDateFromText(_Date);

            DateTime Today = DateTime.Now;

            string sql = " insert into tblincedent (Id,Title,Description,IncedentDate,CreatedDate,TypeId,ApproverId,CreatedUserId,UserType,Status,AssoUser,HeadId,Point,BatchId,ClassId)values (" + _Incedentid + ",'" + _Title + "','" + _Dese + "','" + _InceDate.ToString("s") + "','" + Today.ToString("s") + "'," + _InceType + "," + _Approver + "," + _UserId + ",'" + _Type.ToLower() + "','Created'," + _AssoUserId + "," + _Head + "," + _Point + "," + _BatchId + "," + _ClassId + " )";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);

            m_MyReader.Close();
        }



        public void CreateApprovedIncedent(string _Title, string _Dese, string _Date, int _InceType, int _UserId, string _Type, int _AssoUser, int _HeadId, int _Point, int _BatchId, int _ClassId)
        {
            General _GenObj = new General(m_MysqlDb);
            int _Incedentid = _GenObj.GetTableMaxId("tblview_incident", "Id");
            DateTime _InceDate = _GenObj.GetDateFromText(_Date);
            //string _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime Today = DateTime.Now;
            string sql = "insert into tblincedent (Id,Title,Description,IncedentDate,CreatedDate,TypeId,ApproverId,CreatedUserId,UserType,Status,AssoUser,HeadId,Point,BatchId,ClassId)values (" + _Incedentid + ",'" + _Title + "','" + _Dese + "','" + _InceDate.ToString("s") + "','" + Today.ToString("s") + "'," + _InceType + "," + _UserId + "," + _UserId + ",'" + _Type.ToLower() + "','Approved'," + _AssoUser + "," + _HeadId + "," + _Point + "," + _BatchId + "," + _ClassId + " )";
            m_MysqlDb.ExecuteQuery(sql);

        }

        public string GetStudentMangMenuString(int _roleid)
        {
            //CLogging logger = CLogging.GetLogObject();
            string _MenuStr;
           
            if (m_IncedentMenuStr == "")
            {


                _MenuStr = "<ul><li><a href=\"SearchStudent.aspx\">Search Student</a></li>";
               // logger.LogToFile("GetStudentMangMenuString", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                string sql = "SELECT DISTINCT tblaction.ActionName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND tblroleactionmap.ModuleId=2 AND tblaction.ActionType='Link'";
                //logger.LogToFile("GetStudentMangMenuString", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    //logger.LogToFile("GetStudentMangMenuString", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                    while (m_MyReader.Read())
                    {
                        _MenuStr = _MenuStr + "<li><a href=\"" + m_MyReader.GetValue(1).ToString() + "\">" + m_MyReader.GetValue(0).ToString() + "</a></li>";
                    }

                }
                _MenuStr = _MenuStr + "</ul>";
               // logger.LogToFile("GetStudentMangMenuString", " Closing MyReader ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader.Close();
                m_IncedentMenuStr = _MenuStr;

            }
            else
            {
                _MenuStr = m_IncedentMenuStr;
            }
           // logger.LogToFile("GetStudentMangMenuString", "Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            return _MenuStr;

        }

        public string GetSubStudentMangMenuString(int _roleid)
        {
           // CLogging logger = CLogging.GetLogObject();
            string _MenuStr;
            if (m_SubIncidentMenuStr == "")
            {
                _MenuStr = "<ul class=\"block\"><li><a href=\"StudentDetails.aspx\">Student Details</a></li>";
               // logger.LogToFile(" GetSubStudentMangMenuString", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                string sql = "SELECT DISTINCT tblaction.ActionName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND tblroleactionmap.ModuleId=2 AND tblaction.ActionType='SubLink'";
                //logger.LogToFile("GetSubStudentMangMenuString", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                   // logger.LogToFile("GetSubStudentMangMenuString", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                    while (m_MyReader.Read())
                    {
                        _MenuStr = _MenuStr + "<li><a href=\"" + m_MyReader.GetValue(1).ToString() + "\">" + m_MyReader.GetValue(0).ToString() + "</a></li>";
                    }

                }
                _MenuStr = _MenuStr + "</ul>";
                //logger.LogToFile("GetSubStudentMangMenuString", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader.Close();
                m_SubIncidentMenuStr = _MenuStr;

            }
            else
            {
                _MenuStr = m_SubIncidentMenuStr;
            }
           // logger.LogToFile("GetSubStudentMangMenuString", "Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Close();
            return _MenuStr;
        }

        public string GetIncedentMangMenuString(int _roleid)
        {
            string _MenuStr;

            if (m_IncedentMenuStr == "")
            {


                _MenuStr = "<ul><li><a href=\"WinSchoolHome.aspx\">WinEr Home</a></li>";
                // logger.LogToFile("GetStudentMangMenuString", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                string sql = "SELECT DISTINCT tblaction.ActionName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND tblroleactionmap.ModuleId=20 AND tblaction.ActionType='HomeLink'";
                //logger.LogToFile("GetStudentMangMenuString", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    //logger.LogToFile("GetStudentMangMenuString", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                    while (m_MyReader.Read())
                    {
                        _MenuStr = _MenuStr + "<li><a href=\"" + m_MyReader.GetValue(1).ToString() + "\">" + m_MyReader.GetValue(0).ToString() + "</a></li>";
                    }

                }
                _MenuStr = _MenuStr + "</ul>";
                // logger.LogToFile("GetStudentMangMenuString", " Closing MyReader ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader.Close();
                m_IncedentMenuStr = _MenuStr;

            }
            else
            {
                _MenuStr = m_IncedentMenuStr;
            }
            // logger.LogToFile("GetStudentMangMenuString", "Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            m_MyReader.Close();
            return _MenuStr;
        }



        public bool HasApprovalRight(int _UserId)
        {
            bool _valid = false;
          //string sql = "select tblroleactionmap.ActionId from tblroleactionmap inner join tbluser on tbluser.RoleId = tblroleactionmap.RoleId  inner join  tblaction on tblaction.Id = tblroleactionmap.ActionId   where tbluser.Id=" + _UserId + " and tblaction.ActionName ='Approve Incedents'";
            string sql = "select tblroleactionmap.RoleId from tblroleactionmap INNER join tblaction on tblaction.Id = tblroleactionmap.ActionId  inner join tbluser on tbluser.RoleId = tblroleactionmap.RoleId inner join tblincedent on tblincedent.ApproverId = tbluser.Id  where tblaction.ActionName = 'Approve Incidents' and tblincedent.ApproverId = " + _UserId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _valid = true;
            }
            m_MyReader.Close();
            return _valid;
        }

        public bool HasIncidents(int _UserId)
        {
            bool _valid = false;
            string sql = "select tblincidentqueue.Id from tblincidentqueue where tblincidentqueue.ApproverId=" + _UserId + " or tblincidentqueue.ApproverId=0";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _valid = true;
            }
            m_MyReader.Close();
            return _valid;

        }

        public void UpdateStatus(int _incidentId,int UserId,int BatchId)
        {
            string sql = "update tblincedent set tblincedent.Status='Approved', tblincedent.ApproverId=" + UserId + ",tblincedent.BatchId=" + BatchId + " where tblincedent.Id=" + _incidentId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            //sql = "delete from tblincidentqueue where tblincidentqueue.IncidentId=" + _incidentId + "";
            //m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            m_MyReader.Close();
        }

        public void RejectIncident(int _incidentId, int UserId, int BatchId)
        {
            string sql = "update tblincedent set tblincedent.Status='Rejected', tblincedent.ApproverId=" + UserId + ",tblincedent.BatchId=" + BatchId + " where tblincedent.Id=" + _incidentId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            //sql = "delete from tblincidentqueue where tblincidentqueue.IncidentId=" + _incidentId + "";
            //m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            m_MyReader.Close();


        }

        public string GetPupilname(int _incidentId, string _userType)
        {
            string sql;
            string _PuilName = "";
            if (_userType == "student")
            {
                sql = "select tblstudent.StudentName from tblstudent inner join tblincedent on tblincedent.AssoUser =  tblstudent.Id where tblincedent.Id = " + _incidentId + "";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _PuilName = m_MyReader.GetValue(0).ToString();

                }
            }
            else if (_userType == "staff")
            {
                sql = "select tblview_user.SurName from tblview_user inner join tblincedent on tblincedent.AssoUser =  tblview_user.Id where tblincedent.Id = " + _incidentId + "";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _PuilName = m_MyReader.GetValue(0).ToString();

                }
            }
            else if (_userType == "Class")
            {
                sql = "select tblclass.ClassName from tblclass inner join tblincedent on tblincedent.AssoUser=tblclass.Id WHERE tblincedent.Id=" + _incidentId + " ";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _PuilName = m_MyReader.GetValue(0).ToString();

                }


            }
            m_MyReader.Close();
            return _PuilName;
        }

        public string GetPupilType(int _incidentId)
        {
            string _PuilName = "";
            string sql = "select tblincedent.UserType from tblincedent where tblincedent.Id = " + _incidentId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _PuilName = m_MyReader.GetValue(0).ToString();

            }
            m_MyReader.Close();
            return _PuilName;
        }

        public bool OnLIineApprovel(int _IncidentId ,out string message)
        {
            message = "";
            bool _validate =false;
            string sql = "select tblincidentqueue.UserType from tblincidentqueue where tblincidentqueue.IncidentId=" + _IncidentId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {

                _validate = true;
            }
            else
            {
                message = "Sorry the incident was already approved";
            }
            m_MyReader.Close();
            return _validate;

        }


        public bool OnLineReject(int _IncidentId, out string message)
        {
            message = "";
            bool _validate = false;
            string sql = "select tblincidentqueue.UserType from tblincidentqueue where tblincidentqueue.IncidentId=" + _IncidentId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {

                _validate = true;
            }
            else
            {
                message = "Sorry the incident was already Rejected";
            }
            m_MyReader.Close();
            return _validate;
            
        }

        public bool AlreadyDeleted(int _Incidentid, out string message)
        {
            message = "";
            bool _validate = false;
            string sql = "select tblincedent.UserType from tblincedent where tblincedent.Id=" + _Incidentid + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {

                _validate = true;
            }
            else
            {
                message = "Sorry the incident was already Deleted";
            }
            m_MyReader.Close();
            return _validate;
        }

        public void Delete(int _IncidentId)
        {
            string sql = "delete from tblincedent where tblincedent.Id=" + _IncidentId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }

        public string LoadIncidenceData(int _UserId, string _Type, int _BatchId)
        {
            int _N_IncidentType = 0;
            StringBuilder _IncidentDat = new StringBuilder("");
            DataSet _Mydata_IncidentType;
            int MaxRows, MaxColumn = 3;
            int ExtraRow = 0, i, j;
            int _TypePtr = 0;
            DataRow dr_IncidentType;

            string sql = "select count(tblincedenttype.Id) from tblincedenttype  where  tblincedenttype.IncidentType='NORMAL' ";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _N_IncidentType = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            sql = "select tblincedenttype.Id , tblincedenttype.`Type` from tblincedenttype where tblincedenttype.Visibility = 1  and  tblincedenttype.IncidentType='NORMAL'  order by tblincedenttype.`Order` ";
            _Mydata_IncidentType = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_Mydata_IncidentType != null && _Mydata_IncidentType.Tables != null && _Mydata_IncidentType.Tables[0].Rows.Count > 0)
            {

                if (_N_IncidentType > 0)
                {

                    MaxRows = _N_IncidentType / 3;
                    ExtraRow = _N_IncidentType % 3;
                    if (ExtraRow > 0)
                    {
                        MaxRows = MaxRows + 1;
                    }
                    _IncidentDat.Append("<div class=\"container\">");
                    for (i = 1; i <= MaxRows; i++)
                    {
                        _IncidentDat.Append("<div class=\"row\">");
                        //Count = Count +1;
                        if (i == MaxRows)
                            MaxColumn = ExtraRow;
                        for (j = 0; j < MaxColumn; j++)
                        {

                            dr_IncidentType = _Mydata_IncidentType.Tables[0].Rows[_TypePtr];
                            _IncidentDat.Append(GetIncidentBlockStr(_UserId, _Type, int.Parse(dr_IncidentType[0].ToString()), dr_IncidentType[1].ToString(), _BatchId));

                            _TypePtr++;
                        }
                        _IncidentDat.Append("</div>");
                    }
                    _IncidentDat.Append("</div>");
                }
            }
            return _IncidentDat.ToString();
        }

         private string GetIncidentBlockStr(int _UserId, string _Type, int _IncTypeId, string _IncType, int _BatchId)
        {
            StringBuilder _IncidentBlockStruct = new StringBuilder("<div class=\"row\" style=\"width:100%\"><div class=\"container skin1 \" > <table cellpadding=\"0\" cellspacing=\"0\" class=\"containerTable\"> <tr class=\"top\"><td class=\"no\"> </td> <td class=\"n\">  " + _IncType + " </td> <td class=\"ne\"> </td></tr><tr class=\"middle\"> <td class=\"o\"> </td> 	<td class=\"c\"> <div class =\"IncBlock\">");

            _IncidentBlockStruct.Append(GetBlockData(_UserId, _Type, _IncTypeId, _IncType, _BatchId));
            _IncidentBlockStruct.Append("</div></td> <td class=\"e\"> </td> </tr><tr class=\"bottom\"> <td class=\"so\"> </td> <td class=\"s\"></td> <td class=\"se\"> </td> </tr> 	</table> </div></div>");

            return _IncidentBlockStruct.ToString();

        }

        private string GetBlockData(int _UserId, string _Type, int _IncTypeId, string _IncType, int _BatchId)
        {
            StringBuilder _IncidentBlockData = new StringBuilder("");
            DataSet _Mydata_IncidentData = GetPupilIncidentData(_UserId, _Type, _IncTypeId, _BatchId);
            DateTime _IncidentDate;
            int _index = 1;
            if (_Mydata_IncidentData != null && _Mydata_IncidentData.Tables != null && _Mydata_IncidentData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr_IncidentData in _Mydata_IncidentData.Tables[0].Rows)
                {
                    if (_index < 4)
                    {
                        _IncidentDate = DateTime.Parse(dr_IncidentData[3].ToString());
                        _IncidentBlockData.Append("<b>" + _IncidentDate.Date.ToString("dd-MM-yyyy") + ")" + dr_IncidentData[1].ToString() + "...</b><br />&nbsp; <a href=\"javascript:openIncpopup('ViewIncidence.aspx?id=" + dr_IncidentData[0].ToString() + "&Type=" + _Type + "')\">" + dr_IncidentData[2].ToString() + "...</a><br />");
                    }
                    _index++;
                }
                if (_Type == "student")
                {
                _IncidentBlockData.Append("<div style=\"text-align:right\"><a href=\"ViewIncidenceByType.aspx?id= " + _IncTypeId + "&Type= " + _Type + "\">more...</a> </div>");
                }
                else if (_Type == "Staff")
                {
                    _IncidentBlockData.Append("<div style=\"text-align:right\"><a href=\"ViewStaffIncidenceByType.aspx?id= " + _IncTypeId + "&Type= " + _Type + "\">more...</a> </div>");
                }
                else if (_Type == "Class")
                {
                    _IncidentBlockData.Append("<div style=\"text-align:right\"><a href=\"ViewClassIncidentsByType.aspx?id= " + _IncTypeId + "&Type= " + _Type + "\">more...</a> </div>");
                }

            }
            else
            {
                _IncidentBlockData.Append("<b>No incident reported</b>");
            }



            return _IncidentBlockData.ToString();
        }

        private DataSet GetPupilIncidentData(int _UserId, string _Type, int _IncTypeId,int _BatchId)
        {
            string sql;
            DataSet _Mydata_IncidentData = null;
            if (_Type == "student")
            {
                sql = "select tblview_incident.Id , MID(tblview_incident.Title,1,25) , MID(tblview_incident.Description,1,60) , tblview_incident.IncedentDate from tblview_incident inner join tblview_student on tblview_student.Id = tblview_incident.AssoUser where tblview_incident.UserType = '" + _Type + "' and tblview_incident.TypeId =" + _IncTypeId + "  and tblview_incident.`Status` ='Approved' and tblview_student.Id = " + _UserId + " and  tblview_incident.BatchId="+_BatchId+" order by tblview_incident.CreatedDate DESC";
                _Mydata_IncidentData = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            }
            else if (_Type == "Staff")
            {
                sql = "select tblview_incident.Id , MID(tblview_incident.Title,1,25) , MID(tblview_incident.Description,1,60) , tblview_incident.IncedentDate from tblview_incident inner join tblview_user on tblview_user.Id = tblview_incident.AssoUser where tblview_incident.UserType = '" + _Type + "' and tblview_incident.TypeId =" + _IncTypeId + "   and tblview_incident.`Status` ='Approved' and tblview_user.Id = " + _UserId + " and  tblview_incident.BatchId=" + _BatchId + " order by tblview_incident.CreatedDate DESC";
                _Mydata_IncidentData = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            }
            else if (_Type == "Class")
            {
                sql = "select tblview_incident.Id , MID(tblview_incident.Title,1,25) , MID(tblview_incident.Description,1,60) , tblview_incident.IncedentDate from tblview_incident inner join tblclass on tblclass.Id = tblview_incident.AssoUser where tblview_incident.UserType = '" + _Type + "' and tblview_incident.TypeId =" + _IncTypeId + "   and tblview_incident.`Status` ='Approved' and tblclass.Id = " + _UserId + " and  tblview_incident.BatchId=" + _BatchId + " order by tblview_incident.CreatedDate DESC";
                _Mydata_IncidentData = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            }
            return _Mydata_IncidentData;
        }


        public string GetCreatedUser(int _IncidentId)
        {
             string _UserName = "";
             string sql = "select tblview_user.SurName from tblincedent inner join tblview_user on tblview_user.Id = tblincedent.CreatedUserId where tblincedent.Id = " + _IncidentId + "";
             m_MyReader = m_MysqlDb.ExecuteQuery(sql);
             if (m_MyReader.HasRows)
             {
                 _UserName = m_MyReader.GetValue(0).ToString();
             }
             m_MyReader.Close();
             return _UserName;
        }

        public bool SameUserName(int ApproverId , int UserId)
        {
            bool _Valid = false;
            if (ApproverId == UserId)
            {
                _Valid = true;
            }
            return _Valid;
        }

        #region LIVE INCEDENT SEARCH 14/09/10
         
        public DataSet getIncedentInfo(string keyword, int history)
        {

            string sql = "", IncidentTbl = "tblincedent";
            DataSet mydataset = new DataSet();
            if (history == 1)
                IncidentTbl = "tblview_incident";

            int wrdcount = GetNoOfWords(keyword);
            return FindForUSerInString1(keyword, wrdcount, history);
        
        }


        public DataSet getDeepInfo(string keyword, int history)
        {
            string sql = "";
            DataSet mydataset = new DataSet();
            int wrdcount = GetNoOfWords(keyword);
            return FindForUSerInString(keyword, wrdcount, history);
        }


        public int GetNoOfWords(string s)
        {
            return s.Split(new char[] { ' '}, StringSplitOptions.RemoveEmptyEntries).Length;
        }



        private DataSet FindForUSerInString1(string kewrd, int wcount, int history)
        {
            string kewrdStrng = kewrd;
            string IncidentTbl = "tblincedent";
            if (history == 1)
                IncidentTbl = "tblview_incident";
            string[] a = new string[wcount];
            a = kewrdStrng.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string sql = "", inc_usrqry = "", inc_titleqry = "", inc_typeqry = "", inc_descrpnqry = "";
            string keyword = "", user = "", srchqry = "", inc_usrdfltqry = "", selctnStrng = ""; ;

            inc_usrdfltqry = "select " + IncidentTbl + ".Id , " + IncidentTbl + ".assouser as StudId, " + IncidentTbl + ".title, " + IncidentTbl + ".description,   " + IncidentTbl + ".UserType,DATE_FORMAT(" + IncidentTbl + ".IncedentDate,'%d/%m/%Y') as idate, DATE_FORMAT(" + IncidentTbl + ".CreatedDate,'%d/%m/%Y') as cdate,    tblview_user.SurName as CreatedBy, " + IncidentTbl + ".Point from " + IncidentTbl + "  inner join tblview_user on tblview_user.Id = " + IncidentTbl + ".CreatedUserId     where " + IncidentTbl + ".Status = 'Approved' and " + IncidentTbl + ".AssoUser in (  select id from tblview_student where Studentname like '%" + kewrd + "%' union  select id from tbluser where surname like '%" + kewrd + "%')";
            inc_usrqry = "" + IncidentTbl + ".AssoUser in  (select id from tblview_student where Studentname like '%" + kewrdStrng + "%'  union   select id from tblview_user where surname like '%" + kewrdStrng + "%')";
            inc_typeqry = "(select " + IncidentTbl + ".Id , " + IncidentTbl + ".assouser as StudId, " + IncidentTbl + ".title,       " + IncidentTbl + ".description, " + IncidentTbl + ".UserType, DATE_FORMAT(" + IncidentTbl + ".IncedentDate,'%d/%m/%Y') as idate,         DATE_FORMAT(" + IncidentTbl + ".CreatedDate,'%d/%m/%Y') as cdate,  tblview_user.SurName as CreatedBy, " + IncidentTbl + ".Point from " + IncidentTbl + "          inner join tblview_user on tblview_user.Id = " + IncidentTbl + ".CreatedUserId  inner join tblincedenttype on tblincedenttype.Id in          (select id from tblincedenttype  where type like '%" + kewrd + "%') where " + IncidentTbl + ".Status = 'Approved'  and  tblincedenttype.IncidentType='NORMAL' ";
            inc_titleqry = "(select " + IncidentTbl + ".Id , " + IncidentTbl + ".assouser as StudId, " + IncidentTbl + ".title, " + IncidentTbl + ".description,   " + IncidentTbl + ".UserType,DATE_FORMAT(" + IncidentTbl + ".IncedentDate,'%d/%m/%Y') as idate, DATE_FORMAT(" + IncidentTbl + ".CreatedDate,'%d/%m/%Y') as cdate,     tblview_user.SurName as CreatedBy, " + IncidentTbl + ".Point from " + IncidentTbl + "  inner join tblview_user on tblview_user.Id = " + IncidentTbl + ".CreatedUserId        where " + IncidentTbl + ".Status = 'Approved' and " + IncidentTbl + ".title like '%" + kewrd + "%' )";

            srchqry = inc_typeqry + "and " + inc_usrqry + "union" + inc_titleqry + "and " + inc_usrqry ;

            string SrchKwrd = "";
            int i = wcount,stud=0;
            int j = 0, flg = 0;
            string REGEX = @"([\w]+\s+){" + wcount + "}";
            if (wcount > 1)
            {
                keyword = a[wcount - 1];

                if (wcount > 1)
                {
                    for (j = i - 1; j >= 0; j--)
                    {
                        REGEX = @"([\w]+\s+){" + j + "}";
                        SrchKwrd = Regex.Match(kewrdStrng, REGEX).Value;
                        SrchKwrd = SrchKwrd + a[j];
                        //sql = "  select id from tblstudent where Studentname like '%" + SrchKwrd + "%' union  select id from tbluser where surname like '%" + SrchKwrd + "%'";
                        sql = "  select id from tblview_student where Studentname like '%" + SrchKwrd + "%' union  select id from tblview_user where surname like '%" + SrchKwrd + "%'";

                        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                        if (m_MyReader.HasRows)
                        {
                            stud = 1;
                            keyword = "";
                            flg = 1;
                            user = SrchKwrd;
                            for (int k = j + 1; k < i; k++)
                            {
                                keyword = keyword + " " + a[k];
                            }
                            break;

                        }
                    }
                    if (flg == 0)
                    {
                        
                        keyword = kewrd;
                        user = kewrd;
                    }
                }
                else
                {
                    keyword = kewrd;
                    user = kewrd;

                }
                keyword = keyword.TrimEnd().TrimStart();

                inc_usrdfltqry = "select " + IncidentTbl + ".Id , " + IncidentTbl + ".assouser as StudId, " + IncidentTbl + ".title, " + IncidentTbl + ".description,   " + IncidentTbl + ".UserType,DATE_FORMAT(" + IncidentTbl + ".IncedentDate,'%d/%m/%Y') as idate, DATE_FORMAT(" + IncidentTbl + ".CreatedDate,'%d/%m/%Y') as cdate,    tblview_user.SurName as CreatedBy, " + IncidentTbl + ".Point from " + IncidentTbl + "  inner join tblview_user on tblview_user.Id = " + IncidentTbl + ".CreatedUserId     where " + IncidentTbl + ".Status = 'Approved' and  " + IncidentTbl + ".AssoUser in (  select id from tblview_student where Studentname like '%" + user + "%' union  select id from tblview_user where surname like '%" + user + "%')";
                inc_usrqry = "" + IncidentTbl + ".AssoUser in  (select id from tblview_student where Studentname like '%" + user + "%'  union   select id from tblview_user where surname like '%" + user + "%')";
                inc_typeqry = "(select " + IncidentTbl + ".Id , " + IncidentTbl + ".assouser as StudId, " + IncidentTbl + ".title,       " + IncidentTbl + ".description, " + IncidentTbl + ".UserType, DATE_FORMAT(" + IncidentTbl + ".IncedentDate,'%d/%m/%Y') as idate,         DATE_FORMAT(" + IncidentTbl + ".CreatedDate,'%d/%m/%Y') as cdate,  tblview_user.SurName as CreatedBy, " + IncidentTbl + ".Point from " + IncidentTbl + "          inner join tblview_user on tblview_user.Id = " + IncidentTbl + ".CreatedUserId  inner join tblincedenttype on tblincedenttype.Id in          (select id from tblincedenttype  where type like '%" + keyword + "%') where " + IncidentTbl + ".Status = 'Approved' and  tblincedenttype.IncidentType='NORMAL' ";
                inc_titleqry = "(select " + IncidentTbl + ".Id , " + IncidentTbl + ".assouser as StudId, " + IncidentTbl + ".title, " + IncidentTbl + ".description,   " + IncidentTbl + ".UserType,DATE_FORMAT(" + IncidentTbl + ".IncedentDate,'%d/%m/%Y') as idate, DATE_FORMAT(" + IncidentTbl + ".CreatedDate,'%d/%m/%Y') as cdate,     tblview_user.SurName as CreatedBy, " + IncidentTbl + ".Point from " + IncidentTbl + "  inner join tblview_user on tblview_user.Id = " + IncidentTbl + ".CreatedUserId        where (" + IncidentTbl + ".Status = 'Approved' and " + IncidentTbl + ".title like '%" + keyword + "%' )";
                if (stud == 1)
                {
                    srchqry = inc_typeqry + "and " + inc_usrqry + ") union" + inc_titleqry + " and " + inc_usrqry + ") ";
                }
                else
                {
                    srchqry = inc_typeqry + "and " + inc_usrqry + ") union" + inc_titleqry + ")";
                }
                //    if (m_MysqlDb.ExecuteQuery(srchqry).HasRows)
                //    { }
                //    else
                //    {
                //        srchqry = inc_usrdfltqry;
                //        if (m_MysqlDb.ExecuteQuery(srchqry).HasRows)
                //        { }
                //        else
                //        {
                //            sql = "(select " + IncidentTbl + ".Id , " + IncidentTbl + ".assouser as StudId, " + IncidentTbl + ".title, " + IncidentTbl + ".description, " + IncidentTbl + ".UserType,DATE_FORMAT(" + IncidentTbl + ".IncedentDate,'%d/%m/%Y') as idate, DATE_FORMAT(" + IncidentTbl + ".CreatedDate,'%d/%m/%Y') as cdate,  tbluser.SurName as CreatedBy, " + IncidentTbl + ".Point from " + IncidentTbl + "  inner join tbluser on tbluser.Id = " + IncidentTbl + ".CreatedUserId   where " + IncidentTbl + ".title like '%" + kewrd + "%' )  union (select " + IncidentTbl + ".Id , " + IncidentTbl + ".assouser as StudId, " + IncidentTbl + ".title, " + IncidentTbl + ".description, " + IncidentTbl + ".UserType, DATE_FORMAT(" + IncidentTbl + ".IncedentDate,'%d/%m/%Y') as idate,  DATE_FORMAT(" + IncidentTbl + ".CreatedDate,'%d/%m/%Y') as cdate,  tbluser.SurName as CreatedBy," + IncidentTbl + ".Point from " + IncidentTbl + "  inner join tbluser on tbluser.Id = " + IncidentTbl + ".CreatedUserId  inner join tblincedenttype on tblincedenttype.Id in (select id from tblincedenttype  where type like '%" + kewrd + "%')  where tblincedenttype.Type like '%" + kewrd + "%' ) order by idate DESC ";
                //            srchqry = sql;
                //        }
                //    }

            }
            else
            {

                srchqry = inc_usrdfltqry + " union " + inc_typeqry + ") union " + inc_titleqry;
            }

            if (srchqry != "")
            {
                srchqry = srchqry + " ORDER BY idate";
            }

            return m_MysqlDb.ExecuteQueryReturnDataSet(srchqry );
        }


        private DataSet FindForUSerInString (string kewrd, int wcount, int history)
        {
            string kewrdStrng = kewrd;
            string  IncidentTbl = "tblincedent";
            if (history == 1)
                IncidentTbl = "tblview_incident";
            string [] a= new string[wcount];
            a=kewrdStrng.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string sql = "", inc_usrqry = "", inc_titleqry = "", inc_typeqry = "", inc_descrpnqry = "";
            string keyword="",user="", srchqry="",inc_usrdfltqry="",selctnStrng=""; ;

            inc_usrdfltqry = "select " + IncidentTbl + ".Id , " + IncidentTbl + ".assouser as StudId, " + IncidentTbl + ".title, " + IncidentTbl + ".description,   " + IncidentTbl + ".UserType,DATE_FORMAT(" + IncidentTbl + ".IncedentDate,'%d/%m/%Y') as idate, DATE_FORMAT(" + IncidentTbl + ".CreatedDate,'%d/%m/%Y') as cdate,    tblview_user.SurName as CreatedBy, " + IncidentTbl + ".Point from " + IncidentTbl + "  inner join tblview_user on tblview_user.Id = " + IncidentTbl + ".CreatedUserId     where " + IncidentTbl + ".Status = 'Approved' and " + IncidentTbl + ".AssoUser in (  select id from tblview_student where Studentname like '%" + kewrd + "%' union  select id from tblview_user where surname like '%" + kewrd + "%')";
            inc_usrqry = "" + IncidentTbl + ".AssoUser in  (select id from tblview_student where Studentname like '%" + kewrdStrng + "%'  union   select id from tblview_user where surname like '%" + kewrdStrng + "%')";
            inc_typeqry = "(select " + IncidentTbl + ".Id , " + IncidentTbl + ".assouser as StudId, " + IncidentTbl + ".title,       " + IncidentTbl + ".description, " + IncidentTbl + ".UserType, DATE_FORMAT(" + IncidentTbl + ".IncedentDate,'%d/%m/%Y') as idate,         DATE_FORMAT(" + IncidentTbl + ".CreatedDate,'%d/%m/%Y') as cdate,  tblview_user.SurName as CreatedBy, " + IncidentTbl + ".Point from " + IncidentTbl + "          inner join tblview_user on tblview_user.Id = " + IncidentTbl + ".CreatedUserId  inner join tblincedenttype on tblincedenttype.Id in          (select id from tblincedenttype  where type like '%" + kewrd + "%') where " + IncidentTbl + ".Status = 'Approved'  and  tblincedenttype.IncidentType='NORMAL' ";
            inc_titleqry = "(select " + IncidentTbl + ".Id , " + IncidentTbl + ".assouser as StudId, " + IncidentTbl + ".title, " + IncidentTbl + ".description,   " + IncidentTbl + ".UserType,DATE_FORMAT(" + IncidentTbl + ".IncedentDate,'%d/%m/%Y') as idate, DATE_FORMAT(" + IncidentTbl + ".CreatedDate,'%d/%m/%Y') as cdate,     tblview_user.SurName as CreatedBy, " + IncidentTbl + ".Point from " + IncidentTbl + "  inner join tblview_user on tblview_user.Id = " + IncidentTbl + ".CreatedUserId        where " + IncidentTbl + ".Status = 'Approved' and " + IncidentTbl + ".title like '%" + kewrd + "%' )";
            inc_descrpnqry = " (select " + IncidentTbl + ".Id , " + IncidentTbl + ".assouser as StudId, " + IncidentTbl + ".title, " + IncidentTbl + ".description, " + IncidentTbl + ".UserType,DATE_FORMAT(" + IncidentTbl + ".IncedentDate,'%d/%m/%Y') as idate, DATE_FORMAT(" + IncidentTbl + ".CreatedDate,'%d/%m/%Y') as cdate,  tblview_user.SurName as CreatedBy, " + IncidentTbl + ".Point from " + IncidentTbl + "  inner join tblview_user on tblview_user.Id = " + IncidentTbl + ".CreatedUserId   where " + IncidentTbl + ".Status = 'Approved' and " + IncidentTbl + ".Description like '%" + kewrd + "%' )";

            srchqry =  inc_typeqry + "and " + inc_usrqry + "union" + inc_titleqry + "and " + inc_usrqry + "union " + inc_descrpnqry + "and " + inc_usrqry   ;

            string SrchKwrd = "";
            int i=wcount;
            int j = 0,flg=0;
            string REGEX = @"([\w]+\s+){" + wcount + "}";
            if (wcount > 1)
            {
                keyword = a[wcount - 1];

                if (wcount > 1)
                {
                    for (j = i-1; j >= 0; j--)
                    {
                        REGEX = @"([\w]+\s+){" + j + "}";
                        SrchKwrd = Regex.Match(kewrdStrng, REGEX).Value;
                        SrchKwrd = SrchKwrd + a[j];
                        //sql = "  select id from tblstudent where Studentname like '%" + SrchKwrd + "%' union  select id from tbluser where surname like '%" + SrchKwrd + "%'";
                        sql = "  select id from tblview_student where Studentname like '%" + SrchKwrd + "%' union  select id from tblview_user where surname like '%" + SrchKwrd + "%'";

                        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                        if (m_MyReader.HasRows)
                        {
                            keyword = "";
                            flg = 1;
                            user = SrchKwrd;
                            for (int k = j+1; k < i; k++)
                            {
                                keyword = keyword+" "+ a[k];
                            }
                            break;
                            
                        }
                    }
                    if (flg == 0)
                    {
                        keyword = kewrd;
                        user = kewrd;
                    }
                }
                else
                {
                    keyword = kewrd;
                    user = kewrd;

                }
                keyword = keyword.TrimEnd().TrimStart();

                inc_usrdfltqry = "select " + IncidentTbl + ".Id , " + IncidentTbl + ".assouser as StudId, " + IncidentTbl + ".title, " + IncidentTbl + ".description,   " + IncidentTbl + ".UserType,DATE_FORMAT(" + IncidentTbl + ".IncedentDate,'%d/%m/%Y') as idate, DATE_FORMAT(" + IncidentTbl + ".CreatedDate,'%d/%m/%Y') as cdate,    tblview_user.SurName as CreatedBy, " + IncidentTbl + ".Point from " + IncidentTbl + "  inner join tblview_user on tblview_user.Id = " + IncidentTbl + ".CreatedUserId     where " + IncidentTbl + ".Status = 'Approved' and " + IncidentTbl + ".AssoUser in (  select id from tblview_student where Studentname like '%" + user + "%' union  select id from tblview_user where surname like '%" + user + "%')";
                inc_usrqry = "" + IncidentTbl + ".AssoUser in  (select id from tblview_student where Studentname like '%" + user + "%'  union   select id from tblview_user where surname like '%" + user + "%')";
                inc_typeqry = "(select " + IncidentTbl + ".Id , " + IncidentTbl + ".assouser as StudId, " + IncidentTbl + ".title,       " + IncidentTbl + ".description, " + IncidentTbl + ".UserType, DATE_FORMAT(" + IncidentTbl + ".IncedentDate,'%d/%m/%Y') as idate,         DATE_FORMAT(" + IncidentTbl + ".CreatedDate,'%d/%m/%Y') as cdate,  tblview_user.SurName as CreatedBy, " + IncidentTbl + ".Point from " + IncidentTbl + "          inner join tblview_user on tblview_user.Id = " + IncidentTbl + ".CreatedUserId  inner join tblincedenttype on tblincedenttype.Id in          (select id from tblincedenttype  where type like '%" + keyword + "%') where " + IncidentTbl + ".Status = 'Approved'  and  tblincedenttype.IncidentType='NORMAL' ";
                inc_titleqry = "(select " + IncidentTbl + ".Id , " + IncidentTbl + ".assouser as StudId, " + IncidentTbl + ".title, " + IncidentTbl + ".description,   " + IncidentTbl + ".UserType,DATE_FORMAT(" + IncidentTbl + ".IncedentDate,'%d/%m/%Y') as idate, DATE_FORMAT(" + IncidentTbl + ".CreatedDate,'%d/%m/%Y') as cdate,     tblview_user.SurName as CreatedBy, " + IncidentTbl + ".Point from " + IncidentTbl + "  inner join tblview_user on tblview_user.Id = " + IncidentTbl + ".CreatedUserId        where " + IncidentTbl + ".Status = 'Approved' and (" + IncidentTbl + ".title like '%" + keyword + "%' )";
                inc_descrpnqry = " (select " + IncidentTbl + ".Id , " + IncidentTbl + ".assouser as StudId, " + IncidentTbl + ".title, " + IncidentTbl + ".description, " + IncidentTbl + ".UserType,DATE_FORMAT(" + IncidentTbl + ".IncedentDate,'%d/%m/%Y') as idate, DATE_FORMAT(" + IncidentTbl + ".CreatedDate,'%d/%m/%Y') as cdate,  tblview_user.SurName as CreatedBy, " + IncidentTbl + ".Point from " + IncidentTbl + "  inner join tblview_user on tblview_user.Id = " + IncidentTbl + ".CreatedUserId   where " + IncidentTbl + ".Status = 'Approved' and (" + IncidentTbl + ".Description like '%" + keyword + "%' )";

                srchqry = inc_typeqry + "and " + inc_usrqry + ") union" + inc_titleqry + " and " + inc_usrqry + ") union " + inc_descrpnqry + "and " + inc_usrqry + " )";
                if (m_MysqlDb.ExecuteQuery(srchqry).HasRows)
                { }
                else
                {
                    srchqry = inc_usrdfltqry;
                    if (m_MysqlDb.ExecuteQuery(srchqry).HasRows)
                    { }
                    else
                    {
                        string deepQry = " (select " + IncidentTbl + ".Id , " + IncidentTbl + ".assouser as StudId, " + IncidentTbl + ".title, " + IncidentTbl + ".description, " + IncidentTbl + ".UserType,DATE_FORMAT(" + IncidentTbl + ".IncedentDate,'%d/%m/%Y') as idate, DATE_FORMAT(" + IncidentTbl + ".CreatedDate,'%d/%m/%Y') as cdate,  tbluser.SurName as CreatedBy, " + IncidentTbl + ".Point from " + IncidentTbl + "  inner join tbluser on tbluser.Id = " + IncidentTbl + ".CreatedUserId   where " + IncidentTbl + ".Description like '%" + kewrd + "%' )";
                        sql = "(select " + IncidentTbl + ".Id , " + IncidentTbl + ".assouser as StudId, " + IncidentTbl + ".title, " + IncidentTbl + ".description, " + IncidentTbl + ".UserType,DATE_FORMAT(" + IncidentTbl + ".IncedentDate,'%d/%m/%Y') as idate, DATE_FORMAT(" + IncidentTbl + ".CreatedDate,'%d/%m/%Y') as cdate,  tbluser.SurName as CreatedBy, " + IncidentTbl + ".Point from " + IncidentTbl + "  inner join tbluser on tbluser.Id = " + IncidentTbl + ".CreatedUserId   where " + IncidentTbl + ".title like '%" + kewrd + "%' )  union (select " + IncidentTbl + ".Id , " + IncidentTbl + ".assouser as StudId, " + IncidentTbl + ".title, " + IncidentTbl + ".description, " + IncidentTbl + ".UserType, DATE_FORMAT(" + IncidentTbl + ".IncedentDate,'%d/%m/%Y') as idate,  DATE_FORMAT(" + IncidentTbl + ".CreatedDate,'%d/%m/%Y') as cdate,  tbluser.SurName as CreatedBy," + IncidentTbl + ".Point from " + IncidentTbl + "  inner join tbluser on tbluser.Id = " + IncidentTbl + ".CreatedUserId  inner join tblincedenttype on tblincedenttype.Id in (select id from tblincedenttype  where type like '%" + kewrd + "%')  where tblincedenttype.Type like '%" + kewrd + "%'  and    tblincedenttype.IncidentType='NORMAL' ) union " + deepQry;
                        srchqry = sql;
                    }
                }
            }
            else
            {

                srchqry = inc_usrdfltqry + " union " + inc_typeqry + ") union " + inc_titleqry + " union " + inc_descrpnqry;
            }
            if (srchqry != "")
            {
              srchqry=srchqry+  " ORDER BY idate";
            }
            return m_MysqlDb.ExecuteQueryReturnDataSet(srchqry);
        }

        #endregion

        public string GetPupilTypeForPreviousBatch(int _incidentId)
        {
            string _PuilName = "";
            string sql = "select tblview_incident.UserType from tblview_incident where tblview_incident.Id = " + _incidentId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _PuilName = m_MyReader.GetValue(0).ToString();

            }
            m_MyReader.Close();
            return _PuilName;
        }

        public string GetPupilnameForPreviousBatch(int _incidentId, string _userType)
        {
            string sql;
            string _PuilName = "";
            if (_userType == "student")
            {
                sql = "select tblstudent.StudentName from tblstudent inner join tblview_incident on tblview_incident.AssoUser =  tblstudent.Id where tblview_incident.Id = " + _incidentId;
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _PuilName = m_MyReader.GetValue(0).ToString();
                }
            }
            else if (_userType == "Staff")
            {
                sql = "select tbluser.SurName from tbluser inner join tblview_incident on tblview_incident.AssoUser =  tbluser.Id where tblview_incident.Id = " + _incidentId;
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _PuilName = m_MyReader.GetValue(0).ToString();
                }
            }
            else if (_userType == "Class")
            {
                sql = "select tblclass.ClassName from tblclass inner join tblview_incident on tblview_incident.AssoUser=tblclass.Id WHERE tblview_incident.Id=" + _incidentId;
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _PuilName = m_MyReader.GetValue(0).ToString();
                }
            }
            m_MyReader.Close();
            return _PuilName;
        }

        public void Reportincident(string IncidentCinfigType, int parameter1, int parameter2, string ValueDetails, int AssociatedUserId, string UserType, int ClassId, int BatchId, int UserId, int ActionId,string Reference)
        {
            bool NeedApproval = false;
            string CalculationType = "", Fixedvalue = "", IncedentDesc = "",Titlename="";
            int TitleId=0,LowerLimit=0, UpperLimit=0, TypeId=0;
            double Point_Value = 0;
            if (IsincidentEnabled(IncidentCinfigType,ActionId, out CalculationType, out Fixedvalue, out IncedentDesc, out LowerLimit, out UpperLimit, out Point_Value, out TitleId, out Titlename, out NeedApproval, out TypeId))
            {
                double points = 0;
                if (ValueDetails == "Fixed")
                {
                    points = Math.Round(Point_Value,0);
                }
                else
                {
                    points = CalculateincidentPoints(IncidentCinfigType, parameter1, parameter2, Point_Value);
                }
                int ApproverId = UserId;
                string Status = "Approved";
                if (IncidentCinfigType == "StudentAbsentEquation")
                {
                    IncedentDesc = IncedentDesc.Replace("($AbsentCount$)", parameter2.ToString());
                    IncedentDesc = IncedentDesc.Replace("($Days$)", ValueDetails);
                }
                if (IncidentCinfigType == "RankEquation")
                {
                    IncedentDesc = IncedentDesc.Replace("($Rank$)", parameter2.ToString());
                    IncedentDesc = IncedentDesc.Replace("($Subject:Marks$)", ValueDetails);
                }

                if (IncidentCinfigType == "PaidAfterDueDate")
                {
                    string[] str = ValueDetails.Split(',');
                    IncedentDesc = IncedentDesc.Replace("($NoofDate$)", parameter2.ToString());
                    IncedentDesc = IncedentDesc.Replace("($Date$)", str[1]);
                    IncedentDesc = IncedentDesc.Replace("($FeeName$)", str[0]);
                }
                if (IncidentCinfigType == "PaidBeforeDueDate")
                {
                    string[] str = ValueDetails.Split(',');
                    IncedentDesc = IncedentDesc.Replace("($NoofDate$)", parameter2.ToString());
                    IncedentDesc = IncedentDesc.Replace("($Date$)", str[1]);
                    IncedentDesc = IncedentDesc.Replace("($FeeName$)", str[0]);
                }

                if (NeedApproval)
                {
                    ApproverId = 0;
                    Status = "Need Approval";
                }
                DeleteExistingIncedent(Reference);
                InsertIncident(Titlename, IncedentDesc, DateTime.Now, DateTime.Now, TypeId, ApproverId, UserId, UserType, Status, AssociatedUserId, TitleId, points, BatchId, ClassId, Reference);
            }


        }

        private void DeleteExistingIncedent(string Reference)
        {
            string sql = " DELETE FROM tblincedent WHERE tblincedent.Reference='"+Reference+"'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            m_MyReader.Close();
        }

        private void InsertIncident(string Titlename, string Description, DateTime IncedentDate, DateTime CreatedDate, int TypeId, int ApproverId, int UserId, string UserType, string Status, int AssociatedUserId, int HeadId, double points, int BatchId, int ClassId, string Reference)
        {
            General _GenObj = new General(m_MysqlDb);
            int _Incedentid = _GenObj.GetTableMaxId("tblview_incident", "Id");

            string sql = "insert into tblincedent (Id,Title,Description,IncedentDate,CreatedDate,TypeId,ApproverId,CreatedUserId,UserType,Status,AssoUser,HeadId,Point,BatchId,ClassId,Reference)values (" + _Incedentid + ",'" + Titlename + "','" + Description + "','" + IncedentDate.ToString("s") + "','" + CreatedDate.ToString("s") + "'," + TypeId + "," + ApproverId + "," + UserId + ",'" + UserType.ToLowerInvariant() + "','" + Status + "'," + AssociatedUserId + "," + HeadId + "," + points + "," + BatchId + "," + ClassId + ",'" + Reference + "')";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            m_MyReader.Close();
        }

        private double CalculateincidentPoints(string IncidentCinfigType, int parameter1, int parameter2, double Point_Value)
        {
            double points = 0;

            if (IncidentCinfigType == "StudentAbsentEquation")
            {
                points = (parameter1 * Point_Value) * -1;
            }
            if (IncidentCinfigType == "RankEquation")
            {
                points = ((parameter1 - parameter2) * Point_Value);
            }

            if (IncidentCinfigType == "PaidAfterDueDate")
            {
                points = (parameter1 * Point_Value) * -1;
            }

            if (IncidentCinfigType == "PaidBeforeDueDate")
            {
                points = (parameter1 * Point_Value);
            }

            points = Math.Round(points, 0);
            return points;
        }

        private bool IsincidentEnabled(string IncidentCinfigType,int ActionId,out string CalculationType, out string Fixedvalue, out string IncedentDesc, out int LowerLimit, out int UpperLimit, out double Point_Value, out int TitleId, out string Titlename,  out bool NeedApproval, out int TypeId)
        {
            bool Isenable = false, Done = false;
            NeedApproval = false;
            string IsEnable="";
            int Isactive = 0;
            CalculationType = ""; Fixedvalue = ""; IncedentDesc = ""; Titlename = "";
            TitleId = 0; LowerLimit = 0; UpperLimit = 0; Point_Value = 0; TypeId = 0;
            string sql = "SELECT tblincedenthead.Title,tblincedenthead.NeedApproval,tblincedenthead.IsActive,tblincedentconfig.CalculationType,tblincedentconfig.FixedValue,tblincedentconfig.LowerLimit,tblincedentconfig.UpperLimit,tblincedentconfig.TitleId,tblincedentconfig.IncedentDesc,tblincedentconfig.Point_Value,tblincedentconfig.IsEnable,tblincedenthead.TypeId FROM tblincedentconfig INNER JOIN tblincedenthead ON tblincedenthead.Id=tblincedentconfig.TitleId WHERE tblincedentconfig.`Type`='" + IncidentCinfigType + "' AND tblincedentconfig.ActionId="+ActionId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
             if (m_MyReader.HasRows)
             {
                 Titlename = m_MyReader.GetValue(0).ToString();
                 if (m_MyReader.GetValue(1).ToString().ToUpperInvariant() == "YES")
                 {
                     NeedApproval = true;
                 }
                 int.TryParse(m_MyReader.GetValue(2).ToString(), out Isactive);
                 CalculationType = m_MyReader.GetValue(3).ToString();
                 Fixedvalue = m_MyReader.GetValue(4).ToString();
                 int.TryParse(m_MyReader.GetValue(5).ToString(), out LowerLimit);
                 int.TryParse(m_MyReader.GetValue(6).ToString(), out UpperLimit);
                 int.TryParse(m_MyReader.GetValue(7).ToString(), out TitleId);
                 IncedentDesc = m_MyReader.GetValue(8).ToString();
                 double.TryParse(m_MyReader.GetValue(9).ToString(), out Point_Value);
                 IsEnable = m_MyReader.GetValue(10).ToString();
                 if (int.TryParse(m_MyReader.GetValue(11).ToString(), out TypeId))
                 {
                     if (TypeId > 0)
                     {
                         Done = true;
                     }
                 }
                 if (IsEnable.ToUpperInvariant() == "YES" && Isactive == 1)
                 {
                     Isenable = true;
                 }

             }
             if (!Done)
             {
                 if (ActionId != 0)
                 {
                     sql = "SELECT tblincedenthead.Title,tblincedenthead.NeedApproval,tblincedenthead.IsActive,tblincedentconfig.CalculationType,tblincedentconfig.FixedValue,tblincedentconfig.LowerLimit,tblincedentconfig.UpperLimit,tblincedentconfig.TitleId,tblincedentconfig.IncedentDesc,tblincedentconfig.Point_Value,tblincedentconfig.IsEnable,tblincedenthead.TypeId FROM tblincedentconfig INNER JOIN tblincedenthead ON tblincedenthead.Id=tblincedentconfig.TitleId WHERE tblincedentconfig.`Type`='" + IncidentCinfigType + "' AND tblincedentconfig.ActionId=0";
                     m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                     if (m_MyReader.HasRows)
                     {
                         Titlename = m_MyReader.GetValue(0).ToString();
                         if (m_MyReader.GetValue(1).ToString().ToUpperInvariant() == "YES")
                         {
                             NeedApproval = true;
                         }
                         int.TryParse(m_MyReader.GetValue(2).ToString(), out Isactive);
                         CalculationType = m_MyReader.GetValue(3).ToString();
                         Fixedvalue = m_MyReader.GetValue(4).ToString();
                         int.TryParse(m_MyReader.GetValue(5).ToString(), out LowerLimit);
                         int.TryParse(m_MyReader.GetValue(6).ToString(), out UpperLimit);
                         int.TryParse(m_MyReader.GetValue(7).ToString(), out TitleId);
                         IncedentDesc = m_MyReader.GetValue(8).ToString();
                         double.TryParse(m_MyReader.GetValue(9).ToString(), out Point_Value);
                         IsEnable = m_MyReader.GetValue(10).ToString();
                         int.TryParse(m_MyReader.GetValue(11).ToString(), out TypeId);
                         if (IsEnable.ToUpperInvariant() == "YES" && Isactive == 1)
                         {
                             Isenable = true;
                         }

                     }
                 }
             }
             return Isenable;
        }

        public int GetCurrentBatch_StaffIncidenceRating(int StaffId, int BatchId, out int Current_Batchpoints)
        {
            Current_Batchpoints = 0;
            int Current_BatchRating = 0;
            int Total_AllStaffsPoints = GetTotal_AllStaffsIncidentPoints(BatchId);
            int Total_No_Staffs = GetTotal_Staffs();
            if (Total_No_Staffs > 0)
            {
                int Current_Batch_Class_AVG = Total_AllStaffsPoints / Total_No_Staffs;
                string sql = "SELECT SUM(tblincedent.`Point`) FROM tblincedent WHERE tblincedent.UserType='Staff' AND tblincedent.Status='Approved' AND tblincedent.BatchId="+BatchId+" AND tblincedent.AssoUser="+StaffId;
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    int.TryParse(m_MyReader.GetValue(0).ToString(), out Current_Batchpoints);
                }
                Current_BatchRating = Current_Batchpoints - Current_Batch_Class_AVG;
            }
            return Current_BatchRating;
        }

        private int GetTotal_AllStaffsIncidentPoints(int BatchId)
        {
            int Totalpoints = 0;
            string sql = "SELECT SUM(tblincedent.`Point`) FROM tblincedent WHERE tblincedent.UserType='Staff' AND tblincedent.`Status`='Approved'  AND tblincedent.BatchId=" + BatchId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out Totalpoints);
            }
            return Totalpoints;
        }

        public int GetTotal_Staffs()
        {
            int Count = 0;
            string sql = "SELECT count(tbluser.UserName) FROM tbluser INNER JOIN tblrole ON tblrole.Id=tbluser.RoleId  WHERE tbluser.`Status`=1 AND tblrole.Type='Staff'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out Count);
            }
            return Count;
        }

        public int GetCurrentBatch_IncidenceRating(int StudentId, int ClassId, int BatchId,string IncedentTable, out int Current_Batchpoints)
        {
            Current_Batchpoints = 0;
            int Current_BatchRating = 0;
            int Total_ClassPoints = GetTotal_ClassIncidentPoints(ClassId, BatchId, IncedentTable);
            int Total_No_Students = GetTotal_StudentsInClass(ClassId, BatchId);
            if (Total_No_Students > 0)
            {
                int Current_Batch_Class_AVG = Total_ClassPoints / Total_No_Students;
                string sql = "SELECT SUM(" + IncedentTable + ".`Point`) FROM " + IncedentTable + " WHERE " + IncedentTable + ".UserType='student' AND " + IncedentTable + ".Status='Approved' AND " + IncedentTable + ".ClassId=" + ClassId + " AND " + IncedentTable + ".BatchId=" + BatchId + " AND " + IncedentTable + ".AssoUser=" + StudentId;
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    int.TryParse(m_MyReader.GetValue(0).ToString(), out Current_Batchpoints);
                }
                Current_BatchRating = Current_Batchpoints - Current_Batch_Class_AVG;
            }
            return Current_BatchRating;
        }

        public int GetTotal_StudentsInClass(int ClassId, int CurrentBatchId)
        {
            int count = 0;
            string sql = "SELECT Count(tblstudentclassmap.StudentId) FROM tblstudentclassmap INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudent.Status=1 AND tblstudentclassmap.ClassId=" + ClassId + " AND tblstudentclassmap.BatchId=" + CurrentBatchId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out count);
            }
            return count;
        }

        private int GetTotal_ClassIncidentPoints(int ClassId, int _CurrentBatchid, string IncedentTable)
        {
            int Totalpoints = 0;
            string sql = "SELECT SUM(" + IncedentTable + ".`Point`) FROM " + IncedentTable + " WHERE " + IncedentTable + ".UserType='student'  AND " + IncedentTable + ".Status='Approved' AND " + IncedentTable + ".ClassId=" + ClassId + " AND " + IncedentTable + ".BatchId=" + _CurrentBatchid;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out Totalpoints);
            }
            return Totalpoints;
        }


        public int GetStaffMonthlyRating(int Month, int StaffId, int BatchId)
        {
            int MonthlyPoints = 0;
            int MonthlyRating = 0;
            int Total_ClassPoints = GetMonthly_AllStaffIncidentPoints(BatchId, Month);
            int Total_No_User = GetTotal_Staffs();
            if (Total_No_User > 0)
            {
                int Current_Batch_Class_AVG = Total_ClassPoints / Total_No_User;
                string sql = "SELECT SUM(tblincedent.`Point`) FROM tblincedent WHERE tblincedent.UserType='Staff' AND tblincedent.Status='Approved' AND tblincedent.BatchId=" + BatchId + " AND tblincedent.AssoUser=" + StaffId + " AND Month(tblincedent.IncedentDate)=" + Month;
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    int.TryParse(m_MyReader.GetValue(0).ToString(), out MonthlyPoints);
                }
                MonthlyRating = MonthlyPoints - Current_Batch_Class_AVG;
            }
            return MonthlyRating;
        }

        public int GetMonthly_AllStaffIncidentPoints(int BatchId, int Month)
        {
            int Totalpoints = 0;
            string sql = "SELECT SUM(tblincedent.`Point`) FROM tblincedent WHERE tblincedent.UserType='Staff' AND tblincedent.`Status`='Approved'  AND tblincedent.BatchId=" + BatchId + " AND Month(tblincedent.IncedentDate)=" + Month;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out Totalpoints);
            }
            return Totalpoints;
        }


        public int GetMonthlyRating(string Type, int Month, int StudentId, int ClassId, int BatchId, string IncedentTable)
        {
            int MonthlyPoints = 0;
            int MonthlyRating = 0;
            int Total_ClassPoints = GetMonthly_ClassIncidentPoints(ClassId, BatchId, Month, IncedentTable);
            int Total_No_User = GetTotal_StudentsInClass(ClassId, BatchId);
            if (Total_No_User > 0)
            {
                int Current_Batch_Class_AVG = Total_ClassPoints / Total_No_User;
                string sql = "SELECT SUM(" + IncedentTable + ".`Point`) FROM " + IncedentTable + " WHERE " + IncedentTable + ".UserType='student' AND " + IncedentTable + ".Status='Approved' AND " + IncedentTable + ".ClassId=" + ClassId + " AND " + IncedentTable + ".BatchId=" + BatchId + " AND " + IncedentTable + ".AssoUser=" + StudentId + " AND Month(" + IncedentTable + ".IncedentDate)=" + Month;
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    int.TryParse(m_MyReader.GetValue(0).ToString(), out MonthlyPoints);
                }
                MonthlyRating = MonthlyPoints - Current_Batch_Class_AVG;
            }
            return MonthlyRating;
        }

        public int GetMonthly_ClassIncidentPoints(int ClassId, int BatchId, int Month, string IncedentTable)
        {
            int Totalpoints = 0;
            string sql = "SELECT SUM(" + IncedentTable + ".`Point`) FROM " + IncedentTable + " WHERE " + IncedentTable + ".UserType='student' AND " + IncedentTable + ".Status='Approved' AND " + IncedentTable + ".ClassId=" + ClassId + " AND " + IncedentTable + ".BatchId=" + BatchId + " AND Month(" + IncedentTable + ".IncedentDate)=" + Month;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out Totalpoints);
            }
            return Totalpoints;
        }

        public int GetMonthlyStaffsPoints(int StaffId, int BatchId, int Month)
        {
            int Totalpoints = 0;
            string sql = "SELECT SUM(tblincedent.`Point`) FROM tblincedent WHERE tblincedent.UserType='Staff' AND tblincedent.Status='Approved' AND tblincedent.BatchId=" + BatchId + " AND tblincedent.AssoUser=" + StaffId + " AND Month(tblincedent.IncedentDate)=" + Month;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out Totalpoints);
            }
            return Totalpoints;
        }

        public int GetMonthlyStudent_Points(int StudentId, int BatchId, int Month, string IncedentTable)
        {
            int Totalpoints = 0;
            string sql = "SELECT SUM(" + IncedentTable + ".`Point`) FROM " + IncedentTable + " WHERE " + IncedentTable + ".UserType='student'  AND " + IncedentTable + ".Status='Approved' AND  " + IncedentTable + ".BatchId=" + BatchId + " AND " + IncedentTable + ".AssoUser=" + StudentId + " AND Month(" + IncedentTable + ".IncedentDate)=" + Month;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out Totalpoints);
            }
            return Totalpoints;
        }

        public bool PromotionOrHistory(int studid, int crnt_btch)
        {
            bool PromotionOrHistory = false;
            string sql = "select StudentId from tblstudentclassmap where StudentId=" + studid + " and BatchId = " + crnt_btch + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
            }

            else
            {
                m_MyReader = null;
                sql = "select tblstudent.Id from tblstudent where tblstudent.Id in (select tblpromotionlist.StudentId as Id  from  tblpromotionlist where tblpromotionlist.StudentId = " + studid + " union select tblstudent_history.Id as Id from tblstudent_history where tblstudent_history.Id = " + studid + " and tblstudent_history.Status = 1 ) and tblstudent.Status = 1";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {

                    PromotionOrHistory = true;
                }
            }
            return PromotionOrHistory;
        }







        public void GetPointRating(int _StudentId,int _CurrentBatchId, out int _Point, out int _rating)
        {
          
            StudentManagerClass MyStudent = new StudentManagerClass(m_MysqlDb);
            int TotalPoints = 0;
            int TotalRating = 0;
            string sql = "SELECT tblincidentcalculation.PBR,tblincidentcalculation.PBP FROM tblincidentcalculation WHERE tblincidentcalculation.BatchId<" + _CurrentBatchId + " AND tblincidentcalculation.StudentId=" + _StudentId;
            OdbcDataReader MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    TotalRating = TotalRating + int.Parse(MyReader.GetValue(0).ToString());
                    TotalPoints = TotalPoints + int.Parse(MyReader.GetValue(1).ToString());
                }
            }

            int Current_Batchpoints = 0;
            int ClassId = MyStudent.GetClassId(_StudentId,_CurrentBatchId);

            string IncedentTable = "tblincedent";
            //if (Session["StudType"].ToString() == "2")
            //{
            //    ClassId = MyStudent.GetClassIdHistory(StudentId);
            //    IncedentTable = "tblincedent_history";
            //}
            int Current_BatchRating = GetCurrentBatch_IncidenceRating(_StudentId, ClassId,_CurrentBatchId, IncedentTable, out Current_Batchpoints);
            TotalRating = TotalRating + Current_BatchRating;
            TotalPoints = TotalPoints + Current_Batchpoints;

            _Point=TotalPoints;
            _rating = TotalRating;
           
            MyStudent = null;
        }

        public FeedbackClass[] GetFeedBackDetailsAssociatedClass(int classId, out int incidentMasterId, out int needApproval)
        {

            FeedbackClass[] feedback = null;
            int FeedbackMasterId = getFeedbackMasterId(classId, out needApproval);
            int incidentcount = 0;

            incidentMasterId = FeedbackMasterId;

            if (FeedbackMasterId != 0)
            {

                DataSet IncidetList = GetIncidentListfromFeedbackMasterId(FeedbackMasterId, out incidentcount);

                if (incidentcount > 0 && IncidetList != null && IncidetList.Tables != null && IncidetList.Tables[0].Rows.Count > 0)
                {
                    int i = -1, temp = 0, j = 0;


                    feedback = new FeedbackClass[incidentcount];
                    IncidentList incident = new IncidentList();

                    foreach (DataRow dr in IncidetList.Tables[0].Rows)
                    {
                        if (temp == int.Parse(dr["TypeId"].ToString()))
                        {
                            incident.id = int.Parse(dr["IncidentId"].ToString());
                            incident.Values = dr["Title"].ToString();
                            feedback[i].Incidentheadings.Add(incident);

                        }
                        else
                        {
                            i = i + 1;
                            temp = int.Parse(dr["TypeId"].ToString());

                            feedback[i].IncidentType.id = int.Parse(dr["TypeId"].ToString());
                            feedback[i].IncidentType.Values = dr["IncidentType"].ToString();

                            incident.id = int.Parse(dr["IncidentId"].ToString());
                            incident.Values = dr["Title"].ToString();

                            feedback[i].Incidentheadings = new List<IncidentList>();
                            feedback[i].Incidentheadings.Add(incident);
                        }

                    }

                }
            }

            return feedback;
        }

        private DataSet GetIncidentListfromFeedbackMasterId(int FeedbackMasterId, out int incidentcount)
        {
            incidentcount = 0;

            string sql = "select count(tblincident_feedbackmap.IncidentTypeId) as count from tblincident_feedbackmap where tblincident_feedbackmap.MasterId=" + FeedbackMasterId;
            DataSet dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
            {
                int.TryParse(dt.Tables[0].Rows[0]["count"].ToString(), out incidentcount);
            }

            sql = "Select tblincedenttype.Id as TypeId, tblincedenttype.Type as IncidentType, tblincedenthead.id as IncidentId, tblincedenthead.Title from tblincident_feedbackmap  inner join tblincedenttype on tblincedenttype.Id= tblincident_feedbackmap.IncidentTypeId inner join tblincedenthead  on tblincedenthead.TypeId= tblincedenttype.Id where tblincident_feedbackmap.MasterId=" + FeedbackMasterId + "  and  tblincedenttype.IncidentType='FEEDBACK'  order by tblincedenttype.Id ";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        }

        private int getFeedbackMasterId(int classId, out int needApproval)
        {
            int id = 0;
            needApproval = 0;
            string sql = "select tblincident_feedbackmaster.Id as Id  , tblincident_feedbackmaster.MasterName ,Class, tblincident_feedbackmaster.NeedApproval from tblincident_feedbackmaster";
            DataSet dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
            {
                if (dt.Tables[0].Rows.Count == 1)
                {
                    int.TryParse(dt.Tables[0].Rows[0]["Id"].ToString(), out id);

                    int.TryParse(dt.Tables[0].Rows[0]["NeedApproval"].ToString(), out needApproval);
                }
                else
                {
                    string[] ClassId;

                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {

                        if (dr["Class"].ToString() == "All")
                        {
                            int.TryParse(dr["Id"].ToString(), out id);
                            int.TryParse(dt.Tables[0].Rows[0]["NeedApproval"].ToString(), out needApproval);
                        }
                        else
                        {
                            ClassId = dr["Class"].ToString().Split(',');

                            if (ClassId.Contains(classId.ToString()))
                            {
                                int.TryParse(dr["Id"].ToString(), out id);
                                int.TryParse(dt.Tables[0].Rows[0]["NeedApproval"].ToString(), out needApproval);
                            }
                        }

                    }

                }
            }
            return id;
        }

        public string getFeedbackNamefromId(int feedbackMasterId)
        {


            string sql = "select tblincident_feedbackmaster.MasterName from tblincident_feedbackmaster where tblincident_feedbackmaster.Id=" + feedbackMasterId;
            DataSet dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
            {
                return dt.Tables[0].Rows[0]["MasterName"].ToString();

            }
            return null;
        }

        public int insertTeachersFeedBackTable(int feedbackMasterId, string feedbackmasterName, int staffId, string staffName, int classId, int studentId,int periodId, int batchId)
        {
            string sql = " insert into tblincident_teachersfeedback(StudentId,StaffId,StaffName,ClassID,FeedbackMasterId,FeedbackMasterName,PeriodId,BatchId) values(" + studentId + "," + staffId + ",'" + staffName + "'," + classId + "," + feedbackMasterId + ",'" + feedbackmasterName + "'," + periodId + "," + batchId + ") ";
            m_MysqlDb.ExecuteQuery(sql);

            sql = " select Id from tblincident_teachersfeedback where StudentId=" + studentId + " and StaffId=" + staffId + " and StaffName='" + staffName + "' and ClassID=" + classId + " and  FeedbackMasterId=" + feedbackMasterId + " and FeedbackMasterName='" + feedbackmasterName + "' and PeriodId="+periodId+" and BatchId="+batchId;
            DataSet dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dt.Tables[0].Rows[0]["Id"].ToString());

            }
            return 0;
        }

        public bool SubmitIncident(DataSet incidentDetails)
        {

            int count = 0;
            StringBuilder incidentString = new StringBuilder();
            //dominic 
            DateTime incdt = new DateTime();
            DateTime CRdt = new DateTime();
            incidentString.Append("insert into tblincedent (Title,Description,IncedentDate,CreatedDate,TypeId,ApproverId,CreatedUserId,UserType,Status,AssoUser,HeadId,Point,BatchId,ClassId,Reference,IncidentType,IncidentTypeId) values");

            foreach (DataRow dr in incidentDetails.Tables[0].Rows)
            {
                if (count != 0)
                {
                    incidentString.Append(",");
                }

                DateTime.TryParse(dr["IncedentDate"].ToString(), out incdt);
                DateTime.TryParse(dr["CreatedDate"].ToString(), out CRdt);

                incidentString.Append(" ('" + dr["Title"] + "','" + dr["Description"] + "','" + incdt.ToString("s") + "','" + CRdt.ToString("s") + "'," + dr["TypeId"] + "," + dr["ApproverId"] + "," + dr["CreatedUserId"] + ",'" + dr["UserType"] + "','" + dr["Status"] + "'," + dr["AssoUser"] + "," + dr["HeadId"] + "," + dr["Point"] + "," + dr["BatchId"] + "," + dr["ClassId"] + ",'" + dr["Reference"] + "'," + dr["IncidentType"] + "," + dr["IncidentTypeId"] + ")");
                count = 1;
            }
            m_MysqlDb.ExecuteQuery(incidentString.ToString());


            return true;
        }

        public DataSet getIncidentTypefromMasterId(int feedbackMasterId)
        {
            string sql = " Select tblincedenttype.Id as TypeId, tblincedenttype.Type as IncidentType from tblincident_feedbackmap  inner join tblincedenttype on tblincedenttype.Id= tblincident_feedbackmap.IncidentTypeId where tblincident_feedbackmap.MasterId=" + feedbackMasterId + "  and  tblincedenttype.IncidentType='FEEDBACK'  order by tblincedenttype.Id ";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }

        public bool IsUpdatedFeedback(int studentId, int classId,int periodId ,int batchId,  out int IncidentId)
        {
            IncidentId = 0;
            string sql = "select tblincident_teachersfeedback.id from tblincident_teachersfeedback where tblincident_teachersfeedback.StudentId=" + studentId + " and tblincident_teachersfeedback.ClassID=" + classId + " and PeriodId =" + periodId + " and BatchId=" + batchId;
            DataSet dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
            {
                int.TryParse(dt.Tables[0].Rows[0]["id"].ToString(), out IncidentId);

                return true;


            }
            else
                return false;
        }

        public bool IsUpdatedFeedback(int studentId, int classId,int periodid, int batch, out int IncidentId, out int StaffId, out string StaffName, out int FeedbackMasterId)
        {
            IncidentId = FeedbackMasterId = 0;
            StaffName = "";
            StaffId = 0;
            string sql = "select Id,StaffId,StaffName,ClassID,FeedbackMasterId,FeedbackMasterName from tblincident_teachersfeedback where tblincident_teachersfeedback.StudentId=" + studentId + " and tblincident_teachersfeedback.ClassID=" + classId+" and PeriodId="+periodid+"  and BatchId="+batch;
            DataSet dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
            {
                int.TryParse(dt.Tables[0].Rows[0]["Id"].ToString(), out IncidentId);

                int.TryParse(dt.Tables[0].Rows[0]["StaffId"].ToString(), out StaffId);
                StaffName = dt.Tables[0].Rows[0]["StaffName"].ToString();

                int.TryParse(dt.Tables[0].Rows[0]["FeedbackMasterId"].ToString(), out FeedbackMasterId);


                return true;


            }
            else
                return false;
        }

    

        public DataSet getStudentIncident(int studentId, int classId, int teachersfeedbackid)
        {
            string sql = "select tblincedent.Title, tblincedent.Description from tblincedent where tblincedent.IncidentTypeId=" + teachersfeedbackid + " and tblincedent.UserType='Student' and tblincedent.AssoUser=" + studentId + "";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }

        public int GetPreriodFrequencyType()
        {
            string sql = "select  Value from  tblconfiguration where Module='PSLogin' and Name='FrequencyId'";

            DataSet dt= m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            
            if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dt.Tables[0].Rows[0]["Value"].ToString());

            }
            return 1;
        }

        public DataSet getPeriods(int FrequencyType)
        {
            string sql = "select tblperiod.id, tblperiod.Period from tblperiod where tblperiod.FrequencyId="+FrequencyType+" order by tblperiod.Order";
           
            DataSet dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
            {
                return dt;

            }
            else return null;
        }

        public void DeleteCurrentValues(int incidentId)
        {
            string sql = "delete  from tblincident_teachersfeedback where id=" + incidentId;

            m_MysqlDb.ExecuteQuery(sql);
            sql = "delete from tblincedent where tblincedent.IncidentType=1 and tblincedent.IncidentTypeId=" + incidentId;
            m_MysqlDb.ExecuteQuery(sql);
        }
    }
}
