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

namespace WinEr
{
    public partial class ClassTimeTableReport : System.Web.UI.Page
    {
        private TimeTable MyTimeTable;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        private SchoolClass objSchool = null;
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
            else if (!MyUser.HaveActionRignt(205))
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

                    LoadInitialCondition();
                }
            }

        }

        private void LoadInitialCondition()
        {
            lbl_error.Text = "";
            lbl_ErrorSatff.Text = "";
            LoadAllClassToDropDown();
            LoadAllStaffToDropDown();
        }

        #region CLASS WISE TIME TABLE REPORT FUNCTIONS

        private void LoadAllClassToDropDown()
        {
            Drp_Class.Items.Clear();
            ListItem li = new ListItem("All Class", "0");
            Drp_Class.Items.Add(li);

            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {

                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class.Items.Add(li);

                }

            }
            else
            {
                li = new ListItem("No Class Found", "-1");
                Drp_Class.Items.Add(li);
            }
            Drp_Class.SelectedIndex = 0;


        }

        protected void Btn_Generate_Click(object sender, EventArgs e)
        {

        }

        protected void Img_PdfExport_Click(object sender, ImageClickEventArgs e)
        {
            TimeTableReportPdf MyPdf = new TimeTableReportPdf(MyTimeTable.m_MysqlDb, objSchool);
            string _ErrorMsg = "";
            string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
            string _PdfName = "";
            int _ClassId = int.Parse(Drp_Class.SelectedValue);
            if (_ClassId == 0)
            {
                if (MyPdf.CreateTimeTableReportForAllClasses(MyUser.CurrentBatchName,MyUser.UserId, _physicalpath, out _PdfName, out _ErrorMsg))
                {
                    _ErrorMsg = "";
                    ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                    lbl_error.Text = _ErrorMsg;
                    //MPE_MessageBox.Show();
                }
                else
                {
                    _ErrorMsg = "Failed To Create";
                    lbl_error.Text = _ErrorMsg;
                }
            }
            else
            {
                if (MyPdf.CreateTimeTableReportForSelectedClasses(_ClassId,MyUser.CurrentBatchName, _physicalpath, out _PdfName, out _ErrorMsg))
                {
                    _ErrorMsg = "";
                    ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                    lbl_error.Text = _ErrorMsg;
                    //MPE_MessageBox.Show();
                }
                else
                {
                    _ErrorMsg = "Failed To Create";
                    lbl_error.Text = _ErrorMsg;
                }
            }
        }

        #endregion



        #region STAFF WISE TIME TABLE REPORT FUNCTIONS

        private void LoadAllStaffToDropDown()
        {
            Drp_Staff.Items.Clear();

            string sql = "select distinct tbluser.Id,tbluser.SurName from tbluser inner join tblrole on tblrole.Id = tbluser.RoleId where tbluser.`Status`=1 and  tblrole.Type='staff' ORDER BY tbluser.SurName";
            MyReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("ALL STAFFS", "0");
                Drp_Staff.Items.Add(li);
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Staff.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Staff Found", "-1");
                Drp_Staff.Items.Add(li);
            }

        }

        protected void img_SatffReportPdf_Click(object sender, ImageClickEventArgs e)
        {
            TimeTableReportPdf MyPdf = new TimeTableReportPdf(MyTimeTable.m_MysqlDb, objSchool);
            string _ErrorMsg = "";
            string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
            string _PdfName = "";
            int _StaffId = int.Parse(Drp_Staff.SelectedValue);
            if (_StaffId == 0)
            {
                if (MyPdf.CreateTimeTableReportForAllStaffs(MyUser.CurrentBatchName, MyUser.UserId, _physicalpath, out _PdfName, out _ErrorMsg))
                {
                    _ErrorMsg = "";
                    ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                    lbl_ErrorSatff.Text = _ErrorMsg;
                    //MPE_MessageBox.Show();
                }
                else
                {
                    _ErrorMsg = "Failed To Create";
                    lbl_error.Text = _ErrorMsg;
                }
            }
            else
            {
                if (MyPdf.CreateTimeTableReportForSelectedStaff(_StaffId, MyUser.CurrentBatchName, _physicalpath, out _PdfName, out _ErrorMsg))
                {
                    _ErrorMsg = "";
                    ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                    lbl_ErrorSatff.Text = _ErrorMsg;
                    //MPE_MessageBox.Show();
                }
                else
                {
                    _ErrorMsg = "Failed To Create";
                    lbl_ErrorSatff.Text = _ErrorMsg;
                }
            }
        }

        #endregion

    }
}
