using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using WinBase;
using System.Globalization;

namespace Winer.Portal
{
    public partial class FeeReport : System.Web.UI.Page
    {
        private OdbcDataReader MyReader = null;
        private KnowinEncryption Myencryption;
        private KnowinUser MyUser;
        public MysqlClass _MysqlObj;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PortalUserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["PortalUserObj"];
            HiddenField1.Value = MyUser.m_orgId.ToString();
            RowManualperiod.Visible = false;
            //LoadPeriod();
        }
        protected void Drp_School_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void Btn_Show_Click(object sender, EventArgs e)
        {
            if (int.Parse(Drp_Period.SelectedValue) != 0)
            {
                string fromdate = "";
                string todate = "";
                int period = int.Parse(Drp_Period.SelectedValue);
                if (Txt_FromDate.Text != "" && Txt_ToDate.Text != "")
                {
                    //DateTime frmdt = DateTime.ParseExact(Txt_FromDate.Text, "dd-mm-yyyy",null);
                    DateTime frmdt = Convert.ToDateTime(Txt_FromDate.Text);
                    fromdate = frmdt.ToString("yyyy-MM-dd");
                    DateTime todt = Convert.ToDateTime(Txt_ToDate.Text);
                    todate = todt.ToString("yyyy-MM-dd");

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "Datevalue('" + fromdate + "','" + todate + "','"+ period +"');", true);
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "Datevalue('" + fromdate + "','" + todate + "');", true);
                }
            }
           
        }
       
        protected void Grd_StudentReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void Grd_StudentReport_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void img_export_Excel_Click(object sender, ImageClickEventArgs e)
        {

        }
        protected void Drp_Period_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if(int.Parse(Drp_Period.SelectedValue) == 0)
            {
                Response.Redirect("FeeReport.aspx");
            }
            else if(int.Parse(Drp_Period.SelectedValue) == 3)
            {
                Txt_FromDate.Text = "";
                Txt_ToDate.Text = "";
                LoadPeriod();
            }
            else 
                LoadPeriod();

        }
        private void LoadPeriod()
        {
            string _sdate = null, _edate = null;
            
            if (int.Parse(Drp_Period.SelectedValue) == 1)
            {
                DateTime _date = System.DateTime.Now;
                //DateTime _Endtime = _date.AddDays(-1);
                _edate = MyUser.GerFormatedDatVal(_date);

                DateTime _start = System.DateTime.Now;
                _sdate = MyUser.GerFormatedDatVal(new DateTime(_start.Year, _start.Month, 1));
                Txt_FromDate.Enabled = false;
                Txt_ToDate.Enabled = false;
                Txt_FromDate.Text = _sdate;
                Txt_ToDate.Text = _edate;
                RowManualperiod.Visible = false;
               
            }
            else if (int.Parse(Drp_Period.SelectedValue) == 2)
            {

                Txt_FromDate.Text = General.GerFormatedDatVal(System.DateTime.Now.Date.AddDays(-7));
                Txt_ToDate.Text = General.GerFormatedDatVal(System.DateTime.Now);
                Txt_FromDate.Enabled = false;
                Txt_ToDate.Enabled = false;
                RowManualperiod.Visible = false;

            }
            else if (int.Parse(Drp_Period.SelectedValue) == 3)
            {
                RowManualperiod.Visible = true;
                Txt_FromDate.Enabled = true;
                Txt_ToDate.Enabled = true;
                //Txt_FromDate.Text = "";
                //Txt_ToDate.Text = "";

            }
           
        }
    }
}