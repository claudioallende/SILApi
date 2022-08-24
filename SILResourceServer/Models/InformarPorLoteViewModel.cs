using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class InformarPorLoteViewModel
    {
        public long CuentaComprador { get; set; }
        public long CuentaVendedor { get; set; }
        public long CuentaPuerto { get; set; }
        public int CodigoGrano { get; set; }
        public string CodigoCentro { get; set; }
        public string CodigoCentroDistribucion { get; set; }
        public bool Cyo { get; set; }
    }
}