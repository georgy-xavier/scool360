using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using AjaxControlToolkit;
using System.Data;
using System.Drawing;
using System.IO;

namespace WinEr.WebControls
{
    public partial class ComprehensiveReportControl : System.Web.UI.UserControl
    {
        private         StudentManagerClass MyStudMang;
        private         ClassOrganiser      MyClassMang;
        private         KnowinUser          MyUser;
        private         OdbcDataReader      MyReader            = null;
        public event    EventHandler        EVENTCreateReport;
        
        public string STUDENTID
        {
            get
            {
                return Hdn_StudentID.Value;
            }
            set
            {
                Hdn_StudentID.Value = value;
            }

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }

            MyUser = (KnowinUser)Session["UserObj"];
            MyClassMang = MyUser.GetClassObj();

            if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
           
            else
            {
                if (!IsPostBack)
                {
                    LoadDetails();

                }
            }
        }
        #region Methods

        private void LoadDetails()
        {
            
            int StudentId = 0;
            int ClassId = 0;
            if (Hdn_StudentID.Value != "0")
            {
                int.TryParse(Hdn_StudentID.Value, out StudentId);
                GetClassId(StudentId, out ClassId);
            }


            LoadClassDetails(ClassId);
            LoadExamType();
            LoadExamToDrpList();
            LoadStudentInfo(StudentId);
            LoadReportList();

            if (StudentId != 0)
            {
                Drp_Class.Enabled       = false;
                Drp_StudentList.Enabled = false;
            }
        }


        private void GetClassId(int StudentId, out int ClassId)
        {
            ClassId=1;
            string Sql = "select tblclass.Id from tblview_student INNER join tblclass on tblview_student.ClassId=tblclass.Id where tblview_student.Id=" + StudentId;
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
                int.TryParse(MyReader.GetValue(0).ToString(), out ClassId);
        }

