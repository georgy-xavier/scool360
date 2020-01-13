using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using System.Data.Odbc;
using System.Data;

namespace WinEr.WebControls
{
    public partial class InvItemSelectionControl : System.Web.UI.UserControl
    {
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet = null;
    
        private WinBase.Inventory Myinventory;

        private DataSet Dt_SaleItemsList;
        private int LocationId=0;
        public event EventHandler EventSelection;

        private int SelectedItemId = 0;
        private int SelectedCount = 0;
        private int NeedCount = 0;
        private int Category ;
        private int IsPurchazing = 0;
        private int IsItemAdjustment = 0;
        private string itemadjustmentmode = "";
      
        protected void Page_Load(object sender, EventArgs e)
        {
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
                    LocationId = 0;

                    if (Session["SelectedLocation"] != null)
                        int.TryParse(Session["SelectedLocation"].ToString(), out LocationId);
                    else
                    {
                        lblMsg.Text = "Select Location";
                        DisableContols();
                    }


                    if (int.Parse(hnd_ispurchase.Value) == 1)
                        Txt_itemName_AutoCompleteExtender.ContextKey = "0" + "\\" + Category.ToString();
                    else
                    Txt_itemName_AutoCompleteExtender.ContextKey = LocationId.ToString() + "\\" + Category.ToString(); 

                    loadInitial();
                    Txt_itemName.Focus();
                }
            }
        }

       

        public int Totalcount
        {
            get
            {
                return SelectedCount;
            }
           
        }

        public int SelectedId
        {

            get
            {
                return SelectedItemId;
            }
        }

        public void   IsPurchaze(int IsPurchazing)
        {
            hnd_ispurchase.Value = IsPurchazing.ToString();

            if (int.Parse(hnd_ispurchase.Value) == 1)
                Txt_itemName_AutoCompleteExtender.ContextKey = "0" + "\\" + Category.ToString();
        }

        public void SetNeedItemCount(int NeedItemCount)
        {
            NeedCount = NeedItemCount;
            hdn_Needcount.Value = NeedItemCount.ToString();

        }

        public int GetNeedItemCount()
        {
            return int.Parse(hdn_Needcount.Value);
        }

        public void SetCategoryType(int CategoryType)
        {
            Category = CategoryType;
            hdncategory.Value = Category.ToString();
        }

        public int  GetCategoryType()
        {      
            return int.Parse(hdncategory.Value);
            
        }
        public string getSelectedAdjustmentMode()
        {
            return itemadjustmentmode;
        }


        public void IsAdjustment(int value)
        {
            IsItemAdjustment = value;
            hdn_isItemAdjustment.Value = value.ToString();
          
            if (value == 1)
            {
                AdjustmentArea.Visible = true;
                adjustmentPopuparea.Visible = true;
            }
            else
            {
                AdjustmentArea.Visible = false;
                adjustmentPopuparea.Visible = false;
            }

        }

        public void loadlocations()
        {
            lblMsg.Text = "";
            if ( Session["SelectedLocation"] != null)
                int.TryParse(Session["SelectedLocation"].ToString(), out LocationId);

            if (LocationId > 0)
            {
                if (int.Parse(hnd_ispurchase.Value) == 1)
                    Txt_itemName_AutoCompleteExtender.ContextKey = "0" + "\\" + Category.ToString();
                else
                    Txt_itemName_AutoCompleteExtender.ContextKey = LocationId.ToString() + "\\" + Category.ToString();

                EnableControls();
                lblMsg.Text = "";
                hdnCount.Value = "0";
                hdnItemId.Value = "0";
                lblName.Text = "";
                lblCategory.Text = "";
                lblCount.Text = "";
                Itemdetails.Visible = false;
                Txt_itemName.Focus();
            }
            else
            {
                DisableContols();
                lblMsg.Text = "";
                hdnCount.Value = "0";
                hdnItemId.Value = "0";
                lblName.Text = "";
                lblCategory.Text = "";
                lblCount.Text = "";
                Itemdetails.Visible = false;
                Txt_itemName.Focus();
            }
        }

        private void EnableControls()
        {
            Txt_itemName.Enabled = true;
            Txt_sale_count.Enabled = true;
            btn_Add.Enabled = true;
           
           
        }

        private void DisableContols()
        {
            Txt_itemName.Enabled = false;
            Txt_sale_count.Enabled = false;
            btn_Add.Enabled = false;
           
        }

        private void loadInitial()
        {
            //Itemdetails.Visible = false;

            //string sql="select Value from tblconfiguration where Module='Inventory' and Name='CountInSelectionControl'";
            //DataSet dt=Myinventory.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            
            //if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
            //{
            if (hdn_Needcount.Value.ToString() == "0")
            {
                hdn_Needcount.Value = "0";
                countrow.Visible = false;
                NeedCount = 0;
                
            }
            else
            {
                hdn_Needcount.Value = "1";
                countrow.Visible = true;
                NeedCount = 1;
            }
            Itemdetails.Visible = false;
           
            //}
        
        }

        protected void Txt_sale_count_TextChanged(object sender, EventArgs e)
        {
            if (hdn_isItemAdjustment.Value == "1")
            {

                Rdb_Type.Focus();
            }
            else
            {
                btn_Add.Focus();
            }
        }

        protected void ItemSelected_TextChanged(object sender, EventArgs e)
        {
            lblMsg.Text="";
            string _sql = "select ItemName,tblinv_item.id,Stock,tblinv_item.Category ,tblinv_item.Cost,tblinv_category.Category as catname from tblinv_item inner join tblinv_category on tblinv_category.Id= tblinv_item.Category inner join tblinv_locationitemstock on tblinv_item.Id= tblinv_locationitemstock.ItemId  where   ItemName='" + Txt_itemName.Text + "'  ";
            if (int.Parse(hnd_ispurchase.Value) != 1)
                _sql = _sql + "  and tblinv_locationitemstock.LocationId=" + Session["SelectedLocation"];
            if (int.Parse( hdncategory.Value ) > 0)
                _sql = _sql + " and  tblinv_category.Categorytype=" + hdncategory.Value;


            DataSet   UnConsumableDs = Myinventory.m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
            if (UnConsumableDs != null && UnConsumableDs.Tables != null && UnConsumableDs.Tables[0].Rows.Count > 0)
            {
                Itemdetails.Visible = true;
                lblName.Text = UnConsumableDs.Tables[0].Rows[0]["ItemName"].ToString();
                hdnCount.Value= lblCount.Text = UnConsumableDs.Tables[0].Rows[0]["Stock"].ToString();
                hdnItemId.Value = UnConsumableDs.Tables[0].Rows[0]["id"].ToString();
                lblCategory.Text = UnConsumableDs.Tables[0].Rows[0]["catname"].ToString();
                if (hdn_Needcount.Value == "1")
                    Txt_sale_count.Focus();
                else
                    btn_Add.Focus();
            }
            else
            {
                
                hdnCount.Value = "0";
                hdnItemId.Value = "0";
                lblName.Text = "";
                lblCategory.Text = "";
                lblCount.Text = "";
                Itemdetails.Visible = false;
                lblMsg.Text = "Item details does not found";
                Txt_itemName.Focus();
            }
      
        }

        protected void btn_Add_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (!String.IsNullOrEmpty(Txt_itemName.Text))
            {
                if (hdn_Needcount.Value == "1")
                {
                    int count = 0;
                    int available = 0;
                    int.TryParse(hdnCount.Value, out available);
                    int.TryParse(Txt_sale_count.Text.Trim(), out count);
                    if (count <= 0)
                    {
                        lblMsg.Text = "Count should be greater that 0";
                        Txt_sale_count.Focus();
                    }
                    else if (hdn_isItemAdjustment.Value == "1" && Rdb_Type.SelectedValue == "")
                    {
                        lblMsg.Text = "Please select adjustment mode";
                        Rdb_Type.Focus();
                    }
                    else if (hdn_isItemAdjustment.Value == "1" && Rdb_Type.SelectedValue == "1" && count > available)
                    {
                        lblMsg.Text = "Count should be less than or equal to available count";
                        Txt_sale_count.Focus();
                       
                    }
                    else if (hnd_ispurchase.Value == "0" && hdn_isItemAdjustment.Value == "0" && count > available)
                    {
                        lblMsg.Text = "Count should be less than or equal to available count";
                        Txt_sale_count.Focus();
                    }
                    
                    else
                    {
                        SelectedItemId = int.Parse(hdnItemId.Value);
                        SelectedCount = count;
                        NeedCount = int.Parse(hdn_Needcount.Value);

                        if (int.Parse(hdn_isItemAdjustment.Value) == 1)
                        {
                            itemadjustmentmode = Rdb_Type.SelectedValue;
                        }

                        EventSelection(this, e);

                        Txt_itemName.Text = "";
                        Txt_sale_count.Text = "";
                        hdnCount.Value = "0";
                        hdnItemId.Value = "0";
                        lblName.Text = "";
                        lblCategory.Text = "";
                        lblCount.Text = "";
                        Txt_itemName.Focus();
                        Itemdetails.Visible = false;

                    }

                }
                else
                {
                    SelectedItemId = int.Parse(hdnItemId.Value);

                    EventSelection(this, e);
                    Txt_itemName.Text = "";
                    Txt_sale_count.Text = "";
                    hdnCount.Value = "0";
                    hdnItemId.Value = "0";
                    lblName.Text = "";
                    lblCategory.Text = "";
                    lblCount.Text = "";
                    Txt_itemName.Focus();
                    Itemdetails.Visible = false;

                }

            }
            else
            {
                lblMsg.Text = "Please enter the item name";
            }


        }

        protected void lnk_PickItem_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (Session["SelectedLocation"] != null)
            {
                LoadCategory();
                loadItem(int.Parse(Drp_category.SelectedValue));
                ItemdetailsPopup.Visible = false;
                txt_popupcount.Text = "0";
                MPE_SelectItem.Show();
                Drp_category.Focus();
            }
            else
            {
                lblMsg.Text = "Select location";
            }

        }

        private void LoadCategory()
        {
            Drp_category.Items.Clear();

            if ( Session["SelectedLocation"] != null)
            {
                DataSet dt= Myinventory.GetAllCategory(int.Parse(Session["SelectedLocation"].ToString()), int.Parse(hdncategory.Value));

                if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
                {
                    if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
                    {
                        Drp_category.Items.Add(new ListItem("Select Category", "0"));
                        foreach (DataRow dr in dt.Tables[0].Rows)
                        {
                            Drp_category.Items.Add(new ListItem(dr["Category"].ToString(), dr["Id"].ToString()));

                        }
                    }
                }
                else
                {
                    Drp_category.Items.Add(new ListItem("Category does not found", "-1"));
                        
                }
                Drp_category.SelectedIndex = 0;
            }
        }

        protected void Btn_AddItem_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            lbl_popuperror.Text = "";
            if (hdn_Needcount.Value == "1")
            {
                int count = 0;
                int available = 0;
                int.TryParse(hdnCount.Value, out available);
                int.TryParse(txt_popupcount.Text.Trim(), out count);
                if (count <= 0)
                {
                    lbl_popuperror.Text = "Count should be greater that 0";
                    txt_popupcount.Focus();
                    MPE_SelectItem.Show();
                }
                else if (hdn_isItemAdjustment.Value == "1" && rdoPopupAdjustment.SelectedValue == "")
                {
                    lbl_popuperror.Text = "Please select adjustment mode";
                    rdoPopupAdjustment.Focus();
                    MPE_SelectItem.Show();
                }
                else if (hdn_isItemAdjustment.Value == "1" && rdoPopupAdjustment.SelectedValue =="1" && count > available)
                {
                    lbl_popuperror.Text = "Count should be less than or equal to available count";
                    txt_popupcount.Focus();
                    MPE_SelectItem.Show();
                }
                else if (hnd_ispurchase.Value == "0" && hdn_isItemAdjustment.Value == "0" && count > available)
                {
                    lbl_popuperror.Text = "Count should be less than or equal to available count";
                    txt_popupcount.Focus();
                    MPE_SelectItem.Show();
                }
               
                else
                {
                    SelectedItemId = int.Parse(hdnItemId.Value);
                    SelectedCount = count;
                    NeedCount = int.Parse(hdn_Needcount.Value);

                    if (int.Parse(hdn_isItemAdjustment.Value) == 1)
                    {
                        itemadjustmentmode = rdoPopupAdjustment.SelectedValue;
                    }


                    EventSelection(this, e);

                    Drp_category.SelectedValue = "0";
                    Drp_ItemName.SelectedValue = "0";

                    hdnCount.Value = "0";
                    hdnItemId.Value = "0";
                    lbl_popup_itemnName.Text = "";
                    Lbl_popup_categorey.Text = "";
                    lbl_popup_availablecount.Text = "";
                    Drp_category.Focus();
                    ItemdetailsPopup.Visible = false;
                    txt_popupcount.Text = "0";
                }

            }
            else
            {
                SelectedItemId = int.Parse(hdnItemId.Value);

                EventSelection(this, e);
                Txt_itemName.Text = "";
                Txt_sale_count.Text = "";
                hdnCount.Value = "0";
                hdnItemId.Value = "0";
                lbl_popup_itemnName.Text = "";
                Lbl_popup_categorey.Text = "";
                lbl_popup_availablecount.Text = "";
                Drp_category.SelectedValue = "0";
                Drp_ItemName.SelectedValue = "0";
                txt_popupcount.Text = "0";
                Drp_category.Focus();
                ItemdetailsPopup.Visible = false;

            }
            if (Chk_more.Checked)
            {
                ItemdetailsPopup.Visible = false;
                MPE_SelectItem.Show();
            }

        }

        protected void Drp_category_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_popuperror.Text = "";
            loadItem(int.Parse(Drp_category.SelectedValue));
            MPE_SelectItem.Show();

        }

        private void loadItem(int SelectedCategory)
        {
            lbl_popuperror.Text = "";
            Drp_ItemName.Items.Clear();
            if (SelectedCategory > 0)
            {
                DataSet dt = Myinventory.GetAllItems(SelectedCategory, int.Parse(Session["SelectedLocation"].ToString()));

                if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
                {
                    Drp_ItemName.Items.Add(new ListItem("Select Item", "0"));
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        Drp_ItemName.Items.Add(new ListItem(dr["ItemName"].ToString(), dr["Id"].ToString()));
                    }

                    Drp_ItemName.Focus();
                }
                else
                {

                    Drp_category.Focus();
                    lbl_popuperror.Text = "No items found";
                }
            }
            else
            {
                lbl_popuperror.Text = "Select one category";
            }
           
        }

        protected void Drp_ItemName_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            string _sql = "select ItemName,tblinv_item.id,Stock,tblinv_item.Category ,tblinv_item.Cost,tblinv_category.Category as catname from tblinv_item inner join tblinv_category on tblinv_category.Id= tblinv_item.Category inner join tblinv_locationitemstock on tblinv_item.Id= tblinv_locationitemstock.ItemId  where   tblinv_item.id=" + Drp_ItemName.SelectedValue + "  ";
            if (int.Parse(hnd_ispurchase.Value) != 1)
                _sql = _sql + "  and tblinv_locationitemstock.LocationId=" + Session["SelectedLocation"];
            if (int.Parse(hdncategory.Value) > 0)
                _sql = _sql + " and  tblinv_category.Categorytype=" + hdncategory.Value;


            DataSet UnConsumableDs = Myinventory.m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
            if (UnConsumableDs != null && UnConsumableDs.Tables != null && UnConsumableDs.Tables[0].Rows.Count > 0)
            {
                ItemdetailsPopup.Visible = true;
                lbl_popup_itemnName.Text = UnConsumableDs.Tables[0].Rows[0]["ItemName"].ToString();
                hdnCount.Value = lbl_popup_availablecount.Text = UnConsumableDs.Tables[0].Rows[0]["Stock"].ToString();
                hdnItemId.Value = UnConsumableDs.Tables[0].Rows[0]["id"].ToString();
                Lbl_popup_categorey.Text = UnConsumableDs.Tables[0].Rows[0]["catname"].ToString();
              
                if (hdn_Needcount.Value == "1")
                    txt_popupcount.Focus();
                else
                    btn_Add.Focus();
            }
            else
            {

                hdnCount.Value = "0";
                hdnItemId.Value = "0";
                lbl_popup_itemnName.Text = "";
                lblCategory.Text = "";
                Lbl_popup_categorey.Text = "";
                ItemdetailsPopup.Visible = false;
                lbl_popuperror.Text = "Item details does not found";
                Drp_category.Focus();
            }
            MPE_SelectItem.Show();
        }

        protected void Rdb_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(hdnItemId.Value) > 0)
            {
                if (int.Parse(Rdb_Type.SelectedValue) == 0)
                {
                    OdbcDataReader M_reader = null;
                    int MaxQty = 0, AvailableQty = 0, NewCount = 0;
                    string sql = "";
                    sql = "select tblinv_item.MaxQty, tblinv_item.OpnQuantity from tblinv_item where tblinv_item.Id=" + hdnItemId.Value + " ";
                    M_reader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                    if (M_reader.HasRows)
                    {
                        MaxQty = int.Parse(M_reader.GetValue(0).ToString());
                        AvailableQty = int.Parse(M_reader.GetValue(1).ToString());
                        int.TryParse(Txt_sale_count.Text, out NewCount);
                        int Totalcount = AvailableQty + NewCount;
                        if (Totalcount > MaxQty)
                        {
                            lblMsg.Text = "Item count exceeds the maximum quantity " + MaxQty + ".Do you want to continue?";
                          
                        }
                    }
                }
                else
                {
                    OdbcDataReader M_reader = null;
                    int TotalStck = 0, AvailableQty = 0, NewCount = 0;
                    string sql = "";
                    sql = "select tblinv_item.TotalStock, tblinv_item.OpnQuantity from tblinv_item where tblinv_item.Id=" + int.Parse(hdnItemId.Value) + " ";
                    M_reader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                    if (M_reader.HasRows)
                    {
                        TotalStck = int.Parse(M_reader.GetValue(0).ToString());
                        AvailableQty = int.Parse(M_reader.GetValue(1).ToString());
                        int.TryParse(Txt_sale_count.Text, out NewCount);
                        int NewQty = AvailableQty - NewCount;
                        if (NewQty < 0)
                        {
                            lblMsg.Text = "Count become minus.Do you want to continue?";
                           
                        }
                    }
                }
            }
            else
            {
                lblMsg.Text = "Please select an item";
            }
        }

        protected void rdoPopupAdjustment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(hdnItemId.Value) > 0)
            {
                if (int.Parse(rdoPopupAdjustment.SelectedValue) == 0)
                {
                    OdbcDataReader M_reader = null;
                    int MaxQty = 0, AvailableQty = 0, NewCount = 0;
                    string sql = "";
                    sql = "select tblinv_item.MaxQty, tblinv_item.OpnQuantity from tblinv_item where tblinv_item.Id=" + hdnItemId.Value + " ";
                    M_reader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                    if (M_reader.HasRows)
                    {
                        MaxQty = int.Parse(M_reader.GetValue(0).ToString());
                        AvailableQty = int.Parse(M_reader.GetValue(1).ToString());
                        int.TryParse(txt_popupcount.Text, out NewCount);
                        int Totalcount = AvailableQty + NewCount;
                        if (Totalcount > MaxQty)
                        {
                            lblMsg.Text = "Item count exceeds the maximum quantity " + MaxQty + ".Do you want to continue?";
                          
                        }
                    }
                }
                else
                {
                    OdbcDataReader M_reader = null;
                    int TotalStck = 0, AvailableQty = 0, NewCount = 0;
                    string sql = "";
                    sql = "select tblinv_item.TotalStock, tblinv_item.OpnQuantity from tblinv_item where tblinv_item.Id=" + int.Parse(hdnItemId.Value) + " ";
                    M_reader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                    if (M_reader.HasRows)
                    {
                        TotalStck = int.Parse(M_reader.GetValue(0).ToString());
                        AvailableQty = int.Parse(M_reader.GetValue(1).ToString());
                        int.TryParse(txt_popupcount.Text, out NewCount);
                        int NewQty = AvailableQty - NewCount;
                        if (NewQty < 0)
                        {
                            lblMsg.Text = "Count become minus.Do you want to continue?";
                           
                        }
                    }
                }
            }
            else
            {
                lblMsg.Text = "Please select an item";
            }
            MPE_SelectItem.Show();
        }     
        
    }
}