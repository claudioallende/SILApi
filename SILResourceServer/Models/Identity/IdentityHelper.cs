using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace ResourceServer.Models.Identity
{
    [Obsolete("Usar ClaimsUtil")]
    public class IdentityHelper
    {
        public static IList<string> GetRolesUsuarioLogueado()
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            return claimsIdentity.Claims.Where(x => x.ValueType == ClaimTypes.Role).Select(x => x.Value).ToList();
        }

        [Obsolete("Usar ClaimsUtil.GetClaim")]
        public static string GetUsuarioLogueado()
        {
            return ClaimsUtil.GetClaim("Usuario");
        }

        [Obsolete("Usar ClaimsUtil.GetListClaims")]
        public static IList<string> GetCodigoCentroUsuarioLogueado()
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            return claimsIdentity.Claims.Where(x => x.Type == "Centro").Select(x => x.Value).ToList();
        }

        [Obsolete("Usar ClaimsUtil.GetClaim")]
        public static string GetCodigoCentroPorDefectoUsuarioLogueado()
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            return claimsIdentity.Claims.Where(x => x.Type == "CentroPorDefecto").Select(x => x.Value).FirstOrDefault();
        }

        [Obsolete("Usar ClaimsUtil.GetClaim")]
        public static bool GetAuditoriaUsuarioLogueado()
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            bool result = false;
            if (claimsIdentity.Claims.Where(x => x.Type == "AccesoAuditoriaResourceServer").Select(x => x.Value).FirstOrDefault() != null)
                Boolean.TryParse(claimsIdentity.Claims.Where(x => x.Type == "AccesoAuditoriaResourceServer").Select(x => x.Value).FirstOrDefault(), out result);
            return result;
        }

        [Obsolete("Usar ClaimsUtil.GetClaim")]
        public static bool TieneUnSoloCentro()
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            return (claimsIdentity.Claims.Where(x => x.Type == "Centro").Count() == 1);
        }
    }
}