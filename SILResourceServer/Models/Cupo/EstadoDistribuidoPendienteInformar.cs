using ResourceServer.Models.DataAccess;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Cupo
{
    public class EstadoDistribuidoPendienteInformar : EstadoCupo
    {
        public Cupos CupoPadre { get; set; }

        public EstadoDistribuidoPendienteInformar(Cupos Cupo)
            : base(Cupo)
        {
            base.Nombre = "Distribuido Pendiente de Informar";
            base.Codigo = CodigoEstado.DistribuidoPendienteInformar;
        }

        public override void Distribuir(long vendedor, Consignacion consignacion, string observacion, string contactoComercial, long destino, string centro, DateTime fecha, long Uvdist, ISession session)
        {
            throw new NotImplementedException();
        }

        public override void Informar(Cupos Encabezado, ISession session)
        {
            if (PuedeInformar())
            {
                base.Cupo.Pdf = 0;
                base.Cupo.FechaYHoraInformado = DateTime.Now;
                base.CuposStore.Update(base.Cupo, session);
                base.Cupo.Estado = new EstadoDistribuidoInformado(base.Cupo);
            }
        }

        public override bool PuedeDistribuir()
        {
            return false;
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
            if (PuedeAnular())
            {
                base.Cupo.Motbaja = Motivo;
                base.Cupo.Status = 0;
                //base.Cupo.SetConsignacion(Encabezado.GetConsignacion());
                base.Cupo.Observa = Encabezado.Observa;
                base.Cupo.Pdf = 0;
                base.Cupo.Centrodist = Encabezado.Centrodist;
                base.Cupo.Uvcupodist = 0;
                base.Cupo.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();
                base.Cupo.Estado = new EstadoCreado(base.Cupo);
                if (base.Cupo.EsCuentaYOrden())
                {
                    base.Cupo.Vendcta = 0;
                    //((CuposCorretaje.Models.Cupo.CYO.EstadoDistribuidoConHijoDistribuido)GetCupoPadreCyO(session).GetEstado()).LiberarCupo(session);
                }
                else
                    base.Cupo.Vendcta = Encabezado.Vendcta;

                base.CuposStore.Update(base.Cupo, session);
            }
        }

        public override InformacionModificacionEstado AnularCupo(string Motivo, Cupos Encabezado, ISession session)
        {
            if (PuedeAnular())
            {
                base.Cupo.Status = 3;
                base.Cupo.Pdf = 0;
                base.Cupo.Motbaja = Motivo;
                base.Cupo.Uvcupodist = 0;
                base.Cupo.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();
                //if (base.Cupo.EsCuentaYOrden()) LiberarCyOPadrePorAnulacionAlfaHijo(Motivo, Encabezado, session);
                InformacionModificacionEstado result = GetInformacion();
                base.Cupo.Estado = new EstadoAnulado(base.Cupo);
                if (base.Cupo.EsCuentaYOrden())
                {
                    base.Cupo.Vendcta = 0;
                    //AnularCupoPadre(Motivo, Encabezado, session);
                }
                else
                    base.Cupo.Vendcta = Encabezado.Vendcta;

                base.CuposStore.Update(base.Cupo, session);
                return result;
            }
            throw new Exception("No se puede anular.");
        }
    }
}