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
    public partial class InventoryIssueBook : System.Web.UI.Page
    {
        private ConfigManager MyConfigMang;
        private KnowinUser MyUser;
        private WinBase.Inventory Myinventory;
        private OdbcDataReader MyReader = null;
        private FeeManage MyFeeMang;

        #region Events
      
            protected void Page_Load(object sender, EventArgs e)
            {
                if (Session["UserObj"] == null)
                {
                    Response.Redirect("sectionerr.htm");
                }
                MyUser = (KnowinUser)Session["UserObj"];
                MyConfigMang = MyUser.GetConfigObj();
                MyFeeMang = MyUser.GetFeeObj();
                Myinventory = MyUser.GetInventoryObj();
                if (MyConfigMang == null)
                {
                    Response.Redirect("RoleErr.htm");
                }
                else if (!MyUser.HaveActionRignt(865))
                {
                    Response.Redirect("RoleErr.htm");
                }
                else
                {
                    
                    if (!IsPostBack)
                    {
                        Session["Ds"] = null;
                        //DisplaySpcBook.Visible = false;
                        Pnl_ShowDetails.Visible = false;
                        LoadClassToDropDown();
                        AddStudentToDropDownStudent();
                        AddStudentNameToDropDownStudent();
                        Loadcategorytodropdown();
                        LoadLocationToDropDown();

                    }
                }
            }

            protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
            {
                AddStudentToDropDownStudent();
                AddStudentNameToDropDownStudent();
                ClearGrid();
            }

            private void ClearGrid()
            {
                Grd_IssueBook.DataSource = null;
                Grd_IssueBook.DataBind();
                Grd_IssueSpcBook.DataSource = null;
                Grd_IssueSpcBook.DataBind();
                Lbl_Err.Text = "";
                
            }

            protected void Btn_Show_Click(object sender, EventArgs e)
            {
                Txt_totalCost.Text = "";
                LoadBookScheduleDetails();
            }

            protected void Drp_RollNumber_SelectedIndexChanged(object sender, EventArgs e)
            {
                string sql = "";
                sql = "SELECT map.StudentId,stud.RollNo FROM tblstudentclassmap map inner join tblstudent stud on stud.id= map.Studentid where stud.status=1 and  map.ClassId=" + int.Parse(Drp_Class.SelectedValue) + " and map.RollNo<>-1 and map.BatchId=" + MyUser.CurrentBatchId + " and  map.StudentId=" + int.Parse(Drp_RollNumber.SelectedValue.ToString()) + "  order by map.RollNo";
                MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Drp_Student.SelectedValue = MyReader.GetValue(0).ToString();
                    Drp_RollNumber.SelectedValue = MyReader.GetValue(0).ToString();
                }
                ClearGrid();
            }

            protected void Drp_Student_SelectedIndexChanged(object sender, EventArgs e)
            {
                string sql = "";
                sql = " SELECT map.StudentId,map.RollNo FROM tblstudentclassmap map inner join tblstudent stud on stud.id= map.Studentid where stud.status=1 and  map.ClassId=" + int.Parse(Drp_Class.SelectedValue) + " and map.RollNo<>-1 and map.BatchId="+MyUser.CurrentBatchId+" and  map.StudentId=" + int.Parse(Drp_Student.SelectedValue.ToString()) + "  order by map.RollNo";
                MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Drp_RollNumber.SelectedValue = MyReader.GetValue(0).ToString();
                    Drp_Student.SelectedValue = MyReader.GetValue(0).ToString();
                }

                ClearGrid();
            }

            protected void Btn_IssueBook_Click(object sender, EventArgs e)
            {

                Hdn_Type.Value = "3";
                CheckBox ch = new CheckBox();
                CheckBox chk = new CheckBox();
                TextBox issuecounttest = new TextBox();
                int checkd = 0;
                int Issuecount = 0;
                int countvalue = 0;
                int scheduledcnt = 0;
                DataSet IssuereportDs = new DataSet();
                DataRow _dr;
                DataTable _dt;
                IssuereportDs.Tables.Add(new DataTable("IssueReceipt"));
                _dt = IssuereportDs.Tables["IssueReceipt"];
                _dt.Columns.Add("Bookname");
                string booktestname = "";
                int value = 0;
                foreach (GridViewRow gv in Grd_IssueBook.Rows)
                {
                    issuecounttest = (TextBox)gv.FindControl("TxtIssueCount");
                    ch = (CheckBox)gv.FindControl("CheckBoxUpdate");
                    if (ch.Checked)
                    {
                        checkd = 1;
                        Issuecount = int.Parse(issuecounttest.Text);
                        scheduledcnt = int.Parse(gv.Cells[11].Text);
                        int scheduledID = int.Parse(gv.Cells[2].Text);
                        if (scheduledID != 0)
                        {
                            if (Issuecount > scheduledcnt)
                            {
                                Lbl_Err.Text = "Issue count must be less than scheduled count";
                                value = 2;
                                break;
                            }
                        }

                        if (value == 0)
                        {
                            if (issuecounttest.Text != "" && int.Parse(issuecounttest.Text) != 0)
                            {
                                int bookId = int.Parse(gv.Cells[1].Text.ToString());
                                booktestname = gv.Cells[8].Text.ToString();
                                int AvailableItemCount = Myinventory.GetAvailableCountOfItem(bookId);
                                if (Issuecount != 0)
                                {
                                    int configvalue;
                                    Myinventory.GetConfigValue(out configvalue);
                                    if (configvalue == 0)
                                    {
                                        if (AvailableItemCount == 0)
                                        {
                                            value = 1;
                                            WC_MessageBox.ShowMssage("Available stock is zero,Cannot issue " + booktestname + "!");
                                            break;
                                        }
                                        else
                                        {
                                            if (AvailableItemCount < Issuecount)
                                            {
                                                value = 1;
                                                WC_MessageBox.ShowMssage("Item count become minus,Cannot issue " + booktestname + "!");
                                                break;

                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (AvailableItemCount < Issuecount)
                                        {
                                            countvalue = 3;
                                            _dr = IssuereportDs.Tables["IssueReceipt"].NewRow();
                                            _dr["Bookname"] = booktestname;
                                            IssuereportDs.Tables["IssueReceipt"].Rows.Add(_dr);
                                        }
                                    }
                                    
                                }
                            }
                            else
                            {

                                Lbl_Err.Text = "Please enter Issue count";

                            }
                        }


                    }
                }
                if (checkd == 0)
                {
                    Lbl_Err.Text = "Select any book";
                }


                if (countvalue == 3)
                {
                    string BookName = "";
                    int cnt = 0;
                    if (IssuereportDs != null && IssuereportDs.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in IssuereportDs.Tables[0].Rows)
                        {

                            if (cnt == 0)
                            {
                                BookName = dr["Bookname"].ToString();
                                cnt = 1;
                            }
                            else
                            {
                                BookName = BookName + "," + dr["Bookname"].ToString();


                            }
                        }
                    }
                    Lbl_IssueMsg.Text = "Item count of " + BookName + " become minus.Do you want to continue?";
                    MPE_IssueMSG.Show();
                }
                if (value == 0 && countvalue==0)
                {
                    SaveIssueDataToTable(value);
                }

            }

            protected void Btn_Specialbookissue_Click(object sender, EventArgs e)
            {
                int locationid = 0;
                int.TryParse(Drp_Location.SelectedValue, out locationid);
                if (locationid <= 0)
                {
                    WC_MessageBox.ShowMssage("Select any location");
                }
                else
                {
                    Lbl_Msg.Text = "";
                    Rowchk.Visible = false;
                    Session["Ds"] = null;
                    LoadItemsTodropdown();
                    ClearPopup();
                    MPE_IssueSpecialBook.Show();
                }

            }

            protected void TxtIssueCount_TextChanged(object sender, EventArgs e)
            {
                Hdn_Type.Value = "0";
                GridViewRow currentRow = (GridViewRow)(sender as TextBox).Parent.Parent;
                int row = currentRow.RowIndex;
                Hdn_Row.Value = row.ToString();
                TextBox Txt_Issuecount = new TextBox();
                int schedulecount;
                int issuecount = 0;
                int itemID = 0;
                int value = 0;
                foreach (GridViewRow gr in Grd_IssueBook.Rows)
                {
                    Txt_Issuecount =(TextBox) gr.FindControl("TxtIssueCount");
                    if (gr.RowIndex == row)
                    {
                        schedulecount = int.Parse(gr.Cells[11].Text);
                        int.TryParse(Txt_Issuecount.Text, out issuecount);
                        itemID = int.Parse(gr.Cells[1].Text);
                        if (issuecount > schedulecount)
                        {
                            value = 1;
                            WC_MessageBox.ShowMssage("Issue count must be less than scheduled count");
                            break;
                        }
                        else 
                        {
                            int AvailableItemCount = Myinventory.GetAvailableCountOfItem(itemID);
                            if (issuecount != 0)
                            {
                                int configvalue;
                                Myinventory.GetConfigValue(out configvalue);
                                if (configvalue == 1)
                                {
                                    value = 1;
                                    Lbl_IssueMsg.Text = "Item count become minus.Do you want to continue?";
                                    MPE_IssueMSG.Show();
                                    break;
                                }
                                //else
                                //{
                                //    if (AvailableItemCount == 0)
                                //    {
                                //        value = 1;
                                //        WC_MessageBox.ShowMssage("Available stock is zero,Cannot issue this item!");
                                //        break;
                                //    }
                                //    else
                                //    {
                                //        if (AvailableItemCount < issuecount)
                                //        {
                                //            value = 1;
                                //            WC_MessageBox.ShowMssage("Available count become minus,Cannot issue this item!");
                                //            break;
                                //        }
                                //    }
                                //}
                            }
                        }
                    }
                }
                if (value == 0)
                {
                    FindTotalCost();
                }

            }

            //Txt_SpclBookCountt_TextChanged

            protected void Txt_SpclBookCountt_TextChanged(object sender, EventArgs e)
            {
                MPE_IssueSpecialBook.Show();
            }

            protected void Btn_Yes_Click(object sender, EventArgs e)
            {
                if (int.Parse(Hdn_Type.Value) == 3)
                {
                   
                    int value = 0;
                    SaveIssueDataToTable(value);
                }
                else if (int.Parse(Hdn_Type.Value) == 1)
                {
                    int value = 0; 
                    LoadDs();
                    BindGridView();
                    Txt_SpclBookCount.Text = "";
                    Drp_Category.SelectedValue = "0";
                    Drp_SpclBookName.SelectedValue = "0";
                    MPE_IssueSpecialBook.Show();
                }
                else if (int.Parse(Hdn_Type.Value) == 0)
                {
                    FindTotalCost();
                }

            }

            protected void Btn_No_Click(object sender, EventArgs e)
            {
                if (int.Parse(Hdn_Type.Value) == 0)
                {
                    //TextBox Issuecount = new TextBox();
                    //CheckBox cb = new CheckBox();
                    //foreach (GridViewRow gr in Grd_IssueBook.Rows)
                    //{
                    //    Issuecount = (TextBox)gr.FindControl("TxtIssueCount");
                    //    cb = (CheckBox)gr.FindControl("CheckBoxUpdate");
                    //    if (gr.RowIndex == int.Parse(Hdn_Row.Value))
                    //    {
                    //        Issuecount.Text = "0";
                    //        cb.Checked = false;
                    //    }
                    //}
                    //FindTotalCost();
                }
                else if (int.Parse(Hdn_Type.Value) == 1)
                {
                    Txt_SpclBookCount.Text = "";
                    Drp_Category.SelectedValue = "0";
                    Drp_SpclBookName.SelectedValue = "0";
                    Session["Ds"] = null;
                    MPE_IssueSpecialBook.Show();
                }
                else if (int.Parse(Hdn_Type.Value) == 3)
                {
                  
                }


            }
           
            protected void Btn_IssueSpclBookSave_Click(object sender, EventArgs e)
            {
                int classId = int.Parse(Drp_Class.SelectedValue);
                string classname = Drp_Class.SelectedItem.Text;
                int batchid = MyUser.CurrentBatchId;
                DateTime issuedate = System.DateTime.Now.Date;
                int studid = int.Parse(Drp_Student.SelectedValue);
                int bookid;
                int count;
                int categoryId = int.Parse(Drp_Category.SelectedValue);
                string studname = "";
                int oldissuecount = 0;
                int type = 1;
                //Session["BookName"]

                foreach (GridViewRow gr in Grd_IssueSpcBook.Rows)
                {
                    bookid = int.Parse(gr.Cells[0].Text);
                    count = int.Parse(gr.Cells[2].Text);
                    int opnqty = 0;
                    //Myinventory.SaveIsuueBookDetails(classId, batchid, issuedate, count, bookid, studid, 0, out oldissuecount);
                    Myinventory.AdjustItemCount(1, bookid, count,1);
                    Myinventory.UpdateOpenQuantity(bookid, out opnqty);
                    string bookname = Drp_SpclBookName.SelectedItem.Text;
                    studname = Drp_Student.SelectedItem.Text;
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Inventory : Issue special Book", " Book" + bookname + " is issued to " + studname + "", 1);
                }
                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "openIncpopup('InventoryIssueBookReceipt.aspx?StudName=" + studname + "&className=" + classname + "&Type=" + type + "&studid=" + studid + "&classId=" + int.Parse(Drp_Class.SelectedValue.ToString()) + "');", true);
                Lbl_Msg.Text = "Book issued";
                Rowchk.Visible = false;
                Chk_SpcIssue.Checked = false;
                ClearPopup();
                MPE_IssueSpecialBook.Show();
            }

            protected void Btn_SpcBookAdd_Click(object sender, EventArgs e)
            {
                Lbl_Msg.Text = "";
                int bookid = int.Parse(Drp_SpclBookName.SelectedValue);
                int categoryId = int.Parse(Drp_Category.SelectedValue);
                
                if (categoryId > 0)
                {
                    if (bookid > 0)
                    {
                        Hdn_Type.Value = "1";
                        int issuecount = 0;
                        int itemID = 0;
                        int value = 0;
                        int.TryParse(Txt_SpclBookCount.Text, out issuecount);
                        if (issuecount > 0)
                        {
                            itemID = int.Parse(Drp_SpclBookName.SelectedValue);
                            int AvailableItemCount = Myinventory.GetAvailableCountOfItem(itemID);
                            if (AvailableItemCount < issuecount)
                            {

                                int configvalue;
                                Myinventory.GetConfigValue(out configvalue);
                                if (configvalue == 1)
                                {
                                    value = 1;
                                    Lbl_IssueMsg.Text = "Item count become minus.Do you want to continue?";
                                    MPE_IssueMSG.Show();
                                }
                                else
                                {
                                    if (AvailableItemCount == 0)
                                    {
                                        value = 1;
                                        Lbl_Msg.Text = "Available stock is zero,Cannot issue this item!";
                                    }
                                    else
                                    {
                                        if (AvailableItemCount < issuecount)
                                        {
                                            value = 1;
                                            Lbl_Msg.Text = "Available count is " + AvailableItemCount + ",Cannot issue more than available count!";
                                        }
                                    }
                                }
                                //value = 1;
                                //Lbl_IssueMsg.Text = "Item count become minus.Do you want to continue?";
                                //MPE_IssueMSG.Show();
                            }
                            if (value == 0)
                            {
                                //SaveSpecialBookDataToTable(value);
                                LoadDs();
                                BindGridView();
                                Drp_SpclBookName.SelectedValue = "0";
                                Drp_Category.SelectedValue = "0";
                                Txt_SpclBookCount.Text = "";
                            }
                        }

                        else
                        {
                            Lbl_IssueMsg.Text = "Enter count";
                        }
                       
                    }
                    else
                    {
                        Lbl_Msg.Text = "Select Book";
                    }
                }
                else
                {
                    Lbl_Msg.Text = "Select Category";
                }

                
                MPE_IssueSpecialBook.Show();

            }

            protected void Btn_IssueSpclBookCancel_Click(object sender, EventArgs e)
            {
                ClearPopup();
               Session["Ds"] = null;
            }
            
            protected void Drp_Category_SelectedIndexChanged(object sender, EventArgs e)
            {
                LoadItemsTodropdown();
                MPE_IssueSpecialBook.Show();
                ClearGrid();
            }

            protected void CheckBoxUpdate_CheckedChanged(object sender, EventArgs e)
            {
                FindTotalCost();
            }

            protected void cbSelectAll_CheckedChanged(object sender, EventArgs e)
            {
                FindTotalCost();
            }

        #endregion

        #region Methods


            private void SaveIssueDataToTable(int value)
            {
                int save = 0;
                string bookname = "";
                string studname = "";
                int count = 0;
                int Oldissuecount = 0;
                int scheduledcnt = 0;
                string createduser = MyUser.UserName;
                string classname = "";
                double cost = 0;
                TextBox issuecount = new TextBox();
                TextBox TxtCost = new TextBox();
                int classId = int.Parse(Drp_Class.SelectedValue);
                DateTime issuedate = System.DateTime.Now.Date;
                int studid = int.Parse(Drp_Student.SelectedValue);
                int batchid = MyUser.CurrentBatchId;
                if (value == 0)
                {
                    CheckBox ch = new CheckBox();

                    foreach (GridViewRow gv in Grd_IssueBook.Rows)
                    {
                        ch = (CheckBox)gv.FindControl("CheckBoxUpdate");
                        issuecount = (TextBox)gv.FindControl("TxtIssueCount");
                        TxtCost = (TextBox)gv.FindControl("TxtCost");//TxtCost
                        if (ch.Checked)
                        {
                            Grd_IssueBook.Columns[1].Visible = true;
                            Grd_IssueBook.Columns[2].Visible = true;
                            Grd_IssueBook.Columns[3].Visible = true;
                            Grd_IssueBook.Columns[4].Visible = true;
                            Grd_IssueBook.Columns[5].Visible = true;
                            Grd_IssueBook.Columns[6].Visible = true;
                            Grd_IssueBook.Columns[7].Visible = true;
                            Grd_IssueBook.Columns[14].Visible = true;
                            Grd_IssueBook.Columns[15].Visible = true;

                            int bookId = int.Parse(gv.Cells[1].Text.ToString());
                            int locationid = 0;
                            int.TryParse(Drp_Location.SelectedValue, out locationid);
                            count = int.Parse(issuecount.Text);
                            bookname = gv.Cells[1].Text.ToString();
                            int scheduleId = int.Parse(gv.Cells[2].Text);
                            studname = Drp_Student.SelectedItem.Text;
                            classname = Drp_Class.SelectedItem.Text;
                            save = 1;
                            int OpnQty = 0;
                            double.TryParse(TxtCost.Text, out cost);
                            Myinventory.UpdateStockItem(bookId, count, locationid);
                            Myinventory.UpdateOpenQuantity(bookId, out OpnQty);
                            Myinventory.SaveIsuueBookDetails(classId, batchid, issuedate, count, bookId, studid, scheduleId, cost, out Oldissuecount, createduser, OpnQty);
                            Session["oldissuecount"] = Oldissuecount;
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Inventory : Issue Book", " Book" + bookname + " is issued to " + studname + "", 1);

                            Grd_IssueBook.Columns[1].Visible = false;
                            Grd_IssueBook.Columns[2].Visible = false;
                            Grd_IssueBook.Columns[3].Visible = false;
                            Grd_IssueBook.Columns[4].Visible = false;
                            Grd_IssueBook.Columns[5].Visible = false;
                            Grd_IssueBook.Columns[6].Visible = false;
                            Grd_IssueBook.Columns[7].Visible = false;
                            Grd_IssueBook.Columns[14].Visible = false;
                            Grd_IssueBook.Columns[15].Visible = false;
                        }
                    }
                }

                if (save == 1)
                {
                    Lbl_Err.Text = "Issued successfully";
                    LoadIssueDataset();
                    int type = 0;
                    double totalcost = 0;
                    double.TryParse(Txt_totalCost.Text, out totalcost);
                    //invbufferbill.aspx

                    //InventoryIssueBookReceipt.aspx
                    if (PDFBILL())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "openIncpopup('invbufferbill.aspx?StudName=" + studname + "&className=" + classname + "&Type=" + type + "&studid=" + studid + "&classId=" + int.Parse(Drp_Class.SelectedValue.ToString()) + "&totalcost=" + totalcost + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "openIncpopup('InventoryIssueBookReceipt.aspx?StudName=" + studname + "&className=" + classname + "&Type=" + type + "&studid=" + studid + "&classId=" + int.Parse(Drp_Class.SelectedValue.ToString()) + "&totalcost=" + totalcost + "');", true);
                    }
                    Txt_totalCost.Text = "";
                    LoadBookScheduleDetails();
                }

            }

            private bool PDFBILL()
            {
                string sql = "select  tblconfiguration.Value from tblconfiguration where Module='INVENTORY' and  Name='PDF_IssueBillType'";
                MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    if (Convert.ToInt32(MyReader["Value"]) == 1)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        
            private void BindGridView()
            {
                if (Session["Ds"] != null)
                {
                    DataSet IssueDs = (DataSet)Session["Ds"];
                    //Grd_IssueBook.Columns[0].Visible = true;
                    Pnl_ShowDetails.Visible = true;
                    Grd_IssueBook.Columns[1].Visible = true;
                    Grd_IssueBook.Columns[2].Visible = true;
                    Grd_IssueBook.Columns[3].Visible = true;
                    Grd_IssueBook.Columns[4].Visible = true;
                    Grd_IssueBook.Columns[5].Visible = true;
                    Grd_IssueBook.Columns[6].Visible = true;
                    Grd_IssueBook.Columns[7].Visible = true;
                    Grd_IssueBook.Columns[14].Visible = true;
                    Grd_IssueBook.Columns[15].Visible = true;
                    Grd_IssueBook.DataSource = IssueDs;
                    Grd_IssueBook.DataBind();
                    FixAllValues(IssueDs);
                    Grd_IssueBook.Columns[1].Visible = false;
                    Grd_IssueBook.Columns[2].Visible = false;
                    Grd_IssueBook.Columns[3].Visible = false;
                    Grd_IssueBook.Columns[4].Visible = false;
                    Grd_IssueBook.Columns[5].Visible = false;
                    Grd_IssueBook.Columns[6].Visible = false;
                    Grd_IssueBook.Columns[7].Visible = false;
                    Grd_IssueBook.Columns[14].Visible = false;
                    Grd_IssueBook.Columns[15].Visible = false;
                    //Grd_IssueBook.Columns[0].Visible = false; 
                }
                else
                {
                    Pnl_ShowDetails.Visible = false;
                }
            }

            private void FixAllValues(DataSet IssueDs)
            {
                TextBox TxtIssueCount = new TextBox();
                TextBox TxtCost = new TextBox();
                CheckBox chk = new CheckBox();
                int count = 0;
                foreach (GridViewRow gr in Grd_IssueBook.Rows)
                {
                    int drcount = 0;
                    TxtIssueCount = (TextBox)gr.FindControl("TxtIssueCount");
                    TxtCost = (TextBox)gr.FindControl("TxtCost");
                    chk = (CheckBox)gr.FindControl("CheckBoxUpdate");
                    if (IssueDs != null && IssueDs.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in IssueDs.Tables[0].Rows)
                        {
                            if (drcount == count)
                            {
                                if (dr["Chk"].ToString() == "True")
                                {
                                    chk.Checked = true;
                                }
                                if (dr["Chk"].ToString() == "False")
                                {
                                    chk.Checked = false;
                                }
                                if ( dr["Enabled"].ToString() == "True")
                                {
                                    chk.Enabled = true;
                                }
                                if (dr["Enabled"].ToString() == "False")
                                {
                                    chk.Enabled = false;
                                }
                                if (dr["IssueText"].ToString() == "True")
                                {
                                    TxtIssueCount.Enabled = true;
                                }
                                if (dr["IssueText"].ToString() == "False")
                                {
                                    TxtIssueCount.Enabled = false;
                                }
                                TxtCost.Text = dr["TotalCost"].ToString();
                                TxtIssueCount.Text = dr["IssueCount"].ToString();

                                break;

                            }
                            drcount++;
                        }

                    }
                    
                    count++;
                }
                FindTotalCost();
            } 

            private void LoadDs()      //Creating the new dataset of both special book and scheduled books.
            {
                TextBox TxtIssueCount = new TextBox();
                TextBox TxtCost = new TextBox();
                string sql = "";
                CheckBox chk = new CheckBox();
                DataSet IssuereportDs = new DataSet();
                DataRow _dr;
                DataTable _dt;
                IssuereportDs.Tables.Add(new DataTable("IssueReceipt"));
                _dt = IssuereportDs.Tables["IssueReceipt"];
                _dt.Columns.Add("BookId");
                _dt.Columns.Add("Id");
                _dt.Columns.Add("ItemName");
                _dt.Columns.Add("Cost");
                _dt.Columns.Add("Count");
                _dt.Columns.Add("TotalCost");
                _dt.Columns.Add("IssueCount");
                _dt.Columns.Add("Chk");
                _dt.Columns.Add("Enabled");
                _dt.Columns.Add("IssueText");
                _dt.Columns.Add("Category");
                _dt.Columns.Add("CategoryId");
                _dt.Columns.Add("SpecialItem");
             
                //IssueText
                int IssueCount = 0;
                int TotalCost = 0;

                if (Session["Ds"] != null)
                {
                    DataSet IssueDs = (DataSet)Session["Ds"];
                    if (IssueDs != null && IssueDs.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in IssueDs.Tables[0].Rows)
                        {
                            _dr = IssuereportDs.Tables["IssueReceipt"].NewRow();
                            _dr["Chk"] = dr["Chk"];
                            _dr["Enabled"] = dr["Enabled"];
                            _dr["BookId"] = dr["BookId"];
                            _dr["Id"] = dr["Id"];
                            _dr["ItemName"] = dr["ItemName"].ToString().Replace("&amp;", "&"); ;
                            _dr["Cost"] = dr["Cost"];
                            _dr["Count"] = dr["Count"];
                            _dr["TotalCost"] = dr["TotalCost"];
                            _dr["IssueCount"] = dr["IssueCount"];
                            _dr["IssueText"] = dr["IssueText"];
                            _dr["Category"] = dr["Category"];
                            _dr["CategoryId"] = dr["CategoryId"];
                            _dr["SpecialItem"] = dr["SpecialItem"];
                            IssuereportDs.Tables["IssueReceipt"].Rows.Add(_dr);
                        }
                    }
                    _dr = IssuereportDs.Tables["IssueReceipt"].NewRow();
                    _dr["BookId"] = Drp_SpclBookName.SelectedValue;
                    _dr["ItemName"] = Drp_SpclBookName.SelectedItem.Text.Replace("&amp;", "&");
                    _dr["Category"] = Drp_Category.SelectedItem.Text;
                    _dr["IssueCount"] = Txt_SpclBookCount.Text;
                    sql = "select tblinv_item.Cost from tblinv_item where tblinv_item.id=" + int.Parse(Drp_SpclBookName.SelectedValue) + "";
                    MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        _dr["Cost"] = MyReader.GetValue(0).ToString();
                    }
                    int Count = 0;
                    int.TryParse(Txt_SpclBookCount.Text, out Count);
                    int BasicCost = 0;
                    int.TryParse(MyReader.GetValue(0).ToString(), out BasicCost);
                    _dr["TotalCost"] = (Count * BasicCost).ToString();
                    _dr["Count"] = 0;
                    _dr["Id"] = "0";
                    _dr["Enabled"] = "True";
                    _dr["Chk"] = "True";
                    _dr["IssueText"] = "True";
                    _dr["CategoryId"] = Drp_Category.SelectedValue;
                    _dr["SpecialItem"] = "1";
                    IssuereportDs.Tables["IssueReceipt"].Rows.Add(_dr);
                    Session["Ds"] = IssuereportDs; //set the dataset in session
                  
                }
                if (Session["Ds"] == null)
                {
                   
                    foreach (GridViewRow gr in Grd_IssueBook.Rows) //Read datas from the gridview first and created the dataset
                    {
                        TxtIssueCount = (TextBox)gr.FindControl("TxtIssueCount");
                        TxtCost = (TextBox)gr.FindControl("TxtCost");
                        chk = (CheckBox)gr.FindControl("CheckBoxUpdate");
                        _dr = IssuereportDs.Tables["IssueReceipt"].NewRow();
                        if (chk.Checked == true)
                        {
                            _dr["Chk"] = "True";
                        }
                        else
                        {
                            _dr["Chk"] = "False";
                        }
                        if (chk.Enabled == true)
                        {
                            _dr["Enabled"] = "True";
                        }
                        else
                        {
                            _dr["Enabled"] = "False";
                        }
                        if (TxtIssueCount.Enabled == true)
                        {
                            _dr["IssueText"] = "True";
                        }
                        else
                        {
                            _dr["IssueText"] = "False";
                        }


                        _dr["BookId"] = gr.Cells[1].Text;
                        _dr["Id"] = gr.Cells[2].Text;
                        _dr["ItemName"] = gr.Cells[8].Text.Replace("&amp;", "&"); 
                        _dr["Cost"] = gr.Cells[10].Text;
                        _dr["Count"] = gr.Cells[11].Text;
                        _dr["Category"] = gr.Cells[9].Text;

                        int.TryParse(TxtIssueCount.Text, out IssueCount);
                        int.TryParse(TxtCost.Text, out TotalCost);
                        if (TotalCost != 0)
                        {
                            _dr["TotalCost"] = gr.Cells[1].Text;
                        }
                        else
                        {
                            _dr["TotalCost"] = "";
                        }
                        _dr["IssueCount"] = IssueCount;
                        _dr["CategoryId"] = gr.Cells[14].Text;
                        _dr["SpecialItem"] = gr.Cells[15].Text;
                        IssuereportDs.Tables["IssueReceipt"].Rows.Add(_dr);
                    }
                    _dr = IssuereportDs.Tables["IssueReceipt"].NewRow(); //Added the special book into the dataset.
                    _dr["BookId"] = Drp_SpclBookName.SelectedValue;
                    _dr["ItemName"] = Drp_SpclBookName.SelectedItem.Text;
                    _dr["Category"] = Drp_Category.SelectedItem.Text;
                    _dr["IssueCount"] = Txt_SpclBookCount.Text;
                    sql = "select tblinv_item.Cost from tblinv_item where tblinv_item.id=" + int.Parse(Drp_SpclBookName.SelectedValue) + "";
                    MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        _dr["Cost"] = MyReader.GetValue(0).ToString();
                    }
                    int Count = 0;
                    int.TryParse(Txt_SpclBookCount.Text, out Count);
                    int BasicCost = 0;
                    int.TryParse(MyReader.GetValue(0).ToString(), out BasicCost);
                    _dr["TotalCost"] = (Count * BasicCost).ToString();
                    _dr["Count"] = "0";
                    _dr["Id"] = "0";
                    _dr["Enabled"] = "True";
                    _dr["Chk"] = "True";
                    _dr["IssueText"] = "True";
                    _dr["CategoryId"] = Drp_Category.SelectedValue;
                    _dr["SpecialItem"] = "1";
                    IssuereportDs.Tables["IssueReceipt"].Rows.Add(_dr);
                    Session["Ds"] = IssuereportDs;
                }

            }

            private void SaveSpecialBookDataToTable(int value)
            {
                if (value == 0)
                {
                    Pnl_DisplaySpcBook.Visible = false; ;
                    LoadDs();
                    BindGridView();
                }
                MPE_IssueSpecialBook.Show();

            }

            private void ClearPopup()
            {
                Grd_IssueSpcBook.DataSource = null;
                Grd_IssueSpcBook.DataBind();
                Pnl_DisplaySpcBook.Visible = false;
                Txt_SpclBookCount.Text = "";
                
            }

            private void LoadIssueDataset()
            {

                DataSet IssuereportDs = new DataSet();
                DataRow _dr;
                DataTable _dt;
                int totalsccnt = 0;
                int oldissuecount = 0;
                IssuereportDs.Tables.Add(new DataTable("IssueReceipt"));
                _dt = IssuereportDs.Tables["IssueReceipt"];
                _dt.Columns.Add("BookName");
                _dt.Columns.Add("ScheduledCount");
                _dt.Columns.Add("IssuedCount");
                _dt.Columns.Add("TotalScheduledCount");
                _dt.Columns.Add("BookId");
                _dt.Columns.Add("BasicCost");
                _dt.Columns.Add("Cost");
                _dt.Columns.Add("Category");
                _dt.Columns.Add("CategoryId");
                _dt.Columns.Add("SpecialItem");
                TextBox Txt_Cost = new TextBox();

                string itemname = "";
                int Issuecount = 0,Scheduledcount=0;
                CheckBox ch = new CheckBox();
                foreach (GridViewRow gr in Grd_IssueBook.Rows)
                {
                    Grd_IssueBook.Columns[1].Visible = true;
                    Grd_IssueBook.Columns[2].Visible = true;
                    Grd_IssueBook.Columns[3].Visible = true;
                    Grd_IssueBook.Columns[4].Visible = true;
                    Grd_IssueBook.Columns[5].Visible = true;
                    Grd_IssueBook.Columns[6].Visible = true;
                    Grd_IssueBook.Columns[7].Visible = true;
                    Grd_IssueBook.Columns[14].Visible = true;
                    Grd_IssueBook.Columns[15].Visible = true;

                    ch = (CheckBox)gr.FindControl("CheckBoxUpdate");
                    if (ch.Checked)
                    {
                        TextBox Issuecounttxt = (TextBox)gr.FindControl("TxtIssueCount");
                        Txt_Cost = (TextBox)gr.FindControl("TxtCost");
                        itemname = gr.Cells[8].Text.Replace("&amp;", "&"); 
                        Scheduledcount = int.Parse(gr.Cells[11].Text);
                        if (Issuecounttxt.Text != "")
                        {
                            Issuecount = int.Parse(Issuecounttxt.Text);
                        }

                        _dr = IssuereportDs.Tables["IssueReceipt"].NewRow();
                        //ClassId,BookId,BatchId
                        string sql = "select tblinv_bookscheduledetails.Count from tblinv_bookscheduledetails where tblinv_bookscheduledetails.ClassId=" + int.Parse(Drp_Class.SelectedValue) + " and BookId=" + int.Parse(gr.Cells[1].Text) + " and BatchId=" + MyUser.CurrentBatchId + "";
                        MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                        if (MyReader.HasRows)
                        {
                            _dr["TotalScheduledCount"] = MyReader.GetValue(0).ToString();
                            totalsccnt=int.Parse(MyReader.GetValue(0).ToString());
                        }
                        int ScheduleId = int.Parse(gr.Cells[2].Text);
                        //if (ScheduleId == 0)
                        //{
                        //    string _sql = "select tblinv_category.Category from tblinv_category inner join tblinv_item on tblinv_item.Category = tblinv_category.Id where tblinv_item.Id=" + int.Parse(gr.Cells[1].Text) + "";
                        //    OdbcDataReader Categoryreader = null;
                        //    Categoryreader = Myinventory.m_MysqlDb.ExecuteQuery(_sql);
                        //    if (Categoryreader.HasRows)
                        //    {
                        //        _dr["BookName"] = Categoryreader.GetValue(0).ToString();
                        //    }

                        //}
                        //else
                        //{
                            _dr["BookName"] = itemname;
                       // }
                        oldissuecount =int.Parse(Session["oldissuecount"].ToString());
                        _dr["ScheduledCount"] = totalsccnt -(oldissuecount+ Issuecount); ;
                        _dr["IssuedCount"] = Issuecount;
                        _dr["BasicCost"] = gr.Cells[10].Text;
                        _dr["Category"] = gr.Cells[9].Text;
                        _dr["Cost"] =int.Parse(gr.Cells[10].Text)*Issuecount;
                       
                        _dr["BookId"] = gr.Cells[1].Text;
                         _dr["CategoryId"] = gr.Cells[14].Text;
                         _dr["SpecialItem"] = gr.Cells[15].Text;
                         IssuereportDs.Tables["IssueReceipt"].Rows.Add(_dr);
                        Txt_Cost.Text = "";

                        Grd_IssueBook.Columns[1].Visible = false;
                        Grd_IssueBook.Columns[2].Visible = false;
                        Grd_IssueBook.Columns[3].Visible = false;
                        Grd_IssueBook.Columns[4].Visible = false;
                        Grd_IssueBook.Columns[5].Visible = false;
                        Grd_IssueBook.Columns[6].Visible = false;
                        Grd_IssueBook.Columns[7].Visible = false;
                        Grd_IssueBook.Columns[14].Visible = false;
                        Grd_IssueBook.Columns[15].Visible = false;

                    }
                }
                Session["IssueReceipt"] = IssuereportDs;
            }

            private void LoadBookScheduleDetails()
            {
                Lbl_Err.Text = "";
                int locationId = 0;
                int ClassId = int.Parse(Drp_Class.SelectedValue);
                int studentId = int.Parse(Drp_Student.SelectedValue);
                int batchId = MyUser.CurrentBatchId;
                int categoryid = 0;
                int.TryParse(Drp_issueCategory.SelectedValue, out categoryid);
                int.TryParse(Drp_Location.SelectedValue, out locationId);
                DataSet BookScheduleDetails_Ds = null;
                if (ClassId <= 0)
                {
                    Lbl_Err.Text = "Select any class";
                }
                else if (studentId == -1)
                {
                    Lbl_Err.Text = "No student found";
                }
                else if (locationId<=0)
                {
                    Lbl_Err.Text = "Select location";
                    }
                else
                {
                    BookScheduleDetails_Ds = Myinventory.GetScheduleBookDetails(ClassId, studentId, batchId, categoryid, locationId);

                    if (BookScheduleDetails_Ds != null && BookScheduleDetails_Ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt;
                        DataRow dr;
                        dt = BookScheduleDetails_Ds.Tables[0];
                        dt.Columns.Add("TotalCost");
                        dt.Columns.Add("IssueCount");
                        dt.Columns.Add("Chk");
                        dt.Columns.Add("Enabled");
                        dt.Columns.Add("IssueText");
                        dt.Columns.Add("SpecialItem");
                        foreach (DataRow _dr in BookScheduleDetails_Ds.Tables[0].Rows)
                        {
                            _dr["TotalCost"] = null;
                            _dr["IssueCount"] = "";
                            _dr["Chk"] = "";
                            _dr["Enabled"] = "";
                            _dr["IssueText"] = "";
                            _dr["SpecialItem"] = "0";
                        }
                        //BookScheduleDetails_Ds.Tables[0].ro

                        Grd_IssueBook.Columns[1].Visible = true;
                        Grd_IssueBook.Columns[2].Visible = true;
                        Grd_IssueBook.Columns[3].Visible = true;
                        Grd_IssueBook.Columns[4].Visible = true;
                        Grd_IssueBook.Columns[5].Visible = true;
                        Grd_IssueBook.Columns[6].Visible = true;
                        Grd_IssueBook.Columns[7].Visible = true;
                        Grd_IssueBook.Columns[14].Visible = true;
                        Grd_IssueBook.Columns[15].Visible = true;
                        Grd_IssueBook.DataSource = BookScheduleDetails_Ds;
                        Grd_IssueBook.DataBind();
                        FillIssuecount(BookScheduleDetails_Ds, studentId);
                        Pnl_ShowDetails.Visible = true;
                        Grd_IssueBook.Columns[1].Visible = false;
                        Grd_IssueBook.Columns[2].Visible = false;
                        Grd_IssueBook.Columns[3].Visible = false;
                        Grd_IssueBook.Columns[4].Visible = false;
                        Grd_IssueBook.Columns[5].Visible = false;
                        Grd_IssueBook.Columns[6].Visible = false;
                        Grd_IssueBook.Columns[7].Visible = false;
                        Grd_IssueBook.Columns[14].Visible = false;
                        Grd_IssueBook.Columns[15].Visible = false;
                    }
                    else
                    {
                        Lbl_Err.Text = "No book is pending for issue";
                        Pnl_ShowDetails.Visible = false;
                        Grd_IssueBook.DataSource = null;
                        Grd_IssueBook.DataBind();

                    }
                }
            }

            private void LoadBookScheduleDetailsById()
            {
                Lbl_Err.Text = "";
                int categoryid = 0;
                int.TryParse(Drp_issueCategory.SelectedValue, out categoryid);
                int ClassId = int.Parse(Drp_Class.SelectedValue);
                int studentId = int.Parse(Drp_RollNumber.SelectedValue);
                int locationId = 0;
                int.TryParse(Drp_Location.SelectedValue, out locationId);
                int batchId = MyUser.CurrentBatchId;
                DataSet BookScheduleDetails_Ds = null;
                if (ClassId != -1)
                {
                    if (ClassId != 0)
                    {
                        if (studentId != -1)
                        {
                            BookScheduleDetails_Ds = Myinventory.GetScheduleBookDetails(ClassId, studentId, batchId, categoryid, locationId);
                            if (BookScheduleDetails_Ds != null && BookScheduleDetails_Ds.Tables[0].Rows.Count > 0)
                            {                                
                                Grd_IssueBook.Columns[1].Visible = true;
                                Grd_IssueBook.Columns[2].Visible = true;
                                Grd_IssueBook.Columns[3].Visible = true;
                                Grd_IssueBook.Columns[4].Visible = true;
                                Grd_IssueBook.Columns[5].Visible = true;
                                Grd_IssueBook.Columns[6].Visible = true;
                                Grd_IssueBook.Columns[7].Visible = true;
                                Grd_IssueBook.Columns[14].Visible = true;
                                Grd_IssueBook.Columns[15].Visible = true;

                                Pnl_ShowDetails.Visible = true;
                                Grd_IssueBook.DataSource = BookScheduleDetails_Ds;
                                Grd_IssueBook.DataBind();
                                FillIssuecount(BookScheduleDetails_Ds,studentId);

                                Grd_IssueBook.Columns[1].Visible = false;
                                Grd_IssueBook.Columns[2].Visible = false;
                                Grd_IssueBook.Columns[3].Visible = false;
                                Grd_IssueBook.Columns[4].Visible = false;
                                Grd_IssueBook.Columns[5].Visible = false;
                                Grd_IssueBook.Columns[6].Visible = false;
                                Grd_IssueBook.Columns[7].Visible = false;
                                Grd_IssueBook.Columns[14].Visible = false;
                                Grd_IssueBook.Columns[15].Visible = false;
                            }
                            else
                            {
                                Lbl_Err.Text = "None of the book is remained for scheduling";
                                Pnl_ShowDetails.Visible = false;
                                Grd_IssueBook.DataSource = null;
                                Grd_IssueBook.DataBind();
                            }
                        }
                        else
                        {
                            Lbl_Err.Text = "No student found";
                        }
                    }
                    else
                    {
                        Lbl_Err.Text = "Select any class";
                    }
                }
            }


            private void FillIssuecount(DataSet BookScheduleDetails_Ds, int studid)
            {
                string sql = "";
                int ClassId = int.Parse(Drp_Class.SelectedValue);
                int batchId = MyUser.CurrentBatchId;
                OdbcDataReader issuebookreader = null;
                CheckBox Chk = new CheckBox();
                foreach (GridViewRow gv in Grd_IssueBook.Rows)
                {
                    Chk = (CheckBox)gv.FindControl("CheckBoxUpdate");
                    TextBox IssueCount = (TextBox)gv.FindControl("TxtIssueCount");
                    Grd_IssueBook.Columns[1].Visible = true;
                    Grd_IssueBook.Columns[2].Visible = true;
                    Grd_IssueBook.Columns[3].Visible = true;
                    Grd_IssueBook.Columns[4].Visible = true;
                    Grd_IssueBook.Columns[5].Visible = true;
                    Grd_IssueBook.Columns[6].Visible = true;
                    Grd_IssueBook.Columns[7].Visible = true;
                    Grd_IssueBook.Columns[14].Visible = true;
                    Grd_IssueBook.Columns[15].Visible = true;

                    int bookid = int.Parse(gv.Cells[1].Text);

                    Grd_IssueBook.Columns[1].Visible = false;
                    Grd_IssueBook.Columns[2].Visible = false;
                    Grd_IssueBook.Columns[3].Visible = false;
                    Grd_IssueBook.Columns[4].Visible = false;
                    Grd_IssueBook.Columns[5].Visible = false;
                    Grd_IssueBook.Columns[6].Visible = false;
                    Grd_IssueBook.Columns[7].Visible = false;
                    Grd_IssueBook.Columns[14].Visible = false;
                    Grd_IssueBook.Columns[15].Visible = false;
                    int scheduleid=int.Parse(gv.Cells[2].Text);
                    int schedulecnt = int.Parse(gv.Cells[11].Text);
                    sql = "select tblinv_bookissue.id, tblinv_bookissue.`Count` from tblinv_bookissue where tblinv_bookissue.ScheduleId="+scheduleid+" ";
                    MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        IssueCount.Text = MyReader.GetValue(1).ToString();
                    }
                    else
                    {
                        sql = "select tblinv_bookschedule.`Count`  from tblinv_bookschedule where tblinv_bookschedule.id=" + scheduleid + "";
                        issuebookreader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                        if (issuebookreader.HasRows)
                        {
                            IssueCount.Text = issuebookreader.GetValue(0).ToString();
                           
                        }
                        
                    }
                    if (schedulecnt == 0)
                    {
                        Chk.Enabled = false;
                        IssueCount.Enabled = false;
                    }
                    else
                    {
                        Chk.Enabled = true;
                        IssueCount.Enabled = true;

                    }


                }
            }

            private void LoadClassToDropDown()
            {
                DataSet Class_Ds = new DataSet();
                ListItem li;
                Drp_Class.Items.Clear();
                Class_Ds = MyUser.MyAssociatedClass();
                if (Class_Ds != null && Class_Ds.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("Select Class", "0");
                    Drp_Class.Items.Add(li);
                    foreach (DataRow dr in Class_Ds.Tables[0].Rows)
                    {
                        li = new ListItem(dr[1].ToString(), dr[0].ToString());
                        Drp_Class.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("No Class Found", "-1");
                    Drp_Class.Items.Add(li);
                }
            }

            private void AddStudentToDropDownStudent()
            {
                Drp_RollNumber.Items.Clear();
                string sql = " SELECT map.StudentId,map.RollNo FROM tblstudentclassmap map inner join tblstudent stud on stud.id= map.Studentid where stud.status=1 and  map.ClassId=" + int.Parse(Drp_Class.SelectedValue.ToString()) + " and map.RollNo<>-1 and map.BatchId=" + MyUser.CurrentBatchId + " order by map.RollNo";
                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_RollNumber.Items.Add(li);
                    }


                }
                else
                {
                    ListItem li = new ListItem("No students found", "-1");
                    Drp_RollNumber.Items.Add(li);
                }
            }

            private void AddStudentNameToDropDownStudent()
            {
                Drp_Student.Items.Clear();
                string sql = " SELECT map.StudentId,stud.StudentName FROM tblstudentclassmap map inner join tblstudent stud on stud.id= map.Studentid where stud.status=1 and  map.ClassId=" + int.Parse(Drp_Class.SelectedValue.ToString()) + " and map.RollNo<>-1 and map.BatchId=" + MyUser.CurrentBatchId + " order by map.RollNo";
                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_Student.Items.Add(li);
                    }

                }
                else
                {
                    ListItem li = new ListItem("No students found", "-1");
                    Drp_Student.Items.Add(li);
                }
                MyReader.Close();
            }

            private void LoadItemsTodropdown()
            {
                Drp_SpclBookName.Items.Clear();
                DataSet Item_Ds = new DataSet();
                ListItem li;
                int category = int.Parse(Drp_Category.SelectedValue);
                if (category != -1)
                {
                    if (category > 0)
                    {                        
                        Item_Ds = Myinventory.GetAllItems(category,Drp_Location.SelectedValue);
                        if (Item_Ds != null && Item_Ds.Tables[0].Rows.Count > 0)
                        {
                            li = new ListItem("Select Book", "0");
                            Drp_SpclBookName.Items.Add(li);
                            foreach (DataRow dr in Item_Ds.Tables[0].Rows)
                            {
                                li = new ListItem(dr["ItemName"].ToString(), dr["Id"].ToString());
                                Drp_SpclBookName.Items.Add(li);
                            }
                        }
                        else
                        {
                            li = new ListItem("No item found", "-1");
                            Drp_SpclBookName.Items.Add(li);
                        }
                    }
                    else
                    {
                        li = new ListItem("No item found", "-1");
                        Drp_SpclBookName.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("No item found", "-1");
                    Drp_SpclBookName.Items.Add(li);
                }
            }

            private void Loadcategorytodropdown()
            {
                Drp_Category.Items.Clear();
                Drp_issueCategory.Items.Clear();
                DataSet Category_Ds = new DataSet();
                ListItem li;
                Category_Ds = Myinventory.GetAllCategory(0,0);
                if (Category_Ds != null && Category_Ds.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("Select Category", "0");
                    Drp_Category.Items.Add(li);
                    li = new ListItem("All", "0");
                    Drp_issueCategory.Items.Add(li);
                    foreach (DataRow dr in Category_Ds.Tables[0].Rows)
                    {
                        li = new ListItem(dr["Category"].ToString(), dr["Id"].ToString());
                        Drp_Category.Items.Add(li);
                        Drp_issueCategory.Items.Add(li);
                    }

                }
                else
                {
                    li = new ListItem("No category found", "-1");
                    Drp_Category.Items.Add(li);
                    Drp_issueCategory.Items.Add(li);
                    
                }
            }

            private void FindTotalCost()
            {
                Txt_totalCost.Text = "";
                CheckBox chk = new CheckBox();
                TextBox Txt_IssueCount = new TextBox();
                TextBox Txt_Cost = new TextBox();
                //TxtCost
                int issuecount = 0;
                double BasicCost = 0;
                double TotalCost = 0;
                Grd_IssueBook.Columns[1].Visible = true;
                Grd_IssueBook.Columns[2].Visible = true;
                Grd_IssueBook.Columns[3].Visible = true;
                Grd_IssueBook.Columns[4].Visible = true;
                Grd_IssueBook.Columns[5].Visible = true;
                Grd_IssueBook.Columns[6].Visible = true;
                Grd_IssueBook.Columns[7].Visible = true;
                Grd_IssueBook.Columns[14].Visible = true;
                Grd_IssueBook.Columns[15].Visible = true;

                foreach (GridViewRow gr in Grd_IssueBook.Rows)
                {
                    chk = (CheckBox)gr.FindControl("CheckBoxUpdate");
                    Txt_IssueCount = (TextBox)gr.FindControl("TxtIssueCount");
                    Txt_Cost = (TextBox)gr.FindControl("TxtCost");
                    if (chk.Checked)
                    {
                        double.TryParse(gr.Cells[10].Text, out BasicCost);
                        int.TryParse(Txt_IssueCount.Text, out issuecount);
                        double.TryParse(Txt_totalCost.Text, out TotalCost);
                        Txt_Cost.Text = (BasicCost * issuecount).ToString();
                        TotalCost = TotalCost + (BasicCost * issuecount);
                        Txt_totalCost.Text = TotalCost.ToString();
                    }
                    else
                    {
                        Txt_Cost.Text = "";
                    }

                }
                Grd_IssueBook.Columns[1].Visible = false;
                Grd_IssueBook.Columns[2].Visible = false;
                Grd_IssueBook.Columns[3].Visible = false;
                Grd_IssueBook.Columns[4].Visible = false;
                Grd_IssueBook.Columns[5].Visible = false;
                Grd_IssueBook.Columns[6].Visible = false;
                Grd_IssueBook.Columns[7].Visible = false;
                Grd_IssueBook.Columns[14].Visible = false;
                Grd_IssueBook.Columns[15].Visible = false;
            }

            private void LoadLocationToDropDown()
            {
                DataSet Location_Ds = new DataSet();
                ListItem li;
                Drp_Location.Items.Clear();
                Location_Ds = Myinventory.GetLocationName();
                if (Location_Ds != null && Location_Ds.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("- Inventory Location -", "0");
                    Drp_Location.Items.Add(li);
                    foreach (DataRow dr in Location_Ds.Tables[0].Rows)
                    {
                        //Id,Locationname
                        li = new ListItem(dr["Locationname"].ToString(), dr["Id"].ToString());
                        Drp_Location.Items.Add(li);
                    }

                }
                else
                {
                    li = new ListItem("No Location Found", "-1");
                    Drp_Location.Items.Add(li);

                }

            }

        #endregion
    }
}
