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
    public partial class ResignationReport : System.Web.UI.Page
    {
        private ConfigManager MyConfiMang;
        private KnowinUser MyUser;
        private OdbcDataReader myReader = null;
        private DataSet myDataset = null;

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
            MyConfiMang = MyUser.GetConfigObj();
            if (MyConfiMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(505))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    datesArea.Visible = false;
                    DateTime _date = System.DateTime.Today;
                    string _sdate = MyUser.GerFormatedDatVal(_date);
                    txt_StartDate.Text = _sdate;
                    txt_endDate.Text = _sdate;

                    setInitialActions();
                }
            }
        }

        private void setInitialActions()
        {
            ViewState["StaffResignDS"] = null;
            Grd_staff.DataSource = null;
            Grd_staff.DataBind();

            lbl_StaffResignArea.Text = "";
            StaffResignArea.Visible = false;
        }

        protected void Btn_search_Click(object sender, EventArgs e)
        {
            string _startdate, _enddate, _errormessage;
            if (findStartAndEndDates(out _startdate, out _enddate, out _errormessage))
            {
                lbl_error.Text = "";
                
                loadResignGrid(_startdate, _enddate);
            }
            else
            {
                lbl_error.Text = _errormessage;
            }

        }

        private void loadResignGrid(string _startdate, string _enddate)
        {
            //string _sql = "select tblresignuser.UserId, tbluser_history.UserName, tblresignreason.Reason, tblresignuser.Discription, date_format(tblresignuser.ResignDate, '%d-%m-%Y') AS ResignDate from tblresignuser inner join tblresignreason on tblresignreason.Id= tblresignuser.ReasionId inner join tbluser_history on tbluser_history.Id= tblresignuser.UserId where (date(tblresignuser.ResignDate) BETWEEN '" + _startdate + "' AND '" + _enddate + "')";
            string _sql = "select tbluserdetails_history.UserId, tbluser_history.UserName, tblresignreason.Reason,  tbluserdetails_history.Discription, date_format(tbluserdetails_history.ResignDate, '%d-%m-%Y') AS ResignDate  from tbluserdetails_history inner join tblresignreason on tblresignreason.Id= tbluserdetails_history.ReasionId   inner join tbluser_history on tbluser_history.Id= tbluserdetails_history.UserId where (date(tbluserdetails_history.ResignDate)   BETWEEN '" + _startdate + "' AND '" + _enddate + "')";
            myDataset = MyConfiMang.m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
            if ((myDataset != null) && (myDataset.Tables[0].Rows.Count > 0))
            {
                lbl_StaffResignArea.Text = "";
                StaffResignArea.Visible = true;
                Grd_staff.Columns[0].Visible = true;
                Grd_staff.DataSource = myDataset;
                Grd_staff.DataBind();
                Grd_staff.Columns[0].Visible = false;

                ViewState["StaffResignDS"] = myDataset;
            }
            else
            {
                lbl_StaffResignArea.Text = "NO Staff Resigned During This Batch.";
                StaffResignArea.Visible = false;
            }

        }

        protected void Grd_staff_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_staff.PageIndex = e.NewPageIndex;
            if (ViewState["StaffResignDS"] != null)
            {
                DataSet _pageDS = (DataSet)ViewState["StaffResignDS"];

                Grd_staff.DataSource = _pageDS;
                Grd_staff.DataBind();

            }
        }

        protected void Drp_Timeperiod_SelectedIndexChanged(object sender, EventArgs e)
        {

            setInitialActions();
            string _sdate = null, _edate = null;
            if (Drp_Timeperiod.SelectedItem.Text.ToString() == "Today")
            {
                DateTime _date = System.DateTime.Today;
                _sdate = MyUser.GerFormatedDatVal(_date);
                datesArea.Visible = false;
                txt_StartDate.Text = _sdate;
                txt_endDate.Text = _sdate;
            }
            if (Drp_Timeperiod.SelectedItem.Text.ToString() == "This Month")
            {
                DateTime _date = System.DateTime.Now;
                _edate = MyUser.GerFormatedDatVal(_date);
                DateTime _start = _date.AddDays(-(_date.Day - 1));
                _sdate = MyUser.GerFormatedDatVal(_start);
                datesArea.Visible = false;
                txt_StartDate.Text = _sdate;
                txt_endDate.Text = _edate;
            }
            if (Drp_Timeperiod.SelectedItem.Text.ToString() == "Last Week")
            {
                DateTime _date = System.DateTime.Now;
                _edate = MyUser.GerFormatedDatVal(_date);
                DateTime _start = _date.AddDays(-7);
                _sdate = MyUser.GerFormatedDatVal(_start);
                datesArea.Visible = false;
                txt_StartDate.Text = _sdate;
                txt_endDate.Text = _edate;
            }
            if (Drp_Timeperiod.SelectedItem.Text.ToString() == "Last Month")
            {
                DateTime _date = System.DateTime.Now;
                _edate = MyUser.GerFormatedDatVal(_date);
                DateTime _start = _date.AddMonths(-1);
                _sdate = MyUser.GerFormatedDatVal(_start);
                datesArea.Visible = false;
                txt_StartDate.Text = _sdate;
                txt_endDate.Text = _edate;
            }
            if (Drp_Timeperiod.SelectedItem.Text.ToString() == "Manual")
            {
                datesArea.Visible = true;
                txt_StartDate.Enabled = true;
                txt_endDate.Enabled = true;
                txt_StartDate.Text = "";
                txt_endDate.Text = "";
            }
        }

        private bool findStartAndEndDates(out string _sdate, out string _edate, out string _dateerrormsg)
        {
            bool _valid = true;
            _sdate = "";
            _edate = "";
            _dateerrormsg = "";
            if (DatesRCorrect(out _dateerrormsg))
            {
                _sdate = txt_StartDate.Text.ToString();
                DateTime _sdate1 = GetDateValue(_sdate);
                _sdate = _sdate1.ToString("yyyy-MM-dd");
                //_sdate = _sdate1.ToString("s");
                _edate = txt_endDate.Text.ToString();
                DateTime _edate1 = GetDateValue(_edate);
                _edate = _edate1.ToString("yyyy-MM-dd");
            }
            else
            {
                _valid = false;
            }
            return _valid;
        }

        private DateTime GetDateValue(string _DateStr)
        {
            DateTime _InputDate;
            string[] _DateArray = _DateStr.Split('/');// store DD MM YYYY
            int _Day, _Month, _Year;
            _Day = int.Parse(_DateArray[0]);// day
            _Month = int.Parse(_DateArray[1]);// Month
            _Year = int.Parse(_DateArray[2]);// Year
            _InputDate = new DateTime(_Year, _Month, _Day, 0, 0, 0);
            return _InputDate;
        }

        private bool DatesRCorrect(out string date_errormessage)
        {
            bool valid = true;
            date_errormessage = null;

            if (txt_endDate.Text.Trim() == "")
            {
                date_errormessage = "The End Date Is Empty";
                valid = false;

            }

            if (txt_StartDate.Text.Trim() == "")
            {
                date_errormessage = "The Start Date Is Empty";
                valid = false;
            }

            if (valid == true)
            {
                DateTime _InputDate = new DateTime();
                string[] _DateArray = txt_StartDate.Text.ToString().Split('/');// store DD MM YYYY
                int _Day, _Month, _Year;
                _Day = int.Parse(_DateArray[0]);// day
                _Month = int.Parse(_DateArray[1]);// Month
                _Year = int.Parse(_DateArray[2]);// Year
                _InputDate = new DateTime(_Year, _Month, _Day, 0, 0, 0);

                DateTime _InputDateEnd = new DateTime();
                _DateArray = txt_endDate.Text.ToString().Split('/');// store DD MM YYYY

                _Day = int.Parse(_DateArray[0]);// day
                _Month = int.Parse(_DateArray[1]);// Month
                _Year = int.Parse(_DateArray[2]);// Year
                _InputDateEnd = new DateTime(_Year, _Month, _Day, 0, 0, 0);

                DateTime startdate = _InputDate;
                DateTime enddate = _InputDateEnd;

                TimeSpan diff = enddate.Subtract(startdate);
                int _diff = int.Parse(diff.Days.ToString());

                DateTime today = DateTime.Now;


                if (_diff < 0)
                {
                    date_errormessage = "The Start Date Is Larger Than End Date";
                    valid = false;
                }

                if (startdate > today)
                {
                    date_errormessage = "The Start Date Is Larger Than Todays date";
                    valid = false;
                }

                //if (enddate > today)
                //{
                //    date_errormessage = "The End Date Is Larger Than Todays date";
                //    valid = false;
                //}


            }
            return valid;
        }

    }
}
