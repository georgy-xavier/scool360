using System;
public partial class ManageStudent : System.Web.UI.Page
{
    private StudentManagerClass MyStudMang;
    private KnowinUser MyUser;
    
  
    protected void Page_Load(object sender, EventArgs e)
    {
        WC_ManageStudent.EVNTSave += new EventHandler(Wc_ManageStudent_Saved);
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        if (Session["StudId"] == null)
        {
            Response.Redirect("SearchStudent.aspx");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyStudMang = MyUser.GetStudentObj();
        if (MyStudMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }
        else if (!MyUser.HaveActionRignt(4))
        {
            Response.Redirect("RoleErr.htm");
        }
        else
        {
            if (!IsPostBack)
            {
               WC_ManageStudent.STUDENTID = Session["StudId"].ToString();
            }
        }
    }
    protected void Wc_ManageStudent_Saved(object sender, EventArgs e)
    {

    }
  
    
}  

