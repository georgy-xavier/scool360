using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using WinBase;
namespace WinEr.WebControls
{
    public partial class OtherFeeControl : System.Web.UI.UserControl
    {
        private KnowinUser MyUser;
        private OdbcDataReader m_Myreader = null;
        private FeeManage MyFeeMang;
        public event EventHandler EVNTItemAdded;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }

            MyUser = (KnowinUser)Session["UserObj"];
            MyFeeMang = MyUser.GetFeeObj();
            if (MyFeeMang == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }
            if (!IsPostBack)
            {
                
            }
        }

        private void LoadOtherDrp()
        {
            Drp_FeeName.Items.Clear();
            ListItem li;
            string sql = "select distinct Id,Name from tblfeeothermaster";
            m_Myreader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (m_Myreader.HasRows)
            {
                Btn_Save.Enabled = true;
                while (m_Myreader.Read())
                {
                    li = new ListItem(m_Myreader.GetValue(1).ToString(), m_Myreader.GetValue(0).ToString());
                    Drp_FeeName.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No fee found", "0");
                Drp_FeeName.Items.Add(li);
                Btn_Save.Enabled = false;
            }
        }

        public void LoadOtherFees()
        {
            LoadOtherDrp();
            MPE_OtherFee.Show();
            clear();
        }

        private void clear()
        {
            Txt_Amt.Text = "";
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            double Amount =0;
            if (EVNTItemAdded != null)
            {
                double.TryParse(Txt_Amt.Text.Trim(),out Amount);
                OtherFeeArguments _e = new OtherFeeArguments(-1, "0", "-1", Drp_FeeName.SelectedItem.Text, MyUser.CurrentBatchName, "-", "0", "-", Amount, "-", 0, 0, 0, 0, 2, MyUser.CurrentBatchId.ToString(), "-");
                EVNTItemAdded(this, _e);
            }
        }
        
    }
}