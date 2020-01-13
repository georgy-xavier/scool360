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
public partial class AddMembers : System.Web.UI.Page
{
    private KnowinGroup MyGroup;
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
                AddGroupToCbList(0);
                AddUserToCheckBoxList();
            }
   
        }
    }


    protected void Button2_Click(object sender, EventArgs e)
    {
        Lbl_Note.Text = "";
        for (int i = 0; i <ChkBoxUser.Items.Count; i++)
        {
            if (ChkBoxUser.Items[i].Selected)
            {
               
                    MyGroup.ADDMember(int.Parse(Drp_Group.SelectedValue.ToString()), int.Parse(ChkBoxUser.Items[i].Value.ToString()));
                    ChkBoxUser.Items[i].Selected = false;
                    ChkBoxGrpMemb.Items.Add(ChkBoxUser.Items[i]);
                    ChkBoxUser.Items.Remove(ChkBoxUser.Items[i]);
                    i--;
                    
               
            }
        }
    }
    private void AddGroupToCbList(int _intex)
    {

        Drp_Group.Items.Clear();
        string sql = "SELECT Id,GroupName FROM tblgroup";
        MyReader = MyGroup.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());             
                Drp_Group.Items.Add(li);
                
            }
        }
        else 
        {
           
                ListItem li = new ListItem("NoGroup", "-1");
                Drp_Group.Items.Add(li);
                Btn_Add.Enabled = false;
                Btn_Remove.Enabled = false;
            
        }
        Drp_Group.SelectedIndex = _intex;

    }
    private void AddUserToCheckBoxList()
    {

        ChkBoxGrpMemb.Items.Clear();
        ChkBoxUser.Items.Clear();
        DataSet myDataset;
        string sql = "SELECT Id,UserName FROM tbluser where Status=1";
        MyReader = MyGroup.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            myDataset = MyGroup.GroupMembers(int.Parse( Drp_Group.SelectedValue.ToString()));
            while (MyReader.Read())
            {
                if (MyGroup.IsGroupMember(myDataset, int.Parse(MyReader.GetValue(0).ToString())))
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    ChkBoxGrpMemb.Items.Add(li);
                    //ChkBoxGrpMemb.SelectedIndex = 0;
                }
                else
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    ChkBoxUser.Items.Add(li);
                }
            }
        }

    }
    protected void Drp_Group_SelectedIndexChanged(object sender, EventArgs e)
    {
        Lbl_Note.Text = "";
        AddUserToCheckBoxList();
    }
    protected void Btn_Remove_Click(object sender, EventArgs e)
    {
        Lbl_Note.Text = "";
        for (int i = 0; i < ChkBoxGrpMemb.Items.Count; i++)
        {
            if (ChkBoxGrpMemb.Items[i].Selected)
            {
                if (!MyGroup.IsManager(int.Parse(Drp_Group.SelectedValue.ToString()), int.Parse(ChkBoxGrpMemb.Items[i].Value.ToString())))
                {
                MyGroup.DeleteMember(int.Parse(Drp_Group.SelectedValue.ToString()), int.Parse(ChkBoxGrpMemb.Items[i].Value.ToString()));
                ChkBoxGrpMemb.Items[i].Selected = false;
                ChkBoxUser.Items.Add(ChkBoxGrpMemb.Items[i]);
                ChkBoxGrpMemb.Items.Remove(ChkBoxGrpMemb.Items[i]);
                i--;
                }
                
                else
                {
                    Lbl_Note.Text = "Manager of the group cannot be removed";
                }
            }
        }
    }
}
