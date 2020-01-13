using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.Odbc;
using WinBase;
namespace WinEr
{
    public partial class WebForm12 : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;

        private DataSet MydataSet;
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
            else if (!MyUser.HaveActionRignt(8))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadLastOneMonth();
                    LoadClass3toDrp();
                    LoadAccountTodrp();
                    LoadfeeTodrp();
                    LoadCollectedUser();
                    LoadFeeType();


                    //if (!MyUser.HaveActionRignt(42))
                    //{
                    //    TabPanel2.Visible = false;

                    //}
                    ViewState["_columnName"] = null;
                    //some initlization
                }
            }
        }

        private void LoadfeeTodrp()
        {
            string sql = "SELECT tblfeeaccount.Id, tblfeeaccount.AccountName from tblfeeaccount where tblfeeaccount.Status=1 ORDER BY  tblfeeaccount.AccountName";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li;
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    ChkBx_feeNameInAmt.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Fee Found", "-1");
                ChkBx_feeNameInAmt.Items.Add(li);
                WC_MessageBox.ShowMssage("No Fees Found");
                DisableAll();
            }

        }

        private void LoadFeeType()
        {
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
                DisableAll();
            }
        }

        private void DisableAll()
        {
            Drp_FeeType.Enabled = false;
            Btn_Export.Enabled = false;
            Btn_ExportAmount.Enabled = false;
        }

        #region Amount Collected

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

        private void LoadAccountTodrp()
        {
            Drp_Account.Items.Clear();
            string sql = " SELECT tblaccount.Id,tblaccount.AccountName FROM tblaccount where tblaccount.Id<>2";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("Total Collected", "0");
                Drp_Account.Items.Add(li);
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Account.Items.Add(li);


                }
            }
            else
            {
                ListItem li = new ListItem("NoAccountFound", "-1");
                Drp_Account.Items.Add(li);
            }
            Drp_Account.SelectedIndex = 0;
        }
        
        private void LoadClass3toDrp()
        {
            Drp_Class3.Items.Clear();

            DataSet myDataset;
            myDataset = MyUser.MyAssociatedClass();
            if (myDataset != null && myDataset.Tables != null && myDataset.Tables[0].Rows.Count > 0)
            {
                ListItem li = new ListItem("All Class", "0");
                Drp_Class3.Items.Add(li);

                foreach (DataRow dr in myDataset.Tables[0].Rows)
                {

                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class3.Items.Add(li);

                }

            }
            else
            {
                ListItem li = new ListItem("No Class Found", "-1");
                Drp_Class3.Items.Add(li);
            }


            Drp_Class3.SelectedIndex = 0;
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
        
        protected void Drp_Class3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ReloadData();
        }

        protected void Drp_FeeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDrp_feeNameInAmt();
        }

        private void LoadDrp_feeNameInAmt()
        {
            ChkBx_feeNameInAmt.Items.Clear();
            string sql = "SELECT tblfeeaccount.Id, tblfeeaccount.AccountName from tblfeeaccount where tblfeeaccount.Status=1";
            if (Drp_FeeType.SelectedValue == "1")
            {
                sql = sql + " and tblfeeaccount.`Type`<>2";
            }
            if (Drp_FeeType.SelectedValue == "2")
            {
                sql = sql + " and tblfeeaccount.`Type`<>1";
            }
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li;
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    ChkBx_feeNameInAmt.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Fee Found", "-1");
                ChkBx_feeNameInAmt.Items.Add(li);
                //Lbl_msg.Text = "No Fee Found";
                //this.MPE_MessageBox.Show();
                WC_MessageBox.ShowMssage("No Fees Found");
                DisableAll();
            }
        }

        protected void Btn_getAmount_Click(object sender, EventArgs e)
        {
            Grid_fee3.PageIndex = 0;
            if (LoadDataTogrid())
            {
                LoadAmmount();
            }
        }

        private bool LoadDataTogrid()
        {
            bool _valide = false;
            if (Txt_from.Text.Trim() == "" || Txt_To.Text.Trim() == "")
            {

                //Lbl_msg.Text = "One or more fields are empty";
                //this.MPE_MessageBox.Show();
                WC_MessageBox.ShowMssage("One or more fields are empty");

            }
            else if (Drp_Class3.SelectedValue == "-1")
            {
                WC_MessageBox.ShowMssage("No class Found");
                //Lbl_msg.Text = "No class Found";
                //this.MPE_MessageBox.Show();
            }
            else if (Drp_Account.SelectedValue == "-1")
            {
                WC_MessageBox.ShowMssage("No account Found");
                //Lbl_msg.Text = "No account Found";
                //this.MPE_MessageBox.Show();
            }
            else if (RdBtLstSelectCtgry1.SelectedItem.Text != "All" && !ChkBx_feeNameInAmt.Items.Cast<ListItem>().Any(item => item.Selected))
            {
                WC_MessageBox.ShowMssage("Please select at least one fee.");
                //Lbl_msg.Text = "No Fee Found";
                //this.MPE_MessageBox.Show();
            }
            else
            {
                try
                {

                    DateTime _from = MyUser.GetDareFromText(Txt_from.Text.ToString());
                    DateTime _To = MyUser.GetDareFromText(Txt_To.Text.ToString());
                    if (_from > _To)
                    {
                        WC_MessageBox.ShowMssage("From date should not be greater than to Date");
                        //Lbl_msg.Text = "From date should not be greater than to Date";
                        //this.MPE_MessageBox.Show();
                    }
                    else
                    {
                        string sql = GetGridSqlString(_from, _To);
                        MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                        MydataSet = ReplaceCharector(MydataSet);
                        if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
                        {
                            // Lbl_Message.Text = "";
                            Grid_fee3.DataSource = MydataSet;
                            Grid_fee3.DataBind();
                            EnableTransGrid();
                            Btn_ExportAmount.Enabled = true;
                            Btn_Export.Enabled = true;
                            _valide = true;

                        }
                        else
                        {
                            ReloadData();
                            Grid_fee3.DataSource = null;
                            Grid_fee3.DataBind();
                            Btn_ExportAmount.Enabled = false;
                            Btn_Export.Enabled = false;
                            //Lbl_Message.Text = "No transactions found";
                            WC_MessageBox.ShowMssage("No Fee Found");
                            //Lbl_msg.Text = "No Fee Found";
                            //this.MPE_MessageBox.Show();


                        }
                    }
                }
                catch (Exception Exc)
                {
                    WC_MessageBox.ShowMssage(Exc.Message);
                    //Lbl_msg.Text = Exc.Message;
                    //this.MPE_MessageBox.Show();
                    _valide = false;

                }
            }
            return _valide;

        }

        private void LoadAmmount()
        {
            DateTime _from = MyUser.GetDareFromText(Txt_from.Text.ToString());
            DateTime _To = MyUser.GetDareFromText(Txt_To.Text.ToString());

            string sql = GetGridSqlString(_from, _To);
            double _amount = 0;
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    _amount = double.Parse(MyReader.GetValue(5).ToString()) + _amount;
                }

            }
            Txt_total.Text = _amount.ToString();
        }

        private void EnableTransGrid()
        {
            Pnl_trans.Visible = true;
        }

        private DataSet ReplaceCharector(DataSet MydataSet)
        {
            DataSet MyFeeData = new DataSet();
            MyFeeData.Tables.Add(new DataTable("Fees"));
            DataTable dt = MyFeeData.Tables["Fees"];
            DataRow dr;
            dt.Columns.Add("StudentName");
            dt.Columns.Add("ClassName");
            dt.Columns.Add("FeeName");
            dt.Columns.Add("PeriodName");
            dt.Columns.Add("AccountName");
            dt.Columns.Add("Amount");
            dt.Columns.Add("PaidDate");
            dt.Columns.Add("BillNo");
            foreach (DataRow Dr_Row in MydataSet.Tables[0].Rows)
            {
                dr = MyFeeData.Tables["Fees"].NewRow();
                dr["StudentName"] = Dr_Row[0].ToString().ToUpper();
                dr["ClassName"] = Dr_Row[1].ToString();
                dr["FeeName"] = Dr_Row[2].ToString().Replace("&amp;", "&");
                dr["PeriodName"] = Dr_Row[3].ToString().Replace("&amp;", "&");
                dr["AccountName"] = Dr_Row[4].ToString();
                dr["Amount"] = Dr_Row[5].ToString();
                dr["PaidDate"] = Dr_Row[6].ToString();
                dr["BillNo"] = Dr_Row[7].ToString();
                MyFeeData.Tables["Fees"].Rows.Add(dr);
                //Dr_Row[
            }
            return MyFeeData;
        }

        private string GetGridSqlString(DateTime _from, DateTime _To)
        {

            string _sql = "";
            _sql = "SELECT UPPER(tblview_transaction.StudentName) as `StudentName`, tblclass.ClassName, tblview_transaction.FeeName AS 'Fee Name',tblview_transaction.PeriodName AS 'Period Name', tblaccount.AccountName, tblview_transaction.Amount ,date_format( tblview_transaction.PaidDate , '%d-%m-%Y') AS 'PaidDate', tblview_transaction.BillNo from tblview_transaction inner join tblaccount on tblaccount.Id= tblview_transaction.AccountTo  inner join tblclass on tblclass.Id = tblview_transaction.ClassId where  tblview_transaction.PaidDate >='" + _from.Date.ToString("s") + "' AND tblview_transaction.PaidDate <='" + _To.Date.ToString("s") + "'";

            if (Drp_Class3.SelectedValue != "0")
            {
                _sql = _sql + " AND tblview_transaction.ClassId=" + Drp_Class3.SelectedValue;
            }
            if (Drp_Account.SelectedValue == "0")
            {
                _sql = _sql + " AND (tblview_transaction.AccountTo=1 OR tblview_transaction.AccountTo=4)";
            }
            else
            {
                _sql = _sql + " AND tblview_transaction.AccountTo=" + Drp_Account.SelectedValue;
            }
            if (RdBtLstSelectCtgry1.SelectedItem.Text != "All")
            {
                string temp_FeeId = "";
                foreach (ListItem listItem in ChkBx_feeNameInAmt.Items)
                {
                    if (listItem.Selected)
                    {
                        if (temp_FeeId == "")
                            temp_FeeId = listItem.Value;
                        else
                            temp_FeeId = temp_FeeId + "," + listItem.Value;
                    }
                }
                _sql = _sql + " AND tblview_transaction.FeeId in(" + temp_FeeId + ")";
            }
            if ((Drp_CollectedUser.SelectedValue != "0") && (Drp_CollectedUser.SelectedValue != "-1"))
            {
                _sql = _sql + " AND tblview_transaction.CollectedUser='" + Drp_CollectedUser.SelectedItem.Text + "'";
            }
            if (Drp_FeeType.SelectedValue == "1")
                _sql = _sql + " and tblview_transaction.RegularFee=1";
            if (Drp_FeeType.SelectedValue == "2")
                _sql = _sql + " and tblview_transaction.RegularFee=0";
            _sql = _sql + " and tblview_transaction.Canceled=0 ";
            _sql = _sql + " order by  tblview_transaction.PaidDate asc,tblview_transaction.TransationId asc";
            return _sql;
        }

        protected void Grid_fee3_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grid_fee3.PageIndex = e.NewPageIndex;
            if (LoadDataTogrid())
            {
                LoadAmmount();
            }
        }

        protected void Drp_Account_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ReloadData();

        }

        private void ReloadData()
        {
            Pnl_trans.Visible = false;
            Txt_total.Text = "";
            Grid_fee3.PageIndex = 0;

        }

        #endregion

        protected void Btn_Exportamount(object sender, EventArgs e)
        {
            //            DateTime _from = DateTime.Parse(Txt_from.Text.ToString());
            //            DateTime _To = DateTime.Parse(Txt_To.Text.ToString());
            DateTime _from = MyUser.GetDareFromText(Txt_from.Text.ToString());

            DateTime _To = MyUser.GetDareFromText(Txt_To.Text.ToString());
            if (Drp_Class3.SelectedValue != "-1" && ChkBx_feeNameInAmt.SelectedValue != "-1" && Drp_Account.SelectedValue != "-1" && Txt_from.Text != "" && Txt_To.Text != "")
            {

                if (_from > _To)
                {
                    WC_MessageBox.ShowMssage("From date should not be greater than to Date");
                    //Lbl_msg.Text = "From date should not be greater than to Date";
                    //this.MPE_MessageBox.Show();
                }
                else
                {
                    string sql = GetGridSqlString(_from, _To);
                    MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                }
                //MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    //if (!WinEr.ExcelUtility.ExportDataSetToExcel(MydataSet, "StudentList.xls"))
                    //{
                    //    WC_MessageBox.ShowMssage("MS Excel is missing. Please install");
                    //    //Lbl_msg.Text = "This function need Ms office";
                    //    //this.MPE_MessageBox.Show();
                    //}
                    string FileName = "FeeTransactionReport";

                    string _ReportName = "<table><tr><td colspan=\"7\" style=\"text-align:center;\"><b>Fee Transaction Report</b></td></tr><tr><td>Created Date:" + DateTime.Now.ToString() + "</td><td>From:" + Txt_from.Text + " TO:" + Txt_To.Text + " </td></tr></table>";
                    if (!WinEr.ExcelUtility.ExportDataToExcel(MydataSet, _ReportName, FileName, MyUser.ExcelHeader))
                    {
                        WC_MessageBox.ShowMssage("MS Excel is missing. Please install");
                    }

                }
            }
            else
            {
                WC_MessageBox.ShowMssage("Some fields are empty");
                //Lbl_msg.Text = "Some fields are empty";
                //this.MPE_MessageBox.Show();
            }


        }

        private double getAmount(string _BillNo, int _feeId)
        {
            double _amount = 0;
            string _sql = "SELECT DISTINCT  sum(tblview_transaction.Amount)  from tblview_transaction where tblview_transaction.BillNo='" + _BillNo + "' AND tblview_transaction.FeeId=" + _feeId + " and tblview_transaction.AccountTo<>3";
            OdbcDataReader _treader = MyFeeMang.m_MysqlDb.ExecuteQuery(_sql);
            if (_treader.HasRows)
            {
                double.TryParse(_treader.GetValue(0).ToString(), out _amount);
            }
            return _amount;
        }

        protected void Btn_Export_Click(object sender, EventArgs e)
        {
            //DateTime _from = DateTime.Parse(Txt_from.Text.ToString());
            //      DateTime _To = DateTime.Parse(Txt_To.Text.ToString());
            DateTime _from = MyUser.GetDareFromText(Txt_from.Text.ToString());
            DateTime _To = MyUser.GetDareFromText(Txt_To.Text.ToString());

            if (_from > _To)
            {
                WC_MessageBox.ShowMssage("From date should not be greater than to Date");
                //Lbl_msg.Text = "From date should not be greater than to Date";
                //this.MPE_MessageBox.Show();
            }
            else
            {
                try
                {
                    DataSet FeeReportDataSet = new DataSet();
                    DataTable dt;
                    DataRow dr;
                    DataSet _DSColumns = null;
                    int _feeId = 0;
                    string _columnName;
                    string _sql;
                    double _itemAmount = 0;
                    double TotAmt = 0;
                    #region Decleration Of Dataset
                    FeeReportDataSet.Tables.Add(new DataTable("FeeReport"));
                    dt = FeeReportDataSet.Tables["FeeReport"];
                    dt.Columns.Add("Name");
                    dt.Columns.Add("Class");
                    dt.Columns.Add("Receipt-No");
                    dt.Columns.Add("BillDate");
                    dt.Columns.Add("Other Reference");
                    string _sqlCol = "select tblfeeaccount.Id, tblfeeaccount.AccountName FROM tblfeeaccount where  tblfeeaccount.Status=1";
                    //if ((Drp_FeeType.SelectedValue != "0") && (Drp_FeeType.SelectedValue != "-1")) OLD CODE
                    //{
                    //    _sqlCol += " AND  tblfeeaccount.Type=" + Drp_FeeType.SelectedValue;
                    //}

                    if (RdBtLstSelectCtgry1.SelectedItem.Text != "All")
                    {
                        string temp_FeeId = "";
                        foreach (ListItem listItem in ChkBx_feeNameInAmt.Items)
                        {
                            if (listItem.Selected)
                            {
                                if (temp_FeeId == "")
                                    temp_FeeId = listItem.Value;
                                else
                                    temp_FeeId = temp_FeeId + "," + listItem.Value;
                            }
                        }
                        _sqlCol += " AND tblfeeaccount.Id in(" + temp_FeeId + ")";
                    }
                    _DSColumns = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(_sqlCol);

                    ViewState["_columnName"] = _DSColumns;

                    if ((_DSColumns.Tables[0].Rows.Count > 0) && (_DSColumns != null))
                    {
                        for (int i = 0; i < _DSColumns.Tables[0].Rows.Count; i++)
                        {
                            _columnName = _DSColumns.Tables[0].Rows[i][1].ToString();
                            dt.Columns.Add(_columnName);
                        }
                    }
                    dt.Columns.Add("Total");

                    #endregion

                    #region Area To Get SqlQuerry
                    //if any of the below true then 2 more inner joins needed otherwise we can avoid that
                    if ((Drp_FeeType.SelectedValue != "0") || (Drp_FeeType.SelectedValue != "-1") || (RdBtLstSelectCtgry1.SelectedItem.Text != "All")) //need to inner join to table tblview_transaction_entire AND tblfeeaccount
                    {
                        _sql = "SELECT  distinct tblview_feebill.StudentName, tblclass.ClassName, tblview_feebill.BillNo, tblview_feebill.TotalAmount, date_format( tblview_feebill.Date , '%d-%m-%Y'),tblview_feebill.OtherReference from tblview_feebill  inner join tblview_transaction on tblview_feebill.BillNo = tblview_transaction.BillNo inner join tblclass on tblclass.Id = tblview_transaction.ClassId inner join tblfeeaccount on tblfeeaccount.Id= tblview_transaction.FeeId where  tblview_feebill.Date >='" + _from.Date.ToString("s") + "' AND tblview_feebill.Date <='" + _To.Date.ToString("s") + "'";
                    }
                    else
                        _sql = "SELECT  distinct tblview_feebill.StudentName, tblclass.ClassName, tblview_feebill.BillNo, tblview_feebill.TotalAmount, date_format( tblview_feebill.Date , '%d-%m-%Y'),tblview_feebill.OtherReference from tblview_feebill  inner join tblview_transaction on tblview_feebill.BillNo = tblview_transaction.BillNo inner join tblclass on tblclass.Id = tblview_transaction.ClassId where  tblview_feebill.Date >='" + _from.Date.ToString("s") + "' AND tblview_feebill.Date <='" + _To.Date.ToString("s") + "'";


                    //if ((Drp_FeeType.SelectedValue != "0") && (Drp_FeeType.SelectedValue != "-1")) OLD CODE
                    //{

                    //    _sql = _sql + " AND tblfeeaccount.`Type`=" + Drp_FeeType.SelectedValue;
                    //}


                    if (Drp_FeeType.SelectedValue == "1")
                        _sql = _sql + " and tblview_transaction.RegularFee=1";
                    if (Drp_FeeType.SelectedValue == "2")
                        _sql = _sql + " and tblview_transaction.RegularFee=0";


                    if (Drp_Class3.SelectedValue != "0")
                    {
                        _sql = _sql + " AND tblview_feebill.ClassId=" + Drp_Class3.SelectedValue;
                    }

                    if (RdBtLstSelectCtgry1.SelectedItem.Text != "All")
                    {
                        string temp_FeeId = "";
                        foreach (ListItem listItem in ChkBx_feeNameInAmt.Items)
                        {
                            if (listItem.Selected)
                            {
                                if (temp_FeeId == "")
                                    temp_FeeId = listItem.Value;
                                else
                                    temp_FeeId = temp_FeeId + "," + listItem.Value;
                            }
                        }
                        _sql = _sql + " AND tblview_transaction.FeeId in(" + temp_FeeId + ")";
                    }
                    if ((Drp_CollectedUser.SelectedValue != "0") && (Drp_CollectedUser.SelectedValue != "-1"))
                    {
                        _sql = _sql + " AND tblview_feebill.CollectedUser=" + Drp_CollectedUser.SelectedValue;
                    }
                    _sql = _sql + " AND tblview_feebill.Canceled=0 and tblview_transaction.Canceled=0";
                    _sql = _sql + " order by  tblview_transaction.PaidDate asc,tblview_transaction.TransationId asc";
                    #endregion

                    #region for build Dataset  Based On the sql

                    double _dayTotal = 0;
                    double[] _dayItemTotal = new double[_DSColumns.Tables[0].Rows.Count];//this array to store total amount of individual fee items

                    MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(_sql);
                    if (MyReader.HasRows)
                    {
                        while (MyReader.Read())
                        {
                            TotAmt = 0;
                            dr = FeeReportDataSet.Tables["FeeReport"].NewRow();
                            dr["Name"] = MyReader.GetValue(0).ToString();
                            dr["Class"] = MyReader.GetValue(1).ToString();
                            dr["Receipt-No"] = MyReader.GetValue(2).ToString();

                            #region For Each Bill Find The  Individual Item Amount
                            //_DSColumns = (DataSet)ViewState["_columnName"];                  
                            if (_DSColumns != null)
                            {
                                for (int i = 0; i < _DSColumns.Tables[0].Rows.Count; i++)
                                {

                                    _feeId = int.Parse(_DSColumns.Tables[0].Rows[i][0].ToString());
                                    _columnName = _DSColumns.Tables[0].Rows[i][1].ToString();
                                    _itemAmount = getAmount(MyReader.GetValue(2).ToString(), _feeId);
                                    dr[_columnName] = _itemAmount;
                                    _dayItemTotal[i] += _itemAmount;

                                    TotAmt = TotAmt + _itemAmount;//new code

                                }
                                dr["Total"] = TotAmt.ToString();
                            }
                            #endregion


                            // dr["Total"] = MyReader.GetValue(3).ToString();
                            dr["BillDate"] = MyReader.GetValue(4).ToString();
                            dr["Other Reference"] = MyReader.GetValue(5).ToString();
                            //_dayTotal = _dayTotal + double.Parse(MyReader.GetValue(3).ToString());
                            _dayTotal = _dayTotal + TotAmt;
                            FeeReportDataSet.Tables["FeeReport"].Rows.Add(dr);
                        }
                    }

                    #region Last Row To Get The Total AMount Details
                    dr = FeeReportDataSet.Tables["FeeReport"].NewRow();
                    dr["Name"] = "Total Amount";
                    dr["Class"] = "";
                    dr["Receipt-No"] = "";
                    _DSColumns = (DataSet)ViewState["_columnName"];
                    if (_DSColumns != null)
                    {
                        for (int i = 0; i < _DSColumns.Tables[0].Rows.Count; i++)
                        {
                            _columnName = _DSColumns.Tables[0].Rows[i][1].ToString();
                            _itemAmount = _dayItemTotal[i];
                            dr[_columnName] = _itemAmount;

                        }
                    }
                    dr["Total"] = _dayTotal;
                    FeeReportDataSet.Tables["FeeReport"].Rows.Add(dr);
                    #endregion

                    #endregion

                    //if (!WinEr.ExcelUtility.ExportDataSetToExcel(FeeReportDataSet, "Fee Report F.xls"))
                    //{
                    //    WC_MessageBox.ShowMssage("MS Excel is missing. Please install");
                    //    //Lbl_msg.Text = "This function need Ms office";
                    //    //this.MPE_MessageBox.Show();
                    //}
                    string FileName = "Bill Report";

                    string _ReportName = "<table><tr><td colspan=\"8\" style=\"text-align:center;\"><b>Fee Bill Report</b></td></tr><tr><td>Created Date:" + DateTime.Now.ToString() + "</td><td>From:" + Txt_from.Text + " TO:" + Txt_To.Text + " </td></tr></table>";
                    if (!WinEr.ExcelUtility.ExportDataToExcel(FeeReportDataSet, _ReportName, FileName, MyUser.ExcelHeader))
                    {
                        WC_MessageBox.ShowMssage("MS Excel is missing. Please install");
                    }
                }
                catch (Exception Ex)
                {
                    WC_MessageBox.ShowMssage("Unable To do export.Error:" + Ex.Message);
                }
            }
        }

        protected void RdBtLstSelectCtgry1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RdBtLstSelectCtgry1.SelectedItem.Text != "All")
            {
                Panel_feeCatgry.Visible = true;
                for (int i = 0; i < ChkBx_feeNameInAmt.Items.Count; i++)
                {
                    //ChkBx_feeNameInAmt.Items[i].Enabled = true;
                    ChkBx_feeNameInAmt.Items[i].Selected = false;
                }
            }
            else
            {
                Panel_feeCatgry.Visible = false;
                for (int i = 0; i < ChkBx_feeNameInAmt.Items.Count; i++)
                {
                    //ChkBx_feeNameInAmt.Items[i].Enabled = true;
                    ChkBx_feeNameInAmt.Items[i].Selected = true;
                }
            }
        }
    }
}
