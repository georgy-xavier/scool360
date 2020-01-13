using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
namespace WinEr
{
    public partial class PaySlip : System.Web.UI.Page
    {
        private WinBase.Payroll MyPayroll;
        private KnowinUser MyUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            string  EmplId = Request.QueryString["EmpId"].ToString();
            int MonthId = int.Parse(Request.QueryString["MonthId"].ToString());
            MyUser = (KnowinUser)Session["UserObj"];
            MyPayroll = MyUser.GetPayrollObj();
            LOadSlip();
        }

        private void LOadSlip()
        {
            string  EmplId = Request.QueryString["EmpId"].ToString();
            int MonthId = int.Parse(Request.QueryString["MonthId"].ToString());
            int Year = int.Parse(Request.QueryString["Year"].ToString());

            Div_Payslip.InnerHtml = MyPayroll.GetSalSlip(EmplId, MonthId,Year);
        }
    }
}
