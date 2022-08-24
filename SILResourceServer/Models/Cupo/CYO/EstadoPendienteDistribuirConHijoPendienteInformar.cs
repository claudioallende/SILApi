using ResourceServer.Models.Error.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Cupo.CYO
{
    public class EstadoPendienteDistribuirConHijoPendienteInformar : EstadoCupo
    {
        public EstadoPendienteDistribuirConHijoPendienteInformar(Cupos Cupo)
            : base(Cupo)
        {
            base.Nombre = "Pendiente de Distribuir Cuenta y orden Con Hijo Pendiente de Informar";
            base.Codigo = CodigoEstado.CuentaYOredenPendienteDistribuirHijoPendienteInformar;
        }

        public override void Distribuir(long vendedor, Consignacion consignacion, string observacion, long destino, string centro, DateTime fecha, long Uvdist, NHibernate.ISession session)
        {
            throw new CupoBloqueadoParaDistribuirException();
        }

        public override bool PuedeDistribuir()
        {
            return false;
        }

        public override void Informar(Cupos Encabezado, NHibernate.ISession session)
        {
            base.Cupo.Pdf = 0;
            base.Cupo.Vendcyo = Encabezado.Vendcyo;
            base.Cupo.Vendcta = Encabezado.Vendcta;
            base.Cupo.Uvcupodist = 0;
            base.CuposStore.Update(base.Cupo, session);
        }

        public override bool PuedeInformar()
        {
            return true;
        }

        public override void AnularDistribucion(string Motivo, Cupos Encabezado, NHibernate.ISession session)
        {
        }

        public override InformacionModificacionEstado AnularCupo(string Motivo, Cupos Encabezado, NHibernate.ISession session)
        {
            base.Cupo.Status = 3;
            base.Cupo.Motbaja = Motivo;
            base.Cupo.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();
            base.CuposStore.Update(base.Cupo, session);
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

        public override bool PuedeAnular()
        {
            throw new NotImplementedException();
        }
    }
}