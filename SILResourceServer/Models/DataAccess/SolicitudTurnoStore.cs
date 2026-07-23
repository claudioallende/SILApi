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

    // =========================================================================
    // Métodos nuevos para SolicitudMatch (Bloque 3 del plan).
    // Operan sobre ISession ya abierta para reusar la transacción NHibernate
    // del flujo legacy (atomicidad "todo o nada" sobre CUPOSCORRE, CUPOSDIST,
    // SOLTURNOS y SOLTURNOS_DETALLE).
    // =========================================================================

    /// <summary>
    /// Carga las SolicitudTurno de la tabla SOLTURNOS por una lista de IDs.
    /// Usa SQL nativo porque el modelo NHibernate (SolicitudTurno) sólo mapea
    /// columnas legacy; los IDs vienen ya validados por el caller.
    /// </summary>
    public IList<SolicitudTurno> GetByIds(IList<long> ids, ISession session)
    {
      if (ids == null || ids.Count == 0) return new List<SolicitudTurno>();

      // NHibernate HQL: Select por PK in (...)
      return session.Query<SolicitudTurno>()
                    .Where(s => ids.Contains(s.Id))
                    .ToList();
    }

    /// <summary>
    /// Actualiza los acumuladores cantidad_aceptada y cantidad_futuro_aceptada
    /// de SOLTURNOS por SQL nativo de Oracle. Promueve STATUS a Otorgada (2)
    /// cuando cantidad_aceptada alcanza cantidad. Sólo asigna cupo_id si la fila
    /// aún no tiene uno (la lista completa de cupos vive en SOLTURNOS_DETALLE).
    ///
    /// Devuelve la cantidad de filas afectadas. Si es 0, hay conflicto y el
    /// caller debe hacer rollback completo.
    /// </summary>
    public int IncrementarAceptada(long solicitudId, int n, bool esFuturo, long cupoId, ISession session)
    {
      const string sql = @"
        UPDATE SOLTURNOS
           SET cantidad_aceptada = cantidad_aceptada + :n,
               cantidad_futuro_aceptada = cantidad_futuro_aceptada + CASE WHEN :esfuturo = 1 THEN :n ELSE 0 END,
               status = CASE WHEN cantidad_aceptada + :n = cantidad THEN 2 ELSE status END,
               cupo_id = CASE WHEN status = 0 AND cupo_id IS NULL THEN :cupoid ELSE cupo_id END
         WHERE solturnos_id = :solicitudid
           AND status = 0
           AND cupo_id IS NULL";

      var query = session.CreateSQLQuery(sql);
      query.SetParameter("n", n);
      query.SetParameter("esfuturo", esFuturo ? 1 : 0);
      query.SetParameter("cupoid", cupoId);
      query.SetParameter("solicitudid", solicitudId);
      return query.ExecuteUpdate();
    }

    /// <summary>
    /// Inserta una fila en SOLTURNOS_DETALLE (solicitud_id, cupo_id, estado=1,
    /// fecha_creacion=SYSDATE). El trigger trg_soldet_bi autoincrementa
    /// detalle_id desde SOLTURNOS_DETALLE_SEQ.
    /// Devuelve la cantidad de filas insertadas (1 OK, 0 error).
    /// </summary>
    public int InsertarDetalle(long solicitudId, long cupoId, ISession session)
    {
      const string sql = @"
        INSERT INTO SOLTURNOS_DETALLE (solicitud_id, cupo_id, estado, fecha_creacion)
        VALUES (:solicitudid, :cupoid, 1, SYSDATE)";

      var query = session.CreateSQLQuery(sql);
      query.SetParameter("solicitudid", solicitudId);
      query.SetParameter("cupoid", cupoId);
      return query.ExecuteUpdate();
    }

    /// <summary>
    /// Helper opcional: verifica si un cupo ya está relacionado a una solicitud
    /// en SOLTURNOS_DETALLE. Se usa para detectar conflictos antes del INSERT
    /// y lanzar una excepción de negocio en lugar de esperar el ORA-00001 del
    /// UNIQUE constraint uk_soldet_solicitud_cupo.
    /// </summary>
    public bool ExisteDetalle(long solicitudId, long cupoId, ISession session)
    {
      const string sql = @"
        SELECT COUNT(*) FROM SOLTURNOS_DETALLE
         WHERE solicitud_id = :solicitudid AND cupo_id = :cupoid";

      var query = session.CreateSQLQuery(sql);
      query.SetParameter("solicitudid", solicitudId);
      query.SetParameter("cupoid", cupoId);
      var result = query.UniqueResult();
      if (result == null) return false;
      return Convert.ToInt32(result) > 0;
    }
  }
}