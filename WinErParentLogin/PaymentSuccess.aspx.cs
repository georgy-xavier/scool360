using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using WinBase;
using System.Data.Odbc;
using System.Data;
using UpService;

namespace WinErParentLogin
{
    public partial class PaymentSuccess : System.Web.UI.Page
    {
        private MysqlClass m_MysqlDb;
        private SchoolClass objSchool = null;
        private MysqlClass m_TransationDb = null;
        private OdbcDataReader m_MyReader = null;
        private DataSet MydataSet = null;
        private string m_BillPrefix;
        private int HP_Id = 0;
        private string Connection = "";
        private string Payer_Name = "";
        private string Transaction_No = string.Empty;
        private string Bill_No = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string Bank_Reference_No = "";
            string Payment_Mode = "";
            int School_Id = 0;
            try
            {

                // Declare Variables for Check Hash Sequence
                string[] merc_hash_vars_seq;
                string merc_hash_string = string.Empty;
                string merc_hash = string.Empty;
                string hash_seq = "";
                string Status = "";

                if (Request.Form["status"] == "success")
                {
                    Status = Request.Form["status"];
                }
                if (!IsPostBack)
                {
                    // Check Status of Transaction
                    if (Status.ToLower() == "success")
                    {
                        // Declare Variables for Update DataBase Tables
                        string Salt = "";
                        string action = "";
                        int Header_Id = 0;
                        double Amout_Total = 0;
                        int Student_Id = 0;
                        int Class_Id = 0;
                        int Batch_Id = 0;
                        // Assign Return values of Request Form to variables
                        Transaction_No = Request.Form["txnid"];
                        // Get School Id from Transaction No
                        School_Id = Get_Id_From_TransactionNo(Transaction_No);
                        // creating database object based on School Id
                        if (WinerUtlity.NeedCentrelDB())
                        {
                            objSchool = WinerUtlity.GetSchoolObject(School_Id);
                            Connection = WinerUtlity.GetConnectionString(objSchool);
                            m_MysqlDb = new MysqlClass(Connection);
                            m_TransationDb = m_MysqlDb;
                            //m_MysqlDb = m_MysqlDb;
                        }
                        //
                        Lbl_TransName.Visible = true;
                        LblTransref.Visible = true;
                        lblTransactionId.Text = Request.Form["txnid"].ToString();
                        Bank_Reference_No = Request.Form["bank_ref_num"].ToString();
                        Payment_Mode = Get_Mode_Of_Payment(Request.Form["mode"].ToString());
                        Payer_Name = Request.Form["firstname"].ToString();
                        // Change the Status of Payment Based on Transaction_No
                        Online_Payment_Success(Transaction_No, Bank_Reference_No, Payment_Mode, CommonEnum.OnlineTransactionStatus.PaymentSuccess.ToString());
                        // Save form in database
                        string Form = "";
                        if (Request.Form != null)
                        {
                            Form = Request.Form.ToString();
                        }
                        Save_Form_To_Database(Transaction_No, Form);
                        // Taking Payment Details from DataBase
                        Get_Header_Payment_Details(Transaction_No, out HP_Id, out Header_Id, out Amout_Total, out Student_Id, out Class_Id, out Batch_Id);
                        DataSet Ds_Gateway = new DataSet();
                        Ds_Gateway = Get_Gateway_Configurations(Header_Id);
                        if (Ds_Gateway != null && Ds_Gateway.Tables[0] != null && Ds_Gateway.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in Ds_Gateway.Tables[0].Rows)
                            {
                                Salt = dr["SALT"].ToString();
                                hash_seq = dr["hashSequence"].ToString();

                            }
                        }
                        // Check Return Hash Sequence
                        merc_hash_vars_seq = hash_seq.Split('|');
                        Array.Reverse(merc_hash_vars_seq);
                        merc_hash_string = Request.Form["additionalCharges"] + "|" + Salt + "|" + Request.Form["status"];


                        foreach (string merc_hash_var in merc_hash_vars_seq)
                        {
                            merc_hash_string += "|";
                            merc_hash_string = merc_hash_string + (Request.Form[merc_hash_var] != null ? Request.Form[merc_hash_var] : "");

                        }

                        merc_hash = Generatehash512(merc_hash_string).ToLower();

                        if (merc_hash != Request.Form["hash"])
                        {
                            // Hash Sequence Not Matched And Update Status of Payment In DataBase
                            Online_Payment_Success(Transaction_No, Bank_Reference_No, Payment_Mode, CommonEnum.OnlineTransactionStatus.PaymentSuccessButHashNotMatched.ToString());

                        }
                        else
                        {
                            // Hash Sequence Matched And Update Status of Payment In DataBase
                            Online_Payment_Success(Transaction_No, Bank_Reference_No, Payment_Mode, CommonEnum.OnlineTransactionStatus.BillPending.ToString());
                            // Modifying Internal Fees Tables And Generate Fees Bill
                            Payment_Success(HP_Id, Transaction_No, Student_Id, Class_Id, Amout_Total, Batch_Id, out Bill_No);
                            // After Generate Bill Update Status of Payment In DataBase
                            Online_Payment_Success(Transaction_No, Bank_Reference_No, Payment_Mode, CommonEnum.OnlineTransactionStatus.Billed.ToString());
                            // Add Log in DataBase
                            action = "Online Payment Success:student Id=" + Student_Id + ",Amout=" + Amout_Total + ",Transaction Id=" + Transaction_No + ",Actin Date=" + DateTime.Now.ToString() + "";
                            DBLogClass _DblogObj = new DBLogClass(m_MysqlDb);
                            _DblogObj.LogToDb(Payer_Name, "ParentLogin_OnlinePayment", action, 4, 2);
                            // Load Paid Fees Details
                            LoadPaidFees(HP_Id);
                        }

                    }
                    else
                    {
                        Lbl_TransName.Visible = false;
                        LblTransref.Visible = false;
                    }
                }
                else
                {
                    Lbl_TransName.Visible = false;
                    LblTransref.Visible = false;
                }

            }

