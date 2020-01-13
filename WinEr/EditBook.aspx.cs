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
    public partial class EditBook : System.Web.UI.Page
    {
        private LibraryManagClass MyLibMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
       
        //  private DataSet MydataSet;
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
                    LoadCategoryToDrpList();
                    LoadTypeToDrpList();
                    AddRackToDrpList();
                    loadBookData();
                }
            }

        }

        private void AddRackToDrpList()
        {
            Drp_Rack.Items.Clear();
            string sql = "SELECT Id,RackName FROM tblbookrack";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {

                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Rack.Items.Add(li);
                }
                
            }
            else
            {
                Btn_BkUpdate.Enabled = false;
                Lbl_msg.Text = "Add Racks before updating books";
                this.MPE_MessageBox.Show();
            }
        }

        private void LoadTypeToDrpList()
        {
            Drp_type.Items.Clear();
            string sql = "SELECT Id,TypeName FROM tblbooktype order by TypeName ASC";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_BkUpdate.Enabled = true;
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_type.Items.Add(li);
                }
            }
            else
            {
                Btn_BkUpdate.Enabled = false;
                Lbl_msg.Text = "Add books type before updating books";
                this.MPE_MessageBox.Show();
            }
        }

        private void LoadCategoryToDrpList()
        {
            Drp_catagory.Items.Clear();
            string sql = "SELECT Id,CatogoryName FROM tblbookcatogory order by CatogoryName ASC";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_BkUpdate.Enabled = true;
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_catagory.Items.Add(li);
                }
            }
            else
            {

                Btn_BkUpdate.Enabled = false;
                Lbl_msg.Text = "Add books category before updating books";
                this.MPE_MessageBox.Show();
            }
            
        }

        private void loadBookData()
        {
            int _Year;
            string Price;
            int barcode_type = 0;
            //BarCodeField.Visible = true;
            //string sql = "select tblbookmaster.BookName , tblbookmaster.Author, tblbookmaster.Edition, tblbookmaster.Publisher , tblbookmaster.`Year`, tblbookcatogory.CatogoryName , tblbooktype.TypeName from tblbookmaster inner join tblbookcatogory on tblbookcatogory.Id = tblbookmaster.CatagoryId inner join tblbooktype on tblbooktype.Id = tblbookmaster.TypeId inner join tblbooks on tblbooks.BookId =  tblbookmaster.Id where tblbooks.BookNo='" + Session["BookId"] + "'";
            string sql = "select tblbookmaster.BookName , tblbookmaster.Author, tblbookmaster.Edition, tblbookmaster.Publisher , tblbookmaster.`Year`, tblbookmaster.CatagoryId , tblbookmaster.TypeId, tblbooks.RackId ,tblbookmaster.Cost,tblbooks.Barcode,tblbookmaster.BarcodeType,tblbookmaster.Bookslno from tblbookmaster  inner join tblbooks on tblbooks.BookId =  tblbookmaster.Id where tblbooks.BookNo='" + Session["BookId"] + "'";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    Txt_name.Text = MyReader.GetValue(0).ToString();
                    Txt_auther.Text = MyReader.GetValue(1).ToString();
                    Txt_edition.Text = MyReader.GetValue(2).ToString();
                    Txt_Publisher.Text = MyReader.GetValue(3).ToString();
                   // Txt_year.Text = MyReader.GetValue(4).ToString();
                    _Year = int.Parse(MyReader.GetValue(4).ToString());
                    if (_Year == -1)
                    {
                        Txt_year.Text = "";
                    }
                    else
                    {
                        Txt_year.Text = _Year.ToString();
                    }
                    Price = MyReader.GetValue(8).ToString();
                    if (Price == "-1")
                    {
                        Txt_Price.Text = "";
                    }
                    else
                    {
                        Txt_Price.Text = Price.ToString();
                    }
                    //Drp_catagory.SelectedItem.Text = MyReader.GetValue(5).ToString();
                   // Drp_type.SelectedItem.Text = MyReader.GetValue(6).ToString();
                    Drp_catagory.SelectedValue = MyReader.GetValue(5).ToString();
                    Drp_type.SelectedValue = MyReader.GetValue(6).ToString();
                    Drp_Rack.SelectedValue = MyReader.GetValue(7).ToString();
                    Txt_Barcode.Text = MyReader.GetValue(9).ToString();
                    txt_bookslno.Text = MyReader.GetValue(11).ToString();
                    int.TryParse(MyReader.GetValue(10).ToString(),out barcode_type);
                    //if (MyLibMang.IsBarcodeActive())
                    //{
                    Session["Barcode_type"] = barcode_type;
                        //if (!MyLibMang.UniqueNumber() && !MyLibMang.Isbarcodetype_automatic())
                        if (barcode_type==2)
                        {
                            BarCodeField.Visible =true;
                            Txt_Barcode.Enabled = true;
                        }
                        else
                        {
                            BarCodeField.Visible = false;
                            Txt_Barcode.Enabled = false;
                        }
                    //}
                    //else
                    //{
                    //    BarCodeField.Visible = false;
                    //}
                    
                }
            }
        }
        protected void Btn_BkCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("BookSearchDetails.aspx");
        }
        protected void Btn_BkUpdate_Click(object sender, EventArgs e)
        {
            string message;
            int BookId = 0;
            int BkId = 0;
            int year=-1;
            double Price=-1;
            if (validData(out message))
            {
                //BarCodeField.Visible = true;
                if (Txt_year.Text != "")
                {
                    year = int.Parse(Txt_year.Text.Trim());
                }
                if (Txt_Price.Text !="")
                {
                    Price = double.Parse(Txt_Price.Text.Trim());
                }
                BkId = MyLibMang.GetbookId(Session["BookId"].ToString());
                if (BkId!=0)
                {
                    BookId = MyLibMang.UpdateBooks(Txt_name.Text, Txt_auther.Text, Txt_Publisher.Text, year, Txt_edition.Text, int.Parse(Drp_type.SelectedValue.ToString()), int.Parse(Drp_catagory.SelectedValue.ToString()), BkId, int.Parse(Drp_Rack.SelectedValue.ToString()), Session["BookId"].ToString(), Price, Txt_Barcode.Text.Trim(), int.Parse(Session["Barcode_type"].ToString()),txt_bookslno.Text);
                    if (BookId == 1)
                    {
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "" + Txt_name.Text + " Book Details updated", 1);
                        Lbl_msg.Text = "Book is updated";
                        this.MPE_MessageBox.Show();
                    }
                }
                if (int.Parse(Session["Barcode_type"].ToString()) == 2)
                {
                    BarCodeField.Visible = true;
                }
                else
                {
                    BarCodeField.Visible = false;
                }
            }
            else
            {
                Lbl_msg.Text = message;
                this.MPE_MessageBox.Show();
            }
        }
        private bool validData(out string message)
        {
            bool _valid = true;
            int BkId = MyLibMang.GetbookId(Session["BookId"].ToString());
            message = "";
            if (Txt_name.Text.Trim() == "" || Txt_auther.Text.Trim() == "" || Txt_Publisher.Text.Trim() == "" || Drp_catagory.SelectedItem.ToString() == "" || Drp_type.SelectedItem.ToString() == "")
            {
                _valid = false;
                message = "One or more fields are empty";
            }

            else if (int.Parse(Session["Barcode_type"].ToString()) == 2)
            {
                if (Txt_Barcode.Text == "0" || Txt_Barcode.Text.Trim() == "")
                {

                    _valid = false;
                    message = "One or more fields are empty";

                }
                else if (MyLibMang.BarcodeExistinMaster(Txt_Barcode.Text.ToString()) || MyLibMang.BarcodeExistinCopies(Txt_Barcode.Text.ToString()))
                {
                    message = "" + Txt_Barcode.Text.ToString() + " Barcode is already exist";
                    _valid = false;

                }
                else if (MyLibMang.get_barcodemincount() > Txt_Barcode.Text.ToString().Length)
                {
                    message = "Barcode Text:" + Txt_Barcode.Text.ToString() + " is lessthan Minium Barcode Text";
                    _valid = false;
                }
                else if (MyLibMang.get_barcodemaxcount() < Txt_Barcode.Text.ToString().Length)
                {
                    message = "Barcode Text:" + Txt_Barcode.Text.ToString() + " is Greaterthan Maxium Barcode Text";
                    _valid = false;
                }

            }
            return _valid;
        }
    }
}
