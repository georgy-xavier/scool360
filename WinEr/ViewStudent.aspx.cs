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
public partial class ViewStudent : System.Web.UI.Page
{
    private StudentManagerClass MyStudMang;
    private KnowinUser MyUser;
    //private OdbcDataReader MyReader = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyStudMang = MyUser.GetStudentObj();
        if (MyStudMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }
        else
        {


            if (!IsPostBack)
            {
                string _MenuStr;
                _MenuStr = MyStudMang.GetStudentMangMenuString(MyUser.UserRoleId);
                this.StudentMenu.InnerHtml = _MenuStr;

                //some initlization

            }
        }

    }
  
    protected void bindGridView()
    {
        string sql = "SELECT Id,UserName FROM tbluser";
        DataSet myDataSet;

        try
        {
            myDataSet = MyStudMang.m_MysqlDb.ExecuteQuery(sql,"Class");
            
                // Delay the current Thread to display
                // the UpdateProgress status
                // Not required in real projects
                //System.Threading.Thread.Sleep(4000);

                GridView1.DataSource = myDataSet;
                GridView1.DataBind();
          
            
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = ex.Message;
        }
    }

    protected void btnLoadData_Click(object sender, EventArgs e)
    {
        bindGridView();
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        bindGridView();
    }

}
