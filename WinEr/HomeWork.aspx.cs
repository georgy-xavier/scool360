using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;
using System.Data.Odbc;
using System.Drawing;

namespace WinEr
{
    public partial class HomeWork : System.Web.UI.Page
    {
        private ClassOrganiser My_Class;
        private KnowinUser MyUser;
        private OdbcDataReader My_Reader;
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
                //no rights for this user.
            }
            else
            {
                if (!IsPostBack)
                {
                    Initial_Load();
                    Txt_Search_AutoCompleteExtender.ContextKey = Drp_class.SelectedValue;

                }
            }
        }
        private void Initial_Load()
        {
            Rdb_Selection_type.SelectedValue = "1";
            Load_All_Active_Classes();
            DateTime _date = System.DateTime.Now.AddDays(1);
            Txt_Dt.Text = MyUser.GerFormatedDatVal(_date);
            Panel_Students.Visible = false;
            Txt_Description.Text = "";
            Txt_Search.Text = "";
           
        }
        protected void Rdb_Selection_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Rdb_Selection_type.SelectedValue == "1")
            {
                Panel_Students.Visible = false;
            }
            else
            {
                Panel_Students.Visible = true;
                Load_All_Students();
              
            }
            lbl_Msg.Text = "";
           
        }
        protected void Drp_class_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Rdb_Selection_type.SelectedValue == "1")
            {
                Panel_Students.Visible = false;
            }
            else
            {
                Panel_Students.Visible = true;
                

            }
            Load_All_Students();
            Txt_Search.Text = "";
            Txt_Search_AutoCompleteExtender.ContextKey = Drp_class.SelectedValue;
            lbl_Msg.Text = "";
        }
   
        protected void Btn_Save_click(object sender, EventArgs e)
        {
            string Msg = "";
            try
            {
                if (_validations(out Msg))
                {
                    Save_Home_Work();
                    Initial_Load();
                    lbl_Msg.Text = "Home Work Saved Successfully";
                    lbl_Msg.ForeColor = Color.Green;
                }
                else
                {
                    lbl_Msg.Text = Msg;
                    lbl_Msg.ForeColor = Color.Red;
                }
            }
            catch
            {
                lbl_Msg.Text = "Error Ocurred";
                lbl_Msg.ForeColor = Color.Red;
            }
        }
        protected void Btn_Clear_Click(object sender, EventArgs e)
        {
            Initial_Load();
        }
        private void Load_All_Active_Classes()
        {
            DataSet ds_class = new DataSet();
            string sql = "select Id,ClassName from tblclass where Status=1";
            ds_class = My_Class.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds_class != null && ds_class.Tables[0] != null && ds_class.Tables[0].Rows.Count > 0)
            {
                Drp_class.Items.Clear();
                Drp_class.DataSource = ds_class;
                Drp_class.DataValueField = "Id";
                Drp_class.DataTextField = "ClassName";
                Drp_class.DataBind();

            }
            else
            {
                ListItem li = new ListItem("No Class Exist", "-1");
                Drp_class.Items.Add(li);
            }
        }
        private void Load_All_Students()
        {
            int Class_Id = 0;
            int.TryParse(Drp_class.SelectedValue, out Class_Id);
            if (Class_Id > 0)
            {
                DataSet ds_Student = new DataSet();
                string sql = "select tblstudent.Id,tblstudent.StudentName from tblstudent inner join tblstudentclassmap on tblstudent.Id=tblstudentclassmap.StudentId where tblstudentclassmap.ClassId=" + Class_Id + "";
                ds_Student = My_Class.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (ds_Student != null && ds_Student.Tables[0] != null && ds_Student.Tables[0].Rows.Count > 0)
                {
                    Chkb_students.Items.Clear();
                    Chkb_students.DataSource = ds_Student;
                    Chkb_students.DataTextField = "StudentName";
                    Chkb_students.DataValueField = "Id";
                    Chkb_students.DataBind();
                    lblstudentmsg.Visible = false;
                }
                else
                {
                    Panel_Students.Visible = false;
                    lblstudentmsg.Visible = true;
                }
            }
        }

        private bool Get_Subject_Id(string sub_Name,out int Sub_Id)
        {
            bool valid=true;
            Sub_Id = 0;
            DataSet ds = new DataSet();
            string sql = "select Id from tblsubjects where subject_name='" + sub_Name + "'";
            ds = My_Class.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int.TryParse(dr["Id"].ToString(), out Sub_Id);
                }
            }
            else
            {
                valid = false;
            }

            return valid;
        }
        private bool _validations(out string Msg)
        {
            bool valid = true;
            int sub_Id=0;
            Msg = "";
            string Batch = MyUser.CurrentBatchName;
            string[] split = Batch.Split('-');
            int Year = 0;
            int.TryParse(split[1].ToString(), out Year);
            if (Txt_Search.Text == "")
            {
                valid = false;
                Msg = "Please enter Subject";
            }
            else if (Txt_Description.Text=="")
            {
                valid = false;
                Msg = "Please enter Description";
            }
            else if(Txt_Dt.Text=="")
            {
                valid = false;
                Msg = "Please enter Completion Date";
            }
            else if(!Get_Subject_Id(Txt_Search.Text.Trim(),out sub_Id))
            {
                valid = false;
                Msg = "Entered Subject Not Exists";

            }
            else if (Rdb_Selection_type.SelectedValue == "2" && Chkb_students.SelectedIndex == -1)
            {
                valid = false;
                Msg = "Please select students";
            }
            else if (Rdb_Selection_type.SelectedValue == "1" && Chkb_students.Items.Count <= 0)
            {
                valid = false;
                Msg = "No Student Exist in Selected Class";
            }
            else if (DateTime.Now.Date > MyUser.GetDareFromText(Txt_Dt.Text))
            {
                valid = false;
                Msg = "Enter Valid Completion Date";
            }
            else if (DateTime.Now.Date > MyUser.GetDareFromText(Txt_Dt.Text))
            {
                valid = false;
                Msg = "Enter Valid Completion Date";
            }
            else if (MyUser.GetDareFromText(Txt_Dt.Text).Year > Year)
            {
                valid = false;
                Msg = "Your entred date is out of Batch date";
            }
            return valid;
        }
        private void Save_Home_Work()
        {
            int Annouce_Id = 0;
            int Ref_Type = 0;
            int Sub_Id = 0;
            int Class_Id = 0;
            int Student_Id=0;
            Get_Subject_Id(Txt_Search.Text.Trim(), out Sub_Id);
            int.TryParse(Drp_class.SelectedValue,out Class_Id);
            DateTime Max_Dt = MyUser.GetDareFromText(Txt_Dt.Text.Trim());
            int.TryParse(Rdb_Selection_type.SelectedValue, out Ref_Type);
            DateTime dt_now = System.DateTime.Now;
            string sql = "insert into tbl_announcemnts(Title,Body,RedId,RefId,RefType,CreatedDatetime,ExpiryDatetime,Type) values('" + Txt_Search.Text.Trim() + "','" + Txt_Description.Text.Trim() + "'," + Class_Id + "," + Sub_Id + "," + Ref_Type + ",'" + dt_now.ToString("s") + "','" + Max_Dt.ToString("s") + "',2)";
            My_Class.m_MysqlDb.ExecuteQuery(sql);
            if (Ref_Type == 2)
            {
                sql = "select DISTINCT Id from tbl_announcemnts where CreatedDatetime='" + dt_now.ToString("yyyy-MM-dd HH:mm:ss") +"'";
                DataSet ds = new DataSet();
                ds = My_Class.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count>0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int.TryParse(dr["Id"].ToString(), out Annouce_Id);
                    }
                }
                foreach (ListItem item in this.Chkb_students.Items)
                {
                    if (item.Selected)
                    {
                        int.TryParse(item.Value, out Student_Id);
                        sql = "insert into tbl_annoucement_studentmap(AnnId,StudentId)values(" + Annouce_Id + "," + Student_Id + ")";
                        My_Class.m_MysqlDb.ExecuteQuery(sql);
                    }
                }
            }
        }
    }
}
