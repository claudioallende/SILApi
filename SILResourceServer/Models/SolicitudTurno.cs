using System;

namespace ResourceServer.Models
{
  public class SolicitudTurno
  {
    public virtual long Id { get; set; }
    public virtual long? CuentaComprador { get; set; }
    public virtual long CuentaVendedor { get; set; }
    public virtual long? CuentaDestino { get; set; }
    public virtual TipoDestino? TipoDestino { get; set; }
    public virtual int CodigoEstado { get; set; }
    public virtual int CodigoGrano { get; set; }
    public virtual DateTime FechaCreacion { get; set; }
    public virtual DateTime FechaSolicitado { get; set; }
    public virtual bool EsFuturo { get; set; }
    public virtual string CodigoCentro { get; set; }
    public virtual string Observacion { get; set; }
    public virtual long? CupoId { get; set; }
  }

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
    public int CodigoEstado { get; set; }
    public int NombreEstado { get; set; }
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