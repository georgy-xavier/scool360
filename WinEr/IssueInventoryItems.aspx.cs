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
    public partial class IssueInventoryItems : System.Web.UI.Page
    {
        private ConfigManager MyConfigMang;
        
        private WinBase.Inventory Myinventory;
         
        FeeManage MyFeeMang;

        private WinErSearch MySearchMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MySearchMang = MyUser.GetSearchObj();
            MyFeeMang = MyUser.GetFeeObj();
            if (MySearchMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(406))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    Drp_item.Items.Clear();
                    string sql = " SELECT Id,ItemName from tblinv_item";

                    MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        ListItem ls = new ListItem("Select Item", "-1");
                        Drp_item.Items.Add(ls);
                        while (MyReader.Read())
                        {
                            ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                            Drp_item.Items.Add(li);
                        }
                    }
                    else
                    {
                        ListItem li = new ListItem("No Items Found", "-1");
                        Drp_item.Items.Add(li);
                    }
                    MyReader.Close();
                }
            }
        }

        protected void btn_issue_Click(object sender, EventArgs e)
        {
            lbl_error.Text = "";
            if (txt_itmcount.Text != "")
            {
                if (Convert.ToInt32(lbl_stock.Text) < Convert.ToInt32(txt_itmcount.Text))
                {
                    lbl_error.Text = "insufficent item in stock";
                }
                else
                {
                    if (txt_name.Text != "")
                    {
                        if (Convert.ToInt32(Drp_item.SelectedValue) != -1)
                        {
                            string sql = "insert into tblinv_issueitemstoothers(ItemID,Count,CustName,Address,Phone,Cost) values('" + Drp_item.SelectedValue + "','" + Convert.ToInt32(txt_itmcount.Text) + "','" + txt_name.Text + "','" + txt_adres.Text + "','" + txt_phone.Text + "','" + Convert.ToDouble(lbl_tcost.Text) + "')";
                            MySearchMang.m_MysqlDb.ExecuteQuery(sql);
                            string uptdstck = "update tblinv_item set TotalStock='" + (Convert.ToInt32(lbl_stock.Text) - Convert.ToInt32(txt_itmcount.Text)) + "' where id=" + Drp_item.SelectedValue;
                            MySearchMang.m_MysqlDb.ExecuteQuery(uptdstck);
                        }
                        else
                        {
                            lbl_error.Text = "Please Select Item.";
                        }
                    }
                }
            }
            //MyReader = m_MysqlDb.ExecuteQuery(sql);

            //string updatestock = "update tblinv_item set TotalStock= TotalStock- '" + Convert.ToInt32(txt_itmcount.Text) + "'";
        }

        protected void Drp_item_SelectedIndexChanged(object sender, EventArgs e)
        {
            int stock=0;
            double cost=0.0;

            string sql = "SELECT TotalStock,Cost from tblinv_item where ID="+Drp_item.SelectedValue;
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    stock = Convert.ToInt32(MyReader["TotalStock"].ToString());
                    int a = stock;
                    int b = stock;
                    int c= stock;
                    int d = stock;
                    cost = Convert.ToDouble(MyReader["Cost"].ToString());
                    double cost1 = cost;
                }
            }
            lbl_cost.Text = cost.ToString("0.00");
            lbl_stock.Text = stock.ToString();
        }

        protected void txt_itmcount_TextChanged(object sender, EventArgs e)
        {
            if (txt_itmcount.Text != "")
            {
                if (Convert.ToInt32(lbl_stock.Text) < Convert.ToInt32(txt_itmcount.Text))
                {
                    lbl_error.Text = "insufficent item in stock..";
                }
                else                
                {
                    lbl_error.Text = "";
                    lbl_tcost.Text = (Convert.ToDouble(lbl_cost.Text) * Convert.ToDouble(txt_itmcount.Text)).ToString();
                }
            }
            else if (txt_itmcount.Text == "")
            {
                lbl_tcost.Text = "0.00";
            }
        }
    }
}
