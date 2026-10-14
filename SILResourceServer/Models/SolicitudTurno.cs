using System;

namespace ResourceServer.Models
{
  /// <summary>
  /// Entidad NHibernate mapeada a la tabla <c>SOLTURNOS</c> (mapping
  /// <c>SolTurnos.mpg.xml</c>).
  ///
  /// Tras la refactor que elimina <c>SOLTURNOS.STATUS</c> y
  /// <c>SOLTURNOS_DETALLE.ESTADO</c>, el estado de la solicitud ya no se
  /// persiste como columna; se deriva de los acumuladores
  /// (<c>cantidad_aceptada</c>, <c>cantidad_rechazada</c>, <c>cantidad</c>)
  /// al consultar, así que el modelo NHibernate no expone
  /// <c>CodigoEstado</c>. Las escrituras (alta, incremento de aceptadas) se
  /// hacen por SQL nativo (ver <see cref="DataAccess.SolicitudTurnoStore"/>)
  /// y nunca intentan popular esa columna.
  /// </summary>
  public class SolicitudTurno
  {
    public virtual long Id { get; set; }
    public virtual long? CuentaComprador { get; set; }
    public virtual long CuentaVendedor { get; set; }
    public virtual long? CuentaDestino { get; set; }
    public virtual TipoDestino? TipoDestino { get; set; }
    public virtual int CodigoGrano { get; set; }
    public virtual DateTime FechaCreacion { get; set; }
    public virtual DateTime FechaSolicitado { get; set; }
    public virtual bool EsFuturo { get; set; }
    public virtual string CodigoCentro { get; set; }
    public virtual string Observacion { get; set; }
    public virtual long? CupoId { get; set; }
  }

  /// <summary>
  /// DTO de lectura usado por los endpoints de Pantalla 1. Se hidrata con
  /// <c>AliasToBean</c> desde SQL nativo (ver <see cref="DataAccess.SolicitudTurnoStore.GetByVendedor"/>
  /// y <see cref="DataAccess.SolicitudTurnoStore.GetAll"/>) y se devuelve al
  /// front como JSON.
  ///
  /// Tras la refactor que elimina <c>SOLTURNOS.STATUS</c>, este DTO ya no
  /// expone <c>CodigoEstado</c>/<c>NombreEstado</c>: ninguna query los
  /// popularía y son ruido. Lo que viene de CUPOSCORRE (status/estadocupocnrt)
  /// sí se conserva como <see cref="EstadoCupo"/>/<see cref="CtgCupo"/> y
  /// pertenece al dominio del cupo, no de la solicitud.
  /// </summary>
  public class SolicitudTurnoView
  {
    public long Id { get; set; }
    public long? CuentaComprador { get; set; }
    public string NombreComprador { get; set; }
    public long CuentaVendedor { get; set; }
    public string NombreVendedor { get; set; }
    public long? CuentaDestino { get; set; }
    public string NombreDestino { get; set; }
    public TipoDestino? TipoDestino { get; set; }
    public int CodigoGrano { get; set; }
    public string NombreGrano { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaSolicitado { get; set; }
    public bool EsFuturo { get; set; }
    public string CodigoCentro { get; set; }
    public string NombreCentro { get; set; }
    public string Observacion { get; set; }
    public short? EstadoCupo { get; set; }
    public short? CtgCupo { get; set; }
  }
}
