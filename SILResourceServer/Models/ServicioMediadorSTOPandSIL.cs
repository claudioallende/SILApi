using NHibernate;
using ResourceServer.Models.Configuracion;
using ResourceServer.Models.DataAccess;
using ResourceServer.Models.DTO;
using ResourceServer.Models.Error.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
  public class ServicioMediadorSTOPandSIL
  {
    CuitStore cuitsStore = new CuitStore();
    ISession session;

    public ServicioMediadorSTOPandSIL()
    {
      session = HibernateUtil.OpenSession();
    }

    #region MetodosDeConsulta

    /// <summary>
    /// retorna el conjunto de cupos agrupados dentro de un IndexCupoViewModel
    /// que es la clase utilizada en las vista de SIL.
    /// </summary>
    /// <returns></returns>
    public IndexCupoAutorizarViewModel CuposPendientesDeAgregarEnSIL4View()
    {
      IndexCupoAutorizarViewModel indexcupo = new IndexCupoAutorizarViewModel();
      indexcupo.CuposAgrupados = CuposAgrupadosPendientesDeAgregarEnSIL();
      return indexcupo;
    }

    /// <summary>
    /// este metodo retorna el conjunto de cupos pendientes de autorizar con formato List<ICuposAgrupados>, es decir,
    /// con el formato utilizado por las vistas que utiliza SIL para mostrar la grilla de la pagina inicial de autorizacion
    /// Estos cupos. Se encuentran Agrupados segun el metodo que se invoque
    /// </summary>
    /// <returns></returns>
    public IList<ICuposAgrupados> CuposAgrupadosPendientesDeAgregarEnSIL()
    {
      List<NuevoCupoViewModelDTO> listaDeCupos = new List<NuevoCupoViewModelDTO>();
      try
      {
        IList<CupoInStopAndNotSIL> listaDeCuposInSTOP = this.TurnosPendientesDeAgregar();
        IList<CupoInStopAndNotSIL> compPuertGranoVendCentro = this.GranoPuertoCompradorVendedorPendientesDeAutorizar(listaDeCuposInSTOP);
        List<ICuposAgrupados> CuposAgrupados = new List<ICuposAgrupados>();
        foreach (var consignacion in compPuertGranoVendCentro)
        {
          var listaDeCuposdeSTOPXconsig = listaDeCuposInSTOP.Where
                              (x =>
                                      x.Grano == consignacion.Grano
                                      && x.Idterminal == consignacion.Idterminal
                                      && x.Cuitdestinatario == consignacion.Cuitdestinatario
                                      && x.Cuitremcomercial == consignacion.Cuitremcomercial
                                      && x.Centro == consignacion.Centro
                                      && x.Cuitcorredorv == consignacion.Cuitcorredorv
                                      && x.Cuitcorredorc == consignacion.Cuitcorredorc
                              ).ToList();
          /*construimos el CuposAgrupados*/
          var conjuntoDeCupos = new CuposAgrupados();
          conjuntoDeCupos.Grano = consignacion.Grano;
          CuposGranoSTOP grano = this.ObtenerGranoSTOP(consignacion.Grano, session);
          conjuntoDeCupos.Nomgrano = (grano != null) ? grano.NombreGrano : "";

          Comprador comprador = this.ObtenerCompradorPorCuit(consignacion.Cuitdestinatario, session);
          conjuntoDeCupos.Compcta = (comprador != null) ? comprador.Cuenta : 0;
          conjuntoDeCupos.Comprador = (comprador != null) ? comprador.Nombre : ""; ;
          Vendedor vendedor = this.ObtenerVendedorPorCuit(consignacion.Cuitremcomercial, session);
          conjuntoDeCupos.VendCta = (vendedor != null) ? vendedor.Cuenta : 0;
          conjuntoDeCupos.Vendedor = (vendedor != null) ? vendedor.Nombre : "";

          CuposPuertoSTOP puerto = this.ObtenerPuertoPorIdTerminal(consignacion.Idterminal, session);//verificar si tambien debe filtrar por CUITDESTINO
          conjuntoDeCupos.PuertoCta = puerto != null ? puerto.NroPuerto: 0;
          conjuntoDeCupos.Puerto = puerto != null ? puerto.NombrePuerto: "";
          /*Centro LLeva el de la StopNoCorre*/
          string nomCentro = this.ObtenerNombreDeLCentro(consignacion.Centro, session);
          conjuntoDeCupos.CodCentro = consignacion.Centro;
          conjuntoDeCupos.Centro = nomCentro;
          conjuntoDeCupos.CodCentroDist = consignacion.Centro;
          conjuntoDeCupos.CentroDist = nomCentro;

          conjuntoDeCupos.CorredorC = string.IsNullOrEmpty(consignacion.Cuitcorredorc) ? 0 : Int64.Parse(consignacion.Cuitcorredorc);
          conjuntoDeCupos.CorredorV = string.IsNullOrEmpty(consignacion.Cuitcorredorv) ? 0 : Int64.Parse(consignacion.Cuitcorredorv);

          string Pdia = "", Pcupos = "";
          conjuntoDeCupos.Hoy = DateTime.Now.Date.ToString("dd/MM/yyyy");
          conjuntoDeCupos.D0Cr = this.numeroDeCuposPorDia(DateTime.Now.Date, listaDeCuposdeSTOPXconsig);
          for (int i = 1; i < 21; i++)
          {
            DateTime miFecha = DateTime.Now.Date.AddDays(i);
            Pdia = "Dia" + i;
            conjuntoDeCupos.GetType().GetProperty(Pdia).SetValue(conjuntoDeCupos, miFecha.ToString("dd/MM/yyyy"));
            Pcupos = "D" + i + "Cr";
            conjuntoDeCupos.GetType().GetProperty(Pcupos).SetValue(conjuntoDeCupos, this.numeroDeCuposPorDia(miFecha, listaDeCuposdeSTOPXconsig));

          }
          CuposAgrupados.Add(conjuntoDeCupos);
        }
        return CuposAgrupados;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    /// <summary>
    /// retorna los turnos en la stopNoCorre pendientes de ser autorizados para agregarse a SIL
    /// si pasamos filtros en el parametro datosParaFiltrar. Retorna los turnos en la stopNoCorre pendientes de agregar para esos datos
    /// si no se pasa ningun parametro retorna todos los cupos pendientes de ser autorizados
    /// </summary>
    /// <returns></returns>
    public IList<CupoInStopAndNotSIL> TurnosPendientesDeAgregar(NuevoCupoViewModelDTO datosParaFiltrar = null)
    {
      try
      {
        CupoInStopAndNotSILStore cuposSTOPStore = new CupoInStopAndNotSILStore();
        IList<CupoInStopAndNotSIL> listaDeCuposInSTOP;
        if (datosParaFiltrar == null)
        {
          listaDeCuposInSTOP = cuposSTOPStore.FindAllWithACACorredorCORDestinatarioORCorredorV(session);
        }
        else
        {
          CupoInStopAndNotSIL filtros = new CupoInStopAndNotSIL();
          filtros.SetCuitConsignacion(datosParaFiltrar.GetConsignacion());
          filtros.Grano = datosParaFiltrar.Producto;
          /* Si pasa como parametro comp-vend-puerto (son CUENTAS). obtengo los cuits en sus tablas - stopnocorre (son CUIT)*/
          if (!string.IsNullOrEmpty(datosParaFiltrar.Puerto))
          {
            Puerto puerto = this.ObtenerPuertoPorCuenta(datosParaFiltrar.Puerto, this.session);
            filtros.Idterminal = (puerto != null) ? Int32.Parse(puerto.IdTerminal.Trim()) : 0; //deberia ser puerto.IDTERMINAL (LONG)
          }
          if (!string.IsNullOrEmpty(datosParaFiltrar.Vendcta))
          {
            Vendedor vendedor = this.ObtenerVendedorPorCuenta(datosParaFiltrar.Vendcta, this.session);
            filtros.Cuitremcomercial = (vendedor != null) ? vendedor.Cuit : null;
          }
          if (!string.IsNullOrEmpty(datosParaFiltrar.Compcta))
          {
            Comprador comprador = this.ObtenerCompradorPorCuenta(datosParaFiltrar.Compcta, this.session);
            filtros.Cuitdestinatario = (comprador != null) ? comprador.Cuit : null;
          }
          if (!string.IsNullOrEmpty(datosParaFiltrar.Centro))
          {
            filtros.Centro = datosParaFiltrar.Centro;
          }
          listaDeCuposInSTOP = cuposSTOPStore.FindByFiltroAndACACorredorCORDestinatarioORCorredorV(filtros, session);
        }
        return listaDeCuposInSTOP;
      }
      catch (Exception a)
      {
        throw a;
      }
    }

    /// <summary>
    /// Lista los Grano Puerto Comprador Vendedor Pendientes De ser Autorizados para agregarse a SIL
    /// </summary>
    /// <param name="listOfCupos"></param>
    /// <returns></returns>
    private IList<CupoInStopAndNotSIL> GranoPuertoCompradorVendedorPendientesDeAutorizar(IList<CupoInStopAndNotSIL> listOfCupos)
    {
      IList<CupoInStopAndNotSIL> cuposSegunCompPuertoGrano = listOfCupos.GroupBy(x =>
                                          new
                                          {
                                            Grano = x.Grano,
                                            Idterminal = x.Idterminal,
                                            Cuitdestinatario = x.Cuitdestinatario,
                                            Cuitremcomercial = x.Cuitremcomercial,
                                            Centro = x.Centro,
                                            CorredorComprador = x.Cuitcorredorc,
                                            CorredorVendedor = x.Cuitcorredorv
                                          })
                                          .Select(y =>
                                              new CupoInStopAndNotSIL
                                              {
                                                Grano = y.Key.Grano,
                                                Idterminal = y.Key.Idterminal,
                                                Cuitdestinatario = y.Key.Cuitdestinatario,
                                                Cuitremcomercial = y.Key.Cuitremcomercial,
                                                Centro = y.Key.Centro,
                                                Cuitcorredorc = y.Key.CorredorComprador,
                                                Cuitcorredorv = y.Key.CorredorVendedor
                                              }
                                          )
                                          .OrderBy(x => x.Centro)
                                          .ThenBy(x => x.Grano)
                                          .ThenBy(x => x.Idterminal)
                                          .ThenBy(x => x.Cuitdestinatario)
                                          .ThenBy(x => x.Cuitremcomercial)
                                          .ThenBy(x => x.Cuitcorredorc)
                                          .ThenBy(x => x.Cuitcorredorv)
                                          .ToList();
      return cuposSegunCompPuertoGrano;
    }

    /// <summary>
    /// Retorna la lista de cupos pendientes de autorizar para el filtro especificado en el parametro datosDeFiltro. 
    /// Si no se pasa ningun parametro. Obtiente Todos los cupos pendientes de autorizar de la stopnocorre.
    /// Agrupa los turnos resultantes de la busqueda segun su consigancion.
    /// </summary>
    /// <returns>lista de alfas que no pudieron ser cargados</returns>
    public List<NuevoCupoViewModelDTO> CuposPendientesDeAgregarEnSIL(NuevoCupoViewModelDTO datosDeFiltro = null)
    {
      List<NuevoCupoViewModelDTO> listaDeCupos = new List<NuevoCupoViewModelDTO>();
      try
      {        
        ServicioConfiguracionCuentaCYO configuracion = new ServicioConfiguracionCuentaCYO();
        IList<CupoInStopAndNotSIL> listaDeCuposInSTOP = this.TurnosPendientesDeAgregar(datosDeFiltro);
        IList<CupoInStopAndNotSIL> consignaciones = this.ConsignacionesPendientesDeAgregar(listaDeCuposInSTOP);
        foreach (var consignacion in consignaciones)
        {
          IList<CupoInStopAndNotSIL> listaDeCuposdeSTOPXconsig = listaDeCuposInSTOP.Where(
                                      x =>
                                          x.Idterminal == consignacion.Idterminal
                                          && x.Cuitdestinatario == consignacion.Cuitdestinatario
                                          && x.Cuitorigen == consignacion.Cuitorigen
                                          && x.Cuitintermediario == consignacion.Cuitintermediario
                                          && x.Cuitmercadoatermino == consignacion.Cuitmercadoatermino
                                          && x.Cuitremcomercial == consignacion.Cuitremcomercial
                                          && x.Cuitcorredorv == consignacion.Cuitcorredorv
                                          && x.Cuitcorredorc == consignacion.Cuitcorredorc
                                          && x.Cuitrepresentanteentregador == consignacion.Cuitrepresentanteentregador
                                          && x.Cuitdestino == consignacion.Cuitdestino
                                          && x.Detallecupo == consignacion.Detallecupo
                                          && x.Centro == consignacion.Centro
                              ).Select(y =>
                                  new CupoInStopAndNotSIL
                                  {
                                    Idterminal = y.Idterminal,
                                    Cuitdestinatario = y.Cuitdestinatario,
                                    Cuitorigen = y.Cuitorigen,
                                    Cuitintermediario = y.Cuitintermediario,
                                    Cuitmercadoatermino = y.Cuitmercadoatermino,
                                    Cuitremcomercial = y.Cuitremcomercial,
                                    Cuitcorredorv = y.Cuitcorredorv,
                                    Cuitcorredorc = y.Cuitcorredorc,
                                    Cuitrepresentanteentregador = y.Cuitrepresentanteentregador,
                                    Cuitdestino = y.Cuitdestino,
                                    Detallecupo = y.Detallecupo,
                                    Fecha = y.Fecha,
                                    Idcupo = y.Idcupo,
                                    Nrocupo = y.Nrocupo,
                                    Cuitintermediarioflete = y.Cuitintermediarioflete,
                                    Cuittransportista = y.Cuittransportista,
                                    Cuitchofer = y.Cuitchofer,
                                    Centro = y.Centro
                                  }
                              )
                              .ToList();
          NuevoCupoViewModelDTO conjuntoDeCupos = new NuevoCupoViewModelDTO();
          conjuntoDeCupos = this.SetNombresDeLaConsignacion(conjuntoDeCupos, listaDeCuposdeSTOPXconsig.ElementAt(0).GetCuitConsignacion());
          /*Grano pasamos el de STOP. Que en el dropBox muestre los posibles granosSIL y por defecto el que corresponde*/
          CuposGranoSTOP granoSTOP = ObtenerGranoSTOP(consignacion.Grano, session);
          conjuntoDeCupos.Producto = (int)((granoSTOP != null) ? granoSTOP.NroGrano : 0);
          conjuntoDeCupos.ProductoNombre = (granoSTOP != null) ? granoSTOP.NombreGrano : "";
          
          /*obtener el nombre del idTerminal de la tabla cupospuertoSTOP*/
          CuposPuertoSTOP cp = ObtenerPuertoPorIdTerminal(consignacion.Idterminal, session);          
          conjuntoDeCupos.Puerto = (cp != null) ? cp.NroPuerto.ToString() : "";
          conjuntoDeCupos.PuertoNombre = (cp != null) ? cp.NombrePuerto.ToString() : "";

          Comprador comprador = this.ObtenerCompradorPorCuit(consignacion.Cuitdestinatario, session);
          conjuntoDeCupos.Compcta = (comprador != null) ? comprador.Cuenta.ToString() : "";
          conjuntoDeCupos.CompradorNombre = (comprador != null) ? comprador.Nombre.ToString() : "";
          Vendedor vendedor = this.ObtenerVendedorPorCuit(consignacion.Cuitremcomercial, session);
          conjuntoDeCupos.Vendcta = (vendedor != null) ? vendedor.Cuenta.ToString() : "";
          conjuntoDeCupos.VendedorNombre = (vendedor != null) ? vendedor.Nombre.ToString() : "";
          CuposCentroPorPuerto centro = this.ObtenerCentroPorIdTerminal(consignacion.Idterminal, session);
          if (string.IsNullOrEmpty(consignacion.Centro))
          {
            conjuntoDeCupos.Centro = (centro != null) ? centro.CodigoCentro : "";
            conjuntoDeCupos.CentroAnterior = (centro != null) ? centro.CodigoCentro : "";
          }
          else
          {
            conjuntoDeCupos.Centro = consignacion.Centro;
            conjuntoDeCupos.CentroAnterior = (centro != null) ? centro.CodigoCentro : "";
          }
          conjuntoDeCupos.VendcyoBoolValue = string.IsNullOrEmpty(conjuntoDeCupos.Vendcta) ? false : configuracion.EsCuentaYOrden(conjuntoDeCupos.Vendcta, session);
          conjuntoDeCupos.DetalleCupoCNRT = consignacion.Detallecupo;
          conjuntoDeCupos.EstadoCupoCNRT = 1; //DEFINIR: cristian deberia agregar el campo con el valor
          conjuntoDeCupos.CodigosDias = new List<CuposPorDiaDTO>();
          var fechasConAlfa = listaDeCuposdeSTOPXconsig.GroupBy(x => (DateTime)x.Fecha).ToList();
          foreach (var fecha in fechasConAlfa)
          {
            CuposPorDiaDTO cuposxDia = new CuposPorDiaDTO();
            cuposxDia.Fecha = fecha.Key.Date;
            cuposxDia.Turnos = listaDeCuposdeSTOPXconsig.Where(x => (DateTime)x.Fecha == fecha.Key.Date).Select(y => y.Nrocupo).ToList();
            conjuntoDeCupos.CodigosDias.Add(cuposxDia);
          }
          listaDeCupos.Add(conjuntoDeCupos);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
      return listaDeCupos;
    }

    /// <summary>
    /// Retorna la lista de las diferentes consignaciones pendientes de autorizar. 
    /// Toma como parametro listOfCupos, la lista de cupos pendientes donde consultará las consignaciones
    /// </summary>
    /// <param name="listOfCupos"></param>
    /// <returns></returns>
    public IList<CupoInStopAndNotSIL> ConsignacionesPendientesDeAgregar(IList<CupoInStopAndNotSIL> listOfCupos)
    {
      IList<CupoInStopAndNotSIL> cuposSegunConsignacion = listOfCupos.GroupBy(x =>
                                          new
                                          {
                                                  //Fecha = x.Fecha,
                                                  Grano = x.Grano, /*si*/
                                            Idterminal = x.Idterminal,  /*si*/
                                            Cuitdestinatario = x.Cuitdestinatario,  /*si*/
                                            Cuitorigen = x.Cuitorigen,
                                            Cuitintermediario = x.Cuitintermediario,
                                            Cuitmercadoatermino = x.Cuitmercadoatermino,
                                            Cuitremcomercial = x.Cuitremcomercial,
                                            Cuitcorredorv = x.Cuitcorredorv,
                                            Cuitcorredorc = x.Cuitcorredorc,
                                            Cuitrepresentanteentregador = x.Cuitrepresentanteentregador,
                                            Cuitdestino = x.Cuitdestino,    /*si*/
                                            Detallecupo = x.Detallecupo,
                                            Centro = x.Centro   /*si*/
                                          })
                                          .Select(y =>
                                              new CupoInStopAndNotSIL
                                              {
                                                Grano = y.Key.Grano,
                                                Idterminal = y.Key.Idterminal,
                                                Cuitdestinatario = y.Key.Cuitdestinatario,
                                                Cuitorigen = y.Key.Cuitorigen,
                                                Cuitintermediario = y.Key.Cuitintermediario,
                                                Cuitmercadoatermino = y.Key.Cuitmercadoatermino,
                                                Cuitremcomercial = y.Key.Cuitremcomercial,
                                                Cuitcorredorv = y.Key.Cuitcorredorv,
                                                Cuitcorredorc = y.Key.Cuitcorredorc,
                                                Cuitrepresentanteentregador = y.Key.Cuitrepresentanteentregador,
                                                Cuitdestino = y.Key.Cuitdestino,
                                                Detallecupo = y.Key.Detallecupo,
                                                Centro = y.Key.Centro
                                              }
                                          )
                                          .ToList();
      return cuposSegunConsignacion;
    }


    /// <summary>
    /// obtenemos el conjunto de consignaciones pendientes de autorizar para el filtro especificado en el parametro filtro.
    /// Si no se pasa ningu parametro obtiene todas las consignaciones pendientes de autorizar
    /// </summary>
    /// <param name="filtro"></param>
    /// <returns></returns>
    public List<Consignacion> ConsignacionesPendientesDeAgregarEnSIL4View(NuevoCupoViewModelDTO filtro = null)
    {
      List<Consignacion> listOfConsignaciones = new List<Consignacion>();
      IList<CupoInStopAndNotSIL> consignacionesStopNoCorre = new List<CupoInStopAndNotSIL>();
      consignacionesStopNoCorre = ConsignacionesPendientesDeAgregar(this.TurnosPendientesDeAgregar(filtro));
      listOfConsignaciones = consignacionesStopNoCorre.Select(x => GetConsignacion(x.GetCuitConsignacion())).ToList();
      return listOfConsignaciones;
    }

    #endregion

    #region MetodosComplementarios

    /// <summary>
    /// retorna el numero de cupos para el dia tomado como parametro
    /// </summary>
    /// <param name="fecha"></param>
    /// <param name="listaElementos"></param>
    /// <returns></returns>
    private int numeroDeCuposPorDia(DateTime fecha, IList<CupoInStopAndNotSIL> listaElementos)
    {
      var elementos = listaElementos.Where(x =>
                                          x.Fecha == fecha)
                                          .ToList();
      return elementos.Count();

    }

    /// <summary>
    /// Dado un NuevoCupoViewModelDTO pasado como parametro y una consignacion (que contiene solo cuits).
    /// Setea los nombres de cada cuit de la consignacion en el objeto NuevoCupoViewModelDTO 
    /// </summary>
    /// <param name="ncvm"></param>
    /// <param name="cuitconsignacion"></param>
    /// <returns></returns>
    private NuevoCupoViewModelDTO SetNombresDeLaConsignacion(NuevoCupoViewModelDTO ncvm, Consignacion cuitconsignacion)
    {
      Consignacion consigna = cuitconsignacion;
      consigna.Nomsolicitante = this.ObtenerNombre(cuitconsignacion.Cuitsolicitante, this.session);
      consigna.Nomintermediario = this.ObtenerNombre(cuitconsignacion.Cuitintermediario, this.session);
      consigna.Nomrtecomercial = this.ObtenerNombre(cuitconsignacion.Cuitrtecomercial, this.session);
      consigna.Nomcorrcomp = this.ObtenerNombreCorredor(cuitconsignacion.Cuitcorrcomp, this.session);
      consigna.Nommat = this.ObtenerNombre(cuitconsignacion.Cuitmat, this.session);
      consigna.Nomcorrvta = this.ObtenerNombreCorredor(cuitconsignacion.Cuitcorrvta, this.session);
      consigna.Cuitrteent = this.ObtenerNombre(cuitconsignacion.Cuitrteent, this.session);
      consigna.Nomdestinatario = this.ObtenerNombre(cuitconsignacion.Cuitdestinatario, this.session);
      ncvm.SetConsignacion(consigna);
      return ncvm;
    }

    /// <summary>
    /// Dado un objeto consignacion (que contiene solo cuits). Retorna un objeto Consignacion
    /// con los nombres para cada cuit que la compone.
    /// </summary>
    /// <param name="cuitconsignacion"></param>
    /// <returns></returns>
    private Consignacion GetConsignacion(Consignacion cuitconsignacion)
    {
      Consignacion consigna = cuitconsignacion;
      consigna.Nomsolicitante = this.ObtenerNombre(cuitconsignacion.Cuitsolicitante, this.session);
      consigna.Nomintermediario = this.ObtenerNombre(cuitconsignacion.Cuitintermediario, this.session);
      consigna.Nomrtecomercial = this.ObtenerNombre(cuitconsignacion.Cuitrtecomercial, this.session);
      consigna.Nomcorrcomp = this.ObtenerNombreCorredor(cuitconsignacion.Cuitcorrcomp, this.session);
      consigna.Nommat = this.ObtenerNombre(cuitconsignacion.Cuitmat, this.session);
      consigna.Nomcorrvta = this.ObtenerNombreCorredor(cuitconsignacion.Cuitcorrvta, this.session);
      consigna.Nomrteent = this.ObtenerNombre(cuitconsignacion.Cuitrteent, this.session);
      consigna.Nomdestinatario = this.ObtenerNombre(cuitconsignacion.Cuitdestinatario, this.session);
      return consigna;
    }

    /// <summary>
    /// obtenemos el nombre relacionado al cuit pasado como parametro
    /// esta consulta es sobre la cuposcuit
    /// </summary>
    /// <param name="nroCuit"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    private string ObtenerNombre(string nroCuit, ISession session)
    {
      string cuitAca = ConfigurationManager.AppSettings["cuitCorredorComprador"].Replace("-", "");
      string cuentaAcaConsignacion = ConfigurationManager.AppSettings["cuentaACAConsignacionPorDefecto"].Replace("-", "");
      if (!string.IsNullOrEmpty(nroCuit))
      {
        List<CuposCuit> cuits = (List<CuposCuit>)this.cuitsStore.FindByCuit(nroCuit, session);
        if (cuits.Count > 0)
        {
          CuposCuit cuit = new CuposCuit();
          if (nroCuit == cuitAca)
          {
            cuit = cuits.Where(x => x.Cuenta.ToString() == cuentaAcaConsignacion).FirstOrDefault();
          }
          else
          {
            cuit = cuits.FirstOrDefault();
          }
          return cuit.Nombre;
        }
      }
      return null;
    }

    private string ObtenerNombreCorredor(string nroCuit, ISession session)
    {
      string cuitAca = ConfigurationManager.AppSettings["cuitCorredorComprador"].Replace("-", "");
      if (!string.IsNullOrEmpty(nroCuit))
      {
        List<CuposCuit> cuits = (List<CuposCuit>)this.cuitsStore.FindByCuit(nroCuit, session);
        if (cuits.Count > 0)
        {
          CuposCuit cuit = new CuposCuit();
          if (nroCuit == cuitAca)
          {
            cuit = cuits.Where(x => x.Cuenta.ToString() == cuitAca).FirstOrDefault();
          }
          else
          {
            cuit = cuits.FirstOrDefault();
          }
          return cuit.Nombre;
        }
      }
      return null;
    }
    /// <summary>
    /// obtenemos el cuposcuit relacionado al cuit pasado como parametro
    /// esta consulta es sobre la cuposcuit
    /// </summary>
    /// <param name="nroCuit"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    private CuposCuit ObtenerCuposCuit(string nroCuit, ISession session)
    {
      if (!string.IsNullOrEmpty(nroCuit))
      {
        return this.cuitsStore.FindByCuit(nroCuit, session).FirstOrDefault();
      }
      return null;
    }

    /// <summary>
    /// obetenmos el nro de cuenta relacionado al idTerminal (de STOP) del puerto que pasamos como parametro
    /// </summary>
    /// <param name="nroCuit"></param>
    /// <returns></returns>
    private CuposPuertoSTOP ObtenerPuertoPorIdTerminal(long idTerminal, ISession session)
    {
      if (idTerminal != 0)
      {
        CuposPuertoSTOPStore CPStore = new CuposPuertoSTOPStore();
        CuposPuertoSTOP cp = CPStore.FindByNroPuerto(idTerminal, session);
        return cp;
      }
      return null;
    }

    /// <summary>
    /// Obtenemos el puerto por el nro de cuenta. consulta sobre la vista cuposPuerto
    /// </summary>
    /// <param name="cuenta"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    private Puerto ObtenerPuertoPorCuenta(string cuenta, ISession session)
    {
      if (!string.IsNullOrEmpty(cuenta))
      {
        PuertoStore puertoStore = new PuertoStore();
        Puerto puerto = puertoStore.FindByCuenta(Int64.Parse(cuenta), session).FirstOrDefault();
        return puerto;
      }
      return null;
    }

    /// <summary>
    /// Obtenemos el Grano proveniente de STOP. es decir los granos que se encuentran en la STOPNOCORRE
    /// </summary>
    /// <param name="nroGrano"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    private CuposGranoSTOP ObtenerGranoSTOP(int nroGrano, ISession session)
    {
      if (nroGrano != 0)
      {
        CuposGranoSTOPStore puertoStore = new CuposGranoSTOPStore();
        CuposGranoSTOP grano = puertoStore.FindByNroGrano(nroGrano, session);
        return grano;
      }
      return null;
    }

    /// <summary>
    /// obtenemos la cuenta relacionada al comprador para el cuit pasado como parametros
    /// esta consulta es sobre la cuposcomprador
    /// </summary>
    /// <param name="nroCuit"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    private Comprador ObtenerCompradorPorCuit(string nroCuit, ISession session)
    {
      string cuitAca = ConfigurationManager.AppSettings["cuitCorredorComprador"].Replace("-", "");
      string cuentaAcaCompradorPorDefecto = ConfigurationManager.AppSettings["cuentaACACompradorPorDefecto"].Replace("-", "");
      if (!string.IsNullOrEmpty(nroCuit))
      {
        CompradorStore compradorstore = new CompradorStore();
        IList<Comprador> compradores = compradorstore.FindByCuit(nroCuit, session);
        if (compradores.Count > 0)
        {
          if (nroCuit == cuitAca)
          {
            return compradores.Where(x => x.Cuenta.ToString() == cuentaAcaCompradorPorDefecto).FirstOrDefault();
          }
          return compradores.FirstOrDefault();
        }
      }
      return null;
    }

    /// <summary>
    /// obtenemos la cuenta relacionada al comprador para la cuenta pasada como parametros
    /// esta consulta es sobre la cuposcomprador
    /// </summary>
    /// <param name="cuenta"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    private Comprador ObtenerCompradorPorCuenta(string cuenta, ISession session)
    {
      if (!string.IsNullOrEmpty(cuenta))
      {
        CompradorStore compradorstore = new CompradorStore();
        Comprador comprador = compradorstore.FindByCuenta(Int64.Parse(cuenta), session);
        return comprador;
      }
      return null;
    }
    /// <summary>
    ///  obetenemos la cuenta relacionada al vendedor para el cuit pasado como parametros
    /// esta consulta es sobre la cuposvendedor
    /// </summary>
    /// <param name="nroCuit"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    private Vendedor ObtenerVendedorPorCuit(string nroCuit, ISession session)
    {
      string cuitAca = ConfigurationManager.AppSettings["cuitCorredorComprador"].Replace("-", "");
      string cuentaAcaVendedorPorDefecto = ConfigurationManager.AppSettings["cuentaACAVendedorPorDefecto"].Replace("-", "");
      if (!string.IsNullOrEmpty(nroCuit))
      {
        VendedorStore vendedorStore = new VendedorStore();
        IList<Vendedor> vendedores = vendedorStore.FindVendedorByCuit(nroCuit, session);
        if (vendedores.Count > 0)
        {
          if (nroCuit == cuitAca)
          {
            return vendedores.Where(x => x.Cuenta.ToString() == cuentaAcaVendedorPorDefecto).FirstOrDefault();
          }
          return vendedores.FirstOrDefault();
        }
      }
      return null;
    }

    /// <summary>
    ///  obetenemos la cuenta relacionada al vendedor para la cuenta pasada como parametros
    /// esta consulta es sobre la cuposvendedor
    /// </summary>
    /// <param name="cuenta"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    private Vendedor ObtenerVendedorPorCuenta(string cuenta, ISession session)
    {
      if (!string.IsNullOrEmpty(cuenta))
      {
        VendedorStore vendedorStore = new VendedorStore();
        Vendedor vendedor = vendedorStore.FindByNroCuenta(Int64.Parse(cuenta), session);
        return vendedor;
      }
      return null;
    }

    /// <summary>
    /// Dado el Id del Puerto que viene de STOP obtenemos el centro de la tabla cupospuertocentro (relacion Centro-Puerto)
    /// definida y actualizada por el usuario
    /// </summary>
    /// <param name="idTerminal"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    private CuposCentroPorPuerto ObtenerCentroPorIdTerminal(long idTerminal, ISession session)
    {
      if (idTerminal != 0)
      {
        CentroPorPuertoStore CentroPorPuertST = new CentroPorPuertoStore();
        IList<CuposCentroPorPuerto> Centro = CentroPorPuertST.FindByPuerto(idTerminal, session);
        if (Centro.Count > 0)
        {
          return Centro.FirstOrDefault();
        }
      }
      return null;
    }
    /// <summary>
    /// Obtenemos el nombre del centro filtrando por codigo
    /// </summary>
    /// <param name="codigoCentro"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    private string ObtenerNombreDeLCentro(string codigoCentro, ISession session)
    {
      if (!string.IsNullOrEmpty(codigoCentro))
      {
        CentroStore CentroST = new CentroStore();
        IList<Centro> Centro = CentroST.FindByCentro(codigoCentro, session);
        if (Centro.Count > 0)
        {
          return Centro.FirstOrDefault().Nombre;
        }
      }
      return null;
    }
    private NuevoCupoViewModel QuitarAlfasDelNuevoCupoViewModel(NuevoCupoViewModel ncvm, NuevoCupoViewModelDTO ncvmDTO, List<string> alfas)
    {
      DateTime fechaDeAlfa;
      foreach (string alfa in alfas)
      {
        fechaDeAlfa = (DateTime)ncvmDTO.GetFechaDeAlfa(alfa);
        ncvm.EliminarAlfa(alfa, fechaDeAlfa);
      }
      return ncvm;
    }

    private void QuitarAlfasDeStopNoCorre(NuevoCupoViewModelDTO ncvmDTO, List<string> alfas, ITransaction tx)
    {
      DateTime fechaDeAlfa;
      CupoInStopAndNotSIL turno;
      CupoInStopAndNotSILStore stonNoCo = new CupoInStopAndNotSILStore();
      RelacionGranoSILGranoSTOPStore relacSTO = new RelacionGranoSILGranoSTOPStore();
      RelacionPuertoSILPuertoSTOPStore relacPuertoSTO = new RelacionPuertoSILPuertoSTOPStore();
      /*tendre que recibir el granoSTO y granoSIL esto ret uno solo*/

      foreach (string alfa in alfas)
      {
        fechaDeAlfa = (DateTime)ncvmDTO.GetFechaDeAlfa(alfa);
        turno = new CupoInStopAndNotSIL();
        turno.Fecha = fechaDeAlfa;
        turno.Nrocupo = alfa;
        turno.Grano = (int)relacSTO.FindRelacionCompletaByNroGranoSIL(ncvmDTO.Producto, session).FirstOrDefault().NroGranoSTOP;
        int idPuerto = (int)relacPuertoSTO.FindCompleteRelationByPuertoSIL(Convert.ToInt64(ncvmDTO.Puerto), session).FirstOrDefault().NroPuertoSTOP;
        turno.Idterminal = idPuerto;
        turno.Cuitdestinatario = !string.IsNullOrEmpty(ncvmDTO.Cuitdestinatario) ? ncvmDTO.Cuitdestinatario.Replace("-", string.Empty) : "";
        //if (!string.IsNullOrEmpty(ncvmDTO.Cuitrtecomercial))
        //{
        //    turno.Cuitremcomercial = ncvmDTO.Cuitrtecomercial.Replace("-", string.Empty);
        //}
        /*en caso de que el CentroAnterior<>CentroPorDefecto -> se cambio el centro y se solicitó la autorizacion de turnos*/
        turno.Centro = (ncvmDTO.Centro != ncvmDTO.CentroAnterior && ncvmDTO.CentroAnterior != this.ObtenerCentroPorIdTerminal(turno.Idterminal, session).CodigoCentro) ? ncvmDTO.CentroAnterior : ncvmDTO.Centro;
        //turno.Centro = (ncvmDTO.Centro != ncvmDTO.CentroAnterior)? ncvmDTO.Centro: null;
        turno = stonNoCo.FindByFiltro(turno, session).FirstOrDefault(); ;
        if (turno != null) stonNoCo.Delete(turno, session, tx);
      }
    }

    #endregion

    #region MetodosDeAlta 
    /// <summary>
    /// dado un cupo (alfa) que esta esta en STOP lo busca en la stopnocorre
    /// y lo agrega en la cuposcorre. si al alta dio error retorna el NuevoCupoViewModel, sino null
    /// </summary>
    /// <param name="alfa"></param>
    /// <returns></returns>
    public bool AgregarCupo(string alfa)
    {
      if (alfa != null && alfa.Length > 0)
      {
        try
        {
          CupoInStopAndNotSILStore cuposSTOPStore = new CupoInStopAndNotSILStore();
          CupoInStopAndNotSIL CupoInSTOP = cuposSTOPStore.FindByAlfa(alfa, session);
          NuevoCupoViewModelDTO conjuntoDeCupos = new NuevoCupoViewModelDTO();
          RelacionGranoSILGranoSTOPStore relacionStore = new RelacionGranoSILGranoSTOPStore();
          RelacionGranoSILGranoSTOPCompleta relac = relacionStore.FindRelacionCompletaByNroGranoSTOP(CupoInSTOP.Grano, session).Where(x => x.ValorPorDefecto == 1).FirstOrDefault();
          conjuntoDeCupos.Producto = (int)relac.NroGranoSIL;
          RelacionPuertoSILPuertoSTOPStore realPuerto = new RelacionPuertoSILPuertoSTOPStore();
          RelacionPuertoSILPuertoSTOP relacionResult = realPuerto.FindCompleteRelationByPuertoSTOP(CupoInSTOP.Idterminal, session).Where(x => x.ValorPorDefecto == 1).FirstOrDefault();
          conjuntoDeCupos.Puerto = (relacionResult != null) ? relacionResult.NroPuertoSIL.ToString() : "";
          Comprador comprador = this.ObtenerCompradorPorCuit(CupoInSTOP.Cuitdestinatario, session);
          conjuntoDeCupos.Compcta = (comprador != null) ? comprador.Cuenta.ToString() : "";
          Vendedor vendedor = this.ObtenerVendedorPorCuit(CupoInSTOP.Cuitremcomercial, session);
          conjuntoDeCupos.Vendcta = (vendedor != null) ? vendedor.Cuenta.ToString() : "";
          CuposCentroPorPuerto centro = this.ObtenerCentroPorIdTerminal(CupoInSTOP.Idterminal, session);
          conjuntoDeCupos.Centro = (centro != null) ? centro.CodigoCentro : "";
          conjuntoDeCupos.VendcyoBoolValue = false;   //DEFINIR: siempre son cupos normales?
          conjuntoDeCupos.Cuitdestinatario = CupoInSTOP.Cuitdestinatario;
          conjuntoDeCupos.Nomdestinatario = this.ObtenerNombre(CupoInSTOP.Cuitdestinatario, session);
          conjuntoDeCupos.Cuitsolicitante = CupoInSTOP.Cuitorigen;//ver caso de valvacchia
          conjuntoDeCupos.Nomsolicitante = this.ObtenerNombre(CupoInSTOP.Cuitorigen, session);
          conjuntoDeCupos.Cuitintermediario = CupoInSTOP.Cuitintermediario;
          conjuntoDeCupos.Nomintermediario = this.ObtenerNombre(CupoInSTOP.Cuitintermediario, session);
          conjuntoDeCupos.Cuitmat = CupoInSTOP.Cuitmercadoatermino;
          conjuntoDeCupos.Nommat = this.ObtenerNombre(CupoInSTOP.Cuitmercadoatermino, session);
          conjuntoDeCupos.Cuitrtecomercial = CupoInSTOP.Cuitremcomercial;
          conjuntoDeCupos.Nomrtecomercial = this.ObtenerNombre(CupoInSTOP.Cuitremcomercial, session);
          conjuntoDeCupos.Cuitcorrvta = CupoInSTOP.Cuitcorredorv;
          conjuntoDeCupos.Nomcorrvta = this.ObtenerNombre(CupoInSTOP.Cuitcorredorv, session);
          conjuntoDeCupos.Cuitcorrcomp = CupoInSTOP.Cuitcorredorc;
          conjuntoDeCupos.Nomcorrcomp = this.ObtenerNombre(CupoInSTOP.Cuitcorredorc, session);
          conjuntoDeCupos.Cuitrteent = CupoInSTOP.Cuitrepresentanteentregador;
          conjuntoDeCupos.Nomrteent = this.ObtenerNombre(CupoInSTOP.Cuitrepresentanteentregador, session);
          conjuntoDeCupos.DetalleCupoCNRT = CupoInSTOP.Detallecupo;
          conjuntoDeCupos.EstadoCupoCNRT = 1; //DEFINIR: cristian deberia agregar el campo con el valor
          /*creamos los alfanumericos*/
          CuposPorDiaDTO cuposxDia = new CuposPorDiaDTO();
          cuposxDia.Fecha = CupoInSTOP.Fecha;
          cuposxDia.Turnos = new List<string>();
          cuposxDia.Turnos.Add(CupoInSTOP.Nrocupo);
          conjuntoDeCupos.CodigosDias = new List<CuposPorDiaDTO>();
          conjuntoDeCupos.CodigosDias.Add(cuposxDia);
          return this.AgregarCupoEnSIL(conjuntoDeCupos);
        }
        catch (Exception e)
        {
          throw e;
        }
      }
      return false;
    }

    /// <summary>
    /// Funcion que agrega los cupos pasados como parametros, en formato NuevoCupoViewModelDTO, a la SIL (cuposcorre)
    /// </summary>
    /// <param name="cupos"></param>
    /// <returns></returns>
    public bool AgregarCupoEnSIL(NuevoCupoViewModelDTO cupo)
    {
      NuevoCupoViewModel NCVM;
      if (cupo != null)
      {
        NCVM = new NuevoCupoViewModel(cupo);
        try
        {
          using (ITransaction tx = session.BeginTransaction())
          {
            NCVM.Save(session, tx);
            this.QuitarAlfasDeStopNoCorre(cupo, NCVM.GetAlfas(), tx);
            tx.Commit();
          }
        }
        catch (CodigosAlfanumericoDuplicadosException excep)
        {
          if (cupo.EmparejoTurnos)
          {
            string[] mensage = excep.Message.Split(new char[] { '[', ']' });
            List<string> alfas = mensage[1].Split('|').ToList<string>();
            NCVM = this.QuitarAlfasDelNuevoCupoViewModel(NCVM, cupo, alfas);
            using (ITransaction tx = session.BeginTransaction())
            {
              if (NCVM.CodigosDias.Count() > 0)
              {
                NCVM.Save(session, tx);
              }
              this.QuitarAlfasDeStopNoCorre(cupo, cupo.GetAlfas(), tx);
              tx.Commit();
            }
          }
          else
          {
            throw (excep);
          }
        }
        catch (Exception e)
        {
          throw e;
        }
        return true;
      }
      return false;
    }

    public bool CambiarCentro(NuevoCupoViewModelDTO cupo)
    {
      if (cupo != null)
      {
        if (cupo.Centro != cupo.CentroAnterior)
        {
          List<string> alfas = cupo.GetAlfas();
          CupoInStopAndNotSIL DatosFiltro = new CupoInStopAndNotSIL();
          using (ITransaction tx = session.BeginTransaction())
          {
            try
            {
              CupoInStopAndNotSILStore store = new CupoInStopAndNotSILStore();
              DatosFiltro.SetCuitConsignacion(cupo.GetConsignacion());
              RelacionGranoSILGranoSTOPStore relacSTO = new RelacionGranoSILGranoSTOPStore();
              DatosFiltro.Grano = (int)relacSTO.FindRelacionCompletaByNroGranoSIL(cupo.Producto, session).FirstOrDefault().NroGranoSTOP;
              if (!string.IsNullOrEmpty(cupo.Puerto))
              {
                Puerto puerto = this.ObtenerPuertoPorCuenta(cupo.Puerto, this.session);
                DatosFiltro.Idterminal = (puerto != null) ? Int32.Parse(puerto.IdTerminal.Trim()) : 0; //deberia ser puerto.IDTERMINAL (LONG)
              }
              if (!string.IsNullOrEmpty(cupo.Vendcta))
              {
                Vendedor vendedor = this.ObtenerVendedorPorCuenta(cupo.Vendcta, this.session);
                DatosFiltro.Cuitremcomercial = (vendedor != null) ? (vendedor.Cuit).Replace("-", "") : "";
              }
              if (!string.IsNullOrEmpty(cupo.Compcta))
              {
                Comprador comprador = this.ObtenerCompradorPorCuenta(cupo.Compcta, this.session);
                DatosFiltro.Cuitdestinatario = (comprador != null) ? (comprador.Cuit).Replace("-", "") : "";
              }
              if (!string.IsNullOrEmpty(cupo.CentroAnterior))
              {
                DatosFiltro.Centro = cupo.CentroAnterior;
              }
              List<CupoInStopAndNotSIL> salida = (List<CupoInStopAndNotSIL>)store.FindByAlfasWithACACorredorCORDestinatarioORCorredorV(DatosFiltro, alfas, this.session);
              foreach (CupoInStopAndNotSIL a in salida)
              {
                a.Centro = cupo.Centro;
                store.Update(a.Id, a, this.session);
              }
              tx.Commit();
              return true;
            }
            catch (Exception e)
            {
              tx.Rollback();
              throw e;
            }
          }
        }
      }
      return false;
    }
    #endregion
  }
}