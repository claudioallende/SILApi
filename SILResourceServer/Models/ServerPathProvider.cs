using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class ServerPathProvider : IPathProvider
    {
        public string MapPath(string path)
        {
            return HttpContext.Current.Server.MapPath(path);
        }
    }
}