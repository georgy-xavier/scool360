using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using WC.PDF.Document;
using System.Text.RegularExpressions;
using PdfSharp.Drawing;
using MigraDoc.DocumentObjectModel.Shapes;

 namespace WC.PdfDocumentClass
{
     internal class DocumentParser
    {
         private PageSetup pgSetup;
         private WCpdfDocument DocDefinition;
         private Dictionary<string, string> dataDictionary;
         public Dictionary<string, string> imageUrlDictionary;
         public Dictionary<string, DataSet> DatasetDictionary;
         private PDFUtility obj_pdfutility = null;

         public DocumentParser(PageSetup _PageSetup, WCpdfDocument _DocDefinition, Dictionary<string, string> _dataDictionary, Dictionary<string, string> _imageUrlDictionary, Dictionary<string, DataSet> _DatasetDictionary)
         {
             pgSetup = _PageSetup;
             DocDefinition = _DocDefinition;
             dataDictionary = _dataDictionary;
             imageUrlDictionary = _imageUrlDictionary;
             DatasetDictionary = _DatasetDictionary;
         }

         internal MigraDoc.DocumentObjectModel.Tables.Table GetTable(WC.PDF.Document.Table _Table, double _Tblwidth, double _TblHeight)
         {
             obj_pdfutility = new PDFUtility();

             //Table objects 
             MigraDoc.DocumentObjectModel.Tables.Table returnTable = null;
             MigraDoc.DocumentObjectModel.Tables.Row returnRow = null;
             MigraDoc.DocumentObjectModel.Tables.Cell returnCell = null;

             // table measurements
             double TableWidth = 0, TableHeight = 0;
             TableWidth = _Tblwidth - (double.Parse(_Table.LeftPadding.ToString()) + double.Parse(_Table.RightPadding.ToString()));
             TableHeight = (_TblHeight * double.Parse(_Table.TableHeight.ToString()) / 100);
             TableHeight = TableHeight - (double.Parse(_Table.TopPadding.ToString()) + double.Parse(_Table.BottomPadding.ToString()));
             double[] Columwidths = null;
             string strHeader = "";
             double ColumWith = 0;

             //created table
             returnTable = new MigraDoc.DocumentObjectModel.Tables.Table();

             //table paddings
             returnTable.RightPadding = double.Parse(_Table.RightPadding.ToString());
             returnTable.LeftPadding = double.Parse(_Table.LeftPadding.ToString());
             returnTable.TopPadding = double.Parse(_Table.TopPadding.ToString());
             returnTable.BottomPadding = double.Parse(_Table.BottomPadding.ToString());

             //Table shading color
             returnTable.Shading.Color = obj_pdfutility.GetColors(_Table.ShadingColor.ToString());

             returnTable.Format.Alignment = MigraDoc.DocumentObjectModel.ParagraphAlignment.Center;

             //Table height & width
             TableWidth = Math.Round((double)TableWidth, 2);
             TableHeight = Math.Round((double)TableHeight, 2);

             if (_Table.IsInverted.ToString() == "True")
             {
                 #region Auto Generate Table

                 DataSet _Dataset = null;
                 _Dataset = GetReplaceDataset(_Table.Id.ToString(), false);
                 _Dataset = IsInvertedDataSet(_Dataset);


                 //calculating column width
                 ColumWith = TableWidth / _Dataset.Tables[0].Columns.Count;

                 //calculating row height
                 strHeader = _Table.HeaderWidths.ToString();
                 Columwidths = obj_pdfutility.GetHeaderwiths(strHeader);

                 //creating column width
                 foreach (DataColumn dc in _Dataset.Tables[0].Columns)
                     returnTable.AddColumn(ColumWith);

                 int i = 0;
                 double RowHeight = 0.0;
                 if (_Table.Type.ToString() == "AutoGenerateTableHeaderSection")
                 {
                     //creating header row
                     returnRow = returnTable.AddRow();

                     //Adding header column name

                     foreach (DataColumn dc in _Dataset.Tables[0].Columns)
                     {
                         returnRow.Cells[i].AddParagraph(dc.ColumnName.ToString());

                         #region row cell configuration
                         returnRow.Cells[i].Borders.Width = double.Parse(_Table.BorderWidth.ToString());
                         returnRow.Cells[i].Format.Alignment = ParagraphAlignment.Center;
                         returnRow.Cells[i].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
                         returnRow.Cells[i].Format.Shading.Color = obj_pdfutility.GetColors(_Table.ShadingColor.ToString());
                         returnRow.Cells[i].Format.Font.Bold = true;
                         returnRow.Cells[i].Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 12);

                         #endregion

                         i++;
                     }

                     #region Header settings
                     RowHeight = Math.Round((double)(TableHeight * (double.Parse(Columwidths[0].ToString())) / 100), 2);
                     RowHeight = Math.Round((double)RowHeight, 2);
                     returnRow.Height = RowHeight;
                     #endregion
                 }
                 else
                 {

                     //insert records row by row
                     i = 0;
                     foreach (DataRow dr in _Dataset.Tables[0].Rows)
                     {
                         returnRow = returnTable.AddRow();
                         for (int j = 0; j < _Dataset.Tables[0].Columns.Count; j++)
                         {
                             string Mark = dr[j].ToString();
                             returnRow.Cells[j].AddParagraph(Mark);

                             #region row cell configuration
                             returnRow.Cells[j].Borders.Width = double.Parse(_Table.BorderWidth.ToString());
                             returnRow.Cells[j].Format.Alignment = ParagraphAlignment.Center;
                             returnRow.Cells[j].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
                             returnRow.Cells[j].Format.Shading.Color = obj_pdfutility.GetColors("White");
                             returnRow.Cells[j].Format.Font.Bold = false;
                             returnRow.Cells[j].Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 8);
                             #endregion

                         }

                         #region row settings
                         RowHeight = Math.Round((double)(TableHeight * (double.Parse(Columwidths[i].ToString())) / 100), 2);
                         RowHeight = Math.Round((double)RowHeight, 2);
                         returnRow.Height = RowHeight;
                         returnRow.Format.Shading.Color = obj_pdfutility.GetColors("White");
                         #endregion

                         i++;
                     }
                 }

