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
    public partial class GroupStudentMap : System.Web.UI.Page
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
            else if (!MyUser.HaveActionRignt(3012))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {

                    LoadGroups();
                    LoadClassToDropDown();
                    LoadStudents();
                    //Load student

                }
            }
        }
        private void LoadGroups()
        {
            DataSet GroupDs = new DataSet();
            Drp_StudentGroup.Items.Clear();
            try
            {
                GroupDs = MyGroup.GetAllGroup();
                ListItem li;
                if (GroupDs != null && GroupDs.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("Select", "0");
                    Drp_StudentGroup.Items.Add(li);
                    li = new ListItem("All", "0");
                    foreach (DataRow dr in GroupDs.Tables[0].Rows)
                    {
                        li = new ListItem(dr["GroupName"].ToString(), dr["Id"].ToString());
                        Drp_StudentGroup.Items.Add(li);

                    }
                }
                else
                {
                    li = new ListItem("None", "-1");
                    Drp_StudentGroup.Items.Add(li);
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

        protected void Drp_StudentClass_SelectedIndexChanged(object sender, EventArgs e)
        {
             LoadStudents();
        }
        protected void Btn_StudentGroupMap_Click(object sender, EventArgs e)
        {
            int classid = 0, groupid = 0, userid = 0;
            string msg = "";
            string username = "";
            CheckBox chk = new CheckBox();
            try
            {
                if (check_validates(out groupid,out classid,out  msg ))
                {
                    foreach (GridViewRow gr in grd_studList.Rows)
                    {
                        chk = (CheckBox)gr.FindControl("chk_select");
                        if (chk.Checked)
                        {
                            int.TryParse(gr.Cells[5].Text, out userid);
                            username = gr.Cells[2].Text;
                            MyGroup.MapUser(0, userid, username, groupid);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Group", ""+ username +" mapped with "+Drp_StudentGroup.SelectedItem.Text+"", 1);
                            WC_MessageBox.ShowMssage("User mapped successfully");

                        }
                    }
                    LoadStudents();
                }
                else
                {
                    Lbl_StudErr.Text = msg;
                }
            }
            catch (Exception et)
            {
                Lbl_StudErr.Text = et.Message;
            }



        }
        protected void grd_studList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd_studList.PageIndex = e.NewPageIndex;
            LoadStudents();
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
                        DataSet Ds_Group = new DataSet();
                        DataTable dt_student = new DataTable();
                        dt_student.Columns.Add("Id", typeof(string));
                        dt_student.Columns.Add("StudentName", typeof(string));
                        dt_student.Columns.Add("RollNo", typeof(string));
                        dt_student.Columns.Add("Sex", typeof(string));
                        dt_student.Columns.Add("Address", typeof(string));
                        dt_student.Columns.Add("Groups", typeof(string));
                        DataRow dr_map = null;

                        foreach (DataRow dr in StudDs.Tables[0].Rows)
                        {
                            string Grp_Names = "";
                            dr_map = dt_student.NewRow();
                            dr_map["Id"] = dr["Id"].ToString();
                            dr_map["StudentName"] = dr["StudentName"].ToString();
                            dr_map["RollNo"] = dr["RollNo"].ToString();
                            dr_map["Sex"] = dr["Sex"].ToString();
                            dr_map["Address"] = dr["Address"].ToString();

                            //group names
                            DataSet ds_groups = new DataSet();
                            ds_groups = MyGroup.get_studentgroups(int.Parse(dr["Id"].ToString()));
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
                            dr_map["Groups"] = Grp_Names;
                            dt_student.Rows.Add(dr_map);

                        }
                        Ds_Group.Tables.Add(dt_student);

                        grd_studList.Columns[1].Visible = true;
                        grd_studList.Columns[5].Visible = true;
                        grd_studList.DataSource = Ds_Group;
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
            }
            catch (Exception)
            {
                Lbl_StudErr.Text = "Error.Cannot load..!";
            }
        }
        private bool check_validates(out int groupid,out int classid,out string _msg)
        {
            classid = 0;
            groupid = 0;
            int  userid = 0, chkcount = 0;
            _msg = "";
            bool validate = true;
            CheckBox chk = new CheckBox();
            int.TryParse(Drp_StudentClass.SelectedValue, out classid);
            int.TryParse(Drp_StudentGroup.SelectedValue, out groupid);
            if (classid < 0)
            {
                _msg = "No class found!";
                validate = false;
            }
            if (groupid <= 0)
            {
                _msg = "Select Group!";
                validate = false;
            }
            if (validate)
            {
                foreach (GridViewRow gr in grd_studList.Rows)
                {
                    chk = (CheckBox)gr.FindControl("chk_select");
                    if (chk.Checked)
                    {
                        chkcount++;
                        int.TryParse(gr.Cells[5].Text, out userid);
                        //check user already present in group
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
                    _msg = "Select one or more student!";
                    validate = false;
                }
            }


            return validate;

        }
    }
}
