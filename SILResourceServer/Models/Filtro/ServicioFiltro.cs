using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Filtro
{
    public class ServicioFiltro
    {
        public FiltroDistribucion Filtro { get; set; }
        private IFiltroStore Store { get; set; }
        private int CodigoGrano { get; set; }
        private long CuentaPuerto { get; set; }
        private long CuentaComprador { get; set; }
        private long CuentaVendedor { get; set; }

        public ServicioFiltro(int CodigoGrano, long CuentaPuerto, long CuentaComprador, long CuentaVendedor = 0)
        {
            Store = new FiltroStore();
            this.CodigoGrano = CodigoGrano;
            this.CuentaPuerto = CuentaPuerto;
            this.CuentaComprador = CuentaComprador;
            this.CuentaVendedor = CuentaVendedor;
            Filtro = BuscarFiltro(this.CuentaVendedor);
            if (Filtro == null)
            {
                Filtro = NuevoFiltro();
            }
        }

        private FiltroDistribucion NuevoFiltro() 
        {
            return new FiltroDistribucion(this.CodigoGrano, this.CuentaPuerto, this.CuentaComprador);
        }
        public FiltroDistribucion NuevoFiltro(DateTime? EntregaDesde, DateTime? EntregaHasta, string CosechaDesde, string CosechaHasta, string CentroSeleccionado)
        {
            FiltroDistribucion Filtro = NuevoFiltro();
            Filtro.EntregaDesde = EntregaDesde;
            Filtro.EntregaHasta = EntregaHasta;
            Filtro.CosechaDesde = CosechaDesde;
            Filtro.CosechaHasta = CosechaHasta;
            Filtro.Centro = CentroSeleccionado;
            return Filtro;
        }
        public FiltroDistribucion BuscarFiltro(long CuentaVendedor)
        {
            return Store.FindByKey(this.CodigoGrano, this.CuentaPuerto, this.CuentaComprador, BuscarUltimaDistribucion(CuentaVendedor));
        }
        public FiltroDistribucion CompletarFiltro(FiltroDistribucion FiltroInicial, long CuentaVendedor)
        {
            FiltroDistribucion Filtro;
            if (FiltroInicial.IsFiltroIncompleto())
            {
                Filtro = Store.FindByKey(this.CodigoGrano, this.CuentaPuerto, this.CuentaComprador, BuscarUltimaDistribucion(CuentaVendedor));
                FiltroInicial.SetEntregaDesde(Filtro.EntregaDesde);
                FiltroInicial.SetEntregaHasta(Filtro.EntregaHasta);
                FiltroInicial.SetCosechaDesde(Filtro.CosechaDesde);
                FiltroInicial.SetCosechaHasta(Filtro.CosechaHasta);
                //Falta SetCentro sino me lo trae siempre vacio
            }
            return FiltroInicial;
        }
        private void AnalizarFechaEntrega(DateTime? EntregaDesde, DateTime? EntregaHasta)
        {
            if (EntregaDesde == null) {
                Filtro.EntregaDesde = null;
            }
            else
            {
                Filtro.EntregaDesde = FechaEntregaDesde(Filtro.EntregaDesde, EntregaDesde);
            }
            if (EntregaHasta == null)
            {
                Filtro.EntregaHasta = null;
            }
            else
            {
                Filtro.EntregaHasta = FechaEntregaHasta(Filtro.EntregaHasta, EntregaHasta);
            }
        }
        private DateTime? FechaEntregaDesde(DateTime? EntregaDesdeGuardado, DateTime? EntregaDesdeNuevo)
        {
            if (EntregaDesdeGuardado == null || EntregaDesdeNuevo == null) return null;
            if (EntregaDesdeGuardado > EntregaDesdeNuevo || EntregaDesdeGuardado == default(DateTime)) return EntregaDesdeNuevo.Value.Date;
            return EntregaDesdeGuardado.Value.Date;
        }
        private DateTime? FechaEntregaHasta(DateTime? EntregaHastaGuardado, DateTime? EntregaHastaNuevo)
        {
            if (EntregaHastaGuardado == null) return EntregaHastaNuevo;
            if (EntregaHastaGuardado < EntregaHastaNuevo || EntregaHastaGuardado == default(DateTime)) return EntregaHastaNuevo.Value.Date;
            return EntregaHastaGuardado.Value.Date;
        }
        private void AnalizarCosecha(string CosechaDesde, string CosechaHasta)
        {
            Filtro.CosechaDesde = PeriodoCosechaDesde(Filtro.CosechaDesde, CosechaDesde);
            Filtro.CosechaHasta = PeriodoCosechaHasta(Filtro.CosechaHasta, CosechaHasta);
        }
        private string PeriodoCosechaDesde(string CosechaDesdeGuardado, string CosechaDesdeNuevo)
        {
            int CosechaFiltro = 0;
            int CosechaParametro = 0;
            Int32.TryParse(CosechaDesdeGuardado, out CosechaFiltro);
            Int32.TryParse(CosechaDesdeNuevo, out CosechaParametro);
            if (CosechaFiltro > CosechaParametro || CosechaFiltro == 0) return CosechaDesdeNuevo;
            return CosechaDesdeGuardado;
        }
        private string PeriodoCosechaHasta(string CosechaHastaGuardado, string CosechaHastaNuevo)
        {
            int CosechaFiltro = 0;
            int CosechaParametro = 0;
            Int32.TryParse(CosechaHastaGuardado, out CosechaFiltro);
            Int32.TryParse(CosechaHastaNuevo, out CosechaParametro);
            if (CosechaFiltro < CosechaParametro) return CosechaHastaNuevo;
            return CosechaHastaGuardado;
        }
        public FiltroDistribucion CrearOActualizarFiltro(DateTime? EntregaDesde, DateTime? EntregaHasta, string CosechaDesde, string CosechaHasta, string Centro)
        {
            AnalizarFechaEntrega(EntregaDesde, EntregaHasta);
            AnalizarCosecha(CosechaDesde, CosechaHasta);
            Filtro.Centro = Centro;
            //Filtro.Uvdist = BuscarUltimaDistribucion();
            SaveOrUpdate(Filtro);
            return Filtro;
        }
        public void SaveOrUpdate(FiltroDistribucion Filtro)
        {
            Store.SaveOrUpdate(Filtro);
        }
        private long BuscarUltimaDistribucion()
        {
            if (this.CuentaVendedor != 0)
            {
                ICuposStore StoreCupos = new CuposStore();
                return StoreCupos.LastUvdist(this.CuentaComprador, this.CuentaVendedor, this.CuentaPuerto, this.CodigoGrano);
            }
            else
            {
                return 0;
            }
        }
        private long BuscarUltimaDistribucion(long CuentaVendedor)
        {
            if (CuentaVendedor != 0)
            {
                ICuposStore StoreCupos = new CuposStore();
                return StoreCupos.LastUvdist(this.CuentaComprador, CuentaVendedor, this.CuentaPuerto, this.CodigoGrano);
            }
            else
            {
                return 0;
            }
        }
        public void GuardarFiltro(IList<VistaCuposDistribuidos> Cupos, DateTime? EntregaDesde, DateTime? EntregaHasta, string CosechaDesde, string CosechaHasta, string CentroSeleccionado)
        {
            long ultimo_uvalue = 0;
            FiltroDistribucion BusquedaFiltro;
            foreach (VistaCuposDistribuidos Cupo in Cupos)
            {
                ultimo_uvalue = BuscarUltimaDistribucion(Cupo.Vendcta);
                if (ultimo_uvalue != 0)
                {
                    Filtro.Uvdist = ultimo_uvalue;
                    BusquedaFiltro = BuscarFiltro(Cupo.Vendcta);
                    if (BusquedaFiltro != null) this.Filtro = BusquedaFiltro;
                    Filtro.CuentaVendedor = Cupo.Vendcta;
                    CrearOActualizarFiltro(EntregaDesde, EntregaHasta, CosechaDesde, CosechaHasta, CentroSeleccionado);
                }
            }
        }
        private FiltroDistribucion InicializarFiltro()
        {
            FiltroDistribucion Filtro = NuevoFiltro();
            Filtro.Uvdist = 0;
            Filtro.EntregaDesde = null;
            Filtro.EntregaHasta = DateTime.Now;
            Filtro.CosechaDesde = "";
            Filtro.CosechaHasta = "";
            Filtro.Centro = "";
            return Filtro;
        }
    }
}