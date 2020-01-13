using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.Odbc;
namespace WinEr
{
    public partial class WebForm6 : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (Session["FeeId"] == null)
            {
                Response.Redirect("ManageFeeAccount.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyFeeMang = MyUser.GetFeeObj();
            if (MyFeeMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(43))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                LoadSchdulePage();

                //some initlization

            }

        }

        private void LoadSchdulePage()
        {

            string sql = "SELECT tblfeeaccount.AssociatedId from tblfeeaccount where tblfeeaccount.Id=" + int.Parse(Session["FeeId"].ToString());

            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();


                if (MyReader.GetValue(0).ToString() == "1")
                {
                    Response.Redirect("EditClassFeeSchdule.aspx");
                }
                else
                {
                    Response.Redirect("EditStudFeeSchdule.aspx");

                }


            }
            MyReader.Close();
        }
    }
}
