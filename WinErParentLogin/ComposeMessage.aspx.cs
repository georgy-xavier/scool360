using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;

namespace WinErParentLogin
{
    public partial class ComposeMessage : System.Web.UI.Page
    {
        private ParentInfoClass MyParentInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MyParentObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyParentInfo = (ParentInfoClass)Session["MyParentObj"];
            if (MyParentInfo == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (!IsPostBack)
            {
                Label MyHeader = (Label)this.Master.FindControl("Lbl_PageHeader");
                MyHeader.Text = "Compose";
                
            }
        }

  
        private void ReleaseResourse(MysqlClass _mysqlObj, ParentLogin MyParent)
        {
            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyParent = null;
        }

        protected void Btn_Send_Click(object sender, EventArgs e)
        {
            int toDept = 0, toUsrId = 0, frmUserId = 0,  threadId;
            string subj = "", desc = "";
          //  frmUserId = MyParentInfo.ParentId;
            frmUserId = MyParentInfo.StudentId;
            toUsrId = 1;
            if (toUsrId > 0)
            {
                subj = txt_subject.Text;
                desc = txt_descrpn.Text;


                MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
                ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
                threadId = MyParent.GetThreadId(frmUserId, toUsrId, 1, 2,  subj);
                MyParent.ComposeMessage(frmUserId, toUsrId, 1, 2, subj, desc, 0, 3,  threadId);
                
                MSGBOX.ShowMssage("Message has been sent");

                DBLogClass _DblogObj = new DBLogClass(_mysqlObj);

                _DblogObj.LogToDb(MyParentInfo.ParentName, "ParentLogin_MessageComposing", "New message composed ",2, 2);
                _mysqlObj.CloseConnection();
                _mysqlObj = null;

                txt_subject.Text = "";
                txt_descrpn.Text = "";

            }
            else
            {
                MSGBOX.ShowMssage("Select staff");
            }
        }
    }
}
