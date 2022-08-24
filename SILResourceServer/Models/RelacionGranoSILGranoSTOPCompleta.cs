using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ResourceServer.Models
{
  public class RelacionGranoSILGranoSTOPCompleta
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
        
        public override bool Equals(object obj)
        {
            RelacionGranoSILGranoSTOPCompleta recievedObject = (RelacionGranoSILGranoSTOPCompleta)obj;

            if (ReferenceEquals(recievedObject, null)) return false;
            if (ReferenceEquals(recievedObject, this)) return true;

            if ((this.NroGranoSIL == recievedObject.NroGranoSIL) && (this.NroGranoSTOP == recievedObject.NroGranoSTOP))
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