using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using WinBase;
namespace WinEr
{
    public partial class ViewIncidence : System.Web.UI.Page
    {
        //private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private Incident MyIncident;
        private OdbcDataReader MyReader = null;
        private int UserId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            int IncidentId = int.Parse(Request.QueryString["id"].ToString());
            string Type = Request.QueryString["Type"].ToString();
            if (Type == "student")
            {
                if (Session["StudId"] == null)
                {
                    Response.Redirect("SearchStudent.aspx");
                }
                else
                {
                    UserId = int.Parse(Session["StudId"].ToString());
                }

            }
            else if (Type == "Staff")
            {
                if (Session["StaffId"] == null)
                {
                    Response.Redirect("ViewStaffs.aspx");
                }
                else
                {
                    UserId = int.Parse(Session["StaffId"].ToString());
                }
            }
            else if (Type == "Class")
            {
                if (Session["ClassId"] == null)
                {
                    Response.Redirect("ViewStaffs.aspx");
                }
                else
                {
                    UserId = int.Parse(Session["ClassId"].ToString());
                }
            }
            MyUser = (KnowinUser)Session["UserObj"];
          //  MyStudMang = MyUser.GetStudentObj();
            MyIncident = MyUser.GetIncedentObj();
            
            if (MyIncident == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }
            
            else
            {


                if (!IsPostBack)
                {

                   
                    //some initlization
                    Lbl_Standard.Visible = false;
                    Lbl_Teacher.Visible = false;
                    GetUserImage(Type);
                    LoadPupildData(UserId , Type);
                    LoadIncident(IncidentId,Type);
                }
            }

        }

        private void GetUserImage(string _Type)
        {
            string _ImageType;
            if (_Type == "student")
            {
                _ImageType = "StudentImage";
                Img_PupilImage.ImageUrl = "Handler/ImageReturnHandler.ashx?id=" + UserId + "&type=" + _ImageType + "";
                    //MyUser.GetImageUrl(_ImageType, UserId);
            }
            else if (_Type == "Staff")
            {
                _ImageType = "StaffImage";
                Img_PupilImage.ImageUrl = "Handler/ImageReturnHandler.ashx?id=" + UserId + "&type=" + _ImageType + "";

                    // MyUser.GetImageUrl(_ImageType, UserId);
            }
        }

        private void LoadPupildData(int _UserId , string _Type)
        {
            string sql = "";
            DateTime Dob;
            int Year;
            int Today = DateTime.Now.Year;
            if (_Type == "student")
            {
                sql = "select tblview_student.StudentName , tblview_student.DOB , tblview_student.Sex from tblview_student where tblview_student.Id=" + _UserId + "";// and tblview_student.`Status`=1";
                MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Lbl_PupilName.Text = MyReader.GetValue(0).ToString();
                    Dob = DateTime.Parse(MyReader.GetValue(1).ToString());
       //             Dob = MyUser.GetDareFromText(MyReader.GetValue(1).ToString());

                    Year = Today - Dob.Year;
                    Lbl_Age.Text = Year.ToString();
                    Lbl_Sex.Text = MyReader.GetValue(2).ToString();
                }
            }
            else if (_Type == "Staff")
            {
                
                sql = "select tblview_user.SurName , tblview_staffdetails.Dob , tblview_staffdetails.Sex from tblview_user inner join tblview_staffdetails on tblview_user.Id = tblview_staffdetails.UserId where tblview_user.Id=" + _UserId + " and tblview_user.`Status`=1";
                MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Lbl_Name.Text = "Staff Name";
                    Lbl_PupilName.Text = MyReader.GetValue(0).ToString();
                   Dob = DateTime.Parse(MyReader.GetValue(1).ToString());
            //        Dob = MyUser.GetDareFromText(MyReader.GetValue(1).ToString());

                    Year = Today - Dob.Year;
                    Lbl_Age.Text = Year.ToString();
                    Lbl_Sex.Text = MyReader.GetValue(2).ToString();
                }
            }
            else if (_Type == "Class")
            {
                Lbl_SexName.Visible = false;
                Lbl_Age1.Visible = false;
                Lbl_Standard.Visible = true;
                Lbl_Teacher.Visible = true;
                sql = " select tblclass.ClassName, tblclass.Standard  from tblincedent INNER join tblclass on tblclass.Id = tblincedent.AssoUser where tblincedent.AssoUser=" + _UserId + " and tblincedent.UserType='Class'";
                MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Lbl_PupilName.Text = MyReader.GetValue(0).ToString();
                    Lbl_Sex.Text = MyReader.GetValue(1).ToString();
                }
                sql = "select tblview_user.SurName from tblclassschedule inner join tblview_user on tblview_user.Id = tblclassschedule.ClassTeacherId where tblview_user.`Status` =1 and tblclassschedule.ClassId = " + _UserId + "";
                MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Lbl_Age.Text = MyReader.GetValue(0).ToString();
                }
            }
            
        }

        private void LoadIncident(int IncidentId, string Type)
        {
            DateTime _Date;
            string sql = "select tblview_user.SurName,  tblview_incident.IncedentDate, tblview_incident.Description,tblview_incident.Title  from tblview_incident inner join tblview_user on tblview_user.Id = tblview_incident.CreatedUserId where tblview_incident.Id =" + IncidentId + " and tblview_incident.UserType='" + Type.ToLowerInvariant() + "'";
            MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Lbl_RportedBy.Text = MyReader.GetValue(0).ToString();
               _Date = DateTime.Parse(MyReader.GetValue(1).ToString());
                //_Date = MyUser.GetDareFromText(MyReader.GetValue(1).ToString());

                Lbl_Date.Text = _Date.Date.ToString("dd-MM-yyyy");
                this.IncDesc.InnerHtml = MyReader.GetValue(2).ToString();
                lbl_incHead.Text = MyReader.GetValue(3).ToString();
            }

        }



        

        
    }
}
