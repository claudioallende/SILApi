using ResourceServer.Models.Configuracion;
using ResourceServer.Models.Cupo;
using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Error;
using ResourceServer.Models.Error.Exceptions;
using ResourceServer.Models.Filtro;
using ResourceServer.Models.Pdf;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class ServicioDistribuir
    {
        private IVistaCuposDistribuidosStore CuposDistribuir;
        private ICuposStore Cupostore;
        private ICuposDistStore CuposDist;

        private IList<VistaCuposDistribuidos> CuposDesdeVista { get; set; }
        private IList<Cupos> cuerposDisponibles;
        private IList<Cupos> cuerposDistribuidos;
        private IList<Cupos> CuposDistribuidosModificados;
        private List<Cupos> Cupos;
        private bool huboCambiosDistrib;
        private readonly int CantidadDias = 20;
        private ServicioCupo ServicioCupo;
        private ServicioDistribucion ServicioDistribucion;

        private int INFO_NO_HUBO_CAMBIOS = 300 ;
        private int INFO_NO_ENCUENTRO_REGISTRO = 310;

        private bool Confirmacion;

        public ServicioDistribuir(bool Confirmacion = false)
        {
            CuposDistribuir = new VistaCuposDistribuidosStore();
            Cupostore = new CuposStore();
            CuposDist = new CuposDistStore();
            CuposDistribuidosModificados = new List<Cupos>();
            this.ServicioCupo = new ServicioCupo();
            this.ServicioDistribucion = new ServicioDistribucion();
            this.Confirmacion = Confirmacion;
        }

        public int ValidarYGuardar(RegistroDistribucionViewModel model)
        {
            if (model.CosechaDesde == "0" || model.CosechaDesde == null) model.CosechaDesde = FiltroPorDefecto.GetFiltroPorDefectoCosechaDesde();
            if (model.CosechaHasta == "0" || model.CosechaHasta == null) model.CosechaHasta = FiltroPorDefecto.GetFiltroPorDefectoCosechaHasta();
            if (model.fecha == null) model.fecha = FiltroPorDefecto.GetFiltroPorDefectoFechaHasta();
            if (model.fechaDesde == null) model.fechaDesde = FiltroPorDefecto.GetFiltroPorDefectoFechaDesde();
            return VerificoDisponiblesYregistroDitrubucion(model.cupos, model.CosechaDesde, model.CosechaHasta, model.fechaDesde, model.fecha, Int64.Parse(model.puerto), model.anterior, model.nuevo, model.tieneVendedor);
        }

        public void BuscarYAnularDistribuciones(IList<Cupos> Cupos, string MotivoBaja, bool Cyo, ISession Session)
        {
            if (Cupos != null && Cupos.Count > 0)
            {
                //Descontar y anular distribuciones cupos relacionados
                IList<Cupos> CuposRelacionados = this.ServicioCupo.ObtenerCuposRelacionados(Cupos, Session);
                IList<RelacionDistribucionCupos> CuposAgrupadosDistribucion = this.ServicioCupo.AgruparCuposPorDistribucion(CuposRelacionados, this.ServicioDistribucion.ObtenerDistribucionesPorUvalue(CuposRelacionados.Select(x => x.Uvcupodist).ToList(), Session));
                foreach (RelacionDistribucionCupos Relacion in CuposAgrupadosDistribucion)
                {
                    this.ServicioDistribucion.DescontarDistribucion(Relacion.CuposDeDistribucion.Where(x => !(x.GetEstado().Codigo == CodigoEstado.CuentaYOrdenDistribuidoConHijoDistribuido)).Count(), Relacion.Distribucion, Session);
                }
                //Descontar distribuciones y anular cupos de la lista Cupos recivida por parametro
                CuposAgrupadosDistribucion = this.ServicioCupo.AgruparCuposPorDistribucion(Cupos, this.ServicioDistribucion.ObtenerDistribucionesPorUvalue(Cupos.Select(x => x.Uvcupodist).ToList(), Session));
                foreach (RelacionDistribucionCupos Relacion in CuposAgrupadosDistribucion)
                {
                    AnularYDescontarDistribuciones(Relacion.CuposDeDistribucion, MotivoBaja, Relacion.Distribucion, Cyo, Session);
                }
            }
        }

        //Valida cantidad de cupos a distribuir
        /*queda pendiente el caso en que sume y luego reste en la columna. no te da de alta los cupos por cantidad y luego libera*/
        private int VerificoDisponiblesYregistroDitrubucion(IList<VistaCuposDistribuidos> listaCuposDistribuir, string cosechaD, string cosechaH, DateTime? FechaDesde, DateTime? FechaHasta, long puerto, Cupos cupoViejo, Cupos cupoConsignacion, bool tieneVendedor)
        {
            huboCambiosDistrib = false;
            int acumulado;
            int totalDia0 = listaCuposDistribuir.Sum(x => x.Hoy);
            int totalDia1 = listaCuposDistribuir.Sum(x => x.Dia1);
            int totalDia2 = listaCuposDistribuir.Sum(x => x.Dia2);
            int totalDia3 = listaCuposDistribuir.Sum(x => x.Dia3);
            int totalDia4 = listaCuposDistribuir.Sum(x => x.Dia4);
            int totalDia5 = listaCuposDistribuir.Sum(x => x.Dia5);
            int totalDia6 = listaCuposDistribuir.Sum(x => x.Dia6);
            int totalDia7 = listaCuposDistribuir.Sum(x => x.Dia7);
            int totalDia8 = listaCuposDistribuir.Sum(x => x.Dia8);
            int totalDia9 = listaCuposDistribuir.Sum(x => x.Dia9);
            int totalDia10 = listaCuposDistribuir.Sum(x => x.Dia10);
            int totalDia11 = listaCuposDistribuir.Sum(x => x.Dia11);
            int totalDia12 = listaCuposDistribuir.Sum(x => x.Dia12);
            int totalDia13 = listaCuposDistribuir.Sum(x => x.Dia13);
            int totalDia14 = listaCuposDistribuir.Sum(x => x.Dia14);
            int totalDia15 = listaCuposDistribuir.Sum(x => x.Dia15);
            int totalDia16 = listaCuposDistribuir.Sum(x => x.Dia16);
            int totalDia17 = listaCuposDistribuir.Sum(x => x.Dia17);
            int totalDia18 = listaCuposDistribuir.Sum(x => x.Dia18);
            int totalDia19 = listaCuposDistribuir.Sum(x => x.Dia19);
            int totalDia20 = listaCuposDistribuir.Sum(x => x.Dia20);
            Cupos = new List<Cupos>();
            IList<string> mapping = new List<string>();
            mapping.Add("Cupos.mpg.xml");
            mapping.Add("CuposDist.mpg.xml");
            using (ISession session = HibernateUtil.OpenSession(mapping))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    foreach (VistaCuposDistribuidos vistaElement in listaCuposDistribuir)
                    {
                        acumulado = vistaElement.GetAcumuladoDias();//.Hoy + vistaElement.Dia1 + vistaElement.Dia2 + vistaElement.Dia3 + vistaElement.Dia4 + vistaElement.Dia5;
                        VistaCuposDistribuidos search = new VistaCuposDistribuidos();
                        search.Codproducto = vistaElement.Codproducto;
                        search.Vendcta = vistaElement.Vendcta;
                        search.Compcta = vistaElement.Compcta;
                        search.Ctadestino = vistaElement.Ctadestino;
                        search.SetConsignacion(cupoViejo.GetConsignacion());
                        search.Fechaent = DateUtils.n_date(FechaHasta);
                        search.Codcentro = vistaElement.Centro;
                        VistaCuposDistribuidos CupoDistribuidos = CuposDistribuir.FindByProdCompVendPuerFechaCoseCentDestino(search, cosechaD, cosechaH, FechaDesde, FechaHasta, session);
                        IList<CuposDist> Distribuciones = this.ServicioDistribucion.ObtenerOCrearDistribuciones(vistaElement.Compcta, vistaElement.Vendcta, vistaElement.Codproducto, vistaElement.Ctadestino, vistaElement.Centro, GetFechasModificoDistribucion(vistaElement), cupoConsignacion.GetConsignacion(), session);
                        if (CupoDistribuidos.Cuposadist >= acumulado)
                        {
                            Cupos.AddRange(modificarXDia(0, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia0, Distribuciones, session));
                            totalDia0 -= vistaElement.Hoy;
                            Cupos.AddRange(modificarXDia(1, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia1, Distribuciones, session));
                            totalDia1 -= vistaElement.Dia1;
                            Cupos.AddRange(modificarXDia(2, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia2, Distribuciones, session));
                            totalDia2 -= vistaElement.Dia2;
                            Cupos.AddRange(modificarXDia(3, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia3, Distribuciones, session));
                            totalDia3 -= vistaElement.Dia3;
                            Cupos.AddRange(modificarXDia(4, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia4, Distribuciones, session));
                            totalDia4 -= vistaElement.Dia4;
                            Cupos.AddRange(modificarXDia(5, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia5, Distribuciones, session));
                            totalDia5 -= vistaElement.Dia5;
                            Cupos.AddRange(modificarXDia(6, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia6, Distribuciones, session));
                            totalDia6 -= vistaElement.Dia6;
                            Cupos.AddRange(modificarXDia(7, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia7, Distribuciones, session));
                            totalDia7 -= vistaElement.Dia7;
                            Cupos.AddRange(modificarXDia(8, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia8, Distribuciones, session));
                            totalDia8 -= vistaElement.Dia8;
                            Cupos.AddRange(modificarXDia(9, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia9, Distribuciones, session));
                            totalDia9 -= vistaElement.Dia9;
                            Cupos.AddRange(modificarXDia(10, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia10, Distribuciones, session));
                            totalDia10 -= vistaElement.Dia10;
                            Cupos.AddRange(modificarXDia(11, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia11, Distribuciones, session));
                            totalDia11 -= vistaElement.Dia11;
                            Cupos.AddRange(modificarXDia(12, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia12, Distribuciones, session));
                            totalDia12 -= vistaElement.Dia12;
                            Cupos.AddRange(modificarXDia(13, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia13, Distribuciones, session));
                            totalDia13 -= vistaElement.Dia13;
                            Cupos.AddRange(modificarXDia(14, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia14, Distribuciones, session));
                            totalDia14 -= vistaElement.Dia14;
                            Cupos.AddRange(modificarXDia(15, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia15, Distribuciones, session));
                            totalDia15 -= vistaElement.Dia15;
                            Cupos.AddRange(modificarXDia(16, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia16, Distribuciones, session));
                            totalDia16 -= vistaElement.Dia16;
                            Cupos.AddRange(modificarXDia(17, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia17, Distribuciones, session));
                            totalDia17 -= vistaElement.Dia17;
                            Cupos.AddRange(modificarXDia(18, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia18, Distribuciones, session));
                            totalDia18 -= vistaElement.Dia18;
                            Cupos.AddRange(modificarXDia(19, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia19, Distribuciones, session));
                            totalDia19 -= vistaElement.Dia19;
                            Cupos.AddRange(modificarXDia(20, CupoDistribuidos, vistaElement, puerto, cupoViejo, cupoConsignacion, tieneVendedor, totalDia20, Distribuciones, session));
                            totalDia20 -= vistaElement.Dia20;
                        }
                    }
                    tx.Commit();
                    HibernateUtil.Dispose();
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    HibernateUtil.Dispose();
                    throw e;
                }
            }
            if (!huboCambiosDistrib)
            {
                return INFO_NO_HUBO_CAMBIOS;
            }
            return 1;
        }

        //Ver como solucionar: vistaCuposDistribViewRow es un objeto, distribucioncupos es una lista
        private IList<Cupos> modificarXDia(int dia, VistaCuposDistribuidos vistaCuposDitribBDRow, 
            VistaCuposDistribuidos vistaCuposDistribViewRow, long puerto, Cupos cupoViejo, 
            Cupos cupoConsignacion, bool tieneVendedor, int totalDia, 
            IList<CuposDist> Distribuciones, ISession session)
        {
            string nombre = "Dia";
            if (dia >= 0 && dia <= 20)
            {
                nombre = nombre + dia;
                if (nombre == "Dia0") nombre = "Hoy";
                int cupoPedidosDiaCteBD = (int)vistaCuposDitribBDRow.GetType().GetProperty(nombre).GetValue(vistaCuposDitribBDRow, null);
                int cupoPedidosDiaCteView = (int)vistaCuposDistribViewRow.GetType().GetProperty(nombre).GetValue(vistaCuposDistribViewRow, null);

                if (cupoPedidosDiaCteView != 0 && !(dia == 0 && AppSettingConfig.GetHoraLimite(DateTime.Now.Date) < DateTime.Now))
                {
                    huboCambiosDistrib = true;
                    int cantidadCuposTotalDia = cupoPedidosDiaCteBD + cupoPedidosDiaCteView;
                    IList<Cupos> CuposADistribuir = new List<Cupos>();

                    CuposDist Distribucion = this.ServicioDistribucion.ObtenerDistribucion(Distribuciones, retornaDiainDate(dia));

                    if (cupoPedidosDiaCteBD > cantidadCuposTotalDia)
                    {   //decrementó
                        CuposDistribuidosModificados = BuscarCuposYDescontarDistribuciones(
                            Math.Abs(cupoPedidosDiaCteView),
                            new ClaveCupo { CuentaComprador = vistaCuposDistribViewRow.Compcta, CuentaVendedor = vistaCuposDistribViewRow.Vendcta, CuentaPuerto = puerto, CodigoGrano = vistaCuposDistribViewRow.Codproducto },
                            cupoConsignacion.Motbaja,
                            retornaDiainDate(dia),
                            cupoViejo.GetConsignacion(),
                            Distribucion,
                            session
                        );
                    }
                    else
                    {   //incrementó
                        if (CheckCuposForConsignation(totalDia, vistaCuposDistribViewRow.Compcta, vistaCuposDistribViewRow.Vendcta, vistaCuposDistribViewRow.Codproducto, retornaDiainDate(dia), puerto, cupoViejo, tieneVendedor, cupoPedidosDiaCteBD, session) > -1)
                        {
                            HibernateUtil.RefreshBeforeUpdate<CuposDist>(Distribucion, session);
                            CuposADistribuir = this.ServicioCupo.GetCuposTransformarSiVendedorEsCYO(cuerposDisponibles, vistaCuposDistribViewRow.Vendcta, cupoPedidosDiaCteView, session);
                            IList<Cupos> CuposDistribuidos = DistribuirCupos(CuposADistribuir, vistaCuposDistribViewRow.Vendcta, retornaDiainDate(dia), cupoConsignacion.GetConsignacion(), vistaCuposDistribViewRow.Ctadestino, vistaCuposDistribViewRow.Centro, cupoConsignacion.Observa, cupoConsignacion.ContactoComercial, Distribucion.Uvalue, session);
                            Distribucion.Cupos += cupoPedidosDiaCteView;
                            Distribucion.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();
                            this.ServicioDistribucion.ActualizarDistribucion(Distribucion, session);
                        }
                        else
                        {
                            throw new CuposPorConsignacionExcedidosException();
                        }
                    }
                }
                return Cupos;
            }
            else
            {//dia incorrecto
                throw new Exception("Día incorrecto");
            }
        }

        /// <summary>
        /// Busca los Cupos que coincidan con los datos de la clave y descuenta de la distribucion. Los cupos padre CYO con status 5 no se anulan.
        /// </summary>
        /// <param name="Cantidad"></param>
        /// <param name="ClaveCupo"></param>
        /// <param name="MotivoBaja"></param>
        /// <param name="Dia"></param>
        /// <param name="Consignacion"></param>
        /// <param name="Distribucion"></param>
        /// <param name="Session"></param>
        /// <returns></returns>
        public IList<Cupos> BuscarCuposYDescontarDistribuciones(int Cantidad, ClaveCupo ClaveCupo, string MotivoBaja, DateTime Dia, Consignacion Consignacion, CuposDist Distribucion, ISession Session)
        {
            IList<Cupos> CuposDisponiblesIgnorandoPuerto;
            CuposDisponiblesIgnorandoPuerto = Cupostore.FindByCompradorVendedorGranoFechaConsignacionStatus(ClaveCupo.CuentaComprador, ClaveCupo.CuentaVendedor, ClaveCupo.CodigoGrano, Dia, Consignacion, new List<int> { 4, 5 }, Session);
            cuerposDistribuidos = CuposPuertoSeleccionado(CuposDisponiblesIgnorandoPuerto, ClaveCupo.CuentaPuerto);
            if (!TieneSuficientesCupos(cuerposDistribuidos, Cantidad) && TieneSuficientesCupos(CuposDisponiblesIgnorandoPuerto, Cantidad)) throw new InsuficienteCantidadCuposPuertoSeleccionado();
            IList<Cupos> CuposADistribuir = this.ServicioCupo.FiltrarNoCuposCuentaYOrdenPadreDistribuidosConHijoDistribuido(cuerposDistribuidos).Take(Cantidad).ToList();
            //En caso de que la cantidad de cupos disminuyo debido a que algunos son cyo padre con hijos distribuidos lanza una excepcion con los cupos padre.
            //Esta excepcion se lanza en caso de que no sea confirmado que igual desea anular los cupos que no son cyo padre
            if (CuposADistribuir.Count < Cantidad && !this.Confirmacion)
                throw new CuposNoAnulablesException(this.ServicioCupo.FiltrarCuposCuentaYOrdenPadreDistribuidosConHijoDistribuido(cuerposDistribuidos), Cantidad);
            Cantidad = CuposADistribuir.Count;
            IList<Cupos> CuposDistribuidosModificados = new List<Cupos>();
            //if (CuposADistribuir.Count < Cantidad)
            //    throw new Exception("Cantidad de cupos Distribuidos es menor a la cantidad solicitada para decrementar. Comprador " + ClaveCupo.CuentaComprador + ", Vendedor " + ClaveCupo.CuentaVendedor + ", Puerto " + ClaveCupo.CuentaPuerto + ", Grano " + ClaveCupo.CodigoGrano + ", Fecha " + Dia.Date.ToString() + ".\n" + "Cantidad de cupos a decrementar solicitados: " + Cantidad + ", Cantidad de cupos disponibles para decrementar: " + CuposADistribuir.Count);
            IList<Cupos> Encabezados = this.ServicioCupo.ObtenerEncabezados(CuposADistribuir, Session);
            Cupos EncabezadoSeleccionado = null;
            if (cuerposDistribuidos != null && cuerposDistribuidos.Count > 0)
            {
                HibernateUtil.RefreshBeforeUpdate<CuposDist>(Distribucion, Session);
                //this.ServicioCupo.EliminarCuposHijoCYOFromPadres(CuposADistribuir, Session); //Elimino primero los hijos sino despues cambia de estado.
                for (int i = 0; i < Cantidad; i++)
                {
                    EncabezadoSeleccionado = this.ServicioCupo.ObtenerEncabezado(Encabezados, CuposADistribuir.ElementAt(i));
                    CuposDistribuidosModificados.Add(LiberarCupo(CuposADistribuir.ElementAt(i), MotivoBaja, EncabezadoSeleccionado, Session));
                }
                //Libero los cupos cyo padre si la lista CuposDistribuidosModificados son hijos cyo.
                this.ServicioCupo.ObtenerYLiberarCuposPadreCYO(CuposDistribuidosModificados, Session);
                Distribucion.Cupos -= Cantidad;
                Distribucion.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();
                this.ServicioDistribucion.ActualizarDistribucion(Distribucion, Session);
            }
            return CuposDistribuidosModificados;
        }

        /// <summary>
        /// Anula las distribuciones de los cupos que recive por parametro. Anula aunque sea cupo padre CYO.
        /// </summary>
        /// <param name="CuposADistribuir"></param>
        /// <param name="MotivoBaja"></param>
        /// <param name="Distribucion"></param>
        /// <param name="Session"></param>
        /// <returns></returns>
        public IList<Cupos> AnularYDescontarDistribuciones(IList<Cupos> CuposADistribuir, IList<Cupos> CuposRelacionados, string MotivoBaja, CuposDist Distribucion, bool Cyo, ISession Session)
        {
            List<Cupos> CuposDistribuidosModificados = new List<Cupos>();
            IList<Cupos> Encabezados = this.ServicioCupo.ObtenerEncabezados(CuposADistribuir, Session);
            int CantidadTotalCupos = CuposADistribuir.Where(x => x.Status == 4 || x.Status == 5).Count();
            CuposADistribuir = FiltrarCuposNormalesYAnularCupoPadreCYO(CuposADistribuir, CuposRelacionados, MotivoBaja, Encabezados, Session);
            CuposDistribuidosModificados.AddRange(AnularDistribuciones(CuposADistribuir, MotivoBaja, Encabezados, Cyo, Session));
            if (CantidadTotalCupos > 0) this.ServicioDistribucion.DescontarDistribucion(CantidadTotalCupos, Distribucion, Session);
            return CuposDistribuidosModificados;
        }

        public IList<Cupos> AnularYDescontarDistribuciones(IList<Cupos> CuposADistribuir, string MotivoBaja, CuposDist Distribucion, bool Cyo, ISession Session)
        {
            List<Cupos> CuposDistribuidosModificados = new List<Cupos>();
            IList<Cupos> Encabezados = this.ServicioCupo.ObtenerEncabezados(CuposADistribuir, Session);
            int CantidadTotalCupos = CuposADistribuir.Where(x => x.Status == 4 || x.Status == 5).Count();
            //Anula las distribuciones de padres cyo y normales
            this.ServicioCupo.SetearCuposHijoAPadreCYO(CuposADistribuir, Session);
            CuposADistribuir = this.ServicioCupo.ObtenerCuposPadreCYOYNormales(CuposADistribuir);
            /////////////////////////////////////////////////////////////////////////////////////
            CuposDistribuidosModificados.AddRange(AnularDistribuciones(CuposADistribuir, MotivoBaja, Encabezados, Cyo, Session));
            if (CantidadTotalCupos > 0) this.ServicioDistribucion.DescontarDistribucion(CantidadTotalCupos, Distribucion, Session);
            return CuposDistribuidosModificados;
        }

        public IList<Cupos> FiltrarCuposNormalesYAnularCupoPadreCYO(IList<Cupos> Cupos, IList<Cupos> CuposRelacionados, string Motivo, IList<Cupos> Encabezados, ISession Session)
        {
            IList<Cupos> CuposNormales = this.ServicioCupo.FiltrarCuposNormales(Cupos);
            FiltrarCuposPadreCYOAnularDistribucion(Cupos, CuposRelacionados, Motivo, Encabezados, Session);
            return CuposNormales;
        }

        /// <summary>
        /// Anula los cupos recividos por parametro
        /// </summary>
        /// <param name="CuposADistribuir">Lista de cupos normales no CYO</param>
        /// <param name="MotivoBaja"></param>
        /// <param name="Encabezados"></param>
        /// <param name="Session"></param>
        /// <returns></returns>
        public IList<Cupos> AnularDistribuciones(IList<Cupos> CuposADistribuir, string MotivoBaja, IList<Cupos> Encabezados, bool Cyo, ISession Session)
        {
            Cupos EncabezadoSeleccionado = null;
            int CantidadCupos = CuposADistribuir == null ? 0 : CuposADistribuir.Count;
            if (CuposADistribuir != null && CantidadCupos > 0)
            {
                //Si es Cyo = true significa que estoy anulando el hijo cyo por lo tanto debo liberar a status = 4 al padre. Si es false debo poner en estado 0 al padre.
                if (Cyo)
                {

                    for (int i = 0; i < CantidadCupos; i++)
                    {
                        EncabezadoSeleccionado = this.ServicioCupo.ObtenerEncabezado(Encabezados, CuposADistribuir.ElementAt(i));
                        CuposDistribuidosModificados.Add(LiberarCupo(CuposADistribuir.ElementAt(i), MotivoBaja, EncabezadoSeleccionado, Session));
                    }
                }
                else
                {
                    for (int i = 0; i < CantidadCupos; i++)
                    {
                        EncabezadoSeleccionado = this.ServicioCupo.ObtenerEncabezado(Encabezados, CuposADistribuir.ElementAt(i));
                        CuposADistribuir.ElementAt(i).GetEstado().AnularDistribucion(MotivoBaja, EncabezadoSeleccionado, Session);
                        CuposDistribuidosModificados.Add(CuposADistribuir.ElementAt(i));
                    }
                }
            }
            return CuposDistribuidosModificados;
        }

        public void FiltrarCuposPadreCYOAnularDistribucion(IList<Cupos> Cupos, IList<Cupos> CuposRelacionados, string Motivo, IList<Cupos> Encabezados, ISession Session)
        {
            IList<Cupos> CuposPadre = this.ServicioCupo.FiltrarCuposPadreCYO(Cupos);
            IList<CupoCuentaYOrden> CuposCuentaYOrden = this.ServicioCupo.AsociarCupoPadreHijo(CuposPadre, CuposRelacionados);
            EstadoCupo EstadoHijo;
            foreach (CupoCuentaYOrden Cupo in CuposCuentaYOrden)
            {
                EstadoHijo = Cupo.CupoHijo.GetEstado();
                if (Cupo.CupoHijo.Pdf == 0 && (EstadoHijo.Codigo == CodigoEstado.DistribuidoInformado))
                {
                    //Cupo.CupoPadre.Pdf = 1;
                    Cupo.CupoPadre.Status = 0;
                    Cupo.CupoPadre.Estado = new ResourceServer.Models.Cupo.CYO.EstadoPendienteDistribuirConHijoPendienteInformar(Cupo.CupoPadre);
                }
                else
                {
                    Cupo.CupoPadre.GetEstado().AnularDistribucion(Motivo, this.ServicioCupo.ObtenerEncabezado(Encabezados, Cupo.CupoPadre), Session);
                }
            }
        }

        /// <summary>
        /// Si la lista de CuposRelacionados son cupos hijo, los cupos padre cambian de status 5 a 4. Si son cupos padre elimina los hijos.
        /// </summary>
        /// <param name="CuposRelacionados"></param>
        /// <param name="MotivoBaja"></param>
        /// <param name="Distribucion"></param>
        /// <param name="Session"></param>
        /// <returns></returns>
        public IList<Cupos> AnularYDescontarDistribucionesCuposRelacionados(IList<Cupos> CuposRelacionados, string MotivoBaja, CuposDist Distribucion, ISession Session)
        {
            IList<Cupos> Encabezados = this.ServicioCupo.ObtenerEncabezados(CuposRelacionados, Session);
            IList<Cupos> CuposRelacionadosPadre = ServicioCupo.FiltrarCuposPadreCYOEliminarCuposHijo(CuposRelacionados, Session);
            this.ServicioCupo.LiberarCuposPadreCYO(CuposRelacionadosPadre, Session);
            this.ServicioDistribucion.RestarDistribucionesCuposRelacionados(CuposRelacionados, Session, this.ServicioCupo);
            return CuposRelacionados;
        }

        public Cupos LiberarCupo(Cupos Cupo, string Motivo, Cupos Encabezado, ISession Session)
        {
            if (Cupo.EsCuentaYOrdenPadre() && Cupo.GetEstado().Codigo == CodigoEstado.CuentaYOrdenDistribuidoConHijoDistribuido)
                ((ResourceServer.Models.Cupo.CYO.EstadoDistribuidoConHijoDistribuido)Cupo.GetEstado()).LiberarCupo(Motivo, Encabezado, Session);
            else
                Cupo.GetEstado().AnularDistribucion(Motivo, Encabezado, Session);
            return Cupo;
        }

        private DateTime retornaDiainDate(int dia)
        {
            return DateTime.Now.AddDays(dia).Date;
        }

        /*chequea si estoy asignando mas cupos de los que tengo disponible para esta consignacion
         *  si menor que cero -> pido de mas
         *  si igual que cero -> la misma cantidad
         *  si mayor que cero -> menos de los que tengo disponibles*/
        private int CheckCuposForConsignation(int nroCuposIngresados, Int64 compcta, Int64 vendcta, int grano, DateTime fecha, long puerto, Cupos cupoViejo, bool tieneVendedor, int cuposYaOtorgados, ISession Session)
        {
            IList<Counter<Cupos>> cantConsig;
            if (tieneVendedor)
            {
                cantConsig = Cupostore.FindNumberOfConsignacionesForKey(compcta, vendcta, grano, fecha, puerto, cupoViejo, Session);
                cuerposDisponibles = Cupostore.FindForKey(compcta, vendcta, puerto, grano, fecha, cupoViejo.GetConsignacion(), Session);
            }
            else
            {
                cantConsig = Cupostore.FindNumberOfConsignacionesForKey(compcta, 0, grano, fecha, puerto, cupoViejo, Session);
                cuerposDisponibles = Cupostore.FindForKey(compcta, 0, puerto, grano, fecha, cupoViejo.GetConsignacion(), Session);
            }
            if (cantConsig != null && cantConsig.Count > 0)
            {
                return cantConsig.ElementAt(0).Count - (nroCuposIngresados - cuposYaOtorgados);
            }
            else
            {
                return -1;
            }
        }

        public IList<Cupos> CuposPuertoSeleccionado(IList<Cupos> ListaCupos, long CuentaPuerto)
        {
            return ListaCupos.Where(Cupo => Cupo.Puerto == CuentaPuerto).ToList();
        }

        public bool TieneSuficientesCupos(IList<Cupos> ListaCupos, int Cantidad)
        {
            return ListaCupos.Count >= Cantidad;
        }

        private IList<Cupos> DistribuirCupos(IList<Cupos> ListaCupos, long CuentaVendedor, DateTime Fecha, Consignacion Consignacion, long CuentaDestino, string Centro, string Observacion, string ContactoComercial, long Uvdist, ISession Session)
        {
            IList<Cupos> Lista = new List<Cupos>();
            Cupos CupoSeleccionado = null;
            try
            {
                foreach (Cupos Cupo in ListaCupos)
                {
                    CupoSeleccionado = Cupo;
                    Cupo.GetEstado().Distribuir(CuentaVendedor, Consignacion, Observacion, ContactoComercial, CuentaDestino, Centro, Fecha, Uvdist, Session);
                    Lista.Add(Cupo);
                }
            }
            catch (NHibernate.Exceptions.GenericADOException e)
            {
                if (e.GetBaseException().Message.Contains("ORA-00001"))
                {
                    throw new CodigoAlfanumericoDuplicadoException(CupoSeleccionado.Nrocupo);
                }
                else
                {
                    throw e;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return Lista;
        }

        private Cupos DistribuirCuerpo(int ordenDeDistribucion, Int64 vendcta, DateTime fecha, Cupos cuposConsignacion, long destino, string centro, long uvdist, ISession session)
        {
            if (cuerposDisponibles.Count > 0) {
                Cupos element = cuerposDisponibles.ElementAt(ordenDeDistribucion);
                element.GetEstado().Distribuir(vendcta, cuposConsignacion.GetConsignacion(), cuposConsignacion.Observa, cuposConsignacion.ContactoComercial, destino, centro, fecha, uvdist, session);   
                cuerposDisponibles.RemoveAt(0);
                return element;
            }
            return null;               
        }
        
        private Cupos LiberarCuerpo(int ordenLiberacion, string motBaja, Cupos Encabezado, ISession session)
        {
            if (cuerposDistribuidos != null && cuerposDistribuidos.Count > 0)
            {
                Cupos element = cuerposDistribuidos.ElementAt(ordenLiberacion);
                element.GetEstado().AnularDistribucion(motBaja, Encabezado, session);
                cuerposDistribuidos.RemoveAt(ordenLiberacion);
                return element;
            }
            return null;
        }

        public IList<VistaCtoCorre> ObtenerContratos(long compcta, long vendcta, long ctadestino, string codcentro, int grano, DateTime? fechaent)
        {
            IVistaCtoCorreStore store = new VistaCtoCorreStore();
            return store.FindByCompctaAndVendctaAndCtadestinoAndCodcentroAndGranoAndFechaent(compcta, vendcta, ctadestino, codcentro, grano, DateUtils.n_date(fechaent));
        }

        public IList<int> ObtenerCantidadTotalCuposPorDiaParaConsignacion(Cupos consignacion)
        {
            IList<int> Cantidad = new List<int>();
            IList<Counter<Cupos>> ConsignacionesAgrupadosPorFecha = Cupostore.FindConsignacionesForKeyAndConsignacionGroupByFecha(consignacion);
            for (int Dia = 0; Dia <= CantidadDias; Dia++)
            {
                Cantidad.Add(ConsignacionesAgrupadosPorFecha.Where(x => x.Value.Fecha.Date == DateTime.Now.AddDays(Dia).Date).Sum(x => x.Count));
            }
            return Cantidad;
        }

        /// <summary>
        /// Obtiene una agrupacion de contratos con sus cupos distribuidos asociados, 
        /// cantidad de cupos disponibles para distribuir en cinco dias y cantidad de cupos distribuidos en los cinco dias.
        /// </summary>
        /// <param name="datosContrato">Datos de consignacion, cuenta de comprador, cuenta de vendedor, codigo del producto y centro</param>
        /// <param name="CuentaPuerto"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="cosechaDesde"></param>
        /// <param name="cosechaHasta"></param>
        /// <param name="Cyo"></param>
        /// <returns></returns>
        public DistribucionDisponible ObtenerContratosEInformacion(VistaCuposDistribuidos datosContrato, long CuentaPuerto,
            DateTime? fechaDesde, DateTime? fechaHasta, string cosechaDesde, string cosechaHasta, Consignacion Consignacion, string Cyo)
        {
            if (fechaDesde == null) fechaDesde = FiltroPorDefecto.GetFiltroPorDefectoFechaDesde();
            if (fechaHasta == null || fechaHasta == default(DateTime)) fechaHasta = FiltroPorDefecto.GetFiltroPorDefectoFechaHasta();
            if (string.IsNullOrEmpty(cosechaDesde) || cosechaDesde.Trim() == "0") cosechaDesde = FiltroPorDefecto.GetFiltroPorDefectoCosechaDesde();
            if (string.IsNullOrEmpty(cosechaHasta) || cosechaHasta.Trim() == "0") cosechaHasta = FiltroPorDefecto.GetFiltroPorDefectoCosechaHasta();
            IVistaCuposDistribuidosStore store = new VistaCuposDistribuidosStore();
            /*en principio filtramos por cuenta. ya veremos de donde llama a este procedimiento*/
            IVendedorStore consultaVendedor = new VendedorStore();
            DistribucionDisponible Distribucion;
            if (datosContrato.Vendcta != 0 && !consultaVendedor.IsInternal(datosContrato.Vendcta))
            {
                ICuitStore storeCuentas = new CuitStore();
                CuposCuit cuentaCuit = storeCuentas.FindByCuenta(datosContrato.Vendcta);
                datosContrato.Cuitvend = cuentaCuit.Cuit;
                IList<long> CtasVendedores = consultaVendedor.FindVendedorByCuit(datosContrato.Cuitvend).Select(c => c.Cuenta).ToList();
                Distribucion = new DistribucionDisponible(store.FindByFilterOfView(datosContrato, cosechaDesde, cosechaHasta, DateUtils.n_date(fechaDesde), DateUtils.n_date(fechaHasta), LocalType.ContratosPor.Cuit, CtasVendedores));
            }
            else
            {
                Distribucion = new DistribucionDisponible(store.FindByFilterOfView(datosContrato, cosechaDesde, cosechaHasta, DateUtils.n_date(fechaDesde), DateUtils.n_date(fechaHasta), LocalType.ContratosPor.Cuenta, null));
            }
            SetTotalesDiasConsignacionSeleccionada(datosContrato.Compcta, datosContrato.Vendcta, datosContrato.Codproducto, CuentaPuerto, Consignacion, Distribucion, Cyo);
            return Distribucion;
        }

        /// <summary>
        /// Setea los totales de cupos pendientes de distribuir de los cinco dias siguientes, mas el dia de hoy.
        /// Tiene en cuenta la consignación.
        /// </summary>
        /// <param name="CuentaComprador"></param>
        /// <param name="CuentaVendedor"></param>
        /// <param name="CodigoProducto"></param>
        /// <param name="CuentaPuerto"></param>
        /// <param name="Consignacion"></param>
        /// <param name="DistribucionesDisponibles"></param>
        /// <param name="Cyo"></param>
        /// <returns></returns>
        public DistribucionDisponible SetTotalesDiasConsignacionSeleccionada(long CuentaComprador, long CuentaVendedor, int CodigoProducto, long CuentaPuerto,
            Consignacion Consignacion, DistribucionDisponible DistribucionesDisponibles, string Cyo = "FALSE")
        {
            IList<Counter<Cupos>> ConsignacionesAgrupadosPorFecha;
            ICuposStore StoreCupos = new CuposStore();
            if (!string.IsNullOrEmpty(Cyo) && Cyo.ToUpper() == "TRUE")
            {
                ConsignacionesAgrupadosPorFecha = StoreCupos.FindCuposByCompradorVendedorGranoPuertoConsignacion(CuentaComprador, CuentaVendedor, CodigoProducto, CuentaPuerto, Consignacion, true).ToList();
            }
            else
            {
                ConsignacionesAgrupadosPorFecha = StoreCupos.FindCuposByCompradorVendedorGranoPuertoConsignacion(CuentaComprador, CuentaVendedor, CodigoProducto, CuentaPuerto, Consignacion, false).ToList();
            }
            DistribucionesDisponibles.TotalesDisponiblesDias = new List<int>();
            for (int i = 0; i <= CantidadDias; i++)
            {
                DistribucionesDisponibles.TotalesDisponiblesDias.Add(
                    ConsignacionesAgrupadosPorFecha
                        .Where(x => x.Value.Fecha.Date == DateTime.Now.AddDays(i).Date && x.Value.GetConsignacion().Equals(Consignacion))
                        .Sum(x => x.Count)
                );
            }
            return DistribucionesDisponibles;
        }

        /// <summary>
        /// Setea los totales de cupos pendientes de distribuir de los cinco dias siguientes, mas el dia de hoy.
        /// No tiene en cuenta la consignación.
        /// </summary>
        /// <param name="CuentaComprador"></param>
        /// <param name="CuentaVendedor"></param>
        /// <param name="CodigoProducto"></param>
        /// <param name="CuentaPuerto"></param>
        /// <param name="DistribucionesDisponibles"></param>
        /// <param name="Cyo"></param>
        /// <returns></returns>
        public DistribucionDisponible SetTotalesDias(long CuentaComprador, long CuentaVendedor, int CodigoProducto, long CuentaPuerto,
            DistribucionDisponible DistribucionesDisponibles, string Cyo = "FALSE")
        {
            IList<Counter<Cupos>> ConsignacionesAgrupadosPorFecha;
            ICuposStore StoreCupos = new CuposStore();
            if (!string.IsNullOrEmpty(Cyo) && Cyo.ToUpper() == "TRUE")
            {
                ConsignacionesAgrupadosPorFecha = StoreCupos.FindConsignacionesForKeyGroupByFechaCyO(CuentaComprador, CuentaVendedor, CodigoProducto, CuentaPuerto).ToList();
            }
            else
            {
                ConsignacionesAgrupadosPorFecha = StoreCupos.FindConsignacionesForKeyGroupByFecha(CuentaComprador, CuentaVendedor, CodigoProducto, CuentaPuerto).ToList();
            }
            DistribucionesDisponibles.TotalesDisponiblesDias = new List<int>();
            for (int i = 0; i <= CantidadDias; i++ )
            {
                DistribucionesDisponibles.TotalesDisponiblesDias.Add(
                    ConsignacionesAgrupadosPorFecha
                        .Where(x => x.Value.Fecha.Date == DateTime.Now.AddDays(i).Date)
                        .Sum(x => x.Count)
                );
            }
            return DistribucionesDisponibles;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void SumarEncabezado(Cupos Encabezado)
        {
            if (Encabezado != null)
            {
                Encabezado.Cuposotorgados += 1;
            }
        }

        public IList<DateTime> GetFechasModificoDistribucion(VistaCuposDistribuidos TablaModificaciones)
        {
            IList<DateTime> Fechas = new List<DateTime>();
            if (TablaModificaciones.Hoy != 0) Fechas.Add(DateTime.Now.Date);
            for (int Dia = 1; Dia <= CantidadDias; Dia++)
            {
                if (Int32.Parse(TablaModificaciones.GetType().GetProperty("Dia" + Dia).GetValue(TablaModificaciones, null).ToString()) != 0) Fechas.Add(DateTime.Now.Date.AddDays(Dia));
            }
            return Fechas;
        }
    }
}