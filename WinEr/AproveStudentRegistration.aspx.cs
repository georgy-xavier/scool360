using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using System.Configuration;
using System.Net.Mail;
using System.Collections;
using WinBase;


namespace WinEr{
   
    public partial class ApproveStudentRegistration : System.Web.UI.Page
    {

        private KnowinUser MyUser;
        private StudentManagerClass MyStudMang;
        private Incident MyIncedent;
        private FeeManage MyFeeMang;
        private DataSet MyDataSet = null;
        private OdbcDataReader MyReader = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
           
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MyIncedent = MyUser.GetIncedentObj();
            MyFeeMang = MyUser.GetFeeObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (MyIncedent == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (MyFeeMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(93))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {                   
                    loadstudentgrid();
                }
            }
        }

        private void loadstudentgrid()
        {
            string sql = "select tblstudent.Id, tblstudent.StudentName, tblstudent.Sex, tblstudent.GardianName, tblstudent.AdmitionNo, tblstudtype.TypeName, tblreligion.Religion, tblbatch.BatchName, tblclass.ClassName,tblclass.Id as ClassId, tblstudent.OfficePhNo, tblstudent.TempStudentId from tblstudent inner join tblstudtype on tblstudtype.Id = tblstudent.StudTypeId inner join tblreligion  on tblstudent.Religion = tblreligion.Id  inner join tblbatch  on tblstudent.JoinBatch = tblbatch.Id inner join tblstudentclassmap on tblstudent.Id = tblstudentclassmap.StudentId  inner join tblclass on tblclass.Id = tblstudentclassmap.ClassId where tblstudent.Status=2";
            MyDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyDataSet.Tables[0].Rows.Count > 0)
            {
               
                
                inputtop.Visible = true;
                Grd_StudentRegistration.Columns[1].Visible = true;
                Grd_StudentRegistration.Columns[10].Visible = true;
                Grd_StudentRegistration.Columns[11].Visible = true;
                Grd_StudentRegistration.Columns[12].Visible = true;
                Grd_StudentRegistration.DataSource = MyDataSet;
                Grd_StudentRegistration.DataBind();
                Grd_StudentRegistration.Columns[1].Visible = false;
                Grd_StudentRegistration.Columns[10].Visible = false;
                Grd_StudentRegistration.Columns[11].Visible = false;
                Grd_StudentRegistration.Columns[12].Visible = false;
            }
            else
            {
                inputtop.Visible = false;
                lblerror.Text = "No more students need approval";
                lblmessage.Text = "";
                Grd_StudentRegistration.DataSource = null;
                Grd_StudentRegistration.DataBind(); 
            }
        }

        protected void Lnk_SelectStudents_Click(object sender, EventArgs e)
        {
           
            lblerror.Text = "";
            lblmessage.Text = "";

            if (Lnk_SelectStudents.Text == "Select All")
            {
                foreach (GridViewRow gv in Grd_StudentRegistration.Rows)
                {
                    CheckBox cb = (CheckBox)gv.FindControl("Chk_student");
                    cb.Checked = true;
                }
                Lnk_SelectStudents.Text = "None";
            }
            else
            {
                foreach (GridViewRow gv in Grd_StudentRegistration.Rows)
                {
                    CheckBox cb = (CheckBox)gv.FindControl("Chk_student");
                    cb.Checked = false;
                }
                Lnk_SelectStudents.Text = "Select All";
            }
        }

        protected void Btn_Approve_Click(object sender, EventArgs e)
        {

            bool _success = true;
            int flag = 0;
            Grd_StudentRegistration.Columns[1].Visible = true;
            int _studentid = 0;
            int _classId;
            foreach (GridViewRow gv in Grd_StudentRegistration.Rows)
            {
                CheckBox cb = (CheckBox)gv.FindControl("Chk_student");
                if (cb.Checked)
                {
                    flag = 1;
                    try
                    {
                        MyStudMang.CreateTansationDb();
                        _studentid = int.Parse(gv.Cells[1].Text);
                        _classId = int.Parse(gv.Cells[10].Text);
                        ApprovethisStudentRegistration(_studentid);
                        string _MobileNumber = gv.Cells[11].Text.Replace("&nbsp;", "").Trim();
                        string _StudentName = gv.Cells[2].Text;
                        string _TemperoryId = gv.Cells[12].Text.Trim();
                        int _TempStudentId = 0;
                        if (_TemperoryId != "0")
                        {
                            _TempStudentId = MyStudMang.GetTempStudentIdFromTempIDString(_TemperoryId);
                        }
                        MyStudMang.ScheduleRollNumber(_classId, MyUser.CurrentBatchId, _studentid);

                        if (_TempStudentId > 0)
                        {
                            MyStudMang.UpdateTempStudentStatus(_TempStudentId);
                        }
                        //_continue = UpDateCoustomFields(_userId);

                        if (_MobileNumber != "")
                        {
                            MyStudMang.InsertParentMobileNumberIntoSMSParenstsList(_studentid, _MobileNumber,"");
                            //InsertParentMobileNumberIntoSMSParenstsList();
                        }
                        string Email = "";
                        if (GetEmailIdOfStudent(_studentid, out Email)&& Email !="")
                        {
                            MyStudMang.InsertParentEmailIdIntoEmailParenstsList(_studentid, Email);
                        }
                        if (_TemperoryId != "0")
                            MyStudMang.UpdateJoiningFeeId(_TemperoryId, _studentid);

                        MyStudMang.EndSucessTansationDb();
                        MyIncedent.CreateApprovedIncedent("Student Created", "Student enrollment of " + gv.Cells[2].Text + " is approved on " + General.GerFormatedDatVal(DateTime.Now) + " by " + MyUser.UserName, General.GerFormatedDatVal(DateTime.Now), 1, MyUser.UserId, "student", _studentid, 1, 0, MyUser.CurrentBatchId, _classId);
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Student Creation Approved", "Creation of the student " + gv.Cells[2].Text + " is approved", 1);

                        if ((MyStudMang.IsQuickSchedule()) && (MyStudMang.FeeScheduledForTheClass(_classId, MyUser.CurrentBatchId)))
                        {
                            SheduleFeeForTheStudent(_studentid, _classId, _StudentName);
                        }
                        if (MyStudMang.BookScheduledToClass(_classId, MyUser.CurrentBatchId))
                        {
                            MyStudMang.ScheduleBookToNewStudent(_classId, _studentid, MyUser.CurrentBatchId);
                           
                        }
                    }
                    catch
                    {
                        _success = false;
                        lblmessage.Text = "unable to approve the student " + gv.Cells[2].Text + ".Please try again";
                        MyStudMang.EndFailTansationDb();
                    }
                }
            }

            Grd_StudentRegistration.Columns[1].Visible = false;


            if (flag == 0)
            {
                lblerror.Text = "Please Select A Student and try again";
                lblmessage.Text = "";
            }
            else if (!_success)
            {
                loadstudentgrid();
            }
            else
            {
                loadstudentgrid();
                lblmessage.Text = "The Selected Students Requests are appproved";
                lblerror.Text = "";

            }

        }

        private bool GetEmailIdOfStudent(int _studentid,out string Email)
        {
            bool _valid = false;
            Email = "";
            OdbcDataReader EmailReader = null;
            string sql = "";
            sql = "select tblstudent.Email from tblstudent where Id=" + _studentid + "";
            EmailReader = MyStudMang.m_TransationDb.ExecuteQuery(sql);
            if (EmailReader.HasRows)
            {
                Email = EmailReader.GetValue(0).ToString();
                _valid = true;
            }
            return _valid;
        }

        private bool SheduleFeeForTheStudent(int _StudentId, int _classid, string _StudentName)
        {
            bool _valid = true;
            try
            {
                int NextBatch = MyUser.CurrentBatchId + 1;
                string sql = "select tblfeeschedule.Id,tblfeeschedule.FeeId, tblfeeaccount.AccountName, tblfeeschedule.Amount from tblfeeschedule inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblfeeasso on tblfeeasso.Id = tblfeeaccount.AssociatedId inner join tblbatch on tblbatch.Id = tblfeeschedule.BatchId where (tblfeeschedule.BatchId=" + MyUser.CurrentBatchId + " or tblfeeschedule.BatchId=" + NextBatch + " ) and tblfeeschedule.ClassId=" + _classid + " and tblfeeasso.Name='Class'";
                MyDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (MyDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                    {
                        if (MyFeeMang.CheckForRuleApplicableToClassAndFee1(_classid,int.Parse(dr[1].ToString())))
                        {
                            if (MyFeeMang.CheckRuleIsApplicabletoThisStudent(int.Parse(dr[1].ToString()), _classid, double.Parse(dr[3].ToString()), _StudentId, MyUser.CurrentBatchId, int.Parse(dr[0].ToString())))
                            {
                                MyFeeMang.m_DbLog.LogToDb(MyUser.UserName, "Scheduled Fee", "Scheduled " + dr[2].ToString() + " Fee to Student Named " + _StudentName.ToUpper(), 1);
                            }
                        }
                        else
                        {
                            MyFeeMang.ScheduleStudFee(_StudentId, int.Parse(dr[0].ToString()), double.Parse(dr[3].ToString()), "Scheduled");
                            MyFeeMang.m_DbLog.LogToDb(MyUser.UserName, "Scheduled Fee", "Scheduled " + dr[2].ToString() + " Fee to Student Named " + _StudentName.ToUpper(), 1);
                        }
                    }
                }                
            }
            catch
            {
                _valid = false;
            }
            return _valid;
        }

        private void ApprovethisStudentRegistration(int _studentid)
        {
            string sql = "update tblstudent set tblstudent.Status=1 where tblstudent.Id=" + _studentid;
            MyStudMang.m_TransationDb.ExecuteQuery(sql);
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Approve Student Registration", "Verify the details of Student " + MyUser.m_DbLog.GetStudentName(_studentid, MyStudMang.m_TransationDb) + " and approve the registration of the student", 1, MyStudMang.m_TransationDb);

        }

        protected void Btn_Reject_Click(object sender, EventArgs e)
        {
            int flag = 0;
            foreach (GridViewRow gv in Grd_StudentRegistration.Rows)
            {
                CheckBox cb = (CheckBox)gv.FindControl("Chk_student");
                if (cb.Checked)
                {
                    flag = 1;
                    break;
                }
            }

            if (flag == 0)
            {
                lblerror.Text = "Please select atleast one Student and try again";
                lblmessage.Text = "";
            }
            else
            {
                txt_RejectReason.Text = "";
                MPE_RejectComments.Show();
            }            
        }        

        protected void Grd_StudentRegistration_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSelectedStudentDetails();
            MPE_StudentDetails.Show();
        }

        private void LoadSelectedStudentDetails()
        {
            int _StudentId = int.Parse(Grd_StudentRegistration.SelectedRow.Cells[1].Text.Trim());

            string sql = "SELECT tblstudent.StudentName, tblstudent.AdmitionNo, tblstudent.DOB, tblstudent.Address, tblcast.castname, tblstudent.DateofJoining, tblstudent.Email, tblstudent.Location, tblstudent.Pin, tblstudent.State, tblstudent.Nationality, tblstudent.FatherOccupation, tbllanguage.Language, tblstudent.ResidencePhNo, tblstudent.OfficePhNo, tblstudent.UseBus, tblstudent.UseHostel from tblstudent inner join tblcast on tblstudent.Cast= tblcast.Id inner join tbllanguage on tblstudent.MotherTongue= tbllanguage.Id where tblstudent.Id=" + _StudentId;
            MyReader = MyIncedent.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                lbl_Name.Text = MyReader.GetValue(0).ToString();
                lbl_Admission.Text = MyReader.GetValue(1).ToString();
                lbl_DOB.Text = MyUser.GerFormatedDatVal(DateTime.Parse(MyReader.GetValue(2).ToString()));
                lbl_Address.Text = MyReader.GetValue(3).ToString();
                lbl_Caste.Text = MyReader.GetValue(4).ToString();
                lbl_JoinDate.Text = MyUser.GerFormatedDatVal(DateTime.Parse(MyReader.GetValue(5).ToString()));
                lbl_Email.Text = MyReader.GetValue(6).ToString();
                lbl_Location.Text = MyReader.GetValue(7).ToString();
                lbl_Pin.Text = MyReader.GetValue(8).ToString();
                lbl_State.Text = MyReader.GetValue(9).ToString();
                lbl_Nation.Text = MyReader.GetValue(10).ToString();
                lbl_Father.Text = MyReader.GetValue(11).ToString();
                lbl_MotherTongue.Text = MyReader.GetValue(12).ToString();
                lbl_Phone.Text = MyReader.GetValue(13).ToString();
                lbl_Mobile.Text = MyReader.GetValue(14).ToString();
                if (MyReader.GetValue(15).ToString() == "0")
                {
                    lbl_Bus.Text = "No";
                }
                else
                {
                    lbl_Bus.Text = "Yes";
                }
                if (MyReader.GetValue(16).ToString() == "0")
                {
                    lbl_Hostel.Text = "No";
                }
                else
                {
                    lbl_Hostel.Text = "Yes";
                }
            }            
        }

        protected void Btn_RejectSave_Click(object sender, EventArgs e)
        {
            try
            {
                MyStudMang.CreateTansationDb();

                Grd_StudentRegistration.Columns[1].Visible = true;
                int _studentid = 0;
                string _Comments = txt_RejectReason.Text;
                foreach (GridViewRow gv in Grd_StudentRegistration.Rows)
                {                    
                    CheckBox cb = (CheckBox)gv.FindControl("Chk_student");
                    if (cb.Checked)
                    {
                        _studentid = int.Parse(gv.Cells[1].Text.Trim());
                        string _TemperoryId = gv.Cells[12].Text.Trim();
                        int _TempStudentId = 0;
                        if (_TemperoryId != "0")
                        {
                            _TempStudentId = MyStudMang.GetTempStudentIdFromTempIDString(_TemperoryId);
                        }

                        RejectthisStudentRegistration(_studentid, _TempStudentId, _Comments);
                    }
                }

                Grd_StudentRegistration.Columns[1].Visible = false;

                MyStudMang.EndSucessTansationDb();
                lblmessage.Text = "The selected Students' registrations are rejected";

                loadstudentgrid();
            }
            catch
            {
                lblmessage.Text = "The selected Students are not rejected.Please try again";
                MyStudMang.EndFailTansationDb();
            }
        }

        private void RejectthisStudentRegistration(int _studentid,int _TempStudentId, string _Comments)
        {
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Reject Student Registration", "Verify the details of Student " + MyUser.m_DbLog.GetStudentName(_studentid, MyStudMang.m_TransationDb) + " and reject the registration of the student", 1, MyStudMang.m_TransationDb);

            string sql = "insert into tblrejectedstudent (Id,StudentName,GardianName,AdmitionNo,DOB,Sex,Address,BloodGroup,Religion,Cast,DateofJoining,DateOfLeaving,`Status`,Email,Location,Pin,  State,  Nationality,  FatherEduQuali,  MothersName,  MotherEduQuali,  FatherOccupation,  AnnualIncome,  MotherTongue,  ResidencePhNo,  OfficePhNo,  1stLanguage,  NumberofBrothers,  NumberOfSysters,  JoinBatch,  CreationTime,  Addresspresent,  StudTypeId,  AdmissionTypeId,  CreatedUserName,  CanceledUser,  TempStudentId,  UseBus,  UseHostel,  LastClassId,  RollNo,StudentId) select Id,StudentName,GardianName,AdmitionNo,DOB,Sex,Address,BloodGroup,Religion,Cast,DateofJoining,DateOfLeaving,`Status`,Email,Location,Pin,  State,  Nationality,  FatherEduQuali,  MothersName,  MotherEduQuali,  FatherOccupation,  AnnualIncome,  MotherTongue,  ResidencePhNo,  OfficePhNo,  1stLanguage,  NumberofBrothers,  NumberOfSysters,  JoinBatch,  CreationTime,  Addresspresent,  StudTypeId,  AdmissionTypeId,  CreatedUserName,  CanceledUser,  TempStudentId,  UseBus,  UseHostel,  LastClassId,  RollNo,StudentId from tblstudent where tblstudent.Id=" + _studentid;
            MyStudMang.m_TransationDb.ExecuteQuery(sql);

            sql = "update tblrejectedstudent set tblrejectedstudent.Comment='" + _Comments + "' where tblrejectedstudent.Id=" + _studentid;
            MyStudMang.m_TransationDb.ExecuteQuery(sql);

            sql = "delete from tblstudent where tblstudent.Id=" + _studentid;
            MyStudMang.m_TransationDb.ExecuteQuery(sql);

            sql = "delete from tblstudentclassmap where tblstudentclassmap.StudentId=" + _studentid;
            MyStudMang.m_TransationDb.ExecuteQuery(sql);

            sql = "delete from tblfeestudent where tblfeestudent.StudId=" + _studentid;
            MyStudMang.m_TransationDb.ExecuteQuery(sql);


            sql = "update tbltempstdent set Status=1 where Id=" + _TempStudentId;
            MyStudMang.m_TransationDb.ExecuteQuery(sql);

        }

        protected void Lnk_RejectionList_Click(object sender, EventArgs e)
        {
            Response.Redirect("RejectionList.aspx");
        }
    }
}
