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
    public partial class WebForm22 : System.Web.UI.Page
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
            if (!MyUser.HaveActionRignt(65))
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

            string sql = "select tblincedent.Id as IncidentId, tblincedent.Title, tblincedenttype.`Type` , tblincedent.Description,tblincedent.`Point`  from tblincedent inner join tblincedenttype on tblincedenttype.Id= tblincedent.TypeId  where (tblincedent.Status='Created' or tblincedent.Status='Need Approval') and ( tblincedent.ApproverId =" + MyUser.UserId + " or tblincedent.ApproverId = 0 ) and tblincedenttype.IncidentType='NORMAL' ";
            MyDataSet = MyIncedent.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyDataSet.Tables[0].Rows.Count > 0)
            {
                lbl_ApproveMessage.Text = "";
                ViewState["Incidents"] = MyDataSet;
                Grd_Incident.DataSource = MyDataSet;  
                Grd_Incident.DataBind();
                FillDetails();
                Grd_Incident.Columns[1].Visible = false;
            }  
            else
            {
                lbl_ApproveMessage.Text = "No more incidents need approval";
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

        protected void Grd_Incident_SelectedIndexChanged(object sender, EventArgs e)
        {
            int IncidentId = int.Parse(Grd_Incident.SelectedRow.Cells[1].Text.ToString());
            AddDetailsTopopUp(IncidentId);
            this.MPE_IncidentPopUp.Show();
            
        }

        private void AddDetailsTopopUp(int IncidentId)
        {
            DateTime _IncedentDate;
            DateTime _CreatedDate;
            string sql = "select tblincedenttype.`Type`,(select tblview_user.SurName from  tblview_user where tblview_user.Id= tblincedent.CreatedUserId)  as SurName, tblincedent.UserType, tblincedent.CreatedDate , tblincedent.IncedentDate , tblincedent.Title , tblincedent.Description , tblincedent.AssoUser from tblincedent inner join tblincedenttype on tblincedenttype.Id = tblincedent.TypeId where tblincedent.Id = " + IncidentId + " and tblincedenttype.IncidentType='NORMAL' ";
             MyReader = MyIncedent.m_MysqlDb.ExecuteQuery(sql); 
             if (MyReader.HasRows)
             {
                 Txt_Type.Text = MyReader.GetValue(0).ToString();
                 Txt_CreatedUser.Text = MyReader.GetValue(1).ToString();
                 Txt_UserType.Text = MyReader.GetValue(2).ToString();                
              _IncedentDate = DateTime.Parse(MyReader.GetValue(3).ToString());
                 //_IncedentDate = MyUser.GetDareFromText(MyReader.GetValue(3).ToString());
                 //Txt_IncidentDate.Text = _IncedentDate.ToString("dd-MM-yyyy");
              Txt_IncidentDate.Text = _IncedentDate.Date.Day+"/"+_IncedentDate.Date.Month+"/"+_IncedentDate.Date.Year;

               _CreatedDate = DateTime.Parse(MyReader.GetValue(4).ToString());
               //  _CreatedDate = MyUser.GetDareFromText(MyReader.GetValue(4).ToString());
                 //Txt_CreatedDate.Text = _CreatedDate.ToString("dd-MM-yyyy");
               Txt_CreatedDate.Text = _CreatedDate.Date.Day + "/" + _CreatedDate.Date.Month + "/" + _CreatedDate.Date.Year;

                _IncedentDate = DateTime.Parse(MyReader.GetValue(4).ToString());
                 //Txt_IncidentDate.Text = _IncedentDate.ToString("dd-MM-yyyy");
                Txt_IncidentDate.Text = _IncedentDate.Date.Day + "/" + _IncedentDate.Date.Month + "/" + _IncedentDate.Date.Year;

                 _CreatedDate = DateTime.Parse(MyReader.GetValue(3).ToString());
                 //Txt_CreatedDate.Text = _CreatedDate.ToString("dd-MM-yyyy");
                 Txt_CreatedDate.Text = _CreatedDate.Date.Day + "/" + _CreatedDate.Date.Month + "/" + _CreatedDate.Date.Year;

                Txt_Title.Text = MyReader.GetValue(5).ToString();
                 Txt_Desc.Text = MyReader.GetValue(6).ToString();
                 Txt_UserId.Text = MyReader.GetValue(7).ToString();
                 Txt_IncidentId.Text = IncidentId.ToString();
             }
             if ((Txt_UserType.Text.Trim() == "student") && (Txt_UserId.Text.Trim() != ""))
            {
                Lbl_Class.Visible = true;
                Txt_Class.Visible = true;
                sql = "select tblstudent.StudentName , tblclass.ClassName from tblstudentclassmap inner join tblstudent on tblstudent.Id = tblstudentclassmap.StudentId inner join tblclass on tblclass.Id = tblstudentclassmap.ClassId WHERE tblstudent.Id = " + int.Parse(Txt_UserId.Text.Trim()) + " and tblstudent.Status <> -1  and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + "";
                MyReader = MyIncedent.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Txt_ReportedTo.Text = MyReader.GetValue(0).ToString();
                    Txt_Class.Text = MyReader.GetValue(1).ToString();
                }
            }
             else if ((Txt_UserType.Text.Trim().ToLowerInvariant() == "staff") && ((Txt_UserId.Text.Trim() != "")))
            {
                Lbl_Class.Visible = false;
                Txt_Class.Visible = false;
                sql = "select tblview_user.SurName from  tblview_user WHERE tblview_user.Id = " + int.Parse(Txt_UserId.Text.Trim());
                MyReader = MyIncedent.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Txt_ReportedTo.Text = MyReader.GetValue(0).ToString();
                    //Txt_Class.Text = MyReader.GetValue(1).ToString();
                }
            }
             else if ((Txt_UserType.Text.Trim()=="Class") && ((Txt_UserId.Text.Trim() !="")))
            {
                Lbl_Class.Visible =false;
                Txt_Class.Visible = false;
                sql=" select tblclass.ClassName from tblclass inner join tblincedent on tblincedent.AssoUser=tblclass.Id WHERE tblincedent.Id=" +IncidentId + "";
                MyReader = MyIncedent.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Txt_ReportedTo.Text = MyReader.GetValue(0).ToString();
                    //Txt_Class.Text = MyReader.GetValue(1).ToString();
                }
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

        //protected void Lnk_Select_Click(object sender, EventArgs e)
        //{
        //    if (Lnk_Select.Text == "Select All")
        //    {
        //        foreach (GridViewRow gv in Grd_Incident.Rows)
        //        {
        //            CheckBox cb = (CheckBox)gv.FindControl("Chk_Incident");
        //            cb.Checked = true;

        //        }
        //        Lnk_Select.Text = "None";
        //    }
        //    else
        //    {
        //        foreach (GridViewRow gv in Grd_Incident.Rows)
        //        {
        //            CheckBox cb = (CheckBox)gv.FindControl("Chk_Incident");
        //            cb.Checked = false;

        //        }
        //        Lnk_Select.Text = "Select All";
        //    }

        //}

        protected void Btn_Approve_Click(object sender, EventArgs e)
        {
            int flag = 0;
            foreach (GridViewRow gv in Grd_Incident.Rows)
            {
                CheckBox cb = (CheckBox)gv.FindControl("Chk_Incident");
                if (cb.Checked)
                {
                    flag = 1;
                    //if (MyIncedent.OnLIineApprovel(int.Parse(gv.Cells[1].Text),out message))
                    //{
                    MyIncedent.UpdateStatus(int.Parse(gv.Cells[1].Text.ToString()), MyUser.UserId, MyUser.CurrentBatchId);
                        WC_MessageBox.ShowMssage("Incident is approved");
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Approve Incident", "Approve the Incidents reported about student", 1);
                        
                   // }
                   // else
                   //{
                   //    WC_MessageBox.ShowMssage(message);
                   //}
                }
            }
            if (flag == 0)
            {
                WC_MessageBox.ShowMssage("Select an incident to approve");
            }
            FillGrid();
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
                    //if (MyIncedent.OnLineReject(int.Parse(gv.Cells[1].Text), out message))
                    //{
                    MyIncedent.RejectIncident(int.Parse(gv.Cells[1].Text.ToString()), MyUser.UserId, MyUser.CurrentBatchId);
                        WC_MessageBox.ShowMssage("Incident is Rejected");
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Reject Incident", "Reject the Incidents reported about student", 1);
                        
                    //}
                    //else
                    //{
                    //    WC_MessageBox.ShowMssage(message);
                    //}
                }

            }
            if (flag == 0)
            {
                WC_MessageBox.ShowMssage("Select an incident to reject");
            }
            FillGrid();

        }

        protected void Btn_PopUpApprove_Click(object sender, EventArgs e)
        {
            if (Txt_IncidentId.Text.Trim() != "")
            {
                
                //if (MyIncedent.OnLIineApprovel(int.Parse(Txt_IncidentId.Text), out message))
                //{
                MyIncedent.UpdateStatus(int.Parse(Txt_IncidentId.Text.Trim()), MyUser.UserId, MyUser.CurrentBatchId);
                    FillGrid();
                    WC_MessageBox.ShowMssage("Incident is approved");
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Approve Incident", "Approve the Incidents Reported about " + Txt_UserType.Text.ToString() + " " + Txt_ReportedTo.Text.ToString() + " ", 1);
                //}
                //else
                //{
                //    WC_MessageBox.ShowMssage(message);
                //    FillGrid();
                //}
            }
            else
            {
                WC_MessageBox.ShowMssage("Try again");
            }
        }

        protected void Btn_popUpCancel_Click(object sender, EventArgs e)
        {
            
            if (Txt_IncidentId.Text.Trim() != "")
            {
                //if (MyIncedent.OnLineReject(int.Parse(Txt_IncidentId.Text), out message))
                //{
                MyIncedent.RejectIncident(int.Parse(Txt_IncidentId.Text.Trim()), MyUser.UserId, MyUser.CurrentBatchId);
                    FillGrid();
                    WC_MessageBox.ShowMssage("Incident is rejected");
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Reject Incident", "Reject the Incidents Reported  about " + Txt_UserType.Text + " " + Txt_ReportedTo.Text + " ", 1);
                //}
                //else
                //{
                //    WC_MessageBox.ShowMssage(message);
                //    FillGrid();
                //}
            }
            else
            {
                WC_MessageBox.ShowMssage("Try again");
            }
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
