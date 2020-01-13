using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using WinEr;
using WinBase;
public partial class Tc : System.Web.UI.Page
{
    private StudentManagerClass MyStudMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    private SchoolClass objSchool = null;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_init(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyStudMang = MyUser.GetStudentObj();
        if (MyStudMang == null)
        {
            Response.Redirect("Default.aspx");
            //no rights for this user.
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
                if (WinerUtlity.NeedCentrelDB())
                {
                    if (Session[WinerConstants.SessionSchool] == null)
                    {
                        Response.Redirect("Logout.aspx");
                    }
                    objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                }

                //some initlization
                if (Request.QueryString["StudId"].ToString() != "")
                {
                    try
                    {
                        int StudId = int.Parse(Request.QueryString["StudId"]);
                        LoadLogo();
                        LoadSchoolDetails();
                        LoadStudentDetails(StudId);
                    }
                    catch
                    {

                    }
                }
                //GenerateTcNo();
            }
        }
    }

 /*   private void GenerateTcNo()
    {
        string _strdtNow, Year,Id="",TcNumber;
       
        _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        DateTime _StandardNow = DateTime.Parse(_strdtNow);
        Year=_StandardNow.Year.ToString();
        String Sql = "SELECT Id FROM tbltc WHERE StudentId=" + int.Parse(Session["StudId"].ToString());
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
            Id = MyReader.GetValue(0).ToString();
        }
        TcNumber = Year + "-" + Id;
        Lbl_TcNo.Text = TcNumber;
    }*/

    private void LoadStudentDetails(int StudId)
    {
        DateTime Date_Dob, Date_LastAttendance, Date_TcRcvedDate, Date_TCIssue;//, Date_DteOFAdmission;
        string PromotionDate = "";
        String Sql = "SELECT * FROM tbltc WHERE StudentId=" + StudId;
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
         if (MyReader.HasRows)
         {
             Lbl_TcNo.Text = MyReader.GetValue(2).ToString();
             Lbl_NameOfPupil.Text=MyReader.GetValue(3).ToString();
             Lb_SchoolName.Text = MyReader.GetValue(4).ToString();
             Lbl_AdmissionNo.Text = MyReader.GetValue(5).ToString();
             Lbl_Cumulative.Text = MyReader.GetValue(6).ToString();
             Lbl_Sex.Text = MyReader.GetValue(7).ToString();
             Lbl_FatherName.Text = MyReader.GetValue(8).ToString();
             Lbl_Nationality.Text = MyReader.GetValue(9).ToString();
             Lbl_Religion.Text = MyReader.GetValue(10).ToString();
             Lbl_Cast.Text = MyReader.GetValue(11).ToString();
             Lbl_CastType.Text = MyReader.GetValue(12).ToString();
             Date_Dob = DateTime.Parse(MyReader.GetValue(13).ToString());
//             Date_Dob = MyUser.GetDareFromText(MyReader.GetValue(13).ToString());

             Lbl_Dob.Text = MyUser.GerFormatedDatVal(Date_Dob);
             Lbl_CurrStd.Text = MyReader.GetValue(14).ToString();
             Lbl_LanStd.Text = MyReader.GetValue(15).ToString();
             Lbl_MediumOfIns.Text = MyReader.GetValue(16).ToString();
             Lbl_Syllabus.Text = MyReader.GetValue(17).ToString();
           //  Date_DteOFAdmission = DateTime.Parse(MyReader.GetValue(18).ToString());
            // Lbl_DteOFAdmission.Text = Date_DteOFAdmission.Date.ToString("dd-MM-yyyy");
             PromotionDate = MyReader.GetValue(18).ToString();
             Lbl_DteOFAdmission.Text = PromotionDate;
             Lbl_QualiFoePro.Text = MyReader.GetValue(19).ToString();
             Lbl_FeesDue.Text = MyReader.GetValue(20).ToString();
             Lbl_FeeConcession.Text=MyReader.GetValue(21).ToString();
             Lbl_Scholarship.Text=MyReader.GetValue(22).ToString();
             Lbl_MedicalyExmnd.Text = MyReader.GetValue(23).ToString();
             Date_LastAttendance = DateTime.Parse(MyReader.GetValue(24).ToString());
//             Date_LastAttendance = MyUser.GetDareFromText(MyReader.GetValue(24).ToString());

             Lbl_LastAttendance.Text = MyUser.GerFormatedDatVal(Date_LastAttendance);
             Date_TcRcvedDate = DateTime.Parse(MyReader.GetValue(25).ToString());
//             Date_TcRcvedDate =MyUser.GetDareFromText(MyReader.GetValue(25).ToString());

             Lbl_TcRecvdDate.Text =  MyUser.GerFormatedDatVal(Date_TcRcvedDate);
             Date_TCIssue = DateTime.Parse(MyReader.GetValue(26).ToString());
             //Date_TCIssue =MyUser.GetDareFromText(MyReader.GetValue(26).ToString());

             Lbl_TCIssue.Text =  MyUser.GerFormatedDatVal(Date_TCIssue); 
             Lbl_TotalDays.Text = MyReader.GetValue(27).ToString();
             Lbl_AttendedDays.Text = MyReader.GetValue(28).ToString();
             Lbl_CC.Text = MyReader.GetValue(29).ToString();

         }
    }

    private void LoadLogo()
    {
        Img_Logo.ImageUrl = "Handler/ImageReturnHandler.ashx?id=1&type=Logo";

    }

    private void LoadSchoolDetails()
    {
        string Sql = "SELECT SchoolName,Address FROM tblschooldetails WHERE Id=1";
         MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
         if (MyReader.HasRows)
         {
             Lbl_SchoolName.Text = MyReader.GetValue(0).ToString();
             Lbl_Address.Text = MyReader.GetValue(1).ToString();
         }
    }
}
