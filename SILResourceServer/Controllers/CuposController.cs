using System;
using System.Collections.Generic;
using System.Linq;
using ResourceServer.Models;
using ResourceServer.Models.Contratos;
using ResourceServer.Models.Email;
using ResourceServer.Models.Filtro;
using ResourceServer.Models.Error.Exceptions;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using ResourceServer.Models.AtributosValidacion;
using ResourceServer.Models.DataAccess;

namespace ResourceServer.Controllers
{
  [ExceptionHandling]
  public class CuposController : ApiController
  {
    [Route("api/Cupos/Index")]
    public HttpResponseMessage Index(IndexCupoViewModel Model)
    {
      Model.Filtro();
      return Request.CreateResponse(HttpStatusCode.OK, Model);
    }

    [HttpGet]
    [Route("api/Cupos/Detalle/{id}")]
    public HttpResponseMessage Detalle(string id, string centroorigen, string centrodistribucion, bool cyo = false)
    {
      var model = new DetalleCupoViewModel(id, centroorigen, centrodistribucion, cyo);
      return Request.CreateResponse(HttpStatusCode.OK, model);
    }

    // GET: Cupos/Create
    [HttpGet]
    [Route("api/Cupos/Nuevo/{id}")]
    public HttpResponseMessage Nuevo(int id = 0)
    {
      var model = new NuevoCupoViewModel();
      model.ProductoSeleccionado = id;
      return Request.CreateResponse(HttpStatusCode.OK, model);
    }

    // POST: Cupos/Create
    [HttpPost]
    [Route("api/Cupos/Nuevo")]
    public HttpResponseMessage Nuevo(NuevoCupoViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return Request.CreateErrorResponse(HttpStatusCode.OK, "Campos requeridos no ingresados");
      }

      model.Error = "";
      model.Save();
      return Request.CreateResponse(HttpStatusCode.OK);
    }

    // GET: Cupos/Editar/5
    [HttpGet]
    [Route("api/Cupos/Editar/{id}")]
    public HttpResponseMessage Editar(string id, string centroorigen, string centrodistribucion, bool cyo = false)
    {
      var model = new EditarCupoViewModel(id);
      if (cyo)
      {
        model.BuscarYMostrarDatosCyO(centroorigen, centrodistribucion);
      }
      else
      {
        model.BuscarYMostrarDatos(centroorigen, centrodistribucion);
      }

      if (model.Consignaciones.Count > 0)
      {
        ServicioCuenta sCuenta = new ServicioCuenta(new CuitStore());
        foreach (Consignacion consignacion in model.Consignaciones)
        {
          if (!string.IsNullOrEmpty(consignacion.ContactoComercial))
          {
            var contactosComerciales = consignacion.ContactoComercial.Split(';');
            consignacion.NomContactoComercial = sCuenta.GetCuentasByCuentas(contactosComerciales.ToList()).Select(x => x.Nombre).ToList();
          }
          else
          {
            consignacion.ContactoComercial = "";
          }
        }
      }

      return Request.CreateResponse(HttpStatusCode.OK, model);
    }

    [HttpPost]
    [Route("api/Cupos/Editar")]
    public HttpResponseMessage Editar(BusquedaEditarViewModel model)
    {
      if (ModelState.IsValid)
      {
        model.Modelo.Inicializar();
        if (model.Id.Cyo.ToUpper().Trim() == "TRUE")
        {
          model.Modelo.BuscarYMostrarDatosCyO(model.Id.CentroOrigen, model.Id.CentroDistribucion);
        }
        else
        {
          model.Modelo.BuscarYMostrarDatos(model.Id.CentroOrigen, model.Id.CentroDistribucion);
        }
        return Request.CreateResponse(HttpStatusCode.OK, model.Modelo);
      }
      return Request.CreateErrorResponse(HttpStatusCode.OK, "Modelo no valido");
    }

