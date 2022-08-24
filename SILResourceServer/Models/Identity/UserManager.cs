using ResourceServer.Models.Authentication;
using Microsoft.AspNet.Identity;
using RestSharp;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ResourceServer.Models.Identity
{
    public class UserManager : UserManager<User, string>
    {
        public UserManager(IUserStore<User, string> store)
            : base(store)
        {
            UserValidator = new UserValidator<User, string>(this);
            PasswordValidator = new PasswordValidator() { RequiredLength = 5 };
            Centros = new List<Claim>();
        }

        private IList<System.Security.Claims.Claim> Centros { get; set; }

        public Task<Token> GetToken(string usuario, string password)
        {
            var client = new RestClient(ConfigurationManager.AppSettings["servicio_autenticacion"]);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\r\n\"Username\":\"" + usuario + "\",\r\n\"Password\":\"" + password + "\"\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) //Si responde algun error el servidor de autenticacion va a decir que usuario o contraseña no son correctas
            {
                return Task.FromResult<Token>(new Token { AccessToken = response.Content });
            }
            else
            {
                //throw new System.Exception("Tocken no recivido");
                WriteLog.Write("Log.txt", "Servidor de autenticacion no reconoce usuario o contraseña");
                return Task.FromResult<Token>(new Token { AccessToken = "" });
            }
        }

        public Task<Token> GetTokenSharepoint(string usuario)
        {
            var client = new RestClient(ConfigurationManager.AppSettings["servicio_autenticacion"]);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\r\n\"Username\":\"" + usuario + "\",\r\n\"Sitio\":\"CuposCorretaje\"\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) //Si responde algun error el servidor de autenticacion va a decir que usuario o contraseña no son correctas
            {
                return Task.FromResult<Token>(new Token { AccessToken = response.Content });
            }
            else
            {
                //throw new System.Exception("Tocken no recivido");
                WriteLog.Write("Log.txt", "Servidor de autenticacion no reconoce usuario o contraseña");
                return Task.FromResult<Token>(new Token { AccessToken = "" });
            }
        }

        public Task<Permiso> Authenticate(string token, string usuario)
        {
            Permiso permiso = null;
            if (!string.IsNullOrEmpty(token))
            {
                var client = new RestClient(ConfigurationManager.AppSettings["servidor_validacion"] + "/api/usuarios/" + usuario.ToLower() + "/CentrosCuposCorretaje");
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", "Bearer " + token.Replace("\"", ""));
                IRestResponse response = client.Execute(request);
                permiso = new JavaScriptSerializer().Deserialize<Permiso>(Regex.Unescape(response.Content).Replace("\"{", "{").Replace("}\"", "}"));
                if (response.StatusCode == System.Net.HttpStatusCode.OK && permiso.PermisoDeAcceso == true && permiso.Centros.Length > 0)
                {
                    Centros.Add(new Claim("CentroPorDefecto", permiso.Centros[0]));
                    foreach (string Centro in permiso.Centros)
                    {
                        Centros.Add(new Claim("Centros", Centro));
                    }
                }
                else {
                    permiso = DenegarAcceso();
                }
            }
            else
            {
                permiso = DenegarAcceso();
            }
            return Task.FromResult<Permiso>(permiso);
        }

        private Permiso DenegarAcceso()
        {
            //throw new System.Exception("Tocken no valido");
            WriteLog.Write("Log.txt", "No se recibio tocken");
            return new Permiso { PermisoDeAcceso = false, Centros = new string[] { } };
        }

        public override Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(string userId)
        {
            return Task.FromResult<IList<System.Security.Claims.Claim>>(Centros);
        }

        public override Task<bool> CheckPasswordAsync(User user, string password)
        {
            var token = GetToken(user.UserName, password);
            var Permisos = Authenticate(token.Result.AccessToken, user.UserName);
            bool Authentication = Permisos.Result.PermisoDeAcceso;
            return Task.FromResult<bool>(Authentication);
        }

        public Task<bool> CheckSharepointLoginAsync(User user)
        {
            var token = GetTokenSharepoint(user.UserName);
            var Permisos = Authenticate(token.Result.AccessToken, user.UserName);
            bool Authentication = Permisos.Result.PermisoDeAcceso;
            return Task.FromResult<bool>(Authentication);
        }
    }
}