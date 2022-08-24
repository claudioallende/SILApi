using ResourceServer.Models.Error.Exceptions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Cupo
{
    public class EstadoAnulado : EstadoCupo
    {
        public EstadoAnulado(Cupos Cupo) : base(Cupo) {
            base.Nombre = "Turno Anulado";
            base.Codigo = CodigoEstado.Anulado;
        }

        public override void Distribuir(long vendedor, Consignacion consignacion, string observacion, long destino, string centro, DateTime fecha, long uvdist, ISession session)
        {
            throw new NotImplementedException();
        }

        public override void Informar(Cupos Encabezado, ISession session)
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
            return false;
        }

        public void ActualizarEstado()
        {

        }

        public override void AnularDistribucion(string Motivo, Cupos Encabezado, ISession session)
        {
            //throw new CupoSinDistribucionException();
        }

        public override InformacionModificacionEstado AnularCupo(string Motivo, Cupos Encabezado, ISession session)
        {
            throw new NotImplementedException();
        }
    }
}