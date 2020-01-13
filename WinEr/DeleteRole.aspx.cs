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
public partial class DeleteRole : System.Web.UI.Page
{
    private KnowinRole MyRole;
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
        MyRole = User.GetRoleObj();
        if (MyRole == null)
        {
            Response.Redirect("Default.aspx");
            //no rights for this user.
        }
        else
        {
            //some initlization
            if (!IsPostBack)
            {
                AddRoleToGrid();
            }
        }
    }

    private void AddRoleToGrid()
    {
        string sql = "SELECT Id,RoleName,Discription FROM tblrole";
        MyReader = MyRole.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {

            Grd_Role.DataSource = MyReader;
            Grd_Role.DataBind();

        }
    }
    protected void Grd_Role_SelectedIndexChanged(object sender, EventArgs e)
    {

        int i_SelectedRoleId = int.Parse(Grd_Role.SelectedRow.Cells[1].Text.ToString());
        Set_Role(i_SelectedRoleId);
    }

    private void Set_Role(int i_SelectedRoleId)
    {
        Lbl_FailureNote.Text = "";
        string sql = "SELECT Id,RoleName FROM tblrole where Id=" + i_SelectedRoleId + "";
        MyReader = MyRole.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            MyReader.Read();
            TxtRoleId.Text = MyReader.GetValue(0).ToString();
            TxtRoleName.Text = MyReader.GetValue(1).ToString();
        }

        MyReader.Close();
    }
    protected void BtnDeleteRole_Click(object sender, EventArgs e)
    {
        if (TxtRoleId.Text != "")
        {
            if (TxtRoleName.Text != "Administrator")
            {
                if (MyRole.HasUser(int.Parse(TxtRoleId.Text)))
                {
                    Lbl_FailureNote.Text = "There are users in " + TxtRoleName.Text + " role..";
                }
                else
                {
                    MyRole.DeleteRole(int.Parse(TxtRoleId.Text));
                    Lbl_FailureNote.Text = "Role is Deleted";
                    AddRoleToGrid();
                    clear();
                }
            }
            else
            {
                Lbl_FailureNote.Text = "Administrator cannot be Deleted";
            }

        }
        else
        {
            Lbl_FailureNote.Text = "Please select a Role from grid";
        }
    }
    void clear()
    {
        TxtRoleId.Text = "";
        TxtRoleName.Text = "";
    }
}
