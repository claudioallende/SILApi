using ResourceServer.Models;
using ResourceServer.Models.AtributosValidacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ResourceServer.Controllers
{
    [Authorize]
    [ExceptionHandling]
    public class GranosSTOPController : ApiController
    {
        [HttpPost]
        [Route("api/GranosSTOP/GetGrano")]
        public HttpResponseMessage GetGranoSTOP(CuposGranoSTOP grano)
        {
            ServicioGranoSTOP servicio = new ServicioGranoSTOP();
            if (grano != null) 
            {
                if (ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, servicio.GetGranos(grano));
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Solicitud Incompleta");
            }
            return Request.CreateResponse(HttpStatusCode.OK, servicio.GetGranos());
        }

        [HttpPost]
        [Route("api/GranosSTOP/AddGrano")]
        public HttpResponseMessage AddGrano(CuposGranoSTOP grano)
        {
            if (ModelState.IsValid)
            {
                ServicioGranoSTOP servicio = new ServicioGranoSTOP();
                return Request.CreateResponse(HttpStatusCode.OK, servicio.AgregarGrano(grano));

            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Solicitud Incompleta");
        }

        [HttpPost]
        [Route("api/GranosSTOP/DeleteGrano")]
        public HttpResponseMessage DeleteGrano(CuposGranoSTOP grano)
        {
            if (ModelState.IsValid)
            {
                ServicioGranoSTOP servicio = new ServicioGranoSTOP();
                return Request.CreateResponse(HttpStatusCode.OK, servicio.EliminarGrano(grano));

            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Solicitud Incompleta");
        }

        [HttpPost]
        [Route("api/GranosSTOP/UpdateGrano")]
        public HttpResponseMessage UpdateGrano(CuposGranoSTOP grano)
        {
            if (ModelState.IsValid)
            {
                ServicioGranoSTOP servicio = new ServicioGranoSTOP();
                return Request.CreateResponse(HttpStatusCode.OK, servicio.ModificarGrano(grano));

            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Solicitud Incompleta");
        }
    }
}
