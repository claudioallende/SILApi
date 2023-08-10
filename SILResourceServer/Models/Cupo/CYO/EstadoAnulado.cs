using ResourceServer.Models.Error.Exceptions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Cupo.CYO
{
    public class EstadoAnulado : EstadoCupo
    {
        public EstadoAnulado(Cupos Cupo) : base(Cupo) {
            base.Nombre = "Turno Cuenta y orden Anulado";
            base.Codigo = CodigoEstado.CuentaYOrdenAnulado;
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
            throw new NotImplementedException();
        }

        public override bool PuedeAnular()
        {
            return false;
        }
    }
}