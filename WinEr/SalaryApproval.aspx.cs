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
    public partial class SalaryApproval : System.Web.UI.Page
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
            else if (!MyUser.HaveActionRignt(803))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (!IsPostBack)
                {
                    RdbNonApprove.Checked = true;
                    LoadNonApproveGrid();
                    BtnApprove.Visible = true;
                    BtnReject.Visible = true;
                  
                    //some initlization

                }
            }
            
        }
        protected void RdbApprove_CheckedChanged(object sender, EventArgs e)
        {
            BtnApprove.Visible = false;
            BtnReject.Visible = true;
            LoadApproveGrid();
        }
        protected void RdbRejected_CheckedChanged(object sender, EventArgs e)
        {
            BtnApprove.Visible = true;
            BtnReject.Visible = false; ;
            LoadRejectedGrid();
        }
        protected void RdbNonApprove_CheckedChanged(object sender, EventArgs e)
        {
            BtnApprove.Visible = true;
            BtnReject.Visible = true;
            LoadNonApproveGrid();
        }

        private void LoadApproveGrid()
        {
            DataSet Unresignedds = Mypay.GetApprovedEmp();
            DataSet MyEmp = new DataSet();
            MyEmp = GetUnresignedData(Unresignedds);
            if (MyEmp != null && MyEmp.Tables != null && MyEmp.Tables[0].Rows.Count > 0)
            {
                Grd_EmpPay.Columns[0].Visible = true;
                Grd_EmpPay.Columns[8].Visible = true;
                Grd_EmpPay.DataSource = MyEmp;
                Grd_EmpPay.DataBind();
                Grd_EmpPay.Columns[0].Visible = false;
                Grd_EmpPay.Columns[8].Visible = false;
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
            DataSet Unresignedds = Mypay.GetRejectededEmp();
            DataSet MyEmp = new DataSet();
            MyEmp = GetUnresignedData(Unresignedds);
            if (MyEmp != null && MyEmp.Tables != null && MyEmp.Tables[0].Rows.Count > 0)
            {
                Grd_EmpPay.Columns[0].Visible = true;
                Grd_EmpPay.Columns[8].Visible = true;
                Grd_EmpPay.DataSource = MyEmp;
                Grd_EmpPay.DataBind();
                Grd_EmpPay.Columns[0].Visible = false;
                Grd_EmpPay.Columns[8].Visible = false;
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

        

        private void LoadNonApproveGrid()
        {
            int unresn = 0;
            DataSet Unresignedds = Mypay.GetNonApprovedEmp();
            DataSet MyEmp = new DataSet();
            MyEmp = GetUnresignedData(Unresignedds);
            if (MyEmp != null && MyEmp.Tables != null && MyEmp.Tables[0].Rows.Count > 0)
            {
                Grd_EmpPay.Columns[0].Visible = true;
                Grd_EmpPay.Columns[8].Visible = true;
                Grd_EmpPay.DataSource = MyEmp;
                Grd_EmpPay.DataBind();
                Grd_EmpPay.Columns[0].Visible = false;
                Grd_EmpPay.Columns[8].Visible = false;
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
                resgndate = gv.Cells[9].Text.Replace("&nbsp;", "").ToString();
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
            dt.Columns.Add("Comment");
            dt.Columns.Add("EmpId");
            dt.Columns.Add("Resigndate");


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
                    if (resndate != "")
                    {
                        resdate = General.GetDateTimeFromText(resndate);
                    }
                    //if (unresn == 1)
                    //{
                        if (tdate <= resdate)
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
                            unresgneddata.Tables["Emp"].Rows.Add(dr);
                        }



                    //}
                    //else if (unresn == 0)
                    //{
                    //    dr = unresgneddata.Tables["Emp"].NewRow();
                    //    dr["Id"] = _dr["Id"];
                    //    dr["Surname"] = _dr["Surname"];
                    //    dr["BasicPay"] = _dr["BasicPay"];
                    //    dr["Gross"] = _dr["Gross"];
                    //    dr["Deduction"] = _dr["Deduction"];
                    //    dr["NetAmt"] = _dr["NetAmt"];
                    //    dr["AdvSal"] = _dr["AdvSal"];
                    //    dr["Resigndate"] = resndate;
                    //    dr["EmpId"] = _dr["EmpId"];
                    //    unresgneddata.Tables["Emp"].Rows.Add(dr);
                    //}
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
                    unresgneddata.Tables["Emp"].Rows.Add(dr);
                }
            }
            return unresgneddata;
        }



        protected void BtnApprove_Click(object sender, EventArgs e)
        {

            int flag = 0;
            foreach (GridViewRow gv in Grd_EmpPay.Rows)
            {
                CheckBox Chk_Pay = (CheckBox)gv.FindControl("ChkPayroll");
                int EmpGridId = 0;
                string EmpGridname = "";
                if (Chk_Pay.Checked)
                {
                    flag = 1;
                    bool status = true;
                    EmpGridId = int.Parse(gv.Cells[0].Text.ToString());
                    EmpGridname = gv.Cells[2].Text.ToString();
                    Mypay.UpdateEmpSal(EmpGridId, status, MyUser.UserName, EmpGridname);

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


            if (flag == 1)
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
            int flag = 0;
            foreach (GridViewRow gv in Grd_EmpPay.Rows)
            {
                CheckBox Chk_Pay = (CheckBox)gv.FindControl("ChkPayroll");
                int EmpGridId = 0;
                if (Chk_Pay.Checked)
                {
                    flag = 1;
                    string EmpGridname = "";
                    bool status = false;
                    EmpGridId = int.Parse(gv.Cells[0].Text.ToString());
                    EmpGridname = gv.Cells[2].Text.ToString();
                    Mypay.UpdateEmpSal(EmpGridId, status,MyUser.UserName,EmpGridname);
                   
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
            if (flag == 1)
            {
                WC_MessageBox.ShowMssage("Selected employee salary has been rejected");

            }
            else
            {
                WC_MessageBox.ShowMssage("Please select any employee to reject salary");
            }
            

        }

        protected void Grd_EmpPay_Selectedindexchanged(object sender, EventArgs e)
        {
            OdbcDataReader MyPayReader = null;
            string  Emp_Id = Grd_EmpPay.Rows[Grd_EmpPay.SelectedIndex].Cells[8].Text;
            MyPayReader = Mypay.GetComments(Emp_Id);
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
            string  Emp_Id =Grd_EmpPay.Rows[Grd_EmpPay.SelectedIndex].Cells[8].Text;
            string UpdateComment = Txt_Comment.Text.Trim();
            Mypay.UpdateComment(Emp_Id, UpdateComment);
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
