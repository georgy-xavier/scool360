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
using System.IO;
using System.Data.OleDb;

namespace WinEr
{
    public partial class ImportFeeTransactions : System.Web.UI.Page
    {
        private ClassOrganiser MyClassMang;
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyFeeMang = MyUser.GetFeeObj();
            MyClassMang = MyUser.GetClassObj();
            if (MyFeeMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            //else if (!MyUser.HaveActionRignt(1))
            //{
            //    Response.Redirect("RoleErr.htm");
            //}
            else
            {
                if (!IsPostBack)
                {

                    AddClassNameToDrpList();
                    
                }
            }

        }


        private void AddClassNameToDrpList()
        {
            Drp_ClassName.Items.Clear();

            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {

                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_ClassName.Items.Add(li);

                }

            }
            else
            {
                ListItem li = new ListItem("No Class present", "-1");
                Drp_ClassName.Items.Add(li);
            }
            Drp_ClassName.SelectedIndex = 0;
        }



        protected void Img_ExportTemplate_Click(object sender, ImageClickEventArgs e)
        {
            DataSet MyData = GetFeeImportingexcel(Drp_ClassName.SelectedValue);

            if (MyData.Tables[0].Rows.Count > 0)
            {
                //if (!WinEr.ExcelUtility.ExportDataSetToExcel(MyExamData, "ExamResult.xls"))
                //{
                //    Lbl_message.Text = "This function need Ms office";
                //}

                string FileName = Drp_ClassName.SelectedItem.Text + " FeeImport.xls";
                if (!WinEr.ExcelUtility.ExportDataSetToExcel(MyData, FileName))
                {

                    // Lbl_message.Text = "This function need Ms office";
                }
            }
        }



