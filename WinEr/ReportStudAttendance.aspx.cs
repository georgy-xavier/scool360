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
using System.Data.Odbc;
using System.Drawing;
namespace WinEr
{
    public partial class ReportStudAttendance : System.Web.UI.Page
    {
     
        private ClassOrganiser MyClassMang;
        private Attendance MyAttendance;
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private DataSet MydataSet;
        private OdbcDataReader MyReader = null;
        string Sql;
        public int _ClassId = 0;      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyClassMang = MyUser.GetClassObj();             
            MyAttendance = MyUser.GetAttendancetObj();
            MyStudMang = MyUser.GetStudentObj();
            if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(80))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                

                if (!IsPostBack)
                {
                    daymode.Visible = false;
                    weekmode.Visible = false;
                    weekinput.Visible = false;

                    string _MenuStr;
                    _MenuStr = MyStudMang.GetStudentMangMenuString(MyUser.UserRoleId);
                    this.StudentMenu.InnerHtml = _MenuStr;
                    _MenuStr = MyUser.GetDetailsString(80);
                    this.ActionInfo.InnerHtml = _MenuStr;
                    string _DATEToday = MyAttendance.gettodaydate();
                    DateTime _Todatdate = DateTime.Today;           
                    FillModeDrp();
                    fillClassDrp();
                    Btn_Edit.Visible = false;
                   // loadinivaluetoclasID();
                     string _typ="class";
                     int _clsId = int.Parse(Drp_SelectTheClass.SelectedValue);

                    if (!MyAttendance.CheckAttadanceStatus(_Todatdate,_typ,_clsId))
                    {
                        Pnl_Daily.Visible = true;
                        Pnl_Weekly.Visible = false;                       
                        Btn_UpdateAttandance.Enabled = false;
                        _ClassId = -1;
                        LoadDetailToGridBox();
                    }
                    else
                    {
                        //Lbl_msg.Text = "Todays attendance Have been marked";
                       // this.MPE_MessageBox.Show();
                        Btn_Edit.Visible = true;
                    }
                    Cal_DateEntry.Enabled = false;


                    this.MPE_MessageBox.Hide();
                    
                    //some initlization

                }
            }

        }

        private void loadinivaluetoclasID()
        {
            Sql = "select tblclass.Id from tblclass";
             MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(Sql);
             if (MyReader.HasRows)
             {
                 _ClassId = int.Parse(MyReader.GetValue(0).ToString());
             }

        }

        private void LoadDetailToGridBox()
        {
            try
            {

                Grd_StaffAttandence.DataSource = null;
                Grd_StaffAttandence.DataBind();
                Sql = "select tblstudent.StudentName , tblstudent.Id   from tblstudent INNER join  tblstudentclassmap on tblstudent.Id= tblstudentclassmap.StudentId where tblstudentclassmap.ClassId=" + _ClassId;
                MydataSet = MyAttendance.m_MysqlDb.ExecuteQueryReturnDataSet(Sql);
                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    Grd_StaffAttandence.Columns[0].Visible = true;
                    Grd_StaffAttandence.DataSource = MydataSet;
                    Grd_StaffAttandence.DataBind();

                    Grd_StaffAttandence.Columns[0].Visible = false;
                    //CheckHolidayDaily();
                }
                else
                {
                    Lbl_msg.Text = "The Current Class Has No Student.Please Select another Class  ";
                    this.MPE_MessageBox.Show();
                    Btn_UpdateAttandance.Enabled = false;

                }
            }
            catch (Exception)
            {
                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();

            }


        }

        private void fillClassDrp()
        {
            try
            {
                ListItem li = new ListItem("None", "-1");
                Drp_SelectTheClass.Items.Add(li);
                Sql = "select tblclass.Id, tblclass.ClassName from tblclass";
                MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(Sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_SelectTheClass.Items.Add(li);

                    }
                    Drp_SelectTheClass.SelectedIndex = 0;
                }
                else
                {
                    Lbl_msg.Text = "No Class Found";
                    this.MPE_MessageBox.Show();
                }
               
            }
            catch (Exception)
            {
                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();

            }

        }

        private void FillModeDrp()
        {
            try
            {
                Sql = "select tblattendancemode.Id,tblattendancemode.Attmode from tblattendancemode";
                MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(Sql);
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

        protected void selectDifferMode(object sender, EventArgs e)
        {
            daymode.Visible = false;
            weekmode.Visible = false;
           

               string _DATEToday = MyAttendance.gettodaydate();
               DateTime _Todatdate = DateTime.Today;
               //_DrpModeSelectedValu =
               Cal_DateEntry.SelectedDate = DateTime.Today;
               if (Drp_selectdays.SelectedValue == "2") //week
               {
                   weekinput.Visible = true;
                   Drp_SelectTheClass.SelectedIndex = -1;
                   Btn_UpdateAttandance.Enabled = false;
               }
               if (Drp_selectdays.SelectedValue == "1")
               {
                   weekinput.Visible = false;
                   Drp_SelectTheClass.SelectedIndex = -1;
                   Btn_UpdateAttandance.Enabled = false;
               }
        }

        protected void Btn_UpdateAttandance_Click(object sender, EventArgs e)
        {
            int _batchId = MyUser.CurrentBatchId;
            string message;
            try
            {
                int _UserID = MyUser.UserId;

                if (Grd_StaffAttandence.Visible == true)
                {

                    int _count = 0;
                    DateTime _date;
                    string _DateConvertion;
                    _DateConvertion = (Cal_DateEntry.SelectedDate.ToString());

                    if (_DateConvertion == "1/1/0001 12:00:00 AM")
                    {
                        //_date = DateTime.Parse(_DateConvertion.ToString());
                        _date = DateTime.Today;
                    }
                    else
                    {
                        _date = DateTime.Parse(_DateConvertion.ToString());
                    }
                    int _redateID = MyAttendance.getDateIdFrmMastTbl(_date);
                    string typ = "Class";
                    int _ClsId = int.Parse(Drp_SelectTheClass.SelectedValue);

                    if (MyAttendance.CheckForPrevioudDay(_batchId, _date, _ClsId, typ, out message) == true)
                    {
                        int _dateId = MyAttendance.insertToDateTable(_redateID, typ, _ClsId);
                        foreach (GridViewRow gv in Grd_StaffAttandence.Rows)
                        {
                            CheckBox chksat = (CheckBox)gv.FindControl("Chk_status_daily");
                            if (chksat.Checked == false)
                            {
                                int _StutendId = int.Parse(gv.Cells[0].Text.ToString());
                                MyAttendance.UpadetheStundentfAtceTbl(_StutendId, _dateId);

                              //  MyAttendance.updateIncedentStd(_StutendId, _dateId, _ClsId, _date);
                                _count++;

                            }
                            else
                            {
                                int _StutendId = int.Parse(gv.Cells[0].Text.ToString());
                                MyAttendance.updateIncedentStd(_StutendId, _dateId, _ClsId, _date);

                            }
                        }
                        Lbl_msg.Text = _count + " Student is Absent For the day " + _date.ToString("MMM-dd-yyyy");
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Save Student Attendance", "Save the Student Attendance details for the day " + _date.Date, 1);
                        this.MPE_MessageBox.Show();
                        Btn_UpdateAttandance.Enabled = false;
                        if (Btn_UpdateAttandance.Enabled == false)
                        {
                            Btn_Edit.Visible = true;
                        }
                        else
                        {
                            Btn_Edit.Visible = false;
                        }
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
                    string _typ = "class";
                    int _clsId = int.Parse(Drp_SelectTheClass.SelectedValue);
                    //if (MyAttendance.CheckAttadanceStatus( _getdate, _typ,  _clsId) == false)
                    // {
                    if (MyAttendance.CheckForPrevioudDay(_batchId, _getdate, _clsId, _typ, out message) == true)
                    {
                        MyAttendance.insertToDateTableFrmMonToSat(_getdate, _clsId, _typ);
                        Btn_UpdateAttandance.Enabled = false;
                        foreach (GridViewRow gv in Grd_SelectedWeek.Rows)
                        {
                            _getdate = _rentryOFdate;
                            int clssId = int.Parse(Drp_SelectTheClass.SelectedValue);

                            CheckBox chksat = (CheckBox)gv.FindControl("Chk_status_mon");
                            if (chksat.Checked == false)
                            {

                                _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                int _StutendId = int.Parse(gv.Cells[0].Text.ToString());
                                MyAttendance.UpadetheStundentfAtceTbl(_StutendId, _dateId);
                              
                                

                            }
                            else
                            {
                                _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                int _StutendId = int.Parse(gv.Cells[0].Text.ToString());
                                MyAttendance.updateIncedentStd(_StutendId, _dateId, clssId, _getdate);
                            }

                            CheckBox chksat1 = (CheckBox)gv.FindControl("Chk_status_tu");
                            _getdate = _getdate.AddDays(1);
                            if (chksat1.Checked == false)
                            {


                                _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                int _StutendId = int.Parse(gv.Cells[0].Text.ToString());
                                MyAttendance.UpadetheStundentfAtceTbl(_StutendId, _dateId);
                               


                            }
                            else
                            {
                                _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                int _StutendId = int.Parse(gv.Cells[0].Text.ToString());
                                MyAttendance.updateIncedentStd(_StutendId, _dateId, clssId, _getdate);
                            }
                            CheckBox chksat2 = (CheckBox)gv.FindControl("Chk_status_wed");
                            _getdate = _getdate.AddDays(1);
                            if (chksat2.Checked == false)
                            {


                                _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                int _StutendId = int.Parse(gv.Cells[0].Text.ToString());
                                MyAttendance.UpadetheStundentfAtceTbl(_StutendId, _dateId);
                                
                            }
                            else
                            {
                                _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                int _StutendId = int.Parse(gv.Cells[0].Text.ToString());
                                MyAttendance.updateIncedentStd(_StutendId, _dateId, clssId, _getdate);
                            }
                            CheckBox chksat3 = (CheckBox)gv.FindControl("Chk_status_thu");
                            _getdate = _getdate.AddDays(1);
                            if (chksat3.Checked == false)
                            {


                                _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                int _StutendId = int.Parse(gv.Cells[0].Text.ToString());
                                MyAttendance.UpadetheStundentfAtceTbl(_StutendId, _dateId);
                                
                            }
                            else
                            {
                                _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                int _StutendId = int.Parse(gv.Cells[0].Text.ToString());
                                MyAttendance.updateIncedentStd(_StutendId, _dateId, clssId, _getdate);
                            }
                            CheckBox chksat4 = (CheckBox)gv.FindControl("Chk_status_fri");
                            _getdate = _getdate.AddDays(1);
                            if (chksat4.Checked == false)
                            {


                                _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                int _StutendId = int.Parse(gv.Cells[0].Text.ToString());
                                MyAttendance.UpadetheStundentfAtceTbl(_StutendId, _dateId);
                                
                            }
                            else
                            {
                                _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                int _StutendId = int.Parse(gv.Cells[0].Text.ToString());
                                MyAttendance.updateIncedentStd(_StutendId, _dateId, clssId, _getdate);
                            }

                            CheckBox chksat5 = (CheckBox)gv.FindControl("Chk_status_sat");
                            _getdate = _getdate.AddDays(1);
                            if (chksat5.Checked == false)
                            {


                                _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                int _StutendId = int.Parse(gv.Cells[0].Text.ToString());
                                MyAttendance.UpadetheStundentfAtceTbl(_StutendId, _dateId);
                                
                            }
                            else
                            {
                                _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                                int _StutendId = int.Parse(gv.Cells[0].Text.ToString());
                                MyAttendance.updateIncedentStd(_StutendId, _dateId, clssId, _getdate);
                            }
                            //  _dateId = MyAttendance.insertToDateTable(_getdate);


                        }
                        Lbl_msg.Text = " Student  Attendance have be marked for the  day " + Lbl_startDate.Text + " To " + Lbl_EndDate.Text;
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Save Student Attendance", "Save the Student Attendance details for the week " + Lbl_startDate.Text +"  to "+Lbl_EndDate.Text, 1);
                        this.MPE_MessageBox.Show();

                        // }
                        // else
                        // {
                        //     Lbl_msg.Text = " Attendance is already Marked ";
                        //     this.MPE_MessageBox.Show();
                        //     Btn_UpdateAttandance.Enabled = false;


                        //}


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

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                string _date;
                DateTime _getDate;
                Grd_StaffAttandence.Visible = true;
                _getDate = DateTime.Parse(Cal_DateEntry.SelectedDate.ToString());
                _ClassId = int.Parse(Drp_SelectTheClass.SelectedValue);
                int _batchId = MyUser.CurrentBatchId;
                if (MyAttendance.checkTheBatchDate(_getDate, _batchId) == true)
                {
                    _date = _getDate.ToString("dddd");
                    if (_date != "Sunday")
                    {
                        if (!MyAttendance.CheckAttadanceStatusOfClass(_getDate, _ClassId))
                        {
                            Btn_UpdateAttandance.Enabled = true;
                            Btn_Edit.Visible = false;
                            Pnl_Weekly.Visible = false;
                            Pnl_Daily.Visible = true;
                            CheckHolidayDaily(_ClassId);
                            LoadDetailToGridBox();
                        }
                        else
                        {
                            // Lbl_msg.Text = "Student Attandence Is Already Marked";
                            // this.MPE_MessageBox.Show();
                            LoadDetailToGridBox();
                            StudentAttandanceEdit(_getDate);
                            Btn_UpdateAttandance.Enabled = false;
                            if (Btn_UpdateAttandance.Enabled == false)
                            {
                                Btn_Edit.Visible = true;
                            }
                            else
                            {
                                Btn_Edit.Visible = false;
                            }

                        }                    }
                    else
                    {
                        Lbl_msg.Text = "SUNDAY Select other day";
                        Btn_UpdateAttandance.Enabled = false;
                        this.MPE_MessageBox.Show();
                        Grd_StaffAttandence.Visible = false;

                    }


                }
                else
                {
                    Lbl_msg.Text = "This Is Not A Valid Date.please check the date and try again !";
                    this.MPE_MessageBox.Show();
                    Grd_StaffAttandence.Visible = false;
                    Btn_Edit.Visible = false;
                    Btn_UpdateAttandance.Enabled = false;

                }
            }
            catch (Exception)
            {
                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();

            }
        }

        protected void Txt_statrDate_TextChanged(object sender, EventArgs e)
        {
            try
            {

                Grd_SelectedWeek.Visible = true;
                Lbl_EndDate.Visible = false;
                Lbl_from.Visible = false;
                Lbl_startDate.Visible = false;
                Lbl_to.Visible = false;
                
                _ClassId = int.Parse(Drp_SelectTheClass.SelectedValue);
               
                DateTime _dateFrmTxtBox;
                DateTime _TodaysDateInTxt;
                TofilltheGrdSeleWeekByCheck();
                if (DateTime.TryParse(Txt_statrDate.Text, out _dateFrmTxtBox))
                {

                    _TodaysDateInTxt = _dateFrmTxtBox;
                }
                else
                {
                    _TodaysDateInTxt = DateTime.Today;
                }
                int _batchId = MyUser.CurrentBatchId;
                if (MyAttendance.checkTheBatchDate(_TodaysDateInTxt, _batchId) == true)
                {
                    if (Btn_UpdateAttandance.Enabled == false)
                    {
                        Btn_Edit.Visible = true;
                        Btn_Edit.Enabled = true;
                        TofilltheGrdSeleWeekByCheck();

                        WeeklyStaffAttendanceEdit(_TodaysDateInTxt);

                    }
                    else
                    {
                        TofilltheGrdSeleWeekByCheck();
                        Btn_Edit.Visible = false;
                    }
                }
                else
                {
                    Lbl_msg.Text = "This Is Not A Valid Date.please check the date and try again !";
                    this.MPE_MessageBox.Show();
                    Btn_UpdateAttandance.Enabled = false;
                    Grd_SelectedWeek.Visible = false;

                }
            }
            catch (Exception)
            {
                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();

            }


        }
        //This Funtion is used when different class is selected the funtion swith ie Daily Mode And Weekly Mode. //
        protected void selectDifferclass(object sender, EventArgs e)
        {
            try
            {

                _ClassId = int.Parse(Drp_SelectTheClass.SelectedValue);
                DateTime _todayDate = DateTime.Today;
                //  string _typ = "class";
                int _clsId = int.Parse(Drp_SelectTheClass.SelectedValue);
                if (_clsId == -1)
                {
                    Cal_DateEntry.Enabled = false;
                }
                else
                {
                    Cal_DateEntry.Enabled = true;
                }
                string _DATEToday = MyAttendance.gettodaydate();
                if ((_DATEToday != "Sunday"))
                {
                    if (Drp_selectdays.SelectedValue == "1")
                    {

                        daymode.Visible = true;
                        weekmode.Visible = false;
                        weekinput.Visible = false;
                        
                        if (!MyAttendance.CheckAttadanceStatusOfClass(_todayDate, _clsId))
                        {
                            CheckHolidayDaily(_clsId);
                            Btn_UpdateAttandance.Enabled = true;
                            Btn_Edit.Visible = true;
                            if (Btn_UpdateAttandance.Enabled == false)
                            {
                                Btn_Edit.Visible = true;
                                LoadDetailToGridBox();
                                StudentAttandanceEdit(_todayDate);
                            }
                            else
                            {
                                Btn_Edit.Visible = false;

                                Pnl_Daily.Visible = true;
                                Pnl_Weekly.Visible = false;
                                LoadDetailToGridBox();
                            }

                        }
                        else
                        {
                            //Lbl_msg.Text = "Student Attandence Is Already Marked";
                            // this.MPE_MessageBox.Show();
                            Btn_UpdateAttandance.Enabled = false;
                            Btn_Edit.Visible = true;
                            Pnl_Daily.Visible = true;
                            Pnl_Weekly.Visible = false;
                            LoadDetailToGridBox();
                            StudentAttandanceEdit(_todayDate);

                        }


                    }
                    else
                    {
                        weekmode.Visible = true;
                        daymode.Visible = false;
                        weekinput.Visible = true;

                        Pnl_Daily.Visible = false;
                        Pnl_Weekly.Visible = true;

                        TofilltheGrdSeleWeekByCheck();
                        if (Btn_UpdateAttandance.Enabled == false)
                        {
                            Btn_Edit.Visible = true;
                            Btn_Edit.Enabled = true;
                            TofilltheGrdSeleWeekByCheck();
                            DateTime _dateFrmTxtBox;
                            DateTime _TodaysDateInTxt;
                            if (DateTime.TryParse(Txt_statrDate.Text, out _dateFrmTxtBox))
                            {

                                _TodaysDateInTxt = _dateFrmTxtBox;
                            }
                            else
                            {
                                _TodaysDateInTxt = DateTime.Today;
                            }

                            WeeklyStaffAttendanceEdit(_TodaysDateInTxt);
                        }
                        else
                        {
                            Btn_Edit.Visible = false;
                        }




                        /* }
                         else
                         {
                             Lbl_msg.Text = "Student Attandence Is Already Marked";
                             this.MPE_MessageBox.Show();
                             Btn_UpdateAttandance.Enabled = false;
                         }*/

                        //LoadDetailToGridBox();
                    }
                }
                else
                {
                    Lbl_msg.Text = "Selected day is SUNDAY";
                    this.MPE_MessageBox.Show();
                    Btn_UpdateAttandance.Enabled = false;
                    if (Btn_UpdateAttandance.Enabled == false)
                    {
                        Btn_Edit.Visible = true;
                    }
                    else
                    {
                        Btn_Edit.Visible = false;
                    }

                }

            }
            catch (Exception)
            {
                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();

            }
              
            
           }

        //This Funtion is user to mark the daily holiday of the Student//
        private void CheckHolidayDaily(int _clsId)
        {
            try
            {
                DateTime _CheckDay, _getDate;

                Sql = "select tblmasterdate.`Date` from tblmasterdate inner join tblHoliday on tblHoliday.DateId= tblmasterdate.Id where tblHoliday.Type='class' or tblHoliday.Type='all' or tblHoliday.Class_Id=" + _clsId;
                MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(Sql);
                while (MyReader.Read())
                {
                    if (MyReader.HasRows)
                    {

                        _CheckDay = DateTime.Parse(MyReader.GetValue(0).ToString());
                        _getDate = DateTime.Parse(Cal_DateEntry.SelectedDate.ToString());
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
                        }
                    }
                }
            }
            catch (Exception)
            {
                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();

            }
        }
        //This Funtion is Used to Fill The Weekly Grid view. if the user select any day this funtion redirect to MONDAY as strting day.
        //===== LoadDetailToGridBoxweekly(); is the funtion used in this funtion//
        private void TofilltheGrdSeleWeekByCheck()
        {  DateTime _dateAterTryParse;
        int _flag;
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
                    Lbl_msg.Text = "Selected date is sunday.please select another day";
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
                   _flag= LoadDetailToGridBoxweekly();

                   if (_flag == 1)
                   {
                       if (DateTime.TryParse(Txt_statrDate.Text, out _dateAterTryParse))
                       {
                           Lbl_startDate.Text = _dateAterTryParse.ToString("dd-MMM-yyyy");
                       }
                       else
                       {
                           Lbl_startDate.Text = DateTime.Today.ToString("dd-MMM-yyyy");
                       }

                       //Lbl_startDate.Text = DateTime.Parse(;
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
                    _flag=LoadDetailToGridBoxweekly();
                    if (_flag == 1)
                    {
                        //Lbl_startDate.Text = DateTime.Parse(Txt_statrDate.Text).ToString("dd-MMM-yyyy");
                        if (DateTime.TryParse(Txt_statrDate.Text, out _dateAterTryParse))
                        {
                            Lbl_startDate.Text = _dateAterTryParse.ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            Lbl_startDate.Text = DateTime.Today.ToString("dd-MMM-yyyy");
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
                   _flag= LoadDetailToGridBoxweekly();
                   if (_flag == 1)
                   {
                       // Lbl_startDate.Text = DateTime.Parse(Txt_statrDate.Text).ToString("dd-MMM-yyyy");
                       if (DateTime.TryParse(Txt_statrDate.Text, out _dateAterTryParse))
                       {
                           Lbl_startDate.Text = _dateAterTryParse.ToString("dd-MMM-yyyy");
                       }
                       else
                       {
                           Lbl_startDate.Text = DateTime.Today.ToString("dd-MMM-yyyy");
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
                   _flag= LoadDetailToGridBoxweekly();
                   if (_flag == 1)
                   {
                       // Lbl_startDate.Text = DateTime.Parse(Txt_statrDate.Text).ToString("dd-MMM-yyyy");
                       if (DateTime.TryParse(Txt_statrDate.Text, out _dateAterTryParse))
                       {
                           Lbl_startDate.Text = _dateAterTryParse.ToString("dd-MMM-yyyy");
                       }
                       else
                       {
                           Lbl_startDate.Text = DateTime.Today.ToString("dd-MMM-yyyy");
                       }
                       Lbl_EndDate.Text = _date.AddDays(2).ToString("dd-MMM-yyyy");
                       //Btn_UpdateAttandance.Enabled = true;
                       string _tempString = Lbl_EndDate.Text;
                       DateTime _temp = DateTime.Parse(_tempString.ToString());
                       DateTime _today;
                       _today = DateTime.Now;
                       if (Btn_UpdateAttandance.Enabled == true && _temp > _today)
                       {

                           DisablePreDateIntheGrdWeekly(_today);

                       }
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
                   _flag=LoadDetailToGridBoxweekly();
                   if (_flag == 1)
                   {
                       //Lbl_startDate.Text = DateTime.Parse(Txt_statrDate.Text).ToString("dd-MMM-yyyy");
                       if (DateTime.TryParse(Txt_statrDate.Text, out _dateAterTryParse))
                       {
                           Lbl_startDate.Text = _dateAterTryParse.ToString("dd-MMM-yyyy");
                       }
                       else
                       {
                           Lbl_startDate.Text = DateTime.Today.ToString("dd-MMM-yyyy");
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
                    _flag = LoadDetailToGridBoxweekly();
                    if (_flag == 1)
                    {
                        //  Lbl_startDate.Text = DateTime.Parse(Txt_statrDate.Text).ToString("dd-MMM-yyyy");
                        if (DateTime.TryParse(Txt_statrDate.Text, out _dateAterTryParse))
                        {
                            Lbl_startDate.Text = _dateAterTryParse.ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            Lbl_startDate.Text = DateTime.Today.ToString("dd-MMM-yyyy");
                        }
                        Lbl_EndDate.Text = _date.ToString("dd-MMM-yyyy");
                        //Btn_UpdateAttandance.Enabled = true;
                    }
                }



                /*else
                {
                    Pnl_Daily.Visible = false;
                    Pnl_Weekly.Visible = true;
                    Lbl_EndDate.Visible = true;
                    Lbl_from.Visible = true;
                    Lbl_startDate.Visible = true;
                    Lbl_to.Visible = true;
                    Lbl_startDate.Text = _date.ToString("MMM dd yyyy");
                    Lbl_EndDate.Text = _date.AddDays(5).ToString("MMM dd yyyy");
                    Lbl_msg.Text = "You Can Only Select Monday";
                    this.MPE_MessageBox.Show();
                    Grd_SelectedWeek.Visible = false;
                    Btn_UpdateAttandance.Enabled = false;
                }
                */
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
       /// this funtion Fill the Grid view For the Weekly Mode ///Other Function Used in this function are
        //====DisableAttandanceMarked();,.CheckHoliday(_ClassId);//
        private int  LoadDetailToGridBoxweekly()
        {
            int _flag = 0;
            try
            {
                Sql = "select tblstudent.StudentName , tblstudent.Id   from tblstudent INNER join  tblstudentclassmap on tblstudent.Id= tblstudentclassmap.StudentId where tblstudentclassmap.ClassId=" + _ClassId;
                MydataSet = MyAttendance.m_MysqlDb.ExecuteQueryReturnDataSet(Sql);
                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    //Grd_SelectedWeek.Columns[0].Visible = true;
                    Grd_SelectedWeek.DataSource = MydataSet;
                    Grd_SelectedWeek.DataBind();
                    //Grd_SelectedWeek.Columns[0].Visible = false;
                    DisableAttandanceMarked();
                    CheckHoliday(_ClassId);
                    Grd_SelectedWeek.Visible = true;
                    _flag = 1;

                }
                else
                {
                    Lbl_msg.Text = "No Student found";
                    this.MPE_MessageBox.Show();
                    Btn_UpdateAttandance.Enabled = false;
                    Grd_SelectedWeek.Visible = false;
                    Btn_Edit.Enabled = false;
                    _flag = 0;
                }
            }
            catch (Exception)
            {
                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();
                _flag = 0;

            }
            return _flag;
            
        }
        /// <summary>
        /// /This Funtion is used to Disable the Day Attendance is not marker .this function is used for the weekly attendance edit
        //== other funtion used in this funtion are.MyAttendance.CheckAttadanceStatus,
        /// </summary>
        
        private void DisableAttandanceMarked()
        {  DateTime _getdate;
        DateTime _rentryOFdate;
            try
            {
                if (DateTime.TryParse(Txt_statrDate.Text, out _getdate))
                {
                    _rentryOFdate = _getdate;
                }
                else
                {
                    _rentryOFdate = DateTime.Today;
                }
                //string _WeekDate = Txt_statrDate.Text;
                //DateTime _getdate = DateTime.Parse(_WeekDate.ToString());
               // DateTime _rentryOFdate = _getdate;
                int cont = 0;
                string _typ = "class";
                int _clsid = int.Parse(Drp_SelectTheClass.SelectedValue);
                //int _clsId = int.Parse(Drp_SelectTheClass.SelectedValue);

                foreach (GridViewRow gv in Grd_SelectedWeek.Rows)
                {

                    _getdate = _rentryOFdate;
                    CheckBox chksat = (CheckBox)gv.FindControl("Chk_status_mon");
                    if (MyAttendance.CheckAttadanceStatus(_getdate, _typ, _clsid) == true)
                    {
                        cont++;
                        gv.Cells[2].BackColor = Color.Azure;
                    }
                    else
                    {
                        chksat.Enabled = false;
                    }

                    CheckBox chksat1 = (CheckBox)gv.FindControl("Chk_status_tu");
                    _getdate = _getdate.AddDays(1);
                    if (MyAttendance.CheckAttadanceStatus(_getdate, _typ, _clsid) == true)
                    {
                        cont++;
                        gv.Cells[3].BackColor = Color.Azure;
                    }
                    else
                    {
                        chksat1.Enabled = false;
                    }



                    CheckBox chksat2 = (CheckBox)gv.FindControl("Chk_status_wed");

                    _getdate = _getdate.AddDays(1);
                    if (MyAttendance.CheckAttadanceStatus(_getdate, _typ, _clsid) == true)
                    {
                        cont++;
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
                        cont++;
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
                        cont++;
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
                        cont++;
                        gv.Cells[7].BackColor = Color.Azure;
                    }
                    else
                    {
                        chksat5.Enabled = false;
                    }
                }
                if (cont > 0)
                {
                    Btn_Edit.Visible = true;
                    Btn_UpdateAttandance.Enabled = false;

                }
                else
                {

                    Btn_Edit.Visible = false;
                    Btn_UpdateAttandance.Enabled = true;
                    enableAllTheVlaueInTheGrdView();


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
        // Holiday Check For weekly mode//
        // other fntion used are ValidHoliday
        private void CheckHoliday(int _clsId)
        {
            DateTime _CheckDay;
            string _checkTheDay;
            Sql = "select tblmasterdate.`Date` from tblmasterdate inner join tblHoliday on tblHoliday.DateId= tblmasterdate.Id where tblHoliday.Type='class' or tblHoliday.Type='all' or tblHoliday.Class_Id=" + _clsId;
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

                            }
                            else if (_checkTheDay == "Tuesday")
                            {
                                CheckBox chksat = (CheckBox)gv.Cells[3].FindControl("Chk_status_tu");
                                chksat.Enabled = false;
                            }
                            else if (_checkTheDay == "Wednesday")
                            {
                                CheckBox chksat = (CheckBox)gv.Cells[4].FindControl("Chk_status_wed");
                                chksat.Enabled = false;
                            }
                            else if (_checkTheDay == "Thursday")
                            {
                                CheckBox chksat = (CheckBox)gv.Cells[5].FindControl("Chk_status_thu");
                                chksat.Enabled = false;
                            }
                            else if (_checkTheDay == "Friday")
                            {
                                CheckBox chksat = (CheckBox)gv.Cells[6].FindControl("Chk_status_fri");
                                chksat.Enabled = false;

                            }
                            else if (_checkTheDay == "Saturday")
                            {
                                CheckBox chksat = (CheckBox)gv.Cells[7].FindControl("Chk_status_sat");
                                chksat.Enabled = false;
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
        // this funtion give the view of weekly attendance marked//
        // other funtion used in this funtion are:::MyAttendance.checkOfstudentattendance
        private void WeeklyStaffAttendanceEdit(DateTime _ToDaysDate)
        {
            DateTime _rentryOFdate;
            _rentryOFdate = _ToDaysDate;
            int _StudIdFrmGrid;
          
            foreach (GridViewRow gv in Grd_SelectedWeek.Rows)
            {
                _ToDaysDate = _rentryOFdate;
                _StudIdFrmGrid = int.Parse(gv.Cells[0].Text.ToString());

                if (MyAttendance.checkOfstudentattendance(_ToDaysDate, _StudIdFrmGrid) == true)
                {
                    CheckBox chksatAtt1 = (CheckBox)gv.FindControl("Chk_status_mon");
                    chksatAtt1.Checked = false;

                }
                _ToDaysDate = _ToDaysDate.AddDays(1);
                if (MyAttendance.checkOfstudentattendance(_ToDaysDate, _StudIdFrmGrid) == true)
                {
                    CheckBox chksatAtt2 = (CheckBox)gv.FindControl("Chk_status_tu");
                    chksatAtt2.Checked = false;
                }
                _ToDaysDate = _ToDaysDate.AddDays(1);

                if (MyAttendance.checkOfstudentattendance(_ToDaysDate, _StudIdFrmGrid) == true)
                {
                    CheckBox chksatAtt3 = (CheckBox)gv.FindControl("Chk_status_wed");
                    chksatAtt3.Checked = false;
                }
                _ToDaysDate = _ToDaysDate.AddDays(1);

                if (MyAttendance.checkOfstudentattendance(_ToDaysDate, _StudIdFrmGrid) == true)
                {
                    CheckBox chksatAtt4 = (CheckBox)gv.FindControl("Chk_status_thu");
                    chksatAtt4.Checked = false;
                }
                _ToDaysDate = _ToDaysDate.AddDays(1);

                if (MyAttendance.checkOfstudentattendance(_ToDaysDate, _StudIdFrmGrid) == true)
                {
                    CheckBox chksatAtt5 = (CheckBox)gv.FindControl("Chk_status_fri");
                    chksatAtt5.Checked = false;
                }
                _ToDaysDate = _ToDaysDate.AddDays(1);

                if (MyAttendance.checkOfstudentattendance(_ToDaysDate, _StudIdFrmGrid) == true)
                {
                    CheckBox chksatAtt6 = (CheckBox)gv.FindControl("Chk_status_sat");
                    chksatAtt6.Checked = false;
                }

            }

        }
        //this funtion is check the day in the that week days..this funtion is used inside the CheckHoliday funtion//
        private bool ValidHoliday(DateTime _CheckDay)
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
        //this funtion displays the daily attendance of a particular clas the present and the absent student.
        private void StudentAttandanceEdit(DateTime _dateId)
        {
            try
            {

                Btn_UpdateAttandance.Enabled = false;
                //  Btn_Edit.Enabled = true;
                int _StudIdFrmTbl = 0;
                int _StudIdFrmGrid = 0;
                Sql = "select  tblstudentattendance.StudentId from   tblstudentattendance inner join tbldate on tbldate.Id = tblstudentattendance.DayId inner join tblmasterdate on tblmasterdate.Id= tbldate.DateId where tblmasterdate.`date`='" + _dateId.ToString("yyyy-MM-dd") + "'";
                MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(Sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        _StudIdFrmTbl = int.Parse(MyReader.GetValue(0).ToString());
                        //int _temp = 0;
                        foreach (GridViewRow gv in Grd_StaffAttandence.Rows)
                        {
                            //_temp = 0;


                            _StudIdFrmGrid = int.Parse(gv.Cells[0].Text.ToString());
                            if (_StudIdFrmGrid == _StudIdFrmTbl)
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
                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();

            }

        }
        //funtion  is used to edit the attendance.. 
        protected void Btn_Edit_Click(object sender, EventArgs e)
        {
            try
            {

                if (Grd_StaffAttandence.Visible == true)
                {


                    DateTime _date;
                    string _DateConvertion;
                    _DateConvertion = (Cal_DateEntry.SelectedDate.ToString());

                    if (_DateConvertion == "1/1/0001 12:00:00 AM")
                    {
                        // _date = DateTime.Parse(_DateConvertion.ToString());
                        _date = DateTime.Today;
                    }
                    else
                    {
                        _date = DateTime.Parse(_DateConvertion.ToString());
                    }
                    int _redateID = MyAttendance.getDateIdFrmMastTbl(_date);
                    string typ = "Class";
                    int _ClsId = int.Parse(Drp_SelectTheClass.SelectedValue);
                    int _dateId = MyAttendance.getDateIdFrmDateTable(_redateID, typ, _ClsId);
                    int _StutendId;
                    foreach (GridViewRow gv in Grd_StaffAttandence.Rows)
                    {
                        CheckBox chksat = (CheckBox)gv.FindControl("Chk_status_daily");
                        if (chksat.Checked == false)
                        {
                            _StutendId = int.Parse(gv.Cells[0].Text.ToString());
                            if (!MyAttendance.FindIfTheStundtIsPrntInthetblStuAttandence(_StutendId, _dateId))
                            {
                                MyAttendance.UpadetheStundentfAtceTbl(_StutendId, _dateId);
                                //_count++;
                            }
                        }
                        else
                        {
                            _StutendId = int.Parse(gv.Cells[0].Text.ToString());
                            if (MyAttendance.FindIfTheStundtIsPrntInthetblStuAttandence(_StutendId, _dateId) == true)
                            {
                                MyAttendance.deleteStudentAtt(_StutendId, _dateId);
                            }
                        }

                    }

                 //   Btn_Edit.Visible = false;
                    Btn_UpdateAttandance.Enabled = false;
                    //Lbl_msg.Text = _count + " Student is Absent For the day " + _date.ToString("MMM-dd-yyyy");
                    //this.MPE_MessageBox.Show();

                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Update Student Attendance", "Update the Student Attendance details for the day " + _date.Date, 1);

                    Lbl_msg.Text = "Attendance Details Updated";
                    this.MPE_MessageBox.Show();


                }
                else
                {
                    string _WeekDate = Lbl_startDate.Text;
                    int _dateId;
                    DateTime _getdate = DateTime.Parse(_WeekDate.ToString());
                    DateTime _rentryOFdate = _getdate;
                    string _typ = "class";
                    int _clsId = int.Parse(Drp_SelectTheClass.SelectedValue);
                    //if (MyAttendance.CheckAttadanceStatus( _getdate, _typ,  _clsId) == false)
                    // {

                    //MyAttendance.insertToDateTableFrmMonToSat(_getdate, _clsId, _typ);

                    foreach (GridViewRow gv in Grd_SelectedWeek.Rows)
                    {
                        _getdate = _rentryOFdate;
                        int clssId = int.Parse(Drp_SelectTheClass.SelectedValue);

                        CheckBox chksat = (CheckBox)gv.FindControl("Chk_status_mon");
                        if (chksat.Checked == false)
                        {

                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            int _StutendId = int.Parse(gv.Cells[0].Text.ToString());

                            if (!MyAttendance.FindIfTheStundtIsPrntInthetblStuAttandence(_StutendId, _dateId))
                            {
                                MyAttendance.UpadetheStundentfAtceTbl(_StutendId, _dateId);
                            }
                        }
                        else
                        {
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            int _StutendId = int.Parse(gv.Cells[0].Text.ToString());

                            if (MyAttendance.FindIfTheStundtIsPrntInthetblStuAttandence(_StutendId, _dateId) == true)
                            {
                                MyAttendance.deleteStudentAtt(_StutendId, _dateId);
                            }

                        }

                        CheckBox chksat1 = (CheckBox)gv.FindControl("Chk_status_tu");

                        if (chksat1.Checked == false)
                        {
                            _getdate = _getdate.AddDays(1);
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            int _StutendId = int.Parse(gv.Cells[0].Text.ToString());

                            if (!MyAttendance.FindIfTheStundtIsPrntInthetblStuAttandence(_StutendId, _dateId))
                            {
                                MyAttendance.UpadetheStundentfAtceTbl(_StutendId, _dateId);
                            }
                        }
                        else
                        {
                            _getdate = _getdate.AddDays(1);
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            int _StutendId = int.Parse(gv.Cells[0].Text.ToString());

                            if (MyAttendance.FindIfTheStundtIsPrntInthetblStuAttandence(_StutendId, _dateId) == true)
                            {
                                MyAttendance.deleteStudentAtt(_StutendId, _dateId);
                            }

                        }
                        CheckBox chksat2 = (CheckBox)gv.FindControl("Chk_status_wed");
                        _getdate = _getdate.AddDays(1);
                        if (chksat2.Checked == false)
                        {
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            int _StutendId = int.Parse(gv.Cells[0].Text.ToString());

                            if (!MyAttendance.FindIfTheStundtIsPrntInthetblStuAttandence(_StutendId, _dateId))
                            {
                                MyAttendance.UpadetheStundentfAtceTbl(_StutendId, _dateId);
                            }
                        }
                        else
                        {
                            //_getdate = _getdate.AddDays(1);
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            int _StutendId = int.Parse(gv.Cells[0].Text.ToString());

                            if (MyAttendance.FindIfTheStundtIsPrntInthetblStuAttandence(_StutendId, _dateId) == true)
                            {
                                MyAttendance.deleteStudentAtt(_StutendId, _dateId);
                            }

                        }

                        CheckBox chksat3 = (CheckBox)gv.FindControl("Chk_status_thu");
                        _getdate = _getdate.AddDays(1);
                        if (chksat3.Checked == false)
                        {
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            int _StutendId = int.Parse(gv.Cells[0].Text.ToString());

                            if (!MyAttendance.FindIfTheStundtIsPrntInthetblStuAttandence(_StutendId, _dateId))
                            {
                                MyAttendance.UpadetheStundentfAtceTbl(_StutendId, _dateId);
                            }
                        }
                        else
                        {
                            //  _getdate = _getdate.AddDays(1);
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            int _StutendId = int.Parse(gv.Cells[0].Text.ToString());

                            if (MyAttendance.FindIfTheStundtIsPrntInthetblStuAttandence(_StutendId, _dateId) == true)
                            {
                                MyAttendance.deleteStudentAtt(_StutendId, _dateId);
                            }

                        }

                        CheckBox chksat4 = (CheckBox)gv.FindControl("Chk_status_fri");
                        _getdate = _getdate.AddDays(1);
                        if (chksat4.Checked == false)
                        {
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            int _StutendId = int.Parse(gv.Cells[0].Text.ToString());

                            if (!MyAttendance.FindIfTheStundtIsPrntInthetblStuAttandence(_StutendId, _dateId))
                            {
                                MyAttendance.UpadetheStundentfAtceTbl(_StutendId, _dateId);
                            }
                        }
                        else
                        {
                            // _getdate = _getdate.AddDays(1);
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            int _StutendId = int.Parse(gv.Cells[0].Text.ToString());

                            if (MyAttendance.FindIfTheStundtIsPrntInthetblStuAttandence(_StutendId, _dateId) == true)
                            {
                                MyAttendance.deleteStudentAtt(_StutendId, _dateId);
                            }

                        }


                        CheckBox chksat5 = (CheckBox)gv.FindControl("Chk_status_sat");
                        _getdate = _getdate.AddDays(1);
                        if (chksat5.Checked == false)
                        {
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            int _StutendId = int.Parse(gv.Cells[0].Text.ToString());

                            if (!MyAttendance.FindIfTheStundtIsPrntInthetblStuAttandence(_StutendId, _dateId))
                            {
                                MyAttendance.UpadetheStundentfAtceTbl(_StutendId, _dateId);
                            }
                        }
                        else
                        {
                            _getdate = _getdate.AddDays(1);
                            _dateId = MyAttendance.selectDateIdFrmDay(_getdate, _typ);
                            int _StutendId = int.Parse(gv.Cells[0].Text.ToString());

                            if (MyAttendance.FindIfTheStundtIsPrntInthetblStuAttandence(_StutendId, _dateId) == true)
                            {
                                MyAttendance.deleteStudentAtt(_StutendId, _dateId);
                            }

                        }


                    }
                  //  Btn_Edit.Visible = false;
                    //Btn_UpdateAttandance.Enabled = true;
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Update Student Attendance", "Update the Student Attendance details for the week " + Lbl_startDate.Text +" to "+Lbl_EndDate.Text, 1);
                    Lbl_msg.Text = "Attendance Details Updated";
                    this.MPE_MessageBox.Show();
                }



            }
            catch (Exception)
            {
                Lbl_msg.Text = "Please Try Again";
                this.MPE_MessageBox.Show();

            }

        }
        
    

    }
}
