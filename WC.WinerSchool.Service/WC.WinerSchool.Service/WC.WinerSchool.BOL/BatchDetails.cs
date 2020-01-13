using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WC.WinerSchool.BOL
{
    public class BatchDetails
    {
        public int Id;
        public String BatchName;
        public DateTime StartDate;
        public DateTime EndDate;
        public int int_StartDate;
        public int int_EndDate;
        public int Status;
        public int IsCreated;
        public int NOfWorkingDays;
        public int LastbatchId;
        public long SyncDate;
    }
}
