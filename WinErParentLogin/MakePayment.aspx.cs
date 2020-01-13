using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;
using System.Security.Cryptography;
using System.Data;
using WinBase;
using System.Data.Odbc;
using UpService;
using System.Text.RegularExpressions;

namespace WinErParentLogin
{
    public partial class MakePayment : System.Web.UI.Page
    {
        public string action1 = string.Empty;
        public string hash1 = string.Empty;
        public string txnid1 = string.Empty;
        private ParentInfoClass MyParentInfo;
        private OdbcDataReader MyReader = null;
        private static string Group_Head_Id=string.Empty;
        private static string Group_Head_Name = string.Empty;
        private static string Group_Head_Amount = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                MyParentInfo = (ParentInfoClass)Session["MyParentObj"];
                MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
                if (MyParentInfo == null)
                {
                    Response.Redirect("sectionerr.htm");
                }
                if (!IsPostBack)
                {
                    // Fetching previous page values
                    if (PreviousPage.Fees_Group_Id != null && PreviousPage.Fees_Group_Header != null && PreviousPage.Fees_Group_Header_Total != null)
                    {
                        amount.Text = PreviousPage.Fees_Group_Header_Total.ToString();
                        Hide_Amount.Text = PreviousPage.Fees_Group_Header_Total.ToString();
                        Group_Head_Amount = PreviousPage.Fees_Group_Header_Total;
                        Group_Head_Name = PreviousPage.Fees_Group_Header;
                        Group_Head_Id = PreviousPage.Fees_Group_Id;
                        lbl_Group_Name.Text = PreviousPage.Fees_Group_Header.ToString();
                        productinfo.Text = PreviousPage.Fees_Group_Header.ToString() + " payment";
                        Hide_ProductInfo.Text = PreviousPage.Fees_Group_Header.ToString() + " payment";
                        // Assign Default payment details
                        Assign_Default();
                        if (string.IsNullOrEmpty(Request.Form["hash"]))
                        {
                            submit.Visible = true;
                        }
                        else
                        {
                            submit.Visible = false;
                        }


                    }
                }

            }
            catch (Exception ex)
            {
                lbl_msg.Text = ex.Message;

            }

        }
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
        private void Assign_Default()
        {
          string Email = "";
          DataSet Ds_Default = new DataSet();
          MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
          ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
          string sql;
          sql = "select Email from tblview_student where Id=" + MyParentInfo.StudentId + "";
          Ds_Default = MyParent.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
          if (Ds_Default != null && Ds_Default.Tables[0] != null && Ds_Default.Tables[0].Rows.Count > 0)
          {
              foreach (DataRow dr in Ds_Default.Tables[0].Rows)
              {
                  Email = dr["Email"].ToString();
                  
              }
          }
          email.Text = Email;
          firstname.Text = MyParentInfo.ParentName.ToString();

        }

        private DataSet Get_Gateway_Configurations(int Group_Id)
        {
            DataSet Ds_GateWay = new DataSet();
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            string sql;
            sql = "select * from tbl_feesgatewayconfig inner join tbl_feesgrouphead on tbl_feesgatewayconfig.Id=tbl_feesgrouphead.GateWay_Id where tbl_feesgrouphead.Id=" + Group_Id + "";
            Ds_GateWay = MyParent.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return Ds_GateWay;

        }
        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("FeeDetails.aspx");
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            string Msg="";
            try
            {
                MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
                DBLogClass _DblogObj = new DBLogClass(_mysqlObj);
                if (Validate_Payment(out Msg))
                {
  #region Assign_Payment_Configurations
                    string Hash_Sequence = string.Empty;
                    string Merchant_Key = string.Empty;
                    string Salt = string.Empty;
                    string Base_Url = string.Empty;
                    string Success_Url = string.Empty;
                    string Failure_Url = string.Empty;
                    string Service_Provider = string.Empty;
                    DataSet Ds_Gateway = new DataSet();
                    Ds_Gateway = Get_Gateway_Configurations(int.Parse(Group_Head_Id.ToString()));
                    if (Ds_Gateway != null && Ds_Gateway.Tables[0] != null && Ds_Gateway.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in Ds_Gateway.Tables[0].Rows)
                        {
                            Hash_Sequence = dr["hashSequence"].ToString();
                            Merchant_Key = dr["MERCHANT_KEY"].ToString();
                            key.Value = dr["MERCHANT_KEY"].ToString();
                            Salt = dr["SALT"].ToString();
                            Base_Url = dr["BASE_URL"].ToString();
                            Success_Url = dr["Success_url"].ToString();
                            Failure_Url = dr["Failure_Url"].ToString();
                            Service_Provider = dr["Service_Provider"].ToString();

                        }
                    }
   #endregion

                    string[] hashVarsSeq;
                    string hash_string = string.Empty;
                    string Transaction_No = string.Empty;
                    Transaction_No = Get_Max_Transaction_No();
                    txnid1 = Transaction_No;

                    if (string.IsNullOrEmpty(Request.Form["hash"])) // generating hash value
                    {
                       
                        frmError.Visible = false;
                        hashVarsSeq = Hash_Sequence.Split('|'); // spliting hash sequence from config
                        hash_string = "";
                        foreach (string hash_var in hashVarsSeq)
                        {
                            if (hash_var == "key")
                            {
                                hash_string = hash_string + Merchant_Key;
                                hash_string = hash_string + '|';
                            }
                            else if (hash_var == "txnid")
                            {
                                hash_string = hash_string + txnid1;
                                hash_string = hash_string + '|';
                            }
                            else if (hash_var == "amount")
                            {
                                hash_string = hash_string + Convert.ToDecimal(Request.Form[hash_var]).ToString("g29");
                                hash_string = hash_string + '|';
                            }
                            else
                            {

                                hash_string = hash_string + (Request.Form[hash_var] != null ? Request.Form[hash_var] : "");// isset if else
                                hash_string = hash_string + '|';
                            }
                        }

                        hash_string += Salt;// appending SALT

                        hash1 = Generatehash512(hash_string).ToLower();         //generating hash
                        action1 = Base_Url + "/_payment";// setting URL

                    }
                    else if (!string.IsNullOrEmpty(Request.Form["hash"]))
                    {
                        hash1 = Request.Form["hash"];
                        action1 = Base_Url + "/_payment";

                    }
                    if (!string.IsNullOrEmpty(hash1))
                    {
                        hash.Value = hash1;
                        txnid.Value = txnid1;

                        System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in gash table for data post
                        data.Add("hash", hash.Value);
                        data.Add("txnid", txnid.Value);
                        data.Add("key", key.Value);
                        string AmountForm = Convert.ToDecimal(amount.Text.Trim()).ToString("g29");// eliminating trailing zeros
                        amount.Text = AmountForm;
                        data.Add("amount", AmountForm);
                        data.Add("firstname", firstname.Text.Trim());
                        data.Add("email", email.Text.Trim());
                        data.Add("phone", phone.Text.Trim());
                        data.Add("productinfo", productinfo.Text.Trim());
                        data.Add("surl", Success_Url);
                        data.Add("furl", Failure_Url);
                        data.Add("lastname", lastname.Text.Trim());
                        data.Add("curl", curl.Text.Trim());
                        data.Add("address1", address1.Text.Trim());
                        data.Add("address2", address2.Text.Trim());
                        data.Add("city", city.Text.Trim());
                        data.Add("state", state.Text.Trim());
                        data.Add("country", country.Text.Trim());
                        data.Add("zipcode", zipcode.Text.Trim());
                        data.Add("udf1", udf1.Text.Trim());
                        data.Add("udf2", udf2.Text.Trim());
                        data.Add("udf3", udf3.Text.Trim());
                        data.Add("udf4", udf4.Text.Trim());
                        data.Add("udf5", udf5.Text.Trim());
                        data.Add("pg", pg.Text.Trim());
                        data.Add("service_provider", Service_Provider);

                        Save_Transaction_Details(int.Parse(Group_Head_Id.ToString()));
                        string strForm = PreparePOSTForm(action1, data);
                        Page.Controls.Add(new LiteralControl(strForm));
                        // Add Log to Database abount Transaction Started
                        string action = "Online Payment Started:student Id=" + MyParentInfo.StudentId + " ,Fees Header Id=" + Group_Head_Id.ToString() + ",Amout=" + Group_Head_Amount.ToString() + ",Transaction Id=" + txnid + ",Actin Date=" + DateTime.Now.ToString() + "";
                        _DblogObj.LogToDb(MyParentInfo.ParentName, "ParentLogin_OnlinePayment",action, 4, 2);

                    }

                    else
                    {
                        lbl_msg.Text = "Please Try Again";

                    }
                }
                else
                {
                    lbl_msg.Text = Msg;
                }

            }

            catch (Exception ex)
            {
                lbl_msg.Text = ex.Message;

            }
        }
        #region Payment_Validations
        private bool Validate_Payment(out string msg)
        {
            bool _valid = true;
            msg = "";
            string pattern = null;
            pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            long Phone_No = 0;
            if (phone.Text != "")
            {
                long.TryParse(phone.Text, out Phone_No);
            }
            if (email.Text == "")
            {
                _valid = false;
                msg = "Please enter Email";
            }
            // check email pattern
            else if (!Regex.IsMatch(email.Text, pattern))
            {
                _valid = false;
                msg = "Email Id is not valid";
            }
            else if (firstname.Text == "")
            {
                _valid = false;
                msg = "Please enter Name";
            }
            else if (phone.Text == "")
            {
                _valid = false;
                msg = "Please enter Phone No";
            }
            // Check phone No valid or not
            else if(phone.Text.Length>10 || phone.Text.Length<10 || Phone_No==0)
            {
                 _valid = false;
                msg = "Phone No is not valid";
            }
            // varify paying amount
            else if (Convert.ToDecimal(Group_Head_Amount.ToString()) != Convert.ToDecimal(amount.Text.Trim()))
            {
                _valid = false;
                msg = "Please Try Again";
            }
            return _valid;
        }
        #endregion
        #region Generate Post Form
        private string PreparePOSTForm(string url, System.Collections.Hashtable data)      // post form
        {
            //Set a name for the form
            string formID = "PostForm";
            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" +
                           formID + "\" action=\"" + url +
                           "\" method=\"POST\">");

            foreach (System.Collections.DictionaryEntry key in data)
            {

                strForm.Append("<input type=\"hidden\" name=\"" + key.Key +
                               "\" value=\"" + key.Value + "\">");
            }


            strForm.Append("</form>");
            //Build the JavaScript which will do the Posting operation.
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var v" + formID + " = document." +
                             formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");
            //Return the form and the script concatenated.
            //(The order is important, Form then JavaScript)
            return strForm.ToString() + strScript.ToString();
        }
        #endregion
        // save Transaction details in database
        private void Save_Transaction_Details(int Header_Id)
        {
            ;
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            string sql;
            DateTime Action_Dt=DateTime.Now;
            sql = "Insert into tbl_header_payment(Header_Id,Header_Name,Student_Id,Class_Id,Parent_Name,Email,Phone_No,Amount,Transaction_Id,Action_Date,Status) values(" + int.Parse(Group_Head_Id.ToString()) + ",'" + Group_Head_Name.ToString() + "'," + MyParentInfo.StudentId + "," + MyParentInfo.CLASSID + ",'" + firstname.Text.Trim() + "','" + email.Text.Trim() + "'," + phone.Text.Trim() + "," + amount.Text.Trim() + ",'" + txnid1 + "','" + Action_Dt.ToString("yyyy-MM-dd HH:mm:ss") + "','"+CommonEnum.OnlineTransactionStatus.PaymentStarted.ToString()+"')";
            MyParent.m_MysqlDb.ExecuteQuery(sql);
            DataSet Ds_Header = new DataSet();
            int Head_Id = 0;
            int Header_Payment_Id = 0;
            sql = "select Id,Header_Id from tbl_header_payment where Transaction_Id='" + txnid1 + "' and Action_Date='" + Action_Dt.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            Ds_Header = MyParent.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if(Ds_Header!=null && Ds_Header.Tables[0]!=null && Ds_Header.Tables[0].Rows.Count>0)
            {
                foreach (DataRow dr in Ds_Header.Tables[0].Rows)
                {
                    Head_Id = int.Parse(dr["Header_Id"].ToString());
                    Header_Payment_Id = int.Parse(dr["Id"].ToString());
                }
            }
            sql = "select tblfeeaccount.AccountName as `Fee Name`,tblbatch.BatchName, tblperiod.Period, tblfeestudent.Status,tblfeestudent.BalanceAmount,date_format( tblfeeschedule.LastDate , '%d-%m-%Y') AS 'LastDate', tblfeestudent.SchId, tblfeeschedule.PeriodId, tblfeeschedule.FeeId,tblfeestudent.Id as FeeStudentid, tblfeeschedule.Duedate, tblfeeschedule.BatchId from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId where tblfeestudent.StudId=" + MyParentInfo.StudentId + "  and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion' and tblfeeaccount.Header_Id=" + Head_Id + " and tblfeeschedule.DueDate <= CURRENT_DATE()";
            DataSet Ds_Fees = new DataSet();
            Ds_Fees = MyParent.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            Ds_Fees = CalculateFineAmountofFeesGroup(Ds_Fees);
            if (Ds_Fees != null && Ds_Fees.Tables != null && Ds_Fees.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Ds_Fees.Tables[0].Rows)
                {
                    sql = "Insert into tbl_fees_payment(Header_Id,Fees_Id,Amount,Period,Batch,Fine) values(" + Header_Payment_Id + "," + dr["FeeId"].ToString() + "," + dr["BalanceAmount"].ToString() + ",'" + dr["Period"].ToString() + "'," + dr["BatchId"].ToString() + "," + dr["Fine"].ToString() + ")";
                    MyParent.m_MysqlDb.ExecuteQuery(sql);
                }
            }
        }
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

        // Generate Online Payment Transaction No
        private string Get_Max_Transaction_No()
        {
            string Transaction_No = "";
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            string sql = "";
            sql = "select max(Id)+1 from tbl_header_payment";
            MyReader = _mysqlObj.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    Transaction_No = "OP"+MyParentInfo.SchoolObject.SchoolId.ToString()+"-"+MyReader.GetValue(0).ToString();
                }
            }
            return Transaction_No;
        }
       

    }
}
