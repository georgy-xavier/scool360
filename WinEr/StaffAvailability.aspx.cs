using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using WinBase;

namespace WinEr
{
    public partial class StaffAvailability : System.Web.UI.Page
    {
        private StaffManager MyStaffMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet;
        private TimeTable MyTimeTable;
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
            else if (MyTimeTable == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(631))
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
                    int StaffId = 0;
                    bool Valid = false;
                    if (Session["StaffId"] != null)
                    {
                        if (int.TryParse(Session["StaffId"].ToString(), out StaffId))
                        {
                            if (StaffId > 0)
                            {
                                Valid = true;
                            }
                        }
                    }
                    if (Valid)
                    {
                        LoaduserTopData();
                        LoadStaffAvailability(StaffId);

                    }

                }
            }
        }

       

        private void LoaduserTopData()
        {
             
            string _Studstrip = MyStaffMang.ToStripString(int.Parse(Session["StaffId"].ToString()),"Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StaffId"].ToString()) + "&type=StaffImage");
            this.StudentTopStrip.InnerHtml = _Studstrip;
        }

        private void LoadStaffAvailability(int StaffId)
        {
            Lbl_Error.Text = "";

            string sql = "select PeriodId,FrequencyName from tblattendanceperiod where ModeId=3 ";
            MyDataSet = MyTimeTable.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                Grd_StaffAvailability.Columns[0].Visible = true;
                Grd_StaffAvailability.DataSource = MyDataSet;
                Grd_StaffAvailability.DataBind();
                Grd_StaffAvailability.Columns[0].Visible = false;
                LoadStaffAvlDetails(StaffId);
                Hdn_StaffId.Value = StaffId.ToString();
            }
            else
            {
                Lbl_Error.Text = "Error In Loading";
                Grd_StaffAvailability.DataSource = MyDataSet;
                Grd_StaffAvailability.DataBind();
            }
        }

        private void LoadStaffAvlDetails(int StaffId)
        {
            Lbl_Error.Text = "";
            DataSet MyDay;
            int PeriodId = 0;
            CheckBox Chk_Day;
            bool EntryPresents = false;
            if (MyTimeTable.IsStaffAvailabilityEntryExists(StaffId))
            {
                EntryPresents = true;
            }

            foreach (GridViewRow gv in Grd_StaffAvailability.Rows)
            {
                PeriodId = int.Parse(gv.Cells[0].Text.ToString());
                if (EntryPresents)
                {
                    MyDay = MyTimeTable.GetStaffAvlDayIds_PeriodId(PeriodId, StaffId);
                }
                else
                {
                    MyDay = MyTimeTable.GetGenStaffAvlDetails(PeriodId);
                }
                if (MyDay != null && MyDay.Tables != null && MyDay.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow Dr_Day in MyDay.Tables[0].Rows)
                    {

                        if (Dr_Day[0].ToString() == "1")
                        {
                            Chk_Day = (CheckBox)gv.FindControl("Chk_AvlMon");
                            Chk_Day.Checked = true;
                        }
                        else if (Dr_Day[0].ToString() == "2")
                        {
                            Chk_Day = (CheckBox)gv.FindControl("Chk_AvlTues");
                            Chk_Day.Checked = true;
                        }
                        else if (Dr_Day[0].ToString() == "3")
                        {
                            Chk_Day = (CheckBox)gv.FindControl("Chk_AvlWed");
                            Chk_Day.Checked = true;
                        }
                        else if (Dr_Day[0].ToString() == "4")
                        {
                            Chk_Day = (CheckBox)gv.FindControl("Chk_AvlThur");
                            Chk_Day.Checked = true;
                        }
                        else if (Dr_Day[0].ToString() == "5")
                        {
                            Chk_Day = (CheckBox)gv.FindControl("Chk_AvlFri");
                            Chk_Day.Checked = true;
                        }
                        else if (Dr_Day[0].ToString() == "6")
                        {
                            Chk_Day = (CheckBox)gv.FindControl("Chk_AvlSat");
                            Chk_Day.Checked = true;
                        }
                        else if (Dr_Day[0].ToString() == "7")
                        {
                            Chk_Day = (CheckBox)gv.FindControl("Chk_AvlSun");
                            Chk_Day.Checked = true;
                        }

                    }
                }
            }
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                int PeriodId = 0;
                CheckBox Chk_Monday;
                CheckBox Chk_TuesDay;
                CheckBox Chk_WednesDay;
                CheckBox Chk_ThursDay;
                CheckBox Chk_FriDay;
                CheckBox Chk_SaturDay;
                CheckBox Chk_SunDay;
                foreach (GridViewRow gv in Grd_StaffAvailability.Rows)
                {
                    PeriodId = int.Parse(gv.Cells[0].Text.ToString());
                    Chk_Monday = (CheckBox)gv.FindControl("Chk_AvlMon");
                    Chk_TuesDay = (CheckBox)gv.FindControl("Chk_AvlTues");
                    Chk_WednesDay = (CheckBox)gv.FindControl("Chk_AvlWed");
                    Chk_ThursDay = (CheckBox)gv.FindControl("Chk_AvlThur");
                    Chk_FriDay = (CheckBox)gv.FindControl("Chk_AvlFri");
                    Chk_SaturDay = (CheckBox)gv.FindControl("Chk_AvlSat");
                    Chk_SunDay = (CheckBox)gv.FindControl("Chk_AvlSun");
                    MyTimeTable.UpdateStaffAvail(Chk_Monday.Checked, Chk_TuesDay.Checked, Chk_WednesDay.Checked, Chk_ThursDay.Checked, Chk_FriDay.Checked, Chk_SaturDay.Checked, Chk_SunDay.Checked, PeriodId, int.Parse(Hdn_StaffId.Value));
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Staff Availabulity", "Staff availabulity changed", 1);

                }
                LoadStaffAvailability(int.Parse(Hdn_StaffId.Value));
                Lbl_Error.Text = "Saved successfully";
            }
            catch
            {
                Lbl_Error.Text = "Error In Saving. Try Later";
            }

        }

        protected void Btn_Retore_Click(object sender, EventArgs e)
        {
            try
            {
                MyTimeTable.DeleteStaffAvlDetails(int.Parse(Hdn_StaffId.Value));
                MyTimeTable.ApplyDefltAvlToStaff(int.Parse(Hdn_StaffId.Value));
                LoadStaffAvailability(int.Parse(Hdn_StaffId.Value));
                Lbl_Error.Text = "Successfully restored to general availability of school";
            }
            catch
            {
                Lbl_Error.Text = "Error In Restoring. Try Later";
            }
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("StaffAvailability.aspx");
        }
    }
}