            catch (Exception ex)
            {
                Lbl_FeeBillMessage.Text = ex.Message;
                Online_Payment_Success(Transaction_No, Bank_Reference_No, Payment_Mode, "Excepection Occurred");

            }
        }
        // Check Mode of Payment done Payer
        private string Get_Mode_Of_Payment(string Mode)
        {
            string Payment_Mode = "";
            if (Mode.ToUpper() == "CC")
            {
                Payment_Mode = "credit-card";
            }
            else if (Mode.ToUpper() == "NB")
            {
                Payment_Mode = "net-banking";
            }
            else if (Mode.ToUpper() == "CD")
            {
                Payment_Mode = "cheque";
            }
            else if (Mode.ToUpper() == "CO")
            {
                Payment_Mode = "Cash";
            }
            else if (Mode.ToUpper() == "DC")
            {
                Payment_Mode = "Debit-Card";
            }
            return Payment_Mode;
        }
        #region Public properties for Transfer data to Bill Page

        public string Bill_Id
        {
            get
            {
                return Bill_No;
            }
        }
        public string Trans_No
        {
            get
            {
                return Transaction_No;
            }
        }
        public string Connection_str
        {
            get
            {
                return Connection;
            }
        }

        #endregion
        private int Get_Id_From_TransactionNo(string Transaction_No)
        {
            int School_Id = 0;
            string[] split = Transaction_No.Split('P', '-');
            School_Id = int.Parse(split[1].ToString());
            return School_Id;
        }
        #region Get_Payment_And_Configuration_Details
        private void Get_Header_Payment_Details(string Transaction_No, out int Hp_Id, out int Header_Id, out double Total_Amount, out int Student_Id, out int Class_Id, out int Batch_Id)
        {
            Hp_Id = 0;
            Header_Id = 0;
            Total_Amount = 0;
            Student_Id = 0;
            Class_Id = 0;
            Batch_Id = 0;
            DataSet Ds_HPD = new DataSet();
            MysqlClass _mysqlObj = new MysqlClass(Connection);
            string sql;
            sql = "select * from tbl_header_payment where tbl_header_payment.Transaction_Id='" + Transaction_No + "'";
            Ds_HPD = _mysqlObj.ExecuteQueryReturnDataSet(sql);
            if (Ds_HPD != null && Ds_HPD.Tables[0] != null && Ds_HPD.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Ds_HPD.Tables[0].Rows)
                {
                    int.TryParse(dr["Id"].ToString(), out Hp_Id);
                    int.TryParse(dr["Header_Id"].ToString(), out Header_Id);
                    double.TryParse(dr["Amount"].ToString(), out Total_Amount);
                    int.TryParse(dr["Student_Id"].ToString(), out Student_Id);
                    int.TryParse(dr["Class_Id"].ToString(), out Class_Id);
                    //int.TryParse(dr["Batch_Id"].ToString(), out Batch_Id);
                }
            }

        }
        private DataSet Get_Gateway_Configurations(int Group_Id)
        {
            DataSet Ds_GateWay = new DataSet();
            MysqlClass _mysqlObj = new MysqlClass(Connection);
            string sql;
            sql = "select * from tbl_feesgatewayconfig inner join tbl_feesgrouphead on tbl_feesgatewayconfig.Id=tbl_feesgrouphead.GateWay_Id where tbl_feesgrouphead.Id=" + Group_Id + "";
            Ds_GateWay = _mysqlObj.ExecuteQueryReturnDataSet(sql);
            return Ds_GateWay;

        }
        #endregion
        //save Form to database
        private void Save_Form_To_Database(string Trans_No, string Form)
        {
            DataSet Ds_GateWay = new DataSet();
            MysqlClass _mysqlObj = new MysqlClass(Connection);
            string sql;
            DateTime Action_Dt = DateTime.Now;
            sql = "Insert into tbl_track_hash(Transaction_No,Form,Page_Name,Action_Date) values('" + Trans_No + "','" + Form + "','PaymentSuccess.aspx','" + Action_Dt.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            _mysqlObj.ExecuteQuery(sql);

        }
        // Generate Hash Sequence
        public string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;

        }
        // Update Status of Payment and Mode of Payment,Bank Reference No in Payment Header Table
        private void Online_Payment_Success(string Transaction_No, string Bank_Reference_No, string Payment_Mode, string Status)
        {

            MysqlClass _mysqlObj = new MysqlClass(Connection);
            string sql = "";
            sql = "update tbl_header_payment set Status='" + Status + "',Payment_Mode='" + Payment_Mode + "',Bank_RefNo='" + Bank_Reference_No + "' where Transaction_Id='" + Transaction_No + "'";
            _mysqlObj.ExecuteQuery(sql);
        }


        //Payment Success function

        private void Payment_Success(int Hp_Id, string Transaction_No, int SELECTEDStudentID, int SELECTEDCLASSID, double Total_Amount, int BatchId, out string _BillId)
        {

            int i = 0;
            _BillId = "0";
            bool Clearence = false;

            string Transaction_Bank = "";
            string Batch_Name = "";
            MysqlClass _mysqlObj = new MysqlClass(Connection);

            try
            {

                string StudentName = GetStudentName(SELECTEDStudentID);
                int userId = GetOnlineUserId();

                DataSet Ds_Fees = new DataSet();
                Ds_Fees = Load_Fees_Group_Header(Transaction_No, SELECTEDStudentID);
                if (Ds_Fees != null && Ds_Fees.Tables[0] != null && Ds_Fees.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in Ds_Fees.Tables[0].Rows)
                    {
                        double Deduction = 0;
                        double Fine = double.Parse(dr["Fine"].ToString());
                        double Amount = double.Parse(dr["BalanceAmount"].ToString());
                        double ArrierAmount = 0;
                        double Total = ((Amount - Deduction) - ArrierAmount);
                        int ScheduledFeeId = int.Parse(dr["Id"].ToString());
                        int StudentId = SELECTEDStudentID;

                        string ClassID = SELECTEDCLASSID.ToString();
                        string FeeName = dr["Fee_Name"].ToString();
                        string Period = dr["Period"].ToString();
                        int CollectionType = 5;
                        int PeriodId = int.Parse(dr["PeriodId"].ToString());
                        int FeeId = int.Parse(dr["FeeId"].ToString());
                        int TrnsBatchId = int.Parse(dr["BatchId"].ToString());
                        BatchId = int.Parse(dr["BatchId"].ToString());
                        string _OtherReferance = "";
                        string BillTable = "";
                        string TransactionTable = "";
                        Batch_Name = dr["BatchName"].ToString();
                        if (ValidAmount(ScheduledFeeId, Amount, StudentId) || ScheduledFeeId == -1)
                        {
                            if (i == 0)
                            {
                                double _Total = Total_Amount;


                                BillTable = "tblfeebill";

                                _mysqlObj.MyBeginTransaction();
                                m_TransationDb = _mysqlObj;
                                _BillId = GenBill(_Total, StudentId, "Online Payment", Transaction_No, Transaction_Bank, System.DateTime.Now.Date.ToString("dd/MM/yyyy"), userId, BatchId, ClassID, StudentName, 1, "", _OtherReferance, BillTable);
                                i = 1;
                            }
                            if (_BillId != "0")
                            {
                                if (ScheduledFeeId != -1)// -1 FOR OTHER FEE
                                {
                                    if (ValidTransaction(ScheduledFeeId, StudentId, Amount))
                                    {

                                        TransactionTable = "tbltransaction";

                                        FillTransaction(ScheduledFeeId, StudentId, Amount, Deduction, Fine, Total, ArrierAmount, _BillId, ClassID, StudentName, Payer_Name, TrnsBatchId, FeeName, Period, CollectionType, 1, "", TransactionTable, PeriodId, FeeId, System.DateTime.Now.Date.ToString("dd/MM/yyyy"));
                                    }
                                    else
                                        _BillId = "0";
                                }
                                else
                                {

                                    TransactionTable = "tbltransaction";

                                    FillTransaction(ScheduledFeeId, StudentId, Amount, Deduction, Fine, Total, ArrierAmount, _BillId, ClassID, StudentName, Payer_Name, TrnsBatchId, FeeName, Period, CollectionType, 1, "", TransactionTable, PeriodId, FeeId, System.DateTime.Now.Date.ToString("dd/MM/yyyy"));
                                }

                            }



                        }
                        else
                            _BillId = "0";
                    }
                    //Update Bill_No Payment Header Table
                    Updatebill_No_to_Header(_BillId, Hp_Id);
                }


                if (_BillId != "0")
                {

                    string _PageName = "FeeBillNew.aspx";

                    string Biil_No = _BillId.ToString();

                    string _Bill = Biil_No;

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open(\"" + _PageName + "?BillNo=" + _Bill + "&Trans_No=" + Transaction_No + "\")", true);

                    Lbl_FeeBillMessage.Text = "Fee paid.Your bill number is " + _BillId + "";


                }

                if (_BillId != "0")
                {
                    _mysqlObj.TransactionCommit();
                    int StudentId = SELECTEDStudentID;
                    StoreIncident(_BillId, StudentId, BatchId, SELECTEDCLASSID);

                }
                else
                {
                    _mysqlObj.TransactionRollback();
                }

                if ((_BillId != "0") && (!Clearence))
                {
                    string Biil_No = _BillId.ToString();

                    Lbl_FeeBillMessage.Text = "Fee paid.Your bill number is " + _BillId + " .";


                }

            }

            catch (Exception Ex)
            {
                _mysqlObj.TransactionRollback();
                Lbl_FeeBillMessage.Text = Ex.Message;

            }


        }
        private void Updatebill_No_to_Header(string Bill_No, int Hp_Id)
        {
            if (m_TransationDb != null)
            {
                string sql = "update tbl_header_payment set Biil_No='" + Bill_No + "' where Id=" + Hp_Id + "";
                m_TransationDb.ExecuteQuery(sql);

            }
        }

        private string GetStudentName(int _SELECTEDStudentID)
        {
            string StudentName = "";
            MysqlClass _mysqlObj = new MysqlClass(Connection);
            string sql = "SELECT tblview_student.StudentName FROM tblview_student WHERE tblview_student.Id=" + _SELECTEDStudentID;
            m_MyReader = _mysqlObj.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                StudentName = m_MyReader.GetValue(0).ToString();
            }
            return StudentName;
        }

        private int GetOnlineUserId()
        {
            int _userId = 0;
            MysqlClass _mysqlObj = new MysqlClass(Connection);
            string sql = "select Id from tbluser where UserName='Online'";
            m_MyReader = _mysqlObj.ExecuteQuery(sql);
            while (m_MyReader.Read())
            {
                _userId = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            return _userId;
        }

        private DataSet Load_Fees_Group_Header(string Transaction_Id, int Student_Id)
        {
            MysqlClass _mysqlObj = new MysqlClass(Connection);

            string sql;
            DataSet Ds_Fees = new DataSet();
            sql = "select DISTINCT(tblfeestudent.Id) as FeeStudentid, tblfeeaccount.AccountName as `Fee_Name`,tblbatch.BatchName, tblperiod.Period, tblfeestudent.Status,tblfeestudent.BalanceAmount,date_format( tblfeeschedule.LastDate , '%d-%m-%Y') AS 'LastDate', tblfeestudent.SchId, tblfeeschedule.PeriodId, tblfeeschedule.Id, tblfeeschedule.FeeId,tblfeeschedule.Duedate, tblfeeschedule.BatchId from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId inner join tbl_fees_payment on tbl_fees_payment.Fees_Id=tblfeeaccount.Id inner join tbl_header_payment on tbl_header_payment.Id=tbl_fees_payment.Header_Id where tblfeestudent.StudId=" + Student_Id + " and tbl_header_payment.Transaction_Id='" + Transaction_Id + "' and tblfeeschedule.DueDate <= CURRENT_DATE() and tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion'";
            Ds_Fees = _mysqlObj.ExecuteQueryReturnDataSet(sql);
            Ds_Fees = CalculateFineAmountofFeesGroup(Ds_Fees);
            return Ds_Fees;

        }
        private DataSet CalculateFineAmountofFeesGroup(DataSet FineDs)
        {
            MysqlClass _mysqlObj = new MysqlClass(Connection);

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



        public bool ValidAmount(int _SchdId, double _BalanceAmount, int _StudentId)
        {
            bool _value = false;
            double Bal = 0;
            string sql = "select tblfeestudent.BalanceAmount from tblfeestudent where tblfeestudent.SchId=" + _SchdId + " and tblfeestudent.StudId=" + _StudentId + "";
            if (m_TransationDb != null)
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            else
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                double.TryParse(m_MyReader.GetValue(0).ToString(), out Bal);
                if (_BalanceAmount == Bal)
                    return true;
            }
            return _value;
        }
        public string GenBill(double _Total, int StudentId, string _PayMode, string _payId, string _bank, string _Date, int _UserId, int _BatchId, string _ClassId, string _StudentName, int _Regular, string _TempId, string _OtherReferance, string _BillTable)
        {
            string _billno = "0";

            if (m_BillPrefix == null)
            {
                m_BillPrefix = GetBillPrefix();
            }
            if (m_TransationDb != null)
            {
                General _GenObj = new General(m_TransationDb);
                int _FeeMaxId = _GenObj.GetTableMaxIdWithCondition("tblview_feebill", "Counter", _BatchId, "BatchId");
                int _FeeId = _GenObj.GetTableMaxId("tblview_feebill", "TransationId");

                DateTime _CreatedDate = System.DateTime.Now;
                string Year = GetStartYear(_BatchId);
                if (NeedOnlyNumber_InBill())
                {
                    _billno = _FeeMaxId.ToString();
                }
                else
                {
                    _billno = m_BillPrefix + Year.ToString() + "-" + _FeeMaxId.ToString();
                }
                if (_TempId != "")//Setting student id to zero for registered students
                    StudentId = 0;
                string sql = "insert into " + _BillTable + " (Id,StudentID,TotalAmount,Date,PaymentMode,PaymentModeId,BankName,UserId,CreatedDateTime,Counter,AccYear,ClassID,StudentName,BillNo,RegularFee,TempId,OtherReference) values(" + _FeeId + "," + StudentId + "," + _Total + ",'" + _CreatedDate.ToString("s") + "','" + _PayMode + "','" + _payId + "','" + _bank + "'," + _UserId + ",'" + _CreatedDate.ToString("s") + "'," + _FeeMaxId + "," + _BatchId + "," + _ClassId + ",'" + _StudentName + "','" + _billno + "'," + _Regular + ",'" + _TempId + "','" + _OtherReferance + "')";
                m_TransationDb.TransExecuteQuery(sql);


            }

            return _billno;
        }
        private string GetBillPrefix()
        {
            string _Prefix = "";
            string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name = 'BillPrefix'";
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                m_MyReader.Read();
                _Prefix = m_MyReader.GetValue(0).ToString();
            }
            return _Prefix;
        }
        private bool NeedOnlyNumber_InBill()
        {
            bool _valid = false;
            int _Prefix = 0;
            string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name = 'BillPrefix Only Numbers'";
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {

                int.TryParse(m_MyReader.GetValue(0).ToString(), out _Prefix);
                if (_Prefix == 1)
                {
                    _valid = true;
                }
            }
            return _valid;
        }
        private string GetStartYear(int _BatchId)
        {

            string Year = "2000";

            string sql = "select tblbatch.BatchName from tblbatch where Id=" + _BatchId;
            m_MyReader = m_TransationDb.SelectTansExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                string[] Array = m_MyReader.GetValue(0).ToString().Split('-');
                Year = Array[0];
            }
            m_MyReader.Close();
            return Year;
        }
        public bool ValidTransaction(int ScheduledFeeId, int StudentId, double Amount)
        {
            bool valid = false;

            if (m_TransationDb != null)
            {
                string sql = "select Id from tblfeestudent where SchId=" + ScheduledFeeId + " and StudId=" + StudentId + " and BalanceAmount=" + Amount;

                m_MyReader = m_TransationDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    valid = true;
                }
            }
            return valid;
        }
        public void FillTransaction(int _SfeeId, int _StudId, double _Amount, double _Deduction, double _Fine, double Total, double _Arrier, string _BillId, string _ClassId, string _StudentName, string _UserName, int _BatchId, string _FeeName, string _Period, int _CollectionType, int _RegularFee, string _TempId, string _TransactionTable, int _PeriodId, int _FeeId, string _Date)
        {
            {
                if (_TempId != "")//Setting student id to zero for registered students
                    _StudId = 0;
                DateTime _BillDate = General.GetDateFromText(_Date);
                General _GenObj = new General(m_TransationDb);
                int _TranMaxId = _GenObj.GetTableMaxId("tblview_transaction", "TransationId");
                if (_TransactionTable == "tbltransactionclearence")
                    _TranMaxId = _GenObj.GetTableMaxId("tbltransactionclearence", "TransationId");
                string sql = "INSERT INTO " + _TransactionTable + "(TransationId,PaymentElementId,UserId,Amount,AccountTo,AccountFrom,Type,PaidDate,BillNo,ClassId,StudentName,CollectedUser,BatchId,FeeName,PeriodName,CollectionType,RegularFee,TempId,PeriodId,FeeId) VALUES(" + _TranMaxId + "," + _SfeeId + "," + _StudId + "," + Total + ",1 , 2 ,'C','" + _BillDate.Date.ToString("s") + "','" + _BillId + "'," + _ClassId + ",'" + _StudentName + "','" + _UserName + "'," + _BatchId + ",'" + _FeeName + "','" + _Period + "','" + _CollectionType + "'," + _RegularFee + ",'" + _TempId + "'," + _PeriodId + "," + _FeeId + ")";

                m_TransationDb.TransExecuteQuery(sql);
                if (_SfeeId != -1)// -1 for other fee
                {
                    if (_Deduction > 0)
                    {
                        _TranMaxId = _GenObj.GetTableMaxId("tblview_transaction", "TransationId");
                        if (_TransactionTable == "tbltransactionclearence")
                            _TranMaxId = _GenObj.GetTableMaxId("tbltransactionclearence", "TransationId");
                        sql = "INSERT INTO " + _TransactionTable + "(TransationId,PaymentElementId,UserId,Amount,AccountTo,AccountFrom,Type,PaidDate,BillNo,ClassId,StudentName,CollectedUser,BatchId,FeeName,PeriodName,CollectionType,RegularFee,TempId,PeriodId,FeeId) VALUES(" + _TranMaxId + "," + _SfeeId + "," + _StudId + "," + _Deduction + ",3 , 2 ,'C','" + _BillDate.Date.ToString("s") + "','" + _BillId + "'," + _ClassId + ",'" + _StudentName + "','" + _UserName + "'," + _BatchId + ",'" + _FeeName + "','" + _Period + "','" + _CollectionType + "'," + _RegularFee + ",'" + _TempId + "'," + _PeriodId + "," + _FeeId + ")";

                        m_TransationDb.TransExecuteQuery(sql);

                    }
                    if (_Fine > 0)
                    {
                        _TranMaxId = _GenObj.GetTableMaxId("tblview_transaction", "TransationId");
                        if (_TransactionTable == "tbltransactionclearence")
                            _TranMaxId = _GenObj.GetTableMaxId("tbltransactionclearence", "TransationId");
                        sql = "INSERT INTO " + _TransactionTable + "(TransationId,PaymentElementId,UserId,Amount,AccountTo,AccountFrom,Type,PaidDate,BillNo,ClassId,StudentName,CollectedUser,BatchId,FeeName,PeriodName,CollectionType,RegularFee,TempId,PeriodId,FeeId) VALUES(" + _TranMaxId + "," + _SfeeId + "," + _StudId + "," + _Fine + ",4 , 2 ,'C','" + _BillDate.Date.ToString("s") + "','" + _BillId + "'," + _ClassId + ",'" + _StudentName + "','" + _UserName + "'," + _BatchId + ",'" + _FeeName + "','" + _Period + "','" + _CollectionType + "'," + _RegularFee + ",'" + _TempId + "'," + _PeriodId + "," + _FeeId + ")";
                        m_TransationDb.TransExecuteQuery(sql);

                    }
                    if (_Arrier > 0)
                    {
                        sql = "UPDATE tblfeestudent SET BalanceAmount = " + _Arrier + ", Status='Arrear' WHERE StudId=" + _StudId + " and SchId =" + _SfeeId;
                        m_TransationDb.TransExecuteQuery(sql);
                        sql = "UPDATE " + _TransactionTable + " SET " + _TransactionTable + ".BalanceAmount=" + _Arrier + " where " + _TransactionTable + ".BillNo='" + _BillId + "' AND " + _TransactionTable + ".TransationId=" + _TranMaxId;
                        m_TransationDb.TransExecuteQuery(sql);

                    }
                    else
                    {
                        sql = "UPDATE tblfeestudent SET BalanceAmount = " + _Arrier + ", Status='Paid' WHERE StudId=" + _StudId + " and SchId =" + _SfeeId;
                        m_TransationDb.TransExecuteQuery(sql);
                    }
                }

            }

        }
        private void StoreIncident(string _BillId, int StudentId, int Batch_Id, int Class_Id)
        {
            DateTime LastDate = new DateTime();

            int ScheduleId = 0;
            string FeeName = "";
            DataSet ds_shedule = new DataSet();
            string sql = "SELECT DISTINCT(tbltransaction.PaymentElementId) FROM tbltransaction WHERE tbltransaction.BillNo='" + _BillId + "'";
            ds_shedule = m_TransationDb.ExecuteQueryReturnDataSet(sql);
            General _GenObj = new General(m_TransationDb);
            if (ds_shedule != null && ds_shedule.Tables[0] != null && ds_shedule.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds_shedule.Tables[0].Rows)
                {

                    ScheduleId = 0;
                    FeeName = "";
                    LastDate = new DateTime();
                    int.TryParse(m_MyReader.GetValue(0).ToString(), out ScheduleId);
                    if (ScheduleId != -1)
                    {
                        LastDate = GetLastDateOfFeeSchedule(ScheduleId, out FeeName);
                        TimeSpan no_days;
                        if (LastDate.Date < DateTime.Now.Date)
                        {
                            no_days = DateTime.Now.Subtract(LastDate.Date);
                            Reportincident("PaidAfterDueDate", no_days.Days, no_days.Days, FeeName + "," + DateTime.Now, StudentId, "student", Class_Id, Batch_Id, 0, 0, "Online Payment" + StudentId + Class_Id + Batch_Id + ScheduleId);
                        }
                        else if (LastDate.Date > DateTime.Now.Date)
                        {
                            no_days = LastDate.Subtract(DateTime.Now.Date);
                            Reportincident("PaidBeforeDueDate", no_days.Days, no_days.Days, FeeName + "," + DateTime.Now.Date, StudentId, "student", Class_Id, Batch_Id, 0, 0, "Online Payment" + StudentId + Class_Id + Batch_Id + ScheduleId);
                        }
                    }

                }
            }
        }

        private DateTime GetLastDateOfFeeSchedule(int ScheduleId, out string FeeName)
        {
            FeeName = "";
            DateTime LastDate = new DateTime();
            string sql = "SELECT tblfeeschedule.LastDate,tblfeeaccount.AccountName FROM tblfeeschedule INNER JOIN tblfeeaccount ON tblfeeaccount.Id=tblfeeschedule.FeeId WHERE tblfeeschedule.Id=" + ScheduleId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                DateTime.TryParse(m_MyReader.GetValue(0).ToString(), out LastDate);
                FeeName = m_MyReader.GetValue(1).ToString();
            }
            return LastDate;
        }
        public void Reportincident(string IncidentCinfigType, int parameter1, int parameter2, string ValueDetails, int AssociatedUserId, string UserType, int ClassId, int BatchId, int UserId, int ActionId, string Reference)
        {
            bool NeedApproval = false;
            string CalculationType = "", Fixedvalue = "", IncedentDesc = "", Titlename = "";
            int TitleId = 0, LowerLimit = 0, UpperLimit = 0, TypeId = 0;
            double Point_Value = 0;
            if (IsincidentEnabled(IncidentCinfigType, ActionId, out CalculationType, out Fixedvalue, out IncedentDesc, out LowerLimit, out UpperLimit, out Point_Value, out TitleId, out Titlename, out NeedApproval, out TypeId))
            {
                double points = 0;
                if (ValueDetails == "Fixed")
                {
                    points = Math.Round(Point_Value, 0);
                }
                else
                {
                    points = CalculateincidentPoints(IncidentCinfigType, parameter1, parameter2, Point_Value);
                }
                int ApproverId = UserId;
                string Status = "Approved";
                if (IncidentCinfigType == "StudentAbsentEquation")
                {
                    IncedentDesc = IncedentDesc.Replace("($AbsentCount$)", parameter2.ToString());
                    IncedentDesc = IncedentDesc.Replace("($Days$)", ValueDetails);
                }
                if (IncidentCinfigType == "RankEquation")
                {
                    IncedentDesc = IncedentDesc.Replace("($Rank$)", parameter2.ToString());
                    IncedentDesc = IncedentDesc.Replace("($Subject:Marks$)", ValueDetails);
                }

                if (IncidentCinfigType == "PaidAfterDueDate")
                {
                    string[] str = ValueDetails.Split(',');
                    IncedentDesc = IncedentDesc.Replace("($NoofDate$)", parameter2.ToString());
                    IncedentDesc = IncedentDesc.Replace("($Date$)", str[1]);
                    IncedentDesc = IncedentDesc.Replace("($FeeName$)", str[0]);
                }
                if (IncidentCinfigType == "PaidBeforeDueDate")
                {
                    string[] str = ValueDetails.Split(',');
                    IncedentDesc = IncedentDesc.Replace("($NoofDate$)", parameter2.ToString());
                    IncedentDesc = IncedentDesc.Replace("($Date$)", str[1]);
                    IncedentDesc = IncedentDesc.Replace("($FeeName$)", str[0]);
                }

                if (NeedApproval)
                {
                    ApproverId = 0;
                    Status = "Need Approval";
                }
                DeleteExistingIncedent(Reference);
                InsertIncident(Titlename, IncedentDesc, DateTime.Now, DateTime.Now, TypeId, ApproverId, UserId, UserType, Status, AssociatedUserId, TitleId, points, BatchId, ClassId, Reference);
            }


        }
        private bool IsincidentEnabled(string IncidentCinfigType, int ActionId, out string CalculationType, out string Fixedvalue, out string IncedentDesc, out int LowerLimit, out int UpperLimit, out double Point_Value, out int TitleId, out string Titlename, out bool NeedApproval, out int TypeId)
        {
            bool Isenable = false, Done = false;
            NeedApproval = false;
            string IsEnable = "";
            int Isactive = 0;
            CalculationType = ""; Fixedvalue = ""; IncedentDesc = ""; Titlename = "";
            TitleId = 0; LowerLimit = 0; UpperLimit = 0; Point_Value = 0; TypeId = 0;
            string sql = "SELECT tblincedenthead.Title,tblincedenthead.NeedApproval,tblincedenthead.IsActive,tblincedentconfig.CalculationType,tblincedentconfig.FixedValue,tblincedentconfig.LowerLimit,tblincedentconfig.UpperLimit,tblincedentconfig.TitleId,tblincedentconfig.IncedentDesc,tblincedentconfig.Point_Value,tblincedentconfig.IsEnable,tblincedenthead.TypeId FROM tblincedentconfig INNER JOIN tblincedenthead ON tblincedenthead.Id=tblincedentconfig.TitleId WHERE tblincedentconfig.`Type`='" + IncidentCinfigType + "' AND tblincedentconfig.ActionId=" + ActionId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                Titlename = m_MyReader.GetValue(0).ToString();
                if (m_MyReader.GetValue(1).ToString().ToUpperInvariant() == "YES")
                {
                    NeedApproval = true;
                }
                int.TryParse(m_MyReader.GetValue(2).ToString(), out Isactive);
                CalculationType = m_MyReader.GetValue(3).ToString();
                Fixedvalue = m_MyReader.GetValue(4).ToString();
                int.TryParse(m_MyReader.GetValue(5).ToString(), out LowerLimit);
                int.TryParse(m_MyReader.GetValue(6).ToString(), out UpperLimit);
                int.TryParse(m_MyReader.GetValue(7).ToString(), out TitleId);
                IncedentDesc = m_MyReader.GetValue(8).ToString();
                double.TryParse(m_MyReader.GetValue(9).ToString(), out Point_Value);
                IsEnable = m_MyReader.GetValue(10).ToString();
                if (int.TryParse(m_MyReader.GetValue(11).ToString(), out TypeId))
                {
                    if (TypeId > 0)
                    {
                        Done = true;
                    }
                }
                if (IsEnable.ToUpperInvariant() == "YES" && Isactive == 1)
                {
                    Isenable = true;
                }

            }
            if (!Done)
            {
                if (ActionId != 0)
                {
                    sql = "SELECT tblincedenthead.Title,tblincedenthead.NeedApproval,tblincedenthead.IsActive,tblincedentconfig.CalculationType,tblincedentconfig.FixedValue,tblincedentconfig.LowerLimit,tblincedentconfig.UpperLimit,tblincedentconfig.TitleId,tblincedentconfig.IncedentDesc,tblincedentconfig.Point_Value,tblincedentconfig.IsEnable,tblincedenthead.TypeId FROM tblincedentconfig INNER JOIN tblincedenthead ON tblincedenthead.Id=tblincedentconfig.TitleId WHERE tblincedentconfig.`Type`='" + IncidentCinfigType + "' AND tblincedentconfig.ActionId=0";
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                    if (m_MyReader.HasRows)
                    {
                        Titlename = m_MyReader.GetValue(0).ToString();
                        if (m_MyReader.GetValue(1).ToString().ToUpperInvariant() == "YES")
                        {
                            NeedApproval = true;
                        }
                        int.TryParse(m_MyReader.GetValue(2).ToString(), out Isactive);
                        CalculationType = m_MyReader.GetValue(3).ToString();
                        Fixedvalue = m_MyReader.GetValue(4).ToString();
                        int.TryParse(m_MyReader.GetValue(5).ToString(), out LowerLimit);
                        int.TryParse(m_MyReader.GetValue(6).ToString(), out UpperLimit);
                        int.TryParse(m_MyReader.GetValue(7).ToString(), out TitleId);
                        IncedentDesc = m_MyReader.GetValue(8).ToString();
                        double.TryParse(m_MyReader.GetValue(9).ToString(), out Point_Value);
                        IsEnable = m_MyReader.GetValue(10).ToString();
                        int.TryParse(m_MyReader.GetValue(11).ToString(), out TypeId);
                        if (IsEnable.ToUpperInvariant() == "YES" && Isactive == 1)
                        {
                            Isenable = true;
                        }

                    }
                }
            }
            return Isenable;
        }
        private double CalculateincidentPoints(string IncidentCinfigType, int parameter1, int parameter2, double Point_Value)
        {
            double points = 0;

            if (IncidentCinfigType == "StudentAbsentEquation")
            {
                points = (parameter1 * Point_Value) * -1;
            }
            if (IncidentCinfigType == "RankEquation")
            {
                points = ((parameter1 - parameter2) * Point_Value);
            }

            if (IncidentCinfigType == "PaidAfterDueDate")
            {
                points = (parameter1 * Point_Value) * -1;
            }

            if (IncidentCinfigType == "PaidBeforeDueDate")
            {
                points = (parameter1 * Point_Value);
            }

            points = Math.Round(points, 0);
            return points;
        }
        private void DeleteExistingIncedent(string Reference)
        {
            string sql = " DELETE FROM tblincedent WHERE tblincedent.Reference='" + Reference + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            m_MyReader.Close();
        }

        private void InsertIncident(string Titlename, string Description, DateTime IncedentDate, DateTime CreatedDate, int TypeId, int ApproverId, int UserId, string UserType, string Status, int AssociatedUserId, int HeadId, double points, int BatchId, int ClassId, string Reference)
        {
            General _GenObj = new General(m_MysqlDb);
            int _Incedentid = _GenObj.GetTableMaxId("tblview_incident", "Id");

            string sql = "insert into tblincedent (Id,Title,Description,IncedentDate,CreatedDate,TypeId,ApproverId,CreatedUserId,UserType,Status,AssoUser,HeadId,Point,BatchId,ClassId,Reference)values (" + _Incedentid + ",'" + Titlename + "','" + Description + "','" + IncedentDate.ToString("s") + "','" + CreatedDate.ToString("s") + "'," + TypeId + "," + ApproverId + "," + UserId + ",'" + UserType.ToLowerInvariant() + "','" + Status + "'," + AssociatedUserId + "," + HeadId + "," + points + "," + BatchId + "," + ClassId + ",'" + Reference + "')";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            m_MyReader.Close();
        }


        //show paid fees


        private void LoadPaidFees(int Hp_Id)
        {
            MysqlClass _mysqlObj = new MysqlClass(Connection);
            //ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            string sql;

            sql = "select tblfeeaccount.AccountName as `Fee_Name`,tblbatch.BatchName,tbl_fees_payment.Period,tbl_fees_payment.Fine,tbl_fees_payment.Amount,'Paid' as Status  from tbl_fees_payment  inner join tblfeeaccount on tblfeeaccount.Id = tbl_fees_payment.Fees_Id  inner join tblbatch on tblbatch.Id=tbl_fees_payment.Batch where tbl_fees_payment.Header_Id=" + Hp_Id + "";

            MydataSet = _mysqlObj.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                Grd_Feetopay.DataSource = MydataSet;
                Grd_Feetopay.DataBind();
            }
            else
            {
                Grd_Feetopay.DataSource = null;
                Grd_Feetopay.DataBind();

            }

        }



        protected void Btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("FeeDetails.aspx");
        }


    }

}
