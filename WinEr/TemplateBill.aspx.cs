using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;

namespace WinEr
{
    public partial class TemplateBill : System.Web.UI.Page
    {

        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
       

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
                    if (!IsPostBack)
                    {
                        string BillType = Request.QueryString["BillType"].ToString();

                        BasicInfo _objBasicInfo = new BasicInfo();
                        _objBasicInfo.BillNumber = Request.QueryString["BillNo"].ToString();
                        _objBasicInfo.Batch = MyUser.CurrentBatchName;

                        FeeTotalDetails _objFeeTotalDetails = new FeeTotalDetails();

                        MyFeeMang.GetStudentDetails(_objBasicInfo.BillNumber, out _objBasicInfo.StudentName, out _objBasicInfo.AdmissionNo, out _objBasicInfo.RollNo, out _objBasicInfo.ClassName);
                        LoadTotal(_objBasicInfo.BillNumber, BillType, out _objFeeTotalDetails.TotalInDecimal, out  _objBasicInfo.BillDate);
                        string sql = "SELECT tblconfiguration.Value from tblconfiguration where tblconfiguration.Id = 85 and tblconfiguration.Name='Tax'";
                        MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                        if (MyReader.HasRows)
                        {
                            int val = int.Parse(MyReader.GetValue(0).ToString());
                            if (val == 1)
                            {
                                LoadTax(_objBasicInfo.BillNumber, out _objFeeTotalDetails.TaxAmnt, out _objFeeTotalDetails.TaxName);

                            }
                        }
                        
                     
                        
                        FeeDetails[] _objFeeDetails = LoadFeeDetails(_objBasicInfo.BillNumber, BillType, _objBasicInfo.StudentName, _objBasicInfo.AdmissionNo, _objBasicInfo.RollNo, _objBasicInfo.ClassName, _objFeeTotalDetails.TotalInDecimal, _objBasicInfo.BillDate, out _objFeeTotalDetails.TotalInWords);

                        BillDetails objBillDetails = new BillDetails();
                        objBillDetails.objBasicInfo = _objBasicInfo;
                        objBillDetails.objFeeDetails = _objFeeDetails;
                        objBillDetails.objFeeTotalDetails = _objFeeTotalDetails;

                        System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        string sJSON = oSerializer.Serialize(objBillDetails);

                        Hdn_BillJSON.Value = sJSON;
                        HiddenField1.Value = oSerializer.Serialize(_objBasicInfo);
                        HiddenField2.Value = oSerializer.Serialize(_objFeeDetails);
                        HiddenField3.Value = oSerializer.Serialize(_objFeeTotalDetails);

                        

                        //SchoolClass objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                        //string FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));
                        //FilePath += "\\UpImage\\";

                        SchoolClass objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                        string FilePath = "FileRepository/" + objSchool.FilePath + "/UpImage/";
                        HdnFld_ServerPath.Value = FilePath;
                    }
                }
            }
        }

        private void LoadTax(string BillNo, out string TaxAmnt, out string TaxName)
        {
            string sql = "";
            TaxAmnt = "";
            TaxName = "";


            sql = "select tblview_feebill.TaxAmnt,tbltaxconfig.TaxName from tblview_feebill inner join tbltaxconfig on tbltaxconfig.Id = tblview_feebill.TaxId where tblview_feebill.BillNo='" + BillNo + "'";

            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                TaxAmnt = MyReader.GetValue(0).ToString();
                TaxName = MyReader.GetValue(1).ToString();
                
            }
        }

        private void LoadTotal(string BillNo, string BillType, out string total, out string date)
        {
            string sql = "";
            total = "";
            date = "";
            double tot = 0;
            sql = "select tblview_feebill.TotalAmount as Total ,date_format(tblview_feebill.Date,'%d-%m-%Y') AS 'PaidDate' from tblview_feebill where tblview_feebill.BillNo='" + BillNo + "'";

            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                tot = double.Parse(MyReader.GetValue(0).ToString());
                total = (Math.Round(tot)).ToString();
                date = MyReader.GetValue(1).ToString();
            }
        }

        private void LoadFeeDetails(string BillNo, string BillType, string StudentName, string AdmissionNo, string RollNo, string ClassName, string total, string date, string C_LogoUrl, string C_Name, string Address)
        {
            string table = "";
            DataSet data_Fee, TemplateSet;

            string _Period = "";
            string _FeeName = "";
            double _total = 0;
            string _Batch = "";
            string sql = "";
            string Templates = "";
            sql = "select tblfeebilltemplates.Template from tblfeebilltemplates where tblfeebilltemplates.IsActive=1 and TemplateName='HTML TEMPLATE'";
            TemplateSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (TemplateSet != null && TemplateSet.Tables[0].Rows.Count > 0)
            {
                Templates = TemplateSet.Tables[0].Rows[0][0].ToString();
            }
            if (Templates != "")
            {

                Templates = Templates.Replace("($ClgName$)", C_Name);
                Templates = Templates.Replace("($ClgAddress$)", Address);
                Templates = Templates.Replace("($StudentName$)", StudentName);
                Templates = Templates.Replace("($Class$)", ClassName);
                Templates = Templates.Replace("($RollNo$)", RollNo);
                Templates = Templates.Replace("($AdmNo$)", AdmissionNo);
                Templates = Templates.Replace("($Receiptno$)", BillNo);
                Templates = Templates.Replace("($Date$)", date);



                int count = 1;
                sql = "SELECT  tblview_transaction.PeriodName as Period,tblview_transaction.FeeName AS 'Fee Name',tblaccount.AccountName,tblview_transaction.Amount,tblbatch.BatchName from tblview_transaction inner join tblaccount on tblaccount.Id= tblview_transaction.AccountTo inner join tblbatch on tblbatch.Id = tblview_transaction.BatchId inner join tblperiod on tblperiod.Id= tblview_transaction.PeriodId where  tblview_transaction.BillNo='" + BillNo + "' Order by tblbatch.BatchName ASC ,tblview_transaction.FeeId,tblperiod.`Order` ASC, tblaccount.AccountName DESC";
                data_Fee = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                table = "<tr><td style=\"width:5%;border:#000 thin solid;\">SL.NO</td><td style=\"width:55%;border:#000 thin solid;\">FEE PARTICULARS</td><td style=\"width:20%;border:#000 thin solid;\">DURATION</td><td style=\"width:10%;border:#000 thin solid;\"> AMOUNT</td></tr>";
                if (data_Fee != null && data_Fee.Tables != null && data_Fee.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in data_Fee.Tables[0].Rows)
                    {

                        if (_Period == "" && _FeeName == "" && _Batch == "")
                        {
                            _Period = dr[0].ToString();
                            _FeeName = dr[1].ToString();
                            _Batch = dr[4].ToString();
                            table = table + "<tr><td style=\"border-right:#000 thin solid;border-left:#000 thin solid;\">" + count + "</td><td style=\"border-right:#000 thin solid;\" align=\"left\">" + _FeeName + "-" + dr[2].ToString() + "</td><td style=\"border-right:#000 thin solid;\" align=\"left\">" + _Period + "</td><td style=\"border-right:#000 thin solid;\">" + dr[3].ToString() + "</td> </tr>";



                            if ("Discount" != dr[2].ToString())
                            {
                                _total = double.Parse(dr[3].ToString());
                            }
                        }
                        else if (_Period == dr[0].ToString() && _FeeName == dr[1].ToString() && _Batch == dr[4].ToString())
                        {
                            _Period = dr[0].ToString();
                            _FeeName = dr[1].ToString();
                            _Batch = dr[4].ToString();
                            table = table + "<tr><td style=\"border-right:#000 thin solid;\">" + count + "</td><td style=\"border-right:#000 thin solid;\" align=\"left\">" + _FeeName + "-" + dr[2].ToString() + "</td><td style=\"border-right:#000 thin solid;\" align=\"left\">" + _Period + "</td><td style=\"border-right:#000 thin solid;\">" + dr[3].ToString() + "</td> </tr>";

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
                            table = table + "<tr><td style=\"border-right:#000 thin solid;\">" + count + "</td><td style=\"border-right:#000 thin solid;\" align=\"left\">" + _FeeName + "-" + dr[2].ToString() + "</td><td style=\"border-right:#000 thin solid;\" align=\"left\">" + _Period + "</td><td style=\"border-right:#000 thin solid;\">" + dr[3].ToString() + "</td> </tr>";

                            if ("Discount" != dr[2].ToString())
                            {
                                _total = _total + double.Parse(dr[3].ToString());
                            }
                        }
                        count = count + 1;
                    }
                }

                table = table + "   <tr> <td style=\"border:#000 thin solid;\" colspan=\"2\" align=\"left\"> IN WORDS : ($amtinwords$) </td> <td style=\"border:#000 thin solid;\" align=\"right\">TOTAL </td> <td style=\"border:#000 thin solid;\" colspan=\"2\" align=\"left\">" + _total + "</td></tr>";


                string AmtInwords = MyFeeMang.Convert_Number_To_Words(int.Parse(_total.ToString()));
                table = table.Replace("($amtinwords$)", AmtInwords);

                Templates = Templates.Replace("($Batch$)", _Batch);
                Templates = Templates.Replace("($FeeContent$)", table);

                //this.FeeDetails.InnerHtml = Templates;

            }
            else
            {
                //this.FeeDetails.InnerHtml = "Error: Error in reading templates. Please contact to Support Team";
            }


        }

        private FeeDetails[] LoadFeeDetails(string BillNo, string BillType, string StudentName, string AdmissionNo, string RollNo, string ClassName, string total, string date, out string TotalInWrds)
        {
            DataSet data_Fee;
            string sql = "";
            int count = 0;
            double Total = 0;
            TotalInWrds = "";
            FeeDetails[] lstgroup = null;

            sql = "SELECT  tblview_transaction.PeriodName as Period,tblview_transaction.FeeName AS 'Fee Name',tblaccount.AccountName,tblview_transaction.Amount,tblbatch.BatchName from tblview_transaction inner join tblaccount on tblaccount.Id= tblview_transaction.AccountTo inner join tblbatch on tblbatch.Id = tblview_transaction.BatchId inner join tblperiod on tblperiod.Id= tblview_transaction.PeriodId where  tblview_transaction.BillNo='" + BillNo + "' Order by tblbatch.BatchName ASC ,tblview_transaction.FeeId,tblperiod.`Order` ASC, tblaccount.AccountName DESC";
            data_Fee = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (data_Fee != null && data_Fee.Tables != null && data_Fee.Tables[0].Rows.Count > 0)
            {
                lstgroup = new FeeDetails[data_Fee.Tables[0].Rows.Count];
                foreach (DataRow dr in data_Fee.Tables[0].Rows)
                {
                    FeeDetails objFeedetails = new FeeDetails();

                    objFeedetails.SlNo = count + 1;
                    objFeedetails.FeeName = dr[1].ToString();
                    objFeedetails.Period = dr[0].ToString();
                    objFeedetails.AccountName = dr[2].ToString();
                    objFeedetails.Amount = double.Parse(dr[3].ToString());
                    objFeedetails.BatchName = dr[4].ToString();

                    lstgroup[count] = objFeedetails;
                    Total += objFeedetails.Amount;
                    count += 1;
                }
               // double tot = Math.Round(double.Parse(total));
                TotalInWrds = MyFeeMang.Convert_Number_To_Words(int.Parse(total.ToString()));
            }
            return lstgroup;
        }
    }

    class BillDetails
    {
        public BasicInfo objBasicInfo;
        public FeeDetails[] objFeeDetails;
        public FeeTotalDetails objFeeTotalDetails;
    }

    class BasicInfo
    {
        public int StudenId;
        public string StudentName;
        public string RollNo;
        public string AdmissionNo;
        public string BillNumber;
        public string BillDate;
        public string ClassName;
        public string Batch;
    }

    class FeeDetails
    {
        public int SlNo;
        public string FeeName;
        public string Period;
        public string AccountName;
        public double Amount;
        public string BatchName;
    }

    class FeeTotalDetails
    {
        public string TotalInWords;
        public string TotalInDecimal;
        public string TaxAmnt;
        public string TaxName;
    }
}
