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
    public partial class MonthlySalaryApproval : System.Web.UI.Page
    {
        private WinBase.Payroll Mypay;
        private KnowinUser MyUser;
        private DataSet MydataSet = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            //if (Session["StudId"] == null)
            //{
            //    Response.Redirect("PayrollHead.aspx");
            //}
            MyUser = (KnowinUser)Session["UserObj"];
            Mypay = MyUser.GetPayrollObj();
            if (Mypay == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(805))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (!IsPostBack)
                {
                    LoadYearToDrpList();
                    LoadMonthToDrpList();
                    RdbNonApprove.Checked = true;
                    LoadNonApproveGrid();

                    //some initlization

                }
            }
        }

        private void LoadNonApproveGrid()
        {
            int year = int.Parse(DrpYear.SelectedItem.Text.ToString());
            int MonthId = int.Parse(Drp_Month.SelectedValue.ToString());
            DataSet Unresignedds = Mypay.GetNonApprovedMonthEmp(MonthId, year);
            DataSet MyEmp = new DataSet();
            MyEmp = GetUnresignedData(Unresignedds,year,MonthId);
            if (MyEmp != null && MyEmp.Tables != null && MyEmp.Tables[0].Rows.Count > 0)
            {
                //foreach (DataRow dro in MyEmp.Tables[0].Rows)
                //{
                //    dro["AdvSalary"] = Mypay.GetAdvanceSalary(dro["EmpId"].ToString());
                //    dro["NetAmt"] = (double.Parse(dro["NetAmt"].ToString()) - double.Parse(dro["AdvSalary"].ToString())).ToString();
                //}
                Grd_EmpPay.Columns[0].Visible = true;
                Grd_EmpPay.Columns[10].Visible = true;
                Grd_EmpPay.DataSource = MyEmp;
                Grd_EmpPay.DataBind();
                Grd_EmpPay.Columns[0].Visible = false;
                Grd_EmpPay.Columns[10].Visible = false;
                Pnl_EmpPay.Visible = true;
                Lblmsg.Visible = false;

            }
            else
            {
                Grd_EmpPay.DataSource = null;
                Grd_EmpPay.DataBind();
                Lblmsg.Visible = true;
                Lblmsg.Text = "No employees in this list";
            }
            ColorGrid();
        }

        private void ColorGrid()
        {
            foreach (GridViewRow gv in Grd_EmpPay.Rows)
            {
                string resgndate = "";
                resgndate = gv.Cells[11].Text.Replace("&nbsp;", "").ToString();
                if (resgndate != "")
                {
                    gv.BackColor = Color.Salmon;
                }
            }
        }

        private DataSet GetUnresignedData(DataSet ds,int year,int month)
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
            dt.Columns.Add("Comment");
            dt.Columns.Add("EmpId");
            dt.Columns.Add("Resigndate");
            dt.Columns.Add("AdvSal");
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
                    int yearvalue= 0;
                    int monthId=0 ;
                    if (resndate != "")
                    {
                        resdate = General.GetDateTimeFromText(resndate);
                        yearvalue = resdate.Year;
                        monthId = resdate.Month;
                    }
                    //if (unresn == 1)
                    //{
                    if (year < yearvalue)
                    {

                        dr = unresgneddata.Tables["Emp"].NewRow();
                        dr["Id"] = _dr["Id"];
                        dr["Surname"] = _dr["Surname"];
                        dr["BasicPay"] = _dr["BasicPay"];
                        dr["Gross"] = _dr["Gross"];
                        dr["Deduction"] = _dr["Deduction"];
                        dr["NetAmt"] = _dr["NetAmt"];
                        dr["Comment"] = _dr["Comment"];
                        dr["Resigndate"] = resndate;
                        dr["EmpId"] = _dr["EmpId"];
                        dr["AdvSal"] = _dr["AdvSal"];
                        dr["Advanceamount"] = _dr["Advanceamount"];
                        unresgneddata.Tables["Emp"].Rows.Add(dr);
                    }
                    if (yearvalue == year)
                    {
                        if (month <= monthId)
                        {
                            dr = unresgneddata.Tables["Emp"].NewRow();
                            dr["Id"] = _dr["Id"];
                            dr["Surname"] = _dr["Surname"];
                            dr["BasicPay"] = _dr["BasicPay"];
                            dr["Gross"] = _dr["Gross"];
                            dr["Deduction"] = _dr["Deduction"];
                            dr["NetAmt"] = _dr["NetAmt"];
                            dr["Comment"] = _dr["Comment"];
                            dr["Resigndate"] = resndate;
                            dr["EmpId"] = _dr["EmpId"];
                            dr["AdvSal"] = _dr["AdvSal"];
                            dr["Advanceamount"] = _dr["Advanceamount"];
                            unresgneddata.Tables["Emp"].Rows.Add(dr);
                        }
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
                    dr["Comment"] = _dr["Comment"];
                    dr["Resigndate"] = "";
                    dr["EmpId"] = _dr["EmpId"];
                    dr["AdvSal"] = _dr["AdvSal"];
                    dr["Advanceamount"] = _dr["Advanceamount"];      
                    unresgneddata.Tables["Emp"].Rows.Add(dr);
                }
            }
            return unresgneddata;
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

            }
            else
            {
                DateTime _Current = DateTime.Now.Date;

                ListItem li = new ListItem(_Current.Year.ToString(), "1");
                DrpYear.Items.Add(li);
            }

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

        private void LoadApproveGrid()
        {
            int year = int.Parse(DrpYear.SelectedItem.Text.ToString());
            int MonthId = int.Parse(Drp_Month.SelectedValue.ToString());
            DataSet Unresignedds = Mypay.GetApprovedMonthEmp(MonthId, year);;
            DataSet MyEmp = new DataSet();
            MyEmp = GetUnresignedData(Unresignedds,year,MonthId);
            if (MyEmp != null && MyEmp.Tables != null && MyEmp.Tables[0].Rows.Count > 0)
            {
                //foreach (DataRow dro in MyEmp.Tables[0].Rows)
                //{
                //    dro["AdvSalary"] = Mypay.GetAdvanceSalary(dro["EmpId"].ToString());
                //    dro["NetAmt"] = (double.Parse(dro["NetAmt"].ToString()) - double.Parse(dro["AdvSalary"].ToString())).ToString();
                //}
                Grd_EmpPay.Columns[0].Visible = true;
                Grd_EmpPay.Columns[10].Visible = true;
                Grd_EmpPay.DataSource = MyEmp;
                Grd_EmpPay.DataBind();
                Grd_EmpPay.Columns[0].Visible = false;
                Grd_EmpPay.Columns[10].Visible = false;
                Pnl_EmpPay.Visible = true;
                Lblmsg.Visible = false;

            }
            else
            {
                Grd_EmpPay.DataSource = null;
                Grd_EmpPay.DataBind();
                Lblmsg.Visible = true;
                Lblmsg.Text = "No employees in this list";
            }
            ColorGrid();
        }

        private void LoadRejectedGrid()
        {
            int year = int.Parse(DrpYear.SelectedItem.Text.ToString());
            int MonthId = int.Parse(Drp_Month.SelectedValue.ToString());
            DataSet Unresignedds = Mypay.GetRejectedMonthEmp(MonthId, year);
            DataSet MyEmp = new DataSet();
            MyEmp = GetUnresignedData(Unresignedds,year,MonthId);
            if (MyEmp != null && MyEmp.Tables != null && MyEmp.Tables[0].Rows.Count > 0)
            {

                //foreach (DataRow dro in MyEmp.Tables[0].Rows)
                //{
                //    dro["AdvSalary"] = Mypay.GetAdvanceSalary(dro["EmpId"].ToString());
                //    dro["NetAmt"] = (double.Parse(dro["NetAmt"].ToString()) - double.Parse(dro["AdvSalary"].ToString())).ToString();
                //}
                Grd_EmpPay.Columns[0].Visible = true;
                Grd_EmpPay.Columns[10].Visible = true;
                Grd_EmpPay.DataSource = MyEmp;
                Grd_EmpPay.DataBind();
                Grd_EmpPay.Columns[0].Visible = false;
                Grd_EmpPay.Columns[10].Visible = false;
                Pnl_EmpPay.Visible = true;
                Lblmsg.Visible = false;


            }
            else
            {
                Grd_EmpPay.DataSource = null;
                Grd_EmpPay.DataBind();
                Lblmsg.Visible = true;
                Lblmsg.Text = "No employees in this list";
            }
            ColorGrid();
        }

        protected void RdbApprove_CheckedChanged(object sender, EventArgs e)
        {
            BtnApprove.Visible = false;
            BtnReject.Visible = true;
            LoadApproveGrid();
        }

        protected void RdbNonApprove_CheckedChanged(object sender, EventArgs e)
        {
            BtnApprove.Visible = true;
            BtnReject.Visible = true;
            LoadNonApproveGrid();
        }
        protected void RdbRejected_CheckedChanged(object sender, EventArgs e)
        {
            BtnApprove.Visible = true;
            BtnReject.Visible = false; ;
            LoadRejectedGrid();
        }

       

        protected void BtnApprove_Click(object sender, EventArgs e)
        {
            int f = 0;
            int year = int.Parse(DrpYear.SelectedItem.Text.ToString());
            int MonthId = int.Parse(Drp_Month.SelectedValue.ToString());
            foreach (GridViewRow gv in Grd_EmpPay.Rows)
            {
                CheckBox Chk_Pay = (CheckBox)gv.FindControl("ChkPayroll");
                string  EmpId= "";
                string empname = "";
                if (Chk_Pay.Checked)
                {
                    f = 1;
                    bool Approve = true;
                    EmpId = gv.Cells[10].Text.ToString();
                    empname = gv.Cells[2].Text.ToString();
                    Mypay.UpdateEmpMonthSal(EmpId, MonthId, Approve, year, MyUser.UserName, empname);
                    if (RdbNonApprove.Checked)
                    {
                        LoadNonApproveGrid();
                    }
                    else if (RdbApprove.Checked)
                    {
                        LoadApproveGrid();
                    }
                    else
                    {
                        LoadRejectedGrid();
                    }
                }
                //else
                //{
                //    Lblmsg.Text = "Please select any employee to approve salary";
                //    //WC_MSGBox.ShowMssage("Please select any employee to approve salary");
                //}

            }

            if (f == 1)
            {
                WC_MessageBox.ShowMssage("Selected employee salary has been approved");

            }
            else
            {
                WC_MessageBox.ShowMssage("Please select any employee to approve salary");
            }
            
        }

        protected void BtnReject_Click(object sender, EventArgs e)
        {
            int f = 0;
            int year = int.Parse(DrpYear.SelectedItem.Text.ToString());
            int MonthId = int.Parse(Drp_Month.SelectedValue.ToString());
            foreach (GridViewRow gv in Grd_EmpPay.Rows)
            {
                CheckBox Chk_Pay = (CheckBox)gv.FindControl("ChkPayroll");
                string EmpId = "";
                string empname = "";
                if (Chk_Pay.Checked)
                {
                    f = 1;
                    bool Approve = false;
                    EmpId = gv.Cells[10].Text.ToString();
                    empname = gv.Cells[2].Text.ToString();
                    Mypay.UpdateEmpMonthSal(EmpId, MonthId, Approve,year,MyUser.UserName,empname);
                    if (RdbNonApprove.Checked)
                    {
                        LoadNonApproveGrid();
                    }
                    else if (RdbApprove.Checked)
                    {
                        LoadApproveGrid();
                    }
                    else
                    {
                        LoadRejectedGrid();
                    }
                }
                //else
                //{
                //    //WC_MSGBox.ShowMssage("Please select any employee to reject salary");
                //    Lblmsg.Text = "Please select any employee to reject salary";
                //}
            }
            if (f == 1)
            {
                WC_MessageBox.ShowMssage("Selected employee salary has been rejected");

            }
            else
            {
                WC_MessageBox.ShowMssage("Please select any employee to reject salary");
            }
            

        }

        protected void Drp_Month_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RdbNonApprove.Checked)
            {
                LoadNonApproveGrid();
            }
            else if (RdbApprove.Checked)
            {
                LoadApproveGrid();
            }
            else
            {
                LoadRejectedGrid();
            }
        }

        protected void Grd_EmpPay_Selectedindexchanged(object sender, EventArgs e)
        {
            int year = int.Parse(DrpYear.SelectedItem.Text.ToString());
            int MonthId = int.Parse(Drp_Month.SelectedValue.ToString());
            OdbcDataReader MyPayReader = null;
            string  Emp_Id = Grd_EmpPay.Rows[Grd_EmpPay.SelectedIndex].Cells[10].Text;
            MyPayReader = Mypay.GetMonthlyComments(Emp_Id, MonthId,year);
            if (MyPayReader.HasRows)
            {
                while (MyPayReader.Read())
                {
                    Txt_Comment.Text = MyPayReader.GetValue(0).ToString();
                }
            }
            MPE_EditComment_popup.Show();

        }
        protected void Btn_CommentSave_Click(object sender, EventArgs e)
        {
            int year = int.Parse(DrpYear.SelectedItem.Text.ToString());
            string  Emp_Id =Grd_EmpPay.Rows[Grd_EmpPay.SelectedIndex].Cells[10].Text;
            int MonthId = int.Parse(Drp_Month.SelectedValue.ToString());
            string UpdateComment = Txt_Comment.Text.Trim();
            Mypay.UpdateMonthlyComment(Emp_Id, UpdateComment, MonthId,year);
            if (RdbNonApprove.Checked)
            {
                LoadNonApproveGrid();
            }
            else if (RdbApprove.Checked)
            {
                LoadApproveGrid();
            }
            else
            {
                LoadRejectedGrid();
            }


        }

        protected void DrpYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RdbNonApprove.Checked)
            {
                LoadNonApproveGrid();
            }
            else if (RdbApprove.Checked)
            {
                LoadApproveGrid();
            }
            else
            {
                LoadRejectedGrid();
            }
        }

    }
}
