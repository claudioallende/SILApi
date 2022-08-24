using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Authentication
{
    public class Permiso
    {
        public bool PermisoDeAcceso { get; set; }
        public string[] Centros { get; set; }
        public string Auditoria { get; set; }
    }
}