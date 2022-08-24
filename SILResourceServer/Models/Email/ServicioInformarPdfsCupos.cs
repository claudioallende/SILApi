using NHibernate;
using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Email
{
    public abstract class ServicioInformarPdfsCupos : ServicioInformar
    {
        protected long CuentaComprador { get; set; }
        protected long CuentaPuerto { get; set; }
        protected int CodigoGrano { get; set; }
        protected string CentroAlta { get; set; }
        protected string CentroDist { get; set; }
        protected bool EsCyo { get; set; }
        protected abstract string TipoServicioInformar { get; set; }
        protected ServicioCupo ServicioCupo { get; set; }

        public ServicioInformarPdfsCupos(long CuentaComprador, long CuentaPuerto, int CodigoGrano, string CentroAlta, string CentroDist, bool EsCyo)
        {
            this.ServicioCupo = new ServicioCupo();
            this.CuentaComprador = CuentaComprador;
            this.CuentaPuerto = CuentaPuerto;
            this.CodigoGrano = CodigoGrano;
            this.CentroAlta = CentroAlta;
            this.CentroDist = CentroDist;
            this.EsCyo = EsCyo;
        }

        public override IList<EmailInformado> InformarMails()
        {
            throw new NotImplementedException();
        }

        public override IList<EmailInformado> InformarMails(long CuentaVendedor)
        {
            IList<EmailInformado> EmailsInformados = new List<EmailInformado>();
            IList<Cupos> cupos = (IList<Cupos>)GetCuposInformar(CuentaVendedor);
            if (cupos.Count > 0)
            {
                IList<PdfCupos> PdfsAEnviarPorMails = GenerarPdfsDistribucion(cupos);
                using (ISession session = HibernateUtil.OpenSession())
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        EmailsInformados = ObtenerServiceEInformar(CuentaVendedor, CuentaPuerto, CodigoGrano, PdfsAEnviarPorMails, session);
                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
            return EmailsInformados;
        }

        protected IList<PdfCupos> GenerarPdfsDistribucion(IList<Cupos> cupos)
        {
            IList<PdfCupos> PdfsAEnviarPorMails = new List<PdfCupos>();
            IList<CuposPorConsignacionYFechaDTO> CuposAgrupados = ServicioCupo.AgruparCuposPorConsignacion(cupos);
            foreach (CuposPorConsignacionYFechaDTO CuposPorConsignacionYFecha in CuposAgrupados)
            {
                PdfsAEnviarPorMails.Add(GenerarPdfDistribucion(CuposPorConsignacionYFecha));
            }
            return PdfsAEnviarPorMails;
        }

        protected PdfCupos GenerarPdfDistribucion(CuposPorConsignacionYFechaDTO CuposAgrupados)
        {
            string nombrePDF = GetNombrePdf(CuposAgrupados.Cupos);
            PdfCupos PdfCupos = new PdfCupos() { Consignacion = CuposAgrupados.Consignacion, Pdf = nombrePDF, CuposAInformar = CuposAgrupados.Cupos };
            ManagerPdf manager = new ManagerPdf(GetPdfBuilder(PdfCupos.CuposAInformar));
            manager.Construir("~/Models/Pdf/Files/" + nombrePDF + ".pdf");
            return PdfCupos;
        }

        private string GetNombrePdf(IList<Cupos> cupos)
        {
            return GetNombrePdf(cupos.ElementAt(0));
        }

        private string GetNombrePdf(Cupos cupo)
        {
            return "LOGISTICA_ACA_" + cupo.Fecha.ToString("ddMMyyyy") + "_" + cupo.Uvcupodist.ToString();
        }

        public IList<EmailInformado> ObtenerServiceEInformar(long CuentaVendedor, long CuentaPuerto, int CodigoGrano, IList<PdfCupos> PdfsAEnviarPorMails, ISession session)
        {
            IList<EmailInformado> EmailsInformados = new List<EmailInformado>();
            //foreach (PdfCupos PdfAEnviar in PdfsAEnviarPorMails)
            //{
                string CorreosElectronicosDestinatarios = base.GetEmails(CuentaVendedor, session);
                try
                {
                    ServiceEmailEstadoCupo ServicioEmail = GetServiceEmail(new MailPdfDTO(), session);
                    InformarYGuardarCupos(PdfsAEnviarPorMails, ServicioEmail, CorreosElectronicosDestinatarios, session);
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
            //}
            return EmailsInformados;
        }

        private void InformarYGuardarCupos(IList<PdfCupos> MailPdf, ServiceEmailEstadoCupo ServicioEmail, string CorreosElectronicosDestinatarios, ISession Session)
        {
            ServicioCupo Servicio = new ServicioCupo();
            IList<Cupos> TotalCuposAInformar = MailPdf.SelectMany(x => x.CuposAInformar).ToList();
            IList<Cupos> Encabezados = Servicio.ObtenerEncabezados(TotalCuposAInformar, Session);
            foreach (var Cupo in TotalCuposAInformar)
            {
                Cupo.GetEstado().Informar(Servicio.ObtenerEncabezado(Encabezados, Cupo), Session);
            }
            EnviarEmail(ServicioEmail, CorreosElectronicosDestinatarios, MailPdf);
        }

        public void EnviarEmail(ServiceEmailEstadoCupo ServicioEmail, string CorreosElectronicosDestinatarios, IList<PdfCupos> PdfsAEnviar)
        {
            ServicioEmail.SendMailWithAttachments(CorreosElectronicosDestinatarios, PdfsAEnviar.Select(x => x.Pdf).ToList());
        }

        protected abstract ServiceEmailEstadoCupo GetServiceEmail(MailPdfDTO dto,ISession Session);

        protected override abstract IList<ICupo> GetCuposInformar(long CuentaVendedor);

        protected abstract IPdfBuilder GetPdfBuilder(IList<Cupos> Cupos);
    }
}