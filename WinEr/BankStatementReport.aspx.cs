using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using WinBase;

namespace WinEr
{
    public partial class BankStatementReport : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");

            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyFeeMang = MyUser.GetFeeObj();
            if (MyFeeMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(930))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadLastOneMonth();
                    LoadClassDropDown();
                    LoadCollectedUser();
                    LoadFeeType();
                    Pnl_Report.Visible = false;
                }
            }

        }

        protected void Grid_BankReport_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grid_BankReport.PageIndex = e.NewPageIndex;
            LoadReport();
        }
      

        protected void Btn_ShowReport_Click(object sender, EventArgs e)
        {
            int classid = 0, studentid = 0;
            int.TryParse(Drp_Class.SelectedValue, out classid);
            LoadReport();
        }

        protected void img_export_Excel_Click(object sender, ImageClickEventArgs e)
        {

            DataSet MyData = (DataSet)ViewState["Report"];
            string Name = "BankStatementReport.xls";
            //if (!ExcelUtility.ExportDataSetToExcel(MyData, Name))
            //{

            //}
            string FileName = "BankStatementReport";
            string _ReportName = "Bank Statement Report";
            if (!WinEr.ExcelUtility.ExportDataToExcel(MyData, _ReportName, FileName, MyUser.ExcelHeader))
            {

            }
        }

        private void LoadCollectedUser()
        {

            Drp_CollectedUser.Items.Clear();
            DataSet myDataset;
            myDataset = MyFeeMang.LoadCollectedUser();
            if (myDataset != null && myDataset.Tables != null && myDataset.Tables[0].Rows.Count > 0)
            {
                ListItem li = new ListItem("All User", "0");
                Drp_CollectedUser.Items.Add(li);
                foreach (DataRow dr in myDataset.Tables[0].Rows)
                {
                    li = new ListItem(dr[0].ToString(), dr[1].ToString());
                    Drp_CollectedUser.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No user found", "-1");
                Drp_CollectedUser.Items.Add(li);
            }
            Drp_CollectedUser.SelectedIndex = 0;
        }
        private void LoadAmmount(int _classid, int _feetype, int _userid, int _paymentmode)
        {
            DateTime _from = MyUser.GetDareFromText(Txt_from.Text.ToString());
            DateTime _To = MyUser.GetDareFromText(Txt_To.Text.ToString());
            string sql = "";
            

            //sql = "select tblfeebill.Id, tblfeebill.BillNo as `Bill No`, tblfeebill.BankName as `Bank Name`, Date_Format(tblfeebill.`Date`,'%d-%m-%Y') as `Paid Date`, tblfeebill.PaymentMode as `Mode Of Payment`, tblfeebill.StudentName as `Student Name`, tblfeebill.TotalAmount as `Amount`, tblfeebill.PaymentModeId as `DD/Cheque No`,tblfeebill.StudentID, tblclass.ClassName as `Class`, tblstandard.Name as `Standard`, tblbatch.BatchName as `Batch`, tblview_student.AdmitionNo as `Admission No`,tblfeebill.OtherReference as `Other Reference` from tblfeebill inner join tblclass on tblclass.Id= tblfeebill.ClassID inner join tblstandard on    tblstandard.Id= tblclass.Standard inner join  tblview_student on tblview_student.Id= tblfeebill.StudentID inner join tblbatch on tblfeebill.AccYear= tblbatch.Id   WHERE  tblfeebill.`Date` BETWEEN '" + _from.ToString("s") + "' and '" + _To.ToString("s") + "' and tblfeebill.Canceled=0 ";
            sql = "select tblfeebill.Id, tblfeebill.BillNo as `Bill No`, tblfeebill.BankName as `Bank Name`, Date_Format(tblfeebill.`Date`,'%d-%m-%Y') as `Paid Date`, tblfeebill.PaymentMode as `Mode Of Payment`, tblfeebill.StudentName as `Student Name`, tblfeebill.TotalAmount as `Amount`,tblfeebill.PaymentModeId as `DD/Cheque No`,tblfeebill.StudentID, tblclass.ClassName as `Class`,tblbatch.BatchName as `Batch`,tblfeebill.OtherReference as `Other Reference` from tblfeebill inner join tblclass on tblclass.Id= tblfeebill.ClassID inner join tblbatch on tblfeebill.AccYear= tblbatch.Id  WHERE  tblfeebill.`Date` BETWEEN '" + _from.ToString("s") + "' and '" + _To.ToString("s") + "' and tblfeebill.Canceled=0";
            if (_classid > 0)
            {
                sql = sql + " and tblfeebill.ClassID=" + _classid + "";
            }
            if (_feetype == 1)
            {
                sql = sql + " and tblfeebill.RegularFee=1";
            }
            if (_feetype == 2)
            {
                sql = sql + " and tblfeebill.RegularFee=0";
            }
            if (_userid > 0)
            {
                sql = sql + " and tblfeebill.UserId=" + _userid + "";
            }
            if (_paymentmode == 1)
            {
                sql = sql + " and tblfeebill.PaymentMode='Cash'";
            }
            if (_paymentmode == 2)
            {
                sql = sql + " and tblfeebill.PaymentMode='Demand Draft'";
            }
            if (_paymentmode == 3)
            {
                sql = sql + " and tblfeebill.PaymentMode='Cheque'";
            }
            if (_paymentmode == 4)
            {
                sql = sql + " and tblfeebill.PaymentMode='NEFT'";
            }

            //sql = sql + "  order by `date` desc";
            //sai changed for based on student roll no 
            //sql = sql + "  order by tblview_student.ClassId ASC ,tblview_student.RollNo ASC,`date` desc ";
            //string sql = GetGridSqlString(_from, _To);
            double _amount = 0;
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    _amount = double.Parse(MyReader.GetValue(6).ToString()) + _amount;
                }

            }
            Txt_total.Text = _amount.ToString();
        }
        private void LoadReport()
        {
            Lbl_Msg.Text = "";
            DateTime _fromdate = new DateTime();
            DateTime _Todate = new DateTime();
            DataSet AutoDs = new DataSet();
            int _classid = 0, _feetype = 0, _userid = 0, _paymentmode = 0;
            _fromdate = General.GetDateTimeFromText(Txt_from.Text);
            _Todate = General.GetDateTimeFromText(Txt_To.Text);
            int.TryParse(Drp_Class.SelectedValue, out _classid);
            int.TryParse(Drp_FeeType.SelectedValue, out _feetype);
            int.TryParse(Drp_CollectedUser.SelectedValue, out _userid);
            int.TryParse(Drp_PaymentMode.SelectedValue, out _paymentmode);
            AutoDs = MyFeeMang.GetBankStatementReport(_fromdate, _Todate, _classid, _feetype, _userid, _paymentmode);
            LoadAmmount(_classid, _feetype, _userid, _paymentmode);
            if (AutoDs != null && AutoDs.Tables[0].Rows.Count > 0)
            {
                Pnl_Report.Visible = true;
                //Grid_BankReport.Columns[0].Visible = true;
                //Grid_BankReport.Columns[1].Visible = true;
                Grid_BankReport.DataSource = AutoDs;
                Grid_BankReport.DataBind();
                //Grid_BankReport.Columns[0].Visible = false;
                //Grid_BankReport.Columns[1].Visible = false;
            }
            else
            {
                Grid_BankReport.DataSource = null;
                Grid_BankReport.DataBind();
                Pnl_Report.Visible = false;
                Lbl_Msg.Text = "No report found..";
            }
            ViewState["Report"] = AutoDs;
        }       

       

        private void LoadLastOneMonth()
        {

            if (MyUser.HaveActionRignt(894))
            {
                DateTime _Now = System.DateTime.Now;
                Txt_To.Text = General.GerFormatedDatVal(_Now);
                DateTime _Dt = _Now.AddMonths(-1);
                Txt_from.Text = General.GerFormatedDatVal(_Dt);
            }
            else
            {
                DateTime _Now = System.DateTime.Now.Date;
                Txt_To.Text = MyUser.GerFormatedDatVal(_Now);
                Txt_from.Text = MyUser.GerFormatedDatVal(_Now);
                Txt_To.Enabled = false;
                Txt_from.Enabled = false;
            }

        }

        private void LoadFeeType()
        {
            OdbcDataReader MyReader = null;
            Drp_FeeType.Items.Clear();
            string sql = "SELECT tblfeetype.Id , tblfeetype.Name from tblfeetype";
            if (!MyFeeMang.HasJoiningFee())
            {
                sql = sql + " where tblfeetype.Id<>2";
            }
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("All", "0");
                Drp_FeeType.Items.Add(li);
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_FeeType.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No type Found", "-1");
                Drp_FeeType.Items.Add(li);
            }
        }

        private void LoadClassDropDown()
        {
            Drp_Class.Items.Clear();

            DataSet myDataset;
            myDataset = MyUser.MyAssociatedClass();
            if (myDataset != null && myDataset.Tables != null && myDataset.Tables[0].Rows.Count > 0)
            {
                ListItem li = new ListItem("All Class", "0");
                Drp_Class.Items.Add(li);

                foreach (DataRow dr in myDataset.Tables[0].Rows)
                {

                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class.Items.Add(li);

                }

            }
            else
            {
                ListItem li = new ListItem("No Class Found", "-1");
                Drp_Class.Items.Add(li);
            }


            Drp_Class.SelectedIndex = 0;
        }
    }
}