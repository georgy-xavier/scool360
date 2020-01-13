using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;
using System.Text;

namespace WinEr
{
    public partial class StudentTripMap : System.Web.UI.Page
    {
        private TransportationClass MyTransMang;
        private KnowinUser MyUser;
        private FeeManage MyFeeMang;
        int TripId;
        string Routename = "";
        string tripname="";
        int routId;
        int alloted;

        #region Events
       
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyTransMang = MyUser.GetTransObj();
            MyFeeMang = MyUser.GetFeeObj();
            if (MyTransMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }         
            else
            {
                if (!IsPostBack)
                {
                    TripId = int.Parse(Request.QueryString["TripId"].ToString());
                    Routename = Request.QueryString["routename"].ToString();
                    tripname = Request.QueryString["tripname"].ToString();
                    routId = int.Parse(Request.QueryString["RoutId"].ToString());
                    alloted = int.Parse(Request.QueryString["Occupancy"].ToString());

                    Hdn_alloted.Value = alloted.ToString();
                    Hdn_Routename.Value = Routename.ToString();
                    Hdn_tripname.Value = tripname.ToString();
                    Hdn_routId.Value = routId.ToString();
                    Hdn_TripId.Value = TripId.ToString();
            

                    Pnl_Display.Visible = false;
                    LoadHeading();
                    LoadClassToDropDown();
                    LoadDestinationToDropDown();
                    LoadGrid();
                }

            }
        }      
     
