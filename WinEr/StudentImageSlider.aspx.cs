using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using WinBase;

namespace WinEr
{
    public partial class StudentImageSlider : System.Web.UI.Page
    {
        private StudentManagerClass MyStudentManager;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private OdbcDataReader MyReader1 = null;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudentManager = MyUser.GetStudentObj();
            if (MyUser == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (MyStudentManager == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(600))
            {
                Response.Redirect("RoleErr.htm");
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
                    int ClassId = 0;
                    Lbl_Message.Text = "";
                    Label_ClassName.Text = "Class Not Found";
                    PanelStudents.Visible = false;
                    if (Session["ClassId"] != null)
                    {
                        if (int.TryParse(Session["ClassId"].ToString(), out ClassId))
                        {
                            Load_ClassName(ClassId);
                            Load_StudentImages(ClassId);

                        }
                        else
                        {
                            Lbl_Message.Text = "Class Not Found";
                        }
                    }
                    else
                    {
                        Lbl_Message.Text = "Class Not Found";
                    }
                }
            }

        }

        private void Load_ClassName(int ClassId)
        {
            string sql = "SELECT tblclass.ClassName FROM tblclass WHERE tblclass.Id=" + ClassId;
            MyReader = MyStudentManager.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Label_ClassName.Text = MyReader.GetValue(0).ToString();
            }
        }

        private void Load_StudentImages(int ClassId)
        {
            DateTime _DOB;
            string InnerHTML = "", Name = "", AdmitionNo = "", RollNo = "", StudentId = "", Sex = "", ImageUrl = "", names = "", seperator = "", _age="";
            string sql = "SELECT tblstudent.StudentName,tblstudent.AdmitionNo,tblstudentclassmap.RollNo,tblstudent.Id,tblstudent.Sex,tblstudent.DOB FROM tblstudent INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.`Status`=1 AND tblstudentclassmap.ClassId=" + ClassId + " ORDER BY tblstudent.StudentName";
            MyReader = MyStudentManager.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while(MyReader.Read()) 
                {
                    Name=MyReader.GetValue(0).ToString();
                    AdmitionNo=MyReader.GetValue(1).ToString();
                    RollNo=MyReader.GetValue(2).ToString();
                    StudentId = MyReader.GetValue(3).ToString();
                    Sex = MyReader.GetValue(4).ToString();
                    _DOB = DateTime.Parse(MyReader.GetValue(5).ToString());
                    _age= MyStudentManager.GetAge(_DOB, DateTime.Now).ToString();
                    ImageUrl = "Handler/ImageReturnHandler.ashx?id=" + StudentId + "&type=StudentImage";
                    if(String.IsNullOrEmpty(ImageUrl))
                        ImageUrl=GetStudentImageUrl(StudentId, Sex);
                    if (RollNo == "-1")
                    {
                        RollNo = "Not Assigned";
                    }
                    InnerHTML = InnerHTML + "<div class=\"panel\"><div class=\"inside\"><center> <img src=\"" + ImageUrl + "\" alt=\"picture\" /> <h2>" + Name + "</h2> <p> <table> <tr> <td align=\"right\"> Admission No : </td><td align=\"left\"> " + AdmitionNo + "</td></tr><tr> <td align=\"right\"> Roll No : </td><td align=\"left\"> " + RollNo + "</td></tr><tr> <td align=\"right\"> Age : </td><td align=\"left\"> " + _age + "</td></tr> </table></p>   </center>  <a href=\"StudentSessionMaker.aspx?StudentId=" + StudentId + "\">More Details..</a></div> </div>";
                    names = names + seperator + Name;
                    seperator = "-";
               
                }
                HiddenValueNames.Value = names;
            }
            if (InnerHTML == "")
            {
                Lbl_Message.Text = "No student in selected class";
            }
            else
            {
                PanelStudents.Visible = true;
                this.ImageSliderDIV.InnerHtml = InnerHTML;
            }
        }

        private string GetStudentImageUrl(string StudentId, string Sex)
        {
            string ImageUrl = "";
            string sql = "SELECT FilePath FROM tblfileurl WHERE `Type`='StudentImage' AND UserId=" + StudentId;
            MyReader1 = MyStudentManager.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader1.HasRows)
            {
                if (MyReader1.GetValue(0).ToString() != "")
                {
                    ImageUrl = WinerUtlity.GetRelativeFilePath(objSchool) + "ThumbnailImages/" + MyReader1.GetValue(0).ToString();
                }
            }
            if (ImageUrl == "")
            {
                if (Sex.ToUpperInvariant() == "MALE")
                {
                    ImageUrl = "Pics/user5.png";
                }
                else
                {
                    ImageUrl = "Pics/she_user.png";
                }

            }
            return ImageUrl;
        }
    }
}
