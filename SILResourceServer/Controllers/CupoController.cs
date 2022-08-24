using ResourceServer.Models;
//using ResourceServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ResourceServer.Controllers
{
    public class CupoController : ApiController
    {
        private ServicioCupo servicioCupo;

        public CupoController()
        {
            this.servicioCupo = new ServicioCupo();
        }

        // GET: api/Cupo
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Cupo/5
        public string Get(int id)
        {
            return "bien claudio javier" + id;
        }

        [HttpGet]
        [Route("api/Cupo/GetCuposAlfa/{alfa}")]
        public object GetCuposAlfa(string alfa)
        {
            //return Json("bien claudio javier " + alfa);
            List<string> list = new List<string>();
            list.Add(alfa);
            if (list != null && list.Count() > 0)
            {
                return this.GetCuposPorCodigoAlfanumerico(list);
            }
            return null;
        }

        [HttpPost]
        [Route("api/Cupo/GetCuposAlfa")]
        public IHttpActionResult GetCuposPorAlfa([FromBody]IEnumerable<string> alfas)
        {
            List<string> list = new List<string>(alfas);

            if (list != null && list.Count() > 0)
            {
                //List<string> alfas = new List<string>(new string[]{"VAV023564130719","VAV782364130719"});
                var cupos = this.GetCuposPorCodigoAlfanumerico(list);
                return Json(cupos);
            }
            return null;
        }

        // POST: api/Cupo
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Cupo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Cupo/5
        public void Delete(int id)
        {
        }
        public object GetCuposPorCodigoAlfanumerico(IList<string> CodigosAlfanumericos)
        {
            //return Json("bien claudio javier");
            ServicioCupo Servicio = new ServicioCupo();
            IList<EstadoAlfanumericoModel> Cupos = Servicio.ObtenerCuposPorCodigosAlfanumericos(CodigosAlfanumericos);
            object cuposFiltro = Cupos.Select(x => new
            {
                Alfanumerico = x.Alfanumerico,
                Estado = x.GetEstado().Codigo,
                NombreEstado = x.GetEstado().Nombre,
                Fecha = x.Fecha.ToString("dd/MM/yyyy"),
                Comprador = x.Comprador,
                Vendedor = string.IsNullOrEmpty(x.Vendedor) ? string.Empty : x.Vendedor,
                Puerto = x.Puerto,
                Grano = x.Grano,
                Centro = x.CodCentroOrigen,
                CentroDistribucion = x.CodCentroDistribucion
            });
            return cuposFiltro;
        }
    }
}
