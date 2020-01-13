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

namespace WinEr
{
    public partial class CreateExamMaster : System.Web.UI.Page
    {
        private ExamManage MyExamMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyExamMang = MyUser.GetExamObj();
            if (MyExamMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(5))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    //some initlization
                    AddExamType();
                    AddPeriodType();
                    //AddSubjectToList();
                }
            }
        }

        private void AddExamType()
        {
            Drp_ExamType.Items.Clear();
            string sql = "SELECT Id, sbject_type FROM tblsubject_type";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_ExamType.Items.Add(li);
                }
                Drp_ExamType.SelectedIndex = 0;
            }
        }

        private void AddPeriodType()
        {
            Drp_PeriodType.Items.Clear();
            string sql = "SELECT Id, Name FROM tblfrequency where Name != 'Single Payment'";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_PeriodType.Items.Add(li);
                }
                Drp_PeriodType.SelectedIndex = 0;
            }
        }

        protected void Btn_Create_Click(object sender, EventArgs e)
        {
            string _msg = "";
            if (Txt_ExamName.Text.Trim() == "" || Drp_ExamType.SelectedItem.Text.Trim() == "" || Drp_PeriodType.SelectedItem.Text.Trim() == "")
            {
                _msg = "One or more fields are empty";
            }

            else if (MyExamMang.ExamExist(Txt_ExamName.Text, int.Parse(Drp_ExamType.SelectedValue.ToString()), int.Parse(Drp_PeriodType.SelectedValue.ToString())))
            {

                _msg = "Exam is already exist";
            }

            else
            {
                MyExamMang.CreateExamMaster(Txt_ExamName.Text.ToString(), int.Parse(Drp_PeriodType.SelectedValue), int.Parse(Drp_ExamType.SelectedValue), MyUser.UserName.ToString());
                _msg = "Exam Created..";
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create Exam", "A new Exam "+Txt_ExamName.Text+" is created", 1);
               
                ClearAll();
            }
            WC_MessageBox.ShowMssage(_msg);
            
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void ClearAll()
        {
            Txt_ExamName.Text = "";
        }
    }
}
