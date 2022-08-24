using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class RelacionDistribucionCupos
    {
        public IList<Cupos> CuposDeDistribucion { get; set; }
        public CuposDist Distribucion { get; set; }
    }
}