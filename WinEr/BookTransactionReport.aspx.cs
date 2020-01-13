using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using WinBase;

namespace WinEr
{
    public partial class BookTransactionReport : System.Web.UI.Page
    {
        private LibraryManagClass MyLibMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet = null;

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
            if (!MyUser.HaveActionRignt(847))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    DateTime _date = System.DateTime.Today;
                    string _sdate = MyUser.GerFormatedDatVal(_date);
                    Txt_FromDate.Text = _sdate;
                    Txt_ToDate.Text = _sdate;
                    Txt_FromDate.Enabled = false;
                    Txt_ToDate.Enabled = false;
                    LoadCategoryToDropDown();
                    Pnl_ShowReport.Visible = false;
                }
            }
        }

        protected void Btn_Search_Click(object sender, EventArgs e)
        {

            if (Txt_FromDate.Text == "" || Txt_ToDate.Text == "")
            {
                WC_MessageBox.ShowMssage("Enter from date and to date");
            }
            else
            {
                DateTime Fromdate = General.GetDateTimeFromText(Txt_FromDate.Text);
                DateTime Todate = General.GetDateTimeFromText(Txt_ToDate.Text);
                if (Fromdate > Todate)
                {
                    WC_MessageBox.ShowMssage("Start date must be less than end date");
                }
                else
                {
                    GenerateBookTransactionReport();
                }
            }
        }
  
        protected void Drp_Date_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _sdate = null, _edate = null;
            if (int.Parse(Drp_Date.SelectedValue)==0)
            {
                Txt_FromDate.Text = General.GerFormatedDatVal(System.DateTime.Now);
                Txt_ToDate.Text = General.GerFormatedDatVal(System.DateTime.Now);
                Txt_FromDate.Enabled = false;
                Txt_ToDate.Enabled = false;
            }
            else if (int.Parse(Drp_Date.SelectedValue) == 1)
            {
                DateTime _date = System.DateTime.Now;
                //DateTime _Endtime = _date.AddDays(-1);
                _edate = MyUser.GerFormatedDatVal(_date);

                DateTime _start = System.DateTime.Now;
                _sdate = MyUser.GerFormatedDatVal(new DateTime(_start.Year, _start.Month, 1));
                Txt_FromDate.Enabled = false;
                Txt_ToDate.Enabled = false;
                Txt_FromDate.Text = _sdate;
                Txt_ToDate.Text = _edate;

            }
            else if (int.Parse(Drp_Date.SelectedValue) == 2)
            {
                Txt_FromDate.Enabled = true;
                Txt_ToDate.Enabled = true;
                Txt_FromDate.Text = "";
                Txt_ToDate.Text = "";

            }
        }

        protected void img_export_Excel_Click(object sender, ImageClickEventArgs e)
        {
            DataSet MyData = (DataSet)ViewState["BookTransactionReport"];
            MyData.Tables[0].Columns.Remove("UserId");
            MyData.Tables[0].Columns.Remove("UserTypeId");
            string Name = "BookTransactionReport.xls";
            //if (!ExcelUtility.ExportDataSetToExcel(MyData, Name))
            //{

            //}
            string FileName = "BookTransactionReport";
            string _ReportName = "BookTransactionReport";
            if (!WinEr.ExcelUtility.ExportDataToExcel(MyData, _ReportName, FileName, MyUser.ExcelHeader))
            {

            }
        }


        #region Methods

        private void GenerateBookTransactionReport()
        {
            
            DateTime _Fromdate = MyUser.GetDareFromText(Txt_FromDate.Text);
            DateTime _Todate = MyUser.GetDareFromText(Txt_ToDate.Text);
            int _CatId = int.Parse(Drp_Category.SelectedValue.ToString());
            DataSet BookTransactionReport_Ds = new DataSet();
            BookTransactionReport_Ds = MyLibMang.GetReport(_Fromdate, _Todate, _CatId);
            if (BookTransactionReport_Ds != null && BookTransactionReport_Ds.Tables[0].Rows.Count > 0)
            {
                Grd_Report.Columns[1].Visible = true;
                Grd_Report.Columns[2].Visible = true;
                Grd_Report.DataSource = BookTransactionReport_Ds;
                Grd_Report.DataBind();
                Grd_Report.Columns[1].Visible = false;
                Grd_Report.Columns[2].Visible = false;
                Pnl_ShowReport.Visible = true;
            }
            else
            {
                WC_MessageBox.ShowMssage("No report found in this period");
                Pnl_ShowReport.Visible = false;
                Grd_Report.DataSource = null;
                Grd_Report.DataBind();
            }
            ViewState["BookTransactionReport"] = BookTransactionReport_Ds;
        }

        private void LoadCategoryToDropDown()
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

        #endregion

       
       
    }
}
