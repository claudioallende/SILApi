using ResourceServer.Models.Configuracion;
using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Email;
using ResourceServer.Models.Error;
using ResourceServer.Models.Error.Exceptions;
using ResourceServer.Models.Pdf;
using iTextSharp.text;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;

namespace ResourceServer.Models.Email
{
    public abstract class ServicioInformarEstadoCupo : ServicioInformar
    {
        private SmtpSection Configuracion { get; set; }
        private string OK = "OK";
        private string EMAILS_NO_CONFIGURADOS = "Mails no configurados";
        protected abstract string TipoServicioInformar { get; set; }
        //private string ALGUNOS_PDF_NO_ENVIADO { get; set; }

        protected long CuentaComprador { get; set; }
        protected long CuentaPuerto { get; set; }
        protected int CodigoGrano { get; set; }
        protected string CentroAlta { get; set; }
        protected string CentroDist { get; set; }
        protected bool EsCyo { get; set; }

        public ServicioInformarEstadoCupo(long CuentaComprador, long CuentaPuerto, int CodigoGrano, string CentroAlta, string CentroDist, bool EsCyo)
        {
            this.CuentaComprador = CuentaComprador;
            this.CuentaPuerto = CuentaPuerto;
            this.CodigoGrano = CodigoGrano;
            this.CentroAlta = CentroAlta;
            this.CentroDist = CentroDist;
            this.EsCyo = EsCyo;
        }

        //Enviar a todos en un solo mail (con los mails en un string separados por ;)
        //public override void InformarMail()
        //{
        //    IList<EmailInformado> EmailsInformados = new List<EmailInformado>();
        //    IList<Cupos> cupos = GetCuposAInformar();
        //    if (cupos.Count > 0)
        //    {
        //        IList<MailPdf> PdfsAEnviarPorMails = GenerarPdfsDistribucion(cupos);
        //        EmailsInformados = ObtenerServiceEInformar(CuentaVendedor, CuentaPuerto, CodigoGrano, PdfsAEnviarPorMails);
        //    }
        //}

        public IList<EmailInformado> ObtenerServiceEInformar(long CuentaVendedor, long CuentaPuerto, int CodigoGrano, IList<MailPdf> PdfsAEnviarPorMails, ISession session)
        {
            IList<EmailInformado> EmailsInformados = new List<EmailInformado>();
            foreach (MailPdf PdfAEnviar in PdfsAEnviarPorMails)
            {
                string CorreosElectronicosDestinatarios = base.GetEmails(CuentaVendedor, session);
                try
                {
                    ServiceEmailEstadoCupo ServicioEmail = GetServiceEmail(CuentaPuerto, CodigoGrano, PdfAEnviar.CuposAInformar.ElementAt(0).Fecha, PdfAEnviar.CuposAInformar.Count, session);
                    InformarYGuardarCupos(PdfAEnviar, ServicioEmail, CorreosElectronicosDestinatarios, session);
                    EmailsInformados.Add(new EmailInformado { Estado = 0, Mensaje = "OK", CuentaVendedor = CuentaVendedor, TipoEmail = this.TipoServicioInformar });
                }
                catch (ResourceServer.Models.Error.Exceptions.NeedEmailException e)
                {
                    EmailsInformados.Add(new EmailInformado { Estado = 1, Mensaje = "CORREO NO CONFIGURADO", CuentaVendedor = CuentaVendedor, TipoEmail = this.TipoServicioInformar });
                    throw e;
                }
                catch (Exception e)
                {
                    EmailsInformados.Add(new EmailInformado { Estado = 2, Mensaje = "ERROR", CuentaVendedor = CuentaVendedor, TipoEmail = this.TipoServicioInformar });
                    throw e;
                }
            }
            return EmailsInformados;
        }

        public void EnviarEmail(ServiceEmailEstadoCupo ServicioEmail, string CorreosElectronicosDestinatarios, MailPdf PdfAEnviar)
        {
            ServicioEmail.SendMailWithAttachments(CorreosElectronicosDestinatarios, PdfAEnviar.Pdf);
        }

        private IList<string> GetEmailsFromList(IList<string> Emails)
        {
            IList<string> mails = new List<string>();
            foreach (var mail in Emails)
            {
                if (!string.IsNullOrEmpty(mail))
                {
                    mails.Add(mail);
                }
            }
            return mails;
        }

