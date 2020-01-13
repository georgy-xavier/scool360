using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using WinBase;

namespace WinEr
{
    public partial class RouteManagement : System.Web.UI.Page
    {
        private TransportationClass MyTransMang;
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
            MyTransMang = MyUser.GetTransObj();
            if (MyTransMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(200))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {

                    LoadInitialDeails();

                   
                }

            }
        }

        private void LoadInitialDeails()
        {
            populategrid();
            fill_drp_destins();
            AddRouteTypeToDrpList();
            Pnl_RouteList.Visible = true;
            pnl_add_route.Visible = false;
            lnk_add_route.Visible = true;
            img_Add.Visible = true;
            Pnl_destin_list.Visible = false;
            pnl_route_dtls.Visible = false;
            img_Add.Visible = true;
            lnk_add_route.Visible = true;
            pnl_newDestin.Visible = false;
        }

        private void populategrid()
        {
           
            MyDataSet = getCurrentRoutes();
            ViewState["RouteDataSet"] = MyDataSet;
            if (ViewState["RouteDataSet"] != null)
            {
                MyDataSet = getRouteTripDataSet();
                ViewState["RouteDataSet"] = MyDataSet;
                    Grd_Route.Visible = true;
                    lbl_noRoute.Visible = false;
                    Grd_Route.Columns[0].Visible = true;
                    Grd_Route.Columns[4].Visible = true;
                    Grd_Route.DataSource = MyDataSet;
                    Grd_Route.DataBind();
                    Grd_Route.Columns[0].Visible = false;
                    Grd_Route.Columns[4].Visible = false;
            }
            else
            {
                Grd_Route.Visible = false;
                lbl_noRoute.Visible = true;
                //Lbl_msg.Text = "No Routes are present";
                //MPE_MessageBox.Show();

            }
        }

        private DataSet getRouteTripDataSet()
        {
            DataSet MyDataSet1 = new DataSet();
            MyDataSet = (DataSet) ViewState["RouteDataSet"];
            DataTable dt;
            DataRow dr;
            MyDataSet1.Tables.Add(new DataTable("VehTrip"));
            dt = MyDataSet1.Tables["VehTrip"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Route Name");
            dt.Columns.Add("Distance");
            dt.Columns.Add("Time");
            dt.Columns.Add("Type");
            dt.Columns.Add("Vehicles");
            dt.Columns.Add("Trips");


            int rId = 0;
            foreach (DataRow drow in MyDataSet.Tables[0].Rows)
            {
                rId = int.Parse(drow["Id"].ToString());
                        dr = MyDataSet1.Tables["VehTrip"].NewRow();
                        dr["Id"] = drow["Id"].ToString();
                        dr["Route Name"]=drow["Route Name"].ToString();
                        dr["Distance"] = drow["Distance"].ToString();
                        dr["Time"] = drow["Time"].ToString();
                        dr["Type"] = drow["Type"].ToString();
                        dr["Vehicles"] = MyTransMang.getVehs(rId).ToString();
                        dr["Trips"] = MyTransMang.getTrips(rId).ToString();
                        MyDataSet1.Tables["VehTrip"].Rows.Add(dr);
            }
            return MyDataSet1;
        }

        private DataSet getCurrentRoutes()
        {
            string sql = "SELECT  `tbl_tr_route`.`Id` AS `Id`, `tbl_tr_route`.`RouteName` AS `Route Name`,  `tbl_tr_route`.`OneSideDistance` AS `Distance`, `tbl_tr_route`.`OneSideTime`  AS `Time`, `tbl_tr_routetype`.`Type` AS `Type` FROM  `tbl_tr_route` INNER JOIN  `tbl_tr_routetype` ON `tbl_tr_route`.`RouteTypeId` = `tbl_tr_routetype`.`Id`";
            MyDataSet = MyTransMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return MyDataSet;
        }

        private void AddRouteTypeToDrpList()
        {
            drp_route_type.Items.Clear();
            string sql = "SELECT tbl_tr_routetype.* FROM tbl_tr_routetype";
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    drp_route_type.Items.Add(li);
                }
            }
            drp_route_type.SelectedValue = "1";
        }

        protected void lnk_add_route_Click(object sender, EventArgs e)
        {
            Pnl_RouteList.Visible = false;
            pnl_add_route.Visible = true;
            lnk_add_route.Visible = false;
            img_Add.Visible = false;
            row_type.Visible = false;
        }

        protected void lnk_add_place_Click(object sender, EventArgs e)
        {
            MPE_MessageBox_AddNewPlace.Show();
        }

        protected void btn_add_Click(object sender, EventArgs e)
        {
            int time=0,id = 0;
            double dist = 0;
            id = int.Parse(drp_destination.SelectedValue.ToString());
            if (id >0)
            {
                if (!RouteDestinationExists(id))
                {
                    time = int.Parse(txt_dest_time.Text.ToString());
                    dist = double.Parse(txt_dest_dist.Text.ToString());
                    if (!TimeAndDistCorrect(time, dist))
                    {
                        {

                            DataSet dt = new DataSet();
                            dt = makedataset();


                            if (dt.Tables[0].Rows.Count > 0)
                            {
                                Grd_destins.Columns[0].Visible = true;
                                Grd_destins.DataSource = dt;
                                Grd_destins.DataBind();
                                Grd_destins.Columns[0].Visible = false;
                            }
                        }
                        Pnl_destin_list.Visible = true;
                        txt_dest_time.Text = "";
                        txt_dest_dist.Text = "";
                    }
                    else
                    {
                        Lbl_msg.Text = "Time & Distance Not Less Than Previous Destination!";
                        MPE_MessageBox.Show();
                    }
                }
                else
                {
                    Lbl_msg.Text = "Destination Already Exists!";
                    MPE_MessageBox.Show();
                }
            }
            else
            {
                Lbl_msg.Text = "No Destination!";
                MPE_MessageBox.Show();
            }           

        }

        private bool TimeAndDistCorrect(int time, double dist)
        {
            bool _Exists = false;

            if (Grd_destins.Rows.Count > 0)
            {

                foreach (GridViewRow gv in Grd_destins.Rows)
                {
                    if ((double.Parse(gv.Cells[3].Text.ToString()) > dist) || (int.Parse(gv.Cells[4].Text.ToString()) > time))
                    {
                        _Exists = true;
                        break;
                    }
                }

            }

            return _Exists;
        }

        private DataSet makedataset()
        {

            DataSet MydataSet = new DataSet();
            DataTable dt;
            DataRow dr;
            MydataSet.Tables.Add(new DataTable("Destinations"));
            dt = MydataSet.Tables["Destinations"];
            dt.Columns.Add("Order");
            dt.Columns.Add("Id");
            dt.Columns.Add("Destination");
            dt.Columns.Add("Distance");
            dt.Columns.Add("Time");
            int ordr = 1;

            int i = Grd_destins.Rows.Count;

            if (Grd_destins.Rows.Count > 0)
            {
               

                foreach (GridViewRow gv in Grd_destins.Rows)
                {

                    dr = MydataSet.Tables["Destinations"].NewRow();
                    dr["Order"] = ordr;
                    dr["Id"] = int.Parse(gv.Cells[0].Text.ToString());
                    dr["Destination"] = gv.Cells[2].Text;
                    dr["Distance"] = double.Parse(gv.Cells[3].Text.ToString());
                    dr["Time"] = gv.Cells[4].Text;
                    MydataSet.Tables["Destinations"].Rows.Add(dr);
                    i = int.Parse(gv.Cells[1].Text.ToString());
                    ordr++;
                }
            }

            dr = MydataSet.Tables["Destinations"].NewRow();
            dr["Order"] = ordr;
            dr["id"] = drp_destination.SelectedItem.Value;
            dr["Destination"] = drp_destination.SelectedItem.Text.ToString();
            dr["Distance"] = txt_dest_dist.Text;
            dr["Time"] = txt_dest_time.Text;
            MydataSet.Tables["Destinations"].Rows.Add(dr);
            return MydataSet;


        }

        private DataSet DeleteDestination()
        {

            DataSet MydataSet = new DataSet();
            DataTable dt;
            DataRow dr;
            MydataSet.Tables.Add(new DataTable("Destinations"));
            dt = MydataSet.Tables["Destinations"];
            dt.Columns.Add("Order");
            dt.Columns.Add("Id");
            dt.Columns.Add("Destination");
            dt.Columns.Add("Distance");
            dt.Columns.Add("Time");
            int ordr = 1;

            int i = Grd_destins.Rows.Count;

            if (Grd_destins.Rows.Count > 0)
            {


                foreach (GridViewRow gv in Grd_destins.Rows)
                {

                    dr = MydataSet.Tables["Destinations"].NewRow();
                    dr["Order"] = ordr;
                    dr["Id"] = int.Parse(gv.Cells[0].Text.ToString()); 
                    dr["Destination"] = gv.Cells[2].Text;
                    dr["Distance"] = double.Parse(gv.Cells[3].Text.ToString());
                    dr["Time"] = gv.Cells[4].Text;
                    MydataSet.Tables["Destinations"].Rows.Add(dr);
                    ordr++;
                }
            }

            return MydataSet;


        }

        protected void Btn_Add_new_Place_Click(object sender, EventArgs e)
        {
            Lbl_MsgCreateCategory.Text = "";
            string _Destination = "", _ErrorMsg = "";
            _Destination = txt_new_place.Text.Trim().ToUpper();

            if (_Destination != "")
            {
                if (SaveNewDestination(_Destination, out _ErrorMsg))
                {
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Route Management", "New place:"+_Destination+" added ", 1);
                    fill_drp_destins();

                    // load the created Place as the selected item
                    loadDrp_DestinationSelectedtextAsTheCreatedOne(_Destination);
                    txt_new_place.Text = "";
                }
                else
                {
                    Lbl_MsgCreateCategory.Text = _ErrorMsg;
                    MPE_MessageBox_AddNewPlace.Show();
                }
            }
            else
            {

                MPE_MessageBox_AddNewPlace.Show();
                Lbl_MsgCreateCategory.Text = "Enter Category Name";
                txt_new_place.Text = "";
            }
        }

        private void loadDrp_DestinationSelectedtextAsTheCreatedOne(string _Destination)
        {
            for (int i = 0; i < drp_destination.Items.Count; i++)
            {
                if (drp_destination.Items[i].Text == _Destination)
                {
                    drp_destination.SelectedIndex = i;
                    break;
                }
            }
        }

        private bool SaveNewDestination(string _Destination, out string _ErrorMsg)
        {
            bool _Valid = false;
            _ErrorMsg = "";
            try
            {
                _Destination = _Destination.ToUpper();

                string sql = "SELECT tbl_tr_destinations.Destination from tbl_tr_destinations where tbl_tr_destinations.Destination='" + _Destination + "'";
                MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    _ErrorMsg = "The Destination Name Is Allready Exist";
                }
                else
                {
                    sql = "INSERT into tbl_tr_destinations(Destination) VALUES ('" + _Destination + "')";
                    MyTransMang.m_MysqlDb.ExecuteQuery(sql);
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

        private void fill_drp_destins()
        {
            drp_destination.Items.Clear();
            string sql = "SELECT  `tbl_tr_destinations`.* FROM  `tbl_tr_destinations`ORDER BY  `tbl_tr_destinations`.`Destination`";
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li;
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    drp_destination.Items.Add(li);

                }
                
            }
            else
            {
                ListItem li = new ListItem("No Destinations Found", "-1");
                drp_destination.Items.Add(li);

            }
        }

        protected void btn_add_route_Click(object sender, EventArgs e)
        {


            string _RouteName = "";
            double _OnesideDist = 0;
            int _RouteType = 0, _OnesideTime = 0, _routeId;

            // Fields for Table tbl_tr_route
            _RouteName = txt_route_name.Text.ToUpper();
            _RouteType = int.Parse(drp_route_type.SelectedValue);

            if (!RouteNAmeExists(_RouteName,0))
            {
                int cnt = Grd_destins.Rows.Count - 1;
                _OnesideDist = double.Parse(Grd_destins.Rows[cnt].Cells[3].Text.ToString());
                _OnesideTime = int.Parse(Grd_destins.Rows[cnt].Cells[4].Text.ToString());
                insert_route(_RouteName, _RouteType, _OnesideDist, _OnesideTime);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Route Management", "Route:" + _RouteName + " details added", 1);
                _routeId = getLastrouteID();
                if (insert_route_destinations(_routeId))
                {
                    MPE_RouteCreated.Show();
                    ClearAll();
                }
                if (chk_AddMore.Checked)
                    ClearAll();
                else
                    LoadInitialDeails();
            }
            else
            {
                Lbl_msg.Text = "Route Name Already Exists!";
                MPE_MessageBox.Show();
            }
            
        }

        private int getLastrouteID()
        {
            int _RouteId = 0;

            string sql = "SELECT Max(tbl_tr_route.Id) FROM tbl_tr_route";

            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out _RouteId);
            }
            return _RouteId;
        }

        private bool insert_route_destinations(int _routeId)
        {
            int _order = 0, _DestinationId = 0, _SchoolTime = 0;
            double _SchoolDist = 0;
            bool success = false;
            foreach (GridViewRow gv in Grd_destins.Rows)
            {
                _DestinationId = int.Parse(gv.Cells[0].Text.ToString());
                _SchoolDist = double.Parse(gv.Cells[3].Text.ToString());
                _SchoolTime = int.Parse(gv.Cells[4].Text.ToString());
                _order = int.Parse(gv.Cells[1].Text.ToString());
                insert_route_destinations_db(_routeId, _DestinationId, _SchoolDist, _SchoolTime, _order);
                success = true;
            }
            return success;
            

        }

        private void insert_route(string _RouteName, int _RouteType, double _OnesideDist, int _OnesideTime)
        {
            try
            {
                string sql = "INSERT into tbl_tr_route(RouteName,RouteTypeId,OneSideDistance,OneSideTime) VALUES ('" + _RouteName + "','" + _RouteType + "','" + _OnesideDist + "','" + _OnesideTime + "')";

                MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            }
            catch (Exception e)
            {
                Lbl_msg.Text = e.ToString();
                MPE_MessageBox.Show();
            }

        }

       

        protected void btn_cncl_route_Click(object sender, EventArgs e)
        {
            ClearAll();
            LoadInitialDeails();
        }

        protected void txt_dest_time_TextChanged(object sender, EventArgs e)
        {

        }

        private bool RouteNAmeExists(string _RouteName, int id)
        {
            bool _Exists = false;
            string sql = "SELECT tbl_tr_route.RouteName from tbl_tr_route where tbl_tr_route.RouteName='" + _RouteName + "' and tbl_tr_route.id != "+id+"";
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _Exists = true;
            }
            return _Exists;
        }

        private bool RouteDestinationExists(int id)
        {
            bool _Exists = false;

            if (Grd_destins.Rows.Count > 0)
            {

                foreach (GridViewRow gv in Grd_destins.Rows)
                {
                    if (id == int.Parse(gv.Cells[0].Text.ToString()))
                    {
                        _Exists = true;
                        break;
                    }
                }
                
            }

            return _Exists;

        }

        private void ClearAll()
        {
            txt_route_name.Text = "";
            txt_dest_dist.Text = " ";
            txt_dest_time.Text = "";
            fill_drp_destins();
            Grd_destins.DataSource = null;
            Grd_destins.DataBind();
            grd_route_destins.Columns[5].Visible = false;
            grd_route_destins.Columns[6].Visible = false;
            Pnl_destin_list.Visible = false;

        }




        protected void Grd_Route_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Route.PageIndex = e.NewPageIndex;
            // FillItemGrid("", int.Parse(Drp_Categories.SelectedValue));
            if (ViewState["RouteDataSet"] != null)
            {
                DataSet _pageDS = (DataSet)ViewState["RouteDataSet"];
                if (_pageDS.Tables[0].Rows.Count > 0)
                {
                    Grd_Route.Columns[0].Visible = true;
                    DataTable dtpageData = _pageDS.Tables[0];

                    DataView dataView = new DataView(dtpageData);

                    if (Session["SortDirection1"] != null && Session["SortExpression1"] != null)
                    {
                        dataView.Sort = (string)Session["SortExpression1"] + " " + (string)Session["SortDirection1"];
                    }

                    Grd_Route.DataSource = dataView;
                    Grd_Route.DataBind();
                    Grd_Route.Columns[0].Visible = false;

                }
                else
                {
                    Lbl_msg.Text = "No More Routes";
                }
            }

        }

        protected void Grd_Route_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Grd_Items.PageIndex = 0;
            //FillItemGrid("", int.Parse(Drp_Categories.SelectedValue));
            if (ViewState["RouteDataSet"] != null)
            {
                DataSet _sortlDS = (DataSet)ViewState["RouteDataSet"];

                if (_sortlDS.Tables[0].Rows.Count > 0)
                {
                    DataTable dtData = _sortlDS.Tables[0];

                    DataView dataView = new DataView(dtData);

                    dataView.Sort = e.SortExpression + " " + GetSortDirection1(e.SortExpression);
                    Grd_Route.Columns[0].Visible = true;
                    Grd_Route.DataSource = dataView;
                    Grd_Route.DataBind();
                    Grd_Route.Columns[0].Visible = false;
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
        
        protected void Btn_EditCancel_Click(object sender, EventArgs e)
        {
            LoadInitialDeails();
        }

        protected void Grd_Route_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id;
            id = int.Parse(Grd_Route.SelectedRow.Cells[0].Text.ToString());
            fillRouteDestinationGrid(id);
           RouteDetails(id);

        }





        protected void lnk_newDestin_Click(object sender, EventArgs e)
        {
            NewDestinClear();
            pnl_newDestin.Visible = true;
            txt_newDestinDistanc.Text = "";
            txt_newDestinTime.Text = "";
        }

        private void NewDestinClear()
        {

            int id = int.Parse(Grd_Route.SelectedRow.Cells[0].Text.ToString());
            fill_NewDrp_destins(id);
            fill_AfterDrp_destins(id);
            pnl_newDestin.Visible = false;
            txt_newDestinDistanc.Text = "";
            txt_newDestinTime.Text = "";
        }

        protected void btn_cnclDestin_click(object sender, EventArgs e)
        {
            NewDestinClear();
        }

        private void fill_NewDrp_destins(int RouteId)
        {
            
            drp_newDestin.Items.Clear();
            MyReader = MyTransMang.getDestinsNotinRoute(RouteId);
            if (MyReader.HasRows)
            {
                ListItem li;
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    drp_newDestin.Items.Add(li);

                }

            }
            else
            {
                ListItem li = new ListItem("No Destinations Found", "-1");
                drp_newDestin.Items.Add(li);

            }
        }
        
        private void fill_AfterDrp_destins(int RouteId)
        {
            drp_afterDestin.Items.Clear();
            MyReader = MyTransMang.getDestinsInRoute(RouteId);
            if (MyReader.HasRows)
            {
                ListItem li;
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    drp_afterDestin.Items.Add(li);

                }

            }
            else
            {
                ListItem li = new ListItem("No Destinations Found", "-1");
                drp_afterDestin.Items.Add(li);

            }
        }

        protected void btn_newDestin_Click(object sender, EventArgs e)
        {

            grd_route_destins.Columns[0].Visible = true;
            grd_route_destins.Columns[3].Visible = true;
            grd_route_destins.Columns[4].Visible = true;
            grd_route_destins.Columns[5].Visible = true;
            grd_route_destins.Columns[6].Visible = true;
            string msg = "";
            string newDestId = drp_newDestin.SelectedValue;
            string afterdestId = drp_afterDestin.SelectedValue;
            if (int.Parse(newDestId) > 0)
            {
                int after = 0, newdist = 0, newtime = 0;
                if (radio_after.Checked == true)
                    after = 1;
                int.TryParse(txt_newDestinDistanc.Text, out newdist);
                int.TryParse(txt_newDestinTime.Text, out newtime);

                if (ValidNewDEstinTimeDist(afterdestId, newDestId, after, newdist, newtime, out msg))
                {
                    int id = int.Parse(Grd_Route.SelectedRow.Cells[0].Text.ToString());

                    grd_route_destins.Columns[0].Visible = true;
                    grd_route_destins.DataSource = makeNewDestindataset(afterdestId, newDestId, after, newdist, newtime);
                    grd_route_destins.DataBind();
                    Add_route_destinations(id);
                    grd_route_destins.Columns[0].Visible = false;
                    if (grd_route_destins.Rows.Count != 1)
                    {
                        UpdateRouteTimeDistance(id);
                    }
                    else
                    {
                        AddNewRoureTimeDistance(id);
                    }
                    NewDestinClear();
                }
                else
                {
                    Lbl_msg.Text = msg;
                    MPE_MessageBox.Show();
                }

                grd_route_destins.Columns[0].Visible = false;
                grd_route_destins.Columns[5].Visible = false;
                grd_route_destins.Columns[6].Visible = false;

            }
            else
            {
                Lbl_msg.Text = "No Destination..!";
                MPE_MessageBox.Show();
            }
        }

        private void AddNewRoureTimeDistance(int id)
        {
            MyTransMang.AddRouteTimeDistanc(id);
            ClientScript.RegisterClientScriptBlock(typeof(Page), "AnyScript", "<script>PageRelorad();</script>");

            id = int.Parse(Grd_Route.SelectedRow.Cells[0].Text.ToString());
            fillRouteDestinationGrid(id);
            RouteDetails(id);
        }

        private void UpdateRouteTimeDistance(int id)
        {
            MyTransMang.UpdateRouteTimeDistanc(id);
            ClientScript.RegisterClientScriptBlock(typeof(Page), "AnyScript", "<script>PageRelorad();</script>");

            id = int.Parse(Grd_Route.SelectedRow.Cells[0].Text.ToString());
            fillRouteDestinationGrid(id);
            RouteDetails(id);
        }

        private bool ValidNewDEstinTimeDist(string afterdestId, string newDestId, int after, int newdist, int newtime, out string msg)
        {
            msg = "";
            bool valid = true;

            int beforeTime = 9999, aftertime = 0;
            double beforedist = 9999, afterdist = 0;
            if (after == 1)
            {
                foreach (GridViewRow gv in grd_route_destins.Rows)
                {
                    if (gv.Cells[0].Text == afterdestId)
                    {
                        afterdist = double.Parse(gv.Cells[3].Text.ToString());
                        aftertime = int.Parse(gv.Cells[4].Text.ToString());

                    }
                    else if (afterdist >0)
                    {
                        beforedist = double.Parse(gv.Cells[3].Text.ToString());
                        beforeTime = int.Parse(gv.Cells[4].Text.ToString());

                        break;
                    }

                }
            }
            else
            {
                foreach (GridViewRow gv in grd_route_destins.Rows)
                {
                    if (gv.Cells[0].Text == afterdestId)
                    {
                        
                        beforedist = double.Parse(gv.Cells[3].Text.ToString());
                        beforeTime = int.Parse(gv.Cells[4].Text.ToString());

                        break;
                    }
                    else
                    {
                        afterdist = double.Parse(gv.Cells[3].Text.ToString());
                        aftertime = int.Parse(gv.Cells[4].Text.ToString());
                     }

                }
            }


            if ((newdist > beforedist) || (newdist < afterdist))
            {
                msg = "Distance, ";
                valid = false;
            }
            if ((newtime > beforeTime) || (newtime < aftertime))
            {
                msg += "Time ";
                valid = false;
            }
            if (!valid)
                msg += " Must Be In Increment Order.";

           return valid;
        }

        protected void btn_delDestin_Click(object sender, EventArgs e)
        {
            int RouteId=0,DestinId=0;
            RouteId = int.Parse(Grd_Route.SelectedRow.Cells[0].Text.ToString());
            DestinId = int.Parse(grd_route_destins.SelectedRow.Cells[0].Text.ToString());
           
                    grd_route_destins.Columns[0].Visible = true;
                    grd_route_destins.DataSource = getNewDataset(DestinId);
                    grd_route_destins.DataBind();
                    grd_route_destins.Columns[0].Visible = true;
                    Add_route_destinations(RouteId);
                    UpdateRouteTimeDistance(RouteId);
                    DeleteDestinationFromDb(DestinId);
        }

        private void DeleteDestinationFromDb(int DestinId)
        {
            string sql = "Delete from tbl_tr_destinations where Id=" + DestinId + "";
            MyTransMang.m_MysqlDb.ExecuteQuery(sql);
        }

        private DataSet getNewDataset(int DestinId)
        {
            DataSet MydataSet = new DataSet();
            DataTable dt;
            DataRow dr;
            MydataSet.Tables.Add(new DataTable("Destinations"));
            dt = MydataSet.Tables["Destinations"];
            dt.Columns.Add("D_Order");
            dt.Columns.Add("D_Id");
            dt.Columns.Add("D_Destination");
            dt.Columns.Add("D_Distance");
            dt.Columns.Add("D_Time");
            int ordr = 1, del=1;

            int i = grd_route_destins.Rows.Count;

            if (grd_route_destins.Rows.Count > 0)
            {
                foreach (GridViewRow gv in grd_route_destins.Rows)
                {
                    if (int.Parse(gv.Cells[0].Text.ToString()) != DestinId)
                    {
                        dr = MydataSet.Tables["Destinations"].NewRow();
                        dr["D_Order"] = ordr;
                        dr["D_Id"] = int.Parse(gv.Cells[0].Text.ToString());
                        dr["D_Destination"] = gv.Cells[2].Text;
                        dr["D_Distance"] = double.Parse(gv.Cells[3].Text.ToString());
                        dr["D_Time"] = gv.Cells[4].Text;
                        MydataSet.Tables["Destinations"].Rows.Add(dr);
                        i = int.Parse(gv.Cells[1].Text.ToString());
                        ordr++;
                        del = 0;
                    }
                }
                if (del == 1)
                {
                    MydataSet = null;
                }
            }
            grd_route_destins.DataSource = null;
            grd_route_destins.DataBind();
            return MydataSet;

        }
        
        private DataSet makeNewDestindataset(string afterdestId, string newDestId, int after, int newdist, int newtime)
        {
            DataSet MydataSet = new DataSet();
            DataTable dt;
            DataRow dr;
            MydataSet.Tables.Add(new DataTable("Destinations"));
            dt = MydataSet.Tables["Destinations"];
            dt.Columns.Add("D_Order");
            dt.Columns.Add("D_Id");
            dt.Columns.Add("D_Destination");
            dt.Columns.Add("D_Distance");
            dt.Columns.Add("D_Time");
            int ordr = 1;

            int i = grd_route_destins.Rows.Count;

            if (grd_route_destins.Rows.Count > 0)
            {


                foreach (GridViewRow gv in grd_route_destins.Rows)
                {

                    if (afterdestId == gv.Cells[0].Text.ToString())
                    {

                        if (after == 1)
                        {
                            dr = MydataSet.Tables["Destinations"].NewRow();
                            dr["D_Order"] = ordr;
                            dr["D_Id"] = int.Parse(gv.Cells[0].Text.ToString());
                            dr["D_Destination"] = gv.Cells[2].Text;
                            dr["D_Distance"] = double.Parse(gv.Cells[3].Text.ToString());
                            dr["D_Time"] = gv.Cells[4].Text;
                            MydataSet.Tables["Destinations"].Rows.Add(dr);
                            i = int.Parse(gv.Cells[1].Text.ToString());
                            ordr++;


                            dr = MydataSet.Tables["Destinations"].NewRow();
                            dr["D_Order"] = ordr;
                            dr["D_Id"] = int.Parse(newDestId);
                            dr["D_Destination"] = drp_newDestin.SelectedItem.Text;
                            dr["D_Distance"] = double.Parse(newdist.ToString());
                            dr["D_Time"] = newtime;
                            MydataSet.Tables["Destinations"].Rows.Add(dr);
                            i = int.Parse(gv.Cells[1].Text.ToString());
                            ordr++;
                        }
                        else
                        {
                            dr = MydataSet.Tables["Destinations"].NewRow();
                            dr["D_Order"] = ordr;
                            dr["D_Id"] = int.Parse(newDestId);
                            dr["D_Destination"] = drp_newDestin.SelectedItem.Text;
                            dr["D_Distance"] = double.Parse(newdist.ToString());
                            dr["D_Time"] = newtime;
                            MydataSet.Tables["Destinations"].Rows.Add(dr);
                            i = int.Parse(gv.Cells[1].Text.ToString());
                            ordr++;

                            dr = MydataSet.Tables["Destinations"].NewRow();
                            dr["D_Order"] = ordr;
                            dr["D_Id"] = int.Parse(gv.Cells[0].Text.ToString());
                            dr["D_Destination"] = gv.Cells[2].Text;
                            dr["D_Distance"] = double.Parse(gv.Cells[3].Text.ToString());
                            dr["D_Time"] = gv.Cells[4].Text;
                            MydataSet.Tables["Destinations"].Rows.Add(dr);
                            i = int.Parse(gv.Cells[1].Text.ToString());
                            ordr++;
                        }
                    }
                    else
                    {
                        dr = MydataSet.Tables["Destinations"].NewRow();
                        dr["D_Order"] = ordr;
                        dr["D_Id"] = int.Parse(gv.Cells[0].Text.ToString());
                        dr["D_Destination"] = gv.Cells[2].Text;
                        dr["D_Distance"] = double.Parse(gv.Cells[3].Text.ToString());
                        dr["D_Time"] = gv.Cells[4].Text;
                        MydataSet.Tables["Destinations"].Rows.Add(dr);
                        i = int.Parse(gv.Cells[1].Text.ToString());
                        ordr++;
                    }
                }
            }
            else
            {
                dr = MydataSet.Tables["Destinations"].NewRow();
                dr["D_Order"] = ordr;
                dr["D_Id"] = int.Parse(drp_newDestin.SelectedValue);
                dr["D_Destination"] = drp_newDestin.SelectedItem.Text;
                dr["D_Distance"] =  double.Parse(txt_newDestinDistanc.Text.ToString());
                dr["D_Time"] = txt_newDestinTime.Text;
                MydataSet.Tables["Destinations"].Rows.Add(dr);
                ordr++;
            }

            return MydataSet;


        }

        private bool Add_route_destinations(int _routeId)
        {

            grd_route_destins.Columns[3].Visible = true;
            grd_route_destins.Columns[4].Visible = true;
            string _sql ="";
            int _order = 0, _DestinationId = 0, _SchoolTime = 0;
            double _SchoolDist = 0;
            bool success = false;
            string sql = "Delete from tbl_tr_routedestinations where RouteId=" + _routeId + "";
            MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            _sql = "select tbl_tr_trips.Id FROM tbl_tr_trips where tbl_tr_trips.RouteId=" + _routeId + "";
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(_sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    _sql = "delete from tbl_tr_tripdestinations where TripId="+int.Parse(MyReader.GetValue(0).ToString())+"";
                    MyTransMang.m_MysqlDb.ExecuteQuery(_sql);
                }
            }
            if (grd_route_destins.Rows.Count>0)
            foreach (GridViewRow gv in grd_route_destins.Rows)
            {
                _DestinationId = int.Parse(gv.Cells[0].Text.ToString());
                _SchoolDist = double.Parse(gv.Cells[3].Text.ToString());
                _order = int.Parse(gv.Cells[1].Text.ToString());
                _SchoolTime = int.Parse(gv.Cells[4].Text.ToString());
                insert_route_destinations_db(_routeId, _DestinationId, _SchoolDist, _SchoolTime, _order);
                Insert_Trip_Destination(_routeId, _DestinationId);
                success = true;
            }
          
            return success;


        }

        private void Insert_Trip_Destination(int _routeId,int _DestinationId)
        {

            try
            {
                string sql = "";
                sql = "select tbl_tr_trips.Id FROM tbl_tr_trips where tbl_tr_trips.RouteId=" + _routeId + "";
                MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while(MyReader.Read())
                    {
                        sql = "insert into tbl_tr_tripdestinations(TripId,DestinationId) values(" + int.Parse(MyReader.GetValue(0).ToString()) + "," + _DestinationId + ")";
                        MyTransMang.m_MysqlDb.ExecuteQuery(sql);
                    }
                }
            }
            catch (Exception e)
            {
                Lbl_msg.Text = e.ToString();
                MPE_MessageBox.Show();
            }
           
        }
        private void insert_route_destinations_db(int _routeId, int _DestinationId, double _SchoolDist, int _SchoolTime, int _order)
        {
            try
            {
                string sql = "INSERT into tbl_tr_routedestinations(RouteId,DestinationId,Time,Distance,DestinationOrder) VALUES ('" + _routeId + "','" + _DestinationId + "','" + _SchoolTime + "','" + _SchoolDist + "','" + _order + "')";

                MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            }
            catch (Exception e)
            {
                Lbl_msg.Text = e.ToString();
                MPE_MessageBox.Show();
            }
        }

        private void fillRouteDestinationGrid(int id)
        {
            
            string sql = "SELECT  `tbl_tr_routedestinations`.`DestinationId` AS `D_Id`,  `tbl_tr_routedestinations`.`DestinationOrder` AS `D_Order`,  `tbl_tr_destinations`.`Destination` AS `D_Destination`,  `tbl_tr_routedestinations`.`Distance` AS `D_Distance`,  `tbl_tr_routedestinations`.`Time` AS `D_Time`FROM  `tbl_tr_routedestinations` INNER JOIN  `tbl_tr_destinations` ON `tbl_tr_routedestinations`.`DestinationId` =    `tbl_tr_destinations`.`Id`WHERE  `tbl_tr_routedestinations`.`RouteId` = '"+ id +"' ORDER BY  `tbl_tr_routedestinations`.`DestinationOrder`";
            MyDataSet = MyTransMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ViewState["RouteDestins"] = MyDataSet;
            grd_route_destins.Columns[0].Visible = true;
            grd_route_destins.Columns[3].Visible = true;
            grd_route_destins.Columns[4].Visible = true;
            grd_route_destins.Columns[5].Visible = true;
            grd_route_destins.Columns[6].Visible = true;

            grd_route_destins.DataSource = MyDataSet;
            grd_route_destins.DataBind();

            grd_route_destins.Columns[0].Visible = false;
            grd_route_destins.Columns[5].Visible = false;
            grd_route_destins.Columns[6].Visible = false;

        }

        protected void grd_Destins_RowDeleting(object sender, EventArgs e)
        {
            int RouteId = 0, DestinId = 0;
            RouteId = int.Parse(Grd_Route.SelectedRow.Cells[0].Text.ToString());
            DestinId = int.Parse(grd_route_destins.SelectedRow.Cells[0].Text.ToString());
            if (MyTransMang.CanRemoveDestin(RouteId, DestinId))
            {
                mpe_delConfirmDestin.Show();
            }
            else
            {
                Lbl_msg.Text = "This destination is assigned to some students,Can't delete.!";
                MPE_MessageBox.Show();
            }
           
        }

        private void RouteDetails(int id)
        {
            img_Add.Visible = false;
            lnk_add_route.Visible = false;
            pnl_add_route.Visible = false;
            Pnl_RouteList.Visible = false;
            pnl_route_dtls.Visible = true;

            lbl_RouteName.Visible = true;
            lbl_RouteType.Visible = false;
            rowEdittype.Visible = false;
            lbl_time.Visible = true;
            lbl_dist.Visible = true;
            lbl_trips.Visible = true;
            lbl_vehicles.Visible = true;
            txt_RouteNameE.Visible = false;
            drp_RouteTypeE.Visible = false;

            btn_edit_route.Visible = true;
            btn_update_route.Visible = false;

            MyReader = null;

            DetailsPanelInitial(id);
        }

        private void DetailsPanelInitial(int id)
        {

            //0-id,1-name,2-distance,3-time,4-type,5-typeid
            int distance = 0;
            int time = 0;

            //if (MyDataSet != null && MyDataSet.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow dr in MyDataSet.Tables[0].Rows)
            //    {
            //        distance = distance + int.Parse(dr["D_Distance"].ToString());
            //        time = time + int.Parse(dr["D_Time"].ToString());
            //    }
            //    lbl_dist.Text = " :       " + distance.ToString();
            //    lbl_time.Text = " :       " + time.ToString();
            //}
            MyReader= getRoutedetails(id);
            if (MyReader.HasRows)
            {
                lbl_RouteName.Text = " :       "+MyReader.GetValue(1).ToString();
                lbl_dist.Text = " :       "+MyReader.GetValue(2).ToString();
                lbl_RouteType.Text =" :       " +MyReader.GetValue(4).ToString();
                lbl_time.Text = " :       "+MyReader.GetValue(3).ToString();
                lbl_vehicles.Text =" : "+ Grd_Route.Rows[Grd_Route.SelectedIndex].Cells[5].Text.ToString();

                lbl_trips.Text = " : " + Grd_Route.Rows[Grd_Route.SelectedIndex].Cells[6].Text.ToString();
                txt_RouteNameE.Text = MyReader.GetValue(1).ToString();
            }
            UpdateTripTimenDist();
        }

        private OdbcDataReader getRoutedetails(int id)
        {
            string sql = "SELECT  `tbl_tr_route`.`Id` AS `Id`, `tbl_tr_route`.`RouteName` AS `Route Name`,  `tbl_tr_route`.`OneSideDistance` AS `Distance`, `tbl_tr_route`.`OneSideTime`  AS `Time`, `tbl_tr_routetype`.`Type` AS `Type`,`tbl_tr_routetype`.`Id` FROM  `tbl_tr_route` INNER JOIN  `tbl_tr_routetype` ON `tbl_tr_route`.`RouteTypeId` = `tbl_tr_routetype`.`Id` WHERE tbl_tr_route.Id= '"+ id + "'";
            //0-id,1-name,2-distance,3-time,4-type,5-typeid
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            return MyReader;
        }

        private void LoadRouteTypeToDropDown(int _TypeId)
        {
            drp_RouteTypeE.Items.Clear();

            string sql = "SELECT  `tbl_tr_routetype`.`Type`, `tbl_tr_routetype`.`Id`FROM  `tbl_tr_routetype` ORDER BY  `tbl_tr_routetype`.`Id`"; // where `tbl_tr_routetype`.`Id`!=1
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    drp_RouteTypeE.Items.Add(li);
                }
                drp_RouteTypeE.SelectedValue = _TypeId.ToString();
            }
            else
            {
                ListItem li = new ListItem("No Routes Found", "-1");
                drp_RouteTypeE.Items.Add(li);
            }
        }

        protected void Btn_EditCancel_Click1(object sender, EventArgs e)
        {
            LoadInitialDeails();
        }

        protected void btn_edit_route_Click(object sender, EventArgs e)
        {

           
            grd_route_destins.Columns[5].Visible = true;
            grd_route_destins.Columns[6].Visible = true;

            lbl_RouteName.Visible = false;
            lbl_RouteType.Visible = false;
            rowEdittype.Visible = false;

            txt_RouteNameE.Visible = true;
            drp_RouteTypeE.Visible = true;

            btn_update_route.Visible = true;
            btn_edit_route.Visible = false;


            int _TypeId = 0;
            int id;
            id = int.Parse(Grd_Route.SelectedRow.Cells[0].Text.ToString());

            string sql = "SELECT  `tbl_tr_route`.`RouteTypeId`FROM  `tbl_tr_route`WHERE  `tbl_tr_route`.`Id` ="+id;
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out _TypeId);
            }
            LoadRouteTypeToDropDown(_TypeId);


            TextBox txt_time = new TextBox();
            TextBox txt_dist = new TextBox();

            if (grd_route_destins.Rows.Count > 0)
            {
                foreach (GridViewRow gv in grd_route_destins.Rows)
                {
                    txt_dist = (TextBox)gv.FindControl("txt_newdestindist");
                    txt_time = (TextBox)gv.FindControl("txt_newdesttime");

                    txt_dist.Text = gv.Cells[3].Text;
                    txt_time.Text = gv.Cells[4].Text;
                }
            }
            rowEdittype.Visible = false;
            grd_route_destins.Columns[3].Visible = false;
            grd_route_destins.Columns[4].Visible = false;
        }

        protected void btn_update_route_Click(object sender, EventArgs e)
        {
            string sql = "", _RouteNameE = "";
            int _RouteTypeE = 0;
            int id;
            id = int.Parse(Grd_Route.SelectedRow.Cells[0].Text.ToString());
            string msg = "";
            _RouteNameE = txt_RouteNameE.Text.ToString();
            _RouteTypeE = int.Parse(drp_RouteTypeE.SelectedValue);
            if (ValidForUpdatingRoute(_RouteNameE,id,out msg))
            {

                    sql = "UPDATE tbl_tr_route SET tbl_tr_route.RouteName='" + _RouteNameE + "', tbl_tr_route.RouteTypeId='" + _RouteTypeE + "' where tbl_tr_route.Id='"+ id +"'";
                    MyTransMang.m_MysqlDb.ExecuteQuery(sql);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Route Management", "Updated route:" + _RouteNameE + "  details", 1);
                    ReBuildGridWithNeData();
                    Add_route_destinations(id);
                    Lbl_msg.Text = "Route Details Updated!";
                    MPE_MessageBox.Show();
                
                    RouteDetails(id);
                    fillRouteDestinationGrid(id);
                    UpdateRouteTimeDistance(id);
            }
            else
            {
                Lbl_msg.Text = msg;
                MPE_MessageBox.Show();
            }

        }

        private void ReBuildGridWithNeData()
        {

            TextBox txt_time = new TextBox();
            TextBox txt_dist = new TextBox();

            if (grd_route_destins.Rows.Count > 0)
            {
                foreach (GridViewRow gv in grd_route_destins.Rows)
                {
                    txt_dist = (TextBox)gv.FindControl("txt_newdestindist");
                    txt_time = (TextBox)gv.FindControl("txt_newdesttime");

                    gv.Cells[3].Text = txt_dist.Text;
                    gv.Cells[4].Text = txt_time.Text;
                }
            }

            grd_route_destins.Columns[3].Visible = true;
            grd_route_destins.Columns[4].Visible = true;
            grd_route_destins.Columns[5].Visible = false;
            grd_route_destins.Columns[6].Visible = false;
        }

        private bool ValidForUpdatingRoute(string _RouteNameE,int id, out string msg)
        {
           msg = "";
           bool valid=true;
           if (RouteNAmeExists(_RouteNameE,id))
           {
               msg = "Route Already Exists!";
               valid = false;
           }
           if (!ValidTimeAndDDistancInGrid())
           {
               msg = "Time and Distance Must be in Ascending Order";
               valid = false;
           }
           return valid;
        }

        private bool ValidTimeAndDDistancInGrid()
        {
            bool valid = true;
            int aftertime = 0, prevtime = 0;
            double afterdist = 0, prevdist = 0;
            TextBox txt_time = new TextBox();
            TextBox txt_dist = new TextBox();
            if (grd_route_destins.Rows.Count > 0)
            {
                foreach (GridViewRow gv in grd_route_destins.Rows)
                {
                    txt_dist = (TextBox)gv.FindControl("txt_newdestindist");
                    txt_time = (TextBox)gv.FindControl("txt_newdesttime");

                    aftertime = int.Parse(txt_time.Text);
                    afterdist = double.Parse(txt_dist.Text);

                    if ((aftertime < prevtime) || (afterdist < prevdist))
                    {
                        valid = false;
                        break;
                    }
                    else
                    {
                        prevtime = int.Parse(txt_time.Text);
                        prevdist = double.Parse(txt_dist.Text);
                    }
                }
            }

            return valid;
        }

        protected void Btn_Delete_Click(object sender, EventArgs e)
        {
            int id;
            id = int.Parse(Grd_Route.SelectedRow.Cells[0].Text.ToString());
            if (!RouteNotAssign(id))
            {
                MPE_DeleteConfirm.Show();

            }
            else
            {
                Lbl_msg.Text = "Route -> Trip -> Students Mapping Exists. Can't Delete.";
                MPE_MessageBox.Show();
            }
        }

        protected void Btn_DeleteYes_Click(object sender, EventArgs e)
        {
            int id;
            id = int.Parse(Grd_Route.SelectedRow.Cells[0].Text.ToString());
            MyTransMang.CreateTansationDb();
            try
            {
                MyTransMang.DeleteRoute(id);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Route Management", "Route details deleted", 1);
                Lbl_msg.Text = "Route Details Deleted!";
                MyTransMang.EndSucessTansationDb();
            }
            catch
            {
                Lbl_msg.Text = "Error while deleting route..";
                MyTransMang.EndFailTansationDb();
            }
            MPE_MessageBox.Show();
            LoadInitialDeails();
        }

        private bool RouteNotAssign(int id)
        {

            bool _Exists = false;

            string sql = "select tbl_tr_studtripmap.StudId  from tbl_tr_studtripmap inner join tbl_tr_trips on tbl_tr_trips.Id = tbl_tr_studtripmap.FromTripId WHERE  tbl_tr_trips.RouteId = "+id+"";
            MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _Exists = true;
            }
            else
            {
                sql = "select tbl_tr_studtripmap.StudId  from tbl_tr_studtripmap inner join tbl_tr_trips on tbl_tr_trips.Id = tbl_tr_studtripmap.ToTripId WHERE  tbl_tr_trips.RouteId = 4";
                MyReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    _Exists = true;
                }
            }
                return _Exists;
            
        }

        protected void lnk_newd_Click(object sender, EventArgs e)
        {
            MPE_newDestin.Show();
        }

        protected void Btn_Add_new_Place_Click1(object sender, EventArgs e)
        {
            Lbl_MsgCreateCategory.Text = "";
            string _Destination = "", _ErrorMsg = "";
            _Destination = txt_newd.Text.Trim().ToUpper();

            if (_Destination != "")
            {
                if (SaveNewDestination(_Destination, out _ErrorMsg))
                {

                    int id = int.Parse(Grd_Route.SelectedRow.Cells[0].Text.ToString());
                    fill_NewDrp_destins(id);
                    txt_newd.Text = "";
                }
                else
                {
                    Label2.Text = _ErrorMsg;
                    MPE_newDestin.Show();
                }
            }
            else
            {

                MPE_MessageBox_AddNewPlace.Show();
                Lbl_MsgCreateCategory.Text = "Enter Category Name";
                txt_new_place.Text = "";
            }
        }

        private void UpdateTripTimenDist()
        {
            int id = int.Parse(Grd_Route.SelectedRow.Cells[0].Text.ToString());
            int _IndTime = 0;
            double dist = 0;
            TimeSpan _StartTime = new TimeSpan();
            
            int.TryParse(lbl_time.Text.Replace(':',' ').Trim(), out _IndTime);
            double.TryParse(lbl_dist.Text.Replace(':', ' ').Trim(), out dist);
            MyTransMang.UpdateTripTimeDist(id, GetDestTime(_IndTime),dist);
        }

        private string GetTimeFromDataFromSchool(TimeSpan _StartTime, int _IndTime)
        {
            string _DestinationTime = "";
            TimeSpan _DestTime = new TimeSpan();
            _DestTime = GetDestTime(_IndTime);
            _DestinationTime = (_StartTime + _DestTime).ToString();
            return _DestinationTime;
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

        
    }

}