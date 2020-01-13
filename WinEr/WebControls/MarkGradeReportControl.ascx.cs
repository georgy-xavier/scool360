using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;

namespace WinEr.WebControls
{
    public partial class MarkGradeReportControl : System.Web.UI.UserControl
    {
        private StudentManagerClass MyStudMang;
        private ClassOrganiser MyClassMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        public event EventHandler EVENTCreateReport;

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
        private void LoadDetails()
        {

            int StudentId = 0;
            int ClassId = 1;
            if (Hdn_StudentID.Value != "0")
            {
                int.TryParse(Hdn_StudentID.Value, out StudentId);
                GetClassId(StudentId, out ClassId);
            }


            LoadClassDetails(ClassId);
            LoadExamType();
            LoadExamToDrpList();
            LoadStudentInfo(StudentId);
            LoadCharacterTraitsArea();
            LoadCoCurricularArea();

            if (StudentId != 0)
            {
                Drp_Class.Enabled = false;
                Drp_StudentList.Enabled = false;
            }
        }


        private void GetClassId(int StudentId, out int ClassId)
        {
            ClassId = 1;
            string Sql = "select tblclass.Id from tblview_student INNER join tblclass on tblview_student.ClassId=tblclass.Id where tblview_student.Id=" + StudentId;
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
                int.TryParse(MyReader.GetValue(0).ToString(), out ClassId);
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
                string sql = "select tblstudentmark.StudId from tblstudentmark inner join tblexamschedule on tblexamschedule.Id = tblstudentmark.ExamSchId inner join  tblclassexam on tblclassexam.Id=  tblexamschedule.ClassExamId where tblclassexam.ClassId=" + Drp_Class.SelectedValue + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblclassexam.ExamId= " + Drp_Exam.SelectedValue + " and tblexamschedule.Status='Completed' and tblstudentmark.StudId=" + Drp_StudentList.SelectedValue;
                MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
                if (!MyReader.HasRows)
                {
                    _msg = "Selected student does not attend this exam. Please select another studnet.";
                    _Valid = false;
                }
            }



            return _Valid;
        }

