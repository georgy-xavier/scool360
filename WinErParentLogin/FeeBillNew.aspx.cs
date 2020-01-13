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
using System;
using WinBase;

namespace WinEr
{
    public partial class FeeBillNew : System.Web.UI.Page
    {
        private MysqlClass m_MysqlDb;
        private SchoolClass objSchool = null;
        private MysqlClass m_TransationDb = null;
        private OdbcDataReader m_MyReader = null;
        private static string BillNo = string.Empty;
        private static string Transaction_No = string.Empty;
        private static string Connection = string.Empty;
        private static int Class_Id;
        protected void Page_init(object sender, EventArgs e)
        {
            int School_Id = 0;
            if (!IsPostBack)
            {

                // Fetching previous page values
                if (Request.QueryString["BillNo"] != null && Request.QueryString["Trans_No"] != null)
                {
                    BillNo = Request.QueryString["BillNo"];
                    Transaction_No = Request.QueryString["Trans_No"];
                    School_Id = Get_Id_From_TransactionNo(Transaction_No);
                    if (WinerUtlity.NeedCentrelDB())
                    {
                        objSchool = WinerUtlity.GetSchoolObject(School_Id);
                        Connection = WinerUtlity.GetConnectionString(objSchool);
                        m_MysqlDb = new MysqlClass(Connection);
                        m_TransationDb = m_MysqlDb;
                      
                    }
                    string BillType = "0";
                    Get_Header_Payment_Details(Transaction_No, out Class_Id);
                    Checkpaymentmode(BillNo, BillType);
                    LoadLogo();
                    LoadStudent(BillNo, BillType);
                    LoadFeeDetails(BillNo, BillType);
                    LoadTotal(BillNo, BillType);
                    LoadSchoolDetails();
                }
                MysqlClass _mysqlObj = new MysqlClass(Connection);
                m_TransationDb = _mysqlObj;
                m_MysqlDb = _mysqlObj;
            }
        }
        private int Get_Id_From_TransactionNo(string Transaction_No)
        {
            int School_Id = 0;
            string[] split = Transaction_No.Split('P', '-');
            School_Id = int.Parse(split[1].ToString());
            return School_Id;
        }

        private void Checkpaymentmode(string BillNo, string BillType)
        {
            MysqlClass _mysqlObj = new MysqlClass(Connection);
            string Paymode = "";
            string sql = "SELECT PaymentMode FROM tblview_feebill WHERE BillNo='" + BillNo + "'";

            m_MyReader = _mysqlObj.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                Paymode = m_MyReader.GetValue(0).ToString();
            }
            if (Paymode != "Cash")
            {

                Lbl_OPNO.Text = Transaction_No;
            }
        }

