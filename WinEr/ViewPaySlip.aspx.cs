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
    public partial class ViewPaySlip : System.Web.UI.Page
    {
        private StaffManager MyStaffMang;
        private WinBase.Payroll Mypay;
        private KnowinUser MyUser;
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
            MyStaffMang = MyUser.GetStaffObj();
            if (Mypay == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(844))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                int staffId = int.Parse(Session["StaffId"].ToString());
                int _roleId = Mypay.GetRoleId(staffId);
                int LoginId = MyUser.User_Id;
               
                if (!IsPostBack)
                {
                    if (MyUser.HaveActionRignt(845))
                    {
                        div_NotHaveRight.Visible = false;
                        Div_havrgt.Visible = true;
                        Pnl_View.Visible = false;
                        LoadYearToDropDown();
                        LoaduserTopData();
                        LoadMonthToDropdown();
                        string _SubMenuStr;
                        _SubMenuStr = MyStaffMang.GetSubStaffMangMenuString(MyUser.UserRoleId);
                        this.SubStaffMenu.InnerHtml = _SubMenuStr;
                    }
                    else if (MyUser.HaveActionRignt(846))
                    {
                        if (staffId == LoginId)
                        {
                            div_NotHaveRight.Visible = false ;
                            Div_havrgt.Visible = true ;
                            Pnl_View.Visible = false;
                            LoadYearToDropDown();
                            LoaduserTopData();
                            LoadMonthToDropdown();
                            string _SubMenuStr;
                            _SubMenuStr = MyStaffMang.GetSubStaffMangMenuString(MyUser.UserRoleId);
                            this.SubStaffMenu.InnerHtml = _SubMenuStr;
                        }
                        else
                        {
                            LoaduserTopData();
                            div_NotHaveRight.Visible = true;
                            Div_havrgt.Visible = false;
                            string _SubMenuStr;
                            _SubMenuStr = MyStaffMang.GetSubStaffMangMenuString(MyUser.UserRoleId);
                            this.SubStaffMenu.InnerHtml = _SubMenuStr;
                            Pnl_View.Visible = false;
                        }
                    }
                }
               
            }
        }
        protected void Drp_Year_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMonthToDropdown();
        }

        protected void Btn_View_Click(object sender, EventArgs e)
        {
            Lbl_payslipErr.Text = "";
            int MonthId = int.Parse(Drp_Month.SelectedValue.ToString());
            int Year = int.Parse(Drp_Year.SelectedItem.ToString());
            int staffId =int.Parse(Session["StaffId"].ToString());
            string EmpId = Mypay.GetEmpId(staffId, MonthId, Year);
           // string EmplId = Request.QueryString["EmpId"].ToString();
            
            if (EmpId != "")
            {
                if (MyStaffMang.SalaryPayed(EmpId, MonthId, Year))
                {
                    Div_Payslip.InnerHtml = Mypay.GetSalSlip(EmpId, MonthId, Year);
                    Pnl_View.Visible = true;
                }
                else
                {
                    Pnl_View.Visible = false;
                    Lbl_payslipErr.Text = "No payslip is created for this month";
                }
            }
            else
            {
                Pnl_View.Visible = false;
                Lbl_payslipErr.Text = "No payslip is created for this month";
            }

        }

      

        #region Methods

        private void LoaduserTopData()
        {
            
            string _Studstrip = MyStaffMang.ToStripString(int.Parse(Session["StaffId"].ToString()),"Handler/ImageReturnHandler.ashx?id=" +int.Parse(Session["StaffId"].ToString())+ "&type=StaffImage");
            this.StudentTopStrip.InnerHtml = _Studstrip;
        }

        private void LoadMonthToDropdown()
        {
            int Month = 0;
            string[] months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            DateTime _Current = DateTime.Now.Date;
            int Yr = int.Parse(Drp_Year.SelectedItem.Text.ToString());
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
            Drp_Month.Items.Clear();
            ListItem li;
            for (int i = 0; i < Month; i++)
            {

                li = new ListItem(months[i], (i + 1).ToString());
                Drp_Month.Items.Add(li);
            }
            Drp_Month.SelectedValue = (Month).ToString();
        }

        private void LoadYearToDropDown()
        {
            int CurrentYear = DateTime.Now.Year;
            ListItem li = new ListItem();
            li = new ListItem((CurrentYear - 2).ToString(), "0");
            Drp_Year.Items.Add(li);
            li = new ListItem((CurrentYear - 1).ToString(), "1");
            Drp_Year.Items.Add(li);
            li = new ListItem(CurrentYear.ToString(), "2");
            Drp_Year.Items.Add(li);

            Drp_Year.SelectedValue = "2";
        }


        #endregion

       

       
        
    }
}
