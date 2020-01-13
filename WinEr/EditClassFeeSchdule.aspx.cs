using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.Odbc;
namespace WinEr
{
    public partial class WebForm7 : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        private int BatchId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (Session["FeeId"] == null)
            {
                Response.Redirect("ManageFeeAccount.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyFeeMang = MyUser.GetFeeObj();
            if (MyFeeMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(43))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (Rdo_Batch.SelectedValue == "0")
                    BatchId = MyUser.CurrentBatchId;
                else
                    BatchId = MyUser.CurrentBatchId + 1;
                if (!IsPostBack)
                {
                    string _MenuStr;
                    _MenuStr = MyFeeMang.GetSubFeeMangMenuString(MyUser.UserRoleId, int.Parse(Session["FeeId"].ToString()));
                    this.SubFeeMenu.InnerHtml = _MenuStr;
                    if (MyFeeMang.HasNextBatchSchedule())
                    {
                        Label_NextBatch.Visible = true;
                        Rdo_Batch.Visible = true;
                    }
                    else
                    {
                        Label_NextBatch.Visible = false;
                        Rdo_Batch.Visible = false;
                    }
                    LoadDetails();
                    AddClassToDropDownClass();
                    AddPeriodToDrp();
                    LoadClassAssoSchduleDetails();
                    LoadStudents();
                  
                    //some initlization

                }
            }

        }

