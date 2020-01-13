using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
namespace WinEr
{
    public partial class ViewStudFeeDetails : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (Session["StudId"] == null)
            {
                Response.Redirect("SearchStudent.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MyFeeMang = MyUser.GetFeeObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (MyFeeMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(44))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (WinerUtlity.NeedCentrelDB())
                {
                    if (Session[WinerConstants.SessionSchool] == null)
                    {
                        Response.Redirect("Logout.aspx");
                    }
                    objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                }

                if (!IsPostBack)
                {
                   // string _MenuStr;
                  //  _MenuStr = MyStudMang.GetSubStudentMangMenuString(MyUser.UserRoleId, int.Parse(Session["StudType"].ToString()));
                  //  this.SubStudentMenu.InnerHtml = _MenuStr;
                   
                 
                 //   LoadStudentTopData();
                   

                    LoadPreviousBatchesToDropDown();
                    LoadFeeDetailsToGrid();


                    if (int.Parse(Session["StudType"].ToString()) == 1)
                    {
                        LoadfeeDue();
                        LoadTotalAmount();
                        LoadAdvanceDetails();

                    }
                    else
                    {
                        TabFeeToPay.Visible = false;
                        TabPanel_FeeAdvance.Visible = false;
                    }
                    Tabs.ActiveTabIndex = 0;
                }
            }
        }

       



        #region Fee Advance Area

        private void LoadAdvanceDetails()
        {
            LoadAdvanceToGrid();
        }

