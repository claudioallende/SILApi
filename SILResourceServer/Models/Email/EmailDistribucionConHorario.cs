using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ResourceServer.Models.Email
{
    public class EmailDistribucionConHorario : EmailDistribucionMultiplesPdfs
    {
        public EmailDistribucionConHorario(IList<ICupo> ListaCupos, ISession Session)
            : base(ListaCupos, Session)
        {
        }
        protected override MailMessage GetMessage(MailMessage msg)
        {
            MailMessage message = base.GetMessage(msg);
            message.Body += string.Format("<br><br><b>IMPORTANTE: Horarios de ingreso de los camiones a playa externa de ACA SL:</b><br>");
            message.Body += string.Format(@"Desde las <b><u>16:00 horas</u></b> del d&iacute;a anterior al turno hasta las <b><u>14:00 horas</u></b> del d&iacute;a del turno.<br>
                Los d&iacute;as s&aacute;bado el horario l&iacute;mite de recepci&oacute;n ser&aacute; <b><u>hasta las 10.00 horas</u></b>.<br>
                Cami&oacute;n que arribe con posterioridad a dichos horarios, ser&aacute;n ingresado a nuestra playa pero no descargar&aacute;n en la fecha del turno, <b><u>sin dudas enfrentar&aacute;n demoras para la descarga</u></b>, favor arbitrar los medios necesarios para evitar esta situaci&oacute;n. 
                <br><br>
                Cualquier duda o consulta al respecto favor comunicarse con los equipos de productos agr&iacute;colas respectivos a vuestra zona geogr&aacute;fica.");
            return msg;
        }
    }
}