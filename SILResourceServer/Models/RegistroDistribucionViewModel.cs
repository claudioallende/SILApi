using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ResourceServer.Models.DTO;

namespace ResourceServer.Models
{
    /// <summary>
    /// VM de POST /api/Cupos/ActualizarDistribucion en SILApi.
    /// Se usa para DistribucionManual (legacy) y para SolicitudMatch (AltaSolicitud).
    /// En modo SolicitudMatch sólo se envía <see cref="Modo"/> +
    /// <see cref="AsignacionesSolicitudCupo"/> + <see cref="Confirmacion"/>; el
    /// resto queda null/empty.
    /// </summary>
    public class RegistroDistribucionViewModel
    {
        /// <summary>
        /// Modo de operación. Default = <see cref="ModoActualizacionDistribucion.DistribucionManual"/>
        /// para no romper callers legacy que no lo informen.
        /// </summary>
        public ModoActualizacionDistribucion? Modo { get; set; }

        /// <summary>
        /// Lista de asociaciones solicitud-cupo que SILApi debe procesar. Sólo
        /// se usa en modo <see cref="ModoActualizacionDistribucion.SolicitudMatch"/>
        /// o como asociaciones opcionales en DistribucionManual. En SolicitudMatch
        /// cada entrada lleva <see cref="AsignacionSolicitudCupoDto.CupoSeleccionadoId"/>
        /// explícito; en DistribucionManual lleva <see cref="AsignacionSolicitudCupoDto.CupoReferenciaId"/>
        /// y SILApi resuelve el id final.
        /// </summary>
        public IList<AsignacionSolicitudCupoDto> AsignacionesSolicitudCupo { get; set; }

        /// <summary>
        /// Legacy Distribucion.cshtml. Obligatorio cuando
        /// <see cref="Modo"/> == <see cref="ModoActualizacionDistribucion.DistribucionManual"/>
        /// (validación runtime en CuposController). En SolicitudMatch queda null.
        /// </summary>
        public IList<VistaCuposDistribuidos> cupos { get; set; }

        /// <summary>Legacy Distribucion.cshtml. Obligatorio en DistribucionManual.</summary>
        public Cupos nuevo { get; set; }

        /// <summary>Legacy Distribucion.cshtml. Obligatorio en DistribucionManual.</summary>
        public Cupos anterior { get; set; }

        /// <summary>Legacy Distribucion.cshtml. Obligatorio en DistribucionManual.</summary>
        public string puerto { get; set; }

        /// <summary>Legacy Distribucion.cshtml. Obligatorio en DistribucionManual.</summary>
        [Display(Name = "Consignacion")]
        public Consignacion ConsignacionSeleccionada { get; set; }

        //[Required]
        //[RegularExpression("^([0-9]{4})|0$", ErrorMessage = "Formato incorrecto")]
        public string CosechaDesde { get; set; }
        //[Required]
        //[RegularExpression("^([0-9]{4})|0$", ErrorMessage = "Formato incorrecto")]
        public string CosechaHasta { get; set; }
        public DateTime? fecha { get; set; }
        public bool tieneVendedor { get; set; }

        /// <summary>Legacy Distribucion.cshtml. Obligatorio en DistribucionManual.</summary>
        [Display(Name = "Centro")]
        public string CentroSeleccionado { get; set; }

        public DateTime? fechaDesde { get; set; }
        public bool Confirmacion { get; set; }
    }
}