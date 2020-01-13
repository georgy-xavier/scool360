using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using WinBase;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using System.Data;
using MigraDoc.DocumentObjectModel.Shapes;
using System.IO;


namespace WinEr
{
    public partial class Reportcard : System.Web.UI.Page
    {
        private ExamManage MyExamMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private MysqlClass _Mysqlobj;
        private ClassOrganiser MyClassMngr;
        private SchoolClass objSchool = null;
        private string M_Logo = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
                Response.Redirect("Default.aspx");

            MyUser = (KnowinUser)Session["UserObj"];
            MyExamMang = MyUser.GetExamObj();
            MyClassMngr = MyUser.GetClassObj();

            if (MyExamMang == null)
                Response.Redirect("RoleErr.htm");
            if (!MyUser.HaveActionRignt(3042))
                Response.Redirect("RoleErr.htm");
            string _ConnectionString = WinerUtlity.SingleSchoolConnectionString;
            if (WinerUtlity.NeedCentrelDB())
            {
                if (Session[WinerConstants.SessionSchool] == null)
                {
                    Response.Redirect("Logout.aspx");
                }
                objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                _ConnectionString = objSchool.ConnectionString;
            }
            _Mysqlobj = new MysqlClass(_ConnectionString);
            if (!IsPostBack)
                LoadDetails();
        }

        private void LoadDetails()
        {
            LoadClassDetailsToDropDown();
            LoadStudentsDetailsToDropDown();
        }

