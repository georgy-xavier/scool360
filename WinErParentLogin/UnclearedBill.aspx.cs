﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Configuration;
using WinBase;
using System.Data;

namespace WinErParentLogin
{
    public partial class UnclearedBill : System.Web.UI.Page
    {
        private ParentInfoClass MyParentInfo;
        private OdbcDataReader MyReader = null;
        private FeeManage MyFeeMang;
        private DataSet MydataSet = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            int thrdId = 0;
            if (Session["MyParentObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyParentInfo = (ParentInfoClass)Session["MyParentObj"];
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            if (MyParentInfo == null)
            {
                Response.Redirect("sectionerr.htm");
            }

            else if (!IsPostBack)
            {
                string BillNo = Request.QueryString["BillNo"].ToString();
                string BillType = Request.QueryString["BillType"].ToString();
                Lbl_DDNo.Visible = false;
                Lbl_BankName.Visible = false;
                Lbl_DDNoData.Visible = false;
                Lbl_BankNameData.Visible = false;
                Checkpaymentmode(BillNo, BillType);
                LoadLogo();
                LoadStudent(BillNo, BillType);
                LoadFeeDetails(BillNo, BillType);
                LoadTotal(BillNo, BillType);
                Lbl_StaffName.Text = MyFeeMang.LoadFeeCollectedStaffName(BillNo, BillType);
                LoadSchoolDetails();
            }

        }

        private void Checkpaymentmode(string BillNo, string BillType)
        {
            string Paymode = "";
            MysqlClass _MysqlDb = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_MysqlDb, MyParentInfo.SchoolObject);

            OdbcConnection _MyODBCConn = new OdbcConnection(_MysqlDb.ConnectionStr);

            _MyODBCConn.Open();

            MyFeeMang = new FeeManage(_MysqlDb, _MyODBCConn);
            string sql = "SELECT PaymentMode FROM tblfeebillclearence WHERE BillNo='" + BillNo + "'";

            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Paymode = MyReader.GetValue(0).ToString();
            }
            if (Paymode != "Cash")
            {
                Lbl_DDNo.Visible = true;
                Lbl_BankName.Visible = true;
                Lbl_DDNoData.Visible = true;
                Lbl_BankNameData.Visible = true;

                sql = "SELECT PaymentModeId,BankName FROM tblfeebillclearence WHERE BillNo='" + BillNo + "'";
                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Lbl_DDNoData.Text = MyReader.GetValue(0).ToString();
                    Lbl_BankNameData.Text = MyReader.GetValue(1).ToString();
                }
            }
        }

        private void LoadTotal(string BillNo, string BillType)
        {
            string sql = "";
            MysqlClass _MysqlDb = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_MysqlDb, MyParentInfo.SchoolObject);

            OdbcConnection _MyODBCConn = new OdbcConnection(_MysqlDb.ConnectionStr);

            _MyODBCConn.Open();

            MyFeeMang = new FeeManage(_MysqlDb, _MyODBCConn);
            sql = "select tbltransactionclearence.Amount as Total ,date_format(tbltransactionclearence.PaidDate,'%d-%m-%Y') AS 'PaidDate' from tbltransactionclearence where tbltransactionclearence.BillNo='" + BillNo + "'";

            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                LblAmt.Text = MyReader.GetValue(0).ToString();
                lblpDate.Text = MyReader.GetValue(1).ToString();
            }
        }

        private void LoadFeeDetails(string BillNo, string BillType)
        {
            MysqlClass _MysqlDb = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_MysqlDb, MyParentInfo.SchoolObject);

            OdbcConnection _MyODBCConn = new OdbcConnection(_MysqlDb.ConnectionStr);

            _MyODBCConn.Open();

            MyFeeMang = new FeeManage(_MysqlDb, _MyODBCConn);
            string table = "";
            DataSet data_Fee;
            string _Period = "";
            string _FeeName = "";
            double _total = 0;
            string _Batch = "";
            string sql = "";

            // sql = "SELECT  (select tblperiod.Period from tblperiod where tblperiod.Id= tbltransactionclearence.PeriodId) as Period,tblfeeaccount.AccountName AS 'Fee Name',tblaccount.AccountName,tblview_transaction_entire.Amount,tblbatch.BatchName from tblview_transaction_entire inner join tblaccount on tblaccount.Id= tblview_transaction_entire.AccountTo  inner join tblfeeaccount on tblfeeaccount.Id= tblview_transaction_entire.FeeId inner join tblbatch on tblbatch.Id = tblview_transaction_entire.BatchId where  tblview_transaction_entire.BillNo='" + BillNo + "' Order by tblbatch.BatchName ,tblfeeaccount.AccountName ASC ,tblaccount.AccountName DESC";

            sql = "SELECT  tbltransactionclearence.PeriodName as Period,tbltransactionclearence.FeeName AS 'Fee Name',tblaccount.AccountName,tbltransactionclearence.Amount,tblbatch.BatchName from tbltransactionclearence inner join tblaccount on tblaccount.Id= tbltransactionclearence.AccountTo inner join tblbatch on tblbatch.Id = tbltransactionclearence.BatchId inner join tblperiod on tblperiod.Id= tbltransactionclearence.PeriodId  where  tbltransactionclearence.BillNo='" + BillNo + "' Order by tblbatch.BatchName ASC ,tbltransactionclearence.FeeId,tblperiod.`Order` ASC, tblaccount.AccountName DESC";
            data_Fee = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            table = "<table style=\"margin: inherit; padding: inherit; color: black; width: 100%; border: thin solid #000000;\"  border=\"2\"> <th align=\"left\">Batch</th> <th align=\"left\">Period</th> <th align=\"left\">Fee Name</th>  <th align=\"left\">Amount</th>";//<th align=\"left\">Type</th>
            if (data_Fee != null && data_Fee.Tables != null && data_Fee.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in data_Fee.Tables[0].Rows)
                {

                    if (_Period == "" && _FeeName == "" && _Batch == "")
                    {
                        _Period = dr[0].ToString();
                        _FeeName = dr[1].ToString();
                        _Batch = dr[4].ToString();
                        table = table + "<tr><td class=\"tdStyle\"> &nbsp;" + _Batch + "&nbsp; </td><td class=\"tdStyle\"> &nbsp;" + _Period + "&nbsp; </td><td class=\"tdStyle\">" + _FeeName + "-" + dr[2].ToString() + "</td><td class=\"tdStyle\">" + dr[3].ToString() + "</td></tr>";//<td class=\"tdStyle\">" + dr[2].ToString() + "</td>
                        if ("Discount" != dr[2].ToString())
                        {
                            _total = double.Parse(dr[3].ToString());
                        }
                    }
                    else if (_Period == dr[0].ToString() && _FeeName == dr[1].ToString() && _Batch == dr[4].ToString())
                    {
                        table = table + "<tr><td class=\"tdStyle\"> &nbsp;&nbsp;&nbsp;&nbsp; </td><td class=\"tdStyle\"> &nbsp;&nbsp;&nbsp;&nbsp; </td><td class=\"tdStyle\">" + dr[1].ToString() + "-" + dr[2].ToString() + "</td><td class=\"tdStyle\">" + dr[3].ToString() + "</td></tr>";//<td class=\"tdStyle\">" + dr[2].ToString() + "</td>
                        if ("Discount" != dr[2].ToString())
                        {
                            _total = _total + double.Parse(dr[3].ToString());
                        }
                    }
                    else
                    {
                        _Period = dr[0].ToString();
                        _FeeName = dr[1].ToString();
                        _Batch = dr[4].ToString();
                        //table = table + "<tr><td class=\"tdStyle\"> &nbsp;&nbsp;&nbsp;&nbsp; </td><td class=\"tdStyle\"> &nbsp;&nbsp;&nbsp;&nbsp; </td><td class=\"tdStyle\">&nbsp;&nbsp;&nbsp;&nbsp;</td><td style=\"background-color:Silver\" class=\"tdStyle\"> TOTAL </td><td style=\"background-color:Silver\" class=\"tdStyle\">" + _total.ToString() + "</td></tr>";
                        table = table + "<tr> <td class=\"tdStyle\"> &nbsp;" + _Batch + "&nbsp; </td><td class=\"tdStyle\"> &nbsp;" + _Period + "&nbsp; </td><td class=\"tdStyle\">" + _FeeName + "-" + dr[2].ToString() + "</td><td class=\"tdStyle\">" + dr[3].ToString() + "</td></tr>";//<td class=\"tdStyle\">" + dr[2].ToString() + "</td>
                        if ("Discount" != dr[2].ToString())
                        {
                            _total = double.Parse(dr[3].ToString());
                        }
                    }
                }
                //table = table + "<tr><td class=\"tdStyle\"> &nbsp;&nbsp;&nbsp;&nbsp; </td><td class=\"tdStyle\"> &nbsp;&nbsp;&nbsp;&nbsp; </td><td style=\"background-color:Silver\" class=\"tdStyle\"> TOTAL </td><td style=\"background-color:Silver\" class=\"tdStyle\">" + _total.ToString() + "</td></tr>";//<td class=\"tdStyle\">&nbsp;&nbsp;&nbsp;&nbsp;</td>
            }


            table = table + "</table>";
            this.FeeDetails.InnerHtml = table;
        }


        private void LoadStudent(string BillNo, string BillType)
        {
            MysqlClass _MysqlDb = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_MysqlDb, MyParentInfo.SchoolObject);

            OdbcConnection _MyODBCConn = new OdbcConnection(_MysqlDb.ConnectionStr);

            _MyODBCConn.Open();
          
            MyFeeMang = new FeeManage(_MysqlDb, _MyODBCConn);
            string StudentName = "", AdmissionNo = "", RollNo = "", ClassName = "";
            MyFeeMang.GetStudentDetails(BillNo, out StudentName, out AdmissionNo, out RollNo, out ClassName);
            LblAdminno.Text = AdmissionNo;
            LblCls.Text = ClassName;
            LblName.Text = StudentName;
            lblRno.Text = RollNo;
            Lbl_BillId.Text = BillNo;

        }

        private void LoadSchoolDetails()
        {
            MysqlClass _MysqlDb = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_MysqlDb, MyParentInfo.SchoolObject);

            OdbcConnection _MyODBCConn = new OdbcConnection(_MysqlDb.ConnectionStr);

            _MyODBCConn.Open();

            MyFeeMang = new FeeManage(_MysqlDb, _MyODBCConn);

            string Sql = "SELECT SchoolName,Address FROM tblschooldetails WHERE Id=1";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                Lbl_schoolname.Text = MyReader.GetValue(0).ToString();
                Lbi_subHead.Text = MyReader.GetValue(1).ToString();
            }
        }

        private void LoadLogo()
        {            
            Img_logo.ImageUrl = "Handler/ImageReturnHandler.ashx?id=1&type=Logo";

        }
    }
}
