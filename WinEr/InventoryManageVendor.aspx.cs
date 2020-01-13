using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace WinEr
{
    public partial class InventoryManageVendor : System.Web.UI.Page
    {

        private ConfigManager MyConfigMang;
        private KnowinUser MyUser;
        private WinBase.Inventory Myinventory;


        #region events

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
                else if (!MyUser.HaveActionRignt(868))
                {
                    Response.Redirect("RoleErr.htm");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        LoadVendor();
                    }
                }
            }

            protected void Lnk_AddNewVendor_Click(object sender, EventArgs e)
            {
                Clear();
                Hdn_VId.Value = "0";
                Btn_Delete.Visible = false;
                Lbl_Head.Text = "New Vendor";
                Lbl_VendorErr.Text = "";
                MPE_AddVendor.Show();

            }

            protected void Grd_Vendor_SelectedIndexChanged(object sender, EventArgs e)
            {
                Clear();
                Lbl_Head.Text = "Manage Vendor";
                Lbl_VendorErr.Text = "";
                Hdn_VId.Value = Grd_Vendor.SelectedRow.Cells[0].Text;
                Txt_VendorName.Text = Grd_Vendor.SelectedRow.Cells[1].Text.Replace("&nbsp;", "");
                Txt_City.Text = Grd_Vendor.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");
                Txt_Address.Text = Grd_Vendor.SelectedRow.Cells[3].Text.Replace("&nbsp;", "");
                Txt_Email.Text = Grd_Vendor.SelectedRow.Cells[4].Text.Replace("&nbsp;", "");
                Txt_MobileNumber.Text = Grd_Vendor.SelectedRow.Cells[5].Text.Replace("&nbsp;", "");
                Hdn_VName.Value = Grd_Vendor.SelectedRow.Cells[1].Text.Replace("&nbsp;", "");
                Btn_Delete.Visible = true;
                MPE_AddVendor.Show();
            }

            protected void Btn_VendorSave_Click(object sender, EventArgs e)
            {
                int Id = int.Parse(Hdn_VId.Value);
                string vendorname = Txt_VendorName.Text;
                string city = Txt_City.Text;
                string address = Txt_Address.Text;
                string email = Txt_Email.Text;
                string mobile = Txt_MobileNumber.Text;
                if (int.Parse(Hdn_VId.Value) == 0)
                {
                    Myinventory.SaveVendordetails(vendorname, city, address, email, mobile);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Inventory :  New vendor", "Vendor " + vendorname + " is added", 1);
                    Lbl_VendorErr.Text = "New vendor added successfully";
                    Clear();

                }
                else
                {
                    string oldvendorname = Hdn_VName.Value;
                    Myinventory.UpdateVendorDetails(Id, vendorname, city, address, email, mobile);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Inventory :  Edit vendor Details", "Details of  " + oldvendorname + " is updated", 1);
                    Lbl_VendorErr.Text = "Vendor details updated";
                }
                LoadVendor();
                MPE_AddVendor.Show();
            }

            protected void Grd_Vendor_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                Grd_Vendor.PageIndex = e.NewPageIndex;
                LoadVendor();
            }

            protected void Btn_Delete_Click(object sender, EventArgs e)
            {
                Myinventory.deleteVendor(int.Parse(Hdn_VId.Value));
                Lbl_VendorErr.Text = "Vendor deleted";
                Clear();
                MPE_AddVendor.Show();
                LoadVendor();
            }

        #endregion


        #region Methods

            private void LoadVendor()
        {
            Lbl_Err.Text = "";
            DataSet Vendor_Ds = new DataSet();
            Vendor_Ds = Myinventory.GetAllVendordetails();
            if (Vendor_Ds != null && Vendor_Ds.Tables[0].Rows.Count > 0)
            {
                Grd_Vendor.Columns[0].Visible = true;
                Grd_Vendor.DataSource = Vendor_Ds;
                Grd_Vendor.DataBind();
                Grd_Vendor.Columns[0].Visible = false;
            }
            else
            {
                Grd_Vendor.DataSource = null;
                Grd_Vendor.DataBind();
                Lbl_Err.Text = "No vendor exist";
            }
        }

            private void Clear()
            {
                Txt_Address.Text = "";
                Txt_City.Text = "";
                Txt_Email.Text = "";
                Txt_MobileNumber.Text = "";
                Txt_VendorName.Text = "";
                //Hdn_VId.Value = "";
            }

        #endregion


        
    }
}
