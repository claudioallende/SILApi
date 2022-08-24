using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Pdf
{
    public class PlantillaPdf
    {
        private Document ArchivoPdf { get; set; }
        private Image Encabezado { get; set; }
        private Image Pie { get; set; }
        private string PathImagenEncabezado { get; set; }
        private string PathImagenPie { get; set; }

        private float MargenHoja = 0;

        public PlantillaPdf(Document pdf, PdfWriter writer)
        {
            ArchivoPdf = pdf;
            PathImagenEncabezado = HttpContext.Current.Server.MapPath("~") + "\\Models\\Images\\PdfImages\\EncabezadoPdf.jpg";
            PathImagenPie = HttpContext.Current.Server.MapPath("~") + "\\Models\\Images\\PdfImages\\PiePdf.jpg";
            EventNewPage eventoNuevaPagina = new EventNewPage(PathImagenEncabezado, PathImagenPie);
            writer.PageEvent = eventoNuevaPagina;
        }

        public Image GetEncabezado()
        {
            return Image.GetInstance(PathImagenEncabezado);
        }
        public Image GetPie()
        {
            return Image.GetInstance(PathImagenPie);
        }
        public void SetEncabezado()
        {
            //Encabezado = GetEncabezado();
            ////Escala imagen
            //Encabezado.ScaleAbsolute(ArchivoPdf.GetRight(MargenHoja) - 30, 110);
            //Encabezado.SpacingAfter = 20;
            ////Posiciona imagen
            //ArchivoPdf.Add(Encabezado);
        }
        public void SetPie()
        {
            //Pie = GetPie();
            ////Escala imagen
            //Pie.ScaleAbsolute(ArchivoPdf.GetRight(MargenHoja) - 30, 40);
            ////Posiciona imagen
            //Pie.SetAbsolutePosition(ArchivoPdf.GetLeft(MargenHoja), ArchivoPdf.GetBottom(MargenHoja));
            //ArchivoPdf.Add(Pie);
        }
        public float GetPosicionInicialXContenido()
        {
            return ArchivoPdf.GetLeft(10);
        }
        public float GetPosicionInicialYContenido()
        {
            return ArchivoPdf.GetTop(110);
        }
        public float GetAnchoContenido()
        {
            return ArchivoPdf.GetRight(10) - ArchivoPdf.GetLeft(10);
        }
        public float GetAltoContenido()
        {
            return ArchivoPdf.GetTop(110) - ArchivoPdf.GetBottom(50);
        }
        public float GetInterlineado() { return 5; }
        public Paragraph NuevaLineaTexto(string texto)
        {
            Paragraph p = new Paragraph(texto);
            p.SpacingAfter = 5;
            return p;
        }
        public void AgregarEspacioEnBlanco()
        {
            ArchivoPdf.Add(new Paragraph("\n"));
        }
        public PdfPCell NuevaCelda(string texto, int alineado)
        {
            Paragraph p = new Paragraph(texto);
            //p.Alignment = alineado;
            return new PdfPCell(p) { HorizontalAlignment = alineado, Padding = 5 };
        }
        public Rectangle[] GetColumnas(int cantidad, int yBottom, int yTop)
        {
            int margenCol = 6;
            int margenDoc = 25;
            float posx = margenDoc;
            float tamHoja = ArchivoPdf.PageSize.Width - (margenDoc * 2 + margenCol * (cantidad - 1));
            float tamCol = (tamHoja / cantidad);
            Rectangle[] array = new Rectangle[cantidad];
            for (var i = 0; i < cantidad; i++)
            {
                Rectangle rect = new Rectangle(posx, yBottom, posx + tamCol, yTop);
                rect.Border = Rectangle.BOX;
                array[i] = rect;
                posx += (margenCol + tamCol);
            }
            return array;
        }
        public void AgregarImagenTodasPaginas()
        {

        }
    }

    public class EventNewPage : PdfPageEventHelper
    {

        private Image ImagenEncabezado { get; set; }
        private Image ImagenPie { get; set; }
        private string PathImagenEncabezado { get; set; }
        private string PathImagenPie { get; set; }

        public EventNewPage(string pathEncabezado, string pathPie)
        {
            PathImagenEncabezado = pathEncabezado;
            PathImagenPie = pathPie;
        }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            ImagenEncabezado = Image.GetInstance(PathImagenEncabezado);
            ImagenPie = Image.GetInstance(PathImagenPie);
        }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            //Escala imagen
            ImagenEncabezado.ScaleAbsolute(document.GetRight(0) - 30, 110);
            ImagenEncabezado.SpacingAfter = 20;
            //Posiciona imagen
            document.Add(ImagenEncabezado);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            //Escala imagen
            ImagenPie.ScaleAbsolute(document.GetRight(0) - 30, 40);
            //Posiciona imagen
            ImagenPie.SetAbsolutePosition(document.GetLeft(0), document.GetBottom(0));
            document.Add(ImagenPie);
        }
    }
}