using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;
using System;
using System.Drawing;

namespace WinEr
{
    public partial class LocationMaster : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        private ConfigManager MyConfigMang;
        private WinBase.Inventory Myinventory;

        #region Events

            protected void Page_Load(object sender, EventArgs e)
            {
                if (Session["UserObj"] == null)
                {
                    Response.Redirect("sectionerr.htm");
                }
                MyUser = (KnowinUser)Session["UserObj"];
                MyConfigMang = MyUser.GetConfigObj();
                Myinventory = MyUser.GetInventoryObj();
                if (MyConfigMang == null)
                {
                    Response.Redirect("RoleErr.htm");
                }
                else if (!MyUser.HaveActionRignt(842))
                {
                    Response.Redirect("RoleErr.htm");
                }
                else
                {
                    Lbl_err.Text = "";
                    if (!IsPostBack)
                    {
                        Pnl_locationdisplay.Visible = false;
                        LoadLocationToGrid();
                        Pnl_AddLocation.Visible = false;
                       
                    }
                }
            }       

            protected void Btn_Add_Click(object sender, EventArgs e)
            {
                if (validName())
                {
                    string _locationname = Txt_locationname.Text;
                    string msg = "";
                    Myinventory.Addlocation(_locationname,out msg);
                    if (msg != "")
                    {
                        WC_MessageBox.ShowMssage(msg);
                    }
                    else
                    {
                        Lbl_err.Text = "Location Added";
                        LoadLocationToGrid();
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Add Location", "New location "+_locationname+" is added", 1);
                        
                    }

                }
            }

            protected void Grd_location_RowDeleting(object sender, GridViewDeleteEventArgs e)
            {
                Grd_location.Columns[0].Visible = true;
                string _locationname = "";
                int _locationId = int.Parse(Grd_location.Rows[e.RowIndex].Cells[0].Text.ToString());
                _locationname = Grd_location.Rows[e.RowIndex].Cells[1].Text.ToString();
                //if (Myinventory.ExistInTransaction(_locationId))
                //{
                //    WC_MessageBox.ShowMssage("This location is assigned to some transactions");
                //}

                //else 
                if (_locationId != 1)
                {
                    if (Myinventory.ExistInItemStock(_locationId))
                    {
                        WC_MessageBox.ShowMssage("This location is assigned to some Items,Move items to another location before perform deletion.");

                    }
                    else
                    {

                        Myinventory.DeleteLocation(_locationId);
                        LoadLocationToGrid();                   
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Delete Location", "Location " + _locationname + " is deleted", 1);
                    }
                }
                else
                {
                    WC_MessageBox.ShowMssage("Default store,cannot be deleted");
                }
                Grd_location.Columns[0].Visible = false;
            }
           
            protected void Grd_location_RowDataBound(object sender, GridViewRowEventArgs e)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton l = (LinkButton)e.Row.FindControl("LinkButton1");
                    l.Attributes.Add("onclick", "javascript:return " +
                         "confirm('Are you sure you want to delete this Record ')");
                }
            }
        
            protected void Grd_location_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                Grd_location.PageIndex = e.NewPageIndex;
                LoadLocationToGrid();
               
            }

            protected void Lnk_AddNewLocation_Click(object sender, EventArgs e)
            {
                Pnl_AddLocation.Visible = true;
            }

            protected void Grd_location_SelectedIndexChanged(object sender, EventArgs e)
            {
                Lbl_PopErr.Text = "";
                Hdn_RommId.Value = Grd_location.SelectedRow.Cells[0].Text;
                LoadClassToDropDown();
                MPE_MapLocation.Show();
            }

            protected void Btn_Map_Click(object sender, EventArgs e)
            {
                Lbl_PopErr.Text = "";
                int roomId = int.Parse(Hdn_RommId.Value);
                int classID = int.Parse(Drp_Class.SelectedValue);
                int batchId = MyUser.CurrentBatchId;
                string _locationname = "";
                string classname = "";
                if (classID != -1)
                {
                    if (classID != 0)
                    {
                        if (Myinventory.NotMappedToAnyotherLocation(roomId, classID))
                        {
                            Myinventory.MapLocationToClass(roomId, classID, batchId);
                            Lbl_PopErr.Text = "Mapped successfully";
                            LoadLocationToGrid();
                            _locationname = Myinventory.GetLocationNameById(roomId);
                            classname = Myinventory.GetClassNameByID(classID);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Map Room to class", "" + _locationname+ " is mapped to "+classname+"", 1);
                        }
                        else
                        {
                            Lbl_PopErr.Text = "Class already mapped";
                        }
                    }
                    else
                    {
                        Lbl_PopErr.Text = "Select any class";
                    }
                }
                MPE_MapLocation.Show();
            }

        #endregion

        #region Methods

            private bool validName()
            {
                bool _valid = true;
                if (Txt_locationname.Text == "")
                {
                    _valid = false;
                    WC_MessageBox.ShowMssage("Please enter location name");
                }
                return _valid;
            }

            private void LoadLocationToGrid()
            {
                DataSet LocationDs = new DataSet();
                Lbl_Location.Text = "";
                //string status = "";
                LocationDs = Myinventory.GetLocationName();
                LocationDs = CheckWhetherLocationIsMapped(LocationDs);
                if (LocationDs != null && LocationDs.Tables[0].Rows.Count > 0)
                {
                    Pnl_locationdisplay.Visible = true;
                    Grd_location.Columns[0].Visible = true;
                    Grd_location.DataSource = LocationDs;
                    Grd_location.DataBind();
                    Txt_locationname.Text = "";
                    Grd_location.Columns[0].Visible = false;

                }
                else
                {
                    Lbl_Location.Text = "No location found,Click on 'Add New Location' to add a location";
                    Grd_location.DataSource = null;
                    Grd_location.DataBind();
                    Pnl_locationdisplay.Visible = false;
                }
            }

            private DataSet CheckWhetherLocationIsMapped(DataSet LocationDs)
            {
                DataSet LocationMapDs = new DataSet();
                DataTable dt;
                DataRow _dr;
                LocationMapDs.Tables.Add(new DataTable("Location"));
                dt = LocationMapDs.Tables["Location"];
                //Id,Locationname
                dt.Columns.Add("Id");
                dt.Columns.Add("Locationname");
                dt.Columns.Add("Mappinglocation");
                foreach (DataRow dr in LocationDs.Tables[0].Rows)
                {
                    string classname = Myinventory.GetMappingLocation(int.Parse(dr["Id"].ToString()));
                    _dr = LocationMapDs.Tables["Location"].NewRow();
                    _dr["Id"] = dr["Id"].ToString();
                    _dr["Locationname"] = dr["Locationname"].ToString();
                    _dr["Mappinglocation"] = classname;
                    LocationMapDs.Tables["Location"].Rows.Add(_dr);
                }
                return LocationMapDs;
            }

            private void LoadClassToDropDown()
            {
                DataSet Class_Ds = new DataSet();
                ListItem li;
                Drp_Class.Items.Clear();
                Class_Ds = MyUser.MyAssociatedClass();
                if (Class_Ds != null && Class_Ds.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("Select Class", "0");
                    Drp_Class.Items.Add(li);
                    foreach (DataRow dr in Class_Ds.Tables[0].Rows)
                    {
                        li = new ListItem(dr[1].ToString(), dr[0].ToString());
                        Drp_Class.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("No Class Found", "-1");
                    Drp_Class.Items.Add(li);
                }
            }

        #endregion

        


    }
}
