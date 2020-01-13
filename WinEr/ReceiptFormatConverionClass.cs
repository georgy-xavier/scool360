using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System.Diagnostics;
using PdfSharp.Pdf;
using MigraDoc;
using MigraDoc.DocumentObjectModel.Tables;
using System.Collections;
using System.IO;
using System.Xml;

namespace WinBase
{
    public class ReceiptFormatConverionClass
    {
        public void Convert_HTML_Sting_to_PDF_File(string _PhysicalPath,string html_string,out string PDF_FileName)
        {
            PDF_FileName="";

            Document document = LoadPDFPage(html_string);
            //    {CreateDocument();
            const PdfFontEmbedding embedding = PdfFontEmbedding.Always;

            // ----------------------------------------------------------------------------------------

            const bool unicode = false;

            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF


            pdfRenderer.RenderDocument();

            // Save the document...
            string filename = "", MainName = "";

           

            filename = _PhysicalPath + "\\PDF_Files\\FR_" + MainName + ".pdf";

                pdfRenderer.PdfDocument.Save(filename);
                // ...and start a viewer.
           
        }

        private Document LoadPDFPage(string html_string)
        {
           
           
            Document document = new Document();
            StringReader _Reader=new StringReader(html_string);


            XmlDocument doc = new XmlDocument();
            doc.LoadXml(html_string);
            //#region just to remind myself something
            //XmlNode node = doc.CreateElement("serkan");
            //doc.DocumentElement.AppendChild(node);
            //#endregion
            StringWriter sw = new StringWriter();
            XmlTextWriter tx = new XmlTextWriter(sw);
            doc.WriteTo(tx);

           
            return document;

        }

    }
}
