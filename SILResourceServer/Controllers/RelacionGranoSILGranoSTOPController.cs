using ResourceServer.Models;
using ResourceServer.Models.AtributosValidacion;
using ResourceServer.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ResourceServer.Controllers
{
    [ExceptionHandling]
    public class RelacionGranoSILGranoSTOPController : ApiController
    {
        [HttpPost]
        [Route("api/RelacionGranoSILGranoSTOP/GetRelaciones")]
        public HttpResponseMessage GetRelacionGranoSILGranoSTOP(RelacionGranoSILGranoSTOPDTO relacion)
        {
            if (ModelState.IsValid) 
            {
                ServicioRelacionGranoSILGranoSTOP servicio = new ServicioRelacionGranoSILGranoSTOP();
                return Request.CreateResponse(HttpStatusCode.OK, servicio.GetRelaciones(relacion));
            
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Solicitud Incompleta");
        }

        [HttpPost]
        [Route("api/RelacionGranoSILGranoSTOP/AddRelacion")]
        public HttpResponseMessage AddRelacion(RelacionGranoSILGranoSTOPDTO relacion)
        {
            if (ModelState.IsValid)
            {
                ServicioRelacionGranoSILGranoSTOP servicio = new ServicioRelacionGranoSILGranoSTOP();
                return Request.CreateResponse(HttpStatusCode.OK, servicio.AgregarRelacion(relacion));

            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Solicitud Incompleta");
        }

        [HttpPost]
        [Route("api/RelacionGranoSILGranoSTOP/DeleteRelacion")]
        public HttpResponseMessage DeleteRelacion(RelacionGranoSILGranoSTOPDTO relacion)
        {
            if (ModelState.IsValid)
            {
                ServicioRelacionGranoSILGranoSTOP servicio = new ServicioRelacionGranoSILGranoSTOP();
                return Request.CreateResponse(HttpStatusCode.OK, servicio.EliminarRelacion(relacion));

            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Solicitud Incompleta");
        }

        [HttpPost]
        [Route("api/RelacionGranoSILGranoSTOP/UpdateRelacion")]
        public HttpResponseMessage UpdateRelacion(RelacionGranoSILGranoSTOPDTO relacion)
        {
            if (ModelState.IsValid)
            {
                ServicioRelacionGranoSILGranoSTOP servicio = new ServicioRelacionGranoSILGranoSTOP();
                return Request.CreateResponse(HttpStatusCode.OK, servicio.ModificarRelacion(relacion));

            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Solicitud Incompleta");
        }

        
    }
}
