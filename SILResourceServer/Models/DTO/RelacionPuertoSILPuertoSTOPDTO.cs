using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DTO
{
  public class RelacionPuertoSILPuertoSTOPDTO
  {
    public virtual int Id { get; set; }
    [Display(Name = "Nro. Puerto SIL")]
    [DefaultValue(0)]
    public virtual long NroPuertoSIL { get; set; }

    [Display(Name = "Nombre Puerto")]
    public virtual string nombrePuertoSil { get; set; }

    [Display(Name = "Nro. Puerto STOP")]
    [Range(0, long.MaxValue, ErrorMessage = "Por favor ingrese un NRoPuertoSTOP")]
    [DefaultValue(0)]
    public virtual long NroPuertoSTOP { get; set; }

    [Display(Name = "Nombre Puerto")]
    public virtual string nombrePuertoSTOP { get; set; }
    [DefaultValue(0)]
    public virtual int ValorPorDefecto { get; set; }

    public RelacionPuertoSILPuertoSTOP ToRelacionPuertoSILPuertoSTOP()
    {
      RelacionPuertoSILPuertoSTOP aux = new RelacionPuertoSILPuertoSTOP();
      aux.Id = this.Id;
      aux.NroPuertoSIL = this.NroPuertoSIL;
      aux.NroPuertoSTOP = this.NroPuertoSTOP;
      aux.ValorPorDefecto = this.ValorPorDefecto;
      return aux;
    }
  }
}