        private void LoadTotal(string BillNo, string BillType)
        {
            string sql = "";
            MysqlClass _mysqlObj = new MysqlClass(Connection);
            sql = "select tblview_feebill.TotalAmount as Total ,date_format(tblview_feebill.Date,'%d-%m-%Y') AS 'PaidDate' from tblview_feebill where tblview_feebill.BillNo='" + BillNo + "'";

            m_MyReader = _mysqlObj.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                LblAmt.Text = m_MyReader.GetValue(0).ToString();
                lblpDate.Text = m_MyReader.GetValue(1).ToString();
                lbl_amountWorlds.Text = Convert_Number_To_Words(long.Parse(LblAmt.Text));
            }
        }

        public string Convert_Number_To_Words(long _InpNo)
        {
            long _temp = _InpNo;
            if (_InpNo < 0)
            {
                _InpNo = _InpNo * -1;
            }

            long r = 0, i = 0;
            string Words = "";


            string[] a = { " One ", " Two ", " Three ", " Four ", " Five ", " Six ", " Seven ", " Eight ", " Nine ", " Ten " };

            string[] b = { " Eleven ", " Twelve ", " Thirteen ", " Fourteen ", " Fifteen ", " Sixteen ", " Seventeen ", " Eighteen ", " Nineteen " };

            string[] c = { "Ten", " Twenty ", " Thirty ", " Fourty ", " Fifty ", " Sixty ", " Seventy ", " Eighty ", " Ninety ", " Hundred " };
            try
            {
                if (_InpNo > 9999999)
                {

                    r = _InpNo / 10000000;
                    if (r > 100)
                    {
                        Words = Convert_Number_To_Words(r);
                        Words = Words + "Crore";
                    }
                    else if (r == 10 || r == 20 || r == 30 || r == 40 || r == 50 || r == 60 || r == 70 || r == 80 || r == 90 || r == 100)
                    {

                        r = r / 10;

                        Words = Words + c[r - 1] + " Crore ";


                    }
                    else if (r > 0 && r < 10)
                    {

                        Words += a[r - 1] + " Crore ";


                    }
                    else if (r > 10 && r < 20)
                    {
                        r = r % 10;

                        Words = b[r - 1] + " Crore ";


                    }
                    else
                    {

                        i = r / 10;

                        r = r % 10;

                        Words = Words + c[i - 1] + a[r - 1] + " Crore ";

                    }

                    _InpNo = _InpNo % 10000000;
                }
                if (_InpNo > 99999)
                {

                    r = _InpNo / 100000;
                    if (r == 10 || r == 20 || r == 30 || r == 40 || r == 50 || r == 60 || r == 70 || r == 80 || r == 90 || r == 100)
                    {

                        r = r / 10;

                        Words = Words + c[r - 1] + " Lakh ";


                    }
                    else if (r > 0 && r < 10)
                    {

                        Words += a[r - 1] + " Lakh ";


                    }
                    else if (r > 10 && r < 20)
                    {
                        r = r % 10;

                        Words = b[r - 1] + " Lakh ";


                    }
                    else
                    {

                        i = r / 10;

                        r = r % 10;

                        Words = Words + c[i - 1] + a[r - 1] + " Lakh ";

                    }

                    _InpNo = _InpNo % 100000;
                }
                if (_InpNo > 9999)
                {

                    r = _InpNo / 1000;
                    if (r == 10 || r == 20 || r == 30 || r == 40 || r == 50 || r == 60 || r == 70 || r == 80 || r == 90 || r == 100)
                    {

                        r = r / 10;

                        Words = Words + c[r - 1] + " Thousand ";


                    }

                    else if (r > 10 && r < 20)
                    {
                        r = r % 10;

                        Words = Words + b[r - 1] + "Thousand ";


                    }

                    else
                    {

                        i = r / 10;

                        r = r % 10;

                        Words = Words + c[i - 1] + a[r - 1] + " Thousand ";

                    }

                    _InpNo = _InpNo % 1000;

                }

                if (_InpNo > 999)
                {
                    if (_InpNo == 1000)
                    {
                        Words += " Thousand ";
                        _InpNo = 0;
                    }
                    else
                    {
                        r = _InpNo / 1000;

                        Words += a[r - 1] + " Thousand ";



                        _InpNo = _InpNo % 1000;
                    }
                }

                if (_InpNo > 99)
                {
                    if (_InpNo == 100)
                    {
                        Words += "One Hundred ";
                        _InpNo = 0;
                    }
                    else
                    {
                        r = _InpNo / 100;

                        Words += a[r - 1] + " Hundred ";

                        _InpNo = _InpNo % 100;
                    }

                }

                if (_InpNo > 10 && _InpNo < 20)
                {

                    r = _InpNo % 10;
                    if (Words == "")
                        Words += b[r - 1];
                    else
                        Words += " And " + b[r - 1];
                }

                if (_InpNo > 19 && _InpNo <= 100)
                {

                    r = _InpNo / 10;

                    i = _InpNo % 10;
                    //i=r;
                    if (Words == "")
                    {
                        if (i != 0)
                            Words += c[r - 1] + a[i - 1];
                        else
                            Words += c[r - 1];
                    }
                    else
                    {
                        if (i != 0)
                            Words += " And " + c[r - 1] + a[i - 1];
                        else
                            Words += " And " + c[r - 1];
                    }
                }

                if (_InpNo > 0 && _InpNo <= 10)
                {
                    if (Words == "")
                        Words += a[_InpNo - 1];
                    else
                        Words += " And " + a[_InpNo - 1];

                }


                if (_temp == 0)
                {
                    Words = "Zero";
                }
                else if (_temp < 0)
                {
                    Words = "(-ve) " + Words;
                }


                return Words + " Only";
            }
            catch (Exception)
            {
                return "------------------------------------------------------";
            }

        }

        private void LoadFeeDetails(string BillNo, string BillType)
        {
            string table = "";
            DataSet data_Fee;
            string _Period = "";
            string _FeeName = "";
            double _total = 0;
            string _Batch = "";
            string sql = "";
            MysqlClass _mysqlObj = new MysqlClass(Connection);
            //sql = "SELECT  (select tblperiod.Period from tblperiod where tblperiod.Id= tblview_transaction.PeriodId) as Period,tblfeeaccount.AccountName AS 'Fee Name',tblaccount.AccountName,tblview_transaction.Amount,tblbatch.BatchName from tblview_transaction inner join tblaccount on tblaccount.Id= tblview_transaction.AccountTo  inner join tblfeeaccount on tblfeeaccount.Id= tblview_transaction.FeeId inner join tblbatch on tblbatch.Id = tblview_transaction.BatchId where  tblview_transaction.BillNo='" + BillNo + "' Order by tblbatch.BatchName ,tblfeeaccount.AccountName ASC ,tblaccount.AccountName DESC";
            sql = "SELECT  tblview_transaction.PeriodName as Period,tblview_transaction.FeeName AS 'Fee Name',tblaccount.AccountName,tblview_transaction.Amount,tblbatch.BatchName from tblview_transaction inner join tblaccount on tblaccount.Id= tblview_transaction.AccountTo inner join tblbatch on tblbatch.Id = tblview_transaction.BatchId inner join tblperiod on tblperiod.Id= tblview_transaction.PeriodId where  tblview_transaction.BillNo='" + BillNo + "' Order by tblbatch.BatchName ASC ,tblview_transaction.FeeId,tblperiod.`Order` ASC, tblaccount.AccountName DESC";
            data_Fee = _mysqlObj.ExecuteQueryReturnDataSet(sql);
            table = "<table cellspacing=\"0\" style=\"margin: inherit; padding: inherit; color: black; width: 100%; border: thin solid #000000;\"  border=\"2\"> <th align=\"left\">Batch</th> <th align=\"left\">Period</th> <th align=\"left\">Fee Name</th>  <th align=\"left\">Amount</th>";//<th align=\"left\">Type</th>
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

            string StudentName = "", AdmissionNo = "", RollNo = "", ClassName = "";
            GetStudentDetails(BillNo, out StudentName, out AdmissionNo, out RollNo, out ClassName);
            LblAdminno.Text = AdmissionNo;
            LblCls.Text = ClassName;
            LblName.Text = StudentName;
            lblRno.Text = RollNo;
            Lbl_BillId.Text = BillNo;

        }
        public void GetStudentDetails(string _BillNo, out string _StudentName, out string _AdmissionNo, out  string _RollNumber, out string _ClassName)
        {
            bool _RegularStudent = false;
            _StudentName = "";
            _AdmissionNo = "";
            string _StudentId = "-1";
            _RollNumber = "Not allotted";
            _ClassName = "";
            string _ClassId = "0";
            //SELECT   tblview_studentclassmap.RollNo, tblclass.ClassName from tblview_student inner join tblview_feebill on tblview_feebill.StudentID= tblview_student.Id inner join tblview_studentclassmap on tblview_studentclassmap.StudentId= tblview_student.Id and tblview_studentclassmap.BatchId=tblview_feebill.BatchId inner join tblclass on tblclass.Id = tblview_studentclassmap.ClassId where  tblview_feebill.BillNo= '" + BillNo + "'
            string sql = "select tblview_feebill.StudentID,tblview_feebill.StudentName,tblview_feebill.TempId from tblview_feebill where tblview_feebill.BillNo='" + _BillNo + "'";
            if (m_TransationDb != null)
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            else
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);

            if (m_MyReader.HasRows)
            {
                _StudentId = m_MyReader.GetValue(0).ToString();
                if (_StudentId == "0")
                    _RegularStudent = false;
                _StudentName = m_MyReader.GetValue(1).ToString();
                _AdmissionNo = m_MyReader.GetValue(2).ToString();
            }
            if (!_RegularStudent)
            {
                sql = "SELECT tblview_student.AdmitionNo from tblview_student where  tblview_student.Id= " + _StudentId + "";
                if (m_TransationDb != null)
                    m_MyReader = m_TransationDb.ExecuteQuery(sql);
                else
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);


                if (m_MyReader.HasRows)
                {
                    _AdmissionNo = m_MyReader.GetValue(0).ToString();
                }
            }
            sql = "select Id,ClassName from tblview_feebill inner join  tblclass on tblclass.Id = tblview_feebill.ClassId where tblview_feebill.BillNo='" + _BillNo + "'";

            if (m_TransationDb != null)
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            else
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);


            if (m_MyReader.HasRows)
            {
                _ClassId = m_MyReader.GetValue(0).ToString();
                _ClassName = m_MyReader.GetValue(1).ToString();
            }

            sql = "select RollNo from tblview_studentclassmap where ClassId=" + _ClassId + " and StudentId=" + _StudentId;

            if (m_TransationDb != null)
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            else
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);


            if (m_MyReader.HasRows)
            {
                _RollNumber = m_MyReader.GetValue(0).ToString();
            }
        }
        private void LoadSchoolDetails()
        {
            MysqlClass _mysqlObj = new MysqlClass(Connection);
            string standardId =GetStandardIdfromClassId(Class_Id.ToString());
            string SchoolName = "";
            string NeedSepreteSchoolNameForClass = IsneedSeprateSchoolName(_mysqlObj, out SchoolName);

            if (NeedSepreteSchoolNameForClass != "0")
            {
                string[] _standardIdArray = NeedSepreteSchoolNameForClass.Split('|');
                if (!_standardIdArray.Contains(standardId))
                    SchoolName = "";

            }
            string Sql = "SELECT SchoolName,Address FROM tblschooldetails WHERE Id=1";
            m_MyReader = _mysqlObj.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                if (SchoolName == "")
                {
                    SchoolName = m_MyReader.GetValue(0).ToString();
                }
                Lbl_schoolname.Text = SchoolName;
                Lbi_subHead.Text = m_MyReader.GetValue(1).ToString();
            }
        }
        public string GetStandardIdfromClassId(string _selectedClassId)
        {
            MysqlClass _mysqlObj = new MysqlClass(Connection);
            string sql = "select tblclass.Standard from tblclass where tblclass.Id=" + _selectedClassId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                return m_MyReader.GetValue(0).ToString();
            }
            return "0";
        }
        public string IsneedSeprateSchoolName(MysqlClass _mysqlObj, out string SchoolName)
        {
            SchoolName = "";
            string StandardId = "0";

            string sql = "select tblconfiguration.Value , tblconfiguration.SubValue from tblconfiguration where tblconfiguration.Name='NeedSeparateSchl_NameInBill' and tblconfiguration.Module='PSLogin'";
            OdbcDataReader reader = _mysqlObj.ExecuteQuery(sql);

            if (reader.HasRows)
            {
                StandardId = reader["Value"].ToString();
                if (StandardId != "0")
                {
                    SchoolName = reader["SubValue"].ToString();
                }
            }

            return StandardId;
        }

        public string IsneedSeprateLogo(MysqlClass _mysqlObj, out string SchoolLogo)
        {
            SchoolLogo = "";
            string StandardId = "0";

            string sql = "select tblconfiguration.Value , tblconfiguration.SubValue from tblconfiguration where tblconfiguration.Name='NeedSeparateLogo' and tblconfiguration.Module='OnlineStudentRegistration'";
            OdbcDataReader reader = _mysqlObj.ExecuteQuery(sql);

            if (reader.HasRows)
            {
                StandardId = reader["Value"].ToString();
                if (StandardId != "0")
                {
                    SchoolLogo = reader["SubValue"].ToString();
                }
            }

            return StandardId;
        }


        private void LoadLogo()
        {
            MysqlClass _mysqlObj = new MysqlClass(Connection);
            string standardId = GetStandardIdfromClassId(Class_Id.ToString());
            string SchoolLogo = "";
            string NeedSepretelogoForClass = IsneedSeprateLogo(_mysqlObj, out SchoolLogo);

            if (NeedSepretelogoForClass != "0")
            {
                string[] _standardIdArray = NeedSepretelogoForClass.Split('|');
                if (!_standardIdArray.Contains(standardId))
                    SchoolLogo = "";

            }

            if (SchoolLogo == "")
            {
                Img_logo.ImageUrl = "Handler/ImageReturnHandler.ashx?id=1&type=Logo";
            }
            else
            {
                Img_logo.ImageUrl = "~/images/" + SchoolLogo;
            }
        }
        private void Get_Header_Payment_Details(string Transaction_No,out int Class_Id)
        {

            Class_Id = 0;
            DataSet Ds_HPD = new DataSet();
            MysqlClass _mysqlObj = new MysqlClass(Connection);
            string sql;
            sql = "select * from tbl_header_payment where tbl_header_payment.Transaction_Id='" + Transaction_No + "'";
            Ds_HPD = _mysqlObj.ExecuteQueryReturnDataSet(sql);
            if (Ds_HPD != null && Ds_HPD.Tables[0] != null && Ds_HPD.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Ds_HPD.Tables[0].Rows)
                {
                    int.TryParse(dr["Class_Id"].ToString(), out Class_Id);
           
                }
            }

        }
    }
}
