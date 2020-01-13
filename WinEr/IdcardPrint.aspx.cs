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
using WinBase;


namespace WinEr
{
    public partial class IdcardPrint : System.Web.UI.Page
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
                Response.Redirect("sectionerr.htm");
            }
            if (Session["StudId"] == null)
            {
                Response.Redirect("SearchStudent.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(44))
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
                    
                    LoadStudentData();
                    AddStudentImage();
                    LoadSchoolLogo();
                    //some initlization

                }
            }

        }

        private void LoadSchoolLogo()
        {
            Img_Logo.ImageUrl = "Handler/ImageReturnHandler.ashx?id=1&type=Logo";
        }

        private void AddStudentImage()
        {
            Img_Student.ImageUrl = "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StudId"].ToString()) + "&type=StudentImage";

                //MyUser.GetImageUrl("StudentImage", int.Parse(Session["StudId"].ToString()));
        }

        private void LoadStudentData()
        {
            DateTime DateOFBirth;
            string BldGrp;
            string sql = "SELECT StudentName,DOB,AdmitionNo,BloodGroup,Address,GardianName FROM tblstudent WHERE Id=" + int.Parse(Session["StudId"].ToString());
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
               
                Lbl_StudentName.Text = MyReader.GetValue(0).ToString();
                DateOFBirth = DateTime.Parse(MyReader.GetValue(1).ToString());
               // DateOFBirth = MyUser.GetDareFromText(MyReader.GetValue(1).ToString());

                Lbl_DOB.Text = DateOFBirth.Date.ToString("dd-MM-yyyy");
                Lbl_AdmissionNo.Text = MyReader.GetValue(2).ToString();
                BldGrp = MyReader.GetValue(3).ToString();
                if (BldGrp == "Do Not Know")
                {
                    Lbl_BloodGRp.Text = "";
                }
                else
                {
                    Lbl_BloodGRp.Text = BldGrp;
                }
                Lbl_AddressBack.Text = MyReader.GetValue(4).ToString();
                Lbl_Parent.Text = MyReader.GetValue(5).ToString();

            }
            sql = "SELECT SchoolName,Address FROM tblschooldetails WHERE Id=1";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Lbl_SchoolName.Text = MyReader.GetValue(0).ToString();
                Lbl_School.Text = MyReader.GetValue(0).ToString();
                Lbl_Address.Text = MyReader.GetValue(1).ToString();
            }
        }
    }
}
