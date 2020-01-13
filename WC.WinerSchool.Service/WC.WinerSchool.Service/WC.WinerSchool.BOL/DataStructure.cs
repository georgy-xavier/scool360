using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WC.WinerSchool.BOL
{
    public class StudentSyncInput
    {
        public int SchoolId = -1;
        public long SyncDate;
    }
    public class StudentSyncOutput
    {
        public StudentDetails[] studList;
        public int StudCount = -1;
        public int[] DeletedList;
    }

    public class ClassSyncInput
    {
        public int SchoolId = -1;
        public long SyncDate;
    }
    public class ClassSyncOutput
    {
        public ClassDetails[] ClassList;
        public int ClassCount = -1;
        public int[] DeletedList;
    }

    public class UserSyncInput
    {
        public int SchoolId = -1;
        public long SyncDate;
    }
    public class UserSyncOutput
    {
        public UserDetails[] UserList;
        public int UserCount = -1;
        public int[] DeletedList;
    }

    public class BatchSyncInput
    {
        public int SchoolId = -1;
        public long SyncDate;
    }
    public class BatchSyncOutput
    {
        public BatchDetails[] BatchList;
        public int BatchCount = -1;
        public int[] DeletedList;
    }

    public class FeeDetailsSyncInput
    {
        public int SchoolId = -1;
        public long SyncDate;
    }
    public class FeeDetailsSyncOutput
    {
        public FeeDetails[] FeeDetailsList;
        public int FeeDetailsCount = -1;
        public int[] DeletedList;
        
    }

    public class PeriodDetailsSyncInput
    {
        public int SchoolId = -1;
        public long SyncDate;
    }
    public class PeriodDetailsSyncOutput
    {
        public PeriodDetails[] PeriodDetailsList;
        public int PeriodDetailsCount = -1;
        public int[] DeletedList;
    }

    public class ConfigDetailsSyncInput
    {
        public int SchoolId = -1;
        public long SyncDate;
    }
    public class ConfigDetailsSyncOutput
    {
        public ConfigurationDetails[] ConfigDetailsList;
        public int ConfigDetailsCount = -1;
        public int[] DeletedList;
    }

    public class OtherFeeMasterSyncInput
    {
        public int SchoolId = -1;
        public long SyncDate;
    }
    public class OtherFeeMasterSyncOutput
    {
        public OtherFeeDetails[] OtherFeeMasterList;
        public int OtherFeeMasterCount = -1;
        public int[] DeletedList;
    }

    public class FeeAdvanceDetailsSyncInput
    {
        public int SchoolId = -1;
        public long SyncDate;
    }

    public class FeeAdvanceDetailsSyncOutput
    {
        public FeeAdvanceDetails[] FeeAdvanceDetailsList;
        public int FeeAdvanceDetailsCount = -1;
        public int[] DeletedList;
    }

    public class DownloadAttendanceSyncInput
    {
        public int StanderdId;
        public int BatchId;
    }

    public class DownloadAttendanceSyncOutput
    {
        public List<ClassAttendanceDetails> AttendanceList;
    }

    public class ImageSyncInput
    {
        public long SyncDate;
        public int CountOnLastSyncDate;
        public int TotalCount;
    }

    public class ImageSyncOutput
    {
        public ImageDetails[] ImageList;
        public int[] DeletedList;
        public bool NeedSync;
    }

    public class OfflineUploadSyncInput
    {
        public int Id;
        public string TableName;
        public string ColumnName;
        public int ColValue;
        public string TransactionType;
        public DateTime ChangeDate;
        public string FieldStr;
    }

    public class FeeBillFormatSyncInput
    {
        public int SchoolId = -1;
        public long SyncDate;
    }

    public class FeeBillFormatSyncOutput
    {
        public FeeBillFormat[] FeeBillFormatList;
        public int DetailsCount = -1;
        public int[] DeletedList;
    }

    public class FineSettingSyncInput
    {
        public int SchoolId = -1;
        public long SyncDate;
    }

    public class FineSettingSyncOutput
    {
        public FineSetting[] FineSettingList;
        public int DetailsCount = -1;
        public int[] DeletedList;
    }

    public class HolidaysSyncInput
    {
        public long SyncDate_tblholidayconfig;
        public long SyncDate_tblholidays;
    }

    public class HolidaysSyncOutput
    {
        public tbl_holidayconfig[] HolidayConfig;
        public tbl_holidays[] Holidays;
        public int[] DelList_tblholidayconfig;
        public int[] DelList_tblholidays;
    }

    public class FeeBillDatas
    {
       public string StudentId;
       public double TotalAmount;
       public DateTime Date;
       public string PaymentMode;
       public string PaymentModeId;
       public string BankName;
       public string BillNo;
       public int UserId;
       public string UserName;
       public DateTime CreatedDateTime;
       public int Canceled;
       public int Counter;
       public int AccYear;
       public int ClassID;
       public string StudentName;
       public int RegularFee;
       public string TempId;
       public string OtherReference;
    }

    public class FeeTranactionData
    {
        public int UserId;
        public int PaymentElementId;
        public string BillNo;
        public DateTime PaidDate;
        public DateTime CreatedDate;
        public Double Amount;
        public int AccountTo;
        public int AccountFrom;
        public string Type;
        public int TransType;
        public int Canceled;
        public double BalanceAmount;
        public int RegularFee;
        public int BatchId;
        public string BatchName;
        public int ClassId;
        public string StudentName;
        public string CollectedUser;
        public int CollectionType;
        public string FeeName;
        public string PeriodName;
        public string TempId;
        public int PeriodId;
        public int FeeId;
    }
}
