using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using WinBase;

namespace WinEr
{
    public partial class OnlinpayReport : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
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
            else if (!MyUser.HaveActionRignt(8))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadfeeHeaderTodrp();
                    LoadClass3toDrp();
                    LoadStatustoDrp();
                    DateTime _date = System.DateTime.Today;
                    string _sdate = MyUser.GerFormatedDatVal(_date);
                    Txt_StartDate.Text = _sdate;
                    Txt_EndDate.Text = _sdate;
                    Pnl_Show.Visible = false;
                    Btn_Export.Visible = false;
                }
            }
        }
        protected void Drp_Period_SelectedIndexChanged(object sender, EventArgs e)
        {

            string _sdate = null, _edate = null;
            if (Drp_Period.SelectedItem.Text.ToString() == "Today")
            {
                DateTime _date = System.DateTime.Today;
                //_sdate = _date.ToString("dd/MM/yyyy");
                _sdate = MyUser.GerFormatedDatVal(_date);

                Txt_StartDate.Enabled = false;
                Txt_EndDate.Enabled = false;
                Txt_StartDate.Text = _sdate;
                Txt_EndDate.Text = _sdate;
            }
            else if (Drp_Period.SelectedItem.Text.ToString() == "Last Week")
            {
                DateTime _date = System.DateTime.Now;
                //_edate = _date.ToString("dd/MM/yyyy");
                _edate = MyUser.GerFormatedDatVal(_date);

                DateTime _start = _date.AddDays(-7);
                //_sdate = _start.Date.ToString("dd/MM/yyyy");
                _sdate = MyUser.GerFormatedDatVal(_start);

                Txt_StartDate.Enabled = false;
                Txt_EndDate.Enabled = false;
                Txt_StartDate.Text = _sdate;
                Txt_EndDate.Text = _edate;
            }

            else if (Drp_Period.SelectedItem.Text.ToString() == "This Month")
            {
                DateTime _date = System.DateTime.Now;
                //_edate = _date.ToString("dd/MM/yyyy");
                _edate = MyUser.GerFormatedDatVal(_date);

                DateTime _start = System.DateTime.Now;
                //_sdate = _start.Date.ToString("01/MM/yyyy");
                // _sdate = "01/" + _start.Month + "/" + _start.Year;
                _sdate = MyUser.GerFormatedDatVal(new DateTime(_start.Year, _start.Month, 1));
                Txt_StartDate.Enabled = false;
                Txt_EndDate.Enabled = false;
                Txt_StartDate.Text = _sdate;
                Txt_EndDate.Text = _edate;
            }
            else if (Drp_Period.SelectedItem.Text.ToString() == "Manual")
            {
                Txt_StartDate.Enabled = true;
                Txt_EndDate.Enabled = true;
                Txt_StartDate.Text = "";
                Txt_EndDate.Text = "";
            }

        }
        private void finddates(out DateTime _sdate, out DateTime _edate)
        {

            _sdate = General.GetDateTimeFromText(Txt_StartDate.Text);
            _edate = General.GetDateTimeFromText(Txt_EndDate.Text);
            if (Drp_Period.SelectedItem.Text.ToString() == "Today")
            {
                DateTime _date = System.DateTime.Today;
                _sdate = _date;
                _edate = System.DateTime.Now;

            }
            else if (Drp_Period.SelectedItem.Text.ToString() == "Last Week")
            {
                DateTime _date = System.DateTime.Now;
                _edate = _date;
                DateTime _start = _date.AddDays(-7);
                _sdate = _start;

            }
            else if (Drp_Period.SelectedItem.Text.ToString() == "This Month")
            {
                DateTime _date = System.DateTime.Now;
                _edate = _date;
                DateTime _start = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);
                _sdate = _start;
            }
            else if (Drp_Period.SelectedItem.Text.ToString() == "Manual")
            {

                if ((Txt_EndDate.Text != "") && (Txt_StartDate.Text != ""))
                {
                    //_sdate = DateTime.Parse(Txt_StartDate.Text.ToString());
                    _sdate = MyUser.GetDareFromText(Txt_StartDate.Text);
                    _edate = MyUser.GetDareFromText(Txt_EndDate.Text);
                }
            }
        }

        private void LoadfeeHeaderTodrp()
        {
            string sql = "SELECT Id,Name from tbl_feesgrouphead";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li;
                li = new ListItem("All","0");
                Drp_FeeType.Items.Add(li);
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_FeeType.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Fee Found", "0");
                Drp_FeeType.Items.Add(li);
                WC_MessageBox.ShowMssage("No Fees Header Found");
                DisableAll();
            }
            Drp_FeeType.SelectedIndex = 0;
        }

        private void LoadClass3toDrp()
        {
            Drp_Class3.Items.Clear();

            DataSet myDataset;
            myDataset = MyUser.MyAssociatedClass();
            if (myDataset != null && myDataset.Tables != null && myDataset.Tables[0].Rows.Count > 0)
            {
                ListItem li = new ListItem("All Class", "0");
                Drp_Class3.Items.Add(li);

                foreach (DataRow dr in myDataset.Tables[0].Rows)
                {

                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class3.Items.Add(li);

                }

            }
            else
            {
                ListItem li = new ListItem("No Class Found", "-1");
                Drp_Class3.Items.Add(li);
            }


            Drp_Class3.SelectedIndex = 0;
        }
        private void LoadStatustoDrp()
        {
            Drp_list_Status.Items.Clear();
            ListItem li = new ListItem("All", "0");
            Drp_list_Status.Items.Add(li);
            ListItem li6 = new ListItem(CommonEnum.OnlineTransactionDisplayStatus.PaymentNotCompleted.ToString(), "1");
            Drp_list_Status.Items.Add(li6);
            ListItem li1 = new ListItem(CommonEnum.OnlineTransactionDisplayStatus.BillGenerationPending.ToString(), "2");
            Drp_list_Status.Items.Add(li1);
            ListItem li2 = new ListItem(CommonEnum.OnlineTransactionDisplayStatus.PaymentSuccess.ToString(), "3");
            Drp_list_Status.Items.Add(li2);
            ListItem li3 = new ListItem(CommonEnum.OnlineTransactionDisplayStatus.PaymentFailure.ToString(), "4");
            Drp_list_Status.Items.Add(li3);
            Drp_list_Status.SelectedIndex = 0;
        }
        private void DisableAll()
        {
            Drp_FeeType.Enabled = false;
            Btn_Export.Enabled = false;
            
        }
        private void loadGrid()
        {
            string _sql = "";
       
            _sql = Generate_Sql();

            MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
            if ((MydataSet != null) && (MydataSet.Tables[0].Rows.Count > 0))
            {

                Grid_fee3.DataSource = MydataSet;
                Grid_fee3.DataBind();
                Session["Online"] = MydataSet;
                lbl_Msg.Text = "";
                Pnl_Show.Visible = true;
                Btn_Export.Visible = true;
            }
            else
            {
                Grid_fee3.DataSource = null;
                Grid_fee3.DataBind();
                Btn_Export.Visible = false;
                lbl_Msg.Text = "No Data Exists";
                Pnl_Show.Visible = false;
            }

        }
        private bool validate(out string _message)
        {
            bool valid = true;
            _message = "";

            string _date1 = Txt_StartDate.Text.ToString();
            string _date2 = Txt_EndDate.Text.ToString();

            //DateTime startdate = DateTime.Parse(_date1);
            //DateTime enddate = DateTime.Parse(_date2);

            DateTime startdate = MyUser.GetDareFromText(_date1);
            DateTime enddate = MyUser.GetDareFromText(_date2);

            TimeSpan diff = enddate.Subtract(startdate);
            int _diff = int.Parse(diff.Days.ToString());

            DateTime today = DateTime.Now;


            if (_diff < 0)
            {
                _message = "The Start Date Is Larger Than End Date";
                valid = false;
            }

            else if (startdate > today)
            {
                _message = "The Start Date Is Larger Than Todays date";
                valid = false;
            }

            else if (enddate > today)
            {
                _message = "The End Date Is Larger Than Todays date";
                valid = false;
            }
            return valid;
        }
        private string Generate_Sql()
        {
            string _sql = "";
            string sql_sequence = "";
            DateTime _sdate, _enddate;
            finddates(out _sdate, out _enddate);
            if (Drp_FeeType.SelectedValue != "0")
            {
                sql_sequence = " AND tbl_header_payment.Header_Id=" + int.Parse(Drp_FeeType.SelectedValue) + "";
            }
            if (Drp_Class3.SelectedValue != "0")
            {
                if (sql_sequence != "")
                {
                    sql_sequence = sql_sequence + " and tbl_header_payment.Class_Id=" + int.Parse(Drp_Class3.SelectedValue) + "";
                }
                else
                {
                    sql_sequence = " AND tbl_header_payment.Class_Id=" + int.Parse(Drp_Class3.SelectedValue) + "";
                }
            }
            if (Drp_list_Status.SelectedValue != "0")
            {
                string substring = "";
                if (Drp_list_Status.SelectedValue == "0")
                {
                    substring = "";
                }
                else if (Drp_list_Status.SelectedValue == "1")
                {
                    substring = "tbl_header_payment.Status='PaymentStarted'";
                }
                else if (Drp_list_Status.SelectedValue == "2")
                {
                    substring = "tbl_header_payment.Status IN('PaymentSuccess','PaymentSuccessButHashNotMatched','BillPending')";
                }
                else if (Drp_list_Status.SelectedValue == "3")
                {
                    substring = "tbl_header_payment.Status='Billed'";
                }
                else if (Drp_list_Status.SelectedValue == "4")
                {
                    substring = "tbl_header_payment.Status IN('PaymentFailure','PaymentFailureButHashNotMatched')";
                }
                if (sql_sequence != "")
                {
                    sql_sequence = sql_sequence + " and " +substring;
                }
                else
                {
                    sql_sequence = " AND " + substring;
                }
            }

                _sql = "select tbl_feesgrouphead.Name as Header_Name,tblfeeaccount.AccountName as `Fee_Name`,tblstudent.StudentName,tbl_header_payment.Parent_Name,tblclass.ClassName,tbl_fees_payment.Period, tbl_fees_payment.Amount,tbl_fees_payment.Fine,tbl_header_payment.Biil_No,tbl_header_payment.Transaction_Id,date_format(tbl_header_payment.Action_Date, '%d-%m-%Y')AS 'ActionDate',tbl_header_payment.Status as Status from tbl_header_payment inner join tbl_feesgrouphead on tbl_feesgrouphead.Id=tbl_header_payment.Header_Id inner join tbl_fees_payment on tbl_fees_payment.Header_Id=tbl_header_payment.Id inner join tblfeeaccount on tbl_fees_payment.Fees_Id=tblfeeaccount.Id inner join tblstudent on tblstudent.Id=tbl_header_payment.Student_Id inner join tblclass on tblclass.Id=tbl_header_payment.Class_Id where  tbl_header_payment.Action_Date BETWEEN '" + _sdate.ToString("s") + "' AND '" + _enddate.AddDays(1).ToString("s") + "'";
           

            _sql = _sql +" "+ sql_sequence;
            return _sql;
        }

        protected void Grid_fee3_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grid_fee3.PageIndex = e.NewPageIndex;
            loadGrid();
        }

        protected void Btn_getAmount_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = "";
                if (validate(out msg))
                {


                    loadGrid();

                }
                else
                {
                    lbl_Msg.Text = msg;
                    Btn_Export.Visible = false;
                    Pnl_Show.Visible = false;
                }
            }
            catch (Exception er)
            {
                lbl_Msg.Text = er.Message;
                Btn_Export.Visible = false;
                Pnl_Show.Visible = false;
            }
        }

        protected void Btn_Export_Click(object sender, EventArgs e)
        {

            DataSet MyData = (DataSet)Session["Online"];

            if (MyData.Tables[0].Rows.Count > 0)
            {
                //if (!WinEr.ExcelUtility.ExportDataSetToExcel(MyExamData, "ExamResult.xls"))
                //{
                //    Lbl_message.Text = "This function need Ms office";
                //}

                string FileName = "Online Payment";
                string _ReportName = "Online Payment";
                if (!WinEr.ExcelUtility.ExportDataToExcel(MyData, _ReportName, FileName, MyUser.ExcelHeader))
                {

                    lbl_Msg.Text = "This function need Ms office";
                }
            }
        }
      
    }
}
