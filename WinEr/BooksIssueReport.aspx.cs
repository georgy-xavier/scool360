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
    public partial class BooksIssueReport : System.Web.UI.Page
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
            if (!MyUser.HaveActionRignt(774))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    Pnl_BookDetails.Visible = false;
                    LoadCategories();
                }
                Txt_Search_AutoCompleteExtender.ContextKey = "0";
            }
        }

        private void LoadCategories()
        {

            MyReader = MyLibMang.GetCategories();
            Drp_Category.Items.Clear();
            ListItem li;
            if (MyReader.HasRows)
            {
                li = new ListItem("All", "0");
                Drp_Category.Items.Add(li);
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Category.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No Categories found", "-1");
                Drp_Category.Items.Add(li);
            }
        }

        protected void GrdBooks__PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet Ds = FillGrid();
            if (Ds.Tables[0].Rows.Count > 0)
            {
                GrdBooks.DataSource = Ds;
                GrdBooks.DataBind();
                Pnl_BookDetails.Visible = true;
            }
            else
            {
                GrdBooks.DataSource = null;
                GrdBooks.DataBind();
                Pnl_BookDetails.Visible = false;
                Lbl_Err.Text = "No datas found";
            }
            GrdBooks.PageIndex = e.NewPageIndex;
        }

      

        protected void Btn_Search_OnClick(object sender, EventArgs e)
        {
             DataSet Ds= FillGrid();
            if (Ds.Tables[0].Rows.Count > 0)
            {
                GrdBooks.DataSource = Ds;
                GrdBooks.DataBind();
                Pnl_BookDetails.Visible = true;
            }
            else
            {
                Lbl_Err.Text = "No books found as per the  conditions";
                GrdBooks.DataSource = null;
                GrdBooks.DataBind();
                Pnl_BookDetails.Visible = false;
            }
                
        }


        protected void Btn_Export_Click(object sender, EventArgs e)
        {


            DataSet Dt = FillGrid();
            if (Dt.Tables[0].Rows.Count > 0)
            {
                
                string FileName = "BookList";
                string _ReportName = "BookList";
                if (!WinEr.ExcelUtility.ExportDataToExcel(Dt, _ReportName, FileName, MyUser.ExcelHeader))
                {
                    Lbl_Err.Text = "Please try again.";
                    //this.MPE_MessageBox.Show();
                }
            }
        }
        private DataSet FillGrid()
        {
            int _Temp=0;
            string sql = "Select tblbookmaster.Id, tblbooks.BookNo,  tblbookmaster.BookName, tblbookmaster.Author, tblbookmaster.Edition, tblbookcatogory.CatogoryName as Category, DATE_FORMAT( tblbookissue.DateOfIssue, '%d/%m/%Y') as Issuedate , tblbookissue.UserId, tblbookissue.UserType as UserTypeID, tblbookusertype.USerType from tblbooks inner join tblbookmaster on tblbookmaster.Id= tblbooks.BookId inner join tblbookissue  on tblbookissue.BookNo= tblbooks.BookNo inner join tblbookusertype on tblbookusertype.Id= tblbookissue.UserType inner join tblbookcatogory on tblbookcatogory.Id= tblbookmaster.CatagoryId";

            if (RdoBtn_Due.SelectedValue == "1")
            {
                if (_Temp == 0)
                    sql = sql + " where ";
                else if (_Temp == 1)                
                    sql = sql + "and";

                TimeSpan maxDays; 
                TimeSpan.TryParse(MyLibMang.GetMaxDays().ToString(), out maxDays);
                DateTime Dt = DateTime.Now.Date.Add(-maxDays);
                sql = sql + " tblbookissue.DateOfIssue <='" + Dt.ToString("s") + "'";
                _Temp = 1;
                
            }
            if (Drp_UserWise.SelectedValue == "1")
            {
                if (_Temp == 0)
                    sql = sql + " where ";
                else if (_Temp == 1)
                    sql = sql + "and";
                sql = sql + " tblbookissue.UserType=2";
                _Temp = 1;
            }
            else if (Drp_UserWise.SelectedValue == "2")
            {
                if (_Temp == 0)
                    sql = sql + " where ";
                else if (_Temp == 1)
                    sql = sql + " and "; 
                sql = sql + "  tblbookissue.UserType=1";
                _Temp = 1;
            }
            if (Drp_Category.SelectedValue != "-1" && Drp_Category.SelectedValue != "0")
            {
                if (_Temp == 0)
                    sql = sql + " where ";
                else if (_Temp == 1)
                    sql = sql + " and " ;
                sql = sql + "   tblbookmaster.CatagoryId="+int.Parse(Drp_Category.SelectedValue.ToString());
                _Temp = 1;
            }
            if(Txt_Search.Text.Trim() != "")
            {
                if (_Temp == 0)
                    sql = sql + " where ";
                else if (_Temp == 1)
                    sql = sql + " and ";
                sql = sql + "   tblbookmaster.BookName='" + Txt_Search.Text.Trim() + "'";
                _Temp = 1;
            }
            MydataSet = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (MydataSet.Tables[0].Rows.Count > 0)
            { 
                ViewState["LibraryValues"] = sql;
                DataSet _NewDetails = BuildDataset(MydataSet);
                DataSet _BookUserDetails = LoadTakenUser(_NewDetails);
                DataSet _BookFineDetails = LoadFine(_BookUserDetails);
                return _BookFineDetails;
            }
            return MydataSet;
        }

        private DataSet LoadTakenUser(DataSet _BookDetails)
        {

            foreach (DataRow dr_values in _BookDetails.Tables[0].Rows)
            {
                string Type=dr_values["UserTypeID"].ToString();
                string Id = dr_values["UserId"].ToString();
                string Name=MyLibMang.GetUserName(Type, Id);
                dr_values["Username"] = Name.ToString();
            }
            return _BookDetails;
        }

        private DataSet LoadFine(DataSet _BookDetails)
        {
           
            double Fine;
            double Tottal = 0;

            foreach (DataRow dr_values in _BookDetails.Tables[0].Rows)
            {
                Fine = MyLibMang.GetFine(dr_values["BookNo"].ToString(),int.Parse(dr_values["UserTypeID"].ToString()));
                Fine = Math.Round(Fine);
                dr_values["Fine"] = Fine.ToString("0.0");
                Tottal = Tottal + Fine;
                Tottal = Math.Round(Tottal);
                Lbl_TottalFine.Text = Tottal.ToString("0.0");
            }
            return _BookDetails;
        }

        protected DataSet BuildDataset(DataSet MydataSet)
        {
            DataSet _bookdataset = new DataSet();
            DataTable dt;
            DataRow dr;
            _bookdataset.Tables.Add(new DataTable("books"));

            dt = _bookdataset.Tables["books"];
            dt.Columns.Add("Id");
            dt.Columns.Add("BookNo");
            dt.Columns.Add("BookName");
            dt.Columns.Add("Author");
            dt.Columns.Add("Edition");
            dt.Columns.Add("Category");
            dt.Columns.Add("Issuedate");
            dt.Columns.Add("UserId");
            dt.Columns.Add("UserType");
            dt.Columns.Add("UserTypeID");
            dt.Columns.Add("Username");
          
            dt.Columns.Add("Fine");
           

            foreach (DataRow dr_values in MydataSet.Tables[0].Rows)
            {

                dr = _bookdataset.Tables["books"].NewRow();


                dr["Id"] = dr_values["Id"];
                dr["BookNo"] = dr_values["BookNo"];
                dr["BookName"] = dr_values["BookName"];
                dr["Author"] = dr_values["Author"];
                dr["Edition"] = dr_values["Edition"];
                dr["Category"] = dr_values["Category"];
                dr["Issuedate"] = dr_values["Issuedate"];
                dr["UserId"] = dr_values["UserId"];
                dr["UserTypeID"] = dr_values["UserTypeID"];
                dr["UserType"] = dr_values["UserType"];
                dr["Username"] = "";
                dr["Fine"]="";
              
                _bookdataset.Tables["books"].Rows.Add(dr);

            }


            return _bookdataset;

        }

        protected void Btn_Export_Click(object sender, ImageClickEventArgs e)
        {
            DataSet _Dt = FillGrid();
            if (_Dt.Tables[0].Rows.Count > 0)
            {
                _Dt.Tables[0].Columns.Remove("Id");
                _Dt.Tables[0].Columns.Remove("UserId");
                _Dt.Tables[0].Columns.Remove("UserTypeID");
                

                string FileName = "IssueBooksReport";

                string _ReportName = "Issue Books Report";
                if (!WinEr.ExcelUtility.ExportDataToExcel(_Dt, _ReportName, FileName, MyUser.ExcelHeader))
                {

                    WC_MessageBox.ShowMssage("Please try again");
                }
            }
        }


    }
}
