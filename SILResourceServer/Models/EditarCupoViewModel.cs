using ResourceServer.Models.Configuracion;
using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResourceServer.Models
{
    public class EditarCupoViewModel
    {
        public EditarCupoViewModel()
        {
            Inicializar();
        }
        public EditarCupoViewModel(string id)
        {
            Inicializar();
            this.Id = id;
        }
        public void Inicializar(){
            CuposStore = new CuposStore();
            Store = new EncabezadoStore();
            ListaConsignaciones = new List<SelectListItem>();
            ListaAlfanumerico = new List<NuevoAlfanumerico>();
            for (int Dia = 0; Dia <= 20; Dia++) {
                ListaAlfanumerico.Add(new NuevoAlfanumerico(Dia));
            }
        }
        public string Id { get; set; }
        private ICuposStore CuposStore { get; set; }
        private IEncabezadoStore Store { get; set; }
        public IList<Encabezado> Encabezados { get; set; }
        //Si existe mas de un encabezado para el mismo comprador, vendedor, grano, puerto se pide la seleccion de el 
        //y se setea en EncabezadoSeleccionado.
        public virtual IEnumerable<SelectListItem> ListaConsignaciones { get; set; }
        private IList<Cupos> CuerposConsignacion { get; set; }
        public string Consignacion { get; set; }
        public Consignacion ConsignacionSeleccionada { get; set; }
        public IEnumerable<Consignacion> Consignaciones;
        private IList<Cuerpo> CuerposConConsignacionSeleccionada { get; set; }
        public string Producto { get; set; }
        public string Comprador { get; set; }
        public string Vendedor { get; set; }
        public string Puerto { get; set; }
        public int CodigoProducto { get; set; }
        public long CuentaComprador { get; set; }
        public long CuentaVendedor { get; set; }
        public long CuentaPuerto { get; set; }
        public bool CyO { get; set; }
        public IList<NuevoAlfanumerico> ListaAlfanumerico { get; set; }
        public IEnumerable<DTabla> MotivosAnulacion { get; set; }
        [Display(Name="Motivo")]
        public virtual string MotivoAnulacionSeleccionado { get; set; }
        public string Error { get; set; }
        public void BuscarYMostrarDatos(string Centro, string CentroDistribucion)
        {
            ObtenerIds(Id);
            ObtenerYMostrarDatosConsignacion(Centro, CentroDistribucion);
            MostrarDatosCabecera();
            MostrarCodigosAlfanumericos(new FiltroCentro { CentroOrigen = Centro, CentroDistribucion = CentroDistribucion });
            ObtenerYMostrarMotivosAnulacion();
        }
        public void BuscarYMostrarDatosCyO(string Centro, string CentroDistribucion)
        {
            ObtenerIds(Id);
            ObtenerYMostrarDatosConsignacionCyo(Centro, CentroDistribucion);
            MostrarDatosCabecera();
            MostrarCodigosAlfanumericos(new FiltroCentro { CentroOrigen = Centro, CentroDistribucion = CentroDistribucion });
            ObtenerYMostrarMotivosAnulacion();
        }
        private void ObtenerYMostrarMotivosAnulacion()
        {
            IDTablaStore motivosStore = new DTablaStore();
            MotivosAnulacion = motivosStore.findClaveValorByEntidadAndOrden("MOTBAJA", "MB02");
        }
        private void ObtenerEncabezadosCyO()
        {
            Encabezados = Store
                .FindByGranoAndCompradorAndVendedorAndPuertoCyO(CodigoProducto, CuentaComprador, CuentaVendedor, CuentaPuerto)
                .ToList();
        }

        private void ObtenerYMostrarDatosConsignacion(string Centro, string CentroDistribucion)
        {
            IConsignacionStore ConsignacionStore = new ConsignacionStore();
            Consignaciones = ConsignacionStore.FindByGranoAndCompAndVendAndPuertoAndVendcyoAndCentroAndCentrodist(
                CodigoProducto, 
                CuentaComprador, 
                CuentaVendedor, 
                CuentaPuerto,
                new FiltroCentro { CentroOrigen = Centro, CentroDistribucion = CentroDistribucion }, 
                new long[] { 0, CuentaVendedor }
            );
        }

        private void ObtenerYMostrarDatosConsignacionCyo(string Centro, string CentroDistribucion)
        {
            IConsignacionStore ConsignacionStore = new ConsignacionStore();
            Consignaciones = ConsignacionStore.FindByGranoAndCompAndVendAndPuertoAndVendcyoAndCentroAndCentrodist(
                CodigoProducto, 
                CuentaComprador, 
                CuentaVendedor,
                CuentaPuerto,
                new FiltroCentro { CentroOrigen = Centro, CentroDistribucion = CentroDistribucion }, 
                new long[] { CuentaComprador }
            );
        }

        private void ObtenerIds(string Id)
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

        private string ObtenerNombreGrano(string grano)
        {
            var result = "";
            IGranoStore StoreGrano = new GranoStore();
            Grano obj = StoreGrano.FindById(grano);
            if (obj != null)
            {
                result = obj.Nombre;
            }
            return result;
        }

        private string ObtenerNombreComprador(long comprador)
        {
            var result = "";
            ICompradorStore StoreComprador = new CompradorStore();
            Comprador obj = StoreComprador.FindById(comprador);
            if (obj != null){
                result = obj.Nombre;
            }
            return result;
        }

        private string ObtenerNombrePuerto(long puerto)
        {
            var result = "";
            IPuertoStore StorePuerto = new PuertoStore();
            Puerto obj = StorePuerto.FindById(puerto);
            if (obj != null){
                result = obj.Nombre;
            }
            return result;
        }

        private string ObtenerNombreVendedor(long vendedor)
        {
            var result = "";
            IVendedorStore StoreVendedor = new VendedorStore();
            Vendedor obj = StoreVendedor.FindById(vendedor);
            if (obj != null) {
                result = obj.Nombre;
            }
            return result;
        }

        private IList<Cupos> ObtenerCuerposConConsignacion(Consignacion consignacion, FiltroCentro Filtro)
        {
            CuerposConsignacion = CuposStore.FindByGranoAndCompradorAndVendedorAndPuertoAndVendcyoAndConsignacionAndCentro
                (CodigoProducto, CuentaComprador, CuentaVendedor, CuentaPuerto, 0, consignacion, Filtro);
            return CuerposConsignacion;
        }

        private void MostrarDatosCabecera()
        {
            Producto = ObtenerNombreGrano(CodigoProducto.ToString());
            Puerto = ObtenerNombrePuerto(CuentaPuerto);
            Comprador = ObtenerNombreComprador(CuentaComprador);
            Vendedor = ObtenerNombreVendedor(CuentaVendedor);
            if (Producto == null) Producto = "";
            if (Puerto == null) Puerto = "";
            if (Comprador == null) Comprador = "";
            if (Vendedor == null) Vendedor = "";
        }

        public void MostrarCodigosAlfanumericos(FiltroCentro Filtro)
        {
            IList<Cupos> cuerpos;
            try
            {
                if (Consignaciones.Count() != 1)
                {
                    ObtenerDatosConsignacion(Consignacion);
                }
                else
                {
                    ConsignacionSeleccionada = Consignaciones.FirstOrDefault();
                }
                if (ConsignacionSeleccionada != null && !Filtro.IsOneEmpty())
                {
                    cuerpos = ObtenerCuerposConConsignacion(ConsignacionSeleccionada, Filtro);
                    foreach (NuevoAlfanumerico NuevoAlfa in ListaAlfanumerico)
                    {
                        NuevoAlfa.CuerposDia = cuerpos.Where(x => x.Fecha.Date == DateTime.Now.Date.AddDays(NuevoAlfa.NumeroDia)).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void ObtenerDatosConsignacion(string consignacion)
        {
            if (consignacion != null) 
            { 
                var array = consignacion.Split('/');
                ConsignacionSeleccionada = new Consignacion
                {
                    Cuitsolicitante = array[0],
                    Nomsolicitante = array[1],
                    Cuitintermediario = array[2],
                    Nomintermediario = array[3],
                    Cuitrtecomercial = array[4],
                    Nomrtecomercial = array[5],
                    Cuitcorrcomp = array[6],
                    Nomcorrcomp = array[7],
                    Cuitmat = array[8],
                    Nommat = array[9],
                    Cuitcorrvta = array[10],
                    Nomcorrvta = array[11],
                    Cuitrteent = array[12],
                    Nomrteent = array[13],
                    Cuitdestinatario = array[14],
                    Nomdestinatario = array[15],
                    CuitRteComercialProductor = array[16],
                    NomRteComercialProductor = array[17],
                    CuitRteComercialVentaPrimaria = array[18],
                    NomRteComercialVentaPrimaria = array[19]
                };
            }
        }
    }

    public class AnularViewModel
    {
        public string IdCupos { get; set; }
        public string Tipo { get; set; }
        public string Motivo { get; set; }
        public bool Cyo { get; set; }
    }

    public class NuevoAlfanumerico
    {
        public int NumeroDia { get; set; }
        public IList<string> AlfaDia { get; set; }
        public IList<Cupos> CuerposDia { get; set; }

        public string GetDiaFormateado()
        {
            return DateTime.Now.AddDays(this.NumeroDia).Date.ToString("dd/MM");
        }

        public NuevoAlfanumerico(int NumeroDia)
        {
            this.NumeroDia = NumeroDia;
            CuerposDia = new List<Cupos>();
        }
    }

    public class BusquedaEditarViewModel
    {
        public IdViewModel Id { get; set; }
        public EditarCupoViewModel Modelo { get; set; }
    }
}