using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class AgrupamientoCupos
    {
    }

    public class ConsignacionYFechaDTO
    {
        public Consignacion Consignacion { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class CuposPorConsignacionYFechaDTO
    {
        public Consignacion Consignacion { get; set; }
        public DateTime Fecha { get; set; }
        public IList<Cupos> Cupos { get; set; }
    }
}