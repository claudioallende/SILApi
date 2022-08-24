using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class IdViewModel
    {
        private string _cyo = "false";
        public string Id { get; set; }
        public string CentroOrigen { get; set; }
        public string CentroDistribucion { get; set; }
        public string Cyo { get { return _cyo; } set { _cyo = value; } }
    }
}