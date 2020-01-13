using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;

namespace WinEr
{
    public partial class CCEExamStatusReport : System.Web.UI.Page
    {
        private ExamManage MyExamMang;
        private KnowinUser MyUser;
        private SchoolClass objSchool = null;

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
                    Load_TermDropdown();
                    Load_ClassDropdown();
                    Load_ExamDropdown();
                    Drp_TermSelect.Focus();
                    Lbl_stuTotal.Text = "0";
                    div_result.Visible = false;
                }
            }
        }

        private void Load_ExamDropdown()
        {
            Drp_Exam.Items.Clear();
            string _msg = "";
            bool _Err = false;
            int _Termid = 0;
            int _Classid = 0;
            ListItem li;
            try
            {
                int.TryParse(Drp_TermSelect.SelectedValue.ToString(), out _Termid);
                int.TryParse(Drp_Selectclass.SelectedValue.ToString(), out _Classid);
                if (_Classid != 0)
                {
                    string sql = "SELECT tblcce_colconfig.Id as Id,tblcce_colconfig.ExamName as ExamName,tblcce_colconfig.TableName as TableName,tblcce_colconfig.ColName as ColName from tblcce_colconfig where tblcce_colconfig.TermId=" + _Termid + " AND tblcce_colconfig.GroupId IN (SELECT tblcce_classgroupmap.GroupId from tblcce_classgroupmap where tblcce_classgroupmap.ClassId=" + _Classid + ") AND tblcce_colconfig.TableName='tblcce_result'";
                    DataSet _ExamDs = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (_ExamDs.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drcls in _ExamDs.Tables[0].Rows)
                        {
                            li = new ListItem(drcls["ExamName"].ToString(), drcls["Id"].ToString());
                            Drp_Exam.Items.Add(li);
                        }
                    }
                    else
                    {
                        li = new ListItem("NO Exam Found", "0");
                        Drp_Exam.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("NO Exam Found", "0");
                    Drp_Exam.Items.Add(li);
                }
            }
            catch (Exception ex)
            {
                _msg = ex.Message;
                _Err = true;
            }
            if (_Err)
            {
                WC_MessageBox.ShowMssage(_msg);
                Drp_TermSelect.Focus();
            }
        }

        private void Load_TermDropdown()
        {
            Drp_TermSelect.Items.Clear();
            string sql = "SELECT tblcce_term.Id as Id,tblcce_term.TermName as TermName FROM tblcce_term";
            DataSet Ds_term = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (Ds_term != null && Ds_term.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drcls in Ds_term.Tables[0].Rows)
                {
                    li = new ListItem(drcls["TermName"].ToString(), drcls["Id"].ToString());
                    Drp_TermSelect.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("NO Data Found", "0");
                Drp_TermSelect.Items.Add(li);
            }
        }

        private void Load_ClassDropdown()
        {
            Drp_Selectclass.Items.Clear();
            int _Termid = 0;
            int.TryParse(Drp_TermSelect.SelectedValue.ToString(), out _Termid);
            string sql = "SELECT DISTINCT(tblclass.Id),tblclass.ClassName from tblclass INNER JOIN tblcce_classgroupmap on tblcce_classgroupmap.ClassId=tblclass.Id where tblclass.Status=1 and tblcce_classgroupmap.GroupId IN (SELECT tblcce_colconfig.GroupId from tblcce_colconfig where tblcce_colconfig.TermId=" + _Termid + ")";
            DataSet Ds_class = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (Ds_class != null && Ds_class.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drcls in Ds_class.Tables[0].Rows)
                {
                    li = new ListItem(drcls["ClassName"].ToString(), drcls["Id"].ToString());
                    Drp_Selectclass.Items.Add(li);
                }

            }
            else
            {
                li = new ListItem("NO Class Found", "0");
                Drp_Selectclass.Items.Add(li);
            }
        }

        protected void Btn_click_Click(object sender, EventArgs e)
        {
            Lbl_stuTotal.Text = "0";
            grdResult.DataSource = null;
            string _Err = "";
            string sql1 = "", sql2 = "", sql3 = "", _Subclumn = "";
            try
            {
                if (Validation(out _Err))
                {
                    sql1 = "SELECT tblstudent.Id,tblstudentclassmap.RollNo,tblstudent.StudentName from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + int.Parse(Drp_Selectclass.SelectedValue.ToString()) + " ORDER BY tblstudentclassmap.RollNo";
                    DataSet _StuDs = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql1);

                    sql2 = "SELECT tblsubjects.Id,tblsubjects.subject_name  from tblcce_classsubject inner JOIN tblsubjects on tblcce_classsubject.subjectid=tblsubjects.Id where tblcce_classsubject.classid=" + int.Parse(Drp_Selectclass.SelectedValue.ToString()) + " ORDER by tblcce_classsubject.SubjectOrder";
                    DataSet _SubDs = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql2);

                    sql3 = "SELECT tblcce_colconfig.TableName as TableName,tblcce_colconfig.ColName as ColName from tblcce_colconfig where tblcce_colconfig.Id=" + int.Parse(Drp_Exam.SelectedValue.ToString());
                    DataSet _Column = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql3);

             

                    #region student mark dataset

                    if (_SubDs.Tables[0].Rows.Count == 0 && _SubDs == null)
                        _Err = "Selected class subjects is not configured!";
                    else if (_StuDs.Tables[0].Rows.Count == 0 && _StuDs == null)
                        _Err = "Result is not published!";
                    else
                    {
                        int _SubId = 0, _StuId = 0;
                        string sql = "";

                        #region add subject column
                        foreach (DataRow tc in _SubDs.Tables[0].Rows)
                        {
                            _StuDs.Tables[0].Columns.Add(tc["subject_name"].ToString(), typeof(string));
                        }
                        #endregion

                        foreach (DataRow tr in _Column.Tables[0].Rows)
                        {
                            _Subclumn = tr["TableName"].ToString() + "." + tr["ColName"].ToString();
                        }

                        int i = 0, j = 0;
                        int _stucount = _StuDs.Tables[0].Rows.Count;
                        foreach (DataRow tr1 in _StuDs.Tables[0].Rows)
                        {
                            j = 3;
                            int.TryParse(tr1["Id"].ToString(), out _StuId);
                            int _Subcount = _SubDs.Tables[0].Rows.Count;
                            foreach (DataRow tr2 in _SubDs.Tables[0].Rows)
                            {
                                int.TryParse(tr2["Id"].ToString(), out _SubId);
                                string Submark = "NULL";
                                sql = "SELECT " + _Subclumn + " from tblcce_result WHERE tblcce_result.StudentId=" + _StuId + " and tblcce_result.SubjectId=" + _SubId;
                                DataSet _MarkDs = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                                if (_MarkDs.Tables[0].Rows.Count > 0 && _MarkDs != null)
                                {
                                    Submark = _MarkDs.Tables[0].Rows[0][0].ToString();
                                    _StuDs.Tables[0].Rows[i][j] = Submark;
                                    _Subcount--;
                                }
                                j++;
                            }
                            if (_Subcount > 0)
                            {
                                _StuDs.Tables[0].Rows[i].Delete();
                                _stucount--;
                            }

                            i++;
                        }
                        if (_stucount == 0)
                            _StuDs = null;

                        if (_StuDs != null)
                        {
                            if (_StuDs.Tables[0].Rows.Count > 0)
                            {
                                grdResult.DataSource = _StuDs.Tables[0];
                                grdResult.DataBind();
                                Lbl_stuTotal.Text = _StuDs.Tables[0].Rows.Count.ToString();
                                ViewState["Marks"] = _StuDs;
                                div_result.Visible = true;
                            }

                        }
                        else
                        {
                            ViewState["Marks"] = null;
                            div_result.Visible = false;
                            WC_MessageBox.ShowMssage("Selected class students exam mark is not published!");
                        }
                     
                           
                        
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                _Err = ex.Message;
            }
            if (_Err != "")
            {
                div_result.Visible = false;
                WC_MessageBox.ShowMssage(_Err);
            }
        }

        private bool Validation(out string _Err)
        {
            bool valid = true;
            _Err = "";
            if (int.Parse(Drp_TermSelect.SelectedValue.ToString()) == 0)
                _Err = "Term is not found!";
            else if (int.Parse(Drp_Selectclass.SelectedValue.ToString()) == 0)
                _Err = "Class is not found!";
            else if (int.Parse(Drp_Exam.SelectedValue.ToString()) == 0)
                _Err = "Exam is not found!";
            if (_Err != "")
                valid = false;
            return valid;
        }

        protected void Drp_TermSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_ClassDropdown();
            Load_ExamDropdown();
            div_result.Visible = false;
        }

        protected void Drp_Selectclass_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_ExamDropdown();
            div_result.Visible = false;
        }

        protected void img_export_Excel_Click(object sender, EventArgs e)
        {
            string examname = Drp_Exam.SelectedItem.Text.ToString();
            string filename = examname + " Mark Sheet.xls";
            if (ViewState["Marks"] != null)
            {
                DataSet MyExamData = (DataSet)ViewState["Marks"];
                if (MyExamData.Tables.Count > 0)
                {
                    string FileName = examname + " Mark Sheet";
                    string _ReportName = examname + " Mark Sheet";
                    if (!WinEr.ExcelUtility.ExportDataToExcel(MyExamData, _ReportName, FileName, MyUser.ExcelHeader))
                    {
                        WC_MessageBox.ShowMssage("This function need Ms office");
                    }
                }
            }
        }
    }
}
        
    

