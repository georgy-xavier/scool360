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
    public partial class GroupStaffMap : System.Web.UI.Page
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
            else if (!MyUser.HaveActionRignt(3014))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {

                    LoadGroups();

                    //load staff
                    LoadAllStaff();

                }
            }
        }
        private void LoadGroups()
        {
            DataSet GroupDs = new DataSet();
            Drp_StaffGroup.Items.Clear();

            try
            {
                GroupDs = MyGroup.GetAllGroup();
                ListItem li;
                if (GroupDs != null && GroupDs.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("Select", "0");
                    Drp_StaffGroup.Items.Add(li);
                    li = new ListItem("All", "0");
                    foreach (DataRow dr in GroupDs.Tables[0].Rows)
                    {
                        li = new ListItem(dr["GroupName"].ToString(), dr["Id"].ToString());
                        Drp_StaffGroup.Items.Add(li);

                    }
                }
                else
                {
                    li = new ListItem("None", "-1");
                    Drp_StaffGroup.Items.Add(li);
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

                    DataSet Ds_staffGroup = new DataSet();
                    DataTable dt_staff = new DataTable();
                    dt_staff.Columns.Add("Id", typeof(string));
                    dt_staff.Columns.Add("SurName", typeof(string));
                    dt_staff.Columns.Add("UserName", typeof(string));
                    dt_staff.Columns.Add("RoleName", typeof(string));
                    dt_staff.Columns.Add("Groups", typeof(string));
                    DataRow dr_staff = null;

                    foreach (DataRow dr in StaffDetails_Ds.Tables[0].Rows)
                    {
                        string Grp_Names = "";
                        dr_staff = dt_staff.NewRow();
                        dr_staff["Id"] = dr["Id"].ToString();
                        dr_staff["SurName"] = dr["SurName"].ToString();
                        dr_staff["UserName"] = dr["UserName"].ToString();
                        dr_staff["RoleName"] = dr["RoleName"].ToString();
                        //group names
                        DataSet ds_groups = new DataSet();
                        ds_groups = MyGroup.get_staffgroups(int.Parse(dr["Id"].ToString()));
                        if (ds_groups != null && ds_groups.Tables != null && ds_groups.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr_group in ds_groups.Tables[0].Rows)
                            {
                                if (Grp_Names == "")
                                {
                                    Grp_Names = dr_group["GroupName"].ToString();
                                }
                                else
                                {
                                    Grp_Names = Grp_Names + "," + dr_group["GroupName"].ToString();
                                }
                            }
                        }
                        else
                        {
                            Grp_Names = "No Group";
                        }
                        dr_staff["Groups"] = Grp_Names;
                        dt_staff.Rows.Add(dr_staff);

                    }
                    Ds_staffGroup.Tables.Add(dt_staff);
                    Grd_StaffList.Columns[4].Visible = true;
                    Lbl_ErrStaffMap.Text = "";
                    Grd_StaffList.DataSource = Ds_staffGroup;
                    Grd_StaffList.DataBind();
                    Grd_StaffList.Columns[4].Visible = false;

                }
                else
                {
                    Lbl_ErrStaffMap.Text = "No staff is pending for mapping";
                    Grd_StaffList.DataSource = null;
                    Grd_StaffList.DataBind();
                }
                Btn_StaffGroupMap.Visible = true;
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
        protected void Btn_StaffGroupMap_Click(Object sender, EventArgs e)
        {

            int groupid = 0,userid = 0, usertype = 1;
            string msg = "";
            string username = "";
            try
            {
                if (check_validations(out groupid, out msg))
                {
                    CheckBox chk = new CheckBox();
                    foreach (GridViewRow gr in Grd_StaffList.Rows)
                    {
                        chk = (CheckBox)gr.FindControl("chk_select");
                        if (chk.Checked)
                        {
                            int.TryParse(gr.Cells[4].Text, out userid);
                            username = gr.Cells[1].Text;
                            MyGroup.MapUser(usertype, userid, username, groupid);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Group", "staff:"+username+" mapped with "+Drp_StaffGroup.SelectedItem.Text+"", 1);
                            WC_MessageBox.ShowMssage("User mapped successfully");
                        }
                    }
                    LoadAllStaff();
                     
                }
                else
                {
                    Lbl_ErrStaffMap.Text = msg;
                }
            }
            catch (Exception es)
            {
                Lbl_ErrStaffMap.Text = es.Message;
            }


        }
        private bool check_validations(out int groupid, out string _msg)
        {
            int chkcount = 0, userid = 0;
            bool validate = true;
            _msg = "";
            groupid = 0;
            int.TryParse(Drp_StaffGroup.SelectedValue, out groupid);
            CheckBox chk = new CheckBox();
            if (groupid <= 0)
            {
                _msg = "Select Group!";
                validate = false;
            }
            if (validate)
            {
                foreach (GridViewRow gr in Grd_StaffList.Rows)
                {
                    chk = (CheckBox)gr.FindControl("chk_select");
                    if (chk.Checked)
                    {
                        chkcount++;
                        int.TryParse(gr.Cells[4].Text, out userid);
                        //check already exist
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
                    _msg = "Select one or more staff!";
                    validate = false;
                }
            }
            return validate;
        }
    }
}
