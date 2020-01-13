using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;

namespace WinErParentLogin
{
    public partial class HomeWork : System.Web.UI.Page
    { 
        private ParentInfoClass MyParentInfo;
        private Attendance MyAttendence;
        private DataSet MydataSet = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MyParentObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyParentInfo = (ParentInfoClass)Session["MyParentObj"];
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            MyAttendence = new Attendance(_mysqlObj);
            if (MyParentInfo == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            else if (MyAttendence == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (!IsPostBack)
            {
                Label MyHeader = (Label)this.Master.FindControl("Lbl_PageHeader");
                MyHeader.Text = "Home Work";
                Load_Initials();
            }

        }
        private void Load_Initials()
        {
            Load_Home_Works();
        }
        private void Load_Home_Works()
        {
            
            DateTime Cur_Dt = new DateTime();
            DataSet ds_home = new DataSet();
            Cur_Dt = DateTime.Now.AddDays(-7);
            int Student_Id = 0;
            int Class_Id = 0;
            string Inner_HTML = "";
            try
            {
                Student_Id = MyParentInfo.StudentId;
                Class_Id = MyParentInfo.CLASSID;
                string sql = "(SELECT Id,Title,Body,CreatedDatetime,ExpiryDatetime FROM tbl_announcemnts WHERE ExpiryDatetime >='" + Cur_Dt.ToString("yyyy-MM-dd") + "' and RedId=" + Class_Id + " and  Type=2 )union (SELECT t1.Id,t1.Title,t1.Body,t1.CreatedDatetime,t1.ExpiryDatetime FROM tbl_announcemnts t1 INNER join tbl_annoucement_studentmap t2 on t2.AnnId=t1.Id WHERE t1.ExpiryDatetime>='" + Cur_Dt.ToString("yyyy-MM-dd") + "' and t2.StudentId=" + Student_Id + " and t1.Type=2)order by ExpiryDatetime ASC";
                ds_home = MyAttendence.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (ds_home != null && ds_home.Tables[0] != null && ds_home.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds_home.Tables[0].Rows)
                    {
                        DateTime Tomorrow_Dt = DateTime.Now.AddDays(1);
                        DateTime Expire_Dt = new DateTime();
                        DateTime.TryParse(dr["ExpiryDatetime"].ToString(), out Expire_Dt);
                        if (Expire_Dt.Date == Tomorrow_Dt.Date)
                        {
                            Inner_HTML = Inner_HTML + "<table width=\"100%\"><tr><td align=\"left\" style=\"width:50%;\"><label style=\"color:#008080;;font-size:medium;\">Subject :</label><label style=\"color:#4B85CD;;font-size:medium;\">" + dr["Title"].ToString() + "</label></td><td  align=\"center\" style=\"width:50%;\"><label style=\"color:#2F4F4F;;font-size:medium;\">Submission Date :</label><label style=\"color:#FF4500;font-size:medium;\">Tomorrow</label></td></tr></table> <hr COLOR=\"purple\"><div class=\"Div_Content\" ><p>" + dr["Body"].ToString() + "</p> </div><br /> ";
                        }
                        else if (Expire_Dt.Date == Cur_Dt.Date)
                        {
                            Inner_HTML = Inner_HTML + "<table width=\"100%\"><tr><td align=\"left\" style=\"width:50%;\"><label style=\"color:#008080;;font-size:medium;\">Subject :</label><label style=\"color:#4B85CD;;font-size:medium;\">" + dr["Title"].ToString() + "</label></td><td  align=\"center\" style=\"width:50%;\"><label style=\"color:#2F4F4F;;font-size:medium;\">Submission Date :</label><label style=\"color:Orange;font-size:medium;\">Today</label></td></tr></table> <hr COLOR=\"purple\"><div class=\"Div_Content\" ><p>" + dr["Body"].ToString() + "</p> </div><br /> ";
                        }
                        else
                        {
                            Inner_HTML = Inner_HTML + "<table width=\"100%\"><tr><td align=\"left\" style=\"width:50%;\"><label style=\"color:#008080;;font-size:medium;\">Subject :</label><label style=\"color:#4B85CD;;font-size:medium;\">" + dr["Title"].ToString() + "</label></td><td  align=\"center\" style=\"width:50%;\"><label style=\"color:#2F4F4F;;font-size:medium;\">Submission Date :</label><label style=\"color:#00FF00;font-size:medium;\">" + DateTime.Parse(dr["ExpiryDatetime"].ToString()).ToString("dd/MM/yyyy") + "</label></td></tr></table> <hr COLOR=\"purple\"><div class=\"Div_Content\" ><p>" + dr["Body"].ToString() + "</p> </div><br /> ";

                        }
                    }
                    this.InnerHtml.InnerHtml = Inner_HTML;

                }
                else
                {
                    Inner_HTML = "<div><hr COLOR=\"purple\"><br/><h3 style=\"color:Orange;text-align:center;\">No Home Works Assigned For Your Kid </h3><br/> <hr COLOR=\"purple\"></div> ";
                    this.InnerHtml.InnerHtml = Inner_HTML;
                }
            }
            catch
            {
                Inner_HTML = "<div><hr COLOR=\"purple\"><br/><h3 style=\"color:Orange;text-align:center;\"> Error Occured </h3><br/> <hr COLOR=\"purple\"></div> ";
                this.InnerHtml.InnerHtml = Inner_HTML;

            }
            
        }
    }
}
