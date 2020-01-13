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
    public partial class MonthlySalaryConfig : System.Web.UI.Page
    {
        private WinBase.Payroll Mypay;
        private KnowinUser MyUser;
        private OdbcDataReader m_MyReader = null;
        private OdbcDataReader Myreader = null;
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
            else if (!MyUser.HaveActionRignt(804))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (!IsPostBack)
                {
                    LoadYearToDrpList();
                    LoadMonthToDrpList();
                    LoadCatToDrpList();
                   
                    DrpEmp.Enabled = false;
                    Img_Export.Visible = false;
                    BtnSave.Visible = false;
                    td_color.Visible = false;
                    //some initlization
                    if (Session["MSCmonth"] != null)
                    {

                        DrpEmp.Enabled = true;
                        LoadDrpEmp(int.Parse(Session["MSCCat"].ToString()));
                        DrpYear.SelectedValue = Session["MSCyear"].ToString();
                        LoadMonthToDrpList();
                        DrpMonth.SelectedValue = Session["MSCmonth"].ToString();
                        DrpPayroll.SelectedValue = Session["MSCCat"].ToString();
                        //DrpEmp.SelectedValue = Session["MSCempId"].ToString();
                        Show();
                    }
                }
            }

        }

        private void LoadMonthToDrpList()
        {
            int Month = 0;
            string[] months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            DateTime _Current=DateTime.Now.Date;
            int Yr = int.Parse(DrpYear.SelectedItem.Text.ToString());
            if (Yr == null)
            {
                Yr = System.DateTime.Now.Year;
            }
            if (Yr == _Current.Year)
            {
                Month = _Current.Month;
            }
            else
            {
                Month = 12;
            }
            DrpMonth.Items.Clear();
            ListItem li;
            for (int i = 0; i < Month;i++ )
            {
               
               li = new ListItem(months[i],(i+1).ToString());
               DrpMonth.Items.Add(li);
            }
            DrpMonth.SelectedValue = (Month).ToString();
        }


        //private void LoadYearToDrpList()
        //{
        //    MydataSet = null;
        //    DrpYear.Items.Clear();
        //    int i = 0;
        //    MydataSet = Mypay.FillYearDrp();
        //    if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow Dr_Year in MydataSet.Tables[0].Rows)
        //        {
        //            ListItem li = new ListItem(Dr_Year[0].ToString(), i.ToString());
        //            DrpYear.Items.Add(li);
        //            i++;
        //        }
        //        int CurrentYear = DateTime.Now.Year;
        //        DrpYear.SelectedValue = CurrentYear.ToString();

        //    }
        //    else
        //    {
        //        DateTime _Current = DateTime.Now.Date;

        //        ListItem li = new ListItem(_Current.Year.ToString(), "1");
        //        DrpYear.Items.Add(li);
        //    }

        //}

        private void LoadYearToDrpList()
        {
            int CurrentYear = DateTime.Now.Year;
            ListItem li = new ListItem();
            li = new ListItem((CurrentYear - 2).ToString(), "0");
            DrpYear.Items.Add(li);
            li = new ListItem((CurrentYear-1).ToString(), "1");
            DrpYear.Items.Add(li);
            li = new ListItem(CurrentYear.ToString(), "2");
            DrpYear.Items.Add(li);

            DrpYear.SelectedValue = "2";
        }


        private void LoadCatToDrpList()
        {
            MydataSet = null;
            DrpPayroll.Items.Clear();
            MydataSet = Mypay.FillDrp();

            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                ListItem li;
                li = new ListItem("Select Category", "0");
                DrpPayroll.Items.Add(li);
                foreach (DataRow Dr_Cat in MydataSet.Tables[0].Rows)
                {
                    li = new ListItem(Dr_Cat[1].ToString(), Dr_Cat[0].ToString());
                    DrpPayroll.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Category Found", "-1");
                DrpPayroll.Items.Add(li);
            }
        }

        private void LoadDrpEmp(int _CatId)
        {
            MydataSet = null;
            DrpEmp.Items.Clear();
            int year = int.Parse(DrpYear.SelectedItem.Text);
            int month = int.Parse(DrpMonth.SelectedValue.ToString());
            MydataSet = Mypay.FillEmpDrp(_CatId, year, month);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                ListItem li;
                li = new ListItem("All", "0");
                DrpEmp.Items.Add(li);
                foreach (DataRow Dr_Emp in MydataSet.Tables[0].Rows)
                {
                    li = new ListItem(Dr_Emp[1].ToString(), Dr_Emp[0].ToString());
                    DrpEmp.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Employee Found", "-1");
                DrpEmp.Items.Add(li);
            }
            
        }

        protected void DrpPayroll_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (DrpPayroll.SelectedIndex > 0)
            {

                int _CatId = int.Parse(DrpPayroll.SelectedValue.ToString());
                DrpEmp.Enabled = true;
                LoadDrpEmp(_CatId);
            }
            else
            {
                DrpEmp.Items.Clear();
                DrpEmp.Enabled = false;
                Grd_EmpPay.DataSource = null;
                Grd_EmpPay.DataBind();
                Img_Export.Visible = false;
                BtnSave.Visible = false;
                td_color.Visible = false;
            }

        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (DrpPayroll.SelectedIndex > 0)
            {
                int flg = 0;
                string name = "",sep = " ";
                bool AddMonthEmpSal = false;
                bool DelMonthEmpSal = false;
                int _Month = int.Parse(DrpMonth.SelectedValue.ToString());
                int CatId = int.Parse(DrpPayroll.SelectedValue.ToString());
                int _Yr = int.Parse(DrpYear.SelectedItem.Text.ToString());
                Grd_EmpPay.Columns[0].Visible = true;
                Grd_EmpPay.Columns[13].Visible = true;
                foreach (GridViewRow gv in Grd_EmpPay.Rows)
                {
                    CheckBox Chk_Pay = (CheckBox)gv.FindControl("ChkPayroll");

                    if (Chk_Pay.Checked)
                    {
                        flg = 1;
                        double BP = double.Parse(gv.Cells[3].Text);
                        double Gross = double.Parse(gv.Cells[4].Text);
                        double Deduction = double.Parse(gv.Cells[5].Text);
                        double Netpay = double.Parse(gv.Cells[8].Text);
                        string  EmpId = gv.Cells[13].Text;
                        string empname = gv.Cells[2].Text;
                        int _TotWorking = 25;
                        int _TotWorked = 25;

                        if (Mypay.NotEmpPresent(EmpId, _Month, _Yr))
                        {
                            double advsal = Mypay.GetAdvanceSalary(EmpId.ToString());
                            //Netpay = Netpay - advsal;

                            Mypay.AddMonthEmpSal(_Yr, _Month, BP, EmpId, Gross, Deduction, Netpay, CatId, advsal, _TotWorking, _TotWorked);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Monthly Salary Configuration", "Monthly salary details of employee " + empname + " is cofigured", 1);
                            m_MyReader = Mypay.GetHfrmHCatMap(CatId, EmpId);
                            if (m_MyReader.HasRows)
                            {
                                while (m_MyReader.Read())
                                {
                                    int _HeadId = int.Parse(m_MyReader.GetValue(0).ToString());
                                    double _HeadAmount = double.Parse(m_MyReader.GetValue(1).ToString());
                                    Mypay.InsertMonthEmpHeadMap(_Yr, _Month, EmpId, CatId, _HeadId, _HeadAmount);
                                }
                            }
                            AddMonthEmpSal = true;

                        }
                        else
                        {
                            msg = "Already configured";
                            WC_MessageBox.ShowMssage(msg);
                        }
                        


                    }
                    else
                    {
                        
                        //int EmpId = int.Parse(gv.Cells[10].Text);
                        //if (Mypay.IsEmpPresent(EmpId, _Month))
                        //{
                        //    if (Mypay.NotEmpPayed(EmpId, _Month,_Yr))
                        //    {
                        //        name = name + sep + gv.Cells[2].Text;
                        //        sep = ",";
                        //        Mypay.DeleteMonthEmp(EmpId, _Month);
                        //        Mypay.DeleteEmpMonthMap(EmpId, _Month,_Yr);
                        //        DelMonthEmpSal = true;
                        //    }
                            
                        //}

                    }
                   
                }

                if (AddMonthEmpSal)
                {
                    msg = "Successfully Saved";
                    WC_MessageBox.ShowMssage(msg);
                }

                else if (flg == 0)
                {
                    msg = "No Employees Selected";
                    WC_MessageBox.ShowMssage(msg);
                }


                //if (DelMonthEmpSal)
                //{
                //    WC_MessageBox.ShowMssage("Details for" + name + " are unchecked, So Configuration for these employees will be removed and will be considered as NOT CONFIGURED. You can regenrate all these details later");
                //}
                Grd_EmpPay.Columns[0].Visible = false;
                Grd_EmpPay.Columns[13].Visible = false;
                Show();

                
            }
            else
            {
               msg="Please select a payroll type";
               WC_MessageBox.ShowMssage(msg);

            }
           
        }

        

        protected void Img_Export_Click(object sender, ImageClickEventArgs e)
        {
            DataSet MyData = (DataSet)ViewState["EmpGrid"];
            //MyData.Tables[0].Columns[0].ToString().Remove(0);
            //MyData.Tables[0].Columns[0].ToString().Remove(9);
            MyData.Tables[0].Columns.Remove("Id");
            MyData.Tables[0].Columns.Remove("EmpId");
            MyData.Tables[0].Columns[1].ColumnName= "StaffName";
            string Name = "MonthlySalaryConfig.xls";
            //if (!ExcelUtility.ExportDataSetToExcel(MyData, Name))
            //{

            //}
            string FileName = "MonthlySalaryConfig";
            string _ReportName = "MonthlySalaryConfig";
            if (!WinEr.ExcelUtility.ExportDataToExcel(MyData, _ReportName, FileName, MyUser.ExcelHeader))
            {
                
            }

        }

        protected void BtnShow_Click(object sender, EventArgs e)
        {
            DataSet EmpDs = null;
            int AdvDeduction = 0;
            int month = int.Parse(DrpMonth.SelectedValue.ToString());
            int year = int.Parse(DrpYear.SelectedItem.Text.ToString());
            if (DrpPayroll.SelectedIndex > 0)
            {
                string resigndate = "";
                int _Cat = int.Parse(DrpPayroll.SelectedValue.ToString());
                int id = 0;
                string surname = "";
                double gross = 0;
                double Deduction = 0;
                double NetAmt = 0;
                double BasicPay = 0;
                double AdvSal = 0;
                double AdvAmount = 0.0;
                int status = 0;
                int _Approve = 0;
                int _Reject = 0;
                string Cofig = "";
                string AStatus = "";
                int CatId = 0, payrolltype = 0;
                DataSet Mydataset = new DataSet();
                DataTable dt;
                DataRow dr;
                Mydataset.Tables.Add(new DataTable("Emp"));
                dt = Mydataset.Tables["Emp"];
                dt.Columns.Add("Id");
                dt.Columns.Add("Surname");
                dt.Columns.Add("BasicPay");
                dt.Columns.Add("Gross");
                dt.Columns.Add("Deduction");
                dt.Columns.Add("AdvSalary");
                dt.Columns.Add("NetAmt");
                dt.Columns.Add("Configured");
                dt.Columns.Add("Approval");
                dt.Columns.Add("EmpId");
                dt.Columns.Add("ResignDate");
                dt.Columns.Add("JoiningDate");
                dt.Columns.Add("Advanceamount");
                


                if (DrpEmp.SelectedIndex == 0)
                {
                   

                    EmpDs = Mypay.GetDefaultEmp(_Cat, year, month);
                    if (EmpDs != null && EmpDs.Tables[0].Rows.Count>0)
                    {
                        foreach(DataRow _dr in EmpDs.Tables[0].Rows)
                        {
                            //select tbluserdetails_history.ResignDate from tbluserdetails_history where tbluserdetails_history.UserId in(select tblpay_employee.StaffId from tblpay_employee
                            //where tblpay_employee.EmpId='E1988525')
                            string joindate = _dr["JoiningDate"].ToString();
                            string EmpId = _dr["EmpId"].ToString();
                            int _Status = int.Parse(_dr["Configured"].ToString());
                            string _sql = "select DATE_FORMAT(tbluserdetails_history.ResignDate,'%d/%m/%Y') from tbluserdetails_history where tbluserdetails_history.UserId in(select tblpay_employee.StaffId from tblpay_employee where tblpay_employee.EmpId='" + EmpId + "' and tblpay_employee.`Status`=1)";
                            Myreader = Mypay.m_MysqlDb.ExecuteQuery(_sql);
                            if (Myreader.HasRows)
                            {
                                resigndate = Myreader.GetValue(0).ToString();

                                // dr["ResignDate"] = Myreader.GetValue(0).ToString();
                            }
                            else
                            {
                                resigndate = "";
                            }
                            if (Mypay.IsPresentInMonthly(year, month, EmpId, out id, out surname, out BasicPay, out gross, out Deduction, out NetAmt, out status, out _Approve, out _Reject, out AdvSal,out CatId))
                            {
                                


                                    if (_Approve == 0 && _Reject == 0)
                                    {
                                        AStatus = "Approval Pending";
                                    }
                                    else if (_Approve == 1 && _Reject == 0)
                                    {
                                        AStatus = "Approved";
                                    }
                                    else
                                    {
                                        AStatus = "Rejected";
                                    }
                                    if (status == 1)
                                    {
                                        Cofig = "Configured";
                                    }

                                    dr = Mydataset.Tables["Emp"].NewRow();
                                    dr["Id"] = id;
                                    dr["Surname"] = surname;
                                    dr["BasicPay"] = BasicPay;
                                    dr["Gross"] = gross;
                                    dr["Deduction"] = Deduction;
                                    dr["NetAmt"] = NetAmt;
                                    dr["Configured"] = Cofig;
                                    dr["Approval"] = AStatus;
                                    dr["EmpId"] = EmpId;
                                    dr["ResignDate"] = resigndate;
                                    dr["JoiningDate"] = joindate;
                                    dr["AdvSalary"] = AdvSal;
                                    dr["Advanceamount"] = Mypay.GetAdvanceSalaryOnMonth(dr["EmpId"].ToString(), month, year);
                                    dr["AdvSalary"] = Mypay.GetAdvanceSalaryDeduction(dr["EmpId"].ToString(), month, year);
                                    // AdvAmount=

                                    // dr["AdvSalary"] = Mypay.GetAdvanceSalary(dr["EmpId"].ToString());
                                    if (status != 1)
                                    {
                                        dr["NetAmt"] = (double.Parse(dr["NetAmt"].ToString()) - double.Parse(dr["AdvSalary"].ToString())).ToString();
                                    }
                                    Mydataset.Tables["Emp"].Rows.Add(dr);
                                
                            }
                            else
                            {
                                AStatus = "Approval Pending";
                                if (_Status == 0)
                                {
                                    Cofig = "Not Configured";
                                }
                                
                                dr = Mydataset.Tables["Emp"].NewRow();
                                dr["Id"] = int.Parse(_dr["Id"].ToString());
                                dr["Surname"] = _dr["Surname"].ToString();
                                dr["BasicPay"] = double.Parse(_dr["BasicPay"].ToString());
                                dr["Gross"] = double.Parse(_dr["Gross"].ToString());
                                dr["Deduction"] = double.Parse(_dr["Deduction"].ToString());
                                dr["NetAmt"] = double.Parse(_dr["NetAmt"].ToString());
                                dr["Configured"] = Cofig;
                                dr["Approval"] = AStatus;
                                dr["EmpId"] =_dr["EmpId"].ToString();
                                dr["AdvSalary"] = AdvDeduction.ToString();
                                dr["ResignDate"] = resigndate;
                                dr["JoiningDate"] = joindate; 
                                dr["AdvSalary"] = Mypay.GetAdvanceSalary(dr["EmpId"].ToString());
                                dr["Advanceamount"] = Mypay.GetAdvanceSalaryOnMonth(dr["EmpId"].ToString(), month, year); 
                                if (status != 1)
                                {
                                    dr["NetAmt"] = (double.Parse(dr["NetAmt"].ToString()) - double.Parse(dr["AdvSalary"].ToString())).ToString();

                                }
                                Mydataset.Tables["Emp"].Rows.Add(dr);

                            }
                        }
                        //foreach (DataRow dro in Mydataset.Tables[0].Rows)
                        //{
                        //    dro["AdvSalary"] = Mypay.GetAdvanceSalary(dro["EmpId"].ToString());
                        //    dro["NetAmt"] = (double.Parse(dro["NetAmt"].ToString()) - double.Parse(dro["AdvSalary"].ToString())).ToString();
                        //}
                        Grd_EmpPay.Columns[0].Visible = true;
                        Grd_EmpPay.Columns[13].Visible = true;
                        DataSet unresignedds = new DataSet();
                        DataSet corrcetjoinees = new DataSet();
                        corrcetjoinees = GetCorrectjoinees(Mydataset, month, year);
                        unresignedds = GetUnresignedData(corrcetjoinees, month,year);
                        if (unresignedds != null && unresignedds.Tables[0].Rows.Count > 0)
                        {
                            Grd_EmpPay.DataSource = unresignedds;
                            Grd_EmpPay.DataBind();
                            Grd_EmpPay.Columns[0].Visible = false;
                            Grd_EmpPay.Columns[13].Visible = false;
                            Pnl_EmpPay.Visible = true;
                            Grd_EmpPay.Visible = true;
                            Img_Export.Visible = true;
                            BtnSave.Visible = true;
                            td_color.Visible = true;
                        }
                        else
                        {
                            Grd_EmpPay.DataSource = null;
                            Grd_EmpPay.DataBind();
                            Img_Export.Visible = false;
                            BtnSave.Visible = false;
                            td_color.Visible = false;
                        }



                    }

                    else
                    {
                        Grd_EmpPay.DataSource = null;
                        Grd_EmpPay.DataBind();
                        Img_Export.Visible = false;
                        BtnSave.Visible = false;
                        td_color.Visible = false;
                    }
                    ViewState["EmpGrid"] = Mydataset;

                }
                else
                {
                    string rsgndate = "";
                    string _Emp = DrpEmp.SelectedValue.ToString();
                    m_MyReader = Mypay.FillEmp(_Emp, _Cat);
                    if (m_MyReader.HasRows)
                    {
                        while (m_MyReader.Read())
                        {
                            string joindate = m_MyReader.GetValue(8).ToString();
                            string EmpId = m_MyReader.GetValue(6).ToString();
                            int _Status = int.Parse(m_MyReader.GetValue(7).ToString());
                            string _sql = "select DATE_FORMAT(tbluserdetails_history.ResignDate,'%d/%m/%Y') from tbluserdetails_history where tbluserdetails_history.UserId in(select tblpay_employee.StaffId from tblpay_employee where tblpay_employee.EmpId='" + EmpId + "')";
                            Myreader = Mypay.m_MysqlDb.ExecuteQuery(_sql);
                            if (Myreader.HasRows)
                            {
                                rsgndate = Myreader.GetValue(0).ToString();
                                // dr["ResignDate"] = Myreader.GetValue(0).ToString();
                            }
                            else
                            {
                                rsgndate = "";
                            }
                            if (Mypay.IsPresentInMonthly(year, month, EmpId, out id, out surname, out BasicPay, out gross, out Deduction, out NetAmt, out status, out _Approve, out _Reject, out AdvSal,out CatId))
                            {
                                if (_Approve == 0 && _Reject == 0)
                                {
                                    AStatus = "Approval Pending";
                                }
                                else if (_Approve == 1 && _Reject == 0)
                                {
                                    AStatus = "Approved";
                                }
                                else
                                {
                                    AStatus = "Rejected";
                                }
                                if (status == 1)
                                {
                                    Cofig = "Configured";
                                }
                                dr = Mydataset.Tables["Emp"].NewRow();
                               
                                dr["Id"] = id;
                                dr["Surname"] = surname;
                                dr["BasicPay"] = BasicPay;
                                dr["Gross"] = gross;
                                dr["Deduction"] = Deduction;
                                dr["NetAmt"] = NetAmt;
                                dr["Configured"] = Cofig;
                                dr["Approval"] = AStatus;
                                dr["EmpId"] = EmpId;
                                dr["AdvSalary"] = AdvSal;
                                dr["ResignDate"] = rsgndate;
                                dr["JoiningDate"] = joindate;
                                dr["Advanceamount"] = Mypay.GetAdvanceSalaryOnMonth(dr["EmpId"].ToString(), month, year); 
                                dr["AdvSalary"] = Mypay.GetAdvanceSalaryDeduction(dr["EmpId"].ToString(), month, year);
                                if (status != 1)
                                {
                                    dr["NetAmt"] = (double.Parse(dr["NetAmt"].ToString()) - double.Parse(dr["AdvSalary"].ToString())).ToString();
                                }
                                Mydataset.Tables["Emp"].Rows.Add(dr);
                            }
                            else
                            {
                                int Empstatus = int.Parse(m_MyReader.GetValue(9).ToString());
                                if (Empstatus == 1)
                                {
                                    AStatus = "Approval Pending";
                                    if (_Status == 0)
                                    {
                                        Cofig = "Not Configured";
                                    }
                                    dr = Mydataset.Tables["Emp"].NewRow();
                                    dr["Id"] = int.Parse(m_MyReader.GetValue(0).ToString());
                                    dr["Surname"] = m_MyReader.GetValue(1).ToString();
                                    dr["BasicPay"] = double.Parse(m_MyReader.GetValue(2).ToString());
                                    dr["Gross"] = double.Parse(m_MyReader.GetValue(3).ToString());
                                    dr["Deduction"] = double.Parse(m_MyReader.GetValue(4).ToString());
                                    dr["NetAmt"] = double.Parse(m_MyReader.GetValue(5).ToString());
                                    dr["Configured"] = Cofig;
                                    dr["Approval"] = AStatus;
                                    dr["EmpId"] = m_MyReader.GetValue(6).ToString();
                                    dr["ResignDate"] = rsgndate;
                                    dr["JoiningDate"] = joindate;
                                    dr["AdvSalary"] = AdvDeduction.ToString();
                                    dr["Advanceamount"] = Mypay.GetAdvanceSalaryOnMonth(dr["EmpId"].ToString(), month, year);
                                    dr["AdvSalary"] = Mypay.GetAdvanceSalary(dr["EmpId"].ToString()); if (status != 1)
                                    {
                                        dr["NetAmt"] = (double.Parse(dr["NetAmt"].ToString()) - double.Parse(dr["AdvSalary"].ToString())).ToString();
                                    }
                                    Mydataset.Tables["Emp"].Rows.Add(dr);
                                }

                            }
                        }
                        //foreach (DataRow dro in Mydataset.Tables[0].Rows)
                        //{
                        //    dro["AdvSalary"] = Mypay.GetAdvanceSalary(dro["EmpId"].ToString());
                        //    dro["NetAmt"] = (double.Parse(dro["NetAmt"].ToString()) - double.Parse(dro["AdvSalary"].ToString())).ToString();
                        //}
                        Grd_EmpPay.Columns[0].Visible = true;
                        Grd_EmpPay.Columns[13].Visible = true;
                        DataSet unresignedds = new DataSet();
                        DataSet corrcetjoinees = new DataSet();
                        corrcetjoinees = GetCorrectjoinees(Mydataset, month, year);
                        unresignedds = GetUnresignedData(corrcetjoinees, month, year);
                        if (unresignedds != null && unresignedds.Tables[0].Rows.Count > 0)
                        {
                            Grd_EmpPay.DataSource = unresignedds;
                            Grd_EmpPay.DataBind();
                            Grd_EmpPay.Columns[0].Visible = false;
                            Grd_EmpPay.Columns[13].Visible = false;
                            Pnl_EmpPay.Visible = true;
                            Grd_EmpPay.Visible = true;
                            Img_Export.Visible = true;
                            BtnSave.Visible = true;
                            td_color.Visible = true;
                        }
                        else
                        {
                            Grd_EmpPay.DataSource = null;
                            Grd_EmpPay.DataBind();
                            Img_Export.Visible = false;
                            BtnSave.Visible = false;
                            td_color.Visible = false;
                        }
                    }

                    else
                    {
                        Grd_EmpPay.DataSource = null;
                        Grd_EmpPay.DataBind();
                        Img_Export.Visible = false;
                        BtnSave.Visible = false;
                        td_color.Visible = false;
                    }
                    ViewState["EmpGrid"] = Mydataset;



                }
                ColorGrid();
            }
            else
            {
                WC_MessageBox.ShowMssage("Please Select Payroll");
            }
           //Show();


        }

        private bool ParollIdSame(int CatId,string EmpId)
        {
            bool same = false;
            int payrolltype = int.Parse(DrpPayroll.SelectedValue);           
            if (CatId == payrolltype)
            {
                same = true;
            }
            return same;
        }

        private DataSet GetCorrectjoinees(DataSet Mydataset, int month, int year)
        {
            DataSet correctjoinees = new DataSet();

            correctjoinees.Tables.Add(new DataTable("Emp"));
            DataTable dt;
            DataRow dr;
            dt = correctjoinees.Tables["Emp"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Surname");
            dt.Columns.Add("BasicPay");
            dt.Columns.Add("Gross");
            dt.Columns.Add("Deduction");
            dt.Columns.Add("AdvSalary");
            dt.Columns.Add("NetAmt");
            dt.Columns.Add("Configured");
            dt.Columns.Add("Approval");
            dt.Columns.Add("EmpId");
            dt.Columns.Add("Advanceamount");
            dt.Columns.Add("ResignDate");

            foreach (DataRow _dr in Mydataset.Tables[0].Rows)
            {
                DateTime joindate = General.GetDateTimeFromText(_dr["JoiningDate"].ToString());
                int monthid = joindate.Month;
                int yearvalue = joindate.Year;

                if (yearvalue < year)
                {

                    dr = correctjoinees.Tables["Emp"].NewRow();
                    dr["Id"] = _dr["Id"];
                    dr["Surname"] = _dr["Surname"];
                    dr["BasicPay"] = _dr["BasicPay"];
                    dr["Gross"] = _dr["Gross"];
                    dr["Deduction"] = _dr["Deduction"];
                    dr["NetAmt"] = _dr["NetAmt"];
                    dr["Approval"] = _dr["Approval"];
                    dr["AdvSalary"] = _dr["AdvSalary"];
                    dr["Resigndate"] = _dr["Resigndate"];
                    dr["Configured"] = _dr["Configured"];
                    dr["EmpId"] = _dr["EmpId"];
                    dr["Advanceamount"] = _dr["Advanceamount"];


                    correctjoinees.Tables["Emp"].Rows.Add(dr);
                }
                if (year == yearvalue)
                {
                    if (month >= monthid)
                    {
                        dr = correctjoinees.Tables["Emp"].NewRow();
                        dr["Id"] = _dr["Id"];
                        dr["Surname"] = _dr["Surname"];
                        dr["BasicPay"] = _dr["BasicPay"];
                        dr["Gross"] = _dr["Gross"];
                        dr["Deduction"] = _dr["Deduction"];
                        dr["NetAmt"] = _dr["NetAmt"];
                        dr["Approval"] = _dr["Approval"];
                        dr["AdvSalary"] = _dr["AdvSalary"];
                        dr["Resigndate"] = _dr["Resigndate"];
                        dr["Configured"] = _dr["Configured"];
                        dr["EmpId"] = _dr["EmpId"];
                        dr["Advanceamount"] = _dr["Advanceamount"];


                        correctjoinees.Tables["Emp"].Rows.Add(dr);
                    }
                }

            }

            return correctjoinees;
        }

        private DataSet GetUnresignedData(DataSet Mydataset,int month,int year)
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
            dt.Columns.Add("AdvSalary");
            dt.Columns.Add("NetAmt");
            dt.Columns.Add("Configured");
            dt.Columns.Add("Approval");
            dt.Columns.Add("EmpId");
            dt.Columns.Add("ResignDate");
            dt.Columns.Add("Advanceamount");

            foreach (DataRow _dr in Mydataset.Tables[0].Rows)
            {
                DateTime resdate = new DateTime();
                DateTime tdate = new DateTime();
                //string _sql = "select DATE_FORMAT(tbluserdetails_history.ResignDate,'%d/%m/%Y') from tbluserdetails_history where tbluserdetails_history.UserId in (select tblpay_employee.StaffId from tblpay_employee where tblpay_employee.EmpId='" + _dr["EmpId"].ToString() + "')";
                // myresreader = Mypay.m_MysqlDb.ExecuteQuery(_sql);
                tdate = System.DateTime.Now.Date;

                if (_dr["Resigndate"].ToString() == "")
                {
                    dr = unresgneddata.Tables["Emp"].NewRow();
                    dr["Id"] = _dr["Id"];
                    dr["Surname"] = _dr["Surname"];
                    dr["BasicPay"] = _dr["BasicPay"];
                    dr["Gross"] = _dr["Gross"];
                    dr["Deduction"] = _dr["Deduction"];
                    dr["NetAmt"] = _dr["NetAmt"];
                    dr["Approval"] = _dr["Approval"];
                    dr["AdvSalary"] = _dr["AdvSalary"];
                    dr["Resigndate"] = "";
                    dr["Configured"] = _dr["Configured"];
                    dr["EmpId"] = _dr["EmpId"];
                    dr["Advanceamount"] = _dr["Advanceamount"];
                    unresgneddata.Tables["Emp"].Rows.Add(dr);
                }
                else
                {
                    
                    resdate = General.GetDateTimeFromText(_dr["ResignDate"].ToString());
                    int monthId = resdate.Month;
                    int yearvalue = resdate.Year;
                    if (year < yearvalue)
                    {

                        dr = unresgneddata.Tables["Emp"].NewRow();
                        dr["Id"] = _dr["Id"];
                        dr["Surname"] = _dr["Surname"];
                        dr["BasicPay"] = _dr["BasicPay"];
                        dr["Gross"] = _dr["Gross"];
                        dr["Deduction"] = _dr["Deduction"];
                        dr["NetAmt"] = _dr["NetAmt"];
                        dr["Approval"] = _dr["Approval"];
                        dr["AdvSalary"] = _dr["AdvSalary"];
                        dr["Resigndate"] = _dr["ResignDate"];
                        dr["Configured"] = _dr["Configured"];
                        dr["EmpId"] = _dr["EmpId"];
                        dr["Advanceamount"] = _dr["Advanceamount"];
                        unresgneddata.Tables["Emp"].Rows.Add(dr);
                    }
                    if (yearvalue == year )
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
                            dr["Approval"] = _dr["Approval"];
                            dr["AdvSalary"] = _dr["AdvSalary"];
                            dr["Resigndate"] = _dr["ResignDate"];
                            dr["Configured"] = _dr["Configured"];
                            dr["EmpId"] = _dr["EmpId"];
                            dr["Advanceamount"] = _dr["Advanceamount"];
                            unresgneddata.Tables["Emp"].Rows.Add(dr);
                        }
                    }

                }
            }


            return unresgneddata;
        }

        public void Show()
        {
            DataSet EmpDs = new DataSet();
            DataSet unresignedds = new DataSet();
            DataSet corrcetjoinees = new DataSet();
            string _resgndate = "";
            int AdvDeduction = 0;
            int month = int.Parse(DrpMonth.SelectedValue.ToString());
            int year = int.Parse(DrpYear.SelectedItem.Text.ToString());
            int _Cat = int.Parse(DrpPayroll.SelectedValue.ToString());
            int id = 0;
            string surname = "";
            double gross = 0;
            double Deduction = 0, advsal=0;
            double NetAmt = 0;
            double BasicPay = 0;
            int status = 0;
            int _Approve = 0;
            int _Reject = 0;
            int CatId = 0, PayrollType = 0;
            string Cofig = "Not Configured";
            string AStatus = "";
            DataSet Mydataset = new DataSet();
            DataTable dt;
            DataRow dr;
            Mydataset.Tables.Add(new DataTable("Emp"));
            dt = Mydataset.Tables["Emp"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Surname");
            dt.Columns.Add("BasicPay");
            dt.Columns.Add("Gross");
            dt.Columns.Add("Deduction");
            dt.Columns.Add("NetAmt");
            dt.Columns.Add("Configured");
            dt.Columns.Add("Approval");
            dt.Columns.Add("EmpId");
            dt.Columns.Add("AdvSalary");
            dt.Columns.Add("ResignDate");
            dt.Columns.Add("JoiningDate");
            dt.Columns.Add("Advanceamount");
           
            if (DrpEmp.SelectedIndex == 0)
            {

                EmpDs = Mypay.GetDefaultEmp(_Cat, year, month);
                if (EmpDs != null && EmpDs.Tables[0].Rows.Count>0)
                {
                    foreach(DataRow _dr in EmpDs.Tables[0].Rows)
                    {
                         string joindate = _dr["JoiningDate"].ToString();
                        string EmpId = _dr["EmpId"].ToString();
                        int _Status = int.Parse(_dr["Configured"].ToString());
                       
                        string _sql = "select DATE_FORMAT(tbluserdetails_history.ResignDate,'%d/%m/%Y') from tbluserdetails_history where tbluserdetails_history.UserId in(select tblpay_employee.StaffId from tblpay_employee where tblpay_employee.EmpId='" + EmpId + "')";
                        Myreader = Mypay.m_MysqlDb.ExecuteQuery(_sql);
                        if (Myreader.HasRows)
                        {
                            _resgndate = Myreader.GetValue(0).ToString();

                            // dr["ResignDate"] = Myreader.GetValue(0).ToString();
                        }
                        else
                        {
                            _resgndate = "";
                        }
                        if (Mypay.IsPresentInMonthly(year,month, EmpId, out id, out surname, out BasicPay, out gross, out Deduction, out NetAmt, out status, out _Approve, out _Reject,out advsal,out CatId))
                        {
                            if (_Approve == 0 && _Reject == 0)
                            {
                                AStatus = "Approval Pending";
                            }
                            else if (_Approve == 1 && _Reject == 0)
                            {
                                AStatus = "Approved";
                            }
                            else
                            {
                                AStatus = "Rejected";
                            }
                            if (status == 1)
                            {
                                Cofig = "Configured";
                            }
                            dr = Mydataset.Tables["Emp"].NewRow();
                            
                            dr["Id"] = id;
                            dr["Surname"] = surname;
                            dr["BasicPay"] = BasicPay;
                            dr["Gross"] = gross;
                            dr["Deduction"] = Deduction;
                            dr["NetAmt"] = NetAmt;
                            dr["Configured"] = Cofig;
                            dr["Approval"] = AStatus;
                            dr["EmpId"] = EmpId;
                            dr["ResignDate"] = _resgndate;
                            dr["JoiningDate"] = joindate;
                            
                            dr["AdvSalary"]=advsal;
                            dr["Advanceamount"] = Mypay.GetAdvanceSalaryOnMonth(dr["EmpId"].ToString(), month, year); 
                            Mydataset.Tables["Emp"].Rows.Add(dr);

                        }
                        else
                        {
                            AStatus = "Approval Pending";
                            if (_Status == 0)
                            {
                                Cofig = "Not Configured";
                            }                            
                            dr = Mydataset.Tables["Emp"].NewRow();
                            dr["Id"] = int.Parse(_dr["Id"].ToString());
                            dr["Surname"] = _dr["Surname"].ToString();
                            dr["BasicPay"] = double.Parse(_dr["BasicPay"].ToString());
                            dr["Gross"] = double.Parse(_dr["Gross"].ToString());
                            dr["Deduction"] = double.Parse(_dr["Deduction"].ToString());
                            dr["NetAmt"] = double.Parse(_dr["NetAmt"].ToString());
                            dr["Configured"] = Cofig;
                            dr["Approval"] = AStatus;
                            dr["EmpId"] = _dr["EmpId"].ToString();
                            dr["ResignDate"] = _resgndate;
                            dr["JoiningDate"] = joindate;
                            dr["AdvSalary"] = AdvDeduction;
                            dr["AdvSalary"] = Mypay.GetAdvanceSalary(dr["EmpId"].ToString());
                            dr["Advanceamount"] = Mypay.GetAdvanceSalaryOnMonth(dr["EmpId"].ToString(), month, year); 
                            dr["NetAmt"] = (double.Parse(dr["NetAmt"].ToString()) - double.Parse(dr["AdvSalary"].ToString())).ToString();
                            Mydataset.Tables["Emp"].Rows.Add(dr);

                        }
                    }
                    //foreach (DataRow dro in Mydataset.Tables[0].Rows)
                    //{
                    //    dro["AdvSalary"] = Mypay.GetAdvanceSalary(dro["EmpId"].ToString());
                    //    dro["NetAmt"] = (double.Parse(dro["NetAmt"].ToString()) - double.Parse(dro["AdvSalary"].ToString())).ToString();
                    //}
                    Grd_EmpPay.Columns[0].Visible = true;
                    Grd_EmpPay.Columns[13].Visible = true;
                    corrcetjoinees = GetCorrectjoinees(Mydataset, month, year);
                    unresignedds = GetUnresignedData(corrcetjoinees, month, year);
                    if (unresignedds != null && unresignedds.Tables[0].Rows.Count > 0)
                    {
                        Grd_EmpPay.DataSource = unresignedds;
                        Grd_EmpPay.DataBind();
                        Grd_EmpPay.Columns[0].Visible = false;
                        Grd_EmpPay.Columns[13].Visible = false;
                        Pnl_EmpPay.Visible = true;
                        Grd_EmpPay.Visible = true;
                        Img_Export.Visible = true;
                        BtnSave.Visible = true;
                        td_color.Visible = true;
                    }
                    else
                    {
                        Grd_EmpPay.DataSource = null;
                        Grd_EmpPay.DataBind();
                        Img_Export.Visible = false;
                        BtnSave.Visible = false;
                        td_color.Visible = false;
                    }

                }

                else
                {
                    Grd_EmpPay.DataSource = null;
                    Grd_EmpPay.DataBind();
                    Img_Export.Visible = false;
                    BtnSave.Visible = false;
                    td_color.Visible = false;
                }
                ViewState["EmpGrid"] = Mydataset;

            }
            else
            {
                string  _Emp =DrpEmp.SelectedValue.ToString();
                m_MyReader = Mypay.FillEmp(_Emp, _Cat);
                if (m_MyReader.HasRows)
                {
                    while (m_MyReader.Read())
                    {
                        string joindate = m_MyReader.GetValue(8).ToString();
                        string  EmpId = m_MyReader.GetValue(6).ToString();
                        int _Status = int.Parse(m_MyReader.GetValue(7).ToString());
                        string _sql = "select tbluserdetails_history.ResignDate from tbluserdetails_history where tbluserdetails_history.UserId in(select tblpay_employee.StaffId from tblpay_employee where tblpay_employee.EmpId='" + EmpId + "')";
                        Myreader = Mypay.m_MysqlDb.ExecuteQuery(_sql);
                        if (Myreader.HasRows)
                        {
                            _resgndate = Myreader.GetValue(0).ToString();

                            // dr["ResignDate"] = Myreader.GetValue(0).ToString();
                        }
                        else
                        {
                            _resgndate = "";
                        }
                        if (Mypay.IsPresentInMonthly(year,month, EmpId, out id, out surname, out BasicPay, out gross, out Deduction, out NetAmt, out status, out _Approve, out _Reject,out advsal,out CatId))
                        {
                            if (_Approve == 0 && _Reject == 0)
                            {
                                AStatus = "Approval Pending";
                            }
                            else if (_Approve == 1 && _Reject == 0)
                            {
                                AStatus = "Approved";
                            }
                            else
                            {
                                AStatus = "Rejected";
                            }
                            if (status == 1)
                            {
                                Cofig = "Configured";
                            }
                            dr = Mydataset.Tables["Emp"].NewRow();
                            dr["Id"] = id;
                            dr["Surname"] = surname;
                            dr["BasicPay"] = BasicPay;
                            dr["Gross"] = gross;
                            dr["Deduction"] = Deduction;
                            dr["NetAmt"] = NetAmt;
                            dr["Configured"] = Cofig;
                            dr["Approval"] = AStatus;
                            dr["EmpId"] = EmpId;
                            dr["ResignDate"] = _resgndate;
                            dr["JoiningDate"] = joindate;
                            dr["AdvSalary"] = advsal;
                           
                            Mydataset.Tables["Emp"].Rows.Add(dr);
                        }
                        else
                        {
                            AStatus = "Approval Pending";
                            if (_Status == 0)
                            {
                                Cofig = "Not Configured";
                            }
                            dr = Mydataset.Tables["Emp"].NewRow();
                            dr["Id"] = int.Parse(m_MyReader.GetValue(0).ToString());
                            dr["Surname"] = m_MyReader.GetValue(1).ToString();
                            dr["BasicPay"] = double.Parse(m_MyReader.GetValue(2).ToString());
                            dr["Gross"] = double.Parse(m_MyReader.GetValue(3).ToString());
                            dr["Deduction"] = double.Parse(m_MyReader.GetValue(4).ToString());
                            dr["NetAmt"] = double.Parse(m_MyReader.GetValue(5).ToString());
                            dr["Configured"] = Cofig;
                            dr["Approval"] = AStatus;
                            dr["EmpId"] = m_MyReader.GetValue(6).ToString();
                            dr["ResignDate"] = _resgndate;
                            dr["JoiningDate"] = joindate;
                            dr["AdvSalary"] = Mypay.GetAdvanceSalary(dr["EmpId"].ToString());
                            dr["Advanceamount"] = Mypay.GetAdvanceSalaryOnMonth(dr["EmpId"].ToString(), month, year); 
                            dr["NetAmt"] = (double.Parse(dr["NetAmt"].ToString()) - double.Parse(dr["AdvSalary"].ToString())).ToString();

                            Mydataset.Tables["Emp"].Rows.Add(dr);

                        }
                    }
                    //foreach (DataRow dro in Mydataset.Tables[0].Rows)
                    //{
                    //    dro["AdvSalary"] = Mypay.GetAdvanceSalary(dro["EmpId"].ToString());
                    //    dro["NetAmt"] = (double.Parse(dro["NetAmt"].ToString()) - double.Parse(dro["AdvSalary"].ToString())).ToString();
                    //}
                    Grd_EmpPay.Columns[0].Visible = true;
                    Grd_EmpPay.Columns[13].Visible = true;                   
                    corrcetjoinees = GetCorrectjoinees(Mydataset, month, year);
                    unresignedds = GetUnresignedData(corrcetjoinees, month, year);
                    if (unresignedds != null && unresignedds.Tables[0].Rows.Count > 0)
                    {
                        Grd_EmpPay.DataSource = unresignedds;
                        Grd_EmpPay.DataBind();
                        Grd_EmpPay.Columns[0].Visible = false;
                        Grd_EmpPay.Columns[13].Visible = false;
                        Pnl_EmpPay.Visible = true;
                        Grd_EmpPay.Visible = true;
                        Img_Export.Visible = true;
                        BtnSave.Visible = true;
                        td_color.Visible = true;
                    }
                    else
                    {
                        Grd_EmpPay.DataSource = null;
                        Grd_EmpPay.DataBind();
                        Img_Export.Visible = false;
                        BtnSave.Visible = false;
                        td_color.Visible = false;
                    }
                }

                else
                {
                    Grd_EmpPay.DataSource = null;
                    Grd_EmpPay.DataBind();
                    Img_Export.Visible = false;
                    BtnSave.Visible = false;
                    td_color.Visible = false;
                }
                ViewState["EmpGrid"] = unresignedds;


                
            }
            ColorGrid();
        }

        private void ColorGrid()
        {
            foreach (GridViewRow gv in Grd_EmpPay.Rows)
            {
                string resgndate = gv.Cells[11].Text.Replace("&nbsp;", "").ToString();
                if (resgndate != "")
                {
                    gv.BackColor = Color.Salmon;
                }
            }
        }

        
        protected void Grd_EmpPay_Selectedindexchanged(object sender, EventArgs e)
        {
            if (int.Parse(DrpMonth.SelectedValue.ToString()) > 0)
            {
                int YearId = int.Parse(DrpYear.SelectedValue);
                string  id = Grd_EmpPay.Rows[Grd_EmpPay.SelectedIndex].Cells[0].Text;
                int Cat = int.Parse(DrpPayroll.SelectedValue.ToString());
                string  employeeId = Grd_EmpPay.Rows[Grd_EmpPay.SelectedIndex].Cells[13].Text;
                int MonthId = int.Parse(DrpMonth.SelectedValue.ToString());
                int Year = int.Parse(DrpYear.SelectedItem.Text.ToString());
                double gross = double.Parse(Grd_EmpPay.Rows[Grd_EmpPay.SelectedIndex].Cells[4].Text);
                double total = double.Parse(Grd_EmpPay.Rows[Grd_EmpPay.SelectedIndex].Cells[5].Text);
                double netpay = double.Parse(Grd_EmpPay.Rows[Grd_EmpPay.SelectedIndex].Cells[8].Text);
                string resigndate = Grd_EmpPay.Rows[Grd_EmpPay.SelectedIndex].Cells[11].Text.Replace("&nbsp;","");
                double Advdeduction = double.Parse(Grd_EmpPay.Rows[Grd_EmpPay.SelectedIndex].Cells[7].Text);
                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "openIncpopup('ViewEmployeeSalary.aspx?id=" + id + "&Cat=" + Cat + "&EmpId=" + employeeId + "&ChkId=" + MonthId + "&Year=" + Year + "&Gross=" + gross + " &Total= " + total + " &NetPay= " + netpay + "&resigndate=" + resigndate + "&Advdeduction=" + Advdeduction + "');", true);

                Session["MSCmonth"] = MonthId;
                Session["MSCyear"] = YearId;
                Session["MSCCat"] = Cat;
                Session["MSCempId"] = employeeId;
            }
            else
            {
                WC_MessageBox.ShowMssage("Please Select Month");
            }
        }

        protected void DrpYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMonthToDrpList();
            LoadEmployeeToDropdown();
        }

        private void LoadEmployeeToDropdown()
        {
            DrpEmp.Items.Clear();
            DataSet Empds=new DataSet();
            int monthId=int.Parse(DrpMonth.SelectedValue);
            int Year=int.Parse(DrpYear.SelectedItem.Text);
            int CatId=int.Parse(DrpPayroll.SelectedValue);
            if (int.Parse(DrpPayroll.SelectedValue) > 0)
            {
                Empds = Mypay.FillEmpDrp(CatId, Year, monthId);
                if (Empds != null && Empds.Tables[0].Rows.Count > 0)
                {
                    DrpEmp.Enabled = true;
                    ListItem li;
                    li = new ListItem("All", "0");
                    DrpEmp.Items.Add(li);
                    foreach (DataRow dr in Empds.Tables[0].Rows)
                    {
                        li=new ListItem(dr["Surname"].ToString(), dr["EmpId"].ToString());
                        DrpEmp.Items.Add(li);
                    }
                }
                else
                {
                    DrpEmp.Items.Add("No employees found");
                }
            }
            else
            {
                DrpEmp.Items.Clear();
                DrpEmp.Enabled = false;
            }
        }

        protected void Btn_AddSalAdv_Click(object sender, EventArgs e)
        {

        }

        protected void Grd_EmpPay_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((DataBinder.Eval(e.Row.DataItem, "ResignDate")).ToString() != "")
                {
                    // color the background of the row yellow
                    e.Row.BackColor = Color.Yellow;
                }
            }


            //if (e.Row.Cells[12].Text.ToString() != "" && e.Row.Cells[12].Text.ToString() != "ResignDate")
            //    {
            //        e.Row.BackColor = Color.Red;
            //    }
            ////for (int i = 0; i < Grd_EmpPay.Rows.Count; i++)
            ////{
            ////    if (Grd_EmpPay.Rows[i].Cells[12].Text.ToString() != "")
            ////    {
            ////        Grd_EmpPay.Rows[i].BackColor = Color.Red;
            ////    }
            ////}
            //////   if (e.Row.RowType == DataControlRowType.DataRow)
            //////   {
            //////    if(e.Row.DataItem("UnitsInStock") < 20) 

            //////e.Row.BackColor = Drawing.Color.Red
            //////   }
        }

        protected void DrpMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadEmployeeToDropdown();
        }


        
    }
}



