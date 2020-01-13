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

namespace Scool360student
{
    public partial class FeeBillNew : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_init(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }

            else if (Request.QueryString["BillNo"] == null)
            {
                //Response.Redirect("CollectFee.aspx");
            }
            else if (Request.QueryString["BillType"] == null)
            {
                //Response.Redirect("CollectFee.aspx");
            }
            else
            {
                MyUser = (KnowinUser)Session["UserObj"];
                MyFeeMang = MyUser.GetFeeObj();
                if (MyFeeMang == null)
                {
                    Response.Redirect("Default.aspx");
                    //no rights for this user.
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
                        string BillNo = Request.QueryString["BillNo"].ToString();
                        string BillType = Request.QueryString["BillType"].ToString();
                        Lbl_No.Visible = false;
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
            }
        }

        private void Checkpaymentmode(string BillNo, string BillType)
        {
            string Paymode = "";
            string sql = "SELECT PaymentMode FROM tblview_feebill WHERE BillNo='" + BillNo + "'";          

            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Paymode = MyReader.GetValue(0).ToString();
            }
            if (Paymode != "Cash")
            {
                Lbl_No.Visible = true;
                Lbl_BankName.Visible = true;
                Lbl_DDNoData.Visible = true;
                Lbl_BankNameData.Visible = true;

                sql = "SELECT PaymentModeId,BankName,PaymentMode FROM tblview_feebill WHERE BillNo='" + BillNo + "'";
                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Lbl_DDNoData.Text = MyReader.GetValue(0).ToString();
                    Lbl_BankNameData.Text = MyReader.GetValue(1).ToString();
                    //sai changed for exact payment mode
                    Lbl_No.Text = MyReader.GetValue(2).ToString()+" "+"No";
                    //
                }
            }
        }

        private void LoadTotal(string BillNo, string BillType)
        {
            string sql = "";                
        
            sql = "select tblview_feebill.TotalAmount as Total ,date_format(tblview_feebill.Date,'%d-%m-%Y') AS 'PaidDate' from tblview_feebill where tblview_feebill.BillNo='" + BillNo + "'";
       
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                LblAmt.Text = MyReader.GetValue(0).ToString();
                lblpDate.Text = MyReader.GetValue(1).ToString();
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

            //sql = "SELECT  (select tblperiod.Period from tblperiod where tblperiod.Id= tblview_transaction.PeriodId) as Period,tblfeeaccount.AccountName AS 'Fee Name',tblaccount.AccountName,tblview_transaction.Amount,tblbatch.BatchName from tblview_transaction inner join tblaccount on tblaccount.Id= tblview_transaction.AccountTo  inner join tblfeeaccount on tblfeeaccount.Id= tblview_transaction.FeeId inner join tblbatch on tblbatch.Id = tblview_transaction.BatchId where  tblview_transaction.BillNo='" + BillNo + "' Order by tblbatch.BatchName ,tblfeeaccount.AccountName ASC ,tblaccount.AccountName DESC";
            sql = "SELECT  tblview_transaction.PeriodName as Period,tblview_transaction.FeeName AS 'Fee Name',tblaccount.AccountName,tblview_transaction.Amount,tblbatch.BatchName from tblview_transaction inner join tblaccount on tblaccount.Id= tblview_transaction.AccountTo inner join tblbatch on tblbatch.Id = tblview_transaction.BatchId inner join tblperiod on tblperiod.Id= tblview_transaction.PeriodId where  tblview_transaction.BillNo='" + BillNo + "' Order by tblbatch.BatchName ASC ,tblview_transaction.FeeId,tblperiod.`Order` ASC, tblaccount.AccountName DESC";
            data_Fee = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
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
            MyFeeMang.GetStudentDetails(BillNo, out StudentName, out AdmissionNo, out RollNo, out ClassName);
            LblAdminno.Text = AdmissionNo;
            LblCls.Text = ClassName;
            LblName.Text = StudentName;
            lblRno.Text = RollNo;
            Lbl_BillId.Text = BillNo;

        }

        private void LoadSchoolDetails()
        {
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
