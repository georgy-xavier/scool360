using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WinBase;
using System.Data.Odbc;
using System.Text;

namespace WinEr
{
    public partial class BookIssueReport : System.Web.UI.Page
    {

        private ConfigManager MyConfigMang;
        private KnowinUser MyUser;
        private WinBase.Inventory Myinventory;
        OdbcDataReader Myreader = null;

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
                else if (!MyUser.HaveActionRignt(869))
                {
                    Response.Redirect("RoleErr.htm");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        LoadClassToDropDown();
                        LoadPeriod();
                        AddStudentNameToDropDownStudent();
                        //LoadItemCategoryToDropdown();
                        //LoadItemNameToDropdown();
                        Pnl_Show.Visible = false;
                        RowManualperiod.Visible = false;
                    }
                }
            }
           
            protected void Drp_Period_SelectedIndexChanged(object sender, EventArgs e)
            {
                LoadPeriod();

            }        

            protected void img_export_Excel_Click(object sender, ImageClickEventArgs e)
            {
                DataSet MyData = (DataSet)ViewState["ScheduleBookReport"];
                MyData.Tables[0].Columns.Remove("StudId");
                MyData.Tables[0].Columns.Remove("ClassId");
                MyData.Tables[0].Columns.Remove("Id");
                string Name = "ItemStockReport.xls";
                //if (!ExcelUtility.ExportDataSetToExcel(MyData, Name))
                //{

                //}
                string FileName = "ItemStockReport";
                string _ReportName = "ItemStockReport";
                if (!WinEr.ExcelUtility.ExportDataToExcel(MyData, _ReportName, FileName, MyUser.ExcelHeader))
                {

                }
            }          

            protected void Btn_Show_Click(object sender, EventArgs e)
            {
                GenerateReport();
            }

            protected void Grd_IssueReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                Grd_IssueReport.PageIndex = e.NewPageIndex;
                GenerateReport();
            }

            protected void Grd_IssueReport_SelectedIndexChanged(object sender, EventArgs e)
            {
                OdbcDataReader Myreader = null;
                StringBuilder ReportText = new StringBuilder();
                string report = "";
                int studid =int.Parse(Grd_IssueReport.SelectedRow.Cells[0].Text.ToString());
                int Id =int.Parse(Grd_IssueReport.SelectedRow.Cells[2].Text.ToString());
                string sql = "select Report,BillType from tblinv_viewissuebill where tblinv_viewissuebill.StudId=" + studid + " and Id='" + Id + "'";
                Myreader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                if (Myreader.HasRows)
                {
                    if (Convert.ToInt32(Myreader["BillType"].ToString()) == 0)
                    {
                        ReportText.Append(Myreader.GetValue(0).ToString());
                        Session["ReportText"] = ReportText;
                        ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "openIncpopup('InventoryViewIssueBill.aspx?&studid=" + studid + "');", true);
                    }
                    else if (Convert.ToInt32(Myreader["BillType"].ToString()) == 1)
                    {

                        ReportText.Append(Myreader.GetValue(0).ToString());
                        ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "openIncpopup('invbufferbill.aspx?PDF=" + ReportText + "');", true);
                    }
                }
               
            }

            protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
            {
                AddStudentNameToDropDownStudent();
            }


        #endregion


        #region Methods

            private void GenerateReport()
            {
                Lbl_Err.Text = "";
                DataSet Report_Ds = new DataSet();
                int classId = int.Parse(Drp_Class.SelectedValue);
                DateTime startdate = General.GetDateTimeFromText(Txt_FromDate.Text);
                DateTime Enddate = General.GetDateTimeFromText(Txt_ToDate.Text);
                int StudId = int.Parse(Drp_Student.SelectedValue);
                if (classId != -1 && StudId != -1)
                {
                    Report_Ds = Myinventory.GetBookIssuereport(classId, startdate, Enddate, StudId);
                    if (Report_Ds != null && Report_Ds.Tables[0].Rows.Count > 0)
                    {

                        Pnl_Show.Visible = true;
                        Grd_IssueReport.Columns[0].Visible = true;
                        Grd_IssueReport.Columns[1].Visible = true;
                        Grd_IssueReport.Columns[2].Visible = true;

                        Grd_IssueReport.DataSource = Report_Ds;
                        Grd_IssueReport.DataBind();

                        Grd_IssueReport.Columns[0].Visible = false;
                        Grd_IssueReport.Columns[1].Visible = false;
                        Grd_IssueReport.Columns[2].Visible = false;
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

            private void AddStudentNameToDropDownStudent()
            {
                Drp_Student.Items.Clear();
                string sql = "";
                ListItem li = new ListItem();
                int ClassId = int.Parse(Drp_Class.SelectedValue);
                if (ClassId > 0)
                {
                    sql = " SELECT map.StudentId,stud.StudentName FROM tblstudentclassmap map inner join tblstudent stud on stud.id= map.Studentid where stud.status=1 and  map.ClassId=" + int.Parse(Drp_Class.SelectedValue.ToString()) + " and map.RollNo<>-1 and map.BatchId=" + MyUser.CurrentBatchId + " order by map.RollNo";
                }
                else
                {
                    sql = " SELECT map.StudentId,stud.StudentName FROM tblstudentclassmap map inner join tblstudent stud on stud.id= map.Studentid where stud.status=1 and map.RollNo<>-1 and map.BatchId=" + MyUser.CurrentBatchId + " order by map.RollNo";
                }
                Myreader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                if (Myreader.HasRows)
                {
                    li = new ListItem("All", "0");
                    Drp_Student.Items.Add(li);
                    while (Myreader.Read())
                    {
                         li = new ListItem(Myreader.GetValue(1).ToString(), Myreader.GetValue(0).ToString());
                        Drp_Student.Items.Add(li);
                    }

                }
                else
                {
                     li = new ListItem("No students found", "-1");
                    Drp_Student.Items.Add(li);
                }
                Myreader.Close();
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
          
            //private void LoadItemCategoryToDropdown()
            //{
            //    Drp_Category.Items.Clear();
            //    DataSet Category_Ds = new DataSet();
            //    ListItem li;
            //    Category_Ds = Myinventory.GetAllCategory(0,0);
            //    if (Category_Ds != null && Category_Ds.Tables[0].Rows.Count > 0)
            //    {
            //        li = new ListItem("All", "0");
            //        Drp_Category.Items.Add(li);
            //        foreach (DataRow dr in Category_Ds.Tables[0].Rows)
            //        {
            //            li = new ListItem(dr["Category"].ToString(), dr["Id"].ToString());
            //            Drp_Category.Items.Add(li);
            //        }
            //        Drp_Category.SelectedValue = "0";
            //    }
            //    else
            //    {
            //        li = new ListItem("No category found", "-1");
            //        Drp_Category.Items.Add(li);
            //    }
            //}

            //private void LoadItemNameToDropdown()
            //{
            //    Drp_Item.Items.Clear();
            //    DataSet Item_Ds = new DataSet();
            //    ListItem li;
            //    int category = int.Parse(Drp_Category.SelectedValue);
            //    if (category != -1)
            //    {
            //        Item_Ds = Myinventory.GetAllItems(category);
            //        if (Item_Ds != null && Item_Ds.Tables[0].Rows.Count > 0)
            //        {
            //            li = new ListItem("All", "0");
            //            Drp_Item.Items.Add(li);
            //            foreach (DataRow dr in Item_Ds.Tables[0].Rows)
            //            {
            //                li = new ListItem(dr["ItemName"].ToString(), dr["Id"].ToString());
            //                Drp_Item.Items.Add(li);
            //            }
            //        }
            //        else
            //        {
            //            li = new ListItem("No item found", "-1");
            //            Drp_Item.Items.Add(li);
            //        }
            //    }
            //    else
            //    {
            //        li = new ListItem("No item found", "-1");
            //        Drp_Item.Items.Add(li);
            //    }
            //}

        #endregion
       
    }
}