                 #endregion
             }
             else
             {
                 #region Dynamic Generate Table

                 strHeader = _Table.HeaderWidths.ToString();
                 Columwidths = obj_pdfutility.GetHeaderwiths(strHeader);
                 int ColumnCount = Columwidths.Count();

                 #region Add Column


                 for (int c = 0; c < ColumnCount; c++)
                 {
                     ColumWith = ((TableWidth * Columwidths[c]) / 100);
                     ColumWith = Math.Round((double)ColumWith, 2);
                     returnTable.AddColumn(ColumWith);

                 }

                 #endregion

                 DataSet _Dataset = null;
                 string _Message = "";
                 string Textkeywords = "";
                 string[] Split_Textkeywords = null;
                 string Text = "";
                 string[] _splittxt = null;
                 int fontsize = 8;
                 string fontname = "Calibri";
                 string[] _Bwidht = new string[4];
                 switch (_Table.Type.ToString())
                 {
                     #region DynamicDivieded

                     case "DynamicDivieded":
                         _Dataset = GetReplaceDataset(_Table.Id.ToString(), true);
                         foreach (DataRow dr in _Dataset.Tables[0].Rows)
                         {
                             returnRow = returnTable.AddRow();
                             returnRow.Format.Alignment = MigraDoc.DocumentObjectModel.ParagraphAlignment.Center;
                             double RowHeight = Math.Round((double)(TableHeight / _Dataset.Tables[0].Rows.Count), 2);
                             returnRow.Height = RowHeight;
                             for (int i = 0; i < ColumnCount; i++)
                             {
                                 TableColumnConfiguration confg = _Table.ColumnConfiguration[i];
                                 try
                                 {
                                     _Message = dr[i].ToString();
                                 }
                                 catch
                                 {
                                     _Message = "";
                                 }
                                 returnRow.Cells[i].AddParagraph(_Message);

                                 fontsize = int.Parse(confg.FontSize.ToString());
                                 returnRow.Cells[i].Format.Font = new MigraDoc.DocumentObjectModel.Font(fontname, fontsize);
                                 returnRow.Cells[i].Format.Alignment = obj_pdfutility.GetFormatAlignment(confg.FormatAlignment.ToString());
                                 returnRow.Cells[i].VerticalAlignment = obj_pdfutility.GetVerticalAlignment(confg.VerticalAlignment.ToString());
                                 returnRow.Cells[i].Borders.Width = double.Parse(confg.BorderWidth.ToString());
                                 returnRow.Cells[i].Format.Font.Underline = obj_pdfutility.GetUnderlineFromat(confg.Underline.ToString());
                                 returnRow.Cells[i].Format.Font.Color = obj_pdfutility.GetColors(confg.FontColor.ToString());
                                 returnRow.Cells[i].Borders.Color = obj_pdfutility.GetColors(confg.BordersColor.ToString());
                                 returnRow.Cells[i].Format.Shading.Color = obj_pdfutility.GetColors(confg.ShadingColor.ToString());
                             }
                         }

                         break;

                     #endregion

                     #region Dynamic

                     case "Dynamic":
                         _Dataset = GetReplaceDataset(_Table.Id.ToString(), false);
                         _Dataset = ConvertIsInverted(_Table.IsInverted.ToString(), _Dataset);
                         int _rowcount = 1;
                         foreach (DataRow dr in _Dataset.Tables[0].Rows)
                         {
                             returnRow = returnTable.AddRow();
                             returnRow.Format.Alignment = MigraDoc.DocumentObjectModel.ParagraphAlignment.Center;
                             double RowHeight = Math.Round((double)(TableHeight / _Dataset.Tables[0].Rows.Count), 2);
                             returnRow.Height = RowHeight;

                             for (int i = 0; i < ColumnCount; i++)
                             {
                                 TableColumnConfiguration confg = _Table.ColumnConfiguration[i];
                                 if (confg.ColumnName == "@@Result@@")
                                 {
                                     #region Dynamic subtable
                                     returnCell = returnRow.Cells[i];
                                     double innerTblWidth = Math.Round((double)((TableWidth * Columwidths[i]) / 100), 2);
                                     WC.PDF.Document.Table InnerTable = GetInnerTable(confg.ColumnName.ToString());
                                     MigraDoc.DocumentObjectModel.Tables.Table inntb = GetDynamicTable(InnerTable, innerTblWidth, RowHeight, dr);
                                     returnCell.Elements.Add(inntb);
                                     returnRow.Cells[i].Format.Alignment = ParagraphAlignment.Center;
                                     returnRow.Cells[i].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom;
                                     #endregion

                                 }
                                 else
                                 {
                                     try
                                     {
                                         _Message = dr[confg.ColumnName].ToString();
                                         _splittxt = Regex.Split(_Message, "/nbsp");
                                         _Message = obj_pdfutility.GetParagraphStr(_splittxt);


                                     }
                                     catch
                                     {
                                         _Message = " ";
                                     }
                                     returnRow.Cells[i].AddParagraph(_Message);

                                     fontsize = int.Parse(confg.FontSize.ToString());
                                     returnRow.Cells[i].Format.Font = new MigraDoc.DocumentObjectModel.Font(fontname, fontsize);
                                     returnRow.Cells[i].Format.Alignment = obj_pdfutility.GetFormatAlignment(confg.FormatAlignment.ToString());
                                     returnRow.Cells[i].VerticalAlignment = obj_pdfutility.GetVerticalAlignment(confg.VerticalAlignment.ToString());


                                     #region Border Width
                                     _Bwidht = confg.BorderWidth.ToString().Split(',');
                                     if (_Bwidht.Count() == 1)
                                     {
                                         returnRow.Cells[i].Borders.Width = double.Parse(_Bwidht[0].ToString());
                                     }
                                     else
                                     {
                                         returnRow.Cells[i].Borders.Right.Width = double.Parse(_Bwidht[0].ToString());
                                         returnRow.Cells[i].Borders.Left.Width = double.Parse(_Bwidht[1].ToString());
                                         returnRow.Cells[i].Borders.Top.Width = double.Parse(_Bwidht[2].ToString());
                                         returnRow.Cells[i].Borders.Bottom.Width = double.Parse(_Bwidht[3].ToString());
                                         if (_rowcount == _Dataset.Tables[0].Rows.Count)
                                             returnRow.Cells[i].Borders.Bottom.Width = 0;
                                     }
                                     #endregion



                                     returnRow.Cells[i].Format.Font.Underline = obj_pdfutility.GetUnderlineFromat(confg.Underline.ToString());
                                     returnRow.Cells[i].Format.Font.Color = obj_pdfutility.GetColors(confg.FontColor.ToString());
                                     returnRow.Cells[i].Borders.Color = obj_pdfutility.GetColors(confg.BordersColor.ToString());
                                     returnRow.Cells[i].Format.Shading.Color = obj_pdfutility.GetColors(confg.ShadingColor.ToString());

                                 }

                             }
                             _rowcount++;

                         }
                         //returnRow = returnTable.AddRow();
                         break;

                     #endregion

                     #region Static
                     case "Static":
                         int rowcellpoint = 0;
                         foreach (WC.PDF.Document.TableTR Tr in _Table.Tr)
                         {
                             returnRow = returnTable.AddRow();
                             returnRow.TopPadding = double.Parse(Tr.RowTopPadding.ToString());
                             returnRow.BottomPadding = double.Parse(Tr.RowBottomPadding.ToString());

                             #region Row width desing
                             _Bwidht = Tr.BorderWidth.ToString().Split(',');
                             if (_Bwidht.Count() == 1)
                                 returnRow.Borders.Width = double.Parse(_Bwidht[0].ToString());
                             else
                             {
                                 returnRow.Borders.Right.Width = double.Parse(_Bwidht[0].ToString());
                                 returnRow.Borders.Left.Width = double.Parse(_Bwidht[1].ToString());
                                 returnRow.Borders.Top.Width = double.Parse(_Bwidht[2].ToString());
                                 returnRow.Borders.Bottom.Width = double.Parse(_Bwidht[3].ToString());
                             }
                             #endregion

                             returnRow.Format.Alignment = obj_pdfutility.GetFormatAlignment(Tr.Alignment.ToString());
                             double RowHeight = Math.Round((double)(TableHeight * (double.Parse(Tr.RowHeight.ToString())) / 100), 2);
                             RowHeight = RowHeight - (double.Parse(Tr.RowTopPadding.ToString()) + double.Parse(Tr.RowBottomPadding.ToString()));
                             RowHeight = Math.Round((double)RowHeight, 2);
                             returnRow.Height = RowHeight;

                             rowcellpoint = 0;
                             foreach (WC.PDF.Document.TableTRColumn Td in Tr.Column)
                             {
                                 if (Td.Type == "SubTable")
                                 {
                                     #region SubTable

                                     returnCell = returnRow.Cells[rowcellpoint];
                                     double innerTblWidth = 0.0;
                                     if (Td.MergeRight != "0")
                                     {
                                         int _mergecount = int.Parse(Td.MergeRight.ToString());
                                         double Columnwidth = 0.0;
                                         for (int i = rowcellpoint; i <= _mergecount; i++)
                                         {
                                             Columnwidth += Columwidths[i];
                                         }
                                         innerTblWidth = Math.Round((double)((TableWidth * Columnwidth) / 100), 2);
                                     }
                                     else
                                         innerTblWidth = Math.Round((double)((TableWidth * Columwidths[rowcellpoint]) / 100), 2);

                                     foreach (WC.PDF.Document.TableTRColumnSubTable subTbl in Td.SubTable)
                                     {

                                         WC.PDF.Document.Table InnerTable = GetInnerTable(subTbl.ID.ToString());
                                         MigraDoc.DocumentObjectModel.Tables.Table inntb = GetTable(InnerTable, innerTblWidth, RowHeight);
                                         returnCell.Elements.Add(inntb);

                                         returnRow.Cells[rowcellpoint].Format.Alignment = ParagraphAlignment.Center;
                                         returnRow.Cells[rowcellpoint].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom;

                                     }

                                     #endregion
                                 }
                                 else if (Td.Type == "Image")
                                 {
                                     #region image

                                     string Imgurl = GetReplaceImage(Td.Url.ToString());
                                     returnRow.Cells[rowcellpoint].AddImage(Imgurl);
                                     returnRow.Cells[rowcellpoint].Format.Alignment = obj_pdfutility.GetFormatAlignment(Td.FormatAlignment.ToString());
                                     returnRow.Cells[rowcellpoint].VerticalAlignment = obj_pdfutility.GetVerticalAlignment(Td.VerticalAlignment.ToString());
                                     #endregion

                                 }
                                 else if (Td.Type == "nbsp")
                                 {
                                     returnRow.Cells[rowcellpoint].AddParagraph("");
                                 }
                                 else
                                 {
                                     #region Text

                                     fontsize = int.Parse(Td.FontSize.ToString());
                                     fontname = Td.FontName.ToString();
                                     returnRow.Cells[rowcellpoint].MergeDown = int.Parse(Td.MergeDown.ToString());
                                     returnRow.Cells[rowcellpoint].MergeRight = int.Parse(Td.MergeRight.ToString());
                                     returnRow.Cells[rowcellpoint].Format.Font.Bold = obj_pdfutility.GetBoolenvalue(Td.NeedFontBoldSyle.ToString());
                                     returnRow.Cells[rowcellpoint].Format.Font = new MigraDoc.DocumentObjectModel.Font(fontname, fontsize);
                                     returnRow.Cells[rowcellpoint].Format.Alignment = obj_pdfutility.GetFormatAlignment(Td.FormatAlignment.ToString());
                                     returnRow.Cells[rowcellpoint].VerticalAlignment = obj_pdfutility.GetVerticalAlignment(Td.VerticalAlignment.ToString());

                                     #region Border Width design
                                     _Bwidht = Td.BorderWidth.ToString().Split(',');
                                     if (_Bwidht.Count() == 1)
                                         returnRow.Cells[rowcellpoint].Borders.Width = double.Parse(_Bwidht[0].ToString());
                                     else
                                     {
                                         returnRow.Cells[rowcellpoint].Borders.Right.Width = double.Parse(_Bwidht[0].ToString());
                                         returnRow.Cells[rowcellpoint].Borders.Left.Width = double.Parse(_Bwidht[1].ToString());
                                         returnRow.Cells[rowcellpoint].Borders.Top.Width = double.Parse(_Bwidht[2].ToString());
                                         returnRow.Cells[rowcellpoint].Borders.Bottom.Width = double.Parse(_Bwidht[3].ToString());
                                     }

                                     #endregion


                                     returnRow.Cells[rowcellpoint].Format.Font.Italic = obj_pdfutility.GetBoolenvalue(Td.NeedFontItalicSyle.ToString());
                                     returnRow.Cells[rowcellpoint].Format.Font.Underline = obj_pdfutility.GetUnderlineFromat(Td.Underline.ToString());

                                     returnRow.Cells[rowcellpoint].Format.Font.Color = obj_pdfutility.GetColors(Td.FontColor.ToString());
                                     returnRow.Cells[rowcellpoint].Borders.Color = obj_pdfutility.GetColors(Td.BordersColor.ToString());
                                     returnRow.Cells[rowcellpoint].Format.Shading.Color = obj_pdfutility.GetColors(Td.ShadingColor.ToString());

                                     _Message = "";
                                     Textkeywords = Td.Value.ToString();
                                     Split_Textkeywords = Regex.Split(Textkeywords, "/split/");
                                     for (int i = 0; i < Split_Textkeywords.Count(); i++)
                                     {
                                         Text = GetReplacevalue(Split_Textkeywords[i].ToString());
                                         _splittxt = Regex.Split(Text, "/nbsp");
                                         if (_splittxt.Count() > 1)
                                         {
                                             Text = obj_pdfutility.GetParagraphStr(_splittxt);
                                         }
                                         Split_Textkeywords[i] = Text;
                                     }
                                     for (int i = 0; i < Split_Textkeywords.Count(); i++)
                                     {
                                         if (i == 0)
                                             _Message = Split_Textkeywords[i];
                                         else
                                             _Message += Split_Textkeywords[i];

                                     }
                                     if (_Message == "_")
                                         _Message = obj_pdfutility.GetLineStr(Convert.ToDouble(Td.LineLength.ToString()));

                                     returnRow.Cells[rowcellpoint].AddParagraph(_Message);

                                     #endregion
                                 }
                                 rowcellpoint++;

                             }

                         }
                         break;

                     #endregion

                 }

                 #endregion
             }