        private void LoadAdvanceToGrid()
        {
            string sql;
            sql = "select  tblstudentfeeadvance.FeeName, tblstudentfeeadvance.PeriodName, tblbatch.BatchName, tblstudentfeeadvance.Amount from tblstudentfeeadvance inner join  tblbatch on tblstudentfeeadvance.BatchId= tblbatch.Id where tblstudentfeeadvance.StudentId=" + Session["StudId"].ToString() + " ORDER BY tblbatch.Id";
            MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                GridView_Advance.DataSource = MydataSet;
                GridView_Advance.DataBind();
                Label_AdvNote.Text = "";
                Lbl_TotalAdvance.Text = GetTotalAdvance(int.Parse(Session["StudId"].ToString()));
                Btn_exporttoAdvexel.Enabled = true;
            }
            else
            {
                Lbl_TotalAdvance.Text = "0";
                GridView_Advance.DataSource = null;
                GridView_Advance.DataBind();
                Label_AdvNote.Text = "No Advance Fee found.";
                Btn_exporttoAdvexel.Enabled = false;
            }
            
        }

        private string GetTotalAdvance(int _StudId)
        {
           
            string sql = "select Sum(tblstudentfeeadvance.Amount)  from tblstudentfeeadvance where tblstudentfeeadvance.StudentId=" + _StudId;
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            double _total=0;
            if (MyReader.HasRows)
            {
                if (!double.TryParse(MyReader.GetValue(0).ToString(), out _total))
                {
                    _total = 0;
                }
     
            }
            return _total.ToString();
        }

        protected void Btn_exporttoAdvexel_Click(object sender, ImageClickEventArgs e)
        {
            string sql;
            sql = "select  tblstudentfeeadvance.FeeName, tblstudentfeeadvance.PeriodName, tblbatch.BatchName , tblstudentfeeadvance.Amount from tblstudentfeeadvance inner join  tblbatch on tblstudentfeeadvance.BatchId= tblbatch.Id where tblstudentfeeadvance.StudentId=" + Session["StudId"].ToString();
            MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                string FileName = "AdvanceList";
                string _ReportName = "<table><tr><td><b>Advance List</b></td></tr><tr><td>Student Name :" + MyUser.getStudName(int.Parse(Session["StudId"].ToString())) + "</td><td>Date : " + DateTime.Now.ToString() + "</td></tr></table>";
                if (!WinEr.ExcelUtility.ExportDataToExcel(MydataSet, _ReportName, FileName, MyUser.ExcelHeader))
                {
                    WC_MessageBox.ShowMssage("Unable to do Export.");
                }
            }
            else
            {

                
               WC_MessageBox.ShowMssage("Unable to do Export.");
                
            }
            Tabs.ActiveTabIndex = 2;
        }

       #endregion 

        private void LoadTotalAmount()
        {
            string sql;
            if (chkBoxAll.Checked)
            {
                sql = "select SUM(tblfeestudent.BalanceAmount) from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId where tblfeestudent.StudId=" + Session["StudId"].ToString() + "  and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid'";
            }
            else
            {
                sql = "select SUM(tblfeestudent.BalanceAmount) from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId where tblfeestudent.StudId=" + Session["StudId"].ToString() + "  and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid'  and tblfeeschedule.DueDate <= CURRENT_DATE()";
            }
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            double _total;
            if (MyReader.HasRows)
            {
                if (double.TryParse(MyReader.GetValue(0).ToString(), out _total))
                {
                    Txt_Amount.Text = _total.ToString();
                }
                else
                {
                    Txt_Amount.Text = "0";
                }
            }
            else
            {
                Txt_Amount.Text = "0";
            }
        }

        private void LoadfeeDue()
        {
            string sql;
            if (chkBoxAll.Checked)
            {
                sql = "select tblfeeaccount.AccountName,tblbatch.BatchName, tblperiod.Period, tblfeestudent.Status, tblfeestudent.BalanceAmount,date_format( tblfeeschedule.LastDate , '%d-%m-%Y') AS 'LastDate' from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId where tblfeestudent.StudId=" + Session["StudId"].ToString() + "  and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion' ORDER BY tblbatch.Id,tblperiod.`Order`";
            }
            else
            {
                sql = "select tblfeeaccount.AccountName,tblbatch.BatchName, tblperiod.Period, tblfeestudent.Status, tblfeestudent.BalanceAmount,date_format( tblfeeschedule.LastDate , '%d-%m-%Y') AS 'LastDate' from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId where tblfeestudent.StudId=" + Session["StudId"].ToString() + "  and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion' and tblfeeschedule.DueDate <= CURRENT_DATE()  ORDER BY tblbatch.Id,tblperiod.`Order`";
            }
            MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_Feetopay.DataSource = MydataSet;
                Grd_Feetopay.DataBind();
                Lbl_feeMessage.Text = "";
                Btn_feeexport.Enabled = true;
            }
            else
            {
                Grd_Feetopay.DataSource = null;
                Grd_Feetopay.DataBind();
                Lbl_feeMessage.Text = "No Fee to pay";
                Btn_feeexport.Enabled = false;
            }
            Tabs.ActiveTabIndex = 1;
        }

        protected void Grd_Feetopay_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Feetopay.PageIndex = e.NewPageIndex;
            LoadfeeDue();
            Tabs.ActiveTabIndex = 1;
        }

        protected void chkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            LoadfeeDue();
            LoadTotalAmount();
            Tabs.ActiveTabIndex = 1;
        }
        
        private bool QuickBillEnabled()
        {
            bool _valid = false;
            string QuickBill;
            string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='QuickBill'";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                QuickBill = MyReader.GetValue(0).ToString();
                if (QuickBill == "1")
                {
                    _valid = true;
                }
            }
            return _valid;
        }

        private bool PdfReportEnabled()
        {
            bool _valid = false;
            string PdfReport;
            string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='PdfReport'";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                PdfReport = MyReader.GetValue(0).ToString();
                if (PdfReport == "1")
                {
                    _valid = true;
                }
            }
            return _valid;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string sql;
            if (chkBoxAll.Checked)
            {
                sql = "select tblfeeaccount.AccountName,tblbatch.BatchName, tblperiod.Period, tblfeestudent.Status, tblfeestudent.BalanceAmount,date_format( tblfeeschedule.LastDate , '%d-%m-%Y') AS 'LastDate' from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId where tblfeestudent.StudId=" + Session["StudId"].ToString() + "  and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid'";
            }
            else
            {
                sql = "select tblfeeaccount.AccountName,tblbatch.BatchName, tblperiod.Period, tblfeestudent.Status, tblfeestudent.BalanceAmount,date_format( tblfeeschedule.LastDate , '%d-%m-%Y') AS 'LastDate' from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId where tblfeestudent.StudId=" + Session["StudId"].ToString() + "  and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid'  and tblfeeschedule.DueDate <= CURRENT_DATE()";
            }
            MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                //if (!WinEr.ExcelUtility.ExportDataSetToExcel(MydataSet, "FeeList.xls"))
                //{
                //    WC_MessageBox.ShowMssage("This function need Ms office");
                //}


                string FileName = "FeeDueList";

                string _ReportName = "<table><tr><td><b>Fee Due List</b></td></tr><tr><td>Student Name :" + MyUser.getStudName(int.Parse(Session["StudId"].ToString())) + "</td><td>Date : " + DateTime.Now.ToString() + "</td></tr></table>";
            
                if (!WinEr.ExcelUtility.ExportDataToExcel(MydataSet, _ReportName, FileName, MyUser.ExcelHeader))
                {
                    WC_MessageBox.ShowMssage("This function need Ms office");
                }
            }
            Tabs.ActiveTabIndex = 1;
        }

        #region NEW ALL BATCH FEE DETAILS

        protected void Drp_Batch_SelectedIndexChanged(object sender, EventArgs e)
        {
            Tabs.ActiveTabIndex = 0;
            LoadFeeDetailsToGrid();
        }

        protected void Rdb_FeeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Tabs.ActiveTabIndex = 0;
            LoadFeeDetailsToGrid();
        }

        private void LoadPreviousBatchesToDropDown()
        {
            Drp_Batch.Items.Clear();
            Drp_Batch.Items.Add(new ListItem("ALL", "0"));
            string sql = "select tblbatch.Id, tblbatch.BatchName from tblbatch where tblbatch.LastbatchId=" + MyUser.CurrentBatchId + " ORDER BY tblbatch.BatchName";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                Drp_Batch.Items.Add(li);
            }
            Drp_Batch.Items.Add(new ListItem(MyStudMang.GetClassName(MyStudMang.GetClassId(int.Parse(Session["StudId"].ToString())))+":"+ MyUser.CurrentBatchName, MyUser.CurrentBatchId.ToString()));

            sql = "select tblstudentclassmap_history.BatchId, CONCAT( (select tblclass.ClassName from tblclass where tblclass.Id=tblstudentclassmap_history.ClassId), ':', (select tblbatch.BatchName from tblbatch where tblbatch.Id= tblstudentclassmap_history.BatchId)) as ClassName from tblstudentclassmap_history where tblstudentclassmap_history.BatchId<>" + MyUser.CurrentBatchId + " AND  tblstudentclassmap_history.StudentId=" + int.Parse(Session["StudId"].ToString()) + " order by tblstudentclassmap_history.BatchId desc";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
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
                if (int.Parse(Drp_Batch.Items[i].Value) == MyUser.CurrentBatchId)
                {
                    Drp_Batch.SelectedIndex = i;
                    break;
                }
            }
        }

        private void LoadFeeDetailsToGrid()
        {
            try
            {
                
                //string sql = "select tblview_transaction.FeeName as AccountName, tblview_transaction.PeriodName as Period, tblbatch.BatchName,tblview_transaction.AccountTo as AccountType,   tblview_transaction.Amount, date_format( tblview_transaction.PaidDate , '%d-%m-%Y') AS 'PaidDate' ,tblview_transaction.BillNo, tblfeeaccount.`Type`,tblbatch.Id as BatchId from tblview_transaction inner join tblfeeaccount on tblview_transaction.FeeId= tblfeeaccount.Id inner join tblbatch on tblbatch.Id= tblview_transaction.BatchId WHERE (tblview_transaction.UserId=" + int.Parse(Session["StudId"].ToString()) + "  or  tblview_transaction.UserId=(select tbltempstdent.Id from tbltempstdent inner join tblstudent on tbltempstdent.TempId= tblstudent.TempStudentId WHERE tblstudent.Id=" + int.Parse(Session["StudId"].ToString()) + "))";
                string sql = "select tblview_transaction.FeeName as AccountName, tblview_transaction.PeriodName as Period, tblbatch.BatchName, tblaccount.AccountName as AccountType,   tblview_transaction.Amount, date_format( tblview_transaction.PaidDate , '%d-%m-%Y') AS 'PaidDate' ,tblview_transaction.BillNo, tblfeeaccount.`Type`,tblbatch.Id as BatchId from tblview_transaction inner join tblfeeaccount on tblview_transaction.FeeId= tblfeeaccount.Id inner join tblbatch on tblbatch.Id= tblview_transaction.BatchId inner join tblview_feebill on tblview_feebill.BillNo= tblview_transaction.BillNo inner join tblaccount on tblaccount.Id = tblview_transaction.AccountTo INNER JOIN tblperiod ON tblperiod.Id=tblview_transaction.PeriodId  WHERE tblview_feebill.StudentID=" + int.Parse(Session["StudId"].ToString());
                if (int.Parse(Drp_Batch.SelectedValue) > 0)
                {
                    sql = sql + "  and tblview_transaction.BatchId=" + Drp_Batch.SelectedValue;
                }
                if (Rdb_FeeType.SelectedValue == "1")
                {
                    sql = sql + " and tblfeeaccount.`Type`=1";
                }
                else if (Rdb_FeeType.SelectedValue == "2")
                {
                    sql = sql + " and tblfeeaccount.`Type`=2";
                }
                sql = sql + " and tblview_transaction.Canceled=0 ORDER BY tblview_transaction.BatchId,tblperiod.Order";
                //sql = sql + " and tblview_transaction.BatchId!=" + MyUser.CurrentBatchId;

                MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
                {
                    Lbl_TransAllMsg.Text = "";
                    Grd_TransactionsAll.Columns[7].Visible = true;
                    Grd_TransactionsAll.Columns[8].Visible = true;
                    Grd_TransactionsAll.DataSource = MydataSet;
                    Grd_TransactionsAll.DataBind();
                    Grd_TransactionsAll.Columns[7].Visible = false;
                    Grd_TransactionsAll.Columns[8].Visible = false;
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
                WC_MessageBox.ShowMssage(Exc.Message);
            }
            Tabs.ActiveTabIndex = 0;
        }

        protected void Grd_TransactionsAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int _Type = int.Parse(Grd_TransactionsAll.SelectedRow.Cells[7].Text.Trim());
           // string _BatchName = Grd_TransactionsAll.SelectedRow.Cells[2].Text.Trim();
            string _BillId = Grd_TransactionsAll.SelectedRow.Cells[6].Text.Trim();
            //int _BatchId = int.Parse(Grd_TransactionsAll.SelectedRow.Cells[8].Text.Trim());


            if (_BillId != "")
            {
                int _Value = 1;
                string _PageName = "FeeBillSmall.aspx";
                bool Pdf = false;
                if (MyFeeMang.GetBillType(ref _Value, ref _PageName, out Pdf))
                {
                    if (Pdf)
                    {
                        string _ErrorMsg = "";
                        Pdf MyPdf = new Pdf(MyFeeMang.m_MysqlDb, objSchool);
                        _ErrorMsg = "";
                        string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
                        string _PdfName = "";
                        //if (_Type == 1)
                        //{
                            if (MyPdf.CreateFeeReciptPdf(_BillId, _physicalpath, out _PdfName, out _ErrorMsg, _Value))
                            {
                                _ErrorMsg = "";
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                            }
                            else if (_PdfName == "")
                            {
                                Lbl_TransAllMsg.Text = "Failed To Create";
                            }
                        //}
                        //else
                        //{

                        //    if (MyPdf.CreateJoiningFeeReciptPdf(_BillId, _BatchName, _physicalpath, out _PdfName, out _ErrorMsg, _Value))
                        //    {
                        //        _ErrorMsg = "";
                        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                        //    }
                        //    else if (_PdfName == "")
                        //    {
                        //        Lbl_TransAllMsg.Text = "Faild To Create";
                        //    }
                        //}
                    }
                    else
                    {
                        string _Bill = _BillId;
                        //if (_Type == 1 )
                        //{
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"" + _PageName + "?BillNo=" + _Bill + "&BillType=0\");", true);
                            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "window.open(\"" + _PageName + "?BillNo=" + _Bill + "&BillType=0\");", true);
                        //}
                        //else
                        //{
                        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"" + _PageName + "?BillNo=" + _Bill + "&BillType=1\");", true);
                        //}
                    }
                }
            }

            Tabs.ActiveTabIndex = 0;
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
            DataSet _FeeReport = new DataSet();
            _FeeReport = (DataSet)ViewState["FeeDetails"];
            _FeeReport.Tables[0].Columns.Remove("Type");
            _FeeReport.Tables[0].Columns.Remove("BatchId");
            //if (!WinEr.ExcelUtility.ExportDataSetToExcel(_FeeReport, "FeeTransactionReport.xls"))
            //{
            //    WC_MessageBox.ShowMssage("This function need Ms office");
            //}

            string FileName = "FeeTransactionReport";
            string _ReportName = "<table><tr><td><b>Fee Transaction Report</b></td></tr><tr><td>Student Name :" + MyUser.getStudName(int.Parse(Session["StudId"].ToString())) + "</td><td>Date : " + DateTime.Now.ToString() + "</td></tr></table>";
            
            if (!WinEr.ExcelUtility.ExportDataToExcel(_FeeReport, _ReportName, FileName, MyUser.ExcelHeader))
            {
                WC_MessageBox.ShowMssage("This function need Ms office");
            }
            Tabs.ActiveTabIndex = 0;
        }

        #endregion





        //protected void Lnk_PreviousFeeDetails_Click(object sender, EventArgs e)
        //{
        //    if (MyUser.HaveActionRignt(610))
        //    {
        //        Response.Redirect("FeeReportPreviousBatch.aspx");
        //    }
        //    else
        //    {
        //        WC_MessageBox.ShowMssage("This user has no right to view the details");
        //    }
        //}
        //private void LoadStudentTopData()
        //{

        //    string _Studstrip = MyStudMang.ToStripString(int.Parse(Session["StudId"].ToString()), "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StudId"].ToString()) + "&type=StudentImage", int.Parse(Session["StudType"].ToString()));
        //    this.StudentTopStrip.InnerHtml = _Studstrip;
        //}


        
        

       

    }
}