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
using WinBase;
using System.Data.Odbc;

namespace WinEr
{
    public partial class MonthwiseFeeReport : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        private string[] months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");

            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyFeeMang = MyUser.GetFeeObj();
            if (MyFeeMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(881))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {

                    LoadCollectedYear();


                }
            }
        }


        #region Amount Collected

        private void LoadCollectedYear()
        {

            Drp_Year.Items.Clear();
            DataSet myDataset;
            string sql = "SELECT DISTINCT year(tblview_transaction.PaidDate) as Yearname FROM tblview_transaction ORDER BY Yearname DESC";
            myDataset = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (myDataset != null && myDataset.Tables != null && myDataset.Tables[0].Rows.Count > 0)
            {
                int _count = 0;
                ListItem li = new ListItem("All Year", "-1");
                Drp_Year.Items.Add(li);
                foreach (DataRow dr in myDataset.Tables[0].Rows)
                {
                    li = new ListItem(dr[0].ToString(), _count.ToString());
                    Drp_Year.Items.Add(li);
                    _count++;
                }
            }
            else
            {
                ListItem li = new ListItem("No Data Found", "-1");
                Drp_Year.Items.Add(li);
            }
            Drp_Year.SelectedIndex = 0;
        }





        protected void Btn_getReport_Click(object sender, EventArgs e)
        {
            LoadAmmount();
        }

        protected void Btn_Export_Click(object sender, EventArgs e)
        {

            DataSet _MonthlyDataSet = (DataSet)ViewState["FeeRpt"];
            if (_MonthlyDataSet != null && _MonthlyDataSet.Tables[0].Rows.Count > 0)
            {
                _MonthlyDataSet.Tables[0].Columns.Remove(_MonthlyDataSet.Tables[0].Columns[0]);
                if (!WinEr.ExcelUtility.ExportDataToExcel(_MonthlyDataSet, "Monthly Collection Report", "MonthlyCollectionReport", MyUser.ExcelHeader))
                {
                    WC_MessageBox.ShowMssage("MS Excel is missing. Please install");
                }
            }
            else
            {
                WC_MessageBox.ShowMssage("No data found to export");
            }
        }



        private void LoadAmmount()
        {
            Lbl_msg.Text = "";
            string _msg = "";
            if (ReportPossible(out _msg))
            {
                DataSet _MonthlyDataSet = GetGridDataSet();
                if (_MonthlyDataSet != null && _MonthlyDataSet.Tables[0].Rows.Count > 0)
                {
                    Grd_Monthly.Columns[0].Visible = true;
                    Grd_Monthly.DataSource = _MonthlyDataSet;
                    Grd_Monthly.DataBind();
                    Grd_Monthly.Columns[0].Visible = false;
                    ViewState["FeeRpt"] = _MonthlyDataSet;
                }
                else
                {
                    Lbl_msg.Text = "No data found";
                    Grd_Monthly.DataSource = null;
                    Grd_Monthly.DataBind();
                }
            }
            else
            {
                Grd_Monthly.DataSource = null;
                Grd_Monthly.DataBind();
                Lbl_msg.Text = _msg;
            }
        }

        private DataSet GetGridDataSet()
        {
            DataSet _dataset = new DataSet();
            DataTable dt;
            DataRow dr;
            int i;
            int TotalBillCount = 0;
            double TotalScheduledFees = 0;
            double TotalFine = 0;
            double TotalOtherFees = 0;
            double TotalAdvance = 0;
            double GrantTotal = 0;
            _dataset.Tables.Add(new DataTable("monthly"));
            dt = _dataset.Tables["monthly"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Month");
            dt.Columns.Add("BillCount");
            dt.Columns.Add("Scheduled Fees");
            dt.Columns.Add("Fine");
            dt.Columns.Add("Other Fees");
            dt.Columns.Add("Advance");
            dt.Columns.Add("Total Collected");

            for (i = 0; i < months.Length; i++)
            {
                int _month = i + 1;
                int year=int.Parse(Drp_Year.SelectedItem.Text);
                 int BillCount = GetBillCount(_month, year);
                double ScheduledFees = GetScheduledFeesCollected(_month, year);
                double Fine = GetFineCollected(_month, year);
                double OtherFees = GetOtherFeesCollected(_month, year);
                double Advance = GetAdvanceCollected(_month, year);
                double TotalCollected = ScheduledFees + OtherFees + Advance+ Fine;
                TotalBillCount = TotalBillCount + BillCount;
                TotalScheduledFees = TotalScheduledFees + ScheduledFees;
                TotalFine = TotalFine + Fine;
                TotalOtherFees = TotalOtherFees + OtherFees;
                TotalAdvance = TotalAdvance + Advance;
                GrantTotal = GrantTotal + TotalCollected;
                dr = _dataset.Tables["monthly"].NewRow();
                dr["Id"] = _month;
                dr["Month"] = months[i];
                dr["BillCount"] = BillCount;
                dr["Scheduled Fees"] = ScheduledFees;
                dr["Fine"] = Fine;
                dr["Other Fees"] = OtherFees;
                dr["Advance"] = Advance;
                dr["Total Collected"] = TotalCollected;
                _dataset.Tables["monthly"].Rows.Add(dr);

            }
            dr = _dataset.Tables["monthly"].NewRow();
            dr["Id"] = "0";
            dr["Month"] = "Total";
            dr["BillCount"] = TotalBillCount;
            dr["Scheduled Fees"] = TotalScheduledFees;
            dr["Fine"] = TotalFine;
            dr["Other Fees"] = TotalOtherFees;
            dr["Advance"] = TotalAdvance;
            dr["Total Collected"] = GrantTotal;
            _dataset.Tables["monthly"].Rows.Add(dr);
            return _dataset;
        }

        private double GetAdvanceCollected(int _month, int year)
        {
            double _total = 0;
            string sql = "select  sum(tblview_transaction.Amount) from tblview_transaction where tblview_transaction.CollectionType=3 and MONTH(tblview_transaction.PaidDate)=" + _month + " and YEAR(tblview_transaction.PaidDate)=" + year + " and tblview_transaction.Canceled=0 ";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                double.TryParse(MyReader.GetValue(0).ToString(), out _total);
            }
            return _total;
        }

        private double GetOtherFeesCollected(int _month, int year)
        {
            double _total = 0;
            string sql = "select  sum(tblview_transaction.Amount) from tblview_transaction where tblview_transaction.FeeId=-1 and MONTH(tblview_transaction.PaidDate)=" + _month + " and YEAR(tblview_transaction.PaidDate)=" + year + " and tblview_transaction.Canceled=0 ";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                double.TryParse(MyReader.GetValue(0).ToString(), out _total);
            }
            return _total;
        }

        private double GetFineCollected(int _month, int year)
        {
            double _total = 0;
            string sql = "select  sum(tblview_transaction.Amount) from tblview_transaction where tblview_transaction.AccountTo=4 and MONTH(tblview_transaction.PaidDate)=" + _month + " and YEAR(tblview_transaction.PaidDate)=" + year + " and tblview_transaction.Canceled=0 ";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                double.TryParse(MyReader.GetValue(0).ToString(), out _total);
            }
            return _total;
        }

        private double GetScheduledFeesCollected(int _month, int year)
        {
            double _total = 0;
            string sql = "select  sum(tblview_transaction.Amount) from tblview_transaction where tblview_transaction.AccountTo=1 and tblview_transaction.CollectionType<>3 and tblview_transaction.FeeId<>-1 and MONTH(tblview_transaction.PaidDate)=" + _month + " and YEAR(tblview_transaction.PaidDate)=" + year + " and tblview_transaction.Canceled=0 ";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                double.TryParse(MyReader.GetValue(0).ToString(), out _total);
            }
            return _total;
        }

        private int GetBillCount(int _month, int year)
        {
            int _total = 0;
            string sql = "SELECT COUNT(tblview_feebill.BillNo) FROM tblview_feebill WHERE tblview_feebill.Canceled=0 AND MONTH(tblview_feebill.Date)=" + _month + " AND YEAR(tblview_feebill.Date)=" + year;
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out _total);
            }
            return _total;
        }

        private bool ReportPossible(out string _msg)
        {
            bool _valid = true;
            _msg = "";
            if (Drp_Year.SelectedValue == "-1")
            {
                _msg = "Select Year";
                _valid = false;
            }
            return _valid;
        }






        private string GetGridSqlString(DateTime _from, DateTime _To)
        {

            string _sql = "";
            _sql = "SELECT UPPER(tblview_transaction.StudentName) as `StudentName`, tblclass.ClassName, tblview_transaction.FeeName AS 'Fee Name',tblview_transaction.PeriodName AS 'Period Name', tblaccount.AccountName, tblview_transaction.Amount ,date_format( tblview_transaction.PaidDate , '%d-%m-%Y') AS 'PaidDate', tblview_transaction.BillNo from tblview_transaction inner join tblaccount on tblaccount.Id= tblview_transaction.AccountTo  inner join tblclass on tblclass.Id = tblview_transaction.ClassId where  tblview_transaction.PaidDate >='" + _from.Date.ToString("s") + "' AND tblview_transaction.PaidDate <='" + _To.Date.ToString("s") + "'";


            return _sql;
        }






    




        #endregion

        protected void Grd_Monthly_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _month = 0;
            int.TryParse(Grd_Monthly.SelectedRow.Cells[0].Text.ToString(),out _month);
            if (_month > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "window.open('DailyFeeReport.aspx?Month=" + _month + "&Year=" + Drp_Year.SelectedItem.Text + "', 'Info', 'status=1, width=900, height=400,resizable = 1,scrollbars=1');", true);
            }
        }


       
    }
}
