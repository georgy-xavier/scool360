using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;

namespace WinEr
{
    public partial class StudentView : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            if (MyStudMang == null)
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
            }

        }
        [WebMethod(EnableSession = true)]
        public static string[] StudentDtlsview(string StdntId, string action, string stdntStatus)
        {
            if (action != null)
            {
                // int index = Convert.ToInt32(StdntId);
                KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
                StudentManagerClass MyStudMang = MyUser.GetStudentObj();
                string[] urlRedirect = new string[3];
                urlRedirect[0] = null; urlRedirect[1] = null;
                int StudentId = int.Parse(StdntId);
                int StudentStatus = int.Parse(stdntStatus);
                if (action == "view")
                {
                    if (StudentStatus == 1 || StudentStatus == 2 || StudentStatus == 3)
                    {
                        HttpContext.Current.Session["StudId"] = StudentId;
                        HttpContext.Current.Session["StudType"] = StudentStatus;
                        urlRedirect[0] = "StudentDetails.aspx";
                        urlRedirect[2] = StudentId.ToString();
                       // Response.Redirect("StudentDetails.aspx");
                    }
                    //else if (StudentStatus == 4)
                    //{
                    //    ////  ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScriptNameYouLike", "openIncpopup('SutdDetailsPupUp.aspx?StudId=" + StudentId + "');", true);
                    //}
                    //else if (MyUser.HaveActionRignt(606))
                    //{
                    //    //HiddenField Hdn = new HiddenField();
                    //    //Hdn = (HiddenField)Grd_StudentList.Rows[index].FindControl("Hdn_TempId");
                    //    //string TempId = Hdn.Value;
                    //    //int ClassId = 0;
                    //    //string sql = "";
                    //    //OdbcDataReader Classreader = null;

                    //    //sql = "Select Class from tbltempstdent where tbltempstdent.TempId='" + TempId + "'";
                    //    //Classreader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    //    //if (Classreader.HasRows)
                    //    //{
                    //    //    int.TryParse(Classreader.GetValue(0).ToString(), out ClassId);
                    //    //}

                    //    //// ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScriptNameYouLike", "openIncpopup('RegisteredStudentDetails.aspx?TempStudId=" + TempId + "&ClassId=" + ClassId + "');", true);
                    //}
                    else
                        urlRedirect[1] = "You do not have sufficient rights to perform this action. Contact administrator";
                }
                else if (action == "edit")
                {
                    if (StudentStatus == 1)
                    {
                        if (MyUser.HaveActionRignt(4))
                        {
                            int id = StudentId;
                            ////    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScriptNameYouLike", "openIncpopup('ManageStudentBulk.aspx?StudId=" + id + "');", true);
                            HttpContext.Current.Session["StudId"] = StudentId;
                            HttpContext.Current.Session["StudType"] = StudentStatus;
                            //Session["StudType"] = StudentStatus;
                            //Response.Redirect("ManageStudent.aspx");
                            urlRedirect[0] = "ManageStudent.aspx";
                        }
                        else
                            urlRedirect[1] = "You do not have sufficient rights to perform this action. Contact administrator";

                    }
                    else if (StudentStatus == 5)
                    {
                        urlRedirect[1] = "Registered students can edit at View Registered Students Page.";
                    }
                    else
                    {
                        urlRedirect[1] = "Cannot edit the student";
                    }
                }
                else if (action == "fees")
                {
                    if (StudentStatus == 1 || StudentStatus == 3)
                    {
                        if (MyUser.HaveActionRignt(2))
                        {
                            int RollNumber = -1;
                            int ClassID = MyStudMang.GetClassNroll(StudentId, MyUser.CurrentBatchId, out RollNumber);

                            urlRedirect[0] = "CollectFeeAccount.aspx?ClassId=" + ClassID + "&RollNumber=" + RollNumber + "&StudentId=" + StudentId + "";
                        }
                        else
                            urlRedirect[1] = "You do not have sufficient rights to perform this action. Contact administrator";

                    }
                    else if (StudentStatus == 5)
                    {
                        if (MyUser.HaveActionRignt(605))
                        {
                            urlRedirect[0] = "CollectJoiningFee.aspx?Studentid=" + StudentId;
                        }
                        else
                            urlRedirect[1] = "You do not have sufficient rights to perform this action. Contact administrator";
                    }
                    else
                    {
                        urlRedirect[1] = "Not possible to collect fees";
                    }
                }
                return urlRedirect;
            }
            return null;
        }
    }
}