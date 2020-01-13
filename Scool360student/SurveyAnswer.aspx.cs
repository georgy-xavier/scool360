using Org.BouncyCastle.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Scool360student
{
    public partial class SurveyAnswer : System.Web.UI.Page
    {
        public MysqlClass m_MysqlDb;
        public MysqlClass m_TransationDb = null;
        //private OdbcDataReader m_MyReader = null;
        private ClassOrganiser MyClassMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        public void Page_Load(object sender, EventArgs e)
        {

        }
        public void Page_PreInit(Object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];

            if (MyUser.SELECTEDMODE == 2)
            {
                this.MasterPageFile = "~/WinerSchoolMaster.master";

            }
            else if (MyUser.SELECTEDMODE == 1)
            {
                this.MasterPageFile = "~/WinerStudentMaster.master";
            }

        }

        public void Page_init(object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyClassMang = MyUser.GetClassObj();
            if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");

            }
            else if (!MyUser.HaveActionRignt(3037))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {


                if (!IsPostBack)
                {

                    AddStandardToDrpList();

                    if (MyUser.HaveModule(3))
                    {
                        AddSubjectsToList();
                        studentidvalue();

                    }



                }
            }
        }

        public void FillDropDown()
        {
            if (Drp_Stand.SelectedValue != "")
            {
                DataSet ds = new DataSet();
                string sql = "select question,id from tbl_survey where survey_id='" + Drp_Stand.SelectedValue + "'";
                ds = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                
         
                        //string countvalue = ; 

                       int count = ds.Tables[0].Rows.Count;



                //for (int i = 1; i <= count; i++)
                //   int     {


                int i=1;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    //for (int i = 1; i <= count; i++)
                    //{
                        // DropDownList1.DataValueField = ds.Tables[0].Rows[0][1].ToString();
                        // DropDownList1.Items.Add(new ListItem("Question " + i));
                        DropDownList1.Items.Add(new ListItem() { Text = "Question " + i, Value = row["id"].ToString() });
                    i++;
                    //}
                }
                DropDownList1.Items.Insert(0, new ListItem("Select Question", ""));

                            }
        }
        
       // int studentID = MyUser.UserId.ToString();;
        public void Questions()
        {
            if (DropDownList1.SelectedValue != "")
            {
                DataSet ds = new DataSet();
                string sql = "select Question,id from tbl_survey where id=" + DropDownList1.SelectedValue;
                ds = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                //MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
                Label1.Visible = true;
                Label1.Text = ds.Tables[0].Rows[0].Field<string>(0);

            }
            }


        public void studentidvalue()
        {
            int studentID =int.Parse(MyUser.StudId.ToString());
            if (DropDownList1.SelectedValue != "")
            {
                DataSet ds = new DataSet();
                string sql = "select Studentname from tblstudent where id="+ studentID;
                ds = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                //MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);

                Label6.Text = Convert.ToString(studentID);
                Label7.Text = ds.Tables[0].Rows[0].Field<string>(0);




            }
           
        }
        public void AddSubjectsToList()
        {
            ChkBox_AllsSub.Items.Clear();
            RadioButtonList1.Items.Clear();
            TextBox2.Visible = false;
            RadioButtonList1.Visible = false;




            if (Drp_Stand.SelectedValue != "" && DropDownList1.SelectedValue != "")
            {
                DataSet ds = new DataSet();
                string sql = "select Answer,Ques_type from tbl_survey where Survey_id='" + Drp_Stand.SelectedValue +"' and id= "+ DropDownList1.SelectedValue;


                ds = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

                string questiontype = ds.Tables[0].Rows[0].Field<string>(1);
                if (questiontype == "CheckBox")
                {
                    ChkBox_AllsSub.Visible = true;
                    List<string> str = new List<string>();

                    //str.Add(MyReader.GetValue(0).ToString());
                    str.Add(ds.Tables[0].Rows[0].Field<string>(0));






                    foreach (string p in str)
                    {

                        String[] items = p.Split(',');

                        for (int i = 0; i < items.Length; i++)
                        {
                            ChkBox_AllsSub.Items.Add(items[i]);
                        }


                    }



                }
                else if (questiontype == "TextBox")
                {
                    ChkBox_AllsSub.Visible = false;
                    TextBox2.Visible = true;
                   

                }
                else if (questiontype == "RadioButton")
                {
                    ChkBox_AllsSub.Visible = false;
                    TextBox2.Visible = false;
                    RadioButtonList1.Visible = true;
                    List<string> str = new List<string>();

                    //str.Split(',').ToList();
                    str.Add(ds.Tables[0].Rows[0].Field<string>(0));






                    foreach (string p in str)
                    {

                        String[] items = p.Split(',');

                        for (int i = 0; i < items.Length; i++)
                        {
                            // RadioButton1.Items.Add(items[i]);
                            RadioButtonList1.Items.Add(items[i]);

                        }


                    }

                }



            }

        }
        public void AddStandardToDrpList()
        {
            int studentID = int.Parse(MyUser.StudId.ToString());
            DataSet ds_class = new DataSet();
            string sql = "select DISTINCT tbl_survey.survey_name,tbl_survey.Survey_id from tbl_survey join tbl_gr_groupusermap where tbl_gr_groupusermap.GroupId=tbl_survey.Group_id and tbl_gr_groupusermap.UserId=" + studentID;
            ds_class = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds_class != null && ds_class.Tables[0] != null && ds_class.Tables[0].Rows.Count > 0)
            {
                Drp_Stand.Items.Clear();
                Drp_Stand.DataSource = ds_class;

                Drp_Stand.DataValueField = "Survey_id";
                Drp_Stand.DataTextField = "survey_name";


                Drp_Stand.DataBind();
                Drp_Stand.Items.Insert(0, new ListItem("Select Survey", ""));
                FillDropDown();
                


            }
            else
            {
                ListItem li = new ListItem("No Survey Exist", "-1");
                Drp_Stand.Items.Add(li);
            }

        }
        protected void Drp_Stand_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList1.Items.Clear();
            FillDropDown();


        }





        protected void Btn_CreateClass_Click(object sender, EventArgs e)
        {

            studentidvalue();

            DataSet ds = new DataSet();
            string sql = "select Answer,Ques_type from tbl_survey where id='"+ DropDownList1.SelectedValue +"'";
          

            ds = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            string questiontype = ds.Tables[0].Rows[0].Field<string>(1);
            if (questiontype == "CheckBox")
            {
                if (Drp_Stand.SelectedValue.ToString() == "-1")
            {
                Lbl_msg.Text = "Choose the Dropdown";
            }

           
            else if ((ChkBox_AllsSub.Items.Count == 0) && (MyUser.HaveModule(3)))
                {
                    Lbl_msg.Text = "Please select subjects for this Students";
                }
            }
            else if (questiontype == "Textbox" || questiontype == "TextBox")
            {
                if (Drp_Stand.SelectedValue.ToString() == "-1")
                {
                    Lbl_msg.Text = "Choose the Dropdown";
                }


                else if ((ChkBox_AllsSub.Items.Count == 0) && (MyUser.HaveModule(3)))
                {
                    Lbl_msg.Text = "Please select subjects for this Students";
                }
            }
            else if (questiontype == "RadioButton")
            {
                if (Drp_Stand.SelectedValue.ToString() == "-1")
                {
                    Lbl_msg.Text = "Choose the Dropdown";
                }


                else if ((RadioButtonList1.Items.Count == 0) && (MyUser.HaveModule(3)))
                {
                    Lbl_msg.Text = "Please select subjects for this Students";
                }
            }
            

                if (questiontype == "CheckBox")
                {
                    int _ClassiD;


                    if (MyUser.HaveModule(3))
                    {


                        for (int i = 0; i < ChkBox_AllsSub.Items.Count; i++)
                        {

                        if (ChkBox_AllsSub.Items[i].Selected)
                        {

                            _ClassiD = MyClassMang.SurveyAnswer(int.Parse(Drp_Stand.SelectedValue.ToString()), Label1.Text, ChkBox_AllsSub.Items[i].Value.ToString(), Label6.Text, Label7.Text);

                        }
                        }

                    }
                    MyUser.ClearAssociatedClass();
                    Lbl_msg.Text = "Submitted";


                }
                else if (questiontype == "Textbox" || questiontype == "TextBox")
                {

                    int _ClassiD;


                    if (MyUser.HaveModule(3))
                    {





                        _ClassiD = MyClassMang.SurveyAnswer(int.Parse(Drp_Stand.SelectedValue.ToString()), Label1.Text, TextBox2.Text, Label6.Text, Label7.Text);




                    }
                    MyUser.ClearAssociatedClass();
                    Lbl_msg.Text = "Submitted";


                }
                else if (questiontype == "RadioButton")
                {
                    int _ClassiD;


                    if (MyUser.HaveModule(3))
                    {


                        for (int i = 0; i < RadioButtonList1.Items.Count; i++)
                        {

                        if (RadioButtonList1.Items[i].Selected)
                        {

                            _ClassiD = MyClassMang.SurveyAnswer(int.Parse(Drp_Stand.SelectedValue.ToString()), Label1.Text, RadioButtonList1.Items[i].Value.ToString(),Label6.Text,Label7.Text);

                        }
                        }

                    }
                    MyUser.ClearAssociatedClass();
                    Lbl_msg.Text = "Submitted";



                }
            Clear();
                MPE_MessageBox.Show();
            }
        private void Clear()
        {
            // Drp_Stand.Items.Clear();

            Drp_Stand.Items.Clear();
            AddSubjectsToList();
            TextBox2.Text = "";
            AddStandardToDrpList();
            RadioButtonList1.Items.Clear();
            ChkBox_AllsSub.Items.Clear();
            Questions();
            Label1.Text = "";
            DropDownList1.Items.Clear();
        }

        protected void But_Reset_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Questions();
            AddSubjectsToList();
            
        }
    }
}
