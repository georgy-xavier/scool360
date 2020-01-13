using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;

using System.Data;
using WinBase;

namespace WinEr
{
    public partial class printingpage : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
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
                    if (!IsPostBack)
                    {
                        string BillNo = Request.QueryString["BillNo"].ToString();
                        string BillType = Request.QueryString["BillType"].ToString();
                        string StudentName = "", AdmissionNo = "", RollNo = "", ClassName="";
                        string total="";
                        string date="";
                        string C_Name="";
                        string Address="";
                        string C_LogoUrl="";
                        
                        // Checkpaymentmode(BillNo, BillType);
                        MyFeeMang.GetStudentDetails(BillNo, out StudentName, out AdmissionNo, out RollNo, out ClassName);
                      
                        LoadTotal(BillNo, BillType,out total,out  date);
                        LoadSchoolDetails(out C_LogoUrl,out  C_Name,out  Address);
                        LoadFeeDetails(BillNo, BillType, StudentName, AdmissionNo, RollNo, ClassName, total, date, C_LogoUrl,C_Name,Address);
                    }
                }
            }
        }

       
        private void LoadTotal(string BillNo, string BillType, out string total, out string date)
        {
            string sql  = "";
            total       = "";
            date        = "";

            sql = "select tblview_feebill.TotalAmount as Total ,date_format(tblview_feebill.Date,'%d-%m-%Y') AS 'PaidDate' from tblview_feebill where tblview_feebill.BillNo='" + BillNo + "'";

            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            
            if (MyReader.HasRows)
            {
                total = MyReader.GetValue(0).ToString();
                date  = MyReader.GetValue(1).ToString();
            }
        }

        private void LoadFeeDetails(string BillNo, string BillType, string StudentName, string AdmissionNo, string RollNo, string ClassName, string total, string date, string C_LogoUrl, string C_Name, string Address)
        {
            string table = "";
            DataSet data_Fee,TemplateSet;
            
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

                Templates=Templates.Replace("($ClgName$)",C_Name);
                Templates = Templates.Replace("($ClgAddress$)", Address);
                Templates = Templates.Replace("($StudentName$)", StudentName);
                Templates = Templates.Replace("($Class$)", ClassName);
                Templates = Templates.Replace("($RollNo$)", RollNo);
                Templates = Templates.Replace("($AdmNo$)", AdmissionNo);
                Templates = Templates.Replace("($Receiptno$)", BillNo);
                Templates = Templates.Replace("($Date$)", date);
        

                //($Batch$)
                /*
                  <table width="100%"  style="border-collapse:collapse;Border: #000 solid 1px;">
                    <tr>
                        <td style="width:5%;border:#000 thin solid;">SL.NO</td>
                        <td style="width:55%;border:#000 thin solid;">FEE PARTICULARS</td>
                        <td style="width:20%;border:#000 thin solid;">DURATION</td>
                        <td colspan="2" style="width:10%;border:#000 thin solid;"> AMOUNT</td>
                    </tr>
                    <tr>
                        <td style="border-right:#000 thin solid;"><br /></td>
                        <td style="border-right:#000 thin solid;" align="left">&nbsp;</td>
                        <td style="border-right:#000 thin solid;" align="left">&nbsp;</td>
                        <td style="border-right:#000 thin solid;">&nbsp;</td>
                        
                      
                        
                    </tr>
                    
                    <tr>
                        <td style="border-right:#000 thin solid;"><br /></td>
                        <td style="border-right:#000 thin solid;" align="left">&nbsp;</td>
                        <td style="border-right:#000 thin solid;"  align="left">&nbsp;</td>
                        <td style="border-right:#000 thin solid;">&nbsp;</td>
                       
                      
                        
                    </tr>
                    <tr>
                        <td style="border-right:#000 thin solid;"><br /></td>
                        <td style="border-right:#000 thin solid;" align="left"></td>
                        <td style="border-right:#000 thin solid;" align="left"></td>
                        <td style="border-right:#000 thin solid;"></td>
                       
                        
                    </tr>
                    <tr>
                        <td style="border-right:#000 thin solid;border-bottom:#000 thin solid;"><br /></td>
                        <td style="border-right:#000 thin solid;border-bottom:#000 thin solid;"></td>
                        <td style="border-right:#000 thin solid;border-bottom:#000 thin solid;"></td>
                        <td style="border-right:#000 thin solid;border-bottom:#000 thin solid;"></td>
                                                                    
                    </tr>
                     <tr>
                        <td style="border:#000 thin solid;" colspan="2" align="left"> IN WORDS ($amtinwords$) </td>
                       
                        <td style="border:#000 thin solid;" align="right">TOTAL</td>
                        <td style="border:#000 thin solid;" colspan="2" align="center"></td>
                                                             
                    </tr>
                    
                    
                </table> 
                 * 
                 */
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
                            table = table + "<tr><td style=\"border-right:#000 thin solid;\">" + count + "</td><td style=\"border-right:#000 thin solid;\" align=\"left\">" + _FeeName + "-" + dr[2].ToString() + "</td><td style=\"border-right:#000 thin solid;\" align=\"left\">"+_Period+"</td><td style=\"border-right:#000 thin solid;\">"+dr[3].ToString()+"</td> </tr>";
                    
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
                            table = table + "<tr><td style=\"border-right:#000 thin solid;\">" + count + "</td><td style=\"border-right:#000 thin solid;\" align=\"left\">" + _FeeName + "-" + dr[2].ToString() + "</td><td style=\"border-right:#000 thin solid;\" align=\"left\">"+_Period+"</td><td style=\"border-right:#000 thin solid;\">"+dr[3].ToString()+"</td> </tr>";
                   
                             if ("Discount" != dr[2].ToString())
                            {
                                _total =_total+ double.Parse(dr[3].ToString());
                            }
                        }
                        count = count + 1;
                    }
                  }

                table = table + "   <tr> <td style=\"border:#000 thin solid;\" colspan=\"2\" align=\"left\"> IN WORDS : ($amtinwords$) </td> <td style=\"border:#000 thin solid;\" align=\"right\">TOTAL </td> <td style=\"border:#000 thin solid;\" colspan=\"2\" align=\"left\">"+_total+"</td></tr>";
               

                string AmtInwords = MyFeeMang.Convert_Number_To_Words(int.Parse(_total.ToString()));
                table = table.Replace("($amtinwords$)", AmtInwords);

                Templates = Templates.Replace("($Batch$)", _Batch);
                Templates = Templates.Replace("($FeeContent$)", table);
              
                this.FeeDetails.InnerHtml = Templates;
               
            }
            else
            {
                this.FeeDetails.InnerHtml = "Error: Error in reading templates. Please contact to Support Team";
            }

           
        }

        private void LoadSchoolDetails(out string S_LogoUrl,out string s_Name,out string Address)
        {
            S_LogoUrl = ""; s_Name = ""; Address = "";
            string Sql = "SELECT SchoolName,Address,LogoUrl FROM tblschooldetails WHERE Id=1";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(Sql);
            
            if (MyReader.HasRows)
            {
                S_LogoUrl = MyReader.GetValue(2).ToString();
                s_Name = MyReader.GetValue(0).ToString();
                Address = MyReader.GetValue(1).ToString();
            }
           
        }       
    }

}
