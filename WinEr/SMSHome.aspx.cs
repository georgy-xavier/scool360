using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using RavSoft.GoogleTranslator;

namespace WinEr
{
    public partial class SMSHome : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private SMSManager MysmsMang;
        //private OdbcDataReader MyReader = null;
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
            else if (!MyUser.HaveActionRignt(404))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    
                    //some initlization
                    MysmsMang.InitClass(); 
                    //Load_DrpClass();
                    //sai added
                    Load_Class();
                    //
                    Load_DrpTamplate();

                    LoadNativeLanguageArea();
                    if (Session["ClassId"] != null)
                    {
                        int ClassId;
                        
                        if (int.TryParse(Session["ClassId"].ToString(), out ClassId))
                        {
                            RdBtLstSelectCtgry1.SelectedIndex = 1;
                            Panel_class.Visible = true;
                            Chkb_class.SelectedValue = ClassId.ToString();

                        }

                    }
                    Lnk_Retry.Visible = false;

                    DateTime dt = DateTime.Now;
                    TimeSpan ts = dt.TimeOfDay;
                }
            }


        }

        private void LoadNativeLanguageArea()
        {
           string _NativeLang= MysmsMang.GetNativeLanguage();
           if (MysmsMang.HaveNetConnection() && _NativeLang.Trim() != "" &&  (string.Compare(_NativeLang.Trim(),"English")!=0))
            {
                ntvelng.Visible = true;
                btnConvert.Visible = true;
            }
            else
            {
                ntvelng.Visible = false;
                btnConvert.Visible = false;
            }
        }


        private void Load_DrpTamplate()
        {
            Drp_Template.Items.Clear();
            ListItem li;

            string sql = "SELECT Id,Name FROM tblsmstemplate WHERE SelectType="+Rdb_CheckType.SelectedValue;
            MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("Select", "-1");
                Drp_Template.Items.Add(li);
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
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


        //private void Load_DrpClass()
        //{
        //    Drp_Class.Items.Clear();
        //    ListItem li = new ListItem("All Class", "-1");
        //    Drp_Class.Items.Add(li);

        //    MydataSet = MyUser.MyAssociatedClass();

        //    if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
        //    {

        //        foreach (DataRow dr in MydataSet.Tables[0].Rows)
        //        {

        //             li = new ListItem(dr[1].ToString(), dr[0].ToString());
        //            Drp_Class.Items.Add(li);

        //        }

        //    }
        //    else
        //    {
        //         li = new ListItem("No Class present", "-2");
        //        Drp_Class.Items.Add(li);
        //    }
        //    Drp_Class.SelectedIndex = 0;
        //}
        //sai added for load classes to checkbox list
        private void Load_Class()
        {
            Chkb_class.Items.Clear();
            ListItem li = new ListItem(); 

            MydataSet = MyUser.MyAssociatedClass();

            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Chkb_class.Items.Add(li);
                    
                }
                
            }
            else
            {
                li = new ListItem("No Class present", "-1");
                Chkb_class.Items.Add(li);
            }
            Chkb_class.SelectedIndex = 0;
            
        }
        protected void RdBtLstSelectCtgry1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_DrpTamplate();
            Load_SelectedTemplate();
            lbl_error.Text = "";
            if (RdBtLstSelectCtgry1.SelectedItem.Text != "All")
            {
                Panel_class.Visible = true;
                for (int i = 0; i < Chkb_class.Items.Count; i++)
                {
                    Chkb_class.Items[i].Selected = false;
                }
            }
            else
            {
                Panel_class.Visible = false;
                for (int i = 0; i < Chkb_class.Items.Count; i++)
                {
                    Chkb_class.Items[i].Selected = true;
                }
            }
        }
        //

        protected void Btn_Send_Click(object sender, EventArgs e)
        {
            string phonelist = "";
            string msg = "";
            char symbol=MysmsMang.GetSMS_NumberSeperator_FromDatabase();
            lbl_error.Text = "";
            int smstype=0;
            string nativesms = "";
            string NativePhoneList = "";
            bool send = false;
            bool chk_nos = true;

            string failedList="";
            try
            {

                if (Data_Complete(out msg))
                {
                    DataSet FailedList = GetFormatedDataset();
                    DataRow dr;
                    DataTable dt = FailedList.Tables["List"]; ;
                    if (Rdb_CheckType.SelectedValue == "0")
                    {
                        if (phonelist != "")
                        {
                            phonelist = phonelist + symbol.ToString();
                        }
                        phonelist = phonelist + MysmsMang.Get_StaffPhoneNo_List("");
                        smstype = 0;
                    }
                    else if (Rdb_CheckType.SelectedValue == "2" || Rdb_CheckType.SelectedValue == "1")
                    {
                        string temp_classIds = "";
                        if (RdBtLstSelectCtgry1.SelectedItem.Text != "All")
                        {
                            foreach (ListItem listItem in Chkb_class.Items)
                            {
                                if (listItem.Selected)
                                {
                                    if (temp_classIds == "")
                                    {
                                        temp_classIds = listItem.Value;
                                    }
                                    else
                                    {
                                        temp_classIds = temp_classIds + "," + listItem.Value;
                                    }
                                }
                            }
                        }
                        if (Rdb_CheckType.SelectedValue == "2")
                        {

                            if (phonelist != "")
                            {
                                phonelist = phonelist + symbol.ToString();
                            }
                            phonelist = phonelist + MysmsMang.Get_StudentPhoneNo_List(temp_classIds);
                            smstype = 2;
                        }
                        if (Rdb_CheckType.SelectedValue == "1")
                        {
                            if (phonelist != "")
                            {
                                phonelist = phonelist + symbol.ToString();
                            }
                            if (NativePhoneList != "")
                            {
                                NativePhoneList = NativePhoneList + symbol.ToString();
                            }

                            NativePhoneList = NativePhoneList + MysmsMang.Get_ParentPhoneNo_ListwithNativeLanguage(temp_classIds);
                            phonelist = phonelist + MysmsMang.Get_ParentPhoneNo_List(temp_classIds);
                            smstype = 1;

                            if (!String.IsNullOrEmpty(txtNativelanguage.Text))
                                nativesms = txtNativelanguage.Text.Trim();
                            else
                                nativesms = Txt_Message.Text.Trim();




                        }
                    }

                    //
                    //else if (Rdb_CheckType.SelectedValue == "2")
                    //{
                    //    if (phonelist != "")
                    //    {
                    //        phonelist = phonelist + symbol.ToString();
                    //    }
                    //    phonelist = phonelist + MysmsMang.Get_StudentPhoneNo_List(int.Parse(Drp_Class.SelectedValue));
                    //    smstype = 2;
                    //}
                    //else if (Rdb_CheckType.SelectedValue == "1")
                    //{
                    //    if (phonelist != "")
                    //    {
                    //        phonelist = phonelist + symbol.ToString();
                    //    }
                    //    if (NativePhoneList != "")
                    //    {
                    //        NativePhoneList = NativePhoneList + symbol.ToString();
                    //    }

                    //    NativePhoneList = NativePhoneList + MysmsMang.Get_ParentPhoneNo_ListwithNativeLanguage(int.Parse(Drp_Class.SelectedValue));
                    //    phonelist = phonelist + MysmsMang.Get_ParentPhoneNo_List(int.Parse(Drp_Class.SelectedValue));
                    //    smstype = 1;

                    //    if (!String.IsNullOrEmpty(txtNativelanguage.Text))
                    //        nativesms = txtNativelanguage.Text.Trim();
                    //    else
                    //        nativesms = Txt_Message.Text.Trim();

                    //}
                    if (phonelist == "" && NativePhoneList == "")
                    {
                        chk_nos=false;
                    }
                    if(chk_nos)
                    {
                        if (phonelist != "")
                        {


                            if (MysmsMang.SendBULKSms(phonelist, Txt_Message.Text.Trim(), "90366450445", "WINER", true, out  failedList))
                            {
                                MysmsMang.UpdateSMSLogtotable(phonelist, symbol, Txt_Message.Text, smstype);
                                lbl_error.Text = "SMS Circular has been Sent Successfully";

                                MyUser.m_DbLog.LogToDb(MyUser.UserName, "SMS Circular", "Message : " + Txt_Message.Text, 1);

                                send = true;
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(failedList))
                                {
                                    string[] list = failedList.Split(symbol);

                                    if (list.Length > 0)
                                    {
                                        for (int i = 0; i < list.Length; i++)
                                        {
                                            dr = dt.NewRow();

                                            dr["Phone"] = list[i];
                                            dr["NativeLanguage"] = 0;
                                            FailedList.Tables["List"].Rows.Add(dr);
                                        }
                                    }
                                }

                                //lbl_error.Text = "Error In SMS Circulation";
                            }
                            //string _msg = "";  //Arun Commented Error in Code
                            //if (SentAllMessages(phonelist, Txt_Message.Text, out _msg))
                            //{
                            //    MyUser.m_DbLog.LogToDb(MyUser.UserName, "SMS Circular", "Message : " + Txt_Message.Text, 1);
                            //}
                        }
                        if (NativePhoneList != "")
                        {

                            if (MysmsMang.SendBULKSms(NativePhoneList, nativesms, "90366450445", "WINER", true, out  failedList))
                            {
                                MysmsMang.UpdateSMSLogtotable(NativePhoneList, symbol, Txt_Message.Text, smstype);
                                lbl_error.Text = "SMS Circular has been Sent Successfully";

                                MyUser.m_DbLog.LogToDb(MyUser.UserName, "SMS Circular", "Message : " + Txt_Message.Text+"", 1);

                                send = true;
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(failedList))
                                {
                                    string[] list = failedList.Split(symbol);
                                    if (list.Length > 0)
                                    {
                                        for (int i = 0; i < list.Length; i++)
                                        {
                                            dr = dt.NewRow();

                                            dr["Phone"] = list[i];
                                            dr["NativeLanguage"] = 1;

                                            FailedList.Tables["List"].Rows.Add(dr);
                                        }
                                    }
                                }

                                //lbl_error.Text = "Error In SMS Circulation";
                            }
                        }
                    }
                    else
                    {
                        lbl_error.Text = "Phone numbers not exist";
                    }

                    if (FailedList != null && FailedList.Tables != null && FailedList.Tables[0].Rows.Count > 0)
                    {
                        Session["sms_msg"] = Txt_Message.Text.ToString().Trim();
                        Session["sms_Type"] = smstype;
                        Session["sms_sent_type"] = "SMS Circular";
                        Session["Faillist"] = FailedList;

                        //link button for failure sms retry option
                        lbl_error.Text = "Sending SMS failed,Do you want to retry";
                        Lnk_Retry.Visible = true;

                    }
                    else
                    {
                        clearAll();
                    }
                    if (send)
                    {
                        lbl_error.Text = "SMS Circular has been Sent Successfully";
                    }
                }
                else
                {
                    lbl_error.Text = msg;
                }

            }
            catch
            {
                lbl_error.Text = "Error In SMS Circulation";
            }
           
        }

        private DataSet GetFormatedDataset()
        {
            DataSet PhoneList=new DataSet();
            DataTable dt;
           
            PhoneList.Tables.Add(new DataTable("List"));
            dt = PhoneList.Tables["List"];
            dt.Columns.Add("Phone");
            dt.Columns.Add("NativeLanguage");

            return PhoneList;
        }


        private bool SentAllMessages(string phonelist, string Message, out string _msg)
        {
            bool _valid = true;
            _msg = "";
            string[] UserArray = phonelist.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < UserArray.Length; i++)
            {
                string[] DataArray = UserArray[i].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string newmessage = MysmsMang.GetGeneralReplaceSMSMessage(Message, DataArray[1], DataArray[2]);
                //dominic sms 
                string failedList = "";
                if (!MysmsMang.SendBULKSms(DataArray[0], newmessage, "90366450445", "WINER", true, out  failedList))
                {
                    _valid = false;
                    _msg = "Error while sending";
                    break;
                }

            }
            return _valid;
        }

        private void clearAll()
        {
            Txt_Message.Text = "";
            Drp_Template.SelectedIndex = 0;
            txtNativelanguage.Text = "";
        }

        private bool Data_Complete(out string msg)
        {
            bool valid = true;
            msg = "";
            int check_count = 0;
            
            if (Txt_Message.Text.Trim() == "")
            {
                msg = "Enter SMS Message";
                valid = false;
            }
            else if (Txt_Message.Text.Trim().Contains("($") || Txt_Message.Text.Trim().Contains("$)") || txtNativelanguage.Text.Trim().Contains("($") || txtNativelanguage.Text.Trim().Contains("$)"))
            {
                msg = "Before sending, replace symbol ($ $) parts with correct data in template";
                valid = false;
            }
            else if (Txt_Message.Text.Trim().Contains("&") || txtNativelanguage.Text.Trim().Contains("&"))
            {
                msg = "SMS message should avoid symbol '&'. Message with symbol '&' will not be sent";
                valid = false;
            }
            if (RdBtLstSelectCtgry1.SelectedItem.Text != "All")
            {
                foreach (ListItem listItem in Chkb_class.Items)
                {
                    if (listItem.Selected)
                    {
                        check_count++;
                    }
                }
                if (check_count == 0)
                {
                    msg = "Please select class";
                    valid = false;
                }
            }
            return valid;
        }

        protected void Btn_Clear_Click(object sender, EventArgs e)
        {
            clearAll();
            lbl_error.Text = "";
        }

        protected void Btn_CheckConnection_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (MysmsMang.CheckConnection(out msg))
            {
                MPE_Message.Show();
                DivMainMessage.InnerHtml = msg.Replace("\r","<br/>");
                  
            }
            else
            {
                MPE_Message.Show();
                DivMainMessage.InnerHtml = msg;
            }
        }

        protected void Rdb_CheckType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_DrpTamplate();
            Load_SelectedTemplate();
            lbl_error.Text = "";
            if (Rdb_CheckType.SelectedIndex == 0)
            {
                RdBtLstSelectCtgry1.Visible = false;
                lbl_cls.Visible = false;
                Panel_class.Visible = false;
                RdBtLstSelectCtgry1.SelectedIndex = 0;
            }
            else
            {
                RdBtLstSelectCtgry1.Visible =true;
                lbl_cls.Visible =true;
            }
        }

        private void Load_SelectedTemplate()
        {
            Txt_Message.Text = "";
            if (Drp_Template.SelectedValue != "-1")
            {
                Txt_Message.Text = MysmsMang.GetSMSTemplate(Drp_Template.SelectedValue);
            }
        }

        protected void Drp_Template_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_SelectedTemplate();
        }

        protected void btnConvert_Click(object sender, EventArgs e)
        {
            lbl_error.Text = "";
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
        //sai added failure sms retry option
        protected void Lnk_Retry_Click(object sender, EventArgs e)
        {
            lbl_error.Text = "";
            Lnk_Retry.Visible = false;

            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "var Mleft = (screen.width/2)-(760/2);var Mtop = (screen.height/2)-(700/2);window.open( 'RetryFailureSMS.aspx', null, 'height=510,width=600,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=centre,top=\'+Mtop+\', left=\'+Mleft+\'' );", true);
        }
    }
}
