using System;
using System.Web;
//using System.Web.Services;
//using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Data.Odbc;
using System.Data;
using System.Text;
using WinBase;
public class StaffManager:KnowinGen
{
 
    public MysqlClass m_MysqlDb;
    private OdbcDataReader m_MyReader = null;
    private KnowinEncryption m_MyEncrypt = null;
    public MysqlClass m_TransationDb = null;
    private string m_StaffMenuStr;
    private string m_SubStaffMenuStr;
    
    public StaffManager(KnowinGen _Prntobj)
    {
        m_Parent = _Prntobj;
        m_MyODBCConn = m_Parent.ODBCconnection;
        m_UserName = m_Parent.LoginUserName;
        m_MysqlDb = new MysqlClass(this);
        m_StaffMenuStr = "";
        m_SubStaffMenuStr = "";
    }

    ~StaffManager()
    {
        if (m_MysqlDb != null)
        {
            m_MysqlDb = null;

        } if (m_MyReader != null)
        {
            m_MyReader = null;

        }
        if (m_MyEncrypt != null)
        {
            m_MyEncrypt = null;
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
    public string GetStaffMangMenuString(int _roleid)
    {
        string _MenuStr;
        if (m_StaffMenuStr == "")
        {


            _MenuStr = "<ul><li><a href=\"ViewStaffs.aspx\">Search Staff</a></li>";
            string sql = "SELECT DISTINCT tblaction.MenuName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND (( tblroleactionmap.ModuleId=11 AND tblaction.ActionType='Link') Or tblaction.ActionType='StaffLink')";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {


                    _MenuStr = _MenuStr + "<li><a href=\"" + m_MyReader.GetValue(1).ToString() + "\">" + m_MyReader.GetValue(0).ToString() + "</a></li>";
                }

            }
            _MenuStr = _MenuStr + "</ul>";
            m_MyReader.Close();
            m_StaffMenuStr = _MenuStr;

        }
        else
        {
            _MenuStr = m_StaffMenuStr;
        }
        return _MenuStr;

    }
    public string GetSubStaffMangMenuString(int _roleid)
    {
        string _MenuStr;
        if (m_SubStaffMenuStr == "")
        {

            _MenuStr = "<ul class=\"block\"><li><a href=\"StaffDetails.aspx\">Staff Details</a></li>";
            string sql = "SELECT DISTINCT tblaction.MenuName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND ((tblroleactionmap.ModuleId=11 AND tblaction.ActionType='SubLink') or tblaction.ActionType='StaffSubLink') Order by tblaction.Order";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {


                    _MenuStr = _MenuStr + "<li><a href=\"" + m_MyReader.GetValue(1).ToString() + "\">" + m_MyReader.GetValue(0).ToString() + "</a></li>";
                }

            }
            _MenuStr = _MenuStr + "</ul>";
            m_MyReader.Close();
            m_SubStaffMenuStr = _MenuStr;

        }
        else
        {
            _MenuStr = m_SubStaffMenuStr;
        }
        return _MenuStr;

    }

