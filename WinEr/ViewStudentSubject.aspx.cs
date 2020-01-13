using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WinEr
{
    public partial class ViewStudentSubject : System.Web.UI.Page
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
            My_Class = MyUser.GetClassObj();
            if (My_Class == null)
            {
                Response.Redirect("Default.aspx");

            }
            else
            {
                if (!IsPostBack)
                {
                   
                    AddStandardToDrpList();

                }
            }
        }



        public void AddStandardToDrpList()
        {
            DataSet ds_class = new DataSet();
            string sql = "select Id,ClassName from tblclass where Status=1";
            ds_class = My_Class.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds_class != null && ds_class.Tables[0] != null && ds_class.Tables[0].Rows.Count > 0)
            {
                Drp_Standard.Items.Clear();
                Drp_Standard.DataSource = ds_class;
                Drp_Standard.DataValueField = "Id";
                Drp_Standard.DataTextField = "ClassName";
                Drp_Standard.DataBind();
                Drp_Standard.Items.Insert(0, new ListItem("Select Class", ""));
            }
            else
            {
                ListItem li = new ListItem("No Class Exist", "-1");
                Drp_Standard.Items.Add(li);
            }
        }
            public void fillthegridview(string classnames)
            {
                string sql = "select tblclass.id,tblsubjects.Id,tblstudent.Id,tblclass.ClassName,tblstudent.StudentName,tblsubjects.subject_name from tblclass join tblstudent_indiviualsubject join tblstudent join tblsubjects where  tblstudent_indiviualsubject.ClassName=tblclass.id and  tblclass.id=tblstudent_indiviualsubject.ClassName and tblstudent_indiviualsubject.StudentId=tblstudent.id   and tblsubjects.id=tblstudent_indiviualsubject.SubjectId and tblclass.ClassName='" + classnames+"'";

                DataSet ds_class = new DataSet();
                ds_class = My_Class.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                Grd_StuSub.DataSource = ds_class;
                Grd_StuSub.DataBind();
            }

        protected void Drp_Standard_SelectedIndexChanged(object sender, EventArgs e)
        {
            string values = Drp_Standard.SelectedItem.Value;
            string data = Drp_Standard.SelectedItem.Text;
            fillthegridview(data);
        }

        protected void Grd_StuSub_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            
                string ClassNameId = Grd_StuSub.Rows[e.RowIndex].Cells[1].Text.ToString();
                string StudentName = Grd_StuSub.Rows[e.RowIndex].Cells[3].Text.ToString();
                string SubjectName = Grd_StuSub.Rows[e.RowIndex].Cells[2].Text.ToString();
                string ClassName = Grd_StuSub.Rows[e.RowIndex].Cells[4].Text.ToString();
            
                string sql = "delete from tblstudent_indiviualsubject where ClassName =" + ClassNameId + " and StudentId =" + StudentName + " and SubjectId = " + SubjectName + "";

                My_Class.m_MysqlDb.ExecuteQuery(sql);
                Grd_StuSub.EditIndex = -1;
                fillthegridview(ClassName);
            }
            
            
        

        protected void Grd_StuSub_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;
        }
    }
    }
