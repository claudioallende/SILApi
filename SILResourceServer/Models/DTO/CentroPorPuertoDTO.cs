using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DTO
{
    public class CentroPorPuertoDTO
    {
        public virtual long Id { get; set; }
        [Display(Name = "IdTerminal")]
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor ingrese un número")]
        public virtual long IdTerminal { get; set; }
        public virtual long CuentaTerminal { get; set; }
        public virtual string NombreTerminal { get; set; }
        [Display(Name = "CodigoCentro")]
        [Required]        
        public virtual string CodigoCentro { get; set; }
        public virtual string NombreCentro { get; set; }

        public CentroPorPuertoDTO() { }

        public CentroPorPuertoDTO( CuposCentroPorPuerto RelacionPuertCentro) 
        {
            this.Id = RelacionPuertCentro.Id;
            this.IdTerminal = RelacionPuertCentro.IdTerminal;
            this.CodigoCentro = RelacionPuertCentro.CodigoCentro;
        }

        public CuposCentroPorPuerto ToCentroPorPuerto() 
        {
            CuposCentroPorPuerto a = new CuposCentroPorPuerto();
            a.Id = this.Id;
            a.IdTerminal = this.IdTerminal;
            a.CodigoCentro = this.CodigoCentro;
            return a;
        }
    }
}