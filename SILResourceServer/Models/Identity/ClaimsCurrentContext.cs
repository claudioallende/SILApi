using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace ResourceServer.Models.Identity
{
    public class ClaimsCurrentContext : IClaimsContext
    {
        public IList<string> GetClaimsIdentity(string key)
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            IList<string> claims = claimsIdentity.Claims.Where(x => x.Type == key).Select(x => x.Value).ToList();
            return claims;
        }

        public string GetClaimIdentity(string key)
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            string claim = claimsIdentity.Claims.Where(x => x.Type == key).Select(x => x.Value).FirstOrDefault();
            return claim;
        }
    }
}