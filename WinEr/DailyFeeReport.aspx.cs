using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;
using WinBase;

namespace WinEr
{
    public partial class DailyFeeReport : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
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
                    int month = 0;
                    if (Request.QueryString["Month"] != null)
                    {
                        int.TryParse(Request.QueryString["Month"].ToString(), out month);
                    }

                    int year = 0;
                    if (Request.QueryString["Year"] != null)
                    {
                        int.TryParse(Request.QueryString["Year"].ToString(), out year);
                    }

                    if (month > 0 && year > 0)
                    {
                        LoadDailyCollectionReport(month, year);
                    }
                    else
                    {
                        Lbl_msg.Text = "Data miss match. Try again.";
                    }
                }
            }
        }

        private void LoadDailyCollectionReport(int month,int year)
        {
            Lbl_msg.Text = "";
            string _msg = "";
            DataSet _DailyDataSet = GetGridDataSet(month, year);
            if (_DailyDataSet != null && _DailyDataSet.Tables[0].Rows.Count > 0)
            {

                Grd_Daily.DataSource = _DailyDataSet;
                Grd_Daily.DataBind();
                ViewState["FeeRpt1"] = _DailyDataSet;
            }

            else
            {
                Grd_Daily.DataSource = null;
                Grd_Daily.DataBind();
                Lbl_msg.Text = "No Data Found";
            }
        }

        private DataSet GetGridDataSet(int month, int year)
        {
            DateTime _WorkingDate = new DateTime(year, month, 1);
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
            dt.Columns.Add("Day");
            dt.Columns.Add("BillCount");
            dt.Columns.Add("Scheduled Fees");
            dt.Columns.Add("Fine");
            dt.Columns.Add("Other Fees");
            dt.Columns.Add("Advance");
            dt.Columns.Add("Total Collected");

            while (_WorkingDate.Month == month)
            {

                int BillCount = GetBillCount(_WorkingDate);
                double ScheduledFees = GetScheduledFeesCollected(_WorkingDate);
                double Fine = GetFineCollected(_WorkingDate);
                double OtherFees = GetOtherFeesCollected(_WorkingDate);
                double Advance = GetAdvanceCollected(_WorkingDate);
                double TotalCollected = ScheduledFees + OtherFees + Advance + Fine;
                TotalBillCount = TotalBillCount + BillCount;
                TotalScheduledFees = TotalScheduledFees + ScheduledFees;
                TotalFine = TotalFine + Fine;
                TotalOtherFees = TotalOtherFees + OtherFees;
                TotalAdvance = TotalAdvance + Advance;
                GrantTotal = GrantTotal + TotalCollected;
                dr = _dataset.Tables["monthly"].NewRow();
                dr["Day"] = General.GerFormatedDatVal(_WorkingDate);
                dr["BillCount"] = BillCount;
                dr["Scheduled Fees"] = ScheduledFees;
                dr["Fine"] = Fine;
                dr["Other Fees"] = OtherFees;
                dr["Advance"] = Advance;
                dr["Total Collected"] = TotalCollected;
                _dataset.Tables["monthly"].Rows.Add(dr);
                _WorkingDate = _WorkingDate.AddDays(1);
            }
            dr = _dataset.Tables["monthly"].NewRow();
            dr["Day"] = "Total";
            dr["BillCount"] = TotalBillCount;
            dr["Scheduled Fees"] = TotalScheduledFees;
            dr["Fine"] = TotalFine;
            dr["Other Fees"] = TotalOtherFees;
            dr["Advance"] = TotalAdvance;
            dr["Total Collected"] = GrantTotal;
            _dataset.Tables["monthly"].Rows.Add(dr);
            return _dataset;
        }

        private int GetBillCount(DateTime _WorkingDate)
        {
            int _total = 0;
            string sql = "SELECT COUNT(tblview_feebill.BillNo) FROM tblview_feebill WHERE tblview_feebill.Canceled=0 AND tblview_feebill.Date='" + _WorkingDate.Date.ToString("s") + "'";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out _total);
            }
            return _total;
        }

        private double GetScheduledFeesCollected(DateTime _WorkingDate)
        {
            double _total = 0;
            string sql = "select  sum(tblview_transaction.Amount) from tblview_transaction where tblview_transaction.AccountTo=1 and tblview_transaction.CollectionType<>3 and tblview_transaction.FeeId<>-1 and tblview_transaction.PaidDate='" + _WorkingDate.Date.ToString("s") + "' and tblview_transaction.Canceled=0 ";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                double.TryParse(MyReader.GetValue(0).ToString(), out _total);
            }
            return _total;
        }

        private double GetFineCollected(DateTime _WorkingDate)
        {
            double _total = 0;
            string sql = "select  sum(tblview_transaction.Amount) from tblview_transaction where tblview_transaction.AccountTo=4 and tblview_transaction.PaidDate='" + _WorkingDate.Date.ToString("s") + "' and tblview_transaction.Canceled=0 ";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                double.TryParse(MyReader.GetValue(0).ToString(), out _total);
            }
            return _total;
        }

        private double GetOtherFeesCollected(DateTime _WorkingDate)
        {
            double _total = 0;
            string sql = "select  sum(tblview_transaction.Amount) from tblview_transaction where tblview_transaction.FeeId=-1 and tblview_transaction.PaidDate='" + _WorkingDate.Date.ToString("s") + "' and tblview_transaction.Canceled=0 ";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                double.TryParse(MyReader.GetValue(0).ToString(), out _total);
            }
            return _total;
        }

        private double GetAdvanceCollected(DateTime _WorkingDate)
        {
            double _total = 0;
            string sql = "select  sum(tblview_transaction.Amount) from tblview_transaction where tblview_transaction.CollectionType=3 and tblview_transaction.PaidDate='" + _WorkingDate.Date.ToString("s") + "'  and tblview_transaction.Canceled=0 ";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                double.TryParse(MyReader.GetValue(0).ToString(), out _total);
            }
            return _total;
        }

        protected void Btn_Excel_Click(object sender, EventArgs e)
        {
            Lbl_msg.Text = "";
            DataSet _MonthlyDataSet = (DataSet)ViewState["FeeRpt1"];
            if (_MonthlyDataSet != null && _MonthlyDataSet.Tables[0].Rows.Count > 0)
            {
                if (!WinEr.ExcelUtility.ExportDataToExcel(_MonthlyDataSet, "Daily Collection Report", "DailyCollectionReport", MyUser.ExcelHeader))
                {
                    Lbl_msg.Text ="MS Excel is missing. Please install";
                }
            }
            else
            {
                Lbl_msg.Text ="No data found to export";
            }
        }
    }
}