        protected void Btn_ShowAll_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void Grd_StudentTripMap_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_StudentTripMap.PageIndex = e.NewPageIndex;
            LoadGrid();
        }

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void Drp_Destination_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void Drp_Status_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void Drp_Sex_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void Btn_Assign_Click(object sender, EventArgs e)
        {
            int chkcount = 0;
            int dest = 0;
            Lbl_Err.Text = "";
            int Maxcapacity = MyTransMang.GetMaxCapacityOfTrip(int.Parse(Hdn_TripId.Value),int.Parse(Hdn_routId.Value));
            int allotedcnt = int.Parse(Hdn_alloted.Value);
            int newalloted = 0;
            foreach (GridViewRow gr in Grd_StudentTripMap.Rows)
            {
                CheckBox Chkfee = (CheckBox)gr.FindControl("ChkFee");
                if (Chkfee.Checked)
                {
                    newalloted++;

                }
            }
            if ( Maxcapacity==allotedcnt)
            {
                Lbl_Err.Text = "This trip have no vacancy,Add student to any other trip";
               
            }
            else if (Maxcapacity < (allotedcnt + newalloted))
            {
                Lbl_Err.Text = "Alloted students count exeeds trip capaity,Please uncheck some students!";
            }
            else
            {
                foreach (GridViewRow gr in Grd_StudentTripMap.Rows)
                {
                    CheckBox Chk = (CheckBox)gr.FindControl("ChkFee");
                    if (Chk.Checked)
                    {
                        int studentId = int.Parse(gr.Cells[1].Text);
                        int destinationId = int.Parse(gr.Cells[7].Text);
                        chkcount++;
                        bool destination = true;
                        MyTransMang.SaveDataToTable(int.Parse(Hdn_TripId.Value), studentId, destinationId,int.Parse(Hdn_routId.Value),out destination);
                        if (!destination)
                        {
                            dest = 1;

                        }
                        else
                        {
                            int value = 1;
                            MyTransMang.UpdateTripOccupancy(int.Parse(Hdn_TripId.Value), 1, value);
                            string studname = gr.Cells[2].Text;
                            string classname = gr.Cells[3].Text;
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Assign trip", "A trip of " + Hdn_tripname.Value + " is assigned to " + studname + " of class " + classname + " to the route " + Hdn_Routename.Value + "", 1);
                        }
                        


                    }
                }
                if (chkcount == 0)
                {
                    Lbl_Err.Text = "Select any student";
                }
                else
                {                    
                    LoadGrid();
                    if (dest == 1)
                    {
                        Lbl_Err.Text = "Trip cannot assigned to some students,Destination does not contained in this route!";
                    }
                    else
                    {
                        Lbl_Err.Text = "Trip is assigned to selected students";
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AnyScript", "Reload('OccupancyStatus.aspx');", true);
                    allotedcnt = allotedcnt + chkcount;
                    Hdn_alloted.Value = allotedcnt.ToString();
                }
            }
        }    

        protected void Btn_Calcel_Click(object sender, EventArgs e)
        {

        }

        protected void Btn_Remove_Click(object sender, EventArgs e)
        {
            int chkcount = 0;
            int allotedcnt=int.Parse(Hdn_alloted.Value);
            Lbl_Err.Text = "";
            foreach (GridViewRow gr in Grd_StudentTripMap.Rows)
            {
                CheckBox Chk = new CheckBox();
                Chk = (CheckBox)gr.FindControl("ChkFee");
                if (Chk.Checked)
                {
                    int studentId = int.Parse(gr.Cells[1].Text);
                    int destinationId = int.Parse(gr.Cells[7].Text);
                    chkcount++;
                    MyTransMang.RemoveDataFromTable(int.Parse(Hdn_TripId.Value), studentId, destinationId);
                    string studname = gr.Cells[2].Text;
                    string classname = gr.Cells[3].Text;
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Remove trip", ""+studname+" of class "+classname+" is removed from "+Hdn_tripname.Value+" of the route "+Hdn_Routename.Value+"", 1);
                }
            }
            if (chkcount == 0)
            {
               Lbl_Err.Text="Select any student";
            }
            else
            {
                
                int value = 0;
                MyTransMang.UpdateTripOccupancy(int.Parse(Hdn_TripId.Value), chkcount, value);
               Lbl_Err.Text="Selected students were removed from this trip";
               LoadGrid();
               ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AnyScript", "Reload('OccupancyStatus.aspx');", true);
               allotedcnt = allotedcnt - chkcount;
               Hdn_alloted.Value = allotedcnt.ToString();
            }
        }

        #endregion


        #region Methods

        private void LoadDestinationToDropDown()
        {
            DataSet Destination_Ds = new DataSet();
            ListItem li;
            Drp_Destination.Items.Clear();
            Destination_Ds = MyTransMang.getDestinationsUnderRoute(routId);
            if (Destination_Ds != null && Destination_Ds.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("All", "0");
                Drp_Destination.Items.Add(li);
                foreach (DataRow dr in Destination_Ds.Tables[0].Rows)
                {
                    li = new ListItem(dr["Destination"].ToString(), dr["id"].ToString());
                    Drp_Destination.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No Destination found", "-1");
                Drp_Destination.Items.Add(li);
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
                li = new ListItem("All", "0");
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

        private void LoadGrid()
        {
            Lbl_Err.Text = "";           
            int ClassId = int.Parse(Drp_Class.SelectedValue.ToString());
            int DestinationId = int.Parse(Drp_Destination.SelectedValue.ToString());
            string  Sex = Drp_Sex.SelectedItem.ToString();
            int status = int.Parse(Drp_Status.SelectedValue.ToString());
            DataSet Details_Ds = new DataSet();
            if (ClassId != -1 && DestinationId != -1)
            {
                Details_Ds = MyTransMang.GetTripDetails(ClassId, DestinationId, Sex, status, int.Parse(Hdn_TripId.Value), Hdn_routId.Value);
                if (Details_Ds != null && Details_Ds.Tables[0].Rows.Count > 0)
                {
                    Grd_StudentTripMap.DataSource = null;
                    Grd_StudentTripMap.DataBind();
                    Pnl_Display.Visible = true;
                    Grd_StudentTripMap.Columns[1].Visible = true;
                    Grd_StudentTripMap.Columns[7].Visible = true;
                    Grd_StudentTripMap.DataSource = Details_Ds;
                    Grd_StudentTripMap.DataBind();
                    Grd_StudentTripMap.Columns[1].Visible = false;
                    Grd_StudentTripMap.Columns[7].Visible = false;
                    if (status == 0)
                    {
                        Btn_Remove.Visible = true;
                        Btn_Assign.Visible = false;
                    }
                    else if (status == 1)
                    {

                        Btn_Remove.Visible = false;
                        Btn_Assign.Visible = true;
                    }
                }
                else
                {
                    Pnl_Display.Visible = false;
                    Lbl_Err.Text = "No data exist";
                    Grd_StudentTripMap.DataSource = null;
                    Grd_StudentTripMap.DataBind();
                }
            }
            else
            {
                Pnl_Display.Visible = false;
                Lbl_Err.Text = "No data exist";
                Grd_StudentTripMap.DataSource = null;
                Grd_StudentTripMap.DataBind();
            }
          
        }

        private void LoadHeading()
        {
            StringBuilder header = new StringBuilder();
            header.AppendLine("<center>");
            header.AppendLine("<table width=\"100%\" border=\"1\">");
            header.AppendLine("<tr>");
            header.AppendLine("<td style=\"font-size:larger; font-weight:bold;\">Route: "+Routename+"</td>");
            header.AppendLine("<td style=\"font-size:larger; font-weight:bold;\">Trip: "+tripname+"</td>");
            header.AppendLine("</tr>");
            header.AppendLine("</table>");
            header.AppendLine("</center>");
            HeaderDiv.InnerHtml = header.ToString();
        }       

        #endregion 

    }
}
