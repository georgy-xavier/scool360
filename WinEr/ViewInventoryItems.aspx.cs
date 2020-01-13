using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace WinEr
{
    public partial class ViewInventoryItems : System.Web.UI.Page
    {
        private ClassOrganiser MyClassMang;
        private KnowinUser MyUser;
        private WinBase.Inventory Myinventory;      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (Session["ClassId"] == null)
            {
                Response.Redirect("LoadClassDetails.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyClassMang = MyUser.GetClassObj();
            Myinventory = MyUser.GetInventoryObj();
            if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(848))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {


                if (!IsPostBack)
                {
                    LoadItemsToGrid();
                }
            }
        }
        protected void Grd_Items_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Items.PageIndex = e.NewPageIndex;
            LoadItemsToGrid();
        }

        private void LoadItemsToGrid()
        {
            string ClassName = "";
            Lbl_Caption.Text = "";
            Lbl_err.Text = "";
            int classid = int.Parse(Session["ClassId"].ToString());
            DataSet Inventory_Ds = new DataSet();
            Inventory_Ds = Myinventory.GetItemDetails(classid, out ClassName);
             if (Inventory_Ds != null && Inventory_Ds.Tables[0].Rows.Count > 0)
             {
                 Grd_Items.Columns[0].Visible = true;
                 Lbl_Caption.Text = "List of the items contained in " + ClassName + "";
                 Grd_Items.DataSource = Inventory_Ds;
                 Grd_Items.DataBind();
                 Grd_Items.Columns[0].Visible = false;
             }
             else
             {
                 Lbl_Caption.Text = "";
                 Grd_Items.DataSource = null;
                 Grd_Items.DataBind();
                 Lbl_err.Text = "No Item Found";                 
                 Pnl_ViewInventoryItems.Visible = false;
             }
        }

      
    }
}
