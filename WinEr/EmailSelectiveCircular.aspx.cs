using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;
using System.IO;
using System.Configuration;

namespace WinEr
{
    public partial class EmailSelectiveCircular : System.Web.UI.Page
    {

        private StudentManagerClass MyStudMang;
        private EmailManager Obj_Email;
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private SchoolClass objSchool = null;
        private DataSet MyDataSet = null;
        private MysqlClass _Mysqlobj;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MyFeeMang = MyUser.GetFeeObj();
            Obj_Email = MyUser.GetEmailObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(874))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (WinerUtlity.NeedCentrelDB())
                {
                    if (Session[WinerConstants.SessionSchool] == null)
                    {
                        Response.Redirect("Logout.aspx");
                    }
                    objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                    if (objSchool != null)
                        _Mysqlobj = new MysqlClass(objSchool.ConnectionString);
                }
                else
                {
                    _Mysqlobj = new MysqlClass(WinerUtlity.SingleSchoolConnectionString);
                }
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(Btnattachparrent);
                scriptManager.RegisterPostBackControl(Btnattachstaff); 
             
                if (!IsPostBack)
                {
                    LoadClassToDropDown();
                    LoadStudents();
                    Pnl_studGrid.Visible = false;
                    Panel_Staff_Grid.Visible = false;
                    Load_DrpStaff();
                    LoadTypeDropDown();
                    Session["filelisttable"] = null;
                    Session["filelisttablestaff"] = null;
                    loadattachmentfiles();
                    staffloadattachmentfiles();
                    
                }
                Lbl_Err.Text = "";
                Lbl_StaffErr.Text = "";
                lblattacherror.Text = "";
                lblerrorstaff.Text = "";
            }

        }



        protected void Drp_StaffTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "";
            if (int.Parse(Drp_StaffTemplate.SelectedValue) > 0)
            {
                sql = "Select tbl_generalemailtemplate.Subject, tbl_generalemailtemplate.Body,Enabled from tbl_generalemailtemplate where tbl_generalemailtemplate.Id=" + int.Parse(Drp_StaffTemplate.SelectedValue) + "";
                MyReader = Obj_Email.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Txt_StaffEmailSub.Text = MyReader.GetValue(0).ToString().Replace("&nbsp;", "");
                    Editor_staffBody.Content = MyReader.GetValue(1).ToString().Replace("&nbsp;", "");


                }
            }
            else
            {
                Txt_StaffEmailSub.Text = "";
                Editor_staffBody.Content = "";
            }
        }


        protected void Drp_Template_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "";
            if (int.Parse(Drp_Template.SelectedValue) > 0)
            {
                sql = "Select tbl_generalemailtemplate.Subject, tbl_generalemailtemplate.Body,Enabled from tbl_generalemailtemplate where tbl_generalemailtemplate.Id=" + int.Parse(Drp_Template.SelectedValue) + "";
                MyReader = Obj_Email.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Txt_EmailSubject.Text = MyReader.GetValue(0).ToString().Replace("&nbsp;", "");
                    Editor_Body.Content = MyReader.GetValue(1).ToString().Replace("&nbsp;", "");


                }
            }
            else
            {
                Txt_EmailSubject.Text = "";
                Editor_Body.Content = "";
            }
        }

        private void LoadTypeDropDown()
        {
            DataSet TypeDs = new DataSet();
            ListItem li;
            Drp_Template.Items.Clear();
            Drp_StaffTemplate.Items.Clear();
            TypeDs = MyStudMang.GetGeneralEmailTemplate();
            if (TypeDs != null && TypeDs.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("Select Type", "0");
                Drp_Template.Items.Add(li);
                Drp_StaffTemplate.Items.Add(li);
                foreach (DataRow dr in TypeDs.Tables[0].Rows)
                {
                    li = new ListItem(dr["Type"].ToString(), dr["Id"].ToString());
                    Drp_Template.Items.Add(li);
                    Drp_StaffTemplate.Items.Add(li);
                }

            }
            else
            {
                li = new ListItem("None", "-1");
                Drp_Template.Items.Add(li);
                Drp_StaffTemplate.Items.Add(li);
            }
        }

        private void LoadClassToDropDown()
        {
            DataSet ClassDs = new DataSet();
            ListItem li;
            Drp_Class.Items.Clear();
            ClassDs = Obj_Email.LoadClassDetails();
            if (ClassDs != null && ClassDs.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("Select Class", "0");
                Drp_Class.Items.Add(li);
                foreach (DataRow dr in ClassDs.Tables[0].Rows)
                {
                    li = new ListItem(dr["ClassName"].ToString(), dr["Id"].ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("None", "-1");
                Drp_Class.Items.Add(li);
            }
        }

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {

            //Lbl_Message.Text = "";            
           
                LoadStudents();
        }

        private void LoadStudents()
        {
            Drp_Student.Items.Clear();
            ListItem li;
             
            if (int.Parse(Drp_Class.SelectedValue) > 0)
            {

                //SELECT tblstudent.Id,tblstudent.StudentName from tblstudent INNER JOIN tblstudentclassmap on
                // tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=43
                //  AND tblstudentclassmap.ClassId=
                //2 and tblstudent.Id in(select tbl_emailparentlist.Id  from tbl_emailparentlist where  tbl_emailparentlist.Enabled=1) 
                //  Order by tblstudentclassmap.RollNo ASC


                string sql = "SELECT tblstudent.Id,tblstudent.StudentName from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + "   AND tblstudentclassmap.ClassId=" + int.Parse(Drp_Class.SelectedValue) + "  and tblstudent.Id in(select tbl_emailparentlist.Id  from tbl_emailparentlist where  tbl_emailparentlist.Enabled=1)  Order by tblstudentclassmap.RollNo ASC";
                MyDataSet = Obj_Email.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

                if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
                {
                    Btn_Add.Enabled = true;
                    foreach (DataRow dr in MyDataSet.Tables[0].Rows)
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
            else
            {
                li = new ListItem("No Student present", "-2");
                Drp_Student.Items.Add(li);
                Btn_Add.Enabled = false;

            }
        }

        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            //Lbl_Message.Text = "";
            bool Countinue = false;
            DataSet list = new DataSet();
            if (int.Parse(Drp_Student.SelectedValue) != -2)
            {
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
            else
            {
                GridStudents.DataSource = null;
                GridStudents.DataBind();
                Pnl_studGrid.Visible = false;
            }
        }

        protected void Btn_SendEmail_Click(object sender, EventArgs e)
        {
            OdbcDataReader EmailReader = null;
            int success = 1;
            if (GridStudents.Rows.Count > 0)
            {
                if (Txt_EmailSubject.Text != "" && Editor_Body.Content != "")
                {
                    try
                    {
                  
                  string attach1 = "", attach2 = "", attach3 = "";
                  DataTable dtattach = new DataTable();
                  if (Session["filelisttable"] != null)
                   {
                       dtattach = (DataTable)Session["filelisttable"];
                       for (int i = 0; i < dtattach.Rows.Count; i++)
                       {
                           if (i == 0)
                           {
                               attach1 = dtattach.Rows[i]["RepositoryFileName"].ToString();
                           }
                           if (i == 1)
                           {
                               attach2 = dtattach.Rows[i]["RepositoryFileName"].ToString();
                           }
                           if (i == 2)
                           {
                               attach3 = dtattach.Rows[i]["RepositoryFileName"].ToString();
                           }
                       }
                   }
                
                        Obj_Email.CreateTansationDb();
                        foreach (GridViewRow gr in GridStudents.Rows)
                        {
                            int studentId = int.Parse(gr.Cells[0].Text);
                            EmailReader = Obj_Email.GetEmailParentId(-1, studentId);
                            if (EmailReader.HasRows)
                            {
                                if (EmailReader.GetValue(0).ToString().Replace("&nbsp;", "") != "")
                                {
                                    string EmailBody = Editor_Body.Content.Replace("'", "").Replace("\\", "");
                                    EmailBody = Obj_Email.SeperatorReplace(0, int.Parse(EmailReader.GetValue(1).ToString()), EmailBody);
                                    Obj_Email.InsertDataToAutoEmailListwithattachment(EmailReader.GetValue(0).ToString(), EmailReader.GetValue(1).ToString(), Txt_EmailSubject.Text, EmailBody, 2,attach1,attach2,attach3);
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        success = 0;
                        WC_MsgBox.ShowMssage("Sending failed,Please try again later");
                        Obj_Email.EndFailTansationDb();
                        loadattachmentfiles();

                    }
                    if (success == 1)
                    {
                        Obj_Email.EndSucessTansationDb();
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Email", "Emails sent to students", 1);
                        WC_MsgBox.ShowMssage("Email has been sent successfully");
                        //Obj_Email.EndSucessTansationDb();
                        LoadClassToDropDown();
                        LoadStudents();
                        GridStudents.DataSource = null;
                        GridStudents.DataBind();
                        Pnl_studGrid.Visible = false;
                        Txt_EmailSubject.Text = "";
                        Editor_Body.Content = "";
                        LoadTypeDropDown();
                        Session["Attachmentfilelisttable"] = null;
                        loadattachmentfiles();
                    }
                }
                else
                {
                    Lbl_Err.Text = "Enter email subject and body";
                    loadattachmentfiles();
                }
            }
            else
            {
                Lbl_Err.Text = "Add any student";
                loadattachmentfiles();
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


        protected void Btn_StaffSend_Click(object sender, EventArgs e)
        {
            int success = 1;
            OdbcDataReader Emailreader = null; 
            if (GridStaff.Rows.Count > 0)
            {
                if (Txt_StaffEmailSub.Text != "" && Editor_staffBody.Content != "")
                {
                    try
                    {
                        string s_attach1 = "", s_attach2 = "", s_attach3 = "";
                        DataTable s_dtattach = new DataTable();
                        if (Session["filelisttablestaff"] != null)
                        {
                            s_dtattach = (DataTable)Session["filelisttablestaff"];
                            for (int i = 0; i < s_dtattach.Rows.Count; i++)
                            {
                                if (i == 0)
                                {
                                    s_attach1 = s_dtattach.Rows[i]["RepositoryFileName"].ToString();
                                }
                                if (i == 1)
                                {
                                    s_attach2 = s_dtattach.Rows[i]["RepositoryFileName"].ToString();
                                }
                                if (i == 2)
                                {
                                    s_attach3 = s_dtattach.Rows[i]["RepositoryFileName"].ToString();
                                }
                            }
                        }
                        Obj_Email.CreateTansationDb();
                        foreach (GridViewRow gr in GridStaff.Rows)
                        {
                            string StaffID = gr.Cells[0].Text;
                            Emailreader = Obj_Email.GetEmailStaffId(StaffID);
                            if (Emailreader.HasRows)
                            {
                                if (Emailreader.GetValue(0).ToString().Replace("&nbsp;", "") != "")
                                {
                                    string EmailBody = Editor_staffBody.Content;
                                    EmailBody = Obj_Email.SeperatorReplace(int.Parse(Emailreader.GetValue(1).ToString()), 0, EmailBody);
                                    Obj_Email.InsertDataToAutoEmailListwithattachment(Emailreader.GetValue(0).ToString(), Emailreader.GetValue(1).ToString(), Txt_StaffEmailSub.Text, EmailBody, 1,s_attach1,s_attach2,s_attach3);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        success = 0;
                        WC_MsgBox.ShowMssage("Sending failed,Please try again later");
                        Obj_Email.EndFailTansationDb();
                        staffloadattachmentfiles();
                    }
                    if (success == 1)
                    {
                        Obj_Email.EndSucessTansationDb();
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Email", "Emails sent to staff", 1);
                        WC_MsgBox.ShowMssage("Email has been sent successfully");
                        Obj_Email.EndSucessTansationDb();
                        Load_DrpStaff();
                        LoadTypeDropDown();
                        Editor_staffBody.Content = "";
                        Txt_StaffEmailSub.Text = "";
                        GridStaff.DataSource = null;
                        GridStaff.DataBind();
                        Panel_Staff_Grid.Visible = false;
                        Session["filelisttablestaff"] = null;
                        staffloadattachmentfiles();
                    }
                }
                else
                {
                    Lbl_StaffErr.Text = "Enter email subject and body";
                    staffloadattachmentfiles();
                }
            }
            else
            {
                Lbl_StaffErr.Text = "Select any staff";
                staffloadattachmentfiles();
            }

        }

        private void Load_DrpStaff()
        {
            Drp_Staff.Items.Clear();
            ListItem li;
            string sql = "SELECT tbluser.Id,tbluser.UserName FROM tbluser WHERE tbluser.`Status`=1 AND tbluser.RoleId!=1 and tbluser.Id in(select tbl_emailstafflist.Id from tbl_emailstafflist where tbl_emailstafflist.Enabled=1) ORDER BY tbluser.UserName";
            MyDataSet = Obj_Email.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                Btn_Add.Enabled = true;
                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
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

        protected void Btn_StaffAdd_Click(object sender, EventArgs e)
        {
           // Lbl_Staff_msg.Text = "";
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
       //Email attachment code for parent tab
        protected void Btnattachparrent_Click(object sender, EventArgs e)
        {
            string filename = "";
            string Rep_filename = "";

            try
            {
                if (attachmentvalidationsforparent(out filename, out Rep_filename))
                {

                    getattachfilestogrid(filename, Rep_filename);
                }
            }
            catch (Exception ed)
            {
                WC_MsgBox.ShowMssage(ed.Message);
            }
        }
         private bool attachmentvalidationsforparent( out string filenametosave,out string repfilename)
        {
            bool isvalidattach = false;
            int checkaccessfileattach = 0;
            filenametosave = "";
            repfilename = "";
            string savepath = "";
            //string pathfilesave = Server.MapPath("") + "\\" + ConfigurationSettings.AppSettings["EmailAttachmentFilePath"] + "//";
            string pathfilesave = Server.MapPath("") + "\\" + "Email Attachments\\";
            if (!Directory.Exists(pathfilesave))
            {
                Directory.CreateDirectory(pathfilesave);
            }
            if (fileUploadattachments.HasFile)
            {
                string filename = System.IO.Path.GetFileName(fileUploadattachments.FileName);
                filenametosave = filename.ToString();
                string fileExtension = Path.GetExtension(filename);
                fileExtension = fileExtension.ToLower();
                string[] fnmeparts = filename.Split('.');
                string curdate = System.DateTime.Now.ToString();
                repfilename = objSchool.SchoolId.ToString()+"_"+fnmeparts[0].ToString() +"_"+ curdate + "." + fnmeparts[1].ToString();
                repfilename = repfilename.Replace('/', '_').Replace(':', '_');
                savepath = pathfilesave + repfilename;
                string[] acceptedFileTypes = new string[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".gif", ".png", ".txt", ".xls", ".xlsx" };
                for (int i = 0; i <= acceptedFileTypes.Length-1; i++)
                {
                    if (fileExtension == acceptedFileTypes[i])
                    {
                        checkaccessfileattach = 1;
                    }
                }
               
                if (checkaccessfileattach == 1)
                {
                    if (fileUploadattachments.PostedFile.ContentLength <= 1048576)
                    {
                        DataTable dtattachlist = (DataTable)Session["filelisttable"];
                      
                        if (dtattachlist != null)
                        {
                            if (dtattachlist.Rows.Count < 3)
                            {
                                bool contains = dtattachlist.AsEnumerable().Any(row => filename == row.Field<String>("FileName"));
                                if (contains)
                                {
                                    lblattacherror.Text = "file already attach";

                                }
                                else
                                {
                                    isvalidattach = true;
                                    fileUploadattachments.SaveAs(savepath);

                                }
                            }
                            else
                            {
                              
                                lblattacherror.Text = "maxium file count reached";
                            }
                        }
                        else
                        {
                            isvalidattach = true;
                            fileUploadattachments.SaveAs(savepath);
                        }

                    }
                    else
                    {
                      
                     
                        lblattacherror.Text = "Your browsing file not attached,Please select size below 1MB";
                    }
                }

                else
                {
              
                    lblattacherror.Text = "Your browsing file not supported for attachments";
                }
            }
            else
            {
        
               
                lblattacherror.Text = "Please browse file";
            }
            return isvalidattach;
        }
         private void getattachfilestogrid(string fname, string repsitoryfname)
         {
             //code modified :Dominic

             DataTable dtsession = new DataTable();
             DataRow dr = null;
             if (Session["filelisttable"] != null)
             {
                 dtsession = (DataTable)Session["filelisttable"];

             }
             else
             {
                 dtsession.Columns.Add("FileName", typeof(System.String));
                 dtsession.Columns.Add("RepositoryFileName", typeof(System.String));

             }

             dr = dtsession.NewRow();
             dr[0] = fname.ToString();
             dr[1] = repsitoryfname.ToString();
             dtsession.Rows.Add(dr);

             Session["filelisttable"] = dtsession;
             loadattachmentfiles();

             /*****************************************************************/
             //DataTable dt = new DataTable();
             //dt.Columns.Add("FileName", typeof(System.String));
             //dt.Columns.Add("RepositoryFileName", typeof(System.String));
             //DataRow dr = null;
             //dr = dt.NewRow();
             //dr[0] = fname.ToString();
             //dr[1] = repsitoryfname.ToString();
             //dt.Rows.Add(dr);
             //DataTable mergedt = new DataTable();
             //DataTable dtsession = new DataTable();
             //if (Session["filelisttable"] != null)
             //{
             //    dtsession = (DataTable)Session["filelisttable"];

             //    //if (dtsession.Rows.Count > 0)
             //    //{

             //    mergedt.Merge(dtsession);
             //    mergedt.Merge(dt);


             //    //}
             //}
             //else
             //{
             //    mergedt.Merge(dt);

             //}
             //Session["filelisttable"] = mergedt;
             //loadattachmentfiles();

         }
         private void loadattachmentfiles()
         {
             DataTable dtgrid = new DataTable();
             if (Session["filelisttable"] != null)
             {
                 pnlattachment.Visible = true;
                 dtgrid = (DataTable)Session["filelisttable"];
                 Grd_attachment.Visible = true;
                 Grd_attachment.DataSource = dtgrid;
                 Grd_attachment.DataBind();
                 Grd_attachment.Columns[2].Visible = false;
             }
             else
             {
                 Grd_attachment.DataSource = null;
                 Grd_attachment.DataBind();
                 pnlattachment.Visible = false;
                 Grd_attachment.Visible = false;
             }
         }
         protected void Grd_attachment_RowDeleting(object sender, GridViewDeleteEventArgs e)
         {
             try
             {
                 Grd_attachment.Columns[2].Visible = true;
                 string filename = Grd_attachment.DataKeys[e.RowIndex].Values["FileName"].ToString();
                 string filerep = Grd_attachment.DataKeys[e.RowIndex].Values["RepositoryFileName"].ToString();
                 string pathdelete = Server.MapPath("") + "\\" + "Email Attachments\\";
                 string filepathdelete = pathdelete + filerep;

                 if (File.Exists(filepathdelete))
                 {
                     File.Delete(filepathdelete);
                 }

                 DataTable dtdelete = new DataTable();
                 if (Session["filelisttable"] != null)
                 {
                     dtdelete = (DataTable)Session["filelisttable"];
                     dtdelete.Rows[e.RowIndex].Delete();
                 }
                 Session["filelisttable"] = dtdelete;
                 Grd_attachment.Columns[2].Visible = false;
                 loadattachmentfiles();
                 lblattacherror.Text = "" + filename + " Removed from Attachment";

             }
             catch (Exception eg)
             {
                 lblattacherror.Text = eg.Message;
             }

         }
        //Email attachment code for staff tab
         protected void Btnattachstaff_Click(object sender, EventArgs e)
         {
             string s_filename = "";
             string sRep_filename = "";

             try
             {
                 if (attachmentvalidationsstaff(out s_filename, out sRep_filename))
                 {

                     s_getattachfilestogrid(s_filename, sRep_filename);
                 }
             }
             catch (Exception ed)
             {
                 WC_MsgBox.ShowMssage(ed.Message);
             }
         }
         private bool attachmentvalidationsstaff(out string s_filenametosave, out string s_repfilename)
         {
             bool s_isvalidattach = false;
             int s_checkaccessfile = 0;
             s_filenametosave = "";
             s_repfilename = "";
             string s_savepath = "";
             string s_pathfilesave = Server.MapPath("") + "\\" + "Email Attachments\\";

             if (!Directory.Exists(s_pathfilesave))
             {
                 Directory.CreateDirectory(s_pathfilesave);
             }
             if (fileUploadstaff.HasFile)
             {
                 string s_filename = System.IO.Path.GetFileName(fileUploadstaff.FileName);
                 s_filenametosave = s_filename.ToString();
                 string s_fileExtension = Path.GetExtension(s_filename);
                 s_fileExtension = s_fileExtension.ToLower();
                 string[] s_fnmeparts = s_filename.Split('.');
                 string curdate = System.DateTime.Now.ToString();
                 s_repfilename = objSchool.SchoolId.ToString() + "_" + s_fnmeparts[0].ToString() + "_" + curdate + "." + s_fnmeparts[1].ToString();
                 s_repfilename = s_repfilename.Replace('/', '_').Replace(':', '_');
                 s_savepath = s_pathfilesave + s_repfilename;
                 string[] s_acceptedFileTypes = new string[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".gif", ".png", ".txt", ".xls", ".xlsx" };
                 for (int i = 0; i <= s_acceptedFileTypes.Length - 1; i++)
                 {
                     if (s_fileExtension == s_acceptedFileTypes[i])
                     {
                         s_checkaccessfile = 1;
                     }
                 }

                 if (s_checkaccessfile == 1)
                 {
                     if (fileUploadstaff.PostedFile.ContentLength <= 1048576)
                     {
                         DataTable s_dtattachlist = (DataTable)Session["filelisttablestaff"];

                         if (s_dtattachlist != null)
                         {
                             if (s_dtattachlist.Rows.Count < 3)
                             {
                                 bool contains = s_dtattachlist.AsEnumerable().Any(row => s_filename == row.Field<String>("FileName"));
                                 if (contains)
                                 {
                                     lblerrorstaff.Text = "file already attach";

                                 }
                                 else
                                 {
                                     s_isvalidattach = true;

                                     fileUploadstaff.SaveAs(s_savepath);

                                 }
                             }
                             else
                             {

                                 lblerrorstaff.Text = "maxium file count reached";
                             }
                         }
                         else
                         {
                             s_isvalidattach = true;
                             fileUploadstaff.SaveAs(s_savepath);
                         }

                     }
                     else
                     {

                         lblerrorstaff.Text = "Your browsing file not attached,Please select size below 1MB";
                     }
                 }

                 else
                 {

                     lblerrorstaff.Text = "Your browsing file not supported for attachments";
                 }
             }
             else
             {

                 lblerrorstaff.Text = "Please browse file";
             }
             return s_isvalidattach;
         }
         private void s_getattachfilestogrid(string fname, string repsitoryfname)
         {

             //code modified :Dominic

             DataTable dtsession = new DataTable();
             DataRow dr = null;
             if (Session["filelisttablestaff"] != null)
             {
                 dtsession = (DataTable)Session["filelisttablestaff"];

             }
             else
             {
                 dtsession.Columns.Add("FileName", typeof(System.String));
                 dtsession.Columns.Add("RepositoryFileName", typeof(System.String));

             }

             dr = dtsession.NewRow();
             dr[0] = fname.ToString();
             dr[1] = repsitoryfname.ToString();
             dtsession.Rows.Add(dr);

             Session["filelisttablestaff"] = dtsession;
             staffloadattachmentfiles();

/*************************************************************/


             //DataTable s_dt = new DataTable();
             //s_dt.Columns.Add("FileName", typeof(System.String));
             //s_dt.Columns.Add("RepositoryFileName", typeof(System.String));
             //DataRow s_dr = null;
             //s_dr = s_dt.NewRow();
             //s_dr[0] = fname.ToString();
             //s_dr[1] = repsitoryfname.ToString();
             //s_dt.Rows.Add(s_dr);
             //DataTable s_mergedt = new DataTable();
             //DataTable s_dtsession = new DataTable();
             //if (Session["filelisttablestaff"] != null)
             //{
             //    s_dtsession = (DataTable)Session["filelisttablestaff"];

             //    //if (dtsession.Rows.Count > 0)
             //    //{

             //    s_mergedt.Merge(s_dtsession);
             //    s_mergedt.Merge(s_dt);


             //    //}
             //}
             //else
             //{
             //    s_mergedt.Merge(s_dt);

             //}
             //Session["filelisttablestaff"] = s_mergedt;
             //staffloadattachmentfiles();

         }
         private void staffloadattachmentfiles()
         {
             DataTable s_dtgrid = new DataTable();
             if (Session["filelisttablestaff"] != null)
             {
                 pnlattachstaff.Visible = true;
                 s_dtgrid = (DataTable)Session["filelisttablestaff"];
                 Grdattachstaff.Visible = true;
                 Grdattachstaff.DataSource = s_dtgrid;
                 Grdattachstaff.DataBind();
                 Grdattachstaff.Columns[2].Visible = false;
             }
             else
             {
                 Grdattachstaff.DataSource = null;
                 Grdattachstaff.DataBind();
                 pnlattachstaff.Visible = false;
                 Grdattachstaff.Visible = false;
             }
         }
         protected void Grdattachstaff_RowDeleting(object sender, GridViewDeleteEventArgs e)
         {
             try
             {
                 Grdattachstaff.Columns[2].Visible = true;
                 string s_filename = Grdattachstaff.DataKeys[e.RowIndex].Values["FileName"].ToString();
                 string s_filerep = Grdattachstaff.DataKeys[e.RowIndex].Values["RepositoryFileName"].ToString();
                 string s_pathdelete = Server.MapPath("") + "\\" + "Email Attachments\\";
                 string s_filepathdelete = s_pathdelete + s_filerep;
                
                 if (File.Exists(s_filepathdelete))
                 {
                     File.Delete(s_filepathdelete);
                 }

                 DataTable s_dtdelete = new DataTable();
                 
                 if (Session["filelisttablestaff"] != null)
                 {
                     s_dtdelete = (DataTable)Session["filelisttablestaff"];
                     s_dtdelete.Rows[e.RowIndex].Delete();
                 }

                 Session["filelisttablestaff"] = s_dtdelete;
                 Grdattachstaff.Columns[2].Visible = false;
                 staffloadattachmentfiles();
                 lblerrorstaff.Text = "" + s_filename + " Removed from Attachment";

             }
             catch (Exception eg)
             {
                 lblerrorstaff.Text = eg.Message;
             }

         }
        
    }
}
