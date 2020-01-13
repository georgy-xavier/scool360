using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

namespace WinEr
{
    public partial class SMSConfiguration : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private SMSManager MysmsMang;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MysmsMang = MyUser.GetSMSMngObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MysmsMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(98))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    
                    //some initlization

                    Load_Drp_Options();
                    Load_Seperators();
                    Img_EditMessage.Visible = false;
                    if (MyUser.HaveActionRignt(787))
                    {
                        Img_EditMessage.Visible = true;
                    }
                    DateTime TimeNow = DateTime.Now;
                    int HourNoW = TimeNow.TimeOfDay.Hours;
                }
            }


        }

        private void Load_Seperators()
        {
            string innerhtml = "<table cellspacing=\"10\">";
            string sql = "SELECT tblsmsseperators.`Type`,tblsmsseperators.Seperator FROM tblsmsseperators INNER JOIN tblsmsseperatormap ON tblsmsseperatormap.SeperatorId=tblsmsseperators.Id WHERE tblsmsseperatormap.TypeId=" + Drp_SMS_Options.SelectedValue;
            MyReader = MysmsMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {

                while (MyReader.Read())
                {
                    innerhtml = innerhtml + "<tr style=\"height:20px\"><td align=\"right\">" + MyReader.GetValue(0).ToString() + " : </td> <td  align=\"left\" class=\"new\"> " + MyReader.GetValue(1).ToString() + " </td></tr> ";
                }
            }
            innerhtml = innerhtml + "</table>";
            this.Seperators.InnerHtml = innerhtml;
        }

        private void Load_Drp_Options()
        {
            Drp_SMS_Options.Items.Clear();
            string sql = "SELECT Id,`Type` FROM tblsmsoptionconfig WHERE ShortName<>'' AND SetVisible=1";
            MyReader = MysmsMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_SMS_Options.Items.Add(new ListItem("Select SMS Configurations", "-1"));
                while (MyReader.Read())
                {

                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_SMS_Options.Items.Add(li);
                }
            }
            else
            {
                Drp_SMS_Options.Items.Add(new ListItem("No SMS Configuration Presents", "-1"));
            }
        }

        protected void Drp_SMS_Options_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_msg.Text = "";
            if (Drp_SMS_Options.SelectedValue != "-1")
            {
                Panel_SMS.Visible = true;
                Load_PanelDetails(int.Parse(Drp_SMS_Options.SelectedValue));
                Load_Seperators();
            }
            else
            {
                Panel_SMS.Visible = false;
            }
        }

 

        private void Load_PanelDetails(int OptionId)
        {
            int SMSEnabled = 0, ScheduledEnabled = 0, ScheduedTime = 0;
            TimeSpan ts;
            string sql = "SELECT Type,Enable,Format,ScheduledTime,ScheduledTimeEnabled FROM tblsmsoptionconfig WHERE Id="+OptionId;
            MyReader = MysmsMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(1).ToString(), out SMSEnabled);
                int.TryParse(MyReader.GetValue(4).ToString(), out ScheduledEnabled);
                TimeSpan.TryParse(MyReader.GetValue(3).ToString(), out ts);
                Txt_Message.Text = MyReader.GetValue(2).ToString();
                Txt_Message.Enabled = false;
                Img_EditMessage.ImageUrl = "Pics/lock.png";
                Drp_Schedulehour.SelectedValue = ts.Hours.ToString();
                Drp_ScheduleMinute.SelectedValue = ts.Minutes.ToString();
                if (SMSEnabled == 1)
                {
                    Chk_EnableSMS.Checked = true;
                }
                else
                {
                    Chk_EnableSMS.Checked = false;
                }

                if (ScheduledEnabled == 1)
                {
                    Chk_ScheduleTime.Checked = true;
                    Drp_Schedulehour.Enabled = true;
                    Drp_ScheduleMinute.Enabled = true;
                }
                else
                {
                    Chk_ScheduleTime.Checked = false;
                    Drp_Schedulehour.Enabled = false;
                    Drp_ScheduleMinute.Enabled = false;
                }

                
            }
        }

    

        protected void Img_EditMessage_Click(object sender, ImageClickEventArgs e)
        {
            lbl_msg.Text = "";
            if (Txt_Message.Enabled == false)
            {
                Txt_Message.Enabled = true;
                Img_EditMessage.ImageUrl = "Pics/unlock.png";
            }
            else
            {
                lbl_msg.Text = "Update the changes";
              
            }
        }

        protected void Chk_ScheduleTime_CheckedChanged(object sender, EventArgs e)
        {
            Drp_Schedulehour.SelectedValue = "0";
            Drp_ScheduleMinute.SelectedValue = "0";
        
            if (!Chk_ScheduleTime.Checked)
            {
                Drp_Schedulehour.Enabled = false;
                Drp_ScheduleMinute.Enabled = false;
              

            }
            else
            {
                Drp_Schedulehour.Enabled = true;
                Drp_ScheduleMinute.Enabled = true;
            }
        }

        protected void Btn_Update_Click(object sender, EventArgs e)
        {
            string msg = "";
            lbl_msg.Text = "";
            string _time = "";
            if (DataComplete(out msg))
            {
                try
                {
                    _time = Drp_Schedulehour.SelectedItem.Text + ":" + Drp_ScheduleMinute.SelectedItem.Text + ":00";
                    UpdateConfigDatas(int.Parse(Drp_SMS_Options.SelectedValue), Txt_Message.Text, Chk_EnableSMS.Checked, Chk_ScheduleTime.Checked, _time);
                    lbl_msg.Text = "Successfully Updated";
                    Load_PanelDetails(int.Parse(Drp_SMS_Options.SelectedValue));
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "SMS-Configuration Updated",Drp_SMS_Options.SelectedItem.Text + " is changed", 1);

                }
                catch
                {
                    lbl_msg.Text = "Error in updation";
                }
             }
            else
            {
                lbl_msg.Text = msg;
            }
        }

        private void UpdateConfigDatas(int Id, string Message, bool SmsEnabled, bool SchedulTime, string TimeID)
        {
            int sms_Enabled = 0,ScheduleTime=0;
            if (SmsEnabled)
            {
                sms_Enabled = 1;
            }
            if (SchedulTime)
            {
                ScheduleTime = 1;

            }
            else
            {
                TimeID = "0";
            }
            string sql = "update tblsmsoptionconfig set Enable=" + sms_Enabled + ",Format='" + Message + "',ScheduledTime='" + TimeID + "',ScheduledTimeEnabled=" + ScheduleTime + " where Id=" + Id;
            MysmsMang.m_MysqlDb.ExecuteQuery(sql);
        }

        private bool DataComplete(out string msg)
        {
            msg = "";
            bool valid = true;

            if (Txt_Message.Text.Trim() == "")
            {
                msg = "Message Cannot Be Empty";
                valid = false;
            }
            if (Chk_ScheduleTime.Checked)
            {
                if (Drp_Schedulehour.SelectedValue == "0")
                {
                    msg = "Select hour for scheduled SMS";
                    valid = false;
                }
                if (Drp_ScheduleMinute.SelectedValue == "0")
                {
                    msg = "Select minute for scheduled SMS";
                    valid = false;
                }
            }
            return valid;
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Load_PanelDetails(int.Parse(Drp_SMS_Options.SelectedValue));
            lbl_msg.Text = "";
        }
    }
}
