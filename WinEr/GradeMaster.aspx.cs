using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

namespace WinEr
{
    public partial class GradeMaster : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private DataSet MyDataSet = null;

        private void LoadGradeMaster()
        {
            DrpGradeMstr.Items.Clear();
            string sql = " select tblgrademaster.Id, tblgrademaster.GradeName from tblgrademaster  ";
            DataSet DT = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (DT.Tables[0].Rows.Count > 0)
            {
               
               
                foreach (DataRow Dr in DT.Tables[0].Rows)
                {
                    ListItem  Li = new ListItem(Dr[1].ToString(), Dr[0].ToString());
                    DrpGradeMstr.Items.Add(Li);
                }
            }
            else
            {
                ListItem Li = new ListItem("No masters found", "-1");
                DrpGradeMstr.Items.Add(Li);
            }

        }

        private void LoadGridValues(int _MasatrId)
        {
            GrdVew_ExaGrade.Columns[0].Visible = true;
            GrdVew_ExaGrade.Columns[3].Visible = true;
            string sql = "select tblgrade.Id, tblgrade.Grade, tblgrade.LowerLimit, tblgrade.Result, tblgrade.Status,NumericalGrade from tblgrade where tblgrade.GradeMasterId =" + _MasatrId + "  order by tblgrade.id ASC ";
            MyDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            
            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                GrdVew_ExaGrade.DataSource = MyDataSet;
                GrdVew_ExaGrade.DataBind();
                Hdn_New.Value = "0";
            }
            else
            {
                Hdn_New.Value ="1";
                if (!DefaultGridListExist())
                {
                    GrdVew_ExaGrade.DataSource = null;
                    GrdVew_ExaGrade.DataBind();
                    Lbl_Err1.Text = "Grade List not found. Please contact support team for create new list.";
                }
            }

