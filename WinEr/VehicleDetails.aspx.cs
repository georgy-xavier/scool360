using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;

namespace WinEr
{
    public partial class VehicleDetails : System.Web.UI.Page
    {
        private TransportationClass MyTransMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            if (Session["VehicleId"] == null)
            {
                Response.Redirect("TransportationHome.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyTransMang = MyUser.GetTransObj();
            if (MyTransMang == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }
            
            else
            {
                if (!IsPostBack)
                {
                    string _MenuStr;
                    _MenuStr = MyTransMang.GetSubTransMangMenuString(MyUser.UserRoleId);
                    this.SubTransMenu.InnerHtml = _MenuStr;

                    LoadInitialStage();
                    LoadTopAreaDetailsAndVehicleDetailsToPage();
                }

            }
        }


        #region INITIAL STAGE AND LOADING FUNCTIONS

        private void LoadInitialStage()
        {
            Pnl_EditItem.Visible = true;
            lbl_EditTotalSeats.Visible = true;
            lbl_EditVehicleNo.Visible = true;
            lbl_Category.Visible = true;
            lbl_RegisterNo.Visible = true;
            lbl_mileage.Visible = true;

            txt_EditRegNo.Visible = false;
            txt_EditVehicleNo.Visible = false;
            txt_EditSeats.Visible = false;
            txt_EditMileage.Visible = false;
            Drp_EditCategory.Visible = false;

            Btn_UpdateVehicle.Visible = false;
            Btn_EditVehicle.Visible = true;
            Lnk_EditnewCategory.Visible = false;
        }

        private void LoadTopAreaDetailsAndVehicleDetailsToPage()
        {
            string sql = "SELECT tbl_tr_vehicle.RegNo, tbl_tr_vehicle.VehicleNo, tbl_tr_vehicletype.VehicleType, tbl_tr_vehicle.Capacity, tbl_tr_vehicle.Milage from tbl_tr_vehicle inner join tbl_tr_vehicletype on tbl_tr_vehicle.TypeId=tbl_tr_vehicletype.Id where tbl_tr_vehicle.Id=" + int.Parse(Session["VehicleId"].ToString());
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                lbl_RegNo.Text = MyReader.GetValue(0).ToString();
                lbl_VehicleNo.Text = MyReader.GetValue(1).ToString();
                lbl_VehicleType.Text = MyReader.GetValue(2).ToString();
                lbl_TotalSeats.Text = (int.Parse(MyReader.GetValue(3).ToString())).ToString();

                lbl_RegisterNo.Text = " :       " + MyReader.GetValue(0).ToString();
                lbl_EditVehicleNo.Text = " :       " + MyReader.GetValue(1).ToString();
                lbl_Category.Text = " :       " + MyReader.GetValue(2).ToString();
                lbl_EditTotalSeats.Text = " :       " + (int.Parse(MyReader.GetValue(3).ToString())).ToString();
                lbl_mileage.Text = " :       " + (double.Parse(MyReader.GetValue(4).ToString())).ToString();

                txt_EditRegNo.Text = MyReader.GetValue(0).ToString();
                txt_EditVehicleNo.Text = MyReader.GetValue(1).ToString();
                txt_EditSeats.Text = (int.Parse(MyReader.GetValue(3).ToString())).ToString();
                txt_EditMileage.Text = (double.Parse(MyReader.GetValue(4).ToString())).ToString();

            }
        }

        protected void Btn_EditCancel_Click(object sender, EventArgs e)
        {
            LoadInitialStage();
            LoadTopAreaDetailsAndVehicleDetailsToPage();
        }

        #endregion


        #region EDIT AND UPDATE FUNCTIONS

