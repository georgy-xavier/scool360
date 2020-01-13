using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WinBase;

namespace WinEr
{
    public partial class InventoryPurchaseReport : System.Web.UI.Page
    {

        private ConfigManager MyConfigMang;
        private KnowinUser MyUser;
        private WinBase.Inventory Myinventory;

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
            else if (!MyUser.HaveActionRignt(863))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadPeriod();
                    LoadItemCategoryToDropdown();
                    LoadItemNameToDropdown();
                    LoadvendorToDropDown();
                    Pnl_Show.Visible = false;

                }
                Lbl_PurchaseErr.Text = "";
            }
        }

        protected void Grd_InventoryPurchaseReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_InventoryPurchaseReport.PageIndex = e.NewPageIndex;
            GenerateReport();
        }

        protected void Drp_Period_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPeriod();
        }

        protected void Drp_Category_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadItemNameToDropdown();
        }

        protected void Btn_Show_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }

        protected void img_export_Excel_Click(object sender, ImageClickEventArgs e)
        {
            DataSet MyData = (DataSet)ViewState["InventoryPurchaseReport"];
            MyData.Tables[0].Columns.Remove("Id");
            MyData.Tables[0].Columns.Remove("ItemId");
            string FileName = "PurchaseReport";
            string _ReportName = "PurchaseReport";
            if (!WinEr.ExcelUtility.ExportDataToExcel(MyData, _ReportName, FileName, MyUser.ExcelHeader))
            {

            }
        }

        #endregion
       

        #region Methods

            private void GenerateReport()
            {
                Lbl_PurchaseErr.Text = "";
                int value = 1;
                DataSet Salereport_Ds = new DataSet();
                int categoryId = int.Parse(Drp_Category.SelectedValue);
                int itemid = int.Parse(Drp_Item.SelectedValue);
                int vendorid = int.Parse(Drp_Vendor.SelectedValue);
                DateTime Fromdate = General.GetDateTimeFromText(Txt_FromDate.Text);
                DateTime Todate = General.GetDateTimeFromText(Txt_ToDate.Text);
                if (categoryId != -1 && itemid != -1 && vendorid != -1)
                {
                    Salereport_Ds = Myinventory.GetPurchaseReport(categoryId, itemid, vendorid, Fromdate, Todate, value);
                    if (Salereport_Ds != null && Salereport_Ds.Tables[0].Rows.Count > 0)
                    {

                        Pnl_Show.Visible = true;
                        Grd_InventoryPurchaseReport.Columns[0].Visible = true;
                        Grd_InventoryPurchaseReport.Columns[1].Visible = true;
                        Grd_InventoryPurchaseReport.Columns[8].Visible = true;
                        Grd_InventoryPurchaseReport.DataSource = Salereport_Ds;
                        Grd_InventoryPurchaseReport.DataBind();
                        Grd_InventoryPurchaseReport.Columns[0].Visible = false;
                        Grd_InventoryPurchaseReport.Columns[1].Visible = false;
                        Grd_InventoryPurchaseReport.Columns[8].Visible = false;
                    }
                    else
                    {
                        Pnl_Show.Visible = false;
                        Grd_InventoryPurchaseReport.DataSource = null;
                        Grd_InventoryPurchaseReport.DataBind();
                        Lbl_PurchaseErr.Text = "No report found";
                    }
                }
                else
                {
                    Pnl_Show.Visible = false;
                    Grd_InventoryPurchaseReport.DataSource = null;
                    Grd_InventoryPurchaseReport.DataBind();
                    Lbl_PurchaseErr.Text = "No report found";
                }

                ViewState["InventoryPurchaseReport"] = Salereport_Ds;
            }

            private void LoadvendorToDropDown()
            {
                DataSet vendor_Ds = new DataSet();
                ListItem li;
                Drp_Vendor.Items.Clear();
                vendor_Ds = Myinventory.GetVendordetails();
                if (vendor_Ds != null && vendor_Ds.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("All", "0");
                    Drp_Vendor.Items.Add(li);
                    foreach (DataRow dr in vendor_Ds.Tables[0].Rows)
                    {
                        li = new ListItem(dr["Name"].ToString(), dr["Id"].ToString());
                        Drp_Vendor.Items.Add(li);
                    }

                }
                else
                {
                    li = new ListItem("No vendor exist", "-1");
                    Drp_Vendor.Items.Add(li);
                }
            }

            private void LoadItemNameToDropdown()
            {
                Drp_Item.Items.Clear();
                DataSet Item_Ds = new DataSet();
                ListItem li;
                int category = int.Parse(Drp_Category.SelectedValue);
                if (category != -1)
                {
                    Item_Ds = Myinventory.GetAllItems(category,"");
                    if (Item_Ds != null && Item_Ds.Tables[0].Rows.Count > 0)
                    {
                        li = new ListItem("All", "0");
                        Drp_Item.Items.Add(li);
                        foreach (DataRow dr in Item_Ds.Tables[0].Rows)
                        {
                            li = new ListItem(dr["ItemName"].ToString(), dr["Id"].ToString());
                            Drp_Item.Items.Add(li);
                        }
                    }
                    else
                    {
                        li = new ListItem("No item found", "-1");
                        Drp_Item.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("No item found", "-1");
                    Drp_Item.Items.Add(li);
                }
            }

            private void LoadItemCategoryToDropdown()
            {
                Drp_Category.Items.Clear();
                DataSet Category_Ds = new DataSet();
                ListItem li;
                Category_Ds = Myinventory.GetAllCategory(0,0);
                if (Category_Ds != null && Category_Ds.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("All", "0");
                    Drp_Category.Items.Add(li);
                    foreach (DataRow dr in Category_Ds.Tables[0].Rows)
                    {
                        li = new ListItem(dr["Category"].ToString(), dr["Id"].ToString());
                        Drp_Category.Items.Add(li);
                    }
                    Drp_Category.SelectedValue = "0";
                }
                else
                {
                    li = new ListItem("No category found", "-1");
                    Drp_Category.Items.Add(li);
                }
            }

            private void LoadPeriod()
            {
                string _sdate = null, _edate = null;
                //Pnl_Show.Visible = false;
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
                    RowManualperiod.Visible = false;
                }
                else if (int.Parse(Drp_Period.SelectedValue) == 1)
                {

                    Txt_FromDate.Text = General.GerFormatedDatVal(System.DateTime.Now.Date.AddDays(-7));
                    Txt_ToDate.Text = General.GerFormatedDatVal(System.DateTime.Now);
                    Txt_FromDate.Enabled = false;
                    Txt_ToDate.Enabled = false;
                    RowManualperiod.Visible = false;

                }
                else if (int.Parse(Drp_Period.SelectedValue) == 2)
                {
                    RowManualperiod.Visible = true;
                    Txt_FromDate.Enabled = true;
                    Txt_ToDate.Enabled = true;
                    Txt_FromDate.Text = "";
                    Txt_ToDate.Text = "";

                }
            }

        #endregion
       

      
    }
}
