using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class CuposGranoSTOP
    {
        [Display(Name = "Nro. Grano STOP")]
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor ingrese un NRoGranoSTOP")]
        [DefaultValue(1)]
        public virtual long NroGrano { get; set; }
        [Display(Name = "Nombre Grano")]
        public virtual string NombreGrano { get; set; }
    }
}