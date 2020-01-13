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
    public partial class StaffAttdDetailedReport : System.Web.UI.Page
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
            else if (!MyUser.HaveActionRignt(95))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadDrpMonth();
                    //img_export_Excel.Visible = false;
                    Btn_Excel.Enabled = false;
                    DateTime _date = System.DateTime.Today;
                    //string _sdate = _date.ToString("dd/MM/yyyy");
                    string _sdate = MyUser.GerFormatedDatVal(_date);
                    LoadDateInTextBox();
                }
            }

        }
        protected void Drp_Select_Period_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GridDiv.InnerHtml = "";
            Btn_Excel.Enabled = false;
            LoadDateInTextBox();
        }
        private void LoadDateInTextBox()
        {
            string _sdate = null, _edate = null, _msg = "";

            if (Drp_Select_Month.SelectedItem.ToString() != "NO DATA")
            {

                if (Drp_Select_Period.SelectedItem.Text.ToString() == "Today")
                {
                    Drp_Select_Month.Enabled = false;
                    Btn_show.Enabled = true;
                    DateTime _date = System.DateTime.Today;
                    //_sdate = _date.ToString("dd/MM/yyyy");
                    _sdate = MyUser.GerFormatedDatVal(_date);

                    Txt_SDate.Enabled = false;
                    Txt_EDate.Enabled = false;
                    Txt_SDate.Text = _sdate;
                    Txt_EDate.Text = _sdate;
                }
                if (Drp_Select_Period.SelectedItem.Text.ToString() == "Last Week")
                {
                    Drp_Select_Month.Enabled = false;
                    Btn_show.Enabled = true;
                    DateTime _date = System.DateTime.Now;
                    //_edate = _date.ToString("dd/MM/yyyy");
                    _edate = MyUser.GerFormatedDatVal(_date);

                    DateTime _start = _date.AddDays(-7);
                    //_sdate = _start.Date.ToString("dd/MM/yyyy");
                    _sdate = MyUser.GerFormatedDatVal(_start);

                    Txt_SDate.Enabled = false;
                    Txt_EDate.Enabled = false;
                    Txt_SDate.Text = _sdate;
                    Txt_EDate.Text = _edate;
                }

                if (Drp_Select_Period.SelectedItem.Text.ToString() == "Month Wise")
                {
                    Drp_Select_Month.Enabled = true;
                    LoadDateFromSlectedDate(out _msg);


                }
                if (Drp_Select_Period.SelectedItem.Text.ToString() == "Manual")
                {
                    Btn_show.Enabled = true;
                    Drp_Select_Month.Enabled = false;
                    Txt_EDate.Enabled = true;
                    Txt_SDate.Enabled = true;
                    Txt_SDate.Text = "";
                    Txt_EDate.Text = "";

                }
            }
            else
            {
                Btn_show.Enabled = false;
                Txt_SDate.Text = "";
                Txt_EDate.Text = "";
                Txt_SDate.Enabled = false;
                Txt_EDate.Enabled = false;
                _msg = "No attendance reported";
            }

            Lbl_Err.Text = _msg;
        }
        private void LoadDateFromSlectedDate(out string _Msg)
        {
            _Msg = "";
            int selected_month = int.Parse(Drp_Select_Month.SelectedValue.ToString());

            if (selected_month != -1)
            {
                string _sdate = null, _edate = null;
                DateTime _date, _start;
                Btn_show.Enabled = true;


                if (MyAttendance.CheckSelectedMonthIsCurrentMonthorNot(selected_month))
                {
                    _date = System.DateTime.Now;
                    //_edate = _date.ToString("dd/MM/yyyy");
                    _edate = MyUser.GerFormatedDatVal(_date);

                    _start = System.DateTime.Now;
                    //_sdate = _start.Date.ToString("01/MM/yyyy");
                    _sdate = MyUser.GerFormatedDatVal(new DateTime(_start.Year, _start.Month, 1));

                }
                else
                {

                    int year = findYearofSelectedMonth(selected_month);
                    _start = MyAttendance.GetFirstedDayFromMonth(selected_month);

                    //_sdate = _start.Date.ToString("01/MM/" + year + "");
                    _sdate = MyUser.GerFormatedDatVal(new DateTime(year, _start.Month, 1));

                    _date = MyAttendance.GetLastDateFromMonth(selected_month);
                    //_edate = _date.ToString("dd/MM/" + year + "");
                    _edate = MyUser.GerFormatedDatVal(new DateTime(year, _date.Date.Month, _date.Date.Day));

                }


                Txt_SDate.Enabled = false;
                Txt_EDate.Enabled = false;
                Txt_SDate.Text = _sdate;
                Txt_EDate.Text = _edate;
            }
            else
            {

                Btn_show.Enabled = false;

                Txt_SDate.Text = "";
                Txt_EDate.Text = "";

                if (Drp_Select_Month.SelectedItem.ToString() != "NO DATA")
                {
                    _Msg = "Select a month ";
                }
                else
                {
                    _Msg = "No attendance reported";
                }
            }
        }
        private int findYearofSelectedMonth(int selected_month)
        {
            int year = 0;
            if (DateTime.Now.Date.Month < selected_month)
            {
                year = DateTime.Now.Date.Year - 1;
            }
            else
            {
                year = DateTime.Now.Date.Year;
            }

            return year;
        }
        private void LoadDrpMonth()
        {

            Drp_Select_Month.Items.Clear();


            string Sql = "select tblmonth.Id , tblmonth.`Month` from tblmonth order by tblmonth.Id";
            MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Select_Month.Items.Add(li);


                }

                Drp_Select_Month.SelectedIndex = 0;
            }
            else
            {
                ListItem li = new ListItem("No Month Found", "-1");

                Drp_Select_Month.Items.Add(li);

            }



        }
        protected void Drp_Select_Month_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _Msg;
            this.GridDiv.InnerHtml = "";

            LoadDateFromSlectedDate(out _Msg);
        }
        protected void Btn_show_Click(object sender, EventArgs e)
        {
            string _msg = "";
            Btn_Excel.Enabled = true;
            this.GridDiv.InnerHtml = "";
            Lbl_Err.Text = "";
            if (Txt_EDate.Text != "" && Txt_SDate.Text != "")
            {


                int selected_month = int.Parse(Drp_Select_Month.SelectedValue.ToString());
                string _startDate = Txt_SDate.Text.ToString();
                string _EndDate = Txt_EDate.Text.ToString();

                //below fun is used to check the manual entry date is exist in current month
                if (CheckDatesInExistSelectedMonth(selected_month, Drp_Select_Period.SelectedItem.ToString(), out _msg))
                {
                    // second parameter is used to check the module is sudent manager or class manager

                    if (!FillAttendenceGrid( _startDate, _EndDate))
                    {
                        Lbl_Err.Text = "No data found for report";
                    }
                }
                else
                {
                    Lbl_Err.Text = _msg;
                }
            }
            else
            {
                if (Drp_Select_Period.SelectedItem.Text == "Manual")
                {
                    Lbl_Err.Text = "Enter Date";

                }
                else
                {
                    Lbl_Err.Text = "Select a period ";
                }
            }
        }
        protected void Btn_Excel_Click(object sender, EventArgs e)
        {
            if (this.GridDiv.InnerHtml == "")
            {
                Lbl_Err.Text = "No Data Found For Exporting To Excel";
            }
            else
            {
                Response.ContentType = "application/force-download";
                Response.AddHeader("content-disposition", "attachment; filename=StaffAttendanceReport.xls");
                Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
                Response.Write("<head>");
                Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                Response.Write("<!--[if gte mso 9]><xml>");
                Response.Write("<x:ExcelWorkbook>");
                Response.Write("<x:ExcelWorksheets>");
                Response.Write("<x:ExcelWorksheet>");
                Response.Write("<x:Name>Year Attendance Report</x:Name>");
                Response.Write("<x:WorksheetOptions>");
                Response.Write("<x:Print>");
                Response.Write("<x:ValidPrinterInfo/>");
                Response.Write("</x:Print>");
                Response.Write("</x:WorksheetOptions>");
                Response.Write("</x:ExcelWorksheet>");
                Response.Write("</x:ExcelWorksheets>");
                Response.Write("</x:ExcelWorkbook>");
                Response.Write("</xml>");
                Response.Write("<![endif]--> ");
                Response.Write(this.GridDiv.InnerHtml);
                Response.Write("</head>");
                Response.Flush();
                Response.End();
            }
        }
        private bool CheckDatesInExistSelectedMonth(int selected_month, string _Type, out string _msg)
        {

            bool _exist = false;
            _msg = "";
            if (selected_month != -1)
            {
                DateTime _enddate, _startDate, _s_Text_Date, _e_Text_Date;

                _startDate = MyAttendance.GetFirstedDayFromMonth(selected_month);
                _enddate = MyAttendance.GetLastDateFromMonth(selected_month);

                _s_Text_Date = MyUser.GetDareFromText(Txt_SDate.Text);

                _e_Text_Date = MyUser.GetDareFromText(Txt_EDate.Text);

                TimeSpan _totalday = _e_Text_Date.Subtract(_s_Text_Date);

                if (_Type == "Manual")
                {
                    if (_s_Text_Date <= _e_Text_Date)
                    {
                        if (_totalday.Days <= 31)
                        {

                            _exist = true;
                        }
                        else
                        {
                            _msg = "Total days is greater than 31 days  ";
                            _exist = false;
                        }
                    }
                    else
                    {
                        _msg = "Start date is greater than end date. ";
                        _exist = false;

                    }
                }
                else
                {
                    _exist = true;
                }
            }

            return _exist;

        }
        private bool FillAttendenceGrid(string _startDate, string _EndDate)
        {

            string _msg = "";
            int i = 0;
            bool FirstDone = false;
            string Thead = "", Trows = "";
            string[] Dates = new string[40];
            string[] AttData = new string[40];
            string Tbstr = "<table width=\"100%\" cellspacing=\"0\">  ";
            //string StandardId = MyAttendance.GetStandard_Class(int.Parse(Drp_ClassSelectDetailed.SelectedValue));

            string sql = "SELECT tbluser.Id,tbluser.SurName as StaffName from tblstaffdetails inner join tbluser on tblstaffdetails.UserId= tbluser.Id inner join tblrole on tbluser.RoleId= tblrole.Id where tbluser.Status=1 order by tbluser.SurName ";
            MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {

                    MyAttendance.StaffAttendanceReport(int.Parse(MyReader.GetValue(0).ToString()), _startDate, _EndDate, out _msg, out Dates, out AttData);
                    if (!FirstDone)
                    {
                        Thead = "<tr> <td class=\"TableHeaderStyle\"> Staff Name </td>";
                        for (i = 0; i < Dates.Length; i++)
                        {
                            if (Dates[i] != null)
                            {
                                Thead = Thead + "<td  class=\"TableHeaderStyle\">" + Dates[i] + " </td>";
                            }
                            else
                            {
                                break;
                            }
                        }
                        Thead = Thead + " </tr>";
                        FirstDone = true;
                    }
                    Trows = Trows + "<tr>  <td class=\"SubHeaderStyle\">  " + MyReader.GetValue(1).ToString() + "   </td> ";

                    for (i = 0; i < AttData.Length; i++)
                    {

                        if (AttData[i] != null)
                        {
                            Trows = Trows + " <td class=\"CellStyle\">   " + AttData[i] + " </td>";
                        }
                        else
                        {
                            break;
                        }
                    }
                    Trows = Trows + " </tr>";
                }
            }
            Tbstr = Tbstr + Thead + Trows + "</table>";
            this.GridDiv.InnerHtml = Tbstr;
            return FirstDone;
        }
    }
}