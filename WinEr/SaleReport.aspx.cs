using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.Odbc;

namespace WinEr
{
    public partial class SaleReport : System.Web.UI.Page
    {
        private WinBase.Inventory Myinventory;
        private ConfigManager MyConfigMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            MyUser = (KnowinUser)Session["UserObj"];
            MyConfigMang = MyUser.GetConfigObj();
            Myinventory = MyUser.GetInventoryObj();
            int BillNo = 0;
            string _date = Request.QueryString["Date"].ToString();
            string saleto = Request.QueryString["SaleTo"].ToString();
            double totalcost = double.Parse(Request.QueryString["totalcost"].ToString());
            string BillNum = "";

            StringBuilder stbreceipt = new StringBuilder();
            string sql = "";
            sql = "select Max(Id) from tblinv_transaction where tblinv_transaction.Valuetype=2";
            MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                BillNo = int.Parse(MyReader.GetValue(0).ToString());
                BillNum = "BL:1" + BillNo;


            }
            else
            {
                BillNo = 1;
                BillNum = "BL:1" + BillNo;
            }

            DataSet ReportDs = (DataSet)Session["Reportds"];
            string header = Myinventory.GoodReceiptHeader();
            stbreceipt.Append(header);           
            stbreceipt.Append("<table border=\"1px\" width=\"100%\"><tr><td align=\"center\"><b>ITEM SALE REPORT</b></td></tr></table>");           
            stbreceipt.Append("<table border=\"1px\" width=\"100%\">");
            stbreceipt.Append("<tr><td  align=\"left\"><b>Customer Name:" + saleto + "</b></td><td align=\"left\">Bill No:" + BillNum + "</td><td align=\"right\">Date:" + _date + "</td></tr>");
            stbreceipt.Append("<tr><td align=\"left\"><b>Items</b?</td><td align=\"center\"><b>Count</b></td><td align=\"center\"><b>Cost</b></td></tr>");

            foreach (DataRow dr in ReportDs.Tables[0].Rows)
            {
                stbreceipt.Append("<tr><td align=\"left\">" + dr["ItemName"].ToString() + "</td><td align=\"center\">" + dr["Count"].ToString() + "</td><td align=\"center\">" + dr["Cost"].ToString() + "</td></tr>");
            }
            stbreceipt.Append("<tr><td  colspan=\"2\"></td><td align=\"center\"><b>Total Cost:" + totalcost + "</b></td></tr>");
            stbreceipt.Append("</table>");
            Div_Salereport.InnerHtml = stbreceipt.ToString();
            Session["Reportds"] = null; 
        }
    }
}
