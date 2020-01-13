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
using System.Web.UI.HtmlControls;

namespace WinEr
{
    public partial class MessageThread : System.Web.UI.Page
    {
        private OdbcDataReader MyReader = null;
        private ParentInfoClass MyParentInfo;
        private Attendance MyAttendence;
        private DataSet MydataSet, EventDataset;

        protected void Page_Load(object sender, EventArgs e)
        {
            int thrdId = 0;
            if (Session["MyParentObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyParentInfo = (ParentInfoClass)Session["MyParentObj"];
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            MyAttendence = new Attendance(_mysqlObj);
            if (MyParentInfo == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            else if (MyAttendence == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            else if (!IsPostBack)
            {
                if (Request.QueryString["ThreadId"] != null)
                {
                    int.TryParse(Request.QueryString["ThreadId"].ToString(), out thrdId);
                    LoadMessageThread(thrdId);
                }
            }
        }

        private void LoadMessageThread(int thrdId)
        {
            int FrmId = 0, FrmType = 0;
              MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            MydataSet = MyParent.GetMessageThrds(thrdId, 1);
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

            foreach (GridViewRow gv in grdThreads.Rows)
            {
                int.TryParse(gv.Cells[1].Text.ToString(), out FrmId);
                int.TryParse(gv.Cells[2].Text.ToString(), out FrmType);

                if (FrmType == 2)
                {
                    gv.BackColor = Color.WhiteSmoke;
                }
            }
            //grdThreads.DataSource = MydataSet;
            //grdThreads.DataBind();

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
            int thrdId = 0;
            int.TryParse(Request.QueryString["ThreadId"].ToString(), out thrdId);
            subj = txt_subj.Text;
            descrption = txt_message.Text;
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            MyParent.SendThrdMessage( MyParentInfo.StudentId,1, thrdId,subj,descrption);
            _mysqlObj.CloseConnection();
            txt_message.Text="";
            txt_subj.Text = "";
            
          //  MSGBOX.ShowMssage("Message sent");
            LoadMessageThread(thrdId);
      
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "pagereload('Message has been sent');", true);
        }
    }
}
