using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace ResourceServer.Controllers
{
    [RoutePrefix("api/protected")]
    public class ProtectedController : ApiController
    {
        // GET: api/Protected
        public IEnumerable<object> Get()
        {
            var identity = User.Identity as ClaimsIdentity;
            var CentrosClaim = identity.Claims.FirstOrDefault<Claim>(c=>c.Type == "Centros");
            var Centros = JsonConvert.DeserializeObject<List<string>>(CentrosClaim.Value);
            return Centros ;
            //return identity.Claims.Select(c => new
            //{
            //    Type = c.Type,
            //    Value = c.Value
            //});
        }

        // GET: api/Protected/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Protected
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Protected/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Protected/5
        public void Delete(int id)
        {
        }
    }
}
