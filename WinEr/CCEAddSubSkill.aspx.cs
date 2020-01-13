using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;

namespace WinEr
{
    public partial class CCEAddSubSkill : System.Web.UI.Page
    {
        private ExamManage MyExamMang;
        private KnowinUser MyUser;
        private DataSet MydataSet;
        private SchoolClass objSchool = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyExamMang = MyUser.GetExamObj();
            if (MyExamMang == null)
            {
                Response.Redirect("Default.aspx");
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
                    //some initlization
                   
                    load_Grid();
                    Txt_Skillname.Focus();

                }
            }
        }

        /// <summary>
        /// existing skills show on the grid
        /// </summary>
        private void load_Grid()
        {
            try
            {
                Grd_CCE.DataSource = null;
                string sql = "SELECT tblcce_subjectskills.Id as Id,tblcce_subjectskills.SkillName as SkillName from tblcce_subjectskills";
                MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    Grd_CCE.Columns[0].Visible = true;
                    Grd_CCE.DataSource = MydataSet;
                    Grd_CCE.DataBind();
                    Grd_CCE.Columns[0].Visible = false;
                    GridTable.Visible = true;
                    Label1.Visible = false;
                }
                else
                {
                    GridTable.Visible = false;
                    Label1.Visible = true;
                    Label1.Text = "No Skills Found!";

                }
            }
            catch (Exception ex)
            {
                WC_MessageBox.ShowMssage(ex.Message);
            }
        }


        /// <summary>
        /// removing exsiting skills 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grd_CCE_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            CLogging logger = CLogging.GetLogObject();
            string _Err = "";
            int index = Convert.ToInt32(e.CommandArgument);
            try
            {
                if (e.CommandName == "Remove")
                {
                    string sql = "";
                    int Id = 0;
                    int.TryParse(Grd_CCE.Rows[index].Cells[0].Text, out Id);
                    if (Id != 0)
                    {
                        sql = "select * from tblcce_subjectskillmap where tblcce_subjectskillmap.SkillId="+Id;
                        DataSet ds=MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            sql = "delete from tblcce_subjectskills where Id=" + Id;
                            MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                            _Err = "Skill is  deleted sucessfully! ";
                            logger.LogToFile("Add subject skill", Txt_Skillname.Text + " removed", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, MyUser.LoginUserName);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Add subject skill", Txt_Skillname.Text + " removed", 1);

                        }
                        else
                        {
                            Label1.Visible = true;
                            Label1.Text = "Exam is already Scheduled for this skill.Can not Delete  ";
                        }
                                          
                    }
                }
            }
            catch (Exception ex)
            {
                _Err = " Skill is not deleted sucessfully! "+ ex.Message;
                logger.LogToFile("Add subject skill", "throws Error" + Grd_CCE.Rows[index].Cells[1].Text + _Err, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, MyUser.LoginUserName);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Add subject skill", "throws Error" + Grd_CCE.Rows[index].Cells[1].Text + _Err, 1);
            }
            WC_MessageBox.ShowMssage(_Err);
            load_Grid();

        }

        /// <summary>
        /// this event creating new subject skills
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_add_Click(object sender, EventArgs e)
        {
            Label1.Visible = false;
            string _Err = "";
            try
            {
                if (Txt_Skillname.Text.Trim() != "")
                {
                     Label1.Visible = true;
                     Label1.Text = Validate_And_Insert_Skill(Txt_Skillname.Text.Trim());
                }
                else
                {
                    Label1.Visible = true;
                    Label1.Text = "Enter the skill name.";
                }

            }
            catch(Exception ex)
            {
                _Err = "Skill is not created sucessfully! " + ex.Message;
                Txt_Skillname.Text = "";
                WC_MessageBox.ShowMssage(_Err);
            }
           
            load_Grid();
            Txt_Skillname.Focus();
        }

        /// <summary>
        /// while adding subject skill checking validation
        /// </summary>
        /// <param name="_SkillName"></param>
        /// <returns></returns>
        public string Validate_And_Insert_Skill(string _SkillName)
        {
            CLogging logger = CLogging.GetLogObject();
            string _Err = "";
            string sql = "select * from tblcce_subjectskills where SkillName='" + Txt_Skillname.Text.Trim() + "'";
            MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables.Count > 0)
            {
                if (MydataSet.Tables[0].Rows.Count > 0)
                    _Err = "Existing skill";
                else
                {
                    sql = "insert into tblcce_subjectskills(SkillName) values('" + Txt_Skillname.Text.Trim() + "')";
                    MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                    _Err = "Skill is created sucessfully";
                   
                    logger.LogToFile("Add subject skill", Txt_Skillname.Text + " added", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, MyUser.LoginUserName);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Add subject skill", Txt_Skillname.Text + " added", 1);
                   
                    Txt_Skillname.Text = "";
                }
            }
            return _Err;
        }

    }
}
