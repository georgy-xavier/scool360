using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WC.WinerSchool.BOL;
using WC.WinerSchool.DL;
using System.Web.Script.Serialization;
using OTPLibrary;
using OTPLibrary.Model;

namespace WC.WinerSchool.BL
{
    public class WinErBLClass
    {

        #region WCF methods

        public string Register_Device(string key, string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                #region GetSchool_Id
                byte[] encodedDataAsBytes = System.Convert.FromBase64String(key);
                string DecodeKey = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

                if (key.Equals(""))
                    return strReturn;
                string[] results = DecodeKey.Split('_');
                int _SchoolId = Convert.ToInt32(results[0]);                
                
                if (_SchoolId <= 0)
                    return Constants.ReturnStatus.CANNOTCONNECTTOTHEDATABASE;
                #endregion

                #region Deserialize JSON
                if (string.IsNullOrEmpty(json))
                    return strReturn;
                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                RegisterationDetails objRegisterationDetails = oSerializer.Deserialize<RegisterationDetails>(json);

                if (objRegisterationDetails == null)
                    return strReturn;
                #endregion

                #region Register Device
                DataManager objDL = new DataManager(_SchoolId);
                int intDeviceid = objDL.Save_Device(objRegisterationDetails);
                #endregion

                #region Get School Details
                School_Details objSchool = new School_Details();
                objSchool = objDL.GetSchoolDetails();
                #endregion

                #region Serialize JSON
                if (intDeviceid > 0)
                {
                    objRegisterationDetails.DeviceId = intDeviceid;
                    objRegisterationDetails.SchoolDetails = objSchool;
                    string sJSON = oSerializer.Serialize(objRegisterationDetails);
                    strReturn = sJSON;
                }
                #endregion
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

        public string GetRegisterd_Device_Details(string key, string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                #region GetSchoolId
                int _SchoolId = Convert.ToInt32(key);
                if (_SchoolId <= 0)
                    return Constants.ReturnStatus.CANNOTCONNECTTOTHEDATABASE;
                #endregion

                #region Deserialize JSON
                if (string.IsNullOrEmpty(json))
                    return strReturn;
                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                String _MAC_Address = oSerializer.Deserialize<String>(json);

                if (_MAC_Address == null)
                    return strReturn;
                #endregion

                #region Read Device Details
                DataManager objDL = new DataManager(_SchoolId);
                RegisterationDetails objDeviceDetails = objDL.ReadDetails(_MAC_Address);
                #endregion

                #region Serialize JSON
                if (objDeviceDetails != null)
                {
                    oSerializer.MaxJsonLength = 10000000;
                    string sJSON = oSerializer.Serialize(objDeviceDetails);
                    strReturn = sJSON;
                    
                }
                #endregion
            }
            catch
            {
                return strReturn;
            }
            return strReturn;
        }

        public string User_Authentication(string key,string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                #region key Split
                if (key.Equals(""))
                    return strReturn;
                string[] results = key.Split('|');
                int School_Id = Convert.ToInt32(results[0]);
                int Device_Id = Convert.ToInt32(results[1]);
                #endregion

                #region Device Varification

                if (Device_Id <= 0)
                    return Constants.ReturnStatus.UNKNOWNDEVICE;
                //Validate device in db
                DataManager objDL = new DataManager(School_Id);
                tbldevice objdevice = objDL.CheckDevice(Device_Id);
                if (objdevice == null)
                    return Constants.ReturnStatus.UNKNOWNDEVICE;

                if (objdevice.IsActive.Equals(false))
                    return Constants.ReturnStatus.DEVICEACTIVATIONPANTING;

                #endregion

                #region Deserialize JSON

                if (string.IsNullOrEmpty(json))
                    return strReturn;
                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                AutenticationInput objInput = oSerializer.Deserialize<AutenticationInput>(json);
                if (objInput == null)
                    return strReturn;

                #endregion                

                #region Authenticat User
                tbluser objUser = objDL.GetUser(objInput.UserName);
                if (objUser == null)
                {
                    return Constants.ReturnStatus.UNKNOWNUSER;
                }
                else
                {
                    KnowinEncryption objCrypt = new KnowinEncryption();
                    string strDecPassword = objCrypt.Decrypt(objUser.Password);
                    if (!objInput.Password.Equals(strDecPassword))
                    {
                        return Constants.ReturnStatus.WRONGPASSWORD;
                    }
                }
                #endregion

                #region Create JSON Object
                UserDetails objUserDetails = new UserDetails();
                KnowinEncryption objCryp = new KnowinEncryption();
                tblrole objtblrole = new tblrole();
                objtblrole = objDL.GetRole(Convert.ToInt32(objUser.RoleId));

                objUserDetails.Id = Convert.ToInt32(objUser.Id);
                objUserDetails.UserName = objUser.UserName;
                objUserDetails.Password = objCryp.Decrypt(objUser.Password);
                objUserDetails.DispName = objUser.SurName;
                objUserDetails.RoleId = objUser.RoleId != null ? (int)objUser.RoleId : -1;
                objUserDetails.RoleName = objtblrole.RoleName;
                objUserDetails.RoleType = objtblrole.Type;

                KnowinEncryption objEnCrypt = new KnowinEncryption();
                string SessionKey = key + "|" + DateTime.Today.ToString("MM|dd|yyyy");//key- SchholID|DeviceID|Today's_Date
                objUserDetails.SessionKey = objEnCrypt.Encrypt(SessionKey);                
                
                string sJSON = oSerializer.Serialize(objUserDetails);
                strReturn = sJSON;
                #endregion
            }
            catch (Exception)
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }

