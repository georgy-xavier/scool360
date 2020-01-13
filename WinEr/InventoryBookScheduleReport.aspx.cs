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
    public partial class InventoryBookScheduleReport : System.Web.UI.Page
    {


        private ConfigManager MyConfigMang;
        private KnowinUser MyUser;
        private WinBase.Inventory Myinventory;

        #region Events
        

            protected void Page_Load(object sender, EventArgs e)
            {
                if (Session["UserObj"] == null)
                {
                    Response.Redirect("sectionerr.htm");
                }
                MyUser = (KnowinUser)Session["UserObj"];
                MyConfigMang = MyUser.GetConfigObj();
                Myinventory = MyUser.GetInventoryObj();
                if (MyConfigMang == null)
                {
                    Response.Redirect("RoleErr.htm");
                }
                else if (!MyUser.HaveActionRignt(861))
                {
                    Response.Redirect("RoleErr.htm");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        LoadClassToDropDown();                   
                        LoadPeriod();
                        LoadItemCategoryToDropdown();
                        LoadItemNameToDropdown();
                        Pnl_Show.Visible = false;
                        RowManualperiod.Visible = false;
                    }
                }
            }

            protected void Drp_Period_SelectedIndexChanged(object sender, EventArgs e)
            {
                LoadPeriod();
            }

            protected void Btn_Show_Click(object sender, EventArgs e)
            {
                GenerateReport();
            }

            protected void img_export_Excel_Click(object sender, ImageClickEventArgs e)
            {
                DataSet MyData = (DataSet)ViewState["ScheduleBookReport"];
                MyData.Tables[0].Columns.Remove("BookId");
                MyData.Tables[0].Columns.Remove("StudId");
                MyData.Tables[0].Columns.Remove("ClassId");
                string Name = "ScheduleBookReport.xls";
                //if (!ExcelUtility.ExportDataSetToExcel(MyData, Name))
                //{

                //}
                string FileName = "ScheduleBookReport";
                string _ReportName = "ScheduleBookReport";
                if (!WinEr.ExcelUtility.ExportDataToExcel(MyData, _ReportName, FileName, MyUser.ExcelHeader))
                {

                }
            }

            protected void Grd_ScheduleBookReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                Grd_ScheduleBookReport.PageIndex = e.NewPageIndex;
                GenerateReport();
            }

            protected void Drp_Category_SelectedIndexChanged(object sender, EventArgs e)
            {
                LoadItemNameToDropdown();
            }

        #endregion

        #region Methods

            private void LoadItemNameToDropdown()
            {
                Drp_Item.Items.Clear();
                DataSet Item_Ds = new DataSet();
                ListItem li;
                int category = int.Parse(Drp_Category.SelectedValue);
                if (category != -1)
                {
                    Item_Ds = Myinventory.GetAllItems(category,"");
                    if (Item_Ds != null && Item_Ds.Tables[0].Rows.Count > 0)
                    {
                        li = new ListItem("All", "0");
                        Drp_Item.Items.Add(li);
                        foreach (DataRow dr in Item_Ds.Tables[0].Rows)
                        {
                            li = new ListItem(dr["ItemName"].ToString(), dr["Id"].ToString());
                            Drp_Item.Items.Add(li);
                        }
                    }
                    else
                    {
                        li = new ListItem("No item found", "-1");
                        Drp_Item.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("No item found", "-1");
                    Drp_Item.Items.Add(li);
                }
            }

            private void LoadItemCategoryToDropdown()
            {
                Drp_Category.Items.Clear();
                DataSet Category_Ds = new DataSet();
                ListItem li;
                Category_Ds = Myinventory.GetAllCategory(0,0);
                if (Category_Ds != null && Category_Ds.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("All", "0");
                    Drp_Category.Items.Add(li);
                    foreach (DataRow dr in Category_Ds.Tables[0].Rows)
                    {
                        li = new ListItem(dr["Category"].ToString(), dr["Id"].ToString());
                        Drp_Category.Items.Add(li);
                    }
                    Drp_Category.SelectedValue = "0";
                }
                else
                {
                    li = new ListItem("No category found", "-1");
                    Drp_Category.Items.Add(li);
                }
            }

            private void GenerateReport()
            {
                Lbl_Err.Text = "";
                DataSet Report_Ds = new DataSet();
                int classId = int.Parse(Drp_Class.SelectedValue);
                DateTime startdate = General.GetDateTimeFromText(Txt_FromDate.Text);
                DateTime Enddate= General.GetDateTimeFromText(Txt_ToDate.Text);
                int categoryId = int.Parse(Drp_Category.SelectedValue);
                int itemId = int.Parse(Drp_Item.SelectedValue);
                if (classId != -1 && categoryId != -1 && itemId != -1)
                {
                    Report_Ds = Myinventory.GetScheduleBookreport(classId, startdate, Enddate, categoryId, itemId);
                    if (Report_Ds != null && Report_Ds.Tables[0].Rows.Count > 0)
                    {

                        Pnl_Show.Visible = true;
                        Grd_ScheduleBookReport.Columns[0].Visible = true;
                        Grd_ScheduleBookReport.Columns[1].Visible = true;
                        Grd_ScheduleBookReport.Columns[2].Visible = true;

                        Grd_ScheduleBookReport.DataSource = Report_Ds;
                        Grd_ScheduleBookReport.DataBind();

                        Grd_ScheduleBookReport.Columns[0].Visible = false;
                        Grd_ScheduleBookReport.Columns[1].Visible = false;
                        Grd_ScheduleBookReport.Columns[2].Visible = false;
                    }
                    else
                    {
                        Pnl_Show.Visible = false;
                        Lbl_Err.Text = "No report found";
                    }

                }
                else
                {
                    Pnl_Show.Visible = false;
                    Lbl_Err.Text = "No report found";
                }
                ViewState["ScheduleBookReport"] = Report_Ds;
            }

            private void LoadClassToDropDown()
            {
                DataSet Class_Ds = new DataSet();
                ListItem li;
                Drp_Class.Items.Clear();
                Class_Ds = MyUser.MyAssociatedClass();
                if (Class_Ds != null && Class_Ds.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("All", "0");
                    Drp_Class.Items.Add(li);
                    foreach (DataRow dr in Class_Ds.Tables[0].Rows)
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
            }

            private void LoadPeriod()
            {
                string _sdate = null, _edate = null;
                Pnl_Show.Visible = false;

                if (int.Parse(Drp_Period.SelectedValue) == 0)
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
                else if (int.Parse(Drp_Period.SelectedValue) == 1)
                {

                    Txt_FromDate.Text = General.GerFormatedDatVal(System.DateTime.Now.Date.AddDays(-7));
                    Txt_ToDate.Text = General.GerFormatedDatVal(System.DateTime.Now);
                    Txt_FromDate.Enabled = false;
                    Txt_ToDate.Enabled = false;
                    RowManualperiod.Visible = false;

                }
                else if (int.Parse(Drp_Period.SelectedValue) == 2)
                {
                    RowManualperiod.Visible = true;
                    Txt_FromDate.Enabled = true;
                    Txt_ToDate.Enabled = true;
                    Txt_FromDate.Text = "";
                    Txt_ToDate.Text = "";

                }



            }


        #endregion


        

       

        
        
    }
}
