using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Error.Exceptions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Cupo.CYO
{
    public class EstadoCreado : EstadoCupo
    {
        public EstadoCreado(Cupos Cupo) : base(Cupo) {
            base.Nombre = "Pendiente de Distribuir Cuenta y orden";
            base.Codigo = CodigoEstado.CuentaYOrdenCreado;
        }

        public override void Distribuir(long vendedor, Consignacion consignacion, string observacion, string contactoComercial, long destino, string centro, DateTime fecha, long uvdist, ISession session)
        {
            if (PuedeDistribuir())
            {
                base.Cupo.Vendcta = vendedor;
                base.Cupo.SetConsignacion(consignacion);
                base.Cupo.Observa = observacion;
                base.Cupo.ContactoComercial = contactoComercial;
                base.Cupo.Status = 4;
                base.Cupo.Centrodist = centro;
                base.Cupo.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();

                base.Cupo.Uvcupodist = uvdist;
                base.CuposStore.Update(base.Cupo, session);
                base.CuposStore.Save(NuevoCupoHijo(session), session);

                base.Cupo.Estado = new EstadoDistribuidoPendienteDistribuirHijo(base.Cupo);
            }
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
            throw new NotImplementedException();
        }

        public override bool PuedeInformar()
        {
            return false;
        }

        public override void AnularDistribucion(string Motivo, Cupos Encabezado, ISession session)
        {
        }

        public override InformacionModificacionEstado AnularCupo(string Motivo, Cupos Encabezado, ISession session)
        {
            if (PuedeAnular())
            {
                base.Cupo.Motbaja = Motivo;
                base.Cupo.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();
                base.CuposStore.Anular(base.Cupo, session);
                InformacionModificacionEstado result = GetInformacion();
                base.Cupo.Estado = new EstadoAnulado(base.Cupo);
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

    }
}