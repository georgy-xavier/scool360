using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Collections;
namespace WinEr
{
    public class ExcelUtility
    {
        public static bool ExportDataSetToExcel(DataSet ds, string filename)
        {
            bool _valid;
            try
            {
                HttpResponse response = HttpContext.Current.Response;

                // first let's clean up the response.object
                response.Clear();
                response.Charset = "";

                // set the response mime type for excel
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", "attachment;filename=\"" + filename + "\"");

                // create a string writer
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        // instantiate a datagrid
                        DataGrid dg = new DataGrid();
                        dg.DataSource = ds.Tables[0];
                        dg.DataBind();
                        dg.RenderControl(htw);
                        response.Write(sw.ToString());
                        response.End();
                    }
                }
                _valid = true;
            }
            catch 
            {
                _valid = false;

            }
            return _valid;
        }




        public static bool ExportDataToExcel(DataSet _DataSet, string _ReportName,string _FileName, string _TopHeader)
        {
            bool _valid;
            try
            {
                string _Tablestring = "";
                if (_DataSet != null && _DataSet.Tables != null && _DataSet.Tables[0].Rows.Count > 0)
                {
                    string colspan = "10";
                    DataTable dt = _DataSet.Tables[0];
                    int ColumnCount = dt.Columns.Count;
                    _Tablestring += _TopHeader;
                    _Tablestring += "<table width=\"100%\"><tr>";
                    _Tablestring += "<td colspan=\"" + colspan + "\" style=\"font-size:19px;text-align:center\">" + _ReportName + "";
                    _Tablestring += "</td></tr></table>";
                    _Tablestring +=  "<table width=\"100%\">";
                    for (int i = 0; i < ColumnCount; i++)
                    {
                        _Tablestring += "<th>" + dt.Columns[i].ColumnName + "</th>";
                    }
                    foreach (DataRow dr_Row in _DataSet.Tables[0].Rows)
                    {
                        _Tablestring += "<tr>";
                        for (int i = 0; i < ColumnCount; i++)
                        {
                            _Tablestring += "<td>" + dr_Row[i].ToString() + "</td>";
                        }
                        _Tablestring += "</tr>";
                    }
                    _Tablestring += "</table>";
                    ExportBuiltStringToExcel(_FileName, _Tablestring, _ReportName);
                }
                _valid = true;
            }
            catch
            {
                _valid = false;

            }
            return _valid;
        }

        internal static bool ExportGridViewToExcel(string FileName, GridView Grd_Students)
        {
            HttpResponse Response = HttpContext.Current.Response;
            bool _valid = true;
            try
            {
                Response.Clear();
                Response.Buffer = true;

                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xls", FileName));
                Response.Charset = "";
                System.IO.StringWriter stringwriter = new StringWriter();
                HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
                Grd_Students.RenderControl(htmlwriter);
                Response.Write(stringwriter.ToString());
                Response.End();
            }
            catch
            {
                _valid = false;
            }
            return _valid;
        }

        internal static bool ExportGridViewToWord(string FileName, GridView Grd_Students)
        {
            HttpResponse Response = HttpContext.Current.Response;
            bool _valid = true;
            try
            {
                Response.Clear();
                Response.Buffer = true;

                Response.ContentType = "application/ms-word";
                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.doc", FileName));
                Response.Charset = "";
                System.IO.StringWriter stringwriter = new StringWriter();
                HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
                Grd_Students.RenderControl(htmlwriter);
                Response.Write(stringwriter.ToString());
                Response.End();
            }
            catch
            {
                _valid = false;
            }
            return _valid;
        }

        internal static bool ExportBuiltStringToExcel(string FileName, string _TableString,string _WorkSheetName)
        {
            bool _Valid = true;
            try
            {
                HttpResponse Response = HttpContext.Current.Response;
                Response.ContentType = "application/force-download";
                Response.AddHeader("content-disposition", "attachment; filename=" + FileName + ".xls");
                Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
                Response.Write("<head>");
                Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                Response.Write("<!--[if gte mso 9]><xml>");
                Response.Write("<x:ExcelWorkbook>");
                Response.Write("<x:ExcelWorksheets>");
                Response.Write("<x:ExcelWorksheet>");
                Response.Write("<x:Name>" + _WorkSheetName + "</x:Name>");
                Response.Write("<x:WorksheetOptions>");
                Response.Write("<x:Print>");
                Response.Write("<x:ValidPrinterInfo/>");
                Response.Write("</x:Print>");
                Response.Write("</x:WorksheetOptions>");
                Response.Write("</x:ExcelWorksheet>");
                Response.Write("</x:ExcelWorksheets>");
                Response.Write("</x:ExcelWorkbook>");
                Response.Write("</xml>");
                Response.Write("<![endif]--> ");
                Response.Write(_TableString);
                Response.Write("</head>");
                Response.Flush();
                Response.End();
            }
            catch
            {
                _Valid = false;
            }
            return _Valid;
        }
    }
}