        private void LoadStudents()
        {
            lbl_Message.Text = "";
            GridViewAllFee.Columns[0].Visible = true;
            GridViewAllFee.Columns[1].Visible = true;
            DataSet Mytudents = null;
            int FeeschId = MyFeeMang.GetFeeScheduleId(int.Parse(Session["FeeId"].ToString()),Drp_Class.SelectedValue,Drp_Perod1.SelectedValue,BatchId);
            if (Drp_Class.SelectedValue != "-1" && Drp_Perod1.SelectedValue != "-1")
            {
                GridViewAllFee.Columns[0].Visible = true;
                GridViewAllFee.Columns[1].Visible = true;
                DataSet Fees = new DataSet();
                DataTable dt;
                DataRow dr;
                Fees.Tables.Add(new DataTable("ClassFees"));
                dt = Fees.Tables["ClassFees"];

                dt.Columns.Add("StudId");
                dt.Columns.Add("SchId");
                dt.Columns.Add("StudentName");
         
                dt.Columns.Add("ScheduledAmt");
                dt.Columns.Add("Paid");
                dt.Columns.Add("Balance");
                string sql = "select distinct tblstudent.Id , tblstudent.StudentName from tblstudentclassmap inner join tblstudent on tblstudent.Id = tblstudentclassmap.StudentId where tblstudentclassmap.ClassId=" + Drp_Class.SelectedValue + " and tblstudent.Status=1 order by tblstudent.StudentName asc";
                Mytudents = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (Mytudents != null && Mytudents.Tables != null && Mytudents.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr_Student in Mytudents.Tables[0].Rows)
                    {
                        sql = "select distinct tblfeestudent.SchId , tblfeestudent.Amount, (tblfeestudent.Amount - tblfeestudent.BalanceAmount) as Paid, tblfeestudent.BalanceAmount from  tblfeestudent where tblfeestudent.SchId=" + FeeschId + " and tblfeestudent.StudId=" + dr_Student[0].ToString();
                        //string sql = "select distinct tblstudent.Id ,tblfeestudent.SchId , tblstudent.StudentName , tblfeestudent.Amount, (tblfeestudent.Amount - tblfeestudent.BalanceAmount) as Paid, tblfeestudent.BalanceAmount from tblfeestudent inner join tblstudent on tblstudent.Id = tblfeestudent.StudId inner join tblfeeschedule on tblfeeschedule.Id = tblfeestudent.SchId inner join tblstudentclassmap on tblstudentclassmap.StudentId = tblstudent.Id where tblstudent.`Status`=1 and tblfeeschedule.PeriodId=" + Drp_Perod1.SelectedValue + " and tblstudentclassmap.ClassId=" + Drp_Class.SelectedValue + " and tblfeeschedule.FeeId=" + int.Parse(Session["FeeId"].ToString());
                        MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                        if (MyReader.HasRows)
                        {
                            dr = Fees.Tables["ClassFees"].NewRow();
                            dr["StudId"] = dr_Student[0].ToString();
                            dr["SchId"] = MyReader.GetValue(0).ToString();
                            dr["StudentName"] = dr_Student[1].ToString();

                            dr["ScheduledAmt"] = MyReader.GetValue(1).ToString();
                            dr["Paid"] = MyReader.GetValue(2).ToString();
                            dr["Balance"] = MyReader.GetValue(3).ToString();
                            Fees.Tables["ClassFees"].Rows.Add(dr);

                        }
                        else
                        {
                            dr = Fees.Tables["ClassFees"].NewRow();
                            dr["StudId"] = dr_Student[0].ToString();
                            dr["SchId"] = FeeschId;
                            dr["StudentName"] = dr_Student[1].ToString();
                            dr["ScheduledAmt"] = 0;
                            dr["Paid"] = 0;
                            dr["Balance"] = 0;
                            Fees.Tables["ClassFees"].Rows.Add(dr);

                        }
                    }
                    if (Fees != null && Fees.Tables != null && Fees.Tables[0].Rows.Count > 0)
                    {
                        GridViewAllFee.DataSource = Fees;
                        GridViewAllFee.DataBind();
                        Pnl_Students.Visible = true;
                        GridViewAllFee.Columns[0].Visible = false;
                        GridViewAllFee.Columns[1].Visible = false;
                    }
                    else
                    {
                        GridViewAllFee.DataSource = null;
                        GridViewAllFee.DataBind();
                        Pnl_Students.Visible = false;
                        lbl_Message.Text = "No students found";
                    }
                }
                else
                {
                    GridViewAllFee.DataSource = null;
                    GridViewAllFee.DataBind();
                    Pnl_Students.Visible = false;
                    lbl_Message.Text = "No students found";
                }

          
            }
            else
            {
                GridViewAllFee.DataSource = null;
                GridViewAllFee.DataBind();
                Pnl_Students.Visible = false;
                lbl_Message.Text = "No students found";
            }
        }
        private void LoadClassAssoSchduleDetails()
        {
            if (Drp_Class.SelectedValue != "-1" && Drp_Perod1.SelectedValue != "-1")
            {
                string sql = "select tblfeeschedule.Id,tblfeeschedule.Duedate,tblfeeschedule.LastDate ,tblfeeschedule.Amount from tblfeeschedule where FeeId=" + int.Parse(Session["FeeId"].ToString()) + " and ClassId=" + int.Parse(Drp_Class.SelectedValue.ToString()) + " and PeriodId=" + int.Parse(Drp_Perod1.SelectedValue.ToString()) + " and BatchId=" + BatchId;

                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    MyReader.Read();
                    // Txt_schduleId.Text = MyReader.GetValue(0).ToString();
                    DateTime _dt = DateTime.Parse(MyReader.GetValue(1).ToString());
                    //DateTime _dt = MyUser.GetDareFromText(MyReader.GetValue(1).ToString());
                    //Txt_dudate.Text = _dt.Date.ToString("dd/MM/yyyy");
                    Txt_From.Text = _dt.Date.Day + "/" + _dt.Date.Month + "/" + _dt.Date.Year;

                    DateTime _dt1 = DateTime.Parse(MyReader.GetValue(2).ToString());
                    //DateTime _dt1 = MyUser.GetDareFromText(MyReader.GetValue(2).ToString());
                    //Txt_lastdate.Text = _dt1.Date.ToString("dd/MM/yyyy");
                    Txt_To.Text = _dt1.Date.Day + "/" + _dt1.Date.Month + "/" + _dt1.Date.Year;
                    //sql = "SELECT tblfeestudent.Amount from tblfeestudent WHERE tblfeestudent.SchId=" + int.Parse(Txt_schduleId.Text.ToString());
                    //MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                    // if (MyReader.HasRows)
                    //{
                    //    MyReader.Read();
                    //Txt_amount.Text = MyReader.GetValue(3).ToString();
                    //Txt_amtbachup.Text = Txt_amount.Text;
                    //if (MyFeeMang.AnyOnePaid(Txt_schduleId.Text.ToString()))
                    //{
                    //    Txt_amount.ReadOnly = true;
                    //}
                    //else
                    //{
                    //    Txt_amount.ReadOnly = false;
                    //}

                    //}
                }
                else
                {
                    DateTime _dt = System.DateTime.Now;
                    Txt_From.Text = _dt.Date.Day + "/" + _dt.Date.Month + "/" + _dt.Date.Year;
                    DateTime _dt1 = System.DateTime.Now;
                    Txt_To.Text = _dt1.Date.Day + "/" + _dt1.Date.Month + "/" + _dt1.Date.Year;
                }
                MyReader.Close();

                //if (MyFeeMang.HaveNewStudWithOutFeeSchduled(int.Parse(Txt_schduleId.Text.ToString()), int.Parse(Drp_Class.SelectedValue.ToString()), MyUser.CurrentBatchId))
                //{

                //    TabPanel2.Visible = true;                  
                //    LoadNewStudGrid();

                //    //Btn_Schdule.Text = "Re-Sehedule";
                //}
                //else
                //{
                //    Tabs.ActiveTabIndex = 0;                  
                //    TabPanel2.Visible = false;


                //}
            }
            else
            {
                //ClearData();
                //Tabs.ActiveTabIndex = 0;
                ////TabPanel2.Visible = false;

                //// TabPanel2.Visible = false;
            }
        }

        protected void Grd_newstud_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string value = e.Row.Cells[3].Text;
                if (value == "-1")
                {
                    e.Row.Cells[3].Text = "No Roll No.";
                }
            }
        }

        
        private void AddPeriodToDrp()
        {

            Drp_Perod1.Items.Clear();
            if (Drp_Class.SelectedValue != "-1")
            {
                //if (!MyFeeMang.HaveStudentInClass(int.Parse(Drp_Class.SelectedValue.ToString()), MyUser.CurrentBatchId))
                //{
                //   // Lbl_msg.Text = "No Students in the class";
                //  //  this.MPE_MessageBox.Show();
                //    Btn_Update.Enabled = false;
                //    Drp_Perod1.Enabled = true;
                //    ListItem li = new ListItem("No Scheduled Periods", "-1");
                //    Drp_Perod1.Items.Add(li);

                //}

                //else
                //{



                //}

                string sql = "SELECT  tblperiod.Id, tblperiod.Period from tblperiod inner join tblfeeaccount on tblfeeaccount.FrequencyId = tblperiod.FrequencyId where tblfeeaccount.Id=" + int.Parse(Session["FeeId"].ToString()) + " AND tblperiod.Id IN (SELECT  tblfeeschedule.PeriodId from tblfeeschedule where tblfeeschedule.BatchId=" + BatchId + " AND tblfeeschedule.FeeId=" + int.Parse(Session["FeeId"].ToString()) + " AND tblfeeschedule.ClassId=" + int.Parse(Drp_Class.SelectedValue.ToString()) + ")";
                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        ListItem li = new ListItem(MyReader.GetValue(1).ToString(), int.Parse(MyReader.GetValue(0).ToString()).ToString());
                        Drp_Perod1.Items.Add(li);
                    }
                  //  Btn_Update.Enabled = true;

                }
                else
                {
                    ListItem li = new ListItem("No Scheduled Periods", "-1");
                   // Btn_Update.Enabled = false;
                    Drp_Perod1.Items.Add(li);


                }
            }

            else
            {

                ListItem li = new ListItem("No Periods", "-1");
                Drp_Perod1.Items.Add(li);
            }
        }
        private void AddClassToDropDownClass()
        {
            Drp_Class.Items.Clear();

            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {

                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class.Items.Add(li);

                }
               // Btn_Update.Enabled = true;

            }
            else
            {
                ListItem li = new ListItem("No Class Present", "-1");
                Drp_Class.Items.Add(li);
               // Btn_Update.Enabled = false;
            }

            Drp_Class.SelectedIndex = 0;

        }
        private void LoadDetails()
        {
            string sql = "SELECT tblfeeaccount.AccountName, tblfeefrequencytype.FreequencyName, tblfeeasso.Name from tblfeeaccount inner join tblfeefrequencytype on tblfeefrequencytype.Id= tblfeeaccount.FrequencyId inner join tblfeeasso on tblfeeasso.Id = tblfeeaccount.AssociatedId where tblfeeaccount.Id=" + int.Parse(Session["FeeId"].ToString());
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Lbl_FeeName.Text = MyReader.GetValue(0).ToString();
                Lbl_Freq.Text = MyReader.GetValue(1).ToString();
                Lbl_asso.Text = MyReader.GetValue(2).ToString();

            }
            MyReader.Close();
        }
        protected void Drp_Perod1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClassAssoSchduleDetails();
            LoadStudents();
        }

        
        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddPeriodToDrp();
            LoadClassAssoSchduleDetails();
            LoadStudents();
            
        }



        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageFeeAccount.aspx");
        }

        

        # region Edit all


        protected void Btn_applyAll_Click(object sender, EventArgs e)
        {
            TextBox NewAmount;
            if (Txt_New_Amount.Text.Trim() != "")
            {
                foreach (GridViewRow gv in GridViewAllFee.Rows)
                {
                    NewAmount = (TextBox)gv.FindControl("Txt_NewAmt");
                    NewAmount.Text = Txt_New_Amount.Text.Trim();
                }
            }
        }
        protected void Btn_UpdateAll_Click(object sender, EventArgs e)
        {
            double PaidAmt = 0;
            double NewAmt = 0;
            double ScheduledAmt = 0;
            double NewBalance = 0;
            int StudentId = 0;
            int SchedId = 0;
            int FeeschId = MyFeeMang.GetFeeScheduleId(int.Parse(Session["FeeId"].ToString()), Drp_Class.SelectedValue, Drp_Perod1.SelectedValue, BatchId);
            TextBox NewAmount;
            try
            {
                MyFeeMang.CreateTansationDb();
                DateTime _duedate = MyUser.GetDareFromText(Txt_From.Text.Trim());
                DateTime _lastdate = MyUser.GetDareFromText(Txt_To.Text.Trim());
                foreach (GridViewRow gv in GridViewAllFee.Rows)
                {
                    int.TryParse(gv.Cells[1].Text.ToString(), out StudentId);
                    int.TryParse(gv.Cells[0].Text.ToString(), out SchedId);
                    double.TryParse(gv.Cells[4].Text.ToString(), out PaidAmt);
                    double.TryParse(gv.Cells[3].Text.ToString(), out ScheduledAmt);
                    NewAmount = (TextBox)gv.FindControl("Txt_NewAmt");
                    double.TryParse(NewAmount.Text.Trim(), out NewAmt);
                    NewBalance = NewAmt - PaidAmt;
                    if ((NewBalance >= 0) && (ScheduledAmt != NewAmt))
                        MyFeeMang.UpdateAllClassFee(StudentId, SchedId, NewAmt, NewBalance);
                }
                string sql = "UPDATE tblfeeschedule SET Duedate= '" + _duedate.Date.ToString("s") + "', LastDate = '" + _lastdate.Date.ToString("s") + "', PeriodId=" + Drp_Perod1.SelectedValue + " WHERE Id =" + FeeschId;
                MyFeeMang.m_TransationDb.TransExecuteQuery(sql);
                MyFeeMang.EndSucessTansationDb();
                MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Fee Schedule Updation", Lbl_FeeName.Text + "Schedule is updated for " + Drp_Class.SelectedItem.Text, 1,1);

                LoadStudents();
                Lbl_msg.Text = "Amount updated";
                this.MPE_MessageBox.Show();

            }
            catch
            {
                Lbl_msg.Text = "Please try again";
                this.MPE_MessageBox.Show();
                MyFeeMang.EndFailTansationDb();
            }
        }

        protected void Rdo_Batch_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddPeriodToDrp();
            LoadStudents();
        }
        # endregion
    }
}
