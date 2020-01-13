using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.Odbc;
using WinBase;

namespace WinEr
{
    public partial class ClassStrengthReport : System.Web.UI.Page
    {
        private ClassOrganiser MyClassMang;
        private StudentManagerClass MyStudentMang;
        private Attendance MyAttendance;
        private KnowinUser MyUser;

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudentMang = MyUser.GetStudentObj();
            MyAttendance = MyUser.GetAttendancetObj();
            MyClassMang = MyUser.GetClassObj();
            if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (MyAttendance == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (MyStudentMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(866))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (!IsPostBack)
                {
                    Txt_StartDate.Text = General.GerFormatedDatVal(DateTime.Now.Date);
                    Txt_EndDate.Text = General.GerFormatedDatVal(DateTime.Now.Date);
                    LoadDetails();  
                }
            }
        }
        #endregion

        protected void Btn_Load_Click(object sender, EventArgs e)
        {
            LoadDetails();           
        }

        private void LoadDetails()
        {
            this.div_ClassStrength.InnerHtml = "";
            lbl_error.Text = "";

            string _msg = "";
            if (LoadingPossible(out _msg))
            {
                DateTime _Startdate = new DateTime(), _Enddate = new DateTime();
                _Startdate = General.GetDateTimeFromText(Txt_StartDate.Text);
                _Enddate = General.GetDateTimeFromText(Txt_EndDate.Text);
                Load_ClassStrength(_Startdate, _Enddate);
            }
            else
            {
                lbl_error.Text = _msg;
            }
        }


        private bool LoadingPossible( out string _msg)
        {
            bool _valid = true;
            _msg = "";
            DateTime _Startdate = new DateTime(), _Enddate = new DateTime();
            if (Txt_StartDate.Text == "" || Txt_EndDate.Text == "")
            {
                _msg = "Please enter date";
                _valid = false;
            }
            if (_valid)
            {
                _Startdate = General.GetDateTimeFromText(Txt_StartDate.Text);
                _Enddate = General.GetDateTimeFromText(Txt_EndDate.Text);
                if (_Startdate > _Enddate)
                {
                    _msg = "Start date should be less than end date";
                    _valid = false;
                }
            }
            if (_valid)
            {
                if (_Enddate.Date > DateTime.Now.Date)
                {
                    _msg = "Please donot enter date greater than today";
                    _valid = false;
                }
            }

            return _valid;
        }


        private void Load_ClassStrength(DateTime _Startdate, DateTime _EndDate)
        {
            DateTime _testdate = _Startdate;
            _testdate = _Startdate;
            int _count = 0;
            while (_testdate.Date <= _EndDate.Date)
            {
                _count = _count + 1;
                _testdate = _testdate.Date.AddDays(1);
            }
            int[] _totalarray = new int[_count];
            int _TotalStrenth = 0;
            string _classstr = "";
            string sql = "SELECT tblclass.Id,tblclass.ClassName,tblstandard.Id from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id,tblclass.ClassName";
            OdbcDataReader my_Reader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (my_Reader.HasRows)
            {
                while (my_Reader.Read())
                {
                    string _ClassId = my_Reader.GetValue(0).ToString();
                    string _ClassName = my_Reader.GetValue(1).ToString();
                    string _StandardId = my_Reader.GetValue(2).ToString();
                    int _StudentsCount = MyClassMang.GetTotalNumberOfStudentsInClass(int.Parse(_ClassId), MyUser.CurrentBatchId);
                    _TotalStrenth = _TotalStrenth + _StudentsCount;
                    string DateString = "";

                    _testdate = _Startdate;
                    _count = 0;
                    while (_testdate.Date <= _EndDate.Date)
                    {
                        int _attdcount = GetAttendanceCount(_ClassId, _StandardId, _testdate);
                        DateString = DateString + " <td class=\"TableCell\">  " + _attdcount + " </td> ";
                        _totalarray[_count] = _totalarray[_count] + _attdcount;
                        _testdate = _testdate.Date.AddDays(1);
                        _count++;
                    }
                    _classstr = _classstr + " <tr> <td class=\"TableCellClass\"> " + _ClassName + " : </td>  <td class=\"TableCell\">  " + _StudentsCount + " </td> " + DateString + " </tr>";
                }
            }
            else
            {
                _classstr = "<tr>  <td colspan=\"2\">No class created</td> </tr>";
            }

            string DateHead = "", TotalStr = "";
            _count = 0;
            _testdate = _Startdate;
            while (_testdate.Date <= _EndDate.Date)
            {
                DateHead = DateHead + "<td class=\"TableHead\">   " + General.GerFormatedDatVal(_testdate) + "  </td>";
                TotalStr = TotalStr + " <td class=\"Total\">  " + _totalarray[_count] + " </td>";
                _testdate = _testdate.Date.AddDays(1);
                _count++;
            }
            string str = "<div style=\"width:900px;overflow:auto\"> <table width=\"100%\" cellspacing=\"0\">   <tr>    <td class=\"TableHead\">  Class </td>   <td class=\"TableHead\">   Strength  </td>  " + DateHead + "  </tr>  " + _classstr + " <tr> <td class=\"Total\"> Total : </td>   <td class=\"Total\">  " + _TotalStrenth + " </td>    " + TotalStr + " </tr>  </table>   </div>";
            this.div_ClassStrength.InnerHtml = str;
        }

        private int GetAttendanceCount(string _ClassId, string _StandardId, DateTime _newdate)
        {
            int _count = 0;
            if (MyAttendance.AttendanceTables_Exits(_StandardId, MyUser.CurrentBatchId))
            {
                string Sql = "SELECT Count(DISTINCT t2.StudentId) FROM tblattdcls_std" + _StandardId + "yr" + MyUser.CurrentBatchId + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + MyUser.CurrentBatchId + " t2 ON t2.ClassAttendanceId=t1.Id WHERE t2.PresentStatus<>0 AND t1.ClassId=" + _ClassId + " AND t1.Date='" + _newdate.Date.ToString("s") + "'";
                OdbcDataReader m_MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(Sql);
                if (m_MyReader.HasRows)
                {
                    _count = int.Parse(m_MyReader.GetValue(0).ToString());

                }
            }
            return _count;
        }

        protected void Btn_Excel_Click(object sender, EventArgs e)
        {
            if (this.div_ClassStrength.InnerHtml != "")
            {
                try
                {
                    HttpResponse Response = HttpContext.Current.Response;
                    Response.ContentType = "application/force-download";
                    Response.AddHeader("content-disposition", "attachment; filename=classstrength.xls");
                    Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
                    Response.Write("<head>");
                    Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                    Response.Write("<!--[if gte mso 9]><xml>");
                    Response.Write("<x:ExcelWorkbook>");
                    Response.Write("<x:ExcelWorksheets>");
                    Response.Write("<x:ExcelWorksheet>");
                    Response.Write("<x:Name>classstrength</x:Name>");
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
                    Response.Write(this.div_ClassStrength.InnerHtml);
                    Response.Write("</head>");
                    Response.Flush();
                    Response.End();
                }
                catch (Exception ex)
                {
                    lbl_error.Text = "Error while exporting : " + ex.Message;
                }
            }
            else
            {
                lbl_error.Text = "No data for exporting";
            }
        }
    }
}
