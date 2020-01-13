using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Reflection;

namespace WinEr
{
    public partial class CCEDescriptive : System.Web.UI.Page
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
                    Gridview_Table.Visible = false;
                    load_DefaultDetails();
                    Drp_Class.Focus();
                   
                }
            }

        }

        /// <summary>
        /// loading default conrol values 
        /// </summary>
        private void load_DefaultDetails()
        {
           
            try
            {
                Load_ClassDroupdown();
                Load_PartDroupdown();
                bool subvalid = true, subkillvalid = true;
                Load_SubjectDroupdown(out subvalid);
                Load_SubjectSkillDroupdown(out subkillvalid);
                if (subkillvalid && subvalid)
                {
                    Gridview_Table.Visible = true;
                    Load_GridviewDetails();
                }
                else
                {
                    Gridview_Table.Visible = false;
                    Grd_CCEMark.DataSource = null;
                    Grd_CCEMark.DataBind();
                }

            }
            catch (Exception ex)
            {
                WC_MessageBox.ShowMssage(ex.Message);
            }
        }

        /// <summary>
        /// this function loading class dropdown
        /// </summary>
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

        /// <summary>
        /// this function loading part dropdown
        /// </summary>
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

        /// <summary>
        /// this function loading subject dropdown
        /// </summary>
        private void Load_SubjectDroupdown(out bool subvalid)
        {
            subvalid = true;
            string sql = "";
            Drp_subject.Items.Clear();
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
                        Drp_subject.Items.Add(li);
                    }

                }
                else
                {
                    li = new ListItem("NO Data Found", "0");
                    Drp_subject.Items.Add(li);
                    subvalid = false;

                }

            }
        }

        /// <summary>
        /// this function loading subject skill dropdown
        /// </summary>
        private void Load_SubjectSkillDroupdown(out bool subkillvalid)
        {
            subkillvalid = true;
            string sql = "";
            Drp_skill.Items.Clear();
            int _ClassId = int.Parse(Drp_Class.SelectedValue);
            int _PartId = int.Parse(Drp_part.SelectedValue);
            int _SubjectId = int.Parse(Drp_subject.SelectedValue);
            ListItem li;
            if (_ClassId != 0)
            {
                sql = "select DISTINCT tblcce_subjectskills.Id as skillsId,tblcce_subjectskills.SkillName as SkillName from tblcce_subjectskills inner JOIN tblcce_subjectskillmap ON tblcce_subjectskillmap.SkillId=tblcce_subjectskills.Id where  tblcce_subjectskillmap.ClassId=" + _ClassId + " and tblcce_subjectskillmap.PartId=" + _PartId + " AND tblcce_subjectskillmap.SubjectId=" + _SubjectId + " ORDER BY tblcce_subjectskills.Id";
                DataSet _subjectDS = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (_subjectDS.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drcls in _subjectDS.Tables[0].Rows)
                    {
                        li = new ListItem(drcls["SkillName"].ToString(), drcls["skillsId"].ToString());
                        Drp_skill.Items.Add(li);
                    }

                }
                else
                {
                    li = new ListItem("NO Data Found", "0");
                    Drp_skill.Items.Add(li);
                    subkillvalid = false;

                }

            }

        }

        /// <summary>
        /// this function loading class student list on the grod
        /// </summary>
        private void Load_GridviewDetails()
        {
            int _ClassId = int.Parse(Drp_Class.SelectedValue);
            int _SubjectId = int.Parse(Drp_subject.SelectedValue);
            int _subjectkillid=int.Parse(Drp_skill.SelectedValue);

            #region load student details from _datatable

            string sql = "SELECT tblstudent.Id as StudentId,tblstudent.StudentName as StudentName,tblstudentclassmap.RollNo as StudentRollNo from tblstudent INNER JOIN tblstudentclassmap on  tblstudentclassmap.StudentId=tblstudent.Id where tblstudentclassmap.ClassId=" + _ClassId + " AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " ORDER by tblstudentclassmap.RollNo";
            DataSet _StudentDs = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_StudentDs.Tables[0].Rows.Count > 0)
            {
                Grd_CCEMark.Columns[0].Visible = true;
                Grd_CCEMark.DataSource = _StudentDs;
                Grd_CCEMark.DataBind();

                #region add grad and descriptive

                sql = "SELECT tblstudent.Id as StudentId,tblstudentclassmap.RollNo as StudentRollNo,tblcce_descriptive.DescriptiveIndicator,tblcce_descriptive.Term1,tblcce_descriptive.Term2,tblcce_descriptive.Term3 from tblstudent INNER JOIN tblstudentclassmap on  tblstudentclassmap.StudentId=tblstudent.Id INNER JOIN tblcce_descriptive ON tblcce_descriptive.StudentId=tblstudentclassmap.StudentId where tblstudentclassmap.ClassId=" + _ClassId + " AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblcce_descriptive.SubjectId=" + _SubjectId + " and tblcce_descriptive.SkillId=" + _subjectkillid + " ORDER by tblstudentclassmap.RollNo";
                DataSet Ds_descriptive = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if(Ds_descriptive.Tables[0].Rows.Count>0)
                {
                    int _rowindex = 0;
                    string _stuid = "0", _sturollno = "0";
                    string grd_stuid = "0", grd_sturollno = "0";

                    foreach (DataRow dr in Ds_descriptive.Tables[0].Rows)
                    {
                        _stuid = dr["StudentId"].ToString();
                        _sturollno = dr["StudentRollNo"].ToString();
                        grd_stuid=Grd_CCEMark.Rows[_rowindex].Cells[0].Text.ToString();
                        grd_sturollno=Grd_CCEMark.Rows[_rowindex].Cells[2].Text.ToString();
                        if (_stuid==grd_stuid && _sturollno==grd_sturollno)
                        {
                           
                            TextBox tb1 = (TextBox)Grd_CCEMark.Rows[_rowindex].Cells[3].FindControl("Txt_DescriptiveIndicator");
                            tb1.Text = dr["DescriptiveIndicator"].ToString();
                            TextBox tb2 = (TextBox)Grd_CCEMark.Rows[_rowindex].Cells[4].FindControl("Txt_TermOne");
                            tb2.Text = dr["Term1"].ToString();
                            TextBox tb3 = (TextBox)Grd_CCEMark.Rows[_rowindex].Cells[5].FindControl("Txt_TermTwo");
                            tb3.Text = dr["Term2"].ToString();
                            TextBox tb4 = (TextBox)Grd_CCEMark.Rows[_rowindex].Cells[6].FindControl("Txt_TermThree");
                            tb4.Text = dr["Term3"].ToString();
                            _rowindex++;
                        }
                      
                    }
                          
                }

                #endregion
              
                Grd_CCEMark.Columns[0].Visible = false;


            }
            else
            {
                Grd_CCEMark.DataSource = null;
                Grd_CCEMark.DataBind();
                WC_MessageBox.ShowMssage("No Data Found!");
            }

            #endregion

        }

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool subvalid = true,subkillvalid=true;
            Load_SubjectDroupdown(out subvalid);
            Load_SubjectSkillDroupdown(out subkillvalid);
            if (subkillvalid && subvalid)
            {
                Gridview_Table.Visible = true;
                Load_GridviewDetails();
            }
            else
            {
                Gridview_Table.Visible = false;
                Grd_CCEMark.DataSource = null;
                Grd_CCEMark.DataBind();
            }
        }

        protected void Drp_part_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool subvalid = true, subkillvalid = true;
            Load_SubjectDroupdown(out subvalid);
            Load_SubjectSkillDroupdown(out subkillvalid);
            if (subkillvalid && subvalid)
            {
                Gridview_Table.Visible = true;
                Load_GridviewDetails();
            }
            else
            {
                Gridview_Table.Visible = false;
                Grd_CCEMark.DataSource = null;
                Grd_CCEMark.DataBind();
            }
        }
        
        protected void Drp_subject_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool subkillvalid = true;
            Load_SubjectSkillDroupdown(out subkillvalid);
            if (subkillvalid)
            {
                Gridview_Table.Visible = true;
                Load_GridviewDetails();
            }
            else
            {
                Gridview_Table.Visible = false;
                Grd_CCEMark.DataSource = null;
                Grd_CCEMark.DataBind();
            }
        }

        protected void Drp_skill_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_GridviewDetails();
        }

        protected void Grd_CCEMark_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }

        /// <summary>
        /// saving selected class student discriptive and grade from tblcce_discriptive tables
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            CLogging logger = CLogging.GetLogObject();
            string sql = "", _stuname = "";
            int _stuid = 0;
            int _subjectid = int.Parse(Drp_subject.SelectedValue);
            int _skillid = int.Parse(Drp_skill.SelectedValue);
            int _partid = int.Parse(Drp_part.SelectedValue);
            string _descriptive = "", _term1 = "", _term2 = "", _term3 = "";
            string _Err = "";

            #region Savedata

            try
            {
                if (Grd_CCEMark.Rows.Count > 0)
                {
                    if (Validation(out _Err))
                    {
                        for (int i = 0; i < Grd_CCEMark.Rows.Count; i++)
                        {
                            GridViewRow _row = Grd_CCEMark.Rows[i];
                            bool olddata = false;
                            for (int j = 2; j < _row.Cells.Count; j++)
                            {
                                _stuid = int.Parse(_row.Cells[0].Text.ToString());
                                _stuname = _row.Cells[1].Text.ToString();
                                TextBox tb1 = (TextBox)_row.Cells[3].FindControl("Txt_DescriptiveIndicator");
                                _descriptive = tb1.Text;
                                TextBox tb2 = (TextBox)_row.Cells[4].FindControl("Txt_TermOne");
                                _term1 = tb2.Text;
                                TextBox tb3 = (TextBox)_row.Cells[5].FindControl("Txt_TermTwo");
                                _term2 = tb3.Text;
                                TextBox tb4 = (TextBox)_row.Cells[3].FindControl("Txt_TermThree");
                                _term3 = tb4.Text;
                                if (!CheckWithThisSubIDandStuIDExitOrNot(_stuid, _subjectid, _skillid, _partid))
                                    sql = "INSERT into tblcce_descriptive (StudentId,SubjectId,SkillId,PartId,DescriptiveIndicator,Term1,Term2,Term3) VALUES (" + _stuid + "," + _subjectid + "," + _skillid + "," + _partid + ",'" + _descriptive + "','" + _term1.ToUpper() + "','" + _term2.ToUpper() + "','" + _term3.ToUpper() + "')";
                                else
                                {
                                    olddata = true;
                                    sql = "UPDATE tblcce_descriptive SET tblcce_descriptive.DescriptiveIndicator='" + _descriptive + "',tblcce_descriptive.Term1='" + _term1.ToUpper() + "',tblcce_descriptive.Term2='" + _term2.ToUpper() + "',tblcce_descriptive.Term3='" + _term3.ToUpper() + "' WHERE tblcce_descriptive.StudentId=" + _stuid + " AND tblcce_descriptive.SubjectId=" + _subjectid + " and tblcce_descriptive.SkillId=" + _skillid + " and tblcce_descriptive.PartId=" + _partid;
                                }
                                MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                            }

                            if (olddata)
                            {
                                logger.LogToFile("Descriptive Entry", _stuname + " " + Drp_skill.SelectedItem.Text.ToString() + " descriptive updated", 'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, MyUser.UserName);
                                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Descriptive Entry", _stuname + " " + Drp_skill.SelectedItem.Text.ToString() + " descriptive updated", 1);
                            }
                            else
                            {
                                logger.LogToFile("Descriptive Entry", _stuname + " " + Drp_skill.SelectedItem.Text.ToString() + " descriptive inserted", 'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, MyUser.UserName);
                                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Descriptive Entry", _stuname + " " + Drp_skill.SelectedItem.Text.ToString() + " descriptive inserted", 1);
                            }
                            _Err = "Student Descriptive is Updateded Sucessfully";
                        }

                    }
                }
                else
                    _Err = "Students is not found!";
            }
            catch (Exception ex)
            {
                _Err = "Student Descriptive is not Updateded Sucessfully! " + ex.Message;
                logger.LogToFile("Descriptive Entry", "throws Error " +_Err , 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, MyUser.UserName);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Descriptive Entry", "throws Error " + _Err, 1);

            }
            load_DefaultDetails();
            WC_MessageBox.ShowMssage(_Err);

            #endregion
        }


        /// <summary>
        /// this function checking validaion for while saving discriptive and grade 
        /// </summary>
        /// <param name="_Err"></param>
        /// <returns></returns>
        private bool Validation(out string _Err)
        {
            bool valid = true;
            _Err = "";
            for (int i = 0; i < Grd_CCEMark.Rows.Count; i++)
            {
                GridViewRow _row = Grd_CCEMark.Rows[i];
                for (int j = 2; j < _row.Cells.Count; j++)
                {

                    TextBox tb1 = (TextBox)_row.Cells[3].FindControl("Txt_DescriptiveIndicator");
                    TextBox tb2 = (TextBox)_row.Cells[4].FindControl("Txt_TermOne");
                    TextBox tb3 = (TextBox)_row.Cells[5].FindControl("Txt_TermTwo");
                    TextBox tb4 = (TextBox)_row.Cells[3].FindControl("Txt_TermThree");
                    if (tb1.Text == "" || tb2.Text == "" || tb3.Text == "" || tb4.Text == "")
                    {
                        _Err = "Some grade text field is empty! Enter those fields values";
                        break; 
                    }
                }
                if (_Err != "")
                {
                    valid = false;
                    break;
                }
            }

            return valid;
        }

        /// <summary>
        /// checing validation for selected skill discriptive and grade exising ot not 
        /// </summary>
        /// <param name="_stuid"></param>
        /// <param name="_subjectid"></param>
        /// <param name="_skillid"></param>
        /// <param name="_partid"></param>
        /// <returns></returns>
        private bool CheckWithThisSubIDandStuIDExitOrNot(int _stuid, int _subjectid, int _skillid, int _partid)
        {
            bool valid = false;
            string sql = "SELECT tblcce_descriptive.Id,tblcce_descriptive.DescriptiveIndicator from tblcce_descriptive WHERE tblcce_descriptive.StudentId="+_stuid+" AND tblcce_descriptive.SubjectId="+_subjectid+" and tblcce_descriptive.SkillId="+_skillid+" and tblcce_descriptive.PartId="+_partid;
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
        /// clearing all student discriptive and grade from grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Clear_Click(object sender, EventArgs e)
        {
            load_DefaultDetails();
            Drp_Class.Focus();
        }

        /// <summary>
        /// after clicking this event control will go to import cce discriptive page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Lnk_CCEGrade_Click(object sender, EventArgs e)
        {
            Response.Redirect("ImportCCEDescriptive.aspx");
        }

       
    }
}


