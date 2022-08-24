using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ResourceServer.Models.Pdf
{
    public class PdfDistribucionBuilder : IPdfBuilder
    {

        protected Document ArchivoPdf { get; set; }
        protected PlantillaPdf Plantilla { get; set; }
        protected Cupos DatosDelCupo { get; set; }
        private PdfWriter Writer { get; set; }
        //protected FileStream fs { get; set; }
        protected ServicioPdfDistribucion ServicioDistribucion { get; set; }
        private string UbicacionPdf { get; set; }

        public PdfDistribucionBuilder(IList<Cupos> cupos)
        {
            ServicioDistribucion = new ServicioPdfDistribucion(cupos);
            DatosDelCupo = ServicioDistribucion.GetCupo();
        }

        public void Construir(string path)
        {
            UbicacionPdf = HttpContext.Current.Server.MapPath(path);
            using (FileStream fs = new FileStream(UbicacionPdf, FileMode.Create))
            {
                ArchivoPdf = new Document(PageSize.A4, 25, 25, 30, 30);
                Writer = PdfWriter.GetInstance(ArchivoPdf, fs);
                Plantilla = new PlantillaPdf(ArchivoPdf, Writer);
                ArchivoPdf.Open();
                ConstruirEncabezado();
                ConstruirContenido();
                ConstruirPie();
                ArchivoPdf.Close();
            }
        }

        //Al utilizar la plantilla setea con el evento de inicio la imagen
        public void ConstruirEncabezado()
        {
        }

        public void ConstruirContenido()
        {
            ConstruirDatos(DatosDelCupo);
            ConstruirTablaAlfanumericos(DatosDelCupo.Fecha, ServicioDistribucion.GetAlfanumericosAInformar());
        }

        private void ConstruirDatos(Cupos cupo)
        {
            Puerto puerto = ServicioDistribucion.GetPuerto(DatosDelCupo.Puerto);
            Grano grano = ServicioDistribucion.GetGrano(DatosDelCupo.Grano);
            Plantilla.AgregarEspacioEnBlanco();
            NuevaLinea("TITULAR CARTA DE PORTE: " + cupo.Nomsolicitante + (!string.IsNullOrEmpty(cupo.Cuitsolicitante) ? " - CUIT: " + cupo.Cuitsolicitante : ""));
            NuevaLinea("RTE. COMERCIAL PRODUCTOR: " + cupo.NomRteComercialProductor + (!string.IsNullOrEmpty(cupo.CuitRteComercialProductor) ? " - CUIT: " + cupo.CuitRteComercialProductor : ""));
            NuevaLinea("RTE. COMERCIAL VENTA PRIMARIA: " + cupo.NomRteComercialVentaPrimaria + (!string.IsNullOrEmpty(cupo.CuitRteComercialVentaPrimaria) ? " - CUIT: " + cupo.CuitRteComercialVentaPrimaria : ""));
            NuevaLinea("RTE. COMERCIAL VENTA SECUNDARIA: " + cupo.Nomintermediario + (!string.IsNullOrEmpty(cupo.Cuitintermediario) ? " - CUIT: " + cupo.Cuitintermediario : ""));
            NuevaLinea("RTE. COMERCIAL VENTA SECUNDARIA 2: " + cupo.Nomrtecomercial + (!string.IsNullOrEmpty(cupo.Cuitrtecomercial) ? " - CUIT: " + cupo.Cuitrtecomercial : ""));
            NuevaLinea("MERCADO A TÉRMINO: " + cupo.Nommat + (!string.IsNullOrEmpty(cupo.Cuitmat) ? " - CUIT: " + cupo.Cuitmat : ""));
            NuevaLinea("CORREDOR VENTA PRIMARIA: " + cupo.Nomcorrvta + (!string.IsNullOrEmpty(cupo.Cuitcorrvta) ? " - CUIT: " + cupo.Cuitcorrvta : ""));
            NuevaLinea("CORREDOR VENTA SECUNDARIA: " + cupo.Nomcorrcomp + (!string.IsNullOrEmpty(cupo.Cuitcorrcomp) ? " - CUIT: " + cupo.Cuitcorrcomp : ""));
            NuevaLinea("REPRESENTANTE ENTREGADOR: " + cupo.Nomrteent + (!string.IsNullOrEmpty(cupo.Cuitrteent) ? " - CUIT: " + cupo.Cuitrteent : ""));
            NuevaLinea("DESTINATARIO: " + cupo.Nomdestinatario + (!string.IsNullOrEmpty(cupo.Cuitdestinatario) ? " - CUIT: " + cupo.Cuitdestinatario : ""));
            NuevaLinea("DESTINO: " + puerto.Nombre + (!string.IsNullOrEmpty(puerto.Cuit) ? " - CUIT: " + puerto.Cuit : ""));
            NuevaLinea("GRANO/ESPECIE: " + grano.Nombre);
            NuevaLinea("OBSERVACIONES: " + cupo.Observa);
            NuevaLinea("DIRECCIÓN: " + puerto.Domicilio + " - " + puerto.Cpostal + " " + puerto.Nombre);
            NuevaLinea("Nº DE PLANTA: " + ServicioDistribucion.GetNroPlanta(DatosDelCupo.Puerto));
            Plantilla.AgregarEspacioEnBlanco();
        }

        private void NuevaLinea(string dato)
        {
            ArchivoPdf.Add(Plantilla.NuevaLineaTexto(dato));
        }

        private void ConstruirTablaAlfanumericos(DateTime dia, IList<string> codigos)
        {
            ConstruirEncabezadoTablaAlfanumericos(dia);
            ConstruirContenidoTablaAlfanumericos(codigos);
        }

        protected virtual void ConstruirEncabezadoTablaAlfanumericos(DateTime dia)
        {
            PdfPTable table = new PdfPTable(1);
            table.AddCell(Plantilla.NuevaCelda("DÍA: " + dia.ToString("dd/MM/yyyy"), Element.ALIGN_CENTER));
            table.AddCell(Plantilla.NuevaCelda("CÓDIGO ALFANUMÉRICO", Element.ALIGN_CENTER));
            ArchivoPdf.Add(table);
        }

        private void ConstruirContenidoTablaAlfanumericos(IList<string> codigos)
        {
            int cantidadColumnas = 0;
            if (codigos.Any(x => x.Length >= 24))
                cantidadColumnas = 2;
            else
                cantidadColumnas = 3;
            Rectangle[] COLUMNS = Plantilla.GetColumnas(cantidadColumnas, 100, (int)Writer.GetVerticalPosition(true) - 5);
            bool nuevaPagina = false;
            ColumnText ct = new ColumnText(Writer.DirectContent);
            ct.AddElement(ConstruirListasAlfanumericos(codigos));
            int column = 0;
            do
            {
                if (column == cantidadColumnas)
                {
                    nuevaPagina = true;
                    ArchivoPdf.NewPage();
                    column = 0;
                }
                if (nuevaPagina)
                {
                    for (var i = 0; i < COLUMNS.Length; i++)
                    {
                        COLUMNS[i].Top = 670;
                    }
                }
                ct.SetSimpleColumn(COLUMNS[column++]);
            } while (ColumnText.HasMoreText(ct.Go()));
        }

        private PdfPTable ConstruirListasAlfanumericos(IList<string> codigos)
        {
            PdfPTable table = new PdfPTable(1);
            table.DefaultCell.Border = 0;
            foreach (var codigo in codigos)
            {
                table.AddCell(codigo);
            }
            return table;
        }

        //Al utilizar la plantilla setea con el evento de fin la imagen
        public void ConstruirPie()
        {
        }

        public Document GetPdf()
        {
            return ArchivoPdf;
        }


        public byte[] GetBytes()
        {
            return File.ReadAllBytes(UbicacionPdf);
        }


        public Stream GetStream()
        {
            return null;
        }
    }
}