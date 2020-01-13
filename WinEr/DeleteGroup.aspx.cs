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
public partial class DeleteGroup : System.Web.UI.Page
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
                AddGroupToGrid();
            }

        }


    }
    
    private void AddGroupToGrid()
    {

        //string sql = "SELECT Id,GroupName,Discription,ModifiedDate FROM tblgroup";
        string sql = "SELECT Id,GroupName,ModifiedDate FROM tblgroup";
        MyReader = MyGroup.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {

            Grd_Grp.DataSource = MyReader;
            Grd_Grp.DataBind();

        }


    }
  
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Lbl_FailureNote.Text = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton l = (LinkButton)e.Row.FindControl("LinkButton1");

            l.Attributes.Add("onclick", "javascript:return " +
                 "confirm('Deleting the group " +
                 DataBinder.Eval(e.Row.DataItem, "GroupName") + " will remove all its members and data')");

        }
    }
   
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int GroupID = int.Parse(Grd_Grp.DataKeys[e.RowIndex].Value.ToString());
        DeleteRecordByID(GroupID);

    }

    private void DeleteRecordByID(int GroupID)
    {  
        string _errMsg;
    if (MyGroup.CanDelete(GroupID, out _errMsg))
    {
        MyGroup.DeleteGroup(GroupID);
        AddGroupToGrid();
        Lbl_FailureNote.Text = "Group Is deleted";
    }
    else
    {
        Lbl_FailureNote.Text = _errMsg;
    }
    }
    
   
}
