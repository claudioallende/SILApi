using System.Collections.Generic;

namespace ResourceServer.Models.DTO
{
  /// <summary>
  /// Modo de operación de POST /api/Cupos/ActualizarDistribucion en SILApi.
  /// Espejo de CuposCorretajeWeb.Models.ModoActualizacionDistribucion.
  /// </summary>
  public enum ModoActualizacionDistribucion
  {
    /// <summary>
    /// Comportamiento legacy (Distribucion.cshtml). El cliente envía cantidades
    /// agrupadas por fila/día y SILApi elige los CUPOSCORRE.Id físicos a
    /// distribuir.
    /// </summary>
    DistribucionManual = 1,

    /// <summary>
    /// Aceptación y distribución desde AltaSolicitud.cshtml. El cliente envía
    /// pares explícitos SolicitudId / CupoSeleccionadoId (unidades) y SILApi
    /// distribuye exactamente esos cupos.
    /// </summary>
    SolicitudMatch = 2
  }

  /// <summary>
  /// Asociación solicitud-cupo enviada a SILApi en modo SolicitudMatch (o como
  /// asociación opcional en DistribucionManual). Espejo de
  /// CuposCorretajeWeb.Models.AsignacionSolicitudCupoDto.
  /// </summary>
  public class AsignacionSolicitudCupoDto
  {
    /// <summary>Id de la solicitud a la que se le asigna el cupo.</summary>
    public long SolicitudId { get; set; }

    /// <summary>
    /// AltaSolicitud (SolicitudMatch): ID exacto del CUPOSCORRE que debe
    /// distribuirse y que se persiste como CUPO_ID en SOLTURNOS_DETALLE.
    /// Obligatorio en modo SolicitudMatch.
    /// </summary>
    public long? CupoSeleccionadoId { get; set; }

    /// <summary>
    /// DistribucionManual: referencia del matching; el ID final lo decide SILApi
    /// en función de los cupos disponibles y debe reconciliarse con el campo
    /// CuposEfectivamenteDistribuidosPorFilaDia del ServicioDistribuir.
    /// </summary>
    public long? CupoReferenciaId { get; set; }

    /// <summary>
    /// Cantidad a asignar. En SolicitudMatch se fija siempre en 1 (cada card
    /// representa un cupo y no se permite subdivisión intra-cupo).
    /// </summary>
    public int Cantidad { get; set; }

    /// <summary>
    /// Tipo de match devuelto por el motor: "Directo" | "Parcial" | "Condicional".
    /// Sólo diagnóstico; el backend lo ignora al persistir.
    /// </summary>
    public string MatchType { get; set; }

    /// <summary>Clave de fila/día de Distribucion.cshtml. No se usa en SolicitudMatch.</summary>
    public long Compcta { get; set; }
    /// <summary>Clave de fila/día de Distribucion.cshtml. No se usa en SolicitudMatch.</summary>
    public long Vendcta { get; set; }
    /// <summary>Clave de fila/día de Distribucion.cshtml. No se usa en SolicitudMatch.</summary>
    public int Codproducto { get; set; }
    /// <summary>Clave de fila/día de Distribucion.cshtml. No se usa en SolicitudMatch.</summary>
    public long Ctadestino { get; set; }
    /// <summary>Clave de fila/día de Distribucion.cshtml. No se usa en SolicitudMatch.</summary>
    public string Cosecha { get; set; }
    /// <summary>Clave de fila/día de Distribucion.cshtml. No se usa en SolicitudMatch.</summary>
    public string Centro { get; set; }
    /// <summary>Clave de fila/día de Distribucion.cshtml. No se usa en SolicitudMatch.</summary>
    public int Fechaent { get; set; }
    /// <summary>Día 0..20 dentro de la fila. No se usa en SolicitudMatch.</summary>
    public int Dia { get; set; }
  }

  /// <summary>
  /// Relación solicitud-cupo efectivamente persistida por SILApi en SOLTURNOS_DETALLE.
  /// Espejo de CuposCorretajeWeb.Models.AsignacionRealizadaDto.
  /// </summary>
  public class AsignacionRealizadaDto
  {
    public long SolicitudId { get; set; }
    public long CupoId { get; set; }
  }

  /// <summary>
  /// Respuesta estructurada de POST /api/Cupos/ActualizarDistribucion.
  /// Espejo de CuposCorretajeWeb.Models.ActualizarDistribucionResult.
  /// <see cref="Codigo"/> conserva la semántica legacy (1 = OK con cambios,
  /// 100/200 = errores, 300 = sin cambios) para compatibilidad con
  /// Distribucion.cshtml. En SolicitudMatch el flujo siempre termina en commit
  /// o rollback, así que Codigo será 1 cuando la operación es OK.
  /// </summary>
  public class ActualizarDistribucionResult
  {
    public int Codigo { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }

    public int Solicitados { get; set; }
    public int Asignados { get; set; }
    public int Pendientes { get; set; }

    public IList<AsignacionRealizadaDto> Relaciones { get; set; }

    public ActualizarDistribucionResult()
    {
      Relaciones = new List<AsignacionRealizadaDto>();
    }
  }
}