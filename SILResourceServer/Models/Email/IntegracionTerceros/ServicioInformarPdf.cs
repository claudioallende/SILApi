using NHibernate;
using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Error.Exceptions;
using ResourceServer.Models.Pdf;
using ResourceServer.Models.Pdf.IntegracionTerceros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Threading.Tasks;
using System.Web;

namespace ResourceServer.Models.Email.IntegracionTerceros
{
  public class ServicioInformarPdf: ServicioInformarTerceros
  {
    private SmtpSection Configuracion { get; set; }
    protected string TipoServicioInformar { get; set; }
    public TipoDeEmail TipoDeEmail { get; set; }
    public Vendedor DatosDelVendedor { get; set; } = default;
    protected ServicioCupo ServicioCupo { get; set; }

    public ServicioInformarPdf(TipoDeEmail tipoDeEmail, long vendcta)
    {
      this.ServicioCupo = new ServicioCupo();
      if (vendcta != 0)
        SetVendedor(vendcta);
      TipoDeEmail = tipoDeEmail;
    }
    public override IList<EmailInformado> InformarMails(IList<ICupo> cuposAInformar)
    {
      List<EmailInformado> EmailsInformados = new List<EmailInformado>();
      if (DatosDelVendedor is null || DatosDelVendedor == default)
        SetVendedor(cuposAInformar.First().Vendcta);
      if (cuposAInformar != null && cuposAInformar.Count > 0)
      {
        IList<PdfCupos> PdfsAEnviarPorMails = GenerarPdfs(cuposAInformar.Cast<Cupos>().ToList());
        EmailsInformados.AddRange(GenerarPdfsEInformar(PdfsAEnviarPorMails, cuposAInformar));
      }
      return EmailsInformados;
    }
    private void SetVendedor(long cuenta) 
    {
      try 
      {
        IVendedorStore store = new VendedorStore();
        IList<Vendedor> vendedores = store.FindVendedorByCuit(cuenta.ToString());
        if (vendedores != null && vendedores.Any())
          DatosDelVendedor = vendedores.First();
        else
          DatosDelVendedor = null;
      } 
      catch (Exception ex) 
      { 
        throw; 
      }
    }
    protected IList<PdfCupos> GenerarPdfs(IList<Cupos> cupos)
    {
      IList<PdfCupos> PdfsAEnviarPorMails = new List<PdfCupos>();
      IList<CuposPorConsignacionYFechaDTO> CuposAgrupados = ServicioCupo.AgruparCuposPorConsignacion(cupos);
      foreach (CuposPorConsignacionYFechaDTO CuposPorConsignacionYFecha in CuposAgrupados)
      {
        PdfsAEnviarPorMails.Add(GenerarPdf(CuposPorConsignacionYFecha));
      }
      return PdfsAEnviarPorMails;
    }
    protected PdfCupos GenerarPdf(CuposPorConsignacionYFechaDTO CuposAgrupados)
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
      if (TipoDeEmail == TipoDeEmail.Distribucion)
        return "Asignación de Cupos ACA - LOGISTICA_ACA_" + cupo.Fecha.ToString("ddMMyyyy") + "_" + Guid.NewGuid().ToString().Substring(0, 8);
      if (TipoDeEmail == TipoDeEmail.Anulacion)
        return "Anulación de Cupos ACA - LOGISTICA_ACA_" + cupo.Fecha.ToString("ddMMyyyy") + "_" + Guid.NewGuid().ToString().Substring(0, 8);

      return "NoIdentificado - LOGISTICA_ACA_" + cupo.Fecha.ToString("ddMMyyyy") + "_" + Guid.NewGuid().ToString().Substring(0, 8);
    }
    private IList<EmailInformado> GenerarPdfsEInformar(IList<PdfCupos> PdfsAEnviarPorMails, IList<ICupo> ListaCupos)
    {
      IList<EmailInformado> EmailsInformados = new List<EmailInformado>();
      using (ISession session = HibernateUtil.OpenSession())
      {
        if (ListaCupos.Count > 0)
        {
          EmailsInformados = ObtenerServiceEInformar(PdfsAEnviarPorMails, ListaCupos, session);
        }
      }
      return EmailsInformados;
    }
    public IList<EmailInformado> ObtenerServiceEInformar(IList<PdfCupos> PdfsAEnviarPorMails, IList<ICupo> ListaCupos, ISession session)
    {
      IList<EmailInformado> EmailsInformados = new List<EmailInformado>();
      string CorreosElectronicosDestinatarios = GetEmails();
      try
      {
        ServiceEmail ServicioEmail = GetServiceEmail(ListaCupos, session);
        EnviarEmail(ServicioEmail, CorreosElectronicosDestinatarios, PdfsAEnviarPorMails);
        EmailsInformados.Add(new EmailInformado { Estado = 0, Mensaje = "OK", TipoEmail = this.TipoServicioInformar });
      }
      catch (NeedEmailException e)
      {
        EmailsInformados.Add(new EmailInformado { Estado = 1, Mensaje = "CORREO NO CONFIGURADO", TipoEmail = this.TipoServicioInformar });
        throw e;
      }
      catch (Exception e)
      {
        EmailsInformados.Add(new EmailInformado { Estado = 2, Mensaje = "ERROR",  TipoEmail = this.TipoServicioInformar });
        throw e;
      }
      return EmailsInformados;
    }
    public void EnviarEmail(ServiceEmail ServicioEmail, string CorreosElectronicosDestinatarios, IList<PdfCupos> PdfsAEnviar)
    {
      ServicioEmail.SendMailWithAttachments(CorreosElectronicosDestinatarios, PdfsAEnviar.Select(x => x.Pdf).ToList());
    }
    protected ServiceEmail GetServiceEmail(IList<ICupo> ListaCupos, ISession Session) 
    {
      if (TipoDeEmail == TipoDeEmail.Distribucion) 
      {
        return new EmailDistribucionMUVIN();
      }
      if (TipoDeEmail == TipoDeEmail.Anulacion) 
      {
        return new EmailAnulacionMUVIN();
      }
      throw new NotImplementedException();
    }
    protected IPdfBuilder GetPdfBuilder(IList<Cupos> Cupos) 
    {
      return new PdfInformeMUVINBuilder(Cupos, DatosDelVendedor);
    }
  }
}