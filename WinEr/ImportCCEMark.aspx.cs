using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace WinEr
{
    public partial class ImportCCEMark : System.Web.UI.Page
    {

        private ExamManage MyExamMang;
        private KnowinUser MyUser;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyExamMang = MyUser.GetExamObj();
            if (MyExamMang == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }
            else
            {
                if (!IsPostBack)
                {
                    //some initlization
                    Load_ClassDropDown();
                    Load_ExamDropDown();
                    Load_ExamMaxMark();
                    Drp_Class.Focus();
                }
            }

        }

        private void Load_ExamMaxMark()
        {
            Txt_Maxmark.Text = "";
            string sql = "";
            int id = int.Parse(Drp_Exam.SelectedItem.Value.ToString());
            sql = "SELECT tblcce_colconfig.ExamMaxMark as ExamMaxMark from tblcce_colconfig WHERE tblcce_colconfig.Id=" + id + "";
            DataSet ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Txt_Maxmark.Text = dr["ExamMaxMark"].ToString();

                }
                Validationrow.Visible = true;
            }
            else
                Validationrow.Visible =false;
        }

        private void Load_ExamDropDown()
        {
            Drp_Exam.Items.Clear();
            int _ClassId = int.Parse(Drp_Class.SelectedValue);
            string sql = "SELECT DISTINCT(tblcce_colconfig.Id),tblcce_colconfig.ExamName as ExamName from tblcce_colconfig inner JOIN tblcce_classgroupmap ON tblcce_classgroupmap.GroupId=tblcce_colconfig.GroupId where  tblcce_classgroupmap.ClassId=" + _ClassId + " AND tblcce_colconfig.TableName='tblcce_mark'";
            DataSet Ds_class = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (Ds_class != null && Ds_class.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drcls in Ds_class.Tables[0].Rows)
                {
                    li = new ListItem(drcls["ExamName"].ToString(), drcls["Id"].ToString());
                    Drp_Exam.Items.Add(li);
                }
                Validationrow.Visible = true;
            }
            else
            {
                li = new ListItem("NO Data Found", "0");
                Drp_Exam.Items.Add(li);
                Validationrow.Visible = false;

            }
        }

        private void Load_ClassDropDown()
        {
            Drp_Class.Items.Clear();
            string sql = "SELECT tblclass.Id,tblclass.ClassName from tblclass where tblclass.Status=1 ORDER BY tblclass.ClassName";
            DataSet Ds_class = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (Ds_class != null && Ds_class.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drcls in Ds_class.Tables[0].Rows)
                {
                    li = new ListItem(drcls["ClassName"].ToString(), drcls["Id"].ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("NO Data Found", "0");
                Drp_Class.Items.Add(li);
            }
        }

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_ExamDropDown();
            Load_ExamMaxMark();
        }

        protected void Drp_Exam_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_ExamMaxMark();
        }
       
        protected void Btn_Template_Click(object sender, EventArgs e)
        {
            string _msg="";
            int _ClassId = int.Parse(Drp_Class.SelectedValue);
            int _ExamID = int.Parse(Drp_Exam.SelectedValue);
            if (_ClassId==0)
                _msg = "Class name is not Found!";
            if (_ExamID == 0)
                _msg = "Exam name is not found!";
            else
            {
                string sql = "";
 
                DataSet ExamDataSet = new DataSet();
                DataTable Examdatatbl = new DataTable("Exam");
                Examdatatbl.Columns.Add("StudentId", typeof(int));
                Examdatatbl.Columns.Add("StudentName", typeof(string));
                Examdatatbl.Columns.Add("StudentRollNo", typeof(int));

                #region Subject

                sql = "select tblsubjects.Id as SubjectId,tblsubjects.subject_name as SubjectName from tblsubjects inner JOIN tblcce_classsubject ON tblcce_classsubject.subjectid=tblsubjects.Id where  tblcce_classsubject.classid=" + _ClassId+" ORDER BY tblcce_classsubject.SubjectOrder";
                DataSet _Subject = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (_Subject.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr_sub in _Subject.Tables[0].Rows)
                    {
                        Examdatatbl.Columns.Add(dr_sub["SubjectName"].ToString(), typeof(string));

                    }
                }

                #endregion

                sql = "SELECT tblstudent.Id as StudentId,tblstudent.StudentName as StudentName,tblstudentclassmap.RollNo as StudentRollNo from tblstudent INNER JOIN tblstudentclassmap on  tblstudentclassmap.StudentId=tblstudent.Id where tblstudentclassmap.ClassId=" + _ClassId + " AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " ORDER by tblstudentclassmap.RollNo";
                DataSet _StudentDs = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (_StudentDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _StudentDs.Tables[0].Rows)
                    {
                        Examdatatbl.Rows.Add(int.Parse(dr["StudentId"].ToString()), dr["StudentName"].ToString(), int.Parse(dr["StudentRollNo"].ToString()), "");
                    }
                }
                ExamDataSet.Tables.Add(Examdatatbl);

                if (ExamDataSet.Tables[0].Rows.Count > 0)
                {
                    string FileName = Drp_Class.SelectedItem.Text + ".xls";
                    if (!WinEr.ExcelUtility.ExportDataSetToExcel(ExamDataSet, FileName))
                    {

                    }
                }

            }

            if (_msg != "")
            {
                Label1.Text = _msg;
                Label1.Visible = true;
            }
            else
                Label1.Visible = true;
        }

        protected void Btn_upload_Click(object sender, EventArgs e)
        {
            CLogging logger = CLogging.GetLogObject();
            string message = "", _FileName,studentname="";
            DataSet MydataSet = null;
            int _ClassId=int.Parse(Drp_Class.SelectedValue);
            int _ExamId = int.Parse(Drp_Exam.SelectedValue);

            if (_ClassId == 0)
                message = "Class name is not found!";
            else if (_ExamId == 0)
                message = "Exam name is not found!";
            else
            {
                try
                {
                    if (Check_validity_ToUpload(out message))
                    {
                        if (saveTheExcelFile(out _FileName))
                        {
                            string _msg = "";
                            bool _valid = true;
                            string _physicalpath = MyUser.FilePath + "\\UpImage\\" + _FileName;
                            MydataSet = prepareDataset_FromExcel(_physicalpath, out _msg, out _valid);
                           
                           
                            if (!_valid)
                            {
                                message = _msg;
                            }
                            else
                            {

                                if (Check_MaximumMark_Validation(MydataSet,out _msg,out studentname,_ClassId))
                                {
                                    string sql = "";
                                    int _stuid = 0, _subid = 0;
                                    string _columname = "", _Studentname = "";
                                    string _mark = "0"; 
                                    int rowcolcount = 0;
                                    _columname = GetMarkenteryColumnName();
                                    foreach (DataRow dr in MydataSet.Tables[0].Rows)
                                    {
                                        _stuid = int.Parse(dr["StudentId"].ToString());
                                        _Studentname = dr["StudentName"].ToString();
                                        rowcolcount = dr.Table.Columns.Count;
                                        for (int i = 3; i < rowcolcount; i++)
                                        {
                                            _subid = GetSubjectId(dr.Table.Columns[i].ColumnName.ToString(), _ClassId);
                                            if (_subid > 0)
                                            {
                                                try
                                                {
                                                    _mark = dr[i].ToString();
                                                    string ChkCondition = _mark;
                                                    if (ChkCondition.ToUpper() == "-1" || ChkCondition.ToUpper() == "ABSENT")
                                                        _mark = "ABSENT";

                                                }
                                                catch
                                                {
                                                    _mark = "";
                                                }

                                                if (!CheckWithThisSubIDandStuIDExitOrNot(_stuid, _subid))
                                                {
                                                    sql = "INSERT into tblcce_mark (tblcce_mark.StudentId,tblcce_mark.SubjectId,tblcce_mark." + _columname + ") VALUES (" + _stuid + "," + _subid + ",'" + _mark + "')";
                                                    logger.LogToFile("Mark Entry", _Studentname +" mark  inserted ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, MyUser.LoginUserName);
                                                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Mark Entry", _Studentname + " mark  inserted ", 1);

                                                }
                                                else
                                                {
                                                    sql = "UPDATE tblcce_mark SET tblcce_mark." + _columname + "='" + _mark + "' where tblcce_mark.StudentId=" + _stuid + " and tblcce_mark.SubjectId=" + _subid;
                                                    logger.LogToFile("Mark Entry", _Studentname + " mark  Updated ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, MyUser.LoginUserName);
                                                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Mark Entry", _Studentname + " mark  Updated ", 1);
                                                }
                                                MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                                            }
                                            else
                                            {
                                                message = dr.Table.Columns[i].ColumnName.ToString() + " is not identified as subject";
                                                break;
                                            }
                                        }
                                    }
                                    message = "CCEMark updateded sucessfully";
                                    if (File.Exists(_physicalpath))
                                        File.Delete(_physicalpath);
                                }
                                else
                                {
                                    message = _msg;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogToFile("Mark Entry", "throws Error" + ex.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, MyUser.UserName);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Mark Entry", "throws Error" + ex.Message, 1);
                    WC_MessageBox.ShowMssage(ex.Message);
                }
            }

            if (message != "")
            {
                Label1.Text = message;
                Label1.Visible = true;
            }
            else
                Label1.Visible = true;
        }

        private bool Check_MaximumMark_Validation(DataSet MydataSet, out string _msg, out string studentname,int _ClassId)
        {
            bool valid = true;
            int rowcolcount = 0;
            int MaximumMark = 0;
            string Mark="";
            string subjectname = "";
            if (Txt_Maxmark.Text != "")
                MaximumMark = int.Parse(Txt_Maxmark.Text);
            _msg = "";
            studentname = "";
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                     studentname = dr["StudentName"].ToString();
                    rowcolcount = dr.Table.Columns.Count;
                    for (int i = 3; i < rowcolcount; i++)
                    {
                        subjectname = MydataSet.Tables[0].Columns[i].ColumnName;
                        int _subid = 0;
                        _subid = GetSubjectId(dr.Table.Columns[i].ColumnName.ToString(), _ClassId);
                        if (_subid > 0)
                        {
                            try
                            {
                                Mark = dr[i].ToString();
                                if (Mark != "")
                                {
                                    if ((int.Parse(Mark.ToString())) > MaximumMark)
                                    {
                                        valid = false;
                                        _msg = "Please enter the " + studentname + " his " + subjectname + " mark is less then of " + MaximumMark + ".";
                                        break;
                                    }
                                    if (0 > (int.Parse(Mark.ToString())) && (int.Parse(Mark.ToString())) != -1)
                                    {
                                        valid = false;
                                        _msg = "Please enter the " + studentname + " his " + subjectname + " mark is greater then of  0.";
                                        break;
                                    }
                                }
                                else
                                {
                                    valid = false;
                                    _msg = "Please enter the " + studentname + " his " + subjectname + " mark .";
                                    break;
                                }
                               
                            }
                            catch
                            {
                                Mark = dr[i].ToString();
                            }

                        }
                    }
                }
            }
            return valid; 

        }

        private int GetSubjectId(string subjectname,int _ClassId)
        {
            string _correctedSubjectname=subjectname.ToUpperInvariant().Trim().Replace('#','.');
            int _sbijectid = 0;
            string sql = "select tblsubjects.Id as SubjectId,tblsubjects.subject_name as SubjectName from tblsubjects inner JOIN tblclasssubmap ON tblclasssubmap.SubjectId=tblsubjects.Id where  tblclasssubmap.ClassId="+_ClassId;
         
            DataSet Sub = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Sub.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Sub.Tables[0].Rows)
                {
                    if (dr["SubjectName"].ToString().ToUpperInvariant().Trim() == _correctedSubjectname)
                    {
                        _sbijectid = int.Parse(dr["SubjectId"].ToString());
                    }
                }
            }
            return _sbijectid;
        }

        private bool CheckWithThisSubIDandStuIDExitOrNot(int _stuid, int _subid)
        {
            bool valid = false;
            string sql = "select tblcce_mark.Id from tblcce_mark where tblcce_mark.StudentId=" + _stuid + " AND tblcce_mark.SubjectId=" + _subid;
            DataSet Ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Ds.Tables[0].Rows.Count > 0 && Ds != null)
            {
                foreach (DataRow dr in Ds.Tables[0].Rows)
                {
                    valid = true;
                }

            }
            return valid;
        }

        private DataSet prepareDataset_FromExcel(string _physicalpath, out string _msg, out bool _valid)
        {
            _valid = true;
            _msg = "";
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
                    string c = dr["TABLE_NAME"].ToString();//.Replace("$", "").Replace("'", "");
                    if (dr["TABLE_NAME"].ToString().Replace("$", "").Replace("'", "") == Drp_Class.SelectedItem.Text)
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

        private string GetMarkenteryColumnName()
        {
            string _colname = "empty";
            int _Exampid = int.Parse(Drp_Exam.SelectedValue);
            string _Examname = Drp_Exam.SelectedItem.Text;
            string sql = "select tblcce_colconfig.ColName as ColumnName from tblcce_colconfig where tblcce_colconfig.Id=" + _Exampid + " And tblcce_colconfig.ExamName='" + _Examname + "'";
            DataSet _Colunname = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_Colunname != null && _Colunname.Tables[0].Rows.Count > 0)
            {
                if (_Colunname.Tables[0].Rows[0]["ColumnName"].ToString() != "")
                    _colname = _Colunname.Tables[0].Rows[0]["ColumnName"].ToString();
            }
            return _colname;
        }
                                   
        public bool saveTheExcelFile(out string _FileName)
        {
            bool _valid = true;
            _FileName = null;
            try
            {
                _FileName = FileUpload_Excel.FileName.ToString();
                if (File.Exists(MyUser.FilePath + "\\UpImage\\" + _FileName))
                    File.Delete(MyUser.FilePath + "\\UpImage\\" + _FileName);
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

      

    }
}
