using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.OleDb;

namespace WinEr
{
    public partial class ImportCCEDescriptive : System.Web.UI.Page
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
                    load_DefaultDetails();
                    Drp_Class.Focus();

                }
            }

        }

        private void load_DefaultDetails()
        {
            Load_ClassDroupdown();
            Load_PartDroupdown();
            Load_SubjectDroupdown();
            Load_SubjectSkillDroupdown();
        }

        private void Load_ClassDroupdown()
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

        private void Load_PartDroupdown()
        {
            Drp_part.Items.Clear();
            string sql = "SELECT DISTINCT tblcce_parts.Id,tblcce_parts.Description from tblcce_parts inner JOIN tblcce_subjectskillmap ON tblcce_subjectskillmap.PartId=tblcce_parts.Id ORDER BY tblcce_parts.Id";
            DataSet Ds_part = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (Ds_part != null && Ds_part.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drcls in Ds_part.Tables[0].Rows)
                {
                    li = new ListItem(drcls["Description"].ToString(), drcls["Id"].ToString());
                    Drp_part.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("NO Data Found", "0");
                Drp_part.Items.Add(li);
            }

        }

        private void Load_SubjectDroupdown()
        {
            string sql = "";
            Drp_Subject.Items.Clear();
            int _ClassId = int.Parse(Drp_Class.SelectedValue);
            int _PartId = int.Parse(Drp_part.SelectedValue);
            ListItem li;
            if (_ClassId != 0)
            {
                sql = "select DISTINCT tblsubjects.Id as SubjectId,tblsubjects.subject_name as SubjectName from tblsubjects inner JOIN tblcce_subjectskillmap ON tblcce_subjectskillmap.SubjectId=tblsubjects.Id where  tblcce_subjectskillmap.ClassId=" + _ClassId + " and tblcce_subjectskillmap.PartId=" + _PartId + " ORDER BY tblsubjects.Id";
                DataSet _subjectDS = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (_subjectDS.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drcls in _subjectDS.Tables[0].Rows)
                    {
                        li = new ListItem(drcls["SubjectName"].ToString(), drcls["SubjectId"].ToString());
                        Drp_Subject.Items.Add(li);
                    }
                    validationrow.Visible = true;

                }
                else
                {
                    li = new ListItem("NO Data Found", "0");
                    Drp_Subject.Items.Add(li);
                    validationrow.Visible = false;


                }

            }
        }

        private DataSet Load_SubjectSkillDroupdown()
        {
            string sql = "";
            int _ClassId = int.Parse(Drp_Class.SelectedValue);
            int _PartId = int.Parse(Drp_part.SelectedValue);
            int _SubjectId = int.Parse(Drp_Subject.SelectedValue);
            sql = "select DISTINCT tblcce_subjectskills.Id as skillsId,tblcce_subjectskills.SkillName as SkillName from tblcce_subjectskills inner JOIN tblcce_subjectskillmap ON tblcce_subjectskillmap.SkillId=tblcce_subjectskills.Id where  tblcce_subjectskillmap.ClassId=" + _ClassId + " and tblcce_subjectskillmap.PartId=" + _PartId + " AND tblcce_subjectskillmap.SubjectId=" + _SubjectId + " ORDER BY tblcce_subjectskills.Id";
            DataSet _subjectDS = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return _subjectDS;
        }

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {

            Load_PartDroupdown();
            Load_SubjectDroupdown();
            Load_SubjectSkillDroupdown();
        }

        protected void Drp_part_SelectedIndexChanged(object sender, EventArgs e)
        {
 
            Load_SubjectDroupdown();
            Load_SubjectSkillDroupdown();
        }

        protected void Drp_Subject_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_SubjectSkillDroupdown();
        }

        protected void Drp_skill_SelectedIndexChanged(object sender, EventArgs e)
        {
 
        }

        protected void Btn_Template_Click(object sender, EventArgs e)
        {
            string _msg="";
            int _ClassId = int.Parse(Drp_Class.SelectedValue);
            int _partId = int.Parse(Drp_part.SelectedValue);
            int _SubId = int.Parse(Drp_Subject.SelectedValue);
            if (_ClassId == 0)
                _msg = "Class is not found!";
            else if (_partId == 0)
                _msg = "Part is not found!";
            else if (_SubId == 0)
                _msg = "Subject is not found!";
            else
            {
                try
                {
                    DataSet ExamDataSet = new DataSet();
                    DataTable Examdatatbl = new DataTable("Exam");
                    Examdatatbl.Columns.Add("StudentId", typeof(int));
                    Examdatatbl.Columns.Add("StudentName", typeof(string));
                    Examdatatbl.Columns.Add("StudentRollNo", typeof(int));
                   
                    DataSet SkillDs = Load_SubjectSkillDroupdown();
                    if (SkillDs.Tables[0].Rows.Count > 0)
                    {
                        string Skillname="",Discriptive="",Term1="",Term2="",Term3="";
                        foreach (DataRow dr in SkillDs.Tables[0].Rows)
                        {
                            Skillname = dr[1].ToString();
                            Discriptive=dr[0].ToString() + "D$"+Skillname;
                            Term1 =dr[0].ToString()+"T1$"+Skillname;
                            Term2 =dr[0].ToString() +"T2$"+Skillname;
                            Term3 =dr[0].ToString() +"T3$"+Skillname;
                            Examdatatbl.Columns.Add(Discriptive, typeof(string));
                            Examdatatbl.Columns.Add(Term1, typeof(string));
                            Examdatatbl.Columns.Add(Term2, typeof(string));
                            Examdatatbl.Columns.Add(Term3, typeof(string));
                        }
                    }
                 
                    string sql = "SELECT tblstudent.Id as StudentId,tblstudent.StudentName as StudentName,tblstudentclassmap.RollNo as StudentRollNo from tblstudent INNER JOIN tblstudentclassmap on  tblstudentclassmap.StudentId=tblstudent.Id where tblstudentclassmap.ClassId=" + _ClassId + " AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " ORDER by tblstudentclassmap.RollNo";
                    DataSet _StudentDs = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (_StudentDs.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _StudentDs.Tables[0].Rows)
                        {
                            Examdatatbl.Rows.Add(int.Parse(dr["StudentId"].ToString()), dr["StudentName"].ToString(), int.Parse(dr["StudentRollNo"].ToString()));
                        }
                    }

                    ExamDataSet.Tables.Add(Examdatatbl);
                    if (ExamDataSet.Tables[0].Rows.Count > 0)
                    {
                        string FileName = Drp_Class.SelectedItem.Text+".xls";
                        if (!WinEr.ExcelUtility.ExportDataSetToExcel(ExamDataSet, FileName))
                        {

                        }
                    }

                    
                }
                catch(Exception ex)
                {
                    WC_MessageBox.ShowMssage(ex.Message);
                }
            }

            if (_msg != "")
            {
                Label1.Visible = true;
                Label1.Text = _msg;
            }
            else
            {
                Label1.Visible = false;
            }

            
        }

        protected void Btn_upload_Click(object sender, EventArgs e)
        {
            CLogging logger = CLogging.GetLogObject();
            string sql = "";
            string _msg = "";
            DataSet MydataSet = null;
            string message="", _FileName,_stuname="";
            string _descriptive = "", _term1 = "", _term2 = "", _term3 = "";

            int _StuId = 0;
            int _ClassId = int.Parse(Drp_Class.SelectedValue);
            int _SubjectId = int.Parse(Drp_Subject.SelectedValue);
            int _PartId = int.Parse(Drp_part.SelectedValue);
            int _SkillId = 0;
             if (_ClassId == 0)
                 message = "Class is not found!";
             else if (_PartId == 0)
                 message = "Part is not found!";
             else if (_SubjectId == 0)
                 message = "Subject is not found!";
             else
             {
                 try
                 {
                     if (Check_validity_ToUpload(out message))
                     {
                         if (saveTheExcelFile(out _FileName))
                         {
                             DataSet SkillDs = Load_SubjectSkillDroupdown();
                             if (SkillDs.Tables[0].Rows.Count > 0)
                             {
                                 bool _valid = true;
                                 string _physicalpath = MyUser.FilePath + "\\UpImage\\" + _FileName;
                                 MydataSet = prepareDataset_FromExcel(_physicalpath, out _msg, out _valid);      //prepare dataset from the excel

                                 foreach (DataColumn dc in MydataSet.Tables[0].Columns)
                                 {
                                     string[] colname=dc.ColumnName.ToString().Split('$');
                                     dc.ColumnName = colname[0].ToLower();
                                    
                                 }

                                 string Col1="",Col2="",Col3="",Col4="";

                                 #region import data

                                 if (_valid)
                                 {
                                     foreach (DataRow dr in MydataSet.Tables[0].Rows)
                                     {
                                         _StuId = int.Parse(dr["StudentId"].ToString());
                                         _stuname = dr["StudentName"].ToString();
                                         foreach (DataRow skilldr in SkillDs.Tables[0].Rows)
                                         {
                                             int.TryParse(skilldr["skillsId"].ToString(),out _SkillId);

                                             Col1 = _SkillId + "d"; ;
                                             _descriptive = dr[Col1].ToString();
                                             Col2 = _SkillId + "t1";
                                             _term1 = dr[Col2].ToString();
                                             Col3 = _SkillId + "t2";
                                             _term2 = dr[Col3].ToString();
                                             Col4 = _SkillId + "t3";
                                             _term3 = dr[Col4].ToString();

                                             if (!CheckWithThisSubIDandStuIDExitOrNot(_StuId, _SubjectId, _SkillId, _PartId))
                                             {
                                                 sql = "INSERT into tblcce_descriptive (StudentId,SubjectId,SkillId,PartId,DescriptiveIndicator,Term1,Term2,Term3) VALUES (" + _StuId + "," + _SubjectId + "," + _SkillId + "," + _PartId + ",'" + _descriptive + "','" + _term1.ToUpper() + "','" + _term2.ToUpper() + "','" + _term3.ToUpper() + "')";
                                                 logger.LogToFile("Descriptive Entry", _stuname + " descriptive inserted", 'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, MyUser.UserName);
                                                 MyUser.m_DbLog.LogToDb(MyUser.UserName, "Descriptive Entry", _stuname + " descriptive inserted", 1);

                                             }
                                             else
                                             {
                                                 if (_SkillId == 100)
                                                     sql = "UPDATE tblcce_descriptive SET tblcce_descriptive.DescriptiveIndicator='" + _descriptive + "',tblcce_descriptive.Term1='" + _term1 + "',tblcce_descriptive.Term2='" + _term2 + "',tblcce_descriptive.Term3='" + _term3 + "' WHERE tblcce_descriptive.StudentId=" + _StuId + " AND tblcce_descriptive.SubjectId=" + _SubjectId + " and tblcce_descriptive.SkillId=" + _SkillId + " and tblcce_descriptive.PartId=" + _PartId;
                                                 else
                                                     sql = "UPDATE tblcce_descriptive SET tblcce_descriptive.DescriptiveIndicator='" + _descriptive + "',tblcce_descriptive.Term1='" + _term1.ToUpper() + "',tblcce_descriptive.Term2='" + _term2.ToUpper() + "',tblcce_descriptive.Term3='" + _term3.ToUpper() + "' WHERE tblcce_descriptive.StudentId=" + _StuId + " AND tblcce_descriptive.SubjectId=" + _SubjectId + " and tblcce_descriptive.SkillId=" + _SkillId + " and tblcce_descriptive.PartId=" + _PartId;
                                                 logger.LogToFile("Descriptive Entry", _stuname + " descriptive updated", 'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, MyUser.UserName);
                                                 MyUser.m_DbLog.LogToDb(MyUser.UserName, "Descriptive Entry", _stuname + " descriptive updated", 1);

                                             }
                                             MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                                         }
                                     }
                                 }
                                 else
                                 {
                                     message = _msg;
                                 }

                                 #endregion

                                 File.Delete(MyUser.FilePath + "\\UpImage\\" + _FileName); //delete the file
                                 message = Drp_Class.SelectedItem.Text + " CCE grade uploaded sucessfully";
                             }
                             else
                                 message = "Select Subject Skill not found!";
                         }
                         else
                             message = "Not able to Upload the Excel File. Try again later";
                     }

                 }
                 catch(Exception ex)
                 {
                     WC_MessageBox.ShowMssage(ex.Message);
                     logger.LogToFile("Descriptive Entry", "throws Error" + ex.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, MyUser.UserName);
                     MyUser.m_DbLog.LogToDb(MyUser.UserName, "Descriptive Entry", "throws Error" + ex.Message, 1);

                 }
             }


             if (message != "")
             {
                 Label1.Visible = true;
                 Label1.Text = message;
             }
             else
             {
                 Label1.Visible = false;
             }
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

        private bool saveTheExcelFile(out string _FileName)
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

        private bool CheckWithThisSubIDandStuIDExitOrNot(int _stuid, int _subjectid, int _skillid, int _partid)
        {
            bool valid = false;
            string sql = "SELECT tblcce_descriptive.Id,tblcce_descriptive.DescriptiveIndicator from tblcce_descriptive WHERE tblcce_descriptive.StudentId=" + _stuid + " AND tblcce_descriptive.SubjectId=" + _subjectid + " and tblcce_descriptive.SkillId=" + _skillid + " and tblcce_descriptive.PartId=" + _partid;
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

    }
}
