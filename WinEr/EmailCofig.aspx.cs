using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using WinBase;
using System.Data.OleDb;
using System.IO;

namespace WinEr
{
    public partial class EmailCofig : System.Web.UI.Page
    {

        private StudentManagerClass MyStudMang;
        private EmailManager Obj_Email;
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet = null;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MyFeeMang = MyUser.GetFeeObj();
            Obj_Email = MyUser.GetEmailObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(875))
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
                    Load_ParentClass_Drp();
                    LoadStaffToGrid();
                    Load_StudentClass_Drp();
                    Btn_Parent_Import.Visible = false;
                    Btn_Student_Import.Visible = false;
                }
                Lbl_StaffErr.Text = "";
                Lbl_StaffNoneErr.Text = "";
                Lbl_StudentErr.Text = "";
                Lbl_ParentErr.Text = "";
            }
        }

//Common region
        private void UnselectStaffGrid(int type)
        {
            CheckBox Chk_Select;
            if (type == 1)
            {
                foreach (GridViewRow gv in Grd_Staff.Rows)
                {
                    Chk_Select = (CheckBox)gv.FindControl("Checksms");
                    Chk_Select.Checked = false;
                }
            }
            if (type == 2)
            {
                foreach (GridViewRow gv in Grd_Parent.Rows)
                {
                    Chk_Select = (CheckBox)gv.FindControl("CheckParentEmail");
                    Chk_Select.Checked = false;
                }
            }
            if (type == 3)
            {
                foreach (GridViewRow gv in Grd_Students.Rows)
                {
                    Chk_Select = (CheckBox)gv.FindControl("CheckStudentEmail");
                    Chk_Select.Checked = false;
                }
            }
        }

        private void SelectStaffGrid(int type)
        {
            CheckBox Chk_Select;
            if (type == 1)
            {
                foreach (GridViewRow gv in Grd_Staff.Rows)
                {
                    Chk_Select = (CheckBox)gv.FindControl("Checksms");
                    Chk_Select.Checked = true;
                }
            }
            if (type == 2)
            {
                foreach (GridViewRow gv in Grd_Parent.Rows)
                {
                    Chk_Select = (CheckBox)gv.FindControl("CheckParentEmail");
                    Chk_Select.Checked = true;
                }

            }
            if (type == 3)
            {

                foreach (GridViewRow gv in Grd_Students.Rows)
                {
                    Chk_Select = (CheckBox)gv.FindControl("CheckStudentEmail");
                    Chk_Select.Checked = true;
                }
                
            }
        }

        private void Update_EmailList(int Id, int type, string EmialID, int smsEnabled)
        {
            string tbl = "";
            if (type == 0)
            {
                tbl = "tbl_emailstudentlist";
            }
            else if (type == 1)
            {
                tbl = "tbl_emailparentlist";
            }
            else if (type == 2)
            {
                tbl = "tbl_emailstafflist";
            }
            Delete_Entry(Id, tbl);

            string sql = "INSERT INTO " + tbl + "(Id,EmailId,Enabled) VALUES(" + Id + ", '" + EmialID + "'," + smsEnabled + ")";
            Obj_Email.m_TransationDb.ExecuteQuery(sql);

        }

        private void Delete_Entry(int Id, string tbl)
        {
            string sql = "DELETE FROM " + tbl + " where Id=" + Id;
            Obj_Email.m_TransationDb.ExecuteQuery(sql);
        }
