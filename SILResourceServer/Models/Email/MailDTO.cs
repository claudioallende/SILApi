using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Email
{
    public class MailDTO
    {
    }

    public class MailSimplePdfDTO
    {
        public int CantidadCupos { get; set; }
        public DateTime Fecha { get; set; }
    }
}