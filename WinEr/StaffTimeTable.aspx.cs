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
    public partial class StaffTimeTable : System.Web.UI.Page
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
                Response.Redirect("RoleErr.htm");
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
            else if (!MyUser.HaveActionRignt(635))
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
                    LoaduserTopData();
                    LoadTimeTable(DateTime.Now.Date);
                }
            }
        }

        private static DateTime firstDayOfWeek(DateTime day, DayOfWeek weekStarts)
        {
            DateTime d = day;
            while (d.DayOfWeek != weekStarts)
            {
                d = d.AddDays(-1);
            }

            return d;
        }

        private void LoadTimeTable(DateTime SelectedDate)
        {
            
            DateTime firstDay = firstDayOfWeek(SelectedDate, DayOfWeek.Monday);
            bool IsGenerated ;
            string _Table="";
            this.MyTimeTableDiv.InnerHtml = MyTimeTable.GetMyTimeTable(int.Parse(Session["StaffId"].ToString()), firstDay, MyUser.CurrentBatchId, out IsGenerated, out _Table, MyTimeTable.m_MysqlDb);
            if (IsGenerated)
            {
                Img_Export.Visible = true;
                ViewState["TimeTable"] = _Table;
            }
            else
                Img_Export.Visible = false;
        }

       
        protected void Img_Export_Click(object sender, ImageClickEventArgs e)
        {
           string _StaffName = MyStaffMang.GetStaffName(int.Parse(Session["StaffId"].ToString())) + "TimeTable"; 
           string _Table = (string)ViewState["TimeTable"];
           ExcelUtility.ExportBuiltStringToExcel(_StaffName, _Table, "TimeTable");
        }

        private void LoaduserTopData()
        {

            string _Studstrip = MyStaffMang.ToStripString(int.Parse(Session["StaffId"].ToString()), "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StaffId"].ToString()) + "&type=StaffImage");
            this.StudentTopStrip.InnerHtml = _Studstrip;
        }




        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            LoadTimeTable(Calendar1.SelectedDate);
            calendervisibilty(true);
        }

        protected void Img_Caneder_Click(object sender, ImageClickEventArgs e)
        {
            calendervisibilty(false);
        }

        private void calendervisibilty(bool visibility)
        {
            Img_Caneder.Visible = visibility;
            panelData.Visible = visibility;
            Img_Export.Visible = visibility;
            Calendar1.Visible = !visibility;
            Lbl_label.Visible = !visibility;
        }

    }
}
