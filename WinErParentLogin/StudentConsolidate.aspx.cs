using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Pdf;
using MigraDoc.Rendering;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;

namespace WinErParentLogin
{
    public partial class StudentConsolidate : System.Web.UI.Page
    {
        private ParentInfoClass MyParentInfo;
        private SchoolClass objSchool = null;

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

                    Session["batch"] = "currentbatch";
                    Label MyHeader = (Label)this.Master.FindControl("Lbl_PageHeader");
                    MyHeader.Text = "Student Performance";
                    DisvisibleUI();
                }
            }
        }

        #region student current report

        /// <summary>
        /// this function enabling default UI
        /// </summary>
        /// <param name="Batch"></param>
        /// <returns></returns>
        private void DisvisibleUI()
        {
            LoadTermDrp();
            this.Img_Search.Visible = false;
            this.Lnk_PreviousPerformance.Visible = false;
            if (CheckStudentHistory())
            {
                this.Img_Search.Visible = true;
                this.Lnk_PreviousPerformance.Visible = true;
            }
            this.PriviousbatchDiv.Visible = false;
            this.ResultGridDiv.Visible = false;
            this.ResultDiv.Visible = false;
            this.HeaderstrDiv.Visible = true;
            GetStudentExamPerformanace("currentbatch");

        }

        /// <summary>
        /// this function loading term dropdown
        /// </summary>
        /// <param name="Batch"></param>
        /// <returns></returns>
        private void LoadTermDrp()
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);

            Drp_term.Items.Clear();
            string sql = "SELECT tblcce_term.Id as Id,tblcce_term.TermName as TermName FROM tblcce_term";
            DataSet Ds_term = _mysqlObj.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (Ds_term != null && Ds_term.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drcls in Ds_term.Tables[0].Rows)
                {
                    li = new ListItem(drcls["TermName"].ToString(), drcls["Id"].ToString());
                    Drp_term.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("NO Data Found", "0");
                Drp_term.Items.Add(li);
            }
        }

        protected void Drp_Term_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetStudentExamPerformanace("currentbatch");
        }

        /// <summary>
        /// this function will return student selected term result published or not
        /// </summary>
        /// <param name="Batch"></param>
        /// <returns></returns>
        private bool CheckResultpublish(string Batch)
        {

            bool publish = false;
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);

            string sql = "select tblcce_configmanager.publishstatus from tblcce_configmanager where tblcce_configmanager.Termid=" + int.Parse(Drp_term.SelectedValue.ToString()) + " AND tblcce_configmanager.Classid=" + MyParentInfo.CLASSID;
            DataSet _publishds = _mysqlObj.ExecuteQueryReturnDataSet(sql);
            if(_publishds.Tables[0].Rows.Count>0 && _publishds!=null)
                foreach (DataRow dr in _publishds.Tables[0].Rows)
                {
                    if (dr[0].ToString() == "1") 
                        publish = true;
                }
            if (Batch == "historybatch")
                publish = true;
            return publish;
        }

        #endregion

        #region student history

        /// <summary>
        /// this function checking for student have exam history or not
        /// </summary>
        /// <returns></returns>
        private bool CheckStudentHistory()
        {

            bool history = false;
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            int StudentId = MyParentInfo.StudentId;
            string sql = "SELECT DISTINCT tblcce_result_history.BatchId from tblcce_result_history where tblcce_result_history.StudentId=" + StudentId;
            DataSet ds = _mysqlObj.ExecuteQueryReturnDataSet(sql);
            if(ds.Tables.Count>0)
                if (ds.Tables[0].Rows.Count > 0 && ds != null)
                    history = true;
            return history;

        }

        /// <summary>
        /// after clicking this event conrol will show with history report page design
        /// </summary>
        /// <param name="StudentId"></param>
        protected void Lnk_PreviousPerformance_Click(object sender, EventArgs e)
        {
            this.HeaderstrDiv.Visible = false;
            this.ResultGridDiv.Visible = false;
            this.ResultGridDiv.Visible = false;
            this.PriviousbatchDiv.Visible = true;
            int StudentId = MyParentInfo.StudentId;
            LoadBatchDropdown(StudentId);
            LoadTermBatchDrp();
            Session["batch"] = "historybatch";
            GetStudentExamPerformanace("historybatch");
        }

        /// <summary>
        /// this function loading with term dropdown 
        /// </summary>
        /// <param name="StudentId"></param>
        private void LoadTermBatchDrp()
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);

            Drp_batchTerm.Items.Clear();
            string sql = "SELECT tblcce_term.Id as Id,tblcce_term.TermName as TermName FROM tblcce_term";
            DataSet Ds_term = _mysqlObj.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (Ds_term != null && Ds_term.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drcls in Ds_term.Tables[0].Rows)
                {
                    li = new ListItem(drcls["TermName"].ToString(), drcls["Id"].ToString());
                    Drp_batchTerm.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("NO Data Found", "0");
                Drp_batchTerm.Items.Add(li);
            }
        }
        /// <summary>
        /// this function loading with batch dropdown 
        /// </summary>
        /// <param name="StudentId"></param>
        private void LoadBatchDropdown(int StudentId)
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);

            Drp_batch.Items.Clear();
            string sql = "SELECT DISTINCT tblbatch.Id,tblbatch.BatchName from tblbatch INNER JOIN tblcce_result_history on tblcce_result_history.BatchId=tblbatch.Id where tblcce_result_history.StudentId=" + StudentId;
            DataSet ds = _mysqlObj.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (ds.Tables[0].Rows.Count > 0 && ds != null)
            {
                foreach (DataRow drcls in ds.Tables[0].Rows)
                {
                    li = new ListItem(drcls["BatchName"].ToString(), drcls["Id"].ToString());
                    Drp_batch.Items.Add(li);
                }
            }
        }
        /// <summary>
        /// after clicking this event control will come to current year page desing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Lnk_back_Click(object sender, EventArgs e)
        {
            DisvisibleUI();
            Session["batch"] = "currentbatch";
            GetStudentExamPerformanace("currentbatch");

        }

        protected void Drp_batch_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GetStudentExamPerformanace("historybatch");
        }
    
        #endregion

        #region Taking student mark records
        /// <summary>
        /// collecting students marks and discriptive  dataset
        /// </summary>
        /// <param name="Batch"></param>
        private void GetStudentExamPerformanace(string Batch)
        {
            Lbl_graedesubject.Visible =false;
            grdResult_marksubject.DataSource = null;
            grdResult_marksubject.DataBind();
            grdResult_gradesubject.DataSource = null;
            grdResult_gradesubject.DataBind();
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            Dictionary<string, DataSet> Dataset_Dic = new Dictionary<string, DataSet>();

            string ErrMsg = "";
            bool publish = true;
            bool result = true;
            try
            {
                if (!CheckResultpublish(Batch))
                    publish = false;
                else
                {
                    string quryStr = "";
                    int StudentId = MyParentInfo.StudentId;
                    string sql = "";

                    #region Taking student marks from database
                    string _consoldate = "";
                    if (Batch == "historybatch")
                    {
                        _consoldate = " and tblcce_colconfig.TermId=" + int.Parse(Drp_batchTerm.SelectedValue.ToString());
                        if (Drp_batchTerm.SelectedItem.Text.ToString().ToLower() == "consolidate")
                            _consoldate = " ";
                        sql = "SELECT DISTINCT(tblcce_colconfig.ExamName),tblcce_colconfig.TableName,tblcce_colconfig.ColName from tblcce_colconfig INNER JOIN tblcce_classgroupmap on tblcce_classgroupmap.GroupId=tblcce_colconfig.GroupId inner join tblstudentclassmap_history on tblstudentclassmap_history.ClassId=tblcce_classgroupmap.ClassId where tblcce_colconfig.TableName='tblcce_result' AND tblstudentclassmap_history.StudentId=" + StudentId + " " + _consoldate + " and tblstudentclassmap_history.BatchId=" + int.Parse(Drp_batch.SelectedValue.ToString());
                    }
                    else
                    {
                        _consoldate = " and tblcce_colconfig.TermId=" + int.Parse(Drp_term.SelectedValue.ToString());
                        if (Drp_term.SelectedItem.Text.ToString().ToLower() == "consolidate")
                            _consoldate = " ";
                        sql = "SELECT DISTINCT(tblcce_colconfig.ExamName),tblcce_colconfig.TableName,tblcce_colconfig.ColName from tblcce_colconfig INNER JOIN tblcce_classgroupmap on tblcce_classgroupmap.GroupId=tblcce_colconfig.GroupId inner join tblstudentclassmap on tblstudentclassmap.ClassId=tblcce_classgroupmap.ClassId where  tblstudentclassmap.StudentId=" + StudentId + " " + _consoldate;
                    }
                    DataSet Mark_Ds = _mysqlObj.ExecuteQueryReturnDataSet(sql);
                    if (Mark_Ds.Tables[0].Rows.Count == 0)
                        publish = false;
                    else
                    {
                        foreach (DataRow C_dr in Mark_Ds.Tables[0].Rows)
                        {
                            string replacestr = C_dr[0].ToString();
  
                            if (Batch == "historybatch")
                                quryStr = quryStr + ",tblcce_result_history." + C_dr[2].ToString() + " as " + "`" +replacestr+"`";
                            else
                                quryStr = quryStr + "," + C_dr[1].ToString() + "." + C_dr[2].ToString() + " as " + "`" + replacestr + "`";
                        }
                        Mark_Ds = null;

                        if (Batch == "historybatch")
                            sql = "SELECT tblcce_classsubject.SubjectOrder as SINo,tblsubjects.subject_name as SUBJECT " + quryStr + " from tblsubjects right join tblcce_classsubject on tblcce_classsubject.subjectid=tblsubjects.Id right join tblstudentclassmap_history on tblstudentclassmap_history.ClassId=tblcce_classsubject.classid right join tblcce_result_history on tblcce_result_history.StudentId=tblstudentclassmap_history.StudentId AND tblcce_result_history.SubjectId=tblcce_classsubject.subjectid where tblstudentclassmap_history.StudentId=" + StudentId + "  and tblcce_result_history.BatchId=" + int.Parse(Drp_batch.SelectedValue.ToString()) + "  ORDER BY tblcce_classsubject.SubjectOrder";
                        else
                            sql = "SELECT tblcce_classsubject.SubjectOrder as SINo,tblsubjects.subject_name as SUBJECT " + quryStr + " from tblsubjects right join tblcce_classsubject on tblcce_classsubject.subjectid=tblsubjects.Id right join tblstudentclassmap on tblstudentclassmap.ClassId=tblcce_classsubject.classid right join tblcce_result on tblcce_result.StudentId=tblstudentclassmap.StudentId AND tblcce_result.SubjectId=tblcce_classsubject.subjectid RIGHT join tblcce_mark on tblcce_mark.StudentId=tblstudentclassmap.StudentId AND tblcce_mark.SubjectId=tblcce_classsubject.subjectid where tblstudentclassmap.StudentId=" + StudentId + "  ORDER BY tblcce_classsubject.SubjectOrder";

                        Mark_Ds = _mysqlObj.ExecuteQueryReturnDataSet(sql);
                        if (Mark_Ds.Tables[0].Rows.Count == 0)
                            publish = false;
                        else
                        {
                            foreach (DataRow M_dr in Mark_Ds.Tables[0].Rows)
                            {
                                for (int i = 1; i < Mark_Ds.Tables[0].Columns.Count; i++)
                                {
                                    if (M_dr[i].ToString() == "-" || M_dr[i].ToString() == "AB" || M_dr[i].ToString() == "WITHHELD")
                                        result = false;
                                }
                            }
                        }

                        if (result==true&&publish==true)
                        {
                       
                            grdResult_marksubject.DataSource = Mark_Ds.Tables[0];
                            grdResult_marksubject.DataBind();
                            GridViewRow dr = grdResult_marksubject.HeaderRow;
                            dr.Cells[0].Visible = false;
                            foreach (GridViewRow grd in grdResult_marksubject.Rows)
                            {
                                grd.Cells[0].Visible = false;
                            }
                            Dataset_Dic.Add("@@AcademicPerformance@@",SetDefaultColumnname(Mark_Ds));



                           
                        }

                    }
                    #endregion

                    #region Taking student discriptive marks

                    if (publish == true && result == true)
                        if (LoadDiscriptiveGreadereports(ref Dataset_Dic))
                        {
                            if (Dataset_Dic.ContainsKey("@@gradesubjects@@"))
                            {
                                Lbl_graedesubject.Visible = true;
                                DataSet Grdds = new DataSet();
                                Grdds = Dataset_Dic["@@gradesubjects@@"];
                                grdResult_gradesubject.DataSource = Grdds;
                                grdResult_gradesubject.DataBind();

                                #region set column

                                GridViewRow dr = grdResult_gradesubject.HeaderRow;
                                dr.Cells[0].Visible = false;
                                dr.Cells[1].Text = "SUBJECT";
                                dr.Cells[2].Visible = false;



                                DropDownList obj = new DropDownList();
                                obj.Items.Clear();
                                foreach (ListItem li in Drp_batchTerm.Items)
                                {
                                    obj.Items.Add(li);
                                }
                                obj.SelectedValue = Drp_batchTerm.SelectedValue;
                                if (Batch == "currentbatch")
                                {
                                    obj.Items.Clear();
                                    foreach (ListItem li in Drp_term.Items)
                                    {
                                        obj.Items.Add(li);
                                    }
                                    obj.SelectedValue = Drp_term.SelectedValue;
                                }
                                obj.DataBind();

                                #region grid row
                                if (obj.SelectedValue.ToString() == "1" || obj.SelectedValue.ToString() == "2")
                                {
                                    dr.Cells[3].Text = "GRADE";
                                    dr.Cells[3].Visible = true;
                                }
                                else
                                {
                                    dr.Cells[3].Text = "TERM1";
                                    dr.Cells[4].Text = "TERM2";
                                    dr.Cells[5].Text = "TERM3";
                                    dr.Cells[3].Visible = true;
                                    dr.Cells[4].Visible = true;
                                    dr.Cells[5].Visible = true;
                                }
                                #endregion

                                #region grid column
                                foreach (GridViewRow grd in grdResult_gradesubject.Rows)
                                {
                                    grd.Cells[0].Visible = false;
                                    grd.Cells[2].Visible = false;

                                    if (obj.SelectedValue.ToString() == "1" || obj.SelectedValue.ToString() == "2")
                                        grd.Cells[3].Visible = true;
                                    else
                                    {
                                        grd.Cells[3].Visible = true;
                                        grd.Cells[4].Visible = true;
                                        grd.Cells[5].Visible = true;
                                    }
                                }
                                #endregion

                                #endregion

                            }
                            
                        }



                    #endregion

                }

                #region display student results
                if (!publish)
                {
                    this.ResultGridDiv.Visible = false;
                    this.ResultDiv.Visible = true;
                    Lbl_resultmag.ForeColor = System.Drawing.Color.Red;
                    Session["color"] = "Red";
                    Lbl_resultmag.Text = "Selected term result is not PUBLISHED!";
                    
                }
                else
                {
                    if (!result)
                    {
                        this.ResultGridDiv.Visible = false;
                        this.ResultDiv.Visible = false;
                        Lbl_resultmag.ForeColor = System.Drawing.Color.Red;
                        Session["color"] = "Red";
                        Lbl_resultmag.Text = "Your student is WITHHELD.";
                    }
                    else
                    {
                        this.ResultGridDiv.Visible = true;
                        this.ResultDiv.Visible = false;
                        Lbl_resultmag.ForeColor = System.Drawing.Color.Green;
                        Session["color"] = "Green";
                        Lbl_resultmag.Text = "Congratulations !! Your student promoted to next class.";
                        Session["DataSetDictionary"] = Dataset_Dic;
                    }

                }
                #endregion

            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
            }
            if (ErrMsg != "")
                WC_MessageBox.ShowMssage(ErrMsg);
        }

        #endregion
       
        #region Export pddf report
        /// <summary>
        /// this event export pdf report card
        /// creating pdf
        /// after creating pdf it will delete all the pdf files liks a student image and school log and pdf desgion image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Img_export_Pdf_Click(object sender, ImageClickEventArgs e)
        {
            string _Errmsg = "";
            try
            {
                MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
                ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);


                string _PhysicalPath = WinerUtlity.GetParentLoginAbsoluteFilePath(MyParentInfo.SchoolObject, Server.MapPath("")) + "\\PDF_Files\\" ;
                Dictionary<string, DataSet> Dataset_Dic = new Dictionary<string, DataSet>();
                Dataset_Dic = (Dictionary<string, DataSet>)Session["DataSetDictionary"];
                CCEUtility objCCE = new CCEUtility(_mysqlObj, MyParentInfo, Dataset_Dic);
                objCCE.m_PdfPysicalPath = _PhysicalPath;
                objCCE.BatchId = MyParentInfo.CurrentBatchId;
                objCCE.BatchName = MyParentInfo.CurrentBatchName;
                objCCE.ClassId = MyParentInfo.CLASSID;
                objCCE.ClassName = MyParentInfo.CLASSNAME;
                int.TryParse(Drp_term.SelectedValue.ToString(), out objCCE.TermId);
                objCCE.TermName = Drp_term.SelectedItem.Text.ToString();
                objCCE.Rollno = MyParentInfo.RollNO;


                if (Session["batch"].ToString() == "historybatch")
                {
                    int.TryParse(Drp_batch.SelectedValue.ToString(), out objCCE.BatchId);
                    objCCE.BatchName = Drp_batch.SelectedItem.Text.ToString();
                    LoadHistoryClass(out objCCE.ClassId, out objCCE.ClassName,out objCCE.Rollno, MyParentInfo.StudentId);
                    int.TryParse(Drp_batchTerm.SelectedValue.ToString(), out objCCE.TermId);
                    objCCE.TermName = Drp_batchTerm.SelectedItem.Text.ToString();
                }
                Document PDFdocument = null;
                objCCE.xmlstring = ReadXMLfileFromDataBase(objCCE.ClassId,objCCE.TermId,objCCE.TermName);
                if (!objCCE.Exporttopdfdocument(out PDFdocument, out _Errmsg))
                    _Errmsg = "Please try later!";
                else
                {
                    #region create Pdf

                    string PdfPath = _PhysicalPath + "StuMarkReport_" + objCCE.ClassName + ".pdf";
                    const PdfFontEmbedding embedding = PdfFontEmbedding.Always;
                    const bool unicode = false;
                    PdfDocumentRenderer pdfrenderer = new PdfDocumentRenderer(unicode, embedding);
                    pdfrenderer.Document = PDFdocument;
                    pdfrenderer.RenderDocument();
                    pdfrenderer.PdfDocument.Save(PdfPath);
                    //Process.Start(PdfPath);
                    ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "null", "window.open(\"OpenPdfPage.aspx?PdfName=StuMarkReport_" + objCCE.ClassName + ".pdf\",'Info','status=1, width=1000, height=700,,resizable=1,scrollbars=1');", true);

                    #endregion

                    #region delete all files
                    string[] filePaths = Directory.GetFiles(_PhysicalPath + "TempImg");
                    foreach (string filePath in filePaths)
                        File.Delete(filePath);
                    #endregion
                }
            }

            catch (Exception Ex)
            {
                _Errmsg = Ex.Message;
            }

            if (_Errmsg != "")
                WC_MessageBox.ShowMssage(_Errmsg);
        }

        /// <summary>
        /// this function will load history class dropdown
        /// </summary>
        /// <param name="ClassId"></param>
        /// <param name="ClassName"></param>
        /// <param name="Rollno"></param>
        /// <param name="StudentId"></param>
        private void LoadHistoryClass(out int ClassId, out string ClassName, out string Rollno, int StudentId)
        {
            ClassId = 0;
            ClassName = "";
            Rollno = "";
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            string sql = "SELECT tblclass.Id,tblclass.ClassName,tblstudentclassmap_history.RollNo from tblclass inner join tblstudentclassmap_history on tblstudentclassmap_history.ClassId=tblclass.Id where tblstudentclassmap_history.BatchId=" + int.Parse(Drp_batch.SelectedValue.ToString()) + " and tblstudentclassmap_history.StudentId=" + StudentId;
            DataSet myds = _mysqlObj.ExecuteQueryReturnDataSet(sql);
            if(myds.Tables[0].Rows.Count>0)
            {
                foreach (DataRow dr in myds.Tables[0].Rows)
                {
                    int.TryParse(dr[0].ToString(), out ClassId);
                    ClassName = dr[1].ToString();
                    Rollno = dr[2].ToString();
                }
            }
        }
        /// <summary>
        /// this function will retun xml file string 
        /// </summary>
        /// <param name="classid"></param>
        /// <param name="Termid"></param>
        /// <param name="TermName"></param>
        /// <returns></returns>
        private string ReadXMLfileFromDataBase(int classid,int Termid, string TermName)
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            string xmstring = "Null";
            string sql = "SELECT tblcce_classgroup.TermXMLFile,tblcce_classgroup.ConsoldateXMLFile from tblcce_classgroup inner join tblcce_classgroupmap ON tblcce_classgroupmap.GroupId=tblcce_classgroup.Id where tblcce_classgroupmap.ClassId=" + classid + " and tblcce_classgroupmap.Termid=" + Termid;
            DataSet Ds = _mysqlObj.ExecuteQueryReturnDataSet(sql);
            if (Ds != null || Ds.Tables[0] != null || Ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Ds.Tables[0].Rows)
                {
                    if (TermName.ToLower().Replace(" ", "").ToLower() == "consolidate")
                        xmstring = dr[1].ToString();
                    else
                        xmstring = dr[0].ToString();
                }
            }
            return xmstring;
        }

        #endregion

        #region descriptive parts
        /// <summary>
        /// this function will return student discriptive dataset
        /// </summary>
        /// <param name="Dataset_Dic"></param>
        /// <returns></returns>
        private bool LoadDiscriptiveGreadereports(ref Dictionary<string, DataSet> Dataset_Dic)
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            bool valid = false;
            string Tablename = "tblcce_descriptive";
            string Classid="("+MyParentInfo.CLASSID+")";
            int Termid = 0;
            int.TryParse(Drp_term.SelectedValue.ToString(), out Termid);
            string Termname = Drp_term.SelectedItem.Text.ToString();
            int Batchid = MyParentInfo.CurrentBatchId;
            string Batchidstr = "";

            if (Session["Batch"].ToString() == "historybatch")
            {
                int.TryParse(Drp_batchTerm.SelectedValue.ToString(), out Termid);
                Termname = Drp_batchTerm.SelectedItem.Text.ToString();
                int.TryParse(Drp_batch.SelectedValue.ToString(), out Batchid);
                Tablename = "tblcce_descriptive_history";
                Classid="(SELECT tblstudentclassmap_history.ClassId from tblstudentclassmap_history where tblstudentclassmap_history.BatchId="+Batchid+" AND tblstudentclassmap_history.StudentId="+MyParentInfo.StudentId+")";
                Batchidstr=" AND tblcce_descriptive_history.BatchId=" + Batchid;
            }

            string sql = "SELECT tblcce_parts.Id,tblcce_parts.Description,tblcce_parts.FooterDesc from tblcce_parts";
            DataSet Ds_Part =_mysqlObj.ExecuteQueryReturnDataSet(sql);
            if (Ds_Part.Tables[0].Rows.Count == 0)
                valid = false;
            else
            {
                foreach (DataRow drpart in Ds_Part.Tables[0].Rows)
                {
                    sql = "SELECT DISTINCT tblsubjects.Id,tblsubjects.subject_name from tblsubjects INNER JOIN tblcce_subjectskillmap  on tblsubjects.Id=tblcce_subjectskillmap.SubjectId WHERE tblcce_subjectskillmap.PartId=" + int.Parse(drpart["Id"].ToString()) + " AND tblcce_subjectskillmap.ClassId IN " + Classid + "";
                    DataSet Ds_Subject = _mysqlObj.ExecuteQueryReturnDataSet(sql);
                    if (Ds_Subject.Tables[0].Rows.Count == 0)
                        valid = false;
                    else
                    {
                        string partname = "@@" + drpart["Description"].ToString() + "-desc@@";
                        partname = partname.Replace(" ","");
                        partname = partname.ToLower();
                        Dataset_Dic.Add(partname, GetPartFooterMessageDataSet(drpart["FooterDesc"].ToString()));
                        foreach (DataRow drsubject in Ds_Subject.Tables[0].Rows)
                        {
                            string subquury = "";
                            if (Termname == "Term1")
                                subquury = Tablename + ".DescriptiveIndicator," + Tablename + ".Term1";
                            else if (Termname == "Term2")
                                subquury = Tablename + ".DescriptiveIndicator," + Tablename + ".Term2";
                            else
                                subquury = Tablename + ".DescriptiveIndicator," + Tablename + ".Term1," + Tablename + ".Term2," + Tablename + ".Term3 ";

                            sql = "SELECT DISTINCT(tblcce_subjectskills.SkillName) as Temp,tblcce_subjectskills.SkillName," + subquury + " from " + Tablename + " inner JOIN tblcce_subjectskills on " + Tablename + ".SkillId=tblcce_subjectskills.Id,(SELECT @a:= 0) AS a where " + Tablename + ".SubjectId=" + int.Parse(drsubject["Id"].ToString()) + " AND " + Tablename + ".StudentId=" + MyParentInfo.StudentId + "" + Batchidstr + "";
                            string subjectname ="@@" + drsubject["subject_name"].ToString() + "@@";
                            subjectname = subjectname.Replace(" ","");
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
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            DataSet ds = _mysqlObj.ExecuteQueryReturnDataSet(sql);
            int i = 1;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dr[0] = i;
                i++;
            }
            return SetDefaultColumnname(ds);
        }
        /// <summary>
        /// this function return discriptive dataset
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

        private  DataSet SetDefaultColumnname(DataSet result)//set column name Like a column1 column2....
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

    }
}
