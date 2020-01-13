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


public partial class CreateRole : System.Web.UI.Page
{
    private KnowinRole MyRole;
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
        }

    }
   
        
    protected void Btn_CreateRole_Click(object sender, EventArgs e)
    {

        if (Txt_RoleName.Text.Trim() == "" || Txt_RoleDisc.Text.Trim() == "")
        {
            //Lbl_FailureNote.Text = "One Or more fields are empty";
        }
        else
        {

            if (MyRole.ValidateRoleName(Txt_RoleName.Text.ToString()))
            {
                MyRole.CreateNewRole(Txt_RoleName.Text.ToString(), Txt_RoleDisc.Text.ToString(),Drp_RoleType.SelectedValue.ToString());
                Lbl_FailureNote.Text = "Role is Created";
                clear();
            }
            else
            {
                Lbl_FailureNote.Text = "Role Name is Already Exist, Please try another one...";
            }

        }
    }
    void clear()
    {
        Txt_RoleDisc.Text = "";
        Txt_RoleName.Text = "";
    }

}


