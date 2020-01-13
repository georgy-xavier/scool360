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

namespace WinErParentLogin
{
    public partial class StudentAttendanceYearly : System.Web.UI.Page
    {
        private ParentInfoClass MyParentInfo;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MyParentObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyParentInfo = (ParentInfoClass)Session["MyParentObj"];
            if (MyParentInfo == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (!IsPostBack)
            {

                Load_DrpPreviousBatchsDetails();
                Label MyHeader = (Label)this.Master.FindControl("Lbl_PageHeader");
                MyHeader.Text = "Yearly Attendance";
            }
        }



        private void Load_DrpPreviousBatchsDetails()
        {
            ViewState["DataSet"] = GetDataset();
            Load_Grid();

        }

        private void Load_Grid()
        {
            lbl_gridmsg.Text = "";
            MyDataSet = (DataSet)ViewState["DataSet"];
            if (MyDataSet.Tables[0].Rows.Count > 0)
            {
                Grd_Students.Columns[1].Visible = true;
                Grd_Students.Columns[2].Visible = true;
                Grd_Students.Columns[3].Visible = true;
                Grd_Students.Columns[4].Visible = true;
                Grd_Students.Columns[5].Visible = true;
                Grd_Students.Columns[6].Visible = true;
                Grd_Students.DataSource = MyDataSet;
                Grd_Students.DataBind();
                Grd_Students.Columns[1].Visible = false;
                Grd_Students.Columns[2].Visible = false;
                Grd_Students.Columns[3].Visible = false;
                Grd_Students.Columns[4].Visible = false;
                Grd_Students.Columns[5].Visible = false;
                Grd_Students.Columns[6].Visible = false;
                Load_GridChart();
                DrawPersentageChart();
            }
            else
            {
                Grd_Students.DataSource = null;
                Grd_Students.DataBind();
                lbl_gridmsg.Text = "No Attendance Report Exit";
                chartcontrol_persent.Visible = false;

            }
        }

        private void Load_GridChart()
        {
            foreach (GridViewRow gv in Grd_Students.Rows)
            {
                ChartControl chartcontrol = (ChartControl)gv.FindControl("chartcontrol");
                PieChart Piechart = (PieChart)chartcontrol.Charts.FindChart("Chart");
                Piechart.Colors = new Color[] { Color.Green, Color.Red, Color.Silver, Color.Yellow, Color.AntiqueWhite, Color.OrangeRed };

                int _present = int.Parse(gv.Cells[2].Text.ToString()), _halfday = int.Parse(gv.Cells[3].Text.ToString()), _absent = int.Parse(gv.Cells[4].Text.ToString()), _holidays = int.Parse(gv.Cells[5].Text.ToString());
                Piechart.Data.Clear();
                Piechart.Data.Add(new ChartPoint("Present", _present));
                Piechart.Data.Add(new ChartPoint("Absent", _absent));
                Piechart.Data.Add(new ChartPoint("Halfday", _halfday));
                Piechart.Data.Add(new ChartPoint("Holidays", _holidays));

                Piechart.DataLabels.Visible = false;
                if ((_present + _absent + _halfday + _halfday + _holidays) == 0)
                {
                    Piechart.Data.Add(new ChartPoint("NO DATA", 100));

                }
                chartcontrol.Background.Color = Color.White;
                chartcontrol.RedrawChart();
            }
        }

        private void DrawPersentageChart()
        {
            chartcontrol_persent.Visible = true;
            ColumnChart chart = new ColumnChart();
            foreach (GridViewRow gv in Grd_Students.Rows)
            {
                chart.Data.Add(new ChartPoint(gv.Cells[0].Text.ToString(), float.Parse(gv.Cells[6].Text.ToString())));
            }
            chart.Shadow.Visible = true;
            chart.DataLabels.Visible = true;
            chart.MaxColumnWidth = 20;
            chartcontrol_persent.Charts.Add(chart);
            chart.Fill.Color = System.Drawing.Color.RoyalBlue;
            chartcontrol_persent.Background.Color = Color.White;
            chartcontrol_persent.RedrawChart();
        }


