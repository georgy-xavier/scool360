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
using System.util;
using System.Text;
using WinBase;
using System.IO;

namespace WinEr
{
    public partial class ConsolidateClassExamReport : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        private StudentManagerClass MyStudMang;
        private OdbcDataReader MyReader = null;
        private ExamManage MyExamMang;
        private Attendance MyAttendance;
        private SchoolClass objSchool = null;
        private string M_Logo = "";
        private ExamManage MyExamDetails;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }

            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MyExamMang = MyUser.GetExamObj();
            MyExamDetails = MyUser.GetExamObj();
            MyAttendance = MyUser.GetAttendancetObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (MyExamMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }

            else if (!MyUser.HaveActionRignt(885))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                    loadClassDetails();
                }
            }
        }

        private void LoadDetails()
        {
            loadClassDetails();
            LoadStudentsDetailsToDropDown();

        }

        private void loadClassDetails()
        {
            Drp_Class.Items.Clear();
            OdbcDataReader MyReader = MyExamMang.getclass();
            if (MyReader.HasRows)
            {
                ListItem li1 = new ListItem("Select Class", "0");
                Drp_Class.Items.Add(li1);
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                ListItem li1 = new ListItem("No Class", "0");
                Drp_Class.Items.Add(li1);
            }
            MyReader.Close();
        }
        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {

            LoadStudentsDetailsToDropDown();

        }


        private void LoadStudentsDetailsToDropDown()
        {
            if (Drp_Class.SelectedValue != "-1")
            {
                int ClassId = 0;
                int.TryParse(Drp_Class.SelectedValue.ToString(), out ClassId);

                Drp_Student.Items.Clear();
                string Sql = "select tblview_student.Id, tblview_student.StudentName from tblview_student INNER join tblclass on tblview_student.ClassId=tblclass.Id where tblview_student.ClassId=" + ClassId + " and  tblview_student.LIve=1  and tblview_student.RollNo<>-1  order by  tblview_student.StudentName ASC";
                OdbcDataReader MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(Sql);

                if (MyReader.HasRows)
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("All Student", "0");
                    Drp_Student.Items.Add(li);

                    while (MyReader.Read())
                    {
                        System.Web.UI.WebControls.ListItem Li = new System.Web.UI.WebControls.ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_Student.Items.Add(Li);
                    }
                }
                else
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Student Not Found", "-1");
                    Drp_Student.Items.Add(li);
                }
            }
            else
            {

                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("All Student", "0");
                Drp_Student.Items.Add(li);
            }
        }



        protected void Btn_GenReport_Click(object sender, EventArgs e)
        {
            int studentid = 0;
            if ((Drp_Class.SelectedValue != "-1"))
            {
                studentid = int.Parse(Drp_Student.SelectedValue);
                //int ExamId = int.Parse(Drp_Exam.SelectedValue);
                string FileName = Drp_Class.SelectedItem.ToString();
                // FileName = FileName + "Exam Report.xls";
                // ClassTeacherReport.InnerHtml = MyExamMang.GetClassTeacherReport(int.Parse(Drp_Class.SelectedValue), ExamId);
                Response.ContentType = "application/force-download";
                Response.AddHeader("content-disposition", "attachment; filename=" + FileName + ".xls");
                Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
                Response.Write("<head>");
                Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                Response.Write("<!--[if gte mso 9]><xml>");
                Response.Write("<x:ExcelWorkbook>");
                Response.Write("<x:ExcelWorksheets>");
                Response.Write("<x:ExcelWorksheet>");
                Response.Write("<x:Name>Exam Report</x:Name>");
                Response.Write("<x:WorksheetOptions>");
                Response.Write("<x:Print>");
                Response.Write("<x:ValidPrinterInfo/>");
                Response.Write("</x:Print>");
                Response.Write("</x:WorksheetOptions>");
                Response.Write("</x:ExcelWorksheet>");
                Response.Write("</x:ExcelWorksheets>");
                Response.Write("</x:ExcelWorkbook>");
                Response.Write("</xml>");
                Response.Write("<![endif]--> ");
                string _report = GetConsolClsExamReport(int.Parse((Drp_Class.SelectedValue).ToString()),studentid);
                Response.Write(_report);
                Response.Write("</head>");
                Response.Flush();
                Response.End();
            }

        }

        private string GetExamScheduleId(int ClassId, int ExamId)
        {
            string examScheduledId = "";
            string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period, tblexamschedule.GradeMasterId from tblexammaster inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id     where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblexammaster.Id=" + ExamId + " and tblexamschedule.Status='Completed'  order by tblexamschedule.id ASC";

            OdbcDataReader ClassReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

            if (ClassReader.HasRows)
            {
                examScheduledId = ClassReader.GetValue(0).ToString();
            }


            return examScheduledId;
        }
        private DataSet GetMarkDetails(string ExamScheduledId, string Class)
        {
            string sql = "select  tblstudent.id, tblstudent.StudentName, tblstudentclassmap.RollNo, tblstudentmark.TotalMark, tblstudentmark.TotalMax, tblexammaster.ExamName from tblstudentmark inner join tblstudent on tblstudent.id= tblstudentmark.StudId inner join tblstudentclassmap  on tblstudentclassmap.StudentId= tblstudent.id   inner join tblexamschedule on tblexamschedule.Id=tblstudentmark.ExamSchId inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId  inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId where tblstudentclassmap.ClassId=" + Class + " and tblstudentmark.ExamSchId in(" + ExamScheduledId + ")  order By RollNo";
            DataSet MarkDetails = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return MarkDetails;
        }

        # region Consolidateclassexamreport

        public string GetConsolClsExamReport(int _ClassId,int _studentId)
        {
            DataSet Students = null;
            StringBuilder CTR = new StringBuilder();
            DataSet Subjects = GetSubjects(_ClassId);
            DataSet MainSubjects = GetMainSubjects(_ClassId);
            if(_studentId!=0)
            {
                string StudentName="",AdmitionNo="";
                Students = new DataSet();

                DataTable dt = new DataTable("MyTable");
                dt.Columns.Add(new DataColumn("Id", typeof(int)));
                dt.Columns.Add(new DataColumn("StudentName", typeof(string)));
                dt.Columns.Add(new DataColumn("AdmitionNo", typeof(string)));
                GetStudentDetails(_studentId, out StudentName, out AdmitionNo);
                DataRow dr = dt.NewRow();
                dr["Id"] = _studentId;
                dr["StudentName"] = StudentName;
                dr["AdmitionNo"] = AdmitionNo;
                dt.Rows.Add(dr);
                Students.Tables.Add(dt);
                
            }
            else
            {
                Students = GetStudents(_ClassId);
            }
            
            DataSet Exams = null;
            DataSet ExamMark = null;
            double TotMark = 0;
            double Total = 0;
            double Maxtotal = 0,allMaxTotal = 0;
            double MaxMark = 0;
            string Grade = "", Remark = "";
            int _workingdays, _absentdays, halfdays, _presentdays,subcount=0;
            string Mark = "0";
            double subPercentage = 0;
            double ExamTotal = 0,allExamtotal;
            double percentage = 0, allPercentage=0;
            string allGrade = "";
            string Grp3Grade = "";
            int SlNo = 1;
            int ExamId = 0;
            int ClassExamId = 0;
            subcount = Subjects.Tables[0].Rows.Count;
            string[] ExTotSubMark = new string[subcount];
            //string[] submark; 
            if (Students != null && Students.Tables != null && (Students.Tables[0].Rows.Count > 0))
            {
                //string schoolLogo = GtSchoolLogo();
                int ColumnCount = 30;
                string classteacher = "";
                string sql = "SELECT SchoolName,Address FROM tblschooldetails";
                string classname = Drp_Class.SelectedItem.Text;
                
                MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    
                    CTR.Append( "<table width=\"100%\">");
                    CTR.Append("<tr>");
                    //CTR.Append("<img src="+ schoolLogo + " alt=\"School Logo\" width=\"500\" height=\"377\">");
                    CTR.Append("<td colspan=\"" + ColumnCount + "\" style=\"font-size:24px;text-align:center;height:40px;font-weight:bold\">" + MyReader.GetValue(0).ToString() + "");
                    CTR.Append("</td>");
                    CTR.Append("</tr>");
                    CTR.Append("<tr>");
                    CTR.Append("<td colspan=\"" + ColumnCount + "\" style=\"font-size:20px;text-align:center;height:35px;font-weight:bold;border: thin solid #000000\">" + MyReader.GetValue(1).ToString() + "");
                    CTR.Append("</td>");
                    CTR.Append("</tr>");
                    CTR.Append("</table>");
                }
                MyReader.Close();
                sql = "select tbluser.SurName from tbluser inner join tblclass on tblclass.ClassTeacher= tbluser.Id  where tblclass.Id=" + _ClassId;
                MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                MyReader.Read();
                if (MyReader.HasRows)
                {
                    classteacher = MyReader.GetValue(0).ToString();
                }
                CTR.Append("<table width=\"100%\">");
                CTR.Append("<tr>");
                CTR.Append("<td colspan=\"" + ColumnCount / 2 + "\" style=\"font-size:14px;text-align:left;height:30px;font-weight:bold;border: thin solid #000000\">CLASS:" + classname + "");
                CTR.Append("</td>");
                CTR.Append("<td colspan=\"" + ColumnCount / 2 + "\" style=\"font-size:14px;text-align:right;height:30px;font-weight:bold;border: thin solid #000000\">NAME OF THE CLASS TEACHER:" + classteacher + "");
                CTR.Append("</td>");
                CTR.Append("</tr>");                
                CTR.Append("</table>");
                CTR.Append("<table id=\"MyReport\" runat=\"server\" width=\"100%\" style=\"border: thin solid #000000\">");
                CTR.Append("<tr>");
                CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Sl No</td>");
                CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Adm NO</td>");
                CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Student Name</td>");
                CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Test/Exam</td>");
                foreach (DataRow Dr_Subject in Subjects.Tables[0].Rows)
                {
                    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Dr_Subject[1].ToString() + "");
                    CTR.Append("</td>");
                }
                CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Total</td>");
                CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Percentage</td>");
                CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Grade/Rank</td>");
                CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Total Working Days</td>");
                CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">No of Days Attended</td>");
                CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Remarks</td>");
                CTR.Append("</tr>");

              


                foreach (DataRow Dr_Students in Students.Tables[0].Rows)
                {
                    allExamtotal = 0; allMaxTotal = 0;
                    Exams = GetExams(_ClassId);
                    int _examcount = (Exams.Tables[0].Rows.Count) + 2;
                    int count = 0;
                    double Exsubmartotal = 0;
                    double Exsubperctotal = 0;
                    DataSet subjmarks = null;
                    subjmarks = new DataSet();
                    
                    CTR.Append("<tr>");
                    CTR.Append("<td rowspan=" + _examcount + " style=\" font-weight:bold; text-align:center; border: thin solid #000000\">" + SlNo + "");
                    CTR.Append("</td>");
                    CTR.Append("<td rowspan=" + _examcount + " style=\" font-weight:bold; text-align:center; border: thin solid #000000\">" + Dr_Students[2].ToString() + "");
                    CTR.Append("</td>");
                    CTR.Append("<td rowspan=" + _examcount + " style=\" font-weight:bold; text-align:center; border: thin solid #000000\">" + Dr_Students[1].ToString() + "");
                    CTR.Append("</td>");                  
                    if (Exams != null && Exams.Tables != null && Exams.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow Dr_Exams in Exams.Tables[0].Rows)
                        {
                            TotMark = 0;
                            int gradeId = 0;
                            int sub = 0;
                            DataSet Grades = new DataSet();
                            
                            CTR.Append("<tr>");
                            CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">" + Dr_Exams[1].ToString() + "");
                            CTR.Append("</td>");
                            ExamId = GetExamId(Dr_Exams[0].ToString(), _ClassId);
                            
                            ClassExamId = GetClassExamId(Dr_Exams[0].ToString());
                            Maxtotal = 0;
                            ExamTotal = 0;
                            GetAttendanceDetails(int.Parse(Dr_Students[0].ToString()), _ClassId, out _workingdays, out _presentdays);
                            gradeId = MyExamDetails.GetGradeMaster(ExamId);
                            Grades = MyExamDetails.GetGradeDataSet(gradeId);
                            string[] submark = new string[subcount];

                            DataTable dt = new DataTable("MyTable"+count);
                            dt.Columns.Add(new DataColumn("Marks"+ count, typeof(string)));
                            dt.Columns.Add(new DataColumn("Percent" + count, typeof(string)));
                            foreach (DataRow Dr_Subject in Subjects.Tables[0].Rows)
                            {
                                //if(subjectInTheExam(ClassExamId,Dr_Subject[0].ToString()))
                                //{
                                   
                                    string Mark1 = "";
                                    Mark = "0";
                                    Mark = GetSubjectMarks(Dr_Exams[0].ToString(), Dr_Subject[0].ToString(), Dr_Students[0].ToString());
                                    MaxMark = GetMaxToTal(_ClassId, ExamId, Dr_Subject[0].ToString());
                                   if(int.Parse(Dr_Subject[2].ToString())==8)
                                   {
                                      
                                       if (Mark == "a" || Mark == "A")
                                       {
                                           Mark1 = "0";
                                       }
                                       else
                                       {

                                           Grp3Grade = GetGradeFromMarks(Grades, double.Parse(Mark), MaxMark);
                                           //for group III subjects take the marks and convert to grade
                                          // subPercentage = double.Parse(Grp3Grade);
                                           MaxMark = 0;
                                           Mark1 = "0";
                                       }
                                       MaxMark = 0;
                                       subPercentage = 0;
                                   }
                                   else
                                   {
                                       //MaxMark = GetMaxToTal(_ClassId, ExamId, Dr_Subject[0].ToString(), Dr_Subject[2].ToString());
                                       if (Mark == "a" || Mark == "A")
                                       {
                                           Mark1 = "0";
                                       }
                                       else
                                       {
                                           Mark1 = Mark;
                                       }
                                       if (Dr_Exams[1].ToString().Contains("UNIT TEST"))
                                       {
                                           subPercentage = Math.Round((double.Parse(Mark1) * 0.2), 2);
                                       }
                                       else if (Dr_Exams[1].ToString().Contains("TERM EXAM"))
                                       {
                                           subPercentage = Math.Round((double.Parse(Mark1) * 0.3), 2);
                                       }
                                       else if (Dr_Exams[1].ToString().Contains("ANNUAL EXAM"))
                                       {
                                           subPercentage = Math.Round((double.Parse(Mark1) * 0.4), 2);
                                       }
                                       else
                                       {
                                           subPercentage = Math.Round(((double.Parse(Mark1) / MaxMark) * 100), 2);
                                       }
                                   }
                                    
                                    //subPercentage = Math.Round(((double.Parse(Mark1) / MaxMark) * 10), 2);
                                    CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">");
                                    CTR.Append("<table width=\"100%\">");
                                    CTR.Append("<tr>");
                                    if (subjectInTheExam(ClassExamId, Dr_Subject[0].ToString()))
                                    {
                                       
                                        if (int.Parse(Dr_Subject[2].ToString()) == 8)
                                        {
                                            CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">");
                                            CTR.Append("</td>");
                                            CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">" + Grp3Grade + "");
                                            CTR.Append("</td>");
                                        }
                                        else
                                        {
                                            CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">" + Mark + "");
                                            CTR.Append("</td>");
                                            CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">" + subPercentage + "");
                                            CTR.Append("</td>");
                                        }
                                    }
                                    else
                                    {
                                        Mark1 = "0";
                                        subPercentage = 0;
                                        CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">");
                                        CTR.Append("</td>");
                                        CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">");
                                        CTR.Append("</td>");
                                    }
                                    CTR.Append("</tr>");
                                    CTR.Append("</table>");
                                    CTR.Append("</td>");

                                    //ExamTotal = ExamTotal + double.Parse(Mark1);
                                    Maxtotal = Maxtotal + MaxMark;
                                    //submark[sub] = Mark;
                                    //sub++;
                                    TotMark = TotMark + double.Parse(Mark1);
                                    
                                    
                                    DataRow dr = dt.NewRow();
                                    if (int.Parse(Dr_Subject[2].ToString()) == 8)
                                    {
                                        dr["Marks" + count] = 0;
                                    }
                                    else
                                    {
                                        dr["Marks" + count] = Mark1;
                                    }
                                    dr["Percent" + count] = subPercentage;
                                    dt.Rows.Add(dr);
                                    
                                    
                                //}
                                
                            }
                            subjmarks.Tables.Add(dt);

                            ExamTotal = TotMark;
                            percentage = Math.Round(((ExamTotal / Maxtotal) * 100), 2);
                            //ExamTotal = GetExamTotal(Dr_Students[0].ToString(), Dr_Exams[0].ToString());
                            //percentage = Math.Round(((ExamTotal / Maxtotal) * 100), 2);
                            Grade = GetGrade(ExamTotal, Maxtotal, 0);
                            GetGradeRemark(int.Parse(Dr_Exams[0].ToString()),int.Parse(Dr_Students[0].ToString()),out Remark);
                            CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">" + ExamTotal + "");
                            CTR.Append("</td>");
                            CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">" + percentage + "");
                            CTR.Append("</td>");
                            CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">" + Grade + "");
                            CTR.Append("</td>");
                            //if (count == 0)
                            //{
                                CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">");
                                CTR.Append("</td>");
                                CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">");
                                CTR.Append("</td>");
                            //}
                            CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">" + Remark + "");
                            CTR.Append("</td>");
                            CTR.Append("</tr>");
                            allExamtotal = allExamtotal + ExamTotal;
                            allMaxTotal = allMaxTotal + Maxtotal;
                            allPercentage = Math.Round(((allExamtotal / allMaxTotal) * 100),2);
                           
                            count++;
                        }
                    }
                    allGrade = GetGrade(allExamtotal, allMaxTotal, 0);
                    //CTR.Append("<tr>");
                    CTR.Append("<td style=\" font-weight:bold; text-align:left; border: thin solid #000000\">TOTAL");
                    CTR.Append("</td>");
                    for (int j = 0; j <= subcount-1; j++)
                    {
                        Exsubmartotal = 0;
                        Exsubperctotal = 0;
                        for (int i = 0; i <= _examcount - 3; i++)
                        {
                            double Exsubmar = double.Parse((subjmarks.Tables[i].Rows[j][0]).ToString());
                            string ExSubPer = (subjmarks.Tables[i].Rows[j][1]).ToString();
                            if (ExSubPer == "∞")
                            {
                                Exsubperctotal = Exsubperctotal + 0;
                                
                            }
                            else
                            {
                                Exsubperctotal = Exsubperctotal + double.Parse(ExSubPer);
                            }
                            Exsubmartotal = Exsubmartotal + Exsubmar;
                            
                        }
                        CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">");
                        CTR.Append("<table width=\"100%\">");
                        CTR.Append("<tr>");                                      
                        CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">" + Exsubmartotal + "");
                        CTR.Append("</td>");
                        CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">" + Exsubperctotal + "");
                        CTR.Append("</td>");                        
                        CTR.Append("</tr>");
                        CTR.Append("</table>");
                        CTR.Append("</td>");
                       // ExTotSubMark[subcount] = Exsubmartotal;
                    }
                   
                   
                    CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">" + allExamtotal + "");
                    CTR.Append("</td>");
                    CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">" + allPercentage + "");
                    CTR.Append("</td>");
                    CTR.Append("<td style=\" font-weight:bold; text-align:center; border: thin solid #000000\">" + allGrade + "");
                    CTR.Append("</td>");
                    //CTR.Append("</tr>");

                 


                    SlNo++;
                    CTR.Append("</tr>");
                }


               
                CTR.Append("</table>");
            }
            return CTR.ToString();
        }
        private string GetGradeFromMarks(DataSet Grade, double _Mark, double MaxMark)
        {
            double avg = (_Mark / MaxMark) * 100;

            foreach (DataRow Dr in Grade.Tables[0].Rows)
            {
                if (avg >= double.Parse(Dr[1].ToString()))
                {
                    return Dr[0].ToString();
                }
            }
            return "";
        }

        private bool subjectInTheExam(int _classExamID,string _subId)
        {
            
            DataSet MyDataSet = null;
            string sql = "select tblclassexamsubmap.SubId from tblclassexamsubmap where tblclassexamsubmap.ClassExamId =" + _classExamID + "";
            MyDataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyReader.HasRows)
            {
                foreach (DataRow Dr_Subject in MyDataSet.Tables[0].Rows)
                {
                    if (_subId == Dr_Subject[0].ToString())
                    {
                        return true;
                    }
                    

                }
            }
            else
            {
                return false;
            }
            return false;
           
                
        }
        private double GetExamTotal(string _studId, string _examSchdId)
        {
            double _ExamToTal = 0;
            string sql = "select tblstudentmark.TotalMark from tblstudentmark where tblstudentmark.ExamSchId =" + _examSchdId + " and tblstudentmark.StudId = " + _studId;
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _ExamToTal = double.Parse(MyReader.GetValue(0).ToString());
               
            }
            return _ExamToTal;
        }
        public void GetStudentDetails(int StudentId, out string Studentname,out string Admitionno)
        {
            Studentname = ""; Admitionno = "";
            string sql = "select tblstudent.StudentName , tblstudent.AdmitionNo from tblstudent where tblstudent.`Status`=1 and tblstudent.Id=" + StudentId + "";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Studentname = MyReader.GetValue(0).ToString();
                Admitionno = MyReader.GetValue(1).ToString();
            }
        }
        public void GetGradeRemark(int ScheduleId, int _StudentId, out string remark)
        {

            string grade = ""; remark = "";
            string sql = "select tblstudentmark.Grade,tblstudentmark.Remark from tblstudentmark where tblstudentmark.ExamSchId = " + ScheduleId + "  and tblstudentmark.StudId=" + _StudentId + "";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
               // grade = MyReader.GetValue(0).ToString();
                remark = MyReader.GetValue(1).ToString();
            }

          
            
        }

        private DataSet GetSubjects(int _ClassId)
        {
            DataSet MyDataSet = null;
            // string sql = "select tblsubjects.Id , tblsubjects.subject_name from tblclasssubmap inner join  tblsubjects on tblsubjects.Id = tblclasssubmap.SubjectId where tblclasssubmap.ClassId="+_ClassId;
            string sql = "select distinct tblsubjects.Id , tblsubjects.subject_name,tblsubjects.sub_Catagory  from tblclassexamsubmap INNER join   tblsubjects on tblsubjects.Id = tblclassexamsubmap.SubId inner join tblclassexam on tblclassexam.Id =  tblclassexamsubmap.ClassExamId where tblclassexam.ClassId=" + _ClassId + "";
            MyDataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return MyDataSet;
        }
        private DataSet GetMainSubjects(int _ClassId)
        {
            DataSet MyDataSet = null;
            // string sql = "select tblsubjects.Id , tblsubjects.subject_name from tblclasssubmap inner join  tblsubjects on tblsubjects.Id = tblclasssubmap.SubjectId where tblclasssubmap.ClassId="+_ClassId;
            string sql = "select distinct tblsubjects.Id , tblsubjects.subject_name,tblsubjects.sub_Catagory  from tblclassexamsubmap INNER join   tblsubjects on tblsubjects.Id = tblclassexamsubmap.SubId inner join tblclassexam on tblclassexam.Id =  tblclassexamsubmap.ClassExamId where tblclassexam.ClassId=" + _ClassId + " and tblsubjects.sub_Catagory=1";
            MyDataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return MyDataSet;
        }
        private DataSet GetStudents(int _ClassId)
        {
            DataSet MyDataSet = null;
            string sql = "select tblstudent.Id , tblstudent.StudentName , tblstudent.AdmitionNo from tblstudentclassmap inner join tblstudent on tblstudent.Id = tblstudentclassmap.StudentId where tblstudent.`Status`=1 and tblstudentclassmap.ClassId=" + _ClassId + " ORDER BY tblstudent.StudentName";
            MyDataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return MyDataSet;
        }
        private DataSet GetExams(int _ClassId)
        {
            DataSet MyDataSet = null;
            //string sql = "select tblexamcombmap.Abbreviation,tblexamcombmap.SchExamId from tblexamcombmap where tblexamcombmap.CombinedId=" + _ExamId + " and tblexamcombmap.ClassId=" + _ClassId;
            string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period, tblexamschedule.GradeMasterId from tblsubject_type  inner join tblexammaster on tblexammaster.ExamTypeId=tblsubject_type.Id   inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id     where tblclassexam.ClassId=" + _ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and   tblexamschedule.Status='Completed' order by tblexammaster.ExamOrder";
            MyDataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return MyDataSet;
        }
        private int GetExamId(string _ExmScheduleId, int _ClassId)
        {
            int GetExamId = 0;
            OdbcDataReader MyTempReader = null;
            string sql = "select distinct ExamId FROM tblclassexam where tblclassexam.Id = (select distinct tblexamschedule.ClassExamId from tblexamschedule where Id=" + _ExmScheduleId + " ) and tblclassexam.ClassId =" + _ClassId;
            MyTempReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyTempReader.HasRows)
            {
                int.TryParse(MyTempReader.GetValue(0).ToString(), out GetExamId);
            }
            return GetExamId;
        }
        private int GetClassExamId(string _ExmScheduleId)
        {
            int GetClassExamId = 0;
            OdbcDataReader MyTempReader = null;
            string sql = "select distinct tblexamschedule.ClassExamId from tblexamschedule where Id=" + _ExmScheduleId + "";
            MyTempReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyTempReader.HasRows)
            {
                int.TryParse(MyTempReader.GetValue(0).ToString(), out GetClassExamId);
            }
            return GetClassExamId;
        }
        private string GetSubjectMarks(string ExmSchId, string _SubId, string _StudentId)
        {
            string _MarkColumn = GetMarkColumn(ExmSchId, _SubId);
            string _Mark = GetMark(_MarkColumn, ExmSchId, _StudentId);
            return _Mark;
        }
        private string GetMarkColumn(string _ExmSchId, string _SubId)
        {
            string Column = "Mark1";
            string sql = "select tblexammark.MarkColumn from tblexammark where tblexammark.ExamSchId=" + _ExmSchId + " and tblexammark.SubjectId=" + _SubId + " order by tblexammark.SubjectOrder";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Column = MyReader.GetValue(0).ToString();
            }
            return Column;
        }
        private string GetMark(string _MarkColumn, string _ExmSchId, string _StudentId)
        {
            string _Mark = "0";
            string sql = "select " + _MarkColumn + "  from tblstudentmark where ExamSchId=" + _ExmSchId + " and StudId=" + _StudentId;
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _Mark = MyReader.GetValue(0).ToString();
                if (_Mark == "-1")
                    _Mark = "a";
                if (_Mark == "")
                    _Mark = "0";

            }
            return _Mark;
        }
        private double GetMaxToTal(int _ClassId, int _ExamId, string _SubjectId)
        {
            double _MaxToTal = 0;
           
              
                string sql = "select tblclassexamsubmap.MaxMark from tblclassexamsubmap inner join tblclassexam on tblclassexam.Id = tblclassexamsubmap.ClassExamId where tblclassexamsubmap.SubId=" + _SubjectId + " and tblclassexam.ClassId=" + _ClassId + " and tblclassexam.ExamId=" + _ExamId;
                MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    double.TryParse(MyReader.GetValue(0).ToString(), out _MaxToTal);
                }
           
            return _MaxToTal;
        }
        public string GetGrade(double _Total, double _Maxtotal, int _GradeMasterId)
        {
            string _StudGrade = "";
            double _Avg = 0;
            if (_Maxtotal > 0)
            {
                _Avg = (_Total * 100) / _Maxtotal;
            }

            string sqlstr = "SELECT Grade,LowerLimit,Result FROM tblgrade WHERE `Status`=1 AND GradeMasterId=" + _GradeMasterId + " ORDER BY LowerLimit DESC";
            OdbcDataReader m_MyReader1 = MyExamMang.m_MysqlDb.ExecuteQuery(sqlstr);
            if (m_MyReader1.HasRows)
            {
                while (m_MyReader1.Read())
                {
                    string _Grade = m_MyReader1.GetValue(0).ToString();
                    double _Lowerlimit = 0;
                    double.TryParse(m_MyReader1.GetValue(1).ToString(), out _Lowerlimit);
                    string t_Result = m_MyReader1.GetValue(2).ToString();
                    if (_Avg >= _Lowerlimit)
                    {
                        _StudGrade = _Grade;
                        break;
                    }
                }
            }
            return _StudGrade;
        }
        //private string GtSchoolLogo()
        //{

        //    String ImageUrl = "";


        //    ImageUploaderClass imgobj = new ImageUploaderClass(objSchool);
        //    byte[] img_bytes = imgobj.getImageBytes(objSchool.SchoolId, "Logo");
        //    M_Logo = MyUser.FilePath + "/ThumbnailImages/" + objSchool.SchoolId + "_" + System.DateTime.Now.Millisecond + ".jpg";


        //    File.WriteAllBytes(M_Logo, img_bytes);
        //    ImageUrl = M_Logo;
        //    return ImageUrl;



        //}
        private void GetAttendanceDetails(int studentId, int classId, out int noWorkingDays, out int _presentdays)
        {
            string standard = "";          
            standard = MyAttendance.GetStandard_Class(classId);
            string _sdate="", _enddate="";
            int _workingdays, _absentdays, halfdays;
            double noAbsentDays = 0;
            DateTime _Start, _End;
            MyAttendance.GetbatchDates(MyUser.CurrentBatchId, out _Start, out _End);
            DateTime _date = System.DateTime.Now;
            _enddate = _date.ToString("yyyy-MM-dd");
            _sdate = _Start.Date.ToString("yyyy-MM-dd");
            noWorkingDays = MyAttendance.GetWorkingDaysForThePeriod_New(studentId, standard, MyUser.CurrentBatchId, _sdate, _enddate);
            noAbsentDays = MyAttendance.GetNoOf_AbsentDayForTheperiod_New(studentId, standard, MyUser.CurrentBatchId, _sdate, _enddate);
            _presentdays = MyAttendance.GetNoOf_FullDayForTheperiod_New(studentId, standard, MyUser.CurrentBatchId, _sdate, _enddate);
        }
        # endregion

    }

}


