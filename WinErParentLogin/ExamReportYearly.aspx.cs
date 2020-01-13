using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;
using System.Text;
using WebChart;
using System.Drawing;

namespace WinErParentLogin
{
    public struct ExamArray
    {
        public string ExamName;
        public string Mark;
        public bool Enable;
    }
    public partial class ExamReportYearly : System.Web.UI.Page
    {
        private ParentInfoClass MyParentInfo;
        private OdbcDataReader MyReader = null;
       
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
                LoadAllPreviousClassesToDropDown();
                Img_Export.Visible = false;
                LoadDetails();
                Label MyHeader = (Label)this.Master.FindControl("Lbl_PageHeader");
                MyHeader.Text = "Yearly Exam";
            }
        }


        private void ReleaseResourse(MysqlClass _mysqlObj, ParentLogin MyParent)
        {
            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyParent = null;
        }

        protected void Img_Export_Click(object sender, ImageClickEventArgs e)
        {
            string _WorkSheetName = "ExamDetails";
            string FileName = "PreviousBatchExamReport";

            string _TableString = (string)ViewState["ExcelFormat"];// GetExamReportDetailsToExcel(); 

            if (!WinEr.ExcelUtility.ExportBuiltStringToExcel(FileName, _TableString, _WorkSheetName))
            {
                MSGBOX.ShowMssage("This function need Ms office");
            }
        }

        private string GetExamReportDetailsToExcel()
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            StudentManagerClass MyStudMang = new StudentManagerClass(_mysqlObj);
            OdbcDataReader m_MyReader = null;
            OdbcDataReader MyReader1 = null;
            int _ClassId = 0;
            string[,] _Subjects = new string[20, 2];
            if (int.Parse(Drp_Class.SelectedValue) > 0)
            {
                _ClassId = MyStudMang.GetHistoryClassId(MyParentInfo.StudentId, int.Parse(Drp_Class.SelectedValue));
            }

            StringBuilder CTR = new StringBuilder();

            CTR.Append("<table runat=\"server\" width=\"100%\" style=\"border: thin solid #000000\">");

            CTR.Append("<tr>");
            string sql = "select tblexamschedule_history.Id, tblexamschedule_history.ExamName, ( select tblperiod.Period from tblperiod where tblperiod.Id= tblexamschedule_history.PeriodId) as Period, (SELECT tblclass.ClassName from tblclass where tblclass.Id= tblexamschedule_history.ClassId) as ClassName,(select tblbatch.BatchName from tblbatch where tblbatch.Id= tblexamschedule_history.BatchId) as Batch from tblexamschedule_history where";

            if (int.Parse(Drp_Class.SelectedValue) > 0)
            {
                sql = sql + " tblexamschedule_history.BatchId=" + Drp_Class.SelectedValue + " and tblexamschedule_history.ClassId=" + _ClassId;
            }
            else
            {
                sql = sql + " tblexamschedule_history.BatchId in (select Distinct( tblstudentclassmap_history.BatchId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + MyParentInfo.StudentId + ") and tblexamschedule_history.ClassId in (select Distinct( tblstudentclassmap_history.ClassId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + MyParentInfo.StudentId + ")";
            }

            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    int i = 0;
                    string Query = "";

                    CTR.Append("<td valign=\"top\"><table runat=\"server\" width=\"100%\"><tr><td colspan=\"2\" class=\"TableHeaderStyle\"><b>" + MyReader.GetValue(3).ToString() + ":" + MyReader.GetValue(4).ToString() + "</b></td></tr>  <tr><td colspan=\"2\" class=\"SubHeaderStyle\"><b>" + MyReader.GetValue(1).ToString() + " (" + MyReader.GetValue(2).ToString() + ")" + "</b></td></tr>  <tr><td colspan=\"2\"></td></tr>");

                    string sqlMark = "select tblexammark_history.MarkColumn, tblexammark_history.SubjectName, tblexammark_history.MaxMark  from tblexammark_history where tblexammark_history.ExamSchId=" + MyReader.GetValue(0).ToString() + " order by tblexammark_history.SubjectOrder";
                    m_MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sqlMark);
                    if (m_MyReader.HasRows)
                    {
                        while (m_MyReader.Read())
                        {
                            i++;
                            _Subjects[i, 0] = m_MyReader.GetValue(1).ToString();
                            _Subjects[i, 1] = m_MyReader.GetValue(2).ToString();

                            if (i > 1)
                            {
                                Query = Query + ",";
                            }
                            Query = Query + m_MyReader.GetValue(0).ToString();
                        }
                    }


                    string sql1 = "select " + Query + ", tblstudentmark_history.TotalMax , tblstudentmark_history.TotalMark , tblstudentmark_history.`Avg`, Grade, Result, Rank  from tblstudentclassmap_history inner join tblstudentmark_history on tblstudentmark_history.StudId = tblstudentclassmap_history.StudentId where tblstudentmark_history.ExamSchId=" + MyReader.GetValue(0).ToString() + " and tblstudentmark_history.StudId=" + MyParentInfo.StudentId;
                    MyReader1 = MyStudMang.m_MysqlDb.ExecuteQuery(sql1);
                    if (MyReader1.HasRows)
                    {
                        for (int j = 1; j <= i; j++)
                        {
                            CTR.Append("<tr><td class=\"CellStyle\">" + _Subjects[j, 0] + "</td>  <td class=\"CellStyle\">" + MyReader1.GetValue(j - 1).ToString() + " / " + _Subjects[j, 1] + "</td>  </tr>");
                        }

                        CTR.Append("<tr><td class=\"CellStyle\">Total Mark </td>  <td class=\"CellStyle\">" + MyReader1.GetValue(i + 1).ToString() + "</td>  </tr>");
                        CTR.Append("<tr><td class=\"CellStyle\">Total Maximum </td>  <td class=\"CellStyle\">" + MyReader1.GetValue(i).ToString() + "</td>  </tr>");
                        CTR.Append("<tr><td class=\"CellStyle\">Average </td>  <td class=\"CellStyle\">" + MyReader1.GetValue(i + 2).ToString() + "</td>  </tr>");
                        CTR.Append("<tr><td class=\"CellStyle\">Grade </td>  <td class=\"CellStyle\">" + MyReader1.GetValue(i + 3).ToString() + "</td>  </tr>");
                        CTR.Append("<tr><td class=\"CellStyle\">Result </td>  <td class=\"CellStyle\">" + MyReader1.GetValue(i + 4).ToString() + "</td>  </tr>");
                        CTR.Append("<tr><td class=\"CellStyle\">Rank </td>  <td class=\"CellStyle\">" + MyReader1.GetValue(i + 5).ToString() + "</td>  </tr>");
                    }

                    CTR.Append("</table>");
                    CTR.Append("</td>");
                    CTR.Append("<td></td>");
                }
            }

            CTR.Append("</tr>");
            CTR.Append("</table>");

            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyStudMang = null;
            return CTR.ToString();
        }

        private void LoadAllPreviousClassesToDropDown()
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            Drp_Class.Items.Clear();
            string sql = "select tblstudentclassmap_history.BatchId, CONCAT( (select tblclass.ClassName from tblclass where tblclass.Id=tblstudentclassmap_history.ClassId), ':', (select tblbatch.BatchName from tblbatch where tblbatch.Id= tblstudentclassmap_history.BatchId)) as ClassName from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + MyParentInfo.StudentId + " order by tblstudentclassmap_history.BatchId";
            MyReader = MyParent.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_Class.Items.Add(new ListItem("ALL", "0"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                Drp_Class.Items.Add(new ListItem("No class found", "-1"));
            }
            ReleaseResourse(_mysqlObj, MyParent);
        }



        private void LoadDetails()
        {
            LoadAllPreviousExamReportToGrid();
            string CTR = GetExamReportDetailsToExcel();
            ViewState["ExcelFormat"] = CTR;
            this.ExamReport.InnerHtml = CTR.ToString();
        }

        private void LoadAllPreviousExamReportToGrid()
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            StudentManagerClass MyStudMang = new StudentManagerClass(_mysqlObj);
            OdbcDataReader m_MyReader = null;
            OdbcDataReader MyReaderExamMark = null;

            int _ClassId = 0;
            string[,] _Subjects = new string[50, 4], _Results = new string[6, 2];
            string[,] _ColumnName = new string[100, 2];
            if (int.Parse(Drp_Class.SelectedValue) > 0)
            {
                _ClassId = MyStudMang.GetHistoryClassId(MyParentInfo.StudentId, int.Parse(Drp_Class.SelectedValue));
            }

            _Results[0, 0] = "Total Mark"; _Results[0, 1] = "TotalMark";
            _Results[1, 0] = "Total Max"; _Results[1, 1] = "TotalMax";
            _Results[2, 0] = "Average"; _Results[2, 1] = "Avg";
            _Results[3, 0] = "Grade"; _Results[3, 1] = "Grade";
            _Results[4, 0] = "Result"; _Results[4, 1] = "Result";
            _Results[5, 0] = "Rank"; _Results[5, 1] = "Rank";


            int i = 0;
            string Query = "";
            string sqlSubject = "select distinct(tblexammark_history.SubjectId), tblexammark_history.SubjectName, tblexammark_history.MarkColumn, tblexammark_history.MaxMark  from tblexammark_history where tblexammark_history.ExamSchId in ( select tblexamschedule_history.Id from tblexamschedule_history where";

            if (int.Parse(Drp_Class.SelectedValue) > 0)
            {
                sqlSubject = sqlSubject + " tblexamschedule_history.BatchId=" + Drp_Class.SelectedValue + " and tblexamschedule_history.ClassId=" + _ClassId + ")";
            }
            else
            {
                sqlSubject = sqlSubject + " tblexamschedule_history.BatchId in (select distinct( tblstudentclassmap_history.BatchId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + MyParentInfo.StudentId + ") and tblexamschedule_history.ClassId in (select Distinct( tblstudentclassmap_history.ClassId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + MyParentInfo.StudentId + "))";
            }
            sqlSubject = sqlSubject + " order by tblexammark_history.SubjectOrder";
            m_MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sqlSubject);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    i++;
                    _Subjects[i, 0] = m_MyReader.GetValue(0).ToString();//subject Id
                    _Subjects[i, 1] = m_MyReader.GetValue(1).ToString();//Subject Name
                    _Subjects[i, 2] = m_MyReader.GetValue(2).ToString();//Mark Column
                    _Subjects[i, 3] = m_MyReader.GetValue(3).ToString();//Max Mark

                    if (i > 1)
                    {
                        Query = Query + ",";
                    }
                    Query = Query + m_MyReader.GetValue(2).ToString();
                }
            }

            string[,] _SubjectIndex = new string[i + 1, 2]; //array to store all index values            

            for (int a = 0; a < i; a++)
            {
                _SubjectIndex[a, 0] = _Subjects[a + 1, 1]; _SubjectIndex[a, 1] = a.ToString();
            }
            _SubjectIndex[i, 0] = "Average"; _SubjectIndex[i, 1] = i.ToString();
            Session["Subjects"] = _SubjectIndex;

            DataSet ExamDataSet = new DataSet();
            DataTable dt;
            DataRow dr;

            try
            {
                ExamDataSet.Tables.Add(new DataTable("Exam"));
                dt = ExamDataSet.Tables["Exam"];

                dt.Columns.Add("Subject");
                int j = 0;

                string sql = "select tblexamschedule_history.Id, tblexamschedule_history.ExamName, ( select tblperiod.Period from tblperiod where tblperiod.Id= tblexamschedule_history.PeriodId) as Period, (SELECT tblclass.ClassName from tblclass where tblclass.Id= tblexamschedule_history.ClassId) as ClassName, (select tblbatch.BatchName from tblbatch where tblbatch.Id= tblexamschedule_history.BatchId) as Batch from tblexamschedule_history where";

                if (int.Parse(Drp_Class.SelectedValue) > 0)
                {
                    sql = sql + " tblexamschedule_history.BatchId=" + Drp_Class.SelectedValue + " and tblexamschedule_history.ClassId=" + _ClassId;
                }
                else
                {
                    sql = sql + " tblexamschedule_history.BatchId in (select Distinct( tblstudentclassmap_history.BatchId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + MyParentInfo.StudentId + ") and tblexamschedule_history.ClassId in (select Distinct( tblstudentclassmap_history.ClassId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + MyParentInfo.StudentId + ")";
                }
                sql = sql + " order by tblexamschedule_history.Id ";

                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        dt.Columns.Add(MyReader.GetValue(1).ToString() + " (" + MyReader.GetValue(2).ToString() + ")-" + MyReader.GetValue(3).ToString() + ":" + MyReader.GetValue(4).ToString());
                        _ColumnName[j, 0] = MyReader.GetValue(1).ToString() + " (" + MyReader.GetValue(2).ToString() + ")-" + MyReader.GetValue(3).ToString() + ":" + MyReader.GetValue(4).ToString();//column name
                        _ColumnName[j, 1] = MyReader.GetValue(0).ToString();//ExamId      
                        j++;
                    }
                }

                ExamArray[,] _ExamArray = new ExamArray[i + 1, j];

                for (int a = 1; a <= i; a++)
                {
                    dr = dt.NewRow();
                    dr["Subject"] = _Subjects[a, 1];

                    for (int x = 0; x < j; x++)
                    {
                        string _ExamMark = "select Round( " + _Subjects[a, 2] + ",2)  from  tblstudentmark_history where tblstudentmark_history.ExamSchId =" + _ColumnName[x, 1] + " and tblstudentmark_history.StudId=" + MyParentInfo.StudentId;
                        MyReaderExamMark = MyStudMang.m_MysqlDb.ExecuteQuery(_ExamMark);
                        if (MyReaderExamMark.HasRows)
                        {
                            _ExamArray[a - 1, x].ExamName = _ColumnName[x, 0];
                            _ExamArray[a - 1, x].Enable = true;

                            if (MyReaderExamMark.GetValue(0).ToString() == "")
                            {
                                dr[_ColumnName[x, 0]] = "--";
                                _ExamArray[a - 1, x].Mark = "--";
                                _ExamArray[a - 1, x].Enable = false;

                            }
                            else if (MyReaderExamMark.GetValue(0).ToString() == "-1")
                            {
                                dr[_ColumnName[x, 0]] = "A";
                                _ExamArray[a - 1, x].Mark = "A";

                            }
                            else
                            {
                                dr[_ColumnName[x, 0]] = MyReaderExamMark.GetValue(0).ToString();
                                _ExamArray[a - 1, x].Mark = MyReaderExamMark.GetValue(0).ToString();
                            }
                        }
                        else
                        {
                            dr[_ColumnName[x, 0]] = "--";
                            _ExamArray[a - 1, x].ExamName = _ColumnName[x, 0];
                            _ExamArray[a - 1, x].Mark = "--";
                            _ExamArray[a - 1, x].Enable = false;
                        }
                    }
                    dt.Rows.Add(dr);
                }


                if (i > 0)// Add columns only if student has exam and subjects
                {
                    //one empty row after all subjects
                    dr = dt.NewRow();
                    dr["Subject"] = "";
                    for (int x = 0; x < j; x++)
                    {
                        dr[_ColumnName[x, 0]] = "";
                    }
                    dt.Rows.Add(dr);

                    for (int a = 0; a < 6; a++)
                    {
                        dr = dt.NewRow();
                        dr["Subject"] = _Results[a, 0];

                        for (int x = 0; x < j; x++)
                        {
                            string _ExamMark = "select ROUND(" + _Results[a, 1] + ",2)  from  tblstudentmark_history where tblstudentmark_history.ExamSchId =" + _ColumnName[x, 1] + " and tblstudentmark_history.StudId=" + MyParentInfo.StudentId;
                            MyReaderExamMark = MyStudMang.m_MysqlDb.ExecuteQuery(_ExamMark);
                            if (MyReaderExamMark.HasRows)
                            {
                                if (a == 2)
                                {
                                    _ExamArray[i, x].ExamName = _ColumnName[x, 0];
                                    _ExamArray[i, x].Enable = true;
                                }

                                if (MyReaderExamMark.GetValue(0).ToString() == "")
                                {
                                    dr[_ColumnName[x, 0]] = "--";
                                    if (a == 2)
                                    {
                                        _ExamArray[i, x].Mark = "--";
                                        _ExamArray[i, x].Enable = false;
                                    }
                                }
                                else
                                {
                                    dr[_ColumnName[x, 0]] = MyReaderExamMark.GetValue(0).ToString();
                                    if (a == 2)
                                    {
                                        _ExamArray[i, x].Mark = MyReaderExamMark.GetValue(0).ToString();
                                        _ExamArray[i, x].Enable = true;
                                    }
                                }
                            }
                            else
                            {
                                dr[_ColumnName[x, 0]] = "--";
                                if (a == 2)
                                {
                                    _ExamArray[i, x].ExamName = _ColumnName[x, 0];
                                    _ExamArray[i, x].Mark = "--";
                                    _ExamArray[i, x].Enable = false;
                                }
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }


                if (ExamDataSet.Tables[0].Rows.Count > 0)
                {
                    Session["ExamReportArray"] = _ExamArray;
                    Img_Export.Visible = true;
                    //Pnl_ExamGraph.Visible = true;
                    this.MarkListArea.Visible = true;
                    this.MarkListArea1.Visible = true;
                    //this.ExamNames.Visible = true;
                    Lbl_indexammsg.Text = "";
                    //LoadConditionDropDownWithSubject();
                    //LoadPerformanceGraphWithExamData();
                }
                else
                {
                    //Pnl_ExamGraph.Visible = false;
                    this.MarkListArea.Visible = false;
                    this.MarkListArea1.Visible = false;
                    Lbl_indexammsg.Text = "No Exam Reports found";
                    Img_Export.Visible = false;
                    //this.ExamNames.Visible = false;
                }

            }
            catch (Exception e)
            {
               // Lbl_indexammsg.Text = e.ToString();
                Img_Export.Visible = true;
            }
            finally
            {
                _mysqlObj.CloseConnection();
                MyStudMang = null;

            }
        }

        //private void LoadPerformanceGraphWithExamData()
        //{
        //    //DataSet ExamDataSet = (DataSet)ViewState["ConsoldatedExamData"];
        //    //DataTable dt = ExamDataSet.Tables["Exam"];

        //    ExamArray[,] _ExamArray = (ExamArray[,])Session["ExamReportArray"];
        //    int _SelectedCondition = int.Parse(Drp_SelectList.SelectedValue);
        //    int _RowCount = _ExamArray.GetLength(0);
        //    int _ColumnCount = _ExamArray.GetLength(1);

        //    string _ExamList = " <table width=\"100%\">";
        //    for (int k = 0; k < _ColumnCount; k++)
        //    {
        //        _ExamList = _ExamList + "<tr><td class=\"CellStyle\">Exam " + (k + 1).ToString() + "</td><td class=\"CellStyle\">" + _ExamArray[_SelectedCondition, k].ExamName + "</td></tr>";
        //    }
        //    _ExamList = _ExamList + " </table>";

        //    float MaxVal = 100;
        //    float _val = 0;
        //    if (_RowCount > 0)
        //    {
        //        ColumnChart chart_Bar = new ColumnChart();

        //        Chart chart_Line = new SmoothLineChart();

        //        ChartPointCollection chart_Line_data = chart_Line.Data;

        //        for (int i = 0; i < _ColumnCount; i++)
        //        {
        //            if (float.TryParse(_ExamArray[_SelectedCondition, i].Mark, out _val))
        //            {
        //                //DataColumn dc = _ExamArray[_SelectedCondition, i].ExamName;
        //                string ColumnName = "Exam " + (i + 1).ToString();// _ExamArray[_SelectedCondition, i].ExamName;
        //                if (MaxVal < _val)
        //                {
        //                    MaxVal = _val;
        //                }
        //                chart_Line_data.Add(new ChartPoint(ColumnName, _val));
        //                chart_Bar.Data.Add(new ChartPoint(ColumnName, _val));
        //            }
        //        }

        //        chart_Line.Line.Width = 2;
        //        chart_Line.Line.Color = Color.RoyalBlue;
        //        chart_Line.Legend = Drp_SelectList.SelectedValue;
        //        chartcontrol_ExamChart.Charts.Clear();
        //        chartcontrol_ExamChart.Charts.Add(chart_Line);

        //        //chart_Line.DataLabels.Visible = true;

        //        chartcontrol_ExamChart.YTitle.StringFormat.FormatFlags = StringFormatFlags.DirectionVertical;
        //        chartcontrol_ExamChart.YTitle.StringFormat.Alignment = StringAlignment.Near;


        //        if (MaxVal != 100)
        //        {
        //            MaxVal = MaxVal + 10;
        //        }
        //        chart_Bar.Shadow.Visible = true;
        //        chart_Bar.DataLabels.Visible = true;
        //        chart_Bar.MaxColumnWidth = 20;
        //        chartcontrol_ExamChart.YCustomEnd = MaxVal;
        //        chartcontrol_ExamChart.Charts.Add(chart_Bar);
        //        chart_Bar.Fill.Color = System.Drawing.Color.RoyalBlue;
        //        chartcontrol_ExamChart.Background.Color = Color.White;
        //        chartcontrol_ExamChart.RedrawChart();

        //        this.ExamNames.InnerHtml = _ExamList;
        //    }
        //}

        //private void LoadConditionDropDownWithSubject()
        //{
        //    Drp_SelectList.Items.Clear();

        //    string[,] _SubjectIndex = (string[,])Session["Subjects"];
        //    int _Subject = _SubjectIndex.GetLength(0);
        //    int i = 0;
        //    if (_Subject > 0)
        //    {
        //        while (i < _Subject)
        //        {
        //            ListItem li = new ListItem(_SubjectIndex[i, 0], _SubjectIndex[i, 1]);
        //            Drp_SelectList.Items.Add(li);
        //            i++;
        //        }
        //    }
        //    else
        //    {
        //        Drp_SelectList.Items.Add(new ListItem("No Conditions found", "-1"));
        //    }
        //}

        //protected void Drp_SelectList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    LoadPerformanceGraphWithExamData();
        //}

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDetails();
        }
    }
}
