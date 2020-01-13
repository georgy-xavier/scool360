using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WC.WinerSchool.BOL;
using WC.WinerSchool.BL;
using System.ServiceModel.Activation;

namespace WC.WinerSchool.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WinerAPIService" in code, svc and config file together.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WinerAPIService : IWinerAPIService
    {
        WinErBLClass objBL = new WinErBLClass();

        public string Test_Connection(string key)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes("2_sd");
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);

            byte[] encodedDataAsBytes = System.Convert.FromBase64String(returnValue);
            string aaa = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

            return Constants.ReturnStatus.SUCCESS;
        }

        public string Register_Device(string key, string json)
        {
            return objBL.Register_Device(key, json);
        }

        public string GetDevice_Details(string key, string json)
        {
            return objBL.GetRegisterd_Device_Details(key, json);
        }

        public string Login_Validation(string key, string json)
        {
            return objBL.User_Authentication(key, json);
        }

        public string GetStudent_List(string key, string json)
        {
            return objBL.Get_StudentList(key,json);
        }

        public string GetClass_List(string key, string json)
        {
            return objBL.Get_ClassList(key,json);
        }

        public string GetUser_List(string key, string json)
        {
            return objBL.Get_UserList(key, json);
        }

        public string GetBatch_List(string key, string json)
        {
            return objBL.Get_BatchList(key, json);
        }

        public string GetFeeDetails_List(string key, string json)
        {
            return objBL.Get_FeeDetailsList(key, json);
        }

        public string GetPeriodDetails_List(string key, string json)
        {
            return objBL.Get_PeriodDetailsList(key, json);
        }

        public string GetConfigurationDetails_List(string key, string json)
        {
            return objBL.Get_ConfigurationDetailsList(key, json);
        }

        public string GetOtherFeeMaster_List(string key, string json)
        {
            return objBL.Get_OtherFeeMasterList(key, json);
        }

        public string GetFeeAdvanceDetails_List(string key, string json)
        {
            return objBL.Get_FeeAdvanceDetailsList(key, json);
        }

        public string SaveAttendence_Details(string key, string json)
        {
            return objBL.Save_Attendance(key, json);
        }

        public string DownloadAttendence(string key, string json)
        {
            return objBL.Download_Attendance(key, json);           
        }

        public string DownloadImage(string key, string json)
        { 
            return objBL.Download_Image(key, json);
        }

        public string UploadOfflineTOserver(string key, string json)
        {
            return objBL.Upload_OfflineTOserver(key, json);
        }

        public string GetFeebillFormats(string key, string json)
        {
              return objBL.Get_FeebillFormats(key, json);
        } 

        public string GetHolidays_List(string key, string json)
        {
            return objBL.Get_Holidays_List(key, json);
        }

        public string GetFineSetting_List(string key, string json)
        {
            return objBL.Get_FineSettingList(key, json);
        }

        public string GetOfflineWinEr_Version(string key)
        {
            return "1.0.0";
        }
    }
}