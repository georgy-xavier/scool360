using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using System.Data;
using UpService;

namespace WinErParentLogin
{
    public partial class PaymentFailure : System.Web.UI.Page
    {
        public MysqlClass m_MysqlDb;
        private SchoolClass objSchool = null;
        private DataSet MydataSet = null;
        private int HP_Id = 0;
        private string Connection = "";
        private string Payer_Name = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string Bank_Reference_No = "";
            string Payment_Mode = "";
            int School_Id = 0;
            string Transaction_No = string.Empty;
            try
            {

                MysqlClass _mysqlObj = new MysqlClass(Connection);
                string[] merc_hash_vars_seq;
                string merc_hash_string = string.Empty;
                string merc_hash = string.Empty;
                string order_id = string.Empty;
                string hash_seq = "";
                string Status = "";

                if (Request.Form["status"] != null)
                {
                    Status = Request.Form["status"];
                }
                if (Status.ToLower() == "failure")
                {
                    string Salt = "";
                    string action = "";
                    int Header_Id = 0;
                    double Amout_Total = 0;
                    int Student_Id = 0;
                    int Class_Id = 0;
                    int Batch_Id = 0;
                    Transaction_No = Request.Form["txnid"];
                    // Get School Id from Transaction No
                    School_Id = Get_Id_From_TransactionNo(Transaction_No);
                    // creating database object based on School Id
                    if (WinerUtlity.NeedCentrelDB())
                    {
                        objSchool = WinerUtlity.GetSchoolObject(School_Id);
                        Connection = WinerUtlity.GetConnectionString(objSchool);
                        m_MysqlDb = new MysqlClass(Connection);
                    }
                    //
                    Lbl_TransName.Visible = true;
                    LblTransref.Visible = true;
                    lblTransactionId.Text = Request.Form["txnid"].ToString();
                    Bank_Reference_No = Request.Form["bank_ref_num"].ToString();
                    Payment_Mode = Get_Mode_Of_Payment(Request.Form["mode"].ToString());
                    Payer_Name = Request.Form["firstname"].ToString();
                    // Change the Status of Payment Based on Transaction_No
                    Online_Payment_Failure(Transaction_No, Bank_Reference_No, Payment_Mode, CommonEnum.OnlineTransactionStatus.PaymentFailure.ToString());
                    // Save form in database
                    string Form = "";
                    if (Request.Form != null)
                    {
                        Form = Request.Form.ToString();
                    }
                    Save_Form_To_Database(Transaction_No, Form);
                    Get_Header_Payment_Details(Transaction_No, out HP_Id, out Header_Id, out Amout_Total, out Student_Id, out Class_Id, out Batch_Id);
                    DataSet Ds_Gateway = new DataSet();
                    Ds_Gateway = Get_Gateway_Configurations(int.Parse(Session["Fees_Group_Id"].ToString()));
                    if (Ds_Gateway != null && Ds_Gateway.Tables[0] != null && Ds_Gateway.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in Ds_Gateway.Tables[0].Rows)
                        {
                            Salt = dr["SALT"].ToString();
                            hash_seq = dr["hashSequence"].ToString();
                        }
                    }

                    merc_hash_vars_seq = hash_seq.Split('|');
                    Array.Reverse(merc_hash_vars_seq);
                    merc_hash_string = Salt + "|" + Request.Form["status"];


                    foreach (string merc_hash_var in merc_hash_vars_seq)
                    {
                        merc_hash_string += "|";
                        merc_hash_string = merc_hash_string + (Request.Form[merc_hash_var] != null ? Request.Form[merc_hash_var] : "");

                    }

                    merc_hash = Generatehash512(merc_hash_string).ToLower();

                    if (merc_hash != Request.Form["hash"])
                    {
                        Online_Payment_Failure(Transaction_No, Bank_Reference_No, Payment_Mode, CommonEnum.OnlineTransactionStatus.PaymentFailureButHashNotMatched.ToString());

                    }
                    else
                    {
                        //Payment Failure
                        action = "Online Payment Failure:student Id=" + Student_Id + ",Amout=" + Amout_Total + ",Transaction Id=" + Transaction_No + ",Actin Date=" + DateTime.Now.ToString() + "";
                        DBLogClass _DblogObj = new DBLogClass(_mysqlObj);
                        _DblogObj.LogToDb(Payer_Name, "ParentLogin_OnlinePayment", action, 4, 2);


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
                Response.Write("<span style='color:red'>" + ex.Message + "</span>");

            }
        }
        // Get school Id from Transaction No
        private int Get_Id_From_TransactionNo(string Transaction_No)
        {
            int School_Id = 0;
            string[] split = Transaction_No.Split('P', '-');
            School_Id = int.Parse(split[1].ToString());
            return School_Id;
        }
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
                    int.TryParse(dr["Batch_Id"].ToString(), out Batch_Id);
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
        private void Online_Payment_Failure(string Transaction_No, string Bank_Reference_No, string Payment_Mode, string Status)
        {

            MysqlClass _mysqlObj = new MysqlClass(Connection);
            string sql = "";
            sql = "update tbl_header_payment set Status='" + Status + "',Payment_Mode='" + Payment_Mode + "',Bank_RefNo='" + Bank_Reference_No + "' where Transaction_Id='" + Transaction_No + "'";
            _mysqlObj.ExecuteQuery(sql);
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
        // click event for back to parent login portal
        protected void Btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("FeeDetails.aspx");
        }
    }
}
