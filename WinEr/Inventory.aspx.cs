using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using AjaxControlToolkit;
using System.Drawing;
using System.IO;
using WinBase;
using System.Text;

namespace WinEr
{
    public partial class Inventory : System.Web.UI.Page
    {
        
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet = null;
        private TextBox[] dynamicTextBoxes;
        private int[] Mandatoryflag;
        private string[] FealdName;
        private int CustfieldCount;
        private WinBase.Inventory Myinventory;

        private DataSet Dt_SaleItemsList;
        #region InitialRegion

        //Events

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];

            if (MyUser.SELECTEDMODE == 1)
            {
                this.MasterPageFile = "~/WinerStudentMaster.master";

            }
            else if (MyUser.SELECTEDMODE == 2)
            {

                this.MasterPageFile = "~/WinerSchoolMaster.master";
            }

        }

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

              
                    Clear();
                    LoadInitialStage();
                    FillItemGrid("", 0);
                    LoadCategoryToDropDown(0);
                }
            }
        }

        protected void Btn_exporttoexel_Click(object sender, ImageClickEventArgs e)
        {
            DataSet _ExcelDataSet = new DataSet();
            _ExcelDataSet = (DataSet)ViewState["ItemDataSet"];

            if (_ExcelDataSet.Tables[0].Rows.Count > 0)
            {
                _ExcelDataSet.Tables[0].Columns.Remove("Id");
                if (!ExcelUtility.ExportDataSetToExcel(_ExcelDataSet, "Inventory List"))
                {
                    WC_MessageBox.ShowMssage("MS Excel is missing.Please install..");
                }
            }
            else
            {
                lbl_ItemMessage.Text = "No items exist..";
            }
        }

        protected void Img_Go_Click(object sender, ImageClickEventArgs e)
        {
            string _ItemName = "";
            _ItemName = TxtSearch.Text.Trim();
            FillItemGrid(_ItemName, 0);
            LoadCategoryToDropDown(0);

            Clear();
            LoadInitialStage();
        }

        protected void Img_Search_Click(object sender, ImageClickEventArgs e)
        {
            Clear();
            LoadInitialStage();
            int _CategoryId = int.Parse(Drp_Categories.SelectedValue);
            FillItemGrid("", _CategoryId);
        }

        protected void Grd_Items_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Items.PageIndex = e.NewPageIndex;
            // FillItemGrid("", int.Parse(Drp_Categories.SelectedValue));
            if (ViewState["ItemDataSet"] != null)
            {
                DataSet _pageDS = (DataSet)ViewState["ItemDataSet"];
                if (_pageDS.Tables[0].Rows.Count > 0)
                {
                    Grd_Items.Columns[1].Visible = true;

                    DataTable dtpageData = _pageDS.Tables[0];

                    DataView dataView = new DataView(dtpageData);

                    if (Session["SortDirection1"] != null && Session["SortExpression1"] != null)
                    {
                        dataView.Sort = (string)Session["SortExpression1"] + " " + (string)Session["SortDirection1"];
                    }

                    Grd_Items.DataSource = dataView;
                    Grd_Items.DataBind();
                    Grd_Items.Columns[1].Visible = false;
                }
                else
                {
                    lbl_ItemMessage.Text = "No item present for selected category";
                }
            }
        }

        protected void Grd_Items_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Grd_Items.PageIndex = 0;
            //FillItemGrid("", int.Parse(Drp_Categories.SelectedValue));
            if (ViewState["ItemDataSet"] != null)
            {
                DataSet _sortlDS = (DataSet)ViewState["ItemDataSet"];

                if (_sortlDS.Tables[0].Rows.Count > 0)
                {
                    DataTable dtData = _sortlDS.Tables[0];

                    DataView dataView = new DataView(dtData);

                    dataView.Sort = e.SortExpression + " " + GetSortDirection1(e.SortExpression);
                    Grd_Items.Columns[1].Visible = true;
                    //Grd_Items.Columns[5].Visible = true;
                    //Grd_Items.Columns[6].Visible = true;

                    Grd_Items.DataSource = dataView;
                    Grd_Items.DataBind();
                    Grd_Items.Columns[1].Visible = false;
                    //Grd_Items.Columns[5].Visible = false;
                    //Grd_Items.Columns[6].Visible = false;

                }
            }
        }

        protected void Drp_Searchlocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["SelectedLocation"] = Drp_Searchlocation.SelectedValue.ToString();

            int _CategoryId = int.Parse(Drp_Categories.SelectedValue);
            string _ItemName = "";
            _ItemName = TxtSearch.Text.Trim();
            FillItemGrid(_ItemName, _CategoryId);
        }

        //Methods

        private void LoadInitialStage()
        {
            Pnl_SearchArea.Visible = true;
            Pnl_ItemList.Visible = true;
            Pnl_AddInventory.Visible = false;
            Pnl_EditItem.Visible = false;
          
            LoadTopAreaDetails();
            LoadSearchLocationDropdown();
        }

        private void LoadSearchLocationDropdown()
        {
            Drp_Searchlocation.Items.Clear();
            DataSet locationds = Myinventory.GetActiveLocationNameForMoveItem();
            if (locationds != null && locationds.Tables[0].Rows.Count > 0)
            {
                ListItem li;
                li = new ListItem("All", "0");
                Drp_Searchlocation.Items.Add(li);
                foreach (DataRow dr in locationds.Tables[0].Rows)
                {
                    //Id,Locationname 
                    li = new ListItem(dr["Locationname"].ToString(), dr["Id"].ToString());
                    Drp_Searchlocation.Items.Add(li);

                }
                Drp_Searchlocation.SelectedValue = "1";

            }
            else
            {
                ListItem li = new ListItem("No location found", "-1");
                Drp_Searchlocation.Items.Add(li);
            }
        }

        private void LoadTopAreaDetails()
        {
            Lbl_Ct.Text = GetitemCount().ToString();
            Lbl_WrnCt.Text = GetWarninglevelItemCount().ToString();
        }

        private int GetWarninglevelItemCount()
        {
            int count = 0;
            string sql = "SELECT COUNT(Id) FROM tblinv_item where tblinv_item.TotalStock< tblinv_item.MinQty";
            MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out count);
            }
            return count;
        }

        private int GetitemCount()
        {
            int count = 0;
            string sql = "SELECT COUNT(Id) FROM tblinv_item";
            MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out count);
            }
            return count;
        }

        private void FillItemGrid(string _ItemName, int _CategoryId)
        {
            string Subsql = "", Sql1 = "";
            if (_ItemName == "")
            {
                if (ChkWarning.Checked == true)
                {
                    if (_CategoryId == 0) //(int.Parse(Drp_Categories.SelectedValue) == 0)
                    {
                        Sql1 = " and tblinv_item.TotalStock< tblinv_item.MinQty";
                    }
                    else
                    {
                        Sql1 = " and tblinv_item.TotalStock< tblinv_item.MinQty and tblinv_item.Category=" + _CategoryId;// int.Parse(Drp_Categories.SelectedValue);
                    }
                }
                else
                {
                    if (_CategoryId == 0)// (int.Parse(Drp_Categories.SelectedValue) == 0)
                    {
                        Sql1 = "";
                    }
                    else
                    {
                        Sql1 = " and tblinv_item.Category=" + _CategoryId;
                    }
                }
                Subsql = Sql1;
            }
            else
            {
                Subsql = "and tblinv_item.ItemName='" + _ItemName + "'";
            }
            string sql = "";
            int locationid = 0;
            int.TryParse(Drp_Searchlocation.SelectedValue, out locationid);
            string _stksql="";
            if (locationid == 0)
            {
                sql = "SELECT tblinv_item.Id,tblinv_item.MaxQty, tblinv_item.OpnQuantity, tblinv_item.OpnQuantity as stock,tblinv_item.ItemName,tblinv_item.Description, tblinv_category.Category,tblinv_item.Cost, tblinv_item.UnitType from tblinv_item inner join tblinv_category on tblinv_item.Category= tblinv_category.Id";
            }
            else
            {
                sql = "SELECT tblinv_item.Id,tblinv_item.MaxQty, tblinv_item.OpnQuantity, tblinv_item.ItemName,tblinv_item.Description, tblinv_category.Category,tblinv_locationitemstock.Stock,tblinv_item.Cost, tblinv_item.UnitType from tblinv_item inner join tblinv_category on tblinv_item.Category= tblinv_category.Id inner join tblinv_locationitemstock on tblinv_locationitemstock.ItemId = tblinv_item.Id  where 1=1 and tblinv_locationitemstock.LocationId=" + locationid + " " + Subsql;

            }
            Grd_Items.DataSource = null;
            Grd_Items.DataBind();
            ViewState["ItemDataSet"] = null;

            //string sql = "SEECT tblinv_item.Id, tblinv_item.ItemName,tblinv_item.Description, tblinv_category.Category,tblinv_locationitemstock.Stock, tblinv_item.Cost, tblinv_item.UnitType from tblinv_item inner join tblinv_category on tblinv_item.Category= tblinv_category.Id where 1=1 " + Subsql;
            MyDataSet = Myinventory.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (MyDataSet.Tables[0].Rows.Count > 0)
            {
                ViewState["ItemDataSet"] = MyDataSet;
                Grd_Items.Columns[1].Visible = true;
                Grd_Items.Columns[2].Visible = true;
                Grd_Items.Columns[3].Visible = true;
                Grd_Items.DataSource = MyDataSet;
                Grd_Items.DataBind();
                Grd_Items.Columns[1].Visible = false;
                Grd_Items.Columns[2].Visible = false;
                Grd_Items.Columns[3].Visible = false;
                lbl_ItemMessage.Text = "";
                Btn_exporttoexel.Enabled = true;
            }
            else
            {
                lbl_ItemMessage.Text = "No items exist..";
                Btn_exporttoexel.Enabled = false;
            }

          
        }

        #endregion

        #region CommonRegion

        //Events

        private string GetSortDirection1(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = Session["SortExpression1"] as string;


            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = Session["SortDirection1"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }
            // Save new values in ViewState.
            Session["SortDirection1"] = sortDirection;
            Session["SortExpression1"] = column;

            return sortDirection;
        }

       
       //Methods

        private void LoadCategoryToDropDown(int Type)
        {
            if (Type == 0)
            {
                Drp_Categories.Items.Clear();
            }
            else
            {
                Drp_Category.Items.Clear();
            }

            string sql = "SELECT tblinv_category.Category, tblinv_category.Id from tblinv_category order by tblinv_category.Category";
            MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("All", "0");
                if (Type == 0)
                {
                    Drp_Categories.Items.Add(li);
                }
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    if (Type == 0)
                    {
                        Drp_Categories.Items.Add(li);
                    }
                    else
                    {
                        Drp_Category.Items.Add(li);
                    }

                }
                //Drp_Categories.SelectedIndex = _intex;
            }
            else
            {
                ListItem li = new ListItem("No Category Found", "-1");
                Drp_Categories.Items.Add(li);
                Drp_Category.Items.Add(li);
            }
        }        

      

        #endregion

        #region GoodsReceipt

        //Events

      

        protected void Btn_InputFlow_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("InventoryGoodReceipt.aspx");
        }

     
        //Methods


    

        private DataSet ReturnSearchItem_DataSet()
        {
            DataSet MydataSet;
            MydataSet = new DataSet();
            DataTable dt;
            DataRow dr;
            MydataSet.Tables.Add(new DataTable("Input"));
            dt = MydataSet.Tables["Input"];
            dt.Columns.Add("Id");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Category");
            dt.Columns.Add("MaxQty");
            dt.Columns.Add("OpnQuantity");
            dt.Columns.Add("BasicCost");
            //dt.Columns.Add("TotalStock", typeof(int));

            foreach (GridViewRow gv in Grd_Items.Rows)
            {
                CheckBox cb = (CheckBox)gv.FindControl("Chk_Item");
                if (cb.Checked)
                {

                    dr = MydataSet.Tables["Input"].NewRow();
                    dr["Id"] = gv.Cells[1].Text;
                    dr["ItemName"] = gv.Cells[4].Text.Replace("&amp;", "&");
                    dr["Category"] = gv.Cells[6].Text;
                    dr["MaxQty"] = gv.Cells[2].Text;
                    dr["OpnQuantity"] = gv.Cells[3].Text;
                    dr["BasicCost"] = gv.Cells[9].Text;
                    //      dr["TotalStock"] = gv.Cells[5].Text;
                    MydataSet.Tables["Input"].Rows.Add(dr);
                }
            }
            return MydataSet;

        }

        private string SetDate()
        {
            DateTime _TDate = System.DateTime.Now;
            string _TodayDate = General.GerFormatedDatVal(_TDate);
            return _TodayDate;
        } 

        #endregion

        #region Issue

        //Events


        protected void Btn_Issue_Click(object sender, EventArgs e)
        {
            Response.Redirect("InventoryItemIssuetoStaff.aspx");
          
        }

        //Methods

       
          
        #endregion

        #region Sales

        //Events

        protected void Btn_Sale_Click(object sender, EventArgs e)
        {
            Response.Redirect("InventorySalesItems.aspx");
        
        }
       
        #endregion

        #region MoveItem

        //Events

        protected void Btn_OutputFlow_Click(object sender, EventArgs e)
        {
            Response.Redirect("InventoryMoveItems.aspx");
         }
        #endregion

    
        #region CategoryMangmt

        //Events

        protected void LnkBtn_CreateCategory_Click(object sender, EventArgs e)
        {
            Lbl_MsgCreateCategory.Text = "";
            txt_new_category.Text = "";
            Drp_CategoryType.Items.Clear();
            DataSet categoryds = Myinventory.LoadCategoryType();
            if (categoryds != null && categoryds.Tables[0].Rows.Count > 0)
            {

                ListItem li;
                li = new ListItem("Select Type", "0");
                Drp_CategoryType.Items.Add(li);
                foreach (DataRow dr in categoryds.Tables[0].Rows)
                {
                    //Id,Locationname 
                    li = new ListItem(dr["CategoryType"].ToString(), dr["Id"].ToString());
                    Drp_CategoryType.Items.Add(li);

                }

            }

            else
            {
                ListItem li = new ListItem("None", "-1");
                Drp_CategoryType.Items.Add(li);
            }

            MPE_MessageBox_AddNewCategory.Show();
        }

        protected void Btn_Add_new_cat_Click(object sender, EventArgs e)
        {
            Lbl_MsgCreateCategory.Text = "";
            string _CategoryName = "", _ErrorMsg = "";
            int _categoryType = int.Parse(Drp_CategoryType.SelectedValue);
            _CategoryName = txt_new_category.Text.Trim().ToUpper();

            if (_CategoryName != "")
            {
                if (_categoryType > 0)
                {
                    if (SaveNewCategory(_CategoryName, _categoryType, out _ErrorMsg))
                    {
                        LoadCategoryToDropDown(1);
                        // load the created category as the selected item
                        loadDrp_CategorySelectedtextAsTheCreatedOne(_CategoryName);
                        txt_new_category.Text = "";
                    }
                    else
                    {
                        Lbl_MsgCreateCategory.Text = _ErrorMsg;
                        MPE_MessageBox_AddNewCategory.Show();
                    }
                }
                else
                {

                    MPE_MessageBox_AddNewCategory.Show();
                    Lbl_MsgCreateCategory.Text = "Select Category type";
                }
            }
            else
            {

                MPE_MessageBox_AddNewCategory.Show();
                Lbl_MsgCreateCategory.Text = "Enter Category Name";
                txt_new_category.Text = "";
            }
        }

        //Methods

        private void loadDrp_CategorySelectedtextAsTheCreatedOne(string _CategoryName)
        {
            for (int i = 0; i < Drp_Category.Items.Count; i++)
            {
                if (Drp_Category.Items[i].Text == _CategoryName)
                {
                    Drp_Category.SelectedIndex = i;
                    break;
                }
            }
        }            

        private bool SaveNewCategory(string _CategoryName, int _categoryType, out string _ErrorMsg)
        {
            bool _Valid = false;
            _ErrorMsg = "";
            try
            {
                _CategoryName = _CategoryName.ToUpper();


                string sql = "SELECT tblinv_category.Category from tblinv_category where tblinv_category.Category='" + _CategoryName + "'";
                MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    _ErrorMsg = "Category Name already exists";
                }
                else
                {
                    sql = "INSERT into tblinv_category(Category,CategoryType) VALUES ('" + _CategoryName + "'," + _categoryType + ")";
                    Myinventory.m_MysqlDb.ExecuteQuery(sql);
                    //DBLogClass dblog = new DBLogClass(m_MysqlDb);
                     MyUser.m_DbLog.LogToDb(MyUser.UserName, "Inventory : Create Category", "A New Category " + _CategoryName + "  is created.", 1);
                    _Valid = true;

                }

            }
            catch (Exception e)
            {
                _Valid = false;
                _ErrorMsg = "Please try again";
            }

            return _Valid;

        }

        #endregion

        #region ItemMangmt

        //Events

        protected void Grd_Items_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _ItemId = int.Parse(Grd_Items.SelectedRow.Cells[1].Text.ToString());
            lbl_ItemId.Text = _ItemId.ToString();
            Pnl_AddInventory.Visible = false;
            Pnl_EditItem.Visible = true;
            Pnl_ItemList.Visible = false;
            Pnl_SearchArea.Visible = true; 
            Btn_OutputFlow.Visible = true;
            LoadAllDetailsToEditPage(_ItemId);
            //MPE_EDITITEM.Show();
        }
        
        protected void Lnk_AddNewItem_Click(object sender, EventArgs e)
        {
            Pnl_AddInventory.Visible = true;
            Pnl_ItemList.Visible = false;
            Pnl_SearchArea.Visible = true;
            Pnl_EditItem.Visible = false;
         
            LoadCategoryToDropDown(1);
            LoadUnitTypeToDropDown();
        }

        private void SaveItemDetailsToTable(string _ItemName, int _Category, int _Stock, int _MinQty, int _MaxQty, double _Cost, string _Desc, string _UnitType)
        {
            try
            {
                int _ItemId = 0, _category = 0;
                string sql = "";
                string _Description = "Initial Stock";
                DateTime Today = DateTime.Now;
                string _CreatedUser = MyUser.UserName;
                sql = "INSERT into tblinv_item(ItemName,Category,TotalStock,MinQty,MaxQty,Cost,Description,UnitType) VALUES ('" + _ItemName + "'," + _Category + "," + _Stock + "," + _MinQty + "," + _MaxQty + "," + _Cost + ",'" + _Desc + "','" + _UnitType + "')";
                Myinventory.m_MysqlDb.ExecuteQuery(sql);
                sql = "select Id from tblinv_item where  ItemName='" + _ItemName + "'";
                MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    int opnqty = 0;
                    sql = "insert into tblinv_locationitemstock(ItemId,LocationId,Stock) values(" + int.Parse(MyReader.GetValue(0).ToString()) + ",1," + _Stock + ")";
                    Myinventory.m_MysqlDb.ExecuteQuery(sql);
                    Myinventory.UpdateOpenQuantity(int.Parse(MyReader.GetValue(0).ToString()), out opnqty);
                }

                sql = "SELECT tblinv_item.Id, tblinv_item.Category from tblinv_item  where tblinv_item.ItemName='" + _ItemName + "'";
                MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    _ItemId = int.Parse(MyReader.GetValue(0).ToString());
                    _category = int.Parse(MyReader.GetValue(1).ToString());
                }
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Inventory : Added New Item ", "Item " + _ItemName + " is Added", 1);
                //sql = "INSERT into tblinv_inputflow(ItemId,ItemName,CategoryId,NewStock,Description,PurchaseDate,CreatedUser) VALUES (" + _ItemId + ",'" + _ItemName + "'," + _category + "," + _Stock + ",'" + _Description + "','" + Today.ToString("s") + "','" + _CreatedUser + "')";
                //Myinventory.m_MysqlDb.ExecuteQuery(sql);
            }
            catch (Exception e)
            {
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Inventory :  Error in Adding new item ", "Error " + e.Message + " is Added", 1);
             
                WC_MessageBox.ShowMssage(e.ToString());
            }
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            string _ItemName = "";
            int _Category = 0, _Stock = 0, _MinQty = 0, _MaxQty = 0;
            double _Cost = 0;
            _Category = int.Parse(Drp_Category.SelectedValue);
            _Stock = int.Parse(txt_Stock.Text.Trim());
            _MinQty = int.Parse(txt_minqty.Text.Trim());
            _MaxQty = int.Parse(txt_maxqty.Text.Trim());
            _Cost = double.Parse(txt_cost.Text.Trim());
            string _Desc = "", _UnitType = "";
            _Desc = txt_description.Text.Trim();
            _UnitType = Drp_Unit.SelectedItem.Text;

            _ItemName = txt_Inventoryname.Text.Trim();
            if (_Stock < _MaxQty)
            {
                if (!NameAllReadyExists(_ItemName))
                {
                    SaveItemDetailsToTable(_ItemName, _Category, _Stock, _MinQty, _MaxQty, _Cost, _Desc, _UnitType);
                    if (Chk_AddMore.Checked == true)
                    {
                        Pnl_AddInventory.Visible = true;
                        Pnl_ItemList.Visible = false;
                        Pnl_SearchArea.Visible = true;
                        Pnl_EditItem.Visible = false;
                   
                       // Lnk_AddNewItem.Visible = false;
                        Btn_InputFlow.Visible = false;
                        Btn_OutputFlow.Visible = false;
                        Btn_Issue.Visible = true;
                        WC_MessageBox.ShowMssage("New Item is created successfully..");
                        Clear();
                        LoadCategoryToDropDown(1);
                    }
                    else
                    {
                        WC_MessageBox.ShowMssage("New Item is created successfully..");
                        Clear();
                        LoadInitialStage();
                        FillItemGrid("", 0);
                        LoadCategoryToDropDown(0);
                    }
                }
                else
                {
                    WC_MessageBox.ShowMssage("Item Name already exists!!");
                }
            }
            else
            {
                WC_MessageBox.ShowMssage("Stock should be less tham Max-Quantity!!");
            }
        }

        protected void Btn_clear_Click(object sender, EventArgs e)
        {
            Clear();
            LoadInitialStage();
            FillItemGrid("", 0);
            LoadCategoryToDropDown(0);
        }

        protected void Btn_Update_Click(object sender, EventArgs e)
        {
            string _Name = "", _Desc = "", _Unit = "";
            int _maxQty = 0, _MinQty = 0, _Category = 0;
            double _Cost = 0;
            _Name = txt_EditItemName.Text.Trim();
            _Desc = txt_EditDesc.Text.Trim();
            _Unit = Drp_EditUnit.SelectedItem.Text;
            _maxQty = int.Parse(txt_EditMaxqty.Text.Trim());
            _MinQty = int.Parse(txt_EditMinQty.Text.Trim());
            _Cost = double.Parse(txt_EditCost.Text.Trim());
            _Category = int.Parse(Drp_EditCategory.SelectedValue);

            UpdateAllDetailsOfTheselectedItem(int.Parse(lbl_ItemId.Text.Trim()), _Name, _Desc, _Unit, _maxQty, _MinQty, _Cost, _Category);
            LoadAllDetailsToEditPage(int.Parse(lbl_ItemId.Text.Trim()));
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Inventory : Update Item", "Item " + lbl_Name.Text + " is updated", 1);
            WC_MessageBox.ShowMssage("Item is updated..");
        }

        protected void Btn_EditItem_Click(object sender, EventArgs e)
        {
            LoadAllStyles();
        }

        protected void Btn_Delete_Click(object sender, EventArgs e)
        {
            MPE_DeleteConfirm.Show();
        }

        protected void Btn_DeleteYes_Click(object sender, EventArgs e)
        {
            string sql = "", _ItemName = "", _User = "", _Description = "Item Deleted";
            DateTime _Today = DateTime.Now;
            _User = MyUser.UserName;
            int _ItemId = int.Parse(lbl_ItemId.Text.Trim());
            int _Cat = 0, _Stock = 0;
            sql = "SELECT tblinv_item.ItemName, tblinv_item.Category, tblinv_item.TotalStock from tblinv_item where tblinv_item.Id=" + _ItemId;
            MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _ItemName = MyReader.GetValue(0).ToString();
                _Cat = int.Parse(MyReader.GetValue(1).ToString());
                _Stock = int.Parse(MyReader.GetValue(2).ToString());
            }

            //  SaveOutFlowItemDetailsToTable(_ItemId, _ItemName, _Cat, _Stock, _Description, _Today, _User, 0);

            sql = "DELETE from tblinv_item where tblinv_item.Id=" + _ItemId;
            Myinventory.m_MysqlDb.ExecuteQuery(sql);
            sql = "DELETE from tblinv_locationitemstock where tblinv_locationitemstock.ItemId=" + _ItemId;
            Myinventory.m_MysqlDb.ExecuteQuery(sql);
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Inventory : Delete Item", "Item " + _ItemName + " is deleted", 1);
            Clear();
            LoadInitialStage();
            FillItemGrid("", 0);
            LoadCategoryToDropDown(0);
        }
        //Methods

        private void LoadUnitTypeToDropDown()
        {
            Drp_Unit.Items.Clear();

            string sql = "SELECT tblinv_unittype.UnitName, tblinv_unittype.Id from tblinv_unittype order by tblinv_unittype.Id asc";
            MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_Unit.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Unit Found", "-1");
                Drp_Unit.Items.Add(li);
            }
        }

        private bool NameAllReadyExists(string _ItemName)
        {
            bool _Exists = false;
            string sql = "SELECT tblinv_item.ItemName from tblinv_item where tblinv_item.ItemName='" + _ItemName + "'";
            MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _Exists = true;
            }
            return _Exists;
        }

        private void LoadEditStyles()
        {
            Btn_EditItem.Text = "Edit";
            Btn_Update.Visible = false;
            Btn_EditItem.Visible = true;
            txt_EditItemName.Visible = false;
            Drp_EditCategory.Visible = false;
            txt_EditMaxqty.Visible = false;
            txt_EditMinQty.Visible = false;
            txt_EditCost.Visible = false;
            txt_EditDesc.Visible = false;
        

            Drp_EditUnit.Visible = false;
            lbl_EditUnit.Visible = true;
            lbl_Category.Visible = true;
            lbl_Name.Visible = true;
            lbl_desc.Visible = true;
            lbl_minqty.Visible = true;
            lbl_maxqty.Visible = true;
            lbl_cost.Visible = true;
            Lnk_EditnewCategory.Visible = false;
        } 

        private void LoadEditUnitTypeDropDown(string _SelectedUnit)
        {
            Drp_EditUnit.Items.Clear();
            string sql = "SELECT tblinv_unittype.UnitName, tblinv_unittype.Id from tblinv_unittype order by tblinv_unittype.Id asc";
            MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_EditUnit.Items.Add(li);
                }
                Drp_EditUnit.SelectedItem.Text = _SelectedUnit.ToString();
            }
            else
            {
                ListItem li = new ListItem("No Unit Found", "-1");
                Drp_Unit.Items.Add(li);
            }
        }

        private void LoadAllDetailsToEditPage(int _ItemId)
        {
            LoadEditStyles();

            string sql = "SELECT * from tblinv_item where tblinv_item.Id=" + _ItemId;
            MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int _Category = int.Parse(MyReader.GetValue(2).ToString());
                lbl_Name.Text = ":       " + MyReader.GetValue(1).ToString();
                lbl_Stock.Text = ":       " + MyReader.GetValue(3).ToString();
                lbl_minqty.Text = ":       " + MyReader.GetValue(4).ToString();
                lbl_maxqty.Text = ":       " + MyReader.GetValue(5).ToString();
                lbl_cost.Text = ":       " + MyReader.GetValue(6).ToString();
                lbl_desc.Text = ":       " + MyReader.GetValue(7).ToString();
                lbl_EditUnit.Text = ":       " + MyReader.GetValue(8).ToString();
                string _Unit = MyReader.GetValue(8).ToString();
                txt_EditItemName.Text = MyReader.GetValue(1).ToString();
                txt_EditMinQty.Text = MyReader.GetValue(4).ToString();
                txt_EditMaxqty.Text = MyReader.GetValue(5).ToString();
                txt_EditCost.Text = MyReader.GetValue(6).ToString();
                txt_EditDesc.Text = MyReader.GetValue(7).ToString();

                lbl_Category.Text = ":       " + GetcategoryName(_Category);

                LoadEditCategoryDropDown(_Category);

                LoadEditUnitTypeDropDown(_Unit.Trim());
            }

        }

        private string GetcategoryName(int _Category)
        {
            string Catname = "";
            string sql = "SELECT tblinv_category.Category from tblinv_category where tblinv_category.Id=" + _Category;
            MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Catname = MyReader.GetValue(0).ToString();
            }
            return Catname;
        }

        private void LoadEditCategoryDropDown(int _SelectedCat)
        {
            Drp_EditCategory.Items.Clear();

            string sql = "SELECT tblinv_category.Category, tblinv_category.Id from tblinv_category order by tblinv_category.Category";
            MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_EditCategory.Items.Add(li);

                }
                Drp_EditCategory.SelectedValue = _SelectedCat.ToString();
            }
            else
            {
                ListItem li = new ListItem("No Category Found", "-1");
                Drp_EditCategory.Items.Add(li);
            }
        }       

        private void UpdateAllDetailsOfTheselectedItem(int _ItemId, string _Name, string _Desc, string _Unit, int _maxQty, int _MinQty, double _Cost, int _Category)
        {
            string sql = "UPDATE  tblinv_item set ItemName='" + _Name + "',Category=" + _Category + ",MinQty=" + _MinQty + ",MaxQty=" + _maxQty + ",Cost=" + _Cost + ",Description='" + _Desc + "',UnitType='" + _Unit + "' where tblinv_item.Id=" + _ItemId;
            
            Myinventory.m_MysqlDb.ExecuteQuery(sql);
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Inventory : Update Item Item", "Item " + _Name + " is deleted", 1);
          
        }       

        private void LoadAllStyles()
        {
            Btn_EditItem.Visible = false;
            Btn_Update.Visible = true;
            txt_EditItemName.Visible = true;
            Drp_EditCategory.Visible = true;
            txt_EditMaxqty.Visible = true;
            txt_EditMinQty.Visible = true;
            txt_EditCost.Visible = true;
            txt_EditDesc.Visible = true;
      
            Drp_EditUnit.Visible = true;
            lbl_EditUnit.Visible = false;
            lbl_Category.Visible = false;
            lbl_Name.Visible = false;
            lbl_desc.Visible = false;
            lbl_minqty.Visible = false;
            lbl_maxqty.Visible = false;
            lbl_cost.Visible = false;
            Lnk_EditnewCategory.Visible = true;

        }
        
        private void Clear()
        {
            txt_Inventoryname.Text = "";
            //txt_Stock.Text = "";
            //txt_minqty.Text = "";
            //txt_maxqty.Text = "";
            txt_cost.Text = "";
            TxtSearch.Text = "";
            txt_description.Text = "";
            Chk_AddMore.Checked = false;
        }

        #endregion

        
    }
}
