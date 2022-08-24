using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class InformacionAlfanumerico
    {
        public long Id { get; set; }
        public string Alfanumerico { get; set; }
        public bool EsCuentaYOrden { get; set; }
        public bool EstaConsumido { get; set; }
        public string Observacion { get; set; }
    }
}