using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.Pdf
{
    public interface IPdfBuilder
    {
        void Construir(string path);
        void ConstruirEncabezado();
        void ConstruirContenido();
        void ConstruirPie();
        Document GetPdf();
        byte[] GetBytes();
        Stream GetStream();
    }
}
