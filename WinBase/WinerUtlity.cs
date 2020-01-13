using System;
using System.Configuration;
using System.Data;

namespace WinBase
{
    public class WinerUtlity
    {
        public static bool NeedCentrelDB()
        {
            bool NeedCentrelDB=false;

            if (ConfigurationSettings.AppSettings[WinerConstants.ConfigNeedCentralDB] != null)
                if (ConfigurationSettings.AppSettings[WinerConstants.ConfigNeedCentralDB].Equals("True", StringComparison.InvariantCultureIgnoreCase))
                    NeedCentrelDB = true;

            return NeedCentrelDB;
        }

        public static string GetAbsoluteFilePath(WinBase.SchoolClass objSchool, string ServerPath)
        {
            string strAbsoluteFilePath = string.Empty;
            if (objSchool == null)
                strAbsoluteFilePath = ConfigurationSettings.AppSettings["AbsoluteFilePath"];
            else
                strAbsoluteFilePath = ServerPath + "\\FileRepository\\" + objSchool.FilePath;

            if (string.IsNullOrEmpty(strAbsoluteFilePath))
                strAbsoluteFilePath = ServerPath;

           return strAbsoluteFilePath;
        }

        public static string GetParentLoginAbsoluteFilePath(WinBase.SchoolClass objSchool, string ServerPath)
        {
            string strAbsoluteFilePath = string.Empty;
            if (objSchool == null)
                strAbsoluteFilePath = ConfigurationSettings.AppSettings["AbsoluteFilePath"];

            if (string.IsNullOrEmpty(strAbsoluteFilePath))
                strAbsoluteFilePath = ServerPath;

            return strAbsoluteFilePath;
        }

        public static string GetConnectionString(WinBase.SchoolClass objSchool)
        {
            string strConnectionString = string.Empty;
            if (objSchool == null)
                strConnectionString = ConfigurationSettings.AppSettings["ConnectionInfo"];
            else
                strConnectionString = objSchool.ConnectionString;

            return strConnectionString;
        }

        public static string GetRelativeFilePath(WinBase.SchoolClass objSchool)
        {
            string strRelativeFilePath = string.Empty;
            if (objSchool == null)
                strRelativeFilePath = ConfigurationSettings.AppSettings["AbsoluteFilePath"];
            else
                strRelativeFilePath = "FileRepository/" + objSchool.FilePath+"/";
            return strRelativeFilePath;
        }

        public static WinBase.SchoolClass GetSchoolObject(int intSchoolId)
        {
            SchoolClass objSchool = null;
            try
            {
                MysqlClass objDB = new MysqlClass(CentralConnectionString);
                string sql;
                sql = "select tblschool_list.Id, tblschool_list.SchoolName,tblschool_list.ConnectionString, tblschool_list.FilePath,tblschool_list.CustomerId from tblschool_list where tblschool_list.IsActice=1 and tblschool_list.Id=" + intSchoolId;
                DataSet dt = objDB.ExecuteQueryReturnDataSet(sql);

                if (dt != null && dt.Tables[0] != null && dt.Tables[0].Rows.Count > 0)
                {
                    objSchool = new SchoolClass();
                    objSchool.SchoolId = (int)dt.Tables[0].Rows[0][0];
                    objSchool.SchoolName = dt.Tables[0].Rows[0][1].ToString();
                    objSchool.ConnectionString = dt.Tables[0].Rows[0][2].ToString();
                    objSchool.FilePath = dt.Tables[0].Rows[0][3].ToString();
                    objSchool.CustId = dt.Tables[0].Rows[0][4].ToString();
                }

                objDB.CloseConnection();
                objDB = null;

            }
            catch
            {
                objSchool = null;
            }
            return objSchool;
        }

        public static string CentralConnectionString
         {
             get
             {
                 return ConfigurationSettings.AppSettings["CentralConnectionInfo"];
             }
         }

        public static string SingleSchoolConnectionString
        {
            get
            {
                return ConfigurationSettings.AppSettings["ConnectionInfo"];
            }
        }

        public static int GetSchoolId(SchoolClass objSchool)
        {
            if (objSchool != null)
                return objSchool.SchoolId;
            else
                return -1;
        }

        public static int GetUserIndex(int _UserId, int _SchoolId, System.Collections.Generic.List<UserInfoClass> list)
        {
            int _ReturnIndex = -1;
            for (int _i = 0; _i < list.Count; _i++)
            {
                if (list[_i].UserId == _UserId && list[_i].SchoolId == _SchoolId)
                {
                    return _i;
                }
            }
            return _ReturnIndex;
        }

        public static bool UpdateWinerversionDetails(string sql)
        {
            bool valid = true;
            try
            {
                MysqlClass objDB = new MysqlClass(CentralConnectionString);
                objDB.ExecuteQuery(sql);

            }
            catch
            {
                valid= false;
            }
            return valid;
        }

        #region mani about winner page work
        public static DataSet GetaboutsoftwareDetails(string sql)
        {
            DataSet ds = null;
            try
            {
                MysqlClass objDB = new MysqlClass(CentralConnectionString);
                ds=objDB.ExecuteQueryReturnDataSet(sql);
            }
            catch
            {
                ds = null;
            }
            return ds;
        }
        #endregion


      
    }
}
