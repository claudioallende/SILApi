using NHibernate;
using ResourceServer.Models.Configuracion;
using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Email.IntegracionTerceros
{
  public abstract class ServicioInformarTerceros
  {
    /// <summary>
    /// Obtiene un string de emails separados por punto y coma (;)
    /// </summary>
    /// <returns></returns>
    public string GetEmails()
    {
      string email = ConfigurationManager.AppSettings["MUVIN_Email"];
      if (string.IsNullOrEmpty(email))
        return "allendeclaudio22@gmail.com";
      else
        return email;
    }

    /// <summary>
    /// Creo una lista de correos no vacíos.
    /// </summary>
    /// <param name="Emails"></param>
    /// <returns></returns>
    protected IList<string> GetEmailsFromList(IList<string> Emails)
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

    /// <summary>
    /// Informa por correo a todos los mails al configurados.
    /// </summary>
    /// <param name="CuentaVendedor"></param>
    public abstract IList<EmailInformado> InformarMails(IList<ICupo> cuposAInformar);
  }
}