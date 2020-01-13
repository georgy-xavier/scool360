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
using System.Data.Odbc;
using WinBase;
using System.Drawing;

namespace WinEr
{
    public partial class Timetableinterchange : System.Web.UI.Page
    {
        private StaffManager MyStaffMang;
        private TimeTable MyTimeTable;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
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
            if (Session["StaffId"] == null)
            {
                Response.Redirect("ViewStaffs.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStaffMang = MyUser.GetStaffObj();
            MyTimeTable = MyUser.GetTimeTableObj();
            if (MyStaffMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (MyTimeTable == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(921))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {


                if (!IsPostBack)
                {
                    string _SubMenuStr;
                    _SubMenuStr = MyStaffMang.GetSubStaffMangMenuString(MyUser.UserRoleId);
                    this.SubStaffMenu.InnerHtml = _SubMenuStr;
                    //some initlization
                    LoaduserTopData();
                    WorkloadVisiibility(false);
                    LoadStaffTimetable();
                    
                    LoadStaffDrp();
                }
            }

        }

        private void LoadStaffDrp()
        {
            Drp_Staff.Items.Clear();
           
            string sql = "select distinct tbluser.Id,tbluser.SurName from tbluser inner join tblrole on tblrole.Id = tbluser.RoleId where tbluser.`Status`=1 and tblrole.Type='staff' and tbluser.Id<>" + Session["StaffId"].ToString();
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_Staff.Items.Add(new ListItem("Select Staff", "-1"));
                while (MyReader.Read())
                {
                    Drp_Staff.Items.Add(new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString()));
                }
            }
            else
            {
                Drp_Staff.Items.Add(new ListItem("No other staff found", "-1"));
            }
        }

       

        private void LoaduserTopData()
        {

            string _Studstrip = MyStaffMang.ToStripString(int.Parse(Session["StaffId"].ToString()),"Handler/ImageReturnHandler.ashx?id=" +int.Parse(Session["StaffId"].ToString()) + "&type=StaffImage" );
            this.StudentTopStrip.InnerHtml = _Studstrip;
        }

        private void LoadStaffTimetable()
        {
            lbl_message.Text = "";
            string sql = "SELECT tbltime_classperiod.ClassId as ClassId,tbltime_week.Id as DayId,tblattendanceperiod.PeriodId as PeriodId,tblsubjects.Id as SubjectId,tbltime_week.Name as Day,tblattendanceperiod.FrequencyName as Period,tblsubjects.subject_name as Subject,tblclass.ClassName,tbltime_master.ClassPeriodId,0 as Availability FROM tbltime_master INNER JOIN tbltime_classperiod ON tbltime_classperiod.Id = tbltime_master.ClassPeriodId INNER JOIN tbltime_week ON tbltime_week.Id= tbltime_classperiod.DayId INNER JOIN tblattendanceperiod ON tblattendanceperiod.PeriodId= tbltime_classperiod.PeriodId AND tblattendanceperiod.ModeId=3 INNER JOIN tblsubjects ON tblsubjects.Id= tbltime_master.SubjectId INNER JOIN tblclass ON tblclass.Id=tbltime_classperiod.ClassId WHERE tbltime_master.StaffId=" + Session["StaffId"].ToString() + " ORDER BY tbltime_week.Id,tblattendanceperiod.PeriodId";
            MydataSet = MyStaffMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_StaffTimetable.Columns[0].Visible = true;
                Grd_StaffTimetable.Columns[1].Visible = true;
                Grd_StaffTimetable.Columns[2].Visible = true;
                Grd_StaffTimetable.Columns[3].Visible = true;
                Grd_StaffTimetable.Columns[4].Visible = true;
                Grd_StaffTimetable.Columns[9].Visible = true;

                Grd_StaffTimetable.DataSource = MydataSet;
                Grd_StaffTimetable.DataBind();

                Grd_StaffTimetable.Columns[0].Visible = false;
                Grd_StaffTimetable.Columns[1].Visible = false;
                Grd_StaffTimetable.Columns[2].Visible = false;
                Grd_StaffTimetable.Columns[3].Visible = false;
                Grd_StaffTimetable.Columns[4].Visible = false;

                if (Hd_Availability.Value == "1")
                {
                    CheckStaffAvailability_Period();
                }
                else
                {
                    Grd_StaffTimetable.Columns[9].Visible = false;
                }
            }
            else
            {
                Grd_StaffTimetable.DataSource = null;
                Grd_StaffTimetable.DataBind();
                lbl_message.Text = "No Timetable set for the staff";

            }

        }

