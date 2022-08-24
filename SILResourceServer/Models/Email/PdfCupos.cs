using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Email
{
    public class PdfCupos
    {
        public Consignacion Consignacion { get; set; }
        public string Pdf { get; set; }
        public IList<Cupos> CuposAInformar { get; set; }
    }
}