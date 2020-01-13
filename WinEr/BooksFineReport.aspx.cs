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
    public partial class BooksFineReport : System.Web.UI.Page
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
            if (!MyUser.HaveActionRignt(776))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    Txt_StartDate.Enabled = false;
                    Txt_EndDate.Enabled = false;
                    Pnl_Content.Visible = false;
                    RdBtnType.SelectedValue = "0";
                    LoadDate(0);
                }
            }

        }

        private void LoadDate(int Id)
        {
            if (Id == 0||Id == 4)
            {
                Txt_StartDate.Text =General.GerFormatedDatVal( System.DateTime.Now);
                Txt_EndDate.Text = General.GerFormatedDatVal(System.DateTime.Now); 
                Txt_StartDate.Enabled = false;
                Txt_EndDate.Enabled = false;
            }
            else if (Id == 1)
            {
                
                
                Txt_StartDate.Text = General.GerFormatedDatVal(System.DateTime.Now.Date.AddDays(-7));
                Txt_EndDate.Text =General.GerFormatedDatVal(System.DateTime.Now); 
                Txt_StartDate.Enabled = false;
                Txt_EndDate.Enabled = false;
            }
            else if (Id == 2)
            {
                
                DateTime _week = System.DateTime.Now.Date.AddMonths(-1);
                Txt_StartDate.Text = General.GerFormatedDatVal(_week);
                Txt_EndDate.Text = General.GerFormatedDatVal(System.DateTime.Now); 
                Txt_StartDate.Enabled = false;
                Txt_EndDate.Enabled = false;
            }
            else if (Id == 3)
            {
                Txt_StartDate.Text = "";
                Txt_EndDate.Text = "";
                Txt_StartDate.Enabled = true;
                Txt_EndDate.Enabled = true;
            }
           
        }

        protected void Btn_Show_Click(object sender, EventArgs e)
        {
            DataSet _ds = Showvalues();
            if (_ds.Tables[0].Rows.Count > 0)
            {
                GrdBooks.DataSource = _ds;
                GrdBooks.DataBind();
                Pnl_Content.Visible = true;
            }
            else
            {
                GrdBooks.DataSource = null;
                GrdBooks.DataBind();
                Pnl_Content.Visible = false;
                Lbl_Err.Text = "No details found";
            }
           
        }
        protected void GrdBooks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            DataSet _ds = Showvalues();
            if (_ds.Tables[0].Rows.Count > 0)
            {
                GrdBooks.DataSource = _ds;
                GrdBooks.DataBind();
                Pnl_Content.Visible = true;
            }
            else
            {
                GrdBooks.DataSource = null;
                GrdBooks.DataBind();
                Pnl_Content.Visible = false;
                Lbl_Err.Text = "No details found";
            }
            GrdBooks.PageIndex = e.NewPageIndex;
        }


        private DataSet Showvalues()
        {
            Lbl_Err.Text = "";
            string sql = "";
            DateTime Start;
            DateTime EndDate;
            MydataSet = null;

            if (int.Parse(RdBtnType.SelectedValue.ToString()) != 4)
            {
                if (Txt_StartDate.Text != "" && Txt_EndDate.Text != "")
                {
                    Start = General.GetDateTimeFromText(Txt_StartDate.Text.ToString());
                    EndDate = General.GetDateTimeFromText(Txt_EndDate.Text.ToString());
                    if(Start<=EndDate)
                        sql = "select tblbookmaster.Id,tblbooks.BookNo as BookNo, tblbookmaster.BookName, tblbookmaster.Author, tblbookmaster.Edition,tblbookhistory.TakenDate  Issuedate , DATE_FORMAT( tblbookhistory.ReturnedDate, '%d/%m/%Y') as ReturnDate,  tblbookhistory.UserTypeId, tblbookhistory.UserId as UserId, tblbookhistory.FineAmount as Fine from tblbookmaster inner join tblbooks  on tblbooks.BookId= tblbookmaster.Id inner join tblbookhistory on tblbookhistory.BookId= tblbooks.BookNo  where tblbookhistory.FineAmount<>0 AND tblbookhistory.ReturnedDate BETWEEN '" + Start.ToString("s") + "' and '" + EndDate.ToString("s") + "' ";
                    else
                        WC_MessageBox.ShowMssage("Start date should be less than or equal to end date");

                }
                else
                {
                    WC_MessageBox.ShowMssage("You must enter a start date & end date");
                }
            }
            else
            {
                sql = "select tblbookmaster.Id, tblbooks.BookNo as BookNo,  tblbookmaster.BookName, tblbookmaster.Author, tblbookmaster.Edition,tblbookhistory.TakenDate  Issuedate , DATE_FORMAT( tblbookhistory.ReturnedDate, '%d/%m/%Y') as ReturnDate,  tblbookhistory.UserTypeId, tblbookhistory.UserId as UserId , tblbookhistory.FineAmount as Fine from tblbookmaster inner join tblbooks  on tblbooks.BookId= tblbookmaster.Id inner join tblbookhistory on tblbookhistory.BookId= tblbooks.BookNo  where tblbookhistory.FineAmount<>0 ";
            }
            if (sql != "")
            {
                MydataSet = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (MydataSet.Tables[0].Rows.Count > 0)
                {

                    DataSet _NewDetails = BuildDataset(MydataSet);
                    DataSet _BookUserDetails = LoadTakenUser(_NewDetails);
                    return _BookUserDetails;
                }
            }
            return MydataSet;
        }
        private DataSet LoadTakenUser(DataSet _BookDetails)
        {

            foreach (DataRow dr_values in _BookDetails.Tables[0].Rows)
            {
                string Type = dr_values["UserType"].ToString();
                string Id = dr_values["UserId"].ToString();
                string Name = MyLibMang.GetUserName(Type, Id);
                dr_values["Username"] = Name.ToString();
            }
            return _BookDetails;
        }
        protected DataSet BuildDataset(DataSet MydataSet)
        {
            DataSet _bookdataset = new DataSet();
            DataTable dt;
            DataRow dr;
            int typeid = 0;
            double  Fine=0.0,TFine=0.0;
            _bookdataset.Tables.Add(new DataTable("books"));
            dt = _bookdataset.Tables["books"];
            dt.Columns.Add("Id");
            dt.Columns.Add("BookNo");
            dt.Columns.Add("BookName");
            dt.Columns.Add("Author");
            dt.Columns.Add("Edition");
            dt.Columns.Add("Issuedate");
            dt.Columns.Add("ReturnDate");
            dt.Columns.Add("UserType");
            dt.Columns.Add("UserId");
            dt.Columns.Add("Username");
            dt.Columns.Add("Fine");

            foreach (DataRow dr_values in MydataSet.Tables[0].Rows)
            {

                dr = _bookdataset.Tables["books"].NewRow();

                dr["Id"]        = dr_values["Id"];
                dr["BookNo"]    = dr_values["BookNo"];
                dr["BookName"]  = dr_values["BookName"];
                dr["Author"]    = dr_values["Author"];
                dr["Edition"]   = dr_values["Edition"];
                dr["Issuedate"] = dr_values["Issuedate"];
                dr["ReturnDate"]= dr_values["ReturnDate"];

                int.TryParse(dr_values["UserTypeId"].ToString(), out typeid);
                if(typeid==1)
                    dr["UserType"]  = "Student";
                else if(typeid==2)
                    dr["UserType"] = "Staff";
                dr["UserId"]    = dr_values["UserId"];
                dr["Username"]  = "";
                TFine = double.Parse(dr_values["Fine"].ToString());
                TFine = Math.Round(TFine);
                dr["Fine"] = TFine.ToString();
                Fine = Fine + TFine;

                _bookdataset.Tables["books"].Rows.Add(dr);
            }
            Lbl_TotalFine.Text = Fine.ToString();
            return _bookdataset;
        }
        protected void RdBtnType_SelectedIndexChanged(object sender, EventArgs e)
        {
             LoadDate(int.Parse(RdBtnType.SelectedValue.ToString()));
           
        }

        protected void Btn_Export_Click(object sender, ImageClickEventArgs e)
        {
            DataSet _ds = Showvalues();
            if (_ds.Tables[0].Rows.Count > 0)
            {
                _ds.Tables[0].Columns.Remove("Id");
                _ds.Tables[0].Columns.Remove("UserId");
             
                
                string FileName = "FineCollectionReport";
                string _ReportName = "Fine Collection Report";
                if (!WinEr.ExcelUtility.ExportDataToExcel(_ds, _ReportName, FileName, MyUser.ExcelHeader))
                {
                    Lbl_Err.Text =  "Please try again.";
                }               
            }
        }
    }
}
