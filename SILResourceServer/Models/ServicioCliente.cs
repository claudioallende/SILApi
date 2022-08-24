using NHibernate;
using ResourceServer.Models.Cliente;
using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Filtro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ResourceServer.Models
{
    public class ServicioCliente
    {
        private CuposAgrupadosPorVendedorStore storeCuposAgrup { get; set; }
        private ISession session;

        public ServicioCliente() 
        { 
            this.storeCuposAgrup = new CuposAgrupadosPorVendedorStore();
            this.session = HibernateUtil.OpenSession("");
        }

        public List<CuposAgrupadosPorVendedor> ObtenerByVendedor(Int64 vendcta)
        {
            List<CuposAgrupadosPorVendedor> listaResult = new List<CuposAgrupadosPorVendedor>();
            if (vendcta != null && vendcta != 0)
            {
                listaResult = this.storeCuposAgrup.FindAllByVendedor(vendcta, this.session).ToList();
            }
            return listaResult;
        }
        public IList<CuposAgrupadosPorVendedor> ObtenerInformadosEnStop(IList<long> vendcta)
        {
            IList<CuposAgrupadosPorVendedor> listaResult = new List<CuposAgrupadosPorVendedor>();
            listaResult = this.storeCuposAgrup.FindByCuitOrCuentaAndInformadoStop(vendcta, true, this.session);
            return listaResult;
        }
        public IList<CuposAgrupadosPorVendedor> ObtenerNoInformadosEnStop(IList<long> vendcta)
        {
            IList<CuposAgrupadosPorVendedor> listaResult = new List<CuposAgrupadosPorVendedor>();
            listaResult = this.storeCuposAgrup.FindByCuitOrCuentaAndInformadoStop(vendcta, false, this.session);
            return listaResult;
        }
        public IList<CuposAgrupadosPorVendedor> ObtenerInformadosEnStopNoCyo(IList<long> vendcta, GranoCompradorPuerto filtro = null)
        {
            return this.storeCuposAgrup.FindByCuitOrCuentaAndInformadoStopAndCyo(vendcta, true, false, this.session, filtro);
        }
        public IList<CuposAgrupadosPorVendedor> ObtenerNoInformadosEnStopNoCyo(IList<long> vendcta, GranoCompradorPuerto filtro = null)
        {
            return this.storeCuposAgrup.FindByCuitOrCuentaAndInformadoStopAndCyo(vendcta, false, false, this.session, filtro);
        }
        public IList<CuposAgrupadosPorVendedor> ObtenerInformadosEnStopCyo(IList<long> vendcta, GranoCompradorPuerto filtro = null)
        {
            return this.storeCuposAgrup.FindByCuitOrCuentaAndInformadoStopAndCyo(vendcta, true, true, this.session, filtro);
        }
        public IList<CuposAgrupadosPorVendedor> ObtenerNoInformadosEnStopCyo(IList<long> vendcta, GranoCompradorPuerto filtro = null)
        {
            return this.storeCuposAgrup.FindByCuitOrCuentaAndInformadoStopAndCyo(vendcta, false, true, this.session, filtro);
        }
        public IList<DetalleCuposCliente> ObtenerDetalleCuposCliente(long CuentaComprador, long CuentaPuerto, int CodigoGrano, Consignacion Consignacion, bool InformadoStop, bool EsCYO)
        {
            //DetalleCuposCliente Detalle = new DetalleCuposCliente();
            IList<DetalleCuposCliente> ListaDetalles = new List<DetalleCuposCliente>();
            if (CuentaComprador != 0 && CuentaPuerto != 0 && CodigoGrano != 0 && !Consignacion.IsNullOrEmpty())
            {
                CuposStore Store = new CuposStore();
                var query = Store.FindDetalleCuposCliente(ClaimsUtil.GetCuitOrCuenta(), CuentaComprador, CuentaPuerto, CodigoGrano, Consignacion, InformadoStop, EsCYO);
                var group = query.GroupBy(x => new
                    {
                        x.Compcta,
                        x.Vendcta,
                        x.Puerto,
                        x.NombrePuerto,
                        x.Planta,
                        x.DireccionPlanta,
                        x.CPostal,
                        x.CuitPuerto,
                        x.Grano,
                        x.NombreGrano,
                        x.Nomsolicitante,
                        x.Cuitsolicitante,
                        x.Nomintermediario,
                        x.Cuitintermediario,
                        x.Nomrtecomercial,
                        x.Cuitrtecomercial,
                        x.Nomcorrcomp,
                        x.Cuitcorrcomp,
                        x.Nommat,
                        x.Cuitmat,
                        x.Nomrteent,
                        x.Cuitrteent,
                        x.Nomcorrvta,
                        x.Cuitcorrvta,
                        x.Nomdestinatario,
                        x.Cuitdestinatario
                    });
                var result = group.Select(x => new DetalleCuposCliente
                    {
                        CuentaComprador = x.Key.Compcta,
                        CuentaVendedor = x.Key.Vendcta,
                        Puerto = new Puerto {
                            Cuenta = x.Key.Puerto,
                            Cuit = x.Key.CuitPuerto,
                            Nombre = x.Key.NombrePuerto,
                            Domicilio = x.Key.DireccionPlanta,
                            Ruca = x.Key.Planta,
                            Cpostal = x.Key.CPostal
                        },
                        Grano = new Grano {
                            CodigoGrano = x.Key.Grano,
                            Nombre = x.Key.NombreGrano,
                        },
                        Consignacion = new Consignacion
                        {
                            Nomsolicitante = x.Key.Nomsolicitante,
                            Cuitsolicitante = x.Key.Cuitsolicitante,
                            Nomintermediario = x.Key.Nomintermediario,
                            Cuitintermediario = x.Key.Cuitintermediario,
                            Nomrtecomercial = x.Key.Nomrtecomercial,
                            Cuitrtecomercial = x.Key.Cuitrtecomercial,
                            Nomcorrcomp = x.Key.Nomcorrcomp,
                            Cuitcorrcomp = x.Key.Cuitcorrcomp,
                            Nommat = x.Key.Nommat,
                            Cuitmat = x.Key.Cuitmat,
                            Nomrteent = x.Key.Nomrteent,
                            Cuitrteent = x.Key.Cuitrteent,
                            Nomcorrvta = x.Key.Nomcorrvta,
                            Cuitcorrvta = x.Key.Cuitcorrvta,
                            Nomdestinatario = x.Key.Nomdestinatario,
                            Cuitdestinatario = x.Key.Cuitdestinatario
                        },
                        AlfanumericosPorDia = x.GroupBy(y => y.Fecha)
                            .Select(y => new AlfanumericosDia
                            {
                                Dia = y.Key.Date,
                                Alfanumericos = y.Select(z => new InformacionAlfanumerico
                                {
                                    Alfanumerico = z.Nrocupo,
                                    EsCuentaYOrden = (z.Vendcyo != 0),
                                    EstaConsumido = (z.EstadoCupoCNRT > 1),
                                    Observacion = z.Observa
                                })
                                .ToList()
                            })
                            .OrderBy(w => w.Dia)
                            .ToList()                            
                    });

                ListaDetalles = result.ToList();
                //En el objeto query tengo los datos del cupo hijo.
                if (EsCYO)
                {
                    foreach(DetalleCuposCliente Detalle in ListaDetalles)
                        SetConsignacionVendedorCYO(Detalle.CuentaVendedor, Detalle.Consignacion);
                }
            }
            return ListaDetalles;
        }

        public void SetConsignacionVendedorCYO(long CuentaVendedor, Consignacion Consignacion)
        {
            ICuentaStore Store = new VendedorStore();
            ICuenta DatosVendedor = Store.FindNombreAndCuitByCuenta(CuentaVendedor);
            Consignacion.Nomsolicitante = DatosVendedor.Nombre;
            Consignacion.Cuitsolicitante = DatosVendedor.Cuit.Insert(2, "-").Insert(DatosVendedor.Cuit.Length, "-");
        }

        public IList<Grano> ObtenerGranos()
        {
            ServicioGrano ServicioGrano = new ServicioGrano();
            return ServicioGrano.ObtenerGranos();
        }

        public ConjuntoCuposAgrupadosPorVendedor ObtenerConjuntoCuposAgrupadosPorVendedor(GranoCompradorPuerto filtro = null)
        {
            ConjuntoCuposAgrupadosPorVendedor Conjunto = new ConjuntoCuposAgrupadosPorVendedor();
            IList<long> Cuentas = ClaimsUtil.GetCuitOrCuenta();
            IList<CuposAgrupadosPorVendedor> InformadosStopNoCyo = ObtenerInformadosEnStopNoCyo(Cuentas, filtro);
            IList<CuposAgrupadosPorVendedor> InformadosStopCyo = ObtenerInformadosEnStopCyo(Cuentas, filtro);
            IList<CuposAgrupadosPorVendedor> NoInformadosStopNoCyo = ObtenerNoInformadosEnStopNoCyo(Cuentas, filtro);
            IList<CuposAgrupadosPorVendedor> NoInformadosStopCyo = ObtenerNoInformadosEnStopCyo(Cuentas, filtro);
            Conjunto.CuposInformadosSTOPNoCyo = InformadosStopNoCyo;
            Conjunto.CuposNoInformadosSTOPNoCyo = NoInformadosStopNoCyo;
            Conjunto.CuposInformadosSTOPCyo = InformadosStopCyo;
            Conjunto.CuposNoInformadosSTOPCyo = NoInformadosStopCyo;
            return Conjunto;
        }
    }
}