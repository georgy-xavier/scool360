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
    public partial class BookSearchDetails : System.Web.UI.Page
    {

        private LibraryManagClass MyLibMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
      //  private  DataSet MydataSet;
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
                    
                    loadBookData();
                }
            }

        }

        private void loadBookData()
        {
            int _Year;
            string Price;
            string sql = "select tblbookmaster.BookName , tblbookmaster.Author, tblbookmaster.Edition, tblbookmaster.Publisher , tblbookmaster.`Year`, tblbookcatogory.CatogoryName , tblbooktype.TypeName , tblbookrack.RackName , tblbookmaster.Cost , tblbooks.BookNo , tblbooks.Barcode , tblbookmaster.Bookslno from tblbookmaster inner join tblbookcatogory on tblbookcatogory.Id = tblbookmaster.CatagoryId inner join tblbooktype on tblbooktype.Id = tblbookmaster.TypeId inner join tblbooks on tblbooks.BookId =  tblbookmaster.Id inner join tblbookrack on tblbookrack.Id = tblbooks.RackId where tblbooks.BookNo='" + Session["BookId"] + "'";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                   Lbl_BkName.Text = MyReader.GetValue(0).ToString();
                    Lbl_AuthorName.Text = MyReader.GetValue(1).ToString();
                    Lbl_Edition.Text = MyReader.GetValue(2).ToString();
                    Lbl_Publisher.Text = MyReader.GetValue(3).ToString();
                   // Txt_year.Text = MyReader.GetValue(4).ToString();
                    _Year = int.Parse(MyReader.GetValue(4).ToString());
                    Price = MyReader.GetValue(8).ToString();
                    Lbl_BNo.Text = MyReader.GetValue(9).ToString();
                    lbl_Bbarcode.Text = MyReader.GetValue(10).ToString();
                    lbl_bookslno.Text = MyReader.GetValue(11).ToString();
                    if (_Year == -1)
                    {
                        Lbl_Year.Text = "";
                    }
                    else
                    {
                        Lbl_Year.Text = _Year.ToString();
                    }
                    if (Price == "-1")
                    {
                        Lbl_Price.Text = "";
                    }
                    else
                    {
                        Lbl_Price.Text = Price.ToString();
                    }
                    Lbl_Category.Text = MyReader.GetValue(5).ToString();
                    Lbl_Type.Text = MyReader.GetValue(6).ToString();
                    Lbl_RackNo.Text = MyReader.GetValue(7).ToString();
                }
            }

            sql = "select count(tblbooks.Id) from tblbooks where tblbooks.BookId in (select tblbooks.BookId from tblbooks where tblbooks.BookNo='" + Session["BookId"] + "')";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Lbl_Count.Text = MyReader.GetValue(0).ToString();
            }
        }
    }
}
