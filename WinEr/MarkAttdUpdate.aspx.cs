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
using WinBase;
using System.Data.Odbc;

namespace WinEr
{
    public partial class MarkAttdUpdate : System.Web.UI.Page
    {

        private ClassOrganiser MyClassMang;
        private StudentManagerClass MyStudent;
        private Attendance MyAttendance;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudent = MyUser.GetStudentObj();
            MyClassMang = MyUser.GetClassObj();     
            MyAttendance = MyUser.GetAttendancetObj();
            if (MyStudent == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MyClassMang == null)
            {
                Response.Redirect("sectionerr.htm");
                //no rights for this user.
            }
            else if (MyAttendance == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            //else if (!MyUser.HaveActionRignt(632))
            //{
            //    Response.Redirect("RoleErr.htm");
            //}
            else
            {
                if (!IsPostBack)
                {
                    bool Valid = false;
                    //Start=2012-08-06T00:00:00&Resource=95
                    if (Request.QueryString["Id"] != null)
                    {
                        string Id = Request.QueryString["Id"].ToString();
                        string[] StrArray = Id.Split('$');
                        Hd_DateString.Value = StrArray[0];
                        Hd_ClassId.Value = StrArray[1];
                        Hd_PeriodId.Value = StrArray[2];
                        Hd_HolidayStatus.Value = StrArray[3];
                        Valid = true;

                    }
                    else if (Request.QueryString["Start"] != null)
                    {
                        string Start = Request.QueryString["Start"].ToString();
                        DateTime _Date = DateTime.Parse(Start);
                        if (Request.QueryString["Resource"] != null)
                        {
                            string Resource = Request.QueryString["Resource"];
                            Hd_DateString.Value = General.GerFormatedDatVal(_Date);
                            Hd_ClassId.Value = MyStudent.GetClassId(int.Parse(Resource),MyUser.CurrentBatchId).ToString();
                            Hd_PeriodId.Value = Resource;
                            Hd_HolidayStatus.Value = "";
                            Valid = true;
                        }
                    }

                    if (Valid)
                    {

                        LoadStudentDetails();

                    }



                }


            }
        }

        private void LoadStudentDetails()
        {
            Lbl_ClassName.Text = MyClassMang.GetClassname(int.Parse(Hd_ClassId.Value));
            Lbl_StudentName.Text = MyStudent.GetStudentName(int.Parse(Hd_PeriodId.Value));
            Lbl_Date.Text = Hd_DateString.Value;
            int ss = MyAttendance.GetAttendanceStatus_Student(General.GetDateTimeFromText(Hd_DateString.Value), int.Parse(Hd_ClassId.Value), MyClassMang.GetStandardIdfromClassId(Hd_ClassId.Value), MyUser.CurrentBatchId, Hd_PeriodId.Value);
            Drp_PresentStatus.SelectedValue = ss.ToString();
        }


        protected void Btn_Update_Click(object sender, EventArgs e)
        {

            DateTime _Date = General.GetDateTimeFromText(Hd_DateString.Value);
            if (_Date.Date <= DateTime.Now.Date)
            {
                if (Drp_PresentStatus.SelectedValue != "-1")
                {
                    try
                    {
                        int ClassAttendanceId = 0;
                        string MarkedStatus = "", StandardId = MyClassMang.GetStandardIdfromClassId(Hd_ClassId.Value);
                        int ClassId = int.Parse(Hd_ClassId.Value), YearId = MyUser.CurrentBatchId;
                        int UserId = MyUser.UserId;
                        string Status = Drp_PresentStatus.SelectedValue;
                        int StudentId = int.Parse(Hd_PeriodId.Value);
                        MyAttendance.CreateTansationDb();


                        if (!MyAttendance.IsAttendanceAlreadyMarked(ClassId, _Date, StandardId, YearId, out MarkedStatus, out ClassAttendanceId))
                        {
                            MyAttendance.SaveClassAttendanceDetails(ClassId, _Date, UserId, StandardId, YearId, Status, out ClassAttendanceId);

                            if (ClassAttendanceId > 0)
                            {
                                string Sql = "SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudent.Status=1 AND tblstudentclassmap.ClassId=" + ClassId + " AND tblstudentclassmap.BatchId=" + YearId + " AND tblstudentclassmap.Standard=" + StandardId;
                                OdbcDataReader m_MyReader1 = MyAttendance.m_TransationDb.ExecuteQuery(Sql);
                                if (m_MyReader1.HasRows)
                                {
                                    while (m_MyReader1.Read())
                                    {
                                        MyAttendance.UpdateStudentAttendanceDetails(ClassAttendanceId, m_MyReader1.GetValue(0).ToString(), 0, ClassId, StandardId, YearId);
                                    }
                                }
                            }
                            MyAttendance.UpdateStudentAttendanceDetails(ClassAttendanceId, StudentId.ToString(), int.Parse(Status), ClassId, StandardId, YearId);
                        }
                        else
                        {
                            if (ClassAttendanceId > 0)
                            {
                                MyAttendance.UpdateStudentAttendanceDetails(ClassAttendanceId, StudentId.ToString(), int.Parse(Status), ClassId, StandardId, YearId);
                            }
                        }


                        MyAttendance.EndSucessTansationDb();
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Attendance Marked", "Attendance Mark for Student: " + MyStudent.GetStudentName(int.Parse(Hd_PeriodId.Value)) + "", 1);
                        ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "Openerpagereload();", true);
                        WC_MessageBox.ShowMssage("Successfully Updated");
                    }
                    catch (Exception Ex)
                    {
                        MyAttendance.EndFailTansationDb();
                        WC_MessageBox.ShowMssage("Error while saving attendance. Message : " + Ex.Message);
                    }

                }
                else
                {
                    WC_MessageBox.ShowMssage("Please select present status");
                }
            }
            else
            {
                WC_MessageBox.ShowMssage("Can't mark attendace for future dates");
            }
        }

        protected void Btn_Close_Click(object sender, EventArgs e)
        {

        }


    }
}
