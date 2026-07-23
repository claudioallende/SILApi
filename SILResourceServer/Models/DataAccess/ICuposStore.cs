using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
    public interface ICuposStore
    {
        void Save(Cupos c);
        Int64 Saverow(Cupos c);
        void Save(Cupos c, ISession session, ITransaction transaccion);
        void Save(Cupos c, ISession session);
        void Update(Int64 id, Cupos c);
        void Update(Cupos c);
        void Update(Int64 Id, Cupos c, ISession session, ITransaction transaccion);
        void Delete(Int64 Id);
        void Delete(Cupos cupo, ISession session, ITransaction transaccion);
        void Delete(Cupos cupo, ISession session);
        Cupos FindById(Int64 Id);
        IList<Cupos> FindByIds(IList<long> Ids, ISession session);
        IList<Cupos> FindAll();
        IList<Cupos> FindByGranoAndPuertoAndCompAndVendGroupByGranoAndPuertoAndCompAndVend(int grano, long puerto, Int64 comp, Int64 vend);
        //
        // Summary:
        //    Busca los registros cuerpo  para un comp, vend, grano, fecha y puerto dado para ver 
        //    los datos de consignacion y cuantos cuerpos por cada uno hay
        // Parameters:
        //   compcta: cuenta comprador
        //   vendcta: cuenta vendedora
        //   grano: bnumero de grano
        //   fecha
        //   puerto: nro de puerto
        IList<Counter<Cupos>> FindConsignacionesForKey(Int64 compcta, Int64 vendcta, int grano, DateTime? fechaDesde, DateTime? fechaHasta, long puerto);
        //Cuenta cantidad de cupos de los 5 dias despues de hoy para los parametros seleccionados
        /// <summary>
        /// Además filtra por centro que tiene permiso.
        /// </summary>
        /// <param name="compcta"></param>
        /// <param name="vendcta"></param>
        /// <param name="grano"></param>
        /// <param name="puerto"></param>
        /// <returns></returns>
        IList<Counter<Cupos>> FindConsignacionesForKeyGroupByFecha(Int64 compcta, Int64 vendcta, int grano, long puerto);
        /// <summary>
        /// Además filtra por centro que tiene permiso.
        /// </summary>
        /// <param name="compcta"></param>
        /// <param name="vendcta"></param>
        /// <param name="grano"></param>
        /// <param name="puerto"></param>
        /// <returns></returns>
        IList<Counter<Cupos>> FindConsignacionesForKeyGroupByFechaCyO(Int64 compcta, Int64 vendcta, int grano, long puerto);
        //Cuenta cantidad de cupos de los 5 dias despues de hoy para los parametros seleccionados con los datos de consignacion seleccionados
        /// <summary>
        /// Además filtra por centro a los que tiene permiso de acceso
        /// </summary>
        /// <param name="consignacion"></param>
        /// <returns></returns>
        IList<Counter<Cupos>> FindConsignacionesForKeyAndConsignacionGroupByFecha(Cupos consignacion);
        IList<Counter<Cupos>> FindNumberOfConsignacionesForKey(Int64 compcta, Int64 vendcta, int grano, DateTime fecha, long puerto, Cupos DatosCupoBuscar);
        IList<Counter<Cupos>> FindNumberOfConsignacionesForKey(Int64 compcta, Int64 vendcta, int grano, DateTime fecha, long puerto, Consignacion Consignacion, ISession Session);
        [Obsolete("Reemplazo por FindByConsignacion. Uso la clase Consignacion en vez de DatosBuscar")]
        IList<Cupos> FindForKey(Int64 compcta, Int64 vendcta, int grano, DateTime fecha, long puerto, Cupos DatosCupoBuscar);
        IList<Cupos> FindForKey(Int64 compcta, Int64 vendcta, int grano, DateTime fecha, long puerto, Consignacion Consignacion, ISession Session);
        IList<Cupos> FindForKeyStatusAndType(Int64 compcta, Int64 vendcta, int grano, DateTime fecha, long puerto, Cupos DatosCupoBuscar, int status, int tipo);
        /// <summary>
        /// Además filtra por centro a los que tiene permiso de acceso
        /// </summary>
        /// <param name="CuentaComprador"></param>
        /// <param name="CuentaVendedor"></param>
        /// <param name="CuentaPuerto"></param>
        /// <param name="CodigoGrano"></param>
        /// <param name="Fecha"></param>
        /// <param name="Consignacion"></param>
        /// <param name="Status"></param>
        /// <param name="Session"></param>
        /// <returns></returns>
        IList<Cupos> FindForKeyStatus(long CuentaComprador, long CuentaVendedor, long CuentaPuerto, int CodigoGrano, DateTime Fecha, Consignacion Consignacion, int Status, ISession Session);
        IList<Cupos> FindByCompradorVendedorGranoFechaConsignacionStatus(long CuentaComprador, long CuentaVendedor, int CodigoGrano, DateTime Fecha, Consignacion Consignacion, IList<int> Status, ISession Session);
        IList<Cupos> FindIdorigenForConsignacionDistinct(Cupos cupoPadreNuevo);
        Int64 AgregarCupo(List<List<Cupos>> cuposDeLaSemana, Cupos encabezado, Cupos encabezadoAModificar);
        Cupos FindByCompAndVendAndGranoAndFechaAndPuertoAndIdOrigenAndAlfaAndStatus(Cupos DatosDeBusqueda);
        Cupos FindByCompAndVendAndGranoAndFechaAndPuertoAndIdOrigenAndAlfaAndStatus(Cupos DatosDeBusqueda, ISession Session);
        Cupos FindByCompAndVendAndGranoAndFechaAndPuertoAndIdOrigenAndAlfa(Cupos DatosDeBusqueda);
        Cupos FindByCompAndVendAndGranoAndFechaAndPuertoAndIdOrigenAndAlfa(Cupos DatosDeBusqueda, ISession Session);
        void Anular(Cupos cupo);
        void AnularCYO(Cupos cupo, string motivo);
        void Anular(Cupos cupo, ISession session);
        void AnularCYO(Cupos cupo, string motivo, ISession session);
        //IDictionary<string, string> FindLastObservacionesByConsignacionAndKeyOrderByFechaDesc(Cupos consignacion, long compcta, long vendcta, long puertocta, int grano);
        IList<Cupos> FindByPdf(int informado);
        IList<ICupo> FindCuerposDistribuidosNoInformadosByKey(long compcta, long vendcta, long puertocta, int grano);
        IList<DetalleCupoComplete> FindCuposDistribuidosInformadosBetweenDates(DateTime desde, DateTime hasta, IList<long> CuentasVendedoras);
        IList<DetalleCupoComplete> FindCuposForId(IList<long> ids);
        IList<ICupo> FindCuerposDistribuidosCyONoInformadosByKey(long compcta, long vendcta, long puertocta, int grano);
        IList<Cupos> FindCuerposDistribuidosNoInformadosByKey(long compcta, long vendcta, long puertocta, int grano, string centro, string centrodist);
        IList<Cupos> FindCuerposDistribuidosCyONoInformadosByKey(long compcta, long vendcta, long puertocta, int grano, string centro, string centrodist);
        IList<Cupos> FindCuerposAnuladosNoInformadosByKey(long compcta, long vendcta, long puertocta, int grano);
        IList<Cupos> FindCuerposAnuladosCyONoInformadosByKey(long compcta, long vendcta, long puertocta, int grano);
        IList<ICupo> FindCuerposAnuladosNoInformadosByKey(long compcta, long vendcta, long puertocta, int grano, string centro, string centrodist);
        IList<ICupo> FindCuerposAnuladosCyONoInformadosByKey(long compcta, long vendcta, long puertocta, int grano, string centro, string centrodist);
        IList<Counter<Cupos>> FindConsignacionesAndObservacionForKey(Int64 compcta, Int64 vendcta, int grano, DateTime? fechaDesde, DateTime? fechaHasta, long puerto);
        Cupos FindCupoLastObservacionByConsignacion(long compcta, long vendcta, int grano, long puerto, Consignacion Consignacion);
        string FindLastObservacionByConsignacion(long compcta, long vendcta, int grano, long puerto, Consignacion Consignacion);
        string FindLastContactoComercialByConsignacion(long compcta, long vendcta, int grano, long puerto, Consignacion Consignacion);
        /// <summary>
        /// Busca cupos por grano, comprador, vendedor, puerto, vendcyo y consignacion sin tener en cuenta el status del cupo.
        /// </summary>
        /// <param name="Grano"></param>
        /// <param name="Comprador"></param>
        /// <param name="Vendedor"></param>
        /// <param name="Puerto"></param>
        /// <param name="Vendcyo"></param>
        /// <param name="Consignacion"></param>
        /// <returns></returns>
        IList<Cupos> FindByGranoAndCompradorAndVendedorAndPuertoAndVendcyoAndConsignacion(int Grano, long Comprador, long Vendedor, long Puerto, long Vendcyo, Consignacion Consignacion);
        /// <summary>
        /// Busca por Grano, Comprador, Vendedor, Puerto, Vendcyo, Consignacion, Centro y CentroDist.
        /// Además filtra por los centros que tiene permitido ver.
        /// </summary>
        /// <param name="Grano"></param>
        /// <param name="Comprador"></param>
        /// <param name="Vendedor"></param>
        /// <param name="Puerto"></param>
        /// <param name="Vendcyo"></param>
        /// <param name="Consignacion"></param>
        /// <param name="FiltroCentro"></param>
        /// <returns></returns>
        IList<Cupos> FindByGranoAndCompradorAndVendedorAndPuertoAndVendcyoAndConsignacionAndCentro(int Grano, long Comprador, long Vendedor, long Puerto, long Vendcyo, Consignacion Consignacion, FiltroCentro FiltroCentro);
        void UpdateCupoInformado(IList<Cupos> cupo);
        void UpdateCupoInformado(Cupos cupo);
        void UpdateCupoAnuladoInformado(IList<Cupos> cupo);
        void UpdateCupoAnuladoInformado(Cupos cupo);
        long LastUvdist(long CuentaComprador, long CuentaVendedor, long CuentaPuerto, int CodigoGrano);
        /// <summary>
        /// Busca los Encabezados.
        /// </summary>
        /// <param name="CuentaComprador"></param>
        /// <param name="CuentaVendedor"></param>
        /// <param name="CuentaPuerto"></param>
        /// <param name="CodigoGrano"></param>
        /// <param name="Fecha"></param>
        /// <param name="Consignacion"></param>
        /// <returns></returns>
        IList<Cupos> FindEncabezado(long CuentaComprador, long CuentaVendedor, long CuentaPuerto, int CodigoGrano, DateTime Fecha, Consignacion Consignacion);

        void Update(Cupos cupos, ISession session);
        IList<Cupos> FindEncabezadosByIds(IList<long> Ids);
        IList<Cupos> FindEncabezadosByIds(IList<long> Ids, ISession session);
        IList<Cupos> FindForKey(long CuentaComprador, long CuentaVendedor, long CuentaPuerto, int CodigoGrano, DateTime Fecha, Consignacion Consignacion, ISession Session);
        IList<Cupos> FindByCompradorVendedorGranoFechaConsignacion(long CuentaComprador, long CuentaVendedor, int CodigoGrano, DateTime Fecha, Consignacion Consignacion, ISession Session);
        IList<Cupos> FindByNroCupos(IList<string> CodigosAlfanumericos);
        IList<Counter<Cupos>> FindConsignacionesForKey(Int64 compcta, Int64 vendcta, int grano, DateTime? fechaDesde, DateTime? fechaHasta, long puerto, ISession Session);
        /// <summary>
        /// Además filtra por centro a los que tiene permiso de acceso
        /// </summary>
        /// <param name="compcta"></param>
        /// <param name="vendcta"></param>
        /// <param name="grano"></param>
        /// <param name="fecha"></param>
        /// <param name="puerto"></param>
        /// <param name="DatosCupoBuscar"></param>
        /// <param name="Session"></param>
        /// <returns></returns>
        IList<Counter<Cupos>> FindNumberOfConsignacionesForKey(Int64 compcta, Int64 vendcta, int grano, DateTime fecha, long puerto, Cupos DatosCupoBuscar, ISession Session);
        IList<Cupos> FindByCompAndVendAndGranoAndFechaAndPuertoAndIdOrigenAndAlfaAndStatus(long CuentaVendedor, int CodigoGrano,
            long CuentaComprador, DateTime Fecha, long CuentaPuerto, long IdOrigen, IList<string> Alfanumericos, int Estado, ISession Session);
        IList<Cupos> FindByCompAndVendAndGranoAndFechaAndPuertoAndIdOrigenAndAlfaAndStatus(long CuentaComprador, long CuentaVendedor, 
            int CodigoGrano, long CuentaPuerto, IList<DateTime> Fecha, IList<string> Alfanumericos, int Estado, ISession Session);
        IList<Cupos> FindByCompAndPuertoAndGranoAndAlfaAndFechaAndVendcyo(long CuentaComprador, long CuentaPuerto, int CodigoGrano, string Alfanumerico, DateTime Fecha, long Vendcyo, ISession Session);
        IList<Cupos> FindByCompAndPuertoAndGranoAndAlfaAndFechaAndVendcyo(long CuentaComprador, long CuentaPuerto, int CodigoGrano, IList<string> Alfanumericos, DateTime Fecha, long Vendcyo, ISession Session);
        IList<Cupos> FindByVendAndPuertoAndGranoAndVendcyoAndFechaAndAlfas(long CuentaVendedor, long CuentaPuerto, int CodigoGrano, IList<string> Alfanumericos, DateTime Fecha, long Vendcyo, ISession Session);
        /// <summary>
        /// Hace una consulta a la base de datos por cada elemento en la lista Codigos. 
        /// Cada elemento en la lista tiene una relación entre Fecha y Códigos Alfanuméricos, por ello se hace una consulta por cada item en la lista.
        /// </summary>
        /// <param name="CuentaPuerto"></param>
        /// <param name="CodigoGrano"></param>
        /// <param name="Codigos"></param>
        /// <param name="Session"></param>
        /// <returns></returns>
        IList<Cupos> FindByPuertoAndGranoAndFechaAndAlfas(long CuentaPuerto, int CodigoGrano, IList<CodigosAlfanumericos> Codigos, ISession Session);
        IList<Cupos> FindByVendAndPuertoAndGranoAndVendcyoAndFechaAndAlfasAndUvalueNotZero(long CuentaVendedor, long CuentaPuerto, int CodigoGrano, IList<string> Alfanumericos, DateTime Fecha, long Vendcyo, ISession Session);
        void Delete(IList<Cupos> Cupos, ISession Session);
        IList<long> FindVendedoresConCuposPendDeCTG();
        IList<Cupos> FindCuposPendDeCTG(ISession Session, int dia, long vendCuenta = 0);
        IList<DetalleCupo> FindDetalleCuposPendDeCTG(ISession Session, long vendCuenta);
        IList<Cupos> FindByNroCupoAndCompctaAndVendcyoAndNotStatusAndFecha(IList<string> AlfanumericosPadre, DateTime Fecha, int Status, ISession Session);

        void Update(IList<Cupos> Cupos, ISession Session);

        IList<Cupos> FindByCompAndVendAndGranoAndFechaAndPuertoAndIdOrigenAndAlfaAndStatus(long CuentaVendedor, int CodigoGrano,
            long CuentaComprador, DateTime Fecha, long CuentaPuerto, long IdOrigen, IList<string> Alfanumericos, IList<int> Estado, ISession Session);
        /// <summary>
        /// Busca y cuenta la cantidad de cupos con los criterios pasados por parametro y filtrando por el Centro del usuario.
        /// </summary>
        /// <param name="CuentaComprador"></param>
        /// <param name="CuentaVendedor"></param>
        /// <param name="Grano"></param>
        /// <param name="CuentaPuerto"></param>
        /// <param name="Consignacion"></param>
        /// <param name="Cyo"></param>
        /// <returns></returns>
        IList<Counter<Cupos>> FindCuposByCompradorVendedorGranoPuertoConsignacion(long CuentaComprador, long CuentaVendedor, int Grano, long CuentaPuerto, Consignacion Consignacion, bool Cyo);
        IList<DetalleDeIncumplimientoCupo> DetalleDeIncumplimientoDeCupos(ISession Session, DateTime desde, DateTime hasta, long vendCuenta = 0);
    }
}
