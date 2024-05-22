using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
    interface IVistaCtoCorreStore
    {
        VistaCtoCorre FindById(string id);
        IList<VistaCtoCorre> FindAll();
        IList<VistaCtoCorre> FindByCompctaAndVendctaAndCtadestinoAndCodcentroAndGranoAndFechaent
            (Int64 compcta, Int64 vendcta, Int64 ctadestino, string codcentro, int grano, Int64 fechaent);
    IList<VistaCtoCorre> FindContratos
        (long comprador, long vendedor, IList<long> cuentasDestino, int grano, long fechaDesde, long fechaHasta);
  }
}
