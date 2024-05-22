using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
  public class SolicitudTurnoStore
  {
    public IList<SolicitudTurnoView> GetByVendedor(long cuentaVendedor, DateTime fechaDesde, DateTime fechaHasta)
    {
      using (ISession session = HibernateUtil.OpenSession())
      {
        IQuery query = session.CreateSQLQuery(
          @"SELECT S.CTACOMP as ""CuentaComprador"", cc.nombre as ""NombreComprador"",  
            S.CTAVEND as ""CuentaVendedor"", cv.nombre as ""NombreVendedor"", 
            S.DEST as ""CuentaDestino"", cp.nombre as ""NombreDestino"", 
            S.GRANO as ""CodigoGrano"", cg.nombre as ""NombreGrano"",
            s.Fechasolicitada as ""Fechasolicitado"", 
            s.CENTRO as ""CodigoCentro"",
            cupo.status ""EstadoCupo"", cupo.estadocupocnrt ""CtgCupo""
            FROM SOLTURNOS s
            LEFT OUTER JOIN cuposcomprador cc ON s.ctacomp = cc.cuenta
            INNER JOIN cuposvendedor cv ON CV.CUENTA = S.CTAVEND
            LEFT OUTER JOIN cupospuerto cp ON CP.CUENTA = s.dest
            INNER JOIN cuposgrano cg ON s.grano = cg.grano
            LEFT OUTER JOIN cuposcorre cupo ON s.cupo_id = cupo.id
            WHERE s.ctavend = :p0"
        ).SetResultTransformer(Transformers.AliasToBean<SolicitudTurnoView>());
        query.SetInt64("p0", cuentaVendedor);
        IList<SolicitudTurnoView> solicitudes = query.List<SolicitudTurnoView>();
        HibernateUtil.Dispose();
        return solicitudes;
      }
    }

    public IEnumerable<SolicitudTurnoView> GetAll(DateTime? desde, DateTime? hasta, bool futuro = false)
    {
      using (ISession session = HibernateUtil.OpenSession())
      {
        string strQuery = @"SELECT S.CTACOMP as ""CuentaComprador"", cc.nombre as ""NombreComprador"",  
            S.CTAVEND as ""CuentaVendedor"", cv.nombre as ""NombreVendedor"", 
            S.DEST as ""CuentaDestino"", cp.nombre as ""NombreDestino"", 
            S.GRANO as ""CodigoGrano"", cg.nombre as ""NombreGrano"",
            s.Fechasolicitada as ""Fechasolicitado"", 
            s.CENTRO as ""CodigoCentro"",
            cupo.status ""EstadoCupo"", cupo.estadocupocnrt ""CtgCupo""
            FROM SOLTURNOS s
            LEFT OUTER JOIN cuposcomprador cc ON s.ctacomp = cc.cuenta
            INNER JOIN cuposvendedor cv ON CV.CUENTA = S.CTAVEND
            LEFT OUTER JOIN cupospuerto cp ON CP.CUENTA = s.dest
            INNER JOIN cuposgrano cg ON s.grano = cg.grano
            LEFT OUTER JOIN cuposcorre cupo ON s.cupo_id = cupo.id
            WHERE s.Futuro = ?";

        if (desde.HasValue && hasta.HasValue)
          strQuery += " AND s.Fechasolicitada BETWEEN ? AND ?";

        IQuery query = session.CreateSQLQuery(strQuery).SetResultTransformer(Transformers.AliasToBean<SolicitudTurnoView>());
        query.SetInt32(0, futuro ? 1 : 0);
        if (desde.HasValue && hasta.HasValue)
        {
          query.SetDateTime(1, desde.Value);
          query.SetDateTime(2, hasta.Value);
        }
        IList<SolicitudTurnoView> solicitudes = query.List<SolicitudTurnoView>();
        HibernateUtil.Dispose();
        return solicitudes;
      }
    }

    public void Add(SolicitudTurno solicitud, ISession session)
    {
      session.Save(solicitud);
    }

    public Task AddAsync(SolicitudTurno solicitud, ISession session)
    {
      return session.SaveAsync(solicitud);
    }
  }
}