        private void LoadReportList()
        {
            ChkBxList.Items.Clear();
            ListItem L1 = new ListItem("Main Report", "Main Report");
            ListItem L2 = new ListItem("Graph Report", "Graph Report");
            ListItem L3 = new ListItem("Top Student Report", "Top Student Report");

            ChkBxList.Items.Add(L1);
            ChkBxList.Items.Add(L2);
            ChkBxList.Items.Add(L3);
            if (MyUser.HaveModule(20))
            {
                string sql = "select tblincedenttype.`Type` from tblincedenttype where tblincedenttype.Visibility=1 and   tblincedenttype.IncidentType='NORMAL' ";
                MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        ListItem Li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(0).ToString());
                        ChkBxList.Items.Add(Li);
                    }
                }
            }
            if (MyUser.HaveModule(21))
            {
                ListItem L4 = new ListItem("Attendance Report", "Attendance Report");
                ChkBxList.Items.Add(L4);
            }
            if (MyUser.HaveModule(1))
            {
                ListItem L5 = new ListItem("Fee Due Report", "Fee Due Report");
                ChkBxList.Items.Add(L5);
            }
            ListItem L6 = new ListItem("Summary", "Summary");
            ChkBxList.Items.Add(L6);

        }


        private void LoadClassDetails(int ClassId)
        {

            //Load all class and select one class if _ClassId!=0
            Drp_Class.Items.Clear();
            string sql = "SELECT tblclass.Id,tblclass.ClassName FROM tblclass where tblclass.Status =1 AND tblclass.Id IN (SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ")) ORDER BY tblclass.Standard,tblclass.ClassName";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Class.Items.Add(li);
                }

                Drp_Class.SelectedValue = ClassId.ToString();
            }
            else
            {
                ListItem li = new ListItem("No Class Found", "-1");
                Drp_Class.Items.Add(li);
            }
        }

        private void LoadExamType()
        {
            Drp_ExamType.Items.Clear();

            string sql = "select tblsubject_type.Id, tblsubject_type.sbject_type from tblsubject_type";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_Exam.Enabled = true;
                Drp_ExamType.Items.Add(new ListItem("Select any exam type", "0"));

                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_ExamType.Items.Add(li);
                }

            }
            else
            {
                Drp_ExamType.Items.Add(new ListItem("No exam type found", "-1"));
                Drp_Exam.Enabled = false;
            }
        }

        private void LoadExamToDrpList()
        {

            Drp_Exam.Items.Clear();
            string sql = "select distinct tblexammaster.Id, tblexammaster.ExamName from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster ON tblexammaster.Id= tblclassexam.ExamId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id  where tblclassexam.ClassId = " + int.Parse(Drp_Class.SelectedValue) + " and tblexamschedule.BatchId = " + MyUser.CurrentBatchId + " and tblexammaster.ExamTypeId= " + int.Parse(Drp_ExamType.SelectedValue) + " and tblexamschedule.status ='Completed'";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_Exam.Items.Add(new ListItem("Select any exam", "0"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Exam.Items.Add(li);
                }

            }
            else
            {
                Drp_Exam.Items.Add(new ListItem("No exam found", "-1"));
            }
        }

        private void LoadStudentInfo(int StudentId)
        {

            int ClassId = 0;
            int.TryParse(Drp_Class.SelectedValue.ToString(), out ClassId);
            Drp_StudentList.Items.Clear();
            string Sql = "select tblview_student.Id, tblview_student.StudentName from tblview_student INNER join tblclass on tblview_student.ClassId=tblclass.Id where tblview_student.ClassId=" + ClassId + " and  tblview_student.LIve=1  and tblview_student.RollNo<>-1  order by  tblview_student.StudentName ASC";
            // Up to Nationality Field in tblstudent
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("All Student", "0");
                Drp_StudentList.Items.Add(li);
               
                while (MyReader.Read())
                {
                    ListItem Li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_StudentList.Items.Add(Li);
                }

                Drp_StudentList.SelectedValue = StudentId.ToString();

            }
            else
            {
                ListItem li = new ListItem("No student found", "-1");
                Drp_StudentList.Items.Add(li);
            }

        }

        private bool ValidDatas(out string _msg)
        {
            bool _Valid = true;
            _msg = "";

            if (Drp_Class.SelectedValue == "-1")
            {
                _msg = "Class does not found";
                _Valid = false;
            }
            else if (Drp_ExamType.SelectedValue == "-1")
            {
                _msg = "Exam type does not found";
                _Valid = false;
            }
            else if (Drp_ExamType.SelectedValue == "0")
            {
                _msg = "Select exam type";
                _Valid = false;
            }
            else if (Drp_Exam.SelectedValue == "-1")
            {
                _msg = "Exam  does not found";
                _Valid = false;
            }
            else if (Drp_Exam.SelectedValue == "0")
            {
                _msg = "Select one exam ";
                _Valid = false;
            }
            else if (Drp_StudentList.SelectedValue == "-1")
            {
                _msg = "Student does not found ";
                _Valid = false;
            }
            else if (Drp_Exam.SelectedValue != "-1" && Drp_StudentList.SelectedValue != "-1" && Drp_StudentList.SelectedValue != "0")
            {
                string sql = "select tblstudentmark.StudId from tblstudentmark inner join tblexamschedule on tblexamschedule.Id = tblstudentmark.ExamSchId inner join  tblclassexam on tblclassexam.Id=  tblexamschedule.ClassExamId where tblclassexam.ClassId=" + Drp_Class.SelectedValue + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblclassexam.ExamId= " + Drp_Exam.SelectedValue  + " and tblexamschedule.Status='Completed' and tblstudentmark.StudId=" + Drp_StudentList.SelectedValue;
                 MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
                 if (!MyReader.HasRows)
                 {
                     _msg = "Selected student did not attend this exam.";
                     _Valid = false;
                 }
            }




            return _Valid;
        }

        private void CreateReport()
        {
            int _ClassId = 0;
            int _ExamId = 0;
            int _studentId = 0;


            int.TryParse(Drp_Class.SelectedValue.ToString(), out _ClassId);
            int.TryParse(Drp_Exam.SelectedValue.ToString(), out _ExamId);
            int.TryParse(Drp_StudentList.SelectedValue.ToString(), out _studentId);


            int Temp = 0, i = 0;
            string[,] _CheckList = null;
            if (ChkBxList.Items.Count > 0)
            {
                _CheckList = new string[ChkBxList.Items.Count, 2];


                foreach (ListItem cBox in ChkBxList.Items)
                {
                    _CheckList[i, 0] = cBox.Text;
                    if (cBox.Selected)
                    {
                        Temp = 1;
                        _CheckList[i, 1] = "1";
                    }
                    else
                    {
                        _CheckList[i, 1] = "0";
                    }
                    i++;


                }
            }
            if (Temp == 1)
            {
                if (_CheckList.Length > 0 || _CheckList != null)
                {
                    int Count = _CheckList.Length / 2;
                    string _URL = "";
                    for (i = 0; i < Count; i++)
                    {
                        _URL = _URL + "&" + _CheckList[i, 0] + "=" + _CheckList[i, 1];
                    }
                    ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"DisplyComprehensiveReport.aspx?ClassId=" + _ClassId + "&ExamId=" + _ExamId + "&studentId=" + _studentId + _URL + "&ReturnDate=" + Txt_ReturnDate.Text + "\");", true);
                   
                }
            }
            else
            {
                Lbl_Error.Text = "Select atlease one report type";
            }
        }


        #endregion

        #region Events

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            LoadStudentInfo(0);
            LoadExamType();
        }

        protected void Drp_ExamType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(Drp_ExamType.SelectedValue) != 0)
            {
                LoadExamToDrpList();
            }
            else
            {
                Drp_Exam.Items.Clear();
            }
        }

        protected void Btn_Create_Click(object sender, EventArgs e)
        {
            string _msg = "";
            Lbl_Error.Text = "";
            if (ValidDatas(out _msg))
            {
                CreateReport();
                if (EVENTCreateReport != null)
                {
                    EVENTCreateReport(this, e);
                }
            }
            else
            {
                Lbl_Error.Text = _msg;
            }
        }

        #endregion

       
    }
}