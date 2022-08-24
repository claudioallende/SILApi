using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Cupo
{
    public class InformacionModificacionEstado
    {
        public Cupos CupoPadreCyOModificado { get; set; }
        public Cupos CupoHijoCyOModificado { get; set; }
    }
}