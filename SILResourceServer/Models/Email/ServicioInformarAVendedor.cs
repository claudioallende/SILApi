using NHibernate;
using ResourceServer.Models.Configuracion;
using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Email
{
    public abstract class ServicioInformarAVendedor
    {
        /// <summary>
        /// Obtiene un string de emails separados por punto y coma (;)
        /// </summary>
        /// <param name="CuentaVendedor"></param>
        /// <param name="Session"></param>
        /// <returns></returns>
        public string GetEmails(long CuentaVendedor, ISession Session)
        {
            IVendedorStore VendedorStore = new VendedorStore();
            Vendedor Vendedor = VendedorStore.FindById(CuentaVendedor, Session);
            ServicioConfiguracionEmail serviciomail = new ServicioConfiguracionEmail();
            string mails = "";
            string mailsObtenidos = "";
            mailsObtenidos = serviciomail.GetEmailsByClave(Vendedor.Cuenta.ToString(), Session);
            mails = string.IsNullOrEmpty(mailsObtenidos) ? "" : mailsObtenidos;
            return serviciomail.QuitaUltimoSeparador(mails);
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
        /// Informa por correo todos los mails al vendedor y actualiza en BD al estado informado.
        /// </summary>
        /// <param name="CuentaVendedor"></param>
        public abstract IList<EmailInformado> InformarMails(long CuentaVendedor);
    }
}