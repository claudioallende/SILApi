using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class Proceso
    {
        public virtual long Id { get; set; }
        public virtual string Nombre { get; set; }
        public virtual string Detalle { get; set; }
        public virtual string ComandoExec { get; set; }
        public virtual string EmailError { get; set; }
        public virtual IList<EstadoProceso> Estados { get; set; }
    }
}