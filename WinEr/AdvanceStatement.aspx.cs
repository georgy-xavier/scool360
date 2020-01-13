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
    public partial class AdvanceStatement : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
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
            MyFeeMang = MyUser.GetFeeObj();
            if (MyFeeMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(661))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadLastOneMonth();
                    LoadUser();
                    LoadAccountType();
                }
            }
        }

        private void LoadAccountType()
        {
            Drp_Type.Items.Clear();
            DataSet myDataset;
            string sql = "select tblaccounttype.Id,tblaccounttype.Name from tblaccounttype";
            myDataset = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li = new ListItem("All", "-1");
            Drp_Type.Items.Add(li);
            if (myDataset != null && myDataset.Tables != null && myDataset.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in myDataset.Tables[0].Rows)
                {
                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Type.Items.Add(li);
                }
            }
            else
                Drp_Type.SelectedIndex = 0;
        }

        private void LoadUser()
        {
            Drp_CollectedUser.Items.Clear();
            DataSet myDataset;
            string sql = "select DISTINCT (tblfeeadvancetransaction.CreatedUser) from tblfeeadvancetransaction";
            myDataset = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li = new ListItem("All User", "0");
            Drp_CollectedUser.Items.Add(li);
            if (myDataset != null && myDataset.Tables != null && myDataset.Tables[0].Rows.Count > 0)
            {
              
                foreach (DataRow dr in myDataset.Tables[0].Rows)
                {
                    li = new ListItem(dr[0].ToString(), dr[0].ToString());
                    Drp_CollectedUser.Items.Add(li);
                }
            }
            else
            Drp_CollectedUser.SelectedIndex = 0;
        }

        private void LoadLastOneMonth()
        {
            DateTime _Now = System.DateTime.Now;

            Txt_To.Text = General.GerFormatedDatVal(_Now);
            DateTime _Dt = _Now.AddDays(-7);
            Txt_from.Text = General.GerFormatedDatVal(_Dt) ;

        }

        protected void Btn_Load_Click(object sender, EventArgs e)
        {
            Grd_AdvanceItems.PageIndex = 0;
            string sql;
            try
            {
                DateTime _From = General.GetDateTimeFromText(Txt_from.Text);
                DateTime _To = General.GetDateTimeFromText(Txt_To.Text);
                if (_From > _To)
                {
                    WC_MessageBox.ShowMssage("To date should be greater than from date.");
                }
                else
                {

                    sql = "select tblfeeadvancetransaction.FeeName, tblfeeadvancetransaction.PeriodName, tblbatch.BatchName,tblfeeadvancetransaction.StudentName,tblfeeadvancetransaction.CreatedUser,date_format(tblfeeadvancetransaction.CreatedDate , '%d-%m-%Y') AS 'CreatedDate' , tblfeeadvancetransaction.BillNo ,tblaccounttype.Name , tblfeeadvancetransaction.Amount, tblfeeadvancetransaction.AdvanceBalance from tblfeeadvancetransaction INNER JOIN tblbatch on tblfeeadvancetransaction.BatchId= tblbatch.Id inner join tblaccounttype on tblaccounttype.Id= tblfeeadvancetransaction.`Type` where Date(tblfeeadvancetransaction.CreatedDate) >='" + _From.Date.ToString("s") + "' AND Date(tblfeeadvancetransaction.CreatedDate) <='" + _To.Date.ToString("s") + "'";
                    if (Drp_CollectedUser.SelectedValue != "0")
                    {
                        sql = sql + " AND tblfeeadvancetransaction.CreatedUser='" + Drp_CollectedUser.SelectedValue + "'";
                    }
                    if (Drp_Type.SelectedValue != "-1")
                    {
                        sql = sql + " AND tblfeeadvancetransaction.`Type`=" + Drp_Type.SelectedValue;
                    }
                    sql =sql+ " ORDER BY tblfeeadvancetransaction.CreatedDate";
                    MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (MydataSet.Tables[0].Rows.Count > 0)
                    {
                        ViewState["Advstate"] = sql;
                       
                        Grd_AdvanceItems.DataSource = MydataSet;
                        Grd_AdvanceItems.DataBind();
                      
                        Pnl_trans.Visible = true;
                    }
                    else
                    {
                        ViewState["Advstate"] = null;
                      
                        Grd_AdvanceItems.DataSource = null;
                        Grd_AdvanceItems.DataBind();
                       
                        Pnl_trans.Visible = false;
                        WC_MessageBox.ShowMssage("No transactions found.");
                    }

                } 
              

            }

            catch (Exception Ex)
            {
                WC_MessageBox.ShowMssage("No transactions found.Error" + Ex.Message);
            }
          
        
        }

        protected void Btn_exporttoexel_Click(object sender, ImageClickEventArgs e)
        {
            if (ViewState["Advstate"] != null)
            {
                  string sql = ViewState["Advstate"].ToString();
                  MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                  if (MydataSet.Tables[0].Rows.Count > 0)
                  {
                      string FileName = "Advance-Statement";

                      string _ReportName = "<table><tr><td colspan=\"8\" style=\"text-align:center;\"><b>Advance-Statement</b></td></tr><tr><td>Created Date:" + DateTime.Now.ToString() + "</td><td>From:" + Txt_from.Text + " TO:" + Txt_To.Text + " </td></tr></table>";
                      if (!WinEr.ExcelUtility.ExportDataToExcel(MydataSet, _ReportName, FileName, MyUser.ExcelHeader))
                      {
                          WC_MessageBox.ShowMssage("MS Excel is missing. Please install");
                      }
                  }
                  else
                  {
                      WC_MessageBox.ShowMssage("Unable to do the export");
                  }
            }
            else
            {
                WC_MessageBox.ShowMssage("Unable to do the export");
            }
        }

        protected void Grd_AdvanceItems_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
          
        }

        protected void Grd_AdvanceItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_AdvanceItems.PageIndex = e.NewPageIndex;
            if (ViewState["Advstate"] != null)
            {
                string sql = ViewState["Advstate"].ToString();
                MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    Grd_AdvanceItems.DataSource = MydataSet;
                    Grd_AdvanceItems.DataBind();

                    Pnl_trans.Visible = true;
                }

            }
        }
    }
}
