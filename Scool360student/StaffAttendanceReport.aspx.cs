using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using WinBase;
using System.Text;
using WebChart;
using System.Drawing;
using System.Data;

namespace WinEr
{
    public partial class StaffAttendanceReport : System.Web.UI.Page
    {
        private StaffManager MyStaffMang;
        private KnowinUser MyUser;
        private OdbcDataReader m_MyReader = null;
        private DataSet  m_MyDataSet=null;
        private Incident MyIncident;
        private Attendance MyAttendence;
       // private DataSet MyDataSet;
        string Sql;
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
            if (Session["StaffId"] == null)
            {
                Response.Redirect("ViewStaffs.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStaffMang = MyUser.GetStaffObj();
            MyIncident = MyUser.GetIncedentObj();
            MyAttendence = MyUser.GetAttendancetObj();
            if (MyStaffMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MyIncident == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(83))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {


                if (!IsPostBack)
                {
                    string _SubMenuStr;
                    _SubMenuStr = MyStaffMang.GetSubStaffMangMenuString(MyUser.UserRoleId);
                    this.SubStaffMenu.InnerHtml = _SubMenuStr;
                    LoaduserTopData();
                    LoadYearlyAttendanceReport();
                    LoadDrpMonth();
                    Load_DetailedAttendance();
                }
               
            }

        }


        private void LoaduserTopData()
        {
           
            string _Studstrip = MyStaffMang.ToStripString(int.Parse(Session["StaffId"].ToString()),  "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StaffId"].ToString()) + "&type=StaffImage");
            this.StudentTopStrip.InnerHtml = _Studstrip;
        }

        #region Detailed

        private void LoadDrpMonth()
        {
            Sql = "SELECT DISTINCT tblmonth.Id , tblmonth.`Month` from tblmonth INNER JOIN tblstaffattendance ON MONTH(tblstaffattendance.MarkDate)=tblmonth.Id ORDER BY YEAR(tblstaffattendance.MarkDate),MONTH(tblstaffattendance.MarkDate)";
            OdbcDataReader MyReader = MyAttendence.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Month.Items.Add(li);


                }

