using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WC.WinerSchool.BL
{
    public class CommonUtility
    {
        public static void SplitKey(string key,out int SchoolId, out int DeviceId, out DateTime Date)
        {           
            string[] results = key.Split('|');
            SchoolId = Convert.ToInt32(results[0]);
            DeviceId = Convert.ToInt32(results[1]);
            string month = results[2];
            string day= results[3];
            string year = results[4];
            string aaa = month + "/" + day + "/" + year;
            //Date = Convert.ToDateTime(aaa);
            Date = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
        }
    }
}
