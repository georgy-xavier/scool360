using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using WinBase;
namespace WinEr
{
    public partial class AbsDlyFeeReport : System.Web.UI.Page
    {
        //private OdbcDataReader m_MyReader = null;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        private FeeManage MyFeeMang;
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
            else if (!MyUser.HaveActionRignt(253))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadToday();
                    Pnl_AbsRpt.Visible = false;
                    Img_Export.Enabled = false;

                    if (Session["Session_DataSet"] != null)
                    {
                        MydataSet = (DataSet)Session["Session_DataSet"];
                        string _ToDate = Session["Session_ToDate"].ToString();
                        string _FromDate = Session["Session_FromDate"].ToString();
                        Txt_From.Text = _FromDate;
                        Txt_To.Text = _ToDate;
                        if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
                        {
                            GridFees.DataSource = MydataSet;
                            GridFees.DataBind();
                            Pnl_AbsRpt.Visible = true;
                            Img_Export.Enabled = true;
                            ViewState["FeeRpt"] = MydataSet;
                            Session["Session_DataSet"] = null;
                        }
                        else
                        {
                            Lbl_Message.Text = "No data found";
                            GridFees.DataSource = null;
                            GridFees.DataBind();
                            ViewState["FeeRpt"] = null;
                            Pnl_AbsRpt.Visible = false;
                            Img_Export.Enabled = false;
                        }
                    }

                }
            }
        }

        private void LoadToday()
        {
            DateTime _Now = System.DateTime.Now;
            Txt_To.Text = MyUser.GerFormatedDatVal(_Now);
            Txt_From.Text = Txt_To.Text;
        }

        protected void Btn_Show_Click(object sender, EventArgs e)
        {
            Lbl_Message.Text = "";
            if (Txt_From.Text.Trim() != ""&& Txt_To.Text.Trim()!="")
            {
                DataSet MyClass = MyUser.MyAssociatedClass();
                MydataSet = MyFeeMang.GetDailyAbsFeeReport(Txt_From.Text.Trim(), Txt_To.Text.Trim(), MyClass);
                if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
                {
                    GridFees.DataSource = MydataSet;
                    GridFees.DataBind();
                    Pnl_AbsRpt.Visible = true;
                    Img_Export.Enabled = true;
                    ViewState["FeeRpt"] = MydataSet;
                }
                else
                {
                    Lbl_Message.Text = "No data found";
                    GridFees.DataSource = null;
                    GridFees.DataBind();
                    ViewState["FeeRpt"] = null;
                    Pnl_AbsRpt.Visible = false;
                    Img_Export.Enabled = false;
                }

                Session["Session_ToDate"] = Txt_To.Text.Trim();
                Session["Session_FromDate"] = Txt_From.Text.Trim();
               // Response.Redirect("test1.aspx");
            }
        }

        protected void Img_Export_Click(object sender, ImageClickEventArgs e)
        {
            DataSet MyData = (DataSet)ViewState["FeeRpt"];
            DateTime _Date = General.GetDateTimeFromText(Txt_To.Text.Trim());
            string Date = _Date.Date.Day + "-" + _Date.Date.Month + "-" + _Date.Date.Year;
            string FileName = Date + "_AbstractFeeReport";
            string _ReportName = "Abstract Fee Report";
            if (!WinEr.ExcelUtility.ExportDataToExcel(MyData, _ReportName, FileName, MyUser.ExcelHeader))
            {

            }
        }
        
    }
}
