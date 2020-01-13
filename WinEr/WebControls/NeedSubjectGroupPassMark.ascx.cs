using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

namespace WinEr.WebControls
{
    public partial class NeedSubjectGroupPassMark : System.Web.UI.UserControl
    {

        private KnowinUser MyUser;
        private OdbcDataReader m_Myreader = null;
        private FeeManage MyFeeMang;
        static int Temp_scheduleId = 0;

        public event EventHandler EVNTSave;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyFeeMang = MyUser.GetFeeObj();
            if (MyFeeMang == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }
            if (!IsPostBack)
            {
               
            }
            Lbl_err.Visible = false;
        }

        private void LoadGrid(int Classid)
        {
           
            Grd_CCEstudent.DataSource = null;
            Grd_CCEstudent.DataBind();

            string _Err = "";
     
            try
            {
                string sql = "SELECT tblsubjects.Id as Id,tblsubjects.subject_name SubjectName from tblsubjects INNER JOIN tblclasssubmap ON tblclasssubmap.subjectid=tblsubjects.Id where tblclasssubmap.classid=" + Classid;
                DataSet MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (MydataSet.Tables.Count > 0)
                    if (MydataSet.Tables[0].Rows.Count > 0)
                    {


                        Grd_CCEstudent.Columns[1].Visible = true;
                        Grd_CCEstudent.DataSource = MydataSet;
                        Grd_CCEstudent.DataBind();
                        Grd_CCEstudent.Columns[1].Visible = false;
                        //HtmlDiv.Visible = true;
                        Grd_CCEstudent.Visible = true;

                    }
                    else
                        _Err = "Subject not found.Please map with subject from this Class!.";
                else
                    _Err = "Subject not found.Please map with subject from this Class!.";


            }
            catch (Exception ex)
            {
                _Err = "This class subject not found.Please map with subject from this Class!. " + ex;
                Lbl_err.Text = _Err;
                Lbl_err.Visible = true;
                Grd_CCEstudent.Visible = false;

            }

        }

        protected void ChkSelect_OnCheckedChanged(object sender, EventArgs e)
        {
            LoadCheckboxvalidation();
            MPE_MessageBox.Show();
        }

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

        protected void Chk_temselect_OnCheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int index = row.RowIndex;
            CheckBox chkterm = (CheckBox)Grd_CCEstudent.Rows[index].Cells[0].FindControl("Chk_temselect");
            TextBox tb1 = (TextBox)Grd_CCEstudent.Rows[index].Cells[3].FindControl("Txt_Mark");
            if (chkterm.Checked)
            {
                tb1.Enabled = true;
                tb1.BackColor=System.Drawing.Color.Gray;
                tb1.Focus();
            }

            else
            {
                tb1.Enabled = false;
                tb1.BackColor = System.Drawing.Color.White;
                tb1.Text = "0";
            }

            MPE_MessageBox.Show();
        }

        protected void Btn_save_Click(object sender, EventArgs e)
        {
            Lbl_err.Visible = false;
            if (Btn_save.Text == "Yes" && Btn_magok.Text == "No")
            {
                Popupwindow.Style.Add("style", "width:750px; top:500px;left:400px");
                Btn_save.Text = "Save";
                Btn_magok.Text = "Close";
                int Classid=int.Parse(Label1.Text.ToString());
                LoadGrid(Classid);
                LoadCheckboxvalidation();
                HtmlDiv.Visible = true;
                MPE_MessageBox.Show();
            }
            else
            {
                #region Save
                Grd_CCEstudent.Visible = true;
                Lbl_err.Visible = false;
                string _Err = "";
                int classid = 0, _examid = 0, subid = 0;
                double minpassmark = 0.0;
                try
                {
                    classid = int.Parse(Label1.Text.Trim());
                    _examid = int.Parse(Label2.Text.Trim());

                    foreach (GridViewRow dr in Grd_CCEstudent.Rows)
                    {
                        int.TryParse(dr.Cells[1].Text.ToString(), out subid);
                        CheckBox chkterm = (CheckBox)dr.Cells[0].FindControl("Chk_temselect");
                        if (chkterm.Checked == true)
                        {
                            TextBox tb1 = (TextBox)dr.Cells[2].FindControl("Txt_Mark");
                            double.TryParse(tb1.Text.ToString(), out minpassmark);
                            Config_Subject_For_Class(classid, _examid, minpassmark, subid);
                        }
                        else
                        {
                            string sql = "delete from tblsubgrouppassmark where tblsubgrouppassmark.ClassID=" + classid + " and tblsubgrouppassmark.SubjectId=" + subid + " AND tblsubgrouppassmark.ClassExamId=" + _examid;
                            MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                        }

                    }
                }
                catch (Exception ex)
                {
                    _Err = "Updated not sucessfully!." + ex;
                    Lbl_err.Text = _Err;
                    Grd_CCEstudent.Visible = false;
                    Lbl_err.Visible = true;
                    MPE_MessageBox.Show();

                }
                #endregion
            }

        }

        private void Config_Subject_For_Class(int classid, int _examid, double minpassmark, int subid)
        {
            string sql = "select * from tblsubgrouppassmark where tblsubgrouppassmark.ClassID=" + classid + " and tblsubgrouppassmark.SubjectId=" + subid;
            DataSet MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                sql = "delete from tblsubgrouppassmark where tblsubgrouppassmark.ClassID="+classid+" and tblsubgrouppassmark.SubjectId="+subid+" AND tblsubgrouppassmark.ClassExamId=" + _examid;
                MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            }
            sql = "INSERT into tblsubgrouppassmark (tblsubgrouppassmark.ClassID,tblsubgrouppassmark.ClassExamId,tblsubgrouppassmark.SubjectId,tblsubgrouppassmark.MinPassMark) VALUES(" + classid + "," + _examid + "," + subid + "," + minpassmark + ")";
            MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }

        protected void Txt_Mark_TextChanged(object sender, EventArgs e)
        {   GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            int index = row.RowIndex;
            TextBox tb1 = (TextBox)Grd_CCEstudent.Rows[index].Cells[3].FindControl("Txt_Mark");
            //CheckBox chkterm = (CheckBox)Grd_CCEstudent.Rows[index].Cells[0].FindControl("Chk_temselect");

            double minpassmark=0,passmark=0;
            double.TryParse(Lbl_passmark.Text.ToString(),out passmark);
            double.TryParse(tb1.Text.ToString(),out minpassmark);
            if (minpassmark > passmark)
            {
                tb1.BackColor = System.Drawing.Color.Red;
                tb1.Text = "0";
                tb1.Focus();
            }
            else
            {
                tb1.BackColor = System.Drawing.Color.Gray;
                
            }
            MPE_MessageBox.Show();
        }

        internal void ShowMssage(int Classid, int _examID,double _passmark)
        {
            Lbl_err.Visible = true; 
            try
            {
                Lbl_passmark.Text =Convert.ToString(_passmark);
                Label1.Text = Convert.ToString(Classid);
                Label2.Text = Convert.ToString(_examID);
                //style="width:500px; top:400px;left:400px"
                HtmlDiv.Visible = false;
                Lbl_err.Text = "IF you want to Create Aggregate Subject Group";
                Popupwindow.Style.Add("style", "width:500px; top:250px;left:100px");
                Btn_save.Text = "Yes";
                Btn_magok.Text = "No";
                Btn_magok.Focus();
            }
            catch (Exception ex)
            {
                Lbl_err.Text = ex.Message.ToString();
                Grd_CCEstudent.Visible = false;
            }
            MPE_MessageBox.Show();
        }

    }
}


 