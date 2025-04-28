using NHibernate;
using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Email.IntegracionTerceros;
using ResourceServer.Models.Error.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Email
{
  public abstract class ServicioInformarCupos : ServicioInformarAVendedor
  {
    protected abstract string TipoServicioInformar { get; set; }

    public ServicioInformarCupos()
    {
    }

    protected abstract IList<Cupos> GetCuposInformar(long CuentaVendedor);

    public override IList<EmailInformado> InformarMails(long CuentaVendedor)
    {
      IList<EmailInformado> EmailsInformados = new List<EmailInformado>();
      IList<Cupos> CuposAInformar = GetCuposInformar(CuentaVendedor);
      long vendcta = (CuposAInformar != null && CuposAInformar.Any()) ? CuposAInformar.First().Vendcta : 0;
      ServicioInformarPdf MUVINService = new ServicioInformarPdf(TipoDeEmail.Anulacion, vendcta);
      if (CuposAInformar.Count > 0)
      {
        using (ISession session = HibernateUtil.OpenSession())
        using (ITransaction tx = session.BeginTransaction())
        {
          try
          {
            EmailsInformados = ObtenerServiceEInformar(CuposAInformar, CuentaVendedor, session);
            tx.Commit();
          }
          catch
          {
            tx.Rollback();
            throw;
          }
        }
        MUVINService.InformarMails(CuposAInformar.Cast<ICupo>().ToList());
      }
      return EmailsInformados;
    }

    public IList<EmailInformado> ObtenerServiceEInformar(IList<Cupos> CuposAInformar, long CuentaVendedor, ISession session)
    {
      IList<EmailInformado> EmailsInformados = new List<EmailInformado>();
      string CorreosElectronicosDestinatarios = GetEmails(CuentaVendedor, session);
      try
      {
        //En caso de que haya solo 1 pdf que informar el dto va null
        ServiceEmail ServicioEmailVendedor = GetServiceEmail(CuposAInformar, CuentaVendedor, CuposAInformar, TipoDestinatario.Vendedor, session);
        EnviarEmail(ServicioEmailVendedor, CorreosElectronicosDestinatarios);
        InformarContactosComerciales(CuposAInformar, session);
        GuardarCupos(CuposAInformar, session);
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

    public void InformarContactosComerciales(IList<Cupos> ListaCuposAInformar, ISession Session)
    {
      var ContactosComerciales = ListaCuposAInformar.Where(x => !string.IsNullOrEmpty(x.ContactoComercial)).SelectMany(x => x.ContactoComercial.Split(';')).Distinct().ToList();
      foreach (string ContactoComercial in ContactosComerciales)
      {
        if (long.TryParse(ContactoComercial, out long CuentaContactoComercial))
        {
          string CorreosElectronicosDestinatarios = GetEmails(CuentaContactoComercial, Session);
          IList<Cupos> ListaCuposContactoComercial = ListaCuposAInformar.Cast<Cupos>().Where(x => !string.IsNullOrEmpty(x.ContactoComercial) && x.ContactoComercial.Split(';').Any(y => y == ContactoComercial)).ToList();
          ServiceEmail serviceEmailContactoComercial = GetServiceEmail(ListaCuposContactoComercial, CuentaContactoComercial, ListaCuposContactoComercial, TipoDestinatario.ContactoComercial, Session);
          EnviarEmail(serviceEmailContactoComercial, CorreosElectronicosDestinatarios);
        }
      }
    }

    protected abstract void GuardarCupos(IList<Cupos> CuposInformar, ISession session);

    protected abstract void EnviarEmail(ServiceEmail ServicioEmail, string CorreosElectronicosDestinatarios);

    protected abstract ServiceEmail GetServiceEmail(IList<Cupos> CuposAInformar, long CuentaVendedor, IList<Cupos> ListaCupos, TipoDestinatario tipoDestinatario, NHibernate.ISession Session);
  }
}