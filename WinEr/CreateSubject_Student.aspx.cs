using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WinEr
{
    public partial class CreateSubject_Student : System.Web.UI.Page
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
            else if (!MyUser.HaveActionRignt(16))
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
                        
                    }
                  
                    

                }
            }
        }
        public void AddSubjectsToList()
        {
            ChkBox_AllsSub.Items.Clear();

            // string sql = "SELECT Id,subject_name FROM tblsubjects";
            if (Drp_Stand.SelectedValue != "" && Drp_Students.SelectedValue != "")
            {
                string sql = "SELECT Id,subject_name  FROM tblsubjects where id NOT IN(Select tblsubjects.Id from tblsubjects inner join tblclasssubmap on tblsubjects.Id = tblclasssubmap.SubjectId where tblclasssubmap.ClassId =" + Drp_Stand.SelectedValue + ")and id NOT IN(select SubjectId from tblstudent_indiviualsubject  where StudentId=" + Drp_Students.SelectedValue +  ")";

                MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);

                



                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        ChkBox_AllsSub.Items.Add(li);
                    }
                }
            }

        }



        public void AddStandardToDrpList()
        {
            DataSet ds_class = new DataSet();
            string sql = "select Id,ClassName from tblclass where Status=1";
            ds_class = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds_class != null && ds_class.Tables[0] != null && ds_class.Tables[0].Rows.Count > 0)
            {
                Drp_Stand.Items.Clear();
                Drp_Stand.DataSource = ds_class;
              
                Drp_Stand.DataValueField = "Id";
                Drp_Stand.DataTextField = "ClassName";
               

                Drp_Stand.DataBind();
                Drp_Stand.Items.Insert(0, new ListItem("Select Class", ""));
                AddSubjectsToList();

            }
            else
            {
                ListItem li = new ListItem("No Class Exist", "-1");
                Drp_Stand.Items.Add(li);
            }

        }

        public void AddStudentsToDrpList()
        {
            int Class_Id = 0;
            int.TryParse(Drp_Stand.SelectedValue, out Class_Id);
            if (Class_Id > 0)
            {
                DataSet ds_Student = new DataSet();
                string sql = "select tblstudent.Id,tblstudent.StudentName from tblstudent inner join tblstudentclassmap on tblstudent.Id=tblstudentclassmap.StudentId where tblstudentclassmap.ClassId=" + Class_Id + "";
                ds_Student = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
               
                           Drp_Students.Items.Clear();
                    Drp_Students.DataSource = ds_Student;
               
                Drp_Students.DataValueField = "Id";
                    Drp_Students.DataTextField = "StudentName";
                    Drp_Students.DataBind();

                
                   
                
            }
        }
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ChkBox_AllsSub.Items.Count; i++)
            {
                if (ChkBox_AllsSub.Items[i].Selected)
                {
                    ChkBox_AllsSub.Items[i].Selected = false;
                    ChkBox_Classsubject.Items.Add(ChkBox_AllsSub.Items[i]);
                    ChkBox_AllsSub.Items.Remove(ChkBox_AllsSub.Items[i]);
                    i--;
                }
            }
        }
        protected void Btn_Remove_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ChkBox_Classsubject.Items.Count; i++)
            {
                if (ChkBox_Classsubject.Items[i].Selected)
                {
                    ChkBox_Classsubject.Items[i].Selected = false;
                    ChkBox_AllsSub.Items.Add(ChkBox_Classsubject.Items[i]);
                    ChkBox_Classsubject.Items.Remove(ChkBox_Classsubject.Items[i]);
                    i--;
                }
            }

        }

        

        private void Clear()
        {
           // Drp_Stand.Items.Clear();

            Drp_Students.Items.Clear();
            AddSubjectsToList();
            AddStandardToDrpList();
            ChkBox_Classsubject.Items.Clear();
        }
       
        protected void But_Reset_Click(object sender, EventArgs e)
        {
            Clear();


        }
        //protected void Btn_link_Click(object sender, EventArgs e)
        //{
        //    string pageName = Path.GetFileName(Request.Path);
        //    Lbl_msg.Text = pageName;
        //    MPE_MessageBox.Show();

        //}

        protected void Drp_Stand_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddStudentsToDrpList();
            AddSubjectsToList();

        }

        protected void Drp_Students_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddSubjectsToList();
        }

     

        protected void Btn_CreateClass_Click(object sender, EventArgs e)
        {
            
          
            if (Drp_Stand.SelectedValue.ToString() == "-1")
            {
                Lbl_msg.Text = "Choose the Dropdown";
            }
            if (Drp_Students.SelectedValue.ToString() == "")
            {
                Lbl_msg.Text = "Please Select Students From DropDown";
            }

            else if ((ChkBox_Classsubject.Items.Count == 0) && (MyUser.HaveModule(3)))
            {
                Lbl_msg.Text = "Please select subjects for this Students";
            }
            else
            {
                int _ClassiD;
                

                if (MyUser.HaveModule(3))
                {

                    for (int i = 0; i < ChkBox_Classsubject.Items.Count; i++)
                    {
                      
                        _ClassiD = MyClassMang.Createstudentsuject(int.Parse(Drp_Stand.SelectedValue.ToString()), int.Parse(Drp_Students.SelectedValue.ToString()), int.Parse(ChkBox_Classsubject.Items[i].Value.ToString()));
                    }
                }
                MyUser.ClearAssociatedClass();
                Lbl_msg.Text = "Student Subject is Created";

                Clear();
            }
            MPE_MessageBox.Show();
        }

        
    }
    }
