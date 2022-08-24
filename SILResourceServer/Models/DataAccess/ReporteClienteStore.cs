using NHibernate;
using NHibernate.SqlCommand;
using ResourceServer.Models.Cliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class ReporteClienteStore
    {
        public IList<ReporteViewModel> GetReporteCliente(FiltroReporteViewModel filtro)
        {
            using (ISession Session = HibernateUtil.OpenSession())
            {
                var reporte = Session.Query<ReporteViewModel>().Where(x => ClaimsUtil.GetCuitOrCuenta().Contains(x.CuentaVendedor));
                if (!string.IsNullOrEmpty(filtro.Grano)) reporte = reporte.Where(x => x.CodigoGrano == filtro.Grano);
                if (filtro.FechaDesde != null) reporte = reporte.Where(x => x.Fecha >= filtro.FechaDesde);
                if (filtro.FechaHasta != null) reporte = reporte.Where(x => x.Fecha <= filtro.FechaHasta);
                return reporte.ToList();
            }
        }
    }
}