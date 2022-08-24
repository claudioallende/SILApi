using ResourceServer.Models.DataAccess;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ResourceServer.Models.Email
{
    public class EmailAnulacion : ServiceEmailEstadoCupo
    {
        private long CuentaComprador { get; set; }
        private string Comprador { get; set; }
        private long CuentaPuerto { get; set; }
        private string Puerto { get; set; }
        private int CodigoGrano { get; set; }
        private string Grano { get; set; }
        private string NroPlanta { get; set; }
        private ISession Session { get; set; }
        private MailSimplePdfDTO DTO { get; set; }

        public EmailAnulacion(long CuentaComprador, long CuentaPuerto, int CodigoGrano, MailSimplePdfDTO DTO, ISession Session)
        {
            this.CuentaComprador = CuentaComprador;
            this.CuentaPuerto = CuentaPuerto;
            this.CodigoGrano = CodigoGrano;
            this.Session = Session;
            this.DTO = DTO;
        }

        protected override MailMessage GetMessage(MailMessage msg)
        {
            msg.Subject = String.Format("Anulación Turnos {0} para el {1} producto {2} planta {3}",
                DTO.CantidadCupos,
                DTO.Fecha.ToString("dd/MM/yyyy"),
                GetGrano(Session),
                GetPuerto(Session));
            msg.IsBodyHtml = true;
            msg.Body = String.Format(@"Estimados<br>Se han cancelado {0} turnos de entrega de {1} 
                en la planta {2} para el día {3}.<br/>Adjuntamos un pdf con el detalle de los turnos cancelados.",
                    DTO.CantidadCupos,
                    GetGrano(Session),
                    GetPuerto(Session),
                    DTO.Fecha.ToString("dd/MM/yyyy"));
            return msg;
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

        private string GetComprador(ISession Session)
        {
            if (string.IsNullOrEmpty(Comprador))
            {
                ICompradorStore CompradorStore = new CompradorStore();
                Comprador = CompradorStore.FindById(CuentaComprador, Session).Nombre;
            }
            return Puerto;
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
    }
}