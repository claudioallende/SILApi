using ResourceServer.Models;
using ResourceServer.Models.AtributosValidacion;
using ResourceServer.Models.DataAccess;
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
    public class CuentaController : ApiController
    {
        [Route("api/Cuenta/GetCuentaFromNumeroCuentaComprador/{Cuenta}")]
        public HttpResponseMessage GetCuentaFromNumeroCuentaComprador(string Cuenta)
        {
            var servicio = new ServicioCuenta(new CompradorStore());
            long numero;
            if (Int64.TryParse(Cuenta, out numero))
            {
                return Request.CreateResponse(HttpStatusCode.OK, servicio.GetFromNumeroCuenta(Cuenta));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [Route("api/Cuenta/GetCuentaFromNumeroCuentaVendedor/{Cuenta}")]
        public HttpResponseMessage GetCuentaFromNumeroCuentaVendedor(string Cuenta)
        {
            var servicio = new ServicioCuenta(new VendedorStore());
            long numero;
            if (Int64.TryParse(Cuenta, out numero))
            {
                return Request.CreateResponse(HttpStatusCode.OK, servicio.GetFromNumeroCuenta(Cuenta));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [Route("api/Cuenta/GetCuentaFromNombreCuentaVendedor/{Nombre}")]
        public HttpResponseMessage GetCuentaFromNombreCuentaVendedor(string Nombre)
        {
            var servicio = new ServicioCuenta(new VendedorStore());
            return Request.CreateResponse(HttpStatusCode.OK, servicio.GetFromNombreCuenta(Nombre));
        }

        [Route("api/Cuenta/GetCuentaFromCuitVendedor/{Cuit}")]
        public HttpResponseMessage GetCuentaFromCuitVendedor(string Cuit)
        {
            var servicio = new ServicioCuenta(new VendedorStore());
            return Request.CreateResponse(HttpStatusCode.OK, servicio.GetFromCuit(Cuit));
        }

        [Route("api/Cuenta/GetAllCuentaFromCuitVendedor/{Cuit}")]
        public HttpResponseMessage GetAllCuentaFromCuitVendedor(string Cuit)
        {
            var servicio = new ServicioCuenta(new VendedorStore());
            return Request.CreateResponse(HttpStatusCode.OK, servicio.GetAllFromCuit(Cuit));
        }

        [Route("api/Cuenta/GetCuentaFromCuit/{Cuit}")]
        public HttpResponseMessage GetCuentaFromCuit(string Cuit)
        {
            var servicio = new ServicioCuenta(new CuitStore());
            return Request.CreateResponse(HttpStatusCode.OK, servicio.GetFromCuit(Cuit));
        }

        [Route("api/Cuenta/GetCuentaFromNombre/{Nombre}")]
        public HttpResponseMessage GetCuentaFromNombre(string Nombre)
        {
            var servicio = new ServicioCuenta(new CuitStore());
            return Request.CreateResponse(HttpStatusCode.OK, servicio.GetFromNombreCuenta(Nombre));
        }

        [Route("api/Cuenta/GetCuentaFromNombreCompradorOrNombreCuit/{Nombre}")]
        public HttpResponseMessage GetCuentaFromNombreCompradorOrNombreCuit(string Nombre)
        {
            return Request.CreateResponse(HttpStatusCode.OK, "");
        }

        [Route("api/Cuenta/GetCuitFromNombre/{Nombre}")]
        public HttpResponseMessage GetCuitFromNombre(string Nombre)
        {
            var servicio = new ServicioCuenta(new CuitStore());
            return Request.CreateResponse(HttpStatusCode.OK, servicio.GetFromNombreCuenta(Nombre).Select(x => new { Cuit = x.Cuit, Nombre = x.Nombre }));
        }

        [HttpPost]
        [Route("api/Cuenta/GetCuposCuitFromNroCuentaOrNombre")]
        public HttpResponseMessage GetCuposCuitFromNroCuentaOrNombre([FromBody]string Texto)
        {
            var servicio = new ServicioCuenta(new CuitStore());
            return Request.CreateResponse(HttpStatusCode.OK, servicio.Get(Texto));
        }

        [Route("api/Cuenta/GetVendedorFromNroCuentaOrNombre/{Texto}")]
        public HttpResponseMessage GetVendedorFromNroCuentaOrNombre(string Texto)
        {
            var servicio = new ServicioCuenta(new VendedorStore());
            return Request.CreateResponse(HttpStatusCode.OK, servicio.Get(Texto));
        }

        [Route("api/Cuenta/GetCompradorFromNroCuentaOrNombre/{Texto}")]
        public HttpResponseMessage GetCompradorFromNroCuentaOrNombre(string Texto)
        {
            var servicio = new ServicioCuenta(new CompradorStore());
            return Request.CreateResponse(HttpStatusCode.OK, servicio.Get(Texto));
        }

        [Route("api/Cuenta/GetPuertoFromNroCuentaOrNombre/{Texto}")]
        public HttpResponseMessage GetPuertoFromNroCuentaOrNombre(string Texto)
        {
         var servicio = new ServicioCuenta(new PuertoStore());
            return Request.CreateResponse(HttpStatusCode.OK, servicio.Get(Texto));
        }

        [HttpPost]
        [Route("api/Cuenta/GetPuertoFromNroCuentaOrNombre")]
        public HttpResponseMessage GetFromPostPuertoFromNroCuentaOrNombre([FromBody]string Texto)
        {
            var servicio = new ServicioCuenta(new PuertoStore());
            return Request.CreateResponse(HttpStatusCode.OK, servicio.Get(Texto));
        }

        [HttpPost]
        [Route("api/Cuenta/GetPuertoStopFromNroCuentaOrNombre")]
        public HttpResponseMessage GetPuertoStopFromNroCuentaOrNombre([FromBody] string Texto)
        {
            var servicio = new ServicioCuenta(new CuposPuertoSTOPStore());
            return Request.CreateResponse(HttpStatusCode.OK, servicio.Get(Texto));
        }

        [Route("api/Cuenta/GetPuertos")]
        public HttpResponseMessage GetPuertos()
        {
            var servicio = new ServicioCuenta(new PuertoStore());
            return Request.CreateResponse(HttpStatusCode.OK, servicio.GetAll());
        }
    }
}
