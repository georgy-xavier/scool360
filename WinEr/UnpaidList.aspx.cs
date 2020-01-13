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
    public partial class UnpaidList : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private SMSManager MysmsMang;
        private EmailManager MyEmailMng;
        private DataSet MydataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");

            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyFeeMang = MyUser.GetFeeObj();
            MysmsMang = MyUser.GetSMSMngObj();
            MyEmailMng = MyUser.GetEmailObj();
            if (MyEmailMng == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            if (MysmsMang == null)
            {
                Response.Redirect("RoleErr.htm");

            }
            if (MyFeeMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(660))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    Lbl_note.Visible = false;
                    Pnl_studlist.Visible = false;
                    LoadClassToDrp();
                    LoadfeeTodrp();
                    LoadBatchToDrp();
                    LoadStudentType();
                    //LoadStudentsToGrid();
                    ViewState["_columnName"] = null;
                    //some initlization
                }
            }
        }

        private void LoadStudentType()
        {
            Drp_studenttype.Items.Clear();

            string sql = "SELECT tblstudtype.Id,tblstudtype.TypeName from tblstudtype";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li;
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_studenttype.Items.Add(li);
                }
                li = new ListItem("All category", "0");
                Drp_studenttype.Items.Add(li);
                Drp_studenttype.SelectedValue = "0";

            }
            else
            {
                ListItem li = new ListItem("No category Found", "-1");
                Drp_studenttype.Items.Add(li);
                this.WC_MessageBox.ShowMssage("No category found");
                DisableAll();
            }
        }

        //private void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    switch (RadioButtonList1.SelectedItem.Text)
        //    {
        //        case "Panel1":
        //            Panel1.Visible = true;
        //            Panel2.Visible = false;
        //            break;
        //        case "Panel2":
        //            Panel1.Visible = false;
        //            Panel2.Visible = true;
        //            break;
        //    }
        //}


        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Panel11.Visible = false;
            Panel12.Visible = false;
            switch (RadioButtonList1.SelectedValue)
            {
                case "Panel11":
                    Panel11.Visible = true;
                    Panel12.Visible = false;
                    break;
                case "Panel12":
                    Panel11.Visible = false;
                    Panel12.Visible = true;
                    break;
            }
        }
 
        private void LoadBatchToDrp()
        {
            Drp_Batch.Items.Clear();

            string sql = "SELECT Id,BatchName from tblbatch where tblbatch.Created=1 ORDER BY Id DESC";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li;
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Batch.Items.Add(li);
                }
                li = new ListItem("All Batch", "0");
                Drp_Batch.Items.Add(li);
            }
            else
            {
                ListItem li = new ListItem("No Batch Found", "-1");
                Drp_Batch.Items.Add(li);
                this.WC_MessageBox.ShowMssage("No Batch found");
                DisableAll();
            }
        }

        private void LoadfeeTodrp()
        {
            Drp_FeeName.Items.Clear();

            string sql = "SELECT tblfeeaccount.Id, tblfeeaccount.AccountName from tblfeeaccount where tblfeeaccount.Status=1 and `Type`<>2 ORDER BY tblfeeaccount.AccountName";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li;
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_FeeName.Items.Add(li);
                }
                li = new ListItem("All Fee", "0");
                Drp_FeeName.Items.Add(li);
            }
            else
            {
                ListItem li = new ListItem("No Fee Found", "-1");
                Drp_FeeName.Items.Add(li);
                this.WC_MessageBox.ShowMssage("No fees found");
                DisableAll();
            }

        }

        private void DisableAll()
        {
            Drp_FeeName.Enabled = false;
            Drp_Class.Enabled = false;
            Chk_DueFee.Enabled = false;
            Btn_Export.Enabled = false;
            Drp_Batch.Enabled = false;
            Drp_studenttype.Enabled = false;
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

        private void LoadStudentsToGrid()
        {
            Panel_SearchResult.Visible = false;
            string sql;
            Grid_Stud.Columns[1].Visible = true;
            if (Drp_Class.SelectedValue != "-1" && Drp_FeeName.SelectedValue != "-1" && Drp_Batch.SelectedValue !="-1")
            {
                sql = GetSqlQueryString();
                //  MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                MydataSet = getDateSet(sql);

                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    Grid_Stud.DataSource = MydataSet;

                    Grid_Stud.DataBind();
                    Panel_SearchResult.Visible = true;
                    Btn_Export.Enabled = true;
                    Lbl_note.Visible = false;
                    Pnl_studlist.Visible = true;
                    displaytotalamount(MydataSet);
                    if (MyUser.HaveModule(23) && MyUser.HaveActionRignt(96))
                    {
                        Pnl_SmStext.Visible = true;
                        Txt_SmsText.Text = GetSMSText(4);
                        Load_Seperators();
                    }
                    else
                    {
                        
                        Pnl_SmStext.Visible = false;
                    }

                    if (MyUser.HaveModule(31) )
                    {
                        Pnl_Emailtext.Visible = true;
                        string _subject="";

                        Editor_Body.Content = GetEmailText(4, out _subject);
                        Txt_EmailSubject.Text = _subject;
                        Load_Seperators();
                    }
                    else
                    {

                        Pnl_Emailtext.Visible = false;
                    }

                    Grid_Stud.Columns[1].Visible = false;
                }
                else
                {
                    Grid_Stud.DataSource = null;
                    Grid_Stud.DataBind();
                    Lbl_note.Visible = true;
                    Btn_Export.Enabled = false;
                    Pnl_studlist.Visible = false;
                    //Lbl_msg.Text = "No Students found";
                    //this.MPE_MessageBox.Show();
                }
            }
        }

        private void displaytotalamount(DataSet mydataset)
        {
            double totalunpaidamount = 0;
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < MydataSet.Tables[0].Rows.Count; i++)
                {
                    totalunpaidamount = double.Parse(MydataSet.Tables[0].Rows[i]["BalanceAmount"].ToString()) + totalunpaidamount;


                }



            }



            Txt_total.Text = totalunpaidamount.ToString();
        }

        private string GetSqlQueryString()
        {
            string sql = "";
            string Batch = "";
            //if (Drp_Batch.SelectedValue.ToString() != "0")
            //    Batch = " tblstudentclassmap.BatchId= " + int.Parse(Drp_Batch.SelectedValue.ToString()) + " AND ";


            if (Drp_Class.SelectedValue != "0")
            {
                sql = "SELECT Distinct tblstudent.Id, tblstudent.StudentName, tblstudentclassmap.RollNo , tblclass.ClassName,tblstudent.OfficePhNo FROM tblfeestudent  inner join tblstudentclassmap on tblfeestudent.StudId= tblstudentclassmap.StudentId inner join tblstudent ON  tblstudent.Id= tblfeestudent.StudId inner join tblclass on tblclass.Id= tblstudentclassmap.ClassId inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId INNER JOIN tblstudtype ON tblstudtype.Id=tblstudent.StudTypeId  where  " + Batch + " tblstudent.Status=1 AND tblfeestudent.Status<>'Paid' and tblfeestudent.`Status`<> 'Fee Exemtion'  AND tblstudentclassmap.ClassId=" + Drp_Class.SelectedValue.ToString() + " ";
            }
            else
            {
                sql = "SELECT Distinct tblstudent.Id, tblstudent.StudentName, tblstudentclassmap.RollNo , tblclass.ClassName,tblstudent.OfficePhNo FROM tblfeestudent  inner join tblstudentclassmap on tblfeestudent.StudId= tblstudentclassmap.StudentId inner join tblstudent ON  tblstudent.Id= tblfeestudent.StudId inner join tblclass on tblclass.Id= tblstudentclassmap.ClassId inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId INNER JOIN tblstudtype ON tblstudtype.Id=tblstudent.StudTypeId where  " + Batch + " tblstudent.Status=1 AND tblfeestudent.Status<>'Paid' and tblfeestudent.`Status`<> 'Fee Exemtion'  AND tblstudentclassmap.ClassId IN (SELECT tblclass.Id from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ) ";
            }


            if (Chk_DueFee.Checked)
            {
                sql = sql + " AND tblfeeschedule.DueDate <= CURRENT_DATE()";

            }


            if (Drp_Batch.SelectedValue != "0")
            {
                sql = sql + " AND tblfeeschedule.BatchId=" + Drp_Batch.SelectedValue.ToString();
            }

            if (Drp_FeeName.SelectedValue != "0")
            {
                sql = sql + " AND tblfeeschedule.FeeId=" + Drp_FeeName.SelectedValue.ToString();
            }
            if (Drp_studenttype.SelectedValue != "0")
            {
                sql = sql + " AND tblstudent.StudTypeId=" + Drp_studenttype.SelectedValue.ToString();
            }

            sql = sql + " Order by tblclass.Standard ASC ,tblstudent.LastClassId ASC ,tblstudentclassmap.RollNo ASC";
            return sql;
 //            SELECT Distinct tblstudent.Id, tblstudent.StudentName, tblstudentclassmap.RollNo , tblclass.ClassName,tblstudent.OfficePhNo FROM tblfeestudent 
 //inner join tblstudentclassmap on tblfeestudent.StudId= tblstudentclassmap.StudentId 
 //inner join tblstudent ON  tblstudent.Id= tblfeestudent.StudId 
 //inner join tblclass on tblclass.Id= tblstudentclassmap.ClassId 
 //inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId 

 //where  tblstudentclassmap.BatchId= 45 AND tblstudent.Status=1 AND tblfeestudent.Status<>'Paid' and tblfeestudent.`Status`<> 'Fee Exemtion'  
 //AND tblstudentclassmap.ClassId=17  AND tblfeeschedule.DueDate <= CURRENT_DATE() 
 //AND tblfeeschedule.BatchId=45 

 //Order by tblclass.Standard ASC ,tblstudent.LastClassId ASC ,tblstudentclassmap.RollNo ASC       
            
           
            
        }



        private DataSet getDateSet(string sql)
        {

            DataSet FeeDetails = new DataSet();
            DataTable dt;
            int StudentId = 0;
            double balancetopayamt = 0;
            FeeDetails.Tables.Add(new DataTable("FullFeeDetails"));
            dt = FeeDetails.Tables["FullFeeDetails"];
            // dt.Columns.Add("Id");
            dt.Columns.Add("Id");
            dt.Columns.Add("StudentName");
            //     dt.Columns["TotalTaxAmt"].DataType = System.Type.GetType("System.Double");

            dt.Columns.Add("RollNo");
            //dt.Columns["TotalNetAmt"].DataType = System.Type.GetType("System.Double");

            dt.Columns.Add("ClassName");
            // dt.Columns["TotalDiscAmt"].DataType = System.Type.GetType("System.Double");

            dt.Columns.Add("BalanceAmount");
            //dt.Columns["TotalCashAmt"].DataType = System.Type.GetType("System.Double");

             dt.Columns.Add("OfficePhNo");





            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    StudentId = int.Parse(MyReader.GetValue(0).ToString());
                    balancetopayamt = getbalanceamount(StudentId);
                    if (balancetopayamt != 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr["Id"] = StudentId;
                        dr["StudentName"] = MyReader.GetValue(1).ToString();
                        if (MyReader.GetValue(2).ToString() != "-1")
                        {
                            dr["RollNo"] = MyReader.GetValue(2).ToString();

                        }
                        else
                        {
                            dr["RollNo"] = "NA";

                        }

                        dr["ClassName"] = MyReader.GetValue(3).ToString();

                        dr["BalanceAmount"] = balancetopayamt;

                        dr["OfficePhNo"] = MyReader.GetValue(4).ToString();

                        FeeDetails.Tables["FullFeeDetails"].Rows.Add(dr);
                    }

                }

            }
            return FeeDetails;
        }

        private double getbalanceamount(int StudentId)
        {
            OdbcDataReader MyReader1 = null;
            double _amount = 0;
            string sql = "Select sum(tblfeestudent.BalanceAmount) from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id = tblfeestudent.SchId where tblfeestudent.StudId=" + StudentId;
            if (Drp_FeeName.SelectedValue != "0")
                sql = sql + " and tblfeeschedule.FeeId=" + int.Parse(Drp_FeeName.SelectedValue);

            if (int.Parse(Drp_Batch.SelectedValue.ToString()) > 0)
                sql = sql += " and tblfeeschedule.BatchId=" + int.Parse(Drp_Batch.SelectedValue.ToString());

            if (Chk_DueFee.Checked)
                sql = sql + " AND tblfeeschedule.DueDate <= CURRENT_DATE()";


            MyReader1 = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader1.HasRows)
            {
                _amount = double.Parse(MyReader1.GetValue(0).ToString());
            }
            return _amount;
        }

        protected void Btn_Export_Click(object sender, EventArgs e)
        {
            //BTN
            if (Drp_Class.SelectedValue != "-1" && Drp_FeeName.SelectedValue != "-1" && Drp_studenttype.SelectedValue !="-1")
            {
                string sql = GetSqlQueryString();
                //MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                MydataSet = getDateSet(sql);
                
                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    MydataSet.Tables[0].Columns.Remove(MydataSet.Tables[0].Columns[0]);
                    //if (!WinEr.ExcelUtility.ExportDataSetToExcel(MydataSet, "StudentList.xls"))
                    //{
                    //    WC_MessageBox.ShowMssage("MS Excel is missing. Please install");
                    //}

                    string FileName = "UnpaidStudentList";
                    string _ReportName = "<table><tr><td colspan=\"8\" style=\"text-align:center;\"><b>Fee Bill Report</b></td></tr><tr><td>Created Date:" + DateTime.Now.ToString() + "</td><td></td></tr></table>";
                    if (!WinEr.ExcelUtility.ExportDataToExcel(MydataSet, _ReportName, FileName, MyUser.ExcelHeader))
                    {
                        WC_MessageBox.ShowMssage("MS Excel is missing. Please install");
                    }
                }
            }
        }

        protected void Btn_GetStudents_Click(object sender, EventArgs e)
        {
            LoadStudentsToGrid();
        }


        # region SMS


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


        private string GetEmailText(int _Option, out string subject)
        {
             subject = "";
            string _Format = "";
            string sql = "SELECT Subject,Body FROM tbl_emailoptionconfig WHERE Id=" + _Option;
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _Format = MyReader.GetValue(1).ToString();
                subject = MyReader.GetValue(0).ToString(); 
            }
            return _Format;
        }


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
            this.Email_Seperators.InnerHtml = innerhtml;
        }

        protected void Btn_Send_Click(object sender, EventArgs e)
        {
            MysmsMang.InitClass();
            string phonelist = "";
            string msg = "";
            string Message = "";
            bool Valid = true;

            if (Data_Complete(out msg))
            {
                Grid_Stud.Columns[1].Visible = true;
                string _StudentId = GetStudentIdFromGrid();
                DataSet Parents = GetParentsList(_StudentId);
                Grid_Stud.Columns[1].Visible = false;

                foreach (GridViewRow gv in Grid_Stud.Rows)
                {

                    CheckBox Chk_selected = (CheckBox)gv.FindControl("CheckBoxUpdate");
                    if (Chk_selected.Checked)
                    {
                        phonelist = MysmsMang.Get_SelectedParentPhoneNo_List(gv.Cells[1].Text.ToString());
                        if (phonelist != "")
                        {
                            Message = MysmsMang.GenerateSMSstring(Txt_SmsText.Text, GetParentName(ref Parents, gv.Cells[1].Text.ToString()), gv.Cells[2].Text.ToString(), gv.Cells[5].Text.ToString(),Drp_Batch.SelectedItem.Text);
                            //dominic sms
                            string failedList = "";
                            if (MysmsMang.SendBULKSms(phonelist, Message, "90366450445", "WINER", true, out  failedList))
                            {
                                MyUser.m_DbLog.LogToDb(MyUser.UserName, "SMS In Unpaid Fees", "Message : " + Message, 1);
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

        protected void Btn_EmailSend_Click(object sender, EventArgs e)
        {
            MyEmailMng.InitClass();

            string msg = "";
            string Message = "";
            bool Valid = false;

            if (Data_EmailComplete(out msg))
            {
                Grid_Stud.Columns[1].Visible = true;
                string _StudentId = GetStudentIdFromGrid();
                DataSet Parents = GetParentsList(_StudentId);
                Grid_Stud.Columns[1].Visible = false;

                foreach (GridViewRow gv in Grid_Stud.Rows)
                {

                    CheckBox Chk_selected = (CheckBox)gv.FindControl("CheckBoxUpdate");
                    if (Chk_selected.Checked)
                    {

                        string emailId = MyEmailMng.GetEmailParentId( int.Parse(gv.Cells[1].Text.ToString()));
                        if (emailId != "")
                        {
                            string EmailBody = Editor_Body.Content.Replace("'", "").Replace("\\", "");
                            Message = MysmsMang.GenerateSMSstring(EmailBody, GetParentName(ref Parents, gv.Cells[1].Text.ToString()), gv.Cells[2].Text.ToString(), gv.Cells[5].Text.ToString(), Drp_Batch.SelectedItem.Text);
                            MyEmailMng.InsertDataToAutoEmailList(emailId, gv.Cells[1].Text.ToString(), Txt_EmailSubject.Text, Message, 2);

                                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Email In Unpaid Fees", "Message : " + Message.Substring(0,30), 1);
                                Valid = true;

                        }
                    }
                }
                if (Valid)
                    WC_MessageBox.ShowMssage("Email sent successfully");
                else
                    WC_MessageBox.ShowMssage("Email sending failed. Please try again");

            }
            else
            {
                WC_MessageBox.ShowMssage(msg);
            }
        }

        private bool Data_EmailComplete(out string msg)
        {
            bool valid = true;
            msg = "";
            if (Txt_EmailSubject.Text.Trim() == "")
            {
                msg = "Enter email subject";
                valid = false;
            }
            if (valid && Editor_Body.Content == "")
            {
                msg = "Enter email body";
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
                    msg = "Select student for senting email";
                    valid = false;
                }
            }
            return valid;
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
                        Student = Student + gv.Cells[1].Text.ToString();
                    else
                        Student = Student + "," + gv.Cells[1].Text.ToString();
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
     

        # endregion



        

     
    }
}
