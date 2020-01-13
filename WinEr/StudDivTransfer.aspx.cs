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
using WinBase;

namespace WinEr
{
    public partial class StudDivTransfer : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private ClassOrganiser MyClassMang;
        private Attendance MyAttdMang;
        private KnowinUser MyUser;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (Session["StudId"] == null)
            {
                Response.Redirect("SearchStudent.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MyAttdMang = MyUser.GetAttendancetObj();
            MyClassMang = MyUser.GetClassObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MyAttdMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(892))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {


                if (!IsPostBack)
                {
                   // string _MenuStr;
                  //  _MenuStr = MyStudMang.GetSubStudentMangMenuString(MyUser.UserRoleId, int.Parse(Session["StudType"].ToString()));
                   // this.SubStudentMenu.InnerHtml = _MenuStr;

                    LoadInitailDetails();
                    //some initlization

                }
                Chk_Class.Enabled = false;
                if (MyUser.HaveActionRignt(920))
                {
                    Chk_Class.Enabled = true;
                }



            }
        }

        //private void loadstudstrip()
        //{
        //    string _Studstrip = MyStudMang.ToStripString(int.Parse(Session["StudId"].ToString()), "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StudId"].ToString()) + "&type=StudentImage", int.Parse(Session["StudType"].ToString()));
        //    this.StudentTopStrip.InnerHtml = _Studstrip;
        //}

        private void LoadInitailDetails()
        {
            Panel_Select.Visible = true;
            //loadstudstrip();
            loadDrp_Class();
        }

        private void loadDrp_Class()
        {
            Drp_NewClass.Items.Clear();

            string subsql = "";
            if (Chk_Class.Checked)
            {
                subsql = subsql + " AND Standard=" + MyStudMang.GetStandard(Session["StudId"].ToString());
            }

            string sql = "SELECT Id,ClassName FROM tblclass WHERE  Id<> " + MyStudMang.GetClassId(int.Parse(Session["StudId"].ToString())) + subsql;
            OdbcDataReader _myreader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (_myreader.HasRows)
            {
                Drp_NewClass.Items.Add(new ListItem("Select Class", "-1"));
                while (_myreader.Read())
                {
                    Drp_NewClass.Items.Add(new ListItem(_myreader.GetValue(1).ToString(), _myreader.GetValue(0).ToString()));
                }
                Drp_NewClass.SelectedIndex = 0;
            }
            else
            {
                Drp_NewClass.Items.Add(new ListItem("No Class Found", "-1"));
            
            }

        }

        private void LoadConfirmation_In_RDB(RadioButtonList Rdb_Control)
        {
            Rdb_Control.Items.Clear();
            Rdb_Control.Items.Add(new ListItem("Yes", "0"));
            Rdb_Control.Items.Add(new ListItem("No", "1"));
        }

        protected void Btn_Switch_Click(object sender, EventArgs e)
        {

           
            string _msg="";
            if (IsSwitchPossible(out _msg))
            {
                Panel_Select.Visible = false;
                Panel_Information.Visible = true;
                string ExamStr1 = "", ExamStr2 = "";
                GetExamDetails(int.Parse(Session["StudId"].ToString()), out ExamStr1, out ExamStr2);
                Lbl_Exam1.Text = ExamStr1;
                Lbl_Exam2.Text = ExamStr2;

                string FeeStr1 = "", FeeStr2 = "";
                GetFeeDetails(int.Parse(Session["StudId"].ToString()), out FeeStr1, out FeeStr2);
                Lbl_Fees1.Text = FeeStr1;
                Lbl_Fees2.Text = FeeStr2;


                string Attendancestr1 = "";
                bool _needconfirmation1 = false;
                GetAttendanceDetails(int.Parse(Session["StudId"].ToString()), out Attendancestr1, out _needconfirmation1);
                Lbl_Attendance1.Text = Attendancestr1;
                if (_needconfirmation1)
                {
                    LoadConfirmation_In_RDB(Rdb_Attedance1);
                }

                lbl_newclass.Text = Drp_NewClass.SelectedItem.Text;

            }
            else
            {
                Lbl_mainmsg.Text = _msg;
            }

        }



