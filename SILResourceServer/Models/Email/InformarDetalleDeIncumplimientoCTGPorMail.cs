using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Email
{
    public class InformarDetalleDeIncumplimientoCTGPorMail : ServicioInformarCuposPendienteDeCTG
    {
        IList<ICupo> listaDetalleDeIncumplimientoCupo = new List<ICupo>();
        private DateTime periodoDesde;
        private DateTime periodoHasta;

        public InformarDetalleDeIncumplimientoCTGPorMail(ServiceEmailEstadoCupo serviceEmail, ISession session, long vendcuenta, List<ICupo> listaDetalle, DateTime xperiodoDesde, DateTime xperiodoHasta) 
        {
            base.EmailService = serviceEmail;
            base.mySession = session;
            base.vendcta = vendcuenta;
            this.listaDetalleDeIncumplimientoCupo = listaDetalle;  
            this.periodoDesde = xperiodoDesde;
            this.periodoHasta = xperiodoHasta;
        }

        /// <summary>
        /// este metodo es el que llamamos para disparar todo el proceso. luego es este quien llama a ObtenerServiceEInformar
        /// </summary>
        /// <param name="CuentaVendedor"></param>
        /// <returns></returns>
        public override IList<EmailInformado> InformarMails()
        {   
            IList<EmailInformado> EmailsInformados = new List<EmailInformado>();
            ServicioProceso servicioProceso = new ServicioProceso();

            if (this.listaDetalleDeIncumplimientoCupo.Count > 0)
            {
                EmailsInformados = ObtenerServiceEInformar(this.GeneraMailIncumplimientoVendedor(listaDetalleDeIncumplimientoCupo), false);
            }
                        /*Si son todos igual a 0 no hubo error.*/
            if (!EmailsInformados.Any(x => x.Estado != 0)) 
                servicioProceso.GuardarProceso(servicioProceso.BuildEstadoProcesoTerminadoOk(1339560));
            return EmailsInformados;
        }

        public override IList<EmailInformado> InformarMails(long CuentaVendedor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Llama al EnviarEmail y este al SendMessage del serviceEmail. Este implementado en la clase padre.
        /// </summary>
        /// <param name="MailPdf"></param>
        /// <param name="ServicioEmail"></param>
        /// <param name="CorreosElectronicosDestinatarios"></param>
        /// <param name="Session"></param>
        /// <param name="enviarAdjunto"></param>
        public override void InformarYGuardarCupos(IMailInformation MailPdf, ServiceEmailEstadoCupo ServicioEmail, string CorreosElectronicosDestinatarios, NHibernate.ISession Session, bool enviarAdjunto)
        {   
            EnviarEmail(ServicioEmail, CorreosElectronicosDestinatarios, MailPdf, enviarAdjunto);
        }

        /// <summary>
        /// iteramos sobre todos los registros de DetalleDeIncumplimientoCupo y generamos un list de MailIncumplimientoVendedor
        /// por vendedor donde tiene un listado de DetalleDeIncumplimientoCupo agrupado por grano
        /// </summary>
        /// <param name="DetalleDeIncumplimientoCupo"></param>
        /// <returns></returns>
        private IList<IMailInformation> GeneraMailIncumplimientoVendedor(IList<ICupo> DetalleDeIncumplimientoCupo)
        {
            IList<IMailInformation> MailsAEnviar = new List<IMailInformation>();

            var vendedoresIncumplidores = DetalleDeIncumplimientoCupo
                .GroupBy(x =>
                    new
                    {
                        vendcta = x.Vendcta
                    }
                )
                .Select( y => new
                      {
                        vendcta = y.Key.vendcta
                      }   
                )
               .ToList();

            foreach (var c in vendedoresIncumplidores)
            {
                MailIncumplimientoVendedor IncumplimientoPorVendedor = new MailIncumplimientoVendedor(c.vendcta);
                //IList<DetalleDeIncumplimientoCupo> ListaDeIncumpliXVendedor = (IList<DetalleDeIncumplimientoCupo>) DetalleDeIncumplimientoCupo
                var ListaDeIncumpliXVendedor = DetalleDeIncumplimientoCupo
                    .Where(x => 
                            x.Vendcta == long.Parse(c.vendcta.ToString()) 
                    )
                    .OrderBy(
                        y => y.Grano
                    )
                    .ToList();

                var granosParaElVendedor = ListaDeIncumpliXVendedor
                .GroupBy(x =>
                    new
                    {
                        grano = x.Grano
                    }
                )
                .Select( y => new
                      {
                        grano = y.Key.grano
                      }   
                )
               .ToList();
                foreach (var d in granosParaElVendedor)
                {
                    MailIncumplimientoGrano IncumplimientoPorGrano = new MailIncumplimientoGrano (d.grano);

                    //IList<DetalleDeIncumplimientoCupo> ListaDeIncumpliXgrano = (IList<DetalleDeIncumplimientoCupo>) DetalleDeIncumplimientoCupo
                    var ListaDeIncumpliXgrano = DetalleDeIncumplimientoCupo
                        .Where(x =>
                                x.Vendcta == long.Parse(c.vendcta.ToString()) &&
                                x.Grano == (int)d.grano
                        )
                        .OrderBy(
                            y => y.Fecha
                        )
                        .ToList();
                    IncumplimientoPorGrano.ListDetalleIncumplimiento = ListaDeIncumpliXgrano;
                    IncumplimientoPorVendedor.ListIncumplimientoPorGrano.Add(IncumplimientoPorGrano);
                }

                MailsAEnviar.Add(IncumplimientoPorVendedor);
            }
            return MailsAEnviar;
        }
        
        /// <summary>
        /// Iteramos sobre la lista de MailIncumplimientoVendedo, obtenemos los correos electronicos de cada cuenta 
        /// y disparo el envio de mail por cada cuenta.
        /// </summary>
        /// <param name="Mails"></param>
        /// <param name="enviarAdjunto"></param>
        /// <returns></returns>
        public override IList<EmailInformado> ObtenerServiceEInformar(IList<IMailInformation> Mails, bool enviarAdjunto)
        {  
            IList<EmailInformado> EmailsInformados = new List<EmailInformado>();
            ServicioProceso servicioProceso = new ServicioProceso();
            long CuentaVendedor = 0;
            try
            {
                EmailDetalleDeIncumplimientoCTG ServicioEmail = (EmailDetalleDeIncumplimientoCTG)GetServiceEmail();
                foreach (IMailInformation InfoMailPorVend in Mails) 
                {
                    CuentaVendedor = InfoMailPorVend.Vendcta;
                    /************************************a modo de prueba. para que no dispare el envio de mail********************************/
                    string CorreosElectronicosDestinatarios = GetEmails(CuentaVendedor, this.mySession);
                    if (CorreosElectronicosDestinatarios.Length > 0) 
                    {
                        ServicioEmail.SetInfoCuerpo(InfoMailPorVend, this.periodoDesde, this.periodoHasta);
                        try
                        {
                            InformarYGuardarCupos(InfoMailPorVend, ServicioEmail, CorreosElectronicosDestinatarios, this.mySession, enviarAdjunto);
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
        /// este es usado para tener un detalle del tipo de informe estamos haciendo. 
        /// actualmente lo utilizamos cuando escribimos errores en el Log.
        /// </summary>
        protected override string TipoServicioInformar
        {
            get
            {
                return "Detalle De Incumplimiento CTG";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        protected override IList<ICupo> GetCuposInformar(long CuentaVendedor)
        {
            throw new NotImplementedException();
        }
    }
}