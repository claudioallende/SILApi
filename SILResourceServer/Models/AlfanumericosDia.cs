using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class AlfanumericosDia
    {
        public DateTime Dia { get; set; }
        public IList<InformacionAlfanumerico> Alfanumericos { get; set; }
    }
}