    [HttpPost]
    [Route("api/Cupos/Anular")]
    public HttpResponseMessage Anular(AnularViewModel anula)
    {
      string result = "SIN MOTIVO";
      ServicioEditar model = new ServicioEditar();
      if (!string.IsNullOrEmpty(anula.Motivo.Trim()))
      {
        result = "ANULADOS";
        try
        {
          model.Anular(anula.IdCupos, anula.Motivo, anula.Tipo, anula.Cyo);
        }
        catch (CupoSinDistribucionException e)
        {
          result = e.Message;
        }
        catch (Exception e)
        {
          result = "ERROR";
        }
      }
      return Request.CreateResponse(HttpStatusCode.OK, new
      {
        Respuesta = result,
        Tipo = anula.Tipo,
        Cupos = model.GetCuposModificados().Select(x => new { Id = x.Id, Alfanumerico = x.Nrocupo, CuentaVendedor = x.Vendcta, Status = x.Status })
      });
    }

    [HttpGet]
    [Route("api/Cupos/Distribucion/{id}")]
    public HttpResponseMessage Distribucion(string id, string centroorigen, string centrodistribucion, string cyo = "false")
    {
      var model = new DistribuirCupoViewModel(id, cyo);
      model.ObtenerFiltro();
      return Request.CreateResponse(HttpStatusCode.OK, model);
    }
    /*CA: En este endpoint retornamos las consignaciones para la consulta de la pagina distribucion. definir que hacemos con caratula y CC*/
    [HttpPost]
    [Route("api/Cupos/Distribucion")]
    public HttpResponseMessage Distribucion(BusquedaDistribucionViewModel model)
    {
      //model.Modelo.Id = model.Id.Id;
      if (ModelState.IsValid)
      {
        //model.ObtenerFiltro();
        model.Modelo.ObtenerIds(model.Id.Id);
        model.Modelo.Inicializar(model.Id.Cyo);
        model.Modelo.BuscarYMostrarConsignaciones();
        if (model.Modelo.Consignaciones.Count > 0)
        {
          ServicioCuenta sCuenta = new ServicioCuenta(new CuitStore());
          foreach(Consignacion consignacion in model.Modelo.Consignaciones)
          {
            if (!string.IsNullOrEmpty(consignacion.ContactoComercial))
            {
              var contactosComerciales = consignacion.ContactoComercial.Split(';');
              consignacion.NomContactoComercial = sCuenta.GetCuentasByCuentas(contactosComerciales.ToList()).Select(x => x.Nombre).ToList();
            }
          }
        }
        if (model.Modelo.Consignaciones.Count == 1)
        {
          model.Modelo.Id = model.Id.Id;
          model.Modelo.BuscarYMostrarDatos();
          //Mostrar los contratos si hay una sola consignacion
          ServicioContratos servicio = new ServicioContratos();
          model.Modelo.DistribucionesDisponibles.Distribuciones = servicio.ObtenerContratos(model.Modelo.GetDistribucionPrimeraConsignacion(), model.Modelo.EntregaDesde, model.Modelo.EntregaHasta, model.Modelo.CosechaDesde, model.Modelo.CosechaHasta);
        }
        else if (model.Modelo.Consignaciones.Count == 0)
        {
          return Request.CreateErrorResponse(HttpStatusCode.OK, "No hay Consignaciones Disponibles para el id ingresado");
        }
        return Request.CreateResponse(HttpStatusCode.OK, model.Modelo);
      }
      return Request.CreateResponse(HttpStatusCode.OK, model.Modelo);
    }

