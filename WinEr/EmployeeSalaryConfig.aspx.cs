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
    public partial class EmployeeSalaryConfig : System.Web.UI.Page
    {
        private WinBase.Payroll Mypay;
        private KnowinUser MyUser;
        private OdbcDataReader m_MyReader = null;
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
            else if (!MyUser.HaveActionRignt(802))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (!IsPostBack)
                {
                    LoadYearToDrpList();
                    Lblmsg.Visible = false;
                    LoadGrid();
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

                DrpYear.SelectedValue = (i-1).ToString();
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


        protected void Grd_EmpPay_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            //CheckBox chk_Select;
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
                              
            //    int _StatusId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Approval"));
            //    chk_Select = (CheckBox)e.Row.FindControl("ChkPayroll");
            //    if (_StatusId == 1)
            //    {
            //        chk_Select.Checked = true;
            //    }
            //    else
            //    {
            //        chk_Select.Checked = false;
            //    }
               
            // }

        }
        
        private void LoadGrid()
        {
            
            OdbcDataReader MyPayReader = null;
            DataSet EmpDataSet = new DataSet();
            EmpDataSet.Tables.Add(new DataTable("Emp"));
            DataTable dt;
            DataRow dr;
            dt = EmpDataSet.Tables["Emp"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Surname");
            dt.Columns.Add("BasicPay");
            dt.Columns.Add("Gross");
            dt.Columns.Add("Deduction");
            dt.Columns.Add("NetAmt");
            dt.Columns.Add("Approval");
            dt.Columns.Add("PayrollType");
            dt.Columns.Add("EmpId");
            MyPayReader = Mypay.GetEmp();
            if (MyPayReader.HasRows)
            {
                while (MyPayReader.Read())
                {
                    int Approve = 0;
                    int Reject = 0;
                    string Status = "";
                    Approve = int.Parse(MyPayReader.GetValue(6).ToString());
                    Reject = int.Parse(MyPayReader.GetValue(9).ToString());
                    int userId = int.Parse(MyPayReader.GetValue(0).ToString());                    
                    if (Approve == 0 && Reject == 0)
                    {
                        Status = "Approval Pending";
                    }
                    else if (Approve == 1 && Reject == 0)
                    {
                        Status = "Approved";
                    }
                    else
                    {
                        Status = "Rejected";
                    }
                    dr = EmpDataSet.Tables["Emp"].NewRow();
                    dr["Id"] = int.Parse(MyPayReader.GetValue(0).ToString());
                    dr["Surname"] = MyPayReader.GetValue(1).ToString();
                    dr["BasicPay"] = double.Parse(MyPayReader.GetValue(2).ToString());
                    dr["Gross"] = double.Parse(MyPayReader.GetValue(3).ToString());
                    dr["Deduction"] = double.Parse(MyPayReader.GetValue(4).ToString());
                    dr["NetAmt"] = double.Parse(MyPayReader.GetValue(5).ToString());
                    dr["Approval"] = Status;
                    dr["PayrollType"] = int.Parse(MyPayReader.GetValue(7).ToString());
                    dr["EmpId"] = MyPayReader.GetValue(8).ToString();
                    EmpDataSet.Tables["Emp"].Rows.Add(dr);
                }
                Grd_EmpPay.Columns[0].Visible = true;
                Grd_EmpPay.Columns[1].Visible = true;
                Grd_EmpPay.Columns[8].Visible = true;
                Grd_EmpPay.Columns[9].Visible = true;
                DataSet unresignedds = new DataSet();
                unresignedds= GetUnresignedData(EmpDataSet);
                if (unresignedds != null && unresignedds.Tables[0].Rows.Count > 0)
                {
                    Grd_EmpPay.DataSource = unresignedds;
                    Grd_EmpPay.DataBind();
                    Grd_EmpPay.Columns[0].Visible = false;
                    Grd_EmpPay.Columns[1].Visible = false;
                    Grd_EmpPay.Columns[8].Visible = false;
                    Grd_EmpPay.Columns[9].Visible = false;
                    Pnl_EmpPay.Visible = true;
                    Grd_EmpPay.Visible = true;
                    //Btn_Save.Visible = true;
                    Td_color.Visible = true;
                }
                else
                {
                    Grd_EmpPay.Columns[0].Visible = true;
                    Grd_EmpPay.Columns[1].Visible = true;
                    Grd_EmpPay.Columns[8].Visible = true;
                    Grd_EmpPay.Columns[9].Visible = true;
                    Grd_EmpPay.DataSource = null;
                    Grd_EmpPay.DataBind();
                    Grd_EmpPay.Columns[0].Visible = false;
                    Grd_EmpPay.Columns[1].Visible = false;
                    Grd_EmpPay.Columns[8].Visible = false;
                    Grd_EmpPay.Columns[9].Visible = false;
                    Pnl_EmpPay.Visible = false;
                    Grd_EmpPay.Visible = false;
                    Lblmsg.Visible = true;
                    Lblmsg.Text = "No employees found";
                    Td_color.Visible = false;
                    //Btn_Save.Visible = false;
                }
            }
            else
            {
                Grd_EmpPay.Columns[0].Visible = true;
                Grd_EmpPay.Columns[1].Visible = true;
                Grd_EmpPay.Columns[8].Visible = true;
                Grd_EmpPay.Columns[9].Visible = true;
                Grd_EmpPay.DataSource = null;
                Grd_EmpPay.DataBind();
                Grd_EmpPay.Columns[0].Visible = false;
                Grd_EmpPay.Columns[1].Visible = false;
                Grd_EmpPay.Columns[8].Visible = false;
                Grd_EmpPay.Columns[9].Visible = false;
                Pnl_EmpPay.Visible = false;
                Grd_EmpPay.Visible = false;
                Lblmsg.Visible = true;
                Lblmsg.Text = "No employees found";
                Td_color.Visible = false;
               //Btn_Save.Visible = false;
            }

            ColorGrid();
            
        }
        private void ColorGrid()
        {
            foreach (GridViewRow gv in Grd_EmpPay.Rows)
            {
                string resgndate = gv.Cells[10].Text.Replace("&nbsp;", "").ToString();
                if (resgndate != "")
                {
                    gv.BackColor = Color.Salmon;
                }
            }
        }

        private DataSet GetUnresignedData(DataSet ds)
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
            dt.Columns.Add("Approval");
            dt.Columns.Add("PayrollType");
            dt.Columns.Add("Resigndate");
            dt.Columns.Add("EmpId");

            
            OdbcDataReader myresreader = null;
            foreach (DataRow _dr in ds.Tables[0].Rows)
            {
                DateTime resdate=new DateTime();
                DateTime tdate = new DateTime();
                string resndate;
                string _sql = "select DATE_FORMAT(tbluserdetails_history.ResignDate,'%d/%m/%Y') from tbluserdetails_history where tbluserdetails_history.UserId in (select tblpay_employee.StaffId from tblpay_employee where tblpay_employee.EmpId='" + _dr["EmpId"].ToString() + "')";
                myresreader = Mypay.m_MysqlDb.ExecuteQuery(_sql);
                tdate = System.DateTime.Now.Date;
                if (myresreader.HasRows)
                {
                    resndate = myresreader.GetValue(0).ToString();
                    if (resndate != "")
                    {
                        resdate = General.GetDateTimeFromText(resndate);
                    }
                    if (tdate <= resdate)
                    {

                        dr = unresgneddata.Tables["Emp"].NewRow();
                        dr["Id"] = _dr["Id"];
                        dr["Surname"] = _dr["Surname"];
                        dr["BasicPay"] = _dr["BasicPay"];
                        dr["Gross"] = _dr["Gross"];
                        dr["Deduction"] = _dr["Deduction"];
                        dr["NetAmt"] = _dr["NetAmt"];
                        dr["Approval"] = _dr["Approval"];
                        dr["PayrollType"] = _dr["PayrollType"];
                        dr["Resigndate"] = resndate;
                        dr["EmpId"] = _dr["EmpId"];
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
                    dr["Approval"] = _dr["Approval"];
                    dr["PayrollType"] = _dr["PayrollType"];
                    dr["Resigndate"] = "";
                    dr["EmpId"] = _dr["EmpId"];
                    unresgneddata.Tables["Emp"].Rows.Add(dr);
                }
                
                
            }
            return unresgneddata;
        }

        protected void Grd_EmpPay_Selectedindexchanged(object sender, EventArgs e)
        {
           // int Year = int.Parse(DrpYear.SelectedItem.ToString());

            int Year = System.DateTime.Now.Year;

            int id = int.Parse(Grd_EmpPay.Rows[Grd_EmpPay.SelectedIndex].Cells[0].Text);
            int Cat = int.Parse(Grd_EmpPay.Rows[Grd_EmpPay.SelectedIndex].Cells[8].Text);
            string  employeeId = Grd_EmpPay.Rows[Grd_EmpPay.SelectedIndex].Cells[9].Text;
            double gross = int.Parse(Grd_EmpPay.Rows[Grd_EmpPay.SelectedIndex].Cells[4].Text);
            double total = int.Parse(Grd_EmpPay.Rows[Grd_EmpPay.SelectedIndex].Cells[5].Text);
            double netpay = int.Parse(Grd_EmpPay.Rows[Grd_EmpPay.SelectedIndex].Cells[6].Text);
            int MonthId = 0;
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "openIncpopup('ViewEmployeeSalary.aspx?id=" + id + "&Cat=" + Cat + "&EmpId=" + employeeId + "&ChkId=" + MonthId + "&Year=" + Year + "&Gross=" + gross + " &Total= " + total+ " &NetPay= " + netpay +"');", true);
            

        }

        protected void Grd_EmpPay_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_EmpPay.PageIndex = e.NewPageIndex;
            LoadGrid();
            
        }

        //protected void Grd_EmpPay_PageIndexChanged(object sender, EventArgs e)
        //{
        //    Grd_EmpPay.PageIndex=
        //    LoadGrid();
        //}

    }
}
