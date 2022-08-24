using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Resource;
using System.IO;
using System.Globalization;

namespace ResourceServer.Models.Email
{
    public class EmailDetalleDeIncumplimientoCTG : ServiceEmailEstadoCupo
    {
        private ISession Session { get; set; }
        private IList<Grano> granos { get; set; }
        private Resource.Constants.Informes TipoDeInforme { get; set; }
        private IMailInformation InfoCuerpo { get; set; }
        private DateTime PeriodoDesde;
        private DateTime PeriodoHasta;

        /// <summary>
        /// constructor de clase
        /// </summary>
        /// <param name="session"></param>
        /// <param name="granos"></param>
        /// <param name="tipoDeInforme"></param>
        public EmailDetalleDeIncumplimientoCTG(ISession session, IList<Grano> granos, Resource.Constants.Informes tipoDeInforme)
        {
            this.Session = session;
            this.granos = granos;
            this.TipoDeInforme = tipoDeInforme;
        }

        /// <summary>
        /// setea la info para el armado del body del mail
        /// </summary>
        /// <param name="informacionParaBody"></param>
        public void SetInfoCuerpo(IMailInformation informacionParaBody, DateTime xdesde, DateTime xhasta)
        {
            this.InfoCuerpo = informacionParaBody;
            this.PeriodoDesde = xdesde;
            this.PeriodoHasta = xhasta;            
        }

        /// <summary>
        /// obtiene el mensaje a imprimir en el cuerpo del mail
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected override MailMessage GetMessage(MailMessage msg)
        {
            /*armamos el mensaje*/
            AlternateView bodyWithGraph;
            var body="";
            int cuposPerdidos = this.InfoCuerpo.TotalCuposPerdidos();
            int cuposOtorgados = this.InfoCuerpo.TotalCuposOtorgados();
            decimal porcentajeDeEfectividad = this.InfoCuerpo.PorcentajeDeEfectividad();
            switch (this.TipoDeInforme)
            {
                case Resource.Constants.Informes.SEMANAL_MAÑANA:
                    msg.Subject = String.Format("INFORME SEMANAL DE LOS TURNOS NO CUMPLIDOS EN STOP - Del {0} al {1} ", this.PeriodoDesde.ToString("dd/MM/yyyy"), this.PeriodoHasta.AddDays(-1).ToString("dd/MM/yyyy"));                    
                    msg.IsBodyHtml = true;
                    body = this.primeraLineaDelCuerpo(cuposPerdidos, cuposOtorgados, porcentajeDeEfectividad, this.TipoDeInforme);
                    body += "Adjuntamos el reporte para vuestro análisis. Ante cualquier duda <b>comunicarse </b> con los operadores de logística correspondientes.<Br/>  <b>NO CONTESTAR DICHO EMAIL </b>.<Br/><Br/><Br/>";
                    bodyWithGraph = this.GetBody(true, body, "Saludos cordiales.");
                    msg.AlternateViews.Add(bodyWithGraph);
                    break;
                case Resource.Constants.Informes.SEMANAL_TARDE:
                    Console.WriteLine("Case 2");
                    break;
                case Resource.Constants.Informes.MENSUAL_MAÑANA:
                    msg.Subject = String.Format("INFORME MENSUAL DE LOS TURNOS NO CUMPLIDOS EN STOP - Del {0} al {1} ", this.PeriodoDesde.ToString("dd/MM/yyyy"), this.PeriodoHasta.AddDays(-1).ToString("dd/MM/yyyy"));                    
                    msg.IsBodyHtml = true;                    
                    body = this.primeraLineaDelCuerpo(cuposPerdidos,cuposOtorgados,porcentajeDeEfectividad,this.TipoDeInforme);
                    body += "Adjuntamos el reporte para vuestro análisis. Ante cualquier duda <b>comunicarse </b> con los operadores de logística correspondientes.<Br/>  <b>NO CONTESTAR DICHO EMAIL </b>.<Br/><Br/><Br/>";
                    bodyWithGraph = this.GetBody(true, body, "Saludos cordiales.");
                    msg.AlternateViews.Add(bodyWithGraph);
                    break;
                case Resource.Constants.Informes.MENSUAL_TARDE:
                    Console.WriteLine("Case 2");
                    break;
                case Resource.Constants.Informes.MENSUAL_CALENDARIO:
                    msg.Subject = String.Format("INFORME MENSUAL DE LOS TURNOS NO CUMPLIDOS EN STOP - Del {0} al {1} ", this.PeriodoDesde.ToString("dd/MM/yyyy"), this.PeriodoHasta.AddDays(-1).ToString("dd/MM/yyyy"));
                    msg.IsBodyHtml = true;
                    body = this.primeraLineaDelCuerpo(cuposPerdidos,cuposOtorgados,porcentajeDeEfectividad,this.TipoDeInforme);
                    body += "Adjuntamos el reporte para vuestro análisis. Ante cualquier duda <b>comunicarse </b> con los operadores de logística correspondientes.<Br/>  <b>NO CONTESTAR DICHO EMAIL </b>.<Br/><Br/><Br/>";
                    bodyWithGraph = this.GetBody(true, body, "Saludos cordiales.");
                    msg.AlternateViews.Add(bodyWithGraph);
                    break;
                case Resource.Constants.Informes.PERIODO_PERSONALIZADO:
                    msg.Subject = String.Format("INFORME DE TURNOS NO CUMPLIDOS EN STOP - Del {0} al {1} ", this.PeriodoDesde.ToString("dd/MM/yyyy"), this.PeriodoHasta.AddDays(-1).ToString("dd/MM/yyyy"));                    
                    msg.IsBodyHtml = true;
                    body = this.primeraLineaDelCuerpo(cuposPerdidos,cuposOtorgados,porcentajeDeEfectividad,this.TipoDeInforme);
                    body += "Adjuntamos el reporte para vuestro análisis. Ante cualquier duda <b>comunicarse </b> con los operadores de logística correspondientes.<Br/>  <b>NO CONTESTAR DICHO EMAIL </b>.<Br/><Br/><Br/>";
                    bodyWithGraph = this.GetBody(true, body, "Saludos cordiales.");
                    msg.AlternateViews.Add(bodyWithGraph);
                    break;
            }
            return msg;
        }