        private DataSet GetDataset()
        {

            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            Attendance MyAttendence = new Attendance(_mysqlObj);
            StudentManagerClass MyStudMang = new StudentManagerClass(_mysqlObj);
            int workingdays = 0, _no_presentdays = 0, _no_halfdays = 0, _no_absentdays = 0, _noholidays = 0;
            double _attendencepersent;
            MyDataSet = new DataSet();
            DataTable dt = new DataTable();
            MyDataSet.Tables.Add(new DataTable("Student"));
            dt = MyDataSet.Tables["Student"];
            dt.Columns.Add("Batch");
            dt.Columns.Add("WorkingDays");
            dt.Columns.Add("PresentDays");
            dt.Columns.Add("HalfDays");
            dt.Columns.Add("AbsentDays");
            dt.Columns.Add("Holidays");
            dt.Columns.Add("Percentage");


            string sql = "SELECT tblclass.ClassName,tblbatch.BatchName,tblattdperivousbatch.WorkingDays,tblattdperivousbatch.PresentDays,tblattdperivousbatch.AbsentDays,tblattdperivousbatch.HalfDays,tblattdperivousbatch.Holidays FROM tblstudentclassmap_history INNER JOIN tblbatch ON tblbatch.Id=tblstudentclassmap_history.BatchId INNER JOIN tblclass ON tblclass.Id=tblstudentclassmap_history.ClassId INNER JOIN tblattdperivousbatch ON tblattdperivousbatch.BatchId=tblstudentclassmap_history.BatchId WHERE tblstudentclassmap_history.StudentId=" + MyParentInfo.StudentId + " AND tblattdperivousbatch.StudentId=" + MyParentInfo.StudentId;
            MyReader = MyAttendence.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {

                    workingdays = 0; _no_presentdays = 0; _no_halfdays = 0; _no_absentdays = 0; _noholidays = 0;
                    _attendencepersent = 0;
                    int.TryParse(MyReader.GetValue(2).ToString(), out workingdays);
                    int.TryParse(MyReader.GetValue(3).ToString(), out _no_presentdays);
                    int.TryParse(MyReader.GetValue(4).ToString(), out _no_absentdays);
                    int.TryParse(MyReader.GetValue(5).ToString(), out _no_halfdays);
                    int.TryParse(MyReader.GetValue(6).ToString(), out _noholidays);

                    if (_no_halfdays != 0)
                    {
                        _attendencepersent = (double)(_no_presentdays + (double)(_no_halfdays / 2)) * 100;
                    }
                    else
                    {
                        _attendencepersent = (double)(_no_presentdays) * 100;
                    }
                    if (workingdays != 0)
                        _attendencepersent = _attendencepersent / workingdays;

                    DataRow dr1 = dt.NewRow();
                    dr1["Batch"] = MyReader.GetValue(1).ToString() + "/" + MyReader.GetValue(0).ToString();
                    dr1["WorkingDays"] = workingdays;
                    dr1["PresentDays"] = _no_presentdays;
                    dr1["HalfDays"] = _no_halfdays;
                    dr1["AbsentDays"] = _no_absentdays;
                    dr1["Holidays"] = _noholidays;
                    dr1["Percentage"] = _attendencepersent.ToString("#0.00");
                    MyDataSet.Tables["Student"].Rows.Add(dr1);
                }
            }


            if (MyAttendence.GetCurrentBatchNewattendanceDetails(MyParentInfo.StudentId, out workingdays, out _no_presentdays, out _no_absentdays, out _noholidays, out _no_halfdays, out _attendencepersent, MyParentInfo.CurrentBatchId))
            {
                DataRow dr1 = dt.NewRow();
                dr1["Batch"] = MyParentInfo.CurrentBatchName + "/" + MyStudMang.GetClassName(MyStudMang.GetClassId(MyParentInfo.StudentId, MyParentInfo.CurrentBatchId));
                dr1["WorkingDays"] = workingdays;
                dr1["PresentDays"] = _no_presentdays;
                dr1["HalfDays"] = _no_halfdays;
                dr1["AbsentDays"] = _no_absentdays;
                dr1["Holidays"] = _noholidays;
                dr1["Percentage"] = _attendencepersent.ToString("#0.00");
                MyDataSet.Tables["Student"].Rows.Add(dr1);
            }

            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyAttendence = null;
            MyStudMang = null;
            return MyDataSet;
        }






        protected void Link_ChartDetails_Click(object sender, EventArgs e)
        {
            ShowChartDetais.Show();
            Load_GridChart();
            DrawPersentageChart();
        }

        protected void Img_excel_Click(object sender, ImageClickEventArgs e)
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            if (ViewState["DataSet"] != null)
            {
                DataSet dt = (DataSet)ViewState["DataSet"];
                if (dt.Tables[0].Rows.Count > 0)
                {
                    //if (!WinEr.ExcelUtility.ExportDataSetToExcel(dt, "SutdentAttendenceHistory.xls"))
                    //{
                    //    Lbl_msg.Text = "This function needs Ms office";
                    //    this.MPE_MessageBox.Show();
                    //}
                    string FileName = "SutdentAttendenceHistory";
                    string _ReportName = "SutdentAttendenceHistory";
                    if (!WinEr.ExcelUtility.ExportDataToExcel(dt, _ReportName, FileName, MyParent.ExcelHeader))
                    {
                        MSGBOX.ShowMssage("This function needs Ms office");
                        
                    }
                    else
                    {
                        MSGBOX.ShowMssage("This function needs Ms office");
                        
                    }
                }
                else
                {
                    MSGBOX.ShowMssage("No data found for exporting");
                    
                }
            }
            else
            {
                MSGBOX.ShowMssage("No data found for exporting");
                
            }
            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyParent = null;
        }

    }
}
