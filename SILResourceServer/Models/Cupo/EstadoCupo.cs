using ResourceServer.Models.DataAccess;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Cupo
{
    public abstract class EstadoCupo
    {
        public string Nombre { get; internal set; }
        public CodigoEstado Codigo { get; internal set; }
        public int Status { get; internal set; }
        public bool Informado { get; internal set; }
        public bool EstaDistribuido { get; internal set; }
        public string Mensaje { get; set; }
        protected ICuposStore CuposStore { get; set; }
        protected Cupos Cupo { get; set; }
        protected Cupos CupoHijo { get; set; }
        protected Cupos CupoPadre { get; set; }

        public EstadoCupo(Cupos Cupo)
        {
            this.Status = Cupo.Status;
            this.Informado = (Cupo.Pdf == 1);
            this.EstaDistribuido = (Cupo.Uvcupodist != 0);
            this.Mensaje = "";
            this.Cupo = Cupo;

            this.Nombre = "";
            this.Codigo = 0;

            CuposStore = new CuposStore();
        }

        public abstract void Distribuir(long vendedor, Consignacion consignacion, string observacion, long destino, string centro, DateTime fecha, long Uvdist, ISession session);
        public abstract bool PuedeDistribuir();
        public abstract void Informar(Cupos Encabezado, ISession session);
        public abstract bool PuedeInformar();
        public abstract void AnularDistribucion(string Motivo, Cupos Encabezado, ISession session);
        public abstract InformacionModificacionEstado AnularCupo(string Motivo, Cupos Encabezado, ISession session);
        public abstract bool PuedeAnular();

        protected Cupos ObtenerEncabezado()
        {
            ICuposStore Store = new CuposStore();
            return Store.FindById(this.Cupo.Idorigen);
        }

        public Cupos GetCupoPadreCyO()
        {
            return this.CupoPadre;
        }

        protected Cupos GetCupoPadreCyO(ISession Session)
        {
            if (this.Cupo.CupoPadreCYO == null)
            {
                Cupos buscar = (Cupos)this.Cupo.Clone();
                buscar.Compcta = 0;
                buscar.Vendcta = buscar.Vendcyo;
                buscar.Tipo = 1;
                this.CupoPadre = CuposStore.FindByCompAndVendAndGranoAndFechaAndPuertoAndIdOrigenAndAlfa(buscar, Session);
                return this.CupoPadre;
            }
            else
            {
                this.CupoPadre = this.Cupo.CupoPadreCYO;
                return this.Cupo.CupoPadreCYO;
            }
        }

        protected void AnularCupoPadre(string Motivo, Cupos Encabezado, ISession Session)
        {
            GetCupoPadreCyO(Session).GetEstado().AnularCupo(Motivo, Encabezado, Session);
        }

        protected void AnularCupoHijo(string Motivo, Cupos Encabezado, ISession Session)
        {
            GetCupoHijoCYO(Session).GetEstado().AnularCupo(Motivo, Encabezado, Session);
        }

        protected Cupos GetCupoPadreCyO(int status, ISession Session)
        {
            Cupos buscar = (Cupos)this.Cupo.Clone();
            buscar.Compcta = 0;
            buscar.Vendcta = buscar.Vendcyo;
            buscar.Tipo = 1;
            buscar.Status = status;
            this.CupoPadre = CuposStore.FindByCompAndVendAndGranoAndFechaAndPuertoAndIdOrigenAndAlfaAndStatus(buscar, Session);
            return this.CupoPadre;
        }

        //protected void LiberarCyOPadre(Cupos Encabezado, ISession session)
        //{
        //    Cupos CupoPadre = GetCupoPadreCyO(5, session);
        //    //Los parametros no los uso, solo cambia el estado a 5
        //    CupoPadre.GetEstado().AnularDistribucion(null, Encabezado, session);
        //
        //}

        //protected void LiberarCyOPadrePorAnulacionAlfaHijo(string Motivo, Cupos Encabezado, ISession session)
        //{
        //    Cupos CupoPadre = GetCupoPadreCyO(session);
        //    CupoPadre.Estado = new Models.Cupo.CYO.EstadoDistribuidoPendienteDistribuirHijo(CupoPadre);
        //    CupoPadre.GetEstado().AnularDistribucion(Motivo, Encabezado, session);
        //    CuposStore.Update(CupoPadre, session);
        //}

        protected Cupos NuevoCupoHijo(ISession Session)
        {
            Cupos Hijo = (Cupos)this.Cupo.Clone();
            Hijo.Status = 0;
            Hijo.Compcta = this.Cupo.Vendcta;
            Hijo.Vendcta = 0;
            Hijo.Centro = this.Cupo.Centrodist;
            Hijo.Centrodist = Hijo.Centro;
            Hijo.Vendcyo = this.Cupo.Vendcta;
            Hijo.Observa = this.Cupo.Observa;
            Hijo.Id = 0;
            Hijo.Pdf = 0;
            Hijo.Uvcupodist = 0;
            Hijo.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();
            if (string.IsNullOrEmpty(Hijo.Cuitrtecomercial) && string.IsNullOrEmpty(Hijo.Nomrtecomercial))
            {
                IVendedorStore store = new VendedorStore();
                Vendedor RteComercial = store.FindByNroCuenta(this.Cupo.Vendcta, Session);
                Hijo.Cuitrtecomercial = RteComercial.Cuit.Insert(RteComercial.Cuit.Length - 1, "-").Insert(2, "-");
                Hijo.Nomrtecomercial = RteComercial.Nombre;
            }
            return Hijo;
        }

        protected void AnularCupoPadre(string Motivo, ISession session)
        {
            if (Cupo.EsCuentaYOrden())
            {
                Cupos Padre = GetCupoPadreCyO(session);
                if (Padre.Status != 3)
                {
                    Padre.GetEstado().AnularCupo(Motivo, null, session);
                    Padre.Estado = new Models.Cupo.CYO.EstadoAnulado(Cupo);
                }
            }
        }

        protected Cupos GetCupoHijoCYO(ISession Session)
        {
            if (this.Cupo.CupoHijoCYO == null)
            {
                Cupos buscar = (Cupos)this.Cupo.Clone();
                buscar.Tipo = 1;
                buscar.Compcta = buscar.Vendcyo;
                buscar.Vendcta = 0;
                this.CupoHijo = this.CuposStore.FindByCompAndVendAndGranoAndFechaAndPuertoAndIdOrigenAndAlfa(buscar, Session);
            }
            else
            {
                this.CupoHijo = this.Cupo.CupoHijoCYO;
                return this.Cupo.CupoHijoCYO;
            }
            return this.CupoHijo;
        }

        protected bool EsCuentaYOrden()
        {
            return this.Cupo.Vendcyo != 0;
        }

        protected void DistribuirCyOPadre(ISession session, long UvDist)
        {
            Cupos CupoPadre = GetCupoPadreCyO(4, session);
            //Los parametros no los uso, solo cambia el estado a 5
            CupoPadre.GetEstado().Distribuir(CupoPadre.Vendcta, CupoPadre.GetConsignacion(), CupoPadre.Observa, 0, null, DateTime.Now, UvDist, session);
        }

        //Si es CUIT de ACA verificar que la cuenta este en la DTabla
        //Si no es CUIT de ACA verificar que el CUIT este en la DTabla
        protected bool EsVendedorCuentaYOrden(long CuentaVendedor, ISession Session)
        {
            IDTablaStore DtablaStore = new DTablaStore();
            string CuitACA = "30-50012088-2";
            ServicioCuenta servicio = new ServicioCuenta(new VendedorStore());
            IList<ICuenta> Vendedores = servicio.GetFromNumeroCuenta(CuentaVendedor.ToString(), Session);
            if (Vendedores.Count == 0) return false;
            ICuenta Vendedor = Vendedores.ElementAt(0);
            string CuitConGuiones = Vendedor.Cuit.Insert(2, "-");
            CuitConGuiones = CuitConGuiones.Insert(CuitConGuiones.Length - 1, "-");
            DTabla Entidad = DtablaStore.FindByEntidadAndOrdenAndValor("CUPCYO", "CYO03", CuitConGuiones, Session);
            if (Entidad == null) return false;
            if (Entidad.Valor == CuitACA)
            {
                return (DtablaStore.FindByEntidadAndOrdenAndValor("CUPCYO", "CYO01", CuentaVendedor.ToString(), Session) != null);
            }
            else
            {
                return true;
            }
        }

        protected void ConvertirCupoDistribuidoACyO()
        {
            this.Cupo.Vendcyo = this.Cupo.Vendcta;
        }

        protected InformacionModificacionEstado GetInformacion()
        {
            return new InformacionModificacionEstado { CupoPadreCyOModificado = this.CupoPadre, CupoHijoCyOModificado = this.CupoHijo };
        }
    }

    public enum CodigoEstado : int
    {
        Creado = 0,
        DistribuidoPendienteInformar = 1,
        DistribuidoInformado = 2,
        DistribucionAnuladaPendienteInformar = 3,
        DistribucionAnuladaInformada = 4,
        AnuladoPendienteInformar = 5,
        Anulado = 6,
        CuentaYOrdenCreado = 100,
        CuentaYOrdenDistribuidoPendienteDistribuirHijo = 101,
        CuentaYOrdenDistribuidoConHijoDistribuido = 102,
        CuentaYOrdenAnulado = 103,
        CuentaYOredenPendienteDistribuirHijoPendienteInformar = 104
    }
}