using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class RegistroDistribucionViewModel
    {
        [Required]
        public IList<VistaCuposDistribuidos> cupos { get; set; }
        [Required]
        public Cupos nuevo { get; set; }
        [Required]
        public Cupos anterior { get; set; }
        [Required]
        public string puerto { get; set; }
        [Required]
        [Display(Name="Consignacion")]
        public string ConsignacionSeleccionada { get; set; }
        //[Required]
        //[RegularExpression("^([0-9]{4})|0$", ErrorMessage = "Formato incorrecto")]
        public string CosechaDesde { get; set; }
        //[Required]
        //[RegularExpression("^([0-9]{4})|0$", ErrorMessage = "Formato incorrecto")]
        public string CosechaHasta { get; set; }
        public DateTime? fecha { get; set; }
        public bool tieneVendedor { get; set; }
        [Required]
        [Display(Name = "Centro")]
        public string CentroSeleccionado { get; set; }
        public DateTime? fechaDesde { get; set; }
        public bool Confirmacion { get; set; }
    }
}