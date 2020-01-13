using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OTPLibrary.Model
{
    public class OTPclass
    {
        public string username { get; set; }

        public int entityid { get; set; }

        public string password { get; set; }

        public string enterdotp { get; set; }

        public string currenttime { get; set; }

        public string phonenumber { get; set; }

        public string otpid { get; set; }

    }
}
