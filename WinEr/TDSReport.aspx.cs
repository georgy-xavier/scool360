using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;
using System;

namespace WinEr
{
    public partial class TDSReport : System.Web.UI.Page
    {
        private WinBase.Payroll Mypay;
        private KnowinUser MyUser;
        private DataSet MydataSet = null;
        private OdbcDataReader m_MyReader = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }

            MyUser = (KnowinUser)Session["UserObj"];
            Mypay = MyUser.GetPayrollObj();
            if (Mypay == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(808))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (!IsPostBack)
                {
                    LoadMonthToDrpList();
                    LoadYearToDrpList();
                    //Drp_Year.Enabled = false;
                    BtnExcel.Visible = false;
                    //some initlization

                }
            }

        }
        private void LoadMonthToDrpList()
        {
            MydataSet = null;
            Drp_Month.Items.Clear();
            MydataSet = Mypay.FillMonthDrp();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow Dr_Month in MydataSet.Tables[0].Rows)
                {
                    ListItem li = new ListItem(Dr_Month[1].ToString(), Dr_Month[0].ToString());
                    Drp_Month.Items.Add(li);
                }
                int CurrentMonth = DateTime.Now.Month;
                Drp_Month.SelectedValue = CurrentMonth.ToString();

            }
            else
            {
                ListItem li = new ListItem("No Month Found", "-1");
                Drp_Month.Items.Add(li);
            }
        }

        private void LoadYearToDrpList()
        {
            MydataSet = null;
            Drp_Year.Items.Clear();

            int i = 0;
            MydataSet = Mypay.FillYearDrp();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                BtnShow.Enabled = true;
                foreach (DataRow Dr_Year in MydataSet.Tables[0].Rows)
                {
                    ListItem li = new ListItem(Dr_Year[0].ToString(), i.ToString());
                    Drp_Year.Items.Add(li);
                    i++;
                }
                int CurrentYear = DateTime.Now.Year;
                Drp_Year.SelectedValue = (i - 1).ToString();

            }
            else
            {
                BtnShow.Enabled = false;
                ListItem li = new ListItem("No Year Found", "-1");
                Drp_Year.Items.Add(li);
            }

        }

        protected void BtnShow_Click(object sender, EventArgs e)
        {
            int _monthId = int.Parse(Drp_Month.SelectedValue.ToString());
            int _Year = int.Parse(Drp_Year.SelectedItem.Text.ToString());
            int TDSId = 0;
            OdbcDataReader MyPayReader = null;
            DataSet Mydataset = new DataSet();
            DataTable dt;
            DataRow dr;
            Mydataset.Tables.Add(new DataTable("TDSEmp"));
            dt = Mydataset.Tables["TDSEmp"];
            dt.Columns.Add("EmpId");
            dt.Columns.Add("Surname");
            dt.Columns.Add("Designation");
            dt.Columns.Add("Total");
            m_MyReader = Mypay.GetTDSId();
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {

                    TDSId = int.Parse(m_MyReader.GetValue(0).ToString());
                }
            }
            MyPayReader = Mypay.GetHeadEmp(TDSId, _monthId, _Year);
            if (MyPayReader.HasRows)
            {
                while (MyPayReader.Read())
                {
                    dr = Mydataset.Tables["TDSEmp"].NewRow();
                    dr["EmpId"] = MyPayReader.GetValue(0).ToString();
                    dr["Surname"] = MyPayReader.GetValue(1).ToString();
                    dr["Designation"] = MyPayReader.GetValue(3).ToString();
                    dr["Total"] = double.Parse(MyPayReader.GetValue(2).ToString()); ;
                    Mydataset.Tables["TDSEmp"].Rows.Add(dr);
                }
                Grd_EmpPay.Columns[0].Visible = true;
                Grd_EmpPay.DataSource = Mydataset;
                Grd_EmpPay.DataBind();
                Grd_EmpPay.Columns[0].Visible = false;
                Pnl_EmpPay.Visible = true;
                Grd_EmpPay.Visible = true;
                BtnExcel.Visible = true;
            }
            else
            {
                Grd_EmpPay.DataSource = null;
                Grd_EmpPay.DataBind();
                WC_MessageBox.ShowMssage("No Data present for selected Month and Year");
                BtnExcel.Visible = false;
            }
            ViewState["TDSGrid"] = Mydataset;
        }

        protected void BtnExcel_Click(object sender, EventArgs e)
        {
            DataSet MyData = (DataSet)ViewState["TDSGrid"];

            string Name = "TDS_Report.xls";
            //if (!ExcelUtility.ExportDataSetToExcel(MyData, Name))
            //{

            //}
            string FileName = "TDS_Repor";
            string _ReportName = "TDS_Repor";
            if (!WinEr.ExcelUtility.ExportDataToExcel(MyData, _ReportName, FileName, MyUser.ExcelHeader))
            {
                
            }

        }
    }
}
