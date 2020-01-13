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
    public partial class LoacateStaff : System.Web.UI.Page
    {
        private TimeTable MyTimeTable;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        private StaffManager MyStaffMang;

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
            MyTimeTable = MyUser.GetTimeTableObj();
            MyStaffMang = MyUser.GetStaffObj();
            if (MyStaffMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (MyTimeTable == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (Session["StaffId"] == null)
            {
                Response.Redirect("ViewStaffs.aspx");
            }
            if (!MyUser.HaveActionRignt(622))
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else
            {
                if (!IsPostBack)
                {
                    //some initlization
                    string _SubMenuStr;
                    _SubMenuStr = MyStaffMang.GetSubStaffMangMenuString(MyUser.UserRoleId);
                    this.SubStaffMenu.InnerHtml = _SubMenuStr;
                    int StaffId = int.Parse(Session["StaffId"].ToString());
                    Load_StaffLocations(StaffId);
                    LoaduserTopData();

                }
            }

        }


        private void LoaduserTopData()
        {

            string _Studstrip = MyStaffMang.ToStripString(int.Parse(Session["StaffId"].ToString()), "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StaffId"].ToString()) + "&type=StaffImage");
            this.StudentTopStrip.InnerHtml = _Studstrip;
        }

        private void Load_StaffLocations(int StaffId)
        {
            Lbl_Error.Text = "";
            StaffDetails.InnerHtml = "";
            MydataSet= MyTimeTable.GetDays();
            bool _continue = false;
            string DayId = "";

            // Commented    This portion allows to locate staff on only available days ,, if need uncommet and comment below to uncommented lines
            //foreach (DataRow dr in MydataSet.Tables[0].Rows)
            //{
            //    if (DateTime.Now.Date.DayOfWeek.ToString().ToLowerInvariant() == dr[0].ToString().ToLowerInvariant())
            //    {
            //        DayId = dr[1].ToString();
            //        _continue = true;
            //    }
            //}
            DayId=  MyTimeTable.GetDayId_Date(DateTime.Now.Date).ToString();
           _continue = true;

            if (_continue)
            {
                string MainStr = " <table  class=\"MyTable\" border=\"1\" cellpadding=\"1\" cellspacing=\"10\">  <tr>  ";
                string StrPeriods = "";
                int i = 1;
                MydataSet = new DataSet();
                string sql = "SELECT PeriodId,FrequencyName,FromTime,ToTime FROM tblattendanceperiod WHERE ModeId=3 AND  FromTime<>0";
                MydataSet = MyTimeTable.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drNew in MydataSet.Tables[0].Rows)
                    {
                        StrPeriods = StrPeriods + GetPeriodString(drNew[0].ToString(), drNew[1].ToString(), drNew[2].ToString(), drNew[3].ToString(), StaffId, DayId);

                        if (i % 4 == 0)
                        {
                            StrPeriods = StrPeriods + "</tr><tr>";
                        }
                        i++;
                    }
                }
                MainStr = MainStr + StrPeriods + " </tr> </table>";
                StaffDetails.InnerHtml = MainStr;
            }
            else
            {
                Lbl_Error.Text = "Staff Details Not Found";
            }
            
        }

        private string GetPeriodString(string PeriodId, string PeriodName, string StartTime, string EndTime, int StaffId,string DayId)
        {

            string strPeriod = "  ";
            bool ClassAssaingned = false;
            string ClassName = "", SubjectName = "";

          // Checking Temperory Assigment
            string sql = "SELECT tblclass.ClassName,tbltime_submaster.SubjectId,tbltime_submaster.ClassId FROM tbltime_submaster INNER JOIN tblclass ON tblclass.Id=tbltime_submaster.ClassId INNER JOIN tblattendanceperiod ON tblattendanceperiod.PeriodId=tbltime_submaster.PeriodId WHERE tbltime_submaster.PeriodId=" + PeriodId + " AND tbltime_submaster.`Day`='" + DateTime.Now.Date.ToString("s") + "' AND tbltime_submaster.StaffId=" + StaffId;
            MyReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                string SubjectId = MyReader.GetValue(1).ToString(); 
                ClassName = MyReader.GetValue(0).ToString();
                SubjectName = MyTimeTable.GetSubjectName(SubjectId);
                ClassAssaingned = true;
            }

            // Checking General Assigment
            if (!ClassAssaingned)
            {
                sql = "SELECT tblclass.ClassName,tbltime_master.SubjectId,tbltime_classperiod.ClassId FROM tbltime_master INNER JOIN tbltime_classperiod ON tbltime_classperiod.Id=tbltime_master.ClassPeriodId INNER JOIN tblclass ON tblclass.Id=tbltime_classperiod.ClassId INNER JOIN tblattendanceperiod ON tblattendanceperiod.PeriodId=tbltime_classperiod.PeriodId WHERE tbltime_classperiod.PeriodId=" + PeriodId + " AND tbltime_classperiod.DayId=" + DayId + " AND tbltime_master.StaffId=" + StaffId;
                MyReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    string SubjectId = MyReader.GetValue(1).ToString(); 
                    ClassName = MyReader.GetValue(0).ToString();
                    SubjectName = MyTimeTable.GetSubjectName(SubjectId);
                    ClassAssaingned = true;
                }
            }

            TimeSpan FromTime = TimeSpan.Parse(StartTime), ToTime = TimeSpan.Parse(EndTime);
            if (DateTime.Now.TimeOfDay > FromTime && DateTime.Now.TimeOfDay < ToTime)
            {
                if (ClassAssaingned)
                {
                    strPeriod = "<td class=\"CurrentClassStyle\">   <table width=\"100%\"  cellspacing=\"5\"> <tr>  <td colspan=\"2\" align=\"center\" class=\"ClassHeadingStyle\">     " + PeriodName + "    </td>    </tr>  <tr>    <td align=\"right\">   Class:    </td>   <td class=\"RightStyle\">   " + ClassName + "  </td> </tr>  <tr>   <td align=\"right\">   Subject:  </td>   <td class=\"RightStyle\">  " + SubjectName + " </td> </tr> <tr>  <td align=\"right\"> From:   </td>   <td class=\"RightStyle\">  " + FromTime + "   </td> </tr>  <tr>  <td align=\"right\">    To:    </td>   <td class=\"RightStyle\">    " + ToTime + "  </td>  </tr>  </table>  </td> ";
                }
                else
                {
                    strPeriod = " <td class=\"CurrentClassStyle\"> <table width=\"100%\"  cellspacing=\"5\"> <tr> <td align=\"center\" class=\"ClassHeadingStyle\">     " + PeriodName + "    </td>    </tr> <tr> <td  align=\"center\" class=\"ClassHeadingStyle\">   Free Period  </td>  </tr> </table> </td>";
                }
            }
            else
            {
                if (ClassAssaingned)
                {
                    strPeriod = "<td class=\"OtherClassStyle\">   <table width=\"100%\"  cellspacing=\"5\"> <tr>  <td colspan=\"2\" align=\"center\" class=\"ClassHeadingStyle\">     " + PeriodName + "    </td>    </tr>  <tr>    <td align=\"right\">   Class:    </td>   <td class=\"RightStyle\">   " + ClassName + "  </td> </tr>  <tr>   <td align=\"right\">   Subject:  </td>   <td class=\"RightStyle\">  " + SubjectName + " </td> </tr> <tr>  <td align=\"right\"> From:   </td>   <td class=\"RightStyle\">  " + FromTime + "   </td> </tr>  <tr>  <td align=\"right\">    To:    </td>   <td class=\"RightStyle\">    " + ToTime + "  </td>  </tr>  </table>  </td> ";
                }
                else
                {
                    strPeriod = " <td class=\"FreeClassStyle\"> <table width=\"100%\"  cellspacing=\"5\"> <tr><td  align=\"center\" class=\"ClassHeadingStyle\">     " + PeriodName + "    </td>    </tr> <tr> <td  align=\"center\" class=\"ClassHeadingStyle\">   Free Period  </td>  </tr> </table> </td>";
                }
            }

            return strPeriod;
        }

      
    }
}
