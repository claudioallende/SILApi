using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
  public class RelacionPuertoSILPuertoSTOP
  {
    public virtual int Id { get; set; }

    [Display(Name = "Nro. Puerto SIL")]
    [DefaultValue(0)]
    public virtual long NroPuertoSIL { get; set; }

    [Display(Name = "Nombre Puerto SIL")]
    public virtual string NombrePuertoSIL { get; set; }

    [Display(Name = "Nro. Puerto STOP")]
    [DefaultValue(0)]
    public virtual long NroPuertoSTOP { get; set; }

    [Display(Name = "Nombre Puerto STOP")]
    public virtual string NombrePuertoSTOP { get; set; }

    [DefaultValue(0)]
    public virtual int ValorPorDefecto { get; set; }

    public override bool Equals(object obj)
    {
      RelacionPuertoSILPuertoSTOP recievedObject = (RelacionPuertoSILPuertoSTOP)obj;

      if (ReferenceEquals(recievedObject, null)) return false;
      if (ReferenceEquals(recievedObject, this)) return true;

      if ((this.NroPuertoSIL == recievedObject.NroPuertoSIL) && (this.NroPuertoSTOP== recievedObject.NroPuertoSTOP))
      {
        return (true);
      }
      return (false);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}