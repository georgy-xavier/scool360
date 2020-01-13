using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WinBase;

namespace WinEr
{
    public partial class ParentLoginLogs : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        private StudentManagerClass MyStudMang;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(929))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    Txt_from.Text = MyUser.GerFormatedDatVal(System.DateTime.Now);
                    Txt_To.Text = MyUser.GerFormatedDatVal(System.DateTime.Now);
                    Pnl_Report.Visible = false;
                    rowfrom.Visible = false;
                    rowto.Visible = false;

                }
            }
        }

        protected void Img_Export_Click(object sender, EventArgs e)
        {
            DataSet ParentLogInReport = (DataSet)ViewState["ParentLogInuserlogs"];
            ParentLogInReport.Tables[0].Columns.Remove("Id");
            string FileName = "ParentLogInUser Logst";
            string _ReportName = "Parent Login User Logs";
            if (!WinEr.ExcelUtility.ExportDataToExcel(ParentLogInReport, _ReportName, FileName, MyUser.ExcelHeader))
            {

                Lbl_Msg.Text = "This function need Ms office";
            }
        }

        protected void Grd_ParentLoginReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_ParentLoginReport.PageIndex = e.NewPageIndex;
            DataSet MyDataSetNew = (DataSet)ViewState["ParentLogInuserlogs"];
            Grd_ParentLoginReport.DataSource = MyDataSetNew;
            Grd_ParentLoginReport.DataBind();
        }


        protected void Btn_Generate_Click(object sender, EventArgs e)
        {
            LoadReport();
        }

        protected void Rdb_Type_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (Rdb_Type.SelectedValue == "0")
            {

                rowfrom.Visible = false;
                rowto.Visible = false;
            }
            else if (Rdb_Type.SelectedValue == "1")
            {
                rowfrom.Visible = false;
                rowto.Visible = false;
            }

            else  if (Rdb_Type.SelectedValue == "2")
            {
                rowfrom.Visible = true;
                rowto.Visible = true;
            }
            
        }

        private void LoadReport()
        {
            Lbl_Msg.Text = "";
            int type = 0;
            DataSet ReportDs = new DataSet();


            DateTime fromdate = General.GetDateTimeFromText(Txt_from.Text);
            DateTime Todate = General.GetDateTimeFromText(Txt_To.Text);

            if (Rdb_Type.SelectedValue == "0")
            {
                fromdate = System.DateTime.Now.Date;
                Todate = System.DateTime.Now.Date;

            }
            else if (Rdb_Type.SelectedValue == "1")
            {
                fromdate = new DateTime(System.DateTime.Now.Year,System.DateTime.Now.Month,1);
                Todate = System.DateTime.Now.Date;
            }

            else if (Rdb_Type.SelectedValue == "2")
            {
                 fromdate = General.GetDateTimeFromText(Txt_from.Text);
                 Todate = General.GetDateTimeFromText(Txt_To.Text);
            }


            int.TryParse(Rdb_Type.SelectedValue, out type);
           
            ReportDs = MyStudMang.GetParentLogIn_UserLoginlogs(fromdate, Todate );
           
            if (ReportDs != null && ReportDs.Tables[0].Rows.Count > 0)
            {
                Pnl_Report.Visible = true;
                Grd_ParentLoginReport.Columns[0].Visible = true;
                Grd_ParentLoginReport.DataSource = ReportDs;
                Grd_ParentLoginReport.DataBind();
                Grd_ParentLoginReport.Columns[0].Visible = false;
            }
            else
            {
                Lbl_Msg.Text = "No report found...";
                Grd_ParentLoginReport.DataSource = null;
                Grd_ParentLoginReport.DataBind();
                Pnl_Report.Visible = false;
            }
            ViewState["ParentLogInuserlogs"] = ReportDs;
        }

    }
}
