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

using System.Data.OleDb;
using System.IO;
using WinBase;
using AjaxControlToolkit;
using System.Globalization;


namespace WinEr
{

    public partial class UploadDetails : System.Web.UI.Page
    {
        private ClassOrganiser MyClassMang;
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        private DataSet UplodeddataSet;
        private WinBase.Incident Myincident;
        private int adnomodevalue = 0;

        private TextBox[] dynamicTextBoxes;
        private int[] Mandatoryflag;
        private string[] FealdName;
        private int CustfieldCount;

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            //ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            //scriptManager.RegisterPostBackControl(this.Template);
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
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



        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            if (Session["ClassId"] == null)
            {
                Response.Redirect("LoadClassDetails.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyClassMang = MyUser.GetClassObj();
            MyStudMang = MyUser.GetStudentObj();
            Myincident = MyUser.GetIncedentObj();
            if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(90))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                AddCoustomControls();
                if (!IsPostBack)
                {
                    string _MenuStr;
                    _MenuStr = MyClassMang.GetClassMangSubMenuString(MyUser.UserRoleId, MyUser.SELECTEDMODE);
                    this.SubClassMenu.InnerHtml = _MenuStr;

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

            Grd_CorrectDtls.DataSource = null;
            Grd_CorrectDtls.DataBind();

            Grd_UnCorrectDtls.DataSource = null;
            Grd_UnCorrectDtls.DataBind();

            btn_upload.Enabled = false;
            this.CorrectExceldtls.Visible = false;
            this.UnCorrectExcelDtls.Visible = false;
        }

        private int getadmissionnomode()
        {
            int value = 0;
            string sql = "SELECT tblconfiguration.Value FROM tblconfiguration WHERE tblconfiguration.Id=1 AND tblconfiguration.Name='AdmisionNo'";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                value = int.Parse(MyReader.GetValue(0).ToString());
            }
            return value;
        }