        public IList<MailPdf> GenerarPdfsDistribucion(IList<Cupos> cupos)
        {
            IList<MailPdf> PdfsAEnviarPorMails = new List<MailPdf>();
            var cuposAgrupadosConsginacionAndFecha = cupos.GroupBy(x =>
                    new
                    {
                        Cuitsolicitante = x.Cuitsolicitante,
                        Cuitintermediario = x.Cuitintermediario,
                        Cuitrtecomercial = x.Cuitrtecomercial,
                        Cuitcorrcomp = x.Cuitcorrcomp,
                        Cuitmat = x.Cuitmat,
                        Cuitcorrvta = x.Cuitcorrvta,
                        Cuitrteent = x.Cuitrteent,
                        Cuitdestinatario = x.Cuitdestinatario,
                        Fecha = x.Fecha
                    }
                );
            ManagerPdf manager;
            MailPdf mail;
            Cupos consignacion;
            foreach (var c in cuposAgrupadosConsginacionAndFecha)
            {
                IList<Cupos> cupospdf = cupos.Where(x =>
                        x.Cuitsolicitante == c.Key.Cuitsolicitante &&
                        x.Cuitintermediario == c.Key.Cuitintermediario &&
                        x.Cuitrtecomercial == c.Key.Cuitrtecomercial &&
                        x.Cuitcorrcomp == c.Key.Cuitcorrcomp &&
                        x.Cuitmat == c.Key.Cuitmat &&
                        x.Cuitcorrvta == c.Key.Cuitcorrvta &&
                        x.Cuitrteent == c.Key.Cuitrteent &&
                        x.Cuitdestinatario == c.Key.Cuitdestinatario &&
                        x.Fecha == c.Key.Fecha
                    )
                    .ToList();
                mail = new MailPdf();
                consignacion = new Cupos() {
                    Cuitsolicitante = c.Key.Cuitsolicitante,
                    Cuitintermediario = c.Key.Cuitintermediario,
                    Cuitrtecomercial = c.Key.Cuitrtecomercial,
                    Cuitcorrcomp = c.Key.Cuitcorrcomp,
                    Cuitmat = c.Key.Cuitmat,
                    Cuitcorrvta = c.Key.Cuitcorrvta,
                    Cuitrteent = c.Key.Cuitrteent,
                    Cuitdestinatario = c.Key.Cuitdestinatario
                };
                mail.CuposAInformar = cupospdf;
                manager = new ManagerPdf(GetPdfBuilder(cupospdf));
                //string name = new Random(DateTime.Now.Second).NextDouble().ToString();
                string name = GetNombrePdf(cupospdf.ElementAt(0));
                manager.Construir("~/Models/Pdf/Files/" + name + ".pdf");
                mail.Consignacion = consignacion;
                mail.Pdf = name;
                PdfsAEnviarPorMails.Add(mail);
            }
            //if (PdfsAEnviarPorMails.Count == 0) throw new NoSeGeneraronPdfsException();
            return PdfsAEnviarPorMails;
        }

        private string GetNombrePdf(Cupos cupo)
        {
            return "LOGISTICA_ACA_" + cupo.Fecha.ToString("ddMMyyyy") + "_" + cupo.Uvcupodist.ToString();
        }

        protected abstract ServiceEmailEstadoCupo GetServiceEmail(long CuentaPuerto, int CodigoGrano, DateTime Fecha, int CantidadCupos, ISession Session);

        protected abstract IPdfBuilder GetPdfBuilder(IList<Cupos> Cupos);

        private void InformarYGuardarCupos(MailPdf MailPdf, ServiceEmailEstadoCupo ServicioEmail, string CorreosElectronicosDestinatarios, ISession Session)
        {
            ServicioCupo Servicio = new ServicioCupo();
            IList<Cupos> Encabezados = Servicio.ObtenerEncabezados(MailPdf.CuposAInformar, Session);
            foreach (var Cupo in MailPdf.CuposAInformar)
            {
                Cupo.GetEstado().Informar(Servicio.ObtenerEncabezado(Encabezados, Cupo), Session);
            }
            EnviarEmail(ServicioEmail, CorreosElectronicosDestinatarios, MailPdf);
        }

        public override IList<EmailInformado> InformarMails()
        {
            throw new NotImplementedException();
        }

        public override IList<EmailInformado> InformarMails(long CuentaVendedor)
        {
            if (CuentaVendedor == 0) throw new Exception("La cuenta del vendedor no puede ser igual a 0.");
            IList<ICupo> cupos = GetCuposInformar(CuentaVendedor);
            IList<MailPdf> PdfsAEnviarPorMails = GenerarPdfsDistribucion((IList<Cupos>)cupos);
            IList<EmailInformado> EmailsInformados = new List<EmailInformado>();
            using (ISession session = HibernateUtil.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    EmailsInformados = ObtenerServiceEInformar(CuentaVendedor, this.CuentaPuerto, this.CodigoGrano, PdfsAEnviarPorMails, session);
                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
            if (EmailsInformados.Any(x => x.Estado == 2)) throw new Exception("Alguno de los mails no se pudo enviar correctamente.");
            if (EmailsInformados.Any(x => x.Estado == 1)) throw new NeedEmailException();
            return EmailsInformados;
        }

        protected override abstract IList<ICupo> GetCuposInformar(long CuentaVendedor);
    }
}