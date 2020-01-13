using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using WinBase;

namespace WinEr
{
    public partial class EmailConfiguration : System.Web.UI.Page
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
            if (!MyUser.HaveActionRignt(876))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadTypeDropDown();
                    LoadSeperators();
                }
                  Lbl_Msg.Text = "";
            }
        }
        protected void Btn_Update_Click(object sender, EventArgs e)
        {
            string _time = "";
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
                    _time = Drp_Schedulehour.SelectedItem.Text + ":" + Drp_ScheduleMinute.SelectedItem.Text + ":00";
                    int Id = int.Parse(Drp_Type.SelectedValue);
                    Obj_Email.UpdateEmailConfig(emailSubject, emailBody, enabeld, Id, Chk_ScheduleTime.Checked, _time);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Email", "Email configurations updated", 1);
                    Lbl_Msg.Text = "Updated successfully";
                }
                else
                {
                    Lbl_Msg.Text = "Select any type";
                }
            }

        }

   

        protected void Drp_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "";
            int SMSEnabled = 0, ScheduledEnabled = 0, ScheduedTime = 0;
            TimeSpan ts;
            if (int.Parse(Drp_Type.SelectedValue) > 0)
            {
                sql = "Select tbl_emailoptionconfig.Subject, tbl_emailoptionconfig.Body,Enabled,ScheduledTime,ScheduledTimeEnabled from tbl_emailoptionconfig where tbl_emailoptionconfig.Id=" + int.Parse(Drp_Type.SelectedValue) + "";
                MyReader = Obj_Email.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Txt_EmailSubject.Text = MyReader.GetValue(0).ToString().Replace("&nbsp;","");
                    Editor_Body.Content = MyReader.GetValue(1).ToString().Replace("&nbsp;","");
                    int.TryParse(MyReader.GetValue(4).ToString(), out ScheduledEnabled);
                    TimeSpan.TryParse(MyReader.GetValue(3).ToString(), out ts);
                    Drp_Schedulehour.SelectedValue = ts.Hours.ToString();
                    Drp_ScheduleMinute.SelectedValue = ts.Minutes.ToString();
                    if (int.Parse(MyReader.GetValue(2).ToString()) == 1)
                    {
                        Chk_Enable.Checked = true;
                    }
                    else
                    {
                        Chk_Enable.Checked = false;
                    }

                    if (ScheduledEnabled == 1)
                    {
                        Chk_ScheduleTime.Checked = true;
                        Drp_Schedulehour.Enabled = true;
                        Drp_ScheduleMinute.Enabled = true;
                    }
                    else
                    {
                        Chk_ScheduleTime.Checked = false;
                        Drp_Schedulehour.Enabled = false;
                        Drp_ScheduleMinute.Enabled = false;
                    }


                }
                LoadSeperators();
                LoadReplacement();

                
            }
            else
            {
                Txt_EmailSubject.Text = "";
                Editor_Body.Content = "";
                
                    Chk_Enable.Checked = false;
            }
        }

        private void LoadSeperators()
        {
            ListItem li;
            OdbcDataReader SeperatorReader = null;
            Drp_Replacements.Items.Clear();
            string sql = "";
            if (int.Parse(Drp_Type.SelectedValue) > 0)
            {
                sql = "SELECT tblsmsseperators.`Id`,tblsmsseperators.Seperator FROM tblsmsseperators INNER JOIN tbl_emailseperatormap ON tbl_emailseperatormap.SeperatorId=tblsmsseperators.Id WHERE tbl_emailseperatormap.TypeId=" + int.Parse(Drp_Type.SelectedValue) + "";
                SeperatorReader = Obj_Email.m_MysqlDb.ExecuteQuery(sql);
                if (SeperatorReader.HasRows)
                {
                    while (SeperatorReader.Read())
                    {
                        li = new ListItem(SeperatorReader.GetValue(1).ToString(), SeperatorReader.GetValue(0).ToString());
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

        private void LoadTypeDropDown()
        {
            DataSet TypeDs = new DataSet();
            ListItem li;
            Drp_Type.Items.Clear();
            TypeDs = Obj_Email.GetAllEmailConfigType();
            if (TypeDs != null && TypeDs.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("Select Type", "0");
                Drp_Type.Items.Add(li);
                foreach (DataRow dr in TypeDs.Tables[0].Rows)
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

        protected void Drp_Replacements_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReplacement();
        }

        private void LoadReplacement()
        {
            Lbl_replacement.Text = "";
            if (Drp_Replacements.SelectedValue != "-1" && Drp_Replacements.SelectedValue != "")
            {
                string sql = "";
                sql = "select tblsmsseperators.Type from tblsmsseperators where Id=" + int.Parse(Drp_Replacements.SelectedValue) + "";
                MyReader = Obj_Email.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Lbl_replacement.Text = "(" + MyReader.GetValue(0).ToString().Replace("&nbsp;", "") + ")";
                }

            }
        }


        protected void Chk_ScheduleTime_CheckedChanged(object sender, EventArgs e)
        {
            Drp_Schedulehour.SelectedValue = "0";
            Drp_ScheduleMinute.SelectedValue = "0";

            if (!Chk_ScheduleTime.Checked)
            {
                Drp_Schedulehour.Enabled = false;
                Drp_ScheduleMinute.Enabled = false;


            }
            else
            {
                Drp_Schedulehour.Enabled = true;
                Drp_ScheduleMinute.Enabled = true;
            }
        }
       
    }
}
