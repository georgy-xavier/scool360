using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WinBase;
using System.Text;
using System.Data.Odbc;

namespace WinEr
{
    public partial class InventoryStockTransactonReport : System.Web.UI.Page
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
                else if (!MyUser.HaveActionRignt(858))
                {
                    Response.Redirect("RoleErr.htm");
                }
                else
                {
                    if (!IsPostBack)
                    {

                        LoadLocationToDropDown();
                        LoadCategoryType();
                        LoadCategory();
                        LoadItemsToDropDown();
                        LoadPeriod();
                        Lbl_ReportErr.Text = "";
                        Pnl_DisplayReport.Visible = false;
                    }
                }
            }

            protected void Drp_Category_SelectedIndexChanged1(object sender, EventArgs e)
            {
                LoadItemsToDropDown();
            }

            protected void Drp_CategoryType_SelectedIndexChanged(object sender, EventArgs e)
            {
                LoadCategory();
            }

            protected void Drp_Category_SelectedIndexChanged(object sender, EventArgs e)
            {
                LoadItemsToDropDown();
            }

            protected void Btn_Show_Click(object sender, EventArgs e)
            {
                GenerateStockTransactionReport();
            }

            protected void img_export_Excel_Click(object sender, ImageClickEventArgs e)
            {
                DataSet MyData = (DataSet)ViewState["ItemTransactionreport"];
                MyData.Tables[0].Columns.Remove("Id");
                MyData.Tables[0].Columns.Remove("ItemId");
                MyData.Tables[0].Columns.Remove("Valuetype");
                MyData.Tables[0].Columns.Remove("ActionType");
                MyData.Tables[0].Columns.Remove("Quantity");

                string Name = "ItemTransactionReport.xls";
                //if (!ExcelUtility.ExportDataSetToExcel(MyData, Name))
                //{

                //}
                string FileName = "ItemTransactionReport";
                string _ReportName = "ItemTransactionReport";
                if (!WinEr.ExcelUtility.ExportDataToExcel(MyData, _ReportName, FileName, MyUser.ExcelHeader))
                {

                }
            }

            protected void Grd_Items_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                Grd_Items.PageIndex = e.NewPageIndex;
                GenerateStockTransactionReport();
            }

            protected void Drp_Period_SelectedIndexChanged(object sender, EventArgs e)
            {
                LoadPeriod();
            }

            protected void Drp_Location_SelectedIndexChanged(object sender, EventArgs e)
            {
                LoadItemsToDropDown();
            }


        #endregion

        
        #region Methods



            private void GenerateStockTransactionReport()
            {
                Lbl_ReportErr.Text = "";
                StringBuilder Stb_transaction = new StringBuilder();
                int itemId = int.Parse(Drp_Item.SelectedValue);
                int locationId = int.Parse(Drp_Location.SelectedValue);
                int ActionType = int.Parse(Drp_ActionType.SelectedValue);
                int categoryId = int.Parse(Drp_Category.SelectedValue);
                DateTime startdate = General.GetDateTimeFromText(Txt_FromDate.Text);
                DateTime Enddate = General.GetDateTimeFromText(Txt_ToDate.Text);
                DataSet Transactionreport_Ds = new DataSet();
                if (itemId != -1)
                {
                    Transactionreport_Ds = Myinventory.GetTransationData(startdate, Enddate, itemId, locationId, ActionType, categoryId);
                    Transactionreport_Ds = GetDescribedDataset(Transactionreport_Ds);
                    if (Transactionreport_Ds != null && Transactionreport_Ds.Tables[0].Rows.Count > 0)
                    {
                        Grd_Items.Columns[0].Visible = true;
                        Grd_Items.Columns[1].Visible = true;
                        Grd_Items.Columns[2].Visible = true;
                        Grd_Items.Columns[3].Visible = true;
                        Grd_Items.Columns[4].Visible = true;

                        Pnl_DisplayReport.Visible = true;
                        Grd_Items.DataSource = Transactionreport_Ds;
                        Grd_Items.DataBind();
                        Grd_Items.Columns[0].Visible = false;
                        Grd_Items.Columns[1].Visible = false;
                        Grd_Items.Columns[2].Visible = false;
                        Grd_Items.Columns[3].Visible = false;
                        Grd_Items.Columns[4].Visible = false;


                    }
                    else
                    {
                        Pnl_DisplayReport.Visible = false; ;
                        Grd_Items.DataSource = null;
                        Grd_Items.DataBind();
                        Lbl_ReportErr.Text = "No report found";

                    }
                }
                else
                {
                    Pnl_DisplayReport.Visible = false; ;
                    Grd_Items.DataSource = null;
                    Grd_Items.DataBind();
                    Lbl_ReportErr.Text = "No report found";

                }
                ViewState["ItemTransactionreport"] = Transactionreport_Ds;
            }

            private DataSet GetDescribedDataset(DataSet Transactionreport_Ds)
            {
                string sql = "";
                string _sql = "";
                OdbcDataReader Myreader = null;
                OdbcDataReader Moveitemreader = null;
                Transactionreport_Ds.Tables[0].Columns.Add("Description");
                Transactionreport_Ds.Tables[0].Columns.Add("Stock In");
                Transactionreport_Ds.Tables[0].Columns.Add("Stock Out");
                Transactionreport_Ds.Tables[0].Columns.Add("Action");
                Transactionreport_Ds.Tables[0].Columns.Add("LocationName");

                foreach (DataRow dr in Transactionreport_Ds.Tables[0].Rows)
                {
                    sql = "select tblinv_locationmaster.LocationName from tblinv_locationmaster where tblinv_locationmaster.Id in (select tblinv_transaction.StoreId from tblinv_transaction where tblinv_transaction.Id="+int.Parse(dr["Id"].ToString())+") ";
                    Moveitemreader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                    if (Moveitemreader.HasRows)
                    {
                        dr["LocationName"] = Moveitemreader.GetValue(0).ToString();
                    }
                    if (int.Parse(dr["ValueType"].ToString()) == 1)
                    {
                        sql = "select tblinv_vendor.Name from tblinv_vendor where tblinv_vendor.Id in(select tblinv_transaction.VendorId from tblinv_transaction where tblinv_transaction.Id=" + int.Parse(dr["Id"].ToString()) + ")";
                        Myreader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                        if (Myreader.HasRows)
                        {

                            dr["Description"] = "Item Purchased from " + Myreader.GetValue(0).ToString() + "";
                            dr["Stock In"] = dr["Quantity"].ToString();
                            dr["Action"] = "Purchase";


                        }
                    }
                    if (int.Parse(dr["ValueType"].ToString()) == 2)
                    {
                        sql = "select tblinv_transaction.`Comment` from tblinv_transaction where tblinv_transaction.Id=" + int.Parse(dr["Id"].ToString()) + "";
                        //sql = "select tblinv_vendor.Name from tblinv_vendor where tblinv_vendor.Id in(select tblinv_transaction.VendorId from tblinv_transaction where tblinv_transaction.Id=" + int.Parse(dr["Id"].ToString()) + ")";
                        Myreader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                        if (Myreader.HasRows)
                        {
                            //dr["Description"] = "Item Saled to " + Myreader.GetValue(0).ToString() + "";
                            dr["Description"] = Myreader.GetValue(0).ToString();
                            dr["Stock Out"] = dr["Quantity"].ToString();
                            dr["Action"] = "Sales";
                        }

                    }
                    if (int.Parse(dr["ValueType"].ToString()) == 3)
                    {
                        sql = "select tblview_user.SurName from tblview_user where tblview_user.Id in(select tblinv_transaction.VendorId from tblinv_transaction where tblinv_transaction.Id=" + int.Parse(dr["Id"].ToString()) + ")";
                        Myreader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                        if (Myreader.HasRows)
                        {
                            dr["Description"] = "Item Issued to " + Myreader.GetValue(0).ToString() + "";
                            dr["Stock Out"] = dr["Quantity"].ToString();
                            dr["Action"] = "Issue";
                        }
                        else
                        {
                            sql = "select tblview_student.StudentName from tblview_student where tblview_student.Id in(select tblinv_transaction.VendorId from tblinv_transaction where tblinv_transaction.Id=" + int.Parse(dr["Id"].ToString()) + ")";
                            Myreader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                            if (Myreader.HasRows)
                            {
                                dr["Description"] = "Item Issued to " + Myreader.GetValue(0).ToString() + "";
                                dr["Stock Out"] = dr["Quantity"].ToString();
                                dr["Action"] = "Issue";
                            }
                        }
                    }
                    if (int.Parse(dr["ValueType"].ToString()) == 4)
                    {
                        if (dr["ActionType"].ToString() == "+stock")
                        {
                            _sql = "select tblinv_locationmaster.LocationName from tblinv_locationmaster where tblinv_locationmaster.id in (select  tblinv_transaction.StoreId from tblinv_transaction where tblinv_transaction.Id="+int.Parse(dr["Id"].ToString())+")";
                            Moveitemreader = Myinventory.m_MysqlDb.ExecuteQuery(_sql);
                            if (Moveitemreader.HasRows)
                            {

                                dr["Description"] = "Item is moved to " + Moveitemreader.GetValue(0).ToString() + "";
                                dr["Stock In"] = dr["Quantity"].ToString();
                                dr["Action"] = "Move Item";
     
                            }                   }
                        else if (dr["ActionType"].ToString() == "-stock")
                        {
                            _sql = "select tblinv_locationmaster.LocationName from tblinv_locationmaster where tblinv_locationmaster.id in (select  tblinv_transaction.StoreId from tblinv_transaction where tblinv_transaction.Id="+int.Parse(dr["Id"].ToString())+")";
                            Moveitemreader = Myinventory.m_MysqlDb.ExecuteQuery(_sql);
                            if (Moveitemreader.HasRows)
                            {

                                dr["Description"] = "Item is moved from " + Moveitemreader.GetValue(0).ToString() + "";
                                dr["Stock Out"] = dr["Quantity"].ToString();
                                dr["Action"] = "Move Item";
                            }
                        }


                    }
                    if (int.Parse(dr["ValueType"].ToString()) == 5)
                    {
                        sql = "select tblinv_transaction.Description from tblinv_transaction where tblinv_transaction.Id=" + int.Parse(dr["Id"].ToString()) + "";
                        Myreader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                        if (Myreader.HasRows)
                        {
                            dr["Description"] = "Item Adjustment-Count increased due to " + Myreader.GetValue(0).ToString() + "";
                            dr["Stock In"] = dr["Quantity"].ToString();
                            dr["Action"] = "Adjustment Increase";
                        }
                    }
                    if (int.Parse(dr["ValueType"].ToString()) == 6)
                    {
                        sql = "select tblinv_transaction.Description from tblinv_transaction where tblinv_transaction.Id=" + int.Parse(dr["Id"].ToString()) + "";
                        Myreader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                        if (Myreader.HasRows)
                        {
                            dr["Description"] = "Item Adjustment-Count decreased due to " + Myreader.GetValue(0).ToString() + "";
                            dr["Stock Out"] = dr["Quantity"].ToString();
                            dr["Action"] = "Adjustment Decrease";
                        }
                    }


                }
                return Transactionreport_Ds;
            }

            //private void GenerateReport()
            //{
            //    Lbl_ReportErr.Text = "";
            //    StringBuilder Stb_transaction = new StringBuilder();
            //    int itemId = int.Parse(Drp_Item.SelectedValue);
            //    int categoryId = int.Parse(Drp_Category.SelectedValue);
            //    DateTime startdate = General.GetDateTimeFromText(Txt_FromDate.Text);
            //    DateTime Enddate = General.GetDateTimeFromText(Txt_ToDate.Text);
            //    DataSet Transactionreport_Ds = new DataSet();
            //    Transactionreport_Ds = Myinventory.GetTransationData(startdate, Enddate, itemId, categoryId);
            //    if (Transactionreport_Ds != null && Transactionreport_Ds.Tables[0].Rows.Count > 0)
            //    {
            //        Pnl_DisplayReport.Visible = true;
            //        Stb_transaction.AppendLine("<table width=\"100%\" border=\"1\">");
            //        Stb_transaction.AppendLine("<tr>");
            //        Stb_transaction.AppendLine("<td align=\"center\" style=\"font-weight:bold\">Item Name</td>");
            //        Stb_transaction.AppendLine("<td align=\"center\" style=\"font-weight:bold\">Opening Balance</td>");
            //        Stb_transaction.AppendLine("<td align=\"center\" style=\"font-weight:bold\">Purchase</td>");
            //        Stb_transaction.AppendLine("<td align=\"center\" style=\"font-weight:bold\">Sales</td>");
            //        Stb_transaction.AppendLine("<td align=\"center\" style=\"font-weight:bold\">Issue</td>");
            //        Stb_transaction.AppendLine("<td align=\"center\" style=\"font-weight:bold\">Adjustment/Increase</td>");
            //        Stb_transaction.AppendLine("<td align=\"center\" style=\"font-weight:bold\">Adjustment/Decrease</td>");
            //        Stb_transaction.AppendLine("<td align=\"center\" style=\"font-weight:bold\">Closing Balance</td>");
            //        Stb_transaction.AppendLine("</tr>");
            //        foreach (DataRow dr in Transactionreport_Ds.Tables[0].Rows)
            //        {
            //            Stb_transaction.AppendLine("<tr>");
            //            Stb_transaction.AppendLine("<td align=\"center\">" + dr["ItemName"].ToString() + "</td>");
            //            Stb_transaction.AppendLine("<td align=\"center\"></td>");
            //            Stb_transaction.AppendLine("<td align=\"center\">" + dr["Purchase"].ToString() + "</td>");
            //            Stb_transaction.AppendLine("<td align=\"center\">" + dr["Sales"].ToString() + "</td>");
            //            Stb_transaction.AppendLine("<td align=\"center\">" + dr["Issue"].ToString() + "</td>");
            //            Stb_transaction.AppendLine("<td align=\"center\">" + dr["Adjust/Increase"].ToString() + "</td>");
            //            Stb_transaction.AppendLine("<td align=\"center\">" + dr["Adjust/Decrease"].ToString() + "</td>");
            //            Stb_transaction.AppendLine("<td align=\"center\"></td>");
            //            Stb_transaction.AppendLine("</tr>");
            //        }

            //        Stb_transaction.AppendLine("</table>");
            //        ViewState["ItemTransactionreport"] = Transactionreport_Ds;
            //    }
            //    else
            //    {
            //        Pnl_DisplayReport.Visible = false;
            //        Lbl_ReportErr.Text = "No report found in this period";
            //    }
            //    DisplayReport.InnerHtml = Stb_transaction.ToString();

            //}
     

            private void LoadPeriod()
            {
                string _sdate = null, _edate = null;   
                if (int.Parse(Drp_Period.SelectedValue) == 0)
                {
                    DateTime _date = System.DateTime.Now;
                    //DateTime _Endtime = _date.AddDays(-1);
                    _edate = MyUser.GerFormatedDatVal(_date);

                    DateTime _start = System.DateTime.Now;
                    _sdate = MyUser.GerFormatedDatVal(new DateTime(_start.Year, _start.Month,1));
                    Txt_FromDate.Enabled = false;
                    Txt_ToDate.Enabled = false;
                    Txt_FromDate.Text = _sdate;
                    Txt_ToDate.Text = _edate;
                    //firstDayOfTheMonth.AddMonths(1).AddDays(-1);
                    
                   
                }
                else if (int.Parse(Drp_Period.SelectedValue) == 1)
                {
                   
                    Txt_FromDate.Text = General.GerFormatedDatVal(System.DateTime.Now.Date.AddDays(-7));
                    Txt_ToDate.Text = General.GerFormatedDatVal(System.DateTime.Now);

                    Txt_FromDate.Enabled = false;
                    Txt_ToDate.Enabled = false;

                }
                else if (int.Parse(Drp_Period.SelectedValue) == 2)
                {
                    Txt_FromDate.Enabled = true;
                    Txt_ToDate.Enabled = true;
                    Txt_FromDate.Text = "";
                    Txt_ToDate.Text = "";

                }
            }

            private void LoadItemsToDropDown()
            {
                ListItem li;
                Drp_Item.Items.Clear();
                DataSet Item_Ds = new DataSet();
                int locationId = int.Parse(Drp_Location.SelectedValue);
                int categoryId = int.Parse(Drp_Category.SelectedValue);
                Item_Ds = Myinventory.GetAllItems(categoryId,locationId);
                if (Item_Ds != null && Item_Ds.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("All", "0");
                    Drp_Item.Items.Add(li);
                    foreach (DataRow dr in Item_Ds.Tables[0].Rows)
                    {
                        //Id, tblinv_item.ItemName
                        li = new ListItem(dr["ItemName"].ToString(), dr["Id"].ToString());
                        Drp_Item.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("No item Found", "-1");
                    Drp_Item.Items.Add(li);
                }
            }

            private void LoadCategory()
            {
                int locationId = int.Parse(Drp_Location.SelectedValue);
                int CategoryType = int.Parse(Drp_CategoryType.SelectedValue);
                DataSet CategoryDs = new DataSet();
                ListItem li = new ListItem();
                Drp_Category.Items.Clear();
                CategoryDs = Myinventory.GetAllCategoryDetails(CategoryType);
                if (CategoryDs != null && CategoryDs.Tables[0].Rows.Count > 0)
                {
                    //.Id, tblinv_category.Category 
                    li = new ListItem("All", "0");
                    Drp_Category.Items.Add(li);
                    foreach (DataRow dr in CategoryDs.Tables[0].Rows)
                    {
                        li = new ListItem(dr["Category"].ToString(), dr["Id"].ToString());
                        Drp_Category.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("None", "-1");
                    Drp_Category.Items.Add(li);
                }

            }

            private void LoadCategoryType()
            {
                DataSet CategoryTypeDs = new DataSet();
                Drp_CategoryType.Items.Clear();
                ListItem li = new ListItem();
                CategoryTypeDs = Myinventory.LoadCategoryType();
                if (CategoryTypeDs != null && CategoryTypeDs.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("All", "0");
                    Drp_CategoryType.Items.Add(li);
                    foreach (DataRow dr in CategoryTypeDs.Tables[0].Rows)
                    {
                        //Id,CategoryType
                        li = new ListItem(dr["CategoryType"].ToString(), dr["Id"].ToString());
                        Drp_CategoryType.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("None", "-1");
                    Drp_CategoryType.Items.Add(li);
                }
            }

            private void LoadLocationToDropDown()
            {
                Drp_Location.Items.Clear();
                ListItem li = new ListItem();
                DataSet Location_Ds = new DataSet();
                Location_Ds = Myinventory.GetLocationName();
                if (Location_Ds != null && Location_Ds.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("All", "0");
                    Drp_Location.Items.Add(li);
                    foreach (DataRow dr in Location_Ds.Tables[0].Rows)
                    {
                        //Id, tblinv_item.ItemName
                        li = new ListItem(dr["Locationname"].ToString(), dr["Id"].ToString());
                        Drp_Location.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("No item Found", "-1");
                    Drp_Location.Items.Add(li);
                }

            }


        #endregion

        

       

        
       

       

     

    }
}