    //Ver responder los errores
    [HttpPost]
    [ValidateAjax]
    [Route("api/Cupos/ActualizarDistribucion")]
    public HttpResponseMessage ActualizarDistribucion(RegistroDistribucionViewModel model)//, bool Confirmacion
    {
      if (model == null) return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Campos requeridos no ingresados");
      if (ModelState.IsValid)
      {
        ServicioDistribuir servicio = new ServicioDistribuir(model.Confirmacion);
        int response = 0;
        response = servicio.ValidarYGuardar(model);
        ServicioFiltro filtro = new ServicioFiltro(model.anterior.Grano, model.anterior.Puerto, model.anterior.Compcta);
        filtro.GuardarFiltro(model.cupos, model.fechaDesde, model.fecha, model.CosechaDesde, model.CosechaHasta, model.CentroSeleccionado);
        return Request.CreateResponse(HttpStatusCode.OK, response);
      }
      else
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Campos requeridos no ingresados");
      }

    }

    [HttpPost]
    [Route("api/Cupos/Contratos")]
    public HttpResponseMessage Contratos(BusquedaMultiplesConsignacionesViewModel model)
    {
      ServicioDistribuir servicio = new ServicioDistribuir();
      return Request.CreateResponse(HttpStatusCode.OK, servicio.ObtenerContratos(model.compcta, model.vendcta, model.ctadestino, model.codcentro, model.grano, model.fechaent)
          .Select(c => new
          {
            contrato = c.Contrato,
            neg = c.Negocio,
            oper = c.Operacion,
            prod = c.Producto,
            cos = c.Cosecha,
            entrega = c.Entrega.ToString("dd/MM/yyyy"),
            vtoEnt = c.Vtoent.ToString("dd/MM/yyyy"),
            pactadas = c.Pactadas,
            apli = c.Aplicadas,
            pendEnt = c.Pendientes,
            liq = c.Liquidadas,
            destino = c.Destino,
            precio = c.Precio
          }));
    }

    [HttpGet]
    [Route("api/Cupos/ContratosVendedor")]
    public HttpResponseMessage ContratosVendedor(BusquedaContratosVendedorViewModel model)
    {
      ServicioDistribuir servicio = new ServicioDistribuir();
      return Request.CreateResponse(HttpStatusCode.OK, servicio.ObtenerContratos(model.Comprador, model.Vendedor, model.Destino, model.TipoDestino, model.Grano, model.FechaDesde, model.FechaHasta)
          .Select(c => new
          {
            contrato = c.Contrato,
            neg = c.Negocio,
            oper = c.Operacion,
            prod = c.Producto,
            cos = c.Cosecha,
            entrega = c.Entrega.ToString("dd/MM/yyyy"),
            vtoEnt = c.Vtoent.ToString("dd/MM/yyyy"),
            pactadas = c.Pactadas,
            apli = c.Aplicadas,
            pendEnt = c.Pendientes,
            liq = c.Liquidadas,
            destino = c.Destino,
            precio = c.Precio
          }));
    }

    [HttpGet]
    [Route("api/Cupos/GetMotivo/{id}")]
    public HttpResponseMessage GetMotivo(long id)
    {
      ServicioEditar servicio = new ServicioEditar();
      return Request.CreateResponse(HttpStatusCode.OK, servicio.GetMotivoAnulacion(id));
    }

    [HttpPost]
    [Route("api/Cupos/InformarCuposPorLote")]
    public HttpResponseMessage InformarCuposPorLote(IList<InformarPorLoteViewModel> Lote)
    {
      InformarPorLoteViewModel PrimerElemento = Lote.ElementAt(0);
      ServicioInformarAVendedor servicioDistribucion = new InformarDistribucionPorMail(PrimerElemento.CuentaComprador, PrimerElemento.CuentaPuerto,
          PrimerElemento.CodigoGrano, PrimerElemento.CodigoCentro, PrimerElemento.CodigoCentroDistribucion, PrimerElemento.Cyo);

      ServicioInformarAVendedor servicioAnulacion = new InformarAnulacionPorMail(PrimerElemento.CuentaComprador, PrimerElemento.CuentaPuerto,
          PrimerElemento.CodigoGrano, PrimerElemento.CodigoCentro, PrimerElemento.CodigoCentroDistribucion, PrimerElemento.Cyo);

      List<EmailInformado> EmailsInformados = new List<EmailInformado>();
      foreach (var Informa in Lote)
      {
        EmailsInformados.AddRange(servicioDistribucion.InformarMails(Informa.CuentaVendedor));
        EmailsInformados.AddRange(servicioAnulacion.InformarMails(Informa.CuentaVendedor));
      }
      string Centro = Lote.Select(x => x.CodigoCentro).FirstOrDefault();
      string CentroDistribucion = Lote.Select(x => x.CodigoCentroDistribucion).FirstOrDefault();
      var modelo = new DetalleCupoViewModel(Lote.Select(x => x.CuentaComprador).FirstOrDefault().ToString() + "-" + Lote.Select(x => x.CuentaPuerto).FirstOrDefault().ToString() + "-" + Lote.Select(x => x.CodigoGrano).FirstOrDefault(), Centro, CentroDistribucion, Lote.Select(x => x.Cyo).FirstOrDefault());
      return Request.CreateResponse(HttpStatusCode.OK, new
      {
        result = EmailsInformados,
        model = modelo.CuposAgrupados
      }
      );
    }

    /// <summary>
    /// Busca y muestra los contratos agrupados por vendedor y destino.
    /// </summary>
    /// <param name="datosContrato"></param>
    /// <param name="CuentaPuerto"></param>
    /// <param name="fechaDesde"></param>
    /// <param name="fechaHasta"></param>
    /// <param name="cosechaDesde"></param>
    /// <param name="cosechaHasta"></param>
    /// <param name="Cyo"></param>
    /// <returns>Retorna un PartialView de una tabla con los totales de cupos pendiente de distribuir por dia incluido.</returns>
    [HttpPost]
    [Route("api/Cupos/GetContratos")]
    public HttpResponseMessage GetTablaContratos(BusquedaContratosViewModel model)
    {
      /*CA: ver si en esta etapa del cambio es necesario modificar este metodo - ver con el front*/
      ServicioDistribuir servicio = new ServicioDistribuir();
      DistribucionDisponible Distribucion = servicio.ObtenerContratosEInformacion(model.datosContrato, model.CuentaPuerto, model.fechaDesde, model.fechaHasta, model.cosechaDesde, model.cosechaHasta, model.ConsignacionSeleccionada, model.Cyo);
      return Request.CreateResponse(HttpStatusCode.OK, Distribucion);
    }

    [HttpPost]
    [Route("api/Cupos/GetCuposPorCodigoAlfanumerico")]
    public HttpResponseMessage GetCuposPorCodigoAlfanumerico(IList<string> CodigosAlfanumericos)
    {
      ServicioCupo Servicio = new ServicioCupo();
      IList<EstadoAlfanumericoModel> Cupos = Servicio.ObtenerCuposPorCodigosAlfanumericos(CodigosAlfanumericos);
      return Request.CreateResponse(HttpStatusCode.OK,
          Cupos.Select(x => new
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
            CentroDistribucion = x.CodCentroDistribucion,
            DetalleCupoCNRT = x.DetalleCupoCNRT,
            IdCupo = x.IdCupo
          })
      );
    }

    [HttpPost]
    [Route("api/Cupos/GetCuposIn")]
    public HttpResponseMessage GetCuposIn(filtroInterface filtro)
    {
      ServicioCupo Servicio = new ServicioCupo();
      if (filtro != null)
      {
        IList<DetalleCupoComplete> DetalleCupo = Servicio.ObtenerCuposDistribuidosEInformadosEnPeriodo(filtro.Desde, filtro.Hasta, filtro.Vendedores);
        return Request.CreateResponse(HttpStatusCode.OK, DetalleCupo);
      }
      return Request.CreateResponse(HttpStatusCode.BadRequest, "Requiere el periodo de fechas");
    }

    [HttpPost]
    [Route("api/Cupos/GetCuposForId")]
    public HttpResponseMessage GetCuposForId(IList<long> Ids)
    {
      ServicioCupo Servicio = new ServicioCupo();
      IList<DetalleCupoComplete> DetalleCupo = Servicio.ObtenerCuposPorIds(Ids);
      return Request.CreateResponse(HttpStatusCode.OK, DetalleCupo);
    }

    [HttpGet]
    [Route("api/Cupos/GetVendedoresDeCuposPendientesDeCTG")]
    public HttpResponseMessage GetVendedoresDeCuposPendientesDeCTG()
    {
      ServicioCupo Servicio = new ServicioCupo();
      List<long> cuentasVend = Servicio.GetVendedoresConCuposPendDeCTG();
      return Request.CreateResponse(HttpStatusCode.OK, cuentasVend);
    }

    [HttpGet]
    [Route("api/Cupos/InformarCuposSinCTG/{dia}")]
    public HttpResponseMessage InformarCuposSinCTG(int dia = 0)
    {
      ServicioInformarPorEmail servicioInforma = new ServicioInformarPorEmail();
      List<EmailInformado> EmailsInformados = new List<EmailInformado>();
      EmailsInformados = servicioInforma.InformarCuposSinCTG(dia);
      return Request.CreateResponse(HttpStatusCode.OK, EmailsInformados);
    }

    [HttpGet]
    [Route("api/Cupos/GetDetalleDeIncumplimientoDeCupos/{tipoDeInforme}/{cuenta:long?}/{xdesde?}/{xhasta?}")]
    public HttpResponseMessage GetDetalleDeIncumplimientoDeCupos(string tipoDeInforme, long cuenta = 0, string xdesde = "", string xhasta = "")
    {
      if (Resource.Constants.toInformes(tipoDeInforme) != Resource.Constants.Informes.NINGUNO)
      {
        ServicioInformarPorEmail servicioInforma = new ServicioInformarPorEmail();
        List<EmailInformado> listaDetalle = new List<EmailInformado>();
        listaDetalle = servicioInforma.InformarDetalleDeIncumplimientoDeCTG(tipoDeInforme, xdesde, xhasta, cuenta);
        return Request.CreateResponse(HttpStatusCode.OK, listaDetalle);
      }
      else
      {
        return Request.CreateResponse(HttpStatusCode.BadRequest, "Tipo De Informe inexistente");
      }

    }

    [HttpGet]
    [Route("api/Vendedor/TurnosPendientesDeCTG")]
    public HttpResponseMessage TurnosPendientesDeCTG()
    {
      ServicioCupo Servicio = new ServicioCupo();
      List<Vendedor> Vendedores = Servicio.GetVendedoresConTurnosPendientesDeCTG();
      return Request.CreateResponse(HttpStatusCode.OK, Vendedores);
    }

    [HttpGet]
    [Route("api/Turnos/GetDetalleDeIncumplimientoDeTurnos/{tipoDeInforme}/{cuenta:long?}/{xdesde?}/{xhasta?}")]
    public HttpResponseMessage GetDetalleDeIncumplimientoDeTurnos(string tipoDeInforme, long cuenta = 0, string xdesde = "", string xhasta = "")
    {
      if (Resource.Constants.toInformes(tipoDeInforme) != Resource.Constants.Informes.NINGUNO)
      {
        ServicioInformarPorEmail servicioInforma = new ServicioInformarPorEmail();
        IList<DetalleDeIncumplimientoCupo> listaDetalle = new List<DetalleDeIncumplimientoCupo>();
        listaDetalle = servicioInforma.GetDetalleDeIncumplimientoDeCupos(tipoDeInforme, xdesde, xhasta, cuenta);
        return Request.CreateResponse(HttpStatusCode.OK, listaDetalle);
      }
      else
      {
        return Request.CreateResponse(HttpStatusCode.BadRequest, "Tipo De Informe inexistente");
      }

    }
  }
}