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

namespace WinEr
{
    public partial class AddBookCopies : System.Web.UI.Page
    {
        private LibraryManagClass MyLibMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private int BarCodeType = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            if (Session["BookId"] == null)
            {
                Response.Redirect("LlbraryHome.aspx");
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
                    string _MenuStr;
                    _MenuStr = MyLibMang.GetSubLibraryMangMenuString(MyUser.UserRoleId);
                    this.SubLibMenu.InnerHtml = _MenuStr;
                    Pnl_bookgrid.Visible = false;
                    BarcodeSection.Visible = false;

                    
                    LoadBookData();

                    
                }
            }

        }

        private void LoadBookData()
        {
           
            int BookMastrId = MyLibMang.GetBkId(Session["BookId"].ToString());
            BarCodeType = MyLibMang.get_barcodetype_copies(BookMastrId);
            string sql = "select  tblbookmaster.BookName,tblbookmaster.Barcode from tblbookmaster where tblbookmaster.Id =" + BookMastrId; ;
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if(MyReader.HasRows)
            {
                Lbl_BkName.Text = MyReader.GetValue(0).ToString();
                //previous
                //if (MyLibMang.IsBarcodeActive() && !MyLibMang.UniqueNumber() && MyReader.GetValue(1).ToString() != "0")
                //{
                //    BarcodeSection.Visible = true;
                //    Txt_Barcode.Text = MyReader.GetValue(1).ToString();
                //}
                //else
                //{
                //    BarcodeSection.Visible = false;
                //}
                    BarcodeSection.Visible = false;

            }
        }

        
        protected void Btn_Continue_Click(object sender, EventArgs e)
        {
            try
            {
                // string LastBookId,NewBookId;

            int BookMastrId = MyLibMang.GetBkId(Session["BookId"].ToString());
            BarCodeType = MyLibMang.get_barcodetype_copies(BookMastrId);
            if (BarCodeType == 1 || BarCodeType==2)
            {
                GrdBooks.Columns[2].Visible = true;
            }
            else
            {
                GrdBooks.Columns[2].Visible = false;
            }
                int _count = 1, BookId, N_BookID;
                string Count = Txt_Count.Text.Trim();
                if ((Count != "") && (int.Parse(Count) > 0))
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
                            if (BarCodeType==2)
                            {
                               TextBox Txt_GrdBarcode = (TextBox)gv.FindControl("Txt_Grd_BarCode");
                               //Txt_GrdBarcode.Text = Txt_Barcode.Text.ToString().Trim();
                                //sai changed
                               Txt_GrdBarcode.Text=MyLibMang.barcode_copies_nonunique(BookMastrId);
                                //
                               Txt_GrdBarcode.Enabled = false;
                           }
                          
                            _count++;
                        }
                    }
                    else
                    {
                        foreach (GridViewRow gv in GrdBooks.Rows)
                        {
                            
                            DropDownList Drp_rack = (DropDownList)gv.FindControl("Drp_rack");
                            AddRackIdToDrpList(0, Drp_rack);
                            TextBox Txt_GrdBarcode = (TextBox)gv.FindControl("Txt_Grd_BarCode");
                            //if (MyLibMang.IsBarcodeActive() && !MyLibMang.UniqueNumber() && Txt_Barcode.Text!= "0"&&Txt_Barcode.Text!="")
                            //{
                                
                            //    Txt_GrdBarcode.Text = Txt_Barcode.Text.ToString().Trim();
                            //    Txt_GrdBarcode.Enabled=false;
                            //}
                            //if (MyLibMang.IsBarcodeActive() && !MyLibMang.UniqueNumber() && Txt_Barcode.Text != "0" && Txt_Barcode.Text != "")
                            //{
                            //    Txt_GrdBarcode.Enabled = true; 
                            //}
                            //sai changed
                            if (BarCodeType==1)
                            {
                                Txt_GrdBarcode.Enabled = true; 
                            }
                            if (BarCodeType == 2)
                            {
                                Txt_GrdBarcode.Enabled =false;
                            }
                        }
                    }
                  
                }
                else
                {
                    Lbl_msg.Text = "Count is invalid";
                    this.MPE_MessageBox.Show();
                    Pnl_bookgrid.Visible = false;
                   

                }

            }
        
            catch(Exception et)
            {
                Lbl_msg.Visible=true;
                Lbl_msg.Text=et.Message;
                //Pnl_bookgrid.Visible = false;
                //Btn_save.Enabled = false;

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

            int _count = int.Parse(Txt_Count.Text);

            for (i = 0; i < _count; i++)
            {

                dr = _bookdataset.Tables["books"].NewRow();

                dr["Count"] = i + 1;

                _bookdataset.Tables["books"].Rows.Add(dr);

            }


            return _bookdataset;

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

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                Btn_Save.Enabled = false;
                string message, _message;
                int BookId;
                int BarCodeType = 0;
                int BookMastrId = 0;
                if (validData(out message))
                {
                    BookId = MyLibMang.GetBkId(Session["BookId"].ToString());
                    if (BookId != -1)
                    {
                        if (ValidGridData(out _message))
                        {
                            //sai added for get barcode type
                            BookMastrId = MyLibMang.GetBkId(Session["BookId"].ToString());
                            BarCodeType = MyLibMang.get_barcodetype_copies(BookMastrId);
                            //
                            EnterBook(BookId, BarCodeType);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "Added book copies", 1);
                            Lbl_msg.Text = "Book(s) saved";
                            this.MPE_MessageBox.Show();
                            GrdBooks.DataSource = null;
                            GrdBooks.DataBind();
                            Pnl_bookgrid.Visible = false;

                        }
                        else
                        {
                            Lbl_msg.Text = _message;
                            this.MPE_MessageBox.Show();
                        }
                    }
                }
                else
                {
                    Lbl_msg.Text = message;
                    this.MPE_MessageBox.Show();
                }
                Btn_Save.Enabled = true;
            }
            catch (Exception rt)
            {
                Lbl_msg.Visible = true;
                Lbl_msg.Text = rt.Message;
            }
        }
    
        private bool validData(out string message)
        {
            bool _valid = true;
            message = "";
  
            if (Txt_Count.Text.Trim() == "" || Txt_Count.Text == "0")
            {
                _valid = false;
                message = "Book count is invalid";
              
            }
            return _valid;
        }

        private bool ValidGridData(out string _Message)
        {
            bool _valid = true;
            _Message = "";
            int _gridcount = GrdBooks.Rows.Count;
            int BookMastrId = MyLibMang.GetBkId(Session["BookId"].ToString());
            BarCodeType = MyLibMang.get_barcodetype_copies(BookMastrId);
            string[] BookId = new string[_gridcount];
            string[] BookBarCode = new string[_gridcount];
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

                    if (BarCodeType == 1 || BarCodeType==2)
                    {
                        TextBox Txt_GrdBarcode = (TextBox)gv.FindControl("Txt_Grd_BarCode");
                       
                        if (Txt_GrdBarcode.Text.Trim() == "")
                        {
                            _valid = false;
                            _Message = "Barcode cannot be empty at Book Id:" + Txt_BookId.Text.ToString()+ "";
                            break;
                        }
                            //sai added for barcode text validations
                        else if (MyLibMang.get_barcodemincount() > Txt_GrdBarcode.Text.ToString().Length)
                        {
                            _valid = false;
                            _Message = "Barcode  Text:" + Txt_GrdBarcode.Text.ToString() + " is lessthan Minium Barcode Text";
                        }
                        else if (MyLibMang.get_barcodemaxcount() < Txt_GrdBarcode.Text.ToString().Length)
                        {
                            _valid = false;
                            _Message = "Barcode  Text:" + Txt_GrdBarcode.Text.ToString() + " is Greaterthan Maxium Barcode Text";
                        }
                        else
                        {
                            if (BarCodeType == 1)
                            {
                                BookBarCode[_i] = Txt_GrdBarcode.Text;
                                if (MyLibMang.UniqueNumber())
                                {

                                    if (MyLibMang.isBarcodeExist(Txt_GrdBarcode.Text.ToString().Trim(), 0, ""))
                                    {
                                        _Message = "" + Txt_GrdBarcode.Text.ToString() + " Barcode is already exist";
                                        _valid = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (MyLibMang.isBarcodeExist(Txt_GrdBarcode.Text.ToString().Trim(), BookMastrId, Lbl_BkName.Text))
                                    {
                                        _Message = ""+ Txt_GrdBarcode.Text.ToString() +" Barcode is already exist";
                                        _valid = false;
                                        break;
                                    }
                                }
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
                                    _Message = "BookId Cannot be repeated,Ids are " + BookId[_j].ToString() + " and " + BookId[_k] .ToString()+ " ";

                                    _valid = false;
                                    break;
                                }
                            }
                        }
                    
                }
                if (_valid && BarCodeType==1)
                {

                    for (_j = 0; _j < _gridcount; _j++)
                    {
                        for (_k = _j + 1; _k < _gridcount; _k++)
                        {
                            if (BookBarCode[_j].CompareTo(BookBarCode[_k]) == 0)
                            {
                                _Message = "BarCode Cannot be repeated,Barcodes are " + BookBarCode[_j] + " and " + BookBarCode[_k].ToString()+ "";

                                _valid = false;
                                break;
                            }
                        }
                    }
                    
                }
                if (_valid && BarCodeType==2)
                {

                    for (_j = 0; _j < _gridcount; _j++)
                    {
                        for (_k = _j + 1; _k < _gridcount; _k++)
                        {
                            if (BookBarCode[_j].CompareTo(BookBarCode[_k]) != 0)
                            {
                                _Message = "BarCode should be same";

                                _valid = false;
                                break;
                            }
                        }
                    }

                }
            }
            catch
            {
                //_Message = "Sorry,Please try again";
                //_valid = false;

            }
            return _valid;
        }

        private void EnterBook(int BookId,int barcode_type)
        {
            int rack = -1;
            int _BookId;
            string Barcode="0";
            string err_msg="";
            int BookMastrId = MyLibMang.GetBkId(Session["BookId"].ToString());
            foreach (GridViewRow gv in GrdBooks.Rows)
            {
                TextBox Txt_BookId = (TextBox)gv.FindControl("Txt_BookId");
                DropDownList Drp_rack = (DropDownList)gv.FindControl("Drp_rack");

                if (Drp_rack.SelectedItem.ToString() != "")
                {
                    rack = int.Parse(Drp_rack.SelectedValue.ToString());
                }
                //dominic
                //if (MyLibMang.IsBarcodeActive() && ! MyLibMang.Isbarcodetype_automatic())
                //{
                //sai added for insert barcode details based on barcode type
                if (barcode_type == 1 || barcode_type == 2)
                {
                    TextBox Txt_Barcode = (TextBox)gv.FindControl("Txt_Grd_BarCode");
                    Barcode = Txt_Barcode.Text.ToString().Trim();
                }
                _BookId = MyLibMang.AddBooks(int.Parse(Txt_BookId.Text.ToString()),BookId, Txt_BookId.Text, rack, Barcode);
                if (barcode_type == 3 || barcode_type == 4)
                {
                    if (barcode_type == 3)
                        {
                           Barcode = MyLibMang.generate_Br_input_add_copies(int.Parse(Txt_BookId.Text.ToString()), out err_msg);
                          
                        }
                     if (barcode_type == 4)
                        {
                            Barcode = MyLibMang.barcode_copies_nonunique(BookMastrId);
                        }
                     MyLibMang.AddBookcopies_Automatic(int.Parse(Txt_BookId.Text.ToString()), Barcode, barcode_type);
                }
            }

        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("BookSearchDetails.aspx");
        }
    }
}
