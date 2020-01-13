using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;
using System.Text;

namespace WinEr
{
    public partial class ScheduleTranportationFee : System.Web.UI.Page
    {

        private TransportationClass MyTransMang;
        private KnowinUser MyUser;
        private FeeManage MyFeeMang;
        private int MasterBatchId;

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
            else if (!MyUser.HaveActionRignt(850))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (Rdo_Batch1.SelectedValue == "0")
                    MasterBatchId = MyUser.CurrentBatchId;
                else
                    MasterBatchId = MyUser.CurrentBatchId + 1;
                if (!IsPostBack)
                {

                        Lbl_msg.Text = "";
                        LoadLocationToDropdown();
                        AddPeriodToDrp();
                        LoadClassTodropDown();
                        LoadGrid();
                        CheckAmountAddesToDestination();

                }

            }
        }



  

        protected void Drp_Location_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
            
        }
        protected void Drp_Period_SelectedIndexChanged(object sender, EventArgs e)
        {
            Txt_DueDate.Text = "";
            Txt_LastDate.Text = "";
            LoadGrid();
            CheckAmountAddesToDestination();
        }

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }
        protected void Rdo_Batch1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void Btn_Schedule_Click(object sender, EventArgs e)
        {
            int Save=0;
            int Feeid = 100;
            int count = 0;
            //Id,FeeId,BatchId,Duedate,LastDate,Status,ClassId,PeriodId,Amount
            if (Txt_DueDate.Text == "" || Txt_LastDate.Text == "")
            {
                WC_MessageBox.ShowMssage("Please enter due date and last date!");
            }
            else if (int.Parse(Drp_Period.SelectedValue) == 0)
            {
                WC_MessageBox.ShowMssage("Please select period!");
            }
            else
            {
                try
                {
                    Save = 1;
                    double amount;
                    DateTime DueDate = General.GetDateTimeFromText(Txt_DueDate.Text);
                    DateTime LastDate = General.GetDateTimeFromText(Txt_LastDate.Text);
                    int PeriodId = int.Parse(Drp_Period.SelectedValue.ToString());
                    int BatchId = MasterBatchId;
                    bool _continue = true;
                    MyFeeMang.CreateTansationDb();
                    MyFeeMang.ClearTempVariables();
                    foreach (GridViewRow gr in Grd_ScrechStud.Rows)
                    {
                       
                        TextBox _Cost = (TextBox)gr.FindControl("Txt_NewAmount");
                        CheckBox chk = (CheckBox)gr.FindControl("ChkFee");
                        if (chk.Checked)
                        {
                            count = 1;
                            if (_Cost.Text.Trim() != "" && double.TryParse(_Cost.Text, out amount))
                            {
                                int classid = int.Parse(gr.Cells[1].Text);
                                int StudID = int.Parse(gr.Cells[2].Text);
                                amount = double.Parse(_Cost.Text);
                                if (_continue)
                                {
                                    _continue = MyFeeMang.ScheduleFeeToStudent(Feeid, StudID, classid, PeriodId, BatchId, DueDate, LastDate, amount);
                                    string studname = gr.Cells[4].Text;
                                    string classname = gr.Cells[5].Text;
                                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Schedule transportation fee", "A transportation fee of Rs." + amount + " is scheduled to " + studname + " of class " + classname + "", 1, MyFeeMang.m_TransationDb);

                                }
                            }
                        }


                        //  MyTransMang.SaveDetailsToTable(BatchId, DueDate, LastDate, classid, PeriodId, amount, StudID);
                    }

                    if (count == 0)
                    {
                        WC_MessageBox.ShowMssage("select any student to schedule fee!");
                        MyFeeMang.EndFailTansationDb();
                        LoadGrid();
                    }
                    else
                    {
                        if (_continue)
                        {
                            MyFeeMang.EndSucessTansationDb();
                            // MyUser.m_DbLog.LogToDb(MyUser.UserName, "Student Fee Scheduling", Lbl_FeeName.Text + " is Scheduled to " + Drp_className1.SelectedItem.Text, 1);

                            if (Save == 1)
                            {
                                LoadGrid();
                                WC_MessageBox.ShowMssage("Fee scheduled for the selected students!");
                            }
                        }

                        else
                        {
                            MyFeeMang.EndFailTansationDb();
                            LoadGrid();
                            //GetFoundStudCountString();
                            WC_MessageBox.ShowMssage("Scheduled Failed, Please try again!");
                            // _message = "Scheduled Failed, Please try again ";
                            //MyUser.m_DbLog.LogToDb(MyUser.UserName, "Student Fee Scheduling", Lbl_FeeName.Text + " Scheduling is failed for " + Drp_className1.SelectedItem.Text, 1);
                        }
                    }
                }
                    
                catch (Exception Ex)
                {
                    MyFeeMang.EndFailTansationDb();
                    LoadGrid();
                    // GetFoundStudCountString();
                   // _message = Ex.Message;
                }
            }


        }

        protected void Btn_SalereportCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("BusFeeManagr.aspx");
        }


        #region Methods


        private void CheckAmountAddesToDestination()
        {           
            int count = 1;
            string alldestination = "";
            DataSet des_Ds = new DataSet();
            DataRow dr;
            DataTable dt;
            des_Ds.Tables.Add(new DataTable("DesID"));
            dt = des_Ds.Tables["DesID"];
            dt.Columns.Add("Id");
            dt.Columns.Add("name");
            foreach (GridViewRow gr in Grd_ScrechStud.Rows)
            {
                TextBox _Cost = (TextBox)gr.FindControl("Txt_NewAmount");
                if (int.Parse(_Cost.Text) == 0)
                {
                   
                    if (count > 1)
                    {
                        string destn=gr.Cells[0].Text;
                        int value = 0;
                       
                        foreach (DataRow _dr in des_Ds.Tables[0].Rows)
                        {
                            if (destn == _dr["Id"].ToString())
                            {
                                value = 1;
                            }
                        }
                        if (value == 0)
                        {
                            dr = des_Ds.Tables["DesID"].NewRow();
                            dr["Id"] = gr.Cells[0].Text;
                            dr["name"] = gr.Cells[7].Text;
                            des_Ds.Tables["DesID"].Rows.Add(dr);
                            
                        }
                    }
                    if (count == 1)
                    {
                        dr = des_Ds.Tables["DesID"].NewRow();
                        dr["Id"] = gr.Cells[0].Text;
                        dr["name"] = gr.Cells[7].Text;
                        des_Ds.Tables["DesID"].Rows.Add(dr);

                    }
                    count++;
                }
            }

            int cnt = 0;
            foreach (DataRow Dr in des_Ds.Tables[0].Rows)
            {
                if (cnt == 0)
                {
                    alldestination = Dr["name"].ToString();
                    cnt = 1;
                }
                else
                {
                    alldestination = alldestination + "," + Dr["name"].ToString();
                   
                  
                }
            }
            if (alldestination != "")
            {
                StringBuilder amountstr = new StringBuilder();
                amountstr.Append("<table width=\"100%\">");
                amountstr.Append("<tr><td style=\"color:Red\">You have not added cost for " + alldestination + ".Click on ok to set it now.</td></tr>");
                amountstr.Append("</table>");
                Div_salereport.InnerHtml = amountstr.ToString();
                MPE_SALEREPORT.Show();
            }

        }

        private void LoadGrid()
        {
            Lbl_Error.Text = "";
            DataSet StudentDetails_Ds = new DataSet();
            int LocationId = int.Parse(Drp_Location.SelectedValue.ToString());
            int periodId = int.Parse(Drp_Period.SelectedValue.ToString());
            int classId = int.Parse(Drp_Class.SelectedValue.ToString());
            int currentbatchId = MyUser.CurrentBatchId;
            if (LocationId != -1 && periodId!=-1 && classId!=-1)
            {
                StudentDetails_Ds = MyTransMang.GetStudentDetails(LocationId, MasterBatchId, periodId, currentbatchId, classId);
                if (StudentDetails_Ds != null && StudentDetails_Ds.Tables[0].Rows.Count > 0)
                {
                  
                    Grd_ScrechStud.Columns[0].Visible = true;
                    Grd_ScrechStud.Columns[1].Visible = true;
                    Grd_ScrechStud.Columns[2].Visible = true;
                    Grd_ScrechStud.DataSource = StudentDetails_Ds;
                    Grd_ScrechStud.DataBind();
                    Pnl_DisplayDetails.Visible = true;
                    Grd_ScrechStud.Columns[0].Visible = false; ;
                    Grd_ScrechStud.Columns[1].Visible = false;
                    Grd_ScrechStud.Columns[2].Visible = false;
                    FillTextBox();
                }
                else
                {
                    Lbl_Error.Text = "No students found";
                    Grd_ScrechStud.DataSource = null;
                    Grd_ScrechStud.DataBind();
                    Pnl_DisplayDetails.Visible = false;
                }
            }
            else
            {
                Lbl_Error.Text = "No students found";
                Grd_ScrechStud.DataSource = null;
                Grd_ScrechStud.DataBind();
                Pnl_DisplayDetails.Visible = false;
            }
        }

        private void FillTextBox()
        {
            foreach (GridViewRow gr in Grd_ScrechStud.Rows)
            {
                TextBox _Cost = (TextBox)gr.FindControl("Txt_NewAmount");
                int Id = int.Parse(gr.Cells[0].Text.ToString());
                double Cost = MyTransMang.GetCost(Id);
                _Cost.Text = Cost.ToString();
                    
            }
        }

        private void AddPeriodToDrp()
        {
            Drp_Period.Items.Clear();
            DataSet Period_Ds = new DataSet();
            ListItem li;
            Period_Ds = MyTransMang.GetPeriod();
            if (Period_Ds != null && Period_Ds.Tables[0].Rows.Count > 0)
            {
               
                foreach (DataRow dr in Period_Ds.Tables[0].Rows)
                {
                    //tblperiod.Id, tblperiod.Period
                    li = new ListItem(dr["Period"].ToString(), dr["Id"].ToString());
                    Drp_Period.Items.Add(li);
                }
            }
            Drp_Period.SelectedIndex = 0;
            
           
        }

        private void LoadLocationToDropdown()
        {
            Drp_Location.Items.Clear();
            DataSet Location_Ds = new DataSet();
            ListItem li;
           Location_Ds= MyTransMang.getDestinationsAll();
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

        private void LoadClassTodropDown()
        {
            Drp_Class.Items.Clear();
            DataSet MydataSet = new DataSet();
            MydataSet = MyUser.MyAssociatedClass();
            ListItem li;
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                li = new ListItem("All", "0");
                Drp_Class.Items.Add(li);
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {

                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class.Items.Add(li);

                }
            }
            else
            {
                li = new ListItem("No Class Present", "-1");
                Drp_Class.Items.Add(li);
            }

            Drp_Class.SelectedIndex = 0;
        }

        #endregion

      









    }
}
