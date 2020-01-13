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
    public partial class ImportExamMarks : System.Web.UI.Page
    {
        private ExamManage MyExamMang;
        private KnowinUser MyUser;
        private ClassOrganiser MyClassMang;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (Session["ExamId"] == null)
            {
                Response.Redirect("ViewExams.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyExamMang = MyUser.GetExamObj();
            MyClassMang = MyUser.GetClassObj();
            if (MyExamMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(856))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    string _ExamMenuStr;
                    _ExamMenuStr = MyExamMang.GetSubExamMangMenuString(MyUser.UserRoleId);
                    this.SubExammngMenu.InnerHtml = _ExamMenuStr;
                    LoadExamGeneralDetails();
                    LoadClassAndPerdiod();
                    SelectClassAndPeriod();
                    //SetPanels();               
                    //some initlization
                }
            }
        }



        private void LoadExamGeneralDetails()
        {
            string ExamNme, ExamType, examfreqncy;
            MyExamMang.GetExamDetail(int.Parse(Session["ExamId"].ToString()), out ExamNme, out ExamType, out examfreqncy);
            Lbl_ExamName.Text = ExamNme.ToString();
            Lbl_ExamType.Text = ExamType.ToString();
            Lbl_Frequency.Text = examfreqncy.ToString();
        }


        private void LoadClassAndPerdiod()
        {
            LoadClass();
            LoadPeriod();
        }

        private void LoadPeriod()
        {
            DrpExamPeriod.Items.Clear();
            string sql = "SELECT tblperiod.Id, tblperiod.Period FROM tblexammaster inner join tblperiod  on tblexammaster.PeriodTypeId = tblperiod.FrequencyId WHERE tblexammaster.Id=" + int.Parse(Session["ExamId"].ToString());
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    DrpExamPeriod.Items.Add(li);
                }
            }
        }

        private void LoadClass()
        {
            DrpClassName.Items.Clear();
            MydataSet = MyExamMang.MyAssociatedExamClass(MyUser.UserId, int.Parse(Session["ExamId"].ToString()));
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    DrpClassName.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Class present", "-1");
                DrpClassName.Items.Add(li);
            }
            DrpClassName.SelectedIndex = 0;
        }

        private void SelectClassAndPeriod()
        {
            if (Session["ClassId"] != null)
            {
                DrpClassName.SelectedValue = Session["ClassId"].ToString();
            }
            if (Session["PeriodId"] != null)
            {
                DrpExamPeriod.SelectedValue = Session["PeriodId"].ToString();
            }
        }


        protected void DrpClassName_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ClassId"] = DrpClassName.SelectedValue;
            Lbl_msg.Text = "";
        }

        protected void DrpExamPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["PeriodId"] = DrpExamPeriod.SelectedValue;
        }

        protected void Btn_UploadDetails_Click(object sender, EventArgs e)
        {
            //Clear();
            Lbl_msg.Text = "";
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


            Lbl_msg.Text = message;

        }

        private void builddataset(DataSet MydataSet, out string _msg)
        {
            _msg = "";
            bool _valid = true;
            try
            {
                string[] ExcelHead = new string[MydataSet.Tables[0].Columns.Count];

                for (int i = 0; i < ExcelHead.Length; i++)
                {
                    //ExcelHead[i] = MydataSet.Tables[0].Columns[i].ColumnName.Trim();
                    ExcelHead[i] = MydataSet.Tables[0].Rows[0][i].ToString().Trim();
                }
                if (ExcelHead.Length > 2)
                {
                    string[,] Subject = new string[MydataSet.Tables[0].Columns.Count - 2, 2];
                    string[,] Student = new string[MydataSet.Tables[0].Rows.Count - 1, 2];
                    string[,] Marks = new string[MydataSet.Tables[0].Rows.Count - 1, Subject.Length];
                    string[] Subject_Columnname = new string[MydataSet.Tables[0].Columns.Count - 2];

                    if (ExcelHead[0] != "Roll No")
                    {
                        _msg = "RollNo not found";
                        _valid = false;
                    }
                    else if (ExcelHead[1] != "StudentName")
                    {
                        _msg = "StudentName not found";
                        _valid = false;
                    }
                    if (_valid)
                    {
                        for (int i = 2; i < ExcelHead.Length; i++)
                        {
                            string _subjectId = MyExamMang.getsubjectid(ExcelHead[i]);
                            if (_subjectId == "0")
                            {
                                _subjectId = MyExamMang.getsubjectid(ExcelHead[i].Replace('(', '[').Replace(')', ']'));
                                if (_subjectId == "0")
                                {
                                    _msg = "Subject name miss match on subject : " + ExcelHead[i];
                                    _valid = false;
                                    break;
                                }
                            }


                            Subject_Columnname[i - 2] = "";
                            if (_subjectId != "0")
                            {

                                Subject_Columnname[i - 2] = MyExamMang.GetsubjectColumn(int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, _subjectId, DrpClassName.SelectedValue);
                            }


                            Subject[i - 2, 0] = _subjectId;
                            Subject[i - 2, 1] = ExcelHead[i];
                        }
                    }
                    if (_valid)
                    {
                        for (int i = 1; i <= Student.GetUpperBound(0); i++)
                        {
                            int StudentId = MyExamMang.getstudentid(MydataSet.Tables[0].Rows[i + 1][1].ToString().Trim(), MydataSet.Tables[0].Rows[i + 1][0].ToString().Trim(), DrpClassName.SelectedValue, MyUser.CurrentBatchId);
                            if (StudentId > 0)
                            {
                                Student[i, 0] = StudentId.ToString();
                                Student[i, 1] = MydataSet.Tables[0].Rows[i + 1][1].ToString();
                            }
                            else
                            {
                                _msg = "Student data is miss matching for student : " + MydataSet.Tables[0].Rows[i + 1][1].ToString();
                                _valid = false;
                                break;
                            }


                        }
                    }

                    if (_valid)
                    {

                        for (int row = 1; row <= Student.GetUpperBound(0); row++)
                        {
                            for (int column = 0; column <= Subject.GetUpperBound(0); column++)
                            {
                                if (MydataSet.Tables[0].Rows[row + 1][column + 2].ToString().Trim() == "a")
                                {
                                    string ab = "a";
                                    Marks[row, column] = ab;
                                }


                                else if (MydataSet.Tables[0].Rows[row + 1][column + 2].ToString().Trim() != "")
                                {
                                    double _mark = -2;
                                    double.TryParse(MydataSet.Tables[0].Rows[row + 1][column + 2].ToString().Trim(), out _mark);
                                    if (_mark > -2)
                                    {
                                        Marks[row, column] = _mark.ToString();
                                    }
                                    else
                                    {
                                        _msg = "Incorrect mark for student :" + Student[row, 1] + " on subject : " + Subject[column, 1];
                                        _valid = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    _msg = "Incorrect mark for student :" + Student[row, 1] + " on subject : " + Subject[column, 1];
                                    _valid = false;
                                    break;
                                }
                            }
                            if (!_valid)
                            {
                                break;
                            }

                        }
                    }



                    if (_valid)
                    {

                        if (!chk_removemarks.Checked)
                        {
                            for (int i = 0; i <= Subject.GetUpperBound(0); i++)
                            {
                                if (MarkAllreadyEntered_Subject(Subject[i, 0]))
                                {

                                    _msg = "Mark already entered for subject : " + Subject[i, 1];
                                    _valid = false;
                                    break;

                                }
                            }
                        }
                    }

                    if (_valid)
                    {

                        try
                        {
                            MyExamMang.CreateTansationDb();
                            MyExamMang.RemoveEnteredMarks(DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue, MyUser.CurrentBatchId, int.Parse(Session["ExamId"].ToString()));

                            for (int studentrow = 1; studentrow <= Student.GetUpperBound(0); studentrow++)
                            {
                                for (int Subjectcolumn = 0; Subjectcolumn <= Subject.GetUpperBound(0); Subjectcolumn++)
                                {
                                    if (Subject_Columnname[Subjectcolumn] != "")
                                    {
                                        if (Marks[studentrow, Subjectcolumn] == "a")
                                        {
                                            MyExamMang.FillMarks(DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue, MyUser.CurrentBatchId, int.Parse(Student[studentrow, 0]), int.Parse(Session["ExamId"].ToString()), Subject_Columnname[Subjectcolumn], -1);
                                        }
                                        else
                                        {
                                            MyExamMang.FillMarksAB(DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue, MyUser.CurrentBatchId, int.Parse(Student[studentrow, 0]), int.Parse(Session["ExamId"].ToString()), Subject_Columnname[Subjectcolumn], Marks[studentrow, Subjectcolumn]);
                                        }
                                    }
                                }
                            }
                            MyExamMang.UpdateExamSchedule(DrpClassName.SelectedValue, DrpExamPeriod.SelectedValue, int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, "Updated");
                            MyExamMang.EndSucessTansationDb();
                            _msg = "Mark Successfully Imported";
                        }
                        catch (Exception ex)
                        {
                            MyExamMang.EndFailTansationDb();
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



        private bool MarkAllreadyEntered_Subject(string subjectId)
        {
            bool _valid = false;
            string MarkColumn = "";
            string sql1 = "SELECT tblexammark.MarkColumn FROM tblexammark inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId WHERE tblclassexam.ExamId=" + int.Parse(Session["ExamId"].ToString()) + " AND tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " AND tblexammark.SubjectId=" + subjectId + " AND tblclassexam.ClassId=" + DrpClassName.SelectedValue + " order by tblexammark.SubjectOrder";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql1);
            if (MyReader.HasRows)
            {
                MarkColumn = MyReader.GetValue(0).ToString();

            }
            if (MarkColumn != "")
            {
                sql1 = "Select SUM(tblstudentmark." + MarkColumn + ") From tblstudentmark inner join tblexamschedule on tblexamschedule.Id=tblstudentmark.ExamSchId inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId  where tblexamschedule.PeriodId=" + DrpExamPeriod.SelectedValue + " AND tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " And tblclassexam.ClassId=" + DrpClassName.SelectedValue + " And tblclassexam.ExamId=" + int.Parse(Session["ExamId"].ToString());
                MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql1);
                if (MyReader.HasRows)
                {
                    double _totalMark = 0;
                    double.TryParse(MyReader.GetValue(0).ToString(), out _totalMark);
                    if (_totalMark > 0)
                    {
                        _valid = true;
                    }
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
            _valid = true;
            _msg = "";
            OleDbConnection con;
            System.Data.DataTable dt = null;
            //Connection string for oledb



           // string conn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + _physicalpath + "; Extended Properties='Excel 8.0;IMEX=1;';";
            string conn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + _physicalpath + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1'";


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
                    if (dr["TABLE_NAME"].ToString().Replace("$", "").Replace("'", "") == DrpClassName.SelectedItem.Text)
                    {
                        excelsheets[i] = dr["TABLE_NAME"].ToString();
                        i++;
                    }
                    else
                    {
                        _valid = false;
                        _msg = "Selected Class Is Not Matching With Work Sheet";
                    }
                }
                // here  manaually give the sheet number in the string array
                DataSet ds = new DataSet();
                if (_valid)
                {
                    foreach (string temp in excelsheets)
                    {
                        // Query to get the data for the excel sheet 
                        //temp is the sheet name
                        string query = "select * from [" + temp + "]";
                        OleDbDataAdapter adp = new OleDbDataAdapter(query, con);
                        adp.Fill(ds, temp);//fill the excel sheet data into a dataset ds
                    }
                }
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

        protected void Btn_Template_Click(object sender, EventArgs e)
        {
            int i_ExamId = int.Parse(Session["ExamId"].ToString());
            int Periodid = int.Parse(DrpExamPeriod.SelectedValue);
            DataSet MyExamData = GetAllMarkImportingexcel(Periodid, i_ExamId, MyUser.CurrentBatchId);

            if (MyExamData.Tables[0].Rows.Count > 0)
            {
                //if (!WinEr.ExcelUtility.ExportDataSetToExcel(MyExamData, "ExamResult.xls"))
                //{
                //    Lbl_message.Text = "This function need Ms office";
                //}

                string FileName = DrpClassName.SelectedItem.Text + ".xls";
                if (!WinEr.ExcelUtility.ExportDataSetToExcel(MyExamData, FileName))
                {

                    // Lbl_message.Text = "This function need Ms office";
                }
            }
        }


        private DataSet GetAllMarkImportingexcel(int _Periodid, int i_ExamId, int _BatchId)
        {
            DataSet ExamDataSet = new DataSet();
            DataTable dt;
            DataRow dr;
            int i, j;

            try
            {
                ExamDataSet.Tables.Add(new DataTable("Exam"));
                dt = ExamDataSet.Tables["Exam"];

                DataSet Subjects = MyClassMang.GetScheduledSubjectList(int.Parse(DrpClassName.SelectedValue), i_ExamId, _BatchId);
                string[] name = new string[100];
                if (Subjects != null && Subjects.Tables != null && Subjects.Tables[0].Rows.Count > 0)
                {

                    dt.Columns.Add("Roll No");
                    dt.Columns.Add("StudentName");
                    name[0] = "StudentName";
                    i = 1;

                    foreach (DataRow subject in Subjects.Tables[0].Rows)
                    {
                        dt.Columns.Add(subject[1].ToString());
                        name[i] = subject[1].ToString();
                        i++;
                    }

                    dr = ExamDataSet.Tables["Exam"].NewRow();
                    dr["StudentName"] = " ";

                    j = 0;
                    foreach (DataRow subject in Subjects.Tables[0].Rows)
                    {
                        j++;
                        dr[name[j]] = " ";
                    }


                    ExamDataSet.Tables["Exam"].Rows.Add(dr);


                    DataSet Students = MyClassMang.GetStudentlist(int.Parse(DrpClassName.SelectedValue), _BatchId);
                    if (Students != null && Students.Tables != null && Students.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr_Student in Students.Tables[0].Rows)
                        {
                            dr = ExamDataSet.Tables["Exam"].NewRow();
                            j = 0;
                            dr[name[j]] = dr_Student[1].ToString();
                            dr["Roll No"] = dr_Student[0].ToString();
                            ExamDataSet.Tables["Exam"].Rows.Add(dr);
                        }
                    }
                    
                }
                else
                {
                    Lbl_msg.Text = "Exam is not scheduled for the selected class";
                }
            }
            catch
            {

            }
            return ExamDataSet;
        }


    }
}
