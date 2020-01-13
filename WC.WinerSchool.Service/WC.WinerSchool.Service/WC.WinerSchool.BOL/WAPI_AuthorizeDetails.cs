
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WC.WinerSchool.BOL
{
    public class WAPI_AuthorizeDetails
    {
        public String OTP;
        public String DeviceId;
        public String SchoolId;

        public string PhoneNumber { get; set; }
    }
}
