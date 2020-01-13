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
    public partial class RetryFailureSMS : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private SMSManager MysmsMang;
        private string SMS_Text = "";
        private string Native_Text = "";
        private int Sms_Type = 0;
        private string sms_sent_type = "";
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
                    DataSet DS_Faillist = (DataSet)Session["Faillist"];
                    if (DS_Faillist != null && DS_Faillist.Tables[0] != null && DS_Faillist.Tables[0].Rows.Count > 0)
                    {
                        load_grid_failsmslist(DS_Faillist);
                    }

                }
            }

        }
        private void load_grid_failsmslist(DataSet SMS_Faillist)
        {
            Grd_FailureSMS.DataSource = SMS_Faillist;
            Grd_FailureSMS.DataBind();
            Grd_FailureSMS.Columns[2].Visible = false;

        }
        protected void Btn_Retry_Click(object sender, EventArgs e)
        {
            SMS_Text = Session["sms_msg"].ToString();
            sms_sent_type = Session["sms_sent_type"].ToString();
            Sms_Type = int.Parse(Session["sms_Type"].ToString());
            string RE_phonelist = "";
            string RE_Nativelist = "";
            string msg="";
            char sym_bol = MysmsMang.GetSMS_NumberSeperator_FromDatabase();
            string Re_failedList = "";
            Grd_FailureSMS.Columns[2].Visible = true;
            DataSet DS_Failure = new DataSet();
            DS_Failure = Form_Grid_Dataset();
            if (DS_Failure != null && DS_Failure.Tables[0] != null && DS_Failure.Tables[0].Rows.Count > 0)
            {
                DataSet Re_FailedList = GetFormatedDataset();
                DataRow drr;
                DataTable dt = Re_FailedList.Tables["List"];
                foreach (DataRow dr in DS_Failure.Tables[0].Rows)
                {

                    if (int.Parse(dr["NativeLanguage"].ToString()) == 1)
                    {
                        if (RE_Nativelist != "")
                        {
                            RE_Nativelist = RE_Nativelist + sym_bol.ToString();
                        }
                        RE_Nativelist = RE_Nativelist + dr["Phone"].ToString();

                    }

                    if (int.Parse(dr["NativeLanguage"].ToString()) == 0)
                    {
                        if (RE_phonelist != "")
                        {
                            RE_phonelist = RE_phonelist + sym_bol.ToString();
                        }
                        RE_phonelist = RE_phonelist + dr["Phone"].ToString();
                    }
                }
                if (RE_phonelist != "")
                {


                    if (MysmsMang.SendBULKSms(RE_phonelist, SMS_Text, "90366450445", "WINER", true, out  Re_failedList))
                    {
                        MysmsMang.UpdateSMSLogtotable(RE_phonelist, sym_bol, SMS_Text, Sms_Type);
                        //lbl_error.Text = "SMS Circular has been Sent Successfully";

                        MyUser.m_DbLog.LogToDb(MyUser.UserName,sms_sent_type, "Message : " + SMS_Text, 1);

                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(Re_failedList))
                        {
                            string[] list = Re_failedList.Split(sym_bol);

                            if (list.Length > 0)
                            {
                                for (int i = 0; i < list.Length; i++)
                                {
                                    drr = dt.NewRow();

                                    drr["Phone"] = list[i];
                                    drr["NativeLanguage"] = 0;
                                    Re_FailedList.Tables["List"].Rows.Add(drr);
                                }
                            }
                        }

                    }
                }
                if (RE_Nativelist != "")
                {
                    Native_Text=Convertto_Nativesmstext(SMS_Text,out msg);
                    if (MysmsMang.SendBULKSms(RE_Nativelist, Native_Text, "90366450445", "WINER", true, out  Re_failedList))
                    {
                        MysmsMang.UpdateSMSLogtotable(RE_Nativelist, sym_bol, Native_Text, Sms_Type);
                        //lbl_error.Text = "SMS Circular has been Sent Successfully";
                        MyUser.m_DbLog.LogToDb(MyUser.UserName,sms_sent_type, "Message : " + Native_Text, 1);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(Re_failedList))
                        {
                            string[] list = Re_failedList.Split(sym_bol);
                            if (list.Length > 0)
                            {
                                for (int i = 0; i < list.Length; i++)
                                {
                                    drr = dt.NewRow();

                                    drr["Phone"] = list[i];
                                    drr["NativeLanguage"] = 1;

                                    Re_FailedList.Tables["List"].Rows.Add(drr);
                                }
                            }
                        }
                    }

                }
                if (Re_FailedList != null && Re_FailedList.Tables[0] != null && Re_FailedList.Tables[0].Rows.Count > 0)
                {
                    Grd_FailureSMS.Visible = true;
                    Grd_FailureSMS.DataSource = Re_FailedList;
                    Grd_FailureSMS.DataBind();
                    Grd_FailureSMS.Columns[2].Visible = false;
                    lbl_error.Text = "SMS Failure Phone Numbers";

                }
                else
                {
                    //sessions null
                    Session["sms_msg"] = null;
                    Session["sms_Type"] = null;
                    Session["sms_sent_type"] = null;
                    lbl_error.Text = "SMS has been Sent Successfully";

                }

            }
            else
            {
                Grd_FailureSMS.Visible =false;
                Btn_Retry.Enabled = false;
                lbl_error.Text = "Phonenumbers not checked";
            }

        }
        private DataSet Form_Grid_Dataset()
        {
            DataSet Grid_FailedList = new DataSet();
            Grid_FailedList = GetFormatedDataset();
            DataRow drr;
            DataTable dt = Grid_FailedList.Tables["List"];
            foreach (GridViewRow gr in Grd_FailureSMS.Rows)
            {
                CheckBox chkb = gr.FindControl("Chk_Select") as CheckBox;
                if (chkb.Checked)
                {
                    drr = dt.NewRow();
                    TextBox Txt_Ph = gr.FindControl("Txt_Phone") as TextBox;
                    drr["Phone"] = Txt_Ph.Text;
                    drr["NativeLanguage"] = gr.Cells[2].Text.ToString();
                    Grid_FailedList.Tables["List"].Rows.Add(drr);
                }
            }
            return Grid_FailedList;

        }
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
        private string Convertto_Nativesmstext(string sms_text,out string msg)
        {
            lbl_error.Text = "";
            string _NativesmsCOnvertError = "";
            string NativeLanguage = "";
            string nativesms = "";
            msg = "";
            if (ValidateNativeLanguage_Convert(sms_text, out NativeLanguage, out _NativesmsCOnvertError))
            {
                nativesms = GetNativeLanguageSMS(sms_text, NativeLanguage);
            }
            else
            {
                msg= _NativesmsCOnvertError;
            }
            return nativesms;
        }
        private bool ValidateNativeLanguage_Convert(string sms_text,out string NativeLanguage, out string _NativesmsCOnvertError)
        {
            _NativesmsCOnvertError = "";
            NativeLanguage = MysmsMang.GetNativeLanguage();
            bool _validate = true;
            if (NativeLanguage.Trim() == "")
            {
                _NativesmsCOnvertError = "Native Language Not Configured";
                return false;
            }

            if (sms_text == "")
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
                t.SourceText = Message;


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
    }
}
