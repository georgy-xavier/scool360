using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Configuration;
using System.Data.Odbc;
using System.Collections;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WebChart;
using System.Drawing;
using WinEr;
using System.IO;
using System.Text;

namespace Scool360student
{
    public partial class WinErStudentMaster : System.Web.UI.MasterPage
    {
        private KnowinUser MyUser;
        private ConfigManager MyConfig;
        private SchoolClass objSchool = null;
        private ParentInfoClass MyParentInfo;
       
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            {

                MyUser = (KnowinUser)Session["UserObj"];
                MyConfig = MyUser.GetConfigObj();


                if (WinerUtlity.NeedCentrelDB())
                {
                    if (Session[WinerConstants.SessionSchool] == null)
                    {
                        Response.Redirect("Logout.aspx");
                    }
                    objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                }
               
                    if (MyUser.IsLogedIn )
                    {
                        if (!IsPostBack)
                        {
                            MyUser.SELECTEDMODE = 1;
                            string userId = MyUser.User_Id.ToString();
                            string _InnerHtml = " <script type=\"text/javascript\">sessionStorage.setItem(\"$SMD$\",\"1\");document.cookie = \"$CUR$USR$ID$=" + userId.ToString() + "\";</script>";
                            this.javascriptId1.InnerHtml = _InnerHtml;
                           
                        }
                    }
                    else
                    {
                        //no rights for this user.
                        Response.Redirect("LoginErr.htm");
                    }
               
            }
        }
        # region Application Object Area Fee
      
        private int GetUserIndex(int _UserId, List<UserInfoClass> list)
        {
            int _ReturnIndex = -1;
            for (int _i = 0; _i < list.Count; _i++) // Loop through List with foreach
            {

                if (list[_i].UserId == _UserId)
                {
                    return _i;
                }
            }
            return _ReturnIndex;
        }
        # endregion

    

    }
}
