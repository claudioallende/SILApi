using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace ResourceServer.Models
{
    public class ClaimsUtil
    {
        public static string GetClaim(string key)
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            string claim = claimsIdentity.Claims.Where(x => x.Type == key).Select(x => x.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(claim)) throw new Exception("Token vacío o nulo");
            return claim;
        }

        public static bool GetBooleanClaim(string key)
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            bool result = false;
            var claim = claimsIdentity.Claims.Where(x => x.Type == key).Select(x => x.Value).FirstOrDefault();
            if (claim != null)
                Boolean.TryParse(claim, out result);
            return result;
        }

        public static IList<string> GetListClaims(string key)
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            IList<string> claims = claimsIdentity.Claims.Where(x => x.Type == key).Select(x => x.Value).ToList();
            return claims;
        }

        public static string GetClaim(IEnumerable<Claim> Claims, string key)
        {
            string claim = Claims.Where(x => x.Type == key).Select(x => x.Value).FirstOrDefault();
            return claim;
        }

        public static IList<string> GetListClaims(IEnumerable<Claim> Claims, string key)
        {
            IList<string> claims = Claims.Where(x => x.Type == key).Select(x => x.Value).ToList();
            return claims;
        }

        public static string GetClaimValue(IPrincipal currentPrincipal, string key)
        {
            var identity = currentPrincipal.Identity as ClaimsIdentity;
            if (identity == null)
                return null;

            var claim = identity.Claims.FirstOrDefault(c => c.Type == key);
            return claim == null ? "" : claim.Value;
        }

        public static bool HasSomeClaim(IList<string> ClaimsName)
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            string claim = claimsIdentity.Claims.Where(x => ClaimsName.Contains(x.Type)).Select(x => x.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(claim)) return false;
            return true;
        }

        public static bool HasClaim(string ClaimName)
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            string claim = claimsIdentity.Claims.Where(x => ClaimName == x.Type).Select(x => x.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(claim)) return false;
            return true;
        }

        public static long GetCuit()
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            string cuit = claimsIdentity.Claims.Where(x => x.Type == "Cuit").Select(x => x.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(cuit) || cuit.Trim() == "0") throw new Exception("Token vacío o nulo");
            return Int64.Parse(cuit);
        }

        public static IList<long> GetCuenta()
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            IList<long> cuenta = claimsIdentity.Claims.Where(x => x.Type == "Cuenta").Select(x => Int64.Parse(x.Value)).ToList();
            if (cuenta == null || cuenta.Count == 0) throw new Exception("Token vacío o nulo");
            return cuenta;
        }

        /// <summary>
        /// Responde con Cuit, si es de ACA responde con Cuentas
        /// </summary>
        /// <returns></returns>
        public static IList<long> GetCuitOrCuenta()
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            IList<long> cuenta = new List<long>();
            IList<long> cuit = claimsIdentity.Claims.Where(x => x.Type == "Cuit").Select(x => Int64.Parse(x.Value)).ToList();
            IList<long> result = cuit;
            if (cuit == null || cuit.Count == 0)
            {
                throw new Exception("Token vacío o nulo");
            }
            else
            {
                if (cuit.Contains(30500120882))
                {
                    cuenta = claimsIdentity.Claims.Where(x => x.Type == "Cuenta").Select(x => Int64.Parse(x.Value)).ToList();
                    result = cuenta;
                }
                if (cuit.Contains(30500120882) && (cuenta == null || cuenta.Count == 0)) throw new Exception("Token vacío o nulo");
            }
            return result;
        }

        public static string AddClaim(string key, string value)
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            Claim myClaim = new Claim(key, value);
            claimsIdentity.AddClaim(myClaim);
            string claim = claimsIdentity.Claims.Where(x => x.Type == key).Select(x => x.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(claim) || claim != value) throw new Exception("Claim no agregado correctamente");
            return claim;
        }
    }
}