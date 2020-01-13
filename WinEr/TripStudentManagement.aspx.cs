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
    public partial class TripStudentManagement : System.Web.UI.Page
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
            else if (!MyUser.HaveActionRignt(202))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadInitials();
                }

            }
        }

        #region Search Student

        private void LoadInitials()
        {
            Lnk_AddNewItem.Text = "ADD NEW STUDENT";
            Pnl_NewStudent.Visible = false;
            Pnl_SearchArea.Visible = true;
            pnl_editstud.Visible = false;
            img_Save.Visible = false;
            img_cancel.Visible = false;
            LoadDestinations();
            LoadTrips();
            LoadVehicle();
            LoadSearchClass();
            grd_studList.DataSource = null;
            grd_studList.DataBind();
            Grd_StudentTrips.DataSource = null;
            Grd_StudentTrips.DataBind();

            Img_Export.Visible = false;
            lbl_count.Visible = false;
        }

        private void LoadSearchClass()
        {
            Drp_ClassSelect.Items.Clear();
            MyDataSet = MyUser.MyAssociatedClass();
            if (MyDataSet != null && MyDataSet.Tables[0] != null && MyDataSet.Tables[0].Rows.Count != 0)
            {
                ListItem li = new ListItem("All", "0");
                Drp_ClassSelect.Items.Add(li);

                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {
                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_ClassSelect.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Classes Found", "-1");
                Drp_ClassSelect.Items.Add(li);
            }
        }

        private void LoadVehicle()
        {
            Drp_VehicleSelect.Items.Clear();
            MyReader = MyTransMang.getVehiclesAll();
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("All", "0");
                Drp_VehicleSelect.Items.Add(li);

                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(0).ToString() + "/" + MyReader.GetValue(1).ToString(), MyReader.GetValue(2).ToString());
                    Drp_VehicleSelect.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Vehicles Found", "-1");
                Drp_VehicleSelect.Items.Add(li);
            }
        }

        private void LoadTrips()
        {
            Drp_TripSelect.Items.Clear();
            MyReader = MyTransMang.getTripsAll();
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("All", "-1");
                Drp_TripSelect.Items.Add(li);

                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_TripSelect.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Trips Found", "-1");
                Drp_TripSelect.Items.Add(li);
            }
        }

        private void LoadDestinations()
        {
            Drp_DesinationSelect.Items.Clear();
            Drp_ToDestination.Items.Clear();

            MyDataSet = MyTransMang.getDestinationsAll();

            if (MyDataSet != null && MyDataSet.Tables[0] != null && MyDataSet.Tables[0].Rows.Count != 0)
            {
                ListItem li = new ListItem("All", "-1");
                Drp_DesinationSelect.Items.Add(li);
                li = new ListItem("Select", "0");
                Drp_ToDestination.Items.Add(li);

                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {
                    li = new ListItem(dr[0].ToString(), dr[1].ToString());
                    Drp_DesinationSelect.Items.Add(li);
                    Drp_ToDestination.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Destinations Found", "-1");
                Drp_DesinationSelect.Items.Add(li);
                Drp_ToDestination.Items.Add(li);
            }
        }

        protected void Lnk_AddNewItem_Click(object sender, EventArgs e)
        {
            if (Lnk_AddNewItem.Text == "SEARCH STUDENT")
            {
                LoadInitials();
            }
            else
            {
                Lnk_AddNewItem.Text = "SEARCH STUDENT";
                Pnl_NewStudent.Visible = true;
                Pnl_SearchArea.Visible = false;
                pnl_editstud.Visible = false;
                LoadAddNewClassDrp();
            }
        }

        protected void Btn_Load_Click(object sender, EventArgs e)
        {
            FillSearchStudGrid();
        }

        private void FillSearchStudGrid()
        {
            Lbl_studtripnote.Visible = true;
            Img_Export.Visible = true;
            lbl_count.Visible = true;
            Grd_StudentTrips.Columns[9].Visible = true;
            Grd_StudentTrips.Columns[6].Visible = true;
            Grd_StudentTrips.Columns[7].Visible = true;

            MyDataSet = null;
            Lbl_studtripnote.Text = "";
            int classid = 0, vehicleid = -1, tripid = -1, destinationid = -1;

            int.TryParse(Drp_ClassSelect.SelectedValue, out classid);
            int.TryParse(Drp_VehicleSelect.SelectedValue, out vehicleid);
            int.TryParse(Drp_TripSelect.SelectedValue, out tripid);
            int.TryParse(Drp_DesinationSelect.SelectedValue, out destinationid);

            MyDataSet = MyTransMang.getStudentsTrips(classid, vehicleid, tripid, destinationid,MyUser.CurrentBatchId);
            MyDataSet = getStudTripInfo(MyDataSet);
            Grd_StudentTrips.DataSource = MyDataSet;
            Grd_StudentTrips.DataBind();
            int count = MyDataSet.Tables[0].Rows.Count;
            if (count == 0)
            {
                Img_Export.Visible = false;
                lbl_count.Visible = false;
                Lbl_studtripnote.Text = "No students Found";
                Lbl_studtripnote.Visible = true;
            }
            else
            {
                lbl_count.Text = count + " Students Found";
            }
            Grd_StudentTrips.Columns[9].Visible = false;
            Grd_StudentTrips.Columns[6].Visible = false;
            Grd_StudentTrips.Columns[7].Visible = false;
        }

        private DataSet getStudTripInfo(DataSet studTripDS)
        {
            int destinid = 0, fromschoolId = 0, toschoolId = 0, time = 0;
            string destin = "None", fromschool = "None", toschool = "None", fromtime = "-", ToTime = "-";
            string starttime = "", endtime = "";
            DataTable dt;
            DataRow dr;
            dt = studTripDS.Tables[0];
            dt.Columns.Add("Destination");
            dt.Columns.Add("ToSchool");
            dt.Columns.Add("ToTrip Destination Time");
            dt.Columns.Add("FromSchool");
            dt.Columns.Add("FromTrip Destination Time");

            foreach (DataRow dro in studTripDS.Tables[0].Rows)
            {
                destinid = int.Parse(dro[6].ToString());
                fromschoolId = int.Parse(dro[8].ToString());
                toschoolId = int.Parse(dro[7].ToString());
                if (destinid > 0)
                {
                    destin = MyTransMang.GetDestination(destinid);
                    if (fromschoolId > 0)
                    {
                        fromschool = MyTransMang.GetTripName(fromschoolId);
                        MyTransMang.GetDestinTime(destinid, fromschoolId, out starttime, out endtime, out time);
                        fromtime = GetTimeFromDataFromSchool(starttime, time);
                    }
                    if (toschoolId > 0)
                    {
                        toschool = MyTransMang.GetTripName(toschoolId);
                        MyTransMang.GetDestinTime(destinid, toschoolId, out starttime, out endtime, out time);
                        ToTime = GetTimeFromDataFromSchool(endtime, time);
                    }
                }


                dro["Destination"] = destin;
                dro["ToSchool"] = toschool;
                dro["ToTrip Destination Time"] = ToTime;
                dro["FromSchool"] = fromschool;
                dro["FromTrip Destination Time"] = fromtime;
                destin = "None"; fromschool = "None"; toschool = "None"; fromtime = "-"; ToTime = "-";
            }
            ViewState["VehicleDataSet"] = studTripDS;
            return studTripDS;
        }

        private string GetTimeFromDataToSchool(string EndTime, int _IndTime)
        {
            TimeSpan _EndTime = TimeSpan.Parse(EndTime);
            string _DestinationTime = "";
            TimeSpan _DestTime = new TimeSpan();
            _DestTime = GetDestTime(_IndTime);
            _DestinationTime = (_EndTime - _DestTime).ToString();

            return GetformatedTime(_DestinationTime);
        }

        private string GetTimeFromDataFromSchool(string StartTime, int _IndTime)
        {
            string _DestinationTime = "";
            if (StartTime != "")
            {
                TimeSpan _StartTime = TimeSpan.Parse(StartTime);

                TimeSpan _DestTime = new TimeSpan();
                _DestTime = GetDestTime(_IndTime);
                _DestinationTime = (_StartTime + _DestTime).ToString();
            }
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

        private string GetformatedTime(string _EndTime)
        {

            string time = "";
            string[] _TimeArray = null;
            int _Day, _Month, _Year, _Hour, _Minute;
            string _AM_PM = "AM";

            if (_EndTime != "")
            {
                _TimeArray = _EndTime.Split(':');// store hh:mm AM/PM
            }


            if (_TimeArray != null)
                _Hour = int.Parse(_TimeArray[0]);// hour
            _Hour = 0;
            if (_TimeArray != null)
                _Minute = int.Parse(_TimeArray[1]);
            _Minute = 0;

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

            time = _Hour + ":" + _Minute + " " + _AM_PM;
            return time;
        }





        protected void Grd_StudentTrips_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            Grd_StudentTrips.Columns[9].Visible = true;
            Grd_StudentTrips.Columns[6].Visible = true;
            Grd_StudentTrips.Columns[7].Visible = true;
            Grd_StudentTrips.PageIndex = e.NewPageIndex;
            // FillItemGrid("", int.Parse(Drp_Categories.SelectedValue));
            if (ViewState["VehicleDataSet"] != null)
            {
                DataSet _pageDS = (DataSet)ViewState["VehicleDataSet"];
                if (_pageDS.Tables[0].Rows.Count > 0)
                {
                    Grd_StudentTrips.Columns[0].Visible = true;
                    DataTable dtpageData = _pageDS.Tables[0];

                    DataView dataView = new DataView(dtpageData);

                    if (Session["SortDirection1"] != null && Session["SortExpression1"] != null)
                    {
                        dataView.Sort = (string)Session["SortExpression1"] + " " + (string)Session["SortDirection1"];
                    }

                    Grd_StudentTrips.DataSource = dataView;
                    Grd_StudentTrips.DataBind();
                    Grd_StudentTrips.Columns[9].Visible = false;
                    Grd_StudentTrips.Columns[6].Visible = false;
                    Grd_StudentTrips.Columns[7].Visible = false;

                }
                else
                {
                }
            }
        }


        #endregion

        #region Edit Student-Trip Map

        protected void Grd_StudentTrips_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillEditArea();
        }

        private void FillEditArea()
        {

            Pnl_SearchArea.Visible = false;
            pnl_editstud.Visible = true;
            btn_update.Visible = false;

            btn_update.Visible = false;
            btn_edit.Visible = true;

            lbl_student.Text = Grd_StudentTrips.SelectedRow.Cells[0].Text;
            lbl_class.Text = Grd_StudentTrips.SelectedRow.Cells[1].Text;
            FillEditDestinDrp();

            for (int i = 0; i < drp_destinationedit.Items.Count; i++)
            {
                if (drp_destinationedit.Items[i].Text == Grd_StudentTrips.SelectedRow.Cells[5].Text)
                {
                    drp_destinationedit.SelectedIndex = i;
                    break;
                }
            }

            FillEditFromTripDrp();

            for (int i = 0; i < drp_fromtripedit.Items.Count; i++)
            {
                if (drp_fromtripedit.Items[i].Text == Grd_StudentTrips.SelectedRow.Cells[7].Text)
                {
                    drp_fromtripedit.SelectedIndex = i;
                    break;
                }
            }

            FillEditToTripDrp();
            for (int i = 0; i < drp_totripedit.Items.Count; i++)
            {
                if (drp_totripedit.Items[i].Text == Grd_StudentTrips.SelectedRow.Cells[6].Text)
                {
                    drp_totripedit.SelectedIndex = i;
                    break;
                }
            }

            drp_destinationedit.Enabled = false;
            drp_fromtripedit.Enabled = false;
            drp_totripedit.Enabled = false;


            lbl_destinID.Text = drp_destinationedit.SelectedValue;
            lbl_fromId.Text = drp_fromtripedit.SelectedValue;
            lbl_toId.Text = drp_totripedit.SelectedValue;
        }

        protected void drp_destinationedit_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillEditFromTripDrp();
            for (int i = 0; i < drp_fromtripedit.Items.Count; i++)
            {
                if (drp_fromtripedit.Items[i].Text == Grd_StudentTrips.SelectedRow.Cells[7].Text)
                {
                    drp_fromtripedit.SelectedIndex = i;
                    break;
                }
            }

            FillEditToTripDrp();
            for (int i = 0; i < drp_totripedit.Items.Count; i++)
            {
                if (drp_totripedit.Items[i].Text == Grd_StudentTrips.SelectedRow.Cells[6].Text)
                {
                    drp_totripedit.SelectedIndex = i;
                    break;
                }
            }
        }

        private void FillEditDestinDrp()
        {

            DropDownList drp = new DropDownList();
            MyDataSet = MyTransMang.getDestinationsAll();
            drp_destinationedit.Items.Clear();

            if (MyDataSet != null && MyDataSet.Tables[0] != null && MyDataSet.Tables[0].Rows.Count != 0)
            {
                ListItem li = new ListItem("Select", "0");
                drp_destinationedit.Items.Add(li);

                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {
                    li = new ListItem(dr[0].ToString(), dr[1].ToString());
                    drp_destinationedit.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("None", "0");
                drp_destinationedit.Items.Add(li);
            }
        }

        private void FillEditToTripDrp()
        {
            int Destination = int.Parse(drp_destinationedit.SelectedValue);

            MyReader = MyTransMang.getScholTrips(Destination, 3);
            drp_totripedit.Items.Clear();
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("Select", "0");
                drp_totripedit.Items.Add(li);

                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    drp_totripedit.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("None", "0");
                drp_totripedit.Items.Add(li);
            }
        }

        private void FillEditFromTripDrp()
        {
            int Destination = int.Parse(drp_destinationedit.SelectedValue);

            MyReader = MyTransMang.getScholTrips(Destination, 2);
            drp_fromtripedit.Items.Clear();
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("Select", "0");
                drp_fromtripedit.Items.Add(li);

                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    drp_fromtripedit.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("None", "0");
                drp_fromtripedit.Items.Add(li);
            }
        }

        protected void btn_edit_Click(object sender, EventArgs e)
        {
            btn_update.Visible = true;
            btn_edit.Visible = false;
            drp_destinationedit.Enabled = true;
            drp_fromtripedit.Enabled = true;
            drp_totripedit.Enabled = true;
        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            int oldDestin = 0, Oldfrom = 0, oldto = 0;
            int newDestin = 0, newFrom = 0, newTo = 0;
            int studId = int.Parse(Grd_StudentTrips.SelectedRow.Cells[9].Text);
            oldDestin = int.Parse(lbl_destinID.Text);
            Oldfrom = int.Parse(lbl_fromId.Text);
            oldto = int.Parse(lbl_toId.Text);

            newDestin = int.Parse(drp_destinationedit.SelectedValue);
            newFrom = int.Parse(drp_fromtripedit.SelectedValue);
            newTo = int.Parse(drp_totripedit.SelectedValue);
            MyTransMang.CreateTansationDb();
            try
            {
                if (newDestin > 0)
                {
                    string message = "";
                    MyTransMang.UpdateStudTripMap(studId, Oldfrom, oldto, newDestin, newFrom, newTo, out message);
                    MyTransMang.EndSucessTansationDb();
                    FillSearchStudGrid();
                    FillEditArea();
                    if (message == "")
                    {
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Trip Student Management", "Student:" + Grd_StudentTrips.SelectedRow.Cells[0].Text + " -Trip Mapping is Updated", 1);
                        WC_MessageBox.ShowMssage("Student-Trip Mapping is Updated");
                        btn_update.Visible = false;
                        btn_edit.Visible = true;

                        drp_destinationedit.Enabled = false;
                        drp_fromtripedit.Enabled = false;
                        drp_totripedit.Enabled = false;
                    }
                    else
                    {
                        WC_MessageBox.ShowMssage(message);
                    }
                }
                else
                {
                    WC_MessageBox.ShowMssage("Please select destination");
                    MyTransMang.EndFailTansationDb();
                }
            }
            catch
            {

                WC_MessageBox.ShowMssage("Error While Updating...!");
                MyTransMang.EndFailTansationDb();
            }
        }

        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            int studId = int.Parse(Grd_StudentTrips.SelectedRow.Cells[9].Text);
            int oldDestin = 0, Oldfrom = 0, oldto = 0;
            oldDestin = int.Parse(lbl_destinID.Text);
            Oldfrom = int.Parse(lbl_fromId.Text);
            oldto = int.Parse(lbl_toId.Text);

            MyTransMang.CreateTansationDb();
            try
            {
                MyTransMang.DeleteStudTripMap(studId, Oldfrom, oldto);
                MyTransMang.EndSucessTansationDb();
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Trip Student Management", "Student:" + Grd_StudentTrips.SelectedRow.Cells[0].Text + " Deleted From Trip", 1);
                WC_MessageBox.ShowMssage("Student is Deleted From Trip");
                FillSearchStudGrid();
                pnl_editstud.Visible = false;
                Pnl_SearchArea.Visible = true;
            }
            catch
            {

                WC_MessageBox.ShowMssage("Error While Deleting Student...!");
                MyTransMang.EndFailTansationDb();
            }
        }

        protected void btn_cncl_Click(object sender, EventArgs e)
        {
            Pnl_SearchArea.Visible = true;
            pnl_editstud.Visible = false;
        }

        #endregion

        #region Add New Student

        private void LoadAddNewClassDrp()
        {
            drp_class.Items.Clear();
            MyDataSet = MyUser.MyAssociatedClass();
            if (MyDataSet != null && MyDataSet.Tables[0] != null && MyDataSet.Tables[0].Rows.Count != 0)
            {
                ListItem li = new ListItem("Select Class", "0");
                drp_class.Items.Add(li);

                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {
                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    drp_class.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Classes Found", "-1");
                drp_class.Items.Add(li);
            }
        }

        protected void drp_class_SelectedIndexChanged(object sender, EventArgs e)
        {
            int classid = 0;
            int.TryParse(drp_class.SelectedValue, out classid);
            LoadStudGrid(classid);
        }

        private void LoadStudGrid(int classid)
        {
            lbl_stud.Visible = false;
            img_Save.Visible = false;
            img_cancel.Visible = false;
            grd_studList.Columns[0].Visible = true;
            grd_studList.Columns[5].Visible = true;
            grd_studList.Columns[7].Visible = true;
            grd_studList.Columns[8].Visible = true;
            grd_studList.Columns[6].Visible = true;
            grd_studList.DataSource = null;
            grd_studList.DataBind();
            MyDataSet = MyTransMang.getStudentsFromClass(classid, MyUser.CurrentBatchId);
            grd_studList.DataSource = MyDataSet;
            grd_studList.DataBind();
            grd_studList.Columns[7].Visible = false;
            grd_studList.Columns[8].Visible = false;
            grd_studList.Columns[0].Visible = false;
            grd_studList.Columns[5].Visible = false;
            grd_studList.Columns[6].Visible = false;// Address
            if (grd_studList.Rows.Count > 0)
            {
                img_Save.Visible = true;
                img_cancel.Visible = true;
                LoadDestinationDrpinGrid();
                LoadFromTripDrpinGrid(0);
                LoadToTripDrpinGrid(0);

            }
            else
            {
                lbl_stud.Visible = true;
                lbl_stud.Text = "No students...!";


            }
        }

        private void LoadDestinationDrpinGrid()
        {
            DropDownList drp = new DropDownList();
            MyDataSet = MyTransMang.getDestinationsAll();

            foreach (GridViewRow gv in grd_studList.Rows)
            {

                drp = (DropDownList)gv.FindControl("drp_destination");
                drp.Items.Clear();

                if (MyDataSet != null && MyDataSet.Tables[0] != null && MyDataSet.Tables[0].Rows.Count != 0)
                {
                    ListItem li = new ListItem("None", "0");
                    drp.Items.Add(li);

                    foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                    {
                        li = new ListItem(dr[0].ToString(), dr[1].ToString());
                        drp.Items.Add(li);
                    }
                }
                else
                {
                    ListItem li = new ListItem("None", "0");
                    drp.Items.Add(li);
                }



                ListItem linone = new ListItem("None", "0");

                drp = (DropDownList)gv.FindControl("drp_fromtrips");
                drp.Items.Clear();
                drp.Items.Add(linone);

                drp = (DropDownList)gv.FindControl("drp_totrips");
                drp.Items.Clear();
                drp.Items.Add(linone);

            }

        }

        protected void drp_destination_changed(object sender, EventArgs e)
        {
            var control = sender as DropDownList;
            DropDownList drp = sender as DropDownList;
            int index = (drp.Parent.Parent as GridViewRow).RowIndex;
            LoadFromTripDrpinGrid(index);
            LoadToTripDrpinGrid(index);
        }

        private void LoadFromTripDrpinGrid(int rowindex)
        {
            int Destination = 0;

            foreach (GridViewRow gv in grd_studList.Rows)
            {
                if (gv.RowIndex == rowindex)
                {
                    DropDownList drp = new DropDownList();
                    drp = (DropDownList)gv.FindControl("drp_destination");
                    Destination = int.Parse(drp.SelectedValue);

                    MyReader = MyTransMang.getScholTrips(Destination, 2);
                    drp = (DropDownList)gv.FindControl("drp_fromtrips");
                    drp.Items.Clear();
                    if (MyReader.HasRows)
                    {
                        ListItem li = new ListItem("None", "0");
                        drp.Items.Add(li);

                        while (MyReader.Read())
                        {
                            li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                            drp.Items.Add(li);
                        }
                    }
                    else
                    {
                        ListItem li = new ListItem("None", "0");
                        drp.Items.Add(li);
                    }
                }
            }
        }

        private void LoadToTripDrpinGrid(int rowindex)
        {
            int Destination = 0;

            foreach (GridViewRow gv in grd_studList.Rows)
            {
                if (gv.RowIndex == rowindex)
                {
                    DropDownList drp = new DropDownList();
                    drp = (DropDownList)gv.FindControl("drp_destination");
                    Destination = int.Parse(drp.SelectedValue);

                    MyReader = MyTransMang.getScholTrips(Destination, 3);
                    drp = (DropDownList)gv.FindControl("drp_totrips");
                    drp.Items.Clear();
                    if (MyReader.HasRows)
                    {
                        ListItem li = new ListItem("None", "0");
                        drp.Items.Add(li);

                        while (MyReader.Read())
                        {
                            li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                            drp.Items.Add(li);
                        }
                    }
                    else
                    {
                        ListItem li = new ListItem("None", "0");
                        drp.Items.Add(li);
                    }
                }
            }
        }

        protected void img_cancel_Click(object sender, ImageClickEventArgs e)
        {
            LoadInitials();
        }

        protected void img_Save_Click(object sender, ImageClickEventArgs e)
        {
            int count = 0, destinationid = 0;
            bool success = true;
            int studid = 0, fromtripid = 0, totripid = 0, fromtripcapacity = 0, totripcapacity = 0;
            int.TryParse(Drp_ToDestination.SelectedValue, out destinationid);
            try
            {
                if (destinationid > 0)
                {
                    foreach (GridViewRow gr in grd_studList.Rows)
                    {
                        CheckBox chk = (CheckBox)gr.FindControl("chk_select");
                        if (chk.Checked)
                        {
                            count++;

                            studid = int.Parse(gr.Cells[0].Text);

                            MyTransMang.StudTripMap(studid, destinationid, fromtripid, totripid);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Trip Student Management", "Location is mapped to the Student:" + gr.Cells[3].Text + "", 1);

                            // AddToStudTripMap();                            
                        }
                    }

                    if (count == 0)
                    {
                        WC_MessageBox.ShowMssage("Please select any students");
                        success = false;
                    }
                }
                else
                {
                    WC_MessageBox.ShowMssage("Please select any destination");
                    success = false;

                }

            }

            catch
            {
                success = false;
            }
            if (success)
            {

                WC_MessageBox.ShowMssage("Location is mapped to the selected students");
                int classid = 0;
                int.TryParse(drp_class.SelectedValue, out classid);
                LoadStudGrid(classid);
            }


        }

        private void AddToStudTripMap()
        {
            int studid = 0, destinid = 0, fromtripid = 0, totripid = 0, fromtripcapacity = 0, totripcapacity = 0;
            CheckBox chk = new CheckBox();
            DropDownList drp = new DropDownList();

            foreach (GridViewRow gv in grd_studList.Rows)
            {
                chk = (CheckBox)gv.FindControl("chk_select");
                if (chk.Checked)
                {
                    studid = int.Parse(gv.Cells[0].Text);

                    drp = (DropDownList)gv.FindControl("drp_destination");
                    destinid = int.Parse(drp.SelectedValue);

                    drp = (DropDownList)gv.FindControl("drp_fromtrips");
                    fromtripid = int.Parse(drp.SelectedValue);

                    drp = (DropDownList)gv.FindControl("drp_totrips");
                    totripid = int.Parse(drp.SelectedValue);

                    MyTransMang.StudTripMap(studid, destinid, fromtripid, totripid);
                }
            }
        }

        #endregion

        #region Export to Excel

        protected void Img_Export_Click(object sender, ImageClickEventArgs e)
        {
            DataSet ExportDataSet = (DataSet)ViewState["VehicleDataSet"];
            ExportDataSet = MakeExcelDataset(ExportDataSet);
            ExportDataSet.Tables["Student"].Columns.Remove("To School Trip");
            ExportDataSet.Tables["Student"].Columns.Remove("To School Trip Destination Time");
            ExportDataSet.Tables["Student"].Columns.Remove("From School Trip");
            ExportDataSet.Tables["Student"].Columns.Remove("From School Trip Destination Time");
            string FileName = "Students List";
            string _ReportName = "Transportaion - Students List";
            if (!WinEr.ExcelUtility.ExportDataToExcel(ExportDataSet, _ReportName, FileName, MyUser.ExcelHeader))
            {
                WC_MessageBox.ShowMssage("This function need Ms office");
            }
        }

        private DataSet MakeExcelDataset(DataSet ExportDataSet)
        {

            DataSet ExcelDataset = new DataSet();
            DataTable dt;
            DataRow dr;
            ExcelDataset.Tables.Add(new DataTable("Student"));
            dt = ExcelDataset.Tables["Student"];
            dt.Columns.Add("Student Name");
            dt.Columns.Add("ClassName");
            dt.Columns.Add("RollNo");
            dt.Columns.Add("Address");
            dt.Columns.Add("Destination");
            dt.Columns.Add("To School Trip");
            dt.Columns.Add("To School Trip Destination Time");
            dt.Columns.Add("From School Trip");
            dt.Columns.Add("From School Trip Destination Time");
            foreach (DataRow dro in ExportDataSet.Tables[0].Rows)
            {

                dr = ExcelDataset.Tables["Student"].NewRow();
                dr["Student Name"] = dro["StudentName"].ToString();
                dr["RollNo"] = dro["RollNo"].ToString();
                dr["ClassName"] = dro["ClassName"].ToString();
                dr["Address"] = dro["Address"].ToString();
                dr["Destination"] = dro["Destination"].ToString();
                dr["To School Trip"] = dro["ToSchool"].ToString();
                dr["To School Trip Destination Time"] = dro["ToTrip Destination Time"].ToString();
                dr["From School Trip"] = dro["FromSchool"].ToString();
                dr["From School Trip Destination Time"] = dro["FromTrip Destination Time"].ToString();
                ExcelDataset.Tables["Student"].Rows.Add(dr);
            }
            return ExcelDataset;
        }

        #endregion

        protected void grd_studList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd_studList.PageIndex = e.NewPageIndex;
            int classid = 0;
            int.TryParse(drp_class.SelectedValue, out classid);
            LoadStudGrid(classid);
        }

        protected void Lnk_nontransportaionreport_Click(object sender, EventArgs e)
        {
            Response.Redirect("NonTripStudentManagement.aspx");
        }

    }
}
