using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.Email
{
    public interface IMailInformation
    {
        Cupos Consignacion { get; set; }
        string Pdf { get; set; }
        IList<Cupos> CuposAInformar { get; set; }

        Int64 Vendcta { get; set; }
        IList<MailIncumplimientoGrano> ListIncumplimientoPorGrano { get; set; }

        int TotalCuposPerdidos();
        int TotalCuposOtorgados();
        decimal PorcentajeDeEfectividad();

        MemoryStream GraficoMultipleOtorgadosCumplimientoIncumplimiento(MailIncumplimientoGrano DetalleIncumplimiento, ISession session);
        AlternateView ToStringWithGraph( ISession session, string bodyCorriente = "", string pieDeCorreo = "");
    }
}