        private void GetAttendanceDetails(int StudentId, out string Attendancestr1, out bool _needconfirmation1)
        {
            _needconfirmation1 = false;
            Attendancestr1 = "No attendance marked for the selected student";
            if (MyAttdMang.AttendanceTables_Exits(MyStudMang.GetStandard(Session["StudId"].ToString()).ToString(), MyUser.CurrentBatchId))
            {
                int WorkingDays= MyAttdMang.GetNumberOfworkingDayForClassFromNewAttdance(int.Parse(Session["StudId"].ToString()), MyUser.CurrentBatchId, MyStudMang.GetStandard(Session["StudId"].ToString()));
                if (WorkingDays > 0)
                {
                    string pluralday = "";
                    if (WorkingDays > 1)
                    {
                        pluralday = "s";
                    }
                    Attendancestr1 = "For the current batch "+ WorkingDays + " day" + pluralday + " attendance has been marked for the student. If you want to remove these attendance, please select yes. Otherwise select no";
                    _needconfirmation1 = true;
                }

            }
        }

        private void GetFeeDetails(int StudentId, out string FeeStr1, out string FeeStr2)
        {
            FeeStr1 = "";
            FeeStr2 = "";
            bool _NeedStudentFees= false, _NeedClassFees = false;
            int StudentFeesCount = 0, ClassFeedCount = 0, total = 0;
            string sql = "SELECT DISTINCT tbltransaction.PaymentElementId,tblfeeaccount.AssociatedId   FROM tbltransaction INNER JOIN tblfeebill ON tbltransaction.BillNo=tblfeebill.BillNo INNER JOIN tblfeeschedule ON tblfeeschedule.Id=tbltransaction.PaymentElementId INNER JOIN tblfeeaccount ON tblfeeaccount.Id=tblfeeschedule.FeeId WHERE tbltransaction.Canceled=0 AND tbltransaction.CollectionType=1 AND tbltransaction.PaymentElementId <> -1 AND  tbltransaction.AccountTo=1 AND tbltransaction.AccountFrom=2 AND tblfeeschedule.BatchId=" + MyUser.CurrentBatchId + " AND tblfeebill.StudentID=" + StudentId + " UNION SELECT  DISTINCT tblfeeschedule.Id,tblfeeaccount.AssociatedId FROM tblfeeadvancetransaction INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tblfeeadvancetransaction.StudentId INNER JOIN tblfeeschedule ON tblfeeschedule.ClassId=tblstudentclassmap.ClassId AND tblfeeschedule.PeriodId=tblfeeadvancetransaction.PeriodId AND tblfeeschedule.FeeId=tblfeeadvancetransaction.FeeId AND tblfeeadvancetransaction.BatchId=tblfeeschedule.BatchId INNER JOIN tblfeeaccount ON tblfeeaccount.Id=tblfeeadvancetransaction.FeeId  WHERE tblfeeadvancetransaction.BillNo='NIL' AND tblfeeadvancetransaction.`Type`=0 AND tblfeeadvancetransaction.StudentId="+StudentId+" AND tblfeeadvancetransaction.BatchId="+MyUser.CurrentBatchId;
             DataSet MyDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
             if (MyDataSet != null && MyDataSet.Tables[0].Rows.Count > 0)
             {
                 string AssociatedType = "";
                 foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                 {
                     total++;
                     AssociatedType = dr[1].ToString();


                     if (AssociatedType == "2")
                     {
                         StudentFeesCount++;
                         _NeedStudentFees = true;
                     }
                     else
                     {
                         ClassFeedCount++;
                         _NeedClassFees = true;
                     }
                 }
             }
             else
             {
                 FeeStr1 = "No sheduled fees collected from the selected student.";
             }

             if (_NeedStudentFees)
             {
                 string pluraltotal = "", pluralcount="";
                 if (total > 1)
                 {
                     pluraltotal = "s";
                 }

                 if (StudentFeesCount > 1)
                 {
                     pluralcount = "s";
                 }

                 FeeStr1 = "Out of " + total + " paid scheduled fee" + pluraltotal + ", " + StudentFeesCount + " has been associated to student.";
             }

             if (_NeedClassFees)
             {
                 string pluraltotal = "", pluralcount = "";
                 if (total > 1)
                 {
                     pluraltotal = "s";
                 }

                 if (ClassFeedCount > 1)
                 {
                     pluralcount = "s";
                 }

                 string startstr = "";    
                 if (!_NeedStudentFees)
                 {
                     startstr = "Out of " + total + " paid scheduled fee" + pluraltotal + ", ";
                 }

                 FeeStr2 = startstr + ClassFeedCount + " has been scheduled to class. Amount collected aganist fees scheduled to class will be converted as advance paid.";
             }


        }

