using ResourceServer.Models.Error;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;

namespace ResourceServer.Models.Email
{
    public abstract class ServiceEmail
    {
        private SmtpSection Configuracion { get; set; }
        private SmtpClient Smtp { get; set; }
        public ServiceEmail()
        {
            Smtp = GetSmtp();
        }
        public SmtpClient GetSmtp()
        {
            Configuracion = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            SmtpClient smtp = new SmtpClient();
            return smtp;
        }
        public void SendMail(string Email)
        {
            Smtp.Send(GetMail(Email));
        }
        public void SendMailWithAttachment(string Email, string path)
        {

        }
        public bool SendMailWithAttachments(string Email, IList<string> paths)
        {
            try
            {
                MailMessage msg = GetMail(Email);
                foreach (var path in paths)
                {
                    msg.Attachments.Add(new Attachment(HttpContext.Current.Server.MapPath("~/Models/Pdf/Files/" + path + ".pdf")));
                }
                Smtp.Send(msg);
                return true;
            }
            catch (Exception e)
            {
                ErrorLog.Write(e);
                return false;
            }
        }
        public string SendMailWithAttachments(string Email, string path)
        {
            if (!string.IsNullOrEmpty(Email))
            {
                try
                {
                    MailMessage msg = GetMail(Email);
                    msg.Attachments.Add(new Attachment(HttpContext.Current.Server.MapPath("~/Models/Pdf/Files/" + path + ".pdf")));
                    Smtp.Send(msg);
                    return "OK";
                }
                catch (Exception e)
                {
                    ErrorLog.Write(e);
                    throw e;
                }
            }
            else
            {
                throw new ResourceServer.Models.Error.Exceptions.NeedEmailException();
            }
        }

        private MailMessage GetMail(string Email)
        {
            MailMessage msg = new MailMessage();
            string FromMail = "";
            if (!string.IsNullOrEmpty(Configuracion.From))
            {
                FromMail = Configuracion.From;
            }
            MailAddress Desde = new MailAddress(FromMail);
            msg.From = Desde;
            string[] Destinos = Email.Split(';');
            foreach (var Destino in Destinos)
            {
                if (!string.IsNullOrEmpty(Destino)) msg.To.Add(Destino);
            }
            return GetMessage(msg);
        }

        protected abstract MailMessage GetMessage(MailMessage msg);
    }
}