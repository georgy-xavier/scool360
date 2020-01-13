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

public partial class ExamReport : System.Web.UI.Page
{
    private ExamManage MyExamMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    private OdbcDataReader MyReader1 = null;
    private DataSet MydataSet;
    private StudentManagerClass MyStudMang;
    //String Error;
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
        MyStudMang = MyUser.GetStudentObj();
      
       
        if (MyExamMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }
        else if (MyStudMang == null)
        {
            Response.Redirect("RoleErr.htm");
        }
        else if (!MyUser.HaveActionRignt(28))
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
                SetPanels();
                //some initlization
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

    protected void Grd_CreateReport_RowDataBound(object sender, GridViewRowEventArgs e)
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

    private void EnableGenerateControls()
    {
        Lbl_message.Text = "Click Generate button to generate the exam report..";
        Btn_Gen.Enabled = true;
        //Btn_Gen.CssClass = "graysave";
        Btn_Update.Enabled = false;
        Btn_Update.CssClass = "";
        PanelError.Visible = true;
        Pnl_report.Visible = false;
    }

    private void EnableUpdateControls()
    {
        Lbl_message.Text = "";
        Pnl_report.Visible = true;
        Btn_Gen.Enabled = false;
        Btn_Gen.CssClass = "";
        Btn_Update.Enabled = true;
        //Btn_Update.CssClass = "graysave";
        PanelError.Visible = false;
    }