        private void CreateReport()
        {
            int ClassId = 0;
            int ExamId = 0;
            int StudentId = 0;

            int coCurricularExamId = 0;
            int CharacterTraitsExamId = 0;

            int.TryParse(Drp_Class.SelectedValue.ToString(), out ClassId);
            int.TryParse(Drp_Exam.SelectedValue.ToString(), out ExamId);
            int.TryParse(Drp_StudentList.SelectedValue.ToString(), out StudentId);

            int.TryParse(Drp_Cocurricular.SelectedValue.ToString(), out coCurricularExamId);
            int.TryParse(Drp_CharecterTraits.SelectedValue.ToString(), out CharacterTraitsExamId);

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
                    ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"DisplayMarkGradeReport.aspx?ClassId=" + ClassId + "&ExamId=" + ExamId + "&studentId=" + StudentId + "&cocurriculatexam=" + coCurricularExamId + "&charecterTraitsExamId=" + CharacterTraitsExamId + _URL + "\");", true);

                }
            }
            else
            {
                Lbl_Error.Text = "Select atlease one report type";
            }

        }

        private void LoadCharacterTraitsArea()
        {
            int _SubTypeId = 0;
            string _Co_CurricularExam = GetExamTypeName("Internal", out _SubTypeId);
            if (Drp_Class.SelectedValue != "-1")
            {
                Drp_CharecterTraits.Items.Clear();
                //string sql = "select tblexammaster.Id, tblexammaster.ExamName from tblsubject_type  inner join tblexammaster on tblexammaster.ExamTypeId= tblsubject_type.Id where   tblsubject_type.TypeDisc='CBSE ACTIVITY REPORT';";
                string sql = "select distinct tblexammaster.Id, tblexammaster.ExamName from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster ON tblexammaster.Id= tblclassexam.ExamId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id  where tblclassexam.ClassId = " + int.Parse(Drp_Class.SelectedValue) + " and tblexamschedule.BatchId = " + MyUser.CurrentBatchId + " and tblexammaster.ExamTypeId= " + _SubTypeId + " and tblexamschedule.status ='Completed'";

                MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);

                if (MyReader.HasRows)
                {
                    Drp_CharecterTraits.Enabled = true;
                    Drp_CharecterTraits.Items.Add(new System.Web.UI.WebControls.ListItem("Select Co-Scholastic area", "0"));

                    while (MyReader.Read())
                    {
                        System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_CharecterTraits.Items.Add(li);
                    }
                }
                else
                {
                    Drp_CharecterTraits.Items.Add(new System.Web.UI.WebControls.ListItem("No Details Found", "-1"));
                    Drp_CharecterTraits.Enabled = false;
                }
            }
            else
            {
                Drp_CharecterTraits.Items.Add(new System.Web.UI.WebControls.ListItem("Select Co-Scholastic area", "0"));
            }
        }

        private void LoadCoCurricularArea()
        {
            int _SubTypeId = 0;
            string _Co_CurricularExam = GetExamTypeName("CBSE ACTIVITY REPORT", out _SubTypeId);
            if (Drp_Class.SelectedValue != "-1")
            {
                Drp_Cocurricular.Items.Clear();
                //string sql = "select tblexammaster.Id, tblexammaster.ExamName from tblsubject_type  inner join tblexammaster on tblexammaster.ExamTypeId= tblsubject_type.Id where   tblsubject_type.TypeDisc='CBSE ACTIVITY REPORT';";
                string sql = "select distinct tblexammaster.Id, tblexammaster.ExamName from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster ON tblexammaster.Id= tblclassexam.ExamId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id  where tblclassexam.ClassId = " + int.Parse(Drp_Class.SelectedValue) + " and tblexamschedule.BatchId = " + MyUser.CurrentBatchId + " and tblexammaster.ExamTypeId= " + _SubTypeId + " and tblexamschedule.status ='Completed'";

                MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);

                if (MyReader.HasRows)
                {
                    Drp_Cocurricular.Items.Add(new System.Web.UI.WebControls.ListItem("Select Co-Scholastic area", "0"));

                    while (MyReader.Read())
                    {
                        System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_Cocurricular.Items.Add(li);
                    }
                }
                else
                {
                    Drp_Cocurricular.Items.Add(new System.Web.UI.WebControls.ListItem("No Details Found", "-1"));
                }
            }
            else
            {
                Drp_Cocurricular.Items.Add(new System.Web.UI.WebControls.ListItem("Select Co-Scholastic area", "0"));
            }
        }

        private string GetExamTypeName(string _ExamDescription, out int _SubTypeId)
        {
            _SubTypeId = 0;
            string _ExamTypeName = "";
            string sql = "select tblsubject_type.Id, tblsubject_type.sbject_type from tblsubject_type where tblsubject_type.TypeDisc='" + _ExamDescription + "' ";

            OdbcDataReader Reader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);

            if (Reader.HasRows)
            {
                _ExamTypeName = Reader.GetValue(1).ToString();
                _SubTypeId = int.Parse(Reader.GetValue(0).ToString());
            }

            return _ExamTypeName;

        }


        #region Events

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {

            LoadStudentInfo(0);
            LoadExamType();
            LoadCharacterTraitsArea();
            LoadCoCurricularArea();

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

        protected void ChkBxList_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListItem cBox in ChkBxList.Items)
            {

                if (cBox.Text == "Co-Curricular Activities and Character Traits" )
                {
                    if(cBox.Selected)
                    {
                        Drp_CharecterTraits.Enabled = true;
                        Drp_Cocurricular.Enabled = true;
                    }
                    else
                    {
                        Drp_CharecterTraits.Enabled = false;
                        Drp_Cocurricular.Enabled = false;
                    }
                }

            }
        }
    }
}