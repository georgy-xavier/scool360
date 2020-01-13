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
    public partial class DeleteBook : System.Web.UI.Page
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
                    LoadBookData();
                }
            }

        }
        private void LoadBookData()
        {
            string sql = "select tblbookmaster.BookName from tblbooks inner join tblbookmaster on tblbookmaster.Id = tblbooks.BookId where tblbooks.BookNo='" + Session["BookId"] + "'";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Lbl_BkName.Text = MyReader.GetValue(0).ToString();
                Lbl_BookCode.Text = Session["BookId"].ToString(); 
            }
        }

        protected void Btn_Delete_Click(object sender, EventArgs e)
        {
                int BkId;
                string BookId = Session["BookId"].ToString();
                string message;
                bool Bkd = false; 
                Lbl_msg.Text = "";
                    if (int.Parse(Rdo_List.SelectedValue.ToString()) == 0)
                    {
                        if (ValidForDelete(out message, BookId))
                        {
                            if (MyLibMang.BookAlreadBooked(BookId) == 1)
                            {

                                Bkd = true;
                               // Lbl_msg.Text = "Book is Booked by someone. Deleting will cancel the booking";
                               // this.MPE_MessageBox.Show();
                            }                          
                            if (Bkd)
                                {
                                    MyLibMang.CancelBookin(BookId);
                                }
                            MyLibMang.DeleteBook(BookId);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "Book Deleted", 1);
                            Session["BookId"] = null;
                            Lbl_DelMs.Text = "The Book(s) are deleted";
                            this.MPE_Cancel.Show();
                        }
                        else
                        {
                         Lbl_msg.Text = message;
                         this.MPE_MessageBox.Show();
                        }
                    }
                    else if (int.Parse(Rdo_List.SelectedValue.ToString()) == 1)
                    {

                        BkId = MyLibMang.GetBkId(BookId);
                        if (BkId != -1)
                        {
                            if (MyLibMang.NoBooksIssued(BkId))
                            {
                                if (MyLibMang.NobooksBooked(BkId))
                                {
                                    Bkd = true;
                                   // Lbl_msg.Text = "Some Book(s) are Booked. Deleting will cancel the booking";
                                    //this.MPE_MessageBox.Show();
                                }
                               
                                if (Bkd)
                                {
                                    MyLibMang.CancelAllBooking(BkId);
                                }
                                MyLibMang.DeleteAlBkCopy(BkId);
                                Session["BookId"] = null;
                                Lbl_DelMs.Text = "The Book(s) are deleted";
                                this.MPE_Cancel.Show();
                            }
                            else
                            {
                                Lbl_msg.Text = "Some of the books are issued. Please Collect them and try";
                                this.MPE_MessageBox.Show();
                            }
                        }
                    }
                   
               
        }

        private bool ValidForDelete(out string message, string _BookId)
        {
            message = "";
            bool _Valid = true;

            if (MyLibMang.BookIssued(_BookId))                
            {
                _Valid = false;
                message = "The book is issued to someone. Please collect the book and try again";
            }
            return _Valid;
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("BookSearchDetails.aspx");
        }

        protected void Btn_DelOk_Click(object sender, EventArgs e)
        {
            Response.Redirect("LlbraryHome.aspx");
        }

        
    }
}
