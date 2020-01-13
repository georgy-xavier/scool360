using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.Odbc;
using WinBase;
using System.Data;

namespace WinEr
{
    public partial class ViewEmployeeSalary : System.Web.UI.Page
    {
        private WinBase.Payroll Mypay;
        private StaffManager MyStaffMang;
        private KnowinUser MyUser;
        private DataSet MydataSet = null;
        private OdbcDataReader m_MyReader = null;
        private OdbcDataReader m_MyReader1 = null;
        double gross;
        double total ;
        double netpay;
        string resigndate = "";
        int MonthId;
        double advdedction;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.aspx");
            }
          
            int EId = int.Parse(Request.QueryString["id"].ToString());
            int Cat = int.Parse(Request.QueryString["Cat"].ToString());
            string  EmplId =Request.QueryString["EmpId"].ToString();
            MonthId = int.Parse(Request.QueryString["ChkId"].ToString());
            int Year = int.Parse(Request.QueryString["Year"].ToString());
            gross = double.Parse(Request.QueryString["Gross"].ToString());
            total = double.Parse(Request.QueryString["Total"].ToString());
            netpay = double.Parse(Request.QueryString["NetPay"].ToString());
            

           
           

            if (MonthId == 0)
            {
                Lbl_TotWked.Visible = false;
                Lbl_TotWkng.Visible = false;
                txt_TotWked.Text = "0";
                txt_TotWked.Visible = false;
                txt_TotWkng.Text = "0";
                txt_TotWkng.Visible = false;
                Lbl_Payroll.Visible = true;
                Drp_PayCat.Visible = true;
                Lnk_SalAdvance.Visible = false;
                Lbl_ResignDate.Visible = false;
                Txt_resignDate.Visible = false;
                Txt_previousAdvamount.Visible = false;
                Lbl_PreviousAdvamount.Visible = false;
                Lbl_advdeduction.Visible = false;
                Txt_advsaldeduction.Visible = false;
            
            }
            else
            {
                
                resigndate = Request.QueryString["resigndate"].ToString();
                advdedction = double.Parse(Request.QueryString["Advdeduction"].ToString());
                Lbl_ResignDate.Visible = true;
                Txt_resignDate.Visible = true;
                Lbl_TotWked.Visible = true;
                Lbl_TotWkng.Visible = true;
                txt_TotWked.Visible = true;
                txt_TotWkng.Visible = true;
                Lbl_Payroll.Visible = false;
                Drp_PayCat.Visible = false;
                Lnk_SalAdvance.Visible = true;
                Txt_previousAdvamount.Visible = false;
                Lbl_PreviousAdvamount.Visible = false;
                Lbl_advdeduction.Visible = false;
                Txt_advsaldeduction.Visible = false;

            }
            MyUser = (KnowinUser)Session["UserObj"];
            Mypay = MyUser.GetPayrollObj();
            if (Mypay == null)
            {
                Response.Redirect("RoleErr.aspx");
                //no rights for this user.
            }
            MyStaffMang = MyUser.GetStaffObj();
            if (MyStaffMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else
            {
                if (!IsPostBack)
                {
                   
                    if (MonthId != 0)
                    {
                        ChkPayed(EmplId, MonthId,Year);

                    }
                    LoadEmpData(MonthId, EmplId, Cat, Year);
                    //some initlization
                    InitialChecking();
                }
            }

        }

        private void InitialChecking()
        {
            reg.Enabled = false;
            RequiredFieldValidator7.Enabled = false;
            if (MyStaffMang.NeedPanNumberMantatory())
            {
                reg.Enabled = true;
                RequiredFieldValidator7.Enabled = true;
            }
        }

        private void chkStatus(string  EmplId)
        {
            OdbcDataReader MyPayReader = null;
            MyPayReader = Mypay.CheckStatus(EmplId);
            if (MyPayReader.HasRows)
            {
                while (MyPayReader.Read())
                {
                    int status = int.Parse(MyPayReader.GetValue(0).ToString());
                    if (status == 1)
                    {
                        Pnl_EditEmp.Enabled = false;
                    }
                    else
                    {
                        Pnl_EditEmp.Enabled = true;
                    }
                }
            }
        }

        private void ChkPayed(string EmplId, int MonthId,int year)
        {
            
            OdbcDataReader MyPayReader = null;
            MyPayReader = Mypay.CheckPayed(EmplId, MonthId,year);
            if (MyPayReader.HasRows)
            {
                while (MyPayReader.Read())
                {
                    int Payed = int.Parse(MyPayReader.GetValue(0).ToString());
                    int Status = int.Parse(MyPayReader.GetValue(1).ToString());
                    if (Payed == 1 || Status ==1)
                    {
                        Pnl_EditEmp.Enabled = false;
                    }
                    else
                    {
                        Pnl_EditEmp.Enabled = true;
                    }
                }
            }
        }

       
       

        private void LoadEmpData(int MonthId, string  EmplId, int CatId,int year)
        {

            
            if (MonthId == 0)
            {

                Txt_EditName.Enabled = true;
                Txt_Pan.Enabled = true;
                txt_EditBank.Enabled = true;
                OdbcDataReader MyPayReader = null;
                MyPayReader = Mypay.LoadEditPopUp(EmplId);
                if (MyPayReader.HasRows)
                {
                    while (MyPayReader.Read())
                    {
                        Txt_EditName.Text = MyPayReader.GetValue(0).ToString();
                        txt_EditPay.Text = MyPayReader.GetValue(1).ToString();
                        txt_Pay.Text = MyPayReader.GetValue(1).ToString();
                        Txt_Pan.Text = MyPayReader.GetValue(2).ToString();
                        txt_EditBank.Text = MyPayReader.GetValue(3).ToString();
                        TxtComment.Text = MyPayReader.GetValue(7).ToString();
                        if (resigndate == "")
                        {
                            Lbl_ResignDate.Visible = false;
                            Txt_resignDate.Visible = false;
                        }
                        else
                        {
                            Txt_resignDate.Text = resigndate;
                            
                        }

                        if (advdedction > 0)
                        {
                            Lbl_advdeduction.Visible = true;
                            Txt_advsaldeduction.Visible = true;
                            Txt_advsaldeduction.Text = advdedction.ToString();
                            Hdn_advaded.Value = advdedction.ToString();
                        }
                        else
                        {
                            Lbl_advdeduction.Visible = false;
                            Txt_advsaldeduction.Visible = false;
                            Hdn_advaded.Value = "";
                        }

                    }
                    LoadCatToDrpList(Drp_PayCat, CatId);
                    LoadDeductionGrid(EmplId);
                    LoadEarningGrid(EmplId);
                    LoadAmount();
                }
                
            }
            else
            {
             
                OdbcDataReader MyNewPayReader = null;
                OdbcDataReader MyNewAdvReader = null;

                Txt_EditName.Enabled = false;
                Txt_Pan.Enabled = false;
                txt_EditBank.Enabled = false;
                MyNewPayReader = Mypay.LoadMonthEmp(EmplId, MonthId,year);
                if (MyNewPayReader.HasRows)
                {
                    while (MyNewPayReader.Read())
                    {
                        Txt_EditName.Text = MyNewPayReader.GetValue(1).ToString();
                        txt_EditPay.Text = MyNewPayReader.GetValue(2).ToString();
                        txt_Pay.Text = MyNewPayReader.GetValue(2).ToString();
                        Txt_Pan.Text = MyNewPayReader.GetValue(3).ToString();
                        txt_EditBank.Text = MyNewPayReader.GetValue(4).ToString();
                        txt_TotWkng.Text = MyNewPayReader.GetValue(5).ToString();
                        txt_TotWked.Text = MyNewPayReader.GetValue(6).ToString();
                        TxtComment.Text = MyNewPayReader.GetValue(7).ToString();
                        if (resigndate != "")
                        {

                            Txt_resignDate.Text = resigndate;
                        }
                        else
                        {
                            Lbl_ResignDate.Visible = false;
                            Txt_resignDate.Visible = false;                        
                        }

                        if (advdedction > 0)
                        {
                            Lbl_advdeduction.Visible = true;
                            Txt_advsaldeduction.Visible = true;
                            Txt_advsaldeduction.Text = advdedction.ToString();
                            Hdn_advaded.Value = advdedction.ToString();
                        }
                        else
                        {
                            Lbl_advdeduction.Visible = false;
                            Txt_advsaldeduction.Visible = false;
                            Hdn_advaded.Value ="";
                        }

                      

                    }
                    LoadMonthEarningGrid(EmplId, MonthId);
                    LoadMonthDeductionGrid(EmplId, MonthId);
                    LoadAmount();
                    MyNewAdvReader = Mypay.GetAdvancesalOfMonth(MonthId, EmplId);
                    if (MyNewAdvReader.HasRows)
                    {
                        double advamount=double.Parse(MyNewAdvReader.GetValue(0).ToString());
                        if (advamount != 0)
                        {
                            Txt_previousAdvamount.Visible = true;
                            Lbl_PreviousAdvamount.Visible = true;
                            Txt_previousAdvamount.Text = advamount.ToString();
                        }
                        else
                        {
                            Txt_previousAdvamount.Text = "";
                            Txt_previousAdvamount.Visible = false;
                            Lbl_PreviousAdvamount.Visible = false;
                    }
                    }
                    else
                    {
                        Txt_previousAdvamount.Text = "";
                        Txt_previousAdvamount.Visible = false;
                        Lbl_PreviousAdvamount.Visible = false;
                    }
                  
                    
                }

                else
                {
                    MonthId = 0;
                    LoadEmpData(MonthId, EmplId, CatId, year);
                }
                   
            }
            CalculatEditAmount();
            //Calculations();
        }

