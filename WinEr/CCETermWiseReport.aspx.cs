using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml.Serialization;
using System.Xml;
using WC.PDF.Document;
using System.IO;
using WC.PdfDocumentClass;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Pdf;
using MigraDoc.Rendering;
using System.Diagnostics;
using WinBase;
using System.Drawing;
using System.Text.RegularExpressions;
using WebChart;
using System.Data.Odbc;

namespace WinEr
{
    public partial class CCETermWiseReport : System.Web.UI.Page
    {
        private ExamManage MyExamMang;
        private KnowinUser MyUser;
        private SchoolClass objSchool = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyExamMang = MyUser.GetExamObj();
            if (MyExamMang == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }
            else
            {
                if (WinerUtlity.NeedCentrelDB())
                {
                    if (Session[WinerConstants.SessionSchool] == null)
                    {
                        Response.Redirect("Logout.aspx");
                    }
                    objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                }
                if (!IsPostBack)
                {
                    //some initlization
                    Load_ClassDropdown();
                    Load_TermDropdown();
                    divgrid.Visible = false;
                    Drp_TermSelect.Focus();
                }
            }
        }

        #region default loding function

        private void Load_TermDropdown()
        {
            Drp_TermSelect.Items.Clear();
            string sql = "SELECT tblcce_term.Id as Id,tblcce_term.TermName as TermName FROM tblcce_term";
            DataSet Ds_term = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (Ds_term != null && Ds_term.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drcls in Ds_term.Tables[0].Rows)
                {
                    li = new ListItem(drcls["TermName"].ToString(), drcls["Id"].ToString());
                    Drp_TermSelect.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("NO Data Found", "0");
                Drp_TermSelect.Items.Add(li);
            }
        }

