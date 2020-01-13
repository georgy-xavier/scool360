using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WinEr
{
    public partial class StudentSessionMaker : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["StudentId"] != null)
            {
                Session["StudId"] = Request.QueryString["StudentId"];
                
                    MyUser = (KnowinUser)Session["UserObj"];
                    MyStudMang = MyUser.GetStudentObj();
                    int currentbatch = 0, studid = 0;
                    int.TryParse(Session["StudId"].ToString(), out studid);
                    currentbatch = MyUser.CurrentBatchId;
                    if ((studid != 0) && (currentbatch != 0))
                    {
                      Session["StudType"] = MyStudMang.getStudType(studid, currentbatch);
                    }
                    else
                    { }
                
                if (Request.QueryString["Type"] == null)
                {
                    Response.Redirect("StudentDetails.aspx");
                }
                else
                    if (Request.QueryString["Type"] == "1")
                    {
                        Response.Redirect("ViewIncidenceByType.aspx");
                    }
            }
        }
    }
}