            GrdVew_ExaGrade.Columns[0].Visible = false;
            GrdVew_ExaGrade.Columns[3].Visible = false;
        }

        private bool DefaultGridListExist()
        {
            string sql = "select tblgrade.Id, tblgrade.Grade, tblgrade.LowerLimit, tblgrade.Result, tblgrade.Status,NumericalGrade from tblgrade where tblgrade.GradeMasterId =0 order by tblgrade.id ASC ";
            MyDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                GrdVew_ExaGrade.DataSource = MyDataSet;
                GrdVew_ExaGrade.DataBind();

                foreach (GridViewRow gv in GrdVew_ExaGrade.Rows)
                {
                    TextBox TxtMark = (TextBox)gv.FindControl("Txt_Limit");
                    TextBox TxtResult = (TextBox)gv.FindControl("Txt_Result");
                    TextBox Txt_Grade = (TextBox)gv.FindControl("Txt_Grade");
                    TextBox Txt_Ng = (TextBox)gv.FindControl("Txt_NG");
                        
                    CheckBox Chk = (CheckBox)gv.FindControl("Chk_Status");
                    TxtMark.Text = "";
                    Chk.Checked = false;
                    TxtResult.Text = "";
                    Txt_Ng.Text = "0";
                }

                Lbl_Err1.Text = "Grade list not found under selected grade master. Please fill and update.";
                return true;
            }
            else
                return false;
           
        }

        private void UpdateGrade(out string _Message)
        {
            _Message = "";
            int gradeid=0;
            int status = 0;
            float lowerlimit=0;
            string result="";
            try
            {
                if (Hdn_New.Value == "0")
                {
                    int NG = 0;
                    foreach (GridViewRow gv in GrdVew_ExaGrade.Rows)
                    {
                        NG = 0;
                        TextBox TxtMark = (TextBox)gv.FindControl("Txt_Limit");
                        TextBox TxtResult = (TextBox)gv.FindControl("Txt_Result");
                        TextBox Txt_Grade = (TextBox)gv.FindControl("Txt_Grade");
                        TextBox Txt_Ng = (TextBox)gv.FindControl("Txt_NG");
                        CheckBox Chk = (CheckBox)gv.FindControl("Chk_Status");

                        if (Chk.Checked == true)
                            status = 1;
                        else
                            status = 0;

                        int.TryParse(gv.Cells[0].Text.ToString().Trim(), out gradeid);
                        float.TryParse(TxtMark.Text.ToString().Trim(), out lowerlimit);
                        result = TxtResult.Text.ToString().Trim();
                     
                        int.TryParse(Txt_Ng.Text.Trim(),out NG);
                        string sql_update = "update tblgrade set tblgrade.Grade='" + Txt_Grade.Text + "', tblgrade.LowerLimit=" + lowerlimit + ", tblgrade.Status=" + status + ", tblgrade.Result='" + result + "',tblgrade.NumericalGrade=" + NG + " where tblgrade.Id=" + gradeid;
                        MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql_update);
                        _Message = "Grades updated successfully";

                    }

                }
                else
                {
                    int NG = 0;
                    foreach (GridViewRow gv in GrdVew_ExaGrade.Rows)
                    {
                        NG = 0;
                        TextBox TxtMark = (TextBox)gv.FindControl("Txt_Limit");
                        TextBox TxtResult = (TextBox)gv.FindControl("Txt_Result");
                        TextBox Txt_Grade = (TextBox)gv.FindControl("Txt_Grade");
                        TextBox Txt_Ng = (TextBox)gv.FindControl("Txt_NG");
                        CheckBox Chk = (CheckBox)gv.FindControl("Chk_Status");

                        if (Chk.Checked == true)
                            status = 1;
                        else
                            status = 0;

                        int.TryParse(Txt_Ng.Text.ToString().Trim(), out NG);
                        int.TryParse(gv.Cells[0].Text.ToString().Trim(), out gradeid);
                        float.TryParse(TxtMark.Text.ToString().Trim(), out lowerlimit);
                        result = TxtResult.Text.ToString().Trim();
                        string sql_update = "insert into tblgrade(Grade,LowerLimit,`Status`,Result,GradeMasterId,NumericalGrade) Values('" + Txt_Grade.Text + "' ,'" + lowerlimit + "'," + status + ",'" + TxtResult.Text + "'," + int.Parse(DrpGradeMstr.SelectedValue) + "," + NG + ")";
                        //string sql_update = "update tblgrade set tblgrade.Grade=" + Txt_Grade.Text + ", tblgrade.LowerLimit=" + lowerlimit + ", tblgrade.Status=" + status + ", tblgrade.Result='" + result + "' where tblgrade.Id=" + gradeid;
                        MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql_update);
                        _Message = "Grades updated successfully";

                    }
                }
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Grades Configured", "Grades Configured successfully", 1);
                LoadGridValues(int.Parse(DrpGradeMstr.SelectedValue.ToString())); 
            }
            catch (Exception e)
            {
                _Message = "Error in updation.Please try again";
            }
        }

        private void DoChanges(int _Type)
        {
            //_Type==0 means new name,1 means edit name

            Lbl_Err1.Text = "";
            Hdn_addName.Value = _Type.ToString();

            if (_Type == 1)
                Txt_MasterName.Text = DrpGradeMstr.SelectedItem.ToString();
            else
                Txt_MasterName.Text = "";

            Pnl_Edit.Visible = true;
            
        }

        private bool valideData(out string _Message)
        {
            bool _valid = true;
            int _minMark = -1;
            int _EnteredMark = 0;
            _Message = "";
            try
            {
                foreach (GridViewRow gv in GrdVew_ExaGrade.Rows)
                {
                    TextBox TxtMark = (TextBox)gv.FindControl("Txt_Limit");
                    TextBox TxtResult = (TextBox)gv.FindControl("Txt_Result");
                    CheckBox Chk = (CheckBox)gv.FindControl("Chk_Status");

                    if (TxtMark.Text.Trim() == "" && Chk.Checked == true && TxtResult.Text.Trim() == "")
                    {
                        _Message = "Lower limit and result  cannot be empty";
                        _valid = false;
                        break;
                    }

                    else if (TxtMark.Text != "" && Chk.Checked == true)
                    {
                        int.TryParse(TxtMark.Text.ToString().Trim(), out _EnteredMark);
                        if (_minMark != -1)
                        {
                            //_Message = "Mark cannot be greater than Maximum mark";
                            //_valid = false;
                            //break;
                            if (_minMark < _EnteredMark || _minMark == _EnteredMark)
                            {
                                _Message = "Lower limit cannot be grater than or equal to the lower limit of higher grade";
                                _valid = false;
                                goto ErrLbl;
                            }
                            else
                            {
                                int.TryParse(TxtMark.Text.ToString().Trim(), out _minMark);
                            }
                        }
                        else
                        {
                            int.TryParse(TxtMark.Text.ToString().Trim(), out _minMark);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                _Message = e.Message;
                _valid = false;
            }

        ErrLbl:
            return _valid;
        }

        protected void Btn_update_Click(object sender, EventArgs e)
        {
            string _Message = "";
            GrdVew_ExaGrade.Columns[0].Visible = true;
            GrdVew_ExaGrade.Columns[3].Visible = true;

            if (valideData(out _Message))
            {
                UpdateGrade(out _Message);
                WC_MessageBox.ShowMssage(_Message);
                
            }
            else
            {
                WC_MessageBox.ShowMssage(_Message);
            }

            GrdVew_ExaGrade.Columns[0].Visible = false;
            GrdVew_ExaGrade.Columns[3].Visible = false;

        }
  
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();

            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(771))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (!IsPostBack)
                {
                    Pnl_Edit.Visible = false;
                    LoadGradeMaster();
                    LoadGridValues(int.Parse(DrpGradeMstr.SelectedValue.ToString()));
                }
            }

        }

        protected void GrdVew_ExaGrade_RowBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox Chk = (CheckBox)e.Row.FindControl("Chk_Status");
                if (int.Parse(e.Row.Cells[3].Text.ToString()) == 1)
                {

                    Chk.Checked = true;
                }
                else
                {
                    Chk.Checked = false;
                }
            }
        }

        protected void Btn_Restore_Click(object sender, EventArgs e)
        {
            LoadGridValues(int.Parse(DrpGradeMstr.SelectedValue.ToString()));
        }

        protected void ImgAddNew_Click(object sender, ImageClickEventArgs e)
        {
            DoChanges(0);
        }

        protected void Lnk_AddNewMaster_Click(object sender, EventArgs e)
        {
            DoChanges(0);
        }

        protected void ImgEditNew_Click(object sender, ImageClickEventArgs e)
        {
            DoChanges(1);
        }

        protected void Lnk_EditMaster_Click(object sender, EventArgs e)
        {
            DoChanges(1);
        }

        protected void Btn_SaveName_Click(object sender, EventArgs e)
        {
            if (Txt_MasterName.Text != "")
            {
                int Type = int.Parse(Hdn_addName.Value);
                string Val=DrpGradeMstr.SelectedValue;
                string sqlQry = "";

                if (Type == 1)
                    sqlQry = "update tblgrademaster set tblgrademaster.GradeName='" + Txt_MasterName.Text.Trim() + "' where tblgrademaster.Id=" + DrpGradeMstr.SelectedValue;
                else
                {
                    sqlQry = " insert into tblgrademaster(GradeName) values('" + Txt_MasterName.Text.Trim() + "')";

                }

                MyStudMang.m_MysqlDb.ExecuteQuery(sqlQry);
                Lbl_Err1.Text = "Grade Master updated";

                sqlQry = "select tblgrademaster.Id from tblgrademaster where tblgrademaster.GradeName='" + Txt_MasterName.Text.Trim() + "'";
                OdbcDataReader Reader= MyStudMang.m_MysqlDb.ExecuteQuery(sqlQry);
                if(Reader.HasRows)
                    Val= Reader.GetValue(0).ToString();

                LoadGradeMaster();

                DrpGradeMstr.SelectedValue = Val;
                LoadGridValues(int.Parse(Val.ToString()));
                
                Pnl_Edit.Visible = false;
            }
            else
                Lbl_Err1.Text = "Please enter grade master name";

           
        }

        protected void DrpGradeMstr_SelectedIndexChanged(object sender, EventArgs e)
        {
            Lbl_Err1.Text = "";
            LoadGridValues(int.Parse(DrpGradeMstr.SelectedValue.ToString()));
        }

    }
}

