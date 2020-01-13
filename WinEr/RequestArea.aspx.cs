using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WinBase;

namespace WinEr
{
    public partial class RequestArea : System.Web.UI.Page
    {

        private ConfigManager   MyConfiMang;
        private StaffManager    MyStaffMang;

        private KnowinUser      MyUser;
        private DataSet         MydataSet;
        private SchoolClass     objSchool = null;

        private int REQUEST_TYPE    = 1;
        private int COMPLAINT_TYPE  = 2;
        private int FEEDBACK_TYPE   = 3;
        private int USERTYPE        = 2;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }

            MyUser = (KnowinUser)Session["UserObj"];
            MyStaffMang = MyUser.GetStaffObj();
            if (MyStaffMang == null)
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
                    LoadComplaintInbox("Date", "Desc");
                    LoadRequestInbox("Date", "Desc");
                    LoadFeedBackInbox("Date", "Desc");

                }
            }
        }

     
        #region COMPLAINT AREA

        private void LoadComplaintInbox(string _SortExp, string _SortDir)
        {
            lblcomplaintcount.Text = "";
            Session["SortDirection"] = _SortDir;
            Session["SortExpression"] = _SortExp;

            string _conStr = WinerUtlity.SingleSchoolConnectionString;
            if (WinerUtlity.NeedCentrelDB())
            {
                if (Session[WinerConstants.SessionSchool] == null)
                {
                    Response.Redirect("Logout.aspx");
                }
                objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                _conStr = objSchool.ConnectionString;
            }

            MysqlClass _mysqlObj = new MysqlClass(_conStr);
            ParentLogin MyParent = new ParentLogin(_mysqlObj, objSchool);


            MydataSet = MyParent.GetService_request_complaint_feedback(USERTYPE, COMPLAINT_TYPE);
            _mysqlObj.CloseConnection();

            if (MydataSet != null && MydataSet.Tables[0] != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                DataTable dtAccountData = MydataSet.Tables[0];
                DataView dataView = new DataView(dtAccountData);
                dataView.Sort = _SortExp + " " + _SortDir;

                Grdcomplaints.Columns[0].Visible = true;
                Grdcomplaints.DataSource = dataView;
                Grdcomplaints.DataBind();

                Grdcomplaints.Columns[0].Visible = false;

                lblcomplaintcount.Text = MydataSet.Tables[0].Rows.Count + " complaints";
            }
            else
            {
                Grdcomplaints.DataSource = null;
                Grdcomplaints.DataBind();
              
               lblcomplaintcount.Text =   0 + " complaints";
            }
        }


        protected void Grdcomplaints_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex < 0)
                return;

            ViewState["OrigData"] = e.Row.Cells[3].Text;
            if (e.Row.Cells[3].Text.Length >= 30) //Just change the value of 30 based on your requirements
            {
                e.Row.Cells[3].Text = e.Row.Cells[3].Text.Substring(0, 30) + "...";
                e.Row.Cells[3].ToolTip = ViewState["OrigData"].ToString();
            }
        }

        protected void Grdcomplaints_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grdcomplaints.PageIndex = e.NewPageIndex;
            LoadComplaintInbox(Session["SortExpression"].ToString(), Session["SortDirection"].ToString());
        }

        protected void Grdcomplaints_SelectedIndexChanged(object sender, EventArgs e)
        {

            int threadId = 0;

            int.TryParse(Grdcomplaints.SelectedRow.Cells[0].Text.ToString(), out threadId);
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "openIncpopup('ServicesThreads.aspx?ThreadId=" + threadId + "&RequestType=" + COMPLAINT_TYPE + "');", true);


        }

        #endregion

        #region REQUEST AREA

        private void LoadRequestInbox(string _SortExp, string _SortDir)
        {
            lblreqcount.Text = "";
            Session["SortDirection"] = _SortDir;
            Session["SortExpression"] = _SortExp;

            string _conStr = WinerUtlity.SingleSchoolConnectionString;
            if (WinerUtlity.NeedCentrelDB())
            {
                if (Session[WinerConstants.SessionSchool] == null)
                {
                    Response.Redirect("Logout.aspx");
                }
                objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                _conStr = objSchool.ConnectionString;
            }

            MysqlClass _mysqlObj = new MysqlClass(_conStr);
            ParentLogin MyParent = new ParentLogin(_mysqlObj, objSchool);


            MydataSet = MyParent.GetService_request_complaint_feedback(USERTYPE, REQUEST_TYPE);
            _mysqlObj.CloseConnection();

            if (MydataSet != null && MydataSet.Tables[0] != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                DataTable dtAccountData = MydataSet.Tables[0];
                DataView dataView = new DataView(dtAccountData);
                dataView.Sort = _SortExp + " " + _SortDir;

                GrdRequests.Columns[0].Visible = true;
                GrdRequests.DataSource = dataView;
                GrdRequests.DataBind();

                GrdRequests.Columns[0].Visible = false;

                lblreqcount.Text = MydataSet.Tables[0].Rows.Count + " Requests";
            }
            else
            {
                GrdRequests.DataSource = null;
                GrdRequests.DataBind();

                lblreqcount.Text = 0 + " Requests";
            }
        }

        protected void GrdRequests_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex < 0)
                return;

            ViewState["OrigData"] = e.Row.Cells[3].Text;
            if (e.Row.Cells[3].Text.Length >= 30) //Just change the value of 30 based on your requirements
            {
                e.Row.Cells[3].Text = e.Row.Cells[3].Text.Substring(0, 30) + "...";
                e.Row.Cells[3].ToolTip = ViewState["OrigData"].ToString();
            }
        }

        protected void GrdRequests_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdRequests.PageIndex = e.NewPageIndex;
            LoadRequestInbox(Session["SortExpression"].ToString(), Session["SortDirection"].ToString());
        }

        protected void GrdRequests_SelectedIndexChanged(object sender, EventArgs e)
        {
            int threadId = 0;

            int.TryParse(GrdRequests.SelectedRow.Cells[0].Text.ToString(), out threadId);
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "openIncpopup('ServicesThreads.aspx?ThreadId=" + threadId + "&RequestType=" + REQUEST_TYPE + "');", true);

        }

        #endregion

        #region FEEDBACK AREA

        private void LoadFeedBackInbox(string _SortExp, string _SortDir)
        {
            lblfeedbackcount.Text = "";
            Session["SortDirection"] = _SortDir;
            Session["SortExpression"] = _SortExp;

            string _conStr = WinerUtlity.SingleSchoolConnectionString;
            if (WinerUtlity.NeedCentrelDB())
            {
                if (Session[WinerConstants.SessionSchool] == null)
                {
                    Response.Redirect("Logout.aspx");
                }
                objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                _conStr = objSchool.ConnectionString;
            }

            MysqlClass _mysqlObj = new MysqlClass(_conStr);
            ParentLogin MyParent = new ParentLogin(_mysqlObj, objSchool);


            MydataSet = MyParent.GetService_request_complaint_feedback(USERTYPE, FEEDBACK_TYPE);
            _mysqlObj.CloseConnection();

            if (MydataSet != null && MydataSet.Tables[0] != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                DataTable dtAccountData = MydataSet.Tables[0];
                DataView dataView = new DataView(dtAccountData);
                dataView.Sort = _SortExp + " " + _SortDir;

                GrdFeedBack.Columns[0].Visible = true;
                GrdFeedBack.DataSource = dataView;
                GrdFeedBack.DataBind();

                GrdFeedBack.Columns[0].Visible = false;

                lblfeedbackcount.Text = MydataSet.Tables[0].Rows.Count + " Feedback";
            }
            else
            {
                GrdFeedBack.DataSource = null;
                GrdFeedBack.DataBind();

                lblfeedbackcount.Text = 0 + " Feedback";
            }
        }

        protected void GrdFeedBack_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex < 0)
                return;

            ViewState["OrigData"] = e.Row.Cells[3].Text;
            if (e.Row.Cells[3].Text.Length >= 30) //Just change the value of 30 based on your requirements
            {
                e.Row.Cells[3].Text = e.Row.Cells[3].Text.Substring(0, 30) + "...";
                e.Row.Cells[3].ToolTip = ViewState["OrigData"].ToString();
            }
        }

        protected void GrdFeedBack_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdFeedBack.PageIndex = e.NewPageIndex;
            LoadFeedBackInbox(Session["SortExpression"].ToString(), Session["SortDirection"].ToString());
        }

        protected void GrdFeedBack_SelectedIndexChanged(object sender, EventArgs e)
        {

            int threadId = 0;

            int.TryParse(GrdFeedBack.SelectedRow.Cells[0].Text.ToString(), out threadId);
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "openIncpopup('ServicesThreads.aspx?ThreadId=" + threadId + "&RequestType=" + FEEDBACK_TYPE + "');", true);

        }

        #endregion
    }
}
