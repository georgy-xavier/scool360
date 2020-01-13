using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using WinBase;
namespace WinEr
{
    public partial class CollectJoiningFee : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet = null;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            OtherFeeBox.EVNTItemAdded += new EventHandler(BindOtherFeeItems);
            FeeAdvanceBox.FeeAdvance += new EventHandler(BindAdvanceFees);
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MyFeeMang = MyUser.GetFeeObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(605))
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
                    bool valid = false;
                    int Studentid = 0;
                    if (Request.QueryString["Studentid"] != null)
                    {
                        int.TryParse(Request.QueryString["Studentid"].ToString(), out Studentid);
                        if (Studentid > 0)
                        {
                            valid = true;
                        }

                    }
                    if (valid)
                    {
                        GetStudentDetails(Studentid);
                        BtnCancelBill.Enabled = false;
                        //MPE_Temp_Fee.Show();
                        LoadSuddentFee();
                        //WC_MsgBox.ShowMssage("No fee to Pay.");
                        //Panel_FeeDataArea.Visible = false;
                        //Panel_Complete.Visible = true;
                        //DateTime _now = System.DateTime.Now;
                        //Txt_Pay_Date.Text = MyUser.GerFormatedDatVal(_now);
                        //TxtTotatAmount.Value = "0";
                        //TxtBillNo.Text = "0";
                    }
                }
            }
        }
        # region TEMP FEE

        private void GetStudentDetails(int _StudentId)
        {
            Hdn_studid.Value = _StudentId.ToString();
            string sql = "SELECT tbltempstdent.Name,tbltempstdent.TempId,tbltempstdent.Gender,tblstandard.Name,tblclass.ClassName,tblbatch.BatchName,tbltempstdent.Standard,tbltempstdent.Class  FROM tbltempstdent INNER JOIN tblclass ON tblclass.Id=tbltempstdent.Class INNER JOIN tblstandard ON tblstandard.Id=tbltempstdent.Standard INNER JOIN tblbatch ON tblbatch.Id=tbltempstdent.JoiningBatch WHERE tbltempstdent.Id=" + _StudentId;
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Lbl_StudentName.Text = MyReader.GetValue(0).ToString();
                Lbl_RegistrationId.Text = MyReader.GetValue(1).ToString();
                Lbl_Gender.Text = MyReader.GetValue(2).ToString();
                Lbl_Standard.Text = MyReader.GetValue(3).ToString();
                Lbl_ClassName.Text = MyReader.GetValue(4).ToString();
                Lbl_JoiningBatch.Text = MyReader.GetValue(5).ToString();

                Hdn_Standid.Value = MyReader.GetValue(6).ToString();
                Hdn_ClassId.Value = MyReader.GetValue(7).ToString();
            }
        }


        private void clearDatas()
        {
            Txt_Balance.Value = "Nil";
            Txt_AmountPaying.Text = "0";
            TxtTotatAmount.Value = "0";
            Txt_bank.Text = "";
            Txt_paymentid.Text = "";
            Rdb_PaymentSelect(0);
            Lbl_FeeBillMessage.Text = "";
        }

        private bool AddfeeduesToGrid()
        {
            bool _Valid = true;
            Lbl_FeeBillMessage.Text = "";
            GridViewAllFee.Columns[1].Visible = true;

            BtnBill.Enabled = false;
            Btn_payfee.Enabled = false;

            // string  sql = "select tblfeestudent.SchId, tblfeestudent.Id, tblfeeaccount.AccountName,tblbatch.BatchName, tblperiod.Period, tblfeestudent.Status, tblfeestudent.BalanceAmount,date_format( tblfeeschedule.LastDate , '%m-%d-%Y') AS 'LastDate' from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblfeestudent.StudId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId where tblstudentclassmap.ClassId=" + int.Parse(DropDownClass.SelectedValue.ToString()) + " and tblstudentclassmap.StudentId=" + int.Parse(DropDownStudentId.SelectedValue.ToString()) + "  and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion'";

            string sql = "select tbljoining_feeschedule.Id, tblfeeaccount.AccountName ,tbljoining_feeschedule.Amount , 0 as Regular , 1 as CollectionType, 0 as PeriodId, tblfeeaccount.Id as FeeId, '-' as PeriodName , " + MyUser.CurrentBatchId + " as BatchId from tbljoining_feeschedule inner join tblfeeaccount on tblfeeaccount.Id =tbljoining_feeschedule.FeeId and tblfeeaccount.`Type`=2  where tbljoining_feeschedule.StandardId=" + int.Parse(Hdn_Standid.Value) + " and tbljoining_feeschedule.Amount<>0  and tbljoining_feeschedule.Id not in(select tbltransaction.PaymentElementId from tbltransaction where tbltransaction.TempId='" + Lbl_RegistrationId.Text + "' and tbltransaction.Canceled<>1 and tbltransaction.RegularFee=0)  and tbljoining_feeschedule.Id not in(select tbltransactionclearence.PaymentElementId from tbltransactionclearence where tbltransactionclearence.TempId='" + Lbl_RegistrationId.Text + "' and tbltransactionclearence.Canceled<>1 and tbltransactionclearence.RegularFee=0)";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                //Lbl_Message.Text = "";
                GridViewAllFee.Columns[1].Visible = true;
                GridViewAllFee.Columns[4].Visible = true;
                GridViewAllFee.Columns[5].Visible = true;
                GridViewAllFee.Columns[6].Visible = true;
                GridViewAllFee.Columns[7].Visible = true;
                GridViewAllFee.Columns[8].Visible = true;
                GridViewAllFee.Columns[9].Visible = true;
                GridViewAllFee.DataSource = MyReader;
                GridViewAllFee.DataBind();
                GridViewAllFee.Columns[1].Visible = false;
                GridViewAllFee.Columns[4].Visible = false;
                GridViewAllFee.Columns[5].Visible = false;
                GridViewAllFee.Columns[6].Visible = false;
                GridViewAllFee.Columns[7].Visible = false;
                GridViewAllFee.Columns[8].Visible = false;
                GridViewAllFee.Columns[9].Visible = false;
                Pnl_feearea.Visible = true;
            }

            else
            {
                GridViewAllFee.DataSource = null;
                GridViewAllFee.DataBind();
                _Valid = false;
                //Lbl_Message.Text = "No Fee to pay";
                // Lbl_msg.Text = "No Fee to pay";
                //this.MPE_MessageBox.Show();
                //WC_MsgBox.ShowMssage("No Fee to pay");
            }
            
            return _Valid;
        }

        private void ReloadData()
        {
            Pnl_feearea.Visible = false;
        }



        private void Checkallfee()
        {

            foreach (GridViewRow gv in GridViewAllFee.Rows)
            {

                CheckBox cb = (CheckBox)gv.FindControl("CheckBoxUpdate");

                cb.Checked = true;

            }

        }



        private void Rdb_PaymentSelect(int mode)
        {

            RadioButton0.Checked = false;
            RadioButton1.Checked = false;
            RadioButton2.Checked = false;
            RadioButton3.Checked = false;

            if (mode == 0)
            {
                RadioButton0.Checked = true;
            }
            else if (mode == 1)
            {
                RadioButton1.Checked = true;
            }
            else if (mode == 2)
            {
                RadioButton2.Checked = true;
            }
            else
            {
                RadioButton3.Checked = true;
            }
        }

        private string Rdb_PaymentSelectvalue()
        {
            if (RadioButton0.Checked)
            {
                return "0";
            }
            else if (RadioButton1.Checked)
            {
                return "1";
            }
            else if (RadioButton2.Checked)
            {
                return "2";
            }
            else
            {
                return "3";
            }
        }

        private string Rdb_PaymentSelectText()
        {
            if (RadioButton0.Checked)
            {
                return RadioButton0.Text;
            }
            else if (RadioButton1.Checked)
            {
                return RadioButton1.Text;
            }
            else if (RadioButton2.Checked)
            {
                return RadioButton2.Text;
            }
            else
            {
                return RadioButton3.Text;
            }
        }


        protected void Btn_payfee_Click(object sender, EventArgs e)
        {
            bool Clearnce = false;
            Btn_payfee.Enabled = false;
            int i = 0;
            string _BillId = "0";

            if (Rdb_PaymentSelectvalue()== "1" && (Txt_paymentid.Text.Trim() == "" || Txt_bank.Text.Trim() == ""))
            {
                WC_MsgBox.ShowMssage("Please Enter Cheque Details");
                Btn_payfee.Enabled = true;
            }
            else if (InValideBalance(Txt_Balance.Value))
            {
                WC_MsgBox.ShowMssage("Balance should be Zero for paying a fee");
            }
            else if (Rdb_PaymentSelectvalue() == "2" && (Txt_paymentid.Text.Trim() == "" || Txt_bank.Text.Trim() == ""))
            {
                WC_MsgBox.ShowMssage("Please Enter DD Details");
                Btn_payfee.Enabled = true;
            }
            else if (Rdb_PaymentSelectvalue() == "3" && (Txt_paymentid.Text.Trim() == "" || Txt_bank.Text.Trim() == ""))
            {
                WC_MsgBox.ShowMssage("Please Enter NEFT Details");

                Btn_payfee.Enabled = true;
            }
            else
            {
                try
                {
                    
                    string BillTable = "", TransactionTable="";
                    int BatchId = MyUser.CurrentBatchId;
                    MyFeeMang.CreateTansationDb();

                    if (MyFeeMang.ClearenceEnabled(Rdb_PaymentSelectText()))
                    {
                        Clearnce = true;
                    }
                    

                    foreach (GridViewRow gv in GridViewAllFee.Rows)
                    {
                        CheckBox cb = (CheckBox)gv.FindControl("CheckBoxUpdate");

                        if (cb.Checked)
                        {
                            double Amount = double.Parse(gv.Cells[3].Text.ToString());
                            double Total = Amount;
                            int ScheduledFeeId = int.Parse(gv.Cells[1].Text.ToString());
                            int StudentId = int.Parse(Hdn_studid.Value.ToString());
                            string FeeName=gv.Cells[2].Text.ToString();
                                string Period="-";
                                int CollectionType = int.Parse(gv.Cells[5].Text.ToString());
                                int PeriodId = int.Parse(gv.Cells[6].Text.ToString());
                                int FeeId = int.Parse(gv.Cells[7].Text.ToString());
                                Period = gv.Cells[8].Text.ToString();
                                int _TrnsBatchId = int.Parse(gv.Cells[9].Text.ToString());
                            if (i == 0)
                            {
                                double _Total = double.Parse(TxtTotatAmount.Value.ToString());

                                if (!Clearnce)
                                {
                                    BillTable = "tblfeebill";
                                }
                                else
                                {
                                    BillTable = "tblfeebillclearence";
                                }
                                _BillId = MyFeeMang.GenBill(_Total, StudentId, Rdb_PaymentSelectText(), Txt_paymentid.Text, Txt_bank.Text, Txt_Pay_Date.Text, MyUser.UserId, MyUser.CurrentBatchId, Hdn_ClassId.Value, Lbl_StudentName.Text, 0, Lbl_RegistrationId.Text,"", BillTable);
                                i = 1;

                            }

                            if (_BillId != "0")
                            {

                                if (MyFeeMang.ValidJoingTransaction(ScheduledFeeId, StudentId, Amount, int.Parse(Hdn_Standid.Value)) || ScheduledFeeId==-1)
                                {
                                    if (!Clearnce)
                                        TransactionTable = "tbltransaction";
                                    else
                                        TransactionTable = "tbltransactionclearence";

                                    MyFeeMang.FillTransaction(ScheduledFeeId, StudentId, Amount, 0, 0, Total, 0, _BillId, Hdn_ClassId.Value, Lbl_StudentName.Text, MyUser.UserName, _TrnsBatchId, FeeName, Period, CollectionType, 0, Lbl_RegistrationId.Text, TransactionTable, PeriodId, FeeId, Txt_Pay_Date.Text);
                                    
                                }
                                else
                                {
                                    _BillId = "0";

                                }

                            }


                        }

                    }


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
                                Pdf MyPdf = new Pdf(MyFeeMang.m_TransationDb, objSchool);
                                _ErrorMsg = "";
                                string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
                                string _PdfName = "";

                                if (MyPdf.CreateFeeReciptPdf(_BillId, _physicalpath, out _PdfName, out _ErrorMsg, _Value) && (!Clearnce))
                                {
                                    _ErrorMsg = "";
                                    ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);

                                }
                                else if ((_PdfName == "") && (!Clearnce))
                                {
                                    _ErrorMsg = "Failed To Create";
                                    _BillId = "0";
                                }
                            }
                            else
                            {
                                string _Bill = _BillId;
                                if (!Clearnce)
                                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "keyClientBlock", "window.open(\"" + _PageName + "?BillNo=" + _Bill + "&BillType=1\");", true);
                              
                           }
                        }
                    }

                    if (_BillId != "0")
                    {
                        MyFeeMang.EndSucessTansationDb();
                    }
                    else
                    {
                        MyFeeMang.EndFailTansationDb();
                    }
                    LoadSuddentFee();
                    if ((_BillId != "0") && (!Clearnce))
                    {
                        TxtBillNo.Text = _BillId.ToString();
                        
                        Lbl_FeeBillMessage.Text = "Fee paid.Your bill number is " + _BillId + " .To see your bill,click \"Generate Bill\"";
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Collect Joining Fee", "Fee is collected for billno " + _BillId, 1);
                        BtnBill.Enabled = true;
                        BtnCancelBill.Enabled = true;
                    }
                    else if ((_BillId != "0") && (Clearnce))
                    {
                        Lbl_FeeBillMessage.Text = "Your transaction is waiting for clearence. Get the bill after clearence";
                        BtnBill.Enabled = false;
                        BtnCancelBill.Enabled = false;
                    }
                    else
                    {
                        WC_MsgBox.ShowMssage("Payment is not completed please try again...");
                    }

                    Pnl_feearea.Visible = true;
                    //this.MPE_MessageBox.Hide();
                    if (BtnCancelBill.Enabled)
                    {
                        if (!MyUser.HaveActionRignt(128))
                        {
                            BtnCancelBill.Enabled = false;
                        }
                    }
                    Lnk_Advance.Enabled = false;
                    Lnk_OtherFee.Enabled = false;
                }
                catch (Exception Ex)
                {
                    MyFeeMang.EndFailTansationDb();
                    WC_MsgBox.ShowMssage(Ex.Message);
                    Btn_payfee.Enabled = true;
                }

            }
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "Rdb_Changing" + Rdb_PaymentSelectvalue() + "();", true);
        }

        private bool QuickBillEnabled()
        {
            bool _valid = false;
            string QuickBill;
            string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='QuickBill'";
            MyReader = MyFeeMang.m_TransationDb.ExecuteQuery(sql);
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
            MyReader = MyFeeMang.m_TransationDb.ExecuteQuery(sql);
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

        private bool LoadSuddentFee()
        {
            bool _Valid = true;
            DateTime _now = System.DateTime.Now;
            Txt_Pay_Date.Text = MyUser.GerFormatedDatVal(_now);
            TxtTotatAmount.Value = "0";
            TxtBillNo.Text = "0";
            //if (AddfeeduesToGrid())
            //{
            //    _Valid = true;
                //clearDatas();
                //Checkallfee();
                //ClaculateTotalAmount();
            //    Panel_FeeDataArea.Visible = true;
            //    Panel_Complete.Visible = false;
            //}
            //else
            //{
            WC_MsgBox.ShowMssage("No fee to Pay.");
            Panel_FeeDataArea.Visible = false;
            Panel_Complete.Visible = true;
            //}
            return _Valid;
        }




        private void ClaculateTotalAmount()
        {


            double Sum = 0;
            double _balance;
            bool _checked = false;
            BtnBill.Enabled = false;
            TxtTotatAmount.Value = "0";
            try
            {
                if (Txt_AmountPaying.Text.Trim() == "")
                {

                    Txt_Balance.Value = "0";
                }
                else
                {
                    _balance = Sum - double.Parse(Txt_AmountPaying.Text);
                    Txt_Balance.Value = _balance.ToString();
                }
            }
            catch
            {
                Txt_Balance.Value = "Nil";
            }
            Btn_payfee.Enabled = false;
            foreach (GridViewRow gv in GridViewAllFee.Rows)
            {
                CheckBox cb = (CheckBox)gv.FindControl("CheckBoxUpdate");
                if (cb.Checked)
                {
                    _checked = true;
                    Sum = Sum + double.Parse(gv.Cells[3].Text.ToString());
                    if (Sum >= 0)
                    {
                        TxtTotatAmount.Value = Sum.ToString();
                    }
                    else
                    {
                        //Lbl_MessageError.Text = "This amount is not valid";
                        // this.MPE_MessageError.Show();
                        TxtTotatAmount.Value = "0";
                        Txt_Balance.Value = "Nil";
                        break;
                    }
                    try
                    {
                        if (Txt_AmountPaying.Text.Trim() == "")
                        {
                            Txt_Balance.Value = TxtTotatAmount.Value;
                        }
                        else
                        {
                            _balance = Sum - double.Parse(Txt_AmountPaying.Text);
                            Txt_Balance.Value = _balance.ToString();
                        }
                    }
                    catch
                    {
                        Txt_Balance.Value = "Nil";
                    }
                }
            }
            if (_checked && Txt_Balance.Value == "0")
            {
                Btn_payfee.Enabled = true;
            }
        }

        private bool InValideBalance(string _balance)
        {
            bool _Invalid;
            double _blnce;
            _Invalid = true;
            if (double.TryParse(_balance, out _blnce))
            {
                if (_blnce == 0)
                {
                    _Invalid = false;
                }
            }

            return _Invalid;
        }

        protected void BtnBill_Click(object sender, EventArgs e)
        {

            string _Bill = TxtBillNo.Text.ToString();
            string BillType = "0";



            if (_Bill != "")
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


                        if (MyPdf.CreateFeeReciptPdf(_Bill,_physicalpath, out _PdfName, out _ErrorMsg, _Value))
                        {
                            _ErrorMsg = "";
                            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                        }
                        else if (_PdfName == "")
                        {
                            Lbl_BillMessage.Text = "Failed To Create";
                        }
                    }
                    else
                    {                    
                        ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"" + _PageName + "?BillNo=" + _Bill + "&BillType=1\");", true);
                    }
                }
            }


            
        }
     

        # endregion


        # region CANCEL BILL

        protected void BtnCancelBill_Click(object sender, EventArgs e)
        {

            this.MPE_CancelBill.Show();

        }

        protected void Btn_CancelBill_Click(object sender, EventArgs e)
        {
            int _Studentid = -1;
            string PayedDate = "";
            string _Message;
            if (MyFeeMang.CancelBill(TxtBillNo.Text.Trim(), Hdn_studid.Value, "tblfeebill", "tbltransaction", 2, out _Studentid, out PayedDate, out _Message))
            {
                Lbl_FeeBillMessage.Text = "The Bill " + TxtBillNo.Text + " is canceled";
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Cancel payment", "The billno " + TxtBillNo.Text + " for the student " + Lbl_StudentName.Text + " has been canceled", 1);
                MyFeeMang.WriteCancelLog(TxtBillNo.Text.Trim(), _Studentid, 2, MyUser.UserId, PayedDate, Txt_CancelReason.Text.Trim());//1 Reguler fee
                BtnBill.Enabled = false;
                BtnCancelBill.Enabled = false;
                //Btn_payfee.Enabled = true;
                LoadSuddentFee();
                
                Txt_CancelReason.Text = "";
            }
            else
            {
                Lbl_FeeBillMessage.Text = _Message;
                BtnBill.Enabled = true;

            }
        }

        # endregion




        # region FeeAdvance

        protected void Lnk_Advance_Click(object sender, EventArgs e)
        {
            FeeAdvanceBox.Display();
        }

        protected void BindAdvanceFees(object sender, EventArgs e)
        {

            OtherFeeArguments _e = (OtherFeeArguments)e;
            if (_e.FeeName != "" && _e.Amount > 0)
            {
                BindFeeGrid(_e.SchId, _e.FeeStudId, _e.FeeName, _e.BatchName, _e.Period, _e.Status, _e.Amount, _e.LastDate, _e.Deduction, _e.Arrear, _e.Fine, _e.Regular, _e.CollectionType,_e.PeriodId,_e.Feeid, _e.BatchId);
                Pnl_feearea.Visible = true;
                Panel_FeeDataArea.Visible = true;
                Panel_Complete.Visible = false;
            }
            else
            {
                //WC_MessageBox.ShowMssage("Error. Please try again");
            }
        }

        # endregion


        # region otherFee

        protected void Lnk_OtherFee_Click(object sender, EventArgs e)
        {

            OtherFeeBox.LoadOtherFees();
        }




        protected void BindOtherFeeItems(object sender, EventArgs e)
        {
            OtherFeeArguments _e = (OtherFeeArguments)e;
            if (_e.FeeName != "" && _e.Amount > 0)
            {
                BindFeeGrid(_e.SchId, _e.FeeStudId, _e.FeeName, _e.BatchName, _e.Period, _e.Status, _e.Amount, _e.LastDate, _e.Deduction, _e.Arrear, _e.Fine, _e.Regular, _e.CollectionType , _e.PeriodId , _e.Feeid , _e.BatchId);
                Pnl_feearea.Visible = true;
                Panel_FeeDataArea.Visible = true;
                Panel_Complete.Visible = false;
            }
            else
            {
               // WC_MessageBox.ShowMssage("Error. Please try again");
            }
        }


        private void BindFeeGrid(int SchId, string FeeStudId, string _FeeName, string BatchName, string Period, string Status, double _Amount, string LastDate, double Deduction, double Arrear, double Fine, int Regular, int CollectionType, string PeriodId, string FeeId, string _BatchId)
        {
            GridViewAllFee.Columns[1].Visible = true;
            GridViewAllFee.Columns[4].Visible = true;
            GridViewAllFee.Columns[5].Visible = true;
            GridViewAllFee.Columns[6].Visible = true;
            GridViewAllFee.Columns[7].Visible = true;    
            GridViewAllFee.Columns[8].Visible = true;
            GridViewAllFee.Columns[9].Visible = true;
            DataSet _GridData = LoadoGridData();
            Combine(ref _GridData, SchId, FeeStudId, _FeeName, BatchName, Period, Status, _Amount, LastDate, Deduction, Arrear, Fine, Regular, CollectionType, PeriodId, FeeId , _BatchId);
            GridViewAllFee.DataSource = _GridData;
            GridViewAllFee.DataBind();
            ReloadGridData(_GridData);
            ClaculateTotalAmount();
            GridViewAllFee.Columns[1].Visible = false;
            GridViewAllFee.Columns[4].Visible = false;
            GridViewAllFee.Columns[5].Visible = false;
            GridViewAllFee.Columns[6].Visible = false;
            GridViewAllFee.Columns[7].Visible = false;
            GridViewAllFee.Columns[8].Visible = false;
            GridViewAllFee.Columns[9].Visible = false;
        }

        private void ReloadGridData(DataSet _GridData)
        {
            // UPDATING THE CHECK BOX AND DISCOUNT VALUE
            int _count = 0;
            foreach (GridViewRow gvr in GridViewAllFee.Rows)
            {
                CheckBox _ChkBox = (CheckBox)gvr.FindControl("CheckBoxUpdate");
                if (_GridData.Tables[0].Rows[_count][0].ToString() == "true")
                {
                    _ChkBox.Checked = true;
                }
                else
                {
                    _ChkBox.Checked = false;
                }
                _count++;
            }

        }

        private void Combine(ref DataSet _GridData, int SchId, string FeeStudId, string _FeeName, string BatchName, string Period, string Status, double _Amount, string LastDate, double Deduction, double Arrear, double Fine, int Regular, int _CollectionType, string _PeriodId, string _FeeId , string _BatchId)
        {
            DataRow _Dr;
            _Dr = _GridData.Tables["OtherRegularFee"].NewRow();
            _Dr["CkkBox"] = "true";
            _Dr["Id"] = SchId;
            _Dr["AccountName"] = _FeeName;
            _Dr["Amount"] = _Amount;
            _Dr["Regular"] = Regular;
            _Dr["CollectionType"] = _CollectionType;
            _Dr["PeriodId"] = _PeriodId;
            _Dr["FeeId"] = _FeeId;
            _Dr["PeriodName"] = Period;
            _Dr["BatchId"] = _BatchId;
            _GridData.Tables["OtherRegularFee"].Rows.Add(_Dr);
        }

        private DataSet LoadoGridData()
        {
            DataRow _Dr;
            DataTable dt;
            DataSet _DataSet = new DataSet();
            _DataSet.Tables.Add(new DataTable("OtherRegularFee"));
            dt = _DataSet.Tables["OtherRegularFee"];

            dt.Columns.Add("CkkBox");
            dt.Columns.Add("Id");
            dt.Columns.Add("AccountName");
            dt.Columns.Add("Amount");
            dt.Columns.Add("Regular");
            dt.Columns.Add("CollectionType");
            dt.Columns.Add("PeriodId");
            dt.Columns.Add("FeeId");
            dt.Columns.Add("PeriodName");
            dt.Columns.Add("BatchId");
            foreach (GridViewRow gvr in GridViewAllFee.Rows)
            {
                _Dr = dt.NewRow();
                CheckBox _ChkBox = (CheckBox)gvr.FindControl("CheckBoxUpdate");
                if (_ChkBox.Checked)
                {
                    _Dr["CkkBox"] = "true";
                }
                else
                {
                    _Dr["CkkBox"] = "false";
                }
                _Dr["Id"] = gvr.Cells[1].Text;
                _Dr["AccountName"] = gvr.Cells[2].Text;
                _Dr["Amount"] = gvr.Cells[3].Text;
                _Dr["Regular"] = gvr.Cells[4].Text;
                _Dr["CollectionType"] = gvr.Cells[5].Text;
                _Dr["PeriodId"] = gvr.Cells[6].Text;
                _Dr["FeeId"] = gvr.Cells[7].Text;
                _Dr["PeriodName"] = gvr.Cells[8].Text;
                _Dr["BatchId"] = gvr.Cells[9].Text;
                
                _DataSet.Tables["OtherRegularFee"].Rows.Add(_Dr);
            }
            return _DataSet;

        }

        # endregion
    }
}
