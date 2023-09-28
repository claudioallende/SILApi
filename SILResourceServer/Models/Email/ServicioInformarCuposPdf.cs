using NHibernate;
using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Error.Exceptions;
using ResourceServer.Models.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Web;

namespace ResourceServer.Models.Email
{
  public abstract class ServicioInformarCuposPdf : ServicioInformarAVendedor
  {
    private SmtpSection Configuracion { get; set; }
    protected long CuentaComprador { get; set; }
    protected abstract string TipoServicioInformar { get; set; }
    protected long CuentaPuerto { get; set; }
    protected int CodigoGrano { get; set; }
    protected string CentroAlta { get; set; }
    protected string CentroDist { get; set; }
    protected bool EsCyo { get; set; }
    protected ServicioCupo ServicioCupo { get; set; }
    protected IList<ICupo> ListaCuposAInformar { get; set; }

    public ServicioInformarCuposPdf(long CuentaComprador, long CuentaPuerto, int CodigoGrano, string CentroAlta, string CentroDist, bool EsCyo)
    {
      this.ServicioCupo = new ServicioCupo();
      this.CuentaComprador = CuentaComprador;
      this.CuentaPuerto = CuentaPuerto;
      this.CodigoGrano = CodigoGrano;
      this.CentroAlta = CentroAlta;
      this.CentroDist = CentroDist;
      this.EsCyo = EsCyo;
    }

    /// <summary>
    /// Libera los cupos padre cyo que estaban bloqueados para distribuir ya que el hijo estaba pendiente de informar.
    /// </summary>
    /// <param name="Cupos"></param>
    /// <param name="Session"></param>
    public void LiberarCuposPadreCYO(IList<Cupos> Cupos, ISession Session)
    {
      ServicioCupo serviciocupo = new ServicioCupo();
      IList<Cupos> CuposPadreConHijoPendienteInformar = serviciocupo.GetCuposPadreCyOConHijosPendienteInformar(serviciocupo.FiltrarCuposCuentaYOrdenHijoNoAnulados(Cupos), Session);
      ResourceServer.Models.Cupo.EstadoCupo Estado;
      foreach (Cupos Cupo in CuposPadreConHijoPendienteInformar)
      {
        Estado = Cupo.GetEstado();
        if (Estado.Codigo == Models.Cupo.CodigoEstado.CuentaYOredenPendienteDistribuirHijoPendienteInformar)
        {
          Estado.Informar(null, Session); //No es necesario el encabezado ya que lo unico que hace es borrar el valor UvCupoDist
        }
      }
    }