    public bool IsValidStaffId(string _staffid)
    {
        bool _valide;
        string sql = "SELECT UserName FROM tbluser where UserName='" + _staffid + "'";
       
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

            _valide = false;
        }
        else
        {
            _valide = true;
        }
        m_MyReader.Close();
        return _valide;
    }

    public int CreateStaff(string _StaffName, string _password, string _JoiningDate, string _Address, string _Sex, float _Experience, string _Desig, string _PhNo, string _Email, string _StaffId, string _EduQuli, int _Login, int _Role, String _Dob, string _ParentGrp,string _Aadhar,string _Pan, out string _Message)
    {
        CLogging logger = CLogging.GetLogObject();
        
        _Message = "";
        int _UserId=-1;
        string _cipherText="";
        if (_password != "")
        {
            if (m_MyEncrypt == null)
            {
                m_MyEncrypt = new KnowinEncryption();
            }
            _cipherText = m_MyEncrypt.Encrypt(_password);
        }
        
        string _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        DateTime _joingDate = General.GetDateTimeFromText(_JoiningDate);
        DateTime _DateOfBirth = General.GetDateTimeFromText(_Dob);
        try
        {
            General _GenObj = new General(m_TransationDb);
            _UserId = _GenObj.GetTableMaxId("tblview_user", "Id"); // Arun modified 21-11-11

            string sql = "INSERT INTO tbluser(Id,UserName,Password,EmailId,SurName,CreationTime,RoleId,CanLogin) VALUES (" + _UserId + ",'" + _StaffId + "','" + _cipherText + "','" + _Email + "','" + _StaffName + "','" + _strdtNow + "'," + _Role + "," + _Login + ")";
           // m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_TransationDb != null)
            {
                m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MysqlDb.ExecuteQuery(sql);
            }
            //sql = "SELECT Id FROM tbluser WHERE CreationTime='" + _strdtNow + "' AND SurName='" + _StaffName + "'";  // Arun modified 21-11-11
            //if (m_TransationDb != null)
            //{
            //    m_MyReader = m_TransationDb.ExecuteQuery(sql);
            //}
            //else
            //{
            //    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            //}
           
            //if (m_MyReader.HasRows)
            //{
            //    m_MyReader.Read();

            //    _UserId = int.Parse(m_MyReader.GetValue(0).ToString());

            //}
            //else
            //{
            //    _UserId = -1;
            //}

            if (_UserId != -1)
            {
                sql = "INSERT INTO tblstaffdetails(UserId,JoiningDate,Address,Sex,Experience,Designation,PhoneNumber,EduQualifications,Dob,AadharNo,PanNo) VALUES ( " + _UserId + ", '" + _joingDate.Date.ToString("s") + "', '" + _Address + "', '" + _Sex + "', " + _Experience + ", '" + _Desig + "', '" + _PhNo + "', '" + _EduQuli + "','" + _DateOfBirth.Date.ToString("s") + "','" + _Aadhar + "','" + _Pan + "')";
                if (m_TransationDb != null)
                {
                    m_TransationDb.ExecuteQuery(sql);
                }
                else
                {
                    m_MysqlDb.ExecuteQuery(sql);
                }
                sql = "INSERT INTO tblgroupusermap(GroupId,UserId) VALUES(" + _ParentGrp + "," + _UserId + ")";

                if (m_TransationDb != null)
                {
                    m_TransationDb.ExecuteQuery(sql);
                }
                else
                {
                    m_MysqlDb.ExecuteQuery(sql);
                }

            }
            else
            {

            }


           // m_MyReader.Close();
        }
        catch(Exception exc)
        {
           
            _UserId = -1;
            logger.LogToFile("CreateStaff", "throws Error " + exc.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);       
        }
        return  _UserId;
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



    public bool IsStaffSubject(int _StaffId, int _subId)
    {
        bool _valide;
        string sql = "SELECT * FROM tblstaffsubjectmap where StaffId=" + _StaffId + " And SubjectId=" + _subId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            _valide = true;
        }
        else
        {
            _valide = false;
        }
        m_MyReader.Close();
        return _valide;
    }

    public int UpdateStaff(string StaffName, string Password, string JoiningDate, string Address, string Sex, float Experience, string Desig, string PhNo, string Email, string StaffId, string EduQuli, int Login, int Role, string Dob, int StfId, int _SelectedGroupId,string Aadhar,string PAN)
    {
        CLogging logger = CLogging.GetLogObject();
        MysqlClass MysqlTranDb;
        MysqlTranDb = new MysqlClass(this);
       
        string _cipherText = "",sql;
        if (Password != "")
        {
            if (m_MyEncrypt == null)
            {
                m_MyEncrypt = new KnowinEncryption();
            }
            _cipherText = m_MyEncrypt.Encrypt(Password);
        }
        
        string _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        DateTime _joingDate = General.GetDateTimeFromText(JoiningDate);
        DateTime _DateOfBirth = General.GetDateTimeFromText(Dob);
        try
        {
            MysqlTranDb.MyBeginTransaction();
            if (Password != "")
            {
                sql = "UPDATE tbluser SET UserName='" + StaffId + "',Password='" + _cipherText + "',EmailId='" + Email + "',SurName= '" + StaffName + "',CreationTime= '" + _strdtNow + "',RoleId='" + Role + "',CanLogin='" + Login + "' WHERE Id=" + StfId;
               // m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                MysqlTranDb.TransExecuteQuery(sql);
            }
            else
            {
                sql = "UPDATE tbluser SET UserName='" + StaffId + "',EmailId='" + Email + "',SurName= '" + StaffName + "',CreationTime= '" + _strdtNow + "',RoleId='" + Role + "',CanLogin='" + Login + "' WHERE Id=" + StfId;
                //m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                MysqlTranDb.TransExecuteQuery(sql);
            }
            sql = "SELECT Id FROM tbluser WHERE CreationTime='" + _strdtNow + "' AND SurName= '" + StaffName + "'";
           // m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            m_MyReader = MysqlTranDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                m_MyReader.Read();

                StfId = int.Parse(m_MyReader.GetValue(0).ToString());

            }
            else
            {
                StfId = -1;
            }
            if (StfId != -1)
            {
                sql = "UPDATE tblstaffdetails SET UserId= '" + StfId + "',JoiningDate= '" + _joingDate.Date.ToString("s") + "',Address= '" + Address + "',Sex= '" + Sex + "',Experience= '" + Experience + "',Designation= '" + Desig + "',PhoneNumber= '" + PhNo + "',EduQualifications= '" + EduQuli + "',Dob= '" + _DateOfBirth.Date.ToString("s") + "',AadharNo= '" + Aadhar + "',PanNo= '" + PAN + "' WHERE UserId=" + StfId;
                MysqlTranDb.TransExecuteQuery(sql);
                sql = "UPDATE tblgroupusermap SET GroupId=" + _SelectedGroupId + " where UserId=" + StfId;
                MysqlTranDb.TransExecuteQuery(sql);
                sql = "UPDATE tblsmsstafflist SET PhoneNo=" + PhNo + ",Enabled=1 where Id=" + StfId;
                MysqlTranDb.TransExecuteQuery(sql);
                MysqlTranDb.TransactionCommit();
            }
            else
            {
                MysqlTranDb.TransactionRollback();
            }
           
        }
        catch(Exception exc)
        {
            MysqlTranDb.TransactionRollback();
            StfId = -1;
            logger.LogToFile("UpdateStaff", "throws Error " + exc.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);       
        }
        m_MyReader.Close();
        return StfId;
    }

    public bool HasImage(int _UsrId)
    {
      
        bool ImageFlag = false;
        String Sql = "SELECT FilePath FROM tblfileurl WHERE UserId=" + _UsrId + " AND Type='StaffImage'";
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
        if (m_MyReader.HasRows)
        {
            ImageFlag = true;
        }
          
        return ImageFlag;
    }

    public void DeleteCurrentSubjects(int _StaffId)
    {
        CLogging logger = CLogging.GetLogObject();
        String Sql = "DELETE FROM tblstaffsubjectmap WHERE StaffId=" + _StaffId;
        logger.LogToFile("DeleteCurrentSubjects", " Executing Query " + Sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
    }

    public bool resignstaff(string _name, int _id, int _reasonid, string _dis,DateTime _resigndate, out string ErrorMessage)
    {
        CLogging logger = CLogging.GetLogObject();
        MysqlClass MysqlTranDb;
        MysqlTranDb = new MysqlClass(this);
        int Login;
        ErrorMessage="";
        bool _validate=false;
        try
        {
            MysqlTranDb.MyBeginTransaction();
            string Sql = "";
            //string Sql = "insert into tblresignuser(UserId,ReasionId,Discription,ResignDate) Values(" + _id + "," + _reasonid + ",'" + _dis + "','" + System.DateTime.Now.Date.ToString("s") + "')";
            //MysqlTranDb.TransExecuteQuery(Sql);
            Sql = "INSERT INTO tbluser_history SELECT * FROM tbluser WHERE tbluser.Id=" + _id;
            MysqlTranDb.TransExecuteQuery(Sql);
            //Sql = "INSERT INTO tbluserdetails_history SELECT * FROM tblstaffdetails WHERE tblstaffdetails.UserId=" + _id;
            //Sql="INSERT INTO tbluserdetails_history SELECT * FROM tblview_staffdetails WHERE tblview_staffdetails.UserId = "+ _id;
            Sql = "INSERT INTO tbluserdetails_history(UserId,JoiningDate,Address,Sex,Experience,ExpDescription,Designation,PhoneNumber,EduQualifications,Dob,ReasionId,Discription,ResignDate)  SELECT UserId,JoiningDate,Address,Sex,Experience,ExpDescription,Designation,PhoneNumber,EduQualifications,Dob,ReasionId,Discription,ResignDate FROM tblview_staffdetails WHERE tblview_staffdetails.UserId = " + _id;
            MysqlTranDb.TransExecuteQuery(Sql);
            Sql = "update tbluserdetails_history set ReasionId = " + _reasonid + ", Discription = '" + _dis + "', ResignDate = '" + _resigndate.Date.ToString("s") + "' where UserId = " + _id + "";
            //System.DateTime.Now.Date.ToString("s")
            MysqlTranDb.TransExecuteQuery(Sql);

            string sql = "Insert into tblincedent_history select * from tblincedent where tblincedent.AssoUser=" + _id + " and tblincedent.UserType='staff' and tblincedent.Status='Approved'";
            MysqlTranDb.TransExecuteQuery(sql);
            sql = " Delete   from tblincedent where tblincedent.AssoUser=" + _id + " and tblincedent.UserType='staff' and tblincedent.Status='Created'";
            MysqlTranDb.TransExecuteQuery(sql);
            sql = "Delete from tblincidentqueue where tblincidentqueue.IncidentId in(Select tblincedent.Id from tblincedent  where tblincedent.AssoUser=" + _id + " and tblincedent.`Status`='Created' and tblincedent.UserType='staff') and tblincidentqueue.UserType='staff'";
            MysqlTranDb.TransExecuteQuery(sql);
            sql = "Delete from tblincedent where tblincedent.AssoUser=" + _id + " and tblincedent.UserType='staff' ";
            MysqlTranDb.TransExecuteQuery(sql);

            sql = "Insert into tblfileurl_history  (id,FilePath,Type,UserId,FileBytes)(select Id,FilePath,Type,UserId,FileBytes from tblfileurl where tblfileurl.UserId=" + _id + " and tblfileurl.`Type`='StaffImage')";
            MysqlTranDb.TransExecuteQuery(sql);
            sql = "Delete from tblfileurl where tblfileurl.UserId=" + _id + " and tblfileurl.`Type`='StaffImage'";
            MysqlTranDb.TransExecuteQuery(sql);

            Sql = "Delete from tbluser Where Id=" + _id;
            MysqlTranDb.TransExecuteQuery(Sql);
            Sql = "Delete from tblstaffdetails Where UserId=" + _id;
            MysqlTranDb.TransExecuteQuery(Sql);

            Sql = "Delete from tblstaffsubjectmap Where StaffId=" + _id;
            MysqlTranDb.TransExecuteQuery(Sql);          

            sql = "delete from tblincedent where assouser = "+_id+" and status = 'Created'";
            MysqlTranDb.TransExecuteQuery(Sql);

            Sql = "delete from tblbookissue where tblbookissue.UserId=" + _id + " and tblbookissue.UserType=2";
            MysqlTranDb.TransExecuteQuery(Sql);
            Sql = "delete from tblbookbooking where tblbookbooking.UserId=" + _id + " and tblbookbooking.UserType=2";
            MysqlTranDb.TransExecuteQuery(Sql);

            Sql = "Select ClassId from tblclassstaffmap where StaffId=" + _id;
            m_MyReader = MysqlTranDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                Sql = "Update tblclassstaffmap set StaffId=-1 where  StaffId=" + _id;
                MysqlTranDb.TransExecuteQuery(Sql);

                //tblclassstaffmap
            }



            _validate = true;

            MysqlTranDb.TransactionCommit();

        }
        catch(Exception exc)
        {
            ErrorMessage = "Error while submitting resign. Message : " + exc.Message;
            MysqlTranDb.TransactionRollback();
            logger.LogToFile("resignstaff", "throws Error " + exc.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, LoginUserName);
            _validate = false;
        }
        return _validate;
    }



    public bool roleExists(int _roleId)
    {
        bool isrole = false;
        string Sql = "select tblrole.RoleName from tblrole WHERE tblrole.id="  + _roleId +" and tblrole.Type='staff'";
        
         if (m_TransationDb != null)
         {
             m_MyReader = m_TransationDb.ExecuteQuery(Sql);
         }
         else
         {
             m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
         }
         if (m_MyReader.HasRows)
         {
             isrole = true;
         }
         return isrole;
    }

    public string FillStaffTopData(int _UserId, string Type)
    {
        DataSet _Mydata_PupilData = GetPupilData(_UserId, Type);
        StringBuilder _pupilTopData = new StringBuilder("");
        DateTime _DOB;
        int Year;
        int Today = DateTime.Now.Year;

        if (_Mydata_PupilData != null && _Mydata_PupilData.Tables != null && _Mydata_PupilData.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr_PupilData in _Mydata_PupilData.Tables[0].Rows)
            {

                _DOB = DateTime.Parse(dr_PupilData[2].ToString());

                Year = Today - _DOB.Year;
                _pupilTopData = new StringBuilder("<div class=\"container skin1 \" > <table cellpadding=\"0\" cellspacing=\"0\" class=\"containerTable\"> <tr class=\"top\"><td class=\"no\"> </td> <td class=\"n\"> Staff Details </td> <td class=\"ne\"> </td></tr><tr class=\"middle\"> <td class=\"o\"> </td> 	<td class=\"c\"><table width=\"100%\">");
                _pupilTopData.Append(" <tr> <td style=\"background-color: #C2D5FC\">  <b>Name</b> </td> <td style=\"background-color: #C2D5FC\"> " + dr_PupilData[0].ToString() + "  </td> </tr><tr><td> &nbsp;</td> <td> &nbsp;</td>  </tr><tr> <td style=\"background-color: #C2D5FC\">  <b>Sex</b> </td> <td style=\"background-color: #C2D5FC\"> " + dr_PupilData[1].ToString() + "  </td> </tr><tr><td> &nbsp;</td> <td> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>  </tr>");
  
                _pupilTopData.Append("<tr> <td style=\"background-color: #C2D5FC\">  <b>Experience</b> </td> <td style=\"background-color: #C2D5FC\"> " + dr_PupilData[3].ToString() + "  </td> </tr><tr><td> &nbsp;</td> <td> &nbsp;</td>  </tr><tr> <td style=\"background-color: #C2D5FC\">  <b>DOB</b> </td> <td style=\"background-color: #C2D5FC\"> " + _DOB.ToString("dd-MM-yyyy") + "  </td> </tr><tr><td> &nbsp;</td> <td> &nbsp;</td>  </tr>");
                _pupilTopData.Append("<tr> <td style=\"background-color: #C2D5FC\">  <b>Age</b> </td> <td style=\"background-color: #C2D5FC\"> " + Year + "  </td> </tr><tr><td> &nbsp;</td> <td> &nbsp;</td>  </tr><tr> <td style=\"background-color: #C2D5FC\">  <b>Designation</b> </td> <td style=\"background-color: #C2D5FC\"> " + dr_PupilData[4].ToString() + "  </td> </tr>");
                _pupilTopData.Append("</table></td> <td class=\"e\"> </td> </tr><tr class=\"bottom\"> <td class=\"so\"> </td> <td class=\"s\"></td> <td class=\"se\"> </td> </tr> 	</table> </div>");
            }
        }
        return _pupilTopData.ToString();
    }

    private DataSet GetPupilData(int _userId, string _Type)
    {
        string sql;
        DataSet _Mydata_PupilData = null;
        if (_Type == "Staff")
        {
            sql = "select tbluser.SurName , tblstaffdetails.Sex, tblstaffdetails.Dob , tblstaffdetails.Experience , tblstaffdetails.Designation , tblrole.RoleName  from tbluser inner join tblstaffdetails on tblstaffdetails.UserId = tbluser.Id inner join tblrole on tblrole.Id = tbluser.RoleId where tbluser.Id=" + _userId + " and tbluser.`Status`=1";
            _Mydata_PupilData = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }
        return _Mydata_PupilData;
    }


    public string ToStripString(int _StaffId, string _Url)
    {
        

        DateTime _DOB;
        string _staffname = "", _age = "", role = "";
        int Year;
        DateTime Today = DateTime.Now.Date;
        string Sql = "select tbluser.SurName, tblstaffdetails.Dob, tblrole.RoleName from tbluser inner join tblstaffdetails on tblstaffdetails.UserId =tbluser.Id inner join tblrole on tblrole.Id = tbluser.RoleId where tbluser.Id=" + _StaffId;
        // Up to Nationality Field in tblstudent
        m_MyReader = m_MysqlDb.ExecuteQuery(Sql);
        if (m_MyReader.HasRows)
        {
            m_MyReader.Read();
            _staffname = m_MyReader.GetValue(0).ToString();
            _DOB = DateTime.Parse(m_MyReader.GetValue(1).ToString());
            Year = GetAge(_DOB,Today);
            _age = Year.ToString();
            role = m_MyReader.GetValue(2).ToString();
            m_MyReader.Close();
        }



        StringBuilder _pupilTopData = new StringBuilder("<div id=\"winschoolStudentStrip\"><table class=\"NewStudentStrip\" width=\"100%\"><tr><td class=\"left1\"></td><td class=\"middle1\" ><table><tr><td><img alt=\"\" src=\"" + _Url + "\" width=\"75px\" height=\"80px\" /></td><td> </td><td><table width=\"500\"><tr><td class=\"attributeValue\">Name</td><td></td><td>:</td><td></td><td class=\"DBvalue\">" + _staffname + "</td></tr><tr><td colspan=\"11\"><hr /></td></tr><tr><td class=\"attributeValue\">Role</td><td></td><td>:</td><td></td><td class=\"DBvalue\">" + role + "</td><td class=\"attributeValue\">Age</td><td></td><td>:</td> <td></td> <td class=\"DBvalue\"> " + _age + "</td> <td></td> </tr></table></td>  </tr></table>  </td>  <td class=\"right1\"> </td> </tr></table></div>");
        //StringBuilder _pupilTopData = new StringBuilder("<div id=\"winschoolStudentStrip\"><table class=\"NewStudentStrip\" width=\"100%\"><tr><td class=\"left1\"></td><td class=\"middle1\" ><table><tr><td><img alt=\"\" src=\"E:\\DATA\\ThumbnailImages\\Staff6dfghjjk.jpg\" width=\"75px\" height=\"80px\" /></td><td> </td><td><table width=\"500\"><tr><td class=\"attributeValue\">Name</td><td></td><td>:</td><td></td><td class=\"DBvalue\">" + _staffname + "</td></tr><tr><td colspan=\"11\"><hr /></td></tr><tr><td class=\"attributeValue\">Role</td><td></td><td>:</td><td></td><td class=\"DBvalue\">" + role + "</td><td class=\"attributeValue\">Age</td><td></td><td>:</td> <td></td> <td class=\"DBvalue\"> " + _age + "</td> <td></td> </tr></table></td>  </tr></table>  </td>  <td class=\"right1\"> </td> </tr></table></div>");


        return _pupilTopData.ToString();
    }

    public int GetAge(DateTime dateOfBirth, DateTime dateAsAt)
    {
        return dateAsAt.Year - dateOfBirth.Year - (GetDayOfYear(dateOfBirth) <= dateAsAt.DayOfYear ? 0 : 1);

    }

    private int GetDayOfYear(DateTime dateOfBirth)
    {
        int _dayOfyear = dateOfBirth.DayOfYear;
        if (dateOfBirth.Month > 2)
        {
            if (IsLeapYear(dateOfBirth.Year))
            {
                _dayOfyear--;
            }
        }
        return _dayOfyear;
    }

    public static bool IsLeapYear(int year)
    {

        if (year % 4 != 0)
        {

            return false;

        }

        if (year % 100 == 0)
        {

            return (year % 400 == 0);

        }

        return true;
    }

    public string GetStaffName(int _StaffId)
    {
        string Name = "";
        string sql = "select tbluser.SurName from tbluser where tbluser.Id = "+_StaffId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            Name = m_MyReader.GetValue(0).ToString();
        }
        return Name;
    }

    public string GetStaffNameForSubject(int _ClassId, int _SubjectId)
    {
        string Name = "";
        int i = 0;
        string sql = "select tbluser.SurName from tbluser where tbluser.Id in(select distinct(tblclassstaffmap.StaffId) from tblclassstaffmap where tblclassstaffmap.ClassId="+_ClassId+" and tblclassstaffmap.SubjectId="+_SubjectId+")";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            while (m_MyReader.Read())
            {
                if (i > 0)
                {
                    Name = Name + " / ";
                }
                Name = m_MyReader.GetValue(0).ToString();
                i++;
            }            
        }
        return Name;
    }



    public void AddPayrollEmp(string _Name, string _address, string _EmpId, string _Pan, string _BankName, string _Acc, int _PayrollId, string _Designation, double _BasicPay, double _Gross, double _NetPay, int _StaffId)
    {
        string sql = " insert into tblpay_employee(Surname,EmpId,PresentAddress,Designation,BasicPay,Gross,NetAmt,PAN,BankName,AccNo,PayrollType,StaffId) values('" + _Name + "','" + _EmpId + "','" + _address + "','" + _Designation + "'," + _BasicPay + "," + _Gross + "," + _NetPay + ",'" + _Pan + "','" + _BankName + "'," + _Acc + ", " + _PayrollId + "," + _StaffId + ")";
        if (m_TransationDb != null)
        {
            m_TransationDb.ExecuteQuery(sql);
        }
        else
        {
            m_MysqlDb.ExecuteQuery(sql);
        }
    }

    public bool BankAccountNumValid(string bankaccnum)
    {
        bool valid = true;
        string sql = "";
        OdbcDataReader Panreader = null;
        sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='IsBankAccountNumMandatory'";
        Panreader = m_TransationDb.ExecuteQuery(sql);
        if (Panreader.HasRows)
        {
            if (Panreader.GetValue(0).ToString() == "1")
            {
                if (bankaccnum == "")
                {
                    valid = false;
                }
            }
        }

        return valid;
    }

    public bool BankNameValid(string bankname)
    {
        bool valid = true;
        string sql = "";
        OdbcDataReader Panreader = null;
        sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='IsBankNameMandatory'";
        Panreader = m_TransationDb.ExecuteQuery(sql);
        if (Panreader.HasRows)
        {
            if (Panreader.GetValue(0).ToString() == "1")
            {
                if (bankname == "")
                {
                    valid = false;
                }
            }
        }

        return valid;
    }

    public bool NeedPanNumberMantatory()
    {
        bool valid = false;
        string sql = "";
        OdbcDataReader Panreader = null;
        sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='IsPanNumberMandatory'";
        if (m_TransationDb != null)
            Panreader = m_TransationDb.ExecuteQuery(sql);
        else
            Panreader = m_MysqlDb.ExecuteQuery(sql);
        if (Panreader.HasRows)
        {
            if (Panreader.GetValue(0).ToString() == "1")
            {
                    valid = true;
            }
        }

        return valid;
    }


    public bool PanNumberValid(string pannum)
    {
        bool valid = true;
        string sql = "";
        OdbcDataReader Panreader = null;
        sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='IsPanNumberMandatory'";
        if (m_TransationDb != null)
            Panreader = m_TransationDb.ExecuteQuery(sql);
        else
            Panreader = m_MysqlDb.ExecuteQuery(sql);
        if (Panreader.HasRows)
        {
            if (Panreader.GetValue(0).ToString() == "1")
            {
                if (pannum == "")
                {
                    valid = false;
                }
            }
        }

        return valid;
    }


    public OdbcDataReader GetBasicPay(int _PayrollId)
    {
        string sql = " select tblpay_category.BasicPay from  tblpay_category where tblpay_category.Id = " + _PayrollId + "";
        if (m_TransationDb != null)
        {
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
        }
        else
        {
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }
        return m_MyReader;
    }
    public DataSet FillDrp()
    {
        DataSet _DrpCat = new DataSet();
        string sql = "select tblpay_category.Id,tblpay_category.CategoryName from tblpay_category";
        _DrpCat = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return _DrpCat;
    }

    public OdbcDataReader GetHeadId(int _PayrollId)
    {
        string sql = " select DISTINCT tblpay_headcategorymap.HeadId from tblpay_headcategorymap where tblpay_headcategorymap.CategoryId =" + _PayrollId + "";
        if (m_TransationDb != null)
        {
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
        }
        else
        {
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }
        return m_MyReader;
    }

    public void AddEmpHeadMap(int _HeadId, string _EmpId, int _PayrollId)
    {
        string sql = " insert into tblpay_employeeheadmap(EmployeeId,CategoryId,HeadId) values('" + _EmpId + "'," + _PayrollId + "," + _HeadId + ")";
        if (m_TransationDb != null)
        {
            m_TransationDb.ExecuteQuery(sql);
        }
        else
        {
            m_MysqlDb.ExecuteQuery(sql);
        }
    }

    public void UpdatePayrollEmp(string _Name, string _address, string _EmpId, string _Pan, string _BankName, string _Acc, int _PayrollId, string _Designation, double _BasicPay, double _Gross, double _NetPay, int _StaffId)
    {
        string _sql = "";
        //Surname,EmpId,PresentAddress,Designation,BasicPay,Gross,NetAmt,PAN,BankName,AccNo,PayrollType,StaffId) values('" + _Name + "','" + _EmpId + "','" + _address + "','" + _Designation + "'," + _BasicPay + "," + _Gross + "," + _NetPay + "," + _Pan + ",'" + _BankName + "'," + _Acc + ", " + _PayrollId + "," + _StaffId + ")"
        _sql = "Update tblpay_employee set Surname='" + _Name + "', PresentAddress='" + _address + "',Designation='" + _Designation + "',BasicPay=" + _BasicPay + ",Gross=" + _Gross + ",NetAmt=" + _NetPay + ",PAN='" + _Pan + "',BankName='" + _BankName + "',AccNo=" + _Acc + ",PayrollType=" + _PayrollId + ",StaffId=" + _StaffId + ", status=1 where EmpId='" + _EmpId + "' and StaffId=" + _StaffId + "";
        if (m_TransationDb != null)
        {
            m_TransationDb.ExecuteQuery(_sql);
        }
        else
        {
            m_MysqlDb.ExecuteQuery(_sql);
        }
    }

    public int GetpayrollType(string _EmpId)
    {
        int payrolltype = 0;
      string sql="select PayrollType from tblpay_employee where tblpay_employee.EmpId = '" + _EmpId + "'";
      if (m_TransationDb != null)
      {
        m_MyReader=  m_TransationDb.ExecuteQuery(sql);
      }
      else
      {
          m_MyReader=m_MysqlDb.ExecuteQuery(sql);
      }
      if (m_MyReader.HasRows)
      {
          payrolltype = int.Parse(m_MyReader.GetValue(0).ToString());
      }
      return payrolltype;

    }

    public void DeleteEmpHeadMap(string _EmpId,int headId)
    {
        string sql = "";
        sql = "delete from tblpay_employeeheadmap where HeadId=" + headId + " and EmployeeId='" + _EmpId + "'";
        if (m_TransationDb != null)
        {
            m_TransationDb.ExecuteQuery(sql);
        }
        else
        {
            m_MysqlDb.ExecuteQuery(sql);
        }
    }

    public OdbcDataReader GetHeadIdOfEmp(string _EmpId)
    {
        string sql = "select HeadId from tblpay_employeeheadmap where EmployeeId='" + _EmpId + "'";
        if (m_TransationDb != null)
        {
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
        }
        else
        {
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }
        return m_MyReader;
    }

    public string GetHandledSubject(string StaffId)
    {
        string _Subjects ="",Seperator="";
        string sql = "SELECT tblsubjects.subject_name FROM tblstaffsubjectmap INNER JOIN tblsubjects ON tblsubjects.Id=tblstaffsubjectmap.SubjectId WHERE tblstaffsubjectmap.StaffId=" + StaffId;
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
            while (m_MyReader.Read())
            {
                _Subjects = _Subjects + Seperator + m_MyReader.GetValue(0).ToString();
                Seperator = ", ";
            }
        }
        return _Subjects;
    }

    public void UpdateInactivePayrollEmp(string _EmpId, int _StaffId,string _Name,string _address)
    {
        string _sql = "";
        _sql = "Update tblpay_employee set Surname='" + _Name + "', PresentAddress='" + _address + "', status=0 where EmpId='" + _EmpId + "' and StaffId=" + _StaffId + "";
        if (m_TransationDb != null)
        {
            m_TransationDb.ExecuteQuery(_sql);
        }
        else
        {
            m_MysqlDb.ExecuteQuery(_sql);
        }
    }

    public bool SalaryPayed(string EmpId, int MonthId, int Year)
    {
           bool payed = false;
            string sql = "";
            sql = "select tblpay_empmonthlysalconfig.Payed from tblpay_empmonthlysalconfig where EmpId='"+EmpId+"' and Year="+Year+" and MonthId="+MonthId+"";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int payedvalue = int.Parse(m_MyReader.GetValue(0).ToString());
                if (payedvalue == 1)
                {
                    payed = true;
                }

            }
            return payed;
    }

    public void InsertStaffEmailIdIntoEmailStaffList(int _StaffId, string EmailId)
    {
        string sql = "delete from tbl_emailstafflist where Id=" + _StaffId;
        m_TransationDb.ExecuteQuery(sql);

        sql = "insert into tbl_emailstafflist(Id,EmailId,Enabled) values (" + _StaffId + ",'" + EmailId + "'," + "1)";
        m_TransationDb.ExecuteQuery(sql);
    }
}
