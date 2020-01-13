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
    public partial class AbsenteesReport : System.Web.UI.Page
    {
        private OdbcDataReader m_MyReader = null;
        private KnowinUser MyUser;
        private StudentManagerClass MyStudMang;
        private Attendance MyAttendance;
        private SMSManager MysmsMang;
        private DataSet MyDataSet = new DataSet();
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
                MysmsMang = MyUser.GetSMSMngObj();
                if (MysmsMang == null)
                {
                    Response.Redirect("RoleErr.htm");

                }
                if (MyStudMang == null)
                {
                    Response.Redirect("RoleErr.htm");
                    //no rights for this user.
                }
                else if (!MyUser.HaveActionRignt(904))
                {
                    Response.Redirect("RoleErr.htm");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        Btn_Excel.Enabled = false;
                        DateTime _date = System.DateTime.Today;
                        string _sdate = MyUser.GerFormatedDatVal(_date);
                        Txt_FromDate.Text = _sdate;
                        Txt_ToDate.Text = _sdate;
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
                if (Drp_Period.SelectedItem.Text.ToString() == "Today")
                {
                    DateTime _date = System.DateTime.Today;
                    //_sdate = _date.ToString("dd/MM/yyyy");
                    _sdate = MyUser.GerFormatedDatVal(_date);

                    Txt_FromDate.Enabled = false;
                    Txt_ToDate.Enabled = false;
                    Txt_FromDate.Text = _sdate;
                    Txt_ToDate.Text = _sdate;
                }
                else if (Drp_Period.SelectedItem.Text.ToString() == "Last Week")
                {
                    DateTime _date = System.DateTime.Now;
                    //_edate = _date.ToString("dd/MM/yyyy");
                    _edate = MyUser.GerFormatedDatVal(_date);

                    DateTime _start = _date.AddDays(-7);
                    //_sdate = _start.Date.ToString("dd/MM/yyyy");
                    _sdate = MyUser.GerFormatedDatVal(_start);

                    Txt_FromDate.Enabled = false;
                    Txt_ToDate.Enabled = false;
                    Txt_FromDate.Text = _sdate;
                    Txt_ToDate.Text = _edate;
                }

                else if (Drp_Period.SelectedItem.Text.ToString() == "This Month")
                {
                    DateTime _date = System.DateTime.Now;
                    //_edate = _date.ToString("dd/MM/yyyy");
                    _edate = MyUser.GerFormatedDatVal(_date);

                    DateTime _start = System.DateTime.Now;
                    //_sdate = _start.Date.ToString("01/MM/yyyy");
                    // _sdate = "01/" + _start.Month + "/" + _start.Year;
                    _sdate = MyUser.GerFormatedDatVal(new DateTime(_start.Year, _start.Month, 1));
                    Txt_FromDate.Enabled = false;
                    Txt_ToDate.Enabled = false;
                    Txt_FromDate.Text = _sdate;
                    Txt_ToDate.Text = _edate;
                }
                else if (Drp_Period.SelectedItem.Text.ToString() == "Manual")
                {
                    Txt_FromDate.Enabled = true;
                    Txt_ToDate.Enabled = true;
                    Txt_FromDate.Text = "";
                    Txt_ToDate.Text = "";
                }
            }

            protected void Btn_Show_Click(object sender, EventArgs e)
            {
                DateTime todaysdate = System.DateTime.Now.Date;
                if (Txt_FromDate.Text == "" || Txt_ToDate.Text == "")
                {
                    Lbl_Err.Text = "You must enter the start date and end date";
                    Grd_AbsenteesReport.DataSource = null;
                    Grd_AbsenteesReport.DataBind();
                }
                else
                {
                    DateTime _sdate = MyUser.GetDareFromText(Txt_FromDate.Text);
                    DateTime _edate = MyUser.GetDareFromText(Txt_ToDate.Text);
                    if (_edate.Date < _sdate.Date)
                    {
                        Lbl_Err.Text = "End date must be greater than start date";
                        Grd_AbsenteesReport.DataSource = null;
                        Grd_AbsenteesReport.DataBind();
                    }                  
                    else
                    {
                        GetAbsenteesReport();
                    }
                }
            }

            protected void Grd_AbsenteesReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                Grd_AbsenteesReport.PageIndex = e.NewPageIndex;
                GetAbsenteesReport();

            }          

            protected void Btn_Send_Click(object sender, EventArgs e)
            {
                MysmsMang.InitClass();               
                string phonelist = "";
                string msg = "";
                string Message = "";
                bool Valid = true;

                if (Data_Complete(out msg))
                {
                    Grd_AbsenteesReport.Columns[6].Visible = true;
                    string _StudentId = GetStudentIdFromGrid();
                    DataSet Parents = GetParentsList(_StudentId);
                    Grd_AbsenteesReport.Columns[6].Visible = false;

                    foreach (GridViewRow gv in Grd_AbsenteesReport.Rows)
                    {

                        CheckBox Chk_selected = (CheckBox)gv.FindControl("CheckBoxUpdate");
                        if (Chk_selected.Checked)
                        {
                            string tddate = gv.Cells[4].Text.ToString();
                            phonelist = MysmsMang.Get_SelectedParentPhoneNo_List(gv.Cells[6].Text.ToString());
                            if (phonelist != "")
                            {
                                Message = MysmsMang.GenerateSMSstring(Txt_SmsText.Text, GetParentName(ref Parents, gv.Cells[6].Text.ToString()), gv.Cells[1].Text.ToString(), tddate,MyUser.CurrentBatchName);
                                //dominic
                                string failedList = "";
                                if (MysmsMang.SendBULKSms(phonelist, Message, "90366450445", "WINER", true, out  failedList))
                                {
                                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "SMS In student absentees", "Message : " + Message, 1);
                                    Valid = true;
                                }
                            }
                        }
                    }
                    if (Valid)
                        WC_MessageBox.ShowMssage("SMS sent successfully");
                    else
                        WC_MessageBox.ShowMssage("SMS sending failed. Please try again");

                }
                else
                {
                    WC_MessageBox.ShowMssage(msg);
                }

            }

            private string GetParentName(ref DataSet Parents, string _StudentId)
            {
                string _Parent = "0";
                foreach (DataRow Dr_Parent in Parents.Tables[0].Rows)
                {
                    if (_StudentId == Dr_Parent[0].ToString())
                    {
                        _Parent = Dr_Parent[1].ToString();
                        break;
                    }
                }
                return _Parent;
            }

            protected void Grd_AbsenteesReport_SelectedIndexChanged(object sender, EventArgs e)
            {
                Hdn_StudentId.Value = Grd_AbsenteesReport.SelectedRow.Cells[6].Text;
                Hdn_Studname.Value = Grd_AbsenteesReport.SelectedRow.Cells[1].Text;
                Hdn_tdate.Value = Grd_AbsenteesReport.SelectedRow.Cells[4].Text;
                MPE_SMSPOPUP.Show();
                
            }

            protected void Btn_magok_Click(object sender, EventArgs e)
            {
                Hdn_StudentId.Value = "";
                Hdn_Studname.Value = "";
                Hdn_tdate.Value = "";

            }


            private void Load_Seperators()
            {
                OdbcDataReader AttendanceReader=null;
                string innerhtml = "<table cellspacing=\"10\">";
                string sql = "SELECT tblsmsseperators.`Type`,tblsmsseperators.Seperator FROM tblsmsseperators INNER JOIN tblsmsseperatormap ON tblsmsseperators.Id=tblsmsseperatormap.SeperatorId INNER JOIN tblsmsoptionconfig ON tblsmsseperatormap.TypeId=tblsmsoptionconfig.Id where  tblsmsoptionconfig.ShortName='OnAbsent'";
                AttendanceReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
                if (AttendanceReader.HasRows)
                {

                    while (AttendanceReader.Read())
                    {
                        innerhtml = innerhtml + "<tr style=\"height:20px\"><td>" + AttendanceReader.GetValue(0).ToString() + " : </td> <td class=\"new\"> " + AttendanceReader.GetValue(1).ToString() + " </td></tr> ";
                    }
                }                
                innerhtml = innerhtml + "</table>";
                this.Seperators.InnerHtml = innerhtml;
                Div_ind.InnerHtml = innerhtml;
                AttendanceReader.Close();
            }

            protected void Btn_IndSMS_Click(object sender, EventArgs e)
            {
                 MysmsMang.InitClass();
                 string tddate = Hdn_tdate.Value;
                string phonelist = "";
                string msg = "";
                string Message = "";
                bool Valid = true;
                string _StudentId = "",_studname="";
                string   failedList="";
                if (Txt_InSmStext.Text!="")
                {
                    _StudentId = Hdn_StudentId.Value;
                    _studname = Hdn_Studname.Value;
                    if (_StudentId != "" && _studname!="")
                    {

                        DataSet Parents = GetParentsList(_StudentId);
                        phonelist = MysmsMang.Get_SelectedParentPhoneNo_List(_StudentId);
                        if (phonelist != "")
                        {
                            Message = MysmsMang.GenerateSMSstring(Txt_SmsText.Text, GetParentName(ref Parents, _StudentId), _studname, tddate,MyUser.CurrentBatchName);
                            //dominic sms changes
                            if (MysmsMang.SendBULKSms(phonelist, Message, "90366450445", "WINER", true,out  failedList))
                            {
                                MyUser.m_DbLog.LogToDb(MyUser.UserName, "SMS In student absentees", "Message : " + Message, 1);
                                Valid = true;
                            }
                        }
                    }
                    else
                    {
                        Lbl_Smsmsg.Text = "Error,Can't send SMS";
                    }
                }
                else
                {
                    Lbl_Smsmsg.Text="Enter SMS message";
                }
            }

            private string GetSMSText(int _Option)
            {
                string _Format = "";
                OdbcDataReader MyReader = null;
                string sql = "SELECT `Format` FROM tblsmsoptionconfig WHERE Id=" + _Option;
                MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    _Format = MyReader.GetValue(0).ToString();
                    Txt_InSmStext.Text = MyReader.GetValue(0).ToString();
                }
                MyReader.Close();
                return _Format;
            }

            private void GenerateReport(string ClassId, string ClassName, string StdId)
            {

                if (MyAttendance.AttendanceTables_Exits(StdId, MyUser.CurrentBatchId))
                {
                    DataSet Ds = new DataSet();
                    DateTime _startdate = MyUser.GetDareFromText(Txt_FromDate.Text);
                    DateTime _enddate = MyUser.GetDareFromText(Txt_ToDate.Text);
                    //date(BillDateTime) between '" + _startdate.ToString("s") + " ' and '" + _enddate.ToString("s") + "'
                    string Sql = "SELECT  tblstudent.Id as studid, tblstudent.StudentName,tblstudent.OfficePhNo,DATE_FORMAT(t1.Date,'%d/%m/%Y') as Date FROM tblattdcls_std" + StdId + "yr" + MyUser.CurrentBatchId + " t1 INNER JOIN tblattdstud_std" + StdId + "yr" + MyUser.CurrentBatchId + " t2 ON t2.ClassAttendanceId=t1.Id INNER JOIN tblstudent ON tblstudent.Id=t2.StudentId WHERE t2.PresentStatus=0  AND t1.ClassId=" + ClassId + " AND date(t1.Date) between '" + _startdate.ToString("s") + " ' and '" + _enddate.ToString("s") + "'";
                    Ds = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(Sql);
                    if (Ds != null && Ds.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in Ds.Tables[0].Rows)
                        {
                            DateTime date = General.GetDateTimeFromText(dr["Date"].ToString());
                            string datestr = General.GerFormatedDatVal(date);
                            _dr = _dt.NewRow();
                            _dr["ClassName"] = ClassName;
                            _dr["StudentName"] = dr["StudentName"];
                            _dr["PhoneNumber"] = dr["OfficePhNo"];
                            _dr["studid"] = dr["studid"];
                            _dr["Date"] = datestr;
                            MyDataSet.Tables["Absentees"].Rows.Add(_dr);

                        }

                    }
                }
            }

            protected void Btn_Excel_Click(object sender, EventArgs e)
            {
                DataSet _CountReportList = new DataSet();
                _CountReportList = (DataSet)ViewState["AbsenteesList"];
                //if (!WinEr.ExcelUtility.ExportDataSetToExcel(_CountReportList, "CountReport.xls"))
                //{
                //    Lbl_msg.Text = "This function need Ms office";
                //    this.MPE_MessageBox.Show();
                //}
                string FileName = "AbsenteesReport";
                string _ReportName = "Absentees Report";
                if (!WinEr.ExcelUtility.ExportDataToExcel(_CountReportList, _ReportName, FileName, MyUser.ExcelHeader))
                {

                    Lbl_Err.Text = "This function need Ms office";
                }
            }

        #endregion

        #region Methods

            private void GetAbsenteesReport()
            {
                string _sql = "";
                string ClassId = "";
                string ClassName = "";
                string StdId = "";
                int Value = int.Parse(Drp_Classname.SelectedValue.ToString());
                if (Value == 0)
                {
                    _sql = "SELECT tblclass.Id,tblclass.ClassName,tblstandard.Id from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id,tblclass.ClassName";
                }
                else
                {
                    _sql = "SELECT tblclass.Id,tblclass.ClassName,tblstandard.Id from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.Id=" + Value + " AND  tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id,tblclass.ClassName";
                }
                m_MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(_sql);
                if (m_MyReader.HasRows)
                {
                    MyDataSet.Tables.Add("Absentees");
                    _dt = MyDataSet.Tables["Absentees"];
                    _dt.Columns.Add("StudentName");
                    _dt.Columns.Add("PhoneNumber");
                    _dt.Columns.Add("ClassName");
                    _dt.Columns.Add("Date");
                    _dt.Columns.Add("studid");

                    while (m_MyReader.Read())
                    {
                        ClassId = m_MyReader.GetValue(0).ToString();
                        ClassName = m_MyReader.GetValue(1).ToString();
                        StdId = m_MyReader.GetValue(2).ToString();
                        GenerateReport(ClassId, ClassName, StdId);
                    }
                    if (MyDataSet != null && MyDataSet.Tables[0].Rows.Count > 0)
                    {
                        Lbl_Err.Text = "";
                        Btn_Excel.Enabled = true;
                        Grd_AbsenteesReport.Columns[6].Visible = true;
                        ViewState["AbsenteesList"] = MyDataSet;
                        Grd_AbsenteesReport.DataSource = MyDataSet;
                        Grd_AbsenteesReport.DataBind();

                        if (MyUser.HaveModule(23) && MyUser.HaveActionRignt(96))
                        {
                            Pnl_SmStext.Visible = true;
                            Txt_SmsText.Text = GetSMSText(7);
                            Load_Seperators();
                        }
                        else
                            Pnl_SmStext.Visible = false;
                        Grd_AbsenteesReport.Columns[6].Visible = false;
                    }
                    else
                    {
                        Pnl_SmStext.Visible = false;
                        Btn_Excel.Enabled = false;
                        Grd_AbsenteesReport.DataSource = null;
                        Grd_AbsenteesReport.DataBind();
                        Lbl_Err.Text = "No report found";
                    }


                }
            }

            private bool Data_Complete(out string msg)
            {
                bool valid = true;
                msg = "";
                if (Txt_SmsText.Text.Trim() == "")
                {
                    msg = "Enter SMS Message";
                    valid = false;
                }

                if (valid)
                {
                    bool _selected = false;
                    foreach (GridViewRow gv in Grd_AbsenteesReport.Rows)
                    {
                        CheckBox Chk_selected = (CheckBox)gv.FindControl("CheckBoxUpdate");
                        if (Chk_selected.Checked)
                        {
                            _selected = true;
                            break;
                        }
                    }
                    if (!_selected)
                    {
                        msg = "Select student for senting SMS";
                        valid = false;
                    }
                }
                return valid;
            }

            private string GetStudentIdFromGrid()
            {
                string Student = "";
                foreach (GridViewRow gv in Grd_AbsenteesReport.Rows)
                {
                    CheckBox Chk_selected = (CheckBox)gv.FindControl("CheckBoxUpdate");
                    if (Chk_selected.Checked)
                    {
                        if (Student == "")
                            Student = Student + gv.Cells[6].Text.ToString();
                        else
                            Student = Student + "," + gv.Cells[6].Text.ToString();
                    }
                }
                return Student;
            }

            private DataSet GetParentsList(string _StudentId)
            {
                DataSet Parents = new DataSet();
                OdbcDataReader MyReader = null;
                DataTable dt;
                DataRow dr;
                Parents.Tables.Add(new DataTable("ParentList"));
                dt = Parents.Tables["ParentList"];
                dt.Columns.Add("StudentId");
                dt.Columns.Add("Parent");
                string sql = "select Id,GardianName from tblstudent where Id in (" + _StudentId + ")";
                MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        dr = Parents.Tables["ParentList"].NewRow();
                        dr["StudentId"] = MyReader.GetValue(0).ToString();
                        dr["Parent"] = MyReader.GetValue(1).ToString();
                        Parents.Tables["ParentList"].Rows.Add(dr);
                    }
                }
                MyReader.Close();
                return Parents;
            }

            private void LoadClassToDropdown()
            {
                Drp_Classname.Items.Clear();

                DataSet Ds = new DataSet();
                Ds = MyUser.MyAssociatedClass();
                if (Ds != null && Ds.Tables != null && Ds.Tables[0].Rows.Count > 0)
                {
                    Btn_Show.Enabled = true;
                    ListItem li = new ListItem("ALL", "0");
                    Drp_Classname.Items.Add(li);

                    foreach (DataRow dr in Ds.Tables[0].Rows)
                    {
                        li = new ListItem(dr[1].ToString(), dr[0].ToString());
                        Drp_Classname.Items.Add(li);
                    }
                }
                else
                {
                    Btn_Show.Enabled = false;
                    ListItem li = new ListItem("No Class present", "-1");
                    Drp_Classname.Items.Add(li);
                }
            }

        #endregion

    }
}
