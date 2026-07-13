using ResourceServer.Models;
using ResourceServer.Models.AtributosValidacion;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ResourceServer.Controllers
{
  [ExceptionHandling]
  public class PuertosSTOPController : ApiController
  {
    [HttpPost]
    [Route("api/PuertosSTOP/GetPuerto")]
    public HttpResponseMessage GetPuertoSTOP(CuposPuertoSTOP puerto)
    {
      ServicioPuertoStop servicio = new ServicioPuertoStop();
      if (ModelState.IsValid)
      {
        return Request.CreateResponse(HttpStatusCode.OK, servicio.GetPuertos((puerto != null)? puerto.NroPuerto: 0));
      }
      return Request.CreateResponse(HttpStatusCode.BadRequest, "Solicitud Incompleta");
    }

    [HttpPost]
    [Route("api/PuertosSTOP/AddPuerto")]
    public HttpResponseMessage AddPuerto(CuposPuertoSTOP puerto)
    {
      if (ModelState.IsValid)
      {
        ServicioPuertoStop servicio = new ServicioPuertoStop();
        return Request.CreateResponse(HttpStatusCode.OK, servicio.AddPuerto(puerto));

      }
      return Request.CreateResponse(HttpStatusCode.BadRequest, "Solicitud Incompleta");
    }

    [HttpPost]
    [Route("api/PuertosSTOP/DeletePuerto")]
    public HttpResponseMessage DeletePuerto(CuposPuertoSTOP puerto)
    {
      if (ModelState.IsValid)
      {
        ServicioPuertoStop servicio = new ServicioPuertoStop();
        return Request.CreateResponse(HttpStatusCode.OK, servicio.EliminarPuerto(puerto));

      }
      return Request.CreateResponse(HttpStatusCode.BadRequest, "Solicitud Incompleta");
    }

    [HttpPost]
    [Route("api/PuertosSTOP/UpdatePuerto")]
    public HttpResponseMessage UpdatePuerto(CuposPuertoSTOP puerto)
    {
      if (ModelState.IsValid)
      {
        ServicioPuertoStop servicio = new ServicioPuertoStop();
        return Request.CreateResponse(HttpStatusCode.OK, servicio.UpdatePuerto(puerto));

      }
      return Request.CreateResponse(HttpStatusCode.BadRequest, "Solicitud Incompleta");
    }
  }
}