        private void GetExamDetails(int StudentId, out string ExamStr1, out string ExamStr2)
        {
            ExamStr1 = ""; ExamStr2 = "";
            bool _NeedCompletedExams = false,_NeedUpdatedExams=false;
            int CompletedExamCount = 0, MarkUpdatedCount = 0,total=0;
            string sql = "SELECT tblstudentmark.ExamSchId,tblexamschedule.Status FROM tblstudentmark INNER JOIN tblexamschedule ON tblexamschedule.Id=tblstudentmark.ExamSchId WHERE tblstudentmark.StudId=" + StudentId;
            DataSet MyDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyDataSet != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {
                    if (dr[1].ToString() == "Completed")
                    {
                        CompletedExamCount++;
                        _NeedCompletedExams = true;

                    }
                    else
                    {
                        MarkUpdatedCount++;
                        _NeedUpdatedExams = true;
                    }
                    total++;
                }
            }
            else
            {
                ExamStr1 = "No exam marks has been entered for the selected student.";
            }

            if (_NeedCompletedExams)
            {
                string pluraltotal = "", pluralcount = "";
                if (total > 1)
                {
                    pluraltotal = "s";
                }

                if (CompletedExamCount > 1)
                {
                    pluralcount = "s";
                }

                ExamStr1 = "Out of " + total + " exam" + pluraltotal + ", report has been generated for " + CompletedExamCount + " exam" + pluralcount + ".";
            }

