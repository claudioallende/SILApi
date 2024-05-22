using ResourceServer.Models;
using ResourceServer.Models.SolicitudesTurnos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ResourceServer.Controllers
{
  //[Authorize]
  public class SolicitudesTurnosController : ApiController
  {
    private SolicitudTurnoService SolicitudTurnoService { get; set; }

    public SolicitudesTurnosController()
    {
      SolicitudTurnoService = new SolicitudTurnoService();
    }

    //[HttpGet]
    //[Route("api/SolicitudesTurnos")]
    //public HttpResponseMessage Get()
    //{
    //  IEnumerable<SolicitudTurnoView> solicitudes = SolicitudTurnoService.GetAll(DateTime.Now.Date, DateTime.Now.Date.AddDays(7));
    //  return Request.CreateResponse(HttpStatusCode.OK, solicitudes);
    //}

    [HttpGet]
    [Route("api/SolicitudesTurnos/{cuentaVendedor}")]
    public HttpResponseMessage GetByVendedor(long cuentaVendedor)
    {
      IEnumerable<SolicitudTurnoView> solicitudes = SolicitudTurnoService.GetByVendedor(cuentaVendedor);
      return Request.CreateResponse(HttpStatusCode.OK, solicitudes);
    }

    [HttpGet]
    [Route("api/SolicitudesTurnos/Grupo")]
    public HttpResponseMessage GetGroup([FromUri] bool futuro = false)
    {
      DateTime today = DateTime.Now;
      IEnumerable<SolicitudTurnoView> todasSolicitudes = SolicitudTurnoService.GetAll(DateTime.Now.Date, DateTime.Now.Date.AddDays(7), futuro);
      IEnumerable<SolicitudTurnoGrupoView> solicitudes = todasSolicitudes
        .GroupBy(s => new { s.CodigoGrano, s.NombreGrano, s.CuentaComprador, s.NombreComprador, s.CuentaVendedor, s.NombreVendedor, s.CuentaDestino, s.NombreDestino })
        .Select(solPorGrano => new SolicitudTurnoGrupoView
        {
          CodigoGrano = solPorGrano.Key.CodigoGrano,
          NombreGrano = solPorGrano.Key.NombreGrano,
          CuentaComprador = solPorGrano.Key.CuentaComprador,
          NombreComprador = solPorGrano.Key.NombreComprador,
          CuentaVendedor = solPorGrano.Key.CuentaVendedor,
          NombreVendedor = solPorGrano.Key.NombreVendedor,
          CuentaDestino = solPorGrano.Key.CuentaDestino,
          NombreDestino = solPorGrano.Key.NombreDestino,
          CantidadFechas = solPorGrano.GroupBy(x => x.FechaSolicitado).Select(x => new SolicitudTurnoDetalleGrupoView()
          {
            Fecha = x.Key.ToString("dd/MM/yyyy"),
            Cantidad = x.Count(),
            DiaSemana = (x.Key.Date - DateTime.Now.Date).Days
          })
        });
      return Request.CreateResponse(HttpStatusCode.OK, solicitudes);
    }

    [HttpGet]
    [Route("api/SolicitudesTurnos/Grupo/{cuentaVendedor}")]
    public HttpResponseMessage GetGroupByVendedor(long cuentaVendedor)
    {
      IEnumerable<SolicitudTurnoView> todasSolicitudes = SolicitudTurnoService.GetByVendedor(cuentaVendedor);
      IEnumerable<SolicitudTurnoGrupoVendedorView> solicitudes = todasSolicitudes
        .GroupBy(s => new { s.CodigoGrano, s.NombreGrano })
        .Select(solPorGrano => new SolicitudTurnoGrupoVendedorView
        {
          CodigoGrano = solPorGrano.Key.CodigoGrano,
          NombreGrano = solPorGrano.Key.NombreGrano,
          DetallePendientesDia = solPorGrano
            .Where(sol => sol.CodigoEstado != 3)
            .GroupBy(sol => sol.FechaSolicitado)
            .Select(sol => new SolicitudTurnoGrupoDetallePendienteDiaView
            {
              Fecha = sol.Key.ToString("dd/MM/yyyy"),
              Cantidad = sol.Count()
            }),
          DetallesSolicitados = solPorGrano
            .GroupBy(sol => new { sol.CuentaComprador, sol.NombreComprador, sol.NombreDestino, sol.CuentaDestino })
            .Select(solPorGranoCompradorDestino => new SolicitudTurnoGrupoDetalleSolicitadoView
            {
              CuentaComprador = solPorGranoCompradorDestino.Key.CuentaComprador,
              NombreComprador = solPorGranoCompradorDestino.Key.NombreComprador,
              CuentaDestino = solPorGranoCompradorDestino.Key.CuentaDestino,
              NombreDestino = solPorGranoCompradorDestino.Key.NombreDestino,
              DetallesSolicitadosDia = solPorGranoCompradorDestino
                .GroupBy(sol => sol.FechaSolicitado)
                .Select(solPorGranoCompradorDestinoFecha => new SolicitudTurnoGrupoDetalleSolicitadoDiaView
                {
                  Fecha = solPorGranoCompradorDestinoFecha.Key.ToString("dd/MM/yyyy"),
                  TurnosActivos = solPorGranoCompradorDestinoFecha.Where(x => x.CtgCupo.HasValue).Count(), //Tengo que consultar en los estados de los cupos
                  TurnosOtorgados = solPorGranoCompradorDestinoFecha.Where(x => x.EstadoCupo.HasValue).Count() //Tengo que consultar en los estados del CTG
                })
            })
        });
      return Request.CreateResponse(HttpStatusCode.OK, solicitudes);
    }
  }
}
