using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;

namespace WinEr
{
    public partial class staffattdreport : System.Web.UI.Page
    {

        private ClassOrganiser MyClassMang;
        private Attendance MyAttendance;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;


        protected void Page_PreInit(Object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyClassMang = MyUser.GetClassObj();
            MyAttendance = MyUser.GetAttendancetObj();
            if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(601))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {


                if (!IsPostBack)
                {
                    Btn_export_Excel.Enabled = false;
                    DateTime _date = System.DateTime.Today;
                    string _sdate = MyUser.GerFormatedDatVal(_date);
                    Txt_StartDate.Text = _sdate;
                    Txt_EndDate.Text = _sdate;

                    if (Session["ClassId"] != null)
                    {

                       // Loadclass(int.Parse(Session["ClassId"].ToString()));
                    }
                    else
                    {

                       // Loadclass(0);
                    }

                }
            }
        }




        //private void Loadclass(int _ClassId)
        //{
        //    Drp_ClassSelect.Items.Clear();

        //    MydataSet = MyUser.MyAssociatedClass();
        //    if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
        //    {

        //        foreach (DataRow dr in MydataSet.Tables[0].Rows)
        //        {

        //            ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
        //            Drp_ClassSelect.Items.Add(li);

        //        }
        //        Drp_ClassSelect.SelectedValue = _ClassId.ToString();
        //    }
        //    else
        //    {
        //        ListItem li = new ListItem("No Class present", "-1");
        //        Drp_ClassSelect.Items.Add(li);
        //    }
        //    Drp_ClassSelect.SelectedIndex = 0;
        //}   

        protected void Drp_Period_SelectedIndexChanged(object sender, EventArgs e)
        {

            Btn_export_Excel.Enabled = false;

            lblerror.Text = "";
            string _sdate = null, _edate = null;
            if (Drp_Period.SelectedItem.Text.ToString() == "Today")
            {
                DateTime _date = System.DateTime.Today;
                //_sdate = _date.ToString("dd/MM/yyyy");
                _sdate = MyUser.GerFormatedDatVal(_date);

                Txt_StartDate.Enabled = false;
                Txt_EndDate.Enabled = false;
                Txt_StartDate.Text = _sdate;
                Txt_EndDate.Text = _sdate;
            }
            else if (Drp_Period.SelectedItem.Text.ToString() == "Last Week")
            {
                DateTime _date = System.DateTime.Now;
                //_edate = _date.ToString("dd/MM/yyyy");
                _edate = MyUser.GerFormatedDatVal(_date);

                DateTime _start = _date.AddDays(-7);
                //_sdate = _start.Date.ToString("dd/MM/yyyy");
                _sdate = MyUser.GerFormatedDatVal(_start);

                Txt_StartDate.Enabled = false;
                Txt_EndDate.Enabled = false;
                Txt_StartDate.Text = _sdate;
                Txt_EndDate.Text = _edate;
            }

            else if (Drp_Period.SelectedItem.Text.ToString() == "This Month")
            {
                DateTime _date = System.DateTime.Now;
                //_edate = _date.ToString("dd/MM/yyyy");
                _edate = MyUser.GerFormatedDatVal(_date);

                DateTime _start = System.DateTime.Now;
                //_sdate = _start.Date.ToString("01/MM/yyyy");
                // _sdate = "01/" + _start.Month + "/" + _start.Year;
                _sdate = MyUser.GerFormatedDatVal(new DateTime(_start.Year,_start.Month,1));
                Txt_StartDate.Enabled = false;
                Txt_EndDate.Enabled = false;
                Txt_StartDate.Text = _sdate;
                Txt_EndDate.Text = _edate;
            }
            else if (Drp_Period.SelectedItem.Text.ToString() == "Manual")
            {
                Txt_StartDate.Enabled = true;
                Txt_EndDate.Enabled = true;
                Txt_StartDate.Text = "";
                Txt_EndDate.Text = "";
            }
            else if (Drp_Period.SelectedItem.Text.ToString() == "Current Batch")
            {
                DateTime _Start, _End;
                MyAttendance.GetbatchDates(MyUser.CurrentBatchId,out _Start, out _End);
                DateTime _date = System.DateTime.Now;
                Txt_StartDate.Enabled = false;
                Txt_EndDate.Enabled = false;
                Txt_StartDate.Text = MyUser.GerFormatedDatVal(_Start);
                Txt_EndDate.Text = MyUser.GerFormatedDatVal(_date);
            }
         
        }

 

