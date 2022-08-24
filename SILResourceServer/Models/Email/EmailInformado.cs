using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Email
{
    public class EmailInformado
    {
        public long CuentaVendedor { get; set; }
        public int Estado { get; set; }
        public string Mensaje { get; set; }
        /// <summary>
        /// Si es anulacion, asignacion de distribucion o informacion de pend de CTG 
        /// </summary>
        public string TipoEmail { get; set; }

        public EmailInformado()
        {

        }

        public EmailInformado(int Estado)
        {
            if (Estado == 0) Mensaje = "OK";
            if (Estado == 1) Mensaje = "CORREO NO CONFIGURADO";
            //if (Estado == 2)
        }
    }
}