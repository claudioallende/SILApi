using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resource
{
    public static class Constants
    {
        public const string BaseAddress = "https://localhost:44303/core";
        public const string AuthorizeEndpoint = BaseAddress + "/connect/authorize";
        public const string LogoutEndpoint = BaseAddress + "/connect/endsession";
        public const string TokenEndpoint = BaseAddress + "/connect/token";
        public const string UserInfoEndpoint = BaseAddress + "/connect/userinfo";
        public const string IdentityTokenValidationEndpoint = BaseAddress + "/connect/identitytokenvalidation";
        public const string TokenRevocationEndpoint = BaseAddress + "/connect/revocation";
        public const string AspNetWebApiSampleApi = "http://localhost:2727/";
        public const string AspNetWebApiSampleApiUsingPoP = "http://localhost:46613/";

        public enum Informes
        {
            DIARIO_MAÑANA,
            DIARIO_TARDE,
            SEMANAL_MAÑANA,
            SEMANAL_TARDE,
            MENSUAL_MAÑANA,
            MENSUAL_TARDE,
            MENSUAL_CALENDARIO,
            PERIODO_PERSONALIZADO,
            NINGUNO
        };

        public static Informes toInformes(string informe) 
        {
            if (informe == Informes.DIARIO_MAÑANA.ToString()) return Informes.DIARIO_MAÑANA;
            if (informe == Informes.DIARIO_TARDE.ToString()) return Informes.DIARIO_TARDE;
            if (informe == Informes.SEMANAL_MAÑANA.ToString()) return Informes.SEMANAL_MAÑANA;
            if (informe == Informes.SEMANAL_TARDE.ToString()) return Informes.SEMANAL_TARDE;
            if (informe == Informes.MENSUAL_MAÑANA.ToString()) return Informes.MENSUAL_MAÑANA;
            if (informe == Informes.MENSUAL_TARDE.ToString()) return Informes.MENSUAL_TARDE;
            if (informe == Informes.MENSUAL_CALENDARIO.ToString()) return Informes.MENSUAL_CALENDARIO;
            if (informe == Informes.PERIODO_PERSONALIZADO.ToString()) return Informes.PERIODO_PERSONALIZADO;
            return Informes.NINGUNO;
        }
    }
}