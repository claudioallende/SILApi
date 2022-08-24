using ResourceServer.Models;
using ResourceServer.Models.AtributosValidacion;
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
    [ExceptionHandling]
    public class DatosController : ApiController
    {
        [HttpGet]
        [Route("api/Datos/Productos")]
        public HttpResponseMessage GetProductos()
        {
            IGranoStore store = new GranoStore();
            return Request.CreateResponse(HttpStatusCode.OK, store.FindAll().Select(x => new { Nombre = x.Nombre, Codigo = x.CodigoGrano}));
        }

        [HttpGet]
        [Route("api/Datos/Producto/{id}")]
        public HttpResponseMessage GetProducto(string id)
        {
            IGranoStore store = new GranoStore();
            Grano grano = store.FindById(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { Nombre = grano.Nombre, Codigo = grano.CodigoGrano });
        }

        [HttpPost]
        [Route("api/Datos/GetProductos")]
        public HttpResponseMessage GetProductos(IList<string> ids)
        {
            GranoStore store = new GranoStore();
            IList<Grano> granos = store.FindByIds(ids);
            return Request.CreateResponse(HttpStatusCode.OK, granos.Select(grano => new { Nombre = grano.Nombre, Codigo = grano.CodigoGrano }));
        }

        [HttpGet]
        [Route("api/Datos/Centros")]
        public HttpResponseMessage GetCentros()
        {
            ICentroStore store = new CentroStore();
            return Request.CreateResponse(HttpStatusCode.OK, store.FindAll().Select(x => new { Nombre = x.Nombre, Codigo = x.CodigoCentro }));
        }

        [HttpPost]
        [Route("api/Datos/GetCentros")]
        public HttpResponseMessage GetCentros(IList<string> ids)
        {
            CentroStore store = new CentroStore();
            IList<Centro> centros = store.FindByIds(ids);
            return Request.CreateResponse(HttpStatusCode.OK, centros.Select(centro => new { Nombre = centro.Nombre, Codigo = centro.CodigoCentro }));
        }

        [HttpGet]
        [Route("api/Datos/Monedas")]
        public HttpResponseMessage GetMonedas()
        {
            ServicioVarios servicio = new ServicioVarios();
            return Request.CreateResponse(HttpStatusCode.OK, servicio.GetMonedas().Select(x => new { Nombre = x.Valor, Codigo = x.Clave }));
        }

        [HttpPost]
        [Route("api/Datos/GetMonedas")]
        public HttpResponseMessage GetMonedas(IList<string> ids)
        {
            ServicioVarios store = new ServicioVarios();
            IList<DTabla> monedas = store.GetMonedas(ids);
            return Request.CreateResponse(HttpStatusCode.OK, monedas.Select(moneda => new { Nombre = moneda.Valor, Codigo = moneda.Clave }));
        }

        [HttpGet]
        [Route("api/Datos/CondicionesMercaderia")]
        public HttpResponseMessage GetCondicionesMercaderia()
        {
            ServicioVarios servicio = new ServicioVarios();
            return Request.CreateResponse(HttpStatusCode.OK, servicio.GetCondicionMercaderia().Select(x => new { Nombre = x.Valor, Codigo = x.Clave }));
        }

        [HttpPost]
        [Route("api/Datos/GetCondicionesMercaderia")]
        public HttpResponseMessage GetCondicionesMercaderia(IList<string> ids)
        {
            ServicioVarios store = new ServicioVarios();
            IList<DTabla> mercaderias = store.GetCondicionMercaderia(ids);
            return Request.CreateResponse(HttpStatusCode.OK, mercaderias.Select(mercaderia => new { Nombre = mercaderia.Valor, Codigo = mercaderia.Clave }));
        }

        [HttpGet]
        [Route("api/Datos/TiposCuenta")]
        public HttpResponseMessage GetTiposCuenta()
        {
            ServicioVarios servicio = new ServicioVarios();
            return Request.CreateResponse(HttpStatusCode.OK, servicio.GetTipoCuenta().Select(x => new { Nombre = x.Valor, Codigo = x.Clave }));
        }

        [HttpPost]
        [Route("api/Datos/GetTiposCuenta")]
        public HttpResponseMessage GetTiposCuenta(IList<string> ids)
        {
            ServicioVarios store = new ServicioVarios();
            IList<DTabla> tipos = store.GetTipoCuenta(ids);
            return Request.CreateResponse(HttpStatusCode.OK, tipos.Select(tipo => new { Nombre = tipo.Valor, Codigo = tipo.Clave }));
        }

        [HttpGet]
        [Route("api/Datos/TiposOperacion")]
        public HttpResponseMessage GetTiposOperacion()
        {
            ServicioVarios servicio = new ServicioVarios();
            return Request.CreateResponse(HttpStatusCode.OK, servicio.GetTipoOperacion().Select(x => new { Nombre = x.Valor, Codigo = x.Clave }));
        }

        [HttpPost]
        [Route("api/Datos/GetTiposOperacion")]
        public HttpResponseMessage GetTiposOperacion(IList<string> ids)
        {
            ServicioVarios store = new ServicioVarios();
            IList<DTabla> tipos = store.GetTipoOperacion(ids);
            return Request.CreateResponse(HttpStatusCode.OK, tipos.Select(tipo => new { Nombre = tipo.Valor, Codigo = tipo.Clave }));
        }

        [HttpGet]
        [Route("api/Datos/ZonaComercial")]
        public HttpResponseMessage GetZonaComercial()
        {
            ServicioVarios servicio = new ServicioVarios();
            return Request.CreateResponse(HttpStatusCode.OK, servicio.GetZonaComercial().Select(x => new { Nombre = x.Descripcion, Codigo = x.Codigo }));
        }

        [HttpPost]
        [Route("api/Datos/GetZonaComercial")]
        public HttpResponseMessage GetZonaComercial(IList<string> ids)
        {
            ServicioVarios store = new ServicioVarios();
            return Request.CreateResponse(HttpStatusCode.OK, store.GetZonaComercial(ids).Select(zona => new { Nombre = zona.Descripcion, Codigo = zona.Codigo }));
        }
    }
}
