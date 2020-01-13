using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WC.WinerSchool.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWinerAPIService" in both code and config file together.
    [ServiceContract]
    public interface IWinerAPIService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        string Test_Connection(string key);

        [OperationContract]
        string Register_Device(string key, string json);

        [OperationContract]
        string GetDevice_Details(string key, string json);

        [OperationContract]
        string Login_Validation(string key, string json);

        [OperationContract]
        string GetStudent_List(string key, string json);

        [OperationContract]
        string GetClass_List(string key, string json);

        [OperationContract]
        string GetUser_List(string key, string json);

        [OperationContract]
        string GetBatch_List(string key, string json);

        [OperationContract]
        string GetFeeDetails_List(string key, string json);

        [OperationContract]
        string GetPeriodDetails_List(string key, string json);

        [OperationContract]
        string GetConfigurationDetails_List(string key, string json);

        [OperationContract]
        string GetOtherFeeMaster_List(string key, string json);

        [OperationContract]
        string GetFeeAdvanceDetails_List(string key, string json);

        [OperationContract]
        string SaveAttendence_Details(string key, string json);

        [OperationContract]
        string DownloadAttendence(string key, string json);

        [OperationContract]
        string DownloadImage(string key, string json);

        [OperationContract]
        string UploadOfflineTOserver(string key, string json);

        [OperationContract]
        string GetFeebillFormats(string key, string json);

        [OperationContract]
        string GetHolidays_List(string key, string json);

        [OperationContract]
        string GetFineSetting_List(string key, string json);

        [OperationContract]
        string GetOfflineWinEr_Version(string key);
    }
}
