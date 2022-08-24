using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DTO
{
    public class RelacionGranoSILGranoSTOPDTO
    {
        [Display(Name = "Nro. Grano SIL")]
        //[Required]
        [Range(0, long.MaxValue, ErrorMessage = "Por favor ingrese un NroGranoSIL")]
        [DefaultValue(0)]
        public virtual long NroGranoSIL { get; set; }
        [Display(Name = "Nombre Grano")]
        public virtual string nombreGranoSil { get; set; }
        [Display(Name = "Nro. Grano STOP")]
        //[Required]
        [Range(0, long.MaxValue, ErrorMessage = "Por favor ingrese un NRoGranoSTOP")]
        [DefaultValue(0)]
        public virtual long NroGranoSTOP { get; set; }
        [Display(Name = "Nombre Grano")]
        public virtual string nombreGranoSTOP { get; set; }
        [DefaultValue(0)]
        public virtual int ValorPorDefecto { get; set; }
        public virtual int Id { get; set; }

        public RelacionGranoSILGranoSTOPCompleta ToRelacionGranoSILGranoSTOPCompleta() 
        {
            RelacionGranoSILGranoSTOPCompleta aux = new RelacionGranoSILGranoSTOPCompleta();
            aux.Id = this.Id;
            aux.NroGranoSIL = this.NroGranoSIL;
            aux.NroGranoSTOP = this.NroGranoSTOP;
            aux.ValorPorDefecto = this.ValorPorDefecto;
            return aux;
        }
    }
}