    private void DisableControls()
    {
        Pnl_report.Visible = false;
        PanelError.Visible = true;
        Btn_Gen.Enabled = false;
        Btn_Gen.CssClass = "";
        Btn_Update.Enabled = false;
        Btn_Update.CssClass = "";
        Grd_CreateReport.DataSource = null;
        Grd_CreateReport.DataBind();
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

    private void SetPanels()
    {

        lnk_manageExam.Visible = false;
        lnk_ScheduleExam.Visible = false;
        lnk_Marks.Visible = false;
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
        else
        {
            string _Subjects = "";
            if (LoadStudDetails())
            {
                if (CheckValidMarks(out _Subjects))
                {
                    LoadResult();
                }
                else
                {
                    //Lbl_message.Text = "All marks are not entered.... ";
                    Lbl_message.Text = "All marks are not entered for subjects: " + _Subjects.Trim().TrimEnd(',');
                    if (MyUser.HaveActionRignt(7))
                    {
                        lnk_Marks.Visible = true;
                    }
                    else
                    {
                        lnk_Marks.Visible = false;
                    }
                    DisableControls();
                }
            }
            else
            {
                Lbl_message.Text = "No students present in the class..";
                DisableControls();
            }
        }
    }
   
    protected void Btn_Update_Click(object sender, EventArgs e)
    {
        int _scheduledid = MyExamMang.GetExamScheduleId(int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue);
        foreach (GridViewRow gv in Grd_CreateReport.Rows)
        {
            TextBox TxtRemark = (TextBox)gv.FindControl("Txt_Remark");
            MyExamMang.UpdateRemark(TxtRemark.Text.ToString(), _scheduledid, int.Parse(gv.Cells[0].Text.ToString()));            
        }
        
        WC_MessageBox.ShowMssage("Remarks updated");
        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Exam Report", "Exam Report Remarks Saved for " + Lbl_ExamName.Text.ToString() + " for " + DrpClassName.SelectedItem.Text, 1);

    }    

    private void LoadResult()
    {
        double _tempAvg;
        string sql = "SELECT tblexamschedule.Id FROM tblexamschedule inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId where tblclassexam.ExamId=" + Session["ExamId"].ToString() + " AND  tblclassexam.ClassId=" + DrpClassName.SelectedValue + " and tblexamschedule.`Status`='Updated' AND tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " AND tblexamschedule.PeriodId=" + DrpExamPeriod.SelectedValue;
        MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            EnableGenerateControls();            
        }
        else
        {
            EnableUpdateControls();
            foreach (GridViewRow gv in Grd_CreateReport.Rows)
            {
                Label LblTotal = (Label)gv.FindControl("Lbl_Total");
                Label LblMax = (Label)gv.FindControl("Lbl_Max");
                Label LblAvg = (Label)gv.FindControl("Lbl_Avg");
                Label LblGrade = (Label)gv.FindControl("Lbl_Grade");
                Label LblResult = (Label)gv.FindControl("Lbl_Result");
                Label LblRank = (Label)gv.FindControl("Lbl_Rank");
                TextBox TxtRemark = (TextBox)gv.FindControl("Txt_Remark");
                sql = "select tblstudentmark.TotalMark,tblstudentmark.TotalMax,tblstudentmark.Avg,tblstudentmark.Grade,tblstudentmark.Result,tblstudentmark.Rank,tblstudentmark.Remark from tblstudentmark inner join tblexamschedule on tblexamschedule.Id=tblstudentmark.ExamSchId inner join tblstudentclassmap on tblstudentclassmap.StudentId =tblstudentmark.studid inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId where tblclassexam.ExamId=" + Session["ExamId"].ToString() + " AND  tblclassexam.ClassId=" + DrpClassName.SelectedValue + " AND tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " AND tblexamschedule.PeriodId=" + DrpExamPeriod.SelectedValue + " AND tblstudentclassmap.StudentId=" + gv.Cells[0].Text.ToString();
                MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    MyReader.Read();
                    LblTotal.Text = MyReader.GetValue(0).ToString();
                    LblMax.Text = MyReader.GetValue(1).ToString();
                    if (double.TryParse(MyReader.GetValue(2).ToString(), out _tempAvg))
                    {
                        LblAvg.Text = _tempAvg.ToString("#0.00");
                    }
                    LblGrade.Text = MyReader.GetValue(3).ToString();
                    LblResult.Text = MyReader.GetValue(4).ToString();
                    LblRank.Text = MyReader.GetValue(5).ToString();
                    TxtRemark.Text = MyReader.GetValue(6).ToString();
                    if (LblTotal.Text.Trim() == "" || LblMax.Text.Trim() == "" || LblAvg.Text.Trim() == "" || LblGrade.Text.Trim() == ""  || LblRank.Text.Trim() == "")
                    {
                        DisableControls();
                        EnableGenerateControls();
                        break;
                    }
                }

                else
                {
                    LblTotal.Text = "";
                    LblMax.Text = "";
                    LblAvg.Text = "";
                    LblGrade.Text = "";
                    LblResult.Text = "";
                    LblRank.Text = "";
                }
            }
        }
    }

    private bool LoadStudDetails()
    {
        bool _valid=false;
        string sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudentclassmap.RollNo from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND tblstudentclassmap.ClassId=" + DrpClassName.SelectedValue + " Order by tblstudentclassmap.RollNo ASC";
        MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
            Grd_CreateReport.Columns[0].Visible =true;
            Grd_CreateReport.Columns[8].Visible = true;
            Grd_CreateReport.DataSource = MydataSet;
            Grd_CreateReport.DataBind();
            if (!MyExamMang.NeedRank)
            {
                Grd_CreateReport.Columns[8].Visible = false;
            }
            Grd_CreateReport.Columns[0].Visible = false;
            _valid = true;
        }
        return _valid;
    }


    private bool CheckValidMarks(out string _Subjects)
    {
         _Subjects = "";
        bool _valid = true;
        foreach (GridViewRow gv in Grd_CreateReport.Rows)
        {
            string sql = " SELECT tblexammark.MarkColumn,tblsubjects.subject_name FROM tblexammark inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblsubjects on tblsubjects.Id=tblexammark.SubjectId inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId where tblclassexam.ExamId=" + Session["ExamId"].ToString() + " AND tblclassexam.ClassId=" + DrpClassName.SelectedValue + " AND tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " AND tblexamschedule.PeriodId=" + DrpExamPeriod.SelectedValue + " order by tblexammark.SubjectOrder";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                string _Str_Mark;
                while (MyReader.Read())
                {
                    _Str_Mark = MyExamMang.GetMarks(int.Parse(gv.Cells[0].Text.ToString()), int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, MyReader.GetValue(0).ToString(), DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue);
                     if (_Str_Mark == "")
                    {
                        _Subjects = _Subjects + MyReader.GetValue(1).ToString() + ", ";
                        _valid = false;
                    }
                    
                }
            }
            if (_valid == false)
            {
                break;
            }
        }
        int[] iffff = new int[5] { 1, 2, 3, 4, 5 };
        iffff.Contains(1);
        return _valid;
    }

    private void GenerateAllStudReport()
    {
        string sql;
        int FailSubCount = 0;
        string _FailResult_ResultSubvalue="";
        string _NeedDiff_FailResult_Result = MyExamMang.GetConfigValue("Remarks", "Exam Report", out _FailResult_ResultSubvalue);

        string _FeeDueResult_WithheldSubvalue = "";
        string Need_FeeDueResult_Withheld = MyExamMang.GetConfigValue("FeeDueResult_Withheld", "Exam Report", out _FeeDueResult_WithheldSubvalue);
        DataSet ExamResultRemark_DataSet=new DataSet();
        DataSet WithHeldDataSet = new DataSet();
        ExamResultRemark_DataSet = null;
        if (_NeedDiff_FailResult_Result == "1")
        {
            ExamResultRemark_DataSet = MyExamMang.GetExamResultRemarks();
        }


        if (Need_FeeDueResult_Withheld == "1")
        {
            WithHeldDataSet = MyExamMang.GetFeeDueStudent(DrpClassName.SelectedValue);
        }

        int failcount = 1;
        //double aggregatemark = MyExamMang.GetAggregatemark(int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue);
        int _scheduledid = MyExamMang.GetExamScheduleId(int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue); 
        foreach (GridViewRow gv in Grd_CreateReport.Rows)
        {
             sql = "SELECT tblexammark.SubjectId,tblexammark.MarkColumn,tblsubjects.subject_name FROM tblexammark inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblsubjects on tblsubjects.Id=tblexammark.SubjectId  WHERE tblexamschedule.Id=" + _scheduledid +" order by tblexammark.SubjectOrder";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            
            if (MyReader.HasRows)
            {
                double _Total = 0;
                double _Max_Total = 0;
                int _Sub_Count = 0;
                string _Str_Mark;
                double _Mark = 0;
                string _Result="Passed";
                failcount = 1;
                FailSubCount = 0;
                while (MyReader.Read())
                {
                    double _maxmark = 0;
                    double _minmark=0;
                    MyExamMang.GetMaxAndMinMark(DrpClassName.SelectedValue, Session["ExamId"].ToString(), MyReader.GetValue(0).ToString(), out _maxmark, out _minmark);
                    _Str_Mark = MyExamMang.GetMarks(int.Parse(gv.Cells[0].Text.ToString()), int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, MyReader.GetValue(1).ToString(),DrpClassName.SelectedValue,DrpExamPeriod.SelectedValue);
                    if (_Str_Mark == "")
                    {
                       
                    }
                    else
                    {
                        _Mark = double.Parse(_Str_Mark.ToString());
                        if (_Mark < _minmark)
                        {
                            if (_NeedDiff_FailResult_Result == "1")
                            {
                                if (ExamResultRemark_DataSet != null && ExamResultRemark_DataSet.Tables[0].Rows.Count > 0)
                                {
                                    _Result = GetResultFrom_Count(failcount, ExamResultRemark_DataSet);
                                }
                                else
                                {
                                    _Result = "Failed";
                                }

                                failcount++;
                            }
                            else
                            {
                                _Result = "Failed";
                                FailSubCount++;
                            }
                        }
                        if (_Mark == -1)
                        {
                            _Total = _Total + 0;
                        }
                        if (_Mark == -2)
                        {
                            _Total = _Total + 0;

                        }
                        else
                        {
                            _Total = _Total + _Mark;
                        }

                        if (_Mark == -2)
                        {
                            _Max_Total = _Max_Total + 0;
                        }
                        else
                        {
                            _Max_Total = _Max_Total + _maxmark;
                        }
                        _Sub_Count = _Sub_Count + 1;
                    }
                }

                
                #region aggregate pass mark

                int _Examid = int.Parse(Session["ExamId"].ToString());
                string _examperiod = DrpExamPeriod.SelectedValue;
                int _classid = 0;
                int.TryParse(DrpClassName.SelectedValue.ToString(), out _classid);
                string m_sql = "SELECT tblsubgrouppassmark.SubjectId,tblsubgrouppassmark.MinPassMark from tblsubgrouppassmark where tblsubgrouppassmark.ClassID=" + _classid + " and tblsubgrouppassmark.ClassExamId=" + _Examid;
                DataSet ds_aggre = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(m_sql);

                #endregion


                if (FailSubCount.ToString() != "")
                {
                    string sqlquery = "UPDATE tblstudentmark SET FailSubCount=" + FailSubCount + " WHERE ExamSchId =" + _scheduledid + "  AND Studid=" + int.Parse(gv.Cells[0].Text.ToString());
                    MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sqlquery);
                }
                //MyExamMang.UpdateResult(int.Parse(gv.Cells[0].Text.ToString()), _scheduledid, _Max_Total, _Total, _Sub_Count, _Result);
                MyExamMang.UpdateResult(int.Parse(gv.Cells[0].Text.ToString()), _scheduledid, _Max_Total, _Total, _Sub_Count, _Result, ds_aggre, _scheduledid, _Examid, _classid, MyUser.CurrentBatchId, _examperiod, FailSubCount);
                MyExamMang.UpdateRemark("", _scheduledid, int.Parse(gv.Cells[0].Text.ToString()));
                if (Need_FeeDueResult_Withheld == "1")
                {
                    if (HasFeeDue(WithHeldDataSet, gv.Cells[0].Text.ToString()))
                    {
                        MyExamMang.UpdateRemark(_FeeDueResult_WithheldSubvalue, _scheduledid, int.Parse(gv.Cells[0].Text.ToString()));
                    }
                }
            }
         }
        sql = "update tblexamschedule set Published=0 where Id=" + _scheduledid;
        MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        sql = "delete from tblpromotionlist where tblpromotionlist.ClassId=" + DrpClassName.SelectedValue;
        MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        GenarateRank(_scheduledid, ExamResultRemark_DataSet);// Arun Modified


        



    }

    private void GenerateFailedStudReport()
    {
        string sql;
        int FailSubCount = 0;
        string _FailResult_ResultSubvalue = "";
        string _NeedDiff_FailResult_Result = MyExamMang.GetConfigValue("Remarks", "Exam Report", out _FailResult_ResultSubvalue);

        string _FeeDueResult_WithheldSubvalue = "";
        string Need_FeeDueResult_Withheld = MyExamMang.GetConfigValue("FeeDueResult_Withheld", "Exam Report", out _FeeDueResult_WithheldSubvalue);
        DataSet ExamResultRemark_DataSet = new DataSet();
        DataSet WithHeldDataSet = new DataSet();
        ExamResultRemark_DataSet = null;
        if (_NeedDiff_FailResult_Result == "1")
        {
            ExamResultRemark_DataSet = MyExamMang.GetExamResultRemarks();
        }


        if (Need_FeeDueResult_Withheld == "1")
        {
            WithHeldDataSet = MyExamMang.GetFeeDueStudent(DrpClassName.SelectedValue);
        }

        int failcount = 1;
        //double aggregatemark = MyExamMang.GetAggregatemark(int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue);
        int _scheduledid = MyExamMang.GetExamScheduleId(int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue);
        foreach (GridViewRow gv in Grd_CreateReport.Rows)
        {
            sql = "SELECT tblexammark.SubjectId,tblexammark.FailedMarkColumn,tblsubjects.subject_name FROM tblexammark inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblsubjects on tblsubjects.Id=tblexammark.SubjectId  WHERE tblexamschedule.Id=" + _scheduledid + " order by tblexammark.SubjectOrder";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                double _Total = 0;
                double _Max_Total = 0;
                int _Sub_Count = 0;
                string _Str_Mark;
                double _Mark = 0;
                string _Result = "Passed";
                failcount = 1;
                FailSubCount = 0;
                while (MyReader.Read())
                {
                    double _maxmark = 0;
                    double _minmark = 0;
                    MyExamMang.GetMaxAndMinMark(DrpClassName.SelectedValue, Session["ExamId"].ToString(), MyReader.GetValue(0).ToString(), out _maxmark, out _minmark);
                    _Str_Mark = MyExamMang.GetMarks(int.Parse(gv.Cells[0].Text.ToString()), int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, MyReader.GetValue(1).ToString(), DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue);
                    if (_Str_Mark == "")
                    {

                    }
                    else
                    {
                        _Mark = double.Parse(_Str_Mark.ToString());
                        if (_Mark < _minmark)
                        {
                            if (_NeedDiff_FailResult_Result == "1")
                            {
                                if (ExamResultRemark_DataSet != null && ExamResultRemark_DataSet.Tables[0].Rows.Count > 0)
                                {
                                    _Result = GetResultFrom_Count(failcount, ExamResultRemark_DataSet);
                                }
                                else
                                {
                                    _Result = "Failed";
                                }

                                failcount++;
                            }
                            else
                            {
                                _Result = "Failed";
                                FailSubCount++;
                            }
                        }
                        if (_Mark == -1)
                        {
                            _Total = _Total + 0;
                        }
                        else
                        {
                            _Total = _Total + _Mark;
                        }
                        _Max_Total = _Max_Total + _maxmark;
                        _Sub_Count = _Sub_Count + 1;
                    }
                }


                #region aggregate pass mark

                int _Examid = int.Parse(Session["ExamId"].ToString());
                string _examperiod = DrpExamPeriod.SelectedValue;
                int _classid = 0;
                int.TryParse(DrpClassName.SelectedValue.ToString(), out _classid);
                string m_sql = "SELECT tblsubgrouppassmark.SubjectId,tblsubgrouppassmark.MinPassMark from tblsubgrouppassmark where tblsubgrouppassmark.ClassID=" + _classid + " and tblsubgrouppassmark.ClassExamId=" + _Examid;
                DataSet ds_aggre = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(m_sql);

                #endregion



                //MyExamMang.UpdateResult(int.Parse(gv.Cells[0].Text.ToString()), _scheduledid, _Max_Total, _Total, _Sub_Count, _Result);
                MyExamMang.UpdateResult(int.Parse(gv.Cells[0].Text.ToString()), _scheduledid, _Max_Total, _Total, _Sub_Count, _Result, ds_aggre, _scheduledid, _Examid, _classid, MyUser.CurrentBatchId, _examperiod, FailSubCount);
                MyExamMang.UpdateRemark("", _scheduledid, int.Parse(gv.Cells[0].Text.ToString()));
                if (Need_FeeDueResult_Withheld == "1")
                {
                    if (HasFeeDue(WithHeldDataSet, gv.Cells[0].Text.ToString()))
                    {
                        MyExamMang.UpdateRemark(_FeeDueResult_WithheldSubvalue, _scheduledid, int.Parse(gv.Cells[0].Text.ToString()));
                    }
                }
            }
        }
        sql = "update tblexamschedule set Published=0 where Id=" + _scheduledid;
        MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        sql = "delete from tblpromotionlist where tblpromotionlist.ClassId=" + DrpClassName.SelectedValue;
        MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        GenarateRank(_scheduledid, ExamResultRemark_DataSet);// Arun Modified






    }

    private bool HasFeeDue(DataSet WithHeldDataSet, string StudentId)
    {
        bool Result = false;
        foreach (DataRow dr in WithHeldDataSet.Tables[0].Rows)
        {
            if (dr[0].ToString().Trim() == StudentId)
            {
                Result = true;
                break;
            }
        }
        return Result;
    }

    private string GetResultFrom_Count(int failcount, DataSet ExamResultRemark_DataSet)
    {
        string Result = "";
        foreach (DataRow dr in ExamResultRemark_DataSet.Tables[0].Rows)
        {
            if (dr[0].ToString().Trim() == failcount.ToString())
            {
                Result = dr[1].ToString().Trim();
                break;
            }
        }
        if(Result=="")
            Result = "Failed";
        return Result;
    }


    private string GetStaffNameFromClassStaffMap(int _SubjectId, int _ClassId)
    {
        OdbcDataReader MyReader1 = null;
        string _StaffName = "";
        int a = 0;
        string sql = "select tbluser.SurName from tblclassstaffmap inner join tbluser on tblclassstaffmap.StaffId=tbluser.Id where tblclassstaffmap.ClassId=" + _ClassId + " and tblclassstaffmap.SubjectId=" + _SubjectId;
        MyReader1 = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader1.HasRows)
        {
            while (MyReader1.Read())
            {
                if (a > 0)
                {
                    _StaffName = _StaffName + " / ";
                }
                _StaffName = _StaffName + MyReader1.GetValue(0).ToString();
                a++;
            }
        }
        return _StaffName;
    }


    private void GenarateRank(int ExScID, DataSet ExamResultRemark_DataSet)
    { 
        double _totalmark = 0;
        int i=0;
        string sql = "SELECT tblstudentmark.StudId,tblstudentmark.Result,tblstudentmark.TotalMark , tblstudentmark.TotalMax, tblstudentmark.Grade , tblstudentmark.Result from tblstudentmark inner join tblstudent on tblstudent.id=tblstudentmark.StudId WHERE tblstudent.Status=1 and tblstudentmark.ExamSchId=" + ExScID + " Order by tblstudentmark.TotalMark DESC";
        MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                int _studid = int.Parse(MyReader.GetValue(0).ToString());
                //if ("Failed" != MyReader.GetValue(1).ToString()) Arun Modified
                if (IsPass(MyReader.GetValue(1).ToString(), ExamResultRemark_DataSet))
                {
                    if (_totalmark != double.Parse(MyReader.GetValue(2).ToString()))
                    {
                        i++;
                        _totalmark = double.Parse(MyReader.GetValue(2).ToString());
                        MyExamMang.UpdateRank(i, ExScID, _studid);
                    }
                    else
                    {
                        MyExamMang.UpdateRank(i, ExScID, _studid);
                    }
                }
                else
                {
                    _totalmark = double.Parse(MyReader.GetValue(2).ToString());
                    MyExamMang.UpdateRank(0, ExScID, _studid);
                }
               // MyExamMang.ReportIncident(_studid, i.ToString(), int.Parse(Session["ExamId"].ToString()), _totalmark, MyReader.GetValue(3).ToString(), MyReader.GetValue(4).ToString(), MyReader.GetValue(5).ToString());
            }            
        }
        MyExamMang.UpdateExamSchedule(DrpClassName.SelectedValue,DrpExamPeriod.SelectedValue,int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, "Completed");
    }

    private bool IsPass(string Result, DataSet ExamResultRemark_DataSet)
    {
        bool _valid = true;

        if (Result == "Failed")
        {
            _valid = false;
        }
        else if (ExamResultRemark_DataSet != null && ExamResultRemark_DataSet.Tables[0] !=null&& ExamResultRemark_DataSet.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr in ExamResultRemark_DataSet.Tables[0].Rows)
            {
                if (dr[1].ToString().Trim() == Result)
                {
                    _valid = false;
                    break;
                }
            }

        }

        return _valid;
    }

    protected void Btn_Gen_Click(object sender, EventArgs e)
    {
        GenerateAllStudReport(); 
        //GenerateFailedStudReport();
        LoadResult();
        MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Exam Report", "Exam Report Generated for " + Lbl_ExamName.Text.ToString() + " for " + DrpClassName.SelectedItem.Text, 1,1);

    }

    protected void DrpClassName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["ClassId"] = DrpClassName.SelectedValue;
        SetPanels();
    }

    protected void DrpExamPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["PeriodId"] = DrpExamPeriod.SelectedValue;
        SetPanels();
    }

    protected void Img_ExportToExcel_Click(object sender, EventArgs e)
    {
        string sql = "SELECT  tblstudent.StudentName AS `Student Name`, tblstudentclassmap.RollNo, tblstudentmark.TotalMark AS `Mark Obtained` , tblstudentmark.TotalMax AS `Maximum Mark`, ROUND(tblstudentmark.`Avg`, 2) AS `Average`, tblstudentmark.Grade, tblstudentmark.Result, tblstudentmark.Rank, tblstudentmark.Remark from tblexamschedule inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId inner join tblstudentmark on tblstudentmark.ExamSchId= tblexamschedule.Id inner join tblstudent on tblstudent.Id= tblstudentmark.StudId inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudentmark.StudId AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " And tblexamschedule.PeriodId=" + DrpExamPeriod.SelectedValue + " AND tblclassexam.ClassId=" + DrpClassName.SelectedValue + " AND tblclassexam.ExamId=" + Session["ExamId"].ToString() + " order by tblstudentclassmap.RollNo ASC";
        MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
            //if (!WinEr.ExcelUtility.ExportDataSetToExcel(MydataSet, "ExamResult_"+DrpExamPeriod.SelectedItem.Text+".xls"))
            //{
            //    Lbl_message.Text = "This function need Ms office";
            //}
            string FileName = "ExamResult" + DrpExamPeriod.SelectedItem.Text;
            string _ReportName = "ExamResult"+DrpExamPeriod.SelectedItem.Text;
            if (!WinEr.ExcelUtility.ExportDataToExcel(MydataSet, _ReportName, FileName, MyUser.ExcelHeader))
            {
                Lbl_message.Text = "This function need Ms office";
            }
        }
    }

    protected void lnk_manageExam_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageClassExam.aspx");
    }

    protected void lnk_Schedule_Click(object sender, EventArgs e)
    {
        Response.Redirect("ScheduleExam.aspx");
    }
    protected void lnk_Marks_Click(object sender, EventArgs e)
    {
        Response.Redirect("EnterMark.aspx");
    }

}