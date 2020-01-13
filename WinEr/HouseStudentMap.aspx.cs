using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

namespace WinEr
{
    public partial class HouseStudentMap : System.Web.UI.Page
    {
        private ConfigManager MyConfigMang;
        private WinBase.HouseManager MyHouse;
        private KnowinUser MyUser;
   
        
        #region Events

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
                    else if (!MyUser.HaveActionRignt(902))
                    {
                        Response.Redirect("RoleErr.htm");
                    }
                    else
                    {
                        if (!IsPostBack)
                        {
                            AddClassToDropDownClass();
                            LoadHouseToDropDown();
                            Pnl_Studlist.Visible = false;
                            LoadMappedDetails();
                            LoadConfigvalues();
                            LoadClass();
                            LoadHouse();
                            Paneleditmap.Visible =false;
                            
                        }
                    }
                    lblcheck.Text = "";
                }          

            protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
            {
                LoadStudents();
            }

            protected void grd_studList_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                grd_studList.PageIndex = e.NewPageIndex;
                LoadStudents();
            }
           
            protected void Btn_Map_Click(object sender, EventArgs e)
            {
                int houseid = 0;
                int.TryParse(Drp_House.SelectedValue, out houseid);
                if (houseid > 0)
                {
                    MapStudentToHouse(houseid);
                }
                else
                {
                    Lbl_Err.Text = "Please select house!";
                }
            }

        #endregion

        #region Methods

            private void MapStudentToHouse(int houseid)
            {
                CheckBox chk = new CheckBox();
                int studid = 0;
                int chkcount = 0;
                bool success = true;
                Lbl_Err.Text = "";
                SetGridColumnVisibilityTrue();
                try
                {
                    MyHouse.CreateTansationDb();
                    foreach (GridViewRow gr in grd_studList.Rows)
                    {
                        chk = (CheckBox)gr.FindControl("chk_select");
                        if (chk.Checked)
                        {
                            chkcount++;
                            int.TryParse(gr.Cells[0].Text, out studid);
                            MyHouse.SaveToDataBase(houseid, studid);
                        }
                    }

                }
                catch (Exception)
                {
                    success = false;
                    MyHouse.EndFailTansationDb();
                    Lbl_Err.Text = "Error,Cannot map student...";
                }
                if (chkcount == 0)
                {
                    Lbl_Err.Text = "Select any student!";
                    MyHouse.EndFailTansationDb();
                }
                else if (success)
                {
                    MyHouse.EndSucessTansationDb();
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "House", "house Mapped", 1);
                    Lbl_Err.Text = "Mapped successfully";
                }
                SetGridColumnVisibilityFalse();
                LoadStudents();
            }

            private void LoadStudents()
            {
                int classid = 0;
                DataSet StudDs = new DataSet();
                int.TryParse(Drp_Class.SelectedValue, out classid);
                try
                {
                    if (classid > 0)
                    {
                        StudDs = MyHouse.getStudentsFromClass(classid);
                        if (StudDs != null && StudDs.Tables[0].Rows.Count > 0)
                        {
                            Pnl_Studlist.Visible = true;
                            SetGridColumnVisibilityTrue();
                            grd_studList.DataSource = StudDs;
                            grd_studList.DataBind();
                            SetGridColumnVisibilityFalse();

                        }
                        else
                        {
                            Lbl_Err.Text = "No student is pending for mapping!!";
                            grd_studList.DataSource = null;
                            grd_studList.DataBind();
                            Pnl_Studlist.Visible = false;
                        }
                    }
                }
                catch (Exception)
                {
                    Lbl_Err.Text = "Error.Cannot load..!";
                }

            }

            private void SetGridColumnVisibilityTrue()
            {
                grd_studList.Columns[0].Visible = true;
            }

            private void SetGridColumnVisibilityFalse()
            {
                grd_studList.Columns[0].Visible = false;
            }

            private void LoadHouseToDropDown()
            {
                DataSet HouseDs = new DataSet();
                HouseDs = MyHouse.GetAllActiveHouse();
                ListItem li;
                Drp_House.Items.Clear();
                if (HouseDs != null && HouseDs.Tables != null && HouseDs.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("Select House", "0");
                    Drp_House.Items.Add(li);
                    foreach (DataRow dr in HouseDs.Tables[0].Rows)
                    {
                        li = new ListItem(dr[1].ToString(), dr[0].ToString());
                        Drp_House.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("No House Present", "-1");
                    Drp_Class.Items.Add(li);
                }
                Drp_Class.SelectedIndex = 0;

            }

            private void AddClassToDropDownClass()
            {
                DataSet ClassDs = new DataSet();
                ListItem li;
                Drp_Class.Items.Clear();
                ClassDs = MyUser.MyAssociatedClass();
                if (ClassDs != null && ClassDs.Tables != null && ClassDs.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("Select Class", "0");
                    Drp_Class.Items.Add(li);
                    foreach (DataRow dr in ClassDs.Tables[0].Rows)
                    {
                        li = new ListItem(dr[1].ToString(), dr[0].ToString());
                        Drp_Class.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("No Class Present", "-1");
                    Drp_Class.Items.Add(li);
                }
                Drp_Class.SelectedIndex = 0;

            }

       #endregion  
        

        #region AutoAssign

            private void LoadConfigvalues()
            {
                OdbcDataReader configreader = null;
                int configvalue = 0;
                try
                {
                    string sql = "select value from tblconfiguration where tblconfiguration.Name='Change House on Promotion'";
                    configreader = MyHouse.m_MysqlDb.ExecuteQuery(sql);
                    if (configreader.HasRows)
                    {
                        int.TryParse(configreader.GetValue(0).ToString(), out configvalue);
                    }
                    if (configvalue == 0)
                    {
                        Lbl_config.Text = "As per configuration, the students will be in the same house after promotion!";
                    }
                    else
                    {
                        Lbl_config.Text = "As per configuration, the mapped details will be cleared and remapped to house after promotion!";
                    }
                }
                catch (Exception)
                {

                }
            }

            private void LoadMappedDetails()
            {
                OdbcDataReader Countreader = null;
                int count = 0;
                string sql = "";
                try
                {
                    sql = "select Count(tblstudent.Id) from tblstudent where  tblstudent.Id not in( select StudId from tblhousestudentmap) and tblstudent.status=1";
                    Countreader = MyHouse.m_MysqlDb.ExecuteQuery(sql);
                    if (Countreader.HasRows)
                    {
                        int.TryParse(Countreader.GetValue(0).ToString(), out count);
                    }
                    Lbl_Totunmapstud.Text = count.ToString();
                    if (count == 0)
                    {
                        Btn_AutoMap.Enabled = false;
                    }
                    else
                    {
                        Btn_AutoMap.Enabled = true;
                    }
                    Countreader = null;
                    count = 0;
                    sql = "select count(StudId) from tblhousestudentmap";
                    Countreader = MyHouse.m_MysqlDb.ExecuteQuery(sql);
                    if (Countreader.HasRows)
                    {
                        int.TryParse(Countreader.GetValue(0).ToString(), out count);
                    }
                    Lbl_Totmapstud.Text = count.ToString();
                    Countreader = null;
                    count = 0;
                    sql = "select Count(tblstudent.Id) from tblstudent where tblstudent.status=1";
                    Countreader = MyHouse.m_MysqlDb.ExecuteQuery(sql);
                    if (Countreader.HasRows)
                    {
                        int.TryParse(Countreader.GetValue(0).ToString(), out count);
                    }
                    Lbl_Totstud.Text = count.ToString();
                }
                catch (Exception)
                {
                    Lbl_Totunmapstud.Text = "0";
                    Lbl_Totmapstud.Text = "0";
                    Lbl_Totstud.Text = "0";
                    lbl_AutoErr.Text = "Error,Cannot load mapping details!";

                }
                Countreader.Close();

            }          

            protected void Btn_AutoMap_Click(object sender, EventArgs e)
            {
                DataSet StudentDs = new DataSet();
                OdbcDataReader mappedcounrreader = null, maxhouseidreader = null; ;
                int studid = 0, houseid = 0, maxcount = 0,Maxhouseid=0;
                DataSet HouseDs = new DataSet();
                int total = 0;
                bool success = true;
                try
                {
                    StudentDs = MyHouse.GetAllStudDataSet();
                    if (StudentDs != null && StudentDs.Tables[0].Rows.Count > 0)
                    {
                        string sql = "select MAX(Id) from tblhousestudentmap ";
                        mappedcounrreader = MyHouse.m_MysqlDb.ExecuteQuery(sql);
                        if (mappedcounrreader.HasRows)
                        {
                            int.TryParse(mappedcounrreader.GetValue(0).ToString(), out maxcount);
                        }
                        if (maxcount == 0)
                        {
                            HouseDs = MyHouse.GetAllActiveHouse();
                            if (HouseDs != null && HouseDs.Tables[0].Rows.Count > 0)
                            {
                                //DataView Houseview = HouseDs.Tables[0].DefaultView; // DataSet to DataView
                                //Houseview.Sort = "Id ASC";  // We can able to add more columns in this
                                //DataTable Housetable = Houseview.Table;
                                MyHouse.CreateTansationDb();
                                foreach (DataRow dr in StudentDs.Tables[0].Rows)
                                {
                                    if (total == HouseDs.Tables[0].Rows.Count)
                                    {
                                        total = 0;
                                    }
                                    int.TryParse(dr["Id"].ToString(), out studid);
                                    int.TryParse(HouseDs.Tables[0].Rows[total]["Id"].ToString(), out houseid);
                                    MyHouse.SaveToDataBase(houseid, studid);
                                    total++;
                                }

                            }
                            else
                            {
                                success = false;
                                WC_MessageBox.ShowMssage("No house exist!");

                            }
                        }
                        else
                        {
                            sql = "";
                            sql = "select HouseId from tblhousestudentmap where Id=" + maxcount + " ";
                            maxhouseidreader = MyHouse.m_MysqlDb.ExecuteQuery(sql);
                            if (maxhouseidreader.HasRows)
                            {
                                int.TryParse(maxhouseidreader.GetValue(0).ToString(),out Maxhouseid);
                            }                           
                            HouseDs = MyHouse.GetAllActiveHouse();
                            if (HouseDs != null && HouseDs.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow _dr in HouseDs.Tables[0].Rows)
                                {
                                    if (_dr["Id"].ToString() == Maxhouseid.ToString())
                                    {
                                        total = HouseDs.Tables[0].Rows.IndexOf(_dr);
                                    }
                                }

                                MyHouse.CreateTansationDb();
                                foreach (DataRow dr in StudentDs.Tables[0].Rows)
                                {
                                    if (total == HouseDs.Tables[0].Rows.Count - 1)
                                    {
                                        total = 0;
                                    }
                                    else
                                    {
                                        total++;
                                    }                                 
                                    int.TryParse(dr["Id"].ToString(), out studid);
                                    int.TryParse(HouseDs.Tables[0].Rows[total]["Id"].ToString(), out houseid);
                                    MyHouse.SaveToDataBase(houseid, studid);
                                    
                                    
                                }
                            }
                            else
                            {
                                success = false;
                                WC_MessageBox.ShowMssage("No house exist!");

                            }

                        }

                    }
                    else
                    {
                        success = false;
                        WC_MessageBox.ShowMssage("No student is pending for mapping!");

                    }
                    MyHouse.EndSucessTansationDb();
                }
                catch (Exception)
                {
                    success = false;
                    WC_MessageBox.ShowMssage("Cannot map,Please try again later!");
                    MyHouse.EndFailTansationDb();
                }
                if (success)
                {
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "House", "house Mapped", 1);
                    WC_MessageBox.ShowMssage("Mapped successfully!");
                    

                }
                LoadMappedDetails();

            }
      
       #endregion
            private void LoadClass()
            {
                DropDownListclass.Items.Clear();
                DataSet ClassDs = new DataSet();
                ListItem li;
                ClassDs = MyUser.MyAssociatedClass();
                if (ClassDs != null && ClassDs.Tables != null && ClassDs.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("All", "0");
                    DropDownListclass.Items.Add(li);
                    foreach (DataRow dr in ClassDs.Tables[0].Rows)
                    {
                        li = new ListItem(dr[1].ToString(), dr[0].ToString());
                        DropDownListclass.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("No Class present", "-1");
                    DropDownListclass.Items.Add(li);
                }
            }
            private void LoadHouse()
            {
                DataSet HouseDs = new DataSet();
                ListItem Li = new ListItem();
                DropDownListhouse.Items.Clear();
                HouseDs = MyHouse.GetAllActiveHouse();
                if (HouseDs != null && HouseDs.Tables[0].Rows.Count > 0)
                {
                    Li = new ListItem("All", "0");
                    DropDownListhouse.Items.Add(Li);
                    foreach (DataRow dr in HouseDs.Tables[0].Rows)
                    {
                        Li = new ListItem(dr["HouseName"].ToString(), dr["Id"].ToString());
                        DropDownListhouse.Items.Add(Li);
                    }
                }
                else
                {
                    Li = new ListItem("None", "-1");
                    DropDownListhouse.Items.Add(Li);
                }
            }

            private void loadhousemapedit()
            {
                DataSet housemapDs = new DataSet();
                ListItem Li = new ListItem();
             
                housemapDs = MyHouse.GetAllActiveHouse();
                DropDownList drdList;
                GridViewmap.Columns[1].Visible = true;
                GridViewmap.Columns[2].Visible = true;

                if (housemapDs != null && housemapDs.Tables[0].Rows.Count > 0)
                {

                    foreach (GridViewRow grdRow in GridViewmap.Rows)
                    {
                        drdList = (DropDownList)(GridViewmap.Rows[grdRow.RowIndex].Cells[7].FindControl("DropDownListeditmap"));
                        drdList.Items.Clear();

                        foreach (DataRow dr in housemapDs.Tables[0].Rows)
                        {

                            Li = new ListItem(dr["HouseName"].ToString(), dr["Id"].ToString());
                            drdList.Items.Add(Li);                          

                        }
                            drdList.SelectedValue = grdRow.Cells[2].Text.ToString();
                    }
                }
                else
                {
                    foreach (GridViewRow grdRow in GridViewmap.Rows)
                    {
                        Li = new ListItem("None", "0");
                        drdList = (DropDownList)(GridViewmap.Rows[grdRow.RowIndex].Cells[7].FindControl("DropDownListeditmap"));
                        drdList.Items.Add(Li);
                    }
                }
                GridViewmap.Columns[1].Visible = false;
                GridViewmap.Columns[2].Visible = false;

            }
           
            private void Getmapdetails()
            {
                DataSet housemapDs = new DataSet();
                lblcheck.Text = "";
             
                int houseid = 0, classid = 0, gender = 0;
                int.TryParse(DropDownListclass.SelectedValue, out classid);
                int.TryParse(DropDownListhouse.SelectedValue, out houseid);
                int.TryParse(Drp_Gender.SelectedValue, out gender);
                GridViewmap.DataSource = null;
                GridViewmap.DataBind();
                housemapDs = MyHouse.GetStudentHouseMapReport(classid, houseid, gender);
                if (housemapDs != null && housemapDs.Tables[0].Rows.Count > 0)
                {

                        GridViewmap.Columns[1].Visible = true;
                        GridViewmap.Columns[2].Visible = true;
                        Paneleditmap.Visible = true;
                        GridViewmap.Visible = true;
                        GridViewmap.DataSource = housemapDs;
                        GridViewmap.DataBind();
                       
                        
                    }
               
                else
                {
                    WC_MessageBox.ShowMssage("No student-house map exist!");
                    Paneleditmap.Visible = false;

                }
                GridViewmap.Columns[1].Visible = false;
                GridViewmap.Columns[2].Visible = false;

            }
            protected void btnshow_click(object sender, EventArgs e)
            {
                Getmapdetails();
                loadhousemapedit();
              
                lblcheck.Text = "";
               
    

            }
            protected void GridViewmap_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                GridViewmap.PageIndex = e.NewPageIndex;
                Getmapdetails();
                loadhousemapedit();
            }


            private void loadhousemapupdate()
            {
                int studid;
                int drplistvalue;
                DropDownList drdList;
                CheckBox chkb;
                GridViewmap.Columns[1].Visible = true;
                GridViewmap.Columns[2].Visible = true;
                foreach (GridViewRow grdRow in GridViewmap.Rows)
                {
                    drdList = (DropDownList)(grdRow.FindControl("DropDownListeditmap"));
                     chkb = (CheckBox)(grdRow.FindControl("chk_select"));
                    

                     if (chkb.Checked)
                     {

                         int.TryParse(grdRow.Cells[1].Text, out studid);
                         drplistvalue = Convert.ToInt32(drdList.SelectedValue);
                         DataSet dseditmap = new DataSet();
                         MyHouse.updatehousestudentmap(drplistvalue, studid);
                         Getmapdetails();
                         MyUser.m_DbLog.LogToDb(MyUser.UserName, "House", "students HouseName changed", 1);
                         WC_MessageBox.ShowMssage("Selected students HouseName changed ");
                     }

                     
                 }
                GridViewmap.Columns[1].Visible = false;
                GridViewmap.Columns[2].Visible = false;
                
            }
           
            protected void Btn_save_Click(object sender, EventArgs e)
            {
                CheckBox chkb;
                lblcheck.Text = "";
                int check = 0;
             
                foreach (GridViewRow grdRow in GridViewmap.Rows)
                {
                    chkb = (CheckBox)(grdRow.FindControl("chk_select"));
                   

                    if (chkb.Checked)
                    {
                        check = 1;
                        loadhousemapupdate();
                        Paneleditmap.Visible = true;
                        lblcheck.Text = "";
                    }
                  
                    
               }
                if (check == 1)
                {
                    loadhousemapedit();
                    GridViewmap.Columns[1].Visible = false;
                    GridViewmap.Columns[2].Visible = false;
                    lblcheck.Text = "";
                }
                else
                {
                    lblcheck.Text = " Please select student";
                }
            } 
        
    }

}
