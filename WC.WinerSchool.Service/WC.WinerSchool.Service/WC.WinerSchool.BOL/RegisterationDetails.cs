
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WC.WinerSchool.BOL
{
    public class RegisterationDetails
    {
        public int DeviceId;
        public String DeviceUniqueId;
        public String DeviceName;
        public String AddedUser;
        public String DeviceType;
        public DateTime CreatedDate;
        public DateTime LastLoginDate;
        public bool IsActive;
        public School_Details SchoolDetails;

    }
}
