using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Filtro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Contratos
{
    public class ServicioContratos
    {
        public IList<VistaCuposDistribuidos> ObtenerContratos(VistaCuposDistribuidos datosContrato, 
            DateTime? fechaDesde, DateTime? fechaHasta, string cosechaDesde, string cosechaHasta)
        {
            if (fechaDesde == null) fechaDesde = FiltroPorDefecto.GetFiltroPorDefectoFechaDesde();
            if (fechaHasta == null || fechaHasta == default(DateTime)) fechaHasta = FiltroPorDefecto.GetFiltroPorDefectoFechaHasta();
            if (cosechaDesde == null || cosechaDesde.Trim() == "0") cosechaDesde = FiltroPorDefecto.GetFiltroPorDefectoCosechaDesde();
            if (cosechaHasta == null || cosechaHasta.Trim() == "0") cosechaHasta = FiltroPorDefecto.GetFiltroPorDefectoCosechaHasta();
            IVistaCuposDistribuidosStore store = new VistaCuposDistribuidosStore();
            if (datosContrato.Vendcta != 0)
            {
                IVendedorStore consultaVendedor = new VendedorStore();
                if (consultaVendedor.IsInternal(datosContrato.Vendcta))
                {
                    return store.FindByFilterOfView(datosContrato, cosechaDesde, cosechaHasta, DateUtils.n_date(fechaDesde), DateUtils.n_date(fechaHasta), LocalType.ContratosPor.Cuenta, null);
                }
                else
                {
                    ICuitStore storeCuentas = new CuitStore();
                    CuposCuit cuentaCuit = storeCuentas.FindByCuenta(datosContrato.Vendcta);
                    datosContrato.Cuitvend = cuentaCuit.Cuit;
                    IList<Vendedor> vendedores = consultaVendedor.FindVendedorByCuit(datosContrato.Cuitvend);
                    IList<long> CtasVendedores = vendedores.Select(c => c.Cuenta).ToList();
                    return store.FindByFilterOfView(datosContrato, cosechaDesde, cosechaHasta, DateUtils.n_date(fechaDesde), DateUtils.n_date(fechaHasta), LocalType.ContratosPor.Cuit, CtasVendedores);
                }
            }
            /*sin cta vendedor*/
            return store.FindByFilterOfView(datosContrato, cosechaDesde, cosechaHasta, DateUtils.n_date(fechaDesde),  DateUtils.n_date(fechaHasta), LocalType.ContratosPor.Cuenta, null);
        }

        public IList<Vista_CuposMpeCorreV3> ObtenerDetallePendienteAplicar(long Compcta, long Vendcta, int Producto, 
            long Ctadestino, string Cosecha, string Codcentro)
        {
            Vista_Cuposmpecorrev3Store Store = new Vista_Cuposmpecorrev3Store();
            return Store.FindByCompctaAndVendctaAndProductoAndCtadestinoAndCosechaAndCodcentro(Compcta, Vendcta, Producto, Ctadestino, Cosecha, Codcentro);
        }

        [Obsolete("Lo sacamos de la vista")]
        public IList<VistaCuposmpecorre> ObtenerMercaderiaSinDestino(int Producto, long Compcta, string Centro, long Vendcta)
        {
            VistaCuposmpecorreStore Store = new VistaCuposmpecorreStore();
            return Store.FindByProductoAndCompctaAndCentroAndVendcta(Producto, Compcta, Centro, Vendcta);
        }
    }
}