            if (_NeedUpdatedExams)
            {
                string pluraltotal = "", pluralcount = "";
                if (total > 1)
                {
                    pluraltotal = "s";
                }

                if (MarkUpdatedCount > 1)
                {
                    pluralcount = "s";
                }

                string startstr = "";
                if (!_NeedCompletedExams)
                {
                    startstr = "Out of " + total + " exam" + pluraltotal + ", ";
                }
                ExamStr2 = startstr + " Marks for " + MarkUpdatedCount + " exam" + pluralcount + " has been entered. But for these exams report generation has not been done. Marks of the selected student for these exam will be removed.";
            }
        }


        private bool IsSwitchPossible(out string _msg)
        {
            _msg = "";
            bool _valid = true;

            if (Drp_NewClass.SelectedValue == "-1")
            {
                _msg = "Please select class";
                _valid = false;
            }

            if (_valid)
            {
                if (Txt_Remark.Text == "")
                {
                    _msg = "Please enter the reason for switching class";
                    _valid = false;
                }
            }


            if (_valid)
            {
                if (Txt_Remark.Text.Length>150)
                {
                    _msg = "Please enter the reason in less than 150 characters";
                    _valid = false;
                }
            }

            return _valid;
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudDivTransfer.aspx");
        }

      
        protected void Bnt_CancelSelection_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudDivTransfer.aspx");
        }


        protected void Btn_Confirm_Click(object sender, EventArgs e)
        {
            string _msg = "";
            if (_IsSavingPossible(out _msg))
            {
                string ExamStr = "", FeeStr = "", AttendanceStr = "",ErrorMsg="";
                bool _RemoveAttendance=false,_countine=true;
                int OldClassId=MyStudMang.GetClassId(int.Parse(Session["StudId"].ToString()));
                int StudentId=int.Parse(Session["StudId"].ToString());
                int NewClassId=int.Parse(Drp_NewClass.SelectedValue);
                string newStandard = MyClassMang.GetStandardIdfromClassId(NewClassId.ToString());
                string OldStandard = MyClassMang.GetStandardIdfromClassId(OldClassId.ToString());
                string OldRollNo= MyClassMang.GetRollnumber(StudentId, OldClassId, MyUser.CurrentBatchId);

                if(Rdb_Attedance1.SelectedValue=="0")
                {
                    _RemoveAttendance=true;
                }

                try
                {
                    MyStudMang.CreateTansationDb();

                    if (MyStudMang.SwitchStudentData(StudentId, OldClassId, NewClassId, MyUser.CurrentBatchId, _RemoveAttendance, out ExamStr, out FeeStr, out AttendanceStr, out ErrorMsg))
                    {
                        _countine = true;
                    }

                    MyStudMang.UpdateClassInStudentMap(StudentId, newStandard, Drp_NewClass.SelectedValue);
                    MyStudMang.ScheduleRollNumber(NewClassId, MyUser.CurrentBatchId, StudentId);
                    MyStudMang.UpdateLastClassId(StudentId, NewClassId);

                    if (_countine && MyStudMang.ScheduleClassWisePendingFees(StudentId, MyUser.CurrentBatchId, NewClassId, false))
                    {
                        _countine = true;
                    }

                    MyStudMang.InsertClassSwichinghistory(StudentId, OldClassId, OldStandard, MyUser.CurrentBatchId, OldRollNo, Txt_Remark.Text.Trim());

                    if (_countine)
                    {
                        MyStudMang.EndSucessTansationDb();
                        string _htmldiv = " <br/> <center> <h2> Student successfully switched to new class </h2> <br/> </center> ";
                        if (ExamStr != "")
                        {
                            _htmldiv = _htmldiv + "<br/><br/> <h3>Exam Details</h3> <br/> " + ExamStr;
                        }

                        if (FeeStr != "")
                        {
                            _htmldiv = _htmldiv + "<br/><br/> <h3>Fee Details</h3> <br/> " + FeeStr;
                        }

                        if (AttendanceStr != "")
                        {
                            _htmldiv = _htmldiv + "<br/><br/> <h3>Attendance Details</h3> <br/> " + AttendanceStr;
                        }

                        _htmldiv = _htmldiv + "<br/> <h4>* Please Note</h4> <br/> Please reschedule roll number for the Old and New classes";
                        
                        HtmlDiv.InnerHtml = _htmldiv;
                        MPE_MessageBox.Show();

                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Class Changes", "The student " + MyUser.getStudName(StudentId) + "  is moved from " + MyStudMang.GetClassName(OldClassId) + " to " + MyStudMang.GetClassName(NewClassId) + "", 1);
                       
                    }
                    else
                    {
                        MyStudMang.EndFailTansationDb();
                        WC_MessageBox.ShowMssage("Error while switching class. Please try again later.",MSGTYPE.Alert);
                    }


                }
                catch (Exception Ex)
                {
                    MyStudMang.EndFailTansationDb();
                    WC_MessageBox.ShowMssage("Error while switching class. Error message : " + Ex.Message, MSGTYPE.Alert);
                }
            }
            else
            {
                WC_MessageBox.ShowMssage(_msg,MSGTYPE.Alert);
            }
        }

        private bool _IsSavingPossible(out string _msg)
        {
            _msg = "";
            bool _valid = true;

            if (Rdb_Attedance1.Items.Count > 0)
            {
                if(Rdb_Attedance1.SelectedValue!="0" && Rdb_Attedance1.SelectedValue!="1")
                {
                    _msg = "Select either Yes or No for attendance conversion";
                    _valid = false;
                }
            }

            return _valid;
        }

        protected void Btn_magok_Click(object sender, EventArgs e)
        {
            Response.Redirect("SearchStudent.aspx");
        }

        protected void Chk_Class_CheckedChanged(object sender, EventArgs e)
        {
            loadDrp_Class();
        }

    }
}
