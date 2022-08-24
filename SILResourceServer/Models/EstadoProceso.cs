using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class EstadoProceso
    {
        public virtual long Id { get; set; }
        public virtual long IdProceso { get; set; }
        public virtual DateTime FechaExec { get; set; }
        public virtual int Estado { get; set; }
        public virtual string ErrorDetalle { get; set; }
        public virtual string ErrorDetalExec { get; set; }
    }
}