using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Cliente
{
    public class ReporteViewModel
    {
        public virtual long Id { get; set; }
        public virtual DateTime Fecha { get; set; }
        public virtual string Grano { get; set; }
        public virtual string Alfanumerico { get; set; }
        public virtual string DetalleCupoSTOP { get; set; }
        public virtual string Observacion { get; set; }
        public virtual string Exportador { get; set; }
        public virtual string Intermediario { get; set; }
        public virtual string RteComercial { get; set; }
        public virtual string CorredorComp { get; set; }
        public virtual string CorredorVend { get; set; }
        public virtual string Solicitante { get; set; }
        public virtual string Mat { get; set; }
        public virtual string Rteent { get; set; }
        public virtual long CuentaVendedor { get; set; }
        public virtual string CodigoGrano { get; set; }
        public virtual string Destino { get; set; }
        public virtual long Vendcyo { get; set; }
    }
    public class FiltroReporteViewModel
    {
        public string Comprador { get; set; }
        public string Grano { get; set; }
        public string Puerto { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
    }
}