        protected void Btn_showreport_Click(object sender, EventArgs e)
        {
            string _message;
            if (validdates(out _message))
            {
                lblerror.Text = "";
                MydataSet = buildAttendanceDataset();
               
                bindToreportGrid(MydataSet);
            }
            else
                lblerror.Text = _message;
        }

        private DataSet buildAttendanceDataset()
        {
            //int _classid = int.Parse(Drp_ClassSelect.SelectedValue);
            //string standard = MyAttendance.GetStandard_Class(_classid);
            string _sdate, _enddate, sql;
            finddates(out _sdate, out _enddate);

            int _workingdays, _absentdays,halfdays, _staffid;
            double _percentage, _presentdays;
            DataSet attendance_rept = new DataSet();
            DataTable dt;
            attendance_rept.Tables.Add(new DataTable("AttendanceReport"));
            dt = attendance_rept.Tables["AttendanceReport"];
            dt.Columns.Add("Name");
            dt.Columns.Add("WorkingDays");
            dt.Columns.Add("PresentDays");
            dt.Columns.Add("HalfDays");
            dt.Columns.Add("AbsentDays");
            dt.Columns.Add("Percentage");

            //if (MyAttendance.AttendanceTables_Exits(standard, MyUser.CurrentBatchId))
            //{
               // sql = "SELECT tblstudent.Id, tblstudent.StudentName from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + int.Parse(Drp_ClassSelect.SelectedValue) + " Order by tblstudentclassmap.RollNo ASC";
                sql = "SELECT DISTINCT t.Id,t.SurName FROM tbluser t  inner join tblstaffdetails on tblstaffdetails.UserId = t.Id  inner join tblrole r on t.RoleId=r.Id inner join tblgroupusermap g on t.Id=g.UserId  where t.Status=1 AND r.Type='Staff' AND g.GroupId IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=1 UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=1)";
            
            
                MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        DataRow dr = dt.NewRow();

                        dr["Name"] = MyReader.GetValue(1).ToString();

                        _staffid = int.Parse(MyReader.GetValue(0).ToString());

                        _workingdays = MyAttendance.Numberofstaffworkingdays();
                        
                        _absentdays = MyAttendance.GetNoOf_AbsentDayForTheperiod_New(_staffid, standard, MyUser.CurrentBatchId, _sdate, _enddate);
                        _presentdays = MyAttendance.GetNoOf_FullDayForTheperiod_New(_staffid, standard, MyUser.CurrentBatchId, _sdate, _enddate);
                        halfdays = MyAttendance.GetNoOf_HalfDayForTheperiod_New(_staffid, standard, MyUser.CurrentBatchId, _sdate, _enddate);

                        if (halfdays != 0)
                        {
                            _percentage = (double)(_presentdays + (double)(halfdays / 2)) * 100;
                        }
                        else
                        {
                            _percentage = (double)(_presentdays) * 100;
                        }
                        if (_percentage != 0)
                            _percentage = _percentage / (double)_workingdays;

                        dr["WorkingDays"] = _workingdays;
                        dr["PresentDays"] = _presentdays;
                        dr["HalfDays"] = halfdays;
                        dr["AbsentDays"] = _absentdays;
                        dr["Percentage"] = _percentage.ToString("0.0");
                        attendance_rept.Tables["AttendanceReport"].Rows.Add(dr);
                    }
                }
                
            //}
            return attendance_rept;
        }

        private void finddates(out string _sdate, out string _edate)
        {
            
            _sdate = null;
            _edate = null;
            if (Drp_Period.SelectedItem.Text.ToString() == "Today")
            {
                DateTime _date = System.DateTime.Today;
                _sdate = _date.ToString("yyyy-MM-dd");
                _edate = _sdate;

            }
            else if (Drp_Period.SelectedItem.Text.ToString() == "Last Week")
            {
                DateTime _date = System.DateTime.Now;
                _edate = _date.ToString("yyyy-MM-dd");
                DateTime _start = _date.AddDays(-7);
                _sdate = _start.Date.ToString("yyyy-MM-dd");

            }
            else if (Drp_Period.SelectedItem.Text.ToString() == "This Month")
            {
                DateTime _date = System.DateTime.Now;
                _edate = _date.ToString("yyyy-MM-dd");
                DateTime _start = System.DateTime.Now;
                _sdate = _start.Date.ToString("yyyy-MM-01");
            }
            else if (Drp_Period.SelectedItem.Text.ToString() == "Manual")
            {

                if ((Txt_EndDate.Text != "") && (Txt_StartDate.Text != ""))
                {

                    _sdate = Txt_StartDate.Text.ToString();
                    //DateTime _sdate1 = DateTime.Parse(_sdate);
                    DateTime _sdate1 = MyUser.GetDareFromText(_sdate);
                    _sdate = _sdate1.ToString("yyyy-MM-dd");
                    //_sdate = _sdate1.ToString("s");
                    _edate = Txt_EndDate.Text.ToString();
                    //DateTime _edate1 = DateTime.Parse(_edate);
                    DateTime _edate1 = MyUser.GetDareFromText(_edate);
                    _edate = _edate1.ToString("yyyy-MM-dd");
                    //  _edate = _edate1.ToString("s");

                }
            }
            else if (Drp_Period.SelectedItem.Text.ToString() == "Current Batch")
            {
                DateTime _Start, _End;
                MyAttendance.GetbatchDates(MyUser.CurrentBatchId, out _Start, out _End);
                DateTime _date = System.DateTime.Now;
                _edate = _date.ToString("yyyy-MM-dd");
                _sdate = _Start.Date.ToString("yyyy-MM-dd");
            }
        }

        private void bindToreportGrid(DataSet MydataSet)
        {
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Btn_export_Excel.Enabled = true;

                Grd_Attenndancerept.DataSource = MydataSet;
                Grd_Attenndancerept.DataBind();
                lblerror.Text = "";
                ViewState["Dataset"] = MydataSet;
            }
            else
            {
                Btn_export_Excel.Enabled = false;
                lblerror.Text = "No Attendance Report To View During This Period";
                Grd_Attenndancerept.DataSource = null;
                Grd_Attenndancerept.DataBind();

            }
        }

        private bool validdates(out string _message)
        {
            bool valid = true;
            _message = null;

            if (Txt_EndDate.Text.Trim() == "")
            {
                _message = "The End Date Is Empty";
                valid = false;

            }

            if (Txt_StartDate.Text.Trim() == "")
            {
                _message = "The Start Date Is Empty";
                valid = false;
            }


            if (valid == true)
            {


                string _date1 = Txt_StartDate.Text.ToString();
                string _date2 = Txt_EndDate.Text.ToString();

                //DateTime startdate = DateTime.Parse(_date1);
                //DateTime enddate = DateTime.Parse(_date2);

                DateTime startdate = MyUser.GetDareFromText(_date1);
                DateTime enddate = MyUser.GetDareFromText(_date2);

                TimeSpan diff = enddate.Subtract(startdate);
                int _diff = int.Parse(diff.Days.ToString());

                DateTime today = DateTime.Now;


                if (_diff < 0)
                {
                    _message = "The Start Date Is Larger Than End Date";
                    valid = false;
                }

                if (startdate > today)
                {
                    _message = "The Start Date Is Larger Than Todays date";
                    valid = false;
                }

                if (enddate > today)
                {
                    _message = "The End Date Is Larger Than Todays date";
                    valid = false;
                }
            }
            return valid;

        }

        protected void Grd_Attenndancerept_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Attenndancerept.PageIndex = e.NewPageIndex;
            MydataSet = (DataSet)ViewState["Dataset"];
            bindToreportGrid(MydataSet);
        }



        protected void Btn_export_Excel_Click(object sender, EventArgs e)
        {
            string _message;
            if (validdates(out _message))
            {
                MydataSet = (DataSet)ViewState["Dataset"];
                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    lblerror.Text = "";

                    //if (!WinEr.ExcelUtility.ExportDataSetToExcel(MydataSet, "AttendanceReport.xls"))
                    //{
                    //    lblerror.Text = "This function need Ms office";
                    //}
                    string FileName = "AttendanceReport";
                    string _ReportName = "Attendance Report";
                    if (!WinEr.ExcelUtility.ExportDataToExcel(MydataSet, _ReportName, FileName, MyUser.ExcelHeader))
                    {
                        lblerror.Text = "This function need Ms office";
                    }
                    
                }
                else
                    lblerror.Text = "No Attendance Report For The Class To view during this period";
            }
            else
                lblerror.Text = _message;
        }


    }
}

