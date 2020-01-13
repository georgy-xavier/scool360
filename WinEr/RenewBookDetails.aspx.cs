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
    public partial class RenewBookDetails : System.Web.UI.Page
    {
        private LibraryManagClass MyLibMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;

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
                
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadCategoryToDrp();
                }
            }
        }

        private void LoadCategoryToDrp()
        {
            Drp_SelCategory.Items.Clear();
            MyReader = MyLibMang.GetCategories();
            ListItem Li = new ListItem("All", "0");
            Drp_SelCategory.Items.Add(Li);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    Li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_SelCategory.Items.Add(Li);
                }
            }
            
        }

        protected void Btn_Show_Click(object sender, EventArgs e)
        {
            //select tblbooks.BookNo , tblbookmaster.BookName, tblbookissue.DateOfIssue, tblbookusertype.USerType from tblbookissue inner join tblbooks on tblbooks.BookNo= tblbookissue.BookNo inner join tblbookmaster on tblbookmaster.Id= tblbooks.BookId inner join tblbookusertype on tblbookusertype.Id= tblbookissue.UserType
            DataSet BooksName = MyLibMang.GetBooksFromCategoryId(int.Parse(Drp_SelCategory.SelectedValue.ToString()));
            DataSet BookDetails = new DataSet();
            DataTable dt;
            DataRow dr;

            BookDetails.Tables.Add(new DataTable("Bookdata"));
            dt = BookDetails.Tables["Bookdata"];
            dt.Columns.Add("Id");
            dt.Columns.Add("BookName");
            dt.Columns.Add("Copies");

            if (BooksName != null && BooksName.Tables != null && BooksName.Tables[0].Rows.Count > 0)
            {
                //GrdVew_Books.DataSource = BooksName;
                //GrdVew_Books.DataBind();
                foreach (DataRow dr0 in BooksName.Tables[0].Rows)
                {
                    dr = dt.NewRow();
                    dr["Id"] = dr0["Id"].ToString();
                    dr["BookName"] = dr0["BookName"].ToString();
                    string Count = MyLibMang.GetBookCount(int.Parse(dr["Id"].ToString()));
                    dr["Copies"] = Count;
                    BookDetails.Tables["Bookdata"].Rows.Add(dr);
                }
            }

        }
    }
}
