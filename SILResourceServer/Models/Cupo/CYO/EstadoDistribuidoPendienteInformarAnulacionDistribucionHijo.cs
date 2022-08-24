using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Cupo.CYO
{
    public class EstadoDistribuidoPendienteInformarAnulacionDistribucionHijo : EstadoCupo
    {
        public EstadoDistribuidoPendienteInformarAnulacionDistribucionHijo(Cupos Cupo)
            : base(Cupo)
        {
            base.Nombre = "Distribuido Cuenta y orden con Hijo Pendiente de Informar Anulación de Distribución";
            base.Codigo = CodigoEstado.CuentaYOrdenDistribuidoPendienteInformarAnulacionDistribucionHijo;
        }

        public override void Distribuir(long vendedor, Consignacion consignacion, string observacion, long destino, string centro, DateTime fecha, long Uvdist, NHibernate.ISession session)
        {
        }

        public override bool PuedeDistribuir()
        {
            return true;
        }

        public override void Informar(Cupos Encabezado, NHibernate.ISession session)
        {
            base.Cupo.Status = 5;
            base.Cupo.Estado = new EstadoDistribuidoConHijoDistribuido(base.Cupo);
        }

        public override bool PuedeInformar()
        {
            return false;
        }

        public override void AnularDistribucion(string Motivo, Cupos Encabezado, NHibernate.ISession session)
        {
            base.Cupo.Status = 4;
            base.Cupo.Estado = new EstadoDistribuidoPendienteDistribuirHijo(base.Cupo);
        }

        public override InformacionModificacionEstado AnularCupo(string Motivo, Cupos Encabezado, NHibernate.ISession session)
        {
            base.Cupo.Status = 3;
            base.Cupo.Estado = new EstadoAnulado(base.Cupo);
            return new InformacionModificacionEstado();
        }

        public override bool PuedeAnular()
        {
            return true;
        }
    }
}