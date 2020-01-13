using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using WinBase;
using System.Data;

namespace WinEr
{
    public partial class ExitViolationReport : System.Web.UI.Page
    {
        private OdbcDataReader m_MyReader = null;
        private KnowinUser MyUser;
        private StudentManagerClass MyStudMang;
        private Attendance MyAttendance;
        private DataSet Exitviolationds = new DataSet();
        DataRow _dr;
        DataTable _dt;

        #region Events

            protected void Page_Load(object sender, EventArgs e)
            {
                if (Session["UserObj"] == null)
                {
                    Response.Redirect("sectionerr.htm");
                }
                MyUser = (KnowinUser)Session["UserObj"];
                MyAttendance = MyUser.GetAttendancetObj();
                MyStudMang = MyUser.GetStudentObj();
                if (MyStudMang == null)
                {
                    Response.Redirect("RoleErr.htm");
                    //no rights for this user.
                }
                else if (!MyUser.HaveActionRignt(840))
                {
                    Response.Redirect("RoleErr.htm");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        Btn_Excel.Enabled = false;
                        DateTime _date = System.DateTime.Today;
                        DateTime todate = _date.AddDays(-1);
                        string _todate = MyUser.GerFormatedDatVal(todate);
                        Txt_FromDate.Text = _todate;
                        Txt_ToDate.Text = _todate;
                        Txt_FromDate.Enabled = false;
                        Txt_ToDate.Enabled = false;
                        LoadClassToDropdown();
                    }
                }

            }

            protected void Drp_Period_SelectedIndexChanged(object sender, EventArgs e)
            {
                // lblerror.Text = "";
                string _sdate = null, _edate = null;
                if (Drp_Period.SelectedItem.Text.ToString() == "Last Week")
                {

                    DateTime _date = System.DateTime.Now;
                    DateTime todate = _date.AddDays(-1);
                    string _todate = MyUser.GerFormatedDatVal(todate);
                    DateTime sdate = _date.AddDays(-8);
                    _sdate = MyUser.GerFormatedDatVal(sdate);
                    Txt_FromDate.Text = _sdate;
                    Txt_ToDate.Text = _todate;
                }

                else if (Drp_Period.SelectedItem.Text.ToString() == "This Month")
                {
                    DateTime _date = System.DateTime.Now;
                    //DateTime _Endtime = _date.AddDays(-1);
                    _edate = MyUser.GerFormatedDatVal(_date);

                    DateTime _start = System.DateTime.Now;
                    _sdate = MyUser.GerFormatedDatVal(new DateTime(_start.Year, _start.Month, 1));
                    Txt_FromDate.Enabled = false;
                    Txt_ToDate.Enabled = false;
                    Txt_FromDate.Text = _sdate;
                    Txt_ToDate.Text = _edate;
                }
                else if (Drp_Period.SelectedItem.Text.ToString() == "Last Day")
                {
                    DateTime _date = System.DateTime.Now;
                    DateTime _Endtime = _date.AddDays(-1);
                    _edate = MyUser.GerFormatedDatVal(_Endtime);

                    DateTime _start = System.DateTime.Now;
                    Txt_FromDate.Enabled = false;
                    Txt_ToDate.Enabled = false;
                    Txt_FromDate.Text = _edate;
                    Txt_ToDate.Text = _edate;
                }
                else if (Drp_Period.SelectedItem.Text.ToString() == "Manual")
                {
                    Txt_FromDate.Enabled = true;
                    Txt_ToDate.Enabled = true;
                    Txt_FromDate.Text = "";
                    Txt_ToDate.Text = "";

                }
                string Msg = "";
                if (Drp_Period.SelectedItem.Text.ToString() != "Manual")
                {
                    if (RbLst_Type.SelectedItem.ToString() == "Student")
                    {


                        GenerateStudentExitViolationReport(out Msg);
                        Pnl_ShowReport.Visible = true;
                        Pnl_StaffReport.Visible = false;
                    }
                    else
                    {
                        GenerateStaffExitViolationReport(out Msg);
                        Pnl_ShowReport.Visible = false;
                        Pnl_StaffReport.Visible = true;
                    }
                }
            }

            protected void Txt_ToDate_TextChanged(object sender, EventArgs e)
            {
            }

