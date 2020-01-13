using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace WinEr
{
    public partial class StudentHouseMapReport : System.Web.UI.Page
    {
        private ConfigManager MyConfigMang;
        private WinBase.HouseManager MyHouse;
        private KnowinUser MyUser;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyConfigMang = MyUser.GetConfigObj();
            MyHouse = MyUser.GetHouseObj();
            if (MyConfigMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(903))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadHouse();
                    LoadClass();
                    Pnl_Show.Visible = false;

                }
            }
        }

        private void LoadClass()
        {
            Drp_Class.Items.Clear();
            DataSet ClassDs = new DataSet();
            ListItem li;
            ClassDs = MyUser.MyAssociatedClass();
            if (ClassDs != null && ClassDs.Tables != null && ClassDs.Tables[0].Rows.Count > 0)
            {
                 li = new ListItem("All", "0");
                Drp_Class.Items.Add(li);
                foreach (DataRow dr in ClassDs.Tables[0].Rows)
                {
                     li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class.Items.Add(li);
                } 
            }
            else
            {
                 li = new ListItem("No Class present", "-1");
                Drp_Class.Items.Add(li);
            }
        }

        private void LoadHouse()
        {
            DataSet HouseDs = new DataSet();
            ListItem Li = new ListItem();
            Drp_House.Items.Clear();
            HouseDs = MyHouse.GetAllActiveHouse();
            if (HouseDs != null && HouseDs.Tables[0].Rows.Count > 0)
            {
                Li = new ListItem("All", "0");
                Drp_House.Items.Add(Li);
                foreach (DataRow dr in HouseDs.Tables[0].Rows)
                {
                    Li = new ListItem(dr["HouseName"].ToString(), dr["Id"].ToString());
                    Drp_House.Items.Add(Li);
                }
            }
            else
            {
                Li = new ListItem("None", "-1");
                Drp_House.Items.Add(Li);

            }
        }

        protected void Btn_Show_Click(object sender, EventArgs e)
        {
            GetReport();
        }

        private void GetReport()
        {
            DataSet ReportsDs = new DataSet();
            Lbl_Err.Text = "";
            Lbl_total.Text = "";
            int houseid = 0, classid = 0,gender=0;
            int.TryParse(Drp_Class.SelectedValue, out classid);
            int.TryParse(Drp_House.SelectedValue, out houseid);
            int.TryParse(Drp_Gender.SelectedValue, out gender);
            Grd_Report.DataSource = null;
            Grd_Report.DataBind();
            ReportsDs = MyHouse.GetStudentHouseMapReport(classid, houseid, gender);
            if (ReportsDs != null && ReportsDs.Tables[0].Rows.Count > 0)
            {
                Pnl_Show.Visible = true;
                Grd_Report.Columns[0].Visible = true;
                Grd_Report.Columns[1].Visible = true;
                Grd_Report.Columns[6].Visible = true;
                Grd_Report.DataSource = ReportsDs;
                Grd_Report.DataBind();
                Grd_Report.Columns[0].Visible = false;
                Grd_Report.Columns[1].Visible = false;
                Grd_Report.Columns[6].Visible = false;
                Lbl_total.Text = "<b>Total students:   </b>" + ReportsDs.Tables[0].Rows.Count;
            }
            else
            {
                Lbl_Err.Text = "No report found";
                Grd_Report.DataSource = null;
                Grd_Report.DataBind();
                Pnl_Show.Visible = false;

            }
            ViewState["MapReport"] = ReportsDs;
        }

        protected void Grd_Report_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Report.PageIndex = e.NewPageIndex;
            GetReport();
        }

        protected void img_export_Excel_Click(object sender, ImageClickEventArgs e)
        {

            DataSet MyData = (DataSet)ViewState["MapReport"];
            MyData.Tables[0].Columns.Remove("studid");
            MyData.Tables[0].Columns.Remove("houseid");
            MyData.Tables[0].Columns.Remove("Address");
            if (!WinEr.ExcelUtility.ExportDataSetToExcel(MyData, "Student-House Map Report.xls"))
            {
                Lbl_Err.Text = "";
            }          
        }
    }
}
