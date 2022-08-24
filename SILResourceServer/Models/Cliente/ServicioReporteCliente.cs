using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ResourceServer.Models.Cliente
{
    public class ServicioReporteCliente
    {
        public IList<ReporteViewModel> GetReporteCliente(FiltroReporteViewModel filtro)
        {
            ReporteClienteStore store = new ReporteClienteStore();
            return store.GetReporteCliente(filtro);
        }
    }
}