            return strReturn;
        }

        public string Get_StudentList(string key, string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                #region Session Verification
                if (key == "")
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                KnowinEncryption objDeCrypt = new KnowinEncryption();
                string DeCrptKey = objDeCrypt.Decrypt(key);
                int DeviceId,SchoolId;
                DateTime KeyDate;
                CommonUtility.SplitKey(DeCrptKey, out SchoolId, out DeviceId, out KeyDate);
                //DateTime Today = Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy"));
                int result = DateTime.Compare(KeyDate.Date, DateTime.Now.Date);
                if (result != 0)
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                #endregion

                #region Deserialize JSON

                if (string.IsNullOrEmpty(json))
                    return strReturn;

                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                StudentSyncInput objStudentSyncInput = oSerializer.Deserialize<StudentSyncInput>(json);

                if (objStudentSyncInput == null)
                    return strReturn;
                #endregion

                #region Get Students
                int count;
                DataManager objDL = new DataManager(SchoolId);
                StudentDetails[] lstStudent = objDL.GetStudentList(objStudentSyncInput, out count);
                int[] del_list = objDL.GetDeletedDataList(objStudentSyncInput.SyncDate, "tblstudent");
                if ((lstStudent == null || lstStudent.Length == 0) && (del_list == null || del_list.Length == 0))
                {
                    return Constants.ReturnStatus.SUCCESS;
                }
                #endregion

                #region Serialize
                StudentSyncOutput objStudout = new StudentSyncOutput();
                objStudout.studList = lstStudent;
                objStudout.StudCount = count;
                objStudout.DeletedList = del_list;
                oSerializer.MaxJsonLength = 999999999;
                string sJSON = oSerializer.Serialize(objStudout);
                strReturn = sJSON;
                #endregion 
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

        public string Get_ClassList(string key, string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                #region Session Verification
                if (key == "")
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                KnowinEncryption objDeCrypt = new KnowinEncryption();
                string DeCrptKey = objDeCrypt.Decrypt(key);
                int DeviceId, SchoolId;
                DateTime KeyDate;
                CommonUtility.SplitKey(DeCrptKey, out SchoolId, out DeviceId, out KeyDate);
                //DateTime Today = Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy"));
                int result = DateTime.Compare(KeyDate.Date, DateTime.Now.Date);
                if (result != 0)
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                #endregion

                #region Deserialize JSON

                if (string.IsNullOrEmpty(json))
                    return strReturn;

                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                ClassSyncInput objSyncInput = oSerializer.Deserialize<ClassSyncInput>(json);

                if (objSyncInput == null)
                    return strReturn;
                #endregion

                #region Get ClassList
                int count;
                DataManager objDL = new DataManager(SchoolId);
                ClassDetails[] lstClass = objDL.GetClassList(objSyncInput, out count);
                int[] del_list = objDL.GetDeletedDataList(objSyncInput.SyncDate, "tblclass");
                if ((lstClass == null || lstClass.Length == 0) && (del_list == null || del_list.Length == 0))
                {
                    return Constants.ReturnStatus.SUCCESS;
                }
                #endregion

                #region Serialize
                ClassSyncOutput objClassout = new ClassSyncOutput();
                objClassout.ClassList = lstClass;
                objClassout.ClassCount = count;
                objClassout.DeletedList = del_list;
                oSerializer.MaxJsonLength = 10000000;
                string sJSON = oSerializer.Serialize(objClassout);
                strReturn = sJSON;
                #endregion
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

        public string Get_BatchList(string key, string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                #region Session Verification
                if (key == "")
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                KnowinEncryption objDeCrypt = new KnowinEncryption();
                string DeCrptKey = objDeCrypt.Decrypt(key);
                int DeviceId, SchoolId;
                DateTime KeyDate;
                CommonUtility.SplitKey(DeCrptKey, out SchoolId, out DeviceId, out KeyDate);
                //DateTime Today = Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy"));
                int result = DateTime.Compare(KeyDate.Date, DateTime.Now.Date);
                if (result != 0)
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                #endregion

                #region Deserialize JSON

                if (string.IsNullOrEmpty(json))
                    return strReturn;

                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                BatchSyncInput objSyncInput = oSerializer.Deserialize<BatchSyncInput>(json);

                if (objSyncInput == null)
                    return strReturn;
                #endregion

                #region Get BatchList
                int count;
                DataManager objDL = new DataManager(SchoolId);
                BatchDetails[] lstBatch = objDL.GetBatchList(objSyncInput, out count);
                int[] del_list = objDL.GetDeletedDataList(objSyncInput.SyncDate, "tblbatch");
                if ((lstBatch == null || lstBatch.Length == 0) && (del_list == null || del_list.Length == 0))
                {
                    return Constants.ReturnStatus.SUCCESS;
                }
                #endregion

                #region Serialize
                BatchSyncOutput objBatchout = new BatchSyncOutput();
                objBatchout.BatchList = lstBatch;
                objBatchout.BatchCount = count;
                objBatchout.DeletedList = del_list;
                oSerializer.MaxJsonLength = 10000000;
                string sJSON = oSerializer.Serialize(objBatchout);
                strReturn = sJSON;
                #endregion
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

        public string Get_UserList(string key, string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                #region Session Verification
                if (key == "")
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                KnowinEncryption objDeCrypt = new KnowinEncryption();
                string DeCrptKey = objDeCrypt.Decrypt(key);

                string[] results = DeCrptKey.Split('|');
                int SchoolId = Convert.ToInt32(results[0]);
                int Device_Id = Convert.ToInt32(results[1]);
                #endregion

                #region Deserialize JSON

                if (string.IsNullOrEmpty(json))
                    return strReturn;

                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                UserSyncInput objSyncInput = oSerializer.Deserialize<UserSyncInput>(json);

                if (objSyncInput == null)
                    return strReturn;
                #endregion

                #region Get UserList
                int count;
                DataManager objDL = new DataManager(SchoolId);
                UserDetails[] lstUser = objDL.GetUserList(objSyncInput, out count);
                int[] del_list = objDL.GetDeletedDataList(objSyncInput.SyncDate,"tbluser");
                if ((lstUser == null || lstUser.Length == 0) && (del_list == null || del_list.Length == 0))
                {
                    return Constants.ReturnStatus.SUCCESS;
                }
                #endregion

                #region Serialize
                UserSyncOutput objUserout = new UserSyncOutput();
                objUserout.UserList = lstUser;
                objUserout.UserCount = count;
                objUserout.DeletedList = del_list;
                oSerializer.MaxJsonLength = 10000000;
                string sJSON = oSerializer.Serialize(objUserout);
                strReturn = sJSON;
                #endregion
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

        public string Get_FeeDetailsList(string key, string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                #region Session Verification
                if (key == "")
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                KnowinEncryption objDeCrypt = new KnowinEncryption();
                string DeCrptKey = objDeCrypt.Decrypt(key);
                int DeviceId, SchoolId;
                DateTime KeyDate;
                CommonUtility.SplitKey(DeCrptKey, out SchoolId, out DeviceId, out KeyDate);
                //DateTime Today = Convert.ToDateTime(KeyDate.Date, DateTime.Now.Date);
                int result = DateTime.Compare(KeyDate.Date, DateTime.Now.Date);
                if (result != 0)
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                #endregion
 
                #region Deserialize JSON

                if (string.IsNullOrEmpty(json))
                    return strReturn;

                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                FeeDetailsSyncInput objSyncInput = oSerializer.Deserialize<FeeDetailsSyncInput>(json);

                if (objSyncInput == null)
                    return strReturn;
                #endregion

                #region Get Fee Details List
                int count;
                DataManager objDL = new DataManager(SchoolId);
                FeeDetails[] lstFeeDetails = objDL.GetFeeDetailsList(objSyncInput, out count);
                int[] del_list = objDL.GetDeletedDataList(objSyncInput.SyncDate, "tblfeestudent");
                if ((lstFeeDetails == null || lstFeeDetails.Length == 0) && (del_list == null || del_list.Length == 0))
                {
                    return Constants.ReturnStatus.SUCCESS;
                }
                #endregion

                #region Serialize
                FeeDetailsSyncOutput objFeeDetailsout = new FeeDetailsSyncOutput();
                objFeeDetailsout.FeeDetailsList = lstFeeDetails;
                objFeeDetailsout.FeeDetailsCount = count;
                objFeeDetailsout.DeletedList = del_list;
                oSerializer.MaxJsonLength = 10000000;
                string sJSON = oSerializer.Serialize(objFeeDetailsout);
                strReturn = sJSON;
                #endregion
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

        public string Get_PeriodDetailsList(string key, string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                #region Session Verification
                if (key == "")
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                KnowinEncryption objDeCrypt = new KnowinEncryption();
                string DeCrptKey = objDeCrypt.Decrypt(key);
                int DeviceId, SchoolId;
                DateTime KeyDate;
                CommonUtility.SplitKey(DeCrptKey, out SchoolId, out DeviceId, out KeyDate);
                //DateTime Today = Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy"));
                int result = DateTime.Compare(KeyDate.Date, DateTime.Now.Date);
                if (result != 0)
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                #endregion

                #region Deserialize JSON

                if (string.IsNullOrEmpty(json))
                    return strReturn;

                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                PeriodDetailsSyncInput objSyncInput = oSerializer.Deserialize<PeriodDetailsSyncInput>(json);

                if (objSyncInput == null)
                    return strReturn;
                #endregion

                #region Get Period Details List
                int count;
                DataManager objDL = new DataManager(SchoolId);
                PeriodDetails[] lstPeriodDetails = objDL.GetPeriodDetailsList(objSyncInput, out count);
                int[] del_list = objDL.GetDeletedDataList(objSyncInput.SyncDate, "tblperiod");
                if ((lstPeriodDetails == null || lstPeriodDetails.Length == 0) && (del_list == null || del_list.Length == 0))
                {
                    return Constants.ReturnStatus.SUCCESS;
                }
                #endregion

                #region Serialize
                PeriodDetailsSyncOutput objPeriodDetailsout = new PeriodDetailsSyncOutput();
                objPeriodDetailsout.PeriodDetailsList = lstPeriodDetails;
                objPeriodDetailsout.PeriodDetailsCount = count;
                objPeriodDetailsout.DeletedList = del_list;
                oSerializer.MaxJsonLength = 10000000;
                string sJSON = oSerializer.Serialize(objPeriodDetailsout);
                strReturn = sJSON;
                #endregion
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

        public string Get_ConfigurationDetailsList(string key, string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                #region Session Verification
                if (key == "")
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                KnowinEncryption objDeCrypt = new KnowinEncryption();
                string DeCrptKey = objDeCrypt.Decrypt(key);
                int DeviceId, SchoolId;
                DateTime KeyDate;
                CommonUtility.SplitKey(DeCrptKey, out SchoolId, out DeviceId, out KeyDate);
                //DateTime Today = Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy"));
                int result = DateTime.Compare(KeyDate.Date, DateTime.Now.Date);
                if (result != 0)
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                #endregion

                #region Deserialize JSON

                if (string.IsNullOrEmpty(json))
                    return strReturn;

                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                ConfigDetailsSyncInput objSyncInput = oSerializer.Deserialize<ConfigDetailsSyncInput>(json);

                if (objSyncInput == null)
                    return strReturn;
                #endregion

                #region Get Configuration Details List
                int count;
                DataManager objDL = new DataManager(SchoolId);
                ConfigurationDetails[] lstConfigDetails = objDL.GetConfigDetailsList(objSyncInput, out count);
                int[] del_list = objDL.GetDeletedDataList(objSyncInput.SyncDate, "tblconfiguration");
                if ((lstConfigDetails == null || lstConfigDetails.Length == 0) && (del_list == null || del_list.Length == 0))
                {
                    return Constants.ReturnStatus.SUCCESS;
                }
                #endregion

                #region Serialize
                ConfigDetailsSyncOutput objDetailsout = new ConfigDetailsSyncOutput();
                objDetailsout.ConfigDetailsList = lstConfigDetails;
                objDetailsout.ConfigDetailsCount = count;
                objDetailsout.DeletedList = del_list;
                oSerializer.MaxJsonLength = 10000000;
                string sJSON = oSerializer.Serialize(objDetailsout);
                strReturn = sJSON;
                #endregion
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

        public string Get_OtherFeeMasterList(string key, string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                #region Session Verification
                if (key == "")
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                KnowinEncryption objDeCrypt = new KnowinEncryption();
                string DeCrptKey = objDeCrypt.Decrypt(key);
                int DeviceId, SchoolId;
                DateTime KeyDate;
                CommonUtility.SplitKey(DeCrptKey, out SchoolId, out DeviceId, out KeyDate);
                //DateTime Today = Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy"));
                int result = DateTime.Compare(KeyDate.Date, DateTime.Now.Date);
                if (result != 0)
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                #endregion

                #region Deserialize JSON

                if (string.IsNullOrEmpty(json))
                    return strReturn;

                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                OtherFeeMasterSyncInput objSyncInput = oSerializer.Deserialize<OtherFeeMasterSyncInput>(json);

                if (objSyncInput == null)
                    return strReturn;
                #endregion

                #region Get Other Fee Master List
                int count;
                DataManager objDL = new DataManager(SchoolId);
                OtherFeeDetails[] lstOtherFeeDetails = objDL.GetOtherFeeMasterList(objSyncInput, out count);
                int[] del_list = objDL.GetDeletedDataList(objSyncInput.SyncDate, "tblfeeothermaster");
                if ((lstOtherFeeDetails == null || lstOtherFeeDetails.Length == 0) && (del_list == null || del_list.Length == 0))
                {
                    return Constants.ReturnStatus.SUCCESS;
                }
                #endregion

                #region Serialize
                OtherFeeMasterSyncOutput objDetailsout = new OtherFeeMasterSyncOutput();
                objDetailsout.OtherFeeMasterList = lstOtherFeeDetails;
                objDetailsout.OtherFeeMasterCount = count;
                objDetailsout.DeletedList = del_list;
                oSerializer.MaxJsonLength = 10000000;
                string sJSON = oSerializer.Serialize(objDetailsout);
                strReturn = sJSON;
                #endregion
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

        public string Get_FeeAdvanceDetailsList(string key, string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                #region Session Verification
                if (key == "")
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                KnowinEncryption objDeCrypt = new KnowinEncryption();
                string DeCrptKey = objDeCrypt.Decrypt(key);
                int DeviceId, SchoolId;
                DateTime KeyDate;
                CommonUtility.SplitKey(DeCrptKey, out SchoolId, out DeviceId, out KeyDate);
                //DateTime Today = Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy"));
                int result = DateTime.Compare(KeyDate.Date, DateTime.Now.Date);
                if (result != 0)
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                #endregion

                #region Deserialize JSON

                if (string.IsNullOrEmpty(json))
                    return strReturn;

                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                FeeAdvanceDetailsSyncInput objSyncInput = oSerializer.Deserialize<FeeAdvanceDetailsSyncInput>(json);

                if (objSyncInput == null)
                    return strReturn;
                #endregion

                #region Get Fee Advance Details List
                int count;
                DataManager objDL = new DataManager(SchoolId);
                FeeAdvanceDetails[] lstFeeAdvanceDetails = objDL.GetFeeAdvanceDetailsList(objSyncInput, out count);
                int[] del_list = objDL.GetDeletedDataList(objSyncInput.SyncDate, "tblstudentfeeadvance");
                if ((lstFeeAdvanceDetails == null || lstFeeAdvanceDetails.Length == 0) && (del_list == null || del_list.Length == 0))
                {
                    return Constants.ReturnStatus.SUCCESS;
                }
                #endregion

                #region Serialize
                FeeAdvanceDetailsSyncOutput objDetailsout = new FeeAdvanceDetailsSyncOutput();
                objDetailsout.FeeAdvanceDetailsList = lstFeeAdvanceDetails;
                objDetailsout.FeeAdvanceDetailsCount = count;
                objDetailsout.DeletedList = del_list;
                oSerializer.MaxJsonLength = 10000000;
                string sJSON = oSerializer.Serialize(objDetailsout);
                strReturn = sJSON;
                #endregion
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

        public string Save_Attendance(string key, string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                #region Session Verification
                if (key == "")
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                KnowinEncryption objDeCrypt = new KnowinEncryption();
                string DeCrptKey = objDeCrypt.Decrypt(key);
                int DeviceId, SchoolId;
                DateTime KeyDate;
                CommonUtility.SplitKey(DeCrptKey, out SchoolId, out DeviceId, out KeyDate);
                //DateTime Today = Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy"));
                int result = DateTime.Compare(KeyDate.Date, DateTime.Now.Date);
                if (result != 0)
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                #endregion

                #region Deserialize JSON

                if (string.IsNullOrEmpty(json))
                    return strReturn;

                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                ClassAttendanceDetails objSyncInput = oSerializer.Deserialize<ClassAttendanceDetails>(json);

                if (objSyncInput == null)
                    return strReturn;
                #endregion

                #region Save attendance list to Databse
                DataManager objDL = new DataManager(SchoolId);
                if (objDL.SaveStudentAttendance(objSyncInput))
                    strReturn = Constants.ReturnStatus.SUCCESS;
                #endregion

            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

        public string Download_Attendance(string key, string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                #region Session Verification
                if (key == "")
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                KnowinEncryption objDeCrypt = new KnowinEncryption();
                string DeCrptKey = objDeCrypt.Decrypt(key);
                int DeviceId, SchoolId;
                DateTime KeyDate;
                CommonUtility.SplitKey(DeCrptKey, out SchoolId, out DeviceId, out KeyDate);
                //DateTime Today = Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy"));
                int result = DateTime.Compare(KeyDate.Date, DateTime.Now.Date);
                if (result != 0)
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                #endregion

                #region Deserialize JSON
                if (string.IsNullOrEmpty(json))
                    return strReturn;

                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                DownloadAttendanceSyncInput objSyncInput = oSerializer.Deserialize<DownloadAttendanceSyncInput>(json);

                if (objSyncInput == null)
                    return strReturn;
                #endregion

                #region Read attendance list from Databse
                DataManager objDL = new DataManager(SchoolId);
                List<ClassAttendanceDetails> objAttendanceList = objAttendanceList = objDL.ReadAttendance(objSyncInput);
                if (objAttendanceList == null || objAttendanceList.Count() == 0)
                {
                    return Constants.ReturnStatus.SUCCESS;
                }
                #endregion

                #region Serialize
                DownloadAttendanceSyncOutput objAttList = new DownloadAttendanceSyncOutput();
                objAttList.AttendanceList = objAttendanceList;
                oSerializer.MaxJsonLength = 10000000;
                string sJSON = oSerializer.Serialize(objAttList);
                strReturn = sJSON;
                #endregion
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

        public string Download_Image(string key, string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                #region Session Verification
                if (key == "")
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                KnowinEncryption objDeCrypt = new KnowinEncryption();
                string DeCrptKey = objDeCrypt.Decrypt(key);
                int DeviceId, SchoolId;
                DateTime KeyDate;
                CommonUtility.SplitKey(DeCrptKey, out SchoolId, out DeviceId, out KeyDate);
                //DateTime Today = Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy"));
                int result = DateTime.Compare(KeyDate.Date, DateTime.Now.Date);
                if (result != 0)
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                #endregion

                #region Deserialize JSON
                if (string.IsNullOrEmpty(json))
                    return strReturn;

                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                ImageSyncInput objSyncInput = oSerializer.Deserialize<ImageSyncInput>(json);

                if (objSyncInput == null)
                    return strReturn;
                #endregion

                #region Read attendance list from Databse
                DataManager objDL = new DataManager(SchoolId);
                int count;
                bool NeedSync;
                ImageDetails[] objImageList = objDL.ReadImageDetails(objSyncInput, out count, out NeedSync);
                int[] del_list = objDL.GetDeletedDataList(objSyncInput.SyncDate, "tblfileurl");
                if (objImageList == null || objImageList.Count() == 0)
                {
                    return Constants.ReturnStatus.SUCCESS;
                }
                #endregion

                #region Serialize
                ImageSyncOutput objout = new ImageSyncOutput();
                objout.ImageList = objImageList;
                objout.DeletedList = del_list;
                objout.NeedSync = NeedSync;
                oSerializer.MaxJsonLength = 999999999;
                string sJSON = oSerializer.Serialize(objout);
                strReturn = sJSON;
                #endregion
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

        public string Upload_OfflineTOserver(string key, string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                #region Session Verification
                if (key == "")
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                KnowinEncryption objDeCrypt = new KnowinEncryption();
                string DeCrptKey = objDeCrypt.Decrypt(key);
                int DeviceId, SchoolId;
                DateTime KeyDate;
                CommonUtility.SplitKey(DeCrptKey, out SchoolId, out DeviceId, out KeyDate);
                //DateTime Today = Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy"));
                int result = DateTime.Compare(KeyDate.Date, DateTime.Now.Date);
                if (result != 0)
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                #endregion

                #region Deserialize JSON
                if (string.IsNullOrEmpty(json))
                    return strReturn;

                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                OfflineUploadSyncInput objSyncInput = oSerializer.Deserialize<OfflineUploadSyncInput>(json);

                if (objSyncInput == null)
                    return strReturn;
                #endregion

                #region Upload Offline To Server
                DataManager objDL = new DataManager(SchoolId);
                bool objAttendanceList = objDL.UploadOfflineWinerData(objSyncInput, DeviceId);
                if (objAttendanceList)
                {
                    return Constants.ReturnStatus.SUCCESS;
                }
                else
                {
                    strReturn = Constants.ReturnStatus.FAILURE;
                }
                #endregion
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

        public string Get_FeebillFormats(string key, string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                #region Session Verification
                if (key == "")
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                KnowinEncryption objDeCrypt = new KnowinEncryption();
                string DeCrptKey = objDeCrypt.Decrypt(key);
                int DeviceId, SchoolId;
                DateTime KeyDate;
                CommonUtility.SplitKey(DeCrptKey, out SchoolId, out DeviceId, out KeyDate);
                //DateTime Today = Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy"));
                int result = DateTime.Compare(KeyDate.Date, DateTime.Now.Date);
                if (result != 0)
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                #endregion

                #region Deserialize JSON

                if (string.IsNullOrEmpty(json))
                    return strReturn;

                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                FeeBillFormatSyncInput objSyncInput = oSerializer.Deserialize<FeeBillFormatSyncInput>(json);

                if (objSyncInput == null)
                    return strReturn;
                #endregion

                #region Get Feebill Format
                int count;
                DataManager objDL = new DataManager(SchoolId);
                FeeBillFormat[] lstFeebillFormatDetails = objDL.GetFeeBillFormatList(objSyncInput, out count);
                int[] del_list = objDL.GetDeletedDataList(objSyncInput.SyncDate, "tblfeebilltype");
                if ((lstFeebillFormatDetails == null || lstFeebillFormatDetails.Length == 0) && (del_list == null || del_list.Length == 0))
                {
                    return Constants.ReturnStatus.SUCCESS;
                }
                #endregion

                #region Serialize
                FeeBillFormatSyncOutput objDetailsout = new FeeBillFormatSyncOutput();
                objDetailsout.FeeBillFormatList = lstFeebillFormatDetails;
                objDetailsout.DetailsCount = count;
                objDetailsout.DeletedList = del_list;
                oSerializer.MaxJsonLength = 10000000;
                string sJSON = oSerializer.Serialize(objDetailsout);
                strReturn = sJSON;
                #endregion
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

        public string Get_Holidays_List(string key, string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                #region Session Verification
                if (key == "")
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                KnowinEncryption objDeCrypt = new KnowinEncryption();
                string DeCrptKey = objDeCrypt.Decrypt(key);
                int DeviceId, SchoolId;
                DateTime KeyDate;
                CommonUtility.SplitKey(DeCrptKey, out SchoolId, out DeviceId, out KeyDate);
                //DateTime Today = Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy"));
                int result = DateTime.Compare(KeyDate.Date, DateTime.Now.Date);
                if (result != 0)
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                #endregion

                #region Deserialize JSON

                if (string.IsNullOrEmpty(json))
                    return strReturn;

                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                HolidaysSyncInput objSyncInput = oSerializer.Deserialize<HolidaysSyncInput>(json);

                if (objSyncInput == null)
                    return strReturn;
                #endregion

                #region Get Holidays List
                int count_config;
                int count_holiday;
                DataManager objDL = new DataManager(SchoolId);
                tbl_holidayconfig[] lst_tblholidayconfig = objDL.Get_tbl_holidayconfig_List(objSyncInput.SyncDate_tblholidayconfig, out count_config);
                tbl_holidays[] lst_tblholidays = objDL.Get_tbl_holidays_List(objSyncInput.SyncDate_tblholidays, out count_holiday);
                int[] DelList_tblholidayconfig = objDL.GetDeletedDataList(objSyncInput.SyncDate_tblholidayconfig, "tblholidayconfig");
                int[] DelList_tblholidays = objDL.GetDeletedDataList(objSyncInput.SyncDate_tblholidayconfig, "tblholiday");
                if ((lst_tblholidayconfig == null || lst_tblholidayconfig.Length == 0) && (DelList_tblholidayconfig == null || DelList_tblholidayconfig.Length == 0) && (lst_tblholidays == null || lst_tblholidays.Length == 0) && (DelList_tblholidays == null || DelList_tblholidays.Length == 0))
                {
                    return Constants.ReturnStatus.SUCCESS;
                }
                #endregion

                #region Serialize
                HolidaysSyncOutput objSyncOut = new HolidaysSyncOutput();
                objSyncOut.HolidayConfig = lst_tblholidayconfig;
                objSyncOut.Holidays = lst_tblholidays;
                objSyncOut.DelList_tblholidayconfig = DelList_tblholidayconfig;
                objSyncOut.DelList_tblholidays = DelList_tblholidays;                
                oSerializer.MaxJsonLength = 10000000;
                string sJSON = oSerializer.Serialize(objSyncOut);
                strReturn = sJSON;
                #endregion
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

        public string Get_FineSettingList(string key, string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                #region Session Verification
                if (key == "")
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                KnowinEncryption objDeCrypt = new KnowinEncryption();
                string DeCrptKey = objDeCrypt.Decrypt(key);
                int DeviceId, SchoolId;
                DateTime KeyDate;
                CommonUtility.SplitKey(DeCrptKey, out SchoolId, out DeviceId, out KeyDate);
                //DateTime Today = Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy"));
                int result = DateTime.Compare(KeyDate.Date, DateTime.Now.Date);
                if (result != 0)
                    return Constants.ReturnStatus.SESSIONEXPIRE;
                #endregion

                #region Deserialize JSON

                if (string.IsNullOrEmpty(json))
                    return strReturn;

                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                FineSettingSyncInput objSyncInput = oSerializer.Deserialize<FineSettingSyncInput>(json);

                if (objSyncInput == null)
                    return strReturn;
                #endregion

                #region Get Fee Advance Details List
                int count;
                DataManager objDL = new DataManager(SchoolId);
                FineSetting[] lstFineSetting = objDL.GetFineSettingList(objSyncInput, out count);
                int[] del_list = objDL.GetDeletedDataList(objSyncInput.SyncDate, "tblfine");
                if ((lstFineSetting == null || lstFineSetting.Length == 0) && (del_list == null || del_list.Length == 0))
                {
                    return Constants.ReturnStatus.SUCCESS;
                }
                #endregion

                #region Serialize
                FineSettingSyncOutput objDetailsout = new FineSettingSyncOutput();
                objDetailsout.FineSettingList = lstFineSetting;
                objDetailsout.DetailsCount = count;
                objDetailsout.DeletedList = del_list;
                oSerializer.MaxJsonLength = 10000000;
                string sJSON = oSerializer.Serialize(objDetailsout);
                strReturn = sJSON;
                #endregion
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

        #endregion

        #region Winer Mobile API Methods


        public string WAPI_RegisterDevice(string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                //TODO: decoding of school verification Id... Now using direct schoolId
                /* 
                #region GetSchool_Id
                byte[] encodedDataAsBytes = System.Convert.FromBase64String(key);
                string DecodeKey = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

                if (key.Equals(""))
                    return strReturn;
                string[] results = DecodeKey.Split('_');
                int _SchoolId = Convert.ToInt32(results[0]);

                if (_SchoolId <= 0)
                    return Constants.ReturnStatus.CANNOTCONNECTTOTHEDATABASE;
                #endregion
                */
                
                #region Deserialize JSON
                if (string.IsNullOrEmpty(json))
                    return strReturn;
                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                WAPI_RegisterationDetails objRegisterationDetails = oSerializer.Deserialize<WAPI_RegisterationDetails>(json);

                if (objRegisterationDetails == null)
                    return strReturn;
                #endregion

                #region Register Device

                if (string.IsNullOrEmpty(objRegisterationDetails.SchoolVerificationId))
                    return strReturn;

                int _schoolId = int.Parse(objRegisterationDetails.SchoolVerificationId);

                DataManager objDL = new DataManager(_schoolId);
                RegisterationDetails deviceObj = new RegisterationDetails
                {
                    DeviceName = objRegisterationDetails.PhoneNumber,
                    DeviceType = "PushDevice",
                    DeviceUniqueId = objRegisterationDetails.DeviceTokenId        
                };
                int intDeviceid = objDL.Save_Device(deviceObj);
                #endregion

                #region Get School Details
                School_Details objSchool = new School_Details();
                objSchool = objDL.GetSchoolDetails();
                if(objSchool != null){
                    objSchool.SchoolImage = null;
                }
                #endregion

                var parentName = objDL.GetParentName(objRegisterationDetails.PhoneNumber);
                if (string.IsNullOrEmpty(parentName))
                    return Constants.ReturnStatus.UNKNOWNUSER;

                OTPManager OTPmanager = new OTPManager(objDL.SchoolConnectionString);
                 OTPmanager.GenerateOTP(new OTPclass
                 {
                     username = objRegisterationDetails.PhoneNumber,
                     entityid = 1,
                     phonenumber = objRegisterationDetails.PhoneNumber
                 });

                #region Serialize JSON
                if (intDeviceid > 0)
                {
                    objRegisterationDetails.DeviceId = intDeviceid;
                    objRegisterationDetails.SchoolDetails = objSchool;
                    objRegisterationDetails.ParentName = parentName;
                    string sJSON = oSerializer.Serialize(objRegisterationDetails);
                    strReturn = sJSON;
                }
                #endregion
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

        public string WAPI_AuthorizeDevice(string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                #region Deserialize JSON
                if (string.IsNullOrEmpty(json))
                    return strReturn;
                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                WAPI_AuthorizeDetails objRegisterationDetails = oSerializer.Deserialize<WAPI_AuthorizeDetails>(json);

                if (objRegisterationDetails == null)
                    return strReturn;
                #endregion


                if (string.IsNullOrEmpty(objRegisterationDetails.SchoolId))
                    return strReturn;

                int School_Id = Convert.ToInt32(objRegisterationDetails.SchoolId);
                int Device_Id = Convert.ToInt32(objRegisterationDetails.DeviceId);

                #region Device Varification

                if (Device_Id <= 0)
                    return Constants.ReturnStatus.UNKNOWNDEVICE;
                //Validate device in db
                DataManager objDL = new DataManager(School_Id);
                tbldevice objdevice = objDL.CheckDevice(Device_Id);
                if (objdevice == null)
                    return Constants.ReturnStatus.UNKNOWNDEVICE;
                #endregion

                OTPManager OTPmanager = new OTPManager(objDL.SchoolConnectionString);
                var valid = OTPmanager.Validate(new OTPclass
                {
                    username = objRegisterationDetails.PhoneNumber,
                    entityid = 1,
                    phonenumber = objRegisterationDetails.PhoneNumber,
                    enterdotp = objRegisterationDetails.OTP
                });

                if (valid)
                {
                    objdevice.IsActive = true;
                    if(objDL.ActivateDevice(objdevice))
                        return Constants.ReturnStatus.SUCCESS;
                }

                return strReturn;
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

        #endregion
    }
}