using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;

namespace WinEr
{
    public partial class StudentTripMapReport : System.Web.UI.Page
    {
        private TransportationClass MyTransMang;
        private KnowinUser MyUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }

            MyUser = (KnowinUser)Session["UserObj"];
            MyTransMang = MyUser.GetTransObj();

            if (MyTransMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(912))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadClassToDropDown();
                    LoadLocationToDropdown();
                    Pnl_Show.Visible = false;
                }

            }
        }

        protected void img_export_Excel_Click(object sender, EventArgs e)
        {
            DataSet MyData = (DataSet)ViewState["TripMapReport"];
            MyData.Tables[0].Columns.Remove("ToTripId");
            MyData.Tables[0].Columns.Remove("FromTripId");
            if (!WinEr.ExcelUtility.ExportDataSetToExcel(MyData, "Trip-Student Map Report.xls"))
            {
                Lbl_Msg.Text = "";
            }  
        }

        protected void Grd_TripStudentMapReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_TripStudentMapReport.PageIndex = e.NewPageIndex;
            GetReport();
        }

        protected void Btn_Show_Click(object sender, EventArgs e)
        {
            GetReport();
 
        }

        private void GetReport()
        {
            DataSet TripStudMapDs = new DataSet();
            int classid = 0, destinationid = 0;
            try
            {
                int.TryParse(Drp_Class.SelectedValue, out classid);
                int.TryParse(Drp_Destination.SelectedValue, out destinationid);

                if (classid > 0)
                {
                    TripStudMapDs = MyTransMang.GetTripMapDetails(classid, destinationid);
                    if (TripStudMapDs != null && TripStudMapDs.Tables[0].Rows.Count > 0)
                    {
                        Pnl_Show.Visible = true;
                        Grd_TripStudentMapReport.Columns[4].Visible=true;
                        Grd_TripStudentMapReport.Columns[5].Visible = true;
                        Grd_TripStudentMapReport.DataSource = TripStudMapDs;
                        Grd_TripStudentMapReport.DataBind();
                        Grd_TripStudentMapReport.Columns[4].Visible = false;
                        Grd_TripStudentMapReport.Columns[5].Visible = false;
                    }
                    else
                    {
                        Lbl_Msg.Text = "No report found";
                        Pnl_Show.Visible = false;
                        Grd_TripStudentMapReport.DataSource = null;
                        Grd_TripStudentMapReport.DataBind();

                    }
                    ViewState["TripMapReport"] = TripStudMapDs;
                }
                else
                {
                    Lbl_Msg.Text = "Select Class";
                }

            }
            catch
            {
                Lbl_Msg.Text = "Error,Please try later!";
            }

        }
   
        private void LoadClassToDropDown()
        {
            DataSet Class_Ds = new DataSet();
            ListItem li;
            Drp_Class.Items.Clear();
            Class_Ds = MyTransMang.GetClassDetails();
            if (Class_Ds != null && Class_Ds.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("Select", "0");
                Drp_Class.Items.Add(li);
                foreach (DataRow dr in Class_Ds.Tables[0].Rows)
                {
                    li = new ListItem(dr["ClassName"].ToString(), dr["Id"].ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No class found", "-1");
                Drp_Class.Items.Add(li);
            }
        }

        private void LoadLocationToDropdown()
        {
            Drp_Destination.Items.Clear();
            DataSet Location_Ds = new DataSet();
            ListItem li;
            Location_Ds = MyTransMang.getDestinationsAll();
            if (Location_Ds != null && Location_Ds.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("All", "0");
                Drp_Destination.Items.Add(li);
                foreach (DataRow dr in Location_Ds.Tables[0].Rows)
                {
                    li = new ListItem(dr["Destination"].ToString(), dr["id"].ToString());
                    Drp_Destination.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No location found", "-1");
                Drp_Destination.Items.Add(li);
            }
        }
    }
}
