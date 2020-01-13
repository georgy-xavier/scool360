using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;
using WinBase;
namespace WinEr
{

    public partial class WebForm27 : System.Web.UI.Page
    {
        private StaffManager MyStaffMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private Incident MyIncident;
        private DataSet MyDataSet;
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];

            if (MyUser.SELECTEDMODE == 1)
            {
                this.MasterPageFile = "~/WinerStudentMaster.master";

            }
            else if (MyUser.SELECTEDMODE == 2)
            {

                this.MasterPageFile = "~/WinerSchoolMaster.master";
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (Session["StaffId"] == null)
            {
                Response.Redirect("ViewStaffs.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStaffMang = MyUser.GetStaffObj();
            MyIncident = MyUser.GetIncedentObj();
            if (MyStaffMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MyIncident == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(71))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    string _SubMenuStr;
                    _SubMenuStr = MyStaffMang.GetSubStaffMangMenuString(MyUser.UserRoleId);
                    this.SubStaffMenu.InnerHtml = _SubMenuStr;
                    Pnl_incidentGrid.Visible = false;
                    LoadIncidentTypeToDrpList();
                    setDrpIndex();
                    LoadIncidents();
                    LoaduserTopData();

                    LoadPreviousBatchesToDropDown();
                    Drp_PreviousBatch.Enabled = false;
                    Drp_PreviousBatch.SelectedValue = MyUser.CurrentBatchId.ToString();
                }
            }
        }

