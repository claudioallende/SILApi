using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class DetalleCupoComplete
    {
        public Comprador Comprador { get; set; }
        public Vendedor Vendedor { get; set; }
        public Puerto Puerto { get; set; }
        public Grano Grano { get; set; }
        public Consignacion Consignacion { get; set; }
        public IList<AlfanumericosDia> AlfanumericosPorDia { get; set; }
        public string CodigoEstablecimientoProcedencia { get; set; }
    }
}