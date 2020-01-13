using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;

namespace WinEr
{
    public partial class MarkClassAttendancePage : System.Web.UI.Page
    {
        private Attendance MyAttendance;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private OdbcDataReader MyReader0 = null;
        private DataSet MydataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyAttendance = MyUser.GetAttendancetObj();
            if (MyAttendance == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }


            else
            {
                string _msg = "";
                bool _Valid = true;
                DateTime M_StartTime = new DateTime();
                int  M_ClassId = 0;
                int ClassAttendanceId = 0;

                # region Validation


                    if (Request.QueryString["Date"] == null)
                    {
                        _msg = "Invalid Start Time";
                        _Valid = false;
                    }
                    else
                    {
                        if (!DateTime.TryParse(Request.QueryString["Date"].ToString(), out M_StartTime))
                        {
                            _msg = "Invalid Start Time";
                            _Valid = false;
                        }
                        else
                        {
                            Session["StudDate"] = M_StartTime;
                        }
                    }



              
                if (Session["ClassId"] == null)
                {
                    _msg = "Invalid Class";
                    _Valid = false;


                }
                else
                {
                    if (!int.TryParse(Session["ClassId"].ToString(), out M_ClassId))
                    {
                        _msg = "Invalid Class";
                        _Valid = false;
                    }
                }

                # endregion


                if (_Valid)
                {
                    if (!IsPostBack)
                    {
                        try
                        {

                            Panel_Saving.Visible = false;
                            Panel_Updating.Visible = false;

                            if (M_StartTime <= DateTime.Today)
                            {

                                if (_Valid)
                                {

                                    Load_TopDetails(M_ClassId,  M_StartTime);

                                    string Status = "";

                                    if (MyAttendance.IsAttendanceAlreadyMarked(M_ClassId, M_StartTime, Hidden_StandardId.Value, MyUser.CurrentBatchId, out Status, out ClassAttendanceId))
                                    {
                                        if (MyAttendance.IsRollNoCofigEnabled())
                                        {
                                            if (Status != "3")
                                            {
                                                Panel_Saving.Visible = true;
                                                Rollno_Visibilty(false);
                                                if (Status == "1")
                                                {
                                                    Btn_DeleteMarking.Text = "Cancel ForeNoon Attendance";
                                                    RdBtn_Type.Items[2].Selected = true;
                                                    RdBtn_Type.Items[0].Enabled = false;
                                                    RdBtn_Type.Items[1].Enabled = false;
                                                }
                                                else
                                                {
                                                    Btn_DeleteMarking.Text = "Cancel AfterNoon Attendance";
                                                    RdBtn_Type.Items[1].Selected = true;
                                                    RdBtn_Type.Items[0].Enabled = false;
                                                    RdBtn_Type.Items[2].Enabled = false;
                                                }
                                            }
                                            else
                                            {
                                                Btn_DeleteMarking.Text = "Cancel Full Day Attendance";

                                            }
                                            Panel_Updating.Visible = true;
                                            HiddenClassAttendance.Value = ClassAttendanceId.ToString();
                                            Load_StudentGrid(ClassAttendanceId, M_ClassId, Hidden_StandardId.Value, MyUser.CurrentBatchId, Status);
                                            HiddenStatus.Value = Status;
                                        }
                                        else
                                        {
                                            Btn_DeleteMarking.Text = "Cancel Attendance";
                                            Panel_Updating.Visible = true;
                                            HiddenClassAttendance.Value = ClassAttendanceId.ToString();
                                            Load_StudentGrid(ClassAttendanceId, M_ClassId, Hidden_StandardId.Value, MyUser.CurrentBatchId, Status);
                                            HiddenStatus.Value = "3";
                                        }
                                    }
                                    else 
                                    {
                                        if (MyAttendance.IsRollNoCofigEnabled())
                                        {
                                            HiddenClassAttendance.Value = "0";
                                            Panel_Saving.Visible = true;
                                            Rollno_Visibilty(false);
                                            HiddenStatus.Value = "0";
                                        }
                                        else
                                        {
                                            HiddenClassAttendance.Value = "0";
                                            MarkAllPresent();
                                            Btn_DeleteMarking.Text = "Cancel Attendance";
                                            Panel_Updating.Visible = true;
                                            Load_StudentGrid(int.Parse(HiddenClassAttendance.Value), M_ClassId, Hidden_StandardId.Value, MyUser.CurrentBatchId, Status);
                                            HiddenStatus.Value = "3";
                                        }
                                    }
                                }
                                else
                                {
                                    //ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "pageresize();", true);
                                    Lbl_msgAlert.Text = _msg;
                                    MPE_MessageBox.Show();
                                }

                            }
                            else
                            {
                                //ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "pageresize();", true);
                                Lbl_msgAlert.Text = "You have Selected A Future Date";
                                MPE_MessageBox.Show();
                            }
                        }
                        catch
                        {
                            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "pageresize();", true);
                            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "pagereload();", true);
                            Lbl_msgAlert.Text = "Error In Connection. Try Again";
                            MPE_MessageBox.Show();
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "pageresize();", true);
                    Lbl_msgAlert.Text = _msg;
                    MPE_MessageBox.Show();
                }
            }

        }

        private void MarkAllPresent()
        {
            int ClassId = 0;
            int ClassAttendanceId = 0;
            int YearId = MyUser.CurrentBatchId;
            string StandardId = Hidden_StandardId.Value;
            string msg = "";
            if (Possible(out msg))
            {
                int.TryParse(Session["ClassId"].ToString(), out ClassId);
                try
                {
                    MyAttendance.CreateTansationDb();
                    if (MyAttendance.AttendanceTables_Exits(StandardId, YearId))
                    {
                        if (HiddenClassAttendance.Value == "0")
                        {

                            MyAttendance.SaveClassAttendanceDetails(ClassId, MyUser.GetDareFromText(lbl_Date.Text), MyUser.UserId, StandardId, YearId, RdBtn_Type.SelectedValue, out ClassAttendanceId);
                            HiddenClassAttendance.Value = ClassAttendanceId.ToString();
                        }
                        else
                        {
                            MyAttendance.UpdateClassAttendanceDetails(ClassId, HiddenClassAttendance.Value, RdBtn_Type.SelectedValue, MyUser.UserId, StandardId, YearId);
                        }
                    }
                    //ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "pagereload();", true);
                   
                    MyAttendance.EndSucessTansationDb();
                    MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Attendance Marked", RdBtn_Type.SelectedItem.Text + " Attendance marked for  " + Lbl_Class.Text, 1,1);
                    if (MyUser.HaveModule(20))
                    {
                        MarkAttendanceIncidence(new int[0], 0, ClassId, StandardId, MyUser.GetDareFromText(lbl_Date.Text));
                    }
                }
                catch
                {
                    Lbl_msg.Text = "Error in saving.Try again";
                    MyAttendance.EndFailTansationDb();
                }
            }
            else
            {
                Lbl_msg.Text = msg;

            }
        }




        private void Load_StudentGrid(int ClassAttendanceId, int ClassId, string StandardId, int YearId, string PresentStatus)
        {
            bool NeedPictureAttd_Integration = NeedPictureAttendance_Integration();
            HiddenLink.Value = Student_Attend_Link();
            HiddenEmptyLink.Value = Student_EmptyAttend_Link();
            Lbl_GridMsgCommon.Text = "";
            string sql = "SELECT t1.PresentStatus,tblstudent.Id,tblstudent.StudentName,tblstudentclassmap.RollNo FROM tblstudent INNER JOIN tblattdstud_std" + StandardId + "yr" + YearId + " as t1 ON t1.StudentId=tblstudent.Id INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudentclassmap.ClassId=" + ClassId + " AND t1.classAttendanceId=" + ClassAttendanceId + " ORDER BY tblstudentclassmap.RollNo";
            MydataSet = MyAttendance.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_Students.Columns[0].Visible = true;
                Grd_Students.Columns[1].Visible = true;
                Grd_Students.Columns[5].Visible = true;
                Grd_Students.DataSource = MydataSet;
                Grd_Students.DataBind();
                Grd_Students.Columns[0].Visible = false;
                Grd_Students.Columns[1].Visible = false;
                if (!NeedPictureAttd_Integration)
                {
                    Grd_Students.Columns[5].Visible = false;
                }
                FillPresentStatus(PresentStatus);

            }
            else
            {
                Lbl_GridMsgCommon.Text = "No Student Found";
                Grd_Students.DataSource = null;
                Grd_Students.DataBind();
            }
        }

        private string Student_EmptyAttend_Link()
        {
            string _need = "";
            string sql = "select tblconfiguration.Value from tblconfiguration WHERE tblconfiguration.Name='Student_Attend_Link_Empty'";
            MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _need = MyReader.GetValue(0).ToString();
            }
            return _need;
        }

        private string Student_Attend_Link()
        {
            string _need = "";
            string sql = "select tblconfiguration.Value from tblconfiguration WHERE tblconfiguration.Name='Student_Attend_Link'";
            MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _need = MyReader.GetValue(0).ToString();
            }
            return _need;
        }

        private bool NeedPictureAttendance_Integration()
        {
            bool _need = false;
            string sql = "select tblconfiguration.Value from tblconfiguration WHERE tblconfiguration.Name='NeedPictureAttendance_Integration'";
            MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int _value = 0;
                int.TryParse(MyReader.GetValue(0).ToString(),out _value);
                if (_value == 1)
                {
                    _need = true;
                }
            }
            return _need;
        }

        private void FillPresentStatus(string PresentStatus)
        {
            foreach (GridViewRow gv in Grd_Students.Rows)
            {
                DropDownList dr = (DropDownList)gv.FindControl("Drp_GridStatus");
                dr.Items.Clear();
                dr.Items.Add(new ListItem("Absent", "0"));
                if (PresentStatus == "1")
                {
                    dr.Items.Add(new ListItem("ForeNoon", "1"));
                }
                else if (PresentStatus == "2")
                {
                    dr.Items.Add(new ListItem("AfterNoon", "2"));
                }
                else
                {
                    dr.Items.Add(new ListItem("ForeNoon", "1"));
                    dr.Items.Add(new ListItem("AfterNoon", "2"));
                    dr.Items.Add(new ListItem("Full Day", "3"));
                }
                dr.SelectedValue = gv.Cells[1].Text;
             
            }
        }


        private void Load_TopDetails(int M_ClassId,DateTime M_StartTime)
        {
            Lbl_Class.Text = GetClassName(M_ClassId);
            lbl_Date.Text = MyUser.GerFormatedDatVal(M_StartTime);
            HiddenDate.Value = MyUser.GerFormatedDatVal(M_StartTime);
            Hidden_StandardId.Value = MyAttendance.GetStandard_Class(int.Parse(Session["ClassId"].ToString()));
        }



        private string GetClassName(int M_ClassId)
        {
            string ClassName = "";
            string sql = "SELECT tblclass.ClassName from tblclass where tblclass.Status=1 AND tblclass.Id=" + M_ClassId;
            MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ClassName = MyReader.GetValue(0).ToString();
            }
            return ClassName;
        }

        protected void Lnk_RollNoVisibilty_Click(object sender, EventArgs e)
        {
            Rollno_Visibilty(true);
        }

        private void Rollno_Visibilty(bool visibilty)
        {
            //Label4.Visible = visibilty;
            Panel_RollNo.Visible = visibilty;
          //  Img_Cancel.Visible = visibilty;
            Lnk_Cancel.Visible = visibilty;
            Lnk_RollNoVisibilty.Visible = !visibilty;
            Btn_All_Present.Enabled = !visibilty;
            Btn_Update.Enabled = !visibilty;
            Btn_DeleteMarking.Enabled = !visibilty;
            Btn_CancelUpdate.Enabled = !visibilty;
          
        }



        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            Lbl_msg.Text = "";
            string ClientSideData = HiddenValue.Value;
            if (ClientSideData != "")
            {
               
                string[] RollNo = ClientSideData.Split('/');
                int[] CorrectStudents = new int[RollNo.Length];
                string[] MissRollNo = new string[RollNo.Length];
                string[] ErrorRollNo = new string[RollNo.Length];
                int ClassId = 0;
                int StudentId = 0, ClassAttendanceId = 0;
                int missRollNosCount = 0, ErrorRollNosCount = 0,CorrectRollNoCount=0;
                int YearId = MyUser.CurrentBatchId;
                string StandardId=Hidden_StandardId.Value;
                bool Save_Done = false;
                bool Anycorrect = false;
                bool MasterOnceUpdated = false;
                int.TryParse(Session["ClassId"].ToString(), out ClassId);

                if (MyAttendance.AttendanceTables_Exits(Hidden_StandardId.Value, YearId))
                {
                    ClassAttendanceId = 0;
                    for (int i = 0; i < RollNo.Length; i++)
                    {
                        StudentId = 0;
                        try
                        {
                            MyAttendance.CreateTansationDb();
                            if (ValidEntry(RollNo[i], ClassId, YearId, out StudentId))
                            {
                                Anycorrect = true;
                                if (HiddenClassAttendance.Value == "0")
                                {
                                    if (ClassAttendanceId == 0)
                                    {
                                        MyAttendance.SaveClassAttendanceDetails(ClassId, MyUser.GetDareFromText(lbl_Date.Text), MyUser.UserId, StandardId, YearId, RdBtn_Type.SelectedValue, out ClassAttendanceId);
                                    }
                                    if (ClassAttendanceId > 0)
                                    {

                                        MyAttendance.UpdateStudentAttendanceDetails(ClassAttendanceId, StudentId.ToString(), 0, ClassId, StandardId, YearId);
                                        CorrectStudents[CorrectRollNoCount] = StudentId;
                                        CorrectRollNoCount++;
                                        Save_Done = true;
                                    }
                                }
                                else
                                {
                                    if (!MasterOnceUpdated)
                                    {
                                        MyAttendance.UpdateClassAttendanceDetails(ClassId, HiddenClassAttendance.Value, RdBtn_Type.SelectedValue, MyUser.UserId, StandardId, YearId);
                                        MasterOnceUpdated = true;
                                    }

                                    MyAttendance.UpdateStudentAttendanceDetailsAgain(int.Parse(HiddenClassAttendance.Value), StudentId.ToString(), "-"+RdBtn_Type.SelectedValue, StandardId, YearId);
                                    Save_Done = true;
                                    CorrectStudents[CorrectRollNoCount] = StudentId;
                                    CorrectRollNoCount++;
                                }
                            }
                            else
                            {
                                MissRollNo[missRollNosCount] = RollNo[i];
                                missRollNosCount++;
                            }
                            MyAttendance.EndSucessTansationDb();
                        }
                        catch
                        {
                            MyAttendance.EndFailTansationDb();
                            ErrorRollNo[ErrorRollNosCount] = RollNo[i];
                            ErrorRollNosCount++;
                        }

                    }

                    if (Anycorrect)
                    {

                        string Nos = "";
                        string symbol = "";
                        if (Save_Done)
                        {
                            if (missRollNosCount != 0 || ErrorRollNosCount != 0)
                            {
                                if (missRollNosCount != 0)
                                {
                                    for (int i = 0; i < missRollNosCount; i++)
                                    {
                                        Nos = Nos + symbol + MissRollNo[i];
                                        symbol = ",";
                                    }
                                    lbl_MissRollNos.Text = "The following RollNos are not correct :" + Nos;
                                }
                                Nos = ""; symbol = "";
                                if (ErrorRollNosCount != 0)
                                {
                                    for (int i = 0; i < ErrorRollNosCount; i++)
                                    {
                                        Nos = Nos + symbol + ErrorRollNo[i];
                                        symbol = ",";
                                    }
                                    Lbl_ErrorRolLNo.Text = "Error happened while marking following RollNos :" + Nos;
                                }

                                Lbl_FinishMsg.Text = "Attendance of correct RollNos are marked successfully";
                            }
                            else
                            {
                                Lbl_FinishMsg.Text = "Attendance Marked Successfully";
                                MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Attendance Marked", RdBtn_Type.SelectedItem.Text + " Attendance marked for  " + Lbl_Class.Text, 1,1);

                            }
                            //ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "pagereload();", true);
                            //Lbl_Link.Text = "MarkClassDailyAttendance.aspx?ClassAttendanceId=" + ClassAttendanceId;
                            M_Finish.Show();
                            if (MyUser.HaveModule(20))
                            {
                                MarkAttendanceIncidence(CorrectStudents, CorrectRollNoCount, ClassId, StandardId, MyUser.GetDareFromText(lbl_Date.Text));
                            }
                        }
                        else
                        {
                            Lbl_msg.Text = "Error in saving.Try again";
                        }

                    }
                    else
                    {
                        Lbl_msg.Text = "All RollNos are mistake";
                    }

                }
                else
                {
                    Lbl_msg.Text = "Error In Database";
                }
            }
            else
            {
                Lbl_msg.Text = "No RollNo Found For Saving";
            }
            Rollno_Visibilty(false);
        }

        private void MarkAttendanceIncidence(int[] CorrectStudents, int CorrectRollNoCount, int ClassId, string StandardId, DateTime Day)
        {
            int ContinuesAbsentlimt = 2;
            int StudentStatus = 0,StudentId=0,AbsentDaysCount=0;
            int Status = MyAttendance.GetAttendanceStatus(Day, ClassId, StandardId, MyUser.CurrentBatchId);
            string AbsentDates = "";
            if (Status == 3)
            {
                string Correctstudentstr = "",temp="";
                if (CorrectRollNoCount > 0)
                {
                    for (int i = 0; i < CorrectRollNoCount; i++)
                    {
                        if (CorrectStudents[i] != 0)
                        {
                            Correctstudentstr = Correctstudentstr + temp + CorrectStudents[i];
                            temp = ",";
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    Correctstudentstr = "0";
                }
                string sql = "Select tblstudent.Id, tblstudent.StudentName from tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudent.Id where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " and tblstudentclassmap.ClassId=" + ClassId + " and tblstudent.Id not in(" + Correctstudentstr + ") and  tblstudent.Status=1 order by  tblstudent.StudentName ASC";
                MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        StudentStatus = 0;
                        StudentId=0;
                        int.TryParse(MyReader.GetValue(0).ToString(),out StudentId);
                        if (MyAttendance.IsNew_attendanceReportedOnSelectedDate(StudentId, int.Parse(StandardId), MyUser.CurrentBatchId, Day, out StudentStatus))
                        {
                            if(StudentStatus!=0)
                            {
                                AbsentDaysCount = 0;
                                AbsentDates = "";
                                AbsentDaysCount = Getnumber_PreviousConsicutiveAbsents(StudentId, int.Parse(StandardId), ClassId, MyUser.CurrentBatchId, Day,out AbsentDates);
                                if (AbsentDaysCount >= ContinuesAbsentlimt)
                                {
                                    Incident Myincidence = MyUser.GetIncedentObj();
                                    Myincidence.Reportincident("StudentAbsentEquation", AbsentDaysCount, AbsentDaysCount, AbsentDates, StudentId, "student", ClassId, MyUser.CurrentBatchId, MyUser.UserId, 0, "ATTD" + StudentId.ToString() + ClassId.ToString() + MyUser.CurrentBatchId + MyUser.GerFormatedDatVal(Day));

                                }
                            }
                        }
                    }
                }

            }
        }

        private int Getnumber_PreviousConsicutiveAbsents(int StudentId, int StandardId, int ClassId, int Batchid, DateTime Day,out string AbsentDates)
        {
            AbsentDates = "";
            string seperator = "";
            int Count = 0, Status = 0;
            DateTime EachDay=new DateTime();
            string sql = "SELECT t1.`Date` FROM tblattdcls_std"+StandardId+"yr"+Batchid+" t1 WHERE Date(t1.`Date`)<'"+Day.Date.ToString("s")+"' AND t1.ClassId="+ClassId+" ORDER BY t1.`Date` DESC ";
            MyReader0 = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader0.HasRows)
            {
                while (MyReader0.Read())
                {
                    EachDay=new DateTime();
                    Status = 0;
                    DateTime.TryParse(MyReader0.GetValue(0).ToString(),out EachDay);
                    if (MyAttendance.IsNew_attendanceReportedOnSelectedDate(StudentId, StandardId, Batchid,  EachDay, out Status))
                    {
                        if (Status != 0)
                        {
                            break;
                        }
                        else
                        {
                            Count++;
                            AbsentDates = MyUser.GerFormatedDatVal(EachDay) + seperator + AbsentDates;
                            seperator = ",";
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return Count;
        }


        protected void Btn_All_Present_Click(object sender, EventArgs e)
        {
           
            int ClassId = 0;
            int ClassAttendanceId = 0;
            int YearId = MyUser.CurrentBatchId;
            string StandardId = Hidden_StandardId.Value;
            string msg = "";
            if (Possible(out msg))
            {
                int.TryParse(Session["ClassId"].ToString(), out ClassId);
                try
                {
                    MyAttendance.CreateTansationDb();
                    if (MyAttendance.AttendanceTables_Exits(StandardId, YearId))
                    {
                        if (HiddenClassAttendance.Value == "0")
                        {

                            MyAttendance.SaveClassAttendanceDetails(ClassId, MyUser.GetDareFromText(lbl_Date.Text), MyUser.UserId, StandardId, YearId, RdBtn_Type.SelectedValue, out ClassAttendanceId);
                        }
                        else
                        {
                            MyAttendance.UpdateClassAttendanceDetails(ClassId, HiddenClassAttendance.Value, RdBtn_Type.SelectedValue, MyUser.UserId, StandardId, YearId);
                        }
                    }
                    //ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "pagereload();", true);
                    Lbl_FinishMsg.Text = "Attendance Marked Successfully";
                    Lbl_Link.Text = "MarkClassDailyAttendance.aspx?ClassAttendanceId=" + ClassAttendanceId;
                    MyAttendance.EndSucessTansationDb();
                    MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Attendance Marked", RdBtn_Type.SelectedItem.Text + " Attendance marked for  " + Lbl_Class.Text, 1,1);

                    M_Finish.Show();
                    if (MyUser.HaveModule(20))
                    {
                        MarkAttendanceIncidence(new int[0], 0, ClassId, StandardId, MyUser.GetDareFromText(lbl_Date.Text));
                    }
                }
                catch
                {
                    Lbl_msg.Text = "Error in saving.Try again";
                    MyAttendance.EndFailTansationDb();
                }
            }
            else
            {
                Lbl_msg.Text = msg;
                
            }

        }

        private bool Possible(out string msg)
        {
            msg = "";
            bool valid = false;
            int count = 0;
            string sql = "SELECT Count(tblstudentclassmap.StudentId) FROM tblstudentclassmap INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudent.Status=1 AND tblstudentclassmap.ClassId=" + Session["ClassId"].ToString() + " AND tblstudentclassmap.BatchId="+MyUser.CurrentBatchId;
            MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out count);
                if (count > 0)
                {
                    valid = true;
                }
            }
            if (!valid)
            {
                msg = "No student found";
            }
            return valid;
        }

        private bool ValidEntry(string RollNo, int ClassId, int YearId, out int StudentId)
        {
            StudentId = 0;
            bool valid = false;
            string _rollNo = "";
            string sql = "SELECT tblstudentclassmap.RollNo,tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.ClassId=" + ClassId + " AND tblstudentclassmap.BatchId=" + YearId + " AND tblstudentclassmap.RollNo=" + RollNo;
            MyReader = MyAttendance.m_TransationDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _rollNo = MyReader.GetValue(0).ToString();
                int.TryParse(MyReader.GetValue(1).ToString(), out StudentId);
                if (_rollNo == RollNo && StudentId > 0)
                {
                    valid = true;
                }
            }
            return valid;
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Rollno_Visibilty(false);
        }

 

        protected void Btn_Update_Click(object sender, EventArgs e)
        {
            bool Change = false;
            Lbl_GridMsg.Text = "";
            int Status = 0;
            int ClassId = 0;
            string StandardId = Hidden_StandardId.Value;
            int.TryParse(Session["ClassId"].ToString(), out ClassId);
            try
            {
                MyAttendance.CreateTansationDb();

                foreach (GridViewRow gv in Grd_Students.Rows)
                {
                    Status = 0;
                    DropDownList dr = (DropDownList)gv.FindControl("Drp_GridStatus");
                    int.TryParse( dr.SelectedValue,out Status);
                    if (gv.Cells[1].Text.ToString() != dr.SelectedValue)
                    {
                        MyAttendance.UpdateStudentAttendanceDetails(int.Parse(HiddenClassAttendance.Value), gv.Cells[0].Text.ToString(), Status, int.Parse(Session["ClassId"].ToString()), Hidden_StandardId.Value, MyUser.CurrentBatchId);
                        Change = true;
                    }
                }
                if (Change)
                {
                    UpdateClassAttendanceDetails(HiddenClassAttendance.Value, int.Parse(Session["ClassId"].ToString()), MyUser.CurrentBatchId, MyUser.User_Id);
                   
                }
                MyAttendance.EndSucessTansationDb();
                if (Change)
                {
                    Lbl_FinishMsg.Text = "Successfully Updated";
                    if (MyUser.HaveModule(20))
                    {
                        MarkAttendanceIncidence(new int[0], 0, ClassId, StandardId, MyUser.GetDareFromText(lbl_Date.Text));
                    }
                }
                else
                {
                    Lbl_FinishMsg.Text = "No Changes Made For Updation";
                }
                MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Attendance Updated", "Attendance Updated for  " + Lbl_Class.Text, 1,1);

            }
            catch
            {
                //ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "pagereload();", true);
                Lbl_FinishMsg.Text = "Error In Updation";
                MyAttendance.EndFailTansationDb();
            }
            Lbl_Link.Text = "MarkClassDailyAttendance.aspx?ClassAttendanceId=" + HiddenClassAttendance.Value;
            M_Finish.Show();
        }

        private void UpdateClassAttendanceDetails(string ClassAttdId, int ClassId, int YearId,int UserId)
        {
            string sql = "UPDATE tblattdcls_std" + Hidden_StandardId.Value + "yr" + YearId + " SET LastModifiedDateTime='" + DateTime.Now.ToString("s") + "',LastModifiedUserId=" + UserId + "  WHERE Id=" + ClassAttdId;
            MyAttendance.m_TransationDb.ExecuteQuery(sql);
        }

        protected void Btn_DeleteMarking_Click(object sender, EventArgs e)
        {
            try
            {
                MyAttendance.CreateTansationDb();

                string sql = "DELETE FROM tblattdcls_std" + Hidden_StandardId.Value + "yr" + MyUser.CurrentBatchId + " WHERE Id=" + HiddenClassAttendance.Value;
                MyAttendance.m_TransationDb.ExecuteQuery(sql);

                sql = "DELETE FROM tblattdstud_std" + Hidden_StandardId.Value + "yr" + MyUser.CurrentBatchId + " WHERE classAttendanceId=" + HiddenClassAttendance.Value;
                MyAttendance.m_TransationDb.ExecuteQuery(sql);

                MyAttendance.EndSucessTansationDb();
                MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Attendance Canceled", "Attendance canceled for  " + Lbl_Class.Text, 1,1);

                if (MyAttendance.IsRollNoCofigEnabled())
                {

                    //ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "pagereload();", true);
                    Lbl_FinishMsg.Text = "Attandance for " + lbl_Date.Text + " has been cancelled successfully";
                    //Lbl_Link.Text = "MarkClassDailyAttendance.aspx?Start=" + DateConversion.GetDateFromText(HiddenDate.Value) + "&Resource=" + HiddenPeriodId.Value;
                    M_Finish.Show();
                }
                else
                {
                    Response.Redirect("MarkClassAttendanceMaster.aspx");
                }



            }
            catch
            {
                MyAttendance.EndFailTansationDb();
                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "pagereload();", true);
                Lbl_Link.Text = "MarkClassDailyAttendance.aspx?ClassAttendanceId=" + HiddenClassAttendance.Value;
                Lbl_FinishMsg.Text = "Error in cancelling";
                M_Finish.Show();

            }
        }

        protected void Grd_Students_RowEditing(object sender, GridViewEditEventArgs e)
        {
            
            Session["ViewerParameter"] = "";

            string Studentid = Grd_Students.DataKeys[e.NewEditIndex].Values["Id"].ToString();
            string StudentName = Grd_Students.DataKeys[e.NewEditIndex].Values["StudentName"].ToString();
            string PresentStatus = Grd_Students.DataKeys[e.NewEditIndex].Values["PresentStatus"].ToString();
            
            string _Datestr = HiddenDate.Value;
            if (Studentid != "" && _Datestr != "")
            {

                DateTime _Date = General.GetDateTimeFromText(_Datestr);
                string _parameter = GetStudentParameter(Studentid, _Date);
                string ReffereenceNo = MyAttendance.getStudentEnrollNo(Studentid);
                if (_parameter != "" && PresentStatus!="0")
                {
                    _parameter = GetCorrectParameter(_parameter);
                    _parameter = HiddenLink.Value + _parameter + "student=" + StudentName + "&rollno=" + ReffereenceNo + "&class=" + Lbl_Class.Text + "&exif=" + HiddenDate.Value;

                }
                else
                {
                    
                    _parameter = HiddenEmptyLink.Value + "student=" + StudentName + "&rollno=" + ReffereenceNo + "&class=" + Lbl_Class.Text + "&exif=" + HiddenDate.Value;
                }
                Session["ViewerParameter"] = _parameter;
                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"PictureViewer.aspx\");", true);
            }
            else
            {
                Lbl_msgAlert.Text = "No details show";
                MPE_MessageBox.Show();
            }
            

        }

        private string GetCorrectParameter(string _parameter)
        {
            
            string[] newstr = _parameter.Split(',');
            string _result = "pic=" + newstr[0] + "&rect=", seperator="";
            for (int i = 1; i < newstr.Length; i++)
            {

                _result = _result +seperator+ newstr[i];
                seperator = ",";
            }
            return _result+"&";
        }

        private string GetStudentParameter(string Studentid, DateTime _Date)
        {
            string _DimensionString = "";
            string sql = "SELECT tblexternalattencence.DimensionString FROM tblexternalattencence INNER JOIN tblexternalreff ON tblexternalreff.Id= tblexternalattencence.ExternalReffid AND tblexternalreff.UserType='STUDENT' WHERE tblexternalattencence.ActionStatus in (1,3) AND tblexternalreff.UserId=" + Studentid + " AND DATE(tblexternalattencence.ActionDate)='" + _Date.Date.ToString("s") + "' ORDER BY tblexternalattencence.counter DESC";
            MyReader= MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    _DimensionString = MyReader.GetValue(0).ToString();
                    if (_DimensionString != "")
                    {
                        break;
                    }
                }
            }

            return _DimensionString;
        }



    }
}
