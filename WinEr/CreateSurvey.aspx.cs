using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;
using System.Globalization;

namespace WinEr
{
    public partial class CreateSurvey : System.Web.UI.Page
    {

        private KnowinUser MyUser;
        private GroupManager MyGroup;       
        public MysqlClass m_MysqlDb;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyGroup = MyUser.GetGroupManagerObj();
            if (MyGroup == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(3039))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    Txt_Surveyname.Focus();
                    LoadSurveyCreated();
                }
            }           
        }

        public void LoadSurveyCreated()
        {
            DataSet GroupDs = new DataSet();
            try
            {
                GroupDs = MyGroup.Get_SurveyCreation();
                Lbl_err.Text = string.Empty;
                if (GroupDs != null && GroupDs.Tables[0].Rows.Count > 0)
                {
                    Grd_SurveyCreated.Columns[0].Visible = true;
                    Grd_SurveyCreated.DataSource = GroupDs;
                    Grd_SurveyCreated.DataBind();
                    Grd_SurveyCreated.Columns[0].Visible = false;
                }
                else
                {
                    Lbl_err.Visible = true;
                    Lbl_err.Text = "No Survey Found";
                    Grd_SurveyCreated.DataSource = null;
                    Grd_SurveyCreated.DataBind();
                 
                }
            }
            catch (Exception ex)
            {
                WC_MessageBox.ShowMssage("Error,Please try later");
            }
        }

        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            string surveyname = Txt_Surveyname.Text;
            string lastdate = Txt_SurveyDate.Text;
            
            if (validate())
            {
                string today = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime lastD = DateTime.ParseExact(lastdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime todayD = DateTime.ParseExact(today, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                int result = DateTime.Compare(lastD,todayD);
                if (result < 0)
                {
                    WC_MessageBox.ShowMssage("Error, Invalid Date !!");
                    return;
                }
                try
                {
                    MyGroup.Create_Survey(surveyname,lastdate);
                    LoadSurveyCreated();
                    WC_MessageBox.ShowMssage("Survey name created successfully!");
                    clear_fields();
                }
                catch (Exception)
                {
                    WC_MessageBox.ShowMssage("Error, Please try later");
                }
            }
        }

        public bool validate()
        {
          if (String.IsNullOrEmpty(Txt_Surveyname.Text) || Txt_Surveyname.Text.Trim().Length == 0)
            {
                WC_MessageBox.ShowMssage("Please enter the survey name");
                return false;
            }
            else if (String.IsNullOrEmpty(Txt_SurveyDate.Text) || Txt_SurveyDate.Text.Trim().Length == 0)
            {
                WC_MessageBox.ShowMssage("Please enter the last date for the survey");
                return false;
            }
            return true;
        }

        public void clear_fields()
        {
            Txt_Surveyname.Text = string.Empty;
            Txt_SurveyDate.Text = string.Empty;
        }

    protected void Grd_SurveyCreated_SelectedIndexChanged(object sender, EventArgs e)
        {
            Btn_Add.Visible = false;
            Btn_Update.Visible = true;
            Grd_SurveyCreated.Columns[0].Visible = true;
            Txt_Surveyname.Text = Grd_SurveyCreated.SelectedRow.Cells[1].Text.Replace("&nbsp;", "");           
            Txt_SurveyDate.Text = Grd_SurveyCreated.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");
            SurveyId.Value = Grd_SurveyCreated.SelectedRow.Cells[0].Text;
            Grd_SurveyCreated.Columns[0].Visible = false;
        }

        protected void Btn_Update_Click(object sender, EventArgs e)
        {
            string sql = "";
            string surveyname = Txt_Surveyname.Text;
            string lastdate = Txt_SurveyDate.Text;
            if (validate())
            {
                try
                {
                    if (!String.IsNullOrEmpty(Txt_Surveyname.Text) || Txt_Surveyname.Text.Trim().Length == 0)
                    {
                        if (!MyGroup.SurveynameExist(int.Parse(SurveyId.Value)))
                        {
                            sql = "Update tbl_surveycreation set Survey_name='" + Txt_Surveyname.Text + "',Last_date='" + Txt_SurveyDate.Text + "' where Id=" + SurveyId.Value + "";
                            MyGroup.m_MysqlDb.ExecuteQuery(sql);
                            LoadSurveyCreated();
                            SurveyId.Value = "";
                            Btn_Add.Visible = true;
                            Btn_Update.Visible = false;
                            Txt_Surveyname.Text = "";
                            WC_MessageBox.ShowMssage("Survey name updated successfully");
                            clear_fields();
                        }
                        else
                        {
                            WC_MessageBox.ShowMssage("Cannot update, Survey name is already mapped to Group!");
                            clear_fields();
                        }
                    }
                }

                catch (Exception)
                {
                    WC_MessageBox.ShowMssage("Cannot delete, Please try later!");
                }
            }
        }

        protected void Grd_SurveyCreated_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_SurveyCreated.PageIndex = e.NewPageIndex;
            LoadSurveyCreated();
        }

        protected void Grd_SurveyCreated_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int Id = 0;
            try
            {
                int.TryParse(Grd_SurveyCreated.Rows[e.RowIndex].Cells[0].Text, out Id);
                if (!MyGroup.UserMappedToSurvey(Id))
                {
                    MyGroup.DeleteSurvey(Id);
                    LoadSurveyCreated();                   
                    WC_MessageBox.ShowMssage("Deleted successfully!");
                    Txt_Surveyname.Text = string.Empty;
                    Txt_SurveyDate.Text = string.Empty;                    
                    Btn_Add.Visible = true;
                    Btn_Update.Visible = false;
                }
                else
                {
                    WC_MessageBox.ShowMssage("Can't delete, Some users mapped to this survey!");
                }
            }
            catch
            {
                WC_MessageBox.ShowMssage("Can't delete, Please try again later!");
            }
        }        
    }
}