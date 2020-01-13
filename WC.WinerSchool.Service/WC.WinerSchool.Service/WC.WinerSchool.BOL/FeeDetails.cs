using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WC.WinerSchool.BOL
{
    public class FeeDetails
    {
        public int FeeStudentId;
        public int FeeScheduleId;
        public int StudentId;
        public double Amount;
        public double BalanceAmount;
        public string Status;
        public int FeeId;
        public string FeeName;
        public int BatchId;
        public string BatchName;
        public DateTime Duedate;
        public DateTime Lastdate;
        public int PeriodId;
        public string PeriodName;
        public int FeeType;
        public long SyncDate;
    }
}