        private DataSet DataFormatedDataSet(DataSet MydataSet, DataSet excelField_CusotmerMap, DataSet dynamicFieldDetails)
        {
            char[] replacechr = { '\'', '\\', '/' };
            DataSet FormatedDate = new DataSet();
            DataTable dtcorrect;
            DataRow dr;

            FormatedDate.Tables.Add(new DataTable("correctexceldatas"));
            dtcorrect = FormatedDate.Tables["correctexceldatas"];

            foreach (DataRow Dr in excelField_CusotmerMap.Tables[0].Rows)
            {

                dtcorrect.Columns.Add(Dr["SoftwareField"].ToString());

            }
            foreach (DataRow Dr in dynamicFieldDetails.Tables[0].Rows)
            {
                dtcorrect.Columns.Add(Dr["FieldName"].ToString());
            }

            //dtcorrect.Columns.Add("Description");
            //dtcorrect.Columns.Add("Field1");
            //dtcorrect.Columns.Add("2ndlanguage");
            //dtcorrect.Columns.Add("Subject");
            //dtcorrect.Columns.Add("StudentName");
            //dtcorrect.Columns.Add("Sex");
            //dtcorrect.Columns.Add("DOB-D");
            //dtcorrect.Columns.Add("DOB-M");
            //dtcorrect.Columns.Add("DOB-Y");
            //dtcorrect.Columns.Add("Father/GuardianName");
            //dtcorrect.Columns.Add("Religion");
            //dtcorrect.Columns.Add("Caste");
            //dtcorrect.Columns.Add("AddressPermanent");
            //dtcorrect.Columns.Add("StudentType");
            //dtcorrect.Columns.Add("AdmissionNo");
            //dtcorrect.Columns.Add("AdmissionDate-D");
            //dtcorrect.Columns.Add("AdmissionDate-M");
            //dtcorrect.Columns.Add("AdmissionDate-Y");
            //dtcorrect.Columns.Add("JoiningBatch");

            //dtcorrect.Columns.Add("NewAdmission");
            //dtcorrect.Columns.Add("UseBus");
            //dtcorrect.Columns.Add("UseHostel");



            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Dr_Students in MydataSet.Tables[0].Rows)
                {
                    dr = dtcorrect.NewRow();

                    foreach (DataRow Dr_Tblfield in excelField_CusotmerMap.Tables[0].Rows)
                    {
                        dr[Dr_Tblfield["SoftwareField"].ToString()] = Dr_Students[Dr_Tblfield["ExcelFiled"].ToString()].ToString().Replace("'", "").Replace("\\", "");

                    }
                    //FormatedDate.Tables[0].Rows.Add(dr);
                    foreach (DataRow Dr_Tblfield in dynamicFieldDetails.Tables[0].Rows)
                    {
                        dr[Dr_Tblfield["FieldName"].ToString()] = Dr_Students[Dr_Tblfield["FieldName"].ToString()].ToString().Replace("'", "").Replace("\\", "");
                    }

                    FormatedDate.Tables[0].Rows.Add(dr);

                    //dr = dtcorrect.NewRow();
                    //dr["Id"] = Dr_Students["SlNO"].ToString();
                    //dr["StudentName"] = Dr_Students["Student Name*"].ToString().Replace("'", "").Replace("\\", "").Replace("/","");
                    //dr["Sex"] = Dr_Students["SEX"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    //dr["DOB-D"] = Dr_Students["Date Of Birth(dd)"].ToString();
                    //dr["DOB-M"] = Dr_Students["Date Of Birth(mm)"].ToString();
                    //dr["DOB-Y"] = Dr_Students["Date Of Birth(yyyy)"].ToString();
                    //dr["Father/GuardianName"] = Dr_Students["Father/Guardian Name"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    //dr["Religion"] = Dr_Students["RELIGION"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    //dr["Caste"] = Dr_Students["CASTE"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    //dr["AddressPermanent"] = Dr_Students["Address"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    //dr["StudentType"] = Dr_Students["Student Type"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    //dr["AdmissionNo"] = Dr_Students["Admission No"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    //dr["AdmissionDate-D"] = Dr_Students["Date Of Admission(dd)"].ToString();
                    //dr["AdmissionDate-M"] = Dr_Students["Date Of Admission(mm)"].ToString();
                    //dr["AdmissionDate-Y"] = Dr_Students["Date Of Admission(yyyy)"].ToString();
                    //dr["JoiningBatch"] = Dr_Students["Joining Batch"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    //dr["NewAdmission"] = Dr_Students["New Admission"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    //dr["UseBus"] = Dr_Students["Using Bus"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");
                    //dr["UseHostel"] = Dr_Students["Using Hostel"].ToString().Replace("'", "").Replace("\\", "").Replace("/", "");                                      
                    //FormatedDate.Tables[0].Rows.Add(dr);
                }
            }
            Session["ExcelValues"] = FormatedDate;
            return FormatedDate;
        }

        private bool saveTheExcelFile(out string _FileName)
        {
            bool _valid = true;
            _FileName = null;
            try
            {
                _FileName = FileUpload_Excel.FileName.ToString();

                FileUpload_Excel.SaveAs(MyUser.FilePath + "\\UpImage\\" + _FileName);
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

        private void builddataset(DataSet MyDataSet, DataSet excelField_CusotmerMap, DataSet dynamicFieldDetails)
        {
            //check any null values in every rows  and make a valid and invalid set of rows(2 arrays ) first 
            //try
            //{
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
                        if (AllValuesInRowRCorrect(i, MyDataSet.Tables[0].Rows[i], dynamicFieldDetails, out errormsg)) //check any column empty if so return false
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
                    build_ValidDataset(invalidrowId, _invalidcounts, _validcounts, validrowId, MyDataSet, excelField_CusotmerMap, invalidRows_description, dynamicFieldDetails);
                }
                else   //all entries r un correct and bind these to grid by preparing dataset(buildInvalidDataset())
                {
                    lblerror.Text = "Selected excel file contains some invalid entries";
                    this.CorrectExceldtls.Visible = true;
                    //build_ValidDataset(invalidrowId, _invalidcounts, _validcounts, validrowId, MyDataSet, excelField_CusotmerMap, invalidRows_description, dynamicFieldDetails);
                }
                if (_invalidcounts > 0)
                {
                    this.UnCorrectExcelDtls.Visible = true;
                    lbluncorrect.Text = "";
                    buildInvalidDataset(_invalidcounts, invalidrowId, MyDataSet, excelField_CusotmerMap, invalidRows_description, dynamicFieldDetails);
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

        private void build_ValidDataset(int[] invalidrowId, int _invalidcounts, int _validcounts, int[] validrowId, DataSet MyDataSet, DataSet excelField_CusotmerMap, string[,] invalidRows_description, DataSet dynamicFieldDetails)
        {
            string _religion, _cast, _joinBatch, _studType, errormsg;
            int _validrowId;
            int _relid, _castid, _studtypid, _joinbatchid;
            DataSet correctexceldata = new DataSet();
            DataTable dtcorrect;

            correctexceldata.Tables.Add(new DataTable("correctexceldatas"));

            dtcorrect = correctexceldata.Tables["correctexceldatas"];


            dtcorrect.Columns.Add("SlNO");
            dtcorrect.Columns.Add("StudentName");
            dtcorrect.Columns.Add("Sex");
            dtcorrect.Columns.Add("DOB");
            dtcorrect.Columns.Add("Father/GuardianName");
            dtcorrect.Columns.Add("Religion");
            dtcorrect.Columns.Add("Caste");
            dtcorrect.Columns.Add("AddressPermanent");
            dtcorrect.Columns.Add("StudentType");
            dtcorrect.Columns.Add("AdmissionNo");
            dtcorrect.Columns.Add("AdmissionDate");
            dtcorrect.Columns.Add("JoiningBatch");

            dtcorrect.Columns.Add("ReligionId");
            dtcorrect.Columns.Add("CasteId");
            dtcorrect.Columns.Add("StudentTypeId");
            dtcorrect.Columns.Add("JoiningBatchId");

            dtcorrect.Columns.Add("NewAdmission");
            dtcorrect.Columns.Add("UseBus");
            dtcorrect.Columns.Add("UseHostel");


            foreach (DataRow Dr in excelField_CusotmerMap.Tables[0].Rows)
            {
                if (!dtcorrect.Columns.Contains(Dr["SoftwareField"].ToString()) && (int.Parse(Dr["IsDynamic"].ToString()) == 1 || int.Parse(Dr["IsMandatory"].ToString()) == 0))
                {
                    dtcorrect.Columns.Add(Dr["SoftwareField"].ToString());
                }
            }
            foreach (DataRow Dr in dynamicFieldDetails.Tables[0].Rows)
            {
                if (!dtcorrect.Columns.Contains(Dr["FieldName"].ToString()) && int.Parse(Dr["IsMandatory"].ToString()) == 0)
                {
                    dtcorrect.Columns.Add(Dr["FieldName"].ToString());
                }
            }

            //StudentName(0)  Sex(1)  DOB(2)  Father/GuardianName(3)  Religion(4)  Caste(5)  AddressPermanent(6) StudentType(7)     AdmissionNo(8) AdmissionDate(9) JoiningBatch(10)    ReligionId(11)  CasteId(12)  StudentTypeId(13)  JoiningBatchId(14)


            for (int i = 0; i < _validcounts; i++)
            {
                _validrowId = validrowId[i];

                _religion = MyDataSet.Tables[0].Rows[_validrowId]["Religion"].ToString();
                _cast = MyDataSet.Tables[0].Rows[_validrowId]["Caste"].ToString();
                _studType = MyDataSet.Tables[0].Rows[_validrowId]["StudentType"].ToString();
                _joinBatch = MyDataSet.Tables[0].Rows[_validrowId]["JoiningBatch"].ToString();





                //check cast, rel,join batch etc are valid datas.if yes count it as valid and return their id also
                if (allareAlreadyPresent(_religion, _cast, _studType, _joinBatch, out _relid, out  _castid, out  _studtypid, out _joinbatchid, out errormsg))
                {
                    DataRow drcorrect = dtcorrect.NewRow();
                    drcorrect["SlNO"] = MydataSet.Tables[0].Rows[_validrowId]["SlNO"].ToString();
                    drcorrect["StudentName"] = MyDataSet.Tables[0].Rows[_validrowId]["StudentName"].ToString();
                    drcorrect["Sex"] = MyDataSet.Tables[0].Rows[_validrowId]["Sex"].ToString();

                    //MyUser.TryGetDareFromText(MyDataSet.Tables[0].Rows[_validrowId][3].ToString(), out _FormatedDate);
                    //drcorrect["DOB"] = MyUser.GerFormatedDatVal(_FormatedDate);

                    drcorrect["DOB"] = MyUser.GerFormatedDatVal(new DateTime(int.Parse(MyDataSet.Tables[0].Rows[_validrowId]["DOB-Y"].ToString()), int.Parse(MyDataSet.Tables[0].Rows[_validrowId]["DOB-M"].ToString()), int.Parse(MyDataSet.Tables[0].Rows[_validrowId]["DOB-D"].ToString())));
                    //drcorrect["DOB"] = DateTime.Parse(MyDataSet.Tables[0].Rows[_validrowId][3].ToString()).Date.ToString("dd/MM/yyyy"); 
                    drcorrect["Father/GuardianName"] = MyDataSet.Tables[0].Rows[_validrowId]["Father/GuardianName"].ToString();
                    drcorrect["Religion"] = _religion;
                    drcorrect["Caste"] = _cast;
                    drcorrect["AddressPermanent"] = MyDataSet.Tables[0].Rows[_validrowId]["AddressPermanent"].ToString();
                    drcorrect["StudentType"] = _studType;

                    adnomodevalue = getadmissionnomode();// 1 means  its autonumber generate
                    if (adnomodevalue == 1) //means autogenerate adno so no need to enter adno
                    {
                        //drcorrect["AdmissionNo"] = MyDataSet.Tables[0].Rows[_validrowId][9].ToString();
                        drcorrect["AdmissionNo"] = "_"; //whatever be the adno just enter a _ for adno 
                    }
                    else
                    {
                        if (MyDataSet.Tables[0].Rows[_validrowId]["AdmissionNo"].ToString() != "")
                            drcorrect["AdmissionNo"] = MyDataSet.Tables[0].Rows[_validrowId]["AdmissionNo"].ToString();
                        else
                            drcorrect["AdmissionNo"] = "_";
                    }
                    //MyUser.TryGetDareFromText(MyDataSet.Tables[0].Rows[_validrowId][10].ToString(), out _FormatedDate);
                    //drcorrect["AdmissionDate"] = MyUser.GerFormatedDatVal(_FormatedDate);
                    drcorrect["AdmissionDate"] = MyUser.GerFormatedDatVal(new DateTime(int.Parse(MyDataSet.Tables[0].Rows[_validrowId]["AdmissionDate-Y"].ToString()), int.Parse(MyDataSet.Tables[0].Rows[_validrowId]["AdmissionDate-M"].ToString()), int.Parse(MyDataSet.Tables[0].Rows[_validrowId]["AdmissionDate-D"].ToString())));
                    //drcorrect["AdmissionDate"] = DateTime.Parse(MyDataSet.Tables[0].Rows[_validrowId][10].ToString()).Date.ToString("dd/MM/yyyy");
                    drcorrect["JoiningBatch"] = _joinBatch;
                    drcorrect["ReligionId"] = _relid.ToString();
                    drcorrect["CasteId"] = _castid.ToString();
                    drcorrect["StudentTypeId"] = _studtypid.ToString();
                    drcorrect["JoiningBatchId"] = _joinbatchid.ToString();
                    drcorrect["NewAdmission"] = MyDataSet.Tables[0].Rows[_validrowId]["NewAdmission"].ToString();
                    drcorrect["UseBus"] = MyDataSet.Tables[0].Rows[_validrowId]["UseBus"].ToString();
                    drcorrect["UseHostel"] = MyDataSet.Tables[0].Rows[_validrowId]["UseHostel"].ToString();
                    //drcorrect["StudentId"] = MyDataSet.Tables[0].Rows[_validrowId]["StudentId"].ToString();



                    foreach (DataRow Dr_Tblfield in excelField_CusotmerMap.Tables[0].Rows)
                    {
                        if (int.Parse(Dr_Tblfield["IsDynamic"].ToString()) == 1 || int.Parse(Dr_Tblfield["IsMandatory"].ToString()) == 0)
                        {

                            drcorrect[Dr_Tblfield["SoftwareField"].ToString()] = MyDataSet.Tables[0].Rows[_validrowId][Dr_Tblfield["SoftwareField"].ToString()];
                        }
                    }
                    foreach (DataRow Dr_Tblfield in dynamicFieldDetails.Tables[0].Rows)
                    {
                        if (int.Parse(Dr_Tblfield["IsMandatory"].ToString()) == 0)
                        {
                            drcorrect[Dr_Tblfield["FieldName"].ToString()] = MyDataSet.Tables[0].Rows[_validrowId][Dr_Tblfield["FieldName"].ToString()];
                        }
                    }





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

                Grd_CorrectDtls.Columns[11].Visible = true;
                Grd_CorrectDtls.Columns[12].Visible = true;
                Grd_CorrectDtls.Columns[13].Visible = true;
                Grd_CorrectDtls.Columns[14].Visible = true;

                Grd_CorrectDetails.Columns[11].Visible = true;
                Grd_CorrectDetails.Columns[12].Visible = true;
                Grd_CorrectDetails.Columns[13].Visible = true;
                Grd_CorrectDetails.Columns[14].Visible = true;

                Grd_CorrectDtls.DataSource = correctexceldata;//finally bind the Data into the grid
                Grd_CorrectDtls.DataBind();
                Grd_CorrectDetails.DataSource = correctexceldata;
                Grd_CorrectDetails.DataBind();

                Grd_CorrectDtls.Columns[11].Visible = false;
                Grd_CorrectDtls.Columns[12].Visible = false;
                Grd_CorrectDtls.Columns[13].Visible = false;
                Grd_CorrectDtls.Columns[14].Visible = false;


                Grd_CorrectDetails.Columns[11].Visible = false;
                Grd_CorrectDetails.Columns[12].Visible = false;
                Grd_CorrectDetails.Columns[13].Visible = false;
                Grd_CorrectDetails.Columns[14].Visible = false;
            }
            else
            {
                this.CorrectExceldtls.Visible = false;
                Grd_CorrectDtls.DataSource = null;
                Grd_CorrectDtls.DataBind();

                lblerror.Text = "Excel file contains invalid entries.";
            }

            if (_invalidcounts > 0)
            {
                lbluncorrect.Text = "";

                buildInvalidDataset(_invalidcounts, invalidrowId, MyDataSet, excelField_CusotmerMap, invalidRows_description, dynamicFieldDetails);
            }
            else
            {
                lblerror.Text = "Selected excel file is valid. ";
            }
        }

        private bool AllValuesInRowRCorrect(int i, DataRow dataRow, DataSet dynamicFieldDetails, out string errormsg)
        {
            //DateTime tryparsedatetime;
            bool _validrow = true;
            errormsg = "";

            if (dataRow["StudentName"].ToString().Trim() == "")//Student Name
            {
                _validrow = false;
                errormsg = "Empty Name";
                return _validrow;
            }
            if (_validrow && dataRow["Sex"].ToString().Trim() != "")//Sex
            {
                string _sex = dataRow["Sex"].ToString().Trim().ToLower();
                if (!((dataRow["Sex"].ToString().Trim().ToLower() == "male") || (dataRow["Sex"].ToString().Trim().ToLower() == "female")))
                {
                    _validrow = false;
                    errormsg = "Invalid Sex (Male/Female)";
                    return _validrow;
                }
            }
            else
            {
                _validrow = false;
                errormsg = "Empty Sex";
                return _validrow;
            }
            if (_validrow && dataRow["DOB-D"].ToString().Trim() == "")//Date Of Birth
            {
                _validrow = false;
                errormsg = "Empty DOB - Day";
                return _validrow;
            }

            if (_validrow && dataRow["DOB-M"].ToString().Trim() == "")//Date Of Birth
            {
                _validrow = false;
                errormsg = "Empty DOB- Month";
                return _validrow;
            }

            if (_validrow && dataRow["DOB-Y"].ToString().Trim() == "")//Date Of Birth
            {
                _validrow = false;
                errormsg = "Empty DOB - Year";
                return _validrow;
            }

            if (_validrow)
            {
                try
                {
                    DateTime _new = new DateTime(int.Parse(dataRow["DOB-Y"].ToString().Trim()), int.Parse(dataRow["DOB-M"].ToString().Trim()), int.Parse(dataRow["DOB-D"].ToString().Trim()));
                }
                catch
                {
                    _validrow = false;
                    errormsg = "DOB entry is wrong";
                    return _validrow;
                }
            }

            if (_validrow && dataRow["Father/GuardianName"].ToString().Trim() == "")//Father/Guardian Name
            {
                _validrow = false;
                errormsg = "Empty Parent Name";
                return _validrow;
            }


            if (_validrow && dataRow["Religion"].ToString().Trim() == "")//Religion
            {
                _validrow = false;
                errormsg = "Empty Religion";
                return _validrow;
            }

            if (_validrow && dataRow["Caste"].ToString().Trim() == "")//Caste
            {
                _validrow = false;
                errormsg = "Empty Caste";
                return _validrow;
            }

            if (_validrow && dataRow["AddressPermanent"].ToString().Trim() == "")//Address
            {
                _validrow = false;
                errormsg = "Empty Address";
                return _validrow;
            }

            if (_validrow && dataRow["StudentType"].ToString().Trim() == "")//Student Type
            {
                _validrow = false;
                errormsg = "Empty Student Type";
                return _validrow;
            }


            adnomodevalue = getadmissionnomode();// 1 means  its autonumber generate
            if (adnomodevalue != 1) //means not automatic adno generation  so check thats empty or not
            {
                if (_validrow && dataRow["AdmissionNo"].ToString().Trim() != "")
                {
                    // check admissionNo entered is already present if so  its an invalid entry
                    if (!validadno(dataRow["AdmissionNo"].ToString()))
                    {
                        errormsg = "Invalid Admission Number or already exists";
                        _validrow = false;
                        return _validrow;
                    }
                }
                else
                {
                    errormsg = "Empty Admission No";
                    _validrow = false;
                    return _validrow;
                }
            }


            if (_validrow && dataRow["AdmissionDate-D"].ToString().Trim() == "")//DateOf Admission
            {
                _validrow = false;
                errormsg = "Empty Day Of Admission Date";
                return _validrow;
            }

            if (_validrow && dataRow["AdmissionDate-M"].ToString().Trim() == "")//DateOf Admission
            {
                _validrow = false;
                errormsg = "Empty Month Of Admission Date";
                return _validrow;
            }

            if (_validrow && dataRow["AdmissionDate-Y"].ToString().Trim() == "")//DateOf Admission
            {
                _validrow = false;
                errormsg = "Empty Year Of Admission Date";
                return _validrow;
            }

            if (_validrow)
            {
                try
                {
                    DateTime _new = new DateTime(int.Parse(dataRow["AdmissionDate-Y"].ToString().Trim()), int.Parse(dataRow["AdmissionDate-M"].ToString().Trim()), int.Parse(dataRow["AdmissionDate-D"].ToString().Trim()));
                }
                catch
                {
                    _validrow = false;
                    errormsg = "Admission Date entry is wrong";
                    return _validrow;
                }
            }

            if (_validrow && dataRow["JoiningBatch"].ToString().Trim() == "")//JOining Batch
            {
                errormsg = "Empty Joining Batch";
                _validrow = false;
                return _validrow;
            }

            if (_validrow && dataRow["NewAdmission"].ToString().Trim().ToLower() != "")
            {
                if (!(dataRow["NewAdmission"].ToString().Trim().ToLower() == "yes" || dataRow["NewAdmission"].ToString().Trim().ToLower() == "no"))//New Admission or not
                {
                    errormsg = "Wrong Admission Type";
                    _validrow = false;
                    return _validrow;
                }
            }
            else
            {
                errormsg = "Empty Admission Type";
                _validrow = false;
                return _validrow;
            }

            if (_validrow && dataRow["UseBus"].ToString().Trim() != "")//using college bus
            {
                if (!(dataRow["UseBus"].ToString().Trim().ToLower() == "yes" || dataRow["UseBus"].ToString().Trim().ToLower() == "no"))//using college bus
                {
                    errormsg = "Wrong Bus entries";
                    _validrow = false;
                    return _validrow;
                }
            }
            else
            {
                errormsg = "Empty Bus Field";
                _validrow = false;
                return _validrow;
            }

            if (_validrow && dataRow["UseHostel"].ToString().Trim() != "")//Using Hostel
            {
                if (!(dataRow["UseHostel"].ToString().Trim().ToLower() == "yes" || dataRow["UseHostel"].ToString().Trim().ToLower() == "no"))//using hostel
                {
                    errormsg = "Wrong Hostel value";
                    _validrow = false;
                    return _validrow;
                }
            }
            else
            {
                errormsg = "Empty Hostel Field";
                _validrow = false;
                return _validrow;
            }

            string _religion = dataRow["Religion"].ToString();
            string _cast = dataRow["Caste"].ToString();
            string _studType = dataRow["StudentType"].ToString();
            string _joinBatch = dataRow["JoiningBatch"].ToString();
            string new_errormsg = "";
            int _relid = 0, _castid = 0, _studtypid = 0, _joinbatchid = 0;

            if (!allareAlreadyPresent(_religion, _cast, _studType, _joinBatch, out _relid, out _castid, out _studtypid, out _joinbatchid, out new_errormsg))
            {
                errormsg = new_errormsg;
                _validrow = false;
                return _validrow;
            }

            if (dynamicFieldDetails != null && dynamicFieldDetails.Tables != null && dynamicFieldDetails.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dynamicFieldDetails.Tables[0].Rows)
                {
                    if (_validrow && dr["Ismandatory"].ToString() == "1")
                    {
                        if (dataRow.Table.Columns.Contains(dr["DbColumanName"].ToString()))
                        {
                            if (dataRow[dr["DbColumanName"].ToString()].ToString().Trim() == "")
                            {
                                errormsg = "Empty " + dr["FieldName"].ToString();
                                _validrow = false;
                                return _validrow;
                            }
                        }
                        else
                        {
                            errormsg = "Cannot Find the details " + dr["FieldName"].ToString();
                            _validrow = false;
                            return _validrow;
                        }
                    }


                }
            }


            return _validrow;
        }

        private bool allareAlreadyPresent(string _religion, string _cast, string _studType, string _joinBatch, out int _relid, out int _castid, out int _studtypid, out int _joinbatchid, out string errormsg)
        {
            bool found = false;
            bool _allvalid = false;
            _relid = 0; _castid = 0; _studtypid = 0; _joinbatchid = 0;
            errormsg = "";
            string sql;
            found = false;
            sql = "select distinct tblreligion.Id,  lower(tblreligion.Religion) from tblreligion";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    if (MyReader.GetValue(1).ToString().ToLower().Trim() == _religion.ToLower().Trim())
                    {
                        _relid = int.Parse(MyReader.GetValue(0).ToString());
                        found = true;
                        break;
                    }
                }

            }

            if (found)
            {
                _allvalid = true;
            }
            else
            {
                _allvalid = false;
                errormsg = "Invalid  Religion";
                return _allvalid;
            }


            if (_allvalid)
            {
                found = false;
                if (_cast.ToLower() == "unknown")
                {
                    _castid = 0;
                    found = true;
                }
                else
                {

                    sql = "Select tblcast.id ,tblcast.castname  from tblcast ";
                    MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {

                        while (MyReader.Read())
                        {
                            if ((MyReader.GetValue(1).ToString().ToLower().Trim() == _cast.ToLower().Trim()))
                            {
                                int.TryParse(MyReader.GetValue(0).ToString().Trim(), out _castid);
                                found = true;
                                break;
                            }
                        }
                    }
                }
                if (found)
                {
                    _allvalid = true;
                }
                else
                {
                    _allvalid = false;
                    errormsg = "Invalid  Caste";
                    return _allvalid;
                }
            }

            if (_allvalid)
            {
                found = false;
                sql = "select distinct tblbatch.Id, lower(tblbatch.BatchName) from tblbatch";
                MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    _allvalid = true;

                    while (MyReader.Read())
                    {
                        if (MyReader.GetValue(1).ToString().Trim() == _joinBatch.ToLower().Trim())
                        {
                            _joinbatchid = int.Parse(MyReader.GetValue(0).ToString());
                            found = true;
                            break;
                        }
                    }
                }


                if (found)
                {
                    _allvalid = true;
                }
                else
                {
                    _allvalid = false;
                    errormsg = "Invalid Joining Batch";
                    return _allvalid;
                }
            }
            if (_allvalid)
            {
                found = false;
                sql = "select distinct tblstudtype.Id, lower(tblstudtype.TypeName) from tblstudtype";
                MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        if (MyReader.GetValue(1).ToString().Trim() == _studType.ToLower().Trim())
                        {
                            _studtypid = int.Parse(MyReader.GetValue(0).ToString());
                            found = true;
                            break;
                        }
                    }
                }


                if (found)
                {
                    _allvalid = true;
                }
                else
                {
                    _allvalid = false;
                    errormsg = "Invalid Student Type";
                    return _allvalid;
                }
            }

            return _allvalid;
        }

        private void buildInvalidDataset(int _invalidcounts, int[] invalidrowId, DataSet MyDataSet, DataSet excelField_CusotmerMap, string[,] invalidRows_description, DataSet dynamicFieldDetails)
        {
            int _invalidrowid = 0;
            string _religion, _cast, _joinBatch, _studType;
            int _relid, _castid, _studtypid, _joinbatchid;
            string errormsg = "";
            int _slno = 0;
            DataSet uncorrectexceldata = new DataSet();
            DataTable dtuncorrect;
            uncorrectexceldata.Tables.Add(new DataTable("uncorrectexceldatas"));
            dtuncorrect = uncorrectexceldata.Tables["uncorrectexceldatas"];


            dtuncorrect.Columns.Add("SlNo");
            dtuncorrect.Columns.Add("StudentName");
            dtuncorrect.Columns.Add("Sex");
            dtuncorrect.Columns.Add("DOB");
            dtuncorrect.Columns.Add("Father/GuardianName");
            dtuncorrect.Columns.Add("Religion");
            dtuncorrect.Columns.Add("Caste");
            dtuncorrect.Columns.Add("AddressPermanent");
            dtuncorrect.Columns.Add("StudentType");
            dtuncorrect.Columns.Add("AdmissionNo");
            dtuncorrect.Columns.Add("AdmissionDate");
            dtuncorrect.Columns.Add("JoiningBatch");

            dtuncorrect.Columns.Add("ReligionId");
            dtuncorrect.Columns.Add("CasteId");
            dtuncorrect.Columns.Add("StudentTypeId");
            dtuncorrect.Columns.Add("JoiningBatchId");

            dtuncorrect.Columns.Add("NewAdmission");
            dtuncorrect.Columns.Add("UseBus");
            dtuncorrect.Columns.Add("UseHostel");

            dtuncorrect.Columns.Add("Description");



            foreach (DataRow Dr in excelField_CusotmerMap.Tables[0].Rows)
            {
                if (!dtuncorrect.Columns.Contains(Dr["SoftwareField"].ToString()) && (int.Parse(Dr["IsDynamic"].ToString()) == 1 || int.Parse(Dr["IsMandatory"].ToString()) == 0))
                {
                    dtuncorrect.Columns.Add(Dr["SoftwareField"].ToString());
                }
            }
            foreach (DataRow Dr in dynamicFieldDetails.Tables[0].Rows)
            {
                if (!dtuncorrect.Columns.Contains(Dr["FieldName"].ToString()) && int.Parse(Dr["IsMandatory"].ToString()) == 0)
                {
                    dtuncorrect.Columns.Add(Dr["FieldName"].ToString());
                }
            }


            // SlNo(0)  StudentName(1)  Sex(2)  DOB(3)  Father/GuardianName(4)  Religion(5)  Caste(6)  AddressPermanent(7) StudentType(8)  AdmissionNo(9) AdmissionDate(10) JoiningBatch(11) Description(12)  
            for (int i = 0; i < _invalidcounts; i++)
            {
                _invalidrowid = invalidrowId[i];
                _religion = MyDataSet.Tables[0].Rows[_invalidrowid]["Religion"].ToString();
                _cast = MyDataSet.Tables[0].Rows[_invalidrowid]["Caste"].ToString();
                _studType = MyDataSet.Tables[0].Rows[_invalidrowid]["StudentType"].ToString();
                _joinBatch = MyDataSet.Tables[0].Rows[_invalidrowid]["JoiningBatch"].ToString();
                DataRow druncorrect = dtuncorrect.NewRow();
                if (allareAlreadyPresent(_religion, _cast, _studType, _joinBatch, out _relid, out  _castid, out  _studtypid, out _joinbatchid, out errormsg))
                {
                    //DataRow druncorrect = dtuncorrect.NewRow();

                    _slno += 1;


                    druncorrect["SlNo"] = _slno;

                    if (MyDataSet.Tables[0].Rows[_invalidrowid][1].ToString().Trim() != "")
                        druncorrect["StudentName"] = MyDataSet.Tables[0].Rows[_invalidrowid]["StudentName"].ToString();
                    else
                        druncorrect["StudentName"] = "_";

                    if (MyDataSet.Tables[0].Rows[_invalidrowid]["Sex"].ToString().Trim() != "")
                        druncorrect["Sex"] = MyDataSet.Tables[0].Rows[_invalidrowid]["Sex"].ToString();
                    else
                        druncorrect["Sex"] = "_";

                    if (MyDataSet.Tables[0].Rows[_invalidrowid]["DOB-Y"].ToString() != "" && MyDataSet.Tables[0].Rows[_invalidrowid]["DOB-M"].ToString() != "" && MyDataSet.Tables[0].Rows[_invalidrowid]["DOB-D"].ToString() != "")
                    {

                        try
                        {
                            druncorrect["DOB"] = MyUser.GerFormatedDatVal(new DateTime(int.Parse(MyDataSet.Tables[0].Rows[_invalidrowid]["DOB-Y"].ToString()), int.Parse(MyDataSet.Tables[0].Rows[_invalidrowid]["DOB-M"].ToString()), int.Parse(MyDataSet.Tables[0].Rows[_invalidrowid]["DOB-D"].ToString())));
                        }
                        catch
                        {
                            druncorrect["DOB"] = "_";
                        }
                    }

                    else
                        druncorrect["DOB"] = "_";

                    if (MyDataSet.Tables[0].Rows[_invalidrowid]["Father/GuardianName"].ToString().Trim() != "")
                        druncorrect["Father/GuardianName"] = MyDataSet.Tables[0].Rows[_invalidrowid]["Father/GuardianName"].ToString();
                    else
                        druncorrect["Father/GuardianName"] = "_";

                    if (MyDataSet.Tables[0].Rows[_invalidrowid]["Religion"].ToString().Trim() != "")
                        druncorrect["Religion"] = MyDataSet.Tables[0].Rows[_invalidrowid]["Religion"].ToString();
                    else
                        druncorrect["Religion"] = "_";

                    if (MyDataSet.Tables[0].Rows[_invalidrowid]["Caste"].ToString().Trim() != "")
                        druncorrect["Caste"] = MyDataSet.Tables[0].Rows[_invalidrowid]["Caste"].ToString();
                    else
                        druncorrect["Caste"] = "_";

                    if (MyDataSet.Tables[0].Rows[_invalidrowid]["AddressPermanent"].ToString().Trim() != "")
                        druncorrect["AddressPermanent"] = MyDataSet.Tables[0].Rows[_invalidrowid]["AddressPermanent"].ToString();
                    else
                        druncorrect["AddressPermanent"] = "_";

                    if (MyDataSet.Tables[0].Rows[_invalidrowid]["StudentType"].ToString().Trim() != "")
                        druncorrect["StudentType"] = MyDataSet.Tables[0].Rows[_invalidrowid]["StudentType"].ToString();
                    else
                        druncorrect["StudentType"] = "_";


                    if (MyDataSet.Tables[0].Rows[_invalidrowid]["AdmissionNo"].ToString().Trim() != "")
                        druncorrect["AdmissionNo"] = MyDataSet.Tables[0].Rows[_invalidrowid]["AdmissionNo"].ToString();
                    else
                        druncorrect["AdmissionNo"] = "_";

                    if (MyDataSet.Tables[0].Rows[_invalidrowid]["AdmissionDate-Y"].ToString() != "" && MyDataSet.Tables[0].Rows[_invalidrowid]["AdmissionDate-M"].ToString() != "" && MyDataSet.Tables[0].Rows[_invalidrowid]["AdmissionDate-D"].ToString() != "")
                    {


                        try
                        {
                            druncorrect["AdmissionDate"] = MyUser.GerFormatedDatVal(new DateTime(int.Parse(MyDataSet.Tables[0].Rows[_invalidrowid]["AdmissionDate-Y"].ToString()), int.Parse(MyDataSet.Tables[0].Rows[_invalidrowid]["AdmissionDate-M"].ToString()), int.Parse(MyDataSet.Tables[0].Rows[_invalidrowid]["AdmissionDate-D"].ToString())));
                        }
                        catch
                        {
                            druncorrect["AdmissionDate"] = "_";
                        }
                    }
                    else
                        druncorrect["AdmissionDate"] = "_";

                    if (MyDataSet.Tables[0].Rows[_invalidrowid]["JoiningBatch"].ToString().Trim() != "")
                        druncorrect["JoiningBatch"] = MyDataSet.Tables[0].Rows[_invalidrowid]["JoiningBatch"].ToString();
                    else
                        druncorrect["JoiningBatch"] = "_";

                    druncorrect["ReligionId"] = _relid.ToString();
                    druncorrect["CasteId"] = _castid.ToString();
                    druncorrect["StudentTypeId"] = _studtypid.ToString();
                    druncorrect["JoiningBatchId"] = _joinbatchid.ToString();

                    druncorrect["NewAdmission"] = MyDataSet.Tables[0].Rows[_invalidrowid]["NewAdmission"].ToString();
                    druncorrect["UseBus"] = MyDataSet.Tables[0].Rows[_invalidrowid]["UseBus"].ToString();
                    druncorrect["UseHostel"] = MyDataSet.Tables[0].Rows[_invalidrowid]["UseHostel"].ToString();


                    //druncorrect["StudentId"] = MyDataSet.Tables[0].Rows[_invalidrowid]["StudentId"].ToString();

                    druncorrect["Description"] = invalidRows_description[i, 1];




                    foreach (DataRow Dr_Tblfield in excelField_CusotmerMap.Tables[0].Rows)
                    {
                        if (int.Parse(Dr_Tblfield["IsDynamic"].ToString()) == 1 || int.Parse(Dr_Tblfield["IsMandatory"].ToString()) == 0)
                        {

                            druncorrect[Dr_Tblfield["SoftwareField"].ToString()] = MyDataSet.Tables[0].Rows[_invalidrowid][Dr_Tblfield["SoftwareField"].ToString()];
                        }

                    }
                    foreach (DataRow Dr_Tblfield in dynamicFieldDetails.Tables[0].Rows)
                    {
                        if (int.Parse(Dr_Tblfield["IsMandatory"].ToString()) == 0)
                        {
                            druncorrect[Dr_Tblfield["FieldName"].ToString()] = MyDataSet.Tables[0].Rows[_invalidrowid][Dr_Tblfield["FieldName"].ToString()];
                        }

                    }


                    //}

                    uncorrectexceldata.Tables["uncorrectexceldatas"].Rows.Add(druncorrect);
                }

            }
            if (uncorrectexceldata.Tables[0].Rows.Count > 0)
            {

                Grd_UnCorrectDtls.Columns[0].Visible = true;

                Grd_UnCorrectDtls.DataSource = uncorrectexceldata;//finally bind the Data into the grid
                Grd_UnCorrectDtls.DataBind();
                Grd_UnCorrectDtls.Columns[0].Visible = false;
                Grd_CorrectDetails.Visible = true;

                Session["UnCorrectValues"] = uncorrectexceldata;


            }
            else
            {

                Grd_UnCorrectDtls.DataSource = null;
                //Grd_UnCorrectDtls.DataSource = MyDataSet;

                Grd_UnCorrectDtls.DataBind();
                //Grd_UnCorrectDtls.Columns[0].Visible = true;
                lblerror.Text = errormsg;
            }
        }

        protected void Grd_UnCorrectDtls_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grd_UnCorrectDtls.Columns[0].Visible = true;
            lblselectedrowslno.Text = Grd_UnCorrectDtls.SelectedRow.Cells[0].Text.ToString();
            //lblselectedrowslno.Text = Grd_UnCorrectDtls.SelectedRow.Cells[1].Text.ToString();
            lblediterror.Text = "";
            MPE_PopUpMessageBox.Show();
            fillAllControlsinpopupwindow();
            Grd_UnCorrectDtls.Columns[0].Visible = false;

        }

        private void fillAllControlsinpopupwindow()
        {
            string _rel = null, _cast = null, _studtyp = null, _joinbatch = null;
            int _relid, _castid, _studtypid, _joinbatchid, _slno;

            // SlNo(0)  StudentName(1)  Sex(2)  DOB(3)  Father/GuardianName(4)  Religion(5)  Caste(6)  AddressPermanent(7) StudentType(8)  AdmissionNo(9) AdmissionDate(10) JoiningBatch(11)   Description(12)
            DataSet dt_uncorrectdetail = (DataSet)Session["UnCorrectValues"];

            if (dt_uncorrectdetail != null && dt_uncorrectdetail.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dt_uncorrectdetail.Tables[0].Rows)
                {
                    if (int.Parse(dr["SlNo"].ToString()) == int.Parse(lblselectedrowslno.Text))
                    // if ((dr["StudentName"].ToString()) == lblselectedrowslno.Text)
                    {
                        lblerrordescription.Text = dr["Description"].ToString().Trim();
                        txtslno.Text = dr["SlNO"].ToString().Trim();
                        txtstudname.Text = dr["StudentName"].ToString().Trim();

                        if (dr["Sex"].ToString().Trim().ToLower() == "female")
                            RdBtnLstSex.SelectedIndex = 1;
                        else
                            RdBtnLstSex.SelectedIndex = 0;

                        if (dr["DOB"] != null && !string.IsNullOrEmpty(dr["DOB"].ToString().Trim()))
                        {

                            txt_Dob.Text = dr["DOB"].ToString();
                            if (txt_Dob.Text == "_")
                            {
                                txt_Dob.Text = "_";
                            }
                            else
                            {
                                DateTime dtDob = General.GetDateTimeFromText(dr["DOB"].ToString());
                            }
                        }
                        else
                            txt_Dob.Text = "_";

                        //,culture, DateTimeStyles.None
                        //if ((dr["DOB"].ToString().Trim() != "") && (DateTime.TryParse(dr["DOB"].ToString().Trim() , out tryparsedate)))
                        //    txt_Dob.Text = General.GerFormatedDatVal(tryparsedate);
                        //else
                        //    txt_Dob.Text = "_";


                        txtfathername.Text = dr["Father/GuardianName"].ToString().Trim();

                        _rel = dr["Religion"].ToString().Trim();
                        txtreligion.Text = _rel;

                        _cast = dr["Caste"].ToString().Trim();
                        txtcast.Text = _cast;

                        txtaddress.Text = dr["AddressPermanent"].ToString().Trim();

                        _studtyp = dr["StudentType"].ToString().Trim();
                        txtstudtype.Text = _studtyp;

                        adnomodevalue = getadmissionnomode();// 1 means  its autonumber generate
                        if (adnomodevalue == 1) //means autogenerate adno so no need to enter adno
                        {
                            // txtadmno.Text = Grd_UnCorrectDtls.SelectedRow.Cells[9].Text.ToString();
                            txtenteredadno.Text = dr["AdmissionNo"].ToString().Trim();

                            txtadmno.Text = "_";  //whatever be the values in excel display a _ since mode is autogenerate
                            txtadmno.Enabled = false;
                        }
                        else
                        {
                            txtenteredadno.Text = dr["AdmissionNo"].ToString().Trim();

                            txtadmno.Text = dr["AdmissionNo"].ToString().Trim();
                            txtadmno.Enabled = true;
                        }

                        if (dr["AdmissionDate"] != null && !string.IsNullOrEmpty(dr["AdmissionDate"].ToString().Trim()))
                        {
                            txtadmdate.Text = dr["AdmissionDate"].ToString();
                            //DateTime dtDob = General.GetDateTimeFromText(dr["DOB"].ToString());
                        }
                        else
                            txtadmdate.Text = "_";

                        //if ((dr["AdmissionDate"].ToString().Trim() != "") && (DateTime.TryParse(dr["AdmissionDate"].ToString().Trim(), culture, DateTimeStyles.None, out tryparsedate)))
                        //    txtadmdate.Text = General.GerFormatedDatVal(tryparsedate);
                        //else
                        //    txtadmdate.Text = "_";

                        _joinbatch = dr["JoiningBatch"].ToString().Trim();
                        txtjoinbatch.Text = _joinbatch;


                        _relid = getRelegionId(_rel);
                        _castid = getCastId(_cast);
                        _studtypid = getStudentTypeId(_studtyp);
                        _joinbatchid = getjoinbatchId(_joinbatch);
                        loadalldropdownlistsinpopup(_relid, _castid, _studtypid, _joinbatchid);

                        LoaddynamicValues(dr);
                    }
                }
            }

        }

        private void LoaddynamicValues(DataRow dr)
        {

            DataSet dynamicFieldDetails = MyStudMang.GetCuestomFields();
            DataSet excelField_CusotmerMap = MyClassMang.getStudentExcelFiledMap();

            int count = 0;

            if (dynamicFieldDetails != null && dynamicFieldDetails.Tables != null && dynamicFieldDetails.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Dr in dynamicFieldDetails.Tables[0].Rows)
                //foreach (DataRow Dr in excelField_CusotmerMap.Tables[0].Rows)
                {
                    //if (int.Parse(Dr["IsDynamic"].ToString()) == 1)
                    if (int.Parse(Dr["Ismandatory"].ToString()) == 0)
                    {

                        dynamicTextBoxes[count].Text = dr[Dr["FieldName"].ToString()].ToString().Trim();

                        count++;
                    }

                }


            }

        }

        private void AddCoustomControls()
        {

            CustfieldCount = MyStudMang.CoustomFieldCount;

            if (CustfieldCount == 0)
            {
                //Label lbicusnote = new Label();
                //lbicusnote.ID = "lbicusnote";
                //lbicusnote.Text = "No Coustom Fields Present.";
                //myPlaceHolder.Controls.Add(lbicusnote);
                //Wzd_StudCreation.WizardSteps.Remove(Wzd_StudCreation.WizardSteps[2]);
            }
            else
            {

                dynamicTextBoxes = new TextBox[CustfieldCount];
                Mandatoryflag = new int[CustfieldCount];
                FealdName = new string[CustfieldCount];
                DataSet _CustomFields = MyStudMang.GetCuestomFields();

                if (_CustomFields != null && _CustomFields.Tables != null && _CustomFields.Tables[0].Rows.Count > 0)
                {

                    int i = 0;
                    Table tbl = new Table();
                    TableRow tr = new TableRow();
                    tbl.Width = 800;
                    myPlaceHolder.Controls.Add(tbl);
                    int customControl = 1;

                    TableCell tc1 = new TableCell();
                    tc1.HorizontalAlign = HorizontalAlign.Right;
                    TableCell tc2 = new TableCell();
                    TableCell tc3 = new TableCell();
                    tc3.HorizontalAlign = HorizontalAlign.Right;
                    TableCell tc4 = new TableCell();
                    TableCell tc5 = new TableCell();
                    tc5.HorizontalAlign = HorizontalAlign.Right;
                    TableCell tc6 = new TableCell();

                    foreach (DataRow dr_fieldData in _CustomFields.Tables[0].Rows)
                    {
                        if (customControl == 1 && i != 0)
                        {
                            tr = new TableRow();
                            tr.Height = 40;
                            tc1 = new TableCell();

                            tc1.HorizontalAlign = HorizontalAlign.Right;
                            tc2 = new TableCell();


                            tc3 = new TableCell();

                            tc3.HorizontalAlign = HorizontalAlign.Right;
                            tc4 = new TableCell();

                            tc5 = new TableCell();
                            tc5.HorizontalAlign = HorizontalAlign.Right;

                            tc6 = new TableCell();

                        }



                        TextBox textBox = new TextBox();
                        textBox.MaxLength = int.Parse(dr_fieldData[3].ToString());
                        textBox.ID = "dtxt_" + dr_fieldData[1].ToString();

                        if (customControl == 1)
                            tc2.Controls.Add(textBox);
                        else if (customControl == 2)
                            tc4.Controls.Add(textBox);
                        else if (customControl == 3)
                            tc6.Controls.Add(textBox);

                        Mandatoryflag[i] = int.Parse(dr_fieldData[4].ToString());
                        FealdName[i] = dr_fieldData[0].ToString();
                        dynamicTextBoxes[i] = textBox;

                        FilteredTextBoxExtender FiltTxtbxExt = new FilteredTextBoxExtender();
                        FiltTxtbxExt.ID = "FiltTxtbxExt" + dr_fieldData[1].ToString();
                        FiltTxtbxExt.Enabled = true;

                        if (dr_fieldData[2].ToString() == "1")
                        {

                            FiltTxtbxExt.FilterType = FilterTypes.Numbers;

                        }
                        else if (dr_fieldData[2].ToString() == "2")
                        {

                            FiltTxtbxExt.FilterType = FilterTypes.Custom;
                            FiltTxtbxExt.FilterMode = FilterModes.InvalidChars;
                            FiltTxtbxExt.InvalidChars = "'/\\";
                        }
                        else
                        {
                            FiltTxtbxExt.ValidChars = ".";
                            FiltTxtbxExt.FilterType = FilterTypes.Numbers | FilterTypes.Custom;

                        }
                        FiltTxtbxExt.TargetControlID = "dtxt_" + dr_fieldData[1].ToString();

                        //  myPlaceHolder.Controls.Add(FiltTxtbxExt);

                        tr.Cells.Add(tc1);
                        tr.Cells.Add(tc2);
                        tr.Cells.Add(tc3);
                        tr.Cells.Add(tc4);
                        tr.Cells.Add(tc5);
                        tr.Cells.Add(tc6);

                        if (dr_fieldData[4].ToString() == "1")
                        {
                            // tc1.Text = dr_fieldData[1].ToString() + "<span class=\"redcol\">*</span>";

                            if (customControl == 1)
                                tc1.Text = dr_fieldData[1].ToString() + "<span class=\"redcol\">*</span>";
                            else if (customControl == 2)
                                tc3.Text = dr_fieldData[1].ToString() + "<span class=\"redcol\">*</span>";
                            else if (customControl == 3)
                                tc5.Text = dr_fieldData[1].ToString() + "<span class=\"redcol\">*</span>";


                            RequiredFieldValidator ReqfldvalExt = new RequiredFieldValidator();
                            ReqfldvalExt.ID = "ReqfldvalExt" + dr_fieldData[1].ToString();
                            ReqfldvalExt.ControlToValidate = "dtxt_" + dr_fieldData[1].ToString();
                            ReqfldvalExt.ErrorMessage = "*";
                            ReqfldvalExt.ValidationGroup = "Edit";

                            if (customControl == 1)
                                tc2.Controls.Add(ReqfldvalExt);
                            else if (customControl == 2)
                                tc4.Controls.Add(ReqfldvalExt);
                            else if (customControl == 3)
                                tc6.Controls.Add(ReqfldvalExt);

                        }
                        else
                        {

                            if (customControl == 1)
                                tc1.Text = dr_fieldData[1].ToString();
                            else if (customControl == 2)
                                tc3.Text = dr_fieldData[1].ToString();
                            else if (customControl == 3)
                                tc5.Text = dr_fieldData[1].ToString();

                        }

                        if (customControl == 3 || i == _CustomFields.Tables[0].Rows.Count - 1)
                        {
                            tbl.Rows.Add(tr);
                        }
                        i++;
                        if (customControl == 3)
                        {
                            customControl = 1;
                        }
                        else
                            customControl++;
                    }
                }
            }
        }


        private int getjoinbatchId(string _joinbatch)
        {
            int _joinbatchid = 0;
            string sql = "SELECT tblbatch.Id, tblbatch.BatchName FROM tblbatch WHERE Created=1";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    if (MyReader.GetValue(1).ToString().ToLower() == _joinbatch.ToString().ToLower())
                    {
                        _joinbatchid = int.Parse(MyReader.GetValue(0).ToString());
                        break;
                    }
                }
            }

            return _joinbatchid;
        }

        private int getStudentTypeId(string _studtyp)
        {

            int _studtypid = 0;
            string sql = "SELECT tblstudtype.Id,tblstudtype.TypeName FROM tblstudtype";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    if (MyReader.GetValue(1).ToString().ToLower() == _studtyp.ToString().ToLower())
                    {
                        _studtypid = int.Parse(MyReader.GetValue(0).ToString());
                        break;
                    }
                }
            }
            return _studtypid;
        }

        private int getCastId(string _cast)
        {
            int _castid = 0;
            string sql = "select tblcast.Id, tblcast.castname from tblcast";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    if (MyReader.GetValue(1).ToString().ToLower() == _cast.ToString().ToLower())
                    {
                        _castid = int.Parse(MyReader.GetValue(0).ToString());
                        break;
                    }
                }
            }
            return _castid;
        }

        private int getRelegionId(string _rel)
        {
            int _relid = 0;
            string sql = "SELECT tblreligion.Id,tblreligion.Religion FROM tblreligion where tblreligion.Religion <>'Other' ";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    if (MyReader.GetValue(1).ToString().ToLower() == _rel.ToString().ToLower())
                    {
                        _relid = int.Parse(MyReader.GetValue(0).ToString());
                        break;
                    }
                }
            }

            return _relid;
        }

        private void loadalldropdownlistsinpopup(int _relid, int _castid, int _studtypid, int _joinbatchid)
        {
            string sql = null;

            #region LoadJoiningBatches
            Drpjoinbatch.Items.Clear();
            sql = "SELECT Id,BatchName FROM tblbatch WHERE Created=1";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drpjoinbatch.Items.Add(li);

                }
                if (_joinbatchid != 0)

                    Drpjoinbatch.SelectedValue = _joinbatchid.ToString();
            }
            #endregion

            #region LoadStudenttype
            Drpstudtype.Items.Clear();
            sql = "SELECT Id,TypeName FROM tblstudtype";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drpstudtype.Items.Add(li);

                }
                if (_studtypid != 0)
                    Drpstudtype.SelectedValue = _studtypid.ToString();
            }
            #endregion

            #region LoadReligion
            DrpRelegion.Items.Clear();
            sql = "SELECT Id,Religion FROM tblreligion where Religion <>'Other' ";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {

                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    DrpRelegion.Items.Add(li);
                }
                DrpRelegion.SelectedValue = _relid.ToString();

            }
            #endregion

            #region LoadCast

            Drpcast.Items.Clear();
            sql = "select tblcast.Id, tblcast.castname from tblcast  where tblcast.castname <>'Other' order by tblcast.castname ";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drpcast.Items.Add(li);

                }
                Drpcast.SelectedValue = _castid.ToString();

            }

            #endregion
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

        private void deleteselectedRow(int selectedrowno)
        {
            DataSet excelField_CusotmerMap = MyClassMang.getStudentExcelFiledMap();
            DataSet dynamicFieldDetails = MyStudMang.GetCuestomFields(); ;


            DataSet dtunsorteddataset = (DataSet)Session["UnCorrectValues"];
            foreach (DataRow dr in dtunsorteddataset.Tables[0].Rows)
            {

                if (int.Parse(dr["SlNo"].ToString()) == selectedrowno)
                {
                    dtunsorteddataset.Tables[0].Rows.Remove(dr);
                    break;
                }
            }







            //int _slno = 0;
            //DataSet uncorrectexceldata = new DataSet();
            //DataTable dtuncorrect;
            //uncorrectexceldata.Tables.Add(new DataTable("uncorrectexceldatas"));
            //dtuncorrect = uncorrectexceldata.Tables["uncorrectexceldatas"];
            //dtuncorrect.Columns.Add("SlNo");
            //dtuncorrect.Columns.Add("StudentName");
            //dtuncorrect.Columns.Add("Sex");
            //dtuncorrect.Columns.Add("DOB");
            //dtuncorrect.Columns.Add("Father/GuardianName");
            //dtuncorrect.Columns.Add("Religion");
            //dtuncorrect.Columns.Add("Caste");
            //dtuncorrect.Columns.Add("AddressPermanent");
            //dtuncorrect.Columns.Add("StudentType");            
            //dtuncorrect.Columns.Add("AdmissionNo");
            //dtuncorrect.Columns.Add("AdmissionDate");
            //dtuncorrect.Columns.Add("JoiningBatch");
            //dtuncorrect.Columns.Add("Description");

            //dtuncorrect.Columns.Add("NewAdmission");
            //dtuncorrect.Columns.Add("UseBus");
            //dtuncorrect.Columns.Add("UseHostel");

            //if (dynamicFieldDetails != null && dynamicFieldDetails.Tables != null && dynamicFieldDetails.Tables[0].Rows.Count > 0)
            //{

            //    foreach (DataRow Dr in excelField_CusotmerMap.Tables[0].Rows)
            //    {
            //        if (int.Parse(Dr["IsDynamic"].ToString()) == 1)
            //        {
            //            dtuncorrect.Columns.Add(Dr["SoftwareField"].ToString());
            //        }
            //    }
            //}


            // SlNo(0)  StudentName(1)  Sex(2)  DOB(3)  Father/GuardianName(4)  Religion(5)  Caste(6)  AddressPermanent(7) StudentType(8)  AdmissionNo(9) AdmissionDate(10) JoiningBatch(11)   Description(12)
            //foreach (GridViewRow gvr in Grd_UnCorrectDtls.Rows)
            //{
            //    int rowno = int.Parse(gvr.Cells[1].Text);
            //    if (rowno != selectedrowno)
            //    {
            //        DataRow druncorrect = dtuncorrect.NewRow();
            //        _slno += 1;
            //        druncorrect["SlNo"] = _slno;

            //        if(gvr.Cells[2].Text.Trim() != "")
            //            druncorrect["StudentName"] = gvr.Cells[2].Text.Trim();
            //        else
            //            druncorrect["StudentName"] = "_";

            //        if (gvr.Cells[3].Text.Trim() != "")
            //            druncorrect["Sex"] = gvr.Cells[3].Text.Trim();
            //        else
            //            druncorrect["Sex"] = "_";

            //        if (gvr.Cells[4].Text.Trim() != "")
            //            druncorrect["DOB"] = gvr.Cells[4].Text.Trim();
            //        else
            //            druncorrect["DOB"] = "_";

            //        if (gvr.Cells[5].Text.Trim() != "")
            //            druncorrect["Father/GuardianName"] = gvr.Cells[5].Text.Trim();
            //        else
            //            druncorrect["Father/GuardianName"] = "_";

            //        if (gvr.Cells[6].Text.Trim() != "")
            //            druncorrect["Religion"] = gvr.Cells[6].Text.Trim();
            //        else
            //            druncorrect["Religion"] = "_";

            //        if (gvr.Cells[7].Text.Trim() != "")
            //            druncorrect["Caste"] = gvr.Cells[7].Text.Trim();
            //        else
            //            druncorrect["Caste"] = "_";

            //        if (gvr.Cells[8].Text.Trim() != "")
            //             druncorrect["AddressPermanent"] = gvr.Cells[8].Text.Trim();
            //        else
            //            druncorrect["AddressPermanent"] = "_";

            //        if (gvr.Cells[9].Text.Trim() != "")
            //            druncorrect["StudentType"] = gvr.Cells[9].Text.Trim();
            //        else
            //            druncorrect["StudentType"] = "_";


            //        if (gvr.Cells[10].Text.Trim() != "")
            //            druncorrect["AdmissionNo"] = gvr.Cells[10].Text.Trim();
            //        else
            //            druncorrect["AdmissionNo"] = "_";

            //        if (gvr.Cells[11].Text.Trim() != "")
            //            druncorrect["AdmissionDate"] = gvr.Cells[11].Text.Trim();
            //        else
            //            druncorrect["AdmissionDate"] = "_";

            //        if (gvr.Cells[12].Text.Trim() != "")
            //            druncorrect["JoiningBatch"] = gvr.Cells[12].Text.Trim();
            //        else
            //            druncorrect["JoiningBatch"] = "_";

            //        if (gvr.Cells[13].Text.Trim() != "")
            //             druncorrect["Description"] = gvr.Cells[13].Text.Trim();
            //        else
            //            druncorrect["Description"] = "_";

            //        if (gvr.Cells[14].Text.Trim() != "")
            //            druncorrect["NewAdmission"] = gvr.Cells[14].Text.Trim();
            //        else
            //            druncorrect["NewAdmission"] = "_";

            //        if (gvr.Cells[15].Text.Trim() != "")
            //            druncorrect["UseBus"] = gvr.Cells[15].Text.Trim();
            //        else
            //            druncorrect["UseBus"] = "_";

            //        if (gvr.Cells[16].Text.Trim() != "")
            //            druncorrect["UseHostel"] = gvr.Cells[16].Text.Trim();
            //        else
            //            druncorrect["UseHostel"] = "_";
            //        int count = 17;

            //        foreach (DataRow Dr in excelField_CusotmerMap.Tables[0].Rows)
            //        {
            //            if (int.Parse(Dr["IsDynamic"].ToString()) == 1)
            //            {
            //                druncorrect[Dr["SoftwareField"].ToString()]= gvr.Cells[count].Text.Trim();
            //                count++;
            //            }
            //        }

            //        uncorrectexceldata.Tables["uncorrectexceldatas"].Rows.Add(druncorrect);

            //    }               
            //}

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

        private void movetocorrectGrid()
        {
            DataSet excelField_CusotmerMap;
            DataSet dynamicFieldDetails = MyStudMang.GetCuestomFields();
            // if (dynamicFieldDetails.Tables[0].Rows.Count == 0)
            // {

            excelField_CusotmerMap = MyClassMang.getStudentExcelFiledMap();
            // }
            // else
            //  {
            //     excelField_CusotmerMap = MyClassMang.getStudentDetailExcelFiledMap();
            //}
            DataSet newcorrectdatas = new DataSet();
            DataTable dtnewcorrectdatas;
            string _religion, _cast, _joinBatch, _studType, errormsg;

            int _relid, _castid, _studtypid, _joinbatchid;

            if ((DataSet)Session["CorrectValues"] != null)
            {
                newcorrectdatas = (DataSet)Session["CorrectValues"];
                dtnewcorrectdatas = newcorrectdatas.Tables[0];

            }
            else
            {
                //newcorrectdatas = (DataSet)Session["UnCorrectValues"];
                newcorrectdatas.Tables.Add(new DataTable("newcorrectexceldatas"));
                dtnewcorrectdatas = newcorrectdatas.Tables["newcorrectexceldatas"];
                //dtnewcorrectdatas.Columns.Add("Id");
                dtnewcorrectdatas.Columns.Add("SlNO");
                dtnewcorrectdatas.Columns.Add("StudentName");
                dtnewcorrectdatas.Columns.Add("Sex");
                dtnewcorrectdatas.Columns.Add("DOB");
                dtnewcorrectdatas.Columns.Add("Father/GuardianName");
                dtnewcorrectdatas.Columns.Add("Religion");
                dtnewcorrectdatas.Columns.Add("Caste");
                dtnewcorrectdatas.Columns.Add("AddressPermanent");
                dtnewcorrectdatas.Columns.Add("StudentType");
                dtnewcorrectdatas.Columns.Add("AdmissionNo");
                dtnewcorrectdatas.Columns.Add("AdmissionDate");
                dtnewcorrectdatas.Columns.Add("JoiningBatch");

                dtnewcorrectdatas.Columns.Add("ReligionId");
                dtnewcorrectdatas.Columns.Add("CasteId");
                dtnewcorrectdatas.Columns.Add("StudentTypeId");
                dtnewcorrectdatas.Columns.Add("JoiningBatchId");

                dtnewcorrectdatas.Columns.Add("NewAdmission");
                dtnewcorrectdatas.Columns.Add("UseBus");
                dtnewcorrectdatas.Columns.Add("UseHostel");

                //dtnewcorrectdatas.Columns.Add("StudentId");

                foreach (DataRow Dr in excelField_CusotmerMap.Tables[0].Rows)
                {
                    if (!dtnewcorrectdatas.Columns.Contains(Dr["SoftwareField"].ToString()) && (int.Parse(Dr["IsDynamic"].ToString()) == 1 || int.Parse(Dr["IsMandatory"].ToString()) == 0))
                    {
                        dtnewcorrectdatas.Columns.Add(Dr["SoftwareField"].ToString());
                    }
                }
                foreach (DataRow Dr in dynamicFieldDetails.Tables[0].Rows)
                {
                    if (!dtnewcorrectdatas.Columns.Contains(Dr["FieldName"].ToString()) && int.Parse(Dr["IsMandatory"].ToString()) == 0)
                    {
                        dtnewcorrectdatas.Columns.Add(Dr["FieldName"].ToString());
                    }
                }


            }
            //newcorrectdatas.Tables.Add(new DataTable("newcorrectexceldatas"));
            //dtnewcorrectdatas = newcorrectdatas.Tables["newcorrectexceldatas"];
            //dtnewcorrectdatas.Columns.Add("StudentName");
            //dtnewcorrectdatas.Columns.Add("Sex");
            //dtnewcorrectdatas.Columns.Add("DOB");
            //dtnewcorrectdatas.Columns.Add("Father/GuardianName");
            //dtnewcorrectdatas.Columns.Add("Religion");
            //dtnewcorrectdatas.Columns.Add("Caste");
            //dtnewcorrectdatas.Columns.Add("AddressPermanent");
            //dtnewcorrectdatas.Columns.Add("StudentType");
            //dtnewcorrectdatas.Columns.Add("AdmissionNo");
            //dtnewcorrectdatas.Columns.Add("AdmissionDate");
            //dtnewcorrectdatas.Columns.Add("JoiningBatch");

            //dtnewcorrectdatas.Columns.Add("ReligionId");
            //dtnewcorrectdatas.Columns.Add("CasteId");
            //dtnewcorrectdatas.Columns.Add("StudentTypeId");
            //dtnewcorrectdatas.Columns.Add("JoiningBatchId");

            //dtnewcorrectdatas.Columns.Add("NewAdmission");
            //dtnewcorrectdatas.Columns.Add("UseBus");
            //dtnewcorrectdatas.Columns.Add("UseHostel");

            // if (excelField_CusotmerMap != null && excelField_CusotmerMap.Tables != null && excelField_CusotmerMap.Tables[0].Rows.Count > 0)
            // {

            //     foreach (DataRow Dr in excelField_CusotmerMap.Tables[0].Rows)
            //     {
            //         if (int.Parse(Dr["IsDynamic"].ToString()) == 1)
            //         {
            //             dtnewcorrectdatas.Columns.Add(Dr["SoftwareField"].ToString());
            //         }
            //     }
            // }

            ////StudentName(0)  Sex(1)  DOB(2)  Father/GuardianName(3)  Religion(4)  Caste(5)  AddressPermanent(6) StudentType(7)     AdmissionNo(8) AdmissionDate(9) JoiningBatch(10)    ReligionId(11)  CasteId(12)  StudentTypeId(13)  JoiningBatchId(14)

            // DataRow drcorrect;


            // foreach (GridViewRow gvr in Grd_CorrectDtls.Rows)
            // {


            //         drcorrect = dtnewcorrectdatas.NewRow();

            //         drcorrect["StudentName"] = gvr.Cells[0].Text.Trim();
            //         drcorrect["Sex"] = gvr.Cells[1].Text.Trim();

            //         drcorrect["DOB"] = gvr.Cells[2].Text.Trim();
            //         drcorrect["Father/GuardianName"] = gvr.Cells[3].Text.Trim();
            //         drcorrect["Religion"] = gvr.Cells[4].Text.Trim();
            //         drcorrect["Caste"] = gvr.Cells[5].Text.Trim();
            //         drcorrect["AddressPermanent"] = gvr.Cells[6].Text.Trim();
            //         drcorrect["StudentType"] = gvr.Cells[7].Text.Trim();                        
            //         drcorrect["AdmissionNo"] = gvr.Cells[8].Text.Trim();
            //         drcorrect["AdmissionDate"] = gvr.Cells[9].Text.Trim();
            //         drcorrect["JoiningBatch"] = gvr.Cells[10].Text.Trim();

            //         drcorrect["ReligionId"] = gvr.Cells[11].Text.Trim();
            //         drcorrect["CasteId"] = gvr.Cells[12].Text.Trim();                       
            //         drcorrect["StudentTypeId"] = gvr.Cells[13].Text.Trim();
            //         drcorrect["JoiningBatchId"] = gvr.Cells[14].Text.Trim();

            //         drcorrect["NewAdmission"] = gvr.Cells[15].Text.Trim();
            //         drcorrect["UseBus"] = gvr.Cells[16].Text.Trim();
            //         drcorrect["UseHostel"] = gvr.Cells[17].Text.Trim();

            //         int clmId = 18;

            //         if (excelField_CusotmerMap != null && excelField_CusotmerMap.Tables != null && excelField_CusotmerMap.Tables[0].Rows.Count > 0)
            //         {

            //             foreach (DataRow Dr in excelField_CusotmerMap.Tables[0].Rows)
            //             {
            //                 if (int.Parse(Dr["IsDynamic"].ToString()) == 1)
            //                 {
            //                     drcorrect[Dr["SoftwareField"].ToString()] = gvr.Cells[clmId].Text.Trim();
            //                  clmId++;
            //                 }

            //             }
            //         }


            //         newcorrectdatas.Tables["newcorrectexceldatas"].Rows.Add(drcorrect);


            // }

            //DataRow drcorrect;
            //drcorrect = dtnewcorrectdatas.NewRow();
            //foreach (DataRow Dr in excelField_CusotmerMap.Tables[0].Rows)
            //{
            //    if (dtnewcorrectdatas.Columns.Contains(Dr["SoftwareField"].ToString()))
            //    {
            //        dtnewcorrectdatas.Rows.Add(Dr["SoftwareField"].ToString());
            //    }
            //}

            DataRow drcorrect;
            drcorrect = dtnewcorrectdatas.NewRow();
            //drcorrect["Id"] = txtslno.Text;
            drcorrect["SlNO"] = txtslno.Text;
            drcorrect["StudentName"] = txtstudname.Text;
            drcorrect["Sex"] = RdBtnLstSex.SelectedItem.Text;
            drcorrect["DOB"] = txt_Dob.Text;
            drcorrect["Father/GuardianName"] = txtfathername.Text;
            drcorrect["Religion"] = DrpRelegion.SelectedItem.Text;
            drcorrect["Caste"] = Drpcast.SelectedItem.Text;
            drcorrect["AddressPermanent"] = txtaddress.Text;
            drcorrect["StudentType"] = Drpstudtype.SelectedItem.Text;
            drcorrect["AdmissionNo"] = txtadmno.Text;
            drcorrect["AdmissionDate"] = txtadmdate.Text;
            drcorrect["JoiningBatch"] = Drpjoinbatch.SelectedItem.Text;

            drcorrect["ReligionId"] = DrpRelegion.SelectedValue.ToString();
            drcorrect["CasteId"] = Drpcast.SelectedValue.ToString();
            drcorrect["StudentTypeId"] = Drpstudtype.SelectedValue.ToString();
            drcorrect["JoiningBatchId"] = Drpjoinbatch.SelectedValue.ToString();

            drcorrect["NewAdmission"] = Rdb_NewAdmission.SelectedItem.Text.Trim();
            drcorrect["UseBus"] = Rdb_UseBus.SelectedItem.Text.Trim();
            drcorrect["UseHostel"] = Rdb_UseHostel.SelectedItem.Text.Trim();





            int count = 0;
            if (dynamicFieldDetails != null && dynamicFieldDetails.Tables != null && dynamicFieldDetails.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Dr in dynamicFieldDetails.Tables[0].Rows)
                {
                    if (dtnewcorrectdatas.Columns.Contains(Dr["FieldName"].ToString()) && int.Parse(Dr["IsMandatory"].ToString()) == 0)
                    {
                        drcorrect[Dr["FieldName"].ToString()] = dynamicTextBoxes[count].Text.Trim();
                        count++;
                    }
                }

                //foreach (DataRow Dr in excelField_CusotmerMap.Tables[0].Rows)
                //{
                //    if (int.Parse(Dr["IsDynamic"].ToString()) == 1)
                //    {


                //        drcorrect[Dr["SoftwareField"].ToString()] = dynamicTextBoxes[count].Text.Trim();
                //        count++;
                //    }

                //}


            }


            newcorrectdatas.Tables[0].Rows.Add(drcorrect);

            DataSet UnCorrectData = (DataSet)Session["ExcelValues"];
            //newcorrectdatas = replacedataset(newcorrectdatas, UnCorrectData);

            if (newcorrectdatas.Tables[0] != null)
            {
                DataSet newcorrectdatas1 = new DataSet();
                DataTable dtnewcorrectdatas1;
                newcorrectdatas1.Tables.Add(new DataTable("newcorrectexceldatas1"));
                dtnewcorrectdatas1 = newcorrectdatas1.Tables["newcorrectexceldatas1"];

                dtnewcorrectdatas1.Columns.Add("SlNO");
                //dtnewcorrectdatas1.Columns.Add("Id");
                dtnewcorrectdatas1.Columns.Add("StudentName");
                dtnewcorrectdatas1.Columns.Add("Sex");
                dtnewcorrectdatas1.Columns.Add("DOB");
                dtnewcorrectdatas1.Columns.Add("Father/GuardianName");
                dtnewcorrectdatas1.Columns.Add("Religion");
                dtnewcorrectdatas1.Columns.Add("Caste");
                dtnewcorrectdatas1.Columns.Add("AddressPermanent");
                dtnewcorrectdatas1.Columns.Add("StudentType");
                dtnewcorrectdatas1.Columns.Add("AdmissionNo");
                dtnewcorrectdatas1.Columns.Add("AdmissionDate");
                dtnewcorrectdatas1.Columns.Add("JoiningBatch");

                dtnewcorrectdatas1.Columns.Add("ReligionId");
                dtnewcorrectdatas1.Columns.Add("CasteId");
                dtnewcorrectdatas1.Columns.Add("StudentTypeId");
                dtnewcorrectdatas1.Columns.Add("JoiningBatchId");

                dtnewcorrectdatas1.Columns.Add("NewAdmission");
                dtnewcorrectdatas1.Columns.Add("UseBus");
                dtnewcorrectdatas1.Columns.Add("UseHostel");

                //dtnewcorrectdatas.Columns.Add("StudentId");

                foreach (DataRow Dr in excelField_CusotmerMap.Tables[0].Rows)
                {
                    // if (!dtnewcorrectdatas.Columns.Contains(Dr["SoftwareField"].ToString()) && (int.Parse(Dr["IsDynamic"].ToString()) == 1 || int.Parse(Dr["IsMandatory"].ToString()) == 0))


                    if (!dtnewcorrectdatas1.Columns.Contains(Dr["SoftwareField"].ToString()))
                    {
                        dtnewcorrectdatas1.Columns.Add(Dr["SoftwareField"].ToString());
                    }
                }
                foreach (DataRow Dr in dynamicFieldDetails.Tables[0].Rows)
                {
                    if (!dtnewcorrectdatas1.Columns.Contains(Dr["FieldName"].ToString()))
                    {
                        dtnewcorrectdatas1.Columns.Add(Dr["FieldName"].ToString());
                    }
                }
                foreach (DataRow Drr in newcorrectdatas.Tables[0].Rows)
                {
                    if (Drr["StudentName"] != "")
                    {


                        _religion = Drr["Religion"].ToString();
                        _cast = Drr["Caste"].ToString();
                        _studType = Drr["StudentType"].ToString();
                        _joinBatch = Drr["JoiningBatch"].ToString();
                        if (allareAlreadyPresent(_religion, _cast, _studType, _joinBatch, out _relid, out  _castid, out  _studtypid, out _joinbatchid, out errormsg))
                        {

                            DataRow drcorrect1;
                            drcorrect1 = dtnewcorrectdatas1.NewRow();
                            drcorrect1["SlNO"] = Drr["SlNO"].ToString();
                            drcorrect1["StudentName"] = Drr["StudentName"].ToString();
                            drcorrect1["Sex"] = Drr["Sex"].ToString();
                            drcorrect1["DOB"] = drcorrect["DOB"].ToString();
                            //drcorrect1["DOB"] = MyUser.GerFormatedDatVal(new DateTime(int.Parse(Drr["DOB-Y"].ToString()), int.Parse(Drr["DOB-M"].ToString()), int.Parse(Drr["DOB-D"].ToString())));

                            drcorrect1["Father/GuardianName"] = Drr["Father/GuardianName"].ToString();
                            drcorrect1["Religion"] = Drr["Religion"].ToString();
                            drcorrect1["Caste"] = Drr["Caste"].ToString();
                            drcorrect1["AddressPermanent"] = Drr["AddressPermanent"].ToString();
                            drcorrect1["StudentType"] = Drr["StudentType"].ToString();
                            drcorrect1["AdmissionNo"] = Drr["AdmissionNo"].ToString();
                            drcorrect1["AdmissionDate"] = drcorrect["AdmissionDate"].ToString();
                            //drcorrect1["AdmissionDate"] = MyUser.GerFormatedDatVal(new DateTime(int.Parse(Drr["AdmissionDate-Y"].ToString()), int.Parse(Drr["AdmissionDate-M"].ToString()), int.Parse(Drr["AdmissionDate-D"].ToString())));
                            drcorrect1["JoiningBatch"] = Drr["JoiningBatch"].ToString();

                            drcorrect1["JoiningBatch"] = _joinBatch;
                            drcorrect1["ReligionId"] = _relid.ToString();
                            drcorrect1["CasteId"] = _castid.ToString();
                            drcorrect1["StudentTypeId"] = _studtypid.ToString();
                            drcorrect1["JoiningBatchId"] = _joinbatchid.ToString();

                            drcorrect1["NewAdmission"] = Drr["NewAdmission"].ToString();
                            drcorrect1["UseBus"] = Drr["UseBus"].ToString();
                            drcorrect1["UseHostel"] = Drr["UseHostel"].ToString();
                            foreach (DataRow Dr_Tblfield in excelField_CusotmerMap.Tables[0].Rows)
                            {


                                drcorrect1[Dr_Tblfield["SoftwareField"].ToString()] = Drr[Dr_Tblfield["SoftwareField"].ToString()];


                            }
                            foreach (DataRow Dr_Tblfield in dynamicFieldDetails.Tables[0].Rows)
                            {

                                drcorrect1[Dr_Tblfield["FieldName"].ToString()] = drcorrect[Dr_Tblfield["FieldName"].ToString()];


                            }

                            newcorrectdatas1.Tables[0].Rows.Add(drcorrect1);
                        }
                    }
                }
                //newcorrectdatas1.Tables[0].Columns.Remove("DOB-D");
                //newcorrectdatas1.Tables[0].Columns.Remove("DOB-M");
                //newcorrectdatas1.Tables[0].Columns.Remove("DOB-Y");
                //newcorrectdatas1.Tables[0].Columns.Remove("AdmissionDate-D");
                //newcorrectdatas1.Tables[0].Columns.Remove("AdmissionDate-M");
                //newcorrectdatas1.Tables[0].Columns.Remove("AdmissionDate-Y");
                //newcorrectdatas1.Tables[0].Columns.Remove("Id");


                if (newcorrectdatas1.Tables[0].Rows.Count > 0)
                {
                    Session["CorrectValues"] = null;
                    Session["CorrectValues"] = newcorrectdatas1;
                    this.CorrectExceldtls.Visible = true;
                    lblcorrect.Text = "";

                    Grd_CorrectDtls.Columns[11].Visible = true;
                    Grd_CorrectDtls.Columns[12].Visible = true;
                    Grd_CorrectDtls.Columns[13].Visible = true;
                    Grd_CorrectDtls.Columns[14].Visible = true;

                    Grd_CorrectDetails.Columns[11].Visible = true;
                    Grd_CorrectDetails.Columns[12].Visible = true;
                    Grd_CorrectDetails.Columns[13].Visible = true;
                    Grd_CorrectDetails.Columns[14].Visible = true;

                    Grd_CorrectDtls.DataSource = newcorrectdatas1;//finally bind the Data into the grid
                    Grd_CorrectDtls.DataBind();
                    Grd_CorrectDetails.DataSource = newcorrectdatas1;//finally bind the Data into the grid
                    Grd_CorrectDetails.DataBind();

                    Grd_CorrectDtls.Columns[11].Visible = false;
                    Grd_CorrectDtls.Columns[12].Visible = false;
                    Grd_CorrectDtls.Columns[13].Visible = false;
                    Grd_CorrectDtls.Columns[14].Visible = false;


                    Grd_CorrectDetails.Columns[11].Visible = false;
                    Grd_CorrectDetails.Columns[12].Visible = false;
                    Grd_CorrectDetails.Columns[13].Visible = false;
                    Grd_CorrectDetails.Columns[14].Visible = false;
                    //newcorrectdatas1.Clear();

                }
                else
                {
                    lblcorrect.Text = "No Correct Entries To Display";
                    this.CorrectExceldtls.Visible = false;
                }

            }
        }

        private DataSet replacedataset(DataSet CorrectData, DataSet ExcelData)
        {
            DataSet dataset = new DataSet();
            foreach (DataRow dr in ExcelData.Tables[0].Rows)
            {
                foreach (DataRow dr1 in CorrectData.Tables[0].Rows)
                {
                    //if (dr["Id"].ToString()==  dr1["SlNO"].ToString())
                    if (dr["StudentName"].ToString() == dr1["StudentName"].ToString())
                    {
                        dr["StudentName"] = dr1["StudentName"].ToString();
                        dr["Sex"] = dr1["Sex"].ToString();
                        //dr["DOB"] = dr1["DOB"].ToString();
                        dr["Father/GuardianName"] = dr1["Father/GuardianName"].ToString();
                        dr["Religion"] = dr1["Religion"].ToString();
                        dr["Caste"] = dr1["Caste"].ToString();
                        dr["AddressPermanent"] = dr1["AddressPermanent"].ToString();
                        dr["StudentType"] = dr1["StudentType"].ToString();

                        //dr["AdmissionDate"] = dr1["AdmissionDate"].ToString();
                        dr["JoiningBatch"] = dr1["JoiningBatch"].ToString();
                        dr["AdmissionNo"] = dr1["AdmissionNo"].ToString();
                        dr["NewAdmission"] = dr1["NewAdmission"].ToString();
                        dr["UseBus"] = dr1["UseBus"].ToString();
                        dr["UseHostel"] = dr1["UseHostel"].ToString();
                    }
                    //dataset.Tables[0].Columns.Add=dr1[];
                }
            }
            return ExcelData;
            //{
            //ExcelData_dr = replace column with CorrectData(First row)
            //dataset.add(ExcelData_dr);
            // delet firstrow of CorrectData


            //}

        }

        private bool NewValuesareValid(out string message)
        {
            DateTime tryparsedatetime;
            bool _validvalues = true;
            message = null;
            if ((txt_Dob.Text != "_") || (txt_Dob.Text != ""))
            {
                string _dob = txt_Dob.Text.ToString().Trim();
                if (!MyUser.TryGetDareFromText(_dob, out tryparsedatetime))
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
            if ((txtadmdate.Text != "_") || (txtadmdate.Text != ""))
            {
                string _admdate = txtadmdate.Text.ToString().Trim();
                if (!MyUser.TryGetDareFromText(_admdate, out tryparsedatetime))
                {
                    message = "Date of Admission  is empty";
                    _validvalues = false;
                }
            }
            else
            {
                _validvalues = false;
                message = "Date of Admission empty";
            }

            if ((txtaddress.Text == "_") || (txtaddress.Text.Trim() == ""))
            {
                _validvalues = false;
                message = "Address is empty";
            }

            if ((txtfathername.Text == "_") || (txtfathername.Text.Trim() == ""))
            {
                _validvalues = false;
                message = "Name of Father/Guardian empty";
            }
            if ((txtstudname.Text == "_") || (txtstudname.Text.Trim() == ""))
            {
                _validvalues = false;
                message = "Name of Student empty";
            }

            adnomodevalue = getadmissionnomode();// 1 means  its autonumber generate

            if (adnomodevalue != 1) // if admission no is entered manually
            {
                if ((txtadmno.Text == "_") || (txtstudname.Text.Trim() == ""))
                {
                    _validvalues = false;
                    message = "Admission Number empty";
                }
                else
                {
                    if (!validadno(txtadmno.Text))
                    {
                        _validvalues = false;
                        message = "Admission Number is invalid.Same number may exists";
                    }
                }
            }
            DataSet excelField_CusotmerMap = MyClassMang.getStudentExcelFiledMap();
            DataSet dynamicFieldDetails = MyStudMang.GetCuestomFields();

            int count = 0;
            if (dynamicFieldDetails != null && dynamicFieldDetails.Tables != null && dynamicFieldDetails.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in dynamicFieldDetails.Tables[0].Rows)
                {
                    if (int.Parse(dr["Ismandatory"].ToString()) == 1)
                    {
                        if (dynamicTextBoxes[count].Text.Trim() == "")
                        {
                            _validvalues = false;
                            message = dr["FieldName"] + " is invalid";
                            count++;
                        }

                    }

                }

            }



            return _validvalues;
        }

        private bool validadno(string _adno)
        {
            bool adnovalid = true;
            string sql = "SELECT tblstudent.AdmitionNo FROM tblstudent where tblstudent.AdmitionNo='" + _adno + "'";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                adnovalid = false;
            }
            return adnovalid;
        }

        protected void btn_upload_Click(object sender, EventArgs e)
        {
            if (Grd_UnCorrectDtls.Rows.Count != 0)
                lblerror.Text = "Some student details are not corrected. Correct them and try again";
            else
                Upload_detailstoDataBase();
        }

        private void Upload_detailstoDataBase()
        {
            lblerror.Text = "";
            DataSet dynamicDataset = MyStudMang.GetCuestomFields();
            DataSet staticStudenttableFields = MyStudMang.GetStudenttableFieldWithoutManadatoryField();
            DataSet ValidDetails = (DataSet)Session["CorrectValues"];
            int invalidslno = 0;
            int errFlag = -1;
            #region declare variables to pass to create stud
            string _studname;
            string _sex;
            DateTime _Dob;
            string _Fathername;
            string _address;
            int _joiningbatchid = 0;
            DateTime _Joindate = System.DateTime.Now.Date;

            string _admisionNo = "";
            int _Bloodgroupid = 17;
            string _nationality = "INDIAN";
            int mothertoungid = 1;
            string _mothername = "";
            string _Fathedu = "";
            string _mothedu = "";
            string _fatherOcc = "";
            string _motherOcc = "";
            string _aadharno = "";



            double _Anualincom = 0;

            string _addrspresent = "";
            string _location = "";
            string _State = "";

            int _pin = 0;

            string _resedphon = "";
            string _officephon = "";
            string _email = "";

            string Studid = "";

            int nofbrother = 0;
            int nofsis = 0;
            int firstlng = 1;


            int studcatogory = 0;
            int _relegionId = 0;
            int CastId = 0;

            int _AdmissionType = 1, _UseBus = 0, _UseHostel = 0;
            int _CurrentBatchId = MyUser.CurrentBatchId;

            int _StandardId = 0;
            int _classid = 0;

            string _dynamicfieldname = "";
            string dynamicdatas = "";
            #endregion

            #region declare one dataset to store details of any uninserted rows details
            DataSet uncorrectexceldata = new DataSet();

            DataTable dtuncorrect;
            uncorrectexceldata.Tables.Add(new DataTable("uncorrectexceldatas"));
            dtuncorrect = uncorrectexceldata.Tables["uncorrectexceldatas"];
            dtuncorrect.Columns.Add("SlNo");
            dtuncorrect.Columns.Add("StudentName");
            dtuncorrect.Columns.Add("Sex");
            dtuncorrect.Columns.Add("DOB");
            dtuncorrect.Columns.Add("Father/GuardianName");
            dtuncorrect.Columns.Add("Religion");
            dtuncorrect.Columns.Add("Caste");
            dtuncorrect.Columns.Add("AddressPermanent");
            dtuncorrect.Columns.Add("StudentType");
            dtuncorrect.Columns.Add("AdmissionNo");
            dtuncorrect.Columns.Add("AdmissionDate");
            dtuncorrect.Columns.Add("JoiningBatch");
            dtuncorrect.Columns.Add("Description");

            dtuncorrect.Columns.Add("NewAdmission");
            dtuncorrect.Columns.Add("UseBus");
            dtuncorrect.Columns.Add("UseHostel");

            //dtuncorrect.Columns.Add("StudentId");

            if (dynamicDataset != null && dynamicDataset.Tables != null && dynamicDataset.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow Dr in dynamicDataset.Tables[0].Rows)
                {

                    dtuncorrect.Columns.Add(Dr["FieldName"].ToString());
                }
            }


            #endregion
            if (ValidDetails == null || ValidDetails.Tables[0] == null || ValidDetails.Tables[0].Rows.Count <= 0)
            {
                lblerror.Text = "Cannot read the student records .Please try again";
            }
            else if (getClassStandardId(out _StandardId, out _classid))
            {

                #region insert rows one by one
                int temperr = 0;



                //DataSet UnCorrectData = (DataSet)Session["ExcelValues"];
                //DataSet ddd = (DataSet)Session["ExcelValues"];
                //ValidDetails = replacedataset(ValidDetails, ddd);

                foreach (DataRow dr in ValidDetails.Tables[0].Rows)
                // foreach (GridViewRow gvr in Grd_CorrectDtls.Rows)
                {
                    _AdmissionType = 1;
                    _UseBus = 0;
                    _UseHostel = 0;
                    int createdstatus = -1;//means row is not inserted.If thats inserted then this value changed

                    _studname = dr["StudentName"].ToString().Trim().ToUpper();
                    _sex = dr["Sex"].ToString().Trim().ToUpper();
                    if (_sex == "MALE")
                    {
                        _sex = "Male";
                    }
                    else
                    {
                        _sex = "Female";
                    }
                    _Fathername = dr["Father/GuardianName"].ToString().Trim().ToUpper();
                    _address = dr["AddressPermanent"].ToString().Trim().ToUpper();
                    _admisionNo = dr["AdmissionNo"].ToString().Trim();

                    //_Dob = DateTime.Parse(gvr.Cells[2].Text.Trim());
                    //_Joindate = DateTime.Parse(gvr.Cells[9].Text.Trim());

                    _Dob = MyUser.GetDareFromText(dr["DOB"].ToString().Trim());
                    _Joindate = MyUser.GetDareFromText(dr["AdmissionDate"].ToString().Trim());


                    _relegionId = int.Parse(dr["ReligionId"].ToString().Trim());
                    CastId = int.Parse(dr["CasteId"].ToString().Trim());
                    studcatogory = int.Parse(dr["StudentTypeId"].ToString().Trim());
                    _joiningbatchid = int.Parse(dr["JoiningBatchId"].ToString().Trim());

                    if (dr["NewAdmission"].ToString().Trim().ToLower() == "yes")
                    {
                        _AdmissionType = 2;
                    }
                    if (dr["UseBus"].ToString().Trim().ToLower() == "yes")
                    {
                        _UseBus = 1;
                    }
                    else
                    {
                        _UseBus = 0;
                    }
                    if (dr["UseHostel"].ToString().Trim().ToLower() == "yes")
                    {
                        _UseHostel = 1;
                    }
                    else
                    {
                        _UseHostel = 0;
                    }

                    //}

                    //Studid = dr["StudentId"].ToString().Trim();
                    if (staticStudenttableFields != null && staticStudenttableFields.Tables != null && staticStudenttableFields.Tables[0].Rows.Count > 0)
                    {
                        string field = "";
                        foreach (DataRow dr_studetField in staticStudenttableFields.Tables[0].Rows)
                        {
                            field = dr_studetField["SoftwareField"].ToString();
                            if (field == "BloodGroup")
                                _Bloodgroupid = GetBloodgroupId(dr["BloodGroup"].ToString().Trim());
                            else if (field == "Nationality")
                                _nationality = dr["Nationality"].ToString().Trim();
                            else if (field == "MotherTongue")
                                mothertoungid = getMothertoungId(dr["MotherTongue"].ToString().Trim());
                            else if (field == "MothersName")
                                _mothername = dr["MothersName"].ToString().Trim();
                            else if (field == "FatherEduQuali")
                                _Fathedu = dr["FatherEduQuali"].ToString().Trim();
                            else if (field == "MotherEduQuali")
                                _mothedu = dr["MotherEduQuali"].ToString().Trim();
                            else if (field == "FatherOccupation")
                                _fatherOcc = dr["FatherOccupation"].ToString().Trim();
                            else if (field == "MotherOccupation")
                                _motherOcc = dr["MotherOccupation"].ToString().Trim();
                            else if (field == "Location")
                                _location = dr["Location"].ToString().Trim();
                            else if (field == "State")
                                _State = dr["State"].ToString().Trim();
                            else if (field == "ResidencePhNo")
                                _resedphon = dr["ResidencePhNo"].ToString().Trim();
                            else if (field == "OfficePhNo")
                                _officephon = dr["OfficePhNo"].ToString().Trim();
                            else if (field == "Email")
                                _email = dr["Email"].ToString().Trim();
                            else if (field == "Addresspresent")
                                _addrspresent = dr["Addresspresent"].ToString().Trim();
                            else if (field == "AadharNumber")
                                _aadharno = dr["AadharNumber"].ToString().Trim();
                            else if (field == "Pin")
                            {
                                _pin = 0;
                                int.TryParse(dr["Pin"].ToString().Trim(), out _pin);
                            }
                            else if (field == "AnnualIncome")
                            {
                                _Anualincom = 0;
                                double.TryParse(dr["AnnualIncome"].ToString().Trim(), out _Anualincom);
                            }
                            else if (field == "1stLanguage")
                                firstlng = getMothertoungId(dr["1stLanguage"].ToString().Trim());
                            else if (field == "NumberofBrothers")
                                int.TryParse(dr["NumberofBrothers"].ToString().Trim(), out nofbrother);
                            else if (field == "NumberOfSysters")
                                int.TryParse(dr["NumberOfSysters"].ToString().Trim(), out nofsis);
                           

                        }


                    }



                    int grdval = 18;

                    try
                    {
                        MyStudMang.CreateTansationDb();

                        createdstatus = MyStudMang.CreateStudent(_studname, _sex, _Dob, _Fathername, _address, _joiningbatchid, -1, _Joindate, _StandardId, _classid, _admisionNo, _Bloodgroupid, _nationality, mothertoungid, _mothername, _Fathedu, _mothedu, _fatherOcc, _motherOcc, _Anualincom, _addrspresent, _location, _State, _pin, _resedphon, _officephon, _email, nofbrother, nofsis, firstlng, studcatogory, _relegionId, CastId, _CurrentBatchId, _AdmissionType, "0", _UseBus, _UseHostel, Studid,_aadharno);
                        if (_officephon != "")
                        {
                            MyStudMang.InsertParentMobileNumberIntoSMSParenstsList(createdstatus, _officephon, "");
                        }



                        if (createdstatus != -1)
                        {
                            if (dynamicDataset != null && dynamicDataset.Tables != null && dynamicDataset.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow dr1 in dynamicDataset.Tables[0].Rows)
                                {

                                    if (_dynamicfieldname != "")
                                        _dynamicfieldname = _dynamicfieldname + ",";

                                    _dynamicfieldname = _dynamicfieldname + dr1["DbColumanName"].ToString();

                                    //fieldname = dr1["DbColumanName"].ToString();
                                    if (dynamicdatas != "")
                                        dynamicdatas = dynamicdatas + ",";

                                    dynamicdatas = dynamicdatas + "'" + dr[dr1["FieldName"].ToString()].ToString().Trim().ToLower() + "'";

                                    grdval++;


                                }

                                ViewState["dynamicdatas"] = dynamicdatas;

                                MyStudMang.CreateStudentDetails(_dynamicfieldname, dynamicdatas, createdstatus);
                                _dynamicfieldname = "";
                                dynamicdatas = "";
                            }
                        }


                        if (createdstatus != -1)
                        {
                            if (MyStudMang.ScheduleClassWisePendingFees(createdstatus, _CurrentBatchId, _classid, true))
                            {
                                if (!MyStudMang.NeedStudentApprovel())
                                {
                                    if (MyStudMang.BookScheduledToClass(_classid, MyUser.CurrentBatchId))
                                    {
                                        MyStudMang.ScheduleBookToNewStudent(_classid, createdstatus, MyUser.CurrentBatchId);
                                    }
                                    MyStudMang.ScheduleRollNumber(_classid, MyUser.CurrentBatchId, createdstatus);//assigning roll number
                                }
                                MyStudMang.EndSucessTansationDb();

                                if (!MyStudMang.NeedStudentApprovel())
                                {
                                    Myincident.CreateApprovedIncedent("Student Created", "Student " + _studname + " is admitted to " + MyClassMang.GetClassname(_classid) + " on " + General.GerFormatedDatVal(DateTime.Now), General.GerFormatedDatVal(DateTime.Now), 1, MyUser.UserId, "student", createdstatus, 1, 0, MyUser.CurrentBatchId, _classid);

                                }

                            }
                            else
                            {
                                MyStudMang.EndFailTansationDb();
                                lblerror.Text = "Some error occurs in classwise fee scheduling";
                                errFlag = 1;
                            }
                        }
                        else
                        {
                            MyStudMang.EndFailTansationDb();
                            lblerror.Text = "Some error occurs in student creation. ";
                            errFlag = 1;
                        }

                    }
                    catch (Exception e1)
                    {
                        MyStudMang.EndFailTansationDb();
                        lblerror.Text = "Error:" + e1.Message;
                        errFlag = -1;
                    }


                    if (createdstatus == 1)//means this row is not inserted so build the new dataset for such type of datas
                    {
                        temperr = 1;
                        invalidslno = invalidslno + 1;
                        //SlNo(0)  StudentName(1)  Sex(2)  DOB(3)  Father/GuardianName(4)  Religion(5)  Caste(6)  AddressPermanent(7) StudentType(8)  AdmissionNo(9) AdmissionDate(10) JoiningBatch(11)   
                        buildnoninsertedvaluesdatasetrow(uncorrectexceldata, dtuncorrect, invalidslno, _studname, _sex, _Dob, _Fathername, _address, _admisionNo, _Joindate, dr["Religion"].ToString().Trim(), dr["Caste"].ToString().Trim(), dr["StudentType"].ToString().Trim(), dr["JoiningBatch"].ToString().Trim(), dr["NewAdmission"].ToString().Trim(), dr["UseBus"].ToString().Trim(), dr["UseHostel"].ToString().Trim(), Studid);//, dr["BloodGroup"].ToString().Trim(), _email, _pin.ToString(), _location, _State, _nationality, _Fathedu, _mothername, _mothedu, _fatherOcc, _Anualincom.ToString(), dr["MotherTongue"].ToString().Trim(), _resedphon, _officephon, dr["1stLanguage"].ToString().Trim(), nofbrother.ToString(), nofsis.ToString(), _addrspresent, Studid);

                    }

                }

                if (temperr == 0)
                {
                    Session["UnCorrectValues"] = null;
                    Session["CorrectValues"] = null;
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
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Import Students From Excel", "Some students details are imported from an excel file and saved to the data base", 1);
                        btn_upload.Enabled = false;
                        this.CorrectExceldtls.Visible = false;
                        this.UnCorrectExcelDtls.Visible = false;

                        Grd_CorrectDtls.DataSource = null;
                        Grd_CorrectDtls.DataBind();
                        Grd_UnCorrectDtls.DataSource = null;
                        Grd_UnCorrectDtls.DataBind();
                    }
                    else
                    {
                        this.CorrectExceldtls.Visible = false;
                        Grd_CorrectDtls.DataSource = null;
                        Grd_CorrectDtls.DataBind();

                        lblerror.Text = "Some rows are not inserted.Correct the values and try";
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Import Students from Excel", "Some students details are imported from an excel file and saved to the data base", 1);
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
                    lblerror.Text = "All student Records are successfully saved.";
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Import Students From Excel", "All student recods updated", 1);
                    btn_upload.Enabled = false;
                    this.CorrectExceldtls.Visible = false;
                    this.UnCorrectExcelDtls.Visible = false;

                    Grd_CorrectDtls.DataSource = null;
                    Grd_CorrectDtls.DataBind();
                    Grd_UnCorrectDtls.DataSource = null;
                    Grd_UnCorrectDtls.DataBind();
                }

            }

            else
            {
                lblerror.Text = "Unable to Insert the details.Session may expired, Select class and try again";
            }
        }


        private int getMothertoungId(string languageName)
        {
            int value = 1;
            string sql = "select tbllanguage.Id from tbllanguage where tbllanguage.Language='" + languageName + "'";
            OdbcDataReader reader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (reader.HasRows)
            {

                int.TryParse(reader["Id"].ToString(), out value);

            }
            return value;
        }

        private int GetBloodgroupId(string groupName)
        {
            int value = 17;
            string sql = "select tblbloodgrp.Id from tblbloodgrp where tblbloodgrp.GroupName='" + groupName + "'";
            OdbcDataReader reader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (reader.HasRows)
            {

                int.TryParse(reader["Id"].ToString(), out value);

            }
            return value;
        }


        private void buildnoninsertedvaluesdatasetrow(DataSet uncorrectexceldata, DataTable dtuncorrect, int invalidslno, string _studname, string _sex, DateTime _Dob, string _Fathername, string _address, string _admisionNo, DateTime _Joindate, string relegion, string cast, string studtype, string joinbatch, string _AdmissionType, string _UseBus, string _UseHostel, string StudentId)//,string BloodGroup,string Email,string Location,string Pin,string State,string Nationality,string FatherEduQuali,string MothersName,string MotherEduQuali,string FatherOccupation,string AnnualIncome,string MotherTongue,string  ResidencePhNo,string OfficePhNo,string  FirstLanguage,string NumberofBrothers,string NumberOfSysters,string Addresspresent,string StudentId)
        {
            // SlNo(0)  StudentName(1)  Sex(2)  DOB(3)  Father/GuardianName(4)  Religion(5)  Caste(6)  AddressPermanent(7) StudentType(8)  AdmissionNo(9) AdmissionDate(10) JoiningBatch(11)   
            DataSet dynamicDataset = MyStudMang.GetCuestomFields();


            DataRow druncorrect = dtuncorrect.NewRow();

            druncorrect["SlNo"] = invalidslno;
            druncorrect["StudentName"] = _studname;
            druncorrect["Sex"] = _sex;
            druncorrect["DOB"] = _Dob;
            druncorrect["Father/GuardianName"] = _Fathername;
            druncorrect["Religion"] = relegion;
            druncorrect["Caste"] = cast;
            druncorrect["AddressPermanent"] = _address;
            druncorrect["StudentType"] = studtype;
            druncorrect["AdmissionNo"] = _admisionNo;
            druncorrect["AdmissionDate"] = _Joindate;
            druncorrect["JoiningBatch"] = joinbatch;
            druncorrect["Description"] = "_";

            druncorrect["NewAdmission"] = _AdmissionType;
            druncorrect["UseBus"] = _UseBus;
            druncorrect["UseHostel"] = _UseHostel;




            //druncorrect["StudentId"] = StudentId;

            if (dynamicDataset != null && dynamicDataset.Tables != null && dynamicDataset.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow Dr in dynamicDataset.Tables[0].Rows)
                {

                    dtuncorrect.Columns.Add(Dr["DbColumanName"].ToString());
                }
            }



            uncorrectexceldata.Tables["uncorrectexceldatas"].Rows.Add(druncorrect);
            Session["UnCorrectValues"] = uncorrectexceldata;
        }

        private bool getClassStandardId(out int _StandardId, out int _classid)
        {
            bool _classexist = true;
            _StandardId = 0;

            if (int.TryParse(Session["ClassId"].ToString(), out _classid))
            {
                string sql = "SELECT tblclass.Standard from tblclass where tblclass.Id=" + _classid;
                MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                    _StandardId = int.Parse(MyReader.GetValue(0).ToString());
                else
                    _classexist = false;
            }
            else
                _classexist = false;

            return _classexist;
        }

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

        protected void Btn_UploadDetails_Click(object sender, EventArgs e)
        {
            Clear();

            try
            {
                Session["UnCorrectValues"] = null;
                Session["CorrectValues"] = null;
                string message, _FileName;
                DataSet excelField_CusotmerMap;

                DataSet dynamicFieldDetails = MyStudMang.GetCuestomFields();
                if (dynamicFieldDetails.Tables[0].Rows.Count == 0)
                {
                    excelField_CusotmerMap = MyClassMang.getStudentExcelFiledMap();
                }
                else
                {
                    excelField_CusotmerMap = MyClassMang.getStudentDetailExcelFiledMap();
                }

                if (excelField_CusotmerMap != null && excelField_CusotmerMap.Tables[0].Rows.Count > 0)
                {
                    if (Check_validity_ToUpload(out message))
                    {
                        if (saveTheExcelFile(out _FileName))
                        {

                            // string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath("")) + "\\UpImage\\" + _FileName;
                            string _physicalpath = MyUser.FilePath + "\\UpImage\\" + _FileName;

                            MydataSet = prepareDataset_FromExcel(_physicalpath);      //prepare dataset from the excel
                            //Session["ExcelValues"] = MydataSet;
                            //UplodeddataSet = MydataSet;
                            MydataSet = DataFormatedDataSet(MydataSet, excelField_CusotmerMap, dynamicFieldDetails);
                            builddataset(MydataSet, excelField_CusotmerMap, dynamicFieldDetails); //create new data set based on our requirements
                            File.Delete(MyUser.FilePath + "\\UpImage\\" + _FileName); //delete the file
                        }
                        else

                            lblerror.Text = "Not able to Upload the Excel File. Try again later";

                    }
                    else
                        lblerror.Text = message;
                }
                else
                    lblerror.Text = "Excel-Custmer field map configuration is missing";
            }
            catch (Exception e1)
            {
                lblerror.Text = e1.Message;
            }
        }
        protected void Btn_template_click(object sender, ImageClickEventArgs e)
        {
            DataSet dynamicFieldDetails = MyStudMang.GetCuestomFields();
            if (dynamicFieldDetails.Tables[0].Rows.Count > 0)
            {

                string sql = "";

                DataSet StudentDataSet = new DataSet();
                DataTable Studentdatatbl = new DataTable("Student");
                Studentdatatbl.Columns.Add("SlNo", typeof(int));
                Studentdatatbl.Columns.Add("Student Name*", typeof(String));
                Studentdatatbl.Columns.Add("Sex", typeof(string));
                Studentdatatbl.Columns.Add("Address", typeof(string));
                Studentdatatbl.Columns.Add("Date Of Birth(dd)", typeof(Int32));
                Studentdatatbl.Columns.Add("Date Of Birth(mm)", typeof(Int32));
                Studentdatatbl.Columns.Add("Date Of Birth(yyyy)", typeof(Int32));
                Studentdatatbl.Columns.Add("Father/Guardian Name", typeof(string));
                Studentdatatbl.Columns.Add("Religion", typeof(string));
                Studentdatatbl.Columns.Add("Caste", typeof(string));

                Studentdatatbl.Columns.Add("Student Type", typeof(string));
                Studentdatatbl.Columns.Add("Admission No", typeof(string));
                Studentdatatbl.Columns.Add("Date of Admission(dd)", typeof(Int32));
                Studentdatatbl.Columns.Add("Date of Admission(mm)", typeof(Int32));
                Studentdatatbl.Columns.Add("Date of Admission(yyyy)", typeof(Int32));
                Studentdatatbl.Columns.Add("Joining Batch", typeof(string));
                Studentdatatbl.Columns.Add("New Admission", typeof(string));
                Studentdatatbl.Columns.Add("Using Bus", typeof(string));
                Studentdatatbl.Columns.Add("Using Hostel", typeof(string));
                Studentdatatbl.Columns.Add("BloodGroup", typeof(string));
                Studentdatatbl.Columns.Add("Email", typeof(string));
                Studentdatatbl.Columns.Add("Location", typeof(string));
                Studentdatatbl.Columns.Add("Pin", typeof(string));
                Studentdatatbl.Columns.Add("State", typeof(string));
                Studentdatatbl.Columns.Add("Nationality", typeof(string));
                Studentdatatbl.Columns.Add("FatherEduQuali", typeof(string));
                Studentdatatbl.Columns.Add("AnnualIncome", typeof(string));
                Studentdatatbl.Columns.Add("MotherTongue", typeof(string));
                Studentdatatbl.Columns.Add("ResidencePhNo", typeof(string));
                Studentdatatbl.Columns.Add("OfficePhNo", typeof(string));
                Studentdatatbl.Columns.Add("1stLanguage", typeof(string));
                Studentdatatbl.Columns.Add("NumberofBrothers", typeof(string));
                Studentdatatbl.Columns.Add("NumberofSysters", typeof(string));
               // Studentdatatbl.Columns.Add("JoinBatch", typeof(string));
               // Studentdatatbl.Columns.Add("CreationTime", typeof(string));
                Studentdatatbl.Columns.Add("Addresspresent", typeof(string));
                Studentdatatbl.Columns.Add("StudentId", typeof(string));
                Studentdatatbl.Columns.Add("MothersName", typeof(string));
                Studentdatatbl.Columns.Add("MotherEduQuali", typeof(string));
                
                Studentdatatbl.Columns.Add("FatherOccupation", typeof(string));
                Studentdatatbl.Columns.Add("MotherOccupation", typeof(string));
                Studentdatatbl.Columns.Add("AadharNumber", typeof(string));

                sql = "select tblstudentfieldconfi.Id,tblstudentfieldconfi.FieldName as FieldName,tblstudentfieldconfi.DbColumanName  from tblstudentfieldconfi ";
                DataSet _StudentDs = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (_StudentDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _StudentDs.Tables[0].Rows)
                    {
                        Studentdatatbl.Columns.Add(dr["FieldName"].ToString());
                    }

                }
                sql = "select tblstudent.Id as SlNo,tblstudent.StudentName,tblstudent.Sex,tblstudent.Address,tblstudent.GardianName,tblstudent.Religion,tblstudent.Cast from tblstudent LIMIT 1";
                DataSet val = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (val.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in val.Tables[0].Rows)
                    {
                        Studentdatatbl.Rows.Add(dr["SlNo"], dr["StudentName"].ToString(), dr["Sex"].ToString(), dr["Address"].ToString());
                    }
                }




                StudentDataSet.Tables.Add(Studentdatatbl);
                if (StudentDataSet.Tables[0].Rows.Count > 0)
                {
                    string FileName = "Template.xls";
                    if (!WinEr.ExcelUtility.ExportDataSetToExcel(StudentDataSet, FileName))
                    {

                    }
                }
            }
            else
            {
                string strPath;
                strPath = Server.MapPath("~/UpImage/STUDENTS LIST TEMPLATE.xls");
                if (!string.IsNullOrEmpty(strPath))
                {


                    FileInfo file = new FileInfo(strPath);
                    if (file.Exists)
                    {


                        Response.Clear();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);


                        Response.AddHeader("Content-Length", file.Length.ToString());
                        Response.ContentType = "application/octet-stream";


                        Response.WriteFile(file.FullName);


                        Response.End();


                    }


                }

            }

        }


    }
}




