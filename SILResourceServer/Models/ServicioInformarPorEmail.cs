using NHibernate;
using Resource;
using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class ServicioInformarPorEmail
    {
        private string mapping = "Cupos.mpg.xml";

        public List<EmailInformado> InformarCuposSinCTG(int dia = 0, long vendCuenta = 0) 
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            try
            {
                ServicioProceso servicioProceso = new ServicioProceso();
                servicioProceso.GuardarProceso(servicioProceso.BuildEstadoProcesoEnEjecucion(1339560));

                ICuposStore store = new CuposStore();
                List<ICupo> cupos = new List<ICupo>();
                if (vendCuenta != 0)
                {
                    cupos.AddRange((List<Cupos>)store.FindCuposPendDeCTG(session, dia, vendCuenta));
                }
                else 
                {
                    cupos.AddRange((List<Cupos>)store.FindCuposPendDeCTG(session, dia));
                }
                //cupos.AddRange( (vendCuenta != 0) ? store.FindCuposPendDeCTG(session, dia, vendCuenta) : store.FindCuposPendDeCTG(session, dia));
                IGranoStore granoStore = new GranoStore();
                IList<Grano> granos = granoStore.FindAll(session);
                ServiceEmailEstadoCupo ServicioEmail = new EmailPendienteDeCTG(session, granos, dia);
                ServicioInformar InformarCuposSinCTG = new InformarPendienteDeCTGPorMail(ServicioEmail, session, vendCuenta, cupos);
                List<EmailInformado> EmailsInformados = new List<EmailInformado>();
                EmailsInformados.AddRange(InformarCuposSinCTG.InformarMails());
                return EmailsInformados;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Obtiene el detalle de incumplimiento de cupos para la cuenta ingresada en el periodo especificado en el parametro
        /// TipoDeInforme para la cuenta especificada. si no se especifica ninguna trae el detalle de todas. 
        /// luego realiza el envio de mail a las cuentas de correos configuradas para cada cuenta
        /// En caso de solicitar el tipoDeInforme = PERIODO_PERSONALIZADO se solicita un periodo desde hasta
        /// </summary>
        /// <param name="tipoDeInforme"></param>
        /// <param name="xdesde"></param>
        /// <param name="xhasta"></param>
        /// <param name="vendcta"></param>
        /// <returns></returns>
        public List<EmailInformado> InformarDetalleDeIncumplimientoDeCTG(string xtipoDeInforme, string xdesde, string xhasta, long vendcta) 
        {
            Resource.Constants.Informes tipoDeInforme = Resource.Constants.toInformes(xtipoDeInforme);
            DateTime hasta = DateTime.Now.Date;
            DateTime desde = DateTime.Now.AddDays(-30);       
            using (ISession session = HibernateUtil.OpenSession(mapping))
            try
            {
                ServicioProceso servicioProceso = new ServicioProceso();
                servicioProceso.GuardarProceso(servicioProceso.BuildEstadoProcesoEnEjecucion(1339560));
                List<ICupo> listaDetalle = new List<ICupo>();
                ICuposStore store = new CuposStore();
                switch (tipoDeInforme)
                {
                    case Resource.Constants.Informes.DIARIO_MAÑANA:
                        if (vendcta != 0) return this.InformarCuposSinCTG(0, vendcta);
                        return this.InformarCuposSinCTG(0);
                    case Resource.Constants.Informes.DIARIO_TARDE:
                        if (vendcta != 0) return this.InformarCuposSinCTG(1, vendcta);
                        return this.InformarCuposSinCTG(1, vendcta);                       
                    case Resource.Constants.Informes.SEMANAL_MAÑANA:
                        hasta = DateTime.Now.Date;
                        desde = DateTime.Now.AddDays(-7);
                        break;
                    case Resource.Constants.Informes.SEMANAL_TARDE:
                        hasta = DateTime.Now.Date;
                        desde = DateTime.Now.AddDays(-7);
                        break;
                    case Resource.Constants.Informes.MENSUAL_MAÑANA:
                        hasta = DateTime.Now.Date;
                        desde = DateTime.Now.AddDays(-30);
                        break;
                    case Resource.Constants.Informes.MENSUAL_TARDE:
                        hasta = DateTime.Now.Date;
                        desde = DateTime.Now.AddDays(-30);
                        break;
                    case Resource.Constants.Informes.MENSUAL_CALENDARIO:
                        DateTime mesPasado = DateTime.Now.Date.AddMonths(-1);
                        int ultimoDia = DateTime.DaysInMonth(mesPasado.Year, mesPasado.Month);
                        hasta = new DateTime(mesPasado.Year, mesPasado.Month, ultimoDia);
                        hasta = hasta.AddDays(1);   //corrijo la cota sup. por el select
                        int diasDelMes = DateTime.DaysInMonth(mesPasado.Year, mesPasado.Month);
                        desde = hasta.AddDays(-diasDelMes);
                        break;
                    case Resource.Constants.Informes.PERIODO_PERSONALIZADO:
                        if (xdesde != "")
                        {
                            desde = Convert.ToDateTime(xdesde);
                        }
                        if (xhasta != "")
                        {
                            hasta = Convert.ToDateTime(xhasta);
                        }
                        break;
                }
                listaDetalle.AddRange(store.DetalleDeIncumplimientoDeCupos(session, desde, hasta, vendcta));
                IGranoStore granoStore = new GranoStore();
                ServiceEmailEstadoCupo ServicioEmail = new EmailDetalleDeIncumplimientoCTG(session, granoStore.FindAll(session), tipoDeInforme);
                ServicioInformar InformarDetalleDeIncumplimientoDeCTG = new InformarDetalleDeIncumplimientoCTGPorMail(ServicioEmail, session, vendcta, listaDetalle, desde, hasta);
                List<EmailInformado> EmailsInformados = new List<EmailInformado>();
                EmailsInformados.AddRange(InformarDetalleDeIncumplimientoDeCTG.InformarMails());
                return EmailsInformados;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    /// <summary>
    /// Esto es a modo de Prueba
    /// </summary>
    /// <param name="xtipoDeInforme"></param>
    /// <param name="xdesde"></param>
    /// <param name="xhasta"></param>
    /// <param name="vendcta"></param>
    /// <returns></returns>
    public IList<DetalleDeIncumplimientoCupo> GetDetalleDeIncumplimientoDeCupos(string xtipoDeInforme, string xdesde, string xhasta, long vendcta) 
    {
        Constants.Informes tipoDeInforme = Constants.toInformes(xtipoDeInforme);
        DateTime hasta = DateTime.Now.Date;
        DateTime desde = DateTime.Now.AddDays(-30);
        using (ISession session = HibernateUtil.OpenSession(mapping))
            try
            {
                ServicioProceso servicioProceso = new ServicioProceso();
                servicioProceso.GuardarProceso(servicioProceso.BuildEstadoProcesoEnEjecucion(1339560));
                List<ICupo> listaDetalle = new List<ICupo>();
                ICuposStore store = new CuposStore();
                switch (tipoDeInforme)
                {
                    case Resource.Constants.Informes.SEMANAL_MAÑANA:
                        hasta = DateTime.Now.Date;
                        desde = DateTime.Now.AddDays(-7);
                        break;
                    case Resource.Constants.Informes.SEMANAL_TARDE:
                        hasta = DateTime.Now.Date;
                        desde = DateTime.Now.AddDays(-7);
                        break;
                    case Resource.Constants.Informes.MENSUAL_MAÑANA:
                        hasta = DateTime.Now.Date;
                        desde = DateTime.Now.AddDays(-30);
                        break;
                    case Resource.Constants.Informes.MENSUAL_TARDE:
                        hasta = DateTime.Now.Date;
                        desde = DateTime.Now.AddDays(-30);
                        break;
                    case Resource.Constants.Informes.MENSUAL_CALENDARIO:
                        DateTime mesPasado = DateTime.Now.Date.AddMonths(-1);
                        int ultimoDia = DateTime.DaysInMonth(mesPasado.Year, mesPasado.Month);
                        hasta = new DateTime(mesPasado.Year, mesPasado.Month, ultimoDia);
                        hasta = hasta.AddDays(1);   //corrijo la cota sup. por el select
                        int diasDelMes = DateTime.DaysInMonth(mesPasado.Year, mesPasado.Month);
                        desde = hasta.AddDays(-diasDelMes);
                        break;
                    case Resource.Constants.Informes.PERIODO_PERSONALIZADO:
                        if (xdesde != "")
                        {
                            desde = Convert.ToDateTime(xdesde);
                        }
                        if (xhasta != "")
                        {
                            hasta = Convert.ToDateTime(xhasta);
                        }
                        break;
                }
                IList<DetalleDeIncumplimientoCupo> listaResult = store.DetalleDeIncumplimientoDeCupos(session, desde, hasta, vendcta);
                return listaResult;
            }
            catch (Exception e)
            {
                throw e;
            }
    }


        /// <summary>
        /// Obtiene el detalle de incumplimiento de cupos para la cuenta ingresada en la semana pasada.
        /// si no se especifica cuenta. trae el detalle de todas. luego realiza el envio de mail a las cuentas de correos configuradas 
        /// para cada cuenta
        /// </summary>
        /// <param name="dia"></param>
        /// <param name="vendCuenta"></param>
        /// <returns></returns>
        public List<EmailInformado> InformarDetalleDeIncumplimientoDeCTGSemanal(DateTime desde, DateTime hasta, long vendcta) 
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            try
            {
                ServicioProceso servicioProceso = new ServicioProceso();
                servicioProceso.GuardarProceso(servicioProceso.BuildEstadoProcesoEnEjecucion(1339560));
                List<ICupo> listaDetalle = new List<ICupo>();
                ICuposStore store = new CuposStore();
                listaDetalle.AddRange(store.DetalleDeIncumplimientoDeCupos(session, desde, hasta, vendcta));
                IGranoStore granoStore = new GranoStore();
                ServiceEmailEstadoCupo ServicioEmail = new EmailDetalleDeIncumplimientoCTG(session, granoStore.FindAll(session), Resource.Constants.Informes.SEMANAL_MAÑANA);
                ServicioInformar InformarDetalleDeIncumplimientoDeCTG = new InformarDetalleDeIncumplimientoCTGPorMail(ServicioEmail, session, vendcta, listaDetalle, desde,hasta);
                List<EmailInformado> EmailsInformados = new List<EmailInformado>();
                EmailsInformados.AddRange(InformarDetalleDeIncumplimientoDeCTG.InformarMails());
                return EmailsInformados;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Obtiene el detalle de incumplimiento de cupos para la cuenta ingresada en el periodo señalado.
        /// si no se especifica cuenta. trae el detalle de todas
        /// </summary>
        /// <param name="desde"></param>
        /// <param name="hasta"></param>
        /// <param name="vendcta"></param>
        /// <returns></returns>
        public List<DetalleDeIncumplimientoCupo> ObtenerDetalleDeIncumplimientoDeCupos(DateTime desde, DateTime hasta, long vendcta) 
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                try
                {
                    IList<DetalleDeIncumplimientoCupo> listaDetalle = new List<DetalleDeIncumplimientoCupo>();
                    ICuposStore store = new CuposStore();
                    listaDetalle = store.DetalleDeIncumplimientoDeCupos(session, desde, hasta, vendcta);
                    return (List<DetalleDeIncumplimientoCupo>)listaDetalle;
                }
                catch( Exception e)
                {
                throw e;
                }
            }
        }


    }
}