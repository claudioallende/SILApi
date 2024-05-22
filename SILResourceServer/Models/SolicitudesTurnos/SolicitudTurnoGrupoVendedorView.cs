using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.SolicitudesTurnos
{
  public class SolicitudTurnoGrupoVendedorView
  {
    public int CodigoGrano { get; set; }
    public string NombreGrano { get; set; }
    public IEnumerable<SolicitudTurnoGrupoDetallePendienteDiaView> DetallePendientesDia { get; set; }
    public IEnumerable<SolicitudTurnoGrupoDetalleSolicitadoView> DetallesSolicitados { get; set; }

  }

  public class SolicitudTurnoGrupoDetallePendienteDiaView
  {
    public string Fecha { get; set; }
    public int Cantidad { get; set; }
  }

  public class SolicitudTurnoGrupoDetalleSolicitadoView
  {
    public long? CuentaComprador { get; set; }
    public string NombreComprador { get; set; }
    public long? CuentaDestino { get; set; }
    public string NombreDestino { get; set; }
    public IEnumerable<SolicitudTurnoGrupoDetalleSolicitadoDiaView> DetallesSolicitadosDia { get; set; }
  }

  public class SolicitudTurnoGrupoDetalleSolicitadoDiaView
  {
    public string Fecha { get; set; }
    public int TurnosOtorgados { get; set; }
    public int TurnosActivos { get; set; }
  }
}