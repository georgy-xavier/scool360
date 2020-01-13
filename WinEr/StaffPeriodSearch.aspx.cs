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
    public partial class StaffPeriodSearch : System.Web.UI.Page
    {
        private StaffManager MyStaffMang;
        private TimeTable MyTimeTable;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (Session["StaffId"] == null)
            {
                Response.Redirect("Default.aspx");
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
            else if (!MyUser.HaveActionRignt(800))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadClassDetails();
                    LoadPeriods();
                    LoadDays();
                }
            }
        }

        private void LoadDays()
        {
            Drp_Day.Items.Clear();
            string sql = "SELECT Id,Name FROM tbltime_week";
            MyReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("Any", "0");
                Drp_Day.Items.Add(li);
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Day.Items.Add(li);
                }
            }
        }

        private void LoadPeriods()
        {
            Drp_Period.Items.Clear();
            string sql = "SELECT PeriodId,FrequencyName FROM tblattendanceperiod where PeriodId between 4 and 11";
            MyReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Period.Items.Add(li);
                }
            }
        }

        private void LoadClassDetails()
        {
            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Drp_Class.Items.Clear();
                ListItem Li = new ListItem("Any", "0");
                Drp_Class.Items.Add(Li);
                foreach (DataRow Dr_Class in MydataSet.Tables[0].Rows)
                {
                    Li = new ListItem(Dr_Class[1].ToString(), Dr_Class[0].ToString());
                    Drp_Class.Items.Add(Li);
                }
            }
        }

        protected void Btn_Find_Click(object sender, EventArgs e)
        {
            
           // string sql = "select distinct tbltime_master.StaffId from tbltime_master inner join tbltime_classperiod on tbltime_classperiod.Id = tbltime_master.ClassPeriodId where tbltime_classperiod.ClassId=4 and tbltime_classperiod.DayId =3 and tbltime_classperiod.PeriodId=6";
        }

        protected void Grd_Staff_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sql = "";
            MydataSet = MyTimeTable.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                DataTable dtAccountData = MydataSet.Tables[0];
                DataView dataView = new DataView(dtAccountData);
                dataView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
               
            }
        }

        private string GetSortDirection(string column)
        {
            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";
            // Retrieve the last column that was sorted.
            string sortExpression = Session["SortExpression"] as string;
            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = Session["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }
            // Save new values in ViewState.
            Session["SortDirection"] = sortDirection;
            Session["SortExpression"] = column;
            return sortDirection;
        }

    }
}
