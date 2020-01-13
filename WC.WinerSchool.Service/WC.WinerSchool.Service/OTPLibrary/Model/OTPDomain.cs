using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTPLibrary.Model
{
    public class OTPDomain
    {
        public int OTPId { get; set; }

        public int EntityId { get; set; }

        public string Entityname { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public int Value { get; set; }

        public string currenttime { get; set; }

        public string enterdotp { get; set; }

        public string phonenumber { get; set; }

        public string retrievedotp { get; set; }

        public string msg { get; set; }
    }
}
