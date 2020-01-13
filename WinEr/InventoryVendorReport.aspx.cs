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
    public partial class InventoryVendorReport : System.Web.UI.Page
    {

        private ConfigManager MyConfigMang;
        private KnowinUser MyUser;
        private WinBase.Inventory Myinventory;
        OdbcDataReader Myreader = null;



        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyConfigMang = MyUser.GetConfigObj();
            Myinventory = MyUser.GetInventoryObj();
            if (MyConfigMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(870))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadVendorToDropDown();
                    LoadPeriodToDropDown();
                    LoadItemTodropDown();
                    DisplayVendorReport.Visible = false;
                }
            }
        }

    
        protected void Drp_Period_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPeriodToDropDown();
        }

        protected void Btn_Show_Click(object sender, EventArgs e)
        {
            Lbl_Err.Text = "";
            GenerateVendorReport();
        }

        protected void Grd_VendorReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_VendorReport.PageIndex = e.NewPageIndex;
            GenerateVendorReport();
        }

        protected void img_export_Excel_Click(object sender, ImageClickEventArgs e)
        {
            DataSet MyData = (DataSet)ViewState["VendorReport"];
            MyData.Tables[0].Columns.Remove("Id");
            MyData.Tables[0].Columns.Remove("ItemId");
            string Name = "VendorReport.xls";
            //if (!ExcelUtility.ExportDataSetToExcel(MyData, Name))
            //{

            //}
            string FileName = "VendorReport";
            string _ReportName = "VendorReport";
            if (!WinEr.ExcelUtility.ExportDataToExcel(MyData, _ReportName, FileName, MyUser.ExcelHeader))
            {

            }
        }

       
        #endregion  

        #region Methods

        private void LoadItemTodropDown()
        {
            DataSet ItemDs = new DataSet();
            Drp_ItemName.Items.Clear();
            ListItem li = new ListItem();
            ItemDs = Myinventory.GetAllItems(0, 0);
            if (ItemDs != null && ItemDs.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("All", "0");
                Drp_ItemName.Items.Add(li);
                foreach (DataRow dr in ItemDs.Tables[0].Rows)
                {
                    li = new ListItem(dr["ItemName"].ToString(), dr["Id"].ToString());
                    Drp_ItemName.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("None", "-1");
                Drp_ItemName.Items.Add(li);
            }
        }


        private void GenerateVendorReport()
        {
            DataSet VendorReportDs = new DataSet();
            int vendorId = int.Parse(Drp_Vendor.SelectedValue);
            int ItemID = int.Parse(Drp_ItemName.SelectedValue);            
            DateTime StartDate = General.GetDateTimeFromText(Txt_FromDate.Text);
            DateTime EndDate = General.GetDateTimeFromText(Txt_ToDate.Text);
            VendorReportDs = Myinventory.CreateVendorReport(vendorId, StartDate, EndDate, ItemID);
            VendorReportDs = GetBasicCostForEachItem(VendorReportDs);
            if (vendorId != -1 && ItemID != -1)
            {
                if (VendorReportDs != null && VendorReportDs.Tables[0].Rows.Count > 0)
                {
                    Grd_VendorReport.Columns[0].Visible = true;
                    Grd_VendorReport.Columns[1].Visible = true;
                    DisplayVendorReport.Visible = true;
                    Grd_VendorReport.DataSource = VendorReportDs;
                    Grd_VendorReport.DataBind();
                    Grd_VendorReport.Columns[0].Visible = false;
                    Grd_VendorReport.Columns[1].Visible = false;
                }
                else
                {
                    DisplayVendorReport.Visible = false;
                    Grd_VendorReport.DataSource = null;
                    Grd_VendorReport.DataBind();
                    Lbl_Err.Text = "No report found";
                }
            }
            else
            {
                DisplayVendorReport.Visible = false;
                Grd_VendorReport.DataSource = null;
                Grd_VendorReport.DataBind();
                Lbl_Err.Text = "No report found";
            }
            ViewState["VendorReport"] = VendorReportDs;
        }

        private DataSet GetBasicCostForEachItem(DataSet VendorReportDs)
        {
            VendorReportDs.Tables[0].Columns.Add("BasicCost");
            VendorReportDs.Tables[0].Columns.Add("ActualItemName");
            //&amp; amp;
            OdbcDataReader CostReader = null;
            string sql = "";
            double BasicCost = 0;
            //ItemId
            if (VendorReportDs != null && VendorReportDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in VendorReportDs.Tables[0].Rows)
                {
                    sql = "select tblinv_item.Cost from tblinv_item where tblinv_item.Id=" + int.Parse(dr["ItemId"].ToString()) + "";
                    CostReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                    if (CostReader.HasRows)
                    {
                        double.TryParse(CostReader.GetValue(0).ToString(), out BasicCost);
                        dr["BasicCost"] = BasicCost.ToString();
                        dr["ActualItemName"] = dr["ItemName"].ToString().Replace("&amp;", "&");
                    }
                }
            }
            
            return VendorReportDs;
        }

        private void LoadPeriodToDropDown()
        {
            string _sdate = null, _edate = null;
            if (int.Parse(Drp_Period.SelectedValue) == 0)
            {
                DateTime _date = System.DateTime.Now;
                //DateTime _Endtime = _date.AddDays(-1);
                _edate = MyUser.GerFormatedDatVal(_date);

                DateTime _start = System.DateTime.Now;
                _sdate = MyUser.GerFormatedDatVal(new DateTime(_start.Year, _start.Month, 1));
                Txt_FromDate.Enabled = false;
                Txt_ToDate.Enabled = false;
                Txt_FromDate.Text = _sdate;
                Txt_ToDate.Text = _edate;
                //firstDayOfTheMonth.AddMonths(1).AddDays(-1);


            }
            else if (int.Parse(Drp_Period.SelectedValue) == 1)
            {

                Txt_FromDate.Text = General.GerFormatedDatVal(System.DateTime.Now.Date.AddDays(-7));
                Txt_ToDate.Text = General.GerFormatedDatVal(System.DateTime.Now);

                Txt_FromDate.Enabled = false;
                Txt_ToDate.Enabled = false;

            }
            else if (int.Parse(Drp_Period.SelectedValue) == 2)
            {
                Txt_FromDate.Enabled = true;
                Txt_ToDate.Enabled = true;
                Txt_FromDate.Text = "";
                Txt_ToDate.Text = "";

            }
        }


        private void LoadVendorToDropDown()
        {
            Drp_Vendor.Items.Clear();
            DataSet VendorDs = new DataSet();
            ListItem li = new ListItem();
            VendorDs = Myinventory.GetVendordetails();
            if (VendorDs != null && VendorDs.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("All", "0");
                Drp_Vendor.Items.Add(li);
                foreach (DataRow dr in VendorDs.Tables[0].Rows)
                {
                    li = new ListItem(dr["Name"].ToString(), dr["Id"].ToString());
                    Drp_Vendor.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("None", "-1");
                Drp_Vendor.Items.Add(li);
            }
        }

        #endregion

      
       

     

      
        
    }
}
