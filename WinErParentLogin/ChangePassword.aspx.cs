using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;
namespace WinErParentLogin
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        private ParentInfoClass MyParentInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MyParentObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyParentInfo = (ParentInfoClass)Session["MyParentObj"];
            if (MyParentInfo == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (!IsPostBack)
            {
                Label MyHeader = (Label)this.Master.FindControl("Lbl_PageHeader");
                MyHeader.Text = "Change Password";
            }
        }




        protected void BtnChngePwd_Click(object sender, EventArgs e)
        {
           
            if (TxtNewPwd.Text.Trim() == "" || TxtOldPwd.Text.Trim() == "")
                MSGBOX.ShowMssage("Please Enter old and new password");
            else if (TxtNewPwd.Text != TxtConfirmPwd.Text)
                MSGBOX.ShowMssage("Passwords do not match");
            else
            {
                string _message;
                MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
                ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);

                if (MyParent.ChangePassWord(TxtOldPwd.Text.ToString(), TxtNewPwd.Text.ToString(), out _message,MyParentInfo.ParentId))
                    MSGBOX.ShowMssage(_message);
                else
                    MSGBOX.ShowMssage(_message);

                _mysqlObj.CloseConnection();
                _mysqlObj = null;
                MyParent = null;

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
}
