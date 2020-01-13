using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WinEr
{
    public enum MSGTYPE { Message = 0, Alert = 1 };
    public partial class MsgBoxControl : System.Web.UI.UserControl
    {
        public string m_Skin = "container skin5";
        //private string m_Heading = "Message";
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void ShowMssage(string _Message)
        {
            m_Skin = "container skin5";
            Lbl_Head.Text = "Message";
            Lbl_msg.Text = _Message;
            MPE_MessageBox.Show();
        }
        public void ShowMssage(string _Message,MSGTYPE _MsgType)
        {
            if (_MsgType == MSGTYPE.Message)
            {
                m_Skin = "container skin5";
                Lbl_Head.Text = "Message";
               // DivMessageBoxContainer.Attributes.Add("class", m_Skin);
            }
            else if(_MsgType == MSGTYPE.Alert)
            {
                m_Skin = "container skinAlert";
                Lbl_Head.Text = "Alert";
               // DivMessageBoxContainer.Attributes.Add("class", m_Skin);
            }
            Lbl_msg.Text = _Message;
            MPE_MessageBox.Show();
        }
    }
}