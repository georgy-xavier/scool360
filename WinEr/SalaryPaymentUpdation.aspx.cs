using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;
using System;
using System.Drawing;

namespace WinEr
{
    public partial class SalaryPaymentUpdation : System.Web.UI.Page
    {
        private WinBase.Payroll Mypay;
        private KnowinUser MyUser;
        private DataSet MydataSet = null;
        private OdbcDataReader MypayReader = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            
            MyUser = (KnowinUser)Session["UserObj"];
            Mypay = MyUser.GetPayrollObj();
            if (Mypay == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(806))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (!IsPostBack)
                {
                    LoadYearToDrpList();
                    LoadMonthToDrpList();
                    RdbNonPayed.Checked = true;
                    LoadNonPayedGrid();

                    //some initlization

                }
            }
        }


        private void LoadYearToDrpList()
        {
            MydataSet = null;
            DrpYear.Items.Clear();
            int i = 0;
            MydataSet = Mypay.FillYearDrp();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Dr_Year in MydataSet.Tables[0].Rows)
                {
                    ListItem li = new ListItem(Dr_Year[0].ToString(), i.ToString());
                    DrpYear.Items.Add(li);
                    i++;
                }
                DrpYear.SelectedValue = (i - 1).ToString();
                //int CurrentYear = DateTime.Now.Year;
                //DrpYear.SelectedValue = CurrentYear.ToString();

            }
            else
            {
                DateTime _Current = DateTime.Now.Date;

                ListItem li = new ListItem(_Current.Year.ToString(), "1");
                DrpYear.Items.Add(li);
            }

        }

        private void LoadNonPayedGrid()
        {
            int unresn=0;
            BtnSave.Visible = true;
            int year = int.Parse(DrpYear.SelectedItem.Text.ToString());
            int MonthId = int.Parse(Drp_Month.SelectedValue.ToString());
            DataSet Unresignedds = Mypay.GetNonPayedMonthEmp(MonthId, year);
            DataSet MyEmp = new DataSet();
            MyEmp = GetUnresignedData(Unresignedds, unresn,year,MonthId);
            if (MyEmp != null && MyEmp.Tables != null && MyEmp.Tables[0].Rows.Count > 0)
            {
                Grd_EmpPay.Columns[0].Visible = true;
                Grd_EmpPay.Columns[9].Visible = true;
                Grd_EmpPay.Columns[11].Visible = true;
                Grd_EmpPay.Columns[10].Visible = true;

                Grd_EmpPay.DataSource = MyEmp;
                Grd_EmpPay.DataBind();
                Grd_EmpPay.Columns[0].Visible = false;
                Grd_EmpPay.Columns[11].Visible = false;
                Grd_EmpPay.Columns[10].Visible = false;

                Pnl_EmpPay.Visible = true;
                Lblmsg.Text = "";

            }
            else
            {
                Grd_EmpPay.DataSource = null;
                Grd_EmpPay.DataBind();
                Lblmsg.Text = "No staff found for this month";
            }
            ColorGrid(unresn);
        }

        private void ColorGrid(int resn)
        {
            foreach (GridViewRow gv in Grd_EmpPay.Rows)
            {
                string resgndate = "";
                resgndate = gv.Cells[8].Text.Replace("&nbsp;", "").ToString();
                if (resgndate != "")
                {
                    gv.BackColor = Color.Salmon;
                }
            }
        }

        private DataSet GetUnresignedData(DataSet ds,int unresn,int year,int month)
        {
            DataSet unresgneddata = new DataSet();

            unresgneddata.Tables.Add(new DataTable("Emp"));
            DataTable dt;
            DataRow dr;
            dt = unresgneddata.Tables["Emp"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Surname");
            dt.Columns.Add("BasicPay");
            dt.Columns.Add("Gross");
            dt.Columns.Add("Deduction");
            dt.Columns.Add("NetAmt");
            dt.Columns.Add("AdvSal");
            dt.Columns.Add("EmpId");
            dt.Columns.Add("Resigndate");
            dt.Columns.Add("Advanceamount");
            
            


            OdbcDataReader myresreader = null;
            foreach (DataRow _dr in ds.Tables[0].Rows)
            {
                DateTime resdate = new DateTime();
                DateTime tdate = new DateTime();
                string _sql = "select DATE_FORMAT(tbluserdetails_history.ResignDate,'%d/%m/%Y') from tbluserdetails_history where tbluserdetails_history.UserId in (select tblpay_employee.StaffId from tblpay_employee where tblpay_employee.EmpId='" + _dr["EmpId"].ToString() + "')";
                myresreader = Mypay.m_MysqlDb.ExecuteQuery(_sql);
                tdate = System.DateTime.Now.Date;
                if (myresreader.HasRows)
                {
                    string resndate = myresreader.GetValue(0).ToString();
                    int yearvalue = 0;
                    int monthid = 0;
                    if (resndate != "")
                    {
                        resdate = General.GetDateTimeFromText(resndate);
                        yearvalue = resdate.Year;
                        monthid = resdate.Month;
                    }
                    if (unresn == 1)
                    {
                        if (year < yearvalue)
                        {
                            dr = unresgneddata.Tables["Emp"].NewRow();
                            dr["Id"] = _dr["Id"];
                            dr["Surname"] = _dr["Surname"];
                            dr["BasicPay"] = _dr["BasicPay"];
                            dr["Gross"] = _dr["Gross"];
                            dr["Deduction"] = _dr["Deduction"];
                            dr["NetAmt"] = _dr["NetAmt"];
                            dr["AdvSal"] = _dr["AdvSal"];
                            dr["Resigndate"] = resndate;
                            dr["EmpId"] = _dr["EmpId"];
                            dr["Advanceamount"] = _dr["Advanceamount"];


                            unresgneddata.Tables["Emp"].Rows.Add(dr);
                        }
                        if (yearvalue == year)
                        {
                            if (month <= monthid)
                            {

                                dr = unresgneddata.Tables["Emp"].NewRow();
                                dr["Id"] = _dr["Id"];
                                dr["Surname"] = _dr["Surname"];
                                dr["BasicPay"] = _dr["BasicPay"];
                                dr["Gross"] = _dr["Gross"];
                                dr["Deduction"] = _dr["Deduction"];
                                dr["NetAmt"] = _dr["NetAmt"];
                                dr["AdvSal"] = _dr["AdvSal"];
                                dr["Resigndate"] = resndate;
                                dr["EmpId"] = _dr["EmpId"];
                                dr["Advanceamount"] = _dr["Advanceamount"];


                                unresgneddata.Tables["Emp"].Rows.Add(dr);
                            }
                        }


                       
                    }
                    else if (unresn == 0)
                    {
                        dr = unresgneddata.Tables["Emp"].NewRow();
                        dr["Id"] = _dr["Id"];
                        dr["Surname"] = _dr["Surname"];
                        dr["BasicPay"] = _dr["BasicPay"];
                        dr["Gross"] = _dr["Gross"];
                        dr["Deduction"] = _dr["Deduction"];
                        dr["NetAmt"] = _dr["NetAmt"];
                        dr["AdvSal"] = _dr["AdvSal"];
                        dr["Resigndate"] = resndate;
                        dr["EmpId"] = _dr["EmpId"];
                        dr["Advanceamount"] = _dr["Advanceamount"];
                        unresgneddata.Tables["Emp"].Rows.Add(dr);
                    }
                }
                else
                {
                    dr = unresgneddata.Tables["Emp"].NewRow();
                    dr["Id"] = _dr["Id"];
                    dr["Surname"] = _dr["Surname"];
                    dr["BasicPay"] = _dr["BasicPay"];
                    dr["Gross"] = _dr["Gross"];
                    dr["Deduction"] = _dr["Deduction"];
                    dr["NetAmt"] = _dr["NetAmt"];
                    dr["AdvSal"] = _dr["AdvSal"];
                    dr["Resigndate"] = "";
                    dr["EmpId"] = _dr["EmpId"];
                    dr["Advanceamount"] = _dr["Advanceamount"];
                    unresgneddata.Tables["Emp"].Rows.Add(dr);
                }
            }
            return unresgneddata;
        }


        private void LoadPayedGrid()
        {
            int unresn = 1;
            BtnSave.Visible = false;
            int year = int.Parse(DrpYear.SelectedItem.Text.ToString());
            int MonthId = int.Parse(Drp_Month.SelectedValue.ToString());
            DataSet Unresignedds = Mypay.GetPayedMonthEmp(MonthId, year);
            DataSet MyEmp = new DataSet();
            MyEmp = GetUnresignedData(Unresignedds, unresn,year,MonthId);
            if (MyEmp != null && MyEmp.Tables != null && MyEmp.Tables[0].Rows.Count > 0)
            {
                Grd_EmpPay.Columns[0].Visible = true;
                Grd_EmpPay.Columns[9].Visible = true;
                Grd_EmpPay.Columns[10].Visible = true;
                Grd_EmpPay.Columns[11].Visible = true;
                Grd_EmpPay.DataSource = MyEmp;
                Grd_EmpPay.DataBind();
                Grd_EmpPay.Columns[0].Visible = false;
                Grd_EmpPay.Columns[10].Visible = false;
                Grd_EmpPay.Columns[9].Visible = false;

                Lblmsg.Text = "";
                Pnl_EmpPay.Visible = true;

            }
            else
            {
                Grd_EmpPay.DataSource = null;
                Grd_EmpPay.DataBind();
                Lblmsg.Text = "No staff found for this month";
            }
            ColorGrid(unresn);
        }

        private void LoadMonthToDrpList()
        {
            MydataSet = null;
            Drp_Month.Items.Clear();
            MydataSet = Mypay.FillMonthDrp();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow Dr_Month in MydataSet.Tables[0].Rows)
                {
                    ListItem li = new ListItem(Dr_Month[1].ToString(), Dr_Month[0].ToString());
                    Drp_Month.Items.Add(li);
                }
                int CurrentMonth = DateTime.Now.Month;
                Drp_Month.SelectedValue = CurrentMonth.ToString();

            }
            else
            {
                ListItem li = new ListItem("No Month Found", "-1");
                Drp_Month.Items.Add(li);
            }
        }

        protected void RdbPayed_CheckedChanged(object sender, EventArgs e)
        {
            LoadPayedGrid();
        }

        protected void RdbNonPayed_CheckedChanged(object sender, EventArgs e)
        {
            LoadNonPayedGrid();

        }


        protected void Drp_Month_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RdbNonPayed.Checked)
            {
                LoadNonPayedGrid();
            }
            else
            {
                LoadPayedGrid();
            }

        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            int flag = 0;
            foreach (GridViewRow gv in Grd_EmpPay.Rows)
            {
                CheckBox Chk_Pay = (CheckBox)gv.FindControl("ChkPayed");
                if (Chk_Pay.Checked)
                {
                    flag = 1;
                    MPE_Payed.Show();
                }


            }
            if (flag == 0)
            {
                WC_MsgBox.ShowMssage("Select Any Employee");
            }
            

        }
        protected void btn_Save_confirm_click(object sender, EventArgs e)
        {
            int year = int.Parse(DrpYear.SelectedItem.Text.ToString());
            int MonthId = int.Parse(Drp_Month.SelectedValue.ToString());
            string month = Drp_Month.SelectedItem.ToString();
            foreach (GridViewRow gv in Grd_EmpPay.Rows)
            {
                CheckBox Chk_Pay = (CheckBox)gv.FindControl("ChkPayed");
                string EmpId = "";
                string Empname = "";
                if (Chk_Pay.Checked)
                {
                    
                    EmpId = gv.Cells[10].Text.ToString();
                    Empname = gv.Cells[1].Text.ToString();
                    Mypay.UpdateEmpMonthSalPayed(EmpId, MonthId,year);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Salary Payment Updation", "Paid the salary of employee " + Empname + " for the month "+month+"", 1);
                    MypayReader = Mypay.GetAdvanceSal(EmpId);
                    if (MypayReader.HasRows)
                    {
                        while (MypayReader.Read())
                        {
                            double Bal = 0;
                            double Adv = double.Parse(MypayReader.GetValue(0).ToString());
                            double Per = double.Parse(MypayReader.GetValue(1).ToString());
                             Bal = double.Parse(MypayReader.GetValue(2).ToString());


                             if ((Adv == 0) || (Per == 0))
                             {
                                 Bal = 0;
                             }
                             else
                             {
                                 if (Bal == Adv*2)
                                 {
                                     Bal = Adv;
                                     Mypay.UpdateAdvanceSalary(EmpId, Bal);

                                 }
                                 else if (Bal > 0)
                                 {
                                     Bal = Bal - Adv * (Per / 100);
                                     Mypay.UpdateAdvanceSalary(EmpId, Bal);
                                 }
                                 
                             }
                        }
                    }
                    
                    if (RdbNonPayed.Checked)
                    {
                        LoadNonPayedGrid();
                    }
                    else
                    {
                        LoadPayedGrid();
                    }
                }

            }

           

        }

       
        protected void BtnPrint_Click(object sender, EventArgs e)
        {
           
        }

        protected void Grd_EmpPay_Selectedindexchanged(object sender, EventArgs e)
        {
            int Year = int.Parse(DrpYear.SelectedItem.Text.ToString());
           int MonthId = int.Parse(Drp_Month.SelectedValue.ToString());
            string  employeeId =Grd_EmpPay.Rows[Grd_EmpPay.SelectedIndex].Cells[10].Text;
             
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "openIncpopup('PaySlip.aspx?EmpId=" + employeeId + "&MonthId=" + MonthId +  "&Year=" + Year + "');", true);
        }

        protected void DrpYear_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (RdbNonPayed.Checked)
            {
                LoadNonPayedGrid();
            }
            else
            {
                LoadPayedGrid();
            }
        }

        //protected void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        //{

        //}

       
      
        

       
    }
}
