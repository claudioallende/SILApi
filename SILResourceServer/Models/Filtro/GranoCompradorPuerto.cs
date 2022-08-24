using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Filtro
{
    public class GranoCompradorPuerto
    {
        public int CodigoGrano { get; set; }
        public string NombreGrano { get; set; }
        public long CuentaComprador { get; set; }
        public string NombreComprador { get; set; }
        public long CuentaPuerto { get; set; }
        public string NombrePuerto { get; set; }
    }
}