using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;

namespace WinEr.WebControls
{
    public partial class AdvanceSettelmentControl : System.Web.UI.UserControl
    {
        private KnowinUser MyUser;
        private OdbcDataReader m_Myreader = null;
        private FeeManage MyFeeMang;
        public event EventHandler FeeAdvanceCancled;
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
        public void Display(int _studentID)
        {
            HiddenField_StudentId.Value = _studentID.ToString();
           
            LoadDatas();
           
            MPE_AdvanceSettelment.Show();
            
        }

        private void LoadDatas()
        {
            Lbl_msg.Text = "";
            Btn_AdvcancelSave.Enabled = true;
            LoadFeeschdules();
            LoadAdvances();
        }

        private void LoadFeeschdules()
        {
            Drp_FeeSchedule.Items.Clear();
            ListItem Li;
            string sql = "select tblfeestudent.Id, tblfeeaccount.AccountName,tblbatch.BatchName, tblperiod.Period, tblfeestudent.BalanceAmount from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblstudent on tblstudent.Id = tblfeestudent.StudId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId where tblfeestudent.StudId=" + HiddenField_StudentId.Value + " AND tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion' AND tblfeeaccount.Status=1 ";
            m_Myreader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
           if (m_Myreader.HasRows)
           {
              
              
               while (m_Myreader.Read())
               {
                   Li = new ListItem(m_Myreader.GetValue(1).ToString() + " | " + m_Myreader.GetValue(2).ToString() + " | " + m_Myreader.GetValue(3).ToString() + " | " + m_Myreader.GetValue(4).ToString(), m_Myreader.GetValue(0).ToString());
                   Drp_FeeSchedule.Items.Add(Li);
               }
              
           }
           else
           {
               Li = new ListItem("No Fee Schedule Found", "-1");
               Drp_FeeSchedule.Items.Add(Li);
               Btn_AdvcancelSave.Enabled = false;
           }
        }

        private void LoadAdvances()
        {
            Drp_Advance.Items.Clear();
            ListItem Li;
            string sql = "select tblstudentfeeadvance.Id,tblstudentfeeadvance.FeeName, tblstudentfeeadvance.PeriodName, tblbatch.BatchName , tblstudentfeeadvance.Amount from tblstudentfeeadvance inner join  tblbatch on tblstudentfeeadvance.BatchId= tblbatch.Id where tblstudentfeeadvance.StudentId=" + HiddenField_StudentId.Value;
            m_Myreader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (m_Myreader.HasRows)
            {


                while (m_Myreader.Read())
                {
                    Li = new ListItem(m_Myreader.GetValue(1).ToString() + " | " + m_Myreader.GetValue(2).ToString() + " | " + m_Myreader.GetValue(3).ToString() + " | " + m_Myreader.GetValue(4).ToString(), m_Myreader.GetValue(0).ToString());
                    Drp_Advance.Items.Add(Li);
                }

            }
            else
            {
                Li = new ListItem("No Fee Advance Found", "-1");
                Drp_Advance.Items.Add(Li);
                Btn_AdvcancelSave.Enabled = false;
            }
        }

        protected void Btn_AdvcancelSave_Click(object sender, EventArgs e)
        {
            if (Drp_Advance.SelectedValue != "-1" && Drp_FeeSchedule.SelectedValue != "-1")
            {
                try
                {
                    MyFeeMang.CreateTansationDb();

                    if (MyFeeMang.AdvanceSettelment(int.Parse(Drp_Advance.SelectedValue), int.Parse(Drp_FeeSchedule.SelectedValue)))
                    {
                        MyFeeMang.EndSucessTansationDb();
                        FeeAdvanceCancled(this, e);
                        LoadDatas();
                        Lbl_msg.Text = "Advance settlement successful.";
                    }
                    else
                    {
                        LoadDatas();
                        Lbl_msg.Text = "Unable to do the settlement. Please try again.";
                    }


                }
                catch (Exception Ex)
                {
                    Lbl_msg.Text = "Unable to do the settlement. Please try again. Error : " + Ex.Message;
                    MyFeeMang.EndFailTansationDb();
                }
            }
            MPE_AdvanceSettelment.Show();
        }

       
    }
}