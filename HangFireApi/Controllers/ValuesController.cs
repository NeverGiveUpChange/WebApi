using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HangFireApi.Controllers
{
    public class ValuesController : BaseApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string GetFromNotObj(int id)
        {
            return "value";
        }

        // POST api/values
        public void PostFromObj([FromBody]Tenmp temp)
        {
            var aa = "";
        }

        // PUT api/values/5
        public void GetFromObjTest([FromBody]Tenmp temp)
        {
            var bb = "";
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
    public class Tenmp
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
