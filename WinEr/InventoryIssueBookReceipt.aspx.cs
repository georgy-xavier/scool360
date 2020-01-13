using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.Odbc;
using WinBase;

namespace WinEr
{
    public partial class InventoryIssueBookReceipt : System.Web.UI.Page
    {


        private WinBase.Inventory Myinventory;
        private ConfigManager MyConfigMang;
        private KnowinUser MyUser;
        int studId;
        int classId;
        OdbcDataReader Myreader = null;
        int BillNum = 0;
        string billno = "";
        double totalcost = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            MyUser = (KnowinUser)Session["UserObj"];
            MyConfigMang = MyUser.GetConfigObj();
            Myinventory = MyUser.GetInventoryObj();
            string ClassName = Request.QueryString["className"].ToString();
            int Type = int.Parse(Request.QueryString["Type"].ToString());
            string studName = Request.QueryString["StudName"].ToString();
            studId = int.Parse(Request.QueryString["studid"].ToString());
            classId = int.Parse(Request.QueryString["classId"].ToString());
            totalcost = double.Parse(Request.QueryString["totalcost"].ToString());
            string BillNumber = "";


            StringBuilder stbreceipt = new StringBuilder();
            if (Type == 0)
            {
                DataSet Issuereceipt = (DataSet)Session["IssueReceipt"];
                string sql = "select Max(Id) from tblinv_viewissuebill";
                Myreader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                if (Myreader.HasRows)
                {
                    int.TryParse(Myreader.GetValue(0).ToString(), out BillNum);
                    BillNumber = "BL:1" + BillNum;
                    
                }
                else
                {
                    BillNum = 1;
                    BillNumber = "BL:1" + BillNum;
                }

                billno = "Bill No:" + BillNum + "";
                string header = Myinventory.GoodReceiptHeader();
                stbreceipt.Append(header);
                stbreceipt.Append("<table width=\"100%\" border=\"1px\">");
                stbreceipt.Append("<tr>");
                stbreceipt.Append("<td colspan=\"8\"  align=\"center\" style=\"font-weight:bold\">ITEM ISSUE REPORT </td>");
                stbreceipt.Append("</tr>");
                stbreceipt.Append("<tr>");
                stbreceipt.Append("<td style=\"font-weight:bold\">Student Name</td>");
                stbreceipt.Append("<td >" + studName + "</td>");
                stbreceipt.Append("<td style=\"font-weight:bold\">Class</td>");
                stbreceipt.Append("<td >" + ClassName + "</td>");
                stbreceipt.Append(" </tr>");
                stbreceipt.Append("<tr>");
                stbreceipt.Append("<td style=\"font-weight:bold\">Date</td>");
                stbreceipt.Append("<td >" + General.GerFormatedDatVal(System.DateTime.Now.Date) + "</td>");
                stbreceipt.Append(" <td style=\"font-weight:bold\">Bill No:</td>  ");
                stbreceipt.Append(" <td>" + BillNumber + "</td>");
                stbreceipt.Append("</tr>");
                stbreceipt.Append("</table>");
                stbreceipt.Append("<table width=\"100%\" border=\"1px\">");
                stbreceipt.Append("<tr>");
                stbreceipt.Append("<td colspan=\"2\" align=\"center\" style=\"font-weight:bold\">Item Name</td>");
                stbreceipt.Append("<td colspan=\"2\" align=\"center\" style=\"font-weight:bold\">Category Name</td>");
                stbreceipt.Append("<td colspan=\"2\" align=\"center\" style=\"font-weight:bold\">Basic Cost</td>");
                stbreceipt.Append("<td colspan=\"2\" align=\"center\" style=\"font-weight:bold\">Total Issued Count</td>");
                stbreceipt.Append("<td colspan=\"2\" align=\"center\" style=\"font-weight:bold\">Cost</td>");
                stbreceipt.Append("</tr>");
                if (Issuereceipt != null && Issuereceipt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in Issuereceipt.Tables[0].Rows)
                    {
                        stbreceipt.Append("<tr>");
                        stbreceipt.Append("<td colspan=\"2\" align=\"center\">" + dr["BookName"].ToString() + "</td>");
                        stbreceipt.Append("<td colspan=\"2\" align=\"center\">" + dr["Category"].ToString() + "</td>");
                        stbreceipt.Append("<td colspan=\"2\" align=\"center\">" + dr["BasicCost"].ToString() + "</td>");
                        stbreceipt.Append("<td colspan=\"2\" align=\"center\">" + dr["IssuedCount"].ToString() + "</td>");
                        stbreceipt.Append("<td colspan=\"2\" align=\"center\">" + dr["Cost"].ToString() + "</td>");
                        stbreceipt.Append("</tr>");
                    }
                    stbreceipt.Append("<tr>");
                    stbreceipt.Append("<td colspan=\"8\" align=\"center\"></td>");
                    stbreceipt.Append("<td align=\"center\" style=\"font-weight:bold\">Total Cost: "+totalcost+"</td>");
                    stbreceipt.Append("</tr>");
                }
                stbreceipt.Append("</table>");
                stbreceipt.Append("<table width=\"100%\" border=\"1px\">");
                stbreceipt.Append(" <tr><td style=\"height:50px;width:700px\" align=\"right\">Signature</td>");
                stbreceipt.Append("<td style=\"height:40px;width:300px\">");
                stbreceipt.Append("</td>");
                stbreceipt.Append("</tr>");
                stbreceipt.Append("</table>");
                IssueReceipt.InnerHtml = stbreceipt.ToString();
                InsertIntoViewIssueBillReport(stbreceipt);
            }
            else if(Type==1)
            {
                DataSet Issuereceipt = (DataSet)Session["Ds"];
                string sql = "select Max(Id) from tblinv_viewissuebill";
                Myreader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                if (Myreader.HasRows)
                {
                    int.TryParse(Myreader.GetValue(0).ToString(), out BillNum);
                    if (BillNum == 0)
                    {
                        BillNum = 1;
                    }
                }

                billno = "Bill No:" + BillNum + "";
                string header = Myinventory.GoodReceiptHeader();
                stbreceipt.Append(header);
                stbreceipt.Append("<table width=\"100%\" border=\"1px\">");
                stbreceipt.Append("<tr>");
                stbreceipt.Append("<td colspan=\"8\"  align=\"center\" style=\"font-weight:bold\">SPECIAL BOOK ISSUE REPORT </td>");
                stbreceipt.Append("</tr>");
                stbreceipt.Append("<tr>");
                stbreceipt.Append("<td style=\"font-weight:bold\">Student Name</td>");
                stbreceipt.Append("<td >" + studName + "</td>");
                stbreceipt.Append("<td style=\"font-weight:bold\">Class</td>");
                stbreceipt.Append("<td >" + ClassName + "</td>");
                stbreceipt.Append(" </tr>");
                stbreceipt.Append("<tr>");
                stbreceipt.Append("<td style=\"font-weight:bold\">Date</td>");
                stbreceipt.Append("<td >" + General.GerFormatedDatVal(System.DateTime.Now.Date) + "</td>");
                stbreceipt.Append(" <td style=\"font-weight:bold\">Bill No:</td>  ");
                stbreceipt.Append(" <td>" + BillNum + "</td>");
                stbreceipt.Append("</tr>");
                stbreceipt.Append("</table>");
                stbreceipt.Append("<table width=\"100%\" border=\"1px\">");
                stbreceipt.Append("<tr>");
                stbreceipt.Append("<td colspan=\"2\" align=\"center\" style=\"font-weight:bold\">Item Name</td>");
                stbreceipt.Append("<td colspan=\"2\" align=\"center\" style=\"font-weight:bold\">Total Issued Count</td>");
                stbreceipt.Append("</tr>");
                if (Issuereceipt != null && Issuereceipt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in Issuereceipt.Tables[0].Rows)
                    {
                        stbreceipt.Append("<tr>");
                        stbreceipt.Append("<td colspan=\"2\" align=\"center\">" + dr["ItemName"].ToString() + "</td>");
                        stbreceipt.Append("<td colspan=\"2\" align=\"center\">" + dr["Count"].ToString() + "</td>");
                        stbreceipt.Append("</tr>");
                    }
                }
                stbreceipt.Append("</table>");
                stbreceipt.Append("<table width=\"100%\" border=\"1px\">");
                stbreceipt.Append(" <tr><td style=\"height:50px;width:700px\" align=\"right\">Signature</td>");
                stbreceipt.Append("<td style=\"height:40px;width:300px\">");
                stbreceipt.Append("</td>");
                stbreceipt.Append("</tr>");
                stbreceipt.Append("</table>");
                IssueReceipt.InnerHtml = stbreceipt.ToString();
                InsertIntoViewIssueBillReport(stbreceipt);
            }


        }

        private void InsertIntoViewIssueBillReport(StringBuilder IssueReceipt)  //Save Receipt to the database for view it in future
        {
            string sql = "";
            sql = "insert into tblinv_viewissuebill(Report,Date,StudId,ClassId) values('" + IssueReceipt + "','" + System.DateTime.Now.Date.ToString("s") + "'," + studId + "," + classId + ")";
            Myinventory.m_MysqlDb.ExecuteQuery(sql);
        }
    }
}
