using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MigraDoc.DocumentObjectModel;
using WC.PDF.Document;
using WC.PdfDocumentClass;
using MigraDoc.DocumentObjectModel.Tables;
using System.Text.RegularExpressions;
using System.Collections;

namespace WinBase
{
     public class PDFUtility
    {

        internal MigraDoc.DocumentObjectModel.PageFormat GetPageFormateSize(PageSetup pgSetup, string PageSize)
        {
            if (PageSize == "A0")
                pgSetup.PageFormat= MigraDoc.DocumentObjectModel.PageFormat.A0;
            if (PageSize == "A1")
                pgSetup.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.A1;
            if (PageSize == "A2")
                pgSetup.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.A2;
            if (PageSize == "A3")
                pgSetup.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.A3;
            if (PageSize == "A4")
                pgSetup.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.A4;
            if (PageSize == "A5")
                pgSetup.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.A5;
            if (PageSize == "A6")
                pgSetup.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.A6;
            if (PageSize == "B5")
                pgSetup.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.B5;
            if (PageSize == "Ledger")
                pgSetup.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.Ledger;
            if (PageSize == "Legal")
                pgSetup.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.Legal;
            if (PageSize == "Letter")
                pgSetup.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.Letter;
            if (PageSize == "P11x17")
                pgSetup.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.P11x17;
            return pgSetup.PageFormat;

        }

        internal double[] GetHeaderwiths(string strHeader)
        {
            string[] _header = strHeader.Split(',');
            double[] widths = new double[_header.Count()];
            for (int i = 0; i < widths.Count(); i++)
            {
                widths[i] = float.Parse(_header[i].ToString());
            }
            return widths;
        }

        internal bool GetBoolenvalue(string value)
        {
            bool valid = false;
            if (value == "1")
                valid = true;
            return valid;
        }

        internal string GetParagraphStr(string[] _splittxt)
        {
            string str = "";
            for (int i = 0; i < _splittxt.Count(); i++)
            {
                str = str + _splittxt[i] + "\n";
            }
            return str;

        }

        internal ParagraphAlignment GetFormatAlignment(string value)
        {
            if (value == "Center")
                return MigraDoc.DocumentObjectModel.ParagraphAlignment.Center;
            else if (value == "Left")
                return MigraDoc.DocumentObjectModel.ParagraphAlignment.Left;
            else if (value == "Right")
                return MigraDoc.DocumentObjectModel.ParagraphAlignment.Right;
            else
                return MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify;
        }

        internal VerticalAlignment GetVerticalAlignment(string value)
        {
            if (value == "Center")
                return MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
            else if (value == "Bottom")
                return MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom;
            else
                return MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top;
        }

        internal Underline GetUnderlineFromat(string value)
        {
            if (value == "Dash")
                return Underline.Dash;
            else if (value == "DotDash")
                return Underline.DotDash;
            else if (value == "DotDotDash")
                return Underline.DotDotDash;
            else if (value == "Dotted")
                return Underline.Dotted;
            else if (value == "Single")
                return Underline.Single;
            else if (value == "Words")
                return Underline.Words;
            else
                return Underline.None;
           
        }

        internal Color GetColors(string value)
        {

            if (value == "Red")
                return Colors.Red;
            else if (value == "Gainsboro")
                return Colors.Gainsboro;
            else if (value == "Green")
                return Colors.Green;
            else if (value == "Blue")
                return Colors.Blue;
            else if (value == "Yellow")
                return Colors.Yellow;
            else if (value == "OrangeRed")
                return Colors.OrangeRed;
            else if (value == "Orange")
                return Colors.Orange;
            else if (value == "White")
                return Colors.White;
            else if (value == "Brown")
                return Colors.Brown;
            else if (value == "Pink")
                return Colors.Pink;
            else
                return Colors.Black;
                
        }

        internal string GetLineStr(double Linelength)
        {
            string LineStr = "=";
            for (int j = 0; j < Linelength; j++)
            {
                if (j == Linelength)
                    LineStr = LineStr + " ";
                else
                    LineStr = LineStr + "=";


            }
            return LineStr;
        }
    }



