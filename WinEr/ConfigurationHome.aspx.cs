using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;
public partial class ConfigurationHome : System.Web.UI.Page
{
    private ConfigManager MyConfiMang;
    private KnowinUser MyUser;
    //private OdbcDataReader MyReader = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyConfiMang = MyUser.GetConfigObj();
        if (MyConfiMang == null)
        {
            Response.Redirect("Default.aspx");
            //no rights for this user.
        }
        else
        {
            if (!IsPostBack)
            {
            }
        }

    }
   
}
