using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WC.WinerSchool.BL;

namespace Winer.API.Controllers
{
    [RoutePrefix("api/schools")]
    public class SchoolController : ApiController
    {
        WinerPortalBLClass portalBL = new WinerPortalBLClass();
        [Route("")]
        public IQueryable<string> GetSchools()
        {
            var sss= new List<string>();
            sss.Add("dfdf");
            sss.Add("dsd");
            return sss.AsQueryable();
        }

        [Route("totalFee/{value}")]
        public string GetTotalFee(string value)
        {
            // 19861202_20101130
            string[] valueArray = value.Split('_');
            if (valueArray.Length > 1)
            {
                string json = "{\"FromDate\":\"" + valueArray[0] + "\",\"ToDate\":\"" + valueArray[1] + "\",\"page\":\"" + valueArray[2] + "\",\"pageSize\":\"" + valueArray[3] + "\"}";
                return portalBL.GetSchoolTotalFees(json);
            }

            return "F";
        }

        [Route("totalStudents")]
        public string GetTotalStudents()
        {
            return portalBL.GetSchoolStrength();
        }

        [Route("totalStaffs")]
        public string GetTotalStaffs()
        {
            return portalBL.GetSchoolStaffsStrength();
        }

        
        public class TotalStaffs
        {
            public int Staffs { get; set; }

            public int TotalMale { get; set; }

            public int TotalFemale { get; set; }
            public TotalStaffs(int _totalStaffs, int _totalMale, int _totalFemale)
            {
                Staffs = _totalStaffs;
                TotalMale = _totalMale;
                TotalFemale = _totalFemale;
            }
        }

        public class TotalStudents
        {
            public int Students { get; set; }

            public int TotalMale { get; set; }

            public int TotalFemale { get; set; }
            public TotalStudents(int _totalStudents, int _totalMale, int _totalFemale)
            {
                Students = _totalStudents;
                TotalMale = _totalMale;
                TotalFemale = _totalFemale;
            }
        }
    }
}
