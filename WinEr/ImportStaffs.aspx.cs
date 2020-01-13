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
using System.Net.Mail;

namespace WinEr
{
    public partial class ImportStaffs : System.Web.UI.Page
    {
        private Incident MyIncedent;
        private StaffManager MyStaffMang;
        private ClassOrganiser MyClassMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet = null;
        private SchoolClass objSchool = null;

        protected void Page_PreInit(Object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
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
            MyStaffMang = MyUser.GetStaffObj();
            MyClassMang = MyUser.GetClassObj();
            MyIncedent = MyUser.GetIncedentObj();
            if (MyStaffMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(18))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (!IsPostBack)
                {


                    // Panel3.Visible = false;

                    //some initlization
                    /*
                     * tbluser :  :UserName,Password,EmailId,SurName,LastLogin,CreationTime,RoleId,CanLogin,Status
                     *
                     *tblstaffdetails::    UserId,JoiningDate,Address,Sex,Experience,ExpDescription,Designation,PhoneNumber,EduQualifications,Dob
                     */

                    Clear();

                }
                else
                {
                    lbluncorrect.Text = "";
                    lblcorrect.Text = "";
                    lblerror.Text = "";
                }
            }

        }

        private void Clear()
        {
            Grd_CorrectDetails.DataSource = null;
            Grd_CorrectDetails.DataBind();

            Grd_UnCorrectDtls.DataSource = null;
            Grd_UnCorrectDtls.DataBind();

            btn_upload.Enabled = false;
            this.CorrectExceldtls.Visible = false;
            this.UnCorrectExcelDtls.Visible = false;
        }
        private bool saveTheExcelFile(out string _FileName)
        {
            bool _valid = true;
            _FileName = null;
            try
            {
                _FileName = FileUpload_Excel.FileName.ToString();
                string filpath = Server.MapPath("TemporaryFileManager");
                FileUpload_Excel.SaveAs(filpath + "\\" + _FileName);
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

            if (FileUpload_Excel.PostedFile == null)
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
            string fileExtension = System.IO.Path.GetExtension(FileUpload_Excel.FileName).ToLower();
            string[] allowedExtensions = { ".xlt", ".xls" };
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    _valid = true;
                }
            }
            return _valid;

        }
        private DataSet create_manualdataset()
        {
            DataSet ds_manual = new DataSet();
            DataTable dt_manual = new DataTable();
            //tbl_user
            dt_manual.Columns.Add("UserName");
            dt_manual.Columns.Add("Password");
            dt_manual.Columns.Add("EmailId");
            dt_manual.Columns.Add("SurName");
            dt_manual.Columns.Add("LastLogin");
            dt_manual.Columns.Add("CreationTime");
            dt_manual.Columns.Add("RoleId)");
            dt_manual.Columns.Add("CanLogin");
            dt_manual.Columns.Add("Status");
            ////tblstaffdetails
            dt_manual.Columns.Add("UserId");
            dt_manual.Columns.Add("Date Of Join(dd)");
            dt_manual.Columns.Add("Date Of Join(mm)");
            dt_manual.Columns.Add("Date Of Join(yyyy)");
            dt_manual.Columns.Add("Address");
            dt_manual.Columns.Add("Sex");
            dt_manual.Columns.Add("Experience");
            //dt_manual.Columns.Add("ExpDescription");
            dt_manual.Columns.Add("Designation");
            dt_manual.Columns.Add("PhoneNumber");
            dt_manual.Columns.Add("EduQualifications");
            dt_manual.Columns.Add("Date Of Birth(dd)");
            dt_manual.Columns.Add("Date Of Birth(mm)");
            dt_manual.Columns.Add("Date Of Birth(yyyy)");
            dt_manual.Columns.Add("IsLogin(yes/no)");
            ds_manual.Tables.Add(dt_manual);
            return ds_manual;
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
        private DataSet DataFormatedDataSet(DataSet MydataSet)
        {
            char[] replacechr = { '\'', '\\', '/' };
            DataSet FormatedDate = new DataSet();
            DataTable dtcorrect;
            DataRow dr;

            FormatedDate.Tables.Add(new DataTable("correctexceldatas"));
            dtcorrect = FormatedDate.Tables["correctexceldatas"];

            //foreach (DataRow Dr in DS_Manual.Tables[0].Rows)
            //{

            //    dtcorrect.Columns.Add(Dr["SoftwareField"].ToString());
            //}
            dtcorrect.Columns.Add("Id");
            dtcorrect.Columns.Add("SurName*");
            dtcorrect.Columns.Add("UserName");
            dtcorrect.Columns.Add("EmailId");
            dtcorrect.Columns.Add("Password");
            dtcorrect.Columns.Add("Role");
            dtcorrect.Columns.Add("Date Of Join(dd)");
            dtcorrect.Columns.Add("Date Of Join(mm)");
            dtcorrect.Columns.Add("Date Of Join(yyyy)");
            ////tblstaffdetails
            dtcorrect.Columns.Add("Address");
            dtcorrect.Columns.Add("Sex");
            dtcorrect.Columns.Add("Experience");
            dtcorrect.Columns.Add("Phone");
            dtcorrect.Columns.Add("Designation");
            dtcorrect.Columns.Add("Date Of Birth(dd)");
            dtcorrect.Columns.Add("Date Of Birth(mm)");
            dtcorrect.Columns.Add("Date Of Birth(yyyy)");
            dtcorrect.Columns.Add("EduQualifications");
            dtcorrect.Columns.Add("AadharNo");
            dtcorrect.Columns.Add("PanNo");
            dtcorrect.Columns.Add("IsLogin(yes/no)");

            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Dr_Staff in MydataSet.Tables[0].Rows)
                {
                    dr = dtcorrect.NewRow();
                    dr["Id"] = Dr_Staff["SlNO"].ToString();
                    dr["SurName*"] = Dr_Staff["SurName*"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    dr["UserName"] = Dr_Staff["UserName"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    dr["EmailId"] = Dr_Staff["EmailId"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    dr["Password"] = Dr_Staff["Password"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    dr["Role"] = Dr_Staff["Role"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    dr["Date Of Join(dd)"] = Dr_Staff["Date Of Join(dd)"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    dr["Date Of Join(mm)"] = Dr_Staff["Date Of Join(mm)"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    dr["Date Of Join(yyyy)"] = Dr_Staff["Date Of Join(yyyy)"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    dr["Address"] = Dr_Staff["Address"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    dr["Sex"] = Dr_Staff["Sex"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    dr["Experience"] = Dr_Staff["Experience"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    dr["Phone"] = Dr_Staff["Phone"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    dr["Designation"] = Dr_Staff["Designation"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    dr["Date Of Birth(dd)"] = Dr_Staff["Date Of Birth(dd)"].ToString();
                    dr["Date Of Birth(mm)"] = Dr_Staff["Date Of Birth(mm)"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    dr["Date Of Birth(yyyy)"] = Dr_Staff["Date Of Birth(yyyy)"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    dr["EduQualifications"] = Dr_Staff["EduQualifications"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    dr["AadharNo"] = Dr_Staff["AadharNo"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    dr["PanNo"] = Dr_Staff["PanNo"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    dr["IsLogin(yes/no)"] = Dr_Staff["IsLogin(yes/no)"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    FormatedDate.Tables[0].Rows.Add(dr);
                }
            }

            return FormatedDate;
        }
        private void builddataset(DataSet MyDataSet, DataSet excelField_CusotmerMap)
        {
            lblerror.Text = "";
            int arraysize = MyDataSet.Tables[0].Rows.Count, _invalidcounts = 0, _validcounts = 0;
            int[] invalidrowId = new int[arraysize];
            int[] validrowId = new int[arraysize];
            string errormsg;
            string[,] invalidRows_description = new string[arraysize, 2];

            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                this.UnCorrectExcelDtls.Visible = true;
                this.CorrectExceldtls.Visible = true;
                btn_upload.Enabled = true;
                for (int i = 0; i < MyDataSet.Tables[0].Rows.Count; i++)
                {

                    if (MyDataSet.Tables[0].Rows[i][0].ToString() != "") //means slno is present if not stop the creation of dataset
                    {
                        if (AllValuesInRowRCorrect(i, MyDataSet.Tables[0].Rows[i], out errormsg)) //check any column empty if so return false
                        {
                            validrowId[_validcounts] = i;
                            _validcounts += 1;
                        }
                        else //some empty values so consider that row as invalid 
                        {
                            invalidrowId[_invalidcounts] = i;
                            invalidRows_description[_invalidcounts, 1] = errormsg;
                            _invalidcounts += 1;
                        }
                    }
                    else
                        break;
                }



                if (_validcounts > 0)  //if some valid rows present make them ready to bind to grid by validate further (check all syntax and formats here)
                {
                    this.CorrectExceldtls.Visible = true;
                    lblcorrect.Text = "";
                    build_ValidDataset(invalidrowId, _invalidcounts, _validcounts, validrowId, MyDataSet, excelField_CusotmerMap, invalidRows_description);
                }
                else   //all entries r un correct and bind these to grid by preparing dataset(buildInvalidDataset())
                {
                    lblerror.Text = "Selected excel file contains some invalid entries";
                    this.CorrectExceldtls.Visible = false;
                }
                if (_invalidcounts > 0)
                {
                    this.UnCorrectExcelDtls.Visible = true;
                    lbluncorrect.Text = "";
                    buildInvalidDataset(_invalidcounts, invalidrowId, MyDataSet, excelField_CusotmerMap, invalidRows_description);
                }
                else
                {
                    this.UnCorrectExcelDtls.Visible = false;
                    lblerror.Text = "Selected excel file is valid";
                    Grd_UnCorrectDtls.DataSource = null;
                    Grd_UnCorrectDtls.DataBind();
                }
            }
            else
            {
                Clear();
                lblerror.Text = "Nothing to display. The excel may be empty";
            }
            //}
            //catch 
            //{
            //    this.UnCorrectExcelDtls.Visible = false;
            //    this.CorrectExceldtls.Visible = false;
            //    lblerror.Text = "Unable to read data from the selected excel file";
            //}


        }
        private bool AllValuesInRowRCorrect(int i, DataRow dataRow, out string errormsg)
        {
            //DateTime tryparsedatetime;
            bool _validrow = true;
            errormsg = "";
            string _DOB = "";
            string _JNDT = "";
            if (dataRow["Id"].ToString().Trim() == "")//Id
            {
                _validrow = false;
                errormsg = "Empty Id";
                return _validrow;
            }
            if (_validrow && dataRow["SurName*"].ToString().Trim() == "")//surname
            {
                _validrow = false;
                errormsg = "Invalid SurName*";
                return _validrow;
            }
            if (_validrow && dataRow["UserName"].ToString().Trim() == "")//username
            {
                _validrow = false;
                errormsg = "Empty UserName";
                return _validrow;
            }
            if (!MyStaffMang.IsValidStaffId(dataRow["UserName"].ToString().Trim()))
            {
                _validrow = false;
                errormsg = "UserName already present ";
                return _validrow;
            }
            if (_validrow && dataRow["EmailId"].ToString().Trim() == "")//emailid
            {
                _validrow = false;
                errormsg = "Empty EmailId";
                return _validrow;
            }

            //if (_validrow && dataRow["Password"].ToString().Trim() == "")//password
            //{
            //    _validrow = false;
            //    errormsg = "Empty password";
            //    return _validrow;
            //}
            if (_validrow && dataRow["Role"].ToString().Trim() == "" && check_roleexist(dataRow["Role"].ToString().Trim()))//role
            {
                _validrow = false;
                errormsg = "Invalid Role";
                return _validrow;
            }
            if (_validrow && dataRow["Date Of Join(dd)"].ToString().Trim() == "")//Date Of Join
            {
                _validrow = false;
                errormsg = "Empty Date Of Join(dd)";
                return _validrow;
            }
            if (_validrow && dataRow["Date Of Join(mm)"].ToString().Trim() == "")//Date Of Join
            {
                _validrow = false;
                errormsg = "Empty Date Of Join(mm)";
                return _validrow;
            }
            if (_validrow && dataRow["Date Of Join(yyyy)"].ToString().Trim() == "")//Date Of Join
            {
                _validrow = false;
                errormsg = "Empty Date Of Join(yyyy)";
                return _validrow;
            }
            if (_validrow)
            {
                try
                {
                    DateTime _jndate = new DateTime(int.Parse(dataRow["Date Of Join(yyyy)"].ToString().Trim()), int.Parse(dataRow["Date Of Join(mm)"].ToString().Trim()), int.Parse(dataRow["Date Of Join(dd)"].ToString().Trim()));
                    _JNDT = _jndate.ToString("dd/MM/yyyy");
                }
                catch
                {
                    _validrow = false;
                    errormsg = "Date Of Join entry is wrong";
                    return _validrow;
                }
            }

            if (_validrow && dataRow["Address"].ToString().Trim() == "")//Address
            {
                _validrow = false;
                errormsg = "Empty Address";
                return _validrow;
            }


            if (_validrow && dataRow["Sex"].ToString().Trim() == "")//sex
            {
                _validrow = false;
                errormsg = "Empty Sex";
                return _validrow;
            }
             else
            {
                string _sex = dataRow["Sex"].ToString().Trim().ToLower();
                if (!((dataRow["Sex"].ToString().Trim().ToLower() == "male") || (dataRow["Sex"].ToString().Trim().ToLower() == "female")))
                {
                    _validrow = false;
                    errormsg = "Invalid Sex";
                    return _validrow;
                }
                    
            }
            if (_validrow && dataRow["IsLogin(yes/no)"].ToString().Trim() == "")//login
            {
              
                    _validrow = false;
                    errormsg = "Empty IsLogin";
                    return _validrow;
             
            }
            else
            {
                string _sex = dataRow["IsLogin(yes/no)"].ToString().Trim().ToLower();
                if (!((dataRow["IsLogin(yes/no)"].ToString().Trim().ToLower() == "yes") || (dataRow["IsLogin(yes/no)"].ToString().Trim().ToLower() == "no")))
                {
                    _validrow = false;
                    errormsg = "Invalid IsLogin";
                    return _validrow;
                }
            }
            if (_validrow && dataRow["Experience"].ToString().Trim() == "")//Experience
            {
                _validrow = false;
                errormsg = "Empty Experience";
                return _validrow;
            }
            if (_validrow && dataRow["Experience"].ToString().Trim() != "")//Experience
            {
               float _ExperienceYears = 0;
               if (float.TryParse(dataRow["Experience"].ToString().Trim(), out _ExperienceYears))
               {

               }
               else
               {
                   _validrow = false;
                   errormsg = "Experience invalid";
                   return _validrow;
               }
            }
            if (_validrow && dataRow["Phone"].ToString().Trim() == "")//Phone number
            {
                _validrow = false;
                errormsg = "Empty Phonenumber";
                return _validrow;
            }

            if (_validrow && dataRow["Designation"].ToString().Trim() == "")//designation
            {
                _validrow = false;
                errormsg = "Empty Designation";
                return _validrow;
            }

            if (_validrow && dataRow["Date Of Birth(dd)"].ToString().Trim() == "")//DateOf  birth
            {
                _validrow = false;
                errormsg = "Empty Day Of Date Of Birth(dd)";
                return _validrow;
            }

            if (_validrow && dataRow["Date Of Birth(mm)"].ToString().Trim() == "")//DateOf  birth
            {
                _validrow = false;
                errormsg = "Empty Month Of Date Of Birth(mm)";
                return _validrow;
            }

            if (_validrow && dataRow["Date Of Birth(yyyy)"].ToString().Trim() == "")//DateOf birth
            {
                _validrow = false;
                errormsg = "Empty Year Of Date Of Birth(yyyy)";
                return _validrow;
            }

            if (_validrow)
            {
                try
                {
                    DateTime _dob = new DateTime(int.Parse(dataRow["Date Of Birth(yyyy)"].ToString().Trim()), int.Parse(dataRow["Date Of Birth(mm)"].ToString().Trim()), int.Parse(dataRow["Date Of Birth(dd)"].ToString().Trim()));
                    _DOB = _dob.ToString("dd/MM/yyyy");
                }
                catch
                {
                    _validrow = false;
                    errormsg = "Date Of Birth entry is wrong";
                    return _validrow;
                }
            }
            if (_validrow && !CheckDate(_DOB, _JNDT, out errormsg))
            {
                _validrow = false;
                return _validrow;
            }
            if (_validrow && !mailformate_IsValid(dataRow["EmailId"].ToString().Trim()))
            {
                _validrow = false;
                return _validrow;
            }
            if (_validrow && dataRow["EduQualifications"].ToString().Trim() == "")//Educational qualification
            {
                errormsg = "Empty EduQualifications";
                _validrow = false;
                return _validrow;
            }

            return _validrow;

        }
        private bool CheckDate(string _dob, string _jndob, out string _message)
        {
            bool _valid = true;
            _message = "";
            string _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime Today = DateTime.Now;

            DateTime Date_Birth = MyUser.GetDareFromText(_dob);
            DateTime JoiningDate = MyUser.GetDareFromText(_jndob);

            int Year = Today.Year;
            int Dob_Year = Date_Birth.Year;
            if (Date_Birth > Today)
            {
                _message = "Date of birth is invalid";
                _valid = false;
            }
            else if (Dob_Year >= Year)
            {
                _message = "Date of birth is invalid";
                _valid = false;
            }
            else if (JoiningDate > Today)
            {
                _message = "Date of joining cannot be greater than today";
                _valid = false;
            }
            else if (Date_Birth >= JoiningDate)
            {
                _message = "Date of joining cannot be Less than Date of birth";
                _valid = false;
            }
            return _valid;
        }
        private bool allareAlreadyPresent(string _username, out string errormsg)
        {
            bool found = false;
            bool _allvalid = false;
            errormsg = "";
            string sql;
            found = false;
            sql = "select Id from tbluser WHERE UserName='" + _username + "'";
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    if (MyReader.GetValue(0).ToString().ToLower().Trim() == _username.ToLower().Trim())
                    {
                        found = true;

                    }
                }
            }

            if (found)
            {
                _allvalid = true;
                errormsg = "Username Exist";
            }
            else
            {
                _allvalid = false;

            }

            return _allvalid;
        }
        private bool check_roleexist(string role_name)
        {
            bool _valid = false;
            string sql;
            sql = "select RoleName from tblrole where Id<>1;";
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    if (MyReader.GetValue(0).ToString().ToLower().Trim() == role_name.ToLower().Trim())
                    {
                        _valid = true;

                    }
                }
            }
            return _valid;
        }
        public bool mailformate_IsValid(string emailaddress)
        {

            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            System.Text.RegularExpressions.Regex _Regex = new System.Text.RegularExpressions.Regex(strRegex);
            if (_Regex.IsMatch(emailaddress))
                return (true);
            else
                return (false);

        }
        private void build_ValidDataset(int[] invalidrowId, int _invalidcounts, int _validcounts, int[] validrowId, DataSet MyDataSet, DataSet excelField_CusotmerMap, string[,] invalidRows_description)
        {
            string _username, _email_id, errormsg;
            int _validrowId;
            DataSet correctexceldata = new DataSet();
            DataTable dtcorrect;

            correctexceldata.Tables.Add(new DataTable("correctexceldatas"));
            //some initlization
            /*
             * tbluser :  :UserName,Password,EmailId,SurName,LastLogin,CreationTime,RoleId,CanLogin,Status
             *
             *tblstaffdetails::    UserId,JoiningDate,Address,Sex,Experience,ExpDescription,Designation,PhoneNumber,EduQualifications,Dob
             */
            dtcorrect = correctexceldata.Tables["correctexceldatas"];
            dtcorrect.Columns.Add("UserName");
            dtcorrect.Columns.Add("Password");
            dtcorrect.Columns.Add("EmailId");
            dtcorrect.Columns.Add("SurName");
            dtcorrect.Columns.Add("LastLogin");
            dtcorrect.Columns.Add("CreationTime");
            dtcorrect.Columns.Add("Role");
            dtcorrect.Columns.Add("JoiningDate");
            dtcorrect.Columns.Add("Address");
            dtcorrect.Columns.Add("Sex");
            dtcorrect.Columns.Add("Experience");
            dtcorrect.Columns.Add("PhoneNumber");
            dtcorrect.Columns.Add("Designation");
            dtcorrect.Columns.Add("Dob");
            dtcorrect.Columns.Add("EduQualifications");
            dtcorrect.Columns.Add("AadharNo");
            dtcorrect.Columns.Add("PanNo");
            dtcorrect.Columns.Add("IsLogin(yes/no)");
         
            for (int i = 0; i < _validcounts; i++)
            {
                _validrowId = validrowId[i];
                _username = MyDataSet.Tables[0].Rows[_validrowId]["UserName"].ToString();
                _email_id = MyDataSet.Tables[0].Rows[_validrowId]["EmailId"].ToString();
                if (!allareAlreadyPresent(_username, out errormsg))
                {
                    DataRow drcorrect = dtcorrect.NewRow();
                    drcorrect["UserName"] = MyDataSet.Tables[0].Rows[_validrowId]["UserName"].ToString();
                    drcorrect["Password"] = MyDataSet.Tables[0].Rows[_validrowId]["Password"].ToString();
                    drcorrect["EmailId"] = MyDataSet.Tables[0].Rows[_validrowId]["EmailId"].ToString();
                    //drcorrect["DOB"] = DateTime.Parse(MyDataSet.Tables[0].Rows[_validrowId][3].ToString()).Date.ToString("dd/MM/yyyy"); 
                    drcorrect["SurName"] = MyDataSet.Tables[0].Rows[_validrowId]["SurName*"].ToString();
                    //_rolid = int.Parse(MyDataSet.Tables[0].Rows[_validrowId]["Role"].ToString());
                    drcorrect["Role"] = MyDataSet.Tables[0].Rows[_validrowId]["Role"].ToString();
                    drcorrect["JoiningDate"] = MyUser.GerFormatedDatVal(new DateTime(int.Parse(MyDataSet.Tables[0].Rows[_validrowId]["Date Of Join(yyyy)"].ToString()), int.Parse(MyDataSet.Tables[0].Rows[_validrowId]["Date Of Join(mm)"].ToString()), int.Parse(MyDataSet.Tables[0].Rows[_validrowId]["Date Of Join(dd)"].ToString())));
                    drcorrect["Address"] = MyDataSet.Tables[0].Rows[_validrowId]["Address"].ToString();
                    drcorrect["Sex"] = MyDataSet.Tables[0].Rows[_validrowId]["Sex"].ToString();
                    drcorrect["Experience"] = MyDataSet.Tables[0].Rows[_validrowId]["Experience"].ToString();
                    drcorrect["PhoneNumber"] = MyDataSet.Tables[0].Rows[_validrowId]["Phone"].ToString();
                    drcorrect["Designation"] = MyDataSet.Tables[0].Rows[_validrowId]["Designation"].ToString();

                    drcorrect["Dob"] = MyUser.GerFormatedDatVal(new DateTime(int.Parse(MyDataSet.Tables[0].Rows[_validrowId]["Date Of Birth(yyyy)"].ToString()), int.Parse(MyDataSet.Tables[0].Rows[_validrowId]["Date Of Birth(mm)"].ToString()), int.Parse(MyDataSet.Tables[0].Rows[_validrowId]["Date Of Birth(dd)"].ToString())));
                    drcorrect["EduQualifications"] = MyDataSet.Tables[0].Rows[_validrowId]["EduQualifications"].ToString();
                    drcorrect["AadharNo"] = MyDataSet.Tables[0].Rows[_validrowId]["AadharNo"].ToString();
                    drcorrect["PanNo"] = MyDataSet.Tables[0].Rows[_validrowId]["PanNo"].ToString();
                    drcorrect["IsLogin(yes/no)"] = MyDataSet.Tables[0].Rows[_validrowId]["IsLogin(yes/no)"].ToString();

                    correctexceldata.Tables["correctexceldatas"].Rows.Add(drcorrect);
                }
                else
                {
                    invalidrowId[_invalidcounts] = _validrowId;
                    invalidRows_description[_invalidcounts, 1] = errormsg;
                    _invalidcounts += 1;
                }
            }

            if (correctexceldata.Tables[0].Rows.Count > 0)
            {
                this.CorrectExceldtls.Visible = true;
                lblcorrect.Text = "";
                Session["CorrectValues"] = correctexceldata;

                Grd_CorrectDetails.DataSource = correctexceldata;
                Grd_CorrectDetails.DataBind();

            }
            else
            {
                this.CorrectExceldtls.Visible = false;
                Grd_CorrectDetails.DataSource = null;
                Grd_CorrectDetails.DataBind();

                lblerror.Text = "Excel file contains invalid entries.";
            }

            if (_invalidcounts > 0)
            {
                lbluncorrect.Text = "";

                buildInvalidDataset(_invalidcounts, invalidrowId, MyDataSet, excelField_CusotmerMap, invalidRows_description);
            }
            else
            {
                lblerror.Text = "Selected excel file is valid. ";
            }
        }
        private void buildInvalidDataset(int _invalidcounts, int[] invalidrowId, DataSet MyDataSet, DataSet excelField_CusotmerMap, string[,] invalidRows_description)
        {
            int _invalidrowid = 0;
            int _slno = 0;
            DataSet uncorrectexceldata = new DataSet();
            DataTable dtuncorrect;
            uncorrectexceldata.Tables.Add(new DataTable("uncorrectexceldatas"));
            dtuncorrect = uncorrectexceldata.Tables["uncorrectexceldatas"];
            dtuncorrect.Columns.Add("SlNo");
            dtuncorrect.Columns.Add("UserName");
            dtuncorrect.Columns.Add("Password");
            dtuncorrect.Columns.Add("EmailId");
            dtuncorrect.Columns.Add("SurName");
            dtuncorrect.Columns.Add("Role");
            dtuncorrect.Columns.Add("JoiningDate");
            dtuncorrect.Columns.Add("Address");
            dtuncorrect.Columns.Add("Sex");
            dtuncorrect.Columns.Add("Experience");
            dtuncorrect.Columns.Add("PhoneNumber");
            dtuncorrect.Columns.Add("Designation");
            dtuncorrect.Columns.Add("Dob");
            dtuncorrect.Columns.Add("EduQualifications");
            dtuncorrect.Columns.Add("AadharNo");
            dtuncorrect.Columns.Add("PanNo");
            dtuncorrect.Columns.Add("Description");
            dtuncorrect.Columns.Add("IsLogin(yes/no)");
            // SlNo(0)   StudentName(1)  Sex(2)  DOB(3)  Father/GuardianName(4)  Religion(5)  Caste(6)  AddressPermanent(7) StudentType(8)  AdmissionNo(9) AdmissionDate(10) JoiningBatch(11) Description(12)  
            for (int i = 0; i < _invalidcounts; i++)
            {
                _invalidrowid = invalidrowId[i];

                DataRow druncorrect = dtuncorrect.NewRow();

                _slno += 1;
                druncorrect["SlNo"] = _slno;

                if (MyDataSet.Tables[0].Rows[_invalidrowid][1].ToString().Trim() != "")
                    druncorrect["SurName"] = MyDataSet.Tables[0].Rows[_invalidrowid]["SurName*"].ToString();
                else
                    druncorrect["SurName"] = "_";

                if (MyDataSet.Tables[0].Rows[_invalidrowid]["UserName"].ToString().Trim() != "")
                    druncorrect["UserName"] = MyDataSet.Tables[0].Rows[_invalidrowid]["UserName"].ToString();
                else
                    druncorrect["UserName"] = "_";

                if (MyDataSet.Tables[0].Rows[_invalidrowid]["Date Of Join(yyyy)"].ToString() != "" && MyDataSet.Tables[0].Rows[_invalidrowid]["Date Of Join(mm)"].ToString() != "" && MyDataSet.Tables[0].Rows[_invalidrowid]["Date Of Join(dd)"].ToString() != "")
                {

                    try
                    {
                        druncorrect["JoiningDate"] = MyUser.GerFormatedDatVal(new DateTime(int.Parse(MyDataSet.Tables[0].Rows[_invalidrowid]["Date Of Join(yyyy)"].ToString()), int.Parse(MyDataSet.Tables[0].Rows[_invalidrowid]["Date Of Join(mm)"].ToString()), int.Parse(MyDataSet.Tables[0].Rows[_invalidrowid]["Date Of Join(dd)"].ToString())));
                    }
                    catch
                    {
                        druncorrect["JoiningDate"] = "_";
                    }
                }

                else
                    druncorrect["JoiningDate"] = "_";

                if (MyDataSet.Tables[0].Rows[_invalidrowid]["Password"].ToString().Trim() != "")
                    druncorrect["Password"] = MyDataSet.Tables[0].Rows[_invalidrowid]["Password"].ToString();
                else
                    druncorrect["Password"] = "password";
                if (MyDataSet.Tables[0].Rows[_invalidrowid]["EmailId"].ToString().Trim() != "")
                    druncorrect["EmailId"] = MyDataSet.Tables[0].Rows[_invalidrowid]["EmailId"].ToString();
                else
                    druncorrect["EmailId"] = "_";

                if (MyDataSet.Tables[0].Rows[_invalidrowid]["Role"].ToString().Trim() != "")
                    druncorrect["Role"] = MyDataSet.Tables[0].Rows[_invalidrowid]["Role"].ToString();
                else
                    druncorrect["Role"] = "_";

                if (MyDataSet.Tables[0].Rows[_invalidrowid]["Address"].ToString().Trim() != "")
                    druncorrect["Address"] = MyDataSet.Tables[0].Rows[_invalidrowid]["Address"].ToString();
                else
                    druncorrect["Address"] = "_";

                if (MyDataSet.Tables[0].Rows[_invalidrowid]["Sex"].ToString().Trim() != "")
                    druncorrect["Sex"] = MyDataSet.Tables[0].Rows[_invalidrowid]["Sex"].ToString();
                else
                    druncorrect["Sex"] = "_";

                if (MyDataSet.Tables[0].Rows[_invalidrowid]["IsLogin(yes/no)"].ToString().Trim() != "")
                    druncorrect["IsLogin(yes/no)"] = MyDataSet.Tables[0].Rows[_invalidrowid]["IsLogin(yes/no)"].ToString();
                else
                    druncorrect["IsLogin(yes/no)"] = "_";

                if (MyDataSet.Tables[0].Rows[_invalidrowid]["Experience"].ToString().Trim() != "")
                    druncorrect["Experience"] = MyDataSet.Tables[0].Rows[_invalidrowid]["Experience"].ToString();
                else
                    druncorrect["Experience"] = "_";



                if (MyDataSet.Tables[0].Rows[_invalidrowid]["Phone"].ToString().Trim() != "")
                    druncorrect["PhoneNumber"] = MyDataSet.Tables[0].Rows[_invalidrowid]["Phone"].ToString();
                else
                    druncorrect["PhoneNumber"] = "_";

                if (MyDataSet.Tables[0].Rows[_invalidrowid]["Designation"].ToString().Trim() != "")
                    druncorrect["Designation"] = MyDataSet.Tables[0].Rows[_invalidrowid]["Designation"].ToString();
                else
                    druncorrect["Designation"] = "_";

                if (MyDataSet.Tables[0].Rows[_invalidrowid]["Date Of Birth(yyyy)"].ToString() != "" && MyDataSet.Tables[0].Rows[_invalidrowid]["Date Of Birth(mm)"].ToString() != "" && MyDataSet.Tables[0].Rows[_invalidrowid]["Date Of Birth(dd)"].ToString() != "")
                {
                    try
                    {
                        druncorrect["Dob"] = MyUser.GerFormatedDatVal(new DateTime(int.Parse(MyDataSet.Tables[0].Rows[_invalidrowid]["Date Of Birth(yyyy)"].ToString()), int.Parse(MyDataSet.Tables[0].Rows[_invalidrowid]["Date Of Birth(mm)"].ToString()), int.Parse(MyDataSet.Tables[0].Rows[_invalidrowid]["Date Of Birth(dd)"].ToString())));
                    }
                    catch
                    {
                        druncorrect["Dob"] = "_";
                    }
                }
                else
                    druncorrect["Dob"] = "_";

                if (MyDataSet.Tables[0].Rows[_invalidrowid]["EduQualifications"].ToString().Trim() != "")
                    druncorrect["EduQualifications"] = MyDataSet.Tables[0].Rows[_invalidrowid]["EduQualifications"].ToString();
                else
                    druncorrect["EduQualifications"] = "_";
                if (MyDataSet.Tables[0].Rows[_invalidrowid]["AadharNo"].ToString().Trim() != "")
                    druncorrect["AadharNo"] = MyDataSet.Tables[0].Rows[_invalidrowid]["AadharNo"].ToString();
                else
                    druncorrect["AadharNo"] = "_";
                if (MyDataSet.Tables[0].Rows[_invalidrowid]["PanNo"].ToString().Trim() != "")
                    druncorrect["PanNo"] = MyDataSet.Tables[0].Rows[_invalidrowid]["PanNo"].ToString();
                else
                    druncorrect["PanNo"] = "_";

                druncorrect["Description"] = invalidRows_description[i, 1];


                uncorrectexceldata.Tables["uncorrectexceldatas"].Rows.Add(druncorrect);
            }
            if (uncorrectexceldata.Tables[0].Rows.Count > 0)
            {

                Grd_UnCorrectDtls.Columns[0].Visible = true;

                Grd_UnCorrectDtls.DataSource = uncorrectexceldata;//finally bind the Data into the grid
                Grd_UnCorrectDtls.DataBind();
                Grd_UnCorrectDtls.Columns[0].Visible = false;

                Session["UnCorrectValues"] = uncorrectexceldata;



            }
            else
            {
                Grd_UnCorrectDtls.DataSource = null;
                Grd_UnCorrectDtls.DataBind();

                lblerror.Text = "No UnCorrect Rows To Display.";
            }
        }
        protected void Grd_UnCorrectDtls_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grd_UnCorrectDtls.Columns[0].Visible = true;
            lblselectedrowslno.Text = Grd_UnCorrectDtls.SelectedRow.Cells[0].Text.ToString();
            lblediterror.Text = "";
            MPE_PopUpMessageBox.Show();
            fillAllControlsinpopupwindow();
            Grd_UnCorrectDtls.Columns[0].Visible = false;

        }
        //protected void Grd_UnCorrectDtls_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "select")
        //    {
        //        int index = Convert.ToInt32(e.CommandArgument);
        //        Grd_UnCorrectDtls.Columns[0].Visible = true;
        //        string _staff_sno = Grd_UnCorrectDtls.Rows[index].Cells[0].Text;
        //        lblselectedrowslno.Text = _staff_sno;
        //        lblediterror.Text = "";
        //        MPE_PopUpMessageBox.Show();
        //        fillAllControlsinpopupwindow();
        //        Grd_UnCorrectDtls.Columns[0].Visible = false;
        //    }
        //}
        protected void Grd_UnCorrectDtls_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowState == DataControlRowState.Alternate)
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.cursor='hand'");
                    //e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='gray';this.style.cursor='hand'");
                    //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
                }
                else
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.cursor='hand'");
                    // e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#F7F7DE';");
                }
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Grd_UnCorrectDtls, "Select$" + e.Row.RowIndex);
            }
        }
        private void fillAllControlsinpopupwindow()
        {
            string _role_name = "";
            int role_id;
            DateTime tryparsedate;

            DataSet dt_uncorrectdetail = (DataSet)Session["UnCorrectValues"];

            if (dt_uncorrectdetail != null && dt_uncorrectdetail.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dt_uncorrectdetail.Tables[0].Rows)
                {
                    if (int.Parse(dr["SlNo"].ToString()) == int.Parse(lblselectedrowslno.Text))
                    {
                        lblerrordescription.Text = dr["Description"].ToString().Trim();

                        txtsurname.Text = dr["SurName"].ToString().Trim();
                        if (dr["Sex"].ToString().Equals("female", StringComparison.InvariantCultureIgnoreCase) || dr["Sex"].ToString().Equals("male", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (dr["Sex"].ToString().Equals("female", StringComparison.InvariantCultureIgnoreCase))
                                RdBtnLstSex.SelectedIndex = 1;
                            else
                                RdBtnLstSex.SelectedIndex = 0;
                        }
                        if (dr["IsLogin(yes/no)"].ToString().Trim().Equals("yes", StringComparison.InvariantCultureIgnoreCase) || dr["IsLogin(yes/no)"].ToString().Trim().Equals("no", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (dr["IsLogin(yes/no)"].ToString().Trim().Equals("yes", StringComparison.InvariantCultureIgnoreCase))
                                Rdbislogin.SelectedIndex = 1;
                            else
                                Rdbislogin.SelectedIndex = 0;
                        }
                        if ((dr["Dob"].ToString().Trim() != "") && (dr["Dob"].ToString().Trim() != "_" )&& DateTime.TryParse(General.GetDateTimeFromText(dr["Dob"].ToString().Trim()).ToString(), out tryparsedate))
                            txt_Dob.Text = General.GerFormatedDatVal(tryparsedate);
                        else
                            txt_Dob.Text = "_";

                        if ((dr["JoiningDate"].ToString().Trim() != "") && (dr["JoiningDate"].ToString().Trim() != "_" )&&  DateTime.TryParse(General.GetDateTimeFromText(dr["JoiningDate"].ToString().Trim()).ToString(), out tryparsedate))
                            txtjndate.Text = General.GerFormatedDatVal(tryparsedate);
                        else
                            txtjndate.Text = "_";

                        txtusername.Text = dr["UserName"].ToString().Trim();
                        txtemail.Text = dr["EmailId"].ToString().Trim();
                        Session["_password"] = dr["Password"].ToString().Trim();
                        txtaddress.Text = dr["Address"].ToString().Trim();
                        txtexp.Text = dr["Experience"].ToString().Trim();
                        txtphone.Text = dr["PhoneNumber"].ToString().Trim();
                        txteduqualification.Text = dr["EduQualifications"].ToString().Trim();
                        txtdisgn.Text = dr["Designation"].ToString().Trim();
                        _role_name = dr["Role"].ToString().Trim();
                        txtrole.Text = _role_name;
                        role_id = get_role_id(_role_name);
                        loaddropdownlistsinpopup(role_id);
                    }
                }
            }

        }
        private int get_role_id(string _rolename)
        {
            int role_id = 0;
            string sql = "SELECT Id,RoleName FROM tblrole where Id<>1";
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    if (MyReader.GetValue(1).ToString().ToLower() == _rolename.ToString().ToLower())
                    {
                        role_id = int.Parse(MyReader.GetValue(0).ToString());
                        break;
                    }
                }
            }

            return role_id;
        }
        private void loaddropdownlistsinpopup(int _role_id)
        {
            drplistrolw.Items.Clear();
            string sql = "SELECT Id,RoleName FROM tblrole where Id<>1";
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li1 = new ListItem("Select", "0");
                drplistrolw.Items.Add(li1);
                while (MyReader.Read())
                {
                    ListItem li2 = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    drplistrolw.Items.Add(li2);
                }
                drplistrolw.SelectedValue = _role_id.ToString();

            }
        }
        protected void Btn_save_Click(object sender, EventArgs e)
        {
            string message = null;

            if (NewValuesareValid(out message))
            {
                lblediterror.Text = "";

                movetocorrectGrid();

                deleteselectedRow(int.Parse(lblselectedrowslno.Text));

                lblselectedrowslno.Text = "";

            }
            else
            {
                lblerrordescription.Text = message;
                MPE_PopUpMessageBox.Show();
            }
        }
        private bool NewValuesareValid(out string message)
        {
            DateTime tryparsedatetime_DOB;
            DateTime tryparsedatetime_JNDT;
            bool _validvalues = true;
            message = null;
            if ((txt_Dob.Text != "_") || (txt_Dob.Text != ""))
            {
                string _dob = txt_Dob.Text.ToString().Trim();
                if (!MyUser.TryGetDareFromText(_dob, out tryparsedatetime_DOB))
                {
                    message = "D.O.B is invalid";
                    _validvalues = false;
                }
            }
            else
            {
                message = "D.O.B is empty";
                _validvalues = false;
            }
            if ((txtjndate.Text != "_") || (txtjndate.Text != ""))
            {
                string _admdate = txtjndate.Text.ToString().Trim();
                if (!MyUser.TryGetDareFromText(_admdate, out tryparsedatetime_JNDT))
                {
                    message = "joining date is empty";
                    _validvalues = false;
                }
            }
            else
            {
                _validvalues = false;
                message = "joining date is empty";
            }
            if (Rdbislogin.SelectedItem == null)
            {
                _validvalues = false;
                message = "select login status";
            }
            if (RdBtnLstSex.SelectedItem == null)
            {
                _validvalues = false;
                message = "select sex";
            }
            if ((txtaddress.Text == "_") || (txtaddress.Text.Trim() == ""))
            {
                _validvalues = false;
                message = "Address is empty";
            }

            if ((txtusername.Text == "_") || (txtusername.Text.Trim() == ""))
            {
                _validvalues = false;
                message = "UserName empty";
            }
            if (!MyStaffMang.IsValidStaffId(txtusername.Text.Trim()))
            {
                _validvalues = false;
                message = "UserName Already exist";
            }
            if ((txtsurname.Text == "_") || (txtsurname.Text.Trim() == ""))
            {
                _validvalues = false;
                message = "SurName empty";
            }
            if ((txtemail.Text == "_") || (txtemail.Text.Trim() == ""))
            {
                _validvalues = false;
                message = "Email is empty";
            }
            //if ((txtpwd.Text == "_") || (txtpwd.Text.Trim() == ""))
            //{
            //    _validvalues = false;
            //    message = "Password empty";
            //}
            if ((txtexp.Text == "_") || (txtexp.Text.Trim() == ""))
            {
                _validvalues = false;
                message = "Experience is empty";
            }
            if (txtexp.Text.ToString().Trim() != "")
            {
                float _ExperienceYears = 0;
                if (float.TryParse(txtexp.Text.ToString().Trim(), out _ExperienceYears))
                {

                }
                else
                {
                    _validvalues = false;
                    message = "Experience is invalid";
                }
            }

            if ((txtdisgn.Text.Trim() == "_") || (txtdisgn.Text.Trim() == ""))
            {
                _validvalues = false;
                message = "Designation is empty";
            }
            if ((txtphone.Text == "_") || (txtphone.Text.Trim() == ""))
            {
                _validvalues = false;
                message = "Phone Number is empty";
            }
            if ((txteduqualification.Text == "_") || (txteduqualification.Text.Trim() == ""))
            {
                _validvalues = false;
                message = "Education Qualification is empty";
            }
            if (int.Parse(drplistrolw.SelectedValue.ToString()) == 0)
            {
                _validvalues = false;
                message = "Please select Role";
            }
            if (_validvalues && !CheckDate(txt_Dob.Text.ToString(), txtjndate.Text.ToString(), out message))
            {
                _validvalues = false;
            }
            if (_validvalues && !mailformate_IsValid(txtemail.Text.Trim()))
            {
                _validvalues = false;
                message = "Email id not in current formate";
            }
            return _validvalues;

        }
        private void movetocorrectGrid()
        {
            DataSet newcorrectdatas = new DataSet();
            DataTable dtnewcorrectdatas;
            if ((DataSet)Session["CorrectValues"] != null)
            {
                newcorrectdatas = (DataSet)Session["CorrectValues"];
                dtnewcorrectdatas = newcorrectdatas.Tables[0];

            }
            else
            {

                newcorrectdatas.Tables.Add(new DataTable("newcorrectexceldatas"));
                dtnewcorrectdatas = newcorrectdatas.Tables["newcorrectexceldatas"];
                dtnewcorrectdatas.Columns.Add("UserName");
                dtnewcorrectdatas.Columns.Add("Password");
                dtnewcorrectdatas.Columns.Add("EmailId");
                dtnewcorrectdatas.Columns.Add("SurName");
                dtnewcorrectdatas.Columns.Add("LastLogin");
                dtnewcorrectdatas.Columns.Add("CreationTime");
                dtnewcorrectdatas.Columns.Add("Role");
                dtnewcorrectdatas.Columns.Add("JoiningDate");
                dtnewcorrectdatas.Columns.Add("Address");
                dtnewcorrectdatas.Columns.Add("Sex");
                dtnewcorrectdatas.Columns.Add("Experience");
                dtnewcorrectdatas.Columns.Add("PhoneNumber");
                dtnewcorrectdatas.Columns.Add("Designation");
                dtnewcorrectdatas.Columns.Add("Dob");
                dtnewcorrectdatas.Columns.Add("EduQualifications");
                dtnewcorrectdatas.Columns.Add("IsLogin(yes/no)");

            }


            DataRow drcorrect;
            drcorrect = dtnewcorrectdatas.NewRow();
            drcorrect["UserName"] = txtusername.Text.Trim().ToString();
            drcorrect["Password"] = Session["_password"].ToString();
            drcorrect["EmailId"] = txtemail.Text.Trim().ToString();
            drcorrect["SurName"] = txtsurname.Text.Trim().ToString();
            drcorrect["Role"] = drplistrolw.SelectedItem.Text;
            drcorrect["JoiningDate"] = txtjndate.Text;
            drcorrect["Address"] = txtaddress.Text;
            drcorrect["Sex"] = RdBtnLstSex.SelectedItem.Text;
            drcorrect["Experience"] = txtexp.Text.Trim().ToString();
            drcorrect["PhoneNumber"] = txtphone.Text.Trim().ToString();
            drcorrect["Designation"] = txtdisgn.Text;
            drcorrect["Dob"] = txt_Dob.Text;
            drcorrect["EduQualifications"] = txteduqualification.Text;
            drcorrect["IsLogin(yes/no)"] = Rdbislogin.SelectedItem.Text;



            newcorrectdatas.Tables[0].Rows.Add(drcorrect);


            if (newcorrectdatas.Tables[0].Rows.Count > 0)
            {
                Session["CorrectValues"] = null;
                Session["CorrectValues"] = newcorrectdatas;
                this.CorrectExceldtls.Visible = true;
                lblcorrect.Text = "";
                Grd_CorrectDetails.DataSource = newcorrectdatas;//finally bind the Data into the grid
                Grd_CorrectDetails.DataBind();

            }
            else
            {
                lblcorrect.Text = "No Correct Entries To Display";
                this.CorrectExceldtls.Visible = false;
            }

        }
        private void deleteselectedRow(int selectedrowno)
        {

            DataSet dtunsorteddataset = (DataSet)Session["UnCorrectValues"];
            foreach (DataRow dr in dtunsorteddataset.Tables[0].Rows)
            {

                if (int.Parse(dr["SlNo"].ToString()) == selectedrowno)
                {
                    dtunsorteddataset.Tables[0].Rows.Remove(dr);
                    break;
                }
            }

            Session["UnCorrectValues"] = null;

            if (dtunsorteddataset.Tables[0].Rows.Count > 0)
            {
                this.UnCorrectExcelDtls.Visible = true;
                Grd_UnCorrectDtls.Columns[0].Visible = true;
                Session["UnCorrectValues"] = dtunsorteddataset;
                lbluncorrect.Text = "";
                Grd_UnCorrectDtls.DataSource = dtunsorteddataset;//finally bind the Data into the grid
                Grd_UnCorrectDtls.DataBind();
                Grd_UnCorrectDtls.Columns[0].Visible = false;
            }
            else
            {
                this.UnCorrectExcelDtls.Visible = false;

                Grd_UnCorrectDtls.DataSource = null;
                Grd_UnCorrectDtls.DataBind();
                lbluncorrect.Text = "All invalid values are corrected";
            }
        }
        private void Upload_detailstoDataBase()
        {
            lblerror.Text = "";
            //some initlization
            /*
             * tbluser :  :UserName,Password,EmailId,SurName,LastLogin,CreationTime,RoleId,CanLogin,Status
             *
             *tblstaffdetails::    UserId,JoiningDate,Address,Sex,Experience,ExpDescription,Designation,PhoneNumber,EduQualifications,Dob
             */
            DataSet ValidDetails = (DataSet)Session["CorrectValues"];
            int invalidslno = 0;
            int errFlag = -1;
            string _msg = "";
            int _login = 0;
            string _groupid = "";
            DataSet myDataset;
            myDataset = MyUser.MyAssociatedGroups();
            if (myDataset != null && myDataset.Tables != null && myDataset.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in myDataset.Tables[0].Rows)
                {

                    
                    _groupid = dr["Id"].ToString();

                }

            }


            //string _groupid = objSchool.SchoolId.ToString();
            #region declare variables to pass to create Staff
            //tbluser
            string _UserName;
            string _Password;
            string _EmailId;
            string _SurName;
            string _Role = "";
            int _role_id = 0;
            DateTime _CreationTime = System.DateTime.Now;
            //tblstaffdetails
            string _jngdate;
            string _Address = "";
            string _Sex = "";
            float _Experience = 0;
            string _Designation = "";
            string _PhoneNumber = "";
            string _EduQualifications = "";
            string _Dob;
            string _login_status = "";
            string _aadharno = "";
            string _panno = "";
            #endregion

            #region declare one dataset to store details of any uninserted rows details
            DataSet uncorrectexceldata = new DataSet();
            DataTable dtuncorrect;
            uncorrectexceldata.Tables.Add(new DataTable("uncorrectexceldatas"));
            dtuncorrect = uncorrectexceldata.Tables["uncorrectexceldatas"];
            dtuncorrect.Columns.Add("SlNo");
            dtuncorrect.Columns.Add("UserName");
            dtuncorrect.Columns.Add("Password");
            dtuncorrect.Columns.Add("EmailId");
            dtuncorrect.Columns.Add("SurName");
            dtuncorrect.Columns.Add("Role");
            dtuncorrect.Columns.Add("JoiningDate");
            dtuncorrect.Columns.Add("Address");
            dtuncorrect.Columns.Add("Sex");
            dtuncorrect.Columns.Add("Experience");
            dtuncorrect.Columns.Add("PhoneNumber");
            dtuncorrect.Columns.Add("Designation");
            dtuncorrect.Columns.Add("Dob");
            dtuncorrect.Columns.Add("EduQualifications");
            dtuncorrect.Columns.Add("AadharNo");
            dtuncorrect.Columns.Add("PanNo");
            dtuncorrect.Columns.Add("IsLogin(yes/no)");

            #endregion







            if (ValidDetails == null || ValidDetails.Tables[0] == null || ValidDetails.Tables[0].Rows.Count <= 0)
            {
                lblerror.Text = "Cannot read the Staff records .Please try again";
            }
            else if (ValidDetails != null && ValidDetails.Tables[0] != null && ValidDetails.Tables[0].Rows.Count > 0)
            {

                #region insert rows one by one
                int temperr = 0;

                foreach (DataRow dr in ValidDetails.Tables[0].Rows)
                // foreach (GridViewRow gvr in Grd_CorrectDtls.Rows)
                {
                    int createdstatus = -1;//means row is not inserted.If thats inserted then this value changed

                    _UserName = dr["UserName"].ToString().Trim();
                    _Sex = dr["Sex"].ToString().Trim().ToUpper();
                    if (_Sex == "MALE")
                    {
                        _Sex = "Male";
                    }
                    else
                    {
                        _Sex = "Female";
                    }
                    _login_status = dr["IsLogin(yes/no)"].ToString().Trim().ToUpper();
                    if (_login_status == "YES")
                    {
                        _login =1;
                    }
                    else
                    {
                        _login =0;
                    }
                    _Password = dr["Password"].ToString().Trim();
                    if (_Password == "" || _Password == "_")
                    {
                        _Password = "password";
                    }
                    _EmailId = dr["EmailId"].ToString().Trim();
                    _SurName = dr["SurName"].ToString().Trim();
                    _Role = dr["Role"].ToString().Trim();
                    _jngdate = dr["JoiningDate"].ToString().Trim();
                    _Address = dr["Address"].ToString().Trim();
                    _Experience = float.Parse(dr["Experience"].ToString().Trim());
                    _PhoneNumber = dr["PhoneNumber"].ToString().Trim();
                    _Designation = dr["Designation"].ToString().Trim();
                    _Dob = dr["Dob"].ToString().Trim().ToString();
                    _EduQualifications = dr["EduQualifications"].ToString().Trim();
                    _aadharno = dr["AadharNo"].ToString().Trim();
                    _panno = dr["PanNo"].ToString().Trim();
                    _role_id = get_roleid(_Role);
                    try
                    {

                        MyStaffMang.CreateTansationDb();
                        createdstatus = MyStaffMang.CreateStaff(_SurName, _Password, _jngdate, _Address, _Sex, _Experience, _Designation, _PhoneNumber, _EmailId, _UserName, _EduQualifications, _login, _role_id, _Dob, _groupid, _aadharno, _panno, out _msg);
                        if (createdstatus != -1)
                        {
                            if (_EmailId != "")
                            {
                                MyStaffMang.InsertStaffEmailIdIntoEmailStaffList(createdstatus, _EmailId.ToString());
                            }
                            MyStaffMang.EndSucessTansationDb();
                            MyIncedent.CreateApprovedIncedent("Staff Created", "Staff " + _SurName + " is added  on " + General.GerFormatedDatVal(DateTime.Now), General.GerFormatedDatVal(DateTime.Now), 4, MyUser.UserId, "staff", createdstatus, 34, 0, MyUser.CurrentBatchId, 0);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create Staff", " Create Staff " + _SurName, 1);
                        }
                    }
                    catch (Exception e1)
                    {
                        MyStaffMang.EndFailTansationDb();
                        lblerror.Text = "Error:" + e1.Message;
                        errFlag = 1;
                    }


                    if (createdstatus == -1)//means this row is not inserted so build the new dataset for such type of datas
                    {
                        MyStaffMang.EndFailTansationDb();
                        temperr = 1;
                        invalidslno = invalidslno + 1;
                        string _exp = _Experience.ToString();
                        //SlNo(0)  StudentName(1)  Sex(2)  DOB(3)  Father/GuardianName(4)  Religion(5)  Caste(6)  AddressPermanent(7) StudentType(8)  AdmissionNo(9) AdmissionDate(10) JoiningBatch(11)   
                        buildnoninsertedvaluesdatasetrow(uncorrectexceldata, dtuncorrect, invalidslno, _SurName, _UserName, _Password, _EmailId, _Role, _jngdate, _Address, _Sex, _exp, _PhoneNumber, _Designation, _Dob, _EduQualifications, _login_status);
                    }
                }
                if (temperr == 0)
                {
                    Session["UnCorrectValues"] = null;
                    Session["CorrectValues"] = null;
                    Session["_password"] = null;
                }
                else
                {
                    Session["CorrectValues"] = null;
                }

                #endregion
                if (errFlag == 1)
                {
                    if (invalidslno == 0)
                    {
                        lblerror.Text = "Some Error Occurs.May be uploaded excel contain errors";
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Import Staff From Excel", "Some staff details are imported from an excel file and saved to the data base", 1);
                        btn_upload.Enabled = false;
                        this.CorrectExceldtls.Visible = false;
                        this.UnCorrectExcelDtls.Visible = false;
                        Grd_UnCorrectDtls.DataSource = null;
                        Grd_UnCorrectDtls.DataBind();
                    }
                    else
                    {
                        this.CorrectExceldtls.Visible = false;


                        lblerror.Text = "Some rows are not inserted.Correct the values and try";
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Import Staff from Excel", "Some staff details are imported from an excel file and saved to the data base", 1);
                        if (uncorrectexceldata.Tables[0].Rows.Count > 0)
                        {
                            this.UnCorrectExcelDtls.Visible = true;
                            lbluncorrect.Text = "";
                            Grd_UnCorrectDtls.Columns[0].Visible = true;
                            Grd_UnCorrectDtls.DataSource = uncorrectexceldata;//finally bind the Data into the grid
                            Grd_UnCorrectDtls.DataBind();
                            Grd_UnCorrectDtls.Columns[0].Visible = false;
                        }
                        else
                        {
                            this.UnCorrectExcelDtls.Visible = false;
                            Grd_UnCorrectDtls.DataSource = null;
                            Grd_UnCorrectDtls.DataBind();
                        }
                    }
                }
                else if (lblerror.Text == "" && invalidslno == 0)
                {
                    lblerror.Text = "All staff Records are successfully saved.";
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Import Staff From Excel", "All staff recods updated", 1);
                    btn_upload.Enabled = false;
                    this.CorrectExceldtls.Visible = false;
                    this.UnCorrectExcelDtls.Visible = false;

                    Grd_UnCorrectDtls.DataSource = null;
                    Grd_UnCorrectDtls.DataBind();
                }

            }

            else
            {
                lblerror.Text = "Unable to Insert the details.Session may expired, Select class and try again";
            }
        }
        private void buildnoninsertedvaluesdatasetrow(DataSet uncorrectexceldata, DataTable dtuncorrect, int invalidslno, string _surname, string _username, string _Password, string _EmailId, string _Role, string _JoiningDate, string _Address, string _Sex, string _Experience, string _PhoneNumber, string _Designation, string _Dob, string _EduQualifications, string _Login_Status)
        {
            // SlNo(0)  SurName(1)  UserName(2)  Password(3)  EmailId(4)  Role(5)  JoiningDate(6)  Address(7) Sex(8)  Experience(9) PhoneNumber(10) Designation(11)  Dob(9) EduQualifications(10)    

            DataRow druncorrect = dtuncorrect.NewRow();
            druncorrect["SlNo"] = invalidslno;
            druncorrect["UserName"] = _username;
            druncorrect["Password"] = _Password;
            druncorrect["EmailId"] = _EmailId;
            druncorrect["SurName"] = _surname;
            druncorrect["Role"] = _Role;
            druncorrect["JoiningDate"] = _JoiningDate;
            druncorrect["Address"] = _Address;
            druncorrect["Sex"] = _Sex;
            druncorrect["Experience"] = _Experience;
            druncorrect["PhoneNumber"] = _PhoneNumber;
            druncorrect["Designation"] = _Designation;
            druncorrect["Dob"] = _Dob;
            druncorrect["EduQualifications"] = _EduQualifications;
            druncorrect["IsLogin(yes/no)"] = _Login_Status;
            uncorrectexceldata.Tables["uncorrectexceldatas"].Rows.Add(druncorrect);
            Session["UnCorrectValues"] = uncorrectexceldata;
        }
        private int get_roleid(string role)
        {
            int _Roleid = 0;
            string sql = "SELECT Id FROM tblrole WHERE RoleName='" + role + "' ";
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {

                _Roleid = int.Parse(MyReader.GetValue(0).ToString());

            }
            return _Roleid;
        }
        protected void btn_upload_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grd_UnCorrectDtls.Rows.Count != 0)
                    lblerror.Text = "Some values are not corrected .Correct them and try again";
                else
                    Upload_detailstoDataBase();
            }
            catch (Exception e2)
            {
                lblerror.Text = e2.Message;
            }
        }
        protected void Btn_UploadDetails_Click(object sender, EventArgs e)
        {
            Clear();

            try
            {
                Session["UnCorrectValues"] = null;
                Session["CorrectValues"] = null;
                string message, _FileName;

                DataSet ds_manual = create_manualdataset();

                if (Check_validity_ToUpload(out message))
                {
                    if (saveTheExcelFile(out _FileName))
                    {
                        string _physicalpath = Server.MapPath("TemporaryFileManager") + "\\" + _FileName;
                        MydataSet = prepareDataset_FromExcel(_physicalpath);      //prepare dataset from the excel
                        MydataSet = DataFormatedDataSet(MydataSet);
                        builddataset(MydataSet, ds_manual); //create new data set based on our requirements
                        File.Delete(Server.MapPath("TemporaryFileManager") + "\\" + _FileName);
                    }
                    else
                    {

                        lblerror.Text = "Not able to Upload the Excel File. Try again later";
                    }

                }
                else
                {
                    lblerror.Text = message;
                }
            }
            catch (Exception e1)
            {
                lblerror.Text = e1.Message;
            }
        }
    }

}
