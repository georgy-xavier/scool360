using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace push.api.Controllers
{
    [RoutePrefix("value")]
    public class ValuesController : ApiController
    {
        // GET api/values
        [Route("give")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

         //GET api/values/5
        [HttpGet]
        [Route("pass/{value}")]
        public string pass(string value)
        {
            return value;
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}