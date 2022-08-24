using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourceServer.Models.Filtro;

namespace ResourceServer.Models.DataAccess
{
    interface IFiltroStore
    {
        FiltroDistribucion FindByKey(long CodigoGrano, long CuentaPuerto, long CuentaComprador, long Uvdist);
        void SaveOrUpdate(FiltroDistribucion filtro);
    }
}
