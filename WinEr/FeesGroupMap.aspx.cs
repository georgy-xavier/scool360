using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;

namespace WinEr
{
    public partial class FeesGroupMap : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
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
                Response.Redirect("sectionerr.htm");

            }
            else if (!MyUser.HaveActionRignt(3020))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    Load_Fees_Header();
                    Load_UnMapped_Fees();
                    Mapped_Fees(int.Parse(Drplist_Header.SelectedValue));
                }
            }

        }
        private void Load_Fees_Header()
        {
            DataSet ds_Headers = new DataSet();
            string sql = "select * from tbl_feesgrouphead";
            ds_Headers = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds_Headers != null && ds_Headers.Tables[0] != null && ds_Headers.Tables[0].Rows.Count > 0)
            {
                Drplist_Header.Items.Clear();
                Drplist_Header.DataSource = ds_Headers;
                Drplist_Header.DataValueField = "Id";
                Drplist_Header.DataTextField = "Name";
                Drplist_Header.DataBind();
            }
            else
            {
                ListItem li = new ListItem("No Data Exists", "0");
                Drplist_Header.Items.Add(li);
            }
        }
        private void Load_UnMapped_Fees()
        {
            DataSet ds_Headers = new DataSet();
            string sql = "select * from tblfeeaccount where Status=1 and Id NOT IN(SELECT Fees_Id from tbl_feesheadermap)";
            ds_Headers = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds_Headers != null && ds_Headers.Tables[0] != null && ds_Headers.Tables[0].Rows.Count > 0)
            {
                Drp_Fees.Items.Clear();
                Drp_Fees.DataSource = ds_Headers;
                Drp_Fees.DataValueField = "Id";
                Drp_Fees.DataTextField = "AccountName";
                Drp_Fees.DataBind();
            }
            else
            {
                ListItem li = new ListItem("No Fees Exist To Map", "0");
                Drp_Fees.Items.Add(li);
            }
        }
        private void Mapped_Fees(int Header_Id)
        {
            DataSet ds_Headers = new DataSet();
            string sql = "SELECT tblfeeaccount.Id,tblfeeaccount.AccountName,tbl_feesgrouphead.Name FROM tblfeeaccount INNER JOIN tbl_feesheadermap ON tbl_feesheadermap.Fees_Id=tblfeeaccount.Id INNER JOIN tbl_feesgrouphead ON tbl_feesgrouphead.Id=tbl_feesheadermap.Header_Id where tblfeeaccount.Status=1";
            ds_Headers = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds_Headers != null && ds_Headers.Tables[0] != null && ds_Headers.Tables[0].Rows.Count > 0)
            {
               Grd_Fees.Columns[0].Visible=true;
               Grd_Fees.DataSource=ds_Headers;
               Grd_Fees.DataBind();
               Grd_Fees.Columns[0].Visible=false;
                Lbl_Msg.Visible=false;
            }
            else
            {
                Grd_Fees.DataSource=null;
                Grd_Fees.DataBind();
                Lbl_Msg.Visible = true;
            }
        }
        protected void Btn_Map_Click(object sender, EventArgs e)
        {
           int Fees_Id=0;
           int Header_Id=0;
           try
           {
               if (Drplist_Header.SelectedValue == "0")
               {
                   WC_MsgBox.ShowMssage("No Fees Group Header Exist To Map");
               }
               else if (Drp_Fees.SelectedValue == "0")
               {
                   WC_MsgBox.ShowMssage("No Fees Exist To Map");
               }
               else if (Validation())
               {
                   WC_MsgBox.ShowMssage("Already Some Fees Mapped with This Header");
               }
               else
               {
                   int.TryParse(Drplist_Header.SelectedValue, out Header_Id);
                   int.TryParse(Drp_Fees.SelectedValue, out Fees_Id);
                   string sql = "insert into tbl_feesheadermap(Header_Id,Fees_Id) values(" + Header_Id + "," + Fees_Id + ")";
                   MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                   Mapped_Fees(int.Parse(Drplist_Header.SelectedValue));
                   WC_MsgBox.ShowMssage("Fees mapped successfully");
               }
           }
           catch
           {
               WC_MsgBox.ShowMssage("Please try again");
           }
        }
        private bool Validation()
        {
             bool valide = false;
             DataSet ds_Valide = new DataSet();
             string sql = "SELECT * from tbl_feesheadermap where Header_Id="+int.Parse(Drplist_Header.SelectedValue)+"";
             ds_Valide = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
             if (ds_Valide != null && ds_Valide.Tables[0] != null && ds_Valide.Tables[0].Rows.Count > 0)
             {
                 valide = true;
             }
             return valide;

        }
        protected void Grd_Fees_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                try{
                int Select_Id=0;
                int index = Convert.ToInt32(e.CommandArgument);
                int.TryParse(Grd_Fees.Rows[index].Cells[0].Text, out Select_Id);
                if(Select_Id>0)
                {
                string sql="Delete from tbl_feesheadermap where Fees_Id=" + Select_Id + "";
                MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                Mapped_Fees(int.Parse(Drplist_Header.SelectedValue));
                WC_MsgBox.ShowMssage("Fees UnMapped Successfully");
                }
                else{
                    WC_MsgBox.ShowMssage("Please try again");
                }
                }
                catch{
                    WC_MsgBox.ShowMssage("Please try again");
                }
          
            }
        }
        protected void Grd_Fees_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Fees.PageIndex = e.NewPageIndex;
            Mapped_Fees(int.Parse(Drplist_Header.SelectedValue));
            
        }
    }
}
