using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Filtro;

namespace ResourceServer.Models
{
    public class DistribuirCupoViewModel
    {
        public DistribuirCupoViewModel() { }
        public DistribuirCupoViewModel(string id, string cyo) 
        { 
            Id = id;
            if (EntregaHasta == null || EntregaHasta == default(DateTime)) EntregaHasta = DateTime.Now.AddDays(1);
            Inicializar(cyo);
        }
        public void Inicializar(string cyo)
        {
            Cyo = cyo;
            MotivosDecrementoCupos = new List<DTabla>();
            DistribucionesDisponibles = new DistribucionDisponible(new List<VistaCuposDistribuidos>());
            if (CosechaDesde == null) CosechaDesde = "0";
            if (CosechaHasta == null) CosechaHasta = "0";
            if (CentroSeleccionado == null) CentroSeleccionado = "ROS";
            BuscarYMostrarDatosCabecera();
        }
        private IVistaCuposDistribuidosStore StoreDistribuciones { get; set; }
        private ICentroStore StoreCentro { get; set; }
        private IGranoStore StoreGrano { get; set; }
        private ICompradorStore StoreComprador { get; set; }
        private IPuertoStore StorePuerto { get; set; }
        private ICuposStore StoreCupos { get; set; }
        public string Id { get; set; }
        public string Producto { get; set; }
        public int CodigoProducto { get; set; }
        public string Comprador { get; set; }
        public long CuentaComprador { get; set; }
        public long CuentaVendedor { get; set; }
        public string Puerto { get; set; }
        public long CuentaPuerto { get; set; }
        [Display(Name = "Entrega Hasta")]
        public DateTime? EntregaHasta { get; set; }
        [Display(Name = "Entrega Desde")]
        public DateTime? EntregaDesde { get; set; }
        [Display(Name = "Cosecha Desde")]
        public string CosechaDesde { get; set; }
        [Display(Name = "Cosecha Hasta")]
        public string CosechaHasta { get; set; }
        public IEnumerable<SelectListItem> Centro { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Centro")]
        public string CentroSeleccionado { get; set; }
        public IList<Consignacion> Consignaciones;
        public IList<Counter<Cupos>> ConsignacionesAgrupadosPorFecha;
        public string Buscador { get; set; }
        public DistribucionDisponible DistribucionesDisponibles { get; set; }
        public string Destino { get; set; }
        public IEnumerable<DTabla> MotivosDecrementoCupos { get; set; }
        private string Cyo { get; set; }
        private FiltroDistribucion filtro { get; set; }
        public IList<ICuposAgrupadosPdf> CuposAgrupados { get; set; }

        public void BuscarYMostrarDatosCabecera()
        {
            ObtenerIds(Id);
            BuscarYMostrarProducto();
            BuscarYMostrarComprador();
            BuscarYMostrarPuerto();
            BuscarYMostrarCentros();
            BuscarYMostrarConsignaciones(); /*CA: ver que hacer con caratula y CC. es consigna o no. lo manejamos como observa?*/
            ObtenerUltimaObservacionPorDia();
            ObtenerUltimoContactoComercialPorDia();
            BuscarYMostrarCuposAgrupadosInforamdos();
        }
        public void BuscarYMostrarDatos()
        {
            BuscarYMostrarMotivosDecremento();
            ObtenerUltimaObservacionPorDia();
            BuscarYMostrarTotalesDias();
        }
        public void BuscarYMostrarMotivosDecremento()
        {
            IDTablaStore store = new DTablaStore();
            MotivosDecrementoCupos = store.findClaveValorByEntidadAndOrden("MOTBAJA", "MB02");
        }
        public void BuscarYMostrarCentros()
        {
            StoreCentro = new CentroStore();
            Centro = StoreCentro.FindAll().Select(
                        c => new SelectListItem
                        {
                            Value = c.Id,
                            Text = c.Nombre
                        }
                    );
        }
        public void BuscarYMostrarConsignaciones()
        {
            IConsignacionStore ConsignacionStore = new ConsignacionStore();
            StoreCupos = new CuposStore();
            if (EsCyo())
            {
                Consignaciones = ConsignacionStore.FindByGranoAndCompAndVendAndPuertoAndVendcyoNotInStatus(CodigoProducto, CuentaComprador, CuentaVendedor, CuentaPuerto, new long[] { CuentaComprador }, new int[] { 3 }).ToList();
            }
            else
            {
                Consignaciones = ConsignacionStore.FindByGranoAndCompAndVendAndPuertoAndVendcyoNotInStatus(CodigoProducto, CuentaComprador, CuentaVendedor, CuentaPuerto, new long[] { 0, CuentaVendedor }, new int[] { 3 }).ToList();
            }
        }
        public void ObtenerUltimaObservacionPorDia()
        {
            foreach (var Consignacion in Consignaciones)
            {
                Consignacion.SetObservacion(StoreCupos.FindLastObservacionByConsignacion(CuentaComprador, CuentaVendedor, CodigoProducto, CuentaPuerto, Consignacion));
            }
        }
        public void ObtenerUltimoContactoComercialPorDia()
        {
          foreach (var Consignacion in Consignaciones)
          {
            Consignacion.SetContactoComercial(StoreCupos.FindLastContactoComercialByConsignacion(CuentaComprador, CuentaVendedor, CodigoProducto, CuentaPuerto, Consignacion));
          }
        }
    
        public void BuscarYMostrarProducto()
        {
            StoreGrano = new GranoStore();
            Grano ProductoSeleccionado = StoreGrano.FindById(CodigoProducto.ToString());
            Producto = ProductoSeleccionado != null ? ProductoSeleccionado.Nombre : "";
        }
        public void BuscarYMostrarComprador()
        {
            StoreComprador = new CompradorStore();
            Comprador CompradorSeleccionado = StoreComprador.FindById(CuentaComprador);
            Comprador = CompradorSeleccionado != null ? CompradorSeleccionado.Nombre : "";
        }
        public void BuscarYMostrarPuerto()
        {
            StorePuerto = new PuertoStore();
            Puerto PuertoSeleccionado = StorePuerto.FindById(CuentaPuerto);
            Puerto = PuertoSeleccionado != null ? PuertoSeleccionado.Nombre : "";
        }
        public void ObtenerIds(string Id)
        {
            if (Id != null)
            {
                var arrayIds = Id.Split('-');
                CuentaComprador = Int64.Parse(arrayIds[0]);
                CuentaVendedor = Int64.Parse(arrayIds[1]);
                CuentaPuerto = Int64.Parse(arrayIds[2]);
                CodigoProducto = Int32.Parse(arrayIds[3]);
            }
            else
            {
                CuentaComprador = 0;
                CuentaVendedor = 0;
                CuentaPuerto = 0;
                CodigoProducto = 0;
            }
        }
        private void BuscarYMostrarTotalesDias(){
            ServicioDistribuir servicio_distribuir = new ServicioDistribuir();
            DistribucionesDisponibles = servicio_distribuir.SetTotalesDias(CuentaComprador, CuentaVendedor, CodigoProducto, CuentaPuerto, DistribucionesDisponibles, Cyo);
        }
        public VistaCuposDistribuidos GetDistribucionPrimeraConsignacion()
        {
            VistaCuposDistribuidos result = new VistaCuposDistribuidos();
            result.Compcta = CuentaComprador;
            result.Vendcta = CuentaVendedor;
            result.Ctadestino = CuentaPuerto;
            result.Codproducto = CodigoProducto;
            result.Codcentro = CentroSeleccionado;
            Consignacion Consignacion;
            if (Consignaciones.Count > 0)
            {
                Consignacion = Consignaciones.ElementAt(0);
                result.SetConsignacion(Consignacion);
            }
            return result;
        }
        public void ObtenerFiltro()
        {
            if (filtro == null)
            {
                ServicioFiltro servicio_filtro = new ServicioFiltro(this.CodigoProducto, this.CuentaPuerto, this.CuentaComprador);
                filtro = servicio_filtro.BuscarFiltro(this.CuentaVendedor);
            }
            if (filtro != null)
            {
                EntregaDesde = filtro.EntregaDesde;
                EntregaHasta = filtro.EntregaHasta;
                CosechaDesde = filtro.CosechaDesde;
                CosechaHasta = filtro.CosechaHasta;
                CentroSeleccionado = filtro.Centro;
            }
        }
        public void BuscarYMostrarCuposAgrupadosInforamdos()
        {
            ICuposAgrupadosPdfStore Store = new CuposAgrupadosPdfStore();
            if (EsCyo())
            {
                CuposAgrupados = Store.FindByCompradorAndPuertoAndGranoCyo(CuentaComprador, CuentaPuerto, CodigoProducto);
            }
            else
            {
                CuposAgrupados = Store.FindByCompradorAndPuertoAndGrano(CuentaComprador, CuentaPuerto, CodigoProducto);
            }
        }
        private bool EsCyo()
        {
            return (!string.IsNullOrEmpty(Cyo) && Cyo.ToUpper() == "TRUE");
        }
    }

