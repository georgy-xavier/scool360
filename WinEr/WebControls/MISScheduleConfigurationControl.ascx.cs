using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;

namespace WinErParentLogin.WebControls
{
    public partial class MISScheduleConfigurationControl : System.Web.UI.UserControl
    {
        private KnowinUser MyUser;
        private OdbcDataReader m_Myreader = null;
        private FeeManage MyFeeMang;
        static int Temp_scheduleId = 0;

        public event EventHandler EVNTSave;
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
                Lbl_err.Visible = false;
                LoadCheckBoxReportName();
            }


        }
        private void LoadCheckBoxReportName()
        {
            Chkboxlist_reportname.Items.Clear();
            string sql = "SELECT Id,ReportName from tblmisschedulereport";
            m_Myreader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            ListItem li;
            if (m_Myreader.HasRows)
            {
                while (m_Myreader.Read())
                {

                    //if (Chkboxlist_reportname.Items.Count == 0)
                    //{
                    //    li = new ListItem("All", "0");
                    //    Chkboxlist_reportname.Items.Add(li);
                    //}

                    li = new ListItem(m_Myreader.GetValue(1).ToString(), m_Myreader.GetValue(0).ToString());
                    Chkboxlist_reportname.Items.Add(li);

                }

            }
            else
            {
                li = new ListItem("Item Not Found", "0");
                Chkboxlist_reportname.Items.Add(li);
            }


        }

        public void ShowMssage(int ScheduleId,string Type)
        {
            Lbl_err.Visible = false;
            if (Type == "View")
            {
               
                Txt_emailaddress.Enabled = false;
                Txt_misschedulename.Enabled = false;
                Radiobtn_periodtype.Enabled = false;
                Chkboxlist_reportname.Enabled = false;
                Btn_Update.Visible = false;
                Btn_magok.Text = "OK";
                Btn_magok.Focus();
            }
            else
            {
                Txt_emailaddress.Enabled = true;
                Txt_misschedulename.Enabled = true;
                Radiobtn_periodtype.Enabled = true;
                Chkboxlist_reportname.Enabled = true;
                Btn_Update.Visible = true;
                Btn_magok.Text = "Cancel";
                Btn_Update.Focus();
            }
            Temp_scheduleId = ScheduleId;
            clearcheckitems();

            LoadMISScheduleDeteils(ScheduleId);
            MPE_MessageBox.Show();
           
        }

        private void clearcheckitems()
        {
            for (int i = 0; i < Chkboxlist_reportname.Items.Count; i++)
                Chkboxlist_reportname.Items[i].Selected = false;
        }

        private void LoadMISScheduleDeteils(int ScheduleId)
        {
            DataSet Ds = null;
            string sql = "SELECT ScheduleName,EmailId,PeroidType from tblmisschedule where ScheduleId="+ScheduleId;
            Ds = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Ds.Tables[0].Rows)
                {
                    Txt_emailaddress.Text = dr["EmailId"].ToString();
                    Txt_misschedulename.Text = dr["ScheduleName"].ToString();
                    int _peroidtype = int.Parse(dr["PeroidType"].ToString());
                    Radiobtn_periodtype.SelectedValue = (_peroidtype - 1).ToString();
                }
            }
            Ds = null;
            sql="SELECT ReportId from tblmisschedulereportmap where ScheduleId="+ScheduleId;
            Ds = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

         
            
            if (Ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Ds.Tables[0].Rows)
                {
                    int id = int.Parse(dr["ReportId"].ToString());

                    Chkboxlist_reportname.Items[id - 1].Selected = true;

                }
            }
            
        }

        protected void Btn_Update_Click(object sender, EventArgs e)
        {
            try
            {
                int peroidtype = 1;
                for (int i = 0; i < Radiobtn_periodtype.Items.Count;i++ )
                {
                    if (Radiobtn_periodtype.Items[i].Selected == true)
                        peroidtype = i+1;
                }

                string sql = "UPDATE tblmisschedule SET ScheduleName='"+Txt_misschedulename.Text.Trim()+"',EmailId='"+Txt_emailaddress.Text.Trim()+"',PeroidType="+peroidtype+" WHERE ScheduleId="+Temp_scheduleId;
                MyFeeMang.m_MysqlDb.ExecuteQuery(sql);

                sql = "DELETE from tblmisschedulereportmap where ScheduleId="+Temp_scheduleId;
                MyFeeMang.m_MysqlDb.ExecuteQuery(sql);

                for (int i = 0; i < Chkboxlist_reportname.Items.Count; i++)
                {
                    if (Chkboxlist_reportname.Items[i].Selected == true)
                    {
                        int reportid = int.Parse(Chkboxlist_reportname.Items[i].Value.ToString());
                        sql = "insert into tblmisschedulereportmap (ScheduleId,ReportId) values (" + Temp_scheduleId + "," + reportid + ")";
                        MyFeeMang.m_MysqlDb.ExecuteQuery(sql);

                    }
                }
                Radiobtn_periodtype.Items[0].Selected = true;
                if (EVNTSave != null)
                {
                    EVNTSave(this, e);
                }



            }
            catch(Exception ex)
            {
                Lbl_err.Visible = true;
                Lbl_err.Text = ex.Message;
            }

        }
    }
}