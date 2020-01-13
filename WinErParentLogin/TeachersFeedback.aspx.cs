using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;
using System.Text;


namespace WinErParentLogin
{
    public partial class TeachersFeedback : System.Web.UI.Page
    {
        private ParentInfoClass MyParentInfo;
       
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MyParentObj"] == null)
            {
                Response.Redirect("sectionerr.htm");

            }
            MyParentInfo = (ParentInfoClass)Session["MyParentObj"];
            if (MyParentInfo == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (!IsPostBack)
            {

                Label MyHeader = (Label)this.Master.FindControl("Lbl_PageHeader");
                MyHeader.Text = "Feedback";
                loadmonth();
                mainarea.Visible = false;
                lblmsg.Text = "";
            }
        }

        private void loadmonth()
        {
            
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            Incident MyIncident = new Incident(_mysqlObj);
          int Freequency= MyIncident.GetPreriodFrequencyType();
            DataSet dt = MyIncident.getPeriods(Freequency);

            if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
            {
                drp_period.DataTextField = "Period";
                drp_period.DataValueField = "id";
                drp_period.DataSource = dt;
                drp_period.DataBind();
            }
            else
                drp_period.Items.Add(new ListItem("Period does not found"));
        }

       
        private void LoadFeedBack()
        {
            lblmsg.Text = "";
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            Incident MyIncident = new Incident(_mysqlObj);
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            int teachersfeedbackid = 0, staffId = 0, teachersfeedbackMasterId=0;

            string staffName = "";

            if (MyIncident.IsUpdatedFeedback(MyParentInfo.StudentId, MyParentInfo.CLASSID,int.Parse(drp_period.SelectedValue.ToString()), MyParentInfo.CurrentBatchId, out teachersfeedbackid, out  staffId, out staffName, out teachersfeedbackMasterId))
            {
                if (teachersfeedbackid > 0)
                {

                    DataSet dt = MyIncident.getStudentIncident(MyParentInfo.StudentId, MyParentInfo.CLASSID, teachersfeedbackid);

                    if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
                    {
                        mainarea.Visible = true;
                        lbl_schoolName.Text = MyParentInfo.SCHOOLNAME;
                        lblstudent.Text = MyParentInfo.StudentName;
                        lbl_class.Text = MyParentInfo.CLASSNAME;
                        lbl_month.Text = drp_period.SelectedItem.Text;
                      
                        Performance.InnerHtml = "";

                        StringBuilder sb = new StringBuilder();
                        sb.Append("<table width=\"100%\">");

                        foreach (DataRow dr in dt.Tables[0].Rows)
                        {
                            sb.Append("<tr><td align=\"right\" style=\"padding-right:10px\"> " + dr["Title"] + "</td><td><b>:" + dr["Description"] + "</b></td></tr>");
                            sb.Append("<tr><td colspan=\"2\"><hr /></td></tr>");
                        }
                        sb.Append("</table>");

                        Performance.InnerHtml = sb.ToString();
                        lbl_staff.Text = staffName;

                    }
                    else
                    {
                        mainarea.Visible = false;
                        Performance.InnerHtml = "";
                      
                    }

                }
                else
                {
                    mainarea.Visible = false;
                    Performance.InnerHtml = "";
                    
                }
            }
            else
            {
                lblmsg.Text = "Feddback details does not found in selected month";
                Performance.InnerHtml = "";
                mainarea.Visible = false;
            }
        }

        protected void btnshow_Click(object sender, EventArgs e)
        {
            LoadFeedBack();
        }
    }
}
