using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;
using DayPilot.Utils;

namespace WinEr.WebControls
{
    public partial class TimetableControl : System.Web.UI.UserControl
    {
        private TimeTable MyTimeTable;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        public string ClassId
        {
            get
            {
                return HiddenClassId.Value;
            }
            set
            {
                HiddenClassId.Value = value;
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyTimeTable = MyUser.GetTimeTableObj();
            if (MyTimeTable == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(408))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (!IsPostBack)
                {
                    Load_InitailDetails();
                    

                    //some initlization

                }
            }
        }

       

        public void LoadTimeTableControl(string ClassId)
        {
            HiddenClassId.Value = ClassId;
            Load_InitailDetails();
        }

        private void Load_InitailDetails()
        {
            Load_GridDetails();
            LoadTimeTable();
        }

        private void Load_GridDetails()
        {
            MydataSet = LoadDataSet(HiddenClassId.Value);
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grid_TTConfig.DataSource = MydataSet;
                Grid_TTConfig.DataBind();
            }
            else
            {
                Grid_TTConfig.DataSource = null;
                Grid_TTConfig.DataBind();
            }
            Load_AdditionalDetails();
        }

        private void Load_AdditionalDetails()
        {
            int TotalScheduledCount = 0,AllotedCount=0,FreePeriod=0;
            foreach (GridViewRow gv in Grid_TTConfig.Rows)
            {
                TotalScheduledCount = TotalScheduledCount + int.Parse(gv.Cells[2].Text);
                AllotedCount = AllotedCount + int.Parse(gv.Cells[3].Text);
            }
            FreePeriod = TotalScheduledCount - AllotedCount;
            Lbl_ConfiguredPeriods.Text = TotalScheduledCount.ToString();
            Lbl_AllotedPeriods.Text = AllotedCount.ToString();
            Lbl_FreePeriods.Text = FreePeriod.ToString();
        }

        private DataSet LoadDataSet(string _ClassId)
        {
            bool PeriodExists = false;
            string ClassSubMapId = "", SubjectsId = "", SubjectName = "",StaffName="No Staff Assigned";
            int StaffId = 0, PeriodCount = 0,AllotedCount=0;
            DataSet _Subdataset = new DataSet();
            DataTable dt;
            DataRow dr;

            _Subdataset.Tables.Add(new DataTable("Subjects"));
            dt = _Subdataset.Tables["Subjects"];
            dt.Columns.Add("Subject");
            dt.Columns.Add("Staff");
            dt.Columns.Add("NoPeriods");
            dt.Columns.Add("AllotedPeriods");

            string sql = "SELECT tblclasssubmap.Id,tblclasssubmap.SubjectId,tblsubjects.subject_name,tbltime_subgroup.MinPeriodWeek FROM tblclasssubmap INNER JOIN tblsubjects ON tblsubjects.Id=tblclasssubmap.SubjectId INNER JOIN tbltime_subgroup ON tbltime_subgroup.Id=tblsubjects.sub_Catagory WHERE tblclasssubmap.ClassId=" + _ClassId;
            OdbcDataReader OuterReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            if (OuterReader.HasRows)
            {
                while (OuterReader.Read())
                {
                    StaffId = 0;
                    PeriodCount = 0;
                    AllotedCount = 0;
                    StaffName = "No Staff Assigned";
                    PeriodExists = false;
                    ClassSubMapId = OuterReader.GetValue(0).ToString();
                    SubjectsId = OuterReader.GetValue(1).ToString();
                    SubjectName = OuterReader.GetValue(2).ToString().Replace("amp;", "");
                    int.TryParse(OuterReader.GetValue(3).ToString(), out PeriodCount);
                    sql = "SELECT tbltime_classsubjectstaff.StaffId,tbltime_classsubjectstaff.PeriodCount,tbluser.SurName FROM tbltime_classsubjectstaff INNER JOIN tbluser ON tbluser.Id=tbltime_classsubjectstaff.StaffId  WHERE tbltime_classsubjectstaff.ClassSubjectId=" + ClassSubMapId;
                    OdbcDataReader InnerReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
                    if (InnerReader.HasRows)
                    {
                        while (InnerReader.Read())
                        {
                            StaffId = 0;

                            if (int.TryParse(InnerReader.GetValue(0).ToString(), out StaffId) && int.TryParse(InnerReader.GetValue(1).ToString(), out PeriodCount))
                            {
                                StaffName = InnerReader.GetValue(2).ToString();
                                AllotedCount =MyTimeTable.GetAllotedPeriodCount(_ClassId, SubjectsId,StaffId);
                                dr = _Subdataset.Tables["Subjects"].NewRow();
                                dr["Subject"] = SubjectName;
                                dr["Staff"] = StaffName;
                                dr["NoPeriods"] = PeriodCount;
                                dr["AllotedPeriods"] = AllotedCount;
                                _Subdataset.Tables["Subjects"].Rows.Add(dr);
                                PeriodExists = true;
                            }


                        }
                    }


                    if (!PeriodExists)
                    {
                        AllotedCount = MyTimeTable.GetAllotedPeriodCount(_ClassId, SubjectsId, StaffId);
                        dr = _Subdataset.Tables["Subjects"].NewRow();
                        dr["Subject"] = SubjectName;
                        dr["Staff"] = StaffName;
                        dr["NoPeriods"] = PeriodCount;
                        dr["AllotedPeriods"] = AllotedCount;
                        _Subdataset.Tables["Subjects"].Rows.Add(dr);
                    }

                }
            }

            return _Subdataset;
        }

       


