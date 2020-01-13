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

namespace WinEr
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private ClassOrganiser MyClassMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (Session["ClassId"] == null)
            {
                Response.Redirect("LoadClassDetails.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyClassMang = MyUser.GetClassObj();
            if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(17))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {


                if (!IsPostBack)
                {
                    LoadInitials();
                }
            }

        }

        private void LoadInitials()
        {
            string _MenuStr;
            _MenuStr = MyClassMang.GetClassMangSubMenuString(MyUser.UserRoleId, MyUser.SELECTEDMODE);
            this.SubClassMenu.InnerHtml = _MenuStr;
            AddStandardToDrpList(0);
            AddParentGrpToDrpList();
            AddClassTeacherToDrpList();
            FillData();
            AddSubjectsToList();

            LoadAllSubjectsToDropDown();
            AddAllStaffsToList();
        }

        private void LoadAllSubjectsToDropDown()
        {
            Drp_Subjects.Items.Clear();
            string sql = "SELECT tblsubjects.subject_name, tblclasssubmap.SubjectId from tblsubjects inner join tblclasssubmap on tblsubjects.Id= tblclasssubmap.SubjectId where tblclasssubmap.ClassId=" + int.Parse(Session["ClassId"].ToString());
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_Subjects.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Subjects Found", "-1");
                Drp_Subjects.Items.Add(li);
            }
        }

        private void AddAllStaffsToList()
        {
            Chk_AllStaffs.Items.Clear();
            Chk_ClassStaff.Items.Clear();
            string sql = "select tbluser.SurName, tblstaffdetails.UserId from tblstaffdetails inner join tbluser on tblstaffdetails.UserId= tbluser.Id inner join tblstaffsubjectmap on tblstaffdetails.UserId= tblstaffsubjectmap.StaffId where tbluser.Status=1 and tblstaffsubjectmap.SubjectId=" + Drp_Subjects.SelectedValue + " order by tbluser.SurName";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    if (MyClassMang.IsClassStaff(int.Parse(Session["ClassId"].ToString()), int.Parse(MyReader.GetValue(1).ToString()), int.Parse(Drp_Subjects.SelectedValue)))
                    {
                        Chk_ClassStaff.Items.Add(li);
                    }
                    else
                    {
                        Chk_AllStaffs.Items.Add(li);
                    }
                }
            }
        }

        private void AddClassTeacherToDrpList()
        {
            Drp_Classteacher.Items.Clear();
            Drp_Classteacher.Items.Add(new ListItem("NONE", "0"));
            string sql = "SELECT DISTINCT t.`Id`,t.`SurName` FROM tbluser t inner join tblrole r on r.`Id`=t.`RoleId` inner join tblgroupusermap g on t.`Id`=g.`UserId` where t.`Status`=1 AND r.`Type`='Staff' AND g.`GroupId` IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY t.`SurName`";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Classteacher.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No staff found", "-1");
                Drp_Classteacher.Items.Add(li);
            }
        }

        private void AddSubjectsToList()
        {
            ChkBox_AllsSub.Items.Clear();
            ChkBox_Classsubject.Items.Clear();
            string sql = "SELECT Id,subject_name FROM tblsubjects";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    if (MyClassMang.IsClassSubject(int.Parse(Session["ClassId"].ToString()), int.Parse(MyReader.GetValue(0).ToString())))
                    {
                        ChkBox_Classsubject.Items.Add(li);
                    }
                    else
                    {
                        ChkBox_AllsSub.Items.Add(li);
                    }
                }
            }
        }

        private void AddParentGrpToDrpList()
        {
            Drp_parent.Items.Clear();
            MydataSet = MyUser.MyAssociatedGroups();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_parent.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Groups", "-1");
                Drp_parent.Items.Add(li);
            }
            Drp_parent.SelectedIndex = 0;
        }

        private void AddStandardToDrpList(int _intex)
        {
            Drp_Stand.Items.Clear();

            string sql = "SELECT Id,Name FROM tblstandard";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Stand.Items.Add(li);
                }
                Drp_Stand.SelectedIndex = _intex;
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void Drp_ClassName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FillData()
        {
            bool valid = false;
            int ClassTeacher = -1;
            Txt_ClassName.Text = MyClassMang.GetClassname(int.Parse(Session["ClassId"].ToString()));
            string sql = "SELECT Standard,ParentGroupID,TotalSeats,ClassTeacher FROM tblclass where Id=" + int.Parse(Session["ClassId"].ToString());
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_Stand.SelectedValue = MyReader.GetValue(0).ToString();
                Drp_parent.SelectedValue = MyReader.GetValue(1).ToString();
                txt_TotalSeats.Text = (int.Parse(MyReader.GetValue(2).ToString())).ToString();
                valid = int.TryParse(MyReader.GetValue(3).ToString(), out ClassTeacher);
                Drp_Classteacher.SelectedValue = ClassTeacher.ToString();
            }
            MyReader.Close();
        }

        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ChkBox_AllsSub.Items.Count; i++)
            {
                if (ChkBox_AllsSub.Items[i].Selected)
                {
                    MyClassMang.AddSujectToClass(int.Parse(ChkBox_AllsSub.Items[i].Value.ToString()), int.Parse(Session["ClassId"].ToString()));
                    ChkBox_AllsSub.Items[i].Selected = false;
                    ChkBox_Classsubject.Items.Add(ChkBox_AllsSub.Items[i]);
                    ChkBox_AllsSub.Items.Remove(ChkBox_AllsSub.Items[i]);
                    i--;
                }
            }
            LoadAllSubjectsToDropDown();
            AddAllStaffsToList();
        }

        protected void Btn_Remove_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ChkBox_Classsubject.Items.Count; i++)
            {
                if (ChkBox_Classsubject.Items[i].Selected)
                {
                    if (!AlreadyMappedWithExamSchedule(int.Parse(ChkBox_Classsubject.Items[i].Value.ToString())))
                    {
                        MyClassMang.RemoveSubject(int.Parse(ChkBox_Classsubject.Items[i].Value.ToString()), int.Parse(Session["ClassId"].ToString()));
                        ChkBox_Classsubject.Items[i].Selected = false;
                        ChkBox_AllsSub.Items.Add(ChkBox_Classsubject.Items[i]);
                        ChkBox_Classsubject.Items.Remove(ChkBox_Classsubject.Items[i]);
                        i--;
                    }
                    else
                    {
                        Lbl_msg.Text = "Exam is already Scheduled for this Subject.Can not Delete";
                        MPE_MessageBox.Show();
                    }
                }
            }
            LoadAllSubjectsToDropDown();
            AddAllStaffsToList();
        }

        private bool AlreadyMappedWithExamSchedule(int _SubjectId)
        {
            bool _Exist = false;
            string sql = "select tblclassexamsubmap.ClassExamId from tblclassexamsubmap inner join tblclassexam on tblclassexamsubmap.ClassExamId= tblclassexam.Id where tblclassexamsubmap.SubId=" + _SubjectId + " and tblclassexam.ClassId=" + int.Parse(Session["ClassId"].ToString());
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _Exist = true;
            }
            return _Exist;
        }

        protected void Btn_UpdateClass_Click(object sender, EventArgs e)
        {
            int _TotalStudents = 0;

            string sql = "select count(tblstudentclassmap.StudentId) from tblstudentclassmap where tblstudentclassmap.ClassId=" + int.Parse(Session["ClassId"].ToString());
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out _TotalStudents);
            }

            if (_TotalStudents > int.Parse(txt_TotalSeats.Text))
            {
                Lbl_msg.Text = "Class already contains " + _TotalStudents.ToString() + " Students.Total seats should be greater than total no of students.";
                MPE_MessageBox.Show();
            }
            else
                if (Txt_ClassName.Text != "")
                {
                    sql = "select tblclass.ClassName from tblclass where tblclass.Id!=" + int.Parse(Session["ClassId"].ToString()) + " and tblclass.ClassName='" + Txt_ClassName.Text + "'";
                    MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        Lbl_msg.Text = "Class Name already Exists..";
                        MPE_MessageBox.Show();
                    }
                    else
                    {
                        MyClassMang.UpdateClass(int.Parse(Session["ClassId"].ToString()), Txt_ClassName.Text, Drp_Stand.SelectedValue.ToString(), int.Parse(Drp_parent.SelectedValue), int.Parse(txt_TotalSeats.Text), int.Parse(Drp_Classteacher.SelectedValue));
                        // MyClassMang.UpdateClass(int.Parse(Session["ClassId"].ToString()), Drp_Stand.SelectedValue.ToString(), int.Parse(Drp_parent.SelectedValue.ToString()));//,int.Parse(txt_TotalSeats.Text));
                        //AddClassSubjects(int.Parse(Session["ClassId"].ToString()));                    
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Update Class Details", " Some Details Of the  Class " + Txt_ClassName.Text + " is changed", 1);
                        MyUser.ClearAssociatedClass();
                        LoadInitials();
                        Lbl_msg.Text = "Class is updated";
                        this.MPE_MessageBox.Show();
                    }
                }
        }

        private void AddClassSubjects(int _classid)
        {
            for (int i = 0; i < ChkBox_Classsubject.Items.Count; i++)
            {
                MyClassMang.AddSujectToClass(int.Parse(ChkBox_Classsubject.Items[i].Value.ToString()), _classid);
            }
        }

        protected void Btn_Finish_Click(object sender, EventArgs e)
        {
            Response.Redirect("LoadClassDetails.aspx");
        }

        protected void Btn_Delete_Click(object sender, EventArgs e)
        {
            int _mode;
            bool IsRegular = false;
            if (MyClassMang.CanDelete(int.Parse(Session["ClassId"].ToString()), MyUser.CurrentBatchId, out _mode, out IsRegular))
            {
                MyClassMang.DeletClass(int.Parse(Session["ClassId"].ToString()), _mode);
                MyUser.ClearAssociatedClass();
                Session["ClassId"] = null;
                Lbl_lastmsg.Text = "Class is deleted...";
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Delete Class", "The Class " + Txt_ClassName.Text + " is removed from the software", 1);
                MPE_lastmsg.Show();
                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "clearcookies();", true);
            }
            else
            {
                if (IsRegular)
                {
                    Lbl_altmessage.Text = "Please remove the students in the class before deletion";
                }
                else
                {
                    Lbl_altmessage.Text = "Please remove the registered students in the class before deletion";
                }

                MPE_errmessage.Show();
            }
        }

        protected void Btn_AddStaff_Click(object sender, EventArgs e)
        {
            string subjectId = Drp_Subjects.SelectedValue;
            string subjectname = Drp_Subjects.SelectedItem.ToString();
            string message = "";
            DataSet Staff_Ds = new DataSet();
            DataRow _dr;
            DataTable _dt;
            Staff_Ds.Tables.Add("Staff");
            _dt = Staff_Ds.Tables["Staff"];
            _dt.Columns.Add("StaffId");
            _dt.Columns.Add("StaffName");

            for (int i = 0; i < Chk_AllStaffs.Items.Count; i++)
            {
                if (Chk_AllStaffs.Items[i].Selected)
                {

                    _dr = _dt.NewRow();
                    _dr["StaffId"] = Chk_AllStaffs.Items[i].Value;
                    _dr["StaffName"] = Chk_AllStaffs.Items[i];
                    Staff_Ds.Tables["Staff"].Rows.Add(_dr);

                    Chk_AllStaffs.Items[i].Selected = false;
                    i--;
                }
            }
            if (Staff_Ds != null && Staff_Ds.Tables[0].Rows.Count > 0)
            {
                if (Staff_Ds.Tables[0].Rows.Count > 2)
                {
                    message = "You can assign maximum two staffs only";
                }
                else
                {
                    if (Chk_ClassStaff.Items.Count == 2)
                    {
                        message = "Two staffs are already assigned to this subject";
                    }
                    else
                    {

                        if (Staff_Ds.Tables[0].Rows.Count == 2)
                        {
                            if (Chk_ClassStaff.Items.Count > 0)
                            {
                                message = "You can assign maximum two staffs only,select only one staff";
                            }
                            else
                            {
                                MyClassMang.DeleteStaffIfAlreadyAssigened(int.Parse(Session["ClassId"].ToString()), int.Parse(Drp_Subjects.SelectedValue));
                                foreach (DataRow dr in Staff_Ds.Tables[0].Rows)
                                {
                                    MyClassMang.AddStaffToClassStaffMapWithSubject(int.Parse(dr["StaffId"].ToString()), int.Parse(Session["ClassId"].ToString()), int.Parse(Drp_Subjects.SelectedValue));

                                    if (!AlreadyPresentInStaffSubjectMapTable(int.Parse(dr["StaffId"].ToString()), int.Parse(Drp_Subjects.SelectedValue)))
                                    {
                                        MyClassMang.AddStaffToStaffSubjectMapTable(int.Parse(dr["StaffId"].ToString()), int.Parse(Drp_Subjects.SelectedValue));
                                    }
                                }
                            }

                        }
                        if (Staff_Ds.Tables[0].Rows.Count == 1)
                        {
                            if (Chk_ClassStaff.Items.Count == 1)
                            {
                                foreach (DataRow dr in Staff_Ds.Tables[0].Rows)
                                {
                                    MyClassMang.UpdateStaffToClassStaffMapWithSubject(int.Parse(dr["StaffId"].ToString()), int.Parse(Session["ClassId"].ToString()), int.Parse(Drp_Subjects.SelectedValue));
                                    if (!AlreadyPresentInStaffSubjectMapTable(int.Parse(dr["StaffId"].ToString()), int.Parse(Drp_Subjects.SelectedValue)))
                                    {
                                        MyClassMang.AddStaffToStaffSubjectMapTable(int.Parse(dr["StaffId"].ToString()), int.Parse(Drp_Subjects.SelectedValue));
                                    }

                                    // string s = dr["StaffName"].ToString();

                                }
                            }
                            else
                            {
                                foreach (DataRow dr in Staff_Ds.Tables[0].Rows)
                                {
                                    MyClassMang.DeleteStaffIfAlreadyAssigened(int.Parse(Session["ClassId"].ToString()), int.Parse(Drp_Subjects.SelectedValue));

                                    MyClassMang.AddStaffToClassStaffMapWithSubject(int.Parse(dr["StaffId"].ToString()), int.Parse(Session["ClassId"].ToString()), int.Parse(Drp_Subjects.SelectedValue));
                                    if (!AlreadyPresentInStaffSubjectMapTable(int.Parse(dr["StaffId"].ToString()), int.Parse(Drp_Subjects.SelectedValue)))
                                    {
                                        MyClassMang.AddStaffToStaffSubjectMapTable(int.Parse(dr["StaffId"].ToString()), int.Parse(Drp_Subjects.SelectedValue));
                                    }
                                    // string s = dr["StaffName"].ToString();

                                }
                            }


                            if (Chk_ClassStaff.Items.Count == 0)
                            {
                                MyClassMang.AddStaffToClassStaffMapWithSubject(-1, int.Parse(Session["ClassId"].ToString()), int.Parse(Drp_Subjects.SelectedValue));
                            }
                        }

                    }

                    foreach (DataRow dr in Staff_Ds.Tables[0].Rows)
                    {
                        if (message == "")
                        {
                            Chk_ClassStaff.Items.Add(dr["StaffName"].ToString());
                            Chk_AllStaffs.Items.Remove(dr["StaffName"].ToString());
                        }
                        string sql = "select tblclass.classname from tblclass where tblclass.id=" + int.Parse(Session["ClassId"].ToString()) + "";
                        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
                        if (MyReader.HasRows)
                        {

                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Assign staff to class", "Staff named " + dr["StaffName"].ToString() + " is assigned to class " + MyReader.GetValue(0).ToString() + " for the subject " + subjectname + "", 1);
                        }

                    }
                }
                if (message == "")
                {
                    Drp_Subjects.SelectedValue = subjectId;
                    AddAllStaffsToList();
                }
                //ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "PageRelorad();", true);
                Lbl_StaffErr.Text = message;


            }
            else
            {
                Lbl_StaffErr.Text = "select any staff";
            }
        }

        private void GetStaffCountForEachSubject(int p, int p_2)
        {
            throw new NotImplementedException();
        }

        private bool AlreadyPresentInStaffSubjectMapTable(int _StaffId, int _SubjectId)
        {
            bool _IsActive = false;
            string sql = "SELECT tblstaffsubjectmap.StaffId from tblstaffsubjectmap where tblstaffsubjectmap.StaffId=" + _StaffId + " and tblstaffsubjectmap.SubjectId=" + _SubjectId;
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _IsActive = true;
            }
            return _IsActive;
        }

        private bool AlreadyPresentInTable(int _StaffId, int _ClassId)
        {
            bool _IsActive = false;
            string sql = "SELECT tbltime_classstaffmap.StaffId from tbltime_classstaffmap where tbltime_classstaffmap.StaffId=" + _StaffId + " and tbltime_classstaffmap.ClassId=" + _ClassId;
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _IsActive = true;
            }
            return _IsActive;
        }

        protected void Btn_RemoveStaff_Click(object sender, EventArgs e)
        {
            string subjectId = Drp_Subjects.SelectedValue;
            string subjectname = Drp_Subjects.SelectedItem.ToString();
            DataSet Staff_Ds = new DataSet();
            DataRow _dr;
            DataTable _dt;
            Staff_Ds.Tables.Add("Staff");
            _dt = Staff_Ds.Tables["Staff"];
            _dt.Columns.Add("StaffId");
            _dt.Columns.Add("StaffName");

            for (int i = 0; i < Chk_ClassStaff.Items.Count; i++)
            {
                if (Chk_ClassStaff.Items[i].Selected)
                {
                    _dr = _dt.NewRow();
                    _dr["StaffId"] = Chk_ClassStaff.Items[i].Value;
                    _dr["StaffName"] = Chk_ClassStaff.Items[i];
                    Staff_Ds.Tables["Staff"].Rows.Add(_dr);

                    Chk_ClassStaff.Items[i].Selected = false;
                    i--;

                }
            }

            if (Staff_Ds != null && Staff_Ds.Tables[0].Rows.Count > 0)
            {
                if (Staff_Ds.Tables[0].Rows.Count == 1)
                {
                    foreach (DataRow dr in Staff_Ds.Tables[0].Rows)
                    {
                        MyClassMang.UpdateRemovedStaff(int.Parse(dr["StaffId"].ToString()), int.Parse(Session["ClassId"].ToString()), int.Parse(Drp_Subjects.SelectedValue));
                    }
                }
                else if (Staff_Ds.Tables[0].Rows.Count == 2)
                {
                    foreach (DataRow dr in Staff_Ds.Tables[0].Rows)
                    {
                        MyClassMang.RemoveStaffFromClassStaffMapWithSubject(int.Parse(dr["StaffId"].ToString()), int.Parse(Session["ClassId"].ToString()), int.Parse(Drp_Subjects.SelectedValue));
                    }
                }
            }

            foreach (DataRow dr in Staff_Ds.Tables[0].Rows)
            {
                if (MyUser.HaveModule(25))
                {
                    RemoveFromClassStaffMapForTimeTable(int.Parse(dr["StaffId"].ToString()), int.Parse(Drp_Subjects.SelectedValue), int.Parse(Session["ClassId"].ToString()));
                }
                Chk_AllStaffs.Items.Add(dr["StaffName"].ToString());
                Chk_ClassStaff.Items.Remove(dr["StaffName"].ToString());
                string sql = "select tblclass.classname from tblclass where tblclass.id=" + int.Parse(Session["ClassId"].ToString()) + "";
                MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {

                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Remove staff from class", "Staff named " + dr["StaffName"].ToString() + " is removed from class " + MyReader.GetValue(0).ToString() + "" + subjectname + "", 1);
                }
            }
            Drp_Subjects.SelectedValue = subjectId;
            AddAllStaffsToList();
            //ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "PageRelorad();", true);
            // MyClassMang.RemoveStaffFomStaffSubjectMapTable(int.Parse(Chk_ClassStaff.Items[i].Value.ToString()), int.Parse(Drp_Subjects.SelectedValue));

            //Chk_ClassStaff.Items[i].Selected = false;           
            //i--; 
        }

        private void RemoveFromClassStaffMapForTimeTable(int _StaffId, int SubjectId, int _ClassId)
        {
            string sql = "DELETE FROM tbltime_classsubjectstaff WHERE tbltime_classsubjectstaff.ClassSubjectId in( SELECT tblclasssubmap.Id FROM tblclasssubmap WHERE tblclasssubmap.SubjectId=" + SubjectId + " AND tblclasssubmap.ClassId=" + _ClassId + ") AND tbltime_classsubjectstaff.StaffId=" + _StaffId;
            MyClassMang.m_MysqlDb.ExecuteQuery(sql);

        }

        protected void Drp_Subjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            Lbl_StaffErr.Text = "";
            this.Tabs.ActiveTabIndex = 2;
            AddAllStaffsToList();
        }

    }
}
