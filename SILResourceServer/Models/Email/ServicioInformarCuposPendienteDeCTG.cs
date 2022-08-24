using NHibernate;
using ResourceServer.Models.Configuracion;
using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Email
{
    public abstract class ServicioInformarCuposPendienteDeCTG : ServicioInformar
    {
        private string OK = "OK";
        private string EMAILS_NO_CONFIGURADOS = "Mails no configurados";
        protected long CuentaComprador { get; set; }
        protected abstract string TipoServicioInformar { get; set; }
        /*new*/
        protected ServiceEmailEstadoCupo EmailService;
        protected long compcta{ get; set; }
        protected long vendcta{ get; set; }
        protected long puertocta { get; set; }
        protected int grano{ get; set; }
        protected string codcentro{ get; set; }
        protected string codcentrodistribucion{ get; set; }
        protected bool cyo { get; set; }

        /// <summary>
        /// Este objeto session debe contener el mapeo a la tabla cuposcorre
        /// </summary>
        protected ISession mySession{ get; set; }

        /// <summary>
        /// Busca los encabezados de los cupos, llama al informar del estado del cupo en caso que corresponda (cambia el estado del cupos) 
        /// y llama al EnviarEmail
        /// </summary>
        /// <param name="MailPdf"></param>
        /// <param name="ServicioEmail"></param>
        /// <param name="CorreosElectronicosDestinatarios"></param>
        /// <param name="Session"></param>
        /// <param name="enviarAdjunto"></param>
        public abstract void InformarYGuardarCupos(IMailInformation MailPdf, ServiceEmailEstadoCupo ServicioEmail, string CorreosElectronicosDestinatarios, ISession Session, bool enviarAdjunto);
        
        /// <summary>
        /// setea los parametros mas relevantes de la clase para consultar cupos al momento de la distribucion o anulacion
        /// </summary>
        /// <param name="compcta"></param>
        /// <param name="vendcta"></param>
        /// <param name="puertocta"></param>
        /// <param name="grano"></param>
        /// <param name="codcentro"></param>
        /// <param name="codcentrodistribucion"></param>
        /// <param name="cyo"></param>
        public void SetCompVendPuertoGranoCentroCentrodistCyo(long compcta, long vendcta, long puertocta, int grano, string codcentro, string codcentrodistribucion, bool cyo)
        { 
            this.compcta= compcta;
            this.vendcta=vendcta;
            this.puertocta=puertocta;
            this.grano=grano;
            this.codcentro=codcentro;
            this.codcentrodistribucion=codcentrodistribucion;
            this.cyo = cyo;
        }

        /// <summary>
        /// obtiene los correos electronicos de la cuenta vendedora. instancia el ServiceEmail y lleva a cabo el envio.
        /// En caso de error en el envio de alguno mails, se genera un warning en el log de errores detallando la cuenta vendedora
        /// el tipo de warning
        /// </summary>
        /// <param name="CuentaVendedor"></param>
        /// <param name="PdfsAEnviarPorMails"></param>
        /// <returns>Lista de correos electronicos </returns>
        public abstract IList<EmailInformado> ObtenerServiceEInformar(IList<IMailInformation> Mails, bool enviarAdjunto);
              

        /// <summary>
        /// cheque si en el envio de email hubo algun error y si asi fue dispara la excepcion
        /// para escribirlo en el log
        /// </summary>
        /// <param name="informes"></param>
        /// <returns></returns>
        public string CheckErroresEnEnvio(IList<EmailInformado> informes)
        {
            string concatenoErrores ="";
            List<EmailInformado> listaDeErrores = informes
                .Where(mo => mo.Estado != 0)
                .ToList();
            foreach (EmailInformado e in listaDeErrores)
            {
                concatenoErrores += string.Format(" Cuenta {0} - {1} - {2}", e.CuentaVendedor, e.TipoEmail, e.Mensaje);
            }
            return concatenoErrores;
        }

        /// <summary>
        /// lleva adelante el envio de mail con o sin adjunto
        /// </summary>
        /// <param name="ServicioEmail"></param>
        /// <param name="CorreosElectronicosDestinatarios"></param>
        /// <param name="PdfAEnviar"></param>
        /// <param name="enviarAdjunto"></param>
        public void EnviarEmail(ServiceEmailEstadoCupo ServicioEmail, string CorreosElectronicosDestinatarios, IMailInformation PdfAEnviar, bool enviarAdjunto)
        {
            if (enviarAdjunto)
            {     
                ServicioEmail.SendMailWithAttachments(CorreosElectronicosDestinatarios, PdfAEnviar.Pdf);
            }       
            else {
                ServicioEmail.SendMessage(CorreosElectronicosDestinatarios);
            }   
        }

        /// <summary>
        /// retorna el serviceEmail. donde se lleva a cabo el armado y envio del email
        /// </summary>
        /// <returns>serviceEmail</returns>
        protected ServiceEmailEstadoCupo GetServiceEmail()
        {
            return this.EmailService;
        }

        protected override abstract IList<ICupo> GetCuposInformar(long CuentaVendedor);

        public override abstract IList<EmailInformado> InformarMails();

        public override abstract IList<EmailInformado> InformarMails(long CuentaVendedor);
    }
}