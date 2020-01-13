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
    public partial class FeeAdvanceControl : System.Web.UI.UserControl
    {
        private KnowinUser MyUser;
        private OdbcDataReader m_Myreader = null;
        private FeeManage MyFeeMang;
        public event EventHandler FeeAdvance;
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

        private void LoadData()
        {
            LoadFeeDropDownList();
            LoadBatchToDrpList();
            
        }

        private void LoadBatchToDrpList()
        {
            Drp_Batch.Items.Clear();
            string _NextBatchID = "0";
            //ListItem Li = new ListItem(MyUser.CurrentBatchName, MyUser.CurrentBatchId.ToString());
            //Drp_Batch.Items.Add(Li);
            ListItem Li = new ListItem(GetNextBatchName(out _NextBatchID), _NextBatchID);
            Drp_Batch.Items.Add(Li);
        }

        private string GetNextBatchName(out string _NextBatchID)
        {
            string Batch = MyUser.CurrentBatchName;
            _NextBatchID = "0";
            string sql = "SELECT BatchName,Id FROM tblbatch where LastbatchId="+MyUser.CurrentBatchId;
            m_Myreader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (m_Myreader.HasRows)
            {
                Batch = m_Myreader.GetValue(0).ToString();
                _NextBatchID = m_Myreader.GetValue(1).ToString();
            }
            return Batch;
        }

        private void LoadFeeDropDownList()
        {
            Drp_FeeName.Items.Clear();
             string sql ="";
             if (Rdo_FeeType.SelectedValue == "1")
                 sql = "SELECT Id,AccountName FROM tblfeeaccount where Type=1 and `Status`=1";
             else
                 sql = "SELECT Id,Name FROM tblfeeothermaster ";
            m_Myreader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (m_Myreader.HasRows)
            {
                Btn_Save.Enabled = true;
                ListItem Li;
                while (m_Myreader.Read())
                {
                    Li = new ListItem(m_Myreader.GetValue(1).ToString(), m_Myreader.GetValue(0).ToString());
                    Drp_FeeName.Items.Add(Li);
                }
                LoadPeriodToDrpList();
            }
            else
            {
                Btn_Save.Enabled = false;
            }
        }

        public void Display()
        {
            LoadData();
            MPE_FeeAdvance.Show();
            Clear();
        }

        private void Clear()
        {
            Txt_Amt.Text = "";
            Txt_Period.Text = "";
        }

        protected void Rdo_FeeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            MPE_FeeAdvance.Show();
            LoadFeeDropDownList();
        }

        protected void Drp_FeeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            MPE_FeeAdvance.Show();
            LoadPeriodToDrpList();
        }

        private void LoadPeriodToDrpList()
        {
            Txt_Period.Text = "";
            Drp_Period.Items.Clear();
            string sql = "";
            sql = "select distinct tblperiod.Id,tblperiod.Period from tblfeeaccount inner join tblperiod on tblperiod.FrequencyId = tblfeeaccount.FrequencyId where tblfeeaccount.Id=" + Drp_FeeName.SelectedValue;
            m_Myreader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (m_Myreader.HasRows && Rdo_FeeType.SelectedValue=="1")
            {
                Txt_Period.Visible = false;
                Lbl_TxtPeriod.Visible = false;
                Drp_Period.Visible = true;
                Lbl_DrpPeriod.Visible = true;
                ListItem Li;
                while (m_Myreader.Read())
                {
                    Li = new ListItem(m_Myreader.GetValue(1).ToString(), m_Myreader.GetValue(0).ToString());
                    Drp_Period.Items.Add(Li);
                }
            }
            else
            {
                Txt_Period.Visible = true;
                Drp_Period.Visible = false;
                Lbl_TxtPeriod.Visible = true;
                Lbl_DrpPeriod.Visible = false;
            }
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            double Amount = 0;
            if (FeeAdvance != null)
            {
                double.TryParse(Txt_Amt.Text.Trim(), out Amount);
                string _Period = Txt_Period.Text.Trim();
                string _PeriodId = "0";
                if (_Period == "")
                {
                    _Period = Drp_Period.SelectedItem.Text;
                    _PeriodId = Drp_Period.SelectedValue;
                }
                OtherFeeArguments _e = new OtherFeeArguments(-1, "0", Drp_FeeName.SelectedValue, Drp_FeeName.SelectedItem.Text, Drp_Batch.SelectedItem.Text, _Period, _PeriodId, "-", Amount, "-", 0, 0, 0, 2, 3, Drp_Batch.SelectedValue, "-");
                FeeAdvance(this, _e);
            }
        }
    }
}