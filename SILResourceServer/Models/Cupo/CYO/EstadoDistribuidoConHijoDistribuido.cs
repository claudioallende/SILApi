using ResourceServer.Models.DataAccess;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Cupo.CYO
{
    public class EstadoDistribuidoConHijoDistribuido : EstadoCupo
    {
        public EstadoDistribuidoConHijoDistribuido(Cupos Cupo)
            : base(Cupo)
        {
            base.Nombre = "Distribuido Cuenta y orden con Hijo Distribuido";
            base.Codigo = CodigoEstado.CuentaYOrdenDistribuidoConHijoDistribuido;
        }

        public override void Distribuir(long vendedor, Consignacion consignacion, string observacion, string contactoComercial, long destino, string centro, DateTime fecha, long uvdist, ISession session)
        {
            throw new NotImplementedException();
        }

        public override bool PuedeDistribuir()
        {
            throw new NotImplementedException();
        }

        public override void Informar(Cupos Encabezado, ISession session)
        {
        }

        public override bool PuedeInformar()
        {
            throw new NotImplementedException();
        }

        public override void AnularDistribucion(string Motivo, Cupos Encabezado, ISession session)
        {
            if (PuedeAnular())
            {
                Cupos CupoHijo = GetCupoHijoCYO(session);
                base.Cupo.Status = 0;
                base.Cupo.Motbaja = Motivo;
                base.Cupo.SetConsignacion(Encabezado.GetConsignacionTrim());
                base.Cupo.Centrodist = Encabezado.Centrodist;
                base.Cupo.Observa = Encabezado.Observa;
                base.Cupo.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();

                if (base.Cupo.Vendcyo == 0 && CupoHijo.GetEstado().Codigo != CodigoEstado.DistribuidoInformado)
                {
                    base.Cupo.Vendcyo = Encabezado.Vendcyo;
                    base.Cupo.Vendcta = Encabezado.Vendcta;
                    base.Cupo.Estado = new ResourceServer.Models.Cupo.EstadoCreado(base.Cupo);
                    base.CuposStore.Delete(GetCupoHijoCYO(session), session);
                }
                else
                {
                    if (CupoHijo.GetEstado().Codigo == CodigoEstado.DistribuidoInformado)
                    {
                        base.Cupo.Estado = new EstadoPendienteDistribuirConHijoPendienteInformar(base.Cupo);
                        base.GetCupoHijoCYO(session).GetEstado().AnularDistribucion(Motivo, Encabezado, session);
                    }
                    else
                    {
                        base.Cupo.Vendcyo = Encabezado.Vendcyo;
                        base.Cupo.Vendcta = Encabezado.Vendcta;
                        base.Cupo.Uvcupodist = 0;
                        base.CuposStore.Delete(GetCupoHijoCYO(session), session);
                        base.Cupo.Estado = new EstadoCreado(base.Cupo);
                    }
                }


                base.CuposStore.Update(base.Cupo, session);
            }
        }

        public override InformacionModificacionEstado AnularCupo(string Motivo, Cupos Encabezado, ISession session)
        {
            if (PuedeAnular())
            {
                base.Cupo.Status = 3;
                base.Cupo.Motbaja = Motivo;
                //base.Cupo.Uvcupodist = 0;
                base.Cupo.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();
                base.CuposStore.Update(base.Cupo, session);
                //base.CuposStore.Delete(GetCupoHijoCYO(session), session);
                InformacionModificacionEstado result = GetInformacion();
                if (Encabezado.Vendcyo == 0)
                {
                    base.Cupo.Vendcyo = 0;
                    base.Cupo.Vendcta = Encabezado.Vendcta;
                    base.Cupo.Uvcupodist = 0;
                    base.Cupo.Estado = new ResourceServer.Models.Cupo.EstadoAnulado(base.Cupo);
                }
                else
                {
                    base.Cupo.Estado = new EstadoAnulado(base.Cupo);
                }
                AnularCupoHijo(Motivo, Encabezado, session);
                return result;
            }
            throw new Exception("No se puede anular.");
        }

        public override bool PuedeAnular()
        {
            if (base.Cupo.PuedeOperar())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void LiberarCupo(string Motivo, Cupos Encabezado, ISession Session)
        {
            base.Cupo.Status = 4;
            base.Cupo.Estado = new EstadoDistribuidoPendienteDistribuirHijo(base.Cupo);

            base.GetCupoHijoCYO(Session).GetEstado().AnularDistribucion(Motivo, Encabezado, Session);

            base.CuposStore.Update(base.Cupo, Session);
        }
    }
}