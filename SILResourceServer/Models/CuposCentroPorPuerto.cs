using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class CuposCentroPorPuerto
    {
        public virtual long Id { get; set; }
        public virtual long IdTerminal { get; set; }
        public virtual string CodigoCentro { get; set; }
    }
}