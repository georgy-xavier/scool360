using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;
using System.Text;

namespace WinEr
{
    public partial class EmailGenaralTemplate : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private EmailManager Obj_Email;
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MyFeeMang = MyUser.GetFeeObj();
            Obj_Email = MyUser.GetEmailObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(878))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadInitials();
                }
                Lbl_Msg.Text = "";
                Lbl_TemplateSave.Text = "";
            }

        }

        private void LoadInitials()
        {
            LoadGeneralTemplates();
            Loadseperators();
            Btn_SaveTemplate.Visible = false;
            Button1.Visible = false;
            Pnl_Initial.Visible = true;
            Div_Seperatos.InnerHtml = "";
            Txt_EmailSubject.Text = "";
            Editor_Body.Content = "";
        }

        private void LoadGeneralTemplates()
        {
            string sql = "";
            ListItem li;
            Drp_Type.Items.Clear();
            DataSet GeneralTemplateDs = new DataSet();     
            GeneralTemplateDs = MyStudMang.GetGeneralEmailTemplate();
            if (GeneralTemplateDs != null && GeneralTemplateDs.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("select Type", "0");
                Drp_Type.Items.Add(li);
                foreach (DataRow dr in GeneralTemplateDs.Tables[0].Rows)
                {
                    li = new ListItem(dr["Type"].ToString(), dr["Id"].ToString());
                    Drp_Type.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("None", "-1");
                Drp_Type.Items.Add(li);
            }
        }

        protected void Drp_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet replaceMentDs = new DataSet();
            Drp_Replacements.Items.Clear();
            string sql = "";
            DataSet GeneralTemplateDs = new DataSet();
            ListItem li;
            if (int.Parse(Drp_Type.SelectedValue) > 0)
            {
                sql = "Select tbl_generalemailtemplate.Subject, tbl_generalemailtemplate.Body,Enabled from tbl_generalemailtemplate where tbl_generalemailtemplate.Id=" + int.Parse(Drp_Type.SelectedValue) + "";
                MyReader = Obj_Email.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Txt_EmailSubject.Text = MyReader.GetValue(0).ToString().Replace("&nbsp;", "");
                    Editor_Body.Content = MyReader.GetValue(1).ToString().Replace("&nbsp;", "");
                    if (int.Parse(MyReader.GetValue(2).ToString()) == 1)
                    {
                        Chk_Enable.Checked = true;
                    }
                    else
                    {
                        Chk_Enable.Checked = false;
                    }


                }
                Loadseperators();
                LoadReplacement();
            }
        }

        private void LoadReplacement()
        {
            Lbl_replacement.Text = "";
            if (Drp_Replacements.SelectedValue != "-1")
            {
                string sql = "";
                sql = "select tbl_generalemailseperators.Type from tbl_generalemailseperators where Id=" + int.Parse(Drp_Replacements.SelectedValue) + "";
                MyReader = Obj_Email.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Lbl_replacement.Text = "(" + MyReader.GetValue(0).ToString().Replace("&nbsp;", "") + ")";
                }

            }
        }

        private void Loadseperators()
        {
            DataSet replaceMentDs = new DataSet();
            Drp_Replacements.Items.Clear();
            string sql = "";
            DataSet GeneralTemplateDs = new DataSet();
            ListItem li;
            if (int.Parse(Drp_Type.SelectedValue) > 0)
            {
                sql = "SELECT tbl_generalemailseperators.`Id`,tbl_generalemailseperators.Seperators FROM tbl_generalemailseperators INNER JOIN tbl_generaltemplateseperatormap ON tbl_generaltemplateseperatormap.SeperatorId=tbl_generalemailseperators.Id WHERE tbl_generaltemplateseperatormap.TemplateId=" + int.Parse(Drp_Type.SelectedValue) + "";
                GeneralTemplateDs = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (GeneralTemplateDs != null && GeneralTemplateDs.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("select Type", "0");
                    Drp_Replacements.Items.Add(li);
                    foreach (DataRow dr in GeneralTemplateDs.Tables[0].Rows)
                    {
                        li = new ListItem(dr["Seperators"].ToString(), dr["Id"].ToString());
                        Drp_Replacements.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("None", "-1");
                    Drp_Replacements.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("None", "-1");
                Drp_Replacements.Items.Add(li);
            }

        }

        protected void Btn_Update_Click(object sender, EventArgs e)
        {
            if (int.Parse(Drp_Type.SelectedValue) != -1)
            {
                if (int.Parse(Drp_Type.SelectedValue) > 0)
                {
                    string emailSubject = Txt_EmailSubject.Text;
                    string emailBody = Editor_Body.Content.Replace("'", "").Replace("\\", "");
                    int enabeld = 0;
                    if (Chk_Enable.Checked == true)
                    {
                        enabeld = 1;
                    }
                    int Id = int.Parse(Drp_Type.SelectedValue);
                    Obj_Email.UpdateGeneralTemplate(emailSubject, emailBody, enabeld, Id);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Email", "Emails Templates updated", 1);
                    Lbl_Msg.Text = "Updated successfully";
                }
                else
                {
                    Lbl_Msg.Text = "Select any type";
                }
            }
        }

        protected void Drp_Replacements_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReplacement();
        }

        protected void Lnl_NewTemplate_Click(object sender, EventArgs e)
        {
            Pnl_Initial.Visible = false;
            Txt_EmailSubject.Text = "";
            Editor_Body.Content = "";
            string sql = "";
            Button1.Visible=true;
                Btn_SaveTemplate.Visible=true;
            StringBuilder StbSeperator = new StringBuilder();
            sql = "select tbl_generalemailseperators.Seperators, tbl_generalemailseperators.Type from tbl_generalemailseperators ";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {

                StbSeperator.Append("<table>");
                StbSeperator.Append("<tr>");
                StbSeperator.Append("<td>Keywords:&nbsp;&nbsp;</td>");
                while (MyReader.Read())
                {
                    if (MyReader.GetValue(1).ToString() == "Student Name")
                    {
                        StbSeperator.Append("<td>" + MyReader.GetValue(1).ToString() + ":" + MyReader.GetValue(0).ToString() + "");
                        StbSeperator.Append("</td>");
                    }
                    if (MyReader.GetValue(1).ToString() == "Parent Name")
                    {
                        StbSeperator.Append("<td>,&nbsp;" + MyReader.GetValue(1).ToString() + ":" + MyReader.GetValue(0).ToString() + "");
                        StbSeperator.Append("</td>");
                    }
                    if (MyReader.GetValue(1).ToString() == "Staff Name")
                    {
                        StbSeperator.Append("<td>,&nbsp;" + MyReader.GetValue(1).ToString() + ":" + MyReader.GetValue(0).ToString() + "");
                        StbSeperator.Append("</td>");
                    }
                }
                StbSeperator.Append("</tr>");
                StbSeperator.Append("</table>");                
            }
            Div_Seperatos.InnerHtml = StbSeperator.ToString();
           
        }

        protected void Btn_SaveTemplate_Click(object sender, EventArgs e)
        {
            string templatesubject = Txt_EmailSubject.Text;
            string templateBody = Editor_Body.Content.Replace("'", "").Replace("\\", "");
            try
            {
                Obj_Email.SaveNewTemplate(templatesubject, templateBody);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Email", "Emails template created", 1);
                Lbl_TemplateSave.Text = "Template created successfully";
                Txt_EmailSubject.Text = "";
                Editor_Body.Content = "";
            }
            catch (Exception ex)
            {
                Lbl_TemplateSave.Text = "Template creation failed,Please try again!";
            }
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            LoadInitials();
        }
    }
           
}
