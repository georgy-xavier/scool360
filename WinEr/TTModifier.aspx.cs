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
    public partial class TTModifier : System.Web.UI.Page
    {
        private ClassOrganiser MyClassMang;
        private TimeTable MyTimeTable;
        private Attendance MyAttendance;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyTimeTable = MyUser.GetTimeTableObj();
            MyClassMang = MyUser.GetClassObj();
            MyAttendance = MyUser.GetAttendancetObj();
            if (MyTimeTable == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MyClassMang == null)
            {
                Response.Redirect("sectionerr.htm");
                //no rights for this user.
            }
            else if (MyAttendance == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(632))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                
                {
                    Session["SortExpression1"] = null;

                    Session["SortDirection1"] = null;
                    bool Valid = false;
                    string Id = "";
                    if (Request.QueryString["Id"] != null)
                    {
                        Id = Request.QueryString["Id"].ToString();
                        string[] StrArray = Id.Split('$');
                        Hd_DateString.Value = StrArray[0];
                        Hd_ClassId.Value = StrArray[1];
                        Hd_PeriodId.Value = StrArray[2];
                        Hd_HolidayStatus.Value = StrArray[3];
                        Valid = true;

                    }

                    StoreInitialValues();

                }


            }
        }

        private void AddClassNameToDrpList()
        {
            Drp_Subject.Items.Clear();
            string sql = "SELECT tblsubjects.Id,tblsubjects.subject_name FROM tblsubjects";
            MydataSet = MyTimeTable.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                ListItem li = new ListItem("Free Period", "0");
                Drp_Subject.Items.Add(li);
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {

                     li = new ListItem(dr[1].ToString(), dr[0].ToString());
                     Drp_Subject.Items.Add(li);

                }

            }
            else
            {
                ListItem li = new ListItem("No subject found", "-1");
                Drp_Subject.Items.Add(li);
            }
            Drp_Subject.SelectedIndex = 0;
        }

        private void StoreInitialValues()
        {
            AddClassNameToDrpList();
            int staffId = 0, SubjectId = 0;
            Lbl_TempSetting.Visible =false;
            Lnk_DeleteTempSetting.Visible = false;
            lbl_Class.Text = MyClassMang.GetClassname(int.Parse(Hd_ClassId.Value));
            lbl_Day.Text = Hd_DateString.Value;
            Lbl_Period.Text = MyTimeTable.getPeriodName(int.Parse(Hd_PeriodId.Value));
            bool GeneralBool = false;
            if (MyTimeTable.GetClassPeriodDetails_Weekly(int.Parse(Hd_ClassId.Value), int.Parse(Hd_PeriodId.Value), General.GetDateTimeFromText(Hd_DateString.Value), out staffId, out SubjectId, out GeneralBool))
            {
                Lbl_TempSetting.Visible = !GeneralBool;
                Lnk_DeleteTempSetting.Visible = !GeneralBool;
            }
            if (Hd_HolidayStatus.Value == "0" || !GeneralBool)
            {
                Hd_StaffId.Value = staffId.ToString();
                lbl_staff.Text = MyUser.getStaffName(staffId);
                if (lbl_staff.Text.Trim() == "")
                {
                    lbl_staff.Text = "None";
                }
                lbl_Subject.Text = MyTimeTable.GetSubjectName(SubjectId.ToString());
                if (lbl_Subject.Text.Trim() == "")
                {
                    lbl_Subject.Text = "Free";
                }
                Drp_Subject.SelectedValue = SubjectId.ToString();
            }
            else
            {
                staffId = -1;
                SubjectId = 0;
                Hd_StaffId.Value = staffId.ToString();
                lbl_staff.Text = MyUser.getStaffName(staffId);
                if (lbl_staff.Text.Trim() == "")
                {
                    lbl_staff.Text = "None";
                }
                lbl_Subject.Text = MyTimeTable.GetSubjectName(SubjectId.ToString());
                if (lbl_Subject.Text.Trim() == "")
                {
                    lbl_Subject.Text = "Free";
                }
                Drp_Subject.SelectedValue = SubjectId.ToString();
            }
        }

        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "max();", true);
            MydataSet = GetStaffDataSet_Grid();
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grid_Staff.Columns[0].Visible = true;
                Grid_Staff.DataSource = MydataSet;
                Grid_Staff.DataBind();
                Grid_Staff.Columns[0].Visible = false;
                GridColoring();
                Lbl_Error.Text = "";
            }
            else
            {
                Grid_Staff.DataSource = null;
                Grid_Staff.DataBind();
                Lbl_Error.Text = "No staff available for the selected period";
            }

        }

        private void GridColoring()
        {
            foreach (GridViewRow gv in Grid_Staff.Rows)
            {
                if (gv.Cells[2].Text == "YES")
                {
                    gv.Cells[2].ForeColor =System.Drawing.Color.Green;
                }
                else if (gv.Cells[2].Text == "NO")
                {
                    gv.Cells[2].ForeColor = System.Drawing.Color.Red;
                }



                if (gv.Cells[3].Text == "YES")
                {
                    gv.Cells[3].ForeColor = System.Drawing.Color.Green;
                }
                else if (gv.Cells[3].Text == "NO")
                {
                    gv.Cells[3].ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        private DataSet GetStaffDataSet_Grid()
        {
                
            string StaffId = "", StaffName = "";
            DataSet _Subdataset = new DataSet();
            DataTable dt;
            DataRow dr;
            _Subdataset.Tables.Add(new DataTable("Staff"));
            dt = _Subdataset.Tables["Staff"];
            dt.Columns.Add("StaffId");
            dt.Columns.Add("Staff");
            dt.Columns.Add("Assigned For Class");
            dt.Columns.Add("Assigned For Subject");
          
             
           
            MydataSet = GetStaffDataSet();
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow gv in MydataSet.Tables[0].Rows)
                {
                    string Assign_Class = "NO";
                    string Assign_Subject = "NO";
                    StaffId = gv[0].ToString();
                    StaffName = gv[1].ToString();
                    if (Is_StaffAssigned_Class(StaffId))
                    {
                        Assign_Class = "YES";
                    }
                    if (Is_StaffAssigned_Subject(StaffId))
                    {
                        Assign_Subject = "YES";
                    }
                    
                    dr = _Subdataset.Tables["Staff"].NewRow();
                    dr["StaffId"] = StaffId;
                    dr["Staff"] = StaffName;
                    dr["Assigned For Class"] = Assign_Class;
                    dr["Assigned For Subject"] = Assign_Subject;
                    _Subdataset.Tables["Staff"].Rows.Add(dr);



                }
            }
            return _Subdataset;
        }

        private bool Is_StaffAssigned_Subject(string StaffId)
        {
            int count = 0;
            bool valid = false;
            string sql = "SELECT COUNT(tblstaffsubjectmap.SubjectId) FROM tblstaffsubjectmap WHERE tblstaffsubjectmap.StaffId="+StaffId+" AND tblstaffsubjectmap.SubjectId=" + Drp_Subject.SelectedValue;
            MyReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out count);
                if (count > 0)
                {
                    valid = true;
                }
            }
            return valid;
        }

        private bool Is_StaffAssigned_Class(string StaffId)
        {
            int count = 0;
            bool valid = false;
            string sql = "SELECT COUNT(tblclassstaffmap.ClassId) FROM tblclassstaffmap WHERE tblclassstaffmap.StaffId="+StaffId+" AND tblclassstaffmap.ClassId="+Hd_ClassId.Value;
            MyReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out count);
                if (count > 0)
                {
                    valid = true;
                }
            }
            return valid;
        }

        private DataSet GetStaffDataSet()
        {
            DateTime Day = General.GetDateTimeFromText(Hd_DateString.Value);
            bool Completed = false;
            string SubjectId = Drp_Subject.SelectedValue;    
            string sql = "";
            if (SubjectId == "0")
            {
                sql = "select distinct tbluser.Id,tbluser.SurName from tbluser inner join tblrole on tblrole.Id = tbluser.RoleId where tbluser.`Status`=1 and tblrole.Type='staff'";
                MydataSet = MyTimeTable.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            }
            else
            {
                if (Chk_All.Checked)
                {
                    sql = "SELECT DISTINCT tbluser.Id,tbluser.SurName FROM tbluser inner join tblrole on tblrole.Id = tbluser.RoleId WHERE tbluser.`Status`=1 AND tblrole.Type='staff' AND tbluser.Id!=" + Hd_StaffId.Value + " AND tbluser.Id not in (SELECT tbltime_master.StaffId FROM tbltime_master INNER JOIN tbltime_classperiod ON tbltime_classperiod.Id=tbltime_master.ClassPeriodId INNER JOIN tbltime_week ON tbltime_week.Id=tbltime_classperiod.DayId WHERE lower(tbltime_week.Name)='" + Day.Date.DayOfWeek + "' AND tbltime_classperiod.PeriodId=" + Hd_PeriodId.Value + ")";
                    MydataSet = MyTimeTable.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
                    {
                        Completed = true;
                    }
                }
                if (!Completed)
                {
                    sql = "SELECT DISTINCT tblclassstaffmap.StaffId,tbluser.SurName FROM tblclassstaffmap INNER JOIN tbluser ON tbluser.Id=tblclassstaffmap.StaffId WHERE tblclassstaffmap.SubjectId=" + SubjectId + " AND tblclassstaffmap.ClassId=" + Hd_ClassId.Value + " AND tblclassstaffmap.StaffId!=" + Hd_StaffId.Value + " AND tblclassstaffmap.StaffId not in (SELECT tbltime_master.StaffId FROM tbltime_master INNER JOIN tbltime_classperiod ON tbltime_classperiod.Id=tbltime_master.ClassPeriodId INNER JOIN tbltime_week ON tbltime_week.Id=tbltime_classperiod.DayId WHERE lower(tbltime_week.Name)='" + Day.Date.DayOfWeek + "' AND tbltime_classperiod.PeriodId=" + Hd_PeriodId.Value + ")";
                    MydataSet = MyTimeTable.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
                    {
                        Completed = true;
                    }
                }
                if (!Completed)
                {
                    sql = "SELECT DISTINCT tblstaffsubjectmap.StaffId,tbluser.SurName FROM tblstaffsubjectmap INNER JOIN tbluser ON tbluser.Id=tblstaffsubjectmap.StaffId WHERE tblstaffsubjectmap.SubjectId=" + SubjectId + " AND tblstaffsubjectmap.StaffId!=" + Hd_StaffId.Value + " AND tblstaffsubjectmap.StaffId not in (SELECT tbltime_master.StaffId FROM tbltime_master INNER JOIN tbltime_classperiod ON tbltime_classperiod.Id=tbltime_master.ClassPeriodId INNER JOIN tbltime_week ON tbltime_week.Id=tbltime_classperiod.DayId WHERE lower(tbltime_week.Name)='" + Day.Date.DayOfWeek + "' AND tbltime_classperiod.PeriodId=" + Hd_PeriodId.Value + ")";
                    MydataSet = MyTimeTable.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
                    {
                        Completed = true;
                    }
                }
            }
            return MydataSet;
        }

        protected void Grid_Staff_RowEditing(object sender, GridViewEditEventArgs e)
        {
            string StaffId = Grid_Staff.DataKeys[e.NewEditIndex].Values["StaffId"].ToString();
            Hd_NewStaffId.Value = StaffId;
            Mpe_popup.Show();
        }

        protected void Btn_Assign_Click(object sender, EventArgs e)
        {
            DateTime Day = General.GetDateTimeFromText(Hd_DateString.Value);

            if (MyTimeTable.AddTemperoryTTEntry(Day, Hd_ClassId.Value, Hd_PeriodId.Value, Hd_NewStaffId.Value, Drp_Subject.SelectedValue))
            {
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Time Table", "Staff asigned for subject", 1);
                Lbl_Error.Text = "Staff Successfully Assigned";
                Grid_Staff.DataSource = null;
                Grid_Staff.DataBind();
                StoreInitialValues();
                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "Openerpagereload();", true);
            }
            else
            {
                Lbl_Error.Text = "Error while assigning staff. Try again";
            }

        }

        protected void Lnk_DeleteTempSetting_Click(object sender, EventArgs e)
        {
            DateTime Day = General.GetDateTimeFromText(Hd_DateString.Value);

            if (MyTimeTable.DeleteTemperoryTTEntry(Day, Hd_ClassId.Value, Hd_PeriodId.Value))
            {
                Lbl_Error.Text = "General Setting Restored Successfully";
                Grid_Staff.DataSource = null;
                Grid_Staff.DataBind();
                StoreInitialValues();
                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "Openerpagereload();", true);
            }
            else
            {
                Lbl_Error.Text = "Error In Restoring. Try again";
            }
        }

        protected void Grid_Staff_Sorting(object sender, GridViewSortEventArgs e)
        {
            MydataSet = GetStaffDataSet_Grid();

            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                DataTable dtAccountData = MydataSet.Tables[0];

                DataView dataView = new DataView(dtAccountData);

                dataView.Sort = e.SortExpression + " " + GetSortDirection1(e.SortExpression);
                Grid_Staff.Columns[0].Visible = true;
                Grid_Staff.DataSource = dataView;
                Grid_Staff.DataBind();
                Grid_Staff.Columns[0].Visible = false;
                GridColoring();
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
    }
}
