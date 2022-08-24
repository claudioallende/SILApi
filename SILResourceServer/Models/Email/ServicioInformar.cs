using NHibernate;
using ResourceServer.Models.Configuracion;
using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Email
{
    /// <summary>
    /// Se encarga de enviar un email a las cuentas asociadas al vendedor con códigos alfanuméricos.
    /// </summary>
    public abstract class ServicioInformar
    {
        protected ISession Session { get; set; }

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
        /// Crea el objeto que fabrica el Email.
        /// </summary>
        /// <returns></returns>
        //protected abstract ServiceEmail GetServiceEmail();

        /// <summary>
        /// Lista de Cupos que se informan.
        /// </summary>
        /// <returns></returns>
        protected abstract IList<ICupo> GetCuposInformar(long CuentaVendedor = 0);

        /// <summary>
        /// Informa todos los mails que estén pendientes de informar.
        /// </summary>
        public abstract IList<EmailInformado> InformarMails();

        /// <summary>
        /// Informa los mails al vendedor.
        /// </summary>
        public abstract IList<EmailInformado> InformarMails(long CuentaVendedor);

        /// <summary>
        /// Agrupa los cupos por vendedor e informa el conjunto una vez.
        /// </summary>
        //public void InformarMails()
        //{
        //    IList<Cupos> CuposInformar = GetCuposInformar();
        //    IList<long> Vendedores = CuposInformar.GroupBy(x => x.Vendcta).Select(x => x.Key).ToList();
        //    Session = HibernateUtil.OpenSession();
        //    foreach (long Vendedor in Vendedores)
        //    {
        //        GetServiceEmail().Send(GetEmails(Vendedor, Session));
        //    }
        //    HibernateUtil.Dispose();
        //}
    }
}