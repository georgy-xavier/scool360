using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;
using System;
namespace WinEr
{
    public partial class EmployerContribution : System.Web.UI.Page
    {
        private WinBase.Payroll Mypay;
        private KnowinUser MyUser;
        private DataSet MydataSet = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
           
            MyUser = (KnowinUser)Session["UserObj"];
            Mypay = MyUser.GetPayrollObj();
            if (Mypay == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(806))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (!IsPostBack)
                {
                    //some initlization

                }
            }

        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string Config = Txt_Config.Text;
            int Val = int.Parse(Txt_Value.Text.ToString());
            Mypay.AddConfig(Config, Val);
            WC_MessageBox.ShowMssage(" Successfully Saved");
        }
    }
}
