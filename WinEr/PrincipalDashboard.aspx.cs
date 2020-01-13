using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using WinBase;
using System.Web.Services;
using System.Globalization;

namespace WinEr
{
    public partial class PrincipalDashboard : System.Web.UI.Page
    {
        private ClassOrganiser MyClassMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private ConfigManager MyConfig;
        private Attendance MyAttendance;
        private TransportationClass MyTransMang;

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
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
            MyConfig = MyUser.GetConfigObj();
            MyAttendance = MyUser.GetAttendancetObj();
            MyTransMang = MyUser.GetTransObj();
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
            else if (!MyUser.HaveActionRignt(836))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    MyUser.SetDefaultDashboard(2);
                  //  LoadSchoolDetails();
                    //Load_DrpDashboard();
                   // Load_DashboardDetails();
                }
            }
        }
        protected void generaldashboard_Click(object sender, EventArgs e)
        {
            if (MyUser.SELECTEDMODE != 1)
            {
                Response.Redirect("WinErSchoolHome.aspx");
            }
            else
            {
                Response.Redirect("WinSchoolHome.aspx");
            }


        }
        //private void Load_DrpDashboard()
        //{
        //    Drp_Dashboard.Items.Clear();
        //    MyReader=MyUser.GetDashboardPages_hasRights();
        //    if (MyReader.HasRows)
        //    {
        //        Drp_Dashboard.Items.Add(new ListItem("Select Dashboard","-1"));
        //        while (MyReader.Read())
        //        {

        //            Drp_Dashboard.Items.Add(new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString()));

        //        }
        //    }

        //    PanelDashborad.Visible = false;
        //    if (Drp_Dashboard.Items.Count > 2)
        //    {
        //        PanelDashborad.Visible = true;
        //    }
        //}

        //private void Load_DashboardDetails()
        //{
        //  //  Load_ClassStrength();
        //  //  Load_StudentLateComers();
        //  //  Load_StudentExitVilation();
        //  //  Load_StaffAbsentees();
        //   // Load_StaffExitVilation();
        //  //  Load_StudentUsingTransportation();
        //  //  Load_StudentUsingHostel();
        //}
        //private void LoadSchoolDetails()
        //{
        //    string Sql = "SELECT SchoolName,Address,Disc FROM tblschooldetails WHERE Id=1";
        //    MyReader = MyConfig.m_MysqlDb.ExecuteQuery(Sql);
        //    if (MyReader.HasRows)
        //    {
        //        Lbl_SchoolName.Text = MyReader.GetValue(0).ToString();
        //        Lbl_Subhead.Text = MyReader.GetValue(1).ToString();
        //        string[] aa = new string[2];
        //        //if (MyReader.GetValue(2).ToString().Trim() == "")
        //        //{
        //        //    Pnl_aboutUs.Visible = false;
        //        //}
        //        //this.Description.InnerHtml = MyReader.GetValue(2).ToString();

        //    }
        //}
        public class MyGenClass
        {
            SchoolClass objSchool = (SchoolClass)HttpContext.Current.Session[WinerConstants.SessionSchool];
            DataSet MydataSet;
            ConfigManager MyConfig;
            KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
            OdbcDataReader MyReader = null;
            ClassOrganiser MyClassMang;
            public string GetLateComersList(string _ClassId, string _StandardId, string _ClassName)
            {
                string _latestr = "";
                Attendance MyAttendance = MyUser.GetAttendancetObj();
                if (MyAttendance.AttendanceTables_Exits(_StandardId, MyUser.CurrentBatchId))
                {
                    string Sql = "SELECT tblstudent.StudentName FROM tblattdcls_std" + _StandardId + "yr" + MyUser.CurrentBatchId + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + MyUser.CurrentBatchId + " t2 ON t2.ClassAttendanceId=t1.Id INNER JOIN tblstudent ON tblstudent.Id=t2.StudentId WHERE t2.PresentStatus<>0 AND t2.IsLate=1 AND t1.ClassId=" + _ClassId + " AND t1.Date='" + DateTime.Now.Date.ToString("s") + "'";
                    OdbcDataReader m_MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(Sql);
                    if (m_MyReader.HasRows)
                    {
                        while (m_MyReader.Read())
                        {
                            _latestr = _latestr + "<tr>  <td class=\"TableCellStudent\">  " + m_MyReader.GetValue(0).ToString() + " :  </td> <td class=\"TableCellClassLeft\"> " + _ClassName + "  </td> </tr>";
                        }
                    }
                }
                return _latestr;
            }
            public int gethostelstudentcountfromclass(int classid, int batchid)
            {
                Attendance MyAttendance = MyUser.GetAttendancetObj();
                int _TotalSeats = 0;
                string sql = "select count(tblstudent.Id) from tblstudent inner join tblstudentclassmap  on tblstudentclassmap.StudentId= tblstudent.Id where tblstudent.LastClassId = " + classid + " and tblstudent.status = 1 and tblstudentclassmap.BatchId=" + batchid + " and tblstudent.UseHostel=1  order by tblstudent.RollNo";
                OdbcDataReader my_Reader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
                if (my_Reader.HasRows)
                {
                    int.TryParse(my_Reader.GetValue(0).ToString(), out _TotalSeats);
                }
                return _TotalSeats;

            }
            public int GetAttendanceCount(string _ClassId, string _StandardId)
            {
                Attendance MyAttendance = MyUser.GetAttendancetObj();
                int _count = 0;
                if (MyAttendance.AttendanceTables_Exits(_StandardId, MyUser.CurrentBatchId))
                {
                    string Sql = "SELECT Count(DISTINCT t2.StudentId) FROM tblattdcls_std" + _StandardId + "yr" + MyUser.CurrentBatchId + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + MyUser.CurrentBatchId + " t2 ON t2.ClassAttendanceId=t1.Id  INNER JOIN tblview_student ON tblview_student.Id=t2.StudentId WHERE t2.PresentStatus<>0 AND tblview_student.Status=1 AND t1.ClassId=" + _ClassId + " AND t1.Date='" + DateTime.Now.Date.ToString("s") + "'";
                    OdbcDataReader m_MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(Sql);
                    if (m_MyReader.HasRows)
                    {
                        _count = int.Parse(m_MyReader.GetValue(0).ToString());

                    }
                }
                return _count;
            }
            public int GetTotalNumberOfStudentsInClass(int _ClassId, int _BatchId)
            {
                MysqlClass DbClass = new MysqlClass(objSchool.ConnectionString);
                int _TotalSeats = 0;
                string sql = "select count(tblstudentclassmap.StudentId) from tblstudentclassmap INNER JOIN tblview_student ON tblstudentclassmap.StudentId=tblview_student.Id where tblview_student.`Status`=1 AND tblstudentclassmap.ClassId=" + _ClassId + " and tblstudentclassmap.BatchId=" + _BatchId;
                OdbcDataReader m_MyReader = DbClass.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    int.TryParse(m_MyReader.GetValue(0).ToString(), out _TotalSeats);
                }
                return _TotalSeats;
            }
            public string GetExitViolationList(string _ClassId, string _StandardId, string _ClassName)
            {
                string _latestr = "";
                Attendance MyAttendance = MyUser.GetAttendancetObj();
                //dominic  : Please check this query : Some hrd coded db name in last section.
                if (MyAttendance.AttendanceTables_Exits(_StandardId, MyUser.CurrentBatchId))
                {
                    string Sql = "SELECT tblstudent.StudentName,t2.OutTime FROM tblattdcls_std" + _StandardId + "yr" + MyUser.CurrentBatchId + " t1 INNER JOIN tblattdstud_std" + _StandardId + "yr" + MyUser.CurrentBatchId + " t2 ON t2.ClassAttendanceId=t1.Id INNER JOIN tblstudent ON tblstudent.Id=t2.StudentId WHERE t2.PresentStatus<>0 AND t1.ClassId=" + _ClassId + " AND t1.Date in (SELECT MAX(t3.`Date`) FROM tblattdcls_std" + _StandardId + "yr" + MyUser.CurrentBatchId + " as t3 WHERE t3.`Date`<'" + DateTime.Now.Date.ToString("s") + "')";
                    OdbcDataReader m_MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(Sql);
                    if (m_MyReader.HasRows)
                    {
                        while (m_MyReader.Read())
                        {
                            TimeSpan _tmspan = new TimeSpan(0);
                            TimeSpan _tmspannormal = new TimeSpan(0);
                            string _outtimestr = m_MyReader.GetValue(1).ToString();
                            TimeSpan.TryParse(_outtimestr, out _tmspan);

                            if (_tmspan == _tmspannormal)
                            {
                                _latestr = _latestr + "<tr>  <td class=\"TableCellStudent\">  " + m_MyReader.GetValue(0).ToString() + " :  </td> <td class=\"TableCellClassLeft\"> " + _ClassName + "  </td> </tr>";
                            }


                        }
                    }
                }
                return _latestr;
            }
        }

        [WebMethod (EnableSession=true)]
        public static string Load_ClassStrength()
        {
            MyGenClass GenCls = new MyGenClass();
            KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
            Attendance MyAttendance = MyUser.GetAttendancetObj();
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
                    int _StudentsCount = GenCls.GetTotalNumberOfStudentsInClass(int.Parse(_ClassId), MyUser.CurrentBatchId);
                    int Present_StudentsCount = GenCls.GetAttendanceCount(_ClassId, _StandardId);

                    _classstr = _classstr + " <tr> <td class=\"TableCellClass\"> " + _ClassName + " : </td>  <td class=\"TableCell\">  " + Present_StudentsCount + "/" + _StudentsCount + " </td>  </tr>";

                }
            }
            else
            {
                _classstr = "<tr>  <td colspan=\"2\" class=\"noData\"><i class=\"fa fa-thumbs-o-down\" style=\"font-size:48px;\"> </i>No class created</td> </tr>";
            }

            string str = " <table width=\"100%\" cellspacing=\"0\">  <tr class=\"panel-heading panelHead1\">  <td style=\"color:#fff;\" align=\"center\">  Class </td>  <td style=\"color:#fff;\" align=\"center\">   Strength  </td> </tr> </table> <div class=\"princiDashCardInner scrollStyle scrollbar force-overflow\"> <table width=\"100%\" cellspacing=\"0\"> " + _classstr + "</table> </div>  ";
           // this.div_ClassStrength.InnerHtml = str;
            return str;
        }
        [WebMethod(EnableSession = true)]
        public static string[] LoadSchoolData()
        {
            OdbcDataReader MyReader = null;
            KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
            ConfigManager MyConfig = MyUser.GetConfigObj();
            string[] Dt = new string[3];
            string Sql = "SELECT SchoolName,Address,Disc FROM tblschooldetails WHERE Id=1";
            MyReader = MyConfig.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                Dt[0] = MyReader.GetValue(0).ToString();//school name
                Dt[1] = MyReader.GetValue(1).ToString();//shool addres sub
                Dt[2] = MyReader.GetValue(2).ToString();
            }
            return Dt;
        }
        [WebMethod(EnableSession = true)]
        public static string Load_StudentLateComers()
        {
            MyGenClass GenCls = new MyGenClass();
            KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
            Attendance MyAttendance = MyUser.GetAttendancetObj();
            string _latestr = "";
            string sql = "SELECT tblclass.Id,tblclass.ClassName,tblstandard.Id from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id,tblclass.ClassName";
            OdbcDataReader my_Reader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (my_Reader.HasRows)
            {
                while (my_Reader.Read())
                {
                    string _ClassId = my_Reader.GetValue(0).ToString();
                    string _ClassName = my_Reader.GetValue(1).ToString();
                    string _StandardId = my_Reader.GetValue(2).ToString();
                    _latestr = _latestr + GenCls.GetLateComersList(_ClassId, _StandardId, _ClassName);
                }
            }
            else
            {
                _latestr = "<tr>  <td colspan=\"2\" class=\"noData\"><i class=\"fa fa-thumbs-o-up\" style=\"font-size:48px;\"> </i><br> No class created</td> </tr>";
            }
            if (_latestr == "")
            {
                _latestr = "<tr><td colspan=\"2\" class=\"noData\"><i class=\"fa fa-thumbs-o-up\" style=\"font-size:48px;\"> </i><br> No late comers reported for the day</td> </tr>";
            }
            string str = " <table width=\"100%\" cellspacing=\"0\"> <tr class=\"panel-heading panelHead1\"> <td style=\"color:#fff;\" align=\"center\">  Student </td> <td style=\"color:#fff;\" align=\"center\">  Class  </td> </tr> </table>  <div class=\"princiDashCardInner scrollStyle scrollbar force-overflow\"> <table width=\"100%\"> " + _latestr + " </table>   </div> ";
            //  this.div_StudentLateComers.InnerHtml = str;
            return str;
        }
        [WebMethod(EnableSession = true)]
        public static string Load_StudentExitVilation()
        {
            KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
            Attendance MyAttendance = MyUser.GetAttendancetObj();
            string _Exitstr = "";
            MyGenClass myGenCls = new MyGenClass();
            string sql = "SELECT tblclass.Id,tblclass.ClassName,tblstandard.Id from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id,tblclass.ClassName";
            OdbcDataReader my_Reader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (my_Reader.HasRows)
            {
                while (my_Reader.Read())
                {
                    string _ClassId = my_Reader.GetValue(0).ToString();
                    string _ClassName = my_Reader.GetValue(1).ToString();
                    string _StandardId = my_Reader.GetValue(2).ToString();
                    _Exitstr = _Exitstr + myGenCls.GetExitViolationList(_ClassId, _StandardId, _ClassName);
                }
            }
            else
            {
                _Exitstr = "<tr><td colspan=\"2\" class=\"noData\"><i class=\"fa fa-thumbs-o-down\" style=\"font-size:48px;\"> </i><br>No class created</td> </tr>";
            }
            if (_Exitstr == "")
            {
                _Exitstr = "<tr><td colspan=\"2\" class=\"noData\"><i class=\"fa fa-thumbs-o-up\" style=\"font-size:48px;\"> </i><br> No exit violation for the last day</td></tr>";
            }
            string str = " <table width=\"100%\" cellspacing=\"0\">  <tr class=\"panel-heading panelHead1\"> <td style=\"color:#fff;\" align=\"center\">  Student  </td>  <td style=\"color:#fff;\" align=\"center\"> Class </td> </tr>  </table>  <div class=\"princiDashCardInner scrollStyle scrollbar force-overflow\">  <table width=\"100%\"> " + _Exitstr + " </table>   </div>";
            return str;
        }
        [WebMethod(EnableSession = true)]
        public static string Load_StaffAbsentees()
        {
            KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
            Attendance MyAttendance = MyUser.GetAttendancetObj();
            string _StaffAbsencestr = "";
            string sql = "SELECT tbluser.SurName,tblrole.RoleName FROM tblstaffattendance INNER JOIN tbluser ON tbluser.Id=tblstaffattendance.StaffId INNER JOIN tblrole ON tblrole.Id=tbluser.RoleId WHERE tblstaffattendance.MarkStatus=0 AND tblstaffattendance.MarkDate='" + DateTime.Now.Date.ToString("s") + "'";
            OdbcDataReader my_Reader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (my_Reader.HasRows)
            {
                while (my_Reader.Read())
                {
                    string _UserName = my_Reader.GetValue(0).ToString();
                    string _RoleName = my_Reader.GetValue(1).ToString();
                    _StaffAbsencestr = _StaffAbsencestr + " <tr> <td class=\"TableCellStudent\">  " + _UserName + " :  </td>  <td class=\"TableCellClassLeft\">  " + _RoleName + " </td>  </tr> ";
                }
            }

            if (_StaffAbsencestr == "")
            {
                _StaffAbsencestr = "<tr><td colspan=\"2\" class=\"noData\"><i class=\"fa fa-thumbs-o-up\" style=\"font-size:48px;\"> </i><br> No staff is marked absent today</td> </tr>";
            }
            string str = "<table width=\"100%\" cellspacing=\"0\">  <tr class=\"panel-heading panelHead1\"> <td style=\"color:#fff;\" align=\"center\">  Staff </td>  <td style=\"color:#fff;\" align=\"center\">  Role  </td> </tr> </table> <div class=\"princiDashCardInner scrollStyle scrollbar force-overflow\"> <table width=\"100%\"> " + _StaffAbsencestr + " </table>   </div>  ";
            //  this.div_StaffTodaysAbsentees.InnerHtml = str;
            return str;
        }
        [WebMethod(EnableSession = true)]
        public static string Load_StaffExitVilation()
        {
            OdbcDataReader my_Reader = null;
            KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
            Attendance MyAttendance = MyUser.GetAttendancetObj();
            string _Exitvilationstr = "",sub="";
            string subSql = "SELECT MAX(t3.`MarkDate`) FROM tblstaffattendance as t3 WHERE t3.`MarkDate`<'" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "'";
            my_Reader = MyAttendance.m_MysqlDb.ExecuteQuery(subSql);
            if (my_Reader.HasRows)
            {
                while (my_Reader.Read())
                {
                    DateTime tt = (DateTime)my_Reader.GetValue(0);
                    sub = tt.ToString("yyyy-MM-dd");
                }
            }
          
            string sql = "SELECT tbluser.SurName,tblrole.RoleName,tblstaffattendance.OutTime FROM tblstaffattendance INNER JOIN tbluser ON tbluser.Id=tblstaffattendance.StaffId INNER JOIN tblrole ON tblrole.Id=tbluser.RoleId WHERE tblstaffattendance.MarkStatus<>0 AND tblstaffattendance.MarkDate in ('" + sub + "')";
            my_Reader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (my_Reader.HasRows)
            {
                while (my_Reader.Read())
                {
                    string _UserName = my_Reader.GetValue(0).ToString();
                    string _RoleName = my_Reader.GetValue(1).ToString();
                    TimeSpan _tmspan = new TimeSpan(0);
                    TimeSpan _tmspannormal = new TimeSpan(0);
                    string _outtimestr = my_Reader.GetValue(2).ToString();
                    TimeSpan.TryParse(_outtimestr, out _tmspan);

                    if (_tmspan == _tmspannormal)
                    {
                        _Exitvilationstr = _Exitvilationstr + " <tr> <td class=\"TableCellStudent\">  " + _UserName + " :  </td>  <td class=\"TableCellClassLeft\">  " + _RoleName + " </td>  </tr> ";
                    }



                }
            }

            if (_Exitvilationstr == "")
            {
                _Exitvilationstr = "<tr><td colspan=\"2\" class=\"noData\"><i class=\"fa fa-thumbs-o-up\" style=\"font-size:48px;\"> </i><br> No exit violation for the last day </td> </tr>";
            }
            string str = "<table width=\"100%\" cellspacing=\"0\">  <tr class=\"panel-heading panelHead1\"> <td style=\"color:#fff;\" align=\"center\">  Staff </td>  <td style=\"color:#fff;\" align=\"center\">  Role  </td> </tr> </table> <div class=\"princiDashCardInner scrollStyle scrollbar force-overflow\"> <table width=\"100%\"> " + _Exitvilationstr + " </table>   </div>  ";
            //this.div_StaffExitVilation.InnerHtml = str;
            return str;
        }
        [WebMethod(EnableSession = true)]
        public static string Load_StudentUsingTransportation()
        {
            KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
            Attendance MyAttendance = MyUser.GetAttendancetObj();
            TransportationClass MyTransMang = MyUser.GetTransObj();;
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
                    int _StudentsCount = MyTransMang.getStudentCountFromClass(int.Parse(_ClassId), MyUser.CurrentBatchId);


                    _classstr = _classstr + " <tr> <td class=\"TableCellClass\"> " + _ClassName + " : </td>  <td class=\"TableCell\">  " + _StudentsCount + " </td>  </tr>";

                }
            }
            else
            {
                _classstr = "<tr><td colspan=\"2\" class=\"noData\"><i class=\"fa fa-thumbs-o-down\" style=\"font-size:48px;\"> </i><br>No class created</td> </tr>";
            }
            int totalstudentusingbus = 0;
            sql = "select count(tblstudent.Id) from tblstudent inner join tblstudentclassmap  on tblstudentclassmap.StudentId= tblstudent.Id  where tblstudent.UseBus=1 and tblstudent.status=1 and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " ";
            OdbcDataReader m_MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out totalstudentusingbus);
            }
            _classstr = "<tr> <td class=\"TableCellClass\"> Total : </td>  <td class=\"TableCell\">  " + totalstudentusingbus + " </td>  </tr>" + _classstr;

            string str = " <table width=\"100%\" cellspacing=\"0\">  <tr class=\"panel-heading panelHead1\">  <td style=\"color:#fff;\" align=\"center\">  Class </td>  <td style=\"color:#fff;\" align=\"center\">   Strength  </td> </tr> </table> <div class=\"princiDashCardInner scrollStyle scrollbar force-overflow\"> <table width=\"100%\" cellspacing=\"0\"> " + _classstr + "</table> </div>  ";
            //  this.div_usingtransportation.InnerHtml = str;
            return str;
        }
        [WebMethod(EnableSession = true)]
        public static string Load_StudentUsingHostel()
        {
            MyGenClass GenCls = new MyGenClass();
            KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
            Attendance MyAttendance = MyUser.GetAttendancetObj();
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
                    int _StudentsCount = GenCls.gethostelstudentcountfromclass(int.Parse(_ClassId), MyUser.CurrentBatchId);


                    _classstr = _classstr + " <tr> <td class=\"TableCellClass\"> " + _ClassName + " : </td>  <td class=\"TableCell\">  " + _StudentsCount + " </td>  </tr>";

                }
            }
            else
            {
                _classstr = "<tr><td colspan=\"2\" class=\"noData\"><i class=\"fa fa-thumbs-o-down\" style=\"font-size:48px;\"> </i><br>No class created</td> </tr>";
            }
            int totalstudentusinghostel = 0;
            sql = "select count(tblstudent.Id) from tblstudent inner join tblstudentclassmap  on tblstudentclassmap.StudentId= tblstudent.Id where tblstudent.UseHostel=1 and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + "  ";
            OdbcDataReader m_MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out totalstudentusinghostel);
            }
            _classstr = "<tr> <td class=\"TableCellClass\"> Total : </td>  <td class=\"TableCell\">  " + totalstudentusinghostel + " </td>  </tr>" + _classstr;

            string str = " <table width=\"100%\" cellspacing=\"0\">  <tr class=\"panel-heading panelHead1\">  <td style=\"color:#fff;\" align=\"center\">  Class </td>  <td style=\"color:#fff;\" align=\"center\">   Strength  </td> </tr> </table> <div class=\"princiDashCardInner scrollStyle scrollbar force-overflow\"> <table width=\"100%\" cellspacing=\"0\"> " + _classstr + "</table> </div>  ";
            // this.div_usinghostel.InnerHtml = str;
            return str;
        }
       
        
        
        
        //protected void Drp_Dashboard_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string page = MyUser.SelectDashBoard(Drp_Dashboard.SelectedValue, MyUser.SELECTEDMODE);
        //    if (page != "")
        //    {
        //        Response.Redirect(page);
        //    }
        //}
        protected void Btn_Default_Click(object sender, EventArgs e)
        {
            MyUser.SetDefaultDashboard(2);
        }


    }
}
