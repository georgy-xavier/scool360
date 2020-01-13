using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.Odbc;
using WinBase;

namespace WinEr
{
    public partial class Groupapproval : System.Web.UI.Page
    {
        private Incident MyIncedent;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet = null;
       
        string message;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
           
            MyUser = (KnowinUser)Session["UserObj"];
            if (!MyUser.HaveActionRignt(3043))
            {
                Response.Redirect("RoleErr.htm");
            }
            MyIncedent = MyUser.GetIncedentObj();
            if (MyIncedent == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadIncedents();
                }
            }
        }

        private void LoadIncedents()
        {
            Grd_Incident.PageIndex = 0;
            FillGrid();
        }

        private void FillGrid()
        {
            Grd_Incident.Columns[1].Visible = true;

            string sql = "select tbl_groupapplicationform.Id , tbl_groupapplicationform.Name, tbl_groupapplicationform.Stud_Id , tbl_gr_master.GroupName,tbl_groupapplicationform.Group_Id as GroupId from tbl_groupapplicationform inner join tbl_gr_master on tbl_gr_master.Id= tbl_groupapplicationform.Group_Id  where tbl_groupapplicationform.status = 2";
            MyDataSet = MyIncedent.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyDataSet.Tables[0].Rows.Count > 0)
            {
                lbl_ApproveMessage.Text = "";
                ViewState["Incidents"] = MyDataSet;
                Grd_Incident.DataSource = MyDataSet;
                Grd_Incident.DataBind();
               // FillDetails();
                Grd_Incident.Columns[1].Visible = false;
            }
            else
            {
                lbl_ApproveMessage.Text = "No approval pending";
                Grd_Incident.DataSource = null;
                Grd_Incident.DataBind();
                //Lnk_Select.Visible = false;
                Btn_Approve.Visible = false;
                Btn_Reject.Visible = false;
            }
        }

        private void FillDetails()
        {
            foreach (GridViewRow gv in Grd_Incident.Rows)

            {
                Label lbl_CreatedUser = (Label)gv.FindControl("Lbl_CreatedUser");
                Label Lbl_Pupil = (Label)gv.FindControl("Lbl_PupilName");
                Label Lbl_PupilType = (Label)gv.FindControl("Lbl_PupilType");
                lbl_CreatedUser.Text = MyIncedent.GetCreatedUser(int.Parse(gv.Cells[1].Text.ToString()));
                Lbl_PupilType.Text = MyIncedent.GetPupilType(int.Parse(gv.Cells[1].Text.ToString()));
                Lbl_Pupil.Text = MyIncedent.GetPupilname(int.Parse(gv.Cells[1].Text.ToString()), Lbl_PupilType.Text);
            }
        }

        

        

        protected void Grd_Incident_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Incident.PageIndex = e.NewPageIndex;
            //FillGrid();
            DataSet IncidentDataSet = (DataSet)ViewState["Incidents"];
            Grd_Incident.Columns[1].Visible = true;
            Grd_Incident.DataSource = IncidentDataSet;
            Grd_Incident.DataBind();
            FillDetails();
            Grd_Incident.Columns[1].Visible = false;
        }

        protected void Grd_IncidentDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowState == DataControlRowState.Alternate)
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='gray';this.style.cursor='hand'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
                }
                else
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='gray';this.style.cursor='hand'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#F7F7DE';");
                }
                //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Grd_Student, "Select$" + e.Row.RowIndex);
            }
        }

    

        protected void Btn_Approve_Click(object sender, EventArgs e)
        {
            int flag = 0;
            foreach (GridViewRow gv in Grd_Incident.Rows)
            {
                CheckBox cb = (CheckBox)gv.FindControl("Chk_Incident");
                if (cb.Checked)
                {
                    flag = 1;


                    string sql = "select id from tbl_gr_groupusermap where UserId =" + int.Parse(gv.Cells[3].Text.ToString()) + "";
                    MyReader = MyIncedent.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        UpdateMapUser(0, int.Parse(gv.Cells[3].Text.ToString()), gv.Cells[2].Text.ToString(), int.Parse(gv.Cells[5].Text.ToString()));
                    }
                    else
                    {
                        MapUser(0, int.Parse(gv.Cells[3].Text.ToString()), gv.Cells[2].Text.ToString(), int.Parse(gv.Cells[5].Text.ToString()));
                    }

                      
                    UpdateGRp(int.Parse(gv.Cells[3].Text.ToString()), gv.Cells[2].Text.ToString(), int.Parse(gv.Cells[5].Text.ToString()));
                    WC_MessageBox.ShowMssage("Request is approved");
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Approve Group", "Approve the group application reported about student", 1);

                }
            }
            if (flag == 0)
            {
                WC_MessageBox.ShowMssage("Select an incident to approve");
            }
            FillGrid();
        }
        
        public void MapUser(int usertype, int userid, string username, int groupid)
        {
            string sql = "";
            sql = "Insert Into tbl_gr_groupusermap(UserName,Type,GroupId,UserId) Values('" + username + "'," + usertype + "," + groupid + "," + userid + ")";
            MyIncedent.m_MysqlDb.ExecuteQuery(sql);

        }
        public void UpdateMapUser(int usertype, int userid, string username, int groupid)
        {
            string sql = "";
            sql = "update tbl_gr_groupusermap SET GroupId = " + groupid + " where UserId = " + userid + "";
            MyIncedent.m_MysqlDb.ExecuteQuery(sql);

        }
        public void UpdateGRp(int userid, string username, int groupid)
        {
            string sql = "";
            sql = "update tbl_groupapplicationform set tbl_groupapplicationform.status = 0 where tbl_groupapplicationform.Stud_Id = " + userid + " and tbl_groupapplicationform.Group_Id = " + groupid + "";
            MyIncedent.m_MysqlDb.ExecuteQuery(sql);
        }
        protected void Btn_Reject_Click(object sender, EventArgs e)
        {
            int flag = 0;

            foreach (GridViewRow gv in Grd_Incident.Rows)
            {
                CheckBox cb = (CheckBox)gv.FindControl("Chk_Incident");
                if (cb.Checked)
                {
                    flag = 1;
                    
                    RejectIncident(int.Parse(gv.Cells[3].Text.ToString()), gv.Cells[2].Text.ToString(), int.Parse(gv.Cells[5].Text.ToString()));
                    WC_MessageBox.ShowMssage("Group approval is Rejected");
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Reject Group ", "Reject the group reported about student", 1);

                    
                }

            }
            if (flag == 0)
            {
                WC_MessageBox.ShowMssage("Select an incident to reject");
            }
            FillGrid();

        }
        public void RejectIncident( int userid, string username, int groupid)
        {
            string sql = "";
            sql = "update tbl_groupapplicationform set tbl_groupapplicationform.status = 1 where tbl_groupapplicationform.Stud_Id = " + userid + " and tbl_groupapplicationform.Group_Id = " + groupid + "";
            MyIncedent.m_MysqlDb.ExecuteQuery(sql);
        }

        protected void Grd_Incident_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["Incidents"] != null)
            {
                DataSet _sortlDS = (DataSet)ViewState["Incidents"];

                if (_sortlDS.Tables[0].Rows.Count > 0)
                {
                    DataTable dtData = _sortlDS.Tables[0];

                    DataView dataView = new DataView(dtData);

                    dataView.Sort = e.SortExpression + " " + GetSortDirection1(e.SortExpression);

                    Grd_Incident.Columns[1].Visible = true;
                    Grd_Incident.DataSource = dataView;
                    Grd_Incident.DataBind();
                    FillDetails();
                    Grd_Incident.Columns[1].Visible = false;
                }
            }
        }

        private string GetSortDirection1(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = Session["SortExpression1"] as string;


            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = Session["SortDirection1"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }
            // Save new values in ViewState.
            Session["SortDirection1"] = sortDirection;
            Session["SortExpression1"] = column;

            return sortDirection;
        }

    }
}