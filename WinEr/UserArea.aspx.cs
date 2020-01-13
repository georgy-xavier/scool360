using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Text;
using WinBase;
namespace WinEr
{
    public partial class WebForm23 : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        //private OdbcDataReader MyReader = null;
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }

            MyUser = (KnowinUser)Session["UserObj"];

               if (!IsPostBack)
                {
                    Img_Holder.ImageUrl = "Handler/ImageReturnHandler.ashx?id=" + MyUser.UserId + "&type=StaffImage";

                        
                    // MyUser.GetImageUrl("StaffImage", MyUser.UserId);
                    LoadUserData();
                    //some initlization
          
                 }
        }

        private void LoadUserData()
        {
            StringBuilder _UserData = new StringBuilder("");
            _UserData.Append(MyUser.FillStaffTopData(MyUser.UserId, "User"));
            this.PersonData.InnerHtml = _UserData.ToString();
        }
    }
}
