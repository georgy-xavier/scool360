using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WinEr
{
    public partial class StudSurveyanswer : System.Web.UI.Page
    {
        protected global::WinEr.MsgBoxControl WC_MessageBox;
        private ClassOrganiser My_Class;
        private KnowinUser MyUser;
        public MysqlClass m_MysqlDb;
        public MysqlClass m_TransationDb = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyUser = (KnowinUser)Session["UserObj"];
            if (!MyUser.HaveActionRignt(3044))
            {
                Response.Redirect("RoleErr.htm");
            }
            My_Class = MyUser.GetClassObj();
            if (My_Class == null)
            {
                Response.Redirect("Default.aspx");

            }
            else
            {
                if (!IsPostBack)
                {

                    filldropdown();
                }

                }
            }
           
        public void fillthegridview()
        {
           // string sql = "select studentid AS ID,studentname AS StudentName,survey_ques AS Question,Survey_answer AS ANSWER from tblSurveyAnswer";

            string sql = "select distinct a.studentid AS ID,b.Group_name AS GroupName,a.studentname AS StudentName,a.survey_ques AS Question,a.Survey_answer AS ANSWER from tblSurveyAnswer a join tbl_survey b where b.Survey_id = a.survey and b.Group_name='" + DropDownList1.SelectedValue+"'";

            DataSet ds_class = new DataSet();
            ds_class = My_Class.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            Grd_StuSurvey.DataSource = ds_class;
            Grd_StuSurvey.DataBind();
        }


        public void filldropdown()
        {
            DataSet ds_class = new DataSet();
            string sql = "select distinct b.Group_name from tblSurveyAnswer a join tbl_survey b where b.Survey_id = a.survey";
            ds_class = My_Class.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds_class != null && ds_class.Tables[0] != null && ds_class.Tables[0].Rows.Count > 0)
            {
                DropDownList1.Items.Clear();
                DropDownList1.DataSource = ds_class;

               // DropDownList1.DataValueField = "studentid";
                DropDownList1.DataTextField = "Group_name";


                DropDownList1.DataBind();
                DropDownList1.Items.Insert(0, new ListItem("Select Group", ""));
               



            }
            else
            {
                ListItem li = new ListItem("No Survey Exist", "-1");
                DropDownList1.Items.Add(li);
            }

        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillthegridview();
        }
    }
}