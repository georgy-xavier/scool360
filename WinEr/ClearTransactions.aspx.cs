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
    public partial class ClearTransactions : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        private SchoolClass objSchool = null;
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
            else if (!MyUser.HaveActionRignt(123))
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
                    Btn_Search.Enabled = false;
                    Pnl_pending.Visible = false;
                    Drp_Name1.Visible = false;
                    lbl_clearedStudent.Visible = false;
                    Drp_Student.Visible = false;
                    lbl_selectstudent.Visible = false;
                    Pnl_View.Visible = false;
                    AddClassToDropDownClass();
                    AddClassToDropDown();
                    FillGrid();
                    FillViewGrid();
                    //some initlization

                }
            }
        }

        private void AddClassToDropDown()
        {
            Drp_Class1.Items.Clear();
            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Drp_Class1.Items.Add(new ListItem("Select any class", "-1"));
                Drp_Class1.Items.Add(new ListItem("ALL", "0"));
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class1.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Class Present", "-1");
                Drp_Class1.Items.Add(li);
            }
            Drp_Class1.SelectedIndex = 0;
        }

        private void AddClassToDropDownClass()
        {
            Drp_Class.Items.Clear();
            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Drp_Class.Items.Add(new ListItem("Select any class", "-1"));
                Drp_Class.Items.Add(new ListItem("ALL", "0"));
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Class Present", "-1");
                Drp_Class.Items.Add(li);
            }
            Drp_Class.SelectedIndex = 0;

        }

        private void AddStudentToDropDownStudent()
        {
            Drp_Student.Items.Clear();
            string sql = " SELECT DISTINCT map.StudentId,stud.StudentName FROM tblstudentclassmap map inner join tblstudent stud on stud.id= map.Studentid inner join tblfeebillclearence on tblfeebillclearence.StudentID = stud.Id  where stud.status=1";
            if (Drp_Class.SelectedValue != "0")
            {
                sql = sql + " and  map.ClassId=" + int.Parse(Drp_Class.SelectedValue.ToString()) + "";
            }
            sql = sql + " and map.RollNo<>-1 and map.BatchId=" + MyUser.CurrentBatchId + " order by map.RollNo";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_Search.Enabled = true;
                Drp_Student.Items.Add(new ListItem("ALL", "0"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Student.Items.Add(li);
                }

                FillGrid();
            }
            else
            {
                Btn_Search.Enabled = false;
                ListItem li = new ListItem("No students found", "-1");
                Drp_Student.Items.Add(li);
            }

        }

        protected void Rdb_StudentType_needClerance_SelectedIndexChanged(object sender, EventArgs e)
        {
            Drp_Student.Visible = false;
            lbl_selectstudent.Visible = false;
            Drp_Class.Enabled = false;
            if (Rdb_StudentType_needClerance.SelectedValue == "0")
            {
                Drp_Class.Enabled = true;
            }
            FillGrid();
        }

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            Drp_Student.Visible = false;
            lbl_selectstudent.Visible = false;
            if (Drp_Class.SelectedValue=="-1")
            {
                Pnl_pending.Visible = false;
            }
            else if (Drp_Class.SelectedValue != "0")
            {
                Drp_Student.Visible = true;
                lbl_selectstudent.Visible = true;
                AddStudentToDropDownStudent();
                FillGrid();
            }

            else
            {
               
                FillGrid();
            }
           
           
        }
   
        protected void Drp_Student_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillGrid();
        }

        protected void Lnk_select_Click(object sender, EventArgs e)
        {
            if (Lnk_select.Text == "None")
            {
                foreach (GridViewRow gv in Grd_Pending.Rows)
                {
                    CheckBox Box = (CheckBox)gv.FindControl("CheckBox");
                    Box.Checked = false;
                }
                Lnk_select.Text = "All";
            }
            else
            {
                foreach (GridViewRow gv in Grd_Pending.Rows)
                {
                    CheckBox Box = (CheckBox)gv.FindControl("CheckBox");
                    Box.Checked = true;
                }
                Lnk_select.Text = "None";
            }
        }

        protected void Btn_Search_Click(object sender, EventArgs e)
        {

            FillGrid();

        }

        private void FillGrid()
        {
            Lbl_ClearanceMessage.Text = "";
            string sql = "";
            if (Rdb_StudentType_needClerance.SelectedValue == "0")
            {
                sql = " select tblview_student.Id , tblfeebillclearence.BillNo ,tblview_student.StudentName, tblfeebillclearence.PaymentMode , tblfeebillclearence.PaymentModeId , tblfeebillclearence.BankName , date(tblfeebillclearence.CreatedDateTime) as CreatedDateTime from tblfeebillclearence inner join tblview_student on tblview_student.Id = tblfeebillclearence.StudentID where ";
                if ((Drp_Student.SelectedValue != "0") && (Drp_Student.Visible == true))
                {
                    sql = sql + " tblview_student.Id= " + int.Parse(Drp_Student.SelectedValue) + " and";
                }
                sql = sql + "  tblview_student.`Status`=1 ORDER BY tblfeebillclearence.BillNo";
            }
            else
            {
                sql = " select tbltempstdent.Id , tblfeebillclearence.BillNo ,tbltempstdent.Name as StudentName, tblfeebillclearence.PaymentMode , tblfeebillclearence.PaymentModeId , tblfeebillclearence.BankName , date(tblfeebillclearence.CreatedDateTime) as CreatedDateTime from tblfeebillclearence inner join tbltempstdent on tbltempstdent.TempId = tblfeebillclearence.TempId where tbltempstdent.Status=1  ORDER BY tblfeebillclearence.BillNo";
            }

            MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet!=null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_Pending.Columns[1].Visible = true;
                Grd_Pending.Columns[2].Visible = true;
                Grd_Pending.DataSource = MydataSet;
                Grd_Pending.DataBind();
                Grd_Pending.Columns[1].Visible = false;
                Grd_Pending.Columns[2].Visible = false;
                Pnl_pending.Visible = true;
            }
            else
            {
                Lbl_ClearanceMessage.Text = "No bill found for clearance";
                Grd_Pending.DataSource = null;
                Grd_Pending.DataBind();
                Pnl_pending.Visible = false;
            }

        }


        protected void Btn_ClearBill_Click(object sender, ImageClickEventArgs e)
        {
            ChkIfAnySelected(1);
        }

        protected void Img_Cancel_Click(object sender, ImageClickEventArgs e)
        {
            ChkIfAnySelected(0);            
        }

        private void ChkIfAnySelected(int ClrType)
        {
            int flg = 0;
            foreach (GridViewRow gv in Grd_Pending.Rows)
            {
                CheckBox Box = (CheckBox)gv.FindControl("CheckBox");
                if (Box.Checked)
                {
                    flg = 1;
                }
            }

            if (flg == 0)
            {
                WC_MessageBox.ShowMssage("Select any Transaction");
            }
            else
            {
                MPE_confirmDelete.Show();
                Session["ClrType"] = ClrType;
            }
        }

        protected void btn_Cnfirm_click(object sender, EventArgs e)
        {
            int flg = 0;
            string StudentId = "";
            if (int.Parse(Session["ClrType"].ToString()) == 0)
            {
                

                string BillNo = "";
                string StudentName = "";

                foreach (GridViewRow gv in Grd_Pending.Rows)
                {
                    CheckBox Box = (CheckBox)gv.FindControl("CheckBox");
                    if (Box.Checked)
                    {
                        try
                        {
                            MyFeeMang.CreateTansationDb();
                            string _TempStudId = "";
                            flg = 1;
                            BillNo = gv.Cells[2].Text.ToString();
                            StudentId = gv.Cells[1].Text.ToString();
                            StudentName = gv.Cells[3].Text.ToString();
                            if (Rdb_StudentType_needClerance.SelectedValue == "1")
                            {
                                _TempStudId = GetTempStudentId(StudentId);
                            }
                            MyFeeMang.CancelPayment(BillNo, StudentId, _TempStudId);

                            MyFeeMang.EndSucessTansationDb();
                            FillGrid();
                            WC_MessageBox.ShowMssage("Bill has been canceled successfully");
                            //Lbl_ClearanceMessage.Text = "Bill has been canceled successfully";
                            MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Cancel payment", "The billno " + BillNo + " for the student " + StudentName + " has been canceled", 1,1);
                        }
                        catch
                        {
                            MyFeeMang.EndFailTansationDb();
                            Lbl_ClearanceMessage.Text = "Error while canceling. Try later";
                            break;
                        }
                    }
                }

            }
            else
            {
                string BillNo = "";
                string BilId = "0";



                foreach (GridViewRow gv in Grd_Pending.Rows)
                {
                    CheckBox Box = (CheckBox)gv.FindControl("CheckBox");
                    if (Box.Checked)
                    {
                        try
                        {
                            MyFeeMang.CreateTansationDb();
                            BillNo = gv.Cells[2].Text.ToString();
                            StudentId = gv.Cells[1].Text.ToString();
                            BilId = MyFeeMang.ClearBill(BillNo, StudentId, MyUser.CurrentBatchId);
                            flg = 1;
                            WC_MessageBox.ShowMssage("Bill has been cleared successfully");
                            //Lbl_ClearanceMessage.Text = "Bill has been cleared successfully";
                            MyFeeMang.EndSucessTansationDb();
                            MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Transaction Clearance", "Clearence has been done for billno " + BillNo, 1,1);
                        }
                        catch
                        {
                            MyFeeMang.EndFailTansationDb();
                            Lbl_ClearanceMessage.Text = "Error while clearing. Try later";
                            break;
                        }
                    }
                }

                FillGrid();

            }
            Session["ClrType"] = null;
        }

        private string GetTempStudentId(string StudentId)
        {
            string _tempId = "";
            string sql = "SELECT tbltempstdent.TempId FROM tbltempstdent WHERE tbltempstdent.Id="+StudentId;
            MyReader = MyFeeMang.m_TransationDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _tempId = MyReader.GetValue(0).ToString();
            }
            return _tempId;
        }

        protected void RDB_StudTypeCleranceDone_SelectedIndexChanged(object sender, EventArgs e)
        {
            Drp_Name1.Visible = false;
            lbl_clearedStudent.Visible = false;
            Drp_Class1.Enabled = false;
            Rdo_Options.Visible = false;
            Pnl_View.Visible = false;
            if (RDB_StudTypeCleranceDone.SelectedValue == "0")
            {
                Drp_Class1.Enabled = true;
            }
            FillViewGrid();
        }

        protected void Drp_Class1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Drp_Name1.Visible = false;
            lbl_clearedStudent.Visible = false;
            if (Drp_Class1.SelectedValue == "-1")
            {
                Pnl_View.Visible = false;
            }
            else if (Drp_Class1.SelectedValue != "0")
            {
                Drp_Name1.Visible = true;
                lbl_clearedStudent.Visible = true;
                AddStudentToDrpList();
                Rdo_Options.Visible = true;
            }
            else
            {
                
                FillViewGrid();
                Rdo_Options.Visible = true;
            }
        }

        protected void Drp_Name1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillViewGrid();
        }

        private void AddStudentToDrpList()
        {
            Drp_Name1.Items.Clear();
            Lbl_viewMessage.Text = "";
           
            // string sql = " SELECT DISTINCT map.StudentId,stud.StudentName FROM tblstudentclassmap map inner join tblstudent stud on stud.id= map.Studentid inner join tblbillclearence on tblbillclearence.StudentId = stud.Id where stud.status=1 and  map.ClassId=" + int.Parse(Drp_Class1.SelectedValue.ToString()) + " and map.RollNo<>-1 and map.BatchId=" + MyUser.CurrentBatchId + " order by map.RollNo";
            string sql = " SELECT DISTINCT map.StudentId,stud.StudentName FROM tblstudentclassmap map inner join tblstudent stud on stud.id= map.Studentid inner join tblbillclearence on tblbillclearence.StudentID = stud.Id  where stud.status=1";
            if (Drp_Class1.SelectedValue != "0")
            {
                sql = sql + " and  map.ClassId=" + int.Parse(Drp_Class1.SelectedValue.ToString()) + "";
            }
            sql = sql + " and map.RollNo<>-1 and map.BatchId=" + MyUser.CurrentBatchId + " order by map.RollNo";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_Search.Enabled = true;
                Drp_Name1.Items.Add(new ListItem("ALL", "0"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Name1.Items.Add(li);
                }

                FillViewGrid();
            }
            else
            {
                Btn_Search.Enabled = false;
                ListItem li = new ListItem("No students found", "-1");
                Drp_Name1.Items.Add(li);
            }
        }

        protected void Btn_View_Click(object sender, EventArgs e)
        {
            Lbl_viewMessage.Text = "";
            GrdView.DataSource = null;
            GrdView.DataBind();
            if ((Rdo_Options.SelectedValue == "3")&&((Txt_Startdate.Text.Trim() == "")||(Txt_EndDate.Text.Trim() == "")))
            {

                Lbl_viewMessage.Text = "Date fields cannot be empty";
            }
            else
            {

                DateTime _EndtDate = MyUser.GetDareFromText(Txt_EndDate.Text);
                DateTime _StDate = MyUser.GetDareFromText(Txt_Startdate.Text);
                if (_EndtDate < _StDate)
                {
                    Lbl_viewMessage.Text = "Start Date Must Be Less than End Date";

                }
                else
                {
                    FillViewGrid();
                    if (GrdView.Rows.Count > 0)
                    { }
                    else
                    {
                        Lbl_viewMessage.Text = "No Bills Found...!";

                    }
                }

            }
        }

        private void FillViewGrid()
        {
            Lbl_viewMessage.Text = "";
            DateTime _Date = DateTime.Now;
            string sql = "";

            if (RDB_StudTypeCleranceDone.SelectedValue == "0")
            {
                sql = " select tblview_student.Id ,tblview_student.StudentName, tblview_feebill.BillNo , tblview_feebill.PaymentMode , tblview_feebill.PaymentModeId , tblview_feebill.BankName , date(tblview_feebill.CreatedDateTime) as CreatedDateTime from tblview_feebill inner join tblview_student on tblview_student.Id = tblview_feebill.StudentID inner join tblbillclearence on tblbillclearence.StudentId = tblview_feebill.StudentID and tblbillclearence.BillNo = tblview_feebill.BillNo where ";
                if ((Drp_Name1.SelectedValue != "0") && (Drp_Name1.Visible == true))
                {
                    sql = sql + " tblview_student.Id= " + int.Parse(Drp_Name1.SelectedValue) + " and";
                }
                if (Rdo_Options.SelectedValue == "0")
                {
                    sql = sql + "  tblbillclearence.ClearedDate='" + _Date.Date.ToString("s") + "' and";
                }
                if (Rdo_Options.SelectedValue == "1")
                {
                    sql = sql + "  tblbillclearence.ClearedDate<='" + _Date.Date.ToString("s") + "' and tblbillclearence.ClearedDate>='" + _Date.Date.AddDays(-7).ToString("s") + "' and";
                }
                if (Rdo_Options.SelectedValue == "2")
                {
                    sql = sql + "  tblbillclearence.ClearedDate<='" + _Date.Date.ToString("s") + "' and tblbillclearence.ClearedDate>='" + _Date.Date.AddDays(-31).ToString("s") + "' and";
                }
                if ((Rdo_Options.SelectedValue == "3") && ((Txt_Startdate.Text != "") || (Txt_EndDate.Text != "")))
                {
                    DateTime _EndtDate = General.GetDateTimeFromText(Txt_EndDate.Text);
                    DateTime _StDate = General.GetDateTimeFromText(Txt_Startdate.Text);
                    sql = sql + "  tblbillclearence.ClearedDate <='" + _EndtDate.Date.ToString("s") + "' and tblbillclearence.ClearedDate>='" + _StDate.Date.ToString("s") + "' and";


                }

                sql = sql + "  tblview_student.`Status`<>0 and tblview_feebill.Canceled=0";
            }
            else
            {
                sql = " select tbltempstdent.Id ,tbltempstdent.Name as StudentName, tblfeebill.BillNo , tblfeebill.PaymentMode , tblfeebill.PaymentModeId , tblfeebill.BankName , date(tblfeebill.CreatedDateTime) as CreatedDateTime from tblfeebill inner join tbltempstdent on tbltempstdent.TempId = tblfeebill.TempId inner join tblbillclearence on  tblbillclearence.BillNo = tblfeebill.BillNo where tbltempstdent.`Status`=1 and tblfeebill.Canceled=0";
            }

            MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                GrdView.Columns[0].Visible = true;
                GrdView.DataSource = MydataSet;
                GrdView.DataBind();
                GrdView.Columns[0].Visible = false;

                Pnl_View.Visible = true;
            }
            else
            {
                Lbl_viewMessage.Text = "No cleared bill found";
                GrdView.DataSource = null;
                GrdView.DataBind();
                Pnl_View.Visible = false;
            }
        }

        protected void GrdView_SelectedIndexChanged(object sender, EventArgs e)
        {

            string BillNo = GrdView.SelectedRow.Cells[2].Text.ToString();

            if (BillNo != "")
            {
                int _Value = 1;
                string _PageName = "FeeBillSmall.aspx";
                bool Pdf = false;
                if (MyFeeMang.GetBillType(ref _Value, ref _PageName, out Pdf))
                {
                    if (Pdf)
                    {
                        string _ErrorMsg = "";
                        Pdf MyPdf = new Pdf(MyFeeMang.m_MysqlDb, objSchool);
                        _ErrorMsg = "";
                        string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
                        string _PdfName = "";
                        
                        if (MyPdf.CreateFeeReciptPdf(BillNo, _physicalpath, out _PdfName, out _ErrorMsg, _Value))
                        {
                            _ErrorMsg = "";
                            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);

                        }
                        else if (_PdfName == "")
                        {
                            Lbl_ClearanceMessage.Text = "Failed To Create";
                        }
                    }
                    else
                    {
                        string _Bill = BillNo;                          
                        ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"" + _PageName + "?BillNo=" + _Bill + "&BillType=0\");", true);
                        
                    }
                }
            }
        }

        private bool QuickBillEnabled()
        {
            bool _valid = false;
            string QuickBill;
            string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='QuickBill'";
            if (MyFeeMang.m_TransationDb != null)
            {
                MyReader = MyFeeMang.m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            }
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
            if (MyFeeMang.m_TransationDb != null)
            {
                MyReader = MyFeeMang.m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            }
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

        protected void Rdo_Options_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (Rdo_Options.SelectedValue == "3")
            {
                Txt_EndDate.Visible = true;
                Txt_Startdate.Visible = true;
                Btn_View.Visible = true;
            }
            else
            {
                FillViewGrid();
                Btn_View.Visible = false;
                Txt_EndDate.Visible = false;
                Txt_Startdate.Visible = false;
            }
        }

      

       
    }
}