    internal class CCECustomeReportManager
    {
        public Dictionary<string, string> dataDictionary { get; set; }
        public Dictionary<string, DataSet> DatasetDictionary { get; set; }
        public Dictionary<string, string> imageUrlDictionary { get; set; }
        public string CustomeType = string.Empty;
        public WCpdfDocument DocDefinition { get; set; }
        private PDFUtility obj_pdfutility = null;
        public bool CreateDocument(ref Document PDFdocument, PageSetup pgSetup)
        {
            bool blReturn = true;
            switch (CustomeType)
            {
                    //Carmel
                case "1":
                    blReturn = CreateDocumentCarmel(ref PDFdocument, pgSetup);
                    break;
            }

            return blReturn;
        }

        #region Carmel Customsation

         public bool CreateDocumentCarmel(ref Document PDFdocument, PageSetup pgSetup)
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


                //DocumentParser objDocParser = new DocumentParser(pgSetup, DocDefinition, dataDictionary, imageUrlDictionary, DatasetDictionary);
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
                                Header = GetTable(tblHeader, TableWidth, TableHeight);
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
                                Body = GetTable(tblBody, TableWidth, TableHeight);
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
                                Footer = GetTable(tblFooter, TableWidth, TableHeight);
                                cel.Elements.Add(Footer);
                            }

