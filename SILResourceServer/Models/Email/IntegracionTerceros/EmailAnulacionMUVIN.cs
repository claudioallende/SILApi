using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace ResourceServer.Models.Email.IntegracionTerceros
{
  public class EmailAnulacionMUVIN : ServiceEmail
  {
    public EmailAnulacionMUVIN(){}
    protected override MailMessage GetMessage(MailMessage msg)
    {
      msg.Subject = GetAsunto();
      msg.IsBodyHtml = true;
      msg.Body = GetCuerpo();
      return msg;
    }

    protected string GetAsunto()
    {
      return "Anulación de Cupos ACA";
    }
    protected string GetCuerpo()
    {
      StringBuilder Cuerpo = new StringBuilder("Estimados: <br/><br/>");
      Cuerpo.Append("Informamos Anulación de Distribucion: Adjuntamos PDF con datos de carta de porte.<br/>");
      Cuerpo.Append("Saludos cordiales.");
      return Cuerpo.ToString();
    }
  }
}