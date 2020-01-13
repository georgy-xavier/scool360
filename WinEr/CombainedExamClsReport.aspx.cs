using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;

namespace WinEr
{
    public partial class CombainedExamClsReport : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        private StudentManagerClass MyStudMang;
        private ExamManage MyExamMang;
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }

            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MyExamMang = MyUser.GetExamObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (MyExamMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
          
            else if (!MyUser.HaveActionRignt(885))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    InitializePage();
                }
            }
        }

        protected void Btn_show_Click(object sender, EventArgs e)
        {
            Lbl_Err.Text = "";
            LoadExamDetailsToGrid();            
        }        

        protected void Img_Export_Click(object sender, ImageClickEventArgs e)
        {
            DataSet Details = (DataSet)ViewState["CombainedExamClassDetails"];
            Details.Tables[0].Columns.Remove("Id");
            string FileName = "CombainedClassExamReport";
            string _ReportName = "Combained Class ExamReport";
            if (!WinEr.ExcelUtility.ExportDataToExcel(Details, _ReportName, FileName, MyUser.ExcelHeader))
            {
                Lbl_Err.Text = "This function need Ms office";
            }
        }

        protected void Drp_Calss_SelectedIndexChanged(object sender, EventArgs e)
        {
            Lbl_Err.Text = "";
            loadExamsForTheSelectedClass();
        }
        
        #endregion

        #region Methods

        private void LoadExamDetailsToGrid()
        {
            if (Drp_Calss.SelectedValue != "0")
            {
                int examGradeMasterId = 0;//Dominic Now I'm using this id for Getting Default GreadeMaster.
                string ExamScheduledId = GetExamScheduleId();
               
                if (ExamScheduledId != "")
                {
                    DataSet MarkDetails      = GetMarkDetails(ExamScheduledId, Drp_Calss.SelectedValue);
                    DataRow dr = MarkDetails.Tables[0].NewRow();
                    dr["id"] = "000";
                    dr["StudentName"] = "000";
                    dr["RollNo"] = "0000";
                    dr["TotalMark"] = "000";
                    dr["TotalMax"] = "000";
                    dr["ExamName"] = "0000";

                    MarkDetails.Tables[0].Rows.Add(dr);
                    
                    if (MarkDetails != null && MarkDetails.Tables != null && MarkDetails.Tables[0].Rows.Count > 0)
                    {
                        DataSet GradeMaster = MyExamMang.GetGradeDataSet(examGradeMasterId);
                        DataSet FeeDueStudent = MyExamMang.GetFeeDueStudent(Drp_Calss.SelectedValue.ToString());
                        DataSet PersentageRatio = MyExamMang.GetExamPersentageRatio(Drp_Calss.SelectedValue);
                        DataSet ArrangedMarkList = ArrangeMarksListUsingPersentageRatios(MarkDetails, PersentageRatio, GradeMaster, FeeDueStudent);
                        DataSet ResultList = GetArrangedMarkList(ArrangedMarkList);// Dominic :- Need to calciulate the sum of total marks;
                        showValuesToGrid(ResultList);
                        ViewState["CombainedExamClassDetails"] = ResultList;
                    }
                    else
                    {
                        Lbl_Err.Text = "Exam mark details not found";
                    }
                }
                else
                {
                    Lbl_Err.Text = "Select Exams";
                }
            }
            else
            {
                Lbl_Err.Text = "Select Class";
            }
        }

      

        private void showValuesToGrid(DataSet ArrangedMarkList)
        {
            showReportGridColumns();
            if (ArrangedMarkList != null && ArrangedMarkList.Tables[0].Rows.Count > 0)
            {
                Grd_Result.DataSource = ArrangedMarkList;
                Grd_Result.DataBind();
                Pnl_ReportArea.Visible = true;
            }
            else
            {
                Grd_Result.DataSource = null;
                Grd_Result.DataBind();
                Pnl_ReportArea.Visible = false;
            }
            hideReportGridColumns();
            
        }

        private DataSet GetArrangedMarkList(DataSet MarkDetails)
        {
            DataSet ReportDataset = CreateReportDataset();
            
            DataRow     dr=  ReportDataset.Tables["reports"].NewRow();

            int Rank = 0;
            
            if (MarkDetails != null && MarkDetails.Tables[0].Rows.Count > 0)
            {
                int Count=MarkDetails.Tables[0].Rows.Count;

                for (int j = 0; j < Count; j++)
                {
                    int last_exchange = 0;

                    for (int i = 0; i < Count - 1; i++)
                    {
                        if (double.Parse(MarkDetails.Tables[0].Rows[i]["ObtainedMark"].ToString()) < double.Parse(MarkDetails.Tables[0].Rows[i + 1]["ObtainedMark"].ToString()))
                        {

                            dr["Id"] = MarkDetails.Tables[0].Rows[i]["Id"];
                            dr["StudentName"] = MarkDetails.Tables[0].Rows[i]["StudentName"];
                            dr["Roll"] = MarkDetails.Tables[0].Rows[i]["Roll"];
                            dr["ObtainedMark"] = MarkDetails.Tables[0].Rows[i]["ObtainedMark"];
                            dr["MaxMark"] = MarkDetails.Tables[0].Rows[i]["MaxMark"];
                            dr["Avg"] = MarkDetails.Tables[0].Rows[i]["Avg"];
                            dr["Grade"] = MarkDetails.Tables[0].Rows[i]["Grade"];
                            dr["Result"] = MarkDetails.Tables[0].Rows[i]["Result"];
                            dr["Rank"] = MarkDetails.Tables[0].Rows[i]["Rank"];
                            dr["Remark"] = MarkDetails.Tables[0].Rows[i]["Remark"];


                            MarkDetails.Tables[0].Rows[i]["Id"] = MarkDetails.Tables[0].Rows[i + 1]["Id"];
                            MarkDetails.Tables[0].Rows[i]["StudentName"] = MarkDetails.Tables[0].Rows[i + 1]["StudentName"];
                            MarkDetails.Tables[0].Rows[i]["Roll"] = MarkDetails.Tables[0].Rows[i + 1]["Roll"];
                            MarkDetails.Tables[0].Rows[i]["ObtainedMark"] = MarkDetails.Tables[0].Rows[i + 1]["ObtainedMark"];
                            MarkDetails.Tables[0].Rows[i]["MaxMark"] = MarkDetails.Tables[0].Rows[i + 1]["MaxMark"];
                            MarkDetails.Tables[0].Rows[i]["Avg"] = MarkDetails.Tables[0].Rows[i + 1]["Avg"];
                            MarkDetails.Tables[0].Rows[i]["Grade"] = MarkDetails.Tables[0].Rows[i + 1]["Grade"];
                            MarkDetails.Tables[0].Rows[i]["Result"] = MarkDetails.Tables[0].Rows[i + 1]["Result"];
                            MarkDetails.Tables[0].Rows[i]["Rank"] = MarkDetails.Tables[0].Rows[i + 1]["Rank"];
                            MarkDetails.Tables[0].Rows[i]["Remark"] = MarkDetails.Tables[0].Rows[i + 1]["Remark"];

                            MarkDetails.Tables[0].Rows[i + 1]["Id"] = dr["Id"];
                            MarkDetails.Tables[0].Rows[i + 1]["StudentName"] = dr["StudentName"];
                            MarkDetails.Tables[0].Rows[i + 1]["Roll"] = dr["Roll"];
                            MarkDetails.Tables[0].Rows[i + 1]["ObtainedMark"] = dr["ObtainedMark"];
                            MarkDetails.Tables[0].Rows[i + 1]["MaxMark"] = dr["MaxMark"];
                            MarkDetails.Tables[0].Rows[i + 1]["Avg"] = dr["Avg"];
                            MarkDetails.Tables[0].Rows[i + 1]["Grade"] = dr["Grade"];
                            MarkDetails.Tables[0].Rows[i + 1]["Result"] = dr["Result"];
                            MarkDetails.Tables[0].Rows[i + 1]["Rank"] = dr["Rank"];
                            MarkDetails.Tables[0].Rows[i + 1]["Remark"] = dr["Remark"];
                            
                            last_exchange = i;
                        }
                    }                    
                }
            }
            //foreach (DataRow dr1 in MarkDetails.Tables[0].Rows)
            //{
            //    Rank = Rank + 1;
            //    dr1["Rank"] = Rank.ToString();
            //}
           
            return MarkDetails;
        }

        private string GetSortDirection1(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = Session["SortExpression1"] as string;


            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = Session["SortDirection1"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }
            // Save new values in ViewState.
            Session["SortDirection1"] = sortDirection;
            Session["SortExpression1"] = column;

            return sortDirection;
        }


        private DataSet ArrangeMarksListUsingPersentageRatios(DataSet MarkDetails, DataSet PersentageRatio, DataSet GradeMasterSet, DataSet FeeDueStudent)
        {
            int temp = 0,Id=0;

            string StudentId = "";
            string StudentName = "";
            string RollNum = "";

            DataSet MarkDataset = CreateReportDataset();
            DataRow drow;

            double UTMarks = 0;
            double HalfMarks = 0;
            double AnualMarks = 0;
            double OtherMark=0;

            double UTMax = 0;
            double HalfMax = 0;
            double AnualMax = 0;
            double OtherMax = 0;

            double UTPersentage=100;
            double HalfPersentage=100;
            double AnualPersentage=100;

            double Utin100=0;
            double Halfin100=0;
            double Anualin100=0;

            double UtinPerMark = 0;
            double HalfinPerMark = 0;
            double AnualinPerMark = 0;


            double TotalMarks = 0;
            double TotalMax = 0;

            double avg = 0;
            string Grade = "";

            if (PersentageRatio != null && PersentageRatio.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow dr in PersentageRatio.Tables[0].Rows)
                {
                    if(double.Parse(dr["UT"].ToString())!=0)
                        UTPersentage=double.Parse(dr["UT"].ToString());
                    if (double.Parse(dr["Half"].ToString()) != 0)
                        HalfPersentage = double.Parse(dr["Half"].ToString());
                    if (double.Parse(dr["Annual"].ToString()) != 0)
                        AnualPersentage = double.Parse(dr["Annual"].ToString());
                    
                }               
            }
            try
            {
                foreach (DataRow Dr in MarkDetails.Tables[0].Rows)
                {

                    temp++;
                    
                        if (Id == 0)
                        {
                            Id = int.Parse(Dr[0].ToString());
                        }

                        if (Id == int.Parse(Dr[0].ToString()))
                        {

                            StudentId = Dr["Id"].ToString();

                            if (Dr["ExamName"].ToString().StartsWith("UT"))
                            {
                                UTMarks = UTMarks + double.Parse(Dr["TotalMark"].ToString());
                                UTMax = UTMax + double.Parse(Dr["TotalMax"].ToString());
                            }
                            else if (Dr["ExamName"].ToString().StartsWith("Half"))
                            {
                                HalfMarks = HalfMarks + double.Parse(Dr["TotalMark"].ToString());
                                HalfMax = HalfMax + double.Parse(Dr["TotalMax"].ToString());
                            }
                            else if (Dr["ExamName"].ToString().StartsWith("Annual"))
                            {
                                AnualMarks = AnualMarks + double.Parse(Dr["TotalMark"].ToString());
                                AnualMax = AnualMax + double.Parse(Dr["TotalMax"].ToString());
                            }
                            else
                            {

                                OtherMark = OtherMark + double.Parse(Dr["TotalMark"].ToString());
                                OtherMax = OtherMax + double.Parse(Dr["TotalMax"].ToString());
                            }
                            StudentId = Dr["Id"].ToString();
                            StudentName = Dr["StudentName"].ToString();
                            RollNum = Dr["RollNo"].ToString();
                        }
                        else if (Id != int.Parse(Dr[0].ToString()))
                        {
                            if (UTPersentage != 100 && HalfPersentage != 100 && AnualPersentage != 100)
                            //if (UTPersentage == 100 && HalfPersentage == 100 && AnualPersentage == 100)
                            {
                                if (UTMarks > 0 && UTMax > 0)
                                {
                                    Utin100 = (UTMarks / UTMax) * 100;
                                    UtinPerMark = Utin100 * UTPersentage / 100;
                                    TotalMax = UTMax;
                                    TotalMarks = UTMarks;

                                }
                                else
                                {
                                    Utin100 = 0;
                                    UtinPerMark = 0;
                                }

                                if (HalfMarks > 0 && HalfMax > 0)
                                {
                                    Halfin100 = (HalfMarks / HalfMax) * 100;
                                    HalfinPerMark = Halfin100 * HalfPersentage / 100;
                                    TotalMax = HalfMax;
                                    TotalMarks = HalfMarks;
                                }
                                else
                                {
                                    Halfin100 = 0;
                                    HalfinPerMark = 0;
                                }
                                if (AnualMarks > 0 && AnualMax > 0)
                                {
                                    Anualin100 = (AnualMarks / AnualMax) * 100;
                                    AnualinPerMark = Anualin100 * AnualPersentage / 100;
                                    TotalMax = AnualMax;
                                    TotalMarks = AnualMarks;
                                }

                                else
                                {
                                    Anualin100 = 0;
                                    AnualinPerMark = 0;
                                }
                            }
                            else
                            {
                                TotalMarks = OtherMark;
                                TotalMax = OtherMax;
                            }
                            if (TotalMax != 0)
                            {
                                drow = MarkDataset.Tables["reports"].NewRow();

                                drow["Id"] = StudentId;
                                drow["StudentName"] = StudentName;
                                drow["Roll"] = RollNum;
                                drow["ObtainedMark"] = Math.Round(TotalMarks, 2).ToString();
                                drow["MaxMark"] = Math.Round(TotalMax, 2).ToString();


                                avg = (TotalMarks / TotalMax) * 100;
                                drow["Avg"] = avg.ToString("00.00");
                                Grade = GetGrade(GradeMasterSet, avg);
                                drow["Rank"] = "";
                                drow["Grade"] = Grade;
                                drow["Result"] = "";
                                drow["Remark"] = GetRemark(FeeDueStudent, Dr["Id"].ToString());


                                MarkDataset.Tables["reports"].Rows.Add(drow);

                                UTMarks = HalfMarks = AnualMarks = OtherMark = 0;
                                UTMax = HalfMax = AnualMax = OtherMax = 0;

                                Utin100 = Halfin100 = Anualin100 = 0;
                                UtinPerMark = HalfinPerMark = AnualinPerMark = 0;
                                TotalMarks = TotalMax = 0;
                            }


                            if (Dr["ExamName"].ToString().StartsWith("UT"))
                            {
                                UTMarks = UTMarks + double.Parse(Dr["TotalMark"].ToString());
                                UTMax = UTMax + double.Parse(Dr["TotalMax"].ToString());
                            }
                            else if (Dr["ExamName"].ToString().StartsWith("Half"))
                            {
                                HalfMarks = HalfMarks + double.Parse(Dr["TotalMark"].ToString());
                                HalfMax = HalfMax + double.Parse(Dr["TotalMax"].ToString());
                            }
                            else if (Dr["ExamName"].ToString().StartsWith("Annual"))
                            {
                                AnualMarks = AnualMarks + double.Parse(Dr["TotalMark"].ToString());
                                AnualMax = AnualMax + double.Parse(Dr["TotalMax"].ToString());
                            }

                            else
                            {

                                OtherMark = OtherMark + double.Parse(Dr["TotalMark"].ToString());
                                OtherMax = OtherMax + double.Parse(Dr["TotalMax"].ToString());
                            }

                            Id = int.Parse(Dr[0].ToString());

                            StudentId = Dr["Id"].ToString();
                            StudentName = Dr["StudentName"].ToString();
                            RollNum = Dr["RollNo"].ToString();


                        }
                        if (temp != MarkDetails.Tables[0].Rows.Count)
                        {
                            //if (UTPersentage != 100 && HalfPersentage != 100 && AnualPersentage != 100)
                            if (UTPersentage == 100 && HalfPersentage == 100 && AnualPersentage == 100)
                            {
                                if (UTMarks > 0 && UTMax > 0)
                                {
                                    Utin100 = (UTMarks / UTMax) * 100;
                                    UtinPerMark = Utin100 * UTPersentage / 100;
                                    TotalMax = UTMax;
                                    TotalMarks = UTMarks;

                                }
                                else
                                {
                                    Utin100 = 0;
                                    UtinPerMark = 0;
                                }

                                if (HalfMarks > 0 && HalfMax > 0)
                                {
                                    Halfin100 = (HalfMarks / HalfMax) * 100;
                                    HalfinPerMark = Halfin100 * HalfPersentage / 100;
                                    TotalMax = HalfMax;
                                    TotalMarks = HalfMarks;
                                }
                                else
                                {
                                    Halfin100 = 0;
                                    HalfinPerMark = 0;
                                }
                                if (AnualMarks > 0 && AnualMax > 0)
                                {
                                    Anualin100 = (AnualMarks / AnualMax) * 100;
                                    AnualinPerMark = Anualin100 * AnualPersentage / 100;
                                    TotalMax = AnualMax;
                                    TotalMarks = AnualMarks;
                                }

                                else
                                {
                                    Anualin100 = 0;
                                    AnualinPerMark = 0;
                                }

                                //TotalMarks = UtinPerMark + HalfinPerMark + AnualinPerMark;

                                //TotalMax = (100 * UTPersentage / 100) + (100 * HalfPersentage / 100) + (100 * AnualPersentage / 100);
                            }
                            else
                            {
                                TotalMarks = OtherMark;
                                TotalMax = OtherMax;
                            }

                            if (TotalMax != 0)
                            {
                                drow = MarkDataset.Tables["reports"].NewRow();

                                drow["Id"] = StudentId;
                                drow["StudentName"] = StudentName;
                                drow["Roll"] = RollNum;
                                drow["ObtainedMark"] = Math.Round(TotalMarks, 2).ToString();
                                //drow["ObtainedMark"] = Math.Round(TotalMarks, 2).ToString();
                                //drow["MaxMark"] = Math.Round(TotalMax, 2).ToString();
                                drow["MaxMark"] = Math.Round(TotalMax, 2).ToString();
                                avg = (TotalMarks / TotalMax) * 100;
                                drow["Avg"] = avg.ToString("00.00");
                                Grade = GetGrade(GradeMasterSet, avg);
                                drow["Rank"] = "";
                                drow["Grade"] = Grade;
                                drow["Result"] = "";
                                drow["Remark"] = GetRemark(FeeDueStudent, Dr["Id"].ToString());

                                MarkDataset.Tables["reports"].Rows.Add(drow);

                                UTMarks = HalfMarks = AnualMarks = OtherMark = 0;
                                UTMax = HalfMax = AnualMax = OtherMax = 0;
                                UTPersentage = HalfPersentage = AnualPersentage = 100;
                                Utin100 = Halfin100 = Anualin100 = 0;
                                UtinPerMark = HalfinPerMark = AnualinPerMark = 0;
                                TotalMarks = TotalMax = 0;
                            }


                        }
                    }
                
            }
            catch
            {

            }

            return MarkDataset;
        }

      

        private string GetRemark(DataSet WithHeldDataSet,string StudentId)
        {
              
            foreach (DataRow dr in WithHeldDataSet.Tables[0].Rows)
            {
                if (dr[0].ToString().Trim() == StudentId)
                {
                    return "WithHeld";
                }
            }
            return "";    
        }

        private string GetGrade(DataSet GradeMasterSet, double avg)
        {
            string grade = "";

            if (GradeMasterSet != null)
            {
                foreach (DataRow dr in GradeMasterSet.Tables[0].Rows)
                {
                    if (avg >= double.Parse(dr[1].ToString()))
                    {
                        return dr[0].ToString();
                    }
                }
            }

            return grade;
        }

        private DataSet CreateReportDataset()
        {
            DataSet reportDataset = new DataSet();
            DataTable dt;
            reportDataset.Tables.Add(new DataTable("reports"));
            dt = reportDataset.Tables["reports"];
            dt.Columns.Add("Id");
            dt.Columns.Add("StudentName");
            dt.Columns.Add("Roll");
            dt.Columns.Add("ObtainedMark");
            dt.Columns.Add("MaxMark");
            dt.Columns.Add("Avg");
            dt.Columns.Add("Grade");
            dt.Columns.Add("Result");
            dt.Columns.Add("Rank");
            dt.Columns.Add("Remark");

            return reportDataset;
        }

        private DataSet GetMarkDetails(string ExamScheduledId, string Class)
        {
            string sql = "select  tblstudent.id, tblstudent.StudentName, tblstudentclassmap.RollNo, tblstudentmark.TotalMark, tblstudentmark.TotalMax, tblexammaster.ExamName from tblstudentmark inner join tblstudent on tblstudent.id= tblstudentmark.StudId inner join tblstudentclassmap  on tblstudentclassmap.StudentId= tblstudent.id   inner join tblexamschedule on tblexamschedule.Id=tblstudentmark.ExamSchId inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId  inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId where tblstudentclassmap.ClassId=" + Class + " and tblstudentmark.ExamSchId in(" + ExamScheduledId + ")  order By RollNo";
            DataSet MarkDetails = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return MarkDetails;
        }

        private string GetExamScheduleId()
        {
            showExamGridColumns();
            string examScheduledId="";

            foreach (GridViewRow gr in Grd_Exam.Rows)
            {
                if (examScheduledId != "")
                {
                    examScheduledId = examScheduledId + ",";
                }
                examScheduledId = examScheduledId + gr.Cells[2].Text.ToString();
            }
            hideExamGridColumns();

            return examScheduledId;
        }

        private void InitializePage()
        {
            ViewState["CombainedExamClassDetails"] = null;
            showExamGridColumns();
            Grd_Exam.DataSource = null;
            Grd_Exam.DataBind();
            hideExamGridColumns();

            showReportGridColumns();
            Grd_Result.DataSource = null;
            Grd_Result.DataBind();
            hideReportGridColumns();
            loadClassDetails();
            Pnl_ReportArea.Visible = false;
        }

        private void loadExamsForTheSelectedClass()
        {
            showReportGridColumns();

            Grd_Result.DataSource = null;
            Grd_Result.DataBind();
            Pnl_ReportArea.Visible = false;
            hideReportGridColumns();

            if (Drp_Calss.SelectedValue != "0")
            {
                string sql = "select tblexammaster.Id, tblexammaster.ExamName,tblexamschedule.Id as ExamSchId  from tblexamschedule inner join tblclassexam on tblclassexam.Id=  tblexamschedule.ClassExamId inner join tblexammaster on tblexammaster.Id= tblclassexam.ExamId  where tblclassexam.ClassId= " + Drp_Calss.SelectedValue.ToString() + " and  tblexamschedule.Status='Completed' order by tblexammaster.ExamOrder";
                OdbcDataReader ClassReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

                if (ClassReader.HasRows)
                {
                    showExamGridColumns();
                    Grd_Exam.DataSource = ClassReader;
                    Grd_Exam.DataBind();
                    hideExamGridColumns();
                }
                else
                {
                    showExamGridColumns();
                    Grd_Exam.DataSource = null;
                    Grd_Exam.DataBind();
                    hideExamGridColumns();
                    Lbl_Err.Text = "No Exams Found";
                   
                   
                }
            }
            else
            {
                showExamGridColumns();
                Grd_Exam.DataSource = null;
                Grd_Exam.DataBind();
                hideExamGridColumns();
                Lbl_Err.Text = "Select Class";
            }
        }

        private void loadClassDetails()
        {
            Drp_Calss.Items.Clear();
            OdbcDataReader MyReader = MyExamMang.getclass();
            if (MyReader.HasRows)
            {
                ListItem li1 = new ListItem("Select Class", "0");
                Drp_Calss.Items.Add(li1);
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Calss.Items.Add(li);
                }
            }
            else
            {
                ListItem li1 = new ListItem("No Class", "0");
                Drp_Calss.Items.Add(li1);
            }
            MyReader.Close();
        }

        private void hideReportGridColumns()
        {
            Grd_Result.Columns[0].Visible = false;
        }

        private void showReportGridColumns()
        {
            Grd_Result.Columns[0].Visible = true;
        }

        private void hideExamGridColumns()
        {
            Grd_Exam.Columns[0].Visible = false;
            Grd_Exam.Columns[2].Visible = false;
        }

        private void showExamGridColumns()
        {
            Grd_Exam.Columns[0].Visible = true;
             Grd_Exam.Columns[2].Visible = true;
            
        }

        #endregion

       
    }
}
