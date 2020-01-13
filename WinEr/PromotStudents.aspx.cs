using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;
using WinEr;
using WinBase;
public partial class PromotStudents : System.Web.UI.Page
{
    private ConfigManager MyConfiMang;
    private DataSet MydataSet;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    private FeeManage MyFeeMang;
    private StudentManagerClass MyStudMang;
    protected void Page_PreInit(Object sender, EventArgs e)
    {

        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        else
        {
            MyUser = (KnowinUser)Session["UserObj"];

            if (MyUser.SELECTEDMODE == 1)
            {
                this.MasterPageFile = "~/WinerStudentMaster.master";

            }
            else if (MyUser.SELECTEDMODE == 2)
            {

                this.MasterPageFile = "~/WinerSchoolMaster.master";
            }
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyFeeMang = MyUser.GetFeeObj();
        MyConfiMang = MyUser.GetConfigObj();
        MyStudMang = MyUser.GetStudentObj();
        if (MyConfiMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }
        else if (!MyUser.HaveActionRignt(31))
        {
            Response.Redirect("RoleErr.htm");
        }
        else
        {
            if (!IsPostBack)
            {
               
                LoadFromClassToList();
                LoadTOClassToList();
                LoadGrid();
                CheckAll();
            }
        }
    }

    protected void Grd_Protionlist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string value = e.Row.Cells[3].Text;
            if (value == "-1")
            {
                e.Row.Cells[3].Text = "No Roll No.";
            }
        }
    }

    private void LoadGrid()
    {

        Lnk_select.Text = "Select All";
        string sql = "select tblstudent.Id,tblstudent.StudentName,tblstudentclassmap_history.RollNo FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id inner join tblbatch on tblbatch.Status=1  where tblstudent.Status=1 AND tblstudentclassmap_history.ClassId=" + int.Parse(Drp_ClassFrom.SelectedValue.ToString()) + " AND tblstudentclassmap_history.BatchId=tblbatch.LastbatchId AND tblstudent.Id NOt IN (Select tblstudentclassmap.StudentId from tblstudentclassmap where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " ) Order by tblstudentclassmap_history.RollNo ASC";
        MydataSet = MyConfiMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
            Grd_Protionlist.Columns[0].Visible = true;
            Grd_Protionlist.DataSource = MydataSet;
            Grd_Protionlist.DataBind();
            Grd_Protionlist.Columns[0].Visible = false;
            Pnl_Dataarea.Visible = true;
            Btn_Promote.Enabled = true;
            Btn_History.Enabled = true;
            Lbl_note.Text = "";

        }
        else
        {
            Grd_Protionlist.DataSource = null;
            Grd_Protionlist.DataBind();
            Btn_Promote.Enabled = false;
            Btn_History.Enabled = false;
            Pnl_Dataarea.Visible = false;

            Lbl_note.Text = "No Students from " + Drp_ClassFrom.SelectedItem.Text + " are available for promotion to current batch";
            //this.MPE_MessageBox.Show();
        }
    }

    private void LoadTOClassToList()
    {
        Drp_ClassTo.Items.Clear();
        string sql = "Select tblclass.Id,tblclass.ClassName from tblclass WHERE tblclass.Status=1 and tblclass.Standard IN (select tblpromotionmap.StdTo from tblpromotionmap inner join tblclass on tblclass.Standard=tblpromotionmap.StdFrom where tblclass.Id=" + int.Parse(Drp_ClassFrom.SelectedValue.ToString()) + ")";

        MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                Drp_ClassTo.Items.Add(li);

            }

        }
        else
        {
            ListItem li = new ListItem("No Class present", "-1");
            Drp_ClassTo.Items.Add(li);
        }
        Drp_ClassTo.SelectedIndex = 0;
    }

    private void LoadFromClassToList()
    {

        Drp_ClassFrom.Items.Clear();

        MydataSet = MyUser.MyAssociatedClass();
        if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr in MydataSet.Tables[0].Rows)
            {

                ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                Drp_ClassFrom.Items.Add(li);

            }

        }
        else
        {
            ListItem li = new ListItem("No Class present", "-1");
            Drp_ClassFrom.Items.Add(li);
        }
        Drp_ClassFrom.SelectedIndex = 0;

    }

    protected void Drp_ClassFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadTOClassToList();
        LoadGrid();
        CheckAll();
    }

    protected void Btn_Promote_Click(object sender, EventArgs e)
    {
        Lbl_note.Text = "";

        if (Drp_ClassFrom.SelectedValue == "-1")
        {
            Lbl_msg.Text = "No Class is selected";
            this.MPE_MessageBox.Show();
        }
        else
        {
            DoPromotion();
        }
    }

    protected void Btn_History_Click(object sender, EventArgs e)
    {
        
        //CLogging logger = CLogging.GetLogObject();
        try
        {
           
            foreach (GridViewRow gv in Grd_Protionlist.Rows)
            {
                CheckBox cb = (CheckBox)gv.FindControl("Chh_Stud");
                if (cb.Checked)
                {
       
                        MoveStudenttoHistory(int.Parse(gv.Cells[0].Text.ToString()));
                        MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Student Moved to History", gv.Cells[2].Text + " has been transfered to history", 1, 1);
                   
                }
            }
            MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Students Moved to history", "Students moved from manual promotion to history From class" + Drp_ClassFrom.SelectedItem.Text.ToString() + "", 1, 1);
            Lbl_msg.Text = "Selected Students Moved to History";
            this.MPE_MessageBox.Show();
        }
        catch
        {
            
            Lbl_msg.Text = "Failure,Please try again!!!";
            this.MPE_MessageBox.Show();
        }

    }

    private void MoveStudenttoHistory(int _StudentId)
    {

        MyStudMang.CreateTansationDb();
        MyStudMang.StoreIncedentCalcualtion(_StudentId, MyUser.CurrentBatchId);
        MyStudMang.MoveStudentIncidentToHistory(_StudentId);
        MyStudMang.MoveStudentToHistory(_StudentId, MyUser.CurrentBatchId, 3);
       
        MyStudMang.UpdateInventoryDetails(_StudentId);
        MyStudMang.EndSucessTansationDb();

    }

    private void DoPromotion()
    {
        int flag = 0;

        try
        {
            MyConfiMang.CreateTansationDb();
            foreach (GridViewRow gv in Grd_Protionlist.Rows)
            {
                CheckBox cb = (CheckBox)gv.FindControl("Chh_Stud");
                if (cb.Checked)
                {
                    flag = 1;
                    DropDownList Drpresult = (DropDownList)gv.FindControl("Drp_result");
                    if (Drp_ClassTo.SelectedValue == "-1")
                    {

                        if (Drpresult.SelectedValue != "1")//fail
                        {
                            MyConfiMang.AddTONewBatch(int.Parse(gv.Cells[0].Text.ToString()), MyUser.CurrentBatchId, int.Parse(Drp_ClassFrom.SelectedValue.ToString()),0);
                         
                        }

                    }
                    else
                    {
                        if (Drpresult.SelectedValue == "1")//pass
                        {

                            MyConfiMang.AddTONewBatch(int.Parse(gv.Cells[0].Text.ToString()), MyUser.CurrentBatchId, int.Parse(Drp_ClassTo.SelectedValue.ToString()), int.Parse(Drp_ClassFrom.SelectedValue.ToString()));
                         
                        }
                        else//fail
                        {
                            MyConfiMang.AddTONewBatch(int.Parse(gv.Cells[0].Text.ToString()), MyUser.CurrentBatchId, int.Parse(Drp_ClassFrom.SelectedValue.ToString()),0);
                            
                        }
                    }

                }

            }
           
           // MyConfiMang.ScheduleFeesWhilePromotion(Drp_ClassFrom.SelectedValue, Drp_ClassTo.SelectedValue, MyUser.CurrentBatchId);
            MyConfiMang.EndSucessTansationDb();
            MyConfiMang.ScheduleRollNumber(int.Parse(Drp_ClassFrom.SelectedValue.ToString()), MyUser.CurrentBatchId);//Schedule Roll No.for Failed
            MyConfiMang.ScheduleRollNumber(int.Parse(Drp_ClassTo.SelectedValue.ToString()), MyUser.CurrentBatchId);//Schedule Roll No.for Passed
            if (flag == 0)
            {
                Lbl_msg.Text = "Please select atleast one student to promote";
                this.MPE_MessageBox.Show();
            }
            else
            {
                MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Student Promotion", "Students Promoted From " + Drp_ClassFrom.SelectedItem.Text.ToString() + " to " + Drp_ClassTo.SelectedItem.Text.ToString(), 1,1);

                Lbl_msg.Text = "The selected Students are promoted";
                this.MPE_MessageBox.Show();
            }
        }
        catch
        {
            MyConfiMang.EndFailTansationDb();
            Lbl_msg.Text = "Promotion failed. Please try again";
            this.MPE_MessageBox.Show();
        }

        LoadGrid();

    }

    protected void Btn_magok_click(object sender, EventArgs e)
    {
        
        LoadGrid();
        CheckAll();
    }

    protected void Lnk_select_Click(object sender, EventArgs e)
    {
        CheckAll();

    }

    private void CheckAll()
    {
        if (Lnk_select.Text == "Select All")
        {
            foreach (GridViewRow gv in Grd_Protionlist.Rows)
            {
                CheckBox cb = (CheckBox)gv.FindControl("Chh_Stud");
                cb.Checked = true;
            }
            Lnk_select.Text = "None";

        }
        else
        {
            foreach (GridViewRow gv in Grd_Protionlist.Rows)
            {
                CheckBox cb = (CheckBox)gv.FindControl("Chh_Stud");
                cb.Checked = false;
            }
            Lnk_select.Text = "Select All";

        }
    }

    protected void Grd_Protionlist_SelectedIndexChanged1(object sender, EventArgs e)
    {
        int i_SelectedStudId = int.Parse(Grd_Protionlist.SelectedRow.Cells[0].Text.ToString());
        Session["StudId"] = null;
        Session["StudType"] = 2;
        Response.Redirect("IsssueCertificate.aspx?StudId=" + i_SelectedStudId);
    }

    //protected void GrdStudent_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName == "PayFee")
    //    {
    //        int StudentId = Convert.ToInt32(e.CommandArgument);

    //        if (MyUser.HaveActionRignt(2))
    //        {
    //            if (FeeDue(StudentId))
    //            {
    //                LoadFeeData(StudentId);
    //                this.ModalPopupExtender1_Fee.Show();
    //            }
    //        }
    //        else
    //        {
    //            Lbl_msg.Text = "You do not have right to perorm the action. Contact administrator";
    //            this.MPE_MessageBox.Show();
    //        }
    //    }
    //}
    

    # region Fee
    private bool FeeDue(int StudentId)
    {
        bool Valid = false;
        string sql = "select tblfeestudent.BalanceAmount from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId where tblfeestudent.StudId=" + StudentId + "  and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion'";
        MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);

        if (MyReader.HasRows)
        {
            Valid = true;
        }

        return Valid;
    }

    //private void LoadFeeData(int StudentId)
    //{
    //    Hidden_StudentId.Value = StudentId.ToString();
    //    Hidden_Batch.Value = (MyUser.CurrentBatchId - 1).ToString();
    //    DateTime _now = System.DateTime.Now;

    //    // Txt_Pay_Date.Text = _now.ToString("MM-dd-yyyy");
    //    Txt_Pay_Date.Text = General.GerFormatedDatVal(_now);
    //    TxtTotatAmount.Text = "0";
    //    TxtBillNo.Text = "0";

    //    AddfeeduesToGrid();
    //    clearDatas();
    //    Checkallfee();
    //    ClaculateTotalAmount();
    //}


    //private void ClaculateTotalAmount()
    //{        
    //    double Sum = 0;
    //    double arrieramount;
    //    double Deduction_value;
    //    double Fine_value;
    //    double _balance;
    //    bool _checked = false;
    //    BtnBill.Enabled = false;
    //    TxtTotatAmount.Text = "0";
    //    try
    //    {
    //        if (Txt_AmountPaying.Text.Trim() == "")
    //        {

    //            Txt_Balance.Text = "0";
    //        }
    //        else
    //        {
    //            _balance = Sum - double.Parse(Txt_AmountPaying.Text);
    //            Txt_Balance.Text = _balance.ToString();
    //        }
    //    }
    //    catch
    //    {
    //        Txt_Balance.Text = "Nil";
    //    }

    //    Btn_payfee.Enabled = false;
    //    foreach (GridViewRow gv in GridViewAllFee.Rows)
    //    {
    //        CheckBox cb = (CheckBox)gv.FindControl("CheckBoxUpdate");
    //        if (cb.Checked)
    //        {
    //            _checked = true;
    //            TextBox Tx_dudection = (TextBox)gv.FindControl("TxtDeduction");
    //            TextBox Arrier = (TextBox)gv.FindControl("Txtarrier");
    //            TextBox Txt_fine = (TextBox)gv.FindControl("TxtFine");

    //            try
    //            {
    //                arrieramount = double.Parse(Arrier.Text.ToString());
    //            }
    //            catch
    //            {
    //                Arrier.Text = "0";
    //                arrieramount = 0;                    
    //            }
    //            try
    //            {
    //                Deduction_value = double.Parse(Tx_dudection.Text.ToString());
    //            }
    //            catch
    //            {
    //                Tx_dudection.Text = "0";
    //                Deduction_value = 0;
    //            }
    //            try
    //            {
    //                Fine_value = double.Parse(Txt_fine.Text.ToString());
    //            }
    //            catch
    //            {
    //                Txt_fine.Text = "0";
    //                Fine_value = 0;
    //            }
    //            if (double.Parse(gv.Cells[7].Text.ToString()) < Deduction_value)
    //            {
    //                Lbl_msg.Text = "Deduction should not be greater than amount";
    //                this.MPE_MessageBox.Show();
    //                TxtTotatAmount.Text = "0";
    //                Txt_Balance.Text = "Nil";
    //                break;
    //            }
    //            else if (double.Parse(gv.Cells[7].Text.ToString()) < arrieramount)
    //            {
    //                Lbl_msg.Text = "Arrear should not be greater than amount";
    //                this.MPE_MessageBox.Show();
    //                TxtTotatAmount.Text = "0";
    //                Txt_Balance.Text = "Nil";
    //                break;
    //            }
    //            else if (double.Parse(gv.Cells[7].Text.ToString()) < (arrieramount + Deduction_value))
    //            {
    //                Lbl_msg.Text = "Arrear + Deduction should not be greater than amount";
    //                this.MPE_MessageBox.Show();
    //                TxtTotatAmount.Text = "0";
    //                Txt_Balance.Text = "Nil";
    //                break;
    //            }
    //            else
    //            {
    //                Sum = Sum + (((double.Parse(gv.Cells[7].Text.ToString()) - Deduction_value) - arrieramount) + Fine_value);
    //                if (Sum >= 0)
    //                {
    //                    TxtTotatAmount.Text = Sum.ToString();
    //                }
    //                else
    //                {
    //                    Lbl_msg.Text = "This amount is not valid";
    //                    this.MPE_MessageBox.Show();
    //                    TxtTotatAmount.Text = "0";
    //                    Txt_Balance.Text = "Nil";
    //                    break;
    //                }
    //                try
    //                {
    //                    if (Txt_AmountPaying.Text.Trim() == "")
    //                    {
    //                        Txt_Balance.Text = TxtTotatAmount.Text;
    //                    }
    //                    else
    //                    {
    //                        _balance = Sum - double.Parse(Txt_AmountPaying.Text);
    //                        Txt_Balance.Text = _balance.ToString();
    //                    }
    //                }
    //                catch
    //                {
    //                    Txt_Balance.Text = "Nil";
    //                }
    //            }
    //        }            
    //    }
    //    if (_checked && Txt_Balance.Text == "0")
    //    {
    //        Btn_payfee.Enabled = true;
    //    }
    //}



    //private void Checkallfee()
    //{
    //    foreach (GridViewRow gv in GridViewAllFee.Rows)
    //    {
    //        CheckBox cb = (CheckBox)gv.FindControl("CheckBoxUpdate");
    //        cb.Checked = true;
    //    }
    //}

    //private void clearDatas()
    //{
    //    Txt_Balance.Text = "Nil";
    //    Txt_AmountPaying.Text = "0";
    //    TxtTotatAmount.Text = "0";
    //    Txt_bank.Text = "";
    //    Txt_paymentid.Text = "";
    //    Rdb_PaymentMode.SelectedValue = "0";
    //    Pnl_paymod.Visible = false;
    //    Lbl_FeeBillMessage.Text = "";
    //}

    //private void AddfeeduesToGrid()
    //{
    //    Lbl_FeeBillMessage.Text = "";
    //    GridViewAllFee.Columns[1].Visible = true;
    //    GridViewAllFee.Columns[2].Visible = true;
    //    BtnBill.Enabled = false;
    //    Btn_payfee.Enabled = false;

    //    string sql;
    //    sql = "select tblfeestudent.SchId, tblfeestudent.Id, tblfeeaccount.AccountName,tblbatch.BatchName, tblperiod.Period, tblfeestudent.Status, tblfeestudent.BalanceAmount,date_format( tblfeeschedule.LastDate , '%m-%d-%Y') AS 'LastDate' from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId= tblfeestudent.StudId inner join tblbatch on tblbatch.Id=tblfeeschedule.BatchId where tblstudentclassmap_history.StudentId=" + int.Parse(Hidden_StudentId.Value) + "  and tblstudentclassmap_history.BatchId=" + int.Parse(Hidden_Batch.Value) + " and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion'";

    //    MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);

    //    if (MyReader.HasRows)
    //    {
    //        GridViewAllFee.DataSource = MyReader;
    //        GridViewAllFee.DataBind();
    //    }
    //    else
    //    {
    //        GridViewAllFee.DataSource = null;
    //        GridViewAllFee.DataBind();
    //    }
    //    GridViewAllFee.Columns[1].Visible = false;
    //    GridViewAllFee.Columns[2].Visible = false;
    //}



    //protected void CheckBoxUpdate_CheckedChanged(object sender, EventArgs e)
    //{
    //    this.ModalPopupExtender1_Fee.Show();
    //    ClaculateTotalAmount();

    //}



    //protected void btnShow_Click(object sender, EventArgs e)
    //{
    //    this.ModalPopupExtender1_Fee.Show();
    //    LoadSuddentFee();

    //}

    //private void LoadSuddentFee()
    //{
    //    Btn_CancelBill.Visible = false;
    //    DateTime _now = System.DateTime.Now;
    //    //Txt_Pay_Date.Text = _now.ToString("MM-dd-yyyy");
    //    Txt_Pay_Date.Text = General.GerFormatedDatVal(_now);
    //    TxtTotatAmount.Text = "0";
    //    TxtBillNo.Text = "0";

    //    AddfeeduesToGrid();
    //    clearDatas();
    //    Checkallfee();
    //    ClaculateTotalAmount();

    //}

    //protected void TxtDeduction_TextChanged(object sender, EventArgs e)
    //{
    //    this.ModalPopupExtender1_Fee.Show();
    //    ClaculateTotalAmount();
    //}

    //protected void TxtFine_TextChanged(object sender, EventArgs e)
    //{
    //    this.ModalPopupExtender1_Fee.Show();
    //    ClaculateTotalAmount();

    //}
    //protected void Txtarrier_TextChanged(object sender, EventArgs e)
    //{
    //    this.ModalPopupExtender1_Fee.Show();
    //    ClaculateTotalAmount();

    //}

    //protected void Txt_AmountPaying_TextChanged(object sender, EventArgs e)
    //{
    //    this.ModalPopupExtender1_Fee.Show();
    //    BtnBill.Enabled = false;
    //    try
    //    {
    //        double _balance;
    //        if (Txt_AmountPaying.Text.Trim() == "")
    //        {

    //            Txt_Balance.Text = TxtTotatAmount.Text;
    //        }
    //        else
    //        {

    //            _balance = double.Parse(TxtTotatAmount.Text) - double.Parse(Txt_AmountPaying.Text);
    //            Txt_Balance.Text = _balance.ToString();
    //        }
    //    }
    //    catch
    //    {
    //        Txt_Balance.Text = "Nil";
    //    }

    //    if (IfCheched() && Txt_Balance.Text == "0")
    //    {
    //        Btn_payfee.Enabled = true;
    //    }
    //    else
    //    {
    //        Btn_payfee.Enabled = false;
    //    }
    //}

    //private bool IfCheched()
    //{
    //    bool _checked = false;
    //    foreach (GridViewRow gv in GridViewAllFee.Rows)
    //    {

    //        CheckBox cb = (CheckBox)gv.FindControl("CheckBoxUpdate");
    //        if (cb.Checked)
    //        {
    //            _checked = true;
    //        }
    //    }
    //    return _checked;
    //}

    //protected void Rdb_PaymentMode_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    this.ModalPopupExtender1_Fee.Show();
    //    BtnBill.Enabled = false;
    //    if (Rdb_PaymentMode.SelectedValue == "0")
    //    {
    //        Pnl_paymod.Visible = false;
    //    }
    //    else if (Rdb_PaymentMode.SelectedValue == "1")
    //    {
    //        Pnl_paymod.Visible = true;
    //        Lbl_Id.Text = "Cheque No";
    //    }
    //    else
    //    {
    //        Pnl_paymod.Visible = true;
    //        Lbl_Id.Text = "DD No";
    //    }
    //}

    //protected void Btn_payfee_Click(object sender, EventArgs e)
    //{
    //    bool Clearence = false;
    //    this.ModalPopupExtender1_Fee.Show();
    //    Btn_payfee.Enabled = false;
    //    int i = 0;
    //    string _BillId = "0";
    //    if (Rdb_PaymentMode.SelectedValue == "1" && (Txt_paymentid.Text.Trim() == "" || Txt_bank.Text.Trim() == ""))
    //    {
    //        Lbl_msg.Text = "Please Enter Check Details";
    //        this.MPE_MessageBox.Show();
    //        Btn_payfee.Enabled = true;
    //    }
    //    else if (InValideBalance(Txt_Balance.Text))
    //    {

    //        Lbl_msg.Text = "Balance should be Zero for paying a fee";
    //        this.MPE_MessageBox.Show();
    //    }
    //    else if (Rdb_PaymentMode.SelectedValue == "2" && (Txt_paymentid.Text.Trim() == "" || Txt_bank.Text.Trim() == ""))
    //    {
    //        Lbl_msg.Text = "Please Enter DD Details";
    //        this.MPE_MessageBox.Show();
    //        Btn_payfee.Enabled = true;
    //    }
    //    else
    //    {
    //        try
    //        {
    //            MyFeeMang.CreateTansationDb();

    //            foreach (GridViewRow gv in GridViewAllFee.Rows)
    //            {
    //                CheckBox cb = (CheckBox)gv.FindControl("CheckBoxUpdate");

    //                if (cb.Checked)
    //                {

    //                    TextBox DedTx = (TextBox)gv.FindControl("TxtDeduction");
    //                    TextBox FineTxt = (TextBox)gv.FindControl("TxtFine");
    //                    TextBox Arrier = (TextBox)gv.FindControl("Txtarrier");
    //                    double Deduction = double.Parse(DedTx.Text.ToString());
    //                    double Fine = double.Parse(FineTxt.Text.ToString());
    //                    double Amount = double.Parse(gv.Cells[7].Text.ToString());
    //                    double ArrierAmount = double.Parse(Arrier.Text.ToString());
    //                    double Total = ((Amount - Deduction) - ArrierAmount);
    //                    int ScheduledFeeId = int.Parse(gv.Cells[1].Text.ToString());
    //                    int StudentId = int.Parse(Hidden_StudentId.Value);
    //                    string ClassId = MyFeeMang.GetClassID(int.Parse(Hidden_StudentId.Value));
    //                    int BatchId = MyUser.CurrentBatchId;

    //                    if (i == 0)
    //                    {
    //                        double _Total = double.Parse(TxtTotatAmount.Text.ToString());

    //                        if (!MyFeeMang.ClearenceEnabled(Rdb_PaymentMode.SelectedItem.Text))
    //                        {
    //                            _BillId = MyFeeMang.GenBill(_Total, StudentId, Rdb_PaymentMode.SelectedItem.Text, Txt_paymentid.Text, Txt_bank.Text, Txt_Pay_Date.Text, MyUser.UserId, MyUser.CurrentBatchId, ClassId,gv.Cells[2].Text);
    //                        }
    //                        else
    //                        {
    //                            _BillId = MyFeeMang.GenPendingBill(_Total, StudentId, Rdb_PaymentMode.SelectedItem.Text, Txt_paymentid.Text, Txt_bank.Text, Txt_Pay_Date.Text, MyUser.UserId);
    //                            Clearence = true;
    //                        }
    //                        i = 1;

    //                    }
    //                    if (_BillId != "0")
    //                    {

    //                        if (MyFeeMang.ValidTransaction(ScheduledFeeId, StudentId, Amount))
    //                        {
    //                            if (!Clearence)
    //                            {
    //                                MyFeeMang.FillTransaction(ScheduledFeeId, StudentId, Amount, Deduction, Fine, Total, ArrierAmount, _BillId);
    //                            }
    //                            else
    //                            {
    //                                MyFeeMang.FillPendingTransaction(ScheduledFeeId, StudentId, Amount, Deduction, Fine, Total, ArrierAmount, _BillId);
    //                            }
    //                        }
    //                        else
    //                        {
    //                            _BillId = "0";

    //                        }
    //                    }
    //                }
    //            }


    //            if (_BillId != "0")
    //            {
    //                int _Value = 1;
    //                string _PageName = "FeeBillSmall.aspx";
    //                bool Pdf = false;
    //                if (MyFeeMang.GetBillType(ref _Value, ref _PageName, out Pdf))
    //                {
    //                    if (Pdf)
    //                    {
    //                        string _ErrorMsg = "";
    //                        Pdf MyPdf = new Pdf(MyFeeMang.m_TransationDb);
    //                        _ErrorMsg = "";
    //                        string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
    //                        string _PdfName = "";

    //                        if (MyPdf.CreateFeeReciptPdf(_BillId, MyUser.CurrentBatchName, MyUser.CurrentBatchId, _physicalpath, out _PdfName, out _ErrorMsg, _Value))
    //                        {
    //                            _ErrorMsg = "";
    //                            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);

    //                        }
    //                        else if (_PdfName == "")
    //                        {
    //                            Lbl_msg.Text = "Faild To Create";
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    string _Bill = _BillId;
    //                    ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"" + _PageName + "?BillNo=" + _Bill + "&BillType=0\");", true);
    //                }
    //            }
                
                
    //            //if (_BillId != "0")
    //            //{

    //            //    if (PdfReportEnabled())
    //            //    {
    //            //        string _ErrorMsg = "";
    //            //        Pdf MyPdf = new Pdf(MyFeeMang.m_TransationDb);
    //            //        _ErrorMsg = "";
    //            //        string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
    //            //        string _PdfName = "";

    //            //        if ((MyPdf.CreateFeeReciptPdf(_BillId, MyUser.CurrentBatchName, MyUser.CurrentBatchId, _physicalpath, out _PdfName, out _ErrorMsg, _Value)) && (!Clearence))
    //            //        {
    //            //            _ErrorMsg = "";
    //            //            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);

    //            //        }
    //            //        else if ((_PdfName == "") && (!Clearence))
    //            //        {
    //            //            _ErrorMsg = "Faild To Create";
    //            //            _BillId = "0";

    //            //        }
    //            //    }
    //            //    else
    //            //    {
    //            //        TxtBillNo.Text = _BillId.ToString();
    //            //        if (QuickBillEnabled())
    //            //        {
    //            //            string _Bill = TxtBillNo.Text.ToString();
    //            //            string BillType = "0";
    //            //            if ((MyFeeMang.PrinterTypeisDesk(out BillType)))
    //            //            {
    //            //                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"FeeBill.aspx?BillNo=" + _Bill + "&BillType=0\");", true);

    //            //            }//FeeBill.aspx
    //            //            else
    //            //            {
    //            //                if (BillType == "1")
    //            //                {
    //            //                    ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"FeeBillSmall.aspx?BillNo=" + _Bill + "&BillType=0\");", true);
    //            //                }
    //            //                else if (BillType == "2")
    //            //                {
    //            //                    ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"Dotprintsmallwithlogo.aspx?BillNo=" + _Bill + "&BillType=0\");", true);
    //            //                }
    //            //                else
    //            //                {
    //            //                    ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"FeeBillSmallx.aspx?BillNo=" + _Bill + "&BillType=0\");", true);
    //            //                }


    //            //            }//Fe
    //            //        }
    //            //        Lbl_msg.Text = "Fee paid.Your bill number is " + _BillId + " .To see your bill,click \"Generate Bill\"";
    //            //        this.MPE_MessageBox.Show();
    //            //    }

    //            //}



    //            if (_BillId != "0")
    //            {
    //                MyFeeMang.EndSucessTansationDb();
    //            }
    //            else
    //            {
    //                MyFeeMang.EndFailTansationDb();
    //            }
    //            LoadSuddentFee();
    //            if ((_BillId != "0") && (!Clearence))
    //            {
    //                TxtBillNo.Text = _BillId.ToString();
                   
    //                Lbl_FeeBillMessage.Text = "Fee paid.Your bill number is " + _BillId + " .To see your bill,click \"Generate Bill\"";
    //                BtnBill.Enabled = true;
    //            }
    //            else if ((_BillId != "0") && (Clearence))
    //            {
    //                Lbl_FeeBillMessage.Text = "Your transaction is waiting for clearence. Get the bill after clearence";
    //                BtnBill.Enabled = false;

    //            }
    //            else
    //            {
    //                Lbl_msg.Text = "Payment is not completed please try again...";
    //                this.MPE_MessageBox.Show();
    //            }


    //            this.MPE_MessageBox.Hide();
    //            Btn_CancelBill.Visible = true;

    //        }
    //        catch (Exception Ex)
    //        {
    //            MyFeeMang.EndFailTansationDb();
    //            Lbl_msg.Text = Ex.Message;
    //            this.MPE_MessageBox.Show();
    //            Btn_payfee.Enabled = true;
    //        }
    //    }
    //}

    //private bool QuickBillEnabled()
    //{
    //    bool _valid = false;
    //    string QuickBill;
    //    string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='QuickBill'";
    //    MyReader = MyFeeMang.m_TransationDb.ExecuteQuery(sql);
    //    if (MyReader.HasRows)
    //    {
    //        QuickBill = MyReader.GetValue(0).ToString();
    //        if (QuickBill == "1")
    //        {
    //            _valid = true;
    //        }
    //    }
    //    return _valid;
    //}

    //private bool PdfReportEnabled()
    //{
    //    bool _valid = false;
    //    string PdfReport;
    //    string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='PdfReport'";
    //    MyReader = MyFeeMang.m_TransationDb.ExecuteQuery(sql);
    //    if (MyReader.HasRows)
    //    {
    //        PdfReport = MyReader.GetValue(0).ToString();
    //        if (PdfReport == "1")
    //        {
    //            _valid = true;
    //        }
    //    }
    //    return _valid;
    //}

    //private bool InValideBalance(string _balance)
    //{
    //    bool _Invalid;
    //    double _blnce;
    //    _Invalid = true;
    //    if (double.TryParse(_balance, out _blnce))
    //    {
    //        if (_blnce == 0)
    //        {
    //            _Invalid = false;
    //        }
    //    }

    //    return _Invalid;
    //}


    //protected void Btn_CancelBill_Click(object sender, EventArgs e)
    //{
    //    this.MPE_CancelBill.Show();
    //}


    //protected void Btn_Cancel_Click(object sender, EventArgs e)
    //{
    //    int _Studentid = -1;
    //    string PayedDate = "";
    //    if (MyFeeMang.CancelBill(TxtBillNo.Text.Trim(), Hidden_StudentId.Value, "tblfeebill", "tbltransaction", 1, out _Studentid, out PayedDate))
    //    {
    //        Lbl_FeeBillMessage.Text = "The Bill " + TxtBillNo.Text + " is canceled";
    //        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Cancel payment", "The billno " + TxtBillNo.Text + " has been canceled", 1);
    //        MyFeeMang.WriteCancelLog(TxtBillNo.Text.Trim(), int.Parse(Hidden_StudentId.Value), 1, MyUser.UserId, PayedDate, Txt_CancelReason.Text.Trim());//1 Reguler fee
    //        BtnBill.Enabled = false;
    //        LoadSuddentFee();
    //        Btn_CancelBill.Enabled = false;
    //        Btn_payfee.Enabled = true;

    //    }
    //    else
    //    {
    //        Lbl_FeeBillMessage.Text = "Bill cancelation failed. Please try again";
    //        BtnBill.Enabled = true;

    //    }
    //    Txt_CancelReason.Text = "";
    //}

    //protected void BtnBill_Click(object sender, EventArgs e)
    //{
    //    this.ModalPopupExtender1_Fee.Show();

    //    string _Bill = TxtBillNo.Text.ToString();
    //    MyFeeMang.CreateTansationDb();
    //    if (_Bill != "0")
    //    {
    //        int _Value = 1;
    //        string _PageName = "FeeBillSmall.aspx";
    //        bool Pdf = false;
    //        if (MyFeeMang.GetBillType(ref _Value, ref _PageName, out Pdf))
    //        {
    //            if (Pdf)
    //            {
    //                string _ErrorMsg = "";
    //                Pdf MyPdf = new Pdf(MyFeeMang.m_TransationDb);
    //                _ErrorMsg = "";
    //                string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
    //                string _PdfName = "";

    //                if (MyPdf.CreateFeeReciptPdf(_Bill, MyUser.CurrentBatchName, MyUser.CurrentBatchId, _physicalpath, out _PdfName, out _ErrorMsg, _Value))
    //                {
    //                    _ErrorMsg = "";
    //                    ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
    //                }
    //                else if (_PdfName == "")
    //                {
    //                    Lbl_msg.Text = "Faild To Create";
    //                }
    //            }
    //        }
    //        else
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"" + _PageName + "?BillNo=" + _Bill + "&BillType=0\");", true);
    //        }
    //    }

    //    if (_Bill != "0")
    //    {
    //        MyFeeMang.EndSucessTansationDb();
    //    }
    //    else
    //    {
    //        MyFeeMang.EndFailTansationDb();
    //    }
    //}
    # endregion Fee

    protected void Grd_Protionlist_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int Studentid = int.Parse(Grd_Protionlist.Rows[e.RowIndex].Cells[0].Text.ToString());
        int _ClassId = int.Parse(Drp_ClassFrom.SelectedValue);
        int RollNo = 0;
        int.TryParse(Grd_Protionlist.Rows[e.RowIndex].Cells[3].Text.ToString(), out RollNo);

        if (MyUser.HaveActionRignt(2))
        {
            if (FeeDue(Studentid))
            {
                Response.Redirect("CollectFeeAccount.aspx?StudentId=" + Studentid);
            }
            else
            {
                Lbl_msg.Text = "No fee to pay";
                this.MPE_MessageBox.Show();
            }            
        }
        else
        {
            Lbl_msg.Text = "You do not have right to perorm the action. Contact administrator";
            this.MPE_MessageBox.Show();
        }        
    }

    protected void Grd_Protionlist_RowEditing(object sender, GridViewEditEventArgs e)
    {
        int Studentid = int.Parse(Grd_Protionlist.Rows[e.NewEditIndex].Cells[0].Text.ToString());
        Response.Redirect("StudentSessionMaker.aspx?StudentId=" + Studentid);       
    }
}