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
using WinBase;
using WebChart;
using System.Drawing;
namespace WinErParentLogin
{
    public partial class StudentPerform : System.Web.UI.Page
    {
        private ParentInfoClass MyParentInfo;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet = null;
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
               
              
                LoadExamType();
                // GenerateTable();
                LoadExams();
                Img_Export.Enabled = false;
                Btn_Report.Enabled = false;
                Label MyHeader = (Label)this.Master.FindControl("Lbl_PageHeader");
                MyHeader.Text = "Exam Details";
                //some initlization

            }
            ChartControl.PerformCleanUp();
            onLoadExamGraph();
        }

        private void onLoadExamGraph()
        {

            if (ViewState["ConsoldatedExamData"] == null)
            {
                Pnl_ExamGraph.Visible = false;

            }
            else
            {
                Pnl_ExamGraph.Visible = true;
                //LoadSelectItems();
                LoadExamGraph();

            }
        }



        protected void Grd_ExamList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowState == DataControlRowState.Alternate)
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='gray';this.style.cursor='hand'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
                }
                else
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='gray';this.style.cursor='hand'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#F7F7DE';");
                }
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Grd_ExamList, "Select$" + e.Row.RowIndex);
            }

        }

        protected void LoadExams()
        {

            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            string sql = "select tblstudentmark.ExamSchId, tblexammaster.ExamName , tblperiod.Period from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster ON tblexammaster.Id= tblclassexam.ExamId inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblstudentmark on tblstudentmark.ExamSchId= tblexamschedule.Id where tblexamschedule.BatchId=" + MyParentInfo.CurrentBatchId + " AND tblstudentmark.StudId=" + MyParentInfo.StudentId + " AND tblexammaster.`Status`=1";
            MydataSet = MyParent.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_ExamList.Columns[1].Visible = true;
                Grd_ExamList.Columns[0].Visible = true;
                Grd_ExamList.DataSource = MydataSet;
                Grd_ExamList.DataBind();
                Grd_ExamList.Columns[1].Visible = false;
                Grd_ExamList.Columns[0].Visible = false;


            }
            else
            {
                Grd_ExamList.DataSource = null;
                Grd_ExamList.DataBind();
                Lbl_indexammsg.Text = "No Exams found";

            }
            ReleaseResourse(_mysqlObj, MyParent);
        }
        private void ReleaseResourse(MysqlClass _mysqlObj, ParentLogin MyParent)
        {
            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyParent = null;
        }
        protected void Grd_ExamList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_ExamList.PageIndex = e.NewPageIndex;
            LoadExams();
        }


        #region Consolidate

        private void DrawExamConsolidatedChart()
        {
            if (ViewState["ConsoldatedExamData"] == null)
            {
                Pnl_ExamGraph.Visible = false;

            }
            else
            {
                Pnl_ExamGraph.Visible = true;
                LoadSelectItems();
                LoadExamGraph();

            }
        }

        private void LoadExamGraph()
        {
            chartcontrol_ExamChart.Charts.Clear();
            DataSet ExamDataSet = (DataSet)ViewState["ConsoldatedExamData"];
            DataTable dt = ExamDataSet.Tables["Exam"];
            float MaxVal = 100;
            float _val = 0;
            if (dt.Rows.Count > 0)
            {
                ColumnChart chart_Bar = new ColumnChart();

                Chart chart_Line = new SmoothLineChart();

                ChartPointCollection chart_Line_data = chart_Line.Data;


                foreach (DataRow dr_ItamData in dt.Rows)
                {
                    if (dr_ItamData[0].ToString() == Drp_SelectList.SelectedValue)
                    {

                        for (int i = 1; i < dt.Columns.Count; i++)
                        {
                            if (float.TryParse(dr_ItamData[i].ToString(), out _val))
                            {
                                DataColumn dc = dt.Columns[i];
                                if (MaxVal < _val)
                                {
                                    MaxVal = _val;
                                }
                                chart_Line_data.Add(new ChartPoint(dc.ColumnName, _val));
                                chart_Bar.Data.Add(new ChartPoint(dc.ColumnName, _val));


                            }
                        }
                    }
                }
               
                chart_Line.Line.Width = 2;
                chart_Line.Line.Color = Color.RoyalBlue;
                chart_Line.Legend = Drp_SelectList.SelectedValue;
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
              


            }
        }

        private void LoadSelectItems()
        {
            DataSet ExamDataSet = (DataSet)ViewState["ConsoldatedExamData"];
            DataTable dt = ExamDataSet.Tables["Exam"];
            Drp_SelectList.Items.Clear();
            foreach (DataRow dr_ItemData in dt.Rows)
            {
                dr_ItemData[0].ToString();
                if (dr_ItemData[0].ToString() != "Maximum" && dr_ItemData[0].ToString() != "Grade" && dr_ItemData[0].ToString() != "Result" && dr_ItemData[0].ToString() != "Rank")
                {
                    ListItem li = new ListItem(dr_ItemData[0].ToString(), dr_ItemData[0].ToString());
                    Drp_SelectList.Items.Add(li);
                }

            }
            Drp_SelectList.SelectedValue = "Average";
        }

        private void LoadExamType()
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            Drp_ExamType.Items.Clear();
            string sql = "select tblsubject_type.Id, tblsubject_type.sbject_type from tblsubject_type";
            MyReader = MyParent.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_Exam.Enabled = true;
                Btn_Report.Enabled = true;
                Drp_ExamType.Items.Add(new ListItem("Select any exam type", "0"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_ExamType.Items.Add(li);
                }

            }
            else
            {
                Drp_ExamType.Items.Add(new ListItem("No exam type found", "0"));
                Drp_ExamType.Enabled = false;
                Btn_Report.Enabled = false;
            }
            ReleaseResourse(_mysqlObj, MyParent);
        }

        private void LoadExamToDrpList()
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            Drp_Exam.Items.Clear();
            string sql = "select distinct tblexammaster.Id, tblexammaster.ExamName from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster ON tblexammaster.Id= tblclassexam.ExamId inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId inner join tblstudentmark on tblstudentmark.ExamSchId= tblexamschedule.Id where tblexamschedule.BatchId=" + MyParentInfo.CurrentBatchId + " AND tblstudentmark.StudId=" + MyParentInfo.StudentId + " AND tblexammaster.`Status`=1 and tblexammaster.ExamTypeId=" + int.Parse(Drp_ExamType.SelectedValue);
            MyReader = MyParent.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_Report.Enabled = true;
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Exam.Items.Add(li);
                }

            }
            else
            {
                Drp_Exam.Items.Add(new ListItem("No exam found", "0"));
                Btn_Report.Enabled = false;
            }
            ReleaseResourse(_mysqlObj, MyParent);
        }

        protected void Drp_Exam_SelectedindexChanged(object sender, EventArgs e)
        {
            this.Tabs.ActiveTabIndex = 1;
            EportGrid.DataSource = null;
            EportGrid.DataBind();
            ViewState["ConsoldatedExamData"] = null;
            onLoadExamGraph();
        }

        protected void Btn_Report_Click(object sender, EventArgs e)
        {
            int i_ExamId = int.Parse(Drp_Exam.SelectedValue);
           
            DataSet MyExamData = GetExamData(MyParentInfo.StudentId, i_ExamId, MyParentInfo.CurrentBatchId);
            if (MyExamData != null && MyExamData.Tables != null && MyExamData.Tables[0].Rows.Count > 0)
            {
                EportGrid.DataSource = MyExamData;
                EportGrid.DataBind();
                ViewState["ConsoldatedExamData"] = MyExamData;
                Img_Export.Enabled = true;
            }
            else
            {
                ViewState["ConsoldatedExamData"] = null;
            }
            DrawExamConsolidatedChart();
        }

        private DataSet GetExamData(int _StudentId, int i_ExamId, int _BatchId)
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            StudentManagerClass MyStudMang = new StudentManagerClass(_mysqlObj);
            DataSet ExamDataSet = new DataSet();
            DataTable dt;
            DataRow dr;
            int i, j;

            int _examschid = -1;
            string _temp;
            ExamDataSet.Tables.Add(new DataTable("Exam"));
            dt = ExamDataSet.Tables["Exam"];
            DataSet ExamPeriods = MyStudMang.GetExamPeriods(_StudentId, i_ExamId, _BatchId);
            string[] name = new string[100];
            if (ExamPeriods != null && ExamPeriods.Tables != null && ExamPeriods.Tables[0].Rows.Count > 0)
            {

                dt.Columns.Add(" ");
                name[0] = " ";
                i = 1;

                foreach (DataRow Periods in ExamPeriods.Tables[0].Rows)
                {
                    // dt.Columns.Add(Periods[0].ToString());
                    dt.Columns.Add(Periods[1].ToString());
                    name[i] = Periods[1].ToString();
                    i++;

                }
                //dr = ExamDataSet.Tables["Exam"].NewRow();
                //dr[" "] = " ";
                //j = 0;
                //foreach (DataRow Periods in ExamPeriods.Tables[0].Rows)
                //{
                //    j++;
                //    dr[name[j]] = Periods[1].ToString();
                //}
                //ExamDataSet.Tables["Exam"].Rows.Add(dr);
                DataSet MySubjects = MyStudMang.GetSubjects(_StudentId, i_ExamId, _BatchId);
                if (MySubjects != null && MySubjects.Tables != null && MySubjects.Tables[0].Rows.Count > 0)
                {
                    j = 0;
                    foreach (DataRow dr_subject in MySubjects.Tables[0].Rows)
                    {
                        double _SubMark = 0;
                        j = 0;
                        dr = ExamDataSet.Tables["Exam"].NewRow();
                        dr[name[j]] = dr_subject[0].ToString();
                        foreach (DataRow Periods in ExamPeriods.Tables[0].Rows)
                        {
                            j++;
                            _examschid = MyStudMang.GetExamSchid(_StudentId, i_ExamId, _BatchId, int.Parse(Periods[0].ToString()));
                            _temp = GetMarkfromColumn(_StudentId, _examschid, dr_subject[2].ToString(), int.Parse(Periods[0].ToString()));
                            double.TryParse(_temp, out _SubMark);
                            if (_SubMark == -1)
                            {
                                dr[name[j]] = "A";
                            }
                            else
                            {
                                dr[name[j]] = (Math.Round(_SubMark, 2)).ToString();
                            }
                        }
                        ExamDataSet.Tables["Exam"].Rows.Add(dr);
                    }
                }

                //Maximum mark
                double _TotalMark = 0, _Avg = 0;
                i = 0;
                dr = ExamDataSet.Tables["Exam"].NewRow();
                dr[name[i]] = "Maximum";
                foreach (DataRow Periods in ExamPeriods.Tables[0].Rows)
                {
                    i++;
                    _examschid = MyStudMang.GetExamSchid(_StudentId, i_ExamId, _BatchId, int.Parse(Periods[0].ToString()));
                    dr[name[i]] = MyStudMang.GetMaximumMarkForaPeriod(_examschid, _StudentId);
                }
                ExamDataSet.Tables["Exam"].Rows.Add(dr);

                // Total mark

                i = 0;
                dr = ExamDataSet.Tables["Exam"].NewRow();
                dr[name[i]] = "Total";
                foreach (DataRow Periods in ExamPeriods.Tables[0].Rows)
                {
                    i++;
                    _examschid = MyStudMang.GetExamSchid(_StudentId, i_ExamId, _BatchId, int.Parse(Periods[0].ToString()));

                    double.TryParse(MyStudMang.GetTotalMarkForaPeriod(_examschid, _StudentId), out _TotalMark);

                    dr[name[i]] = Math.Round(_TotalMark, 2).ToString();
                }
                ExamDataSet.Tables["Exam"].Rows.Add(dr);

                //Average Mark

                i = 0;
                dr = ExamDataSet.Tables["Exam"].NewRow();
                dr[name[i]] = "Average";
                foreach (DataRow Periods in ExamPeriods.Tables[0].Rows)
                {
                    i++;
                    _examschid = MyStudMang.GetExamSchid(_StudentId, i_ExamId, _BatchId, int.Parse(Periods[0].ToString()));

                    double.TryParse(MyStudMang.GetAvgMarkForaPeriod(_examschid, _StudentId), out _Avg);
                    dr[name[i]] = Math.Round(_Avg, 2).ToString();
                }
                ExamDataSet.Tables["Exam"].Rows.Add(dr);

                //Grade
                i = 0;
                dr = ExamDataSet.Tables["Exam"].NewRow();
                dr[name[i]] = "Grade";
                foreach (DataRow Periods in ExamPeriods.Tables[0].Rows)
                {
                    i++;
                    _examschid = MyStudMang.GetExamSchid(_StudentId, i_ExamId, _BatchId, int.Parse(Periods[0].ToString()));
                    dr[name[i]] = MyStudMang.GetGradeForaPeriod(_examschid, _StudentId);
                }
                ExamDataSet.Tables["Exam"].Rows.Add(dr);
                // Result
                i = 0;
                dr = ExamDataSet.Tables["Exam"].NewRow();
                dr[name[i]] = "Result";
                foreach (DataRow Periods in ExamPeriods.Tables[0].Rows)
                {
                    i++;
                    _examschid = MyStudMang.GetExamSchid(_StudentId, i_ExamId, _BatchId, int.Parse(Periods[0].ToString()));
                    dr[name[i]] = MyStudMang.GetResultForaPeriod(_examschid, _StudentId);
                }
                ExamDataSet.Tables["Exam"].Rows.Add(dr);
                //Rank
                i = 0;
                dr = ExamDataSet.Tables["Exam"].NewRow();
                dr[name[i]] = "Rank";
                foreach (DataRow Periods in ExamPeriods.Tables[0].Rows)
                {
                    i++;
                    _examschid = MyStudMang.GetExamSchid(_StudentId, i_ExamId, _BatchId, int.Parse(Periods[0].ToString()));
                    dr[name[i]] = MyStudMang.GetRankForaPeriod(_examschid, _StudentId);
                }
                ExamDataSet.Tables["Exam"].Rows.Add(dr);
            }
            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyStudMang = null;
            return ExamDataSet;
        }

        protected void Grd_ExamList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _ExamScheduleId = int.Parse(Grd_ExamList.SelectedRow.Cells[1].Text.ToString());
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "<script language=JavaScript>window.open(\"StudExamReport.aspx?SchId=" + _ExamScheduleId + "\")</script>");
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "openIncpopup('StudExamReport.aspx?SchId=" + _ExamScheduleId + "');", true);
        }

        protected void Grd_ExamList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            int _ExamScheduleId = int.Parse(Grd_ExamList.Rows[e.RowIndex].Cells[1].Text.ToString());
            string _ExamName = Grd_ExamList.Rows[e.RowIndex].Cells[2].Text.ToString();
            int _PeriodId = 0, _ExamId = 0, ClassId_Scheduled = 0;
            string sql = "select tblexamschedule.PeriodId,tblclassexam.ClassId from tblexamschedule INNER JOIN tblclassexam ON tblexamschedule.ClassExamId=tblclassexam.Id where tblexamschedule.Id=" + _ExamScheduleId;
            MyReader = MyParent.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _PeriodId = int.Parse(MyReader.GetValue(0).ToString());
                ClassId_Scheduled = int.Parse(MyReader.GetValue(1).ToString());
            }
            sql = "SELECT tblexammaster.Id from tblexammaster where tblexammaster.ExamName='" + _ExamName + "'";
            MyReader = MyParent.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _ExamId = int.Parse(MyReader.GetValue(0).ToString());
            }


            ExamReportPdf MyPdf = new ExamReportPdf(_mysqlObj, MyParentInfo.SchoolObject);

            string _ErrorMsg = "";
            string _physicalpath = WinerUtlity.GetParentLoginAbsoluteFilePath(MyParentInfo.SchoolObject, Server.MapPath(""));
            string _PdfName = "";
            int _StudentId = MyParentInfo.StudentId;


            if (MyPdf.CreateIndividualStudentPeriodExamReportPdf(_StudentId, _ExamId, _PeriodId, MyParentInfo.CurrentBatchId, MyParentInfo.CurrentBatchName, ClassId_Scheduled, _physicalpath, out _PdfName, out _ErrorMsg))
            {
                _ErrorMsg = "";
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
            }
            else
            {
               
            }
            ReleaseResourse(_mysqlObj, MyParent);
           
        }

        private bool PdfReportEnabled()
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            bool _valid = false;
            string PdfReport;
            string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='PdfReport'";
            MyReader = MyParent.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                PdfReport = MyReader.GetValue(0).ToString();
                if (PdfReport == "1")
                {
                    _valid = true;
                }
            }
            ReleaseResourse(_mysqlObj, MyParent);
            return _valid;
        }

        public string GetMarkfromColumn(int _studId, int _examschid, string _column, int Period)
        {
            string _ExamMark;
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            string sql = "Select tblstudentmark." + _column + " From  tblstudentmark inner join tblexamschedule on tblexamschedule.Id = tblstudentmark.ExamSchId where tblstudentmark.ExamSchId=" + _examschid + " AND tblstudentmark.StudId=" + _studId + " and tblexamschedule.PeriodId =" + Period + "";
            MyReader = MyParent.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _ExamMark = MyReader.GetValue(0).ToString();
            }
            else
            {
                _ExamMark = "-1";
            }
            ReleaseResourse(_mysqlObj, MyParent);
            return _ExamMark;
        }

        protected void Drp_Examlist_SelectedindexChanged(object sender, EventArgs e)
        {
            EportGrid.DataSource = null;
            EportGrid.DataBind();
            ViewState["ConsoldatedExamData"] = null;
            if (Drp_ExamType.SelectedValue != "0")
            {
                LoadExamToDrpList();
            }
            this.Tabs.ActiveTabIndex = 1;
            onLoadExamGraph();
        }

        protected void Img_Export_Click(object sender, ImageClickEventArgs e)
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            if (ViewState["ConsoldatedExamData"] != null)
            {
                
                DataSet MyExamData = (DataSet)ViewState["ConsoldatedExamData"];
                if (MyExamData.Tables.Count > 0)
                {


                    string FileName = "ExamReport";
                    string _ReportName = "ExamReport";
                    if (!WinEr.ExcelUtility.ExportDataToExcel(MyExamData, _ReportName, FileName, MyParent.ExcelHeader))
                    {
                        Lbl_Message.Text = "This function need Ms office";
                    }
                }

            }
            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyParent = null;
        }

        protected void Img_PdfExport_Click(object sender, ImageClickEventArgs e)
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            if ((Drp_Exam.Text != "") && (Drp_ExamType.SelectedValue != ""))
            {

                ExamReportPdf MyPdf = new ExamReportPdf(_mysqlObj, MyParentInfo.SchoolObject);
                string _ErrorMsg = "";
                string _physicalpath = WinerUtlity.GetParentLoginAbsoluteFilePath(MyParentInfo.SchoolObject, Server.MapPath(""));
                string _PdfName = "";
                int _StudentId = MyParentInfo.StudentId;
                int _ExamId = int.Parse(Drp_Exam.SelectedValue);
                int a = int.Parse(Drp_ExamType.SelectedValue);
                string c = Drp_ExamType.SelectedIndex.ToString();
                string d = Drp_Exam.SelectedIndex.ToString();
                if (MyPdf.CreateIndividualStudentExamReportPdf(_StudentId, _ExamId, MyParentInfo.CurrentBatchId, MyParentInfo.CurrentBatchName, _physicalpath, out _PdfName, out _ErrorMsg))
                {
                    _ErrorMsg = "";
                    ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                   // ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                    
                }
                else
                {
                    
                }
               
            }
           
        }


        protected void Drp_SelectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadExamGraph();
        }

        # endregion

        protected void Lnk_PreviousPerformance_Click(object sender, EventArgs e)
        {
            Response.Redirect("ExamReportYearly.aspx");
                 //ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open('ExamReportPreviousBatch.aspx, 'Info', status='width=1100,height=600,scrollbars,resizable'');", true);
                 //ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "openIncpopup('ExamReportPreviousBatch.aspx');", true);
        }

    }
}
