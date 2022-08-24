using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public static class LocalType
    {
        [Flags]
        public enum ContratosPor : short
        {
            Cuenta = 600,
            Cuit = 655            
        }
    }
    public class filtroInterface
    {
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public IList<long> Vendedores { get; set; }
    }
}