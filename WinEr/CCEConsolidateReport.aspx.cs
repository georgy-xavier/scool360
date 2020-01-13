using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WinBase;
using System.Configuration;
using System.Xml.Serialization;
using WC.PDF.Document;
using System.Xml;
using System.IO;
using System.Text;
using WC.PdfDocumentClass;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Pdf;
using MigraDoc.Rendering;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using WebChart;
using System.Data.Odbc;

namespace WinEr
{
    public partial class CCEConsolidateReport : System.Web.UI.Page
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
                    divgrid.Visible = false;
                    Drp_ClassSelect.Focus();
                }
            }

        }

        #region default event

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
            if (Drp_ClassSelect.SelectedItem.Value == "0")
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
            string sql = "select tblcce_configmanager.reportsstatus from tblcce_configmanager where tblcce_configmanager.Termid in (1,2) AND tblcce_configmanager.Classid=" + int.Parse(Drp_ClassSelect.SelectedValue.ToString());
            DataSet _publishds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_publishds.Tables[0].Rows.Count > 0 && _publishds != null)
                foreach (DataRow dr in _publishds.Tables[0].Rows)
                    if (dr[0].ToString() == "2")
                    {
                        Estatus = true;
                        break;
                    }
            return Estatus;
        }

        /// <summary>
        /// student list load function 
        /// </summary>
        private void load_Grid()
        {
            int _ClassId = int.Parse(Drp_ClassSelect.SelectedValue);
            string sql = "SELECT tblstudent.Id as StudentId,tblstudent.RollNo as StudentRollno,tblstudent.StudentName as StudentName from tblstudent INNER JOIN tblstudentclassmap on  tblstudentclassmap.StudentId=tblstudent.Id where tblstudentclassmap.ClassId=" + _ClassId + " AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId+ " ORDER by tblstudentclassmap.RollNo";
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
                div1.Visible = false;
                divgrid.Visible = true;

            }
            else
            {
                Grd_CCEstudent.DataSource = null;
                Grd_CCEstudent.DataBind();
                div1.Visible = true;
                Lbl_stuTotal.Text = "0"; 
                divgrid.Visible = false;
                WC_MessageBox.ShowMssage("NO Student Found!");
         
            }
        }

        protected void Btn_cancel_Click(object sender, EventArgs e)
        {
            divgrid.Visible = false;
            Load_ClassDropdown();
            div1.Visible = true;
            Drp_ClassSelect.Focus();
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

        #endregion

        #region Taking student mark records

        private void GetStudentExamPerformanace(out string ErrMsg)
        {
            ErrMsg = "";
            try
            {
                Dictionary<string, DataSet> Dataset_Dic = new Dictionary<string, DataSet>();

                bool publish = true;
                bool result = true;
                string quryStr = "";
                int StudentId = int.Parse(Session["StudId"].ToString());
                string sql = "";

                #region Marksubject Result

                sql = "SELECT DISTINCT(tblcce_colconfig.ExamName),tblcce_colconfig.TableName,tblcce_colconfig.ColName from tblcce_colconfig INNER JOIN tblcce_classgroupmap on tblcce_classgroupmap.GroupId=tblcce_colconfig.GroupId inner join tblstudentclassmap on tblstudentclassmap.ClassId=tblcce_classgroupmap.ClassId where tblcce_colconfig.TableName='tblcce_result' AND tblstudentclassmap.StudentId=" + StudentId;
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
                        quryStr = quryStr + "," + C_dr[1].ToString() + "." + C_dr[2].ToString() + " as " + "`"+replacestr+"`";
                    }
                    Mark_Ds = null;
                    sql = "SELECT tblcce_classsubject.SubjectOrder as SINo,tblsubjects.subject_name as SUBJECT " + quryStr + " from tblsubjects INNER join tblcce_classsubject on tblcce_classsubject.subjectid=tblsubjects.Id inner join tblstudentclassmap on tblstudentclassmap.ClassId=tblcce_classsubject.classid inner join tblcce_result on tblcce_result.StudentId=tblstudentclassmap.StudentId AND tblcce_result.SubjectId=tblcce_classsubject.subjectid where tblstudentclassmap.StudentId=" + StudentId + "  ORDER BY tblcce_classsubject.SubjectOrder";
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
                                if (M_dr[i].ToString() == "-" || M_dr[i].ToString() == "WITHHELD")
                                    result = false;
                            }
                        }
                    }

                    if (result == true && publish == true)
                    {
                        Dataset_Dic.Add("@@AcademicPerformance@@", SetDefaultColumnname(Mark_Ds));
                    }

                }
                #endregion

                #region Taking student Discriptive marks

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

        private bool LoadDiscriptiveGreadereports(ref Dictionary<string, DataSet> Dataset_Dic)
        {
            bool valid = false;

            string Tablename = "tblcce_descriptive";
            int Classid = int.Parse(Drp_ClassSelect.SelectedValue.ToString());
            int Batchid = MyUser.CurrentBatchId;

            string sql = "SELECT tblcce_parts.Id,tblcce_parts.Description,tblcce_parts.FooterDesc from tblcce_parts";
            DataSet Ds_Part = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Ds_Part.Tables[0].Rows.Count == 0)
                valid = false;
            else
            {
                foreach (DataRow drpart in Ds_Part.Tables[0].Rows)
                {
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
                            sql = "SELECT DISTINCT(tblcce_subjectskills.SkillName) as Temp,tblcce_subjectskills.SkillName," + Tablename + ".DescriptiveIndicator," + Tablename + ".Term1," + Tablename + ".Term2," + Tablename + ".Term3 from " + Tablename + " inner JOIN tblcce_subjectskills on " + Tablename + ".SkillId=tblcce_subjectskills.Id inner JOIN tblcce_subjectskillmap on tblcce_subjectskillmap.SkillId=tblcce_subjectskills.Id where " + Tablename + ".SubjectId=" + int.Parse(drsubject["Id"].ToString()) + " AND " + Tablename + ".StudentId=" + int.Parse(Session["StudId"].ToString()) + " AND tblcce_subjectskills.Id NOT IN('100') order by tblcce_subjectskillmap.SkillOrder";
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

        private DataSet SetDefaultColumnname(DataSet result)//set column name Like a column1 column2....
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

        /// <summary>
        /// Generation event function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Generate_Click(object sender, EventArgs e)
        {
            string _Errmsg = "";
            try
            {
                bool NeedGraph = CheckGraphConfig();
                DataSet classTopMark = null;
                DataSet classStudentMark = null;
                float examMaxMark = 100;

                int Chkcount = ValidationCheckbox();
                if (Chkcount <= 0)
                    _Errmsg = "Please select students!";
                else
                {
                    #region reports

                    string _PhysicalPath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath("")) + "\\PDF_Files\\"; //Path for PDF creation
                    string _DefaultImgpath = Server.MapPath("") + "\\Pics";
                    string _TempImgpath = Server.MapPath("") + "\\ThumbnailImages";
                    
                    Document PDFdocument = new Document();//PDF dll class
                    PageSetup pgSetup = PDFdocument.DefaultPageSetup;//PDF dll class
                    string xmlstr = ReadXMLfileFromDataBase(int.Parse(Drp_ClassSelect.SelectedValue.ToString()));//Reading xml file from database

                    if (xmlstr == "")
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
                                    int.TryParse("3", out objCCE.TermId);
                                    objCCE.TermName = "consolidatereport";
                                    objCCE.m_DefaultImgpath = _DefaultImgpath;
                                    objCCE.m_TempImgpath = _TempImgpath;
                                    if (NeedGraph)
                                        objCCE.m_PerformanceChartURI = GetStudentPerformanceChart(classTopMark, classStudentMark, examMaxMark);

                                    objCCE.xmlstring = xmlstr;
                                    if (!objCCE.Exporttopdfdocument(ref PDFdocument, pgSetup, out _Errmsg))//calling this function creation selected student PDFdocument 
                                        _Errmsg = "Please try later!";
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

        private string GetStudentPerformanceChart(DataSet classMaxMark, DataSet classStudetnMark, float examMaxMark)
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

        private DataSet GetMaxMark(ref float ExamMaxMark, ref DataSet studentMark_Ds)
        {
            DataSet topMark_Ds = new DataSet();
            studentMark_Ds = new DataSet();
            //string sql = "SELECT DISTINCT(tblcce_colconfig.ExamName),tblcce_colconfig.TableName,tblcce_colconfig.ColName,tblcce_colconfig.ExamMaxMark from tblcce_colconfig INNER JOIN tblcce_classgroupmap on tblcce_classgroupmap.GroupId=tblcce_colconfig.GroupId inner join tblstudentclassmap on tblstudentclassmap.ClassId=tblcce_classgroupmap.ClassId where tblstudentclassmap.StudentId=" + int.Parse(Session["StudId"].ToString()) + " and tblcce_colconfig.TermId=" + int.Parse(Drp_TermSelect.SelectedValue.ToString()) + " and tblcce_colconfig.TableName='tblcce_mark' order by tblcce_colconfig.Id DESC";
            string sql = "SELECT DISTINCT(tblcce_colconfig.ExamName),tblcce_colconfig.TableName,tblcce_colconfig.ColName,tblcce_colconfig.ExamMaxMark from tblcce_colconfig INNER JOIN tblcce_classgroupmap on tblcce_classgroupmap.GroupId=tblcce_colconfig.GroupId inner join tblstudentclassmap on tblstudentclassmap.ClassId=tblcce_classgroupmap.ClassId where tblstudentclassmap.StudentId=" + int.Parse(Session["StudId"].ToString()) + " and tblcce_colconfig.TableName='tblcce_mark' order by tblcce_colconfig.Id DESC";
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

        private bool CheckGraphConfig()
        {
            bool _needGraph = false;
            string sql = "select * from tblconfiguration where id=83";
            OdbcDataReader Config_Dr = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (Config_Dr.HasRows)
            {
                if (Config_Dr.GetValue(3).ToString() == "1")
                    _needGraph = true;
            }
            return _needGraph;
        }

        private string ReadXMLfileFromDataBase(int classid)
        {
            string xmstring = "";
            //string sql = "SELECT tblcce_classgroup.TermXMLFile,tblcce_classgroup.ConsoldateXMLFile from tblcce_classgroup inner join tblcce_classgroupmap ON tblcce_classgroupmap.GroupId=tblcce_classgroup.Id where tblcce_classgroupmap.ClassId=" + classid + " and tblcce_classgroupmap.Termid=3";
            string sql = "SELECT tblcce_classgroup.TermXMLFile,tblcce_classgroup.ConsoldateXMLFile from tblcce_classgroup inner join tblcce_classgroupmap ON tblcce_classgroupmap.GroupId=tblcce_classgroup.Id where tblcce_classgroupmap.ClassId=" + classid;
            DataSet Ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Ds != null || Ds.Tables[0] != null || Ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Ds.Tables[0].Rows)
                {
                    xmstring = dr[1].ToString(); 
                }
            }
            return xmstring;
        }

    }
}




