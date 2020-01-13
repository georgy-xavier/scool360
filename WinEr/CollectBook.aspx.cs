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
    public partial class CollectBook : System.Web.UI.Page
    {
        private LibraryManagClass MyLibMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;

        protected void Page_Load(object sender, EventArgs e)
        {
            WC_bookcopies.EventSelection += new EventHandler(OnItemSelected);
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
                    Txt_bookid_AutoCompleteExtender.ContextKey = "0";
                    LoadSearchDrp();

                }
            }

        }

        private void LoadSearchDrp()
        {
            if(MyLibMang.IsBarcodeActive())
            {
                ListItem li= new ListItem("BarCode","2");
                Drp_booksearch.Items.Add(li);
            }

        }

        protected void Drp_booksearch_selectedIndexChanged(object sender, EventArgs e)
        {
            Txt_bookid_AutoCompleteExtender.ContextKey = Drp_booksearch.SelectedValue.ToString();
            Txt_bookid.Text = "";
        }
        protected void OnItemSelected(object sender, EventArgs e)
        {
            int Id = WC_bookcopies.SelectedId;
            
            
             if (WC_bookcopies.SearchType == "BarCode" || WC_bookcopies.SearchType == "Book Name" || WC_bookcopies.SearchType == "Book No")
            {
                CollectBooks(Id.ToString());
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
            dt.Columns.Add("BookId");
            dt.Columns.Add("BookName");
            dt.Columns.Add("UserID");
            dt.Columns.Add("UserName");
            dt.Columns.Add("Fine");


            int _count = int.Parse(Txt_bookid.Text);

            for (i = 0; i < _count; i++)
            {

                dr = _bookdataset.Tables["books"].NewRow();
                dr["BookId"] = i + 1;
                dr["Count"] = i + 1;
                dr["Fine"] = "0";
                _bookdataset.Tables["books"].Rows.Add(dr);

            }


            return _bookdataset;

        }

        protected void Btn_addbook_Click(object sender, EventArgs e)
        {
            WC_bookcopies.SearchType = "";
            WC_bookcopies.SearchValue = "";
            if (Txt_bookid.Text.Trim() != "")
            {
                string SearchType = Drp_booksearch.SelectedValue.ToString();
                WC_bookcopies.SearchType = Drp_booksearch.SelectedItem.ToString();
                WC_bookcopies.SearchValue = Txt_bookid.Text.Trim();
                WC_bookcopies.Show();
            }
            //CollectBooks();
        }

        private void CollectBooks(string BookId)
        {
            GrdBooks.Columns[2].Visible = true;
            
            string message;
           
            if (validBook(out message, BookId))
            {
                if (GrdBooks.Rows.Count > 0)
                {
                    if (BookExistsInGrid(GrdBooks, BookId))
                    {
                        GrdBooks.DataSource = AddPreviousData(BookId);
                        GrdBooks.DataBind();
                        LoadUserID(GrdBooks);
                        LoadFine(GrdBooks);
                        LoadIssuedDate(GrdBooks);
                    }
                    else
                    {
                        Lbl_msg.Text = "The Book is already added";
                        this.MPE_MessageBox.Show();
                    }
                }
                else
                {

                    string sql = "select tblbooks.BookNo, tblbookmaster.BookName ,  tblbookusertype.USerType from tblbooks inner join tblbookmaster on tblbookmaster.Id = tblbooks.BookId inner join tblbookissue on tblbookissue.BookNo = tblbooks.BookNo inner join tblbookusertype on tblbookusertype.Id = tblbookissue.UserType where tblbooks.BookNo='" + BookId + "'";
                    MydataSet = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (MydataSet.Tables[0].Rows.Count > 0)
                    {
                        GrdBooks.DataSource = MydataSet;
                        GrdBooks.DataBind();
                        Pnl_bookgrid.Visible = true;
                        LoadUserID(GrdBooks);
                        LoadFine(GrdBooks);
                        LoadIssuedDate(GrdBooks);

                    }
                    else
                    {
                        GrdBooks.DataSource = null;
                        GrdBooks.DataBind();
                        Pnl_bookgrid.Visible = false;
                        Lbl_msg.Text = "No books found";
                        this.MPE_MessageBox.Show();
                    }
                }
            }
            else
            {
                Lbl_msg.Text = message;
                this.MPE_MessageBox.Show();
            }
            GrdBooks.Columns[2].Visible = false;
            
        }

        private void LoadUserID(GridView GrdBooks)
        {

           
            string UserID;
            string name = "";
            if (GrdBooks.Rows.Count > 0)
            {
                foreach (GridViewRow gv in GrdBooks.Rows)
                {
                    Label Lbl_UserId = (Label)gv.FindControl("Lbl_UserId");
                    Label Lbl_UserName = (Label)gv.FindControl("Lbl_UserName");
                    UserID = MyLibMang.GetUserUniqueId(gv.Cells[0].Text.ToString(), out name);
                    
                    Lbl_UserId.Text = UserID;
                    Lbl_UserName.Text = name;

                }
            }
            
        }

      

        private void LoadIssuedDate(GridView GrdBooks)
        {

            DateTime _Date;
            string _date1;
            if (GrdBooks.Rows.Count > 0)
            {
                foreach (GridViewRow gv in GrdBooks.Rows)
                {
                    Label _Lbl_Date = (Label)gv.FindControl("Lbl_Date");
                    _date1 = MyLibMang.GetIssuedDate(gv.Cells[0].Text.ToString());
                    _Date = DateTime.Parse(_date1);
                  //  _Date = MyUser.GetDareFromText(_date1);
                    _Lbl_Date.Text = General.GerFormatedDatVal(_Date.Date);                
                }               
            }
        }

        private void LoadFine(GridView GrdBooks)
        {
            double Fine;
            double Tottal = 0;
            if (GrdBooks.Rows.Count > 0)
            {
                foreach (GridViewRow gv in GrdBooks.Rows)
                {
                    int type = 1;
                    Label Lbl_Fine = (Label)gv.FindControl("Lbl_Fine");
                    if (gv.Cells[4].Text.ToString() == "Staff")
                    {
                        type = 2;
                    }
                    Fine = MyLibMang.GetFine(gv.Cells[0].Text.ToString(), type);
                    Fine = Math.Round(Fine);
                    Lbl_Fine.Text = Fine.ToString("0.0");
                    Tottal = Tottal + Fine;
                }
                Tottal = Math.Round(Tottal);
                Lbl_TottalFine.Text = Tottal.ToString("0.0");
            }
        }


        private DataSet AddPreviousData(string BookId)
        {

           
            DataSet _bookdataset = new DataSet();
            DataTable dt;
            DataRow dr;
            int i;
            _bookdataset.Tables.Add(new DataTable("books"));
            dt = _bookdataset.Tables["books"];
           
            dt.Columns.Add("BookNo");
            dt.Columns.Add("BookName");
          
            dt.Columns.Add("USerType");
            dt.Columns.Add("USername");
      
            i = 0;
            foreach (GridViewRow gv in GrdBooks.Rows)
            {
                i++;
                dr = _bookdataset.Tables["books"].NewRow();
                //dr["Count"] = i;
                dr["BookNo"] = gv.Cells[0].Text.ToString();
                dr["BookName"] = gv.Cells[1].Text.ToString();
                
                dr["USerType"] = gv.Cells[4].Text.ToString();

                _bookdataset.Tables["books"].Rows.Add(dr);

            }
            string _BookNo = Txt_bookid.Text.Trim();
            string sql = "select tblbooks.BookNo, tblbookmaster.BookName , tblbookusertype.USerType from tblbooks inner join tblbookmaster on tblbookmaster.Id = tblbooks.BookId inner join tblbookissue on tblbookissue.BookNo = tblbooks.BookNo inner join tblbookusertype on tblbookusertype.Id = tblbookissue.UserType where tblbooks.BookNo='" + BookId + "'";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                dr = _bookdataset.Tables["books"].NewRow();
               
                dr["BookNo"] = MyReader.GetValue(0).ToString();

                dr["BookName"] = MyReader.GetValue(1).ToString();
              
                dr["USerType"] = MyReader.GetValue(2).ToString();
                _bookdataset.Tables["books"].Rows.Add(dr);

            }
          
            return _bookdataset;
        }

        private bool BookExistsInGrid(GridView GrdBooks, string BookId)
        {
            bool _valid = true;
            
            foreach (GridViewRow gv in GrdBooks.Rows)
            {
                if (BookId.CompareTo(gv.Cells[0].Text.ToString()) == 0)
                {
                    _valid = false;
                }
            }
            return _valid;

        }

        private bool validBook(out string message, string _bookid)
        {
            message = "";
            bool _valid = true;
            if (_bookid == "")
            {
                _valid = false;
                message = "Please enter the Book Id ";
            }
            else if (MyLibMang.BookIdExists(int.Parse(_bookid)))
            {
                _valid = false;
                message = "No book exists with the given Id";
            }
            else if (!MyLibMang.BookIssued(_bookid))
            {
                _valid = false;
                message="The book is not issued to anybody. Please verify";
            }
            return _valid;
        }

        protected void GrdBooks_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GrdBooks.Columns[2].Visible = true;
            string _temp = GrdBooks.Rows[e.RowIndex].Cells[0].Text.ToString();
            try
            {
                if (GrdBooks.Rows.Count > 1)
                {

                    GrdBooks.DataSource = Delete_Row(_temp);
                    GrdBooks.DataBind();
                    LoadUserID(GrdBooks);
                    LoadFine(GrdBooks);
                    LoadIssuedDate(GrdBooks);
                    Pnl_bookgrid.Visible = true;

                }
                else
                {
                    GrdBooks.DataSource = null;
                    GrdBooks.DataBind();
                    Pnl_bookgrid.Visible = false;


                }


            }

            catch
            {
                Pnl_bookgrid.Visible = false;


            }

            GrdBooks.Columns[2].Visible = false;


        }
        protected void GrdBooks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Renew")
            {
                int type = 0;
                int index = Convert.ToInt32(e.CommandArgument);
                MyLibMang.CollectBook(GrdBooks.Rows[index].Cells[0].Text);
                Label Lbl_GrdUserId = (Label)GrdBooks.Rows[index].FindControl("Lbl_UserId");
                Label Lbl_IssDate = (Label)GrdBooks.Rows[index].FindControl("Lbl_Date");
                Label Lbl_Fine = (Label)GrdBooks.Rows[index].FindControl("Lbl_Fine");
                TextBox Txt_Comment = (TextBox)GrdBooks.Rows[index].FindControl("Txt_Comment");
                if (GrdBooks.Rows[index].Cells[4].Text.ToString() == "Student")
                {
                    type = 1;
                }
                else
                {
                    type = 2;
                }
                MyLibMang.ReportHistory(GrdBooks.Rows[index].Cells[0].Text.ToString(), Lbl_GrdUserId.Text, type.ToString(), Lbl_IssDate.Text, Lbl_Fine.Text, Txt_Comment.Text, MyUser.CurrentBatchId);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "Book:" + GrdBooks.Rows[index].Cells[1].Text.ToString() + " collected", 1);
                MyLibMang.IssueBook(Lbl_GrdUserId.Text, GrdBooks.Rows[index].Cells[0].Text.ToString(),type);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "Book:" + GrdBooks.Rows[index].Cells[1].Text.ToString() + " issued", 1);
                GrdBooks.Columns[2].Visible = true;
                string _temp = GrdBooks.Rows[index].Cells[0].Text.ToString();
                try
                {
                    if (GrdBooks.Rows.Count > 1)
                    {

                        GrdBooks.DataSource = Delete_Row(_temp);
                        GrdBooks.DataBind();
                        LoadUserID(GrdBooks);
                        LoadFine(GrdBooks);
                        LoadIssuedDate(GrdBooks);
                        Pnl_bookgrid.Visible = true;

                    }
                    else
                    {
                        GrdBooks.DataSource = null;
                        GrdBooks.DataBind();
                        Pnl_bookgrid.Visible = false;


                    }


                }

                catch
                {
                    Pnl_bookgrid.Visible = false;


                }

                GrdBooks.Columns[2].Visible = false;
                Lbl_msg.Text = "The book is collected and Renewal";
                this.MPE_MessageBox.Show();
            }
        }
        private DataSet Delete_Row(string _temp)
        {
            DataSet _bookdataset = new DataSet();
            DataTable dt;
            DataRow dr;
            _bookdataset.Tables.Add(new DataTable("books"));
            dt = _bookdataset.Tables["books"];
            dt.Columns.Add("BookNo");
            dt.Columns.Add("BookName");
            dt.Columns.Add("UserType");
            foreach (GridViewRow gv in GrdBooks.Rows)
            {
                if (gv.Cells[0].Text.ToString() != _temp)
                {
                    dr = _bookdataset.Tables["books"].NewRow();
                    dr["BookNo"] = gv.Cells[0].Text;                   
                    dr["BookName"] = gv.Cells[1].Text;
                    dr["UserType"] = gv.Cells[4].Text;                   
                    _bookdataset.Tables["books"].Rows.Add(dr);
                }
            }
            return _bookdataset;
        }
        protected void Btn_Collect_Click(object sender, EventArgs e)
        {
            int type=0;
            GrdBooks.Columns[2].Visible = true;
            if (GrdBooks.Rows.Count > 0)
            {
             
                    foreach (GridViewRow gv in GrdBooks.Rows)
                    {
                        type = 0;                  
                        MyLibMang.CollectBook(gv.Cells[0].Text.ToString());
                        Label Lbl_GrdUserId = (Label)gv.FindControl("Lbl_UserId");
                        Label Lbl_IssDate = (Label)gv.FindControl("Lbl_Date");
                        Label Lbl_Fine = (Label)gv.FindControl("Lbl_Fine");
                        TextBox Txt_Comment = (TextBox)gv.FindControl("Txt_Comment");
                        if (gv.Cells[4].Text.ToString() == "Student")
                        {
                            type = 1;
                        }
                        else
                            type = 2;

                        MyLibMang.ReportHistory(gv.Cells[0].Text.ToString(), Lbl_GrdUserId.Text, type.ToString(), Lbl_IssDate.Text, Lbl_Fine.Text, Txt_Comment.Text, MyUser.CurrentBatchId);
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "Book:"+ gv.Cells[1].Text.ToString() +" collected", 1);
                  
                       
                    }
                    Cleardata();
                   
                    Lbl_msg.Text = "The book(s) are collected ";
                    this.MPE_MessageBox.Show();
            }
            Txt_bookid.Text = "";
            GrdBooks.Columns[2].Visible = false;
        }
        private void Cleardata()
        {
            GrdBooks.Columns[2].Visible = true;
            GrdBooks.DataSource = null;
            GrdBooks.DataBind();
            Pnl_bookgrid.Visible = false;
            Txt_bookid.Text = "";
            GrdBooks.Columns[2].Visible = false;
        }

       

    }
}
