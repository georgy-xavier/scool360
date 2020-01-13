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
    public partial class StaffWorkLoadReport : System.Web.UI.Page
    {
        private TimeTable MyTimeTable;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyTimeTable = MyUser.GetTimeTableObj();
            if (MyTimeTable == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(621))
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else
            {
                if (!IsPostBack)
                {
                    //some initlization

                    Load_Report();

                }
            }

        }

        private void Load_Report()
        {
            Lbl_msg.Text = "";
            DataSet StaffReport = new DataSet();
            DataTable dt;
            StaffReport.Tables.Add(new DataTable("Staff"));
            dt = StaffReport.Tables["Staff"];
            dt.Columns.Add("Id");
            dt.Columns.Add("StaffName");
            dt.Columns.Add("PeriodCount");
            dt.Columns["PeriodCount"].DataType = System.Type.GetType("System.Int32");

            int PeriodCount = 0;
            int MaxPeriod = int.Parse(MyTimeTable.GetTeacherMaxPeriod());
            string StaffId = "", StaffName = "";
            string MainStr = "<table width=\"100%\" cellspacing=\"10\"> <tr> <td>    <table width=\"100%\" cellspacing=\"0\"><tr class=\"BorderStyle\"> <td class=\"HeadStaffStyle\">  Staff Name </td> <td class=\"HeadProgressBar\">   Work Load Percent </td> <td style=\"width:5%;\" align=\"center\" valign=\"middle\" > </td>	 <td> </td>	</tr>	  </table>  </td>  </tr><tr>  ";
            string TRstr="", TdStr = "";
            string sql = "SELECT DISTINCT tbltime_classsubjectstaff.StaffId,tbluser.SurName FROM tbltime_classsubjectstaff INNER JOIN tbluser ON  tbluser.Id=tbltime_classsubjectstaff.StaffId";
            OdbcDataReader MyReadernew = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            if (MyReadernew.HasRows)
            {
                while (MyReadernew.Read())
                {
                    PeriodCount = 0;
                    StaffId = MyReadernew.GetValue(0).ToString();
                    StaffName = MyReadernew.GetValue(1).ToString();
                    PeriodCount = MyTimeTable.GetStaff_PeriodCount(StaffId);
                    TdStr = TdStr + TRstr + GetStaffDetails(StaffId, StaffName, MaxPeriod, PeriodCount);
                    TRstr = "</tr><tr>";
                    DataRow dr = dt.NewRow();
                    dr["Id"] = StaffId;
                    dr["StaffName"] = StaffName;
                    dr["PeriodCount"] = PeriodCount;
                    StaffReport.Tables["Staff"].Rows.Add(dr);
                }
                MainStr = MainStr + TdStr + "   </tr> </table>";
               
            }
            else
            {
                Lbl_msg.Text = "Time Table Not Configured";
                MainStr = "";
            }
            DivStaff.InnerHtml = MainStr;
            ViewState["Dataset"] = StaffReport;
        }

        private string GetStaffDetails(string StaffId, string StaffName, int MaxPeriod, int PeriodCount)
        {
            
            string ProgressStr = "<div class=\"ProgressStyle\" > <table width=\"100%\"  cellspacing=\"0\"   title=\"" + PeriodCount + "\">  <tr>   ";
            int Percent = 0;
            int RemainingPercent = 0;
            string SecondTd = "", occupancyStyle = "", TdValue = "";
            if (MaxPeriod > 0)
            {
                Percent = (PeriodCount * 100) / MaxPeriod;
            }
            if (Percent == 0)
            {
                occupancyStyle = "EmptyStyle";
                TdValue = " <td class=\"" + occupancyStyle + "\" style=\"width:100%\"> &nbsp;  </td>";
            }
            else if (Percent == 100)
            {
                occupancyStyle = "FullOccupied";
                TdValue = " <td class=\"" + occupancyStyle + "\" style=\"width:" + Percent + "%\"> &nbsp;  </td>";
            }
            else if (Percent > 100)
            {
                occupancyStyle = "OverOccupied";
                TdValue = " <td class=\"" + occupancyStyle + "\" style=\"width:100%\"> &nbsp;  </td> ";
            }
            else
            {

                RemainingPercent = 100 - Percent;
                occupancyStyle = "HalfOccupied";
                SecondTd = "EmptyStyle";
                TdValue = " <td class=\"" + occupancyStyle + "\" style=\"width:" + Percent + "%\"> &nbsp;  </td> <td  class=\"" + SecondTd + "\" style=\"width:" + RemainingPercent + "%\"> &nbsp; </td> ";

            }
          
            ProgressStr = ProgressStr + TdValue + " </tr>    </table>  </div>";


            string TdStr = "<td>  <table width=\"100%\" cellspacing=\"0\"><tr class=\"BorderStyle\"> <td class=\"StaffStyle\">   " + StaffName + " </td> <td class=\"ProgressBar\">  " + ProgressStr + "  </td>	 <td style=\"width:5%;\" align=\"center\" valign=\"middle\" >  <div class=\"CountStyle\">    " + PeriodCount + "/" + MaxPeriod + "	  </div>  </td> <td>	 </td>	</tr>	   </table>   </td> ";
            return TdStr;
            
        }

       

        protected void Drp_SortType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_ReportFrom_Grid(Drp_SortType.SelectedValue);
        }

        private void Load_ReportFrom_Grid(string SelectedValue)
        {
            Lbl_msg.Text = "";
            string sortDirection = "ASC";//DESC  ASC
            string Expression = "PeriodCount";
            MydataSet=(DataSet)ViewState["Dataset"];
            if (MydataSet != null && MydataSet.Tables[0] != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                int PeriodCount = 0;
                int MaxPeriod = int.Parse(MyTimeTable.GetTeacherMaxPeriod());
                string StaffId = "", StaffName = "";
                string MainStr = "<table width=\"100%\" cellspacing=\"10\"> <tr> <td>    <table width=\"100%\" cellspacing=\"0\"><tr class=\"BorderStyle\"> <td class=\"HeadStaffStyle\">  Staff Name </td> <td class=\"HeadProgressBar\">   Work Load Percent </td> <td style=\"width:5%;\" align=\"center\" valign=\"middle\" > </td>	 <td> </td>	</tr>	  </table>  </td>  </tr><tr>  ";
                string TRstr = "", TdStr = "";
                DataTable table = MydataSet.Tables[0];
                if (SelectedValue == "-1")
                {
                    table = MydataSet.Tables[0];
                }
                else if (SelectedValue == "0")
                {
                    Expression = "StaffName";
                    DataView dataView = new DataView(MydataSet.Tables[0]);
                    dataView.Sort = Expression + " " + sortDirection;
                    table = dataView.ToTable();
                }
                else if (SelectedValue == "1")
                {
                    sortDirection = "DESC";
                    Expression = "PeriodCount";
                    DataView dataView = new DataView(MydataSet.Tables[0]);
                    dataView.Sort = Expression + " " + sortDirection;
                    table = dataView.ToTable();
                }
                else if (SelectedValue == "2")
                {
                    sortDirection = "ASC";
                    Expression = "PeriodCount";
                    DataView dataView = new DataView(MydataSet.Tables[0]);
                    dataView.Sort = Expression + " " + sortDirection;
                    table = dataView.ToTable();
                }
                foreach (DataRow dr in table.Rows)
                {


                    StaffId = dr[0].ToString();
                    StaffName = dr[1].ToString();
                    PeriodCount = int.Parse(dr[2].ToString());
                    TdStr = TdStr + TRstr + GetStaffDetails(StaffId, StaffName, MaxPeriod, PeriodCount);
                    TRstr = "</tr><tr>";

                }
                MainStr = MainStr + TdStr + "   </tr> </table>";
                DivStaff.InnerHtml = MainStr;
                if (MydataSet != null && MydataSet.Tables[0] != null)
                    MydataSet.Tables.RemoveAt(0);
                MydataSet.Tables.Add(table);
                ViewState["Dataset"] = MydataSet;
            }
            else
            {
                Lbl_msg.Text = "Time Table Not Configured";
            }
        }

        protected void Btn_Export_Click(object sender, ImageClickEventArgs e)
        {
            string _ReportName = "Staff Work Load Report";
            string FileName = "StaffWorkLoadReport";
            MydataSet = (DataSet)ViewState["Dataset"];
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count>0)
            {
                MydataSet.Tables[0].Columns.Remove(MydataSet.Tables[0].Columns[0]);
                if (!WinEr.ExcelUtility.ExportDataToExcel(MydataSet, _ReportName, FileName, MyUser.ExcelHeader))
                {
                    Lbl_msg.Text = "You may missing Excel or try again later";
                }
            }
            else
            {
                Lbl_msg.Text = "No Data To Export";
            }
        }
    }
}
