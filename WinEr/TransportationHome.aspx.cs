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

namespace WinEr
{
    public partial class TransportationHome : System.Web.UI.Page
    {
        private TransportationClass MyTransMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet = null; 

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
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
                    LoadInitialStage();
                    Clear();
                    FillItemGrid("", 0);
                    LoadCategoryToDropDown(0);
                }
            }
        }

        #region VEHICLE LIST AND LOAD FUNCTIONS

        private void LoadCategoryToDropDown(int Type)
        {
            if (Type == 0)
            {
                Drp_Categories.Items.Clear();
            }
            else
            {
                Drp_VehicleType.Items.Clear();
            }

            string sql = "SELECT tbl_tr_vehicletype.VehicleType, tbl_tr_vehicletype.Id   from tbl_tr_vehicletype ORDER by tbl_tr_vehicletype.Id asc ";
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
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
                        Drp_VehicleType.Items.Add(li);
                    }

                }
                //Drp_Categories.SelectedIndex = _intex;
            }
            else
            {
                ListItem li = new ListItem("No Category Found", "-1");
                Drp_Categories.Items.Add(li);
                Drp_VehicleType.Items.Add(li);
            }
        }

        private void FillItemGrid(string _VehicleName, int _VehicleType)
        {
            Lbl_NoteErr.Text = "";
            string Subsql = "", Sql1 = "";
            if (_VehicleName == "")
            {
                if (_VehicleType == 0)// (int.Parse(Drp_Categories.SelectedValue) == 0)
                {
                    Sql1 = "";
                }
                else
                {
                    Sql1 = " where tbl_tr_vehicle.TypeId=" + _VehicleType;
                }

                Subsql = Sql1;
            }
            else
            {
                Subsql = " where tbl_tr_vehicle.VehicleNo='" + _VehicleName + "'";
            }

            Grd_Vehicles.DataSource = null;
            Grd_Vehicles.DataBind();
            ViewState["ItemDataSet"] = null;

            Lbl_msg.Text = "";
            string sql = "SELECT tbl_tr_vehicle.Id, tbl_tr_vehicle.VehicleNo, tbl_tr_vehicle.RegNo,tbl_tr_vehicletype.VehicleType,tbl_tr_vehicle.Capacity , tbl_tr_vehicle.Milage from tbl_tr_vehicle INNER JOIN tbl_tr_vehicletype on tbl_tr_vehicletype.Id= tbl_tr_vehicle.TypeId" + Subsql;
            MyDataSet = GetDetailedDataSet(MyTransMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql));
            
            ViewState["VehicleDataSet"] = MyDataSet;
            if (MyDataSet.Tables[0].Rows.Count > 0)
            {
                Grd_Vehicles.Columns[0].Visible = true;
                Grd_Vehicles.DataSource = MyDataSet;
                Grd_Vehicles.DataBind();
                Grd_Vehicles.Columns[0].Visible = false;
            }
            else
            {
                Lbl_NoteErr.Text = "No vehicles found.";
                
            }
        }

        private DataSet GetDetailedDataSet(DataSet TripdataSet)
        {
            if (TripdataSet != null && TripdataSet.Tables[0] != null && TripdataSet.Tables[0].Rows.Count != 0)
            {
                int id = 0,trips=0;
                double dist = 0;
                string tripnames="NoTrips";
                DataTable dt;
                DataRow dr;
                dt = TripdataSet.Tables[0];
                dt.Columns.Add("Trips");
                dt.Columns.Add("Distance");
                dt.Columns.Add("TripNames");

                foreach (DataRow dro in TripdataSet.Tables[0].Rows)
                {
                    id = int.Parse(dro[0].ToString());
                    MyTransMang.GetTripInfo(id, out trips, out dist, out tripnames);
                    dro["Trips"] = trips.ToString();
                    dro["Distance"] = dist.ToString();
                    dro["TripNames"] = tripnames.ToString();
                }

            }
            return TripdataSet;
        }

        private void Clear()
        {
            TxtSearch.Text = "";
            txt_Mileage.Text = "";
            txt_VehicleNo.Text = "";
            txt_SeatNo.Text = "";
            txt_RegistrationNo.Text = "";
            Lbl_NoteErr.Text = "";
        }

        private void LoadInitialStage()
        {
            Pnl_SearchArea.Visible = true;
            Pnl_VehicleList.Visible = true;
            Pnl_AddVehicle.Visible = false;

            Lnk_AddNewVehicle.Visible = true;
            Img_Add.Visible = true;
            LoadTopAreaDetails();
        }

        private void LoadTopAreaDetails()
        {
            lbl_VehicleNo.Text = GetTotalvehicles().ToString();
            lbl_VehicleTypeNo.Text = GetTotalVehicleTypeNo().ToString();
        }

        private object GetTotalVehicleTypeNo()
        {
            int count = 0;
            string sql = "SELECT COUNT(Id) FROM tbl_tr_vehicletype";
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out count);
            }
            return count;
        }

        private int GetTotalvehicles()
        {
            int count = 0;
            string sql = "SELECT COUNT(Id) FROM tbl_tr_vehicle";
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out count);
            }
            return count;
        }

        protected void Img_Search_Click(object sender, ImageClickEventArgs e)
        {
            Clear();
            LoadInitialStage();
            int _CategoryId = int.Parse(Drp_Categories.SelectedValue);
            FillItemGrid("", _CategoryId);
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

        protected void Grd_Vehicles_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Grd_Items.PageIndex = 0;
            //FillItemGrid("", int.Parse(Drp_Categories.SelectedValue));
            if (ViewState["VehicleDataSet"] != null)
            {
                DataSet _sortlDS = (DataSet)ViewState["VehicleDataSet"];

                if (_sortlDS.Tables[0].Rows.Count > 0)
                {
                    DataTable dtData = _sortlDS.Tables[0];

                    DataView dataView = new DataView(dtData);

                    dataView.Sort = e.SortExpression + " " + GetSortDirection1(e.SortExpression);
                    Grd_Vehicles.Columns[0].Visible = true;
                    Grd_Vehicles.DataSource = dataView;
                    Grd_Vehicles.DataBind();
                    Grd_Vehicles.Columns[0].Visible = false;
                }
            }
        }

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

        protected void Grd_Vehicles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Vehicles.PageIndex = e.NewPageIndex;
            // FillItemGrid("", int.Parse(Drp_Categories.SelectedValue));
            if (ViewState["VehicleDataSet"] != null)
            {
                DataSet _pageDS = (DataSet)ViewState["VehicleDataSet"];
                if (_pageDS.Tables[0].Rows.Count > 0)
                {
                    Grd_Vehicles.Columns[0].Visible = true;
                    DataTable dtpageData = _pageDS.Tables[0];

                    DataView dataView = new DataView(dtpageData);

                    if (Session["SortDirection1"] != null && Session["SortExpression1"] != null)
                    {
                        dataView.Sort = (string)Session["SortExpression1"] + " " + (string)Session["SortDirection1"];
                    }

                    Grd_Vehicles.DataSource = dataView;
                    Grd_Vehicles.DataBind();
                    Grd_Vehicles.Columns[0].Visible = false;

                }
                else
                {
                    Lbl_msg.Text = "No item present for selected category";
                }
            }



        }

        protected void Grd_Vehicles_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["VehicleId"] = int.Parse(Grd_Vehicles.SelectedRow.Cells[0].Text.ToString());
            Response.Redirect("VehicleDetails.aspx");            
        }

        #endregion

        #region ADD NEW VEHICLE FUNCTIONS

        protected void Btn_SaveVehicle_Click(object sender, EventArgs e)
        {
            string _VehicleRegNo = "", _VehicleNo = "";
            double _mileage = 0;
            int _VehicleType = 0, _NoOfSeats = 0;

            _VehicleRegNo = txt_RegistrationNo.Text.Trim();
            _VehicleNo = txt_VehicleNo.Text.Trim();
            _mileage = double.Parse(txt_Mileage.Text.Trim());
            _VehicleType = int.Parse(Drp_VehicleType.SelectedValue);
            _NoOfSeats = int.Parse(txt_SeatNo.Text.Trim());

            if (!MyTransMang.VehicleRegistrationNoExists(_VehicleRegNo))
            {
                if (!MyTransMang.VehicleNoExists(_VehicleNo))
                {
                    SaveVehicleDetailsToTable(_VehicleNo,_VehicleType,_NoOfSeats,_mileage,_VehicleRegNo);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "transportation vehicle", "Vehicle:" + Drp_VehicleType.SelectedItem.Text + " added,vehicle No:" + _VehicleNo + "", 1);
                    if (Chk_AddMore.Checked == true)
                    {
                        Pnl_SearchArea.Visible = true;
                        Pnl_VehicleList.Visible = false;
                        Pnl_AddVehicle.Visible = true;

                        Lnk_AddNewVehicle.Visible = false;
                        Img_Add.Visible = false;
                        Clear();
                        LoadCategoryToDropDown(1);
                        LoadTopAreaDetails();
                        
                        Lbl_msg.Text = "Vehicle Details Saved Successfully";
                        MPE_MessageBox.Show();
                    }
                    else
                    {
                        Clear();
                        LoadInitialStage();
                        FillItemGrid("", 0);
                        LoadCategoryToDropDown(0);
                    }
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

        private void SaveVehicleDetailsToTable(string _VehicleNo, int _VehicleType, int _NoOfSeats, double _mileage, string _VehicleRegNo)
        {
            try
            {
                string sql = "INSERT into tbl_tr_vehicle(VehicleNo,TypeId,Capacity,Milage,RegNo) VALUES ('"+_VehicleNo+"',"+_VehicleType+","+_NoOfSeats+","+_mileage+",'"+_VehicleRegNo+"')";
                MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            }
            catch (Exception e)
            {
                Lbl_msg.Text = e.ToString();
                MPE_MessageBox.Show();
            }
        }
                
        protected void Btn_VehicleCancel_Click(object sender, EventArgs e)
        {
            Clear();
            LoadInitialStage();
            FillItemGrid("", 0);
            LoadCategoryToDropDown(0);            
        }

        protected void LnkBtn_CreateType_Click(object sender, EventArgs e)
        {
            Lbl_MsgCreateCategory.Text = "";
            txt_new_category.Text = "";
            MPE_MessageBox_AddNewCategory.Show();
        }

        protected void Lnk_AddNewVehicle_Click(object sender, EventArgs e)
        {
            Pnl_SearchArea.Visible = true;
            Pnl_VehicleList.Visible = false;
            Pnl_AddVehicle.Visible = true;

            Lnk_AddNewVehicle.Visible = false;
            Img_Add.Visible = false;
            LoadCategoryToDropDown(1);
            LoadTopAreaDetails();
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

                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "transportation vehicle category", "New Vehicle category:" + _CategoryName + " added", 1);
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
            for (int i = 0; i < Drp_VehicleType.Items.Count; i++)
            {
                if (Drp_VehicleType.Items[i].Text == _CategoryName)
                {
                    Drp_VehicleType.SelectedIndex = i;
                    break;
                }
            }
        }

        #endregion        

    }
}
