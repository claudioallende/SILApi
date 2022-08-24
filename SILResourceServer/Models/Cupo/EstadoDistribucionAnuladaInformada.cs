using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Error.Exceptions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Cupo
{
    public class EstadoDistribucionAnuladaInformada : EstadoCupo
    {
        private Cupos CupoPadre { get; set; }

        public EstadoDistribucionAnuladaInformada(Cupos Cupo)
            : base(Cupo)
        {
            base.Nombre = "Pendiente de Distribuir";
            base.Codigo = CodigoEstado.DistribucionAnuladaInformada;
        }

        public override void Distribuir(long vendedor, Consignacion consignacion, string observacion, long destino, string centro, DateTime fecha, long uvdist, ISession session)
        {
            if (PuedeDistribuir())
            {
                base.Cupo.Vendcta = vendedor;
                base.Cupo.SetConsignacion(consignacion);
                base.Cupo.Observa = observacion;
                base.Cupo.Status = 4;
                base.Cupo.Pdf = 1;
                base.Cupo.Centrodist = centro;
                base.Cupo.Uvcupodist = uvdist;
                base.Cupo.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();

                base.CuposStore.Update(base.Cupo, session);
                base.Cupo.Estado = new EstadoDistribuidoPendienteInformar(base.Cupo);

                if (EsCuentaYOrden()) DistribuirCyOPadre(session, uvdist);
            }
        }

        public override void Informar(Cupos Encabezado, ISession session)
        {
            throw new NotImplementedException();
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

        public override bool PuedeInformar()
        {
            return false;
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

        public override void AnularDistribucion(string Motivo, Cupos Encabezado, ISession session)
        {
        }

        public override InformacionModificacionEstado AnularCupo(string Motivo, Cupos Encabezado, ISession session)
        {
            base.Cupo.Motbaja = Motivo;
            base.Cupo.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();
            base.CuposStore.Anular(base.Cupo, session);
            InformacionModificacionEstado result = GetInformacion();
            base.Cupo.Estado = new EstadoAnulado(base.Cupo);
            //if (base.Cupo.EsCuentaYOrden())
            //    AnularCupoPadre(Motivo, Encabezado, session);
            return result;
        }

    }
}