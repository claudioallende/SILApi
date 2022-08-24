using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class Grano
    {
        public virtual string Id { get; set; }
        public virtual int CodigoGrano { get; set; }
        public virtual string Nombre { get; set; }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Grano c = (Grano)obj;
                return this.Id == c.Id;
            }
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}