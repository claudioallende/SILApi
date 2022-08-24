using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Resource;
using IdentityServer3.AccessTokenValidation;
using System.Configuration;

[assembly: OwinStartup(typeof(ResourceServer.Startup))]
namespace ResourceServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // token validation
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = ConfigurationManager.AppSettings["AUTH_SERVER"] + "/identity",         //"https://localhost:44319/identity",
                RequiredScopes = new[] { "CuposCorrRSRCServ" }
                //clientId="322717dee8f24a8b965f78ec0d99c66a"
                //secrets="mDRtSMykxP4qEIrXGFsH9kHitdiTlCxYzbI0FseTSB8"
            });

            // add app local claims per request
            app.UseClaimsTransformation(incoming =>
            {
                // either add claims to incoming, or create new principal
                var appPrincipal = new ClaimsPrincipal(incoming);
                //incoming.Identities.First().AddClaim(new Claim("appSpecific", "some_value_Claudio_Javier"));

                return Task.FromResult(appPrincipal);
            });

            // web api configuration
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            app.UseWebApi(config);
           
        }       
    }
}