        private void LoadTimeTable()
        {
            DayPilot_TimeTable.Resources.Clear();
            DayPilot_TimeTable.DataSource = null;
            DayPilot_TimeTable.DataBind();

            DataSet Days = new DataSet();
            //DayPilotScheduler1.StartDate = Week.FirstDayOfWeek(DateTime.Today, DayOfWeek.Monday); Week.FirstDayOfWeek();
            AddResources(ref  Days);
            DateTime Day = new DateTime(2010, 3, 1, 0, 0, 0);

            DayPilot_TimeTable.StartDate = Week.FirstDayOfWeek(Day, DayOfWeek.Monday);

            DayPilot_TimeTable.DataSource = getData(ref Days);
            DataBind();
        }

        private void AddResources(ref DataSet Days)
        {
            Days = MyTimeTable.GetDays();
            if (Days != null && Days.Tables != null && Days.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Dr_Day in Days.Tables[0].Rows)
                {
                    DayPilot_TimeTable.Resources.Add(Dr_Day[0].ToString(), Dr_Day[1].ToString());
                }
            }
        }

        protected DataTable getData(ref DataSet _days)
        {

            int columnCount = 0;
            DataTable dt;
            DataSet Periods = new DataSet();
            dt = new DataTable();
            string StaffName = "";
            string Subjectname = "";

            dt.Columns.Add("start", typeof(DateTime));
            dt.Columns.Add("end", typeof(DateTime));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("resource", typeof(string));
            dt.Columns.Add("barColor", typeof(string));
            DataRow dr;
            string sql = "";
            Periods = GetAllPeriodsAvaililable_Week_Class(int.Parse(HiddenClassId.Value));
            for (int k = 0; k < _days.Tables[0].Rows.Count; k++)
            {
                string DayId = _days.Tables[0].Rows[k][1].ToString(); 
                if (Periods != null && Periods.Tables != null && Periods.Tables[0].Rows.Count > 0)
                {
                    if (columnCount < Periods.Tables[0].Rows.Count)
                    {
                        columnCount = Periods.Tables[0].Rows.Count;
                    }

                    for (int j = 0; j < Periods.Tables[0].Rows.Count; j++)
                    {
                        int ClassPeriodId = 0;
                        DataRow Dr_Period = Periods.Tables[0].Rows[j];
                        if (MyTimeTable.IsperiodAvailable_Class(Dr_Period[0].ToString(), int.Parse(HiddenClassId.Value), DayId, out ClassPeriodId))
                        {
                            sql = "select SubjectId from tbltime_master where ClassPeriodId=" + ClassPeriodId;
                            MyReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
                            if (MyReader.HasRows)
                            {
                                dr = dt.NewRow();
                                StaffName = MyTimeTable.GetStaffName(ClassPeriodId.ToString());
                                Subjectname = MyTimeTable.GetSubjectName(MyReader.GetValue(0).ToString());
                                dr["id"] = ClassPeriodId + "\\" + StaffName + "\\" + System.Environment.NewLine + "" + Subjectname + "\\" + DayId + "\\" + Dr_Period[0].ToString();
                                DateTime Day = new DateTime(2010, 3, 1, 0, 0, 0);
                                dr["start"] = Day.AddDays(j);
                                dr["end"] = Day.AddDays(j + 1);
                                dr["name"] = Subjectname + " \\ " + StaffName;
                                dr["resource"] = DayId;
                                if (Subjectname == "Free")
                                {
                                    dr["barColor"] = "#fff4cc";//free
                                }
                                else
                                {
                                    dr["barColor"] = "#e1fec7";
                                }
                                dt.Rows.Add(dr);

                            }
                            else
                            {
                                dr = dt.NewRow();
                                StaffName = "";
                                Subjectname = "Free";
                                dr["id"] = ClassPeriodId + "\\" + StaffName + "\\" + Subjectname + "\\" + DayId + "\\" + Dr_Period[0].ToString();
                                DateTime Day = new DateTime(2010, 3, 1, 0, 0, 0);
                                dr["start"] = Day.AddDays(j);
                                dr["end"] = Day.AddDays(j + 1);
                                dr["name"] = " Free\\ ";
                                dr["resource"] = DayId;
                                dr["barColor"] = "#fff4cc";//free
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }
            }
            DayPilot_TimeTable.Days = columnCount;
            return dt;
        }

        private DataSet GetAllPeriodsAvaililable_Week_Class(int ClassId)
        {
            string sql = "select DISTINCT PeriodId from tbltime_classperiod where ClassId=" + ClassId + " order by PeriodId";
            DataSet Periods = MyTimeTable.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return Periods;
        }

        protected void TimeTPeriodClick(object sender, DayPilot.Web.Ui.EventClickEventArgs e)
        {
            lbl_SchError.Text = "";
            Hdn_ClassPeriodId.Value = "";
            Hdn_Staff.Value = "";
            Hdn_Subject.Value = "";
            string value = e.Value;
            string ClassPeriodId;
            string Staff = "";
            string Subject = "";
            string[] Array = value.Split('\\');
            if (Array.Length == 1)
                ClassPeriodId = Array[0];
            if (Array.Length == 5)
            {
                ClassPeriodId = Array[0];
                Staff = Array[1];
                Subject = Array[2];
                Hdn_ClassPeriodId.Value = ClassPeriodId;
                Hdn_Subject.Value = Subject;
                Hdn_Staff.Value = Staff;
                Hdn_DayId.Value = Array[3];
                Hdn_Period.Value = Array[4];
            }
            SelectAction(Staff, Subject);
        }


        private void SelectAction(string _StaffName, string _Subject)
        {
            if (_StaffName == "")
            {
                LoadSubjectToDrpList();
                LoadStaffToDrpList(int.Parse(Drp_Subject.SelectedValue));
                MPE_SchedulePeriod.Show();
            }
            else
            {
                Lbl_SelectedStaff.Text = _StaffName;
                Lbl_SelectedSubject.Text = _Subject;
                MPE_FreePeriod.Show();
            }
        }


        protected void Drp_Subject_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStaffToDrpList(int.Parse(Drp_Subject.SelectedValue));
            MPE_SchedulePeriod.Show();
        }


        private void LoadStaffToDrpList(int _SubjectId)
        {
            Drp_Staff.Items.Clear();
            if (_SubjectId == -1)
            {
                ListItem li = new ListItem("No staff present", "-1");
                Drp_Staff.Items.Add(li);
            }
            else
            {
                string sql = "select tbluser.Id , tbluser.SurName from tblstaffsubjectmap inner join tbluser on tbluser.Id = tblstaffsubjectmap.StaffId where tbluser.Id in (select tblclassstaffmap.StaffId from tblclassstaffmap where tblclassstaffmap.ClassId=" + int.Parse(HiddenClassId.Value) + ") and tblstaffsubjectmap.SubjectId=" + _SubjectId;
                MyReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_Staff.Items.Add(li);
                    }
                }
                else
                {
                    ListItem li = new ListItem("No staff present", "-1");
                    Drp_Staff.Items.Add(li);
                }
            }
        }


        protected void Btn_CancelPeriod_Save(object sender, EventArgs e)
        {
            MyTimeTable.DeleteTimeTableEntry(Hdn_ClassPeriodId.Value);
            Load_InitailDetails();

        }


        protected void Btn_SchedulePeriod_Click(object sender, EventArgs e)
        {
            string msg = "";
            lbl_SchError.Text = "";
            if ((Drp_Subject.SelectedValue != "-1") && (Drp_Staff.SelectedValue != "-1"))//assign subject to free period
            {

                if (MyTimeTable.IsTimeTableEntryPossible(Hdn_ClassPeriodId.Value, Drp_Staff.SelectedValue, out msg))
                {
                    MyTimeTable.InsertMasterEntry(Hdn_ClassPeriodId.Value, Drp_Subject.SelectedValue, Drp_Staff.SelectedValue, HiddenClassId.Value);
                }
                else
                {
                    lbl_SchError.Text = msg;
                    MPE_SchedulePeriod.Show();
                    //MyTimeTable.UpdateTimetableentry(Hdn_ClassPeriodId.Value, Drp_Subject.SelectedValue, Drp_Staff.SelectedValue, 1);

                }
                Load_InitailDetails();
            }

        }





        private void LoadSubjectToDrpList()
        {
            Drp_Subject.Items.Clear();
            string sql = "select tblsubjects.Id , tblsubjects.subject_name from tblclasssubmap inner join tblsubjects on tblsubjects.Id = tblclasssubmap.SubjectId where tblclasssubmap.ClassId=" + int.Parse(HiddenClassId.Value);
            MyReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Subject.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No subject present", "-1");
                Drp_Subject.Items.Add(li);
            }
        }

    }
}