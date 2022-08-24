using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Error.Exceptions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Cupo
{
    public class EstadoCreado : EstadoCupo
    {
        private Cupos CupoPadre { get; set; }

        public EstadoCreado(Cupos Cupo)
            : base(Cupo)
        {
            base.Nombre = "Pendiente de Distribuir";
            base.Codigo = CodigoEstado.Creado;
        }

        public override void Distribuir(long vendedor, Consignacion consignacion, string observacion, long destino, string centro, DateTime fecha, long Uvdist, ISession session)
        {
            //var condicion = false;
            if (PuedeDistribuir())
            {
                base.Cupo.Vendcta = vendedor;
                base.Cupo.SetConsignacion(consignacion);
                base.Cupo.Observa = observacion;
                base.Cupo.Status = 4;
                base.Cupo.Pdf = 1;
                base.Cupo.Centrodist = centro;
                base.Cupo.Uvcupodist = Uvdist;
                base.Cupo.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();

                base.CuposStore.Update(base.Cupo, session);
                base.Cupo.Estado = new EstadoDistribuidoPendienteInformar(base.Cupo);

                if (EsCuentaYOrden()) DistribuirCyOPadre(session, Uvdist);
            }
        }

        public override void Informar(Cupos Encabezado, ISession session)
        {
            base.Mensaje = "Se encuentra en estado Creado, no puede informarse";
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
            //if (base.Cupo.EsCuentaYOrden()) base.CuposStore.Delete(base.Cupo, session);
        }

        public override InformacionModificacionEstado AnularCupo(string Motivo, Cupos Encabezado, ISession session)
        {
            if (PuedeAnular())
            {
                base.Cupo.Motbaja = Motivo;
                base.Cupo.Status = 3;
                base.Cupo.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();
                base.CuposStore.Anular(base.Cupo, session);
                InformacionModificacionEstado result = GetInformacion();
                base.Cupo.Estado = new EstadoAnulado(base.Cupo);
                //if (base.Cupo.EsCuentaYOrden()) 
                //    AnularCupoPadre(Motivo, Encabezado, session);
                return result;
            }
            throw new Exception("No se puede anular.");
        }

    }
}