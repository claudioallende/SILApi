using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class CodigosAlfanumericos
    {
        public int NumeroDia { 
            get {
                return (Dia.Date - DateTime.Now.Date).Days;
            } 
        }
        public DateTime Dia { get; set; }
        public string Alfanumerico { get; set; }
        public string CantidadCupos { get; set; }

        public string DiaFormateado { 
            get 
            {
                return Dia.ToString("dd/MM");
            }
            set { }
        }

        public CodigosAlfanumericos() {}

        public bool CodigosIsNullOrEmpty()
        {
            return string.IsNullOrEmpty(this.Alfanumerico) && string.IsNullOrEmpty(this.CantidadCupos);
        }

        public string[] obtenerCodigosAlfanumericos()
        {
            if (!string.IsNullOrEmpty(Alfanumerico))
            {
                return obtenerCodigosAlfanumericos(new ObtenerCodigosDesdeString(Alfanumerico));
            }
            if (!string.IsNullOrEmpty(CantidadCupos)) 
            {
                return obtenerCodigosAlfanumericos(new ObtenerCodigosDesdeNroDeAlfa(CantidadCupos, this.Dia));
            }
            return null;
        }

        public string[] obtenerCodigosAlfanumericos(IObtenerCodigosAlfanumericos objObtener)
        {
            return objObtener.obtenerCodigosAlfanumericos();
        }

        public int GetCantidadCupos()
        {
            return obtenerCodigosAlfanumericos().Count();
        }
    }
}