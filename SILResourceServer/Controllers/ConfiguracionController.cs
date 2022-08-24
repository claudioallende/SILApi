using ResourceServer.Models;
using ResourceServer.Models.AtributosValidacion;
using ResourceServer.Models.Configuracion;
using ResourceServer.Models.DTO;
using ResourceServer.Models.Email;
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
    public class ConfiguracionController : ApiController
    {
        [HttpGet]
        [Route("api/Configuracion/Index")]
        public HttpResponseMessage Index()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new PanelViewModel());
        }

        [HttpPost]
        [ClaimsAuthorize("AccesoConfiguracionCuposCorretaje", "True")]
        [Route("api/Configuracion/Guardar")]
        public HttpResponseMessage Index(PanelViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppSettingConfig settings = new AppSettingConfig();
                settings.Save(model);
            }
            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        [HttpPost]
        [ClaimsAuthorize("AccesoConfiguracionCYOeEmailDeCuentasCuposCorretaje", "True")]
        [Route("api/Configuracion/GuardarEmail")]
        public HttpResponseMessage Email(GuardarEmailViewModel data)
        {
            ServicioConfiguracionEmail servicio = new ServicioConfiguracionEmail();
            servicio.SaveEmails(data.NumeroCuenta.ToString(), data.NombreCuenta, data.EMail);
            return Request.CreateResponse(HttpStatusCode.OK, new EmailViewModel());
        }

        [Route("api/Configuracion/GetEmailByCuit/{cuit}")]
        public HttpResponseMessage GetEmailByCuit(string cuit)
        {
            ServicioConfiguracionEmail servicio = new ServicioConfiguracionEmail();
            return Request.CreateResponse(HttpStatusCode.OK, servicio.GetEmailsByCuit(cuit));
        }

        [Route("api/Configuracion/GetEmailByCuenta/{Cuenta}")]
        public HttpResponseMessage GetEmailByCuenta(string Cuenta)
        {
            ServicioConfiguracionEmail servicio = new ServicioConfiguracionEmail();
            return Request.CreateResponse(HttpStatusCode.OK, servicio.GetEmailsByClave(Cuenta));
        }

        [HttpPost]
        [ClaimsAuthorize("AccesoConfiguracionCYOeEmailDeCuentasCuposCorretaje", "True")]
        [Route("api/Configuracion/GuardarCuentaCYO")]
        public HttpResponseMessage CuentaCYO(CuentaCYOViewModel model)
        {
            if (ModelState.IsValid)
            {
                string Error = "OK";
                ServicioConfiguracionCuentaCYO servicio = new ServicioConfiguracionCuentaCYO();
                if (model.EsCuentaYOrden)
                {
                    Error = servicio.SaveOrUpdateCuentaCYO(model);
                }
                else
                {
                    servicio.DeleteCuentaCYO(model);
                }
                //if (!string.IsNullOrEmpty(Error) && !(Error == "OK")) ViewBag.Error = servicio.SaveOrUpdateCuentaCYO(model);
                ModelState.Clear();
            }
            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        [Route("api/Configuracion/GetCuentaYOrdenByNumeroCuenta/{NumeroCuenta}")]
        public HttpResponseMessage GetCuentaYOrdenByNumeroCuenta(string NumeroCuenta)
        {
            ServicioConfiguracionCuentaCYO servicio = new ServicioConfiguracionCuentaCYO();
            return Request.CreateResponse(HttpStatusCode.OK, servicio.EsCuentaYOrden(NumeroCuenta));
        }

        [Route("api/Configuracion/GetCuentaYOrdenByNumeroCuentaAndCuit/{Cuit}/{NumeroCuenta}")]
        public HttpResponseMessage GetCuentaYOrdenByNumeroCuentaAndCuit(string NumeroCuenta, string Cuit)
        {
            ServicioConfiguracionCuentaCYO servicio = new ServicioConfiguracionCuentaCYO();
            return Request.CreateResponse(HttpStatusCode.OK, servicio.EsCuentaYOrden(NumeroCuenta, Cuit));
        }

                        /*Relacion GranoSIL GranoSTOP Controller*/

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

                            /*Centro Por Puerto Controller*/

        [HttpPost]
        [Route("api/CentroPorPuerto/Relaciones")]
        public HttpResponseMessage Get(CentroPorPuertoDTO filtro)
        {
            ServicioCentroPorPuerto ServCentroXPuerto = new ServicioCentroPorPuerto();
            return Request.CreateResponse(HttpStatusCode.OK, ServCentroXPuerto.GetRelacionPuertoCentro(filtro));
        }

        [HttpGet]
        [Route("api/CentroPorPuerto/Centros/{Id?}")]
        public HttpResponseMessage Centros(string Id = "")
        {
            ServicioCentroPorPuerto ServCentroXPuerto = new ServicioCentroPorPuerto();
            return Request.CreateResponse(HttpStatusCode.OK, ServCentroXPuerto.GetCentros(Id));
        }

        [HttpGet]
        [Route("api/CentroPorPuerto/Puertos/{Id?}")]
        public HttpResponseMessage Puertos(string Id = "")
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
                if (result.Id != 0)
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
            if (filtro != null)
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
