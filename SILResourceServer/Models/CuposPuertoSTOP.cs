using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class CuposPuertoSTOP : ICuenta
    {
        [Display(Name = "Nro. Puerto STOP")]
        [DefaultValue(1)]
        public virtual long NroPuerto { get; set; }
        [Display(Name = "Nombre Puerto")]
        public virtual string NombrePuerto { get; set; }
        public virtual long Cuenta { get => NroPuerto; set => NroPuerto = value; }
        public virtual string Nombre { get => NombrePuerto; set => NombrePuerto = value; }
        public virtual string Cuit { get => NroPuerto.ToString(); set => throw new NotImplementedException(); }
    }
}