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
    public partial class Monthlywisereport : System.Web.UI.Page
    {
        private WinBase.Payroll Mypay;
        private KnowinUser MyUser;
        private OdbcDataReader m_MyReader = null;
        private DataSet MydataSet = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            //if (Session["StudId"] == null)
            //{
            //    Response.Redirect("PayrollHead.aspx");
            //}
            MyUser = (KnowinUser)Session["UserObj"];
            Mypay = MyUser.GetPayrollObj();
            if (Mypay == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(843))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (!IsPostBack)
                {
                    //DateTime _date = System.DateTime.Today;
                    //string _sdate = MyUser.GerFormatedDatVal(_date);
                    //Txt_FromDate.Text = _sdate;
                    //Txt_ToDate.Text = _sdate;
                    //Txt_FromDate.Enabled = false;
                    //Txt_ToDate.Enabled = false;
                    int peyrolltype = 0;
                    LoadDrpPayrollType();
                    Drp_PayrollType.SelectedValue = peyrolltype.ToString();
                    LoadYearToDropDown();
                    LoadMonthToDropdown();
                    LoadHedToDropdown();
                    Pnl_ShowReport.Visible = false;
                    SelectperiodRow.Visible = false;
                }
            }
        }

        protected void Btn_reportShow_Click(object sender, EventArgs e)
        {
            GenerateMonthlyWisePayrollReport();
        } 

        protected void Grd_MonthlyPayrollReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_MonthlyPayrollReport.PageIndex = e.NewPageIndex;
            GenerateMonthlyWisePayrollReport();
            
        }

         protected void Drp_Year_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            LoadMonthToDropdown();
        }

        protected void Btn_ShowExcelReport_Click(object sender, EventArgs e)
        {
            DataSet MyData = (DataSet)ViewState["MonthlyWiseReport"];
            if (MyData != null && MyData.Tables[0].Rows.Count > 0)
            {
                MyData.Tables[0].Columns.Remove("EmpId");
                MyData.Tables[0].Columns[0].ColumnName = "StaffName";
                string Name = "MonthlyWiseReport.xls";
                //if (!ExcelUtility.ExportDataSetToExcel(MyData, Name))
                //{

                //}
                string FileName = "MonthlyWiseReport";
                string _ReportName = "MonthlyWiseReport";
                if (!WinEr.ExcelUtility.ExportDataToExcel(MyData, _ReportName, FileName, MyUser.ExcelHeader))
                {

                }
            }
        }

        protected void Drp_PayrollType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadHedToDropdown();
        }

        protected void Drp_SelectPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime TDate = System.DateTime.Now.Date;
            int _ThisMonthId = TDate.Month;
            int _year = TDate.Year;
            int _yearId = 2;
            if (int.Parse(Drp_SelectPeriod.SelectedValue) == 0)
            {
                Drp_Year.SelectedValue = _yearId.ToString();
                Drp_FromMonth.SelectedValue = _ThisMonthId.ToString();
                Drp_ToMonth.SelectedValue = _ThisMonthId.ToString();
                SelectperiodRow.Visible = false;

            }

            if (int.Parse(Drp_SelectPeriod.SelectedValue) == 1)
            {

                int _LastMonthId = _ThisMonthId - 1;
                Drp_FromMonth.SelectedValue = _LastMonthId.ToString();
                Drp_ToMonth.SelectedValue = _LastMonthId.ToString();
                Drp_Year.SelectedValue = _yearId.ToString();
                SelectperiodRow.Visible = false;
            }

            if (int.Parse(Drp_SelectPeriod.SelectedValue) == 2)
            {
                GetDropDownValue(_ThisMonthId);
                SelectperiodRow.Visible = false; ;
            }

            if (int.Parse(Drp_SelectPeriod.SelectedValue) == 3)
            {
                GetDropDownValue(_ThisMonthId);                
                SelectperiodRow.Visible = true;
            }
        }

        #region Methods

        private void GetDropDownValue(int _ThisMonthId)
        {
            int _yearId = 2;
            int _fromMonthId = 1;
            int _toMonthId;
            if (int.Parse(Drp_Year.SelectedValue) < 2)
            {
                _toMonthId = 12;

            }
            else
            {
                _toMonthId = _ThisMonthId;
            }
            Drp_FromMonth.SelectedValue = _fromMonthId.ToString();
            Drp_ToMonth.SelectedValue = _toMonthId.ToString();
            Drp_Year.SelectedValue = _yearId.ToString();
        }

        private void GenerateMonthlyWisePayrollReport()
        {
            int _MonthId = 0;
            Lbl_monthpayrollreport_err.Text = "";
            DataSet MonthPayroll_Ds = new DataSet();
            int _year = int.Parse(Drp_Year.SelectedItem.ToString());
            int _FromMonthId = int.Parse(Drp_FromMonth.SelectedValue.ToString());
            int _ToMonthId = int.Parse(Drp_ToMonth.SelectedValue.ToString());
            int _PayrollType = int.Parse(Drp_PayrollType.SelectedValue.ToString());
            int _payrollHeadId = int.Parse(Drp_Head.SelectedValue.ToString());
            MonthPayroll_Ds = Mypay.GenerateMonthlyPayrollReport(_year, _FromMonthId,_ToMonthId, _PayrollType, _payrollHeadId);
            if (MonthPayroll_Ds != null && MonthPayroll_Ds.Tables[0].Rows.Count > 0)
            {
                Grd_MonthlyPayrollReport.Columns[0].Visible = true;
                Pnl_ShowReport.Visible = true;
                Grd_MonthlyPayrollReport.DataSource = MonthPayroll_Ds;
                Grd_MonthlyPayrollReport.DataBind();
                Grd_MonthlyPayrollReport.Columns[0].Visible = false;
            }
            else
            {
                Pnl_ShowReport.Visible = false;
                Lbl_monthpayrollreport_err.Text = "No report exist for this month";
                Grd_MonthlyPayrollReport.DataSource = null;
                Grd_MonthlyPayrollReport.DataBind();
            }
            ViewState["MonthlyWiseReport"] = MonthPayroll_Ds;
        }


        private void LoadMonthToDropdown()
        {
            //Drp_Month.Items.Clear();
            Drp_FromMonth.Items.Clear();
            Drp_ToMonth.Items.Clear();
            int Month = 0;
            string[] months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            DateTime _Current = DateTime.Now.Date;
            int Yr = int.Parse(Drp_Year.SelectedItem.Text.ToString());
            if (Yr == null)
            {
                Yr = System.DateTime.Now.Year;
            }
            if (Yr == _Current.Year)
            {
                Month = _Current.Month;
            }
            else
            {
                Month = 12;
            }
            Drp_FromMonth.Items.Clear();
            Drp_ToMonth.Items.Clear();
            ListItem li;
            for (int i = 0; i < Month; i++)
            {

                li = new ListItem(months[i], (i + 1).ToString());
                 Drp_FromMonth.Items.Add(li);
                 Drp_ToMonth.Items.Add(li);
            }
            Drp_FromMonth.SelectedValue = (Month).ToString();
            Drp_ToMonth.SelectedValue = (Month).ToString();
        }

        private void LoadYearToDropDown()
        {
            Drp_Year.Items.Clear();
            int CurrentYear = DateTime.Now.Year;
            ListItem li = new ListItem();
            li = new ListItem((CurrentYear - 2).ToString(), "0");
            Drp_Year.Items.Add(li);
            li = new ListItem((CurrentYear - 1).ToString(), "1");
            Drp_Year.Items.Add(li);
            li = new ListItem(CurrentYear.ToString(), "2");
            Drp_Year.Items.Add(li);

            Drp_Year.SelectedValue = "2";
        }

        private void LoadDrpPayrollType()
            {
                MydataSet = null;
                Drp_PayrollType.Items.Clear();
                MydataSet = Mypay.FillDrp();

                if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
                {
                    ListItem li;
                    li = new ListItem("All", "0");
                    Drp_PayrollType.Items.Add(li);
                    foreach (DataRow Dr_Cat in MydataSet.Tables[0].Rows)
                    {
                        li = new ListItem(Dr_Cat[1].ToString(), Dr_Cat[0].ToString());
                        Drp_PayrollType.Items.Add(li);
                    }
                }
                else
                {
                    ListItem li = new ListItem("No Category Found", "-1");
                    Drp_PayrollType.Items.Add(li);
                }
            }

        private void LoadHedToDropdown()
            {
                Drp_Head.Items.Clear();
                OdbcDataReader _CatHeadreader = null;
                int AllHead = 0;
                int payrollType = int.Parse(Drp_PayrollType.SelectedValue.ToString());
                if (payrollType > 0)
                {
                    _CatHeadreader = Mypay.GetHeadName(payrollType);
                }
                else
                {
                    _CatHeadreader = Mypay.GetHeadName(AllHead);
                }
                if (_CatHeadreader.HasRows)
                {
                    ListItem li;
                    li = new ListItem("All", "0");
                    Drp_Head.Items.Add(li);
                    while (_CatHeadreader.Read())
                    {
                        li = new ListItem(_CatHeadreader.GetValue(1).ToString(), _CatHeadreader.GetValue(0).ToString());
                        Drp_Head.Items.Add(li);
                    }
                }
                else
                {
                    ListItem li = new ListItem("No Category Found", "-1");
                    Drp_Head.Items.Add(li);
                }

            }

        #endregion

           
           

         
           
            

           

       

       

    }
}
