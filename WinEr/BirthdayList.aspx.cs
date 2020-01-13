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
using System.Data.Odbc;
using RavSoft.GoogleTranslator;

namespace WinEr
{
    public partial class BirthdayList : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private SMSManager MysmsMang;
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
            else if (!MyUser.HaveActionRignt(893))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {

                    //some initlization
                    MysmsMang.InitClass();
                    LoadUserList();
                }
            }


        }
        private void LoadUserList()
        {
            Grid_List.DataSource = null;
            Grid_List.DataBind();

            Grid_Staff.DataSource = null;
            Grid_Staff.DataBind();


            if (Rdb_UserType.SelectedValue == "0")
            {
                LoadStudents();
                LoadSMS("OnStudentBirthday");
            }
            else
            {
                LoadStaff();
                LoadSMS("OnStaffBirthday");
            }
        }

        private void LoadSMS(string _type)
        {
            string _Message="",ScheduleTime="";
            MysmsMang.SMS_Enabled(_type,out _Message,out ScheduleTime);
            Txt_Message.Text = _Message;
            Load_Seperators(_type);
        }

        private void Load_Seperators(string _type)
        {
            string innerhtml = "<table cellspacing=\"10\">";
            string sql = "SELECT tblsmsseperators.`Type`,tblsmsseperators.Seperator FROM tblsmsseperators INNER JOIN tblsmsseperatormap ON tblsmsseperators.Id=tblsmsseperatormap.SeperatorId INNER JOIN tblsmsoptionconfig ON tblsmsseperatormap.TypeId=tblsmsoptionconfig.Id where  tblsmsoptionconfig.ShortName='" + _type + "'";
            OdbcDataReader MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {

                while (MyReader.Read())
                {
                    innerhtml = innerhtml + "<tr style=\"height:20px\"><td>" + MyReader.GetValue(0).ToString() + " : </td> <td class=\"new\"> " + MyReader.GetValue(1).ToString() + " </td></tr> ";
                }
            }
            innerhtml = innerhtml + "</table>";
            this.Seperators.InnerHtml = innerhtml;
        }

        private void LoadStudents()
        {
            lbl_msg.Text = "";
            MydataSet = GetStudentDataSet();
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grid_List.Columns[1].Visible = true;
                Grid_List.Columns[2].Visible = true;
                Grid_List.DataSource = MydataSet;
                Grid_List.DataBind();
                Grid_List.Columns[1].Visible = false;
                Grid_List.Columns[2].Visible = false;
            }
            else
            {
                lbl_msg.Text = "No student has birthday today";
                Grid_List.DataSource = null;
                Grid_List.DataBind();
            }
           
        }

        private DataSet GetStudentDataSet()
        {
            DateTime CurrentDate = DateTime.Now.Date;
            int Month = CurrentDate.Month;
            int Day = CurrentDate.Day;
            DataSet m_DataSet = new DataSet();
            m_DataSet.Tables.Add(new DataTable("List"));
            DataTable dt;
            DataRow dr;
            dt = m_DataSet.Tables["List"];
            dt.Columns.Add("Id");
            dt.Columns.Add("ParentName");
            dt.Columns.Add("StudentName");
            dt.Columns.Add("ClassName");
            dt.Columns.Add("Age");

            string sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblclass.ClassName,tblstudent.DOB,tblstudent.GardianName FROM tblstudent INNER JOIN tblclass ON tblstudent.LastClassId=tblclass.Id WHERE tblstudent.`Status`=1 AND Month(tblstudent.DOB)=" + Month + " AND Day(tblstudent.DOB)=" + Day + " ORDER BY tblclass.Id,tblstudent.StudentName";
            MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drow in MydataSet.Tables[0].Rows)
                {
                    dr = m_DataSet.Tables["List"].NewRow();
                    dr["Id"] = drow[0].ToString();
                    dr["StudentName"] = drow[1].ToString();
                    dr["ClassName"] = drow[2].ToString();
                    dr["Age"] = MyStudMang.GetAge(DateTime.Parse(drow[3].ToString()), CurrentDate);
                    dr["ParentName"] = drow[4].ToString();
                    m_DataSet.Tables["List"].Rows.Add(dr);
                }
            }
            return m_DataSet;
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

        private void LoadStaff()
        {
            lbl_msg.Text = "";
            MydataSet = GetStaffDataSet();
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grid_Staff.Columns[1].Visible = true;
                Grid_Staff.DataSource = MydataSet;
                Grid_Staff.DataBind();
                Grid_Staff.Columns[1].Visible = false;
            }
            else
            {
                lbl_msg.Text = "No staff has birthday today";
                Grid_Staff.DataSource = null;
                Grid_Staff.DataBind();
            }
        }

        private DataSet GetStaffDataSet()
        {
            DateTime CurrentDate = DateTime.Now.Date;
            int Month = CurrentDate.Month;
            int Day = CurrentDate.Day;
            DataSet m_DataSet = new DataSet();
            m_DataSet.Tables.Add(new DataTable("List"));
            DataTable dt;
            DataRow dr;
            dt = m_DataSet.Tables["List"];
            dt.Columns.Add("Id");
            dt.Columns.Add("StaffName");
            dt.Columns.Add("Age");

            string sql = "SELECT DISTINCT t.`Id`,t.`SurName`, tblstaffdetails.Dob  FROM tbluser t  inner join tblstaffdetails on tblstaffdetails.UserId = t.Id  inner join tblrole r on t.`RoleId`=r.`Id` where t.`Status`=1 AND r.`Type`='Staff' AND Month(tblstaffdetails.Dob)=" + Month + " AND Day(tblstaffdetails.Dob)=" + Day + " ORDER BY Day(tblstaffdetails.Dob),t.`SurName`";
            MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drow in MydataSet.Tables[0].Rows)
                {
                    dr = m_DataSet.Tables["List"].NewRow();
                    dr["Id"] = drow[0].ToString();
                    dr["StaffName"] = drow[1].ToString();
                    dr["Age"] = MyStudMang.GetAge(DateTime.Parse(drow[2].ToString()), CurrentDate);
                    m_DataSet.Tables["List"].Rows.Add(dr);
                }
            }
            return m_DataSet;
        }

        protected void Rdb_UserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUserList();
        }

        protected void Grid_List_RowEditing(object sender, GridViewEditEventArgs e)
        {
            string StudentId = Grid_List.Rows[e.NewEditIndex].Cells[1].Text;
            string ParentName = Grid_List.Rows[e.NewEditIndex].Cells[2].Text;
            string StudentName = Grid_List.Rows[e.NewEditIndex].Cells[3].Text;
            string Age = Grid_List.Rows[e.NewEditIndex].Cells[5].Text;
            string sms = "";
            if (Txt_Message.Text.Trim() != "")
            {
                string _msg = "";
                if (MysmsMang.GetCheckURL(out _msg) != "")
                {
                    try
                    {
                        if (SendSMSStudent(Txt_Message.Text.Trim(), StudentId, ParentName, StudentName, Age, out sms))
                        {
                            WC_MessageBox.ShowMssage("SMS Send Successfully");
                        }
                        else
                        {
                            if(String.IsNullOrEmpty(sms))
                            WC_MessageBox.ShowMssage("SMS Connection Failed", MSGTYPE.Alert);
                            else
                                WC_MessageBox.ShowMssage(sms);
                        }
                       
                    }
                    catch (Exception Ex)
                    {
                        WC_MessageBox.ShowMssage("Error while sending sms. Message : " + Ex.Message, MSGTYPE.Alert);
                    }
                }
                else
                {
                    WC_MessageBox.ShowMssage("SMS Connection Failed", MSGTYPE.Alert);
                }
                
                
            }
            else
            {
                WC_MessageBox.ShowMssage("Message cannot be empty", MSGTYPE.Alert);
            }
        }

        private bool SendSMSStudent(string Message, string StudentId, string ParentName, string StudentName, string Age, out string sms)
        {
            bool _done = false;
            Message = Message.Replace("($Parent$)", ParentName);
            Message = Message.Replace("($Student$)", StudentName);
            Message = Message.Replace("($Age$)", Age);
       
          
            string PhoneNo= MysmsMang.Get_SelectedParentPhoneNo_List(StudentId);

            sms = "";
            if (PhoneNo != "" && PhoneNo != "0")
            {
                //dominic
                string failedList = "";
                if (MysmsMang.SendBULKSms(PhoneNo, Message, "", "999", true, out  failedList))
                {
                    MysmsMang.UpdateSMSLogtotable(PhoneNo, ',', Txt_Message.Text, 2);
                    _done = true;
                    sms="Success" ;
                }
            }
            else
            {
             
                sms = "Please varify the phone numbers and try again";
                _done = false;
            }
           
            return _done;
        }


        protected void Grid_Staff_RowEditing(object sender, GridViewEditEventArgs e)
        {
            string StaffId = Grid_Staff.Rows[e.NewEditIndex].Cells[1].Text;
            string StaffName = Grid_Staff.Rows[e.NewEditIndex].Cells[2].Text;
            string Age = Grid_Staff.Rows[e.NewEditIndex].Cells[3].Text;
            if (Txt_Message.Text.Trim() != "")
            {
                
                string _msg = "";
                if (MysmsMang.GetCheckURL(out _msg) != "")
                {
                    try
                    {
                        if (SendStaffSMS(Txt_Message.Text.Trim(), StaffId, StaffName, Age))
                        {
                           

                            WC_MessageBox.ShowMssage("SMS Send Successfully");
                        }
                        else
                        {
                            WC_MessageBox.ShowMssage("SMS Connection Failed", MSGTYPE.Alert);
                        }
                    }
                    catch (Exception Ex)
                    {
                        WC_MessageBox.ShowMssage("Error while sending sms. Message : " + Ex.Message, MSGTYPE.Alert);
                    }
                }
                else
                {
                    WC_MessageBox.ShowMssage("SMS Connection Failed", MSGTYPE.Alert);
                }
                
            }
            else
            {
                WC_MessageBox.ShowMssage("Message cannot be empty", MSGTYPE.Alert);
            }
        }

        private bool SendStaffSMS(string Message, string StaffId, string StaffName, string Age)
        {
            bool _done = false;
            string failedList = "";
            Message = Message.Replace("($StaffName$)", StaffName);
            Message = Message.Replace("($Age$)", Age);
            string PhoneNo = MysmsMang.Get_StaffPhoneNo_List(StaffId);
            if (PhoneNo != "")
            {
                if (MysmsMang.SendBULKSms(PhoneNo, Message, "", "999", true,out  failedList))
                {
                    MysmsMang.UpdateSMSLogtotable(PhoneNo, ',', Txt_Message.Text, 0);
                    _done = true;
                }
            }
            return _done;
        }



        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("BirthdayList.aspx");
        }

        protected void Btn_SendAll_Click(object sender, EventArgs e)
        {
            string _msg = "";
         

            if (SmsSendingPossible(out _msg))
            {
                try
                {
                    bool _countinue = true;
                    if (Rdb_UserType.SelectedValue == "0")
                    {
                        foreach (GridViewRow gv in Grid_List.Rows)
                        {
                            CheckBox Chk = (CheckBox)gv.FindControl("CheckBoxUpdate");
                            
                            
                            
                            if (Chk.Checked && _countinue)
                            {

                                string StudentId = gv.Cells[1].Text;
                                string ParentName = gv.Cells[2].Text;
                                string StudentName = gv.Cells[3].Text;
                                string Age = gv.Cells[5].Text;




                                if (!SendSMSStudent(Txt_Message.Text.Trim(), StudentId, ParentName, StudentName, Age, out _msg))
                                {
                                    _countinue = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (GridViewRow gv in Grid_Staff.Rows)
                        {
                            CheckBox Chk = (CheckBox)gv.FindControl("CheckBoxUpdatenew");
                            if (Chk.Checked && _countinue)
                            {
                                string StaffId = gv.Cells[1].Text;
                                string StaffName = gv.Cells[2].Text;
                                string Age = gv.Cells[3].Text;
                                if (!SendStaffSMS(Txt_Message.Text.Trim(), StaffId, StaffName, Age))
                                {
                                    _countinue = false;
                                }
                            }
                        }
                    }

                    if (_countinue)
                    {
                        WC_MessageBox.ShowMssage("SMS Successfully Delivered");
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(_msg))

                            WC_MessageBox.ShowMssage("SMS to some numbers has not been send. Please try later", MSGTYPE.Alert);
                        else {
                            WC_MessageBox.ShowMssage("SMS to some numbers has not been send." + _msg, MSGTYPE.Alert);
                        }
                    
                    }

                }
                catch
                {
                    WC_MessageBox.ShowMssage("Error while senting SMS. Try later", MSGTYPE.Alert);
                }
            }
            else
            {
                WC_MessageBox.ShowMssage(_msg);
            }
        }

        private bool SmsSendingPossible(out string _msg)
        {
            _msg = "";
            bool _valid = true;

            if (Txt_Message.Text.Trim() == "")
            {
                _msg = "Message cannot be empty";
                _valid = false;
            }

            

            if (_valid)
            {
                bool _selected=false;
                if (Rdb_UserType.SelectedValue == "0")
                {
                    foreach (GridViewRow gv in Grid_List.Rows)
                    {
                        CheckBox Chk = (CheckBox)gv.FindControl("CheckBoxUpdate");
                        if (Chk.Checked)
                        {
                            _selected = true;
                            break;
                        }
                    }
                    if (!_selected)
                    {
                        _msg = "Please select student for senting SMS";
                        _valid = false;
                    }
                }
                else
                {
                    foreach (GridViewRow gv in Grid_Staff.Rows)
                    {
                        CheckBox Chk = (CheckBox)gv.FindControl("CheckBoxUpdatenew");
                        if (Chk.Checked)
                        {
                            _selected = true;
                            break;
                        }
                    }
                    if (!_selected)
                    {
                        _msg = "Please select staff for senting SMS";
                        _valid = false;
                    }
                }
            }


            if (_valid)
            {
                if (MysmsMang.GetCheckURL(out _msg) == "")
                {
                    _msg = "SMS Connection Failed";
                    _valid = false;
                }
            }

            return _valid;
        }

     

       
    }
}
