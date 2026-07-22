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

    /// <summary>
    /// Carga varias solicitudes por id dentro de la sesión del caller
    /// (compartida con la transacción de distribución + relación).
    /// Devuelve sólo las que existan; las faltantes quedan en <paramref name="idsFaltantes"/>.
    /// </summary>
    public IList<SolicitudTurno> GetByIds(IEnumerable<long> ids, ISession session, out IList<long> idsFaltantes)
    {
      if (session == null) throw new ArgumentNullException(nameof(session));
      idsFaltantes = new List<long>();

      var lista = (ids ?? Enumerable.Empty<long>())
        .Where(id => id > 0)
        .Distinct()
        .ToList();
      if (lista.Count == 0) return new List<SolicitudTurno>();

      var encontrados = session.Query<SolicitudTurno>()
        .Where(s => lista.Contains(s.Id))
        .ToList();

      var encontradosSet = new HashSet<long>(encontrados.Select(s => s.Id));
      foreach (long id in lista)
      {
        if (!encontradosSet.Contains(id)) idsFaltantes.Add(id);
      }
      return encontrados;
    }

    // ====================================================================
    // SOLTURNOS_DETALLE — modelo "Detalle Acumulativo" (portado de ACA_SILData)
    // Todos los métodos operan sobre la ISession/transacción del caller
    // (ServicioDistribuir), para que la atomicidad "todo o nada" abarque
    // CUPOSCORRE, CUPOSDIST, SOLTURNOS y SOLTURNOS_DETALLE en un único commit.
    // ====================================================================

    /// <summary>
    /// UPDATE atómico de SOLTURNOS: incrementa el acumulador correspondiente
    /// (CANTIDAD_ACEPTADA si la solicitud no es futura, CANTIDAD_FUTURO_ACEPTADA
    /// si lo es — el FUTURO se lee de la propia fila) y promueve STATUS a
    /// Otorgada (=2) cuando el acumulador alcanza CANTIDAD. La guardia
    /// STATUS = 0 AND CUPO_ID IS NULL mantiene la concurrencia optimista:
    /// si la solicitud ya no está pendiente o ya tiene cupo, rowcount = 0
    /// y se devuelve false (el caller debe abortar la transacción).
    /// </summary>
    public bool IncrementarAceptada(long solicitudId, int incremento, ISession session)
    {
      if (solicitudId <= 0)
        throw new ArgumentOutOfRangeException(nameof(solicitudId), "Id de solicitud inválido.");
      if (incremento <= 0)
        throw new ArgumentOutOfRangeException(nameof(incremento), "El incremento debe ser positivo.");

      const string sql = @"
        UPDATE SOLTURNOS
           SET cantidad_aceptada        = cantidad_aceptada        + CASE WHEN NVL(FUTURO,0) = 0 THEN :inc ELSE 0 END,
               cantidad_futuro_aceptada = cantidad_futuro_aceptada + CASE WHEN NVL(FUTURO,0) = 1 THEN :inc ELSE 0 END,
               STATUS = CASE
                          WHEN (cantidad_aceptada + CASE WHEN NVL(FUTURO,0) = 0 THEN :inc ELSE 0 END) >= NVL(cantidad,0)
                          THEN 2 ELSE 0
                        END
         WHERE SOLTURNOS_ID = :solicitudId
           AND STATUS = 0
           AND CUPO_ID IS NULL";

      int filas = session.CreateSQLQuery(sql)
        .SetInt32("inc", incremento)
        .SetInt64("solicitudId", solicitudId)
        .ExecuteUpdate();

      return filas > 0;
    }

    /// <summary>
    /// Inserta una fila en SOLTURNOS_DETALLE por cada cupo aceptado
    /// (estado = 1 = Asignado, fecha_creacion = SYSDATE). detalle_id lo genera
    /// el trigger trg_soldet_bi. La constraint UNIQUE (solicitud_id, cupo_id)
    /// protege contra duplicados: cualquier violación se propaga para que el
    /// caller haga rollback. Se ejecuta en la sesión/transacción del caller.
    /// </summary>
    public void InsertarDetalle(long solicitudId, IEnumerable<long> cupoIds, ISession session)
    {
      if (solicitudId <= 0)
        throw new ArgumentOutOfRangeException(nameof(solicitudId), "Id de solicitud inválido.");

      var ids = (cupoIds ?? Enumerable.Empty<long>())
        .Where(id => id > 0)
        .Distinct()
        .ToList();
      if (ids.Count == 0) return;

      const string sql = @"
        INSERT INTO SOLTURNOS_DETALLE (solicitud_id, cupo_id, estado, fecha_creacion)
        VALUES (:solicitudId, :cupoId, 1, SYSDATE)";

      foreach (long cupoId in ids)
      {
        session.CreateSQLQuery(sql)
          .SetInt64("solicitudId", solicitudId)
          .SetInt64("cupoId", cupoId)
          .ExecuteUpdate();
      }
    }
  }
}