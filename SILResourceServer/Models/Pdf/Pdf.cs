using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace CuposCorretaje.Models.Pdf
{
    public class Pdf
    {

        public Pdf()
	    {
		    //
		    // TODO: Add constructor logic here
		    //
        }
        
        public int GenerarArchivo( Cupos cupoDatos) {
            System.IO.FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("pdf") + "\\" + "primerPDF.pdf", FileMode.Create);
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);

            document.AddAuthor("Micke Blomquist");
            document.AddCreator("Sample application using iTextSharp");
            document.AddKeywords("PDF tutorial education");
            document.AddSubject("Document subject - Describing the steps creating a PDF document");
            document.AddTitle("The document title - PDF creation using iTextSharp");
            return 1;
        }


        
    }
}