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
using System.Text;
namespace WinEr
{
    public partial class WebForm9 : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private EmailManager Obj_Email;
        private OdbcDataReader MyReader = null;
        private OdbcDataReader MyReader1 = null;
        private DataSet MydataSet;
        private SchoolClass objSchool = null;

        public int SELECTEDCLASSID
        {
            get
            {
                return int.Parse(HiddenField_ClassId.Value);
            }
            set
            {
                HiddenField_ClassId.Value = value.ToString();
            }

        }

        public int SELECTEDStudentID
        {
            get
            {
                return int.Parse(HiddenField_StudentId.Value);
            }
            set
            {
                HiddenField_StudentId.Value = value.ToString();
            }


        }

        protected void Page_Load(object sender, EventArgs e)
        {
            OtherFeeBox.EVNTItemAdded += new EventHandler(BindOtherFeeItems);
            FeeAdvanceBox.FeeAdvance += new EventHandler(BindAdvanceFees);
            WC_SETTLEADVANCE.FeeAdvanceCancled += new EventHandler(AdvanceFeeCanclled);

            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }

            MyUser = (KnowinUser)Session["UserObj"];
            MyFeeMang = MyUser.GetFeeObj();
            Obj_Email = MyUser.GetEmailObj();
            if (MyFeeMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(2))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (WinerUtlity.NeedCentrelDB())
                {
                    if (Session[WinerConstants.SessionSchool] == null)
                    {
                        Response.Redirect("Logout.aspx");
                    }
                    objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                }

