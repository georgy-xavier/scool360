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

namespace WinEr
{
    public partial class ClasswiseStaffAnalysisReport : System.Web.UI.Page
    {
        private StaffManager MyStaffMang;
        private KnowinUser MyUser;
        private Incident Myincident;
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
            Myincident = MyUser.GetIncedentObj();
            if (MyStaffMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(750))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                { 
                    LoadAllClassesToDropDown();
                    LoadExamsForSelectedClass();
                    Clear();
                    if (int.Parse(Drp_Exam.SelectedValue) > -1)
                    {
                        LoadStaffPerformanceDetails();
                    }
                }
            }
        }

        private void Clear()
        {
            this.MarkListArea.Visible = false;
            this.ExamReport.Visible = false;
            Pnl_PerformanceGraph.Visible = false;
            this.StaffList.Visible = false;
        }
       
        private void LoadStaffPerformanceDetails()
        {
            OdbcDataReader MyReaderStaffs = null;
            OdbcDataReader MyReaderMarkDetails = null;

            DataSet _StaffPerformanceDS = new DataSet();
            DataTable dt;
            DataRow dr;

            _StaffPerformanceDS.Tables.Add(new DataTable("Staff"));
            dt = _StaffPerformanceDS.Tables["Staff"];

            dt.Columns.Add("Subject");
            dt.Columns.Add("Teacher");
            dt.Columns.Add("ClassAverage(%)");
            dt.Columns.Add("TotalNo");
            dt.Columns.Add("No.Passed");
            dt.Columns.Add("No.Failed");
            dt.Columns.Add("0-20%");
            dt.Columns.Add("20-40%");
            dt.Columns.Add("40-60%");
            dt.Columns.Add("60-80%");
            dt.Columns.Add("80-100%");

            int _TotalSeats = GetTotalNumberOfStudentsInClass(int.Parse(Drp_Class.SelectedValue));

            string[,] _StaffToAverageData = new string[20, 2];

            string sql = "SELECT DISTINCT( tblsubjects.Id), tblsubjects.subject_name from tblsubjects inner join tblclasssubmap on tblsubjects.Id= tblclasssubmap.SubjectId where tblclasssubmap.ClassId="+Drp_Class.SelectedValue+" order by tblsubjects.Id";
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int _RowCount = MyReader.RecordsAffected,_CurrentRow=0;
                _StaffToAverageData = new string[_RowCount, 2];
                while (MyReader.Read())
                {
                    string _MarkColumn = "";
                    int _Limit1 = 0, _Limit2 = 0, _Limit3 = 0, _Limit4 = 0, _Limit5 = 0, _Passed = 0;
                    double _MinMark = 0, _MaxMark = 0, _TotalMark = 0;

                    dr=dt.NewRow();
                    dr["Subject"] = MyReader.GetValue(1).ToString();

                    string _StaffName = GetStaffNameFromClassStaffMap(int.Parse(MyReader.GetValue(0).ToString()),int.Parse(Drp_Class.SelectedValue));
                
                    string sqlStaffs = "select Distinct(tblexammark.MarkColumn), tblclassexamsubmap.MinMark, tblclassexamsubmap.MaxMark from tblexammark  inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=(select tblexamschedule.ClassExamId from tblexamschedule where tblexamschedule.Id="+Drp_Exam.SelectedValue+")  where tblexammark.ExamSchId="+Drp_Exam.SelectedValue+" and tblexammark.SubjectId="+MyReader.GetValue(0).ToString() +" ORDER BY tblexammark.SubjectOrder";
                    MyReaderStaffs = MyStaffMang.m_MysqlDb.ExecuteQuery(sqlStaffs);
                    if (MyReaderStaffs.HasRows)
                    {
                        double.TryParse(MyReaderStaffs.GetValue(1).ToString(), out _MinMark);
                        double.TryParse(MyReaderStaffs.GetValue(2).ToString(), out _MaxMark);
                        _MarkColumn = MyReaderStaffs.GetValue(0).ToString();
                    }
                    dr["Teacher"] = _StaffName;
                    dr["TotalNo"] = "";
                    dr["ClassAverage(%)"] = "";
                    dr["No.Passed"] = "";
                    dr["No.Failed"] = "";
                    dr["0-20%"] = "";
                    dr["20-40%"] = "";
                    dr["40-60%"] = "";
                    dr["60-80%"] = "";
                    dr["80-100%"] = "";

                    if (_MarkColumn != "")
                    {

                        string sqlMarkDetails = "select " + _MarkColumn + " from tblstudentmark where tblstudentmark.ExamSchId=" + Drp_Exam.SelectedValue + " and tblstudentmark.StudId in (select tblstudentclassmap.StudentId from tblstudentclassmap inner join tblstudent on tblstudentclassmap.StudentId= tblstudent.Id and tblstudent.Status=1 where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " and tblstudentclassmap.ClassId=" + int.Parse(Drp_Class.SelectedValue) + ")";
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
                            if (_Limit1 != (_TotalSeats -( _Limit2 + _Limit3 + _Limit4 + _Limit5)))
                            {
                                _Limit1 = _Limit1 + (_TotalSeats - (_Limit1 + _Limit2 + _Limit3 + _Limit4 + _Limit5));
                            }

                            dr["TotalNo"] = _TotalSeats.ToString();
                            dr["ClassAverage(%)"] = Math.Round((_TotalMark / (_MaxMark * _TotalSeats)) * 100, 2).ToString();
                            dr["No.Passed"] = _Passed.ToString();
                            dr["No.Failed"] = (_TotalSeats - _Passed).ToString();
                            dr["0-20%"] = _Limit1.ToString();
                            dr["20-40%"] = _Limit2.ToString();
                            dr["40-60%"] = _Limit3.ToString();
                            dr["60-80%"] = _Limit4.ToString();
                            dr["80-100%"] = _Limit5.ToString();

                            _StaffToAverageData[_CurrentRow, 0] = _StaffName;
                            _StaffToAverageData[_CurrentRow, 1] = Math.Round((_TotalMark / (_MaxMark * _TotalSeats)) * 100, 2).ToString();
                        }
                    }
                    dt.Rows.Add(dr);
                    _CurrentRow++;
                }               
            }

            if (_StaffPerformanceDS.Tables[0].Rows.Count > 0)
            {
                Grd_ExamDetails.DataSource = _StaffPerformanceDS;
                Grd_ExamDetails.DataBind();
                ViewState["StaffDetails"] = _StaffPerformanceDS;

                Pnl_PerformanceGraph.Visible = true;
                this.MarkListArea.Visible = true;
                this.ExamReport.Visible = true;
                this.StaffList.Visible = true;
                LoadStaffPerformanceGraph(_StaffToAverageData);
            }
            else
            {
                Clear();
            }
        }

        private void LoadStaffPerformanceGraph(string[,] _StaffToAverageData)
        {
            int _RowCount = _StaffToAverageData.GetLength(0);
            int _ColumnCount = _StaffToAverageData.GetLength(1);

            string _StaffList = " <table width=\"100%\">";
            for (int k = 0; k < _RowCount; k++)
            {
                if (_StaffToAverageData[k, 0] != null)
                {
                    _StaffList = _StaffList + "<tr><td class=\"CellStyle\">Staff " + (k + 1).ToString() + "</td><td class=\"CellStyle\">" + _StaffToAverageData[k, 0] + "</td></tr>";
                }
            }
            _StaffList = _StaffList + " </table>";

            float MaxVal = 100;
            float _val = 0;
            if (_RowCount > 0)
            {
                ColumnChart chart_Bar = new ColumnChart();

                Chart chart_Line = new SmoothLineChart();

                ChartPointCollection chart_Line_data = chart_Line.Data;

                for (int i = 0; i < _RowCount; i++)
                {
                    if (float.TryParse(_StaffToAverageData[i, 1], out _val))
                    {
                        string ColumnName = "Staff " + (i + 1).ToString();// _StaffToAverageData[i, 0];
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
                //chart_Line.Legend = Drp_SelectList.SelectedValue;
                chartcontrol_ExamChart.Charts.Clear();
                chartcontrol_ExamChart.Charts.Add(chart_Line);

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

                this.StaffList.InnerHtml = _StaffList;
            }
        }

        private string GetStaffNameFromClassStaffMap(int _SubjectId, int _ClassId)
        {
            OdbcDataReader MyReader1 = null;
            string _StaffName = "";
            int a = 0;
            string sql = "select  tbluser.SurName from tblclassstaffmap inner join tbluser on tblclassstaffmap.StaffId= tbluser.Id where tblclassstaffmap.ClassId=" + _ClassId + " and tblclassstaffmap.SubjectId=" + _SubjectId;
            MyReader1 = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader1.HasRows)
            {
                while (MyReader1.Read())
                {
                    if (a > 0)
                    {
                        _StaffName = _StaffName + " / ";
                    }
                    _StaffName = _StaffName + MyReader1.GetValue(0).ToString();
                    a++;
                }
            }
            return _StaffName;
        }

        private int GetTotalNumberOfStudentsInClass(int _ClassId)
        {
            int _TotalSeats = 0;
            string sql = "select count(tblstudentclassmap.StudentId) from tblstudentclassmap inner join tblstudent on tblstudent.Id=tblstudentclassmap.StudentId where tblstudent.`Status`=1 and tblstudentclassmap.ClassId=" + _ClassId + " and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId;
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out _TotalSeats);
            }
            return _TotalSeats;
        }

        private void LoadExamsForSelectedClass()
        {
            Drp_Exam.Items.Clear();

            string sql = "select tblexamschedule.Id, CONCAT( tblexammaster.ExamName,' (',(select tblperiod.Period from tblperiod where tblperiod.Id= tblexamschedule.PeriodId) ,')') as ExamName FROM tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster on tblclassexam.ExamId= tblexammaster.Id where tblclassexam.ClassId="+Drp_Class.SelectedValue+" and tblclassexam.Status=1 and tblexamschedule.BatchId="+MyUser.CurrentBatchId;
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

        private void LoadAllClassesToDropDown()
        {
            Drp_Class.Items.Clear();

            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Class present", "-1");
                Drp_Class.Items.Add(li);
            }
        }

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear();
            LoadExamsForSelectedClass();
        }

        protected void Btn_Show_Click(object sender, EventArgs e)
        {
            LoadStaffPerformanceDetails();
        }

        protected void Img_Export_Click(object sender, ImageClickEventArgs e)
        {
            DataSet _StaffReport = new DataSet();
            _StaffReport = (DataSet)ViewState["StaffDetails"];
            //if (!WinEr.ExcelUtility.ExportDataSetToExcel(_StaffReport, "Class-wiseStaffPerformanceReport.xls"))
            //{
            //    WC_MessageBox.ShowMssage("This function need Ms office");
            //}
            string FileName = "Class-wiseStaffPerformanceReport";
            string _ReportName = "Class-wise Staff Performance Report";
            if (!WinEr.ExcelUtility.ExportDataToExcel(_StaffReport, _ReportName, FileName, MyUser.ExcelHeader))
            {
                WC_MessageBox.ShowMssage("This function need Ms office");
            }
        }

    }
}
