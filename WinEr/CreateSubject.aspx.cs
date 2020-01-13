using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;

public partial class CreateSubject : System.Web.UI.Page
{
    private ConfigManager MyConfigMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    protected void Page_PreInit(Object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        MyUser = (KnowinUser)Session["UserObj"];

        if (MyUser.SELECTEDMODE == 1)
        {
            this.MasterPageFile = "~/WinerStudentMaster.master";

        }
        else if (MyUser.SELECTEDMODE == 2)
        {

            this.MasterPageFile = "~/WinerSchoolMaster.master";
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_init(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyConfigMang = MyUser.GetConfigObj();
        if (MyConfigMang == null)
        {
            Response.Redirect("RoleErr.htm");
        }
        else if (!MyUser.HaveActionRignt(14))
        {
            Response.Redirect("RoleErr.htm");
        }
        else
        {
            if (!IsPostBack)
            {
                // SubjectTypeLoading();
                 AddSubjectToGrid();
                 LoadSubjectGroup();
            }
        }
    }

    private void LoadSubjectGroup()
    {

        Drp_Subgrp.Items.Clear();

        string sql = "SELECT Id, Name from tbltime_subgroup";
        MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                Drp_Subgrp.Items.Add(li);
            }
        }
        else
        {
            ListItem li = new ListItem("No Subject Group Found", "-1");
            Drp_Subgrp.Items.Add(li);
        }
    }

   
    //private void SubjectTypeLoading()
    //{
    //    Drp_SubjectType.Items.Clear();

    //    string sql = "SELECT Id, Name from tbltime_subgroup";
    //    MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sql);
    //    if (MyReader.HasRows)
    //    {
    //        while (MyReader.Read())
    //        {
    //            ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
    //            Drp_SubjectType.Items.Add(li);
    //        }
    //    }
    //    else
    //    {
    //        ListItem li = new ListItem("No Subject Type Found", "-1");
    //        Drp_SubjectType.Items.Add(li);
    //    }
    //}
    

