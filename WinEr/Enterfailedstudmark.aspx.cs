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
using System.Data.OleDb;
using System.IO;

public partial class Enterfailedstudmark : System.Web.UI.Page
{
    private ExamManage MyExamMang;
    private KnowinUser MyUser;
    private ClassOrganiser MyClassMang;
    private OdbcDataReader MyReader = null;
    private DataSet MydataSet;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        if (Session["ExamId"] == null)
        {
            Response.Redirect("ViewExams.aspx");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyExamMang = MyUser.GetExamObj();
        MyClassMang = MyUser.GetClassObj();
        if (MyExamMang == null)
        {
            Response.Redirect("RoleErr.htm");
        }
        else if (MyClassMang == null)
        {
            Response.Redirect("RoleErr.htm");
        }
        else if (!MyUser.HaveActionRignt(7))
        {
            Response.Redirect("RoleErr.htm");
        }
        else
        {
            if (!IsPostBack)
            {
                string _ExamMenuStr;
                _ExamMenuStr = MyExamMang.GetSubExamMangMenuString(MyUser.UserRoleId);
                this.SubExammngMenu.InnerHtml = _ExamMenuStr;
                LoadExamGeneralDetails();
                LoadClassAndPerdiod();
                SelectClassAndPeriod();
                LoadSubjectInfo();
                //SetPanels();               
                //some initlization
            }
        }
    }

