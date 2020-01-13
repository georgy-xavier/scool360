using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WinBase;

namespace WinEr
{
    public partial class EditStudentMap : System.Web.UI.Page
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
            else if (!MyUser.HaveActionRignt(3013))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {

                    LoadGroups();
                    LoadGroupStudentMap();
                    LoadClassToDropDown();

                }
            }

        }
        private void LoadGroups()
        {
            DataSet GroupDs = new DataSet();
            Drp_StudentGroup.Items.Clear();
            Drp_StudGroupFrom.Items.Clear();

            try
            {
                GroupDs = MyGroup.GetAllGroup();
                ListItem li;
                if (GroupDs != null && GroupDs.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("Select", "0");
                    Drp_StudentGroup.Items.Add(li);
                    li = new ListItem("All", "0");
                    Drp_StudGroupFrom.Items.Add(li);
                    foreach (DataRow dr in GroupDs.Tables[0].Rows)
                    {
                        li = new ListItem(dr["GroupName"].ToString(), dr["Id"].ToString());
                        Drp_StudentGroup.Items.Add(li);
                        Drp_StudGroupFrom.Items.Add(li);

                    }
                }
                else
                {
                    li = new ListItem("None", "-1");
                    Drp_StudentGroup.Items.Add(li);
                    Drp_StudGroupFrom.Items.Add(li);

                }
            }
            catch (Exception)
            {
                WC_MessageBox.ShowMssage("Error,Please try later");
            }
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

        }
        protected void Drp_StudentClass_SelectedIndexChanged(object sender, EventArgs e)
        {
             LoadGroupStudentMap();
        }
        protected void Drp_StudGroupFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGroupStudentMap();
        }
        protected void grd_studList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_studentEditMap.PageIndex = e.NewPageIndex;
            LoadGroupStudentMap();
        }
        protected void Btn_StudGroupUpdate_Click(object sender, EventArgs e)
        {
            int groupid = 0, userid = 0, usertype = 0, GroupTableid = 0;
            string msg = "";
            string username = "";
            try
            {
                if (validate_update(out groupid, out GroupTableid, out msg))
                {
                    CheckBox chk = new CheckBox();
                    foreach (GridViewRow gr in Grd_studentEditMap.Rows)
                    {
                        chk = (CheckBox)gr.FindControl("chk_select");
                        if (chk.Checked)
                        {
                            int.TryParse(gr.Cells[5].Text, out userid);
                            int.TryParse(gr.Cells[6].Text, out GroupTableid);
                            username = gr.Cells[2].Text;
                            MyGroup.UpdateGroupUserMap(usertype, userid, username, groupid, GroupTableid);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Group", "" + username + " Group map editted", 1);
                            WC_MessageBox.ShowMssage("Group map editted successfully");
                        }
                    }
                    LoadGroupStudentMap();
                }
                else
                {
                    Lbl_StudErr.Text = msg;
                }
            }
            catch (Exception er)
            {
                Lbl_StudErr.Text = er.Message;
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
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Group", "Student Removed from Group", 1);
                WC_MessageBox.ShowMssage("Student Removed from Group successfully!");
                LoadGroupStudentMap();
            }
        }
        private bool validate_update(out int groupid, out int fromgroupid, out string _msg)
        {
            bool validate = true;
            groupid = 0;
            fromgroupid = 0;
            _msg = "";
            int chkcount = 0, userid = 0;
            int.TryParse(Drp_StudentGroup.SelectedValue, out groupid);
            int.TryParse(Drp_StudGroupFrom.SelectedValue, out fromgroupid);
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

                foreach (GridViewRow gr in Grd_studentEditMap.Rows)
                {
                    chk = (CheckBox)gr.FindControl("chk_select");
                    if (chk.Checked)
                    {
                        chkcount++;
                        int.TryParse(gr.Cells[5].Text, out userid);

                        DataSet ds_check = new DataSet();
                        ds_check = MyGroup.Is_usergroupexist(userid, groupid);
                        if (ds_check != null && ds_check.Tables != null && ds_check.Tables[0].Rows.Count > 0)
                        {

                            validate = false;
                            _msg = "mapping failed,Because users already present in this group";
                        }
                    }
                }

                if (chkcount == 0)
                {
                    _msg = "Please select one or more student";
                    validate = false;
                }
            }

            return validate;
        }
    }
}