        private void CheckStaffAvailability_Period()
        {
            bool _countinue = true;
            string StaffId = Drp_Staff.SelectedValue;
            foreach (GridViewRow gv in Grd_StaffTimetable.Rows)
            {
                string _msg = "";
                string ClassId = gv.Cells[0].Text;
                string SubjectId = gv.Cells[1].Text;
                string DayId = gv.Cells[2].Text;
                string PeriodId = gv.Cells[3].Text;
                string ClassPeriodId = gv.Cells[4].Text;
                if (MyTimeTable.IsTimeTableEntryPossible(ClassPeriodId, StaffId, out _msg))
                {
                    gv.Cells[9].Text = "FREE";
                    gv.Cells[9].Font.Bold = true;
                    gv.Cells[9].ForeColor = Color.Green;
                }
                else
                {
                    gv.Cells[9].Text = "NOT AVAILABALE";
                    gv.Cells[9].Font.Bold = true;
                    gv.Cells[9].ForeColor = Color.Red;
                    _countinue = false;
                }

            }


            if (_countinue)
            {
                lbl_2.Visible = true;
                Btn_Assign.Visible = true;
            }
        }

        protected void Drp_Staff_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_message.Text = "";
            WorkloadVisiibility(false);
            string _msg = "";
            if (_IsVaildSelection(out _msg))
            {
                WorkloadVisiibility(true);
                lbl_workload.Text = MyTimeTable.GetStaff_PeriodCount(Drp_Staff.SelectedValue).ToString();
                LoadStaffTimetable();
            }
            else
            {
                lbl_message.Text = _msg;
            }
        }

        private void WorkloadVisiibility(bool _visibility)
        {
            lbl_2.Visible = false;
            Btn_Assign.Visible = false;
            lbl_1.Visible = _visibility;
            lbl_workload.Visible = _visibility;
            Hd_Availability.Value = "0";
            if (_visibility)
            {
                Hd_Availability.Value = "1";
            }
        }

        private bool _IsVaildSelection(out string _msg)
        {
            _msg = "";
            bool _valid = true;

            if (Drp_Staff.SelectedValue == "-1")
            {
                _valid = false;
                _msg = "Please select staff";
            }

            return _valid;
        }

        protected void Btn_Assign_Click(object sender, EventArgs e)
        {
            string OldStaffId = Session["StaffId"].ToString();
            string NewStaffId = Drp_Staff.SelectedValue;
            try
            {
                MyTimeTable.CreateTansationDb();


                foreach (GridViewRow gv in Grd_StaffTimetable.Rows)
                {
                    string ClassId = gv.Cells[0].Text;
                    string SubjectId = gv.Cells[1].Text;
                    string DayId = gv.Cells[2].Text;
                    string PeriodId = gv.Cells[3].Text;
                    string ClassPeriodId = gv.Cells[4].Text;

                    Assign_NewStaff_Timetable(NewStaffId, ClassId, SubjectId, DayId, PeriodId, ClassPeriodId);
                    Resign_OldStaff_Timetable(OldStaffId, ClassId, SubjectId, DayId, PeriodId, ClassPeriodId);

                }
                MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Staff Timetable", " Timetable assigned to new staff", 1,1);
                Lbl_msgpopup.Text = "Timetable has been successfully assigned to new staff";
                MPE_MessageBox.Show();
                MyTimeTable.EndSucessTansationDb();
                WorkloadVisiibility(false);
                LoadStaffTimetable();
            }
            catch
            {
                MyTimeTable.EndFailTansationDb();
                Lbl_msgpopup.Text = "Error while assigning timetable to new staff. Please try later.";
                MPE_MessageBox.Show();
                
            }
            //MyTimeTable.AddSubjectsToStaff
        }

        private void Resign_OldStaff_Timetable(string OldStaffId, string ClassId, string SubjectId, string DayId, string PeriodId, string ClassPeriodId)
        {
            MyTimeTable.RemoveSubjectFromStaff(OldStaffId, SubjectId);
            RemoveClassFromStaff_WithSubject(OldStaffId,ClassId, OldStaffId);
            int ClassSubId=MyTimeTable.GetClassSubjectMap(SubjectId, ClassId);
            MyTimeTable.DeleteClassSubjectStaff_ParticularStaff(ClassSubId, int.Parse(OldStaffId));

        }

        private void Assign_NewStaff_Timetable(string NewStaffId, string ClassId, string SubjectId, string DayId, string PeriodId, string ClassPeriodId)
        {
            if (!isStaffsubjectmapExists(NewStaffId, SubjectId))
            {
                MyTimeTable.AddSubjectsToStaff(int.Parse(SubjectId), int.Parse(NewStaffId));
            }

            if (!isClassToStaff_SubjectExists(NewStaffId, ClassId, SubjectId))
            {
                AssignClassToStaff_WithSubject(NewStaffId, ClassId, SubjectId);
            }
            int _PeriodCount = 0;
            int ClassSubId=MyTimeTable.GetClassSubjectMap(SubjectId, ClassId);
            if (Is_Timetable_ClassToStaff_SubjectExists(NewStaffId, ClassSubId, out _PeriodCount))
            {
                string _PeriodCount_str = (_PeriodCount + 1).ToString();
                Update_ClassSubjectStaff_Entry(ClassSubId.ToString(), NewStaffId, _PeriodCount_str);
            }
            else
            {
                InsertClassSubjectStaff_Entry(ClassSubId.ToString(), NewStaffId, "1");
            }

            MyTimeTable.UpdateTimetableentry(ClassPeriodId, SubjectId, NewStaffId, 1);
        }

       

        

        private void AssignClassToStaff_WithSubject(string _StaffId, string _ClassId, string _SubjectId)
        {
            string sql = "INSERT INTO tblclassstaffmap(ClassId,SubjectId,StaffId) VALUES (" + _ClassId + "," + _SubjectId + ", " + _StaffId + ")";
            MyTimeTable.m_TransationDb.ExecuteQuery(sql);
        }

        private void RemoveClassFromStaff_WithSubject(string _StaffId, string _ClassId, string _SubjectId)
        {
            string sql = "DELETE FROM tblclassstaffmap WHERE ClassId=" + _ClassId + " and SubjectId=" + _SubjectId + " and StaffId=" + _StaffId;
            if (MyTimeTable.m_TransationDb != null)
            {
                MyTimeTable.m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            }
        }

        private void Update_ClassSubjectStaff_Entry(string ClassSubId, string StaffId, string PeriodCount)
        {
            string sql = "update tbltime_classsubjectstaff set PeriodCount=" + PeriodCount + "  where  ClassSubjectId=" + ClassSubId + " and StaffId=" + StaffId;
            MyTimeTable.m_TransationDb.ExecuteQuery(sql);
        }

        private void InsertClassSubjectStaff_Entry(string ClassSubId, string StaffId, string PeriodCount)
        {
            string sql = "insert into  tbltime_classsubjectstaff (ClassSubjectId,StaffId,PeriodCount) values(" + ClassSubId + " , " + StaffId + " ," + PeriodCount + ")";
            MyTimeTable.m_TransationDb.ExecuteQuery(sql);
        }

        private bool Is_Timetable_ClassToStaff_SubjectExists(string NewStaffId, int ClassSubId, out int _PeriodCount)
        {
            _PeriodCount = 0;
            bool _valid = false;
            string sql = "SELECT tbltime_classsubjectstaff.PeriodCount FROM tbltime_classsubjectstaff WHERE tbltime_classsubjectstaff.StaffId=" + NewStaffId + " AND tbltime_classsubjectstaff.ClassSubjectId=" + ClassSubId;
            OdbcDataReader _myreader = MyTimeTable.m_TransationDb.ExecuteQuery(sql);
            if (_myreader.HasRows)
            {

                int.TryParse(_myreader.GetValue(0).ToString(), out _PeriodCount);

                if (_PeriodCount > 0)
                {
                    _valid = true;
                }
            }
            return _valid;
        }


        private bool isClassToStaff_SubjectExists(string NewStaffId, string ClassId, string SubjectId)
        {
            bool _valid = false;
            string sql = "SELECT COUNT(tblclassstaffmap.StaffId) FROM tblclassstaffmap WHERE tblclassstaffmap.StaffId=" + NewStaffId + " AND tblclassstaffmap.ClassId="+ClassId+" AND tblclassstaffmap.SubjectId=" + SubjectId;
            OdbcDataReader _myreader = MyTimeTable.m_TransationDb.ExecuteQuery(sql);
            if (_myreader.HasRows)
            {
                int _count = 0;
                int.TryParse(_myreader.GetValue(0).ToString(), out _count);

                if (_count > 0)
                {
                    _valid = true;
                }
            }
            return _valid;
        }

        private bool isStaffsubjectmapExists(string NewStaffId, string SubjectId)
        {
            bool _valid = false;
            string sql = "SELECT COUNT(tblstaffsubjectmap.StaffId) FROM tblstaffsubjectmap WHERE tblstaffsubjectmap.StaffId=" + NewStaffId + " AND tblstaffsubjectmap.SubjectId=" + SubjectId;
            OdbcDataReader _myreader = MyTimeTable.m_TransationDb.ExecuteQuery(sql);
            if (_myreader.HasRows)
            {
                int _count = 0;
                int.TryParse(_myreader.GetValue(0).ToString(), out _count);

                if (_count > 0)
                {
                    _valid = true;
                }
            }
            return _valid;
        }

 

       
    }
}