        private void Load_ClassDropdown()
        {
            Drp_ClassSelect.Items.Clear();
            string sql = "SELECT tblclass.Id,tblclass.ClassName from tblclass where tblclass.Status=1 ORDER BY tblclass.ClassName";
            DataSet Ds_class = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (Ds_class != null && Ds_class.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drcls in Ds_class.Tables[0].Rows)
                {
                    li = new ListItem(drcls["ClassName"].ToString(), drcls["Id"].ToString());
                    Drp_ClassSelect.Items.Add(li);
                }

            }
            else
            {
                li = new ListItem("NO Data Found", "0");
                Drp_ClassSelect.Items.Add(li);

            }
        }

        protected void Btn_Continue_Click(object sender, EventArgs e)
        {
            string Err = "";
            if (Drp_TermSelect.SelectedItem.Value == "0")
                Err = "Term is not found!";
            else if (Drp_ClassSelect.SelectedItem.Value == "0")
                Err = "Class is not found!";
            else
            {
                if (Examstatus())
                    load_Grid();
                else
                    Err = "Selected term results is not published!";
            }

            if (Err != "")
            {
                WC_MessageBox.ShowMssage(Err);
            }
        }

        /// <summary>
        /// checking exam student marks updloaded or not 
        /// </summary>
        /// <param name="Batch"></param>
        /// <returns></returns>
        private bool Examstatus()
        {
            bool Estatus = false;
            string sql = "select tblcce_configmanager.reportsstatus from tblcce_configmanager where tblcce_configmanager.Termid=" + int.Parse(Drp_TermSelect.SelectedValue.ToString()) + " AND tblcce_configmanager.Classid=" + int.Parse(Drp_ClassSelect.SelectedValue.ToString());
            DataSet _publishds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_publishds.Tables[0].Rows.Count > 0 && _publishds != null)
                foreach (DataRow dr in _publishds.Tables[0].Rows)
                {
                    if (dr[0].ToString() == "2")
                        Estatus = true;
                }

            return Estatus;
        }
        /// <summary>
        /// student list load function 
        /// </summary>
        private void load_Grid()
        {
            int _ClassId = int.Parse(Drp_ClassSelect.SelectedValue);
            string sql = "SELECT tblstudent.Id as StudentId,tblstudent.StudentName as StudentName,tblstudentclassmap.RollNo as StudentRollno from tblstudent INNER JOIN tblstudentclassmap on  tblstudentclassmap.StudentId=tblstudent.Id where tblstudentclassmap.ClassId=" + _ClassId + " AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " ORDER by tblstudentclassmap.RollNo";
            DataSet _StudentDs = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (_StudentDs.Tables[0].Rows.Count > 0)
            {

                Grd_CCEstudent.Columns[1].Visible = true;
                Grd_CCEstudent.Columns[2].Visible = true;
                Grd_CCEstudent.DataSource = _StudentDs;
                Grd_CCEstudent.DataBind();
                Grd_CCEstudent.Columns[1].Visible = false;
                Grd_CCEstudent.Columns[2].Visible = false;
                Lbl_stuTotal.Text = Grd_CCEstudent.Rows.Count.ToString();
                Lbl_classname.Text = Drp_ClassSelect.SelectedItem.Text;
                Lbl_termname.Text = Drp_TermSelect.SelectedItem.Text;
                div1.Visible = false;
                divgrid.Visible = true;

            }
            else
            {
                Grd_CCEstudent.DataSource = null;
                Grd_CCEstudent.DataBind();
                Lbl_stuTotal.Text = "0";
                div1.Visible = true;
                divgrid.Visible = false;
                WC_MessageBox.ShowMssage("NO Student Found!");
            }
        }

        protected void Btn_cancel_Click(object sender, EventArgs e)
        {
            divgrid.Visible = false;
            Load_ClassDropdown();
            Load_TermDropdown();
            div1.Visible = true;
            Drp_TermSelect.Focus();
        }
        /// <summary>
        /// check box change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChkSelect_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chkselect = (CheckBox)Grd_CCEstudent.HeaderRow.Cells[0].FindControl("ChkSelect");
            if (_chkselect.Checked)
            {
                for (int i = 0; i < Grd_CCEstudent.Rows.Count; i++)
                {
                    CheckBox chkterm = (CheckBox)Grd_CCEstudent.Rows[i].Cells[0].FindControl("Chk_temselect");
                    chkterm.Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < Grd_CCEstudent.Rows.Count; i++)
                {
                    CheckBox chkterm = (CheckBox)Grd_CCEstudent.Rows[i].Cells[0].FindControl("Chk_temselect");
                    chkterm.Checked = false;
                }
            }
        }
        /// <summary>
        /// Grid checkbox validation
        /// </summary>
        /// <returns></returns>
        private int ValidationCheckbox()
        {
            int count = 0;
            for (int i = 0; i < Grd_CCEstudent.Rows.Count; i++)
            {
                CheckBox chkterm = (CheckBox)Grd_CCEstudent.Rows[i].Cells[0].FindControl("Chk_temselect");
                if (chkterm.Checked == true)
                {
                    count++;
                }
            }
            return count;
        }
        /// <summary>
        /// Generation event function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 




        protected void Btn_Generate_Click(object sender, EventArgs e)
        {

            string _Errmsg = "";
            try
            {
                int Chkcount = ValidationCheckbox();
                if (Chkcount <= 0)
                    _Errmsg = "Please select students!";
                else
                {
                    #region creating PDF report card


                    string _PhysicalPath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath("")) + "\\PDF_Files\\";//Path for PDF creation
                    string _DefaultImgpath = Server.MapPath("") + "\\Pics";
                    string _TempImgpath = Server.MapPath("") + "\\ThumbnailImages";

                    bool NeedGraph = CheckGraphConfig();
                    DataSet classTopMark = null;
                    DataSet classStudentMark = null;
                    float examMaxMark = 100;


                    Document PDFdocument = new Document();//PDF dll class
                    PageSetup pgSetup = PDFdocument.DefaultPageSetup;//PDF dll class

                    ConfigManager objConfigMgr = MyUser.GetConfigObj();
                    string reportType = "";string xmlstr = "";
     
                        xmlstr = ReadXMLfileFromDataBase(int.Parse(Drp_ClassSelect.SelectedValue.ToString()), int.Parse(Drp_TermSelect.SelectedValue.ToString()), Drp_TermSelect.SelectedItem.Text.ToString());//Reading xml file from database
                    if ( xmlstr == "")
                        _Errmsg = "Xml file is not founded.Please import xml file!";
                    else
                    {
                        foreach (GridViewRow dr in Grd_CCEstudent.Rows)
                        {
                            CheckBox chkterm = (CheckBox)dr.Cells[0].FindControl("Chk_temselect");
                            if (chkterm.Checked == true)
                            {
                                Session["StudId"] = dr.Cells[1].Text.ToString();
                                if (NeedGraph)
                                {
                                    if (classTopMark == null)
                                        classTopMark = GetMaxMark(ref examMaxMark, ref classStudentMark);
                                }
                                GetStudentExamPerformanace(out _Errmsg);//student marks taking from database
                                if (_Errmsg == "")
                                {
                                    Dictionary<string, DataSet> Dataset_Dic = new Dictionary<string, DataSet>();
                                    Dataset_Dic = (Dictionary<string, DataSet>)Session["DataSetDictionary"];

                                    CCEUtility objCCE = new CCEUtility(MyUser, objSchool, Dataset_Dic, int.Parse(Session["StudId"].ToString()));//This class is collect all student information 
                                    objCCE.m_PdfPysicalPath = _PhysicalPath;
                                    objCCE.BatchId = MyUser.CurrentBatchId;
                                    objCCE.BatchName = MyUser.CurrentBatchName;
                                    objCCE.ClassId = int.Parse(Drp_ClassSelect.SelectedValue.ToString());
                                    objCCE.ClassName = Drp_ClassSelect.SelectedItem.Text.ToString();
                                    int.TryParse(Drp_TermSelect.SelectedValue.ToString(), out objCCE.TermId);
                                    objCCE.TermName = Drp_TermSelect.SelectedItem.Text.ToString();
                                    objCCE.m_DefaultImgpath = _DefaultImgpath;
                                    objCCE.m_TempImgpath = _TempImgpath;
                                    if (NeedGraph)
                                        objCCE.m_PerformanceChartURI = GetStudentPerformanceChart(classTopMark, classStudentMark, examMaxMark);
                                    objCCE.xmlstring = xmlstr;
                                    if (!objCCE.Exporttopdfdocument(ref PDFdocument, pgSetup, out _Errmsg))//calling this function creation selected student PDFdocument 
                                        _Errmsg = _Errmsg + " Please try later!";

                                }
                            }
                            if (_Errmsg != "")
                                break;
                        }
                    }
                    #endregion




                    if (_Errmsg == "")
                    {
                        #region created Pdf from this region

                        string PdfPath = _PhysicalPath + "StuMarkReport_" + Drp_ClassSelect.SelectedItem.Text.ToString() + ".pdf";

                        if (File.Exists(PdfPath))
                              File.Delete(PdfPath);

                        const PdfFontEmbedding embedding = PdfFontEmbedding.Always;
                        const bool unicode = false;
                        PdfDocumentRenderer pdfrenderer = new PdfDocumentRenderer(unicode, embedding);
                        pdfrenderer.Document = PDFdocument;
                        pdfrenderer.RenderDocument();
                        pdfrenderer.PdfDocument.Save(PdfPath);
                        //Process.Start(PdfPath);
                        ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "null", "window.open(\"OpenPdfPage.aspx?PdfName=StuMarkReport_" + Drp_ClassSelect.SelectedItem.Text.ToString() + ".pdf\",'Info','status=1, width=1000, height=700,,resizable=1,scrollbars=1');", true);

                        #endregion

                        #region deleted all templet files like a student image,school log etc

                        string[] filePaths = Directory.GetFiles(_TempImgpath);
                        foreach (string filePath in filePaths)
                            File.Delete(filePath);

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                _Errmsg = ex.Message + "!Pdf is not created Sucessfully";
            }
            if (_Errmsg != "")
                WC_MessageBox.ShowMssage(_Errmsg);

        }

        private bool CheckGraphConfig()
        {
            bool _needGraph=false;
            string sql = "select * from tblconfiguration where id=83";
            OdbcDataReader Config_Dr = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (Config_Dr.HasRows)
            {
                if (Config_Dr.GetValue(3).ToString() == "1")
                    _needGraph = true;
            }
            return _needGraph;
        }

        private DataSet GetMaxMark(ref float ExamMaxMark, ref DataSet studentMark_Ds)
        {
            DataSet topMark_Ds = new DataSet();
            studentMark_Ds = new DataSet();
            string sql = "SELECT DISTINCT(tblcce_colconfig.ExamName),tblcce_colconfig.TableName,tblcce_colconfig.ColName,tblcce_colconfig.ExamMaxMark from tblcce_colconfig INNER JOIN tblcce_classgroupmap on tblcce_classgroupmap.GroupId=tblcce_colconfig.GroupId inner join tblstudentclassmap on tblstudentclassmap.ClassId=tblcce_classgroupmap.ClassId where tblstudentclassmap.StudentId=" + int.Parse(Session["StudId"].ToString()) + " and tblcce_colconfig.TermId=" + int.Parse(Drp_TermSelect.SelectedValue.ToString()) + " and tblcce_colconfig.TableName='tblcce_mark' order by tblcce_colconfig.Id DESC";
            DataSet Config_Ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Config_Ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                foreach (DataRow Config_dr in Config_Ds.Tables[0].Rows)
                {
                    sql = "select tblsubjects.subject_name,SubjectId,max(CONVERT(" + Config_dr[2].ToString() + ",DECIMAL(10,2))) as 'Maxmark' from tblcce_mark inner join tblsubjects on tblcce_mark.SubjectId= tblsubjects.Id  where StudentId in (select id from tblstudent where tblstudent.LastClassId=" + int.Parse(Drp_ClassSelect.SelectedValue.ToString()) + ") GROUP BY SubjectId";
                    topMark_Ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    double Mark;
                    if (double.TryParse((topMark_Ds.Tables[0].Rows[0][2].ToString()), out Mark))
                    {
                        string markQuery = "select tblsubjects.subject_name,SubjectId,tblcce_mark.StudentId," + Config_dr[2].ToString() + " from tblcce_mark inner join tblsubjects on tblcce_mark.SubjectId= tblsubjects.Id where StudentId in (select id from tblstudent where tblstudent.LastClassId=" + int.Parse(Drp_ClassSelect.SelectedValue.ToString()) + ")";
                        studentMark_Ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(markQuery);
                        float.TryParse(Config_dr[3].ToString(), out ExamMaxMark);
                        return topMark_Ds;
                    }
                }
                return null;
            }
        }

        private string GetStudentPerformanceChart(DataSet classMaxMark,DataSet classStudetnMark, float examMaxMark)
        {

            string strRetunl = string.Empty;

            WebChart.ChartControl ChartPerform = new WebChart.ChartControl
            {
                Width = 550,
                Height = 200,
                BorderStyle = System.Web.UI.WebControls.BorderStyle.None,
                ChartPadding = 20,
                YCustomEnd = 100,
                HasChartLegend = true,
                ID = "chartcontrol_ExamChart",
                BorderWidth = 0,
                Padding = 5,
                TopPadding = 0,
                YCustomStart = 0,
                YValuesInterval = 0

            };
            ChartPerform.Background.Color = System.Drawing.Color.LightSteelBlue;

            //Setting Top Mark in chart
            ColumnChart chart_Bar_Top = new ColumnChart();
            ColumnChart chart_Bar = new ColumnChart();
            chart_Bar_Top.Legend = "Top Student";
            chart_Bar.Legend = "Student";
            if (classMaxMark != null || classMaxMark.Tables[0].Rows.Count != 0 || classStudetnMark.Tables[0].Rows.Count != 0)
                foreach (DataRow maxMark_dr in classMaxMark.Tables[0].Rows)
                {
                    chart_Bar_Top.Data.Add(new ChartPoint(maxMark_dr[0].ToString(), float.Parse(Math.Round((Convert.ToDouble(maxMark_dr[2].ToString()) / (examMaxMark) * (100)), 1).ToString())));
                    foreach (DataRow mark_Dr in classStudetnMark.Tables[0].Rows)
                    {
                        if ((maxMark_dr[1].ToString() == mark_Dr[1].ToString()) && Session["StudId"].ToString() == mark_Dr[2].ToString())
                        {
                            float _mark = 0f;
                            if (float.TryParse(mark_Dr[3].ToString(), out _mark))
                                chart_Bar.Data.Add(new ChartPoint(mark_Dr[0].ToString(), float.Parse((Math.Round(_mark / (examMaxMark) * 100)).ToString())));
                            break;
                        }
                    }
                }


            chart_Bar.ShowLegend = true;
            chart_Bar_Top.ShowLegend = true;
            ChartPerform.YTitle.StringFormat.FormatFlags = StringFormatFlags.DirectionVertical;
            ChartPerform.YTitle.StringFormat.Alignment = StringAlignment.Near;
            ChartPerform.Charts.Clear();
            chart_Bar_Top.Shadow.Visible = false;
            chart_Bar_Top.DataLabels.Visible = false;
            chart_Bar_Top.MaxColumnWidth = 10;
            chart_Bar_Top.Fill.Color = System.Drawing.Color.Gray;

            chart_Bar.Shadow.Visible = false;
            chart_Bar.DataLabels.Visible = false;
            chart_Bar.MaxColumnWidth = 10;
            chart_Bar.Fill.Color = System.Drawing.Color.White;

            ChartPerform.Charts.Add(chart_Bar_Top);
            ChartPerform.Charts.Add(chart_Bar);
            ChartPerform.YCustomEnd = 100;
            ChartPerform.Background.Color = System.Drawing.Color.White;


            ChartPerform.RedrawChart();

            strRetunl = Server.MapPath("~/WebCharts/" + ChartPerform.ImageID + ".png");


            return strRetunl;
        }

        /// <summary>
        /// Taking xml file from database
        /// </summary>
        /// <param name="classid"></param>
        /// <param name="Termid"></param>
        /// <param name="TermName"></param>
        /// <returns></returns>
        private string ReadXMLfileFromDataBase(int classid, int Termid, string TermName)
        {
            string xmstring = "Null";
            string sql = "SELECT tblcce_classgroup.TermXMLFile,tblcce_classgroup.ConsoldateXMLFile from tblcce_classgroup inner join tblcce_classgroupmap ON tblcce_classgroupmap.GroupId=tblcce_classgroup.Id where tblcce_classgroupmap.ClassId=" + classid + " and tblcce_classgroupmap.Termid=" + Termid;
            DataSet Ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Ds != null || Ds.Tables[0] != null || Ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Ds.Tables[0].Rows)
                {
                    TermName = TermName.ToLower();
                    TermName = TermName.Replace(" ", "");
                    if (TermName.ToLower() == "consolidate")
                        xmstring = dr[1].ToString();
                    else
                        xmstring = dr[0].ToString();
                }
            }
            return xmstring;
        }

        #endregion

        #region Get student mark records
        /// <summary>
        /// collecting all student exam details 
        /// </summary>
        /// <param name="ErrMsg"></param>
        private void GetStudentExamPerformanace(out string ErrMsg)
        {
            ErrMsg = "";
            try
            {
                Dictionary<string, DataSet> Dataset_Dic = new Dictionary<string, DataSet>();//dataset set Dictionary .this dataset have all student mark information

                bool publish = true;
                bool result = true;
                string quryStr = "";
                int StudentId = int.Parse(Session["StudId"].ToString());
                string sql = "";

                #region Taking Marksubject Result

                sql = "SELECT DISTINCT(tblcce_colconfig.ExamName),tblcce_colconfig.TableName,tblcce_colconfig.ColName from tblcce_colconfig INNER JOIN tblcce_classgroupmap on tblcce_classgroupmap.GroupId=tblcce_colconfig.GroupId inner join tblstudentclassmap on tblstudentclassmap.ClassId=tblcce_classgroupmap.ClassId where tblstudentclassmap.StudentId=" + StudentId + " and tblcce_colconfig.TermId=" + int.Parse(Drp_TermSelect.SelectedValue.ToString());
                DataSet Mark_Ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (Mark_Ds.Tables[0].Rows.Count == 0)
                {
                    publish = false;
                    ErrMsg = "Column configuration is not created .Please check with exe!";
                }
                else
                {
                    foreach (DataRow C_dr in Mark_Ds.Tables[0].Rows)
                    {
                        string replacestr = C_dr[0].ToString();
                        quryStr = quryStr + "," + C_dr[1].ToString() + "." + C_dr[2].ToString() + " as " + "`" + replacestr + "`";
                    }
                    Mark_Ds = null;
                    sql = "SELECT tblcce_classsubject.SubjectOrder as SINo,tblsubjects.subject_name as SUBJECT " + quryStr + " from tblsubjects right join tblcce_classsubject on tblcce_classsubject.subjectid=tblsubjects.Id right join tblstudentclassmap on tblstudentclassmap.ClassId=tblcce_classsubject.classid right join tblcce_result on tblcce_result.StudentId=tblstudentclassmap.StudentId AND tblcce_result.SubjectId=tblcce_classsubject.subjectid RIGHT join tblcce_mark on tblcce_mark.StudentId=tblstudentclassmap.StudentId AND tblcce_mark.SubjectId=tblcce_classsubject.subjectid  where tblstudentclassmap.StudentId=" + StudentId + "  ORDER BY tblcce_classsubject.SubjectOrder";
                    Mark_Ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (Mark_Ds.Tables[0].Rows.Count == 0)
                    {
                        publish = false;
                        ErrMsg = "Selected term mark is not updated.Please check with exe!";
                    }
                    else
                    {
                        foreach (DataRow M_dr in Mark_Ds.Tables[0].Rows)
                        {
                            for (int i = 1; i < Mark_Ds.Tables[0].Columns.Count; i++)
                            {
                                if (M_dr[i].ToString() == "-" || M_dr[i].ToString() == "AB" || M_dr[i].ToString() == "WITHHELD")
                                    result = true; //result = false;
                            }
                        }
                    }

                    if (result == true && publish == true)
                    {
                        Dataset_Dic.Add("@@AcademicPerformance@@", SetDefaultColumnname(Mark_Ds));
                    }

                }
                #endregion

                #region Taking  Discriptive mark

                if (publish == true && result == true)
                    if (LoadDiscriptiveGreadereports(ref Dataset_Dic))
                        Session["DataSetDictionary"] = Dataset_Dic;
                    else
                        Session["DataSetDictionary"] = Dataset_Dic;

                #endregion
            }
            catch (Exception ex)
            {
                ErrMsg = ErrMsg + ex.Message;
            }

        }

        #endregion

        #region descriptive parts
        /// <summary>
        /// this function collectio all discrptive information
        /// </summary>
        /// <param name="Dataset_Dic"></param>
        /// <returns></returns>
        private bool LoadDiscriptiveGreadereports(ref Dictionary<string, DataSet> Dataset_Dic)
        {
            bool valid = false;

            string Tablename = "tblcce_descriptive", Termcolumn = "";
            int Classid = int.Parse(Drp_ClassSelect.SelectedValue.ToString());
            int Termid = 0;
            int.TryParse(Drp_TermSelect.SelectedValue.ToString(), out Termid);
            string Termname = Drp_TermSelect.SelectedItem.Text.ToString();
            int Batchid = MyUser.CurrentBatchId;

            string sql = "SELECT tblcce_parts.Id,tblcce_parts.Description,tblcce_parts.FooterDesc from tblcce_parts";
            DataSet Ds_Part = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Ds_Part.Tables[0].Rows.Count == 0)
                valid = false;
            else
            {
                foreach (DataRow drpart in Ds_Part.Tables[0].Rows)
                {
                    //sql = "SELECT DISTINCT tblsubjects.Id,tblsubjects.subject_name from tblsubjects INNER JOIN tblcce_subjectskillmap  on tblsubjects.Id=tblcce_subjectskillmap.SubjectId WHERE tblcce_subjectskillmap.PartId=" + int.Parse(drpart["Id"].ToString()) + " AND tblcce_subjectskillmap.ClassId=" + Classid + "";
                    sql = "SELECT DISTINCT tblsubjects.Id,tblsubjects.subject_name from tblsubjects INNER JOIN tblcce_subjectskillmap  on tblsubjects.Id=tblcce_subjectskillmap.SubjectId WHERE tblcce_subjectskillmap.PartId=" + int.Parse(drpart["Id"].ToString()) + " AND tblcce_subjectskillmap.ClassId=" + Classid + "";
                    DataSet Ds_Subject = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (Ds_Subject.Tables[0].Rows.Count == 0)
                        valid = false;
                    else
                    {
                        string partname = "@@" + drpart["Description"].ToString() + "-desc@@";
                        partname = partname.Replace(" ", "");
                        partname = partname.ToLower();
                        Dataset_Dic.Add(partname, GetPartFooterMessageDataSet(drpart["FooterDesc"].ToString()));
                        foreach (DataRow drsubject in Ds_Subject.Tables[0].Rows)
                        {
                            if (Termid == 1)
                                Termcolumn = "Term1";
                            else if (Termid == 2)
                                Termcolumn = "Term2";
                            else
                                Termcolumn = "Term1,Term2,Term3";

                            sql = "SELECT DISTINCT(tblcce_subjectskills.SkillName) as Temp,tblcce_subjectskills.SkillName," + Tablename + ".DescriptiveIndicator," + Tablename + "." + Termcolumn + " from " + Tablename + " inner JOIN tblcce_subjectskills on " + Tablename + ".SkillId=tblcce_subjectskills.Id inner JOIN tblcce_subjectskillmap on tblcce_subjectskillmap.SkillId=tblcce_subjectskills.Id where " + Tablename + ".SubjectId=" + int.Parse(drsubject["Id"].ToString()) + " AND " + Tablename + ".StudentId=" + int.Parse(Session["StudId"].ToString()) + " AND tblcce_subjectskills.Id NOT IN('100') order by tblcce_subjectskillmap.SkillOrder";
                            string subjectname = "@@" + drsubject["subject_name"].ToString() + "@@";
                            subjectname = subjectname.Replace(" ", "");
                            subjectname = subjectname.ToLower();
                            Dataset_Dic.Add(subjectname, GetSubjectGrade(sql));
                        }
                        valid = true;
                    }

                }
            }
            return valid;
        }
        /// <summary>
        /// each discrptive skill dataset creating here  
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private DataSet GetSubjectGrade(string sql)
        {
            int i = 1;
            DataSet ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dr[0] = i;
                i++;
            }
            return SetDefaultColumnname(ds);
        }
        /// <summary>
        /// each part footer message  dataset creating here
        /// </summary>
        /// <param name="FooterDesc"></param>
        /// <returns></returns>
        private DataSet GetPartFooterMessageDataSet(string FooterDesc)
        {
            string[] _Message = Regex.Split(FooterDesc, "/nbsp");
            DataSet Obj_DS = new DataSet();
            DataTable Obj_Tbl = new DataTable();
            Obj_Tbl.Columns.Add("Column1", typeof(string));
            Obj_Tbl.Columns.Add("Column2", typeof(string));
            Obj_Tbl.Columns.Add("Column3", typeof(string));
            if (_Message[0].ToString() != "")
            {
                for (int i = 0; i < _Message.Count(); i++)
                {
                    Obj_Tbl.Rows.Add(_Message[i].ToString(), ":", _Message[i + 1].ToString());
                    i = i + 1;
                }
            }
            else
            {
                Obj_Tbl.Rows.Add("Data not found", ":", "Data not found");
            }
            Obj_DS.Tables.Add(Obj_Tbl);
            return Obj_DS;
        }
        /// <summary>
        /// set column name Like a column1 column2....
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private DataSet SetDefaultColumnname(DataSet result)
        {
            int k = 1;
            for (int j = 0; j < result.Tables[0].Columns.Count; j++)
            {
                if (j == 0)
                    result.Tables[0].Columns[j].ColumnName = "SINo";
                else
                {
                    result.Tables[0].Columns[j].ColumnName = "Column" + k;
                    k++;
                }
            }
            return result;
        }

        #endregion
      
        protected void Excel_Export_Click(object sender, EventArgs e)
        {
            string Err = "";
            try
            {
                int _ClassId = int.Parse(Drp_ClassSelect.SelectedValue);
                int _TermID = int.Parse(Drp_TermSelect.SelectedValue);
                int _GroupID = 0;
                int _ExamID = 0;
                string sql = "";
                double _ExamMaxMark = 0.0;
                DataTable gradmaster = null;
                if (_ClassId == 0)
                    Err = "Class name is not Found!";

                sql = "SELECT DISTINCT(tblcce_colconfig.Id),tblcce_colconfig.ExamName as ExamName from tblcce_colconfig inner JOIN tblcce_classgroupmap ON tblcce_classgroupmap.GroupId=tblcce_colconfig.GroupId where  tblcce_classgroupmap.ClassId=" + _ClassId + " AND tblcce_colconfig.TableName='tblcce_mark' and tblcce_colconfig.TermId=" + _TermID;
                DataSet _ExamDs = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (!(_ExamDs.Tables[0].Rows.Count > 0))
                {
                    Err = "Exam name is not found!";
                }
                else
                {
                    DataSet ExamDataSet = new DataSet();
                    DataTable Examdatatbl = new DataTable("Exam");
                    Examdatatbl.Columns.Add("StudentId", typeof(int));
                    Examdatatbl.Columns.Add("StudentRollNo", typeof(int));
                    Examdatatbl.Columns.Add("StudentName", typeof(string));
                    Examdatatbl.Columns.Add("ExamName", typeof(string));



                    #region Subject

                    sql = "select tblsubjects.Id as SubjectId,tblsubjects.subject_name as SubjectName from tblsubjects inner JOIN tblcce_classsubject ON tblcce_classsubject.subjectid=tblsubjects.Id where  tblcce_classsubject.classid=" + _ClassId + " order by tblcce_classsubject.classid";
                    DataSet _Subject = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (_Subject.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr_sub in _Subject.Tables[0].Rows)
                        {
                            Examdatatbl.Columns.Add(dr_sub["SubjectName"].ToString(), typeof(string));
                            //Examdatatbl.Columns.Add(dr_sub["SubjectName"].ToString() + " Grade", typeof(string));
                        }
                    }

                    #endregion



                    sql = "SELECT tblstudent.Id as StudentId,tblstudentclassmap.RollNo as StudentRollNo,tblstudent.StudentName as StudentName from tblstudent INNER JOIN tblstudentclassmap on  tblstudentclassmap.StudentId=tblstudent.Id where tblstudentclassmap.ClassId=" + _ClassId + " AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " ORDER by tblstudent.StudentName";
                    DataSet _StudentDs = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (_StudentDs.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _StudentDs.Tables[0].Rows)
                        {
                            bool IsFirst = true;
                            if (_ExamDs.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow drExam in _ExamDs.Tables[0].Rows)
                                {
                                    if (IsFirst)
                                    {
                                        Examdatatbl.Rows.Add(int.Parse(dr["StudentId"].ToString()), int.Parse(dr["StudentRollNo"].ToString()), dr["StudentName"].ToString(), drExam["ExamName"].ToString());
                                        IsFirst = false;
                                    }
                                    else
                                        Examdatatbl.Rows.Add(null, null, null, drExam["ExamName"].ToString());
                                }
                            }

                        }
                    }



                    ExamDataSet.Tables.Add(Examdatatbl);



                    string tablename = "";
                    int Row = 0;
                    int sudentid = 0;
                    foreach (DataRow dr1 in ExamDataSet.Tables[0].Rows)
                    {
                        int tempID;
                        int.TryParse(dr1[0].ToString(), out tempID);
                        if (tempID != 0)
                            sudentid = tempID;
                        if (_ExamDs.Tables[0].Rows.Count > 0)
                        {
                            string columnname = "";
                            foreach (DataRow drExam in _ExamDs.Tables[0].Rows)
                            {
                                if (drExam["ExamName"].ToString() == dr1["ExamName"].ToString())
                                {
                                    _ExamID = int.Parse(drExam["Id"].ToString());
                                    break;
                                }
                            }
                            sql = "SELECT tblcce_colconfig.TableName,tblcce_colconfig.ColName,tblcce_colconfig.GroupId,tblcce_colconfig.ExamMaxMark from tblcce_colconfig INNER JOIN tblcce_classgroupmap on tblcce_classgroupmap.GroupId=tblcce_colconfig.GroupId where tblcce_classgroupmap.ClassId=" + _ClassId + " and tblcce_colconfig.Id=" + _ExamID;
                            DataSet Columnds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                            if (Columnds.Tables[0].Rows.Count > 0)
                            {

                                foreach (DataRow dr in Columnds.Tables[0].Rows)
                                {
                                    tablename = dr[0].ToString();
                                    columnname = dr[1].ToString();
                                    int.TryParse(dr[2].ToString(), out _GroupID);
                                    double.TryParse(dr[3].ToString(), out _ExamMaxMark);
                                }
                                int column = 4;
                                foreach (DataRow dr2 in _Subject.Tables[0].Rows)
                                {
                                    int subid = 0;
                                    string submark = "0";
                                    int.TryParse(dr2[0].ToString(), out subid);
                                    sql = "SELECT " + tablename + "." + columnname + " from " + tablename + " inner join tblstudentclassmap ON tblstudentclassmap.StudentId=tblcce_mark.StudentId where tblstudentclassmap.ClassId=" + _ClassId + " AND tblcce_mark.SubjectId=" + subid + " AND tblcce_mark.StudentId=" + sudentid;
                                    DataSet submarkds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                                    if (submarkds.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow _markdr in submarkds.Tables[0].Rows)
                                        {
                                            submark = _markdr[0].ToString();
                                        }
                                    }
                                    ExamDataSet.Tables[0].Rows[Row][column] = submark;
                                    column++;
                                }
                            }
                        }
                        Row++;
                    }

                    ExamDataSet.Tables[0].Columns.RemoveAt(0);
                    //ExamDataSet.Tables.Add(gradmaster);

                    if (ExamDataSet.Tables[0].Rows.Count > 0)
                    {
                        string FileName = Drp_ClassSelect.SelectedItem.Text + ".xls";
                        if (!WinEr.ExcelUtility.ExportDataSetToExcel(ExamDataSet, FileName))
                        {

                        }

                    }

                }
            }
            catch (Exception ex)
            {
                WC_MessageBox.ShowMssage(ex.Message);
            }    
        }

    }
}


//GridView gv = new GridView();
//gv.DataSource = Examdatatbl;
//gv.DataBind();
//for (int i = gv.Rows.Count - 1; i > 0; i--)
//{
//    GridViewRow row = gv.Rows[i];
//    GridViewRow previousRow = gv.Rows[i - 1];
//    for (int j = 0; j < row.Cells.Count; j++)
//    {
//        if (row.Cells[j].Text == previousRow.Cells[j].Text)
//        {
//            if (previousRow.Cells[j].RowSpan == 0)
//            {
//                if (row.Cells[j].RowSpan == 0)
//                {
//                    previousRow.Cells[j].RowSpan += 2;
//                }
//                else
//                {
//                    previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
//                }
//                row.Cells[j].Visible = false;
//            }
//        }
//    }
//}
