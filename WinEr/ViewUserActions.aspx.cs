using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using WebChart;
using System.Drawing;


namespace WinEr
{
    public partial class ViewUserActions: System.Web.UI.Page
    {
        private ConfigManager MyConfiMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
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
            if (MyConfiMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(94))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {                   
                    Txt_StartDate.Enabled = false;
                    Txt_EndDate.Enabled = false;
                    DateTime _date = System.DateTime.Today;
                    //string _sdate = _date.ToString("dd/MM/yyyy");
                    string _sdate = MyUser.GerFormatedDatVal(_date);

                    Txt_StartDate.Text = _sdate;
                    Txt_EndDate.Text = _sdate;
                    Pnl_UserAction.Visible = false;

                    LoadUserToList(0);
                    LoadActionsToList(0);

                }

                ChartControl.PerformCleanUp();
            }
        }
        private void LoadActionsToList(int usertype)
        {
            string sql = null;
            Drp_Action.Items.Clear();
            if (usertype == 0) //means all users
            {
                sql = "SELECT DISTINCT tbllog.Action from tbllog";
            }
            else if (usertype == 1)//means staff
            {
                sql = "SELECT DISTINCT tbllog.Action from tbllog where tbllog.UserTypeId=" +1;
            }
            else if (usertype == 2)//means parent
            {
                sql = "SELECT DISTINCT tbllog.Action from tbllog where tbllog.UserTypeId=" + 2;
            }
            else if (usertype == 3)//means student
            {
                sql = "SELECT DISTINCT tbllog.Action from tbllog where tbllog.UserTypeId=" + 3;
            }
            else
            {
                ListItem li = new ListItem("No Actions Found", "-1");
                Drp_Action.Items.Add(li);
            }

               
            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("All", "0");
                Drp_Action.Items.Add(li);
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Action.Items.Add(li);

                }

            }
            else
            {
                ListItem li = new ListItem("No Actions Found", "-1");
                Drp_Action.Items.Add(li);
            }
        }

        private void LoadUserToList(int usertype)
        {
            Drp_UserName.Items.Clear();

            string sql = null;
                if (usertype == 0) //means all users
                {
                    sql = "SELECT DISTINCT tbllog.UserName from tbllog";
                }
                else if (usertype == 1)//means staff
                {
                    sql = "SELECT DISTINCT tbllog.UserName from tbllog where tbllog.UserTypeId=" + 1;
                }
                else if (usertype == 2)//means parent
                {
                    sql = "SELECT DISTINCT tbllog.UserName from tbllog where tbllog.UserTypeId=" + 2;
                }
                else if (usertype == 3)//means student
                {
                    sql = "SELECT DISTINCT tbllog.UserName from tbllog where tbllog.UserTypeId=" + 3;
                }
                else
                {
                    ListItem li = new ListItem("No User Found", "-1");
                    Drp_UserName.Items.Add(li);
                }
               
                MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    ListItem li = new ListItem("All", "0");
                    Drp_UserName.Items.Add(li);
                    while (MyReader.Read())
                    {
                        li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(0).ToString());
                        Drp_UserName.Items.Add(li);

                    }

                }
                else
                {
                    ListItem li = new ListItem("No User Found", "-1");                   
                    Drp_UserName.Items.Add(li);
                    
                }
            
        }


        private void finddates(out string _sdate,out string _edate)
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
//                    DateTime _sdate1 = DateTime.Parse(_sdate);
                    DateTime _sdate1 = MyUser.GetDareFromText(_sdate);

                    _sdate = _sdate1.ToString("yyyy-MM-dd");
                    //_sdate = _sdate1.ToString("s");
                    _edate = Txt_EndDate.Text.ToString();
