using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Error.Exceptions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Cupo
{
    public class EstadoDistribucionAnuladaPendienteInformar : EstadoCupo
    {
        private Cupos CupoPadre { get; set; }

        public EstadoDistribucionAnuladaPendienteInformar(Cupos Cupo)
            : base(Cupo)
        {
            base.Nombre = "Distribución Anulada Pendiente de Informar";
            base.Codigo = CodigoEstado.DistribucionAnuladaPendienteInformar;
        }

        public override void Distribuir(long vendedor, Consignacion consignacion, string observacion, string contactoComercial, long destino, string centro, DateTime fecha, long uvdist, ISession session)
        {
            base.Cupo.Motbaja = "";
            base.Cupo.SetConsignacion(consignacion);
            base.Cupo.Status = 4;
            base.Cupo.Centrodist = centro;
            base.Cupo.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();

            //Si es distribuido de nuevo con otros datos de consignacion se debe informar de nuevo, sino no es necesario.
            if (!base.Cupo.GetConsignacion().Equals(consignacion))
            {
                base.Cupo.Pdf = 1;
                base.Cupo.Estado = new EstadoDistribuidoPendienteInformar(base.Cupo);
            }
            else
            {
                base.Cupo.Pdf = 0;
                base.Cupo.Estado = new EstadoDistribuidoInformado(base.Cupo);
            }
            base.Cupo.Uvcupodist = uvdist;
            base.CuposStore.Update(base.Cupo, session);
            if (EsCuentaYOrden()) DistribuirCyOPadre(session, uvdist);
        }

        public override void Informar(Cupos Encabezado, ISession session)
        {
            //Setear vendedor en 0 si es CYO y del Encabezado si no
            base.Cupo.Pdf = 0;
            base.Cupo.Centrodist = Encabezado.Centrodist;
            base.Cupo.SetConsignacion(Encabezado.GetConsignacion());
            base.Cupo.Observa = Encabezado.Observa;
            base.Cupo.FechaYHoraInformado = DateTime.Now;
            base.Cupo.Estado = new EstadoDistribucionAnuladaInformada(base.Cupo);
            EstadoCupo EstadoCupoPadre = null;
            if (EsCuentaYOrden())
            {
                EstadoCupoPadre = GetCupoPadreCyO(session).GetEstado();
                base.Cupo.Vendcta = 0;
                EstadoCupoPadre.Informar(Encabezado, session);
            }
            else
            {
                base.Cupo.Vendcta = Encabezado.Vendcta;
            }
            if (EstadoCupoPadre != null && EstadoCupoPadre.GetType() == typeof(ResourceServer.Models.Cupo.CYO.EstadoPendienteDistribuirConHijoPendienteInformar))
            {
                base.CuposStore.Delete(base.Cupo, session);
            }
            else
            {
                base.CuposStore.Update(base.Cupo, session);
            }
        }

        public override bool PuedeDistribuir()
        {
            if (base.Cupo.PuedeOperar() && base.Cupo.Vendcta != 0)
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
            if (base.Cupo.PuedeOperar())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool PuedeAnular()
        {
            return false;
        }

        public void ActualizarEstado()
        {
        }

        public override void AnularDistribucion(string Motivo, Cupos Encabezado, ISession session)
        {
        }

        public override InformacionModificacionEstado AnularCupo(string Motivo, Cupos Encabezado, ISession session)
        {
            throw new NotImplementedException();
        }

    }
}