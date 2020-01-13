using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WC.WinerSchool.BOL
{
    public class ClassAttendanceDetails
    {
        public int ClassId;
        public int DeviceId;
        public String Att_Date;
        public int Att_Mode;
        public int CreatedUserId;
        public String CreatedUserName;
        public String StrModifiedDate;
        public List<StudentAttendanceStatus> AttendanceList;
    }
}