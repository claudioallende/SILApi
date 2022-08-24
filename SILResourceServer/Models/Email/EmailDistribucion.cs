using ResourceServer.Models.DataAccess;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ResourceServer.Models.Email
{
    public class EmailDistribucion : ServiceEmailEstadoCupo
    {
        private long CuentaPuerto { get; set; }
        private string Puerto { get; set; }
        private int CodigoGrano { get; set; }
        private string Grano { get; set; }
        private string NroPlanta { get; set; }
        private ISession Session { get; set; }
        private MailSimplePdfDTO DTO { get; set; }

        public EmailDistribucion(long CuentaPuerto, int CodigoGrano, MailSimplePdfDTO DTO, ISession Session)
        {
            this.CuentaPuerto = CuentaPuerto;
            this.CodigoGrano = CodigoGrano;
            this.Session = Session;
            this.DTO = DTO;
        }
        protected override MailMessage GetMessage(MailMessage msg)
        {
            msg.Subject = string.Format("Turnos {0} para el {1} producto {2} planta {3}", DTO.CantidadCupos, DTO.Fecha.Date.ToString("dd/MM/yyyy"), GetGrano(Session), GetPuerto(Session));
            msg.IsBodyHtml = true;
            msg.Body += "Estimados: <br/>";
            msg.Body += string.Format(@"Asignamos la cantidad de {0} turnos de {1}
                 para ser entregado sobre la planta {2} para el d&iacute;a {3}.<br/>", DTO.CantidadCupos, GetGrano(Session), GetPuerto(Session), DTO.Fecha.Date.ToString("dd/MM/yyyy"));
            msg.Body += "Si existe alguna duda o imposibilidad de cumplir con el turno, rogamos comunicarse con los operadores log&iacute;sticos correspondientes a sus centros.<br/>";
            msg.Body += "Adjuntamos un pdf con el modelo de carta de porte.<br/>";
            msg.Body += "Saludos cordiales.";
            return msg;
        }

        private string GetGrano(ISession Session)
        {
            if (string.IsNullOrEmpty(Grano))
            {
                IGranoStore GranoStore = new GranoStore();
                Grano = GranoStore.FindById(CodigoGrano.ToString(), Session).Nombre;
            }
            return Grano;
        }

        private string GetPuerto(ISession Session)
        {
            if (string.IsNullOrEmpty(Puerto))
            {
                IPuertoStore PuertoStore = new PuertoStore();
                Puerto = PuertoStore.FindById(CuentaPuerto, Session).Nombre;
            }
            return Puerto;
        }

        private string GetNroPlanta(ISession Session)
        {
            if (string.IsNullOrEmpty(NroPlanta))
            {
                IPuertoStore PuertoStore = new PuertoStore();
                NroPlanta = PuertoStore.FindNroPlantaByCuentaPuerto(CuentaPuerto, Session);
            }
            return NroPlanta;
        }
    }
}