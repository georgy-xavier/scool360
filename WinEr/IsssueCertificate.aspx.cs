using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;
using WinEr;
using WinBase;
using System.Text;
using System.Globalization;
using System.Threading;
public partial class IsssueCertificate : System.Web.UI.Page
{
    private StudentManagerClass MyStudMang;
    private KnowinUser MyUser;
    private Incident MyIncedent;
    private OdbcDataReader MyReader = null;
    private SchoolClass objSchool = null;
    protected void Page_Unload(object sender, EventArgs e)
    { 
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }

        MyUser = (KnowinUser)Session["UserObj"];
        MyStudMang = MyUser.GetStudentObj();
        MyIncedent = MyUser.GetIncedentObj();
        if (MyStudMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }
        else if (!MyUser.HaveActionRignt(36))
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
            }
            if (!IsPostBack)
            {
                string _MenuStr = "";

                if (Session["StudId"] == null)
                {
                    _MenuStr = MyStudMang.GetSubStudentMangMenuString(MyUser.UserRoleId, 3);
                    Getsession();
                    Hdn_QueryString.Value = "1";
                }
                else if (Session["StudId"] != null && Session["StudType"].ToString() == "1")
                {
                    _MenuStr = MyStudMang.GetSubStudentMangMenuString(MyUser.UserRoleId, int.Parse(Session["StudType"].ToString()));
                    Hdn_QueryString.Value = "2";
                    Text_session.Text = Session["StudId"].ToString();
                    int Batch = MyUser.CurrentBatchId;
                    Text_Batch.Text = Batch.ToString();
                }
                else //if (Session["StudId"] != null && Session["StudType"].ToString() == "2")
                {
                    _MenuStr = MyStudMang.GetSubStudentMangMenuString(MyUser.UserRoleId, int.Parse(Session["StudType"].ToString()));
                    Text_session.Text = Session["StudId"].ToString();
                    int Batch = MyUser.CurrentBatchId;
                    Text_Batch.Text = Batch.ToString();
                    //Hdn_QueryString.Value = "1";
                    Hdn_QueryString.Value = "3";
                }

             //   this.SubStudentMenu.InnerHtml = _MenuStr;
                // Getsession();
                Panel1.Visible = true;
                Panel2.Visible = false;
                //Btn_Cancel.Enabled = false;
                //Btn_Save.Enabled = false;
                CheckFeesDue(Text_session.Text);
                CheckTcTable(Text_session.Text);// checking whether the tc table has been created or not
                //LoadStudentTopData();
                LoadTCNumberConfigurations();
                //some initlization

            }
        }
    }
    private void LoadTCNumberConfigurations()
    {
        if (MyStudMang.IsDynamicTCNumber())
        {
            Lbl_TCNo.Visible = false;
            txt_TCNo.Text = "";
            txt_TCNo.Visible = false;
        }
        else
        {
            Lbl_TCNo.Visible = true;
            txt_TCNo.Text = "";
            txt_TCNo.Visible = true;

        }
    }
   

    //private void LoadStudentTopData()
    //{
    //    string _Studstrip = "";
    //    if (Session["StudType"] == null)
    //       // _Studstrip = MyStudMang.ToStripString(int.Parse(Text_session.Text), MyUser.GetImageUrl("StudentImage", int.Parse(Text_session.Text)), 3);
    //        _Studstrip = MyStudMang.ToStripString(int.Parse(Text_session.Text), "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Text_session.Text) + "&type=StudentImage", 3);
            
    //    else
    //        _Studstrip = MyStudMang.ToStripString(int.Parse(Text_session.Text), "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Text_session.Text) + "&type=StudentImage", int.Parse(Session["StudType"].ToString()));
       
    //    this.StudentTopStrip.InnerHtml = _Studstrip;
    //}

    private void Getsession()
    {
        if (Request.QueryString["StudId"].ToString()!=null && Request.QueryString["StudId"].ToString() != "")
        {
            try
            {
                int StudId = int.Parse(Request.QueryString["StudId"]);
                Text_session.Text = StudId.ToString();
                Text_Batch.Text = GetBatchID(Text_session.Text);
            }
            catch
            {

            }

        }
    }

    private string GetBatchID(string p)
    {
        string batchId = "";
        string sql = "select tblbatch.LastbatchId FROM tblbatch WHERE tblbatch.Id=" + MyUser.CurrentBatchId;
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            batchId = MyReader.GetValue(0).ToString();
        }
        MyReader.Close();
        return batchId;

    }

    private void CheckTcTable(string _session)
    {
        if (!HasTcTable(_session))
        {//if not created load available data from different tables
            loadSchoolDetails();
            LoadStudentDetails(_session);
            LoadCurrentStandard(_session);
            LoadFeeConcessions(_session);
        }
        else
        {//if ceated clear data and load available data from different tables
            //GetTcTableData(_session);
            //ClearTcData(_session);
            loadSchoolDetails();
            LoadStudentDetails(_session);
            LoadCurrentStandard(_session);
            LoadFeeConcessions(_session);
        }
    }

    private void ClearTcData(string _session)
    {
        string Sql = "DELETE FROM tbltc WHERE tbltc.StudentId =" + _session + " AND tbltc.Status=1";
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
    }

    private void GetTcTableData(string session)
    {
        DateTime Dob, DateOfAdmission, LastAttendance, AppTcDate, TcDate;
        string Sql = "SELECT * FROM tbltc WHERE StudentId=" + int.Parse(session) + " AND Status=1";
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
            Txt_PupilName.Text = MyReader.GetValue(3).ToString();
            Txt_SchoolName.Text = MyReader.GetValue(4).ToString();
            Txt_AdmissionNo.Text = MyReader.GetValue(5).ToString();
            Txt_Cumulative.Text = MyReader.GetValue(6).ToString();
            Txt_Sex.Text = MyReader.GetValue(7).ToString();
            Txt_NameOfFather.Text = MyReader.GetValue(8).ToString();
            Txt_Nationality.Text = MyReader.GetValue(9).ToString();
            Txt_Religion.Text = MyReader.GetValue(10).ToString();
            Txt_Cast.Text = GetCastFormId(int.Parse(MyReader.GetValue(11).ToString()));
            Txt_CastType.Text = MyReader.GetValue(12).ToString();
            
            Dob = DateTime.Parse(MyReader.GetValue(13).ToString());
            //Txt_Dob.Text = Dob.Date.ToString("MM-dd-yyyy");
            //Txt_Dob.Text = Dob.Date.ToString("dd/MM/yyyy");
            Txt_Dob.Text = Dob.Date.Day + "/" + Dob.Date.Month + "/" + Dob.Date.Year;
            Txt_CurrentStd.Text = MyReader.GetValue(14).ToString();
            Txt_LangStd.Text = MyReader.GetValue(15).ToString();
            Txt_MediumOfIns.Text = MyReader.GetValue(16).ToString();
            Txt_Syllabus.Text = MyReader.GetValue(17).ToString();

            DateOfAdmission = DateTime.Parse(MyReader.GetValue(18).ToString());
            //Txt_DateOfAdmission0.Text = DateOfAdmission.Date.ToString("MM-dd-yyyy");
            //Txt_DateOfAdmission0.Text = DateOfAdmission.Date.ToString("dd/MM/yyyy");
            Txt_DateOfAdmission0.Text = DateOfAdmission.Date.Day + "/" + DateOfAdmission.Date.Month + "/" + DateOfAdmission.Date.Year;
            Txt_Quali_Promo0.Text = MyReader.GetValue(19).ToString();
            Txt_Feesdue0.Text = MyReader.GetValue(20).ToString();
            Txt_FeeCon.Text = MyReader.GetValue(21).ToString();
            Txt_Scholarship.Text = MyReader.GetValue(22).ToString();
            Txt_MedicalyExmnd0.Text = MyReader.GetValue(23).ToString();

            LastAttendance = DateTime.Parse(MyReader.GetValue(24).ToString());
            //Txt_LastAttendance0.Text = LastAttendance.Date.ToString("MM-dd-yyyy");
            //Txt_LastAttendance0.Text = LastAttendance.Date.ToString("dd/MM/yyyy");
            Txt_LastAttendance0.Text = LastAttendance.Date.Day + "/" + LastAttendance.Date.Month + "/" + LastAttendance.Date.Year;

            
            AppTcDate = DateTime.Parse(MyReader.GetValue(25).ToString());
            //Txt_AppTcDate0.Text = AppTcDate.Date.ToString("MM-dd-yyyy");
            //Txt_AppTcDate0.Text = AppTcDate.Date.ToString("dd/MM/yyyy");
            Txt_AppTcDate0.Text = AppTcDate.Date.Day + "/" + AppTcDate.Date.Month + "/" + AppTcDate.Year;

            TcDate = DateTime.Parse(MyReader.GetValue(26).ToString());
            //Txt_TcDate0.Text = TcDate.Date.ToString("MM-dd-yyyy");
            //Txt_TcDate0.Text = TcDate.Date.ToString("dd/MM/yyyy");
            Txt_TcDate0.Text = TcDate.Date.Day + "/" + TcDate.Date.Month + "/" + TcDate.Year;


            Txt_TotalSchoolDays0.Text = MyReader.GetValue(27).ToString();
            Txt_DaysAttended0.Text = MyReader.GetValue(28).ToString();
            Txt_CC.Text = MyReader.GetValue(29).ToString();

           Txt_MotherName.Text= MyReader.GetValue(33).ToString();
           txt_Reson.Text= MyReader.GetValue(35).ToString();
           Txt_ResAddress.Text= MyReader.GetValue(34).ToString();
           Txt_subjects.Text= MyReader.GetValue(36).ToString();
           Txt_LastExamDetails.Text= MyReader.GetValue(37).ToString();
           Txt_newschoolName.Text = MyReader.GetValue(38).ToString();
            DateTime lastclsdate= DateTime.Parse(MyReader.GetValue(39).ToString());
           txt_LastClassDate.Text = lastclsdate.Date.Day + "/" + lastclsdate.Date.Month + "/" + lastclsdate.Date.Year; 

        }
    }

    private string GetCastFormId(int _CastId)
    {
        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
        TextInfo textInfo = cultureInfo.TextInfo;
        string _cast = "";
        string sql = "select tblcast.castname from tblcast where tblcast.Id=" + _CastId;
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            _cast = textInfo.ToTitleCase(MyReader.GetValue(0).ToString().ToLower());
        }
        return _cast;
    }

    private bool HasTcTable(string session)//checking whether the tc table has been created for the student or not
    {
        bool HasTC = false;
        string Sql = "SELECT TcNumber FROM tbltc WHERE StudentId=" + int.Parse(session);
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
            HasTC = true;
        }
        return HasTC;
    }

    private void CheckFeesDue(string _session)
    {
        int Feeflag = 0, BookFlag = 0;
        string _message = "";
        //if (!MyStudMang.HasFeesDue(int.Parse(_session)))
        //{
        //    Panel1.Visible = true;
        //}
        //else
        //{
        //    Panel2.Visible = true;
        //    Lbl_Message.Text = "The Student has not paid all the fees due to the school";
        //    //this.MPE_MessageBox.Show();
        //}

        if (!MyStudMang.HasFeesDue(int.Parse(_session)))  //|| (!MyStudMang.HasPendingbooks(int.Parse(_session))))
        {
            // Panel1.Visible = true;
            Feeflag = 1;

        }
        if (!MyStudMang.HasPendingbooks(int.Parse(_session)))
        {
            BookFlag = 1;
        }
        if (Feeflag == 0)
        {
            _message = _message + "The student has not paid all the fees due";
            Panel1.Visible = false;
            Panel2.Visible = true;

        }
        if (BookFlag == 0)
        {
            _message = _message + " The student has not returned all the books";
            Panel1.Visible = false;
            Panel2.Visible = true;
        }

        Lbl_Message.Text = _message;



    }

    private void LoadFeeConcessions(string _session)
    {
        string Seperator = "";
        string FeeConcessios = "";
        string Period = "";
        string Fee = "";
        string Sql = "select tblfeeaccount.AccountName, tblperiod.Period from tbltransaction inner join tblfeeschedule on tblfeeschedule.Id= tbltransaction.PaymentElementId inner join tblfeeaccount on tblfeeaccount.Id= tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId where tbltransaction.Canceled=0 AND tbltransaction.UserId=" + int.Parse(_session) + " AND tbltransaction.AccountTo=3";
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                FeeConcessios = FeeConcessios + Seperator + MyReader.GetValue(0).ToString() ;
                Period = Period + Seperator + MyReader.GetValue(1).ToString();
                Seperator = ", ";
            }
            Fee = "Concessions are given for " + FeeConcessios + " In the period of " + Period ;
            if (Seperator != "")
            {
                Fee = Fee + " respectively";
            }
        }
        else
        {
            Fee = "No";
        }
        Txt_FeeCon.Text = Fee; 
    }

    private void LoadCurrentStandard(string _session)
    {
        //string Sql = "SELECT tblstudent.StudentName,tblstudentclassmap.Standard FROM tblstudent INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId= tblstudent.Id WHERE tblstudent.Id=" + int.Parse(_session) + " AND tblstudentclassmap.BatchId=" +int.Parse(Text_Batch.Text.ToString());
        string Sql = "SELECT tblstandard.Name FROM tblstudentclassmap INNER JOIN tblstandard ON tblstudentclassmap.Standard = tblstandard.Id WHERE tblstudentclassmap.BatchId=" + int.Parse(Text_Batch.Text.ToString()) + " AND tblstudentclassmap.StudentId=" + int.Parse(_session);
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
            MyReader.Read();
            Txt_CurrentStd.Text = MyReader.GetValue(0).ToString();
            MyReader.Close();
        }
    }

    private void LoadStudentDetails(string _session)
    {
        DateTime Date_AdmissionDate, Date_Dob;
        string sql = "";
        if (Hdn_QueryString.Value == "2")
        {
            sql = "SELECT tblstudent.AdmitionNo,tblstudent.StudentName,tblstudent.Sex,tblstudent.GardianName,tblstudent.Nationality,tblstudent.Religion,tblstudent.Cast,tblstudent.DOB,tblstudent.DateofJoining, tblstudentclassmap.ClassId,tblstudent.MothersName,tblstudent.Addresspresent  FROM tblstudent inner join tblstudentclassmap on tblstudent.Id= tblstudentclassmap.StudentId WHERE tblstudent.Id=" + int.Parse(_session);
        }
        else if (Hdn_QueryString.Value == "1")//If from promotion page
        {
            sql = "SELECT tblstudent.AdmitionNo,tblstudent.StudentName,tblstudent.Sex,tblstudent.GardianName,tblstudent.Nationality,tblstudent.Religion,tblstudent.Cast,tblstudent.DOB,tblstudent.DateofJoining, tblstudentclassmap_history.ClassId,tblstudent.MothersName,tblstudent.Addresspresent FROM tblstudent inner join tblstudentclassmap_history on tblstudent.Id= tblstudentclassmap_history.StudentId WHERE tblstudent.Id=" + int.Parse(_session);
        }
        else if (Hdn_QueryString.Value == "3")
        {
            sql = "SELECT tblstudent_history.AdmitionNo,tblstudent_history.StudentName,tblstudent_history.Sex,tblstudent_history.GardianName,tblstudent_history.Nationality,tblstudent_history.Religion,tblstudent_history.Cast,tblstudent_history.DOB,tblstudent_history.DateofJoining, tblstudent_history.ClassId,tblstudent_history.MothersName,tblstudent_history.Addresspresent  FROM tblstudent_history WHERE tblstudent_history.Id=" + int.Parse(_session);

        }
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            Txt_Feesdue0.Text = "Yes";
            Txt_AdmissionNo.Text = MyReader.GetValue(0).ToString();
            Txt_PupilName.Text = textInfo.ToTitleCase(MyReader.GetValue(1).ToString().ToLower());
            Txt_Sex.Text = textInfo.ToTitleCase(MyReader.GetValue(2).ToString().ToLower());
            Txt_NameOfFather.Text =textInfo.ToTitleCase(MyReader.GetValue(3).ToString().ToLower());
            
            Txt_Nationality.Text = textInfo.ToTitleCase(MyReader.GetValue(4).ToString().ToLower());
            Txt_Cast.Text = textInfo.ToTitleCase(MyReader.GetValue(6).ToString().ToLower());
            Date_Dob = DateTime.Parse(MyReader.GetValue(7).ToString());
            Txt_Dob.Text = Date_Dob.Date.Day + "/" + Date_Dob.Date.Month + "/" + Date_Dob.Date.Year;
            Date_AdmissionDate = DateTime.Parse(MyReader.GetValue(8).ToString());
            Txt_DateOfAdmission0.Text = Date_AdmissionDate.Date.Day + "/" + Date_AdmissionDate.Date.Month + "/" + Date_AdmissionDate.Date.Year;

            int _ClassId = int.Parse(MyReader.GetValue(9).ToString());
            Txt_Quali_Promo0.Text = "Yes";
            Txt_Scholarship.Text = "No";
            Txt_MedicalyExmnd0.Text = "No";
            Txt_CC.Text = "Good";
            DateTime _TodayDate = System.DateTime.Today;
            //Txt_AppTcDate0.Text = _TodayDate.ToString("MM/dd/yyyy");
            //Txt_TcDate0.Text = _TodayDate.ToString("MM/dd/yyyy");
            Txt_AppTcDate0.Text = _TodayDate.Date.Day + "/" + _TodayDate.Date.Month + "/" + _TodayDate.Date.Year;
            //Txt_TcDate0.Text = _TodayDate.ToString("MM/dd/yyyy");
            Txt_TcDate0.Text = _TodayDate.Date.Day + "/" + _TodayDate.Date.Month + "/" + _TodayDate.Date.Year;
            Txt_MotherName.Text = textInfo.ToTitleCase(MyReader.GetValue(10).ToString().ToLower());
            Txt_ResAddress.Text = textInfo.ToTitleCase(MyReader.GetValue(11).ToString().ToLower());

            Txt_Religion.Text = GetReligionName(int.Parse(MyReader.GetValue(5).ToString()));
            Txt_Cast.Text = GetCastFormId(int.Parse(Txt_Cast.Text.ToString()));
            if (Txt_Cast.Text == "SC" || Txt_Cast.Text == "ST")
            {
                Txt_CastType.Text = textInfo.ToTitleCase(Txt_Cast.Text.ToLower());
            }
            else
            {
                Txt_CastType.Text = "None";
            }
            
           LoadStudentAttendanceDetails(_session, _ClassId, Txt_DateOfAdmission0.Text.Trim(), Txt_TcDate0.Text.Trim());
            //if (Hdn_QueryString.Value != "3")
            //{
            StudentAttendanceDetails(_session, _ClassId, Txt_DateOfAdmission0.Text.Trim(), Txt_TcDate0.Text.Trim(), Hdn_QueryString.Value);
            //}
            //else
            //{

               // StudentAttendanceDetailsAlumini(_session, _ClassId, Txt_DateOfAdmission0.Text.Trim(), Txt_TcDate0.Text.Trim(), Hdn_QueryString.Value);
            //}
            MyReader.Close();
        }
    }
    

    private void StudentAttendanceDetails(string _session, int _ClassId, string _StartDate, string _EndDate,string hdnquerystring)
    {
        int _WorkingDays = 0, _AbsentDays = 0, _PresentDays = 0, batch=0, std;
        DateTime _LastPresentDate = new DateTime();
        DateTime newDate = new DateTime();
        _PresentDays = _WorkingDays - _AbsentDays;

        batch = MyUser.CurrentBatchId;
        if (hdnquerystring != "3")
        {
            if (MyUser.HaveModule(21))
            {
                Attendance MyAttendance;
                MyAttendance = MyUser.GetAttendancetObj();
                int _no_workingdays, _no_presentdays, _no_absentdays, _no_holidays, _no_halfdays;
                double _attendencepersent;
                std = MyStudMang.GetLastStandard(_session);


                batch = MyAttendance.getWrkingBatch(_session, std);


                if (MyUser.CurrentBatchId == batch)
                {
                    MyAttendance.GetCurrentBatchNewattendanceDetails(int.Parse(_session), out _no_workingdays, out _no_presentdays, out _no_absentdays, out _no_holidays, out _no_halfdays, out _attendencepersent, batch);
                    int.TryParse(_no_workingdays.ToString(), out _WorkingDays);
                    int.TryParse(_no_presentdays.ToString(), out _PresentDays);
                    if (MyAttendance.AttendanceTables_Exits(std.ToString(), batch))
                    {
                        _LastPresentDate = MyAttendance.GetLastAttendDay(_session, _ClassId, std, batch);
                    }
                }
                else
                {
                    int classid = 0;
                    classid = MyAttendance.LastClassId(_session);
                    MyAttendance.GetPrevBatchNewattendanceDetails(int.Parse(_session), out _no_workingdays, out _no_presentdays, classid, batch);
                    int.TryParse(_no_workingdays.ToString(), out _WorkingDays);
                    int.TryParse(_no_presentdays.ToString(), out _PresentDays);
                    if (MyAttendance.AttendanceTables_Exits(std.ToString(), batch))
                    {
                        _LastPresentDate = MyAttendance.GetLastAttendDay(_session, _ClassId, std, batch);
                    }
                }
            }
        }
        
        Txt_TotalSchoolDays0.Text = _WorkingDays.ToString();
        Txt_DaysAttended0.Text = _PresentDays.ToString();
        if (_LastPresentDate != newDate)
        {
            Txt_LastAttendance0.Text = _LastPresentDate.Date.Day + "/" + _LastPresentDate.Date.Month + "/" + _LastPresentDate.Date.Year;
        }
        else
        {
            Txt_LastAttendance0.Text = "";
        }
    }
    private void LoadStudentAttendanceDetails(string _session, int _ClassId, string _StartDate, string _EndDate)
    {
        try
        {

            int _WorkingDays = 0, _AbsentDays = 0, _PresentDays = 0;
            DateTime _LastPresentDate = new DateTime();
            _WorkingDays = GetNoOfWorkingDays(_ClassId, _StartDate, _EndDate);
            _AbsentDays = GetNoOfAbsentDays(_ClassId, _session, _StartDate, _EndDate);
            _PresentDays = _WorkingDays - _AbsentDays;

            Txt_TotalSchoolDays0.Text = _WorkingDays.ToString();
            Txt_DaysAttended0.Text = _PresentDays.ToString();

            string sql = "SELECT max(tblmasterdate.`date`) from tblmasterdate inner join tbldate on tblmasterdate.Id= tbldate.DateId where tbldate.classId=" + _ClassId + " AND tbldate.Id not in (SELECT tblstudentattendance.DayId from tblstudentattendance where tblstudentattendance.StudentId=" + _session + ")";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _LastPresentDate = DateTime.Parse(MyReader.GetValue(0).ToString());
            }
            //Txt_LastAttendance0.Text = _LastPresentDate.Date.ToString("MM/dd/yyyy");
            //Txt_LastAttendance0.Text = _LastPresentDate.Date.ToString("dd/MM/yyyy");
            Txt_LastAttendance0.Text = _LastPresentDate.Date.Day + "/" + _LastPresentDate.Date.Month + "/" + _LastPresentDate.Date.Year;


        }
        catch
        {
            DateTime _TodayDate = System.DateTime.Today;
            //Txt_LastAttendance0.Text = _TodayDate.ToString("MM/dd/yyyy");
            Txt_LastAttendance0.Text = _TodayDate.Date.Day + "/" + _TodayDate.Date.Month + "/" + _TodayDate.Date.Year;

        }
    }

    private int GetNoOfAbsentDays(int _ClassId, string _session, string _StartDate, string _EndDate)
    {
        int _TotalabsentDay = 0;
        string sql = "select count(tblstudentattendance.Id) from tblstudentattendance inner join  tbldate on tblstudentattendance.DayId= tbldate.Id  WHERE tbldate.`Status`='class' AND tbldate.classId=" + _ClassId + " And tblstudentattendance.StudentId=" + _session;
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            _TotalabsentDay = int.Parse(MyReader.GetValue(0).ToString());

        }
        return _TotalabsentDay;
    }

    private int GetNoOfWorkingDays(int _ClassId, string _StartDate, string _EndDate)
    {
        int _TotalworkingDay = 0;
        string Sql = "select count(tbldate.Id) from tbldate   WHERE tbldate.`Status`='class' AND tbldate.classId=" + _ClassId;
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
            _TotalworkingDay = int.Parse(MyReader.GetValue(0).ToString());

        }
        return _TotalworkingDay;
    }

    private string GetReligionName(int _ReligionId)
    {
        string _Religion = "";
        string sql = "SELECT tblreligion.Religion from tblreligion WHERE tblreligion.Id=" + _ReligionId;
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            _Religion = MyReader.GetValue(0).ToString();
        }
        return _Religion;
    }

    private void loadSchoolDetails()
    {
        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
        string sql = "SELECT SchoolName,Syllabus,MediumofInstruction FROM tblschooldetails";
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            MyReader.Read();
            Txt_SchoolName.Text = textInfo.ToTitleCase(MyReader.GetValue(0).ToString().ToLower());
            Txt_Syllabus.Text = textInfo.ToTitleCase(MyReader.GetValue(1).ToString());
            Txt_MediumOfIns.Text = textInfo.ToTitleCase(MyReader.GetValue(2).ToString().ToLower());
            MyReader.Close();
        }

    }

    protected void TextBox4_TextChanged(object sender, EventArgs e)
    {

    }

    protected void Btn_ClearFeeDue_Click(object sender, EventArgs e)
    {
        Panel2.Visible = false;
        Panel1.Visible = true;
    }

    protected void Btn_ProCancel_Click(object sender, EventArgs e)
    {

        if (Session["StudId"] == null)
        {
            Response.Redirect("PromotStudents.aspx");
        }
        else
        {
            Response.Redirect("SearchStudent.aspx");
        }
    }

    protected void Btn_Redirect_Click(object sender, EventArgs e)
    {
        if (Session["StudId"] == null)
        {
            Response.Redirect("PromotStudents.aspx");
        }
        else
        {
            Response.Redirect("SearchStudent.aspx");
        }
    }

    protected void Btn_magok_Click(object sender, EventArgs e)
    {
        MPE_MessageBox.Hide();
    }

    protected void Img_Export_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Message1.Text = "";
        string SchoolName, School;
        
        MyUser.m_DbLog.LogToDb(MyUser.UserName, "TC Issued", "Issue TC for " + Txt_PupilName.Text, 1);

        if (Txt_LastAttendance0.Text.Trim() == "" || Txt_AppTcDate0.Text.Trim() == "" || Txt_TcDate0.Text.Trim() == "")// || Txt_TotalSchoolDays0.Text.Trim() == "" || Txt_DaysAttended0.Text.Trim() == "")
        {
            Lbl_msg.Text = "One or more Fields are empty";
            this.MPE_MessageBox.Show();
        }
        else
        {
            if (ValidTcNumber())
            {
                SchoolName = Txt_SchoolName.Text;
                School = SchoolName.Replace("\'", "\\'");
                MyStudMang.GenerateTC(Txt_PupilName.Text, School, Txt_AdmissionNo.Text, Txt_Cumulative.Text, Txt_Sex.Text, Txt_NameOfFather.Text, Txt_Nationality.Text, Txt_Religion.Text, Txt_Cast.Text, Txt_CastType.Text, Txt_Dob.Text, Txt_CurrentStd.Text, Txt_LangStd.Text, Txt_MediumOfIns.Text, Txt_Syllabus.Text, Txt_DateOfAdmission0.Text, Txt_Quali_Promo0.Text, Txt_Feesdue0.Text, Txt_FeeCon.Text, Txt_Scholarship.Text, Txt_MedicalyExmnd0.Text, Txt_LastAttendance0.Text, Txt_AppTcDate0.Text, Txt_TcDate0.Text, Txt_TotalSchoolDays0.Text, Txt_DaysAttended0.Text, Txt_CC.Text, int.Parse(Text_session.Text.ToString()), int.Parse(Text_Batch.Text.ToString()), Txt_MotherName.Text, Txt_ResAddress.Text, txt_Reson.Text, Txt_subjects.Text, Txt_LastExamDetails.Text, txt_TCNo.Text.Trim(), Txt_newschoolName.Text, txt_LastClassDate.Text, Txt_enrollment.Text);

                //Page.RegisterStartupScript("keyClientBlock", "<script language=JavaScript>window.open(\"Tc.aspx?StudId=" + int.Parse(Text_session.Text) + "\")</script>");
                //Response.Write("<script language=JavaScript>window.open(\"Tc.aspx?StudId=" + int.Parse(Text_session.Text) + "\")</script>");


                // ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "<script language=JavaScript>window.open(\"Tc.aspx?StudId=" + int.Parse(Text_session.Text) + "\")</script>");

                string Err = "";
                StringBuilder _GeneratedTC = MyStudMang.GenerateDynamicTC(int.Parse(Text_session.Text.ToString()), objSchool, out Err);
                if (_GeneratedTC.ToString() != "")
                {
                    Session["StudTc"] = _GeneratedTC.ToString();
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "<script language=JavaScript>window.open(\"ShowDynamicTc.aspx?StudId=" + int.Parse(Text_session.Text) + "\")</script>");
                    if (int.Parse(Session["StudType"].ToString()) != 2)
                    {
                        MPE_HistoryConfirm.Show();
                    }
                    else
                    {
                        MPE_HistoryConfirm.Show();
                    }
                }
                else
                {
                    Lbl_msg.Text = Err;
                    MPE_MessageBox.Show();
                }

                if (int.Parse(Session["StudType"].ToString()) != 2)
                {
                    MPE_HistoryConfirm.Show();
                }
            }
            else
            {
                Lbl_msg.Text = "Entered TC number already exist.";
                MPE_MessageBox.Show();
            }

        }
          



    }

    //private StringBuilder GenerateDynamicTC()
    //{
    //     StringBuilder _TC = new StringBuilder();
    //        string sql = "select tbltcformat.TCFormat from tbltcformat ";
    //        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);

    //        if (!MyReader.HasRows)
    //        {
    //            //dominic show error mseeage
    //            Lbl_msg.Text = "Tc Format is not available.Create a TC format for the institution before create TC";
    //            _TC.Append("");
    //            MPE_MessageBox.Show();
    //        }
    //        else
    //        {
    //            _TC.Append(MyReader.GetValue(0).ToString());
    //            _TC = LoadLogo(_TC);
    //            _TC = LoadSchoolDetails(_TC);
    //            _TC = LoadStudentDetails(int.Parse(Text_session.Text.ToString()), _TC);
    //        }
    //        return _TC;

    //}



    //private StringBuilder LoadStudentDetails(int StudId, StringBuilder _TC)
    //{
    //    DateTime Date_Dob, Date_LastAttendance, Date_TcRcvedDate, Date_TCIssue;//, Date_DteOFAdmission;

    //    String Sql = "SELECT * FROM tbltc WHERE StudentId=" + StudId;
    //    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
    //    if (MyReader.HasRows)
    //    {
    //        _TC.Replace("[$TCNo$]", MyReader.GetValue(2).ToString());
    //        _TC.Replace("[$StudName$]", MyReader.GetValue(3).ToString());
    //        _TC.Replace("[$SchoolName$]", MyReader.GetValue(4).ToString());
    //        _TC.Replace("[$AdmissionNo$]", MyReader.GetValue(5).ToString());
    //        _TC.Replace("[$CumulativeNo$]", MyReader.GetValue(6).ToString());
    //        _TC.Replace("[$Sex$]", MyReader.GetValue(7).ToString());
    //        _TC.Replace("[$FatherName$]", MyReader.GetValue(8).ToString());
    //        _TC.Replace("[$MotherName$]", MyReader.GetValue(33).ToString());

    //        Date_Dob = DateTime.Parse(MyReader.GetValue(13).ToString());
    //        _TC.Replace("[$DOB$]", Date_Dob.Date.ToString("dd-MM-yyyy"));

    //        _TC.Replace("[$ResAddress$]", MyReader.GetValue(34).ToString());
    //        _TC.Replace("[$Nationality$]", MyReader.GetValue(9).ToString());
    //        _TC.Replace("[$Religion$]", MyReader.GetValue(10).ToString());
    //        _TC.Replace("[$Caste$]", MyReader.GetValue(11).ToString());
    //        _TC.Replace("[$SCST$]", MyReader.GetValue(12).ToString());
    //        _TC.Replace("[$LastStandard$]", MyReader.GetValue(14).ToString());
    //        _TC.Replace("[$LanguageStudied$]", MyReader.GetValue(15).ToString());
    //        _TC.Replace("[$SubjectStudied$]", MyReader.GetValue(36).ToString());
    //        _TC.Replace("[$Medium$]", MyReader.GetValue(16).ToString());

    //        _TC.Replace("[$Syllabus$]", MyReader.GetValue(17).ToString());
    //        DateTime AdmissionDate = DateTime.Parse(MyReader.GetValue(18).ToString());
    //        _TC.Replace("[$DoA$]", AdmissionDate.Date.ToString("dd-MM-yyyy"));
    //        _TC.Replace("[$Result$]", MyReader.GetValue(19).ToString());
    //        _TC.Replace("[$Fee$]", MyReader.GetValue(20).ToString());
    //        _TC.Replace("[$FeeCon$]", MyReader.GetValue(21).ToString());
    //        _TC.Replace("[$Scholarship$]", MyReader.GetValue(22).ToString());
    //        _TC.Replace("[$Medical$]", MyReader.GetValue(23).ToString());
    //        Date_LastAttendance = DateTime.Parse(MyReader.GetValue(24).ToString());
    //        _TC.Replace("[$LastDate$]", Date_LastAttendance.Date.ToString("dd-MM-yyyy"));
    //        Date_TcRcvedDate = DateTime.Parse(MyReader.GetValue(25).ToString());
    //        _TC.Replace("[$DateOfTCApp$]", Date_TcRcvedDate.Date.ToString("dd-MM-yyyy"));


    //        Date_TCIssue = DateTime.Parse(MyReader.GetValue(26).ToString());
    //        _TC.Replace("[$DateOfTCIssue$]", Date_TCIssue.Date.ToString("dd-MM-yyyy"));
    //        _TC.Replace("[$TotalDays$]", MyReader.GetValue(27).ToString());
    //        _TC.Replace("[$NoOfAttendance$]", MyReader.GetValue(28).ToString());
    //        _TC.Replace("[$Reason$]", MyReader.GetValue(35).ToString());
    //        _TC.Replace("[$LastExamDetails$]", MyReader.GetValue(37).ToString());
    //        _TC.Replace("[$Character$]", MyReader.GetValue(29).ToString());

    //    }
    //    return _TC;
    //}

    //private StringBuilder LoadLogo(StringBuilder _TC)
    //{//dominic test the image name 
    //    String ImageUrl = "";
    //    //String ImageUrl = "";
    //    String Sql = "SELECT  LogoUrl FROM tblschooldetails WHERE Id=1";
    //    OdbcDataReader MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
       
    //    if (MyReader.HasRows)
    //    {
    //        ImageUrl = MyReader.GetValue(0).ToString();
    //    }
    //    else
    //    {
    //        ImageUrl = "img.png";
    //    }

    //    string _image = " <img src=\"ThumbnailImages/" + ImageUrl + "\" alt=\" \" height=\"120px\" width=\"125px\" />";

    //    _TC.Replace("LOGO : [$Logo$]", _image);

    //    return _TC;

    //}

    //private StringBuilder LoadSchoolDetails(StringBuilder _TC)
    //{
    //    String Sql = "SELECT SchoolName,Address FROM tblschooldetails WHERE Id=1";
                
    //    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);

    //    if (MyReader.HasRows)
    //    {
    //        _TC.Replace("[$InstitutionName$]", MyReader.GetValue(0).ToString());
    //        _TC.Replace("[$Address$]", MyReader.GetValue(1).ToString());

    //    }
    //    return _TC;
    //}

    protected void Img_PdfExport_Click(object sender, ImageClickEventArgs e)
    {
        
        Lbl_Message1.Text = "";
        string SchoolName, School;

        if (Txt_LastAttendance0.Text.Trim() == "" || Txt_AppTcDate0.Text.Trim() == "" || Txt_TcDate0.Text.Trim() == "")// || Txt_TotalSchoolDays0.Text.Trim() == "" || Txt_DaysAttended0.Text.Trim() == "")
        {
            Lbl_msg.Text = "One or more Fields are empty";
            this.MPE_MessageBox.Show();
        }
        else
        {
            SchoolName = Txt_SchoolName.Text;
            School = SchoolName.Replace("\'", "\\'");
            if (ValidTcNumber())
            {
                MyStudMang.GenerateTC(Txt_PupilName.Text, School, Txt_AdmissionNo.Text, Txt_Cumulative.Text, Txt_Sex.Text, Txt_NameOfFather.Text, Txt_Nationality.Text, Txt_Religion.Text, Txt_Cast.Text, Txt_CastType.Text, Txt_Dob.Text, Txt_CurrentStd.Text, Txt_LangStd.Text, Txt_MediumOfIns.Text, Txt_Syllabus.Text, Txt_DateOfAdmission0.Text, Txt_Quali_Promo0.Text, Txt_Feesdue0.Text, Txt_FeeCon.Text, Txt_Scholarship.Text, Txt_MedicalyExmnd0.Text, Txt_LastAttendance0.Text, Txt_AppTcDate0.Text, Txt_TcDate0.Text, Txt_TotalSchoolDays0.Text, Txt_DaysAttended0.Text, Txt_CC.Text, int.Parse(Text_session.Text.ToString()), int.Parse(Text_Batch.Text.ToString()), Txt_MotherName.Text, Txt_ResAddress.Text, txt_Reson.Text, Txt_subjects.Text, Txt_LastExamDetails.Text, txt_TCNo.Text.Trim(), Txt_newschoolName.Text, txt_LastClassDate.Text, Txt_enrollment.Text);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "TC Issued", "Issue TC for " + Txt_PupilName.Text, 1);

                //Page.RegisterStartupScript("keyClientBlock", "<script language=JavaScript>window.open(\"Tc.aspx?StudId=" + int.Parse(Text_session.Text) + "\")</script>");
                //Response.Write("<script language=JavaScript>window.open(\"Tc.aspx?StudId=" + int.Parse(Text_session.Text) + "\")</script>");
                //Btn_Cancel.Enabled = true;
                //Btn_Save.Enabled = true;

                TCpdf MyPdf = new TCpdf(MyStudMang.m_MysqlDb, objSchool);
                string _ErrorMsg = "";
                string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
                string _PdfName = "";
                int _StudentId = int.Parse(Text_session.Text);
                if (MyPdf.GenerateStudentTCPdf(_StudentId, _physicalpath, out _PdfName, out _ErrorMsg))
                {
                    _ErrorMsg = "";
                    //ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                    //Lbl_msg.Text = _ErrorMsg;
                    //MPE_MessageBox.Show();

                    MPE_HistoryConfirm.Show();
                }
                else
                {
                    // _ErrorMsg = "Faild To Create";
                    //Lbl_msg.Text = _ErrorMsg;
                    //MPE_MessageBox.Show();
                }
            }
            else
            {
                Lbl_msg.Text = "Entered TC number already exist.";
                MPE_MessageBox.Show();
            }
        }
    }

    private bool ValidTcNumber()
    {
        if (!MyStudMang.IsDynamicTCNumber())
        {
            string TcNumber = txt_TCNo.Text.ToString();
            string sql = "select tbltc.Id from tbltc where tbltc.TcNumber='" + TcNumber + "'";

            if (MyStudMang.m_TransationDb != null)
                MyReader = MyStudMang.m_TransationDb.ExecuteQuery(sql);
            else
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                return false;
            }
            return true;
        }
        return true;
    }

    private void UpdateTripAssignedToStudents(int _StudentId)
    {
        string sql = "";
        sql = "select tbl_tr_studtripmap.ToTripId, tbl_tr_studtripmap.FromTripId from tbl_tr_studtripmap where tbl_tr_studtripmap.StudId=" + _StudentId + " and (tbl_tr_studtripmap.FromTripId<>0 or tbl_tr_studtripmap.ToTripId<>0)";
        MyReader = MyStudMang.m_TransationDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            int fromtripId = int.Parse(MyReader.GetValue(1).ToString());
            int totripId = int.Parse(MyReader.GetValue(0).ToString());
            sql = "delete from tbl_tr_studtripmap where tbl_tr_studtripmap.StudId=" + _StudentId + "";
            MyStudMang.m_TransationDb.ExecuteQuery(sql);
            if (fromtripId != 0)
            {
                sql = "select Occupied from tbl_tr_trips where tbl_tr_trips.Id=" + fromtripId + "";
                MyReader = MyStudMang.m_TransationDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    int newoccupied = int.Parse(MyReader.GetValue(0).ToString()) - 1;
                    sql = "Update tbl_tr_trips set Occupied=" + newoccupied + " where tbl_tr_trips.Id=" + fromtripId + "";
                    MyStudMang.m_TransationDb.ExecuteQuery(sql);
                }

            }
            if (totripId != 0)
            {
                sql = "select Occupied from tbl_tr_trips where tbl_tr_trips.Id=" + totripId + "";
                MyReader = MyStudMang.m_TransationDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    int newoccupied = int.Parse(MyReader.GetValue(0).ToString()) - 1;
                    sql = "Update tbl_tr_trips set Occupied=" + newoccupied + " where tbl_tr_trips.Id=" + totripId + "";
                    MyStudMang.m_TransationDb.ExecuteQuery(sql);
                }

            }
        }
    }

    protected void Btn_History_Yes_Click(object sender, EventArgs e)
    {
        try
        {
            int _StudentId = int.Parse(Text_session.Text);
            MyIncedent.CreateApprovedIncedent("Student TC Given", "TC is Given to the student  " + Txt_PupilName.Text + ", on " + General.GerFormatedDatVal(DateTime.Now), General.GerFormatedDatVal(DateTime.Now), 1, MyUser.UserId, "student", _StudentId, 3, 0, MyUser.CurrentBatchId, MyStudMang.GetClassId(_StudentId));
            MyStudMang.CreateTansationDb();
            MyStudMang.StoreIncedentCalcualtion(int.Parse(Text_session.Text), MyUser.CurrentBatchId);
            MyStudMang.MoveStudentIncidentToHistory(int.Parse(Text_session.Text));
            MyStudMang.MoveStudentToHistory(int.Parse(Text_session.Text), MyUser.CurrentBatchId, 3);
            UpdateTripAssignedToStudents(_StudentId);
            MyStudMang.UpdateInventoryDetails(_StudentId);
           // MyStudMang.ChangeStudentToHistory(int.Parse(Text_session.Text), MyUser.CurrentBatchId);
            //MyStudMang.MoveStudentIncidentToHistory(int.Parse(Text_session.Text));
            MyStudMang.EndSucessTansationDb();
            MyUser.UpdateGroupMapDetails(_StudentId, 0);
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Student Moved to History", Txt_PupilName.Text + " has been transfered to history", 1);

            this.MPE_MessageBox.Show();
            Lbl_msg.Text = "Student has been transfered to history";
            Btn_magok.Visible = false;

            Btn_Redirect.Visible = true;
            this.MPE_MessageBox.Show();

        }
        catch (Exception exc)
        {
            MyStudMang.EndFailTansationDb();

            this.MPE_MessageBox.Show();
            Lbl_msg.Text = exc.Message;
        }
    }

    protected void Btn_History_No_Click(object sender, EventArgs e)
    {
        string Sql = "DELETE FROM tbltc WHERE StudentId=" + int.Parse(Text_session.Text) + " AND Status=1";
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
        Lbl_msg.Text = "The action is canceled. You can regenerate TC later";
        this.MPE_MessageBox.Show();
    }
}