                if (!IsPostBack)
                {

                    AddClassToDropDownClass();
                    if (Session["ClassId"] != null)
                    {
                        int ClassId;
                        if (int.TryParse(Session["ClassId"].ToString(), out ClassId))
                        {
                            DropDownClass.SelectedValue = ClassId.ToString();
                            if (MyFeeMang.HaveStudWithOutRollNoInClass(int.Parse(DropDownClass.SelectedValue.ToString()), MyUser.CurrentBatchId))
                            {
                                Lbl_info.Text = "Some students in " + DropDownClass.SelectedItem.Text + " are not having Roll No. Please Schedule Roll No before collecting fee for them. ";
                            }
                            else
                            {
                                Lbl_info.Text = "";
                            }


                        }

                    }


                    AddStudentToDropDownStudent();
                    AddStudentNameToDropDownStudent();
                    if (StudentIdMandatory())
                    {
                        RowStudentId.Visible = true;
                        AddStudentIdToDropDown();
                    }
                    else
                    {
                        RowStudentId.Visible = false;
                    }

                    ReloadData();
                    DropDownClass.Focus();
                    QuickFeecollect();


                    //some initlization

                }
            }
        }

        private void AddStudentIdToDropDown()
        {
            string sql = "";
            Drp_StudentId.Items.Clear();
            sql = " SELECT tblstudent.Id,tblstudent.StudentId FROM tblstudent  inner join tblstudentclassmap on tblstudent.id= tblstudentclassmap.Studentid where tblstudent.status=1 and  tblstudentclassmap.ClassId=" + int.Parse(DropDownClass.SelectedValue.ToString()) + " and tblstudentclassmap.RollNo<>-1 and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " order by tblstudentclassmap.RollNo";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_StudentId.Items.Add(li);
                }


            }
            else
            {
                ListItem li = new ListItem("No students found", "-1");
                Drp_StudentId.Items.Add(li);
            }
        }

        private bool StudentIdMandatory()
        {
            string sql = "";
            OdbcDataReader Configvaluereader = null;
            bool mandatory = false;
            int value = 0;
            sql = "Select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='IsStudentIdMandatory'";
            Configvaluereader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (Configvaluereader.HasRows)
            {
                int.TryParse(Configvaluereader.GetValue(0).ToString(), out value);
                if (value == 1)
                {
                    mandatory = true;
                }
            }
            return mandatory;
        }

        private void LoadPrinterModel(out string PrinterType, out int FeeCount)
        {
            PrinterType = "0";
            FeeCount = 0;
            string sql = "select tblconfiguration.Value, tblconfiguration.SubValue from tblconfiguration where tblconfiguration.Module='Fee Manager' and tblconfiguration.Name='DotMetrixPrinter'";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                PrinterType = MyReader.GetValue(0).ToString();
                FeeCount = int.Parse(MyReader.GetValue(1).ToString());
            }

        }

        private void QuickFeecollect()
        {

            try
            {
                if ((Request.QueryString["StudentId"] != null))
                {
                    Chk_AllFees.Checked = true;
                    chkBoxAll.Checked = true;
                    int _StudentId = int.Parse(Request.QueryString["StudentId"].ToString());
                    int _classID = MyFeeMang.GetClassID(_StudentId);
                    if (_classID != 0)
                    {
                        SELECTEDCLASSID = _classID;
                        SELECTEDStudentID = _StudentId;

                        LoadSuddentFee();
                        Pnl_feearea.Visible = true;
                    }

                }
            }
            catch
            {

            }
        }

        private void AddClassToDropDownClass()
        {
            DropDownClass.Items.Clear();
            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    DropDownClass.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Class Present", "-1");
                DropDownClass.Items.Add(li);
            }
            DropDownClass.SelectedIndex = 0;
            if (MyFeeMang.HaveStudWithOutRollNoInClass(int.Parse(DropDownClass.SelectedValue.ToString()), MyUser.CurrentBatchId))
            {
                Lbl_info.Text = "Some students in " + DropDownClass.SelectedItem.Text + " are not having Roll No. Please Schedule Roll No. before collecting fee for them. ";
            }
        }

        private void AddStudentToDropDownStudent()
        {
            DropDownStudentId.Items.Clear();
            string sql = " SELECT map.StudentId,map.RollNo FROM tblstudentclassmap map inner join tblstudent stud on stud.id= map.Studentid where stud.status=1 and  map.ClassId=" + int.Parse(DropDownClass.SelectedValue.ToString()) + " and map.RollNo<>-1 and map.BatchId=" + MyUser.CurrentBatchId + " order by map.RollNo";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    DropDownStudentId.Items.Add(li);
                }


            }
            else
            {
                ListItem li = new ListItem("No students found", "-1");
                DropDownStudentId.Items.Add(li);
            }
        }
        private void AddStudentNameToDropDownStudent()
        {
            Drp_Studname.Items.Clear();
            string sql = " SELECT map.StudentId,stud.StudentName FROM tblstudentclassmap map inner join tblstudent stud on stud.id= map.Studentid where stud.status=1 and  map.ClassId=" + int.Parse(DropDownClass.SelectedValue.ToString()) + " and map.RollNo<>-1 and map.BatchId=" + MyUser.CurrentBatchId + " order by map.RollNo";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Studname.Items.Add(li);
                }

            }
            else
            {
                ListItem li = new ListItem("No students found", "-1");
                Drp_Studname.Items.Add(li);
            }
            MyReader.Close();
        }



        private void ClaculateTotalAmount()
        {


            double Sum = 0;
            double arrieramount;
            double Deduction_value;
            double Fine_value;
            double _balance;
            bool _checked = false;
            BtnBill.Enabled = false;
            TxtTotatAmount.Value = "0";
            try
            {
                if (Txt_AmountPaying.Text.Trim() == "")
                {

                    Txt_Balance.Value = "0";
                }
                else
                {
                    _balance = Sum - double.Parse(Txt_AmountPaying.Text);
                    Txt_Balance.Value = _balance.ToString();
                }
            }
            catch
            {
                Txt_Balance.Value = "Nil";
            }
            Btn_payfee.Enabled = false;
            foreach (GridViewRow gv in GridViewAllFee.Rows)
            {
                TextBox Txt_fine = (TextBox)gv.FindControl("TxtFine");
                CheckBox cb = (CheckBox)gv.FindControl("CheckBoxUpdate");
                if (cb.Checked)
                {
                    _checked = true;
                    TextBox Tx_dudection = (TextBox)gv.FindControl("TxtDeduction");
                    TextBox Arrier = (TextBox)gv.FindControl("Txtarrier");


                    try
                    {
                        arrieramount = double.Parse(Arrier.Text.ToString());
                    }
                    catch
                    {
                        Arrier.Text = "0";
                        arrieramount = 0;

                    }
                    try
                    {
                        Deduction_value = double.Parse(Tx_dudection.Text.ToString());
                    }
                    catch
                    {
                        Tx_dudection.Text = "0";
                        Deduction_value = 0;

                    }
                    try
                    {
                        Fine_value = double.Parse(Txt_fine.Text.ToString());
                    }
                    catch
                    {
                        Txt_fine.Text = "0";
                        Fine_value = 0;
                    }
                    if (double.Parse(gv.Cells[7].Text.ToString()) < Deduction_value)
                    {
                        Lbl_MessageError.Text = "Deduction should not be greater than amount";
                        this.MPE_MessageError.Show();
                        TxtTotatAmount.Value = "0";
                        Txt_Balance.Value = "Nil";
                        break;
                    }
                    else if (double.Parse(gv.Cells[7].Text.ToString()) < arrieramount)
                    {
                        Lbl_MessageError.Text = "Arrear should not be greater than amount";
                        this.MPE_MessageError.Show();
                        TxtTotatAmount.Value = "0";
                        Txt_Balance.Value = "Nil";
                        break;
                    }
                    else if (double.Parse(gv.Cells[7].Text.ToString()) < (arrieramount + Deduction_value))
                    {
                        Lbl_MessageError.Text = "Arrear + Deduction should not be greater than amount";
                        this.MPE_MessageError.Show();
                        TxtTotatAmount.Value = "0";
                        Txt_Balance.Value = "Nil";
                        break;
                    }
                    else
                    {
                        Sum = Sum + (((double.Parse(gv.Cells[7].Text.ToString()) - Deduction_value) - arrieramount) + Fine_value);
                        if (Sum >= 0)
                        {
                            TxtTotatAmount.Value = Sum.ToString();
                        }
                        else
                        {
                            Lbl_MessageError.Text = "This amount is not valid";
                            this.MPE_MessageError.Show();
                            TxtTotatAmount.Value = "0";
                            Txt_Balance.Value = "Nil";
                            break;
                        }
                        try
                        {
                            if (Txt_AmountPaying.Text.Trim() == "")
                            {

                                Txt_Balance.Value = TxtTotatAmount.Value;
                            }
                            else
                            {
                                _balance = Sum - double.Parse(Txt_AmountPaying.Text);
                                Txt_Balance.Value = _balance.ToString();
                            }
                        }
                        catch
                        {
                            Txt_Balance.Value = "Nil";
                        }

                    }

                }

            }
            if (_checked && Txt_Balance.Value == "0")
            {
                Btn_payfee.Enabled = true;
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            Chk_AllFees.Checked = chkBoxAll.Checked;
            if (int.Parse(DropDownStudentId.SelectedValue.ToString()) != -1)
            {
                SELECTEDCLASSID = int.Parse(DropDownClass.SelectedValue);
                SELECTEDStudentID = int.Parse(DropDownStudentId.SelectedValue);
                Session["ClassId"] = SELECTEDCLASSID;
                LoadSuddentFee();
            }
            else
            {
                Pnl_feearea.Visible = false;
                WC_MessageBox.ShowMssage("No student in this class");
            }
        }


        protected void DropDownClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadData();
            if (MyFeeMang.HaveStudWithOutRollNoInClass(int.Parse(DropDownClass.SelectedValue.ToString()), MyUser.CurrentBatchId))
            {
                Lbl_info.Text = "Some students in " + DropDownClass.SelectedItem.Text + " are not having Roll No. Please Schedule Roll No before collecting fee for them. ";
            }
            else
            {
                Lbl_info.Text = "";
            }
            AddStudentToDropDownStudent();
            AddStudentNameToDropDownStudent();
            AddStudentIdToDropDown();

            //if (int.Parse(DropDownStudentId.SelectedValue.ToString()) != -1)
            //{
            //    SELECTEDCLASSID = int.Parse(DropDownClass.SelectedValue);
            //    SELECTEDStudentID = int.Parse(DropDownStudentId.SelectedValue);
            //    Session["ClassId"] = SELECTEDCLASSID;
            //    LoadSuddentFee();
            //}
            //else
            //{
            //    Pnl_feearea.Visible = false;
            //    WC_MessageBox.ShowMssage("No student in this class");
            //}
        }

        private void ReloadData()
        {
            Pnl_feearea.Visible = false;

        }

        protected void Drp_StudentId_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadData();
            Drp_Studname.SelectedIndex = Drp_StudentId.SelectedIndex;
            DropDownStudentId.SelectedIndex = Drp_StudentId.SelectedIndex;

            if (int.Parse(DropDownStudentId.SelectedValue.ToString()) != -1)
            {
                SELECTEDCLASSID = int.Parse(DropDownClass.SelectedValue);
                SELECTEDStudentID = int.Parse(DropDownStudentId.SelectedValue);
                Session["ClassId"] = SELECTEDCLASSID;
                LoadSuddentFee();
            }
            else
            {
                Pnl_feearea.Visible = false;
                WC_MessageBox.ShowMssage("No student in this class");
            }

        }

        protected void DropDownStudentId_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadData();
            Drp_Studname.SelectedIndex = DropDownStudentId.SelectedIndex;
            if (StudentIdMandatory())
            {
                Drp_StudentId.SelectedIndex = DropDownStudentId.SelectedIndex;
            }

            if (int.Parse(DropDownStudentId.SelectedValue.ToString()) != -1)
            {
                SELECTEDCLASSID = int.Parse(DropDownClass.SelectedValue);
                SELECTEDStudentID = int.Parse(DropDownStudentId.SelectedValue);
                Session["ClassId"] = SELECTEDCLASSID;
                LoadSuddentFee();
            }
            else
            {
                Pnl_feearea.Visible = false;
                WC_MessageBox.ShowMssage("No student in this class");
            }
        }

        protected void Drp_Studname_SelectedIndexChanged(object sender, EventArgs e)
        {
            Chk_AllFees.Checked = chkBoxAll.Checked;
            ReloadData();
            DropDownStudentId.SelectedIndex = Drp_Studname.SelectedIndex;
            if (StudentIdMandatory())
            {
                Drp_StudentId.SelectedIndex = Drp_Studname.SelectedIndex;
            }
            if (int.Parse(DropDownStudentId.SelectedValue.ToString()) != -1)
            {
                SELECTEDCLASSID = int.Parse(DropDownClass.SelectedValue);
                SELECTEDStudentID = int.Parse(DropDownStudentId.SelectedValue);
                Session["ClassId"] = SELECTEDCLASSID;
                LoadSuddentFee();
            }
            else
            {
                Pnl_feearea.Visible = false;
                WC_MessageBox.ShowMssage("No student in this class");
            }
        }

        protected void Drp_Tax_SelectedIndexChanged(object sender, EventArgs e)
        {
            double feeAmnt = double.Parse(TxtTotatAmount.Value);
            double taxPercentage = 0;
            double taxAmnt = 0;
            double totalAmnt = 0;
            if (int.Parse(Drp_Tax.SelectedValue) != 0)
            {
                taxPercentage = GetTaxPercentage(Drp_Tax.SelectedValue);
                taxAmnt = (feeAmnt * taxPercentage) / 100;
                // string taxamount = taxAmnt.ToString();
                Txt_TaxAmnt.Value = taxAmnt.ToString();
                totalAmnt = feeAmnt + taxAmnt;
                TxtTotatAmount.Value = totalAmnt.ToString();
                Txt_Balance.Value = totalAmnt.ToString();
            }


        }
        private double GetTaxPercentage(string SelectedValue)
        {
            double taxpercentage = 0;
            string sql = "SELECT tbltaxconfig.TaxPercentage from tbltaxconfig where tbltaxconfig.Id=" + SelectedValue + "";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                taxpercentage = double.Parse(MyReader.GetValue(0).ToString());
            }
            return taxpercentage;
        }
        private void LoadSuddentFee()
        {
            Txt_Pay_Date.Enabled = false;
            if (MyFeeMang.CanChangeFeeBillDate())
            {
                Txt_Pay_Date.Enabled = true;
            }

            DateTime _now = System.DateTime.Now;
            Txt_Pay_Date.Text = MyUser.GerFormatedDatVal(_now);

            Txt_OtherReference.Text = "";
            TxtTotatAmount.Value = "0";
            TxtBillNo.Text = "0";
            LoadTax();
            LoadStudentFeeAdvance();
            LoadTransportationRoute();
            AddfeeduesToGrid();
            clearDatas();
            Checkallfee();
            ClaculateTotalAmount();



        }
        private void LoadTax()
        {
            string sql = "SELECT tblconfiguration.Value from tblconfiguration where tblconfiguration.Id = 85 and tblconfiguration.Name='Tax'";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int val = int.Parse(MyReader.GetValue(0).ToString());
                if (val == 1)
                {
                    tax_area.Visible = true;
                    tax_amnt.Visible = true;
                    Drp_Tax.Items.Clear();
                    sql = " SELECT tbltaxconfig.Id,tbltaxconfig.TaxName from tbltaxconfig where tbltaxconfig.IsActive=1";
                    MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        ListItem li = new ListItem("Select Tax", "0");
                        Drp_Tax.Items.Add(li);
                        while (MyReader.Read())
                        {

                            li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                            Drp_Tax.Items.Add(li);
                        }

                    }
                    else
                    {
                        ListItem li = new ListItem("No Tax found", "-1");
                        Drp_Tax.Items.Add(li);
                    }
                    MyReader.Close();
                }
            }
            MyReader.Close();
        }

        private void LoadTransportationRoute()
        {
            Label_TransportationMsg.Visible = false;
            Label_TransportationRoute.Visible = false;
            if (MyUser.HaveModule(26))
            {
                Label_TransportationMsg.Visible = true;
                Label_TransportationRoute.Visible = true;
                Label_TransportationRoute.Text = "Not Assigned";

                string sql = "SELECT tbl_tr_route.RouteName FROM tbl_tr_studtripmap INNER JOIN tbl_tr_routedestinations ON tbl_tr_routedestinations.DestinationId=tbl_tr_studtripmap.DestinationId INNER JOIN tbl_tr_route ON tbl_tr_routedestinations.RouteId=tbl_tr_route.Id WHERE tbl_tr_studtripmap.StudId=" + SELECTEDStudentID;
                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    if (MyReader.GetValue(0).ToString().Trim() != "")
                    {
                        Label_TransportationRoute.Text = MyReader.GetValue(0).ToString().Trim();
                    }
                }

            }
        }

        private void LoadStudentFeeAdvance()
        {
            string sql = "select Sum(tblstudentfeeadvance.Amount)  from tblstudentfeeadvance where tblstudentfeeadvance.StudentId=" + SELECTEDStudentID;
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            double _total = 0;
            if (MyReader.HasRows)
            {
                if (!double.TryParse(MyReader.GetValue(0).ToString(), out _total))
                {
                    _total = 0;
                }


            }
            Label_TotalAdvance.Text = _total.ToString();
        }


        private void LoadpupilTopData()
        {
            int _StudentId = int.Parse(DropDownStudentId.SelectedValue.ToString());
            if ((Request.QueryString["StudentId"] != null))
            {

                _StudentId = int.Parse(Request.QueryString["StudentId"].ToString());
            }

            StudentManagerClass MyStudMang = MyUser.GetStudentObj();
            //  string _Studstrip = MyStudMang.ToStripString(SELECTEDStudentID, MyUser.GetImageUrl("StudentImage", _StudentId), 1);
            string _Studstrip = MyStudMang.ToStripString(SELECTEDStudentID, "Handler/ImageReturnHandler.ashx?id=" + _StudentId + "&type=StudentImage", 1);

            this.StudentTopStrip.InnerHtml = _Studstrip;
        }


        private void clearDatas()
        {
            Txt_Balance.Value = "Nil";
            Txt_AmountPaying.Text = "0";
            TxtTotatAmount.Value = "0";
            Txt_bank.Text = "";
            Txt_paymentid.Text = "";
            Rdb_PaymentSelect(0);
            //Pnl_paymod.Visible = false;
            Lbl_FeeBillMessage.Text = "";
        }

        private void Rdb_PaymentSelect(int mode)
        {

            RadioButton0.Checked = false;
            RadioButton1.Checked = false;
            RadioButton2.Checked = false;
            RadioButton3.Checked = false;

            if (mode == 0)
            {
                RadioButton0.Checked = true;
            }
            else if (mode == 1)
            {
                RadioButton1.Checked = true;
            }
            else if (mode == 2)
            {
                RadioButton2.Checked = true;
            }
            else
            {
                RadioButton3.Checked = true;
            }
        }

        private string Rdb_PaymentSelectvalue()
        {
            if (RadioButton0.Checked)
            {
                return "0";
            }
            else if (RadioButton1.Checked)
            {
                return "1";
            }
            else if (RadioButton2.Checked)
            {
                return "2";
            }
            else
            {
                return "3";
            }
        }

        private string Rdb_PaymentSelectText()
        {
            if (RadioButton0.Checked)
            {
                return RadioButton0.Text;
            }
            else if (RadioButton1.Checked)
            {
                return RadioButton1.Text;
            }
            else if (RadioButton2.Checked)
            {
                return RadioButton2.Text;
            }
            else
            {
                return RadioButton3.Text;
            }
        }


        private void AddfeeduesToGrid()
        {
            Panel_FeeDataArea.Visible = true;
            Panel_Complete.Visible = true;
            Lbl_FeeBillMessage.Text = "";
            GridViewAllFee.Columns[1].Visible = true;
            GridViewAllFee.Columns[2].Visible = true;
            //GridViewAllFee.Columns[12].Visible = true;
            GridViewAllFee.Columns[13].Visible = true;
            GridViewAllFee.Columns[14].Visible = true;
            GridViewAllFee.Columns[15].Visible = true;
            GridViewAllFee.Columns[16].Visible = true;
            GridViewAllFee.Columns[17].Visible = true;
            GridViewAllFee.Columns[18].Visible = true;
            BtnBill.Enabled = false;
            Btn_payfee.Enabled = false;
            string sql;
            if (Chk_AllFees.Checked)
            {

                sql = "select tblfeestudent.SchId, tblfeestudent.Id, tblfeeaccount.AccountName,tblbatch.BatchName, tblperiod.Period, tblfeestudent.Status, tblfeestudent.BalanceAmount,date_format( tblfeeschedule.LastDate , '%d-%m-%Y') AS 'LastDate' , 1 as Regular , 1 as CollectionType , tblperiod.Id as PeriodId , tblfeeaccount.Id as FeeId , tblbatch.Id as BatchId,date_format( tblfeeschedule.Duedate , '%d-%m-%Y') AS 'Duedate' from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblstudent on tblstudent.Id = tblfeestudent.StudId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId where tblfeestudent.StudId=" + SELECTEDStudentID + " and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion' and tblstudent.Status=1 and tblfeestudent.BalanceAmount>0  ORDER BY tblfeeschedule.Duedate";
            }
            else
            {
                sql = "select tblfeestudent.SchId, tblfeestudent.Id, tblfeeaccount.AccountName,tblbatch.BatchName, tblperiod.Period, tblfeestudent.Status, tblfeestudent.BalanceAmount,date_format( tblfeeschedule.LastDate , '%d-%m-%Y') AS 'LastDate' , 1 as Regular , 1 as CollectionType , tblperiod.Id as PeriodId , tblfeeaccount.Id as FeeId , tblbatch.Id as BatchId,date_format( tblfeeschedule.Duedate , '%d-%m-%Y') AS 'Duedate'  from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblstudent on tblstudent.Id = tblfeestudent.StudId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId where tblfeestudent.StudId=" + SELECTEDStudentID + " and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion' and tblfeeschedule.DueDate <= CURRENT_DATE() and tblstudent.Status=1  and tblfeestudent.BalanceAmount>0 ORDER BY tblperiod.Order";

            }
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                GridViewAllFee.DataSource = MyReader;
                GridViewAllFee.DataBind();
                Pnl_feearea.Visible = true;
                Panel_Complete.Visible = false;
                LoadpupilTopData();
                PanelSelect.Visible = false;
                PanelStudent.Visible = true;
                Panel_FeeDataArea.Visible = true;
                CalculateFineAmount();
            }
            else
            {

                GridViewAllFee.DataSource = null;
                GridViewAllFee.DataBind();
                Pnl_feearea.Visible = true;
                Panel_Complete.Visible = true;
                Panel_FeeDataArea.Visible = false;
                LoadpupilTopData();
                PanelSelect.Visible = false;
                PanelStudent.Visible = true;
                // WC_MessageBox.ShowMssage("No Fee to pay");

            }
            GridViewAllFee.Columns[1].Visible = false;
            GridViewAllFee.Columns[2].Visible = false;
            // GridViewAllFee.Columns[12].Visible = false;
            GridViewAllFee.Columns[13].Visible = false;
            GridViewAllFee.Columns[14].Visible = false;
            GridViewAllFee.Columns[15].Visible = false;
            GridViewAllFee.Columns[16].Visible = false;
            GridViewAllFee.Columns[17].Visible = false;
            GridViewAllFee.Columns[18].Visible = false;

        }

        private void CalculateFineAmount()
        {
            foreach (GridViewRow gv in GridViewAllFee.Rows)
            {
                TextBox Txt_fine = (TextBox)gv.FindControl("TxtFine");
                string sql = "";
                DateTime tdydate = System.DateTime.Now.Date;
                DateTime lastdate = new DateTime();
                double BalAmnt = 0;
                double.TryParse(gv.Cells[7].Text, out BalAmnt);
                int finetype = 0, finedate = 0, fineduration;
                double fine = 0, fineforoneday = 0;
                OdbcDataReader fineamountreader = null;
                sql = "SELECT tblfine.Amount, tblfine.Finedate, tblfine.FineAmounttype,tblfine.FineDuration from tblfine where tblfine.FeeId=" + gv.Cells[16].Text + " and tblfine.Amount>0";
                fineamountreader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (fineamountreader.HasRows)
                {
                    int.TryParse(fineamountreader.GetValue(2).ToString(), out finetype);
                    int.TryParse(fineamountreader.GetValue(1).ToString(), out finedate);
                    double.TryParse(fineamountreader.GetValue(0).ToString(), out fine);
                    int.TryParse(fineamountreader.GetValue(3).ToString(), out fineduration);
                    int diff = 0, lastday = 0, today = 0;
                    string strdate = "";
                    if (finedate == 1)
                    {
                        strdate = gv.Cells[8].Text.Replace("-", "/");

                    }
                    else if (finedate == 2)
                    {
                        strdate = gv.Cells[18].Text.Replace("-", "/");
                    }

                    lastdate = General.GetDateTimeFromText(strdate);

                    if (tdydate > lastdate)
                    {
                        double daydiff = (tdydate - lastdate).TotalDays;
                        diff = (int)daydiff;
                        lastday = lastdate.Day;
                        today = tdydate.Day;
                        int extradays = 0;
                        double Finetotal = 0;
                        //diff = (today - lastday);
                        if (diff >= 1)
                        {
                            if (finetype == 1)
                            {
                                if (diff < fineduration)
                                {
                                    fineforoneday = fine / fineduration;
                                    Finetotal = (diff * fineforoneday);
                                }
                                else
                                {
                                    Finetotal = fine;

                                }
                                Finetotal = Math.Round(Finetotal);
                                Txt_fine.Text = Finetotal.ToString();
                            }
                            else if (finetype == 2)
                            {
                                double amount = 0;
                                amount = (BalAmnt * fine / 100);
                                fineforoneday = amount / fineduration;
                                if (diff < fineduration)
                                {
                                    Finetotal = (diff * fineforoneday);
                                }
                                else
                                {
                                    Finetotal = amount;

                                }
                                Finetotal = Math.Round(Finetotal);
                                Txt_fine.Text = Finetotal.ToString();

                            }
                            else
                            {
                                if (finetype == 3)
                                {
                                    fineforoneday = fine / fineduration;
                                }
                                else if (finetype == 4)
                                {
                                    double amount = 0;
                                    amount = (BalAmnt * fine / 100);
                                    fineforoneday = amount / fineduration;
                                }
                                if ((diff % fineduration == 0) || (diff < fineduration))
                                {
                                    Finetotal = (diff * fineforoneday);
                                    Finetotal = Math.Round(Finetotal);
                                    Txt_fine.Text = Finetotal.ToString();
                                }
                                else if (diff % fineduration > 0)
                                {
                                    extradays = diff % fineduration;
                                    int days = 0;
                                    days = diff - extradays;
                                    Finetotal = (days * fineforoneday) + (fineforoneday * extradays);
                                    Finetotal = Math.Round(Finetotal);
                                    Txt_fine.Text = Finetotal.ToString();

                                }
                            }


                        }
                    }
                    else
                    {
                        Txt_fine.Text = "0";
                    }
                }
                else
                {
                    Txt_fine.Text = "0";
                }
            }
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            this.MPE_CancelBill.Show();
        }

        protected void Btn_CancelBill_Click(object sender, EventArgs e)
        {
            int _Studentid = -1;
            string PayedDate = "";
            string _Message;
            if (MyFeeMang.CancelBill(TxtBillNo.Text.Trim(), Drp_Studname.SelectedValue.ToString(), "tblfeebill", "tbltransaction", 1, out _Studentid, out PayedDate, out _Message))
            {
                Lbl_FeeBillMessage.Text = "The Bill " + TxtBillNo.Text + " is canceled";
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Cancel payment", "The billno " + TxtBillNo.Text + " for the student " + Drp_Studname.SelectedItem + " has been canceled", 1);
                MyFeeMang.WriteCancelLog(TxtBillNo.Text.Trim(), _Studentid, 1, MyUser.UserId, PayedDate, Txt_CancelReason.Text.Trim());//1 Reguler fee
                BtnBill.Enabled = false;
                LoadSuddentFee();
                BtnCancel.Enabled = false;
                Btn_payfee.Enabled = true;

            }
            else
            {
                Lbl_FeeBillMessage.Text = _Message;
                BtnBill.Enabled = true;

            }
            Txt_CancelReason.Text = "";
        }



        private void Checkallfee()
        {

            foreach (GridViewRow gv in GridViewAllFee.Rows)
            {

                CheckBox cb = (CheckBox)gv.FindControl("CheckBoxUpdate");

                cb.Checked = true;

            }

        }



        protected void Btn_payfee_Click(object sender, EventArgs e)
        {


            DateTime billdate = General.GetDateTimeFromText(Txt_Pay_Date.Text);
            if (billdate <= DateTime.Now.Date)
            {
                string sql = "";
                sql = "select value from tblconfiguration where tblconfiguration.module='Fee Collection' and tblconfiguration.Name='Advance Settlement  Needed'";
                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    if (MyReader.GetValue(0).ToString() == "1")
                    {
                        sql = "select Sum(tblstudentfeeadvance.Amount)  from tblstudentfeeadvance where tblstudentfeeadvance.StudentId=" + SELECTEDStudentID;
                        MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);

                        if (MyReader.HasRows)
                        {
                            if (MyReader.GetValue(0).ToString() == "")
                            {
                                GridViewAllFee.Columns[1].Visible = true;
                                GridViewAllFee.Columns[2].Visible = true;
                                //GridViewAllFee.Columns[12].Visible = true;
                                GridViewAllFee.Columns[13].Visible = true;
                                GridViewAllFee.Columns[14].Visible = true;
                                GridViewAllFee.Columns[15].Visible = true;
                                GridViewAllFee.Columns[16].Visible = true;
                                GridViewAllFee.Columns[17].Visible = true;
                                GridViewAllFee.Columns[18].Visible = true;
                                Btn_payfee.Enabled = false;
                                int i = 0;
                                string _BillId = "0";
                                bool Clearence = false;
                                string PrinterType = "";
                                int FeeCount = 0;
                                LoadPrinterModel(out PrinterType, out  FeeCount);
                                if (PrinterType == "1" && !FeeValidCount(FeeCount))
                                {
                                    WC_MessageBox.ShowMssage(" Printer does not support more than " + FeeCount + " fee items .Please use multiple bill");

                                    Btn_payfee.Enabled = true;
                                }
                                else if (Rdb_PaymentSelectvalue() == "1" && (Txt_paymentid.Text.Trim() == "" || Txt_bank.Text.Trim() == ""))
                                {
                                    WC_MessageBox.ShowMssage("Please Enter Cheque Details");

                                    Btn_payfee.Enabled = true;
                                }
                                else if (InValideBalance(Txt_Balance.Value))
                                {

                                    Lbl_MessageError.Text = "Balance should be Zero for paying a fee";
                                    this.MPE_MessageError.Show();
                                }
                                else if (Rdb_PaymentSelectvalue() == "2" && (Txt_paymentid.Text.Trim() == "" || Txt_bank.Text.Trim() == ""))
                                {
                                    WC_MessageBox.ShowMssage("Please Enter DD Details");

                                    Btn_payfee.Enabled = true;
                                }
                                //sai added
                                else if (Rdb_PaymentSelectvalue() == "3" && (Txt_paymentid.Text.Trim() == "" || Txt_bank.Text.Trim() == ""))
                                {
                                    WC_MessageBox.ShowMssage("Please Enter NEFT Details");

                                    Btn_payfee.Enabled = true;
                                }
                                //
                                else if (Txt_OtherReference.Text.Trim().Length > 240)
                                {
                                    WC_MessageBox.ShowMssage("Other reference can only have 240 letters");

                                    Btn_payfee.Enabled = true;
                                }
                                else
                                {
                                    try
                                    {
                                        string StudentName = GetStudentName(SELECTEDStudentID);
                                        MyFeeMang.CreateTansationDb();

                                        foreach (GridViewRow gv in GridViewAllFee.Rows)
                                        {
                                            CheckBox cb = (CheckBox)gv.FindControl("CheckBoxUpdate");

                                            if (cb.Checked)
                                            {

                                                TextBox DedTx = (TextBox)gv.FindControl("TxtDeduction");
                                                TextBox FineTxt = (TextBox)gv.FindControl("TxtFine");
                                                TextBox Arrier = (TextBox)gv.FindControl("Txtarrier");
                                                double Deduction = double.Parse(DedTx.Text.ToString());
                                                double Fine = double.Parse(FineTxt.Text.ToString());
                                                double Amount = double.Parse(gv.Cells[7].Text.ToString());
                                                double ArrierAmount = double.Parse(Arrier.Text.ToString());
                                                double Total = ((Amount - Deduction) - ArrierAmount);
                                                int ScheduledFeeId = int.Parse(gv.Cells[1].Text.ToString());
                                               
                                                  
                                               
                                                
                                                
                                                int StudentId = SELECTEDStudentID;

                                                string ClassID = SELECTEDCLASSID.ToString();
                                                string FeeName = gv.Cells[3].Text.ToString();
                                                string Period = gv.Cells[5].Text.ToString();
                                                int CollectionType = int.Parse(gv.Cells[14].Text.ToString());
                                                int PeriodId = int.Parse(gv.Cells[15].Text.ToString());
                                                int FeeId = int.Parse(gv.Cells[16].Text.ToString());
                                                int TrnsBatchId = int.Parse(gv.Cells[17].Text.ToString());
                                                int BatchId = MyUser.CurrentBatchId;
                                                string _OtherReferance = Txt_OtherReference.Text.Trim();
                                                string BillTable = "";
                                                string TransactionTable = "";
                                                if (MyFeeMang.ValidAmount(ScheduledFeeId, double.Parse(gv.Cells[7].Text.ToString()), StudentId) || ScheduledFeeId == -1)
                                                {
                                                    if (i == 0)
                                                    {
                                                        double _Total = double.Parse(TxtTotatAmount.Value.ToString());

                                                        if (!MyFeeMang.ClearenceEnabled(Rdb_PaymentSelectText()))
                                                            BillTable = "tblfeebill";
                                                        else
                                                        {
                                                            BillTable = "tblfeebillclearence";
                                                            Clearence = true;
                                                        }
                                                        if (MyFeeMang.TaxEnabled())
                                                        {
                                                            int TaxId = int.Parse(Drp_Tax.SelectedValue);
                                                            double TaxAmnt = double.Parse(Txt_TaxAmnt.Value);
                                                            _BillId = MyFeeMang.GenBillWithTax(_Total, StudentId, Rdb_PaymentSelectText(), Txt_paymentid.Text, Txt_bank.Text, Txt_Pay_Date.Text, MyUser.UserId, MyUser.CurrentBatchId, ClassID, StudentName, 1, "", _OtherReferance, BillTable, TaxId, TaxAmnt);
                                                        }
                                                        else
                                                        {
                                                            _BillId = MyFeeMang.GenBill(_Total, StudentId, Rdb_PaymentSelectText(), Txt_paymentid.Text, Txt_bank.Text, Txt_Pay_Date.Text, MyUser.UserId, MyUser.CurrentBatchId, ClassID, StudentName, 1, "", _OtherReferance, BillTable);
                                                        }
                                                        i = 1;
                                                    }



                                                    if (_BillId != "0")
                                                    {
                                                        if (ScheduledFeeId != -1)// -1 FOR OTHER FEE
                                                        {
                                                            if (MyFeeMang.ValidTransaction(ScheduledFeeId, StudentId, Amount))
                                                            {
                                                                if (!Clearence)
                                                                    TransactionTable = "tbltransaction";
                                                                else
                                                                    TransactionTable = "tbltransactionclearence";
                                                                MyFeeMang.FillTransaction(ScheduledFeeId, StudentId, Amount, Deduction, Fine, Total, ArrierAmount, _BillId, ClassID, StudentName, MyUser.UserName, TrnsBatchId, FeeName, Period, CollectionType, 1, "", TransactionTable, PeriodId, FeeId, Txt_Pay_Date.Text);
                                                            }
                                                            else
                                                                _BillId = "0";
                                                        }
                                                        else
                                                        {
                                                            if (!Clearence)
                                                                TransactionTable = "tbltransaction";
                                                            else
                                                                TransactionTable = "tbltransactionclearence";
                                                            MyFeeMang.FillTransaction(ScheduledFeeId, StudentId, Amount, Deduction, Fine, Total, ArrierAmount, _BillId, ClassID, StudentName, MyUser.UserName, TrnsBatchId, FeeName, Period, CollectionType, 1, "", TransactionTable, PeriodId, FeeId, Txt_Pay_Date.Text);
                                                        }
                                                    }



                                                }
                                                else
                                                    _BillId = "0";
                                            }

                                        }
                                        if (_BillId != "0")
                                        {
                                            int _Value = 1;
                                            string _PageName = "FeeBillSmall.aspx";
                                            bool Pdf = false;
                                            if (MyFeeMang.GetBillType(ref _Value, ref _PageName, out Pdf))
                                            {
                                                if (Pdf)
                                                {
                                                    string _ErrorMsg = "";
                                                    Pdf MyPdf = new Pdf(MyFeeMang.m_TransationDb, objSchool);
                                                    _ErrorMsg = "";
                                                    string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
                                                    string _PdfName = "";

                                                    if ((MyPdf.CreateFeeReciptPdf(_BillId, _physicalpath, out _PdfName, out _ErrorMsg, _Value)) && (!Clearence))
                                                    {
                                                        _ErrorMsg = "";
                                                        ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);

                                                    }
                                                    else if ((_PdfName == "") && (!Clearence))
                                                    {
                                                        _ErrorMsg = "Failed To Create";
                                                        _BillId = "0";

                                                    }
                                                }
                                                else
                                                {
                                                    TxtBillNo.Text = _BillId.ToString();
                                                    if ((QuickBillEnabled()) && (!Clearence))
                                                    {
                                                        string _Bill = TxtBillNo.Text.ToString();
                                                        //dominic -comment this code for testing
                                                        if (PrinterType != "1")
                                                            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"" + _PageName + "?BillNo=" + _Bill + "&BillType=0\");", true);
                                                        else
                                                        {
                                                            string _ErrorMsg = "";
                                                            Pdf MyPdf = new Pdf(MyFeeMang.m_TransationDb, objSchool);
                                                            _ErrorMsg = "";
                                                            string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
                                                            string _PdfName = "";

                                                            if (MyPdf.CreateFeeRecipt(_BillId, _physicalpath, out _PdfName, out _ErrorMsg, _Value, MyFeeMang, FeeCount))
                                                            {
                                                                _ErrorMsg = "";
                                                                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);

                                                            }
                                                            else if ((_PdfName == ""))
                                                            {
                                                                _ErrorMsg = "Failed To Create";
                                                                _BillId = "0";

                                                            }
                                                        }


                                                    }
                                                    WC_MessageBox.ShowMssage("Fee paid.Your bill number is " + _BillId + " .To see your bill,click \"View Bill\"");

                                                }
                                            }



                                        }

                                        if (_BillId != "0")
                                        {
                                            MyFeeMang.EndSucessTansationDb();
                                            int StudentId = SELECTEDStudentID;
                                            StoreIncident(_BillId, StudentId);
                                            StoreSMS(_BillId, StudentId);
                                        }
                                        else
                                        {
                                            MyFeeMang.EndFailTansationDb();
                                        }
                                        LoadSuddentFee();
                                        if ((_BillId != "0") && (!Clearence))
                                        {
                                            TxtBillNo.Text = _BillId.ToString();

                                            Lbl_FeeBillMessage.Text = "Fee paid.Your bill number is " + _BillId + " .To see your bill,click \"View Bill\"";
                                            MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Collect Fee", "Fee is collected for billno " + _BillId, 1,1);
                                            BtnBill.Enabled = true;
                                            BtnCancel.Enabled = true;
                                        }
                                        else if ((_BillId != "0") && (Clearence))
                                        {
                                            Lbl_FeeBillMessage.Text = "Your transaction is waiting for clearence. Get the bill after clearence";
                                            BtnBill.Enabled = false;
                                            BtnCancel.Enabled = false;
                                        }
                                        else
                                        {
                                            Lbl_MessageError.Text = "Payment is not completed please try again...";
                                            this.MPE_MessageError.Show();
                                        }

                                        Pnl_feearea.Visible = true;

                                        //Lnk_Check.Text = "None";

                                    }
                                    catch (Exception Ex)
                                    {
                                        MyFeeMang.EndFailTansationDb();
                                        Lbl_MessageError.Text = Ex.Message;
                                        this.MPE_MessageError.Show();
                                        Btn_payfee.Enabled = true;
                                    }

                                }
                                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "Rdb_Changing" + Rdb_PaymentSelectvalue() + "();", true);
                                GridViewAllFee.Columns[1].Visible = false;
                                GridViewAllFee.Columns[2].Visible = false;
                                //GridViewAllFee.Columns[12].Visible = false;
                                GridViewAllFee.Columns[13].Visible = false;
                                GridViewAllFee.Columns[14].Visible = false;
                                GridViewAllFee.Columns[15].Visible = false;
                                GridViewAllFee.Columns[16].Visible = false;
                                GridViewAllFee.Columns[17].Visible = false;
                                GridViewAllFee.Columns[18].Visible = false;
                            }
                            else
                            {

                                Lbl_FeeBillMessage.Text = "Settle advance amount before collecting new amount";
                            }

                        }

                    }
                    else if (MyReader.GetValue(0).ToString() == "0")
                    {




                        GridViewAllFee.Columns[1].Visible = true;
                        GridViewAllFee.Columns[2].Visible = true;
                        //GridViewAllFee.Columns[12].Visible = true;
                        GridViewAllFee.Columns[13].Visible = true;
                        GridViewAllFee.Columns[14].Visible = true;
                        GridViewAllFee.Columns[15].Visible = true;
                        GridViewAllFee.Columns[16].Visible = true;
                        GridViewAllFee.Columns[17].Visible = true;
                        GridViewAllFee.Columns[18].Visible = true;
                        Btn_payfee.Enabled = false;
                        int i = 0;
                        string _BillId = "0";
                        bool Clearence = false;
                        string PrinterType = "";
                        int FeeCount = 0;
                        LoadPrinterModel(out PrinterType, out  FeeCount);
                        if (PrinterType == "1" && !FeeValidCount(FeeCount))
                        {
                            WC_MessageBox.ShowMssage(" Printer does not support more than " + FeeCount + " fee items .Please use multiple bill");

                            Btn_payfee.Enabled = true;
                        }
                        else if (Rdb_PaymentSelectvalue() == "1" && (Txt_paymentid.Text.Trim() == "" || Txt_bank.Text.Trim() == ""))
                        {
                            WC_MessageBox.ShowMssage("Please Enter Cheque Details");

                            Btn_payfee.Enabled = true;
                        }
                        else if (InValideBalance(Txt_Balance.Value))
                        {

                            Lbl_MessageError.Text = "Balance should be Zero for paying a fee";
                            this.MPE_MessageError.Show();
                        }
                        else if (Rdb_PaymentSelectvalue() == "2" && (Txt_paymentid.Text.Trim() == "" || Txt_bank.Text.Trim() == ""))
                        {
                            WC_MessageBox.ShowMssage("Please Enter DD Details");

                            Btn_payfee.Enabled = true;
                        }
                        //sai added
                        else if (Rdb_PaymentSelectvalue() == "3" && (Txt_paymentid.Text.Trim() == "" || Txt_bank.Text.Trim() == ""))
                        {
                            WC_MessageBox.ShowMssage("Please Enter NEFT Details");

                            Btn_payfee.Enabled = true;
                        }
                        //
                        else if (Txt_OtherReference.Text.Trim().Length > 240)
                        {
                            WC_MessageBox.ShowMssage("Other reference can only have 240 letters");

                            Btn_payfee.Enabled = true;
                        }
                        else
                        {
                            try
                            {
                                string StudentName = GetStudentName(SELECTEDStudentID);
                                MyFeeMang.CreateTansationDb();

                                foreach (GridViewRow gv in GridViewAllFee.Rows)
                                {
                                    CheckBox cb = (CheckBox)gv.FindControl("CheckBoxUpdate");

                                    if (cb.Checked)
                                    {

                                        TextBox DedTx = (TextBox)gv.FindControl("TxtDeduction");
                                        TextBox FineTxt = (TextBox)gv.FindControl("TxtFine");
                                        TextBox Arrier = (TextBox)gv.FindControl("Txtarrier");
                                        double Deduction = double.Parse(DedTx.Text.ToString());
                                        double Fine = double.Parse(FineTxt.Text.ToString());
                                        double Amount = double.Parse(gv.Cells[7].Text.ToString());
                                        double ArrierAmount = double.Parse(Arrier.Text.ToString());
                                        double Total = ((Amount - Deduction) - ArrierAmount);
                                        int ScheduledFeeId = int.Parse(gv.Cells[1].Text.ToString());
                                        
                                        int StudentId = SELECTEDStudentID;

                                        string ClassID = SELECTEDCLASSID.ToString();
                                        string FeeName = gv.Cells[3].Text.ToString();
                                        string Period = gv.Cells[5].Text.ToString();
                                        int CollectionType = int.Parse(gv.Cells[14].Text.ToString());
                                        int PeriodId = int.Parse(gv.Cells[15].Text.ToString());
                                        int FeeId = int.Parse(gv.Cells[16].Text.ToString());
                                        int TrnsBatchId = int.Parse(gv.Cells[17].Text.ToString());
                                        int BatchId = MyUser.CurrentBatchId;
                                        string _OtherReferance = Txt_OtherReference.Text.Trim();
                                        string BillTable = "";
                                        string TransactionTable = "";
                                        if (MyFeeMang.ValidAmount(ScheduledFeeId, double.Parse(gv.Cells[7].Text.ToString()), StudentId) || ScheduledFeeId == -1)
                                        {
                                            if (i == 0)
                                            {
                                                double _Total = double.Parse(TxtTotatAmount.Value.ToString());

                                                if (!MyFeeMang.ClearenceEnabled(Rdb_PaymentSelectText()))
                                                    BillTable = "tblfeebill";
                                                else
                                                {
                                                    BillTable = "tblfeebillclearence";
                                                    Clearence = true;
                                                }

                                                if (MyFeeMang.TaxEnabled())
                                                {
                                                    int TaxId = int.Parse(Drp_Tax.SelectedValue);
                                                    string TxAmnt = Txt_TaxAmnt.Value;
                                                    if(TxAmnt=="")
                                                    {
                                                        TxAmnt = "0";
                                                    }
                                                    double TaxAmnt = double.Parse(TxAmnt);
                                                   
                                                    _BillId = MyFeeMang.GenBillWithTax(_Total, StudentId, Rdb_PaymentSelectText(), Txt_paymentid.Text, Txt_bank.Text, Txt_Pay_Date.Text, MyUser.UserId, MyUser.CurrentBatchId, ClassID, StudentName, 1, "", _OtherReferance, BillTable, TaxId, TaxAmnt);

                                                    }
                                                else
                                                {
                                                    _BillId = MyFeeMang.GenBill(_Total, StudentId, Rdb_PaymentSelectText(), Txt_paymentid.Text, Txt_bank.Text, Txt_Pay_Date.Text, MyUser.UserId, MyUser.CurrentBatchId, ClassID, StudentName, 1, "", _OtherReferance, BillTable);
                                                }

                                                i = 1;

                                            }
                                            if (_BillId != "0")
                                            {
                                                if (ScheduledFeeId != -1)// -1 FOR OTHER FEE
                                                {
                                                    if (MyFeeMang.ValidTransaction(ScheduledFeeId, StudentId, Amount))
                                                    {
                                                        if (!Clearence)
                                                            TransactionTable = "tbltransaction";
                                                        else
                                                            TransactionTable = "tbltransactionclearence";
                                                        MyFeeMang.FillTransaction(ScheduledFeeId, StudentId, Amount, Deduction, Fine, Total, ArrierAmount, _BillId, ClassID, StudentName, MyUser.UserName, TrnsBatchId, FeeName, Period, CollectionType, 1, "", TransactionTable, PeriodId, FeeId, Txt_Pay_Date.Text);
                                                    }
                                                    else
                                                        _BillId = "0";
                                                }
                                                else
                                                {
                                                    if (!Clearence)
                                                        TransactionTable = "tbltransaction";
                                                    else
                                                        TransactionTable = "tbltransactionclearence";
                                                    MyFeeMang.FillTransaction(ScheduledFeeId, StudentId, Amount, Deduction, Fine, Total, ArrierAmount, _BillId, ClassID, StudentName, MyUser.UserName, TrnsBatchId, FeeName, Period, CollectionType, 1, "", TransactionTable, PeriodId, FeeId, Txt_Pay_Date.Text);
                                                }
                                            }



                                        }
                                        else
                                            _BillId = "0";
                                    }

                                }
                                if (_BillId != "0")
                                {
                                    int _Value = 1;
                                    string _PageName = "FeeBillSmall.aspx";
                                    bool Pdf = false;
                                    if (MyFeeMang.GetBillType(ref _Value, ref _PageName, out Pdf))
                                    {
                                        if (Pdf)
                                        {
                                            string _ErrorMsg = "";
                                            Pdf MyPdf = new Pdf(MyFeeMang.m_TransationDb, objSchool);
                                            _ErrorMsg = "";
                                            string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
                                            string _PdfName = "";

                                            if ((MyPdf.CreateFeeReciptPdf(_BillId, _physicalpath, out _PdfName, out _ErrorMsg, _Value)) && (!Clearence))
                                            {
                                                _ErrorMsg = "";
                                                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);

                                            }
                                            else if ((_PdfName == "") && (!Clearence))
                                            {
                                                _ErrorMsg = "Failed To Create";
                                                _BillId = "0";

                                            }
                                        }
                                        else
                                        {
                                            TxtBillNo.Text = _BillId.ToString();
                                            if ((QuickBillEnabled()) && (!Clearence))
                                            {
                                                string _Bill = TxtBillNo.Text.ToString();
                                                //dominic -comment this code for testing
                                                if (PrinterType != "1")
                                                    ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"" + _PageName + "?BillNo=" + _Bill + "&BillType=0\");", true);
                                                else
                                                {
                                                    string _ErrorMsg = "";
                                                    Pdf MyPdf = new Pdf(MyFeeMang.m_TransationDb, objSchool);
                                                    _ErrorMsg = "";
                                                    string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
                                                    string _PdfName = "";

                                                    if (MyPdf.CreateFeeRecipt(_BillId, _physicalpath, out _PdfName, out _ErrorMsg, _Value, MyFeeMang, FeeCount))
                                                    {
                                                        _ErrorMsg = "";
                                                        ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);

                                                    }
                                                    else if ((_PdfName == ""))
                                                    {
                                                        _ErrorMsg = "Failed To Create";
                                                        _BillId = "0";

                                                    }
                                                }


                                            }
                                            WC_MessageBox.ShowMssage("Fee paid.Your bill number is " + _BillId + " .To see your bill,click \"View Bill\"");

                                        }
                                    }



                                }

                                if (_BillId != "0")
                                {
                                    MyFeeMang.EndSucessTansationDb();
                                    int StudentId = SELECTEDStudentID;
                                    StoreIncident(_BillId, StudentId);
                                    StoreSMS(_BillId, StudentId);
                                }
                                else
                                {
                                    MyFeeMang.EndFailTansationDb();
                                }
                                LoadSuddentFee();
                                if ((_BillId != "0") && (!Clearence))
                                {
                                    TxtBillNo.Text = _BillId.ToString();

                                    Lbl_FeeBillMessage.Text = "Fee paid.Your bill number is " + _BillId + " .To see your bill,click \"View Bill\"";
                                    MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Collect Fee", "Fee is collected for billno " + _BillId, 1,1);
                                    BtnBill.Enabled = true;
                                    BtnCancel.Enabled = true;
                                }
                                else if ((_BillId != "0") && (Clearence))
                                {
                                    Lbl_FeeBillMessage.Text = "Your transaction is waiting for clearence. Get the bill after clearence";
                                    BtnBill.Enabled = false;
                                    BtnCancel.Enabled = false;
                                }
                                else
                                {
                                    Lbl_MessageError.Text = "Payment is not completed please try again...";
                                    this.MPE_MessageError.Show();
                                }

                                Pnl_feearea.Visible = true;

                                //Lnk_Check.Text = "None";

                            }
                            catch (Exception Ex)
                            {
                                MyFeeMang.EndFailTansationDb();
                                Lbl_MessageError.Text = Ex.Message;
                                this.MPE_MessageError.Show();
                                Btn_payfee.Enabled = true;
                            }

                        }
                        ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "Rdb_Changing" + Rdb_PaymentSelectvalue() + "();", true);
                        GridViewAllFee.Columns[1].Visible = false;
                        GridViewAllFee.Columns[2].Visible = false;
                        //GridViewAllFee.Columns[12].Visible = false;
                        GridViewAllFee.Columns[13].Visible = false;
                        GridViewAllFee.Columns[14].Visible = false;
                        GridViewAllFee.Columns[15].Visible = false;
                        GridViewAllFee.Columns[16].Visible = false;
                        GridViewAllFee.Columns[17].Visible = false;
                        GridViewAllFee.Columns[18].Visible = false;



                    }
                }


            }
            else
            {
                Lbl_FeeBillMessage.Text = "You have Selected A Future Date";
                Btn_payfee.Enabled = true;
            }


        }

        private bool FeeValidCount(int FeeCount)
        {
            int count = 0;
            foreach (GridViewRow gv in GridViewAllFee.Rows)
            {
                CheckBox cb = (CheckBox)gv.FindControl("CheckBoxUpdate");
                if (cb.Checked)
                {
                    count++;
                }
            }
            if (FeeCount > count)

                return true;
            else
                return false;

        }

        private string GetStudentName(int _SELECTEDStudentID)
        {
            string StudentName = "";
            string sql = "SELECT tblview_student.StudentName FROM tblview_student WHERE tblview_student.Id=" + _SELECTEDStudentID;
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                StudentName = MyReader.GetValue(0).ToString();
            }
            return StudentName;
        }

        private void StoreSMS(string _BillId, int StudentId)
        {
            SMSManager MySMS = MyUser.GetSMSMngObj();
            MySMS.InitClass();
            string SmsMessage = "";
            string ScheduleTime = "0";
            if (MySMS.SMS_Enabled("OnfeeCollection", out SmsMessage, out ScheduleTime))
            {

                string StudentName = "", ParentName = "", PhoneNumber = "";
                bool ParentEnabled = false;
                MySMS.GetParentDetails(StudentId, out StudentName, out ParentName, out PhoneNumber, out ParentEnabled);

                if (ParentEnabled)
                {
                    string _txtsms = SmsMessage;
                    DateTime PaidDate = new DateTime();
                    int ScheduleId = 0;
                    string FeeName = "";
                    string PeriodName = "";
                    string sql = "SELECT DISTINCT(tbltransaction.PaymentElementId),tbltransaction.PaidDate,tbltransaction.Amount,tbltransaction.FeeName,tbltransaction.PeriodName FROM tbltransaction WHERE tbltransaction.AccountTo=1 AND tbltransaction.AccountFrom=2 AND tbltransaction.BillNo='" + _BillId + "'";
                    MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        while (MyReader.Read())
                        {
                            SmsMessage = _txtsms;
                            ScheduleId = 0;
                            FeeName = "";
                            PaidDate = new DateTime();
                            int.TryParse(MyReader.GetValue(0).ToString(), out ScheduleId);
                            DateTime.TryParse(MyReader.GetValue(1).ToString(), out PaidDate);
                            string Amount = MyReader.GetValue(2).ToString();
                            FeeName = MyReader.GetValue(3).ToString();
                            PeriodName = MyReader.GetValue(4).ToString();

                            SmsMessage = SmsMessage.Replace("($Student$)", StudentName);
                            SmsMessage = SmsMessage.Replace("($Parent$)", ParentName);
                            SmsMessage = SmsMessage.Replace("($Amt$)", Amount);
                            SmsMessage = SmsMessage.Replace("($Fees$)", FeeName);
                            SmsMessage = SmsMessage.Replace("($Date$)", General.GerFormatedDatVal(PaidDate));
                            SmsMessage = SmsMessage.Replace("($BillNo$)", _BillId);
                            SmsMessage = SmsMessage.Replace("($PeriodName$)", PeriodName);


                            MySMS.AddtoAutoSMS(PhoneNumber, SmsMessage, ScheduleTime);


                        }
                    }

                }
            }
            string EmailSubject = "", EmailBody = "";
            if (Obj_Email.Email_Enabled("OnfeeCollection", out EmailSubject, out EmailBody))
            {

                string Email = "";
                bool ParentEnabled = false;
                string studentname = "", parentname = "";
                int ScheduleId = 0;
                string FeeName = "";
                string PeriodName = "";
                DateTime PaidDate = new DateTime();
                Obj_Email.GetParentDetails(StudentId, out Email, out ParentEnabled, out studentname, out parentname);
                {
                    if (ParentEnabled)
                    {
                        string sql = "SELECT DISTINCT(tbltransaction.PaymentElementId),tbltransaction.PaidDate,tbltransaction.Amount,tbltransaction.FeeName,tbltransaction.PeriodName FROM tbltransaction WHERE tbltransaction.AccountTo=1 AND tbltransaction.AccountFrom=2 AND tbltransaction.BillNo='" + _BillId + "'";
                        MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                        if (MyReader.HasRows)
                        {
                            while (MyReader.Read())
                            {

                                int.TryParse(MyReader.GetValue(0).ToString(), out ScheduleId);
                                DateTime.TryParse(MyReader.GetValue(1).ToString(), out PaidDate);
                                string Amount = MyReader.GetValue(2).ToString();
                                FeeName = MyReader.GetValue(3).ToString();
                                PeriodName = MyReader.GetValue(4).ToString();

                                EmailBody = EmailBody.Replace("($Student$)", studentname);
                                EmailBody = EmailBody.Replace("($Parent$)", parentname);
                                EmailBody = EmailBody.Replace("($Amt$)", Amount);
                                EmailBody = EmailBody.Replace("($Fees$)", FeeName);
                                EmailBody = EmailBody.Replace("($Date$)", General.GerFormatedDatVal(PaidDate));
                                EmailBody = EmailBody.Replace("($BillNo$)", _BillId);
                                EmailBody = EmailBody.Replace("($PeriodName$)", PeriodName);
                                string StdId = StudentId.ToString();
                                Obj_Email.CreateTansationDb();
                                Obj_Email.InsertDataToAutoEmailList(Email, StdId, EmailSubject, EmailBody, 2);
                                Obj_Email.EndSucessTansationDb();

                            }
                        }

                    }
                }
            }



        }


        private void StoreIncident(string _BillId, int StudentId)
        {
            DateTime LastDate = new DateTime();
            int ScheduleId = 0;
            string FeeName = "";
            string sql = "SELECT DISTINCT(tbltransaction.PaymentElementId) FROM tbltransaction WHERE tbltransaction.BillNo='" + _BillId + "'";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ScheduleId = 0;
                    FeeName = "";
                    LastDate = new DateTime();
                    int.TryParse(MyReader.GetValue(0).ToString(), out ScheduleId);
                    if (ScheduleId != -1)
                    {
                        LastDate = GetLastDateOfFeeSchedule(ScheduleId, out FeeName);
                        Incident Myincidence = MyUser.GetIncedentObj();
                        TimeSpan no_days;
                        if (LastDate.Date < DateTime.Now.Date)
                        {
                            no_days = DateTime.Now.Subtract(LastDate.Date);
                            Myincidence.Reportincident("PaidAfterDueDate", no_days.Days, no_days.Days, FeeName + "," + MyUser.GerFormatedDatVal(DateTime.Now), StudentId, "student", SELECTEDCLASSID, MyUser.CurrentBatchId, MyUser.UserId, 0, "CollectFee" + StudentId + int.Parse(DropDownClass.SelectedValue) + MyUser.CurrentBatchId + ScheduleId);
                        }
                        else if (LastDate.Date > DateTime.Now.Date)
                        {
                            no_days = LastDate.Subtract(DateTime.Now.Date);
                            Myincidence.Reportincident("PaidBeforeDueDate", no_days.Days, no_days.Days, FeeName + "," + MyUser.GerFormatedDatVal(DateTime.Now), StudentId, "student", SELECTEDCLASSID, MyUser.CurrentBatchId, MyUser.UserId, 0, "CollectFee" + StudentId + int.Parse(DropDownClass.SelectedValue) + MyUser.CurrentBatchId + ScheduleId);
                        }
                    }
                }
            }
        }

        private DateTime GetLastDateOfFeeSchedule(int ScheduleId, out string FeeName)
        {
            FeeName = "";
            DateTime LastDate = new DateTime();
            string sql = "SELECT tblfeeschedule.LastDate,tblfeeaccount.AccountName FROM tblfeeschedule INNER JOIN tblfeeaccount ON tblfeeaccount.Id=tblfeeschedule.FeeId WHERE tblfeeschedule.Id=" + ScheduleId;
            MyReader1 = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader1.HasRows)
            {
                DateTime.TryParse(MyReader1.GetValue(0).ToString(), out LastDate);
                FeeName = MyReader1.GetValue(1).ToString();
            }
            return LastDate;
        }


        private bool QuickBillEnabled()
        {
            bool _valid = false;
            string QuickBill;
            string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='QuickBill'";
            MyReader = MyFeeMang.m_TransationDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                QuickBill = MyReader.GetValue(0).ToString();
                if (QuickBill == "1")
                {
                    _valid = true;
                }
            }
            return _valid;
        }

        private bool PdfReportEnabled()
        {
            bool _valid = false;
            string PdfReport;
            string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='PdfReport'";
            MyReader = MyFeeMang.m_TransationDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                PdfReport = MyReader.GetValue(0).ToString();
                if (PdfReport == "1")
                {
                    _valid = true;
                }
            }
            return _valid;
        }



        private bool InValideBalance(string _balance)
        {
            bool _Invalid;
            double _blnce;
            _Invalid = true;
            if (double.TryParse(_balance, out _blnce))
            {
                if (_blnce == 0)
                {
                    _Invalid = false;
                }
            }

            return _Invalid;
        }

        protected void BtnBill_Click(object sender, EventArgs e)
        {
            string _Bill = TxtBillNo.Text.ToString();
            // string BillType = "0";

            MyFeeMang.CreateTansationDb();

            if (_Bill != "0")
            {
                int _Value = 1;
                string _PageName = "FeeBillSmall.aspx";
                bool Pdf = false;
                if (MyFeeMang.GetBillType(ref _Value, ref _PageName, out Pdf))
                {
                    if (Pdf)
                    {
                        string _ErrorMsg = "";
                        Pdf MyPdf = new Pdf(MyFeeMang.m_TransationDb, objSchool);
                        _ErrorMsg = "";
                        string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
                        string _PdfName = "";

                        if (MyPdf.CreateFeeReciptPdf(_Bill, _physicalpath, out _PdfName, out _ErrorMsg, _Value))
                        {
                            _ErrorMsg = "";
                            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);

                        }
                        else if (_PdfName == "")
                        {
                            _ErrorMsg = "Faild To Create";
                            _Bill = "0";
                        }
                    }
                    else
                    {
                        TxtBillNo.Text = _Bill.ToString();
                        if (QuickBillEnabled())
                        {
                            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"" + _PageName + "?BillNo=" + _Bill + "&BillType=0\");", true);
                        }
                    }
                }
            }
            if (_Bill != "0")
            {
                MyFeeMang.EndSucessTansationDb();
            }
            else
            {
                MyFeeMang.EndFailTansationDb();
            }

        }

        protected void Link_CollectFee_Click(object sender, EventArgs e)
        {

            Response.Redirect("CollectFeeAccount.aspx");


        }


        # region FeeAdvance

        protected void Lnk_Advance_Click(object sender, EventArgs e)
        {
            FeeAdvanceBox.Display();
        }

        protected void BindAdvanceFees(object sender, EventArgs e)
        {

            OtherFeeArguments _e = (OtherFeeArguments)e;
            if (_e.FeeName != "" && _e.Amount > 0)
            {
                BindFeeGrid(_e.SchId, _e.Feeid, _e.FeeName, _e.BatchName, _e.Period, _e.Status, _e.Amount, _e.LastDate, _e.Deduction, _e.Arrear, _e.Fine, _e.Regular, _e.CollectionType, _e.PeriodId, _e.FeeStudId, _e.BatchId, _e.DueDate);
                Pnl_feearea.Visible = true;
                Panel_Complete.Visible = false;
                Panel_FeeDataArea.Visible = true;
            }
            else
            {
                WC_MessageBox.ShowMssage("Error. Please try again");
            }
        }

        # endregion


        # region otherFee

        protected void Lnk_OtherFee_Click(object sender, EventArgs e)
        {

            OtherFeeBox.LoadOtherFees();
        }




        protected void BindOtherFeeItems(object sender, EventArgs e)
        {
            OtherFeeArguments _e = (OtherFeeArguments)e;
            if (_e.FeeName != "" && _e.Amount > 0)
            {
                BindFeeGrid(_e.SchId, _e.Feeid, _e.FeeName, _e.BatchName, _e.Period, _e.Status, _e.Amount, _e.LastDate, _e.Deduction, _e.Arrear, _e.Fine, _e.Regular, _e.CollectionType, _e.PeriodId, _e.FeeStudId, _e.BatchId, _e.DueDate);
                Pnl_feearea.Visible = true;
                Panel_Complete.Visible = false;
                Panel_FeeDataArea.Visible = true;
            }
            else
            {
                WC_MessageBox.ShowMssage("Error. Please try again");
            }
        }


        private void BindFeeGrid(int SchId, string FeeId, string _FeeName, string BatchName, string Period, string Status, double _Amount, string LastDate, double Deduction, double Arrear, double Fine, int Regular, int CollectionType, string PeriodId, string _FeeStudentId, string _BatchId, string DueDate)
        {
            GridViewAllFee.Columns[1].Visible = true;
            GridViewAllFee.Columns[2].Visible = true;
            //GridViewAllFee.Columns[12].Visible = true;
            GridViewAllFee.Columns[13].Visible = true;
            GridViewAllFee.Columns[14].Visible = true;
            GridViewAllFee.Columns[15].Visible = true;
            GridViewAllFee.Columns[16].Visible = true;
            GridViewAllFee.Columns[17].Visible = true;
            GridViewAllFee.Columns[18].Visible = true;
            DataSet _GridData = LoadoGridData();
            Combine(ref _GridData, SchId, FeeId, _FeeName, BatchName, Period, Status, _Amount, LastDate, Deduction, Arrear, Fine, Regular, CollectionType, PeriodId, _FeeStudentId, _BatchId, DueDate);
            GridViewAllFee.DataSource = _GridData;
            GridViewAllFee.DataBind();
            ReloadGridData(_GridData);
            ClaculateTotalAmount();
            GridViewAllFee.Columns[1].Visible = false;
            GridViewAllFee.Columns[2].Visible = false;
            //GridViewAllFee.Columns[12].Visible = false;
            GridViewAllFee.Columns[13].Visible = false;
            GridViewAllFee.Columns[14].Visible = false;
            GridViewAllFee.Columns[15].Visible = false;
            GridViewAllFee.Columns[16].Visible = false;
            GridViewAllFee.Columns[17].Visible = false;
            GridViewAllFee.Columns[18].Visible = false;
        }

        private void ReloadGridData(DataSet _GridData)
        {
            // UPDATING THE CHECK BOX AND DISCOUNT VALUE
            int _count = 0;
            foreach (GridViewRow gvr in GridViewAllFee.Rows)
            {
                CheckBox _ChkBox = (CheckBox)gvr.FindControl("CheckBoxUpdate");
                if (_GridData.Tables[0].Rows[_count][0].ToString() == "true")
                {
                    _ChkBox.Checked = true;
                }
                else
                {
                    _ChkBox.Checked = false;
                }
                TextBox Tbox_Deduction = (TextBox)gvr.FindControl("TxtDeduction");
                TextBox Tbox_Arrier = (TextBox)gvr.FindControl("Txtarrier");
                TextBox Tbox_Fine = (TextBox)gvr.FindControl("TxtFine");
                TextBox Tbox_Amntpaid = (TextBox)gvr.FindControl("Txtamntpaid");

                Tbox_Deduction.Text = _GridData.Tables[0].Rows[_count][9].ToString();

                Tbox_Arrier.Text = _GridData.Tables[0].Rows[_count][10].ToString();
                Tbox_Fine.Text = _GridData.Tables[0].Rows[_count][11].ToString();
                Tbox_Amntpaid.Text = _GridData.Tables[0].Rows[_count][12].ToString();
                if (Tbox_Amntpaid.Text == "")
                {
                    Tbox_Amntpaid.Text = _GridData.Tables[0].Rows[_count][7].ToString(); ;
                }

                // disable the text box of other fee
                if (int.Parse(_GridData.Tables[0].Rows[_count][13].ToString()) == 0)// other fee
                {
                    Tbox_Deduction.Enabled = false;
                    Tbox_Arrier.Enabled = false;
                    Tbox_Fine.Enabled = false;

                    Tbox_Amntpaid.Enabled = false;
                }
                if (int.Parse(_GridData.Tables[0].Rows[_count][13].ToString()) == 2)// other fee
                {
                    Tbox_Deduction.Enabled = false;
                    Tbox_Arrier.Enabled = false;
                    Tbox_Fine.Enabled = false;
                    Tbox_Amntpaid.Enabled = false;

                }

                _count++;
            }

        }

        private void Combine(ref DataSet _GridData, int SchId, string FeeId, string _FeeName, string BatchName, string Period, string Status, double _Amount, string LastDate, double Deduction, double Arrear, double Fine, int Regular, int _CollectionType, string _PeriodId, string _FeeStudentId, string _BatchId, string _DueDate)
        {
            DataRow _Dr;
            _Dr = _GridData.Tables["OtherRegularFee"].NewRow();
            _Dr["CkkBox"] = "true";
            _Dr["SchId"] = SchId;
            _Dr["Id"] = _FeeStudentId;
            _Dr["AccountName"] = _FeeName;
            _Dr["BatchName"] = BatchName;
            _Dr["Period"] = Period;
            _Dr["Status"] = Status;
            _Dr["BalanceAmount"] = _Amount;
            _Dr["LastDate"] = LastDate;
            _Dr["Deduction"] = Deduction;
            _Dr["Arrear"] = Arrear;
            _Dr["Fine"] = Fine;
            _Dr["Regular"] = Regular;
            _Dr["CollectionType"] = _CollectionType;
            _Dr["PeriodId"] = _PeriodId;
            _Dr["FeeId"] = FeeId;
            _Dr["BatchId"] = _BatchId;
            _Dr["Duedate"] = _DueDate;
            _GridData.Tables["OtherRegularFee"].Rows.Add(_Dr);
        }

        private DataSet LoadoGridData()
        {
            DataRow _Dr;
            DataTable dt;
            DataSet _DataSet = new DataSet();
            _DataSet.Tables.Add(new DataTable("OtherRegularFee"));
            dt = _DataSet.Tables["OtherRegularFee"];

            dt.Columns.Add("CkkBox");
            dt.Columns.Add("SchId");
            dt.Columns.Add("Id");
            dt.Columns.Add("AccountName");
            dt.Columns.Add("BatchName");
            dt.Columns.Add("Period");
            dt.Columns.Add("Status");
            dt.Columns.Add("BalanceAmount");
            dt.Columns.Add("LastDate");
            dt.Columns.Add("Deduction");
            dt.Columns.Add("Arrear");
            dt.Columns.Add("Fine");
            dt.Columns.Add("AmountPaid");
            dt.Columns.Add("Regular");
            dt.Columns.Add("CollectionType");
            dt.Columns.Add("PeriodId");
            dt.Columns.Add("FeeId");
            dt.Columns.Add("BatchId");
            dt.Columns.Add("Duedate");


            foreach (GridViewRow gvr in GridViewAllFee.Rows)
            {
                _Dr = dt.NewRow();
                CheckBox _ChkBox = (CheckBox)gvr.FindControl("CheckBoxUpdate");
                if (_ChkBox.Checked)
                {
                    _Dr["CkkBox"] = "true";
                }
                else
                {
                    _Dr["CkkBox"] = "false";
                }
                _Dr["SchId"] = gvr.Cells[1].Text;
                _Dr["Id"] = gvr.Cells[2].Text;
                _Dr["AccountName"] = gvr.Cells[3].Text;
                _Dr["BatchName"] = gvr.Cells[4].Text;
                _Dr["Period"] = gvr.Cells[5].Text;
                _Dr["Status"] = gvr.Cells[6].Text;
                _Dr["BalanceAmount"] = gvr.Cells[7].Text;
                _Dr["LastDate"] = gvr.Cells[8].Text;
                _Dr["Regular"] = gvr.Cells[13].Text;
                _Dr["CollectionType"] = gvr.Cells[14].Text;
                _Dr["PeriodId"] = gvr.Cells[15].Text;
                _Dr["FeeId"] = gvr.Cells[16].Text;
                _Dr["BatchId"] = gvr.Cells[17].Text;
                _Dr["Duedate"] = gvr.Cells[18].Text;
                TextBox Tbox_Deduction = (TextBox)gvr.FindControl("TxtDeduction");
                TextBox Tbox_Arrier = (TextBox)gvr.FindControl("Txtarrier");
                TextBox Tbox_Fine = (TextBox)gvr.FindControl("TxtFine");
                TextBox Tbox_Addition = (TextBox)gvr.FindControl("TxtAddition");
                TextBox Tbox_amntpaid = (TextBox)gvr.FindControl("Txtamntpaid");
                _Dr["Deduction"] = Tbox_Deduction.Text;
                _Dr["Arrear"] = Tbox_Arrier.Text;
                _Dr["Fine"] = Tbox_Fine.Text;
                _Dr["AmountPaid"] = Tbox_amntpaid.Text;
                _DataSet.Tables["OtherRegularFee"].Rows.Add(_Dr);
            }
            return _DataSet;

        }

        # endregion

        protected void Btn_advanceSettelment_Click(object sender, EventArgs e)
        {
            WC_SETTLEADVANCE.Display(SELECTEDStudentID);
        }

        protected void AdvanceFeeCanclled(object sender, EventArgs e)
        {
            LoadSuddentFee();
        }

        protected void Chk_AllFees_CheckedChanged(object sender, EventArgs e)
        {
            LoadSuddentFee();
        }
    }
}
