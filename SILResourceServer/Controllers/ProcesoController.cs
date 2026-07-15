using ResourceServer.Models;
using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ResourceServer.Controllers
{
    [Authorize]
    public class ProcesoController : ApiController
    {
        [HttpGet]
        [Route("api/Proceso/GetEstadoProcesos/{id}")]
        public HttpResponseMessage GetEstadoProcesos(long id = 0)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new List<EstadoProceso>());
        }

        [HttpGet]
        [Route("api/Proceso/GetEstadosProcesosFecha/{fecha}/{id}")]
        public HttpResponseMessage GetEstadosProcesosFecha(string fecha, long id = 0)
        {
            ProcesoStore store = new ProcesoStore();
            if (id > 0)
                return Request.CreateResponse(HttpStatusCode.OK, store.FindByIdProcesoAndFecha(id, Convert.ToDateTime(fecha)));
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
