using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace ResourceServer.Models.Email.IntegracionTerceros
{
  public class EmailDistribucionMUVIN : ServiceEmail
  {
    public EmailDistribucionMUVIN() {}
    protected override MailMessage GetMessage(MailMessage msg)
    {
      msg.Subject = GetAsunto();
      msg.IsBodyHtml = true;
      msg.Body = GetCuerpo();
      return msg;
    }

    protected string GetAsunto()
    {
      return "Asignación de Cupos ACA";
    }
    protected string GetCuerpo()
    {
      StringBuilder Cuerpo = new StringBuilder("Estimados: <br/><br/>");
      Cuerpo.Append("Informamos Distribucion: Adjuntamos PDF con datos de carta de porte.<br/>");
      Cuerpo.Append("Saludos cordiales.");
      return Cuerpo.ToString();
    }
  }
}