    //Enviar a todos en un solo mail (con los mails en un string separados por ;)
    public override IList<EmailInformado> InformarMails(long CuentaVendedor)
    {
      IList<EmailInformado> EmailsInformados = new List<EmailInformado>();
      this.ListaCuposAInformar = GetCuposInformar(CuentaVendedor);
      if (this.ListaCuposAInformar.Count > 0)
      {
        IList<PdfCupos> PdfsAEnviarPorMails = GenerarPdfsDistribucion(this.ListaCuposAInformar.Cast<Cupos>().ToList());

        GenerarPdfsEInformar(CuentaVendedor, PdfsAEnviarPorMails);

        var ContactosComerciales = this.ListaCuposAInformar.Cast<Cupos>().SelectMany(x => x.ContactoComercial?.Split(';')).GroupBy(x => x).Select(x => x.Key);
        foreach (string ContactoComercial in ContactosComerciales)
        {
          IList<Cupos> ListaCuposContactoComercial = this.ListaCuposAInformar.Cast<Cupos>().Where(x => x.ContactoComercial.Split(';').Any(y => y == ContactoComercial)).ToList();
          IList<PdfCupos> PdfsAEnviarPorMailsContactoComercial = GenerarPdfsDistribucion(ListaCuposContactoComercial);

          GenerarPdfsEInformar(long.Parse(ContactoComercial), PdfsAEnviarPorMailsContactoComercial);
        }

        using (ISession session = HibernateUtil.OpenSession())
        using (ITransaction tx = session.BeginTransaction())
        {
          try
          {
            CambiarEstadoCupos(PdfsAEnviarPorMails, session);
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

    private IList<EmailInformado> GenerarPdfsEInformar(long Cuenta, IList<PdfCupos> PdfsAEnviarPorMails)
    {
      IList<EmailInformado> EmailsInformados = new List<EmailInformado>();
      using (ISession session = HibernateUtil.OpenSession())
      {
        if (this.ListaCuposAInformar.Count > 0)
        {
          EmailsInformados = ObtenerServiceEInformar(Cuenta, CuentaPuerto, CodigoGrano, PdfsAEnviarPorMails, session);
        }
      }
      return EmailsInformados;
    }

    public IList<EmailInformado> ObtenerServiceEInformar(long CuentaVendedor, long CuentaPuerto, int CodigoGrano, IList<PdfCupos> PdfsAEnviarPorMails, ISession session)
    {
      IList<EmailInformado> EmailsInformados = new List<EmailInformado>();
      string CorreosElectronicosDestinatarios = GetEmails(CuentaVendedor, session);
      try
      {
        //En caso de que haya solo 1 pdf que informar el dto va null
        ServiceEmail ServicioEmail = GetServiceEmail(CuentaVendedor, session);
        EnviarEmail(ServicioEmail, CorreosElectronicosDestinatarios, PdfsAEnviarPorMails);
        EmailsInformados.Add(new EmailInformado { Estado = 0, Mensaje = "OK", CuentaVendedor = CuentaVendedor, TipoEmail = this.TipoServicioInformar });
      }
      catch (NeedEmailException e)
      {
        EmailsInformados.Add(new EmailInformado { Estado = 1, Mensaje = "CORREO NO CONFIGURADO", CuentaVendedor = CuentaVendedor, TipoEmail = this.TipoServicioInformar });
        throw e;
      }
      catch (Exception e)
      {
        EmailsInformados.Add(new EmailInformado { Estado = 2, Mensaje = "ERROR", CuentaVendedor = CuentaVendedor, TipoEmail = this.TipoServicioInformar });
        throw e;
      }
      return EmailsInformados;
    }

    public void EnviarEmail(ServiceEmail ServicioEmail, string CorreosElectronicosDestinatarios, MailPdf PdfAEnviar)
    {
      ServicioEmail.SendMailWithAttachments(CorreosElectronicosDestinatarios, PdfAEnviar.Pdf);
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
      return "LOGISTICA_ACA_" + cupo.Fecha.ToString("ddMMyyyy") + "_" + Guid.NewGuid().ToString().Substring(0, 8);
    }

    private void CambiarEstadoCupos(IList<PdfCupos> MailPdf, ISession Session)
    {
      ServicioCupo Servicio = new ServicioCupo();
      IList<Cupos> TotalCuposAInformar = MailPdf.SelectMany(x => x.CuposAInformar).ToList();
      IList<Cupos> Encabezados = Servicio.ObtenerEncabezados(TotalCuposAInformar, Session);
      foreach (var Cupo in TotalCuposAInformar)
      {
        Cupo.GetEstado().Informar(Servicio.ObtenerEncabezado(Encabezados, Cupo), Session);
      }
    }

    //private void InformarYGuardarCupos(IList<PdfCupos> MailPdf, string EmailVendedor, ServiceEmail ServicioEmail, ISession Session)
    //{
    //  ServicioCupo Servicio = new ServicioCupo();
    //  var CuentasAInformar = MailPdf.GroupBy(x => x.CuposAInformar.)
    //  IList<string> CorreosElectronicosDestinatarios = new List<string> { EmailVendedor };
    //  IList<Cupos> TotalCuposAInformar = MailPdf.SelectMany(x => x.CuposAInformar).ToList();
    //  IList<Cupos> Encabezados = Servicio.ObtenerEncabezados(TotalCuposAInformar, Session);
    //  foreach (var Cupo in TotalCuposAInformar)
    //  {
    //    Cupo.GetEstado().Informar(Servicio.ObtenerEncabezado(Encabezados, Cupo), Session);
    //  }
    //  EnviarEmail(ServicioEmail, CorreosElectronicosDestinatarios, MailPdf);
    //}

    public void EnviarEmail(ServiceEmail ServicioEmail, string CorreosElectronicosDestinatarios, IList<PdfCupos> PdfsAEnviar)
    {
      ServicioEmail.SendMailWithAttachments(CorreosElectronicosDestinatarios, PdfsAEnviar.Select(x => x.Pdf).ToList());
    }

    protected abstract IList<ICupo> GetCuposInformar(long CuentaVendedor);

    //Sacar los parametros porque para cada ServiceEmail los constructores pueden diferir en los parametros
    protected abstract ServiceEmail GetServiceEmail(long CuentaVendedor, ISession Session);

    protected abstract IPdfBuilder GetPdfBuilder(IList<Cupos> Cupos);
  }
}