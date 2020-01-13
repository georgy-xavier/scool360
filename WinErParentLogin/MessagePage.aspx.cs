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
using WinBase;

namespace WinErParentLogin
{
    public partial class MessagePage : System.Web.UI.Page
    {
        private ParentInfoClass MyParentInfo;
        private Attendance MyAttendence;
        protected void Page_Load(object sender, EventArgs e)
        {
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
            if (!IsPostBack)
            {
                Label MyHeader = (Label)this.Master.FindControl("Lbl_PageHeader");
                MyHeader.Text = "Message";
                loadMessages();
            }
        }

        private void loadMessages()
        {
            string sql = "SELECT Id,UserTypeId,UserTypeRemark,UserId,UserName,Subject,Message,CreatedDate,Visited where";
        }

        protected void Imgbtn_ShowPanel_Click(object sender, ImageClickEventArgs e)
        {
            PanelSend.Visible = !PanelSend.Visible;
            ClearAll();
        }

        private void ClearAll()
        {
            Txt_Message.Text = "";
            Txt_Subject.Text = "";
            LblFailureNotice.Text = "";
        }

        protected void Lnk_ShowPAnel_Click(object sender, EventArgs e)
        {
            PanelSend.Visible = !PanelSend.Visible;
            ClearAll();
        }

        protected void Btn_SendMessage_Click(object sender, EventArgs e)
        {
            string _msg = "";
            LblFailureNotice.Text = "";
            if (IsSedingPossible(out _msg))
            {
                try
                {
                    string UserTypeRemark = "Me";
                    string sql = "INSERT INTO tblparentmessages (UserTypeId,UserTypeRemark,UserId,UserName,Subject,Message,CreatedDate) VALUES (0,'" + UserTypeRemark + "'," + MyParentInfo.ParentId + ",'" + MyParentInfo.ParentName + "','" + Txt_Subject.Text.Trim() + "','" + Txt_Message.Text.Trim() + "','" + DateTime.Now.ToString("s") + "');";
                    MyAttendence.m_MysqlDb.ExecuteQuery(sql);
                    LblFailureNotice.Text = "Successfully Send";
                }
                catch(Exception ex)
                {
                    LblFailureNotice.Text ="Error while sending. Error Message : "+ ex.Message;
                }
            }
            else
            {
                LblFailureNotice.Text = _msg;
            }


        }

        private bool IsSedingPossible(out string _msg)
        {
            _msg = "";
            bool _valid = true;

            if (Txt_Subject.Text.Trim() == "")
            {
                _msg = "Please enter subject";
                _valid = false;
            }
            if (_valid)
            {
                if (Txt_Message.Text.Trim() == "")
                {
                    _msg = "Please enter message";
                    _valid = false;
                }
            }

            return _valid;
        }

        protected void Btn_Clear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

    }
}
