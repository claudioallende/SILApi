using ResourceServer.Models.DataAccess;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Configuracion
{
    public class ServicioConfiguracionEmail
    {
        IDTablaStore StoreDtabla { get; set; }
        private char SeparadorEMails { get; set; }

        public ServicioConfiguracionEmail()
        {
            SeparadorEMails = ';';
            StoreDtabla = new DTablaStore();
        }

        public void SaveEmails(string Cuenta, string Nombre, string Emails)
        {
            string[] mails = GetMails(Emails);
            IList<DTabla> EmailExistente = StoreDtabla.FindEmailByClaveCuenta(Cuenta);
            if (EmailExistente.Count > 0)
            {
                UpdateEmails(EmailExistente, mails[0], mails[1], mails[2], mails[3], mails[4]);
            }
            else
            {
                StoreDtabla.SaveEmails(Cuenta, Nombre, mails[0], mails[1], mails[2], mails[3], mails[4]);
            }
        }

        public string[] GetMails(string Emails)
        {
            string Email1 = "";
            string Email2 = "";
            string Email3 = "";
            string Email4 = "";
            string Email5 = "";
            if (Emails.Length > 120)
            {
                var ArrayEmails = GetArrayOfEmails(Emails);
                foreach (var Email in ArrayEmails)
                {
                    if (Email4.Length + Email.Length > 120)
                    {
                        Email5 += Email + SeparadorEMails;
                    }
                    else
                    {

                        if (Email3.Length + Email.Length > 120)
                        {
                            Email4 += Email + SeparadorEMails;
                        }
                        else
                        {

                            if (Email2.Length + Email.Length > 120)
                            {
                                Email3 += Email + SeparadorEMails;
                            }
                            else
                            {

                                if (Email1.Length + Email.Length > 120)
                                {
                                    Email2 += Email + SeparadorEMails;
                                }
                                else
                                {
                                    Email1 += Email + SeparadorEMails;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Email1 = Emails;
            }
            return new string[] { QuitaUltimoSeparador(Email1), QuitaUltimoSeparador(Email2), QuitaUltimoSeparador(Email3), QuitaUltimoSeparador(Email4), QuitaUltimoSeparador(Email5) };
        }

        public string QuitaUltimoSeparador(string str)
        {
            str = str.Trim();
            if (str != null && str.Length > 0 && str[str.Length - 1] == SeparadorEMails)
            {
                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }

        private void UpdateEmails(IList<DTabla> lista, string Email1, string Email2, string Email3, string Email4, string Email5)
        {
            DTabla ObjetoEmail1 = lista.ElementAt(0);
            ObjetoEmail1.Valor = Email1;
            DTabla ObjetoEmail2 = lista.ElementAt(1);
            ObjetoEmail2.Valor = Email2;
            DTabla ObjetoEmail3 = lista.ElementAt(2);
            ObjetoEmail3.Valor = Email3;
            DTabla ObjetoEmail4 = lista.ElementAt(3);
            ObjetoEmail4.Valor = Email4;
            DTabla ObjetoEmail5 = lista.ElementAt(4);
            ObjetoEmail5.Valor = Email5;
            StoreDtabla.UpdateEmails(ObjetoEmail1, ObjetoEmail2, ObjetoEmail3,
                ObjetoEmail4, ObjetoEmail5);
        }

        public string[] GetArrayOfEmails(string Emails)
        {
            return Emails.Split(SeparadorEMails);
        }

        public string GetEmailsByCuit(string Cuit)
        {
            List<string> emails = new List<string>();
            string emailResult = "";
            if (!string.IsNullOrEmpty(Cuit))
            {
                Cuit = Cuit.Replace("-", string.Empty);
                emails = (List<string>)StoreDtabla.FindEmailByCuit(Cuit);
                if (emails.Count > 0)
                {
                    foreach (string var in emails)
                    {
                        if (!string.IsNullOrEmpty(var))
                        {
                            emailResult += ((emailResult.Length == 0) ? var : SeparadorEMails + var);
                        }
                    }
                }
            }
            return emailResult;
        }

        public string GetEmailsByClave(string Clave)
        {
            List<string> emails = new List<string>();
            string emailResult = "";
            if (!string.IsNullOrEmpty(Clave))
            {
                emails = (List<string>)StoreDtabla.GetEmailsByClave(Clave);
                if (emails.Count > 0)
                {
                    foreach (string var in emails)
                    {
                        if (!string.IsNullOrEmpty(var))
                        {
                            emailResult += ((emailResult.Length == 0) ? var : SeparadorEMails + var);
                        }
                    }
                }
            }
            return emailResult;
        }

        public string GetEmailsByClave(string Clave, ISession Session)
        {
            List<string> emails = new List<string>();
            string emailResult = "";
            if (!string.IsNullOrEmpty(Clave))
            {
                emails = (List<string>)StoreDtabla.GetEmailsByClave(Clave, Session);
                if (emails.Count > 0)
                {
                    foreach (string var in emails)
                    {
                        if (!string.IsNullOrEmpty(var))
                        {
                            emailResult += ((emailResult.Length == 0) ? var : SeparadorEMails + var);
                        }
                    }
                }
            }
            return emailResult;
        }
    }
}