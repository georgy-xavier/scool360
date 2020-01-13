using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WC.PDF.Document;
using System.Data;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Pdf;
using MigraDoc.Rendering;
using System.Diagnostics;

namespace WC.PdfDocumentClass
{
    public class DocumentCreator
    {
        public WCpdfDocument DocDefinition { get; set; }
        public Dictionary<string, string> dataDictionary { get; set; }
        public Dictionary<string, string> imageUrlDictionary { get; set; }
        public Dictionary<string, DataSet> DatasetDictionary {get;set;}

        private PDFUtility obj_pdfutility = null;


        public DocumentCreator()
        {

        }
        public bool CreateDocument(ref Document PDFdocument, PageSetup pgSetup)
        {
            if (obj_pdfutility==null)
                obj_pdfutility = new PDFUtility();

            if (DocDefinition == null)
                return false;

            Section section = null;
            MigraDoc.DocumentObjectModel.Tables.Table table = null;
            MigraDoc.DocumentObjectModel.Tables.Row row = null;
            MigraDoc.DocumentObjectModel.Tables.Column Column = null;
            MigraDoc.DocumentObjectModel.Tables.Cell cel = null;
            double _Tblwidth = 0, _TblHeight = 0;

            foreach (WC.PDF.Document.WCpdfDocumentSection objsection in DocDefinition.Section)
            {
                #region pagesetup

                if (objsection.PageFormatSetting == "BasedonPageFormate")
                {
                    if (objsection.PageFormat == "A0" || objsection.PageFormat == "A1" || objsection.PageFormat == "A2" ||
                        objsection.PageFormat == "A3" || objsection.PageFormat == "A4" || objsection.PageFormat == "A5" ||
                        objsection.PageFormat == "A6" || objsection.PageFormat == "B5" || objsection.PageFormat == "Ledger" ||
                        objsection.PageFormat == "Legal" || objsection.PageFormat == "Letter" || objsection.PageFormat == "P11x17")
                        pgSetup.PageFormat = obj_pdfutility.GetPageFormateSize(pgSetup, objsection.PageFormat.ToString());
                    else if (objsection.PageFormat == "Landscape")
                        pgSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Landscape;
                    else
                        pgSetup.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.A4;
                }
                else
                {
                    if (objsection.PageFormat == "Landscape")
                    {
                        pgSetup.PageWidth = double.Parse(objsection.PgSetupHeight.ToString());
                        pgSetup.PageHeight = double.Parse(objsection.PgSetupWidth.ToString());
                    }
                    else
                    {
                        pgSetup.PageWidth = double.Parse(objsection.PgSetupWidth.ToString());
                        pgSetup.PageHeight = double.Parse(objsection.PgSetupHeight.ToString());
                    }
                }

                if (objsection.PgSetupPadSettings == "Manule")
                {
                    pgSetup.LeftMargin = double.Parse(objsection.PgSetupLeftMargin.ToString());
                    pgSetup.RightMargin = double.Parse(objsection.PgSetupRightMargin.ToString());
                    pgSetup.TopMargin = double.Parse(objsection.PgSetupTopMarigin.ToString());
                    pgSetup.BottomMargin = double.Parse(objsection.PgSetupBottomMargin.ToString());
                }



                #endregion


              
                if (objsection.PageFormat == "Landscape")
                {
                    _Tblwidth = pgSetup.PageHeight - (pgSetup.LeftMargin + pgSetup.RightMargin);
                    _TblHeight = pgSetup.PageWidth - (pgSetup.TopMargin + pgSetup.BottomMargin);
                }
                else
                {
                    _Tblwidth = pgSetup.PageWidth - (pgSetup.LeftMargin + pgSetup.RightMargin);
                    _TblHeight = pgSetup.PageHeight - (pgSetup.TopMargin + pgSetup.BottomMargin);
                }
                _Tblwidth = Math.Round((double)_Tblwidth, 2);
                _TblHeight = Math.Round((double)_TblHeight, 2);

                section = PDFdocument.AddSection();
                
                table = section.AddTable();
                table.Format.Alignment = MigraDoc.DocumentObjectModel.ParagraphAlignment.Center;
                Column = table.AddColumn(_Tblwidth);
                row = table.AddRow();
                row.Format.Alignment = MigraDoc.DocumentObjectModel.ParagraphAlignment.Center;
                row.Height = _TblHeight;
                row.Cells[0].Format.Alignment = MigraDoc.DocumentObjectModel.ParagraphAlignment.Center;
                cel = row.Cells[0];
                table.Borders.Width = double.Parse(objsection.MainTableBorderWidth.ToString());


                DocumentParser objDocParser = new DocumentParser(pgSetup, DocDefinition, dataDictionary, imageUrlDictionary, DatasetDictionary);
                double TableWidth = 0, TableHeight = 0;
                TableWidth = _Tblwidth;

                string sections = objsection.ContainSections.ToString();
                string[] m_section = sections.Split(',');

                foreach (string sectionname in m_section)
                {
                    switch (sectionname)
                    {
                        case "Header":
                            #region Add Header

                            WCpdfDocumentSectionHeader objHeader = objsection.Header.FirstOrDefault();
                            TableHeight = Convert.ToInt32(Math.Round((_TblHeight * double.Parse(objHeader.Height.ToString())) / 100));
                            MigraDoc.DocumentObjectModel.Tables.Table Header = new MigraDoc.DocumentObjectModel.Tables.Table();
                            foreach (WC.PDF.Document.Table tblHeader in objHeader.Table)
                            {
                                Header = objDocParser.GetTable(tblHeader, TableWidth, TableHeight);
                                cel.Elements.Add(Header);
                            }

                            #endregion
                            break;
                        case "Body":
                            #region Add Body
                            WCpdfDocumentSectionBody objBody = objsection.Body.FirstOrDefault();

                            TableWidth = TableWidth - ((double.Parse(objsection.MainTableBorderWidth.ToString())) * 2);
                            TableWidth = Math.Round((double)TableWidth, 2);

                            TableHeight = _TblHeight - ((double.Parse(objsection.MainTableBorderWidth.ToString())) * 2);
                            TableHeight = ((TableHeight * double.Parse(objBody.Height.ToString())) / 100);
                            TableHeight = Math.Round((double)TableHeight, 2);

                            MigraDoc.DocumentObjectModel.Tables.Table Body = new MigraDoc.DocumentObjectModel.Tables.Table();
                            foreach (WC.PDF.Document.Table tblBody in objBody.Table)
                            {
                                Body = objDocParser.GetTable(tblBody, TableWidth, TableHeight);
                                cel.Elements.Add(Body);
                            }
                            #endregion
                            break;
                        case "Footer":
                            #region Add Footer

                            WCpdfDocumentSectionFooter objFooter = objsection.Footer.FirstOrDefault();
                            TableHeight = Convert.ToInt32(Math.Round((_TblHeight * double.Parse(objFooter.Height.ToString())) / 100));
                            MigraDoc.DocumentObjectModel.Tables.Table Footer = new MigraDoc.DocumentObjectModel.Tables.Table();
                            foreach (WC.PDF.Document.Table tblFooter in objFooter.Table)
                            {
                                Footer = objDocParser.GetTable(tblFooter, TableWidth, TableHeight);
                                cel.Elements.Add(Footer);
                            }

                            #endregion
                            break;
                    }
                }
            }

            return true;
        }

        


    }
}

