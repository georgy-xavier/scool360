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
using WinEr;
using WinBase;
using System.Drawing;

namespace WinEr
{
    public partial class PromotionListNew : System.Web.UI.Page
    {
        private ConfigManager MyConfiMang;
        private DataSet MydataSet;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private OdbcDataReader m_MyReader = null;
        private Attendance MyAttendance;
        private StudentManagerClass MyStudMang;

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            else
            {
                MyUser = (KnowinUser)Session["UserObj"];

                if (MyUser.SELECTEDMODE == 1)
                {
                    this.MasterPageFile = "~/WinerStudentMaster.master";

                }
                else if (MyUser.SELECTEDMODE == 2)
                {

                    this.MasterPageFile = "~/WinerSchoolMaster.master";
                }
            }

        }
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyConfiMang = MyUser.GetConfigObj();
            MyAttendance = MyUser.GetAttendancetObj();
            MyStudMang = MyUser.GetStudentObj();

            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            if (MyConfiMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(303))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadBatchToDropDown();
                    LoadAllClassToDropDownList();                    
                    Pnl_StudentsList.Visible = false;
                    LoadStudentsListCondition();
                    LoadStudentsListOfSelectedType();

                    LoadPromotionListStatusGrid();
                    LoadGenerateButtonStatus();
                    LoadAllClassToPreviewDropDown();
                    LoadPreviewBatchToDropDown();
                }
            }
        }
        


        #region PROMOTION LIST AREA FUNCTIONS

        private void LoadStudentsListCondition()
        {
            int _BatchId = MyUser.CurrentBatchId;
            if (Drp_SelectType.SelectedValue == "1")
            {
                _BatchId = (MyUser.CurrentBatchId - 1);
            }
            if (CheckValidMarks())
            {
                DataSet _StudentList = new DataSet();
                //string sql = "SELECT tblpromotionlist.Id, tblpromotionlist.StudentId, tblpromotionlist.StudentName, tblpromotionlist.RollNo, tblpromotionlist.AdmissionNo, tblpromotionlist.Remarks from tblpromotionlist where tblpromotionlist.ClassId=" + Drp_Class.SelectedValue + " and tblpromotionlist.BatchId=" + MyUser.CurrentBatchId;
                string sql = "SELECT tblpromotionlist.Id, tblpromotionlist.StudentId, tblpromotionlist.StudentName, tblpromotionlist.RollNo, tblpromotionlist.AdmissionNo, tblpromotionlist.Remarks from tblpromotionlist where tblpromotionlist.ClassId=" + Drp_Class.SelectedValue + " and tblpromotionlist.ToBatchId=" + MyUser.CurrentBatchId;
                _StudentList = MyConfiMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (_StudentList.Tables[0].Rows.Count <= 0)
                {
                    Lbl_note.Text = "No List Exists.Generate Promotion List";
                    Pnl_StudentsList.Visible = false;
                }
                else
                {
                    Lbl_note.Text = "";
                }
            }

            else
            {
                string sql = "Delete from tblpromotionlist where tblpromotionlist.ClassId=" + Drp_Class.SelectedValue;
                MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
                Lbl_note.Text = "Exam Report is not generated for the Class..";
                // Drp_SelectType.Enabled = true;
                //Btn_GenerateList.Enabled = false;
            }
        }

        private void GenerateStudentsListBasedOnPromotionRules(out DataSet _StudentPromotion)
        {
            string[,] _PromotionRules = new string[20, 2];
            GetAllPromotionRuleId(out _PromotionRules);
            int _ExamCount = 0, __AttendanceCount = 0;
            string[,] _ExamPromotionRules = new string[20, 5];
            string[,] _AttendancePromotionRules = new string[20, 2];

            _StudentPromotion = new DataSet();

            if (HaveExamPromotionRule(out _ExamPromotionRules, out _ExamCount))
            {
            }
            if(HaveAttendancePromotionRule(out _AttendancePromotionRules, out __AttendanceCount))
            {                
            }
            if (_ExamCount > 0 || __AttendanceCount > 0)
            {
                _StudentPromotion = GenerateStudentListOnPromotionRules(_ExamPromotionRules, _ExamCount, _AttendancePromotionRules, __AttendanceCount);
            }
            else
            {
                _StudentPromotion = GenerateStudentsListForOPromotionWithNoRules();
            }
        }

        private DataSet GenerateStudentsListForOPromotionWithNoRules()
        {
            int _BatchId = 0, _ToBatchId = 0;
            if (Drp_SelectType.SelectedValue == "1")
            {
                _BatchId = (MyUser.CurrentBatchId - 1);
                _ToBatchId = MyUser.CurrentBatchId;
            }
            else
            {
                _BatchId = MyUser.CurrentBatchId;
                _ToBatchId = (MyUser.CurrentBatchId + 1);
            }

            DataSet StudentDataSet = new DataSet();
            DataTable dt;
            DataRow dr;
            StudentDataSet.Tables.Add(new DataTable("Student"));
            dt = StudentDataSet.Tables["Student"];

            dt.Columns.Add("StudentId");
            dt.Columns.Add("StudentName");
            dt.Columns.Add("RollNo");
            dt.Columns.Add("ClassId");
            dt.Columns.Add("BatchId");
            dt.Columns.Add("AdmissionNo");
            dt.Columns.Add("IsEligible");
            dt.Columns.Add("Remarks");
            dt.Columns.Add("ToBatchId");
            dt.Columns.Add("Link");

            string sql = "SELECT tblview_studentclassmap.StudentId, tblview_studentclassmap.RollNo, tblstudent.StudentName,tblstudent.AdmitionNo from tblview_studentclassmap inner JOIN tblstudent on tblview_studentclassmap.StudentId= tblstudent.Id  where tblstudent.`Status`=1 and tblview_studentclassmap.ClassId=" + Drp_Class.SelectedValue + " and tblview_studentclassmap.BatchId=" + _BatchId + " and tblview_studentclassmap.StudentId not in (select tblpromotionlist.StudentId from tblpromotionlist union select tblstudentclassmap.StudentId from tblstudentclassmap where tblstudentclassmap.BatchId=" + _ToBatchId + ")";
            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    dr = StudentDataSet.Tables["Student"].NewRow();

                    dr["StudentId"] = MyReader.GetValue(0).ToString();
                    dr["StudentName"] = MyReader.GetValue(2).ToString();
                    dr["RollNo"] = MyReader.GetValue(1).ToString();
                    dr["ClassId"] = Drp_Class.SelectedValue;
                    dr["BatchId"] = _BatchId.ToString();
                    dr["AdmissionNo"] = MyReader.GetValue(3).ToString();
                    dr["IsEligible"] = "1";
                    dr["Remarks"] = "Eligible For Promotion";
                    dr["ToBatchId"] = _ToBatchId.ToString();
                    dr["Link"] = "Make Non-Eligible";
                    StudentDataSet.Tables["Student"].Rows.Add(dr);
                }
            }
            return StudentDataSet;
        }

        private DataSet GenerateStudentListOnPromotionRules(string[,] _ExamPromotionRules, int _ExamCount, string[,] _AttendancePromotionRules, int __AttendanceCount)
        {
            int _BatchId = 0, _ToBatchId = 0;
            if (Drp_SelectType.SelectedValue == "1")
            {
                _BatchId = (MyUser.CurrentBatchId - 1);
                _ToBatchId = MyUser.CurrentBatchId;
            }
            else
            {
                _BatchId = MyUser.CurrentBatchId;
                _ToBatchId = (MyUser.CurrentBatchId + 1);
            }
            
            DataSet StudentDataSet = new DataSet();
            DataTable dt;
            DataRow dr;
            StudentDataSet.Tables.Add(new DataTable("Student"));
            dt = StudentDataSet.Tables["Student"];

            dt.Columns.Add("StudentId");
            dt.Columns.Add("StudentName");
            dt.Columns.Add("RollNo");
            dt.Columns.Add("ClassId");
            dt.Columns.Add("BatchId");
            dt.Columns.Add("AdmissionNo");
            dt.Columns.Add("IsEligible");
            dt.Columns.Add("Remarks");
            dt.Columns.Add("ToBatchId");
            dt.Columns.Add("Link");

            string sql = "SELECT tblview_studentclassmap.StudentId, tblview_studentclassmap.RollNo, tblstudent.StudentName,tblstudent.AdmitionNo from tblview_studentclassmap inner JOIN tblstudent on tblview_studentclassmap.StudentId= tblstudent.Id  where tblstudent.`Status`=1 and tblview_studentclassmap.ClassId=" + Drp_Class.SelectedValue + " and tblview_studentclassmap.BatchId=" + _BatchId + " and tblview_studentclassmap.StudentId not in (select tblpromotionlist.StudentId from tblpromotionlist union select tblstudentclassmap.StudentId from tblstudentclassmap where tblstudentclassmap.BatchId=" + _ToBatchId + ")";
            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    string _RemarksExam = "", _RemarksAttendance = "",_FinalMesssage="";
                    int _ExamEligible = 1, _AttendanceEligible = 1;

                    dr = StudentDataSet.Tables["Student"].NewRow();

                    dr["StudentId"] = MyReader.GetValue(0).ToString();
                    dr["StudentName"] = MyReader.GetValue(2).ToString();
                    dr["RollNo"] = MyReader.GetValue(1).ToString();
                    dr["ClassId"] = Drp_Class.SelectedValue;
                    dr["BatchId"] = _BatchId.ToString();
                    dr["AdmissionNo"] = MyReader.GetValue(3).ToString();
                    dr["ToBatchId"] = _ToBatchId.ToString();
                    if (StudentIsEligibleByExamRules(int.Parse(MyReader.GetValue(0).ToString()), _ExamCount, _ExamPromotionRules, out _RemarksExam, out _ExamEligible) && StudentIsElgibleByAttendanceRules(int.Parse(MyReader.GetValue(0).ToString()), __AttendanceCount, _AttendancePromotionRules, out _RemarksAttendance, out _AttendanceEligible))
                    {
                        dr["IsEligible"] = "1";
                        dr["Remarks"] = "Eligible For Promotion";
                        dr["Link"] = "Make Non-Eligible";
                    }
                    else
                    {
                        dr["IsEligible"] = "0";
                        dr["Link"] = "Make Eligible";

                        if (_ExamCount > 0 && _ExamEligible == 0)
                        {
                            _FinalMesssage = _RemarksExam;
                        }
                        if (__AttendanceCount > 0 && _AttendanceEligible==0)
                        {
                            if (_ExamCount > 0 && _ExamEligible == 0)
                            {
                                _FinalMesssage = _FinalMesssage + " and " + _RemarksAttendance;
                            }
                            else
                            {
                                _FinalMesssage = _RemarksAttendance;
                            }                            
                        }
                        dr["Remarks"] = _FinalMesssage;
                    }

                    StudentDataSet.Tables["Student"].Rows.Add(dr);
                }
            }
            return StudentDataSet;
        }

        private bool StudentIsElgibleByAttendanceRules(int _StudentId, int __AttendanceCount, string[,] _AttendancePromotionRules, out string _RemarksAttendance, out int _AttendanceEligible)
        {
            OdbcDataReader MyReaderAttendance = null;
            bool _Eligible = true;
            _RemarksAttendance = "";
            _AttendanceEligible = 1;
            string _StartDate = GetStartDateOfCurrentBatch();
            string _EndDate = DateTime.Now.Date.ToString("s");//MyUser.GerFormatedDatVal(DateTime.Now.Date);
            int _WorkingDays = 0, _PresentDays = 0, _AbsentDays = 0;
            int _Standard = MyStudMang.GetStandard(_StudentId.ToString());
            int _TotalWorkingDays = 0;

            if (Drp_SelectType.SelectedValue == "1")
            {
                double _StudAttendancePer = 0;
                //string sql = "select tblattendancehistory.percentage, tblattendancehistory_master.totalWorkingDays from tblattendancehistory inner join tblattendancehistory_master on tblattendancehistory.masterId= tblattendancehistory_master.Id where tblattendancehistory.studentId=" + _StudentId + " and tblattendancehistory_master.classId=" + Drp_Class.SelectedValue + " and tblattendancehistory_master.batchId=" + (MyUser.CurrentBatchId - 1);
                string sql = "select tblattdperivousbatch.PresentDays, tblattdperivousbatch.WorkingDays from tblattdperivousbatch where tblattdperivousbatch.StudentId=" + _StudentId + " and tblattdperivousbatch.ClassId=" + Drp_Class.SelectedValue + " and tblattdperivousbatch.BatchId=" + (MyUser.CurrentBatchId - 1);
                MyReaderAttendance = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReaderAttendance.HasRows)
                {
                    int.TryParse(MyReaderAttendance.GetValue(0).ToString(), out _PresentDays);
                    int.TryParse(MyReaderAttendance.GetValue(1).ToString(), out _TotalWorkingDays);
                    if (_TotalWorkingDays > 0)
                    {
                        _StudAttendancePer = (_PresentDays / _TotalWorkingDays) * 100;
                    }
                }

                for (int i = 0; i < __AttendanceCount; i++)
                {
                    if (_StudAttendancePer < (double.Parse(_AttendancePromotionRules[i, 0])) && _TotalWorkingDays > 0)
                    {
                        _AttendanceEligible = 0;
                        _Eligible = false;
                        _RemarksAttendance = "Attendance is Less than " + _AttendancePromotionRules[i, 0] + " %";
                    }
                }
            }
            else
            {
                //_WorkingDays = MyAttendance.get_NoOf_WorkingDaysForThePeriod(int.Parse(Drp_Class.SelectedValue), _StartDate, _EndDate);
                //_AbsentDays = MyAttendance.get_NoOf_AbsentDayForTheperiod(_StudentId, int.Parse(Drp_Class.SelectedValue), _StartDate, _EndDate);
                _WorkingDays = MyAttendance.GetWorkingDaysForThePeriod_New(_StudentId, _Standard.ToString(), MyUser.CurrentBatchId, _StartDate, _EndDate);
                _AbsentDays = MyAttendance.GetNoOf_AbsentDayForTheperiod_New(_StudentId, _Standard.ToString(), MyUser.CurrentBatchId, _StartDate, _EndDate);
                _PresentDays = _WorkingDays - _AbsentDays;

                for (int i = 0; i < __AttendanceCount; i++)
                {
                    if (_PresentDays < _WorkingDays * ((double.Parse(_AttendancePromotionRules[i, 0]) / 100)))
                    {
                        _AttendanceEligible = 0;
                        _Eligible = false;
                        _RemarksAttendance = "Attendance is Less than " + _AttendancePromotionRules[i, 0] + " %";
                    }
                }
            }

            return _Eligible;
        }

        private string GetStartDateOfCurrentBatch()
        {
            OdbcDataReader MyReaderDate = null;
            string _StartDate = "";
            DateTime _StrtDate = new DateTime();
            string sql = "SELECT tblbatch.StartDate from tblbatch where tblbatch.Id=" + MyUser.CurrentBatchId;
            MyReaderDate = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReaderDate.HasRows)
            {
                DateTime.TryParse(MyReaderDate.GetValue(0).ToString(), out _StrtDate);
            }
            _StartDate = _StrtDate.ToString("s");
            return _StartDate;
        }

        private bool StudentIsEligibleByExamRules(int _StudentId, int _ExamCount, string[,] _ExamPromotionRules, out string _Remarks, out int _ExamEligible)
        {
            bool _Eligible = true;
            _Remarks = "";
            _ExamEligible = 1;
            string _Msg = "Low Marks For ";
            double _IndTotal = 0, _IndTotalMax = 0, _CombinedTotal = 0, _CombinedTotalMax = 0;
            for (int i = 0; i < _ExamCount; i++)
            {
                if (_ExamPromotionRules[i, 0] == "1")
                {
                    _CombinedTotal = GetCombinedTotalForExam(_ExamPromotionRules[i, 1], _StudentId);
                    _CombinedTotalMax = GetCombinedTotalMaxForExam(_ExamPromotionRules[i, 1], _StudentId);
                    if (_CombinedTotal < (_CombinedTotalMax * (double.Parse(_ExamPromotionRules[i, 3]) / 100)))
                    {
                        _ExamEligible = 0;
                        _Eligible = false;
                        _Msg = _Msg + " " + _ExamPromotionRules[i, 2] + ",";
                    }
                }
                else
                {
                    _IndTotal = GetTotalMarkForIndividualExamSchedule(_ExamPromotionRules[i, 1], _StudentId, int.Parse(_ExamPromotionRules[i, 4]));
                    _IndTotalMax = GetTotalMarkMaxForIndividualExamSchedule(_ExamPromotionRules[i, 1], _StudentId, int.Parse(_ExamPromotionRules[i, 4]));
                    if (_IndTotal < (_IndTotalMax * (double.Parse(_ExamPromotionRules[i, 3]) / 100)))
                    {
                        _ExamEligible = 0;
                        _Eligible = false;
                        _Msg = _Msg + _ExamPromotionRules[i, 2] + ",";
                    }
                }
            }
            _Remarks = _Msg;
            return _Eligible;
        }

        private double GetTotalMarkMaxForIndividualExamSchedule(string _ExamId, int _StudentId, int _PeriodId)
        {
            OdbcDataReader MyReaderTotal = null;
            double _CombineTotalMax = 0;

            string sql = "";

            if (Drp_SelectType.SelectedValue == "1")
            {
                sql = "SELECT tblstudentmark_history.TotalMax from tblstudentmark_history where tblstudentmark_history.StudId=" + _StudentId + " AND  tblstudentmark_history.ExamSchId =(SELECT tblexamschedule_history.Id FROM tblexamschedule_history WHERE tblexamschedule_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " and tblexamschedule_history.PeriodId=" + _PeriodId + " and tblexamschedule_history.ClassId=" + Drp_Class.SelectedValue + " and tblexamschedule_history.ExamId=" + _ExamId + ")";
            }
            else
            {
                sql = "SELECT tblstudentmark.TotalMax from tblstudentmark where tblstudentmark.StudId=" + _StudentId + " AND  tblstudentmark.ExamSchId =(SELECT tblexamschedule.Id FROM tblexamschedule WHERE tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblexamschedule.PeriodId=" + _PeriodId + " and tblexamschedule.ClassExamId= (select tblclassexam.Id from tblclassexam where tblclassexam.ClassId=" + Drp_Class.SelectedValue + " and tblclassexam.ExamId=" + _ExamId + "))";
            }

            MyReaderTotal = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReaderTotal.HasRows)
            {
                double.TryParse(MyReaderTotal.GetValue(0).ToString(), out _CombineTotalMax);
            }
            return _CombineTotalMax;
        }

        private double GetTotalMarkForIndividualExamSchedule(string _ExamId, int _StudentId, int _PeriodId)
        {
            OdbcDataReader MyReaderTotal = null;
            double _CombineTotal = 0;
            string sql = "";

            if (Drp_SelectType.SelectedValue == "1")
            {
                sql = "SELECT tblstudentmark_history.TotalMark from tblstudentmark_history where tblstudentmark_history.StudId=" + _StudentId + " AND  tblstudentmark_history.ExamSchId =(SELECT tblexamschedule_history.Id FROM tblexamschedule_history WHERE tblexamschedule_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " and tblexamschedule_history.PeriodId=" + _PeriodId + " and tblexamschedule_history.ClassId=" + Drp_Class.SelectedValue + " and tblexamschedule_history.ExamId=" + _ExamId + ")";
            }
            else
            {
                sql = "SELECT tblstudentmark.TotalMark from tblstudentmark where tblstudentmark.StudId=" + _StudentId + " AND  tblstudentmark.ExamSchId =(SELECT tblexamschedule.Id FROM tblexamschedule WHERE tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblexamschedule.PeriodId=" + _PeriodId + " and tblexamschedule.ClassExamId= (select tblclassexam.Id from tblclassexam where tblclassexam.ClassId=" + Drp_Class.SelectedValue + " and tblclassexam.ExamId=" + _ExamId + "))";
            }

            MyReaderTotal = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReaderTotal.HasRows)
            {
                double.TryParse(MyReaderTotal.GetValue(0).ToString(), out _CombineTotal);
            }
            return _CombineTotal;
        }

        private double GetCombinedTotalMaxForExam(string _ExamId, int _StudentId)
        {
            OdbcDataReader MyReaderTotal = null;
            double _CombineTotalMax = 0;
            string sql = "";
            if (Drp_SelectType.SelectedValue == "1")
            {
                sql = "SELECT SUM(tblstudentmark_history.TotalMax) from tblstudentmark_history where tblstudentmark_history.StudId=" + _StudentId + " AND  tblstudentmark_history.ExamSchId in(select tblexamschedule_history.Id from tblexamschedule_history inner join tblclassexam on tblexamschedule_history.ClassId= tblclassexam.ClassId inner join tblexamcombmap on tblexamcombmap.ExamId= tblclassexam.ExamId where tblexamschedule_history.PeriodId= tblexamcombmap.PeriodId and tblclassexam.ClassId= tblexamcombmap.ClassId AND tblexamcombmap.ClassId=" + Drp_Class.SelectedValue + " and  tblexamcombmap.CombinedId=" + _ExamId + ")";
            }
            else
            {
                sql = "SELECT SUM(tblstudentmark.TotalMax) from tblstudentmark where tblstudentmark.StudId=" + _StudentId + " AND  tblstudentmark.ExamSchId in(select tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexamcombmap on tblexamcombmap.ExamId= tblclassexam.ExamId where tblexamschedule.PeriodId= tblexamcombmap.PeriodId and tblclassexam.ClassId= tblexamcombmap.ClassId AND tblexamcombmap.ClassId=" + Drp_Class.SelectedValue + " and  tblexamcombmap.CombinedId=" + _ExamId + ")";
            }

            MyReaderTotal = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReaderTotal.HasRows)
            {
                double.TryParse(MyReaderTotal.GetValue(0).ToString(), out _CombineTotalMax);
            }
            return _CombineTotalMax;
        }

        private double GetCombinedTotalForExam(string _ExamId, int _StudentId)
        {
            OdbcDataReader MyReaderTotal = null;
            double _CombineTotal = 0;
            string sql = "";
            if (Drp_SelectType.SelectedValue == "1")
            {
                sql = "SELECT SUM(tblstudentmark_history.TotalMark) from tblstudentmark_history where tblstudentmark_history.StudId=" + _StudentId + " AND  tblstudentmark_history.ExamSchId in(select tblexamschedule_history.Id from tblexamschedule_history inner join tblclassexam on tblexamschedule_history.ClassId= tblclassexam.ClassId inner join tblexamcombmap on tblexamcombmap.ExamId= tblclassexam.ExamId where tblexamschedule_history.PeriodId= tblexamcombmap.PeriodId and tblclassexam.ClassId= tblexamcombmap.ClassId AND tblexamcombmap.ClassId=" + Drp_Class.SelectedValue + " and  tblexamcombmap.CombinedId=" + _ExamId + ")";
            }
            else
            {
                sql = "SELECT SUM(tblstudentmark.TotalMark) from tblstudentmark where tblstudentmark.StudId=" + _StudentId + " AND  tblstudentmark.ExamSchId in(select tblexamschedule.Id from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexamcombmap on tblexamcombmap.ExamId= tblclassexam.ExamId where tblexamschedule.PeriodId= tblexamcombmap.PeriodId and tblclassexam.ClassId= tblexamcombmap.ClassId AND tblexamcombmap.ClassId=" + Drp_Class.SelectedValue + " and  tblexamcombmap.CombinedId=" + _ExamId + ")";
            }
            MyReaderTotal = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReaderTotal.HasRows)
            {
                double.TryParse(MyReaderTotal.GetValue(0).ToString(), out _CombineTotal);
            }
            return _CombineTotal;
        }

        private bool HaveAttendancePromotionRule(out string[,] _AttendancePromotionRules, out int _AttendanceCount)
        {
            bool _HaveRule = false;
            _AttendanceCount = 0;
            _AttendancePromotionRules = new string[10, 2];
            string sql = "SELECT tblpromotionrule_attendance.Percentage, tblpromotionrule_attendance.MasterId from tblpromotionrule_attendance where tblpromotionrule_attendance.MasterId in(SELECT tblclasspromotionrulemap.RuleId from tblclasspromotionrulemap inner join tblpromotionrule_master on tblpromotionrule_master.RuleId= tblclasspromotionrulemap.RuleId where tblclasspromotionrulemap.ClassId=" + Drp_Class.SelectedValue + " and tblpromotionrule_master.`Type`=2)";
            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _AttendancePromotionRules = new string[MyReader.RecordsAffected, 2];
                int i = 0;
                _HaveRule = true;
                while (MyReader.Read())
                {
                    _AttendancePromotionRules[i, 0] = MyReader.GetValue(0).ToString();
                    _AttendancePromotionRules[i, 1] = MyReader.GetValue(1).ToString();
                    i++;
                }
                _AttendanceCount = i;
            }
            return _HaveRule;
        }

        private bool HaveExamPromotionRule(out string[,] _ExamPromotionRules, out int _ExamCount)
        {
            bool _HaveRules = false;
            _ExamCount = 0;
            _ExamPromotionRules = new string[20, 5];
            string sql = "SELECT tblpromotionrule_exam.Combined,tblpromotionrule_exam.ExamId, tblpromotionrule_exam.ExamName, tblpromotionrule_exam.Percentage, tblpromotionrule_exam.Period from tblpromotionrule_exam where tblpromotionrule_exam.MasterId in(SELECT tblclasspromotionrulemap.RuleId from tblclasspromotionrulemap inner join tblpromotionrule_master on tblpromotionrule_master.RuleId= tblclasspromotionrulemap.RuleId where tblclasspromotionrulemap.ClassId=" + Drp_Class.SelectedValue + " and tblpromotionrule_master.`Type`=1)";
            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _ExamPromotionRules = new string[MyReader.RecordsAffected, 5];
                int i = 0;
                _HaveRules = true;
                while (MyReader.Read())
                {
                    _ExamPromotionRules[i, 0] = MyReader.GetValue(0).ToString();
                    _ExamPromotionRules[i, 1] = MyReader.GetValue(1).ToString();
                    _ExamPromotionRules[i, 2] = MyReader.GetValue(2).ToString();
                    _ExamPromotionRules[i, 3] = MyReader.GetValue(3).ToString();
                    _ExamPromotionRules[i, 4] = MyReader.GetValue(4).ToString();
                    i++;
                }
                _ExamCount = i;
            }

            return _HaveRules;
        }

        private void GetAllPromotionRuleId(out string[,] _PromotionRules)
        {
            _PromotionRules = new string[0, 2];
            string sql = "SELECT tblclasspromotionrulemap.RuleId, tblpromotionrule_master.`Type` from tblclasspromotionrulemap inner join tblpromotionrule_master on tblpromotionrule_master.RuleId= tblclasspromotionrulemap.RuleId where tblclasspromotionrulemap.ClassId=" + Drp_Class.SelectedValue;
            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int i = 0;
                _PromotionRules = new string[MyReader.RecordsAffected, 2];
                while (MyReader.Read())
                {
                    _PromotionRules[i, 0] = MyReader.GetValue(0).ToString();
                    _PromotionRules[i, 1] = MyReader.GetValue(1).ToString();
                    i++;
                }
            }
        }

        private void LoadToClassToDropDown()
        {
            DataSet _DSEligibleToClass = null, _DSNonEligible = null;

            string sql = "Select tblclass.Id,tblclass.ClassName from tblclass WHERE tblclass.Status=1 and tblclass.Standard IN (select tblpromotionmap.StdTo from tblpromotionmap inner join tblclass on tblclass.Standard=tblpromotionmap.StdFrom where tblclass.Id=" + int.Parse(Drp_Class.SelectedValue.ToString()) + ")";
            _DSEligibleToClass = MyConfiMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            sql = "Select tblclass.Id,tblclass.ClassName from tblclass WHERE tblclass.Status=1 and tblclass.Standard IN (select tblpromotionmap.StdFrom from tblpromotionmap inner join tblclass on tblclass.Standard=tblpromotionmap.StdFrom where tblclass.Id=" + int.Parse(Drp_Class.SelectedValue.ToString()) + ")";
            _DSNonEligible = MyConfiMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            foreach (GridViewRow gv in Grd_StudentsList.Rows)
            {
                int _IsEligible = int.Parse(gv.Cells[10].Text);
                int _ToClassId = int.Parse(gv.Cells[11].Text);

                DropDownList Drp_ToClass = (DropDownList)gv.FindControl("Drp_ToClass");
                if (_IsEligible == 1)
                {
                    LoadToClassForEligibleStudents(Drp_ToClass, _DSEligibleToClass, _ToClassId);
                }
                else
                {
                    LoadToClassForNonEligibleStudents(Drp_ToClass, _DSNonEligible, _ToClassId);
                }
            }
        }

        private void LoadToClassForNonEligibleStudents(DropDownList Drp_ToClass, DataSet _DSNonEligible, int _ToClassId)
        {
            Drp_ToClass.Items.Clear();

            if (_DSNonEligible.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in _DSNonEligible.Tables[0].Rows)
                {
                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_ToClass.Items.Add(li);
                }
                Drp_ToClass.SelectedValue = _ToClassId.ToString();
            }
            else
            {
                ListItem li = new ListItem("No Class present", "-1");
                Drp_ToClass.Items.Add(li);
            }
        }

        private void LoadToClassForEligibleStudents(DropDownList Drp_ToClass, DataSet _DSEligibleToClass, int _ToClassId)
        {
            Drp_ToClass.Items.Clear();

            if (_DSEligibleToClass.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in _DSEligibleToClass.Tables[0].Rows)
                {
                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_ToClass.Items.Add(li);
                }
                Drp_ToClass.SelectedValue = _ToClassId.ToString();
            }
            else
            {
                ListItem li = new ListItem("No Class present", "-1");
                Drp_ToClass.Items.Add(li);
            }
        }

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pnl_StudentsList.Visible = false;
            LoadStudentsListCondition();
            LoadStudentsListOfSelectedType();

            LoadGenerateButtonStatus();
        }

        protected void Drp_SelectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAllClassToDropDownList();
            //LoadPromotionListStatusGrid();
            LoadStudentsListOfSelectedType();
            LoadGenerateButtonStatus();
        }

        private void LoadStudentsListOfSelectedType()
        {
            int _BatchId = MyUser.CurrentBatchId;
            if (Drp_SelectType.SelectedValue == "1")
            {
                _BatchId = (MyUser.CurrentBatchId - 1);
            }

            DataSet _StudentList = new DataSet();
            string sql = "SELECT tblpromotionlist.Id, tblpromotionlist.StudentId, tblpromotionlist.StudentName, tblpromotionlist.RollNo, tblpromotionlist.AdmissionNo, tblpromotionlist.Remarks,tblpromotionlist.IsEligible,tblpromotionlist.Link, tblpromotionlist.ToClassId from tblpromotionlist where tblpromotionlist.ClassId=" + Drp_Class.SelectedValue + " and tblpromotionlist.BatchId=" + _BatchId + " order by tblpromotionlist.RollNo asc";
            _StudentList = MyConfiMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_StudentList.Tables[0].Rows.Count > 0)
            {
                Pnl_StudentsList.Visible = true;
                Grd_StudentsList.Columns[0].Visible = true;
                Grd_StudentsList.Columns[1].Visible = true;
                Grd_StudentsList.Columns[5].Visible = true;
                Grd_StudentsList.Columns[7].Visible = true;
                Grd_StudentsList.Columns[10].Visible = true;
                Grd_StudentsList.Columns[11].Visible = true;
                Grd_StudentsList.DataSource = _StudentList;
                Grd_StudentsList.DataBind();
                Grd_StudentsList.Columns[0].Visible = false;
                Grd_StudentsList.Columns[1].Visible = false;
                Grd_StudentsList.Columns[5].Visible = false;
                Grd_StudentsList.Columns[7].Visible = false;
                Grd_StudentsList.Columns[10].Visible = false;
                Grd_StudentsList.Columns[11].Visible = false;

                Lbl_note.Text = "";

                LoadToClassToDropDown();
                // Drp_SelectType.Enabled = true;
                //Btn_GenerateList.Enabled = false;
            }
            else
            {
                Pnl_StudentsList.Visible = false;
            }
        }

        protected void Grd_StudentsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Txt_CancelReason.Text = "";
            MPE_ChangePromotion.Show();
        }

        protected void Btn_Change_Click(object sender, EventArgs e)
        {
            int _PromotionId = int.Parse(Grd_StudentsList.SelectedRow.Cells[0].Text);
            int _StudentId = int.Parse(Grd_StudentsList.SelectedRow.Cells[1].Text);
            int IsEligible = int.Parse(Grd_StudentsList.SelectedRow.Cells[10].Text);
            string _Remarks = Txt_CancelReason.Text;
            string sql = "UPDATE tblpromotionlist set";

            if (IsEligible == 0)
            {
                sql = sql + " tblpromotionlist.IsEligible=1,Link='Make Non-Eligible'";
            }
            else
            {
                sql = sql + " tblpromotionlist.IsEligible=0,Link='Make Eligible'";
            }
            sql = sql + ", tblpromotionlist.Remarks='" + _Remarks + "' where tblpromotionlist.Id=" + _PromotionId + " and tblpromotionlist.StudentId=" + _StudentId;
            MyConfiMang.m_MysqlDb.ExecuteQuery(sql);

            LoadStudentsListOfSelectedType();
            UpdatePromotionList();
        }

        protected void Btn_Update_Click(object sender, EventArgs e)
        {
            UpdatePromotionList();
            LoadStudentsListOfSelectedType();
        }

        private void UpdatePromotionList()
        {
            foreach (GridViewRow gv in Grd_StudentsList.Rows)
            {
                int _ToClassId = 0;
                int _ListId = int.Parse(gv.Cells[0].Text);
                int _StudentId = int.Parse(gv.Cells[1].Text);
                DropDownList Drp_ToClass = (DropDownList)gv.FindControl("Drp_ToClass");
                _ToClassId = int.Parse(Drp_ToClass.SelectedValue);

                string sql = "update tblpromotionlist set tblpromotionlist.ToClassId=" + _ToClassId + " where tblpromotionlist.Id=" + _ListId + " and tblpromotionlist.StudentId=" + _StudentId;
                MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            }
        }

        private bool CheckValidMarks()
        {
            bool _valid = true;

            string sql1 = "SELECT tblstudentmark.TotalMark from tblstudentmark where tblstudentmark.ExamSchId in(SELECT DISTINCT(tblexamschedule.Id)  FROM tblexamschedule inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId where  tblclassexam.ClassId=" + Drp_Class.SelectedValue + " AND tblexamschedule.BatchId=" + MyUser.CurrentBatchId + ") ";
            m_MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql1);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    if (m_MyReader.GetValue(0).ToString() == "")
                    {
                        _valid = false;
                        break;
                    }
                }
            }
            return _valid;
        }

        protected void Btn_GenerateList_Click(object sender, EventArgs e)
        {
            LoadToClassSelectionDropDown();
            MPE_TOClassSelection.Show();
        }

        private void LoadToClassSelectionDropDown()
        {
            OdbcDataReader MyReaderClass = null;
            Drp_ToClassSelect.Items.Clear();
            int[] _ToClassArray = new int[6];
            int _TotalToClasses = 0;

            string sql = "Select tblclass.Id,tblclass.ClassName from tblclass WHERE tblclass.Status=1 and tblclass.Standard IN (select tblpromotionmap.StdTo from tblpromotionmap inner join tblclass on tblclass.Standard=tblpromotionmap.StdFrom where tblclass.Id=" + int.Parse(Drp_Class.SelectedValue.ToString()) + ")";
            MyReaderClass = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReaderClass.HasRows)
            {
                _TotalToClasses = MyReaderClass.RecordsAffected;
                Btn_ToClassSave.Enabled = true;
                int i = 0;
                while (MyReaderClass.Read())
                {
                    ListItem li = new ListItem(MyReaderClass.GetValue(1).ToString(), MyReaderClass.GetValue(0).ToString());
                    Drp_ToClassSelect.Items.Add(li);

                    _ToClassArray[i] = int.Parse(MyReaderClass.GetValue(0).ToString());
                    i++;
                }
            }
            else
            {
                Btn_ToClassSave.Enabled = false;
                ListItem li = new ListItem("No class present", "-1");
                Drp_ToClassSelect.Items.Add(li);
            }
            ViewState["TotalClassCount"] = _TotalToClasses;
            ViewState["ToClassArray"] = _ToClassArray;
        }

        private void InsertAllStudentsIntoPromotionListTable(DataSet _StudentPromotion, int _ToClassId)
        {
            int _ToClassCount = (int)ViewState["TotalClassCount"];
            int _ToClassIdFinal = 0;
            int[] _ToClassArray = (int[])ViewState["ToClassArray"];
            string sql = "";
            int i = 0, _ToClassIdRandom = 0;
            foreach (DataRow dr in _StudentPromotion.Tables[0].Rows)
            {
                if (dr[6].ToString() == "0")
                {
                    _ToClassIdFinal = int.Parse(Drp_Class.SelectedValue);
                }
                else
                {
                    if (Hdn_ToClassIdType.Value == "0")
                    {
                        _ToClassIdFinal = _ToClassId;
                    }
                    else
                    {
                        if (i == _ToClassCount)
                        {
                            i = 0;
                        }
                        int.TryParse(_ToClassArray[i].ToString(), out _ToClassIdRandom);
                        i++;
                        _ToClassIdFinal = _ToClassIdRandom;
                    }
                }
                sql = "INSERT into tblpromotionlist(StudentId,StudentName,RollNo,AdmissionNo,ClassId,BatchId,IsEligible,Remarks,ToBatchId,Link,ToClassId) VALUES(" + int.Parse(dr[0].ToString()) + ",'" + dr[1].ToString() + "'," + int.Parse(dr[2].ToString()) + ",'" + dr[5].ToString() + "'," + int.Parse(dr[3].ToString()) + "," + int.Parse(dr[4].ToString()) + "," + int.Parse(dr[6].ToString()) + ",'" + dr[7].ToString() + "'," + dr[8].ToString() + ",'" + dr[9].ToString() + "'," + _ToClassIdFinal + ")";
                MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            }
        }

        protected void Btn_ToClassSave_Click(object sender, EventArgs e)
        {
            Hdn_SelectedToClass.Value = Drp_ToClassSelect.SelectedValue;
            Hdn_ToClassIdType.Value = Rdb_ToClassType.SelectedValue;
            int _ToClassId = 0;
            int.TryParse(Hdn_SelectedToClass.Value, out _ToClassId);
            DataSet _StudentPromotion = new DataSet();
            GenerateStudentsListBasedOnPromotionRules(out _StudentPromotion);
            if (_StudentPromotion.Tables[0].Rows.Count > 0)
            {
                InsertAllStudentsIntoPromotionListTable(_StudentPromotion, _ToClassId);
                Lbl_note.Text = "";
            }
            else
            {
                Lbl_note.Text = "No students in the selected class";
            }
            LoadPromotionListStatusGrid();
            LoadStudentsListOfSelectedType();
            LoadGenerateButtonStatus();
        }

        protected void Btn_PromotionExcel_Click(object sender, EventArgs e)
        {
            int _BatchId = 0, _ToBatchId = 0;
            if (Drp_SelectType.SelectedValue == "1")
            {
                _BatchId = (MyUser.CurrentBatchId - 1);
                _ToBatchId = MyUser.CurrentBatchId;
            }
            else
            {
                _BatchId = MyUser.CurrentBatchId;
                _ToBatchId = (MyUser.CurrentBatchId + 1);
            }

            DataSet _MyDatasetAfterPromo = null;
            string _ReportName = "Promotion List - " + Drp_Class.SelectedItem.Text + " (" + Drp_SelectType.SelectedItem.Text + ")";
            string FileName = "PromotionList";
            string sql = "select tblpromotionlist.StudentName, tblpromotionlist.RollNo, tblpromotionlist.AdmissionNo, tbloptionmaster.`Option` as EligibleForPromotion, tblclass.ClassName as ToClassName, tblpromotionlist.Remarks from tblpromotionlist inner join tbloptionmaster on tblpromotionlist.IsEligible = tbloptionmaster.Id inner join tblclass on tblpromotionlist.ToClassId= tblclass.Id where tblpromotionlist.BatchId=" + _BatchId + " and tblpromotionlist.ClassId=" + Drp_Class.SelectedValue + " order by tblpromotionlist.StudentName";
            _MyDatasetAfterPromo = MyConfiMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_MyDatasetAfterPromo.Tables[0].Rows.Count > 0)
            {
                if (!WinEr.ExcelUtility.ExportDataToExcel(_MyDatasetAfterPromo, _ReportName, FileName, MyUser.ExcelHeader))
                {
                    WC_MessageBox.ShowMssage("MS Excel is missing. Please install");
                }
            }
        }

        protected void Btn_ClearPromotion_Click(object sender, EventArgs e)
        {
            int _BatchId = MyUser.CurrentBatchId;
            if (Drp_SelectType.SelectedValue == "1")
            {
                _BatchId = (MyUser.CurrentBatchId - 1);
            }

            string sql = "delete from tblpromotionlist where tblpromotionlist.BatchId=" + _BatchId + " and tblpromotionlist.ClassId=" + Drp_Class.SelectedValue;
            MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            
            LoadPromotionListStatusGrid();
            LoadStudentsListOfSelectedType();
            LoadGenerateButtonStatus();
        }

        #endregion



        #region PROMOTION PREVIEW AREA FUNCTIONS

        private void LoadPreviewBatchToDropDown()
        {
            Drp_SelectBatchType.Items.Clear();

            string sql = "select tblbatch.BatchName, tblbatch.Id from tblbatch where tblbatch.Id in(" + MyUser.CurrentBatchId + "," + (MyUser.CurrentBatchId+1) + ")  order by tblbatch.Id";
            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_SelectBatchType.Items.Add(li);
                }
            }
        }       

        private void LoadAllClassToPreviewDropDown()
        {
            Drp_PreviewClass.Items.Clear();
            string sql = "select tblclass.Id,tblclass.ClassName from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id,tblclass.ClassName";
            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_Preview.Enabled = true;
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_PreviewClass.Items.Add(li);
                }
            }
            else
            {
                Btn_Preview.Enabled = false;
            }
        }

        protected void Btn_Preview_Click(object sender, EventArgs e)
        {
            DataSet _MyDatasetAfterPromo = null;
            int _ClassId = int.Parse(Drp_PreviewClass.SelectedValue);
            string sql = "SELECT tblstudent.StudentName, tblstudent.AdmitionNo as AdmissionNo, tblstudent.Address, tblstudent.Sex, DATE_FORMAT( tblstudent.DOB,'%d/%m/%Y') as DOB from tblstudent WHERE tblstudent.`Status`=1 AND tblstudent.Id in(select tblstudentclassmap.StudentId from tblstudentclassmap where tblstudentclassmap.BatchId=" + Drp_SelectBatchType.SelectedValue + " and tblstudentclassmap.ClassId=" + _ClassId + " union select tblpromotionlist.StudentId from tblpromotionlist where tblpromotionlist.ToBatchId=" + Drp_SelectBatchType.SelectedValue + " and tblpromotionlist.ToClassId=" + _ClassId + ") order by tblstudent.StudentName";
            _MyDatasetAfterPromo = MyConfiMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_MyDatasetAfterPromo.Tables[0].Rows.Count > 0)
            {
                Grd_Preview.DataSource = _MyDatasetAfterPromo;
                Grd_Preview.DataBind();
                lbl_PreviewNote.Text = "";
            }
            else
            {
                Grd_Preview.DataSource = null;
                Grd_Preview.DataBind();
                lbl_PreviewNote.Text = "No Students for this class..";
            }
        }

        protected void Btn_PreviewExcel_Click(object sender, EventArgs e)
        {
            int _BatchId = 0, _ToBatchId = 0;
            if (Drp_SelectBatchType.SelectedValue == "1")
            {
                _BatchId = (MyUser.CurrentBatchId - 1);
                _ToBatchId = MyUser.CurrentBatchId;
            }
            else
            {
                _BatchId = MyUser.CurrentBatchId;
                _ToBatchId = (MyUser.CurrentBatchId + 1);
            }

            DataSet _MyDatasetAfterPromo = null;
            int _ClassId = int.Parse(Drp_PreviewClass.SelectedValue);
            string _ReportName = Drp_PreviewClass.SelectedItem.Text + " (" + Drp_SelectBatchType.SelectedItem.Text + ")";
            string FileName = "PromotionPreview";
            string sql = "SELECT tblstudent.StudentName, tblstudent.AdmitionNo as AdmissionNo, tblstudent.Address, tblstudent.Sex, DATE_FORMAT( tblstudent.DOB,'%d/%m/%Y') as DOB from tblstudent WHERE  tblstudent.`Status`=1 and tblstudent.Id in(select tblstudentclassmap.StudentId from tblstudentclassmap where tblstudentclassmap.BatchId=" + Drp_SelectBatchType.SelectedValue + " and tblstudentclassmap.ClassId=" + _ClassId + " union select tblpromotionlist.StudentId from tblpromotionlist where tblpromotionlist.ToBatchId=" + Drp_SelectBatchType.SelectedValue + " and tblpromotionlist.ToClassId=" + _ClassId + ") order by tblstudent.StudentName";
            _MyDatasetAfterPromo = MyConfiMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_MyDatasetAfterPromo.Tables[0].Rows.Count > 0)
            {
                if (!WinEr.ExcelUtility.ExportDataToExcel(_MyDatasetAfterPromo, _ReportName, FileName, MyUser.ExcelHeader))
                {
                    WC_MessageBox.ShowMssage("MS Excel is missing. Please install");
                }
            }
            else
            {
                lbl_PreviewNote.Text = "No Students for this class..";
            }
        }

        #endregion

    


        #region PROMOTION AREA FUNCTIONS

        private void LoadPromotionListStatusGrid()
        {
            int _BatchId = 0, _ToBatchId = 0;
            if (Drp_SelectType2.SelectedValue == "1")
            {
                _BatchId = (MyUser.CurrentBatchId - 1);
                _ToBatchId = MyUser.CurrentBatchId;
            }
            else
            {
                _BatchId = MyUser.CurrentBatchId;
                _ToBatchId = (MyUser.CurrentBatchId + 1);
            }

            DataSet ClassDataSet = new DataSet();
            DataSet PromotionDataSet = new DataSet();
            DataTable dt;
            DataRow dr;

            PromotionDataSet.Tables.Add(new DataTable("PromotionStatus"));
            dt = PromotionDataSet.Tables["PromotionStatus"];
            dt.Columns.Add("Id");
            dt.Columns.Add("ClassName");
            dt.Columns.Add("Status");
            dt.Columns.Add("PendingStudents");
            dt.Columns.Add("PromotionCount");

            //string sql = "SELECT tblclass.Id,tblclass.ClassName, (select count( tblstudentclassmap.StudentId) from tblstudentclassmap where tblstudentclassmap.ClassId= tblclass.Id and tblstudentclassmap.BatchId="+_BatchId+" AND tblstudentclassmap.StudentId not in (select tblpromotionlist.StudentId from tblpromotionlist)) as Students  from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id,tblclass.ClassName";
            //string sql = "SELECT tblclass.Id,tblclass.ClassName, (select count( tblview_studentclassmap.StudentId) from tblview_studentclassmap where tblview_studentclassmap.ClassId= tblclass.Id and tblview_studentclassmap.BatchId=" + _BatchId + " AND tblview_studentclassmap.StudentId not in (select tblpromotionlist.StudentId from tblpromotionlist union select tblstudentclassmap.StudentId from tblstudentclassmap where tblstudentclassmap.BatchId=" + _ToBatchId + ")) as Students  from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id,tblclass.ClassName";

            string sql = "SELECT tblclass.Id,tblclass.ClassName, (select count( tblview_studentclassmap.StudentId) from tblview_studentclassmap inner join tblstudent on tblview_studentclassmap.StudentId= tblstudent.Id where tblview_studentclassmap.ClassId= tblclass.Id and tblview_studentclassmap.BatchId=" + _BatchId + " AND tblview_studentclassmap.StudentId not in (select tblpromotionlist.StudentId from tblpromotionlist union select tblstudentclassmap.StudentId from tblstudentclassmap where tblstudentclassmap.BatchId=" + _ToBatchId + ")) as Students, (select count(tblpromotionlist.StudentId) from tblpromotionlist where tblpromotionlist.ClassId= tblclass.Id and tblpromotionlist.ToBatchId=" + _ToBatchId + ") as Promotion  from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id,tblclass.ClassName";
            ClassDataSet = MyConfiMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ClassDataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drClass in ClassDataSet.Tables[0].Rows)
                {
                    int _Count = 0;
                    int.TryParse(drClass[2].ToString(), out _Count);
                    dr = dt.NewRow();
                    dr["Id"] = drClass[0].ToString();
                    dr["ClassName"] = drClass[1].ToString();
                    dr["PendingStudents"] = _Count.ToString();
                    dr["PromotionCount"] = drClass[3].ToString();

                    if (_Count == 0 && drClass[3].ToString()=="0")
                    {
                        dr["Status"] = "No students";                        
                    }
                    else if (_Count > 0)
                    {
                        dr["Status"] = "Promotion-List to be generated";
                    }
                    else
                    {
                        dr["Status"] = "Promotion-List generated";
                    }
                    dt.Rows.Add(dr);
                }

                if (PromotionDataSet.Tables[0].Rows.Count > 0)
                {
                    int _Count = PromotionDataSet.Tables[0].Rows.Count;
                    Pnl_PromotionStatus.Visible = true;
                    Grd_PromotionStatus.Columns[5].Visible = true;
                    Grd_PromotionStatus.DataSource = PromotionDataSet;
                    Grd_PromotionStatus.DataBind();
                    Grd_PromotionStatus.Columns[5].Visible = false;

                    ColorGridRowsBasedOnPromotionList(_Count);
                }
                else
                {
                    Pnl_PromotionStatus.Visible = false;
                }
            }
        }

        private void ColorGridRowsBasedOnPromotionList(int _GridRowCount)
        {
            int _PendingCount = 0, _PromotionCount = 0;
            for (int i = 0; i < _GridRowCount; i++)
            {
                CheckBox Chk_Select = (CheckBox)Grd_PromotionStatus.Rows[i].FindControl("Chk_Select");

                int.TryParse(Grd_PromotionStatus.Rows[i].Cells[3].Text, out _PendingCount);
                int.TryParse(Grd_PromotionStatus.Rows[i].Cells[4].Text, out _PromotionCount);
                if (_PendingCount == 0 && _PromotionCount > 0)
                {
                    Grd_PromotionStatus.Rows[i].BackColor = Color.LightCyan;
                    Chk_Select.Enabled = true;
                }
                else
                {
                    Grd_PromotionStatus.Rows[i].BackColor = Color.MistyRose;
                    Chk_Select.Enabled = false;
                }
            }
        }
  
        protected void Btn_Promotion_Click(object sender, EventArgs e)
        {
            int _Count = 0;
            try
            {
                MyConfiMang.CreateTansationDb();
                int _Promoted = 0, _ClassId = 0;
                foreach (GridViewRow gv in Grd_PromotionStatus.Rows)
                {
                    CheckBox Chk_Select = (CheckBox)gv.FindControl("Chk_Select");
                    
                    if (Chk_Select.Checked == true)
                    {
                        int.TryParse(gv.Cells[5].Text.Trim(), out _ClassId);

                        PromoteStudentsOfSelectedClassToNextClass(_ClassId, ref _Promoted);
                        _Count++;
                    }                   
                }

                MyConfiMang.EndSucessTansationDb();
                if (_Count == 0)
                {
                    WC_MessageBox.ShowMssage("Please select any class..");
                }
                else if (_Promoted > 0)
                {
                    LoadPromotionListStatusGrid();
                    WC_MessageBox.ShowMssage("The selected Students are Promoted");
                }                
            }
            catch (Exception Error)
            {
                WC_MessageBox.ShowMssage(Error.Message);
                MyConfiMang.EndFailTansationDb();
            }
        }

        private void PromoteStudentsOfSelectedClassToNextClass(int _ClassId,ref int _Promoted)
        {
            OdbcDataReader MyReaderPromotion = null;
            string sql = "select tblpromotionlist.StudentId, tblpromotionlist.ToClassId, tblpromotionlist.ClassId, tblpromotionlist.IsEligible from tblpromotionlist where tblpromotionlist.ClassId=" + _ClassId + " and tblpromotionlist.ToBatchId=" + MyUser.CurrentBatchId;
            MyReaderPromotion = MyConfiMang.m_TransationDb.ExecuteQuery(sql);
            if (MyReaderPromotion.HasRows)
            {
                while (MyReaderPromotion.Read())
                {
                    int _IsEligible = 0;
                    int.TryParse(MyReaderPromotion.GetValue(3).ToString(), out _IsEligible);

                    if (int.Parse(MyReaderPromotion.GetValue(1).ToString()) == -1)
                    {
                        MyConfiMang.AddTONewBatch(int.Parse(MyReaderPromotion.GetValue(0).ToString()), MyUser.CurrentBatchId, int.Parse(MyReaderPromotion.GetValue(2).ToString()), 0);
                    }
                    else
                    {
                        if (_IsEligible == 1)//pass
                        {
                            MyConfiMang.AddTONewBatch(int.Parse(MyReaderPromotion.GetValue(0).ToString()), MyUser.CurrentBatchId, int.Parse(MyReaderPromotion.GetValue(1).ToString()), int.Parse(MyReaderPromotion.GetValue(2).ToString()));
                        }
                        else//fail
                        {
                            MyConfiMang.AddTONewBatch(int.Parse(MyReaderPromotion.GetValue(0).ToString()), MyUser.CurrentBatchId, int.Parse(MyReaderPromotion.GetValue(1).ToString()), 0);
                        }
                        _Promoted++;
                    }
                    DeleteEntryFromPromotionList(int.Parse(MyReaderPromotion.GetValue(0).ToString()), MyReaderPromotion.GetValue(2).ToString());
                }
            }

            ScheduleRollNoForPossibleClassesToPromotion(_ClassId);
            //LoadStudentListOfSelectedClassToGrid();
        }

        private void ScheduleRollNoForPossibleClassesToPromotion(int _ClassId)
        {
            OdbcDataReader MyReaderRoll = null;
            string sql1 = "Select tblclass.Id from tblclass WHERE tblclass.Status=1 and tblclass.Standard IN (select tblpromotionmap.StdTo from tblpromotionmap inner join tblclass on tblclass.Standard=tblpromotionmap.StdFrom where tblclass.Id=" + _ClassId + " UNION select tblpromotionmap.StdFrom from tblpromotionmap inner join tblclass on tblclass.Standard=tblpromotionmap.StdFrom where tblclass.Id=" + _ClassId + ")";
            MyReaderRoll = MyConfiMang.m_TransationDb.ExecuteQuery(sql1);
            if (MyReaderRoll.HasRows)
            {
                while (MyReaderRoll.Read())
                {
                    MyConfiMang.ScheduleRollNumberNew(int.Parse(MyReaderRoll.GetValue(0).ToString()), MyUser.CurrentBatchId);
                }
            }
        }

        private void DeleteEntryFromPromotionList(int _StudentId, string _ClassId)
        {
            string sql = "Delete from tblpromotionlist where tblpromotionlist.ClassId=" + _ClassId + " and tblpromotionlist.StudentId=" + _StudentId;
            MyConfiMang.m_TransationDb.ExecuteQuery(sql);
        }

        protected void Drp_SelectType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPromotionListStatusGrid();
            if (Drp_SelectType2.SelectedValue == "1")
            {
                Btn_Promotion.Visible = true;
            }
            else
            {
                Btn_Promotion.Visible = false;
            }
        }

        #endregion



        #region COMMON FUNCTIONS FOR THE PAGE

        private void LoadAllClassToDropDownList()
        {
            int _BatchId = MyUser.CurrentBatchId;
            if (Drp_SelectType.SelectedValue == "1")
            {
                _BatchId = (MyUser.CurrentBatchId - 1);
            }
            Drp_Class.Items.Clear();
            string sql = "select distinct(tblview_studentclassmap.ClassId), tblclass.ClassName from tblview_studentclassmap inner join tblclass on tblview_studentclassmap.ClassId= tblclass.Id where tblview_studentclassmap.BatchId=" + _BatchId + " order by tblview_studentclassmap.ClassId,tblclass.ClassName";
            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_GenerateList.Enabled = true;
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Class present", "-1");
                Drp_Class.Items.Add(li);
                Btn_GenerateList.Enabled = false;
            }
            Drp_Class.SelectedIndex = 0;
        }

        private void LoadBatchToDropDown()
        {
            int _PreviousBatch = (MyUser.CurrentBatchId - 1);
            int _NextBatch = (MyUser.CurrentBatchId + 1);
            Drp_SelectType.Items.Clear();
            Drp_SelectType2.Items.Clear();

            string _CurrentBatch = MyUser.CurrentBatchName;
            string sql = "select distinct(select tblbatch.BatchName from tblbatch where tblbatch.Id=" + _PreviousBatch + ") as PreviousBatch,( select tblbatch.BatchName from tblbatch where tblbatch.Id=" + _NextBatch + ") as NextBatch from tblbatch";
            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem(MyReader.GetValue(0).ToString() + " TO " + _CurrentBatch, "1");
                Drp_SelectType.Items.Add(li);
                Drp_SelectType2.Items.Add(li);
                li = new ListItem(_CurrentBatch + " TO " + MyReader.GetValue(1).ToString(), "2");
                Drp_SelectType.Items.Add(li);
                Drp_SelectType2.Items.Add(li);
            }
        }

        private void LoadGenerateButtonStatus()
        {
            OdbcDataReader MyReaderGenerate = null;
            int _BatchId = 0, _ToBatchId = 0, _Pending = 0;
            if (Drp_SelectType.SelectedValue == "1")
            {
                _BatchId = (MyUser.CurrentBatchId - 1);
                _ToBatchId = MyUser.CurrentBatchId;
            }
            else
            {
                _BatchId = MyUser.CurrentBatchId;
                _ToBatchId = (MyUser.CurrentBatchId + 1);
            }

            string sql = "select count( tblview_studentclassmap.StudentId) from tblview_studentclassmap inner join tblstudent on tblview_studentclassmap.StudentId= tblstudent.Id where  tblstudent.`Status`=1 and tblview_studentclassmap.ClassId=" + Drp_Class.SelectedValue + " and tblview_studentclassmap.BatchId=" + _BatchId + " AND tblview_studentclassmap.StudentId not in (select tblpromotionlist.StudentId from tblpromotionlist union select tblstudentclassmap.StudentId from tblstudentclassmap where tblstudentclassmap.BatchId=" + _ToBatchId + ")";
            MyReaderGenerate = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReaderGenerate.HasRows)
            {
                int.TryParse(MyReaderGenerate.GetValue(0).ToString(), out _Pending);
            }
            if (_Pending > 0)
            {
                Btn_GenerateList.Enabled = true;
            }
            else
            {
                Btn_GenerateList.Enabled = false;
            }
        }

        #endregion

       
    }
}
