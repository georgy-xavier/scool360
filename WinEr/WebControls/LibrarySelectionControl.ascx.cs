using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;
namespace WinEr.WebControls
{
    public partial class BookCopiesControl : System.Web.UI.UserControl
    {
        private KnowinUser MyUser;
        private OdbcDataReader m_Myreader = null;
        private LibraryManagClass MyLibMang;
        private OdbcDataReader MyReader = null;
        private DataSet M_Dataset = null;
        public event EventHandler EventSelection;

        private string _Type = "";
        private string _Value ="";
        private int _ValueType = 0;
        private int _SelectedId = -1;

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
            

        }



        public string SearchType
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
            }

        }
        public int valueType
        {
            get
            {
                return _ValueType;
            }
            set
            {
                _ValueType = value;
            }

        }
        public string SearchValue
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }
        public int SelectedId
        {

            get
            {
                return _SelectedId;
            }
        }
        public void Show()
        {
            Lbl_Error.Text = "";
            Grd_Books.DataSource = null;
            Grd_Books.DataBind();
            Grd_Books.Columns[0].Visible = true;
            Grd_Books.Columns[2].Visible = true;
            string sql = "";
            Lbl_SearchType.Text = _Type;
            if (_Type=="Student")
            {
                Grd_Books.Columns[1].HeaderText = "Student Name";
                Grd_Books.Columns[2].HeaderText = "Class Name";
                Grd_Books.Columns[3].HeaderText = "Roll N0";
                if(_ValueType==1)
                    sql = "select tblstudent.id as Id, tblstudent.StudentName as Name , tblclass.ClassName as Val2,tblstudent.RollNo as Val3 from tblstudent  inner join tblclass on tblclass.Id = tblstudent.LastClassId where tblstudent.StudentName='" + _Value + "' and tblstudent.Status=1 ";
                else
                    sql = "select tblstudent.id as Id, tblstudent.StudentName as Name , tblclass.ClassName as Val2,tblstudent.RollNo as Val3 from tblstudent  inner join tblclass on tblclass.Id = tblstudent.LastClassId  where tblstudent.AdmitionNo='" + _Value + "' and tblstudent.Status=1 ";

                
            }
            else if(_Type=="Staff")
            {
                Grd_Books.Columns[1].HeaderText = "Staff Name";
                Grd_Books.Columns[2].HeaderText = "UserName";
                
                Grd_Books.Columns[3].HeaderText = "Role";
                if (_ValueType == 1)
                    
                    sql = "select tbluser.id as Id, tbluser.SurName  as Name ,tbluser.UserName as Val2, tblrole.RoleName as Val3 from tbluser inner join tblrole on tblrole.Id= tbluser.RoleId where tbluser.SurName='" + _Value + "' and tbluser.Status=1 and tbluser.RoleId<>1";
                else
                    sql = "select tbluser.id as Id, tbluser.SurName  as Name,tbluser.UserName as Val2 , tblrole.RoleName as Val3 from tbluser inner join tblrole on tblrole.Id= tbluser.RoleId where tbluser.UserName='" + _Value + "' and tbluser.Status=1 and tbluser.RoleId<>1";

            }
            else if (_Type == "BarCode")
            {
                Grd_Books.Columns[0].HeaderText = "Book No";
                Grd_Books.Columns[1].HeaderText = "Book Name";
                Grd_Books.Columns[2].HeaderText = "Author";
                Grd_Books.Columns[3].HeaderText = "Edition";
                if (_ValueType == -1)
                    sql = "select tblbooks.BookNo as Id, tblbookmaster.BookName  as Name ,tblbookmaster.Author as Val2,tblbookmaster.Edition as Val3 from tblbooks inner join tblbookmaster on tblbookmaster.Id= tblbooks.BookId where tblbooks.Barcode='" + _Value + "'";
                else
                    sql = "select tblbookissue.BookNo  as Id, tblbookmaster.BookName as Name,tblbookmaster.Author as Val2, tblbookmaster.Edition as Val3 from tblbookissue inner join tblbooks on tblbooks.BookNo=tblbookissue.BookNo inner join tblbookmaster on tblbookmaster.Id= tblbooks.BookId where tblbooks.Barcode='" + _Value + "'";
            }
            else if (_Type == "Book Name")
            {
                Grd_Books.Columns[0].HeaderText = "Book No";
                Grd_Books.Columns[1].HeaderText = "Book Name";
                Grd_Books.Columns[2].HeaderText = "Author";
                Grd_Books.Columns[3].HeaderText = "Edition";
                if (_ValueType == -1)
                    sql = "select tblbooks.BookNo as Id, tblbookmaster.BookName as Name  ,tblbookmaster.Author as Val2, tblbookmaster.Edition as Val3 from tblbooks inner join tblbookmaster on tblbookmaster.Id= tblbooks.BookId where tblbookmaster.BookName='" + _Value + "'";
                else
                    sql = "select tblbookissue.BookNo as Id, tblbookmaster.BookName as Name,tblbookmaster.Author as Val2, tblbookmaster.Edition as Val3  from tblbookissue inner join tblbooks on tblbooks.BookNo=tblbookissue.BookNo inner join tblbookmaster on tblbookmaster.Id= tblbooks.BookId where tblbookmaster.BookName='" + _Value + "'";
            }
            else if (_Type == "Book No")
            {
                Grd_Books.Columns[0].HeaderText = "Book No";
                Grd_Books.Columns[1].HeaderText = "Book Name";
                Grd_Books.Columns[2].HeaderText = "Author";
                Grd_Books.Columns[3].HeaderText = "Edition";
                if (_ValueType == -1)
                    sql = "select tblbooks.BookNo as Id, tblbookmaster.BookName  as Name,tblbookmaster.Author as Val2 , tblbookmaster.Edition as Val3 from  tblbooks inner join tblbookmaster on tblbookmaster.Id= tblbooks.BookId where tblbooks.BookNo='" + _Value + "'";
                else
                    sql = "select tblbookissue.BookNo as Id ,tblbookmaster.BookName as Name,tblbookmaster.Author as Val2 , tblbookmaster.Edition as Val3 from tblbookissue inner join tblbooks on tblbooks.BookNo=tblbookissue.BookNo inner join tblbookmaster on tblbookmaster.Id= tblbooks.BookId where  tblbooks.BookNo=" + int.Parse(_Value.ToString());
            }

            M_Dataset = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);



            if (M_Dataset.Tables[0].Rows.Count > 0)
            {
                ViewState["Dataset"] = M_Dataset;
                Grd_Books.DataSource = M_Dataset;
                Grd_Books.DataBind();
                if (_Type == "Student" || _Type == "Staff")
                {
                    Grd_Books.Columns[0].Visible = false;
                    if (_Type == "Staff")
                    {
                        Grd_Books.Columns[2].Visible = false;
                    }
                }

                
            }
            else
            {
                Lbl_Error.Text = "No datas found";
            }
            

            MPE_BookDetails.Show();
        }


        protected void Grd_Books_SelectedIndexChanged(object sender, EventArgs e)
        {
            int Id = -1;

            Grd_Books.Columns[0].Visible = true;
            int.TryParse(Grd_Books.SelectedRow.Cells[0].Text.ToString(), out Id);
            SearchType = Lbl_SearchType.Text;
            Grd_Books.Columns[0].Visible = false;
            _SelectedId = Id;
            EventSelection(this, e);
        }
        protected void Grd_Books_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Books.PageIndex = e.NewPageIndex;
            DataSet MydataSet = (DataSet)ViewState["Dataset"];
            Grd_Books.DataSource = MydataSet;
            Grd_Books.DataBind();
            MPE_BookDetails.Show();
        }


       
    }
}