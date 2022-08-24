using NHibernate;
using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Email
{
    public class InformarPendienteDeCTGPorMail: ServicioInformarCuposPendienteDeCTG
    {
        private List<ICupo> cuposAinformar; 

        public InformarPendienteDeCTGPorMail(ServiceEmailEstadoCupo serviceEmail, ISession session, long vendcuenta, IList<ICupo> cuposAinformar) 
        {
            base.EmailService = serviceEmail;
            base.mySession = session;
            base.vendcta = vendcuenta;
            this.cuposAinformar = new List<ICupo>();
            this.cuposAinformar.AddRange(cuposAinformar);
        }

        /// <summary>
        /// obtiene los cupos a informar y envia mail
        /// si necesita comp, vend, grano, etc las toma de la clase
        /// </summary>
        /// <returns>Lista de correos electronicos </returns>
        public override IList<EmailInformado> InformarMails()
        {
            IList<EmailInformado> EmailsInformados = new List<EmailInformado>();
            CuentaComprador = compcta;
            List<Cupos> cupos =GetCuposInformar().Cast<Cupos>().ToList(); 
            ServicioProceso servicioProceso = new ServicioProceso();
            if (cupos.Count > 0)
            {
                IList<IMailInformation> PdfsAEnviarPorMails = this.GeneraMailPdf(cupos);
                EmailsInformados = ObtenerServiceEInformar(PdfsAEnviarPorMails, false);
            }
            if (!EmailsInformados.Any(x => x.Estado != 0)) //Si son todos igual a 0 no hubo error.
                servicioProceso.GuardarProceso(servicioProceso.BuildEstadoProcesoTerminadoOk(1339560));
            return EmailsInformados;
        }
        /// <summary>
        /// llama al EnviarEmail
        /// </summary>
        /// <param name="MailPdf"></param>
        /// <param name="ServicioEmail"></param>
        /// <param name="CorreosElectronicosDestinatarios"></param>
        /// <param name="Session"></param>
        /// <param name="enviarAdjunto"></param>
        public override void InformarYGuardarCupos(IMailInformation MailPdf, ServiceEmailEstadoCupo ServicioEmail, string CorreosElectronicosDestinatarios, ISession Session, bool enviarAdjunto)
        {
            EnviarEmail(ServicioEmail, CorreosElectronicosDestinatarios, MailPdf, enviarAdjunto);
        }
        /*--------------------------------------------------------------------------------------------------*/
        /// <summary>
        /// obtiene los correos electronicos de la cuenta vendedora. instancia el ServiceEmail y lleva a cabo el envio.
        /// En caso de error en el envio de alguno mails, se genera un warning en el log de errores detallando la cuenta vendedora
        /// el tipo de warning
        /// </summary>
        /// <param name="CuentaVendedor"></param>
        /// <param name="PdfsAEnviarPorMails"></param>
        /// <returns>Lista de correos electronicos </returns>
        public override IList<EmailInformado> ObtenerServiceEInformar(IList<IMailInformation> Mails, bool enviarAdjunto)
        {
            IList<EmailInformado> EmailsInformados = new List<EmailInformado>();
            ServicioProceso servicioProceso = new ServicioProceso();
            long CuentaVendedor = 0;
            try
            {
                EmailPendienteDeCTG ServicioEmail = (EmailPendienteDeCTG)GetServiceEmail();
                foreach (MailPdf mailAEnviar in Mails)
                {
                    CuentaVendedor = mailAEnviar.CuposAInformar[0].Vendcta;
                    string CorreosElectronicosDestinatarios =  GetEmails(CuentaVendedor, this.mySession);
                    if (CorreosElectronicosDestinatarios.Length > 0) 
                    {
                        ServicioEmail.setCupos((List<Cupos>)mailAEnviar.CuposAInformar);
                        try
                        {
                            InformarYGuardarCupos(mailAEnviar, ServicioEmail, CorreosElectronicosDestinatarios, this.mySession, enviarAdjunto);
                            EmailsInformados.Add(new EmailInformado { Estado = 0, Mensaje = "OK", CuentaVendedor = CuentaVendedor, TipoEmail = this.TipoServicioInformar });
                        }
                        catch (ResourceServer.Models.Error.Exceptions.NeedEmailException e)
                        {
                            servicioProceso.GuardarProceso(servicioProceso.BuildEstadoProcesoErrorNoBloqueante(1339560, CuentaVendedor.ToString(), e.Message));

                            EmailsInformados.Add(new EmailInformado { Estado = 1, Mensaje = "CORREO NO CONFIGURADO", CuentaVendedor = CuentaVendedor, TipoEmail = this.TipoServicioInformar });
                        }
                        catch (System.Net.Mail.SmtpException)
                        {
                            //Lanza una excepcion para que interrumpa el envio de mails, ya que hay un error en la configuracion del SMTP. 
                            //El try catch de arriba captura el error y lo guarda en BD
                            throw;
                        }
                        catch (Exception e)
                        {
                            servicioProceso.GuardarProceso(servicioProceso.BuildEstadoProcesoErrorNoBloqueante(1339560, CuentaVendedor.ToString(), e.Message));

                            EmailsInformados.Add(new EmailInformado { Estado = 2, Mensaje = "ERROR", CuentaVendedor = CuentaVendedor, TipoEmail = this.TipoServicioInformar });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                servicioProceso.GuardarProceso(servicioProceso.BuildEstadoProcesoErrorBloqueante(1339560, CuentaVendedor.ToString(), e.Message));

                throw e;
            }
            string erroresEnEnvio = CheckErroresEnEnvio(EmailsInformados);
            if (erroresEnEnvio.Length > 0)
            {
                throw new ResourceServer.Models.Error.Exceptions.ErrorEnEnvioDeEmailException(erroresEnEnvio);
            }
            return EmailsInformados;
        }

        /// <summary>
        /// genera una lista de elementos MailPdf que contienen los cupos, agrupdos por vendedor
        /// </summary>
        /// <param name="cupos"></param>
        /// <param name="generaArchivoPdf"></param>
        /// <returns>lista de elementos que contienen los cupos, consignacion y path del pdf en caso que corresponda</returns>
        public IList<IMailInformation> GeneraMailPdf(IList<Cupos> cupos)
        {
            IList<IMailInformation> PdfsAEnviarPorMails = new List<IMailInformation>();
            var cuposAgrupadosxVendedor = cupos.GroupBy(x =>
                    new
                    {
                        vendcta = x.Vendcta
                    }
                )
                .Select(z => 
                    new
                    {
                        vendcta = z.Key.vendcta
                    })
               .ToList();
            foreach (var c in cuposAgrupadosxVendedor)
            {
                IList<Cupos> cupospdf = cupos.Where(x =>
                        x.Vendcta == long.Parse(c.vendcta.ToString())
                    ).ToList();
                MailPdf mail = new MailPdf();
                mail.Pdf = "";
                mail.Consignacion = null;
                mail.CuposAInformar = cupospdf;
                PdfsAEnviarPorMails.Add(mail);
            }
            return PdfsAEnviarPorMails;
        }

        protected override string TipoServicioInformar
        {
            get
            {
                return "Informacion Sin CTG";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override IList<EmailInformado> InformarMails(long CuentaVendedor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// traemos los cupos que cumplen la condicion que debemos informar
        /// </summary>
        /// <returns></returns>
        protected override IList<ICupo> GetCuposInformar(long CuentaVendedor = 0)
        {
            return this.cuposAinformar;
        }

       
    }
}