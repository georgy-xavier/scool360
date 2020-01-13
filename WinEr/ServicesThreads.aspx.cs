using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WinBase;
using System.Data.Odbc;
using System.Drawing;

namespace WinEr
{
    public partial class ServicesThreads : System.Web.UI.Page
    {
        private StaffManager MyStaffMang;
        private KnowinUser MyUser;
        private DataSet MydataSet;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            int thrdId = 0;
            int ReqType = 0;
            //if (Session["MyParentObj"] == null)
            //{
            //    Response.Redirect("sectionerr.htm");
            //}
            //MyParentInfo = (ParentInfoClass)Session["MyParentObj"];
            //MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            //if (MyParentInfo == null)
            //{
            //    Response.Redirect("sectionerr.htm");
            //}


            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }

            MyUser = (KnowinUser)Session["UserObj"];
            MyStaffMang = MyUser.GetStaffObj();
            if (WinerUtlity.NeedCentrelDB())
            {
                if (Session[WinerConstants.SessionSchool] == null)
                {
                    Response.Redirect("Logout.aspx");
                }
                objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
            }
            if (MyStaffMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!IsPostBack)
            {
                if (Request.QueryString["ThreadId"] != null && Request.QueryString["RequestType"] != null)
                {
                    int.TryParse(Request.QueryString["ThreadId"].ToString(), out thrdId);
                    int.TryParse(Request.QueryString["RequestType"].ToString(), out ReqType);
                    LoadMessageThread(thrdId, ReqType);
                }
                
            }
        }

        private void LoadMessageThread(int thrdId, int ReqType)
        {
            int FrmId = 0, FrmType = 0;

            string _conStr = WinerUtlity.SingleSchoolConnectionString;
            if (WinerUtlity.NeedCentrelDB())
            {
                if (Session[WinerConstants.SessionSchool] == null)
                {
                    Response.Redirect("Logout.aspx");
                }
                objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                _conStr = objSchool.ConnectionString;
            }

            MysqlClass _mysqlObj = new MysqlClass(_conStr);
            ParentLogin MyParent = new ParentLogin(_mysqlObj, objSchool);
            MydataSet = MyParent.GetServiceThrds(thrdId, 1, ReqType);
            _mysqlObj.CloseConnection();

            grdThreads.Columns[0].Visible = true;
            grdThreads.Columns[1].Visible = true;
            grdThreads.Columns[2].Visible = true;


            DataTable dtMessageData = MydataSet.Tables[0];
            DataView dataView = new DataView(dtMessageData);
            dataView.Sort = "Id" + " " + "Desc";

            grdThreads.DataSource = dataView;
            grdThreads.DataBind();

            string Subject = dataView[0][4].ToString();

            txt_subj.Text = "Re:" + Subject;

            //grdThreads.DataSource = MydataSet;
            //grdThreads.DataBind();
            foreach (GridViewRow gv in grdThreads.Rows)
            {
                int.TryParse(gv.Cells[1].Text.ToString(), out FrmId);
                int.TryParse(gv.Cells[2].Text.ToString(), out FrmType);

                if (FrmType == 1)
                {
                    gv.BackColor = Color.WhiteSmoke;
                }
            }
            grdThreads.Columns[0].Visible = false;
            grdThreads.Columns[1].Visible = false;
            grdThreads.Columns[2].Visible = false;
            grdThreads.HeaderRow.Visible = false;
          
        }
        protected void WC_EditStud_Saved(object sender, EventArgs e)
        {

            //ClientScript.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "<script>PageRelorad();</script>"); 

            //ClientScript.RegisterClientScriptBlock(typeof(Page), "AnyScript", "<script>PageRelorad();</script>");
            //ClientScript.RegisterClientScriptBlock(typeof(Page), "AnyScript", "<script>CloseWindow();</script>");
            ////ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaetBookAllot, this.pnlAjaxUpdaetBookAllot.GetType(), "AnyScript", "PageRelorad();", true);
        }
        protected void CancelClick(object sender, EventArgs e)
        { 
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "CloseWindow();", true);           
      
        }

        protected void btn_msg_Click(object sender, EventArgs e)
        {
            string subj = "",descrption="";
            int thrdId = 0, ReqType=0;
            int.TryParse(Request.QueryString["ThreadId"].ToString(), out thrdId);
            int.TryParse(Request.QueryString["RequestType"].ToString(), out ReqType);
            subj = txt_subj.Text;
            descrption = txt_message.Text;

            string _conStr = WinerUtlity.SingleSchoolConnectionString;
            if (WinerUtlity.NeedCentrelDB())
            {
                if (Session[WinerConstants.SessionSchool] == null)
                {
                    Response.Redirect("Logout.aspx");
                }
                objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                _conStr = objSchool.ConnectionString;
            }

            MysqlClass _mysqlObj = new MysqlClass(_conStr);
            ParentLogin MyParent = new ParentLogin(_mysqlObj, objSchool);
            MyParent.SendServicesThrd(MyUser.User_Id, 2, thrdId, subj, descrption);

            _mysqlObj.CloseConnection();
            txt_message.Text="";
            txt_subj.Text = "";
            
          //  MSGBOX.ShowMssage("Message sent");
            LoadMessageThread(thrdId, ReqType); 
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "pagereload('Message has been sent');", true);

            
        }
    }
}
