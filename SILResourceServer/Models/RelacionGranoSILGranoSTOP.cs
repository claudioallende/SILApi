using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class RelacionGranoSILGranoSTOP
    {     
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor ingrese un NroGranoSIL")]
        public virtual long NroGranoSIL {get;set;}
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor ingrese un NRoGranoSTOP")]
        public virtual long NroGranoSTOP {get;set;}
        [DefaultValue(0)]
        public virtual int ValorPorDefecto { get; set; }

        public override bool Equals(object obj)
        {
            RelacionGranoSILGranoSTOP recievedObject = (RelacionGranoSILGranoSTOP)obj;

            if (ReferenceEquals(recievedObject, null)) return false;
            if (ReferenceEquals(recievedObject, this)) return true;

            if ((this.NroGranoSIL == recievedObject.NroGranoSIL) && (this.NroGranoSTOP== recievedObject.NroGranoSTOP))
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