using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data.Odbc;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;
using DayPilot.Utils;
namespace WinEr
{
    public partial class HolidayManager : System.Web.UI.Page
    {
        private ConfigManager MyConfiMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet = null, holidaydataset,EventDataset;
        private Attendance MyAttendance;
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];

            if (MyUser.SELECTEDMODE == 1)
            {
                this.MasterPageFile = "~/WinerStudentMaster.master";
            }
            else if (MyUser.SELECTEDMODE == 2)
            {
                this.MasterPageFile = "~/WinerSchoolMaster.master";
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyConfiMang = MyUser.GetConfigObj();
            MyAttendance = MyUser.GetAttendancetObj();
            if (MyConfiMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MyAttendance == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(92))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadClassDetails();
                    Load_Calender();

                }
            }
        }

        private void LoadClassDetails()
        {
            Drp_class.Items.Clear();
            ListItem li = new ListItem("All Class", "-1");
            Drp_class.Items.Add(li);
            MydataSet = MyUser.MyAssociatedClass();

            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {

                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_class.Items.Add(li);

                }
                Drp_class.SelectedIndex = 0;
            }
        }

        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime nextholidayDate;

            // Holiday Parsing
            if (holidaydataset != null && holidaydataset.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in holidaydataset.Tables[0].Rows)
                {
                    nextholidayDate = (DateTime)dr[0];
                    if (MyAttendance.IsDefaultHoliday(e.Day.Date.DayOfWeek))
                    {
                        e.Cell.BackColor = System.Drawing.Color.Khaki;
                    }
                    else
                    {
                        if (nextholidayDate == e.Day.Date)
                        {
                            if (dr[1].ToString() == "class")
                                e.Cell.BackColor = System.Drawing.Color.Gold;
                            else
                                e.Cell.BackColor = System.Drawing.Color.Gold;

                            Label lbl = new Label();
                            lbl.Font.Size = 10;
                            string Name = dr[2].ToString();
                            if (Name.Length > 10)
                            {
                                Name = Name.Substring(0, 9);
                            }
                            lbl.Text = "<br>" + Name;
                            e.Cell.Controls.Add(lbl);
                        }
                    }
                }
            }
            else
            {
                if (MyAttendance.IsDefaultHoliday(e.Day.Date.DayOfWeek))
                {
                    e.Cell.BackColor = System.Drawing.Color.Khaki;
                }
            }


            // Event Parsing

            if (EventDataset != null && EventDataset.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in EventDataset.Tables[0].Rows)
                {
                    nextholidayDate = (DateTime)dr[0];

                    if (nextholidayDate == e.Day.Date)
                    {
                        if (dr[1].ToString() == "0")
                            e.Cell.BackColor = System.Drawing.Color.Turquoise;
                        else
                            e.Cell.BackColor = System.Drawing.Color.Turquoise;

                        Label lbl = new Label();
                        lbl.Font.Size = 10;
                        string EventName = dr[2].ToString();
                        if (EventName.Length > 10)
                        {
                            EventName = EventName.Substring(0,9);
                        }
                        lbl.Text = "<br>" + EventName;
                        e.Cell.Controls.Add(lbl);
                    }

                }
            }

        }

        protected void Calendar1_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            Load_Calender();
        }

        protected void Drp_class_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_Calender();
        }

        private void Load_Calender()
        {
            Calendar1.SelectedDate = new DateTime().Date;

            if (Pnl_FilterPanel.Visible)
            {
                // EventList Calling
                GetDuration();
                LoadEventList();
              
            }
            else
            {
                string type = "class";
                int Classid = int.Parse(Drp_class.SelectedValue);

                if (Drp_class.SelectedValue == "-1")
                {
                    type = "all";
                    Classid = 0;
                }
                holidaydataset = MyAttendance.MyAssociatedHolidays(type, Classid, MyUser.User_Id);
                EventDataset = MyAttendance.MyAssociatedEventsDataSet(Classid, MyUser.User_Id);
            }

          
        }

        private DateTime GetCalenderDate()
        {
            DateTime SelectedDate = Calendar1.SelectedDate;
            if (Session["SelectedDate"] != null)
            {
                DateTime.TryParse(Session["SelectedDate"].ToString(), out SelectedDate);
            }
            return SelectedDate;
        }
   

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            Session["SelectedDate"] = Calendar1.SelectedDate.Date;
            LinkButton_AddEvent.Visible = true;
            bool IsHoliday = false,IsEvent=false;
            string ClassId = "0", EventName = "", EventId = "", Description = "", Publish="" ;
            if (Drp_class.SelectedValue != "-1")
            {
                ClassId = Drp_class.SelectedValue;
            }

            if (MyAttendance.IsDefaultHoliday(GetCalenderDate().Date.DayOfWeek))
            {
                IsHoliday = true;
            }
            else if (MyAttendance.holiday(GetCalenderDate(), int.Parse(ClassId)))
            {
                IsHoliday = true;
            }

            if (MyAttendance.IsEventPresent(GetCalenderDate(), int.Parse(ClassId), out EventName, out EventId, out Description, out Publish))
            {
                IsEvent = true;
            }

            if (IsHoliday && IsEvent)
            {
                MPE_SelectDay.Show();
            }
            else if (IsHoliday)
            {
                ManageHoliday();
            }
            else if (IsEvent)
            {
                ManageEvents();
            }
            else
            {
                MPE_SelectDay.Show();
            }
          
        }

        protected void Btn_Close_Click(object sender, EventArgs e)
        {
           
            Load_Calender();
        }



        protected void Btn_Continue_Click(object sender, EventArgs e)
        {
         
            if (Rdb_Type.SelectedValue == "0")
            {
                ManageHoliday();
            }
            else
            {
                ManageEvents();
            }
        }


        protected void Lnk_GotoEvent_Click(object sender, EventArgs e)
        {
            ManageEvents();
        }

        protected void Lnk_GotoHoliday_Click(object sender, EventArgs e)
        {
            ManageHoliday();
        }
       


        # region Code For Managing Events


        private void ManageEvents()
        {
            string ClassId = "0",EventName="",EventId="",Description = "", Publish="" ;
            if (Drp_class.SelectedValue != "-1")
            {
                ClassId = Drp_class.SelectedValue;
            }

            if (MyAttendance.IsEventPresent(GetCalenderDate(), int.Parse(ClassId), out EventName, out EventId, out Description, out Publish))
            {
                Lnk_GotoHoliday.Visible = true;
                Hdn_EventId.Value = EventId;
                Txt_EditEventName.Text = EventName;
                Txt_EditEventDescriptionHtml.Content = Description;
                if (Publish == "1")
                {
                    Chk_EditEventPublish.Checked = true;
                }
                else
                {
                    Chk_EditEventPublish.Checked = false;
                }
                MPE_EditEvent.Show();
            }
            else
            {
                MPE_AddEvent.Show();

            }


          

        }

       

        protected void Btn_AddEvent_Click(object sender, EventArgs e)
        {
            Lbl_EventMsg.Text = "";
            string Msg="";
            if (IsEventAdditionPossible(out Msg))
            {
                try
                {
                    MyAttendance.CreateTansationDb();

                    StoreEvent(0, GetCalenderDate());

                    MyAttendance.EndSucessTansationDb();
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create Event", " Create Event " + Txt_EventName.Text, 1);
                    ClearAddEvent();
                    WC_MessageBox.ShowMssage("Event Created Successfully.");
                    
                }
                catch
                {
                    MyAttendance.EndFailTansationDb();
                    Lbl_EventMsg.Text = "Error while adding event. Try later";
                    MPE_AddEvent.Show();
                }
            }
            else
            {
                Lbl_EventMsg.Text = Msg;
                MPE_AddEvent.Show();
                
            }
            Load_Calender();
        }

        private void ClearAddEvent()
        {
            Txt_EventName.Text = "";
            Txt_EventDescriptionHtml.Content = "";
            Chk_Publish.Checked = false;
            Chk_DefaultHolidayForEvent.Checked = false;
            Txt_NoEventDays.Text = "";
            Lbl_EventMsg.Text = "";
        }

        private void StoreEvent(int ClassId, DateTime _currentday)
        {
            int _cnt = 0, _maxlimit = int.Parse(Txt_NoEventDays.Text);
            int EventId = 0;
            string description = Txt_EventDescriptionHtml.Content;
            string EventName = Txt_EventName.Text;
            bool Publish = Chk_Publish.Checked;

            MyAttendance.InsertMasterEvent(EventName, description, Publish,MyUser.UserName, out EventId);

            while (_cnt < _maxlimit)
            {
                if (!Chk_DefaultHolidayForEvent.Checked)
                {
                    while (MyAttendance.IsDefaultHoliday(_currentday.Date.DayOfWeek))
                    {
                        _currentday = _currentday.AddDays(1);
                    }
                }
                MyAttendance.InsertEvents(EventId, ClassId, _currentday);
                _currentday = _currentday.AddDays(1);
                _cnt = _cnt + 1;
                

            }
        }

        


        private bool IsEventAdditionPossible(out string Msg)
        {
            Msg = "";
            bool valid = true;
            if (Txt_EventName.Text.Trim() == "")
            {
                Msg = "Enter event name";
                valid = false;
            }
            //else if (CheckEventName(Txt_EventName.Text,"0"))
            //{
            //    Msg = "Same event already exist";
            //    valid = false;
            //}
            else if (Txt_EventDescriptionHtml.Content == "")
            {
                Msg = "Enter event description";
                valid = false;
            }
            else if (Txt_NoEventDays.Text.Trim() == "")
            {
                Msg = "Enter no of event days";
                valid = false;
            }
           string Test=  Txt_EventDescriptionHtml.Content;
           Test = Test.Replace("'", "");
           Test = Test.Replace("\\", "");
           Txt_EventDescriptionHtml.Content = Test;

            return valid;
        }

        private bool CheckEventName(string EventName,string EventId)
        {
            bool valid = false;

            string sql = "SELECT COUNT(tbleventmaster.Id) FROM tbleventmaster WHERE tbleventmaster.EventName='" + EventName + "' AND tbleventmaster.Id!=" + EventId;
            MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                if (int.Parse(MyReader.GetValue(0).ToString()) > 0)
                {
                    valid = true;
                }
            }
            return valid;
        }

        protected void Btn_EventCLose1_Click(object sender, EventArgs e)
        {
            ClearAddEvent();
            Load_Calender();
        }


        protected void Btn_UpdateEvent_Click(object sender, EventArgs e)
        {
            Lbl_EventUpdateMsg.Text = "";
            string Msg = "";
            if (IsEventUpdationPossible(out Msg))
            {
                try
                {
                    MyAttendance.CreateTansationDb();

                    UpdateEvent(0, GetCalenderDate());

                    MyAttendance.EndSucessTansationDb();
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Update Event", " Update Event " + Txt_EditEventName.Text, 1);
                    ClearUpdateEvent();
                }
                catch
                {
                    MyAttendance.EndFailTansationDb();
                    Lbl_EventUpdateMsg.Text = "Error while updating. Try later";
                    MPE_EditEvent.Show();
                    
                }
            }
            else
            {
                Lbl_EventUpdateMsg.Text = Msg;
                MPE_EditEvent.Show();
                
            }
            Load_Calender();

        }

        private void ClearUpdateEvent()
        {
            Txt_EditEventName.Text = "";
            Txt_EditEventDescriptionHtml.Content = "";
            Chk_EditEventPublish.Text = "";
            Lbl_EventUpdateMsg.Text = "";
        }

        private void UpdateEvent(int ClassId, DateTime Day)
        {
            int ispublish = 0;
            if (Chk_EditEventPublish.Checked)
            {
                ispublish = 1;
            }
            string sql = "UPDATE tbleventmaster SET EventName='" + Txt_EditEventName.Text + "',Description='" + Txt_EditEventDescriptionHtml.Content + "',IsPublish=" + ispublish + ",CreatedBy='" + MyUser.UserName + "',CreatedDateTime='" + DateTime.Now.Date.ToString("s") + "' WHERE Id=" + Hdn_EventId.Value;
            MyAttendance.m_TransationDb.ExecuteQuery(sql);
        }

        private bool IsEventUpdationPossible(out string Msg)
        {
            Msg = "";
            bool valid = true;
            if (Txt_EditEventName.Text.Trim() == "")
            {
                Msg = "Enter event name";
                valid = false;
            }
            else if (CheckEventName(Txt_EditEventName.Text,Hdn_EventId.Value))
            {
                Msg = "Same event already exist";
                valid = false;
            }
            else if (Txt_EditEventDescriptionHtml.Content == "")
            {
                Msg = "Enter event description";
                valid = false;
            }
            string Test = Txt_EditEventDescriptionHtml.Content;
            Test = Test.Replace("'", "");
            Test = Test.Replace("\\", "");
            Txt_EditEventDescriptionHtml.Content = Test;
            return valid;
        }

        protected void Btn_DeleteEvent_Click(object sender, EventArgs e)
        {

            Mpe_DeleteEvent.Show();

        }

        protected void Btn_EventClose2_Click(object sender, EventArgs e)
        {
            Load_Calender();
        }

        protected void Btn_EventClose3_Click(object sender, EventArgs e)
        {
            Load_Calender();
        }

        protected void Btn_DeleteButton_Click(object sender, EventArgs e)
        {
            if (Rdb_EventDeleteionType.SelectedValue == "0")
            {
                DeleteDailyEvent(Hdn_EventId.Value);
            }
            else if (Rdb_EventDeleteionType.SelectedValue == "1")
            {
                DeleteEntireEvent(Hdn_EventId.Value);              
            }
            Load_Calender();
        }

        private void DeleteDailyEvent(string Id)
        {
            DateTime Day = GetCalenderDate();
            if (Hd_SelectedDate.Value != "")
            {
                Day = General.GetDateTimeFromText(Hd_SelectedDate.Value);
                Hd_SelectedDate.Value = "";
            }
            int ClassId = 0,Count=0;

            string sql = "SELECT COUNT(EventId) FROM tblcalender_event WHERE EventId="+Id;
            MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(),out Count);
            }
            int DateID= MyAttendance.getDateIdFrmMastTbl(Day);

            try
            {
                MyAttendance.CreateTansationDb();

                sql = "DELETE FROM tblcalender_event WHERE EventId="+Id+" AND DateId="+DateID+" AND ClassId="+ClassId;
                MyAttendance.m_TransationDb.ExecuteQuery(sql);
                if (Count == 1)
                {
                    sql = "DELETE FROM tbleventmaster WHERE Id=" + Id;
                    MyAttendance.m_TransationDb.ExecuteQuery(sql);
                }


                MyAttendance.EndSucessTansationDb();
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Delete Event", " Delete Event " + Txt_EventName.Text, 1);
            }
            catch
            {
                MyAttendance.EndFailTansationDb();
            }
           
        }

        private void DeleteEntireEvent(string Id)
        {
            try
            {
                MyAttendance.CreateTansationDb();

                string sql = "DELETE FROM tblcalender_event WHERE EventId=" + Id;
                MyAttendance.m_TransationDb.ExecuteQuery(sql);

                sql = "DELETE FROM tbleventmaster WHERE Id=" + Id;
                MyAttendance.m_TransationDb.ExecuteQuery(sql);

                

                MyAttendance.EndSucessTansationDb();
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Delete Event", " Delete Event " + Txt_EventName.Text, 1);
            }
            catch
            {
                MyAttendance.EndFailTansationDb();
            }
        }

        # endregion




        # region Code For Managing Holiday

        private void ManageHoliday()
        {
            int holidayid;
            string type = "";
            string _mode = "", holidaydescription;
            int _clasId = 0;
            try
            {

                DateTime _calSelectDate = System.DateTime.Today;

                _calSelectDate = GetCalenderDate();

                if (MyAttendance.IsDefaultHoliday(_calSelectDate.Date.DayOfWeek))
                {
                    LinkButton_AddEvent.Visible = true;
                    Lbl_msg.Text = _calSelectDate.Date.DayOfWeek.ToString() + " is Default Holiday";
                    this.MPE_MessageBox.Show();
                }
                else
                {
                    if (Drp_class.SelectedValue != "-1")
                    {
                        _mode = "class";
                        _clasId = int.Parse(Drp_class.SelectedValue.ToString());
                    }
                    else
                    {
                        _mode = "all";
                        _clasId = 0;

                    }


                    if (CheckdayHasAlreadyMarkedAsHoliDay(GetCalenderDate(), _mode, _clasId, out holidaydescription, out holidayid, out type)) // any holiday is assignrd to the selected day
                    {

                        lbl_date.Text = MyUser.GerFormatedDatVal(_calSelectDate);
                        lbl_edit_holidayid.Text = holidayid.ToString();
                        lbl_editmode.Text = _mode;
                        lbl_editclass.Text = _clasId.ToString();

                        txt_editdescription.Text = holidaydescription;

                        lblallclass.Text = Drp_class.SelectedItem.Text;
                        if (type == "all")
                        {
                            lblallclass.Text = "All Class";
                        }
                        Lnk_GotoEvent.Visible = true;
                        ModalPopupExtender_editdetailsofholiday.Show();


                    }
                    else
                    {
                        lblstartdate.Text = MyUser.GerFormatedDatVal(_calSelectDate.Date);
                        lblmode.Text = _mode;
                        lblclass.Text = _clasId.ToString();
                        txtdescription.Text = "";
                        Txt_number.Text = "1";
                        lblerror.Text = "";
                        ModalPopupExtender_addholiday.Show();

                    }

                }
            }
            catch
            {
                Lbl_msg.Text = "Try Again";
                this.MPE_MessageBox.Show();
            }
            
            Load_Calender();
        }

        


        private bool CheckdayHasAlreadyMarkedAsHoliDay(DateTime _calSelectDate, string mode, int classid, out string holidaydescription, out int holidayid, out string type)
        {
            bool _flag = false;
            holidaydescription = "";
            holidayid = 0;
            type = "";
            int _dateid = MyAttendance.getDateIdFrmMastTbl(_calSelectDate);

            string sql = "select distinct tblmasterdate.date, tblholiday.Desc, tblholiday.Id,tblholiday.Type from tblmasterdate inner join tblholiday on tblholiday.dateId= tblmasterdate.Id  where tblholiday.dateId=" + _dateid + " and ((tblholiday.Type='all') or (tblholiday.Class_Id= " + classid + " and tblholiday.Type='" + mode + "'))";

            MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _flag = true;
                holidaydescription = MyReader.GetValue(1).ToString();
                holidayid = int.Parse(MyReader.GetValue(2).ToString());
                type = MyReader.GetValue(3).ToString();
            }

            return _flag;

        }

     


  

        protected void btn_addholiday_Click(object sender, EventArgs e)
        {

            int _ClassId = int.Parse(lblclass.Text);
            string _type = lblmode.Text, _msgerror = null;
            bool _noerroroccurs = true;
            try
            {

                if (txtdescription.Text.Trim() != "")
                {

                    MyAttendance.CreateTansationDb();

                    if (!isAttendance_NotMarked_ON_AnyDay_In_List( _ClassId, _type, out _msgerror)) //check attendance is marked on any of the days in list
                        _noerroroccurs = false;

                    if (_noerroroccurs)
                    {

                            if (some_holiday_isExists_inTheRange( _ClassId, _type, out _msgerror))
                                //delete all categorys if the type is not all 
                                _noerroroccurs = false;

                    }
                    if (_noerroroccurs)
                    {

                            if (_type == "class")
                            {
                                addToholidaysDb( _ClassId, "class");
                            }
                            else
                            {

                                addToholidaysDb( 0, "all");
                            }

                    }
                    else
                    {
                        _noerroroccurs = false;

                    }
                    if (_noerroroccurs)
                    {
                        MyAttendance.EndSucessTansationDb();
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Add Holiday", "Add Holiday from " + lblstartdate.Text +". Number of days is "+Txt_number.Text, 1);
                        Lbl_msg.Text = "The Holiday Details Are Saved";
                        this.MPE_MessageBox.Show();


                    }
                    else
                    {
 
                            Lbl_msg.Text = _msgerror;
                            lblerror.Text = "";
                            this.MPE_MessageBox.Show();
                            MyAttendance.EndFailTansationDb();

                    }
                }
                else
                {
                    lblerror.Text = "Enter The Description First To Save The Holiday";
                    ModalPopupExtender_addholiday.Show();
                }

            }
            catch
            {
                MyAttendance.EndFailTansationDb();
                Lbl_msg.Text = "Holiday Details are Not Saved.Please Try Again";
                this.MPE_MessageBox.Show();

            }
            Load_Calender();
        }


        protected void btn_cancelholiday_Click(object sender, EventArgs e)
        {
            Load_Calender();
        }






        private bool some_holiday_isExists_inTheRange( int _Classid, string _type, out string _msgerror)
        {
            bool _valid_day = false;
            _msgerror = "";
            int _cnt = 0, _maxlimit = int.Parse(Txt_number.Text), _dateid;

            DateTime _currentday = MyUser.GetDareFromText(lblstartdate.Text.ToString());
            while (_cnt < _maxlimit)
            {
                _dateid = MyAttendance.getDateIdFrmMastTbl(_currentday);
                if (!Chk_defaultHolidayStatus.Checked)
                {
                    while (MyAttendance.IsDefaultHoliday(_currentday.Date.DayOfWeek))
                    {
                        _currentday = _currentday.AddDays(1);
                    }
                }
                if (isHoliday(_dateid, _type,  _Classid))
                {
                    string str = Drp_class.SelectedItem.Text;
                    if (_type == "all")
                    {
                        str = "all years";
                    }
                    _valid_day = true;

                    _msgerror = "Holiday is already assigned for " + str + " on " + MyUser.GerFormatedDatVal(_currentday) + " Not possible to save this day as Holiday .Please try again after reducing no. of days";

                    break;
                }
                _currentday = _currentday.AddDays(1);
                _cnt = _cnt + 1;
            }
            return _valid_day;
        }

        private bool isHoliday(int _dateid, string _type,  int _Classid)
        {
            bool _flag = false;

            string sql = "select distinct tblholiday.Desc from tblmasterdate inner join tblholiday on tblholiday.dateId= tblmasterdate.Id  where tblholiday.dateId=" + _dateid + " and tblholiday.Class_Id=" + _Classid + " and tblholiday.Type='" + _type + "'";

            MyReader = MyAttendance.m_TransationDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _flag = true;
            }
            return _flag;
        }


        private bool isAttendance_NotMarked_ON_AnyDay_In_List( int _Classid, string _type, out string _msgerror)
        {
            bool _validdays = true;
            _msgerror = "";
            string to_whome;
            int _cnt = 0, _maxlimit = int.Parse(Txt_number.Text), _dateid;
            DateTime _currentday = MyUser.GetDareFromText(lblstartdate.Text.ToString());
            while (_cnt < _maxlimit)
            {
                _dateid = MyAttendance.getDateIdFrmMastTbl(_currentday);

                if (isAttendanceAssigned(_Classid, _dateid,_currentday, _type, out to_whome))
                {
                    _validdays = false;

                    _msgerror = "Attendance is marked for " + to_whome + " on the day " + MyUser.GerFormatedDatVal(_currentday) + ". Not Possible to save this day as Holiday .Please try again";

                    break;
                }

                _currentday = _currentday.AddDays(1);
                _cnt = _cnt + 1;
            }
            return _validdays;

        }


        private bool isAttendanceAssigned( int ClassId, int _dateid, DateTime _currentday, string _typ, out string to_whome)
        {
            bool attendanceassigned = false;
            string sql = "";
            to_whome = null;
            string standardId = "";
            if (MyAttendance.IsNewAttendance())
            {

                if (_typ == "class")
                {
                    standardId = MyAttendance.GetStandard_Class(ClassId);
                    if (MyAttendance.AttendanceTables_Exits(standardId, MyUser.CurrentBatchId))
                    {

                        sql = "SELECT tblclass.ClassName FROM tblattdcls_std" + standardId + "yr" + MyUser.CurrentBatchId + " as t1 INNER JOIN tblclass ON tblclass.Id=t1.ClassId WHERE t1.`Date`='" + _currentday.Date.ToString("s") + "'";
                        MyReader = MyAttendance.m_TransationDb.ExecuteQuery(sql);

                        if (MyReader.HasRows) //means attendance is marked on the date
                        {
                            attendanceassigned = true;
                            to_whome = MyReader.GetValue(0).ToString();
                        }
                    }
                }
                else
                {
                    string TempClassId = "";
                    MydataSet = MyUser.MyAssociatedClass();
                    if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in MydataSet.Tables[0].Rows)
                        {
                            TempClassId = dr[0].ToString();
                            if (!attendanceassigned)
                            {
                                standardId = MyAttendance.GetStandard_Class(int.Parse(TempClassId));
                                if (MyAttendance.AttendanceTables_Exits(standardId, MyUser.CurrentBatchId))
                                {
                                    sql = "SELECT tblclass.ClassName FROM tblattdcls_std" + standardId + "yr" + MyUser.CurrentBatchId + " as t1 INNER JOIN tblclass ON tblclass.Id=t1.ClassId WHERE t1.`Date`='" + _currentday.Date.ToString("s") + "'";
                                    MyReader = MyAttendance.m_TransationDb.ExecuteQuery(sql);

                                    if (MyReader.HasRows) //means attendance is marked on the date
                                    {
                                        attendanceassigned = true;
                                        to_whome = MyReader.GetValue(0).ToString();
                                        break;
                                    }
                                }
                            }
                        }
                    }

                }

            }
            else
            {

                //check is assigned to class

                if (_typ == "class")
                {
                    sql = "select tbldate.Id, tbldate.Status from tbldate inner join tblclass on tblclass.Id=tbldate.classId where tbldate.Status='class' and tbldate.classId=" + ClassId + " and tbldate.DateId=" + _dateid;
                }
                else
                {
                    sql = "select tbldate.Id, tbldate.Status from tbldate where tbldate.DateId=" + _dateid;
                }

                MyReader = MyAttendance.m_TransationDb.ExecuteQuery(sql);
                if (MyReader.HasRows) //means attendance is marked on the date
                {
                    attendanceassigned = true;
                    to_whome = MyReader.GetValue(1).ToString();
                }
            }
            return attendanceassigned;
        }

        private void addToholidaysDb( int _ClassId, string _type)
        {
            int _cnt = 0, _maxlimit = int.Parse(Txt_number.Text);
            DateTime _currentday = MyUser.GetDareFromText(lblstartdate.Text.ToString());

            string description;
            while (_cnt < _maxlimit)
            {
                if (!Chk_defaultHolidayStatus.Checked)
                {
                    while (MyAttendance.IsDefaultHoliday(_currentday.Date.DayOfWeek))
                    {
                        _currentday = _currentday.AddDays(1);
                    }
                }
                description = txtdescription.Text;
                MyAttendance.SaveDate_In_HolidayTbl(_currentday, description, _type, _ClassId);

                _currentday = _currentday.AddDays(1);

                _cnt = _cnt + 1;
                // MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create Holiday", "Create Holiday on " + MyUser.GerFormatedDatVal(_currentday) + " and reason is " + description + " ", 1, MyAttendance.m_TransationDb);

            }

        }



       



        protected void btn_deletedtls_Click(object sender, EventArgs e)
        {
            DateTime _calSelectDate = MyUser.GetDareFromText(lbl_date.Text);
            int _dateid = MyAttendance.getDateIdFrmMastTbl(_calSelectDate);

            string sql = "DELETE from  tblholiday where tblholiday.dateId=" + _dateid + " and tblholiday.Id=" + lbl_edit_holidayid.Text;
            MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);

            Lbl_msg.Text = "Holiday Details Deleted";
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Delete Holiday", "Delete Holiday on " + lbl_date.Text, 1);
            Load_Calender();
            this.MPE_MessageBox.Show();
        }

        protected void btn_updatedetls_Click(object sender, EventArgs e)
        {
            DateTime _calSelectDate = MyUser.GetDareFromText(lbl_date.Text);
            int _dateid = MyAttendance.getDateIdFrmMastTbl(_calSelectDate);
            if (txt_editdescription.Text.Trim() != "")
            {
                string sql = "update tblholiday set tblholiday.Desc='" + txt_editdescription.Text + "'where tblholiday.dateId=" + _dateid + " and tblholiday.Class_Id= " + int.Parse(lbl_editclass.Text) + " and tblholiday.Type='" + lbl_editmode.Text + "'";

                MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
                Lbl_msg.Text = "Details Updated";
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Update Holiday", "Update Holiday on " + lbl_date.Text, 1);
                this.MPE_MessageBox.Show();
            }
            else
            {
                lblupdateerror.Text = "Enter Description";
                ModalPopupExtender_editdetailsofholiday.Show();
            }

            Load_Calender();
        }



        protected void Btn_cancelediting_Click(object sender, EventArgs e)
        {
            Load_Calender();
        }



        protected void Lnk_DefaultHoliday_Click(object sender, EventArgs e)
        {
            Lbl_Gridmsg.Text = "";
            Btn_DefaultHoliday_update.Visible = true;
            string sql = "SELECT tblholidayconfig.Id,tblholidayconfig.`Day` , tblholidayconfig.`Status`,tblholidayconfig.StaffStatus FROM tblholidayconfig";
            MydataSet = MyAttendance.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_Defaultholiday.Columns[2].Visible = true;
                Grd_Defaultholiday.Columns[3].Visible = true;
                Grd_Defaultholiday.Columns[4].Visible = true;
                Grd_Defaultholiday.DataSource = MydataSet;
                Grd_Defaultholiday.DataBind();
                Grd_Defaultholiday.Columns[2].Visible = false;
                Grd_Defaultholiday.Columns[3].Visible = false;
                Grd_Defaultholiday.Columns[4].Visible = false;
                Load_GridStatus();
            }
            else
            {
                Grd_Defaultholiday.DataSource = null;
                Grd_Defaultholiday.DataBind();

                Lbl_Gridmsg.Text = "Error In Retreiving Data From Database.Try Again";
                Btn_DefaultHoliday_update.Visible = false;
            }

            M_DefaultHoliday.Show();

            Load_Calender();
        }

        private void Load_GridStatus()
        {
            foreach (GridViewRow gv in Grd_Defaultholiday.Rows)
            {
                CheckBox chstud = (CheckBox)gv.FindControl("CheckBoxStudent");
                CheckBox chstaff = (CheckBox)gv.FindControl("CheckBoxStaff");
                if (gv.Cells[2].Text == "1")
                {
                    chstud.Checked = true;
                }
                if (gv.Cells[3].Text == "1")
                {
                    chstaff.Checked = true;
                }
            }
        }

        protected void Btn_DefaultHoliday_update_Click(object sender, EventArgs e)
        {
            int Status = 0,StaffStatus=0;
            string msg = "";
            if (IsSavable(out msg))
            {

                try
                {
                    MyAttendance.CreateTansationDb();

                    foreach (GridViewRow gv in Grd_Defaultholiday.Rows)
                    {
                        Status = 0;
                        StaffStatus = 0;
                        CheckBox chstud = (CheckBox)gv.FindControl("CheckBoxStudent");
                        CheckBox chstaff = (CheckBox)gv.FindControl("CheckBoxStaff");
                        if (chstud.Checked)
                        {
                            Status = 1;
                        }

                        if (chstaff.Checked)
                        {
                            StaffStatus = 1;
                        }

                        UpdateDefaultholiday(gv.Cells[4].Text, Status, StaffStatus);

                    }
                    MyAttendance.EndSucessTansationDb();
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Update Default Holiday", "Update Default Holiday", 1);
                }
                catch
                {
                    MyAttendance.EndFailTansationDb();
                    Lbl_Gridmsg.Text = "Error In Updation";
                    M_DefaultHoliday.Show();
                }
            }
            else
            {
                string[] msgarray = msg.Split('_');
                DateTime dt = new DateTime();
                DateTime.TryParse(msgarray[0], out dt);
                Lbl_Gridmsg.Text = "Attendance is marked for " + msgarray[1] + " on " + MyUser.GerFormatedDatVal(dt) + ", which is " + msgarray[2] + ". Cancel attendance before assigning default holiday on that day.";
                M_DefaultHoliday.Show();
            }

            Load_Calender();


        }

        private bool IsSavable(out string msg)
        {
            msg = "";
            bool vaild = true;
            foreach (GridViewRow gv in Grd_Defaultholiday.Rows)
            {
                CheckBox ch = (CheckBox)gv.FindControl("CheckBoxStudent");
                if (vaild)
                {
                    if (ch.Checked)
                    {

                        if (IsAttendanceDoneOn(gv.Cells[5].Text, out msg))
                        {
                            vaild = false;
                            break;
                        }
                    }
                }
            }
            return vaild;
        }

        private bool IsAttendanceDoneOn(string WeekDay, out string msg)
        {
            msg = "";
            bool attendanceassigned = false;
            string TempClassId = "", sql = "", standard = "";
            if (MyAttendance.IsNewAttendance())
            {
                MydataSet = MyUser.MyAssociatedClass();
                if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in MydataSet.Tables[0].Rows)
                    {
                        TempClassId = dr[0].ToString();
                        standard = MyAttendance.GetStandard_Class(int.Parse(TempClassId));
                        if (!attendanceassigned)
                        {
                            if (MyAttendance.AttendanceTables_Exits(standard, MyUser.CurrentBatchId))
                            {
                                sql = "SELECT t1.`Date`,tblclass.ClassName FROM tblattdcls_std" + standard + "yr" + MyUser.CurrentBatchId + " as t1 INNER JOIN tblclass ON tblclass.Id=t1.ClassId";
                                MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);

                                if (MyReader.HasRows) //means attendance is marked on the date
                                {
                                    while (MyReader.Read())
                                    {
                                        if (DateTime.Parse(MyReader.GetValue(0).ToString()).Date.DayOfWeek.ToString().ToUpperInvariant() == WeekDay.ToUpperInvariant())
                                        {
                                            attendanceassigned = true;
                                            msg = MyReader.GetValue(0).ToString() + "_" + MyReader.GetValue(1).ToString() + "_" + WeekDay;
                                            break;
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            break;
                        }

                    }
                }
            }
            else
            {
                sql = "SELECT tblmasterdate.`date`, tblclass.ClassName FROM tbldate INNER JOIN tblclass ON tblclass.Id=tbldate.classId INNER JOIN tblmasterdate ON tblmasterdate.Id=tbldate.DateId";
                MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);

                if (MyReader.HasRows) //means attendance is marked on the date
                {
                    while (MyReader.Read())
                    {
                        if (DateTime.Parse(MyReader.GetValue(0).ToString()).Date.DayOfWeek.ToString().ToUpperInvariant() == WeekDay.ToUpperInvariant())
                        {
                            attendanceassigned = true;
                            msg = MyReader.GetValue(0).ToString() + "-" + MyReader.GetValue(1).ToString() + "-" + WeekDay;
                            break;
                        }
                    }

                }
            }
            return attendanceassigned;
        }




        private void UpdateDefaultholiday(string Id, int Status, int StaffStatus)
        {
            string Sql = "UPDATE tblholidayconfig SET `Status`=" + Status + ",StaffStatus=" + StaffStatus + " WHERE Id=" + Id;
            MyAttendance.m_TransationDb.ExecuteQuery(Sql);
        }

        # endregion


       # region Code for Managing Filter 

        protected void Lnk_EventFilter_Click(object sender, EventArgs e)
        {

            Pnl_FilterPanel.Visible = Pnl_CalenderPanel.Visible;
            Pnl_CalenderPanel.Visible = !Pnl_CalenderPanel.Visible;
            if (Pnl_FilterPanel.Visible)
            {
                Lnk_EventFilter.Text = "Switch To Calender View";
            }
            else
            {
                Lnk_EventFilter.Text = "Switch To List View";
               
            }
            Load_Calender();
        }

        protected void Drp_FilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtStartDate.Enabled = false;
            TxtEndDate.Enabled = false;
            GetDuration();
        }

        private void GetDuration()
        {
            DateTime today = System.DateTime.Now.Date;
            DateTime SDate = System.DateTime.Now.Date, EDate = System.DateTime.Now.Date;
            int i = int.Parse(Drp_FilterType.SelectedItem.Value);
            TxtStartDate.Text = General.GerFormatedDatVal(SDate);
            TxtEndDate.Text = General.GerFormatedDatVal(EDate);
            if (i == 0)
            {
                SDate = General.GetDateTimeFromText("01/01/" + today.Year);
                EDate = General.GetDateTimeFromText("31/12/" + today.Year);
                TxtStartDate.Text = General.GerFormatedDatVal(SDate);
                TxtEndDate.Text = General.GerFormatedDatVal(EDate);
            }
            if (i == 1)
            {
                SDate = today;
                SDate = Week.FirstDayOfWeek(SDate, DayOfWeek.Monday);
                EDate = SDate.AddDays(6);
                TxtStartDate.Text = General.GerFormatedDatVal(SDate);
                TxtEndDate.Text = General.GerFormatedDatVal(EDate);
            }
            else if (i == 2)
            {
                DateTime sdate1, edate1;
                sdate1 = General.GetDateTimeFromText("01/" + SDate.Month + "/" + SDate.Year);
                edate1 = sdate1.AddMonths(1);
                SDate = sdate1;
                EDate = edate1.AddDays(-1);
                TxtStartDate.Text = General.GerFormatedDatVal(SDate);
                TxtEndDate.Text = General.GerFormatedDatVal(EDate);
            }
            else if (i == 3)
            {
                SDate = today;
                EDate = SDate.AddDays(7);
                SDate = Week.FirstDayOfWeek(EDate, DayOfWeek.Monday);
                EDate = SDate.AddDays(6);
                TxtStartDate.Text = General.GerFormatedDatVal(SDate);
                TxtEndDate.Text = General.GerFormatedDatVal(EDate);
            }
           
            else if (i == 4)
            {
                TxtStartDate.Enabled = true;
                TxtEndDate.Enabled = true;
                TxtStartDate.Text = "";
                TxtEndDate.Text = "";
            }

        }


        protected void Lnk_Export_Click(object sender, EventArgs e)
        {
            string PageName = "EventAndHolidays";
            string _ReportName="Event And Holidays List";
            MydataSet = GetEventListDataSet();

            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                MydataSet.Tables[0].Columns.Remove(MydataSet.Tables[0].Columns[0]);
                MydataSet.Tables[0].Columns.Remove(MydataSet.Tables[0].Columns[2]);

                if (Drp_Type.SelectedValue != "0")
                {
                    MydataSet.Tables[0].Columns.Remove(MydataSet.Tables[0].Columns[1]);
                    PageName = Drp_Type.SelectedItem.Text +"List";
                    _ReportName=Drp_Type.SelectedItem.Text +" List";
                }
                if (!WinEr.ExcelUtility.ExportDataToExcel(MydataSet, _ReportName, PageName, MyUser.ExcelHeader))
                {
                    Lbl_ListGrid_Msg.Text = "Error in exporting to excel";
                }
            }
            else
            {
                Lbl_ListGrid_Msg.Text = "No data found for exporting";
            }
   
        }

        protected void Img_Excel_Click(object sender, ImageClickEventArgs e)
        {
            string PageName = "EventAndHolidays";
            string _ReportName = "Event And Holidays List";
            MydataSet = GetEventListDataSet();

            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                MydataSet.Tables[0].Columns.Remove(MydataSet.Tables[0].Columns[0]);
                MydataSet.Tables[0].Columns.Remove(MydataSet.Tables[0].Columns[2]);

                if (Drp_Type.SelectedValue != "0")
                {
                    MydataSet.Tables[0].Columns.Remove(MydataSet.Tables[0].Columns[1]);
                    PageName = Drp_Type.SelectedItem.Text + "List";
                    _ReportName = Drp_Type.SelectedItem.Text + " List";
                }
                if (!WinEr.ExcelUtility.ExportDataToExcel(MydataSet, _ReportName, PageName, MyUser.ExcelHeader))
                {
                    Lbl_ListGrid_Msg.Text = "Error in exporting to excel";
                }
            }
            else
            {
                Lbl_ListGrid_Msg.Text = "No data found for exporting";
            }
   
        }

        protected void Btn_Load_Click(object sender, EventArgs e)
        {
            Lbl_ListGrid_Msg.Text = "";
            string msg = "";
            if (EventListLoadable(out msg))
            {
                LoadEventList();
            }
            else
            {
                Grid_EventList.DataSource = null;
                Grid_EventList.DataBind();
                Lbl_ListGrid_Msg.Text = msg;
            }
        }

        private bool EventListLoadable(out string msg)
        {
            bool valid = true;
            msg = "";
            if (TxtStartDate.Text.Trim() == "")
            {
                msg = "Enter FROM date";
                valid = false;
            }
            else if (TxtEndDate.Text.Trim() == "")
            {
                msg = "Enter TO date";
                valid = false;
            }
            else
            {
                DateTime StartDate = General.GetDateTimeFromText(TxtStartDate.Text);
                DateTime EndDate = General.GetDateTimeFromText(TxtEndDate.Text);

                if (StartDate > EndDate)
                {
                    msg = "FROM date should not be greater than TO date";
                    valid = false;
                }


            }


            return valid;
        }



        private void LoadEventList()
        {
            Lbl_ListGrid_Msg.Text = "";
            MydataSet = GetEventListDataSet();
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grid_EventList.Columns[0].Visible = true;
                Grid_EventList.Columns[1].Visible = true;
                Grid_EventList.Columns[2].Visible = true;
                Grid_EventList.DataSource = MydataSet;
                Grid_EventList.DataBind();
                Grid_EventList.Columns[0].Visible = false;
                Grid_EventList.Columns[1].Visible = false;
                Grid_EventList.Columns[2].Visible = false;
            }
            else
            {
                Grid_EventList.DataSource = null;
                Grid_EventList.DataBind();
                Lbl_ListGrid_Msg.Text = "No Data Found";
            }
        }

        private DataSet GetEventListDataSet()
        {
            DataSet _eventdataset = new DataSet();
            DataTable dt;
            DataRow dr;
            _eventdataset.Tables.Add(new DataTable("Event"));
            dt = _eventdataset.Tables["Event"];     
            dt.Columns.Add("Id");
            dt.Columns.Add("Date");
            dt.Columns.Add("Type");
            dt.Columns.Add("Divinfo");
            dt.Columns.Add("Name");
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();


            if (TxtStartDate.Text!="" && TxtEndDate.Text!="")
            {
                StartDate = General.GetDateTimeFromText(TxtStartDate.Text);
                EndDate = General.GetDateTimeFromText(TxtEndDate.Text);

                string sqlholiday = "", strunion = "", sqlevent = "";

                if (Drp_Type.SelectedValue == "0")
                {
                    sqlholiday = " select distinct tblmasterdate.`date` as DateValue,tblholiday.Id, tblholiday.`Desc` as NameValue,0 as `CodeValue` from tblmasterdate inner join tblholiday on tblholiday.dateId= tblmasterdate.Id  where ((tblholiday.`Type`='class' and tblholiday.Class_Id=0) or(tblholiday.`Type`='all')) and tblmasterdate.`date` between '" + StartDate.ToString("s") + "' and '" + EndDate.ToString("s") + "'";
                    strunion = " union ";
                    sqlevent = " select distinct tblmasterdate.`date` as DateValue,tbleventmaster.Id, tbleventmaster.EventName as NameValue ,1 as `CodeValue` from tblmasterdate inner join tblcalender_event on tblcalender_event.DateId= tblmasterdate.Id INNER JOIN tbleventmaster ON tbleventmaster.Id=tblcalender_event.EventId WHERE tblmasterdate.`date` between '" + StartDate.ToString("s") + "' and '" + EndDate.ToString("s") + "' order by DateValue ";
                }
                else if (Drp_Type.SelectedValue == "1")
                {
                    sqlevent = " select distinct tblmasterdate.`date` as DateValue,tbleventmaster.Id, tbleventmaster.EventName as NameValue ,1 as `CodeValue` from tblmasterdate inner join tblcalender_event on tblcalender_event.DateId= tblmasterdate.Id INNER JOIN tbleventmaster ON tbleventmaster.Id=tblcalender_event.EventId WHERE tblmasterdate.`date` between '" + StartDate.ToString("s") + "' and '" + EndDate.ToString("s") + "' order by DateValue ";
                }
                else if (Drp_Type.SelectedValue == "2")
                {
                    sqlholiday = " select distinct tblmasterdate.`date` as DateValue,tblholiday.Id, tblholiday.`Desc` as NameValue,0 as `CodeValue` from tblmasterdate inner join tblholiday on tblholiday.dateId= tblmasterdate.Id  where ((tblholiday.`Type`='class' and tblholiday.Class_Id=0) or(tblholiday.`Type`='all')) and tblmasterdate.`date` between '" + StartDate.ToString("s") + "' and '" + EndDate.ToString("s") + "' order by DateValue ";
                }

                string sql = sqlholiday + strunion + sqlevent;
                MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        string StatusText = "Holiday";
                        DateTime _Date = DateTime.Parse(MyReader.GetValue(0).ToString());
                        string Style = "CalenderBackRed";
                        if (MyReader.GetValue(3).ToString() == "1")
                        {
                            Style = "CalenderBackBlue";
                            StatusText = "Event";
                        }
                        string CalenderBackStr = "<div class=\"" + Style + "\">   <table width=\"100%\">  <tr> <td class=\"CalenderTop\" align=\"center\" valign=\"top\">   " + _Date.ToString("MMM").ToUpperInvariant() + "   </td>  </tr>  <tr>  <td class=\"CalenderData\"  align=\"center\" valign=\"middle\">   " + _Date.Date.Day + "  </td>  </tr>  </table> </div>";

                        dr = _eventdataset.Tables["Event"].NewRow();
                        dr["Id"] = MyReader.GetValue(1).ToString();
                        dr["Date"] = General.GerFormatedDatVal(_Date);
                        dr["Type"] = StatusText;
                        dr["Divinfo"] = CalenderBackStr;
                        dr["Name"] = MyReader.GetValue(2).ToString();
                        _eventdataset.Tables["Event"].Rows.Add(dr);
                    }
                }
            }
            return _eventdataset;
        }


        protected void Grid_EventList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Status = Grid_EventList.SelectedRow.Cells[2].Text;
            DateTime SelectedDate =General.GetDateTimeFromText(Grid_EventList.SelectedRow.Cells[1].Text.ToString());
            Hd_SelectedDate.Value = Grid_EventList.SelectedRow.Cells[1].Text.ToString();
            string Id = Grid_EventList.SelectedRow.Cells[0].Text.ToString();
            if (Status == "Holiday")
            {
                string holidaydescription = "", type = "";
                int holidayid = 0;
                if (CheckdayHasAlreadyMarkedAsHoliDay(SelectedDate, "all", 0, out holidaydescription, out holidayid, out type)) // any holiday is assignrd to the selected day
                {
                    Lnk_GotoEvent.Visible = false;
                    lbl_date.Text = General.GerFormatedDatVal(SelectedDate);
                    lbl_edit_holidayid.Text = holidayid.ToString();
                    lbl_editmode.Text = "all";
                    lbl_editclass.Text = "0";

                    txt_editdescription.Text = holidaydescription;

                    lblallclass.Text = Drp_class.SelectedItem.Text;
                    if (type == "all")
                    {
                        lblallclass.Text = "All Class";
                    }

                    ModalPopupExtender_editdetailsofholiday.Show();


                }
            }
            else
            {
                string ClassId = "0", EventName = "", EventId = "", Description = "", Publish = "";


                if (MyAttendance.IsEventPresent(SelectedDate, int.Parse(ClassId), out EventName, out EventId, out Description, out Publish))
                {
                    Lnk_GotoHoliday.Visible = false;
                    Hdn_EventId.Value = EventId;
                    Txt_EditEventName.Text = EventName;
                    Txt_EditEventDescriptionHtml.Content = Description;
                    if (Publish == "1")
                    {
                        Chk_EditEventPublish.Checked = true;
                    }
                    else
                    {
                        Chk_EditEventPublish.Checked = false;
                    }
                    MPE_EditEvent.Show();
                }
            }
        }


        protected void Img_AddEvent_Click(object sender, ImageClickEventArgs e)
        {
            SwitchTo_CalenderView();
        }

      

        protected void Lnk_AddEvent_Click(object sender, EventArgs e)
        {
            SwitchTo_CalenderView();
        }

        protected void Img_AddHoliday_Click(object sender, ImageClickEventArgs e)
        {
            SwitchTo_CalenderView();
        }

        protected void Lnk_AddHoliday_Click(object sender, EventArgs e)
        {
            SwitchTo_CalenderView();
        }
        private void SwitchTo_CalenderView()
        {
            Pnl_FilterPanel.Visible = false;
            Pnl_CalenderPanel.Visible = true;
            Lnk_EventFilter.Text = "Switch To List View";
            TxtStartDate.Text = "";
            TxtEndDate.Text = "";
            Load_Calender();

        }

        protected void Grid_EventList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grid_EventList.PageIndex = e.NewPageIndex;
            LoadEventList();
        }
#endregion



        
       

       

       



        

       




      

    }
}
