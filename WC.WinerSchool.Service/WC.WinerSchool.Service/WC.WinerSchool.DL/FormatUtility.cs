using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace WC.WinerSchool.DL
{
    public class FormatUtility
    {
        public static String GetDateString(DateTime _dt)
        {
            return _dt.Year + "_" + _dt.Month + "_" + _dt.Day + "_" + _dt.Hour + "_" + _dt.Minute + "_" + _dt.Second;
        }

        public static String GetDateOnlyString(DateTime _dt)
        {
            return _dt.Year + "/" + _dt.Month + "/" + _dt.Day ;
        }

        public static int GetDateInt(DateTime _dt)
        {
            String DateString = Convert.ToDateTime(_dt).ToString("yyyyMMdd");
            int DateInt = int.Parse(DateString);
            return DateInt;
        }

        public static DateTime GetDate(String str_dt)
        {
            String[] _temp = str_dt.Split('_');
            return new DateTime(int.Parse(_temp[0]), int.Parse(_temp[1]), int.Parse(_temp[2]), int.Parse(_temp[3]), int.Parse(_temp[4]), int.Parse(_temp[5]));
        }
    }
}
