using WC.WinerSchool.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace WinerSchoolWebApi.Controllers
{
    [RoutePrefix("api/Device")]
    public class DeviceController : ApiController
    {
        WinErBLClass objBL = new WinErBLClass();

        [Route("CreateProcessMaster")]
        public IEnumerable<string> Get()
        {
            string json = "{\"PhoneNumber\":\"9036650445\",\"SchoolVerificationId\":\"4\",\"DeviceTokenId\":\"DeviceTokenId\"}";
            string test = objBL.WAPI_RegisterDevice(json);
            return new string[] { "value1", "value2", test };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
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