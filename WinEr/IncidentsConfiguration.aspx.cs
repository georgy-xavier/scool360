using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;

namespace WinEr
{
    public partial class IncidentsConfiguration : System.Web.UI.Page
    {
        private Incident MyIncedent;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            
            MyUser = (KnowinUser)Session["UserObj"];
            MyIncedent = MyUser.GetIncedentObj();
            if (MyIncedent == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadIncidentTitleArea();
                    LoadAutoConfigurationArea();
                    LoadIncidentTypesToDropDown();
                }
            }
        }

        #region INCIDENT TITLE FUNCTIONS

        private void LoadIncidentTypesToDropDown()
        {
            OdbcDataReader MyReader1 = null;
            Drp_IncidentType.Items.Clear();

            string sql = "SELECT tblincedenttype.`Type`, tblincedenttype.Id from tblincedenttype where tblincedenttype.Visibility='1' order by tblincedenttype.`Order` ";
            MyReader1 = MyIncedent.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader1.HasRows)
            {
                Btn_SaveTitle.Enabled = true;
                while (MyReader1.Read())
                {
                    ListItem li = new ListItem(MyReader1.GetValue(0).ToString(), MyReader1.GetValue(1).ToString());
                    Drp_IncidentType.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Types Found", "-1");
                Drp_IncidentType.Items.Add(li);
                Btn_SaveTitle.Enabled = false;
            }
        }

        private void LoadIncidentTitleArea()
        {
            LoadTitlesToGrid();
        }

        private void LoadTitlesToGrid()
        {
            string sql = "select tblincedenthead.Id, tblincedenthead.Title, tblincedenttype.`Type`, tblincedenthead.UserType, tblincedenthead.`Point`, tblincedenthead.`Mode`, tblincedenthead.NeedApproval from tblincedenthead inner join tblincedenttype on tblincedenttype.Id= tblincedenthead.TypeId where tblincedenthead.IsActive=1 and    tblincedenttype.IncidentType='NORMAL' ";
            MydataSet = MyIncedent.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                ViewState["TitleList"] = MydataSet;
                Grd_IncidentTitles.Columns[0].Visible = true;
                Grd_IncidentTitles.DataSource = MydataSet;
                Grd_IncidentTitles.DataBind();
                Grd_IncidentTitles.Columns[0].Visible = false;
                Grd_IncidentTitles.PageIndex = 0;
                Lbl_note.Text = "";
            }
            else
            {
                Grd_IncidentTitles.DataSource = null;
                Grd_IncidentTitles.DataBind();
                Lbl_note.Text = "No Titles Found.";

            }
        }

        protected void Img_AddNewTitle_Click(object sender, ImageClickEventArgs e)
        {
            TitleFieldsClear();
            MPE_AddTitle.Show();
        }

        protected void Lnk_AddTitle_Click(object sender, EventArgs e)
        {
            TitleFieldsClear();
            MPE_AddTitle.Show();
        }

        private void TitleFieldsClear()
        {
            Txt_TitleName.Text = "";
            txt_Point.Text = "";
            Rdb_For.SelectedValue = "0";
            Rdb_Mode.SelectedValue = "0";
            Drp_IncidentType.SelectedIndex = 0;
            lbl_TitleError.Text = "";
            txt_Point.ReadOnly = false;
        }

        protected void Rdb_Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Rdb_Mode.SelectedValue == "0")
            {
                txt_Point.ReadOnly = false;
                txt_Point.Text = "";
                MPE_AddTitle.Show();
                lbl_TitleError.Text = "";
            }
            else
            {
                txt_Point.ReadOnly = true;
                txt_Point.Text = "0";
                MPE_AddTitle.Show();
                lbl_TitleError.Text = "";
            }
        }

        protected void Btn_SaveTitle_Click(object sender, EventArgs e)
        {
            try
            {
                int _Point = 0;
                _Point = int.Parse(txt_Point.Text);
                string _NeedApproval = "NO";
                if (Chk_NeedApproval.Checked == true)
                {
                    _NeedApproval = "YES";
                }

                string sql1 = "select tblincedenthead.Id from tblincedenthead where tblincedenthead.Title='" + Txt_TitleName.Text.Trim() + "' and tblincedenthead.IsActive=1";
                MyReader = MyIncedent.m_MysqlDb.ExecuteQuery(sql1);
                if (MyReader.HasRows)
                {
                    lbl_TitleError.Text = "Incident Title already exists..";
                    MPE_AddTitle.Show();
                }
                else
                {
                    if (_Point == 0 || _Point < 0 && _Point >= -100 || _Point > 0 && _Point <= 100)
                    {
                        string sql = "insert into tblincedenthead(TypeId,Title,`Point`,UserType,`Mode`,NeedApproval) values(" + Drp_IncidentType.SelectedValue + ",'" + Txt_TitleName.Text + "'," + _Point + ",'" + Rdb_For.SelectedItem.Text + "','" + Rdb_Mode.SelectedItem.Text + "','" + _NeedApproval + "')";
                        MyIncedent.m_MysqlDb.ExecuteQuery(sql);

                        LoadIncidentTitleArea();
                    }
                    else
                    {
                        lbl_TitleError.Text = "Point should be in the range -100 to 100";
                        MPE_AddTitle.Show();
                    }
                }
            }
            catch
            {
                lbl_TitleError.Text = "Enter valid data";
                MPE_AddTitle.Show();
            }
        }

        protected void Grd_IncidentTitles_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _TitleId = int.Parse(Grd_IncidentTitles.SelectedRow.Cells[0].Text);

            string sql = "SELECT tblview_incident.Id from tblview_incident where tblview_incident.HeadId=" + _TitleId;
            MyReader = MyIncedent.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                sql = "UPDATE tblincedenthead set tblincedenthead.IsActive=0 where tblincedenthead.Id=" + _TitleId;
            }
            else
            {
                sql = "DELETE from tblincedenthead where tblincedenthead.Id=" + _TitleId;
            }
            MyIncedent.m_MysqlDb.ExecuteQuery(sql);

            sql = "UPDATE tblincedentconfig set TitleId=0, IncedentDesc='', Point_Value=0, IsEnable='NO' where tblincedentconfig.TitleId=" + _TitleId;
            MyIncedent.m_MysqlDb.ExecuteQuery(sql);

            LoadIncidentTitleArea();
            LoadAutoConfigurationArea();
        }

        protected void Grd_IncidentTitles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ImgDelete = (ImageButton)e.Row.FindControl("Img_ButtonDelete");
                //LinkButton _LinkButton1 = (LinkButton)e.Row.FindControl("LinkButton1");
                ImgDelete.Attributes.Add("onclick", "javascript:return " +
                     "confirm('Are you sure, do you want to delete " +
                     DataBinder.Eval(e.Row.DataItem, "Title") + "  Permanently')");
            }
        }

        protected void Grd_IncidentTitles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_IncidentTitles.PageIndex = e.NewPageIndex;
            DataSet _TitleDataSe = (DataSet)ViewState["TitleList"];

            Grd_IncidentTitles.Columns[0].Visible = true;
            Grd_IncidentTitles.DataSource = _TitleDataSe;
            Grd_IncidentTitles.DataBind();
            Grd_IncidentTitles.Columns[0].Visible = false;
        }

        protected void Grd_IncidentTitles_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["TitleList"] != null)
            {
                DataSet _sortlDS = (DataSet)ViewState["TitleList"];

                if (_sortlDS.Tables[0].Rows.Count > 0)
                {
                    DataTable dtData = _sortlDS.Tables[0];

                    DataView dataView = new DataView(dtData);

                    dataView.Sort = e.SortExpression + " " + GetSortDirection1(e.SortExpression);

                    Grd_IncidentTitles.Columns[0].Visible = true;
                    Grd_IncidentTitles.DataSource = dataView;
                    Grd_IncidentTitles.DataBind();
                    Grd_IncidentTitles.Columns[0].Visible = false;
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

        #endregion
   
        
        #region AUTO CONFIGURATION FUNCTIONS

        private void LoadAutoConfigurationArea()
        {
            LoadModules();
            LoadAutoConfigGrid();
        }

        private void LoadAutoConfigGrid()
        {
            string sql = "select tblincedentconfig.Id, tblmodule.ModuleName , tblincedentconfig.Description ,'NONE' AS `Title`,'NONE' AS `IncedentDesc` , tblincedentconfig.Point_Value ,tblincedentconfig.IsEnable from tblincedentconfig inner join tblmodule on tblmodule.Id= tblincedentconfig.ModuleId AND tblmodule.IsActive=1 where tblincedentconfig.TitleId=0 AND tblincedentconfig.ModuleId=" + Drp_Module.SelectedValue + " AND tblincedentconfig.Actor='" + Rdb_IncUserType.SelectedValue + "' Union  select tblincedentconfig.Id, tblmodule.ModuleName , tblincedentconfig.Description ,tblincedenthead.Title , tblincedentconfig.IncedentDesc , tblincedentconfig.Point_Value ,tblincedentconfig.IsEnable from tblincedentconfig inner join tblmodule on tblmodule.Id= tblincedentconfig.ModuleId AND tblmodule.IsActive=1 inner join tblincedenthead on tblincedenthead.Id = tblincedentconfig.TitleId   where tblincedentconfig.TitleId<>0 AND tblincedentconfig.ModuleId=" + Drp_Module.SelectedValue + " AND tblincedentconfig.Actor='" + Rdb_IncUserType.SelectedValue + "'";
            MydataSet = MyIncedent.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                ViewState["ConfigList"] = MydataSet;
                Grd_AutoIncConfig.Columns[0].Visible = true;
                Grd_AutoIncConfig.DataSource = MydataSet;
                Grd_AutoIncConfig.DataBind();
                Grd_AutoIncConfig.Columns[0].Visible = false;
                Grd_AutoIncConfig.PageIndex = 0;
                Lbl_IncConfigurationNote.Text = "";
            }
            else
            {
                Grd_AutoIncConfig.DataSource = null;
                Grd_AutoIncConfig.DataBind();
                Lbl_IncConfigurationNote.Text = "No Configuration Found.";

            }
        }

        private void LoadModules()
        {
            Drp_Module.Items.Clear();
            ListItem li;
            string sql = "select DISTINCT tblmodule.Id, tblmodule.ModuleName  from tblmodule inner join tblincedentconfig on tblincedentconfig.ModuleId= tblmodule.Id  where tblmodule.IsActive=1 ";

            MyReader = MyIncedent.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Module.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No Modules Found", "-1");
                Drp_Module.Items.Add(li);
            }
            Drp_Module.SelectedIndex = 0;
        }

        protected void Rdb_IncUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAutoConfigGrid();
        }

        protected void Drp_Module_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAutoConfigGrid();
        }

        protected void Grd_AutoIncConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadIncidentTitleToDropDown();
            LoadConfigurationFieldsWithAlreadyFilledValues();
            MPE_Configuration.Show();
        }

        private void LoadConfigurationFieldsWithAlreadyFilledValues()
        {
            lbl_ConfigError.Text = "";
            this.Div_Module.InnerText = Grd_AutoIncConfig.SelectedRow.Cells[1].Text;
            this.Div_Description.InnerHtml = Grd_AutoIncConfig.SelectedRow.Cells[2].Text;
            if (Grd_AutoIncConfig.SelectedRow.Cells[4].Text.Trim() == "NONE" || Grd_AutoIncConfig.SelectedRow.Cells[4].Text.Trim() == "")
            {
                txt_IncidentDescription.Text = "";
            }
            else
            {
                txt_IncidentDescription.Text = Grd_AutoIncConfig.SelectedRow.Cells[4].Text.Trim();
            }

            this.IncidentReplacements.InnerHtml = LoadIncedentReplacements(Grd_AutoIncConfig.SelectedRow.Cells[0].Text);

            txt_ConfigPoint.Text = Grd_AutoIncConfig.SelectedRow.Cells[5].Text;
            if (Grd_AutoIncConfig.SelectedRow.Cells[6].Text == "YES")
            {
                Chk_Enable.Checked = true;
            }
            else
            {
                Chk_Enable.Checked = false;
            }

            for (int i = 0; i < Drp_Title.Items.Count; i++)
            {
                if (Drp_Title.Items[i].Text == Grd_AutoIncConfig.SelectedRow.Cells[3].Text)
                {
                    Drp_Title.SelectedIndex = i;
                    break;
                }
            }
        }

        private string LoadIncedentReplacements(string IncedentConfigId)
        {
            string innerHtml = "<table cellspacing=\"10\"> ";
            string Tempstr = "";

            string sql = "SELECT tblincedentreplacement.Name FROM tblincedentreplacement INNER JOIN tblincedentreplacementmap ON tblincedentreplacementmap.ReplacementId=tblincedentreplacement.Id WHERE tblincedentreplacementmap.IncedentId=" + IncedentConfigId;
            MyReader = MyIncedent.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    Tempstr = Tempstr + "<tr>  <td  class=\"ReplacementStyle\" > <input type=\"text\" value=\"" + MyReader.GetValue(0).ToString() + "\" style=\"width:100%;\"/>   </td> </tr> ";
                }
            }
            else
            {
                Tempstr = "<tr>  <td  class=\"ReplacementStyle\" > No Replacement Found </td> </tr>";

            }
            innerHtml=innerHtml+Tempstr+"</table>";
            return innerHtml;
        }

        private void LoadIncidentTitleToDropDown()
        {
            OdbcDataReader MyReader1 = null;
            Drp_Title.Items.Clear();

            string sql = "select tblincedenthead.Title, tblincedenthead.Id from tblincedenthead where tblincedenthead.`Mode`='Automatic' and tblincedenthead.UserType='" + Rdb_IncUserType.SelectedValue + "' and tblincedenthead.IsActive=1 order by tblincedenthead.Id";
            MyReader1 = MyIncedent.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader1.HasRows)
            {
                Drp_Title.Items.Add(new ListItem("NONE", "0"));
                while (MyReader1.Read())
                {
                    ListItem li = new ListItem(MyReader1.GetValue(0).ToString(), MyReader1.GetValue(1).ToString());
                    Drp_Title.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("NONE", "0");
                Drp_Title.Items.Add(li);
                //Btn_ConfigSave.Enabled = false;
            }
        }

        protected void Grd_AutoIncConfig_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["ConfigList"] != null)
            {
                DataSet _sortlDS = (DataSet)ViewState["ConfigList"];

                if (_sortlDS.Tables[0].Rows.Count > 0)
                {
                    DataTable dtData = _sortlDS.Tables[0];

                    DataView dataView = new DataView(dtData);

                    dataView.Sort = e.SortExpression + " " + GetSortDirection1(e.SortExpression);

                    Grd_AutoIncConfig.Columns[0].Visible = true;
                    Grd_AutoIncConfig.DataSource = dataView;
                    Grd_AutoIncConfig.DataBind();
                    Grd_AutoIncConfig.Columns[0].Visible = false;
                }
            }
        }

        protected void Grd_AutoIncConfig_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_AutoIncConfig.PageIndex = e.NewPageIndex;
            DataSet _TitleDataSe = (DataSet)ViewState["ConfigList"];

            Grd_AutoIncConfig.Columns[0].Visible = true;
            Grd_AutoIncConfig.DataSource = _TitleDataSe;
            Grd_AutoIncConfig.DataBind();
            Grd_AutoIncConfig.Columns[0].Visible = false;
        }

        protected void Btn_ConfigSave_Click(object sender, EventArgs e)
        {
            try
            {
                double _Point = 0;
                int _ConfigId = int.Parse(Grd_AutoIncConfig.SelectedRow.Cells[0].Text.Trim());
                _Point = double.Parse(txt_ConfigPoint.Text);
                string _IsEnable = "NO";
                if (Chk_Enable.Checked == true)
                {
                    _IsEnable = "YES";
                }

                if (_Point == 0 || _Point < 0 && _Point >= -100 || _Point > 0 && _Point <= 100)
                {
                    string sql = "UPDATE tblincedentconfig set TitleId=" + Drp_Title.SelectedValue + ", IncedentDesc='" + txt_IncidentDescription.Text.Trim() + "', Point_Value=" + _Point + ",IsEnable='" + _IsEnable + "' where tblincedentconfig.Id=" + _ConfigId;
                    MyIncedent.m_MysqlDb.ExecuteQuery(sql);

                    LoadAutoConfigGrid();
                }
                else
                {
                    lbl_ConfigError.Text = "Point should be in the range -100 to 100";
                    MPE_Configuration.Show();
                }
            }
            catch
            {
                lbl_ConfigError.Text = "Enter valid data";
                MPE_Configuration.Show();
            }
        }



        #endregion



        
    }
}
