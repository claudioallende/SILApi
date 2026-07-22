using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class RegistroDistribucionViewModel : IValidatableObject
    {
        // NOTA: los [Required] de los campos del modo manual se reemplazaron por
        // validación condicional en Validate(...) para permitir el modo SolicitudMatch,
        // que no envía la grilla (cupos/nuevo/anterior/puerto/consignación/centro).
        public IList<VistaCuposDistribuidos> cupos { get; set; }
        public Cupos nuevo { get; set; }
        public Cupos anterior { get; set; }
        public string puerto { get; set; }
        [Display(Name="Consignacion")]
        public Consignacion ConsignacionSeleccionada { get; set; }
        //[Required]
        //[RegularExpression("^([0-9]{4})|0$", ErrorMessage = "Formato incorrecto")]
        public string CosechaDesde { get; set; }
        //[Required]
        //[RegularExpression("^([0-9]{4})|0$", ErrorMessage = "Formato incorrecto")]
        public string CosechaHasta { get; set; }
        public DateTime? fecha { get; set; }
        public bool tieneVendedor { get; set; }
        [Display(Name = "Centro")]
        public string CentroSeleccionado { get; set; }
        public DateTime? fechaDesde { get; set; }
        public bool Confirmacion { get; set; }

        /// <summary>
        /// Modo de operación. Si el request no lo informa (clientes previos),
        /// se asume DistribucionManual.
        /// </summary>
        public ModoActualizacionDistribucion? Modo { get; set; }

        /// <summary>
        /// Relaciones solicitud-cupo. Opcional: null/vacía ⇒ distribución tradicional
        /// sin match (comportamiento actual intacto).
        /// </summary>
        public IList<AsignacionSolicitudCupoDto> AsignacionesSolicitudCupo { get; set; }

        public ModoActualizacionDistribucion ModoEfectivo()
        {
            return Modo ?? ModoActualizacionDistribucion.DistribucionManual;
        }

        public bool TieneAsignaciones()
        {
            return AsignacionesSolicitudCupo != null && AsignacionesSolicitudCupo.Any();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ModoEfectivo() == ModoActualizacionDistribucion.SolicitudMatch)
            {
                // SolicitudMatch: exige pares explícitos SolicitudId/CupoSeleccionadoId con Cantidad = 1.
                if (!TieneAsignaciones())
                {
                    yield return new ValidationResult(
                        "El modo SolicitudMatch requiere al menos una asignación.",
                        new[] { nameof(AsignacionesSolicitudCupo) });
                    yield break;
                }

                int i = 0;
                foreach (var a in AsignacionesSolicitudCupo)
                {
                    if (a.SolicitudId <= 0)
                        yield return new ValidationResult(
                            "SolicitudId debe ser positivo.",
                            new[] { $"AsignacionesSolicitudCupo[{i}].SolicitudId" });
                    if (!a.CupoSeleccionadoId.HasValue || a.CupoSeleccionadoId.Value <= 0)
                        yield return new ValidationResult(
                            "CupoSeleccionadoId debe ser positivo en modo SolicitudMatch.",
                            new[] { $"AsignacionesSolicitudCupo[{i}].CupoSeleccionadoId" });
                    if (a.Cantidad != 1)
                        yield return new ValidationResult(
                            "Cada par SolicitudId/CupoSeleccionadoId debe tener Cantidad = 1.",
                            new[] { $"AsignacionesSolicitudCupo[{i}].Cantidad" });
                    i++;
                }
            }
            else
            {
                // DistribucionManual: mantiene los campos obligatorios del flujo actual.
                if (cupos == null)
                    yield return new ValidationResult("cupos es obligatorio.", new[] { nameof(cupos) });
                if (nuevo == null)
                    yield return new ValidationResult("nuevo es obligatorio.", new[] { nameof(nuevo) });
                if (anterior == null)
                    yield return new ValidationResult("anterior es obligatorio.", new[] { nameof(anterior) });
                if (string.IsNullOrWhiteSpace(puerto))
                    yield return new ValidationResult("puerto es obligatorio.", new[] { nameof(puerto) });
                if (ConsignacionSeleccionada == null)
                    yield return new ValidationResult("Consignacion es obligatoria.", new[] { nameof(ConsignacionSeleccionada) });
                if (string.IsNullOrWhiteSpace(CentroSeleccionado))
                    yield return new ValidationResult("Centro es obligatorio.", new[] { nameof(CentroSeleccionado) });
            }
        }
    }
}