             return returnTable;
         }

        

         private MigraDoc.DocumentObjectModel.Tables.Table GetDynamicTable(WC.PDF.Document.Table InnerTable, double innerTblWidth, double RowHeight,DataRow dr)
         {
             MigraDoc.DocumentObjectModel.Tables.Table tb = new MigraDoc.DocumentObjectModel.Tables.Table();
             MigraDoc.DocumentObjectModel.Tables.Row row = null;
             MigraDoc.DocumentObjectModel.Tables.Cell cel = new MigraDoc.DocumentObjectModel.Tables.Cell();
             string strHeader = InnerTable.HeaderWidths.ToString();
             double[] Columwidths = obj_pdfutility.GetHeaderwiths(strHeader);
             int ColumnCount = Columwidths.Count();

             #region Add Column

             double ColumWith = 0;
             for (int c = 0; c < ColumnCount; c++)
             {
                 ColumWith = ((innerTblWidth * Columwidths[c]) / 100);
                 ColumWith = Math.Round((double)ColumWith, 2);
                 tb.AddColumn(ColumWith);
             }

             #endregion
             row = tb.AddRow();
             row.Height = RowHeight;
             int Ds_column = 2, column=0;
             foreach (TableColumnConfiguration confg in InnerTable.ColumnConfiguration)
             {
                 row.Cells[column].Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 8);
                 row.Cells[column].Format.Alignment = obj_pdfutility.GetFormatAlignment(confg.FormatAlignment.ToString());
                 row.Cells[column].VerticalAlignment = obj_pdfutility.GetVerticalAlignment(confg.VerticalAlignment.ToString());
                 row.Cells[column].Borders.Width = double.Parse(confg.BorderWidth.ToString());
                 row.Cells[column].Format.Font.Underline = obj_pdfutility.GetUnderlineFromat(confg.Underline.ToString());
                 row.Cells[column].Format.Font.Color = obj_pdfutility.GetColors(confg.FontColor.ToString());
                 row.Cells[column].Borders.Color = obj_pdfutility.GetColors(confg.BordersColor.ToString());
                 row.Cells[column].Format.Shading.Color = obj_pdfutility.GetColors(confg.ShadingColor.ToString());
         
                 row.Cells[column].AddParagraph(dr[Ds_column].ToString());
                 column++;
                 Ds_column++;

             }
             tb.Borders.Visible = true;
             return tb;
         }

         private WC.PDF.Document.Table GetInnerTable(string Tablename)
         {
             WC.PDF.Document.Table returnInnerTable = null;
             WCpdfDocumentTableList obj_Tbllist = DocDefinition.tableList.FirstOrDefault();
             foreach (WC.PDF.Document.Table innerTbl in obj_Tbllist.Table)
             {
                 if (innerTbl.Id == Tablename)
                     returnInnerTable = innerTbl;
             }
             return returnInnerTable;
         }
    
         private string GetReplaceImage(string Keyword)
         {
             string returnReplaceImage = "";
             if (imageUrlDictionary.ContainsKey(Keyword))
                 returnReplaceImage = imageUrlDictionary[Keyword];
             else
                 returnReplaceImage = Keyword;
             return returnReplaceImage;
         }

         private string GetReplacevalue(string Keyword)
         {
             string returnReplacevalue = "";

             if (dataDictionary.ContainsKey(Keyword))
                 returnReplacevalue = dataDictionary[Keyword];
             else
                 returnReplacevalue = Keyword;
             return returnReplacevalue;
         }

         private DataSet GetReplaceDataset(string Keyword,bool ColumDivided)
         {
             DataSet returnReplacevalue =new DataSet();
             if (DatasetDictionary.ContainsKey(Keyword))
                 returnReplacevalue = DatasetDictionary[Keyword];
             else
             {
                 DataTable table = new DataTable("Table");
                 table.Columns.Add("Columns",typeof(string));
                 table.Rows.Add("NoDataFound");
                 returnReplacevalue.Tables.Add(table);

             }

             #region columDivided

             if (ColumDivided)
             {
                 DataTable _table = new DataTable("Table1");
                 for (int i = 1; i < 7; i++)
                 {
                     _table.Columns.Add("Column"+i.ToString());
                 }
                //_table.Rows.Add();

                bool valid = true;
                int newtblrow = 0,oldtblcoumn=0;
                if (returnReplacevalue.Tables[0].Columns.Count == 4)
                {
                    foreach (DataRow dr in returnReplacevalue.Tables[0].Rows)
                    {
                        if (valid == true)
                        {
                            oldtblcoumn = 0;
                            _table.Rows.Add();
                            newtblrow++;
                           
                        }
                        _table.Rows[newtblrow][oldtblcoumn] = dr[1].ToString();
                        _table.Rows[newtblrow][oldtblcoumn+1] = ":";
                        _table.Rows[newtblrow][oldtblcoumn+2] = dr[3].ToString();
                        oldtblcoumn=oldtblcoumn + 3;
                        valid = false;
                        if (oldtblcoumn == _table.Columns.Count)
                        {
                            valid = true;
                            //_table.Rows.Add();
                            newtblrow++;
                        }

                    }
                    returnReplacevalue.Tables.Clear();
                    returnReplacevalue.Tables.Add(_table);
                }
             }
             #endregion

             return returnReplacevalue;
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

     }
}


  //internal MigraDoc.DocumentObjectModel.Tables.Table GetTable(WC.PDF.Document.Table _Table, int _Tblwidth, int _TblHeight)
  //       {
  //           obj_pdfutility = new PDFUtility();

  //           int TableWidth =0,TableHeight=0;

  //           TableWidth = _Tblwidth - (Convert.ToInt32((Math.Round(double.Parse(_Table.LeftPadding.ToString()) + double.Parse(_Table.RightPadding.ToString())))));
  //           TableHeight = Convert.ToInt32((_TblHeight*double.Parse(_Table.TableHeight.ToString()))/100);
  //           TableHeight=TableHeight-(Convert.ToInt32(Math.Round(double.Parse(_Table.TopPadding.ToString())+double.Parse(_Table.BottomPadding.ToString()))));
  //           string strHeader = _Table.HeaderWidths.ToString();
  //           double[] Columwidths =obj_pdfutility.GetHeaderwiths(strHeader);
  //           int ColumnCount = Columwidths.Count();

  //           MigraDoc.DocumentObjectModel.Tables.Table returnTable = new MigraDoc.DocumentObjectModel.Tables.Table();
  //           MigraDoc.DocumentObjectModel.Tables.Row returnRow = null;
  //           //if(_Table.Id=="Part3Healthandphysicalactivities_Table")
  //           //     returnTable.Shading.Color = obj_pdfutility.GetColors(_Table.ShadingColor.ToString());
  //           returnTable.Shading.Color = obj_pdfutility.GetColors(_Table.ShadingColor.ToString());


            

  //           MigraDoc.DocumentObjectModel.Tables.Cell returnCell = null;

  //           switch (_Table.Type.ToString())
  //           {

  //               #region Dynamic
  //               case "Dynamic":
  //                   break;

  //               #endregion

  //               #region Static

  //               case "Static":

  //                   #region Add Column
  //                   int ColumWith=0;
  //                   for (int c = 0; c < ColumnCount; c++)
  //                   {
  //                       ColumWith = Convert.ToInt32(Math.Round((TableWidth * Columwidths[c]) / 100));
  //                       returnTable.AddColumn(ColumWith);
  //                   }

  //                   #endregion

  //                   #region Add Row

  //                    int rowcellpoint = 0;
  //                    foreach (WC.PDF.Document.TableTR Tr in _Table.Tr)
  //                    {
  //                        returnRow = returnTable.AddRow();

  //                        int RowHeight =Convert.ToInt32(Math.Round(TableHeight*(double.Parse(Tr.RowHeight.ToString()))/100));
  //                        RowHeight=RowHeight-Convert.ToInt32(Math.Round((double.Parse(Tr.RowTopPadding.ToString())+ double.Parse(Tr.RowBottomPadding.ToString()))));
                          
  //                        returnRow.Height = RowHeight;

  //                        returnRow.Borders.Width = double.Parse(Tr.BorderWidth.ToString());
  //                        returnRow.TopPadding = double.Parse(Tr.RowTopPadding.ToString());
  //                        returnRow.BottomPadding = double.Parse(Tr.RowBottomPadding.ToString());
  //                        returnRow.Format.Alignment = obj_pdfutility.GetFormatAlignment(Tr.Alignment.ToString());

  //                         rowcellpoint = 0;
  //                         foreach (WC.PDF.Document.TableTRColumn Td in Tr.Column)
  //                         {
  //                             if (Td.Type == "SubTable")
  //                             {
  //                                 #region SubTable

  //                                 returnCell = returnRow.Cells[rowcellpoint];
  //                                 int innerTblWidth = Convert.ToInt32(Math.Round((TableWidth * Columwidths[rowcellpoint]) / 100));
  //                                 foreach (WC.PDF.Document.TableTRColumnSubTable subTbl in Td.SubTable)
  //                                 {
  //                                     WC.PDF.Document.Table InnerTable = GetInnerTable(subTbl.ID.ToString());
  //                                     MigraDoc.DocumentObjectModel.Tables.Table inntb = GetTable(InnerTable, innerTblWidth, RowHeight);
  //                                     returnCell.Elements.Add(inntb);

  //                                     returnRow.Cells[rowcellpoint].Format.Alignment = ParagraphAlignment.Center;
  //                                     returnRow.Cells[rowcellpoint].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom;

  //                                 }

  //                                 #endregion
  //                             }
  //                             else if (Td.Type == "Image")
  //                             {
  //                                 #region image
                                   
  //                                 string Imgurl = GetReplaceImage(Td.Url.ToString());
  //                                 //Image image = returnRow.Cells[0].Elements.AddImage(Imgurl);
  //                                 ////image.LockAspectRatio = true;
  //                                 ////image.RelativeVertical = RelativeVertical.Line;
  //                                 ////image.RelativeHorizontal = RelativeHorizontal.Margin;
  //                                 ////image.Top = ShapePosition.Top;
  //                                 ////image.Left = ShapePosition.Right;
  //                                 ////image.WrapFormat.Style = WrapStyle.None;

  //                                 returnRow.Cells[rowcellpoint].AddImage(Imgurl);
  //                                 returnRow.Cells[rowcellpoint].Format.Alignment = obj_pdfutility.GetFormatAlignment(Td.FormatAlignment.ToString());
  //                                 returnRow.Cells[rowcellpoint].VerticalAlignment = obj_pdfutility.GetVerticalAlignment(Td.VerticalAlignment.ToString());
  //                                 #endregion

  //                             }
  //                             else if (Td.Type == "nbsp")
  //                             {
  //                                 returnRow.Cells[rowcellpoint].AddParagraph("");
  //                             }
  //                             else
  //                             {
  //                                 #region Text

  //                                 int fontsize = int.Parse(Td.FontSize.ToString());
  //                                 string fontname = Td.FontName.ToString();

  //                                 returnRow.Cells[rowcellpoint].MergeDown = int.Parse(Td.MergeDown.ToString());
  //                                 returnRow.Cells[rowcellpoint].MergeRight = int.Parse(Td.MergeRight.ToString());
  //                                 returnRow.Cells[rowcellpoint].Format.Font.Bold =obj_pdfutility.GetBoolenvalue(Td.NeedFontBoldSyle.ToString());
  //                                 returnRow.Cells[rowcellpoint].Format.Font = new MigraDoc.DocumentObjectModel.Font(fontname, fontsize);
  //                                 returnRow.Cells[rowcellpoint].Format.Alignment = obj_pdfutility.GetFormatAlignment(Td.FormatAlignment.ToString());
  //                                 returnRow.Cells[rowcellpoint].VerticalAlignment = obj_pdfutility.GetVerticalAlignment(Td.VerticalAlignment.ToString());
  //                                 returnRow.Cells[rowcellpoint].Borders.Width = double.Parse(Td.BorderWidth.ToString());
  //                                 returnRow.Cells[rowcellpoint].Format.Font.Italic = obj_pdfutility.GetBoolenvalue(Td.NeedFontItalicSyle.ToString());
  //                                 returnRow.Cells[rowcellpoint].Format.Font.Underline = obj_pdfutility.GetUnderlineFromat(Td.Underline.ToString());

  //                                 returnRow.Cells[rowcellpoint].Format.Font.Color = obj_pdfutility.GetColors(Td.FontColor.ToString());
  //                                 returnRow.Cells[rowcellpoint].Borders.Color = obj_pdfutility.GetColors(Td.BordersColor.ToString());
  //                                 returnRow.Cells[rowcellpoint].Format.Shading.Color = obj_pdfutility.GetColors(Td.ShadingColor.ToString());
                                  

  //                                 string Text =GetReplacevalue(Td.Value.ToString());

  //                                 if (Text == "_")
  //                                 {
  //                                     double Linelength = ((ColumWith *double.Parse(Td.LineLength.ToString())) / 100);
  //                                     Linelength = Math.Round(Linelength);

  //                                     Text = " ";
  //                                     for (int i = 0; i < Linelength; i++)
  //                                     {
  //                                         if(i==Linelength)
  //                                             Text = Text + " ";
  //                                         else
  //                                             Text = Text + "_";

                                           
  //                                     }
  //                                 }
  //                                 string[] _splittxt = Regex.Split(Text, "/nbsp");//Text.Split('@');
  //                                 if (_splittxt.Count() > 1)
  //                                 {
  //                                     Text =obj_pdfutility.GetParagraphStr(_splittxt);
  //                                 }
  //                                 returnRow.Cells[rowcellpoint].AddParagraph(Text);
                                

  //                                 #endregion
  //                             }
  //                             rowcellpoint++;


  //                         }
  //                    }

  //                   #endregion

  //                   break;

  //               #endregion

                
  //           }


  //           return returnTable;
  //       }



