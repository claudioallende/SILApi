using ResourceServer.Models.DataAccess;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    /// <summary>
    /// Realiza operaciones de modificación, filtrado y busquedas de cupos.
    /// </summary>
    public class ServicioCupo
    {
        private ICuposStore CuposStore { get; set; }

        public ServicioCupo()
        {
            CuposStore = new CuposStore();
        }

        public Cupos ObtenerEncabezado(IList<Cupos> Encabezados, Cupos Cupo)
        {
            return Encabezados.Where(x => x.Id == Cupo.Idorigen).FirstOrDefault();
        }

        public IList<Cupos> ObtenerEncabezados(IList<Cupos> Cupos)
        {
            IList<long> Ids = Cupos.GroupBy(x => x.Idorigen).Select(x => x.Key).ToList();
            return CuposStore.FindEncabezadosByIds(Ids);
        }

        public IList<Cupos> ObtenerEncabezados(IList<Cupos> Cupos, ISession Session)
        {
            IList<long> Ids = Cupos.GroupBy(x => x.Idorigen).Select(x => x.Key).ToList();
            return CuposStore.FindEncabezadosByIds(Ids, Session);
        }

        public IList<EstadoAlfanumericoModel> ObtenerCuposPorCodigosAlfanumericos(IList<string> CodigosAlfanumericos)
        {
            EstadoAlfanumericoStore Store = new EstadoAlfanumericoStore();
            return Store.FindByAlfanumericos(CodigosAlfanumericos);
        }
        /// <summary>
        /// Retorna el conjunto de cupos cuyo ID estan dentro del listado de IDs pasado como parametro
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public IList<DetalleCupoComplete> ObtenerCuposPorIds(IList<long> Ids)
        {
            List<DetalleCupoComplete> listaDeCupos = new List<DetalleCupoComplete>();
            if (Ids != null && Ids.Count > 0)
            {
                IList<IList<long>> IdsPaginados = PaginateList<long>(Ids, 990);
                foreach (IList<long> IdsBusqueda in IdsPaginados)
                {
                    if (IdsBusqueda != null && IdsBusqueda.Count > 0)
                    {
                        listaDeCupos.AddRange(CuposStore.FindCuposForId(IdsBusqueda));
                    }
                }
            }
            return AgruparPorDiaDetalleCupos(listaDeCupos);
        }

        public IList<IList<T>> PaginateList<T>(IList<T> elements, int max)
        {
            IList<IList<T>> pagination = new List<IList<T>>();
            int pages = elements.Count / max;
            if (elements.Count % max > 0)
                pages++;
            for (int i = 0; i < pages; i++)
            {
                pagination.Add(elements.Skip(i * max).Take(max).ToList());
            }
            return pagination;
        }
        /// <summary>
        /// Retorna el conjunto de cupos distribuidos e informados en el periodo ingresado
        /// </summary>
        /// <param name="CodigosAlfanumericos"></param>
        /// <returns></returns>
        public IList<DetalleCupoComplete> ObtenerCuposDistribuidosEInformadosEnPeriodo(DateTime desde, DateTime hasta, IList<long> CuentasVendedoras = null)
        {
            IList<DetalleCupoComplete> listaDeCupos = new List<DetalleCupoComplete>();
            if (desde != null && hasta != null && desde <= hasta) {
                listaDeCupos = CuposStore.FindCuposDistribuidosInformadosBetweenDates(desde, hasta, CuentasVendedoras);
                
            }
            return AgruparPorDiaDetalleCupos(listaDeCupos);
        }

        public IList<DetalleCupoComplete> AgruparPorDiaDetalleCupos(IList<DetalleCupoComplete> lista)
        {
            //DetalleCuposCliente Detalle = new DetalleCuposCliente();
            IList<DetalleCupoComplete> ListaDetalles = new List<DetalleCupoComplete>();
            if (lista != null && lista.Count > 0)
            {
                var group = lista.GroupBy(x => new
                {
                    x.Comprador,
                    x.Vendedor,
                    x.Puerto,
                    x.Grano,
                    x.Consignacion,
                    x.CodigoEstablecimientoProcedencia
                });
                var listaAlfas = lista.SelectMany(y => y.AlfanumericosPorDia);
                var result = group.Select(x => new DetalleCupoComplete
                {
                    Comprador = x.Key.Comprador,
                    Vendedor = x.Key.Vendedor,
                    Puerto = x.Key.Puerto,
                    Grano = x.Key.Grano,
                    Consignacion = x.Key.Consignacion,
                    AlfanumericosPorDia = x.SelectMany(y => y.AlfanumericosPorDia).GroupBy(y => y.Dia)
                            .Select(y => new AlfanumericosDia
                            {
                                Dia = y.Key.Date,
                                Alfanumericos = y.SelectMany(z => z.Alfanumericos).ToList()
                            })
                            .OrderBy(w => w.Dia)
                            .ToList(),
                    CodigoEstablecimientoProcedencia = x.Key.CodigoEstablecimientoProcedencia
                });

                ListaDetalles = result.ToList();
            }
            return ListaDetalles;
        }

        /// <summary>
        /// Busca en base de datos los cupos padre cyo. Filtra los que tienen status = 5. Los actualiza al estado 4.
        /// </summary>
        /// <param name="Cupos"></param>
        /// <param name="Session"></param>
        /// <returns>Lista de cupos padre cyo modificados</returns>
        public IList<Cupos> ObtenerYLiberarCuposPadreCYO(IList<Cupos> Cupos, ISession Session)
        {
            IList<Cupos> CuposHijo = FiltrarCuposCuentaYOrdenHijoNoAnulados(Cupos);
            IList<Cupos> CuposPadre = GetCuposPadreCyO(CuposHijo, Session);
            return LiberarCuposPadreCYO(CuposPadre, Session);
        }

        public IList<Cupos> LiberarCuposPadreCYO(IList<Cupos> CuposPadres, ISession Session)
        //public IList<Cupos> LiberarCuposPadreCYO(IList<CupoCuentaYOrden> CuposCuentaYOrden, ISession Session)
        {
            IList<Cupos> CuposPadreModificados = new List<Cupos>();
            foreach (Cupos Cupo in CuposPadres)
            {
                if (Cupo.Status == 5)
                {
                    Cupo.Status = 4;
                    CuposPadreModificados.Add(Cupo);
                    Cupo.Estado = new ResourceServer.Models.Cupo.CYO.EstadoDistribuidoPendienteDistribuirHijo(Cupo);
                }
            }
            this.CuposStore.Update(CuposPadreModificados, Session);
            return CuposPadreModificados;
        }

        public IList<Cupos> ObtenerCuposRelacionados(IList<Cupos> Cupos, ISession Session)
        {
            List<Cupos> CuposRelacionados = new List<Cupos>();
            CuposRelacionados.AddRange(ObtenerCuposCuentaYOrdenHijo(Cupos, Session));
            CuposRelacionados.AddRange(GetCuposPadreCyO(Cupos, Session));
            return CuposRelacionados;
        }

        public IList<RelacionDistribucionCupos> AgruparCuposPorDistribucion(IList<Cupos> Cupos, IList<CuposDist> Distribuciones)
        {
            IList<RelacionDistribucionCupos> CuposAgrupadosPorDistribucion = new List<RelacionDistribucionCupos>();
            foreach (CuposDist Distribucion in Distribuciones)
            {
                CuposAgrupadosPorDistribucion.Add(new RelacionDistribucionCupos
                {
                    CuposDeDistribucion = Cupos.Where(x => x.Uvcupodist == Distribucion.Uvalue).ToList(),
                    Distribucion = Distribucion
                });
            }
            return CuposAgrupadosPorDistribucion;
        }

        [Obsolete]
        public IList<Cupos> ObtenerCuposPadreCyo(IList<Cupos> Cupos, ISession Session)
        {
            ICuposStore Store = new CuposStore();
            List<Cupos> CuposPadre = new List<Cupos>();
            IList<IList<Cupos>> ListaDeCuposPorFecha = ListaCuposPorFecha(Cupos);
            foreach (var CuposDeFecha in ListaDeCuposPorFecha)
            {
                CuposPadre.AddRange(Store.FindByCompAndPuertoAndGranoAndAlfaAndFechaAndVendcyo(CuposDeFecha.ElementAt(0).Vendcta, CuposDeFecha.ElementAt(0).Puerto, CuposDeFecha.ElementAt(0).Grano, CuposDeFecha.Select(x => x.Nrocupo).ToList(), CuposDeFecha.ElementAt(0).Fecha.Date, CuposDeFecha.ElementAt(0).Vendcyo, Session));
            }
            return CuposPadre;
        }

        [Obsolete]
        public IList<IList<Cupos>> ListaCuposPorFecha(IList<Cupos> Cupos)
        {
            IList<IList<Cupos>> CuposPorFecha = new List<IList<Cupos>>();
            for (var i = 0; i <= 20; i++)
            {
                CuposPorFecha.Add(Cupos.Where(x => x.Fecha.Date == DateTime.Now.AddDays(i).Date).ToList());
            }
            return CuposPorFecha;
        }

        public IList<Cupos> FiltrarCuposCuentaYOrdenDistribuidos(IList<Cupos> ListaCupos)
        {
            return ListaCupos.Where(x => 
                x.GetEstado().Codigo == Cupo.CodigoEstado.CuentaYOrdenDistribuidoPendienteDistribuirHijo ||
                x.GetEstado().Codigo == Cupo.CodigoEstado.CuentaYOrdenDistribuidoConHijoDistribuido
            ).ToList();
        }

        public IList<Cupos> FiltrarCuposCuentaYOrdenPadreNoAnulados(IList<Cupos> ListaCupos)
        {
            return ListaCupos.Where(x =>
                x.Vendcta != 0 && x.Vendcta == x.Vendcyo && x.Status != 3
            ).ToList();
        }

        public IList<Cupos> FiltrarCuposCuentaYOrdenPadreDistribuidosSinHijoDistribuido(IList<Cupos> ListaCupos)
        {
            return ListaCupos.Where(x =>
                x.GetEstado().Codigo == Cupo.CodigoEstado.CuentaYOrdenDistribuidoPendienteDistribuirHijo
            ).ToList();
        }

        public IList<Cupos> FiltrarCuposCuentaYOrdenPadreDistribuidosConHijoDistribuido(IList<Cupos> ListaCupos)
        {
            return ListaCupos.Where(x =>
                x.GetEstado().Codigo == Cupo.CodigoEstado.CuentaYOrdenDistribuidoConHijoDistribuido
            ).ToList();
        }

        public IList<Cupos> FiltrarNoCuposCuentaYOrdenPadreDistribuidosConHijoDistribuido(IList<Cupos> ListaCupos)
        {
            return ListaCupos.Where(x =>
                x.GetEstado().Codigo != Cupo.CodigoEstado.CuentaYOrdenDistribuidoConHijoDistribuido
            ).ToList();
        }

        public IList<Cupos> FiltrarCuposCuentaYOrdenHijoNoAnulados(IList<Cupos> ListaCupos)
        {
            return ListaCupos.Where(x =>
                x.Vendcyo != 0 && x.Status != 3 && x.Compcta == x.Vendcyo
            ).ToList();
        }

        /// <summary>
        /// Evalua que los parametros sean correctos y asocia en un objeto CupoCuentaYOrden la relacion entre padre e hijo.
        /// </summary>
        /// <param name="CuposPadre">Se filtran los cupos padre cyo</param>
        /// <param name="CuposHijo">Se filtran los cupos hijo cyo</param>
        /// <returns></returns>
        public IList<CupoCuentaYOrden> AsociarCupoPadreHijo(IList<Cupos> CuposPadre, IList<Cupos> CuposHijo)
        {
            IList<CupoCuentaYOrden> CuposCuentaYOrden = CuposPadre
                .Where(x => x.EsCuentaYOrdenPadre())
                .Join(CuposHijo, padre => padre.Nrocupo, hijo => hijo.Nrocupo, (padre, hijo) => new { CupoPadre = padre, CupoHijo = hijo })
                .Where(x => x.CupoHijo.EsCuentaYOrdenHijo())
                .Select(x => new CupoCuentaYOrden
                {
                    CupoPadre = x.CupoPadre,
                    CupoHijo = x.CupoHijo
                })
                .ToList();
            return CuposCuentaYOrden;
        }

        /// <summary>
        /// Obtiene de base de datos los Cupos Padre de los cupos pasados como parametro.
        /// </summary>
        /// <param name="Cupos"></param>
        /// <param name="Session"></param>
        /// <returns></returns>
        public IList<Cupos> GetCuposPadreCyO(IList<Cupos> Cupos, ISession Session)
        {
            IList<Cupos> CuposCYOHijos = FiltrarCuposCuentaYOrdenHijoNoAnulados(Cupos);
            List<Cupos> CuposPadre = new List<Cupos>();
            if (CuposCYOHijos.Count > 0)
            {
                ICuposStore Store = new CuposStore();
                Cupos CupoHijo = CuposCYOHijos.ElementAt(0);
                IList<DateTime> Fechas = GetFechasCupos(CuposCYOHijos);
                foreach (DateTime Fecha in Fechas)
                {
                    CuposPadre.AddRange(Store.FindByVendAndPuertoAndGranoAndVendcyoAndFechaAndAlfas(
                        CupoHijo.Compcta, 
                        CupoHijo.Puerto, 
                        CupoHijo.Grano, 
                        GetCuposFecha(Cupos, Fecha).Select(x => x.Nrocupo).ToList(), 
                        Fecha, 
                        CupoHijo.Vendcyo, 
                        Session)
                    );
                }
            }
            return CuposPadre;
        }

        public IList<Cupos> GetCuposPadreCyOConHijosPendienteInformar(IList<Cupos> Cupos, ISession Session)
        {
            IList<Cupos> CuposCYOHijos = FiltrarCuposCuentaYOrdenHijoNoAnulados(Cupos);
            List<Cupos> CuposPadre = new List<Cupos>();
            if (CuposCYOHijos.Count > 0)
            {
                ICuposStore Store = new CuposStore();
                Cupos CupoHijo = CuposCYOHijos.ElementAt(0);
                IList<DateTime> Fechas = GetFechasCupos(CuposCYOHijos);
                foreach (DateTime Fecha in Fechas)
                {
                    CuposPadre.AddRange(Store.FindByVendAndPuertoAndGranoAndVendcyoAndFechaAndAlfasAndUvalueNotZero(
                        CupoHijo.Compcta,
                        CupoHijo.Puerto,
                        CupoHijo.Grano,
                        GetCuposFecha(Cupos, Fecha).Select(x => x.Nrocupo).ToList(),
                        Fecha,
                        CupoHijo.Vendcyo,
                        Session)
                    );
                }
            }
            return CuposPadre;
        }

        public IList<Cupos> GetCuposFecha(IList<Cupos> Cupos, DateTime Fecha)
        {
            IList<Cupos> CuposFecha = Cupos.Where(x => x.Fecha.Date == Fecha.Date).ToList();
            return CuposFecha;
        }

        public IList<DateTime> GetFechasCupos(IList<Cupos> Cupos)
        {
            IList<DateTime> Fechas = Cupos.GroupBy(x => x.Fecha.Date).Select(x => x.Key.Date).ToList();
            return Fechas;
        }

        public IList<Cupos> FiltrarCuposPadreCYOEliminarCuposHijo(IList<Cupos> Cupos, ISession Session)
        {
            EliminarCuposHijoCYO(Cupos, Session);
            return FiltrarCuposPadreCYO(Cupos);
        }

        /// <summary>
        /// De la lista filtra los cupos que son cuenta y orden padre
        /// </summary>
        /// <param name="Cupos"></param>
        /// <returns></returns>
        public IList<Cupos> FiltrarCuposPadreCYO(IList<Cupos> Cupos)
        {
            return Cupos.Where(x => x.EsCuentaYOrdenPadre()).ToList();
        }

        /// <summary>
        /// De la lista Cupos elimina los cupos que sean hijo cyo no informados. En caso de que estén informados se anula la distribucion
        /// </summary>
        /// <param name="Cupos"></param>
        /// <param name="Session"></param>
        public void EliminarCuposHijoCYOYAnulaDistribuionPendienteInformar(IList<Cupos> Cupos, string Motivo, ISession Session)
        {
            IList<Cupos> CuposHijoPendienteInformar = Cupos.Where(x => x.Pdf == 0 && x.EsCuentaYOrdenHijo() && x.GetEstado().Codigo == Cupo.CodigoEstado.DistribuidoInformado).ToList();
            foreach (var Cupo in CuposHijoPendienteInformar)
            {
                Cupo.GetEstado().AnularDistribucion(Motivo, null, Session);
            }
            this.EliminarCuposHijoCYO(CuposHijoPendienteInformar, Session);
        }

        /// <summary>
        /// De la lista Cupos elimina los cupos que sean hijo cyo no informados. En caso de que estén informados se anula el cupo
        /// </summary>
        /// <param name="Cupos"></param>
        /// <param name="Session"></param>
        public void EliminarCuposHijoCYOYAnulaCupoPendienteInformar(IList<Cupos> Cupos, string Motivo, ISession Session)
        {
            IList<Cupos> CuposHijoPendienteInformar = Cupos.Where(x => x.Pdf == 0 && x.EsCuentaYOrdenHijo() && x.GetEstado().Codigo == Cupo.CodigoEstado.DistribuidoInformado).ToList();
            foreach (var Cupo in CuposHijoPendienteInformar)
            {
                Cupo.GetEstado().AnularCupo(Motivo, null, Session);
            }
            this.EliminarCuposHijoCYO(CuposHijoPendienteInformar, Session);
        }

        /// <summary>
        /// Elimina solo los cupos hijo de los padres cyo que recive como parametro que esten en status 4.
        /// </summary>
        /// <param name="ListaCuposPadre"></param>
        /// <param name="Session"></param>
        public void EliminarCuposHijoCYOFromPadres(IList<Cupos> ListaCuposPadre, ISession Session)
        {
            IList<Cupos> CuposHijoCYONoDistribuido = GetCuposHijoCYONoDistribuido(ListaCuposPadre, Session);
            EliminarCuposHijoCYO(CuposHijoCYONoDistribuido, Session);
        }

        public void EliminarCuposHijoCYO(IList<Cupos> Cupos, ISession Session)
        {
            this.CuposStore.Delete(Cupos.Where(x => x.EsCuentaYOrdenHijo() && x.Pdf == 0).ToList(), Session);
        }
        /// <summary>
        /// Filtra los cupos padre de la lista de cupos que recive como parametro y busca los cupos cuenta y orden hijos en base de datos.
        /// </summary>
        /// <param name="ListaCupos"></param>
        /// <param name="Session"></param>
        /// <returns></returns>
        public IList<Cupos> ObtenerCuposCuentaYOrdenHijo(IList<Cupos> ListaCupos, ISession Session)
        {
            IList<Cupos> CuposPadre = FiltrarCuposCuentaYOrdenPadreNoAnulados(ListaCupos);
            List<Cupos> CuposHijo = new List<Cupos>();
            if (CuposPadre.Count > 0)
            {
                ICuposStore Store = new CuposStore();
                var CuposPadreAgrupadosPorFecha = CuposPadre.GroupBy(x => x.Fecha);
                foreach (var Cupos in CuposPadreAgrupadosPorFecha)
                {
                    CuposHijo.AddRange(Store.FindByNroCupoAndCompctaAndVendcyoAndNotStatusAndFecha(Cupos.Select(x => x.Nrocupo).ToList(), Cupos.Key, 3, Session));
                }
            }
            return CuposHijo;
        }
        public IList<Cupos> GetCuposHijoCYONoDistribuido(IList<Cupos> CuposPadre, ISession Session)
        {
            IList<Cupos> CuposPadreSinHijoDistribuido = FiltrarCuposCuentaYOrdenPadreDistribuidosSinHijoDistribuido(CuposPadre);
            IList<Cupos> CuposHijoCYONoDistribuido = new List<Cupos>();
            if (CuposPadreSinHijoDistribuido.Count > 0)
            {
                Cupos CupoPadre = CuposPadreSinHijoDistribuido.ElementAt(0);
                ICuposStore Store = new CuposStore();
                CuposHijoCYONoDistribuido = Store.FindByCompAndVendAndGranoAndFechaAndPuertoAndIdOrigenAndAlfaAndStatus(
                    CupoPadre.Vendcyo, 
                    0, 
                    CupoPadre.Grano, 
                    CupoPadre.Puerto,
                    CuposPadreSinHijoDistribuido.GroupBy(x => x.Fecha).Select(x => x.Key.Date).ToList(),
                    CuposPadreSinHijoDistribuido.Select(x => x.Nrocupo).ToList(),
                    0,
                    Session
                );
            }
            return CuposHijoCYONoDistribuido;
        }
        public IList<Cupos> DevolverCupoCYOANormal(IList<Cupos> ListaCupos, long CuentaVendedor, int Cantidad, ISession Session)
        {
            throw new NotImplementedException();
        }
        public IList<Cupos> GetCuposTransformarSiVendedorEsCYO(IList<Cupos> ListaCupos, long CuentaVendedor, int Cantidad, ISession Session)
        {
            IList<Cupos> Cupos = TransformarCuposACYO(ListaCupos, CuentaVendedor, Cantidad, Session);
            if (Cupos.Count == 0)
            {
                Cupos = ListaCupos.Take(Cantidad).ToList();
            }
            return Cupos;
        }
        public IList<Cupos> TransformarCuposACYO(IList<Cupos> ListaCupos, long CuentaVendedor, int Cantidad, ISession Session)
        {
            IList<Cupos> CuposATransformarEnCYO = GetCuposNormalesConVendedorCYO(ListaCupos, CuentaVendedor, Session).Take(Cantidad).ToList();
            foreach (Cupos Cupo in CuposATransformarEnCYO)
            {
                Cupo.Vendcyo = CuentaVendedor;
                Cupo.Estado = new Models.Cupo.CYO.EstadoCreado(Cupo);
            }
            return CuposATransformarEnCYO;
        }
        public IList<Cupos> GetCuposNormalesConVendedorCYO(IList<Cupos> ListaCupos, long CuentaVendedor, ISession Session)
        {
            IList<Cupos> CuposNormalesConVendedorCYO = new List<Cupos>();
            IList<Cupos> CuposNormales = FiltrarCuposNormales(ListaCupos);
            IDTablaStore DtablaStore = new DTablaStore();
            //IList<ICuenta> Vendedores = CuposNormales.Select(x => new Vendedor { Cuenta = x.Vendcta }).ToList<ICuenta>();
            DTabla Entidad = DtablaStore.FindByEntidadAndOrdenAndValor(
                "CUPCYO",
                "CYO01",
                CuentaVendedor.ToString(),
                Session
            );
            if (Entidad != null)
            {
                CuposNormalesConVendedorCYO = CuposNormales;
            }
            return CuposNormalesConVendedorCYO;
        }
        public IList<Cupos> FiltrarCuposNormales(IList<Cupos> ListaCupos)
        {
            return ListaCupos.Where(x => x.Vendcyo == 0).ToList();
        }
        /// <summary>
        /// Obtiene una lista de listas de agrupaciones de cupos por fecha
        /// </summary>
        /// <param name="Cupos"></param>
        /// <returns></returns>
        public IList<List<Cupos>> ObtenerListaDeListaCuposAgrupadosPorFecha(IList<Cupos> Cupos)
        {
            IList<List<Cupos>> ListaDeListaCuposAgrupadosPorFecha = Cupos.GroupBy(cupo => cupo.Fecha)
                .Select(cupos => cupos.ToList())
                .ToList();
            return ListaDeListaCuposAgrupadosPorFecha;
        }
        /// <summary>
        /// Obtiene cupos repetidos en base de datos
        /// </summary>
        /// <param name="CuentaPuerto"></param>
        /// <param name="CodigoProducto"></param>
        /// <param name="Alfanumericos"></param>
        /// <param name="Fecha"></param>
        /// <param name="Session"></param>
        /// <returns></returns>
        public IList<Cupos> GetCuposRepetidos(long CuentaPuerto, int CodigoProducto, IList<CodigosAlfanumericos> Codigos, ISession Session)
        {
            ICuposStore Store = new CuposStore();
            return Store.FindByPuertoAndGranoAndFechaAndAlfas(CuentaPuerto, CodigoProducto, Codigos, Session);
        }

        public bool ValidaCompradorYVendedorCupo(long CuentaComprador, long CuentaVendedor)
        {
            return (CuentaComprador != CuentaVendedor);
        }

        public List<long> GetVendedoresConCuposPendDeCTG()
        {
            ICuposStore Store = new CuposStore();
            List<long> vendedores = Store.FindVendedoresConCuposPendDeCTG().ToList<long>();
            return vendedores;
        }

        public List<Vendedor> GetVendedoresConTurnosPendientesDeCTG()
        {
            ICuposStore Store = new CuposStore();
            List<long> VendedoresConPendiente = Store.FindVendedoresConCuposPendDeCTG().ToList<long>();
            IVendedorStore StoreVend = new VendedorStore();
            IList<Vendedor> listaDeVendedores = StoreVend.FindAll();
            return listaDeVendedores.Where(x => VendedoresConPendiente.Contains(x.Cuenta)).ToList<Vendedor>();
        }

        public IList<Cupos> FiltrarCuposPadreCYOYAnularlos(IList<Cupos> CuposPadre, ISession Session)
        {
            IList<Cupos> CuposPadreFiltrados = FiltrarCuposPadreCYO(CuposPadre);
            foreach (Cupos Cupo in CuposPadreFiltrados)
            {
                Cupo.Status = 3;
            }
            this.CuposStore.Update(CuposPadreFiltrados, Session);
            return CuposPadreFiltrados;
        }

        public void AnularDistribucionesCuposPadre(IList<Cupos> Cupos, string Motivo, IList<Cupos> Encabezados, ISession Session)
        {
            IList<Cupos> CuposPadre = FiltrarCuposPadreCYO(Cupos);
            foreach (Cupos Cupo in CuposPadre)
            {
                Cupo.GetEstado().AnularDistribucion(Motivo, ObtenerEncabezado(Encabezados, Cupo), Session);
            }
        }

        public void SetearCuposHijoAPadreCYO(IList<Cupos> Cupos, ISession Session)
        {
            IList<Cupos> CuposRelacionados = ObtenerCuposRelacionados(Cupos, Session);
            SetearCuposHijoAPadreCYO(Cupos, CuposRelacionados, Session);
        }

        public void SetearCuposHijoAPadreCYO(IList<Cupos> Cupos, IList<Cupos> CuposRelacionados, ISession Session)
        {
            //Obtener CuposPadre de las 2 listas
            List<Cupos> CuposPadre = Cupos.Where(x => x.EsCuentaYOrdenPadre()).ToList();
            CuposPadre.AddRange(CuposRelacionados.Where(x => x.EsCuentaYOrdenPadre()));
            //Obtener CuposHijo de las 2 listas
            List<Cupos> CuposHijo = Cupos.Where(x => x.EsCuentaYOrdenHijo()).ToList();
            CuposHijo.AddRange(CuposRelacionados.Where(x => x.EsCuentaYOrdenHijo()));
            //Asociar Padre con Hijo
            IList<CupoCuentaYOrden> CuposCuentaYOrden = AsociarCupoPadreHijo(CuposPadre, CuposHijo);
            //Setear al CupoPadre el CupoHijo
            foreach (CupoCuentaYOrden Cupo in CuposCuentaYOrden)
            {
                Cupo.CupoPadre.CupoHijoCYO = Cupo.CupoHijo;
                Cupo.CupoHijo.CupoPadreCYO = Cupo.CupoPadre;
            }
        }

        public IList<Cupos> ObtenerCuposPadreCYOYNormales(IList<Cupos> Cupos)
        {
            List<Cupos> Lista = new List<Cupos>();
            Lista.AddRange(Cupos.Where(x => x.EsCuentaYOrdenHijo()).Select(x => x.CupoPadreCYO).ToList());
            Lista.AddRange(Cupos.Where(x => x.EsCuentaYOrdenPadre() || !x.EsCuentaYOrden()).ToList());
            return Lista;
        }

        public IList<CuposPorConsignacionYFechaDTO> AgruparCuposPorConsignacion(IList<Cupos> Cupos)
        {
            IList<CuposPorConsignacionYFechaDTO> Result = new List<CuposPorConsignacionYFechaDTO>();
            var CuposAgrupados = Cupos
                .GroupBy(x =>
                    new
                    {
                        Cuitsolicitante = x.Cuitsolicitante,
                        Nomsolicitante = x.Nomsolicitante,
                        Cuitintermediario = x.Cuitintermediario,
                        Nomintermediario = x.Nomintermediario,
                        Cuitrtecomercial = x.Cuitrtecomercial,
                        Nomrtecomercial = x.Nomrtecomercial,
                        Cuitcorrcomp = x.Cuitcorrcomp,
                        Nomcorrcomp = x.Nomcorrcomp,
                        Cuitmat = x.Cuitmat,
                        Nommat = x.Nommat,
                        Cuitcorrvta = x.Cuitcorrvta,
                        Nomcorrvta = x.Nomcorrvta,
                        Cuitrteent = x.Cuitrteent,
                        Nomrteent = x.Nomrteent,
                        Cuitdestinatario = x.Cuitdestinatario,
                        Nomdestinatario = x.Nomdestinatario,
                        Fecha = x.Fecha,
                        CondicionGrano = x.CondicionGrano
                    }
                );

            return CuposAgrupados.Select(x => new CuposPorConsignacionYFechaDTO
            {
                Consignacion = new Consignacion {
                    Cuitsolicitante = x.Key.Cuitsolicitante,
                    Nomsolicitante = x.Key.Nomsolicitante,
                    Cuitintermediario = x.Key.Cuitintermediario,
                    Nomintermediario = x.Key.Nomintermediario,
                    Cuitrtecomercial = x.Key.Cuitrtecomercial,
                    Nomrtecomercial = x.Key.Nomrtecomercial,
                    Cuitcorrcomp = x.Key.Cuitcorrcomp,
                    Nomcorrcomp = x.Key.Nomcorrcomp,
                    Cuitmat = x.Key.Cuitmat,
                    Nommat = x.Key.Nommat,
                    Cuitcorrvta = x.Key.Cuitcorrvta,
                    Nomcorrvta = x.Key.Nomcorrvta,
                    Cuitrteent = x.Key.Cuitrteent,
                    Nomrteent = x.Key.Nomrteent,
                    Cuitdestinatario = x.Key.Cuitdestinatario,
                    Nomdestinatario = x.Key.Nomdestinatario,
                    CondicionGrano = x.Key.CondicionGrano
                },
                Fecha = x.Key.Fecha,
                Cupos = x.Where(y =>
                        y.Cuitsolicitante == x.Key.Cuitsolicitante &&
                        y.Nomsolicitante == x.Key.Nomsolicitante &&
                        y.Cuitintermediario == x.Key.Cuitintermediario &&
                        y.Nomintermediario == x.Key.Nomintermediario &&
                        y.Cuitrtecomercial == x.Key.Cuitrtecomercial &&
                        y.Nomrtecomercial == x.Key.Nomrtecomercial &&
                        y.Cuitcorrcomp == x.Key.Cuitcorrcomp &&
                        y.Nomcorrcomp == x.Key.Nomcorrcomp &&
                        y.Cuitmat == x.Key.Cuitmat &&
                        y.Nommat == x.Key.Nommat &&
                        y.Cuitcorrvta == x.Key.Cuitcorrvta &&
                        y.Nomcorrvta == x.Key.Nomcorrvta &&
                        y.Cuitrteent == x.Key.Cuitrteent &&
                        y.Nomrteent == x.Key.Nomrteent &&
                        y.Cuitdestinatario == x.Key.Cuitdestinatario &&
                        y.Nomdestinatario == x.Key.Nomdestinatario &&
                        y.Fecha == x.Key.Fecha &&
                        y.CondicionGrano == x.Key.CondicionGrano
                    ).ToList()
            })
            .ToList();
        }
        //public IList<DetalleCuposCliente> ObtenerCupos(IList<long> CuentaVendedor, DateTime? FechaDesde = null, DateTime? FechaHasta = null)
        //{

        //}
    }
}