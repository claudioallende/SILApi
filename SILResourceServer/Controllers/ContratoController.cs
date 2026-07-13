using ResourceServer.Models.AtributosValidacion;
using ResourceServer.Models.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ResourceServer.Controllers
{
    [ExceptionHandling]
    public class ContratoController : ApiController
    {
        [HttpGet]
        [Route("api/Contrato/GetDetallePendienteAplicar")]
        public HttpResponseMessage GetDetallePendienteAplicar(long Compcta, long Vendcta, int Producto, long Ctadestino,
            string Cosecha, string Codcentro)
        {
            ServicioContratos servicio = new ServicioContratos();
            return Request.CreateResponse(HttpStatusCode.OK, servicio.ObtenerDetallePendienteAplicar(Compcta, Vendcta, Producto, Ctadestino, Cosecha, Codcentro));
        }
    }
}
