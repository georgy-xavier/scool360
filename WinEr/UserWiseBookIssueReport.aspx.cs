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
    public partial class UserWiseBookIssueReport : System.Web.UI.Page
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
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(775))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    Pnl_BookDetails.Visible = false;

                    Txt_Search_AutoCompleteExtender.ContextKey = "1" + "\\1";
                    Rdo_btnUserDetails.SelectedValue = "0";
                }
            }

        }

        protected void OnItemSelected(object sender, EventArgs e)
        {
            int Id = WC_bookcopies.SelectedId;

            ViewState["UserId"] = Id.ToString();
            if (WC_bookcopies.SearchType == "Student" || WC_bookcopies.SearchType == "Staff")
            {
                DataSet Ds = FillGrid(Id);
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    GrdBooks.DataSource = Ds;
                    GrdBooks.DataBind();
                    Pnl_BookDetails.Visible = true;
                }
                else
                {
                    Lbl_Err.Text = "No Details Found";
                    GrdBooks.DataSource = null;
                    GrdBooks.DataBind();
                    Pnl_BookDetails.Visible = false;
                }
            }
        }

        protected void Btn_QuickSearch_Click(object sender, EventArgs e)
        {
            Lbl_TottalFine.Text="0";
            Lbl_PaidFine.Text="0";
            
            Lbl_Err.Text = "";
            if (Txt_Search.Text != "")
            {
                WC_bookcopies.SearchType = Drp_Quick.SelectedItem.ToString();
                WC_bookcopies.SearchValue = Txt_Search.Text.Trim();
                WC_bookcopies.valueType = 1;
                WC_bookcopies.Show();
            }
            else
            {
                WC_MessageBox.ShowMssage("Enter search keys");
            }
        }

        private DataSet FillGrid(int UserId)
        {
            int _Temp = 0;
            MydataSet = null;
            string sql = "";
            DataSet validDataset = NewDataset();
            DataSet _NewDetails = validDataset;
           

            if (Rdo_btnUserDetails.SelectedValue == "1")
            {
                sql = "Select tblbookmaster.Id, tblbooks.BookNo,  tblbookmaster.BookName, tblbookmaster.Author, tblbookmaster.Edition, tblbookcatogory.CatogoryName as Category, DATE_FORMAT( tblbookissue.DateOfIssue, '%d/%m/%Y') as Issuedate   from tblbooks inner join tblbookmaster on tblbookmaster.Id= tblbooks.BookId inner join tblbookissue  on tblbookissue.BookNo= tblbooks.BookNo inner join tblbookusertype on tblbookusertype.Id= tblbookissue.UserType inner join tblbookcatogory on tblbookcatogory.Id= tblbookmaster.CatagoryId";

                if (Drp_Quick.SelectedValue == "1")
                {
                    sql = sql + " inner join tblstudent on tblstudent.Id= tblbookissue.UserId where tblstudent.id=" + UserId;

                }
                else if (Drp_Quick.SelectedValue == "2")
                {

                    sql = sql + " inner join tbluser  on tbluser.Id= tblbookissue.UserId where tbluser.id=" + UserId;
                }
                 MydataSet = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

                 if (MydataSet.Tables[0].Rows.Count > 0)
                 {
                     _NewDetails = BuildDataset(validDataset, MydataSet);
                     _NewDetails = LoadFine(_NewDetails);
                 }
                 Lbl_PaidFine.Visible = false;
                 Lbl_Paid.Visible = false;
            }
            else if (Rdo_btnUserDetails.SelectedValue == "0")
            {
                
                sql = "Select tblbookmaster.Id, tblbooks.BookNo,  tblbookmaster.BookName, tblbookmaster.Author, tblbookmaster.Edition, tblbookcatogory.CatogoryName as Category, DATE_FORMAT( tblbookissue.DateOfIssue, '%d/%m/%Y') as Issuedate   from tblbooks inner join tblbookmaster on tblbookmaster.Id= tblbooks.BookId inner join tblbookissue  on tblbookissue.BookNo= tblbooks.BookNo inner join tblbookusertype on tblbookusertype.Id= tblbookissue.UserType inner join tblbookcatogory on tblbookcatogory.Id= tblbookmaster.CatagoryId";

                if (Drp_Quick.SelectedValue == "1")
                {
                    sql = sql + " inner join tblstudent on tblstudent.Id= tblbookissue.UserId where tblstudent.id=" + UserId;

                }
                else if (Drp_Quick.SelectedValue == "2")
                {

                    sql = sql + " inner join tbluser  on tbluser.Id= tblbookissue.UserId where tbluser.id=" + UserId;
                }
                MydataSet = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    _NewDetails = BuildDataset(validDataset, MydataSet);
                    _NewDetails = LoadFine(_NewDetails);
                }
                
                sql = "Select tblbookmaster.Id, tblbooks.BookNo,  tblbookmaster.BookName, tblbookmaster.Author, tblbookmaster.Edition, tblbookcatogory.CatogoryName as Category, tblbookhistory.TakenDate as Issuedate  , DATE_FORMAT( tblbookhistory.ReturnedDate, '%d/%m/%Y') as ReturnDate, round( tblbookhistory.FineAmount) as Fine from tblbooks inner join tblbookmaster on tblbookmaster.Id= tblbooks.BookId inner join tblbookhistory  on tblbookhistory.BookId= tblbooks.BookNo inner join tblbookusertype on tblbookusertype.Id= tblbookhistory.UserTypeId inner join tblbookcatogory on tblbookcatogory.Id= tblbookmaster.CatagoryId";
                if (Drp_Quick.SelectedValue == "1")
                {
                    sql = sql + " inner join tblstudent on tblstudent.Id= tblbookhistory.UserId where tblstudent.id=" + UserId;

                }
                else if (Drp_Quick.SelectedValue == "2")
                {

                    sql = sql + " inner join tbluser  on tbluser.Id= tblbookhistory.UserId where tbluser.id=" + UserId;
                }
                MydataSet = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    _NewDetails = BuildhistoryDataset(_NewDetails, MydataSet);
                }
               


            }
            else if (Rdo_btnUserDetails.SelectedValue == "2")
            {
                sql = "Select tblbookmaster.Id, tblbooks.BookNo,  tblbookmaster.BookName, tblbookmaster.Author, tblbookmaster.Edition, tblbookcatogory.CatogoryName as Category, tblbookhistory.TakenDate as Issuedate  , DATE_FORMAT( tblbookhistory.ReturnedDate, '%d/%m/%Y') as ReturnDate, round( tblbookhistory.FineAmount) as Fine from tblbooks inner join tblbookmaster on tblbookmaster.Id= tblbooks.BookId inner join tblbookhistory  on tblbookhistory.BookId= tblbooks.BookNo inner join tblbookusertype on tblbookusertype.Id= tblbookhistory.UserTypeId inner join tblbookcatogory on tblbookcatogory.Id= tblbookmaster.CatagoryId";
                if (Drp_Quick.SelectedValue == "1")
                {
                    sql = sql + " inner join tblstudent on tblstudent.Id= tblbookhistory.UserId where tblstudent.id=" + UserId;

                }
                else if (Drp_Quick.SelectedValue == "2")
                {

                    sql = sql + " inner join tbluser  on tbluser.Id= tblbookhistory.UserId where tbluser.id=" + UserId;
                }
                MydataSet = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    _NewDetails = BuildhistoryDataset(validDataset, MydataSet);
                }
                Lbl_TottalFine.Visible = false;
                Lbl_Tottal.Visible = false;
            }
            
                    
            return _NewDetails;
        }

        private DataSet BuildDataset(DataSet Datacontainer, DataSet MydataSet)
        {
            DataTable dt;
            DataRow dr;
           // Datacontainer.Tables.Add(new DataTable("books"));
            foreach (DataRow dr_values in MydataSet.Tables[0].Rows)
            {

                dr = Datacontainer.Tables["books"].NewRow();


                dr["Id"] = dr_values["Id"];
                dr["BookNo"] = dr_values["BookNo"];
                dr["BookName"] = dr_values["BookName"];
                dr["Author"] = dr_values["Author"];
                dr["Edition"] = dr_values["Edition"];
                dr["Category"] = dr_values["Category"];
                dr["Issuedate"] = dr_values["Issuedate"];
                dr["ReturnDate"] = " Not returned";
                dr["Fine"] = "";
                Datacontainer.Tables["books"].Rows.Add(dr);

            }
            return Datacontainer;
        }



        protected DataSet BuildhistoryDataset(DataSet Datacontainer, DataSet MydataSet)
        {
            DataSet _bookdataset = new DataSet();

            DataRow dr;
            double Fine = 0.0, Tottal = 0.0, Amt = 0.0; ;
            // _bookdataset.Tables.Add(new DataTable("books"));
            foreach (DataRow dr_values in MydataSet.Tables[0].Rows)
            {

                dr = Datacontainer.Tables["books"].NewRow();


                dr["Id"] = dr_values["Id"];
                dr["BookNo"] = dr_values["BookNo"];
                dr["BookName"] = dr_values["BookName"];
                dr["Author"] = dr_values["Author"];
                dr["Edition"] = dr_values["Edition"];
                dr["Category"] = dr_values["Category"];
                dr["Issuedate"] = dr_values["Issuedate"];
                dr["ReturnDate"] = dr_values["ReturnDate"];
                dr["Fine"] = dr_values["Fine"];
                Fine = double.Parse(dr_values["Fine"].ToString());
                Fine = Math.Round(Fine);
                Tottal = Tottal + Fine;
                Datacontainer.Tables["books"].Rows.Add(dr);

            }

           
            Tottal = Math.Round(Tottal);
            Lbl_PaidFine.Text = Tottal.ToString();
           
            return Datacontainer;

        }

        private DataSet LoadFine(DataSet _BookDetails)
        {

            double Fine=0.0;
            double Tottal = 0.0, Amt=0.0;

            foreach (DataRow dr_values in _BookDetails.Tables[0].Rows)
            {
                Fine = MyLibMang.GetFine(dr_values["BookNo"].ToString(),int.Parse(dr_values["UserTypeID"].ToString()));
                Fine = Math.Round(Fine);
                dr_values["Fine"] = Fine.ToString();
                Tottal = Tottal + Fine;
                Tottal = Math.Round(Tottal);
                Lbl_TottalFine.Text = Tottal.ToString();
                
            }
            
            return _BookDetails;
        }

        private DataSet NewDataset()
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
            dt.Columns.Add("ReturnDate");
            dt.Columns.Add("Fine");

            return _bookdataset;

        }

        protected void Drp_SearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            Txt_Search.Text = "";

            Txt_Search_AutoCompleteExtender.ContextKey = Drp_Quick.SelectedValue.ToString()+"\\1"; 
            //Tabs.ActiveTabIndex = 1;
        }

        protected void Btn_Export_Click(object sender, EventArgs e)
        {
            if (ViewState["UserId"] != null)
            {
                int Id = int.Parse(ViewState["UserId"].ToString());
                DataSet Dt = FillGrid(Id);

             
                if (Dt.Tables[0].Rows.Count > 0)
                {
                    Dt.Tables[0].Columns.Remove("Id");
                    string FileName = "UserWiseReport";
                    string _ReportName = "UserWiseReport";
                    if (!WinEr.ExcelUtility.ExportDataToExcel(Dt, _ReportName, FileName, MyUser.ExcelHeader))
                    {
                        Lbl_Err.Text = "Please try again.";
                        //this.MPE_MessageBox.Show();
                    }
                }
            }
        }

     
    }
}
