using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using WinBase;
using System.Data;


namespace WinEr
{
    public partial class InventoryMoveItems : System.Web.UI.Page
    {

        private DataSet Dt_MoveITemList;
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
            LoadMoveLocation();
            Txt_MoveDate.Text = General.GerFormatedDatVal(System.DateTime.Now);
            if (Session["SelectedLocation"] != null)
            {
                int.TryParse(Session["SelectedLocation"].ToString(), out LocationId);
                drp_location.SelectedValue = LocationId.ToString();

            }
            Session["Dt_MoveITemList"] = null;
    
            Btn_Move.Enabled = false;

            WC_selectItem.IsAdjustment(0);
            WC_selectItem.SetCategoryType(0);
            WC_selectItem.SetNeedItemCount(1);
            WC_selectItem.IsPurchaze(0);

        }
        private void LoadMoveLocation()
        {
            drp_location.Items.Clear();
            Drp_ToLocation.Items.Clear();
            DataSet locationds = Myinventory.GetActiveLocationNameForMoveItem();
            if (locationds != null && locationds.Tables[0].Rows.Count > 0)
            {
                ListItem li;
                li = new ListItem("- Inventory Location -", "0");
                drp_location.Items.Add(li);
                Drp_ToLocation.Items.Add(li);
                foreach (DataRow dr in locationds.Tables[0].Rows)
                {
                    //Id,Locationname 
                    li = new ListItem(dr["Locationname"].ToString(), dr["Id"].ToString());
                    drp_location.Items.Add(li);
                    Drp_ToLocation.Items.Add(li);

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
            Grd_Moveitem.Columns[0].Visible = true;
            Grd_Moveitem.Columns[1].Visible = true;
            lbl_error.Text = "";
            int Id = WC_selectItem.SelectedId;
            int temp = 0;

            if (WC_selectItem.SelectedId != 0 )
            {
                string _sql = "";
                _sql = "select ItemName,tblinv_item.id,Stock,tblinv_item.Category ,tblinv_item.Cost ,tblinv_category.Category as Categoryname from tblinv_item inner join tblinv_category on tblinv_category.Id= tblinv_item.Category inner join tblinv_locationitemstock on tblinv_item.Id= tblinv_locationitemstock.ItemId  where  tblinv_locationitemstock.LocationId=" + Session["SelectedLocation"] + " and  tblinv_item.id='" + WC_selectItem.SelectedId + "'  ";

                DataSet UnConsumableDs = Myinventory.m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
                DataRow dr;
                if (UnConsumableDs != null && UnConsumableDs.Tables != null && UnConsumableDs.Tables[0].Rows.Count > 0)
                {

                    int count = 0;

                    if (WC_selectItem.GetNeedItemCount() == 1)
                    {

                        count = WC_selectItem.Totalcount;

                        Dt_MoveITemList = (DataSet)Session["Dt_MoveITemList"];
                        foreach (DataRow dr1 in UnConsumableDs.Tables[0].Rows)
                        {
                            if (int.Parse(dr1["Stock"].ToString()) >= count)
                            {
                                if (Dt_MoveITemList != null && Dt_MoveITemList.Tables != null && Dt_MoveITemList.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow drnew in Dt_MoveITemList.Tables[0].Rows)
                                    {
                                        if (int.Parse(drnew["Id"].ToString()) == int.Parse(dr1["Id"].ToString()))
                                        {
                                            temp = 1;

                                            if (int.Parse(dr1["Stock"].ToString()) >= int.Parse(drnew["Count"].ToString()) + count)
                                            {
                                                count = int.Parse(drnew["Count"].ToString()) + count;
                                                drnew["Count"] = count.ToString();
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
                                    Dt_MoveITemList = getDataSet();
                                    dr = Dt_MoveITemList.Tables["Dt_MoveITemList"].NewRow();
                                    dr["id"] = dr1["Id"].ToString();
                                    dr["Category"] = dr1["Category"].ToString();
                                    dr["ItemName"] = dr1["ItemName"].ToString().Replace("&amp;", "&"); ;
                                    dr["Stock"] = dr1["Stock"].ToString();
                                    dr["Count"] = count.ToString();
                                    dr["Categoryname"] = dr1["Categoryname"].ToString(); 

                                    Dt_MoveITemList.Tables["Dt_MoveITemList"].Rows.Add(dr);
                                }
                                Session["Dt_MoveITemList"] = null;
                                Session["Dt_MoveITemList"] = Dt_MoveITemList;

                                Grd_Moveitem.DataSource = Dt_MoveITemList;
                                Grd_Moveitem.DataBind();
                            }
                            else
                            {
                                lbl_error.Text = "Count must be less than or equal to available stock";
                            }
                        }
                        


                    }
                    else
                    {
                        count = 0;

                        Dt_MoveITemList = (DataSet)Session["Dt_MoveITemList"];
                        foreach (DataRow dr1 in UnConsumableDs.Tables[0].Rows)
                        {
                            if (Dt_MoveITemList != null && Dt_MoveITemList.Tables != null && Dt_MoveITemList.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow drnew in Dt_MoveITemList.Tables[0].Rows)
                                {
                                    if (int.Parse(drnew["Id"].ToString()) == int.Parse(dr1["Id"].ToString()))
                                    {
                                        temp = 1;
                                        count = int.Parse(drnew["Count"].ToString()) + count;
                                        drnew["Count"] = count.ToString();

                                    }
                                }
                            }
                            if (temp == 0)
                            {
                                Dt_MoveITemList = getDataSet();
                                dr = Dt_MoveITemList.Tables["Dt_MoveITemList"].NewRow();
                                dr["id"] = dr1["Id"].ToString();
                                dr["Category"] = dr1["Category"].ToString();
                                dr["ItemName"] = dr1["ItemName"].ToString().Replace("&amp;", "&"); ;
                                dr["Stock"] = dr1["Stock"].ToString();
                                dr["Count"] = count.ToString();
                                dr["Categoryname"]= dr1["Categoryname"].ToString(); 

                                Dt_MoveITemList.Tables["Dt_MoveITemList"].Rows.Add(dr);
                            }

                        }
                        Session["Dt_MoveITemList"] = null;
                        Session["Dt_MoveITemList"] = Dt_MoveITemList;

                        Grd_Moveitem.DataSource = Dt_MoveITemList;
                        Grd_Moveitem.DataBind();


                    }
                    Grd_Moveitem.Columns[0].Visible = false;
                    Grd_Moveitem.Columns[1].Visible = false;
                    Btn_Move.Enabled = true;
                }
                else
                {
                    lbl_error.Text = "Selected Item does not exist";
                }
            }

        }
        private DataSet getDataSet()
        {
            if (Session["Dt_MoveITemList"] == null)
            {
                Dt_MoveITemList = new DataSet();

                DataTable dt;

                Dt_MoveITemList.Tables.Add(new DataTable("Dt_MoveITemList"));
                dt = Dt_MoveITemList.Tables["Dt_MoveITemList"];
                dt.Columns.Add("id");
                dt.Columns.Add("Category");
                dt.Columns.Add("ItemName");
                dt.Columns.Add("Stock");
                dt.Columns.Add("Count");
                dt.Columns.Add("Categoryname");

      


            }
            else
            {
                TextBox tct_descr = new TextBox();

                DataRow dr;
                Dt_MoveITemList = (DataSet)Session["Dt_MoveITemList"];
                Dt_MoveITemList.Tables[0].Rows.Clear();
                foreach (GridViewRow gr in Grd_Moveitem.Rows)
                {


                    dr = Dt_MoveITemList.Tables["Dt_MoveITemList"].NewRow();
                    dr["id"] = gr.Cells[0].Text.ToString();
                    dr["Category"] = gr.Cells[1].Text.ToString();
                    dr["ItemName"] = gr.Cells[2].Text.ToString();
                    dr["Stock"] = gr.Cells[4].Text.ToString();
                    dr["Count"] = gr.Cells[5].Text.ToString();
                    dr["Categoryname"] = gr.Cells[3].Text.ToString();
                   
                    Dt_MoveITemList.Tables["Dt_MoveITemList"].Rows.Add(dr);
                }
            }
            return Dt_MoveITemList;

        }
        protected void Grd_Grd_Moveitem_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grd_Moveitem.Columns[0].Visible = true;
            Grd_Moveitem.Columns[1].Visible = true;

            int selectedrowindex = Grd_Moveitem.SelectedIndex;

            int _id = int.Parse(Grd_Moveitem.Rows[selectedrowindex].Cells[0].Text);

            Dt_MoveITemList = (DataSet)Session["Dt_MoveITemList"];

            if (Dt_MoveITemList != null && Dt_MoveITemList.Tables != null && Dt_MoveITemList.Tables[0].Rows.Count > 0)
            {
                if (Dt_MoveITemList.Tables[0].Rows.Count == 1)
                {
                    Grd_Moveitem.DataSource = null;
                    Grd_Moveitem.DataBind();
                    Dt_MoveITemList.Tables[0].Rows.Clear();
                    Session["Dt_MoveITemList"] = Dt_MoveITemList;
                }
                else
                {
                    Dt_MoveITemList.Tables[0].Rows[selectedrowindex].Delete();
                    Grd_Moveitem.DataSource = Dt_MoveITemList;
                    Grd_Moveitem.DataBind();
                    Session["Dt_MoveITemList"] = Dt_MoveITemList;
                }


               
            }
            Grd_Moveitem.Columns[0].Visible = false;
            Grd_Moveitem.Columns[1].Visible = false;
        }

        protected void Btn_move_Click(object sender, EventArgs e)
        {
            lbl_error.Text = "";
            int _ChkCount = 0;
            int spec = 0;
            int _ItemId = 0, _OutStock = 0, _CurrentStock = 0, _Category = 0, _fromlocationId, _tolocationId;
            string _Description = "", _CreatedUser = "", _ItemName = "";
            _CreatedUser = MyUser.UserName;
           
            DateTime Today = DateTime.Now.Date;
            DateTime _OutFlowDate = new DateTime();
            int value = 0;





            foreach (GridViewRow gv in Grd_Moveitem.Rows)
            {

               
                _ChkCount++;
                _CurrentStock = int.Parse(gv.Cells[4].Text.Trim());
                int FromLocation_Id = int.Parse(Session["SelectedLocation"].ToString());
                int ToLocation_Id = int.Parse(Drp_ToLocation.SelectedValue.ToString());
                if (ToLocation_Id == 0)
                {
                    value = 1;
                    lbl_error.Text = "Please select To location!";
                }

                else if (FromLocation_Id == ToLocation_Id)
                {
                    value = 1;
                    lbl_error.Text = "You cannot move item to the same location!";
                }
                 
                else
                {
                    _OutStock = int.Parse(gv.Cells[5].Text.Trim());
                    if (_CurrentStock < _OutStock)
                    {
                        value = 1;
                        lbl_error.Text = "Stock-Out should be less than available stock!";
                    }
                }
            }
            if (value == 0)
            {
                try
                {
                    foreach (GridViewRow gv in Grd_Moveitem.Rows)
                    {

                        _ChkCount++;
                        _ItemId = int.Parse(gv.Cells[0].Text);
                        _ItemName = gv.Cells[2].Text;
                        if (gv.Cells[5].Text != "")
                        {
                            _OutStock = int.Parse(gv.Cells[5].Text);
                        }
                        _Description = txt_moveDescription.Text.Trim();
                        if (txt_moveDescription.Text.Trim().Length > 250)
                            _Description = txt_moveDescription.Text.Trim().Substring(0, 249);
                        else
                            _Description = txt_moveDescription.Text.Trim();

                        if (Txt_MoveDate.Text != "")
                        {
                            _OutFlowDate = MyUser.GetDareFromText(Txt_MoveDate.Text).Date;

                        }
                        else
                        {
                            _OutFlowDate = Today;

                        }
                        //_CurrentStock = int.Parse(gv.Cells[4].Text);
                        _Category = int.Parse(gv.Cells[1].Text.Trim());
                        _CurrentStock = int.Parse(gv.Cells[4].Text.Trim());
                        _tolocationId = int.Parse(Drp_ToLocation.SelectedValue);
                        _fromlocationId = int.Parse(Session["SelectedLocation"].ToString());

                        try
                        {

                            spec = 1;
                            int opnqty = 0;
                            string fromlocation = drp_location.SelectedItem.Text;
                            string tolocation = Drp_ToLocation.SelectedItem.Text;
                            // SaveOutFlowItemDetailsToTable(_ItemId, _ItemName, _Category, _OutStock, _Description, _OutFlowDate, _CreatedUser, _CurrentStock);
                            Myinventory.m_MysqlDb.MyBeginTransaction();
                            Myinventory.UpdateOpenQuantity(_ItemId, out opnqty);
                            Myinventory.MoveItemToLocation(_ItemId, _ItemName, _Category, _OutStock, _OutFlowDate, _tolocationId, _Description, _CreatedUser, _fromlocationId, _CurrentStock, opnqty);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Inventory : Move Item", "Item " + _ItemName + " is Moved from " + fromlocation + " To " + tolocation + ",Count is " + _OutStock + "", 1, Myinventory.m_MysqlDb);
                            Myinventory.m_MysqlDb.TransactionCommit();
                            Session["Dt_MoveITemList"] = null;
                            lbl_error.Text = "Selected items to be moved!";

                        }
                        catch
                        {
                            lbl_error.Text = "Please refresh the page and try again...";
                            Myinventory.m_MysqlDb.TransactionRollback();
                        }


                    }
                    Grd_Moveitem.DataSource = null;
                    Grd_Moveitem.DataBind();
                
                    Txt_MoveDate.Text = General.GerFormatedDatVal(System.DateTime.Now);
                    txt_moveDescription.Text = "";

                  

                }
                catch (Exception error)
                {
                    lbl_error.Text=error.Message;
                }
            }

            if (_ChkCount == 0)
            {
                lbl_error.Text="Please select an item!";
            }
            if (spec == 1)
            {
               lbl_error.Text="Selected items are moved!";
               Grd_Moveitem.DataSource = null;
               Grd_Moveitem.DataBind();
             

            }
        }
        protected void drp_location_SelectedIndexChanged(object sender, EventArgs e)
        {
            WC_selectItem.IsAdjustment(0);
            WC_selectItem.SetCategoryType(0);
            WC_selectItem.SetNeedItemCount(1);
            WC_selectItem.IsPurchaze(0);
            Session["SelectedLocation"] = drp_location.SelectedValue;
            WC_selectItem.loadlocations();
            Session["Dt_MoveITemList"] = null;
            Grd_Moveitem.DataSource = null;
            Grd_Moveitem.DataBind();

        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Grd_Moveitem.DataSource = null;
            Grd_Moveitem.DataBind();
          
            Txt_MoveDate.Text = General.GerFormatedDatVal(System.DateTime.Now); 
            txt_moveDescription.Text = "";
        }
    }
}
