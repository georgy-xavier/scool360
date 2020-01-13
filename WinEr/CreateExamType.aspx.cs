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
public partial class CreateSubjectType : System.Web.UI.Page
{
   // private ExamManage MyExamMang;
    private ExamManage MyExamMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyExamMang = MyUser.GetExamObj();

        if (MyExamMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }

        else if (!MyUser.HaveActionRignt(23))
        {
            Response.Redirect("RoleErr.htm");
        }
        else
        {
            if (!IsPostBack)
            {
                //string _MenuStr;
                //_MenuStr = MyExamMang.GetExamMangMenuString(MyUser.UserRoleId);
                //this.ExammngMenu.InnerHtml = _MenuStr;
                //_MenuStr = MyUser.GetDetailsString(23);
                //this.ActionInfo.InnerHtml = _MenuStr;
                AddSubjectTypeToGrid();
            }

        }
    }
   
    private void AddSubjectTypeToGrid()
    {
        string sql = "SELECT sbject_type,TypeDisc,Id FROM tblsubject_type";
        MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        GridSubjectType.Columns[0].Visible = true;
        if (MyReader.HasRows)
        {
           GridSubjectType.DataSource = MyReader;
           GridSubjectType.DataBind();
        }
        GridSubjectType.Columns[0].Visible = false;
    }

  
    private void Clear()
    {
        TxtSubTypeDescription.Text = "";
        TxtSubTypeName.Text = "";
    }
    protected void BtnAddSubTyp_Click(object sender, EventArgs e)
    {
        string _msg = "";
        string sql = "Select sbject_type from tblsubject_type where sbject_type='" + TxtSubTypeName.Text.ToString().Trim() + "'";
        MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        MyReader.Read();
        if (MyReader.HasRows)
        {
            _msg = "Exam Type Already Exist";
        }
        else if (TxtSubTypeName.Text.Trim()=="")
        {
            _msg = "Please enter an Exam type";
        }
        else
        {
   
            MyExamMang.CreateExamType(TxtSubTypeName.Text.ToString().Trim(), TxtSubTypeDescription.Text.ToString());
            AddSubjectTypeToGrid();
            _msg = "Exam Type is created";
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create Exam Type", "An Exam Type " + TxtSubTypeName .Text+ "is created", 1);
            Clear();
        }
        
        WC_MessageBox.ShowMssage(_msg);
    }
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string _message, _msg="";
        int _subTypeID = int.Parse(GridSubjectType.DataKeys[e.RowIndex].Value.ToString());
       
        if (_subTypeID == 1||_subTypeID==2||_subTypeID==3||_subTypeID==4)
        {
            _msg = "Exam Type can not be deleted";
        }
        else
        {
            if (MyExamMang.DeleteExamTypeById(_subTypeID, out _message))
            {
                AddSubjectTypeToGrid();
                _msg = _message;
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Delete Exam Type", "An Exam Type is deleted", 1);
            }
            else
            {
                _msg = _message;
            }
        }
        WC_MessageBox.ShowMssage(_msg);
   
    }
 
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton _LinkButton1 = (LinkButton)e.Row.FindControl("LinkButton1");
            _LinkButton1.Attributes.Add("onclick", "javascript:return " +
                 "confirm('Are you sure you want to delete " +
                 DataBinder.Eval(e.Row.DataItem, "sbject_type") + "')");
        }
    }
}
