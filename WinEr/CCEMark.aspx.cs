using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Data.Odbc;

namespace WinEr
{
   
    public partial class CCEMark : System.Web.UI.Page
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
                    GridViewtable.Visible = false;
                    load_DefaultValues();

                }
            }

        }

        /// <summary>
        /// this function loading all default page event with values
        /// </summary>

        private void load_DefaultValues()
        {
            Load_ClassDropDown();
            Load_SubjectDropDown();
            bool valid = true;
            Load_ExamDropDown(out valid);
            if (valid)
            {
                Load_ExamMaxMark();
                load_Grid();
            }
            else
            {
                Txt_Maxmark.Text = "";
                GridViewtable.Visible = false;
                Grd_CCEMark.DataSource = null;
                Grd_CCEMark.DataBind();
                Drp_Class.Focus();
            }
        }

        /// <summary>
        /// this function return max mark to textbox
        /// </summary>
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
            }
        }

        /// <summary>
        /// this function loding subject dropdown value
        /// </summary>
        private void Load_SubjectDropDown()
        {
            string sql = "";
            Drp_subject.Items.Clear();
            int _ClassId = int.Parse(Drp_Class.SelectedValue);
            ListItem li;
            if (_ClassId != 0)
            {
                sql = "select tblsubjects.Id as SubjectId,tblsubjects.subject_name as SubjectName from tblsubjects inner JOIN tblcce_classsubject ON tblcce_classsubject.subjectid=tblsubjects.Id where  tblcce_classsubject.classid="+_ClassId;
                DataSet _subjectDS = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (_subjectDS.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drcls in _subjectDS.Tables[0].Rows)
                    {
                        li = new ListItem(drcls["SubjectName"].ToString(), drcls["SubjectId"].ToString());
                        Drp_subject.Items.Add(li);
                    }

                }
                else
                {
                    li = new ListItem("NO Data Found", "0");
                    Drp_subject.Items.Add(li);
                    
                }

            }
           
        }
  
        /// <summary>
        /// this function loaing exam dropdown value
        /// </summary>
        /// <param name="valid"></param>
        private void Load_ExamDropDown(out bool valid)
        {
            valid = true;
            Drp_Exam.Items.Clear();
            int _ClassId=int.Parse(Drp_Class.SelectedValue);
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
            }
            else
            {
                li = new ListItem("NO Data Found", "0");
                Drp_Exam.Items.Add(li);
                valid = false;
            }
           
        }

        /// <summary>
        /// this function loading class dropdown values
        /// </summary>
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
            bool _valid = true;
            Load_ExamDropDown(out _valid);
            Load_SubjectDropDown();
            if (_valid)
            {
                Load_ExamMaxMark();
                load_Grid();
            }
            else
            {
                Txt_Maxmark.Text = "";
                GridViewtable.Visible = false;
                Grd_CCEMark.DataSource = null;
                Grd_CCEMark.DataBind();
            }
        }
      
        protected void Drp_Exam_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_ExamMaxMark();
            load_Grid();
        }

        protected void Drp_subject_SelectedIndexChanged(object sender, EventArgs e)
        {
                load_Grid();
        }
       
        /// <summary>
        /// clearing all selected subjected mark on the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Clear_Click(object sender, EventArgs e)
        {
            if (Grd_CCEMark.Rows.Count > 0)
            {
                load_Grid();
                Drp_Class.Focus();
            }
        }

        /// <summary>
        /// while saving mark it will return  colum name.
        /// </summary>
        /// <param name="_columname"></param>
        /// <returns></returns>
        private bool GetMarkenteryColumnName(out string _columname)
        {
            bool valid = false;
            _columname = "Col1";
            int _Exampid = int.Parse(Drp_Exam.SelectedValue);
            string _Examname = Drp_Exam.SelectedItem.Text;
            string sql = "select tblcce_colconfig.ColName as ColumnName from tblcce_colconfig where tblcce_colconfig.Id=" + _Exampid + " And tblcce_colconfig.ExamName='" + _Examname + "'";
            DataSet _Colunname = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_Colunname != null && _Colunname.Tables[0].Rows.Count > 0)
            {
                if (_Colunname.Tables[0].Rows[0]["ColumnName"].ToString() != "")
                {
                    _columname = _Colunname.Tables[0].Rows[0]["ColumnName"].ToString();
                    valid = true;
                }

            }
            return valid;
        }

        /// <summary>
        /// while saving marks it will check with allready selected exam subject mark updated or not
        /// if updated means those subect mark updated from tblcce_mark table.
        /// otherwise marks will inseted from tblcce_mark table.
        /// </summary>
        /// <param name="_stuid"></param>
        /// <param name="_subid"></param>
        /// <returns></returns>
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

        /// <summary>
        /// it will return subject id
        /// </summary>
        /// <param name="_subname"></param>
        /// <returns></returns>
        private int GetSubjectId(string _subname)
        {
            int _subid = 0;
            int _ClassId = int.Parse(Drp_Class.SelectedValue);
            string sql = "select tblsubjects.Id as SubjectId,tblsubjects.subject_name as SubjectName from tblsubjects inner JOIN tblclasssubmap ON tblclasssubmap.SubjectId=tblsubjects.Id where  tblclasssubmap.ClassId=" + _ClassId;
            DataSet Sub = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Sub.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Sub.Tables[0].Rows)
                {
                    if (dr["SubjectName"].ToString() == _subname)
                        _subid = int.Parse(dr["SubjectId"].ToString());
                }
            }
            return _subid;
        }

        /// <summary>
        /// this function load with selected class student mark shown on the grid
        /// </summary>
        private void load_Grid()
        {
            Label1.Visible = false;
            string _Err = "";

            string Markcolumnname=Drp_subject.SelectedItem.Text;
            Grd_CCEMark.Columns[Grd_CCEMark.Columns.Count - 1].HeaderText = Markcolumnname+" Mark";
            int _ClassId = int.Parse(Drp_Class.SelectedValue);
            int _SubjectId=int.Parse(Drp_subject.SelectedValue);

            string _ColumnName = "";
            if (GetMarkenteryColumnName(out _ColumnName))
            {
                #region load student details from _datatable

                string sql = "SELECT tblstudent.Id as StudentId,tblstudent.StudentName as StudentName,tblstudentclassmap.RollNo as StudentRollNo from tblstudent INNER JOIN tblstudentclassmap on  tblstudentclassmap.StudentId=tblstudent.Id where tblstudentclassmap.ClassId=" + _ClassId + " AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " ORDER by tblstudentclassmap.RollNo";
                DataSet _StudentDs = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (_StudentDs.Tables[0].Rows.Count > 0)
                {
                    GridViewtable.Visible = true;
                    Grd_CCEMark.Columns[0].Visible = true;
                    Grd_CCEMark.DataSource = _StudentDs;
                    Grd_CCEMark.DataBind();

                    #region add Mark
                    double _grandTotal = 0;
                    int _rowindex = 0;
                    sql = "SELECT tblstudent.Id as StudentId,tblstudentclassmap.RollNo as StudentRollNo,tblcce_mark." + _ColumnName + " as Mark from tblstudent INNER JOIN tblstudentclassmap on  tblstudentclassmap.StudentId=tblstudent.Id INNER JOIN tblcce_mark ON tblcce_mark.StudentId=tblstudentclassmap.StudentId where tblstudentclassmap.ClassId=" + _ClassId + " AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + "  AND tblcce_mark.SubjectId=" + _SubjectId + " ORDER by tblstudentclassmap.RollNo";
                    DataSet Ds_mark = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (Ds_mark.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in Ds_mark.Tables[0].Rows)
                        {
                            if (dr["StudentId"].ToString() == Grd_CCEMark.Rows[_rowindex].Cells[0].Text.ToString())
                            {
                                TextBox tb = (TextBox)Grd_CCEMark.Rows[_rowindex].Cells[3].FindControl("Txt_Mark");
                                tb.Text = dr["Mark"].ToString();
                                double _mark=0;
                                if (double.TryParse(tb.Text, out _mark))
                                {
                                    _grandTotal += _mark;
                                }
                                _rowindex++;

                            }
                        }
                    }

                    #endregion

                    Grd_CCEMark.Columns[0].Visible = false;
                }
                else
                {
                    GridViewtable.Visible = false;
                    Grd_CCEMark.DataSource = null;
                    Grd_CCEMark.DataBind();
                    _Err="Student marks is not impoerted!.Please have to enter students mark. ";
                }

                #endregion
            }
            else
                _Err = Drp_Exam.SelectedItem.Text + " mark entry colum is not configured. Please configure mark entry column";

            if (_Err != "")
            {
                Label1.Visible = true;
                Label1.Text = _Err;
            }

        }

        /// <summary>
        /// afer clicking this event save student mark from tblcce_mark table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            Label1.Visible =false;
            CLogging logger = CLogging.GetLogObject();
            string _msg = "",_Examname = Drp_Exam.SelectedItem.Text,_columname = "", sql = "", _stuname = "",_mark = "-1";
            int _stuid = 0, _subid = 0;
            bool _continue = true;

            ViewState["classid"] = Drp_Class.SelectedValue.ToString();
            ViewState["examname"] = _Examname;
            try
            {
                if (!GetMarkenteryColumnName(out _columname))
                    _msg = "Mark entry column configuration not done please do";
                else
                {
                    #region student mark inserting
                    for (int i = 0; i < Grd_CCEMark.Rows.Count; i++)
                    {
                        GridViewRow _row = Grd_CCEMark.Rows[i];
                        for (int j = 2; j < _row.Cells.Count; j++)
                        {
                            _stuid = int.Parse(_row.Cells[0].Text.ToString());
                            _subid = int.Parse(Drp_subject.SelectedValue);
                            _stuname = _row.Cells[1].Text.ToString();

                            TextBox tb1 = (TextBox)_row.Cells[2].FindControl("Txt_Mark");
                            _mark = tb1.Text;
                            if (_mark == "")
                            {
                                _continue = false;
                                break;
                            }

                            if (_continue)
                            {
                                if (!CheckWithThisSubIDandStuIDExitOrNot(_stuid, _subid))
                                {
                                    sql = "INSERT into tblcce_mark (tblcce_mark.StudentId,tblcce_mark.SubjectId,tblcce_mark." + _columname + ") VALUES (" + _stuid + "," + _subid + ",'" + _mark + "')";
                                    logger.LogToFile(" Import Marks ", _stuname + " " + Drp_subject.SelectedItem.Text.ToString() + " mark imported", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, MyUser.LoginUserName);
                                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Import Marks ", _stuname + " " + Drp_subject.SelectedItem.Text.ToString() + " mark imported", 1);
                                    MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                                }
                                else
                                {
                                    sql = "UPDATE tblcce_mark SET tblcce_mark." + _columname + "='" + _mark + "' where tblcce_mark.StudentId=" + _stuid + " and tblcce_mark.SubjectId=" + _subid;
                                    logger.LogToFile(" Update Marks ", _stuname + " " + Drp_subject.SelectedItem.Text.ToString() + " mark updated ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, MyUser.LoginUserName);
                                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Update Marks  ", _stuname + " " + Drp_subject.SelectedItem.Text.ToString() + " mark updated", 1);
                                    MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                                }
                            }
                        }
                    }
                    #endregion
                }
                if (!_continue)
                    _msg = "Some students mark is not eneterd please do";
                else
                {
                    load_DefaultValues();
                    _msg = "Student Mark Updateded Sucessfully";
                }
            }
            catch (Exception ex)
            {
                logger.LogToFile("Mark Entry", "throws Error" + ex.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, MyUser.LoginUserName);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Mark Entry", "throws Error" + ex.Message, 1);
                WC_MessageBox.ShowMssage("Student Mark not Updateded Sucessfully!" + ex.Message);
            }

            if (_msg !="")
            {
                Label1.Visible = true;
                Label1.Text = _msg;
            }

            if (checkpublishstatus())
            {
                MPE_yesornoMessageBox.Show();
            }
           
        }

        private bool checkpublishstatus()
        {
            bool valid=false;
            string sql = "SELECT tblcce_configmanager.reportsstatus from tblcce_configmanager inner join tblcce_colconfig on tblcce_colconfig.GroupId=tblcce_configmanager.GroupId where tblcce_configmanager.Classid=" + int.Parse(ViewState["classid"].ToString()) + " and tblcce_colconfig.ExamName='" + ViewState["examname"].ToString() + "'";
            DataSet ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() == "2")
                    valid = true;
                
            }
            return valid;
        }

        protected void Btn_yes_Click(object sender, EventArgs e)
        {
            string sql = "update tblcce_configmanager inner join tblcce_colconfig on tblcce_colconfig.GroupId=tblcce_configmanager.GroupId set tblcce_configmanager.reportsstatus=1,tblcce_configmanager.publishstatus=0 where tblcce_configmanager.Classid=" + int.Parse(ViewState["classid"].ToString()) + " and tblcce_colconfig.ExamName='" + ViewState["examname"].ToString() + "'";
            MyUser.m_MysqlDb.ExecuteQuery(sql);
        }

        protected void Grd_CCEMark_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           //bv
        }

        /// <summary>
        /// after clicking this event control will go to import cce mark page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Lnk_importmark_Click(object sender, EventArgs e)
        {
            Response.Redirect("ImportCCEMark.aspx");
        }

        /// <summary>
        /// export student mark entry excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void img_export_Excel_Click(object sender, ImageClickEventArgs e)
        {
            string Err = "";
            try
            {
                Label1.Visible = false;
                int _ClassId = int.Parse(Drp_Class.SelectedValue);
                int _ExamID = int.Parse(Drp_Exam.SelectedValue);
                int _GroupID = 0;
                double _ExamMaxMark = 0.0;
                DataTable gradmaster = null;
                if (_ClassId == 0)
                    Err = "Class name is not Found!";
                if (_ExamID == 0)
                    Err = "Exam name is not found!";
                else
                {
                    string sql = "";
                    DataSet ExamDataSet = new DataSet();
                    DataTable Examdatatbl = new DataTable("Exam");
                    Examdatatbl.Columns.Add("StudentId", typeof(int));
                    Examdatatbl.Columns.Add("StudentRollNo", typeof(int));
                    Examdatatbl.Columns.Add("StudentName", typeof(string));
                   

                    #region Subject

                    sql = "select tblsubjects.Id as SubjectId,tblsubjects.subject_name as SubjectName from tblsubjects inner JOIN tblcce_classsubject ON tblcce_classsubject.subjectid=tblsubjects.Id where  tblcce_classsubject.classid=" + _ClassId+" order by tblcce_classsubject.classid";
                    DataSet _Subject = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (_Subject.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr_sub in _Subject.Tables[0].Rows)
                        {
                            Examdatatbl.Columns.Add(dr_sub["SubjectName"].ToString(), typeof(string));
                            Examdatatbl.Columns.Add(dr_sub["SubjectName"].ToString()+" Grade", typeof(string));
                        }
                    }

                    #endregion

                    sql = "SELECT tblstudent.Id as StudentId,tblstudentclassmap.RollNo as StudentRollNo,tblstudent.StudentName as StudentName from tblstudent INNER JOIN tblstudentclassmap on  tblstudentclassmap.StudentId=tblstudent.Id where tblstudentclassmap.ClassId=" + _ClassId + " AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " ORDER by tblstudent.StudentName";
                    DataSet _StudentDs = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (_StudentDs.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _StudentDs.Tables[0].Rows)
                        {
                            Examdatatbl.Rows.Add(int.Parse(dr["StudentId"].ToString()), int.Parse(dr["StudentRollNo"].ToString()), dr["StudentName"].ToString(), "");
                        }
                    }
                    ExamDataSet.Tables.Add(Examdatatbl);

                    sql = "SELECT tblcce_colconfig.TableName,tblcce_colconfig.ColName,tblcce_colconfig.GroupId,tblcce_colconfig.ExamMaxMark from tblcce_colconfig INNER JOIN tblcce_classgroupmap on tblcce_classgroupmap.GroupId=tblcce_colconfig.GroupId where tblcce_classgroupmap.ClassId=" + _ClassId + " and tblcce_colconfig.Id=" + _ExamID;
                    DataSet Columnds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (Columnds.Tables[0].Rows.Count > 0)
                    {
                        string columnname = "";
                        string tablename="";
                        foreach (DataRow dr in Columnds.Tables[0].Rows)
                        {
                            tablename = dr[0].ToString();
                            columnname = dr[1].ToString();
                            int.TryParse(dr[2].ToString(), out _GroupID);
                            double.TryParse(dr[3].ToString(), out _ExamMaxMark);
                        }
                        gradmaster = new DataTable("GradeMaster");
                        gradmaster = GetGradeTable(_GroupID);
                        int Row = 0;
                        foreach (DataRow dr1 in ExamDataSet.Tables[0].Rows)
                        {
                            int column = 3;
                            int sudentid = 0;
                            int.TryParse(dr1[0].ToString(), out sudentid);
                            foreach (DataRow dr2 in _Subject.Tables[0].Rows)
                            {
                                int subid = 0;
                                string submark = "0";
                                int.TryParse(dr2[0].ToString(), out subid);
                                sql = "SELECT " + tablename + "." + columnname + " from " + tablename + " inner join tblstudentclassmap ON tblstudentclassmap.StudentId=tblcce_mark.StudentId where tblstudentclassmap.ClassId=" + _ClassId + " AND tblcce_mark.SubjectId=" + subid + " AND tblcce_mark.StudentId=" + sudentid;
                                DataSet submarkds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                                if (submarkds.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow _markdr in submarkds.Tables[0].Rows)
                                    {
                                        submark = _markdr[0].ToString();
                                    }
                                }
                                ExamDataSet.Tables[0].Rows[Row][column] = submark;
                                column++;
                                double mark=0.0;
                                double.TryParse(submark, out mark);
                                mark = (mark / _ExamMaxMark) * 100;
                                ExamDataSet.Tables[0].Rows[Row][column] = GetGrade(gradmaster, mark);
                                column++;
                            }
                            Row++;
                        }
                        ExamDataSet.Tables[0].Columns.RemoveAt(0);
                        ExamDataSet.Tables.Add(gradmaster);
 
                    }

                    if (ExamDataSet.Tables[0].Rows.Count > 0)
                    {
                        string FileName = Drp_Class.SelectedItem.Text+"_"+Drp_Exam.SelectedItem.Text+".xls";
                        if (!WinEr.ExcelUtility.ExportDataSetToExcel(ExamDataSet,FileName))
                        {

                        }

                    }

                }
            }
            catch (Exception ex)
            {
               WC_MessageBox.ShowMssage(ex.Message);
            }
            if (Err != "")
            {
                Label1.Visible =true;
                Label1.Text = Err;
            }          
        }

        public DataTable GetGradeTable(int GroupId)
        {
            #region Read Grade Table
            List<OdbcParameter> lstParameter = new List<OdbcParameter>();
            DataTable returndataset=null;
            string query = "select tblGrade.Grade,tblGrade.LowerLimit from tblGrade where tblGrade.GradeMasterId = (select tblcce_classgroup.GradeMaster from tblcce_classgroup where tblcce_classgroup.Id=" + GroupId + ") and tblGrade.Status=1 ORDER BY tblgrade.LowerLimit DESC";
            returndataset = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataTable(query, lstParameter);
            return returndataset;
            #endregion
        }

        public static string GetGrade(DataTable _GradeTable, double TotalMark)
        {
            #region Calculate Grade
            string Grade = "NA";
            foreach (DataRow dr in _GradeTable.Rows)
            {
                if (double.Parse(dr["LowerLimit"].ToString()) <= TotalMark)
                {
                    Grade = dr["Grade"].ToString();
                    break;
                }
            }
            return Grade;
            #endregion
        }

    }

}




