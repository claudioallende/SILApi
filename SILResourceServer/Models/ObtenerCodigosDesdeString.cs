using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class ObtenerCodigosDesdeString : IObtenerCodigosAlfanumericos
    {
        private string CodigosAlfanumericos = "";

        public ObtenerCodigosDesdeString(string codigos)
        {
            this.CodigosAlfanumericos = codigos.Replace(" ", string.Empty);
        }

        string[] IObtenerCodigosAlfanumericos.obtenerCodigosAlfanumericos()
        {
            string[] separators = { "\r\n", "\r", "\n" };
            string[] alfanumericos = CodigosAlfanumericos.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return alfanumericos.Distinct().ToArray();
        }
    }
}