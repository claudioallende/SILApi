using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Email
{
    public class DominioEImagen
    {
        public string Dominio { get; set; }
        public string Imagen { get; set; }

    }
    public class DominioEImagenForMultiLine
    {
        public string nombre { get; set; }
        public int codigoNombre { get; set; }
        public IList<DominioEImagen> ListOfDominioEImagen { get; set; }
    }
}