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
using System.IO;
namespace WinEr
{
    public partial class LlbraryHome : System.Web.UI.Page
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
                //no rights for this user.
            }
            else
            {
                if (!IsPostBack)
                {
                    Pnl_bookgrid.Visible = false;
                    //checkTab();
                    LoadGenaralDetails();
                    LoadBookCategoryToDrpList(0);
                    LoadBookType(0);
                    LoadSearchitems();
                    AutoCompleteExtender_BookId.ContextKey = "0";
                    Txt_Search_AutoCompleteExtender.ContextKey = "1";
                    if (MyLibMang.IsBarcodeActive())
                    {
                        BoundField b = new BoundField();
                        b.DataField = "BarCode";
                        b.HeaderText = "BarCode";
                        GrdBooks.Columns.Add(b);


                    }
                }
            }

        }

        private void LoadSearchitems()
        {
            if (MyLibMang.IsBarcodeActive())
            {
                ListItem li=new ListItem("Barcode","5");
                Drp_Quick.Items.Add(li);
            }
        }
        
        private void LoadGenaralDetails()
        {
             string sql = "select count(tblbooks.Id) from tblbooks";
             MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
             if (MyReader.HasRows)
             {
                 Lbl_Books.Text = MyReader.GetValue(0).ToString();
             }
             sql = "select count( tblbookissue.BookNo ) from tblbooks inner join tblbookissue on tblbookissue.BookNo = tblbooks.BookNo";
             MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
             if (MyReader.HasRows)
             {
                 Lbl_IssuedBooks.Text = MyReader.GetValue(0).ToString();
             }
             sql = "select count( tblbookbooking.bookId) from tblbooks inner join tblbookbooking on tblbookbooking.bookId = tblbooks.BookNo";
             MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
             if (MyReader.HasRows)
             {
                 Lbl_BkdBooks.Text = MyReader.GetValue(0).ToString();
             }
             sql = "select count(tblbooks.BookNo) from tblbooks where tblbooks.BookNo not in (select tblbookissue.BookNo from tblbooks inner join tblbookissue on tblbookissue.BookNo = tblbooks.BookNo)";
             MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
             if (MyReader.HasRows)
             {
                 Lbl_AvailableBooks.Text = MyReader.GetValue(0).ToString();
             }
             sql = "select count(tblbookissue.BookNo) from tblbookissue where tblbookissue.UserType=1";
             MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
             if (MyReader.HasRows)
             {
                 Lbl_BksStudents.Text = MyReader.GetValue(0).ToString();
             }
             sql = "select count(tblbookissue.BookNo) from tblbookissue where tblbookissue.UserType=2";
             MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
             if (MyReader.HasRows)
             {
                 Lbl_BksStaff.Text = MyReader.GetValue(0).ToString();
             }
        }
       
        private void LoadBookType(int _intex)
        {
            Drp_Type.Items.Clear();
            string sql = "SELECT Id,TypeName FROM tblbooktype order by TypeName ASC";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_Type.Items.Add(new ListItem("Any", "0"));
                Btn_Search.Enabled = true;
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Type.Items.Add(li);
                }
                Drp_Type.SelectedIndex = _intex;
            }
            else
            {
                Drp_Type.Items.Add(new ListItem("No Type Found", "-1"));
                Btn_Search.Enabled = false;
                Lbl_msg.Text = "Add book type before searching";
                this.MPE_MessageBox.Show();
            }
        }

        private void LoadBookCategoryToDrpList(int _intex)
        {
            Drp_Cat.Items.Clear();
            string sql = "SELECT Id,CatogoryName FROM tblbookcatogory order by CatogoryName ASC";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_Search.Enabled = true;
                Drp_Cat.Items.Add(new ListItem("Any", "0"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Cat.Items.Add(li);
                }
                Drp_Cat.SelectedIndex = _intex;
            }
            else
            {
                Drp_Cat.Items.Add(new ListItem("No Category Found", "-1"));
                Btn_Search.Enabled = false;
                Lbl_msg.Text = "Add book category before searching";
                this.MPE_MessageBox.Show();
            }
        }
        protected void Btn_QuickSearch_Click(object sender, EventArgs e)
        {
            if (Txt_Search.Text.Trim() == "")
            {
                Lbl_msg.Text = "Search field cannot be empty";
                this.MPE_MessageBox.Show();
            }
            else
            {
                ViewState["AsvanceSearch"] = "0";

                FillGrid();
            }
            
        }
       
        private void FillGrid()
        {
            GrdBooks.DataSource = null;
            GrdBooks.DataBind();
            string sql = "select tblbooks.BookNo, tblbookmaster.BookName, tblbookmaster.Author, tblbookmaster.Publisher, tblbookmaster.Bookslno ";
            
            if (MyLibMang.IsBarcodeActive())
            {
                sql=sql+ " ,tblbooks.Barcode as BarCode ";
            }
            sql=sql+"from tblbooks inner join tblbookmaster on tblbookmaster.Id = tblbooks.BookId";

            if (Drp_Quick.SelectedIndex == 0)
            {
                sql = sql + " where tblbooks.BookNo='" + Txt_Search.Text.Trim() + "'";
            }
            else if (Drp_Quick.SelectedIndex == 1)
            {
                sql = sql + " where tblbookmaster.BookName like'%" + Txt_Search.Text.Trim() + "%'";
            }
            else if (Drp_Quick.SelectedIndex == 2)
            {
                sql = sql + " where tblbookmaster.Author like '%" + Txt_Search.Text.Trim() + "%'";
            }
            else if (Drp_Quick.SelectedIndex == 3)
            {
                sql = sql + " where tblbookmaster.Publisher='" + Txt_Search.Text.Trim() + "'";
            }
            else if (Drp_Quick.SelectedIndex == 5)
            {
                sql = sql + " where tblbooks.Barcode='" + Txt_Search.Text.Trim() + "'";
            }
            else if (Drp_Quick.SelectedIndex == 4)
            {
                sql = sql + " where tblbookmaster.Bookslno='" + Txt_Search.Text.Trim() + "'";
            }
            MydataSet = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                ViewState["LibraryValues"] = sql;
                
                GrdBooks.DataSource = MydataSet;
                GrdBooks.DataBind();
                LoadTakenUser();
                LoadExtraValues(MydataSet);
                Pnl_bookgrid.Visible = true;


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

        private void LoadExtraValues(DataSet MydataSet)
        {

            int count = MydataSet.Tables[0].Rows.Count;
            int IssuedBooksCount=0;
            int staffissued=0;
            int studentissued=0;
            string sql,sqlStaff,sql_stud;
            sql = "select count(tblbooks.BookNo)from tblbooks inner join tblbookmaster on tblbookmaster.Id = tblbooks.BookId  inner join tblbookissue on tblbookissue.BookNo= tblbooks.BookNo";
            if (Drp_Quick.SelectedIndex == 0)
            {
                sql = sql + " where tblbooks.BookNo='" + Txt_Search.Text.Trim() + "'";

            }
            else if (Drp_Quick.SelectedIndex == 1)
            {
                sql = sql + " where tblbookmaster.BookName like'%" + Txt_Search.Text.Trim() + "%'";
            }
            else if (Drp_Quick.SelectedIndex == 2)
            {
                sql = sql + " where tblbookmaster.Author like '%" + Txt_Search.Text.Trim() + "%'";
            }
            else if (Drp_Quick.SelectedIndex == 3)
            {
                sql = sql + " where tblbookmaster.Publisher='" + Txt_Search.Text.Trim() + "'";
            }
            else if (Drp_Quick.SelectedIndex == 4)
            {
                sql = sql + " where tblbooks.Barcode='" + Txt_Search.Text.Trim() + "'";
            }
            sqlStaff=sql+" and tblbookissue.UserType=2";
            sql_stud=sql+" and tblbookissue.UserType=1";

            MyReader= MyLibMang.m_MysqlDb.ExecuteQuery(sql);

            if(MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(),out IssuedBooksCount);
            }
            MyReader= MyLibMang.m_MysqlDb.ExecuteQuery(sqlStaff);

            if(MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(),out staffissued);
            }
            MyReader= MyLibMang.m_MysqlDb.ExecuteQuery(sql_stud);

            if(MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(),out studentissued);
            }

            Lbl_Srch_Total.Text=count.ToString();
            Lbl_Srch_IssuedBooks.Text=IssuedBooksCount.ToString();
            Lbl_Srch_StaffIssued.Text=staffissued.ToString();
            Lbl_Srch_StudIssued.Text=studentissued.ToString();   
                
        }

        private void LoadTakenUser()
        {
            foreach (GridViewRow gv in GrdBooks.Rows)
            {
                Label _Lbl_TakenBy = (Label)gv.FindControl("Lbl_TakenBy");
              //  _Lbl_TakenBy.Text = MyLibMang.GetTakenByUser(gv.Cells[1].Text.ToString());
                _Lbl_TakenBy.Text = MyLibMang.GetUserUniqueName(gv.Cells[1].Text.ToString());
                Label _Lbl_bookedBy = (Label)gv.FindControl("Lbl_bookedBy");
                _Lbl_bookedBy.Text = MyLibMang.GetBookingUserUniqueId(gv.Cells[1].Text.ToString());
                
            }
        }

        protected void GrdBooks__PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdBooks.PageIndex = e.NewPageIndex;
            if (ViewState["AsvanceSearch"].ToString() == "1")
            {
                AdvanceSearch();
            }
            else
            {
                FillGrid();
            }
            
            
        }
        protected void GrdBooks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowState == DataControlRowState.Alternate)
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='gray';this.style.cursor='hand'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
                }
                else
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='gray';this.style.cursor='hand'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
                }
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GrdBooks, "Select$" + e.Row.RowIndex);
            }

        }
        protected void GrdBooks_SelectedIndexChanged(object sender, EventArgs e)
        {
            string BookId= GrdBooks.SelectedRow.Cells[1].Text.ToString();
           // Response.Redirect("BookSearchDetails.aspx?BookId='" + BookId + "'");
            Session["BookId"] = BookId;
            Response.Redirect("BookSearchDetails.aspx");
        }

      

        //private void FillGridDetails()
        //{
        //    if (Txt_BkId.Text.Trim() != "" || Txt_BkName.Text.Trim() != "" || Txt_Publisher.Text.Trim() != "" || Txt_Author.Text.Trim() != "" || Drp_Type.SelectedValue != "0" || Drp_Cat.SelectedValue != "0")
        //    {

        //        string sql = "select tblbooks.BookNo, tblbookmaster.BookName, tblbookmaster.Author, tblbookmaster.Publisher, tblbooktype.TypeName, tblbookcatogory.CatogoryName from tblbooks inner join tblbookmaster on tblbookmaster.Id = tblbooks.BookId inner join tblbooktype on tblbooktype.Id = tblbookmaster.TypeId inner join tblbookcatogory on tblbookcatogory.Id = tblbookmaster.CatagoryId where tblbooks.Id <>-1";
        //        if (Txt_BkId.Text.Trim() !="")
        //        {
        //            sql = sql + " and tblbooks.BookNo='" + Txt_BkId.Text.Trim()+ "'";
        //        }
        //        if (Txt_BkName.Text.Trim() != "")
        //        {
        //            sql = sql + " and tblbookmaster.BookName='" + Txt_BkName.Text.Trim() + "'";
        //        }
        //        if (Txt_Publisher.Text.Trim() !="")
        //        {
        //            sql = sql + " and tblbookmaster.Publisher='" + Txt_Publisher.Text.Trim() + "'";
        //        }
        //        if (Txt_Author.Text.Trim()!="")
        //        {
        //            sql = sql + " and tblbookmaster.Author like '%" + Txt_Author.Text.Trim() + "%'";
        //        }
        //        if (Drp_Type.SelectedValue != "0")
        //        {
        //            sql = sql + " and tblbooktype.Id='" + Drp_Type.SelectedValue + "'";
        //        }
        //        if (Drp_Cat.SelectedValue != "0")
        //        {
        //            sql = sql + " and tblbookcatogory.Id='" + Drp_Cat.SelectedValue + "'";
        //        }
        //        MydataSet = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        //        if (MydataSet.Tables[0].Rows.Count > 0)
        //        {
        //            GrdBooks.DataSource = MydataSet;
        //            GrdBooks.DataBind();
        //            LoadTakenUser();
        //            Pnl_bookgrid.Visible = true;
        //        }
        //        else
        //        {
        //            GrdBooks.DataSource = null;
        //            GrdBooks.DataBind();
        //            Pnl_bookgrid.Visible = false;
        //            Lbl_msg.Text = "No books found";                  
        //            this.MPE_MessageBox.Show();

        //        }
        //    }
        //    else
        //    {
        //        GrdBooks.DataSource = null;
        //        GrdBooks.DataBind();
        //        Pnl_bookgrid.Visible = false;
        //        Lbl_msg.Text = "Please enter any value";
        //        this.MPE_MessageBox.Show();
        //    }
        //}


        protected void Btn_Export_Click(object sender, EventArgs e)
        {

            DataTable dt;
            DataRow dr;
            DataSet Dt = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet((string)ViewState["LibraryValues"]);
           
          
            if (Dt.Tables[0].Rows.Count > 0)
            {

                Dt=BuildDataset(Dt);
                foreach (DataRow dr_values in Dt.Tables[0].Rows)
                {
                    string _BookNo=dr_values["BookNo"].ToString();
                    dr_values["IssuedUserType"] = getIssuedUserType(_BookNo);
                    dr_values["BookedUserType"] = getBookedUserType(_BookNo);
                    dr_values["IssuedBy"] = MyLibMang.GetUserUniqueName(_BookNo);
                    dr_values["BookedBy"] = MyLibMang.GetBookingUserUniqueId(_BookNo);
                }

                string FileName = "BookList";
                string _ReportName = "BookList";
                if (!WinEr.ExcelUtility.ExportDataToExcel(Dt, _ReportName, FileName, MyUser.ExcelHeader))
                {
                    Lbl_msg.Text = "Please try again.";
                    this.MPE_MessageBox.Show();
                }
            }
        }

        private string getBookedUserType(string _BookNo)
        {
            string sql = "select tblbookbooking.UserId, tblbookbooking.UserType from tblbookbooking where tblbookbooking.bookId = '" + _BookNo + "'";
            MyReader =MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int UserTypeId = int.Parse(MyReader.GetValue(1).ToString());
                if (UserTypeId == 1)
                    return "Student";
                else
                    return "Staff";
                
            }
            return "none";
        }

        private string getIssuedUserType(string _BookNo)
        {
            string sql = "select UserType,UserId from tblbookissue where BookNo = '" + _BookNo + "'";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int UserTypeId = int.Parse(MyReader.GetValue(0).ToString());
                if (UserTypeId == 1)
                    return "Student";
                else
                    return "Staff";

            }
            return "none";
        }


        protected DataSet BuildDataset(DataSet MydataSet)
        {
            DataSet _bookdataset = new DataSet();
            DataTable dt;
            DataRow dr;
            int i;
            _bookdataset.Tables.Add(new DataTable("books"));

            dt = _bookdataset.Tables["books"];
            dt.Columns.Add("BookNo");
            dt.Columns.Add("BookName");
            dt.Columns.Add("Author");
            dt.Columns.Add("Publisher");
            if (MyLibMang.IsBarcodeActive())
            {
                dt.Columns.Add("Barcode");
            }
            dt.Columns.Add("IssuedBy");
            dt.Columns.Add("IssuedUserType");
            dt.Columns.Add("BookedBy");
            dt.Columns.Add("BookedUserType");
            dt.Columns.Add("BookSlNo");

            foreach (DataRow dr_values in MydataSet.Tables[0].Rows)
            {

                dr = _bookdataset.Tables["books"].NewRow();


               
                dr["BookNo"] = dr_values["BookNo"];
                dr["BookName"] = dr_values["BookName"];
                dr["Author"] = dr_values["Author"];
                dr["Publisher"] = dr_values["Publisher"];
                if (MyLibMang.IsBarcodeActive())
                {
                    dr["Barcode"] = dr_values["Barcode"];
                   
                }
                dr["IssuedBy"] ="";
                dr["IssuedUserType"] = "";
                dr["BookedBy"] = "";
                dr["BookedUserType"] = "";
                dr["BookSlNo"] = dr_values["BookSlNo"];
               

                _bookdataset.Tables["books"].Rows.Add(dr);

            }


            return _bookdataset;

        }

        protected void Drp_SearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {   
            Txt_Search.Text = "";
            GrdBooks.DataSource = null;
            GrdBooks.DataBind();
            Pnl_bookgrid.Visible = false;
            Txt_Search_AutoCompleteExtender.ContextKey = Drp_Quick.SelectedValue.ToString();
            //Tabs.ActiveTabIndex = 1;
        }
        

      
        protected void Lnk_Detail_Click(object sender, EventArgs e)
        {
            Txt_BkName.Text = "";
            Txt_BkId.Text="";
            Txt_Author.Text = "";
            Txt_Publisher.Text = "";
            Txt_bookslno.Text = "";
            lbl_Err.Text = "";
            LoadBookCategoryToDrpList(0);
            LoadBookType(0);
            MPE_AdvanceSearch.Show();
        }
        protected void Btn_AdvanceSearch_Click(object sender, EventArgs e)
        {
            AdvanceSearch();

            ViewState["AsvanceSearch"] = "1";
        }

        private void AdvanceSearch()
        {
            string Name = Txt_BkName.Text;
            string BookId = Txt_BkId.Text;
            string Author = Txt_Author.Text;
            string Publisher = Txt_Publisher.Text;
            string CategoryId = Drp_Cat.SelectedValue.ToString();
            string TypeId = Drp_Type.SelectedValue.ToString();
            string bookslno = Txt_bookslno.Text;
            int flag = 0;

            if (Name != "" || BookId != "" || Author != "" || Publisher != "" || CategoryId != "-1" || TypeId != "-1" || bookslno!="")
            {
                string sql = "select tblbooks.BookNo, tblbookmaster.BookName, tblbookmaster.Author, tblbookmaster.Publisher,tblbookmaster.Bookslno"; 
                if (MyLibMang.IsBarcodeActive())
                {
                    sql = sql + " ,tblbooks.Barcode as BarCode ";
                }
                sql = sql + "  from tblbooks inner join tblbookmaster on tblbookmaster.Id = tblbooks.BookId ";
                if (BookId != "")
                {
                    if (flag == 0)
                        sql = sql + " where ";
                    if (flag == 1)
                        sql = sql + " and ";
                    sql = sql + "  tblbooks.BookNo='" + BookId + "'";
                    flag = 1;
                }
                else if (Name != "")
                {
                    if (flag == 0)
                        sql = sql + " where ";
                    if (flag == 1)
                        sql = sql + " and ";
                    sql = sql + "  tblbookmaster.BookName ='" + Name + "'";
                    flag = 1;

                }
                else if (Author != "")
                {
                    if (flag == 0)
                        sql = sql + " where ";
                    if (flag == 1)
                        sql = sql + " and ";
                    sql = sql + "  tblbookmaster.Author ='" + Author + "'";
                    flag = 1;
                }
                else if (Publisher != "")
                {
                    if (flag == 0)
                        sql = sql + " where ";
                    if (flag == 1)
                        sql = sql = " and ";
                    sql = sql + "  tblbookmaster.Publisher ='" + Publisher + "'";
                    flag = 1;
                }
                else if (bookslno != "")
                {
                    if (flag == 0)
                        sql = sql + " where ";
                    if (flag == 1)
                        sql = sql = " and ";
                    sql = sql + "  tblbookmaster.Bookslno ='" + bookslno + "'";
                    flag = 1;
                }
                if (CategoryId != "0")
                {
                    if (flag == 0)
                        sql = sql + " where ";
                    if (flag == 1)
                        sql = sql + " and ";
                    sql = sql + "  tblbookmaster.CatagoryId =" + int.Parse(CategoryId) + "";
                    flag = 1;

                }
                if (TypeId != "0")
                {
                    if (flag == 0)
                        sql = sql + " where ";
                    if (flag == 1)
                        sql = sql + " and ";
                    sql = sql + "  tblbookmaster.TypeId =" + int.Parse(TypeId) + "";
                    flag = 1;

                }
               


                MydataSet = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    ViewState["LibraryValues"] = sql;
                    GrdBooks.DataSource = MydataSet;
                    GrdBooks.DataBind();
                    LoadTakenUser();
                    LoadValues(MydataSet);
                    Pnl_bookgrid.Visible = true;

                }
                else
                {
                    GrdBooks.DataSource = null;
                    GrdBooks.DataBind();
                    Pnl_bookgrid.Visible = false;
                    lbl_Err.Text = "No books found";
                    this.MPE_AdvanceSearch.Show();
                }
            }
            else
            {
                lbl_Err.Text = "Please enter atleast one search key words";
                this.MPE_AdvanceSearch.Show();
            }
          





        }

        private void LoadValues(DataSet MydataSet)
        {
            int count = MydataSet.Tables[0].Rows.Count;
            int IssuedBooksCount = 0;
            int staffissued = 0;
            int studentissued = 0;
            int flag = 0;
            string sql, sqlStaff, sql_stud;
            sql = "select count(tblbooks.BookNo)from tblbooks inner join tblbookmaster on tblbookmaster.Id = tblbooks.BookId  inner join tblbookissue on tblbookissue.BookNo= tblbooks.BookNo";
            if (Txt_BkId.Text.Trim() != "")
            {
                if (flag == 0)
                {
                    sql = sql + " where ";
                }
                else if(flag==1)
                    sql = sql + " and ";
                flag = 1;
                sql = sql + "  tblbooks.BookNo='" + Txt_BkId.Text.Trim() + "'";

            }
            else if (Txt_BkName.Text.Trim() != "")
            {
                if (flag == 0)
                {
                    sql = sql + " where ";
                }
                else if (flag == 1)
                    sql = sql + " and ";
                flag = 1;
                sql = sql + "  tblbookmaster.BookName like'%" + Txt_BkName.Text.Trim() + "%'";
            }
            else if (Txt_Author.Text.Trim() != "")
            {
                if (flag == 0)
                {
                    sql = sql + " where ";
                }
                else if (flag == 1)
                    sql = sql + " and ";
                flag = 1;

                sql = sql + "  tblbookmaster.Author like '%" + Txt_Author.Text.Trim() + "%'";
            }
            else if (Txt_Publisher.Text.Trim()!= "")
            {
                if (flag == 0)
                {
                    sql = sql + " where ";
                }
                else if (flag == 1)
                    sql = sql + " and ";
                flag = 1;
                sql = sql + "  tblbookmaster.Publisher='" + Txt_Publisher.Text.Trim() + "'";
            }
            else if (Drp_Cat.SelectedValue != "0")
            {
                if (flag == 0)
                {
                    sql = sql + " where ";
                }
                else if (flag == 1)
                    sql = sql + " and ";
                flag = 1;
                sql = sql + "  tblbookmaster.CatagoryId=" + int.Parse(Drp_Cat.SelectedValue.ToString()) + "";
            }
            else if (Drp_Type.SelectedValue != "0")
            {
                if (flag == 0)
                {
                    sql = sql + " where ";
                }
                else if (flag == 1)
                    sql = sql + " and ";
                flag = 1;
                sql = sql + "  tblbookmaster.TypeId=" + int.Parse(Drp_Cat.SelectedValue.ToString()) + "";
            }
            
            sqlStaff = sql + " and tblbookissue.UserType=2";
            sql_stud = sql + " and tblbookissue.UserType=1";

            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out IssuedBooksCount);
            }
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sqlStaff);

            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out staffissued);
            }
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql_stud);

            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out studentissued);
            }

            Lbl_Srch_Total.Text = count.ToString();
            Lbl_Srch_IssuedBooks.Text = IssuedBooksCount.ToString();
            Lbl_Srch_StaffIssued.Text = staffissued.ToString();
            Lbl_Srch_StudIssued.Text = studentissued.ToString();   
        }
    }
}
