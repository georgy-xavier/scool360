using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WinBase;

namespace WinErAcademics
{
    public partial class MessageInbox : System.Web.UI.Page
    {
        //private ConfigManager MyConfiMang;
        private StaffManager MyStaffMang;

        private KnowinUser MyUser;
        private DataSet MydataSet;
        private SchoolClass objSchool = null;
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
                    LoadMessageInbox("Date", "Desc");

                }
            }
        }

        private void LoadMessageInbox(string _SortExp, string _SortDir)
        {
            Lbl_ApprovellistCount.Text = "";
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

            
           // MydataSet=MyParent.GetMessages(MyUser.User_Id,2);//changed for new upadtes in query //AMK
             MydataSet = MyParent.GetAllMessages(MyUser.User_Id, 2);
            _mysqlObj.CloseConnection();

            if (MydataSet!=null&& MydataSet.Tables[0]!=null&& MydataSet.Tables[0].Rows.Count > 0)
            {

                DataTable dtAccountData = MydataSet.Tables[0];
                DataView dataView = new DataView(dtAccountData);
                dataView.Sort = _SortExp + " " + _SortDir;

                GrdMessage.Columns[0].Visible = true;
                GrdMessage.DataSource = dataView;
                GrdMessage.DataBind();
                Lbl_Note.Text = "";
                GrdMessage.Columns[0].Visible = false;



                //Lbl_ApprovellistCount.Text = MydataSet.Tables[0].Rows.Count + " messages found";
            }
            else
            {
                GrdMessage.DataSource = null;
                GrdMessage.DataBind();
                Lbl_Note.Text = "No messages found.";
                Lbl_ApprovellistCount.Text = 0 + " Messages";
            }
        }

             

        protected void Grd_ApprovelList_Sorting(object sender, GridViewSortEventArgs e)
        {
            LoadMessageInbox(e.SortExpression, GetSortDirection(e.SortExpression));
        }

        protected void GrdMessage_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdMessage.PageIndex = e.NewPageIndex;
            LoadMessageInbox(Session["SortExpression"].ToString(), Session["SortDirection"].ToString());
        }
        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = Session["SortExpression"] as string;


            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = Session["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }
            // Save new values in ViewState.
            Session["SortDirection"] = sortDirection;
            Session["SortExpression"] = column;

            return sortDirection;
        }

        protected void GrdMessage_RowDataBound(object sender, GridViewRowEventArgs e)
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



        protected void GrdMessage_SelectedIndexChanged(object sender, EventArgs e)
        {
            int threadId=0;


            int.TryParse(GrdMessage.SelectedRow.Cells[0].Text.ToString(), out threadId);

           // Response.Redirect("MessageThreadAccdmics.aspx?ThreadId=" + threadId);
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "openIncpopup('MessageThreadAccdmics.aspx?ThreadId=" + threadId + "');", true);
            updateReadStatus(threadId);
        }

        private void updateReadStatus(int thrdId)
        {
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
            string sql = " update tblparent_mail set MessageReadStatus='READ' where ThreadId=" + thrdId;
            _mysqlObj.ExecuteQuery(sql);
        }
        





        
    }
}
