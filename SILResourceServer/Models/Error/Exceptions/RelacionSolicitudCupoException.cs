using System;

namespace ResourceServer.Models.Error.Exceptions
{
    /// <summary>
    /// Se lanza cuando la conciliación entre las asociaciones solicitud-cupo y los
    /// cupos efectivamente distribuidos falla (faltan cupos reales, la solicitud ya
    /// no está disponible, o los datos de la asociación son inconsistentes).
    /// Al derivar de BusinessException, ExceptionHandlingAttribute devuelve el mensaje
    /// al cliente y la transacción compartida hace rollback de CUPOSCORRE, CUPOSDIST,
    /// SOLTURNOS y SOLTURNOS_DETALLE.
    /// </summary>
    [Serializable()]
    public class RelacionSolicitudCupoException : BusinessException
    {
        public RelacionSolicitudCupoException(string mensaje) : base(mensaje) { }
    }
}
