using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WinBase;
using System.Data.Odbc;

namespace WinEr
{
    public partial class InventorySalesItems : System.Web.UI.Page
    {
        private DataSet Dt_SaleItemsList;
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
            Session["Dt_SaleItemsList"] = null;
            Txt_SaleDate.Text = General.GerFormatedDatVal(System.DateTime.Now); 
            LoadSaleLocation();

            if (Session["SelectedLocation"] != null)
            {
                int.TryParse(Session["SelectedLocation"].ToString(), out LocationId);
                drp_location.SelectedValue = LocationId.ToString();

            }
            WC_selectItem.IsAdjustment(0);
            WC_selectItem.SetCategoryType(2);
            WC_selectItem.IsPurchaze(0);
            WC_selectItem.SetNeedItemCount(1);
            Btn_Salesave.Enabled = false;
            lbl_error.Text = "Please add items for sale";
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
            Grd_saleitem.Columns[0].Visible = true;
            Grd_saleitem.Columns[1].Visible = true;
            int Id = WC_selectItem.SelectedId;
            int temp = 0;

            if (WC_selectItem.SelectedId != 0 || WC_selectItem.Totalcount != 0)
            {
                string _sql = "";
                _sql = "select ItemName,tblinv_item.id,Stock,tblinv_item.Category  ,tblinv_item.Cost,tblinv_category.Category as Categoryname  from tblinv_item inner join tblinv_category on tblinv_category.Id= tblinv_item.Category inner join tblinv_locationitemstock on tblinv_item.Id= tblinv_locationitemstock.ItemId  where tblinv_category.Categorytype=2 and tblinv_locationitemstock.LocationId=" + Session["SelectedLocation"] + " and  tblinv_item.id='" + WC_selectItem.SelectedId + "'  ";

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

                        Dt_SaleItemsList = (DataSet)Session["Dt_SaleItemsList"];
                        foreach (DataRow dr1 in UnConsumableDs.Tables[0].Rows)
                        {
                            if (int.Parse(dr1["Stock"].ToString()) >= count)
                            {
                                if (Dt_SaleItemsList != null && Dt_SaleItemsList.Tables != null && Dt_SaleItemsList.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow drnew in Dt_SaleItemsList.Tables[0].Rows)
                                    {
                                        if (int.Parse(drnew["Id"].ToString()) == int.Parse(dr1["Id"].ToString()))
                                        {
                                            temp = 1;
                                            if (int.Parse(dr1["Stock"].ToString()) >= int.Parse(drnew["Count"].ToString()) + count)
                                            {

                                                count = int.Parse(drnew["Count"].ToString()) + count;
                                                total = double.Parse(dr1["Cost"].ToString()) * count;

                                                drnew["Count"] = count.ToString();
                                                drnew["Total"] = total.ToString();
                                            }
                                            else
                                            {
                                                lbl_error.Text = "Count must be less than or equal to available stock";
                                            }


                                        }
                                    }
                                }
                                if (temp == 0)
                                {
                                    Dt_SaleItemsList = getDataSet();
                                    dr = Dt_SaleItemsList.Tables["SaleItemsList"].NewRow();
                                    dr["id"] = dr1["Id"].ToString();
                                    dr["Category"] = dr1["Category"].ToString();
                                    dr["ItemName"] = dr1["ItemName"].ToString().Replace("&amp;", "&"); 
                                    dr["Stock"] = dr1["Stock"].ToString();
                                    dr["Count"] = count.ToString();
                                    dr["Amount"] = dr1["Cost"].ToString();
                                    total = double.Parse(dr1["Cost"].ToString()) * count;
                                    dr["Total"] = total.ToString();
                                    dr["Categoryname"] = dr1["Categoryname"].ToString();
                         

                                    Dt_SaleItemsList.Tables["SaleItemsList"].Rows.Add(dr);
                                }
                            }
                            else
                            {
                                lbl_error.Text = "Count must be less than or equal to available stock";
                            }
                        }

                    }
                    Session["Dt_SaleItemsList"] = null;
                    Session["Dt_SaleItemsList"] = Dt_SaleItemsList;

                    Grd_saleitem.DataSource = Dt_SaleItemsList;
                    Grd_saleitem.DataBind();

                    double totalamt = 0;

