using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WC.WinerSchool.BOL
{
    public class Constants
    {
        public static class ReturnStatus
        {
            public static string SUCCESS = "S";
            public static string FAILURE = "F";
            public static string SESSIONEXPIRE = "R";
            public static string UNKNOWNDEVICE = "X";
            public static string UNKNOWNUSER = "U";
            public static string DEVICEACTIVATIONPANTING = "P";
            public static string CANNOTCONNECTTOTHEDATABASE = "C";
            public static string WRONGPASSWORD = "W";

        }

        public static class AttendanceStatus
        {
            public const String Absent = "A";
            public const String Present = "P";
            public const String ForeNoon = "FN";
            public const String AfterNoon = "AN";
        }
    }
}
