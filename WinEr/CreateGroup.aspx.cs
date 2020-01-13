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

public partial class CreateGroup : System.Web.UI.Page
{
    //private string Str_Connection = "Driver={MySQL ODBC 3.51 Driver};Server=localhost;Database=knowindb;User=root;Password=win;";
    private KnowinGroup MyGroup ;
    private OdbcDataReader MyReader = null;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
  
    protected void Page_init(object sender, EventArgs e)
    {
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
    
    private void AddUserToCbList()
    {
        Drp_UserList.Items.Clear();

        string sql = "SELECT Id,UserName FROM tbluser where Status=1";
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
        }
        Drp_UserList.SelectedIndex = 0;
    }
    private void AddGroupToGrid()
    {

        string sql = "SELECT Id,GroupName,Discription,CreatedDate FROM tblgroup";
        MyReader = MyGroup.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {

            Grd_Group.DataSource = MyReader;
             Grd_Group.DataBind();
           
        }


    }
    protected void Btn_CreateGroup_Click(object sender, EventArgs e)
    {
        if (Txt_GpName.Text.Trim() == "" || Txt_GpDiscr.Text.Trim() == "")
        {
            Lbl_FailureNote.Text="One Or more fields are empty";
        }
        else if (Drp_UserList.SelectedValue=="-1")
        {
            Lbl_FailureNote.Text = "Group must need a Manager";
        }
        else
        {
            string _GroupName = Txt_GpName.Text.Replace("\'", "\\'");
            if (MyGroup.ValidadGroupName(_GroupName))
            {
                MyGroup.CreateNewGroup(_GroupName, Txt_GpDiscr.Text.ToString(), int.Parse(Drp_ParentList.SelectedValue), int.Parse(Drp_UserList.SelectedValue), int.Parse(Drp_GroupTypeList.SelectedValue));
            MyGroup.ADDMember(MyGroup.m_GroupId, int.Parse(Drp_UserList.SelectedValue.ToString()));      
            Lbl_FailureNote.Text = "Group is Created";
            AddGroupToGrid();
            AddGroupToCbList();
            clear();
            }
            else
            {
                Lbl_FailureNote.Text = "Group Name is Allready Present, Please try another one...";
            }
          
        }
        
    }
    void clear()
    {
        Txt_GpDiscr.Text = "";
        Txt_GpName.Text = "";
    } 
}