#region  f
//public bool CreateDocument()
//        {
//            obj_pdfutility = new PDFUtility();

//            if (DocDefinition == null)
//                return false;

//            if (string.IsNullOrEmpty(FilePath))
//                return false;

//            if (string.IsNullOrEmpty(FileName))
//                return false;

//            string strPhysicalFilePath = FilePath + FileName;



//            Document PDFdocument = new Document();
//            PageSetup pgSetup = PDFdocument.DefaultPageSetup;

//            Section section = null;
//            MigraDoc.DocumentObjectModel.Tables.Table table = null;
//            MigraDoc.DocumentObjectModel.Tables.Row row = null;
//            MigraDoc.DocumentObjectModel.Tables.Column Column = null;
//            MigraDoc.DocumentObjectModel.Tables.Cell cel = null;
//            double _Tblwidth = 0, _TblHeight = 0;


//            foreach (WC.PDF.Document.WCpdfDocumentSection objsection in DocDefinition.Section)
//            {
//                #region pagesetup

//                if (objsection.PageFormatSetting == "BasedonPageFormate")
//                {
//                    if (objsection.PageFormat == "A0" || objsection.PageFormat == "A1" || objsection.PageFormat == "A2" ||
//                        objsection.PageFormat == "A3" || objsection.PageFormat == "A4" || objsection.PageFormat == "A5" ||
//                        objsection.PageFormat == "A6" || objsection.PageFormat == "B5" || objsection.PageFormat == "Ledger" ||
//                        objsection.PageFormat == "Legal" || objsection.PageFormat == "Letter" || objsection.PageFormat == "P11x17")
//                        pgSetup.PageFormat = obj_pdfutility.GetPageFormateSize(pgSetup, objsection.PageFormat.ToString());
//                    else if (objsection.PageFormat == "Landscape")
//                        pgSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Landscape;
//                    else
//                        pgSetup.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.A4;
//                }
//                else
//                {
//                    if (objsection.PageFormat == "Landscape")
//                    {
//                        pgSetup.PageWidth = double.Parse(objsection.PgSetupHeight.ToString());
//                        pgSetup.PageHeight = double.Parse(objsection.PgSetupWidth.ToString());
//                    }
//                    else
//                    {
//                        pgSetup.PageWidth = double.Parse(objsection.PgSetupWidth.ToString());
//                        pgSetup.PageHeight = double.Parse(objsection.PgSetupHeight.ToString());
//                    }
//                }

