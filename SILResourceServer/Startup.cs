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
using System.Text;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Resource;
using System.Configuration;

[assembly: OwinStartup(typeof(ResourceServer.Startup))]
namespace ResourceServer
{
  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      // Auth local temporal (baja de acabase.com.ar / IdentityServer3): antes se validaba el
      // Bearer contra el Authority externo (discovery + JWKS). Ahora se valida localmente un JWT
      // HS256 propio (emitido por CuposCorretajeWeb, ver Models\Auth\SimpleJwtBuilder.cs alla),
      // firmado con una clave simetrica compartida (JWT_SIGNING_KEY, debe coincidir en ambos
      // Web.config). ValidAudience reemplaza al chequeo de RequiredScopes de antes: el JWT
      // emitido del lado Web pone aud=JWT_AUDIENCE="CuposCorrRSRCServ", igual que el scope viejo.
      byte[] signingKeyBytes = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["JWT_SIGNING_KEY"]);
      app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
      {
        AuthenticationMode = AuthenticationMode.Active,
        TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidIssuer = ConfigurationManager.AppSettings["JWT_ISSUER"],
          ValidateAudience = true,
          ValidAudience = ConfigurationManager.AppSettings["JWT_AUDIENCE"],
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new InMemorySymmetricSecurityKey(signingKeyBytes),
          ValidateLifetime = true
        }
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