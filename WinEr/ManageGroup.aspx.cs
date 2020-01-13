using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Odbc;

public partial class ManageGroup : System.Web.UI.Page
{
    private KnowinGroup MyGroup;
    private OdbcDataReader MyReader = null;
    //private int i_CurrentParentId ;
    //private string Str_CurrentGroupName;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_init(object sender, EventArgs e)
    {
        //i_CurrentParentId = -1;
       // Str_CurrentGroupName = "";
        if (Session["UserObj"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        KnowinUser User = (KnowinUser)Session["UserObj"];
        MyGroup = User.GetGroupObj();
        if (MyGroup == null)
        {
            Response.Redirect("Default.aspx");
            //no rights for this user.
        }
        else
        {
            if (!IsPostBack)
            {
                AddGroupToCbList();
                AddGroupToGrid();
                AddUserToCbList();
                AddGroupTypeToCbList();
            }
        }

    }
   
    protected void Page_UnLoad(object sender, EventArgs e)
    {
        if (MyGroup != null)
        {
            MyGroup = null;
        }
        if (MyReader != null)
        {
            MyReader = null;
       
        }
        
    }
    private void AddGroupTypeToCbList()
    {

        Drp_GroupTypeList.Items.Clear();
        string sql = "SELECT Id,TypeName FROM tblgrouptype";
        MyReader = MyGroup.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                Drp_GroupTypeList.Items.Add(li);
            }
            Drp_GroupTypeList.SelectedIndex = 0;
        }

    }
    private void AddGroupToCbList()
    {

        Drp_ParentList.Items.Clear();
        Drp_ParentList.Items.Add(new ListItem("None", "-1"));
        Drp_ParentList.SelectedIndex = 0;
        string sql = "SELECT Id,GroupName FROM tblgroup";
        MyReader = MyGroup.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                Drp_ParentList.Items.Add(li);
            }
        }

    }

    private void AddUserToCbList()
    {
        Drp_UserList.Items.Clear();
        
        string sql = "SELECT Id,UserName FROM tbluser";
        MyReader = MyGroup.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                Drp_UserList.Items.Add(li);
            }
        }
        else
        {
            Drp_UserList.Items.Add(new ListItem("None", "-1"));
            Drp_UserList.SelectedIndex = 0;
        }

    }
    private void AddGroupToGrid()
    {

        string sql = "SELECT Id,GroupName,Discription,ModifiedDate FROM tblgroup";
        MyReader = MyGroup.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {

            Grd_Group.DataSource = MyReader;
            Grd_Group.DataBind();

        }


    }
   
    protected void Set_Group(int i_GpId)
    {
        Lbl_FailureNote.Text = "";
        string sql = "SELECT Id,GroupName,Discription,ParentId,ManagerId,GroupTypeId FROM tblgroup where Id=" + i_GpId + "";
        MyReader = MyGroup.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            MyReader.Read();
            Txt_GpId.Text = MyReader.GetValue(0).ToString();
            MyGroup.m_GroupName=MyReader.GetValue(1).ToString();
            Txt_GpName.Text = MyReader.GetValue(1).ToString();
            Txt_GpDiscr.Text = MyReader.GetValue(2).ToString();
            MyGroup.m_ParentId=int.Parse( MyReader.GetValue(3).ToString());
            Drp_ParentList.SelectedValue =MyReader.GetValue(3).ToString();
            Drp_UserList.SelectedValue = MyReader.GetValue(4).ToString();
            Drp_GroupTypeList.SelectedValue = MyReader.GetValue(5).ToString();  
      }

        MyReader.Close();

    }
    protected void Grd_Group_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i_SelectedGroupId = int.Parse(Grd_Group.SelectedRow.Cells[1].Text.ToString());
        Set_Group(i_SelectedGroupId);

    }
    protected void Grd_Group_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='Orange';this.style.cursor='hand'";
            e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Grd_Group, "Select$" + e.Row.RowIndex);
        }

    }
    protected void Btn_CreateGroup_Click(object sender, EventArgs e)
    {
        if (Txt_GpId.Text != "")
        {
            if (Txt_GpName.Text.Trim() == "" || Txt_GpDiscr.Text.Trim() == "")
            {
                Lbl_FailureNote.Text = "One Or more fields are empty";
            }
            else if (Drp_UserList.SelectedValue == "-1")
            {
                Lbl_FailureNote.Text = "Group must need a Manager";
            }
            else
            {
                string _GroupName = Txt_GpName.Text.Replace("\'", "\\'");
                if ((MyGroup.ValidadGroupName(_GroupName)) || (MyGroup.m_GroupName == Txt_GpName.Text))
                {
                    if (int.Parse(Drp_ParentList.SelectedValue) != int.Parse(Txt_GpId.Text))
                    {
                        if (MyGroup.VadidateChild(int.Parse(Txt_GpId.Text), int.Parse(Drp_ParentList.SelectedValue)) == false)
                        {
                            MyGroup.UpdateGroup(int.Parse(Txt_GpId.Text), _GroupName, Txt_GpDiscr.Text.ToString(), int.Parse(Drp_ParentList.SelectedValue), int.Parse(Drp_UserList.SelectedValue), int.Parse(Drp_GroupTypeList.SelectedValue));
                            Lbl_FailureNote.Text = "Group is Updated";
                            AddGroupToGrid();
                            AddGroupToCbList();
                            Drp_ParentList.SelectedValue = MyGroup.m_ParentId.ToString();
                        }
                        else
                        {
                            Lbl_FailureNote.Text = "The parent Group selected is already a child of this group...";
                        }
                    }
                    else
                    {
                        Lbl_FailureNote.Text = "Same group cannot be parent group...";
                    }
                }
                else
                {
                    Lbl_FailureNote.Text = "Group Name is Allready Present, Please try another one...";
                }

            }
        }
        else
        {
            Lbl_FailureNote.Text = "Please select a Group from grid";
        }

    }

    
}
