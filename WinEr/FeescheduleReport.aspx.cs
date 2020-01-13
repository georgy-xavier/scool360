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
    public partial class FeescheduleReport : System.Web.UI.Page
    {

        private FeeManage MyFeeMang;
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
            MyFeeMang = MyUser.GetFeeObj();
            if (MyFeeMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(128))
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
                    string _FeeMenuStr;
                    _FeeMenuStr = MyFeeMang.GetFeeMangMenuString(MyUser.UserRoleId);
                    this.FeeMenu.InnerHtml = _FeeMenuStr;
                    _FeeMenuStr = MyUser.GetDetailsString(128);
                    this.ActionInfo.InnerHtml = _FeeMenuStr;
                    lbl_Error.Visible = false;
                    lbl_Error.Text = "";

                    LoadClassNameToDropDownList();

                }
            }
        }

        private void LoadClassNameToDropDownList()
        {
            Drp_Class.Items.Clear();
            ListItem li = new ListItem("ALL CLASS", "0");
            Drp_Class.Items.Add(li);
            string sql = " SELECT tblclass.Id,tblclass.ClassName from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id,tblclass.ClassName";

            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Class.Items.Add(li);
                }


            }
            else
            {
                 li = new ListItem("No students found", "-1");
                Drp_Class.Items.Add(li);
            }
        }

        protected void Img_PdfExport_Click(object sender, ImageClickEventArgs e)
        {
            int _ClassId = 0;
            _ClassId = int.Parse(Drp_Class.SelectedValue.ToString());

            //if (PdfReportEnabled())
            //{
            FeeScheduleReportPdf MyPdf = new FeeScheduleReportPdf(MyFeeMang.m_MysqlDb, objSchool);
            string _ErrorMsg = "";
            string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
            string _PdfName = "";


            if (MyPdf.CreateFeeScheduleReportPdf(_ClassId, MyUser.CurrentBatchId, MyUser.CurrentBatchName, _physicalpath, out _PdfName, out _ErrorMsg))
            {
                _ErrorMsg = "";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);

                //ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                lbl_Error.Visible = false;
                lbl_Error.Text = "";
            }
            else
            {
                _ErrorMsg = "Failed To Create";
                lbl_Error.Visible = true;
                lbl_Error.Text = _ErrorMsg;
                //MPE_MessageBox.Show();
            }
            //}
        }

        private bool PdfReportEnabled()
        {
            bool _valid = false;
            string PdfReport;
            string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='PdfReport'";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                PdfReport = MyReader.GetValue(0).ToString();
                if (PdfReport == "1")
                {
                    _valid = true;
                }
            }
            return _valid;
        }

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_Error.Visible = false;
            lbl_Error.Text = "";
        }
    }
}
