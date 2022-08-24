using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class Vendedor : ICuenta
    {
        public virtual long Id { get; set; }
        public virtual long Cuenta { get; set; }
        [Display(Name = "CUIT")]
        public virtual string Cuit { get; set; }
        public virtual string Domicilio { get; set; }
        public virtual string Localidad { get; set; }
        public virtual string Nombre { get; set; }
        public virtual string Provincia { get; set; }
        [Display(Name = "Tipo Proveedor")]
        public virtual int Stipprovee { get; set; }
        [Display(Name = "Tipo de Cuenta")]
        public virtual string Tipodecuenta { get; set; }
        public virtual int Interexter { get; set; }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Vendedor c = (Vendedor)obj;
                return this.Id == c.Id;
            }
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}