using ResourceServer.Models;
using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResourceServer.Services
{
    public class ServiceCupo
    {
        public CuposStore StoreCupo;

        public ServiceCupo()
        {
            StoreCupo = new CuposStore();
        }

        //public JsonResult GetCuposPorCodigoAlfanumerico(IList<string> CodigosAlfanumericos)
        //{
        //    ServicioCupo Servicio = new ServicioCupo();
        //    IList<EstadoAlfanumericoModel> Cupos = Servicio.ObtenerCuposPorCodigosAlfanumericos(CodigosAlfanumericos);
        //    return Json(new
        //    {
        //        Cupos = Cupos.Select(x => new
        //        {
        //            Alfanumerico = x.Alfanumerico,
        //            Estado = x.GetEstado().Codigo,
        //            NombreEstado = x.GetEstado().Nombre,
        //            Fecha = x.Fecha.ToString("dd/MM/yyyy"),
        //            Comprador = x.Comprador,
        //            Vendedor = string.IsNullOrEmpty(x.Vendedor) ? string.Empty : x.Vendedor,
        //            Puerto = x.Puerto,
        //            Grano = x.Grano,
        //            Centro = x.CodCentroOrigen,
        //            CentroDistribucion = x.CodCentroDistribucion
        //        })
        //    });
        //}
    }
}