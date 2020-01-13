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
    public struct StaffArray
    {
        public string ClassName;
        public string Mark;
    }

    public partial class TeacherwisePerformance : System.Web.UI.Page
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
            else if (!MyUser.HaveActionRignt(751))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadAllStaffsToDropDown();
                    LoadExamsForSubjectsTheSelectedStaffTaken();
                    LoadExamPeriodsToDropDown();
                    Clear();
                    if (int.Parse(Drp_Exam.SelectedValue) > -1 && int.Parse(Drp_Staff.SelectedValue) > -1 && int.Parse(Drp_Period.SelectedValue) > -1)
                    {
                        LoadStaffPerformanceDetails();
                    }
                }
            }
        }
       

        private void LoadStaffPerformanceDetails()
        {
            string _ClassTable = "", SqlClass = "", _SqlSubjects = "",_SubjectTable="";
            OdbcDataReader MyReaderSubjects = null;
            OdbcDataReader MyReaderStaff = null;
            StringBuilder CTR = new StringBuilder();
            
            _SqlSubjects = "select distinct(tblclassstaffmap.SubjectId), tblsubjects.subject_name, tblexammark.MarkColumn from tblclassstaffmap inner join tblsubjects on tblclassstaffmap.SubjectId= tblsubjects.Id inner join tblexammark on tblclassstaffmap.SubjectId= tblexammark.SubjectId where tblclassstaffmap.StaffId=" + Drp_Staff.SelectedValue + " and tblclassstaffmap.ClassId in(select tblclassexam.ClassId from tblclassexam inner join tblexammaster on tblclassexam.ExamId= tblexammaster.Id where tblclassexam.ExamId=" + Drp_Exam.SelectedValue + " and tblexammaster.Status=1) and tblclassstaffmap.SubjectId in (select distinct(tblclassexamsubmap.SubId) from tblclassexamsubmap where tblclassexamsubmap.ClassExamId in (select tblclassexam.Id from tblclassexam where tblclassexam.ExamId=" + Drp_Exam.SelectedValue + ")) order by tblclassstaffmap.SubjectId";
            MyReaderSubjects = MyStaffMang.m_MysqlDb.ExecuteQuery(_SqlSubjects);
            if (MyReaderSubjects.HasRows)
            {
                string[,] _SubjectIndex = new string[MyReaderSubjects.RecordsAffected, 2];

                StaffArray[,] _StaffPerformance = new StaffArray[MyReaderSubjects.RecordsAffected, 20];

                _ClassTable = GetClassTableString();
               

                CTR.Append("<table runat=\"server\" width=\"100%\" style=\"border: thin solid #000000\">");
                CTR.Append("<tr>");
                CTR.Append("<td valign=\"bottom\">" + _ClassTable + "</td>");
                int i = 0;
                while (MyReaderSubjects.Read())
                {
                    _SubjectIndex[i, 0] = i.ToString();
                    _SubjectIndex[i, 1] = MyReaderSubjects.GetValue(1).ToString();                    

                    _SubjectTable = GetSubjectTableString(int.Parse(MyReaderSubjects.GetValue(0).ToString()), MyReaderSubjects.GetValue(1).ToString(), MyReaderSubjects.GetValue(2).ToString(), ref _StaffPerformance,i);
                    i++;
                    CTR.Append("<td>" + _SubjectTable + "</td>");
                }
                CTR.Append("</tr>");
                CTR.Append("</table>");               

                StaffArray[,] _StaffPerformanceNew = new StaffArray[MyReaderSubjects.RecordsAffected, int.Parse(Hdn_ColumnCount.Value)];
                for (int a = 0; a < MyReaderSubjects.RecordsAffected; a++)
                {
                    for (int b = 0; b < int.Parse(Hdn_ColumnCount.Value); b++)
                    {
                        _StaffPerformanceNew[a, b].ClassName = _StaffPerformance[a, b].ClassName;
                        _StaffPerformanceNew[a, b].Mark = _StaffPerformance[a, b].Mark;
                    }
                }
                
                string _TableString = CTR.ToString();

                if (_TableString != "")
                {
                    Lbl_indexammsg.Text = "";
                    this.MarkListArea.Visible = true;
                    this.ExamReport.Visible = true;
                    Pnl_PerformanceGraph.Visible = true;
                    this.StaffList.Visible = true;

                    this.ExamReport.InnerHtml = _TableString;
                    ViewState["ExcelData"] = _TableString;
                    Session["GraphDetail"] = _StaffPerformanceNew;
                    LoadConditionDropDownWithSubject(_SubjectIndex);

                    LoadPerformanceGraphWithExamData(_StaffPerformanceNew);
                }
                else
                {
                    Clear();
                    Lbl_indexammsg.Text = "No Report exists";
                }
            }           
            
            else
            {
                Clear();
                Lbl_indexammsg.Text = "No Report exists";
            }
        }

        private void LoadPerformanceGraphWithExamData(StaffArray[,] _StaffPerformanceNew)
        {
            int _SelectedCondition = int.Parse(Drp_SelectSubject.SelectedValue);
            int _RowCount = _StaffPerformanceNew.GetLength(0);
            int _ColumnCount = _StaffPerformanceNew.GetLength(1);
                  

            float MaxVal = 100;
            float _val = 0;
            if (_RowCount > 0)
            {
                ColumnChart chart_Bar = new ColumnChart();

                Chart chart_Line = new SmoothLineChart();

                ChartPointCollection chart_Line_data = chart_Line.Data;

                for (int i = 0; i < _ColumnCount; i++)
                {
                    if (float.TryParse(_StaffPerformanceNew[_SelectedCondition, i].Mark, out _val))
                    {
                        //DataColumn dc = _ExamArray[_SelectedCondition, i].ExamName;
                        string ColumnName = _StaffPerformanceNew[_SelectedCondition, i].ClassName;
                        if (MaxVal < _val)
                        {
                            MaxVal = _val;
                        }
                        chart_Line_data.Add(new ChartPoint(ColumnName, _val));
                        chart_Bar.Data.Add(new ChartPoint(ColumnName, _val));
                    }
                }

                chart_Line.Line.Width = 2;
                chart_Line.Line.Color = Color.RoyalBlue;
                chart_Line.Legend = Drp_SelectSubject.SelectedValue;
                chartcontrol_ExamChart.Charts.Clear();
                chartcontrol_ExamChart.Charts.Add(chart_Line);

                //chart_Line.DataLabels.Visible = true;

                chartcontrol_ExamChart.YTitle.StringFormat.FormatFlags = StringFormatFlags.DirectionVertical;
                chartcontrol_ExamChart.YTitle.StringFormat.Alignment = StringAlignment.Near;


                if (MaxVal != 100)
                {
                    MaxVal = MaxVal + 10;
                }
                chart_Bar.Shadow.Visible = true;
                chart_Bar.DataLabels.Visible = true;
                chart_Bar.MaxColumnWidth = 20;
                chartcontrol_ExamChart.YCustomEnd = MaxVal;
                chartcontrol_ExamChart.Charts.Add(chart_Bar);
                chart_Bar.Fill.Color = System.Drawing.Color.RoyalBlue;
                chartcontrol_ExamChart.Background.Color = Color.White;
                chartcontrol_ExamChart.RedrawChart();
            }

        }

        private void LoadConditionDropDownWithSubject(string[,] _SubjectIndex)
        {
            Drp_SelectSubject.Items.Clear();

            int _Subject = _SubjectIndex.GetLength(0);
            int i = 0;
            if (_Subject > 0)
            {
                while (i < _Subject)
                {
                    ListItem li = new ListItem(_SubjectIndex[i, 1], _SubjectIndex[i, 0]);
                    Drp_SelectSubject.Items.Add(li);
                    i++;
                }
            }
            else
            {
                Drp_SelectSubject.Items.Add(new ListItem("No Conditions found", "-1"));
            }
        }

        private string GetSubjectTableString(int _SubjectId, string _SubjectName, string _MarkColumn, ref  StaffArray[,]  _StaffPerformance,int _RowCount)
        {
            OdbcDataReader MyReaderMarkDetails = null;
            string _SubjectTable = "";
            
            StringBuilder CTR = new StringBuilder();
            if (_MarkColumn != "")
            {
                string SqlClass = "select distinct( tblclassstaffmap.ClassId), tblclass.ClassName from tblclassstaffmap inner join tblclass on tblclassstaffmap.ClassId = tblclass.Id where tblclassstaffmap.StaffId=" + Drp_Staff.SelectedValue + " and tblclassstaffmap.ClassId in(select tblclassexam.ClassId from tblclassexam inner join tblexammaster on tblclassexam.ExamId= tblexammaster.Id where tblclassexam.ExamId=" + Drp_Exam.SelectedValue + " and tblexammaster.Status=1) and tblclassstaffmap.SubjectId in (select distinct(tblclassexamsubmap.SubId) from tblclassexamsubmap where tblclassexamsubmap.ClassExamId in (select tblclassexam.Id from tblclassexam where tblclassexam.ExamId=" + Drp_Exam.SelectedValue + ")) order by tblclassstaffmap.ClassId";
                MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(SqlClass);
                if (MyReader.HasRows)
                {
                    Hdn_ColumnCount.Value = MyReader.RecordsAffected.ToString();

                    CTR.Append("<table runat=\"server\" width=\"100%\" style=\"border: thin solid #000000\">");

                    CTR.Append("<tr><td colspan=\"9\" class=\"TableHeaderStyle\" align=\"center\"><b>" + _SubjectName + "</b></td></tr>");
                    CTR.Append("<tr><td class=\"SubHeaderStyle\"><b>Total No</b></td> <td class=\"SubHeaderStyle\"><b>Class Average(%)</b></td> <td class=\"SubHeaderStyle\"><b>No.Passed</b></td> <td class=\"SubHeaderStyle\"><b>No.Failed</b></td>");
                    CTR.Append("<td class=\"SubHeaderStyle\"><b>0-20%</b></td> <td class=\"SubHeaderStyle\"><b>20-40%</b></td> <td class=\"SubHeaderStyle\"><b>40-60%</b></td> <td class=\"SubHeaderStyle\"><b>60-80%</b></td> <td class=\"SubHeaderStyle\"><b>80-100%</b></td> </tr>");
                    int _Column = 0;

                    while (MyReader.Read())
                    {
                        if (IsSubjectTakenBySelectedStaffForSelectedClass(int.Parse(MyReader.GetValue(0).ToString()), _SubjectId))
                        {
                            double _MinMark = 0, _MaxMark = 0, _TotalMark = 0, _ClassAverage = 0;
                            int _Limit1 = 0, _TotalSeats = 0, _Limit2 = 0, _Limit3 = 0, _Limit4 = 0, _Limit5 = 0, _Passed = 0, _Failed = 0;

                            int _ExamScheduleId = GetExamScheduleId(int.Parse(MyReader.GetValue(0).ToString()), MyUser.CurrentBatchId, int.Parse(Drp_Period.SelectedValue), int.Parse(Drp_Exam.SelectedValue));

                            _TotalSeats = MyClassManage.GetTotalNumberOfStudentsInClass(int.Parse(MyReader.GetValue(0).ToString()), MyUser.CurrentBatchId);

                            GetMinAndMaxMarkForSubjectForSelectedExam(int.Parse(MyReader.GetValue(0).ToString()), int.Parse(Drp_Exam.SelectedValue), _SubjectId, out _MinMark, out _MaxMark);



                            string sqlMarkDetails = "select " + _MarkColumn + " from tblstudentmark where tblstudentmark.ExamSchId=" + _ExamScheduleId + " and tblstudentmark.StudId in (select tblstudentclassmap.StudentId from tblstudentclassmap inner join tblstudent on tblstudentclassmap.StudentId= tblstudent.Id and tblstudent.Status=1 where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " and tblstudentclassmap.ClassId=" + int.Parse(MyReader.GetValue(0).ToString()) + ")";
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
                            }
                            //}
                            _StaffPerformance[_RowCount, _Column].ClassName = MyReader.GetValue(1).ToString();
                            _StaffPerformance[_RowCount, _Column].Mark = _ClassAverage.ToString();

                            CTR.Append("<tr><td class=\"CellStyle\">" + _TotalSeats + "</td> <td class=\"CellStyle\">" + _ClassAverage + "</td> <td class=\"CellStyle\">" + _Passed + "</td> <td class=\"CellStyle\">" + _Failed + "</td>");
                            CTR.Append("<td class=\"CellStyle\">" + _Limit1 + "</td> <td class=\"CellStyle\">" + _Limit2 + "</td> <td class=\"CellStyle\">" + _Limit3 + "</td> <td class=\"CellStyle\">" + _Limit4 + "</td> <td class=\"CellStyle\">" + _Limit5 + "</td> </tr>");
                        }
                        else
                        {
                            _StaffPerformance[_RowCount, _Column].ClassName = MyReader.GetValue(1).ToString();
                            _StaffPerformance[_RowCount, _Column].Mark = "--";

                            CTR.Append("<tr><td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td>");
                            CTR.Append("<td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> <td class=\"CellStyle\">--</td> </tr>");
                        }
                        _Column++;
                    }
                    CTR.Append("</table>");
                }
            }           
            _SubjectTable = CTR.ToString();
            return _SubjectTable;
        }

        private bool IsSubjectTakenBySelectedStaffForSelectedClass(int _ClassId, int _SubjectId)
        {
            bool _Valid = false;
            OdbcDataReader MyReader1 = null;
            string sql = "select tblclassstaffmap.StaffId from tblclassstaffmap where tblclassstaffmap.ClassId="+_ClassId+" and tblclassstaffmap.SubjectId=" + _SubjectId;
            MyReader1= MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader1.HasRows)
            {
                while (MyReader1.Read())
                {
                    if (int.Parse(MyReader1.GetValue(0).ToString()) == int.Parse(Drp_Staff.SelectedValue))
                    {
                        _Valid = true;
                    }
                }
            }
            return _Valid;
        }

        private void GetMinAndMaxMarkForSubjectForSelectedExam(int _ClassId, int _ExamId, int _SubjectId, out double _MinMark, out double _MaxMark)
        {
            OdbcDataReader MyReaderMark = null;
            _MinMark = 0; _MaxMark = 0;
            string sql = "select tblclassexamsubmap.MinMark, tblclassexamsubmap.MaxMark from tblclassexamsubmap where tblclassexamsubmap.SubId="+_SubjectId+" and tblclassexamsubmap.ClassExamId= (select tblclassexam.Id from tblclassexam where tblclassexam.ClassId=" + _ClassId + " and tblclassexam.ExamId=" + _ExamId + ")";
            MyReaderMark = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReaderMark.HasRows)
            {
              double.TryParse(MyReaderMark.GetValue(0).ToString(),out _MinMark);
              double.TryParse(MyReaderMark.GetValue(1).ToString(), out _MaxMark);
            }           
        }

        private int GetExamScheduleId(int _ClassId, int _BatchId, int _PeriodId, int _ExamId)
        {
            int _ExamScheduleId = 0;
            OdbcDataReader MyReaderExam = null;
            string sql = "select distinct(tblexamschedule.Id) from tblexamschedule inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId  and tblclassexam.ExamId="+_ExamId+"  where tblclassexam.ClassId="+_ClassId+" and tblexamschedule.BatchId="+_BatchId+" and tblexamschedule.PeriodId=" + _PeriodId;
            MyReaderExam =MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReaderExam.HasRows)
            {
                _ExamScheduleId = int.Parse(MyReaderExam.GetValue(0).ToString());
            }
            return _ExamScheduleId;
        }

        private string GetClassTableString()
        {
            StringBuilder CTR = new StringBuilder();

            CTR.Append("<table runat=\"server\" width=\"100%\" style=\"border: thin solid #000000\">");

            CTR.Append("<tr><td class=\"TableHeaderStyle\"><b>Class</b></td></tr>");
            CTR.Append("<tr><td class=\"SubHeaderStyle1\">.</td></tr>");           

            string _ClassTable = "";
            string SqlClass = "select Distinct(tblclass.ClassName) from tblclassstaffmap inner join tblclass on tblclassstaffmap.ClassId= tblclass.Id where tblclassstaffmap.StaffId=" + Drp_Staff.SelectedValue + " and tblclassstaffmap.ClassId in(select tblclassexam.ClassId from tblclassexam inner join tblexammaster on tblclassexam.ExamId= tblexammaster.Id where tblclassexam.ExamId=" + Drp_Exam.SelectedValue + " and tblexammaster.Status=1) and tblclassstaffmap.SubjectId in (select distinct(tblclassexamsubmap.SubId) from tblclassexamsubmap where tblclassexamsubmap.ClassExamId in (select tblclassexam.Id from tblclassexam where tblclassexam.ExamId=" + Drp_Exam.SelectedValue + ")) order by tblclassstaffmap.ClassId";
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(SqlClass);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    CTR.Append("<tr><td class=\"CellStyle1\"><b>" + MyReader.GetValue(0).ToString().Trim() + "</b></td></tr>");
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

        private void LoadExamsForSubjectsTheSelectedStaffTaken()
        {
            Drp_Exam.Items.Clear();

            string sql = "select distinct( tblexammaster.Id), tblexammaster.ExamName FROM tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster on tblclassexam.ExamId= tblexammaster.Id where tblclassexam.Status=1 and tblexamschedule.BatchId=" + MyUser.CurrentBatchId;
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
            }
        }

        private void LoadAllStaffsToDropDown()
        {
            Drp_Staff.Items.Clear();

            string sql = "select DISTINCT( tbluser.Id), tbluser.SurName from tbluser inner join tblclassstaffmap on tbluser.Id= tblclassstaffmap.StaffId where tbluser.Status=1 ORDER BY tbluser.SurName";
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_Show.Enabled = true;
                Lbl_indexammsg.Text = "";
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Staff.Items.Add(li);
                }
            }
            else
            {
                Btn_Show.Enabled = false;
                ListItem li = new ListItem("No Staff present", "-1");
                Drp_Staff.Items.Add(li);
                Lbl_indexammsg.Text = "No Staff found";
            }
        }

        protected void Drp_Staff_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear();
            LoadExamsForSubjectsTheSelectedStaffTaken();
            LoadExamPeriodsToDropDown();
        }

        private void LoadExamPeriodsToDropDown()
        {
            Drp_Period.Items.Clear();

            string sql = "select distinct(tblperiod.Id), tblperiod.Period from tblperiod inner join tblexamschedule on tblperiod.Id= tblexamschedule.PeriodId where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblexamschedule.ClassExamId in(select tblclassexam.Id from tblclassexam where tblclassexam.ExamId=" + Drp_Exam.SelectedValue + " and tblclassexam.ClassId in(select tblclassstaffmap.ClassId from tblclassstaffmap where tblclassstaffmap.StaffId=" + Drp_Staff.SelectedValue + "))";
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

        protected void Btn_Show_Click(object sender, EventArgs e)
        {
            LoadStaffPerformanceDetails();
        }

        protected void Img_Export_Click(object sender, ImageClickEventArgs e)
        {
            StringBuilder CTR = new StringBuilder();
            string _WorkSheetName = "TeacherwisePerformance";
            string FileName = "TeacherwisePerformanceReport";
            string _TableString = (string)ViewState["ExcelData"];

            CTR.Append("<table runat=\"server\" width=\"100%\" style=\"border: thin solid #000000\">");
            CTR.Append("<tr><td align=\"center\"><b>" + Drp_Staff.SelectedItem.Text.ToUpper() + "</b></td></tr>");
            CTR.Append("<tr><td align=\"center\"><b>" + Drp_Exam.SelectedItem.Text.ToUpper() + " (" + Drp_Period.SelectedItem.Text + ")" + "</b></td></tr>");

            CTR.Append("<tr><td>" + _TableString);
            CTR.Append("</td></tr></table>");
            string _TableStringFull = CTR.ToString();
            WinEr.ExcelUtility.ExportBuiltStringToExcel(FileName, _TableStringFull, _WorkSheetName);          
        }

        protected void Drp_Exam_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear();
            LoadExamPeriodsToDropDown();
        }

        protected void Drp_SelectSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            StaffArray[,] _GraphData = (StaffArray[,])Session["GraphDetail"];
            LoadPerformanceGraphWithExamData(_GraphData);
        }

    }
}
