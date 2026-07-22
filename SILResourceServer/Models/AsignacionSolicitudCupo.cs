using System.Collections.Generic;

namespace ResourceServer.Models
{
    /// <summary>
    /// Modo de operación del endpoint api/Cupos/ActualizarDistribucion.
    /// Si el request no lo informa, se asume DistribucionManual (compatibilidad).
    /// </summary>
    public enum ModoActualizacionDistribucion
    {
        DistribucionManual = 1,
        SolicitudMatch = 2
    }

    /// <summary>
    /// Asociación solicitud-cupo enviada por las pantallas.
    /// - Distribucion.cshtml (DistribucionManual): informa CupoReferenciaId + la clave
    ///   de fila/día; SILApi decide el CUPOSCORRE.Id final realmente distribuido.
    /// - AltaSolicitud.cshtml (SolicitudMatch): informa CupoSeleccionadoId, que es el
    ///   id exacto a distribuir y también el cupo_id que se insertará en SOLTURNOS_DETALLE.
    /// </summary>
    public class AsignacionSolicitudCupoDto
    {
        public long SolicitudId { get; set; }

        // AltaSolicitud: ID exacto que se debe distribuir.
        public long? CupoSeleccionadoId { get; set; }

        // Distribucion: referencia del matching; el ID final lo decide SILApi.
        public long? CupoReferenciaId { get; set; }

        public int Cantidad { get; set; }
        public string MatchType { get; set; }

        // Clave de fila/día utilizada por Distribucion.cshtml.
        public long Compcta { get; set; }
        public long Vendcta { get; set; }
        public int Codproducto { get; set; }
        public long Ctadestino { get; set; }
        public string Cosecha { get; set; }
        public string Centro { get; set; }
        public int Fechaent { get; set; }
        public int Dia { get; set; }

        /// <summary>
        /// Clave normalizada de fila/día usada para conciliar el DTO con los cupos
        /// efectivamente distribuidos por ServicioDistribuir. Debe construirse con los
        /// mismos campos que <see cref="ServicioDistribuir"/> usa al registrar los cupos.
        /// </summary>
        public string ClaveFilaDia()
        {
            return ClaveFilaDia(Compcta, Vendcta, Codproducto, Ctadestino, Cosecha, Centro, Dia);
        }

        public static string ClaveFilaDia(long compcta, long vendcta, int codproducto,
            long ctadestino, string cosecha, string centro, int dia)
        {
            return string.Join("|",
                compcta,
                vendcta,
                codproducto,
                ctadestino,
                (cosecha ?? string.Empty).Trim(),
                (centro ?? string.Empty).Trim(),
                dia);
        }
    }

    /// <summary>
    /// Relación solicitud-cupo efectivamente persistida en SOLTURNOS_DETALLE.
    /// </summary>
    public class AsignacionRealizadaDto
    {
        public long SolicitudId { get; set; }
        public long CupoId { get; set; }
    }

    /// <summary>
    /// Resultado estructurado de api/Cupos/ActualizarDistribucion. Reemplaza al int
    /// que devolvía el endpoint; <see cref="Codigo"/> conserva la semántica legacy
    /// (1 = OK, 100/200 = errores, 300 = sin cambios) para el modo manual.
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

        public static ActualizarDistribucionResult DesdeCodigo(int codigo)
        {
            return new ActualizarDistribucionResult
            {
                Codigo = codigo,
                Success = codigo == 1,
                Message = null
            };
        }
    }
}