        private void LoadClassDetailsToDropDown()
        {
            Drp_SelectClass.Items.Clear();
            string sql = "SELECT tblclass.Id,tblclass.ClassName FROM tblclass where tblclass.Status =1 AND tblclass.Id IN (SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ")) ORDER BY tblclass.Standard,tblclass.ClassName";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_SelectClass.Items.Add(li);
                }
            }
            else
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("No Class Found", "-1");
                Drp_SelectClass.Items.Add(li);
            }

        }

        public void LoadStudentsDetailsToDropDown()
        {
            if (Drp_SelectClass.SelectedValue != "-1")
            {
                int ClassId = 0;
                int.TryParse(Drp_SelectClass.SelectedValue.ToString(), out ClassId);

                Drp_SelectStudent.Items.Clear();
                string sql = " SELECT stud.Id, stud.StudentName FROM tblstudentclassmap map inner join tblstudent stud on stud.id= map.Studentid where stud.status=1 and  map.ClassId=" + ClassId + " and map.RollNo<>-1 and map.BatchId=" + MyUser.CurrentBatchId + " order by  stud.StudentName ASC";
                MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

                if (MyReader.HasRows)
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("All Student", "0");
                    Drp_SelectStudent.Items.Add(li);

                    while (MyReader.Read())
                    {
                        System.Web.UI.WebControls.ListItem Li = new System.Web.UI.WebControls.ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_SelectStudent.Items.Add(Li);
                    }
                }
                else
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Student Not Found", "-1");
                    Drp_SelectStudent.Items.Add(li);
                }
            }
            else
            {

                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("All Student", "0");
                Drp_SelectStudent.Items.Add(li);
            }
        }

        protected void Drp_SelectClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentsDetailsToDropDown();
        }

        protected void Btn_ExamReport_Click(object sender, EventArgs e)
        {
            Lbl_Err.Text = "";
            try
            {
                if (ValidEntries())
                {
                    //Dominic In this type of report, they are using two different format, 
                    // upto 8th standard, thay are using normal format  and in other classes, they are using like CBSE format
                    //if ("NEW_REPORT" == TypofReport())
                    //{
                    //Create_New_Report();
                    //}
                    //else if ("HIGH_SCHOOL_REPORT" == TypofReport())
                    //{
                    //CreateHigherReport();
                    //}
                    if ("PRIMARY_SCHOOL_REPORT" == TypofReport())
                    {
                        CreatePrimarySchoolReport();
                    }
                    else
                    {
                        Lbl_Err.Text = "Select class";
                    }

                }

            }
            catch (Exception err)
            {
                Lbl_Err.Text = err.Message;
            }
        }

        private bool ValidEntries()
        {
            if (Drp_SelectClass.SelectedValue == "-1")
            {
                Lbl_Err.Text = "No class found.";
                return false;
            }
            else if (Txt_ReportName.Text == "")
            {
                Lbl_Err.Text = "Enter Report Name.";
                return false;
            }

            return true;
        }

        public string TypofReport()
        {
            int standard = 0;
            int _ClassId = 0;
            int fromclassconfi = 0;
            int toclassconfi = 0;
            string sqlstandard = " select tblconfiguration.Value,tblconfiguration.SubValue from tblconfiguration where tblconfiguration.Name='StandardId' and tblconfiguration.Module='Report'";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sqlstandard);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out fromclassconfi);
                int.TryParse(MyReader.GetValue(1).ToString(), out toclassconfi);

            }
            string sql = "select tblclass.Standard from tblclass WHERE tblclass.id=" + Drp_SelectClass.SelectedValue;
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);



            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out standard);

                    return "PRIMARY_SCHOOL_REPORT";
                
            }
            return "";
        }

        //public void Create_New_Report()
        //{
        //    DataSet TotalStudList;
        //    DataSet totalMarkRatios = null;
        //    ExamNode[] _ExamDetails;
        //    ExamNode[] _SubDetails;

        //    SchoolDetails _SchoolDetails;
        //    ExamNode[] CC_ExamDetails = null;
        //    ExamNode[] CC_SubDetails = null;



        //    List<EvaluationReportClass> ClsObj = new List<EvaluationReportClass>();
        //    EvaluationReportClass _ReportObj;


        //    GetSchoolDetails(out _SchoolDetails);

        //    GetExamDetails(int.Parse(Drp_SelectClass.SelectedValue), "MAIN REPORT", out _ExamDetails);
        //    GetSubDetails(int.Parse(Drp_SelectClass.SelectedValue), "MAIN REPORT", out _SubDetails);

        //    if (_ExamDetails != null && _SubDetails != null && _ExamDetails.Length > 0 && _SubDetails.Length > 0)
        //    {
        //        TotalStudList = GetAllStudentListFromClass(int.Parse(Drp_SelectStudent.SelectedValue), int.Parse(Drp_SelectClass.SelectedValue));
        //        totalMarkRatios = GetMarkRatio(Drp_SelectClass.SelectedValue);


        //        foreach (DataRow Dr in TotalStudList.Tables[0].Rows)
        //        {
        //            _ReportObj = new EvaluationReportClass(_Mysqlobj, int.Parse(Dr[0].ToString()));
        //            _ReportObj.ExamDetails(_ExamDetails, _SubDetails, Dr, "Main");
        //            ClsObj.Add(_ReportObj);
        //        }
        //        Create_New_Report(ClsObj, _ExamDetails, _SubDetails, CC_ExamDetails, CC_SubDetails, _SchoolDetails, int.Parse(Drp_SelectStudent.SelectedValue), totalMarkRatios);
        //    }
        //    else
        //    {
        //        Lbl_Err.Text = "Exam details not found";
        //    }

        //}

        private void GetSchoolDetails(out SchoolDetails _SchoolDetails)
        {
            _SchoolDetails.SchoolName = "";
            _SchoolDetails.Address = "";
            _SchoolDetails.LogoURL = "";

            string sql = "select tblschooldetails.SchoolName, tblschooldetails.Address, tblschooldetails.LogoUrl from tblschooldetails";
            OdbcDataReader MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                _SchoolDetails.SchoolName = MyReader.GetValue(0).ToString();
                _SchoolDetails.Address = MyReader.GetValue(1).ToString();
                _SchoolDetails.LogoURL = MyReader.GetValue(2).ToString();
            }

        }

        private void GetExamDetails(int ClassId, string ExamType, out ExamNode[] _ExamDetails)
        {
            //string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period from tblexammaster inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id     where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and tblexammaster.Id=" + ExamId + " and tblexamschedule.Status='Completed'  order by tblexamschedule.id desc";
            string sql = "select tblexamschedule.id, tblexammaster.ExamName , tblperiod.Period, tblexamschedule.GradeMasterId from tblsubject_type  inner join tblexammaster on tblexammaster.ExamTypeId=tblsubject_type.Id   inner join tblclassexam on tblclassexam.ExamId = tblexammaster.Id inner join tblexamschedule on tblexamschedule.ClassExamId = tblclassexam.id inner join tblperiod on tblexamschedule.PeriodId = tblperiod.Id     where tblclassexam.ClassId=" + ClassId + "  and tblexamschedule.Id in (select tblstudentmark.ExamSchId from  tblstudentmark) and  tblsubject_type.TypeDisc='MAIN REPORT' and tblexamschedule.Status='Completed'  order by  tblexammaster.ExamOrder ";
            OdbcDataReader MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                int i = 0;
                int Count = MyReader.RecordsAffected;
                _ExamDetails = new ExamNode[Count];

                while (MyReader.Read())
                {
                    _ExamDetails[i].Id = int.Parse(MyReader.GetValue(0).ToString());
                    _ExamDetails[i].Name = MyReader.GetValue(1).ToString();
                    _ExamDetails[i].GradeMasterId = int.Parse(MyReader.GetValue(3).ToString());
                    string sql1 = "select Max( tblexammark.ExamDate) from tblexammark where tblexammark.ExamSchId=" + int.Parse(MyReader.GetValue(0).ToString());
                    OdbcDataReader dr = MyExamMang.m_MysqlDb.ExecuteQuery(sql1);

                    if (dr.HasRows)
                    {

                        _ExamDetails[i].Date = DateTime.Parse(dr.GetValue(0).ToString());
                    }

                    i++;
                }
            }
            else
            {
                _ExamDetails = new ExamNode[0];
            }
        }

        private void GetSubDetails(int ClassId, string ExamType, out ExamNode[] _SubDetails)
        {
            //string sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name, tblclassexamsubmap.MaxMark from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId=tblexamschedule.ClassExamId and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId  where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblclassexam.ClassId=" + ClassId + " and tblexammaster.Id=" + ExamId;
            string sql = " SELECT distinct tblsubjects.Id,tblsubjects.subject_name,tblsubjects.sub_description from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id  inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId   inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId   inner join tblclassexamsubmap on tblclassexamsubmap. ClassExamId=tblexamschedule.ClassExamId   and tblclassexamsubmap.SubId= tblsubjects.Id inner join tblexammaster on tblexammaster.Id=  .tblclassexam.ExamId Inner join tblsubject_type on  tblexammaster.ExamTypeId=tblsubject_type.Id   where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "  and tblclassexam.ClassId=" + ClassId + "  and     tblsubject_type.TypeDisc='MAIN REPORT' and tblexamschedule.Status='Completed'  order by tblexammark.SubjectOrder  ";

            OdbcDataReader MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int i = 0;
                int Count = MyReader.RecordsAffected;
                _SubDetails = new ExamNode[Count];

                while (MyReader.Read())
                {
                    _SubDetails[i].Id = int.Parse(MyReader.GetValue(0).ToString());
                    _SubDetails[i].Name = MyReader.GetValue(1).ToString();

                    _SubDetails[i].Desc = MyReader.GetValue(2).ToString();
                    i++;
                }
            }
            else
            {
                _SubDetails = new ExamNode[0];
            }

        }

        private DataSet GetAllStudentListFromClass(int StudentId, int ClassId)
        {

            DataSet _StdentSet = new DataSet();
            DataTable dt;
            DataRow dr;

            _StdentSet.Tables.Add(new DataTable("Student"));

            dt = _StdentSet.Tables["Student"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("RollNum");

            dt.Columns.Add("DOB");
            dt.Columns.Add("Class");
            dt.Columns.Add("AdmissionNum");
            dt.Columns.Add("FatherName");
            dt.Columns.Add("MotherName");
            dt.Columns.Add("Tel");
            dt.Columns.Add("Add");
            dt.Columns.Add("Year");
            dt.Columns.Add("State");
            dt.Columns.Add("Sex");

            string sql_Student = "select tblview_student.Id, tblview_student.StudentName, tblview_student.RollNo, tblclass.ClassName,date_Format(tblview_student.DOB,'%d/%m/%Y') as DOB , tblview_student.AdmitionNo,tblview_student.GardianName,tblview_student.MothersName, tblview_student.ResidencePhNo ,tblview_student.Address ,tblview_student.sex ,tblbatch.BatchName ,tblview_student.state  from tblview_student inner join tblclass on tblclass.Id= tblview_student.ClassId inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblview_student.Id  inner join tblbatch on tblbatch.id= tblview_student.JoinBatch  where tblview_student.ClassId=" + ClassId + " and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId;

            if (StudentId != 0)
                sql_Student = sql_Student + " and tblview_student.Id=" + StudentId;



            DataSet _studList = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql_Student);


            foreach (DataRow dr_values in _studList.Tables[0].Rows)
            {

                dr = _StdentSet.Tables["Student"].NewRow();

                dr["Id"] = dr_values["Id"];
                dr["Name"] = dr_values["StudentName"];
                dr["RollNum"] = dr_values["RollNo"];
                dr["DOB"] = dr_values["DOB"];
                dr["Class"] = dr_values["ClassName"];
                dr["AdmissionNum"] = dr_values["AdmitionNo"];
                dr["FatherName"] = dr_values["GardianName"];
                dr["MotherName"] = dr_values["MothersName"];
                dr["Tel"] = dr_values["ResidencePhNo"];
                dr["Add"] = dr_values["Address"];

                dr["Year"] = dr_values["BatchName"];
                dr["State"] = dr_values["state"];
                dr["Sex"] = dr_values["sex"];
                _StdentSet.Tables["Student"].Rows.Add(dr);

            }
            return _StdentSet;
        }

        private DataSet GetMarkRatio(string _selectedClassId)
        {
            string stdId = MyClassMngr.GetStandardIdfromClassId(_selectedClassId);
            string sqlRation = "select FA1,FA2,SA1,FA3,FA4,SA2 from tblcbsegraderatio where tblcbsegraderatio.StandardId=" + stdId;
            DataSet Dt = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sqlRation);
            return Dt;
        }

        

        private void CreatePrimarySchoolReport()
        {
            DataSet TotalStudList;
            DataSet totalMarkRatios = null;
            ExamNode[] _ExamDetails;
            ExamNode[] _SubDetails;

            SchoolDetails _SchoolDetails;
            ExamNode[] CC_ExamDetails = null;
            ExamNode[] CC_SubDetails = null;



            List<EvaluationReportClass> ClsObj = new List<EvaluationReportClass>();
            EvaluationReportClass _ReportObj;


            GetSchoolDetails(out _SchoolDetails);

            GetExamDetails(int.Parse(Drp_SelectClass.SelectedValue), "MAIN REPORT", out _ExamDetails);
            GetSubDetails(int.Parse(Drp_SelectClass.SelectedValue), "MAIN REPORT", out _SubDetails);

            if (_ExamDetails != null && _SubDetails != null && _ExamDetails.Length > 0 && _SubDetails.Length > 0)
            {
                TotalStudList = GetAllStudentListFromClass(int.Parse(Drp_SelectStudent.SelectedValue), int.Parse(Drp_SelectClass.SelectedValue));


                totalMarkRatios = GetMarkRatio(Drp_SelectClass.SelectedValue);


                foreach (DataRow Dr in TotalStudList.Tables[0].Rows)
                {

                    _ReportObj = new EvaluationReportClass(_Mysqlobj, int.Parse(Dr[0].ToString()));
                    _ReportObj.ExamDetails(_ExamDetails, _SubDetails, Dr, "Main");
                    ClsObj.Add(_ReportObj);
                }

                CreatePrimaryReport(ClsObj, _ExamDetails, _SubDetails, _SchoolDetails, int.Parse(Drp_SelectStudent.SelectedValue), totalMarkRatios);
            }
            else
            {
                Lbl_Err.Text = "Exam details not found";
            }
        }

        private void Create_New_Report(List<EvaluationReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, ExamNode[] CC_ExamDetails, ExamNode[] CC_SubDetails, SchoolDetails _SchoolDetails, int _StudId, DataSet totalMarkRatios)
        {
            string _PhysicalPath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));
            //int _StudentId = 0;
            Document document = Load_New_PDFPage(ClsObj, _ExamDetails, _SubDetails, _SchoolDetails, CC_ExamDetails, CC_SubDetails, totalMarkRatios);

            const PdfFontEmbedding embedding = PdfFontEmbedding.Always;

            // ----------------------------------------------------------------------------------------

            const bool unicode = false;

            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF


            pdfRenderer.RenderDocument();

            // Save the document...
            string filename = "", MainName = "";

            if (_StudId == 0)
                MainName = Drp_SelectClass.SelectedItem.ToString();
            else
                MainName = Drp_SelectStudent.SelectedItem.ToString();

            filename = _PhysicalPath + "\\PDF_Files\\ECH_" + MainName + ".pdf";

            pdfRenderer.PdfDocument.Save(filename);
            // ...and start a viewer.
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "null", "window.open(\"OpenPdfPage.aspx?PdfName=ECH_" + MainName + ".pdf\",'Info','status=1, width=1000, height=700,,resizable=1,scrollbars=1');", true);

            //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=PRC_" + MainName + ".pdf\");", true);
            // Response.Redirect("OpenPdfPage.aspx?PdfName=PRC_" + MainName + ".pdf", false);
            if (!String.IsNullOrEmpty(M_Logo))
            {
                File.Delete(M_Logo);
            }
        }

        private Document Load_New_PDFPage(List<EvaluationReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, SchoolDetails _SchoolDetails, ExamNode[] CC_ExamDetails, ExamNode[] CC_SubDetails, DataSet totalMarkRatios)
        {
            Document document = new Document();
            string path = GtSchoolLogo();

            for (int i = 0; i < ClsObj.Count; i++)
            {



                PageSetup PgSt = document.DefaultPageSetup;
                PgSt.PageHeight = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(210);
                PgSt.PageWidth = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(297);

                PgSt.LeftMargin = 0;
                PgSt.RightMargin = 0;
                PgSt.TopMargin = 30;
                PgSt.BottomMargin = 0;

                PgSt.HeaderDistance = 0;
                PgSt.FooterDistance = 0;



                // Add a section to the document
                Section section = document.AddSection();

                MigraDoc.DocumentObjectModel.Tables.Table table = section.AddTable();



                table.Borders.Width = 2;
                //table.Borders.Left.Width = 0.5;
                //table.Borders.Right.Width = 0.5;
                table.Rows.LeftIndent = 25;
                MigraDoc.DocumentObjectModel.Tables.Column Col = table.AddColumn(PgSt.PageWidth - 50);
                MigraDoc.DocumentObjectModel.Tables.Row row = table.AddRow();
                row.Height = PgSt.PageHeight - 100;
                Col.Borders.Visible = true;

                MigraDoc.DocumentObjectModel.Tables.Cell cel = row.Cells[0];
                // Add a paragraph to the section
                Paragraph paragraph = section.AddParagraph();
                paragraph = cel.AddParagraph();
                paragraph.AddLineBreak();


                MigraDoc.DocumentObjectModel.Tables.Table tb = new MigraDoc.DocumentObjectModel.Tables.Table();


                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                tb.AddColumn((PgSt.PageWidth - 80) / 7);
                tb.AddColumn((PgSt.PageWidth - 80) - ((PgSt.PageWidth - 80) / 7));

                row = tb.AddRow();
                row.Cells[0].AddImage(path);
                row.Cells[0].MergeDown = 2;

                row.Cells[1].Format.Font.Size = 16;
                row.Cells[1].Format.Font.Bold = true;
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].AddParagraph(MyUser.SchoolName.ToUpperInvariant());
                row = tb.AddRow();

                string _smallAddress = MyUser.GetSchoolSmallAdress();
                string[] Address = _smallAddress.Split('|');
                string _Address1 = "", _Address2 = "";
                if (Address.Length > 1)
                {
                    _Address1 = Address[0];
                }

                if (Address.Length > 2)
                {
                    _Address2 = Address[1];
                }

                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].AddParagraph(_Address1);

                row = tb.AddRow();
                row.Cells[1].Format.Font.Size = 16;
                row.Cells[1].Format.Font.Bold = true;
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].AddParagraph("SCHOOL EVALUATION RECORD CARD");

                cel.Elements.Add(tb);

                //paragraph = section.AddParagraph();
                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);
                paragraph.AddFormattedText("STUDENT INFORMATIONS", TextFormat.Underline | TextFormat.Bold);
                paragraph.AddLineBreak();

                tb = new MigraDoc.DocumentObjectModel.Tables.Table();


                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                tb.AddColumn((PgSt.PageWidth - 80) / 6);
                tb.AddColumn((PgSt.PageWidth - 80) / 3);
                tb.AddColumn((PgSt.PageWidth - 80) / 6);
                tb.AddColumn((PgSt.PageWidth - 80) / 3);

                row = tb.AddRow();


                row = tb.AddRow();
                row.Cells[0].AddParagraph("Name");
                row.Cells[1].AddParagraph(":" + ClsObj[i].m_StudDetails.Name);
                row.Cells[2].AddParagraph("Sex");
                row.Cells[3].AddParagraph(":" + ClsObj[i].m_StudDetails.Sex);
                row = tb.AddRow();
                row.Cells[0].AddParagraph("Class & Sec");
                row.Cells[1].AddParagraph(":" + ClsObj[i].m_StudDetails.Class);
                row.Cells[2].AddParagraph("Roll No");
                row.Cells[3].AddParagraph(":" + ClsObj[i].m_StudDetails.RollNum);
                row = tb.AddRow();
                row.Cells[0].AddParagraph("Year Of Admission");
                row.Cells[1].AddParagraph(":" + ClsObj[i].m_StudDetails.Year);
                row.Cells[2].AddParagraph("Attendance");
                row.Cells[3].AddParagraph(":" + ClsObj[i].m_StudDetails.Attendance);
                row = tb.AddRow();
                row.Cells[0].AddParagraph("Guardian's Name");
                row.Cells[1].AddParagraph(":" + ClsObj[i].m_StudDetails.FatherName);
                row.Cells[2].AddParagraph("Address");
                row.Cells[3].AddParagraph(":" + ClsObj[i].m_StudDetails.Add);

                row = tb.AddRow();

                row.Cells[0].AddParagraph("State");
                row.Cells[1].AddParagraph(":" + ClsObj[i].m_StudDetails.State);

                tb.Borders.Visible = false;

                cel.Elements.Add(tb);





                // main exam area

                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);
                paragraph.AddLineBreak();
                paragraph.AddFormattedText("Scholastic Area", TextFormat.Underline | TextFormat.Bold);
                paragraph.AddLineBreak();

                tb = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                tb.AddColumn((PgSt.PageWidth - 80) * 4 / 19);
                tb.AddColumn((PgSt.PageWidth - 80) / 14);
                tb.AddColumn((PgSt.PageWidth - 80) / 14);
                tb.AddColumn((PgSt.PageWidth - 80) / 14);
                tb.AddColumn((PgSt.PageWidth - 80) / 14);
                tb.AddColumn((PgSt.PageWidth - 80) / 14);
                tb.AddColumn((PgSt.PageWidth - 80) / 14);
                tb.AddColumn((PgSt.PageWidth - 80) / 14);
                //tb.AddColumn((PgSt.PageWidth - 80) / 19);
                //tb.AddColumn((PgSt.PageWidth - 80) / 19);
                //tb.AddColumn((PgSt.PageWidth - 80) / 19);
                tb.AddColumn((PgSt.PageWidth - 80) / 14);
                tb.AddColumn((PgSt.PageWidth - 80) / 14);

                tb.AddColumn((PgSt.PageWidth - 80) * 3 / 20);

                row = tb.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("MARKS SECURED IN");
                row.Cells[0].AddParagraph().AddLineBreak();
                row.Cells[0].AddParagraph().AddFormattedText("SUBJECTS");
                //row.Cells[0].MergeDown = 1;
                row.Cells[1].AddParagraph().AddFormattedText("UT 25 Marks (a)", TextFormat.Bold); //Exam Name

                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[2].AddParagraph().AddFormattedText("UT 25 Marks (b)", TextFormat.Bold); //Exam Name

                row.Cells[2].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[3].AddParagraph().AddFormattedText("UT 25 Marks (c)", TextFormat.Bold); //Exam Name

                row.Cells[3].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[4].AddParagraph().AddFormattedText("UT 25 Marks (d)", TextFormat.Bold); //Exam Name

                row.Cells[4].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[5].AddParagraph().AddFormattedText("Total of a+b+c+d (100 Marks) X", TextFormat.Bold); //Exam Name

                row.Cells[5].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[6].AddParagraph().AddFormattedText("Half Yearly Exam (50 Marks) Y", TextFormat.Bold); //Exam Name

                row.Cells[6].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[7].AddParagraph().AddFormattedText("Annual Exam (50 Marks) Z", TextFormat.Bold); //Exam Name

                row.Cells[7].Format.Alignment = ParagraphAlignment.Center;

                //TextFrame Frame = row.Cells[8].AddTextFrame();
                //Frame.Orientation = TextOrientation.Upward;
                //Frame.Width = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1);
                //Paragraph para = Frame.AddParagraph("20 % of X");

                //para.Format.Font.Bold = true;
                ////para.Format.ClearAll();
                //para.Format.Alignment = ParagraphAlignment.Center;

                //Frame = row.Cells[9].AddTextFrame();
                //Frame.Orientation = TextOrientation.Upward;
                //Frame.Width = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1);
                //para = Frame.AddParagraph("30 % of Y");

                //para.Format.Font.Bold = true;
                ////para.Format.ClearAll();
                //para.Format.Alignment = ParagraphAlignment.Center;

                //Frame = row.Cells[10].AddTextFrame();
                //Frame.Orientation = TextOrientation.Upward;
                //Frame.Width = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1);
                //para = Frame.AddParagraph("50 % of Z");
                //para.Format.Font.Bold = false;
                row.Cells[8].AddParagraph().AddFormattedText("Total of X+Y+Z", TextFormat.Bold); //Exam Name

                row.Cells[8].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[9].AddParagraph().AddFormattedText("%", TextFormat.Bold); //Exam Name

                row.Cells[9].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[9].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;


                row.Cells[10].AddParagraph().AddFormattedText("Remarks", TextFormat.Bold); //Exam Name

                row.Cells[10].Format.Alignment = ParagraphAlignment.Center;


                // para.Format.Font.Bold = true;




                //para.Format.ClearAll();
                //para.Format.Alignment = ParagraphAlignment.Center;


                int tempmearge = 0;
                double _TotalMaxMark = 0, _TotalMark = 0, _Sub_Mark = 0, _Sub_Max = 0;

                double[] _ExamWiseTotalMark = new double[_ExamDetails.Length];
                double[] _ExamWiseSubMark = new double[_ExamDetails.Length];

                DataSet Grade = MyExamMang.GetGradeDataSet(_ExamDetails[_ExamDetails.Length - 1].GradeMasterId);

                double sumOfUnitTestMark = 0;
                double sumOfUnitTestMax = 0;
                double halfYearlyMark = 0;
                double halfYearlyMax = 0;
                double AnnualMark = 0;
                double AnnualMax = 0;
                double _20ofX = 0;
                double _30ofY = 0;
                double _50ofZ = 0;


                double TotoalXYZ = 0;

                for (int Subjct = 0; Subjct < _SubDetails.Length; Subjct++)
                {
                    row = tb.AddRow();
                    row.Cells[0].AddParagraph().AddFormattedText(_SubDetails[Subjct].Name); //Subject Name

                    int k = 1;

                    sumOfUnitTestMark = 0;
                    sumOfUnitTestMax = 0;
                    halfYearlyMark = 0;
                    halfYearlyMax = 0;
                    AnnualMark = 0;
                    AnnualMax = 0;

                    for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                    {
                        if (_ExamDetails[ExamCount].Name.StartsWith("UT") || _ExamDetails[ExamCount].Name.StartsWith("FA"))
                        {
                            if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString() != "-1" && ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString() != "-2")
                            {
                                row.Cells[k].AddParagraph().AddFormattedText(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                                sumOfUnitTestMark = sumOfUnitTestMark + double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());

                            }
                            else
                                row.Cells[k].AddParagraph().AddFormattedText("-");
                            double max_Mark = MyExamMang.GetMaxMarkFromExamScheduleIdandSubjectId(_ExamDetails[ExamCount].Id, _SubDetails[Subjct].Id);
                            sumOfUnitTestMax = sumOfUnitTestMax + max_Mark;
                            k = k + 1;
                        }
                        else if (_ExamDetails[ExamCount].Name.StartsWith("Half") || _ExamDetails[ExamCount].Name.StartsWith("SA1"))
                        {
                            row.Cells[6].AddParagraph().AddFormattedText(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                            double.TryParse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString(), out halfYearlyMark);
                            double max_Mark = MyExamMang.GetMaxMarkFromExamScheduleIdandSubjectId(_ExamDetails[ExamCount].Id, _SubDetails[Subjct].Id);
                            halfYearlyMax = max_Mark;

                        }
                        else if (_ExamDetails[ExamCount].Name.StartsWith("Annual") || _ExamDetails[ExamCount].Name.StartsWith("SA2"))
                        {
                            row.Cells[7].AddParagraph().AddFormattedText(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                            double.TryParse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString(), out AnnualMark);
                            double max_Mark = MyExamMang.GetMaxMarkFromExamScheduleIdandSubjectId(_ExamDetails[ExamCount].Id, _SubDetails[Subjct].Id);
                            AnnualMax = max_Mark;
                        }

                    }
                    row.Cells[5].AddParagraph().AddFormattedText(sumOfUnitTestMark.ToString());

                    //_20ofX = sumOfUnitTestMark * 20 / 100;

                    //_30ofY = halfYearlyMark * 30 / 100;
                    //_50ofZ = AnnualMark * 50 / 100;

                    _20ofX = sumOfUnitTestMark;

                    _30ofY = halfYearlyMark;
                    _50ofZ = AnnualMark;

                    double _XYZ = _20ofX + _30ofY + _50ofZ;


                    TotoalXYZ = TotoalXYZ + _XYZ;
                    //row.Cells[8].AddParagraph().AddFormattedText(_20ofX.ToString("0.00"));
                    //row.Cells[9].AddParagraph().AddFormattedText(_30ofY.ToString("0.00"));
                    //row.Cells[10].AddParagraph().AddFormattedText(_50ofZ.ToString("0.00"));
                    row.Cells[8].AddParagraph().AddFormattedText(_XYZ.ToString("0"));

                    double Per = (_XYZ / (200)) * 100;
                    //Per = Math.Round(Per,2);
                    row.Cells[9].AddParagraph().AddFormattedText(Per.ToString("0.00"));

                    if (tempmearge == 0)
                    {
                        //  row.Cells[13].MergeDown = _SubDetails.Length;
                        row.Cells[10].MergeDown = _SubDetails.Length + 1;
                        tempmearge = 1;
                    }




                }

                row = tb.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("Sign of Class Teacher"); //Subject Name
                row = tb.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("Environmental Science"); //Subject Name

                row.Cells[8].AddParagraph().AddFormattedText(TotoalXYZ.ToString("0"));
                //double Per2 = (TotoalXYZ / (2000)) * 100;
                ////Per2 = Math.Round(Per2,2);
                //row.Cells[9].AddParagraph().AddFormattedText(Per2.ToString("0.00"));
                tb.Borders.Visible = true;






                cel.Elements.Add(tb);




                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);
                paragraph.AddLineBreak();
                paragraph.AddFormattedText("Co-Scholastic Area", TextFormat.Underline | TextFormat.Bold);
                paragraph.AddLineBreak();

                tb = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                tb.AddColumn((PgSt.PageWidth - 80) / 5);
                tb.AddColumn((PgSt.PageWidth - 80) / 5);
                tb.AddColumn((PgSt.PageWidth - 80) / 5);
                tb.AddColumn((PgSt.PageWidth - 80) / 5);
                tb.AddColumn((PgSt.PageWidth - 80) / 5);


                row = tb.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("WORK EXPERIENCE");
                row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                //row.Cells[0].MergeDown = 1;
                row.Cells[1].AddParagraph().AddFormattedText("GAMES & SPORTS", TextFormat.Bold); //Exam Name

                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[2].AddParagraph().AddFormattedText("SCOUTS & GUIDES", TextFormat.Bold); //Exam Name

                row.Cells[2].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[3].AddParagraph().AddFormattedText("LITERARY & CULTURAL", TextFormat.Bold); //Exam Name

                row.Cells[3].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[4].AddParagraph().AddFormattedText("GENERAL CONDUCTS", TextFormat.Bold); //Exam Name

                row.Cells[4].Format.Alignment = ParagraphAlignment.Center;


                row = tb.AddRow();



                row.Cells[0].AddParagraph().AddFormattedText("Activity :");
                row.Cells[0].AddParagraph().AddLineBreak();
                row.Cells[0].AddParagraph().AddFormattedText("Grade :");

                row.Cells[1].AddParagraph().AddFormattedText("Activity :");
                row.Cells[1].AddParagraph().AddLineBreak();
                row.Cells[1].AddParagraph().AddFormattedText("Grade :");

                row.Cells[2].AddParagraph().AddFormattedText("Activity :");
                row.Cells[2].AddParagraph().AddLineBreak();
                row.Cells[2].AddParagraph().AddFormattedText("Grade :");

                row.Cells[3].AddParagraph().AddFormattedText("Activity :");
                row.Cells[3].AddParagraph().AddLineBreak();
                row.Cells[3].AddParagraph().AddFormattedText("Grade :");

                row.Cells[4].AddParagraph().AddFormattedText("Activity :");
                row.Cells[4].AddParagraph().AddLineBreak();
                row.Cells[4].AddParagraph().AddFormattedText("Grade :");

                tb.Borders.Visible = true;
                cel.Elements.Add(tb);


                // dominic new 




                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.AddLineBreak();
                paragraph.AddLineBreak();
                paragraph.AddLineBreak();


                tb = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                tb.AddColumn((PgSt.PageWidth - 80) / 2);
                tb.AddColumn((PgSt.PageWidth - 80) / 2);

                row = tb.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("CLASS TEACHER");
                row.Cells[0].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[1].AddParagraph().AddFormattedText("PRINCIPAL");
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;

                tb.Borders.Visible = false;
                cel.Elements.Add(tb);



            }
            return document;

        }

        private string GtSchoolLogo()
        {
            String ImageUrl = "";
            ImageUploaderClass imgobj = new ImageUploaderClass(objSchool);
            byte[] img_bytes = imgobj.getImageBytes(objSchool.SchoolId, "Logo");
            M_Logo = MyUser.FilePath + "/ThumbnailImages/" + objSchool.SchoolId + "_" + System.DateTime.Now.Millisecond + ".jpg";


            File.WriteAllBytes(M_Logo, img_bytes);
            ImageUrl = M_Logo;
            return ImageUrl;
        }

        private void CreateReport(List<EvaluationReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, ExamNode[] CC_ExamDetails, ExamNode[] CC_SubDetails, SchoolDetails _SchoolDetails, int _StudId, DataSet totalMarkRatios)
        {
            string _PhysicalPath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));
            //int _StudentId = 0;
            Document document = LoadPDFPage(ClsObj, _ExamDetails, _SubDetails, _SchoolDetails, CC_ExamDetails, CC_SubDetails, totalMarkRatios);

            const PdfFontEmbedding embedding = PdfFontEmbedding.Always;

            // ----------------------------------------------------------------------------------------

            const bool unicode = false;

            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF


            pdfRenderer.RenderDocument();

            // Save the document...
            string filename = "", MainName = "";

            if (_StudId == 0)
                MainName = Drp_SelectClass.SelectedItem.ToString();
            else
                MainName = Drp_SelectStudent.SelectedItem.ToString();

            filename = _PhysicalPath + "\\PDF_Files\\ECH_" + MainName + ".pdf";

            pdfRenderer.PdfDocument.Save(filename);
            // ...and start a viewer.
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "null", "window.open(\"OpenPdfPage.aspx?PdfName=ECH_" + MainName + ".pdf\",'Info','status=1, width=1000, height=700,,resizable=1,scrollbars=1');", true);

            //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=PRC_" + MainName + ".pdf\");", true);
            // Response.Redirect("OpenPdfPage.aspx?PdfName=PRC_" + MainName + ".pdf", false);
            if (!String.IsNullOrEmpty(M_Logo))
            {
                File.Delete(M_Logo);
            }
        }

        private Document LoadPDFPage(List<EvaluationReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, SchoolDetails _SchoolDetails, ExamNode[] CC_ExamDetails, ExamNode[] CC_SubDetails, DataSet totalMarkRatios)
        {

            Document document = new Document();
            string path = GtSchoolLogo();

            for (int i = 0; i < ClsObj.Count; i++)
            {



                PageSetup PgSt = document.DefaultPageSetup;
                PgSt.PageHeight = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(210);
                PgSt.PageWidth = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(297);

                PgSt.LeftMargin = 0;
                PgSt.RightMargin = 0;
                PgSt.TopMargin = 30;
                PgSt.BottomMargin = 0;

                PgSt.HeaderDistance = 0;
                PgSt.FooterDistance = 0;



                // Add a section to the document
                Section section = document.AddSection();

                MigraDoc.DocumentObjectModel.Tables.Table table = section.AddTable();



                table.Borders.Width = 2;
                //table.Borders.Left.Width = 0.5;
                //table.Borders.Right.Width = 0.5;
                table.Rows.LeftIndent = 25;
                MigraDoc.DocumentObjectModel.Tables.Column Col = table.AddColumn(PgSt.PageWidth - 50);
                MigraDoc.DocumentObjectModel.Tables.Row row = table.AddRow();
                row.Height = PgSt.PageHeight - 100;
                Col.Borders.Visible = true;

                MigraDoc.DocumentObjectModel.Tables.Cell cel = row.Cells[0];
                // Add a paragraph to the section
                Paragraph paragraph = section.AddParagraph();
                paragraph = cel.AddParagraph();
                paragraph.AddLineBreak();


                MigraDoc.DocumentObjectModel.Tables.Table tb = new MigraDoc.DocumentObjectModel.Tables.Table();


                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                tb.AddColumn((PgSt.PageWidth - 80) / 7);
                tb.AddColumn((PgSt.PageWidth - 80) - ((PgSt.PageWidth - 80) / 7));

                row = tb.AddRow();
                row.Cells[0].AddImage(path);
                row.Cells[0].MergeDown = 2;

                row.Cells[1].Format.Font.Size = 16;
                row.Cells[1].Format.Font.Bold = true;
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].AddParagraph(MyUser.SchoolName.ToUpperInvariant());
                row = tb.AddRow();

                string _smallAddress = MyUser.GetSchoolSmallAdress();
                string[] Address = _smallAddress.Split('|');
                string _Address1 = "", _Address2 = "";
                if (Address.Length > 1)
                {
                    _Address1 = Address[0];
                }

                if (Address.Length > 2)
                {
                    _Address2 = Address[1];
                }

                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].AddParagraph(_Address1);

                row = tb.AddRow();
                row.Cells[1].Format.Font.Size = 16;
                row.Cells[1].Format.Font.Bold = true;
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].AddParagraph("SCHOOL EVALUATION RECORD CARD");

                cel.Elements.Add(tb);

                //paragraph = section.AddParagraph();
                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);
                paragraph.AddFormattedText("STUDENT INFORMATIONS", TextFormat.Underline | TextFormat.Bold);
                paragraph.AddLineBreak();

                tb = new MigraDoc.DocumentObjectModel.Tables.Table();


                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                tb.AddColumn((PgSt.PageWidth - 80) / 6);
                tb.AddColumn((PgSt.PageWidth - 80) / 3);
                tb.AddColumn((PgSt.PageWidth - 80) / 6);
                tb.AddColumn((PgSt.PageWidth - 80) / 3);

                row = tb.AddRow();


                row = tb.AddRow();
                row.Cells[0].AddParagraph("Name");
                row.Cells[1].AddParagraph(":" + ClsObj[i].m_StudDetails.Name);
                row.Cells[2].AddParagraph("Sex");
                row.Cells[3].AddParagraph(":" + ClsObj[i].m_StudDetails.Sex);
                row = tb.AddRow();
                row.Cells[0].AddParagraph("Class & Sec");
                row.Cells[1].AddParagraph(":" + ClsObj[i].m_StudDetails.Class);
                row.Cells[2].AddParagraph("Roll No");
                row.Cells[3].AddParagraph(":" + ClsObj[i].m_StudDetails.RollNum);
                row = tb.AddRow();
                row.Cells[0].AddParagraph("Year of Admission");
                row.Cells[1].AddParagraph(":" + ClsObj[i].m_StudDetails.Year);
                row.Cells[2].AddParagraph("Attendance");
                row.Cells[3].AddParagraph(":" + ClsObj[i].m_StudDetails.Attendance);
                row = tb.AddRow();
                row.Cells[0].AddParagraph("Guardian's Name");
                row.Cells[1].AddParagraph(":" + ClsObj[i].m_StudDetails.FatherName);
                row.Cells[2].AddParagraph("Address");
                row.Cells[3].AddParagraph(":" + ClsObj[i].m_StudDetails.Add);

                row = tb.AddRow();

                row.Cells[0].AddParagraph("State");
                row.Cells[1].AddParagraph(":" + ClsObj[i].m_StudDetails.State);

                tb.Borders.Visible = false;

                cel.Elements.Add(tb);





                // main exam area

                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);
                paragraph.AddLineBreak();
                paragraph.AddFormattedText("Scholastic Area", TextFormat.Underline | TextFormat.Bold);
                paragraph.AddLineBreak();

                tb = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                tb.AddColumn((PgSt.PageWidth - 80) * 4 / 19);
                tb.AddColumn((PgSt.PageWidth - 80) / 19);
                tb.AddColumn((PgSt.PageWidth - 80) / 19);
                tb.AddColumn((PgSt.PageWidth - 80) / 19);
                tb.AddColumn((PgSt.PageWidth - 80) / 19);
                tb.AddColumn((PgSt.PageWidth - 80) / 19);
                tb.AddColumn((PgSt.PageWidth - 80) / 19);
                tb.AddColumn((PgSt.PageWidth - 80) / 19);
                tb.AddColumn((PgSt.PageWidth - 80) / 19);
                tb.AddColumn((PgSt.PageWidth - 80) / 19);
                tb.AddColumn((PgSt.PageWidth - 80) / 19);
                tb.AddColumn((PgSt.PageWidth - 80) / 19);
                tb.AddColumn((PgSt.PageWidth - 80) / 19);

                tb.AddColumn((PgSt.PageWidth - 80) * 3 / 20);

                row = tb.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("MARKS SECURED IN");
                row.Cells[0].AddParagraph().AddLineBreak();
                row.Cells[0].AddParagraph().AddFormattedText("SUBJECTS");
                //row.Cells[0].MergeDown = 1;
                row.Cells[1].AddParagraph().AddFormattedText("UT 25 Marks (a)", TextFormat.Bold); //Exam Name

                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[2].AddParagraph().AddFormattedText("UT 25 Marks (b)", TextFormat.Bold); //Exam Name

                row.Cells[2].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[3].AddParagraph().AddFormattedText("UT 25 Marks (c)", TextFormat.Bold); //Exam Name

                row.Cells[3].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[4].AddParagraph().AddFormattedText("UT 25 Marks (d)", TextFormat.Bold); //Exam Name

                row.Cells[4].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[5].AddParagraph().AddFormattedText("Total of a+b+c+d (100 Marks) X", TextFormat.Bold); //Exam Name

                row.Cells[5].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[6].AddParagraph().AddFormattedText("Half Yearly Exam (100 Marks) Y", TextFormat.Bold); //Exam Name

                row.Cells[6].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[7].AddParagraph().AddFormattedText("Annual Exam (100 Marks) Z", TextFormat.Bold); //Exam Name

                row.Cells[7].Format.Alignment = ParagraphAlignment.Center;

                TextFrame Frame = row.Cells[8].AddTextFrame();
                Frame.Orientation = TextOrientation.Upward;
                Frame.Width = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1);
                Paragraph para = Frame.AddParagraph("20 % of X");

                para.Format.Font.Bold = true;
                //para.Format.ClearAll();
                para.Format.Alignment = ParagraphAlignment.Center;

                Frame = row.Cells[9].AddTextFrame();
                Frame.Orientation = TextOrientation.Upward;
                Frame.Width = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1);
                para = Frame.AddParagraph("30 % of Y");

                para.Format.Font.Bold = true;
                //para.Format.ClearAll();
                para.Format.Alignment = ParagraphAlignment.Center;

                Frame = row.Cells[10].AddTextFrame();
                Frame.Orientation = TextOrientation.Upward;
                Frame.Width = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1);
                para = Frame.AddParagraph("50 % of Z");
                para.Format.Font.Bold = false;
                row.Cells[11].AddParagraph().AddFormattedText("Total of X+Y+Z", TextFormat.Bold); //Exam Name

                row.Cells[11].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[12].AddParagraph().AddFormattedText("%", TextFormat.Bold); //Exam Name

                row.Cells[12].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[12].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;


                row.Cells[13].AddParagraph().AddFormattedText("Remarks", TextFormat.Bold); //Exam Name

                row.Cells[13].Format.Alignment = ParagraphAlignment.Center;


                para.Format.Font.Bold = true;




                //para.Format.ClearAll();
                para.Format.Alignment = ParagraphAlignment.Center;


                int tempmearge = 0;
                double _TotalMaxMark = 0, _TotalMark = 0, _Sub_Mark = 0, _Sub_Max = 0;

                double[] _ExamWiseTotalMark = new double[_ExamDetails.Length];
                double[] _ExamWiseSubMark = new double[_ExamDetails.Length];

                DataSet Grade = MyExamMang.GetGradeDataSet(_ExamDetails[_ExamDetails.Length - 1].GradeMasterId);

                double sumOfUnitTestMark = 0;
                double sumOfUnitTestMax = 0;
                double halfYearlyMark = 0;
                double halfYearlyMax = 0;
                double AnnualMark = 0;
                double AnnualMax = 0;
                double _20ofX = 0;
                double _30ofY = 0;
                double _50ofZ = 0;


                double TotoalXYZ = 0;

                for (int Subjct = 0; Subjct < _SubDetails.Length; Subjct++)
                {
                    row = tb.AddRow();
                    row.Cells[0].AddParagraph().AddFormattedText(_SubDetails[Subjct].Name); //Subject Name

                    int k = 1;

                    sumOfUnitTestMark = 0;
                    sumOfUnitTestMax = 0;
                    halfYearlyMark = 0;
                    halfYearlyMax = 0;
                    AnnualMark = 0;
                    AnnualMax = 0;

                    for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                    {
                        if (_ExamDetails[ExamCount].Name.StartsWith("UT") || _ExamDetails[ExamCount].Name.StartsWith("FA"))
                        {
                            if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString() != "-1")
                            {
                                row.Cells[k].AddParagraph().AddFormattedText(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                                sumOfUnitTestMark = sumOfUnitTestMark + double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());

                            }
                            else
                                row.Cells[k].AddParagraph().AddFormattedText("-");
                            double max_Mark = MyExamMang.GetMaxMarkFromExamScheduleIdandSubjectId(_ExamDetails[ExamCount].Id, _SubDetails[Subjct].Id);
                            sumOfUnitTestMax = sumOfUnitTestMax + max_Mark;
                            k = k + 1;
                        }
                        else if (_ExamDetails[ExamCount].Name.StartsWith("Half") || _ExamDetails[ExamCount].Name.StartsWith("SA1"))
                        {
                            row.Cells[6].AddParagraph().AddFormattedText(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                            double.TryParse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString(), out halfYearlyMark);
                            double max_Mark = MyExamMang.GetMaxMarkFromExamScheduleIdandSubjectId(_ExamDetails[ExamCount].Id, _SubDetails[Subjct].Id);
                            halfYearlyMax = max_Mark;

                        }
                        else if (_ExamDetails[ExamCount].Name.StartsWith("Annual") || _ExamDetails[ExamCount].Name.StartsWith("SA2"))
                        {
                            row.Cells[7].AddParagraph().AddFormattedText(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                            double.TryParse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString(), out AnnualMark);
                            double max_Mark = MyExamMang.GetMaxMarkFromExamScheduleIdandSubjectId(_ExamDetails[ExamCount].Id, _SubDetails[Subjct].Id);
                            AnnualMax = max_Mark;
                        }

                    }
                    row.Cells[5].AddParagraph().AddFormattedText(sumOfUnitTestMark.ToString());

                    _20ofX = sumOfUnitTestMark * 20 / 100;

                    _30ofY = halfYearlyMark * 30 / 100;
                    _50ofZ = AnnualMark * 50 / 100;

                    double _XYZ = _20ofX + _30ofY + _50ofZ;


                    TotoalXYZ = TotoalXYZ + _XYZ;
                    row.Cells[8].AddParagraph().AddFormattedText(_20ofX.ToString("0.00"));
                    row.Cells[9].AddParagraph().AddFormattedText(_30ofY.ToString("0.00"));
                    row.Cells[10].AddParagraph().AddFormattedText(_50ofZ.ToString("0.00"));
                    row.Cells[11].AddParagraph().AddFormattedText(_XYZ.ToString("0.00"));



                    if (tempmearge == 0)
                    {
                        //  row.Cells[13].MergeDown = _SubDetails.Length;
                        row.Cells[13].MergeDown = _SubDetails.Length + 1;
                        tempmearge = 1;
                    }




                }

                row = tb.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("Sign of Class Teacher"); //Subject Name
                row = tb.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("Environmental Science"); //Subject Name

                row.Cells[11].AddParagraph().AddFormattedText(TotoalXYZ.ToString("0.00"));
                double Per = (TotoalXYZ / (_SubDetails.Length * 100)) * 100;
                Per = Math.Round(Per);
                row.Cells[12].AddParagraph().AddFormattedText(Per.ToString());
                tb.Borders.Visible = true;






                cel.Elements.Add(tb);




                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);
                paragraph.AddLineBreak();
                paragraph.AddFormattedText("Co-Scholastic Area", TextFormat.Underline | TextFormat.Bold);
                paragraph.AddLineBreak();

                tb = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                tb.AddColumn((PgSt.PageWidth - 80) / 5);
                tb.AddColumn((PgSt.PageWidth - 80) / 5);
                tb.AddColumn((PgSt.PageWidth - 80) / 5);
                tb.AddColumn((PgSt.PageWidth - 80) / 5);
                tb.AddColumn((PgSt.PageWidth - 80) / 5);


                row = tb.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("WORK EXPERIENCE");
                row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                //row.Cells[0].MergeDown = 1;
                row.Cells[1].AddParagraph().AddFormattedText("GAMES & SPORTS", TextFormat.Bold); //Exam Name

                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[2].AddParagraph().AddFormattedText("SCOUTS & GUIDES", TextFormat.Bold); //Exam Name

                row.Cells[2].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[3].AddParagraph().AddFormattedText("LITERARY & CULTURAL", TextFormat.Bold); //Exam Name

                row.Cells[3].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[4].AddParagraph().AddFormattedText("GENERAL CONDUCTS", TextFormat.Bold); //Exam Name

                row.Cells[4].Format.Alignment = ParagraphAlignment.Center;


                row = tb.AddRow();



                row.Cells[0].AddParagraph().AddFormattedText("Activity :");
                row.Cells[0].AddParagraph().AddLineBreak();
                row.Cells[0].AddParagraph().AddFormattedText("Grade :");

                row.Cells[1].AddParagraph().AddFormattedText("Activity :");
                row.Cells[1].AddParagraph().AddLineBreak();
                row.Cells[1].AddParagraph().AddFormattedText("Grade :");

                row.Cells[2].AddParagraph().AddFormattedText("Activity :");
                row.Cells[2].AddParagraph().AddLineBreak();
                row.Cells[2].AddParagraph().AddFormattedText("Grade :");

                row.Cells[3].AddParagraph().AddFormattedText("Activity :");
                row.Cells[3].AddParagraph().AddLineBreak();
                row.Cells[3].AddParagraph().AddFormattedText("Grade :");

                row.Cells[4].AddParagraph().AddFormattedText("Activity :");
                row.Cells[4].AddParagraph().AddLineBreak();
                row.Cells[4].AddParagraph().AddFormattedText("Grade :");

                tb.Borders.Visible = true;
                cel.Elements.Add(tb);


                // dominic new 




                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.AddLineBreak();
                paragraph.AddLineBreak();
                paragraph.AddLineBreak();


                tb = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                tb.AddColumn((PgSt.PageWidth - 80) / 2);
                tb.AddColumn((PgSt.PageWidth - 80) / 2);

                row = tb.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("CLASS TEACHER");
                row.Cells[0].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[1].AddParagraph().AddFormattedText("PRINCIPAL");
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;

                tb.Borders.Visible = false;
                cel.Elements.Add(tb);



            }
            return document;
        }


        private void CreatePrimaryReport(List<EvaluationReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, SchoolDetails _SchoolDetails, int _StudId, DataSet totalMarkRatios)
        {
            string _PhysicalPath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));
            //int _StudentId = 0;
            Document document = LoadPrimaryClassPDFPage(ClsObj, _ExamDetails, _SubDetails, _SchoolDetails, totalMarkRatios);

            const PdfFontEmbedding embedding = PdfFontEmbedding.Always;

            // ----------------------------------------------------------------------------------------

            const bool unicode = false;

            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF


            pdfRenderer.RenderDocument();

            // Save the document...
            string filename = "", MainName = "";

            if (_StudId == 0)
                MainName = Drp_SelectClass.SelectedItem.ToString();
            else
                MainName = Drp_SelectStudent.SelectedItem.ToString();

            filename = _PhysicalPath + "\\PDF_Files\\ECP_" + MainName + ".pdf";

            pdfRenderer.PdfDocument.Save(filename);
            // ...and start a viewer.
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "null", "window.open(\"OpenPdfPage.aspx?PdfName=ECP_" + MainName + ".pdf\",'Info','status=1, width=1000, height=700,,resizable=1,scrollbars=1');", true);

            //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=PRC_" + MainName + ".pdf\");", true);
            //Response.Redirect("OpenPdfPage.aspx?PdfName=PRC_" + MainName + ".pdf", false);

        }

        private Document LoadPrimaryClassPDFPage(List<EvaluationReportClass> ClsObj, ExamNode[] _ExamDetails, ExamNode[] _SubDetails, SchoolDetails _SchoolDetails, DataSet totalMarkRatios)

        {
            Document document = new Document();
            string path = GtSchoolLogo();
            //Dominic In bolosing school , they are using different remarks .
            int examFailCount = 0;
            int usingDifferentRemarks = 0;
            string mainRemark = "";
            double _doubleMinimumPersentage = 0;
            DataSet DifferentRemarks = null;
            int no_of_students = 0;

            no_of_students = TotalClassStrength(int.Parse(Drp_SelectClass.SelectedValue));

            if (UseDifferentRemarks(out _doubleMinimumPersentage))
            {
                usingDifferentRemarks = 1;
                DifferentRemarks = GetRemarks();
            }
            else
            {

                mainRemark = "Promoted To    ";
            }
            for (int i = 0; i < ClsObj.Count; i++)
            {
                PageSetup PgSt = document.DefaultPageSetup;
                PgSt.PageHeight = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(270);
                PgSt.PageWidth = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(297);

                PgSt.LeftMargin = 0;
                PgSt.RightMargin = 0;
                PgSt.TopMargin = 50;
                PgSt.BottomMargin = 0;

                PgSt.HeaderDistance = 0;
                PgSt.FooterDistance = 0;



                // Add a section to the document
                Section section = document.AddSection();

                MigraDoc.DocumentObjectModel.Tables.Table table = section.AddTable();



                table.Borders.Width = 2;
                //table.Borders.Left.Width = 0.5;
                //table.Borders.Right.Width = 0.5;
                table.Rows.LeftIndent = 25;
                MigraDoc.DocumentObjectModel.Tables.Column Col = table.AddColumn(PgSt.PageWidth - 50);
                MigraDoc.DocumentObjectModel.Tables.Row row = table.AddRow();
                row.Height = PgSt.PageHeight - 100;
                Col.Borders.Visible =true;

                MigraDoc.DocumentObjectModel.Tables.Cell cel = row.Cells[0];
                // Add a paragraph to the section
                Paragraph paragraph = section.AddParagraph();


                paragraph.Format.Alignment = ParagraphAlignment.Center;


                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Center;

                MigraDoc.DocumentObjectModel.Tables.Table tb = new MigraDoc.DocumentObjectModel.Tables.Table();


                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                tb.AddColumn((PgSt.PageWidth - 80) / 7);
                tb.AddColumn((PgSt.PageWidth - 80) - ((PgSt.PageWidth - 80) / 7));

                row = tb.AddRow();
                row.Cells[0].AddImage(path);
                row.Cells[0].MergeDown = 3;

                row.Cells[1].Format.Font.Size = 12;
                row.Cells[1].Format.Font.Bold = true;
                row.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                row.Cells[1].AddParagraph(MyUser.SchoolName.ToUpperInvariant());
                row = tb.AddRow();


                string _smallAddress = MyUser.GetSchoolSmallAdress();
                string[] Address = _smallAddress.Split('|');
                string _Address1 = "", _Address2 = "";
                if (Address.Length > 0)
                {
                    _Address1 = Address[0];
                }

                if (Address.Length > 1)
                {
                    _Address2 = Address[1];
                }


                row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[1].AddParagraph(_Address1);
                row = tb.AddRow();
                row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[1].AddParagraph(_Address2);
                row = tb.AddRow();
                row.Cells[1].Format.Font.Size = 16;
                row.Cells[1].Format.Font.Bold = true;
                //row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                //row.Cells[1].AddParagraph("REPORT CARD S1");




                cel.Elements.Add(tb);

                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);
                //paragraph.AddFormattedText("STUDENT INFORMATIONS", TextFormat.Underline | TextFormat.Bold);
                
                tb = new MigraDoc.DocumentObjectModel.Tables.Table();
                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);

                tb.AddColumn((PgSt.PageWidth - 80) / 3);
                
                //tb.AddColumn(((PgSt.PageWidth - 80) / 3)+100);
                tb.AddColumn(((PgSt.PageWidth - 80) / 3) + 150);

                tb.AddColumn((PgSt.PageWidth - 80) / 3);


                row = tb.AddRow();
               
                row.Cells[0].AddParagraph("Name: " + ClsObj[i].m_StudDetails.Name);

                row = tb.AddRow();
                row.Cells[0].AddParagraph("Class & Sec :" + ClsObj[i].m_StudDetails.Class);
                
                //row.Cells[2].AddParagraph("Roll No  :" + ClsObj[i].m_StudDetails.RollNum);


                tb.Borders.Visible = false;

                cel.Elements.Add(tb);

                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);
               
                //paragraph.AddFormattedText("REPORT CARD S1", TextFormat.Underline | TextFormat.Bold);
                //paragraph.AddLineBreak();

                tb = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 10);


                tb.AddColumn((PgSt.PageWidth - 80) / 2);
                tb.AddColumn(((PgSt.PageWidth - 80) / 3)-80);
                tb.AddColumn(((PgSt.PageWidth - 80) / 3)-80);




                #region tbl

                MigraDoc.DocumentObjectModel.Tables.Table tb1 = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb1.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 8);
               
                tb1.AddColumn((PgSt.PageWidth - 80) * 2 / 12+60);

                tb1.AddColumn(((PgSt.PageWidth - 80) / 12) - 2);
                tb1.AddColumn(((PgSt.PageWidth - 80) / 12) - 2);
                tb1.AddColumn(((PgSt.PageWidth - 80) / 12) - 2);
                //tb1.AddColumn(((PgSt.PageWidth - 80) / 12) - 2);

                row = tb1.AddRow();

                row.Cells[0].AddParagraph().AddFormattedText("MATA PELAJARAN KURIKULUM SWASTA");
                row.Cells[0].AddParagraph().AddFormattedText("PRIVATE CURRICULUM SUBJECTS");              
                row.Cells[1].AddParagraph().AddFormattedText("SEMESTER 1");
                row.Cells[2].AddParagraph().AddFormattedText("SEMESTER 2");
                //row.Cells[3].AddParagraph().AddFormattedText("SEMESTER 3");
                row.Cells[3].AddParagraph().AddFormattedText("JUMLAH");
                row.Cells[3].AddParagraph().AddFormattedText("Total");

                

                // dominic  Marks

                double _TotalMaxMark = 0;

                double[] _ExamWiseTotalMark = new double[_ExamDetails.Length];
                double[] _ExamWiseSubMark = new double[_SubDetails.Length];

                double[] _ExamWiseTotaMax = new double[_ExamDetails.Length];
                double[] _ExamWiseSubMax = new double[_SubDetails.Length];
                double[] _SubjectWiseMark = new double[_ExamDetails.Length];


                double MarksIn100 = 0;
                double MaxMarkForRemark = 0;
                int rank = 0;
                examFailCount = 0;
                for (int Subjct = 0; Subjct < _SubDetails.Length; Subjct++)
                {
                    MaxMarkForRemark = 0;
                    row = tb1.AddRow();
                    row.Cells[0].AddParagraph().AddFormattedText(_SubDetails[Subjct].Name.ToUpper()); //Subject Name

                    for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                    {

                        double max_Mark = MyExamMang.GetMaxMarkFromExamScheduleIdandSubjectId(_ExamDetails[ExamCount].Id, _SubDetails[Subjct].Id);


                        if (_ExamDetails[ExamCount].Name.Contains("Semester 1")|| _ExamDetails[ExamCount].Name.Contains("semester 1")|| _ExamDetails[ExamCount].Name.Contains("SEMESTER 1"))
                        {
                            if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString() != "-1")
                            {
                                row.Cells[1].AddParagraph().AddFormattedText(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                                _ExamWiseTotalMark[ExamCount] = _ExamWiseTotalMark[ExamCount] + double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                                if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark == 0)
                                    MarksIn100 = 0;
                                else
                                    MarksIn100 = ((double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString())) / max_Mark) * 100;
                                _ExamWiseSubMark[Subjct] = _ExamWiseSubMark[Subjct] + (double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString()));
                                _SubjectWiseMark[ExamCount] = _SubjectWiseMark[ExamCount] + MarksIn100;
                                MaxMarkForRemark = MaxMarkForRemark + MarksIn100;
                            }
                            else
                                row.Cells[1].AddParagraph().AddFormattedText("-");
                        }
                        else if (_ExamDetails[ExamCount].Name.Contains("Semester 2"))
                        {
                            if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString() != "-1")
                            {
                                row.Cells[2].AddParagraph().AddFormattedText(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                                _ExamWiseTotalMark[ExamCount] = _ExamWiseTotalMark[ExamCount] + double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                                if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark == 0)
                                    MarksIn100 = 0;
                                else
                                    MarksIn100 = ((double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString())) / max_Mark) * 100;
                                _ExamWiseSubMark[Subjct] = _ExamWiseSubMark[Subjct] + (double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString()));
                                _SubjectWiseMark[ExamCount] = _SubjectWiseMark[ExamCount] + MarksIn100;
                                MaxMarkForRemark = MaxMarkForRemark + MarksIn100;
                            }
                            else
                                row.Cells[2].AddParagraph().AddFormattedText("-");
                        }
                        //else if (_ExamDetails[ExamCount].Name.Contains("Third"))
                        //{
                        //    if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString() != "-1")
                        //    {
                        //        row.Cells[3].AddParagraph().AddFormattedText(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                        //        _ExamWiseTotalMark[ExamCount] = _ExamWiseTotalMark[ExamCount] + double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                        //        if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark == 0)
                        //            MarksIn100 = 0;
                        //        else
                        //            MarksIn100 = ((double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString())) / max_Mark) * 100;
                        //        _ExamWiseSubMark[Subjct] = _ExamWiseSubMark[Subjct] + (double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString()));
                        //        _SubjectWiseMark[ExamCount] = _SubjectWiseMark[ExamCount] + MarksIn100;
                        //        MaxMarkForRemark = MaxMarkForRemark + MarksIn100;
                        //    }
                        //    else
                        //        row.Cells[3].AddParagraph().AddFormattedText("-");
                        //}


                        _ExamWiseTotaMax[ExamCount] = _ExamWiseTotaMax[ExamCount] + max_Mark;
                        _ExamWiseSubMax[Subjct] = _ExamWiseSubMax[Subjct] + max_Mark;



                    }
                    double MaxMark = 100 * _ExamDetails.Length;
                    if (((MaxMarkForRemark / MaxMark) * 100) < _doubleMinimumPersentage)
                    {
                        examFailCount = examFailCount + 1;
                    }

                    MaxMarkForRemark = MaxMarkForRemark + MarksIn100;
                    row.Cells[3].AddParagraph().AddFormattedText(_ExamWiseSubMark[Subjct].ToString());


                }
                row = tb1.AddRow();
                //row.Borders.Visible = false;
                row.Height = 20;
                //row.Cells[0].AddParagraph().AddFormattedText("");
                //row.Cells[1].AddParagraph().AddFormattedText("");
                //row.Cells[2].AddParagraph().AddFormattedText("");
                //row.Cells[3].AddParagraph().AddFormattedText("");
                //row.Cells[4].AddParagraph().AddFormattedText("");
                //row = tb1.AddRow();

                //row = tb1.AddRow();
                //row = tb1.AddRow();
                row = tb1.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("JUMLAH MARKAH");
                row.Cells[0].AddParagraph().AddFormattedText("TOTAL SCORE"); //Subject Name

                double Total_Mark = 0;
                for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                {

                    if (_ExamDetails[ExamCount].Name.Contains("Semester 1") || _ExamDetails[ExamCount].Name.Contains("semester 1") || _ExamDetails[ExamCount].Name.Contains("SEMESTER 1"))
                    {
                        row.Cells[1].AddParagraph().AddFormattedText(_ExamWiseTotalMark[ExamCount].ToString());

                    }
                    else if (_ExamDetails[ExamCount].Name.Contains("Semester 2") || _ExamDetails[ExamCount].Name.Contains("semester 2") || _ExamDetails[ExamCount].Name.Contains("SEMESTER 2"))
                    {
                        row.Cells[2].AddParagraph().AddFormattedText(_ExamWiseTotalMark[ExamCount].ToString());

                    }
                    //else if (_ExamDetails[ExamCount].Name.Contains("Third"))
                    //{
                    //    row.Cells[3].AddParagraph().AddFormattedText(_ExamWiseTotalMark[ExamCount].ToString());

                    //}
                    Total_Mark = Total_Mark + _ExamWiseTotalMark[ExamCount];


                }
                row.Cells[3].AddParagraph().AddFormattedText(Total_Mark.ToString());


                // Chaitra - 09th Dec 2019 
                // Logic has to implemeted to calc DEDUCTION FOR ABSENTEEISM	

                row = tb1.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("TOLAK MARKAH KETIDKHADIRAN");
                row.Cells[0].AddParagraph().AddFormattedText("DEDUCTION FOR ABSENTEEISM");



                // Chaitra - 09th Dec 2019 
                // Logic has to implemeted to calc ACTUAL TOTAL SCORE		

                row = tb1.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("JUMLAH MARKAH SEBENAR");
                row.Cells[0].AddParagraph().AddFormattedText("ACTUAL TOTAL SCORE");

                double Total_Mark2 = 0;
                for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                {

                    if (_ExamDetails[ExamCount].Name.Contains("Semester 1") || _ExamDetails[ExamCount].Name.Contains("semester 1") || _ExamDetails[ExamCount].Name.Contains("SEMESTER 1"))
                    {
                        row.Cells[1].AddParagraph().AddFormattedText(_ExamWiseTotalMark[ExamCount].ToString());

                    }
                    else if (_ExamDetails[ExamCount].Name.Contains("Semester 2") || _ExamDetails[ExamCount].Name.Contains("semester 2") || _ExamDetails[ExamCount].Name.Contains("SEMESTER 2"))
                    {
                        row.Cells[2].AddParagraph().AddFormattedText(_ExamWiseTotalMark[ExamCount].ToString());

                    }

                    Total_Mark2 = Total_Mark2 + _ExamWiseTotalMark[ExamCount];


                }
                row.Cells[3].AddParagraph().AddFormattedText(Total_Mark.ToString());




                row = tb1.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("MARKAH PURATA");
                row.Cells[0].AddParagraph().AddFormattedText("AVERAGE SCORE"); //Subject Name


                double _Per = 0, ToatlMark = 0, ToatalMax = 0;

                for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                {
                    //  _Per =( _SubjectWiseMark[ExamCount] / (_SubDetails.Length* 100))*100;
                    _Per = (_ExamWiseTotalMark[ExamCount] / (_ExamWiseTotaMax[ExamCount])) * 100;

                    if (_ExamDetails[ExamCount].Name.Contains("Semester 1") || _ExamDetails[ExamCount].Name.Contains("semester 1") || _ExamDetails[ExamCount].Name.Contains("SEMESTER 1"))
                    {
                        row.Cells[1].AddParagraph().AddFormattedText(_Per.ToString("00.00"));

                    }
                    else if (_ExamDetails[ExamCount].Name.Contains("Semester 2") || _ExamDetails[ExamCount].Name.Contains("semester 2") || _ExamDetails[ExamCount].Name.Contains("SEMESTER 2"))
                    {

                        row.Cells[2].AddParagraph().AddFormattedText(_Per.ToString("00.00"));

                    }
                    //else if (_ExamDetails[ExamCount].Name.Contains("Third"))
                    //{

                    //    row.Cells[3].AddParagraph().AddFormattedText(_Per.ToString("00.00"));

                    //}

                    ToatlMark = ToatlMark + _ExamWiseTotalMark[ExamCount];
                    ToatalMax = ToatalMax + _ExamWiseTotaMax[ExamCount];

                }

                _Per = (ToatlMark / (ToatalMax)) * 100;

                row.Cells[3].AddParagraph().AddFormattedText(_Per.ToString("00.00"));


                // Chaitra - 09th Dec 2019 
                // Logic has to implemeted to calc POSITION IN CLASS		

                row = tb1.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("KEDUDUKAN DALAM KELAS");
                row.Cells[0].AddParagraph().AddFormattedText("POSITION IN CLASS");
                for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                {

                    if (_ExamDetails[ExamCount].Name.Contains("Semester 1") || _ExamDetails[ExamCount].Name.Contains("semester 1") || _ExamDetails[ExamCount].Name.Contains("SEMESTER 1"))
                    {
                        rank = GetRank(_ExamDetails[ExamCount].Id, ClsObj[i].m_StudDetails.Id);
                        row.Cells[1].AddParagraph().AddFormattedText(rank.ToString());
                    }
                    else if (_ExamDetails[ExamCount].Name.Contains("Semester 2") || _ExamDetails[ExamCount].Name.Contains("semester 2") || _ExamDetails[ExamCount].Name.Contains("SEMESTER 2"))
                    {
                        rank = GetRank(_ExamDetails[ExamCount].Id, ClsObj[i].m_StudDetails.Id);
                        row.Cells[2].AddParagraph().AddFormattedText(rank.ToString());
                    }
                }
                        // Chaitra - 09th Dec 2019 
                        // Logic has to implemeted to calc NUMBER OF STUDENTS IN CLASS		

                        row = tb1.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("BILANGAN PELAJAR DALAM KELAS");
                row.Cells[0].AddParagraph().AddFormattedText("NUMBER OF STUDENTS IN CLASS");
                row.Cells[1].AddParagraph().AddFormattedText(no_of_students.ToString());
                row.Cells[2].AddParagraph().AddFormattedText(no_of_students.ToString());




                tb.Borders.Visible = true;
                cel.Elements.Add(tb);
                #endregion

          
                #region tbl2
                MigraDoc.DocumentObjectModel.Tables.Table tb2 = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb2.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 8);

                tb2.AddColumn((PgSt.PageWidth - 80) * 2 / 12+60);

                tb2.AddColumn(((PgSt.PageWidth - 80) / 12) - 2);
                tb2.AddColumn(((PgSt.PageWidth - 80) / 12) - 2);
                tb2.AddColumn(((PgSt.PageWidth - 80) / 12) - 2);
                //tb2.AddColumn(((PgSt.PageWidth - 80) / 12) - 2);

                row = tb2.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("MATA PELAJARAN KURIKULUM KEBANGSAAN");
                row.Cells[0].AddParagraph().AddFormattedText("NATIONAL CURRICULUM SUBJECTS");
                row.Cells[1].AddParagraph().AddFormattedText("SEMESTER 1");
                row.Cells[2].AddParagraph().AddFormattedText("SEMESTER 2");
                //row.Cells[3].AddParagraph().AddFormattedText("SEMESTER 3");
                row.Cells[3].AddParagraph().AddFormattedText("JUMLAH");
                row.Cells[3].AddParagraph().AddFormattedText("TOTAL");

                double _TotalMaxMark1 = 0;

                double[] _ExamWiseTotalMark1 = new double[_ExamDetails.Length];
                double[] _ExamWiseSubMark1 = new double[_SubDetails.Length];

                double[] _ExamWiseTotaMax1= new double[_ExamDetails.Length];
                double[] _ExamWiseSubMax1 = new double[_SubDetails.Length];
                double[] _SubjectWiseMark1 = new double[_ExamDetails.Length];


                double Marks1In100 = 0;
                double MaxMarkForRemark1 = 0;
                examFailCount = 0;

                for (int Subjct = 0; Subjct < _SubDetails.Length; Subjct++)
                {
                    MaxMarkForRemark = 0;
                    row = tb2.AddRow();
                    row.Cells[0].AddParagraph().AddFormattedText(_SubDetails[Subjct].Name.ToUpper()); //Subject Name

                    for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                    {

                        double max_Mark = MyExamMang.GetMaxMarkFromExamScheduleIdandSubjectId(_ExamDetails[ExamCount].Id, _SubDetails[Subjct].Id);


                        if (_ExamDetails[ExamCount].Name.Contains("Semester 1") || _ExamDetails[ExamCount].Name.Contains("semester 1") || _ExamDetails[ExamCount].Name.Contains("SEMESTER 1"))
                        {
                            if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString() != "-1")
                            {
                                row.Cells[1].AddParagraph().AddFormattedText(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                                _ExamWiseTotalMark1[ExamCount] = _ExamWiseTotalMark1[ExamCount] + double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                                if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark == 0)
                                    MarksIn100 = 0;
                                else
                                    MarksIn100 = ((double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString())) / max_Mark) * 100;
                                _ExamWiseSubMark1[Subjct] = _ExamWiseSubMark1[Subjct] + (double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString()));
                                _SubjectWiseMark1[ExamCount] = _SubjectWiseMark1[ExamCount] + MarksIn100;
                                MaxMarkForRemark1 = MaxMarkForRemark1 + MarksIn100;
                            }
                            else
                                row.Cells[1].AddParagraph().AddFormattedText("-");
                        }
                        else if (_ExamDetails[ExamCount].Name.Contains("Semester 2") || _ExamDetails[ExamCount].Name.Contains("semester 2") || _ExamDetails[ExamCount].Name.Contains("SEMESTER 2"))
                        {
                            if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString() != "-1")
                            {
                                row.Cells[2].AddParagraph().AddFormattedText(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                                _ExamWiseTotalMark1[ExamCount] = _ExamWiseTotalMark1[ExamCount] + double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                                if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark == 0)
                                    Marks1In100 = 0;
                                else
                                    Marks1In100 = ((double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString())) / max_Mark) * 100;
                                _ExamWiseSubMark1[Subjct] = _ExamWiseSubMark1[Subjct] + (double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString()));
                                _SubjectWiseMark1[ExamCount] = _SubjectWiseMark1[ExamCount] + MarksIn100;
                                MaxMarkForRemark1 = MaxMarkForRemark1 + MarksIn100;
                            }
                            else
                                row.Cells[2].AddParagraph().AddFormattedText("-");
                        }
                        //else if (_ExamDetails[ExamCount].Name.Contains("Third"))
                        //{
                        //    if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString() != "-1")
                        //    {
                        //        row.Cells[3].AddParagraph().AddFormattedText(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                        //        _ExamWiseTotalMark[ExamCount] = _ExamWiseTotalMark[ExamCount] + double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString());
                        //        if (ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark == 0)
                        //            MarksIn100 = 0;
                        //        else
                        //            MarksIn100 = ((double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString())) / max_Mark) * 100;
                        //        _ExamWiseSubMark[Subjct] = _ExamWiseSubMark[Subjct] + (double.Parse(ClsObj[i].m_ExamMarks[ExamCount, Subjct].MaxMark.ToString()));
                        //        _SubjectWiseMark[ExamCount] = _SubjectWiseMark[ExamCount] + MarksIn100;
                        //        MaxMarkForRemark = MaxMarkForRemark + MarksIn100;
                        //    }
                        //    else
                        //        row.Cells[3].AddParagraph().AddFormattedText("-");
                        //}


                        _ExamWiseTotaMax1[ExamCount] = _ExamWiseTotaMax1[ExamCount] + max_Mark;
                        _ExamWiseSubMax1[Subjct] = _ExamWiseSubMax1[Subjct] + max_Mark;



                    }
                    double MaxMark = 100 * _ExamDetails.Length;
                    if (((MaxMarkForRemark / MaxMark) * 100) < _doubleMinimumPersentage)
                    {
                        examFailCount = examFailCount + 1;
                    }

                    MaxMarkForRemark1 = MaxMarkForRemark1 + MarksIn100;
                    row.Cells[3].AddParagraph().AddFormattedText(_ExamWiseSubMark1[Subjct].ToString());
                }

                row = tb2.AddRow();
                //row.Borders.Visible = false;
                row.Height = 20;

                row = tb2.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("JUMLAH MARKAH");
                row.Cells[0].AddParagraph().AddFormattedText("TOTAL SCORE"); //Subject Name

                double Total_Mark1 = 0;
                for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                {

                    if (_ExamDetails[ExamCount].Name.Contains("Semester 1") || _ExamDetails[ExamCount].Name.Contains("semester 1") || _ExamDetails[ExamCount].Name.Contains("SEMESTER 1"))
                    {
                        row.Cells[1].AddParagraph().AddFormattedText(_ExamWiseTotalMark[ExamCount].ToString());

                    }
                    else if (_ExamDetails[ExamCount].Name.Contains("Semester 2") || _ExamDetails[ExamCount].Name.Contains("semester 2") || _ExamDetails[ExamCount].Name.Contains("SEMESTER 2"))
                    {
                        row.Cells[2].AddParagraph().AddFormattedText(_ExamWiseTotalMark[ExamCount].ToString());

                    }
                    //else if (_ExamDetails[ExamCount].Name.Contains("Third"))
                    //{
                    //    row.Cells[3].AddParagraph().AddFormattedText(_ExamWiseTotalMark[ExamCount].ToString());

                    //}
                    Total_Mark1 = Total_Mark1 + _ExamWiseTotalMark[ExamCount];


                }
                row.Cells[3].AddParagraph().AddFormattedText(Total_Mark.ToString());

                row = tb2.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("TOLAK MARKAH KETIDKHADIRAN");
                row.Cells[0].AddParagraph().AddFormattedText("DEDUCTION FOR ABSENTEEISM");
                row = tb2.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("JUMLAH MARKAH SEBENAR");
                row.Cells[0].AddParagraph().AddFormattedText("ACTUAL TOTAL SCORE");

                double Total_Mark3 = 0;
                for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                {

                    if (_ExamDetails[ExamCount].Name.Contains("Semester 1") || _ExamDetails[ExamCount].Name.Contains("semester 1") || _ExamDetails[ExamCount].Name.Contains("SEMESTER 1"))
                    {
                        row.Cells[1].AddParagraph().AddFormattedText(_ExamWiseTotalMark[ExamCount].ToString());

                    }
                    else if (_ExamDetails[ExamCount].Name.Contains("Semester 2") || _ExamDetails[ExamCount].Name.Contains("semester 2") || _ExamDetails[ExamCount].Name.Contains("SEMESTER 2"))
                    {
                        row.Cells[2].AddParagraph().AddFormattedText(_ExamWiseTotalMark[ExamCount].ToString());

                    }
                    
                    Total_Mark3 = Total_Mark3 + _ExamWiseTotalMark[ExamCount];


                }
                row.Cells[3].AddParagraph().AddFormattedText(Total_Mark.ToString());

                row = tb2.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("MARKAH PURATA");
                row.Cells[0].AddParagraph().AddFormattedText("AVERAGE SCORE");

                //double _Per = 0, ToatlMark = 0, ToatalMax = 0;

                for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                {
                    //  _Per =( _SubjectWiseMark[ExamCount] / (_SubDetails.Length* 100))*100;
                    _Per = (_ExamWiseTotalMark[ExamCount] / (_ExamWiseTotaMax[ExamCount])) * 100;

                    if (_ExamDetails[ExamCount].Name.Contains("Semester 1") || _ExamDetails[ExamCount].Name.Contains("semester 1") || _ExamDetails[ExamCount].Name.Contains("SEMESTER 1"))
                    {
                        row.Cells[1].AddParagraph().AddFormattedText(_Per.ToString("00.00"));

                    }
                    else if (_ExamDetails[ExamCount].Name.Contains("Semester 2") || _ExamDetails[ExamCount].Name.Contains("semester 2") || _ExamDetails[ExamCount].Name.Contains("SEMESTER 2"))
                    {

                        row.Cells[2].AddParagraph().AddFormattedText(_Per.ToString("00.00"));

                    }
                  
                    ToatlMark = ToatlMark + _ExamWiseTotalMark[ExamCount];
                    ToatalMax = ToatalMax + _ExamWiseTotaMax[ExamCount];

                }

                _Per = (ToatlMark / (ToatalMax)) * 100;

                row.Cells[3].AddParagraph().AddFormattedText(_Per.ToString("00.00"));


                row = tb2.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("KEDUDUKAN DALAM KELAS");
                row.Cells[0].AddParagraph().AddFormattedText("POSITION IN CLASS");
                for (int ExamCount = 0; ExamCount < _ExamDetails.Length; ExamCount++)
                {

                    if (_ExamDetails[ExamCount].Name.Contains("Semester 1") || _ExamDetails[ExamCount].Name.Contains("semester 1") || _ExamDetails[ExamCount].Name.Contains("SEMESTER 1"))
                    {
                        rank = GetRank(_ExamDetails[ExamCount].Id, ClsObj[i].m_StudDetails.Id);
                        row.Cells[1].AddParagraph().AddFormattedText(rank.ToString());
                    }
                    else if (_ExamDetails[ExamCount].Name.Contains("Semester 2") || _ExamDetails[ExamCount].Name.Contains("semester 2") || _ExamDetails[ExamCount].Name.Contains("SEMESTER 2"))
                    {
                        rank = GetRank(_ExamDetails[ExamCount].Id, ClsObj[i].m_StudDetails.Id);
                        row.Cells[2].AddParagraph().AddFormattedText(rank.ToString());
                    }
                }


                row = tb2.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("BILANGAN PELAJAR DALAM KELAS");
                row.Cells[0].AddParagraph().AddFormattedText("NUMBER OF STUDENTS IN CLASS");
                row.Cells[1].AddParagraph().AddFormattedText(no_of_students.ToString());
                row.Cells[2].AddParagraph().AddFormattedText(no_of_students.ToString());


                #endregion

                row = tb1.AddRow();
                row.Borders.Visible = false;
                row = tb1.AddRow();
                row.Borders.Visible = false;

                row = tb2.AddRow();
                row.Borders.Visible = false;
                row = tb2.AddRow();
                row.Borders.Visible = false;


                #region tbl3

                MigraDoc.DocumentObjectModel.Tables.Table tb3 = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb3.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 8);

                tb3.AddColumn(((PgSt.PageWidth - 80) * 2/3)-135);
                row = tb3.AddRow();
                row.Height = 50;
            

                row.Cells[0].AddParagraph().AddFormattedText("ULASAN GURU TINGKATAN");
                row.Cells[0].AddParagraph().AddFormattedText("FORM TEACHER'S COMMENTS");



                row.Cells[0].AddParagraph().AddLineBreak();
                
                #endregion

                row = tb3.AddRow();
                row.Borders.Visible = false;
                row = tb3.AddRow();
                row.Borders.Visible = false;

                #region tbl4
                MigraDoc.DocumentObjectModel.Tables.Table tb4 = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb4.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 8);

                tb4.AddColumn((PgSt.PageWidth - 80) * 2 / 12);

                tb4.AddColumn(((PgSt.PageWidth - 80) / 12) - 2);
                tb4.AddColumn(((PgSt.PageWidth - 80) / 12) - 2);
                tb4.AddColumn(((PgSt.PageWidth - 80) / 12) - 2);
                tb4.AddColumn(((PgSt.PageWidth - 80) / 12) - 2);

                row = tb4.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("GURU TINGKATAN");
                row.Cells[0].AddParagraph().AddFormattedText("FORM TEACHER");
                row.Cells[1].AddParagraph().AddFormattedText("KOKURIKULUM");
                row.Cells[1].AddParagraph().AddFormattedText("CO-CURRICULUM");
                row.Cells[2].AddParagraph().AddFormattedText("HAL EHWAL PELAJAR");
                row.Cells[2].AddParagraph().AddFormattedText("DISCIPLINE MASTER");
                row.Cells[3].AddParagraph().AddFormattedText("HAL EHWAL AKADEMIK");
                row.Cells[3].AddParagraph().AddFormattedText("ACADEMIC");
                row.Cells[4].AddParagraph().AddFormattedText("PENGETUA");
                row.Cells[4].AddParagraph().AddFormattedText("PRINCIPAL");

                row = tb4.AddRow();
                row.Height = 50;

              
                #endregion

         
                row = tb4.AddRow();
                row.Borders.Visible = false;
                row = tb4.AddRow();
                row.Borders.Visible = false;

                #region tbl5
                MigraDoc.DocumentObjectModel.Tables.Table tb5 = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb5.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 8);

                tb5.AddColumn((PgSt.PageWidth - 80) * 2 / 6);

                row = tb5.AddRow();
                row.Borders.Visible = false;

                row.Cells[0].AddParagraph().AddLineBreak();
                row.Cells[0].AddParagraph().AddFormattedText("TANDATANGAN IBUBAPA");
                row.Cells[0].AddParagraph().AddFormattedText("PARENT'S SIGNATURE  _______________________________");

                #endregion

                #region tbl6
                MigraDoc.DocumentObjectModel.Tables.Table tb6 = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb6.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 8);
                
                tb6.AddColumn((PgSt.PageWidth - 80) * 2 / 16 - 25);
                tb6.AddColumn(((PgSt.PageWidth - 80) / 12) - 14.5);
                tb6.AddColumn(((PgSt.PageWidth - 80) / 12) - 14.5);

                row = tb6.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("KELAKUAN");
                row.Cells[0].AddParagraph().AddFormattedText("CONDUCT");
                row.Cells[1].AddParagraph().AddFormattedText("SEMESTER 1");
                row.Cells[2].AddParagraph().AddFormattedText("SEMESTER 2");

                row = tb6.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("PUJIAN ");
                row.Cells[0].AddParagraph().AddFormattedText("COMENDATIONS");

                row = tb6.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("MERIT");
                row.Cells[0].AddParagraph().AddFormattedText("MERIT");

                row = tb6.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("MERIT MULIA");
                row.Cells[0].AddParagraph().AddFormattedText("GREAT MERIT");

                row = tb6.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("AMARAN");
                row.Cells[0].AddParagraph().AddFormattedText("WARNINGS");

                row = tb6.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("DEMERIT");
                row.Cells[0].AddParagraph().AddFormattedText("DEMERIT");

                row = tb6.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("DEMERIT SERIUS");
                row.Cells[0].AddParagraph().AddFormattedText("SERIOUS DEMERIT");

                row = tb6.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("GRED KELAKUAN");
                row.Cells[0].AddParagraph().AddFormattedText("CONDUCT GRADE");



                #endregion

                row = tb6.AddRow();
                row.Borders.Visible = false;
                row = tb6.AddRow();
                row.Borders.Visible = false;

                #region tbl25

                MigraDoc.DocumentObjectModel.Tables.Table tb25 = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb25.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 8);

                
                tb25.AddColumn(((PgSt.PageWidth - 80) / 10) + 18);
                tb25.AddColumn(((PgSt.PageWidth - 80) / 12) - 2);
                tb25.AddColumn(((PgSt.PageWidth - 80) / 12) - 2);

                row = tb25.AddRow();
                row.Height = 232;
                

                #endregion



                row = tb25.AddRow();
                row.Borders.Visible = false;
                row = tb25.AddRow();
                row.Borders.Visible = false;
              
        
              
               
                #region tbl7

                MigraDoc.DocumentObjectModel.Tables.Table tb7 = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb7.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 8);
               
                tb7.AddColumn(((PgSt.PageWidth - 80) / 10) + 20-5);

                tb7.AddColumn(((PgSt.PageWidth - 80) / 12) +2- 12);
                tb7.AddColumn(((PgSt.PageWidth - 80) / 12) +2-12);

                row = tb7.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("KEHADIRAN SEKOLAH");
                row.Cells[0].AddParagraph().AddFormattedText("SCHOOL ATTENDANCE");
                row.Cells[1].AddParagraph().AddFormattedText("SEMESTER 1");
                row.Cells[2].AddParagraph().AddFormattedText("SEMESTER 2");

                row = tb7.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("PUJIAN ");
                row.Cells[0].AddParagraph().AddFormattedText("COMENDATIONS");

                row = tb7.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("MERIT");
                row.Cells[0].AddParagraph().AddFormattedText("MERIT");

                row = tb7.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("MERIT MULIA");
                row.Cells[0].AddParagraph().AddFormattedText("GREAT MERIT");

                row = tb7.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("AMARAN");
                row.Cells[0].AddParagraph().AddFormattedText("WARNINGS");

                row = tb7.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("DEMERIT");
                row.Cells[0].AddParagraph().AddFormattedText("DEMERIT");

                row = tb7.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("DEMERIT SERIUS");
                row.Cells[0].AddParagraph().AddFormattedText("SERIOUS DEMERIT");

                row = tb7.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("GRED KELAKUAN");
                row.Cells[0].AddParagraph().AddFormattedText("CONDUCT GRADE");
                
                #endregion


                #region tbl8
                MigraDoc.DocumentObjectModel.Tables.Table tb8 = new MigraDoc.DocumentObjectModel.Tables.Table();

                tb8.Format.Font = new MigraDoc.DocumentObjectModel.Font("Calibri", 8);

                
                tb8.AddColumn((PgSt.PageWidth - 80) * 3/ 18);

                tb8.AddColumn((PgSt.PageWidth - 80) / 5+35);
                tb8.AddColumn(((PgSt.PageWidth - 80) / 12) - 2);

                row = tb8.AddRow();
                row.Cells[0].AddParagraph().AddFormattedText("BIL. HARI BERSEKOLAH");
                row.Cells[0].AddParagraph().AddFormattedText("TOTAL SCHOOL DAYS");
                row.Cells[1].AddParagraph().AddFormattedText("SEMESTER 1 (2/1/19 - 24/5/19)          99");
                row.Cells[2].AddParagraph().AddFormattedText("HARI");
                row.Cells[2].AddParagraph().AddFormattedText("DAYS");

                row = tb8.AddRow();
                row.Cells[1].AddParagraph().AddFormattedText("SEMESTER 2");
                row.Cells[2].AddParagraph().AddFormattedText("HARI");
                row.Cells[2].AddParagraph().AddFormattedText("DAYS");

                #endregion

                row = tb.AddRow();
                tb1.Borders.Visible = true;

                row.Cells[0].Elements.Add(tb1);

                tb2.Borders.Visible = true;

                row.Cells[1].Elements.Add(tb2);

                tb3.Borders.Visible = true;

                row.Cells[0].Elements.Add(tb3);

                tb4.Borders.Visible = true;

                row.Cells[0].Elements.Add(tb4);


                tb5.Borders.Visible = true;

                row.Cells[0].Elements.Add(tb5);

                tb6.Borders.Visible = true;

                row.Cells[1].Elements.Add(tb6);

                tb25.Borders.Visible = false;

                row.Cells[2].Elements.Add(tb25);

                tb7.Borders.Visible = true;


                row.Cells[2].Elements.Add(tb7);

                tb8.Borders.Visible = true;

                row.Cells[1].Elements.Add(tb8);

                tb.Borders.Visible = true;

                paragraph = cel.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.AddLineBreak();
                paragraph.AddLineBreak();
                paragraph.AddLineBreak();
                tb.Borders.Visible = false;
                
             }

            return document;
        }

        private bool UseDifferentRemarks(out double _doubleMinimumPersentage)
        {
            _doubleMinimumPersentage = 0;
            bool differentRemarks = false; ;
            int Val = 0;
            string sql = "select tblconfiguration.Value, tblconfiguration.SubValue from tblconfiguration where tblconfiguration.Name='Remarks'  and Module='Exam Report'";

            OdbcDataReader _Reader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (_Reader.HasRows)
            {
                if (int.TryParse(_Reader.GetValue(0).ToString(), out Val))
                {
                    if (Val == 1)
                    {
                        differentRemarks = true;
                        double.TryParse(_Reader.GetValue(1).ToString(), out _doubleMinimumPersentage);
                    }
                }
            }
            return differentRemarks;
        }

        private DataSet GetRemarks()
        {
            string sql = "select tblexamremark.ExamCount, tblexamremark.Remarks from tblexamremark where tblexamremark.Active=1";
            DataSet _RemarksDetails = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return _RemarksDetails;
        }


        public int TotalClassStrength( int classId)
        {
            string sql = "select count(*) from(select tblview_student.Id, tblview_student.StudentName, tblview_student.RollNo, tblclass.ClassName,date_Format(tblview_student.DOB,'%d/%m/%Y') as DOB , tblview_student.AdmitionNo,tblview_student.GardianName,tblview_student.MothersName, tblview_student.ResidencePhNo ,tblview_student.Address ,tblview_student.sex ,tblbatch.BatchName ,tblview_student.state  from tblview_student inner join tblclass on tblclass.Id= tblview_student.ClassId inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblview_student.Id  inner join tblbatch on tblbatch.id= tblview_student.JoinBatch  where tblview_student.ClassId="+ classId + " and tblstudentclassmap.BatchId="+MyUser.CurrentBatchId+")t";
            int strength = MyExamMang.m_MysqlDb.ExecuteScalar(sql);
            return strength;
        }

        public int GetRank(int ExamId,int StdId)
        {
            string sql = "select rank from tblstudentmark where StudId=" + StdId + " and ExamSchId=" + ExamId + "";
            int rank= MyExamMang.m_MysqlDb.ExecuteScalar(sql);
            return rank;
        }

        private string GetLastRemarkFromCount(int examFailCount, DataSet DifferentRemarks)
        {
            string NextClass = GetNextClass();
            if (examFailCount == 0)
            {
                return "Promoted to  Class -" + NextClass + ".                               Remark : Passed";
            }
            else if (examFailCount > 0)
            {
                int Count = 0;
                foreach (DataRow dr in DifferentRemarks.Tables[0].Rows)
                {
                    int.TryParse(dr[0].ToString(), out Count);

                    if (Count == examFailCount)
                    {
                        return " Promoted to  Class -" + NextClass + ". Remark : " + dr[1].ToString() + " ";
                    }

                }
                if (examFailCount > Count)
                    return " Detained in Class -" + Drp_SelectClass.SelectedItem;
            }
            return "";

        }

        private string GetNextClass()
        {
            int _ClassId = 0;
            int.TryParse(Drp_SelectClass.SelectedValue, out _ClassId);

            string sql = " select tblstandard.Name from tblclass inner join tblstandard on  tblstandard.Id= tblclass.Standard+1 where tblclass.Id=" + _ClassId;
            //   string sql = "select tblstandard.name from tblstandard where tblstandard.Id=" + _ClassId;
            OdbcDataReader _Reader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (_Reader.HasRows)
            {
                return _Reader.GetValue(0).ToString();
            }
            else return "";

        }

        private string GetClassDivision(double _Per)
        {
            string sql = "select tblexamdivisionpersentage.Division, tblexamdivisionpersentage.MinPersentage  from tblexamdivisionpersentage ";
            DataSet Division = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (Division != null && Division.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Division.Tables[0].Rows)
                {
                    if (_Per >= double.Parse(dr[1].ToString()))
                    {
                        return dr[0].ToString();
                    }
                }
            }
            return "";

        }
    }
}