                            #endregion
                            break;
                    }
                }
            }

            return true;
        }

        

        private DataSet IsInvertedDataSet(DataSet _Dataset)
        {
            DataSet _returnds = new DataSet();
            DataTable table = new DataTable("cceresult");
            //add column
            foreach (DataRow dr in _Dataset.Tables[0].Rows)
            {
                table.Columns.Add(dr[1].ToString());
            }
            //add row
            int i = 2;
            foreach (DataColumn dc in _Dataset.Tables[0].Columns)
            {
                DataRow row = table.Rows.Add();
                int j = 0;
                foreach (DataRow dr in _Dataset.Tables[0].Rows)
                {
                    row[j] = dr[i].ToString();
                    j++;
                }
                i++;
                if (_Dataset.Tables[0].Columns.Count == i)
                    break;
            }
            _returnds.Tables.Add(table);
            return _returnds;
        }

        private DataSet ConvertIsInverted(string IsInverted, DataSet _Dataset)
        {
            DataSet IsInvertedds = new DataSet("TableIsInverted");
            if (IsInverted == "True")
            {
                foreach (DataColumn dr in _Dataset.Tables[0].Columns)
                {

                }
            }
            else
                IsInvertedds = _Dataset;
            return IsInvertedds;
        }

        internal double[] GetHeaderwiths(string strHeader)
        {
            string[] _header = strHeader.Split(',');
            double[] widths = new double[_header.Count()];
            for (int i = 0; i < widths.Count(); i++)
            {
                widths[i] = float.Parse(_header[i].ToString());
            }
            return widths;
        }

        internal ParagraphAlignment GetFormatAlignment(string value)
        {
            if (value == "Center")
                return MigraDoc.DocumentObjectModel.ParagraphAlignment.Center;
            else if (value == "Left")
                return MigraDoc.DocumentObjectModel.ParagraphAlignment.Left;
            else if (value == "Right")
                return MigraDoc.DocumentObjectModel.ParagraphAlignment.Right;
            else
                return MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify;
        }

        
   

        internal MigraDoc.DocumentObjectModel.Tables.Table GetTable(WC.PDF.Document.Table _Table, double _Tblwidth, double _TblHeight)
        {
            this.obj_pdfutility = new PDFUtility();
            double num = 0.0;
            double num2 = 0.0;
            num = _Tblwidth - (double.Parse(_Table.LeftPadding.ToString()) + double.Parse(_Table.RightPadding.ToString()));
            num2 = _TblHeight * double.Parse(_Table.TableHeight.ToString()) / 100.0;
            num2 -= double.Parse(_Table.TopPadding.ToString()) + double.Parse(_Table.BottomPadding.ToString());
            string strHeader = _Table.HeaderWidths.ToString();
            double[] headerwiths = this.obj_pdfutility.GetHeaderwiths(strHeader);
            int num3 = headerwiths.Count<double>();
            MigraDoc.DocumentObjectModel.Tables.Table table = new MigraDoc.DocumentObjectModel.Tables.Table();
            Row row = null;
            table.RightPadding = double.Parse(_Table.RightPadding.ToString());
            table.LeftPadding = double.Parse(_Table.LeftPadding.ToString());
            table.TopPadding = double.Parse(_Table.TopPadding.ToString());
            table.BottomPadding = double.Parse(_Table.BottomPadding.ToString());
            table.Shading.Color = this.obj_pdfutility.GetColors(_Table.ShadingColor.ToString());
            table.Format.Alignment = ParagraphAlignment.Center;
            num = Math.Round(num, 2);
            num2 = Math.Round(num2, 2);
            for (int i = 0; i < num3; i++)
            {
                double num4 = num * headerwiths[i] / 100.0;
                num4 = Math.Round(num4, 2);
                table.AddColumn(num4);
            }
            DataSet dataSet = null;
            string text = "";
            string name = "Calibri";
            string[] array = new string[4];
            string a;
            if ((a = _Table.Type.ToString()) != null)
            {
                if (!(a == "DynamicDivieded"))
                {
                    if (!(a == "Dynamic"))
                    {
                        if (!(a == "Static"))
                        {
                            return table;
                        }
                        goto IL_943;
                    }
                }
                else
                {
                    dataSet = this.GetReplaceDataset(_Table.Id.ToString(), true);
                    IEnumerator enumerator = dataSet.Tables[0].Rows.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            DataRow dataRow = (DataRow)enumerator.Current;
                            row = table.AddRow();
                            row.Format.Alignment = ParagraphAlignment.Center;
                            double num5 = Math.Round(num2 / (double)dataSet.Tables[0].Rows.Count, 2);
                            row.Height = num5;
                            for (int j = 0; j < num3; j++)
                            {
                                TableColumnConfiguration tableColumnConfiguration = _Table.ColumnConfiguration[j];
                                try
                                {
                                    text = dataRow[j].ToString();
                                }
                                catch
                                {
                                    text = "";
                                }
                                row.Cells[j].AddParagraph(text);
                                int value = int.Parse(tableColumnConfiguration.FontSize.ToString());
                                row.Cells[j].Format.Font = new Font(name, value);
                                row.Cells[j].Format.Alignment = this.obj_pdfutility.GetFormatAlignment(tableColumnConfiguration.FormatAlignment.ToString());
                                row.Cells[j].VerticalAlignment = this.obj_pdfutility.GetVerticalAlignment(tableColumnConfiguration.VerticalAlignment.ToString());
                                row.Cells[j].Borders.Width = double.Parse(tableColumnConfiguration.BorderWidth.ToString());
                                row.Cells[j].Format.Font.Underline = this.obj_pdfutility.GetUnderlineFromat(tableColumnConfiguration.Underline.ToString());
                                row.Cells[j].Format.Font.Color = this.obj_pdfutility.GetColors(tableColumnConfiguration.FontColor.ToString());
                                row.Cells[j].Borders.Color = this.obj_pdfutility.GetColors(tableColumnConfiguration.BordersColor.ToString());
                                row.Cells[j].Format.Shading.Color = this.obj_pdfutility.GetColors(tableColumnConfiguration.ShadingColor.ToString());
                            }
                        }
                        return table;
                    }
                    finally
                    {
                        IDisposable disposable = enumerator as IDisposable;
                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }
                    }
                }
                dataSet = this.GetReplaceDataset(_Table.Id.ToString(), false);
                int num6 = 1;
                IEnumerator enumerator2 = dataSet.Tables[0].Rows.GetEnumerator();
                try
                {
                    while (enumerator2.MoveNext())
                    {
                        DataRow dataRow2 = (DataRow)enumerator2.Current;
                        row = table.AddRow();
                        row.Format.Alignment = ParagraphAlignment.Center;
                        double num7 = Math.Round(num2 / (double)dataSet.Tables[0].Rows.Count, 2);
                        row.Height = num7;
                        for (int k = 0; k < num3; k++)
                        {
                            TableColumnConfiguration tableColumnConfiguration2 = _Table.ColumnConfiguration[k];
                            if (tableColumnConfiguration2.ColumnName == "@@Result@@")
                            {
                                Cell cell = row.Cells[k];
                                double innerTblWidth = Math.Round(num * headerwiths[k] / 100.0, 2);
                                WC.PDF.Document.Table innerTable = this.GetInnerTable(tableColumnConfiguration2.ColumnName.ToString());
                                MigraDoc.DocumentObjectModel.Tables.Table dynamicTable = this.GetDynamicTable(innerTable, innerTblWidth, num7, dataRow2);
                                cell.Elements.Add(dynamicTable);
                                row.Cells[k].Format.Alignment = ParagraphAlignment.Center;
                                row.Cells[k].VerticalAlignment = VerticalAlignment.Bottom;
                            }
                            else
                            {
                                try
                                {
                                    text = dataRow2[tableColumnConfiguration2.ColumnName].ToString();
                                    string[] array2 = Regex.Split(text, "/nbsp");
                                    text = this.obj_pdfutility.GetParagraphStr(array2);
                                }
                                catch
                                {
                                    text = " ";
                                }
                                row.Cells[k].AddParagraph(text);
                                int value = int.Parse(tableColumnConfiguration2.FontSize.ToString());
                                row.Cells[k].Format.Font = new Font(name, value);
                                row.Cells[k].Format.Alignment = this.obj_pdfutility.GetFormatAlignment(tableColumnConfiguration2.FormatAlignment.ToString());
                                row.Cells[k].VerticalAlignment = this.obj_pdfutility.GetVerticalAlignment(tableColumnConfiguration2.VerticalAlignment.ToString());
                                array = tableColumnConfiguration2.BorderWidth.ToString().Split(new char[]
								{
									','
								});
                                if (array.Count<string>() == 1)
                                {
                                    row.Cells[k].Borders.Width = double.Parse(array[0].ToString());
                                }
                                else
                                {
                                    row.Cells[k].Borders.Right.Width = double.Parse(array[0].ToString());
                                    row.Cells[k].Borders.Left.Width = double.Parse(array[1].ToString());
                                    row.Cells[k].Borders.Top.Width = double.Parse(array[2].ToString());
                                    row.Cells[k].Borders.Bottom.Width = double.Parse(array[3].ToString());
                                    if (num6 == dataSet.Tables[0].Rows.Count)
                                    {
                                        row.Cells[k].Borders.Bottom.Width = 0;
                                    }
                                }
                                row.Cells[k].Format.Font.Underline = this.obj_pdfutility.GetUnderlineFromat(tableColumnConfiguration2.Underline.ToString());
                                row.Cells[k].Format.Font.Color = this.obj_pdfutility.GetColors(tableColumnConfiguration2.FontColor.ToString());
                                row.Cells[k].Borders.Color = this.obj_pdfutility.GetColors(tableColumnConfiguration2.BordersColor.ToString());
                                row.Cells[k].Format.Shading.Color = this.obj_pdfutility.GetColors(tableColumnConfiguration2.ShadingColor.ToString());
                            }
                        }
                        num6++;
                    }
                    return table;
                }
                finally
                {
                    IDisposable disposable2 = enumerator2 as IDisposable;
                    if (disposable2 != null)
                    {
                        disposable2.Dispose();
                    }
                }
            IL_943:
                TableTR[] tr = _Table.Tr;
                for (int l = 0; l < tr.Length; l++)
                {
                    TableTR tableTR = tr[l];
                    row = table.AddRow();
                    row.TopPadding = double.Parse(tableTR.RowTopPadding.ToString());
                    row.BottomPadding = double.Parse(tableTR.RowBottomPadding.ToString());
                    array = tableTR.BorderWidth.ToString().Split(new char[]
					{
						','
					});
                    if (array.Count<string>() == 1)
                    {
                        row.Borders.Width = double.Parse(array[0].ToString());
                    }
                    else
                    {
                        row.Borders.Right.Width = double.Parse(array[0].ToString());
                        row.Borders.Left.Width = double.Parse(array[1].ToString());
                        row.Borders.Top.Width = double.Parse(array[2].ToString());
                        row.Borders.Bottom.Width = double.Parse(array[3].ToString());
                    }
                    row.Format.Alignment = this.obj_pdfutility.GetFormatAlignment(tableTR.Alignment.ToString());
                    double num8 = Math.Round(num2 * double.Parse(tableTR.RowHeight.ToString()) / 100.0, 2);
                    num8 -= double.Parse(tableTR.RowTopPadding.ToString()) + double.Parse(tableTR.RowBottomPadding.ToString());
                    num8 = Math.Round(num8, 2);
                    row.Height = num8;
                    int num9 = 0;
                    TableTRColumn[] column = tableTR.Column;
                    for (int m = 0; m < column.Length; m++)
                    {
                        TableTRColumn tableTRColumn = column[m];
                        if (tableTRColumn.Type == "SubTable")
                        {
                            Cell cell = row.Cells[num9];
                            double tblwidth;
                            if (tableTRColumn.MergeRight != "0")
                            {
                                int num10 = int.Parse(tableTRColumn.MergeRight.ToString());
                                double num11 = 0.0;
                                for (int n = num9; n <= num10; n++)
                                {
                                    num11 += headerwiths[n];
                                }
                                tblwidth = Math.Round(num * num11 / 100.0, 2);
                            }
                            else
                            {
                                tblwidth = Math.Round(num * headerwiths[num9] / 100.0, 2);
                            }
                            TableTRColumnSubTable[] subTable = tableTRColumn.SubTable;
                            for (int num12 = 0; num12 < subTable.Length; num12++)
                            {
                                TableTRColumnSubTable tableTRColumnSubTable = subTable[num12];
                                WC.PDF.Document.Table innerTable2 = this.GetInnerTable(tableTRColumnSubTable.ID.ToString());
                                MigraDoc.DocumentObjectModel.Tables.Table table2 = this.GetTable(innerTable2, tblwidth, num8);
                                cell.Elements.Add(table2);
                                row.Cells[num9].Format.Alignment = ParagraphAlignment.Center;
                                row.Cells[num9].VerticalAlignment = VerticalAlignment.Bottom;
                            }
                        }
                        else
                        {
                            if (tableTRColumn.Type == "Image")
                            {
                                string replaceImage = this.GetReplaceImage(tableTRColumn.Url.ToString());
                                row.Cells[num9].AddImage(replaceImage);
                                row.Cells[num9].Format.Alignment = this.obj_pdfutility.GetFormatAlignment(tableTRColumn.FormatAlignment.ToString());
                                row.Cells[num9].VerticalAlignment = this.obj_pdfutility.GetVerticalAlignment(tableTRColumn.VerticalAlignment.ToString());
                            }
                            else
                            {
                                if (tableTRColumn.Type == "nbsp")
                                {
                                    row.Cells[num9].AddParagraph("");
                                }
                                else
                                {
                                    int value = int.Parse(tableTRColumn.FontSize.ToString());
                                    name = tableTRColumn.FontName.ToString();
                                    row.Cells[num9].MergeDown = int.Parse(tableTRColumn.MergeDown.ToString());
                                    row.Cells[num9].MergeRight = int.Parse(tableTRColumn.MergeRight.ToString());
                                    row.Cells[num9].Format.Font.Bold = this.obj_pdfutility.GetBoolenvalue(tableTRColumn.NeedFontBoldSyle.ToString());
                                    row.Cells[num9].Format.Font = new Font(name, value);
                                    row.Cells[num9].Format.Alignment = this.obj_pdfutility.GetFormatAlignment(tableTRColumn.FormatAlignment.ToString());
                                    row.Cells[num9].VerticalAlignment = this.obj_pdfutility.GetVerticalAlignment(tableTRColumn.VerticalAlignment.ToString());
                                    array = tableTRColumn.BorderWidth.ToString().Split(new char[]
									{
										','
									});
                                    if (array.Count<string>() == 1)
                                    {
                                        row.Cells[num9].Borders.Width = double.Parse(array[0].ToString());
                                    }
                                    else
                                    {
                                        row.Cells[num9].Borders.Right.Width = double.Parse(array[0].ToString());
                                        row.Cells[num9].Borders.Left.Width = double.Parse(array[1].ToString());
                                        row.Cells[num9].Borders.Top.Width = double.Parse(array[2].ToString());
                                        row.Cells[num9].Borders.Bottom.Width = double.Parse(array[3].ToString());
                                    }
                                    row.Cells[num9].Format.Font.Italic = this.obj_pdfutility.GetBoolenvalue(tableTRColumn.NeedFontItalicSyle.ToString());
                                    row.Cells[num9].Format.Font.Underline = this.obj_pdfutility.GetUnderlineFromat(tableTRColumn.Underline.ToString());
                                    row.Cells[num9].Format.Font.Color = this.obj_pdfutility.GetColors(tableTRColumn.FontColor.ToString());
                                    row.Cells[num9].Borders.Color = this.obj_pdfutility.GetColors(tableTRColumn.BordersColor.ToString());
                                    row.Cells[num9].Format.Shading.Color = this.obj_pdfutility.GetColors(tableTRColumn.ShadingColor.ToString());
                                    text = "";
                                    string input = tableTRColumn.Value.ToString();
                                    string[] array3 = Regex.Split(input, "/split/");
                                    for (int num13 = 0; num13 < array3.Count<string>(); num13++)
                                    {
                                        string text2 = this.GetReplacevalue(array3[num13].ToString());
                                        string[] array2 = Regex.Split(text2, "/nbsp");
                                        if (array2.Count<string>() > 1)
                                        {
                                            text2 = this.obj_pdfutility.GetParagraphStr(array2);
                                        }
                                        array3[num13] = text2;
                                    }
                                    for (int num14 = 0; num14 < array3.Count<string>(); num14++)
                                    {
                                        if (num14 == 0)
                                        {
                                            text = array3[num14];
                                        }
                                        else
                                        {
                                            text += array3[num14];
                                        }
                                    }
                                    if (text == "_")
                                    {
                                        text = this.obj_pdfutility.GetLineStr(Convert.ToDouble(tableTRColumn.LineLength.ToString()));
                                    }
                                    row.Cells[num9].AddParagraph(text);
                                }
                            }
                        }
                        num9++;
                    }
                }
            }
            return table;
        }
        private MigraDoc.DocumentObjectModel.Tables.Table GetDynamicTable(WC.PDF.Document.Table InnerTable, double innerTblWidth, double RowHeight, DataRow dr)
        {
            MigraDoc.DocumentObjectModel.Tables.Table table = new MigraDoc.DocumentObjectModel.Tables.Table();
            new Cell();
            string strHeader = InnerTable.HeaderWidths.ToString();
            double[] headerwiths = this.obj_pdfutility.GetHeaderwiths(strHeader);
            int num = headerwiths.Count<double>();
            for (int i = 0; i < num; i++)
            {
                double num2 = innerTblWidth * headerwiths[i] / 100.0;
                num2 = Math.Round(num2, 2);
                table.AddColumn(num2);
            }
            Row row = table.AddRow();
            row.Height = RowHeight;
            int num3 = 2;
            int num4 = 0;
            TableColumnConfiguration[] columnConfiguration = InnerTable.ColumnConfiguration;
            for (int j = 0; j < columnConfiguration.Length; j++)
            {
                TableColumnConfiguration tableColumnConfiguration = columnConfiguration[j];
                row.Cells[num4].Format.Font = new Font("Calibri", 8);
                row.Cells[num4].Format.Alignment = this.obj_pdfutility.GetFormatAlignment(tableColumnConfiguration.FormatAlignment.ToString());
                row.Cells[num4].VerticalAlignment = this.obj_pdfutility.GetVerticalAlignment(tableColumnConfiguration.VerticalAlignment.ToString());
                row.Cells[num4].Borders.Width = double.Parse(tableColumnConfiguration.BorderWidth.ToString());
                row.Cells[num4].Format.Font.Underline = this.obj_pdfutility.GetUnderlineFromat(tableColumnConfiguration.Underline.ToString());
                row.Cells[num4].Format.Font.Color = this.obj_pdfutility.GetColors(tableColumnConfiguration.FontColor.ToString());
                row.Cells[num4].Borders.Color = this.obj_pdfutility.GetColors(tableColumnConfiguration.BordersColor.ToString());
                row.Cells[num4].Format.Shading.Color = this.obj_pdfutility.GetColors(tableColumnConfiguration.ShadingColor.ToString());
                row.Cells[num4].AddParagraph(dr[num3].ToString());
                num4++;
                num3++;
            }
            table.Borders.Visible = true;
            return table;
        }
        private WC.PDF.Document.Table GetInnerTable(string Tablename)
        {
            WC.PDF.Document.Table result = null;
            WCpdfDocumentTableList wCpdfDocumentTableList = this.DocDefinition.tableList.FirstOrDefault<WCpdfDocumentTableList>();
            WC.PDF.Document.Table[] table = wCpdfDocumentTableList.Table;
            for (int i = 0; i < table.Length; i++)
            {
                WC.PDF.Document.Table table2 = table[i];
                if (table2.Id == Tablename)
                {
                    result = table2;
                }
            }
            return result;
        }
        private string GetReplaceImage(string Keyword)
        {
            string result;
            if (this.imageUrlDictionary.ContainsKey(Keyword))
            {
                result = this.imageUrlDictionary[Keyword];
            }
            else
            {
                result = Keyword;
            }
            return result;
        }
        private string GetReplacevalue(string Keyword)
        {
            string result;
            if (this.dataDictionary.ContainsKey(Keyword))
            {
                result = this.dataDictionary[Keyword];
            }
            else
            {
                result = Keyword;
            }
            return result;
        }
        private DataSet GetReplaceDataset(string Keyword, bool ColumDivided)
        {
            DataSet dataSet = new DataSet();
            if (this.DatasetDictionary.ContainsKey(Keyword))
            {
                dataSet = this.DatasetDictionary[Keyword];
            }
            else
            {
                DataTable dataTable = new DataTable("Table");
                dataTable.Columns.Add("Columns", typeof(string));
                dataTable.Rows.Add(new object[]
				{
					"NoDataFound"
				});
                dataSet.Tables.Add(dataTable);
            }
            if (ColumDivided)//ColumDivided made false
            {
                DataTable dataTable2 = new DataTable("Table1");
                for (int i = 0; i < 6; i++)
                {
                    dataTable2.Columns.Add(i.ToString());
                }
                dataTable2.Rows.Add(new object[0]);
                bool flag = true;
                int num = 0;
                for (int j = 0; j < dataSet.Tables[0].Rows.Count; j++)
                {
                    if (flag)
                    {
                        num = 0;
                        dataTable2.Rows.Add(new object[0]);
                    }
                    for (int k = 0; k < dataSet.Tables[0].Columns.Count - 1; k++)
                    {
                        if (k == 1)
                        {
                            num++;
                            dataTable2.Rows[dataTable2.Rows.Count - 1][num] = dataSet.Tables[0].Rows[j][k].ToString();
                        }
                        else
                        {
                            dataTable2.Rows[dataTable2.Rows.Count - 1][num] = dataSet.Tables[0].Rows[j][k].ToString();
                            num++;
                            dataTable2.Rows[dataTable2.Rows.Count - 1][num] = ":";
                        }
                    }
                    flag = (dataTable2.Columns.Count - 1 == num);
                    num++;
                }
                dataSet.Tables.Clear();
                dataSet.Tables.Add(dataTable2);
            }
            return dataSet;
        }

        #endregion


        #region Custome Report Common
      

        #endregion
    }
}
