using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Data.Odbc;
using WinBase;

namespace WinEr
{
    public partial class OpenBarcodePdf : System.Web.UI.Page
    {
        private LibraryManagClass MyLibMang;
        private KnowinUser MyUser;
        private string m_PdfName = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyLibMang = MyUser.GetLibObj();
            if (MyLibMang == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }
            else
            if (Request.QueryString["PdfName"] != null)
            {
                m_PdfName = Request.QueryString["PdfName"].ToString();
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
            if (!IsPostBack)
            {
                if (m_PdfName != "")
                {
                    loadBarcodeToPage(m_PdfName);
                }
            }
        }

        private void loadBarcodeToPage(string _PdfName)
        {

            string _physicalpath =Server.MapPath("Barcode_files") + "\\Barcode_pdfs\\" + _PdfName;
            string __pdfl = _physicalpath;
            string fileExtension = System.IO.Path.GetExtension(".pdf").ToLower();
            string ContentType = "application/msword";
            if (fileExtension == ".pdf")
            {
                ContentType = "application/pdf";
            }
            //string __pdfl = "PDF_Files/Quotation" + Lbl_qtId.Text + ".pdf";

            if (File.Exists(__pdfl))
            {
                if (!IsFileInUse(__pdfl))
                {
                    FileStream fs = File.Open(__pdfl, FileMode.Open, FileAccess.Read);
                    Stream output = Response.OutputStream;
                    int BUFFER_SIZE = (int)fs.Length + 500;
                    byte[] buffer = new byte[BUFFER_SIZE];
                    int read_count = fs.Read(buffer, 0, BUFFER_SIZE);
                    while (read_count > 0)
                    {
                        output.Write(buffer, 0, read_count);
                        read_count = fs.Read(buffer, 0, BUFFER_SIZE);
                    }

                    fs.Close();

                    Response.Clear();
                    Response.ContentType = ContentType;
                    //Response.AddHeader("Content-Disposition", "attachment; filename=form.pdf");
                    Response.OutputStream.Write(buffer, 0, BUFFER_SIZE);
                    Response.Output.Flush();
                    Response.End();
                    fs.Dispose();

                }
                else
                {
                    string _Str = "";
                    _Str = "<center><h3>The process cannot access the document.Another person is using the same document</h3></center>";
                    HtmlContainerControl SubLinkDiv = (HtmlContainerControl)this.FindControl("type");
                    SubLinkDiv.InnerHtml = _Str;
                }
            }
            else
            {
                string _Str = "";
                _Str = "<center><h3>Document Has Been Removed</h3></center>";
                HtmlContainerControl SubLinkDiv = (HtmlContainerControl)this.FindControl("type");
                SubLinkDiv.InnerHtml = _Str;
            }


        }

        private bool IsFileInUse(string __pdfl)
        {
            try
            {

                using (FileStream fs = new FileStream(__pdfl, FileMode.OpenOrCreate))
                {

                }
                return false;
            }
            catch (IOException ex)
            {
                string message = ex.Message.ToString();

                if (message.Contains("The process cannot access the file"))
                {

                    return true;
                }
                else
                    throw;
            }

        }
    }
}
