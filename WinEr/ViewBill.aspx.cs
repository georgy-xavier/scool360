using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.Odbc;
using System.IO;
using WinBase;
namespace WinEr
{
    public partial class WebForm11 : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet = null;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyFeeMang = MyUser.GetFeeObj();
            if (MyFeeMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(40))
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
                   // Txt_Search_AutoCompleteExtender.ContextKey = "0";
                    LoadLastOneMonth();
                   
                    LoadClassToDrp();
                  
                    LoadCollectedUser();
                    LoadFeeType();
                    Img_Excel.Enabled = false;
                    btn_pdf.Enabled = false;
                    Pnl_Bills.Visible = false;
                    //some initlization
                }
            }
        }



        private void LoadClassToDrp()
        {
            Drp_Class.Items.Clear();

            DataSet myDataset;
            myDataset = MyUser.MyAssociatedClass();
            if (myDataset != null && myDataset.Tables != null && myDataset.Tables[0].Rows.Count > 0)
            {
                ListItem li = new ListItem("All Class", "0");
                Drp_Class.Items.Add(li);

                foreach (DataRow dr in myDataset.Tables[0].Rows)
                {
                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Class Present", "-1");
                Drp_Class.Items.Add(li);
           
                Lbl1_BillMessage.Text = "No Class Present";
                DisableAll();
            }
            Drp_Class.SelectedIndex = 0;
        }

     
        #region Amount Collected

        private void LoadFeeType()
        {
            Drp_FeeType.Items.Clear();
            string sql = "SELECT tblfeetype.Id , tblfeetype.Name from tblfeetype";
            if (!MyFeeMang.HasJoiningFee())
            {
                sql = sql + " where tblfeetype.Id<>2";
            }
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("All", "0");
                Drp_FeeType.Items.Add(li);
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_FeeType.Items.Add(li);
                }
                     
            }
            else
            {
                ListItem li = new ListItem("No type Found", "-1");
                Drp_FeeType.Items.Add(li);
                Lbl1_BillMessage.Text = "No fee type Found";
                DisableAll();
            }
        }

        private void LoadLastOneMonth()
        {
            if (MyUser.HaveActionRignt(894))
            {
                DateTime _Now = System.DateTime.Now;
                Txt_To.Text = MyUser.GerFormatedDatVal(_Now);
                //_Now.Date.ToString("dd/MM/yyyy");
                DateTime _Dt = _Now.AddMonths(-1);
                Txt_from.Text = MyUser.GerFormatedDatVal(_Dt);
                //_Dt.Date.ToString("dd/MM/yyyy");
            }
            else
            {
                DateTime _Now = System.DateTime.Now.Date;
                Txt_To.Text = MyUser.GerFormatedDatVal(_Now);
                Txt_from.Text = MyUser.GerFormatedDatVal(_Now);
                Txt_To.Enabled = false;
                Txt_from.Enabled = false;
            }

        }
   
        private void LoadClass3toDrp()
        {
            Drp_Class.Items.Clear();

            DataSet myDataset;
            myDataset = MyUser.MyAssociatedClass();
            if (myDataset != null && myDataset.Tables != null && myDataset.Tables[0].Rows.Count > 0)
            {
                ListItem li = new ListItem("ALL Class", "0");
                Drp_Class.Items.Add(li);

                foreach (DataRow dr in myDataset.Tables[0].Rows)
                {
                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Class Found", "-1");
                Drp_Class.Items.Add(li);
                Lbl1_BillMessage.Text = "No Class Found";
            }
            Drp_Class.SelectedIndex = 0;
        }

        private void LoadCollectedUser()
        {
            Drp_CollectedUser.Items.Clear();
            DataSet myDataset;
            myDataset = MyFeeMang.LoadCollectedUser();
            if (myDataset != null && myDataset.Tables != null && myDataset.Tables[0].Rows.Count > 0)
            {
                ListItem li = new ListItem("All User", "0");
                Drp_CollectedUser.Items.Add(li);
                foreach (DataRow dr in myDataset.Tables[0].Rows)
                {
                    li = new ListItem(dr[0].ToString(), dr[1].ToString());
                    Drp_CollectedUser.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No user found", "-1");
                Drp_CollectedUser.Items.Add(li);
                Lbl1_BillMessage.Text = "No user found";
            }
            Drp_CollectedUser.SelectedIndex = 0;
        }

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ReloadData();
        }

        protected void Grid_fee3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ImgBtn = (ImageButton)e.Row.FindControl("ImgCancel");
                ImgBtn.Attributes.Add("onclick", "javascript:return " +
                     "confirm('Are you sure you want to cancel the Bill " +
                     DataBinder.Eval(e.Row.DataItem, "BillNo") + " ')");
            }
        }

        private void DisableAll()
        {
           
        }

        protected void Btn_getAmount_Click(object sender, EventArgs e)
        {
            Grid_fee3.PageIndex = 0;
            if (LoadDataTogrid())
            {
                if (Rdo_BillType.SelectedValue == "2")
                {
                    Grid_fee3.Columns[6].Visible = true;
                    Grid_fee3.Columns[7].Visible = true;
                    Grid_fee3.Columns[8].Visible = false;
                    Grid_fee3.Columns[9].Visible = false;
                    LoadOtherdata();
                }
                else
                {
                    Grid_fee3.Columns[6].Visible = false;
                    Grid_fee3.Columns[7].Visible = false;
                    Grid_fee3.Columns[8].Visible = true;
                    Grid_fee3.Columns[9].Visible = true;
                }
            }

        }

        private void LoadOtherdata()
        {
            string Desc = "";
            foreach (GridViewRow gv in Grid_fee3.Rows)
            {

                 Label Lbl_CancelDate = (Label)gv.FindControl("Lbl_CanDate");
                 Lbl_CancelDate.Text = GetCanceledDate(gv.Cells[5].Text.ToString(), out Desc);
                 Label Lbl_Reason = (Label)gv.FindControl("Lbl_Reason");
                 Lbl_Reason.Text = Desc;
            }
        }

        private string GetCanceledDate(string _BillNo , out string _Desc)
        {
            string _Date = "";
            _Desc = "";
            string sql = "select  DATE_FORMAT( tblbillcancel.CancelDate,'%d/%m/%Y') as CancelDate,SUBSTRING(Reason,1,30) as Reason from tblbillcancel where BillNo='" + _BillNo + "'";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _Date = MyReader.GetValue(0).ToString();
                _Desc = MyReader.GetValue(1).ToString();
            }
            return _Date;
        }

        private bool LoadDataTogrid()
        {
            Lbl1_BillMessage.Text = "";
            bool _valide = false;
            if (Txt_from.Text.Trim() == "" || Txt_To.Text.Trim() == "")
            {
                Lbl1_BillMessage.Text = "One or more fields are empty";
                 //WC_MessageBox.ShowMssage("One or more fields are empty");
            }
            else if (Drp_Class.SelectedValue == "-1")
            {
                Lbl1_BillMessage.Text = "No class Found";
                //WC_MessageBox.ShowMssage("No class Found");
            }
            
            else
            {
                try
                {

                    DateTime _from = MyUser.GetDareFromText(Txt_from.Text.ToString());
                    DateTime _To = MyUser.GetDareFromText(Txt_To.Text.ToString());
                    if (_from > _To)
                    {
                        Lbl1_BillMessage.Text = "From date should not be greater than to date";
                        //WC_MessageBox.ShowMssage("From date should not be greater than to Date");
                    }
                    else
                    {
                        string sql="";
                        //if (Rdo_BillType.SelectedValue == "1")
                        //{
                            sql = GetGridSqlString(_from, _To);
                        //}
                        //else if (Rdo_BillType.SelectedValue == "2")
                        //{
                        //    sql = GetCanceledGridSqlString(_from, _To);
                        //}
                        DataSet FeeBill = new DataSet();
                        DataTable dt;
                        DataRow dr;

                        FeeBill.Tables.Add(new DataTable("Bill"));
                        dt = FeeBill.Tables["Bill"];
                        dt.Columns.Add("Id");
                     
                        dt.Columns.Add("name");
                        dt.Columns.Add("class");
                        dt.Columns.Add("Amount");
                        dt.Columns.Add("PaidDate");
                        dt.Columns.Add("BillNo");
                       

                        MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

                        if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow Dr_FeeBill in MydataSet.Tables[0].Rows)
                            {
                                dr = FeeBill.Tables["Bill"].NewRow();
                                dr["Id"] = Dr_FeeBill[0].ToString();
                                dr["name"] = Dr_FeeBill[1].ToString().ToUpper();
                                dr["class"] = Dr_FeeBill[2].ToString();
                               // dr["Amount"] = MyFeeMang.GetTotalAmount(Dr_FeeBill[4].ToString(),Drp_FeeType.SelectedValue);
                                dr["Amount"] = Dr_FeeBill[5].ToString();
                                dr["PaidDate"] = Dr_FeeBill[3].ToString();
                                dr["BillNo"] = Dr_FeeBill[4].ToString();
                                FeeBill.Tables["Bill"].Rows.Add(dr);
                            }

                            Grid_fee3.Columns[0].Visible = true;
                            Grid_fee3.DataSource = FeeBill;
                            Grid_fee3.DataBind();
                            Grid_fee3.Columns[0].Visible = false;
                            ViewState["Sql"] = sql;
                            ViewState["FeeBill"] = FeeBill;
                            Img_Excel.Enabled = true;
                            btn_pdf.Enabled = true;
                            _valide = true;
                            Pnl_Bills.Visible = true;
                            if (Rdo_BillType.SelectedValue == "1")
                            {
                                Lbl_Amt.Text = CalculateAmt(_from, _To);
                                Pnl_CollectedAmt.Visible = true;
                            }
                            else
                            {
                                Lbl_Amt.Text = "0";
                                Pnl_CollectedAmt.Visible = false;
                            }
                        }
                        else
                        {
                             Lbl_Amt.Text="0";
                            Grid_fee3.DataSource = null;
                            Grid_fee3.DataBind();
                            Pnl_Bills.Visible = true;
                            Img_Excel.Enabled = false;
                            btn_pdf.Enabled = false;
                            Lbl1_BillMessage.Text = "No Fee Found";
                           // WC_MessageBox.ShowMssage("No Fee Found");

                        }
                    }
                }
                catch (Exception Exc)
                {
                    WC_MessageBox.ShowMssage(Exc.Message);
                    _valide = false;
                }
            }
            return _valide;

        }      

        private string CalculateAmt(DateTime _from, DateTime _To)
        {
            string Amt="0";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(GetTotalAmount(_from, _To));
            if (MyReader.HasRows)
            {
                Amt = MyReader.GetValue(0).ToString();
            }
            return Amt;
        }

        protected void Rdo_BillType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid_fee3.DataSource = null;
            Grid_fee3.DataBind();
            Lbl_Amt.Text = "0";
            Pnl_CollectedAmt.Visible = false;
        }

        private string GetCanceledGridSqlString(DateTime _from, DateTime _To)
        {
            string _sql = "";
            _sql = "SELECT DISTINCT  tblview_feebill.StudentID as Id ,tblview_feebill.StudentName as name, tblclass.ClassName as class,  date_format( tblbillcancel.PaymentDate , '%d-%m-%Y') AS 'PaidDate', tblbillcancel.BillNo ,tblview_feebill.TotalAmount as Amount , DATE_FORMAT( tblbillcancel.CancelDate,'%d/%m/%Y') as CancelDate  , tblbillcancel.Reason  from tblbillcancel  inner join tblview_feebill on tblview_feebill.BillNo = tblbillcancel.BillNo inner join tblclass on tblclass.Id = tblview_feebill.ClassId where tblbillcancel.CancelDate>='" + _from.Date.ToString("s") + "' and tblbillcancel.CancelDate<='" + _To.Date.ToString("s") + "' ";
            if (Drp_FeeType.SelectedValue == "1")
            {
                _sql += " and tblbillcancel.FeeType=1 ";
            }
            else if (Drp_FeeType.SelectedValue == "2")
            {
                _sql += " and tblbillcancel.FeeType=2 ";
            }
            if (Drp_Class.SelectedValue != "0")
            {
                _sql = _sql + " AND tblview_feebill.ClassId=" + Drp_Class.SelectedValue;
            }

            if ((Drp_CollectedUser.SelectedValue != "0") && (Drp_CollectedUser.SelectedValue != "-1"))
            {
                _sql = _sql + " AND tblbillcancel.CanceledBy=" + Drp_CollectedUser.SelectedValue;
            }
            if (Drp_FeeType.SelectedValue == "1")
            {
                _sql = _sql + " AND tblview_feebill.`status`<>3";
            }
            else if (Drp_FeeType.SelectedValue == "2")
            {
                _sql = _sql + " AND tblview_feebill.`status`=3";
            }
            if (Rdo_BillType.SelectedValue == "1")
                _sql = _sql + " AND tblview_feebill.`Canceled`<>1 ";
            else if (Rdo_BillType.SelectedValue == "2")
                _sql = _sql + " AND tblview_feebill.`Canceled`=1 ";

            _sql = _sql + " order by  tblview_feebill.TransationId desc";
            return _sql;
        }



        private bool PdfReportEnabled()
        {
            bool _valid = false;
            string PdfReport;
            string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='PdfReport'";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
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

        protected void Btn_CancelBill_Click(object sender, EventArgs e)
        {
            int Id;
            string _Date;
            //int feeType = int.Parse(Drp_FeeType.SelectedValue);
            int feeType = MyFeeMang.GetBillType(Hdn_BillNo.Value);
            string _Message = "Cannot cancel the bill. Please try again";
            if (feeType!=0 && MyFeeMang.CancelBill(Hdn_BillNo.Value, Hdn_StdId.Value, "tblfeebill", "tbltransaction", feeType, out Id, out _Date, out _Message))
            {
                WC_MessageBox.ShowMssage("The Bill " + Hdn_BillNo.Value + " is canceled");

                MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Cancel Bill", "The billno " + Hdn_BillNo.Value + " has been canceled", 1,1);
                MyFeeMang.WriteCancelLog(Hdn_BillNo.Value, int.Parse(Hdn_StdId.Value), 1, MyUser.UserId, _Date, Txt_CancelReason.Text.Trim());//1 Reguler fee

                Txt_CancelReason.Text = "";
                LoadDataTogrid();
            }
            else
            {
                WC_MessageBox.ShowMssage(_Message);
                Txt_CancelReason.Text = "";
            }
        }

        protected void Grid_fee3_onrowdelete(object sender, GridViewDeleteEventArgs e)
        {

            string Studentid = Grid_fee3.DataKeys[e.RowIndex].Values["Id"].ToString();
            string BillNo = Grid_fee3.Rows[e.RowIndex].Cells[5].Text.ToString();
            if (MyFeeMang.IsRegularBill(BillNo) || !LiveStudent(Studentid))
            {
                if (Studentid != "0" && LiveStudent(Studentid))
                {
                    Hdn_StdId.Value = Studentid.ToString();
                    Hdn_BillNo.Value = BillNo.ToString();
                    MPE_CancelBill.Show();
                }
                else if (Studentid == "0")
                {
                    Hdn_StdId.Value = Studentid.ToString();
                    Hdn_BillNo.Value = BillNo.ToString();
                    MPE_CancelBill.Show();
                }
                else
                {
                    WC_MessageBox.ShowMssage("Cannot cancel the bill. The student has been moved history");
                }
            }
            else
            {
                WC_MessageBox.ShowMssage("Cannot cancel the bill. Selected bill was created while paying joining fee.");
            }
            

        }

        private bool LiveStudent(string _Studentid)
        {
            bool _Valid = false;
            string sql = "select StudentId from tblstudentclassmap where StudentId="+_Studentid;
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _Valid = true;
            }
            return _Valid;
        }

        protected void Img_Excel_Click(object sender, EventArgs e)
        {
            DataSet _FeeBill = new DataSet();
            string sql = (string)ViewState["Sql"];
            _FeeBill = (DataSet)ViewState["FeeBill"];
            _FeeBill = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            _FeeBill.Tables[0].Columns.Remove("Id");
            DateTime _from = MyUser.GetDareFromText(Txt_from.Text.ToString());
            DateTime _To = MyUser.GetDareFromText(Txt_To.Text.ToString());
            Lbl_Amt.Text = CalculateAmt(_from, _To);
            //if (!WinEr.ExcelUtility.ExportDataSetToExcel(_FeeBill, "FeeBils.xls"))
            //{
            //    WC_MessageBox.ShowMssage("This function need Ms office");
            //}

            string FileName = "FeeBillReport";

            string _ReportName = "<table><tr><td colspan=\"8\" style=\"text-align:center;\"><b>Fee Bill Report</b></td></tr><tr><td>Created Date:" + DateTime.Now.ToString() + "</td><td>From:" + Txt_from.Text + " TO:" + Txt_To.Text + " </td><td>Total Amount : "+Lbl_Amt.Text+" </td></tr></table>";
            if (!WinEr.ExcelUtility.ExportDataToExcel(_FeeBill, _ReportName, FileName, MyUser.ExcelHeader))
            {
                WC_MessageBox.ShowMssage("MS Excel is missing. Please install");
            }
        }

        private string GetGridSqlString(DateTime _from, DateTime _To)
        {
            string _sql = "";
         
            _sql = "SELECT DISTINCT tblview_feebill.StudentID as Id ,tblview_feebill.StudentName as name, tblclass.ClassName as class,  date_format( tblview_feebill.`Date` , '%d-%m-%Y') AS 'PaidDate', tblview_feebill.BillNo , tblview_feebill.TotalAmount as Amount from tblview_feebill inner join tblclass on tblclass.Id = tblview_feebill.ClassId where  tblview_feebill.`Date` >='" + _from.Date.ToString("s") + "' AND tblview_feebill.`Date` <='" + _To.Date.ToString("s") + "'";
           
            if (Drp_Class.SelectedValue != "0")
            {
                _sql = _sql + " AND tblview_feebill.ClassId=" + Drp_Class.SelectedValue;
            }
            
            if ((Drp_CollectedUser.SelectedValue != "0") && (Drp_CollectedUser.SelectedValue != "-1"))
            {
                _sql = _sql + " AND tblview_feebill.CollectedUser=" + Drp_CollectedUser.SelectedValue;
            }
            if (Drp_FeeType.SelectedValue == "1")
            {
                _sql = _sql + " AND tblview_feebill.`RegularFee`=1"; // Regulaer Fee
            }
            else if (Drp_FeeType.SelectedValue == "2")
            {
                _sql = _sql + " AND tblview_feebill.`RegularFee`=0";//Joiningh Fee
            }
            if(Rdo_BillType.SelectedValue=="1")
                _sql = _sql + " AND tblview_feebill.`Canceled`<>1 ";
            else if(Rdo_BillType.SelectedValue=="2")
                _sql = _sql + " AND tblview_feebill.`Canceled`=1 ";
            _sql = _sql + " order by  tblview_feebill.Date asc, tblview_feebill.TransationId asc";
            return _sql;
        }

        private string GetTotalAmount(DateTime _from, DateTime _To)
        {
            string _sql = "";

            _sql = "SELECT sum(tblview_feebill.TotalAmount) as Amount from tblview_feebill inner join tblclass on tblclass.Id = tblview_feebill.ClassId where  tblview_feebill.`Date` >='" + _from.Date.ToString("s") + "' AND tblview_feebill.`Date` <='" + _To.Date.ToString("s") + "'";

            if (Drp_Class.SelectedValue != "0")
            {
                _sql = _sql + " AND tblview_feebill.ClassId=" + Drp_Class.SelectedValue;
            }

            if ((Drp_CollectedUser.SelectedValue != "0") && (Drp_CollectedUser.SelectedValue != "-1"))
            {
                _sql = _sql + " AND tblview_feebill.CollectedUser=" + Drp_CollectedUser.SelectedValue;
            }
            if (Drp_FeeType.SelectedValue == "1")
            {
                _sql = _sql + " AND tblview_feebill.`RegularFee`=1"; // Regulaer Fee
            }
            else if (Drp_FeeType.SelectedValue == "2")
            {
                _sql = _sql + " AND tblview_feebill.`RegularFee`=0";//Joiningh Fee
            }
            if (Rdo_BillType.SelectedValue == "1")
                _sql = _sql + " AND tblview_feebill.`Canceled`<>1 ";
            else if (Rdo_BillType.SelectedValue == "2")
                _sql = _sql + " AND tblview_feebill.`Canceled`=1 ";
            _sql = _sql + " order by tblview_feebill.Date asc, tblview_feebill.TransationId asc";
            return _sql;
        }

        protected void Grid_fee3_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grid_fee3.PageIndex = e.NewPageIndex;

            Grid_fee3.Columns[1].Visible = true;
            Grid_fee3.Columns[6].Visible = true;
            Grid_fee3.Columns[7].Visible = true;
            Grid_fee3.Columns[8].Visible = true;
            Grid_fee3.Columns[9].Visible = true;
           
            string sql = (string)ViewState["Sql"];
           // MydataSet = (DataSet)ViewState["FeeBill"];
            MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                Grid_fee3.DataSource = MydataSet;
                Grid_fee3.DataBind();
            }
            if (Rdo_BillType.SelectedValue == "2")
            {
                Grid_fee3.Columns[6].Visible = true;
                Grid_fee3.Columns[7].Visible = true;
                Grid_fee3.Columns[8].Visible = false;
                Grid_fee3.Columns[9].Visible = false;
                LoadOtherdata();
            }
            else
            {
                Grid_fee3.Columns[6].Visible = false;
                Grid_fee3.Columns[7].Visible = false;
                Grid_fee3.Columns[8].Visible = true;
                Grid_fee3.Columns[9].Visible = true;
            }
        }

        protected void Drp_Account_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ReloadData();
        }
        

        #endregion

        protected void Drp_FeeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid_fee3.DataSource = null;
            Grid_fee3.DataBind();
            Img_Excel.Enabled = false;
            btn_pdf.Enabled = false;
            Lbl_Amt.Text = "0";
            Pnl_CollectedAmt.Visible = false;
        }

        protected void Grd_FeeBillSorting(object sender, GridViewSortEventArgs e)
        {
            Grid_fee3.Columns[1].Visible = true;
            Grid_fee3.Columns[6].Visible = true;
            Grid_fee3.Columns[7].Visible = true;
            Grid_fee3.Columns[8].Visible = true;
            Grid_fee3.Columns[9].Visible = true;
            MydataSet = (DataSet)ViewState["FeeBill"];

            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                DataTable dtAccountData = MydataSet.Tables[0];

                DataView dataView = new DataView(dtAccountData);

                dataView.Sort = e.SortExpression + " " + GetSortDirection1(e.SortExpression);

                Grid_fee3.DataSource = dataView;
                Grid_fee3.DataBind();
                DataSet ds = new DataSet();
                DataTable dTable = dataView.ToTable();
                ds.Tables.Add(dTable);
                ViewState["FeeBill"] = ds;
                if (Rdo_BillType.SelectedValue == "2")
                {
                    Grid_fee3.Columns[6].Visible = true;
                    Grid_fee3.Columns[7].Visible = true;
                    Grid_fee3.Columns[8].Visible = false;
                    Grid_fee3.Columns[9].Visible = false;
                    LoadOtherdata();
                }
                else
                {
                    Grid_fee3.Columns[6].Visible = false;
                    Grid_fee3.Columns[7].Visible = false;
                    Grid_fee3.Columns[8].Visible = true;
                    Grid_fee3.Columns[9].Visible = true;
                }               
            }
        }
     
        private string GetSortDirection1(string column)
        {
            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = Session["SortExpression1"] as string;


            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = Session["SortDirection1"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }
            // Save new values in ViewState.
            Session["SortDirection1"] = sortDirection;
            Session["SortExpression1"] = column;

            return sortDirection;
        }

        protected void Grid_fee3_RowEditing(object sender, GridViewEditEventArgs e)
        {
            string Studentid = Grid_fee3.DataKeys[e.NewEditIndex].Values["Id"].ToString();
            string BillNo = Grid_fee3.DataKeys[e.NewEditIndex].Values["BillNo"].ToString(); ;

            if (BillNo != "")
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
                        //if (Drp_FeeType.SelectedValue == "1")
                        //{
                        //if (MyPdf.CreateFeeReciptPdf(BillNo, MyUser.CurrentBatchName, MyUser.CurrentBatchId, _physicalpath, out _PdfName, out _ErrorMsg,_Value))
                        if (MyPdf.CreateFeeReciptPdf(BillNo, _physicalpath, out _PdfName, out _ErrorMsg, _Value))
                        {
                            _ErrorMsg = "";
                            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);

                        }
                        else if (_PdfName == "")
                        {
                            WC_MessageBox.ShowMssage("Failed To Create");
                        }
                        //}
                        //else
                        //{

                        //    //if (MyPdf.CreateJoiningFeeReciptPdf(BillNo, MyUser.CurrentBatchName, _physicalpath, out _PdfName, out _ErrorMsg,_Value))
                        //    if (MyPdf.CreateFeeReciptPdf(BillNo, MyUser.CurrentBatchName, MyUser.CurrentBatchId, _physicalpath, out _PdfName, out _ErrorMsg, _Value))
                        //    {
                        //        _ErrorMsg = "";
                        //        ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);

                        //    }
                        //    else if (_PdfName == "")
                        //    {
                        //        WC_MessageBox.ShowMssage("Faild To Create");
                        //    }
                        //}
                    }
                    else
                    {
                        string _Bill = BillNo;
                        string _PdfName = "",PrinterType="";
                        int FeeCount=0;
                   
                        string sql="select tblconfiguration.Value, tblconfiguration.SubValue from tblconfiguration where tblconfiguration.Module='Fee Manager' and tblconfiguration.Name='DotMetrixPrinter'";
                        MyReader=MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                        if(MyReader.HasRows)
                        {
                            PrinterType = MyReader.GetValue(0).ToString();
                            FeeCount = int.Parse(MyReader.GetValue(1).ToString());
                        }


                        if (PrinterType == "1" && GetFileName(_Bill, out _PdfName))
                        {
                            try
                            {
                                MyFeeMang.CreateTansationDb();

                                string _ErrorMsg = "";
                                Pdf MyPdf = new Pdf(MyFeeMang.m_TransationDb, objSchool);
                                _ErrorMsg = "";
                                string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
                                RemovePDF_ifExist(_Bill, _physicalpath);// Arun added on 27-0ct-11
                                string _PdfNamenew = "";

                                if (MyPdf.CreateFeeRecipt(_Bill, _physicalpath, out _PdfNamenew, out _ErrorMsg, _Value, MyFeeMang, FeeCount))
                                {
                                    _ErrorMsg = "";
                                    ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfNamenew + "\");", true);

                                }
                                else if ((_PdfName == ""))
                                {
                                    WC_MessageBox.ShowMssage("Failed To Create");

                                }

                                MyFeeMang.EndSucessTansationDb();
                            }
                            catch (Exception ex)
                            {

                                WC_MessageBox.ShowMssage("Failed To Create. " + ex.Message);
                                MyFeeMang.EndFailTansationDb();
                            }
                        }
                        else
                        {
                            if (Drp_FeeType.SelectedValue == "1")
                            {
                                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"" + _PageName + "?BillNo=" + _Bill + "&BillType=0\");", true);
                            }
                            else
                            {

                                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"" + _PageName + "?BillNo=" + _Bill + "&BillType=1\");", true);
                            }
                        }

                       

                    }
                }
            }
        }
        private void RemovePDF_ifExist(string _Bill, string _physicalpath)
        {
            string _PdfName = "";
            string sql = "select tblview_feebill.TransationId from tblview_feebill where tblview_feebill.BillNo='" + _Bill + "'";
            OdbcDataReader m_MyReader = MyFeeMang.m_TransationDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
                _PdfName = "T_" + m_MyReader.GetValue(0).ToString() + ".pdf";
            else
                _PdfName = "Temp.pdf";
            if (File.Exists(_physicalpath + "\\PDF_Files\\" + _PdfName))
            {
                File.Delete(_physicalpath + "\\PDF_Files\\" + _PdfName);
            }
        }
        private bool GetFileName(string _Bill,out string _PdfName)
        {
            _PdfName = "";
            string sql = "select tblview_feebill.TransationId from tblview_feebill where tblview_feebill.BillNo='" + _Bill + "'";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                _PdfName = "T_" + MyReader.GetValue(0).ToString() + ".pdf";
                
                return true;
            }
            else
                return false;
                
                
        }

    }
}
