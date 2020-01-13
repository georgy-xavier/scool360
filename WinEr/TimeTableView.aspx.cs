using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using WinBase;

namespace WinEr
{
    public partial class TimeTableView : System.Web.UI.Page
    {
        private ClassOrganiser MyClassMang;
        private TimeTable MyTimeTable;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyTimeTable = MyUser.GetTimeTableObj();
            MyClassMang = MyUser.GetClassObj();
            if (MyTimeTable == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(408))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    bool valid = false;
                    int ClassId=0;
                    if (Request.QueryString["ClassId"] != null)
                    {
                        if (int.TryParse(Request.QueryString["ClassId"].ToString(), out ClassId))
                        {
                            valid = true;

                        }
                    }

                    if (!valid)
                    {
                        if (Session["ClassId"] != null)
                        {

                            if (int.TryParse(Session["ClassId"].ToString(), out ClassId))
                            {
                                valid = true;

                            }

                        }
                    }
                    if (valid)
                    {
                        Lbl_Class.Text = MyClassMang.GetClassname(ClassId);
                        if (Lbl_Class.Text.Trim() == "")
                        {
                            valid = false;
                        }
                    }
                  
                    if (valid)
                    {
                        Lbl_Class.Text = MyClassMang.GetClassname(ClassId);
                        WC_TimeTableControl.ClassId = ClassId.ToString();
                    }
                    else
                    {
                        WC_TimeTableControl.Dispose();
                        Mpe_Finishpopup.Show();
                    }
                   
                   
                    //some initlization

                }


            }
        }
    }
}
