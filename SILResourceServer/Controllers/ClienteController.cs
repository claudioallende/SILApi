using ResourceServer.Models;
using ResourceServer.Models.AtributosValidacion;
using ResourceServer.Models.Cliente;
using ResourceServer.Models.Filtro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace ResourceServer.Controllers
{
    [ExceptionHandling]
    public class ClienteController : ApiController
    {
        private ServicioCliente servCliente = new ServicioCliente();

        // GET: api/Cliente
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Cliente/5
        public object Get(long id)
        {
            if (id != 0) 
            {
                return Json(servCliente.ObtenerByVendedor(id));
            }
            return null;
        }

        [HttpGet]
        [Route("api/Cliente/GetCupos/{informa2Stop}")]
        public object GetCuposByStop(int informa2Stop)
        {
            if (informa2Stop == 0)
            {       //no informado
                return servCliente.ObtenerNoInformadosEnStop(ClaimsUtil.GetCuitOrCuenta());
            }
            else    //informado
            {
                return servCliente.ObtenerInformadosEnStop(ClaimsUtil.GetCuitOrCuenta());
            }
        }

        [HttpGet]
        [Route("api/Cliente/GetCuposAgrupados")]
        public object GetCuposByStop()
        {
            return servCliente.ObtenerConjuntoCuposAgrupadosPorVendedor();
        }

        [HttpPost]
        [Route("api/Cliente/GetDetalleCuposStop")]
        public IHttpActionResult GetDetalleCuposStop(IdentidadDetalle data)
        {
            return Json(servCliente.ObtenerDetalleCuposCliente(data.CuentaComprador, data.CuentaPuerto, data.CodigoGrano, data.Consignacion, data.InformadoStop, data.EsCYO));
        }

        [HttpPost]
        [Route("api/Cliente/BuscarCuposAgrupados")]
        public IHttpActionResult BuscarCuposAgrupados(GranoCompradorPuerto filtro)
        {
            return Json(servCliente.ObtenerConjuntoCuposAgrupadosPorVendedor(filtro));
        }

        [Route("api/Cliente/GetGranos")]
        public IHttpActionResult GetGranos()
        {
            return Json(servCliente.ObtenerGranos());
        }

        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Cliente/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Cliente/5
        public void Delete(int id)
        {
        }

        [HttpPost]
        [Route("api/Cliente/GetReporteCliente")]
        public IList<ReporteViewModel> GetReporteCliente(FiltroReporteViewModel filtro)
        {
            ServicioReporteCliente servicio = new ServicioReporteCliente();
            return servicio.GetReporteCliente(filtro);
        }
    }
}