        private void LoadPreviousBatchesToDropDown()
        {
            Drp_PreviousBatch.Items.Clear();
            string sql = "select tblbatch.BatchName, tblbatch.Id from tblbatch where tblbatch.Id BETWEEN "+ (MyUser.CurrentBatchId -3) +" and " + MyUser.CurrentBatchId + " order by tblbatch.Id desc";
            MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_PreviousBatch.Items.Add(li);
                }
            }
        }

        private void LoaduserTopData()
        {

            string _Studstrip = MyStaffMang.ToStripString(int.Parse(Session["StaffId"].ToString()), "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StaffId"].ToString()) + "&type=StaffImage");
            this.StudentTopStrip.InnerHtml = _Studstrip;
        }
 
        private void LoadIncidents()
        {
            Pnl_incidentGrid.Visible = true;
            Grd_Incident.PageIndex = 0;
            FillGrid();
        }

        private void FillGrid()
        {
            string sql = "";
            Grd_Incident.Columns[1].Visible = true;
            Grd_Incident.Columns[4].Visible = true;

            if (Rdb_Batch.SelectedValue == "0")
            {
                sql = "select tblincedent.Id , tblincedent.Title , tblincedenttype.`Type`,tblincedent.`Point`,DATE_FORMAT(tblincedent.IncedentDate,'%d/%m%Y') as IncidentDate from tblincedent inner join tblincedenttype on tblincedenttype.Id = tblincedent.TypeId inner join tbluser on tbluser.Id = tblincedent.AssoUser where tbluser.Id =" + int.Parse(Session["StaffId"].ToString()) + " and tblincedent.`Status` = 'Approved' and tblincedent.UserType='staff'  and  tblincedenttype.IncidentType='NORMAL'  and  tblincedent.BatchId=" + MyUser.CurrentBatchId;
                if (Drp_IncType.SelectedValue != "0")
                {
                    sql = sql + " and tblincedenttype.Id=" + int.Parse(Drp_IncType.SelectedValue) + " order by IncidentDate";
                }
            }
            else if (Rdb_Batch.SelectedValue == "1")
            {
                sql = "select tblview_incident.Id , tblview_incident.Title , tblincedenttype.`Type`,tblview_incident.`Point`,DATE_FORMAT(tblview_incident.IncedentDate,'%d/%m%Y') as IncidentDate from tblview_incident inner join tblincedenttype on tblincedenttype.Id = tblview_incident.TypeId inner join tbluser on tbluser.Id = tblview_incident.AssoUser where tbluser.Id =" + int.Parse(Session["StaffId"].ToString()) + " and tblview_incident.`Status` = 'Approved' and tblview_incident.UserType='staff'  and  tblincedenttype.IncidentType='NORMAL'  and tblview_incident.BatchId=" + Drp_PreviousBatch.SelectedValue;
                if (Drp_IncType.SelectedValue != "0")
                {
                    sql = sql + " and tblincedenttype.Id=" + int.Parse(Drp_IncType.SelectedValue) + " order by IncidentDate";
                }
            }
            else
            {
                sql = "select tblview_incident.Id , tblview_incident.Title , tblincedenttype.`Type`,tblview_incident.`Point`,DATE_FORMAT(tblview_incident.IncedentDate,'%d/%m%Y') as IncidentDate from tblview_incident inner join tblincedenttype on tblincedenttype.Id = tblview_incident.TypeId inner join tbluser on tbluser.Id = tblview_incident.AssoUser where tbluser.Id =" + int.Parse(Session["StaffId"].ToString()) + " and tblview_incident.`Status` = 'Approved'  and  tblincedenttype.IncidentType='NORMAL'  and tblview_incident.UserType='staff'";
                if (Drp_IncType.SelectedValue != "0")
                {
                    sql = sql + " and tblincedenttype.Id=" + int.Parse(Drp_IncType.SelectedValue) + " order by IncidentDate";
                }
            }

            MyDataSet = MyIncident.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyDataSet.Tables[0].Rows.Count > 0)
            {
                ViewState["Incidents"] = MyDataSet;
                lbl_viewIncidentMsg.Text = "";
                Grd_Incident.DataSource = MyDataSet;
                Grd_Incident.DataBind();
                FillDetails();
                Btn_Delete.Enabled = true;
                Lnk_Select.Visible = true;
                Grd_Incident.Columns[1].Visible = false;
                Grd_Incident.Columns[4].Visible = false;

                lbl_Points.Visible = true;
                lbl_TotalPoints.Visible = true;
                Img_Points.Visible = true;
            }
            else
            {
                Grd_Incident.DataSource = null;
                Grd_Incident.DataBind();

                lbl_viewIncidentMsg.Text = "No Incidents found";
                lbl_Points.Visible = false;
                lbl_TotalPoints.Visible = false;
                Btn_Delete.Enabled = false;
                Lnk_Select.Visible = false;
                Img_Points.Visible = false;
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
            string sql = "select tblincedenttype.`Type`, tbluser.SurName , tblincedent.UserType, tblincedent.CreatedDate , tblincedent.IncedentDate , tblincedent.Title , tblincedent.Description , tblincedent.AssoUser from tblincedent inner join tblincedenttype on tblincedenttype.Id = tblincedent.TypeId inner join tbluser on tbluser.Id = tblincedent.CreatedUserId  where tblincedent.Id = " + IncidentId + " and  tblincedenttype.IncidentType='NORMAL' ";
            MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Txt_Type.Text = MyReader.GetValue(0).ToString();
                Txt_CreatedUser.Text = MyReader.GetValue(1).ToString();
                Txt_UserType.Text = MyReader.GetValue(2).ToString();
                _IncedentDate = DateTime.Parse(MyReader.GetValue(3).ToString());
                //_IncedentDate = MyUser.GetDareFromText(MyReader.GetValue(3).ToString());

                Txt_IncidentDate.Text = _IncedentDate.ToString("dd/MM/yyyy");
                _CreatedDate = DateTime.Parse(MyReader.GetValue(4).ToString());
                //_CreatedDate =MyUser.GetDareFromText(MyReader.GetValue(4).ToString());

                Txt_CreatedDate.Text = _CreatedDate.ToString("dd/MM/yyyy");
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
                MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
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
                sql = "select tbluser.SurName from  tbluser WHERE tbluser.Id = " + int.Parse(Txt_UserId.Text.Trim()) + "";
                MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Txt_ReportedTo.Text = MyReader.GetValue(0).ToString();
                    //Txt_Class.Text = MyReader.GetValue(1).ToString();
                }
            }
            else if ((Txt_UserType.Text.Trim() == "Class") && ((Txt_UserId.Text.Trim() != "")))
            {
                Lbl_Class.Visible = false;
                Txt_Class.Visible = false;
                sql = " select tblclass.ClassName from tblclass inner join tblincedent on tblincedent.AssoUser=tblclass.Id WHERE tblincedent.Id=" + IncidentId + "";
                MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
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
            FillGrid();
            Lnk_Select.Text = "Select All";
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

        private void FillDetails()
        {
            int _Points = 0;
            foreach (GridViewRow gv in Grd_Incident.Rows)
            {
                //Label Lbl_Pupil = (Label)gv.FindControl("Lbl_PupilName");
                //Label Lbl_PupilType = (Label)gv.FindControl("Lbl_PupilType");
                Label Lbl_Point = (Label)gv.FindControl("lbl_Point");
                Image Img_Point = (Image)gv.FindControl("Img_Point");
                Lbl_Point.Text = gv.Cells[4].Text.ToString();

                _Points = _Points + int.Parse(gv.Cells[4].Text.ToString());
                if (int.Parse(gv.Cells[4].Text.ToString()) >= 0)
                {
                    Img_Point.ImageUrl = "images/pt1_up.png";
                }
                else
                {
                    Img_Point.ImageUrl = "images/pt1_dwn.png";
                }

                //if (Rdb_Batch.SelectedValue == "0")
                //{
                //    Lbl_PupilType.Text = MyIncident.GetPupilType(int.Parse(gv.Cells[1].Text.ToString()));
                //    Lbl_Pupil.Text = MyIncident.GetPupilname(int.Parse(gv.Cells[1].Text.ToString()), Lbl_PupilType.Text);
                //}
                //else
                //{
                //    Lbl_PupilType.Text = MyIncident.GetPupilTypeForPreviousBatch(int.Parse(gv.Cells[1].Text.ToString()));
                //    Lbl_Pupil.Text = MyIncident.GetPupilnameForPreviousBatch(int.Parse(gv.Cells[1].Text.ToString()), Lbl_PupilType.Text);
                //}
            }
            lbl_TotalPoints.Text = _Points.ToString();
            if (_Points < 0)
            {
                Img_Points.ImageUrl = "images/pt1_dwn.png";
                lbl_TotalPoints.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                Img_Points.ImageUrl = "images/pt1_up.png";
                lbl_TotalPoints.ForeColor = System.Drawing.Color.LimeGreen;
            }
        }

        private void setDrpIndex()
        {
            string TypeId = "";

            try
            {
                TypeId = Request.QueryString["id"].ToString();
            }
            catch
            {
            }
            if (TypeId != "")
            {
                int Index = int.Parse(TypeId);
                Drp_IncType.SelectedValue = Index.ToString();
            }
        }

        private void LoadIncidentTypeToDrpList()
        {
            Drp_IncType.Items.Clear();
            string sql = "select tblincedenttype.Type,tblincedenttype.Id from tblincedenttype  where  tblincedenttype.IncidentType='NORMAL' ";
            MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_IncType.Items.Add(new ListItem("All", "0"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_IncType.Items.Add(li);
                }
            }
        }

        protected void Drp_IncType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadIncidents();
            Lnk_Select.Text = "Select All";
        }

        protected void Lnk_Select_Click(object sender, EventArgs e)
        {
            if (Lnk_Select.Text == "Select All")
            {
                foreach (GridViewRow gv in Grd_Incident.Rows)
                {
                    CheckBox cb = (CheckBox)gv.FindControl("Chk_Incident");
                    cb.Checked = true;
                }
                Lnk_Select.Text = "None";
            }
            else
            {
                foreach (GridViewRow gv in Grd_Incident.Rows)
                {
                    CheckBox cb = (CheckBox)gv.FindControl("Chk_Incident");
                    cb.Checked = false;
                }
                Lnk_Select.Text = "Select All";
            }
        }

        protected void Btn_Delete_Click(object sender, EventArgs e)
        {
            int flag = 0;
            foreach (GridViewRow gv in Grd_Incident.Rows)
            {
                CheckBox cb = (CheckBox)gv.FindControl("Chk_Incident");
                if (cb.Checked)
                {
                    flag = 1;
                }
            }
            if (flag == 1)
            {
                Lbl_Confirm.Text = "Are you sure you want to delete the Incident(s)";
                this.ModalPopupExtender_Confirm.Show();
            }
            else if (flag == 0)
            {
                WC_MessageBox.ShowMssage("Please check at least one incident");
            }
        }

        protected void Btn_Confirm_Click(object sender, EventArgs e)
        {
            string message;
            foreach (GridViewRow gv in Grd_Incident.Rows)
            {
                CheckBox cb = (CheckBox)gv.FindControl("Chk_Incident");
                if (cb.Checked)
                {
                    if (MyIncident.AlreadyDeleted(int.Parse(gv.Cells[1].Text), out message))
                    {
                        MyIncident.Delete(int.Parse(gv.Cells[1].Text.ToString()));
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Delete Incident", ""+gv.Cells[2].Text+" Deleted", 1);
                        WC_MessageBox.ShowMssage("Incident(s) are deleted");
                    }
                    else
                    {
                        WC_MessageBox.ShowMssage(message);
                    }
                }
            }
            FillGrid();
        }

        protected void Btn_DeletePopup_Click(object sender, EventArgs e)
        {
            string message;
            if (Txt_IncidentId.Text.Trim() != "")
            {
                if (MyIncident.AlreadyDeleted(int.Parse(Txt_IncidentId.Text), out message))
                {
                    MyIncident.Delete(int.Parse(Txt_IncidentId.Text.ToString()));
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Delete Incident", "" + Txt_Title.Text + " Deleted", 1);
                    WC_MessageBox.ShowMssage("Incident(s) are deleted");
                }
                else
                {
                    WC_MessageBox.ShowMssage(message);
                }
                FillGrid();
            }
            else
            {
                WC_MessageBox.ShowMssage("Try again");
            }
        }

        protected void Rdb_Batch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Rdb_Batch.SelectedValue == "0" || Rdb_Batch.SelectedValue == "2")
            {
                Drp_PreviousBatch.SelectedValue = MyUser.CurrentBatchId.ToString();
                Drp_PreviousBatch.Enabled = false;
            }
            else
            {
                Drp_PreviousBatch.Enabled = true;
            }
            LoadIncidents();
        }

        protected void Drp_PreviousBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadIncidents();
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
                    Grd_Incident.Columns[4].Visible = true;
                    Grd_Incident.DataSource = dataView;
                    Grd_Incident.DataBind();
                    FillDetails();
                    Grd_Incident.Columns[1].Visible = false;
                    Grd_Incident.Columns[4].Visible = false;
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
