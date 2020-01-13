using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.OleDb;
using WinBase;
namespace WinEr
{
    public partial class SMSConfig : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private SMSManager MysmsMang;
        //private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MysmsMang = MyUser.GetSMSMngObj();
            if (MyStudMang == null)
            {
                Response.Redirect("sectionerr.htm");
                //no rights for this user.
            }
            else if (MysmsMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(97))
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
                    
                    //some initlization
                    MysmsMang.InitClass();
                    Load_ParentClass_Drp();
                    Load_StudentClass_Drp();

                   

                    int Id = 0;
                    string Type = "";
                    bool _Continue = false;
                    if (Request.QueryString["Id"] != null)
                    {
                        int.TryParse(Request.QueryString["Id"].ToString(), out Id);                                       
                    }
                    if (Request.QueryString["Type"] != null)
                    {
                        Type = Request.QueryString["Type"].ToString();
                    }
                    if (Id > 0 && Type!="")
                    {
                        _Continue = true;
                    }

                    if (_Continue)
                    {
                        Hd_Type.Value = Type;
                        Hd_Id.Value = Id.ToString();
                        int ClassId = 0;

                        if (Type == "Staff")
                        {
                            Tabs.ActiveTabIndex = 0;
                        }
                        else if (Type == "Parent")
                        {
                            Tabs.ActiveTabIndex = 1;
                            ClassId = MyStudMang.GetClassId(Id);
                            if (ClassId > 0)
                                Drp_ParentClass.SelectedValue = ClassId.ToString();
                        }
                        else if (Type == "Student")
                        {
                            Tabs.ActiveTabIndex = 2;
                            ClassId = MyStudMang.GetClassId(Id);
                            if (ClassId > 0)
                                Drp_StudentClass.SelectedValue = ClassId.ToString();

                        }
                    }


