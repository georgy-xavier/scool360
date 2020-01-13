using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WinBase;

namespace WinEr
{
    public partial class CCEsubskillmap : System.Web.UI.Page
    {
        private ExamManage MyExamMang;
        private KnowinUser MyUser;
        private DataSet MydataSet;
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
                    divgrid.Visible = false;
                    Load_PartDropDown();
                    Load_ClassDropDown();
                    Load_SubDropDown();
                    Drp_part.Focus();

                }
            }

        }

        /// <summary>
        /// this function subject names lodaing from subject dropdown control
        /// </summary>
        private void Load_SubDropDown()
        {
            Drp_subject.Items.Clear();
            string sql = "SELECT tblsubjects.Id as Id,tblsubjects.subject_name as Subjectname from tblsubjects";
            DataSet Ds_sub = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (Ds_sub != null && Ds_sub.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drcls in Ds_sub.Tables[0].Rows)
                {
                    li = new ListItem(drcls["Subjectname"].ToString(), drcls["Id"].ToString());
                    Drp_subject.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("NO Data Found", "0");
                Drp_subject.Items.Add(li);
            }
        }

        /// <summary>
        ///  this function part names lodaing from parts dropdown control
        /// </summary>
        private void Load_PartDropDown()
        {
            Drp_part.Items.Clear();
            string sql = "SELECT tblcce_parts.Id as Id,tblcce_parts.Description as Description from tblcce_parts";
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
        ///  this function class names lodaing from class dropdown control
        /// </summary>
        private void Load_ClassDropDown()
        {
            Drp_ClassSelect.Items.Clear();
            string sql = "SELECT tblclass.Id,tblclass.ClassName from tblclass where tblclass.Status=1 ORDER BY tblclass.ClassName";
            DataSet Ds_class = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (Ds_class != null && Ds_class.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drcls in Ds_class.Tables[0].Rows)
                {
                    li = new ListItem(drcls["ClassName"].ToString(), drcls["Id"].ToString());
                    Drp_ClassSelect.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("NO Data Found", "0");
                Drp_ClassSelect.Items.Add(li);
            }

        }

        /// <summary>
        /// after clicking this event control will come to previous page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_cancel_Click(object sender, EventArgs e)
        {
            divgrid.Visible = false;
            Load_PartDropDown();
            Load_ClassDropDown();
            Load_SubDropDown();
            div1.Visible = true;
            Drp_part.Focus();
        }

        /// <summary>
        /// after clicking this event control will show mapping UI based on the selected class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Continue_Click(object sender, EventArgs e)
        {
            Grd_CCEstudent.DataSource = null;
            Grd_CCEstudent.DataBind();
            string _Err = "";
            int ClassId = 0;
            try
            {
                int.TryParse(Drp_ClassSelect.SelectedItem.Value, out ClassId);
                string sql = "SELECT tblcce_subjectskills.Id as Id,tblcce_subjectskills.SkillName as Skillname from tblcce_subjectskills";
                MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (MydataSet.Tables.Count > 0)
                    if (MydataSet.Tables[0].Rows.Count > 0)
                    {
                        Lbl_classname.Text = Drp_ClassSelect.SelectedItem.Text;
                        Lbl_subjectname.Text = Drp_subject.SelectedItem.Text;


                        Grd_CCEstudent.Columns[1].Visible = true;
                        Grd_CCEstudent.DataSource = MydataSet;
                        Grd_CCEstudent.DataBind();
                        Grd_CCEstudent.Columns[1].Visible = false;
                      
                        int partid=int.Parse(Drp_part.SelectedItem.Value);
                        int classid = int.Parse(Drp_ClassSelect.SelectedItem.Value);
                        int subjectid = int.Parse(Drp_subject.SelectedItem.Value);

                        LoadChkBox_AllsSub(partid, classid, subjectid);
                        div1.Visible = false;
                        divgrid.Visible = true;
                       
                    }
                    else
                        _Err = "Skill not found";
                else
                    _Err = "Skill not found";


            }
            catch (Exception ex)
            {
                _Err = "Skill not found!."+ex;
            }
            if (_Err != "")
            {
                WC_MessageBox.ShowMssage(_Err);
            }

        }


        /// <summary>
        /// this function selected all skills checkbox on the grid
        /// </summary>
        /// <param name="partid"></param>
        /// <param name="classid"></param>
        /// <param name="subjectid"></param>
        private void LoadChkBox_AllsSub(int partid, int classid, int subjectid)
        {
            string sql = "SELECT tblcce_subjectskillmap.SkillId,tblcce_subjectskillmap.SkillOrder  from tblcce_subjectskillmap WHERE tblcce_subjectskillmap.PartId=" + partid + " AND  tblcce_subjectskillmap.ClassId=" + classid + " AND tblcce_subjectskillmap.SubjectId=" + subjectid;
            MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                int i = 0;
                foreach (GridViewRow dr in Grd_CCEstudent.Rows)
                {
                    GridViewRow _row = Grd_CCEstudent.Rows[i];
                    CheckBox chkterm = (CheckBox)_row.Cells[0].FindControl("Chk_temselect");
                    TextBox tb1 = (TextBox)_row.Cells[3].FindControl("Txt_Mark");
                    foreach (DataRow row in MydataSet.Tables[0].Rows)
                    {
                        if (dr.Cells[1].Text.ToString() == row[0].ToString())
                        {
                            chkterm.Checked = true;
                            tb1.Enabled = true;
                            if (row[1].ToString() != "")
                                tb1.Text = row[1].ToString();
                            else
                                tb1.Text = "0";
                        }
                        else
                        {
                            if (chkterm.Checked == false)
                            {
                                chkterm.Checked = false;
                                tb1.Enabled = false;
                            }
                        }
                    }
                    i++;
                }
            }
            else
                LoadCheckboxvalidation();

               
        }


        /// <summary>
        /// after clicking this event updated skill mapping and oder list from tblcce_subjectskillmap table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_update_Click(object sender, EventArgs e)
        {
            CLogging logger = CLogging.GetLogObject();
            string _Err = "";
            int classid = 0, partid = 0, subjectid=0;
            try
            {
                int.TryParse(Drp_part.SelectedItem.Value, out partid);
                int.TryParse(Drp_ClassSelect.SelectedItem.Value, out classid);
                int.TryParse(Drp_subject.SelectedItem.Value, out subjectid);
                string sql = "";
                foreach (GridViewRow dr in Grd_CCEstudent.Rows)
                {
                    CheckBox chkterm = (CheckBox)dr.Cells[0].FindControl("Chk_temselect");
                      TextBox tb1 = (TextBox)dr.Cells[3].FindControl("Txt_Mark");
                    if (chkterm.Checked)
                    {
                        string k = dr.Cells[1].Text.ToString();
                        Config_Subject_Skills(classid, subjectid, partid, int.Parse(dr.Cells[1].Text.ToString()), int.Parse(tb1.Text.Trim()));
                       
                        logger.LogToFile("Subject skill mapping", dr.Cells[2].Text.ToString() + " mapped from " + Drp_ClassSelect.SelectedItem.Text.ToString(), 'I', CLogging.PriorityEnum.LEVEL_DEBUG, MyUser.LoginUserName);
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Subject skill mapping", dr.Cells[2].Text.ToString() + " mapped from " + Drp_ClassSelect.SelectedItem.Text.ToString(), 1);

                    }
                    else
                    {
                        sql = "delete from tblcce_subjectskillmap where tblcce_subjectskillmap.ClassId=" + classid + " and tblcce_subjectskillmap.SubjectId=" + subjectid + " and tblcce_subjectskillmap.PartId=" + partid + " and tblcce_subjectskillmap.SkillId=" + int.Parse(dr.Cells[1].Text.ToString()) + "";
                        MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                       
                        logger.LogToFile("Subject skill mapping", dr.Cells[2].Text.ToString() + " mapped removed from " + Drp_ClassSelect.SelectedItem.Text.ToString(), 'I', CLogging.PriorityEnum.LEVEL_DEBUG, MyUser.LoginUserName);
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Subject skill mapping", dr.Cells[2].Text.ToString() + " mapped removed from " + Drp_ClassSelect.SelectedItem.Text.ToString(), 1);
                    }
                    _Err = "Updated  sucessfully.";
                }
            }
            catch (Exception ex)
            {
                _Err = "sucessfully not Updated!." + ex;
                
                logger.LogToFile("subject skill mapping", "throws Error " + _Err, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, MyUser.LoginUserName);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Subject skill mapping","throws Error " + _Err, 1);

            }
            WC_MessageBox.ShowMssage(_Err);
            LoadChkBox_AllsSub(partid, classid, subjectid);
           

        }


        /// <summary>
        /// update selected skills  from tblcce_subjectskillmap table
        /// </summary>
        /// <param name="_ClassID"></param>
        /// <param name="_SubjectID"></param>
        /// <param name="_PartID"></param>
        /// <param name="_SkillID"></param>
        /// <param name="_SkillOrder"></param>
        public void Config_Subject_Skills(int _ClassID, int _SubjectID, int _PartID, int _SkillID, int _SkillOrder)
        {
            string sql = "select * from tblcce_subjectskillmap where tblcce_subjectskillmap.ClassId=" + _ClassID + " and tblcce_subjectskillmap.SubjectId=" + _SubjectID + " and tblcce_subjectskillmap.PartId=" + _PartID + " and tblcce_subjectskillmap.SkillId=" + _SkillID;
            MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
                sql = "UPDATE tblcce_subjectskillmap SET tblcce_subjectskillmap.ClassId=" + _ClassID + ",tblcce_subjectskillmap.SubjectId=" + _SubjectID + ",tblcce_subjectskillmap.SkillId=" + _SkillID + ",tblcce_subjectskillmap.PartId=" + _PartID + ",tblcce_subjectskillmap.SkillOrder=" + _SkillOrder + " where tblcce_subjectskillmap.ClassId=" + _ClassID + " and tblcce_subjectskillmap.SubjectId=" + _SubjectID + " and tblcce_subjectskillmap.SkillId=" + _SkillID + " and tblcce_subjectskillmap.PartId=" + _PartID;
            else
                sql = "INSERT into tblcce_subjectskillmap (tblcce_subjectskillmap.ClassId,tblcce_subjectskillmap.SubjectId,tblcce_subjectskillmap.SkillId,tblcce_subjectskillmap.PartId,tblcce_subjectskillmap.SkillOrder) VALUES(" + _ClassID + "," + _SubjectID + "," + _SkillID + "," + _PartID + "," + _SkillOrder + ")";
            MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        }

        protected void ChkSelect_OnCheckedChanged(object sender, EventArgs e)
        {
            LoadCheckboxvalidation();
        }

        protected void Chk_temselect_OnCheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int index = row.RowIndex;
            CheckBox chkterm = (CheckBox)Grd_CCEstudent.Rows[index].Cells[0].FindControl("Chk_temselect");
            TextBox tb1 = (TextBox)Grd_CCEstudent.Rows[index].Cells[3].FindControl("Txt_Mark");
            if (chkterm.Checked)
            {
                tb1.Enabled = true;
            }
            else
            {
                tb1.Enabled = false;
                tb1.Text = "0";
            }
        }

        /// <summary>
        /// this event check with subject oder validation it will not accept duplicut order no
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Txt_Mark_TextChanged(object sender, EventArgs e)
        {
            bool valid = false;
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            int index = row.RowIndex;
            TextBox tb1 = (TextBox)Grd_CCEstudent.Rows[index].Cells[3].FindControl("Txt_Mark");
            tb1.BackColor = System.Drawing.Color.White;
            int i = 0;
            foreach (GridViewRow dr in Grd_CCEstudent.Rows)
            {
                TextBox tb2 = (TextBox)dr.Cells[3].FindControl("Txt_Mark");
                if (i != index)
                {
                   
                    if (tb1.Text == tb2.Text)
                    {
                        valid = true;
                        break;
                    }
                }
                i++;
            }
            if (valid == true)
            {
                tb1.BackColor = System.Drawing.Color.Red;
                WC_MessageBox.ShowMssage("Existing orderno");
                tb1.Text = "";
            }
        }

        /// <summary>
        ///  grid inner check box validation
        /// </summary>
        private void LoadCheckboxvalidation()
        {
            CheckBox _chkselect = (CheckBox)Grd_CCEstudent.HeaderRow.Cells[0].FindControl("ChkSelect");
            if (_chkselect.Checked)
            {
                for (int i = 0; i < Grd_CCEstudent.Rows.Count; i++)
                {
                    CheckBox chkterm = (CheckBox)Grd_CCEstudent.Rows[i].Cells[0].FindControl("Chk_temselect");
                    TextBox tb1 = (TextBox)Grd_CCEstudent.Rows[i].Cells[3].FindControl("Txt_Mark");
                    chkterm.Checked = true;
                    tb1.Enabled = true;
                }
            }
            else
            {
                for (int i = 0; i < Grd_CCEstudent.Rows.Count; i++)
                {
                    CheckBox chkterm = (CheckBox)Grd_CCEstudent.Rows[i].Cells[0].FindControl("Chk_temselect");
                    TextBox tb1 = (TextBox)Grd_CCEstudent.Rows[i].Cells[3].FindControl("Txt_Mark");
                    chkterm.Checked = false;
                    tb1.Enabled = false;
                    tb1.Text = "0";
                }
            }
        }


    }
}
