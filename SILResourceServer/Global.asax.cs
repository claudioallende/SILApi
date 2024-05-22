using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ResourceServer
{
  public class WebApiApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      GlobalConfiguration.Configure(WebApiConfig.Register);

      ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11;
    }

    protected void Application_Error()
    {
      var ex = Server.GetLastError();
      //log the error!
      Models.Error.ErrorLog.Write("Excepción no controlada", ex);
    }
  }
}
