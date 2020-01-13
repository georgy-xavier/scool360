using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using WinBase;
namespace WinEr
{
    public partial class ParentLoginManager : System.Web.UI.Page
    {
        private ConfigManager MyConfiMang;
        private DataSet MydataSet;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private SMSManager MysmsMang;
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyConfiMang = MyUser.GetConfigObj();
            MysmsMang = MyUser.GetSMSMngObj();
            if (MysmsMang == null)
            {
                Response.Redirect("RoleErr.htm");

            }
            if (MyConfiMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(760))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    AddClassToDropDownClass();
                    LoadParentLoginStatus();
                }
            }
        }
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];

            if (MyUser.SELECTEDMODE == 1)
            {
                this.MasterPageFile = "~/WinerStudentMaster.master";

            }
            else if (MyUser.SELECTEDMODE == 2)
            {

                this.MasterPageFile = "~/WinerSchoolMaster.master";
            }

        }

       
        private void LoadParentLoginStatus()
        {
            string sql = "select Value from tblparent_config where ID=2";
            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                if (MyReader.GetValue(0).ToString() == "1")
                    Chk_ActivateParentLogin.Checked = true;
                else
                    Chk_ActivateParentLogin.Checked = false;
                   
            }
        }

        private void AddClassToDropDownClass()
        {
            Drp_Class.Items.Clear();
            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Btn_Show.Enabled = true;
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Class Present", "-1");
                Drp_Class.Items.Add(li);
                Btn_Show.Enabled = false;
            }
            Drp_Class.SelectedIndex = 0;
        }

        protected void Btn_Show_Click(object sender, EventArgs e)
        {
            FillGrid();
            FillPopSmSDetails();
            //FillSmSDetails();
        }

        //private void FillSmSDetails()
        //{
        //    Txt_SmsText.Text = "Dear parent , your login to our web portal has been activated. Url is ($Url$) , User Name is ($UserName$) and Password is ($Password$) from ($School$)";
        //    string innerhtml = "<table cellspacing=\"10\">";
           

        //    innerhtml = innerhtml + "<tr style=\"height:20px\"><td>($Url$): </td> <td class=\"new\"> Web portel name </td></tr> ";
        //    innerhtml = innerhtml + "<tr style=\"height:20px\"><td>($UserName$): </td> <td class=\"new\"> Login name </td></tr> ";
        //    innerhtml = innerhtml + "<tr style=\"height:20px\"><td>($Password$): </td> <td class=\"new\"> Password</td></tr> ";
        //    innerhtml = innerhtml + "<tr style=\"height:20px\"><td>($School$): </td> <td class=\"new\"> School Name</td></tr> ";

        //    innerhtml = innerhtml + "</table>";
        //    this.Seperators.InnerHtml = innerhtml;
        //}



        private void FillGrid()
        {
            Lbl_Message.Text = "";
            Grd_ParentList.Columns[0].Visible = true;
            Grd_ParentList.Columns[6].Visible = true;
            Grd_ParentList.Columns[7].Visible = true;
            Grd_ParentList.Columns[8].Visible = true;
            Grd_ParentList.Columns[9].Visible = true;
            Grd_ParentList.Columns[11].Visible = true;
            DataSet MyParenetsList = MyConfiMang.GetParentsList(Drp_Class.SelectedValue);
            if (MyParenetsList != null && MyParenetsList.Tables != null && MyParenetsList.Tables[0].Rows.Count > 0)
            {
                Grd_ParentList.DataSource = MyParenetsList;
                Grd_ParentList.DataBind();
               // Pnl_SMS.Visible = true;
                Pnl_Identifier.Visible = true;
                Btn_SaveChanges.Visible = true;
                Btn_GenCred.Enabled = true;
                Btn_SendSMS.Enabled = true;
            }
            else
            {
                Lbl_Message.Text = "No parent list found";
                Pnl_Identifier.Visible = false;
                Grd_ParentList.DataSource = null;
                Grd_ParentList.DataBind();
              //  Pnl_SMS.Visible = false;
                Btn_SaveChanges.Visible = false;
                Btn_GenCred.Enabled = false;
                Btn_SendSMS.Enabled = false;
            }
            Grd_ParentList.Columns[0].Visible = false;
            Grd_ParentList.Columns[6].Visible = false;
            Grd_ParentList.Columns[7].Visible = false;
            Grd_ParentList.Columns[8].Visible = false;
            Grd_ParentList.Columns[9].Visible = false;
            Grd_ParentList.Columns[11].Visible = false;
        }


        protected void GrdParent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image img_Status = (Image)e.Row.FindControl("Img_ParentStatus");
                CheckBox Chk_Login = (CheckBox)e.Row.FindControl("Chk_Login");
                if (e.Row.Cells[6].Text == "1")// SMS sent
                    img_Status.ImageUrl = "~/images/04fd3f.jpg";
                else if (e.Row.Cells[9].Text != "0") //USerNAme Generated
                    img_Status.ImageUrl = "~/images/0277fb.jpg";
                else
                    img_Status.ImageUrl = "~/images/f93204.jpg";

                //if (e.Row.Cells[4].Text == "1")
                   
                //    e.Row.BackColor = System.Drawing.Color.LightGreen;
                //else
                //{
                //     e.Row.BackColor =System.Drawing.Color.White;
                //     e.Row.BorderColor = System.Drawing.Color.Olive;
                //}
                if (e.Row.Cells[7].Text == "1")
                    Chk_Login.Checked = true;
                else
                    Chk_Login.Checked = false;
            }

        }
        protected void Btn_GenerateCredentials_Click(object sender, EventArgs e)
        {
            try
            {
                KnowinEncryption MyEncription = new KnowinEncryption();
                if (Rdo_GenCredOption.SelectedValue == "2")
                {
                    string _PassWord = "";
                    string _CanLogin = "0";
                    string EmailId = "";
                    int ActiveSecureAuth = 0;
                    CheckBox Chk_Select;
                   
                    foreach (GridViewRow gv in Grd_ParentList.Rows)
                    {
                        EmailId = "";
                        Chk_Select = (CheckBox)gv.FindControl("Chk_Login");
                        if (Chk_Select.Checked && gv.Cells[8].Text.ToString() != "0")
                        _CanLogin = "1";
                        else
                        _CanLogin = "0";
                        _PassWord = MysmsMang.GenerateParentLoginPassword();

                        int SiblingId = MyConfiMang.GetSiblingId(gv.Cells[0].Text.ToString());
                        string CypherText = MyEncription.Encrypt(_PassWord);

                        if (!String.IsNullOrEmpty(gv.Cells[5].Text.Trim()) && gv.Cells[5].Text.Contains("@gmail.com"))
                        {
                            ActiveSecureAuth=1;
                            EmailId = gv.Cells[5].Text.Trim();
                        }
                        
                        if (gv.Cells[4].Text.ToString() != "0" && (Rdo_GenCredOption.SelectedValue == "2" && gv.Cells[8].Text == "0"))
                        {
                            if (gv.Cells[8].Text.ToString() != "0")
                                MysmsMang.UpdateParentCredentials(CypherText, gv.Cells[4].Text.ToString(), gv.Cells[3].Text.ToString(), _CanLogin, "0", gv.Cells[8].Text.ToString(),EmailId, ActiveSecureAuth);
                            else
                                MysmsMang.SaveParentCredentials(CypherText, gv.Cells[4].Text.ToString(), gv.Cells[3].Text.ToString(), _CanLogin, "0", gv.Cells[0].Text.ToString(), SiblingId, EmailId, ActiveSecureAuth);
                        }
                    }
                }
                else
                {
                    DataSet MyParentData = MysmsMang.GetAllAPrentData();
                    DataSet SelectedPArents = MysmsMang.GetSelectedParents();
                    DataSet ParentStudentList = MysmsMang.GetParentStudentMap();
                    string _ParentId = "0";

                    string EmailId = "";
                    int ActiveSecureAuth = 0;

                    foreach (DataRow Dr_AllParent in MyParentData.Tables[0].Rows)
                    {
                        EmailId = "";
                        int SiblingId = MyConfiMang.GetSiblingId(Dr_AllParent[2].ToString());
                        if (PasswordnotGenerated(Dr_AllParent, SelectedPArents)) // insert parent entry
                        {
                            string CypherText = MyEncription.Encrypt(MysmsMang.GenerateParentLoginPassword());

                            if (!String.IsNullOrEmpty(Dr_AllParent["Email"].ToString()) && Dr_AllParent["Email"].ToString().Contains("@gmail.com"))
                            {
                                EmailId = Dr_AllParent["Email"].ToString();
                                ActiveSecureAuth = 0;
                            }

                            MysmsMang.SaveParentCredentials(CypherText, Dr_AllParent[1].ToString(), Dr_AllParent[0].ToString(), "0", "0", Dr_AllParent[2].ToString(), SiblingId, EmailId, ActiveSecureAuth);
                        }
                        else if (!StudentAdded(Dr_AllParent, SelectedPArents, ParentStudentList, out _ParentId)) // insert only student entry
                        {
                            MysmsMang.AddStudentToParent(_ParentId, Dr_AllParent[2].ToString());
                        }
                        MyParentData = MysmsMang.GetAllAPrentData();
                        SelectedPArents = MysmsMang.GetSelectedParents();
                        ParentStudentList = MysmsMang.GetParentStudentMap();
                    }
                }
                MyEncription = null;
            }
            catch
            {
                WC_MessageBox.ShowMssage("Credentials are not generated for some parents");
            }
           
            FillGrid();
            WC_MessageBox.ShowMssage("Credentials generated successfully");
        }

        private bool StudentAdded(DataRow Dr_AllParent, DataSet SelectedPArents, DataSet ParentStudentList, out string ParentID)
        {
            bool Valid = false;
            ParentID = GetParentID(Dr_AllParent, SelectedPArents);
            if (ParentID != "0")
            {
                foreach (DataRow Dr_ParentStudentMap in ParentStudentList.Tables[0].Rows)
                {
                    if (ParentID == Dr_ParentStudentMap[0].ToString() && Dr_AllParent[2].ToString() == Dr_ParentStudentMap[0].ToString())
                        return true;
                }
            }
            return Valid;
        }

        private string GetParentID(DataRow Dr_AllParent, DataSet SelectedPArents)
        {
            string ParentId = "0";
            foreach (DataRow Dr_SelParent in SelectedPArents.Tables[0].Rows)
            {
                if (Dr_AllParent[0].ToString().ToLower() == Dr_SelParent[1].ToString().ToLower())
                    return Dr_SelParent[0].ToString();
            }
            return ParentId;
        }

        private bool PasswordnotGenerated(DataRow Dr_AllParent, DataSet SelectedPArents)
        {
            bool Valid = true;
            foreach (DataRow Dr_SelParent in SelectedPArents.Tables[0].Rows)
            {
                if (Dr_AllParent[0].ToString().ToLower() == Dr_SelParent[1].ToString().ToLower())
                    return false;
            }
            return Valid;
        }
        

        protected void Btn_SaveChanges_Click(object sender, EventArgs e)
        {
            CheckBox Chk_Select;
            string _CanLogin = "0";
            foreach (GridViewRow gv in Grd_ParentList.Rows)
            {
                Chk_Select = (CheckBox)gv.FindControl("Chk_Login");
                if (Chk_Select.Checked && gv.Cells[8].Text.ToString()!="0")
                    _CanLogin = "1";
                else
                    _CanLogin = "0";
                MysmsMang.UpdateParentLoginStatus(gv.Cells[8].Text.ToString(), _CanLogin);
            }
            FillGrid();
            WC_MessageBox.ShowMssage("Changes are saved");
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Parent Login Modification", "Parent Login Modification done for the class " + Drp_Class.SelectedItem.Text, 1);
        }

        protected void Btn_Send_Click(object sender, EventArgs e)
        {
            if (Hdn_StudentId.Value == "0")
            {
                SendBulkSMS();
            }
            else
            {
                if (SendIndividualSMS())
                {
                    Hdn_StudentId.Value = "0";
                    FillGrid();
                    WC_MessageBox.ShowMssage("SMS sent successfully");
                }
                else
                {
                    WC_MessageBox.ShowMssage("Could not send SMS. Please verify the data provided");
                }
            }
           
        }

        private bool SendIndividualSMS()
        {
            bool Valid = false;
            try
            {
                MysmsMang.InitClass();
                string phonelist = "";
                string Message = "";
                string _PassWord = "";
                string CypherText = "";
                string ParentName = "";
                string _UserName = "";
                string parentId = "0";
                string failedList = "";
                bool _New = false;
                char symbol = MysmsMang.GetSMS_NumberSeperator_FromDatabase();
                int SiblingId = MyConfiMang.GetSiblingId(Hdn_StudentId.Value);
                string EmailId = "";
                int ActiveSecureAuth=0;
                if (MysmsMang.GetParentData(out ParentName, out parentId, out _UserName, out EmailId, Hdn_StudentId.Value, SiblingId))
                {
                    _New = false;
                    _PassWord = MysmsMang.ReadPassword(parentId);
                }
                else // Generate password & send
                {
                    KnowinEncryption MyEncription = new KnowinEncryption();
                    _PassWord = MysmsMang.GenerateParentLoginPassword();
                    CypherText = MyEncription.Encrypt(_PassWord);
                    _New = true;
                    MyEncription = null;
                }
                if (_UserName != "" && _UserName != "0")
                {
                    phonelist = _UserName + symbol;
                    Message = MysmsMang.ParentLoginSMSstring(Txt_InSmStext.Text, ParentName, MyUser.ParentLoginUrl, MyUser.SchoolName, _UserName, _PassWord);
                    // dominic sms
                   
                    if (MysmsMang.SendBULKSms(phonelist, Message, "90366450445", "WINER", true, out  failedList)) // if newly generated then save username and password
                    {
                        if (_New)
                        {
                            if (!String.IsNullOrEmpty(EmailId) && EmailId.Contains("@gmail.com"))
                            {
                                ActiveSecureAuth = 1;
                            }
                            MysmsMang.SaveParentCredentials(CypherText, _UserName, ParentName, "0", "1", Hdn_StudentId.Value, SiblingId, EmailId, ActiveSecureAuth);                                                        
                        }
                        else
                            MysmsMang.UpdateSMSStatus(parentId);
                        Valid = true;
                        
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Parent Credentials Send", "Parent Credentials Send to parent " + ParentName, 1);
                    }
                }
                else
                {
                    Valid = false;
                }
            }
            catch
            {
                Valid = false;
            }
            return Valid;
        }

        private void SendBulkSMS()
        {
            MysmsMang.InitClass();
            string phonelist = "";
            string msg = "";
            string Message = "";
            bool Valid= false;
            string _PassWord = "";
             string CypherText="";
            bool _New= false;
            bool sent = false;
            Grd_ParentList.Columns[0].Visible = true;
            Grd_ParentList.Columns[6].Visible = true;
            Grd_ParentList.Columns[7].Visible = true;
            Grd_ParentList.Columns[8].Visible = true;
            Grd_ParentList.Columns[11].Visible = true;
            char symbol = MysmsMang.GetSMS_NumberSeperator_FromDatabase();
          
            if (Data_Complete(out msg))
            {
                CheckBox Chk_Select;
                string _CanLogin = "0";
                KnowinEncryption MyEncription = new KnowinEncryption();
                if (Rdo_ClassList.SelectedValue == "2")//selected class
                {
                    foreach (GridViewRow gv in Grd_ParentList.Rows)
                    {
                        //if (gv.Cells[4].Text.ToString() != "0" && (Rdo_GenCredOption.SelectedValue == "1" || (Rdo_GenCredOption.SelectedValue == "2" && gv.Cells[5].Text == "0")))
                        //{
                            Chk_Select = (CheckBox)gv.FindControl("Chk_Login");
                            if (Chk_Select.Checked && gv.Cells[7].Text.ToString() != "0")
                                _CanLogin = "1";
                            else
                                _CanLogin = "0";
                            phonelist = gv.Cells[4].Text.ToString();
                            if (phonelist != "")
                                phonelist = phonelist + symbol.ToString();
                            if (phonelist != "")
                            {
                                if (Rdo_SMSSendOption.SelectedValue == "1" || (Rdo_SMSSendOption.SelectedValue == "2" && gv.Cells[6].Text == "0"))
                                {
                                    sent = false;
                                    _PassWord = "";
                                    if (gv.Cells[9].Text.ToString() != "0")
                                    {
                                        _New = false;
                                        _PassWord = MysmsMang.ReadPassword(gv.Cells[8].Text.ToString());
                                    }
                                    else // Generate password & send
                                    {
                                        _PassWord = MysmsMang.GenerateParentLoginPassword();
                                        CypherText = MyEncription.Encrypt(_PassWord);
                                        _New = true;
                                    }
                                    if (_CanLogin == "1")
                                    {
                                        int SiblingId = MyConfiMang.GetSiblingId(gv.Cells[0].Text.ToString());
                                        Message = MysmsMang.ParentLoginSMSstring(Txt_InSmStext.Text, gv.Cells[3].Text.ToString(), MyUser.ParentLoginUrl, MyUser.SchoolName, gv.Cells[4].Text.ToString(), _PassWord);
                                        //dominic sms
                                        string failedList = "";
                                      
                                        if (MysmsMang.SendBULKSms(phonelist, Message, "90366450445", "WINER", true,out  failedList)) // if newly generated then save username and password
                                        {
                                            if (_New)
                                            {
                                                int ActiveSecureAuth = 0;
                                                string EmailId = "";
                                                if (!String.IsNullOrEmpty(gv.Cells[5].Text.Trim()) && gv.Cells[5].Text.Contains("@gmail.com"))
                                                {
                                                    ActiveSecureAuth = 1;
                                                    EmailId = gv.Cells[5].Text.Trim();
                                                }

                                                MysmsMang.SaveParentCredentials(CypherText, gv.Cells[4].Text.ToString(), gv.Cells[3].Text.ToString(), _CanLogin, "1", gv.Cells[0].Text.ToString(), SiblingId, EmailId, ActiveSecureAuth);
                                            }
                                            else
                                                MysmsMang.UpdateSMSStatus(gv.Cells[8].Text.ToString());
                                            Valid = true;
                                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Parent Credentials Send", "Parent Credentials Send to parent " + gv.Cells[3].Text.ToString(), 1);
                                        }
                                    }
                                }
                                else if (Rdo_SMSSendOption.SelectedValue == "2" && gv.Cells[5].Text == "1")
                                {
                                    sent = true;
                                }
                            }
                        //}

                    }
                }
                else // ForAll students
                {
                    DataSet MyParentData = GetAllAPrentData();
                    foreach (DataRow Dr_AllParent in MyParentData.Tables[0].Rows)
                    {
                        if (Rdo_SMSSendOption.SelectedValue == "1" || (Rdo_SMSSendOption.SelectedValue == "2" && Dr_AllParent[3].ToString() == "0"))
                        {
                            phonelist = Dr_AllParent[2].ToString();
                            if (phonelist != "")
                                phonelist = phonelist + symbol.ToString();
                            if (phonelist != "")
                            {
                                if (Dr_AllParent[4].ToString() != "0")
                                {
                                    _New = false;
                                    _PassWord = Dr_AllParent[4].ToString();
                                }
                                else // Generate password & send
                                {
                                    _PassWord = MysmsMang.GenerateParentLoginPassword();
                                    CypherText = MyEncription.Encrypt(_PassWord);
                                    _New = true;
                                }
                                int SiblingId = MyConfiMang.GetSiblingId(Dr_AllParent[0].ToString());
                                Message = MysmsMang.ParentLoginSMSstring(Txt_InSmStext.Text, Dr_AllParent[1].ToString(), MyUser.ParentLoginUrl, MyUser.SchoolName, Dr_AllParent[2].ToString(), _PassWord);

                                //dominic
                                string failedList = "";
                                if (MysmsMang.SendBULKSms(phonelist, Message, "90366450445", "WINER", true,out  failedList)) // if newly generated then save username and password
                                {
                                    if (_New)
                                    {
                                        string EmailId="";
                                        int ActiveSecureAuth = 0;
                                        if (!String.IsNullOrEmpty(Dr_AllParent["Email"].ToString()) && Dr_AllParent["Email"].ToString().Contains("@gmail.com"))
                                        {
                                            EmailId = Dr_AllParent["Email"].ToString();
                                            ActiveSecureAuth = 0;
                                        }

                                        MysmsMang.SaveParentCredentials(CypherText, Dr_AllParent[2].ToString(), Dr_AllParent[1].ToString(), _CanLogin, "1", Dr_AllParent[0].ToString(), SiblingId, EmailId, ActiveSecureAuth);
                                    }
                                    else
                                        MysmsMang.UpdateSMSStatus(Dr_AllParent[8].ToString());
                                    Valid = true;
                                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Parent Credentials Send", "Parent Credentials Send to parent " + Dr_AllParent[1].ToString(), 1);
                                }
                            }
                        }
                        
                        MyParentData = GetAllAPrentData();
                    }
                }
                MyEncription = null;


            }
            Grd_ParentList.Columns[0].Visible = false;
            Grd_ParentList.Columns[6].Visible = false;
            Grd_ParentList.Columns[7].Visible = false;
            Grd_ParentList.Columns[8].Visible = false;
            Grd_ParentList.Columns[11].Visible = false;
            FillGrid();

            if (Valid)
                WC_MessageBox.ShowMssage("SMS sent successfully");
            else if(sent)
                WC_MessageBox.ShowMssage("Login Credentials already sent");
            else
                WC_MessageBox.ShowMssage("SMS sending failed. Please try again");
        }

        private DataSet GetAllAPrentData()
        {
            DataSet MyDataset = new DataSet();
            MyDataset.Tables.Add(new DataTable("Parent"));
            DataTable dt = MyDataset.Tables["Parent"];
            DataRow dr;
            dt.Columns.Add("StudentId");
            dt.Columns.Add("ParentName");
            dt.Columns.Add("MobNumber");
            dt.Columns.Add("SMSSend");
            dt.Columns.Add("Password");
            dt.Columns.Add("ParentId");
            string _Password="" , ParentId="";
            DataSet MyParents = MysmsMang.GetAllAPrentData();

            if (MyParents != null && MyParents.Tables != null && MyParents.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Dr_Pa in MyParents.Tables[0].Rows)
                {
                    dr = MyDataset.Tables["Parent"].NewRow();
                    dr["StudentId"] = Dr_Pa[2].ToString();
                    dr["ParentName"] = Dr_Pa[0].ToString();
                    dr["MobNumber"] = Dr_Pa[1].ToString();
                    dr["SMSSend"] = GetSendStatus(dr["StudentId"].ToString(), out _Password, out ParentId);
                    dr["Password"] = _Password;
                    dr["ParentId"] = ParentId;
                    MyDataset.Tables["Parent"].Rows.Add(dr);
                }
            }
            return MyDataset;
        }

        private string GetSendStatus(string _StudId, out string _Password , out string _ParentId)
        {
            string Status = "0";
            _Password = "0";
            _ParentId = "0";
            OdbcDataReader MyTempreader = null;
            KnowinEncryption MyEncript = new KnowinEncryption();
            string sql = "select tblparent_parentdetails.SentCredentials , tblparent_parentdetails.Password ,tblparent_parentdetails.Id from tblparent_parentstudentmap inner join tblparent_parentdetails on tblparent_parentdetails.Id = tblparent_parentstudentmap.ParentId where tblparent_parentstudentmap.StudentId in(" + _StudId + ")";
            MyTempreader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyTempreader.HasRows)
            {
                Status = MyTempreader.GetValue(0).ToString();
                _Password = MyEncript.Decrypt(MyTempreader.GetValue(1).ToString());
                _ParentId = MyTempreader.GetValue(1).ToString();
            }
            MyEncript = null;
            return Status;
        }

       

        private bool Data_Complete(out string msg)
        {
            bool valid = true;
            msg = "";
            if (Txt_InSmStext.Text.Trim() == "")
            {
                msg = "Enter SMS Message";
                valid = false;
            }
            return valid;
        }


        protected void Btn_SendSMS_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Show();
        //    MPE_MessageBox.Show();
            Rdo_SMSSendOption.Visible = true;
            Rdo_ClassList.Visible = true;
            FillPopSmSDetails();
        }

        protected void GrdParent_RowDelete(object sender, GridViewDeleteEventArgs e)
        {
            ModalPopupExtender1.Show();
            Hdn_StudentId.Value = Grd_ParentList.Rows[e.RowIndex].Cells[0].Text.ToString();
            Rdo_SMSSendOption.Visible = false;
            Rdo_ClassList.Visible = false;
            FillPopSmSDetails();
        }

        protected void Btn_magok_Click(object sender, EventArgs e)
        {
            Hdn_StudentId.Value = "0";
           
        }


        protected void Grd_ParentList_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            Lbl_Password.Text = "";
            Hdn_PassStudentId.Value = Grd_ParentList.SelectedRow.Cells[0].Text;
            Txt_Student.Text = Grd_ParentList.SelectedRow.Cells[2].Text;
            Txt_ParentName.Text = Grd_ParentList.SelectedRow.Cells[3].Text;
            Txt_USerId.Text = Grd_ParentList.SelectedRow.Cells[4].Text;

            Txt_logindate.Text=Grd_ParentList.SelectedRow.Cells[11].Text.Replace("&nbsp;","");
            Txt_EmailId.Text = Grd_ParentList.SelectedRow.Cells[5].Text.Replace("&nbsp;", "");

            Lbl_Password.Text = MysmsMang.GetPassword(Txt_ParentName.Text, Txt_USerId.Text, Hdn_PassStudentId.Value);
            Lbl_Password.Visible = false;
            Lbl_DetailsMessage.Text = "";
            Hdn_ParentId.Value = Grd_ParentList.SelectedRow.Cells[8].Text;
            ModalPopupExtender_Details.Show();
            CheckBox chkBx = (CheckBox)Grd_ParentList.SelectedRow.FindControl("Chk_Login");
            if (chkBx.Checked)
                Chk_EdtCanLogin.Checked = true;
            else
                Chk_EdtCanLogin.Checked = false;
            Lnk_GenPass.Text = "Show Password";
        }

        protected void Chk_ActivateParentLogin_Cheked(object sender, EventArgs e)
        {
            string _Status="0";
            string Message = "";
            if (Chk_ActivateParentLogin.Checked)
            {

                _Status = "1";
                Message = "Parent Login is enabled";
            }
            else
            {
                _Status = "0";
                Message = "Parent Login is disabled";
            }
            MysmsMang.UpdateParentLoginStatus(_Status);
            Lbl_EnableLogin.Text = Message;
            MPE_AdvanceOption.Show();
        }

        protected void Lnk_GenPass_Click(object sender, EventArgs e)
        {
            if (Lnk_GenPass.Text == "Show Password")
            {
                Lnk_GenPass.Text = "Hide";
                Lbl_Password.Visible = true;
            }
            else
            {
                Lbl_Password.Visible = false;
                Lnk_GenPass.Text ="Show Password";
            }
            ModalPopupExtender_Details.Show();
        }
        
        private void FillPopSmSDetails()
        {
            Txt_InSmStext.Text = "Dear parent , your login to our web portal has been activated. Url is ($Url$) , User Name is ($UserName$) and Password is ($Password$) from ($School$)";
            string innerhtml = "<table cellspacing=\"10\">";
            innerhtml = innerhtml + "<tr style=\"height:20px\"><td>($Url$): </td> <td class=\"new\"> Web portel name </td></tr> ";
            innerhtml = innerhtml + "<tr style=\"height:20px\"><td>($UserName$): </td> <td class=\"new\"> Login name </td></tr> ";
            innerhtml = innerhtml + "<tr style=\"height:20px\"><td>($Password$): </td> <td class=\"new\"> Password</td></tr> ";
            innerhtml = innerhtml + "<tr style=\"height:20px\"><td>($School$): </td> <td class=\"new\"> School Name</td></tr> ";
            innerhtml = innerhtml + "</table>";
            this.Div_ind.InnerHtml = innerhtml;
        }

        protected void Btn_SaveDetails_Click(object sender, EventArgs e)
        {
            try
            {
                KnowinEncryption MyEncription = new KnowinEncryption();
                string _msg = "";

                if (IsSave_possible(out _msg))
                {
                    string _CanLogin = "0";
                    if (Chk_EdtCanLogin.Checked)
                        _CanLogin = "1";
                    else
                        _CanLogin = "0";
                    string Password = "";
                    int SiblingId = MyConfiMang.GetSiblingId(Hdn_PassStudentId.Value);
                    if (Lbl_Password.Text != "")
                        Password = Lbl_Password.Text;
                    else
                        Password = MysmsMang.GenerateParentLoginPassword();
                    string sql = "";
                    string gmailId="";
                    int AciveSecureAuth = 0;
                   
                    if (!String.IsNullOrEmpty(Txt_EmailId.Text.Trim()) && Txt_EmailId.Text.Trim().Contains("@gmail.com"))
                    {
                        gmailId=Txt_EmailId.Text.Trim();
                        AciveSecureAuth = 1;

                    }
                    else
                    {
                        gmailId="";
                    }

                    if (Hdn_ParentId.Value != "0")
                    {
                        sql = "update tblparent_parentdetails set UserName='" + Txt_USerId.Text.Trim() + "' , Password='" + MyEncription.Encrypt(Password) + "',CanLogin=" + _CanLogin + ",GmailAuthId='" + gmailId + "', SiblingId=" + SiblingId + ", IsActiveSecure="+AciveSecureAuth+" Where Id=" + Hdn_ParentId.Value;
                        MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
                       
                        if (!MysmsMang.ParentStudentExists(Hdn_ParentId.Value, Hdn_PassStudentId.Value))
                        {
                            if (!IsStudentEntryExists_InPerantLogin(Hdn_PassStudentId.Value))
                            {
                                sql = "insert into tblparent_parentstudentmap(ParentId,StudentId) values(" + Hdn_ParentId.Value + "," + Hdn_PassStudentId.Value + ")";
                                MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
                            }
                        }

                    }
                    else
                    {
                        MysmsMang.SaveParentCredentials(MyEncription.Encrypt(Password), Txt_USerId.Text.Trim(), Txt_ParentName.Text.Trim(), _CanLogin, "0", Hdn_PassStudentId.Value, SiblingId, gmailId, AciveSecureAuth);
                    }
                    //sql = "update tblstudent set OfficePhNo ='" + Txt_USerId.Text.Trim() + "' where Id=" + Hdn_PassStudentId.Value;
                    //MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
                    //MysmsMang.InsertParentMobileNumberIntoSMSParenstsList(int.Parse(Hdn_PassStudentId.Value), Txt_USerId.Text.Trim());
                    FillGrid();
                    WC_MessageBox.ShowMssage("Details Saved");
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Parent credentials Saved", "Credentials saved for parent " + Txt_ParentName.Text, 1);
                }
                else
                {
                    Lbl_DetailsMessage.Text = _msg;
                    ModalPopupExtender_Details.Show();
                }
            }
            catch(Exception ex)
            {
                WC_MessageBox.ShowMssage("Error while saving. Try later... Error Message : " + ex.Message);
            }
        }

        private bool IsStudentEntryExists_InPerantLogin(string _StudentId)
        {
            bool Valid = false;
            string sql = "select ParentId from tblparent_parentstudentmap where StudentId=" + _StudentId;
            MyReader = MysmsMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Valid = true;
            }
            return Valid;
        }

        private bool IsSave_possible(out string _msg)
        {
            _msg = "";
            bool _valid = true;
            if (Txt_USerId.Text.Trim() == "")
            {
                _msg = "Please enter the userId";
                _valid = false;
            }
            if (_valid)
            {
                if (Chk_EdtCanLogin.Checked)
                {
                    if (Txt_USerId.Text.Trim() == "0")
                    {
                        _msg = "Please enter the userId";
                        _valid = false;
                    }
                }
            }
            if (_valid)
            {
                string sql = "select Id from tblparent_parentdetails where UserName='" + Txt_USerId.Text.Trim() + "' and Id<>" + Hdn_ParentId.Value+" and SiblingId=0";
                MyReader = MysmsMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    _msg = "UserID is already exist";
                    _valid = false;
                }
            }
            if (_valid)
            {
                if (!String.IsNullOrEmpty(Txt_EmailId.Text.Trim()) && !Txt_EmailId.Text.Trim().Contains("@gmail.com"))
                {
                    _msg = "Please enter your gmail Id";
                    _valid = false;

                }
                else if (!String.IsNullOrEmpty(Txt_EmailId.Text.Trim()))
                {
                    string sql = "select Id from tblparent_parentdetails where GmailAuthId='" + Txt_EmailId.Text.Trim() + "' and Id<>" + Hdn_ParentId.Value ;
                    MyReader = MysmsMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        _msg = "Email id  is already exist";
                        _valid = false;
                    }
                }

            }

            return _valid;

        }

        protected void Btn_RemoveCand_Click(object sender, EventArgs e)
        {
            if (Hdn_ParentId.Value != "0")
            {
                string _studid = Hdn_PassStudentId.Value;
                int _parentId = int.Parse(Hdn_ParentId.Value);
                string sql = "Delete from tblparent_parentstudentmap where tblparent_parentstudentmap.StudentId=" + _studid;
                MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
                if (!HasChild(_parentId))
                {
                    sql = "Delete from tblparent_parentdetails where tblparent_parentdetails.Id=" + _parentId;
                    MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
                }
                FillGrid();
                WC_MessageBox.ShowMssage("Credentials Removed.");
            }
        }

        private bool HasChild(int _parentId)
        {
            bool _HasChild = false;
            string sql = "SELECT tblparent_parentstudentmap.ParentId FROM tblparent_parentstudentmap WHERE tblparent_parentstudentmap.ParentId=" + _parentId;
            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _HasChild = true;
            }
            return _HasChild;
        }
        
    }
}
