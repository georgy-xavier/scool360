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
    public partial class InvItemAdjustment : System.Web.UI.Page
    {
        private DataSet Dt_ItemAdjustmentList;
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
            LoadSaleLocation();
            Session["Dt_ItemAdjustmentList"] = null;
            if (Session["SelectedLocation"] != null)
            {
                int.TryParse(Session["SelectedLocation"].ToString(), out LocationId);
                drp_location.SelectedValue = LocationId.ToString();

            }
            WC_selectItem.IsAdjustment(1);
            WC_selectItem.SetCategoryType(0);
            WC_selectItem.SetNeedItemCount(1);
            WC_selectItem.IsPurchaze(1);
            Btn_ItemAdjustment.Enabled = false;
            Txt_adjustDate.Text = General.GerFormatedDatVal(System.DateTime.Now);
            lbl_error.Text = "Please add items to list for adjustment";
          
        }
        private void LoadSaleLocation()
        {
            drp_location.Items.Clear();
            DataSet locationds = Myinventory.GetActiveLocationNameForMoveItem();
            if (locationds != null && locationds.Tables[0].Rows.Count > 0)
            {
                ListItem li;
                li = new ListItem(" - Inventory  Location -", "0");
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
                ListItem li = new ListItem("- No location found -", "-1");
                drp_location.Items.Add(li);
            }
        }
        protected void OnItemSelected(object sender, EventArgs e)
        {
            lbl_error.Text = "";
            Grd_adjustment.Columns[0].Visible = true;
            Grd_adjustment.Columns[1].Visible = true;
            Grd_adjustment.Columns[9].Visible = true;
            int Id = WC_selectItem.SelectedId;
          
            int temp = 0;

            if (WC_selectItem.SelectedId != 0 || WC_selectItem.Totalcount != 0)
            {
                string SelectedAdjustmentMode = WC_selectItem.getSelectedAdjustmentMode();
                string _sql = "";
                _sql = "select ItemName,tblinv_item.id,Stock,tblinv_item.Category ,tblinv_item.Cost ,tblinv_item.MinQty, tblinv_item.MaxQty from tblinv_item inner join tblinv_category on tblinv_category.Id= tblinv_item.Category inner join tblinv_locationitemstock on tblinv_item.Id= tblinv_locationitemstock.ItemId  where tblinv_locationitemstock.LocationId=" + Session["SelectedLocation"] + " and  tblinv_item.id='" + WC_selectItem.SelectedId + "'  ";

                DataSet UnConsumableDs = Myinventory.m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
                DataRow dr;
                if (UnConsumableDs != null && UnConsumableDs.Tables != null && UnConsumableDs.Tables[0].Rows.Count > 0)
                {

                    int count = 0;

                    if (WC_selectItem.GetNeedItemCount() == 1)
                    {

                        count = WC_selectItem.Totalcount;

                        Dt_ItemAdjustmentList = (DataSet)Session["Dt_ItemAdjustmentList"];
                        foreach (DataRow dr1 in UnConsumableDs.Tables[0].Rows)
                        {
                              if (Dt_ItemAdjustmentList != null && Dt_ItemAdjustmentList.Tables != null && Dt_ItemAdjustmentList.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow drnew in Dt_ItemAdjustmentList.Tables[0].Rows)
                                    {
                                        if (int.Parse(drnew["Id"].ToString()) == int.Parse(dr1["Id"].ToString()))
                                        {
                                            temp = 1;
                                            lbl_error.Text = "Item is already added. Please remove and add the item once again";

                                        }
                                    }
                                }
                                if (temp == 0)
                                {
                                    Dt_ItemAdjustmentList = getDataSet();
                                    dr = Dt_ItemAdjustmentList.Tables["AdjustmentItemList"].NewRow();
                                    
                                    dr["id"]        = dr1["Id"].ToString();
                                    dr["Category"]  = dr1["Category"].ToString();
                                    dr["ItemName"]  = dr1["ItemName"].ToString().Replace("&amp;", "&"); ;
                                    dr["Stock"]     = dr1["Stock"].ToString();
                                    
                                    if(SelectedAdjustmentMode=="0")
                                        dr["Count"] = "+"+count.ToString();
                                    else
                                        dr["Count"] = "-" + count.ToString();

                                    dr["MaxStock"] = dr1["MaxQty"].ToString();
                                    dr["MinStock"] = dr1["MinQty"].ToString();

                                    dr["AdjustmentMode"] = SelectedAdjustmentMode.ToString();

                                    if (SelectedAdjustmentMode == "0")
                                        dr["AdjustedStock"] = (int.Parse(dr1["Stock"].ToString()) + count).ToString();
                                    else
                                        dr["AdjustedStock"] = (int.Parse(dr1["Stock"].ToString()) - count).ToString();

                                    Dt_ItemAdjustmentList.Tables["AdjustmentItemList"].Rows.Add(dr);
                                }
                            
                        }

                    }
                    Session["Dt_ItemAdjustmentList"] = null;
                    Session["Dt_ItemAdjustmentList"] = Dt_ItemAdjustmentList;

                    Grd_adjustment.DataSource = Dt_ItemAdjustmentList;
                    Grd_adjustment.DataBind();

                 
                }
                else
                {
                    lbl_error.Text = "Selected item does not found in selected location";
                }
                Grd_adjustment.Columns[0].Visible = false;
                Grd_adjustment.Columns[1].Visible = false;
                Grd_adjustment.Columns[9].Visible = false;
                Btn_ItemAdjustment.Enabled = true;
            }

        }
        private DataSet getDataSet()
        {
            if (Session["Dt_ItemAdjustmentList"] == null)
            {
                Dt_ItemAdjustmentList = new DataSet();

                DataTable dt;

                Dt_ItemAdjustmentList.Tables.Add(new DataTable("AdjustmentItemList"));
                dt = Dt_ItemAdjustmentList.Tables["AdjustmentItemList"];
                dt.Columns.Add("id");
                dt.Columns.Add("Category");
                dt.Columns.Add("ItemName");
                dt.Columns.Add("Stock");
                dt.Columns.Add("Count");
                dt.Columns.Add("MaxStock");
                dt.Columns.Add("MinStock");
                dt.Columns.Add("AdjustmentMode");
                dt.Columns.Add("AdjustedStock"); 
                
                 
            }
            else
            {
                TextBox tct_descr = new TextBox();

                DataRow dr;

                Dt_ItemAdjustmentList = (DataSet)Session["Dt_ItemAdjustmentList"];
                Dt_ItemAdjustmentList.Tables[0].Rows.Clear();
               
                RadioButtonList rdo_btn_type=new RadioButtonList();
                foreach (GridViewRow gr in Grd_adjustment.Rows)
                {
                   
                   

                    dr = Dt_ItemAdjustmentList.Tables["AdjustmentItemList"].NewRow();
                    dr["id"] = gr.Cells[0].Text.ToString();
                    dr["Category"] = gr.Cells[1].Text.ToString();
                    dr["ItemName"] = gr.Cells[2].Text.ToString();
                    dr["Stock"] = gr.Cells[3].Text.ToString();
                    dr["Count"] =  gr.Cells[6].Text.ToString();
                    dr["MaxStock"] = gr.Cells[4].Text.ToString();
                    dr["MinStock"] = gr.Cells[5].Text.ToString();
                    dr["AdjustmentMode"] = gr.Cells[9].Text.ToString();
                    dr["AdjustedStock"] =  gr.Cells[7].Text.ToString();
                    Dt_ItemAdjustmentList.Tables["AdjustmentItemList"].Rows.Add(dr);
                }
            }
            return Dt_ItemAdjustmentList;

        }
    

        protected void drp_location_SelectedIndexChanged(object sender, EventArgs e)
        {
            WC_selectItem.IsAdjustment(1);
            WC_selectItem.SetCategoryType(0);
            WC_selectItem.IsPurchaze(1);
            WC_selectItem.SetNeedItemCount(1);

            Session["SelectedLocation"] = drp_location.SelectedValue;
            WC_selectItem.loadlocations();
            Grd_adjustment.DataSource = null;
            Grd_adjustment.DataBind();
            Session["Dt_ItemAdjustmentList"] = null;

        }     
        protected void Grd_adjustment_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grd_adjustment.Columns[0].Visible = true;
            Grd_adjustment.Columns[1].Visible = true;
            Grd_adjustment.Columns[9].Visible = true;

            int selectedrowindex = Grd_adjustment.SelectedIndex;

            int _id = int.Parse(Grd_adjustment.Rows[selectedrowindex].Cells[0].Text);

            Dt_ItemAdjustmentList = (DataSet)Session["Dt_ItemAdjustmentList"];

            if (Dt_ItemAdjustmentList != null && Dt_ItemAdjustmentList.Tables != null && Dt_ItemAdjustmentList.Tables[0].Rows.Count > 0)
            {
                if (Dt_ItemAdjustmentList.Tables[0].Rows.Count == 1)
                {
                    Grd_adjustment.DataSource = null;
                    Grd_adjustment.DataBind();
                    Dt_ItemAdjustmentList.Tables[0].Rows.Clear();
                    Session["Dt_ItemAdjustmentList"] = Dt_ItemAdjustmentList;
                }
                else
                {
                    Dt_ItemAdjustmentList.Tables[0].Rows[selectedrowindex].Delete();
                    Grd_adjustment.DataSource = Dt_ItemAdjustmentList;
                    Grd_adjustment.DataBind();
                    Session["Dt_ItemAdjustmentList"] = Dt_ItemAdjustmentList;
                }
                
            }
            Grd_adjustment.Columns[0].Visible = false;
            Grd_adjustment.Columns[1].Visible = false;
            Grd_adjustment.Columns[9].Visible = false;
        }

        protected void Btn_ItemAdjustment_Click(object sender, EventArgs e)
        {
            Grd_adjustment.Columns[0].Visible = true;
            Grd_adjustment.Columns[1].Visible = true;
            Grd_adjustment.Columns[9].Visible = true;
            
            if (IsValid())
            {
                SaveDataTotable();
            }

            Grd_adjustment.Columns[0].Visible = false;
            Grd_adjustment.Columns[1].Visible = false;
            Grd_adjustment.Columns[9].Visible = false;
        }

        private bool IsValid()
        {

            bool valid = true;

            if (Grd_adjustment.Rows.Count <= 0)
            {            
                lbl_error.Text = "Select item to be adjust";
                valid = false;
            }
            

            return valid;
        }

        private void SaveDataTotable()
        {
            lbl_error.Text = "";
            int count, availcount = 0;
         
            int save = 0;
            string log = "";
            int valuetype = 0;
            string reason = "";
            if (txt_adjustDescription.Text.Trim().Length > 250)
                reason = txt_adjustDescription.Text.Trim().Substring(0, 249);
            else
                reason = txt_adjustDescription.Text.Trim();


            int locationId = 0;
            int.TryParse(drp_location.SelectedValue, out locationId);
            string countstring="";


            foreach (GridViewRow gr in Grd_adjustment.Rows)
            {

                int ItemId = int.Parse(gr.Cells[0].Text);
                countstring = gr.Cells[6].Text.Replace('-', ' ').Replace('+', ' ').Trim().ToString();

                int.TryParse(countstring.Trim().ToString(), out count);
                int.TryParse(gr.Cells[3].Text, out availcount);
                int chkvalue = int.Parse(gr.Cells[9].Text);
                
                if (chkvalue == 0)
                {

                    valuetype = 5;
                    log = "Item count is increased by " + count + "";
                }
                else
                {


                    valuetype = 6;
                    log = "Item count is decreased by " + count + "";
                }
                try
                {
                    Myinventory.m_MysqlDb.MyBeginTransaction();

                    Myinventory.AdjustItemCount(chkvalue, ItemId, count, locationId);
                    int opnqty = 0;
                    Myinventory.UpdateOpenQuantity(ItemId, out opnqty);

                    string itemname = gr.Cells[2].Text;
                    string actiontype;
                    if (chkvalue == 0)
                        actiontype = "Adjustment/Increase";
                    else
                        actiontype = "Adjustment/Decrease";

                    Myinventory.EntryToTransaction(ItemId, itemname, count, reason, actiontype, MyUser.UserName, valuetype, opnqty, locationId);
                    //    loadItemCountByLocation();
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Inventory : " + actiontype + "", "" + log + "", 1, Myinventory.m_MysqlDb);
                    ClearValues();
                    save = 1;
                    Myinventory.m_MysqlDb.TransactionCommit();
                    Session["Dt_ItemAdjustmentList"] = null;
                }
                catch
                {
                    lbl_error.Text = "Please refresh the page and try again...";
                    Myinventory.m_MysqlDb.TransactionRollback();
                }

                if (save == 1)
                {
                    lbl_error.Text = "Item Adjusted successfuly ";
                }
            }
        }

        private void ClearValues()
        {
            Grd_adjustment.DataSource = null; Grd_adjustment.DataBind();
            Txt_adjustDate.Text = General.GerFormatedDatVal(System.DateTime.Now);
            txt_adjustDescription.Text = "";
   
        }

        protected void Btn_ItemAdjustmentCancel_Click(object sender, EventArgs e)
        {
            ClearValues();
        }



    }
}
