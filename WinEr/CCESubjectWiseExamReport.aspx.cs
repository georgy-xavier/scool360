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
    public partial class CCESubjectWiseExamReport : System.Web.UI.Page
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
                    Load_ExamGrid();
                    Drp_TermSelect.Focus();
                    div3.Visible = false;

                }
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
            string sql = "SELECT tblclass.Id,tblclass.ClassName from tblclass INNER JOIN tblcce_classgroupmap on tblcce_classgroupmap.ClassId=tblclass.Id  where tblclass.Status=1 and tblcce_classgroupmap.GroupId IN (SELECT tblcce_colconfig.GroupId from tblcce_colconfig where tblcce_colconfig.TermId=" + _Termid + ")";
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

        private void Load_ExamGrid()
        {
            string _msg = "";
            bool _Err = false;
            int _Termid = 0;
            int _Classid = 0;
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
                        Grd_CCEExam.Columns[1].Visible = true;
                        Grd_CCEExam.Columns[3].Visible = true;
                        Grd_CCEExam.Columns[4].Visible = true;
                        Grd_CCEExam.DataSource = _ExamDs;
                        Grd_CCEExam.DataBind();
                        Grd_CCEExam.Columns[1].Visible = false;
                        Grd_CCEExam.Columns[3].Visible = false;
                        Grd_CCEExam.Columns[4].Visible = false;
                        Load_SubjectDropdown();
                        div2.Visible = true;
                    }
                    else
                    {
                        div2.Visible = false;
                        Grd_CCEExam.DataSource = null;
                        Grd_CCEExam.DataBind();
                        _Err = true;
                        _msg = "NO Exam Found!";

                    }
                }
                else
                    div2.Visible = false;


               
               
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

        protected void Drp_TermSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_ClassDropdown();
            Load_ExamGrid();
        }

        protected void Drp_Selectclass_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_ExamGrid();
        }

        private void Load_SubjectDropdown()
        {
            Drp_subject.Items.Clear();
            int _SubTypepid = -1,_Classid=0;
            int.TryParse(Drp_subjecttype.SelectedValue.ToString(), out _SubTypepid);
            int.TryParse(Drp_Selectclass.SelectedValue.ToString(), out _Classid);
            string sql = "";
            if (_SubTypepid == 0)
                sql = "SELECT tblsubjects.Id as Id,tblsubjects.subject_name as subjectName from tblsubjects INNER JOIN tblcce_classsubject on tblcce_classsubject.subjectid=tblsubjects.Id where tblcce_classsubject.classid="+_Classid+" order by SubjectOrder;";
            else
                sql = "SELECT tblcce_subjectskills.Id as Id,tblcce_subjectskills.SkillName as subjectName from tblcce_subjectskills INNER JOIN tblcce_subjectskillmap on tblcce_subjectskillmap.SkillId=tblcce_subjectskills.Id where tblcce_subjectskillmap.ClassId=" + _Classid + " order by SkillOrder";
            DataSet Ds_class = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (Ds_class != null && Ds_class.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drcls in Ds_class.Tables[0].Rows)
                {
                    li = new ListItem(drcls["subjectName"].ToString(), drcls["Id"].ToString());
                    Drp_subject.Items.Add(li);
                }

            }
            else
            {
                li = new ListItem("NO Subject Found", "0");
                Drp_subject.Items.Add(li);
            }
        }

        protected void ChkSelect_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chkselect = (CheckBox)Grd_CCEExam.HeaderRow.Cells[0].FindControl("ChkSelect");
            if (_chkselect.Checked)
            {
                for (int i = 0; i < Grd_CCEExam.Rows.Count; i++)
                {
                    CheckBox chkterm = (CheckBox)Grd_CCEExam.Rows[i].Cells[0].FindControl("Chk_temselect");
                    chkterm.Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < Grd_CCEExam.Rows.Count; i++)
                {
                    CheckBox chkterm = (CheckBox)Grd_CCEExam.Rows[i].Cells[0].FindControl("Chk_temselect");
                    chkterm.Checked = false;
                }
            }
        }

        protected void Drp_subjecttype_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Load_SubjectDropdown();
        }

        protected void Btn_Continue_Click(object sender, EventArgs e)
        {
            string _Err = "";
            Lbl_termname.Text = "";
            Lbl_classname.Text ="";
            Lbl_subjectname.Text = "";
            Lbl_stuTotal.Text = "";
            try
            {
                int _SubId = 0, _ClassId = 0, _SubType = -1;
                string _Tblname = "", _Colname = "", _Examname = "";
                string sql = "", _subquery = "";
                grdResult.DataSource = null;
                if (Validation(out _Err))
                {
                    int.TryParse(Drp_subject.SelectedValue.ToString(), out _SubId);
                    int.TryParse(Drp_Selectclass.SelectedValue.ToString(), out _ClassId);
                    int.TryParse(Drp_subjecttype.SelectedValue.ToString(), out _SubType);
                    foreach (GridViewRow gvr in Grd_CCEExam.Rows)
                    {
                        CheckBox chk = (CheckBox)gvr.FindControl("Chk_temselect");
                        if (chk.Checked == true)
                        {
                            _Examname = gvr.Cells[2].Text.ToString();
                            _Tblname = gvr.Cells[3].Text.ToString();
                            _Colname = gvr.Cells[4].Text.ToString();
                            _subquery +=","+ _Tblname + "." + _Colname + " as `" + _Examname+"`";
                        }

                    }
                    if (_SubType == 0)
                        sql = "SELECT tblstudent.RollNo as RollNo,tblstudent.StudentName as StudentName" + _subquery + "   from tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id inner join tblcce_result on tblcce_result.StudentId=tblstudent.Id Where tblstudentclassmap.ClassId=" + _ClassId + " and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblcce_result.SubjectId=" + _SubId + " order by tblstudent.RollNo";
                    else
                        sql = "";
                    DataSet _MarkDs = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (_MarkDs.Tables[0].Rows.Count > 0)
                    {
                        grdResult.DataSource = _MarkDs;
                        grdResult.DataBind();
                        ViewState["Marks"] = _MarkDs;
                        Lbl_termname.Text = Drp_TermSelect.SelectedItem.Text.ToString();
                        Lbl_classname.Text = Drp_Selectclass.SelectedItem.Text.ToString();
                        Lbl_subjectname.Text = Drp_subject.SelectedItem.Text.ToString();
                        Lbl_stuTotal.Text = _MarkDs.Tables[0].Rows.Count.ToString();
                        div3.Visible = true;
                        div1.Visible = false;
                        div2.Visible = false;
                    }
                    else
                    {
                        _Err="Exam result is not publish!";
                        ViewState["Marks"] = null;
                    }
                }
            }
            catch (Exception ex)
            {
                _Err = ex.Message;
            }
            if(_Err!="")
                WC_MessageBox.ShowMssage(_Err);
        }

        private bool Validation(out string _Err)
        {
            bool validate = true;
            _Err = "";
            if (int.Parse(Drp_subject.SelectedValue.ToString()) == 0)
            {
                _Err = "Subject is not found! Please configure class subjects in Exam module.";
                validate = false;
            }
            else if(!chekExamGrid())
            {
                _Err = "Please select exam name on the grid!";
                validate = false;
            }
            return validate;
        }

        private bool chekExamGrid()
        {
            bool valid = false;
            foreach (GridViewRow gvr in Grd_CCEExam.Rows)
            {
                CheckBox chk = (CheckBox)gvr.FindControl("Chk_temselect");
                if (chk.Checked == true)
                {
                    valid = true;
                    break;
                }
            }
            return valid;

        }

        protected void img_export_Excel_Click(object sender, EventArgs e)
        {
            string examname = Drp_subject.SelectedItem.Text.ToString();
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

        protected void Btn_back_Click(object sender, EventArgs e)
        {
            div1.Visible = true;
            div2.Visible = true;
            div3.Visible = false;
            ViewState["Marks"] = null;
            grdResult.DataSource = null;
            
            
        }
    }
}