                Drp_Month.SelectedValue = DateTime.Now.Month.ToString();
            }
            else
            {
                ListItem li = new ListItem("No Month Found", "-1");

                Drp_Month.Items.Add(li);

            }
        }

        protected void Drp_Month_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_DetailedAttendance();
        }

        private void Load_DetailedAttendance()
        {
            string MonthId = Drp_Month.SelectedValue;
            if (MonthId != "-1")
            {
                string DetailedStr = GetAttendanceDetailedStr();
                DivDetails.InnerHtml = DetailedStr;
            }
            else
            {
                DivDetails.InnerHtml = "";
            }
        }

        private string GetAttendanceDetailedStr()
        {
            string DetailedStr = "";
            int MonthId = int.Parse(Drp_Month.SelectedValue);
            string RowStr = "";
            DateTime SelectedDate = GetStartDate(MonthId);
            while (SelectedDate.Month == MonthId)
            {
                string InTime = "";
                string OutTime = "";
                string PresentHours = "";
                string Status = "";
                string style = "";

                if(MyAttendence.IsDateHoliday(0,SelectedDate))
                {
                    Status = "Holiday";
                    InTime = "_:_:_";
                    OutTime = "_:_:_";
                    PresentHours = "_:_:_"; 
                    style = "style=\"color:Orange\"";
                }
                else if (MyAttendence.StaffAttendanceEnteredOnDay(Session["StaffId"].ToString(), SelectedDate))
                {

                    int AttendanceStatus = MyAttendence.GetStaffAttedanceStatus_OneStaff(Session["StaffId"].ToString(), SelectedDate, out InTime, out OutTime, out PresentHours);
                    if (AttendanceStatus == 0)
                    {
                        Status = "Absent";
                        InTime = "_:_:_";
                        OutTime = "_:_:_";
                        PresentHours = "_:_:_"; 
                        style = "style=\"color:Red\"";
                    }
                    else if (AttendanceStatus == 3)
                    {
                        Status = "Full Day";
                        style = "style=\"color:Green\"";
                    }
                    else
                    {
                        Status = "Half Day";
                        style = "style=\"color:Green\"";
                    }
                }
                else
                {
                    Status = "No Marked";
                    InTime = "_:_:_";
                    OutTime = "_:_:_";
                    PresentHours = "_:_:_"; 
                    style = "style=\"color:Gray\"";
                }
                RowStr = RowStr + " <tr>  <td class=\"HeaderStyle\" style=\"color:Black\">  " + General.GerFormatedDatVal(SelectedDate) + "  </td>   <td class=\"CellStyle\" " + style + " >  " + Status + " </td>  <td class=\"CellStyle\">   " + InTime + "  </td> <td class=\"CellStyle\">  " + OutTime + " </td>  <td class=\"CellStyle\">  " + PresentHours + " </td>   </tr> ";
                SelectedDate = SelectedDate.AddDays(1);
            }

            DetailedStr = "<table width=\"100%\" cellspacing=\"0\">  <tr>   <td class=\"SubHeaderStyle\">    Date  </td>  <td class=\"SubHeaderStyle\">  Attendance Status  </td>   <td class=\"SubHeaderStyle\">   In Time    </td>  <td class=\"SubHeaderStyle\">   out Time  </td>   <td class=\"SubHeaderStyle\">  Present Hours  </td> </tr> " + RowStr + " </table>";

            return DetailedStr;
        }

        private DateTime GetStartDate(int MonthId)
        {
            DateTime SelectedDate = new DateTime();
            string sql = "SELECT MAX(tblstaffattendance.MarkDate) FROM tblstaffattendance WHERE MONTH(tblstaffattendance.MarkDate)=" + MonthId;
            m_MyReader = MyAttendence.m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                SelectedDate = DateTime.Parse(m_MyReader.GetValue(0).ToString());
            }
            SelectedDate = new DateTime(SelectedDate.Year, SelectedDate.Month, 01);
            return SelectedDate;
        }

        #endregion


        #region Yearly

        private void LoadYearlyAttendanceReport()
        {
            
            string StaffSttedanceStr = GetStaffAttendanceString();
            divGrid.InnerHtml = StaffSttedanceStr;
        }

        private string GetStaffAttendanceString()
        {
            DateTime StartDate, EndDate;
            int StaffId = int.Parse(Session["StaffId"].ToString());
            MyAttendence.GetbatchDates(MyUser.CurrentBatchId, out StartDate, out EndDate);
            string Str = "";
            string Row = "";
            int TOTALWorkingDays = 0, TOTALFullDays = 0, TOTALHalfDays = 0, TOTALAbsent = 0, TOTALApprovedLeave = 0, TOTALOtherLeave = 0;
            double TOTALPercentage = 0;
            int i = 0;
            string sql = "SELECT DISTINCT tblmonth.Id , tblmonth.`Month`, YEAR(tblstaffattendance.MarkDate) from tblmonth INNER JOIN tblstaffattendance ON MONTH(tblstaffattendance.MarkDate)=tblmonth.Id ORDER BY YEAR(tblstaffattendance.MarkDate),MONTH(tblstaffattendance.MarkDate)";
            m_MyReader = MyAttendence.m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {

                    string MonthId = m_MyReader.GetValue(0).ToString();
                    string Month = m_MyReader.GetValue(1).ToString().ToUpperInvariant();
                    string YearId = m_MyReader.GetValue(2).ToString();

                    int WorkingDays = 0, FullDays = 0, HalfDays = 0, Absent = 0, ApprovedLeave = 0, OtherLeave = 0;

                    double Percentage = 0;
                    GetMonthlyStaffAttendanceDetails(StaffId, MonthId, YearId, out WorkingDays, out FullDays, out HalfDays, out Absent, out ApprovedLeave, out OtherLeave);

                    if (HalfDays != 0)
                    {
                        Percentage = (double)(FullDays + (double)(HalfDays / 2)) * 100;
                    }
                    else
                    {
                        Percentage = (double)(FullDays) * 100;
                    }
                    if (WorkingDays != 0)
                    {
                        Percentage = Percentage / WorkingDays;
                    }

                    TOTALWorkingDays = TOTALWorkingDays + WorkingDays;
                    TOTALFullDays = TOTALFullDays + FullDays;
                    TOTALHalfDays = TOTALHalfDays + HalfDays;
                    TOTALAbsent = TOTALAbsent + Absent;
                    TOTALApprovedLeave = TOTALApprovedLeave + ApprovedLeave;
                    TOTALOtherLeave = TOTALOtherLeave + OtherLeave;
                    TOTALPercentage = TOTALPercentage + Percentage;

                    i++;

                    Row = Row + " <tr>    <td class=\"HeaderStyle\"  style=\"color:Black\"> " + Month + " </td> <td  class=\"CellStyle\">   " + WorkingDays + "  </td>   <td  class=\"CellStyle\">   " + FullDays + "  </td>  <td  class=\"CellStyle\">  " + HalfDays + "  </td> <td  class=\"CellStyle\"> " + Absent + "  </td>  <td  class=\"CellStyle\">    " + ApprovedLeave + "  </td>   <td  class=\"CellStyle\">  " + OtherLeave + "   </td>  <td  class=\"CellStyle\">   " + Percentage.ToString("#0.00") + "%  </td>  </tr>";

                }

                if (Row != "" && i>0)
                {
                    TOTALPercentage = TOTALPercentage / i;
                    Row = Row + "<tr> <td class=\"SubHeaderStyle\">   Total </td>   <td  class=\"SubHeaderStyle\">  " + TOTALWorkingDays + " </td> <td  class=\"SubHeaderStyle\">  " + TOTALFullDays + "  </td>  <td  class=\"SubHeaderStyle\">  " + TOTALHalfDays + "  </td>   <td  class=\"SubHeaderStyle\">   " + TOTALAbsent + " </td>  <td  class=\"SubHeaderStyle\">   " + TOTALApprovedLeave + " </td>  <td  class=\"SubHeaderStyle\">    " + TOTALOtherLeave + "    </td> <td  class=\"SubHeaderStyle\">  " + TOTALPercentage.ToString("#0.00") + "% </td></tr>";
                }

            }

            if (Row != "")
            {
                Str = "<table width=\"100%\"  cellspacing=\"0\">  <tr>  <td class=\"SubHeaderStyle\">  </td>  <td  class=\"SubHeaderStyle\">    Working Days     </td>   <td  class=\"SubHeaderStyle\">   Full Day </td>   <td  class=\"SubHeaderStyle\">  Half Day  </td>  <td  class=\"SubHeaderStyle\">  Absent  </td>  <td  class=\"SubHeaderStyle\">  Approved Leave  </td>  <td  class=\"SubHeaderStyle\">  Other Leave   </td>     <td  class=\"SubHeaderStyle\">   Percentage   </td>   </tr>  " + Row + " </table>";
            }
            else
            {
                Str = " Staff attendance not marked for current batch";
            }
            return Str;
        }

        private void GetMonthlyStaffAttendanceDetails(int StaffId, string MonthId, string YearId, out int WorkingDays, out int FullDays, out int HalfDays, out int Absent, out int ApprovedLeave, out int OtherLeave)
        {
            WorkingDays = MyAttendence.GetNo_StaffAttendanceMarkedInMonth(MonthId, YearId);
            FullDays = MyAttendence.GetNo_FullDayAttendance_Month_Staff(StaffId, MonthId, YearId);
            HalfDays = MyAttendence.GetNo_HalfDayAttendance_Month_Staff(StaffId, MonthId, YearId);
            Absent = MyAttendence.GetNo_AbsentAttendance_Month_Staff(StaffId, MonthId, YearId);
            ApprovedLeave = 0;
            OtherLeave = 0;
        }

        #endregion



    }
}
