using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ResourceServer.Models.Email
{
	[Obsolete("Reemplazo por PdfCupos")]
    public class MailPdf : IMailInformation
    {
        public Cupos Consignacion { get; set; }
        public string Pdf { get; set; }
        public IList<Cupos> CuposAInformar { get; set; }

        public long Vendcta
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public IList<MailIncumplimientoGrano> ListIncumplimientoPorGrano
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    
        public int TotalCuposPerdidos()
        {
            throw new NotImplementedException();
        }
        public MemoryStream GraficoMultipleOtorgadosCumplimientoIncumplimiento(MailIncumplimientoGrano DetalleIncumplimiento, ISession session)
        {
            throw new NotImplementedException();
        }
        public AlternateView ToStringWithGraph( ISession session, string bodyCorriente = "", string pieDeCorreo = "") 
        {
            throw new NotImplementedException();
        }


        public int TotalCuposOtorgados()
        {
            throw new NotImplementedException();
        }

        public decimal PorcentajeDeEfectividad()
        {
            throw new NotImplementedException();
        }
    }
}