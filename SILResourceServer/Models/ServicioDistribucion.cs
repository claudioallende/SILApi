using ResourceServer.Models.DataAccess;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    /// <summary>
    /// Realiza operaciones de modificación, filtrado y busquedas de distribuciones.
    /// </summary>
    public class ServicioDistribucion
    {
        private ICuposDistStore CuposDistStore { get; set; }

        public ServicioDistribucion()
        {
            CuposDistStore = new CuposDistStore();
        }

        public CuposDist ObtenerDistribucion(IList<CuposDist> Distribuciones, Cupos Cupo)
        {
            return Distribuciones.Where(x => x.Uvalue == Cupo.Uvcupodist).FirstOrDefault();
        }

        public CuposDist ObtenerDistribucion(long CuentaComprador, long CuentaVendedor, int CodigoGrano, long Destino, string Centro, DateTime Fecha, Consignacion Consignacion, ISession Session)
        {
            return CuposDistStore.FindByCompVendGranoDestCentroFechaConsignacion(CuentaComprador, CuentaVendedor, CodigoGrano, Destino, Centro, Fecha, Consignacion, Session);
        }

        public CuposDist ObtenerOCrearDistribucion(long CuentaComprador, long CuentaVendedor, int CodigoGrano, long Destino, string Centro, DateTime Fecha, Consignacion Consignacion, ISession Session)
        {
            CuposDist Distribucion = ObtenerDistribucion(CuentaComprador, CuentaVendedor, CodigoGrano, Destino, Centro, Fecha, Consignacion, Session);
            if (Distribucion == null)
            {
                Distribucion = new CuposDist(CuentaComprador, CuentaVendedor, CodigoGrano, Destino, Centro, Fecha, Consignacion);
                CuposDistStore.Save(Distribucion, Session);
            }
            return Distribucion;
        }

        public IList<CuposDist> ObtenerOCrearDistribuciones(long CuentaComprador, long CuentaVendedor, int CodigoGrano, long Destino, string Centro, IList<DateTime> Fechas, Consignacion Consignacion, ISession Session)
        {
            IList<CuposDist> Distribuciones = ObtenerDistribuciones(CuentaComprador, CuentaVendedor, CodigoGrano, Destino, Centro, Fechas, Consignacion, Session);
            foreach (DateTime Fecha in Fechas)
            {
                CuposDist Distribucion = Distribuciones.Where(x => x.Fecha.Date == Fecha.Date).FirstOrDefault();
                if (Distribucion == null)
                {
                    Distribucion = new CuposDist(CuentaComprador, CuentaVendedor, CodigoGrano, Destino, Centro, Fecha, Consignacion);
                    CuposDistStore.Save(Distribucion, Session);
                    Distribuciones.Add(Distribucion);
                }
            }
            return Distribuciones;
        }

        public IList<CuposDist> ObtenerDistribucionesPorUvalue(IList<long> Uvalues, ISession Session)
        {
            ICuposDistStore Store = new CuposDistStore();
            return Store.FindByUvalues(Uvalues, Session);

        }

        public CuposDist ObtenerDistribucion(IList<CuposDist> Distribuciones, DateTime Fecha)
        {
            if (Distribuciones == null) throw new Exception("No hay Distribuciones");
            return Distribuciones.Where(x => x.Fecha.Date == Fecha.Date).FirstOrDefault();
        }

        public IList<CuposDist> ObtenerDistribuciones(long CuentaComprador, long CuentaVendedor, int CodigoGrano, long Destino, string Centro, IList<DateTime> Fechas, Consignacion Consignacion, ISession Session)
        {
            IList<CuposDist> Distribuciones = CuposDistStore.FindByCompVendGranoDestCentroFechasConsignacion(CuentaComprador, CuentaVendedor, CodigoGrano, Destino, Centro, Fechas, Consignacion, Session);
            return Distribuciones;
        }

        public void ActualizarDistribucion(CuposDist Distribucion, ISession Session)
        {
            CuposDistStore.Update(Distribucion, Session);
        }

        public IList<CuposDist> ObtenerYRestarDistribucionesCuposPadreCYO(IList<Cupos> Cupos, ISession Session)
        {
            if (Cupos.Any(x => x.Vendcyo != 0 && x.Vendcta == x.Vendcyo))
            {
                return ObtenerYRestarDistribuciones(Cupos, Session);
            }
            return null;
        }

        public IList<CuposDist> ObtenerYRestarDistribuciones(IList<Cupos> Cupos, ISession Session)
        {
            if (Cupos != null && Cupos.Count > 0)
            {
                Cupos SeleccionCupo = Cupos.ElementAt(0);
                IList<CuposDist> Distribuciones = ObtenerDistribuciones(SeleccionCupo.Compcta, SeleccionCupo.Vendcta, SeleccionCupo.Grano, 1, SeleccionCupo.Centrodist, Cupos.Select(x => x.Fecha.Date).ToList(), SeleccionCupo.GetConsignacion(), Session);
                IList<Counter<DateTime>> CantidadPorFechasDistribuciones = CantidadCuposPorFecha(Cupos);
                IList<CuposDist> DistribucionesModificadas = RestarDistribuciones(Distribuciones, CantidadPorFechasDistribuciones);
                return DistribucionesModificadas;
            }
            else
            {
                throw new Exception("No hay Cupos para operar");
            }
        }

        public IList<Counter<DateTime>> CantidadCuposPorFecha(IList<Cupos> Cupos)
        {
            IList<Counter<DateTime>> CantidadPorFechasDistribuciones = new List<Counter<DateTime>>();
            CantidadPorFechasDistribuciones = Cupos
                .GroupBy(x => x.Fecha.Date)
                .Select(x => new Counter<DateTime>
                {
                    Value = x.Key.Date,
                    Count = x.Count()
                })
                .ToList();
            return CantidadPorFechasDistribuciones;
        }

        public IList<CuposDist> RestarDistribuciones(IList<CuposDist> Distribuciones, IList<Counter<DateTime>> CantidadPorFechasDistribuciones)
        {
            CuposDist Distribucion = null;
            IList<CuposDist> DistribucionesModificadas = new List<CuposDist>();
            foreach (Counter<DateTime> CantidadCuposPorFecha in CantidadPorFechasDistribuciones)
            {
                Distribucion = Distribuciones.Where(x => CantidadCuposPorFecha.Value == x.Fecha.Date).FirstOrDefault();
                if (Distribucion.Cupos < CantidadCuposPorFecha.Count) throw new Exception("La distribucion posee menos cupos distribuidos de los que se intenta decrementar. Cantidad distribuida: " + Distribucion.Cupos + ", Cantidad que se intenta decrementar: " + CantidadCuposPorFecha.Count);
                Distribucion.Cupos -= CantidadCuposPorFecha.Count;
                DistribucionesModificadas.Add(Distribucion);
            }
            return DistribucionesModificadas;
        }

        public void RestarDistribucionesCuposRelacionados(IList<Cupos> CuposRelacionados, ISession Session, ServicioCupo ServicioCupo = null)
        {
            if (ServicioCupo == null) ServicioCupo = new ServicioCupo();
            //Descontar distribuciones cupos relacionados
            IList<RelacionDistribucionCupos> CuposAgrupadosDistribucion = ServicioCupo.AgruparCuposPorDistribucion(CuposRelacionados, ObtenerDistribucionesPorUvalue(CuposRelacionados.Select(x => x.Uvcupodist).ToList(), Session));
            foreach (RelacionDistribucionCupos Relacion in CuposAgrupadosDistribucion)
            {
                if (Relacion.CuposDeDistribucion.Any(x => !(x.EsCuentaYOrdenPadre() && x.Status == 4))) //En el caso de que sea cupo padre cyo con status 4 no debe descontar distribucion.
                    DescontarDistribucion(Relacion.CuposDeDistribucion.Count, Relacion.Distribucion, Session);
            }
        }

        public void DescontarDistribucion(int Cantidad, CuposDist Distribucion, ISession Session)
        {
            Distribucion.Cupos -= Cantidad;
            Distribucion.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();
            ActualizarDistribucion(Distribucion, Session);
        }
    }
}