using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
    interface IVistaCuposDistribuidosStore
    {
        void Save(VistaCuposDistribuidos c);
        void Update(int Id, VistaCuposDistribuidos c);
        void Delete(int Id);
        VistaCuposDistribuidos FindByCuentaComprador(int id);
        IList<VistaCuposDistribuidos> FindByFilterOfView(VistaCuposDistribuidos CompVendDestProdCenConsignacion, string cosechaDesde,
            string cosechaHasta, int fechaDesde, int fechaEntrega, LocalType.ContratosPor contratosPor, IList<long> vendedores);
        VistaCuposDistribuidos FindByProdCompVendPuerFechaCoseCentDestino(VistaCuposDistribuidos CompVendProdCentroDestFechaConsignacion,
            string cosechaD, string cosechaH);
        IList<VistaCuposDistribuidos> FindAll();
        [Obsolete("Unificar con el metodo FindByFilterOfView")]
        VistaCuposDistribuidos FindByProdCompVendPuerFechaCoseCentDestino(VistaCuposDistribuidos CompVendProdCentroDestFechaConsignacion,
            string cosechaD, string cosechaH, DateTime? FechaDesde, DateTime? FechaHasta, ISession Session);
    }
}
