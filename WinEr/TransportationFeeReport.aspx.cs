using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;
using System.Data.Odbc;

namespace WinEr
{
    public partial class TransportationFeeReport : System.Web.UI.Page
    {
        private TransportationClass MyTransMang;
        private KnowinUser MyUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }

            MyUser = (KnowinUser)Session["UserObj"];
            MyTransMang = MyUser.GetTransObj();

            if (MyTransMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(913))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadClassToDropDown();
                    LoadPeriod();
                    Pnl_Show.Visible = false;
                }

            }

        }

        protected void Btn_Show_Click(object sender, EventArgs e)
        {
            LoadTransFeeReport();
        }
         

        protected void img_export_Excel_Click(object sender, EventArgs e)
        {
            DataSet MyData = (DataSet)ViewState["TransFeeReport"];
            MyData.Tables[0].Columns.Remove("StudId");
            if (!WinEr.ExcelUtility.ExportDataSetToExcel(MyData, "Transporation Fee Report.xls"))
            {
                Lbl_msg.Text = "";
            }  
        }

        protected void Grd_TransFeeReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_TransFeeReport.PageIndex = e.NewPageIndex;
            LoadTransFeeReport();
        }

        private void LoadTransFeeReport()
        {

            int _periodid = 0, _classid = 0, _batchid = 0;
            DataSet ReportDs = new DataSet();
            _batchid = MyUser.CurrentBatchId;
            try
            {
                int.TryParse(Drp_Period.SelectedValue, out _periodid);
                int.TryParse(Drp_Class.SelectedValue, out _classid);
                if (_periodid > 0)
                {
                    ReportDs = MyTransMang.GetTransportationFeeReport(_periodid, _classid, _batchid);
                    if (ReportDs != null && ReportDs.Tables[0].Rows.Count > 0)
                    {
                        Lbl_msg.Text = "";
                        Pnl_Show.Visible = true;
                        Grd_TransFeeReport.Columns[5].Visible = true;
                        Grd_TransFeeReport.Columns[4].Visible = true;
                        Grd_TransFeeReport.DataSource = ReportDs;
                        Grd_TransFeeReport.DataBind();
                        Grd_TransFeeReport.Columns[5].Visible = false;
                        Grd_TransFeeReport.Columns[4].Visible = false;
                    }
                    else
                    {
                        Pnl_Show.Visible = false;
                        Lbl_msg.Text = "No report found!";
                        Grd_TransFeeReport.DataSource = null;
                        Grd_TransFeeReport.DataBind();

                    }
                }
                else
                {
                    Lbl_msg.Text = "Select period!";
                }
               
                
            }
            catch (Exception)
            {
                Lbl_msg.Text = "Error,Can't load report!";
                Grd_TransFeeReport.DataSource = null;
                Grd_TransFeeReport.DataBind();
                Pnl_Show.Visible = false;
            }
            ViewState["TransFeeReport"] = ReportDs;

        }
      
        private void LoadPeriod()
        {
            OdbcDataReader FrequencyIdReader = null, PeriodReader = null;
            Drp_Period.Items.Clear();
            ListItem li = new ListItem();
            string sql = "select tblfeeaccount.FrequencyId from tblfeeaccount where tblfeeaccount.Id=100";
            FrequencyIdReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
            if (FrequencyIdReader.HasRows)
            {
                sql = "select tblperiod.Period , tblperiod.Id from tblperiod where tblperiod.FrequencyId=" + FrequencyIdReader.GetValue(0).ToString() + "";
                PeriodReader = MyTransMang.m_MysqlDb.ExecuteQuery(sql);
                if (PeriodReader.HasRows)
                {
                    li = new ListItem("Select", "0");
                    Drp_Period.Items.Add(li);
                    while (PeriodReader.Read())
                    {
                        li = new ListItem(PeriodReader.GetValue(0).ToString(), PeriodReader.GetValue(1).ToString());
                        Drp_Period.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem();
                    li = new ListItem("None", "-1");
                    Drp_Period.Items.Add(li);
                }

            }
            else
            {
                li = new ListItem();
                li = new ListItem("None", "-1");
                Drp_Period.Items.Add(li);
            }
        }

        private void LoadClassToDropDown()
        {
            DataSet Class_Ds = new DataSet();
            ListItem li;
            Drp_Class.Items.Clear();
            Class_Ds = MyTransMang.GetClassDetails();
            if (Class_Ds != null && Class_Ds.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("All", "0");
                Drp_Class.Items.Add(li);
                foreach (DataRow dr in Class_Ds.Tables[0].Rows)
                {
                    li = new ListItem(dr["ClassName"].ToString(), dr["Id"].ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No class found", "-1");
                Drp_Class.Items.Add(li);
            }
        }
    }
}
