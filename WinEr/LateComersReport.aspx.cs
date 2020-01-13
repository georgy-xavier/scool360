using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using WinBase;

namespace WinEr
{
    public partial class LateComersReport : System.Web.UI.Page
    {
        private OdbcDataReader m_MyReader = null;
        private KnowinUser MyUser;
        private StudentManagerClass MyStudMang;
        private DataSet MyDataSet=new DataSet();
        private Attendance MyAttendance;
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
                else if (!MyUser.HaveActionRignt(841))
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
                        Pnl_StaffDetailsDisplay.Visible = false;
                        Pnl_ShowReport.Visible = false;

                    }
                }

            }

            protected void RbLst_Type_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (RbLst_Type.SelectedItem.ToString() == "Student")
                {
                    Drp_Classname.Enabled = true;
                }
                else
                {
                    Drp_Classname.Enabled = false;
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
                    Grd_LateComerReport.DataSource = null;
                    Grd_LateComerReport.DataBind();
                }
                else
                {
                    DateTime _sdate = MyUser.GetDareFromText(Txt_FromDate.Text);
                    DateTime _edate = MyUser.GetDareFromText(Txt_ToDate.Text);
                    if (_edate.Date < _sdate.Date)
                    {
                        Lbl_Err.Text = "End date must be greater than start date";
                        Grd_LateComerReport.DataSource = null;
                        Grd_LateComerReport.DataBind();
                    }
                    
                   else if (_edate > todaysdate)
                    {
                        Lbl_Err.Text = "End date is greater than todays date";

                    }
                    else
                    {
                        GetLateComerReport();
                    }
                }
            }

            protected void Btn_Excel_Click(object sender, EventArgs e)
            {
                DataSet _CountReportList = new DataSet();
                _CountReportList = (DataSet)ViewState["LateComerList"];
                //if (!WinEr.ExcelUtility.ExportDataSetToExcel(_CountReportList, "CountReport.xls"))
                //{
                //    Lbl_msg.Text = "This function need Ms office";
                //    this.MPE_MessageBox.Show();
                //}
                string FileName = "LateComerReport";
                string _ReportName = "LateComer Report";
                if (!WinEr.ExcelUtility.ExportDataToExcel(_CountReportList, _ReportName, FileName, MyUser.ExcelHeader))
                {

                    Lbl_Err.Text = "This function need Ms office";
                }
            }

            protected void Grd_LateComerReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                Grd_LateComerReport.PageIndex = e.NewPageIndex;
                GetLateComerReport();
            }
            protected void Grd_StaffLateComers_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                Grd_StaffLateComers.PageIndex = e.NewPageIndex;
                GetLateComerReport();
            }


            protected void Txt_ToDate_TextChanged(object sender, EventArgs e)
            {
                
            }

    #endregion

        #region Functions

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

            private void GetLateComerReport()
            {
                string _sql = "";
                string ClassId = "";
                string ClassName = "";
                string StdId = "";
                DataSet StaffreportDs = new DataSet();
                if (RbLst_Type.SelectedItem.ToString() == "Student")
                {
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
                        MyDataSet.Tables.Add("Latecomer");
                        _dt = MyDataSet.Tables["Latecomer"];
                        _dt.Columns.Add("Name");
                        _dt.Columns.Add("PhoneNumber");
                        _dt.Columns.Add("ClassName");
                        _dt.Columns.Add("Date");
                        while (m_MyReader.Read())
                        {
                            ClassId = m_MyReader.GetValue(0).ToString();
                            ClassName = m_MyReader.GetValue(1).ToString();
                            StdId = m_MyReader.GetValue(2).ToString();
                            GenerateReport(ClassId, ClassName, StdId);
                        }
                    }

                    if (MyDataSet != null && MyDataSet.Tables[0].Rows.Count > 0)
                    {
                        Pnl_ShowReport.Visible = true;
                        Pnl_StaffDetailsDisplay.Visible = false;
                        Lbl_Err.Text = "";
                        Btn_Excel.Enabled = true;
                        ViewState["LateComerList"] = MyDataSet;
                        Grd_LateComerReport.DataSource = MyDataSet;
                        Grd_LateComerReport.DataBind();

                    }
                    else
                    {
                        Btn_Excel.Enabled = false;
                        Pnl_ShowReport.Visible = false;
                        Pnl_StaffDetailsDisplay.Visible = false;
                        Grd_LateComerReport.DataSource = null;
                        Grd_LateComerReport.DataBind();
                        Lbl_Err.Text = "No report found";
                    }
                }
                else
                {
                    DateTime _startdate = MyUser.GetDareFromText(Txt_FromDate.Text);
                    DateTime _enddate = MyUser.GetDareFromText(Txt_ToDate.Text);
                    _sql = "select  tbluser.SurName, tblstaffdetails.PhoneNumber,  DATE_FORMAT(tblstaffattendance.MarkDate,'%d/%m/%Y') as MarkDate , tblstaffattendance.InTime from tbluser inner join tblstaffdetails on tbluser.Id= tblstaffdetails.UserId inner join tblstaffattendance on tbluser.Id= tblstaffattendance.StaffId where tblstaffattendance.InTime > (select tblstaffattdconfig.Value from tblstaffattdconfig where tblstaffattdconfig.Name='Late InTime')  and tblstaffattendance.MarkDate between '" + _startdate.ToString("s") + " ' and '" + _enddate.ToString("s") + "'";
                    StaffreportDs = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
                    if (StaffreportDs != null && StaffreportDs.Tables[0].Rows.Count > 0)
                    {
                        Pnl_ShowReport.Visible = false;
                        Pnl_StaffDetailsDisplay.Visible = true;
                        Lbl_Err.Text = "";
                        Btn_Excel.Enabled = true;
                        ViewState["LateComerList"] = StaffreportDs;
                        Grd_StaffLateComers.DataSource = StaffreportDs;
                        Grd_StaffLateComers.DataBind();

                    }
                    else
                    {
                        Pnl_ShowReport.Visible = false;
                        Pnl_StaffDetailsDisplay.Visible = false;
                        Btn_Excel.Enabled = false;
                        Grd_StaffLateComers.DataSource = null;
                        Grd_StaffLateComers.DataBind();
                        Lbl_Err.Text = "No report found";
                    }
                }



            }

            private void GenerateReport(string ClassId, string ClassName, string StdId)
            {
                
                 if (MyAttendance.AttendanceTables_Exits(StdId, MyUser.CurrentBatchId))
                 {
                     DataSet Ds = new DataSet();
                     DateTime _startdate =MyUser.GetDareFromText(Txt_FromDate.Text);
                     DateTime _enddate = MyUser.GetDareFromText(Txt_ToDate.Text);
                     //date(BillDateTime) between '" + _startdate.ToString("s") + " ' and '" + _enddate.ToString("s") + "'
                     string Sql = "SELECT tblstudent.StudentName,tblstudent.OfficePhNo,DATE_FORMAT(t1.Date,'%d/%m/%Y') as Date FROM tblattdcls_std" + StdId + "yr" + MyUser.CurrentBatchId + " t1 INNER JOIN tblattdstud_std" + StdId + "yr" + MyUser.CurrentBatchId + " t2 ON t2.ClassAttendanceId=t1.Id INNER JOIN tblstudent ON tblstudent.Id=t2.StudentId WHERE t2.PresentStatus<>0 AND t2.IsLate=1 AND t1.ClassId=" + ClassId + " AND date(t1.Date) between '" + _startdate.ToString("s") + " ' and '" + _enddate.ToString("s") + "'";
                     Ds = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(Sql);
                     if (Ds != null && Ds.Tables[0].Rows.Count > 0)
                     {

                         foreach (DataRow dr in Ds.Tables[0].Rows)
                         {
                             DateTime date = General.GetDateTimeFromText(dr["Date"].ToString());
                             string datestr = General.GerFormatedDatVal(date);
                             _dr = _dt.NewRow();
                             _dr["ClassName"] = ClassName;
                             _dr["Name"] = dr["StudentName"];
                             _dr["PhoneNumber"] = dr["OfficePhNo"];
                             _dr["Date"] = datestr;
                             MyDataSet.Tables["Latecomer"].Rows.Add(_dr);

                         }

                     }
                 }
            }

        #endregion


      

    }
}
