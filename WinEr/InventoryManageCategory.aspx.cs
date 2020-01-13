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
    public partial class InventoryManageCategory : System.Web.UI.Page
    {
        private ConfigManager MyConfigMang;
        private KnowinUser MyUser;
        private WinBase.Inventory Myinventory;
        OdbcDataReader MyReader = null;


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
            else if (!MyUser.HaveActionRignt(867))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadCategory();
                    FillCategoryDropDown();
                }
            }
        }



        protected void Lnk_AddNewCategory_Click(object sender, EventArgs e)
        {
            Lbl_MsgCreateCategory.Text = "";
            Btn_Update.Visible = false;
            Btn_Add_new_cat.Visible = true;
            Btn_Delete.Visible = false;
            txt_new_category.Text = "";
            Lbl_Head.Text = "New Category";
            MPE_MessageBox_AddNewCategory.Show();


        }

        protected void Btn_Add_new_cat_Click(object sender, EventArgs e)
        {

            if (int.Parse(Drp_CategoryType.SelectedValue) > 0)
            {
                if (txt_new_category.Text != "")
                {

                    string categoryName = txt_new_category.Text.ToUpper();
                    string sql = "SELECT tblinv_category.Category from tblinv_category where tblinv_category.Category='" + categoryName + "'";
                    MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        Lbl_MsgCreateCategory.Text = "Category Name already exists";
                    }
                    else
                    {
                        int categorytype = int.Parse(Drp_CategoryType.SelectedValue);
                        Myinventory.SaveNewCategory(categoryName, categorytype);
                        LoadCategory();
                        txt_new_category.Text = "";
                        Lbl_MsgCreateCategory.Text = "New category created";
                    }
                }

            }
            else
            {
                Lbl_MsgCreateCategory.Text = "Select type";
            }
            MPE_MessageBox_AddNewCategory.Show();
        }

        protected void Grd_Category_SelectedIndexChanged(object sender, EventArgs e)
        {
            Lbl_MsgCreateCategory.Text = "";
            Btn_Update.Visible = true;
            Btn_Add_new_cat.Visible = false;
            Btn_Delete.Visible = true;
            txt_new_category.Text = "";
            Lbl_Head.Text = "Manage Category";
            FillCategoryDropDown();
            txt_new_category.Text = Grd_Category.SelectedRow.Cells[2].Text;
            Drp_CategoryType.SelectedValue = Grd_Category.SelectedRow.Cells[1].Text;
            HdnId.Value = Grd_Category.SelectedRow.Cells[0].Text; ;
            MPE_MessageBox_AddNewCategory.Show();
        }

        protected void Grd_Category_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Category.PageIndex = e.NewPageIndex;
            LoadCategory();

        }

        protected void Btn_Update_Click(object sender, EventArgs e)
        {
            string categoryName = txt_new_category.Text;
            string sql = "SELECT tblinv_category.Category from tblinv_category where tblinv_category.Category='" + categoryName + "' and Id<>" + int.Parse(HdnId.Value) + "";
            MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Lbl_MsgCreateCategory.Text = "Category Name already exists";
            }
            else
            {
                if (int.Parse(Drp_CategoryType.SelectedValue) > 0)
                {

                    int id = int.Parse(HdnId.Value);
                    int categorytype = int.Parse(Drp_CategoryType.SelectedValue);
                    Myinventory.UpadteCategory(id, categoryName, categorytype);
                    LoadCategory();
                    txt_new_category.Text = "";
                    Lbl_MsgCreateCategory.Text = "category Updated";
                }
                else
                {
                    Lbl_MsgCreateCategory.Text = "Select type";
                }
            }
            MPE_MessageBox_AddNewCategory.Show();
        }

        protected void Btn_Delete_Click(object sender, EventArgs e)
        {

            string sql = "";
            sql = "select Id from tblinv_item where tblinv_item.Category=" + int.Parse(HdnId.Value) + " ";
            MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Lbl_MsgCreateCategory.Text = "Cannot delete,Category assigned to some items";
            }
            else
            {
                sql = "Delete from tblinv_category where Id=" + int.Parse(HdnId.Value) + " ";
                Myinventory.m_MysqlDb.ExecuteQuery(sql);
                Lbl_MsgCreateCategory.Text = "Category deleted";
            }
            LoadCategory();
            txt_new_category.Text = "";
            MPE_MessageBox_AddNewCategory.Show();
        }

        #endregion

        #region Methods

        private void FillCategoryDropDown()
        {
            Drp_CategoryType.Items.Clear();
            DataSet CategoryType_Ds = new DataSet();
            CategoryType_Ds = Myinventory.LoadCategoryType();
            if (CategoryType_Ds != null && CategoryType_Ds.Tables[0].Rows.Count > 0)
            {

                ListItem li;
                li = new ListItem("Select Type", "0");
                Drp_CategoryType.Items.Add(li);
                foreach (DataRow dr in CategoryType_Ds.Tables[0].Rows)
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




        }

        private void LoadCategory()
        {
            Lbl_Err.Text = "";
            DataSet category_Ds = new DataSet();
            int type = 0;
            category_Ds = Myinventory.GetAllCategoryDetails(type);
            if (category_Ds != null && category_Ds.Tables[0].Rows.Count > 0)
            {
                Pnl_ManageCategory.Visible = true;
                Grd_Category.Columns[0].Visible = true;
                Grd_Category.Columns[1].Visible = true;
                Grd_Category.DataSource = category_Ds;
                Grd_Category.DataBind();
                Grd_Category.Columns[0].Visible = false;
                Grd_Category.Columns[1].Visible = false;

            }
            else
            {
                Grd_Category.DataSource = null;
                Grd_Category.DataBind();
                Pnl_ManageCategory.Visible = false;
                Lbl_Err.Text = "No category exist";

            }
        }

        #endregion


        
    }
}
