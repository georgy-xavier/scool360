using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.Odbc;
using WinBase;

namespace WinEr
{
    public partial class PrintCertificate : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        protected void Page_Load(object sender, EventArgs e)
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

            }
            if (Session["StudId"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    //some initlization
                    //if (Request.QueryString["Data"] != null)
                    //{
                    //    KnowinEncryption My_Encryption = new KnowinEncryption();
                    //    string dfff = HttpUtility.UrlDecode(Request.QueryString["Data"].ToString()).Replace("@per@", "%").Replace("@plus@", "+");

                    //    this.DivData.InnerHtml = My_Encryption.Decrypt(dfff.Replace(' ', '+'));
                    //}
                    if (Session["CertificateData"] != null)
                    {
                        string Data_replace = Session["CertificateData"].ToString();
                       // Data_replace = Session["CertificateData"].ToString().Replace("&amp;", "&").Replace("&lt;", "<").ToString().Replace("&gt;", ">").ToString().Replace("&quot;", "\"").ToString().Replace("&amp;", "&").Replace("&nbsp;", " ").ToString();
                        //.Replace("&lt;", "<").ToString().Replace("&gt;", ">").ToString().Replace("&quot", "\"").ToString().Replace("nbsp;", " ").ToString().Replace(";School", " ").ToString().Replace("&", " ").ToString().Replace("amp; ", " ");

                        string _needboarder = "";
                        if (Request.QueryString["NeedBoarder"] != null)
                        {
                            if (Request.QueryString["NeedBoarder"].ToString().Trim() == "YES")
                            {
                                _needboarder = "style=\"border:#4a4a4a thin solid;\"";
                            }
                        }

                        this.DivData.InnerHtml = "<div  class=\"page\" " + _needboarder + "> " + Data_replace + " </div>";
                    }
                }
            }
        }


        //protected void Page_init(object sender, EventArgs e)
        //{
        //    //if (Session["UserObj"] == null)
        //    //{
        //    //    Response.Redirect("Default.aspx");
        //    //}
        //    //MyUser = (KnowinUser)Session["UserObj"];
        //    //MyStudMang = MyUser.GetStudentObj();
        //    //if (MyStudMang == null)
        //    //{
        //    //    Response.Redirect("Default.aspx");
                
        //    //}
        //    //if(Session["StudId"] == null)
        //    //{
        //    //      Response.Redirect("sectionerr.htm");
        //    //}
        //    //else
        //    //{
        //    //    if (!IsPostBack)
        //    //    {
        //    //        //some initlization
        //    //        //if (Request.QueryString["C_Type"] != null && Request.QueryString["C_Name"] != null)
        //    //        //{
        //    //        //    try
        //    //        //    {
        //    //        //         LoadSchoolDetails();  
        //    //        //    }
        //    //        //    catch
        //    //        //    {

        //    //        //    }
        //    //        //}
                    
        //    //    }
        //    //}
        //}

        //private void LoadSchoolDetails()
        //{

        //    string sql = "select tblschooldetails.LogoUrl, tblschooldetails.SchoolName, tblschooldetails.Address from tblschooldetails where tblschooldetails.SchoolName='" + MyUser.SchoolName + "'";
        //    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
        //    if (MyReader.HasRows)
        //    {
        //        Img_Logo.ImageUrl = "~/ThumbnailImages/" + MyReader.GetValue(0).ToString();
        //        Lbl_ClgName.Text = MyReader.GetValue(1).ToString();
        //        Lbl_Add.Text = MyReader.GetValue(2).ToString();
        //    }

          
        //    //Student Details
        //    string Sql_StudDetails = "select AdmitionNo from tblview_student  where tblview_student.Id=" + int.Parse(Session["StudId"].ToString());
        //    OdbcDataReader StudReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql_StudDetails);
            
        //    if (StudReader.HasRows)
        //    {  
        //        Lbl_AdmissionNum.Text ="Admission No : "+ StudReader.GetValue(0).ToString();
        //    }

        //     if (Request.QueryString["C_Name"] != null)
        //         Lbl_CertificateName.Text=Request.QueryString["C_Name"].ToString();
        //    if (Request.QueryString["C_Content"] != null)
        //        Contents.InnerHtml=Request.QueryString["C_Content"].ToString();
        //    if (Request.QueryString["IsNeed_Estd"] != null && Request.QueryString["IsNeed_Estd"] == "1" && Request.QueryString["ESTD"] != null && Request.QueryString["ESTD"] != "")
        //      {
        //          Lbl_Estd.Text="Estd : "+Request.QueryString["ESTD"];
        //          Lbl_Estd.Visible=true;
        //      }
        //      else
        //      {
        //          Lbl_Estd.Visible=false;
        //      }
        //      if (Request.QueryString["ISNeed_SchoolCode"] != null && Request.QueryString["ISNeed_SchoolCode"] == "1" && Request.QueryString["SchoolCode"] != null && Request.QueryString["SchoolCode"] != "")
        //      {
        //          Lbl_Code.Text="School Code : "+Request.QueryString["SchoolCode"];
        //          Lbl_Code.Visible=true;
        //      }
        //      else
        //      {
        //          Lbl_Code.Visible=false;
        //      }
        //    if (Request.QueryString["Footer1"] != null)
        //    {
        //        Lbl_FL1.Text = Request.QueryString["Footer1"];
        //    }
        //    if (Request.QueryString["Footer2"] != null)
        //    {
        //        lbl_FC1.Text = Request.QueryString["Footer2"];
        //    }
        //    if (Request.QueryString["Footer3"] != null)
        //    {
        //        Lbl_FR1.Text = Request.QueryString["Footer3"];
        //    }
        //    if (Request.QueryString["Behaviour"] != null)
        //    {
        //        behaviour.InnerHtml = Request.QueryString["Behaviour"].ToString();
        //    }

        //    if (Request.QueryString["C_Date"] != null)
        //    {
        //        Lbl_Date.Text = Request.QueryString["C_Date"].ToString();
        //    }

           

        //}
    }
}