        private void LoadAmount()
        {
            txt_NetPay.Value =netpay.ToString();
            txt_Gross.Value = gross.ToString();
            Txt_Total.Value = total.ToString();
        }

        
        private void LoadMonthDeductionGrid(string  EmplId, int MonthId)
        {
            string _type = "";
            string sep = "";
            DataSet MyDedEmp = Mypay.GetMonthDeductionEmp(EmplId, MonthId);
            if (MyDedEmp != null && MyDedEmp.Tables != null && MyDedEmp.Tables[0].Rows.Count > 0)
            {
                Grd_Deduction.Columns[0].Visible = true;
                Grd_Deduction.Columns[3].Visible = true;
                Grd_Deduction.Columns[5].Visible = true;
                Grd_Deduction.DataSource = MyDedEmp;
                Grd_Deduction.DataBind();
                foreach (GridViewRow gv in Grd_Deduction.Rows)
                {

                    _type = _type + sep + gv.Cells[5].Text.Trim();
                    sep = ",";

                }
                HdnDedType.Value = _type;
                Grd_Deduction.Columns[0].Visible = false;
                Grd_Deduction.Columns[3].Visible = false;
                Grd_Deduction.Columns[5].Visible = false;


            }
            else
            {
                Grd_Deduction.DataSource = null;
                Grd_Deduction.DataBind();
            }
            ViewState["DedGrid"] = MyDedEmp;
        }

        private void LoadMonthEarningGrid(string EmplId, int MonthId)
        {
            int Year = int.Parse(Request.QueryString["Year"].ToString());

            string _type = "";
            string sep = "";
            DataSet MyEarnEmp = Mypay.GetMonthEarningEmp(EmplId,MonthId,Year);
            if (MyEarnEmp != null && MyEarnEmp.Tables != null && MyEarnEmp.Tables[0].Rows.Count > 0)
            {
                Grd_Earning.Columns[0].Visible = true;
                Grd_Earning.Columns[3].Visible = true;
                Grd_Earning.Columns[5].Visible = true;
                Grd_Earning.DataSource = MyEarnEmp;
                Grd_Earning.DataBind();
                foreach (GridViewRow gv in Grd_Earning.Rows)
                {

                    _type = _type + sep + gv.Cells[5].Text.Trim();
                    sep = ",";

                }
                HdnEarnType.Value = _type;
                Grd_Earning.Columns[0].Visible = false;
                Grd_Earning.Columns[3].Visible = false;
                Grd_Earning.Columns[5].Visible = false;


            }
            else
            {
                Grd_Earning.DataSource = null;
                Grd_Earning.DataBind();
            }
            ViewState["EarnGrid"] = MyEarnEmp;
        }

        public void Calculations()
        {
            double GAmt = 0;
            double DAmt = 0;
            double _Bp = 0;
            string _EarnPer = "";
            string _DedPer = "";
            string _sep = "";
            double EarnPercent = 0;
            double DedPercent = 0;
            HtmlInputControl Txt_Amt;
            double.TryParse(txt_EditPay.Text.ToString(),out _Bp);
            foreach (GridViewRow gv in Grd_Earning.Rows)
            {
              //  string Txt_Amt = this.Request.InputStream.Read(text);
                Txt_Amt = (HtmlInputControl)gv.FindControl("Txt_Earamount");
                //TextBox Txt_Amt = (TextBox)gv.FindControl("Txt_Earamount");
                double EarnHead = 0;
                EarnHead = double.Parse(Txt_Amt.Value.ToString());
                GAmt = GAmt + EarnHead;
                EarnPercent = (EarnHead * 100) / _Bp;
                TextBox Txt_Percent = (TextBox)gv.FindControl("Txt_EarnPercent");
                Txt_Percent.Text = EarnPercent.ToString();

            }
            Grd_Earning.Columns[0].Visible = true;
            Grd_Earning.Columns[3].Visible = true;
            Grd_Earning.Columns[5].Visible = true;
            foreach (GridViewRow gv in Grd_Earning.Rows)
            {
                TextBox Txt_Per = (TextBox)gv.FindControl("Txt_EarnPercent");
                double EarnPer = 0;
                EarnPer = double.Parse(Txt_Per.Text.ToString());
                _EarnPer = _EarnPer + _sep + EarnPer;
                _sep = ",";

            }
            Grd_Earning.Columns[0].Visible = false;
            Grd_Earning.Columns[3].Visible = false;
            Grd_Earning.Columns[5].Visible = false;
            HdnEarnPer.Value = _EarnPer;
            double _Gross = GAmt + _Bp;
            txt_Gross.Value = _Gross.ToString();
            _sep = "";
            foreach (GridViewRow gv in Grd_Deduction.Rows)
            {
               // TextBox Txt_Amt = (TextBox)gv.FindControl("Txt_Dedamount");
                Txt_Amt = (HtmlInputControl)gv.FindControl("Txt_Dedamount");
                double DedHead = 0;
                DedHead = double.Parse(Txt_Amt.Value.ToString());
                DAmt = DAmt + DedHead;
                DedPercent = (DedHead * 100) / _Bp;
                TextBox Txt_DedPercent = (TextBox)gv.FindControl("Txt_DedPercent");
                Txt_DedPercent.Text = DedPercent.ToString();
            }
            Grd_Deduction.Columns[0].Visible = true;
            Grd_Deduction.Columns[3].Visible = true;
            Grd_Deduction.Columns[5].Visible = true;
            foreach (GridViewRow gv in Grd_Deduction.Rows)
            {
                TextBox Txt_Per = (TextBox)gv.FindControl("Txt_DedPercent");
                double DedPer = 0;
                DedPer = double.Parse(Txt_Per.Text.ToString());
                _DedPer = _DedPer + _sep + DedPer;
                _sep = ",";

            }
            Grd_Deduction.Columns[0].Visible = false;
            Grd_Deduction.Columns[3].Visible = false;
            Grd_Deduction.Columns[5].Visible = false;
            HdnDedPer.Value = _DedPer;

            double _Total = DAmt;
            Txt_Total.Value = _Total.ToString();

            txt_NetPay.Value = (_Gross - _Total).ToString();
        }

