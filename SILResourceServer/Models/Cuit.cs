using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class CuposCuit : ICuenta
    {
        public virtual string Cuit { get; set; }
        public virtual string Nombre { get; set; }
        public virtual string Domicilio { get; set; }
        public virtual string Localidad { get; set; }
        public virtual string Provincia { get; set; }
        public virtual long Cuenta { get; set; }

        public override bool Equals(object obj)
        {
            CuposCuit recievedObject = (CuposCuit)obj;

            if (ReferenceEquals(recievedObject, null)) return false;
            if (ReferenceEquals(recievedObject, this)) return true;

            if ((this.Cuit == recievedObject.Cuit) && (this.Nombre == recievedObject.Nombre))
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