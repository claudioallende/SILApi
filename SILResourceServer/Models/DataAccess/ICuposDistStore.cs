using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
    public interface ICuposDistStore
    {
        void Save(CuposDist c);
        void Save(CuposDist c, ISession session);
        Int64 Save(CuposDist c, ISession sesion, ITransaction transaccion);
        //void Update(Int64 Id, CuposDist c, ISession session, ITransaction tx);
        void Update(CuposDist Distribucion, ISession Session);
        void Update(IList<CuposDist> Distribuciones, ISession Session);
        void Delete(Int64 Uvalue);
        CuposDist FindByUvalue(long uvalue);
        CuposDist FindByUvalue(long uvalue, ISession Session);
        CuposDist FindByCompVendGranoDestCentroFecha(Int64 ctacomp, Int64 ctaVend, int grano, Int64 destino, string centro, DateTime fecha);
        CuposDist FindByCompVendGranoDestCentroFechaConsignacion(Int64 ctacomp, Int64 ctaVend, int grano, Int64 destino, string centro, DateTime fecha, Consignacion Consignacion);
        CuposDist FindByCompVendGranoDestCentroFechaConsignacion(Int64 ctacomp, Int64 ctaVend, int grano, Int64 destino, string centro, DateTime fecha, Consignacion Consignacion, ISession Session);
        IList<CuposDist> FindAll();
        IList<CuposDist> FindByUvalues(IList<long> Uvalues);
        IList<CuposDist> FindByUvalues(IList<long> Uvalues, ISession Session);
        IList<CuposDist> FindByCompVendGranoDestCentroFechasConsignacion(long CuentaComprador, long CuentaVendedor, int CodigoGrano, long Destino, string Centro, IList<DateTime> Fechas, Consignacion Consignacion);
        IList<CuposDist> FindByCompVendGranoDestCentroFechasConsignacion(long CuentaComprador, long CuentaVendedor, int CodigoGrano, long Destino, string Centro, IList<DateTime> Fechas, Consignacion Consignacion, ISession Session);
    }
}
