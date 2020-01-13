using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Configuration;
using System.Data.Odbc;
using WinBase;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.IO;

namespace Scool360student
{
    /// <summary>
    /// Summary description for WinErWebService
    /// </summary>
     
    [System.Web.Script.Services.ScriptService]
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WinErWebService : System.Web.Services.WebService
    {
        //SchoolClass objSchool = null;
         KnowinUser MyUser=null;
        
        void InitilizeObjects()
        {
              //if (WinerUtlity.NeedCentrelDB())
              //  {
              //      if (Session[WinerConstants.SessionSchool] != null)
              //      {
              //          objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
              //      }

              //  }

            if (Session[WinerConstants.SessionUser] != null)
            {
                MyUser = (KnowinUser)Session[WinerConstants.SessionUser];
            }
            
        }
       
        [WebMethod(EnableSession=true)]

        public string[] GetStaffName(string prefixText, int count, string contextKey)
        {
            InitilizeObjects();
            string[] Datas = contextKey.Split('/');

            string sql = "select DISTINCT tbluser.SurName as Value from tbluser inner join tblrole on tblrole.Id = tbluser.RoleId inner join tblgroupusermap on tbluser.`Id`=tblgroupusermap.`UserId` where tbluser.Status=1 and tblrole.`Type`='Staff'  and tbluser.SurName Like '" + @prefixText + "%' and tblgroupusermap.`GroupId` IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + Datas[1] + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + Datas[1] + ")";
            if (Datas[0] == "0")
            {
                sql = "select DISTINCT tbluser.UserName as Value from tbluser inner join tblrole on tblrole.Id = tbluser.RoleId inner join tblgroupusermap on tbluser.`Id`=tblgroupusermap.`UserId` where tbluser.Status=1 and tblrole.`Type`='Staff' and tbluser.UserName Like '" + @prefixText + "%' and tblgroupusermap.`GroupId` IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + Datas[1] + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + Datas[1] + ")";
            }
            else if (Datas[0] == "1")
            {
                sql = "select DISTINCT tbluser.SurName as Value from tbluser inner join tblrole on tblrole.Id = tbluser.RoleId inner join tblgroupusermap on tbluser.`Id`=tblgroupusermap.`UserId` where tbluser.Status=1 and tblrole.`Type`='Staff'  and tbluser.SurName Like '" + @prefixText + "%' and tblgroupusermap.`GroupId` IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + Datas[1] + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + Datas[1] + ")";
            }
            else if (Datas[0] == "2")
            {
                sql = "select DISTINCT tblsubjects.subject_name as Value from tbluser inner join tblrole on tblrole.Id = tbluser.RoleId inner join tblstaffsubjectmap on tbluser.Id=tblstaffsubjectmap.StaffId inner join tblsubjects on tblstaffsubjectmap.SubjectId=tblsubjects.Id  inner join tblgroupusermap on tbluser.`Id`=tblgroupusermap.`UserId` where tbluser.Status=1 and tblrole.`Type`='Staff' and tblsubjects.subject_name Like '" + @prefixText + "%' and tblgroupusermap.`GroupId` IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + Datas[1] + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + Datas[1] + ")";
            }
            else if (Datas[0] == "3")
            {
                sql = "select DISTINCT tblstaffdetails.Designation as Value from tbluser inner join tblrole on tblrole.Id = tbluser.RoleId inner join tblstaffdetails on tbluser.Id=tblstaffdetails.UserId inner join tblgroupusermap on tbluser.`Id`=tblgroupusermap.`UserId` where tbluser.Status=1 and tblrole.`Type`='Staff' and tblstaffdetails.Designation Like '" + @prefixText + "%' and tblgroupusermap.`GroupId` IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + Datas[1] + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + Datas[1] + ")";
            }
            else if (Datas[0] == "4")
            {
                double Experience=0;
                double.TryParse(prefixText,out Experience);
                sql = "select DISTINCT tblstaffdetails.Experience as Value from tbluser inner join tblrole on tblrole.Id = tbluser.RoleId inner join tblstaffdetails on tbluser.Id=tblstaffdetails.UserId inner join tblgroupusermap on tbluser.`Id`=tblgroupusermap.`UserId` where tbluser.Status=1 and tblrole.`Type`='Staff' and tblstaffdetails.Experience>=" + @prefixText + " and tblgroupusermap.`GroupId` IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + Datas[1] + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + Datas[1] + ")";
            }
            else if (Datas[0] == "5")
            {
                double Experience=0;
                double.TryParse(prefixText,out Experience);
                sql = "select DISTINCT tblstaffdetails.Experience as Value from tbluser inner join tblrole on tblrole.Id = tbluser.RoleId inner join tblstaffdetails on tbluser.Id=tblstaffdetails.UserId inner join tblgroupusermap on tbluser.`Id`=tblgroupusermap.`UserId` where tbluser.Status=1 and tblrole.`Type`='Staff' and tblstaffdetails.Experience<=" + @prefixText + " and tblgroupusermap.`GroupId` IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + Datas[1] + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + Datas[1] + ")";
            }

            
            OdbcParameter odbcParameter=new OdbcParameter(){ParameterName="@prefixText",OdbcType=OdbcType.VarChar,Size=50,Value=prefixText + "%"};
            List<OdbcParameter> lstParameter=new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);

            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr["Value"].ToString(), i);
                i++;
            }
            return items;

        }


        [WebMethod(EnableSession = true)]

        public string[] GetStaffHistoryName(string prefixText)
        {

            InitilizeObjects();
            string sql = "select tbluser_history.SurName from tbluser_history inner join tblrole on tblrole.Id = tbluser_history.RoleId where tbluser_history.Status=1 and tblrole.`Type`='Staff' and tbluser_history.SurName Like '" + @prefixText + "%'";
            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);

            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr["SurName"].ToString(), i);
                i++;
            }
            return items;

        }


        [WebMethod(EnableSession=true)]

        public string[] GetStudentName(string prefixText, int count, string contextKey)
        {

            // int count = 10;
            string[] _temp = contextKey.Split('\\');
            string _type = _temp[0];
            string _Userid = _temp[1];
            string Live = _temp[2];
            string History = _temp[3];
            string PromoList = _temp[4];
            string Approval = _temp[5];
            string joining = _temp[6];
            InitilizeObjects();
            string sql="";


         

            if (_type == "0")
            {
                if (Live != "0")
                    sql += " (SELECT tblstudent.AdmitionNo  FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId inner join tblbatch on tblbatch.Id= tblstudentclassmap.BatchId AND tblbatch.`Status`=1 where  tblstudent.`Status`=1 AND tblstudent.AdmitionNo Like '%" + @prefixText + "%' AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " )) union";
                if (History != "0")
                {
                    //Removed  "AND tblbatch.`Status`=1 " on 05-07-2018 by Georgy & Amal
                    sql += " (SELECT tblstudent_history.AdmitionNo  FROM tblstudent_history inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent_history.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId inner join tblbatch on tblbatch.Id= tblstudentclassmap_history.BatchId where  tblstudent_history.`Status`<>1 AND tblstudent_history.AdmitionNo Like '%" + @prefixText + "%' AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " )) union";
                    sql += " (SELECT tblstudent.AdmitionNo  FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId inner join tblbatch on tblbatch.Id= tblstudentclassmap_history.BatchId AND tblbatch.`Status`=1 where  tblstudent.`Status`<>1 AND tblstudent.AdmitionNo Like '%" + @prefixText + "%' AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " ) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap)) union";
                }
                if (PromoList != "0")
                    sql += " (SELECT tblstudent.AdmitionNo  FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId inner join tblbatch on tblbatch.Id= tblstudentclassmap_history.BatchId AND tblbatch.`Status`=1 where  tblstudent.`Status`=1 AND tblstudent.AdmitionNo Like '%" + @prefixText + "%' AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " ) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap)) union";
                if (Approval != "0")
                    sql += " (SELECT tblstudent.AdmitionNo  FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId inner join tblbatch on tblbatch.Id= tblstudentclassmap.BatchId AND tblbatch.`Status`=1 where  tblstudent.`Status`=1 AND tblstudent.AdmitionNo Like '%" + @prefixText + "%' AND tblstudent.Status=2 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " )) union";
                if (joining != "0")
                    sql += " (SELECT tbltempstdent.TempId  FROM tbltempstdent inner join tblclass on tblclass.Id=tbltempstdent.Class where tbltempstdent.TempId Like '%" + @prefixText + "%' AND tbltempstdent.TempId not in(select TempStudentId from tblstudent) AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " )) union";
                sql += " (SELECT tblstudent.AdmitionNo  FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId inner join tblbatch on tblbatch.Id= tblstudentclassmap.BatchId AND tblbatch.`Status`=1 where  tblstudent.`Status`=134 AND tblstudent.AdmitionNo Like '%" + @prefixText + "%' AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " )) ";
            }
            else if (_type == "1")
            {
                if (Live != "0")
                    sql += " (SELECT tblstudent.StudentName  FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId inner join tblbatch on tblbatch.Id= tblstudentclassmap.BatchId AND tblbatch.`Status`=1 where  tblstudent.`Status`=1 AND tblstudent.StudentName Like '%" + @prefixText + "%' AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " )) union";
                if (History != "0")
                {
                    sql += " (SELECT tblstudent_history.StudentName  FROM tblstudent_history inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent_history.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId inner join tblbatch on tblbatch.Id= tblstudentclassmap_history.BatchId where  tblstudent_history.`Status`<>1 AND tblstudent_history.StudentName Like '%" + @prefixText + "%' AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " )) union";
                    sql += " (SELECT tblstudent.StudentName  FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId inner join tblbatch on tblbatch.Id= tblstudentclassmap_history.BatchId AND tblbatch.`Status`<>1 where  tblstudent.`Status`<>1 AND tblstudent.StudentName Like '%" + @prefixText + "%' AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " ) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap)) union";
                }
                if (PromoList != "0")
                    sql += " (SELECT tblstudent.StudentName  FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId inner join tblbatch on tblbatch.Id= tblstudentclassmap_history.BatchId AND tblbatch.`Status`<>1 where  tblstudent.`Status`=1 AND tblstudent.StudentName Like '%" + @prefixText + "%' AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " ) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap)) union";
                if (Approval != "0")
                    sql += " (SELECT tblstudent.StudentName  FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId inner join tblbatch on tblbatch.Id= tblstudentclassmap.BatchId AND tblbatch.`Status`=1 where  tblstudent.`Status`=2 AND tblstudent.StudentName Like '%" + @prefixText + "%' AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " )) union";
                if (joining != "0")
                    sql += " (SELECT tbltempstdent.Name  FROM tbltempstdent inner join tblclass on tblclass.Id=tbltempstdent.Class where tbltempstdent.Name Like '" + @prefixText + "%' AND tbltempstdent.TempId not in(select TempStudentId from tblstudent) AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " )) union";
                sql += " (SELECT tblstudent.StudentName  FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId inner join tblbatch on tblbatch.Id= tblstudentclassmap.BatchId AND tblbatch.`Status`=1 where  tblstudent.`Status`=134 AND tblstudent.StudentName Like '%" + @prefixText + "%' AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " )) ";
            }
            else if (_type == "2")
            {
                sql = " SELECT tblclass.ClassName FROM tblclass where tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " ) AND  tblclass.ClassName LIKE '%" + @prefixText + "%' AND tblclass.`Status`=1";
            }
            else if (_type == "3")
            {
                sql = "SELECT tblbatch.BatchName FROM tblbatch WHERE tblbatch.Created=1 AND tblbatch.BatchName LIKE '%" + @prefixText + "%'";
            }
            else if (_type == "4")
            {
                if (Live != "0")
                    sql += " (SELECT tblstudent.StudentId  FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId inner join tblbatch on tblbatch.Id= tblstudentclassmap.BatchId AND tblbatch.`Status`=1 where  tblstudent.`Status`=1 AND tblstudent.StudentId Like '%" + @prefixText + "%' AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " )) union";
                if (History != "0")
                {
                    sql += " (SELECT tblstudent_history.StudentId  FROM tblstudent_history inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent_history.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId inner join tblbatch on tblbatch.Id= tblstudentclassmap_history.BatchId where  tblstudent_history.`Status`<>1 AND tblstudent_history.StudentId Like '%" + @prefixText + "%' AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " )) union";
                    sql += " (SELECT tblstudent.StudentId  FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId inner join tblbatch on tblbatch.Id= tblstudentclassmap_history.BatchId AND tblbatch.`Status`=1 where  tblstudent.`Status`<>1 AND tblstudent.StudentId Like '%" + @prefixText + "%' AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " ) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap)) union";
                }
                if (PromoList != "0")
                    sql += " (SELECT tblstudent.StudentId  FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId inner join tblbatch on tblbatch.Id= tblstudentclassmap_history.BatchId AND tblbatch.`Status`=1 where  tblstudent.`Status`=1 AND tblstudent.StudentId Like '%" + @prefixText + "%' AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " ) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap)) union";
                if (Approval != "0")
                    sql += " (SELECT tblstudent.StudentId  FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId inner join tblbatch on tblbatch.Id= tblstudentclassmap.BatchId AND tblbatch.`Status`=1 where  tblstudent.`Status`=1 AND tblstudent.StudentId Like '%" + @prefixText + "%' AND tblstudent.Status=2 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " )) union";
                if (joining != "0")
                    sql += " (SELECT tbltempstdent.TempId  FROM tbltempstdent inner join tblclass on tblclass.Id=tbltempstdent.Class where tbltempstdent.TempId Like '%" + @prefixText + "%' AND tbltempstdent.TempId not in(select TempStudentId from tblstudent) AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " )) union";
                sql += " (SELECT tblstudent.StudentId  FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId inner join tblbatch on tblbatch.Id= tblstudentclassmap.BatchId AND tblbatch.`Status`=1 where  tblstudent.`Status`=134 AND tblstudent.StudentId Like '%" + @prefixText + "%' AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _Userid + " )) ";
            }

            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
            }
            return items;
        }
        

        [WebMethod(EnableSession=true)]

        public string[] GetStudentAdvSearchValue(string prefixText, int count, string contextKey)
        {
            // int count = 10;
            string[] _temp = contextKey.Split('\\');
            string _type = _temp[0];
            string _Userid = _temp[1];
            InitilizeObjects();
            string sql="";


            if (_type == "0")
            {
                sql = "SELECT DISTINCT(tblstudent.Sex) from tblstudent where tblstudent.`Status`=1 and tblstudent.Sex LIKE '" + @prefixText + "%'";
            }
            else if (_type == "1")
            {
                sql = "SELECT tblreligion.Religion from tblreligion where tblreligion.Religion LIKE '" + @prefixText + "%'";
            }
            else if (_type == "2")
            {
                sql = "SELECT tblcast.castname from tblcast where tblcast.castname LIKE '" + @prefixText + "%'";
            }
            else
            {
                sql = "SELECT tblbloodgrp.GroupName from tblbloodgrp where tblbloodgrp.GroupName LIKE '" + @prefixText + "%'";
            }

            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
            }
            return items;
        }


        [WebMethod(EnableSession=true)]




        public string[] GetHistStudentName(string prefixText, int count, string contextKey)
        {

            // int count = 10;
            string[] _temp = contextKey.Split('\\'); 
            string _Batch = _temp[0];
            string _Class = _temp[1];
            string _Status = _temp[2];
            string _User = _temp[3];
            InitilizeObjects();
            string sql;
            if (_Status == "1")
            {
                sql = "SELECT tblview_student.StudentName  FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id  and tblview_studentclassmap.ClassId = " + _Class + "  and tblview_studentclassmap.BatchId = "+ _Batch+" inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId inner join tblbatch on tblbatch.Id= tblview_studentclassmap.BatchId AND tblbatch.`Status`=1 where  tblview_student.`Status`=1 AND tblview_student.StudentName Like '" + @prefixText + "%' AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _User + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _User + " )";
            }
            else
            {
                sql = "SELECT tblview_student.StudentName  FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id  and tblview_studentclassmap.ClassId = " + _Class + "  and tblview_studentclassmap.BatchId = " + _Batch + " inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId inner join tblbatch on tblbatch.Id= tblview_studentclassmap.BatchId AND tblbatch.`Status`=1 where  tblview_student.`Status`=0 AND tblview_student.StudentName Like '" + @prefixText + "%' AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _User + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _User + " )";
            }
            //string sql = "Select tblstudent.StudentName from tblstudent Where tblstudent.StudentName like '" + @prefixText + "%' and tblstudent.Status=1";
            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
            }
            return items;
        }


        [WebMethod(EnableSession=true)]
        public string[] GetHistAdNo(string prefixText, int count, string contextKey)
        {

            // int count = 10;
            string[] _temp = contextKey.Split('\\');
            string _Batch = _temp[0];
            string _Class = _temp[1];
            string _Status = _temp[2];
            string _User = _temp[3];
            InitilizeObjects();
            string sql;
            if (_Status == "1")
            {
                sql = "SELECT tblview_student.AdmitionNo  FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id  and tblview_studentclassmap.ClassId = " + _Class + "  and tblview_studentclassmap.BatchId = " + _Batch + " inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId inner join tblbatch on tblbatch.Id= tblview_studentclassmap.BatchId AND tblbatch.`Status`=1 where  tblview_student.`Status`=1 AND tblview_student.StudentName Like '" + @prefixText + "%' AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _User + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _User + " )";
            }
            else
            {
                sql = "SELECT tblview_student.AdmitionNo  FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id  and tblview_studentclassmap.ClassId = " + _Class + "  and tblview_studentclassmap.BatchId = " + _Batch + " inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId inner join tblbatch on tblbatch.Id= tblview_studentclassmap.BatchId AND tblbatch.`Status`=1 where  tblview_student.`Status`=0 AND tblview_student.StudentName Like '" + @prefixText + "%' AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _User + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + _User + " )";
            }
            //string sql = "Select tblstudent.StudentName from tblstudent Where tblstudent.StudentName like '" + @prefixText + "%' and tblstudent.Status=1";
            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);

            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
            }
            return items;
        }

        #region Library

        [WebMethod(EnableSession=true)]
        public string[] GetBookData(string prefixText, string contextKey)
        {
            InitilizeObjects();
            string[] items = null;
            string sql = "";
            if (contextKey == "0")
            {
                sql = "Select DISTINCT tblbooks.BookNo from tblbookmaster inner join tblbooks on tblbooks.BookId = tblbookmaster.Id  Where tblbooks.BookNo like '" + @prefixText + "%'";
            }
            else if (contextKey == "1")
            {
                sql = "Select DISTINCT tblbookmaster.BookName from tblbookmaster Where tblbookmaster.BookName like '" + @prefixText + "%'";
            }
            else if (contextKey == "2")
            {
                sql = "Select DISTINCT tblbookmaster.Author from tblbookmaster Where tblbookmaster.Author like '" + @prefixText + "%'";
            }
            else if (contextKey == "3")
            {
                sql = "Select DISTINCT tblbookmaster.Publisher from tblbookmaster Where tblbookmaster.Publisher like '" + @prefixText + "%'";
            }
            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
            }

            return items;
        }

        [WebMethod(EnableSession=true)]

        public string[] GetBookName(string prefixText)
        {

            // int count = 10;
            InitilizeObjects();
            string sql = "Select DISTINCT tblbookmaster.BookName from tblbookmaster Where tblbookmaster.BookName like '" + @prefixText + "%'";
            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
            }
            return items;
        }

        [WebMethod(EnableSession=true)]

        public string[] GetBookId(string prefixText, string contextKey)
        {

            // int count = 10;
             string sql="";
            InitilizeObjects();
            if(contextKey=="1")
            {
                sql = "Select  tblbooks.BookNo  from tblbookmaster inner join tblbooks on tblbooks.BookId = tblbookmaster.Id  Where tblbooks.BookNo like '" + @prefixText + "%'";
            }
            else if(contextKey=="0")
            {
                sql = "Select distinct tblbookmaster.BookName  from tblbookmaster inner join tblbooks on tblbooks.BookId = tblbookmaster.Id  Where  tblbookmaster.BookName like '" + @prefixText + "%'";
            }

            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
            }
            return items;
        }

        [WebMethod(EnableSession=true)]

        public string[] GetBook_Autor(string prefixText)
        {

            // int count = 10;
            InitilizeObjects();
            string sql = "Select DISTINCT tblbookmaster.Author from tblbookmaster Where tblbookmaster.Author like '" + @prefixText + "%'";
            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr["Author"].ToString(), i);
                i++;
            }
            return items;
        }

        [WebMethod(EnableSession=true)]

        public string[] GetBook_Publisher(string prefixText)
        {

            // int count = 10;
            InitilizeObjects();
            string sql = "Select DISTINCT tblbookmaster.Publisher from tblbookmaster Where tblbookmaster.Publisher like '" + @prefixText + "%'";
            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr["Publisher"].ToString(), i);
                i++;
            }
            return items;
        }

        //[WebMethod(EnableSession=true)]
        //public string[] GetUserName(string prefixText, string contextKey)
        //{
        //    InitilizeObjects();
        //    string[] items = null;
        //    string sql = "";

        //    if (contextKey == "1")
        //    {
        //        sql = "Select tblstudent.AdmitionNo from tblstudent Where tblstudent.AdmitionNo like '" + @prefixText + "%' and tblstudent.`Status`=1";
        //    }
        //    else if (contextKey == "2")
        //    {
        //        sql = "Select tbluser.UserName from tbluser Where tbluser.UserName like '" + @prefixText + "%' and tbluser.Id <>1";
        //    }

        //    OdbcDataAdapter Od = new OdbcDataAdapter(sql, con);
        //    Od.SelectCommand.Parameters.Add("@prefixText", OdbcType.VarChar, 50).Value = prefixText + "%";
        //    DataTable dt = new DataTable();
        //    Od.Fill(dt);
        //    items = new string[dt.Rows.Count];
        //    int i = 0;
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        items.SetValue(dr[0].ToString(), i);
        //        i++;
        //    }

        //    return items;
        //}

        [WebMethod(EnableSession=true)]

        public string[] GetIsssuedBookId(string prefixText, string contextKey)
        {

            // int count = 10;
            string sql="";
            InitilizeObjects();
            if (contextKey == "0")
            {
                sql = "select distinct  tblbookmaster.BookName from tblbookissue inner join tblbooks on tblbooks.BookNo=tblbookissue.BookNo inner join tblbookmaster on tblbookmaster.Id= tblbooks.BookId where tblbookmaster.BookName like '" + @prefixText + "%'";
            }
            else if (contextKey == "1")
            {
                sql = "select tblbookissue.BookNo from tblbookissue Where tblbookissue.BookNo like '" + @prefixText + "%'";
            }
            else if (contextKey == "1")
            {
                sql = "select distinct tblbooks.Barcode  from tblbooks inner join tblbookissue on  tblbookissue.BookNo= tblbooks.BookNo Where tblbooks.Barcode  like  '" + @prefixText + "%'";
            }
            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
            }
            return items;
        }
        #endregion


        [WebMethod(EnableSession=true)]

        public string[] GetFeeBill(string prefixText, int count, string contextKey)
        {
           
            string sql = "";
            InitilizeObjects();
            if (contextKey == "0")
            {
                sql = "select tblfeebill.BillNo  from tblfeebill where tblfeebill.BillNo Like '" + prefixText + "%'";
            }
            else if (contextKey == "1")
            {
                sql = "select tbljoining_feebill.BillNo  from tbljoining_feebill where tbljoining_feebill.BillNo Like '" + prefixText + "%'";
            }


            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr["BillNo"].ToString(), i);
                i++;
            }
            return items;

        }

        [WebMethod(EnableSession=true)]
        public string[] GetItem(string prefixText)
        {
            InitilizeObjects();
            string sql = "SELECT tblinv_item.ItemName from tblinv_item where tblinv_item.ItemName LIKE '" + @prefixText + "%'";
            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr["ItemName"].ToString(), i);
                i++;
            }
            return items;

        }
        //GetStudentNameData
        [WebMethod(EnableSession=true)]
        public string[] GetStudentNameData(string prefixText)
        {

            InitilizeObjects();
            string sql = "";
            sql = "select distinct tblstudent.StudentName from tblstudent where tblstudent.StudentName Like '" + @prefixText + "%' and tblstudent.Status=1";
            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr["StudentName"].ToString(), i);
                i++;
            }
            return items;

        }

        [WebMethod(EnableSession = true)]
        public string[] GetExamNameData(string prefixText)
        {

            InitilizeObjects();
            string sql = "";
            sql = "select distinct tblexammaster.ExamName from tblexammaster where tblexammaster.ExamName Like '" + @prefixText + "%' and tblexammaster.Status=1";
            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr["ExamName"].ToString(), i);
                i++;
            }
            return items;

        }

        [WebMethod(EnableSession=true)]
        public string[] GetGurardianNameData(string prefixText)
        {
            InitilizeObjects();
            string sql = "";
            sql = "select distinct tblstudent.GardianName from tblstudent where tblstudent.GardianName Like '" + @prefixText + "%' and tblstudent.Status=1";
            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr["GardianName"].ToString(), i);
                i++;
            }
            return items;

        }
        [WebMethod(EnableSession=true)]
        public string[] GetPhoneNumberData(string prefixText)
        {

            InitilizeObjects();
            string sql = "";
            sql = "select distinct tblstudent.OfficePhNo from tblstudent where tblstudent.OfficePhNo Like '" + @prefixText + "%' and tblstudent.Status=1";
            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("OfficePhNo");
            DataRow dtRow;
            foreach (DataRow _dr in dt.Rows)
            {
                string[] s=  _dr["OfficePhNo"].ToString().Split(',');
                for (int k = 0; k < s.Length; k++)
                {
                    dtRow = dt1.NewRow();
                    dtRow["OfficePhNo"] = s[k];
                    dt1.Rows.Add(dtRow);

                   
                }
                
            }
            string[] items = new string[dt1.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt1.Rows)
            {
                items.SetValue(dr["OfficePhNo"].ToString(), i);
                i++;
            }
            return items;

        }

        [WebMethod(EnableSession=true)]
        public string[] GetVehicle(string prefixText)
        {
            InitilizeObjects();
            string sql = "SELECT tbl_tr_vehicle.VehicleNo from tbl_tr_vehicle where tbl_tr_vehicle.VehicleNo LIKE '" + @prefixText + "%'";
            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr["VehicleNo"].ToString(), i);
                i++;
            }
            return items;

        }

        [WebMethod(EnableSession=true)]
        public string[] GetIncedents(string prefixText, int count, string contextKey)
        {
           
            // int count = 10;
            string[] _temp = contextKey.Split('\\');
            string _type = _temp[0];
            string _Userid = _temp[1];
            InitilizeObjects();
            string sql;


            if (_type == "0")
            {
                //Student
                sql = "  (select tblincedenthead.Title as Title from tblincedenthead where Title  Like '" + @prefixText + "%') union (select Type as Title   from tblincedenttype where Type  Like '" + @prefixText + "%')   union          (select tbluser.SurName as Title from tbluser where tbluser.SurName Like '" + @prefixText + "%')          union          (select tbluser_history.SurName as Title from tbluser_history where tbluser_history.SurName Like '" + @prefixText + "%')          union          (SELECT tblview_student.StudentName as Title FROM tblview_student where tblview_student.StudentName Like '" + @prefixText + "%')          order by Title  ";
            }
            else
            {
                //Staff
                sql = "  (select tblincedenthead.Title as Title from tblincedenthead where Title  Like '" + @prefixText + "%') union (select Type as Title   from tblincedenttype where Type  Like '" + @prefixText + "%')   union          (select tbluser.SurName as Title from tbluser where tbluser.SurName Like '" + @prefixText + "%')          union          (select tbluser_history.SurName as Title from tbluser_history where tbluser_history.SurName Like '" + @prefixText + "%')          union          (SELECT tblview_student.StudentName as Title FROM tblview_student where tblview_student.StudentName Like '" + @prefixText + "%')          order by Title  ";
            }

            //string sql = "Select tblstudent.StudentName from tblstudent Where tblstudent.StudentName like '" + @prefixText + "%' and tblstudent.Status=1";
            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
            }
            return items;
        }

        [WebMethod(EnableSession=true)]

        public string[] GetTCNo(string prefixText)
        {
         
            InitilizeObjects();
            string sql = " select TcNumber from tbltc where TcNumber Like '" + @prefixText + "%'";
            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr["TcNumber"].ToString(), i);
                i++;
            }
            return items;

        }

        [WebMethod(EnableSession=true)]
        public string[] GetUserName(string prefixText, int count, string contextKey)
        {
           
            string[] _temp = contextKey.Split('\\');
            string _type = _temp[0];
            string _Userid = _temp[1];
            InitilizeObjects();
            string sql="";


            if (_type == "1" && _Userid=="0")
            {
                //Student with admission number   
                sql = "  select distinct tblstudent.AdmitionNo from tblstudent where tblstudent.AdmitionNo Like '" + @prefixText + "%' and tblstudent.Status=1"; 
               
            }
            else if (_type == "1" && _Userid=="1")
            {
                //Student with username
                sql = "select distinct tblstudent.StudentName from tblstudent where tblstudent.StudentName Like '" + @prefixText + "%' and tblstudent.Status=1"; 
            }
            else if (_type == "2" && _Userid == "1")
            {
                //staff with name
                sql = " select distinct tbluser.SurName from tbluser where tbluser.SurName Like  '" + @prefixText + "%'  and tbluser.Status=1 and tbluser.RoleId <> 1";
            }
            else if (_type == "2" && _Userid == "0")
            {
                //Staff with id
                sql = "select distinct tbluser.UserName from tbluser where tbluser.UserName Like '" + @prefixText + "%' and tbluser.Status=1 and tbluser.RoleId <> 1";
            }


            //string sql = "Select tblstudent.StudentName from tblstudent Where tblstudent.StudentName like '" + @prefixText + "%' and tblstudent.Status=1";
            if (sql != "")
            {
                OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
                List<OdbcParameter> lstParameter = new List<OdbcParameter>();
                lstParameter.Add(odbcParameter);
                DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
                string[] items = new string[dt.Rows.Count];
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    items.SetValue(dr[0].ToString(), i);
                    i++;
                }
                return items;
            }
            return null;
        }

        [WebMethod(EnableSession=true)]
        public string[] GetUserNameFromIssueBooks(string prefixText, string contextKey)
        {

            InitilizeObjects();
            string[] items = null;
            string sql = "";
            if (contextKey == "1")
            {
                sql = "select distinct tblstudent.StudentName from tblstudent inner join tblbookissue on tblbookissue.UserId= tblstudent.Id where tblbookissue.UserType=1  and tblstudent.StudentName like '" + @prefixText + "%'";
            }
            else if (contextKey == "2")
            {
                sql = "select distinct tbluser.SurName from tbluser inner join tblbookissue on tblbookissue.UserId= tbluser.Id where tbluser.RoleId<>1 and tblbookissue.UserType=2  and tbluser.UserName like '" + @prefixText + "%'";
            }

            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
            }

            return items;
        }

        [WebMethod(EnableSession = true)]

         public string[] GetItemList(string prefixText,string contextKey)
        {
            string[] _temp = contextKey.Split('\\');
            string _type = _temp[1];
            string locId = _temp[0];
     

            InitilizeObjects();
            string sql = "";

            sql = "select ItemName from tblinv_item inner join tblinv_category on tblinv_category.Id= tblinv_item.Category  ";

            if (int.Parse(_temp[0].ToString()) > 0)
            {
                  sql = sql + "inner join  tblinv_locationitemstock on tblinv_item.Id= tblinv_locationitemstock.ItemId ";             
            }

           sql = sql + "  where  ItemName LIKE '" + @prefixText + "%'";

           if (int.Parse(_temp[0].ToString()) > 0)
           {
               sql = sql + " and tblinv_locationitemstock.LocationId=" + locId;
           }

            if (int.Parse(_temp[1].ToString()) > 0)

                sql =sql+ " and  tblinv_category.Categorytype=" + _temp[1].ToString();

            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
            }
            return items;
        }
        [WebMethod(EnableSession = true)]

        public string[] Get_Subject_Name(string prefixText, int count, string contextKey)
        {

            int Class_Id = 0;
            int.TryParse(contextKey,out Class_Id);
            InitilizeObjects();
            string sql = "";
            if (Class_Id == 0)
            {
                sql = "select tblsubjects.subject_name from tblsubjects inner join tblclasssubmap on tblclasssubmap.SubjectId=tblsubjects.Id where tblsubjects.subject_name Like '" + @prefixText + "%' "; 
            }
            else
            {
                sql = "select tblsubjects.subject_name from tblsubjects inner join tblclasssubmap on tblclasssubmap.SubjectId=tblsubjects.Id where tblclasssubmap.ClassId=" + Class_Id + " and tblsubjects.subject_name Like '" + @prefixText + "%'";
            }

            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
            }
            return items;
        }

        //other loads
        #region loadExtra Details
        public class GenClas
        {
            public string GetDataFromDb(string sql)
            {
                SchoolClass objSchool = (SchoolClass)HttpContext.Current.Session[WinerConstants.SessionSchool];
                KnowinGen _Prntobj = (KnowinGen)HttpContext.Current.Session[WinerConstants.SessionUser];
                StudentManagerClass MyStudMang = new StudentManagerClass(_Prntobj);
                DataSet MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql); //Using ExecuteQueryReturnDataSet from MySqlClass
                ArrayList myArrayList = new ArrayList();
                if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable dtAccountData = MydataSet.Tables[0];
                    for (int i = 0; i < dtAccountData.Rows.Count; i++)
                    {

                        string[] stringArr = dtAccountData.Rows[i].ItemArray.Select(x => x.ToString()).ToArray();
                        myArrayList.Add(stringArr);
                    }
                    string JsonDt = JsonConvert.SerializeObject(myArrayList);
                    return JsonDt;
                }
                return null;
            }
            public string GetDataFromMethod()
            {
                return null;
            }
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string LoadAllClass()
        {
            InitilizeObjects();
            KnowinGen _Prntobj = (KnowinGen)HttpContext.Current.Session[WinerConstants.SessionUser];
            StudentManagerClass MyStudMang = new StudentManagerClass(_Prntobj);
            ArrayList myArrayList = new ArrayList();
            DataSet MydataSet = MyUser.MyAssociatedClass();
          
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                DataTable dtAccountData = MydataSet.Tables[0];
                for (int i = 0; i < dtAccountData.Rows.Count; i++)
                {

                    string[] stringArr = dtAccountData.Rows[i].ItemArray.Select(x => x.ToString()).ToArray();
                    myArrayList.Add(stringArr);
                }
                string JsonDt = JsonConvert.SerializeObject(myArrayList);
                return JsonDt;
            }
            return null;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string LoadAllBloodGroup()
        {
            InitilizeObjects();
            string sql = "SELECT Id,GroupName FROM tblbloodgrp";
            GenClas dt = new GenClas();
            string objDt=dt.GetDataFromDb(sql);
            if( objDt!=null){
                return objDt;
            }
            return null;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string LoadAllReligion()
        {
            InitilizeObjects();
            string sql = "SELECT Id,Religion FROM tblreligion where Religion <>'Other' ";
            GenClas dt = new GenClas();
            string objDt = dt.GetDataFromDb(sql);
            if (objDt != null)
            {
                return objDt;
            }
            return null;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string LoadAllCaste()
        {
            InitilizeObjects();
            string sql = "select tblcast.Id, tblcast.castname from tblcast ORDER BY tblcast.castname ASC";
            GenClas dt = new GenClas();
            string objDt = dt.GetDataFromDb(sql);
            if (objDt != null)
            {
                return objDt;
            }
            return null;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string LoadAllBatch()
        {
            InitilizeObjects();
            string sql = "select Id,BatchName from tblbatch where Created=1 ORDER BY BatchName";
            GenClas dt = new GenClas();
            string objDt = dt.GetDataFromDb(sql);
            if (objDt != null)
            {
                return objDt;
            }
            return null;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string LoadAllStudntType()
        {
            InitilizeObjects();
            string sql = "select Id,TypeName from tblstudtype ";
            GenClas dt = new GenClas();
            string objDt = dt.GetDataFromDb(sql);
            if (objDt != null)
            {
                return objDt;
            }
            return null;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string LoadAllLanguages()
        {
            InitilizeObjects();
            string sql = "select Id,tbllanguage.Language from tbllanguage";
            GenClas dt = new GenClas();
            string objDt = dt.GetDataFromDb(sql);
            if (objDt != null)
            {
                return objDt;
            }
            return null;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string LoadAllGender()
        {
            InitilizeObjects();
            string sql = "select Id,gentname from tblgender";
            GenClas dt = new GenClas();
            string objDt = dt.GetDataFromDb(sql);
            if (objDt != null)
            {
                return objDt;
            }
            return null;
        }
        #endregion
        
        [WebMethod(EnableSession = true)]
        public string GetAllActionDt(string prefixText)
        {
            InitilizeObjects();
            string sql = "";
            sql = "SELECT ActionName,Link,Description FROM tblaction WHERE ActionType != 'No Link' AND Link <> '' AND Link IS NOT NULL AND ActionName LIKE '%" + @prefixText + "%' OR Link LIKE '%" + @prefixText + "%' OR Description LIKE '%" + @prefixText + "%';";
            OdbcParameter odbcParameter = new OdbcParameter() { ParameterName = "@prefixText", OdbcType = OdbcType.VarChar, Size = 50, Value = prefixText + "%" };
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            lstParameter.Add(odbcParameter);
            DataTable dt = MyUser.m_MysqlDb.ExecuteQueryReturnDataTable(sql, lstParameter);
            ArrayList myArrayList = new ArrayList();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataTable dtAccountData = dt;
                for (int i = 0; i < dtAccountData.Rows.Count; i++)
                {
                    string[] stringArr = dtAccountData.Rows[i].ItemArray.Select(x => x.ToString()).ToArray();
                    myArrayList.Add(stringArr);
                }
                string JsonDt = JsonConvert.SerializeObject(myArrayList);
                return JsonDt;
            }
            return null;
        }
        

        #region studentDetails
        //[WebMethod(EnableSession = true)]
        //[ScriptMethod(UseHttpGet = true)]
        //public string getCurrentStudent()
        //{
        //    InitilizeObjects();
        //    return HttpContext.Current.Session["StudId"].ToString();
        //}
        
       
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string[] loadStdntTopDt()
        {
            InitilizeObjects();
            string[] JsonDt = new string[2];
            int studentID = Convert.ToInt32(HttpContext.Current.Session["StudId"]);
            JsonDt[0] = studentID.ToString();
            GenClsForStDtls db = new GenClsForStDtls();
            KnowinGen _Prntobj = (KnowinGen)HttpContext.Current.Session[WinerConstants.SessionUser];
            StudentManagerClass MyStudMang = new StudentManagerClass(_Prntobj);
         //   string sql = "SELECT StudentName,AdmitionNo,StudentId,DATE_FORMAT(DOB,'%d-%M-%Y') AS DOB,Sex,DATE_FORMAT(DateofJoining,'%d-%M-%Y') AS DateofJoining,RollNo,OfficePhNo FROM tblstudent WHERE tblstudent.Id=" + studentID + "";
            string sql = "SELECT StudentName,AdmitionNo,tblstudent.StudentId,tblbloodgrp.GroupName,tblclass.ClassName,DATE_FORMAT(DOB,'%d-%M-%Y') AS DOB,Sex,DATE_FORMAT(DateofJoining,'%d-%M-%Y') AS DateofJoining,tblstudent.RollNo,OfficePhNo FROM tblstudent INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId = tblstudent.Id INNER JOIN tblclass ON tblclass.Id = tblstudentclassmap.ClassId INNER JOIN tblbloodgrp ON tblbloodgrp.Id = tblstudent.BloodGroup WHERE tblstudent.Id=" + studentID + ";";
            JsonDt[1] = db.buildJSONFromDataSet(db.readFromDb(sql));//student_name,adm_No,Stdnt_Id,DOB,Gender,DOJ,gardian_name,Permanent_address,Communication_addrs,Res_Ph,Off_Ph,Roll_no,Loc,State,Nationality,Pin,email...
            if (JsonDt[1] != null)
                return JsonDt;
            return null;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string loadStdntSubMenu()
        {
            InitilizeObjects();
            string _MenuStr;
            KnowinGen _Prntobj = (KnowinGen)HttpContext.Current.Session[WinerConstants.SessionUser];
            KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
            int StudType = (int)(HttpContext.Current.Session["StudType"]);
            StudentManagerClass MyStudMang = new StudentManagerClass(_Prntobj);
            _MenuStr = MyStudMang.GetSubStudentMangerMenuString(1, StudType);
            if ((_MenuStr != null) || (_MenuStr != ""))
                return _MenuStr;
            else
                return null;
        }
        [WebMethod(EnableSession = true)]
        public string[] getStudentDtlsById(string studentId)
        {
            InitilizeObjects();
            string[] JsonDt = new string[2];
            string sql = "SELECT tblstudent.Id,StudentName,AdmitionNo,tblstudent.StudentId,tblclass.ClassName,tblstudent.RollNo,DATE_FORMAT(DOB,'%d-%M-%Y') AS DOB,Sex,tblbloodgrp.GroupName AS BloodGroup,GardianName,OfficePhNo,ResidencePhNo,Email,Address,Addresspresent,tblreligion.Religion,tblcast.castname,tblcast_category.CategoryName,DATE_FORMAT(DateofJoining,'%d-%M-%Y') AS DateofJoining,DATE_FORMAT(DateOfLeaving,'%d-%M-%Y') AS DateOfLeaving,Location,Pin,State,Nationality,AadharNumber AS SocialID,FatherEduQuali,MothersName,MotherEduQuali,FatherOccupation,MotherOccupation,AnnualIncome,MotherTongue,tbllanguage.`Language` AS 1stLanguage,NumberofBrothers,NumberOfSysters,tblbatch.BatchName AS JoiningBatch,CreationTime,tblstudtype.TypeName AS StudentType,tbladmisiontype.Name AS AdmissionType,UseBus,UseHostel FROM tblstudent INNER JOIN tblbatch ON tblbatch.Id = tblstudent.JoinBatch INNER JOIN tbladmisiontype ON tbladmisiontype.Id = tblstudent.AdmissionTypeId INNER JOIN tblstudtype ON tblstudtype.Id = tblstudent.StudTypeId INNER JOIN tblcast ON tblcast.Id = tblstudent.Cast INNER JOIN tbllanguage ON tbllanguage.Id = tblstudent.1stLanguage INNER JOIN tblcast_category ON tblcast_category.Id = tblcast.CategoryId INNER JOIN tblreligion ON tblreligion.Id = tblstudent.Religion INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId = tblstudent.Id INNER JOIN tblclass ON tblclass.Id = tblstudentclassmap.ClassId INNER JOIN tblbloodgrp ON tblbloodgrp.Id = tblstudent.BloodGroup WHERE tblstudent.`Status`='1' AND tblstudent.Id=" + studentId + ";";
            GenClsForStDtls db = new GenClsForStDtls();
            JsonDt[1] = db.buildJSONFromDataSet(db.readFromDb(sql));
            DataSet ImgDs = null;
          
            string Imgpath = "";
            int _stuId = int.Parse(studentId);

          //  #region get img from database
            string Img_name = "_temp_stdnt_img.png";
            string Studentimgpath = HttpContext.Current.Server.MapPath("~/images/");
            KnowinGen _Prntobj = (KnowinGen)HttpContext.Current.Session[WinerConstants.SessionUser];
            KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
            int StudType = (int)(HttpContext.Current.Session["StudType"]);
            StudentManagerClass MyStudMang = new StudentManagerClass(_Prntobj);
            sql = "SELECT tblfileurl.FileBytes,tblfileurl.FilePath from tblfileurl where tblfileurl.UserId=" + _stuId + " and tblfileurl.Type='StudentImage'";
            ImgDs = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (ImgDs != null && ImgDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ImgDs.Tables[0].Rows)
                {

                   // Img_name = dr[1].ToString();
                    byte[] image = (byte[])dr[0];
                    Imgpath = Studentimgpath + Img_name;
                    if (!GetImageLinkForBytes(image, Imgpath))
                    {
                        if (!Directory.Exists(Studentimgpath))
                            Directory.CreateDirectory(Studentimgpath);
                        Imgpath = Studentimgpath + Img_name;
                    }
                }

            }
            else
            {
                if (!Directory.Exists(Studentimgpath))
                    Directory.CreateDirectory(Studentimgpath);
                Imgpath = Studentimgpath + Img_name;
            }
            JsonDt[0] = "images/" + Img_name; ;
            if (JsonDt[1] != null)
                return JsonDt;
            return null;
        }
        public bool GetImageLinkForBytes(byte[] _Imagebytes, string Studentimgpath)
        {
            bool valid = true;
            try
            {
                if (_Imagebytes != null)
                {
                    System.Drawing.Image image;
                    var inputStream = new MemoryStream(_Imagebytes);
                    image = System.Drawing.Image.FromStream(inputStream);
                    using (System.Drawing.Image Img = image)
                    {
                        System.Drawing.Size ThumbNailSize = NewImageSize(Img.Height, Img.Width,400);
                        using (System.Drawing.Image ImgThnail =
                        new System.Drawing.Bitmap(Img, ThumbNailSize.Width, ThumbNailSize.Height))
                        {
                            ImgThnail.Save(Studentimgpath, Img.RawFormat);
                            ImgThnail.Dispose();
                        }
                        Img.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                valid = false;
            }

            return valid;
        }
        public static System.Drawing.Size NewImageSize(int OriginalHeight, int OriginalWidth, double FormatSize)
        {
            System.Drawing.Size NewSize;
            double tempval;

            if (OriginalHeight > FormatSize && OriginalWidth > FormatSize)
            {
                if (OriginalHeight > OriginalWidth)
                    tempval = FormatSize / Convert.ToDouble(OriginalHeight);
                else
                    tempval = FormatSize / Convert.ToDouble(OriginalWidth);

                NewSize = new System.Drawing.Size(Convert.ToInt32(tempval * OriginalWidth), Convert.ToInt32(tempval * OriginalHeight));
            }
            else
                NewSize = new System.Drawing.Size(OriginalWidth, OriginalHeight); return NewSize;
        }

        public class GenClsForStDtls
        {
            public DataSet readFromDb(string sql)
            {
                SchoolClass objSchool = (SchoolClass)HttpContext.Current.Session[WinerConstants.SessionSchool];
                KnowinGen _Prntobj = (KnowinGen)HttpContext.Current.Session[WinerConstants.SessionUser];
                StudentManagerClass MyStudMang = new StudentManagerClass(_Prntobj);
                DataSet MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql); //Using ExecuteQueryReturnDataSet from MySqlClass
                if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
                    return MydataSet;
                return null;
                
            }
            public string buildJSONFromDataSet(DataSet MydataSet)
            {
                string JsonDt = null;
                ArrayList myArrayList = new ArrayList();
                if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable dtAccountData = MydataSet.Tables[0];
                    for (int i = 0; i < dtAccountData.Rows.Count; i++)
                    {
                        string[] stringArr = dtAccountData.Rows[i].ItemArray.Select(x => x.ToString()).ToArray();
                        myArrayList.Add(stringArr);
                    }
                    JsonDt = JsonConvert.SerializeObject(myArrayList);
                    // return JsonDt;
                }
                else
                {
                    JsonDt = null;
                }
                return JsonDt;
            }
            public DataSet GetDtls(DataSet MyDataset)
            {
                int ClassId = -1, PrevClassId = -1, rowId = 0;
                int studentID = Convert.ToInt32(HttpContext.Current.Session["StudId"]);
                KnowinGen _Prntobj = (KnowinGen)HttpContext.Current.Session[WinerConstants.SessionUser];
                StudentManagerClass MyStudMang = new StudentManagerClass(_Prntobj);
                if (MyDataset != null && MyDataset.Tables[0] != null && MyDataset.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = MyDataset.Tables[0];
                    dt.Columns.Add("Result");
                    foreach (DataRow dro in MyDataset.Tables[0].Rows)
                    {
                        string result = "Passed";
                        ClassId = int.Parse(dro["ClassId"].ToString());
                        if (PrevClassId == ClassId)
                        {
                            int id = rowId - 1;
                            MyDataset.Tables[0].Rows[id]["Result"] = "Failed";
                        }
                        PrevClassId = ClassId;
                        dro["Result"] = result;
                        rowId++;
                    }
                    MyDataset.Tables[0].Rows[rowId - 1]["Result"] = MyStudMang.getStatus(studentID);
                }
                return MyDataset;
            }
        }



       
        #endregion




        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string loadMarkAttendanceSubMenu()
        {
            InitilizeObjects();
            string _MenuStr;
            KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
            Attendance MyAttendance = MyUser.GetAttendancetObj();
            ClassOrganiser MyClassMang = new ClassOrganiser(MyAttendance.m_MysqlDb);
            _MenuStr = MyClassMang.GetClassMangSubMenuString(MyUser.UserRoleId, MyUser.SELECTEDMODE);
            if ((_MenuStr != null) || (_MenuStr != ""))
                return _MenuStr;
            else
                return null;
        }
       

        #region helpdocument
    
        [WebMethod(EnableSession = true)]
        public string LoadHelpDocSrch(string keyword)//load help data from central db.not from school db
        {
            InitilizeObjects();
            string sql = "SELECT id,name,description,link,comnNotes,topic,type,youtubeVideoId,videoLink FROM tblknowledgebasemaster WHERE keywords LIKE '%" + keyword + "%' AND tblknowledgebasemaster.`status`=1";
            helpDocCls db = new helpDocCls();
            return db.GetDataFromDb(sql);
        }
        [WebMethod(EnableSession = true)]
        public string LoadMasterDtById(string helpId)//load help data from central db.not from school db
        {
            InitilizeObjects();
            string sql = "SELECT id,name,description,link,comnNotes,topic,type,youtubeVideoId,videoLink FROM tblknowledgebasemaster WHERE id =" + helpId + " AND tblknowledgebasemaster.`status`=1";
            helpDocCls db = new helpDocCls();
            return db.GetDataFromDb(sql);
        }
        [WebMethod(EnableSession = true)]
        public string LoadHelpDocDt(string helpId)//load help data from central db.not from school db
        {
            InitilizeObjects();
            string sql = "SELECT id,masterId,position,steps,imageLink,notes FROM tblknowledgebasedata WHERE tblknowledgebasedata.masterId =" + helpId + " ORDER BY position;";
            helpDocCls db = new helpDocCls();
            return db.GetDataFromDb(sql);
        }
        [WebMethod(EnableSession = true)]
        public string LoadHelpDocHomeDt()//load help data from central db.not from school db
        {
            InitilizeObjects();
            MysqlClass objDB = new MysqlClass(WinerUtlity.CentralConnectionString);
            string sql = "SELECT id,name FROM tblknowledgebasemaster;";
            helpDocCls db = new helpDocCls();
            return db.GetDataFromDb(sql);
        }

        //common class
        public class helpDocCls
        {
            public string GetDataFromDb(string sql)
            {
                MysqlClass objDB = new MysqlClass(WinerUtlity.CentralConnectionString);
                DataSet dt = objDB.ExecuteQueryReturnDataSet(sql);
                ArrayList myArrayList = new ArrayList();
                if (dt != null && dt.Tables[0] != null && dt.Tables[0].Rows.Count > 0)
                {
                    DataTable dtAccountData = dt.Tables[0];
                    for (int i = 0; i < dtAccountData.Rows.Count; i++)
                    {
                        string[] stringArr = dtAccountData.Rows[i].ItemArray.Select(x => x.ToString()).ToArray();
                        myArrayList.Add(stringArr);
                    }
                    string JsonDt = JsonConvert.SerializeObject(myArrayList);
                    return JsonDt;
                }
                return null;
            }
        }
     
        #endregion
        #region MasterItms
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public void ChkSessionState()
        {
            InitilizeObjects();
            KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
            if (MyUser == null)
            {
                // Response.Redirect("sectionerr.htm");
            }
            StudentManagerClass MyStudMang = MyUser.GetStudentObj();
            if (MyStudMang == null)
            {
                // Response.Redirect("RoleErr.htm");
                // no rights for this user.
            }
            else
            {
                if (WinerUtlity.NeedCentrelDB())
                {
                    if (HttpContext.Current.Session[WinerConstants.SessionSchool] == null)
                    {
                        // Response.Redirect("Logout.aspx");
                    }
                    SchoolClass objSchool = (SchoolClass)HttpContext.Current.Session[WinerConstants.SessionSchool];
                }
            }
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string[] LoadSchoolData()
        {
            InitilizeObjects();
            OdbcDataReader MyReader = null;
            KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
            ConfigManager MyConfig = MyUser.GetConfigObj();
            string[] Dt = new string[3];
            string Sql = "SELECT SchoolName,Address,Disc FROM tblschooldetails WHERE Id=1";
            MyReader = MyConfig.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                Dt[0] = MyReader.GetValue(0).ToString();//school name
                Dt[1] = MyReader.GetValue(1).ToString();//shool addres sub
                Dt[2] = MyReader.GetValue(2).ToString();
            }
            return Dt;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string GetMainMenuDt()
        {
            InitilizeObjects();
            string _MenuStr = null;
            int SELECTMODE = MyUser.SELECTEDMODE;
            _MenuStr = MyUser.GetStudentMenuString(SELECTMODE);
            return _MenuStr;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string[] MasterData()
        {
            InitilizeObjects();
            SchoolClass objSchool = (SchoolClass)HttpContext.Current.Session[WinerConstants.SessionSchool];
            string[] JsonDt = new string[5];
            JsonDt[0] = objSchool.SchoolName.ToString();
            JsonDt[1] = MyUser.CurrentBatchName;
            JsonDt[2] = MyUser.FillUserProfileData(MyUser.UserId, "User");
            JsonDt[3] = MyUser.GerFormatedDatVal(System.DateTime.Now);
            JsonDt[4] = "Handler/ImageReturnHandler.ashx?id=" + MyUser.UserId + "&type=StaffImage";
            return JsonDt;

        }


        public string[] StudMasterData()
        {
            InitilizeObjects();
            SchoolClass objSchool = (SchoolClass)HttpContext.Current.Session[WinerConstants.SessionSchool];
            string[] JsonDt = new string[5];
            JsonDt[0] = objSchool.SchoolName.ToString();
            JsonDt[1] = MyUser.CurrentBatchName;
            JsonDt[2] = MyUser.FillstudProfileData(MyUser.User_Id, "User");
            JsonDt[3] = MyUser.GerFormatedDatVal(System.DateTime.Now);
            JsonDt[4] = "Handler/ImageReturnHandler.ashx?id=" + MyUser.UserId + "&type=StaffImage";
            return JsonDt;

        }


        [WebMethod(EnableSession = true)]
        public string getSystNotification(string notLvl, string notTime)//(1,2018-03-07)
        {
             //TODO:check user rights with action id 905(view inbox)
            InitilizeObjects();
            if (MyUser.HaveActionRignt(905))
            {
                string sql = "SELECT UserName,Action,TIme,Description FROM tbllog WHERE NotificationLevel='" + notLvl + "' AND DATE(tbllog.Time) = '" + notTime + "' ORDER BY tbllog.Time ASC";
                GenClas dt = new GenClas();
                string objDt = dt.GetDataFromDb(sql);
                if (objDt != null)
                    return objDt;
            }
            return null;
        }
        [WebMethod(EnableSession = true)]
        public string getSystNotificCount(string notLvl, string notTime)//(1,2018-03-07)
        {
            //TODO:check user rights with action id 905(view inbox)
            InitilizeObjects();
            if (MyUser.HaveActionRignt(905))
            {
                string sql = "SELECT COUNT(*) FROM tbllog WHERE NotificationLevel='" + notLvl + "' AND DATE(tbllog.Time) = '" + notTime + "' ORDER BY tbllog.Time DESC";
                GenClas dt = new GenClas();
                string objDt = dt.GetDataFromDb(sql);
                if (objDt != null)
                    return objDt;
            }
            return null;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string getMessageCount()
        {
            //TODO:check user rights with action id 3028(view system notifications)
            InitilizeObjects();
            if (MyUser.HaveActionRignt(3028))
            {
                string sql = "SELECT COUNT(*) FROM tblparent_mail WHERE MessageReadStatus='UNREAD'";
                GenClas dt = new GenClas();
                string objDt = dt.GetDataFromDb(sql);
                if (objDt != null)
                    return objDt;
            }
           
            return null;
        }
        #endregion
        #region common Utils
        public string DataTableToJSON(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }  
        #endregion
        //SELECT UserName,Action,TIme,Description FROM tbllog WHERE NotificationLevel='1' AND DATE(tbllog.Time) = '2018-03-07'
        //SELECT COUNT(*) FROM tblparent_mail WHERE MessageReadStatus='UNREAD'
    }
}
