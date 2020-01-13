using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;

using WinBase;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;
namespace WinErParentLogin
{
    public partial class FeeDetails : System.Web.UI.Page
    {
        private ParentInfoClass MyParentInfo;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet = null;
        //for online payment
        public string action1 = string.Empty;
        public string hash1 = string.Empty;
        public string txnid1 = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MyParentObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyParentInfo = (ParentInfoClass)Session["MyParentObj"];            
            if (MyParentInfo == null)
            {
                Response.Redirect("sectionerr.htm");
            }
           
            if (!IsPostBack)
            {
                Btn_DD.Visible = false;
                LoadConfiguration();     
                LoadPreviousBatchesToDropDown();
                LoadFeeDetailsToGrid();
                LoadfeeDue();
                LoadTotalAmount();
                //sai added
                Load_Group_Headers();
                //
                Label MyHeader = (Label)this.Master.FindControl("Lbl_PageHeader");
                MyHeader.Text = "Fee Details";
                Tabs.ActiveTabIndex = 0;

                MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
                DBLogClass _DblogObj = new DBLogClass(_mysqlObj);

                _DblogObj.LogToDb(MyParentInfo.ParentName, "ParentLogin_Visit Fee Page", " visited Fee details Page ", 4, 2);
                _DblogObj = null;
                _mysqlObj.CloseConnection();
            }
        }
        // check online payment cofigure this school or not
        private bool Is_Online_Payment_Enable()
        {
            bool enable = false;
            string sql = "";
            MyReader = null;
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));

            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='Online_Payment'";
            MyReader = _mysqlObj.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    if (int.Parse(MyReader.GetValue(0).ToString())==1)
                    {
                        enable = true;
                    }
                }
            }
            return enable;
        }
        private void LoadConfiguration()
        {
            int configvalue = 0;
            string sql = "";
            MyReader = null;
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            try
            {

                ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
                sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='DD Submission Needed'";
                MyReader = _mysqlObj.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    int.TryParse(MyReader.GetValue(0).ToString(), out configvalue);
                }
                if (configvalue == 0)
                {
                    Btn_DD.Visible = false;
                }
                else
                {
                    Btn_DD.Visible = true;
                }
                MyReader.Close();

                sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='Bill View Needed'";
                MyReader = _mysqlObj.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Hdn_Billviewneeded.Value = MyReader.GetValue(0).ToString();
                }
                MyReader.Close();
                ReleaseResourse(_mysqlObj, MyParent);

            }
            catch (Exception rt)
            {

            }


        }

        private void LoadTotalAmount()
        {
            double _totalamt = 0, _fine = 0, _amount = 0;
            CheckBox chk = new CheckBox();
            if (Grd_Feetopay.Rows.Count > 0)
            {
                foreach (GridViewRow gr in Grd_Feetopay.Rows)
                {
                    //chk = (CheckBox)gr.FindControl("CheckBoxUpdate");
                    //if (chk.Checked)
                    //{
                        double.TryParse(gr.Cells[5].Text.Replace("&nbsp;", ""), out _amount);
                        double.TryParse(gr.Cells[13].Text.Replace("&nbsp;", ""), out _fine);
                        _totalamt = _totalamt + _amount + _fine;
                        
                    //}
                }
            }
            Txt_Amount.Text = _totalamt.ToString();
        }

        private void ReleaseResourse(MysqlClass _mysqlObj, ParentLogin MyParent)
        {
            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyParent = null;
        }

        private void LoadfeeDue()
        {
           
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            string sql;
           
            //if (chkBoxAll.Checked)
            //{
            //    sql = "select tblfeeaccount.AccountName as `Fee Name`,tblbatch.BatchName, tblperiod.Period, tblfeestudent.Status, tblfeestudent.BalanceAmount,date_format( tblfeeschedule.LastDate , '%d-%m-%Y') AS 'LastDate', tblfeestudent.SchId, tblfeeschedule.PeriodId, tblfeeschedule.FeeId,tblfeestudent.Id as FeeStudentid, tblfeeschedule.Duedate, tblfeeschedule.BatchId from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId where tblfeestudent.StudId=" + MyParentInfo.StudentId + "  and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion'";
            //}
            //else
            //{
            sql = "select tblfeeaccount.AccountName as `Fee Name`,tblbatch.BatchName, tblperiod.Period, tblfeestudent.Status, tblfeestudent.BalanceAmount,date_format( tblfeeschedule.LastDate , '%d-%m-%Y') AS 'LastDate', tblfeestudent.SchId, tblfeeschedule.PeriodId, tblfeeschedule.FeeId,tblfeestudent.Id as FeeStudentid, date_format( tblfeeschedule.Duedate , '%d-%m-%Y') AS 'Duedate', tblfeeschedule.BatchId from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId where tblfeestudent.StudId=" + MyParentInfo.StudentId + "  and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion'";
            //}
            MydataSet = MyParent.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            MydataSet = CalculateFineAmount(MydataSet);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_Feetopay.Columns[7].Visible = true;
                Grd_Feetopay.Columns[3].Visible = true;
                Grd_Feetopay.Columns[8].Visible = true;
                Grd_Feetopay.Columns[9].Visible = true;
                Grd_Feetopay.Columns[10].Visible = true;
                Grd_Feetopay.Columns[11].Visible = true;
                Grd_Feetopay.Columns[12].Visible = true;
               Grd_Feetopay.DataSource = MydataSet;
                Grd_Feetopay.DataBind();
                Lbl_feeMessage.Text = "";
                Btn_feeexport.Enabled = true;
                Grd_Feetopay.Columns[3].Visible = true;
                Grd_Feetopay.Columns[7].Visible = false;
                Grd_Feetopay.Columns[8].Visible = false;
                Grd_Feetopay.Columns[9].Visible = false;
                Grd_Feetopay.Columns[10].Visible = false;
                Grd_Feetopay.Columns[11].Visible = false;
                Grd_Feetopay.Columns[12].Visible = false;
            }
            else
            {
                Grd_Feetopay.DataSource = null;
                Grd_Feetopay.DataBind();
                Lbl_feeMessage.Text = "No Fee to pay";
                Btn_feeexport.Enabled = false;
                Btn_DD.Visible = false;
            }
            Tabs.ActiveTabIndex = 1;
            ReleaseResourse(_mysqlObj, MyParent);
        }

        private DataSet CalculateFineAmount(DataSet FineDs)
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            if (FineDs != null && FineDs.Tables[0].Rows.Count > 0)
            {
                FineDs.Tables[0].Columns.Add("Fine");
                
                foreach (DataRow dr in FineDs.Tables[0].Rows)
                {
                    string sql = "";
                    DateTime tdydate = System.DateTime.Now.Date;
                    DateTime lastdate = new DateTime();
                    double BalAmnt = 0;
                    double.TryParse(dr["BalanceAmount"].ToString(), out BalAmnt);
                    int finetype = 0, finedate = 0, fineduration;
                    double fine = 0, fineforoneday = 0;
                    OdbcDataReader fineamountreader = null;
                    sql = "SELECT tblfine.Amount, tblfine.Finedate, tblfine.FineAmounttype,tblfine.FineDuration from tblfine where tblfine.FeeId=" + dr["FeeId"].ToString() + " and tblfine.Amount>0";
                    fineamountreader = _mysqlObj.ExecuteQuery(sql);
                    if (fineamountreader.HasRows)
                    {
                        int.TryParse(fineamountreader.GetValue(2).ToString(), out finetype);
                        int.TryParse(fineamountreader.GetValue(1).ToString(), out finedate);
                        double.TryParse(fineamountreader.GetValue(0).ToString(), out fine);
                        int.TryParse(fineamountreader.GetValue(3).ToString(), out fineduration);
                        int diff = 0, lastday = 0, today = 0;
                        string strdate = "";
                        if (finedate == 1)
                        {
                            strdate = dr["LastDate"].ToString().Replace("-", "/");

                        }
                        else if (finedate == 2)
                        {
                            strdate = dr["Duedate"].ToString().Replace("-", "/");
                        }

                        lastdate = General.GetDateFromText(strdate);

                        if (tdydate > lastdate)
                        {
                            double daydiff = (tdydate - lastdate).TotalDays;
                            diff = (int)daydiff;
                            lastday = lastdate.Day;
                            today = tdydate.Day;
                            int extradays = 0;
                            double Finetotal = 0;
                            //diff = (today - lastday);
                            if (diff >= 1)
                            {
                                if (finetype == 1)
                                {
                                    if (diff < fineduration)
                                    {
                                        fineforoneday = fine / fineduration;
                                        Finetotal = (diff * fineforoneday);
                                    }
                                    else
                                    {
                                        Finetotal = fine;

                                    }
                                    Finetotal = Math.Round(Finetotal);
                                    dr["Fine"] = Finetotal.ToString();
                                }
                                else if (finetype == 2)
                                {
                                    double amount = 0;
                                    amount = (BalAmnt * fine / 100);
                                    fineforoneday = amount / fineduration;
                                    if (diff < fineduration)
                                    {
                                        Finetotal = (diff * fineforoneday);
                                    }
                                    else
                                    {
                                        Finetotal = amount;

                                    }
                                    Finetotal = Math.Round(Finetotal);
                                    dr["Fine"] = Finetotal.ToString();

                                }
                                else
                                {
                                    if (finetype == 3)
                                    {
                                        fineforoneday = fine / fineduration;
                                    }
                                    else if (finetype == 4)
                                    {
                                        double amount = 0;
                                        amount = (BalAmnt * fine / 100);
                                        fineforoneday = amount / fineduration;
                                    }
                                    if ((diff % fineduration == 0) || (diff < fineduration))
                                    {
                                        Finetotal = (diff * fineforoneday);
                                        Finetotal = Math.Round(Finetotal);
                                        dr["Fine"] = Finetotal.ToString();
                                    }
                                    else if (diff % fineduration > 0)
                                    {
                                        extradays = diff % fineduration;
                                        int days = 0;
                                        days = diff - extradays;
                                        Finetotal = (days * fineforoneday) + (fineforoneday * extradays);
                                        Finetotal = Math.Round(Finetotal);
                                        dr["Fine"] = Finetotal.ToString();

                                    }
                                }


                            }
                        }
                        else
                        {
                            dr["Fine"] = "0";
                        }
                    }
                    else
                    {
                        dr["Fine"] = "0";
                    }
                    fineamountreader.Close();
                }
            }

            return FineDs;
        }

        protected void Grd_Feetopay_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Feetopay.PageIndex = e.NewPageIndex;
            LoadfeeDue();
            Tabs.ActiveTabIndex = 1;
            MPE_DD.Show();
        }

        protected void chkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
           
            LoadfeeDue();
            LoadTotalAmount();
            LoadConfiguration();     
            Tabs.ActiveTabIndex = 1;
        }

        protected void CheckBoxUpdate_CheckedChanged(object sender, EventArgs e)
        {
            LoadTotalAmount();
        }

        private bool QuickBillEnabled()
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            bool _valid = false;
            string QuickBill;
            string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='QuickBill'";
            MyReader = MyParent.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                QuickBill = MyReader.GetValue(0).ToString();
                if (QuickBill == "1")
                {
                    _valid = true;
                }
            }
            ReleaseResourse(_mysqlObj, MyParent);
            return _valid;
        }

        private bool PdfReportEnabled()
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            bool _valid = false;
            string PdfReport;
            string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='PdfReport'";
            MyReader = MyParent.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                PdfReport = MyReader.GetValue(0).ToString();
                if (PdfReport == "1")
                {
                    _valid = true;
                }
            }
            ReleaseResourse(_mysqlObj, MyParent);
            return _valid;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            string sql;
            //if (chkBoxAll.Checked)
            //{
            //    sql = "select tblfeeaccount.AccountName as `Fee Name`,tblbatch.BatchName, tblperiod.Period, tblfeestudent.Status, tblfeestudent.BalanceAmount,date_format( tblfeeschedule.LastDate , '%d-%m-%Y') AS 'LastDate', tblfeestudent.SchId, tblfeeschedule.PeriodId, tblfeeschedule.FeeId,tblfeestudent.Id as FeeStudentid, tblfeeschedule.Duedate, tblfeeschedule.BatchId from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId where tblfeestudent.StudId=" + MyParentInfo.StudentId + "  and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid'";
            //}
            //else
            //{
                sql = "select tblfeeaccount.AccountName as `Fee Name`,tblbatch.BatchName, tblperiod.Period, tblfeestudent.Status, tblfeestudent.BalanceAmount,date_format( tblfeeschedule.LastDate , '%d-%m-%Y') AS 'LastDate', tblfeestudent.SchId, tblfeeschedule.PeriodId, tblfeeschedule.FeeId,tblfeestudent.Id as FeeStudentid, date_format( tblfeeschedule.Duedate , '%d-%m-%Y') AS 'Duedate', tblfeeschedule.BatchId from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId where tblfeestudent.StudId=" + MyParentInfo.StudentId + "  and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid'";
            //}
            MydataSet = MyParent.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            MydataSet = CalculateFineAmount(MydataSet);
            MydataSet.Tables[0].Columns.Remove("SchId");
            MydataSet.Tables[0].Columns.Remove("PeriodId");
            MydataSet.Tables[0].Columns.Remove("FeeId");
            MydataSet.Tables[0].Columns.Remove("FeeStudentid");
            MydataSet.Tables[0].Columns.Remove("Duedate");
            MydataSet.Tables[0].Columns.Remove("BatchId");
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
             
                string FileName = "FeeDueList";

                string _ReportName = "<table><tr><td><b>Fee Due List</b></td></tr><tr><td>Student Name :" + MyParentInfo.StudentName + "</td><td>Date : " + DateTime.Now.ToString() + "</td></tr></table>";

                if (!WinEr.ExcelUtility.ExportDataToExcel(MydataSet, _ReportName, FileName, MyParent.ExcelHeader))
                {
                    MSGBOX.ShowMssage("This function need Ms office");
                }
            }
            Tabs.ActiveTabIndex = 1;
            ReleaseResourse(_mysqlObj, MyParent);
        }       

        #region NEW ALL BATCH FEE DETAILS

        protected void Drp_Batch_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            LoadFeeDetailsToGrid();
            Tabs.ActiveTabIndex = 0;
        }

        protected void Rdb_FeeType_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            LoadFeeDetailsToGrid();
            Tabs.ActiveTabIndex = 0;
        }

        protected void Rdb_BillType_SelectedIndexChanged(object sender, EventArgs e)
        {

            LoadFeeDetailsToGrid();
            Tabs.ActiveTabIndex = 0;
        }

        private void LoadPreviousBatchesToDropDown()
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            Drp_Batch.Items.Clear();
            Drp_Batch.Items.Add(new ListItem("ALL", "0"));
            string sql = "select tblbatch.Id, tblbatch.BatchName from tblbatch where tblbatch.LastbatchId=" + MyParentInfo.CurrentBatchId;
            MyReader = MyParent.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                Drp_Batch.Items.Add(li);
            }
            Drp_Batch.Items.Add(new ListItem(MyParentInfo.CurrentBatchName, MyParentInfo.CurrentBatchId.ToString()));

            sql = "select tblstudentclassmap_history.BatchId, CONCAT( (select tblclass.ClassName from tblclass where tblclass.Id=tblstudentclassmap_history.ClassId), ':', (select tblbatch.BatchName from tblbatch where tblbatch.Id= tblstudentclassmap_history.BatchId)) as ClassName from tblstudentclassmap_history where tblstudentclassmap_history.BatchId<>" + MyParentInfo.CurrentBatchId + " AND tblstudentclassmap_history.StudentId=" + MyParentInfo.StudentId + " order by tblstudentclassmap_history.BatchId desc";
            MyReader = MyParent.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Batch.Items.Add(li);
                }
            }

            for (int i = 0; i < Drp_Batch.Items.Count; i++)
            {
                if (int.Parse(Drp_Batch.Items[i].Value) == MyParentInfo.CurrentBatchId)
                {
                    Drp_Batch.SelectedIndex = i;
                    break;
                }
            }
            ReleaseResourse(_mysqlObj, MyParent);
        }

        private void LoadFeeDetailsToGrid()
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            int configvalue_billviewneeded = 0;
            int.TryParse(Hdn_Billviewneeded.Value, out configvalue_billviewneeded);
            try
            {
                string sql = "";
                //string sql = "select tblview_transaction.FeeName as AccountName, tblview_transaction.PeriodName as Period, tblbatch.BatchName,tblview_transaction.AccountTo as AccountType,   tblview_transaction.Amount, date_format( tblview_transaction.PaidDate , '%d-%m-%Y') AS 'PaidDate' ,tblview_transaction.BillNo, tblfeeaccount.`Type`,tblbatch.Id as BatchId from tblview_transaction inner join tblfeeaccount on tblview_transaction.FeeId= tblfeeaccount.Id inner join tblbatch on tblbatch.Id= tblview_transaction.BatchId WHERE (tblview_transaction.UserId=" + MyParentInfo.StudentId + "  or  tblview_transaction.UserId=(select tbltempstdent.Id from tbltempstdent inner join tblstudent on tbltempstdent.TempId= tblstudent.TempStudentId WHERE tblstudent.Id=" + MyParentInfo.StudentId + "))";
                if (Rdb_BillType.SelectedValue == "1")
                {
                     sql = "select tblview_transaction.FeeName as `Fee Name`, tblview_transaction.PeriodName as Period, tblbatch.BatchName, tblaccount.AccountName as AccountType,   tblview_transaction.Amount, date_format( tblview_transaction.PaidDate , '%d-%m-%Y') AS 'PaidDate' ,tblview_transaction.BillNo, tblfeeaccount.`Type`,tblbatch.Id as BatchId from tblview_transaction inner join tblfeeaccount on tblview_transaction.FeeId= tblfeeaccount.Id inner join tblbatch on tblbatch.Id= tblview_transaction.BatchId inner join tblview_feebill on tblview_feebill.BillNo= tblview_transaction.BillNo inner join tblaccount on tblaccount.Id = tblview_transaction.AccountTo  WHERE tblview_feebill.Canceled=0 AND tblview_feebill.StudentID=" + MyParentInfo.StudentId;
                    if (int.Parse(Drp_Batch.SelectedValue) > 0)
                    {
                        sql = sql + "  and tblview_transaction.BatchId=" + Drp_Batch.SelectedValue;
                    }
                }
                else
                {
                    sql = "select  tbltransactionclearence.FeeName as `Fee Name`, tbltransactionclearence.PeriodName as Period,  tblbatch.BatchName, tblaccount.AccountName as AccountType,  tbltransactionclearence.Amount   , date_format( tbltransactionclearence.PaidDate , '%d-%m-%Y') AS 'PaidDate', tbltransactionclearence.BillNo  , tblfeeaccount.`Type`,tblbatch.Id as BatchId  from tbltransactionclearence      inner join tblfeeaccount on tbltransactionclearence.FeeId= tblfeeaccount.Id inner join tblbatch      on tblbatch.Id= tbltransactionclearence.BatchId inner join tblaccount on tblaccount.Id = tbltransactionclearence.AccountTo WHERE   tbltransactionclearence.UserId=" + MyParentInfo.StudentId + " and tbltransactionclearence.Canceled=0  ";
                     if (int.Parse(Drp_Batch.SelectedValue) > 0)
                    {
                        sql = sql + "  and tbltransactionclearence.BatchId=" + Drp_Batch.SelectedValue;
                    }
                }
                if (Rdb_FeeType.SelectedValue == "1")
                {
                    sql = sql + " and tblfeeaccount.`Type`=1";
                }
                else if (Rdb_FeeType.SelectedValue == "2")
                {
                    sql = sql + " and tblfeeaccount.`Type`=2";
                }
                //sql = sql + " and tblview_transaction.BatchId!=" + MyUser.CurrentBatchId;

                MydataSet = MyParent.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
                {
                    Lbl_TransAllMsg.Text = "";
                    Grd_TransactionsAll.Columns[7].Visible = true;
                    Grd_TransactionsAll.Columns[8].Visible = true;
                    Grd_TransactionsAll.Columns[9].Visible = true;
                    Grd_TransactionsAll.Columns[6].Visible = true;
                    Grd_TransactionsAll.DataSource = MydataSet;
                    Grd_TransactionsAll.DataBind();
                    Grd_TransactionsAll.Columns[7].Visible = false;
                    Grd_TransactionsAll.Columns[8].Visible = false;
                    if (configvalue_billviewneeded == 0 || Rdb_BillType.SelectedValue=="2")
                    {
                        Grd_TransactionsAll.Columns[9].Visible = false;
                        Grd_TransactionsAll.Columns[6].Visible = false;

                    }
                    ViewState["FeeDetails"] = MydataSet;
                    ImgExportAll.Visible = true;
                }
                else
                {
                    Lbl_TransAllMsg.Text = "No Fee Details Exist";
                    Grd_TransactionsAll.DataSource = null;
                    Grd_TransactionsAll.DataBind();
                    ImgExportAll.Visible = false;
                }
            }
            catch (Exception Exc)
            {
                MSGBOX.ShowMssage(Exc.Message);
            }
            Tabs.ActiveTabIndex = 0;
            ReleaseResourse(_mysqlObj, MyParent);
        }

        protected void Grd_TransactionsAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            FeeManage MyFeeMang = new FeeManage(_mysqlObj);
            int _Type = int.Parse(Grd_TransactionsAll.SelectedRow.Cells[7].Text.Trim());
            string _BatchName = Grd_TransactionsAll.SelectedRow.Cells[2].Text.Trim();
            string _BillId = Grd_TransactionsAll.SelectedRow.Cells[6].Text.Trim();
            int _BatchId = int.Parse(Grd_TransactionsAll.SelectedRow.Cells[8].Text.Trim());


            if (_BillId != "")
            {
                if (Rdb_BillType.SelectedValue == "1")
                {
                    int _Value = 1;
                    string _PageName = "FeeBillSmall.aspx";
                    bool Pdf = false;
                    if (MyFeeMang.GetBillType(ref _Value, ref _PageName, out Pdf))
                    {
                        if (Pdf)
                        {
                            string _ErrorMsg = "";
                            Pdf MyPdf = new Pdf(MyFeeMang.m_MysqlDb, MyParentInfo.SchoolObject);
                            _ErrorMsg = "";
                            string _physicalpath = WinerUtlity.GetAbsoluteFilePath(MyParentInfo.SchoolObject, Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
                            string _PdfName = "";
                            if (_Type == 1)
                            {
                                if (MyPdf.CreateFeeReciptPdf(_BillId, _BatchName, MyParentInfo.CurrentBatchId, _physicalpath, out _PdfName, out _ErrorMsg, _Value))
                                {
                                    _ErrorMsg = "";
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                                }
                                else if (_PdfName == "")
                                {
                                    Lbl_TransAllMsg.Text = "Failed To Create";
                                }
                            }
                            else
                            {

                                if (MyPdf.CreateJoiningFeeReciptPdf(_BillId, _BatchName, _physicalpath, out _PdfName, out _ErrorMsg, _Value))
                                {
                                    _ErrorMsg = "";
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                                }
                                else if (_PdfName == "")
                                {
                                    Lbl_TransAllMsg.Text = "Failed To Create";
                                }
                            }
                        }
                        else
                        {
                            string _Bill = _BillId;
                            if (_Type == 1)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"" + _PageName + "?BillNo=" + _Bill + "&BillType=0\");", true);
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"" + _PageName + "?BillNo=" + _Bill + "&BillType=1\");", true);
                            }
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AnyScriptNameYouLike", "window.open(\"UnclearedBill.aspx?BillNo=" + _BillId + "&BillType=0\");", true);
                }
            }

            Tabs.ActiveTabIndex = 0;
            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyFeeMang = null;
        }

        protected void Grd_TransactionsAll_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_TransactionsAll.PageIndex = e.NewPageIndex;
            DataSet _JoinDataSet = (DataSet)ViewState["FeeDetails"];
            Grd_TransactionsAll.Columns[7].Visible = true;
            Grd_TransactionsAll.Columns[8].Visible = true;
            Grd_TransactionsAll.DataSource = _JoinDataSet;
            Grd_TransactionsAll.DataBind();
            Grd_TransactionsAll.Columns[7].Visible = false;
            Grd_TransactionsAll.Columns[8].Visible = false;
            Tabs.ActiveTabIndex = 0;
        }

        protected void ImgExportAll_Click(object sender, ImageClickEventArgs e)
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            DataSet _FeeReport = new DataSet();
            _FeeReport = (DataSet)ViewState["FeeDetails"];
            _FeeReport.Tables[0].Columns.Remove("Type");
            _FeeReport.Tables[0].Columns.Remove("BatchId");
            string FileName = "FeeTransactionReport";
            string _ReportName = "<table><tr><td><b>Fee Transaction Report</b></td></tr><tr><td>Student Name :" + MyParentInfo.StudentName + "</td><td>Date : " + DateTime.Now.ToString() + "</td></tr></table>";

            if (!WinEr.ExcelUtility.ExportDataToExcel(_FeeReport, _ReportName, FileName, MyParent.ExcelHeader))
            {
                MSGBOX.ShowMssage("This function need Ms office");
            }
           // MyParentInfo.StudentId
            Tabs.ActiveTabIndex = 0;
            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            _FeeReport = null;
        }

        #endregion


        //Demand Draft


        protected void Btn_DD_Click(object sender, EventArgs e)
        {
            //Lbl_feeMessage.Text = "";
            //CheckBox chk = new CheckBox();
            //bool chkvalue = false;
            //Btn_Pay.Enabled = true;
            //foreach (GridViewRow gr in Grd_Feetopay.Rows)
            //{
            //    chk = (CheckBox)gr.FindControl("CheckBoxUpdate");
            //    if (chk.Checked)
            //    {
            //        chkvalue = true;
            //    }
            //}
            //if (chkvalue)
            //{
            //    Txt_DDNumber.Text = "";
            //    Txt_BankName.Text = "";
            //    Txt_totalAmount.Text = Txt_Amount.Text;
            //    Lbl_PopErr.Text = "";
            //    MPE_DD.Show();
            //}
            //else
            //{
            //    Lbl_feeMessage.Text = "Select any fee";

            //}
        }



        // sai added Online Payment code code

        #region Online Payment
        private void Load_Group_Headers()
        {
            DataSet Ds_Header = new DataSet();
             MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            string sql;
            sql = "select tbl_feesgrouphead.Id,tbl_feesgrouphead.Name,sum(tblfeestudent.BalanceAmount) as BalanceAmount from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId INNER join tbl_feesgrouphead on tbl_feesgrouphead.Id=tblfeeaccount.Header_Id where tblfeestudent.StudId=" + MyParentInfo.StudentId + "  and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion' and tbl_feesgrouphead.Is_Enable=1";
            Ds_Header = MyParent.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Ds_Header != null && Ds_Header.Tables != null && Ds_Header.Tables[0].Rows.Count > 0)
            {
                Grd_Fees_Header.Columns[0].Visible = true;
                if (!Is_Online_Payment_Enable())
                {
                    Grd_Fees_Header.Columns[6].Visible = true;
                }
                Grd_Fees_Header.DataSource = Ds_Header;
                Grd_Fees_Header.DataBind();
                Grd_Fees_Header.Columns[0].Visible = false;
                if (!Is_Online_Payment_Enable())
                {
                    Grd_Fees_Header.Columns[6].Visible = false;
                }
                Pnl_FeeToPay.Visible = true;
                Load_Fine_Amount();
               
            }
            else
            {
                Grd_Fees_Header.DataSource = null;
                Grd_Fees_Header.DataBind();
                Pnl_FeeToPay.Visible = false;
                Lbl_Header.Text = "No Fee to pay";
            }
        }
        protected void Grd_Fees_Header_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Fees_Header.PageIndex = e.NewPageIndex;
            Load_Group_Headers();
            Tabs.ActiveTabIndex = 1;
            
        }
        protected void Grd_Fees_Header_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "select")
            {
                int id = 0;
                int index = Convert.ToInt32(e.CommandArgument);
                id = int.Parse(Grd_Fees_Header.Rows[index].Cells[0].Text.ToString());
                Session["Fees_Group_Id"] = id;
                Session["Fees_Group_Header"] = Grd_Fees_Header.Rows[index].Cells[1].Text.ToString();
                Txt_Amount.Text = Grd_Fees_Header.Rows[index].Cells[4].Text.ToString();
                Hdn_TotalAmount.Value = Grd_Fees_Header.Rows[index].Cells[4].Text.ToString();
                Load_Fees_Group_Header(id);
                MPE_DD.Show();
            }
            if (e.CommandName == "Payment")
            {
                // check online payment enable for this school or not
                //if (Is_Online_Payment_Enable())
                //{
                    int id = 0;
                    int index = Convert.ToInt32(e.CommandArgument);
                    id = int.Parse(Grd_Fees_Header.Rows[index].Cells[0].Text.ToString());
                    Txt_Id.Text = id.ToString();
                    Txt_Name.Text = Grd_Fees_Header.Rows[index].Cells[1].Text.ToString();
                    Txt_TAmount.Text = Grd_Fees_Header.Rows[index].Cells[4].Text.ToString();
                    Server.Transfer("MakePayment.aspx");
                   
                //}
                //else
                //{
                //    MSGBOX.ShowMssage("Online Payment Not Available For This School");
                //}

            }
        }

        #region Public properties for Transfer data to makepayment page

        public string Fees_Group_Header
        {
            get
            {
                return Txt_Name.Text;
            }
        }
        public string Fees_Group_Id
        {
            get
            {
                return Txt_Id.Text;
            }
        }
        public string Fees_Group_Header_Total
        {
            get
            {
                return Txt_TAmount.Text;
            }
        }

        #endregion

        private void Load_Fine_Amount()
        {
            if (Grd_Fees_Header.Rows.Count > 0)
            {
                foreach (GridViewRow gr in Grd_Fees_Header.Rows)
                {
                        int Header_Id = 0;
                        float Amount = 0;
                        float Fine = 0;
                        bool _in=false;
                        int.TryParse(gr.Cells[0].Text, out Header_Id);
                        if (Header_Id != 0)
                        {
                            gr.Cells[3].Text = Calculate_Fine_Amount(Header_Id).ToString();
                            float.TryParse(gr.Cells[2].Text, out Amount);
                            float.TryParse(gr.Cells[3].Text, out Fine);
                            gr.Cells[4].Text = (Amount + Fine).ToString();
                            _in = true;
                          
                        }
                        if (_in)
                        {
                            Pnl_FeeToPay.Visible = true;
                            Lbl_Header.Text = "";
                        }
                        else
                        {
                            Pnl_FeeToPay.Visible = false;
                            Lbl_Header.Text = "No Fee to pay";

                        }

                    
                }
            }
            
        }
        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            MPE_DD.Hide();
        }
        // calculate Fine Amount for individual fees
        private float Calculate_Fine_Amount(int Header_Id)
        {
            float Fine = 0;
            string sql;
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            sql = "select tblfeeaccount.AccountName as `Fee Name`,tblbatch.BatchName, tblperiod.Period, tblfeestudent.Status, tblfeestudent.BalanceAmount,date_format( tblfeeschedule.LastDate , '%d-%m-%Y') AS 'LastDate', tblfeestudent.SchId, tblfeeschedule.PeriodId, tblfeeschedule.FeeId,tblfeestudent.Id as FeeStudentid, tblfeeschedule.Duedate, tblfeeschedule.BatchId from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId INNER join tbl_feesgrouphead on tbl_feesgrouphead.Id=tblfeeaccount.Header_Id where tblfeestudent.StudId=" + MyParentInfo.StudentId + "  and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion'";
            MydataSet = MyParent.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                MydataSet.Tables[0].Columns.Add("Fine");

                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    DateTime tdydate = System.DateTime.Now.Date;
                    DateTime lastdate = new DateTime();
                    double BalAmnt = 0;
                    double.TryParse(dr["BalanceAmount"].ToString(), out BalAmnt);
                    int finetype = 0, finedate = 0, fineduration;
                    double fine = 0, fineforoneday = 0;
                    OdbcDataReader fineamountreader = null;
                    sql = "SELECT tblfine.Amount, tblfine.Finedate, tblfine.FineAmounttype,tblfine.FineDuration from tblfine where tblfine.FeeId=" + dr["FeeId"].ToString() + " and tblfine.Amount>0";
                    fineamountreader = _mysqlObj.ExecuteQuery(sql);
                    if (fineamountreader.HasRows)
                    {
                        int.TryParse(fineamountreader.GetValue(2).ToString(), out finetype);
                        int.TryParse(fineamountreader.GetValue(1).ToString(), out finedate);
                        double.TryParse(fineamountreader.GetValue(0).ToString(), out fine);
                        int.TryParse(fineamountreader.GetValue(3).ToString(), out fineduration);
                        int diff = 0, lastday = 0, today = 0;
                        string strdate = "";
                        if (finedate == 1)
                        {
                            strdate = dr["LastDate"].ToString().Replace("-", "/");

                        }
                        else if (finedate == 2)
                        {
                            strdate = dr["Duedate"].ToString().Replace("-", "/");
                        }

                        lastdate = General.GetDateFromText(strdate);

                        if (tdydate > lastdate)
                        {
                            double daydiff = (tdydate - lastdate).TotalDays;
                            diff = (int)daydiff;
                            lastday = lastdate.Day;
                            today = tdydate.Day;
                            int extradays = 0;
                            double Finetotal = 0;
                            //diff = (today - lastday);
                            if (diff >= 1)
                            {
                                if (finetype == 1)
                                {
                                    if (diff < fineduration)
                                    {
                                        fineforoneday = fine / fineduration;
                                        Finetotal = (diff * fineforoneday);
                                    }
                                    else
                                    {
                                        Finetotal = fine;

                                    }
                                    Finetotal = Math.Round(Finetotal);
                                    dr["Fine"] = Finetotal.ToString();
                                }
                                else if (finetype == 2)
                                {
                                    double amount = 0;
                                    amount = (BalAmnt * fine / 100);
                                    fineforoneday = amount / fineduration;
                                    if (diff < fineduration)
                                    {
                                        Finetotal = (diff * fineforoneday);
                                    }
                                    else
                                    {
                                        Finetotal = amount;

                                    }
                                    Finetotal = Math.Round(Finetotal);
                                    dr["Fine"] = Finetotal.ToString();

                                }
                                else
                                {
                                    if (finetype == 3)
                                    {
                                        fineforoneday = fine / fineduration;
                                    }
                                    else if (finetype == 4)
                                    {
                                        double amount = 0;
                                        amount = (BalAmnt * fine / 100);
                                        fineforoneday = amount / fineduration;
                                    }
                                    if ((diff % fineduration == 0) || (diff < fineduration))
                                    {
                                        Finetotal = (diff * fineforoneday);
                                        Finetotal = Math.Round(Finetotal);
                                        dr["Fine"] = Finetotal.ToString();
                                    }
                                    else if (diff % fineduration > 0)
                                    {
                                        extradays = diff % fineduration;
                                        int days = 0;
                                        days = diff - extradays;
                                        Finetotal = (days * fineforoneday) + (fineforoneday * extradays);
                                        Finetotal = Math.Round(Finetotal);
                                        dr["Fine"] = Finetotal.ToString();

                                    }
                                }


                            }
                        }
                        else
                        {
                            dr["Fine"] = "0";
                        }
                    }
                    else
                    {
                        dr["Fine"] = "0";
                    }
                    fineamountreader.Close();
                    Fine = float.Parse(dr["Fine"].ToString());
                }
            }
            return Fine;
        }
        private void Load_Fees_Group_Header(int Header_Id)
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            string sql;
            sql = "select tblfeeaccount.AccountName as `Fee Name`,tblbatch.BatchName, tblperiod.Period, tblfeestudent.Status,tblfeestudent.BalanceAmount,date_format( tblfeeschedule.LastDate , '%d-%m-%Y') AS 'LastDate', tblfeestudent.SchId, tblfeeschedule.PeriodId, tblfeeschedule.FeeId,tblfeestudent.Id as FeeStudentid, tblfeeschedule.Duedate, tblfeeschedule.BatchId from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId where tblfeestudent.StudId=" + MyParentInfo.StudentId + "  and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion' and tblfeeaccount.Header_Id=" + Header_Id + "";
            MydataSet = MyParent.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            MydataSet = CalculateFineAmountofFeesGroup(MydataSet);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_Feetopay.Columns[0].Visible = true;
                Grd_Feetopay.Columns[7].Visible = true;
                Grd_Feetopay.Columns[8].Visible = true;
                Grd_Feetopay.Columns[9].Visible = true;
                Grd_Feetopay.Columns[10].Visible = true;
                Grd_Feetopay.Columns[11].Visible = true;
                Grd_Feetopay.Columns[12].Visible = true;
                Grd_Feetopay.DataSource = MydataSet;
                Grd_Feetopay.DataBind();
                Lbl_feeMessage.Text = "";
                Btn_feeexport.Enabled = true;
                Grd_Feetopay.Columns[0].Visible = false;
                Grd_Feetopay.Columns[7].Visible = false;
                Grd_Feetopay.Columns[8].Visible = false;
                Grd_Feetopay.Columns[9].Visible = false;
                Grd_Feetopay.Columns[10].Visible = false;
                Grd_Feetopay.Columns[11].Visible = false;
                Grd_Feetopay.Columns[12].Visible = false;
            }
            else
            {
                Grd_Feetopay.DataSource = null;
                Grd_Feetopay.DataBind();
                Lbl_feeMessage.Text = "No Fee to pay";
                Btn_feeexport.Enabled = false;
                Btn_DD.Visible = false;
            }
            Tabs.ActiveTabIndex = 1;
            ReleaseResourse(_mysqlObj, MyParent);
        }
        // calculate Fine Amount for Fees Group Head
        private DataSet CalculateFineAmountofFeesGroup(DataSet FineDs)
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            if (FineDs != null && FineDs.Tables[0].Rows.Count > 0)
            {
                FineDs.Tables[0].Columns.Add("Fine");

                foreach (DataRow dr in FineDs.Tables[0].Rows)
                {
                    string sql = "";
                    DateTime tdydate = System.DateTime.Now.Date;
                    DateTime lastdate = new DateTime();
                    double BalAmnt = 0;
                    double.TryParse(dr["BalanceAmount"].ToString(), out BalAmnt);
                    int finetype = 0, finedate = 0, fineduration;
                    double fine = 0, fineforoneday = 0;
                    OdbcDataReader fineamountreader = null;
                    sql = "SELECT tblfine.Amount, tblfine.Finedate, tblfine.FineAmounttype,tblfine.FineDuration from tblfine where tblfine.FeeId=" + dr["FeeId"].ToString() + " and tblfine.Amount>0";
                    fineamountreader = _mysqlObj.ExecuteQuery(sql);
                    if (fineamountreader.HasRows)
                    {
                        int.TryParse(fineamountreader.GetValue(2).ToString(), out finetype);
                        int.TryParse(fineamountreader.GetValue(1).ToString(), out finedate);
                        double.TryParse(fineamountreader.GetValue(0).ToString(), out fine);
                        int.TryParse(fineamountreader.GetValue(3).ToString(), out fineduration);
                        int diff = 0, lastday = 0, today = 0;
                        string strdate = "";
                        if (finedate == 1)
                        {
                            strdate = dr["LastDate"].ToString().Replace("-", "/");

                        }
                        else if (finedate == 2)
                        {
                            strdate = dr["Duedate"].ToString().Replace("-", "/");
                        }

                        lastdate = General.GetDateFromText(strdate);

                        if (tdydate > lastdate)
                        {
                            double daydiff = (tdydate - lastdate).TotalDays;
                            diff = (int)daydiff;
                            lastday = lastdate.Day;
                            today = tdydate.Day;
                            int extradays = 0;
                            double Finetotal = 0;
                            //diff = (today - lastday);
                            if (diff >= 1)
                            {
                                if (finetype == 1)
                                {
                                    if (diff < fineduration)
                                    {
                                        fineforoneday = fine / fineduration;
                                        Finetotal = (diff * fineforoneday);
                                    }
                                    else
                                    {
                                        Finetotal = fine;

                                    }
                                    Finetotal = Math.Round(Finetotal);
                                    dr["Fine"] = Finetotal.ToString();
                                }
                                else if (finetype == 2)
                                {
                                    double amount = 0;
                                    amount = (BalAmnt * fine / 100);
                                    fineforoneday = amount / fineduration;
                                    if (diff < fineduration)
                                    {
                                        Finetotal = (diff * fineforoneday);
                                    }
                                    else
                                    {
                                        Finetotal = amount;

                                    }
                                    Finetotal = Math.Round(Finetotal);
                                    dr["Fine"] = Finetotal.ToString();

                                }
                                else
                                {
                                    if (finetype == 3)
                                    {
                                        fineforoneday = fine / fineduration;
                                    }
                                    else if (finetype == 4)
                                    {
                                        double amount = 0;
                                        amount = (BalAmnt * fine / 100);
                                        fineforoneday = amount / fineduration;
                                    }
                                    if ((diff % fineduration == 0) || (diff < fineduration))
                                    {
                                        Finetotal = (diff * fineforoneday);
                                        Finetotal = Math.Round(Finetotal);
                                        dr["Fine"] = Finetotal.ToString();
                                    }
                                    else if (diff % fineduration > 0)
                                    {
                                        extradays = diff % fineduration;
                                        int days = 0;
                                        days = diff - extradays;
                                        Finetotal = (days * fineforoneday) + (fineforoneday * extradays);
                                        Finetotal = Math.Round(Finetotal);
                                        dr["Fine"] = Finetotal.ToString();

                                    }
                                }


                            }
                        }
                        else
                        {
                            dr["Fine"] = "0";
                        }
                    }
                    else
                    {
                        dr["Fine"] = "0";
                    }
                    fineamountreader.Close();
                }
            }

            return FineDs;
        }
        #endregion

    }
}
