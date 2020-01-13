
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WC.WinerSchool.BOL
{
    public class WAPI_RegisterationDetails
    {
        public String PhoneNumber;
        public String SchoolVerificationId;
        public String DeviceTokenId;
        public int DeviceId { get; set; }
        public School_Details SchoolDetails { get; set; }
        public string ParentName { get; set; }
    }
}