                    foreach (GridViewRow gr in Grd_saleitem.Rows)
                    {
                        totalamt = totalamt + double.Parse(gr.Cells[7].Text);

                    }
                    Txt_saletotalAmnt.Text = (Math.Round(totalamt, 2)).ToString();
                }
                else
                {
                    lbl_error.Text = "Selected item does not found in selected location";
                }
                Grd_saleitem.Columns[0].Visible = false;
                Grd_saleitem.Columns[1].Visible = false;
                Btn_Salesave.Enabled = true; 
            }

        }

        protected void Btn_saleCancel_Click(object sender, EventArgs e)
        {
            Grd_saleitem.DataSource = null;
            Grd_saleitem.DataBind();

            Txt_saletotalAmnt.Text = "0";
            Txt_Customer.Text = "";
            Txt_SaleDate.Text = General.GerFormatedDatVal(System.DateTime.Now); 
            txt_saleDescription.Text = "";

        }

        protected void Btn_Salesave_Click(object sender, EventArgs e)
        {
            Grd_saleitem.Columns[0].Visible = true;
            Grd_saleitem.Columns[1].Visible = true;

            int _ItemID = 0, _CategoryId = 0, count = 0, _availstock = 0, _locationId = 0;
            double totalamount = 0.0;
            int value = 0;
            int chkcount = 0;
            int save = 0;
            int grdvalue = 0;
            DateTime _saledate = new DateTime();
            DateTime Today = DateTime.Now.Date;
            string saleto = "";
            string ItemName = "", _description = "", _CreatedUser = "";
            _CreatedUser = MyUser.UserName;

            if (txt_saleDescription.Text.Trim().Length > 250)
                _description = txt_saleDescription.Text.Trim().Substring(0, 249);
            else
                _description = txt_saleDescription.Text.Trim();


            try
            {
                if (Grd_saleitem.Rows.Count > 0)
                {
                    if (grdvalue == 0)
                    {

                        foreach (GridViewRow gv in Grd_saleitem.Rows)
                        {



                            chkcount++;
                            _ItemID = int.Parse(gv.Cells[0].Text);
                            _CategoryId = int.Parse(gv.Cells[1].Text);
                            ItemName = gv.Cells[2].Text.Replace("&amp;", "&"); ;
                            _availstock = int.Parse(gv.Cells[4].Text);

                            if (gv.Cells[5].Text.ToString() != "")
                            {
                                count = int.Parse(gv.Cells[5].Text.ToString());
                            }
                            if (gv.Cells[7].Text.ToString() != "")
                            {
                                totalamount = int.Parse(gv.Cells[7].Text.ToString());
                            }
                            if (Txt_SaleDate.Text != "")
                            {
                                _saledate = MyUser.GetDareFromText(Txt_SaleDate.Text).Date;

                            }
                            else
                            {
                                _saledate = Today;

                            }

                            try
                            {

                                save = 1;
                                Myinventory.m_MysqlDb.MyBeginTransaction();
                                _locationId = LocationId;
                                string itemname = gv.Cells[2].Text.Replace("&amp;", "&");
                                saleto = Txt_Customer.Text;
                                int opnqty = 0;
                                Myinventory.AdjustItemCount(1, _ItemID, count, 1);
                                Myinventory.UpdateOpenQuantity(_ItemID, out opnqty);
                                Myinventory.SaleItemDetails(_ItemID, _CategoryId, count, _availstock, _locationId, totalamount, _saledate, saleto, ItemName, _description, _CreatedUser, opnqty);
                                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Inventory : Sale Item", "Item " + itemname + " is sold to " + saleto + ", Count is " + count + ",Cost is " + totalamount + "", 1, Myinventory.m_MysqlDb);
                                Myinventory.m_MysqlDb.TransactionCommit();
                                Session["Dt_SaleItemsList"] = null;
                            }
                            catch
                            {


                                //WC_MessageBox.ShowMssage("Please refresh the page and try again...");
                                Myinventory.m_MysqlDb.TransactionRollback();
                            }


                        }

                    }



                    if (chkcount == 0)
                    {
                        lbl_error.Text = "Please select items";
                    }
                    if (save == 1)
                    {
                        string _date = "", saletoreport = "";
                        double totalcost = 0.0;
                        // FillItemGrid("", 0);
                        LoadPopUpData();

                        if (Txt_SaleDate.Text != "")
                        {
                            _date = Txt_SaleDate.Text;
                        }
                        else
                        {
                            _date = General.GerFormatedDatVal(System.DateTime.Now.Date);
                        }
                        if (Txt_Customer.Text != "")
                        {
                            saletoreport = Txt_Customer.Text;
                        }
                        totalcost = double.Parse(Txt_saletotalAmnt.Text);

                        ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "openIncpopup('SaleReport.aspx?SaleTo=" + saletoreport + "&Date=" + _date + "&totalcost=" + totalcost + "');", true);
                        //LoadSaleGrid();
                  

                        Grd_saleitem.DataSource = null;
                        Grd_saleitem.DataBind();

                        Txt_saletotalAmnt.Text = "0";
                        Txt_Customer.Text = "";
                        Txt_SaleDate.Text = General.GerFormatedDatVal(System.DateTime.Now);
                        txt_saleDescription.Text = "";
                        lbl_error.Text = "Selling items are completed";
                        Session["Dt_SaleItemsList"] = null;
                    }
                    Grd_saleitem.Columns[0].Visible = false;
                    Grd_saleitem.Columns[1].Visible = false;
                }
                else
                {
                    lbl_error.Text = "Please select items";
                }
            }
            catch (Exception error)
            {
                lbl_error.Text = error.Message;
            }
        }
        private void LoadPopUpData()
        {
            DataSet ReportDs = new DataSet();
            DataRow _dr;
            DataTable _dt;
            ReportDs.Tables.Add(new DataTable("Salereport"));
            _dt = ReportDs.Tables["Salereport"];
            _dt.Columns.Add("ItemName");
            _dt.Columns.Add("Categoryname");
            _dt.Columns.Add("Cost");
            _dt.Columns.Add("Count");
            string itemname = "";
            double cost = 0.0;
            int itemcount = 0;

            foreach (GridViewRow gr in Grd_saleitem.Rows)
            {




                itemname = gr.Cells[2].Text;
                if (gr.Cells[7].Text != "")
                {
                    cost = double.Parse(gr.Cells[7].Text);
                }
                if (gr.Cells[5].Text != "")
                {
                    itemcount = int.Parse(gr.Cells[5].Text);
                }
                _dr = ReportDs.Tables["Salereport"].NewRow();
                _dr["ItemName"] = itemname.Replace("&amp;", "&");
                _dr["Cost"] = cost;
                _dr["Count"] = itemcount;
                //      dr["TotalStock"] = gv.Cells[5].Text;
                ReportDs.Tables["Salereport"].Rows.Add(_dr);


            }
            Session["Reportds"] = ReportDs;
        }

        protected void Grd_saleitem_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grd_saleitem.Columns[0].Visible = true;
            Grd_saleitem.Columns[1].Visible = true;

            int selectedrowindex = Grd_saleitem.SelectedIndex;

            int _id = int.Parse(Grd_saleitem.Rows[selectedrowindex].Cells[0].Text);

            Dt_SaleItemsList = (DataSet)Session["Dt_SaleItemsList"];

            if (Dt_SaleItemsList != null && Dt_SaleItemsList.Tables != null && Dt_SaleItemsList.Tables[0].Rows.Count > 0)
            {
                if (Dt_SaleItemsList.Tables[0].Rows.Count == 1)
                {
                    Grd_saleitem.DataSource = null;
                    Grd_saleitem.DataBind();
                    Dt_SaleItemsList.Tables[0].Rows.Clear();
                    Session["Dt_SaleItemsList"] = Dt_SaleItemsList;
                }
                else
                {
                    Dt_SaleItemsList.Tables[0].Rows[selectedrowindex].Delete();
                    Grd_saleitem.DataSource = Dt_SaleItemsList;
                    Grd_saleitem.DataBind();
                    Session["Dt_SaleItemsList"] = Dt_SaleItemsList;
                }


                double totalamt = 0;

                if (Grd_saleitem.Rows.Count > 0)
                {
                    foreach (GridViewRow gr in Grd_saleitem.Rows)
                    {
                        totalamt = totalamt + double.Parse(gr.Cells[7].Text);

                    }
                    Txt_saletotalAmnt.Text = (Math.Round(totalamt, 2)).ToString();
                }
                else
                {
                    Txt_saletotalAmnt.Text = "0";
                }
            }
            Grd_saleitem.Columns[0].Visible = false;
            Grd_saleitem.Columns[1].Visible = false;
        }

        protected void drp_location_SelectedIndexChanged(object sender, EventArgs e)
        {
            WC_selectItem.IsAdjustment(0);
            WC_selectItem.SetCategoryType(2);
            WC_selectItem.IsPurchaze(0);
            WC_selectItem.SetNeedItemCount(1);
            Session["SelectedLocation"] = drp_location.SelectedValue;
            WC_selectItem.loadlocations();
            Session["Dt_SaleItemsList"] = null;
            Grd_saleitem.DataSource = null;
            Grd_saleitem.DataBind();
        }

        private DataSet getDataSet()
        {
            if (Session["Dt_SaleItemsList"] == null)
            {
                Dt_SaleItemsList = new DataSet();

                DataTable dt;

                Dt_SaleItemsList.Tables.Add(new DataTable("SaleItemsList"));
                dt = Dt_SaleItemsList.Tables["SaleItemsList"];
                dt.Columns.Add("id");
                dt.Columns.Add("Category");
                dt.Columns.Add("ItemName");
                dt.Columns.Add("Stock");
                dt.Columns.Add("Count");
                dt.Columns.Add("Categoryname");
                dt.Columns.Add("Amount");
                dt.Columns.Add("Total");
              

            }
            else
            {
                TextBox tct_descr = new TextBox();

                DataRow dr;
                Dt_SaleItemsList = (DataSet)Session["Dt_SaleItemsList"];
                Dt_SaleItemsList.Tables[0].Rows.Clear();
                foreach (GridViewRow gr in Grd_saleitem.Rows)
                {


                    dr = Dt_SaleItemsList.Tables["SaleItemsList"].NewRow();
                    dr["id"] = gr.Cells[0].Text.ToString();
                    dr["Category"] = gr.Cells[1].Text.ToString();
                    dr["ItemName"] = gr.Cells[2].Text.ToString();
                    dr["Stock"] = gr.Cells[4].Text.ToString();
                    dr["Count"] = gr.Cells[5].Text.ToString();

                    dr["Amount"] = gr.Cells[6].Text.ToString();
                    dr["Total"] = gr.Cells[7].Text.ToString();
                    dr["Categoryname"] = gr.Cells[3].Text.ToString();
        
                    Dt_SaleItemsList.Tables["SaleItemsList"].Rows.Add(dr);
                }
            }
            return Dt_SaleItemsList;

        }
    }
}
