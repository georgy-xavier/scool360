using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;

namespace WinEr
{
    public partial class InventoryStockReport : System.Web.UI.Page
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
                else if (!MyUser.HaveActionRignt(857))
                {
                    Response.Redirect("RoleErr.htm");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        LoadLocationToDropDown();
                        LoadItemCategoryToDropdown();
                        LoadItemNameToDropdown();
                        Pnl_Show.Visible = false;

                    }
                }
            }

            protected void Drp_Category_SelectedIndexChanged(object sender, EventArgs e)
            {
                LoadItemNameToDropdown();
                GetItemStockReport();
            }

            protected void Drp_LocationName_SelectedIndexChanged(object sender, EventArgs e)
            {
                LoadItemCategoryToDropdown();
                LoadItemNameToDropdown();
                //GetItemStockReport();
            }

            protected void Btn_Show_Click(object sender, EventArgs e)
            {
                GetItemStockReport();
            }

            protected void Drp_ItemName_SelectedIndexChanged(object sender, EventArgs e)
            {

                GetItemStockReport();
            }

            protected void Grd_Items_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                Grd_Items.PageIndex = e.NewPageIndex;
                GetItemStockReport();
            }

            protected void img_export_Excel_Click(object sender, ImageClickEventArgs e)
            {

                DataSet MyData = (DataSet)ViewState["ItemStockReport"];
                MyData.Tables[0].Columns.Remove("Id");
                MyData.Tables[0].Columns.Remove("Id1");
                string Name = "ItemStockReport.xls";
                //if (!ExcelUtility.ExportDataSetToExcel(MyData, Name))
                //{

                //}
                string FileName = "ItemStockReport";
                string _ReportName = "ItemStockReport";
                if (!WinEr.ExcelUtility.ExportDataToExcel(MyData, _ReportName, FileName, MyUser.ExcelHeader))
                {

                }
            }


        #endregion

        
        #region Methods

            private void GetItemStockReport()
            {
                Lbl_Err.Text = "";
                int itemId = int.Parse(Drp_ItemName.SelectedValue);
                int Category = int.Parse(Drp_Category.SelectedValue);
                int locationId=int.Parse(Drp_LocationName.SelectedValue);
                DataSet ItemStock_DS = new DataSet();
                if (itemId != -1 && Category != -1 && locationId!=-1)
                {
                    ItemStock_DS = Myinventory.GetStockReport(itemId, Category, locationId);
                    if (ItemStock_DS != null && ItemStock_DS.Tables[0].Rows.Count > 0)
                    {
                        Grd_Items.Columns[0].Visible = true;
                        Grd_Items.Columns[1].Visible = true;
                        Grd_Items.DataSource = ItemStock_DS;
                        Grd_Items.DataBind();
                        Pnl_Show.Visible = true;
                        Grd_Items.Columns[0].Visible = false;
                        Grd_Items.Columns[1].Visible = false;
                    }
                    else
                    {
                        Lbl_Err.Text = "No report found";
                        Pnl_Show.Visible = false;
                        Grd_Items.DataSource = null;
                        Grd_Items.DataBind();
                    }
                }
                else
                {
                    Lbl_Err.Text = "No report found";
                    Pnl_Show.Visible = false;
                    Grd_Items.DataSource = null;
                    Grd_Items.DataBind();
                }

                ViewState["ItemStockReport"] = ItemStock_DS;
            }

            private void LoadItemCategoryToDropdown()
            {
                Drp_Category.Items.Clear();
                DataSet Category_Ds = new DataSet();
                int locationId = int.Parse(Drp_LocationName.SelectedValue);
                ListItem li;
                Category_Ds = Myinventory.GetAllCategory(locationId,0);
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

            private void LoadItemNameToDropdown()
            {
                Drp_ItemName.Items.Clear();
                DataSet Item_Ds = new DataSet();
                ListItem li;
                int category = int.Parse(Drp_Category.SelectedValue);
                int locationId = int.Parse(Drp_LocationName.SelectedValue);
                if (category != -1)
                {
                    Item_Ds = Myinventory.GetAllItems(category, locationId);
                    if (Item_Ds != null && Item_Ds.Tables[0].Rows.Count > 0)
                    {
                        li = new ListItem("All", "0");
                        Drp_ItemName.Items.Add(li);
                        foreach (DataRow dr in Item_Ds.Tables[0].Rows)
                        {
                            li = new ListItem(dr["ItemName"].ToString(), dr["Id"].ToString());
                            Drp_ItemName.Items.Add(li);
                        }
                    }
                    else
                    {
                        li = new ListItem("No item found", "-1");
                        Drp_ItemName.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("No item found", "-1");
                    Drp_ItemName.Items.Add(li);
                }
            }

            private void LoadLocationToDropDown()
            {
                DataSet Location_Ds = new DataSet();
                ListItem li;
                Drp_LocationName.Items.Clear();
                Location_Ds = Myinventory.GetLocationName();
                if (Location_Ds != null && Location_Ds.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("All", "0");
                    Drp_LocationName.Items.Add(li);
                    foreach (DataRow dr in Location_Ds.Tables[0].Rows)
                    {
                        //Id,Locationname
                        li = new ListItem(dr["Locationname"].ToString(), dr["Id"].ToString());
                        Drp_LocationName.Items.Add(li);
                    }

                }
                else
                {
                    li = new ListItem("No Location Found", "-1");
                    Drp_LocationName.Items.Add(li);

                }

            }

        #endregion

   

      

    
    }
}
