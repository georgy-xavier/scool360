using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel.Shapes;
using System.Drawing.Drawing2D;

namespace WinEr
{
    public partial class WebForm14 : System.Web.UI.Page
    {
        private LibraryManagClass MyLibMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
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
            {
                if (!IsPostBack)
                {
                    initial_load();
                  
                }
            }
        }
        private void initial_load()
        {
            Drp_search.SelectedIndex = 0;
            Drplist_period.SelectedIndex = 0;
            Txt_name.Visible = false;
            LoadDateInTextBox();
            load_searchdetails();
            //LoadBookType(0);
            LoadBookCategoryToDrpList(0);
            Lbl_err.Visible = false;
            Pnl_Showfound.Visible = false;
        }
        //private void LoadBookType(int _intex)
        //{
        //    Drp_type.Items.Clear();
        //    string sql = "SELECT Id,TypeName FROM tblbooktype";
        //    MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
        //    if (MyReader.HasRows)
        //    {
        //        ListItem li = new ListItem("Select", "0");
        //        Drp_type.Items.Add(li);
        //        while (MyReader.Read())
        //        {
        //            ListItem li2 = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
        //            Drp_type.Items.Add(li2);
        //        }
        //        Drp_type.SelectedIndex = _intex;
        //    }
        //    else
        //    {
        //        ListItem li = new ListItem("No Data Exist","0");
        //        Drp_type.Items.Add(li);
        //        Drp_type.SelectedIndex = 0;
        //    }
        //}

