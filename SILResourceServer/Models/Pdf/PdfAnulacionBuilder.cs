using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Pdf
{
    public class PdfAnulacionBuilder : PdfDistribucionBuilder
    {
        public PdfAnulacionBuilder(IList<Cupos> cupos) : base(cupos) {}

        protected override void ConstruirEncabezadoTablaAlfanumericos(DateTime dia)
        {
            PdfPTable table = new PdfPTable(1);
            table.AddCell(Plantilla.NuevaCelda("DÍA: " + dia.ToString("dd/MM/yyyy"), Element.ALIGN_CENTER));
            table.AddCell(Plantilla.NuevaCelda("CÓDIGO ALFANUMÉRICO ANULADOS", Element.ALIGN_CENTER));
            ArchivoPdf.Add(table);
        }
    }
}