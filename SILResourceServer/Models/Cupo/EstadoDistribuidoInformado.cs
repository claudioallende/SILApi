using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Cupo
{
    public class EstadoDistribuidoInformado : EstadoCupo
    {
        public EstadoDistribuidoInformado(Cupos Cupo)
            : base(Cupo)
        {
            base.Nombre = "Distribuido Informado";
            base.Codigo = CodigoEstado.DistribuidoInformado;
        }

        public override void Distribuir(long vendedor, Consignacion consignacion, string observacion, string contactoComercial, long destino, string centro, DateTime fecha, long Uvdist, NHibernate.ISession session)
        {
            throw new NotImplementedException();
        }

        public override void Informar(Cupos Encabezado, NHibernate.ISession session)
        {
            throw new NotImplementedException();
        }

        public override bool PuedeDistribuir()
        {
            return false;
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

        public override void AnularDistribucion(string Motivo, Cupos Encabezado, NHibernate.ISession session)
        {
            if (PuedeAnular())
            {
                base.Cupo.Motbaja = Motivo;
                base.Cupo.Status = 0;
                base.Cupo.Pdf = 1;
                base.Cupo.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();

                //if (base.Cupo.EsCuentaYOrden())
                //    GetCupoPadreCyO(session).GetEstado().AnularDistribucion(Motivo, Encabezado, session);

                base.CuposStore.Update(base.Cupo, session);
                base.Cupo.Estado = new EstadoDistribucionAnuladaPendienteInformar(base.Cupo);
            }
        }

        public override InformacionModificacionEstado AnularCupo(string Motivo, Cupos Encabezado, NHibernate.ISession session)
        {
            if (PuedeAnular())
            {
                base.Cupo.Status = 3;
                base.Cupo.Pdf = 1;
                base.Cupo.Motbaja = Motivo;
                //base.Cupo.Uvcupodist = 0;
                base.Cupo.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();

                InformacionModificacionEstado result = GetInformacion();
                base.Cupo.Estado = new EstadoAnuladoPendienteInformar(base.Cupo);
                //if (base.Cupo.EsCuentaYOrden()) 
                //    AnularCupoPadre(Motivo, Encabezado, session);

                base.CuposStore.Update(base.Cupo, session);

                return result;
            }
            throw new Exception("No se puede anular.");
        }
    }
}