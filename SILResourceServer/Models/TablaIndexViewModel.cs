using ResourceServer.Models.Vista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class TablaIndexViewModel
    {
        public IList<ICuposAgrupados> Datos { get; set; }
        public IList<ColumnaTablaHTML> Columnas { get; set; }
        public bool TieneFiltroGrano { get; set; }
        public bool TieneUnCentro { get; set; }
        public readonly int CantidadDias = 20;
    }
}