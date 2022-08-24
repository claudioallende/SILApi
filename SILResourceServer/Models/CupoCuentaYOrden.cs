using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class CupoCuentaYOrden
    {
        public Cupos CupoPadre { get; set; }
        public Cupos CupoHijo { get; set; }
    }
}