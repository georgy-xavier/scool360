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
using WinBase;
using System.Drawing;
using System.Data.Odbc;
namespace WinEr
{
    public partial class ReportStaffAttendance : System.Web.UI.Page
    {
        private Attendance MyAttendance;
        private DataSet MydataSet;
        private StaffManager MyStaffMang;
        private KnowinUser MyUser;
        public MysqlClass MysqlDb;
        private OdbcDataReader MyReader = null;
        string Sql;
        private DataSet holidaydataset;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStaffMang = MyUser.GetStaffObj();
            MyAttendance = MyUser.GetAttendancetObj();
            if ((MyAttendance == null) || (MyStaffMang == null))
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }

            else if (!MyUser.HaveActionRignt(81))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    string _MenuStr;
                    _MenuStr = MyStaffMang.GetStaffMangMenuString(MyUser.UserRoleId);
                    this.StaffMenu.InnerHtml = _MenuStr;
                    _MenuStr = MyUser.GetDetailsString(81);
                    this.ActionInfo.InnerHtml = _MenuStr;
                    string _DATEToday = MyAttendance.gettodaydate();
                    DateTime _Todatdate = DateTime.Today;
                    // btn_Edit.Enabled = false;
                    FillTheDrpBox();
                    btn_Edit.Visible = false;
                    //Btn_UpdateAttandance.Enabled = false;
                    //LoadDetailToGridBox();
                    //changeColorAttandanceMarkedDay();
                    // Cal_DateEntry.SelectedDayStyle.BackColor = Color.Cyan;
                    CheckSunday(_DATEToday, _Todatdate);
                  //  holidaydataset = MyUser.MyAssociatedHolidays("staff",0);

                }
            }
        }

       

        private void CheckSunday(string _DATEToday, DateTime _Todatdate)
        {
            try
            {

                if ((int.Parse(Drp_selectdays.SelectedValue) == 1) && (_DATEToday != "Sunday"))
                {
                    string _typ = "staff";
                    int _clsId = 0;

                    if (!MyAttendance.CheckAttadanceStatus(_Todatdate, _typ, _clsId))
                    {

                        CheckHolidayDaily();
                        LoadDetailToGridBox();
                        Btn_UpdateAttandance.Enabled = true;
                    }
                    else
                    {
                        //Lbl_msg.Text = "Staff Attandence Is Already Marked";
                        //this.MPE_MessageBox.Show();
                        Btn_UpdateAttandance.Enabled = false;
                        btn_Edit.Visible = true;
                        LoadDetailToGridBox();
                        StaffAttandanceEdit(_Todatdate);
                    }

                }
                else
                {
                    Lbl_msg.Text = "Selected Day is SUNDAY. Select another day";
                    this.MPE_MessageBox.Show();
                }
                Pnl_Weekly.Visible = false;
            }
            catch (Exception)
            {
                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();

            }
        }


        //funtion used to changer the color if the attandance is masked  in the calander::used in isposebackfuntion,
        private void changeColorAttandanceMarkedDay()
        {
            try
            {
                Sql = "select tblmasterdate.`date`from tblmasterdate inner join tbldate on tbldate.DateId= tblmasterdate.Id where tbldate.`Status`='staff' or tbldate.`Status`='all'";
                MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(Sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        DateTime _dt = DateTime.Parse(MyReader.GetValue(0).ToString());
                        Cal_DateEntry.SelectedDates.Add(_dt.Date);
                        Cal_DateEntry.SelectedDayStyle.BackColor = Color.Cyan;

                    }
                    // Cal_DateEntry.SelectedDayStyle.BackColor = Color.CornflowerBlue;
                }
            }
            catch (Exception)
            {
                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();

            }


        }

        private void FillTheDrpBox()
        {
            try
            {
                Sql = "select tblattendancemode.Id,tblattendancemode.Attmode from tblattendancemode";
                MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(Sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_selectdays.Items.Add(li);

                    }
                }
            }
            catch (Exception)
            {
                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();

            }
        }

        //To Select The Different eg:Daily And Weekly Mode from DropDownList//

        protected void selectDifferMode(object sender, EventArgs e)
        {
            try
            {
                btn_Edit.Visible = false;
                string _DATEToday = MyAttendance.gettodaydate();
                DateTime _Todatdate;
                // DateTime _Todatdate = DateTime.Today;
                DateTime str;
                str = DateTime.Parse("1/1/0001 12:00:00 AM");

                if (Cal_DateEntry.SelectedDate == str)
                {
                    _Todatdate = DateTime.Now;
                }
                else
                {

                    _Todatdate = (Cal_DateEntry.SelectedDate);
                }


                Lbl_EndDate.Visible = false;
                Lbl_from.Visible = false;
                Lbl_startDate.Visible = false;
                Lbl_to.Visible = false;

                // Btn_UpdateAttandance.Enabled = true;
                if (int.Parse(Drp_selectdays.SelectedValue) == 1)
                {

                    Pnl_Weekly.Visible = false;
                    Pnl_Daily.Visible = true;
                    Cal_DateEntry.SelectedDate = DateTime.Today;
                    if (_DATEToday != "Sunday")
                    {
                        string _typ = "staff";
                        int _clsId = 0;
                        LoadDetailToGridBox();
                        if (!MyAttendance.CheckAttadanceStatus(_Todatdate, _typ, _clsId))
                        {
                            Btn_UpdateAttandance.Enabled = true;
                            CheckHolidayDaily();

                            Btn_UpdateAttandance.Enabled = false;
                        }
                        else
                        {
                            StaffAttandanceEdit(_Todatdate);
                            btn_Edit.Visible = true;
                            Btn_UpdateAttandance.Enabled = false;
                        }
                    }
                    else
                    {
                        Lbl_msg.Text = "SUNDAY. Select other day";
                        this.MPE_MessageBox.Show();
                        btn_Edit.Visible = false;
                    }

                }

                else if (int.Parse(Drp_selectdays.SelectedValue) == 2)
                {
                    Pnl_Daily.Visible = false;
                    Pnl_Weekly.Visible = true;
                    Lbl_EndDate.Visible = false;
                    Lbl_from.Visible = false;
                    Lbl_startDate.Visible = false;
                    Lbl_to.Visible = false;
                    Grd_SelectedWeek.Visible = true;
                    DateTime _date, _Dt_DOB;
                    Grd_StaffAttandence.Visible = true;
                    if (DateTime.TryParse(Txt_statrDate.Text, out _Dt_DOB))
                    {

                        _date = _Dt_DOB;
                    }
                    else
                    {
                        _date = DateTime.Today;
                    }


                    TofilltheGrdSeleWeekByCheck();
                    Btn_UpdateAttandance.Enabled = false;
                    btn_Edit.Enabled = true;
                    DateTime _day;
                    DateTime _ToDaysDate;
                    if (DateTime.TryParse(Txt_statrDate.Text, out _day) == true)
                    {
                        _ToDaysDate = _day;
                        Lbl_startDate.Text = _ToDaysDate.ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        _ToDaysDate = DateTime.Now;
                        Lbl_startDate.Text = _ToDaysDate.ToString("dd-MMM-yyyy");

                    }
                    if (Btn_UpdateAttandance.Enabled == false)
                    {
                        btn_Edit.Visible = true;
                        TofilltheGrdSeleWeekByCheck();
                        WeeklyStaffAttendanceEdit(_ToDaysDate);
                    }


                    Lbl_EndDate.Text = _date.AddDays(5).ToString("dd-MM-yyyy");


                }
                else
                {
                    Lbl_msg.Text = "SUNDAY Select other day";
                    this.MPE_MessageBox.Show();
                    btn_Edit.Visible = false;
                }
            }
            catch (Exception)
            {
                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();

            }
        }
        //This Funtion is used to Get the Details of absent and Present Staff for a Week//
        private void WeeklyStaffAttendanceEdit(DateTime _ToDaysDate)
        {

            try
            {
                DateTime _rentryOFdate;
                _rentryOFdate = _ToDaysDate;
                int _StaffIdFrmGrid;



                foreach (GridViewRow gv in Grd_SelectedWeek.Rows)
                {
                    _ToDaysDate = _rentryOFdate;
                    _StaffIdFrmGrid = int.Parse(gv.Cells[0].Text.ToString());

                    if (MyAttendance.checkOfattendance(_ToDaysDate, _StaffIdFrmGrid) == true)
                    {
                        CheckBox chksatAtt1 = (CheckBox)gv.FindControl("Chk_status_mon");
                        chksatAtt1.Checked = false;

                    }
                    _ToDaysDate = _ToDaysDate.AddDays(1);
                    if (MyAttendance.checkOfattendance(_ToDaysDate, _StaffIdFrmGrid) == true)
                    {
                        CheckBox chksatAtt2 = (CheckBox)gv.FindControl("Chk_status_tu");
                        chksatAtt2.Checked = false;
                    }
                    _ToDaysDate = _ToDaysDate.AddDays(1);

                    if (MyAttendance.checkOfattendance(_ToDaysDate, _StaffIdFrmGrid) == true)
                    {
                        CheckBox chksatAtt3 = (CheckBox)gv.FindControl("Chk_status_wed");
                        chksatAtt3.Checked = false;
                    }
                    _ToDaysDate = _ToDaysDate.AddDays(1);

                    if (MyAttendance.checkOfattendance(_ToDaysDate, _StaffIdFrmGrid) == true)
                    {
                        CheckBox chksatAtt4 = (CheckBox)gv.FindControl("Chk_status_thu");
                        chksatAtt4.Checked = false;
                    }
                    _ToDaysDate = _ToDaysDate.AddDays(1);

                    if (MyAttendance.checkOfattendance(_ToDaysDate, _StaffIdFrmGrid) == true)
                    {
                        CheckBox chksatAtt5 = (CheckBox)gv.FindControl("Chk_status_fri");
                        chksatAtt5.Checked = false;
                    }
                    _ToDaysDate = _ToDaysDate.AddDays(1);

                    if (MyAttendance.checkOfattendance(_ToDaysDate, _StaffIdFrmGrid) == true)
                    {
                        CheckBox chksatAtt6 = (CheckBox)gv.FindControl("Chk_status_sat");
                        chksatAtt6.Checked = false;
                    }

                }
            }
            catch
            {
                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();

            }

        }

        private void LoadDetailToGridBoxweekly()
        {
            try
            {
                Sql = "select tbluser.Id,  tbluser.SurName  from tbluser  inner join tblrole on tblrole.Id= tbluser.RoleId where tblrole.Type='staff' and tbluser.Status=1";
                MydataSet = MyStaffMang.m_MysqlDb.ExecuteQueryReturnDataSet(Sql);
                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    Grd_SelectedWeek.Columns[0].Visible = true;
                    Grd_SelectedWeek.DataSource = MydataSet;
                    Grd_SelectedWeek.DataBind();
                    Grd_SelectedWeek.Columns[0].Visible = false;
                    DisableAttandanceMarked();
                    CheckHoliday();

                }
                else
                {
                    Lbl_msg.Text = "No staffs found";
                    this.MPE_MessageBox.Show();

                }
            }
            catch (Exception)
            {
                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();

            }



        }
        //this funtion is ued to changer the color of the gride view where attence is masked//
        private void DisableAttandanceMarked()
        {
            try
            {
                string _WeekDate = Txt_statrDate.Text;
                DateTime _getdate;
                DateTime _getDateFrmTryParse;
                DateTime _rentryOFdate;
                if (DateTime.TryParse(_WeekDate.ToString(), out _getDateFrmTryParse) == true)
                {

                    _getdate = _getDateFrmTryParse;
                }
                else
                {
                    _getdate = DateTime.Now;
                }

                string _typ = "staff";
                int _clsid = 0;
                int count = 0;
                //int _clsId = int.Parse(Drp_SelectTheClass.SelectedValue);
                _rentryOFdate = _getdate;
                foreach (GridViewRow gv in Grd_SelectedWeek.Rows)
                {
                    count = 0;
                    _getdate = _rentryOFdate;
                    CheckBox chksat = (CheckBox)gv.FindControl("Chk_status_mon");
                    if (MyAttendance.CheckAttadanceStatus(_getdate, _typ, _clsid) == true)
                    {
                        gv.Cells[2].BackColor = Color.Azure;
                        count++;

                    }
                    else
                    {
                        chksat.Enabled = false;
                    }
                    CheckBox chksat1 = (CheckBox)gv.FindControl("Chk_status_tu");
                    _getdate = _getdate.AddDays(1);
                    if (MyAttendance.CheckAttadanceStatus(_getdate, _typ, _clsid) == true)
                    {

                        gv.Cells[3].BackColor = Color.Azure;
                        count++;
                    }
                    else
                    {
                        chksat1.Enabled = false;
                    }

                    CheckBox chksat2 = (CheckBox)gv.FindControl("Chk_status_wed");

                    _getdate = _getdate.AddDays(1);
                    if (MyAttendance.CheckAttadanceStatus(_getdate, _typ, _clsid) == true)
                    {

                        count++;
                        gv.Cells[4].BackColor = Color.Azure;
                    }
                    else
                    {
                        chksat2.Enabled = false;
                    }
                    CheckBox chksat3 = (CheckBox)gv.FindControl("Chk_status_thu");

                    _getdate = _getdate.AddDays(1);

                    if (MyAttendance.CheckAttadanceStatus(_getdate, _typ, _clsid) == true)
                    {
                        // Grd_SelectedWeek.HeaderStyle.BackColor = Color.Red;
                        //chksat3.Enabled = false;
                        count++;
                        gv.Cells[5].BackColor = Color.Azure;
                    }
                    else
                    {
                        chksat3.Enabled = false;
                    }
                    CheckBox chksat4 = (CheckBox)gv.FindControl("Chk_status_fri");
                    _getdate = _getdate.AddDays(1);
                    if (MyAttendance.CheckAttadanceStatus(_getdate, _typ, _clsid) == true)
                    {
                        // Grd_SelectedWeek.HeaderStyle.BackColor = Color.Red;
                        //                    chksat4.Enabled = false;
                        count++;
                        gv.Cells[6].BackColor = Color.Azure;
                    }
                    else
                    {
                        chksat4.Enabled = false;
                    }

                    CheckBox chksat5 = (CheckBox)gv.FindControl("Chk_status_sat");
                    _getdate = _getdate.AddDays(1);
                    if (MyAttendance.CheckAttadanceStatus(_getdate, _typ, _clsid) == true)
                    {
                        // Grd_SelectedWeek.HeaderStyle.BackColor = Color.Red;
                        //  chksat5.Enabled = false;
                        count++;
                        gv.Cells[7].BackColor = Color.Azure;
                    }
                    else
                    {
                        chksat5.Enabled = false;
                    }

                }
                if (count > 0)
                {
                    Btn_UpdateAttandance.Enabled = false;
                    btn_Edit.Visible = true;
                }
                else
                {
                    Btn_UpdateAttandance.Enabled = true;
                    enableAllTheVlaueInTheGrdView();
                    btn_Edit.Visible = false;
                }
            }
            catch (Exception)
            {
                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();

            }

        }

        private void enableAllTheVlaueInTheGrdView()
        {

            foreach (GridViewRow gv in Grd_SelectedWeek.Rows)
            {

                CheckBox chksat = (CheckBox)gv.FindControl("Chk_status_mon");
                chksat.Enabled = true;
                CheckBox chksat1 = (CheckBox)gv.FindControl("Chk_status_tu");
                chksat1.Enabled = true;
                CheckBox chksat2 = (CheckBox)gv.FindControl("Chk_status_wed");
                chksat2.Enabled = true;
                CheckBox chksat3 = (CheckBox)gv.FindControl("Chk_status_thu");
                chksat3.Enabled = true;
                CheckBox chksat4 = (CheckBox)gv.FindControl("Chk_status_fri");
                chksat4.Enabled = true;
                CheckBox chksat5 = (CheckBox)gv.FindControl("Chk_status_sat");
                chksat5.Enabled = true;
            }

        }
        //Checking the Holiday table For Weekly Entry//
        private void CheckHoliday()
        {
            DateTime _CheckDay;
            string _checkTheDay;
            Sql = "select tblmasterdate.`Date` from tblmasterdate inner join tblHoliday on tblHoliday.DateId= tblmasterdate.Id where tblHoliday.Type='staff' or tblHoliday.Type='all'";
            MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    _CheckDay = DateTime.Parse(MyReader.GetValue(0).ToString());
                    _checkTheDay = _CheckDay.ToString("dddd");
                    if (ValidHoliday(_CheckDay) == true)
                    {

                        foreach (GridViewRow gv in Grd_SelectedWeek.Rows)
                        {
                            if (_checkTheDay == "Monday")
                            {
                                CheckBox chksat = (CheckBox)gv.Cells[2].FindControl("Chk_status_mon");
                                chksat.Enabled = false;
                                gv.Cells[2].BackColor = Color.Red;

                            }
                            else if (_checkTheDay == "Tuesday")
                            {
                                CheckBox chksat = (CheckBox)gv.Cells[3].FindControl("Chk_status_tu");
                                chksat.Enabled = false;
                                gv.Cells[3].BackColor = Color.Red;
                            }
                            else if (_checkTheDay == "Wednesday")
                            {
                                CheckBox chksat = (CheckBox)gv.Cells[4].FindControl("Chk_status_wed");
                                chksat.Enabled = false;
                                gv.Cells[4].BackColor = Color.Red;
                            }
                            else if (_checkTheDay == "Thursday")
                            {
                                CheckBox chksat = (CheckBox)gv.Cells[5].FindControl("Chk_status_thu");
                                chksat.Enabled = false;
                                gv.Cells[5].BackColor = Color.Red;
                            }
                            else if (_checkTheDay == "Friday")
                            {
                                CheckBox chksat = (CheckBox)gv.Cells[6].FindControl("Chk_status_fri");
                                chksat.Enabled = false;
                                gv.Cells[6].BackColor = Color.Red;

                            }
                            else if (_checkTheDay == "Saturday")
                            {
                                CheckBox chksat = (CheckBox)gv.Cells[7].FindControl("Chk_status_sat");
                                chksat.Enabled = false;
                                gv.Cells[7].BackColor = Color.Red;
                            }
                            else
                            {
                                Lbl_msg.Text = "SUNDAY Select other day";
                                this.MPE_MessageBox.Show();
                            }
                        }
                    }
                }
            }
        }
        //This Funtion IS used to Put the First Value To the Text Box in the Weekly Panal//
        public bool ValidHoliday(DateTime _CheckDay)
        {
            bool _boolen = false;
            int i = 0;
            DateTime _Dt_DOB;
            DateTime _date = DateTime.Today;

            if (DateTime.TryParse(Txt_statrDate.Text, out _Dt_DOB))
            {

                _date = _Dt_DOB;
            }
            else
            {
                _date = DateTime.Today;
            }


            while (i < 6)
            {
                if (_CheckDay == _date)
                {
                    _boolen = true;

                }

                _date = _date.AddDays(1);
                DateTime dt = _date;
                i++;
            }
            return _boolen;
        }

        private void LoadDetailToGridBox()
        {
            string Sql;
            Sql = "select tbluser.Id,  tbluser.SurName  from tbluser  inner join tblrole on tblrole.Id= tbluser.RoleId where tblrole.Type='staff'";
            MydataSet = MyStaffMang.m_MysqlDb.ExecuteQueryReturnDataSet(Sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_StaffAttandence.Columns[0].Visible = true;
                Grd_StaffAttandence.DataSource = MydataSet;
                Grd_StaffAttandence.DataBind();

                Grd_StaffAttandence.Columns[0].Visible = false;
                CheckHolidayDaily();

            }
            else
            {
                Lbl_msg.Text = "No staffs found";
                this.MPE_MessageBox.Show();

            }
        }
        //to Check the Holiday List For the Daily Entry//
        private void CheckHolidayDaily()
        {
            try
            {
                DateTime _CheckDay, _getDate;

                Sql = "select tblmasterdate.`Date` from tblmasterdate inner join tblHoliday on tblHoliday.DateId= tblmasterdate.Id where tblHoliday.Type='staff' or tblHoliday.Type='all'";
                MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(Sql);
                while (MyReader.Read())
                {
                    if (MyReader.HasRows)
                    {

                        _CheckDay = DateTime.Parse(MyReader.GetValue(0).ToString());
                        _getDate = DateTime.Parse(Cal_DateEntry.SelectedDate.ToString());
                        //string _getstringdate=_getDate.ToString();
                        if (_getDate == null)
                        {
                            _getDate = DateTime.Today;
                        }
                        _getDate = DateTime.Parse(Cal_DateEntry.SelectedDate.ToString());

                        // _checkTheDay = _CheckDay.ToString("dddd");
                        if ((_CheckDay) == (_getDate))
                        {

                            Lbl_msg.Text = _getDate.ToString("MMMM dd yyyy") + " is a Holiday";
                            this.MPE_MessageBox.Show();
                            Btn_UpdateAttandance.Enabled = false;
                            Grd_StaffAttandence.Visible = false;
                            btn_Edit.Enabled = false;
                        }
                    }
                }
            }
            catch
            {
                Lbl_msg.Text = "Please try again";
                this.MPE_MessageBox.Show();
            }
        }
        //This Function Is selecting Different date from the calender
        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                string _date;
                DateTime _getDate;
                Grd_StaffAttandence.Visible = true;
                Btn_UpdateAttandance.Enabled = true;
                _getDate = DateTime.Parse(Cal_DateEntry.SelectedDate.ToString());
                _date = _getDate.ToString("dddd");
                DateTime _today;
                int _batchId = MyUser.CurrentBatchId;
                _today = DateTime.Now;
                if (MyAttendance.checkTheBatchDate(_getDate, _batchId))
                {
                    if (_date != "Sunday")
                    {
                        string _typ = "staff";
                        int _clsId = 0;
                        if (!MyAttendance.CheckAttadanceStatus(_getDate, _typ, _clsId))
                        {
                            btn_Edit.Visible = false;

                            Pnl_Weekly.Visible = false;
                            Pnl_Daily.Visible = true;
                            LoadDetailToGridBox();


                        }
                        else
                        {
                            //Lbl_msg.Text = "Staff Attandence Is Already Marked";
                            //this.MPE_MessageBox.Show();
                            Btn_UpdateAttandance.Enabled = false;
                            LoadDetailToGridBox();
                            StaffAttandanceEdit(_getDate);
                            btn_Edit.Enabled = true;

                        }

                    }
                    else
                    {
                        Lbl_msg.Text = "SUNDAY Select other day";
                        Btn_UpdateAttandance.Enabled = false;
                        this.MPE_MessageBox.Show();
                        Grd_StaffAttandence.Visible = false;
                        btn_Edit.Enabled = false;

                    }
                    if (Btn_UpdateAttandance.Enabled == false)
                    {
                        btn_Edit.Visible = true;
                    }
                }
                else
                {
                    Lbl_msg.Text = "This Is Not A Valied Date.please check the date and try again !";
                    this.MPE_MessageBox.Show();
                    Grd_StaffAttandence.Visible = false;
                    btn_Edit.Visible = false;
                    Btn_UpdateAttandance.Enabled = false;

                }
            }
            catch (Exception)
            {

                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();


            }

        }
        // This Funtion is Used For Txt Change For The weekly entry//
        protected void Txt_statrDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Grd_SelectedWeek.Visible = true;
                Lbl_EndDate.Visible = false;
                Lbl_from.Visible = false;
                Lbl_startDate.Visible = false;
                Lbl_to.Visible = false;
                TofilltheGrdSeleWeekByCheck();
                DateTime _dateFrmTxtBox;
                DateTime _TodaysDateInTxt;
                DateTime _today;
                // DateTime _txtValue;
                //_txtValue = DateTime.Parse(Txt_statrDate.ToString());
                if (DateTime.TryParse(Txt_statrDate.Text, out _dateFrmTxtBox))
                {

                    _TodaysDateInTxt = _dateFrmTxtBox;
                }
                else
                {
                    _TodaysDateInTxt = DateTime.Today;
                }
                _today = DateTime.Now;
                if (_TodaysDateInTxt <= _today)
                {
                    if (Btn_UpdateAttandance.Enabled == false)
                    {
                        btn_Edit.Enabled = true;
                        btn_Edit.Visible = true;
                        TofilltheGrdSeleWeekByCheck();
                        changeColorAttandanceMarkedDay();
                        WeeklyStaffAttendanceEdit(_TodaysDateInTxt);
                    }

                    //WeeklyStaffAttendanceEdit(_TodaysDateInTxt);


                }
                else
                {
                    Lbl_msg.Text = "This Is Not A Valied Date.please check the date and try again !";
                    this.MPE_MessageBox.Show();
                    Grd_SelectedWeek.Visible = false;
                    btn_Edit.Visible = false;
                    Btn_UpdateAttandance.Enabled = false;

                }
            }
            catch (Exception)
            {
                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();

            }
        }

        private void TofilltheGrdSeleWeekByCheck()
        {
            try
            {
                DateTime _date, _Dt_DOB;
                if (DateTime.TryParse(Txt_statrDate.Text, out _Dt_DOB))
                {

                    _date = _Dt_DOB;
                }
                else
                {
                    _date = DateTime.Today;
                }

                string _getDate = _date.ToString("dddd");
                if (_getDate == "Sunday")
                {
                    Lbl_msg.Text = "SUNDAY Select other day";
                    this.MPE_MessageBox.Show();
                    Btn_UpdateAttandance.Enabled = false;
                }
                else if (_getDate == "Monday")
                {
                    Pnl_Daily.Visible = false;
                    Pnl_Weekly.Visible = true;
                    Lbl_EndDate.Visible = true;
                    Lbl_from.Visible = true;
                    Lbl_startDate.Visible = true;
                    Lbl_to.Visible = true;
                    LoadDetailToGridBoxweekly();
                    DateTime _day;
                    DateTime _ToDaysDate;
                    if (DateTime.TryParse(Txt_statrDate.Text, out _day) == true)
                    {
                        Lbl_startDate.Text = _day.ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        _ToDaysDate = DateTime.Now;
                        Lbl_startDate.Text = _ToDaysDate.ToString("dd-MMM-yyyy");

                    }


                    Lbl_EndDate.Text = _date.AddDays(5).ToString("dd-MMM-yyyy");
                    string _tempString = Lbl_EndDate.Text;
                    DateTime _temp = DateTime.Parse(_tempString.ToString());

                    DateTime _today;
                    _today = DateTime.Now;
                    if (Btn_UpdateAttandance.Enabled == true && _temp > _today)
                    {

                        DisablePreDateIntheGrdWeekly(_today);


                    }
                }
                else if (_getDate == "Tuesday")
                {
                    Txt_statrDate.Text = _date.AddDays(-1).ToString("dd-MMM-yyyy");

                    Pnl_Daily.Visible = false;
                    Pnl_Weekly.Visible = true;
                    Lbl_EndDate.Visible = true;
                    Lbl_from.Visible = true;
                    Lbl_startDate.Visible = true;
                    Lbl_to.Visible = true;
                    LoadDetailToGridBoxweekly();
                    DateTime _day;
                    DateTime _ToDaysDate;
                    if (DateTime.TryParse(Txt_statrDate.Text, out _day) == true)
                    {
                        Lbl_startDate.Text = _day.ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        _ToDaysDate = DateTime.Now;
                        Lbl_startDate.Text = _ToDaysDate.ToString("dd-MMM-yyyy");

                    }
                    Lbl_EndDate.Text = _date.AddDays(4).ToString("dd-MMM-yyyy");
                    string _tempString = Lbl_EndDate.Text;
                    DateTime _temp = DateTime.Parse(_tempString.ToString());

                    DateTime _today;
                    _today = DateTime.Now;
                    if (Btn_UpdateAttandance.Enabled == true && _temp > _today)
                    {

                        DisablePreDateIntheGrdWeekly(_today);


                    }


                }
                else if (_getDate == "Wednesday")
                {
                    Txt_statrDate.Text = _date.AddDays(-2).ToString("dd-MMM-yyyy");
                    Pnl_Daily.Visible = false;
                    Pnl_Weekly.Visible = true;
                    Lbl_EndDate.Visible = true;
                    Lbl_from.Visible = true;
                    Lbl_startDate.Visible = true;
                    Lbl_to.Visible = true;
                    LoadDetailToGridBoxweekly();
                    DateTime _day;
                    DateTime _ToDaysDate;
                    if (DateTime.TryParse(Txt_statrDate.Text, out _day) == true)
                    {
                        Lbl_startDate.Text = _day.ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        _ToDaysDate = DateTime.Now;
                        Lbl_startDate.Text = _ToDaysDate.ToString("dd-MMM-yyyy");

                    }
                    Lbl_EndDate.Text = _date.AddDays(3).ToString("dd-MMM-yyyy");
                    string _tempString = Lbl_EndDate.Text;
                    DateTime _temp = DateTime.Parse(_tempString.ToString());

                    DateTime _today;
                    _today = DateTime.Now;
                    if (Btn_UpdateAttandance.Enabled == true && _temp > _today)
                    {

                        DisablePreDateIntheGrdWeekly(_today);


                    }
                }
                else if (_getDate == "Thursday")
                {
                    Txt_statrDate.Text = _date.AddDays(-3).ToString("dd-MMM-yyyy");
                    Pnl_Daily.Visible = false;
                    Pnl_Weekly.Visible = true;
                    Lbl_EndDate.Visible = true;
                    Lbl_from.Visible = true;
                    Lbl_startDate.Visible = true;
                    Lbl_to.Visible = true;
                    LoadDetailToGridBoxweekly();
                    DateTime _day;
                    DateTime _ToDaysDate;
                    if (DateTime.TryParse(Txt_statrDate.Text, out _day) == true)
                    {
                        Lbl_startDate.Text = _day.ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        _ToDaysDate = DateTime.Now;
                        Lbl_startDate.Text = _ToDaysDate.ToString("dd-MMM-yyyy");

                    }
                    Lbl_EndDate.Text = _date.AddDays(2).ToString("dd-MMM-yyyy");
                    string _tempString = Lbl_EndDate.Text;
                    DateTime _temp = DateTime.Parse(_tempString.ToString());

                    DateTime _today;
                    _today = DateTime.Now;
                    if (Btn_UpdateAttandance.Enabled == true && _temp > _today)
                    {

                        DisablePreDateIntheGrdWeekly(_today);


                    }
                }
                else if (_getDate == "Friday")
                {
                    Txt_statrDate.Text = _date.AddDays(-4).ToString("dd-MMM-yyyy");
                    Pnl_Daily.Visible = false;
                    Pnl_Weekly.Visible = true;
                    Lbl_EndDate.Visible = true;
                    Lbl_from.Visible = true;
                    Lbl_startDate.Visible = true;
                    Lbl_to.Visible = true;
                    LoadDetailToGridBoxweekly();
                    DateTime _day;
                    DateTime _ToDaysDate;
                    if (DateTime.TryParse(Txt_statrDate.Text, out _day) == true)
                    {
                        Lbl_startDate.Text = _day.ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        _ToDaysDate = DateTime.Now;
                        Lbl_startDate.Text = _ToDaysDate.ToString("dd-MMM-yyyy");

                    }
                    Lbl_EndDate.Text = _date.AddDays(1).ToString("dd-MMM-yyyy");
                    string _tempString = Lbl_EndDate.Text;
                    DateTime _temp = DateTime.Parse(_tempString.ToString());

                    DateTime _today;
                    _today = DateTime.Now;
                    if (Btn_UpdateAttandance.Enabled == true && _temp > _today)
                    {

                        DisablePreDateIntheGrdWeekly(_today);


                    }
                }
                else if (_getDate == "Saturday")
                {
                    Txt_statrDate.Text = _date.AddDays(-5).ToString("dd-MMM-yyyy");
                    Pnl_Daily.Visible = false;
                    Pnl_Weekly.Visible = true;
                    Lbl_EndDate.Visible = true;
                    Lbl_from.Visible = true;
                    Lbl_startDate.Visible = true;
                    Lbl_to.Visible = true;
                    LoadDetailToGridBoxweekly();
                    DateTime _day;
                    DateTime _ToDaysDate;
                    if (DateTime.TryParse(Txt_statrDate.Text, out _day) == true)
                    {
                        Lbl_startDate.Text = _day.ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        _ToDaysDate = DateTime.Now;
                        Lbl_startDate.Text = _ToDaysDate.ToString("dd-MMM-yyyy");

                    }
                    Lbl_EndDate.Text = _date.ToString("dd-MMM-yyyy");
                }


            }
            catch (Exception)
            {
                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();

            }

        }

        private void DisablePreDateIntheGrdWeekly(DateTime _today)
        {
            try
            {
                string _date;
                _date = _today.ToString("dddd");
                if (_date == "Monday")
                {
                    foreach (GridViewRow gv in Grd_SelectedWeek.Rows)
                    {
                        CheckBox chksat1 = (CheckBox)gv.FindControl("Chk_status_tu");
                        chksat1.Enabled = false;
                        CheckBox chksat2 = (CheckBox)gv.FindControl("Chk_status_wed");
                        chksat2.Enabled = false;
                        CheckBox chksat3 = (CheckBox)gv.FindControl("Chk_status_thu");
                        chksat3.Enabled = false;
                        CheckBox chksat4 = (CheckBox)gv.FindControl("Chk_status_fri");
                        chksat4.Enabled = false;
                        CheckBox chksat5 = (CheckBox)gv.FindControl("Chk_status_sat");
                        chksat5.Enabled = false;
                    }

                }
                else if (_date == "Tuesday")
                {
                    foreach (GridViewRow gv in Grd_SelectedWeek.Rows)
                    {


                        CheckBox chksat2 = (CheckBox)gv.FindControl("Chk_status_wed");
                        chksat2.Enabled = false;
                        CheckBox chksat3 = (CheckBox)gv.FindControl("Chk_status_thu");
                        chksat3.Enabled = false;
                        CheckBox chksat4 = (CheckBox)gv.FindControl("Chk_status_fri");
                        chksat4.Enabled = false;
                        CheckBox chksat5 = (CheckBox)gv.FindControl("Chk_status_sat");
                        chksat5.Enabled = false;
                    }

                }
                else if (_date == "Wednesday")
                {
                    foreach (GridViewRow gv in Grd_SelectedWeek.Rows)
                    {


                        CheckBox chksat3 = (CheckBox)gv.FindControl("Chk_status_thu");
                        chksat3.Enabled = false;
                        CheckBox chksat4 = (CheckBox)gv.FindControl("Chk_status_fri");
                        chksat4.Enabled = false;
                        CheckBox chksat5 = (CheckBox)gv.FindControl("Chk_status_sat");
                        chksat5.Enabled = false;
                    }

                }
                else if (_date == "Thursday")
                {
                    foreach (GridViewRow gv in Grd_SelectedWeek.Rows)
                    {
                        CheckBox chksat4 = (CheckBox)gv.FindControl("Chk_status_fri");
                        chksat4.Enabled = false;
                        CheckBox chksat5 = (CheckBox)gv.FindControl("Chk_status_sat");
                        chksat5.Enabled = false;
                    }

                }
                else if (_date == "Friday")
                {
                    foreach (GridViewRow gv in Grd_SelectedWeek.Rows)
                    {

                        CheckBox chksat5 = (CheckBox)gv.FindControl("Chk_status_sat");
                        chksat5.Enabled = true;
                    }

                }
            }
            catch (Exception)
            {
                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();

            }
        }

        protected void Btn_UpdateAttandance_Click(object sender, EventArgs e)
        {
            int _batchId = MyUser.CurrentBatchId;
            string message;
            try
            {
                int _UserID = 0;
                _UserID = MyUser.UserId;

                if (Grd_StaffAttandence.Visible == true)
                {

                    int _count = 0;

                    DateTime _date;
                    string _DateConvertion;

                    _DateConvertion = (Cal_DateEntry.SelectedDate.ToString());

                    if (_DateConvertion == "1/1/0001 12:00:00 AM")
                    {
                        _date = DateTime.Parse(_DateConvertion.ToString());
                        _date = DateTime.Today;
                    }
                    else
                    {
                        _date = DateTime.Parse(_DateConvertion.ToString());
                    }
                    int _redateID = MyAttendance.getDateIdFrmMastTbl(_date);
                    string _typ = "staff";
                    int _clsId = 0;

                    if (MyAttendance.CheckForPrevioudDay(_batchId, _date, _clsId, _typ, out message) == true)
                    {
                        int _dateId = MyAttendance.insertToDateTable(_redateID, _typ, _clsId);
                        foreach (GridViewRow gv in Grd_StaffAttandence.Rows)
                        {
                            CheckBox chksat = (CheckBox)gv.FindControl("Chk_status_daily");
                            if (chksat.Checked == false)
                            {
                                int _StaffId = int.Parse(gv.Cells[0].Text.ToString());

                                MyAttendance.UpadetheStfAtceTbl(_StaffId, _dateId);

                                _count++;
                            }
                            else
                            {
                                int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                                MyAttendance.UpdateIncedentTbl(_StaffId, _dateId, _date, _UserID);
                            }

                        }
                        Lbl_msg.Text = _count + " Staff is Absent For the day " + _date.ToString("MMM-dd-yyyy");
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "save Staff Attendance", "Save Staff Attendance for the day" + _date.Day, 1);
                        this.MPE_MessageBox.Show();
                        Btn_UpdateAttandance.Enabled = false;
                    }
                    else
                    {
                        Lbl_msg.Text = message;
                        this.MPE_MessageBox.Show();
                    }




                }
                else
                {
                    string _WeekDate = Lbl_startDate.Text;
                    int _dateId;
                    DateTime _getdate = DateTime.Parse(_WeekDate.ToString());
                    DateTime _rentryOFdate = _getdate;
                    string _typ = "staff";
                    int _clsId = 0;

                    if (MyAttendance.CheckForPrevioudDay(_batchId, _getdate, _clsId, _typ, out message) == true)
                    {
                        if (MyAttendance.CheckAttadanceStatus(_getdate, _typ, _clsId) == false)
                        {

                            MyAttendance.insertToDateTableFrmMonToSat(_getdate, _clsId, _typ);
                            Btn_UpdateAttandance.Enabled = false;

                            foreach (GridViewRow gv in Grd_SelectedWeek.Rows)
                            {
                                _getdate = _rentryOFdate;
                                CheckBox chksat = (CheckBox)gv.FindControl("Chk_status_mon");
                                if (chksat.Checked == false)
                                {

                                    _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                    int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                                    MyAttendance.UpadetheStfAtceTbl(_StaffId, _dateId);
                                    MyAttendance.UpdateIncedentTbl(_StaffId, _dateId, _getdate, _UserID);
                                }
                                else
                                {
                                    _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                    int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                                    MyAttendance.UpdateIncedentTbl(_StaffId, _dateId, _getdate, _UserID);
                                }
                                CheckBox chksat1 = (CheckBox)gv.FindControl("Chk_status_tu");
                                _getdate = _getdate.AddDays(1);
                                if (chksat1.Checked == false)
                                {

                                    _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                    int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                                    MyAttendance.UpadetheStfAtceTbl(_StaffId, _dateId);
                                    MyAttendance.UpdateIncedentTbl(_StaffId, _dateId, _getdate, _UserID);

                                }
                                else
                                {
                                    _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                    int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                                    MyAttendance.UpdateIncedentTbl(_StaffId, _dateId, _getdate, _UserID);
                                }
                                CheckBox chksat2 = (CheckBox)gv.FindControl("Chk_status_wed");
                                _getdate = _getdate.AddDays(1);
                                if (chksat2.Checked == false)
                                {

                                    _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                    int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                                    MyAttendance.UpadetheStfAtceTbl(_StaffId, _dateId);
                                    MyAttendance.UpdateIncedentTbl(_StaffId, _dateId, _getdate, _UserID);

                                }
                                else
                                {
                                    _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                    int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                                    MyAttendance.UpdateIncedentTbl(_StaffId, _dateId, _getdate, _UserID);
                                }
                                CheckBox chksat3 = (CheckBox)gv.FindControl("Chk_status_thu");
                                _getdate = _getdate.AddDays(1);
                                if (chksat3.Checked == false)
                                {

                                    _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                    int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                                    MyAttendance.UpadetheStfAtceTbl(_StaffId, _dateId);
                                    MyAttendance.UpdateIncedentTbl(_StaffId, _dateId, _getdate, _UserID);

                                }
                                else
                                {
                                    _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                    int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                                    MyAttendance.UpdateIncedentTbl(_StaffId, _dateId, _getdate, _UserID);
                                }
                                CheckBox chksat4 = (CheckBox)gv.FindControl("Chk_status_fri");
                                _getdate = _getdate.AddDays(1);
                                if (chksat4.Checked == false)
                                {

                                    _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                    int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                                    MyAttendance.UpadetheStfAtceTbl(_StaffId, _dateId);
                                    MyAttendance.UpdateIncedentTbl(_StaffId, _dateId, _getdate, _UserID);

                                }
                                else
                                {
                                    _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                    int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                                    MyAttendance.UpdateIncedentTbl(_StaffId, _dateId, _getdate, _UserID);
                                }
                                CheckBox chksat5 = (CheckBox)gv.FindControl("Chk_status_sat");
                                _getdate = _getdate.AddDays(1);
                                if (chksat5.Checked == false)
                                {

                                    _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                    int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                                    MyAttendance.UpadetheStfAtceTbl(_StaffId, _dateId);
                                    MyAttendance.UpdateIncedentTbl(_StaffId, _dateId, _getdate, _UserID);

                                }
                                else
                                {
                                    _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                    int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                                    MyAttendance.UpdateIncedentTbl(_StaffId, _dateId, _getdate, _UserID);
                                }


                                //  _dateId = MyAttendance.insertToDateTable(_getdate);


                            }
                            Lbl_msg.Text = " Staff Attendance have been marked for the  day " + Lbl_startDate.Text + " To " + Lbl_EndDate.Text;
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Save Staff Attendance", "Save Staff Attendance details from " + Lbl_startDate.Text + " to " + Lbl_EndDate.Text, 1);
                            this.MPE_MessageBox.Show();
                            Btn_UpdateAttandance.Enabled = false;


                        }
                        else
                        {
                            Lbl_msg.Text = " Attendance is already Marked ";
                            this.MPE_MessageBox.Show();
                            Btn_UpdateAttandance.Enabled = false;


                        }
                    }
                    else
                    {
                        Lbl_msg.Text = message;
                        this.MPE_MessageBox.Show();
                    }


                }
            }
            catch (Exception)
            {
                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();

            }



        }
        // funtion is used to edit the daily attandance :: used in calander changefunction in the else case,
        private void StaffAttandanceEdit(DateTime _dateId)
        {
            try
            {

                int _StaffIdFrmTbl = 0;
                int _StaffIdFrmGrid = 0;
                Sql = "select  tblstaffattendance.StaffId from   tblstaffattendance inner join tbldate on tbldate.Id = tblstaffattendance.DayId inner join tblmasterdate on tblmasterdate.Id= tbldate.DateId where tblmasterdate.`date`='" + _dateId.ToString("yyyy-MM-dd") + "'";
                MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(Sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        _StaffIdFrmTbl = int.Parse(MyReader.GetValue(0).ToString());
                        //int _temp = 0;
                        foreach (GridViewRow gv in Grd_StaffAttandence.Rows)
                        {
                            //_temp = 0;


                            _StaffIdFrmGrid = int.Parse(gv.Cells[0].Text.ToString());
                            if (_StaffIdFrmGrid == _StaffIdFrmTbl)
                            {

                                CheckBox chksatAtt = (CheckBox)gv.FindControl("Chk_status_daily");

                                chksatAtt.Checked = false;

                                break;
                            }

                        }

                    }

                }
                else
                {
                    LoadDetailToGridBox();
                }

            }
            catch (Exception)
            {
                Lbl_msg.Text = "Please Try again";
                this.MPE_MessageBox.Show();

            }

        }


        protected void Btn_EditAttandance_Click(object sender, EventArgs e)
        {
            try
            {
                //int _count = 0;

                if (Grd_StaffAttandence.Visible == true)
                {
                    DateTime _date;
                    string _DateConvertion;
                    _DateConvertion = (Cal_DateEntry.SelectedDate.ToString());

                    if (_DateConvertion == "1/1/0001 12:00:00 AM")
                    {
                        _date = DateTime.Parse(_DateConvertion.ToString());
                        _date = DateTime.Today;
                    }
                    else
                    {
                        _date = DateTime.Parse(_DateConvertion.ToString());
                    }
                    int _redateID = MyAttendance.getDateIdFrmMastTbl(_date);
                    string _typ = "staff";
                    int _clsId = 0;
                    int _dateId = MyAttendance.getDateIdFrmDateTable(_redateID, _typ, _clsId);
                    foreach (GridViewRow gv in Grd_StaffAttandence.Rows)
                    {
                        CheckBox chksat = (CheckBox)gv.FindControl("Chk_status_daily");
                        if (chksat.Checked == false)
                        {
                            int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                            if (!MyAttendance.FindIfTheStaffIsPresentInthetblstaffAttandence(_StaffId, _dateId))
                            {
                                MyAttendance.UpadetheStfAtceTbl(_StaffId, _dateId);

                            }

                        }
                        else
                        {
                            int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                            if (MyAttendance.FindIfTheStaffIsPresentInthetblstaffAttandence(_StaffId, _dateId) == true)
                            {
                                MyAttendance.DeletetheStfAtceTbl(_StaffId, _dateId);

                            }

                        }
                        btn_Edit.Visible = true;

                    }
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Update Staff Attendance", "Update Staff Attendance for the day" + _date.Day, 1);
                }
                else
                {

                    DateTime _getdate;
                    string _WeekDate = Txt_statrDate.Text;
                    DateTime _getDateFrmTryParse;
                    DateTime _rentryOFdate;
                    int _dateId;
                    string _typ = "staff";
                    if (DateTime.TryParse(_WeekDate.ToString(), out _getDateFrmTryParse) == true)
                    {

                        _getdate = _getDateFrmTryParse;
                    }
                    else
                    {
                        _getdate = DateTime.Now;
                    }

                    _rentryOFdate = _getdate;

                    foreach (GridViewRow gv in Grd_SelectedWeek.Rows)
                    {
                        _getdate = _rentryOFdate;
                        CheckBox chksat = (CheckBox)gv.FindControl("Chk_status_mon");
                        if (chksat.Checked == false)
                        {

                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                            if (!MyAttendance.FindIfTheStaffIsPresentInthetblstaffAttandence(_StaffId, _dateId))
                            {
                                MyAttendance.UpadetheStfAtceTbl(_StaffId, _dateId);

                            }

                        }
                        else
                        {
                            int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            if (MyAttendance.FindIfTheStaffIsPresentInthetblstaffAttandence(_StaffId, _dateId) == true)
                            {
                                MyAttendance.DeletetheStfAtceTbl(_StaffId, _dateId);

                            }


                        }
                        CheckBox chksat1 = (CheckBox)gv.FindControl("Chk_status_tu");

                        if (chksat1.Checked == false)
                        {
                            _getdate = _getdate.AddDays(1);
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            int _StaffId = int.Parse(gv.Cells[0].Text.ToString());

                            if (!MyAttendance.FindIfTheStaffIsPresentInthetblstaffAttandence(_StaffId, _dateId))
                            {
                                MyAttendance.UpadetheStfAtceTbl(_StaffId, _dateId);
                            }
                        }
                        else
                        {
                            _getdate = _getdate.AddDays(1);
                            int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            if (MyAttendance.FindIfTheStaffIsPresentInthetblstaffAttandence(_StaffId, _dateId) == true)
                            {
                                MyAttendance.DeletetheStfAtceTbl(_StaffId, _dateId);

                            }


                        }
                        CheckBox chksat2 = (CheckBox)gv.FindControl("Chk_status_wed");
                        if (chksat2.Checked == false)
                        {
                            _getdate = _getdate.AddDays(1);
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                            if (!MyAttendance.FindIfTheStaffIsPresentInthetblstaffAttandence(_StaffId, _dateId))
                            {
                                MyAttendance.UpadetheStfAtceTbl(_StaffId, _dateId);
                            }
                        }
                        else
                        {
                            _getdate = _getdate.AddDays(1);
                            int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            if (MyAttendance.FindIfTheStaffIsPresentInthetblstaffAttandence(_StaffId, _dateId) == true)
                            {
                                MyAttendance.DeletetheStfAtceTbl(_StaffId, _dateId);

                            }


                        }
                        CheckBox chksat3 = (CheckBox)gv.FindControl("Chk_status_thu");
                        if (chksat3.Checked == false)
                        {
                            _getdate = _getdate.AddDays(1);
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                            if (!MyAttendance.FindIfTheStaffIsPresentInthetblstaffAttandence(_StaffId, _dateId))
                            {
                                MyAttendance.UpadetheStfAtceTbl(_StaffId, _dateId);
                            }
                        }
                        else
                        {
                            _getdate = _getdate.AddDays(1);
                            int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            if (MyAttendance.FindIfTheStaffIsPresentInthetblstaffAttandence(_StaffId, _dateId) == true)
                            {
                                MyAttendance.DeletetheStfAtceTbl(_StaffId, _dateId);

                            }


                        }
                        CheckBox chksat4 = (CheckBox)gv.FindControl("Chk_status_fri");
                        if (chksat4.Checked == false)
                        {
                            _getdate = _getdate.AddDays(1);
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                            if (!MyAttendance.FindIfTheStaffIsPresentInthetblstaffAttandence(_StaffId, _dateId))
                            {
                                MyAttendance.UpadetheStfAtceTbl(_StaffId, _dateId);
                            }
                        }
                        else
                        {
                            _getdate = _getdate.AddDays(1);
                            int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            if (MyAttendance.FindIfTheStaffIsPresentInthetblstaffAttandence(_StaffId, _dateId) == true)
                            {
                                MyAttendance.DeletetheStfAtceTbl(_StaffId, _dateId);

                            }


                        }
                        CheckBox chksat5 = (CheckBox)gv.FindControl("Chk_status_sat");
                        if (chksat5.Checked == false)
                        {
                            _getdate = _getdate.AddDays(1);
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                            if (!MyAttendance.FindIfTheStaffIsPresentInthetblstaffAttandence(_StaffId, _dateId))
                            {
                                MyAttendance.UpadetheStfAtceTbl(_StaffId, _dateId);
                            }

                        }
                        else
                        {
                            _getdate = _getdate.AddDays(1);
                            int _StaffId = int.Parse(gv.Cells[0].Text.ToString());
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            if (MyAttendance.FindIfTheStaffIsPresentInthetblstaffAttandence(_StaffId, _dateId) == true)
                            {
                                MyAttendance.DeletetheStfAtceTbl(_StaffId, _dateId);

                            }


                        }

                    }
                    btn_Edit.Visible = true;


                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Update Staff Attendance", "Update Staff Attendance details from " + Lbl_startDate.Text + " to " + Lbl_EndDate.Text, 1);
                }


            }
            catch (Exception)
            {
                Lbl_msg.Text = " Please Try Again  ";
                this.MPE_MessageBox.Show();

            }


        }

        protected void Cal_DateEntry_DayRender(object sender, DayRenderEventArgs e)
        {
                      
            DateTime nextholidayDate;
            if (holidaydataset != null)
            {
                foreach (DataRow dr in holidaydataset.Tables[0].Rows)
                {
                   
                    nextholidayDate = (DateTime)dr[0];
                    if (nextholidayDate == e.Day.Date)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Orange;
                    //    Label lbl = new Label();
                    //    lbl.Text = "<br>HoliDay!";
                    //    e.Cell.Controls.Add(lbl);
                    }
                   
                }
            }

          

        }

        protected void Cal_DateEntry_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
           // holidaydataset = MyUser.MyAssociatedHolidays("staff",0);

        }
    }
}
    

