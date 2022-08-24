using ResourceServer.Models;
using ResourceServer.Models.AtributosValidacion;
using ResourceServer.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ResourceServer.Controllers
{
    [Authorize]
    [ExceptionHandling]
    public class MediadorSTOPandSILController : ApiController
    {
        [HttpGet]
        [Route("api/CupoSTOPtoSIL/ExistenCuposPendientes")]
        public HttpResponseMessage ExistenCuposPendientes()
        {
            ServicioMediadorSTOPandSIL servicioMediador = new ServicioMediadorSTOPandSIL();
            return Request.CreateResponse(HttpStatusCode.OK, servicioMediador.TurnosPendientesDeAgregar().Count > 0);
        }

        [HttpPost]
        [Route("api/CupoSTOPtoSIL/CuposPendientes")]
        public HttpResponseMessage CuposPendientes(NuevoCupoViewModelDTO GranoPuertoCompradorVendedor)
        {
            ServicioMediadorSTOPandSIL servicioMediador = new ServicioMediadorSTOPandSIL();
            if (GranoPuertoCompradorVendedor != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, servicioMediador.CuposPendientesDeAgregarEnSIL(GranoPuertoCompradorVendedor).FirstOrDefault());
            }
            else {
                return Request.CreateResponse(HttpStatusCode.OK, servicioMediador.CuposPendientesDeAgregarEnSIL4View());
            }
        }

        [HttpPost]
        [Route("api/CupoSTOPtoSIL/AgregarCupos")]
        public HttpResponseMessage AgregarCupos(NuevoCupoViewModelDTO cupo)
        {
            ServicioMediadorSTOPandSIL servicioMediador = new ServicioMediadorSTOPandSIL();
            return Request.CreateResponse(HttpStatusCode.OK, servicioMediador.AgregarCupoEnSIL(cupo));
        }

        [HttpPost]
        [Route("api/CupoSTOPtoSIL/CambiarCentro")]
        public HttpResponseMessage CambiarCentro(NuevoCupoViewModelDTO cupo)
        {
            ServicioMediadorSTOPandSIL servicioMediador = new ServicioMediadorSTOPandSIL();
            return Request.CreateResponse(HttpStatusCode.OK, servicioMediador.CambiarCentro(cupo));
        }

        [HttpPost]
        [Route("api/CupoSTOPtoSIL/ConsignacionesPendientes")]
        public HttpResponseMessage ConsignacionesPendientes(NuevoCupoViewModelDTO filtro)
        {
            ServicioMediadorSTOPandSIL servicioMediador = new ServicioMediadorSTOPandSIL();
            return Request.CreateResponse(HttpStatusCode.OK, servicioMediador.ConsignacionesPendientesDeAgregarEnSIL4View(filtro));
        }

        [HttpGet]
        [Route("api/CupoSTOPtoSIL/CuposPendientesInNCVM")]
        public HttpResponseMessage CuposPendientesInNCVM()
        {
            ServicioMediadorSTOPandSIL servicioMediador = new ServicioMediadorSTOPandSIL();
            return Request.CreateResponse(HttpStatusCode.OK, servicioMediador.CuposPendientesDeAgregarEnSIL());
        }
        [HttpGet]
        [Route("api/CupoSTOPtoSIL/AgregarCupo/alfa")]
        public HttpResponseMessage AgregarCupoDeSTOPaSIL(string alfa)
        {
            ServicioMediadorSTOPandSIL servicioMediador = new ServicioMediadorSTOPandSIL();
            return Request.CreateResponse(HttpStatusCode.OK, servicioMediador.AgregarCupo(alfa));
        }
    }
}
