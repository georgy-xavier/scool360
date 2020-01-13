using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;
using RavSoft.GoogleTranslator;

namespace WinEr
{
    public partial class SendGroupSMS : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        private GroupManager MyGroup;
        private SMSManager MysmsMang;

        #region Events
        
            protected void Page_Load(object sender, EventArgs e)
            {
                
                if (Session["UserObj"] == null)
                {
                    Response.Redirect("sectionerr.htm");
                }
                MyUser = (KnowinUser)Session["UserObj"];
                MyGroup = MyUser.GetGroupManagerObj();
                MysmsMang = MyUser.GetSMSMngObj();
                if (MyGroup == null)
                {
                    Response.Redirect("RoleErr.htm");
                    //no rights for this user.
                }
                else if (MysmsMang == null)
                {
                    Response.Redirect("RoleErr.htm");
                }
                else if (!MyUser.HaveActionRignt(925))
                {
                    Response.Redirect("RoleErr.htm");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        MysmsMang.InitClass(); 
                        LoadGroupToDropDown();
                        Pnl_Display.Visible = false;
                        //Lnk_Retry.Visible = false;
                    }
                    Lnk_Retry.Visible = false;
                    lbl_error.Text = "";
                }
            }


            protected void Btn_Birthday_CheckConnection_Click(object sender, EventArgs e)
            {
                string msg = "";
                if (MysmsMang.CheckConnection(out msg))
                {
                    MPE_Message.Show();
                    DivMainMessage.InnerHtml = msg.Replace("\r", "<br/>");

                }
                else
                {
                    MPE_Message.Show();
                    DivMainMessage.InnerHtml = msg;
                }               
            }

            protected void Grd_UserList_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                Grd_UserList.PageIndex = e.NewPageIndex;
                LoadAllUsers();
            }

            protected void Btn_Show_Click(object sender, EventArgs e)
            {
                if (LoadAllUsers())
                {

                    Load_DrpTamplate();
                }
              
            }

            protected void Btn_SendSMS_Click(object sender, EventArgs e)
            {
                int chkcount = 0,typeid = 0;
                char symbol = MysmsMang.GetSMS_NumberSeperator_FromDatabase();
                string msg = "", userid = "";
                string failedlist = "";
                int native_need = 0;
                bool send = true;
                
                  //check already send or not
                    List<string> li_user = new List<string>();

                CheckBox chk = new CheckBox();
                try
                {
                    DataSet FailedList = GetFormatedDataset();
                    DataRow dr;
                    DataTable dt = FailedList.Tables["List"];
                    foreach (GridViewRow gr in Grd_UserList.Rows)
                    {
                        int check = 0;
                        chk = (CheckBox)gr.FindControl("chk_select");
                        if (chk.Checked)
                        {
                            chkcount++;
                            if (Data_Complete(out msg))
                            {                               
                                int.TryParse(gr.Cells[4].Text, out typeid);
                                userid = gr.Cells[5].Text;
                                //sai added for avoid repeated users
                                if (!li_user.Contains(userid))
                                {
                                    li_user.Add(userid);
                                    check = 1;
                                    if (typeid == 1)
                                    {
                                        if (!SendStaffSMS(Txt_Message.Text, userid, out failedlist, out native_need))
                                        {
                                            send = false;
                                        }
                                    }
                                    if (typeid == 0)
                                    {
                                        if (!SendSMSStudent(Txt_Message.Text, userid, out failedlist, out native_need))
                                        {
                                            send = false;
                                        }

                                    }

                                }
                                if (check == 1)
                                {
                                    if (!String.IsNullOrEmpty(failedlist))
                                    {
                                        string[] list = failedlist.Split(symbol);
                                        if (list.Length > 0)
                                        {
                                            for (int i = 0; i < list.Length; i++)
                                            {
                                                dr = dt.NewRow();
                                                dr["Phone"] = list[i];
                                                dr["NativeLanguage"] = native_need;
                                                FailedList.Tables["List"].Rows.Add(dr);
                                            }
                                        }
                                    }
                                }
                            }

                            else
                            {
                                lbl_error.Text = msg;
                                send = false;
                            }

                        }

                    }

                    if (chkcount == 0)
                    {
                        lbl_error.Text = "Select any user..!";
                        send = false;
                       
                    }
                    //check failedlist for retry option
                    if (FailedList != null && FailedList.Tables[0] != null && FailedList.Tables[0].Rows.Count > 0)
                    {
                        Session["sms_msg"] = Txt_Message.Text.ToString().Trim();
                        Session["sms_Type"] = 2;
                        Session["sms_sent_type"] = "Group SMS";
                        Session["Faillist"] = FailedList;

                        //link button
                        lbl_error.Text = "Sending SMS failed,Do you want to retry";
                        Lnk_Retry.Visible = true;
                        send = false;
                    }

                }
                catch (Exception)
                {
                    lbl_error.Text = "Cannot send sms..Please try later..!";                  

                }
                if (send)
                {
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Group", "SMS sent to Group numbers", 1);
                    WC_MessageBox.ShowMssage("SMS has been sent successfully..!");
                }
            }

            protected void Drp_Template_SelectedIndexChanged(object sender, EventArgs e)
            {
                Load_SelectedTemplate();
            }

          

        #endregion

        #region Methods

            private void LoadGroupToDropDown()
                {
                    DataSet GroupDs = new DataSet();
                    ListItem li;
                    Drp_Group.Items.Clear();
                    GroupDs = MyGroup.GetAllGroup();
                    if (GroupDs != null && GroupDs.Tables[0].Rows.Count > 0)
                    {
                        li = new ListItem("All", "0");
                        Drp_Group.Items.Add(li);
                        foreach (DataRow dr in GroupDs.Tables[0].Rows)
                        {
                            li = new ListItem(dr["GroupName"].ToString(),dr["Id"].ToString());
                            Drp_Group.Items.Add(li);
                        }
                        
                    }
                    else
                    {
                        li = new ListItem("None", "-1");
                        Drp_Group.Items.Add(li);
                    }
                }

            private bool LoadAllUsers()
            {
                DataSet UsersDs = new DataSet();
                lbl_error.Text = "";
                int groupid = 0, type = 0;
                try
                {
                    int.TryParse(Drp_Group.SelectedValue, out groupid);
                    int.TryParse(Drp_Type.SelectedValue, out type);
                    UsersDs = MyGroup.GetAllUsers(groupid, type);

                    if (UsersDs != null && UsersDs.Tables[0].Rows.Count > 0)
                    {
                        Pnl_Display.Visible = true;
                        Grd_UserList.Columns[4].Visible = true;
                        Grd_UserList.Columns[5].Visible = true;
                        Grd_UserList.DataSource = UsersDs;
                        Grd_UserList.DataBind();
                        Grd_UserList.Columns[4].Visible = false;
                        Grd_UserList.Columns[5].Visible = false;

                    }
                    else
                    {
                        Grd_UserList.DataSource = null;
                        Grd_UserList.DataBind();
                        lbl_error.Text = "No users found..";
                        Pnl_Display.Visible = false;
                        return false;
                    }
                }
                catch (Exception)
                {

                    lbl_error.Text = "Please try later";
                    return false;
                }
                return true;
            }


            private void Load_DrpTamplate()
            {
                Drp_Template.Items.Clear();
                ListItem li;
                lbl_error.Text = "";
                DataSet TemplateDs = new DataSet();
                try
                {
                    string sql = "SELECT Id,Name FROM tblsmstemplate WHERE Active=1";
                    TemplateDs = MyGroup.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (TemplateDs != null && TemplateDs.Tables != null && TemplateDs.Tables[0].Rows.Count > 0)
                    {
                        li = new ListItem("Select", "-1");
                        Drp_Template.Items.Add(li);
                        foreach (DataRow dr in TemplateDs.Tables[0].Rows)
                        {

                            li = new ListItem(dr[1].ToString(), dr[0].ToString());
                            Drp_Template.Items.Add(li);
                        }
                    }
                    else
                    {
                        li = new ListItem("No Template Found", "-1");
                        Drp_Template.Items.Add(li);
                    }
                    Drp_Template.SelectedIndex = 0;
                }
                catch (Exception)
                {
                    lbl_error.Text = "Cannot load templates,Please try later..!";
                }
            }

            private bool SendSMSStudent(string Message, string StudentId, out string failedList, out int Is_Native)
            {
                bool _done = false;
                bool Nativesms_Need = false;
                string PhoneNo = MysmsMang.Get_PhoneNo_List(StudentId, out Nativesms_Need);
                failedList = "";
                Is_Native = 0;
                string sms_text = "";
                if (txtNativelanguage.Text == "")
                {
                    sms_text = Message;
                }
                else
                {
                    sms_text = txtNativelanguage.Text;
                }

                if (PhoneNo != "")
                {
                    if (Nativesms_Need)
                    {
                        if (MysmsMang.SendBULKSms(PhoneNo, sms_text, "", "999", true, out  failedList))
                        {
                            MysmsMang.UpdateSMSLogtotable(PhoneNo, ',', sms_text, 2);
                            _done = true;
                        }
                        Is_Native = 1;
                    }
                    else
                    {
                        if (MysmsMang.SendBULKSms(PhoneNo, Message, "", "999", true, out  failedList))
                        {
                            MysmsMang.UpdateSMSLogtotable(PhoneNo, ',', Message, 2);
                            _done = true;
                        }
                        Is_Native = 0;
                    }
                }
                return _done;
            }

            private bool SendStaffSMS(string Message, string StaffId,out string failedlist,out int Is_Nativesms)
            {
                bool _done = false;
                failedlist = "";
                Is_Nativesms = 0;
                string PhoneNo = MysmsMang.Get_StaffPhoneNo_List(StaffId);
                if (PhoneNo != "")
                {

                    //dominic
                    string failedList = "";



                    if (MysmsMang.SendBULKSms(PhoneNo, Message, "", "999", true, out  failedList))
                    {
                        MysmsMang.UpdateSMSLogtotable(PhoneNo, ',', Txt_Message.Text, 0);
                        _done = true;
                    }

                }
                return _done;
            }

            private bool Data_Complete(out string msg)
            {
                bool valid = true;
                msg = "";

                if (Txt_Message.Text.Trim() == "")
                {
                    msg = "Enter SMS Message";
                    valid = false;
                }
                else if (Txt_Message.Text.Trim().Contains("($") || Txt_Message.Text.Trim().Contains("$)"))
                {
                    msg = "Before sending, replace symbol ($ $) parts with correct data in template";
                    valid = false;
                }
                else if (Txt_Message.Text.Trim().Contains("&"))
                {
                    msg = "SMS message should avoid symbol '&'. Message with symbol '&' will not be sent";
                    valid = false;
                }
                else if (txtNativelanguage.Text.Trim().Contains("($") || txtNativelanguage.Text.Trim().Contains("$)"))
                {
                    msg = "Before sending, replace symbol ($ $) parts with correct data in template";
                    valid = false;
                }
                else if (txtNativelanguage.Text.Trim().Contains("&"))
                {
                    msg = "SMS message should avoid symbol '&'. Message with symbol '&' will not be sent";
                    valid = false;
                }
                return valid;
            }

            private void Load_SelectedTemplate()
            {
                Txt_Message.Text = "";
                if (Drp_Template.SelectedValue != "-1")
                {
                    Txt_Message.Text = MysmsMang.GetSMSTemplate(Drp_Template.SelectedValue);
                }
            }



        #endregion
        #region nativesms
            protected void btnConvert_Click(object sender, EventArgs e)
            {
                string _NativesmsCOnvertError = "";
                string NativeLanguage = "";
                if (ValidateNativeLanguage_Convert(out NativeLanguage, out _NativesmsCOnvertError))
                {
                    string nativesms = GetNativeLanguageSMS(Txt_Message.Text, NativeLanguage);
                    txtNativelanguage.Text = nativesms;
                }
                else
                {
                    lbl_error.Text = _NativesmsCOnvertError;
                }
            }
            private bool ValidateNativeLanguage_Convert(out string NativeLanguage, out string _NativesmsCOnvertError)
            {
                _NativesmsCOnvertError = "";
                NativeLanguage = MysmsMang.GetNativeLanguage();
                bool _validate = true;
                if (NativeLanguage.Trim() == "")
                {
                    _NativesmsCOnvertError = "Native Language Not Configured";
                    return false;
                }

                if (Txt_Message.Text.Trim() == "")
                {
                    _NativesmsCOnvertError = "Please enter message for converting to native language";
                    return false;
                }

                return _validate;
            }
            private string GetNativeLanguageSMS(string Message, string NativeLanguage)
            {

                string NativeSMS = "";
                if (NativeLanguage.CompareTo("English") != 0)
                {
                    Translator t = new Translator();

                    t.SourceLanguage = "English";// (string)this._comboFrom.SelectedItem;
                    t.TargetLanguage = NativeLanguage;//(string)this._comboTo.SelectedItem;
                    t.SourceText = Txt_Message.Text;


                    try
                    {
                        // Forward translation             
                        t.Translate();
                        NativeSMS = t.Translation;

                    }
                    catch
                    {
                        NativeSMS = Message;
                    }

                }
                return NativeSMS;
            }
        #endregion
        #region Failure_sms_dataset

            private DataSet GetFormatedDataset()
            {
                DataSet PhoneList = new DataSet();
                DataTable dt;

                PhoneList.Tables.Add(new DataTable("List"));
                dt = PhoneList.Tables["List"];
                dt.Columns.Add("Phone");
                dt.Columns.Add("NativeLanguage");

                return PhoneList;
            }
            protected void Lnk_Retry_Click(object sender, EventArgs e)
            {
                lbl_error.Text = "";
                Lnk_Retry.Visible = false;

                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "var Mleft = (screen.width/2)-(760/2);var Mtop = (screen.height/2)-(700/2);window.open( 'RetryFailureSMS.aspx', null, 'height=510,width=600,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=centre,top=\'+Mtop+\', left=\'+Mleft+\'' );", true);
            }
        #endregion
    }
}
