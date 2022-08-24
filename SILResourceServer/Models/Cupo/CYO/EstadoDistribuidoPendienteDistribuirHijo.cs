using ResourceServer.Models.DataAccess;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Cupo.CYO
{
    public class EstadoDistribuidoPendienteDistribuirHijo : EstadoCupo
    {
        public EstadoDistribuidoPendienteDistribuirHijo(Cupos Cupo)
            : base(Cupo)
        {
            base.Nombre = "Distribuido Cuenta y orden";
            base.Codigo = CodigoEstado.CuentaYOrdenDistribuidoPendienteDistribuirHijo;
        }

        public override void Distribuir(long vendedor, Consignacion consignacion, string observacion, long destino, string centro, DateTime fecha, long uvdist, ISession session)
        {
            base.Cupo.Status = 5;
            base.Cupo.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();
            base.CuposStore.Update(base.Cupo, session);
            base.Cupo.Estado = new EstadoDistribuidoConHijoDistribuido(base.Cupo);
        }

        public override bool PuedeDistribuir()
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
                base.Cupo.Motbaja = Motivo;
                base.Cupo.Status = 0;
                base.Cupo.SetConsignacion(Encabezado.GetConsignacion());
                base.Cupo.Observa = Encabezado.Observa;
                base.Cupo.Centrodist = Encabezado.Centrodist;
                base.Cupo.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();
                base.Cupo.Uvcupodist = 0;

                if (Encabezado.Vendcyo == 0)
                {
                    base.Cupo.Vendcyo = 0;
                    base.Cupo.Vendcta = Encabezado.Vendcta;
                    base.Cupo.Estado = new ResourceServer.Models.Cupo.EstadoCreado(base.Cupo);
                }
                else
                {
                    base.Cupo.Estado = new EstadoCreado(base.Cupo);
                }
                base.CuposStore.Delete(GetCupoHijoCYO(session), session);

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

        public void DevolverCupoModificado()
        {
            base.Cupo.Vendcyo = 0;
        }

    }
}