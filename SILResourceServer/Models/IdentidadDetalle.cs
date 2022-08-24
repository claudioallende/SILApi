using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class IdentidadDetalle
    {
        public long CuentaComprador { get; set; }
        public long CuentaPuerto { get; set; }
        public int CodigoGrano { get; set; }
        public Consignacion Consignacion { get; set; }
        public bool InformadoStop { get; set; }
        public bool EsCYO { get; set; }
    }
}