    public class BusquedaDistribucionViewModel
    {
        public IdViewModel Id { get; set; }
        public DistribuirCupoViewModel Modelo { get; set; }
    }

    public class BusquedaMultiplesConsignacionesViewModel
    {
        public long compcta { get; set; }
        public long vendcta { get; set; }
        public long ctadestino { get; set; }
        public string codcentro { get; set; }
        public int grano { get; set; }
        public DateTime? fechaent { get; set; }
  }

  public class BusquedaContratosVendedorViewModel
  {
    public long Comprador { get; set; }
    public long Vendedor { get; set; }
    public long Destino { get; set; }
    public TipoDestino TipoDestino { get; set; }
    //public string CodigoCentro { get; set; }
    public int Grano { get; set; }
    public DateTime FechaDesde { get; set; }
    public DateTime FechaHasta { get; set; }
  }

  public class BusquedaContratosViewModel
    {
        private string _cyo ="FALSE";
        public VistaCuposDistribuidos datosContrato { get; set; }
        public long CuentaPuerto { get; set; }
        public DateTime? fechaDesde { get; set; }
        public DateTime? fechaHasta { get; set; }
        public string cosechaDesde { get; set; }
        public string cosechaHasta { get; set; }
        public Consignacion ConsignacionSeleccionada { get; set; }
        public string Cyo { get { return _cyo; } set { _cyo = value; } }
    }
}