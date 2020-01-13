using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.Odbc;
using WinBase;
namespace WinEr
{
    public partial class LibConfig : System.Web.UI.Page
    {
        private LibraryManagClass MyLibMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyLibMang = MyUser.GetLibObj();
            if (MyLibMang == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }
            else
            {
                if (!IsPostBack)
                {
                    Btn_Upcat.Visible = false;
                    Btn_UpType.Visible = false;
                    Btn_UpRack.Visible = false;
                    Pnl_CatGrd.Visible = false;
                    Pnl_Type_Grd.Visible = false;
                    Pnl_RackGrid.Visible = false;
                    LoadCatToGrid();
                    LoadAutoNumSatatus();
                    LoadFineDetails();
                    LoadBookTypes();
                    LoadRacks();
                    LoadLimitBookLimit();
                    LoadBarcodeConfig();
                    //sai changes for Auto Book Id
                    Btn_AutoNum_Save.Enabled=false;
                    Drp_AutoNum.Enabled = false;
                    //
                   
                }
            }

        }

        private void LoadBarcodeConfig()
        {
            LoadBarcodeConfiguration_IsActive();
            LoadBarcodeConfiguration_UseUniqueNumber();
            loadbarcodetextneeded_ornot();
            load_Barcode_Type();
            load_Barcode_prefix();
            //sai put comments
            //if (CheckBooksIsalreadyEnterd())
            //{
            //    Rdo_UniqueBarcode.Enabled = false;
            //}
            if (!MyLibMang.IsBarcodeActive())
            {
                Txt_barcode_prefix.Enabled = false;
                rdb_barcode_text.Enabled = false;
                rdb_barcode_type.Enabled = false;
            }
        }

