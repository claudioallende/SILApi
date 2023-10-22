using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Pdf;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Email
{
  public class InformarAnulacionPorMail : ServicioInformarCupos
  {
    private long CuentaComprador { get; set; }
    private long CuentaPuerto { get; set; }
    private int CodigoGrano { get; set; }
    private bool EsCyo { get; set; }

    public InformarAnulacionPorMail(long CuentaComprador, long CuentaPuerto, int CodigoGrano, string CentroAlta, string CentroDist, bool EsCyo)
    //public InformarAnulacionPorMail(long CuentaComprador, long CuentaPuerto, int CodigoGrano, bool EsCyo)
    {
      this.CuentaComprador = CuentaComprador;
      this.CuentaPuerto = CuentaPuerto;
      this.CodigoGrano = CodigoGrano;
      this.EsCyo = EsCyo;
    }

    protected override string TipoServicioInformar
    {
      get
      {
        return "ANULACION";
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    protected override IList<Cupos> GetCuposInformar(long CuentaVendedor)
    {
      ICuposStore store = new CuposStore();
      if (this.EsCyo)
      {
        return store.FindCuerposAnuladosCyONoInformadosByKey(this.CuentaComprador, CuentaVendedor, this.CuentaPuerto, this.CodigoGrano);
      }
      else
      {
        return store.FindCuerposAnuladosNoInformadosByKey(this.CuentaComprador, CuentaVendedor, this.CuentaPuerto, this.CodigoGrano);
      }
    }

    protected override ServiceEmail GetServiceEmail(IList<Cupos> CuposAInformar, long CuentaVendedor, TipoDestinatario tipoDestinatario, ISession Session)
    {
      return new EmailAnulacionCupos(CuposAInformar, Session);
    }

    protected override void GuardarCupos(IList<Cupos> CuposInformar, ISession Session)
    {
      ServicioCupo Servicio = new ServicioCupo();
      IList<Cupos> Encabezados = Servicio.ObtenerEncabezados(CuposInformar, Session);
      foreach (var Cupo in CuposInformar)
      {
        Cupo.GetEstado().Informar(Servicio.ObtenerEncabezado(Encabezados, Cupo), Session);
      }
    }

    protected override void EnviarEmail(ServiceEmail ServicioEmail, string CorreosElectronicosDestinatarios)
    {
      ServicioEmail.SendMail(CorreosElectronicosDestinatarios);
    }
  }
}