    protected void Grd_Entermarks_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string value = e.Row.Cells[2].Text;
            if (value == "-1")
            {
                e.Row.Cells[2].Text = "No Roll No.";
            }
        }
    }

    private void SelectClassAndPeriod()
    {
        if (Session["ClassId"] != null)
        {
            DrpClassName.SelectedValue = Session["ClassId"].ToString();
        }
        if (Session["PeriodId"] != null)
        {
            DrpExamPeriod.SelectedValue = Session["PeriodId"].ToString();
        }
    }

    private void LoadSubjectInfo()
    {

        lnk_manageExam.Visible = false;
        lnk_ScheduleExam.Visible = false;
        int ifAssignToClass = MyExamMang.IfExamAssignedToAnyClass(int.Parse(Session["ExamId"].ToString()));
        if (ifAssignToClass == 0)
        {
            Lbl_message.Text = "This exam is not mapped to any Class..!";
            if (MyUser.HaveActionRignt(76))
            {
                lnk_manageExam.Visible = true;
            }
            else
            {
                lnk_manageExam.Visible = false;
            }
            DisableControls();
        }
        else if (DrpClassName.SelectedValue == "-1")
        {
            Lbl_message.Text = "No Class Selected..";
            DisableControls();
        }

        else if (!MyExamMang.ExamSchduled(int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue))
        {
            Lbl_message.Text = "Exam is not Scheduled for the select class and period..";
            if (MyUser.HaveActionRignt(6))
            {
                lnk_ScheduleExam.Visible = true;
            }
            else
            {
                lnk_ScheduleExam.Visible = false;
            }
            DisableControls();
        }
        else if (AddSubjectsToDrpList())
        {
            EnableControls();
            LoadSubjectDetails();
            LoadStudToGrid();
            LoadMarks();
        }
        else
        {
            Lbl_message.Text = "You don't have rignts to enter mark for any subject of this exam";
            DisableControls();
        }
    }

    private void EnableControls()
    {
        Btn_Update.Enabled = true;
        Drp_Subject.Enabled = true;
        Lbl_message.Text = "";
        Pnl_studentlist.Visible = true;
        Btn_Undo.Enabled = true;
        PanelError.Visible = false;
    }

    private void DisableControls()
    {
        Pnl_studentlist.Visible = false;
        Btn_Update.Enabled = false;
        Drp_Subject.Enabled = false;
        Btn_Undo.Enabled = false;
        Txt_Maxmark.Text = "";
        PanelError.Visible = true;
    }

    private void LoadClassAndPerdiod()
    {
        LoadClass();
        LoadPeriod();
    }

    private void LoadPeriod()
    {
        DrpExamPeriod.Items.Clear();
        string sql = "SELECT tblperiod.Id, tblperiod.Period FROM tblexammaster inner join tblperiod  on tblexammaster.PeriodTypeId = tblperiod.FrequencyId WHERE tblexammaster.Id=" + int.Parse(Session["ExamId"].ToString());
        MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                DrpExamPeriod.Items.Add(li);
            }
        }
    }

    private void LoadClass()
    {
        DrpClassName.Items.Clear();
        MydataSet = MyExamMang.MyAssociatedExamClass(MyUser.UserId, int.Parse(Session["ExamId"].ToString()));
        if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in MydataSet.Tables[0].Rows)
            {
                ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                DrpClassName.Items.Add(li);
            }
        }
        else
        {
            ListItem li = new ListItem("No Class present", "-1");
            DrpClassName.Items.Add(li);
        }
        DrpClassName.SelectedIndex = 0;
    }

    private void LoadExamGeneralDetails()
    {
        string ExamNme, ExamType, examfreqncy;
        MyExamMang.GetExamDetail(int.Parse(Session["ExamId"].ToString()), out ExamNme, out ExamType, out examfreqncy);
        Lbl_ExamName.Text = ExamNme.ToString();
        Lbl_ExamType.Text = ExamType.ToString();
        Lbl_Frequency.Text = examfreqncy.ToString();
    }

    private bool AddSubjectsToDrpList()
    {
        bool _valid;
        Drp_Subject.Items.Clear();
        string sql;
        if ((!MyUser.HaveActionRignt(41)))
        {
            sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " AND tblclassexam.ExamId=" + int.Parse(Session["ExamId"].ToString()) + " AND tblclassexam.ClassId=" + DrpClassName.SelectedValue + " AND tblsubjects.Id IN ( SELECT tblstaffsubjectmap.SubjectId from tblstaffsubjectmap where tblstaffsubjectmap.StaffId=" + MyUser.UserId + " ) order by tblexammark.SubjectOrder";
        }
        else
        {
            sql = "SELECT distinct tblsubjects.Id,tblsubjects.subject_name from tblsubjects inner JOIN tblexammark on tblexammark.SubjectId=tblsubjects.Id inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " AND tblclassexam.ExamId=" + int.Parse(Session["ExamId"].ToString()) + " AND tblclassexam.ClassId=" + DrpClassName.SelectedValue + " order by tblexammark.SubjectOrder";
        }
        MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                Drp_Subject.Items.Add(li);
            }
            _valid = true;
        }
        else
        {
            ListItem li = new ListItem("No Subjects", "-1");
            Drp_Subject.Items.Add(li);
            _valid = false;
        }
        return _valid;
    }

    private void LoadSubjectDetails()
    {
        //Txt_Batch.Text = MyUser.CurrentBatchName;
        string sql = "SELECT tblclassexamsubmap.MaxMark from tblclassexam inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblclassexam.Id where tblclassexamsubmap.SubId=" + int.Parse(Drp_Subject.SelectedValue.ToString()) + " AND tblclassexam.ClassId=" + DrpClassName.SelectedValue + " AND tblclassexam.ExamId=" + int.Parse(Session["ExamId"].ToString());
        MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            MyReader.Read();
            Txt_Maxmark.Text = MyReader.GetValue(0).ToString();
        }
        sql = "SELECT tblexammark.MarkColumn,FailedMarkColumn FROM tblexammark inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId WHERE tblclassexam.ExamId=" + int.Parse(Session["ExamId"].ToString()) + " AND tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "  AND tblexamschedule.PeriodId=" + DrpExamPeriod.SelectedValue + " AND tblexammark.SubjectId=" + int.Parse(Drp_Subject.SelectedValue.ToString()) + " AND tblclassexam.ClassId=" + DrpClassName.SelectedValue + " order by tblexammark.SubjectOrder";
        MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            MyReader.Read();
            Txt_markCol.Text = MyReader.GetValue(0).ToString();
            Txt_failmarkCol.Text = MyReader.GetValue(1).ToString();
            // Txt_Type.Text = MyExamMang.ExamType(int.Parse(Session["ExamId"].ToString()));
        }
    }

    private void LoadMarks()
    {
        int _grandTotal = 0;
        foreach (GridViewRow gv in Grd_Entermarks.Rows)
        {
            TextBox TxtMark = (TextBox)gv.FindControl("Txt_Mark");
            string _Str_Mark = MyExamMang.GetMarks(int.Parse(gv.Cells[0].Text.ToString()), int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, Txt_failmarkCol.Text.ToString(), DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue);
            if (_Str_Mark == "-1")
            {
                TxtMark.Text = "a";
            }
            else
            {
                int _mark = 0;
                if (int.TryParse(_Str_Mark, out _mark))
                {
                    _grandTotal = _grandTotal + _mark;
                }

                TxtMark.Text = _Str_Mark;
            }

        }
        Txt_GrandTotal.Text = _grandTotal.ToString();

    }

    private void LoadStudToGrid()
    {
        int i_ExamId = int.Parse(Session["ExamId"].ToString());
        int Periodid = int.Parse(DrpExamPeriod.SelectedValue);
        int ScheduleId = GetExamSchid(int.Parse(DrpClassName.SelectedValue), i_ExamId, MyUser.CurrentBatchId, Periodid);


        string sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudentclassmap.RollNo,tblstudentmark.Failsubcount from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id INNER JOIN tblstudentmark on tblstudentmark.StudId=tblstudent.Id WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND tblstudentclassmap.ClassId=" + DrpClassName.SelectedValue + " AND tblstudentmark.FailSubCount>2 Order by tblstudentclassmap.RollNo ASC";
        MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
            Grd_Entermarks.Columns[0].Visible = true;
            Grd_Entermarks.DataSource = MydataSet;
            Grd_Entermarks.DataBind();
            Grd_Entermarks.Columns[0].Visible = false;
        }
        else
        {
            Lbl_message.Text = "No Students found in the Class ";
            DisableControls();
        }
    }

    private bool valideData(out string _Message)
    {
        bool _valid = true;
        _Message = "";
        try
        {
            foreach (GridViewRow gv in Grd_Entermarks.Rows)
            {
                TextBox TxtMark = (TextBox)gv.FindControl("Txt_Mark");
                if (TxtMark.Text.Trim() == "")
                {
                    _Message = "Mark cannot be empty";
                    _valid = false;
                    break;
                }
                else if (TxtMark.Text != "a")
                {
                    if (Double.Parse(TxtMark.Text.ToString()) > Double.Parse(Txt_Maxmark.Text.ToString()))
                    {
                        _Message = "Mark cannot be greater than Maximum mark";
                        _valid = false;
                        break;
                    }
                }

                //TxtMark.Text = MyExamMang.GetMarks(int.Parse(gv.Cells[0].Text.ToString()), int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, Txt_markCol.Text.ToString());
                //MyClassMang.UpdateRollNo(int.Parse(Session["ClassId"].ToString()), MyUser.CurrentBatchId, int.Parse(TxtRollNumber.Text.ToString()), int.Parse(gv.Cells[0].Text.ToString()));
            }
        }
        catch (Exception e)
        {
            _Message = e.Message;
            _valid = false;
        }
        return _valid;
    }

    private void UpdateMark()
    {
        foreach (GridViewRow gv in Grd_Entermarks.Rows)
        {
            TextBox TxtMark = (TextBox)gv.FindControl("Txt_Mark");
            if (TxtMark.Text == "a")
            {
                MyExamMang.FillMarksFail(DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue, MyUser.CurrentBatchId, int.Parse(gv.Cells[0].Text.ToString()), int.Parse(Session["ExamId"].ToString()), Txt_failmarkCol.Text, -1);
            }
            else
            {
               
                MyExamMang.FillMarksFail(DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue, MyUser.CurrentBatchId, int.Parse(gv.Cells[0].Text.ToString()), int.Parse(Session["ExamId"].ToString()), Txt_failmarkCol.Text, Double.Parse(TxtMark.Text.ToString()));
            }
        }
    }

    protected void Btn_Update_Click1(object sender, EventArgs e)
    {
        string _message;
        if (valideData(out _message))
        {
            UpdateMark();
            MyExamMang.UpdateExamSchedule(DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue, int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, "Updated");

            _message = "Mark Entered";
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Mark Entry", "Entered Marks for the Exam " + Lbl_ExamName.Text.ToString(), 1);
        }
        WC_MessageBox.ShowMssage(_message);
        LoadMarks();

    }

    protected void Btn_Undo_Click(object sender, EventArgs e)
    {
        LoadMarks();
    }

    protected void Drp_Subject_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSubjectDetails();
        LoadStudToGrid();
        LoadMarks();
    }

    protected void DrpClassName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["ClassId"] = DrpClassName.SelectedValue;
        LoadSubjectInfo();
    }

    protected void DrpExamPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["PeriodId"] = DrpExamPeriod.SelectedValue;
        LoadSubjectInfo();
    }

    protected void Btn_exporttoexel_Click(object sender, ImageClickEventArgs e)
    {
        int i_ExamId = int.Parse(Session["ExamId"].ToString());
        int Periodid = int.Parse(DrpExamPeriod.SelectedValue);
        DataSet MyExamData = GetExamData(Periodid, i_ExamId, MyUser.CurrentBatchId);

        if (MyExamData.Tables[0].Rows.Count > 0)
        {
            //if (!WinEr.ExcelUtility.ExportDataSetToExcel(MyExamData, "ExamResult.xls"))
            //{
            //    Lbl_message.Text = "This function need Ms office";
            //}

            string FileName = "ExamResult";
            string _ReportName = "Exam Result";
            if (!WinEr.ExcelUtility.ExportDataToExcel(MyExamData, _ReportName, FileName, MyUser.ExcelHeader))
            {

                Lbl_message.Text = "This function need Ms office";
            }
        }
    }



    protected void Btn_GradeExportToExcel_Click(object sender, ImageClickEventArgs e)
    {
        int i_ExamId = int.Parse(Session["ExamId"].ToString());
        int Periodid = int.Parse(DrpExamPeriod.SelectedValue);
        DataSet MyExamData = GetExamGradeData(Periodid, i_ExamId, MyUser.CurrentBatchId);
        if (MyExamData.Tables[0].Rows.Count > 0)
        {
            //if (!WinEr.ExcelUtility.ExportDataSetToExcel(MyExamData, "ExamResult.xls"))
            //{
            //    Lbl_message.Text = "This function need Ms office";
            //}

            string FileName = "GradeResult";
            string _ReportName = "Exam Result";
            if (!WinEr.ExcelUtility.ExportDataToExcel(MyExamData, _ReportName, FileName, MyUser.ExcelHeader))
            {

                Lbl_message.Text = "This function need Ms office";
            }
        }


    }

    private DataSet GetExamData(int _Periodid, int i_ExamId, int _BatchId)
    {
        DataSet ExamDataSet = new DataSet();
        DataTable dt;
        DataRow dr;
        int i, j;

        try
        {
            ExamDataSet.Tables.Add(new DataTable("Exam"));
            dt = ExamDataSet.Tables["Exam"];

            DataSet Subjects = MyClassMang.GetSubjectList(int.Parse(DrpClassName.SelectedValue), i_ExamId, _BatchId);
            string[] name = new string[100];
            if (Subjects != null && Subjects.Tables != null && Subjects.Tables[0].Rows.Count > 0)
            {
                dt.Columns.Add("RollNo");
                dt.Columns.Add("StudentName");
                name[0] = "StudentName";
                i = 1;

                foreach (DataRow subject in Subjects.Tables[0].Rows)
                {
                    dt.Columns.Add(subject[1].ToString());
                    name[i] = subject[1].ToString();
                    i++;
                }
                dt.Columns.Add("ObtainedMark");
                dt.Columns.Add("TotalMark");
                dt.Columns.Add("Avg");
                dt.Columns.Add("Grade");
                dt.Columns.Add("Result");
                if (MyExamMang.NeedRank)
                {
                    dt.Columns.Add("Rank");
                }

                dr = ExamDataSet.Tables["Exam"].NewRow();
                dr["StudentName"] = " ";

                j = 0;
                foreach (DataRow subject in Subjects.Tables[0].Rows)
                {
                    j++;
                    dr[name[j]] = " ";
                }
                dr["TotalMark"] = " ";
                dr["ObtainedMark"] = " ";
                dr["Avg"] = " ";
                dr["Grade"] = " ";
                dr["Result"] = " ";

                if (MyExamMang.NeedRank)
                {
                    dr["Rank"] = " ";
                }

                ExamDataSet.Tables["Exam"].Rows.Add(dr);


                DataSet Students = GetStudentMarkDetails(int.Parse(DrpClassName.SelectedValue), i_ExamId, _BatchId, _Periodid);
                if (Students != null && Students.Tables != null && Students.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr_Student in Students.Tables[0].Rows)
                    {
                        dr = ExamDataSet.Tables["Exam"].NewRow();
                        j = 0;
                        dr[name[j]] = dr_Student[0].ToString();
                        foreach (DataRow subject in Subjects.Tables[0].Rows)
                        {
                            j++;
                            if (dr_Student[j].ToString() == "-1")
                            {
                                dr[name[j]] = "A";
                            }
                            else
                            {
                                dr[name[j]] = dr_Student[j].ToString();

                            }
                        }
                        dr["TotalMark"] = dr_Student[j + 1].ToString();
                        dr["ObtainedMark"] = dr_Student[j + 2].ToString();
                        dr["Avg"] = dr_Student[j + 3].ToString();
                        dr["Grade"] = dr_Student[j + 4].ToString();
                        dr["Result"] = dr_Student[j + 5].ToString();
                        if (MyExamMang.NeedRank)
                        {
                            dr["Rank"] = dr_Student[j + 6].ToString();
                        }
                        dr["RollNo"] = dr_Student[j + 7].ToString();
                        ExamDataSet.Tables["Exam"].Rows.Add(dr);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string ss = ex.Message;
        }
        return ExamDataSet;
    }


    private DataSet GetExamGradeData(int _Periodid, int i_ExamId, int _BatchId)
    {
        DataSet ExamDataSet = new DataSet();
        DataSet GradeDS = new DataSet();
        DataTable dt;
        DataRow dr;
        int i, j;

        try
        {
            ExamDataSet.Tables.Add(new DataTable("Exam"));
            dt = ExamDataSet.Tables["Exam"];

            DataSet Subjects = MyClassMang.GetScheduledSubjectList(int.Parse(DrpClassName.SelectedValue), i_ExamId, _BatchId);
            string[] name = new string[100];
            if (Subjects != null && Subjects.Tables != null && Subjects.Tables[0].Rows.Count > 0)
            {
                dt.Columns.Add("RollNo");
                dt.Columns.Add("StudentName");
                name[0] = "StudentName";
                i = 1;

                foreach (DataRow subject in Subjects.Tables[0].Rows)
                {
                    dt.Columns.Add(subject[1].ToString());
                    name[i] = subject[1].ToString();
                    i++;
                }
                dt.Columns.Add("ObtainedMark");
                dt.Columns.Add("TotalMark");
                dt.Columns.Add("Avg");
                dt.Columns.Add("Grade");
                dt.Columns.Add("Result");
                if (MyExamMang.NeedRank)
                {
                    dt.Columns.Add("Rank");
                }


                dr = ExamDataSet.Tables["Exam"].NewRow();
                dr["StudentName"] = " ";

                j = 0;
                foreach (DataRow subject in Subjects.Tables[0].Rows)
                {
                    j++;
                    dr[name[j]] = " ";
                }
                dr["TotalMark"] = " ";
                dr["ObtainedMark"] = " ";
                dr["Avg"] = " ";
                dr["Grade"] = " ";
                dr["Result"] = " ";

                if (MyExamMang.NeedRank)
                {
                    dr["Rank"] = " ";
                }

                ExamDataSet.Tables["Exam"].Rows.Add(dr);


                DataSet Students = GetStudentMarkDetails(int.Parse(DrpClassName.SelectedValue), i_ExamId, _BatchId, _Periodid);
                if (Students != null && Students.Tables != null && Students.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr_Student in Students.Tables[0].Rows)
                    {
                        dr = ExamDataSet.Tables["Exam"].NewRow();
                        j = 0;
                        dr[name[j]] = dr_Student[0].ToString();
                        foreach (DataRow subject in Subjects.Tables[0].Rows)
                        {
                            j++;
                            if (dr_Student[j].ToString() == "-1")
                            {
                                dr[name[j]] = "-";
                            }
                            else
                            {
                                double percentage = 0;

                                GradeDS = GetActiveGrade(int.Parse(DrpClassName.SelectedValue), i_ExamId, _BatchId, _Periodid);
                                double MaxMark = MyClassMang.GetMaximumMarkOfSubject(int.Parse(DrpClassName.SelectedValue), i_ExamId, _BatchId, int.Parse(subject["Id"].ToString()));
                                if (MaxMark != 0)
                                {
                                    double mark = 0;
                                    double.TryParse(dr_Student[j].ToString(), out mark);
                                    percentage = (mark * 100) / MaxMark;
                                }
                                if (GradeDS != null && GradeDS.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow grd in GradeDS.Tables[0].Rows)
                                    {
                                        //tblgrade.LowerLimit, tblgrade.UpperLimit
                                        int lowerlimit = int.Parse(grd["LowerLimit"].ToString());
                                        if (percentage >= lowerlimit)
                                        {
                                            dr[name[j]] = grd["Grade"].ToString();
                                            break;
                                        }

                                    }
                                }

                            }
                        }
                        dr["TotalMark"] = dr_Student[j + 1].ToString();
                        dr["ObtainedMark"] = dr_Student[j + 2].ToString();
                        dr["Avg"] = dr_Student[j + 3].ToString();
                        dr["Grade"] = dr_Student[j + 4].ToString();
                        dr["Result"] = dr_Student[j + 5].ToString();
                        if (MyExamMang.NeedRank)
                        {
                            dr["Rank"] = dr_Student[j + 6].ToString();
                        }
                        dr["RollNo"] = dr_Student[j + 7].ToString();
                        ExamDataSet.Tables["Exam"].Rows.Add(dr);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string ss = ex.Message;
        }
        return ExamDataSet;
    }

    private DataSet GetActiveGrade(int ClassId, int ExamId, int BatchId, int PeriodId)
    {
        string sql = "";
        DataSet GradeDs = new DataSet();
        sql = "select tblgrade.Grade, tblgrade.LowerLimit, tblgrade.UpperLimit from tblgrade INNER JOIN tblexamschedule ON tblgrade.GradeMasterId=tblexamschedule.GradeMasterId inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId where tblgrade.`Status`=1 AND tblclassexam.ClassId =" + ClassId + " and tblclassexam.ExamId = " + ExamId + " AND  tblexamschedule.BatchId=" + BatchId + " AND tblexamschedule.PeriodId=" + PeriodId + " ORDER BY tblgrade.LowerLimit DESC";
        GradeDs = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return GradeDs;
    }



    private DataSet GetStudentMarkDetails(int _ClassID, int i_ExamId, int _BatchId, int _Periodid)
    {
        DataSet Students = null;
        string Query = "";
        int i = 0;
        int ScheduleId = GetExamSchid(_ClassID, i_ExamId, _BatchId, _Periodid);
        string sql = "select tblexammark.MarkColumn from tblexammark where tblexammark.ExamSchId=" + ScheduleId + " order by tblexammark.SubjectOrder";
        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                i++;
                if (i > 1)
                {
                    Query = Query + ",";
                }
                Query = Query + MyReader.GetValue(0).ToString();
            }
        }
        sql = "select DISTINCT tblstudent.StudentName, " + Query + ", tblstudentmark.TotalMax , tblstudentmark.TotalMark , ROUND(tblstudentmark.`Avg`, 2) AS `Avg`, Grade, Result, Rank,tblstudentclassmap.RollNo  from tblstudentclassmap inner join tblstudent on tblstudentclassmap.StudentId = tblstudent.Id and tblstudentclassmap.BatchId=" + _BatchId + " and tblstudentclassmap.ClassId = " + _ClassID + " inner join tblstudentmark on tblstudentmark.StudId = tblstudentclassmap.StudentId where tblstudentmark.ExamSchId in (select tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblclassexam.Id =  tblexamschedule.ClassExamId  where tblexamschedule.PeriodId =" + _Periodid + " and tblexamschedule.BatchId = " + _BatchId + " and tblclassexam.ExamId=" + i_ExamId + " and tblclassexam.ClassId =" + _ClassID + ") and tblstudent.`Status`=1 order by tblstudent.StudentName asc";
        Students = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        return Students;
    }

    private int GetExamSchid(int _ClassID, int i_ExamId, int _BatchId, int _Period)
    {
        int SchedId = -2;
        string sql = "select tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblclassexam.Id = tblexamschedule.ClassExamId and tblclassexam.ClassId =" + _ClassID + " and tblclassexam.ExamId = " + i_ExamId + "  where tblexamschedule.BatchId =" + _BatchId + " and tblexamschedule.PeriodId =" + _Period + "";
        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            SchedId = int.Parse(MyReader.GetValue(0).ToString());
        }
        return SchedId;
    }

    protected void lnk_manageExam_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageClassExam.aspx");
    }

    protected void lnk_Schedule_Click(object sender, EventArgs e)
    {
        Response.Redirect("ScheduleExam.aspx");
    }






}