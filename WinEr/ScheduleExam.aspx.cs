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

public partial class SheduleExam : System.Web.UI.Page
{
    private ExamManage MyExamMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    private OdbcDataReader MyReader1 = null;
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
        if (MyExamMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }
        else if (!MyUser.HaveActionRignt(6))
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
                //some initlization   
                LoadExamGeneralDetails();
                LoadClassAndPerdiod();
                SelectClassAndPeriod();
                LoadGradeMaster();
                LoadScheduleDetails();
               
            }
        }
    }
    private void LoadGradeMaster()
    {
        DrpGradeMaster.Items.Clear();
        string sql = " select tblgrademaster.Id, tblgrademaster.GradeName from tblgrademaster  ";
        DataSet DT = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (DT.Tables[0].Rows.Count > 0)
        {


            foreach (DataRow Dr in DT.Tables[0].Rows)
            {
                ListItem Li = new ListItem(Dr[1].ToString(), Dr[0].ToString());
                DrpGradeMaster.Items.Add(Li);
            }
        }
        else
        {
            ListItem Li = new ListItem("No masters found", "-1");
            DrpGradeMaster.Items.Add(Li);
        }

        
    }

    private void LoadScheduleDetails()
    {
       
        lnk_assign.Visible = false;
        Lbl_note.Text = "";
        if (DrpClassName.SelectedValue=="-1")
        {
            Lbl_note.Text = "This Exam not Mapped to Any Class";
            if (MyUser.HaveActionRignt(76))
            {
                lnk_assign.Visible = true;
            }
            else
            {
                lnk_assign.Visible = false;
            }
            btn_UpdateSchdule.Enabled = false;
            Btn_ScheduleExam.Enabled = false;
            Btn_clear.Enabled = false;
         
        }
        else if (MyExamMang.ExamSchduled(int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue))
        {
            LoadSchedule();
        }
        else
        {
            ScheduleExam();
        }
       // LoadDetails();
    }

    private void ScheduleExam()
    {
        if (AddSubjectsToGrid())
        {
            Btn_exporttoexel.Enabled = false;
            FillTimeSlot();
            GetClasExamId();
            Btn_ScheduleExam.Enabled = true;
            btn_UpdateSchdule.Enabled = false;
            Btn_clear.Enabled = false;

         
           
        }

    }

    private void FillTimeSlot()
    {
        foreach (GridViewRow gv in Grd_ExamSchdule.Rows)
        {
           
            TextBox TxExamdate = (TextBox)gv.FindControl("Txt_ExamDate");
            TxExamdate.Enabled = true;
            DropDownList DrpTimeSlot = (DropDownList)gv.FindControl("Drp_TimeSlot");
            string sql1 = "SELECT tbltimeslot.Id, tbltimeslot.Session, tbltimeslot.StartTime, tbltimeslot.EndTime FROM tbltimeslot";
            MyReader1 = MyExamMang.m_MysqlDb.ExecuteQuery(sql1);
            if (MyReader1.HasRows)
            {
                DrpTimeSlot.Items.Clear();
                while (MyReader1.Read())
                {
                    ListItem li = new ListItem(MyReader1.GetValue(1).ToString() + " " + MyReader1.GetValue(2).ToString() + "-" + MyReader1.GetValue(3).ToString(), MyReader1.GetValue(0).ToString());
                    DrpTimeSlot.Items.Add(li);
                }
            }
        }
    }

    private void LoadSchedule()
    {
        if (AddSubjectsToGrid())
        {
            Btn_exporttoexel.Enabled = true;
            FillTimeSlot();
            GetClasExamId();
            SetScheduleDetails();
            
            Btn_ScheduleExam.Enabled = false;
            btn_UpdateSchdule.Enabled = true;
            setGradeMaster();
            btn_UpdateSchdule.Text = "Edit";
            //Btn_clear.Enabled = true;

            //mani updated on 26.12.2013
         
        }
    }

   

    private void setGradeMaster()
    {
        string sql = " select distinct  tblexamschedule.GradeMasterId from tblexamschedule where tblexamschedule.ClassExamId=" + int.Parse(Txt_CXmId.Text) + " AND tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblexamschedule.PeriodId=" + DrpExamPeriod.SelectedValue ;
        MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            DrpGradeMaster.SelectedValue = MyReader.GetValue(0).ToString();
        }
    }

    private void SetScheduleDetails()
    {
        string sql = "SELECT tblexammark.ExamDate, tblexammark.TimeSlotId,SubjectOrder from tblexammark INNER JOIN tblexamschedule on tblexammark.ExamSchId=tblexamschedule.Id where tblexamschedule.ClassExamId=" + int.Parse(Txt_CXmId.Text) + " AND tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " and tblexamschedule.PeriodId=" + DrpExamPeriod.SelectedValue + "";
        MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
      
        if (MyReader.HasRows)
        {
            foreach (GridViewRow gv in Grd_ExamSchdule.Rows)
            {
                MyReader.Read();
                TextBox TxExamdate = (TextBox)gv.FindControl("Txt_ExamDate");
                DateTime _dt = DateTime.Parse(MyReader.GetValue(0).ToString());
                //DateTime _dt = MyUser.GetDareFromText(MyReader.GetValue(0).ToString());

                //TxExamdate.Text = _dt.Date.ToString("dd/MM/yyyy");
                TxExamdate.Text = _dt.Date.Day+"/"+_dt.Date.Month+"/"+_dt.Date.Year;
               
                TxExamdate.Enabled = false;
                DropDownList DrpTimeSlot = (DropDownList)gv.FindControl("Drp_TimeSlot");
                DrpTimeSlot.SelectedValue = MyReader.GetValue(1).ToString();
                DrpTimeSlot.Enabled = false;
                TextBox TxExamorder = (TextBox)gv.FindControl("txt_SubjectOrder");
                TxExamorder.Text = MyReader.GetValue(2).ToString();

                TxExamorder.Enabled = false;
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
    }

    private void LoadExamGeneralDetails()
    {
        string ExamNme, ExamType, examfreqncy;
        MyExamMang.GetExamDetail(int.Parse(Session["ExamId"].ToString()), out ExamNme, out ExamType, out examfreqncy);
        Lbl_ExamName.Text = ExamNme.ToString();
        Lbl_ExamType.Text = ExamType.ToString();
        Lbl_Frequency.Text = examfreqncy.ToString();
    }


    protected void DrpClassName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["ClassId"] = DrpClassName.SelectedValue;
        LoadScheduleDetails();
    }

    private void GetClasExamId()
    {
        string sql = "SELECT tblclassexam.Id FROM tblclassexam WHERE tblclassexam.ExamId=" + int.Parse(Session["ExamId"].ToString()) + " AND tblclassexam.ClassId=" + int.Parse(DrpClassName.SelectedValue);
        MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            Txt_CXmId.Text = MyReader.GetValue(0).ToString();
        }
    }

    private void EnableGrid()
    {
        foreach (GridViewRow gv in Grd_ExamSchdule.Rows)
        {
            TextBox TxExamdate = (TextBox)gv.FindControl("Txt_ExamDate");
            TxExamdate.Enabled = true;

            DropDownList DrpTimeSlot = (DropDownList)gv.FindControl("Drp_TimeSlot");
            DrpTimeSlot.Enabled = true;

            TextBox TxExamorder = (TextBox)gv.FindControl("txt_SubjectOrder");
            TxExamorder.Enabled = true;
        }
    }

    private void DisenableGrid()
    {
        foreach (GridViewRow gv in Grd_ExamSchdule.Rows)
        {
            TextBox TxExamdate = (TextBox)gv.FindControl("Txt_ExamDate");
            TxExamdate.Enabled = false;
            DropDownList DrpTimeSlot = (DropDownList)gv.FindControl("Drp_TimeSlot");
          
            DrpTimeSlot.Enabled = false;
          
        }
    }

    private bool AddSubjectsToGrid()
    {
        bool _valid = false;
        string sql = "SELECT tblsubjects.Id, tblsubjects.subject_name, tblsubjects.SubjectCode FROM tblsubjects INNER JOIN tblclassexamsubmap ON tblsubjects.Id=tblclassexamsubmap.SubId INNER JOIN tblclassexam ON tblclassexamsubmap.ClassExamId=tblclassexam.Id WHERE tblclassexam.ClassId=" + int.Parse(DrpClassName.SelectedValue) + " AND tblclassexam.ExamId=" + int.Parse(Session["ExamId"].ToString());
        
        MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            Grd_ExamSchdule.Columns[0].Visible = true;
            Grd_ExamSchdule.DataSource = MyReader;
            Grd_ExamSchdule.DataBind();
            Grd_ExamSchdule.Columns[0].Visible = false;
            _valid = true;
        }
        return _valid;
    }

    protected void Btn_clear_Click(object sender, EventArgs e)
    {
        FillTimeSlot();
        
        SetScheduleDetails();
        btn_UpdateSchdule.Text = "Edit";
        Btn_clear.Enabled = false;
    }

   


    



    protected void Btn_ScheduleExam_Click(object sender, EventArgs e)
    {
        string _Message = "";
        if (valideSchedule(out _Message))
        {
            int _ExSchId;
            DateTime _ScheduledDate = GetFirstDate();
         
            _ExSchId = MyExamMang.ScheduleExamMaster(int.Parse(Txt_CXmId.Text), MyUser.CurrentBatchId, int.Parse(DrpExamPeriod.SelectedValue), int.Parse(DrpGradeMaster.SelectedValue.ToString()), _ScheduledDate);
            if (_ExSchId != -1)
            {
                AddScheduledSubjects(_ExSchId);
                _Message = "Exam is Scheduled";
                MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Schedule Exam", "Exam " + Lbl_ExamName.Text.ToString() + " is scheduled", 1,1);
                Btn_ScheduleExam.Enabled = false;
                Btn_exporttoexel.Enabled = true;
                btn_UpdateSchdule.Enabled = true;
                // Btn_clear.Text = "Undo";
                DisenableGrid();
            }
            else
            {
                _Message = "Error While Scheduling";
            }
        }
        
        WC_MessageBox.ShowMssage(_Message);
        
    }

    private DateTime GetFirstDate()
    {
        DateTime _Date = new DateTime(), comparedate = new DateTime();
        bool _first = true; 
        foreach (GridViewRow gv in Grd_ExamSchdule.Rows)
        {
            TextBox TxExamdate = (TextBox)gv.FindControl("Txt_ExamDate");
            DateTime.TryParse(TxExamdate.Text, out _Date);
            if (_first)
            {
                comparedate = _Date;
                _first = false;
            }
            if (_Date < comparedate)
            {
                comparedate = _Date;
            }

        }
        return comparedate;
    }

    private void AddScheduledSubjects(int _ExSchId)
    {
        int _markcount = 1;
        string _columnname,_failedcolumnname;
        int SubOrder = 0;
        foreach (GridViewRow gv in Grd_ExamSchdule.Rows)
        {
            TextBox TxExamdate = (TextBox)gv.FindControl("Txt_ExamDate");
        
            _columnname = "Mark" + _markcount.ToString();
            _markcount = _markcount + 1;
            _failedcolumnname = "Mark" + _markcount.ToString();
            DropDownList drptime = (DropDownList)gv.FindControl("Drp_TimeSlot");
            TextBox txtSubOrder = (TextBox)gv.FindControl("txt_SubjectOrder");
            int.TryParse(txtSubOrder.Text, out SubOrder);
            MyExamMang.ScheduleSubjects(_ExSchId, _columnname,_failedcolumnname, int.Parse(gv.Cells[0].Text.ToString()), int.Parse(drptime.SelectedValue), MyUser.GetDareFromText(TxExamdate.Text), SubOrder);
            //MyExamMang.ScheduleSubjects(_ExSchId, _columnname, int.Parse(gv.Cells[0].Text.ToString()), int.Parse(drptime.SelectedValue), MyUser.GetDareFromText(TxExamdate.Text), SubOrder);
            _markcount++;

        }
    }

    private bool valideSchedule(out string _Message)
    {
        bool _valid = true;
        _Message = "";
      
           

        foreach (GridViewRow gv in Grd_ExamSchdule.Rows)
        {
            TextBox TxExamdate = (TextBox)gv.FindControl("Txt_ExamDate");
            DateTime _dt;
            if (TxExamdate.Text.Trim() == "")
            {
                _Message = "Date Cannot be Empty";
                _valid = false;
                break;
            }
            else if (!MyUser.TryGetDareFromText(TxExamdate.Text.ToString(), out _dt))
            {
                _Message = "Please enter a valid date";
                _valid = false;
                break;
            }            
        }
        return _valid;
    }

    protected void btn_UpdateSchdule_Click(object sender, EventArgs e)
    {
        if (btn_UpdateSchdule.Text == "Edit")
        {
            EnableGrid();
            
          
            Btn_clear.Enabled = true;
            btn_UpdateSchdule.Text = "Update";
        }
        else
        {
            string _Message = "";
            if (valideSchedule(out _Message))
            {
                Btn_clear.Enabled = false;
                UpdateExamScheduled();
                DisenableGrid();
                btn_UpdateSchdule.Text = "Edit";
                _Message = "Exam timetable is updated";
                MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Update Exam Schedule", "One Exam " + Lbl_ExamName.Text.ToString() + " Schedule details are updated", 1,1);
            }
            
            WC_MessageBox.ShowMssage(_Message);
            
        }
    }

    private void UpdateExamScheduled()
    {
        string _Message = "";
        if (valideSchedule(out _Message))
        {
            int _ExSchId;
            _ExSchId = MyExamMang.GetExamScheduleId(int.Parse(Session["ExamId"].ToString()),MyUser.CurrentBatchId,DrpClassName.SelectedValue,DrpExamPeriod.SelectedValue);
            if (_ExSchId != -1)
            {
              
                UpdateScheduledSubjects(_ExSchId);
                UpdateGradeMasterId(_ExSchId,DrpGradeMaster.SelectedValue);
            }
        }
        else
        {
            WC_MessageBox.ShowMssage(_Message);
            
        }
    }

   

    private void UpdateGradeMasterId(int _ExSchId, string _GradeId)
    {
        string sql = "update tblexamschedule set tblexamschedule.GradeMasterId=" + _GradeId+" where tblexamschedule.id=" + _ExSchId;
        MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
    }

    private void UpdateScheduledSubjects(int _ExSchId)
    {
        int SubjectOrder = 0;
        foreach (GridViewRow gv in Grd_ExamSchdule.Rows)
        {
            TextBox TxExamdate = (TextBox)gv.FindControl("Txt_ExamDate");
            TextBox TxtBox = (TextBox)gv.FindControl("txt_SubjectOrder");
            int.TryParse(TxtBox.Text, out  SubjectOrder);
            DropDownList drptime = (DropDownList)gv.FindControl("Drp_TimeSlot");
     //       MyExamMang.UpdateScheduleSubjects(_ExSchId, int.Parse(gv.Cells[0].Text.ToString()), int.Parse(drptime.SelectedValue), DateTime.Parse(TxExamdate.Text));
            MyExamMang.UpdateScheduleSubjects(_ExSchId, int.Parse(gv.Cells[0].Text.ToString()), int.Parse(drptime.SelectedValue), MyUser.GetDareFromText(TxExamdate.Text), SubjectOrder);

        }
    }

    protected void DrpExamPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["PeriodId"] = DrpExamPeriod.SelectedValue;
        LoadScheduleDetails();
    }

    protected void Btn_exporttoexel_Click(object sender, EventArgs e)
    {
        string sql = "select tblsubjects.subject_name AS `Subject Name`, tblsubjects.SubjectCode AS `Subject Code`, date_format(tblexammark.ExamDate,'%d/%m/%Y') AS `Exam Date`, tbltimeslot.StartTime AS `Start Time`, tbltimeslot.EndTime AS `End Time`, tblclassexamsubmap.MinMark AS `Pass Mark`, tblclassexamsubmap.MaxMark AS `Maximum Mark` from tblclassexam inner join tblexamschedule on tblclassexam.Id= tblexamschedule.ClassExamId AND tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " AND tblexamschedule.PeriodId=" + DrpExamPeriod.SelectedValue + " inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblclassexam.Id inner join tblexammark on tblexammark.ExamSchId= tblexamschedule.Id AND tblexammark.SubjectId= tblclassexamsubmap.SubId inner join tbltimeslot on tbltimeslot.Id= tblexammark.TimeSlotId inner join tblsubjects on tblsubjects.Id= tblexammark.SubjectId where tblclassexam.ClassId=" + DrpClassName.SelectedValue + " AND tblclassexam.ExamId=" + Session["ExamId"].ToString() + " order by tblexammark.SubjectOrder";
        MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
            //if (!WinEr.ExcelUtility.ExportDataSetToExcel(MydataSet, "TimeTable_"+DrpExamPeriod.SelectedItem.Text+".xls"))
            //{
            //    Lbl_note.Text = "This function need Ms office";
            //}
            string FileName = "TimeTable_" + DrpExamPeriod.SelectedItem.Text;
            string _ReportName = "TimeTable_" + DrpExamPeriod.SelectedItem.Text;
            if (!WinEr.ExcelUtility.ExportDataToExcel(MydataSet, _ReportName, FileName, MyUser.ExcelHeader))
            {

            }
        }
    }
    protected void lnk_manageExam_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageClassExam.aspx");
    }

   

}