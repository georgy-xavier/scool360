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
    public partial class ScheduleBookToClass : System.Web.UI.Page
    {

        private ConfigManager MyConfigMang;
        private KnowinUser MyUser;
        private WinBase.Inventory Myinventory;

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyConfigMang = MyUser.GetConfigObj();
            Myinventory = MyUser.GetInventoryObj();
            if (MyConfigMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(859))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    Pnl_DisplayItem.Visible = false;
                    LoadCategoryToDropDown();
                    LoadClassToDropDown();
                    LoadStudentsToGrid();
                   // Btn_SelectStudent.Visible = false;
                    Img_Search.Visible = false;

                }
            }
        }

        protected void Img_Search_Click(object sender, ImageClickEventArgs e)
        {
            MPE_SelectStudent.Show();
        }

        protected void Btn_Show_Click(object sender, EventArgs e)
        {
            Lbl_Err.Text = "";
            GetItemDetails();
        }

        protected void Btn_Schedule_Click(object sender, EventArgs e)
        {
            Lbl_Err.Text = "";            
           ChecksheduleBookdetails();
        }   

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {

            LoadStudentsToGrid();
        }

       


        protected void Btn_CancelSchedule_Click(object sender, EventArgs e)
        {
            Lbl_Err.Text = "";
            Hdn_Value.Value = "1";
            CheckBox chk = new CheckBox();
            string bookname="";
            int chked = 0;
            int issue = 0;
            int cancel = 0;
            foreach (GridViewRow gr in Grd_SelectItem.Rows)
            {
                chk = (CheckBox)gr.FindControl("ChkFee");
                if (chk.Checked)
                {
                    chked=1;

                    int bookid = int.Parse(gr.Cells[1].Text);
                    int classid = int.Parse(Drp_Class.SelectedValue);
                    int batchId = MyUser.CurrentBatchId;
                    string status = gr.Cells[7].Text;
                    bookname = gr.Cells[2].Text;                  

                }
            }
            if(chked==0)
            {
                WC_MessageBox.ShowMssage("Select any Item");
            }
            if (issue == 0)
            {
                SaveCancelDetailsToTable();
            }

        }


        protected void Btn_magok_Click(object sender, EventArgs e)
        {
            SaveCancelDetailsToTable();
        }

        protected void Btn_MsgCancel_Click(object sender, EventArgs e)
        {

        }
        protected void ChkFee_CheckedChanged(object sender, EventArgs e)
        {
            MPE_SelectStudent.Show();
        }

        protected void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            MPE_SelectStudent.Show();
        }

        protected void Btn_Ok_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (GridViewRow gr in Grd_SelectStudent.Rows)
            {
                CheckBox ChkSelectStudent = new CheckBox();
                ChkSelectStudent = (CheckBox)gr.FindControl("ChkFee");
                if (ChkSelectStudent.Checked)
                {
                    count++;
                }

            }
            Lbl_StudentCount.Text = "Selected Students: " + count + "";
            //SavesheduleBookdetails();
        }

 
      

        #endregion  

        #region Methods

        private void SavesheduleBookdetails()
        {
            CheckBox chk = new CheckBox();
            TextBox BookCount = new TextBox();
            int chkcount = 0;
            string classname = "";
            string bookname = "";
            int classid = int.Parse(Drp_Class.SelectedValue);
            int save = 0;
            int value = 0;
            foreach (GridViewRow gr in Grd_SelectItem.Rows)
            {
                chk = (CheckBox)gr.FindControl("ChkFee");
                BookCount = (TextBox)gr.FindControl("Txt_Count");
                if (chk.Checked)
                {
                    string status = gr.Cells[7].Text;

                   chkcount++;
                    //if (status == "Pending" || status == "Cancelled")
                    //{
                        Grd_SelectItem.Columns[1].Visible = true;
                        Grd_SelectItem.Columns[4].Visible = true;
                        int itemId = int.Parse(gr.Cells[1].Text);
                        int batchId = MyUser.CurrentBatchId;
                        int count = int.Parse(BookCount.Text);
                        foreach (GridViewRow gv in Grd_SelectStudent.Rows)
                        {
                            CheckBox ChkSelectStudent = new CheckBox();
                            ChkSelectStudent = (CheckBox)gv.FindControl("ChkFee");
                            if (ChkSelectStudent.Checked)
                            {
                                int studentId = int.Parse(gv.Cells[1].Text);
                                Myinventory.ScheduleBookToStudents(classid, itemId, batchId, count, studentId);
                            }
                        }
                        classname = Drp_Class.SelectedItem.Text;
                        bookname = gr.Cells[2].Text;
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Inventory : Schedule Item to class", "Item " + bookname + " is scheduled to " + classname + "", 1);
                        save++;
                        Grd_SelectItem.Columns[1].Visible = false;
                        Grd_SelectItem.Columns[4].Visible = false;
                   // }

                }
            }
            if (save != 0)
            {
                Lbl_Err.Text = "Scheduled successfully";
                GetItemDetails();
            }
        }

        private bool BookAlreadyIssued(int batchId, int bookid, int classid)
        {
            OdbcDataReader ScheduleReader = null;
            bool Issue = false;
            OdbcDataReader IssueReader = null;
            string sql = "select tblinv_bookschedule.Id from tblinv_bookschedule inner join tblinv_bookscheduledetails on tblinv_bookscheduledetails.Id= tblinv_bookschedule.ScheduleId where tblinv_bookscheduledetails.BatchId=" + batchId + " and tblinv_bookschedule.BookId=" + bookid + " and tblinv_bookscheduledetails.ClassId=" + classid + " ";
            ScheduleReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
            if (ScheduleReader.HasRows)
            {
                while (ScheduleReader.Read())
                {
                    sql = "select Id from tblinv_bookissue  where tblinv_bookissue.ScheduleId=" + int.Parse(ScheduleReader.GetValue(0).ToString()) + "";
                    IssueReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
                    if (IssueReader.HasRows)
                    {
                        Issue = true;
                    }

                }
            }
            return Issue;
        }



        private void LoadStudentsToGrid()
        {
            DataSet  StudentDS = null;
            int ClassId = int.Parse(Drp_Class.SelectedValue);
            string sql = "";
                sql = " SELECT map.StudentId,stud.StudentName FROM tblstudentclassmap map inner join tblstudent stud on stud.id= map.Studentid where stud.status=1 and  map.ClassId=" + ClassId + " and map.RollNo<>-1 and map.BatchId=" + MyUser.CurrentBatchId + " order by map.RollNo";
                StudentDS = Myinventory.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (StudentDS!=null && StudentDS.Tables[0].Rows.Count>0)
            {
                Grd_SelectStudent.Columns[1].Visible = true;
                Grd_SelectStudent.DataSource = StudentDS;
                Grd_SelectStudent.DataBind();
                Grd_SelectStudent.Columns[1].Visible = false;
            }
            
        }

        private void SaveCancelDetailsToTable()
        {
            Lbl_Err.Text = "";
            CheckBox chk = new CheckBox();
            string bookname = "";
            int chked = 0;
            int cancel = 0;
            foreach (GridViewRow gr in Grd_SelectItem.Rows)
            {
                chk = (CheckBox)gr.FindControl("ChkFee");
                if (chk.Checked)
                {
                    chked = 1;
                    string status = gr.Cells[7].Text;
                    bookname = gr.Cells[2].Text;
                        cancel = 1;
                        int bookid = int.Parse(gr.Cells[1].Text);
                        int classid = int.Parse(Drp_Class.SelectedValue);
                        int batchId = MyUser.CurrentBatchId;
                        foreach (GridViewRow gv in Grd_SelectStudent.Rows)
                        {
                            CheckBox chkselectstudent = new CheckBox();
                            chkselectstudent = (CheckBox)gv.FindControl("ChkFee");
                            if (chkselectstudent.Checked)
                            {
                                int studentId = int.Parse(gv.Cells[1].Text);
                                Myinventory.CancelSchedule(batchId, bookid, classid, studentId);
                            }
                        }
                }
            }
            if (chked == 0)
            {
                WC_MessageBox.ShowMssage("Select any item");
            }
            if (cancel == 1)
            {

                GetItemDetails();
                Lbl_Err.Text = "Cancelled successfully";
            }
        }

        private void ChecksheduleBookdetails()
        {
            int classid = int.Parse(Drp_Class.SelectedValue);
            CheckBox chk = new CheckBox();
            TextBox BookCount = new TextBox();
            string bookname = "";
            int chkcount = 0;
            int value=0;
            foreach (GridViewRow gr in Grd_SelectItem.Rows)
            {
                
                chk = (CheckBox)gr.FindControl("ChkFee");
                BookCount = (TextBox)gr.FindControl("Txt_Count");
                if (chk.Checked)
                {
                   
                    bookname = gr.Cells[2].Text;
                    chkcount++;
                    int bookId = int.Parse(gr.Cells[1].Text);
                    if(BookScheduledToAllStudentsInClass(bookId))
                    {
                        Lbl_Err.Text = "" + bookname + " is already scheduled to all students,Please unselect it!";
                        value = 1;
                        break;
                    }
                    string status = gr.Cells[7].Text;
                    //if (status == "Scheduled")
                    //{
                    //    chkcount = 1;
                    //    value=1;
                    //    Lbl_Err.Text = "Some items are already scheduled,select the unscheduled items!";
                    //}

                }
            }
            if (value == 0)
            {
                SavesheduleBookdetails();
               
            }
            if (chkcount == 0)
            {
                Lbl_Err.Text = "Select any item";
            }
           

        }

        private void GetItemDetails()
        {
            
            DataSet ItemDetails_Ds = new DataSet();
            Lbl_StudentCount.Text = "";
            int categoryId = int.Parse(Drp_Category.SelectedValue);
            int ClassId = int.Parse(Drp_Class.SelectedValue);
            if (categoryId != -1 && ClassId != -1)
            {
                if (categoryId != 0)
                {
                    if (ClassId != 0)
                    {
                        int StudCount = Myinventory.GetStudentCount(ClassId,MyUser.CurrentBatchId);
                        if (StudCount != 0)
                        {
                            Grd_SelectItem.Columns[1].Visible = true;
                            Grd_SelectItem.Columns[4].Visible = true;
                            Grd_SelectItem.Columns[5].Visible = true;
                            Lbl_StudentCount.Visible = true;
                           Img_Search.Visible = true;
                            Lbl_StudentCount.Text = "Selected Students: " + StudCount + "";
                            Pnl_DisplayItem.Visible = true;
                            int batchid = MyUser.CurrentBatchId;
                            ItemDetails_Ds = Myinventory.DisplayItemDetailstoGrid(categoryId, ClassId, batchid);
                            if (ItemDetails_Ds != null && ItemDetails_Ds.Tables[0].Rows.Count > 0)
                            {
                                Grd_SelectItem.DataSource = ItemDetails_Ds;
                                Grd_SelectItem.DataBind();
                                FillTextBox(ItemDetails_Ds);

                                Grd_SelectItem.Columns[4].Visible = false;
                                Grd_SelectItem.Columns[1].Visible = false;
                                Grd_SelectItem.Columns[5].Visible = false; 
                                Grd_SelectItem.Columns[7].Visible = false;
                               
                            }
                            else
                            {
                                Lbl_Err.Text = "No item exist under this category";
                                Lbl_StudentCount.Visible = false;
                                Img_Search.Visible = false;
                                Pnl_DisplayItem.Visible = false;
                                Grd_SelectItem.DataSource = null;
                                Grd_SelectItem.DataBind();
                            }
                        }
                        else
                        {
                            Lbl_Err.Text = "No student exist in this class";
                            Img_Search.Visible = false;
                            Lbl_StudentCount.Visible = false;
                            Pnl_DisplayItem.Visible = false;
                            Grd_SelectItem.DataSource = null;
                            Grd_SelectItem.DataBind();
                        }

                    }

                    else
                    {
                        Lbl_Err.Text = "Select any class";
                    }
                }
                else
                {
                    Lbl_Err.Text = "Select any category";
                }
            }
            else
            {
                Pnl_DisplayItem.Visible = false;
                Grd_SelectItem.DataSource = null;
                Grd_SelectItem.DataBind();
                Lbl_Err.Text = "No item found";
            }
        }

        private void FillTextBox(DataSet ItemDetails_Ds)
        {
            TextBox txtcount = new TextBox();
            CheckBox chk = new CheckBox();
            foreach (GridViewRow gr in Grd_SelectItem.Rows)
            {

               
                txtcount = (TextBox)gr.FindControl("Txt_Count");
                chk = (CheckBox)gr.FindControl("ChkFee");
                string status = gr.Cells[7].Text;
                txtcount.Enabled = true;
                if (gr.Cells[5].Text.Replace("&nbsp;","") != "")
                {
                    int Schedulecount = int.Parse(gr.Cells[5].Text);
                    txtcount.Text = Schedulecount.ToString();
                    int bookId=int.Parse(gr.Cells[1].Text);
                    if (BookScheduledToAllStudentsInClass(bookId))
                    {
                        txtcount.Enabled = false;
                       // chk.Enabled = false;
                    }
                    
                }
               // int Schedulecount = int.Parse(gr.Cells[6].Text);                
                else
                {
                    txtcount.Text = "1";
                }
            }

        }

        private bool BookScheduledToAllStudentsInClass(int bookId)
        {
            OdbcDataReader Studentreader = null;
            string sql = "";
            bool valid = true;
            sql = "   SELECT map.StudentId FROM tblstudentclassmap map inner join tblstudent  stud on stud.id= map.Studentid where stud.status=1 and  map.ClassId=" + int.Parse(Drp_Class.SelectedValue) + " and map.RollNo<>-1  and map.BatchId=" + MyUser.CurrentBatchId + " and map.StudentId not in(select StudId from tblinv_bookschedule inner join  tblinv_bookscheduledetails on tblinv_bookscheduledetails.Id= tblinv_bookschedule.ScheduleId where tblinv_bookscheduledetails.BookId=" + bookId + "   and tblinv_bookscheduledetails.BatchId=" + MyUser.CurrentBatchId + " and tblinv_bookscheduledetails.ClassId=" + int.Parse(Drp_Class.SelectedValue) + ") order by map.RollNo";
            Studentreader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
            if (Studentreader.HasRows)
            {                
                valid = false;
            }
            return valid;

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


        private void LoadCategoryToDropDown()
        {
            DataSet Category_Ds = new DataSet();
            ListItem li;
            Drp_Category.Items.Clear();
            Category_Ds = Myinventory.GetAllCategory(0,1);
            if (Category_Ds != null && Category_Ds.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("select Category", "0");
                Drp_Category.Items.Add(li);
                foreach (DataRow dr in Category_Ds.Tables[0].Rows)
                {
                    li = new ListItem(dr["Category"].ToString(), dr["Id"].ToString());
                    Drp_Category.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No category found", "-1");
                Drp_Category.Items.Add(li);
            }
        }

        #endregion

       
    }
}
