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
    public partial class IsseueBook : System.Web.UI.Page
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
                    Pnl_userdetails.Visible = false;
                    Pnl_bookarea.Visible = false;
                    Pnl_bookgrid.Visible = false;
                    Lbl_GridMessage.Text = "";
                    LoadUserTypeToRdoGrp(0);
                    LoadDrpdwn();
                    Txt_userid_AutoCompleteExtender.ContextKey = "1"+"\\1";
                    Txt_bookid_AutoCompleteExtender.ContextKey = "0";
                }
            }

        }

        private void LoadDrpdwn()
        {
            if(MyLibMang.IsBarcodeActive())
            {
                ListItem li = new ListItem("BarCode", "2");
                Drp_booksearch.Items.Add(li);
            }
           
        }
        protected void OnItemSelected(object sender, EventArgs e)
        {
            int Id = WC_bookcopies.SelectedId;
            int searchtype = 0;
            if (WC_bookcopies.SearchType == "Student"||WC_bookcopies.SearchType == "Staff")
            {
                Pnl_userdetails.Visible = true;
                Pnl_bookarea.Visible = true;
                Pnl_bookgrid.Visible = false;
                int.TryParse(Drp_SearchBy.SelectedValue.ToString(), out searchtype);
                int userType=0;
                int.TryParse(RdoBtn_UserType.SelectedValue.ToString(), out userType);
                LoadStudent(Id, userType, searchtype);
                LoadBooks(Id.ToString(), userType);
                Txt_SaveUser.Text = Id.ToString();
                
            }
            
            else if (WC_bookcopies.SearchType == "BarCode"||WC_bookcopies.SearchType == "Book Name"||WC_bookcopies.SearchType == "Book No")
            {
                IssueBooks(Id.ToString());
            }
            
        }

        private void LoadUserTypeToRdoGrp(int _index)
        {
            RdoBtn_UserType.Items.Clear();
            string sql = "SELECT Id,USerType FROM tblbookusertype";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
               
               
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    RdoBtn_UserType.Items.Add(li);
                }
                RdoBtn_UserType.SelectedIndex = _index;
            }
            else
            {
                
            }
        }
       
        protected void Btn_continue_Click(object sender, EventArgs e)
        {
            
            clear();
            string _message;
            int UserType = int.Parse(RdoBtn_UserType.SelectedValue.ToString());
            int searchtype=0;
            int.TryParse(Drp_SearchBy.SelectedValue.ToString(), out searchtype);

            Pnl_bookarea.Visible = false;
            Pnl_bookgrid.Visible = false;
            Pnl_userdetails.Visible = false;

            if(validuserId(out _message))
            {
                if (MyLibMang.UserExists(Txt_userid.Text.Trim(), UserType, searchtype))
                {
                    


                    WC_bookcopies.SearchType = RdoBtn_UserType.SelectedItem.ToString();
                    WC_bookcopies.SearchValue=Txt_userid.Text.Trim();
                    WC_bookcopies.valueType = searchtype;
                    WC_bookcopies.Show();
                    //problem for loading staff and student by name(same  name for ,multiple users)
                    
                   // LoadStudent(Txt_userid.Text.Trim(), UserType, searchtype);
                   // Txt_SaveUser.Text = Txt_userid.Text;
                    //Btn_AddUser.Visible = false;
                   // LoadBooks(Txt_userid.Text.Trim(),UserType);
                    //Btn_edit.Visible = true;
                   
                }
                else
                {
                    //Btn_edit.Visible = false;
                    Lbl_msg.Text = "No user found";
                    Pnl_bookarea.Visible = false;
                    Pnl_bookgrid.Visible = false;
                    Pnl_userdetails.Visible = false;
                    this.MPE_MessageBox.Show();
                }
            }
            else
            {              
                Lbl_msg.Text = _message;
                this.MPE_MessageBox.Show();
                Pnl_bookarea.Visible = false;
                Pnl_bookgrid.Visible = false;
                Pnl_userdetails.Visible = false;
            }

        }

        private void clear()
        {
            Grd_ViewBook.DataSource = null;
            Grd_ViewBook.DataBind();
            GrdBooks.DataSource = null;
            GrdBooks.DataBind();
            //Txt_desc.Text = "";
            Lbl_class_department.Text = "";
            Lbl_username.Text = "";
            Txt_SaveUser.Text = "";
            Txt_bookid.Text = "";
            Lbl_GridMessage.Text = "";
        }

        private void LoadBooks(string _userId, int _UserType)
        {
            
            DataSet _bookdataset = new DataSet();
            DataTable dt;
            DataRow dr;
            int checkBook=0;
            _bookdataset.Tables.Add(new DataTable("books"));
            dt = _bookdataset.Tables["books"];
            dt.Columns.Add("BookNo");
            dt.Columns.Add("BookName");
            dt.Columns.Add("Author");
            dt.Columns.Add("Action");
            dt.Columns.Add("Date");

                       // select tblbookissue.BookNo, tblbookmaster.BookName, tblbookmaster.Author  from tblbookissue inner join tblbooks on tblbooks.BookNo = tblbookissue.BookNo inner join tblbookmaster on tblbookmaster.Id = tblbooks.BookId inner join tblstudent on tblstudent.Id = tblbookissue.UserId WHERE tblbookissue.UserType=1 
            string sql = "select tblbookissue.BookNo, tblbookmaster.BookName, tblbookmaster.Author , DATE_FORMAT( tblbookissue.DateOfIssue, '%d/%m/%Y')as Date  from tblbookissue inner join tblbooks on tblbooks.BookNo = tblbookissue.BookNo inner join tblbookmaster on tblbookmaster.Id = tblbooks.BookId ";// where tblbookissue.UserId='" + _userId + "'";

            if (_UserType==1  )
            {
                sql = sql + "inner join tblstudent on tblstudent.Id = tblbookissue.UserId where tblstudent.Id='" + _userId + "' and tblbookissue.UserType=1";
            }
            else if (_UserType==2)
            {
                sql = sql + "inner join tbluser on tbluser.Id = tblbookissue.UserId where tbluser.Id='" + _userId + "' and tblbookissue.UserType=2";
            }
            
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                checkBook = 1;
                while (MyReader.Read())
                {
                    dr = _bookdataset.Tables["books"].NewRow();
                    dr["BookNo"] = MyReader.GetValue(0).ToString();
                    dr["BookName"] = MyReader.GetValue(1).ToString();
                    dr["Author"] = MyReader.GetValue(2).ToString();

                    dr["Action"] = "Issued";
                    dr["Date"] = MyReader.GetValue(3).ToString();
                    _bookdataset.Tables["books"].Rows.Add(dr);
                }
            }
            if (_UserType == 1)
            {
                sql = "select tblbookbooking.bookId, tblbookmaster.BookName, tblbookmaster.Author ,  DATE_FORMAT( tblbookbooking.DateOfBooking, '%d/%m/%Y')as Date  from tblbookbooking inner join tblbooks on tblbooks.BookNo = tblbookbooking.bookId inner join tblbookmaster on tblbookmaster.Id = tblbooks.BookId inner join tblstudent on tblstudent.Id = tblbookbooking.UserId where tblstudent.Id='" + _userId + "' and tblbookbooking.UserType=1";
            }
            else if (_UserType == 2)
            {
                sql = "select tblbookbooking.bookId, tblbookmaster.BookName, tblbookmaster.Author , DATE_FORMAT( tblbookbooking.DateOfBooking, '%d/%m/%Y')as Date   from tblbookbooking inner join tblbooks on tblbooks.BookNo = tblbookbooking.bookId inner join tblbookmaster on tblbookmaster.Id = tblbooks.BookId inner join tbluser on tbluser.Id = tblbookbooking.UserId where tbluser.Id='" + _userId + "' and tblbookbooking.UserType=2";
            }
            
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                checkBook = 1;
                while (MyReader.Read())
                {
                    dr = _bookdataset.Tables["books"].NewRow();
                    dr["BookNo"] = MyReader.GetValue(0).ToString();
                    dr["BookName"] = MyReader.GetValue(1).ToString();
                    dr["Author"] = MyReader.GetValue(2).ToString();
                    dr["Action"] = "Booked";
                    dr["Date"] = MyReader.GetValue(3).ToString();
                    _bookdataset.Tables["books"].Rows.Add(dr);
                }
            }
            Grd_ViewBook.DataSource = _bookdataset;
            Grd_ViewBook.DataBind();
           // Pnl_bookgrid.Visible = true;
            LoadActionToViewGrid(Grd_ViewBook, _userId, _UserType);
            LoadDateToViewGrid(Grd_ViewBook, _userId, _UserType);
            if (checkBook == 0)
            {
                Lbl_GridMessage.Text = "There is no book is pending for the user";
            }
        }

        private void LoadDateToViewGrid(GridView Grd_ViewBook, string _userId , int _UserType)
        {
            string status;
            DateTime _Time;
            if (Grd_ViewBook.Rows.Count > 0)
            {

                foreach (GridViewRow gv in Grd_ViewBook.Rows)
                {
                    Label _Label_Date = (Label)gv.FindControl("Label_Date");
                    status = MyLibMang.GetTheCurrentViewGridDate(gv.Cells[0].Text.ToString(), _userId , _UserType);
                    if (status != "")
                    {
                        _Time = DateTime.Parse(status);
                        // _Time = MyUser.GetDareFromText(status);
                        _Label_Date.Text = General.GerFormatedDatVal(_Time.Date);
                    }
                }
            }
        }

        private void LoadActionToViewGrid(GridView Grd_ViewBook,string _UserId , int _UserType)
        {
            string status;
            if (Grd_ViewBook.Rows.Count > 0)
            {

                foreach (GridViewRow gv in Grd_ViewBook.Rows)
                {

                    //DropDownList Drp_Status = (DropDownList)gv.FindControl("Drp_Status");
                    Label Label_Action = (Label)gv.FindControl("Label_Action");
                    //Drp_Status.Items.Clear();

                    status = MyLibMang.GetTheCurrentViewGridStatus(gv.Cells[0].Text.ToString(), _UserId, _UserType);
                    if (status == "Issued")
                    {
                        //Drp_Status.Items.Add(new ListItem("Issued", "2"));
                        Label_Action.Text = "Issued";
                    }
                    else if (status == "Booked")
                    {
                        //Drp_Status.Items.Add(new ListItem("Booked", "1"));
                        Label_Action.Text = "Booked";
                    }
                }
            }
        }

        //protected void Btn_yes_Click(object sender, EventArgs e)
        //{
        //    Pnl_userdetails.Visible = true;
        //   // Pnl_bookarea.Visible = true;
        //    LaoadUserTypeToDrpList();
        //    Btn_AddUser.Visible = true;
        //}

        //private bool validUserData(out string message)
        //{
        //    bool _valid = true;
        //    message = "";
        //    if (Txt_userid.Text.Trim() == "")
        //    {
        //        _valid = false;
        //        message = "User Id  cannot be empty";
        //    }
        //    else if(Txt_username.Text.Trim() == "")
        //    {
        //        _valid = false;
        //        message = "User name cannot be empty";
        //    }
        //    else if (Txt_class_department.Text.Trim() == "")
        //    {
        //        _valid = false;
        //        message = "Class/Department cannot be empty";
        //    }
        //    else if (Drp_usertype.SelectedItem.ToString() == "")
        //    {
        //        _valid = false;
        //        message = "User type cannot be empty";
        //    }
        //    return _valid;
        //}

        protected void Btn_No_Click(object sender, EventArgs e)
        {
            Pnl_userdetails.Visible = false;
            Pnl_bookarea.Visible = false;
        }
     
        private void LoadStudent(int _UserId, int _UserType, int _searchtype)
        {
            Txt_SaveUser.Text = _UserId.ToString();
            string sql="";
            if (_UserType == 1 )
            {
                //if (_searchtype == 0)
                
                    //sql = "select tblstudent.StudentName, tblclass.ClassName from tblstudent  inner join tblstudentclassmap on tblstudentclassmap.StudentId = tblstudent.Id inner join tblclass on tblclass.Id = tblstudentclassmap.ClassId where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " and tblstudent.`Status`=1 and tblstudent.AdmitionNo='" + _UserId + "'";
                sql = "select tblstudent.StudentName, tblclass.ClassName from tblstudent  inner join tblstudentclassmap on tblstudentclassmap.StudentId = tblstudent.Id inner join tblclass on tblclass.Id = tblstudentclassmap.ClassId where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " and tblstudent.`Status`=1 and tblstudent.Id=" + _UserId ;
                //else
                  //  sql = "select tblstudent.StudentName, tblclass.ClassName from tblstudent  inner join tblstudentclassmap on tblstudentclassmap.StudentId = tblstudent.Id inner join tblclass on tblclass.Id = tblstudentclassmap.ClassId where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " and tblstudent.`Status`=1 and tblstudent.StudentName='" + _UserId + "'";
                
                MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Lbl_username.Text = MyReader.GetValue(0).ToString();
                    Lbl_class_department.Text = MyReader.GetValue(1).ToString();
                    lbl_UserType.Text = "Student";
                    Lbl_class_department.Visible = true;
                    Lbl_Class.Visible = true;
                }
            }
            else if (_UserType==2)
            {
                 sql = "select tbluser.SurName from tbluser where tbluser.Id=" + _UserId + " and tbluser.`Status`=1";
                MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Lbl_username.Text = MyReader.GetValue(0).ToString();
                    Lbl_class_department.Visible = false;
                    Lbl_Class.Visible = false;
                    lbl_UserType.Text = "Staff";
                }
            }
              MyReader.Close();
        }

        private bool validuserId(out string _message)
        {
            _message = "";
            bool _valid = true;
            if (Txt_userid.Text.Trim() == "")
            {
                _valid = false;
                _message=" Please enter userId";
            }
            return _valid;
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
            dt.Columns.Add("Author");
            int _count = int.Parse(Txt_bookid.Text);
            for (i = 0; i < _count; i++)
            {
                dr = _bookdataset.Tables["books"].NewRow();
                dr["Count"] = i + 1;
                dr["BookId"] = i + 1;
                _bookdataset.Tables["books"].Rows.Add(dr);

            }
            return _bookdataset;
        }

        protected void Btn_addbook_Click(object sender, EventArgs e)
        { 
            string SearchType = Drp_booksearch.SelectedValue.ToString();
            if (Txt_bookid.Text.Trim() != "")
            {
                if (Bookexist(SearchType))
                {
                    WC_bookcopies.SearchType = Drp_booksearch.SelectedItem.ToString();
                    WC_bookcopies.SearchValue = Txt_bookid.Text.Trim();
                    WC_bookcopies.valueType = -1;
                    WC_bookcopies.Show();
                }
                else
                {
                    WC_MessageBox.ShowMssage("No Items Found");
                }
            }
            else
            {
                WC_MessageBox.ShowMssage("Enter search key word");
            }
           
        }

        private bool Bookexist(string SearchType)
        {
             string sql="";
             if (Txt_bookid.Text.Trim() != "")
             {
                 if (SearchType == "0")
                     sql = "select distinct tblbookmaster.Id from tblbookmaster inner join tblbooks on tblbooks.BookId= tblbookmaster.Id   where tblbookmaster.BookName='" + Txt_bookid.Text.Trim() + "'";
                 else if (SearchType == "1")
                 {
                     sql = "select distinct tblbookmaster.Id from tblbookmaster inner join tblbooks on tblbooks.BookId= tblbookmaster.Id where   tblbooks.BookNo=" + int.Parse(Txt_bookid.Text.Trim()) + "";
                 }
                 else if (SearchType == "2")
                 {
                     sql = "select distinct tblbookmaster.Id from tblbookmaster inner join tblbooks on tblbooks.BookId= tblbookmaster.Id where   tblbooks.Barcode='" + Txt_bookid.Text.Trim() + "'";
                 }
                 MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
                 if (MyReader.HasRows)
                 {
                     return true;
                 }
                 else
                     return false;
             }
             else
             {
                 return false;
             }
        }
        private void IssueBooks(string _BookNo )
        {
            
            int Issued;
            int Booked;
            if (MyLibMang.IsbookExists(_BookNo))
            {
                Issued = MyLibMang.BookAlreadyissued(_BookNo); // 0 - not issued
                Booked = MyLibMang.BookAlreadBooked(_BookNo);  // 1 - issued     
                if ((Issued == 1) && (Booked == 1)) // BOOK IS ISSUED AND BOOKED
                {
                    Lbl_msg.Text = "The book is already issued and booked";
                    this.MPE_MessageBox.Show();
                }
                else if ((Issued == 0) && (Booked == 1))// issued =0 and booked =1
                {
                    string message;
                    if (MyLibMang.DifferentUserBooked(Txt_SaveUser.Text, _BookNo, out message,int.Parse(RdoBtn_UserType.SelectedValue)))
                    {

                        BKNumber.Text = _BookNo.ToString();
                        Label_issue.Text = "The book is already booked by '" + message + "'. Do you want to continue any way";
                        this.MPE_Issue.Show();
                    }
                    else if (MyLibMang.SameUserBooked(Txt_SaveUser.Text, _BookNo,int.Parse(RdoBtn_UserType.SelectedValue)))
                    {
                        if (GrdBooks.Rows.Count > 0)
                        {
                            if (BookExistsInGrid(GrdBooks, _BookNo))
                            {
                                GrdBooks.DataSource = AddPreviousData(_BookNo);
                                GrdBooks.DataBind();
                                LoadActionToGrid(GrdBooks);

                            }
                            else
                            {
                                Lbl_msg.Text = "The Book is already added";
                                this.MPE_MessageBox.Show();
                            }
                        }
                        else
                        {
                            string sql = "select tblbooks.BookNo, tblbookmaster.BookName, tblbookmaster.Author from tblbooks inner join tblbookmaster on tblbookmaster.Id = tblbooks.BookId where tblbooks.BookNo='" + _BookNo + "'";
                            MydataSet = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                            if (MydataSet.Tables[0].Rows.Count > 0)
                            {
                                GrdBooks.DataSource = MydataSet;
                                GrdBooks.DataBind();
                                Pnl_bookgrid.Visible = true;
                                LoadActionToGrid(GrdBooks);

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


                }
                //else if (((Issued == 1) && (Booked == 0)) && (MyLibMang.IssuedToSameUser(Txt_SaveUser.Text, _BookNo,int.Parse(RdoBtn_UserType.SelectedValue))))
                //{
                //    Lbl_msg.Text = "The book is issued to the same user";
                //    this.MPE_MessageBox.Show();
                //}
                else if (MyLibMang.IssuedToSameUser(Txt_SaveUser.Text, _BookNo, int.Parse(RdoBtn_UserType.SelectedValue)))
                {
                    Lbl_msg.Text = "The book is issued to the same user";
                    this.MPE_MessageBox.Show();
                }

                else if (((Issued == 1) && (Booked == 0)) || ((Issued == 0) && (Booked == 0))) // BOOK IS ISSUED AND NOT BOOKED OR THE BOOK IS NOT ISSUED AND BOOKED
                {
                    if (GrdBooks.Rows.Count > 0)
                    {
                        if (BookExistsInGrid(GrdBooks, _BookNo))
                        {
                            GrdBooks.DataSource = AddPreviousData(_BookNo);
                            GrdBooks.DataBind();
                            LoadActionToGrid(GrdBooks);
                        }
                        else
                        {
                            Lbl_msg.Text = "The Book is already added";
                            this.MPE_MessageBox.Show();
                        }
                    }
                    else
                    {
                        string sql = "select tblbooks.BookNo, tblbookmaster.BookName, tblbookmaster.Author from tblbooks inner join tblbookmaster on tblbookmaster.Id = tblbooks.BookId where tblbooks.BookNo='" + _BookNo + "'";
                        MydataSet = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                        if (MydataSet.Tables[0].Rows.Count > 0)
                        {
                            GrdBooks.DataSource = MydataSet;
                            GrdBooks.DataBind();
                            Pnl_bookgrid.Visible = true;
                            LoadActionToGrid(GrdBooks);

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

            }
            else
            {
                Lbl_msg.Text = "No books found";
                this.MPE_MessageBox.Show();
            }
        }

       protected void Btn_IssueBk_click(object sender, EventArgs e)
        {
            string _BookNo = BKNumber.Text.Trim();
            if (GrdBooks.Rows.Count > 0)
            {
                if (BookExistsInGrid(GrdBooks, _BookNo))
                {
                    GrdBooks.DataSource = AddPreviousData(_BookNo);
                    GrdBooks.DataBind();
                    LoadActionToGrid(GrdBooks);
                }
                else
                {
                    Lbl_msg.Text = "The Book is already added";
                    this.MPE_MessageBox.Show();
                }
            }
            else
            {
                string sql = "select tblbooks.BookNo, tblbookmaster.BookName, tblbookmaster.Author from tblbooks inner join tblbookmaster on tblbookmaster.Id = tblbooks.BookId where tblbooks.BookNo='" + _BookNo + "'";
                MydataSet = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    GrdBooks.DataSource = MydataSet;
                    GrdBooks.DataBind();
                    Pnl_bookgrid.Visible = true;
                    LoadActionToGrid(GrdBooks);

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

        private void LoadActionToGrid(GridView GrdBooks)
        {
            string status;
            if (GrdBooks.Rows.Count > 0)
            {  

                foreach (GridViewRow gv in GrdBooks.Rows)
                {

                        DropDownList Drp_Status = (DropDownList)gv.FindControl("Drp_Status");
                        Drp_Status.Items.Clear();

                        status = MyLibMang.GetTheCurrentStatus( gv.Cells[0].Text.ToString());
                        if (status == "Issued")
                        {
                            Drp_Status.Items.Add(new ListItem("Book", "2"));
                        }
                        else if (status == "Booked")
                        {
                            Drp_Status.Items.Add(new ListItem("Issue", "1"));
                        }
                        else
                        {
                            Drp_Status.Items.Add(new ListItem("Issue", "1"));
                            //Drp_Status.Items.Add(new ListItem("Book", "2"));
                       }
                }
            }
        }

        private bool BookExistsInGrid(GridView GrdBooks, string _BookNo)
        {
            bool _valid = true;
            foreach (GridViewRow gv in GrdBooks.Rows)
            {
                
                if (gv.Cells[0].Text.ToString().ToLower() == _BookNo.ToString().ToLower())
                {
                    _valid = false;
                }
            }
            return _valid;
        }

        private DataSet AddPreviousData(string _BookNo)
        {
           
            DataSet _bookdataset = new DataSet();
            DataTable dt;
            DataRow dr;
            int i;
            _bookdataset.Tables.Add(new DataTable("books"));
            dt = _bookdataset.Tables["books"];
           // dt.Columns.Add("Count");
            dt.Columns.Add("BookNo");
            dt.Columns.Add("BookName");
            dt.Columns.Add("Author");
            
            i = 0;
            foreach (GridViewRow gv in GrdBooks.Rows)
            {             
                    i++;
                    dr = _bookdataset.Tables["books"].NewRow();
                    //dr["Count"] = i;
                    dr["BookNo"] = gv.Cells[0].Text.ToString();
                    dr["BookName"] = gv.Cells[1].Text.ToString();
                    dr["Author"] = gv.Cells[2].Text.ToString();
                    _bookdataset.Tables["books"].Rows.Add(dr);                
                   
            }

            string sql = "select tblbooks.BookNo, tblbookmaster.BookName, tblbookmaster.Author from tblbooks inner join tblbookmaster on tblbookmaster.Id = tblbooks.BookId where tblbooks.BookNo='" + _BookNo + "'";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                dr = _bookdataset.Tables["books"].NewRow();
                //dr["Count"] = i;
                dr["BookNo"] = MyReader.GetValue(0).ToString();
             
                dr["BookName"] = MyReader.GetValue(1).ToString();
                dr["Author"] = MyReader.GetValue(2).ToString();
                _bookdataset.Tables["books"].Rows.Add(dr);
                
            }
            
            return _bookdataset;
        }

        //protected void Btn_AddUser_Click(object sender, EventArgs e)
        //{
        //    string message;
        //    if (validUserData(out message))
        //    {        
        //        MyLibMang.AddUser(Txt_userid.Text, Txt_username.Text, Txt_desc.Text, int.Parse(Drp_usertype.SelectedValue.ToString()), Txt_class_department.Text);
        //        Lbl_msg.Text = "User data is saved";
        //        Txt_SaveUser.Text = Txt_userid.Text;
        //        this.MPE_MessageBox.Show();
        //        Pnl_bookarea.Visible = true;
        //        Btn_AddUser.Visible = false;
        //        Lbl_GridMessage.Text = "No book(s) are pending for this user";
        //    }
        //    else
        //    {
        //        Lbl_msg.Text = message;
        //        this.MPE_MessageBox.Show();
        //    }
        //}

        protected void GrdBooks_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string _temp = GrdBooks.Rows[e.RowIndex].Cells[0].Text.ToString();
            try
            {
                if (GrdBooks.Rows.Count > 1)
                {

                    GrdBooks.DataSource = Delete_Row(_temp);
                    GrdBooks.DataBind();
                    LoadActionToGrid(GrdBooks);
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




        }

        private DataSet Delete_Row(string _temp)
        {
            DataSet _bookdataset = new DataSet();
            DataTable dt;
            DataRow dr;
            //int i;
            _bookdataset.Tables.Add(new DataTable("books"));
            dt = _bookdataset.Tables["books"];
          //  dt.Columns.Add("Count");
            dt.Columns.Add("BookNo");
            dt.Columns.Add("BookName");
            dt.Columns.Add("Author");
            //i = 0;
            foreach (GridViewRow gv in GrdBooks.Rows)
            {
                if (gv.Cells[0].Text.ToString() != _temp)
                {
                    //i++;
                    dr = _bookdataset.Tables["books"].NewRow();
                    //dr["Count"] = i;
                    dr["BookNo"] = gv.Cells[0].Text.ToString();
                    dr["BookName"] = gv.Cells[1].Text.ToString();
                    dr["Author"] = gv.Cells[2].Text.ToString();

                    _bookdataset.Tables["books"].Rows.Add(dr);
                }
            }
            return _bookdataset;
        }

        protected void Btn_Issue_Click(object sender, EventArgs e)
        {
            if (GrdBooks.Rows.Count > 0)
            {
                if (Txt_SaveUser.Text !="")
                {
                    string BookId;
                    int valid1 = 1,valid2=1;
                    
                    int Issuedbooks = 0,BookedBooks=0;
                    int UserId = MyLibMang.GetUserId(Txt_SaveUser.Text.Trim(),int.Parse(RdoBtn_UserType.SelectedValue.ToString()));

                    if (UserId != -1)
                    {
                         foreach (GridViewRow gv in GrdBooks.Rows)
                            {
                                DropDownList Drp_Status = (DropDownList)gv.FindControl("Drp_Status");
                                BookId = gv.Cells[0].Text.ToString();
                                if (Drp_Status.SelectedValue == "1")
                                {

                                        if (MyLibMang.IssueLimNotExceeds(UserId, int.Parse(RdoBtn_UserType.SelectedValue)))
                                        {

                                            MyLibMang.IssueBook(UserId.ToString(), BookId, int.Parse(RdoBtn_UserType.SelectedValue.ToString()));
                                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "Book:" + gv.Cells[1].Text + " issued", 1);
                                            Issuedbooks += 1;
                                            if (MyLibMang.SameUserBooked(Txt_SaveUser.Text, BookId, int.Parse(RdoBtn_UserType.SelectedValue)))
                                            {
                                                MyLibMang.RemoveBooking(Txt_SaveUser.Text, BookId, int.Parse(RdoBtn_UserType.SelectedValue));
                                            }
                                        }
                                        else
                                        {
                                            valid1 = 0;
                                        }

                                 
                                }
                                else if (Drp_Status.SelectedValue == "2")
                                {

                                        if (MyLibMang.BookingLimNotExceeds(UserId, int.Parse(RdoBtn_UserType.SelectedValue)))
                                        {

                                            MyLibMang.Book_Book(UserId.ToString(), BookId, int.Parse(RdoBtn_UserType.SelectedValue.ToString()));
                                            BookedBooks += 1;
                                        }
                                        else
                                        {
                                            valid2 = 0;
                                        }

                                }
                            }

                            Cleardata();
                            LoadBooks(Txt_SaveUser.Text, int.Parse(RdoBtn_UserType.SelectedValue));
                            LoadDateToViewGrid(Grd_ViewBook, Txt_SaveUser.Text, int.Parse(RdoBtn_UserType.SelectedValue));
                            Lbl_GridMessage.Text = "";
                            Lbl_msg.Text = GenerateMessage(valid1, valid2, Issuedbooks, BookedBooks);
                            
                            this.MPE_MessageBox.Show();
                           
                    }
                }
            }
        }

        private string GenerateMessage(int valid1, int valid2,int NumIssue,int Numbook)
        {
            string message = "";

                if (NumIssue != 0)
                {
                    message = message + "  Issued: " + NumIssue;
                }
                if (Numbook != 0)
                {
                    message = message + " Booked: " + Numbook;
                }
                if (NumIssue == 0)
                {
                    message = message + " Issued: 0";
                }
                if (Numbook == 0)
                {
                    message = message + " Booked: 0";
                }
                if (valid1 != 1 && NumIssue == 0 && Numbook == 0)
                {
                    message = "Maximum count of books issuing is exceed.";

                }
                else if (valid2 != 1 && Numbook == 0 && NumIssue == 0)
                {
                    message = "Maximum count of  booking is exceed.";
                }
          
           return message;
        }

        private void Cleardata()
        {
            Txt_bookid.Text = "";
            GrdBooks.DataSource = null;
            GrdBooks.DataBind();
            Pnl_bookgrid.Visible = false;
        }

        protected void RdoBtn_UserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem l1,l2;
            Drp_SearchBy.Items.Clear();
            Pnl_bookarea.Visible = false;
            Pnl_bookgrid.Visible = false;
            Pnl_userdetails.Visible = false;
            if (RdoBtn_UserType.SelectedValue.ToString() == "1")
            {

                l1 = new ListItem("Admission No", "0");
                l2 = new ListItem("Student Name", "1");

                Drp_SearchBy.Items.Add(l1);
                Drp_SearchBy.Items.Add(l2);
                Drp_SearchBy.SelectedValue = "1";

            }
            else
            {
                l1 = new ListItem("Staff Id", "0");
                l2 = new ListItem("Staff Name", "1");

                Drp_SearchBy.Items.Add(l1);
                Drp_SearchBy.Items.Add(l2);
                Drp_SearchBy.SelectedValue = "1";
            }
            Txt_userid.Text = "";
            Txt_userid_AutoCompleteExtender.ContextKey = RdoBtn_UserType.SelectedValue.ToString() + "\\" + Drp_SearchBy.SelectedValue;
        }

        protected void Drp_SearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            Txt_userid_AutoCompleteExtender.ContextKey = RdoBtn_UserType.SelectedValue.ToString() + "\\" + Drp_SearchBy.SelectedValue;
            Txt_userid.Text = "";
        }

        protected void Drp_booksearch_selectedIndexChanged(object sender, EventArgs e)
        {
            Txt_bookid_AutoCompleteExtender.ContextKey = Drp_booksearch.SelectedValue.ToString();
            Txt_bookid.Text = "";
        }
       
        //private void UpdateUser()
        //{
        //    string message;
        //    if (validUserData(out message))
        //    {

        //        MyLibMang.UpdateUserdetails(Txt_userid.Text.ToString(), Txt_username.Text, Txt_desc.Text, int.Parse(Drp_usertype.SelectedValue.ToString()), Txt_class_department.Text);
        //        Lbl_msg.Text = "User data is Updated";
   

        //    }
        //    else
        //    {
        //        Lbl_msg.Text = message;
        //        this.MPE_MessageBox.Show();
        //    }
             
        //}

        //protected void Btn_edid_Click1(object sender, EventArgs e)
        //{
           
            
        //        UpdateUser();
            
        //}




       

    }
}
