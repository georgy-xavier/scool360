using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

namespace WC.PdfDocumentClass
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
}
