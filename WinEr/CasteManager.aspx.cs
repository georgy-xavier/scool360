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
    public partial class CasteManager : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();

            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(770))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (!IsPostBack)
                {
                    LoadInitialValues();
                    GrdVew_Category.Columns[0].Visible = false;
                    Grd_Caste.Columns[0].Visible = false;
                }
            }
        }

        private void LoadInitialValues()
        {
            LoadCategories();
            LoadCategoryGrd();

        }
        
        private void LoadCategoryGrd()
        {
            Lbl_CategoryErr.Text = "";
            GrdVew_Category.Columns[0].Visible = true;
            MyDataSet = LoadCategoriGrid();
            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count>0)
            {

                GrdVew_Category.DataSource = MyDataSet;
                GrdVew_Category.DataBind();
            }
            else
            {
                GrdVew_Category.DataSource = null;
                GrdVew_Category.DataBind();
                Lbl_CategoryErr.Text = "No categories found";
            }
            GrdVew_Category.Columns[0].Visible = false;
            
        }

        private void LoadCategories()
        {
            MyReader = GetInitialvalues();
            Drp_Category.Items.Clear();
            Drp_Shocategory.Items.Clear();
            Drp_EditCaste.Items.Clear();
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Category.Items.Add(li);
                    Drp_Shocategory.Items.Add(li);
                    Drp_EditCaste.Items.Add(li);
                }
                
            }
            else
            {
                ListItem li = new ListItem("No Categories Found", "-1");
                Drp_Category.Items.Add(li);
                Drp_Shocategory.Items.Add(li);
                Drp_EditCaste.Items.Add(li);
            }
        }

        private OdbcDataReader GetInitialvalues()
        {
            string sql = "select tblcast_category.Id, tblcast_category.CategoryName from tblcast_category  order by tblcast_category.CategoryName ASC ";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            return MyReader;
        }

        private DataSet LoadCategoriGrid()
        {
            string sql = "select tblcast_category.Id, tblcast_category.CategoryName from tblcast_category  order by tblcast_category.CategoryName ASC ";
            MyDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return MyDataSet;
        }

       

        private void FillGrid(int selectedValues)
        {
            Lbl_ShowErr.Text = "";
            string sql = "select tblcast.Id, tblcast.castname , tblcast_category.CategoryName from tblcast inner join tblcast_category on tblcast_category.Id= tblcast.CategoryId where tblcast_category.Id=" + selectedValues + " Order by tblcast.castname asc ";
            MyDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            Grd_Caste.Columns[0].Visible = true;
            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                Grd_Caste.DataSource = MyDataSet;
                Grd_Caste.DataBind();
            }
            else
            {
                Grd_Caste.DataSource = null;
                Grd_Caste.DataBind();
                Lbl_ShowErr.Text = "No caste  found";
            }
            Grd_Caste.Columns[0].Visible = false;
        }
        
        protected void Btn_ShowCaste_Click(object sender, EventArgs e)
        {
            int selectedValues=0;
            int.TryParse(Drp_Shocategory.SelectedValue.ToString(), out selectedValues);
            if (selectedValues != -1)
                FillGrid(selectedValues);
            else
                WC_MessageBox.ShowMssage("Select a category");
        }

        protected void Btn_AddCategory_Click(object sender, EventArgs e)
        {
            Lbl_CategoryError.Text = "";
            if (Txt_Category.Text.ToString().Trim() == "")
            {
                Lbl_CategoryError.Text = "Please enter category name";
            }
            else
            {
                string Category = Txt_Category.Text.ToString().Trim();
                string categoryExist = "Select tblcast_category.Id from tblcast_category where tblcast_category.CategoryName='" + Category + "'";
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(categoryExist);
                General MyGenObj = new General(MyStudMang.m_MysqlDb);
                int MaxId = MyGenObj.GetTableMaxId("tblcast_category", "Id");

                if (!MyReader.HasRows)
                {
                    string sql = "insert into tblcast_category (tblcast_category.Id,tblcast_category.CategoryName) values(" + MaxId + ",'" + Category + "')";
                    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    LoadInitialValues();
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Cast Manager", "" + Category + " Cast category added", 1);
                    Lbl_CategoryError.Text = "Category successfully added";
                    Txt_Category.Text = "";
                   
                }
                else
                {
                    Lbl_CategoryError.Text="Category already exist";
                    Txt_Category.Text = "";
                   
                }
            }
            MPE_Categories.Show();
        }

        protected void Btn_AddCaste_Click(object sender, EventArgs e)
        {
            Lbl_Cast_Error.Text = "";
            string  Caste  = Txt_NewCastName.Text.ToString().Trim();
            if (Caste == "")
            {
                Lbl_Cast_Error.Text = "Please enter caste name";
            }
            else
            {
                int CategoryID = int.Parse(Drp_Category.SelectedValue.ToString());

                string CastExist = "select tblcast.Id from tblcast where tblcast.castname='" + Caste + "' and tblcast.CategoryId=" + CategoryID;

                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(CastExist);
                General MyGenObj = new General(MyStudMang.m_MysqlDb);
                int MaxId = MyGenObj.GetTableMaxId("tblcast", "Id");

                if (!MyReader.HasRows)
                {
                    string sql = "insert into tblcast (tblcast.Id,tblcast.castname, tblcast.CategoryId) values(" + MaxId + ",'" + Caste + "'," + CategoryID + ")";
                    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    FillGrid(CategoryID);
                    Drp_Shocategory.SelectedValue = CategoryID.ToString();
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Cast Manager", "" + Caste + " Cast added", 1);
                    Lbl_Cast_Error.Text = "Caste successfully added";
                    Txt_NewCastName.Text = "";
                   

                }
                else
                {
                    Lbl_Cast_Error.Text = "Caste already exist";
                    Txt_NewCastName.Text = "";
                    
                }
            }
            MPE_AddCast.Show();
        }
        protected void GrdVew_Category_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            
            LoadCategoryGrd();
            GrdVew_Category.PageIndex = e.NewPageIndex;
        }

        protected void Grd_Caste_Category_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            int selectedValues = 0;
            int.TryParse(Drp_Shocategory.SelectedValue.ToString(), out selectedValues);
            if (selectedValues != -1)
                FillGrid(selectedValues);
            Grd_Caste.PageIndex = e.NewPageIndex;  
        }
        protected void GrdVew_CategorySelectedIndexChanged(object sender, EventArgs e)
        {
            clear();
            int CategoryId = 0;
            int.TryParse(GrdVew_Category.SelectedRow.Cells[0].Text.ToString(), out CategoryId);
            string  CategoryName      = GrdVew_Category.SelectedRow.Cells[1].Text.ToString();

            Lbl_CategoryID.Text       = CategoryId.ToString();
            Txt_EditCategoryName.Text = CategoryName;

            MPE_EditCategory.Show();
        }
        private void clear()
        {
            Lbl_EditCategry_Error.Text = "";
            
        }
        protected void Btn_UpdateCategory_Click(object sender ,EventArgs e)
        {
            
            Lbl_EditCategry_Error.Text = "";
            if (Txt_EditCategoryName.Text.ToString().Trim() == "")
            {
                Lbl_EditCategry_Error.Text = "Please enter the category name";
            }
            else
            {
                string sql = "update tblcast_category set tblcast_category.CategoryName='" + Txt_EditCategoryName.Text.ToString().Trim() + "' where tblcast_category.Id=" + int.Parse(Lbl_CategoryID.Text.ToString().Trim());
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Cast Manager", "" + Txt_EditCategoryName.Text.ToString().Trim() + " Category Updated", 1);
                Lbl_EditCategry_Error.Text = "Category Updated";
                MPE_EditCategory.Show();
            }
            LoadCategoryGrd();
           
            

        }
        protected void imgbtn_addCategory_Click(object sender, EventArgs e)
        {
            Lbl_CategoryError.Text = "";
            MPE_Categories.Show();
        }
        protected void LnkBtn_AddCategory_Click(object sender, EventArgs e)
        {
            Lbl_CategoryError.Text = "";
            MPE_Categories.Show();
        }
        
        protected void ImgAddCast_Click(object sender, EventArgs e)
        {
            Lbl_Cast_Error.Text = "";
            MPE_AddCast.Show();
        }
        protected void Lnk_AddCast_Click(object sender, EventArgs e)
        {
            Lbl_Cast_Error.Text = "";
            MPE_AddCast.Show();
        }
        
        protected void Grd_CasterSelectedIndexChanged(object sender, EventArgs e)
        {
            Lbl_EditCast_Error.Text = "";
            int CasteId = 0;
            Lbl_Cast_Error.Text = "";
            int.TryParse(Grd_Caste.SelectedRow.Cells[0].Text.ToString(), out CasteId);
            string Category    = Grd_Caste.SelectedRow.Cells[2].Text.ToString();
            string CastName    = Grd_Caste.SelectedRow.Cells[1].Text.ToString();
        
            int CategoryId=GetCategoryIdfromCastId(CasteId ,out Category);
            Drp_EditCaste.SelectedValue = CategoryId.ToString();

            Txt_EditCaste.Text=CastName;
            
            Lbl_EditCaste_CasteId.Text = CasteId.ToString();
            Lbl_EditCaste_CatId.Text = CategoryId.ToString();
            MPE_EditCaste.Show();
        }
        protected void Btn_UpdateCaste_Click(object sender, EventArgs e)
        {
            Lbl_EditCast_Error.Text = "";
            string selectedValue = Drp_Shocategory.SelectedValue;
            if(Txt_EditCaste.Text.ToString()=="")
            {
                Lbl_EditCast_Error.Text="Please enter the caste name";
            }
            else
            {
                int CastId = 0;
                int CategoryId = 0;
                int oldCatId = 0;
                int.TryParse(Lbl_EditCaste_CasteId.Text.ToString(),out CastId);
                int.TryParse(Drp_EditCaste.SelectedValue.ToString(),out CategoryId);
                string CastName = Txt_EditCaste.Text.ToString();
                string sql = "update tblcast set tblcast.castname='" + CastName + "' , tblcast.CategoryId=" + CategoryId + " where tblcast.Id=" + CastId;
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Cast Manager", "" + CastName + " Cast Updated", 1);
                Lbl_EditCast_Error.Text = "Caste Name updated";
               
                Txt_EditCaste.Text = "";
                LoadInitialValues();
                int.TryParse(Lbl_EditCaste_CatId.Text.ToString(),out oldCatId);
                FillGrid(oldCatId);
                Drp_Shocategory.SelectedValue = selectedValue;
                MPE_EditCaste.Show();
            }

        }
        protected void GrdVew_Category_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               // LinkButton l = (LinkButton)e.Row.FindControl("lnk_DeleteCategory");
                ImageButton l = (ImageButton)e.Row.FindControl("ImgBtn");
                l.Attributes.Add("onclick", "javascript:return " +
                     "confirm('Are you sure you want to delete the category " +
                     DataBinder.Eval(e.Row.DataItem, "CategoryName") + " ')");
            }
        }

        protected void GrdVew_Category_RowData_Delete(object sender, GridViewDeleteEventArgs e)
        {
            int _CategoryID = 0;
            Lbl_CategoryError.Text = "";
            int.TryParse(GrdVew_Category.Rows[e.RowIndex].Cells[0].Text.ToString(), out _CategoryID);

            string sql = " select tblcast.Id from tblcast where tblcast.CategoryId="+_CategoryID;
             MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (!MyReader.HasRows)
            {
                string sql_delete = "delete from tblcast_category where tblcast_category.Id=" + _CategoryID;
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql_delete);
                LoadInitialValues();
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Cast Manager", "Cast category deleted", 1);
                WC_MessageBox.ShowMssage("Selected category deleted successfully");

            }
            else
            {
               
                WC_MessageBox.ShowMssage("Selected category cannot deleted");
            }


        }

        protected void Grd_Caste_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                ImageButton l = (ImageButton)e.Row.FindControl("ImgBtnDeleteCaste");
                l.Attributes.Add("onclick", "javascript:return " +
                     "confirm('Are you sure you want to delete the caste " +
                     DataBinder.Eval(e.Row.DataItem, "castname") + " ')");
            }
        }

        protected void Grd_Caste_RowData_Delete(object sender, GridViewDeleteEventArgs e)
        {
            Lbl_CastId.Text = "";
            Lbl_CatId.Text = "";
            string CategoryName = "";
            int _casteID = 0;
            Drp_NewCast.Items.Clear();
            int.TryParse(Grd_Caste.Rows[e.RowIndex].Cells[0].Text.ToString(), out _casteID);

            int _categoryId=GetCategoryIdfromCastId(_casteID,out CategoryName);

            if (!IsCasteUsed(_casteID))
            {
                string sql_delete="delete from tblcast where tblcast.Id="+_casteID;
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql_delete);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Cast Manager", "Cast deleted", 1);
                //Fill grid after delete a caste
                FillGrid(_categoryId);

                WC_MessageBox.ShowMssage("Selected caste deleted successfully");

            }
            else
            {
                 string sql_newcast="select tblcast.id , tblcast.castname from tblcast where tblcast.Id<> "+_casteID;
                 MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql_newcast);
                 if(MyReader.HasRows)
                 {
                     while(MyReader.Read())
                     {
                         ListItem li=new ListItem(MyReader.GetValue(1).ToString(),MyReader.GetValue(0).ToString());
                         Drp_NewCast.Items.Add(li);
                     }
                 }
                 ListItem l1 = new ListItem("Select New Categry","-1");
                 Drp_NewCast.Items.Add(l1);
                 Drp_NewCast.SelectedValue = "-1";
                 Lbl_CastId.Text = _casteID.ToString();
                 Lbl_CatId.Text = _categoryId.ToString();
                 Lbl_CastName.Text = Grd_Caste.Rows[e.RowIndex].Cells[1].Text.ToString();
                 MPE_UpdateStudCast.Show();
                
            }

        }
       
        protected void Btn_UpdateStudCaste_Click(object sender, EventArgs e)
        {
            int selectCast=0;
            int.TryParse(Drp_NewCast.SelectedValue.ToString(),out selectCast);
            if (Lbl_CastId.Text != "" && selectCast!=-1)
            {

                string sql = " update tblstudent set tblstudent.Cast=" + selectCast + " where tblstudent.Cast=" + int.Parse(Lbl_CastId.Text.ToString().Trim());
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                string sql_delete = "delete from tblcast where tblcast.Id=" + int.Parse(Lbl_CastId.Text.ToString().Trim());
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql_delete);
                //Fill grid after delete a caste
                FillGrid(int.Parse(Lbl_CatId.Text.ToString().Trim()));
                WC_MessageBox.ShowMssage("Selected caste deleted successfully.");
             
            }
            else
            {
                WC_MessageBox.ShowMssage("Cannot delete the selected caste.");
            }
            
        }

        private bool IsCasteUsed(int _casteID)
        {
            bool _castused = false;
            string sql = "select DISTINCT tblstudent.Id from tblstudent  where tblstudent.Cast=" + _casteID;
            MyReader   = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _castused = true;
            }

            return _castused;
        }

        private int GetCategoryIdfromCastId(int _casteID,out string CategoryName)
        {

            int     CategoryId   = 0;
                    CategoryName = "";
            string  sql = "select  tblcast_category.Id,tblcast_category.CategoryName  from tblcast_category inner join tblcast on tblcast.CategoryId= tblcast_category.Id where tblcast.Id=" + _casteID;

            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                MyReader.Read();

                int.TryParse(MyReader.GetValue(0).ToString(),out CategoryId);
                CategoryName = MyReader.GetValue(1).ToString();
            }

            return  CategoryId;
            
        }

                
    }
}
