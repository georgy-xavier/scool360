using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;
using WinBase;

namespace WinEr
{
    public partial class WebForm26 : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private Incident MyIncident;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet;
        //private int UserId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (Session["StudId"] == null)
            {
                Response.Redirect("SearchStudent.aspx");
            }    
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MyIncident = MyUser.GetIncedentObj();
            if (MyIncident == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(70))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    int studId = 0;
                    int.TryParse(Session["StudId"].ToString(), out studId);
                    IfInPromotionOrHistory(studId);

                  //  string _MenuStr;
                  //  _MenuStr = MyStudMang.GetSubStudentMangMenuString(MyUser.UserRoleId, int.Parse(Session["StudType"].ToString()));
                   // this.SubStudentMenu.InnerHtml = _MenuStr;
                    Pnl_incidentGrid.Visible = false;
                 //   LoadStudentTopData();
                    LoadIncidentTypeToDrpList();
                    setDrpIndex();
                    LoadIncidents();

                    LoadPreviousBatchesToDropDown();
                    Drp_PreviousBatch.Enabled = false;
                    Drp_PreviousBatch.SelectedValue = MyUser.CurrentBatchId.ToString();
                }
            }
        }
        private void IfInPromotionOrHistory(int studid)
        {
            int studtype = 1;
            if (Session["StudType"] != null)
            {
                studtype = int.Parse(Session["StudType"].ToString());
            }
            if (studtype > 1)
            {
                Rdb_Batch.SelectedValue = "2";
                Btn_Delete.Visible = false;
                Btn_DeletePopUp.Visible = false;
            }
           
        }

        private void LoadPreviousBatchesToDropDown()
        {
            Drp_PreviousBatch.Items.Clear();
            string sql = "select tblbatch.BatchName, tblbatch.Id from tblbatch where tblbatch.Id BETWEEN (select tblview_student.JoinBatch from tblview_student where tblview_student.Id="+int.Parse(Session["StudId"].ToString())+") and "+MyUser.CurrentBatchId+" order by tblbatch.Id desc";
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

        //private void LoadStudentTopData()
        //{

        //    string _Studstrip = MyStudMang.ToStripString(int.Parse(Session["StudId"].ToString()), "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StudId"].ToString()) + "&type=StudentImage", int.Parse(Session["StudType"].ToString()));
        //    this.StudentTopStrip.InnerHtml = _Studstrip;
        //}

        private void LoadIncidents()
        {
            Pnl_incidentGrid.Visible = true;
            Grd_Incident.PageIndex = 0;
            FillGrid();
        }

        private void FillGrid()
        {
            string sql="";
            Grd_Incident.Columns[1].Visible = true;
            Grd_Incident.Columns[4].Visible = true;

            if (Rdb_Batch.SelectedValue == "0")
            {
                sql = "select tblincedent.Id , tblincedent.Title , tblincedenttype.`Type` , (select MAX(tblview_user.SurName) from  tblview_user where tblview_user.Id= tblincedent.CreatedUserId)  as SurName, tblincedent.`Point`, DATE_FORMAT(tblincedent.IncedentDate,'%d/%m/%Y') as IncidentDate from tblincedent INNER join tblincedenttype on tblincedenttype.Id = tblincedent.TypeId inner join tblview_student on tblview_student.Id = tblincedent.AssoUser where tblincedent.`Status` = 'Approved' AND tblincedent.UserType='student'  and  tblincedenttype.IncidentType='NORMAL'   and tblview_student.Id = " + int.Parse(Session["StudId"].ToString()) + " and tblincedent.BatchId=" + MyUser.CurrentBatchId;

                if (Drp_IncType.SelectedValue != "0")
                {
                    sql = sql + " and tblincedenttype.Id = " + int.Parse(Drp_IncType.SelectedValue);
                }
                sql = sql + " order by tblincedent.CreatedDate";
            }
            else if (Rdb_Batch.SelectedValue == "1")
            {
                sql = "select tblview_incident.Id , tblview_incident.Title , tblincedenttype.`Type` , (select tblview_user.SurName from  tblview_user where tblview_user.Id= tblview_incident.CreatedUserId)  as SurName,tblview_incident.`Point`,DATE_FORMAT(tblview_incident.IncedentDate,'%d/%m/%Y') as IncidentDate from tblview_incident INNER join tblincedenttype on tblincedenttype.Id = tblview_incident.TypeId inner join tblview_student on tblview_student.Id = tblview_incident.AssoUser where tblview_incident.`Status` = 'Approved' AND tblview_incident.UserType='student'  and  tblincedenttype.IncidentType='NORMAL'  and tblview_student.Id =" + int.Parse(Session["StudId"].ToString()) + " and tblview_incident.BatchId=" + Drp_PreviousBatch.SelectedValue;

                if (Drp_IncType.SelectedValue != "0")
                {
                    sql = sql + " and tblincedenttype.Id = " + int.Parse(Drp_IncType.SelectedValue);
                }
                sql = sql + " order by tblview_incident.CreatedDate";
            }
            else
            {
                sql = "select tblview_incident.Id , tblview_incident.Title , tblincedenttype.`Type` ,(select tblview_user.SurName from  tblview_user where tblview_user.Id= tblview_incident.CreatedUserId)  as SurName,tblview_incident.`Point`,DATE_FORMAT(tblview_incident.IncedentDate,'%d/%m/%Y') as IncidentDate from tblview_incident INNER join tblincedenttype on tblincedenttype.Id = tblview_incident.TypeId inner join tblview_student on tblview_student.Id = tblview_incident.AssoUser where tblview_incident.`Status` = 'Approved' AND tblview_incident.UserType='student' and  tblincedenttype.IncidentType='NORMAL'   and tblview_student.Id =" + int.Parse(Session["StudId"].ToString());

                if (Drp_IncType.SelectedValue != "0")
                {
                    sql = sql + " and tblincedenttype.Id = " + int.Parse(Drp_IncType.SelectedValue);
                }
                sql = sql + " order by tblview_incident.CreatedDate";
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
                lbl_Points.Visible = false;
                lbl_TotalPoints.Visible = false;
                lbl_viewIncidentMsg.Text = "No Incidents found";

                Btn_Delete.Enabled = false;
                Lnk_Select.Visible = false;
                Img_Points.Visible = false;
            }
        }

        protected void Grd_Incident_SelectedIndexChanged(object sender, EventArgs e)
        {
            int IncidentId = int.Parse(Grd_Incident.SelectedRow.Cells[1].Text.ToString());
            Response.Redirect("ViewIncidence.aspx?id=" + IncidentId + "&Type=student");
          //  ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "openIncpopup('ViewIncidence.aspx?id=" + IncidentId + "&Type=student');", true);
            //AddDetailsTopopUp(IncidentId);
            //this.MPE_IncidentPopUp.Show();
        }

        private void AddDetailsTopopUp(int IncidentId)
        {
                
            DateTime _IncedentDate;
            DateTime _CreatedDate;
            string sql = "";
            sql = "select tblincedenttype.`Type`, tblview_user.SurName , tblview_incident.UserType, tblview_incident.CreatedDate , tblview_incident.IncedentDate , tblview_incident.Title , tblview_incident.Description , tblview_incident.AssoUser,tblclass.ClassName from tblview_incident inner join tblclass on tblclass.Id=tblview_incident.ClassId  inner join tblincedenttype on tblincedenttype.Id = tblview_incident.TypeId inner join tblview_user on tblview_user.Id = tblview_incident.CreatedUserId where tblview_incident.Id = " + IncidentId + " and  tblincedenttype.IncidentType='NORMAL' ";

            MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Txt_Type.Text = MyReader.GetValue(0).ToString();
                Txt_CreatedUser.Text = MyReader.GetValue(1).ToString();
                Txt_UserType.Text = MyReader.GetValue(2).ToString();
                _IncedentDate = DateTime.Parse(MyReader.GetValue(4).ToString());
                //_IncedentDate =MyUser.GetDareFromText(MyReader.GetValue(4).ToString());

                Txt_IncidentDate.Text = _IncedentDate.ToString("dd/MM/yyyy");
                _CreatedDate = DateTime.Parse(MyReader.GetValue(3).ToString());
                //_CreatedDate = MyUser.GetDareFromText(MyReader.GetValue(3).ToString());

                Txt_CreatedDate.Text = _CreatedDate.ToString("dd/MM/yyyy");
                Txt_Title.Text = MyReader.GetValue(5).ToString();
                Txt_Desc.Text = MyReader.GetValue(6).ToString();
                Txt_UserId.Text = MyReader.GetValue(7).ToString();
                Txt_Class.Text = MyReader.GetValue(8).ToString();
                Txt_IncidentId.Text = IncidentId.ToString();
                Lbl_Class.Visible = true;
                Txt_Class.Visible = true;
            }
            if ((Txt_UserType.Text.Trim() == "student") && (Txt_UserId.Text.Trim() != ""))
            {

                sql = "select tblview_student.StudentName from tblview_student WHERE tblview_student.Id = " + int.Parse(Txt_UserId.Text.Trim());
                MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Txt_ReportedTo.Text = MyReader.GetValue(0).ToString();
                    //Txt_Class.Text = MyReader.GetValue(1).ToString();
                }
            }
            else if ((Txt_UserType.Text.Trim().ToLowerInvariant() == "staff") && ((Txt_UserId.Text.Trim() != "")))
            {
                Lbl_Class.Visible = false;
                Txt_Class.Visible = false;
                sql = "select tblview_user.SurName from  tblview_user WHERE tblview_user.Id = " + int.Parse(Txt_UserId.Text.Trim()) + "";
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
                sql = " select tblclass.ClassName from tblclass inner join tblview_incident on tblview_incident.AssoUser=tblclass.Id WHERE tblview_incident.Id=" + IncidentId + "";

                MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Txt_ReportedTo.Text = MyReader.GetValue(0).ToString();
                    //Txt_Class.Text = MyReader.GetValue(0).ToString();
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
            DataSet MyDataSet = (DataSet)ViewState["Incidents"];
            int _Points = 0;
            if (MyDataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {
                    _Points = _Points + int.Parse(dr[4].ToString());

                }

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
          
            foreach (GridViewRow gv in Grd_Incident.Rows)
            {
               // Label Lbl_Pupil = (Label)gv.FindControl("Lbl_PupilName");
               // Label Lbl_PupilType = (Label)gv.FindControl("Lbl_PupilType");
                Label Lbl_Point = (Label)gv.FindControl("lbl_Point");
                Image Img_Point = (Image)gv.FindControl("Img_Point");
                Lbl_Point.Text = gv.Cells[4].Text.ToString();

               
                if (int.Parse(gv.Cells[4].Text.ToString()) >= 0)
                {
                    Img_Point.ImageUrl = "images/pt1_up.png";
                }
                else
                {
                    Img_Point.ImageUrl = "images/pt1_dwn.png";
                }

          
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
            if (TypeId !="")
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
            if (flag==1)
            {
            Lbl_Confirm.Text = "Are you sure you want to delete the Incident(s)";
            this.ModalPopupExtender_Confirm.Show();
            }
            else if (flag ==0)
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
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Delete Incident", "Incident about student " + MyUser.m_DbLog.GetStudentName(int.Parse(Session["StudId"].ToString())) + " has been deleted. Deleted incident was " + gv.Cells[2].Text.ToString(), 1);
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
