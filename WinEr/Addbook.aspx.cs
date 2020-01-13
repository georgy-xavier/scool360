using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.Odbc;
using WinBase;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.IO;
namespace WinEr
{
    public partial class Addbook : System.Web.UI.Page
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
                    Pnl_bookgrid.Visible = false;
                   
                    LoadBookCategoryToDrpList(0);
                    LoadBookType(0);
                    LoadBarcode();
                    
                }
            }
        }
        private void LoadBarcode()
        {
            if (MyLibMang.IsBarcodeActive() && !MyLibMang.UniqueNumber() && !MyLibMang.Isbarcodetype_automatic())
            {
                BarCodeField.Visible = true;
            }
            else
            {
                BarCodeField.Visible = false;
            }
            //else if (MyLibMang.IsBarcodeActive() && MyLibMang.UniqueNumber())
            //{
            //    BarCodeField.Visible = false;
            //}
            //else if (MyLibMang.IsBarcodeActive() && MyLibMang.Isbarcodetype_automatic())
            //{
            //    BarCodeField.Visible = false;
            //}
            //else if (!MyLibMang.IsBarcodeActive())
            //{
            //    BarCodeField.Visible = false;
            //}

        }
        private void LoadBookType(int _intex)
        {
            Drp_type.Items.Clear();
            string sql = "SELECT Id,TypeName FROM tblbooktype order by TypeName ASC";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_Continue.Enabled = true;
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_type.Items.Add(li);
                }
                Drp_type.SelectedIndex = _intex;
            }
            else
            {
                Btn_Continue.Enabled = false;
                Lbl_msg.Text = "Add books type before entering books";
                this.MPE_MessageBox.Show();
            }
        }

        private void LoadBookCategoryToDrpList(int _intex)
        {
           Drp_catagory.Items.Clear();
           string sql = "SELECT Id,CatogoryName FROM tblbookcatogory order by CatogoryName ASC";
           MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
           if (MyReader.HasRows)
           {
               
               while (MyReader.Read())
               {
                   ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                   Drp_catagory.Items.Add(li);
               }
               Drp_catagory.SelectedIndex = _intex;
           }
           else
           {
              
               Lbl_msg.Text = "Add books category before entering books";
               this.MPE_MessageBox.Show();
           }
        }

        protected DataSet BuildDataset()
        {

            DataSet _bookdataset = new DataSet();

            DataTable dt;

            DataRow dr;

            int i;

            _bookdataset.Tables.Add(new DataTable("books"));

            dt = _bookdataset.Tables["books"];

            dt.Columns.Add("Count");

            int _count = int.Parse(Txt_count.Text);

            for (i = 0; i < _count; i++)
            {

                dr = _bookdataset.Tables["books"].NewRow();

                dr["Count"] = i + 1;

                _bookdataset.Tables["books"].Rows.Add(dr);
               
            }

           
            return _bookdataset;

        }

        protected void Btn_Continue_Click(object sender, EventArgs e)
        {
            Lbl_Message.Text = "";
            try
            {
               // string LastBookId,NewBookId;
                int _count = 1,BookId,N_BookID;
                if (!MyLibMang.IsBarcodeActive())
                {
                    GrdBooks.Columns[2].Visible = false;
                }
                else if (MyLibMang.IsBarcodeActive() && !MyLibMang.Isbarcodetype_automatic())
                {
                    GrdBooks.Columns[2].Visible = true;
                }
                else if (MyLibMang.Isbarcodetype_automatic())
                {
                    GrdBooks.Columns[2].Visible = false;
                }
                if (validentries())
                {
                    GrdBooks.DataSource = BuildDataset();
                    GrdBooks.DataBind();
                    Pnl_bookgrid.Visible = true;
                    if (MyLibMang.AutoBookId())
                    {
                        BookId = MyLibMang.GetlastBookId();
                        foreach (GridViewRow gv in GrdBooks.Rows)
                        {
                            TextBox Txt_BookId = (TextBox)gv.FindControl("Txt_BookId");
                            DropDownList Drp_rack = (DropDownList)gv.FindControl("Drp_rack");
                           
                            N_BookID = BookId + _count;
                            Txt_BookId.Text = N_BookID.ToString();
                            AddRackIdToDrpList(0, Drp_rack);
                            Txt_BookId.Enabled = false;
                            _count++;
                            //get book ids here for auto bookid

                            //
                            if (MyLibMang.IsBarcodeActive() && !MyLibMang.UniqueNumber())
                            {
                                TextBox Txt_GrdBarCode = (TextBox)gv.FindControl("Txt_Grd_Barcode");
                                Txt_GrdBarCode.Text = Txt_Barcode.Text.ToString().Trim();
                                Txt_GrdBarCode.Enabled = false;
                                //GrdBooks.Columns[2].Visible = false;
                            }

                        }

                    }
                    else
                    {
                        foreach (GridViewRow gv in GrdBooks.Rows)
                        {
                            DropDownList Drp_rack = (DropDownList)gv.FindControl("Drp_rack");
                            AddRackIdToDrpList(0, Drp_rack);
                            if (MyLibMang.IsBarcodeActive() && !MyLibMang.UniqueNumber())
                            {
                                TextBox Txt_GrdBarCode = (TextBox)gv.FindControl("Txt_Grd_Barcode");
                                Txt_GrdBarCode.Text = Txt_Barcode.Text.ToString().Trim();
                                Txt_GrdBarCode.Enabled = false;
                                //GrdBooks.Columns[2].Visible = false;
                            }
                        }
                    }

                }
                else
                {
                    Pnl_bookgrid.Visible = false;
                }

            }
            catch 
            {
                Pnl_bookgrid.Visible = false;
            }
        }

        private bool validentries()
        {
            bool valid = true;
            if (Txt_name.Text != "")
            {
                string sql = "select tblbookmaster.BookName from tblbookmaster where tblbookmaster.BookName='"+Txt_name.Text+"'";
                MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Lbl_Message.Text = "Book Name is already exist";
                
                    valid = false;
                }
            }
            else if (Txt_count.Text.Trim() == "" || Txt_count.Text == "0")
            {
                Lbl_Message.Text = "Count should be one or more than one";
                
                valid= false;
            }
            else if (MyLibMang.IsBarcodeActive() && !MyLibMang.UniqueNumber() && !!MyLibMang.Isbarcodetype_automatic())
            {
                string _Barcode = Txt_Barcode.Text.ToString().Trim();
                if (_Barcode == "")
                {
                    Lbl_Message.Text = "Enter barcode";
                    valid = false;
                }
                else if (MyLibMang.BarcodeExistinMaster(_Barcode) || MyLibMang.BarcodeExistinCopies(_Barcode))
                {
                    Lbl_Message.Text = "" + _Barcode + " Barcode is already exist";
                    valid = false;
                }
            }
            else if (MyLibMang.get_barcodemincount() > Txt_Barcode.Text.ToString().Length)
            {
                Lbl_Message.Text = "Barcode Text:" + Txt_Barcode.Text.ToString() + " is lessthan Minium Barcode Text";
                valid = false;
            }
            else if (MyLibMang.get_barcodemaxcount() < Txt_Barcode.Text.ToString().Length)
            {
                Lbl_Message.Text = "Barcode Text:" + Txt_Barcode.Text.ToString() + " is Greaterthan Maxium Barcode Text";
                valid = false;
            }
            else if (int.Parse(Txt_count.Text) <= 0)
            {
                Lbl_Message.Text = "Enter book count";
                valid = false;
            }

            return valid;
        }

        private void AddRackIdToDrpList(int _intex, DropDownList Drp_rack)
        {
           Drp_rack.Items.Clear();
           string sql = "SELECT Id,RackName FROM tblbookrack";
           MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
           if (MyReader.HasRows)
           {

               while (MyReader.Read())
               {
                   ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                   Drp_rack.Items.Add(li);
               }
               Drp_rack.SelectedIndex = _intex;
           }
        }

        

        protected void Btn_save_Click(object sender, EventArgs e)
        {
            Btn_save.Enabled = false;
            string message, _message;
            string _err_msg = "";
            int year = -1, BookId = 0;
            double price = -1;
            int BarCodeType = 0;
            string Barcode = "0";

            //if (MyLibMang.IsBarcodeActive() && !MyLibMang.UniqueNumber() && !MyLibMang.Isbarcodetype_automatic())
            //{
            //previous

            //if (MyLibMang.IsBarcodeActive() && !MyLibMang.Isbarcodetype_automatic())
            //{
            //    BarCodeType = 1;
            //}
            //else if (MyLibMang.IsBarcodeActive() && MyLibMang.UniqueNumber() && MyLibMang.Isbarcodetype_automatic())
            //{
            //    BarCodeType = 2;
            //}
            //else if(!MyLibMang.IsBarcodeActive())
            //{
            //    BarCodeType = 0;
            //}
             
            //
            //change
             if(!MyLibMang.IsBarcodeActive())
             {
                 BarCodeType = 0;
             }
             else if (MyLibMang.IsBarcodeActive() && MyLibMang.UniqueNumber() && !MyLibMang.Isbarcodetype_automatic())
             {
                 BarCodeType = 1;
             }
             else if (MyLibMang.IsBarcodeActive() && !MyLibMang.UniqueNumber() && !MyLibMang.Isbarcodetype_automatic())
             {
                 BarCodeType = 2;
             }
             else if (MyLibMang.IsBarcodeActive() && MyLibMang.UniqueNumber() && MyLibMang.Isbarcodetype_automatic())
             {
                 BarCodeType = 3;
             }
             else if (MyLibMang.IsBarcodeActive() && !MyLibMang.UniqueNumber() && MyLibMang.Isbarcodetype_automatic())
             {
                 BarCodeType = 4;
             }
            //
            if (validData(out message))
            {
                try
                {

                    if (ValidGridData(out _message))
                    {
                        if (Txt_year.Text != "")
                        {
                            int.TryParse(Txt_year.Text.ToString().Trim(), out year);
                        }
                        if (Txt_Price.Text != "")
                        {
                            double.TryParse(Txt_Price.Text.ToString().Trim(), out price);
                        }
                        if (BarCodeType == 2)
                        {
                            Barcode = Txt_Barcode.Text.ToString().Trim();
                        }
                        BookId = MyLibMang.AddBookMaster(Txt_name.Text, Txt_auther.Text, Txt_Publisher.Text, year, Txt_edition.Text, int.Parse(Drp_type.SelectedValue.ToString()), int.Parse(Drp_catagory.SelectedValue.ToString()), Txt_count.Text, price, Barcode, BarCodeType, Txt_bookslno.Text);
                        EnterBook(BookId, BarCodeType, out _err_msg);
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "" + Txt_name.Text + " Book added", 1);
                        //Lbl_msg.Text = "Book(s) are saved";
                        // this.MPE_MessageBox.Show();
                        GrdBooks.DataSource = null;
                        GrdBooks.DataBind();
                        Pnl_bookgrid.Visible = false;
                        Clear();
                        
                        Lbl_Message.Text = "The book is saved";

                    }

                    else
                    {
                        Lbl_msg.Text = _message;
                        this.MPE_MessageBox.Show();
                    }
                }
                catch (Exception df)
                {
                    Lbl_Message.Text = df.Message;
                }
                
            }
            else
            {
                Lbl_msg.Text = message;
                this.MPE_MessageBox.Show();
            }
            Btn_save.Enabled = true;
        }

        private void Clear()
        {
            Txt_auther.Text = "";
            Txt_name.Text = "";
            Txt_Publisher.Text = "";
            Txt_year.Text = "";
            Txt_edition.Text = "";
            Txt_count.Text = "";
            Txt_Price.Text = "";
            Txt_Barcode.Text = "";
        }
        private bool ValidGridData(out string _Message)
        {
            bool _valid = true;
            _Message = "";
            int _gridcount = GrdBooks.Rows.Count;
            string[] BookId = new string[_gridcount];
             string[] BarCodeArray=new string [_gridcount];
            int _i = 0, _j, _k;
            try
            {
                foreach (GridViewRow gv in GrdBooks.Rows)
                 {
                    TextBox Txt_BookId = (TextBox)gv.FindControl("Txt_BookId");
                    DropDownList Drp_rack = (DropDownList)gv.FindControl("Drp_rack");
                    if (Txt_BookId.Text.Trim() == "")
                    {
                        _valid = false;
                        _Message = "BookId cannot be empty";
                       
                        break;
                    }
                    else if (Drp_rack.Items.Count==0)
                    {
                        _valid = false;
                        _Message = "Rack number cannot be empty";
                        
                        break;
                    }
                    else if (Drp_rack.SelectedItem.ToString() == "")
                    {
                        _valid = false;
                        _Message = "Rack number cannot be empty";
                       
                        break;
                    }

                    if (!MyLibMang.BookIdExists(int.Parse(Txt_BookId.Text)))
                    {
                        _valid = false;
                        _Message = "The Book Id '" + Txt_BookId.Text + "' already exists. Please enter another one";

                        break;
                    }
                    else
                    {
                        BookId[_i] = Txt_BookId.Text.ToString();
                    }
                    if (MyLibMang.IsBarcodeActive() && !MyLibMang.Isbarcodetype_automatic())
                    {
                        TextBox Txt_ValidBarCode = (TextBox)gv.FindControl("Txt_Grd_Barcode");
                        string Barcode = Txt_ValidBarCode.Text.ToString().Trim();

                        if (Txt_ValidBarCode.Text.Trim() == "")
                        {
                            _valid = false;
                            _Message = "Barcode cannot be empty";

                            break;
                        }
                        else if (MyLibMang.get_barcodemincount() > Txt_ValidBarCode.Text.ToString().Length)
                        {
                            _valid = false;
                            _Message = "Barcode  Text:" + Txt_ValidBarCode.Text.ToString() + " is lessthan Minium Barcode Text";
                        }
                        else if (MyLibMang.get_barcodemaxcount() < Txt_ValidBarCode.Text.ToString().Length)
                        {
                            _valid = false;
                            _Message = "Barcode  Text:" + Txt_ValidBarCode.Text.ToString() + " is Greaterthan Maxium Barcode Text";
                        }
                        else
                        {
                            BarCodeArray[_i] = Barcode;
                            if (MyLibMang.isBarcodeExist(Barcode, 0, ""))
                            {

                                _valid = false;
                                _Message = "" + Barcode + " Barcode is already exist";


                            }
                        }


                    }
                   
                    _i++;
                }
                if (_valid)
                {
                        for (_j = 0; _j < _gridcount; _j++)
                        {
                            for (_k = _j + 1; _k < _gridcount; _k++)
                            {
                                if (BookId[_j].CompareTo(BookId[_k]) == 0)
                                {
                                    _Message = "BookId Cannot be repeated,Ids are:" + BookId[_j].ToString() + " and " + BookId[_k] .ToString()+ "";

                                    _valid = false;
                                    break;

                                }
                            }
                        }

                }
                if (_valid && MyLibMang.UniqueNumber())
                {
                       for (_j = 0; _j < _gridcount; _j++)
                        {
                            for (_k = _j + 1; _k < _gridcount; _k++)
                            {
                                if (BarCodeArray[_j].CompareTo(BarCodeArray[_k]) == 0)
                                {
                                    _Message = "Bar-code Cannot be repeated,Barcodes are "+ BarCodeArray[_j].ToString() +" and "+ BarCodeArray[_k].ToString() +"";
                                    _valid = false;
                                    break;
                                }
                            }
                        }
                }
            }
            catch
            {

            }
            return _valid;
        }



        private void EnterBook(int BookId, int BarCodeType, out string _errmsg)
        {//dominic check the values and read the text bok values and pass to next function
            
            int rack=-1;
            int _BookId;
            string Barcode_Manual = "0";
            string Barcode_Auto = "0";
            int Bookid_Auto = 0;
            _errmsg="";
            DataSet ds_save_barcodetext = new DataSet();
            foreach (GridViewRow gv in GrdBooks.Rows)
            {
                TextBox Txt_BookId = (TextBox)gv.FindControl("Txt_BookId");
                TextBox Txt_BarCode = (TextBox)gv.FindControl("Txt_Grd_Barcode");
                DropDownList Drp_rack = (DropDownList)gv.FindControl("Drp_rack");

                if (Drp_rack.SelectedItem.ToString() != "")
                {
                    rack = int.Parse(Drp_rack.SelectedValue.ToString());
                }
                if (BarCodeType == 1 || BarCodeType == 2)
                {
                    Barcode_Manual = Txt_BarCode.Text.ToString().Trim();
                }
                _BookId = MyLibMang.AddBooks(int.Parse(Txt_BookId.Text.ToString()),BookId, Txt_BookId.Text.ToString().Trim(), rack, Barcode_Manual);

            }
            if (BarCodeType == 3 || BarCodeType == 4)
            {
                ds_save_barcodetext = MyLibMang.Generate_barcodeinput(BookId, out _errmsg);
                if (ds_save_barcodetext != null && ds_save_barcodetext.Tables[0] != null && ds_save_barcodetext.Tables[0].Rows.Count > 0)
                {
                    if (BarCodeType == 3)
                    {
                        foreach (DataRow dr_bar in ds_save_barcodetext.Tables[0].Rows)
                        {
                            Bookid_Auto = int.Parse(dr_bar["Bookid"].ToString());
                            Barcode_Auto = dr_bar["Barcodetext"].ToString();
                            MyLibMang.AddBooks_Automatic(Bookid_Auto, Barcode_Auto);

                        }
                    }
                    if (BarCodeType == 4)
                    {
                        string change_br = "";
                        MyLibMang.Addbarcode_bookmaster(BookId, ds_save_barcodetext.Tables[0].Rows[0]["Barcodetext"].ToString(),out change_br);
                        MyLibMang.Update_Barcode_automatic(BookId,change_br);
                    }
                }
                else
                {
                    _errmsg = "Barcode Input Text Not Generated";
                }
            }
        }

        private bool validData(out string message)
        {
            bool _valid = true;
            message = "";
            if (Txt_name.Text.Trim() == "" || Txt_auther.Text.Trim() == "" || Txt_Publisher.Text.Trim() == "" || Drp_catagory.SelectedItem.ToString() == "" || Drp_type.SelectedItem.ToString() == "")
            {
                _valid = false;
                message = "One or more fields are empty";
                
            }
            else if(Txt_count.Text.Trim() == "" || Txt_count.Text == "0" )
            {
                _valid = false;
                message = "Book count is invalid";
                
            }
            else if (BarCodeField.Visible == true)
            {
                if (Txt_Barcode.Text.Trim() == "")
                {
                    _valid = false;
                    message = "Barcode cannot be empty";
                }
                else if (MyLibMang.get_barcodemincount() > Txt_Barcode.Text.ToString().Length)
                {
                    _valid = false;
                    message = "Barcode  Text is lessthan Minium Barcode Text";
                }
                else if (MyLibMang.get_barcodemaxcount() < Txt_Barcode.Text.ToString().Length)
                {
                    _valid = false;
                    message = "Barcode  Text is Greaterthan Maxium Barcode Text";
                }

            }
           
            return _valid;
        }

        protected void Drp_type_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
  
    }
}
