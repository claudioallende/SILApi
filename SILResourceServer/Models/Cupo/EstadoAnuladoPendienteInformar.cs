using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Error.Exceptions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Cupo
{
    public class EstadoAnuladoPendienteInformar : EstadoCupo
    {
        public EstadoAnuladoPendienteInformar(Cupos Cupo)
            : base(Cupo)
        {
            base.Nombre = "Anulado Pendiente de Informar";
            base.Codigo = CodigoEstado.AnuladoPendienteInformar;
        }

        public override void Distribuir(long vendedor, Consignacion consignacion, string observacion, string contactoComercial, long destino, string centro, DateTime fecha, long uvdist, ISession session)
        {
            throw new NotImplementedException();
        }

        public override bool PuedeDistribuir()
        {
            return false;
        }

        public override void Informar(Cupos Encabezado, ISession session)
        {
            if (PuedeInformar())
            {
                //Cupos CupoEncabezado = ObtenerEncabezado(base.Cupo);
                base.Cupo.Pdf = 0;
                if (EsCuentaYOrden())
                    base.Cupo.Vendcta = 0;
                else
                    base.Cupo.Vendcta = Encabezado.Vendcta;
                base.Cupo.FechaYHoraInformado = DateTime.Now;
                base.CuposStore.Update(base.Cupo, session);
                base.Cupo.Estado = new EstadoAnulado(base.Cupo);
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

        public override void AnularDistribucion(string Motivo, Cupos Encabezado, ISession session)
        {
        }

        public override InformacionModificacionEstado AnularCupo(string Motivo, Cupos Encabezado, ISession session)
        {
            throw new NotImplementedException();
        }

        public override bool PuedeAnular()
        {
            return false;
        }
    }
}