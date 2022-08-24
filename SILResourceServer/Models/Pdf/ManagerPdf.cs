using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Pdf
{
    public class ManagerPdf
    {
        private IPdfBuilder PdfBuilder;
        public ManagerPdf(IPdfBuilder builder)
        {
            PdfBuilder = builder;
        }

        public void Construir(string path)
        {
            PdfBuilder.Construir(path);
        }

        public Document GetPdf()
        {
            return PdfBuilder.GetPdf();
        }

        public Stream GetStream()
        {
            return PdfBuilder.GetStream();
        }

        public void Download()
        {
            HttpContext.Current.Response.Buffer = false;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            //Set the appropriate ContentType.
            HttpContext.Current.Response.ContentType = "Application/pdf";
            //Write the file content directly to the HTTP content output stream.
            HttpContext.Current.Response.BinaryWrite(PdfBuilder.GetBytes());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
    }
}