            protected void Grd_ExitViolationReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                string Msg="";
                Grd_ExitViolationReport.PageIndex = e.NewPageIndex;
                GenerateStudentExitViolationReport(out Msg);
            }

            protected void Grd_StaffExitViolationReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                string Msg = "";
                Grd_StaffExitViolationReport.PageIndex = e.NewPageIndex;
                GenerateStaffExitViolationReport(out Msg);
            }


            protected void RbLst_Type_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (RbLst_Type.SelectedItem.ToString() == "Student")
                {
                    Drp_ClassName.Enabled = true;
                }
                else
                {
                    Drp_ClassName.Enabled = false;
                }
            }

            protected void Btn_Show_Click(object sender, EventArgs e)
            {

                string message = "";
                string Msg = "";
                Lbl_Err.Text = "";
                Lbl_report_err.Text = "";
                if (Txt_FromDate.Text == "" || Txt_ToDate.Text == "")
                {
                    message = "You must enter the start date and end date";
                    Grd_ExitViolationReport.DataSource = null;
                    Grd_ExitViolationReport.DataBind();
                }
                else
                {
                    DateTime _sdate = MyUser.GetDareFromText(Txt_FromDate.Text);
                    DateTime _edate = MyUser.GetDareFromText(Txt_ToDate.Text);
                    if (_edate.Date < _sdate.Date)
                    {
                        message = "End date must be greater than start date";
                        Grd_ExitViolationReport.DataSource = null;
                        Grd_ExitViolationReport.DataBind();
                    }

                    else if (Drp_Period.SelectedItem.ToString() == "Manual")
                    {

                        DateTime tdate = System.DateTime.Now.Date;
                        DateTime todate = General.GetDateTimeFromText(Txt_ToDate.Text);
                        if (todate == tdate || todate > tdate)
                        {
                            message = "You must enter the date less than todays date";
                        }
                    }
                }
                if (message == "")
                {
                    if (RbLst_Type.SelectedItem.ToString() == "Student")
                    {
                        

                        GenerateStudentExitViolationReport(out Msg);
                        Pnl_ShowReport.Visible = true;
                        Pnl_StaffReport.Visible = false;
                    }
                    else
                    {
                        GenerateStaffExitViolationReport(out Msg);
                        Pnl_ShowReport.Visible = false;
                        Pnl_StaffReport.Visible = true;
                    }
                }
                Lbl_Err.Text = message;
                Lbl_report_err.Text = Msg;
            }

            private void GenerateStaffExitViolationReport(out string Msg)
            {
                Msg = "";
                DateTime _startdate = MyUser.GetDareFromText(Txt_FromDate.Text);
                DateTime _enddate = MyUser.GetDareFromText(Txt_ToDate.Text);          
                DataSet StaffExitviolationDs = new DataSet();
                DataRow _dr;
                DataTable _dt;
                StaffExitviolationDs.Tables.Add("StaffExitViolation");
                _dt = StaffExitviolationDs.Tables["StaffExitViolation"];
                _dt.Columns.Add("SurName");
                _dt.Columns.Add("RoleName");
                                                                                                                                                                                                                                                                                                                                                      //date(t3.`MarkDate`) between '" + _startdate.ToString("s") + " ' and '" + _enddate.ToString("s") + "')
                string sql = "SELECT tbluser.SurName,tblrole.RoleName,tblstaffattendance.OutTime FROM tblstaffattendance INNER JOIN tbluser ON tbluser.Id=tblstaffattendance.StaffId INNER JOIN tblrole ON tblrole.Id=tbluser.RoleId WHERE tblstaffattendance.MarkStatus<>0 AND tblstaffattendance.MarkDate in (SELECT MAX(t3.`MarkDate`) FROM tblstaffattendance as t3 WHERE date(t3.`MarkDate`) between '" + _startdate.ToString("s") + " ' and '" + _enddate.ToString("s") + "')";
                DataSet StaffDs= MyAttendance.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (StaffDs != null && StaffDs.Tables[0].Rows.Count>0)
                {
                    foreach(DataRow dr in StaffDs.Tables[0].Rows)
                    {
                        
                        TimeSpan _tmspan = new TimeSpan(0);
                        TimeSpan _tmspannormal = new TimeSpan(0);
                        string _outtimestr = dr["OutTime"].ToString();
                        TimeSpan.TryParse(_outtimestr, out _tmspan);

                        if (_tmspan == _tmspannormal)
                        {
                            _dr = _dt.NewRow();
                            _dr["SurName"] = dr["SurName"];
                            _dr["RoleName"] = dr["RoleName"];
                            StaffExitviolationDs.Tables["StaffExitViolation"].Rows.Add(_dr);
                        }
                    }
                    if (StaffExitviolationDs != null && StaffExitviolationDs.Tables[0].Rows.Count > 0)
                    {

                        ViewState["ExitViolationList"] = StaffExitviolationDs;
                        Grd_StaffExitViolationReport.DataSource = StaffExitviolationDs;
                        Grd_StaffExitViolationReport.DataBind();
                        Btn_Excel.Enabled = true;
                        Lbl_Err.Text = "";
                        Grd_ExitViolationReport.Visible = false;
                        Grd_StaffExitViolationReport.Visible = true;
                    }
                    else
                    {
                        Btn_Excel.Enabled = false;
                        Grd_StaffExitViolationReport.DataSource = null;
                        Grd_StaffExitViolationReport.DataBind();
                        Msg = "No report found";
                    }
                }
            }

            protected void Btn_Excel_Click(object sender, EventArgs e)
            {
                string FileName;
                string _ReportName;
                DataSet ExitViolationExcel = (DataSet)ViewState["ExitViolationList"];
                if (RbLst_Type.SelectedItem.ToString() == "Student")
                {
                    FileName = "StudentExitViolationReport";
                    _ReportName = "StudentExitViolation Report";
                }
                else
                {
                    FileName = "StaffExitViolationReport";
                    _ReportName = "StaffExitViolation Report";
                }
                if (!WinEr.ExcelUtility.ExportDataToExcel(ExitViolationExcel, _ReportName, FileName, MyUser.ExcelHeader))
                {

                    Lbl_Err.Text = "This function need Ms office";
                }
            }


        #endregion

        #region Functions

            private void LoadClassToDropdown()
            {
                Drp_ClassName.Items.Clear();
                DataSet MyDataSet = new DataSet();
                MyDataSet = MyUser.MyAssociatedClass();
                if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
                {
                    Btn_Show.Enabled = true;
                    ListItem li = new ListItem("ALL", "0");
                    Drp_ClassName.Items.Add(li);

                    foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                    {
                        li = new ListItem(dr[1].ToString(), dr[0].ToString());
                        Drp_ClassName.Items.Add(li);
                    }
                }
                else
                {
                    Btn_Show.Enabled = false;
                    ListItem li = new ListItem("No Class present", "-1");
                    Drp_ClassName.Items.Add(li);
                }
            }

            private void GenerateStudentExitViolationReport(out string message)
            {
                 message = "";
                string _sql = "";
                string ClassId = "";
                string ClassName = "";
                string StdId = "";
                int Value = int.Parse(Drp_ClassName.SelectedValue.ToString());
                if (Value == 0)
                {
                    _sql = "SELECT tblclass.Id,tblclass.ClassName,tblstandard.Id from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id,tblclass.ClassName";
                }
                else
                {
                    _sql = "SELECT tblclass.Id,tblclass.ClassName,tblstandard.Id from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.Id=" + Value + " AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id,tblclass.ClassName";
                    
                }
                m_MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(_sql);
                if (m_MyReader.HasRows)
                {

                    Exitviolationds.Tables.Add("ExitViolation");
                    _dt = Exitviolationds.Tables["ExitViolation"];
                    _dt.Columns.Add("StudentName");
                    _dt.Columns.Add("ClassName");
                    while (m_MyReader.Read())
                    {
                        ClassId = m_MyReader.GetValue(0).ToString();
                        ClassName = m_MyReader.GetValue(1).ToString();
                        StdId = m_MyReader.GetValue(2).ToString();
                        GenerateReport(ClassId, ClassName, StdId);
                    }
                    if (Exitviolationds != null && Exitviolationds.Tables[0].Rows.Count > 0)
                    {
                        Lbl_Err.Text = "";
                        Btn_Excel.Enabled = true;
                        ViewState["ExitViolationList"] = Exitviolationds;
                        Grd_ExitViolationReport.DataSource = Exitviolationds;
                        Grd_ExitViolationReport.DataBind();
                        Grd_ExitViolationReport.Visible = true;
                        Grd_StaffExitViolationReport.Visible = false;
                    }
                    else
                    {
                        Btn_Excel.Enabled = false;
                        Grd_ExitViolationReport.DataSource = null;
                        Grd_ExitViolationReport.DataBind();
                        message = "No report found";
                    }
                }
            }

            private void GenerateReport(string ClassId, string ClassName, string StdId)
            {
                if (MyAttendance.AttendanceTables_Exits(StdId, MyUser.CurrentBatchId))
                {
                    DataSet Ds = new DataSet();
                    DateTime _startdate = MyUser.GetDareFromText(Txt_FromDate.Text);
                    DateTime _enddate = MyUser.GetDareFromText(Txt_ToDate.Text);                                                                                                                                                                                                                                                                                                                                                                                                                                                                   // date(t1.Date) between '" + _startdate.ToString("s") + " ' and '" + _enddate.ToString("s") + "'
                    string Sql = "SELECT tblstudent.StudentName,t2.OutTime FROM tblattdcls_std" + StdId + "yr" + MyUser.CurrentBatchId + " t1 INNER JOIN tblattdstud_std" + StdId + "yr" + MyUser.CurrentBatchId + " t2 ON t2.ClassAttendanceId=t1.Id INNER JOIN tblstudent ON tblstudent.Id=t2.StudentId WHERE t2.PresentStatus<>0 AND t1.ClassId=" + ClassId + " AND t1.Date in (SELECT MAX(t3.`Date`) FROM tblattdcls_std" + StdId + "yr" + MyUser.CurrentBatchId + " as t3 WHERE date(t3.`Date`) between '" + _startdate.ToString("s") + " ' and '" + _enddate.ToString("s") + "')";
                    Ds = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(Sql);
                    if (Ds != null && Ds.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in Ds.Tables[0].Rows)
                        {
                            TimeSpan _tmspan = new TimeSpan(0);
                            TimeSpan _tmspannormal = new TimeSpan(0);
                            string _outtimestr = dr["OutTime"].ToString();
                            TimeSpan.TryParse(_outtimestr, out _tmspan);
                            if (_tmspan == _tmspannormal)
                            {

                                _dr = _dt.NewRow();
                                _dr["ClassName"] = ClassName;
                                _dr["StudentName"] = dr["StudentName"];
                                Exitviolationds.Tables["ExitViolation"].Rows.Add(_dr);
                            }

                        }

                    }
                }
            }

        #endregion

          
          

    }
}