    private void AddSubjectToGrid()
    {
        GridSubjects.Columns[4].Visible = true;
        string sql = "Select tblsubjects.Id As 'SubId', tblsubjects.subject_name, tblsubjects.SubjectCode,tbltime_subgroup.Name as subtype,tblsubjects.Combined FROM tblsubjects INNER JOIN tbltime_subgroup on tblsubjects.sub_Catagory= tbltime_subgroup.Id ";
        MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sql);
        GridSubjects.Columns[0].Visible = true;
        if (MyReader.HasRows)
        {
            GridSubjects.DataSource = MyReader;
            GridSubjects.DataBind();
            GridSubjects.Columns[4].Visible = false;
            Pnl_sublist.Visible = true;
        }
        else
        {
            Pnl_sublist.Visible = false;
        }
        GridSubjects.Columns[0].Visible = false;
    }

    protected void DrpSubType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void BtnCreate_Click(object sender, EventArgs e)
    {
        if (TxtSubName.Text.Trim() != "")
        {
            string sql = "Select subject_name from tblsubjects where subject_name='" + TxtSubName.Text + "'";
            MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Lbl_msg.Text = "Subject Already Exist";
            }
            else if (Txt_SubCode.Text.Trim() != "")
            {
                sql = "Select SubjectCode from tblsubjects where SubjectCode='" + Txt_SubCode.Text + "'";
                MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sql);
                MyReader.Read();
                if (MyReader.HasRows)
                {
                    Lbl_msg.Text = "Subject Code Already Exist";
                }

                else
                {
                    if (Hdn_Comb.Value == "0")
                    {
                        MyConfigMang.CreateSubject(TxtSubName.Text.ToString(), Txt_SubCode.Text.ToString(), int.Parse(Drp_Subgrp.SelectedValue));
                    }
                    else
                    {
                        MyConfigMang.CreateCombSubject(TxtSubName.Text.ToString(), Txt_SubCode.Text.ToString(), int.Parse(Drp_Subgrp.SelectedValue), Hdn_SubIds.Value);
                    }
                    Lbl_msg.Text = "Subject is created";
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create Subject", "One Subject " + TxtSubName.Text.ToString() + " is created", 1);
                    clear();
                    AddSubjectToGrid();
                }
            }
            else
            {
                Lbl_msg.Text = "Subject code cannot be empty";
            }
        }
        else
        {
            Lbl_msg.Text = "Subject name cannot be empty";
        }
        this.MPE_MessageBox.Show();
    }

    private void clear()
    {
            
        TxtSubName.Text = "";
        //TxtSubDis.Text = "";
        Txt_SubCode.Text = "";

    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string _message="";
        int _subID = int.Parse(GridSubjects.DataKeys[e.RowIndex].Value.ToString());
        int Type = int.Parse(GridSubjects.Rows[e.RowIndex].Cells[4].Text.ToString());

        if ((Type ==0)&&(MyConfigMang.DeleteSubjectById(_subID, out _message)))
        {
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Delete Subject", "One Subject:" + GridSubjects.Rows[e.RowIndex].Cells[1].Text.ToString() + " is deleted", 1);
            AddSubjectToGrid();
            Lbl_msg.Text = _message;
           
        }
        else if (Type == 1)
        {
            MyConfigMang.DeleteCombinedSubject(_subID, out _message);
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Delete Subject", "One Subject:" + GridSubjects.Rows[e.RowIndex].Cells[1].Text.ToString() + " is deleted", 1);
            AddSubjectToGrid();
            Lbl_msg.Text = _message;
          
        }
        else
        {
            Lbl_msg.Text = _message;
        }
        this.MPE_MessageBox.Show();

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton l = (LinkButton)e.Row.FindControl("LinkButton1");
            l.Attributes.Add("onclick", "javascript:return " +
                 "confirm('Are you sure you want to delete the subject " +
                 DataBinder.Eval(e.Row.DataItem, "subject_name") + " ')");
        }
    }

    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ConfigurationHome.aspx");
    }

    protected void Lnk_Combined_Click(object sender, EventArgs e)
    {
        this.MPE_CombinedSub.Show();
        AddSubjectToChkList();
    }

    private void AddSubjectToChkList()
    {
        Lbl_CombMessage.Text = "";
        ChkBox_AllsSub.Items.Clear();

        string sql = "SELECT Id,subject_name FROM tblsubjects where  Combined<>1";
        MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                ChkBox_AllsSub.Items.Add(li);
            }
        }
        else
        {
            Lbl_CombMessage.Text = "No subject found";
        }
    }


    protected void Btn_Combine_Click(object sender, EventArgs e)
    {
        string SubName = "";
        string SubId = "";
        for (int i = 0; i < ChkBox_AllsSub.Items.Count; i++)
        {
            if (ChkBox_AllsSub.Items[i].Selected)
            {
                if (SubName != "")
                {
                    SubName = SubName + "/" + ChkBox_AllsSub.Items[i].Text;
                    SubId = SubId + "\\" + ChkBox_AllsSub.Items[i].Value;
                    
                }
                else
                {
                    SubName = SubName + ChkBox_AllsSub.Items[i].Text;
                    SubId = SubId + ChkBox_AllsSub.Items[i].Value;
                }
            }
           
        }
        Hdn_Comb.Value = "1";
        Hdn_SubIds.Value = SubId;
        TxtSubName.Text = SubName;
    }

    protected void ChkBox_AllsSub_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.MPE_CombinedSub.Show();
    }

    protected void lnkaddcategory_Click(object sender, EventArgs e)
    {
        this.ModalPopupExtenderaddgroup.Show();
        txtaddgroup.Text = "";
        lbladdgrouperror.Text = "";
    }
    protected void lnkremoveGroup_Click(object sender, EventArgs e)
    {
        loadgridremove();
        this.ModalPopupExtenderremovegroup.Show();
        lblgroupremoveerror.Text = "";
    }
    protected void btnadd_group_Click(object sender, EventArgs e)
    {
        this.ModalPopupExtenderaddgroup.Show();
        if (txtaddgroup.Text.Trim() != "")
        {
            string sql = "Select Id from tbltime_subgroup where Name='" + txtaddgroup.Text + "'";
            MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                lbladdgrouperror.Text = "Group Already Exist";
            }
            else
            {
                MyConfigMang.creategroup(txtaddgroup.Text.ToString());
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create Group", "One Group: " + txtaddgroup.Text.ToString() + " is created", 1);
                lbladdgrouperror.Text = "Group successfully created";

            }
            txtaddgroup.Text = "";
            LoadSubjectGroup();
        }
        else
        {
            lbladdgrouperror.Text = "Please Enter Group Name";
        }
    }
    private void loadgridremove()
    {
        string sql = "Select Id,Name FROM tbltime_subgroup";
        MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            Grdremovegroup.DataSource = MyReader;
            Grdremovegroup.DataBind();
            Grdremovegroup.Columns[1].Visible = false;
        }
        else
        {
            lblgroupremoveerror.Text = "No Group Exists";
        }
    }
    protected void Grdremovegroup_rowdeleting(object sender, GridViewDeleteEventArgs e)
    {
        Grdremovegroup.Columns[1].Visible =true;
        this.ModalPopupExtenderremovegroup.Show();
        int group_id = int.Parse(Grdremovegroup.DataKeys[e.RowIndex].Values["Id"].ToString());
        if (removegrouphassubjects(group_id))
        {
            MyConfigMang.removegroup(group_id);
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Remove Group", "Group: " + Grdremovegroup.DataKeys[e.RowIndex].Values["Name"].ToString() + " removed", 1);
            lblgroupremoveerror.Text = "group remove successfully";
        }
        else
        {
            lblgroupremoveerror.Text = "cannot remove group,group has subjects";
        }
        Grdremovegroup.Columns[1].Visible = false;
        loadgridremove();
        LoadSubjectGroup();
    }
   
    private bool removegrouphassubjects(int grpid)
    {
        bool check_sub = false;
        string sqlcheck = "SELECT Id FROM tblsubjects where sub_Catagory=" + grpid + "";
        MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sqlcheck);
        if (MyReader.HasRows)
        {
            check_sub = false;
        }
        else
        {
            check_sub =true;
        }
        return check_sub;
    }
}

