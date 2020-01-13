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
    public partial class StockReport : System.Web.UI.Page
    {

        private LibraryManagClass MyLibMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyLibMang = MyUser.GetLibObj();

            if (MyLibMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(931))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    Lbl_Msg.Text = "";
                    LoadCategory();
                    Pnl_Dispaly.Visible = false;
                }
            }
        }

        private void LoadCategory()
        {
            MyReader = MyLibMang.GetCategories();
            Drp_Category.Items.Clear();
            ListItem li;
            if (MyReader.HasRows)
            {
                li = new ListItem("All", "0");
                Drp_Category.Items.Add(li);
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Category.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No Categories found", "-1");
                Drp_Category.Items.Add(li);
            }
            MyReader.Close();
        }

        protected void Btn_Show_Click(object sender,EventArgs e)
        {
            Lbl_Msg.Text = ""; 
            int catid = 0;
            MydataSet = new DataSet();
            int.TryParse(Drp_Category.SelectedValue, out catid);
            try
            {
                MydataSet = CreateReport(catid);
                if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
                {
                    Pnl_Dispaly.Visible = true;
                    GrdBooks.Columns[0].Visible = true;
                    GrdBooks.DataSource = MydataSet;
                    GrdBooks.DataBind();
                    GrdBooks.Columns[0].Visible = false;
                }
                else
                {
                    Lbl_Msg.Text = "No data found...";
                    GrdBooks.DataSource = null;
                    GrdBooks.DataBind();
                    Pnl_Dispaly.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Lbl_Msg.Text = "Please try later..Error:" + ex.ToString();
            }
            ViewState["Report"] = MydataSet;

        }

        private DataSet CreateReport(int catid)
        {           
            MydataSet = MyLibMang.GetBookDetails(catid);
            return MydataSet;
        }

        protected void GrdBooks__PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Lbl_Msg.Text = "";
            GrdBooks.PageIndex = e.NewPageIndex;
            DataSet report = new DataSet();
             report = (DataSet)ViewState["Report"];
             if (report != null && report.Tables[0].Rows.Count > 0)
             {
                 GrdBooks.DataSource = report;
                 GrdBooks.DataBind();
             }
            
        }

        protected void Btn_Export_Click(object sender, EventArgs e)
        {
            Lbl_Msg.Text = "";
            DataSet Excelds = new DataSet();
            int catid = 0;
            try
            {
                int.TryParse(Drp_Category.SelectedValue, out catid);
                Excelds = CreateReport(catid);
                if (Excelds != null && Excelds.Tables[0].Rows.Count > 0)
                {
                    Excelds.Tables[0].Columns.Remove("Id");
                    Excelds.Tables[0].Columns.Remove("Author");
                    Excelds.Tables[0].Columns.Remove("Edition");
                    Excelds.Tables[0].Columns.Remove("Publisher");
                    string FileName = "StockReport";

                    string _ReportName = "Stock Report";
                    if (!WinEr.ExcelUtility.ExportDataToExcel(Excelds, _ReportName, FileName, MyUser.ExcelHeader))
                    {
                        WC_MessageBox.ShowMssage("Please try again");
                    }
                }
                else
                {
                    Lbl_Msg.Text = "No data found...";
                }
            }
            catch (Exception ex)
            {
                Lbl_Msg.Text = "Pleae try later,Error:" + ex.ToString();
            }
        }
    }
}
