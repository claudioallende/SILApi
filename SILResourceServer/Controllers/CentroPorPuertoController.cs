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
    public class CentroPorPuertoController : ApiController
    {
        [HttpPost]
        [Route("api/CentroPorPuerto/Relaciones")]
        public HttpResponseMessage Get(CentroPorPuertoDTO filtro)
        {
            ServicioCentroPorPuerto ServCentroXPuerto = new ServicioCentroPorPuerto();
            return Request.CreateResponse(HttpStatusCode.OK, ServCentroXPuerto.GetRelacionPuertoCentro(filtro));
        }

        [HttpGet]
        [Route("api/CentroPorPuerto/Centros/{Id?}")]
        public HttpResponseMessage Centros(string Id="")
        {
            ServicioCentroPorPuerto ServCentroXPuerto = new ServicioCentroPorPuerto();
            return Request.CreateResponse(HttpStatusCode.OK, ServCentroXPuerto.GetCentros(Id));
        }

        [HttpGet]
        [Route("api/CentroPorPuerto/Puertos/{Id?}")]
        public HttpResponseMessage Puertos(string Id="")
        {
            ServicioCentroPorPuerto ServCentroXPuerto = new ServicioCentroPorPuerto();
            return Request.CreateResponse(HttpStatusCode.OK, ServCentroXPuerto.GetPuerto(Id));
        }

        [HttpPost]
        [Route("api/CentroPorPuerto/SaveRelacion")]
        public HttpResponseMessage SaveRelacion(CentroPorPuertoDTO relacion)
        {
            if (ModelState.IsValid)
            {
                ServicioCentroPorPuerto ServCentroXPuerto = new ServicioCentroPorPuerto();
                CentroPorPuertoDTO result = ServCentroXPuerto.Save(relacion);
                if (result.Id!= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.Created, result);
                }
                return Request.CreateResponse(HttpStatusCode.Conflict, "Ya existe esta relacion");
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Solicitud Incompleta");
        }

        [HttpPost]
        [Route("api/CentroPorPuerto/UpdateRelacion")]
        public HttpResponseMessage UpdateRelacion(CentroPorPuertoDTO filtro)
        {
            if (ModelState.IsValid)
            {
                ServicioCentroPorPuerto ServCentroXPuerto = new ServicioCentroPorPuerto();
                return Request.CreateResponse(HttpStatusCode.OK, ServCentroXPuerto.Update(filtro));
            }            
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Solicitud Incompleta");
        }

        [HttpPost]
        [Route("api/CentroPorPuerto/DeleteRelacion")]
        public HttpResponseMessage DeleteRelacion(CentroPorPuertoDTO filtro)
        {
            if (filtro!=null) 
            {
                ServicioCentroPorPuerto ServCentroXPuerto = new ServicioCentroPorPuerto();
                return Request.CreateResponse(HttpStatusCode.OK, ServCentroXPuerto.EliminarRelacion(filtro));
            }
            else 
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Solicitud Incompleta");
            }
        }
    }
}
