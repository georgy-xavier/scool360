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
    public partial class Finesettings : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;


        #region Events

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
                else if (!MyUser.HaveActionRignt(906))
                {
                    Response.Redirect("RoleErr.htm");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        LoadInitails();
                        Pnl_FineSettings.Visible = false;
                        LoadFeeAmount();
                    }
                }
            }       

            protected void Btn_save_Click(object sender, EventArgs e)
            {
                string sql = "";
                string message = "";
                try
                {
                    if (FineDurationisValid(out message))
                    {
                        MyFeeMang.m_MysqlDb.MyBeginTransaction();

                        sql = "UPDATE tblconfiguration SET Value=" + Rdb_FineAmount.SelectedValue + " WHERE Name='Fine Amount'";
                        MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                        sql = "UPDATE tblconfiguration SET Value=" + Rdb_Finedate.SelectedValue + " WHERE Name='Fine Date'";
                        MyFeeMang.m_MysqlDb.ExecuteQuery(sql);

                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Fee Configuration", "Fee settings have been updated", 2, MyFeeMang.m_MysqlDb);

                        Lbl_Msg.Text = "Settings saved successfully!";

                        MyFeeMang.m_MysqlDb.TransactionCommit();
                        LoadInitails();
                    }
                    else
                    {
                        Lbl_Msg.Text = message;
                    }
                }
                catch (Exception)
                {
                    MyFeeMang.m_MysqlDb.TransactionRollback();
                    Lbl_Msg.Text = "Cannot save,Please try again later!";
                }
            }       

            protected void Btn_FineMAntSave_Click(object sender, EventArgs e)
            {
                TextBox txt_fine = new TextBox();
                Lbl_MsgErr.Text = "";
                string message="";
                int fineduration;
                TextBox txt_Duration = new TextBox();
                DropDownList drpfinetype = new DropDownList();
                DropDownList drpfinedate = new DropDownList();
                int Feeid = 0,freqncyId=0,Typeid=0,finetype=0,finedate=0;      
                double fineamount=0;
                try
                {
                    if (FineDurationisValid(out message))
                    {
                        MyFeeMang.m_MysqlDb.MyBeginTransaction();
                        foreach (GridViewRow gr in GridFineAmount.Rows)
                        {
                            drpfinetype = (DropDownList)gr.FindControl("Drp_FineType");
                            drpfinedate = (DropDownList)gr.FindControl("Drp_FineDate");
                            txt_Duration = (TextBox)gr.FindControl("TxtDuration");
                            txt_fine = (TextBox)gr.FindControl("TxtFine");
                            double.TryParse(txt_fine.Text, out fineamount);
                            if (fineamount <= 0)
                            {
                                fineduration = 0;
                            }
                            else
                            {
                                int.TryParse(txt_Duration.Text, out fineduration);
                            }
                            int.TryParse(drpfinetype.SelectedValue, out finetype);
                            int.TryParse(drpfinedate.SelectedValue, out finedate);
                            int.TryParse(gr.Cells[0].Text, out Feeid);
                            int.TryParse(gr.Cells[1].Text, out freqncyId);
                            int.TryParse(gr.Cells[2].Text, out Typeid);
                            MyFeeMang.SaveFineAmountToDatabase(fineamount, Feeid, Typeid, freqncyId, finetype, finedate, fineduration);

                        }
                        MyFeeMang.m_MysqlDb.TransactionCommit();
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Fee Configuration", "Fine config saved", 2, MyFeeMang.m_MysqlDb);
                        Lbl_MsgErr.Text = "Fine config saved successfully!";
                    }
                    else
                    {
                        Lbl_MsgErr.Text = message;
                    }
                }
                catch (Exception)
                {
                    Lbl_MsgErr.Text = "Cannot save,Please try later!";
                    MyFeeMang.m_MysqlDb.TransactionRollback();
                }
            }

            protected void Drp_FineType_SelectedIndexChanged(object sender, EventArgs e)
            {
                GridViewRow currentRow = (GridViewRow)(sender as DropDownList).Parent.Parent;
                DropDownList drpfine = new DropDownList();
                TextBox txtfine = new TextBox();
                int row = currentRow.RowIndex; 
                foreach (GridViewRow gr in GridFineAmount.Rows)
                {
                    txtfine = (TextBox)gr.FindControl("TxtDuration");
                    drpfine = (DropDownList)gr.FindControl("Drp_FineType");
                    if (gr.RowIndex == row)
                    {
                        if (drpfine.SelectedValue == "1" || drpfine.SelectedValue == "2")
                        {
                            txtfine.Text = "0";
                            txtfine.Enabled = false;
                        }
                        else
                        {
                            txtfine.Enabled = true;
                        }
                    }
                }
                
            }

            protected void GridFineAmount_SelectedIndexChanged(object sender, EventArgs  e)
            {
            }

        #endregion


        #region Methods

            private bool FineDurationisValid(out string message)
            {
                bool valid = true;
                message = "";
                TextBox txt_fine = new TextBox();
                TextBox txt_Duration = new TextBox();
                DropDownList drpfine = new DropDownList();
                double fineamount = 0;
                int fineduration;
                foreach (GridViewRow gr in GridFineAmount.Rows)
                {
                    txt_fine = (TextBox)gr.FindControl("TxtFine");
                    txt_Duration = (TextBox)gr.FindControl("TxtDuration");
                    drpfine = (DropDownList)gr.FindControl("Drp_FineType");
                    double.TryParse(txt_fine.Text, out fineamount);
                    int.TryParse(txt_Duration.Text, out fineduration);
                    if (fineamount < 0)
                    {
                        valid = false;
                        message = "Enter fine amount!";
                    }
                    else if((drpfine.SelectedValue!="1" && drpfine.SelectedValue!="2") && fineduration <= 0)
                    {
                        valid = false;
                        message = "Enter fine duration!";
                    }
                }
                return valid;
            }

        private void LoadFeeAmount()
        {
            DataSet FineAmountDs = new DataSet();
            FineAmountDs = MyFeeMang.GetAllFeeAmount();
            if (FineAmountDs != null && FineAmountDs.Tables[0].Rows.Count > 0)
            {
                GridFineAmount.Columns[0].Visible = true;
                GridFineAmount.Columns[1].Visible = true;
                GridFineAmount.Columns[2].Visible = true;
                GridFineAmount.DataSource = FineAmountDs;
                GridFineAmount.DataBind();
                BindFineAMount();
                GridFineAmount.Columns[0].Visible = false;
                GridFineAmount.Columns[1].Visible = false;
                GridFineAmount.Columns[2].Visible = false;
            }
            else
            {

                GridFineAmount.DataSource = null;
                GridFineAmount.DataBind();
            }
        }

        private void BindFineAMount()
        {
            string tempsql = "";
            TextBox Txt_fine = new TextBox();
            TextBox Txt_Duration = new TextBox();
            DropDownList drpfinetype = new DropDownList();
            DropDownList drpfinedate = new DropDownList();
            OdbcDataReader FineAMountReader = null;
            double fineamount = 0;
            int finetype = 0, finedate = 0;
            foreach (GridViewRow gr in GridFineAmount.Rows)
            {
                Txt_fine = (TextBox)gr.FindControl("TxtFine");
                Txt_Duration = (TextBox)gr.FindControl("TxtDuration");
                drpfinetype = (DropDownList)gr.FindControl("Drp_FineType");
                drpfinedate = (DropDownList)gr.FindControl("Drp_FineDate");
                tempsql = "select tblfine.Amount,FineAmounttype,Finedate,FineDuration  from tblfine where tblfine.FeeId=" + gr.Cells[0].Text + " ";
                FineAMountReader = MyFeeMang.m_MysqlDb.ExecuteQuery(tempsql);
                if (FineAMountReader.HasRows)
                {
                    double.TryParse(FineAMountReader.GetValue(0).ToString(), out fineamount);
                    int.TryParse(FineAMountReader.GetValue(1).ToString(), out finetype);
                    int.TryParse(FineAMountReader.GetValue(2).ToString(), out finedate);
                    Txt_fine.Text = fineamount.ToString();
                    if (finetype == 2)
                    {
                        drpfinetype.SelectedValue = "2";
                    }
                    else if (finetype == 3)
                    {
                        drpfinetype.SelectedValue = "3";
                        Txt_Duration.Enabled = true;

                    }
                    else if (finetype == 4)
                    {
                        drpfinetype.SelectedValue = "4";
                        Txt_Duration.Enabled = true;

                    }
                    else 
                    {
                        drpfinetype.SelectedValue = "1";

                    }
                    if (finedate == 2)
                    {
                        drpfinedate.SelectedValue = "2";
                    }
                    else
                    {
                        drpfinedate.SelectedValue = "1";
                    }
                    Txt_Duration.Text = FineAMountReader.GetValue(3).ToString();


                }
                else
                {
                    Txt_fine.Text = "0";
                    drpfinetype.SelectedValue = "1";
                    drpfinedate.SelectedValue = "1";
                }

            }

        }

        private void LoadInitails()
        {
            string sql = "";
            DataSet ConfigDs = new DataSet();
            string value = "";
            try
            {
                sql = "SELECT Value FROM tblconfiguration WHERE Name in ('Fine amount','Fine date')";
                ConfigDs = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (ConfigDs != null && ConfigDs.Tables[0].Rows.Count > 0)
                {
                    if (ConfigDs.Tables[0].Rows[0][0].ToString() == "2")
                    {
                        Rdb_Finedate.SelectedValue = "2";
                    }
                    else
                    {
                        Rdb_Finedate.SelectedValue = "1";
                    }
                    if (ConfigDs.Tables[0].Rows[1][0].ToString() == "2")
                    {
                        Rdb_FineAmount.SelectedValue = "2";
                    }
                    else
                    {
                        Rdb_FineAmount.SelectedValue = "1";
                    }

                }
            }
            catch (Exception)
            {
                Lbl_Msg.Text = "Error,Cannot load initial datas!";
            }

        }

        #endregion
    }
}
