using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using WinBase;
using System.Text;
namespace WinEr
{
    public partial class TcIssueReport : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private ConfigManager MyConfiMang;
        private KnowinUser MyUser;
        private OdbcDataReader myReader = null;
        private DataSet myDataset = null;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyConfiMang = MyUser.GetConfigObj();
            MyStudMang = MyUser.GetStudentObj();
            if (MyConfiMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(129))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (WinerUtlity.NeedCentrelDB())
                {
                    if (Session[WinerConstants.SessionSchool] == null)
                    {
                        Response.Redirect("Logout.aspx");
                    }
                    objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                }
                if (!IsPostBack)
                {
                    //txt_StartDate.Enabled = false;
                    //txt_endDate.Enabled = false;
                    datesArea.Visible = false;
                    DateTime _date = System.DateTime.Today;
                    string _sdate = GetFormattedDate(_date);
                    txt_StartDate.Text = _sdate;
                    txt_endDate.Text = _sdate;

                    setInitialActions();                   
                }
            }
        }

        private void setInitialActions()
        {
            
            ViewState["IssueTCDS"] = null;
            Grd_Student.DataSource = null;
            Grd_Student.DataBind();

            StaffResignArea.Visible = false;
            lbl_studentTcAreaMsg.Text = "";
            studentTcArea.Visible = false;
        }

        private string GetFormattedDate(DateTime _date)
        {         
            string _formattedDate="00/00/0000";
            int _month = _date.Month;
            int _day = _date.Day;
            int _year = _date.Year;
            _formattedDate = _day+"/"+_month+"/"+_year;
            return _formattedDate;
        }
      
        protected void Btn_search_Click(object sender, EventArgs e)
        {
            lbl_error.Text = "";
            string _startdate, _enddate, _errormessage;
            if (findStartAndEndDates(out _startdate, out _enddate, out _errormessage))
            {                    
                loadTCIssueGrid(_startdate, _enddate);
            }
            else
            {
                lbl_error.Text = _errormessage;
            }
        }      

        private void loadTCIssueGrid(string _startdate, string _enddate)
        {
            string _sql = "select tblview_student.Id, tblview_student.StudentName, tblview_student.GardianName, tblview_student.AdmitionNo, tblview_student.Sex, date_format( tblview_student.DateOfLeaving, '%d-%m-%Y') AS DateOfLeaving from tblview_student  where  tblview_student.Id in (select tbltc.StudentId from tbltc ) AND (date(tblview_student.DateOfLeaving) BETWEEN '" + _startdate + "' AND '" + _enddate + "')";
            _sql = _sql + " order by tblview_student.ClassId ASC ,tblview_student.RollNo ASC ";
            FillGrid(_sql);
        }

        private void FillGrid(string _sql)
        {

            myDataset = MyConfiMang.m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
            if ((myDataset != null) && (myDataset.Tables[0].Rows.Count > 0))
            {
                Grd_Student.Columns[0].Visible = true;
                lbl_studentTcAreaMsg.Text = "";
                studentTcArea.Visible = true;
                Grd_Student.DataSource = myDataset;
                Grd_Student.DataBind();
                Grd_Student.Columns[0].Visible = false;
                ViewState["IssueTCDS"] = myDataset;
                Img_Excel.Visible = true;
            }
            else
            {
                Img_Excel.Visible = false;
                lbl_studentTcAreaMsg.Text = "No TC Found.";
                studentTcArea.Visible = false;

            }
        }
      
        protected void Grd_Student_SelectedIndexChanged(object sender, EventArgs e)
        {

            int _studId = int.Parse(Grd_Student.SelectedRow.Cells[0].Text);
            Session["StudId"] = _studId;
            Session["StudType"] = 2;
            Response.Redirect("StudentDetails.aspx");
            //Grd_Student.Columns[0].Visible = true;

            //this.studdtls.InnerHtml = "";
            //MPE_MessageBox.Show();
            //this.studdtls.InnerHtml = getStudDetails(_studId);          
            //Grd_Student.Columns[0].Visible = false;
        }

        private string getStudDetails(int _studId)
        {
            string _studDetails = "";

            string _sql = "select  tblview_student.StudentName, tblview_student.GardianName, tblview_student.AdmitionNo, tblview_student.Sex , tblview_student.Address, tblview_student.Pin, tblview_student.ResidencePhNo, tblview_student.Email ,date_format( tblview_student.DOB , '%d-%m-%Y'),date_format( tblview_student.DateofJoining , '%d-%m-%Y'), date_format( tblview_student.DateOfLeaving, '%d-%m-%Y'), (select tblreligion.Religion from tblreligion where tblreligion.Id= tblview_student.Religion) as Religion, tblbatch.BatchName, (select tblcast.castname from tblcast where tblcast.Id=tblview_student.`Cast`) as castname  from tblview_student inner join tblbatch on tblbatch.Id= tblview_student.JoinBatch  where tblview_student.Id=" + _studId;
            myReader = MyConfiMang.m_MysqlDb.ExecuteQuery(_sql);
            if (myReader.HasRows)
            {
               _studDetails="<table>";
                _studDetails +="<tr><td>Student Name:</td><td class=\"rightside\"><b>"+myReader.GetValue(0).ToString()+"</b></td></tr>";
                _studDetails += "<tr><td>Guardian Name:</td><td class=\"rightside\"><b>" + myReader.GetValue(1).ToString() + "</b></td></tr>";
                _studDetails += "<tr><td>Admission No:</td><td class=\"rightside\"><b>" + myReader.GetValue(2).ToString() + "</b></td></tr>";
                _studDetails += "<tr><td >Sex:</td><td class=\"rightside\"><b>" + myReader.GetValue(3).ToString() + "</b></td></tr>";
                _studDetails += "<tr><td >Address:</td><td class=\"rightside\"><b>" + myReader.GetValue(4).ToString() + "</b></td></tr>";
                _studDetails += "<tr><td >Pin:</td><td class=\"rightside\"><b>" + myReader.GetValue(5).ToString() + "</b></td></tr>";
                _studDetails += "<tr><td>Residence PhNo:</td><td class=\"rightside\"><b>" + myReader.GetValue(6).ToString() + "</b></td></tr>";
                _studDetails += "<tr><td>Email:</td><td class=\"rightside\"><b>" + myReader.GetValue(7).ToString() + "</b></td></tr>";
                _studDetails += "<tr><td>DOB:</td><td class=\"rightside\"><b>" + myReader.GetValue(8).ToString() + "</b></td></tr>";
                _studDetails += "<tr><td>Date of Joining:</td><td class=\"rightside\"><b>" + myReader.GetValue(9).ToString() + "</b></td></tr>";
                _studDetails += "<tr><td >Date Of Leaving:</td><td class=\"rightside\"><b>" + myReader.GetValue(10).ToString() + "</b></td></tr>";                
                 _studDetails += "<tr><td>Religion:</td><td class=\"rightside\"><b>" + myReader.GetValue(11).ToString() + "</b></td></tr>";
                 _studDetails += "<tr><td >Caste:</td><td class=\"rightside\"><b>" + myReader.GetValue(13).ToString() + "</b></td></tr>";
                _studDetails += "<tr><td >Batch:</td><td class=\"rightside\"><b>" + myReader.GetValue(12).ToString() + "</b></td></tr>";
 
                _studDetails += "</table>";              
            }

            return _studDetails;
        }

        protected void Grd_Student_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Student.PageIndex = e.NewPageIndex;
            if (ViewState["IssueTCDS"] != null)
            {
                Grd_Student.Columns[0].Visible = true;
                DataSet _pageDS = (DataSet)ViewState["IssueTCDS"];

                Grd_Student.DataSource = _pageDS;
                Grd_Student.DataBind();
                Grd_Student.Columns[0].Visible = false;
            } 
        }           
        
        protected void Drp_Timeperiod_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_error.Text = "";
            setInitialActions();
            string _sdate = null, _edate = null;
            if (Drp_Timeperiod.SelectedItem.Text.ToString() == "Today")
            {
                DateTime _date = System.DateTime.Today;
                _sdate = GetFormattedDate(_date);
                //txt_StartDate.Enabled = false;
                //txt_endDate.Enabled = false;
                datesArea.Visible = false;
                txt_StartDate.Text = _sdate;
                txt_endDate.Text = _sdate;
            }
            if (Drp_Timeperiod.SelectedItem.Text.ToString() == "This Month")
            {
                DateTime _date = System.DateTime.Now;
                _edate = GetFormattedDate(_date);
                DateTime _start = _date.AddDays(-(_date .Day-1)); 
                _sdate = GetFormattedDate(_start);
                //txt_StartDate.Enabled = false;
                //txt_endDate.Enabled = false;
                datesArea.Visible = false;
                txt_StartDate.Text = _sdate;
                txt_endDate.Text = _edate;
            }
            if (Drp_Timeperiod.SelectedItem.Text.ToString() == "Last Week")
            {
                DateTime _date = System.DateTime.Now;
                _edate = GetFormattedDate(_date);
                DateTime _start = _date.AddDays(-7);
                _sdate = GetFormattedDate(_start);
                //txt_StartDate.Enabled = false;
                //txt_endDate.Enabled = false;
                datesArea.Visible = false;
                txt_StartDate.Text = _sdate;
                txt_endDate.Text = _edate;
            }
            if (Drp_Timeperiod.SelectedItem.Text.ToString() == "Last Month")
            {
                DateTime _date = System.DateTime.Now;
                _edate = GetFormattedDate(_date);
                DateTime _start = _date.AddMonths(-1);
                _sdate = GetFormattedDate(_start);
                //txt_StartDate.Enabled = false;
                //txt_endDate.Enabled = false;
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
                //#region Findstartenddates
                //if (Drp_Timeperiod.SelectedItem.Text.ToString() == "Today")
                //{
                //    DateTime _date = System.DateTime.Today;
                //    _sdate = _date.ToString("yyyy-MM-dd");
                //    _edate = _sdate;
                //}
                //else if (Drp_Timeperiod.SelectedItem.Text.ToString() == "This Month")
                //{
                //    DateTime _date = System.DateTime.Now;
                //    _edate = _date.ToString("yyyy-MM-dd");
                //    DateTime _start = System.DateTime.Now;
                //    _sdate = _start.Date.ToString("yyyy-MM-01");

                //}
                //else if (Drp_Timeperiod.SelectedItem.Text.ToString() == "Last Week")
                //{
                //    DateTime _date = System.DateTime.Now;
                //    _edate = _date.ToString("yyyy-MM-dd");
                //    DateTime _start = _date.AddDays(-7);
                //    _sdate = _start.Date.ToString("yyyy-MM-dd");

                //}
                //else if (Drp_Timeperiod.SelectedItem.Text.ToString() == "Last Month")
                //{
                //    DateTime _date = System.DateTime.Now;
                //    _edate = _date.ToString("yyyy-MM-dd");
                //    DateTime _start = _date.AddMonths(-1);
                //    _sdate = _start.Date.ToString("yyyy-MM-dd");
                //}
                //else if (Drp_Timeperiod.SelectedItem.Text.ToString() == "Manual")
                //{
                    _sdate = txt_StartDate.Text.ToString();
                    DateTime _sdate1 = GetDateValue(_sdate);
                    _sdate = _sdate1.ToString("yyyy-MM-dd");
                    //_sdate = _sdate1.ToString("s");
                    _edate = txt_endDate.Text.ToString();
                    DateTime _edate1 = GetDateValue(_edate);
                    _edate = _edate1.ToString("yyyy-MM-dd");
                    //  _edate = _edate1.ToString("s");
                //}
               //  #endregion
            }
            else
            {
                _valid = false;
            }
            return _valid;
        }

        private DateTime GetDateValue(string _DateStr)
        {
           DateTime  _InputDate;            
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

        protected void btn_qkSrch_Click(object sender, EventArgs e)
        {
            string tcno = "";
            tcno = txt_TCNo.Text;
            string sql = "select tblview_student.Id, tblview_student.StudentName, tblview_student.GardianName, tblview_student.AdmitionNo, tblview_student.Sex, date_format( tblview_student.DateOfLeaving, '%d-%m-%Y') AS DateOfLeaving from tblview_student where tblview_student.Id = (select tbltc.StudentId from tbltc where tbltc.TcNumber ='" + tcno + "' )";

            //string sql="select tblview_student.Id, tblview_student.StudentName, tblview_student.GardianName, tblview_student.AdmitionNo, tblview_student.Sex, date_format( tblview_student.DateOfLeaving, '%d-%m-%Y') AS DateOfLeaving ,CONCAT(tblstandard.Name,'-',tblclass.ClassName ) as class from tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId = tblview_student.Id inner join tblclass on tblview_studentclassmap.ClassId = tblclass.Id inner join tblstandard on tblview_studentclassmap.Standard = tblstandard.Id where tblview_student.Id = 2 and tblview_studentclassmap.BatchId = (select max(tblview_studentclassmap.BatchId) from tblview_studentclassmap where tblview_studentclassmap.StudentId = 2)"
            FillGrid(sql);
        }
       
        protected void Grd_Student_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int studId = 0;
            studId = int.Parse(Grd_Student.Rows[e.RowIndex].Cells[0].Text.ToString());
            TCpdf MyPdf = new TCpdf(MyStudMang.m_MysqlDb,objSchool);
            string _ErrorMsg = "";
            string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
            string _PdfName = "";
            int _StudentId = studId;

            if (MyPdf.GenerateStudentTCPdf(_StudentId, _physicalpath, out _PdfName, out _ErrorMsg))
            {
                //ScriptManager.RegisterClientScriptBlock(this.updt1, this.updt1.GetType(), "AnyScriptNameYouLike", "window.open(\"" + _PageName + "?BillNo=" + _Bill + "&BillType=0\");", true);
                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
            }
            else
            { }
        }

        protected void Grd_Student_Editing(object sender, GridViewEditEventArgs e)
        {
            int studId = 0;
            studId = int.Parse(Grd_Student.Rows[e.NewEditIndex].Cells[0].Text.ToString());
            TCpdf MyPdf = new TCpdf(MyStudMang.m_MysqlDb, objSchool);
            string _ErrorMsg = "";
           
            int _StudentId = studId;


            StringBuilder _GeneratedTC = MyStudMang.GenerateDynamicTC(_StudentId,objSchool, out _ErrorMsg);
            if (_GeneratedTC.ToString() != "")
            {
                Session["StudTc"] = _GeneratedTC.ToString();
               // ClientScript.RegisterClientScriptBlock(this.pnlAjaxUpdaet, "keyClientBlock", "<script language=JavaScript>window.open(\"ShowDynamicTc.aspx?StudId=" + _StudentId + "\")</script>");
                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"ShowDynamicTc.aspx?StudId=" + _StudentId + "\");", true);

            }
            else
            {
                Lbl_msg.Text = _ErrorMsg;
                MPE_MessageBox.Show();
            }



        }
        protected void Img_Excel_Click(object sender, ImageClickEventArgs e)
        {
            myDataset = newDataSet((DataSet)ViewState["IssueTCDS"]);
            //myDataset = ViewState["IssueTCDS"];
            string FileName = "TCIssue_Report";
            string _ReportName = "TCIssue_Report";
            if (!WinEr.ExcelUtility.ExportDataToExcel(myDataset, _ReportName, FileName, MyUser.ExcelHeader))
            {
                Lbl_msg.Text = "This function need Ms office";
                this.MPE_MessageBox.Show();
            }
        }

        private DataSet newDataSet(DataSet dataSet)
        {
            DataSet newDataset = new DataSet();
            DataTable dt;
            DataRow dr;

            newDataset.Tables.Add(new DataTable("TcData"));
            dt = newDataset.Tables["TcData"];
            dt.Columns.Add("Student Name");
            dt.Columns.Add("Guardian Name");
            dt.Columns.Add("Admission No");
            //dt.Columns.Add("Class");
            dt.Columns.Add("Sex");
            dt.Columns.Add("Date of Leaving");
            foreach (DataRow dtr in dataSet.Tables[0].Rows)
            {
                dr = dt.NewRow();
                dr["Student Name"] = dtr[1].ToString();
                dr["Guardian Name"] = dtr[2].ToString();
                dr["Admission No"] = dtr[3].ToString();
                //dr["Class"] = "";
                dr["Sex"] = dtr[4].ToString();
                dr["Date of Leaving"] = dtr[5].ToString();
                newDataset.Tables["TcData"].Rows.Add(dr);

            }
            //foreach (GridViewRow gv in Grd_Student.Rows)
            //{
            //    dr = dt.NewRow();
            //    dr["Student Name"] = gv.Cells[1].Text;
            //    dr["Guardian Name"] = gv.Cells[2].Text;
            //    dr["Admission No"] = gv.Cells[3].Text;
            //    //dr["Class"] = "";
            //    dr["Sex"] = gv.Cells[4].Text;
            //    dr["Date of Leaving"] = gv.Cells[5].Text;
            //    newDataset.Tables["TcData"].Rows.Add(dr);
            //}
            return newDataset;
        }

    }
}

