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

    protected override ServiceEmail GetServiceEmail(long CuentaVendedor, TipoDestinatario tipoDestinatario, ISession Session)
    {
      if (TipoDestinatario.Vendedor == tipoDestinatario)
      {
        if (CuentaPuerto == 100007 || CuentaPuerto == 100212)
          return new EmailDistribucionConHorario(base.ListaCuposAInformar, Session);
        else
          return new EmailDistribucionMultiplesPdfs(base.ListaCuposAInformar, Session);
      }
      else if (TipoDestinatario.ContactoComercial == tipoDestinatario)
      {
        return new EmailDistribucionContactoComercial(base.ListaCuposAInformar, Session);
      }
      else
      {
        throw new NotImplementedException();
      }
    }
  }
}