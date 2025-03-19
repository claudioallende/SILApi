using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ResourceServer.Models.Email
{
  public class EmailDistribucionPuertoSanLorenzo: EmailDistribucionConHorario
  {
    public EmailDistribucionPuertoSanLorenzo(IList<ICupo> ListaCupos, ISession Session)
        : base(ListaCupos, Session)
    {
    }
    protected override MailMessage GetMessage(MailMessage msg)
    {
      MailMessage message = base.GetMessage(msg);
      message.Body += string.Format("<br><br>Para agilizar tu operatoria en el puerto, bajate nuestra app <b>AL2</b> y pag&aacute; la tasa de sobrecarga de una manera f&aacute;cil y segura. <a href='https://onelink.al2.com.ar/wZrT/srlpzqf4?af_qr=true'>Descargá AL2 desde tu celular aquí </a>.<br><br>");
      return msg;
    }
  }
}