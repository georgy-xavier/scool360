using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using WinBase;
namespace WinEr
{
    public partial class InventoryItemIssuetoStaff : System.Web.UI.Page
    {
        private DataSet Dt_IssueStaffItemsList;
        private KnowinUser MyUser;
        private DataSet MyDataSet = null;
        private WinBase.Inventory Myinventory;
        private int LocationId;

        protected void Page_Load(object sender, EventArgs e)
        {
            WC_selectItem.EventSelection += new EventHandler(OnItemSelected);
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];

            Myinventory = MyUser.GetInventoryObj();
            if (Myinventory == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(131))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadInitial();
                }
            }
        }
        private void LoadInitial()
        {
            LoadSaleLocation();
            Session["Dt_IssueStaffItemsList"] = null;
            if (Session["SelectedLocation"] != null)
            {
                int.TryParse(Session["SelectedLocation"].ToString(), out LocationId);
                drp_location.SelectedValue = LocationId.ToString();

            }
            WC_selectItem.IsAdjustment(0);
            WC_selectItem.SetCategoryType(0);
            WC_selectItem.SetNeedItemCount(1);
            WC_selectItem.IsPurchaze(0);
             Btn_Issue.Enabled = false;
            Txt_IssueDate.Text = General.GerFormatedDatVal(System.DateTime.Now);
            LoadStaffName();
            lbl_error.Text = "Please add items to list for issuing to staff ";
        }

        private void LoadSaleLocation()
        {
            drp_location.Items.Clear();
            DataSet locationds = Myinventory.GetActiveLocationNameForMoveItem();
            if (locationds != null && locationds.Tables[0].Rows.Count > 0)
            {
                ListItem li;
                li = new ListItem("- Inventory Location -", "0");
                drp_location.Items.Add(li);
                foreach (DataRow dr in locationds.Tables[0].Rows)
                {
                    //Id,Locationname 
                    li = new ListItem(dr["Locationname"].ToString(), dr["Id"].ToString());
                    drp_location.Items.Add(li);

                }


            }
            else
            {
                ListItem li = new ListItem("No location found", "-1");
                drp_location.Items.Add(li);
            }
        }
        protected void OnItemSelected(object sender, EventArgs e)
        {
            lbl_error.Text = "";
            Grd_issue.Columns[0].Visible = true;
            Grd_issue.Columns[1].Visible = true;
             int Id = WC_selectItem.SelectedId;
            int temp = 0;

            if (WC_selectItem.SelectedId != 0 || WC_selectItem.Totalcount != 0)
            {
                string _sql = "";
                _sql = "select ItemName,tblinv_item.id,Stock,tblinv_item.Category ,tblinv_item.Cost,tblinv_category.Category as Categoryname  from tblinv_item inner join tblinv_category on tblinv_category.Id= tblinv_item.Category inner join tblinv_locationitemstock on tblinv_item.Id= tblinv_locationitemstock.ItemId  where  tblinv_locationitemstock.LocationId=" + Session["SelectedLocation"] + " and  tblinv_item.id='" + WC_selectItem.SelectedId + "'  ";

                DataSet UnConsumableDs = Myinventory.m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
                DataRow dr;
                if (UnConsumableDs != null && UnConsumableDs.Tables != null && UnConsumableDs.Tables[0].Rows.Count > 0)
                {

                    int count = 0;
                 
                    if (WC_selectItem.GetNeedItemCount() == 1)
                    {

                        count = WC_selectItem.Totalcount;

                        Dt_IssueStaffItemsList = (DataSet)Session["Dt_IssueStaffItemsList"];
                        foreach (DataRow dr1 in UnConsumableDs.Tables[0].Rows)
                        {
                            if (int.Parse(dr1["Stock"].ToString()) >= count)
                            {
                                if (Dt_IssueStaffItemsList != null && Dt_IssueStaffItemsList.Tables != null && Dt_IssueStaffItemsList.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow drnew in Dt_IssueStaffItemsList.Tables[0].Rows)
                                    {
                                        if (int.Parse(drnew["Id"].ToString()) == int.Parse(dr1["Id"].ToString()))
                                        {
                                            temp = 1;
                                            if (int.Parse(dr1["Stock"].ToString()) >= int.Parse(drnew["Count"].ToString()) + count)
                                            {
                                                count = int.Parse(drnew["Count"].ToString()) + count;

                                                drnew["Count"] = count.ToString();
                                            }
                                            else
                                            {
                                                lbl_error.Text = "Count must be less than or equal to available stock";
                                            }

                                        }
                                    }
                                }
                                if (temp == 0)
                                {
                                    Dt_IssueStaffItemsList = getDataSet();
                                    dr = Dt_IssueStaffItemsList.Tables["issueItemsList"].NewRow();
                                    dr["id"] = dr1["Id"].ToString();
                                    dr["Category"] = dr1["Category"].ToString();
                                    dr["ItemName"] = dr1["ItemName"].ToString().Replace("&amp;", "&"); ;
                                    dr["Stock"] = dr1["Stock"].ToString();
                                    dr["Count"] = count.ToString();
                                    dr["Categoryname"] = count.ToString();
                                   
                                 
                                    Dt_IssueStaffItemsList.Tables["issueItemsList"].Rows.Add(dr);
                                }
                            }
                            else
                            {
                                lbl_error.Text = "Count must be less than or equal to available stock";
                            }
                        }

                    }
                    Session["Dt_IssueStaffItemsList"] = null;
                    Session["Dt_IssueStaffItemsList"] = Dt_IssueStaffItemsList;

                    Grd_issue.DataSource = Dt_IssueStaffItemsList;
                    Grd_issue.DataBind();

                  
                }
                else
                {
                    lbl_error.Text = "Selected item does not found in selected location";
                }
                Grd_issue.Columns[0].Visible = false;
                Grd_issue.Columns[1].Visible = false;
                Btn_Issue.Enabled = true;
            }

        }

        protected void Grd_issue_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grd_issue.Columns[0].Visible = true;
            Grd_issue.Columns[1].Visible = true;

            int selectedrowindex = Grd_issue.SelectedIndex;

            int _id = int.Parse(Grd_issue.Rows[selectedrowindex].Cells[0].Text);

            Dt_IssueStaffItemsList = (DataSet)Session["Dt_IssueStaffItemsList"];

            if (Dt_IssueStaffItemsList != null && Dt_IssueStaffItemsList.Tables != null && Dt_IssueStaffItemsList.Tables[0].Rows.Count > 0)
            {
                if (Dt_IssueStaffItemsList.Tables[0].Rows.Count == 1)
                {
                    Grd_issue.DataSource = null;
                    Grd_issue.DataBind();
                    Dt_IssueStaffItemsList.Tables[0].Rows.Clear();
                    Session["Dt_IssueStaffItemsList"] = Dt_IssueStaffItemsList;
                }
                else
                {
                    Dt_IssueStaffItemsList.Tables[0].Rows[selectedrowindex].Delete();
                    Grd_issue.DataSource = Dt_IssueStaffItemsList;
                    Grd_issue.DataBind();
                    Session["Dt_IssueStaffItemsList"] = Dt_IssueStaffItemsList;
                }
            }
            Grd_issue.Columns[0].Visible = false;
            Grd_issue.Columns[1].Visible = false;
        }

        protected void drp_location_SelectedIndexChanged(object sender, EventArgs e)
        {
            WC_selectItem.IsAdjustment(0);
            WC_selectItem.SetCategoryType(0);
            WC_selectItem.IsPurchaze(0);
            WC_selectItem.SetNeedItemCount(1);
            Session["SelectedLocation"] = drp_location.SelectedValue;
            WC_selectItem.loadlocations();

            Grd_issue.DataSource =null;
            Grd_issue.DataBind();
            Session["Dt_IssueStaffItemsList"] = null;

        }

        private DataSet getDataSet()
        {
            if (Session["Dt_IssueStaffItemsList"] == null)
            {
                Dt_IssueStaffItemsList = new DataSet();

                DataTable dt;

                Dt_IssueStaffItemsList.Tables.Add(new DataTable("issueItemsList"));
                dt = Dt_IssueStaffItemsList.Tables["issueItemsList"];
                dt.Columns.Add("id");
                dt.Columns.Add("Category");
                dt.Columns.Add("ItemName");
                dt.Columns.Add("Stock");
                dt.Columns.Add("Count");
                dt.Columns.Add("Categoryname");

            }
            else
            {
                TextBox tct_descr = new TextBox();

                DataRow dr;
                Dt_IssueStaffItemsList = (DataSet)Session["Dt_IssueStaffItemsList"];
                Dt_IssueStaffItemsList.Tables[0].Rows.Clear();

                foreach (GridViewRow gr in Grd_issue.Rows)
                {


                    dr = Dt_IssueStaffItemsList.Tables["issueItemsList"].NewRow();
                    dr["id"] = gr.Cells[0].Text.ToString();
                    dr["Category"] = gr.Cells[1].Text.ToString();
                    dr["ItemName"] = gr.Cells[2].Text.ToString();
                    dr["Stock"] = gr.Cells[4].Text.ToString();
                    dr["Count"] = gr.Cells[5].Text.ToString();
                    dr["Categoryname"]= gr.Cells[3].Text.ToString();
                    Dt_IssueStaffItemsList.Tables["issueItemsList"].Rows.Add(dr);
                }
            }
            return Dt_IssueStaffItemsList;

        }


        private void LoadStaffName()
        {
            Drp_Staffname.Items.Clear();
            Drp_Staffname.Items.Add(new ListItem("Select staff", "0"));
            string sql = "SELECT DISTINCT t.`Id`,t.`SurName` FROM tbluser t inner join tblrole r on r.`Id`=t.`RoleId` inner join tblgroupusermap g on t.`Id`=g.`UserId` where t.`Status`=1 AND r.`Type`='Staff' AND g.`GroupId` IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY t.`SurName`";
            OdbcDataReader MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Staffname.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No staff found", "-1");
                Drp_Staffname.Items.Add(li);
            }
        }       

        protected void Btn_Issue_Click(object sender, EventArgs e)
        {
            int _ItemID = 0, _CategoryId = 0, count = 0, _availstock = 0, _locationId = 0;
            int chkcount = 0;
            int save = 0;
            DateTime _issuedate = new DateTime();
            DateTime Today = DateTime.Now.Date;
            string StaffName = "", ItemName = "", _description = "", _CreatedUser = "";
            _CreatedUser = MyUser.UserName;
            int value = 0;
            int grdvalue = 0;
            int issuescount = 0;
            Grd_issue.Columns[0].Visible = true;
            Grd_issue.Columns[1].Visible = true;
            foreach (GridViewRow gv in Grd_issue.Rows)
            {


                int.TryParse(gv.Cells[4].Text, out issuescount);
                    _ItemID = int.Parse(gv.Cells[0].Text);
                    int AvailableItemCount = int.Parse(gv.Cells[4].Text); ;

                    if (int.Parse(Drp_Staffname.SelectedValue) <= 0)
                    {
                        lbl_error.Text="Select a staff";
                        grdvalue = 1;

                    }
                    else if (int.Parse(drp_location.SelectedValue) <= 0)
                    {
                        lbl_error.Text="Select location";
                        grdvalue = 1;

                    }
                    else if (AvailableItemCount < count)
                    {
                        int configvalue;
                        Myinventory.GetConfigValue(out configvalue);
                        if (configvalue == 0)
                        {
                            if (AvailableItemCount == 0)
                            {
                                grdvalue = 1;
                                lbl_error.Text="Available stock is zero,Cannot issue "+gv.Cells[2].Text+" item!";
                                break;
                            }
                            else
                            {
                                if (AvailableItemCount < count)
                                {
                                    grdvalue = 1;
                                   lbl_error.Text="Count must be less than available stock!";
                                    break;
                                }
                            }
                        }

                    }

                   
                    else if (Txt_IssueDate.Text != "")
                    {
                        _issuedate = MyUser.GetDareFromText(Txt_IssueDate.Text).Date;
                        if (_issuedate > System.DateTime.Now.Date)
                        {
                            lbl_error.Text = "Date should not be greater than today's date";
                            grdvalue = 1;
                        }
                    }

               
            }

            if (grdvalue == 0)
            {

                try
                {

                    foreach (GridViewRow gv in Grd_issue.Rows)
                    {

                        chkcount++;
                        _ItemID = int.Parse(gv.Cells[0].Text);
                        _CategoryId = int.Parse(gv.Cells[1].Text);
                        ItemName = gv.Cells[2].Text.Replace("&amp;", "&");
                        _availstock = int.Parse(gv.Cells[4].Text);
                        if (txt_issueDescription.Text != "")
                        {
                           

                            if (txt_issueDescription.Text.Trim().Length > 250)
                                _description = txt_issueDescription.Text.Trim().Substring(0, 249);
                            else
                                _description = txt_issueDescription.Text.Trim();

                        }
                        if (gv.Cells[4].Text != "")
                        {
                            count = int.Parse(gv.Cells[5].Text);
                        }
                        if (Txt_IssueDate.Text != "")
                        {
                            _issuedate = MyUser.GetDareFromText(Txt_IssueDate.Text).Date;

                        }
                        else
                        {
                            _issuedate = Today;

                        }


                        StaffName = Drp_Staffname.SelectedItem.ToString();
                        int staffId = int.Parse(Drp_Staffname.SelectedValue);
                        _locationId = int.Parse(drp_location.SelectedValue);
                        string itemname = gv.Cells[2].Text;
                        int opnqty = 0;
                        try
                        {
                            save = 1;
                            Myinventory.m_MysqlDb.MyBeginTransaction();
                            Myinventory.AdjustItemCount(1, _ItemID, count, int.Parse(drp_location.SelectedValue));
                            Myinventory.UpdateOpenQuantity(_ItemID, out opnqty);
                            Myinventory.IssueItemToStaff(_ItemID, _CategoryId, count, _availstock, _locationId, _issuedate, StaffName, ItemName, _description, _CreatedUser, opnqty, staffId);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Inventory :  Issue Item", "Item " + itemname + " is issued to " + StaffName + " Count is " + count + "", 1, Myinventory.m_MysqlDb);
                            Myinventory.m_MysqlDb.TransactionCommit();
                            Session["Dt_IssueStaffItemsList"] = null;
                        }
                        catch
                        {
                            lbl_error.Text = "Please refresh the page and try again...";
                            Myinventory.m_MysqlDb.TransactionRollback();
                        }

                    }


                    if (chkcount == 0)
                    {
                        lbl_error.Text = " Please select an item";

                    }
                    if (save == 1)
                    {
                        //  LoadIssueGrid();
                        // FillItemGrid("", 0);
                        lbl_error.Text = "Selected items are issued to the staff";
                        Grd_issue.DataSource = null;
                        Grd_issue.DataBind();
                         Txt_IssueDate.Text = General.GerFormatedDatVal(System.DateTime.Now);
                        txt_issueDescription.Text = "";
                    }

                }
                catch (Exception error)
                {
                    lbl_error.Text = error.ToString();
                }
            }
            Grd_issue.Columns[0].Visible = false;
            Grd_issue.Columns[1].Visible = false;
        }

        protected void Btn_issueCancel_Click(object sender, EventArgs e)
        {
            Grd_issue.DataSource = null;
            Grd_issue.DataBind();         
             Txt_IssueDate.Text = General.GerFormatedDatVal(System.DateTime.Now);
            txt_issueDescription.Text = "";

        }
    }
}
