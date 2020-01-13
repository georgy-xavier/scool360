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
    public partial class FeeBillWithLogoAndHeader : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;

        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
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
                       // Lbl_DDNo.Visible = false;
                       // Lbl_BankName.Visible = false;
                      //  Lbl_DDNoData.Visible = false;
                     //   Lbl_BankNameData.Visible = false;
                       // Checkpaymentmode(BillNo, BillType);
                        LoadHeaderAndFooter();
                        LoadStudent(BillNo, BillType);
                        LoadFeeDetails(BillNo, BillType);
                      //  LoadTotal(BillNo, BillType);
                      //  Lbl_StaffName.Text = MyFeeMang.LoadFeeCollectedStaffName(BillNo, BillType);
                       // LoadSchoolDetails();
                    }
                }
            }
        }

        private void LoadTotal(string BillNo, string BillType,out string total)
        {
            string sql = "";
            total = "";
            sql = "select tblview_feebill.TotalAmount as Total ,date_format(tblview_feebill.Date,'%d-%m-%Y') AS 'PaidDate' from tblview_feebill where tblview_feebill.BillNo='" + BillNo + "'";

            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                total = MyReader.GetValue(0).ToString();
                lblpDate.Text = MyReader.GetValue(1).ToString();
            }
        }


          
    

        private void LoadFeeDetails(string BillNo, string BillType)
        {
            string table = "";
            int slno = 1;
            double _total = 0;
            DataSet data_Fee;
            string _Period = "";
            string _FeeName = "";
            string _Batch = "";
            string sql = "",total="";
            string _Payment_mode = "By Cash";
            //sai added for payment mode
            string sql_paymode = "SELECT  tblview_feebill.PaymentMode  from tblview_feebill where BillNo='" + BillNo + "'";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql_paymode);
            if (MyReader.HasRows)
            {
                _Payment_mode ="By "+ MyReader.GetValue(0).ToString();
            }
            //

            //tblbatch.BatchName ASC ,tblview_transaction.FeeId,, tblaccount.AccountName DESC
            sql = "SELECT  tblview_transaction.PeriodName as Period,tblview_transaction.FeeName AS 'Fee Name',tblaccount.AccountName,tblview_transaction.Amount,tblbatch.BatchName, tblview_transaction.BalanceAmount from tblview_transaction inner join tblaccount on tblaccount.Id= tblview_transaction.AccountTo inner join tblbatch on tblbatch.Id = tblview_transaction.BatchId inner join tblperiod on tblperiod.Id= tblview_transaction.PeriodId  where  tblview_transaction.BillNo='" + BillNo + "' Order by tblperiod.`Id` ASC, tblaccount.AccountName  ASC";
            data_Fee = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            table = "<table width=\"100%\"  style=\"border-collapse:collapse;\"  align=\"center\">";
            table = table + "<tr>";
            //table = table + "<td class=\"bordercls\" align=\"center\" ><b>Batch</b></td>  <td  class=\"bordercls\" align=\"center\"> " + _Batch + " </td>";
            table = table + " <td  align=\"center\" style=\"border-right:1px solid #000000\"  ><b>SL No</b>     </td>";
            //table = table + "<td class=\"bordercls\" align=\"center\" ><b>Period</b></td> <td class=\"bordercls\" align=\"center\"> " + _Period + " </td> <td class=\"bordercls\" align=\"center\">  </td> <td class=\"bordercls\" align=\"center\"> " + _Period + " </td>";
            table = table + "<td align=\"left\" style=\"border-right:1px solid #000000;padding-left:30px\"   ><b>Fee Particulars</b></td>";
            table = table + "<td  align=\"center\"  style=\"border-right:1px solid #000000\" ><b>Balance Amount</b></td>";
            table = table + " <td align=\"center\"   ><b>Amount Paid</b></td>";
            table = table + "</tr>";
            table=table+"   <tr><td style=\"border-bottom:solid 1px #000000\" colspan=\"4\"></td></tr>";
           // table = table + "<td colspan=\"4\"><div style=\"min-height:500px\">";
            if (data_Fee != null && data_Fee.Tables != null && data_Fee.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in data_Fee.Tables[0].Rows)
                {

                    if (_Period == "" && _FeeName == "" && _Batch == "")
                    {
                        _Period = dr[0].ToString();
                        _FeeName = dr[1].ToString();
                        _Batch = dr[4].ToString();
                        table = table + " <tr><td style=\"border-right: 1px solid #000000;\"  align=\"center\"> " + slno + " </td> <td  style=\"border-right: 1px solid #000000;padding-left:30px\"  align=\"left\">" + _FeeName + "(" + _Period + ":" + _Batch + " )</td> <td  style=\"border-right: 1px solid #000000;\"  align=\"center\">" + dr[5].ToString() + "</td><td  align=\"center\">" + dr[3].ToString() + "</td></tr>";
                        if ("Discount" != dr[2].ToString())
                        {
                            _total = double.Parse(dr[3].ToString());
                        }
                        slno++;
                    }
                    else if (_Period == dr[0].ToString() && _FeeName == dr[1].ToString() && _Batch == dr[4].ToString())
                    {
                        table = table + "<tr><td  style=\"border-right: 1px solid #000000;\"  align=\"center\"> </td><td  style=\"border-right: 1px solid #000000;padding-left:30px\"  align=\"left\">" + dr[1].ToString() + "-" + dr[2].ToString() + "(" + _Period + ":" + _Batch + " )</td><td  style=\"border-right: 1px solid #000000;\"  align=\"center\">" + dr[5].ToString() + "</td> <td   align=\"center\" >" + dr[3].ToString() + " </td></tr>";//<td class=\"tdStyle\">" + dr[2].ToString() + "</td>
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
                        // table = table + "<tr><td class=\"tdStyle\">  </td><td class=\"tdStyle\">  </td><td class=\"tdStyle\"></td><td style=\"background-color:Silver\" class=\"tdStyle\"> TOTAL </td><td style=\"background-color:Silver\" class=\"tdStyle\">" + _total.ToString() + "</td></tr>";
                        table = table + " <tr><td style=\"border-right: 1px solid #000000;\"  align=\"center\"> " + slno + " </td> <td  style=\"border-right: 1px solid #000000;padding-left:30px\"  align=\"left\">" + _FeeName + "(" + _Period + ":" + _Batch + " )</td> <td  style=\"border-right: 1px solid #000000;\"  align=\"center\">" + dr[5].ToString() + "</td><td  align=\"center\">" + dr[3].ToString() + "</td></tr>";
                        if ("Discount" != dr[2].ToString())
                        {
                            _total = double.Parse(dr[3].ToString());
                        }
                        slno++;
                    }
                }
               // table = table + "</div></td>";
                int rowcount = GetRowCount();
                for(int i=0;i<=rowcount;i++)
                {
                    table = table + "<tr><td style=\"border-right: 1px solid #000000;\"  align=\"center\">&nbsp;&nbsp;</td> <td  style=\"border-right: 1px solid #000000;\"  align=\"center\"> &nbsp;&nbsp;</td> <td  style=\"border-right: 1px solid #000000;\"  align=\"center\">&nbsp;&nbsp;  </td><td  align=\"center\">&nbsp;&nbsp;  </td></tr>";
                }
                LoadTotal(BillNo, BillType, out total);
                double.TryParse(total, out _total);
                string Words = Convert_Number_To_Words((long)_total).ToUpper();
                table=table+"<tr><td style=\"border-bottom:solid 2px #000000\" colspan=\"4\"></td></tr>";

                table = table + "<tr>";
                table = table + "<td   colspan=\"2\"></td>";
                table = table + "<td  align=\"center\" style=\"border-right:1px solid #000000\"><b>Total:</td>";
                table = table + "<td   align=\"center\"><b></b>" + _total + "</td>";
                table = table + "</tr>";

                table=table+" <tr><td style=\"border-bottom:solid 1px #000000\" colspan=\"4\"></td></tr>";

                table = table + "<tr>";
                table = table + "<td   colspan=\"2\"></td>";
                table = table + "<td  align=\"center\" style=\"border-right:1px solid #000000\"><b>"+_Payment_mode+":</td>";
                table = table + "<td  align=\"center\"><b>" + _total + "</b></td>";
                table = table + "</tr>";

                table=table+"<tr><td style=\"border-bottom:solid 1px #000000\" colspan=\"4\"></td></tr>";

                table = table + "<tr>";
                table = table + "<td  colspan=\"4\" width=\"100%\">In Words:" + Words + "</td>";
                table = table + "</tr>";

                table = table + "<tr><td style=\"border-bottom:solid 1px #000000\" colspan=\"4\"></td></tr>";

                table = table + "<tr>";
                table = table + "<td   colspan=\"2\" style=\"border-right:1px solid #000000\"> Condition : <br />* This receipt is valid only with the seal and signature of the authorised person of the above institution.</td>";
                table = table + "<td   align=\"center\" colspan=\"2\" rowspan=\"2\">Authorised Signature</td>";
                table = table + "</tr>";

                table = table + "<tr><td style=\"border-bottom:solid 1px #000000\" colspan=\"2\"></td></tr>";

                table = table + "<tr>";
                table = table + "<td  colspan=\"2\"> Bill Issued By: " + MyUser.UserName + " &nbsp;&nbsp;&nbsp; Issued Date: " + System.DateTime.Now.Date.ToShortDateString() + "</td>";

                table = table + "</tr>";
                table = table + "<tr><td style=\"border-bottom:solid 1px #000000\" colspan=\"4\"></td></tr>";

            }

            table = table + "</table>";
           this.FeeDetails.InnerHtml = table;
        }

        private int GetRowCount()
        {
            int count = 0;
            OdbcDataReader countreader = null;
            string sql = "Select Subvalue from tblconfiguration where Name='Bill View Needed'";
            countreader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (countreader.HasRows)
            {
                int.TryParse(countreader.GetValue(0).ToString(), out count);
            }

            return count;
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
       

        //private void Checkpaymentmode(string BillNo, string BillType)
        //{
        //    string Paymode = "";
        //    string sql = "SELECT PaymentMode FROM tblview_feebill WHERE BillNo='" + BillNo + "'";

        //    MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
        //    if (MyReader.HasRows)
        //    {
        //        Paymode = MyReader.GetValue(0).ToString();
        //    }
        //    if (Paymode != "Cash")
        //    {
        //        Lbl_DDNo.Visible = true;
        //        Lbl_BankName.Visible = true;
        //        Lbl_DDNoData.Visible = true;
        //        Lbl_BankNameData.Visible = true;

        //        sql = "SELECT PaymentModeId,BankName FROM tblview_feebill WHERE BillNo='" + BillNo + "'";
        //        MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
        //        if (MyReader.HasRows)
        //        {
        //            Lbl_DDNoData.Text = MyReader.GetValue(0).ToString();
        //            Lbl_BankNameData.Text = MyReader.GetValue(1).ToString();
        //        }
        //    }
        //}
        
        private void LoadStudent(string BillNo, string BillType)
        {

            string StudentName="",AdmissionNo="",RollNo="",ClassName="";
            MyFeeMang.GetStudentDetails(BillNo, out StudentName, out AdmissionNo, out RollNo, out ClassName);
            LblAdminno.Text = AdmissionNo;
            LblCls.Text = ClassName;
            LblName.Text = StudentName;
            //lblRno.Text = RollNo;
            Lbl_BillId.Text = BillNo;
            Lbl_BatchName.Text = MyUser.CurrentBatchName;

        }


        private void LoadHeaderAndFooter()
        {
            string sql = "";
            MyReader = null;
            sql = "Select tblheaderfooter.BillHeader, tblheaderfooter.BillFooter from tblheaderfooter where tblheaderfooter.Id=1";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Img_Header.ImageUrl = WinerUtlity.GetRelativeFilePath(objSchool) + "ThumbnailImages/" + MyReader.GetValue(0).ToString();
                Img_Footer.ImageUrl = WinerUtlity.GetRelativeFilePath(objSchool) + "ThumbnailImages/" + MyReader.GetValue(1).ToString();
            }
            else
            {
                Img_Header.ImageUrl = WinerUtlity.GetRelativeFilePath(objSchool) + "ThumbnailImages/img.png";
                Img_Footer.ImageUrl = WinerUtlity.GetRelativeFilePath(objSchool) + "ThumbnailImages/img.png";
            }
        }
    }
}
