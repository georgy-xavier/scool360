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
    public partial class GroupUserMap : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        private GroupManager MyGroup;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyGroup = MyUser.GetGroupManagerObj();
            if (MyGroup == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(910))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {

                    LoadGroups();

                    //load staff
                    LoadGroupUserMap();
                    Grd_EditStaffGroup.Columns[0].Visible = false;
                    if (Grd_EditStaffGroup.Rows.Count > 0)
                    {
                        Lnk_EditUser.Visible = true;
                        Img_EditUser.Visible = true;
                    }
                    else
                    {
                        Lnk_EditUser.Visible = false;
                        Img_EditUser.Visible = false;
                    }
                    rowadd.Visible = false;
                   // rowEdit.Visible = false;
                    Btn_StaffGroupUpdate.Visible = false;
                    Btn_StaffGroupMap.Visible = false;
                    Btn_DeleteStaff.Visible = false;
                    Lbl_StaffToGroup.Text = "Group:";   

                    //Load student
                    LoadGroupStudentMap();
                    Lnk_StudentMapAdd.Visible = true;
                    Img_StudentMapAdd.Visible = true;
                    if (Grd_studentEditMap.Rows.Count > 0)
                    {
                        Lnk_StudentMapEdit.Visible = true;
                        Img_StudentMapEdit.Visible = true;
                    }
                    else
                    {
                        Lnk_StudentMapEdit.Visible = false;
                        Img_StudentMapEdit.Visible = false;
                    }

                    Grd_studentEditMap.Columns[0].Visible = false;
                    LoadClassToDropDown();
                    LblStudGroup.Text = "Group:";
                   // rowgroupfrom.Visible = false;
                    rowgroupto.Visible = false;
                    rowclass.Visible = false;
                    Btn_StudGroupUpdate.Visible = false;
                    Btn_StudentGroupMap.Visible = false;
                    Btn_deletestudent.Visible = false;

                }
            }
        }

        #region  StudentGroupmap
                
            protected void Lnk_StudentMapAdd_Click(object sender, EventArgs e)
            {
                try
                {                
                    LoadStudents();
                    Lnk_StudentMapEdit.Visible = true;
                    Lnk_StudentMapAdd.Visible = false;
                    Img_StudentMapEdit.Visible = true;
                    Img_StudentMapAdd.Visible = false;
                    Lbl_StudErr.Text = "";
                    rowclass.Visible = true;
                    rowgroupto.Visible = true;
                    rowgroupfrom.Visible = false;
                    LblStudGroup.Text = "Group:";
                    Btn_deletestudent.Visible = false;
                    Btn_StudGroupUpdate.Visible = false;
                    Btn_StudentGroupMap.Visible = true;
                }
                catch (Exception)
                {
                    WC_MessageBox.ShowMssage("Error,Please try later!");
                }
               

            }

            protected void Lnk_StudentMapEdit_Click(object sender, EventArgs e)
            {
                try
                {
                    LoadGroupStudentMap();
                    Lnk_StudentMapEdit.Visible = false;
                    rowgroupfrom.Visible = true;
                    Lnk_StudentMapAdd.Visible = true;
                    Img_StudentMapEdit.Visible = false;
                    Img_StudentMapAdd.Visible = true;
                    Lbl_StudErr.Text = "";
                    rowclass.Visible = true;
                    rowgroupfrom.Visible = true;
                    rowgroupto.Visible = true;
                    LblStudGroup.Text = "Group To:";
                    Btn_deletestudent.Visible = true;
                    Btn_StudGroupUpdate.Visible = true;
                    Btn_StudentGroupMap.Visible = false;

                }
                catch (Exception)
                {
                    WC_MessageBox.ShowMssage("Error,Please try later!");
                }
            }           

            protected void grd_studList_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                grd_studList.PageIndex = e.NewPageIndex;
                LoadStudents();
            }

            protected void Btn_StudentGroupMap_Click(object sender, EventArgs e)
            {
                int classid = 0, groupid = 0, userid = 0,chkcount=0;
                string username="";
                bool success = true;
                CheckBox chk = new CheckBox();
                try
                {
                    int.TryParse(Drp_StudentClass.SelectedValue, out classid);
                    int.TryParse(Drp_StudentGroup.SelectedValue, out groupid);
                    if (classid < 0)
                    {
                        Lbl_StudErr.Text = "No class found!";
                        success = false;
                      
                    }
                    else if (groupid <= 0)
                    {
                        Lbl_StudErr.Text = "Select Group!";
                        success = false;
                    }
                    else
                    {
                        MyGroup.CreateTansationDb();
                        foreach (GridViewRow gr in grd_studList.Rows)
                        {
                            chk = (CheckBox)gr.FindControl("chk_select");
                            if (chk.Checked)
                            {
                                chkcount++;
                                int.TryParse(gr.Cells[5].Text, out userid);
                                username=gr.Cells[2].Text;
                                MyGroup.MapUser(0, userid, username, groupid);                           
                            }
                        }
                        if (chkcount == 0)
                        {
                            MyGroup.EndFailTansationDb();
                            Lbl_StudErr.Text = "Select one or more student!";
                            success = false;
                        }
                    }
                  
                }
                catch (Exception)
                {
                    MyGroup.EndFailTansationDb();
                    Lbl_StudErr.Text = "Can't map,Please try later!";
                    success = false;
                }
                if (success)
                {
                    MyGroup.EndSucessTansationDb();
                    WC_MessageBox.ShowMssage("User mapped successfully");
                    LoadStudents();

                }

            }

            protected void Btn_StudGroupUpdate_Click(object sender, EventArgs e)
            {
                int groupid = 0, chkcount = 0, userid = 0, usertype = 0, GroupTableid = 0, fromgroupid = 0;
                bool success = true;
                string username = "";
                try
                {
                    MyGroup.CreateTansationDb();
                    int.TryParse(Drp_StudentGroup.SelectedValue, out groupid);
                    int.TryParse(Drp_StudGroupFrom.SelectedValue, out fromgroupid);
                    CheckBox chk = new CheckBox();

                    if (groupid > 0)
                    {
                        if (fromgroupid == groupid)
                        {
                            MyGroup.EndFailTansationDb();
                            WC_MessageBox.ShowMssage("Can't map to the same group!");
                            success = false;

                        }
                        else
                        {
                            foreach (GridViewRow gr in Grd_studentEditMap.Rows)
                            {
                                chk = (CheckBox)gr.FindControl("chk_select");
                                if (chk.Checked)
                                {
                                    chkcount++;
                                    int.TryParse(gr.Cells[5].Text, out userid);
                                    int.TryParse(gr.Cells[6].Text, out GroupTableid);
                                    username = gr.Cells[2].Text;
                                    MyGroup.UpdateGroupUserMap(usertype, userid, username, groupid, GroupTableid);
                                }
                            }


                            if (chkcount == 0)
                            {
                                MyGroup.EndFailTansationDb();
                                Lbl_StudErr.Text = "Please select one or more student";
                                success = false;
                            }

                        }
                    }
                    else
                    {
                        MyGroup.EndFailTansationDb();
                        WC_MessageBox.ShowMssage("Please select group to!");
                        success = false;
                    }

                }
                catch (Exception)
                {
                    MyGroup.EndFailTansationDb();
                    WC_MessageBox.ShowMssage("Can't map,Please try later!");
                    success = false;
                }
                if (success)
                {
                    MyGroup.EndSucessTansationDb();
                    LoadGroups();
                    WC_MessageBox.ShowMssage("Student mapped successfully!");
                    LoadGroupStudentMap();
                }
            }

            protected void Btn_deletestudent_Click(object sender, EventArgs e)
            {
                int chkcount = 0, GroupTableid = 0;
                bool success = true;
                try
                {
                    MyGroup.CreateTansationDb();
                    CheckBox chk = new CheckBox();


                    foreach (GridViewRow gr in Grd_studentEditMap.Rows)
                    {
                        chk = (CheckBox)gr.FindControl("chk_select");
                        if (chk.Checked)
                        {
                            chkcount++;
                            int.TryParse(gr.Cells[6].Text, out GroupTableid);
                            MyGroup.DeleteGroupUserMap(GroupTableid);
                        }
                    }


                    if (chkcount == 0)
                    {
                        MyGroup.EndFailTansationDb();
                        Lbl_StudErr.Text = "Please select one or more student";
                        success = false;
                    }
                }
                catch (Exception)
                {
                    MyGroup.EndFailTansationDb();
                    WC_MessageBox.ShowMssage("Can't map,Please try later!");
                    success = false;
                }
                if (success)
                {
                    MyGroup.EndSucessTansationDb();
                    LoadGroups();
                    WC_MessageBox.ShowMssage("Student mapped successfully!");
                    LoadGroupStudentMap();
                }
            }

            protected void Drp_StudentClass_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (Lnk_StudentMapAdd.Visible == false)
                {
                    LoadStudents();
                }
                else
                {
                    LoadGroupStudentMap();
                }
            }

            protected void Drp_StudGroupFrom_SelectedIndexChanged(object sender, EventArgs e)
            {
                LoadGroupStudentMap();
            }

            private void LoadGroupStudentMap()
            {
                DataSet StudentGroupDs = new DataSet();
                int GroupId = 0, classid = 0;
                int.TryParse(Drp_StudGroupFrom.SelectedValue, out GroupId);
                int.TryParse(Drp_StudentClass.SelectedValue, out classid);
                StudentGroupDs = MyGroup.GetAllStudentInGroup(GroupId, classid, 0);
                if (StudentGroupDs != null && StudentGroupDs.Tables[0].Rows.Count > 0)
                {
                    Grd_studentEditMap.Columns[6].Visible = true;
                    Grd_studentEditMap.Columns[1].Visible = true;
                    Grd_studentEditMap.Columns[5].Visible = true;
                    Grd_studentEditMap.Columns[0].Visible = true;
                    Lbl_StudErr.Text = "";
                    Grd_studentEditMap.DataSource = StudentGroupDs;
                    Grd_studentEditMap.DataBind();
                    Grd_studentEditMap.Columns[6].Visible = false;
                    Grd_studentEditMap.Columns[5].Visible = false;
                    Grd_studentEditMap.Columns[1].Visible = false;
                }
                else
                {
                    Lbl_StudErr.Text = "No student found";
                    Grd_studentEditMap.DataSource = null;
                    Grd_studentEditMap.DataBind();
                }
                grd_studList.DataSource = null;
                grd_studList.DataBind();
                
            }

            private void LoadClassToDropDown()
            {

                DataSet ClassDs = new DataSet();
                ListItem li;
                Drp_StudentClass.Items.Clear();
                ClassDs = MyUser.MyAssociatedClass();
                if (ClassDs != null && ClassDs.Tables != null && ClassDs.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("All", "0");
                    Drp_StudentClass.Items.Add(li);
                    foreach (DataRow dr in ClassDs.Tables[0].Rows)
                    {
                        li = new ListItem(dr[1].ToString(), dr[0].ToString());
                        Drp_StudentClass.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("No Class Present", "-1");
                    Drp_StudentClass.Items.Add(li);
                }
                Drp_StudentClass.SelectedIndex = 0;

            }

            private void LoadStudents()
            {
                int classid = 0;
                Lbl_StudErr.Text = "";
                DataSet StudDs = new DataSet();
                int.TryParse(Drp_StudentClass.SelectedValue, out classid);
                try
                {
                    if (classid >= 0)
                    {
                        StudDs = MyGroup.getStudentsFromClass(classid);
                        if (StudDs != null && StudDs.Tables[0].Rows.Count > 0)
                        {
                            grd_studList.Columns[1].Visible = true;
                            grd_studList.Columns[5].Visible = true;
                            grd_studList.DataSource = StudDs;
                            grd_studList.DataBind();
                            grd_studList.Columns[5].Visible = false;
                            grd_studList.Columns[1].Visible = false;

                        }
                        else
                        {
                            Lbl_StudErr.Text = "No student is pending for mapping!!";
                            grd_studList.DataSource = null;
                            grd_studList.DataBind();
                        }
                    }
                    else
                    {
                        grd_studList.DataSource = null;
                        grd_studList.DataBind();

                    }
                    Grd_studentEditMap.DataSource = null;
                    Grd_studentEditMap.DataBind();
                    Btn_StudentGroupMap.Visible = true;
                    Btn_StudGroupUpdate.Visible = false;
                }
                catch (Exception)
                {
                    Lbl_StudErr.Text = "Error.Cannot load..!";
                }

            }

        #endregion

        #region StaffGroupMap

          

            protected void Lnk_EditUser_Click(object sender, EventArgs e)
            {
                edituser();
                /*
                 *    Btn_StaffGroupMap.Visible = false;
                Btn_StaffGroupUpdate.Visible = false;
                Btn_DeleteStaff.Visible = false;
                 */
            }

            private void edituser()
            {
                try
                {
                    rowadd.Visible = true;
                    rowEdit.Visible = true;
                    Lbl_ErrStaffMap.Text = "";
                    LoadGroupUserMap();
                    Lnk_EditUser.Visible = false;
                    Lnk_AddNewUer.Visible = true;
                    Img_EditUser.Visible = false;
                    Img_AddUser.Visible = true;
                    Lbl_StaffToGroup.Text = "Group To:";
                    Btn_StaffGroupUpdate.Visible = true;
                    Btn_DeleteStaff.Visible = true;
                    Btn_StaffGroupMap.Visible = false;
                }
                catch (Exception)
                {
                    WC_MessageBox.ShowMssage("Error,Please try later!");
                }
            }

            protected void Lnk_AddNewUer_Click(object sender, EventArgs e)
            {
                try
                {
                    rowadd.Visible = true;
                    rowEdit.Visible = false;
                    Btn_StaffGroupUpdate.Visible = false;
                    Btn_StaffGroupMap.Visible = true;
                    Lbl_ErrStaffMap.Text = "";
                    LoadAllStaff();
                    Lnk_EditUser.Visible = true;
                    Lnk_AddNewUer.Visible = false;
                    Img_EditUser.Visible = true;
                    Img_AddUser.Visible = false;
                    Lbl_StaffToGroup.Text = "Group:";
                    Btn_DeleteStaff.Visible = false;
                }
                catch (Exception)
                {
                    WC_MessageBox.ShowMssage("Error,Please try later!");
                }

            }
           
            protected void Grd_StaffList_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                Grd_StaffList.PageIndex = e.NewPageIndex;
                LoadAllStaff();
            }

            protected void Grd_EditStaffGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {

                Grd_EditStaffGroup.PageIndex = e.NewPageIndex;
                LoadGroupUserMap();
            }

            protected void Btn_StaffGroupUpdate_Click(object sender, EventArgs e)
            {
                int groupid = 0, chkcount = 0, userid = 0, usertype = 1, GroupTableid = 0, fromgroupid = 0;
                bool success = true;
                string username = "";
                try
                {
                    MyGroup.CreateTansationDb();
                    int.TryParse(Drp_StaffGroup.SelectedValue, out groupid);
                    int.TryParse(Drp_StaffGroupFrom.SelectedValue, out fromgroupid);
                    CheckBox chk = new CheckBox();

                    if (groupid > 0)
                    {
                        if (fromgroupid == groupid)
                        {
                            WC_MessageBox.ShowMssage("Can't map to the same group!");
                            success = false;
                            MyGroup.EndFailTansationDb();
                        }
                        else
                        {

                            foreach (GridViewRow gr in Grd_EditStaffGroup.Rows)
                            {
                                chk = (CheckBox)gr.FindControl("chk_select");
                                if (chk.Checked)
                                {
                                    chkcount++;
                                    int.TryParse(gr.Cells[4].Text, out userid);
                                    int.TryParse(gr.Cells[5].Text, out GroupTableid);
                                    username = gr.Cells[1].Text;
                                    MyGroup.UpdateGroupUserMap(usertype, userid, username, groupid, GroupTableid);
                                }
                            }


                            if (chkcount == 0)
                            {
                                MyGroup.EndSucessTansationDb();
                                WC_MessageBox.ShowMssage("Please select staff!");
                                success = false;
                            }

                        }
                    }
                    else
                    {
                        MyGroup.EndFailTansationDb();
                        WC_MessageBox.ShowMssage("Please select group to!");
                        success = false;
                    }

                }
                catch (Exception)
                {
                    MyGroup.EndFailTansationDb();
                    WC_MessageBox.ShowMssage("Can't map user,Please try later!");
                    success = false;
                }
                if (success)
                {
                    MyGroup.EndSucessTansationDb();
                    WC_MessageBox.ShowMssage("User mapped successfully!");
                    LoadGroupUserMap();
                }
                edituser();
            }

            protected void Btn_DeleteStaff_Click(object sender, EventArgs e)
            {
                int chkcount = 0,GroupTableid = 0;
                bool success = true;
                try
                {
                    MyGroup.CreateTansationDb();
                    CheckBox chk = new CheckBox();
                    foreach (GridViewRow gr in Grd_EditStaffGroup.Rows)
                    {
                        chk = (CheckBox)gr.FindControl("chk_select");
                        if (chk.Checked)
                        {
                            chkcount++;
                            int.TryParse(gr.Cells[5].Text, out GroupTableid);
                            MyGroup.DeleteGroupUserMap(GroupTableid);
                        }
                    }


                    if (chkcount == 0)
                    {
                        MyGroup.EndSucessTansationDb();
                        WC_MessageBox.ShowMssage("Please select staff!");
                        success = false;
                    }

                }
                catch (Exception)
                {
                    MyGroup.EndFailTansationDb();
                    WC_MessageBox.ShowMssage("Can't map user,Please try later!");
                    success = false;
                }
                if (success)
                {
                    MyGroup.EndSucessTansationDb();
                    LoadGroups();
                    WC_MessageBox.ShowMssage("User deleted successfully!");
                    LoadGroupUserMap();
                }                
            }

            protected void Drp_StaffGroupFrom_SelectedIndexChanged(object sender, EventArgs e)
            {
                LoadGroupUserMap();
            }

            protected void Btn_StaffGroupMap_Click(Object sender, EventArgs e)
            {
                int groupid = 0, chkcount = 0, userid = 0, usertype = 1;
                bool success = true;
                string username = "";
                try
                {
                    MyGroup.CreateTansationDb();
                    int.TryParse(Drp_StaffGroup.SelectedValue, out groupid);
                    CheckBox chk = new CheckBox();

                    if (groupid > 0)
                    {                       
                        foreach (GridViewRow gr in Grd_StaffList.Rows)
                        {
                            chk = (CheckBox)gr.FindControl("chk_select");
                            if (chk.Checked)
                            {
                                chkcount++;
                                int.TryParse(gr.Cells[4].Text, out userid);
                                username = gr.Cells[1].Text;
                                MyGroup.MapUser(usertype, userid, username, groupid);
                            }
                        }

                        if (chkcount == 0)
                        {
                            MyGroup.EndFailTansationDb();
                            WC_MessageBox.ShowMssage("Please select staff!");
                            success = false;
                        }                        
                    }
                    else
                    {
                        MyGroup.EndFailTansationDb();
                        WC_MessageBox.ShowMssage("Please select group!");
                        success = false;
                    }
                }
                catch (Exception)
                {
                    MyGroup.EndFailTansationDb();
                    WC_MessageBox.ShowMssage("Can't map user,Please try later!");
                    success = false;
                }
                if (success)
                {
                    MyGroup.EndSucessTansationDb();
                    WC_MessageBox.ShowMssage("User mapped successfully!");
                    LoadAllStaff();
                }




            }

            private void LoadGroups()
            {
                DataSet GroupDs = new DataSet();
                Drp_StaffGroup.Items.Clear();
                    Drp_StaffGroupFrom.Items.Clear();
                    Drp_StudentGroup.Items.Clear();
                    Drp_StudGroupFrom.Items.Clear();
                  
                try
                {
                    GroupDs = MyGroup.GetAllGroup();
                    ListItem li;
                    if (GroupDs != null && GroupDs.Tables[0].Rows.Count > 0)
                    {
                        li = new ListItem("Select", "0");
                        Drp_StaffGroup.Items.Add(li);
                        Drp_StudentGroup.Items.Add(li);
                        li = new ListItem("All", "0");
                        Drp_StaffGroupFrom.Items.Add(li);
                        Drp_StudGroupFrom.Items.Add(li);
                        foreach (DataRow dr in GroupDs.Tables[0].Rows)
                        {
                            li = new ListItem(dr["GroupName"].ToString(), dr["Id"].ToString());
                            Drp_StaffGroup.Items.Add(li);
                            Drp_StaffGroupFrom.Items.Add(li);
                            Drp_StudentGroup.Items.Add(li);
                            Drp_StudGroupFrom.Items.Add(li);
                           
                        }
                    }
                    else
                    {
                        li = new ListItem("None", "-1");
                        Drp_StaffGroup.Items.Add(li);
                        Drp_StaffGroupFrom.Items.Add(li);
                        Drp_StudentGroup.Items.Add(li);
                        Drp_StudGroupFrom.Items.Add(li);
                       
                    }
                }
                catch (Exception)
                {
                    WC_MessageBox.ShowMssage("Error,Please try later");
                }
            }

            private void LoadAllStaff()
            {
                DataSet StaffDetails_Ds = new DataSet();
                try
                {
                    StaffDetails_Ds = MyGroup.GetStaffDetails(MyUser.User_Id);
                    if (StaffDetails_Ds != null && StaffDetails_Ds.Tables[0].Rows.Count > 0)
                    {
                        Grd_StaffList.Columns[4].Visible = true;
                        Lbl_ErrStaffMap.Text = "";
                        Grd_StaffList.DataSource = StaffDetails_Ds;
                        Grd_StaffList.DataBind();
                        Grd_StaffList.Columns[4].Visible = false;
                    }
                    else
                    {
                        Lbl_ErrStaffMap.Text = "No staff is pending for mapping";
                        Grd_StaffList.DataSource = null;
                        Grd_StaffList.DataBind();
                    }
                    Grd_EditStaffGroup.DataSource = null;
                    Grd_EditStaffGroup.DataBind();
                    Btn_StaffGroupMap.Visible = true;
                    Btn_StaffGroupUpdate.Visible = false;
                }
                catch (Exception)
                {
                    WC_MessageBox.ShowMssage("Error,Please try later!");
                }
            }

            private void LoadGroupUserMap()
            {
                DataSet StaffGroupDs = new DataSet();
                int GroupId = 0;
                int.TryParse(Drp_StaffGroupFrom.SelectedValue, out GroupId);
                StaffGroupDs = MyGroup.GetAllStaffInGroup(GroupId, 1);
                if (StaffGroupDs != null && StaffGroupDs.Tables[0].Rows.Count > 0)
                {
                    Grd_EditStaffGroup.Columns[4].Visible = true;
                    Grd_EditStaffGroup.Columns[5].Visible = true;
                    Grd_EditStaffGroup.Columns[0].Visible = true;
                    Lbl_ErrStaffMap.Text = "";
                    Grd_EditStaffGroup.DataSource = StaffGroupDs;
                    Grd_EditStaffGroup.DataBind();
                    Grd_EditStaffGroup.Columns[4].Visible = false;
                    Grd_EditStaffGroup.Columns[5].Visible = false;
                }
                else
                {
                    Lbl_ErrStaffMap.Text = "No staff found";
                    Grd_EditStaffGroup.DataSource = null;
                    Grd_EditStaffGroup.DataBind();
                }
                Grd_StaffList.DataSource = null;
                Grd_StaffList.DataBind();
             
            }


        #endregion

          
    }
}