        private string primeraLineaDelCuerpo( int cuposPerdidos, int cuposOtorgados, decimal porcentajeDeEfectividad, Resource.Constants.Informes tipoReporte)
        {
            string primeralinea="";
            switch (tipoReporte)
            {
                case Resource.Constants.Informes.SEMANAL_MAÑANA:
                    if (cuposPerdidos==0)
                    {
                        primeralinea = String.Format(@"Estimado:<br>Informamos que usted en el transcurso de la última semana cumplió con la totalidad de los turnos asignados, <b>EFECTIVIDAD 100% FELICITACIONES!!!!</b> Total de turnos otorgados {0}.<br>", cuposOtorgados);
                    }else{
                        primeralinea = String.Format(@"Estimado:<br>Informamos que usted en el transcurso de la ultima semana <b>NO cumplió con {0} turnos</b> (aproximadamente {1} Toneladas), de un total de turnos otorgados de <b>{2}</b> (aproximadamente {3} toneladas - <b>Efectividad {4}%</b>)<br>", cuposPerdidos, (cuposPerdidos * 29).ToString("N0", new CultureInfo("es-AR")), cuposOtorgados, (cuposOtorgados * 29).ToString("N0", new CultureInfo("es-AR")), porcentajeDeEfectividad);
                    }
                    break;
                case Resource.Constants.Informes.MENSUAL_MAÑANA:
                    if (cuposPerdidos==0)
                    {
                        primeralinea = String.Format(@"Estimado:<br>Informamos que usted en el transcurso del último mes cumplió con la totalidad de los turnos asignados, <b>EFECTIVIDAD 100% FELICITACIONES!!!!</b> Total de turnos otorgados {0}.<br>", cuposOtorgados);
                    }else{
                        primeralinea = String.Format(@"Estimado:<br>Informamos que usted en el transcurso del último mes <b>NO cumplió con {0} turnos</b> (aproximadamente {1} Toneladas), de un total de turnos otorgados de <b>{2}</b> (aproximadamente {3} toneladas - <b>Efectividad {4}%</b>)<br>", cuposPerdidos, (cuposPerdidos * 29).ToString("N0", new CultureInfo("es-AR")), cuposOtorgados, (cuposOtorgados * 29).ToString("N0", new CultureInfo("es-AR")), porcentajeDeEfectividad);
                    }
                    break;
                case Resource.Constants.Informes.MENSUAL_CALENDARIO:
                    if (cuposPerdidos == 0)
                    {
                        primeralinea = String.Format(@"Estimado:<br>Informamos que usted en el transcurso del último mes cumplió con la totalidad de los turnos asignados, <b>EFECTIVIDAD 100% FELICITACIONES!!!!</b> Total de turnos otorgados {0}.<br>", cuposOtorgados);
                    }
                    else
                    {
                        primeralinea = String.Format(@"Estimado:<br>Informamos que usted en el transcurso del último mes <b>NO cumplió con {0} turnos</b> (aproximadamente {1} Toneladas), de un total de turnos otorgados de <b>{2}</b> (aproximadamente {3} toneladas - <b>Efectividad {4}%</b>)<br>", cuposPerdidos, (cuposPerdidos * 29).ToString("N0", new CultureInfo("es-AR")), cuposOtorgados, (cuposOtorgados * 29).ToString("N0", new CultureInfo("es-AR")), porcentajeDeEfectividad);
                    }
                    break;
                case Resource.Constants.Informes.PERIODO_PERSONALIZADO:
                    if (cuposPerdidos == 0)
                    {
                        primeralinea = String.Format(@"Estimado:<br>Informamos que usted en el transcurso del periodo señalado en el asunto cumplió con la totalidad de los turnos asignados, <b>EFECTIVIDAD 100% FELICITACIONES!!!!</b> Total de turnos otorgados {0}.<br>", cuposOtorgados);
                    }
                    else
                    {
                        primeralinea = String.Format(@"Estimado:<br>Informamos que usted en el transcurso del periodo señalado en el asunto <b>NO cumplió con {0} turnos</b> (aproximadamente {1} Toneladas), de un total de turnos otorgados de <b>{2}</b> (aproximadamente {3} toneladas - <b>Efectividad {4}%</b>)<br>", cuposPerdidos, (cuposPerdidos * 29).ToString("N0", new CultureInfo("es-AR")), cuposOtorgados, (cuposOtorgados * 29).ToString("N0", new CultureInfo("es-AR")), porcentajeDeEfectividad);
                    }
                    break;
            }
            return primeralinea;
        }

        /// <summary>
        /// Funcion que retorna el body del mail. Este muestra el detalle de incumplimiento en el periodo determinado
        /// con un corte por grano. En formato Html.
        /// </summary>
        /// <returns></returns>
        private AlternateView GetBody(bool conGraficoPorTabla = false, string bodyCorriente = "", string pieDeCorreo = "") 
        {
            string body = bodyCorriente;
            if (conGraficoPorTabla)
            {
                return this.InfoCuerpo.ToStringWithGraph(this.Session, body, pieDeCorreo);
            }
            else 
            {
                body = this.InfoCuerpo.ToString();
                return AlternateView.CreateAlternateViewFromString(body, null, "text/html");
            }          
        }

        /// <summary>
        /// Obtiene el nombre del grano. dado su codigo
        /// </summary>
        /// <param name="idGrano"></param>
        /// <returns></returns>
        private string getNombreGrano(int idGrano)
        {
            string salida = this.granos
                .Where(x => x.CodigoGrano == idGrano)
                .Select(x => x.Nombre)
                .FirstOrDefault();
            return salida;
        }
    }
}