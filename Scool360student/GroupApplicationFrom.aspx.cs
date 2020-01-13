using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using System.Text;
using WinBase;
using System.Web.Services;
using Newtonsoft.Json;
using System.Collections;
using System.Web.Script.Services;
namespace Scool360student
{
    public partial class GroupApplicationFrom : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private ClassOrganiser MyClassMang;
        public MysqlClass m_MysqlDb;

        private OdbcDataReader MyReader = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(3036))
            {
                Response.Redirect("RoleErr.htm");
            }

            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            string studentID = MyUser.StudId.ToString();

            if (!IsPostBack)
            {
                loadGeneralDetails();

            }

        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public void loadGeneralDetails()
        {

            int studentID = int.Parse(MyUser.StudId.ToString());


            studentID = int.Parse(MyUser.StudId.ToString());
            string _sqlMarksColums = "select Name from tbl_groupapplicationform where Stud_Id=" + studentID + " and status = 2";
            string sql2 = "select Name from tbl_groupapplicationform where Stud_Id=" + studentID + " and status = 0";
            //DataSet ColumnDetailsss = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(_sqlMarksColums);
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(_sqlMarksColums);
            if (MyReader.HasRows)
            {
                PanelApproval.Visible = true;
                panel_application.Visible = false;
                pnl_grpapprvd.Visible = false;
            }

            else
            {
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql2);
                if(MyReader.HasRows)
                {
                    PanelApproval.Visible = false;
                    pnl_grpapprvd.Visible = true;
                    panel_application.Visible = false;
                }
                else
                {
                    PanelApproval.Visible = false;
                    pnl_grpapprvd.Visible = false;
                    panel_application.Visible = true;
                    string _sqlMarksColum = "select DISTINCT StudentName from tblstudent  where Id=" + studentID;
                    DataSet ColumnDetails = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(_sqlMarksColum);
                    // DataSet ColumnDetails = m_MysqlDb.ExecuteQueryReturnDataSet(_sqlMarksColum);
                    Txt_ExamName.Text = studentID.ToString();

                    TextBox1.Text = ColumnDetails.Tables[0].Rows[0][0].ToString();
                    TextBox1.DataBind();

                    string sqls = "Select tbl_gr_master.Id,tbl_gr_master.GroupName from tbl_gr_master";
                    DataSet ColumnDetailss = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sqls);
                    Drp_PeriodType.Items.Clear();
                    Drp_PeriodType.DataSource = ColumnDetailss;

                    Drp_PeriodType.DataValueField = "Id";
                    Drp_PeriodType.DataTextField = "GroupName";
                    Drp_PeriodType.DataBind();
                }
             
            }



        }
        public void loadDetails()
        {

            int studentID = int.Parse(MyUser.StudId.ToString());


            studentID = int.Parse(MyUser.StudId.ToString());

            PanelApproval.Visible = false;
            string _sqlMarksColum = "select DISTINCT StudentName from tblstudent  where Id=" + studentID;
            DataSet ColumnDetails = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(_sqlMarksColum);
            // DataSet ColumnDetails = m_MysqlDb.ExecuteQueryReturnDataSet(_sqlMarksColum);
            Txt_ExamName.Text = studentID.ToString();

            TextBox1.Text = ColumnDetails.Tables[0].Rows[0][0].ToString();
            TextBox1.DataBind();

            string sqls = "Select tbl_gr_master.Id,tbl_gr_master.GroupName from tbl_gr_master";
            DataSet ColumnDetailss = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sqls);
            Drp_PeriodType.Items.Clear();
            Drp_PeriodType.DataSource = ColumnDetailss;

            Drp_PeriodType.DataValueField = "Id";
            Drp_PeriodType.DataTextField = "GroupName";
            Drp_PeriodType.DataBind();




        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        protected void Btn_Create_Click(object sender, EventArgs e)
        {
            int studentID = int.Parse(MyUser.StudId.ToString());
            string sql = "";
            int GroupName = int.Parse(Drp_PeriodType.SelectedValue.ToString());
            string name = TextBox1.Text;
            string sql1 = "select id from tbl_groupapplicationform where Stud_Id = " + studentID + "";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql1);
            if (MyReader.HasRows)
            {
                sql = "update tbl_groupapplicationform SET Group_Id =  " + GroupName + ",Status = 2 where Stud_Id = " + studentID + "";
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            }
            else
            {
                sql = "INSERT INTO tbl_groupapplicationform(Stud_Id,Name,Group_Id,Status) VALUES (" + studentID + ",'" + name + "'," + GroupName + " ,2) ";
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                
            }

            WC_MessageBox.ShowMssage("Application waiting for approval");

            //loadGeneralDetails();
        }
        protected void Btn_CancelBill_Click(object sender, EventArgs e)
        {
            loadGeneralDetails();
        }
        protected void lnk_grpsubmit_Click(object sender, EventArgs e)
        {
            panel_application.Visible = true;
            PanelApproval.Visible = false;
            loadDetails();
        }

    }
}