//                    DateTime _edate1 = DateTime.Parse(_edate);
                    DateTime _edate1 = MyUser.GetDareFromText(_edate);

                    _edate = _edate1.ToString("yyyy-MM-dd");
                    //  _edate = _edate1.ToString("s");

                }
            }
        }


        protected void Drp_Period_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            lblerror.Text = "";
            string _sdate=null,_edate=null;           
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
            if (Drp_Period.SelectedItem.Text.ToString() == "Last Week")
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

            if (Drp_Period.SelectedItem.Text.ToString() == "This Month")
            {
                DateTime _date = System.DateTime.Now;
                //_edate = _date.ToString("dd/MM/yyyy");
                _edate = MyUser.GerFormatedDatVal(_date);

                DateTime _start = System.DateTime.Now;
                //_sdate = _start.Date.ToString("01/MM/yyyy");
                _sdate = "01/" + _start.Date.Month + "/" + _start.Date.Year;
                Txt_StartDate.Enabled = false;
                Txt_EndDate.Enabled = false;
                Txt_StartDate.Text = _sdate;
                Txt_EndDate.Text = _edate;
            }
            if (Drp_Period.SelectedItem.Text.ToString() == "Manual")
            {
                Txt_StartDate.Enabled = true;
                Txt_EndDate.Enabled = true;
                Txt_StartDate.Text = "";
                Txt_EndDate.Text = "";
            }
        }

        protected void Btn_View_Click(object sender, EventArgs e)
        {
            chartcontrol_modulesuse.Visible = false;
            string _message;
             if (validdates(out _message))
             {
                 lblerror.Text = "";
                 MydataSet = buildDataset(0);
                 bindToGrid(MydataSet);
             }
             else
                 lblerror.Text = _message;
        }

        private void bindToGrid(DataSet MydataSet)
        {
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Pnl_UserAction.Visible = true;
                Grd_Actions.DataSource = MydataSet;
                Grd_Actions.DataBind();
                lblerror.Text = "";
            }
            else
            {
                lblerror.Text = "No Actions To View During This Period";
                Grd_Actions.DataSource = null;
                Grd_Actions.DataBind();
                Pnl_UserAction.Visible = false;
            }
        }

        private DataSet buildDataset(int IsExport)
        {
            DataSet dataset = new DataSet();
            DataTable dt;
            DataRow dr;
            dataset.Tables.Add(new DataTable("Reqtbl"));
            dt = dataset.Tables["Reqtbl"];
            dt.Columns.Add("UserName");
            dt.Columns.Add("Action");
            dt.Columns.Add("Time");
            dt.Columns.Add("Description");
            string _sdate, _enddate;
            finddates(out _sdate, out _enddate);
            string sql = null;
            sql = "select tbllog.UserName, tbllog.Action, tbllog.Time, tbllog.Description from tbllog  where date(tbllog.Time) between  '" + _sdate + "' and '" + _enddate + "'";
            
            if (Drp_UserType.SelectedValue != "0")
                sql = sql + " and tbllog.UserTypeId=" + Drp_UserType.SelectedValue;

            if ((Drp_UserName.SelectedValue != "0") && (Drp_UserName.SelectedValue != "-1"))
                sql = sql + " and  tbllog.UserName='" + Drp_UserName.SelectedItem.ToString() + "'";
            
            if ((Drp_Action.SelectedValue != "0") && (Drp_Action.SelectedValue != "-1"))
                sql = sql + " and  tbllog.Action='"+Drp_Action.SelectedValue.ToString()+"'";           

            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {

                while (MyReader.Read())
                {
                    string Desc = MyReader.GetValue(3).ToString();
                    if (Desc.Length > 70 && IsExport==0)
                    {
                       Desc=Desc.Substring(0,69)+"..";
                    }

                    dr = dataset.Tables["Reqtbl"].NewRow();
                    dr["UserName"] = MyReader.GetValue(0).ToString();
                    dr["Action"] = MyReader.GetValue(1).ToString();
                    dr["Time"] = MyReader.GetValue(2).ToString();
                    dr["Description"] = Desc;

                    dataset.Tables["Reqtbl"].Rows.Add(dr);
                }
            }



            return dataset;
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

                //if (enddate > today)
                //{
                //    _message = "The End Date Is Larger Than Todays date";
                //    valid = false;
                //}


            }
            return valid;

        }

        protected void Btn_export_Click(object sender, EventArgs e)
        {
            chartcontrol_modulesuse.Visible = false;
            string _message;
            if (validdates(out _message))
             {
                MydataSet=buildDataset(1);
                 if (MydataSet.Tables[0].Rows.Count > 0)
                 {
                     lblerror.Text = "";

                     //if (!WinEr.ExcelUtility.ExportDataSetToExcel(MydataSet, "ActionReport.xls"))
                     //{
                     //    lblerror.Text = "This function need Ms office";                         
                     //}
                     string FileName = "ActionReport";
                     string _ReportName = "ActionReport";
                     if (!WinEr.ExcelUtility.ExportDataToExcel(MydataSet, _ReportName, FileName, MyUser.ExcelHeader))
                     {
                         lblerror.Text = "This function need Ms office";
                     }

                 }
                 else
                     lblerror.Text = "No Actions during this period";
             }
            else
                lblerror.Text = _message;
        }

        protected void Grd_Actions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Actions.PageIndex = e.NewPageIndex;
            MydataSet = buildDataset(0);
            bindToGrid(MydataSet);
        }

        protected void Drp_UserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grd_Actions.DataSource = null;
            Grd_Actions.DataBind();
            Pnl_UserAction.Visible = false;
            LoadUserToList(int.Parse(Drp_UserType.SelectedValue.ToString()));
            LoadActionsToList(int.Parse(Drp_UserType.SelectedValue.ToString()));
            lblerror.Text = "";
            chartcontrol_modulesuse.Visible = false;
        }

        protected void Drp_UserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grd_Actions.DataSource = null;
            Grd_Actions.DataBind();
            Pnl_UserAction.Visible = false;
            lblerror.Text = "";
            chartcontrol_modulesuse.Visible = false;
        }

        protected void Drp_Action_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grd_Actions.DataSource = null;
            Grd_Actions.DataBind();
            Pnl_UserAction.Visible = false;
            lblerror.Text = "";
            chartcontrol_modulesuse.Visible = false;
        }

        protected void Btn_ModuleUsageChart_Click(object sender, EventArgs e)
        {
            Grd_Actions.DataSource = null;
            Grd_Actions.DataBind();
            Pnl_UserAction.Visible = false;
            lblerror.Text = "";
            ShowChartView();
            
            
        }

        private void ShowChartView()
        {
            chartcontrol_modulesuse.Visible = true;
            PieChart YearlyPiechart = (PieChart)chartcontrol_modulesuse.Charts.FindChart("yearly_Chart");
            YearlyPiechart.Colors = new Color[] { Color.LightGreen, Color.Cyan, Color.Navy, Color.Yellow, Color.AntiqueWhite, Color.RosyBrown, Color.Blue, Color.Red, Color.Green, Color.Orange, Color.Olive, Color.Orchid, Color.Purple, Color.CadetBlue, Color.Gold, Color.Black };

            int _FEES,_STUDENT, _EXAM, _CLASS, _STAFF, _ATTENDANCE, _LIBRARY, _SMS, _TIMETABLE, TRANSPORTATION, INVENTORY, PAYROLL, EMAIL,INCIDENT,CONFIG;
            _FEES = getUsageCount(new string[] { "Fee", "payment" });
            _STUDENT = getUsageCount(new string[] { "Student", "TC", "Enrollment" });
            _EXAM = getUsageCount(new string[] { "Exam","CCE" });
            _CLASS = getUsageCount(new string[] { "Class"});
            _STAFF = getUsageCount(new string[] { "Staff" });
            _ATTENDANCE=getUsageCount(new string[] { "Attendance" });
            _LIBRARY = getUsageCount(new string[] { "Book" });
            _SMS = getUsageCount(new string[] { "sms" });
            _TIMETABLE = getUsageCount(new string[] { "Timetable" });
            TRANSPORTATION = getUsageCount(new string[] { "Transport" });
            INVENTORY = getUsageCount(new string[] { "Inventory" });
            PAYROLL = getUsageCount(new string[] { "payroll" });
            EMAIL = getUsageCount(new string[] { "Email" });
            INCIDENT = getUsageCount(new string[] { "Incident" });
            CONFIG = getUsageCount(new string[] { "Batch", "Holiday" });

            YearlyPiechart.Data.Clear();
            YearlyPiechart.Data.Add(new ChartPoint("Fee", _FEES));
            YearlyPiechart.Data.Add(new ChartPoint("Staff", _STAFF));
            YearlyPiechart.Data.Add(new ChartPoint("Exam", _EXAM));
            YearlyPiechart.Data.Add(new ChartPoint("Class", _CLASS));
            YearlyPiechart.Data.Add(new ChartPoint("Attendance", _ATTENDANCE));
            YearlyPiechart.Data.Add(new ChartPoint("Student", _STUDENT));
            YearlyPiechart.Data.Add(new ChartPoint("Library", _LIBRARY));
            YearlyPiechart.Data.Add(new ChartPoint("SMS", _SMS));
            YearlyPiechart.Data.Add(new ChartPoint("Timetable", _TIMETABLE));
            YearlyPiechart.Data.Add(new ChartPoint("Transportation", TRANSPORTATION));
            YearlyPiechart.Data.Add(new ChartPoint("Inventory", INVENTORY));
            YearlyPiechart.Data.Add(new ChartPoint("Payroll", PAYROLL));
            YearlyPiechart.Data.Add(new ChartPoint("Email", EMAIL));
            YearlyPiechart.Data.Add(new ChartPoint("Incident", INCIDENT));
            YearlyPiechart.Data.Add(new ChartPoint("Configuration", CONFIG));
 
            YearlyPiechart.DataLabels.Visible = false;
            if ((_FEES + _STUDENT + _EXAM + _CLASS + _STAFF + _ATTENDANCE + _LIBRARY + _SMS + _TIMETABLE + TRANSPORTATION + INVENTORY + PAYROLL + EMAIL + INCIDENT + CONFIG) == 0)
            {
                YearlyPiechart.Data.Add(new ChartPoint("NO DATA", 100));

                YearlyPiechart.DataLabels.Visible = false;
            }
            chartcontrol_modulesuse.Background.Color = Color.White;
            chartcontrol_modulesuse.RedrawChart();
        }

        private int getUsageCount(string[] keywords)
        {
            int _count = 0;
            if (keywords.Length > 0)
            {
                string sql_part = " AND ( tbllog.Action LIKE  ('%" + keywords[0] + "%')";
                for(int i=1;i<keywords.Length;i++)
                {
                    sql_part = sql_part + " or tbllog.Action LIKE  ('%" + keywords[i] + "%')";
                }

                string _sdate, _enddate;
                finddates(out _sdate, out _enddate);
                string sql = "Select count(Id) FROM tbllog where date(tbllog.Time) between  '" + _sdate + "' and '" + _enddate + "'" + sql_part+")";
                MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    int.TryParse(MyReader.GetValue(0).ToString(), out _count);
                }
            }
            return _count;
        }

    }
}