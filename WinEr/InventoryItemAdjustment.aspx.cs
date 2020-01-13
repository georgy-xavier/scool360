using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

namespace WinEr
{
    public partial class InventoryItemAdjustment : System.Web.UI.Page
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
            else if (!MyUser.HaveActionRignt(864))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    Row_lctnItemCount.Visible = false;
                    LoadItemToDropDown();
                    LoadLocationToDropDown();
                }
            }
        }

        protected void Btn_AdjustItem_Click(object sender, EventArgs e)
        {
            SaveDataTotable();
        }

        protected void Txt_Count_TextChanged(object sender, EventArgs e)
        {

        }

        protected void Btn_MsgCancel_Click(object sender, EventArgs e)
        {
            Txt_Count.Text = "";
        }

        protected void Rdb_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(Rdb_Type.SelectedValue) == 0)
            {
                OdbcDataReader M_reader = null;
                int MaxQty = 0, AvailableQty = 0, NewCount = 0;
                string sql = "";
                sql = "select tblinv_item.MaxQty, tblinv_item.OpnQuantity from tblinv_item where tblinv_item.Id=" + int.Parse(Drp_ItemName.SelectedValue) + " ";
                M_reader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                if (M_reader.HasRows)
                {
                    MaxQty = int.Parse(M_reader.GetValue(0).ToString());
                    AvailableQty = int.Parse(M_reader.GetValue(1).ToString());
                    int.TryParse(Txt_Count.Text, out NewCount);
                    int Totalcount = AvailableQty + NewCount;
                    if (Totalcount > MaxQty)
                    {
                        Lbl_msg.Text = "Item count exceeds the maximum quantity " + MaxQty + ".Do you want to continue?";
                        MPE_MessageBox.Show();
                    }
                }
            }
            else
            {
                OdbcDataReader M_reader = null;
                int TotalStck = 0, AvailableQty = 0, NewCount = 0;
                string sql = "";
                sql = "select tblinv_item.TotalStock, tblinv_item.OpnQuantity from tblinv_item where tblinv_item.Id=" + int.Parse(Drp_ItemName.SelectedValue) + " ";
                M_reader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                if (M_reader.HasRows)
                {
                    TotalStck = int.Parse(M_reader.GetValue(0).ToString());
                    AvailableQty = int.Parse(M_reader.GetValue(1).ToString());
                    int.TryParse(Txt_Count.Text, out NewCount);
                    int NewQty = AvailableQty - NewCount;
                    if (NewQty < 0)
                    {
                        Lbl_msg.Text = "Count become minus.Do you want to continue?";
                        MPE_MessageBox.Show();
                    }
                }
            }
        }

        protected void Drp_Location_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadItemCountByLocation();

        }

        #endregion

           

        #region Methods

            private void LoadLocationToDropDown()
            {

                Drp_Location.Items.Clear();
                DataSet locationds = Myinventory.GetActiveLocationNameForMoveItem();
                if (locationds != null && locationds.Tables[0].Rows.Count > 0)
                {
                    ListItem li;
                    li = new ListItem("- Inventory Location -", "0");
                    Drp_Location.Items.Add(li);
                    foreach (DataRow dr in locationds.Tables[0].Rows)
                    {
                        //Id,Locationname 
                        li = new ListItem(dr["Locationname"].ToString(), dr["Id"].ToString());
                        Drp_Location.Items.Add(li);
                    }


                }
                else
                {
                    ListItem li = new ListItem("No location found", "-1");
                    Drp_Location.Items.Add(li);
                }
            }

            private void SaveDataTotable()
            {
                Lbl_AdjustErr.Text = "";
                int count, availcount = 0;
                int checkeditem = 0;
                string Des = "";
                int save = 0;
                string log = "";
                int valuetype = 0;
                int ItemId = int.Parse(Drp_ItemName.SelectedValue);
                int locationid = 0;
                int.TryParse(Drp_Location.SelectedValue, out locationid);
                if (ItemId != -1)
                {
                    if (ItemId > 0)
                    {
                        int.TryParse(Txt_Count.Text, out count);
                        int.TryParse(Txt_LctnItemCount.Text, out availcount);
                        if (count > 0)
                        {
                            if (locationid > 0)
                            {
                                if (Rdb_Type.SelectedValue == "")
                                {
                                    Lbl_AdjustErr.Text = "Select any type";
                                }
                                else
                                {
                                    //    if (Rdb_Type.SelectedValue == "1" && availcount < count)
                                    //    {
                                    //        Lbl_AdjustErr.Text = "Count must be less than available stock..!";
                                    //    }
                                    //    else
                                    //    {

                                    int chkvalue = int.Parse(Rdb_Type.SelectedValue);
                                    if (chkvalue == 0)
                                    {
                                        Des = "Increased";
                                        valuetype = 5;
                                        log = "Item count is increased to " + count + "";
                                    }
                                    else
                                    {

                                        Des = "Decreased";
                                        valuetype = 6;
                                        log = "Item count is decreased to " + count + "";
                                    }
                                    try
                                    {
                                        Myinventory.m_MysqlDb.MyBeginTransaction();
                                        int locationId = int.Parse(Drp_Location.SelectedValue);
                                        Myinventory.AdjustItemCount(chkvalue, ItemId, count, locationId);
                                        int opnqty = 0;
                                        Myinventory.UpdateOpenQuantity(ItemId, out opnqty);
                                        string reason = Txt_reason.Text;
                                        string itemname = Drp_ItemName.SelectedItem.Text;
                                        string actiontype = Rdb_Type.SelectedItem.Text;
                                        Myinventory.EntryToTransaction(ItemId, itemname, count, reason, actiontype, MyUser.UserName, valuetype, opnqty, locationId);
                                        loadItemCountByLocation();
                                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "" + actiontype + "", "" + log + "", 1, Myinventory.m_MysqlDb);
                                        ClearValues();
                                        save = 1;
                                        Myinventory.m_MysqlDb.TransactionCommit();
                                    }
                                    catch
                                    {
                                        WC_MessageBox.ShowMssage("Please refresh the page and try again...");
                                        Myinventory.m_MysqlDb.TransactionRollback();
                                    }

                                    // }
                                }
                            }
                            else
                            {
                                Lbl_AdjustErr.Text = "Select location";

                            }
                        }
                        else
                        {
                            Lbl_AdjustErr.Text = "Enter Count";
                        }
                    }
                    else
                    {
                        Lbl_AdjustErr.Text = "Please select an item";
                    }
                }
                else
                {
                    Lbl_AdjustErr.Text = "No item found";
                }
                if (save == 1)
                {
                    Lbl_AdjustErr.Text = "Item count " + Des + "";
                }
            }

            private void ClearValues()
            {
                Txt_Count.Text = "";
                Txt_reason.Text = "";
                Drp_ItemName.SelectedValue = "0";
            }      

            private void LoadItemToDropDown()
            {
                DataSet Item_Ds = new DataSet();
                Drp_ItemName.Items.Clear();
                ListItem li;
                Item_Ds = Myinventory.GetAllItems(0,"");
                if (Item_Ds != null && Item_Ds.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("Select Item", "0");
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

         private void loadItemCountByLocation()
        {
            string sql = "";
            OdbcDataReader Myreader = null;
            sql = "select tblinv_locationitemstock.Stock from tblinv_locationitemstock where tblinv_locationitemstock.ItemId=" + int.Parse(Drp_ItemName.SelectedValue) + " and tblinv_locationitemstock.LocationId=" + int.Parse(Drp_Location.SelectedValue) + "";
            Myreader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
            if (Myreader.HasRows)
            {
                Row_lctnItemCount.Visible = true;
                Txt_LctnItemCount.Text = Myreader.GetValue(0).ToString();

            }
            else
            {
                Row_lctnItemCount.Visible = true;
                Txt_LctnItemCount.Text = "0";
            }
        }

        #endregion

       

      

       
     
      
    }
}