//Endregion

        #region Staff     

        protected void Btn_Staff_Export_Click(object sender, EventArgs e)
        {
            MyDataSet = Obj_Email.GetStaffEmailDs();
            MyDataSet.Tables[0].Columns.Remove(MyDataSet.Tables[0].Columns[0]);
            MyDataSet.Tables[0].Columns.Remove(MyDataSet.Tables[0].Columns[2]);
            if (MyDataSet != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                if (!WinEr.ExcelUtility.ExportDataSetToExcel(MyDataSet, "Staff Verification.xls"))
                {
                    Lbl_StaffErr.Text = "";
                }
            }
            else
            {
                Lbl_StaffErr.Text = "No data found for exporting";
            }

        }

        private void LoadStaffToGrid()
        {
            DataSet StaffDs = new DataSet();
            StaffDs = Obj_Email.GetStaffEmailDs();
            if (StaffDs != null && StaffDs.Tables[0].Rows.Count > 0)
            {
                Grd_Staff.Columns[0].Visible = true;
                Grd_Staff.Columns[2].Visible = true;
                Grd_Staff.Columns[3].Visible = true;
                Grd_Staff.DataSource = StaffDs;
                Grd_Staff.DataBind();
                FillTemplateFields();
                Grd_Staff.Columns[0].Visible = false;
                Grd_Staff.Columns[2].Visible = false;
                Grd_Staff.Columns[3].Visible = false;
                Pnl_Initial.Visible = true;
                Pnl_Staff.Visible = true;
                Btn_Staff_Import.Visible = true;
            }
            else
            {
                Pnl_Initial.Visible = false;
                Pnl_Staff.Visible = false;
                Grd_Staff.DataSource = null;
                Grd_Staff.DataBind();
                Btn_Staff_Import.Visible = false;
                Lbl_StaffNoneErr.Text = "No staff found";
            }
        }

        private void FillTemplateFields()
        {
            TextBox Txt_EmailId = new TextBox();
            CheckBox Chk = new CheckBox();
            foreach (GridViewRow gr in Grd_Staff.Rows)
            {
                Txt_EmailId = (TextBox)gr.FindControl("Txt_StaffEmailId");
                Chk = (CheckBox)gr.FindControl("Checksms");
                Txt_EmailId.Text = gr.Cells[2].Text.Replace("&nbsp;", "");
                int Enabled = 0;
                int.TryParse(gr.Cells[3].Text.Replace("&nbsp;", ""), out Enabled);
                if (Enabled == 1)
                {
                    Chk.Checked = true;
                }
                else
                {
                    Chk.Checked = false;
                }


            }
        }

        protected void Btn_UpdateStaff_Click(object sender, EventArgs e)
        {
            int Id = 0, type = 2, smsEnabled = 0, tempsms = 1;
            string ph = "";
            int success = 1;
            try
            {
                Obj_Email.CreateTansationDb();
                foreach (GridViewRow gv in Grd_Staff.Rows)
                {
                    smsEnabled = 0;
                    TextBox Txt_Email = (TextBox)gv.FindControl("Txt_StaffEmailId");
                    Id = int.Parse(gv.Cells[0].Text.ToString());
                    CheckBox chk = (CheckBox)gv.FindControl("Checksms");
                    if (chk.Checked)
                    {
                        smsEnabled = 1;
                    }
                    ph = gv.Cells[2].Text.ToString().Replace("&nbsp;","");
                    int.TryParse(gv.Cells[3].Text.ToString(), out tempsms);
                    if (Txt_Email.Text != ph || smsEnabled != tempsms)
                    {
                        Update_EmailList(Id, type, Txt_Email.Text, smsEnabled);
                        string sql = " update tbluser set EmailId = '" + Txt_Email.Text + "' where Id = " + Id + "";
                        Obj_Email.m_TransationDb.ExecuteQuery(sql);

                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Email.List", "Staff Email. List Configuration is changed", 1, Obj_Email.m_TransationDb);

                    }
                }
            }
            catch (Exception ex)
            {
                success=0;
                Obj_Email.EndFailTansationDb();
                WC_MsgBox.ShowMssage("Updation failed,Please try again later");

            }
            if (success == 1)
            {
                Obj_Email.EndSucessTansationDb();
                WC_MsgBox.ShowMssage("Updated successfully");
                LoadStaffToGrid();
            }
        }

  

        protected void Lnk_Selectall_Click(object sender, EventArgs e)
        {
            if (Lnk_Selectall.Text == "Select All")
            {
                Lnk_Selectall.Text = "None";
                SelectStaffGrid(1);
            }
            else
            {
                Lnk_Selectall.Text = "Select All";
                UnselectStaffGrid(1);
            }
        }

  
        #endregion

        #region Parent

        private void LoadParentEmailListToGrid()
        {
            Load_Parents();
        }

        private void Load_ParentClass_Drp()
        {
            Drp_ParentClass.Items.Clear();
            ListItem li = new ListItem("Select any class", "0");
            Drp_ParentClass.Items.Add(li);

            MyDataSet = MyUser.MyAssociatedClass();
            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {

                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_ParentClass.Items.Add(li);

                }
            }
            else
            {
                li = new ListItem("No Class present", "-2");
                Drp_ParentClass.Items.Add(li);
            }
            Drp_ParentClass.SelectedIndex = 0;
        }

        private DataSet GetParentDataSet()
        {
            int ClassId = int.Parse(Drp_ParentClass.SelectedValue);
            string temp = "";
            temp = " AND tblstudentclassmap.ClassId=" + ClassId;
            string sql = "";
            sql = "SELECT tblstudentclassmap.StudentId,tblstudent.GardianName as Parent,tblstudent.StudentName,tbl_emailparentlist.EmailId,tbl_emailparentlist.Enabled FROM tblstudentclassmap LEFT OUTER JOIN tbl_emailparentlist ON tblstudentclassmap.StudentId=tbl_emailparentlist.Id INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudent.`Status`=1 " + temp + " ORDER BY tblstudentclassmap.RollNo asc";
            MyDataSet = Obj_Email.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return MyDataSet;
        }

        private void Load_Parents()
        {
            Grd_Parent.DataSource = null;
            Grd_Parent.DataBind();
            Lnk_Parent.Visible = false;
            int ClassId = int.Parse(Drp_ParentClass.SelectedValue);
            if (ClassId != -2)
            {
                if (ClassId > 0)
                {
                    MyDataSet = GetParentDataSet();
                    if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
                    {
                        Grd_Parent.Columns[0].Visible = true;
                        Grd_Parent.Columns[3].Visible = true;
                        Grd_Parent.Columns[4].Visible = true;
                        Grd_Parent.DataSource = MyDataSet;
                        Grd_Parent.DataBind();
                        Grd_Parent.Columns[0].Visible = false;
                        Grd_Parent.Columns[3].Visible = false;
                        Grd_Parent.Columns[4].Visible = false;
                        Lnk_Parent.Visible = true;
                        FillTextBox();
                        Pnl_ParentEmailDisplay.Visible = true;
                        Btn_Parent_Import.Visible = true;
                    }
                    else
                    {
                        Pnl_ParentEmailDisplay.Visible = false;
                        Btn_Parent_Import.Visible = false;
                        Lbl_ParentErr.Text = "Parent Not Found For Selected Class";
                    }
                }
                else
                {
                    Lbl_ParentErr.Text = "Select any class";
                }
            }
        }

        private void FillTextBox()
        {

            TextBox Txt_EmailId = new TextBox();
            CheckBox Chk = new CheckBox();
            foreach (GridViewRow gr in Grd_Parent.Rows)
            {
                Txt_EmailId = (TextBox)gr.FindControl("Txt_ParentEmailId");
                Chk = (CheckBox)gr.FindControl("CheckParentEmail");
                Txt_EmailId.Text = gr.Cells[3].Text.Replace("&nbsp;", "");
                int Enabled = 0;
                int.TryParse(gr.Cells[4].Text.Replace("&nbsp;", ""), out Enabled);
                if (Enabled == 1)
                {
                    Chk.Checked = true;
                }
                else
                {
                    Chk.Checked = false;
                }


            }
        }

        protected void Drp_ParentClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_Parents();
        }

        protected void Lnk_Parent_Click(object sender, EventArgs e)
        {
            if (Lnk_Parent.Text == "Select All")
            {
                Lnk_Parent.Text = "None";
                SelectStaffGrid(2);
            }
            else
            {
                Lnk_Parent.Text = "Select All";
                UnselectStaffGrid(2);
            }
        }

        protected void Btn_ParentExport_Click(object sender, EventArgs e)
        {
            MyDataSet = GetParentDataSet();
            MyDataSet.Tables[0].Columns.Remove(MyDataSet.Tables[0].Columns[0]);
            MyDataSet.Tables[0].Columns.Remove(MyDataSet.Tables[0].Columns[3]);
            if (MyDataSet != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                if (!WinEr.ExcelUtility.ExportDataSetToExcel(MyDataSet, "Parent Verification.xls"))
                {
                    Lbl_ParentErr.Text = "";
                }
            }
            else
            {
                Lbl_ParentErr.Text = "No data found for exporting";
            }
        }

         private void Update_Parent()
        {
            int Id = 0, type = 1, smsEnabled = 0, tempsms = 1;
            string Email = "";
            foreach (GridViewRow gv in Grd_Parent.Rows)
            {
                smsEnabled = 0;
                TextBox Txt_Email = (TextBox)gv.FindControl("Txt_ParentEmailId");
                Id = int.Parse(gv.Cells[0].Text.ToString().Replace("&nbsp;", ""));
                CheckBox chk = (CheckBox)gv.FindControl("CheckParentEmail");
                if (chk.Checked)
                {
                    smsEnabled = 1;
                }
                Email = gv.Cells[3].Text.ToString().Replace("&nbsp;", "");
                int.TryParse(gv.Cells[4].Text.ToString().Replace("&nbsp;", ""), out tempsms);
                if (Txt_Email.Text != Email || smsEnabled != tempsms)
                {
                    Update_EmailList(Id, type, Txt_Email.Text, smsEnabled);
                    string sql = " update tblstudent set Email = '" + Txt_Email.Text + "' where Id = " + Id + "";
                    Obj_Email.m_TransationDb.ExecuteQuery(sql);
                }
            }
        }

        protected void Btn_ParentUpdate_Click(object sender, EventArgs e)
        {
            string msg = "";
            Lbl_ParentErr.Text = "";
                try
                {

                    Obj_Email.CreateTansationDb();
                    Update_Parent();
                    Obj_Email.EndSucessTansationDb();
                    Lbl_ParentErr.Text = "Successfully Updated";
                    Load_Parents();
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Email. List", "Parents Email. List Configuration is changed", 1);

                }
                catch
                {
                    Obj_Email.EndFailTansationDb();
                    Lbl_ParentErr.Text = "Error While Updating";
                }
          
        }


        #endregion


        #region Student

        private void Load_StudentClass_Drp()
        {
            Drp_StudentClass.Items.Clear();
            ListItem li = new ListItem("Select any class", "0");
            Drp_StudentClass.Items.Add(li);

            MyDataSet = MyUser.MyAssociatedClass();
            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {

                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_StudentClass.Items.Add(li);

                }

            }
            else
            {
                li = new ListItem("No Class present", "-2");
                Drp_StudentClass.Items.Add(li);
            }
            Drp_StudentClass.SelectedIndex = 0;
        }

        protected void Drp_StudentClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_Students();
        }

        private void Load_Students()
        {
            Grd_Students.DataSource = null;
            Grd_Students.DataBind();
            Lnk_Student.Visible = false;
            int ClassId = int.Parse(Drp_StudentClass.SelectedValue);
            if (ClassId != -2)
            {
                if (ClassId > 0)
                {
                    MyDataSet = GetStudentDataSet();
                    if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
                    {
                        Grd_Students.Columns[0].Visible = true;
                        Grd_Students.Columns[2].Visible = true;
                        Grd_Students.Columns[3].Visible = true;
                        Grd_Students.DataSource = MyDataSet;
                        Grd_Students.DataBind();
                        Grd_Students.Columns[0].Visible = false;
                        Grd_Students.Columns[2].Visible = false;
                        Grd_Students.Columns[3].Visible = false;
                        FillStudentTextBox();
                        Lnk_Student.Visible = true;
                        Btn_Student_Import.Visible = true;
                    }
                    else
                    {
                        Btn_Student_Import.Visible = false;
                        Lbl_StudentErr.Text = "Student Not Found For Selected Class";
                    }
                }
            }
        }
        
        private void FillStudentTextBox()
        {

            TextBox Txt_EmailId = new TextBox();
            CheckBox Chk = new CheckBox();
            foreach (GridViewRow gr in Grd_Students.Rows)
            {
                Txt_EmailId = (TextBox)gr.FindControl("Txt_StudentEmailId");
                Chk = (CheckBox)gr.FindControl("CheckStudentEmail");
                Txt_EmailId.Text = gr.Cells[2].Text.Replace("&nbsp;", "");
                int Enabled = 0;
                int.TryParse(gr.Cells[3].Text.Replace("&nbsp;", ""), out Enabled);
                if (Enabled == 1)
                {
                    Chk.Checked = true;
                }
                else
                {
                    Chk.Checked = false;
                }


            }
        }

        private DataSet GetStudentDataSet()
        {
            int ClassId = int.Parse(Drp_StudentClass.SelectedValue);
            string temp = "";
                temp = " AND tblstudentclassmap.ClassId=" + ClassId;
            string sql = "";
            sql = "SELECT tblstudentclassmap.StudentId,tblstudent.StudentName, tbl_emailstudentlist.EmailId,tbl_emailstudentlist.Enabled FROM tblstudentclassmap LEFT OUTER JOIN tbl_emailstudentlist ON tblstudentclassmap.StudentId=tbl_emailstudentlist.Id INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudent.`Status`=1 " + temp + " ORDER BY tblstudent.StudentName";
            MyDataSet = Obj_Email.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return MyDataSet;
        }

        protected void Lnk_Student_Click(object sender, EventArgs e)
        {

            if (Lnk_Student.Text == "Select All")
            {
                Lnk_Student.Text = "None";
                SelectStaffGrid(3);
            }
            else
            {
                Lnk_Student.Text = "Select All";
                UnselectStaffGrid(3);
            }
        }

        protected void Btn_StudentExport_Click(object sender, EventArgs e)
        {
            MyDataSet = GetStudentDataSet();
            MyDataSet.Tables[0].Columns.Remove(MyDataSet.Tables[0].Columns[0]);
            MyDataSet.Tables[0].Columns.Remove(MyDataSet.Tables[0].Columns[2]);
            if (MyDataSet != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                if (!WinEr.ExcelUtility.ExportDataSetToExcel(MyDataSet, "Staff Verification.xls"))
                {
                    Lbl_StudentErr.Text = "";
                }
            }
            else
            {
                Lbl_StudentErr.Text = "No data found for exporting";
            }
        }

        protected void Btn_StudentUpdate_Click(object sender, EventArgs e)
        {
            string msg = "";
            Lbl_ParentErr.Text = "";
            if (int.Parse(Drp_StudentClass.SelectedValue) > 0)
            {
                try
                {

                    Obj_Email.CreateTansationDb();
                    Update_Student();
                    Obj_Email.EndSucessTansationDb();
                    Lbl_StudentErr.Text = "Successfully Updated";
                    Load_Students();
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Email.List", "Students Email. List Configuration is changed", 1);

                }
                catch
                {
                    Obj_Email.EndFailTansationDb();
                    Lbl_StudentErr.Text = "Error While Updating";
                }
            }
            else
            {
                Lbl_StudentErr.Text = "Select any class";
            }

        }

        private void Update_Student()
        {
            int Id = 0, type = 0, smsEnabled = 0, tempsms = 1;
            string Email = "";
            foreach (GridViewRow gv in Grd_Students.Rows)
            {
                smsEnabled = 0;
                TextBox Txt_Email = (TextBox)gv.FindControl("Txt_StudentEmailId");
                Id = int.Parse(gv.Cells[0].Text.ToString().Replace("&nbsp;", ""));
                CheckBox chk = (CheckBox)gv.FindControl("CheckStudentEmail");
                if (chk.Checked)
                {
                    smsEnabled = 1;
                }
                Email = gv.Cells[2].Text.ToString().Replace("&nbsp;", "");
                int.TryParse(gv.Cells[3].Text.ToString().Replace("&nbsp;", ""), out tempsms);
                if (Txt_Email.Text != Email || smsEnabled != tempsms)
                {
                    Update_EmailList(Id, type, Txt_Email.Text, smsEnabled);
                    string sql = " update tblstudent set Email = '" + Txt_Email.Text + "' where Id = " + Id + "";
                    Obj_Email.m_TransationDb.ExecuteQuery(sql);
                }
            }
        }



        #endregion


        private bool Check_validity_ToUpload(out string message)
        {
            message = null;
            bool _valid = true;

            if (UploadExcel.PostedFile == null)
            {
                _valid = false;
                message = "Select a file to upload";
            }
            if (!validExtension())
            {
                _valid = false;
                message = "Selected File is not in excel Format{Only .xlt and .xls formats are supportable}";
            }
            return _valid;

        }

        private bool validExtension()
        {
            bool _valid = false;
            string fileExtension = System.IO.Path.GetExtension(UploadExcel.FileName).ToLower();
            string[] allowedExtensions = { ".xlt", ".xls", ".ods" };
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    _valid = true;
                }
            }
            return _valid;

        }

        private bool saveTheExcelFile(out string _FileName)
        {
            bool _valid = true;
            _FileName = null;
            try
            {
                _FileName = "Temp" + UploadExcel.FileName.ToString();

                UploadExcel.SaveAs(MyUser.FilePath + "\\UpImage\\" + _FileName);
            }
            catch
            {
                _valid = false;
            }
            return _valid;
        }

        private DataSet prepareDataset_FromExcel(string _physicalpath)
        {
            OleDbConnection con;
            System.Data.DataTable dt = null;
            //Connection string for oledb
            string conn = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + _physicalpath + "; Extended Properties=Excel 8.0;";
            con = new OleDbConnection(conn);
            try
            {
                con.Open();
                //get the sheet name in to a table
                dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                String[] excelsheets = new String[dt.Rows.Count];
                int i = 0;
                //using foreach get the sheet name in a string array called excelsheets[]
                foreach (DataRow dr in dt.Rows)
                {
                    excelsheets[i] = dr["TABLE_NAME"].ToString();
                    i++;
                }
                // here  manaually give the sheet number in the string array
                DataSet ds = new DataSet();
                foreach (string temp in excelsheets)
                {
                    // Query to get the data for the excel sheet 
                    //temp is the sheet name
                    string query = "select * from [" + temp + "]";
                    OleDbDataAdapter adp = new OleDbDataAdapter(query, con);
                    adp.Fill(ds, temp);//fill the excel sheet data into a dataset ds
                }
                return ds;

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                DataSet ds = null;
                return ds;
            }
            finally
            {
                con.Close();
            }

        }

        protected void Btn_Student_Import_Click(object sender, EventArgs e)
        {
            this.MPE_Import.Show();
            Lbl_IpmortMessage.Text = "";
            Lbl_type.Text = "2";
        }


        protected void Btn_Parent_Import_Click(object sender, EventArgs e)
        {
            this.MPE_Import.Show();
            Lbl_IpmortMessage.Text = "";
            Lbl_type.Text = "1";
        }

        protected void Btn_Staff_Import_Click(object sender, EventArgs e)
        {
            this.MPE_Import.Show();
            Lbl_IpmortMessage.Text = "";
            Lbl_type.Text = "0";
        }

        private void DisplayNumbers(DataSet MydataSet)
        {
            if (Lbl_type.Text == "0")//staff
            {
                LoadStaffGrid(MydataSet, Grd_Staff);
            }
            else if (Lbl_type.Text == "1")//parent
            {
                LoadParentGrid(MydataSet, Grd_Parent);
            }
            else//student
            {
                LoadStudentGrid(MydataSet, Grd_Students);
            }
        }

        private void LoadParentGrid(DataSet MyDataSet, GridView Grd_Parent)
        {
            string Id, ExcelName, Number, GrdName;
            foreach (GridViewRow gv in Grd_Parent.Rows)
            {
                GrdName = gv.Cells[1].Text.ToString().ToLower();
                TextBox Txt_Email = (TextBox)gv.FindControl("Txt_ParentEmailId");
                if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < MyDataSet.Tables[0].Rows.Count; i++)
                    {
                        Id = MyDataSet.Tables[0].Rows[i][0].ToString().Trim();
                        ExcelName = MyDataSet.Tables[0].Rows[i][1].ToString().ToLower().Trim();
                        Number = MyDataSet.Tables[0].Rows[i][2].ToString().Trim();
                        if (Id != "") //means slno is present if not stop the creation of dataset
                        {
                            if (GrdName == ExcelName)
                            {
                                Txt_Email.Text = Number.ToString();
                            }
                        }
                        else
                            break;
                    }
                }
            }
        }

        private void LoadStaffGrid(DataSet MyDataSet, GridView Grd_Staff)
        {
            string Id, ExcelName, Number, GrdName;
            foreach (GridViewRow gv in Grd_Staff.Rows)
            {
                GrdName = gv.Cells[1].Text.ToString().ToLower();
                TextBox Txt_Email = (TextBox)gv.FindControl("Txt_StaffEmailId");
                if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < MyDataSet.Tables[0].Rows.Count; i++)
                    {
                        Id = MyDataSet.Tables[0].Rows[i][0].ToString().Trim();
                        ExcelName = MyDataSet.Tables[0].Rows[i][1].ToString().ToLower().Trim();
                        Number = MyDataSet.Tables[0].Rows[i][2].ToString().Trim();
                        if (Id != "") //means slno is present if not stop the creation of dataset
                        {
                            if (GrdName == ExcelName)
                            {
                                Txt_Email.Text = Number.ToString();
                            }
                        }
                        else
                            break;
                    }
                }
            }
        }

        private void LoadStudentGrid(DataSet MyDataSet, GridView Grd_Student)
        {
            string Id, ExcelName, Number, GrdName;
            foreach (GridViewRow gv in Grd_Student.Rows)
            {
                GrdName = gv.Cells[1].Text.ToString().ToLower();
                TextBox Txt_Email = (TextBox)gv.FindControl("Txt_StudentEmailId");
                if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < MyDataSet.Tables[0].Rows.Count; i++)
                    {
                        Id = MyDataSet.Tables[0].Rows[i][0].ToString().Trim();
                        ExcelName = MyDataSet.Tables[0].Rows[i][1].ToString().ToLower().Trim();
                        Number = MyDataSet.Tables[0].Rows[i][2].ToString().Trim();
                        if (Id != "") //means slno is present if not stop the creation of dataset
                        {
                            if (GrdName == ExcelName)
                            {
                                Txt_Email.Text = Number.ToString();
                            }
                        }
                        else
                            break;
                    }
                }
            }
        }


        protected void Btn_ImportFromExcel_Click(object sender, EventArgs e)
        {
            string message, _FileName;
            if (Check_validity_ToUpload(out message))
            {
                if (saveTheExcelFile(out _FileName))
                {

                    string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath("")) + "\\UpImage\\" + _FileName;

                    MyDataSet = prepareDataset_FromExcel(_physicalpath);      //prepare dataset from the excel
                    DisplayNumbers(MyDataSet);
                    //builddataset(MydataSet); //create new data set based on our requirements
                    File.Delete(MyUser.FilePath + "\\UpImage\\" + _FileName); //delete the file
                }
                else
                {
                    Lbl_IpmortMessage.Text = "Not Able To Upload The Excel File. Try Again Later";
                    this.MPE_Import.Show();
                }
            }
            else
            {
                Lbl_IpmortMessage.Text = message;
                this.MPE_Import.Show();
            }
        }
      

    }
}
