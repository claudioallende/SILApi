using ResourceServer.Models.AtributosValidacion;
using ResourceServer.Models.Auditoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ResourceServer.Controllers
{
    [ClaimsAuthorize("AccesoAuditoriaCuposCorretaje", "True")]
    [ExceptionHandling]
    public class AuditoriaController : ApiController
    {
        [HttpPost]
        [Route("api/Auditoria/GetAuditoriaCuposCorre")]
        public HttpResponseMessage ObtenerAuditoriaCuposCorre(AuditoriaCuposCorreViewModel filtro)
        {
            ServicioAuditoria Servicio = new ServicioAuditoria();
            return Request.CreateResponse(HttpStatusCode.OK, Servicio.ObtenerAuditoriaCupo(filtro.Usuario, filtro.FechaDesde, filtro.FechaHasta, filtro.OperacionSeleccionada));
        }

        [HttpGet]
        [Route("api/Auditoria/GetAuditoriaCupo/{IdCupo}")]
        public HttpResponseMessage GetAuditoriaCupo(long IdCupo)
        {
            ServicioAuditoria Servicio = new ServicioAuditoria();
            IList<AuditoriaCuposCorre> Auditorias = Servicio.ObtenerAuditoriaCupo(IdCupo);
            return Request.CreateResponse(HttpStatusCode.OK, Auditorias);
        }
    }
}