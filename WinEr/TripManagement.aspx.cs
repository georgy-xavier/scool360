using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;

namespace WinEr
{
    public partial class TripManagement : System.Web.UI.Page
    {
        private TransportationClass MyTransMang;
        private KnowinUser MyUser;
        private SMSManager MysmsMang;
        private OdbcDataReader MyReader = null;
        private OdbcDataReader MyReader1 = null;
        private OdbcDataReader MyReader2 = null;
        private DataSet MyDataSet = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }

            MyUser = (KnowinUser)Session["UserObj"];
            MyTransMang = MyUser.GetTransObj();
            MysmsMang = MyUser.GetSMSMngObj();
            if (MyTransMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(201))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    MysmsMang.InitClass();
                    LoadInitialStage();
                    Clear();

                }

            }
        }


        #region INITIAL AND TRIP DETAILS DISPLAY FUNCTIONS


        private void Clear()
        {

            lbl_EndTime.Text = "";
            lbl_EditEndTime.Text = "";
            //Btn_SaveVehicle.Enabled = false;
            Btn_UpdateTrip.Enabled = false;
            Chk_TripDirection.Checked = false;
            Grd_RouteDestinations.DataSource = null;
            Grd_RouteDestinations.DataBind();
            Grd_EditDestinations.DataSource = null;
            Grd_EditDestinations.DataBind();
        }

        private void LoadInitialStage()
        {
            Pnl_AddButtonArea.Visible = true;
            Pnl_TripList.Visible = true;
            Pnl_AddNewTrip.Visible = false;
            Pnl_EditTrip.Visible = false;

            LoadTripDetailsGrid();
        }

        private void LoadTripDetailsGrid()
        {
            Lbl_tripnote.Text = "";
            Grd_Trips.DataSource = null;
            Grd_Trips.DataBind();
            ViewState["ItemDataSet"] = null;
            string sql = "SELECT tbl_tr_trips.Id,tbl_tr_trips.TripName,tbl_tr_route.RouteName, tbl_tr_routetype.`Type`,  tbl_tr_trips.StartTime, tbl_tr_trips.EndTime, tbl_tr_trips.Distance, tbl_tr_trips.Capacity,tbl_tr_trips.ExtraDistance, tbl_tr_trips.ContactNo , tbl_tr_trips.VehicleId, tbl_tr_trips.Occupied from tbl_tr_trips inner join tbl_tr_route on tbl_tr_trips.RouteId=tbl_tr_route.Id inner JOIN tbl_tr_routetype on tbl_tr_trips.RouteTypeId=tbl_tr_routetype.Id";
            MyDataSet = GetDetailedDataSet(MyTransMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql));

            if (MyDataSet.Tables[0].Rows.Count > 0)
            {
                ViewState["TripDataSet"] = MyDataSet;

                Grd_Trips.Columns[0].Visible = true;
                Grd_Trips.DataSource = MyDataSet;
                Grd_Trips.DataBind();
                Grd_Trips.Columns[0].Visible = false;
            }
            else
            {
                Lbl_tripnote.Text = "No trips found.";

            }
        }

        private DataSet GetDetailedDataSet(DataSet TripdataSet)
        {
            if (TripdataSet != null && TripdataSet.Tables[0] != null && TripdataSet.Tables[0].Rows.Count != 0)
            {
                int capacity = 0,id=0,vehId=0;
                string Vehicle = "None";
                DataTable dt;
                DataRow dr;
                dt = TripdataSet.Tables[0];
                dt.Columns.Add("Vehicle");

                foreach (DataRow dro in TripdataSet.Tables[0].Rows)
                {
                    id = int.Parse(dro[0].ToString());
                    vehId = int.Parse(dro["VehicleId"].ToString());
                    capacity = int.Parse(dro["Capacity"].ToString());
                    if (vehId > 0)
                    {
                      //  capacity = MyTransMang.GetMaxCapacity(capacity, vehId);
                        Vehicle=MyTransMang.GetVehInfo(vehId);
                    }

                   // dro["Capacity"] = capacity.ToString();
                    dro["Vehicle"] = Vehicle.ToString();
                }

            }
            return TripdataSet;
        }

        protected void Grd_Trips_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Grd_Items.PageIndex = 0;
            //FillItemGrid("", int.Parse(Drp_Categories.SelectedValue));
            if (ViewState["TripDataSet"] != null)
            {
                DataSet _sortlDS = (DataSet)ViewState["TripDataSet"];

                if (_sortlDS.Tables[0].Rows.Count > 0)
                {
                    DataTable dtData = _sortlDS.Tables[0];

                    DataView dataView = new DataView(dtData);

                    dataView.Sort = e.SortExpression + " " + GetSortDirection1(e.SortExpression);
                    Grd_Trips.Columns[0].Visible = true;
                    Grd_Trips.DataSource = dataView;
                    Grd_Trips.DataBind();
                    Grd_Trips.Columns[0].Visible = false;
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

        protected void Grd_Trips_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Trips.PageIndex = e.NewPageIndex;
            // FillItemGrid("", int.Parse(Drp_Categories.SelectedValue));
            if (ViewState["TripDataSet"] != null)
            {
                DataSet _pageDS = (DataSet)ViewState["TripDataSet"];
                if (_pageDS.Tables[0].Rows.Count > 0)
                {
                    Grd_Trips.Columns[0].Visible = true;
                    DataTable dtpageData = _pageDS.Tables[0];

                    DataView dataView = new DataView(dtpageData);

                    if (Session["SortDirection1"] != null && Session["SortExpression1"] != null)
                    {
                        dataView.Sort = (string)Session["SortExpression1"] + " " + (string)Session["SortDirection1"];
                    }

                    Grd_Trips.DataSource = dataView;
                    Grd_Trips.DataBind();
                    Grd_Trips.Columns[0].Visible = false;

                }
                else
                {
                 
                }
            }
        }

        protected void Grd_Trips_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _TripId = int.Parse(Grd_Trips.SelectedRow.Cells[0].Text.ToString());
            LoadEditPageStyles();
            LoadAllTripDetailsToEditPage(_TripId);
           Editonesidetrip.Visible = false;
        }

        #endregion

        #region SMS

        protected void Grd_Trips_Deleting(object sender, GridViewDeleteEventArgs e)
        {
            lbl_SMStripId.Text = Grd_Trips.Rows[e.RowIndex].Cells[0].Text.ToString();
            chk_driver.Checked = true;
            chk_parents.Checked = true;
            MPE_SMSMessageBox.Show();
        }

        protected void btn_sendSMS_Click(object sender, EventArgs e)
        {
            int recivr = 0;
            int tripId = int.Parse(lbl_SMStripId.Text);
            string phonelist = "";
            string msg = "";
            string failedList = "";
            char symbol = MysmsMang.GetSMS_NumberSeperator_FromDatabase();
            if (Data_Complete(out msg))
            {
                if (!chk_driver.Checked)
                {
                    recivr = 1;
                }
                else if (!chk_parents.Checked)
                {
                    recivr = 2;
                }
                if (phonelist != "")
                {
                    phonelist = phonelist + symbol.ToString();
                }
                phonelist = phonelist + MysmsMang.Get_DriverAndTripStudentParentPhoneNo_List(tripId,recivr);


                if (phonelist != "")
                {//dominic sms
                    if (MysmsMang.SendBULKSms(phonelist, txt_SMS.Text, "90366450445", "WINER", true, out  failedList))
                    {
                        msg = "Trip Info SMS has been Send Successfully";
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "SMS In Trip Management", "Message : " + txt_SMS.Text, 1);
                    }
                    else
                    {
                        msg = "Error while Sending SMS.";
                    }
                }
                else
                {
                    msg = "Number not available for sending the message";
                }
            }
            WC_MessageBox.ShowMssage(msg);

            MPE_SMSMessageBox.Show();

        }

        private bool Data_Complete(out string msg)
        {
            bool valid = true;
            msg = "";

            if (txt_SMS.Text.Trim() == "")
            {
                msg = "Enter SMS Message";
                valid = false;
            }
            if (!chk_parents.Checked && !chk_driver.Checked)
            {
                msg = "Select Recepients for SMS";
                valid = false;
            }
            return valid;
        }

        protected void btn_chkSMS_Click(object sender, EventArgs e)
        {

            string msg = "";
            if (MysmsMang.CheckConnection(out msg))
            {
                msg = "Successfully Connected";

            }
         
            WC_MessageBox.ShowMssage(msg);
        }

        #endregion
        
        #region ADD NEW TRIP DETAILS FUNCTIONS

        protected void Txt_From_TextChanged(object sender, EventArgs e)
        {
            try
            {
                LoadRouteDtls();
            }
            catch
            {

            }
        }

        protected void Lnk_AddNewItem_Click(object sender, EventArgs e)
        {
            Pnl_AddButtonArea.Visible = false;
            Pnl_TripList.Visible = false;
            Pnl_AddNewTrip.Visible = true;
            Pnl_EditTrip.Visible = false;
            txt_TripName.Text = "";

            LoadvehicleDropDown();
            LoadRouteDropDown();
            GetVehicleCapacity(int.Parse(drp_veshicle.SelectedValue));
            rowonesidetrip.Visible = false;
        }

        private void LoadRouteDropDown()
        {
            Drp_Routes.Items.Clear();

            string sql = "SELECT tbl_tr_route.RouteName, tbl_tr_route.Id from tbl_tr_route order by tbl_tr_route.Id asc";
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("Select Route", "0");
                Drp_Routes.Items.Add(li);

                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_Routes.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Routes Found", "-1");
                Drp_Routes.Items.Add(li);
            }
            int _Direction = int.Parse(Drp_Routes.SelectedValue), _DirectionType = 0;
            sql = "SELECT tbl_tr_route.RouteTypeId from tbl_tr_route where tbl_tr_route.Id=" + _Direction;
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out _DirectionType);
            }
            LoadDirectionsTodropDown(_DirectionType);
        }

        private void LoadDirectionsTodropDown(int _Direction)
        {
            Drp_Directions.Items.Clear();
            string sql = "";
            if (_Direction == 1)
            {
                sql = "SELECT tbl_tr_routetype.Type, tbl_tr_routetype.Id from tbl_tr_routetype where tbl_tr_routetype.Id!=" + _Direction;
            }

            else
            {
                sql = "SELECT tbl_tr_routetype.Type, tbl_tr_routetype.Id from tbl_tr_routetype where tbl_tr_routetype.Id=" + _Direction;
            }

            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_Directions.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Direction Found", "-1");
                Drp_Directions.Items.Add(li);
            }
        }

        private void LoadvehicleDropDown()
        {
            drp_veshicle.Items.Clear();

            string sql = "select tbl_tr_vehicletype.VehicleType, tbl_tr_vehicle.VehicleNo, tbl_tr_vehicle.Id from tbl_tr_vehicle inner join tbl_tr_vehicletype on  tbl_tr_vehicle.TypeId = tbl_tr_vehicletype.Id";
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("Select Vehicle", "0");
                drp_veshicle.Items.Add(li);

                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(0).ToString()+"/" + MyReader.GetValue(1).ToString(), MyReader.GetValue(2).ToString());
                    drp_veshicle.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Vehicles Found", "-1");
                drp_veshicle.Items.Add(li);
            }
        }

        protected void Drp_Routes_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear();
            int _Direction = int.Parse(Drp_Routes.SelectedValue), _DirectionType = 0;
            string sql = "SELECT tbl_tr_route.RouteTypeId from tbl_tr_route where tbl_tr_route.Id=" + _Direction;
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out _DirectionType);
            }

            LoadDirectionsTodropDown(_DirectionType);
            LoadRouteDtls();
        }

        private void LoadRouteDtls()
        {

            try
            {
                LoadEndTimeValue();
                LoadDestinationGrid();
                Btn_SaveVehicle.Enabled = true;
            }
            catch (Exception error)
            {
                WC_MessageBox.ShowMssage(error.ToString());

            }
        }

        protected void Drp_Directions_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRouteDtls();
        }

        protected void Btn_LoadDestinations_Click(object sender, EventArgs e)
        {
        }

        private void LoadEndTimeValue()
        {
            string FromTime = GetTimeFromTextBox(Txt_From.Text);

            TimeSpan _StartTime = TimeSpan.Parse(FromTime);
            int _IndDestTime = 0;
            string _EndTime = "";
            string sql = "SELECT  max(tbl_tr_routedestinations.`Time`),max(tbl_tr_routedestinations.Distance) from  tbl_tr_routedestinations inner join tbl_tr_destinations on tbl_tr_routedestinations.DestinationId=tbl_tr_destinations.Id where tbl_tr_routedestinations.RouteId=" + int.Parse(Drp_Routes.SelectedValue);
            MyReader1 = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader1.HasRows)
            {
                int.TryParse(MyReader1.GetValue(0).ToString(), out _IndDestTime);
                lbl_routeDist.Text = MyReader1.GetValue(1).ToString();
            }
            _EndTime = GetTimeFromDataFromSchool(_StartTime, _IndDestTime);

            lbl_EndTime.Text = _EndTime;
        }

        private string GetTimeFromTextBox(string TextTime)
        {
            string time = "";

            int _Day, _Month, _Year, _Hour, _Minute;
            string _AP_PM;

            string[] _TimeArray = TextTime.Split(':');// store hh:mm AM/PM
            _Hour = int.Parse(_TimeArray[0]);// hour
            _TimeArray = _TimeArray[1].Split(' ');
            _Minute = int.Parse(_TimeArray[0]);
            _AP_PM = _TimeArray[1].Trim();
            if (_AP_PM == "PM" && _Hour != 12)
                _Hour = _Hour + 12;
            else if (_AP_PM == "AM" && _Hour == 12)
                _Hour = 0;
            time = _Hour + ":" + _Minute + ":00";
            return time;
        }

        private string GetformatedTime(string _EndTime)
        {

            string time = "",h="",m="";

            int _Day, _Month, _Year, _Hour, _Minute;
            string _AM_PM = "AM";

            string[] _TimeArray = _EndTime.Split(':');// store hh:mm AM/PM
            _Hour = int.Parse(_TimeArray[0]);// hour
            _Minute = int.Parse(_TimeArray[1]);

            if (_Hour == 0)
            {
                _Hour = 12;
            }
            else if (_Hour == 12)
            {
                _AM_PM = "PM";
            }
            else if (_Hour > 12)
            {
                _AM_PM = "PM";
                _Hour = _Hour - 12;
            }
            if (_Hour < 10)
            {
                h = "0";
            }
            if (_Minute < 10)
            {
                m = "0";
            }
            time = h+_Hour + ":" +m+ _Minute + " " + _AM_PM;
            return time;
        }

        private void LoadDestinationGrid()
        {
            Grd_RouteDestinations.DataSource = null;
            Grd_RouteDestinations.DataBind();
            ViewState["RouteDestination"] = null;

            MyDataSet = GetRouteDestinationDataSet();

            ViewState["RouteDestination"] = MyDataSet;
            if (MyDataSet.Tables[0].Rows.Count > 0)
            {
                Grd_RouteDestinations.Columns[2].Visible = true;
                Grd_RouteDestinations.DataSource = MyDataSet;
                Grd_RouteDestinations.DataBind();
                Grd_RouteDestinations.Columns[2].Visible = false;
            }
            else
            {
                WC_MessageBox.ShowMssage("No destinations present for Selected Type");
            }
        }

        private DataSet GetRouteDestinationDataSet()
        {
            string fromtime = GetTimeFromTextBox(Txt_From.Text);
            TimeSpan _StartTime = TimeSpan.Parse(fromtime);
            TimeSpan _EndTime = TimeSpan.Parse(GetTimeFromTextBox(lbl_EndTime.Text));
            int _TripDirection = 0;
            _TripDirection = int.Parse(Drp_Directions.SelectedValue);

            MyDataSet = new DataSet();
            DataTable dt;
            DataRow dr;
            MyDataSet.Tables.Add(new DataTable("RouteDestination"));
            dt = MyDataSet.Tables["RouteDestination"];
            dt.Columns.Add("Destination");
            dt.Columns.Add("Time");
            dt.Columns.Add("DestinationId");
            if (_TripDirection == 2)
            {
                string sql = "SELECT  tbl_tr_destinations.Destination,tbl_tr_routedestinations.`Time`, tbl_tr_routedestinations.DestinationId from  tbl_tr_routedestinations inner join tbl_tr_destinations on tbl_tr_routedestinations.DestinationId=tbl_tr_destinations.Id where tbl_tr_routedestinations.RouteId=" + int.Parse(Drp_Routes.SelectedValue) + " order by tbl_tr_routedestinations.DestinationOrder asc";
                MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        dr = MyDataSet.Tables["RouteDestination"].NewRow();
                        dr["Destination"] = MyReader.GetValue(0).ToString();
                        dr["Time"] = GetTimeFromDataFromSchool(_StartTime, int.Parse(MyReader.GetValue(1).ToString())).ToString();
                        dr["DestinationId"] = MyReader.GetValue(2).ToString();
                        MyDataSet.Tables["RouteDestination"].Rows.Add(dr);
                    }
                }

            }
            if (_TripDirection == 3)
            {

                string sql = "SELECT  tbl_tr_destinations.Destination,tbl_tr_routedestinations.`Time`, tbl_tr_routedestinations.DestinationId from  tbl_tr_routedestinations inner join tbl_tr_destinations on tbl_tr_routedestinations.DestinationId=tbl_tr_destinations.Id where tbl_tr_routedestinations.RouteId=" + int.Parse(Drp_Routes.SelectedValue) + " order by tbl_tr_routedestinations.DestinationOrder desc";
                MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        dr = MyDataSet.Tables["RouteDestination"].NewRow();
                        dr["Destination"] = MyReader.GetValue(0).ToString();
                        dr["Time"] = GetTimeFromDataToSchool(_EndTime, int.Parse(MyReader.GetValue(1).ToString())).ToString();
                        dr["DestinationId"] = MyReader.GetValue(2).ToString();
                        MyDataSet.Tables["RouteDestination"].Rows.Add(dr);
                    }
                }
            }
            return MyDataSet;

        }

        private string GetTimeFromDataToSchool(TimeSpan _EndTime, int _IndTime)
        {
            string _DestinationTime = "";
            TimeSpan _DestTime = new TimeSpan();
            _DestTime = GetDestTime(_IndTime);
            _DestinationTime = (_EndTime - _DestTime).ToString();

            return GetformatedTime(_DestinationTime);
        }

        private string GetTimeFromDataFromSchool(TimeSpan _StartTime, int _IndTime)
        {
            string _DestinationTime = "";
            TimeSpan _DestTime = new TimeSpan();
            _DestTime = GetDestTime(_IndTime);
            _DestinationTime = (_StartTime + _DestTime).ToString();

            return GetformatedTime(_DestinationTime);
        }

        private TimeSpan GetDestTime(int _IndTime)
        {
            TimeSpan _DestTime1 = new TimeSpan();
            string _Hour = "00", _min = "00", _sec = "00", _t = "";
            int hh = 0, mm = 0, ss = 0, a = _IndTime;
            if (_IndTime < 60)
            {
                mm = _IndTime;
            }
            else
            {
                while (a > 0)
                {
                    hh = a / 60;
                    a = a % 60;
                    mm = a % 60;
                    a = a / 60;
                }
            }
            if (hh < 10)
            {
                _Hour = "0" + hh.ToString();
            }
            else
            {
                _Hour = hh.ToString();
            }
            if (mm < 10)
            {
                _min = "0" + mm.ToString();
            }
            else
            {
                _min = mm.ToString();
            }
            _t = _Hour + ":" + _min + ":" + _sec;

            _DestTime1 = TimeSpan.Parse(_t);

            return _DestTime1;
        }

        protected void Btn_SaveVehicle_Click(object sender, EventArgs e)
        {
            string _TripName = txt_TripName.Text.Trim();
            int _VehicleId = int.Parse(drp_veshicle.SelectedValue); ;
            int _TripId = 0, capacity = 0;
            int _RouteId = int.Parse(Drp_Routes.SelectedValue);
            int _RouteTypeId = int.Parse(Drp_Directions.SelectedValue);
            string phone = txt_phone.Text;
            double extradist = 0;
            if (_RouteId != 0)
            {
                int.TryParse(txt_capacity.Text, out capacity);
                double.TryParse(txt_ExtraDistance.Text, out extradist);

                string fromtime = GetTimeFromTextBox(Txt_From.Text);
                TimeSpan _StartTime = TimeSpan.Parse(fromtime);

                TimeSpan _EndTime = TimeSpan.Parse(GetTimeFromTextBox(lbl_EndTime.Text.Trim()));
                double _Distance = GetDistanceforSelectedRoute(_RouteId);
                int _OneSideTrip = 1;
                if (Chk_TripDirection.Checked == false)
                {
                    _OneSideTrip = 0;
                    _Distance = _Distance * 2;
                }
                string _starttime = Txt_From.Text;
                int value = 0;
                int tripId = 0;
                int routetype = int.Parse(Drp_Directions.SelectedValue);
                if (TimeAlreadyAssigned(_VehicleId, _starttime, value, tripId, routetype))
                {
                    if (!TripNameAlreadyExists(_TripName))
                    {
                        try
                        {
                            SaveTripDetailsToTable(_TripName, _VehicleId, _RouteId, _RouteTypeId, _StartTime, _EndTime, _Distance, _OneSideTrip, extradist, phone, capacity, out _TripId);
                            SaveTripandDestinationDetailsToTable(_TripId);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Trip Management", "Trip:" + _TripName + " added", 1);
                            WC_MessageBox.ShowMssage("Trip Created Successfully");

                            LoadInitialStage();
                            Clear();

                            txt_TripName.Text = "";
                        }
                        catch (Exception error)
                        {
                            WC_MessageBox.ShowMssage(error.ToString());

                        }
                    }
                    else
                    {
                        WC_MessageBox.ShowMssage("Trip Name Already Exists!");
                    }
                }
                else
                {
                    WC_MessageBox.ShowMssage("A trip is assigned to this vehicle in the same time,Select any other vehicle");
                }

            }

            else
            {
                WC_MessageBox.ShowMssage("No route Selected..!");
            }
        }

        private void SaveTripandDestinationDetailsToTable(int _TripId)
        {
            int _DestinationId = 0;
            TimeSpan _DestTime = new TimeSpan();

            foreach (GridViewRow gv in Grd_RouteDestinations.Rows)
            {
                
                _DestinationId = int.Parse(gv.Cells[2].Text);
                _DestTime = TimeSpan.Parse( GetTimeFromTextBox(gv.Cells[1].Text));
                InsertRouteDestinationValuesIntoTable(_TripId, _DestinationId, _DestTime);
            }
        }

        private void InsertRouteDestinationValuesIntoTable(int _TripId, int _DestinationId, TimeSpan _DestTime)
        {
            string sql = "INSERT into tbl_tr_tripdestinations(TripId,DestinationId) VALUES (" + _TripId + "," + _DestinationId + ")";
            MyTransMang.m_MysqlDb.ExecuteQuery(sql);
        }

        private bool TripNameAlreadyExists(string _TripName)
        {
            bool _Exists = false;
            string sql = "SELECT tbl_tr_trips.TripName from tbl_tr_trips where tbl_tr_trips.TripName='" + _TripName + "'";
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _Exists = true;
            }
            return _Exists;
        }

        private void SaveTripDetailsToTable(string _TripName, int _VehicleId, int _RouteId, int _RouteTypeId, TimeSpan _StartTime, TimeSpan _EndTime, double _Distance, int _OneSideTrip, double extradist, string phone, int capacity, out int _TripId)
        {
            _TripId = 0;
            string sql = "";
            sql = "INSERT into tbl_tr_trips(TripName,VehicleId,RouteId,RouteTypeId,StartTime,EndTime,Distance,IsOneSideTrip, ExtraDistance,ContactNo,Capacity,Occupied) VALUES ('" + _TripName + "'," + _VehicleId + "," + _RouteId + "," + _RouteTypeId + ",'" + _StartTime + "','" + _EndTime + "'," + _Distance + "," + _OneSideTrip + "," + extradist + ",'" + phone + "'," + capacity + ",0)";
            MyTransMang.m_MysqlDb.ExecuteQuery(sql);

            sql = "SELECT tbl_tr_trips.Id from tbl_tr_trips where tbl_tr_trips.TripName='" + _TripName + "'";
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out _TripId);
            }
        } 

        private double GetDistanceforSelectedRoute(int _RouteId)
        {
            double _Distance = 0;
            string sql = "SELECT  tbl_tr_route.OneSideDistance from tbl_tr_route where tbl_tr_route.Id=" + _RouteId;
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                double.TryParse(MyReader.GetValue(0).ToString(), out _Distance);
            }
            return _Distance;
        }

        protected void Btn_VehicleCancel_Click(object sender, EventArgs e)
        {
            LoadInitialStage();
            Clear();

            txt_TripName.Text = "";
        }

        #endregion

        protected void btn_SMS_Click(object sender, EventArgs e)
        {
            lbl_SMStripId.Text = lbl_TripId.Text.Trim();
            chk_driver.Checked = true;
            chk_parents.Checked = true;
            MPE_SMSMessageBox.Show();
        }

        #region EDIT TRIP DETAILS FUNCTIONS

        protected void Btn_EditTrip_Click(object sender, EventArgs e)
        {

            loadEditvehicleDrp(lbl_vehicle.Text);
            LoadUpdatePageStyle();
            LoadEditRouteDropDown(lbl_RouteName.Text.Trim());
            LoadEditTripTimings();
            GetVehicleCapacity(int.Parse(drp_editVehicle.SelectedValue));
        }

        private void LoadAllTripDetailsToEditPage(int _TripId)
        {

            lbl_TripName.Text = Grd_Trips.SelectedRow.Cells[1].Text.ToString();
            lbl_RouteName.Text = Grd_Trips.SelectedRow.Cells[2].Text.ToString();
            lbl_Direction.Text = Grd_Trips.SelectedRow.Cells[3].Text.ToString();
            lbl_StartTime.Text = GetformatedTime( Grd_Trips.SelectedRow.Cells[4].Text.ToString());
            lbl_EditEndTime.Text = GetformatedTime( Grd_Trips.SelectedRow.Cells[5].Text.ToString());
            lbl_capacity.Text = Grd_Trips.SelectedRow.Cells[8].Text.ToString();
            lbl_extradistance.Text = Grd_Trips.SelectedRow.Cells[7].Text.ToString();
            lbl_vehicle.Text = Grd_Trips.SelectedRow.Cells[10].Text.ToString();
            lbl_phone.Text = Grd_Trips.SelectedRow.Cells[11].Text.ToString();

            lbl_TripId.Text = _TripId.ToString();
            int _Oneside = 0;
            string sql = "SELECT tbl_tr_trips.IsOneSideTrip from tbl_tr_trips where tbl_tr_trips.Id=" + _TripId;
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out _Oneside);
            }
            if (_Oneside == 0)
            {
                lbl_OneSideTrip.Text = "No";
                Chk_EditTripOneSide.Checked = false;
            }
            else
            {
                lbl_OneSideTrip.Text = "Yes";
                Chk_EditTripOneSide.Checked = true;
            }

            txt_tripId.Text = Grd_Trips.SelectedRow.Cells[0].Text.ToString();
            txt_EditTripName.Text = Grd_Trips.SelectedRow.Cells[1].Text.ToString();
            Txt_EditFrom.Text = GetformatedTime(Grd_Trips.SelectedRow.Cells[4].Text.Trim()).ToString();
            txt_editphone.Text = Grd_Trips.SelectedRow.Cells[11].Text.ToString();
            txt_editExtradistance.Text = Grd_Trips.SelectedRow.Cells[7].Text.ToString();
            txt_editcapcity.Text = Grd_Trips.SelectedRow.Cells[8].Text.ToString();
        }

        private void LoadEditPageStyles()
        {
            Pnl_AddButtonArea.Visible = true;
            Pnl_EditTrip.Visible = true;
            Pnl_TripList.Visible = false;

            lbl_Direction.Visible = true;
            lbl_EditEndTime.Visible = true;
            lbl_OneSideTrip.Visible = true;
            lbl_RouteName.Visible = true;
            lbl_StartTime.Visible = true;
            lbl_TripName.Visible = true;

            lbl_capacity.Visible = true;
            lbl_extradistance.Visible = true;
            lbl_vehicle.Visible = true;
            lbl_phone.Visible = true;

            Btn_EditTrip.Visible = true;
            Btn_UpdateTrip.Visible = false;

            txt_EditTripName.Visible = false;
            Txt_EditFrom.Visible = false;
            Drp_EditRoute.Visible = false;
            Drp_EditDirection.Visible = false;

            txt_editphone.Visible = false;
            txt_editExtradistance.Visible = false;
            txt_editcapcity.Visible = false;
            drp_editVehicle.Visible = false;

            Chk_EditTripOneSide.Visible = false;

        }

        private void loadEditvehicleDrp(string vehicle)
        {
            drp_editVehicle.Items.Clear();

            string sql = "select tbl_tr_vehicletype.VehicleType, tbl_tr_vehicle.VehicleNo, tbl_tr_vehicle.Id from tbl_tr_vehicle inner join tbl_tr_vehicletype on  tbl_tr_vehicle.TypeId = tbl_tr_vehicletype.Id";
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("None", "0"); 
                drp_editVehicle.Items.Add(li);
                while (MyReader.Read())
                {

                    li = new ListItem(MyReader.GetValue(0).ToString() + "/" + MyReader.GetValue(1).ToString(), MyReader.GetValue(2).ToString());
                    drp_editVehicle.Items.Add(li);
                }
                for (int i = 0; i < drp_editVehicle.Items.Count; i++)
                {
                    if (drp_editVehicle.Items[i].Text == vehicle)
                    {
                        drp_editVehicle.SelectedIndex = i;
                        break;
                    }
                }

            }
            else
            {
                ListItem li = new ListItem("No Vehicles Found", "-1");
                drp_editVehicle.Items.Add(li);
            }
        }

        private void LoadEditRouteDropDown(string _SelectedRouteName)
        {
            Drp_EditRoute.Items.Clear();

            string sql = "SELECT tbl_tr_route.RouteName, tbl_tr_route.Id from tbl_tr_route order by tbl_tr_route.Id asc";
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_EditRoute.Items.Add(li);
                }
                for (int i = 0; i < Drp_EditRoute.Items.Count; i++)
                {
                    if (Drp_EditRoute.Items[i].Text == _SelectedRouteName.Trim())
                    {
                        Drp_EditRoute.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                ListItem li = new ListItem("No Route Found", "-1");
                Drp_EditRoute.Items.Add(li);
            }
            int _Direction = int.Parse(Drp_EditRoute.SelectedValue), _DirectionType = 0;
            sql = "SELECT tbl_tr_route.RouteTypeId from tbl_tr_route where tbl_tr_route.Id=" + _Direction;
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out _DirectionType);
            }
            LoadEditDirectionsTodropDown(_DirectionType, lbl_Direction.Text.Trim());
        }

        private void LoadEditDirectionsTodropDown(int _DirectionType, string _SelectedDirection)
        {
            Drp_EditDirection.Items.Clear();
            string sql = "";
            if (_DirectionType == 1)
            {
                sql = "SELECT tbl_tr_routetype.Type, tbl_tr_routetype.Id from tbl_tr_routetype where tbl_tr_routetype.Id!=" + _DirectionType;
            }
            else
            {
                sql = "SELECT tbl_tr_routetype.Type, tbl_tr_routetype.Id from tbl_tr_routetype where tbl_tr_routetype.Id=" + _DirectionType;
            }

            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_EditDirection.Items.Add(li);
                }
                if (_SelectedDirection != "")
                {
                    for (int i = 0; i < Drp_EditDirection.Items.Count; i++)
                    {
                        if (Drp_EditDirection.Items[i].Text == _SelectedDirection.Trim())
                        {
                            Drp_EditDirection.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            else
            {
                ListItem li = new ListItem("No Category Found", "-1");
                Drp_EditDirection.Items.Add(li);
            }
        }

        protected void Btn_EditCancel_Click(object sender, EventArgs e)
        {
            LoadInitialStage();
            Clear();

            txt_TripName.Text = "";
        }

        protected void Drp_EditRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear();
            int _Direction = int.Parse(Drp_EditRoute.SelectedValue), _DirectionType = 0;
            string sql = "SELECT tbl_tr_route.RouteTypeId from tbl_tr_route where tbl_tr_route.Id=" + _Direction;
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out _DirectionType);
            }
            LoadEditDirectionsTodropDown(_DirectionType, "");

            LoadEditTripTimings();
        }

        protected void Drp_EditDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadEditTripTimings();
        }

        protected void Txt_EditFrom_TextChanged(object sender, EventArgs e)
        {
            LoadEditTripTimings();
        }


        #endregion
        
        #region UPDATE TRIP DETAILS FUNCTIONS

        private void LoadUpdatePageStyle()
        {
            lbl_Direction.Visible = false;
            lbl_EditEndTime.Visible = true;
            lbl_OneSideTrip.Visible = false;
            lbl_RouteName.Visible = false;
            lbl_StartTime.Visible = false;
            lbl_TripName.Visible = false;
            Btn_EditTrip.Visible = false;
            Btn_UpdateTrip.Visible = true;


            lbl_capacity.Visible = false;
            lbl_extradistance.Visible = false;
            lbl_vehicle.Visible = false;
            lbl_phone.Visible = false;

            txt_EditTripName.Visible = true;
            Txt_EditFrom.Visible = true;
            Drp_EditRoute.Visible = true;
            Drp_EditDirection.Visible = true;
            Chk_EditTripOneSide.Visible = true;

            txt_editphone.Visible = true;
            txt_editExtradistance.Visible = true;
            txt_editcapcity.Visible = true;
            drp_editVehicle.Visible = true;
        }

        private void SaveNewTripandDestinationDetailsToTable(int _TripId)
        {
            string sql = "DELETE from tbl_tr_tripdestinations where tbl_tr_tripdestinations.TripId=" + _TripId;
            MyTransMang.m_MysqlDb.ExecuteQuery(sql);

            int _DestinationId = 0;
            TimeSpan _DestTime = new TimeSpan();

            foreach (GridViewRow gv in Grd_EditDestinations.Rows)
            {
                _DestinationId = int.Parse(gv.Cells[2].Text);
                _DestTime = TimeSpan.Parse(GetTimeFromTextBox(gv.Cells[1].Text));
                InsertRouteDestinationValuesIntoTable(_TripId, _DestinationId, _DestTime);
            }
        }

        private bool NewTripNameAlreadyExists(string _TripName, int _TripId)
        {
            bool _Exists = false;
            string sql = "SELECT tbl_tr_trips.TripName from tbl_tr_trips where tbl_tr_trips.TripName='" + _TripName + "' and tbl_tr_trips.Id!=" + _TripId;
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _Exists = true;
            }
            return _Exists;
        }

        private void LoadEditTripTimings()
        {
            try
            {
                LoadEditEndTimeValue();
                LoadEditDestinationGrid();
                Btn_UpdateTrip.Enabled = true;
            }
            catch (Exception error)
            {
                WC_MessageBox.ShowMssage(error.ToString());
            }
        }

        private void LoadEditDestinationGrid()
        {
            Grd_EditDestinations.DataSource = null;
            Grd_EditDestinations.DataBind();
            ViewState["RouteDestination"] = null;

            MyDataSet = GetEditRouteDestinationDataSet();

            ViewState["RouteDestination"] = MyDataSet;
            if (MyDataSet.Tables[0].Rows.Count > 0)
            {
                Grd_EditDestinations.Columns[2].Visible = true;
                Grd_EditDestinations.DataSource = MyDataSet;
                Grd_EditDestinations.DataBind();
                Grd_EditDestinations.Columns[2].Visible = false;
            }
            else
            {
                WC_MessageBox.ShowMssage("No item present for Selected Type");

            }
        }

        private DataSet GetEditRouteDestinationDataSet()
        {
            string fromtime = GetTimeFromTextBox(Txt_EditFrom.Text);
            TimeSpan _StartTime = TimeSpan.Parse(fromtime);
            TimeSpan _EndTime = TimeSpan.Parse(GetTimeFromTextBox(lbl_EditEndTime.Text));

            int _TripDirection = 0;
            _TripDirection = int.Parse(Drp_EditDirection.SelectedValue);

            MyDataSet = new DataSet();
            DataTable dt;
            DataRow dr;
            MyDataSet.Tables.Add(new DataTable("RouteDestination"));
            dt = MyDataSet.Tables["RouteDestination"];
            dt.Columns.Add("Destination");
            dt.Columns.Add("Time");
            dt.Columns.Add("DestinationId");
            if (_TripDirection == 2)
            {
                string sql = "SELECT  tbl_tr_destinations.Destination,tbl_tr_routedestinations.`Time`, tbl_tr_routedestinations.DestinationId from  tbl_tr_routedestinations inner join tbl_tr_destinations on tbl_tr_routedestinations.DestinationId=tbl_tr_destinations.Id where tbl_tr_routedestinations.RouteId=" + int.Parse(Drp_EditRoute.SelectedValue) + " order by tbl_tr_routedestinations.DestinationOrder asc";
                MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        dr = MyDataSet.Tables["RouteDestination"].NewRow();
                        dr["Destination"] = MyReader.GetValue(0).ToString();
                        dr["Time"] = GetTimeFromDataFromSchool(_StartTime, int.Parse(MyReader.GetValue(1).ToString())).ToString();
                        dr["DestinationId"] = MyReader.GetValue(2).ToString();
                        MyDataSet.Tables["RouteDestination"].Rows.Add(dr);
                    }
                }

            }
            if (_TripDirection == 3)
            {

                string sql = "SELECT  tbl_tr_destinations.Destination,tbl_tr_routedestinations.`Time`, tbl_tr_routedestinations.DestinationId from  tbl_tr_routedestinations inner join tbl_tr_destinations on tbl_tr_routedestinations.DestinationId=tbl_tr_destinations.Id where tbl_tr_routedestinations.RouteId=" + int.Parse(Drp_EditRoute.SelectedValue) + " order by tbl_tr_routedestinations.DestinationOrder desc";
                MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        dr = MyDataSet.Tables["RouteDestination"].NewRow();
                        dr["Destination"] = MyReader.GetValue(0).ToString();
                        dr["Time"] = GetTimeFromDataToSchool(_EndTime, int.Parse(MyReader.GetValue(1).ToString())).ToString();
                        dr["DestinationId"] = MyReader.GetValue(2).ToString();
                        MyDataSet.Tables["RouteDestination"].Rows.Add(dr);
                    }
                }
            }
            return MyDataSet;
        }

        private void LoadEditEndTimeValue()
        {
            string FromTime = GetTimeFromTextBox(Txt_EditFrom.Text);

            TimeSpan _StartTime = TimeSpan.Parse(FromTime);
            int _IndDestTime = 0;
            string _EndTime = "";

            string sql = "SELECT  max(tbl_tr_routedestinations.`Time`) from  tbl_tr_routedestinations inner join tbl_tr_destinations on tbl_tr_routedestinations.DestinationId=tbl_tr_destinations.Id where tbl_tr_routedestinations.RouteId=" + int.Parse(Drp_EditRoute.SelectedValue);
            MyReader1 = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader1.HasRows)
            {
                int.TryParse(MyReader1.GetValue(0).ToString(), out _IndDestTime);
            }
            _EndTime = GetTimeFromDataFromSchool(_StartTime, _IndDestTime);
            lbl_EditEndTime.Text = _EndTime;
        }

        protected void Btn_UpdateTrip_Click(object sender, EventArgs e)
        {
            string _TripName = txt_EditTripName.Text.Trim();
            int _RouteId = int.Parse(Drp_EditRoute.SelectedValue);
            int _RouteTypeId = int.Parse(Drp_EditDirection.SelectedValue);
            int _VehicleId = int.Parse(drp_editVehicle.SelectedValue);
            int _TripId = int.Parse(lbl_TripId.Text.Trim());
            int capacity = 0;
            string phone = txt_editphone.Text;
            double extradist = 0;
            string msg="";
            int.TryParse(txt_editcapcity.Text, out capacity);
            double.TryParse(txt_editExtradistance.Text, out extradist);

            string fromtime = GetTimeFromTextBox(Txt_EditFrom.Text);
            TimeSpan _StartTime = TimeSpan.Parse(fromtime);

            TimeSpan _EndTime = TimeSpan.Parse(GetTimeFromTextBox(lbl_EditEndTime.Text.Trim()));

            double _Distance = GetDistanceforSelectedRoute(_RouteId);
            int _OneSideTrip = 1;
            if (Chk_EditTripOneSide.Checked == false)
            {
                _OneSideTrip = 0;
                _Distance = _Distance * 2;
            }
             string _starttime = Txt_EditFrom.Text;
                int value = 1;
                int routtype = int.Parse(Drp_EditDirection.SelectedValue);
                if (TimeAlreadyAssigned(_VehicleId, _starttime, value, _TripId, routtype))
                {
                    if (validUpdation(_TripName, _TripId, out msg))
                    {
                        try
                        {
                            UpdateTripDetailsWithNewValues(_TripName, _RouteId, _RouteTypeId, _StartTime, _EndTime, _Distance, _OneSideTrip, _TripId, capacity, _VehicleId, phone, extradist);
                            SaveNewTripandDestinationDetailsToTable(_TripId);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Trip Management", "Trip:" + _TripName + " details updated", 1);
                            msg = "Trip Details Updated";

                            LoadInitialStage();
                            Clear();

                            txt_TripName.Text = "";
                        }
                        catch (Exception error)
                        {
                            msg = error.ToString();
                        }
                    }
                    WC_MessageBox.ShowMssage(msg);
                }
                else
                {
                    WC_MessageBox.ShowMssage("A trip is assigned to this vehicle in the same time,Select any other vehicle");
                }

        }

        private bool validUpdation(string _TripName, int _TripId, out string msg)
        {
            msg="";
            bool valid = true;

            if (NewTripNameAlreadyExists(_TripName, _TripId))
            {
                valid = false;
                msg = "Trip Name Exists..!";
            }
            if (MyTransMang.StudExistsInTrip(_TripId))
            {
                if (lbl_Direction.Text.Trim() != Drp_EditDirection.SelectedItem.Text.Trim())
                {
                    valid = false;
                    msg = "Students in Trip.. Can't change TripType!";
                }
            }
            return valid;
        }


        private void UpdateTripDetailsWithNewValues(string _TripName, int _RouteId, int _RouteTypeId, TimeSpan _StartTime, TimeSpan _EndTime, double _Distance, int _OneSideTrip, int _TripId, int capacity, int vehicleid, string phone, double extradistance)
        {
            string sql = "UPDATE tbl_tr_trips SET tbl_tr_trips.TripName='" + _TripName + "',tbl_tr_trips.RouteId=" + _RouteId + ",tbl_tr_trips.RouteTypeId=" + _RouteTypeId + ", tbl_tr_trips.StartTime='" + _StartTime + "', tbl_tr_trips.EndTime='" + _EndTime + "', tbl_tr_trips.Distance=" + _Distance + ", tbl_tr_trips.IsOneSideTrip=" + _OneSideTrip + " ,tbl_tr_trips.Capacity=" + capacity + ", tbl_tr_trips.VehicleId=" + vehicleid + ", tbl_tr_trips.ContactNo='" + phone + "',tbl_tr_trips.Extradistance=" + extradistance + " where tbl_tr_trips.Id=" + _TripId;
            MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            sql = "update tbl_tr_vehicle set tbl_tr_vehicle.Capacity=" + capacity + " where tbl_tr_vehicle.Id=" + vehicleid + "";
            MyTransMang.m_MysqlDb.ExecuteQuery(sql);
        }

        #endregion

        #region DELETE TRIP DETAILS FUNCTIONS

        protected void Btn_Delete_Click(object sender, EventArgs e)
        {
            int _TripId = int.Parse(lbl_TripId.Text.Trim());
            if (MyTransMang.CanDeleteTrip(_TripId))
            {
                MPE_DeleteConfirm.Show();
            }
            else
            {
                WC_MessageBox.ShowMssage("Can't delete,Student exists in trip");
            }
        }

        protected void Btn_DeleteYes_Click(object sender, EventArgs e)
        {
            int _TripId = int.Parse(lbl_TripId.Text.Trim());
            MyTransMang.DeleteTrip(_TripId);
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Trip Management", "Trip deleted", 1);
            WC_MessageBox.ShowMssage("Trip Deleted Succesfully.");
            Response.Redirect("TripManagement.aspx");
        }

        #endregion

        protected void drp_veshicle_SelectedIndexChanged(object sender, EventArgs e)
        {
            int vehicleId = int.Parse(drp_veshicle.SelectedValue.ToString());
           // DateTime  starttime=General.GetDateTimeFromText(Txt_From.Text);
            string _starttime=Txt_From.Text;
            int value = 0;
            int tripId = 0;
            int routetypeId = int.Parse(Drp_Directions.SelectedValue);
            if (TimeAlreadyAssigned(vehicleId, _starttime, value, tripId, routetypeId))
            {
                //int tripid = int.Parse(txt_tripId.Text);            
                DataSet Vehicl_Ds = new DataSet();
                GetVehicleCapacity(vehicleId);
            }
            else
            {
                WC_MessageBox.ShowMssage("A trip is assigned to this vehicle in the same time,Select any other vehicle");
            }
           
        }

        private bool TimeAlreadyAssigned(int vehicleId, string  starttime,int value,int tripID,int routetype)
        {
            bool exist = true;
            string sql = "";
            string _starttime = starttime.Substring(0, 5);
            string time=_starttime+":00";
            if (value == 0)
            {
                sql = "SELECT tbl_tr_trips.Id from tbl_tr_trips where tbl_tr_trips.VehicleId=" + vehicleId + " and tbl_tr_trips.StartTime='" + time + "' and tbl_tr_trips.RouteTypeId="+routetype+"";
                MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    exist = false;
                }
            }
            else if (value == 1)
            {
                sql = "SELECT tbl_tr_trips.Id from tbl_tr_trips where tbl_tr_trips.VehicleId=" + vehicleId + " and tbl_tr_trips.StartTime='" + time + "' and tbl_tr_trips.Id<>" + tripID + " and tbl_tr_trips.RouteTypeId="+routetype+"";
                MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    exist = false;
                }
            }
            return exist;
        }

        private void GetVehicleCapacity(int vehicleId)
        {            
            string sql = "";
            int capacity = 0;
            sql = "select capacity from tbl_tr_vehicle where id = "+vehicleId+"";
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                capacity = int.Parse(MyReader.GetValue(0).ToString());
            }
            txt_capacity.Text = capacity.ToString();
            txt_editcapcity.Text = capacity.ToString();
        }

        protected void drp_editVehicle_SelectedIndexChanged(object sender, EventArgs e)
        {
            int vehicleId = int.Parse(drp_editVehicle.SelectedValue.ToString());
            int tripId =int.Parse(txt_tripId.Text);
            int value = 1;
             string _starttime=Txt_EditFrom.Text;
             int routype = int.Parse(Drp_EditDirection.SelectedValue);
             if (TimeAlreadyAssigned(vehicleId, _starttime, value, tripId, routype))
            {
                DataSet Vehicl_Ds = new DataSet();
                GetVehicleCapacity(vehicleId);
            }
            else
            {
                WC_MessageBox.ShowMssage("A trip is assigned to this vehicle in the same time,Select any other vehicle");
            }
           
           
        }


    }
}
