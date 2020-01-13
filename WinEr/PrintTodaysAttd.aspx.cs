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
    public partial class PrintTodaysAttd : System.Web.UI.Page
    {
        private ClassOrganiser MyClassMang;
        private StudentManagerClass MyStudMang;
        private Attendance MyAttendance;
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
            MyClassMang = MyUser.GetClassObj();
            MyAttendance = MyUser.GetAttendancetObj();
            MyStudMang = MyUser.GetStudentObj();
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
                    Load_Report();

                }
            }
        }

        private void Load_Report()
        {
            lbl_Date.Text = General.GerFormatedDatVal(DateTime.Now.Date);
            lbl_Time.Text = DateTime.Now.TimeOfDay.ToString().Substring(0, 8);
            string str = "";
            string _msg = "";
            if (IsReportPossible(out _msg))
            {
                string ClassAttendanceStr = GetClass_AttendStr();

                str = ClassAttendanceStr;
            }
            else
            {

                str = _msg;

            }

            Div_Report.InnerHtml = str;
        }

        private string GetClass_AttendStr()
        {
            string _classstr = "<table width=\"800px\">";

            string sql = "SELECT tblclass.Id,tblclass.ClassName,tblstandard.Id from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id,tblclass.ClassName";
            OdbcDataReader my_Reader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (my_Reader.HasRows)
            {
                while (my_Reader.Read())
                {
                    string _ClassId = my_Reader.GetValue(0).ToString();
                    string _ClassName = my_Reader.GetValue(1).ToString();
                    string _StandardId = my_Reader.GetValue(2).ToString();
                    _classstr = _classstr + " <tr>  <td> <br/> <br/> " + Get_EachClassString(_ClassId, _ClassName, _StandardId) + "</td>    </tr>";

                }
            }

            return _classstr + "</table>";
        }
        private string Get_EachClassString(string _ClassId, string _ClassName, string _StandardId)
        {
            string _classstr = "";


            int _StudentsCount = MyClassMang.GetTotalNumberOfStudentsInClass(int.Parse(_ClassId), MyUser.CurrentBatchId);
            int Present_StudentsCount = GetAttendanceCount(_ClassId, _StandardId);
            string _studentStr = "";
            if (_StudentsCount == 0)
            {
                _studentStr = "<td  colspan=\"2\" align=\"center\" valign=\"middle\" class=\"StudentRow\"> No students in the class </td>";
            }
            else if (_StudentsCount <= Present_StudentsCount)
            {
                _studentStr = "<td  colspan=\"2\" align=\"center\" valign=\"middle\" class=\"StudentRow\">All Students are present</td>";
            }
            else
            {
                if (MyAttendance.AttendanceTables_Exits(_StandardId, MyUser.CurrentBatchId))
                {

                    string Sql = "SELECT DISTINCT t2.StudentId,tblstudentclassmap.RollNo FROM tblattdcls_std" + _StandardId + "yr" + MyUser.CurrentBatchId + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + MyUser.CurrentBatchId + " t2 ON t2.ClassAttendanceId=t1.Id INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=t2.StudentId WHERE t2.PresentStatus=0 AND t1.ClassId=" + _ClassId + " AND t1.Date='" + DateTime.Now.Date.ToString("s") + "' ORDER BY tblstudentclassmap.RollNo";
                    OdbcDataReader m_MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(Sql);
                    if (m_MyReader.HasRows)
                    {
                        int _count = 1;
                        while (m_MyReader.Read())
                        {
                            string RollNo = m_MyReader.GetValue(1).ToString();
                            string _studName = MyUser.getStudName(int.Parse(m_MyReader.GetValue(0).ToString()));
                            _studentStr = _studentStr + "<td align=\"left\" valign=\"middle\" class=\"StudentRow\" >  &nbsp;&nbsp  " + RollNo + " . " + _studName + "  </td>";
                            if (_count % 2 == 0)
                            {
                                _studentStr = _studentStr + "</tr><tr>";
                            }
                            _count++;
                        }
                        if (_count % 2 == 0)
                        {
                            _studentStr = _studentStr + "<td align=\"center\" valign=\"middle\" class=\"StudentRow\" >    </td>  ";
                        }
                    }
                    else
                    {
                        _studentStr = "<td colspan=\"2\" align=\"center\" valign=\"middle\" class=\"StudentRow\">Attendance not marked</td>";
                    }


                }
                else
                {
                    _studentStr = "<td colspan=\"2\" align=\"center\" valign=\"middle\" class=\"StudentRow\">Attendance not marked</td>";
                }
            }

            if (_studentStr != "")
            {
                _classstr = " <table width=\"100%\" cellspacing=\"0\">  <tr>   <td align=\"center\" valign=\"middle\" class=\"ClassHeading\" >    ClassName : " + _ClassName + "   </td> <td align=\"center\" valign=\"middle\" class=\"ClassHeading\" > Attendance : " + Present_StudentsCount + "/" + _StudentsCount + "  </td>     </tr> <tr>    <td align=\"left\" colspan=\"2\" class=\"StudentRow\" style=\"font-size:14px;color:Black;text-decoration:underline;padding-left:10px;\">  Absent List  </td></tr><tr>" + _studentStr + " </tr>   <tr>  <td colspan=\"2\" class=\"StudentRow\">     <table width=\"100%\"><tr> <td align=\"right\" style=\"height:30px\">  Verified &amp; Approved By&nbsp;&nbsp;  </td></tr> <tr> <td  style=\"height:30px\"> </td>    </tr><tr>  <td  align=\"right\" style=\"height:30px\">  Signature&nbsp;&nbsp;   </td>    </tr>   </table>   </td>  </tr>   </table>  ";
            }
            return _classstr;
        }

        private int GetAttendanceCount(string _ClassId, string _StandardId)
        {
            int _count = 0;
            if (MyAttendance.AttendanceTables_Exits(_StandardId, MyUser.CurrentBatchId))
            {
                string Sql = "SELECT Count(DISTINCT t2.StudentId) FROM tblattdcls_std" + _StandardId + "yr" + MyUser.CurrentBatchId + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + MyUser.CurrentBatchId + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.PresentStatus<>0 AND t1.ClassId=" + _ClassId + " AND t1.Date='" + DateTime.Now.Date.ToString("s") + "'";
                OdbcDataReader m_MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(Sql);
                if (m_MyReader.HasRows)
                {
                    _count = int.Parse(m_MyReader.GetValue(0).ToString());

                }
            }
            return _count;
        }

        private bool IsReportPossible(out string _msg)
        {
            _msg = "Attendance Not Yet Mark For Today";
            bool valid = false;

            int BatchId = MyUser.CurrentBatchId;
            int StandardId = 0;

            string sql = "SELECT Id,Name FROM  tblstandard";
            OdbcDataReader m_MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {

                    StandardId = 0;
                    int.TryParse(m_MyReader.GetValue(0).ToString(), out StandardId);
                    if (StandardId > 0)
                    {
                        if (MyAttendance.AttendanceTables_Exits(StandardId.ToString(), BatchId))
                        {
                            if (IsAttendanceAlreadyMarked(DateTime.Now.Date, StandardId.ToString(), BatchId))
                            {
                                valid = true;
                            }

                        }
                    }
                }

            }


            return valid;
        }

        public bool IsAttendanceAlreadyMarked(DateTime M_StartTime, string StandardId, int YearId)
        {
            bool valid = false;
            int ClassAttendanceId = 0;
            string Sql = "SELECT Id FROM tblattdcls_std" + StandardId + "yr" + YearId + " WHERE Date='" + M_StartTime.Date.ToString("s") + "'";

            OdbcDataReader m_MyReader1 = MyAttendance.m_MysqlDb.ExecuteQuery(Sql);

            if (m_MyReader1.HasRows)
            {

                if (int.TryParse(m_MyReader1.GetValue(0).ToString(), out ClassAttendanceId))
                {
                    if (ClassAttendanceId > 0)
                    {
                        valid = true;
                    }
                }

            }
            return valid;
        }
    }
}
