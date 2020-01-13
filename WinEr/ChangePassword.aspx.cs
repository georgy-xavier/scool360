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
public partial class ChangePassword : System.Web.UI.Page
{
    private ConfigManager MyConfiMang;
    private KnowinUser MyUser;
   // private OdbcDataReader MyReader = null;
   // private DataSet MydataSet;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
  
    protected void Page_PreInit(Object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        else
        {
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

    }

    protected void Page_init(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyConfiMang = MyUser.GetConfigObj();
        if (MyConfiMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }
        else if (!MyUser.HaveActionRignt(32))
        {
            Response.Redirect("RoleErr.htm");
        }
        else
        {
            if (!IsPostBack)
            {
            }
        }
    }

    protected void BtnChngePwd_Click(object sender, EventArgs e)
    {
        if (TxtNewPwd.Text.Trim() == "" || TxtOldPwd.Text.Trim() == "")
        {
            WC_MessageBox.ShowMssage("Please Enter old and new password");
            
        }
        else if (TxtNewPwd.Text != TxtConfirmPwd.Text )
        {
            WC_MessageBox.ShowMssage("Passwords do not match");
            
        }
        else
        {
            string _message;
            if (MyUser.ChangePassWord(TxtOldPwd.Text.ToString(), TxtNewPwd.Text.ToString(), out _message))
            {
                WC_MessageBox.ShowMssage(_message);
                
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Reset Password", "Changed Password to a new value", 1);
            
            }
            else
            {
                WC_MessageBox.ShowMssage( _message);
                
            }
        }
    }
    
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        TxtConfirmPwd.Text = "";
        TxtNewPwd.Text = "";
        TxtOldPwd.Text = "";
        LblFailureNotice.Text = "";
    }
}