        private void LoadBarcodeConfiguration_UseUniqueNumber()
        {

            string sql = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.FieldName='UniqueBarcode'";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            int Use_UniqueNum = 0;
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out Use_UniqueNum);
            }
            if (Use_UniqueNum == 1)
            {
                Rdo_UniqueBarcode.SelectedValue = "1";
            }
            else
            {
                Rdo_UniqueBarcode.SelectedValue = "2";
            }
        }
        
        private void LoadBarcodeConfiguration_IsActive()
        {
            string sql = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.FieldName='ActiveBarcode'";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            int IsActive=0;
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out IsActive);
            }
            if (IsActive == 1)
            {
                Rdo_Barcodeactive.SelectedValue = "1";
            }
            else
            {
                Rdo_Barcodeactive.SelectedValue = "2";
                Rdo_UniqueBarcode.SelectedValue = "2";
                Rdo_UniqueBarcode.Enabled = false;
            }
        }
        //sai added for load barcode text needed or not and barcode automatic or manual and load_Barcode_prefix
        private void loadbarcodetextneeded_ornot()
        {
            string sql = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.FieldName='NeedBarcodeText'";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            int _IsNeeded = 0;
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out _IsNeeded);
            }
            if (_IsNeeded == 1)
            {
                rdb_barcode_text.SelectedValue = "1";
            }
            else
            {
                rdb_barcode_text.SelectedValue = "0";
            }
        }
        private void load_Barcode_Type()
        {
            string sql = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.FieldName='BarcodeAutomatic'";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            int _IsNeeded = 0;
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out _IsNeeded);
            }
            if (_IsNeeded == 1)
            {
                rdb_barcode_type.SelectedValue = "1";
            }
            else
            {
                rdb_barcode_type.SelectedValue = "0";
            }
        }
        private void load_Barcode_prefix()
        {
            string sql = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.FieldName='Barcodeprefix'";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                if (MyReader.GetValue(0).ToString() != "")
                {
                    Txt_barcode_prefix.Text = MyReader.GetValue(0).ToString();
                }
                else
                {
                    Txt_barcode_prefix.Text = "";
                }
            }
            else
            {
                Txt_barcode_prefix.Text = "";
            }
        }
        private void LoadLimitBookLimit()
        {
            string sql = "select tbllibconfig.Value1,tbllibconfig.Value2 from tbllibconfig where tbllibconfig.FieldName='Limit'";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Txt_IssueLimit.Text = MyReader.GetValue(0).ToString();
                Txt_IssueLimit_st.Text = MyReader.GetValue(1).ToString();
               
            }
        }

        private void LoadRacks()
        {
            lbl_RackError.Text = "";
            string sql = "select tblbookrack.Id,tblbookrack.RackName, SUBSTRING(tblbookrack.`Desc`,1,30) as `Desc` from tblbookrack";
            MydataSet = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                Grid_Rack.Columns[0].Visible = true;
                Grid_Rack.DataSource = MydataSet;
                Grid_Rack.DataBind();
                Grid_Rack.Columns[0].Visible = false;
                Pnl_RackGrid.Visible = true;
            }
            else
            {
                Grid_Rack.DataSource = null;
                Grid_Rack.DataBind();
                Pnl_RackGrid.Visible = false;
               // Lbl_msg.Text = "No Rack found";
               // this.MPE_MessageBox.Show();
                lbl_RackError.Text="No Rack found";
            }
            Txt_RackNo.Text = "";
            Txt_RackId.Text = "";
            Txt_RackDesc.Text = "";
        }

        private void LoadCatToGrid()
        {
            Lbl_CategoryError.Text = "";
          
            string sql = "select tblbookcatogory.Id, tblbookcatogory.CatogoryName, SUBSTRING(tblbookcatogory.Disc,1,30) as Disc from tblbookcatogory";
            MydataSet = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_Catagory.Columns[0].Visible = true;
                Grd_Catagory.DataSource = MydataSet;
                Grd_Catagory.DataBind();
                Grd_Catagory.Columns[0].Visible = false;
                Pnl_CatGrd.Visible = true;
            }
            else
            {
                Grd_Catagory.DataSource = null;
                Grd_Catagory.DataBind();
                Lbl_CategoryError.Text = "No Category found";
               // Lbl_msg.Text = "No Catagory found";
               // this.MPE_MessageBox.Show();
            }
            Txt_CatId.Text = "";
            Text_Category.Text = "";
            Text_Desc_Cat.Text = "";
        }

        private void LoadBookTypes()
        {
            Lbl_BookType_Error.Text= "";
            string sql = "select tblbooktype.Id, tblbooktype.TypeName, SUBSTRING(tblbooktype.`Desc`,1,30) as `Desc` from tblbooktype";
            MydataSet = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_ViewType.Columns[0].Visible = true;
                Grd_ViewType.DataSource = MydataSet;
                Grd_ViewType.DataBind();
                Grd_ViewType.Columns[0].Visible = false;
                Pnl_Type_Grd.Visible = true;
            }
            else
            {
                Grd_ViewType.DataSource = null;
                Grd_ViewType.DataBind();
                Lbl_BookType_Error.Text = "No Type found";
                //Lbl_msg.Text = "No Type found";
                //this.MPE_MessageBox.Show();
            }
            Txt_TypeId.Text = "";
            Text_Type.Text = "";
            Text_Des_Type.Text = "";
        }

        private void LoadFineDetails()
        {
            string sql = "select tbllibconfig.Value1, tbllibconfig.Value2 from tbllibconfig where tbllibconfig.FieldName='Fine'";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Text_Fine.Text = MyReader.GetValue(0).ToString();
                Text_MaxDay.Text = MyReader.GetValue(1).ToString();
            }
            sql = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.FieldName='StaffFine'";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Text_Fine_staff.Text = MyReader.GetValue(0).ToString();
            }
        }

        private void LoadAutoNumSatatus()
        {

            string sql = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.ConfigName='AutoBookId'";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql); 
            if (MyReader.HasRows)
            {
                Drp_AutoNum.SelectedValue = MyReader.GetValue(0).ToString();
               
            }
        }

        protected void Btn_AutoNum_Save_Click(object sender, EventArgs e)
        {
            try
            {
                int value = int.Parse(Drp_AutoNum.SelectedValue.ToString());
                string sql = "update tbllibconfig set Value1='" + value + "' where ConfigName='AutoBookId'";
                MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "Auto Book Id Configuration updated", 1);
                Lbl_msg.Text = "The action is done";
                this.MPE_MessageBox.Show();
            }
            catch
            {
                Lbl_msg.Text = "Please try again";
                this.MPE_MessageBox.Show();
            }
        }

        protected void Btn_Fine_Save_Click(object sender, EventArgs e)
        {
            if (Text_Fine.Text.Trim() == "" || Text_MaxDay.Text.Trim() == "" || Text_Fine_staff.Text=="")
            {
                Lbl_msg.Text = "Please Fill all the fields";
                this.MPE_MessageBox.Show();
            }
            else
            {
                try
                {
                    MyLibMang.SaveFine(Text_Fine.Text, Text_MaxDay.Text, Text_Fine_staff.Text);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "Book Fine details updated", 1);
                    Lbl_msg.Text = "Fine Details are saved";
                    this.MPE_MessageBox.Show();
                }
                catch
                {
                    Lbl_msg.Text = "Please try again";
                    this.MPE_MessageBox.Show();
                }
               
            }
        }

        protected void Btn_Rack_Click(object sender, EventArgs e)
        {
            string message;
            lbl_RackError.Text = "";
            if (ValidRack(out message))
            {
                try
                {
                    MyLibMang.AddRack(Txt_RackNo.Text.Trim(), Txt_RackDesc.Text.Trim());
                    
                    Lbl_msg.Text = "The rack is saved";
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "" + Txt_RackNo.Text.Trim() + " rack added", 1);
                    this.MPE_MessageBox.Show();
                    LoadRacks();
                    Txt_RackNo.Text = "";
                    Txt_RackDesc.Text = "";
                }
                catch
                {
                    Lbl_msg.Text = "Please try again";
                    this.MPE_MessageBox.Show();
                }
            }
            else
            {
                Lbl_msg.Text = message;
                this.MPE_MessageBox.Show();
            }
        }

        private bool ValidRack(out string message)
        {
            message = "";
            bool _valid = true;
           
            if(Txt_RackNo.Text.Trim()== "")
            {
                message = "Please fill Rack Number";
                _valid = false;
            }
            else if (!MyLibMang.RackExists(Txt_RackNo.Text.Trim()))
            {
                message = "Rack Number already exists";
                _valid = false;
            }
            return _valid;

        }

        protected void But_Type_Save_Click(object sender, EventArgs e)
        {
            Lbl_BookType_Error.Text = "";
            string message;
            if (validType(out message))
            {
                try
                {
                    MyLibMang.SaveBookType(Text_Type.Text, Text_Des_Type.Text);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "" + Text_Type.Text + " Book Type added", 1);
                    LoadBookTypes();
                   
                    Lbl_BookType_Error.Text = "Book Type is saved";
                   // Lbl_msg.Text = "Book Type is saved";
                    //this.MPE_MessageBox.Show();
                    Text_Type.Text = "";
                    Text_Des_Type.Text = "";
                }
                catch
                {
                    Lbl_BookType_Error.Text = "Please try again";
                   // Lbl_msg.Text = "Please try again";
                  //  this.MPE_MessageBox.Show();
                }
            }
           
            else
            {
                Lbl_msg.Text =message;
                this.MPE_MessageBox.Show();
            }
        }

        private bool validType(out string message)
        {
            bool _valid = true;
            message = "";
            if (Text_Type.Text.Trim() == "")
            {
                message = "Please fill type name";
                _valid = false;
            }
            else if (!MyLibMang.TypeExists(Text_Type.Text.Trim()))
            {
                message = "Type already exists";
                _valid = false;
            }
            return _valid;
        }

        protected void But_AddCat_Click(object sender, EventArgs e)
        {
            Lbl_CategoryError.Text = "";
            string message;
            if (ValidCat(out message))
            {
                try
                {
                    MyLibMang.SaveBookCategory(Text_Category.Text, Text_Desc_Cat.Text);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "" + Text_Category.Text + " Book category added", 1);
                    LoadCatToGrid();
                    
                    Lbl_CategoryError.Text = "Book category is saved";
                    //Lbl_msg.Text = "Book category is saved";
                    //this.MPE_MessageBox.Show();
                    Text_Category.Text = "";
                    Text_Desc_Cat.Text = "";
                }
                catch
                {
                    Lbl_msg.Text = "Please try again";
                    this.MPE_MessageBox.Show();
                }
            }
            
            else
            {
                Lbl_msg.Text = message;
                this.MPE_MessageBox.Show();
            }
        }

        private bool ValidCat(out string message)
        {
            message = "";
            bool _valid= true;
            if (Text_Category.Text.Trim() == "")
            {
                message = "Please fill category name";
                _valid = false;
            }
            else if (!MyLibMang.categoryExists(Text_Category.Text.Trim()))
            {
                message = "Category already exists";
                _valid = false;
            }
            return _valid;
        }

        protected void Grd_ViewType_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string _message;
            int _TypeId = int.Parse(Grd_ViewType.DataKeys[e.RowIndex].Value.ToString());
           
            if (MyLibMang.DeleteTypeById(_TypeId, out _message))
            {
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "Book type deleted", 1);
                LoadBookTypes();
                Lbl_msg.Text = _message;
            }
            else
            {
                Lbl_msg.Text = _message;
            }
            this.MPE_MessageBox.Show();

        }

        protected void Grd_ViewType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton l = (LinkButton)e.Row.FindControl("LinkButton1");
                l.Attributes.Add("onclick", "javascript:return " +
                     "confirm('Are you sure you want to delete the Type " +
                     DataBinder.Eval(e.Row.DataItem, "TypeName") + " ')");
            }
        }

        protected void Grd_Catagory_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string _message;
            int _CatId = int.Parse(Grd_Catagory.DataKeys[e.RowIndex].Value.ToString());

            if (MyLibMang.DeleteCategoryById(_CatId, out _message))
            {
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "Book category deleted", 1);
                LoadCatToGrid();
                Lbl_msg.Text = _message;
            }
            else
            {
                Lbl_msg.Text = _message;
            }
            this.MPE_MessageBox.Show();

        }

        protected void Grd_Catagory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton l = (LinkButton)e.Row.FindControl("Link_Grd_Catagory");
                l.Attributes.Add("onclick", "javascript:return " +
                     "confirm('Are you sure you want to delete the Category " +
                     DataBinder.Eval(e.Row.DataItem, "CatogoryName") + " ')");
            }
           
        }

        protected void Grd_ViewRack_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            string _message;
            int _RackId = int.Parse(Grid_Rack.DataKeys[e.RowIndex].Value.ToString());

            if (MyLibMang.DeleteRackById(_RackId, out _message))
            {
                LoadRacks();
                Lbl_msg.Text = _message;
            }
            else
            {
                Lbl_msg.Text = _message;
            }
            this.MPE_MessageBox.Show();

        }

        protected void Grd_ViewRack_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton li = (LinkButton)e.Row.FindControl("LinkBtn_Rack");
                    li.Attributes.Add("onclick", "javascript:return " +
                         "confirm('Are you sure you want to delete the Rack " +
                         DataBinder.Eval(e.Row.DataItem, "RackName") + " ')");
                }
            }
        }

        protected void Btn_AutoNum_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("LlbraryHome.aspx");
        }

        protected void Btn_Fine_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("LlbraryHome.aspx");
        }

        protected void But_Type_Save_Cancel_Click(object sender, EventArgs e)
        {
            if (Btn_UpType.Visible == false)
            {
                Response.Redirect("LlbraryHome.aspx");
            }
            else
            {
                Txt_TypeId.Text = "";
                Text_Type.Text = "";
                Text_Des_Type.Text = "";
                Btn_UpType.Visible = false;
                But_Type_Save.Visible = true;
               
            }
        }

        protected void But_Cat_Cancel_Click(object sender, EventArgs e)
        {
            if(Btn_Upcat.Visible == false)
            {
                 Response.Redirect("LlbraryHome.aspx");
            }
            else
            {
                Lbl_CategoryError.Text = "";
                Txt_CatId.Text = "";
                Text_Category.Text = "";
                Text_Desc_Cat.Text = "";
                Btn_Upcat.Visible = false;
                But_AddCat.Visible = true;
            }
        }

        protected void Txt_RackCancel_Click(object sender, EventArgs e)
        {
            if(Btn_UpRack.Visible == false)
            {
                Response.Redirect("LlbraryHome.aspx");
            }
            else
            {
                Txt_RackNo.Text = "";
                Txt_RackId.Text = "";
                Txt_RackDesc.Text = "";
                Btn_UpRack.Visible = false;
                Btn_Rack.Visible = true;
            }
        }

        protected void Grd_ViewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int TypeId = -1;
            string sql;
            try
            {
                Grd_ViewType.Columns[0].Visible = true;
                TypeId = int.Parse(Grd_ViewType.SelectedRow.Cells[1].Text.ToString());
                Grd_ViewType.Columns[0].Visible = false;
            }
            catch
            {
                Lbl_msg.Text = "Please refresh the page and try again";
                this.MPE_MessageBox.Show();
            }
            if (TypeId != -1)
            {
                sql = "select tblbooktype.TypeName, tblbooktype.Desc from tblbooktype where tblbooktype.Id=" + TypeId;
                MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    But_Type_Save.Visible = false;
                    Btn_UpType.Visible = true;
                    Text_Type.Text = MyReader.GetValue(0).ToString();
                    Text_Des_Type.Text = MyReader.GetValue(1).ToString();
                    Txt_TypeId.Text = TypeId.ToString();
                }
            }
        }

        protected void Grd_Catagory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int CatId = -1;
            string sql;
            try
            {
                Grd_Catagory.Columns[0].Visible = true;
                //Grd_Catagory.Columns[1].Visible = true;
                CatId = int.Parse(Grd_Catagory.SelectedRow.Cells[1].Text.ToString());
               // Grd_Catagory.Columns[1].Visible = false;
                Grd_Catagory.Columns[0].Visible = false;
            }
            catch
            {

                Lbl_msg.Text = "Please refresh the page and try again";
                this.MPE_MessageBox.Show();
            }
            if (CatId != -1)
            {
                sql = "select tblbookcatogory.CatogoryName, tblbookcatogory.Disc from tblbookcatogory where tblbookcatogory.Id=" + CatId;
                MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    But_AddCat.Visible = false;
                    Btn_Upcat.Visible = true;
                    Text_Category.Text = MyReader.GetValue(0).ToString();
                    Text_Desc_Cat.Text = MyReader.GetValue(1).ToString();
                    Txt_CatId.Text = CatId.ToString();
                }
            }
        }

        protected void Grid_Rack_SelectedIndexChanged(object sender, EventArgs e)
        {
            int RackId = -1;
            string sql;
            try
            {
                Grid_Rack.Columns[0].Visible = true;
                RackId = int.Parse(Grid_Rack.SelectedRow.Cells[1].Text.ToString());
                Grid_Rack.Columns[0].Visible = false;
            }
            catch
            {

                Lbl_msg.Text = "Please refresh the page and try again";
                this.MPE_MessageBox.Show();
            }
            if (RackId != -1)
            {
              
                sql = "select tblbookrack.RackName,tblbookrack.Desc from tblbookrack where tblbookrack.Id=" + RackId;
                MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Btn_Rack.Visible = false;
                    Btn_UpRack.Visible = true;
                    Txt_RackNo.Text = MyReader.GetValue(0).ToString();
                    Txt_RackDesc.Text = MyReader.GetValue(1).ToString();
                    Txt_RackId.Text = RackId.ToString();
                }
            }
        }

        protected void Btn_Upcat_Click(object sender, EventArgs e)
        {
            Lbl_CategoryError.Text = "";
            if(Text_Category.Text.Trim() == "")
            {
                Lbl_CategoryError.Text="Category name cannot be empty";
                //Lbl_msg.Text = "Category name cannot be empty";
                //this.MPE_MessageBox.Show();
            }
            else if (Txt_CatId.Text.Trim() == "")
            {
                Lbl_CategoryError.Text="Please try again";
                //Lbl_msg.Text = "Please try again";
                //this.MPE_MessageBox.Show();
            }
            else
            {
                try
                {
                    MyLibMang.UpdateCategory(Text_Category.Text.Trim(), Text_Desc_Cat.Text.Trim(), int.Parse(Txt_CatId.Text.Trim()));
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "Book category:"+Text_Category.Text.Trim()+" Updated", 1);
                    LoadCatToGrid();
                    Txt_CatId.Text = "";
                    Text_Category.Text = "";
                    Text_Desc_Cat.Text = "";
                    But_AddCat.Visible = true;
                    Btn_Upcat.Visible = false;
                 
                    Lbl_msg.Text = "Category Updated";
                    this.MPE_MessageBox.Show();
                }
                catch
                {
                    Lbl_msg.Text = "Please try again";
                    this.MPE_MessageBox.Show();
                }
            }
        }

        protected void Btn_UpType_Click(object sender, EventArgs e)
        {
            Lbl_BookType_Error.Text = "";
            if (Text_Type.Text.Trim() == "")
            {
                Lbl_BookType_Error.Text = "Type name cannot be empty";
               // this.MPE_MessageBox.Show();
            }
            else if (Txt_TypeId.Text.Trim() == "")
            {
                Lbl_BookType_Error.Text = "Please try again";
                //this.MPE_MessageBox.Show();
            }
            else
            {
                try
                {
                    MyLibMang.UpdateType(Text_Type.Text.Trim(), Text_Des_Type.Text.Trim(), int.Parse(Txt_TypeId.Text.Trim()));
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "Book Type:" + Text_Type.Text.Trim() + " Updated", 1);
                    LoadBookTypes();
                    Txt_TypeId.Text = "";
                    Text_Type.Text = "";
                    Text_Des_Type.Text = "";
                    But_Type_Save.Visible = true;
                    Btn_UpType.Visible = false;
                    Lbl_msg.Text = "Type Updated";
                    this.MPE_MessageBox.Show();
                }
                catch
                {
                    Lbl_msg.Text = "Please try again";
                    this.MPE_MessageBox.Show();
                }
            }
        }

        protected void Btn_UpRack_Click(object sender, EventArgs e)
        {
            lbl_RackError.Text = "";
            if (Txt_RackNo.Text.Trim() == "")
            {
                Lbl_msg.Text = "Rack Number cannot be empty";
                this.MPE_MessageBox.Show();
            }
            else if (Txt_RackId.Text.Trim() == "")
            {
                Lbl_msg.Text = "Please try again";
                this.MPE_MessageBox.Show();
            }
            else
            {
                try
                {
                    MyLibMang.UpdateRack(Txt_RackNo.Text.Trim(), Txt_RackDesc.Text.Trim(), int.Parse(Txt_RackId.Text.Trim()));
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "Book Rack:" + Txt_RackNo.Text.Trim() + " Updated", 1);
                    LoadRacks();
                    Txt_RackId.Text = "";
                    Txt_RackNo.Text = "";
                    Txt_RackDesc.Text = "";
                    Btn_Rack.Visible = true;
                    Btn_UpRack.Visible = false;
                    Lbl_msg.Text = "Rack Updated";
                    this.MPE_MessageBox.Show();
                }
                catch
                {
                    Lbl_msg.Text = "Please try again";
                    this.MPE_MessageBox.Show();
                }
            }
        }

        protected void Btn_Limit_Click(object sender, EventArgs e)
        {
            string message;
            if (validLimit(out message))
            {
                try
                {
                    MyLibMang.AddBookLimit(Txt_IssueLimit.Text.Trim(), Txt_IssueLimit_st.Text);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "Book limit Staff,Student:" + Txt_IssueLimit.Text.Trim() + "," + Txt_IssueLimit_st.Text.Trim() + " added", 1);
                    Lbl_msg.Text = "Book limit saved";
                    this.MPE_MessageBox.Show();
                }
                catch
                {
                    Lbl_msg.Text = "Please try again";
                    this.MPE_MessageBox.Show();
                }
            }
            else
            {
                Lbl_msg.Text = message;
                this.MPE_MessageBox.Show();
            }
        }

        private bool validLimit(out string message)
        {
            message = "";
            bool _valid = true;
            if (Txt_IssueLimit.Text.Trim() == "")
            {
                _valid = false;
                message = "Please Enter Student Book Limit";
            }
            if (Txt_IssueLimit_st.Text.Trim()=="")
            {
                _valid = false;
                message = "Please Enter Staff Book Limit";
            }
            return _valid;
        }

        protected void Btn_LimitCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("LlbraryHome.aspx");
        }

        protected void Rdo_Barcodeactive_SelectedIndexChanged(object sender, EventArgs e)
        {
            Lbl_BarcodeConfigErr.Text = "";
            int value = 0;
            int.TryParse(Rdo_Barcodeactive.SelectedValue.ToString(), out value);
            if (value == 1)
            {
               
                Rdo_UniqueBarcode.Enabled = true;
                //sai added
                Txt_barcode_prefix.Enabled =true;
                rdb_barcode_text.Enabled = true;
                rdb_barcode_type.Enabled =true;
                //
            }
            else
            {
                Rdo_UniqueBarcode.Enabled = false;
                //sai added
                Txt_barcode_prefix.Enabled = false;
                rdb_barcode_text.Enabled = false;
                rdb_barcode_type.Enabled = false;

                //
            }
            //sai put comments
            //if (CheckBooksIsalreadyEnterd())
            //{
            //    Rdo_UniqueBarcode.Enabled = false;
            //}

        }
        //sai put comments
        //protected void Rdo_UniqueBarcode_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (CheckBooksIsalreadyEnterd())
        //    {
        //        Lbl_BarcodeConfigErr.Text = "Bar-code is already enterd on some books.If you change the bar-code configuretion, please update barcode of  books before using";
        //    }
        //    else
        //    {
        //        Lbl_BarcodeConfigErr.Text = "";
        //    }
        //}

        private bool CheckBooksIsalreadyEnterd()
        {
            string sql1="select tblbooks.id from tblbooks inner join tblbookmaster on tblbookmaster.id= tblbooks.BookId where tblbooks.Barcode<>'0'";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql1);
            if (MyReader.HasRows)
            {
                return true;
            }
            return false;
      
        }
        
        protected void Btn_Save_Click(object sender, EventArgs e)
        {

            int     IsActive        = 0;
            int     UniqueNumber    = 0;
            int need_barcode_name = 0;
            int Barcode_type = 0;
            string  sql             = "";

            Lbl_BarcodeConfigErr.Text = "";

            int.TryParse(Rdo_Barcodeactive.SelectedValue.ToString(), out IsActive);
            int.TryParse(Rdo_UniqueBarcode.SelectedValue.ToString(), out UniqueNumber);

            if (IsActive == 1)
            {
                sql = "update tbllibconfig set tbllibconfig.Value1=1 where tbllibconfig.FieldName='ActiveBarcode'";
                MyReader=MyLibMang.m_MysqlDb.ExecuteQuery(sql);
                UpdateUniqueNumber(UniqueNumber);

                
            }
            else if (IsActive == 2)
            {
                sql = "update tbllibconfig set tbllibconfig.Value1=0 where tbllibconfig.FieldName='ActiveBarcode'";
                MyReader=MyLibMang.m_MysqlDb.ExecuteQuery(sql);
                
            }
            //sai added code for barcode prefix and text needed or not
            int.TryParse(rdb_barcode_text.SelectedValue.ToString(), out need_barcode_name);
            int.TryParse(rdb_barcode_type.SelectedValue.ToString(), out Barcode_type);
           
            sql = "update tbllibconfig set tbllibconfig.Value1='" + Txt_barcode_prefix.Text.ToString() + "' where tbllibconfig.FieldName='Barcodeprefix'";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            if (need_barcode_name == 0)
            {
                sql = "update tbllibconfig set tbllibconfig.Value1=0 where tbllibconfig.FieldName='NeedBarcodeText'";
                MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            }
            else if (need_barcode_name == 1)
            {
                sql = "update tbllibconfig set tbllibconfig.Value1=1 where tbllibconfig.FieldName='NeedBarcodeText'";
                MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            }
            if (Barcode_type == 1)
            {
                sql = "update tbllibconfig set tbllibconfig.Value1=1 where tbllibconfig.FieldName='BarcodeAutomatic'";
                MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            }
            else if (Barcode_type == 0)
            {
                sql = "update tbllibconfig set tbllibconfig.Value1=0 where tbllibconfig.FieldName='BarcodeAutomatic'";
                MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            }
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "Barcode configurations updated", 1);
            Lbl_BarcodeConfigErr.Text = "Barcode configuration updated ";   
            
        }

        private void UpdateUniqueNumber(int UniqueNumber)
        {
            int ActiveVal = 0;
            string config = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.ConfigName='Unique Barode'";
            MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(config);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out ActiveVal);
            }
            string sql="";
            if (ActiveVal != UniqueNumber)
            {
                if (UniqueNumber == 1)
                {
                    sql = "update tbllibconfig set tbllibconfig.Value1=1 where tbllibconfig.FieldName='UniqueBarcode'";
                }
                else if (UniqueNumber == 2)
                {
                    sql = "update tbllibconfig set tbllibconfig.Value1=2 where tbllibconfig.FieldName='UniqueBarcode'";
                }
                MyReader = MyLibMang.m_MysqlDb.ExecuteQuery(sql);
            }
        }

      
    }
}
