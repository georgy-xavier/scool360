using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WC.WinerSchool.BOL
{
    public class UserDetails
    {

        public int Id;
        public String UserName;
        public String SurName;
        public String Password;
        public String EmailId;
        public String DispName;
        public int RoleId;
        public String RoleName;
        public String RoleType;
        public long SyncDate;
        public String SessionKey;
        public int CurrentBatchID;
        public string CurrentBatchName;
        public int SchoolID;
        public string SchoolName;
        public int TotalStaffs { get; set; }
        public int TotalMaleStaffs { get; set; }
        public int TotalFemaleStaffs { get; set; }
        


    }

    public class ModuleAction
    {
        public int ActionId { get; set; }
    }
}
