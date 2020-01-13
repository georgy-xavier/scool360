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
    public partial class BookCategoryReport : System.Web.UI.Page
    {private LibraryManagClass MyLibMang;
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
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(772))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadCategories();
                }
            }
        }

        private void LoadCategories()
        {
          
             MyReader   = MyLibMang.GetCategories();
            Drp_Category.Items.Clear();
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Category.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Categories found", "-1");
                Drp_Category.Items.Add(li);
            }
        }
        protected void Btn_ShowReport_Click(object sender, EventArgs e)
        {
            int CategoryId = -1;
            int.TryParse(Drp_Category.SelectedValue.ToString(), out CategoryId);

            LoadBasicDetails(CategoryId);
            LoadBookDetails(CategoryId);
        }

        private void LoadBookDetails(int CategoryId)
        {
            DataSet BooksName = MyLibMang.GetBooksFromCategoryId(CategoryId);

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
                GrdVew_Books.DataSource = BookDetails;
                GrdVew_Books.DataBind();
            }
            else
            {
                Lbl_CategoryErr.Text = "No items found";
                GrdVew_Books.DataSource = null;
                GrdVew_Books.DataBind();
            }

           
        }
       
        private void LoadBasicDetails(int CategoryId)
        {
            
            int TotalCount      = MyLibMang.BookCount(CategoryId);
            int TotalBookissued = MyLibMang.IssuedBooks(CategoryId);
            int TeachersIssued  = MyLibMang.IssuedBooksbyTeaschers(CategoryId);
            int StudentIssued   = MyLibMang.IssuedBooksbyStudent(CategoryId);

            Lbl_SelectedCategory.Text   = Drp_Category.SelectedItem.ToString();
            Lbl_TotalBooks.Text         = TotalCount.ToString();
            lbl_IssuedBooks.Text        = TotalBookissued.ToString();
            Lbl_TeachersIssued.Text     = TeachersIssued.ToString();
            Lbl_StudentIssuedBooks.Text = StudentIssued.ToString();

        }
    }
}
