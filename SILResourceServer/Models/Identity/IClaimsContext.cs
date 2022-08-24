using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Identity
{
    public interface IClaimsContext
    {
        IList<string> GetClaimsIdentity(string key);
        string GetClaimIdentity(string key);
    }
}