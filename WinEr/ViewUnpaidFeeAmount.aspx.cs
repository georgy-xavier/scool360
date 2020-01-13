using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

namespace WinEr
{
    public partial class ViewUnpaidFeeAmount : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private SMSManager MysmsMang;
        private OdbcDataReader MyReader = null;

        #region Events

            protected void Page_Load(object sender, EventArgs e)
            {

                if (Session["UserObj"] == null)
                {
                    Response.Redirect("sectionerr.htm");

                }
                MyUser = (KnowinUser)Session["UserObj"];
                MyFeeMang = MyUser.GetFeeObj();
                MysmsMang = MyUser.GetSMSMngObj();
                if (MysmsMang == null)
                {
                    Response.Redirect("RoleErr.htm");

                }
                if (MyFeeMang == null)
                {
                    Response.Redirect("RoleErr.htm");
                    //no rights for this user.
                }
                else if (!MyUser.HaveActionRignt(907))
                {
                    Response.Redirect("RoleErr.htm");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        DateTime _date = System.DateTime.Today;
                        string _sdate = MyUser.GerFormatedDatVal(_date);
                        LoadfeeTodrp();
                        LoadClassToDrp();
                        ListItem li = new ListItem();
                        li = new ListItem("None", "-1");
                        Drp_Period.Items.Add(li);
                        Pnl_AddedFees.Visible = false;
                        Pnl_DisplayArea.Visible = false;
                        Pnl_SmStext.Visible = false;
                    }
                }
            }

            protected void btn_excel_Click(object sender, EventArgs e)
            {
                DataSet ViewUnpaidStudentList = new DataSet();
                ViewUnpaidStudentList = (DataSet)ViewState["UnpaidList"];
                ViewUnpaidStudentList.Tables[0].Columns.Remove("Id");
                //if (!WinEr.ExcelUtility.ExportDataSetToExcel(_CountReportList, "CountReport.xls"))
                //{
                //    Lbl_msg.Text = "This function need Ms office";
                //    this.MPE_MessageBox.Show();
                //}
                string FileName = "UnpaidStudentList";
                string _ReportName = "Unpaid Student List";
                if (!WinEr.ExcelUtility.ExportDataToExcel(ViewUnpaidStudentList, _ReportName, FileName, MyUser.ExcelHeader))
                {

                    WC_MessageBox.ShowMssage("This function need Ms office");
                }

            }

            protected void Btn_Send_Click(object sender, EventArgs e)
            {
                MysmsMang.InitClass();
                string phonelist = "";
                string msg = "";
                string Message = "";
                bool Valid = true;
                string failedList = "";

                if (Data_Complete(out msg))
                {
                    Grid_Stud.Columns[0].Visible = true;
                    string _StudentId = GetStudentIdFromGrid();
                    DataSet Parents = GetParentsList(_StudentId);
                    Grid_Stud.Columns[0].Visible = false;

                    foreach (GridViewRow gv in Grid_Stud.Rows)
                    {

                        CheckBox Chk_selected = (CheckBox)gv.FindControl("CheckBoxUpdate");
                        if (Chk_selected.Checked)
                        {
                            phonelist = MysmsMang.Get_SelectedParentPhoneNo_List(gv.Cells[0].Text.ToString());
                            if (phonelist != "")
                            {
                                Message = MysmsMang.GenerateSMSstring(Txt_SmsText.Text, GetParentName(ref Parents, gv.Cells[0].Text.ToString()), gv.Cells[2].Text.ToString(), gv.Cells[5].Text.ToString(),MyUser.CurrentBatchName);
                                //dominic sms change
                                if (MysmsMang.SendBULKSms(phonelist, Message, "90366450445", "WINER", true,out  failedList))
                                {
                                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "SMS In Advanced Unpaid Fees", "Message : " + Message, 1);
                                    Valid = true;
                                }
                            }
                        }
                    }
                    if (Valid)
                        WC_MessageBox.ShowMssage("SMS sent successfully");
                    else
                        WC_MessageBox.ShowMssage("SMS sending failed. Please try again");

                }
                else
                {
                    WC_MessageBox.ShowMssage(msg);
                }

            }

            protected void Drp_Period_SelectedIndexChanged(object sender, EventArgs e)
            {
                LoadPeriod();

            }

            protected void Btn_Add_Click(object sender, EventArgs e)
            {
                Chk_DueFee.Checked = false;
                DataRow _Dr;
                DataTable dt;
                DataSet _DataSet = new DataSet();
                _DataSet.Tables.Add(new DataTable("OtherRegularFee"));
                dt = _DataSet.Tables["OtherRegularFee"];
                bool exist = true;
                int feevalue = 0;
                int.TryParse(Drp_Fee.SelectedValue, out feevalue);

                dt.Columns.Add("FeeId");
                dt.Columns.Add("ClassId");
                dt.Columns.Add("FeeName");
                dt.Columns.Add("PeriodId");
                dt.Columns.Add("PeriodName");
                foreach (GridViewRow gvr in Grd_AddedFees.Rows)
                {
                    _Dr = dt.NewRow();
                    if (Drp_Fee.SelectedValue == gvr.Cells[0].Text && Drp_Period.SelectedValue == gvr.Cells[1].Text && Drp_Class.SelectedValue == gvr.Cells[2].Text)
                    {
                        exist = false;
                    }
                    _Dr["FeeId"] = gvr.Cells[0].Text;
                    _Dr["ClassId"] = gvr.Cells[2].Text;
                    _Dr["FeeName"] = gvr.Cells[3].Text;
                    _Dr["PeriodId"] = gvr.Cells[1].Text;
                    _Dr["PeriodName"] = gvr.Cells[4].Text;
                    _DataSet.Tables["OtherRegularFee"].Rows.Add(_Dr);
                }

                if (feevalue == 0)
                {
                    WC_MessageBox.ShowMssage("Select any fee");
                }
                else
                {
                    if (exist)
                    {
                        _Dr = dt.NewRow();
                        _Dr["FeeId"] = Drp_Fee.SelectedValue;
                        _Dr["ClassId"] = Drp_Class.SelectedValue;
                        _Dr["FeeName"] = Drp_Fee.SelectedItem;
                        _Dr["PeriodId"] = Drp_Period.SelectedValue;
                        _Dr["PeriodName"] = Drp_Period.SelectedItem;
                        _DataSet.Tables["OtherRegularFee"].Rows.Add(_Dr);
                    }
                    else
                    {
                        WC_MessageBox.ShowMssage("Fee already added");
                    }


                }
                if (_DataSet != null && _DataSet.Tables[0].Rows.Count > 0)
                {
                    Pnl_AddedFees.Visible = true;
                    Pnl_DisplayArea.Visible = false;
                    Pnl_SmStext.Visible = false;
                    Grd_AddedFees.Columns[0].Visible = true;
                    Grd_AddedFees.Columns[1].Visible = true;
                    Grd_AddedFees.Columns[2].Visible = true;

                    Grd_AddedFees.DataSource = _DataSet;
                    Grd_AddedFees.DataBind();

                    Grd_AddedFees.Columns[0].Visible = false;
                    Grd_AddedFees.Columns[1].Visible = false;
                    Grd_AddedFees.Columns[2].Visible = false;
                }
                else
                {
                    Pnl_AddedFees.Visible = false;
                    Pnl_DisplayArea.Visible = false;
                    Pnl_SmStext.Visible = false;
                }

            }

            protected void Btn_Show_Click(object sender, EventArgs e)
            {
                DataRow _Dr;
                DataTable dt;
                DataSet _DataSet = new DataSet();
                _DataSet.Tables.Add(new DataTable("OtherRegularFee"));
                dt = _DataSet.Tables["OtherRegularFee"];

                int classid = 0;

                dt.Columns.Add("Id");
                dt.Columns.Add("StudentName");
                dt.Columns.Add("RollNo");
                dt.Columns.Add("ClassName");
                dt.Columns.Add("BalanceAmount");
                dt.Columns.Add("OfficePhNo");

                
                foreach (GridViewRow gr in Grd_AddedFees.Rows)
                {
                    int.TryParse(gr.Cells[2].Text, out classid);
                    if (classid == 0)
                    {
                        DataSet myDataset;
                        myDataset = MyUser.MyAssociatedClass();
                        if (myDataset != null && myDataset.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in myDataset.Tables[0].Rows)
                            {
                                int.TryParse(dr["Id"].ToString(), out classid);
                                _DataSet = GetDataset(gr.Cells[0].Text, gr.Cells[1].Text, classid, _DataSet);
                            }
                        }
                    }
                    else if (classid > 0)
                    {
                        _DataSet = GetDataset(gr.Cells[0].Text, gr.Cells[1].Text, classid, _DataSet);
                    }

                }

                if (_DataSet != null && _DataSet.Tables[0].Rows.Count > 0)
                {
                    Grid_Stud.Columns[0].Visible = true;
                    Pnl_DisplayArea.Visible = true;
                    Pnl_SmStext.Visible = true;
                    Txt_SmsText.Text = GetSMSText(4);
                    Load_Seperators();
                    Grid_Stud.DataSource = _DataSet;
                    Grid_Stud.DataBind();
                    Grid_Stud.Columns[0].Visible = false;
                }
                else
                {
                    Pnl_DisplayArea.Visible = false;
                    Pnl_SmStext.Visible = false;
                    WC_MessageBox.ShowMssage("No report found!");
                }
                ViewState["UnpaidList"] = _DataSet;
                LoadfeeTodrp();
                LoadClassToDrp();
                LoadPeriod();
                Grd_AddedFees.DataSource = null;
                Grd_AddedFees.DataBind();
                Pnl_AddedFees.Visible = false;

            }

        #endregion

        #region Methods

            private void Load_Seperators()
            {
                string innerhtml = "<table cellspacing=\"10\">";
                string sql = "SELECT `Type`,Seperator FROM tblsmsseperators where  Fees=1";
                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {

                    while (MyReader.Read())
                    {
                        innerhtml = innerhtml + "<tr style=\"height:20px\"><td>" + MyReader.GetValue(0).ToString() + " : </td> <td class=\"new\"> " + MyReader.GetValue(1).ToString() + " </td></tr> ";
                    }
                }
                innerhtml = innerhtml + "</table>";
                this.Seperators.InnerHtml = innerhtml;
            }

            private string GetSMSText(int _Option)
            {
                string _Format = "";
                string sql = "SELECT `Format` FROM tblsmsoptionconfig WHERE Id=" + _Option;
                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    _Format = MyReader.GetValue(0).ToString();
                }
                return _Format;
            }

            private string GetParentName(ref DataSet Parents, string _StudentId)
            {
                string _Parent = "0";
                foreach (DataRow Dr_Parent in Parents.Tables[0].Rows)
                {
                    if (_StudentId == Dr_Parent[0].ToString())
                    {
                        _Parent = Dr_Parent[1].ToString();
                        break;
                    }
                }
                return _Parent;
            }

            private DataSet GetParentsList(string _StudentId)
            {
                DataSet Parents = new DataSet();
                DataTable dt;
                DataRow dr;
                Parents.Tables.Add(new DataTable("ParentList"));
                dt = Parents.Tables["ParentList"];
                dt.Columns.Add("StudentId");
                dt.Columns.Add("Parent");
                string sql = "select Id,GardianName from tblstudent where Id in (" + _StudentId + ")";
                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        dr = Parents.Tables["ParentList"].NewRow();
                        dr["StudentId"] = MyReader.GetValue(0).ToString();
                        dr["Parent"] = MyReader.GetValue(1).ToString();
                        Parents.Tables["ParentList"].Rows.Add(dr);
                    }
                }
                return Parents;
            }

            private string GetStudentIdFromGrid()
            {
                string Student = "";
                foreach (GridViewRow gv in Grid_Stud.Rows)
                {
                    CheckBox Chk_selected = (CheckBox)gv.FindControl("CheckBoxUpdate");
                    if (Chk_selected.Checked)
                    {
                        if (Student == "")
                            Student = Student + gv.Cells[0].Text.ToString();
                        else
                            Student = Student + "," + gv.Cells[0].Text.ToString();
                    }
                }
                return Student;
            }

            private bool Data_Complete(out string msg)
            {
                bool valid = true;
                msg = "";
                if (Txt_SmsText.Text.Trim() == "")
                {
                    msg = "Enter SMS Message";
                    valid = false;
                }

                if (valid)
                {
                    bool _selected = false;
                    foreach (GridViewRow gv in Grid_Stud.Rows)
                    {
                        CheckBox Chk_selected = (CheckBox)gv.FindControl("CheckBoxUpdate");
                        if (Chk_selected.Checked)
                        {
                            _selected = true;
                            break;
                        }
                    }
                    if (!_selected)
                    {
                        msg = "Select student for senting SMS";
                        valid = false;
                    }
                }
                return valid;
            }


            private void LoadPeriod()
            {
                OdbcDataReader FrequencyIdReader = null, PeriodReader = null;
                Drp_Period.Items.Clear();
                ListItem li = new ListItem();
                string sql = "select tblfeeaccount.FrequencyId from tblfeeaccount where tblfeeaccount.Id=" + Drp_Fee.SelectedValue + "";
                FrequencyIdReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (FrequencyIdReader.HasRows)
                {
                    sql = "select tblperiod.Period , tblperiod.Id from tblperiod where tblperiod.FrequencyId=" + FrequencyIdReader.GetValue(0).ToString() + "";
                    PeriodReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                    if (PeriodReader.HasRows)
                    {
                        while (PeriodReader.Read())
                        {
                            li = new ListItem(PeriodReader.GetValue(0).ToString(), PeriodReader.GetValue(1).ToString());
                            Drp_Period.Items.Add(li);
                        }
                    }
                    else
                    {
                        li = new ListItem();
                        li = new ListItem("None", "-1");
                        Drp_Period.Items.Add(li);
                    }

                }
                else
                {
                    li = new ListItem();
                    li = new ListItem("None", "-1");
                    Drp_Period.Items.Add(li);
                }
            }

            private void LoadfeeTodrp()
            {
                Drp_Fee.Items.Clear();
                OdbcDataReader MyReader = null;
                string sql = "SELECT tblfeeaccount.Id, tblfeeaccount.AccountName from tblfeeaccount where tblfeeaccount.Status=1 and `Type`<>2 ORDER BY tblfeeaccount.AccountName";
                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    ListItem li;
                    li = new ListItem("Select Fee", "0");
                    Drp_Fee.Items.Add(li);
                    while (MyReader.Read())
                    {
                        li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_Fee.Items.Add(li);
                    }

                }
                else
                {
                    ListItem li = new ListItem("No Fee Found", "0");
                    Drp_Fee.Items.Add(li);
                    this.WC_MessageBox.ShowMssage("No fees found");
                    DisableAll();
                }
                MyReader.Close();

            }

            private void DisableAll()
            {
                Drp_Fee.Enabled = false;
                Drp_Class.Enabled = false;
                //Btn_Export.Enabled = false;
            }
            
            private void LoadClassToDrp()
            {
                Drp_Class.Items.Clear();

                DataSet myDataset;
                myDataset = MyUser.MyAssociatedClass();
                if (myDataset != null && myDataset.Tables != null && myDataset.Tables[0].Rows.Count > 0)
                {
                    ListItem li;
                    foreach (DataRow dr in myDataset.Tables[0].Rows)
                    {

                        li = new ListItem(dr[1].ToString(), dr[0].ToString());
                        Drp_Class.Items.Add(li);

                    }
                    li = new ListItem("All Class", "0");
                    Drp_Class.Items.Add(li);

                }
                else
                {
                    ListItem li = new ListItem("No Class Present", "-1");
                    Drp_Class.Items.Add(li);

                    this.WC_MessageBox.ShowMssage("No Class Present");
                    DisableAll();
                }

                Drp_Class.SelectedIndex = 0;
            }

            private DataSet GetDataset(string _feeid, string _periodid, int _classid, DataSet UnpaidDs)
            {
                int StudentId = 0;
                double balancetopayamt = 0;
                string sql = "",sub_sql="";
                bool valid = true;
                
                if (Chk_DueFee.Checked)
                {
                    sub_sql = sub_sql + " AND tblfeeschedule.DueDate <= CURRENT_DATE()";

                }

                sql = "SELECT Distinct tblstudent.Id , tblstudent.StudentName, tblstudentclassmap.RollNo , tblclass.ClassName,tblstudent.OfficePhNo FROM tblfeestudent  inner join tblstudentclassmap on tblfeestudent.StudId= tblstudentclassmap.StudentId inner join tblstudent ON  tblstudent.Id= tblfeestudent.StudId inner join tblclass on tblclass.Id= tblstudentclassmap.ClassId inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId where  tblstudentclassmap.BatchId= " + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND tblfeestudent.Status<>'Paid'  AND tblfeestudent.`Status`<> 'Fee Exemtion'  AND tblstudentclassmap.ClassId= " + _classid + " AND tblfeeschedule.PeriodId=" + _periodid + " AND tblfeeschedule.FeeId=" + _feeid + sub_sql+" Order by tblclass.Standard ASC ,tblstudent.LastClassId ASC ,tblstudentclassmap.RollNo ASC";
                OdbcDataReader FeeReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (FeeReader.HasRows)
                {
                    while (FeeReader.Read())
                    {
                       valid = true;
                        int.TryParse(FeeReader.GetValue(0).ToString(), out StudentId);
                        foreach (DataRow _dr in UnpaidDs.Tables[0].Rows)
                        {
                            if (StudentId.ToString() == _dr["Id"].ToString())
                            {
                                double feeamount = 0;
                                double.TryParse(_dr["BalanceAmount"].ToString(), out feeamount);
                                balancetopayamt = getbalanceamount(StudentId, _feeid, _periodid);
                                _dr["BalanceAmount"] = (feeamount + balancetopayamt).ToString();
                                valid = false;

                            }
                        }
                        if (valid)
                        {
                            balancetopayamt = getbalanceamount(StudentId, _feeid, _periodid);
                            DataRow dr = UnpaidDs.Tables[0].NewRow();
                            dr["Id"] = FeeReader.GetValue(0).ToString();
                            dr["StudentName"] = FeeReader.GetValue(1).ToString();

                            dr["RollNo"] = FeeReader.GetValue(2).ToString();
                            dr["ClassName"] = FeeReader.GetValue(3).ToString();
                            dr["BalanceAmount"] = balancetopayamt.ToString();
                            dr["OfficePhNo"] = FeeReader.GetValue(4).ToString();
                            UnpaidDs.Tables["OtherRegularFee"].Rows.Add(dr);
                        }
                    }
                }
                return UnpaidDs;
            }

            private double getbalanceamount(int StudentId, string _feeid, string _periodid)
            {
                OdbcDataReader MyReader1 = null;
                string sub_sql = "";
                if (Chk_DueFee.Checked)
                {
                    sub_sql = sub_sql + " AND tblfeeschedule.DueDate <= CURRENT_DATE()";

                }
                double _amount = 0;
                string sql = "Select sum(tblfeestudent.BalanceAmount)  from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id = tblfeestudent.SchId where tblfeestudent.StudId=" + StudentId + "  and tblfeeschedule.FeeId=" + _feeid + " AND tblfeeschedule.PeriodId=" + _periodid + sub_sql;
                MyReader1 = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader1.HasRows)
                {
                    double.TryParse(MyReader1.GetValue(0).ToString(), out _amount);
                }
                return _amount;
            }

        #endregion


       

    }
}
