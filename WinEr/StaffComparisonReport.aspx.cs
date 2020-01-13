using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;
using WebChart;
using System.Drawing;
using System.Text;

namespace WinEr
{
    public partial class StaffComparisonReport : System.Web.UI.Page
    {
        private StaffManager MyStaffMang;
        private KnowinUser MyUser;
        private ClassOrganiser MyClassManage;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet = null;

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];

            if (MyUser.SELECTEDMODE == 1)
            {
                this.MasterPageFile = "~/WinerStudentMaster.master";

            }
            else if (MyUser.SELECTEDMODE == 2)
            {

                this.MasterPageFile = "~/WinerSchoolMaster.master";
            }

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStaffMang = MyUser.GetStaffObj();
            MyClassManage = MyUser.GetClassObj();
            if (MyStaffMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (MyClassManage == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(752))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadAllStandardsToDropDown();
                    LoadExamsForSTheSelectedStandard();
                    LoadExamPeriodsToDropDown();
                    Clear();
                    if (int.Parse(Drp_Exam.SelectedValue) > -1 && int.Parse(Drp_Standard.SelectedValue) > -1 && int.Parse(Drp_Period.SelectedValue) > -1)
                    {
                        LoadStaffPerformanceDetails();
                    }
                }
            }
        }

        private void LoadStaffPerformanceDetails()
        {
            string _ClassTable = "", SqlClass = "", _SqlSubjects = "", _SubjectTable = "", _TableString = "";
           // OdbcDataReader MyReaderSubjects = null;
            OdbcDataReader MyReaderStaff = null;
            OdbcDataReader MyReaderClass = null;
            StringBuilder CTR = new StringBuilder();

            SqlClass = "select tblclass.Id, tblclass.ClassName from tblclass where tblclass.Status=1 and tblclass.Standard=" + Drp_Standard.SelectedValue;
            MyReaderClass = MyStaffMang.m_MysqlDb.ExecuteQuery(SqlClass);
            if (MyReaderClass.HasRows)
            {
                _SubjectTable = GetSubjectTable();

                CTR.Append("<table runat=\"server\" width=\"100%\" style=\"border: thin solid #000000\">");
                CTR.Append("<tr>");
                CTR.Append("<td valign=\"bottom\">" + _SubjectTable + "</td>");
                int i = 0;
                while (MyReaderClass.Read())
                {
                    _ClassTable = GetClassTableString(int.Parse(MyReaderClass.GetValue(0).ToString()), MyReaderClass.GetValue(1).ToString());
                   // _ClassName[i, 0] = MyReaderClass.GetValue(0).ToString();
                  //  _ClassName[i, 1] = MyReaderClass.GetValue(1).ToString();

                   // _SubjectTable = GetSubjectTableString(int.Parse(MyReaderSubjects.GetValue(0).ToString()), MyReaderSubjects.GetValue(1).ToString(), MyReaderSubjects.GetValue(2).ToString(), ref _StaffPerformance, i);
                    i++;
                    CTR.Append("<td>" + _ClassTable + "</td>");
                }
                CTR.Append("</tr>");
                CTR.Append("</table>");

                _TableString = CTR.ToString();

                if (_TableString != "")
                {
                    this.MarkListArea.Visible = true;
                    this.ExamReport.Visible = true;
                    //Pnl_PerformanceGraph.Visible = true;
                   // this.StaffList.Visible = true;

                    this.ExamReport.InnerHtml = _TableString;
                    ViewState["ExcelData"] = _TableString;
                   // Session["GraphDetail"] = _StaffPerformanceNew;
                }
                else
                {
                    Clear();
                }
            }
            else
            {
                Clear();
            }
        }

        private string GetClassTableString(int _ClassId, string _ClassName)
        {
            OdbcDataReader MyReaderMarkDetails = null;
            string _SubjectTable = "";

            StringBuilder CTR = new StringBuilder();
            string SqlClass = "select distinct( tblclassexamsubmap.SubId), tblexammark.MarkColumn, tblclassexamsubmap.MinMark, tblclassexamsubmap.MaxMark from tblclassexamsubmap inner join tblexammark on tblclassexamsubmap.SubId= tblexammark.SubjectId where tblclassexamsubmap.ClassExamId in( select tblclassexam.Id from tblclassexam where tblclassexam.ExamId=" + Drp_Exam.SelectedValue + " and  tblclassexam.ClassId in( select tblclass.Id from tblclass where tblclass.Standard=" + Drp_Standard.SelectedValue + " and tblclass.Status=1)) order by tblclassexamsubmap.SubId";
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(SqlClass);
            if (MyReader.HasRows)
            {
                Hdn_ColumnCount.Value = MyReader.RecordsAffected.ToString();

                CTR.Append("<table runat=\"server\" width=\"100%\" style=\"border: thin solid #000000\">");

                CTR.Append("<tr><td colspan=\"10\" class=\"TableHeaderStyle\" align=\"center\"><b>" + _ClassName + "</b></td></tr>");
                CTR.Append("<tr> <td class=\"SubHeaderStyle\"><b>Staff</b></td> <td class=\"SubHeaderStyle\"><b>Total No</b></td> <td class=\"SubHeaderStyle\"><b>Class Average(%)</b></td> <td class=\"SubHeaderStyle\"><b>No.Passed</b></td> <td class=\"SubHeaderStyle\"><b>No.Failed</b></td>");
                CTR.Append("<td class=\"SubHeaderStyle\"><b>0-20%</b></td> <td class=\"SubHeaderStyle\"><b>20-40%</b></td> <td class=\"SubHeaderStyle\"><b>40-60%</b></td> <td class=\"SubHeaderStyle\"><b>60-80%</b></td> <td class=\"SubHeaderStyle\"><b>80-100%</b></td> </tr>");

                while (MyReader.Read())
                {
                    if (SubjectPresentForSelectedExam(int.Parse(MyReader.GetValue(0).ToString()), _ClassId))
                    {                       

                        string _MarkColumn = MyReader.GetValue(1).ToString();
                        if (_MarkColumn != "")
                        {
                            double _MinMark = 0, _MaxMark = 0, _TotalMark = 0, _ClassAverage = 0;
                            int _Limit1 = 0, _TotalSeats = 0, _Limit2 = 0, _Limit3 = 0, _Limit4 = 0, _Limit5 = 0, _Passed = 0, _Failed = 0;

                            double.TryParse(MyReader.GetValue(2).ToString(), out _MinMark);
                            double.TryParse(MyReader.GetValue(3).ToString(), out _MaxMark);

                            int _ExamScheduleId = GetExamScheduleId(_ClassId, MyUser.CurrentBatchId, int.Parse(Drp_Period.SelectedValue), int.Parse(Drp_Exam.SelectedValue));

                            _TotalSeats = MyClassManage.GetTotalNumberOfStudentsInClass(_ClassId, MyUser.CurrentBatchId);

                            string _StaffName = MyStaffMang.GetStaffNameForSubject(_ClassId, int.Parse(MyReader.GetValue(0).ToString()));

                            string sqlMarkDetails = "select " + _MarkColumn + " from tblstudentmark where tblstudentmark.ExamSchId=" + _ExamScheduleId + " and " + _MarkColumn + "!=\"\" and tblstudentmark.StudId in (select tblstudentclassmap.StudentId from tblstudentclassmap inner join tblstudent on tblstudentclassmap.StudentId= tblstudent.Id and tblstudent.Status=1 where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " and tblstudentclassmap.ClassId=" + _ClassId + ")";
                            MyReaderMarkDetails = MyStaffMang.m_MysqlDb.ExecuteQuery(sqlMarkDetails);
                            if (MyReaderMarkDetails.HasRows)
                            {
                                while (MyReaderMarkDetails.Read())
                                {
                                    double _SubjectMark = 0, _SubjectPercentage = 0;
                                    if (MyReaderMarkDetails.GetValue(0).ToString() != "")
                                    {
                                        double.TryParse(MyReaderMarkDetails.GetValue(0).ToString(), out _SubjectMark);
                                        _TotalMark = _TotalMark + _SubjectMark;

                                        if (_SubjectMark > _MinMark)
                                        {
                                            _Passed++;
                                        }
                                        _SubjectPercentage = (_SubjectMark / _MaxMark) * 100;

                                        if (_SubjectPercentage > 0 && _SubjectPercentage <= 20)
                                        {
                                            _Limit1++;
                                        }
                                        else if (_SubjectPercentage > 20 && _SubjectPercentage <= 40)
                                        {
                                            _Limit2++;
                                        }
                                        else if (_SubjectPercentage > 40 && _SubjectPercentage <= 60)
                                        {
                                            _Limit3++;
                                        }
                                        else if (_SubjectPercentage > 60 && _SubjectPercentage <= 80)
                                        {
                                            _Limit4++;
                                        }
                                        else
                                        {
                                            _Limit5++;
                                        }
                                    }
                                }
                                if (_Limit1 != (_TotalSeats - (_Limit2 + _Limit3 + _Limit4 + _Limit5)))
                                {
                                    _Limit1 = _Limit1 + (_TotalSeats - (_Limit1 + _Limit2 + _Limit3 + _Limit4 + _Limit5));
                                }
                                _ClassAverage = Math.Round((_TotalMark / (_MaxMark * _TotalSeats)) * 100, 2);
                                _Failed = _TotalSeats - _Passed;

                                CTR.Append("<tr> <td class=\"CellStyle1\" >" + _StaffName + "</td> <td class=\"CellStyle\">" + _TotalSeats + "</td> <td class=\"CellStyle\">" + _ClassAverage + "</td> <td class=\"CellStyle\">" + _Passed + "</td> <td class=\"CellStyle\">" + _Failed + "</td>");
                                CTR.Append("<td class=\"CellStyle\">" + _Limit1 + "</td> <td class=\"CellStyle\">" + _Limit2 + "</td> <td class=\"CellStyle\">" + _Limit3 + "</td> <td class=\"CellStyle\">" + _Limit4 + "</td> <td class=\"CellStyle\">" + _Limit5 + "</td> </tr>");
                            }
                            else
                            {
                                CTR.Append("<tr> <td class=\"CellStyle1\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td>");
                                CTR.Append("<td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> </tr>");
                            }
                        }
                        else
                        {
                            CTR.Append("<tr> <td class=\"CellStyle1\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> ");
                            CTR.Append("<td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> </tr>");
                        }
                    }
                    else
                    {
                        CTR.Append("<tr> <td class=\"CellStyle1\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> ");
                        CTR.Append("<td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> </tr>");
                    }
                }
                CTR.Append("</table>");
            }
        _SubjectTable = CTR.ToString();
        return _SubjectTable;
        }

        private bool SubjectPresentForSelectedExam(int _SubjectId,int _ClassId)
        {
            OdbcDataReader m_MyReader = null;
            bool _Present = false;
            string Sql = "select tblclassexamsubmap.SubId from tblclassexamsubmap where tblclassexamsubmap.SubId=" + _SubjectId + " and  tblclassexamsubmap.ClassExamId =(select tblclassexam.Id from tblclassexam where tblclassexam.ClassId=" + _ClassId + " and tblclassexam.ExamId=" + Drp_Exam.SelectedValue + ")";
            m_MyReader =MyStaffMang.m_MysqlDb.ExecuteQuery(Sql);
            if (m_MyReader.HasRows)
            {
                _Present = true;
            }
            return _Present;
        }

        private int GetExamScheduleId(int _ClassId, int _BatchId, int _PeriodId, int _ExamId)
        {
            int _ExamScheduleId = 0;
            OdbcDataReader MyReaderExam = null;
            string sql = "select distinct(tblexamschedule.Id) from tblexamschedule inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId  and tblclassexam.ExamId=" + _ExamId + "  where tblclassexam.ClassId=" + _ClassId + " and tblexamschedule.BatchId=" + _BatchId + " and tblexamschedule.PeriodId=" + _PeriodId;
            MyReaderExam = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReaderExam.HasRows)
            {
                _ExamScheduleId = int.Parse(MyReaderExam.GetValue(0).ToString());
            }
            return _ExamScheduleId;
        }

        private string GetSubjectTable()
        {
            StringBuilder CTR = new StringBuilder();

            CTR.Append("<table runat=\"server\" width=\"100%\" style=\"border: thin solid #000000\">");

            CTR.Append("<tr><td class=\"TableHeaderStyle\"><b>Subject</b></td></tr>");
            CTR.Append("<tr><td class=\"SubHeaderStyle1\">.</td></tr>");

            string _ClassTable = "";
            string SqlSubjects = "select distinct(tblclassexamsubmap.SubId), tblsubjects.subject_name from  tblclassexamsubmap inner join tblsubjects on tblclassexamsubmap.SubId= tblsubjects.Id where tblclassexamsubmap.ClassExamId in(select tblclassexam.Id from tblclassexam where  tblclassexam.ExamId=" + Drp_Exam.SelectedValue + " and tblclassexam.ClassId in ( select tblclass.Id from tblclass where tblclass.Standard=" + Drp_Standard.SelectedValue + ")) order by tblclassexamsubmap.SubId";
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(SqlSubjects);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    CTR.Append("<tr><td class=\"CellStyle1\"><b>" + MyReader.GetValue(1).ToString() + "</b></td></tr>");
                }
            }
            CTR.Append("</table>");
            _ClassTable = CTR.ToString();
            return _ClassTable;
        }

        private void Clear()
        {
            this.MarkListArea.Visible = false;
            this.ExamReport.Visible = false;
            Pnl_PerformanceGraph.Visible = false;
            this.StaffList.Visible = false;
        }

        private void LoadExamPeriodsToDropDown()
        {
            Drp_Period.Items.Clear();

            string sql = "select distinct(tblperiod.Id), tblperiod.Period from tblperiod inner join tblexamschedule on tblperiod.Id= tblexamschedule.PeriodId where tblexamschedule.BatchId="+MyUser.CurrentBatchId+" and tblexamschedule.ClassExamId in(select tblclassexam.Id from tblclassexam where tblclassexam.ExamId="+Drp_Exam.SelectedValue+" and tblclassexam.ClassId in(select tblclass.Id from tblclass where tblclass.Status=1 and tblclass.Standard=" + Drp_Standard.SelectedValue + "))";
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_Show.Enabled = true;
                Lbl_indexammsg.Text = "";
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Period.Items.Add(li);
                }
            }
            else
            {
                Btn_Show.Enabled = false;
                ListItem li = new ListItem("No Periods present", "-1");
                Drp_Period.Items.Add(li);
                Lbl_indexammsg.Text = "No Periods found";
            }
        }

        private void LoadExamsForSTheSelectedStandard()
        {
            Drp_Exam.Items.Clear();

            string sql = "select distinct( tblexammaster.Id), tblexammaster.ExamName FROM tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster on tblclassexam.ExamId= tblexammaster.Id where tblclassexam.Status=1 and tblclassexam.ClassId in( select tblclass.Id from tblclass where tblclass.Status=1 and tblclass.Standard="+Drp_Standard.SelectedValue+") and tblexamschedule.BatchId=" + MyUser.CurrentBatchId;
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_Show.Enabled = true;
                Lbl_indexammsg.Text = "";
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Exam.Items.Add(li);
                }
            }
            else
            {
                Btn_Show.Enabled = false;
                ListItem li = new ListItem("No Exams present", "-1");
                Drp_Exam.Items.Add(li);
                Lbl_indexammsg.Text = "No Exams found";

                Drp_Period.Items.Clear();
                li = new ListItem("No Periods present", "-1");
                Drp_Period.Items.Add(li);
            }
        }

        private void LoadAllStandardsToDropDown()
        {
            Drp_Standard.Items.Clear();

            string sql = "select DISTINCT( tblclass.Standard), tblstandard.Name from tblclass inner join tblstandard on tblclass.Standard= tblstandard.Id ORDER by tblstandard.Id";
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_Show.Enabled = true;
                Lbl_indexammsg.Text = "";
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Standard.Items.Add(li);
                }
            }
            else
            {
                Btn_Show.Enabled = false;
                ListItem li = new ListItem("No Standard present", "-1");
                Drp_Standard.Items.Add(li);
                Lbl_indexammsg.Text = "No Standard found";
            }
        }

        protected void Drp_Standard_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear();
            LoadExamsForSTheSelectedStandard();
            LoadExamPeriodsToDropDown();
        }

        protected void Drp_Exam_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear();
            LoadExamPeriodsToDropDown();
        }

        protected void Btn_Show_Click(object sender, EventArgs e)
        {
            LoadStaffPerformanceDetails();
        }

        protected void Img_Export_Click(object sender, ImageClickEventArgs e)
        {
            StringBuilder CTR = new StringBuilder();
            string _WorkSheetName = "StaffComparison";
            string FileName = "Standard-wiseStaffAnalysis";
            string _TableString = (string)ViewState["ExcelData"];

            CTR.Append("<table runat=\"server\" width=\"100%\" style=\"border: thin solid #000000\">");
            CTR.Append("<tr><td align=\"center\"><b> Standard : " + Drp_Standard.SelectedItem.Text.ToUpper() + "</b></td></tr>");
            CTR.Append("<tr><td align=\"center\"><b>" + Drp_Exam.SelectedItem.Text.ToUpper() + " (" + Drp_Period.SelectedItem.Text + ")" + "</b></td></tr>");

            CTR.Append("<tr><td>" + _TableString);
            CTR.Append("</td></tr></table>");
            string _TableStringFull = CTR.ToString();

            if (!WinEr.ExcelUtility.ExportBuiltStringToExcel(FileName, _TableStringFull, _WorkSheetName))
            {
                WC_MessageBox.ShowMssage("This function need Ms office");
            }
        }
    }
}
