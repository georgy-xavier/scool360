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
    public partial class StudentAttendHistory : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private Attendance MyAttendence;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (Session["StudId"] == null)
            {
                Response.Redirect("SearchStudent.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MyAttendence = MyUser.GetAttendancetObj();
            if (MyAttendence == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(603))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (!IsPostBack)
                {

                    string _MenuStr;
                    _MenuStr = MyStudMang.GetSubStudentMangMenuString(MyUser.UserRoleId, int.Parse(Session["StudType"].ToString()));
                    this.SubStudentMenu.InnerHtml = _MenuStr;
                    LoadStudentTopData();
                    Load_DrpPreviousBatchsDetails();
                }
            }
        }


        private void LoadStudentTopData()
        {
            string _Studstrip = MyStudMang.ToStripString(int.Parse(Session["StudId"].ToString()), "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StudId"].ToString()) + "&type=StudentImage", int.Parse(Session["StudType"].ToString()));
            this.StudentTopStrip.InnerHtml = _Studstrip;
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
                if ((_present + _absent+_halfday+_halfday+_holidays) == 0)
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


            string sql = "SELECT tblclass.ClassName,tblbatch.BatchName,tblattdperivousbatch.WorkingDays,tblattdperivousbatch.PresentDays,tblattdperivousbatch.AbsentDays,tblattdperivousbatch.HalfDays,tblattdperivousbatch.Holidays FROM tblstudentclassmap_history INNER JOIN tblbatch ON tblbatch.Id=tblstudentclassmap_history.BatchId INNER JOIN tblclass ON tblclass.Id=tblstudentclassmap_history.ClassId INNER JOIN tblattdperivousbatch ON tblattdperivousbatch.BatchId=tblstudentclassmap_history.BatchId WHERE tblstudentclassmap_history.StudentId=" + int.Parse(Session["StudId"].ToString()) + " AND tblattdperivousbatch.StudentId=" + int.Parse(Session["StudId"].ToString());
            MyReader = MyAttendence.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {

                    workingdays = 0; _no_presentdays = 0; _no_halfdays = 0; _no_absentdays = 0; _noholidays = 0;
                    _attendencepersent=0;
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


            if (MyAttendence.GetCurrentBatchNewattendanceDetails(int.Parse(Session["StudId"].ToString()), out workingdays, out _no_presentdays, out _no_absentdays, out _noholidays, out _no_halfdays, out _attendencepersent, MyUser.CurrentBatchId))
            {
                DataRow dr1 = dt.NewRow();
                dr1["Batch"] = MyUser.CurrentBatchName + "/" + MyStudMang.GetClassName(MyStudMang.GetClassId(int.Parse(Session["StudId"].ToString()), MyUser.CurrentBatchId));
                dr1["WorkingDays"] = workingdays;
                dr1["PresentDays"] = _no_presentdays;
                dr1["HalfDays"] = _no_halfdays;
                dr1["AbsentDays"] = _no_absentdays;
                dr1["Holidays"] = _noholidays;
                dr1["Percentage"] = _attendencepersent.ToString("#0.00");
                MyDataSet.Tables["Student"].Rows.Add(dr1);
            }


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
                    if (!WinEr.ExcelUtility.ExportDataToExcel(dt, _ReportName, FileName, MyUser.ExcelHeader))
                    {
                        Lbl_msg.Text = "This function needs Ms office";
                        this.MPE_MessageBox.Show();
                    }
                    else
                    {
                        Lbl_msg.Text = "This function needs Ms office";
                        this.MPE_MessageBox.Show();
                    }
                }
                else
                {
                    Lbl_msg.Text = "No data found for exporting";
                    this.MPE_MessageBox.Show();
                }
            }
            else
            {
                Lbl_msg.Text = "No data found for exporting";
                this.MPE_MessageBox.Show();
            }
        }



        

    }
}