        protected void Btn_EditVehicle_Click(object sender, EventArgs e)
        {
            Pnl_EditItem.Visible = true;
            lbl_EditTotalSeats.Visible = false;
            lbl_EditVehicleNo.Visible = false;
            lbl_Category.Visible = false;
            lbl_RegisterNo.Visible = false;
            lbl_mileage.Visible = false;

            txt_EditRegNo.Visible = true;
            txt_EditVehicleNo.Visible = true;
            txt_EditSeats.Visible = true;
            txt_EditMileage.Visible = true;
            Drp_EditCategory.Visible = true;

            Btn_UpdateVehicle.Visible = true;
            Btn_EditVehicle.Visible = false;
            Lnk_EditnewCategory.Visible = true;

            int _TypeId = 0;
            string sql = "SELECT tbl_tr_vehicle.TypeId from tbl_tr_vehicle where tbl_tr_vehicle.Id=" + int.Parse(Session["VehicleId"].ToString());
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out _TypeId);
            }
            LoadVehicleTypeToDropDown(_TypeId);
        }

        private void LoadVehicleTypeToDropDown(int _TypeId)
        {
            Drp_EditCategory.Items.Clear();

            string sql = "SELECT tbl_tr_vehicletype.VehicleType, tbl_tr_vehicletype.Id   from tbl_tr_vehicletype ORDER by tbl_tr_vehicletype.Id asc ";
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_EditCategory.Items.Add(li);
                }
                Drp_EditCategory.SelectedValue = _TypeId.ToString();
            }
            else
            {
                ListItem li = new ListItem("No Category Found", "-1");
                Drp_EditCategory.Items.Add(li);
            }
        }

        protected void Btn_UpdateVehicle_Click(object sender, EventArgs e)
        {
            string sql = "", _RegNo = "", _VehicleNo = "";
            double _Mileage = 0, _ExtraDist = 0;
            int _Seat = 0, _VehicleType = 0;
            _RegNo = txt_EditRegNo.Text.Trim();
            _VehicleNo = txt_EditVehicleNo.Text.Trim();
            _Mileage = double.Parse(txt_EditMileage.Text.Trim());
            _Seat = int.Parse(txt_EditSeats.Text.Trim());
            _VehicleType = int.Parse(Drp_EditCategory.SelectedValue);

            if (!VehicleRegistrationNoExists(_RegNo))
            {
                if (!VehicleNoExists(_VehicleNo))
                {
                    sql = "UPDATE tbl_tr_vehicle SET tbl_tr_vehicle.RegNo='" + _RegNo + "', tbl_tr_vehicle.VehicleNo='" + _VehicleNo + "', tbl_tr_vehicle.TypeId=" + _VehicleType + ", tbl_tr_vehicle.Milage=" + _Mileage + ", tbl_tr_vehicle.Capacity=" + _Seat + " where tbl_tr_vehicle.Id=" + int.Parse(Session["VehicleId"].ToString());
                    MyTransMang.m_MysqlDb.ExecuteQuery(sql);
                    sql = "Update tbl_tr_trips set capacity=" + _Seat + " where VehicleId="+ int.Parse(Session["VehicleId"].ToString())+"";
                    MyTransMang.m_MysqlDb.ExecuteQuery(sql);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Edit transportation vehicle", "Updated vehicle details,vehicle No: " + _VehicleNo + "", 1);
                    Lbl_msg.Text = "Vehicle Details Updated!";
                    MPE_MessageBox.Show();
                    LoadInitialStage();
                    LoadTopAreaDetailsAndVehicleDetailsToPage();
                }
                else
                {
                    Lbl_msg.Text = "Vehicle Number Already Exists!";
                    MPE_MessageBox.Show();
                }
            }
            else
            {
                Lbl_msg.Text = "Vehicle Already Exists!";
                MPE_MessageBox.Show();
            }
        }

        private bool VehicleNoExists(string _VehicleNo)
        {
            bool _Exists = false;
            string sql = "SELECT tbl_tr_vehicle.VehicleNo from tbl_tr_vehicle where tbl_tr_vehicle.VehicleNo='" + _VehicleNo + "' and tbl_tr_vehicle.Id!=" + int.Parse(Session["VehicleId"].ToString());
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _Exists = true;
            }
            return _Exists;
        }

        private bool VehicleRegistrationNoExists(string _RegNo)
        {
            bool _Exists = false;
            string sql = "SELECT tbl_tr_vehicle.RegNo from tbl_tr_vehicle where tbl_tr_vehicle.RegNo='" + _RegNo + "' and tbl_tr_vehicle.Id!=" + int.Parse(Session["VehicleId"].ToString());
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _Exists = true;
            }
            return _Exists;
        }

        protected void Lnk_EditnewCategory_Click(object sender, EventArgs e)
        {
            MPE_MessageBox_AddNewCategory.Show();
        }

        protected void Btn_Add_new_cat_Click(object sender, EventArgs e)
        {
            Lbl_MsgCreateCategory.Text = "";
            string _CategoryName = "", _ErrorMsg = "";
            _CategoryName = txt_new_category.Text.Trim().ToUpper();

            if (_CategoryName != "")
            {
                if (SaveNewCategory(_CategoryName, out _ErrorMsg))
                {
                    LoadVehicleTypeToDropDown(1);
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
                Lbl_MsgCreateCategory.Text = "Enter Category Name";
                txt_new_category.Text = "";
            }
        }

        private bool SaveNewCategory(string _CategoryName, out string _ErrorMsg)
        {
            bool _Valid = false;
            _ErrorMsg = "";
            try
            {
                _CategoryName = _CategoryName.ToUpper();

                string sql = "SELECT tbl_tr_vehicletype.VehicleType from tbl_tr_vehicletype where tbl_tr_vehicletype.VehicleType='" + _CategoryName + "'";
                MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    _ErrorMsg = "This Type Name Is Allready Exist";
                }
                else
                {
                    sql = "INSERT into tbl_tr_vehicletype(VehicleType) VALUES ('" + _CategoryName + "')";
                    MyTransMang.m_MysqlDb.ExecuteQuery(sql);
                    //DBLogClass dblog = new DBLogClass(m_MysqlDb);
                    //dblog.LogToDb(m_UserName, "Create Category", "A New Category " + _CategoryName + "  is created.", 1);
                    _Valid = true;

                }

            }
            catch (Exception e)
            {
                _Valid = false;
                _ErrorMsg = "Please Try Again";
            }

            return _Valid;

        }

        private void loadDrp_CategorySelectedtextAsTheCreatedOne(string _CategoryName)
        {
            for (int i = 0; i < Drp_EditCategory.Items.Count; i++)
            {
                if (Drp_EditCategory.Items[i].Text == _CategoryName)
                {
                    Drp_EditCategory.SelectedIndex = i;
                    break;
                }
            }
        }

        #endregion


        #region DELETE FUNCTIONS

        protected void Btn_Delete_Click(object sender, EventArgs e)
        {
            MPE_DeleteConfirm.Show();
        }

        protected void Btn_DeleteYes_Click(object sender, EventArgs e)
        {
           
            string vehiclesql = "select tbl_tr_trips.VehicleId from tbl_tr_trips where tbl_tr_trips.VehicleId=" + int.Parse(Session["VehicleId"].ToString());
            OdbcDataReader vehicleidreader = MyTransMang.m_MysqlDb.ExecuteQuery(vehiclesql);
            if (vehicleidreader.HasRows)
            {
                Lbl_msg.Text = "Cannot delete,this vehicle is assigned to some trips!";
                MPE_MessageBox.Show();
            }
            else
            {
                string sql = "DELETE from tbl_tr_vehicle where tbl_tr_vehicle.Id=" + int.Parse(Session["VehicleId"].ToString());
                MyTransMang.m_MysqlDb.ExecuteQuery(sql);
                sql = " update tbl_tr_trips set VehicleId = 0 where VehicleId = " + int.Parse(Session["VehicleId"].ToString());
                MyTransMang.m_MysqlDb.ExecuteQuery(sql);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "delete transportation vehicle", "vehicle deleted", 1);
                Lbl_msg.Text = "Vehicle Details Deleted!";
                MPE_MessageBox.Show();

                Response.Redirect("TransportationHome.aspx");
            }
        }

       

        #endregion

    }
}
