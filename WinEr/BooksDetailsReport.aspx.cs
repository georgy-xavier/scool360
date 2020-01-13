using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Data.Odbc;
using WinBase;
using System.IO;
using System.Data;

namespace WinEr
{
    public partial class BooksDetailsReport : System.Web.UI.Page
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
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(3017))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    Pnl_BookDetails.Visible = false;
                  
                }
            }

        }
        protected void Btn_Search_OnClick(object sender, EventArgs e)
        {
            try
            {

                generate_books_data();
            }
            catch(Exception eb)
            {
                Lbl_Err.Text = eb.Message;
                GrdBooks.DataSource = null;
                GrdBooks.DataBind();
                Pnl_BookDetails.Visible = false;
            }
        }
        protected void Btn_Export_Click(object sender, EventArgs e)
        {
            DataSet Dt_books = new DataSet();
            Dt_books = (DataSet)Session["ds_books"];
            if (Dt_books.Tables[0].Rows.Count > 0)
            {

                string FileName = "BookList";
                string _ReportName = "BookList";
                if (!WinEr.ExcelUtility.ExportDataToExcel(Dt_books, _ReportName, FileName, MyUser.ExcelHeader))
                {
                    Lbl_Err.Text = "Please try again.";
                   
                }
            }
        }
        protected void GrdBooks__PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdBooks.PageIndex = e.NewPageIndex;
            generate_books_data();
        }
        private void generate_books_data()
        {
            DataSet Ds_Books = new DataSet();
            Ds_Books = BuildDataset();
            if (Ds_Books.Tables.Count > 0)
            {
                if (Ds_Books != null && Ds_Books.Tables[0] != null && Ds_Books.Tables[0].Rows.Count > 0)
                {
                    Session["ds_books"] = Ds_Books;
                    GrdBooks.DataSource = Ds_Books;
                    GrdBooks.DataBind();
                    Pnl_BookDetails.Visible = true;
                    Lbl_Err.Text = "";
                }
                else
                {
                    Lbl_Err.Text = "No books found as per the  conditions";
                    GrdBooks.DataSource = null;
                    GrdBooks.DataBind();
                    Pnl_BookDetails.Visible = false;
                }
            }
            else
            {
                Lbl_Err.Text = "No books found as per the  conditions";
                GrdBooks.DataSource = null;
                GrdBooks.DataBind();
                Pnl_BookDetails.Visible = false;
            }
        }
        private DataSet Fill_Books_Grid()
        {
            DataSet ds_books = new DataSet();
            string publisher="",author="",sql="",temp_sql="";
            publisher = Txt_publisher.Text.Trim().ToString();
            author = Txt_author.Text.Trim().ToString();
            sql = "Select tblbookmaster.Id,tblbookmaster.BookName,tblbookmaster.Author,tblbookmaster.Publisher,tblbookmaster.Edition,tblbookcatogory.CatogoryName as Category from tblbookmaster  inner join tblbookcatogory on tblbookcatogory.Id= tblbookmaster.CatagoryId ";
            if (publisher != "")
            {
                temp_sql =" where Publisher='" + publisher + "'";
            }
            if (author != "")
            {
                if (temp_sql != "")
                {
                    temp_sql = temp_sql + " AND Author='" + author + "'";
                }
                else
                {
                    temp_sql = " where Author='" + author + "'";
                }
            }
            sql = sql + temp_sql;
            ds_books = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return ds_books;
        }
        protected DataSet BuildDataset()
        {
            DataSet _ds_books = new DataSet();
            _ds_books = Fill_Books_Grid();
            DataSet _books_dataset = new DataSet();
            if (_ds_books != null && _ds_books.Tables[0] != null && _ds_books.Tables[0].Rows.Count > 0)
            {
               
                DataTable dt;
                DataRow dr;
                _books_dataset.Tables.Add(new DataTable("books"));

                dt = _books_dataset.Tables["books"];
                dt.Columns.Add("BookName");
                dt.Columns.Add("Author");
                dt.Columns.Add("Publisher");
                dt.Columns.Add("Edition");
                dt.Columns.Add("Category");
                dt.Columns.Add("Count");


                foreach (DataRow dr_values in _ds_books.Tables[0].Rows)
                {
                    int book_id = 0;
                    dr = _books_dataset.Tables["books"].NewRow();
                    book_id = int.Parse(dr_values["Id"].ToString());
                    dr["BookName"] = dr_values["BookName"];
                    dr["Author"] = dr_values["Author"];
                    dr["Publisher"] = dr_values["Publisher"];
                    dr["Edition"] = dr_values["Edition"];
                    dr["Category"] = dr_values["Category"];
                    dr["Count"] = int.Parse(Get_book_count(book_id).ToString());
                    _books_dataset.Tables["books"].Rows.Add(dr);

                }
            }


            return _books_dataset;

        }
        private int Get_book_count(int Book_Id)
        {
            string sql = "";
            int count = 0;
            sql = "Select COUNT(tblbooks.Id) as Count from tblbooks where tblbooks.BookId=" + Book_Id + "";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
          
            if (MyReader.HasRows)
            {

                while (MyReader.Read())
                {
                    count = int.Parse(MyReader.GetValue(0).ToString());
                }
            }
            return count;
        }

       
    }
}
