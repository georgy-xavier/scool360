using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WC.WinerSchool.BOL
{
    public class School_Details
    {
        public int SchoolID;
        public string SchoolName;
        public string Address;
        public string Syllabus;
        public string Mediumofinstruction;
        public string Disc;
        public byte[] SchoolLogo;
        public byte[] SchoolImage;
        public long MaxBillCount;
        public ConfigurationDetails Configuration;
        public int TotalStudents { get; set; }
        public int TotalMale { get; set; }
        public int TotalFemale { get; set; }
        public decimal TotalFees { get; set; }
        public decimal TotalDueFees { get; set; }
    }
}
