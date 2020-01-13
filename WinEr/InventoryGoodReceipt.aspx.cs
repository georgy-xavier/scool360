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
    public partial class InventoryGoodReceipt : System.Web.UI.Page
    {
        private DataSet Dt_goodsItemsList;
        private KnowinUser MyUser;
        private WinBase.Inventory Myinventory;
        private int LocationId;

        protected void Page_Load(object sender, EventArgs e)
        {
            WC_selectItem.EventSelection += new EventHandler(OnItemSelected);
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];

            Myinventory = MyUser.GetInventoryObj();
            if (Myinventory == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(131))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadInitial();
                }
            }
        }

        private void LoadInitial()
        {
            Session["Dt_goodsItemsList"] = null;
            Txt_purchasedate.Text = General.GerFormatedDatVal(System.DateTime.Now);
            LoadSaleLocation();

            if (Session["SelectedLocation"] != null)
            {
                int.TryParse(Session["SelectedLocation"].ToString(), out LocationId);
                drp_location.SelectedValue = LocationId.ToString();

            }
            LoadVendor();
            WC_selectItem.IsAdjustment(0);
            WC_selectItem.SetCategoryType(0);
            WC_selectItem.SetNeedItemCount(1);
            WC_selectItem.IsPurchaze(1);
         
            Btn_purchase.Enabled = false;
            lbl_error.Text = "Please add items to list for purchase";

        }
        private void LoadVendor()
        {
            Drp_SelectVendor.Items.Clear();
            //Drp_SaleVendor.Items.Clear();
            DataSet Vendor_Ds = new DataSet();
            ListItem li;
            Vendor_Ds = Myinventory.GetVendordetails();
            if (Vendor_Ds != null && Vendor_Ds.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("- Select Vendor - ", "0");
                Drp_SelectVendor.Items.Add(li);
                // Drp_SaleVendor.Items.Add(li);
                foreach (DataRow dr in Vendor_Ds.Tables[0].Rows)
                {
                    //Id, tblinv_vendor.Name 
                    li = new ListItem(dr["Name"].ToString(), dr["Id"].ToString());
                    Drp_SelectVendor.Items.Add(li);
                    // Drp_SaleVendor.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("-No vendor exist-", "-1");
                Drp_SelectVendor.Items.Add(li);
                // Drp_SaleVendor.Items.Add(li);
            }
        }
        private void LoadSaleLocation()
        {
            drp_location.Items.Clear();
            DataSet locationds = Myinventory.GetActiveLocationNameForMoveItem();
            if (locationds != null && locationds.Tables[0].Rows.Count > 0)
            {
                ListItem li;
                li = new ListItem("- Inventory Location -", "0");
                drp_location.Items.Add(li);
                foreach (DataRow dr in locationds.Tables[0].Rows)
                {
                    //Id,Locationname 
                    li = new ListItem(dr["Locationname"].ToString(), dr["Id"].ToString());
                    drp_location.Items.Add(li);

                }


            }
            else
            {
                ListItem li = new ListItem("No location found", "-1");
                drp_location.Items.Add(li);
            }
        }

        protected void OnItemSelected(object sender, EventArgs e)
        {
            lbl_error.Text = "";
            Grd_purchase.Columns[0].Visible = true;
            Grd_purchase.Columns[1].Visible = true;
            lbl_error.Text = "";
          
            int Id = WC_selectItem.SelectedId;
            int temp = 0;

            if (WC_selectItem.SelectedId != 0 || WC_selectItem.Totalcount != 0)
            {
                string _sql = "";
                _sql = "select ItemName,tblinv_item.id,Stock,tblinv_item.Category ,tblinv_item.Cost, tblinv_category.Category as Categoryname, tblinv_item.Description from tblinv_item inner join tblinv_category on tblinv_category.Id= tblinv_item.Category inner join tblinv_locationitemstock on tblinv_item.Id= tblinv_locationitemstock.ItemId  where  tblinv_locationitemstock.LocationId=" + Session["SelectedLocation"].ToString() + " and   tblinv_item.id='" + WC_selectItem.SelectedId + "'  ";

                DataSet UnConsumableDs = Myinventory.m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
                DataRow dr;
                if (UnConsumableDs != null && UnConsumableDs.Tables != null && UnConsumableDs.Tables[0].Rows.Count > 0)
                {
                   
                    int count = 0;
                    double cost = 0;
                    double total = 0;
                    if (WC_selectItem.GetNeedItemCount() == 1)
                    {

                        count = WC_selectItem.Totalcount;

                        Dt_goodsItemsList = (DataSet)Session["Dt_goodsItemsList"];
                        foreach (DataRow dr1 in UnConsumableDs.Tables[0].Rows)
                        {
                              if (Dt_goodsItemsList != null && Dt_goodsItemsList.Tables != null && Dt_goodsItemsList.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow drnew in Dt_goodsItemsList.Tables[0].Rows)
                                    {
                                        if (int.Parse(drnew["Id"].ToString()) == int.Parse(dr1["Id"].ToString()))
                                        {
                                            temp = 1;
                                           
                                            count = int.Parse(drnew["Count"].ToString()) + count;
                                            total = double.Parse(dr1["Cost"].ToString()) * count;
                                          
                                                drnew["Count"] = count.ToString();
                                                drnew["Total"] = total.ToString();

                                        }
                                    }
                                }
                                if (temp == 0)
                                {
                                    Dt_goodsItemsList = getDataSet();
                                    dr = Dt_goodsItemsList.Tables["Dt_goodsItemsList"].NewRow();
                                    dr["id"] = dr1["Id"].ToString();
                                    dr["Category"] = dr1["Category"].ToString();
                                    dr["Categoryname"] = dr1["Categoryname"].ToString();
                                    
                                    dr["ItemName"] = dr1["ItemName"].ToString().Replace("&amp;", "&"); ;
                                 
                                    dr["Count"] = count.ToString();
                                    dr["Amount"] = dr1["Cost"].ToString();
                                    total = double.Parse(dr1["Cost"].ToString()) * count;
                                    dr["Total"] = total.ToString();


                                    Dt_goodsItemsList.Tables["Dt_goodsItemsList"].Rows.Add(dr);
                                }
                            
                        }

                    }
                    Session["Dt_goodsItemsList"] = null;
                    Session["Dt_goodsItemsList"] = Dt_goodsItemsList;

                    Grd_purchase.DataSource = Dt_goodsItemsList;
                    Grd_purchase.DataBind();

                    double totalamt = 0;

                    foreach (GridViewRow gr in Grd_purchase.Rows)
                    {
                        totalamt = totalamt + double.Parse(gr.Cells[6].Text);

                    }
                    Txt_PurchasingCost.Text = (Math.Round(totalamt, 2)).ToString();
                }
                else
                {
                    lbl_error.Text = "Selected item does not found in selected location";
                }
                Grd_purchase.Columns[0].Visible = false;
                Grd_purchase.Columns[1].Visible = false;
                Btn_purchase.Enabled = true;
            }
        }
        protected void Grd_purchase_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grd_purchase.Columns[0].Visible = true;
            Grd_purchase.Columns[1].Visible = true;

            int selectedrowindex = Grd_purchase.SelectedIndex;

            int _id = int.Parse(Grd_purchase.Rows[selectedrowindex].Cells[0].Text);

            Dt_goodsItemsList = (DataSet)Session["Dt_goodsItemsList"];

            if (Dt_goodsItemsList != null && Dt_goodsItemsList.Tables != null && Dt_goodsItemsList.Tables[0].Rows.Count > 0)
            {
                if (Dt_goodsItemsList.Tables[0].Rows.Count == 1)
                {
                    Grd_purchase.DataSource = null;
                    Grd_purchase.DataBind();
                    Dt_goodsItemsList.Tables[0].Rows.Clear();
                    Session["Dt_goodsItemsList"] = Dt_goodsItemsList;
                }
                else
                {
                    Dt_goodsItemsList.Tables[0].Rows[selectedrowindex].Delete();
                    Grd_purchase.DataSource = Dt_goodsItemsList;
                    Grd_purchase.DataBind();
                    Session["Dt_goodsItemsList"] = Dt_goodsItemsList;
                }


                double totalamt = 0;

                if (Grd_purchase.Rows.Count > 0)
                {
                    foreach (GridViewRow gr in Grd_purchase.Rows)
                    {
                        totalamt = totalamt + double.Parse(gr.Cells[6].Text);

                    }
                    Txt_PurchasingCost.Text = (Math.Round(totalamt, 2)).ToString();
                }
                else
                {
                    Txt_PurchasingCost.Text = "0";
                    lbl_error.Text = "Item details does not found";
                }
            }
            Grd_purchase.Columns[0].Visible = false;
            Grd_purchase.Columns[1].Visible = false;
        }

        protected void drp_location_SelectedIndexChanged(object sender, EventArgs e)
        {
            WC_selectItem.IsAdjustment(0);
            WC_selectItem.SetCategoryType(0);
            WC_selectItem.SetNeedItemCount(1);
            WC_selectItem.IsPurchaze(1);
            Session["SelectedLocation"] = drp_location.SelectedValue;
            WC_selectItem.loadlocations();

            Session["Dt_goodsItemsList"] = null;
            Grd_purchase.DataSource = null;
            Grd_purchase.DataBind();

        }

        private DataSet getDataSet()
        {
            if (Session["Dt_goodsItemsList"] == null)
            {
                Dt_goodsItemsList = new DataSet();

                DataTable dt;

                Dt_goodsItemsList.Tables.Add(new DataTable("Dt_goodsItemsList"));
                dt = Dt_goodsItemsList.Tables["Dt_goodsItemsList"];
                dt.Columns.Add("id");
                dt.Columns.Add("Category");
                dt.Columns.Add("ItemName");
                dt.Columns.Add("Categoryname");                
             
                dt.Columns.Add("Count");
                dt.Columns.Add("Amount");
                dt.Columns.Add("Total");


            }
            else
            {
                TextBox tct_descr = new TextBox();

                DataRow dr;
                Dt_goodsItemsList = (DataSet)Session["Dt_goodsItemsList"];
                Dt_goodsItemsList.Tables[0].Rows.Clear();
                foreach (GridViewRow gr in Grd_purchase.Rows)
                {


                    dr = Dt_goodsItemsList.Tables["Dt_goodsItemsList"].NewRow();
                    dr["id"] = gr.Cells[0].Text.ToString();
                    dr["Category"] = gr.Cells[1].Text.ToString();
                    dr["Categoryname"] = gr.Cells[3].Text.ToString();
                    dr["ItemName"] = gr.Cells[2].Text.ToString();
                 
                    dr["Count"] = gr.Cells[4].Text.ToString();

                    dr["Amount"] = gr.Cells[5].Text.ToString();
                    dr["Total"] = gr.Cells[6].Text.ToString();

                    Dt_goodsItemsList.Tables["Dt_goodsItemsList"].Rows.Add(dr);
                }
            }
            return Dt_goodsItemsList;

        }
        protected void Lnk_AddVendor_Click(object sender, EventArgs e)
        {
            ClearVendor();
            Lbl_VendorErr.Text = "";

            MPE_AddVendor.Show();
            // Lnk_AddNewItem.Visible = true;
            Lnk_AddVendor.Visible = true;
            //Img_AddVendor.Visible = true;
            // Img_Add.Visible = true;
        }

        protected void Btn_VendorCancel_Click(object sender, EventArgs e)
        {

        }

        protected void Btn_VendorSave_Click(object sender, EventArgs e)
        {

            string vendorname = Txt_VendorName.Text;
            string City = Txt_City.Text;
            string Address = Txt_Address.Text;
            string Email = Txt_Email.Text;
            string Mobilenum = Txt_MobileNumber.Text;
            Myinventory.SaveVendordetails(vendorname, City, Address, Email, Mobilenum);
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Inventory :  New vendor", "Vendor " + vendorname + " is added", 1);
            Lbl_VendorErr.Text = "New vendor added successfully";
            ClearVendor();
            LoadVendor();
            MPE_AddVendor.Show();

        }
        private void ClearVendor()
        {
            Txt_VendorName.Text = "";
            Txt_City.Text = "";
            Txt_Address.Text = "";
            Txt_Email.Text = "";
            Txt_MobileNumber.Text = "";
        }

        protected void Btn_PurchaseCancel_Click(object sender, EventArgs e)
        {
            Grd_purchase.DataSource = null;
            Grd_purchase.DataBind();
            Drp_SelectVendor.SelectedIndex = 0;
            txt_purchaseDescription.Text = "";
           
        }

        protected void Btn_purchase_Click(object sender, EventArgs e)
        {
            Grd_purchase.Columns[0].Visible = true;
            Grd_purchase.Columns[1].Visible = true;

            int _ChkCount = 0;
            int value = 0;
            int grdvalue = 0;
            int supplier = 0;
            int _ItemId = 0, _NewStock = 0, _CurrentStock = 0, _Category = 0;
            string _Description = "", _CreatedUser = "", _ItemName = "", comment = "";
            _CreatedUser = MyUser.UserName;
            double totalcost = 0.0;
            int save = 0;
            DateTime Today = DateTime.Now;
            DateTime _PurchaseDate = new DateTime();
            if (Txt_purchasedate.Text != "")
            {
                _PurchaseDate = MyUser.GetDareFromText(Txt_purchasedate.Text);
            }
            if (int.Parse(Drp_SelectVendor.SelectedValue) == 0)
            {
                lbl_error.Text="Please select vendor";
                value = 1;
            }
            else if (int.Parse(Drp_SelectVendor.SelectedValue) == -1)
            {
                value = 1;
               lbl_error.Text="No vendor exist,Add new vendor";
            }
            else if (_PurchaseDate > System.DateTime.Now.Date)
            {
                lbl_error.Text="Date should not be greater than today's date";
                value = 1;
            }
            if (value == 0)
            {
                foreach (GridViewRow gv in Grd_purchase.Rows)
                {
                   
                   
                    _ChkCount++;
                    _ItemName = gv.Cells[2].Text;
                    if (gv.Cells[4].Text == "")
                    {
                        lbl_error.Text="Enter count for all selected items!";
                        grdvalue = 1;
                        break;
                    }
                    else if (gv.Cells[4].Text == "0")
                    {
                        lbl_error.Text="Enter count for all selected items!";
                        grdvalue = 1;
                        break;
                    }
                    else if (gv.Cells[5].Text == "")
                    {
                       lbl_error.Text="Enter cost for " + _ItemName + "!";
                        grdvalue = 1;
                        break;
                    }
                    
                   
                }
            }
            if (value == 0 && grdvalue == 0)
            {

                if (txt_purchaseDescription.Text.Trim().Length > 250)
                    _Description = txt_purchaseDescription.Text.Trim().Substring(0, 249);
                else
                    _Description = txt_purchaseDescription.Text.Trim();

                try
                {
                    foreach (GridViewRow gv in Grd_purchase.Rows)
                    {
                       
                       
                        _ItemId = int.Parse(gv.Cells[0].Text);
                        _ItemName = gv.Cells[2].Text.Replace("&amp;", "&");
                        if (gv.Cells[4].Text != "")
                        {
                            _NewStock = int.Parse(gv.Cells[4].Text.Trim());
                        }
                       
                        if (Txt_purchasedate.Text != "")
                        {
                            _PurchaseDate = MyUser.GetDareFromText(Txt_purchasedate.Text);
                        }
                        else
                        {
                            _PurchaseDate = Today;
                        }
                        if (txt_purchaseDescription.Text != "")
                        {
                            comment = txt_purchaseDescription.Text;
                        }
                        if (gv.Cells[6].Text != "")
                        {
                            totalcost = double.Parse(gv.Cells[6].Text);
                        }

                        //_CurrentStock = int.Parse(gv.Cells[4].Text);
                        _Category = int.Parse(gv.Cells[1].Text);
                        int spec = 0;
                        supplier = int.Parse(Drp_SelectVendor.SelectedValue);
                        //ItemId,ItemName,CategoryId,Quantity,TotalCost,ActionType,ActionDate,ReferenceId,ReferenceType,StoreId,Description,Comment,CreatedUser
                        // SaveInflowItemDetailsToTable(_ItemId, _ItemName, _Category, _NewStock, _Description, _PurchaseDate, _CreatedUser, _CurrentStock);
                        try
                        {
                            save = 1;

                            Myinventory.m_MysqlDb.MyBeginTransaction();
                            Myinventory.AddLocationstock(_ItemId, _NewStock);
                            int opnqty = 0;
                            double purchasingCost = double.Parse(gv.Cells[5].Text);
                            Myinventory.UpdateOpenQuantity(_ItemId, out opnqty);
                            Myinventory.PurchaseItem(_ItemId, _ItemName, _Category, _NewStock, _PurchaseDate, 1, _Description, _CreatedUser, spec, comment, totalcost, supplier, opnqty, purchasingCost);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Inventory :  Purchase Item", "Item " + _ItemName + " is purchased from " + supplier + ", Count is " + _NewStock + ",Cost is " + totalcost + "", 1, Myinventory.m_MysqlDb);
                            Myinventory.m_MysqlDb.TransactionCommit();
                            Session["Dt_goodsItemsList"] = null;
                           
                        }
                        catch
                        {
                            lbl_error.Text="Please refresh the page and try again...";
                            Myinventory.m_MysqlDb.TransactionRollback();
                        }


                        //Clear();
                        //LoadInitialStage();
                        //FillItemGrid("", 0);
                        //LoadCategoryToDropDown(0);

                        
                    }

                  
                    if (save == 1)
                    {
                        string suppliername = Drp_SelectVendor.SelectedItem.Text;
                        string _date = Txt_purchasedate.Text;
                        LoadGoodReport();
                        double Totalcost = double.Parse(Txt_PurchasingCost.Text);
                        Session["SupplierName"] = suppliername;
                        ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "openIncpopup('GoodReceiptReport.aspx?Date=" + _date + "&totalcost=" + Totalcost + "');", true);
                        Txt_PurchasingCost.Text = "";
                        lbl_error.Text = "Item purchased successfully,Would you like to purchase more items?!";
                        Session["Dt_goodsItemsList"] = null;
                        //Hdn_Row.Value = "-1";
                       // MPE_IssueMSG.Show();
                        //WC_MessageBox.ShowMssage("Item purchased successfully,Do you want to purchase more items!");
                        Grd_purchase.DataSource = null;
                        Grd_purchase.DataBind();
                        Drp_SelectVendor.SelectedIndex = 0;
                        txt_purchaseDescription.Text = "";
                       
                    }
                }
                catch (Exception error)
                {
                    lbl_error.Text = error.Message.ToString();
                }
              
            }  Grd_purchase.Columns[0].Visible = false;
                Grd_purchase.Columns[1].Visible = false;

        }

        private void LoadGoodReport()
        {

            DataSet GoodsreportDs = new DataSet();
            DataRow _dr;
            DataTable _dt;
            GoodsreportDs.Tables.Add(new DataTable("Goodsreport"));
            _dt = GoodsreportDs.Tables["Goodsreport"];
            _dt.Columns.Add("ItemName");
            _dt.Columns.Add("Cost");
            _dt.Columns.Add("Count");
            _dt.Columns.Add("PurchasingCost");
            string itemname = "";
            double cost = 0.0;
            int itemcount = 0;
            CheckBox chk = new CheckBox();
            foreach (GridViewRow gr in Grd_purchase.Rows)
            {

              
                itemname = gr.Cells[2].Text;
                if (gr.Cells[6].Text != "")
                {
                    cost = double.Parse(gr.Cells[6].Text);
                }
                if (gr.Cells[4].Text != "")
                {
                    itemcount = int.Parse(gr.Cells[4].Text);
                }

                _dr = GoodsreportDs.Tables["Goodsreport"].NewRow();
                _dr["ItemName"] = itemname.Replace("&amp;", "&");
                _dr["Cost"] = cost;
                _dr["PurchasingCost"] = gr.Cells[5].Text;
                _dr["Count"] = itemcount;
                //      dr["TotalStock"] = gv.Cells[5].Text;
                GoodsreportDs.Tables["Goodsreport"].Rows.Add(_dr);

              //  _InflowStock.Text = "";
              //  _Cost.Text = "";


            }
            Session["Goodsreport"] = GoodsreportDs;
        }

    }
}
