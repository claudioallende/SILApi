using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using NHibernate;
using ResourceServer.Models.DataAccess;
using System.Globalization;

namespace ResourceServer.Models.Email
{
    public class EmailPendienteDeCTG: ServiceEmailEstadoCupo
    {
        private ISession Session { get; set; }
        private IList<Cupos> cupos { get; set; }
        private IList<Grano> granos {get; set; }
        private int dia { get; set; }

        public EmailPendienteDeCTG(ISession session, IList<Grano> granos, int queDia)
        {
            this.Session = session;
            this.granos = granos;
            this.dia = queDia;
        }
        
        /// <summary>
        /// dados los cupos ordenados por vendedor, fecha y grano arma el cuerpo del correo
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public void setCupos(List<Cupos> miscupos)
        {
            this.cupos = miscupos;
        }


        protected override MailMessage GetMessage(MailMessage msg)
        {
            switch (this.dia) 
            {
                case 0: /*corresponde al mail enviado por la mañana*/
                    msg.Subject = String.Format("TURNOS SIN ACTIVACIÓN del {0} ", DateTime.Now.Date.AddDays(this.dia).ToString("dd/MM/yyyy"));
                    msg.IsBodyHtml = true;
                    msg.Body = String.Format(@"Estimado:<br>Informamos que usted tiene <b>turnos pendiente de activar para la fecha</b>. Favor tomar los recaudos pertinentes para dar cumplimiento a los mismos. <b>IMPORTANTE</b> considerar los horarios de descarga del destino final. Ante cualquier duda <b>comunicarse</b> con los operadores de logística correspondiente. <b>NO CONTESTAR DICHO EMAIL</b>.<br> {0} <br><br> Saludos cordiales.",
                    MostrarAlfasInTable());
                    break;
                case 1: /*corresponde al mail enviado por la tarde*/
                    msg.Subject = String.Format("TURNOS SIN ACTIVACIÓN del {0} (STOP)", DateTime.Now.Date.AddDays(this.dia).ToString("dd/MM/yyyy"));
                    msg.IsBodyHtml = true;
                    msg.Body = String.Format(@"Estimado:<br>Informamos que usted tiene <b>turnos pendientes de activar para el día de mañana</b>. Favor tomar los recaudos pertinentes para dar cumplimiento a los mismos. Ante cualquier duda <b>comunicarse</b> con los operadores de logística correspondiente. <b>NO CONTESTAR DICHO EMAIL</b>.<br> {0} <br><br> Saludos cordiales.",
                    MostrarAlfasInTable());
                    break;
            
            }
            return msg;
        }

        /// <summary>
        /// los cupos vienen ordenados por grano
        /// </summary>
        /// <returns>string con la concatenacion de los alfas</returns>
        private string MostrarAlfas()
        {
            String alfas = "";
            //int grano = 0;
            var granos = this.cupos.GroupBy(x =>
                    new
                    {
                        grano = x.Grano
                    }
                )
                .Select(z =>
                    new
                    {
                        grano = z.Key.grano
                    })
                .ToList();
            foreach (var migrano in granos) 
            {
                alfas += "<br><i>" + getNombreGrano(migrano.grano) + "</i><br><br>";
                IList<Cupos> cuposXgrano = this.cupos.Where(x =>
                        x.Grano == migrano.grano
                    ).ToList();
                foreach (Cupos cupos in cuposXgrano) 
                {
                    alfas += cupos.Nrocupo.ToString() + "<br>";
                } 
            }
            return alfas;
        }

        protected string MostrarAlfasInTable() 
        {
            String alfasYfecha = "";
            //int grano = 0;
            var granos = this.cupos.GroupBy(x =>
                    new
                    {
                        grano = x.Grano
                    }
                )
                .Select(z =>
                    new
                    {
                        grano = z.Key.grano
                    })
                .ToList();
            foreach (var migrano in granos)
            { 
                List<InfoDeCupoPendDeCTG> ListainfoCuerpo = this.cupos
                                        .Where(x=> x.Grano == migrano.grano)
                                        .Select(y =>
                                                new InfoDeCupoPendDeCTG
                                                {
                                                    Cupo = y.Nrocupo,
                                                    FechaInformado = (y.FechaYHoraInformado != null) ? (DateTime)y.FechaYHoraInformado : default(DateTime)
                                                })
                                        .ToList();

                alfasYfecha += "<br><b>" + getNombreGrano(migrano.grano) + "</b><br><br>";
                ListToHtmlTable conversor = new ListToHtmlTable("center", "Left", new List<string>());
                alfasYfecha += conversor.GetMyTable(ListainfoCuerpo, x => x.Cupo, x => x.Informado);
                alfasYfecha += "<br>";
            }
            return alfasYfecha;
        }

        private string getNombreGrano(int idGrano) 
        { 
           string salida = this.granos
               .Where(x => x.CodigoGrano==idGrano)
               .Select(x=> x.Nombre)
               .FirstOrDefault();
           return salida;
        }
    }
}