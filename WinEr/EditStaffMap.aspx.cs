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
    public partial class EditStaffMap : System.Web.UI.Page
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
            else if (!MyUser.HaveActionRignt(3015))
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
                }
            }

        }
        private void LoadGroups()
        {
            DataSet GroupDs = new DataSet();
            Drp_StaffGroup.Items.Clear();
            Drp_StaffGroupFrom.Items.Clear();

            try
            {
                GroupDs = MyGroup.GetAllGroup();
                ListItem li;
                if (GroupDs != null && GroupDs.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("Select", "0");
                    Drp_StaffGroup.Items.Add(li);
                    li = new ListItem("All", "0");
                    Drp_StaffGroupFrom.Items.Add(li);
                    foreach (DataRow dr in GroupDs.Tables[0].Rows)
                    {
                        li = new ListItem(dr["GroupName"].ToString(), dr["Id"].ToString());
                        Drp_StaffGroup.Items.Add(li);
                        Drp_StaffGroupFrom.Items.Add(li);

                    }
                }
                else
                {
                    li = new ListItem("None", "-1");
                    Drp_StaffGroup.Items.Add(li);
                    Drp_StaffGroupFrom.Items.Add(li);

                }
            }
            catch (Exception)
            {
                WC_MessageBox.ShowMssage("Error,Please try later");
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

        }
        protected void Drp_StaffGroupFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGroupUserMap();
        }
        protected void Btn_StaffGroupUpdate_Click(object sender, EventArgs e)
        {
            int groupid = 0, userid = 0, usertype = 1, GroupTableid = 0;
            string username = "";
            string msg="";
            try
            {
                if (update_validations(out groupid, out msg))
                {
                    CheckBox chk = new CheckBox();
                    foreach (GridViewRow gr in Grd_EditStaffGroup.Rows)
                    {
                        chk = (CheckBox)gr.FindControl("chk_select");
                        if (chk.Checked)
                        {
                            int.TryParse(gr.Cells[4].Text, out userid);
                            int.TryParse(gr.Cells[5].Text, out GroupTableid);
                            username = gr.Cells[1].Text;
                            MyGroup.UpdateGroupUserMap(usertype, userid, username, groupid, GroupTableid);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Group", ""+username+" Group map editted", 1);
                            WC_MessageBox.ShowMssage("Group map editted successfully");

                        }
                    }
                    LoadGroupUserMap();
                }
                else
                {
                    Lbl_ErrStaffMap.Text = msg;
                }
                
            }
            catch (Exception ed)
            {
                Lbl_ErrStaffMap.Text = ed.Message;
            }
        }
        protected void Btn_DeleteStaff_Click(object sender, EventArgs e)
        {
            int chkcount = 0, GroupTableid = 0;
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
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Group", ""+gr.Cells[1].Text+" deleted from Group", 1);
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
        protected void Grd_EditStaffGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_EditStaffGroup.PageIndex = e.NewPageIndex;
            LoadGroupUserMap();
        }
        private bool update_validations(out int groupid, out string _msg)
        {
            int chkcount = 0, userid = 0,fromgroupid = 0;
            groupid = 0;
            _msg = "";
            bool validate = true;
            int.TryParse(Drp_StaffGroup.SelectedValue, out groupid);
            int.TryParse(Drp_StaffGroupFrom.SelectedValue, out fromgroupid);
            CheckBox chk = new CheckBox();

            if (groupid > 0)
            {
                if (fromgroupid == groupid)
                {
                    _msg = "Can't map to the same group!";
                    validate = false;
                }
            }
            else
            {
                _msg = "select group";
                validate = false;

            }
            if (validate)
            {
                foreach (GridViewRow gr in Grd_EditStaffGroup.Rows)
                {
                    chk = (CheckBox)gr.FindControl("chk_select");
                    if (chk.Checked)
                    {
                        chkcount++;
                        int.TryParse(gr.Cells[4].Text, out userid);
                        DataSet ds_check = new DataSet();
                        ds_check = MyGroup.Is_staffgroupexist(userid, groupid);
                        if (ds_check != null && ds_check.Tables != null && ds_check.Tables[0].Rows.Count > 0)
                        {
                            validate = false;
                            _msg = "mapping failed,Because users already present in this group";
                        }
                    }
                }

                if (chkcount == 0)
                {
                    _msg = "Please select one or more staff";
                    validate = false;
                }
            }
            return validate;

        }
    }
}
