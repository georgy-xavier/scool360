using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace WinEr
{
    public enum MSGTYPE { Message = 0, Alert = 1, HTML = 2 };
   
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
            Btn_magok.Focus();
        }

        public void ShowMssage(string _Message,MSGTYPE _MsgType)
        {

            Lbl_msg.ForeColor = Color.Black;
            if (_MsgType == MSGTYPE.Message)
            {
                m_Skin = "container skin5";
                Lbl_Head.Text = "Message";
                Lbl_msg.Text = _Message;
               // DivMessageBoxContainer.Attributes.Add("class", m_Skin);
            }
            else if(_MsgType == MSGTYPE.Alert)
            {
                m_Skin = "container skinAlert";
                Lbl_Head.Text = "Alert";
                Lbl_msg.Text = _Message;
                Lbl_msg.ForeColor = Color.Red;
               // DivMessageBoxContainer.Attributes.Add("class", m_Skin);
            }
            else if (_MsgType == MSGTYPE.HTML)
            {

                m_Skin = "container skin5";
                Lbl_Head.Text = "Message";
                HtmlDiv.InnerHtml = _Message;
            }
            
            MPE_MessageBox.Show();
            Btn_magok.Focus();
        }
    }
}