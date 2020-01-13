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
    public partial class CCEclasssubjectmap : System.Web.UI.Page
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
                    Load_ClassDropDown();
                    Drp_ClassSelect.Focus();

                }
            }

        }


        /// <summary>
        /// here loading class dropdown 
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
        /// after clickng this button show with selected class subject after that we can map and fix subect order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Continue_Click(object sender, EventArgs e)
        {
            Grd_CCEstudent.DataSource = null;
            Grd_CCEstudent.DataBind();

            string _Err = "";
            int ClassId=0;
            try
            {
                int.TryParse(Drp_ClassSelect.SelectedItem.Value,out ClassId);
                string sql = "SELECT tblsubjects.Id as Id,tblsubjects.subject_name SubjectName from tblsubjects INNER JOIN tblclasssubmap ON tblclasssubmap.subjectid=tblsubjects.Id where tblclasssubmap.classid=" + ClassId;
                MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (MydataSet.Tables.Count > 0)
                    if (MydataSet.Tables[0].Rows.Count > 0)
                    {
                        Lbl_classname.Text = Drp_ClassSelect.SelectedItem.Text;
                       
                        Grd_CCEstudent.Columns[1].Visible = true;
                        Grd_CCEstudent.DataSource = MydataSet;
                        Grd_CCEstudent.DataBind();
                        Grd_CCEstudent.Columns[1].Visible = false;
                        
                        int classid = int.Parse(Drp_ClassSelect.SelectedItem.Value);
                        LoadChkBox_AllsSub(classid);
                        div1.Visible = false;
                        divgrid.Visible = true;
                        Label1.Visible = false;
                        
                    }
                    else
                        _Err = "Subject not found.Please map with subject from this Class!.";
                else
                    _Err = "Subject not found.Please map with subject from this Class!.";



                if (_Err != "")
                {
                    Label1.Visible = true;
                    Label1.Visible = true;
                }
                

            }
            catch (Exception ex)
            {
                _Err = "This class subject not found.Please map with subject from this Class!. "+ex;
                WC_MessageBox.ShowMssage(_Err);
            }
           

        }

        /// <summary>
        /// checkbox event if click this event select all subject from on the grid
        /// </summary>
        /// <param name="classid"></param>
        private void LoadChkBox_AllsSub(int classid)
        {
            int orderno = 0;
            string sql = "select tblcce_classsubject.subjectid,tblcce_classsubject.SubjectOrder from tblcce_classsubject where tblcce_classsubject.classid=" + classid;
            MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (GridViewRow dr in Grd_CCEstudent.Rows)
                {
                    CheckBox chkterm = (CheckBox)dr.Cells[0].FindControl("Chk_temselect");
                    TextBox tb1 = (TextBox)dr.Cells[3].FindControl("Txt_Mark");
                    foreach (DataRow row in MydataSet.Tables[0].Rows)
                    {
                        if (dr.Cells[1].Text.ToString() == row[0].ToString())
                        {
                            int.TryParse(row[1].ToString(), out orderno);
                            chkterm.Checked = true;
                            tb1.Enabled = true;
                            tb1.Text = orderno.ToString();
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
                }
            }

        }

        /// <summary>
        /// this event is update events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_update_Click(object sender, EventArgs e)
        {
            CLogging logger = CLogging.GetLogObject();
            string _Err = "";
            int classid = 0,Orderno=0;
            try
            {
                  
                  int.TryParse(Drp_ClassSelect.SelectedItem.Value, out classid);
                  string sql = "";
                  foreach (GridViewRow dr in Grd_CCEstudent.Rows)
                  {
                      CheckBox chkterm = (CheckBox)dr.Cells[0].FindControl("Chk_temselect");
                      if (chkterm.Checked==true)
                      {
                          TextBox tb1 = (TextBox)dr.Cells[2].FindControl("Txt_Mark");
                          int.TryParse(tb1.Text.ToString(), out Orderno);
                          Config_Subject_For_Class(Orderno, classid, int.Parse(dr.Cells[1].Text.ToString()));
                      }
                      else
                      {
                          sql = "delete from tblcce_classsubject where tblcce_classsubject.classid=" + classid + " and tblcce_classsubject.subjectid=" + int.Parse(dr.Cells[1].Text.ToString()) + "";
                          MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                      }
                      _Err = "Updated  sucessfully.";
                      logger.LogToFile("Subject Configuration", Drp_ClassSelect.SelectedItem.Text.ToString() + " subject caonfigured", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, MyUser.LoginUserName);
                      MyUser.m_DbLog.LogToDb(MyUser.UserName, "Subject Configuration", Drp_ClassSelect.SelectedItem.Text.ToString() + " subject caonfigured", 1);

                  }
            }
            catch (Exception ex)
            {
                _Err = " Updated not sucessfully!."+ex;
                logger.LogToFile("Subject configuration", "throws Error" + Drp_ClassSelect.SelectedItem.Text +_Err, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, MyUser.LoginUserName);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Subject Configuration", "throws Error" + Drp_ClassSelect.SelectedItem.Text + _Err, 1);
            }
            WC_MessageBox.ShowMssage(_Err);
            LoadChkBox_AllsSub(classid);
            
        }

        /// <summary>
        /// this function insert and updated subject mapping from tblcce_classsubject table
        /// </summary>
        /// <param name="_OrderNo"></param>
        /// <param name="_ClassId"></param>
        /// <param name="_SubjectId"></param>
        public void Config_Subject_For_Class(int _OrderNo,int _ClassId,int _SubjectId)
        {
            string sql = "select * from tblcce_classsubject where tblcce_classsubject.classid=" + _ClassId + " and tblcce_classsubject.subjectid=" + _SubjectId;
            MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
                sql = "UPDATE tblcce_classsubject SET tblcce_classsubject.classid=" + _ClassId + ",tblcce_classsubject.subjectid=" + _SubjectId + ",tblcce_classsubject.SubjectOrder=" + _OrderNo + " where tblcce_classsubject.classid=" + _ClassId + " and tblcce_classsubject.subjectid=" + _SubjectId;
            else
                sql = "INSERT into tblcce_classsubject (tblcce_classsubject.classid,tblcce_classsubject.subjectid,tblcce_classsubject.SubjectOrder) VALUES(" + _ClassId + "," + _SubjectId + "," + _OrderNo + ")";
            MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        }

        /// <summary>
        /// after clicking this event control should go to previous design
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_cancel_Click(object sender, EventArgs e)
        {
            divgrid.Visible =false;
            Load_ClassDropDown();
            div1.Visible = true;
            Drp_ClassSelect.Focus();

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
                tb1.Enabled = true;
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
            int i=0;
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
        /// grid inner check box validation
        /// </summary>
        private void LoadCheckboxvalidation()
        {
            CheckBox _chkselect = (CheckBox)Grd_CCEstudent.HeaderRow.Cells[0].FindControl("ChkSelect");
            if (_chkselect.Checked)
            {
                foreach (GridViewRow dr in Grd_CCEstudent.Rows)
                {
                    CheckBox chkterm = (CheckBox)dr.Cells[0].FindControl("Chk_temselect");
                    TextBox tb2 = (TextBox)dr.Cells[3].FindControl("Txt_Mark");
                    chkterm.Checked = true;
                    tb2.Enabled = true;
                }
            }
            else
            {
                foreach (GridViewRow dr in Grd_CCEstudent.Rows)
                {
                    CheckBox chkterm = (CheckBox)dr.Cells[0].FindControl("Chk_temselect");
                    TextBox tb2 = (TextBox)dr.Cells[3].FindControl("Txt_Mark");
                    chkterm.Checked = false;
                    tb2.Enabled = false;
                }
                
            }
        }

    }
}
