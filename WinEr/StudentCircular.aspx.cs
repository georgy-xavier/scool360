using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using RavSoft.GoogleTranslator;
using WinBase;



namespace WinEr
{
    public partial class StudentCircular : System.Web.UI.Page
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
            else if (!MyUser.HaveActionRignt(306))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {

                    //some initlization
                    Btn_Add.Enabled = false;
                    MysmsMang.InitClass();
                    Load_DrpClass();
                    Load_DrpClass1();
                    Load_DrpStaff();
                    Load_ParentDrpTamplate();
                    Load_ParentDrpTamplate1();
                    Load_StaffDrpTamplate();
                    Pnl_studGrid.Visible = false;
                    Panel_Staff_Grid.Visible = false;
                    loadnativelanguagearea();

                    Lnk_Retry.Visible = false;
                    Lnk_retrystaff.Visible = false;
                    Link_retry.Visible = false;
                }
            }
        }

        private void loadnativelanguagearea()
        {
            string _NativeLang = MysmsMang.GetNativeLanguage();
            if (MysmsMang.HaveNetConnection() && _NativeLang.Trim() != "" && (string.Compare(_NativeLang.Trim(), "English") != 0))
            {
                txtnewlanguage.Visible = true;
                btnConvert.Visible = true;
            }
            else
            {
                txtnewlanguage.Visible = false;
                btnConvert.Visible = false;
            }
        }



        # region Parent


        private void Load_ParentDrpTamplate()
        {
            Drp_parentTemplate.Items.Clear();
            ListItem li;

            string sql = "SELECT Id,Name FROM tblsmstemplate WHERE SelectType=1";
            DataSet dt = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("Select", "-1");
                Drp_parentTemplate.Items.Add(li);
                foreach (DataRow dr in dt.Tables[0].Rows)
                {

                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_parentTemplate.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No Template Found", "-1");
                Drp_parentTemplate.Items.Add(li);
            }
            Drp_parentTemplate.SelectedIndex = 0;
        }
        private void Load_ParentDrpTamplate1()
        {
            drp_parentlisttemplate.Items.Clear();
            ListItem li;

            string sql = "SELECT Id,Name FROM tblsmstemplate WHERE SelectType=1";
            DataSet dt = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("Select", "-1");
                drp_parentlisttemplate.Items.Add(li);
                foreach (DataRow dr in dt.Tables[0].Rows)
                {

                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    drp_parentlisttemplate.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No Template Found", "-1");
                drp_parentlisttemplate.Items.Add(li);
            }
            drp_parentlisttemplate.SelectedIndex = 0;
        }
        private void Load_SelectedparentTemplate()
        {
            Txt_Message.Text = "";
            if (Drp_parentTemplate.SelectedValue != "-1")
            {
                Txt_Message.Text = MysmsMang.GetSMSTemplate(Drp_parentTemplate.SelectedValue);
            }
        }

        protected void Drp_parentTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_SelectedparentTemplate();
        }



        private void Load_DrpClass()
        {

            Drp_Class.Items.Clear();
            ListItem li = new ListItem("Select any class", "-1");
            Drp_Class.Items.Add(li);
            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                Btn_Add.Enabled = false;
                li = new ListItem("No Class present", "-2");
                Drp_Class.Items.Add(li);
            }
            Drp_Class.SelectedIndex = 0;
        }
        private void Load_DrpClass1()
        {

            class_dropdown.Items.Clear();
            ListItem li = new ListItem("Select any class", "-1");
            class_dropdown.Items.Add(li);
            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    class_dropdown.Items.Add(li);
                }
            }
            else
            {
                Btn_Add.Enabled = false;
                li = new ListItem("No Class present", "-2");
                class_dropdown.Items.Add(li);
            }
            class_dropdown.SelectedIndex = 0;
        }
        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            Lbl_Message.Text = "";
            LoadStudents();
        }
        protected void class_dropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            Lbl_Message1.Text = "";
            LoadStudents1();
        }

        private void LoadStudents()
        {
            Drp_Student.Items.Clear();
            ListItem li;
            string sql = "SELECT tblstudent.Id,tblstudent.StudentName from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + int.Parse(Drp_Class.SelectedValue) + " Order by tblstudentclassmap.RollNo ASC";
            MydataSet = MysmsMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Btn_Add.Enabled = true;
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Student.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No Student present", "-2");
                Drp_Student.Items.Add(li);
                Btn_Add.Enabled = false;
            }
            Drp_Student.SelectedIndex = 0;
        }
        private void LoadStudents1()
        {
            DropDown_student.Items.Clear();
            ListItem li;
            string sql = "SELECT tblstudent.Id,tblstudent.StudentName from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id where tblstudent.OfficePhNo NOT IN(SELECT tblstudent.OfficePhNo from tblstudent INNER JOIN tbldevice on tbldevice.DeviceName = tblstudent.OfficePhNo where tbldevice.IsActive=1 AND tbldevice.DeviceType='PushDevice')AND tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + int.Parse(class_dropdown.SelectedValue) + " Order by tblstudentclassmap.RollNo ASC";
           // string sql = "SELECT tblstudent.Id,tblstudent.StudentName from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + int.Parse(class_dropdown.SelectedValue) + " Order by tblstudentclassmap.RollNo ASC";
            MydataSet = MysmsMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Button_add.Enabled = true;
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    DropDown_student.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No Student present", "-2");
                DropDown_student.Items.Add(li);
                Button_add.Enabled = false;
            }
            DropDown_student.SelectedIndex = 0;
        }


        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            Lbl_Message.Text = "";
            bool Countinue = false;
            DataSet list = new DataSet();
            if (GridStudents.Rows.Count > 0)
            {
                if (!AddedToGrid())
                {
                    list = AddPreviousData();
                    Countinue = true;
                }
            }
            else
            {
                list = GetStudentsList();
                Countinue = true;
            }
            if (Countinue && list != null && list.Tables != null && list.Tables[0].Rows.Count > 0)
            {
                GridStudents.Columns[0].Visible = true;
                GridStudents.Columns[2].Visible = true;
                GridStudents.DataSource = list;
                GridStudents.DataBind();
                GridStudents.Columns[0].Visible = false;
                GridStudents.Columns[2].Visible = false;
                Pnl_studGrid.Visible = true;
            }
            else if (Countinue)
            {
                GridStudents.DataSource = null;
                GridStudents.DataBind();
                Pnl_studGrid.Visible = false;
            }
        }
        protected void Button_Add_Click(object sender, EventArgs e)
        {
            Lbl_Message1.Text = "";
            bool Countinue = false;
            DataSet list = new DataSet();
            if (grid_student.Rows.Count > 0)
            {
                if (!AddedToGrid1())
                {
                    list = AddPreviousData1();
                    Countinue = true;
                }
            }
            else
            {
                list = GetStudentsList1();
                Countinue = true;
            }
            if (Countinue && list != null && list.Tables != null && list.Tables[0].Rows.Count > 0)
            {
                grid_student.Columns[0].Visible = true;
                grid_student.Columns[2].Visible = true;
                grid_student.DataSource = list;
                grid_student.DataBind();
                grid_student.Columns[0].Visible = false;
                grid_student.Columns[2].Visible = false;
                panel_student.Visible = true;
            }
            else if (Countinue)
            {
                grid_student.DataSource = null;
                grid_student.DataBind();
                panel_student.Visible = false;
            }
        }

        private bool AddedToGrid()
        {
            bool valid = false;
            foreach (GridViewRow gv in GridStudents.Rows)
            {
                if ((gv.Cells[0].Text.ToString() == Drp_Student.SelectedValue) && (gv.Cells[2].Text.ToString() == Drp_Class.SelectedValue))
                {
                    valid = true;
                    return true;
                }
            }
            return valid;
        }
        private bool AddedToGrid1()
        {
            bool valid = false;
            foreach (GridViewRow gv in grid_student.Rows)
            {
                if ((gv.Cells[0].Text.ToString() == DropDown_student.SelectedValue) && (gv.Cells[2].Text.ToString() == class_dropdown.SelectedValue))
                {
                    valid = true;
                    return true;
                }
            }
            return valid;
        }


        private DataSet GetStudentsList()
        {
            DataSet Students = new DataSet();
            DataTable dt;
            DataRow dr;
            Students.Tables.Add(new DataTable("StudList"));
            dt = Students.Tables["StudList"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("ClassId");
            dt.Columns.Add("Class");
            dr = Students.Tables[0].NewRow();
            dr["Id"] = Drp_Student.SelectedValue;
            dr["Name"] = Drp_Student.SelectedItem;
            dr["ClassId"] = Drp_Class.SelectedValue;
            dr["Class"] = Drp_Class.SelectedItem;
            Students.Tables[0].Rows.Add(dr);
            return Students;
        }
        private DataSet GetStudentsList1()
        {
            DataSet Students = new DataSet();
            DataTable dt;
            DataRow dr;
            Students.Tables.Add(new DataTable("StudList"));
            dt = Students.Tables["StudList"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("ClassId");
            dt.Columns.Add("Class");
            dr = Students.Tables[0].NewRow();
            dr["Id"] = DropDown_student.SelectedValue;
            dr["Name"] = DropDown_student.SelectedItem;
            dr["ClassId"] = class_dropdown.SelectedValue;
            dr["Class"] = class_dropdown.SelectedItem;
            Students.Tables[0].Rows.Add(dr);
            return Students;
        }

        private DataSet AddPreviousData()
        {

            DataSet _Students = new DataSet();
            DataTable dt;
            DataRow dr;
            int i;
            _Students.Tables.Add(new DataTable("List"));
            dt = _Students.Tables["List"];
            // dt.Columns.Add("Count");
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("ClassId");
            dt.Columns.Add("Class");

            i = 0;
            foreach (GridViewRow gv in GridStudents.Rows)
            {
                i++;
                dr = _Students.Tables["List"].NewRow();
                //dr["Count"] = i;
                dr["Id"] = gv.Cells[0].Text.ToString();
                dr["Name"] = gv.Cells[1].Text.ToString();
                dr["ClassId"] = gv.Cells[2].Text.ToString();
                dr["Class"] = gv.Cells[3].Text.ToString();
                _Students.Tables["List"].Rows.Add(dr);

            }
            dr = _Students.Tables[0].NewRow();
            dr["Id"] = Drp_Student.SelectedValue;
            dr["Name"] = Drp_Student.SelectedItem;
            dr["ClassId"] = Drp_Class.SelectedValue;
            dr["Class"] = Drp_Class.SelectedItem;
            _Students.Tables[0].Rows.Add(dr);

            return _Students;
        }
        private DataSet AddPreviousData1()
        {

            DataSet _Students = new DataSet();
            DataTable dt;
            DataRow dr;
            int i;
            _Students.Tables.Add(new DataTable("List"));
            dt = _Students.Tables["List"];
            // dt.Columns.Add("Count");
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("ClassId");
            dt.Columns.Add("Class");

            i = 0;
            foreach (GridViewRow gv in grid_student.Rows)
            {
                i++;
                dr = _Students.Tables["List"].NewRow();
                //dr["Count"] = i;
                dr["Id"] = gv.Cells[0].Text.ToString();
                dr["Name"] = gv.Cells[1].Text.ToString();
                dr["ClassId"] = gv.Cells[2].Text.ToString();
                dr["Class"] = gv.Cells[3].Text.ToString();
                _Students.Tables["List"].Rows.Add(dr);

            }
            dr = _Students.Tables[0].NewRow();
            dr["Id"] = DropDown_student.SelectedValue;
            dr["Name"] = DropDown_student.SelectedItem;
            dr["ClassId"] = class_dropdown.SelectedValue;
            dr["Class"] = class_dropdown.SelectedItem;
            _Students.Tables[0].Rows.Add(dr);

            return _Students;
        }

        protected void GridStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            string studentId = GridStudents.Rows[e.RowIndex].Cells[0].Text.ToString();
            DataSet list = DeleteRow(e.RowIndex);
            if (list != null && list.Tables != null && list.Tables[0].Rows.Count > 0)
            {
                GridStudents.DataSource = list;
                GridStudents.DataBind();
                Pnl_studGrid.Visible = true;
            }
            else
            {
                GridStudents.DataSource = null;
                GridStudents.DataBind();
                Pnl_studGrid.Visible = false;
            }

        }
        protected void grid_students_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            string studentId = grid_student.Rows[e.RowIndex].Cells[0].Text.ToString();
            DataSet list = DeleteRow1(e.RowIndex);
            if (list != null && list.Tables != null && list.Tables[0].Rows.Count > 0)
            {
                grid_student.DataSource = list;
                grid_student.DataBind();
                panel_student.Visible = true;
            }
            else
            {
                grid_student.DataSource = null;
                grid_student.DataBind();
                panel_student.Visible = false;
            }

        }

        private DataSet DeleteRow(int Id)
        {
            DataSet _Students = new DataSet();
            DataTable dt;
            DataRow dr;
            int i = 0;
            _Students.Tables.Add(new DataTable("List"));
            dt = _Students.Tables["List"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("ClassId");
            dt.Columns.Add("Class");

            foreach (GridViewRow gv in GridStudents.Rows)
            {
                if (i != Id)
                {
                    dr = _Students.Tables["List"].NewRow();
                    dr["Id"] = gv.Cells[0].Text.ToString();
                    dr["Name"] = gv.Cells[1].Text.ToString();
                    dr["ClassId"] = gv.Cells[2].Text.ToString();
                    dr["Class"] = gv.Cells[3].Text.ToString();
                    _Students.Tables["List"].Rows.Add(dr);
                }
                i++;
            }
            return _Students;
        }
        private DataSet DeleteRow1(int Id)
        {
            DataSet _Students = new DataSet();
            DataTable dt;
            DataRow dr;
            int i = 0;
            _Students.Tables.Add(new DataTable("List"));
            dt = _Students.Tables["List"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("ClassId");
            dt.Columns.Add("Class");

            foreach (GridViewRow gv in grid_student.Rows)
            {
                if (i != Id)
                {
                    dr = _Students.Tables["List"].NewRow();
                    dr["Id"] = gv.Cells[0].Text.ToString();
                    dr["Name"] = gv.Cells[1].Text.ToString();
                    dr["ClassId"] = gv.Cells[2].Text.ToString();
                    dr["Class"] = gv.Cells[3].Text.ToString();
                    _Students.Tables["List"].Rows.Add(dr);
                }
                i++;
            }
            return _Students;
        }
        protected void Btn_CheckConnection_Click(object sender, EventArgs e)
        {
            Lbl_Message.Text = "";
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

        protected void Btn_Send_Click(object sender, EventArgs e)
        {
            DataSet FailedList = GetFormatedDataset();
            DataRow dr;
            DataTable dt = FailedList.Tables["List"];
            bool existphonenumber = false;
            string _msg = "";
            string sms = "";
            bool SmsSent = false;
            //
            int smstype = 1;
            //
            string NativeSMS = "";
            if (!String.IsNullOrEmpty(txtnewlanguage.Text))
                NativeSMS = txtnewlanguage.Text.Trim();
            else
                NativeSMS = Txt_Message.Text.Trim();

            sms = Txt_Message.Text.Trim();

            if (Data_Complete(sms.Trim(), out _msg))
            {


                if (isParentSmsPossible(out _msg))
                {
                    string _StudentId = GetStudentIdFromGrid();
                    string phonelist = "";
                    string nativePhoneList = "";
                    char symbol = MysmsMang.GetSMS_NumberSeperator_FromDatabase();
                    Lbl_Message.Text = "";
                    if (phonelist != "")
                    {
                        phonelist = phonelist + symbol.ToString();
                    }
                    if (nativePhoneList != "")
                        nativePhoneList = nativePhoneList + symbol.ToString();

                    if (_StudentId != "")
                    {
                        phonelist = phonelist + MysmsMang.Get_SelectedParentPhoneNo_ListwioutNativelanguage(_StudentId);
                        nativePhoneList = nativePhoneList + MysmsMang.Get_SelectedParentPhoneNo_ListwithNativeLanguage(_StudentId);
                    }

                    if (phonelist != "")
                    {
                        //  if (MysmsMang.SendBULKSms(phonelist, Txt_Message.Text, "90366450445", "WINER", true))
                        //dominic
                        string failedList = "";
                        if (MysmsMang.SendBULKSms(phonelist, sms, "90366450445", "WINER", true, out  failedList))
                        {
                            MysmsMang.UpdateSMSLogtotable(phonelist, symbol, Txt_Message.Text, smstype);
                            //_msg = "SMS Circular has been Send Successfully";
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "SMS Selective Parent Circular", "Message : " + Txt_Message.Text, 1);
                            SmsSent = true;
                        }
                        else
                        {
                            //Lbl_Message.Text = "Error SMS Circulation";

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
                        }
                        existphonenumber = true;
                    }
                    else
                    {
                        existphonenumber = false;
                    }
                    if (nativePhoneList != "")
                    {
                        //  if (MysmsMang.SendBULKSms(phonelist, Txt_Message.Text, "90366450445", "WINER", true))
                        //dominic
                        string failedList = "";
                        if (MysmsMang.SendBULKSms(nativePhoneList, NativeSMS, "90366450445", "WINER", true, out  failedList))
                        {
                            MysmsMang.UpdateSMSLogtotable(nativePhoneList, symbol, NativeSMS, smstype);
                            // _msg = "SMS Circular has been Send Successfully";
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "SMS Selective Parent Circular", "Message : " + Txt_Message.Text, 1);
                            SmsSent = true;

                        }
                        else
                        {
                            //Lbl_Message.Text = "Error SMS Circulation";

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
                        }


                    }
                    if (FailedList != null && FailedList.Tables != null && FailedList.Tables[0].Rows.Count > 0)
                    {
                        Session["sms_msg"] = Txt_Message.Text.ToString().Trim();
                        Session["sms_Type"] = smstype;
                        Session["sms_sent_type"] = "SMS Selective Parent Circular";
                        Session["Faillist"] = FailedList;
                        Lnk_Retry.Visible = true;
                        Lbl_Message.Text = "Sending SMS failed,Do you want to retry";


                    }
                    else
                    {
                        existphonenumber = false;
                    }

                    if (phonelist == "" && nativePhoneList == "")
                    {
                        _msg = "Number not available for sending the message";


                    }
                }
                else
                {
                    Lbl_Message.Text = _msg;
                }
                if (SmsSent)
                    Lbl_Message.Text = "SMS Circular has been Send Successfully";
            }
            else
            {
                Lbl_Message.Text = _msg;
            }

            //Lbl_Message.Text = _msg;
        }

        protected void sendbutton_Click(object sender, EventArgs e)
        {
            DataSet FailedList = GetFormatedDataset();
            DataRow dr;
            DataTable dt = FailedList.Tables["List"];
            bool existphonenumber = false;
            string _msg = "";
            string sms = "";
            bool SmsSent = false;
            //
            int smstype = 1;
            //
            string NativeSMS = "";


            sms = txt_mobileappmsg.Text.Trim();

            if (Data_Complete(sms.Trim(), out _msg))
            {


                if (isParentSmsPossible1(out _msg))
                {
                    string _StudentId = GetStudentIdFromGrid1();
                    string phonelist = "";
                    string nativePhoneList = "";
                    char symbol = MysmsMang.GetSMS_NumberSeperator_FromDatabase();
                    Lbl_Message1.Text = "";
                    if (phonelist != "")
                    {
                        phonelist = phonelist + symbol.ToString();
                    }

                    if (_StudentId != "")
                    {
                        phonelist = phonelist + MysmsMang.Get_SelectedParentPhoneNo_ListwioutNativelanguage(_StudentId);

                    }

                    if (phonelist != "")
                    {
                        //  if (MysmsMang.SendBULKSms(phonelist, Txt_Message.Text, "90366450445", "WINER", true))
                        //dominic
                        string failedList = "";
                        if (MysmsMang.SendBULKSms(phonelist, sms, "90366450445", "WINER", true, out  failedList))
                        {
                            MysmsMang.UpdateSMSLogtotable(phonelist, symbol, Txt_Message.Text, smstype);
                            //_msg = "SMS Circular has been Send Successfully";
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "SMS Selective Parent Circular", "Message : " + Txt_Message.Text, 1);
                            SmsSent = true;
                        }
                        else
                        {
                            //Lbl_Message.Text = "Error SMS Circulation";

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
                        }
                        existphonenumber = true;
                    }
                    else
                    {
                        existphonenumber = false;
                    }

                    if (FailedList != null && FailedList.Tables != null && FailedList.Tables[0].Rows.Count > 0)
                    {
                        Session["sms_msg"] = Txt_Message.Text.ToString().Trim();
                        Session["sms_Type"] = smstype;
                        Session["sms_sent_type"] = "SMS Selective Parent Circular";
                        Session["Faillist"] = FailedList;
                        Link_retry.Visible = true;
                        Lbl_Message.Text = "Sending SMS failed,Do you want to retry";


                    }
                    else
                    {
                        existphonenumber = false;
                    }

                    if (phonelist == "" && nativePhoneList == "")
                    {
                        _msg = "Number not available for sending the message";


                    }
                }
                else
                {
                    Lbl_Message1.Text = _msg;
                }
                if (SmsSent)
                    Lbl_Message1.Text = "SMS Circular has been Send Successfully";
            }
            else
            {
                Lbl_Message1.Text = _msg;
            }

            //Lbl_Message.Text = _msg;
        }


        private bool isParentSmsPossible(out string msg)
        {
            bool valid = true;
            msg = "";

            if (Txt_Message.Text.Trim() == "")
            {
                msg = "Enter SMS Message";
                valid = false;
            }

            else if (Txt_Message.Text.Trim().Contains("&"))
            {
                msg = "SMS message should avoid symbol '&'. Message with symbol '&' will not be sent";
                valid = false;
            }
            return valid;
        }
        private bool isParentSmsPossible1(out string msg)
        {
            bool valid = true;
            msg = "";

            if (txt_mobileappmsg.Text.Trim() == "")
            {
                msg = "Enter SMS Message";
                valid = false;
            }

            else if (txt_mobileappmsg.Text.Trim().Contains("&"))
            {
                msg = "SMS message should avoid symbol '&'. Message with symbol '&' will not be sent";
                valid = false;
            }
            return valid;
        }


        private string GetStudentIdFromGrid()
        {
            string Student = "";
            foreach (GridViewRow gv in GridStudents.Rows)
            {
                if (Student == "")
                    Student = Student + gv.Cells[0].Text.ToString();
                else
                    Student = Student + "," + gv.Cells[0].Text.ToString();
            }
            return Student;
        }
        private string GetStudentIdFromGrid1()
        {
            string Student = "";
            foreach (GridViewRow gv in grid_student.Rows)
            {
                if (Student == "")
                    Student = Student + gv.Cells[0].Text.ToString();
                else
                    Student = Student + "," + gv.Cells[0].Text.ToString();
            }
            return Student;
        }
        //link for retry
        protected void Lnk_Retry_Click(object sender, EventArgs e)
        {
            Lnk_Retry.Visible = false;
            Lbl_Message.Text = "";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "var Mleft = (screen.width/2)-(760/2);var Mtop = (screen.height/2)-(700/2);window.open( 'RetryFailureSMS.aspx', null, 'height=510,width=600,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=centre,top=\'+Mtop+\', left=\'+Mleft+\'' );", true);
        }
        protected void Link_Retry_Click(object sender, EventArgs e)
        {
            Link_retry.Visible = false;
            Lbl_Message1.Text = "";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "var Mleft = (screen.width/2)-(760/2);var Mtop = (screen.height/2)-(700/2);window.open( 'RetryFailureSMS.aspx', null, 'height=510,width=600,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=centre,top=\'+Mtop+\', left=\'+Mleft+\'' );", true);
        }
        # endregion

        # region staff

        private void Load_StaffDrpTamplate()
        {
            Drp_staffTemplates.Items.Clear();
            ListItem li;

            string sql = "SELECT Id,Name FROM tblsmstemplate WHERE SelectType=0";
            DataSet dtstaff = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (dtstaff != null && dtstaff.Tables != null && dtstaff.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("Select", "-1");
                Drp_staffTemplates.Items.Add(li);
                foreach (DataRow dr in dtstaff.Tables[0].Rows)
                {

                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_staffTemplates.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No Template Found", "-1");
                Drp_staffTemplates.Items.Add(li);
            }
            Drp_parentTemplate.SelectedIndex = 0;
        }

        private void Load_SelectedstaffTemplate()
        {
            Txt_Staff_Message.Text = "";
            if (Drp_staffTemplates.SelectedValue != "-1")
            {
                Txt_Staff_Message.Text = MysmsMang.GetSMSTemplate(Drp_staffTemplates.SelectedValue);
            }
        }

        protected void Drp_StaffTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_SelectedstaffTemplate();
        }



        private void Load_DrpStaff()
        {
            Drp_Staff.Items.Clear();
            ListItem li;
            string sql = "SELECT tbluser.Id,tbluser.SurName FROM tbluser WHERE tbluser.`Status`=1 AND tbluser.RoleId!=1 ORDER BY tbluser.UserName";
            MydataSet = MysmsMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Btn_Add.Enabled = true;
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Staff.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No Staff Found", "-2");
                Drp_Staff.Items.Add(li);
                Btn_Add.Enabled = false;
            }
            Drp_Staff.SelectedIndex = 0;
        }


        protected void GridStaff_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataSet list = DeleteStaffRow(e.RowIndex);
            if (list != null && list.Tables != null && list.Tables[0].Rows.Count > 0)
            {
                GridStaff.DataSource = list;
                GridStaff.DataBind();
                Panel_Staff_Grid.Visible = true;
            }
            else
            {
                GridStaff.DataSource = null;
                GridStaff.DataBind();
                Panel_Staff_Grid.Visible = false;
            }
        }

        private DataSet DeleteStaffRow(int Id)
        {
            DataSet _Staffs = new DataSet();
            DataTable dt;
            DataRow dr;
            int i = 0;
            _Staffs.Tables.Add(new DataTable("List"));
            dt = _Staffs.Tables["List"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");


            foreach (GridViewRow gv in GridStaff.Rows)
            {
                if (i != Id)
                {
                    dr = _Staffs.Tables["List"].NewRow();
                    dr["Id"] = gv.Cells[0].Text.ToString();
                    dr["Name"] = gv.Cells[1].Text.ToString();

                    _Staffs.Tables["List"].Rows.Add(dr);
                }
                i++;
            }
            return _Staffs;
        }

        protected void Btn_Staff_CheckConnection_Click(object sender, EventArgs e)
        {
            Lbl_Staff_msg.Text = "";
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


        private string GetStaffIdFromGrid()
        {
            string Staff = "";
            foreach (GridViewRow gv in GridStaff.Rows)
            {
                if (Staff == "")
                    Staff = Staff + gv.Cells[0].Text.ToString();
                else
                    Staff = Staff + "," + gv.Cells[0].Text.ToString();
            }
            return Staff;
        }

        protected void Btn_Staff_Send_Click(object sender, EventArgs e)
        {
            Lbl_Staff_msg.Text = "";
            string _msg = "";
            string failedList = "";

            if (Data_Complete(Txt_Staff_Message.Text.Trim(), out _msg))
            {
                if (IstaffSmsPossible(out _msg))
                {
                    DataSet FailedList = GetFormatedDataset();
                    DataRow dr;
                    DataTable dt = FailedList.Tables["List"]; ;
                    string _StaffId = GetStaffIdFromGrid();
                    string phonelist = "";
                    char symbol = MysmsMang.GetSMS_NumberSeperator_FromDatabase();
                    Lbl_Message.Text = "";
                    if (phonelist != "")
                    {
                        phonelist = phonelist + symbol.ToString();
                    }
                    if (_StaffId != "")
                    {
                        phonelist = phonelist + MysmsMang.Get_StaffPhoneNo_List(_StaffId);
                    }
                    if (phonelist != "")
                    {
                        // dominic sms
                        if (MysmsMang.SendBULKSms(phonelist, Txt_Staff_Message.Text, "90366450445", "WINER", true, out  failedList))
                        {
                            MysmsMang.UpdateSMSLogtotable(phonelist, symbol, Txt_Message.Text, 0);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "SMS Selective Staff Circular", "Message : " + Txt_Message.Text, 1);
                            Txt_Staff_Message.Text = "";
                            GridStaff.DataSource = null;
                            GridStaff.DataBind();
                            Panel_Staff_Grid.Visible = false;
                            Lbl_Staff_msg.Text = "SMS Circular has been Sent Successfully";


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

                            //Lbl_Staff_msg.Text = "Error In SMS Circulation";
                        }
                        if (FailedList != null && FailedList.Tables[0] != null && FailedList.Tables[0].Rows.Count > 0)
                        {
                            Session["sms_msg"] = Txt_Staff_Message.Text.ToString().Trim();
                            Session["sms_Type"] = 0;
                            Session["sms_sent_type"] = "SMS Selective Staff Circular";
                            Session["Faillist"] = FailedList;

                            //link button
                            Lbl_Staff_msg.Text = "Sending SMS failed,Do you want to retry";
                            Lnk_retrystaff.Visible = true;

                        }
                    }
                    else
                    {
                        Lbl_Staff_msg.Text = "Number not available for sending the message";
                    }
                }
                else
                {
                    Lbl_Staff_msg.Text = _msg;
                }
            }
            else
            {
                Lbl_Staff_msg.Text = _msg;
            }
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

        private bool IstaffSmsPossible(out string msg)
        {
            bool valid = true;
            msg = "";

            if (Txt_Staff_Message.Text.Trim() == "")
            {
                msg = "Enter SMS Message";
                valid = false;
            }

            else if (Txt_Staff_Message.Text.Trim().Contains("&"))
            {
                msg = "SMS message should avoid symbol '&'. Message with symbol '&' will not be sent";
                valid = false;
            }
            return valid;
        }

        protected void Btn_Staff_Add_Click(object sender, EventArgs e)
        {
            Lbl_Staff_msg.Text = "";
            bool Countinue = false;
            DataSet list = new DataSet();
            if (GridStaff.Rows.Count > 0)
            {
                if (!AddedToStaffGrid())
                {
                    list = AddStaffPreviousData();
                    Countinue = true;
                }
            }
            else
            {
                list = GetStaffsList();
                Countinue = true;
            }
            if (Countinue && list != null && list.Tables != null && list.Tables[0].Rows.Count > 0)
            {
                GridStaff.Columns[0].Visible = true;

                GridStaff.DataSource = list;
                GridStaff.DataBind();
                GridStaff.Columns[0].Visible = false;
                Panel_Staff_Grid.Visible = true;
            }
            else if (Countinue)
            {
                GridStaff.DataSource = null;
                GridStaff.DataBind();
                Panel_Staff_Grid.Visible = false;
            }
        }

        private bool AddedToStaffGrid()
        {

            bool valid = false;
            foreach (GridViewRow gv in GridStaff.Rows)
            {
                if ((gv.Cells[0].Text.ToString() == Drp_Staff.SelectedValue))
                {
                    valid = true;
                    return true;
                }
            }
            return valid;

        }

        private DataSet GetStaffsList()
        {
            DataSet Staffs = new DataSet();
            DataTable dt;
            DataRow dr;
            Staffs.Tables.Add(new DataTable("List"));
            dt = Staffs.Tables["List"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");

            dr = Staffs.Tables[0].NewRow();
            dr["Id"] = Drp_Staff.SelectedValue;
            dr["Name"] = Drp_Staff.SelectedItem;

            Staffs.Tables[0].Rows.Add(dr);
            return Staffs;
        }

        private DataSet AddStaffPreviousData()
        {
            DataSet _Staffs = new DataSet();
            DataTable dt;
            DataRow dr;
            int i;
            _Staffs.Tables.Add(new DataTable("List"));
            dt = _Staffs.Tables["List"];
            // dt.Columns.Add("Count");
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");


            i = 0;
            foreach (GridViewRow gv in GridStaff.Rows)
            {
                i++;
                dr = _Staffs.Tables["List"].NewRow();
                //dr["Count"] = i;
                dr["Id"] = gv.Cells[0].Text.ToString();
                dr["Name"] = gv.Cells[1].Text.ToString();

                _Staffs.Tables["List"].Rows.Add(dr);

            }
            dr = _Staffs.Tables[0].NewRow();
            dr["Id"] = Drp_Staff.SelectedValue;
            dr["Name"] = Drp_Staff.SelectedItem;

            _Staffs.Tables[0].Rows.Add(dr);

            return _Staffs;
        }

        //link retry

        protected void Lnk_retrystaff_Click(object sender, EventArgs e)
        {
            Lnk_retrystaff.Visible = false;
            Lbl_Staff_msg.Text = "";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "var Mleft = (screen.width/2)-(760/2);var Mtop = (screen.height/2)-(700/2);window.open( 'RetryFailureSMS.aspx', null, 'height=510,width=600,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=centre,top=\'+Mtop+\', left=\'+Mleft+\'' );", true);
        }

        # endregion

        private bool Data_Complete(string message, out string msg)
        {
            bool valid = true;
            msg = "";

            if (message.Trim() == "")
            {
                msg = "Enter SMS Message";
                valid = false;
            }
            else if (message.Trim().Contains("($") || message.Trim().Contains("$)"))
            {
                msg = "Before sending, replace symbol ($ $) parts with correct data in template";
                valid = false;
            }
            else if (message.Trim().Contains("&"))
            {
                msg = "SMS message should avoid symbol '&'. Message with symbol '&' will not be sent";
                valid = false;
            }
            return valid;
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
        protected void btnconvert_Click(object sender, EventArgs e)
        {
            string _NativesmsCOnvertError = "";
            string NativeLanguage = "";
            if (ValidateNativeLanguage_Convert(out NativeLanguage, out _NativesmsCOnvertError))
            {
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
                        txtnewlanguage.Text = t.Translation;

                    }
                    catch (Exception ex)
                    {
                        // Lbl_Message.Text = ex.Message;
                        Lbl_Message.Text = "Does not support  Native language";
                        txtnewlanguage.Text = Txt_Message.Text;

                    }
                    finally
                    {

                    }
                }
                else
                {
                    txtnewlanguage.Text = Txt_Message.Text;
                }
            }
            else
            {
                Lbl_Message.Text = _NativesmsCOnvertError;
            }
        }


    }
}
