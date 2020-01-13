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

namespace WinErParentLogin
{
    public partial class Servicecomplaints : System.Web.UI.Page
    {
        private ParentInfoClass MyParentInfo;
        private OdbcDataReader MyReader = null;
        private Attendance MyAttendence;
        private DataSet MydataSet, EventDataset;
        ParentLogin MyParent;
             MysqlClass _mysqlObj;
        private   const int  SERVICE_TYPE=2;

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
                MyHeader.Text = "Complaints";
               LoadMessageInbox("Date", "Desc");
                loadrequestType();
            }
        }

        private void loadrequestType()
        {
            drp_servicetype.Items.Clear();
             _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
             MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);

             DataSet Dt = MyParent.getRequestType(SERVICE_TYPE);

            if (Dt != null && Dt.Tables != null && Dt.Tables[0].Rows.Count > 0)
            {
                drp_servicetype.DataTextField = "ServiceName";
                drp_servicetype.DataValueField = "Id";
                drp_servicetype.DataSource = Dt.Tables[0];
                drp_servicetype.DataBind();
            }
            else
            {
                drp_servicetype.Items.Add(new ListItem("school does not allow to submit complaints","0"));

                btnsendnewmessage.Enabled = false;
            }
              _mysqlObj.CloseConnection();
        }


        protected void btnsendnewmessage_Click(object sender, EventArgs e)
        {
            string serviceId = drp_servicetype.SelectedValue;
            string serviceHeading = drp_servicetype.SelectedItem.Text;
            string serviceDescription = txtnewmessage.Text;
           
            int fromUserType=1;
            int toUserType = 2;
            int toUserId = 1;
            int fromUserId = MyParentInfo.StudentId;
             _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
             MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);

             MyParent.createNewServiceLog(serviceId, SERVICE_TYPE, serviceHeading, serviceDescription, fromUserType, toUserType, fromUserId, toUserId);

             DBLogClass _DblogObj = new DBLogClass(_mysqlObj);

             _DblogObj.LogToDb(MyParentInfo.ParentName, "ParentLogin_Complaint", " Creaetd new complaint ," + serviceDescription + "", 2, 2);
             _DblogObj = null;
           
            _mysqlObj.CloseConnection();

  
            
            txtnewmessage.Text = "";
            lblerr.Text = "Your request submitted";
            LoadMessageInbox("Date", "Desc");

           ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "showdirectnewmessage();", true);           
        }



        private void LoadMessageInbox(string _SortExp, string _SortDir)
        {
           // Lbl_ApprovellistCount.Text = "";
            int ReadStatus = -1;

            Session["SortDirection"] = _SortDir;
            Session["SortExpression"] = _SortExp;


            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);

            // int.TryParse(DrpList_MsgStatus.SelectedItem.Value.ToString(), out ReadStatus);

            MydataSet = MyParent.GetAllServiceRequests(MyParentInfo.StudentId, ReadStatus, 1, SERVICE_TYPE);
            _mysqlObj.CloseConnection();

            if (MydataSet != null && MydataSet.Tables[0] != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                
                DataTable dtAccountData = MydataSet.Tables[0];
                DataView dataView = new DataView(dtAccountData);
                dataView.Sort = _SortExp + " " + _SortDir;
                GrdMessage.Columns[0].Visible = true;
                GrdMessage.DataSource = dataView;
                GrdMessage.DataBind();
              //  Lbl_Note.Text = "";
                GrdMessage.Columns[0].Visible = false;


                //Lbl_ApprovellistCount.Text = MydataSet.Tables[0].Rows.Count + " messages found";
            }
            else
            {
                GrdMessage.DataSource = null;
                GrdMessage.DataBind();
              //  Lbl_Note.Text = "No messages found.";
             //   Lbl_ApprovellistCount.Text = 0 + " Messages";
            }
        }

        protected void GrdMessage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex < 0)
                return;

            ViewState["OrigData"] = e.Row.Cells[2].Text;
            if (e.Row.Cells[2].Text.Length >= 30) //Just change the value of 30 based on your requirements
            {
                e.Row.Cells[2].Text = e.Row.Cells[2].Text.Substring(0, 30) + "...";
                e.Row.Cells[2].ToolTip = ViewState["OrigData"].ToString();
            }
        }



        protected void GrdMessage_SelectedIndexChanged(object sender, EventArgs e)
        {
            int threadId = 0;
            int.TryParse(GrdMessage.SelectedRow.Cells[0].Text.ToString(), out threadId);
            lblerror.Text = "";
            LoadMessageThread(threadId);
            //ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "openIncpopup('MessageThread.aspx?ThreadId=" + threadId + "');", true);
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "showthreaddirect();", true);
        }



        protected void GrdMessage_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdMessage.PageIndex = e.NewPageIndex;
            LoadMessageInbox("Date", "Desc");
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
        private void LoadMessageThread(int thrdId)
        {

            hdnthredid.Value = thrdId.ToString();
            int FrmId = 0, FrmType = 0;
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            MydataSet = MyParent.GetServiceThrds(thrdId, 1, SERVICE_TYPE);
            _mysqlObj.CloseConnection();

            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    if (int.Parse(dr["FromUserType"].ToString()) == 1 && int.Parse(dr["FromUserId"].ToString()) == MyParentInfo.StudentId)
                    {
                        dr["Name"] = "Me";
                    }
                }
            }

            grdThreads.Columns[0].Visible = true;
            grdThreads.Columns[1].Visible = true;
            grdThreads.Columns[2].Visible = true;


            DataTable dtMessageData = MydataSet.Tables[0];
            DataView dataView = new DataView(dtMessageData);
            dataView.Sort = "Id" + " " + "Desc";

            grdThreads.DataSource = dataView;
            grdThreads.DataBind();

            string Subject = dataView[0][4].ToString();

            txt_subj.Text = "Re:" + Subject;

            foreach (GridViewRow gv in grdThreads.Rows)
            {
                int.TryParse(gv.Cells[1].Text.ToString(), out FrmId);
                int.TryParse(gv.Cells[2].Text.ToString(), out FrmType);

                if (FrmType == 2)
                {
                    gv.BackColor = Color.WhiteSmoke;
                }

            }
            //grdThreads.DataSource = MydataSet;
            //grdThreads.DataBind();

            grdThreads.Columns[0].Visible = false;
            grdThreads.Columns[1].Visible = false;
            grdThreads.Columns[2].Visible = false;
            grdThreads.HeaderRow.Visible = false;
        }
        protected void btn_msg_Click(object sender, EventArgs e)
        {
            string subj = "", descrption = "";
            int thrdId = 0;
            int.TryParse(hdnthredid.Value, out thrdId);
            subj = txt_subj.Text;
            descrption = txt_message.Text;
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            MyParent.SendServicesThrd(MyParentInfo.StudentId, 1, thrdId, subj, descrption);
            _mysqlObj.CloseConnection();
            txt_message.Text = "";
            txt_subj.Text = "";
            lblerror.Text = "Message has been sent";
            //  MSGBOX.ShowMssage("Message sent");
            LoadMessageThread(thrdId);

            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "showthreaddirect();", true);
        }
    }
}
