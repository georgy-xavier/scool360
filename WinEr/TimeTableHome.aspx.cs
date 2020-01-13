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
using WinBase;
using DayPilot;
using DayPilot.Utils;
namespace WinEr
{
    public partial class TimeTableHome : System.Web.UI.Page
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
            else if (!MyUser.HaveActionRignt(408))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    //some initlization

                    Load_ClassView();
                }
                   
            }
           
        }

        private void Load_ClassView()
        {
            int count = 1;
            string MainStr = "<div class=\"garagedoor\" id=\"garagedoor\">	<table width=\"100%\" cellspacing=\"20\"> <tr>";

            string ClassMainStr = " ";


             MydataSet = MyUser.MyAssociatedClass();
             if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
             {

                 foreach (DataRow dr in MydataSet.Tables[0].Rows)
                 {
                     ClassMainStr=GetClassStr(dr[0].ToString(),dr[1].ToString());
                     MainStr = MainStr + "<td>" + ClassMainStr + "</td>";
                     if (count % 5 == 0)
                     {
                         MainStr = MainStr + "</tr><tr>";
                     }
                     count++;
                 }
             }

             MainStr = MainStr + "</tr> </table>	</div>";
             DivClass.InnerHtml = MainStr;
        }

        private string GetClassStr(string ClassId,string ClassName)
        {
            bool disable = false;
            string CellStyle = "";
            

            if (!MyTimeTable.IsClassAvailibilityStored(ClassId))
            {
                CellStyle = "Yellowoverlay";
                disable = true;
            }

            int SubjectCount = MyTimeTable.getTotalSubject_Class(ClassId);
            int StaffCount = MyTimeTable.getTotalStaff_Class(ClassId);
            int PeriodAlloted = MyTimeTable.getAllotedCount_Class(ClassId);
            string AvailablePeriod = MyTimeTable.GetTotalClassAllocationPeriods(ClassId);
            int FreePeriod = int.Parse(AvailablePeriod) - PeriodAlloted;

            if (!disable)
            {
                if (FreePeriod <= 5)
                {
                    CellStyle = "Greenoverlay";
                }
                else
                {
                    CellStyle = "Redoverlay";
                }
            }
            string page = "TimeTableView.aspx?ClassId=" + ClassId ;
            string StyleStr = "class=\"item\"";
            if (disable)
            {
                page = "#";
                StyleStr = "class=\"dummyitem\"";
            }
            string ClassMainStr = " <table class=\"ClassStyle\">  <tr>  <td colspan=\"2\">  <div title=\"" + ClassName + "\" " + StyleStr + " id=\"" + page + "\">  <div class=\"underlay\">	</div>	<div class=\"" + CellStyle + "\" > " + ClassName + " </div>	<div class=\"mouse\"><img src=\"images/nothing.gif\" alt=\"\"> &nbsp;</div>  </div>   </td>  </tr> <tr>   <td class=\"LeftStyle\">   Total Subjects :   </td>   <td class=\"RightStyle\">   " + SubjectCount + " </td>  </tr> <tr>  <td class=\"LeftStyle\">    Total Staffs :    </td>  <td class=\"RightStyle\">    " + StaffCount + " </td>  </tr>  <tr>  <td class=\"LeftStyle\">   Periods Allotted :   </td>  <td class=\"RightStyle\">  " + PeriodAlloted + "   </td>  </tr>	  <tr>  <td class=\"LeftStyle\">   Free Periods :   </td>  <td class=\"RightStyle\">  " + FreePeriod + "   </td>  </tr> </table>	 ";

            return ClassMainStr;
        }





    }
}