//                if (objsection.PgSetupPadSettings == "Manule")
//                {
//                    pgSetup.LeftMargin = double.Parse(objsection.PgSetupLeftMargin.ToString());
//                    pgSetup.RightMargin = double.Parse(objsection.PgSetupRightMargin.ToString());
//                    pgSetup.TopMargin = double.Parse(objsection.PgSetupTopMarigin.ToString());
//                    pgSetup.BottomMargin = double.Parse(objsection.PgSetupBottomMargin.ToString());
//                }



//                #endregion

//                section = PDFdocument.AddSection();
//                table = section.AddTable();
//                _Tblwidth = pgSetup.PageWidth - (pgSetup.LeftMargin + pgSetup.RightMargin);
//                _TblHeight = pgSetup.PageHeight - (pgSetup.TopMargin + pgSetup.BottomMargin);

//                _Tblwidth = Math.Round((double)_Tblwidth, 2);
//                _TblHeight = Math.Round((double)_TblHeight, 2);

//                Column = table.AddColumn(_Tblwidth);
//                row = table.AddRow();
//                row.Height = _TblHeight;
//                cel = row.Cells[0];
//                //row.Cells[0].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
//                table.Borders.Width = double.Parse(objsection.MainTableBorderWidth.ToString());

//                DocumentParser objDocParser = new DocumentParser(pgSetup, DocDefinition, dataDictionary, imageUrlDictionary, dsDataTables);
//                double TableWidth = 0, TableHeight = 0;
//                TableWidth = _Tblwidth;

//                string sections = objsection.ContainSections.ToString();
//                string[] m_section = sections.Split(',');

//                foreach (string sectionname in m_section)
//                {
//                    switch (sectionname)
//                    {
//                        case "Header":
//                            #region Add Header

//                            WCpdfDocumentSectionHeader objHeader = objsection.Header.FirstOrDefault();
//                            TableHeight = Convert.ToInt32(Math.Round((_TblHeight * double.Parse(objHeader.Height.ToString())) / 100));
//                            MigraDoc.DocumentObjectModel.Tables.Table Header = new MigraDoc.DocumentObjectModel.Tables.Table();
//                            foreach (WC.PDF.Document.Table tblHeader in objHeader.Table)
//                            {
//                                Header = objDocParser.GetTable(tblHeader, TableWidth, TableHeight);
//                                cel.Elements.Add(Header);
//                            }

//                            #endregion
//                            break;
//                        case "Body":
//                            #region Add Body
//                            WCpdfDocumentSectionBody objBody = objsection.Body.FirstOrDefault();

//                            TableWidth = TableWidth - ((double.Parse(objsection.MainTableBorderWidth.ToString())) * 2);
//                            TableWidth = Math.Round((double)TableWidth, 2);

//                            TableHeight = _TblHeight - ((double.Parse(objsection.MainTableBorderWidth.ToString())) * 2);
//                            TableHeight = ((TableHeight * double.Parse(objBody.Height.ToString())) / 100);
//                            TableHeight = Math.Round((double)TableHeight, 2);

//                            MigraDoc.DocumentObjectModel.Tables.Table Body = new MigraDoc.DocumentObjectModel.Tables.Table();
//                            foreach (WC.PDF.Document.Table tblBody in objBody.Table)
//                            {
//                                Body = objDocParser.GetTable(tblBody, TableWidth, TableHeight);
//                                cel.Elements.Add(Body);
//                            }
//                            #endregion
//                            break;
//                        case "Footer":
//                            #region Add Footer

//                            WCpdfDocumentSectionFooter objFooter = objsection.Footer.FirstOrDefault();
//                            TableHeight = Convert.ToInt32(Math.Round((_TblHeight * double.Parse(objFooter.Height.ToString())) / 100));
//                            MigraDoc.DocumentObjectModel.Tables.Table Footer = new MigraDoc.DocumentObjectModel.Tables.Table();
//                            foreach (WC.PDF.Document.Table tblFooter in objFooter.Table)
//                            {
//                                Footer = objDocParser.GetTable(tblFooter, TableWidth, TableHeight);
//                                cel.Elements.Add(Footer);
//                            }

//                            #endregion
//                            break;
//                    }
//                }
//            }

//            #region create pdf.

//            //const PdfFontEmbedding embedding = PdfFontEmbedding.Always;
//            //const bool unicode = false;
//            //PdfDocumentRenderer pdfrenderer = new PdfDocumentRenderer(unicode, embedding);
//            //pdfrenderer.Document = PDFdocument;
//            //pdfrenderer.RenderDocument();
//            //pdfrenderer.PdfDocument.Save(strPhysicalFilePath);
//            //Process.Start(strPhysicalFilePath);

//            #endregion

//            return true;
//        }
#endregion