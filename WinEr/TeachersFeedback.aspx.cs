using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;
using System.Drawing;
using System.Text;

namespace WinEr
{
    public partial class TeachersFeedback : System.Web.UI.Page
    {
        private Incident            MyIncedent;
        private KnowinUser          MyUser;
        private ClassOrganiser      MyClass;
        private StudentManagerClass MystudentObj;
        RadioButtonList[] dynamicRdoBtn;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            
            MyUser          = (KnowinUser)Session["UserObj"];
            MyIncedent      = MyUser.GetIncedentObj();
            MyClass         = MyUser.GetClassObj();
            MystudentObj    = MyUser.GetStudentObj();

            if (MyIncedent == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }
            else
            {
                if (!IsPostBack)
                {
                    btnSubmit.Visible = false;
                    LoadInitialValue();
                }
                else
                {
                   
                        LoadIncidentDetails();
                    
                }
            }
        }
       
        protected void btnSelect_Click(object sender, EventArgs e)
        {
            //LoadIncidentDetails();
            lblMessage.Text = "";
            if (drp_period.SelectedValue != "0")
            {
                loadStudentDetails();
            }
            else
            {
                lblMessage.Text = "Select period";
            }
         
        }

       


        protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            LoadStudent();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Validdata())
            {
                saveIncident();
            }
            
        }

        private bool Validdata()
        {
            if (dynamicRdoBtn.Length > 0)
            {
                bool entered = true;
                for (int i = 0; i < dynamicRdoBtn.Length; i++)
                {
                    if(dynamicRdoBtn[i].SelectedIndex==-1)
                    {
                        entered=false;
                        lblMessage.Text = "Please select all the feedbacks";
                    }                    

                }
                


                return entered;
            }
            return false;
        }

        private void loadStudentDetails()
        {
            int teachersfeedbackid=0;

            if (MyIncedent.IsUpdatedFeedback(int.Parse(drp_Student.SelectedValue), int.Parse(drpClass.SelectedValue), int.Parse(drp_period.SelectedValue),MyUser.CurrentBatchId, out  teachersfeedbackid))
            {
                if (teachersfeedbackid > 0)
                {

                    DataSet dt = MyIncedent.getStudentIncident(int.Parse(drp_Student.SelectedValue), int.Parse(drpClass.SelectedValue), teachersfeedbackid);

                    if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
                    {
                        userfeedback.Visible = true;
                        userfeedback.InnerHtml = "";

                        StringBuilder sb = new StringBuilder();
                        sb.Append("<table width=\"100%\">");

                        foreach (DataRow dr in dt.Tables[0].Rows)
                        {
                            sb.Append("<tr><td>" + dr["Title"] + "</td><td>" + dr["Description"] + "</td></tr>");
                            sb.Append("<tr><td colspan=\"2\"><hr /></td></tr>");
                        }
                        sb.Append("</table>");
                        
                        userfeedback.InnerHtml = sb.ToString();
                       
                    }
                    else
                    {
                        userfeedback.InnerHtml = "";
                        userfeedback.Visible = false;
                    }
                
                }
                else
                {
                    userfeedback.InnerHtml = "";
                    userfeedback.Visible = false;
                }
            }
            else
            {
                userfeedback.InnerHtml = "";
                userfeedback.Visible = false;
            }
        }

       

        private void saveIncident()
        {
            int needApproval        = 0;
            int feedbackMasterId    = 0;
            int studentId           = 0;
            int classId             = 0;
            int staffId             = 0;
            
            string staffName        = "";
            string feedbackmasterName   = "";

            DateTime createdDateTime    = System.DateTime.Now;

            int.TryParse(hdn_MasterId.Value,        out feedbackMasterId);
            int.TryParse(hdn_NeedApproval.Value,    out needApproval);
            int.TryParse(drpClass.SelectedValue,    out classId);
            int.TryParse(drp_Student.SelectedValue, out studentId);

            staffId     = MyUser.UserId;
            staffName   = MyUser.UserName;

            try
            {


                MyIncedent.m_MysqlDb.MyBeginTransaction();

                int incidentId = 0;

                if (MyIncedent.IsUpdatedFeedback(int.Parse(drp_Student.SelectedValue), int.Parse(drpClass.SelectedValue),int.Parse(drp_period.SelectedValue),MyUser.CurrentBatchId, out  incidentId))
                {
                    if (incidentId > 0)
                    {
                        MyIncedent.DeleteCurrentValues(incidentId);
                    }
                }


                feedbackmasterName = MyIncedent.getFeedbackNamefromId(feedbackMasterId);

                int teachersFeedbackid = MyIncedent.insertTeachersFeedBackTable(feedbackMasterId, feedbackmasterName, staffId, staffName, classId, studentId,int.Parse(drp_period.SelectedValue.ToString()),MyUser.CurrentBatchId);

                if (teachersFeedbackid != 0)
                {
                    DataSet incidentDetails = getIncidentDataset(teachersFeedbackid, feedbackMasterId, feedbackmasterName, staffId, staffName, classId, studentId, createdDateTime);
                  
                    if (incidentDetails != null && incidentDetails.Tables != null && incidentDetails.Tables[0].Rows.Count > 0)
                    {
                         
                        bool success = MyIncedent.SubmitIncident(incidentDetails);
                    }

                }

                lblMessage.Text = "Incident succesfully submitted";
                loadStudentDetails();
                MyIncedent.m_MysqlDb.TransactionCommit();
            }
            catch (Exception e)
            {
                MyIncedent.m_MysqlDb.TransactionRollback();
                lblMessage.Text = "Cannot submit the incident now.Please try again later; Error Message:"+ e.Message;
            }

        }

        private DataSet getIncidentDataset(int teachersFeedbackid, int feedbackMasterId, string feedbackmasterName, int staffId, string staffName, int classId, int studentId, DateTime createdDateTime)
        {
            DataSet insData = getFreshDataset();
            DataRow dr;

            DataSet incidentType = MyIncedent.getIncidentTypefromMasterId(feedbackMasterId);
         
            if (incidentType != null && incidentType.Tables != null && incidentType.Tables[0].Rows.Count > 0)
            {
                int i=0;
                foreach (DataRow dr2 in incidentType.Tables[0].Rows)
                {
                    
                    dr = insData.Tables["IncidentTable"].NewRow();

                    dr["Title"] = dr2["IncidentType"];
                    dr["Description"] = dynamicRdoBtn[i].SelectedItem.Text;
                    dr["IncedentDate"] = createdDateTime;
                    dr["CreatedDate"] = createdDateTime;
                    dr["TypeId"] =  dr2["TypeId"];
                    dr["ApproverId"] =1;
                    dr["CreatedUserId"] = staffId;
                    dr["UserType"] = "student";
                    dr["Status"] = "Approved";
                    dr["AssoUser"] = studentId;
                    dr["HeadId"] = dynamicRdoBtn[i].SelectedValue;
                    dr["Point"] = 0;
                    dr["BatchId"] = MyUser.CurrentBatchId;
                    dr["ClassId"] = classId;
                    dr["Reference"] = "";
                    dr["IncidentType"] = 1;
                    dr["IncidentTypeId"] = teachersFeedbackid;
                    i++;
                    insData.Tables["IncidentTable"].Rows.Add(dr);
                }
            }
            return insData;
        }

        private DataSet getFreshDataset()
        {
            DataSet     dataset = new DataSet();
            DataTable   dt;
            DataRow     dr;

            dataset.Tables.Add(new DataTable("IncidentTable"));
            dt = dataset.Tables["IncidentTable"];

            dt.Columns.Add("Title");
            dt.Columns.Add("Description");
            dt.Columns.Add("IncedentDate");
            dt.Columns.Add("CreatedDate");
            dt.Columns.Add("TypeId");
            dt.Columns.Add("ApproverId");
            dt.Columns.Add("CreatedUserId");
            dt.Columns.Add("UserType");
            dt.Columns.Add("Status");
            dt.Columns.Add("AssoUser");
            dt.Columns.Add("HeadId");
            dt.Columns.Add("Point");
            dt.Columns.Add("BatchId");
            dt.Columns.Add("ClassId");
            dt.Columns.Add("Reference");
            dt.Columns.Add("IncidentType");
            dt.Columns.Add("IncidentTypeId");

            return dataset;
        }
            
        private void LoadInitialValue()
        {
            LoadPeriod();
            LoadClassDetails();
        }

        private void LoadPeriod()
        {
            int FrequencyType = MyIncedent.GetPreriodFrequencyType();
            DataSet dt = MyIncedent.getPeriods(FrequencyType);
            
            if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
            {
                drp_period.DataTextField="Period";
                drp_period.DataValueField="id";
                drp_period.DataSource= dt;
                drp_period.DataBind();
            }
            else
                drp_period.Items.Add(new ListItem("Period does not found"));


        }

        private void LoadClassDetails()
        {
            DataSet dt_class= MyUser.MyAssociatedClass();

            if (dt_class != null && dt_class.Tables != null && dt_class.Tables[0].Rows.Count > 0)
            {
                drpClass.DataTextField      = "ClassName";
                drpClass.DataValueField     = "Id";
                drpClass.DataSource         = dt_class;
                drpClass.DataBind();
                LoadStudent();
            }
            else
            {
                drpClass.Items.Add(new ListItem("No class Present","0"));
            }
        }

        private void LoadStudent()
        {

            drp_Student.Items.Clear();
            DataSet dt_Student = MyClass.GetStudentlistWithStudentIdAndName(int.Parse(drpClass.SelectedValue), MyUser.CurrentBatchId);

            if (dt_Student != null && dt_Student.Tables != null && dt_Student.Tables[0].Rows.Count > 0)
            {
                drp_Student.DataTextField   = "StudentName";
                drp_Student.DataValueField  = "Id";
                drp_Student.DataSource      = dt_Student;
                drp_Student.DataBind();
                btnSelect.Enabled = true; 
                plchfeedbackarea.Controls.Clear();
                userfeedback.Visible = false;
                btnSubmit.Visible = false;
            }
            else
            {
                btnSelect.Enabled = false;
                drp_Student.Items.Add(new ListItem("No Students Present", "0"));
                plchfeedbackarea.Controls.Clear();
                userfeedback.Visible = false;
                btnSubmit.Visible = false;
                
            }
        }

        private void LoadIncidentDetails()
        {
            int FeedbackMasterId = 0;
            int needApproval = 0;
            FeedbackClass[] incident = MyIncedent.GetFeedBackDetailsAssociatedClass(int.Parse(drpClass.SelectedValue), out FeedbackMasterId, out needApproval);

            if (incident!=null  && incident.Length > 0)
            {
                dynamicRdoBtn = new RadioButtonList[incident.Length];
                hdn_MasterId.Value      = FeedbackMasterId.ToString();
                hdn_NeedApproval.Value  = needApproval.ToString();


                RadioButtonList rdoFeedvback;
                Label           lblIncident;

                Table table = new Table();
                table.Width = Unit.Percentage(100);
                TableRow    row1;
                TableCell   cell1;
                TableCell   cell2;

                for (int i = 0; i < incident.Length; i++)
                {
                    row1    = new TableRow();
                    table.Rows.Add(row1);

                    cell1   = new TableCell();
                    cell2   = new TableCell();

                    row1.Cells.Add(cell1);
                    row1.Cells.Add(cell2);


                    lblIncident         = new Label();
                    lblIncident.ID      = incident[i].IncidentType.id.ToString();
                    lblIncident.Text    = incident[i].IncidentType.Values;

                    cell1.Controls.Add(lblIncident);
                    rdoFeedvback        = new RadioButtonList();


                    rdoFeedvback.ID = "rdo_" + i;

                    for (int j = 0; j < incident[i].Incidentheadings.Count; j++)
                    {
                        rdoFeedvback.Items.Add(new ListItem(incident[i].Incidentheadings[j].Values, incident[i].Incidentheadings[j].id.ToString()));
                    }

                    dynamicRdoBtn[i] = rdoFeedvback;
                    cell2.Controls.Add(rdoFeedvback);

                    row1        = new TableRow();
                    row1.Height = Unit.Percentage(10);

                    table.Rows.Add(row1);
                    cell1       = new TableCell { Text = "<hr/>" };

                    cell1.ColumnSpan = 2;
                    row1.Cells.Add(cell1); 
                }
                plchfeedbackarea.Controls.Add(table);
                // plchfeedbackarea.
            
                btnSubmit.Visible = true;
            }
            else
            {
                btnSubmit.Visible = false;
                lblMessage.Text = "Feedback master does not configured.";
            }

        }       

    }
}
