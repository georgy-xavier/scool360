using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;
using System.Data.Odbc;

namespace WinEr
{
    public partial class EditTransportationFee : System.Web.UI.Page
    {

        private TransportationClass MyTransMang;
        private KnowinUser MyUser;
        private FeeManage MyFeeMang;
        private int MasterBatchId;
        private OdbcDataReader MyReader = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyTransMang = MyUser.GetTransObj();
            MyFeeMang = MyUser.GetFeeObj();
            if (MyTransMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(852))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (Rdo_Batch.SelectedValue == "0")
                    MasterBatchId = MyUser.CurrentBatchId;
                else
                    MasterBatchId = MyUser.CurrentBatchId + 1;

                if (!IsPostBack)
                {
                    LoadLocationToDropdown();
                    AddClassToDropDownClass();
                    AddPeriodToDrp();
                    LoadDataToGrid();
                }

            }
        }
        protected void Drp_class2_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddPeriodToDrp();
            LoadDataToGrid();
        }
        protected void Drp_Location_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataToGrid();
        }

        protected void Rdo_Batch_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddPeriodToDrp();
            LoadDataToGrid();
        }

        protected void Drp_Perod2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Txt_DueStud.Text = "";
            Txt_LastStud.Text = "";
            LoadDataToGrid();
        }

        protected void Btn_Edit_Click(object sender, EventArgs e)
        {
            int check=0;
            if (Txt_DueStud.Text == "" || Txt_LastStud.Text == "")
            {
                WC_MessageBox.ShowMssage("Please enter due date and last date!");
            }
            else
            {
                foreach (GridViewRow gr in Grd_Amound.Rows)
                {
                    CheckBox chk = (CheckBox)gr.FindControl("ChkFee");
                    TextBox amount = (TextBox)gr.FindControl("Txt_NewAmount");
                    if (chk.Checked)
                    {
                        int classid = int.Parse(gr.Cells[2].Text);
                        int StudID = int.Parse(gr.Cells[3].Text);
                        int periodId = int.Parse(Drp_Perod2.SelectedValue.ToString());
                        int batchId = MasterBatchId;
                        check = 1;
                        double feeamount = double.Parse(amount.Text);
                        DateTime _duedate = General.GetDateTimeFromText(Txt_DueStud.Text);
                        DateTime _lastdate = General.GetDateTimeFromText(Txt_LastStud.Text);
                        MyTransMang.UpdateStudFeeSchedule_InTransporation(_duedate, _lastdate, classid, periodId, batchId);
                        MyTransMang.EditFee_Intransaportation(classid, StudID, periodId, batchId, feeamount);
                        string studname = gr.Cells[4].Text;
                        string classname = gr.Cells[5].Text;
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Edit transportation fee", "Transportation fee of " + studname + " of class " + classname + " is updated to " + feeamount + "", 1);
                    }
                }
                if (check == 0)
                {
                    WC_MessageBox.ShowMssage("Select any student");
                }
                else
                {
                    WC_MessageBox.ShowMssage("Fee Updated for the selected students");
                    LoadDataToGrid();
                }
            }
        }

        #region Methods

        private void LoadDataToGrid()
        {
            Lbl_Err.Text = "";
            DataSet StudentDetails_Ds = new DataSet();
            int LocationId = int.Parse(Drp_Location.SelectedValue.ToString());
            int periodId = int.Parse(Drp_Perod2.SelectedValue.ToString());
            int classId = int.Parse(Drp_class2.SelectedValue.ToString());
            int currentbatchId = MyUser.CurrentBatchId;
            if (LocationId != -1 && periodId!=-1 && classId!=-1)           
            {
                StudentDetails_Ds = MyTransMang.GetScheduledStudentDetails(LocationId, MasterBatchId, periodId, classId, currentbatchId);
                if (StudentDetails_Ds != null && StudentDetails_Ds.Tables[0].Rows.Count > 0)
                {
                 
                    Grd_Amound.Columns[3].Visible = true;
                    Grd_Amound.Columns[1].Visible = true;
                    Grd_Amound.Columns[2].Visible = true;
                    Grd_Amound.DataSource = StudentDetails_Ds;
                    Grd_Amound.DataBind();
                    Pnl_AssStud.Visible = true;
                    Grd_Amound.Columns[3].Visible = false; ;
                    Grd_Amound.Columns[1].Visible = false;
                    Grd_Amound.Columns[2].Visible = false;
                    FillAmountAndStaus();
                }
                else
                {
                    Lbl_Err.Text = "No students found";
                    Grd_Amound.DataSource = null;
                    Grd_Amound.DataBind();
                    Pnl_AssStud.Visible = false;
                }
            }
            else
            {
                Lbl_Err.Text = "No students found";
                Grd_Amound.DataSource = null;
                Grd_Amound.DataBind();
                Pnl_AssStud.Visible = false;
            }
          
        }

        private void FillAmountAndStaus()
        {
            OdbcDataReader Amountreader = null;
            foreach (GridViewRow gv in Grd_Amound.Rows)
            {
                int classId = int.Parse(gv.Cells[2].Text);
                int periodId = int.Parse(Drp_Perod2.SelectedValue.ToString());
                int BatchId = MasterBatchId;
                string sql = "select tblfeeschedule.Id,tblfeeschedule.Duedate,tblfeeschedule.LastDate from tblfeeschedule where FeeId=100 and ClassId=" + classId + " and PeriodId=" + int.Parse(Drp_Perod2.SelectedValue.ToString()) + " and BatchId=" + MasterBatchId;
                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    MyReader.Read();
                    Txt_schduleId.Text = MyReader.GetValue(0).ToString();
                }

                sql = "SELECT tblfeestudent.BalanceAmount as Amount,tblfeestudent.Status from tblfeestudent WHERE tblfeestudent.StudId=" + int.Parse(gv.Cells[3].Text.ToString()) + " AND tblfeestudent.SchId=" + int.Parse(Txt_schduleId.Text.ToString());
                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                TextBox TxtAmount = (TextBox)gv.FindControl("Txt_NewAmount");
                TxtAmount.ReadOnly = false;
                if (MyReader.HasRows)
                {
                    MyReader.Read();


                    TxtAmount.Text = MyReader.GetValue(0).ToString();
                    TextBox Txtstatus = (TextBox)gv.FindControl("Txt_Status");
                    Txtstatus.Text = MyReader.GetValue(1).ToString();
                    if (MyReader.GetValue(1).ToString() != "Scheduled")
                    {
                        TxtAmount.ReadOnly = true;
                    }
                }

            }

            MyReader.Close();

        }

       
        private void AddClassToDropDownClass()
        {
            Drp_class2.Items.Clear();
            DataSet MydataSet = new DataSet();
            MydataSet = MyUser.MyAssociatedClass();
            ListItem li;
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                li = new ListItem("All", "0");
                Drp_class2.Items.Add(li);
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {

                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_class2.Items.Add(li);

                }
            }
            else
            {
                li = new ListItem("No Class Present", "-1");
                Drp_class2.Items.Add(li);
            }

            Drp_class2.SelectedIndex = 0;

        }

        private void AddPeriodToDrp()
        {
            Drp_Perod2.Items.Clear();
            DataSet Class_Ds=new DataSet();
             string sql = "";
            int classId = int.Parse(Drp_class2.SelectedValue.ToString());
            if (classId == 0)
            {
                DataSet ClassDs = new DataSet();
                DataSet Period_Ds = new DataSet();
                DataSet Period = new DataSet();
                DataTable dt;
                DataRow _dr;
                Period_Ds.Tables.Add(new DataTable("Class"));
                dt = Period_Ds.Tables["Class"];
                dt.Columns.Add("PeriodId");
                dt.Columns.Add("Period");
                DataTable _dt;
                DataRow _drd;
                Period.Tables.Add(new DataTable("Period"));
                _dt = Period.Tables["Period"];
                _dt.Columns.Add("Id");
                _dt.Columns.Add("Periodname");
                Class_Ds = MyUser.MyAssociatedClass();

                if (MyTransMang.HaveStudentInClass(int.Parse(Drp_class2.SelectedValue.ToString()), MyUser.CurrentBatchId, Class_Ds, out Class_Ds))
                {
                    foreach (DataRow dr in Class_Ds.Tables[0].Rows)
                    {


                        sql = "SELECT distinct  tblperiod.Id, tblperiod.Period from tblperiod inner join tblfeeaccount on tblfeeaccount.FrequencyId = tblperiod.FrequencyId where tblfeeaccount.Id=100 AND tblperiod.Id IN (SELECT  tblfeeschedule.PeriodId from tblfeeschedule where tblfeeschedule.BatchId=" + MasterBatchId + " AND tblfeeschedule.FeeId=100 AND tblfeeschedule.ClassId=" + int.Parse(dr["Id"].ToString()) + ") order by  tblperiod.order";
                        MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                        if (MyReader.HasRows)
                        {
                            while (MyReader.Read())
                            {
                                _dr = Period_Ds.Tables["Class"].NewRow();
                                _dr["PeriodId"] = MyReader.GetValue(0).ToString();
                                _dr["Period"] = MyReader.GetValue(1).ToString();
                                Period_Ds.Tables["Class"].Rows.Add(_dr);
                            }
                        }
                    }
                    if (Period_Ds != null && Period_Ds.Tables[0].Rows.Count > 0)
                    {


                        foreach (DataRow dr in Period_Ds.Tables[0].Rows)
                        {
                            int count = 0;
                            if (Period != null && Period.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow DR in Period.Tables[0].Rows)
                                {
                                    if (DR["Id"].ToString() == dr["PeriodId"].ToString())
                                    {
                                        count = 1;
                                    }
                                }
                                if (count == 0)
                                {
                                    _drd = Period.Tables["Period"].NewRow();
                                    _drd["Id"] = dr["PeriodId"].ToString();
                                    _drd["Periodname"] = dr["Period"].ToString();
                                    Period.Tables["Period"].Rows.Add(_drd);
                                }
                            }
                            else
                            {
                                _drd = Period.Tables["Period"].NewRow();
                                _drd["Id"] = dr["PeriodId"].ToString();
                                _drd["Periodname"] = dr["Period"].ToString();
                                Period.Tables["Period"].Rows.Add(_drd);
                            }


                        }
                    }
                    if (Period != null && Period.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in Period.Tables[0].Rows)
                        {
                            ListItem li = new ListItem(dr["Periodname"].ToString(), dr["Id"].ToString());
                            Drp_Perod2.Items.Add(li);
                        }
                    }
                    else
                    {
                        ListItem li = new ListItem("No Scheduled Periods", "-1");
                        Drp_Perod2.Items.Add(li);
                    }
                }

                else
                {
                    ListItem li = new ListItem("No Scheduled Periods", "-1");
                    Drp_Perod2.Items.Add(li);
                }
            }
            else if (classId > 0)
            
            {
                sql = "SELECT  tblperiod.Id, tblperiod.Period from tblperiod inner join tblfeeaccount on tblfeeaccount.FrequencyId = tblperiod.FrequencyId where tblfeeaccount.Id=100 AND tblperiod.Id IN (SELECT  tblfeeschedule.PeriodId from tblfeeschedule where tblfeeschedule.BatchId=" + MasterBatchId + " AND tblfeeschedule.FeeId=100 AND tblfeeschedule.ClassId=" + int.Parse(Drp_class2.SelectedValue.ToString()) + ")";
                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        ListItem li = new ListItem(MyReader.GetValue(1).ToString(), int.Parse(MyReader.GetValue(0).ToString()).ToString());
                        Drp_Perod2.Items.Add(li);
                    }

                  
                    // LoadStudentsToGrid();
                }
                else
                {
                    ListItem li = new ListItem("No Scheduled Periods", "-1");

                    Drp_Perod2.Items.Add(li);
                    // ClearGrid();
                }
            }            
            if (Drp_class2.SelectedValue == "-1")
            {
                // ClearGrid();
                ListItem li = new ListItem("No Periods", "-1");
                Drp_Perod2.Items.Add(li);
            }
        }

        private void LoadLocationToDropdown()
        {
            Drp_Location.Items.Clear();
            DataSet Location_Ds = new DataSet();
            ListItem li;
            Location_Ds = MyTransMang.getDestinationsAll();
            if (Location_Ds != null && Location_Ds.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("All", "0");
                Drp_Location.Items.Add(li);
                foreach (DataRow dr in Location_Ds.Tables[0].Rows)
                {
                    li = new ListItem(dr["Destination"].ToString(), dr["id"].ToString());
                    Drp_Location.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No location found", "-1");
                Drp_Location.Items.Add(li);
            }
        }

        #endregion

        

       

       

    }
}