        private void LoadCatToDrpList(DropDownList Drp_PayCat, int _CatId)
        {
            Drp_PayCat.Items.Clear();
            MydataSet = Mypay.FillDrp();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                ListItem li;
                li = new ListItem("Select Category", "0");
                Drp_PayCat.Items.Add(li);
                foreach (DataRow Dr_Cat in MydataSet.Tables[0].Rows)
                {
                    li = new ListItem(Dr_Cat[1].ToString(), Dr_Cat[0].ToString());
                    Drp_PayCat.Items.Add(li);
                }
                Drp_PayCat.SelectedValue = _CatId.ToString();
            }
            else
            {
                ListItem li = new ListItem("No Category Found", "-1");
                Drp_PayCat.Items.Add(li);
            }
        }
        private void LoadEarningGrid(string  _EmpId)
        {
            string _type = "";
            string sep = "";

            DataSet MyEarnEmp = Mypay.GetNewEarningEmp(_EmpId);
            if (MyEarnEmp != null && MyEarnEmp.Tables != null && MyEarnEmp.Tables[0].Rows.Count > 0)
            {
               
                Grd_Earning.Columns[0].Visible = true;
                Grd_Earning.Columns[3].Visible = true;
                Grd_Earning.Columns[5].Visible = true;
                Grd_Earning.DataSource = MyEarnEmp;
                Grd_Earning.DataBind();
                foreach (GridViewRow gv in Grd_Earning.Rows)
                {
 
                    _type = _type + sep + gv.Cells[5].Text.Trim();
                    sep = ",";
  
                }
                HdnEarnType.Value = _type;
                Grd_Earning.Columns[0].Visible = false;
                Grd_Earning.Columns[3].Visible = false;
                Grd_Earning.Columns[5].Visible = false;
            }
            else
            {

                Grd_Earning.DataSource = null;
                Grd_Earning.DataBind();
            }
            ViewState["EarnGrid"]=MyEarnEmp;

        }
        //private void LoadTEarningGrid(int _EmpId)
        //{

        //    DataSet MyEarnEmp = Mypay.GetNewTEarningEmp(_EmpId);
        //    if (MyEarnEmp != null && MyEarnEmp.Tables != null && MyEarnEmp.Tables[0].Rows.Count > 0)
        //    {
        //        Grd_Earning.Columns[0].Visible = true;
        //        Grd_Earning.Columns[3].Visible = true;
        //        Grd_Earning.Columns[5].Visible = true;
        //        Grd_Earning.DataSource = MyEarnEmp;
        //        Grd_Earning.DataBind();
        //        Grd_Earning.Columns[0].Visible = false;
        //        Grd_Earning.Columns[3].Visible = false;
        //        Grd_Earning.Columns[5].Visible = false;
        //    }
        //    else
        //    {
        //        Grd_Earning.DataSource = null;
        //        Grd_Earning.DataBind();
        //    }
        //}
        private void LoadDeductionGrid(string _EmpId)
        {
            string _type = "";
            string sep = "";
            DataSet MyDedEmp = Mypay.GetNewDeductionEmp(_EmpId);
            if (MyDedEmp != null && MyDedEmp.Tables != null && MyDedEmp.Tables[0].Rows.Count > 0)
            {
                Grd_Deduction.Columns[0].Visible = true;
                Grd_Deduction.Columns[3].Visible = true;
                Grd_Deduction.Columns[5].Visible = true;
                Grd_Deduction.DataSource = MyDedEmp;
                Grd_Deduction.DataBind();
                foreach (GridViewRow gv in Grd_Deduction.Rows)
                {

                    _type = _type + sep + gv.Cells[5].Text.Trim();
                    sep = ",";

                }
                HdnDedType.Value = _type;
                Grd_Deduction.Columns[0].Visible = false;
                Grd_Deduction.Columns[3].Visible = false;
                Grd_Deduction.Columns[5].Visible = false;
            }
            else
            {
                Grd_Deduction.DataSource = null;
                Grd_Deduction.DataBind();
            }
            ViewState["DedGrid"] = MyDedEmp;
        }
        //private void LoadTDeductionGrid(int _EmpId)
        //{

        //    DataSet MyDedEmp = Mypay.GetNewTDeductionEmp(_EmpId);
        //    if (MyDedEmp != null && MyDedEmp.Tables != null && MyDedEmp.Tables[0].Rows.Count > 0)
        //    {
        //        Grd_Deduction.Columns[0].Visible = true;
        //        //Grd_Deduction.Columns[3].Visible = true;
        //        Grd_Deduction.Columns[5].Visible = true;
        //        Grd_Deduction.DataSource = MyDedEmp;
        //        Grd_Deduction.DataBind();
        //        Grd_Deduction.Columns[0].Visible = false;
        //        //Grd_Deduction.Columns[3].Visible = false;
        //        Grd_Deduction.Columns[5].Visible = false;
              
        //    }
        //    else
        //    {
        //        Grd_Deduction.DataSource = null;
        //        Grd_Deduction.DataBind();
        //    }
        //}

        public void CalculatEditAmount()
        {
            double GAmt = 0;
            double DAmt = 0;
            double _Bp = 0;
            string _EarnPer = "";
            string _DedPer = "";
            string _sep = "";
            double EarnPercent = 0;
            double DedPercent = 0;
            HtmlInputControl Txt_Amt;            
            double.TryParse(txt_EditPay.Text.ToString(), out _Bp);
            foreach (GridViewRow gv in Grd_Earning.Rows)
            {
                //  string Txt_Amt = this.Request.InputStream.Read(text);
                Txt_Amt = (HtmlInputControl)gv.FindControl("Txt_Earamount");
                //TextBox Txt_Amt = (TextBox)gv.FindControl("Txt_Earamount");
                double EarnHead = 0;
                EarnHead = double.Parse(Txt_Amt.Value.ToString());
                GAmt = GAmt + EarnHead;
               
                    EarnPercent = (EarnHead * 100) / _Bp;
                    TextBox Txt_Percent = (TextBox)gv.FindControl("Txt_EarnPercent");
                    Txt_Percent.Text = EarnPercent.ToString();
                    int per = 0;
                    if (Txt_Percent.Text == "NaN")
                    {
                        Txt_Percent.Text = per.ToString();
                    }
                

            }
            Grd_Earning.Columns[0].Visible = true;
            Grd_Earning.Columns[3].Visible = true;
            Grd_Earning.Columns[5].Visible = true;
            foreach (GridViewRow gv in Grd_Earning.Rows)
            {
                TextBox Txt_Per = (TextBox)gv.FindControl("Txt_EarnPercent");
                double EarnPer = 0;
                EarnPer = double.Parse(Txt_Per.Text.ToString());
                _EarnPer = _EarnPer + _sep + EarnPer;
                _sep = ",";

            }
            Grd_Earning.Columns[0].Visible = false;
            Grd_Earning.Columns[3].Visible = false;
            Grd_Earning.Columns[5].Visible = false;
            HdnEarnPer.Value = _EarnPer;
            double Advance = 0.0;
            if (Hdn_Advance.Value != "")
            {
                Advance = double.Parse(Hdn_Advance.Value);
            }
            else if(Txt_previousAdvamount.Text!="")
            {

               Advance = double.Parse(Txt_previousAdvamount.Text);
            }

            double _Gross = GAmt + _Bp + Advance;
            _Gross = _Gross - advdedction;
            txt_Gross.Value = _Gross.ToString();     
            _sep = "";
            foreach (GridViewRow gv in Grd_Deduction.Rows)
            {
                // TextBox Txt_Amt = (TextBox)gv.FindControl("Txt_Dedamount");
                Txt_Amt = (HtmlInputControl)gv.FindControl("Txt_Dedamount");
                double DedHead = 0;
                DedHead = double.Parse(Txt_Amt.Value.ToString());
                DAmt = DAmt + DedHead;
                DedPercent = (DedHead * 100) / _Bp;
                TextBox Txt_DedPercent = (TextBox)gv.FindControl("Txt_DedPercent");
                Txt_DedPercent.Text = DedPercent.ToString();
                int dedper = 0;
                if (Txt_DedPercent.Text == "NaN")
                {
                    Txt_DedPercent.Text = dedper.ToString();
                }
            }
            Grd_Deduction.Columns[0].Visible = true;
            Grd_Deduction.Columns[3].Visible = true;
            Grd_Deduction.Columns[5].Visible = true;
            foreach (GridViewRow gv in Grd_Deduction.Rows)
            {
                TextBox Txt_Per = (TextBox)gv.FindControl("Txt_DedPercent");
                double DedPer = 0;
                DedPer = double.Parse(Txt_Per.Text.ToString());
                _DedPer = _DedPer + _sep + DedPer;
                _sep = ",";

            }
            Grd_Deduction.Columns[0].Visible = false;
            Grd_Deduction.Columns[3].Visible = false;
            Grd_Deduction.Columns[5].Visible = false;
            HdnDedPer.Value = _DedPer;
            

            double _Total = DAmt;
            double _gamnt = GAmt;
            Hdn_Dedamnt.Value=_Total.ToString();
            Hdn_earnamnt.Value = _gamnt.ToString();
            txt_Gross.Value = (GAmt + _Bp + Advance).ToString();
            Txt_Total.Value = _Total.ToString();                
            txt_NetPay.Value = (_Gross - _Total).ToString();
        }

        protected void Btn_Update_Click(object sender, EventArgs e)
        {
            
            Hdn_Dedamnt.Value = "";
            Hdn_earnamnt.Value = "";
            if(int.Parse(txt_TotWked.Text)>int.Parse(txt_TotWkng.Text))
            {
                WC_MessageBox.ShowMssage("Worked days must be less than working days");
            }

            else if (double.Parse(txt_NetPay.Value) >= 0)
            {
                CalculatEditAmount();

                if (int.Parse(txt_EditPay.Text) == 0 && (int.Parse(Hdn_Dedamnt.Value) > 0 || int.Parse(Hdn_earnamnt.Value) > 0))
                {
                    WC_MessageBox.ShowMssage("Cannot add earnings or deduction while Basic pay is zero");
                }
                else
                {
                    //int Year = System.DateTime.Now.Year;
                    int Year = int.Parse(Request.QueryString["Year"].ToString());

                    string EmplId = Request.QueryString["EmpId"].ToString();
                    int EGridId = int.Parse(Request.QueryString["id"].ToString());
                    int MonthId = int.Parse(Request.QueryString["ChkId"].ToString());
                    int Cat = int.Parse(Request.QueryString["Cat"].ToString());
                    string _Surname = Txt_EditName.Text;
                    //int _CatId = int.Parse(Drp_PayCat.SelectedValue.ToString());
                    string _Pan = Txt_Pan.Text.ToString();
                    double _Bank = double.Parse(txt_EditBank.Text.ToString());
                    double _BasicPay = double.Parse(txt_EditPay.Text.ToString());
                    double _gross = double.Parse(txt_Gross.Value.ToString());
                    double _netpay = double.Parse(txt_NetPay.Value.ToString());
                    double _total = double.Parse(Txt_Total.Value.ToString());
                    string Comment = TxtComment.Text.Trim();
                    // double totalearn =double.Parse(Hdn_totalEarn.Value);
                    int Head = 0;
                    HtmlInputControl Txt_Amt;
                    Mypay.CreateTansationDb();
                    try
                    {
                        if (MonthId == 0)
                        {
                            int _CatId = int.Parse(Drp_PayCat.SelectedValue.ToString());
                            Mypay.UpdateEmp(EGridId, _Surname, _CatId, _Pan, _Bank, _gross, _netpay, _total, _BasicPay, Comment);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Employee Salary Configuration", "Salary details of employee " + _Surname + " is cofigured", 1, Mypay.m_TransationDb);
                            Mypay.DeleteCatHead(EmplId);
                        }
                        else
                        {
                            int _TotWorking = int.Parse(txt_TotWkng.Text.ToString());
                            int _TotWorked = int.Parse(txt_TotWked.Text.ToString());

                            if (Mypay.NotEmpPresent(EmplId, MonthId, Year))
                            {
                                double advsal = Mypay.GetAdvanceSalary(EmplId.ToString());
                                // _netpay = _netpay - advsal;


                                Mypay.AddMonthEmpSal(Year, MonthId, _BasicPay, EmplId, _gross, _total, _netpay, Cat, _TotWorking, _TotWorked, Comment, advsal);
                                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Monthly Salary Configuration", "Monthly salary details of employee " + _Surname + " is cofigured", 1, Mypay.m_TransationDb);
                                Mypay.DeleteMonthhead(EmplId, MonthId, Year);



                            }
                            else
                            {

                                Mypay.UpdateMonthEmp(Year, MonthId, _BasicPay, EmplId, _gross, _total, _netpay, _TotWorking, _TotWorked, Comment);


                                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Monthly Salary Configuration", "Updated the monthly salary cofiguration details of employee " + _Surname + "", 1, Mypay.m_TransationDb);
                                Mypay.DeleteMonthhead(EmplId, MonthId, Year);
                            }
                            //double AdvDeduction = 0, advnceamnt = 0;
                            //Mypay.GetAdvanceDetails(EmplId, MonthId, Year, out AdvDeduction, out advnceamnt);
                            //if (advdedction > 0)
                            //{
                            //    Mypay.UpdateAdvDeduction(advdedction, EmplId, MonthId, Year);
                            //}

                            if (Hdn_previousadv.Value != "" && Hdn_previouspercentage.Value != "")
                            {

                                Txt_previousAdvamount.Visible = false;
                                Lbl_PreviousAdvamount.Visible = false;
                                Lbl_advdeduction.Visible = false;
                                Txt_advsaldeduction.Visible = false;

                                double Amount = double.Parse(Hdn_previousadv.Value.ToString());
                                double Percent = double.Parse(Hdn_previouspercentage.Value.ToString());
                                Mypay.AddAdvanceToMonthlyConfig(MonthId, EmplId, Percent, Amount);
                                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Add advance amount", "An advance amount of " + Amount + " is added to employee " + _Surname + "", 1, Mypay.m_TransationDb);
                            }


                        }
                        Grd_Earning.Columns[0].Visible = true;
                        Grd_Earning.Columns[3].Visible = true;
                        Grd_Earning.Columns[5].Visible = true;

                        foreach (GridViewRow gv in Grd_Earning.Rows)
                        {
                            Txt_Amt = (HtmlInputControl)gv.FindControl("Txt_Earamount");

                            double Amt = 0;
                            Head = int.Parse(gv.Cells[0].Text);
                            Amt = double.Parse(Txt_Amt.Value.ToString());
                            if (MonthId == 0)
                            {
                                int _CatId = int.Parse(Drp_PayCat.SelectedValue.ToString());
                                Mypay.AddNewHeadMap(Head, EmplId, _CatId, Amt);
                            }
                            else
                            {

                                Mypay.AddNewMonthHeadMap(MonthId, EmplId, Head, Cat, Amt, Year);
                            }

                        }
                        Grd_Earning.Columns[0].Visible = false;
                        Grd_Earning.Columns[3].Visible = false;
                        Grd_Earning.Columns[5].Visible = false;

                        Grd_Deduction.Columns[0].Visible = true;
                        Grd_Deduction.Columns[3].Visible = true;
                        Grd_Deduction.Columns[5].Visible = true;

                        foreach (GridViewRow gv in Grd_Deduction.Rows)
                        {
                            Txt_Amt = (HtmlInputControl)gv.FindControl("Txt_Dedamount");

                            double Amt = 0;
                            Head = int.Parse(gv.Cells[0].Text);
                            Amt = double.Parse(Txt_Amt.Value.ToString());
                            if (MonthId == 0)
                            {
                                int _CatId = int.Parse(Drp_PayCat.SelectedValue.ToString());
                                Mypay.AddNewHeadMap(Head, EmplId, _CatId, Amt);
                            }
                            else
                            {
                                Mypay.AddNewMonthHeadMap(MonthId, EmplId, Head, Cat, Amt, Year);
                            }
                        }
                        Grd_Deduction.Columns[0].Visible = false;
                        Grd_Deduction.Columns[3].Visible = false;
                        Grd_Deduction.Columns[5].Visible = false;

                        Mypay.EndSucessTansationDb();
                        OdbcDataReader Myadvreader = null;
                        Myadvreader = Mypay.GetAdvancesalOfMonth(MonthId, EmplId);
                        if (Myadvreader.HasRows)
                        {
                            double advamount = double.Parse(Myadvreader.GetValue(0).ToString());
                            double advded = double.Parse(Myadvreader.GetValue(2).ToString());
                            if (advamount != 0)
                            {
                                Txt_previousAdvamount.Visible = true;
                                Lbl_PreviousAdvamount.Visible = true;
                                Txt_previousAdvamount.Text = advamount.ToString();
                            }
                            else
                            {
                                Txt_previousAdvamount.Visible = false;
                                Lbl_PreviousAdvamount.Visible = false;
                            }
                            if (advded > 0)
                            {

                                Txt_advsaldeduction.Visible = true;
                                Lbl_advdeduction.Visible = true;
                                Txt_advsaldeduction.Text = advamount.ToString();
                                Hdn_advaded.Value = advamount.ToString();
                            }
                            else
                            {
                                Txt_advsaldeduction.Visible = false;
                                Lbl_advdeduction.Visible = false;
                                Hdn_advaded.Value = "";
                            }


                        }
                        else
                        {
                            Txt_previousAdvamount.Visible = false;
                            Lbl_PreviousAdvamount.Visible = false;
                            Txt_advsaldeduction.Visible = false;
                            Lbl_advdeduction.Visible = false;
                            Hdn_advaded.Value = "";
                        }
                        WC_MessageBox.ShowMssage("Successfully Saved");
                        //double advamount = 0.0;
                        //advamount = Mypay.GetAdvancesalOfThisMonth(MonthId, EmplId);
                        //if (advamount != 0)
                        //{
                        //    Txt_previousAdvamount.Visible = true;
                        //    Lbl_PreviousAdvamount.Visible = true;
                        //    Txt_previousAdvamount.Text = advamount.ToString();
                        //}
                        //else
                        //{
                        //    Txt_previousAdvamount.Visible = false;
                        //    Lbl_PreviousAdvamount.Visible = false;
                        //}

                    }
                    catch
                    {
                        Mypay.EndFailTansationDb();
                        WC_MessageBox.ShowMssage("Updation Failed");
                    }

                    //ClientScript.RegisterClientScriptBlock(typeof(Page), "AnyScript", "<script>PageRelorad();</script>");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScript", "PageRelorad();", true);
                }
            }
            else
            {
                WC_MessageBox.ShowMssage("Net pay cannot be less than zero");
            }
            

        }

        

        protected void Btn_AddEarning_Click(object sender, EventArgs e)
        {
            LoadEarnToDrpList(Drp_AddEarn);
            MPE_AddEarn_popup.Show();
        }
        protected void Btn_AddDeduction_Click(object sender, EventArgs e)
        {
            LoadDedToDrpList(Drp_AddDed);
            MPE_AddDed_popup.Show();
        }

        private void LoadEarnToDrpList(DropDownList Drp_AddEarn)
        {
            Drp_AddEarn.Items.Clear();
            MydataSet = Mypay.FillEarnDrp();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                ListItem li;
                li = new ListItem("Select Head", "0");
                Drp_AddEarn.Items.Add(li);
                foreach (DataRow Dr_Cat in MydataSet.Tables[0].Rows)
                {
                    li = new ListItem(Dr_Cat[1].ToString(), Dr_Cat[0].ToString());
                    Drp_AddEarn.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Heads Found", "-1");
                Drp_AddEarn.Items.Add(li);
            }
        }
        private void LoadDedToDrpList(DropDownList Drp_AddDed)
        {
            Drp_AddDed.Items.Clear();
            MydataSet = Mypay.FillDedDrp();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                ListItem li;
                li = new ListItem("Select Head", "0");
                Drp_AddDed.Items.Add(li);
                foreach (DataRow Dr_Cat in MydataSet.Tables[0].Rows)
                {
                    li = new ListItem(Dr_Cat[1].ToString(), Dr_Cat[0].ToString());
                    Drp_AddDed.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Heads Found", "-1");
                Drp_AddDed.Items.Add(li);
            }
        }

        

        protected void Btn_Add_Click(object sender, EventArgs e)
        {
             int EId = int.Parse(Request.QueryString["id"].ToString());
            string  EmplId = Request.QueryString["EmpId"].ToString();
            int MonthId = int.Parse(Request.QueryString["ChkId"].ToString());
            int _NewEarnId = int.Parse(Drp_AddEarn.SelectedValue.ToString());
            string _type = "";
            string sep = "";
            if (MonthId == 0)
            {
                int _CatId = int.Parse(Drp_PayCat.SelectedValue.ToString());

                if (_CatId == 0)
                {
                    WC_MessageBox.ShowMssage("Please Select Payroll Type");
                }
                else
                {
                    if (NotHeadPresent(_NewEarnId))
                    {

                        MydataSet = Mypay.AddDedToGrid(_NewEarnId);
                        DataSet HeadDataSet = (DataSet)ViewState["EarnGrid"];
                        DataTable dt;
                        DataRow dr;

                        foreach (DataRow row1 in MydataSet.Tables[0].Rows)
                        {
                            dr = HeadDataSet.Tables[0].NewRow();
                            dr["HeadId"] = row1[0].ToString();
                            dr["HeadName"] = row1[1].ToString();
                            dr["HeadAmount"] = row1[2].ToString();
                            HeadDataSet.Tables[0].Rows.Add(dr);
                        }
                        Grd_Earning.Columns[0].Visible = true;
                        Grd_Earning.Columns[3].Visible = true;
                        Grd_Earning.Columns[5].Visible = true;
                        Grd_Earning.DataSource = HeadDataSet;
                        Grd_Earning.DataBind();
                        foreach (GridViewRow gv in Grd_Earning.Rows)
                        {

                            _type = _type + sep + gv.Cells[5].Text.Trim();
                            sep = ",";

                        }
                        HdnEarnType.Value = _type;
                        ViewState["EarnGrid"] = HeadDataSet;
                        Grd_Earning.Columns[0].Visible = false;
                        Grd_Earning.Columns[3].Visible = false;
                        Grd_Earning.Columns[5].Visible = false;
                        
                    }
                    else
                    {
                        WC_MessageBox.ShowMssage("Head Already Present");

                    }

                }
            }
            else
            {
                if (NotHeadPresent(_NewEarnId))
                {

                    MydataSet = Mypay.AddDedToGrid(_NewEarnId);
                    DataSet HeadDataSet = (DataSet)ViewState["EarnGrid"];
                    DataTable dt;
                    DataRow dr;

                    foreach (DataRow row1 in MydataSet.Tables[0].Rows)
                    {
                        dr = HeadDataSet.Tables[0].NewRow();
                        dr["HeadId"] = row1[0].ToString();
                        dr["HeadName"] = row1[1].ToString();
                        dr["HeadAmount"] = row1[2].ToString();
                        HeadDataSet.Tables[0].Rows.Add(dr);
                    }
                    Grd_Earning.Columns[0].Visible = true;
                    Grd_Earning.Columns[3].Visible = true;
                    Grd_Earning.Columns[5].Visible = true;
                    Grd_Earning.DataSource = HeadDataSet;
                    Grd_Earning.DataBind();
                    foreach (GridViewRow gv in Grd_Earning.Rows)
                    {

                        _type = _type + sep + gv.Cells[5].Text.Trim();
                        sep = ",";

                    }
                    HdnEarnType.Value = _type;
                    ViewState["EarnGrid"] = HeadDataSet;
                    Grd_Earning.Columns[0].Visible = false;
                    Grd_Earning.Columns[3].Visible = false;
                    Grd_Earning.Columns[5].Visible = false;
                    Drp_PayCat.CssClass = "noscreen";
                }
                else
                {
                    WC_MessageBox.ShowMssage("Head Already Present");

                }
            }


        }

        private bool NotHeadPresent(int _NewEarnId)
        {
            bool _head = true;
            int Head = 0;
            foreach (GridViewRow gv in Grd_Earning.Rows)
            {
                Head = int.Parse(gv.Cells[0].Text);
                if (Head == _NewEarnId)
                {
                    _head = false;
                }
                
            }
            return _head;
        }
        protected void Btn_AddD_Click(object sender, EventArgs e)
        {
            int EId = int.Parse(Request.QueryString["id"].ToString());
            string  EmplId = Request.QueryString["EmpId"].ToString();
            int MonthId = int.Parse(Request.QueryString["ChkId"].ToString());
            int _NewDedId = int.Parse(Drp_AddDed.SelectedValue.ToString());
            string _type = "";
            string sep = "";
            if (MonthId == 0)
            {
                int _CatDedId = int.Parse(Drp_PayCat.SelectedValue.ToString());

                if (_CatDedId == 0)
                {
                    WC_MessageBox.ShowMssage("Please Select Payroll Type");
                }
                else
                {
                    if (NotDedHeadPresent(_NewDedId))
                    {
                        MydataSet = Mypay.AddDedToGrid(_NewDedId);
                        DataSet DedDataSet = (DataSet)ViewState["DedGrid"];
                        DataTable dt;
                        DataRow dr;

                        foreach (DataRow row1 in MydataSet.Tables[0].Rows)
                        {
                            dr = DedDataSet.Tables[0].NewRow();
                            dr["HeadId"] = row1[0].ToString();
                            dr["HeadName"] = row1[1].ToString();
                            dr["HeadAmount"] = row1[2].ToString();
                            DedDataSet.Tables[0].Rows.Add(dr);
                        }
                        Grd_Deduction.Columns[0].Visible = true;
                        Grd_Deduction.Columns[3].Visible = true;
                        Grd_Deduction.Columns[5].Visible = true;
                        Grd_Deduction.DataSource = DedDataSet;
                        Grd_Deduction.DataBind();
                        foreach (GridViewRow gv in Grd_Deduction.Rows)
                        {

                            _type = _type + sep + gv.Cells[5].Text.Trim();
                            sep = ",";

                        }
                        HdnDedType.Value = _type;
                        ViewState["DedGrid"] = DedDataSet;
                        Grd_Deduction.Columns[0].Visible = false;
                        Grd_Deduction.Columns[3].Visible = false;
                        Grd_Deduction.Columns[5].Visible = false;
                    }
                    else
                    {
                        WC_MessageBox.ShowMssage("Head Already Present");

                    }
                }
            }
            else
            {
                if (NotDedHeadPresent(_NewDedId))
                {
                    MydataSet = Mypay.AddDedToGrid(_NewDedId);
                    DataSet DedDataSet = (DataSet)ViewState["DedGrid"];
                    DataTable dt;
                    DataRow dr;

                    foreach (DataRow row1 in MydataSet.Tables[0].Rows)
                    {
                        dr = DedDataSet.Tables[0].NewRow();
                        dr["HeadId"] = row1[0].ToString();
                        dr["HeadName"] = row1[1].ToString();
                        dr["HeadAmount"] = row1[2].ToString();
                        DedDataSet.Tables[0].Rows.Add(dr);
                    }
                    Grd_Deduction.Columns[0].Visible = true;
                    Grd_Deduction.Columns[3].Visible = true;
                    Grd_Deduction.Columns[5].Visible = true;
                    Grd_Deduction.DataSource = DedDataSet;
                    Grd_Deduction.DataBind();
                    foreach (GridViewRow gv in Grd_Deduction.Rows)
                    {

                        _type = _type + sep + gv.Cells[5].Text.Trim();
                        sep = ",";

                    }
                    HdnDedType.Value = _type;
                    ViewState["DedGrid"] = DedDataSet;
                    Grd_Deduction.Columns[0].Visible = false;
                    Grd_Deduction.Columns[3].Visible = false;
                    Grd_Deduction.Columns[5].Visible = false;
                }
                else
                {
                    WC_MessageBox.ShowMssage("Head Already Present");

                }

            }


        }

        private bool NotDedHeadPresent(int _NewDedId)
        {
            bool _head = true;
            int Head = 0;
            foreach (GridViewRow gv in Grd_Deduction.Rows)
            {
                Head = int.Parse(gv.Cells[0].Text);
                if (Head == _NewDedId)
                {
                    _head = false;
                }

            }
            return _head;
        }

        private void LoadNewDeductionGrid(string  EmplId)
        {
            string _type = "";
            string sep = "";
            string  _newDedId = EmplId;
            DataSet MyDedEmp = Mypay.GetNewDeductionEmp(_newDedId);
            if (MyDedEmp != null && MyDedEmp.Tables != null && MyDedEmp.Tables[0].Rows.Count > 0)
            {
                Grd_Deduction.Columns[0].Visible = true;
                Grd_Deduction.Columns[3].Visible = true;
                Grd_Deduction.Columns[5].Visible = true;
                Grd_Deduction.DataSource = MyDedEmp;
                Grd_Deduction.DataBind();
                foreach (GridViewRow gv in Grd_Deduction.Rows)
                {

                    _type = _type + sep + gv.Cells[5].Text.Trim();
                    sep = ",";

                }
                HdnDedType.Value = _type;
                ViewState["DedGrid"] = MyDedEmp;
                Grd_Deduction.Columns[0].Visible = false;
                Grd_Deduction.Columns[3].Visible = false;
                Grd_Deduction.Columns[5].Visible = false;
           

            }
            else
            {
                Grd_Deduction.DataSource = null;
                Grd_Deduction.DataBind();
            }
        }

        private void LoadNewEarningGrid(string _EId)
        {
            string _type = "";
            string sep = "";
            string  _newEarnId = _EId;
            DataSet MyEarnEmp = Mypay.GetNewEarningEmp(_newEarnId);
            if (MyEarnEmp != null && MyEarnEmp.Tables != null && MyEarnEmp.Tables[0].Rows.Count > 0)
            {
               
                Grd_Earning.Columns[0].Visible = true;
                Grd_Earning.Columns[3].Visible = true;
                Grd_Earning.Columns[5].Visible = true;
                Grd_Earning.DataSource = MyEarnEmp;
                Grd_Earning.DataBind();
                foreach (GridViewRow gv in Grd_Earning.Rows)
                {

                    _type = _type + sep + gv.Cells[5].Text.Trim();
                    sep = ",";

                }
                HdnEarnType.Value = _type;
                ViewState["EarnGrid"] = MyEarnEmp;
                Grd_Earning.Columns[0].Visible = false;
                Grd_Earning.Columns[3].Visible = false;
                Grd_Earning.Columns[5].Visible = false;

            }
            else
            {
                Grd_Earning.DataSource = null;
                Grd_Earning.DataBind();
            }
           
        }

        protected void Grd_Deduction_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton l = (LinkButton)e.Row.FindControl("LinkButton1");
                l.Attributes.Add("onclick", "javascript:return " +
                     "confirm('Are you sure you want to delete this Head? ')");
            }
        }
        protected void Grd_Deduction_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            OdbcDataReader MyPayReader = null;
            string  EmplId = Request.QueryString["EmpId"].ToString();
            int EId = int.Parse(Request.QueryString["id"].ToString());
            int Cat = int.Parse(Request.QueryString["Cat"].ToString());
            string _type = "";
            string sep = "";
            HtmlInputControl Txt_Amt;
            int _HeadID = int.Parse(Grd_Deduction.Rows[e.RowIndex].Cells[0].Text);
            //DataSet DedDataSet = (DataSet)ViewState["DedGrid"];
            MyPayReader = Mypay.CheckFixed(_HeadID);
            if (MyPayReader.HasRows)
            {
                while (MyPayReader.Read())
                {
                    //if (MyPayReader.GetValue(0).ToString() != "1")
                    //{
                        Grd_Deduction.Columns[0].Visible = true;
                        Grd_Deduction.Columns[3].Visible = true;
                        Grd_Deduction.Columns[5].Visible = true;
                        DataSet DedDataSet = new DataSet();
                        DedDataSet.Tables.Add(new DataTable("HeadDt"));
                        DataTable dt;
                        DataRow dr;
                        dt = DedDataSet.Tables["HeadDt"];
                        dt.Columns.Add("HeadId");
                        dt.Columns.Add("HeadName");
                        dt.Columns.Add("HeadAmount");
                        dt.Columns.Add("DecreaseType");
                        foreach (GridViewRow gv in Grd_Deduction.Rows)
                        {
                            if (gv.Cells[0].Text != _HeadID.ToString())
                            {
                                Txt_Amt = (HtmlInputControl)gv.FindControl("Txt_Dedamount");

                                dr = DedDataSet.Tables[0].NewRow();
                                dr["HeadId"] = gv.Cells[0].Text;
                                dr["HeadName"] = gv.Cells[1].Text;
                                dr["HeadAmount"] = Txt_Amt.Value;
                                dr["DecreaseType"] = gv.Cells[5].Text;
                                DedDataSet.Tables["HeadDt"].Rows.Add(dr);
                            }

                        }
                        Grd_Deduction.DataSource = DedDataSet;
                        Grd_Deduction.DataBind();
                        foreach (GridViewRow gv in Grd_Deduction.Rows)
                        {

                            _type = _type + sep + gv.Cells[5].Text.Trim();
                            sep = ",";

                        }
                        HdnDedType.Value = _type;
                        Grd_Deduction.Columns[0].Visible = false;
                        Grd_Deduction.Columns[3].Visible = false;
                        Grd_Deduction.Columns[5].Visible = false;
                        ViewState["DedGrid"] = DedDataSet;

                    //}
                    //else
                    //{
                    //    WC_MessageBox.ShowMssage("This head Cannot be Deleted");
                    //}
                }
            }


            CalculatEditAmount();
            
           
        }
        protected void Grd_Earning_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton l = (LinkButton)e.Row.FindControl("LinkButton1");
                l.Attributes.Add("onclick", "javascript:return " +
                     "confirm('Are you sure you want to delete this Head? ')");
            }
        }
        protected void Grd_Earning_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            OdbcDataReader MyPayReader = null;
            string  EmplId = Request.QueryString["EmpId"].ToString();
            int EId = int.Parse(Request.QueryString["id"].ToString());
            int Cat = int.Parse(Request.QueryString["Cat"].ToString());
            int _HeadID = int.Parse(Grd_Earning.Rows[e.RowIndex].Cells[0].Text);
            string _type = "";
            string sep = "";
            HtmlInputControl Txt_Amt;
            //DataSet DedDataSet = (DataSet)ViewState["EarnGrid"];
            MyPayReader = Mypay.CheckFixed(_HeadID);
            if (MyPayReader.HasRows)
            {
                while (MyPayReader.Read())
                {
                    if (MyPayReader.GetValue(0).ToString() != "1")
                    {
                        Grd_Earning.Columns[0].Visible = true;
                        Grd_Earning.Columns[3].Visible = true;
                        Grd_Earning.Columns[5].Visible = true;
                        DataSet EarnDataSet = new DataSet();
                        EarnDataSet.Tables.Add(new DataTable("HeadDt"));
                        DataTable dt;
                        DataRow dr;
                        dt = EarnDataSet.Tables["HeadDt"];
                        dt.Columns.Add("HeadId");
                        dt.Columns.Add("HeadName");
                        dt.Columns.Add("HeadAmount");
                        dt.Columns.Add("DecreaseType");
                        foreach (GridViewRow gv in Grd_Earning.Rows)
                        {
                            if (gv.Cells[0].Text != _HeadID.ToString())
                            {
                                Txt_Amt = (HtmlInputControl)gv.FindControl("Txt_Earamount");

                                dr = EarnDataSet.Tables[0].NewRow();
                                dr["HeadId"] = gv.Cells[0].Text;
                                dr["HeadName"] = gv.Cells[1].Text;
                                dr["HeadAmount"] = Txt_Amt.Value;
                                dr["DecreaseType"] = gv.Cells[5].Text;
                                EarnDataSet.Tables["HeadDt"].Rows.Add(dr);
                            }

                        }
                        Grd_Earning.DataSource = EarnDataSet;
                        Grd_Earning.DataBind();
                        foreach (GridViewRow gv in Grd_Earning.Rows)
                        {

                            _type = _type + sep + gv.Cells[5].Text.Trim();
                            sep = ",";

                        }
                        HdnEarnType.Value = _type;
                        Grd_Earning.Columns[0].Visible = false;
                        Grd_Earning.Columns[3].Visible = false;
                        Grd_Earning.Columns[5].Visible = false;
                        ViewState["EarnGrid"] = EarnDataSet;
                    }
                    else
                    {
                        WC_MessageBox.ShowMssage("This head Cannot be Deleted");
                    }
                }

            }
            CalculatEditAmount();
           //Calculations();
        }

        public void clear()
        {
            txt_Gross.Value = "";
            txt_NetPay.Value = "";
            Txt_Total.Value = "";
        }

        protected void Drp_PayCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _type = "";
            string sep = "";
            int _PayCatId = int.Parse(Drp_PayCat.SelectedValue.ToString());
            string  EmplId = Request.QueryString["EmpId"].ToString();
            Mypay.CreateTansationDb();

            try
            {
                string sql = "select tblpay_category.BasicPay from tblpay_category where tblpay_category.Id = " + _PayCatId + "";
                m_MyReader = Mypay.m_TransationDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    while (m_MyReader.Read())
                    {
                        int _BasicPay = int.Parse(m_MyReader.GetValue(0).ToString());
                        txt_EditPay.Text = _BasicPay.ToString();
                    }

                }

                DataSet HeadDataSet = Mypay.GetEarnHeadOfCatId(_PayCatId);

                if (HeadDataSet != null && HeadDataSet.Tables != null && HeadDataSet.Tables[0].Rows.Count > 0)
                {
                    Grd_Earning.Columns[0].Visible = true;
                    Grd_Earning.Columns[3].Visible = true;
                    Grd_Earning.Columns[5].Visible = true;
                    Grd_Earning.DataSource = HeadDataSet;
                    Grd_Earning.DataBind();
                    foreach (GridViewRow gv in Grd_Earning.Rows)
                    {

                        _type = _type + sep + gv.Cells[5].Text.Trim();
                        sep = ",";

                    }
                    HdnEarnType.Value = _type;
                    ViewState["EarnGrid"] = HeadDataSet;
                    Grd_Earning.Columns[0].Visible = false;
                    Grd_Earning.Columns[3].Visible = false;
                    Grd_Earning.Columns[5].Visible = false;


                }
                else
                {
                    Grd_Earning.DataSource = null;
                    Grd_Earning.DataBind();
                }
                DataSet NewHeadDataSet = Mypay.GetDedHeadOfCatId(_PayCatId);
                if (NewHeadDataSet != null && NewHeadDataSet.Tables != null && NewHeadDataSet.Tables[0].Rows.Count > 0)
            {
                Grd_Deduction.Columns[0].Visible = true;
                Grd_Deduction.Columns[3].Visible = true;
                Grd_Deduction.Columns[5].Visible = true;
                Grd_Deduction.DataSource = NewHeadDataSet;
                Grd_Deduction.DataBind();
                foreach (GridViewRow gv in Grd_Deduction.Rows)
                {

                    _type = _type + sep + gv.Cells[5].Text.Trim();
                    sep = ",";

                }
                HdnDedType.Value = _type;
                ViewState["DedGrid"] = NewHeadDataSet;
                Grd_Deduction.Columns[0].Visible = false;
                Grd_Deduction.Columns[3].Visible = false;
                Grd_Deduction.Columns[5].Visible = false;
               

            }
            else
            {
                Grd_Deduction.DataSource = null;
                Grd_Deduction.DataBind();
            }

                Mypay.EndSucessTansationDb();
            }
            catch
            {
                Mypay.EndFailTansationDb();
            }
            CalculatEditAmount();
           

            //int _PayCatId = int.Parse(Drp_PayCat.SelectedValue.ToString());
            //int EmplId = int.Parse(Request.QueryString["EmpId"].ToString());
            //Mypay.CreateTansationDb();

            //try
            //{
            //    string sql = "select tblpay_category.BasicPay from tblpay_category where tblpay_category.Id = " + _PayCatId + "";
            //    m_MyReader1= Mypay.m_TransationDb.ExecuteQuery(sql);
            //    if (m_MyReader1.HasRows)
            //    {
            //        while (m_MyReader1.Read())
            //        {
            //            int _BasicPay = int.Parse(m_MyReader1.GetValue(0).ToString());
            //            txt_EditPay.Text = _BasicPay.ToString();
            //        }

            //    }
            //    string sql1 = "delete from tblpay_employeeheadmap where tblpay_employeeheadmap.EmployeeId=" + EmplId + "";
            //    Mypay.m_TransationDb.ExecuteQuery(sql1);
            //    string sql2 = "select tblpay_headcategorymap.HeadId, tblpay_head.Amount from tblpay_headcategorymap inner join tblpay_head on tblpay_head.Id = tblpay_headcategorymap.HeadId where tblpay_headcategorymap.CategoryId=" + _PayCatId + "";
            //    m_MyReader = Mypay.m_TransationDb.ExecuteQuery(sql2);
            //    if (m_MyReader.HasRows)
            //    {
            //        while (m_MyReader.Read())
            //        {
            //            int _PayHeadId = int.Parse(m_MyReader.GetValue(0).ToString());
            //            int _amount = int.Parse(m_MyReader.GetValue(1).ToString());
            //            string sql3 = " insert into tblpay_employeeheadmap(EmployeeId,CategoryId,HeadId,HeadAmount) values (" + EmplId + ", " + _PayCatId + ", " + _PayHeadId + "," + _amount + ")";
            //            Mypay.m_TransationDb.ExecuteQuery(sql3);
            //        }
            //    }
            //    Mypay.EndSucessTansationDb();
            //}
            //catch 
            //{
            //    Mypay.EndFailTansationDb();
            //}
            //LoadEarningGrid(EmplId);
            //LoadDeductionGrid(EmplId);
            //Calculations();
        }

        protected void Btn_AddSalAdv_Click(object sender, EventArgs e)
        {
            double advded = 0.0;
            string EmplId = Request.QueryString["EmpId"].ToString();
            double Amount = 0;
            double Percent = 0;
            if (Txt_advsaldeduction.Text != "")
            {
                WC_MessageBox.ShowMssage("You can get the advance, only after  paying the entire previous advance amount");
                Txt_advsaldeduction.Visible = true;
                Lbl_advdeduction.Visible = true;
                Hdn_previousadv.Value = "";
                Hdn_previouspercentage.Value = "";

            }

            else if (Txt_Amount.Text == "" || int.Parse(Txt_Amount.Text) == 0)
            {
                WC_MessageBox.ShowMssage("Enter amount");
                Hdn_previousadv.Value = "";
                Hdn_previouspercentage.Value = "";
                MPE_SalAdv_popup.Show();

            }
            else if (Txt_Per.Text == "" || int.Parse(Txt_Per.Text) == 0)
            {
                WC_MessageBox.ShowMssage("Enter percentage");
                Hdn_previousadv.Value = "";
                Hdn_previouspercentage.Value = "";
                MPE_SalAdv_popup.Show();
            }
            else if (int.Parse(Txt_Per.Text) >100)
            {
                WC_MessageBox.ShowMssage("Percentage cannot be greater than zero");
                Hdn_previousadv.Value = "";
                Hdn_previouspercentage.Value = "";
                MPE_SalAdv_popup.Show();
            }
            else
            {

                Amount = double.Parse(Txt_Amount.Text.ToString());
                Percent = double.Parse(Txt_Per.Text.ToString());
                Hdn_previousadv.Value = Txt_Amount.Text;
                Hdn_previouspercentage.Value = Txt_Per.Text;

                if (Txt_previousAdvamount.Text != "")
                {
                    Txt_previousAdvamount.Visible = true;
                    Lbl_PreviousAdvamount.Visible = true;
                }
                else
                {

                    Txt_previousAdvamount.Visible = false;
                    Lbl_PreviousAdvamount.Visible = false;
                }

                Mypay.AddAdvSal(EmplId, Amount, Percent);
                //Mypay.UpdateAdvanceSalary(EmplId, Amount);
                Txt_Amount.Text = "";
                Txt_Rs.Text = "";
                Txt_Per.Text = "";
                double netpay = double.Parse(txt_Gross.Value);
                double Advance = 0.0;
                if (Hdn_Advance.Value != "")
                {
                    if (Hdn_Advance.Value != Hdn_CheckAdv.Value)
                    {
                        Advance = double.Parse(Hdn_Advance.Value);
                    }
                    else
                    {
                        Advance = 0;
                    }
                }

                txt_Gross.Value = (netpay + Advance).ToString();
                double gross = double.Parse(txt_Gross.Value);
                double deduction = double.Parse(Txt_Total.Value);
                txt_NetPay.Value = (gross - deduction).ToString();
                WC_MessageBox.ShowMssage("Successfully Saved");
            }
        }
        
            
            
   

        protected void Lnk_SalAdvance_Click(object sender, EventArgs e)
        {
            string EmplId = Request.QueryString["EmpId"].ToString();
            OdbcDataReader MyPayReader = null;
            Txt_Amount.Text = "";
            Txt_Rs.Text = "";
            Txt_Per.Text = "";
            MyPayReader = Mypay.GetAdvancesalOfMonth(MonthId, EmplId);
            if (MyPayReader.HasRows)
            {

                Txt_Amount.Text = MyPayReader.GetValue(0).ToString();
                Hdn_CheckAdv.Value = Txt_Amount.Text;
                Txt_Rs.Text = "";
                Txt_Per.Text = MyPayReader.GetValue(1).ToString();
            }
            else
            {
                int a = 0;
                int b = 0;
                int c = 0;
                Txt_Amount.Text = a.ToString();
                Txt_Rs.Text = c.ToString();
                Txt_Per.Text = b.ToString();
            }
            MPE_SalAdv_popup.Show();
        }

        protected void Grd_Earning_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        protected void Grd_Deduction_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            if (Txt_previousAdvamount.Text != "")
            {
                Txt_previousAdvamount.Visible = true;
                Lbl_PreviousAdvamount.Visible = true;
            }
            else
            {

                Txt_previousAdvamount.Visible = false;
                Lbl_PreviousAdvamount.Visible = false;
            }
        }    

    }
}