                    Load_Grids();
                }
            }
        }

        private void Load_Grids()
        {
            Load_Parents();
            Load_Students();
            Load_Staff();
     
        }

        private void Load_ParentClass_Drp()
        {
            Drp_ParentClass.Items.Clear();
            ListItem li = new ListItem("Select any class", "-2");
            Drp_ParentClass.Items.Add(li);

            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in MydataSet.Tables[0].Rows)
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
       
        private void Load_StudentClass_Drp()
        {
            Drp_StudentClass.Items.Clear();
            ListItem li = new ListItem("Select any class", "-2");
            Drp_StudentClass.Items.Add(li);

            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in MydataSet.Tables[0].Rows)
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
     
        private void Load_Staff()
        {
            Grd_Staff.DataSource = null;
            Grd_Staff.DataBind();
            lbl_Staff_Error.Text = "";
            Lnl_Staff.Visible = false;
            MydataSet = GetStaffDataSet();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_Staff.Columns[0].Visible = true;
                Grd_Staff.Columns[2].Visible = true;
                Grd_Staff.Columns[3].Visible = true;
                Grd_Staff.DataSource = MydataSet;
                Grd_Staff.DataBind();
                Grd_Staff.Columns[0].Visible = false;
                Grd_Staff.Columns[2].Visible = false;
                Grd_Staff.Columns[3].Visible = false;
                Lnl_Staff.Visible = true;
                Load_Staff_PhoneNo_SMS();
            }
            else
            {
                lbl_Staff_Error.Text = "Staff Not Found For Selected Class";
            }
        }

        private DataSet GetStaffDataSet()
        {
            string temp = "";
            string sql = "";
            int ClassId = -1;
            if (ClassId == -1)
            {
                sql = "SELECT tbluser.Id,tbluser.SurName as `Staff Name`,tblsmsstafflist.PhoneNo,tblsmsstafflist.Enabled FROM tbluser LEFT OUTER JOIN tblsmsstafflist ON tbluser.Id=tblsmsstafflist.Id WHERE tbluser.`Status`=1 AND tbluser.RoleId!=1 ORDER BY tbluser.SurName ASC";
            }
            else
            {
                sql = "SELECT tblclassstaffmap.StaffId as Id,tbluser.SurName as `Staff Name`,tblsmsstafflist.PhoneNo,tblsmsstafflist.Enabled FROM tblclassstaffmap LEFT OUTER JOIN tblsmsstafflist ON tblclassstaffmap.StaffId=tblsmsstafflist.Id INNER JOIN tbluser ON tbluser.Id=tblsmsstafflist.Id WHERE tbluser.`Status`=1 AND tbluser.RoleId!=1 AND tblclassstaffmap.ClassId=" + ClassId + " ORDER BY tbluser.UserName";
            }
            MydataSet = MysmsMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return MydataSet;
        }

        private void Load_Staff_PhoneNo_SMS()
        {
            int i = 0, selectedIndex = -1;
            int enable_status = 1;
            foreach (GridViewRow gv in Grd_Staff.Rows)
            {
                if (Hd_Type.Value == "Staff")
                {
                    if (Hd_Id.Value == gv.Cells[0].Text.ToString())
                    {
                        selectedIndex = i;
                    }
                }
                TextBox Txt_Phone = (TextBox)gv.FindControl("Txt_StaffPhone");
                if (gv.Cells[2].Text.ToString().Trim() != "&nbsp;")
                {
                    Txt_Phone.Text = gv.Cells[2].Text.ToString().Trim();
                }
                CheckBox chk = (CheckBox)gv.FindControl("Checksms");
                int.TryParse(gv.Cells[3].Text.ToString(), out enable_status);
                if (enable_status != 1)
                {
                    chk.Checked = false;
                }
                i++;
            }

                Grd_Staff.SelectedIndex = selectedIndex;
            
        }

        private void Load_Parents()
        {
            Grd_Parent.DataSource = null;
            Grd_Parent.DataBind();
            lbl_Parent_Error.Text = "";
            Lnk_Parent.Visible = false;
            MydataSet = GetParentDataSet();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_Parent.Columns[0].Visible = true;
                Grd_Parent.Columns[3].Visible = true;
                Grd_Parent.Columns[4].Visible = true;
                Grd_Parent.Columns[5].Visible = true;
                Grd_Parent.Columns[6].Visible = true;
                Grd_Parent.DataSource = MydataSet;
                Grd_Parent.DataBind();
                Grd_Parent.Columns[0].Visible = false;
                Grd_Parent.Columns[3].Visible = false;
                Grd_Parent.Columns[4].Visible = false;
                Grd_Parent.Columns[5].Visible = false;
                Grd_Parent.Columns[6].Visible = false;
                Lnk_Parent.Visible = true;
                Load_Parent_PhoneNo_SMS();
            }
            else
            {
                lbl_Parent_Error.Text = "Parent Not Found For Selected Class";
            }
        }

        private DataSet GetParentDataSet()
        {
            int ClassId = int.Parse(Drp_ParentClass.SelectedValue);
            
            string temp = "";
            if (ClassId != -1) //|| (ClassId != -2))
            {
                temp = " AND tblstudentclassmap.ClassId=" + ClassId;
            }
            string sql = "";
            sql = "SELECT tblstudentclassmap.StudentId,tblstudent.GardianName as Parent,tblstudent.StudentName,tblsmsparentlist.PhoneNo,tblsmsparentlist.SecondaryNo,tblsmsparentlist.Enabled,  ifNull( IsActiveNativeLanguage,0) as IsActiveNativeLanguage  FROM tblstudentclassmap LEFT OUTER JOIN tblsmsparentlist ON tblstudentclassmap.StudentId=tblsmsparentlist.Id INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudent.`Status`=1" + temp + " ORDER BY tblstudent.StudentName asc";
            MydataSet = MysmsMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

           
            return MydataSet;

        }
        protected void Grd_Parent_RowBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox Chk = (CheckBox)e.Row.FindControl("chkNativelanguage");
                if (int.Parse(e.Row.Cells[5].Text.ToString()) == 1)
                {

                    Chk.Checked = true;
                }
                else
                {
                    Chk.Checked = false;
                }
            }
        }


        private void Load_Parent_PhoneNo_SMS()
        {
            int i = 0,selectedIndex=-1;;
            int enable_status = 1;
            foreach (GridViewRow gv in Grd_Parent.Rows)
            {
                if (Hd_Type.Value == "Parent")
                {
                    if (Hd_Id.Value == gv.Cells[0].Text.ToString())
                    {
                        selectedIndex = i;
                    }
                }
                TextBox Txt_Phone = (TextBox)gv.FindControl("Txt_ParentPhone");
                if (gv.Cells[3].Text.ToString().Trim() != "&nbsp;")
                {
                    Txt_Phone.Text = gv.Cells[3].Text.ToString().Trim();
                }
                TextBox Txt_SecondPhone = (TextBox)gv.FindControl("Txt_ParentSecondaryPhone");
                if (gv.Cells[6].Text.ToString().Trim() != "&nbsp;")
                {
                    Txt_SecondPhone.Text = gv.Cells[6].Text.ToString().Trim();
                }
                CheckBox chk = (CheckBox)gv.FindControl("Checksms");
                int.TryParse(gv.Cells[4].Text.ToString(), out enable_status);
                if (enable_status != 1)
                {
                    chk.Checked = false;
                }
                i++;
            }


                Grd_Parent.SelectedIndex = selectedIndex;

        }

        private void Load_Students()
        {
            Grd_Student.DataSource = null;
            Grd_Student.DataBind();
            lbl_Student_Error.Text = "";
            Lnk_Student.Visible = false;
            MydataSet = GetStudentDataSet();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_Student.Columns[0].Visible = true;
                Grd_Student.Columns[2].Visible = true;
                Grd_Student.Columns[3].Visible = true;
                Grd_Student.DataSource = MydataSet;
                Grd_Student.DataBind();
                Grd_Student.Columns[0].Visible = false;
                Grd_Student.Columns[2].Visible = false;
                Grd_Student.Columns[3].Visible = false;
                Lnk_Student.Visible = true;
                Load_Student_PhoneNo_SMS();
            }
            else
            {
                lbl_Student_Error.Text = "Student Not Found For Selected Class";
            }
        }

        private DataSet GetStudentDataSet()
        {
            int ClassId = int.Parse(Drp_StudentClass.SelectedValue);
            string temp = "";
            if (ClassId != -1)
            {
                temp = " AND tblstudentclassmap.ClassId=" + ClassId;
            }
            string sql = "";
            sql = "SELECT tblstudentclassmap.StudentId,tblstudent.StudentName,tblsmsstudentlist.PhoneNo,tblsmsstudentlist.Enabled FROM tblstudentclassmap LEFT OUTER JOIN tblsmsstudentlist ON tblstudentclassmap.StudentId=tblsmsstudentlist.Id INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudent.`Status`=1" + temp + " ORDER BY tblstudent.StudentName";
            MydataSet = MysmsMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return MydataSet;
        }

        private void Load_Student_PhoneNo_SMS()
        {
            int i = 0, selectedIndex = -1;
            int enable_status = 1;
            foreach (GridViewRow gv in Grd_Student.Rows)
            {
                if (Hd_Type.Value == "Student")
                {
                    if (Hd_Id.Value == gv.Cells[0].Text.ToString())
                    {
                        selectedIndex = i;
                    }
                }
                TextBox Txt_Phone = (TextBox)gv.FindControl("Txt_StudentPhone");
                if (gv.Cells[2].Text.ToString().Trim() != "&nbsp;")
                {
                    Txt_Phone.Text = gv.Cells[2].Text.ToString().Trim();
                }
                CheckBox chk = (CheckBox)gv.FindControl("Checksms");
                int.TryParse(gv.Cells[3].Text.ToString(), out enable_status);
                if (enable_status != 1)
                {
                    chk.Checked = false;
                }
                
                i++;
            }


                Grd_Student.SelectedIndex = selectedIndex;
          
        }

        # region Upload From Excel

  

        protected void Btn_ImportFromExcel_Click(object sender, EventArgs e)
        {
            string message, _FileName;
            if (Check_validity_ToUpload(out message))
            {
                if (saveTheExcelFile(out _FileName))
                {

                    string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath("")) + "\\UpImage\\" + _FileName;

                    MydataSet = prepareDataset_FromExcel(_physicalpath);      //prepare dataset from the excel
                    DisplayNumbers(MydataSet);
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

        private void DisplayNumbers(DataSet MydataSet)
        {
            if (Lbl_type.Text=="0")//staff
           {
             LoadStaffGrid(MydataSet, Grd_Staff);
           }
            else if (Lbl_type.Text == "1")//parent
           {
               LoadParentGrid(MydataSet, Grd_Parent);
           }
           else//student
           {
               LoadStudentGrid(MydataSet, Grd_Student);
           }
        }

        private void LoadParentGrid(DataSet MyDataSet, GridView Grd_Parent)
        {
            string Id, ExcelName, Number,Number2, GrdName;
            foreach (GridViewRow gv in Grd_Parent.Rows)
            {
                GrdName = gv.Cells[1].Text.ToString().ToLower();
                TextBox Txt_Ph = (TextBox)gv.FindControl("Txt_ParentPhone");
                TextBox Txt_Ph2 = (TextBox)gv.FindControl("Txt_ParentSecondaryPhone");
                if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < MyDataSet.Tables[0].Rows.Count; i++)
                    {
                        Id = MyDataSet.Tables[0].Rows[i][0].ToString().Trim();
                        ExcelName = MyDataSet.Tables[0].Rows[i][1].ToString().ToLower().Trim();
                        Number = MyDataSet.Tables[0].Rows[i][2].ToString().Trim();
                        Number2 = MyDataSet.Tables[0].Rows[i][3].ToString().Trim();
                        if (Id != "") //means slno is present if not stop the creation of dataset
                        {
                            if (GrdName == ExcelName)
                            {
                                Txt_Ph.Text = Number.ToString();
                                Txt_Ph2.Text = Number2.ToString();
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
                TextBox Txt_Ph = (TextBox)gv.FindControl("Txt_StaffPhone");
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
                                Txt_Ph.Text = Number.ToString();
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
            string Id, ExcelName, Number,GrdName;
            foreach (GridViewRow gv in Grd_Student.Rows)
            {
                GrdName = gv.Cells[1].Text.ToString().ToLower();
                TextBox Txt_Ph = (TextBox)gv.FindControl("Txt_StudentPhone");
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
                                Txt_Ph.Text = Number.ToString();
                            }
                        }
                        else
                            break;
                    }
                }
            }
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
            string[] allowedExtensions = { ".xlt", ".xls",".ods" };
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    _valid = true;
                }
            }
            return _valid;

        }

        # endregion 



        private void Update_Parent()
        {
            int Id = 0, type = 1, smsEnabled = 0, tempsms = 1,nativelanguage=0;
            string ph = "",ph2="";
            foreach (GridViewRow gv in Grd_Parent.Rows)
            {
                smsEnabled = 0;
                nativelanguage = 0;

                TextBox Txt_Phone = (TextBox)gv.FindControl("Txt_ParentPhone");
                TextBox Txt_Phone_secondery = (TextBox)gv.FindControl("Txt_ParentSecondaryPhone");
                Id = int.Parse(gv.Cells[0].Text.ToString());
                CheckBox chk = (CheckBox)gv.FindControl("Checksms");
                if (chk.Checked)
                {
                    smsEnabled = 1;
                }
                CheckBox chknativelanguage = (CheckBox)gv.FindControl("chkNativelanguage");
                if (chknativelanguage.Checked)
                {
                    nativelanguage = 1;
                }

                ph = gv.Cells[3].Text.ToString();
                ph2 = gv.Cells[7].Text.ToString();
                int.TryParse(gv.Cells[4].Text.ToString(), out tempsms);
                if (Txt_Phone.Text != ph || smsEnabled != tempsms || Txt_Phone_secondery.Text != ph2)
                {
                    Update_PhoneList(Id, type, Txt_Phone.Text, smsEnabled);
                    string sql = " update tblstudent set OfficePhNo = '" + Txt_Phone.Text + "' where Id = " + Id + "";
                    MyStudMang.m_TransationDb.ExecuteQuery(sql);
                    sql = " update tblsmsparentlist set PhoneNo = '" + Txt_Phone.Text + "',SecondaryNo='" + Txt_Phone_secondery.Text + "',IsActiveNativeLanguage=" + nativelanguage + " where Id = " + Id + "";
                    MyStudMang.m_TransationDb.ExecuteQuery(sql);
                }
            }
        }

        private void Update_Staff()
        {
            int Id = 0, type = 2, smsEnabled = 0, tempsms = 1;
            string ph = "";
            foreach (GridViewRow gv in Grd_Staff.Rows)
            {
                smsEnabled = 0;
                TextBox Txt_Phone = (TextBox)gv.FindControl("Txt_StaffPhone");
                Id = int.Parse(gv.Cells[0].Text.ToString());
                CheckBox chk = (CheckBox)gv.FindControl("Checksms");
                if (chk.Checked)
                {
                    smsEnabled = 1;
                }
                ph = gv.Cells[2].Text.ToString();
                int.TryParse(gv.Cells[3].Text.ToString(), out tempsms);
                if (Txt_Phone.Text != ph || smsEnabled != tempsms)
                {
                    Update_PhoneList(Id, type, Txt_Phone.Text, smsEnabled);
                string sql = " update tblstaffdetails set PhoneNumber = '" + Txt_Phone.Text + "' where UserId = " + Id + "";
                MyStudMang.m_TransationDb.ExecuteQuery(sql);
                }
            }
        }

        private void Update_Student()
        {
            int Id = 0, type = 0, smsEnabled = 0, tempsms = 1;
            string ph = "";
            foreach (GridViewRow gv in Grd_Student.Rows)
            {
                smsEnabled = 0;
                TextBox Txt_Phone = (TextBox)gv.FindControl("Txt_StudentPhone");
                Id = int.Parse(gv.Cells[0].Text.ToString());
                CheckBox chk = (CheckBox)gv.FindControl("Checksms");
                if (chk.Checked)
                {
                    smsEnabled = 1;
                }
                ph = gv.Cells[2].Text.ToString();
                int.TryParse(gv.Cells[3].Text.ToString(), out tempsms);
                if (Txt_Phone.Text != ph || smsEnabled != tempsms)
                {
                    Update_PhoneList(Id, type, Txt_Phone.Text, smsEnabled);
                    string sql = " update tblstudent set ResidencePhNo = '" + Txt_Phone.Text + "' where Id = " + Id + "";
                    MyStudMang.m_TransationDb.ExecuteQuery(sql);
                }
            }

        }

        private void Update_PhoneList(int Id, int type, string phno, int smsEnabled)
        {
            string tbl = "";
            if (type == 0)
            {
                tbl = "tblsmsstudentlist";
            }
            else if (type == 1)
            {
                tbl = "tblsmsparentlist";
            }
            else if (type == 2)
            {
                tbl = "tblsmsstafflist";
            }
            Delete_Entry(Id, tbl);

            string sql = "INSERT INTO " + tbl + "(Id,PhoneNo,Enabled) VALUES(" + Id + ", '" + phno + "'," + smsEnabled + ")";
            MyStudMang.m_TransationDb.ExecuteQuery(sql);

        }

        private void Delete_Entry(int Id, string tbl)
        {
             string sql = "DELETE FROM "+tbl+" where Id="+ Id;
             MyStudMang.m_TransationDb.ExecuteQuery(sql);
        }

        private bool Data_Correct(out string msg,string _type)
        {
            msg = "";
            bool _continue = true;

            if (_type=="student")
            {
                foreach (GridViewRow gv in Grd_Student.Rows)
                {
                    TextBox Txt_Phone = (TextBox)gv.FindControl("Txt_StudentPhone");                    
                    if (Txt_Phone.Text!="0" && Txt_Phone.Text.Length < 10 )
                    {
                        _continue = false;
                        msg = "Phone number should contain atleast 10 digit or it should be 0";
                        break;
                    }
                }
            }
            if (_type == "parent")
            {
                foreach (GridViewRow gv in Grd_Parent.Rows)
                {
                    TextBox Txt_Phone = (TextBox)gv.FindControl("Txt_ParentPhone");
                    TextBox Txt_Phone2 = (TextBox)gv.FindControl("Txt_ParentSecondaryPhone");
                    if ((Txt_Phone.Text != "0" && Txt_Phone.Text.Length < 10) || (Txt_Phone2.Text != "0" && Txt_Phone2.Text.Length < 10))
                    {
                        _continue = false;
                        msg = "Phone number should contain atleast 10 digit or it should be 0";
                        break;
                    }
                }
            }
            if (_type == "staff")
            {
                foreach (GridViewRow gv in Grd_Staff.Rows)
                {
                    TextBox Txt_Phone = (TextBox)gv.FindControl("Txt_StaffPhone");
                    if (Txt_Phone.Text != "0" && Txt_Phone.Text.Length < 10)
                    {
                        _continue = false;
                        msg = "Phone number should contain atleast 10 digit or it should be 0";
                        break;
                    }
                }
            }

            return _continue;
        }

        protected void Grd_Student_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Student.PageIndex = e.NewPageIndex;
            Load_Students();
        }

        protected void Grd_Parent_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Parent.PageIndex = e.NewPageIndex;
            Load_Parents();
        }

        protected void Grd_Staff_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Staff.PageIndex = e.NewPageIndex;
            Load_Staff();
        }

        protected void Lnl_Staff_Click(object sender, EventArgs e)
        {
            if (Lnl_Staff.Text == "Select All")
            {
                Lnl_Staff.Text = "None";
                SelectStaffGrid();
            }
            else
            {
                Lnl_Staff.Text = "Select All";
                UnselectStaffGrid();
            }
        }

        private void UnselectStaffGrid()
        {
            CheckBox Chk_Select;
            foreach (GridViewRow gv in Grd_Staff.Rows)
            {
                Chk_Select = (CheckBox)gv.FindControl("Checksms");
                Chk_Select.Checked = false;
            }
        }

        private void SelectStaffGrid()
        {
            CheckBox Chk_Select;
            foreach (GridViewRow gv in Grd_Staff.Rows)
            {
                Chk_Select = (CheckBox)gv.FindControl("Checksms");
                Chk_Select.Checked = true;
            }
        }

        protected void Lnk_Parent_Click(object sender, EventArgs e)
        {
            if (Lnk_Parent.Text == "Select All")
            {
                Lnk_Parent.Text = "None";
                SelectParentfGrid();
            }
            else
            {
                Lnk_Parent.Text = "Select All";
                UnselectParentGrid();
            }
        }

        private void UnselectParentGrid()
        {
            CheckBox Chk_Select;
            foreach (GridViewRow gv in Grd_Parent.Rows)
            {
                Chk_Select = (CheckBox)gv.FindControl("Checksms");
                Chk_Select.Checked = false;
            }
        }

        private void SelectParentfGrid()
        {
            CheckBox Chk_Select;
            foreach (GridViewRow gv in Grd_Parent.Rows)
            {
                Chk_Select = (CheckBox)gv.FindControl("Checksms");
                Chk_Select.Checked = true;
            }
        }

        protected void Lnk_Student_Click(object sender, EventArgs e)
        {
            if (Lnk_Student.Text == "Select All")
            {
                Lnk_Student.Text = "None";
                SelectStudentGrid();
            }
            else
            {
                Lnk_Student.Text = "Select All";
                UnselectStudentGrid();
            }
        }

        private void UnselectStudentGrid()
        {
            CheckBox Chk_Select;
            foreach (GridViewRow gv in Grd_Student.Rows)
            {
                Chk_Select = (CheckBox)gv.FindControl("Checksms");
                Chk_Select.Checked = false;
            }
        }

        private void SelectStudentGrid()
        {
            CheckBox Chk_Select;
            foreach (GridViewRow gv in Grd_Student.Rows)
            {
                Chk_Select = (CheckBox)gv.FindControl("Checksms");
                Chk_Select.Checked = true;
            }
        }

        protected void Drp_StudentClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            Lbl_S_error.Text = "";
            Load_Students();
        }

        protected void Drp_ParentClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_P_error.Text = "";
            Load_Parents();
        }


        protected void Btn_Student_Export_Click(object sender, EventArgs e)
        {
            MydataSet = GetStudentDataSet();
            MydataSet.Tables[0].Columns.Remove(MydataSet.Tables[0].Columns[0]);
            MydataSet.Tables[0].Columns.Remove(MydataSet.Tables[0].Columns[2]);
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                if (!WinEr.ExcelUtility.ExportDataSetToExcel(MydataSet, "Student Verification.xls"))
                {
                    Lbl_S_error.Text = "";
                }
            }
            else
            {
                Lbl_S_error.Text = "No data found for exporting";
            }          
        }

        protected void Btn_Student_Import_Click(object sender, EventArgs e)
        {
            this.MPE_Import.Show();
            Lbl_IpmortMessage.Text = "";
            Lbl_type.Text = "2";            
        }

        protected void Btn_Student_Update_Click(object sender, EventArgs e)
        {
            string msg = "";
            Lbl_S_error.Text = "";
            if (Data_Correct(out msg,"student"))
            {
                try
                {

                    MyStudMang.CreateTansationDb();
                    Update_Student();
                
                    MyStudMang.EndSucessTansationDb();
                    Lbl_S_error.Text = "Successfully Updated";
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "SMS-PhoneNo List", "Students PhoneNo List Configuration is changed", 1);

                }
                catch
                {
                    MyStudMang.EndFailTansationDb();
                    Lbl_S_error.Text = "Error While Updating";
                }
            }
            else
            {
                Lbl_S_error.Text = msg;
            } 
        }

        protected void Btn_Student_Cancel_Click(object sender, EventArgs e)
        {

            Load_Students();

            Lbl_S_error.Text = "";
        }


        protected void Btn_Parent_Export_Click(object sender, EventArgs e)
        {
            MydataSet = GetParentDataSet();
            MydataSet.Tables[0].Columns.Remove(MydataSet.Tables[0].Columns[0]);
            MydataSet.Tables[0].Columns.Remove(MydataSet.Tables[0].Columns[5]);
            MydataSet.Tables[0].Columns.Remove(MydataSet.Tables[0].Columns[4]);
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                if (!WinEr.ExcelUtility.ExportDataSetToExcel(MydataSet, "Parent Verification.xls"))
                {
                    lbl_P_error.Text = "";
                }
            }
            else
            {
                lbl_P_error.Text = "No data found for exporting";
            }
        }

        protected void Btn_Parent_Import_Click(object sender, EventArgs e)
        {
            this.MPE_Import.Show();
            Lbl_IpmortMessage.Text = "";
            Lbl_type.Text = "1";
        }

        protected void Btn_Parent_Update_Click(object sender, EventArgs e)
        {
            string msg = "";
            lbl_P_error.Text = "";
            if (Data_Correct(out msg,"parent"))
            {
                try
                {

                    MyStudMang.CreateTansationDb();
                    Update_Parent();
                    MyStudMang.EndSucessTansationDb();
                    lbl_P_error.Text = "Successfully Updated";
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "SMS-PhoneNo List", "Parents PhoneNo List Configuration is changed", 1);

                }
                catch
                {
                    MyStudMang.EndFailTansationDb();
                    lbl_P_error.Text = "Error While Updating";
                }
            }
            else
            {
                lbl_P_error.Text = msg;
            } 
        }

        protected void Btn_Parent_Cancel_Click(object sender, EventArgs e)
        {
            Load_Parents();

            lbl_P_error.Text = "";
        }



        protected void Btn_Staff_Export_Click(object sender, EventArgs e)
        {
            MydataSet = GetStaffDataSet();
            MydataSet.Tables[0].Columns.Remove(MydataSet.Tables[0].Columns[0]);
            MydataSet.Tables[0].Columns.Remove(MydataSet.Tables[0].Columns[2]);
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                if (!WinEr.ExcelUtility.ExportDataSetToExcel(MydataSet, "Staff Verification.xls"))
                {
                    Lbl_Sf_error.Text = "";
                }
            }
            else
            {
                Lbl_Sf_error.Text = "No data found for exporting";
            }

        }

        protected void Btn_Staff_Import_Click(object sender, EventArgs e)
        {
            this.MPE_Import.Show();
            Lbl_IpmortMessage.Text = "";
            Lbl_type.Text = "0";
        }

        protected void Btn_Staff_Update_Click(object sender, EventArgs e)
        {

            string msg = "";
            Lbl_Sf_error.Text = "";
            if (Data_Correct(out msg,"staff"))
            {
                try
                {

                    MyStudMang.CreateTansationDb();
                    Update_Staff();
                    MyStudMang.EndSucessTansationDb();
                    Lbl_Sf_error.Text = "Successfully Updated";
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "SMS-PhoneNo List", "Staffs PhoneNo List Configuration is changed", 1);

                }
                catch
                {
                    MyStudMang.EndFailTansationDb();
                    Lbl_Sf_error.Text = "Error While Updating";
                }
            }
            else
            {
                Lbl_Sf_error.Text = msg;
            } 

        }

        protected void Btn_Staff_Cancel_Click(object sender, EventArgs e)
        {
            Load_Staff();
            Lbl_Sf_error.Text = "";
        }
    }
}
