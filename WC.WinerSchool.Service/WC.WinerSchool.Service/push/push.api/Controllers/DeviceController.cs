using WC.WinerSchool.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace push.api.Controllers
{
    [RoutePrefix("device")]
    public class DeviceController : ApiController
    {
        WinErBLClass objBL = new WinErBLClass();

        [HttpGet]
        [Route("Register/{value}")]
        public string Register(string value)
        {
            string[] valueArray = value.Split('_');
            if (valueArray.Length > 2)
            {
                string json = "{\"PhoneNumber\":\"" + valueArray[0] + "\",\"SchoolVerificationId\":\"" + valueArray[1] + "\",\"DeviceTokenId\":\"" + valueArray[2] + "\"}";
                return objBL.WAPI_RegisterDevice(json);
            }
            
            return "F";
        }


        [HttpGet]
        [Route("AuthorizeDevice/{value}")]
        public string AuthorizeDevice(string value)
        {
            string[] valueArray = value.Split('_');
            if (valueArray.Length > 3)
            {
                string json = "{\"OTP\":\"" + valueArray[0] + "\",\"SchoolId\":\"" + valueArray[1] + "\",\"DeviceId\":\"" + valueArray[2] + "\",\"PhoneNumber\":\"" + valueArray[3] + "\"}";
                return objBL.WAPI_AuthorizeDevice(json);
            }

            return "F";
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public string Post(string value)
        {
            return "value";
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}