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
    public partial class SurveyPage : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        private GroupManager MyGroup;
        public static List<string> lst = new List<string>();

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
            else if (!MyUser.HaveActionRignt(3038))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadGroups();
                    LoadSurveyname();
                    LoadSurveyGrid();
                }
            }
        }
        private void LoadGroups()
        {
            DataSet GroupDs = new DataSet();


            try
            {
                GroupDs = MyGroup.GetAllGroup();
                ListItem li;
                if (GroupDs != null && GroupDs.Tables[0].Rows.Count > 0)
                {

                    li = new ListItem("All", "0");
                    Drp_Groupall.Items.Add(li);
                    li = new ListItem("Select", "0");
                    Drp_Group.Items.Add(li);
                    foreach (DataRow dr in GroupDs.Tables[0].Rows)
                    {
                        li = new ListItem(dr["GroupName"].ToString(), dr["Id"].ToString());
                        Drp_Group.Items.Add(li);
                        Drp_Groupall.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("None", "-1");
                    Drp_Group.Items.Add(li);
                    Drp_Groupall.Items.Add(li);

                }
            }
            catch (Exception)
            {
                WC_MessageBox.ShowMssage("Error, Please try later");
            }
        }

        private void LoadSurveyname()
        {
            DataSet GroupDs = new DataSet();


            try
            {
                GroupDs = MyGroup.GetAllSurvey();
                ListItem li;
                if (GroupDs != null && GroupDs.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("Select", "0");
                    Drp_Surveyname.Items.Add(li);
                    foreach (DataRow dr in GroupDs.Tables[0].Rows)
                    {
                        li = new ListItem(dr["Survey_name"].ToString(), dr["Id"].ToString());
                        Drp_Surveyname.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("None", "-1");
                    Drp_Surveyname.Items.Add(li);
                }
            }
            catch (Exception)
            {
                WC_MessageBox.ShowMssage("Error, Please try later");
            }
        }


        public void LoadSurveyGrid()
        {
            DataSet GroupDs = new DataSet();
            try
            {
                GroupDs = MyGroup.Get_Survey_all();
                if (GroupDs != null && GroupDs.Tables[0].Rows.Count > 0)
                {
                    Grid_edit.Columns[0].Visible = true;
                    Grid_edit.Columns[1].Visible = true;
                    Grid_edit.DataSource = GroupDs;
                    Grid_edit.DataBind();
                    Grid_edit.Columns[0].Visible = false;
                    Grid_edit.Columns[1].Visible = false;
                    Grd_Survey.DataSource = GroupDs;
                    Grd_Survey.DataBind();
                    Grd_Survey.Columns[0].Visible = false;
                    Grd_Survey.Columns[1].Visible = false;
                    Lbl_ErrStaffMap.Visible = false;

                }
                else
                {
                    Lbl_ErrStaffMap.Visible = true;
                    Lbl_ErrStaffMap.Text = "No Survey Found";
                    Grd_Survey.DataSource = null;
                    Grd_Survey.DataBind();
                    Grid_edit.DataSource = null;
                    Grid_edit.DataBind();
                }
            }
            catch (Exception)
            {
                WC_MessageBox.ShowMssage("Error, Please try later");
            }
        }
        protected void Lnk_AddSurvey_Click(object sender, EventArgs e)
        {
            load_for_addlink();
        }

        protected void Lnk_EditSurvey_Click(object sender, EventArgs e)
        {
            load_for_editlink();
        }

        protected void Drp_Groupall_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = int.Parse(Drp_Groupall.SelectedValue);
            if (index == 0)
                LoadSurveyGrid();
            else
                LoadSelectedGrpSurvey(index);
        }


        public void LoadSelectedGrpSurvey(int Group_Id)
        {
            DataSet GroupDs = new DataSet();
            try
            {
                GroupDs = MyGroup.Get_Selected_Survey(Group_Id);
                if (GroupDs != null && GroupDs.Tables[0].Rows.Count > 0)
                {
                    Grid_edit.Columns[0].Visible = true;
                    Grid_edit.Columns[1].Visible = true;
                    Grid_edit.DataSource = GroupDs;
                    Grid_edit.DataBind();
                    Grid_edit.Columns[0].Visible = false;
                    Grid_edit.Columns[1].Visible = false;
                    Grd_Survey.Columns[0].Visible = false;
                    Grd_Survey.Columns[1].Visible = false;
                    Grd_Survey.DataSource = GroupDs;
                    Grd_Survey.DataBind();
                    Lbl_ErrStaffMap.Visible = false;
                }
                else
                {
                    Lbl_ErrStaffMap.Visible = true;
                    Lbl_ErrStaffMap.Text = "No Survey Found";
                    Grd_Survey.DataSource = null;
                    Grd_Survey.DataBind();
                    Grid_edit.DataSource = null;
                    Grid_edit.DataBind();
                }
            }
            catch (Exception)
            {
                WC_MessageBox.ShowMssage("Error, Please try later");
            }
        }

        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            int Groupid = int.Parse(Drp_Group.SelectedValue);
            string surveyname = Drp_Surveyname.SelectedItem.Text;
            string Groupname = Drp_Group.SelectedItem.Text;
            string ques_type = Drp_Qtype.SelectedItem.Text;
            string ques = Ques.Text;
            int survey_id = int.Parse(Drp_Surveyname.SelectedValue);
            string ans = string.Join(",", lst.ToArray());
            if (validate())
            {
                try
                {
                    if(Drp_Surveyname.SelectedItem.Text=="None")
                    {
                        WC_MessageBox.ShowMssage("Please add a Survey name!");
                        return;
                    }
                    if (Drp_Qtype.SelectedItem.Text == "TextBox")
                    {
                        lst.Clear();
                        ans = string.Empty;
                        
                    }                        
                    MyGroup.Map_ques(Groupid, survey_id,surveyname, Groupname, ques_type, ques, ans);
                    LoadSelectedGrpSurvey(Groupid);
                    WC_MessageBox.ShowMssage("Survey Inserted Successfully!");
                    lst.Clear();
                    clear_fields();
                }
                catch (Exception)
                {
                    WC_MessageBox.ShowMssage("Error, Please try later");
                }
            }

        }

        public bool validate()
        {

            if (int.Parse(Drp_Surveyname.SelectedValue) == 0)
            {
                WC_MessageBox.ShowMssage("Please select the survey name");
                return false;
            }
            else if (int.Parse(Drp_Group.SelectedValue) == 0)
            {
                WC_MessageBox.ShowMssage("Please select the group name");
                return false;
            }
            else if (String.IsNullOrEmpty(Ques.Text) || Ques.Text.Trim().Length == 0)
            {
                WC_MessageBox.ShowMssage("Please enter the question for survey");
                return false;
            }
            else if (int.Parse(Drp_Qtype.SelectedValue) == 0)
            {
                WC_MessageBox.ShowMssage("Please select the question type");
                return false;
            }
            else if (Convert.ToInt16(Drp_Qtype.SelectedIndex) == 2 || Convert.ToInt16(Drp_Qtype.SelectedIndex) == 3)
            {
                if ((lst.Count <= 1))
                {

                    WC_MessageBox.ShowMssage("Please enter atleast 2 options for the question type selected");
                    return false;
                }
            }
            return true;
        }

        protected void Drp_Group_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = int.Parse(Drp_Group.SelectedValue);
            if (index == 0)
                LoadSurveyGrid();
            else
                LoadSelectedGrpSurvey(index);
        }     

        protected void Grd_Survey_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            int index = int.Parse(Drp_Group.SelectedValue);
            Grd_Survey.PageIndex = e.NewPageIndex;
            if (index == 0)
            {
                LoadSurveyGrid();
            }
            else
                LoadSelectedGrpSurvey(index);
        }

        protected void Grid_edit_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            int index = int.Parse(Drp_Group.SelectedValue);
            Grid_edit.PageIndex = e.NewPageIndex;
            if (index == 0)
            {
                LoadSurveyGrid();
            }
            else
                LoadSelectedGrpSurvey(index);
        }

        public void load_for_addlink()
        {
            try
            {

                //check for dropdownlist count then clear...

                if (Drp_Group.SelectedItem.Text != "None")
                {
                    Drp_Group.SelectedValue = "0";
                }
                else
                {
                    WC_MessageBox.ShowMssage("Please create a Group");
                    return;
                }
                if (Drp_Surveyname.SelectedItem.Text != "None")
                {
                    Drp_Surveyname.SelectedValue = "0";
                }
                else
                {
                    WC_MessageBox.ShowMssage("Please create a Survey");
                    return;
                }

                lbl_Surveyname.Visible = true;
                Drp_Surveyname.Visible = true;
                group_all.Visible = false;
                Drp_Groupall.Visible = false;
                Btn_Add.Visible = true;
                Ques.Visible = true;
                lbl_Qtype.Visible = true;
                Drp_Qtype.Visible = true;
                lbl_ques.Visible = true;
                Drp_Group.Visible = true;
                group_select.Visible = true;
                Grid_edit.Visible = false;
                Grd_Survey.Visible = true;
                Drp_Qtype.SelectedValue = "0";
                Ques.Text = string.Empty;
                Lbl_option.Visible = false;
                Txt_answer.Visible = false;
                Btn_answer.Visible = false;
                Lbl_ErrStaffMap.Visible = false;
                Txt_answer.Text = string.Empty;
                LoadSurveyGrid();
                Btn_Update.Visible = false;               
                lst.Clear();
            }
            catch (Exception)
            {
                WC_MessageBox.ShowMssage("Error, Please try later");
            }
        }
        public void load_for_editlink()
        {
            try
            {
                if (Drp_Group.SelectedItem.Text != "None")
                {
                    Drp_Group.SelectedValue = "0";
                }
                else
                {
                    WC_MessageBox.ShowMssage("Please create a Group");
                    return;
                }
                if (Drp_Surveyname.SelectedItem.Text != "None")
                {
                    Drp_Surveyname.SelectedValue = "0";
                }
                else
                {
                    WC_MessageBox.ShowMssage("Please create a Survey");
                    return;
                }
                lbl_Surveyname.Visible = true;
                Drp_Surveyname.Visible = true;
                group_all.Visible = false;
                Drp_Groupall.Visible = false;
                Btn_Add.Visible = false;
                Ques.Visible = true;
                lbl_Qtype.Visible = true;
                Drp_Qtype.Visible = true;
                lbl_ques.Visible = true;
                Drp_Group.Visible = true;
                group_select.Visible = true;
                Grid_edit.Visible = true;
                Grd_Survey.Visible = false;
                Drp_Qtype.SelectedValue = "0";
                Ques.Text = string.Empty;
                Lbl_option.Visible = false;
                Txt_answer.Visible = false;
                Btn_answer.Visible = false;
                Lbl_ErrStaffMap.Visible = false;
                Txt_answer.Text = string.Empty;
                LoadSurveyGrid();
                lst.Clear();
            }
            catch (Exception)
            {
                WC_MessageBox.ShowMssage("Error, Please try later");
            }
        }

        public void clear_fields()
        {
            Drp_Surveyname.SelectedValue = "0";
            Drp_Group.SelectedValue = "0";
            Ques.Text = string.Empty;
            Drp_Qtype.SelectedValue = "0";
            Lbl_option.Visible = false;
            Txt_answer.Visible = false;
            Btn_answer.Visible = false;
        }

        protected void Drp_Qtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt16(Drp_Qtype.SelectedValue) == 2 || Convert.ToInt16(Drp_Qtype.SelectedValue) == 3)
                {
                    Lbl_option.Visible = true;
                    Txt_answer.Visible = true;
                    Btn_answer.Visible = true;
                    //Btn_View.Visible = true;
                }
                else
                {

                    Lbl_option.Visible = false;
                    Txt_answer.Visible = false;
                    Btn_answer.Visible = false;
                }
            }
            catch
            {
                WC_MessageBox.ShowMssage("Please select the question type");
            }
        }

        protected void Btn_answer_Click(object sender, EventArgs e)
        {
            string ans = Txt_answer.Text;

            if (int.Parse(Drp_Surveyname.SelectedValue) == 0)
            {
                WC_MessageBox.ShowMssage("Please select the survey name");
                return;
            }
            else if (int.Parse(Drp_Group.SelectedValue) == 0)
            {
                WC_MessageBox.ShowMssage("Please select the group name");
                return;
            }
            else if (String.IsNullOrEmpty(Ques.Text) || Ques.Text.Trim().Length == 0)
            {
                WC_MessageBox.ShowMssage("Please enter the question for survey");
                return;
            }
            else if (int.Parse(Drp_Qtype.SelectedValue) == 0)
            {
                WC_MessageBox.ShowMssage("Please select the question type");
                return;
            }
            
                try
                {
                    if (!(string.IsNullOrEmpty(Txt_answer.Text) || Txt_answer.Text.Trim().Length == 0))
                    {

                        lst.Add(ans);
                        //MessageBox Here
                        WC_MessageBox.ShowMssage("No. " + lst.Count + ": " + Txt_answer.Text + " <br/> Added Successfully");
                        Txt_answer.Text = string.Empty;
                    }
                    else
                        WC_MessageBox.ShowMssage("Please input the answer");
                }
                catch (Exception)
                {
                    WC_MessageBox.ShowMssage("Error, Please try later");
                }
            

        }

        protected void Grid_edit_SelectedIndexChanged(object sender, EventArgs e)
        {
            Btn_Add.Visible = false;
            Btn_Update.Visible = true;
            Grid_edit.Columns[0].Visible = true;
            Drp_Group.SelectedIndex = Drp_Group.Items.IndexOf(Drp_Group.Items.FindByText(Grid_edit.SelectedRow.Cells[3].Text));
            Ques.Text = Grid_edit.SelectedRow.Cells[4].Text.Replace("&nbsp;", "");
            Drp_Qtype.SelectedIndex = Drp_Qtype.Items.IndexOf(Drp_Qtype.Items.FindByText(Grid_edit.SelectedRow.Cells[5].Text));
            if (Convert.ToInt16(Drp_Qtype.SelectedIndex) == 2 || Convert.ToInt16(Drp_Qtype.SelectedIndex) == 3)
            {
                Lbl_option.Visible = true;
                Txt_answer.Visible = true;
                Btn_answer.Visible = true;
            }
            else
            {

                Lbl_option.Visible = false;
                Txt_answer.Visible = false;
                Btn_answer.Visible = false;
            }
            Drp_Surveyname.SelectedIndex = Drp_Surveyname.Items.IndexOf(Drp_Surveyname.Items.FindByText(Grid_edit.SelectedRow.Cells[2].Text));
            GroupMapId.Value = Grid_edit.SelectedRow.Cells[0].Text;
            Grid_edit.Columns[0].Visible = false;
        }

        protected void Grid_edit_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int Id = 0;
            try
            {
                int.TryParse(Grid_edit.Rows[e.RowIndex].Cells[0].Text, out Id);
                MyGroup.Delete_Survey(Id);
                LoadSurveyGrid();
                WC_MessageBox.ShowMssage("Deleted Successfully!");
                clear_fields();
                Btn_Add.Visible = false;
                Btn_Update.Visible = false;
            }
            catch
            {
                WC_MessageBox.ShowMssage("Can't delete, Please try again later!");
            }
        }

        protected void Btn_Update_Click(object sender, EventArgs e)
        {
            bool success = true;
            int id = int.Parse(GroupMapId.Value);
            int Groupid = int.Parse(Drp_Group.SelectedValue);
            string Groupname = Drp_Group.SelectedItem.Text;
            string ques_type = Drp_Qtype.SelectedItem.Text;
            string surveyname= Drp_Surveyname.SelectedItem.Text;
            int survey_id = int.Parse(Drp_Surveyname.SelectedValue);
            string ques = Ques.Text;
            string ans = string.Join(",", lst.ToArray());
            try
            {
                if (validate())
                {
                    if (Drp_Surveyname.SelectedItem.Text == "None")
                    {
                        WC_MessageBox.ShowMssage("Please add a survey name!");
                        return;
                    }
                    MyGroup.CreateTansationDb();
                    if (Groupid > 0)
                    {
                        MyGroup.Map_Update(id, Groupid, survey_id, surveyname, Groupname, ques, ques_type, ans);
                    }
                }
                else
                    success = false;
                if (success)
                {
                    MyGroup.EndSucessTansationDb();
                    WC_MessageBox.ShowMssage("Survey edited successfully!");
                    LoadSurveyGrid();
                    lst.Clear();
                    clear_fields();
                }
            }
            catch (Exception)
            {
                MyGroup.EndFailTansationDb();
                WC_MessageBox.ShowMssage("Can't edit survey, Please try later!");
                success = false;
            }
        }

        //protected void Btn_View_Click(object sender, EventArgs e)
        //{
        //    mpePopUp.Show();
        //}

        //protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        //{
        //    mpePopUp.Hide();
        //}
    }
}
