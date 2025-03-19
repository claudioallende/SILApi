using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Pdf;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Email
{
  public class InformarDistribucionPorMail : ServicioInformarCuposPdf
  {
    public InformarDistribucionPorMail(long CuentaComprador, long CuentaPuerto, int CodigoGrano, string CentroAlta, string CentroDist, bool EsCyo)
        : base(CuentaComprador, CuentaPuerto, CodigoGrano, CentroAlta, CentroDist, EsCyo)
    {
    }

    protected override IPdfBuilder GetPdfBuilder(IList<Cupos> Cupos)
    {
      if (Cupos != null && Cupos.Any(x => x.Puerto == 100007))
        return new PdfDistribucionSanLorenzoBuilder(Cupos);
      else
        return new PdfDistribucionBuilder(Cupos);
    }

    protected override string TipoServicioInformar
    {
      get
      {
        return "DISTRIBUCION";
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    protected override IList<ICupo> GetCuposInformar(long CuentaVendedor)
    {
      ICuposStore store = new CuposStore();
      if (base.EsCyo)
      {
        return store.FindCuerposDistribuidosCyONoInformadosByKey(base.CuentaComprador, CuentaVendedor, base.CuentaPuerto, base.CodigoGrano);
      }
      else
      {
        return store.FindCuerposDistribuidosNoInformadosByKey(base.CuentaComprador, CuentaVendedor, base.CuentaPuerto, base.CodigoGrano);
      }
    }

    protected override ServiceEmail GetServiceEmail(long CuentaVendedor, IList<ICupo> ListaCupos, TipoDestinatario tipoDestinatario, ISession Session)
    {
      if (TipoDestinatario.Vendedor == tipoDestinatario)
      {
        if(CuentaPuerto == 100007)
          return new EmailDistribucionPuertoSanLorenzo(ListaCupos, Session);
        else if (CuentaPuerto == 100212)
          return new EmailDistribucionConHorario(ListaCupos, Session);
        else
          return new EmailDistribucionMultiplesPdfs(ListaCupos, Session);
      }
      else if (TipoDestinatario.ContactoComercial == tipoDestinatario)
      {
        return new EmailDistribucionContactoComercial(ListaCupos, Session);
      }
      else
      {
        throw new NotImplementedException();
      }
    }
  }
}