        private DataSet GetFeeImportingexcel(string ClassId)
        {
            DataSet FeeDataSet = new DataSet();
            DataTable dt;
            DataRow dr;

            try
            {
                FeeDataSet.Tables.Add(new DataTable("Fee"));
                dt = FeeDataSet.Tables["Fee"];
                dt.Columns.Add("AdmitionNo");
                dt.Columns.Add("StudentName");
                int _Fee_ImportCount = MyFeeMang.GetFeeImport_ColumnCount();
                if (_Fee_ImportCount > 0)
                {



                    for (int i = 1; i <= _Fee_ImportCount; i++)
                    {
                        dt.Columns.Add("Payment" + i);
                    }

                    dt.Columns.Add("Total Fees Paid");

                    DataSet Students = MyClassMang.GetStudentlist_AdmissionNo(int.Parse(ClassId), MyUser.CurrentBatchId);
                    if (Students != null && Students.Tables != null && Students.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr_Student in Students.Tables[0].Rows)
                        {
                            dr = FeeDataSet.Tables["Fee"].NewRow();
                            dr["StudentName"] = dr_Student[1].ToString();
                            dr["AdmitionNo"] = dr_Student[0].ToString();

                            FeeDataSet.Tables["Fee"].Rows.Add(dr);
                        }
                    }
                    else
                    {
                        WC_MessageBox.ShowMssage("No student present in the selected class");
                    }

                }
                else
                {
                    WC_MessageBox.ShowMssage("Excel Confguration Error. Please contact admin");
                }
                
            }
            catch(Exception Ex)
            {
                WC_MessageBox.ShowMssage("Error while downloading. Message : "+Ex.Message);
            }
            return FeeDataSet;
        }



        protected void Drp_ClassName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Btn_Upload_Click(object sender, EventArgs e)
        {
            
            string message, _FileName;
            if (Check_validity_ToUpload(out message))
            {
                if (saveTheExcelFile(out _FileName))
                {
                    string _msg = "";
                    bool _valid = true;
                    // string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath("")) + "\\UpImage\\" + _FileName;
                    string _physicalpath = MyUser.FilePath + "\\UpImage\\" + _FileName;

                    MydataSet = prepareDataset_FromExcel(_physicalpath, out _msg, out _valid);      //prepare dataset from the excel
                    if (_valid)
                    {
                        builddataset(MydataSet, out message); //create new data set based on our requirements
                    }
                    else
                    {
                        message = _msg;
                    }
                    File.Delete(MyUser.FilePath + "\\UpImage\\" + _FileName); //delete the file
                }
                else
                    message = "Not able to Upload the Excel File. Try again later";

            }

            WC_MessageBox.ShowMssage(message);
            
        }

        private void builddataset(DataSet MydataSet, out string _msg)
        {
            _msg = "";
            bool _valid = true;
            try
            {
                int Price_Column = MyFeeMang.GetFeeImport_ColumnCount();
                double _totalamount_excel = 0;
                string[] ExcelHead = new string[MydataSet.Tables[0].Columns.Count];

                for (int i = 0; i < ExcelHead.Length; i++)
                {
                    ExcelHead[i] = MydataSet.Tables[0].Columns[i].ColumnName.Trim();
                }
                if (ExcelHead.Length > 2)
                {
                    string[,] Student = new string[MydataSet.Tables[0].Rows.Count, 2];
                    string[] Other_Columnname = new string[MydataSet.Tables[0].Columns.Count - 2];
                    double[,] FeeAmount = new double[MydataSet.Tables[0].Rows.Count, Other_Columnname.Length];


                    if (ExcelHead[0] != "AdmitionNo")
                    {
                        _msg = "AdmitionNo not found";
                        _valid = false;
                    }
                    else if (ExcelHead[1] != "StudentName")
                    {
                        _msg = "StudentName not found";
                        _valid = false;
                    }

                    if (_valid)
                    {
                        for (int i = 0; i <= Student.GetUpperBound(0); i++)
                        {
                            int StudentId = MyFeeMang.getstudentid(MydataSet.Tables[0].Rows[i][1].ToString().Trim(), MydataSet.Tables[0].Rows[i][0].ToString().Trim(), Drp_ClassName.SelectedValue, MyUser.CurrentBatchId);
                            if (StudentId > 0)
                            {
                                Student[i, 0] = StudentId.ToString();
                                Student[i, 1] = MydataSet.Tables[0].Rows[i][1].ToString();
                            }
                            else
                            {
                                _msg = "Student data is miss matching for student : " + MydataSet.Tables[0].Rows[i][1].ToString();
                                _valid = false;
                                break;
                            }


                        }
                    }

                    if (_valid)
                    {


                        for (int row = 0; row <= Student.GetUpperBound(0); row++)
                        {
                            string _StudentId = Student[row, 0];
                            string _StudentName = Student[row, 1];
                            double _pendingPayment = MyFeeMang.GetTotalFeeAmount_Student(_StudentId);
                            double _totalPaymentSum = 0;
                            for (int column = 0; column < Other_Columnname.Length; column++)
                            {

                                double _amount = 0;
                                double.TryParse(MydataSet.Tables[0].Rows[row][column + 2].ToString().Trim(), out _amount);

                                if (MydataSet.Tables[0].Columns[column + 2].ColumnName.ToString().Trim() != "Total Fees Paid")
                                {
                                    _totalPaymentSum = _totalPaymentSum + _amount;
                                    _totalamount_excel = _totalamount_excel + _amount;
                                    FeeAmount[row, column] = _amount;
                                }
                                else
                                {

                                    if (_totalPaymentSum == _amount)
                                    {

                                        if (_pendingPayment < _totalPaymentSum)
                                        {
                                            _valid = false;
                                            _msg = "Total amount payable is less than what entered in excel for student " + _StudentName;
                                        }

                                    }
                                    else
                                    {
                                        _valid = false;
                                        _msg = "Sum of payment entered is not matching with the Total Fees Paid column for student " + _StudentName;
                                    }

                                }

                                if (!_valid)
                                {
                                    break;
                                }

                            }
                        }
                    }


                    if (_valid)
                    {
                        if (_totalamount_excel == 0)
                        {
                            _valid = false;
                            _msg = "No Fee transaction found in the excel";
                        }
                    }

                    if (_valid)
                    {

                        try
                        {
                            MyFeeMang.CreateTansationDb();

                            for (int studentrow = 0; studentrow <= Student.GetUpperBound(0); studentrow++)
                            {
                                for (int Othercolumn = 0; Othercolumn < Other_Columnname.Length - 1; Othercolumn++)
                                {
                                    double _amount=0;
                                    if (Other_Columnname[Othercolumn] != "")
                                    {
                                        double.TryParse(FeeAmount[studentrow, Othercolumn].ToString(),out _amount);
                                        if(_amount>0)
                                        {
                                            MyFeeMang.StoreFeeTransactions(Student[studentrow, 0], Student[studentrow, 1], _amount, MyUser.UserId, MyUser.CurrentBatchId,int.Parse(Drp_ClassName.SelectedValue),MyUser.UserName);
                                        }
                                    }
                                    else
                                    {


                                    }

                                    
                                    
                                }
                            }

                            MyFeeMang.EndSucessTansationDb();
                            _msg = "Fee Transactions Successfully Imported";
                        }
                        catch (Exception ex)
                        {
                            MyFeeMang.EndFailTansationDb();
                            _msg = "Error Message : " + ex.Message;
                        }

                    }
                }

                if (_msg == "")
                {
                    _msg = "Excel data is not valid";
                }

            }
            catch (Exception ex)
            {
                _msg = ex.Message;
            }

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

        private DataSet prepareDataset_FromExcel(string _physicalpath, out string _msg, out bool _valid)
        {
            _msg = "";
            _valid = false;
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
                _valid = true;
                return ds;

            }
            catch (Exception ex)
            {
                _msg = ex.Message;
                DataSet ds = null;
                return ds;
            }
            finally
            {
                con.Close();
            }



        }


        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {

        }
    }
}
