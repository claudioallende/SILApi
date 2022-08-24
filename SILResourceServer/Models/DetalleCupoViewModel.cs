using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class DetalleCupoViewModel
    {
        public DetalleCupoViewModel(string id, string centro, string centrodistribucion, bool cyo)
        {
            StoreCupos = new CuposAgrupadosStore();
            ObtenerIds(id);
            ObtenerCentros(centro, centrodistribucion);
            BuscarYMostrarCuposAgrupados(cyo);
        }
        private ICuposAgrupadosStore StoreCupos;
        public int CodigoGrano { get; set; }
        public long CuentaComprador { get; set; }
        public long CuentaPuerto { get; set; }
        public string NombreComprador {
            get 
            {
                var nombre = "";
                if (CuposAgrupados.Count > 0)
                {
                    nombre = this.CuposAgrupados[0].Comprador;
                }
                return nombre;
            }
            set { } 
        }
        public string NombrePuerto
        {
            get
            {
                var nombre = "";
                if (CuposAgrupados.Count > 0)
                {
                    nombre = this.CuposAgrupados[0].Puerto;
                }
                return nombre;
            }
            set { }
        }
        public string CodigoCentro { get; set; }
        public string CodigoCentroDistribucion { get; set; }
        public string NombreCentro { get; set; }
        public string NombreCentroDistribucion { get; set; }
        public virtual IList<ICuposAgrupadosDetalle> CuposAgrupados { get; set; }
        private void ObtenerIds(string id)
        {
            if (id != null)
            {
                var arrayIds = id.Split('-');
                CuentaComprador = Int64.Parse(arrayIds[0]);
                CuentaPuerto = Int64.Parse(arrayIds[1]);
                CodigoGrano = Int32.Parse(arrayIds[2]);
            }
            else
            {
                CuentaComprador = 0;
                CuentaPuerto = 0;
                CodigoGrano = 0;
            }
        }
        private void ObtenerCentros(string CodigoCentro, string CodigoCentroDistribucion)
        {
            ICentroStore store = new CentroStore();
            ObtenerCentro(CodigoCentro, store);
            ObtenerCentroDistribucion(CodigoCentroDistribucion, store);
        }
        private void ObtenerCentro(string CodigoCentro, ICentroStore Store)
        {
            this.CodigoCentro = CodigoCentro;
            Centro centro = Store.FindById(CodigoCentro);
            string Nombre = "";
            if (centro != null) Nombre = centro.Nombre;
            //Buscar en base de datos nombre del centro
            NombreCentro = Nombre;
            //NombreCentro = CuposAgrupados.Select(x => x.Centro).FirstOrDefault();
        }
        private void ObtenerCentroDistribucion(string CodigoCentroDistribucion, ICentroStore Store)
        {
            this.CodigoCentroDistribucion = CodigoCentroDistribucion;
            Centro centro = Store.FindById(CodigoCentroDistribucion);
            string Nombre = "";
            if (centro != null) Nombre = centro.Nombre;
            //Buscar en base de datos nombre del centro
            NombreCentroDistribucion = Nombre;
            //NombreCentroDistribucion = CuposAgrupados.Select(x => x.CentroDist).FirstOrDefault();
        }
        public void BuscarYMostrarCuposAgrupados(bool cyo){
            if (cyo)
            {
                CuposAgrupados = StoreCupos.FindByPuertoAndCompradorAndGranoAndCentroAndCentrodistGroupByPuertoAndCompradorAndCyO(CuentaComprador, CuentaPuerto, CodigoGrano, CodigoCentro, CodigoCentroDistribucion);
            }
            else
            {
                CuposAgrupados = StoreCupos.FindByPuertoAndCompradorAndGranoAndCentroAndCentrodistGroupByPuertoAndComprador(CuentaPuerto, CuentaComprador, CodigoGrano, CodigoCentro, CodigoCentroDistribucion);
            }
        }
        public readonly int CantidadDias = 20;
    }
}