        private void LoadBookCategoryToDrpList(int _intex)
        {
            Drp_catagory.Items.Clear();
            string sql = "SELECT Id,CatogoryName FROM tblbookcatogory";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("Any", "0");
                Drp_catagory.Items.Add(li);

                while (MyReader.Read())
                {
                    ListItem li2 = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_catagory.Items.Add(li2);
                }
                Drp_catagory.SelectedIndex = _intex;
            }
            else
            {
                ListItem li = new ListItem("No Data Exist", "0");
                Drp_catagory.Items.Add(li);
                Drp_catagory.SelectedIndex = 0;
            }
        }
        protected void Drplist_period_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDateInTextBox();
        }
        protected void Drp_search_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_searchdetails();
        }
        private void LoadDateInTextBox()
        {
            string _sdate = null, _edate = null;
            if (int.Parse(Drplist_period.SelectedValue.ToString()) == 0)
            {
                DateTime _date = System.DateTime.Today;
                //_sdate = _date.ToString("dd/MM/yyyy");
                _sdate = MyUser.GerFormatedDatVal(_date);

                Txt_frmdate.Enabled = false;
                Txt_todate.Enabled = false;
                Txt_frmdate.Text = _sdate;
                Txt_todate.Text = _sdate;
            }
            if (int.Parse(Drplist_period.SelectedValue.ToString()) == 1)
            {
                DateTime _date = System.DateTime.Now;
                //_edate = _date.ToString("dd/MM/yyyy");
                _edate = MyUser.GerFormatedDatVal(_date);

                DateTime _start = _date.AddDays(-7);
                //_sdate = _start.Date.ToString("dd/MM/yyyy");
                _sdate = MyUser.GerFormatedDatVal(_start);

                Txt_frmdate.Enabled = false;
                Txt_todate.Enabled = false;
                Txt_frmdate.Text = _sdate;
                Txt_todate.Text = _edate;
            }
            if (int.Parse(Drplist_period.SelectedValue.ToString()) == 2)
            {
                DateTime _date = System.DateTime.Now;
                //_edate = _date.ToString("dd/MM/yyyy");
                _edate = MyUser.GerFormatedDatVal(_date);

                DateTime _start = System.DateTime.Now;
                //_sdate = _start.Date.ToString("01/MM/yyyy");
                _sdate = MyUser.GerFormatedDatVal(new DateTime(_start.Year, _start.Month, 1));
                Txt_todate.Enabled = false;
                Txt_frmdate.Enabled = false;
                Txt_frmdate.Text = _sdate;
                Txt_todate.Text = _edate;

            }
            if (int.Parse(Drplist_period.SelectedValue.ToString()) == 3)
            {
                Txt_todate.Enabled = true;
                Txt_frmdate.Enabled = true;
                Txt_frmdate.Text = "";
                Txt_todate.Text = "";
            }
        }
        private void load_searchdetails()
        {
            if (int.Parse(Drp_search.SelectedValue.ToString()) == 1)
            {
                lbl_select.Text = "Book Name:";
                Txt_name.Visible = true;
            }
            else if (int.Parse(Drp_search.SelectedValue.ToString()) == 2)
            {
                lbl_select.Text = "Publisher Name:";
                Txt_name.Visible = true;
            }
            else if (int.Parse(Drp_search.SelectedValue.ToString()) == 3)
            {
                lbl_select.Text = "Author Name:";
                Txt_name.Visible = true;
            }
            else
            {
                lbl_select.Text = "";
                Txt_name.Visible = false;
            }
        }
        protected void Btn_Show_Click(object sender, EventArgs e)
        {
            try
            {
                string err_msg = "";
                DataSet ds_Barcode = new DataSet();
                ds_Barcode = Generate_Dataset(out err_msg);
                load_Barcode_Details(ds_Barcode, out err_msg);
                if (err_msg != "")
                {
                    Pnl_Showfound.Visible = false;
                    Lbl_err.Visible = true;
                    Lbl_err.Text = err_msg;
                }
                else
                {
                    Pnl_Showfound.Visible = true;
                    Lbl_err.Visible = false;
                }
            }
            catch (Exception br)
            {
                Lbl_err.Visible = true;
                Lbl_err.Text = br.Message;
            }
            
        }
        private DataSet Generate_Dataset(out string _errmsg)
        {
            string sql = "";
            _errmsg = "";
            DataSet ds_search = new DataSet();
            DateTime _from = MyUser.GetDareFromText(Txt_frmdate.Text.ToString());
            DateTime _To = MyUser.GetDareFromText(Txt_todate.Text.ToString());
            DateTime _change_todate = new DateTime();
            check_dates(_from, _To, out _change_todate, out _errmsg);
            sql = "SELECT DISTINCT(tblbooks.Id),tblbooks.BookId,tblbooks.Barcode From tblbooks  WHERE tblbooks.BookId IN(select tblbookmaster.Id from tblbookmaster WHERE tblbookmaster.BarcodeType <> 0  AND tblbookmaster.CreatedDate >='" + _from.Date.ToString("s") + "' AND tblbookmaster.CreatedDate <='" + _change_todate.ToString("s") + "'";
            if (int.Parse(Drp_catagory.SelectedValue.ToString()) != 0)
            {
                sql=sql+" AND tblbookmaster.CatagoryId="+ int.Parse(Drp_catagory.SelectedValue.ToString()) +"";
            }
            //if (int.Parse(Drp_type.SelectedValue.ToString()) != 0)
            //{
            //    sql = sql + " AND tblbookmaster.TypeId=" + int.Parse(Drp_type.SelectedValue.ToString()) + "";
            //}
            if (int.Parse(Drp_search.SelectedValue.ToString())!=0)
            {
                if (int.Parse(Drp_search.SelectedValue.ToString()) == 1)
                {
                    sql = sql + " AND tblbookmaster.BookName LIKE '%"+ Txt_name.Text.ToString()+"%'";
                }
                else if (int.Parse(Drp_search.SelectedValue.ToString()) == 2)
                {
                    sql = sql + " AND tblbookmaster.Publisher LIKE '%" + Txt_name.Text.ToString() + "%'";
                }
                else if (int.Parse(Drp_search.SelectedValue.ToString()) == 2)
                {
                    sql = sql + " AND tblbookmaster.Author LIKE '%" + Txt_name.Text.ToString() + "%'";
                }
            }
            sql = sql + ")";
            ds_search = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return ds_search;
        }
        private void check_dates(DateTime _frdate,DateTime _todate,out DateTime _To,out string _error)
        {
            _To=_todate;
            _error = "";
            if (_To == DateTime.Parse(DateTime.Now.ToString("d")))
            {
                _To = DateTime.Now;
            }
            if (DateTime.Parse(_To.ToString("g")) > DateTime.Parse(DateTime.Now.ToString("g")))
            {
                _error = "To Date is Greaterthan Today";
            }

        }
        private void load_Barcode_Details(DataSet ds_Barcode,out string errmsg)
        {
            int found_bookcount = 0;
            errmsg = "";
            if (ds_Barcode != null && ds_Barcode.Tables[0] != null && ds_Barcode.Tables[0].Rows.Count > 0)
            {
                Pnl_Showfound.Visible = true;
                found_bookcount = ds_Barcode.Tables[0].Rows.Count;
                Lbl_count.Text = found_bookcount.ToString();
                Session["ds_barcode"] = ds_Barcode;
            }
            else
            {
                errmsg = "Books not found";
            }
        }
        protected void Btn_Generate_Click(object sender, EventArgs e)
        {
            try
            {

                string _err_msg = "";
                string Pdf_Name = "";
                DataSet ds_barcode = (DataSet)Session["ds_barcode"];
                //generate Barcode images
                Generate_Barcode_image(ds_barcode, out Pdf_Name, out _err_msg);
                Pnl_Showfound.Visible = false;
                if (_err_msg != "")
                {
                    Lbl_err.Visible = true;
                    Lbl_err.Text = _err_msg;
                }
                else
                {
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "Barcode generated for books", 1);
                    ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenBarcodePdf.aspx?PdfName=" + Pdf_Name + "\");", true);
                    Session["ds_barcode"] = null;
                }
            }
            catch (Exception gr)
            {
                Lbl_err.Visible = true;
                Lbl_err.Text = gr.Message;
            }
        }
        private void Generate_Barcode_image(DataSet ds_generate,out string Pdf_Name,out string _errmsg)
        {
            _errmsg = "";
            Pdf_Name = "";
            int check_count = 1;
            int i = 1;
            double img_height = 0;
            double img_width = 0;
            double _check_imgwidth = 0;
            double _Max_imgwidth = 0;
            double _check_imgheight = 0;
            double _Max_imgheight = 0;
            //need Barcode Book Name
            DataSet ds_needtext = new DataSet();
            DataTable dt_needtext = new DataTable();
            dt_needtext.Columns.Add("BookId", typeof(System.String));
            dt_needtext.Columns.Add("Imgname", typeof(System.String));
            DataRow dr_needtext = null;
            string barcode_imagepath = generate_imagepath();
            string pdf_path = generate_pdfpath();
            int _barcodesize = MyLibMang.get_barcode_imagesize();
            //delete barcode images before generating new barcode image
            Delete_Barcode_Images(barcode_imagepath, out _errmsg);
            if (_errmsg == "")
            {
                if (ds_generate != null && ds_generate.Tables[0] != null && ds_generate.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds_generate.Tables[0].Rows)
                    {
                        string save_imagepath = barcode_imagepath + "/sample" + i + ".gif";
                        //generate Barcode Image Bitmap
                        Bitmap barCode = createbarcode(dr["Barcode"].ToString(), _barcodesize);
                        img_height = double.Parse(barCode.Size.Height.ToString());
                        img_width = double.Parse(barCode.Size.Width.ToString());
                        barCode.Save(save_imagepath, ImageFormat.Gif);
                        save_imagepath = "";
                        _check_imgwidth = img_width;
                        if (_Max_imgwidth < _check_imgwidth)
                        {
                            _Max_imgwidth = _check_imgwidth;
                        }
                        _check_imgheight = img_height;
                        if (_Max_imgheight < _check_imgheight)
                        {
                            _Max_imgheight = _check_imgheight;
                        }
                        //need Barcode book name
                        dr_needtext = dt_needtext.NewRow();
                        dr_needtext[0] = dr["BookId"].ToString();
                        dr_needtext[1] = "sample" + i + ".gif";
                        dt_needtext.Rows.Add(dr_needtext);
                        i++;
                        check_count++;

                    }
                    //generate Barcode pdf page
                    ds_needtext.Tables.Add(dt_needtext);
                    generate_barcodepdf(pdf_path, barcode_imagepath, _Max_imgheight, _Max_imgwidth, ds_needtext, out Pdf_Name, out _errmsg);
                }
                else
                {
                    _errmsg = "Barcode Data Not Found";
                }
            }

        }
        private void Delete_Barcode_Images(string barcode_img_path,out string _error)
        {
            _error = "";
            try
            {
                string[] filePaths = Directory.GetFiles(barcode_img_path);
                foreach (string filePath in filePaths)
                {
                    File.Delete(filePath);
                }
            }
            catch(Exception er)
            {
                _error = er.Message;
            }
        }
        private Bitmap createbarcode(string data, int size)
        {
            string barcode_fontpath = Server.MapPath("Barcode_files");
            barcode_fontpath = barcode_fontpath + "\\IDAutomationHC39M.ttf";
            Bitmap barcode = new Bitmap(1,1);
            string bdata = "*" + data + "*";
            //Font threeofnine = new Font("IDAutomationHC39M",size, FontStyle.Regular, GraphicsUnit.Point);
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile(barcode_fontpath);
            System.Drawing.Font threeofnine = new System.Drawing.Font(pfc.Families[0],size, FontStyle.Regular, GraphicsUnit.Pixel);
            Graphics graphic = Graphics.FromImage(barcode);
            graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            SizeF datasize = graphic.MeasureString(bdata, threeofnine);
            barcode = new Bitmap(barcode, datasize.ToSize());
            graphic = Graphics.FromImage(barcode);
            //image quality
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.Clear(System.Drawing.Color.White);
            graphic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
            ////graphic.FillRectangle(oBrush, 0, 0, w, 100);
            graphic.DrawString(bdata, threeofnine, new SolidBrush(System.Drawing.Color.Black), 0, 0);
            graphic.Flush();
            threeofnine.Dispose();
            graphic.Dispose();
            return barcode;
        }
        private string generate_pdfpath()
        {
            string pdf_path = Server.MapPath("Barcode_files") + "\\Barcode_pdfs\\";
            if (!Directory.Exists(pdf_path))
            {
                Directory.CreateDirectory(pdf_path);
            }
            return pdf_path;
        }
        private string generate_imagepath()
        {
            string Img_path = Server.MapPath("Barcode_files") + "\\Barcode_images\\";
            if (!Directory.Exists(Img_path))
            {
                Directory.CreateDirectory(Img_path);
            }
            return Img_path;
        }
        public void generate_barcodepdf(string _PhysicalPath, string _imagepath,double Maximgheight,double Maximgwidth,DataSet ds_bookname,out string Pdf_Name, out string _errmsg)
        {
            
                _errmsg = "";
                double _pageheight = 0;
                double _pagewidth = 0;
                Pdf_Name = "";
                try
                {
                    //Delete Barcode Pdfs Before Generate Pdf
                    Delete_Pdfs(_PhysicalPath);
                    bool IS_Horizantal = MyLibMang._ishorizontal();
                    bool Is_booknameneed = MyLibMang._Is_text_needed();
                    MyLibMang.Get_pdfpagetype(out _pagewidth, out  _pageheight, out _errmsg);
                    Document document = createpdf(_imagepath, _PhysicalPath, _pagewidth, _pageheight, IS_Horizantal, Maximgheight, Maximgwidth, ds_bookname, Is_booknameneed, out _errmsg);
                    Pdf_Name = "Barcode_Generate" + "_" + System.DateTime.Now.ToString().Replace('/', '_').Replace(':', '_') + ".pdf";
                    _PhysicalPath = _PhysicalPath + Pdf_Name;
                    MigraDoc.Rendering.DocumentRenderer renderer = new DocumentRenderer(document);
                    MigraDoc.Rendering.PdfDocumentRenderer pdfRenderer = new MigraDoc.Rendering.PdfDocumentRenderer();
                    pdfRenderer.Document = document;
                    pdfRenderer.RenderDocument();
                    pdfRenderer.Save(_PhysicalPath);
                }
                catch (Exception et)
                {
                    _errmsg = et.Message+"barcode size greaterthan page size";
                }
        }
        private void Delete_Pdfs(string pdf_path)
        {
            try
            {
                string[] filePaths = Directory.GetFiles(pdf_path);
                foreach (string filePath in filePaths)
                {
                    File.Delete(filePath);
                }
            }
            catch
            {

            }
        }
        public Document createpdf(string imagepath, string docsavepath,double page_width,double page_height, bool _ishorizontal,double maximgheight,double maximgwidth,DataSet ds_bkname,bool Is_booknameneed, out string msg)
        {
            msg = "";
            Document document = new Document();
            PageSetup PgSt = document.DefaultPageSetup;
            //set pdf page size based on library configurations
            PgSt.PageWidth =page_width;
            PgSt.PageHeight =page_height;
            PgSt.LeftMargin = 15;
            PgSt.RightMargin = 5;
            PgSt.TopMargin = 5;
            PgSt.BottomMargin = 5;
            PgSt.HeaderDistance = 0;
            PgSt.FooterDistance = 0;
            Section section = document.AddSection();
            Paragraph header = section.Headers.Primary.AddParagraph(" ");
            //get Barcode images details
            DataSet ds_Images = Get_Images(imagepath);
            if (_ishorizontal)
            {
                MigraDoc.DocumentObjectModel.Tables.Table generate_tablehorizontal = Generate_horizontalpdf(ds_Images, imagepath, page_height, page_width, maximgheight, maximgwidth,ds_bkname,Is_booknameneed, out msg);
                document.LastSection.Add(generate_tablehorizontal);

            }
            else
            {
                MigraDoc.DocumentObjectModel.Tables.Table generate_tablevertical = Generate_Verticalpdf(ds_Images, imagepath, page_height, page_width, maximgheight, maximgwidth,ds_bkname,Is_booknameneed,out msg);
                document.LastSection.Add(generate_tablevertical);
            }
            return document;
        }
        //generate Pdf as Horizontal alignment
        private MigraDoc.DocumentObjectModel.Tables.Table Generate_horizontalpdf(DataSet ds_horizontal, string _img_path, double page_height, double page_width, double maximgheight, double mximgwidth, DataSet ds_bkname,bool Is_bookname_need, out string _err_msg)
        {
            _err_msg = "";
            int col_count = 0;
            int barcode_count = 0;
            string Book_Name = "";
            int Book_Id = 0;
            int br_textcount = 0;
            MigraDoc.DocumentObjectModel.Tables.Table table = new MigraDoc.DocumentObjectModel.Tables.Table();
            Column column = new Column();
            mximgwidth = mximgwidth + 20;
            //convert image sizes pixel to points
            maximgheight = double.Parse(((maximgheight * 72) / 96).ToString());
            mximgwidth = double.Parse(((mximgwidth * 72) / 96).ToString());

            int column_count = int.Parse(Math.Floor((page_width-20) / (mximgwidth)).ToString());
            double remain_space = double.Parse(((page_width - 20) % ((mximgwidth) * column_count)).ToString());
            if (remain_space < 1)
            {
                column_count = column_count - 1;
            }
            if (column_count == 0)
            {
                _err_msg = "image size width is greater than page width";

            }
            else
            {
                if (ds_horizontal != null && ds_horizontal.Tables[0] != null && ds_horizontal.Tables[0].Rows.Count > 0)
                {
                    barcode_count = ds_horizontal.Tables[0].Rows.Count;
                    if (column_count == 1)
                    {
                        return Generate_Verticalpdf(ds_horizontal, _img_path, page_height, page_width, maximgheight, mximgwidth, ds_bkname,Is_bookname_need, out _err_msg);
                    }
                    else
                    {
                        //dist_width =(remain_space) / (double.Parse(column_count.ToString()));
                        //dist_width = Math.Round((double)dist_width, 2);
                        mximgwidth = Math.Round((double)mximgwidth, 2);
                        for (int i = 0; i < column_count; i++)
                        {
                            column = table.AddColumn(mximgwidth);
                            col_count++;
                        }

                        for (int checkcount = 0; checkcount < barcode_count; )
                        {
                            Row row = table.AddRow();
                            
                            row.BottomPadding = 10;
                            row.Height = maximgheight;
                            Cell cell = new Cell();
                            var results = new List<string>();
                            cell.Borders.Bottom.Color = Colors.White;
                            for (int j = 0; j < col_count; j++)
                            {
                                string barcode_img = "";
                                barcode_img = _img_path + ds_horizontal.Tables[0].Rows[checkcount][0].ToString();
                                cell = row.Cells[j];
                                cell.AddImage(barcode_img);
                                results.Add(ds_horizontal.Tables[0].Rows[checkcount][0].ToString());
                                checkcount++;
                              
                                if (barcode_count == checkcount)
                                {
                                    j = col_count + 1;
                                }
                            }
                            //Book Name needed under barcode image
                            if (Is_bookname_need)
                            {
                                if (ds_bkname != null && ds_bkname.Tables[0] != null && ds_bkname.Tables[0].Rows.Count > 0)
                                {
                                    Row row2 = table.AddRow();
                                    for (int k = 0; k < col_count; k++)
                                    {
                                        foreach (DataRow dr_bk in ds_bkname.Tables[0].Rows)
                                        {
                                            //if (results.Any(item => item.Equals(dr_bk["Imgname"].ToString())))
                                            if (results.First().Equals(dr_bk["Imgname"].ToString(), StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                Book_Id = int.Parse(dr_bk["BookId"].ToString());
                                                if (Book_Id != 0)
                                                {
                                                    Book_Name = MyLibMang.Get_Book_Name(Book_Id);
                                                    results.Remove(dr_bk["Imgname"].ToString());
                                                    break;
                                                }
                                            }
                                        }
    
                                        cell = row2.Cells[k];
                                        row2.BottomPadding = 10;
                                        row2.Cells[k].Format.Alignment = ParagraphAlignment.Center;
                                        cell.AddParagraph(Book_Name);
                                        br_textcount++;
                                        if (barcode_count == br_textcount)
                                        {
                                            k = col_count + 1;
                                        }
                                        Book_Name = "";
                                    }

                                }
                            }
                        }
                    }
                }

                else
                {
                    _err_msg = "No Barcode Data exist";
                }
            }
            return table;
        }
        //generate Pdf as Vertical alignment
        private MigraDoc.DocumentObjectModel.Tables.Table Generate_Verticalpdf(DataSet ds_vertical, string _imgpath, double pageheight, double pagewidth,double maximgheight,double mximgwidth,DataSet ds_bkname,bool Is_bookname_need, out string _errmsg)
        {
            _errmsg = "";
            string Book_Name = "";
            int Book_Id = 0;
            maximgheight = double.Parse(((maximgheight * 72) / 96).ToString());
            mximgwidth = double.Parse(((mximgwidth * 72) / 96).ToString()); ;
            MigraDoc.DocumentObjectModel.Tables.Table table = new MigraDoc.DocumentObjectModel.Tables.Table();
            Column column = new Column();
            column = table.AddColumn(mximgwidth);
            Cell cell = new Cell();
            cell.Borders.Bottom.Color = Colors.White;
            if (mximgwidth + 20 > pagewidth)
            {
                _errmsg = "image size width is greater than page width";
            }
            else
            {
                if (ds_vertical != null && ds_vertical.Tables[0] != null && ds_vertical.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds_vertical.Tables[0].Rows)
                    {
                        string barcode_img = "";
                        barcode_img = _imgpath + dr[0].ToString();
                        Row row = table.AddRow();
                        row.BottomPadding = 10;
                        row.Height = maximgheight + 5;
                        cell = row.Cells[0];
                        cell.AddImage(barcode_img);
                        //Book Name needed under barcode image
                        if (Is_bookname_need)
                        {
                            if (ds_bkname != null && ds_bkname.Tables[0] != null && ds_bkname.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow dr_bk in ds_bkname.Tables[0].Rows)
                                {
                                    if (dr[0].ToString().Equals(dr_bk["Imgname"].ToString(), StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        Book_Id = int.Parse(dr_bk["BookId"].ToString());
                                        if (Book_Id != 0)
                                        {
                                            Book_Name = MyLibMang.Get_Book_Name(Book_Id);
                                        }
                                    }
                                }
                            }
                            Row row2 = table.AddRow();
                            cell = row2.Cells[0];
                            row2.BottomPadding = 10;
                            row2.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                            cell.AddParagraph(Book_Name);
                            Book_Name = "";
                        }
                    }
                }
                else
                {
                    _errmsg = "No Barcode Data exist";
                }
            }
            return table;
        }
        //Get Barcode Images from folder
        private DataSet Get_Images(string _Img_path)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("img_files", typeof(System.String));
            DataRow dr = null;
            DirectoryInfo dir = new DirectoryInfo(_Img_path);
            if (Directory.Exists(_Img_path))
            {
                FileInfo[] sqlFiles = dir.GetFiles("*.gif");
                foreach (FileInfo fi in sqlFiles)
                {
                    dr = dt.NewRow();
                    dr[0] = fi.Name.ToString();
                    dt.Rows.Add(dr);
                }
            }
            DataSet ds_pdffiles = new DataSet();
            ds_pdffiles.Tables.Add(dt);
            return ds_pdffiles;
        }
    }
}
