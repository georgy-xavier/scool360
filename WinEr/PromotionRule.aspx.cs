using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;

namespace WinEr
{
    public partial class PromotionRule : System.Web.UI.Page
    {
        private ConfigManager MyConfiMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet = null;
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            else
            {
                MyUser = (KnowinUser)Session["UserObj"];

                if (MyUser.SELECTEDMODE == 1)
                {
                    this.MasterPageFile = "~/WinerStudentMaster.master";

                }
                else if (MyUser.SELECTEDMODE == 2)
                {

                    this.MasterPageFile = "~/WinerSchoolMaster.master";
                }
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyConfiMang = MyUser.GetConfigObj();
            if (MyConfiMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(252))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    //string _MenuStr;
                    //_MenuStr = MyConfiMang.GetConfigMangMenuString(MyUser.UserRoleId);
                    //this.ConfigMenu.InnerHtml = _MenuStr;
                    //_MenuStr = MyUser.GetDetailsString(252);
                    //this.ActionInfo.InnerHtml = _MenuStr;
                    this.Tabs.ActiveTabIndex = 0;
                    Pnl_Exam.Visible = true;
                    Pnl_DelRul.Visible = false;
                    Pnl_Attendance.Visible = false;
                    LoadPromotionCriteriatoDrpList();
                    LoadGrid();
                    LoadRuleToGrid();
                    AddSubjectToChkList();
                    LoadClassToDrpList();
                }
            }
        }

        private void LoadClassToDrpList()
        {
            Drp_RlClass.Items.Clear();
            MyDataSet = MyUser.MyAssociatedClass();
            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                ListItem li;
                foreach (DataRow Dr_Class in MyDataSet.Tables[0].Rows)
                {
                    li = new ListItem(Dr_Class[1].ToString(), Dr_Class[0].ToString());
                    Drp_RlClass.Items.Add(li);
                }
            }
        }

        private void LoadRuleToGrid()
        {
            Grd_DeleteRule.Columns[0].Visible = true;
            Grd_DeleteRule.Columns[2].Visible = true;
            string sql = "select RuleId,Name,Type from tblpromotionrule_master";
            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Grd_DeleteRule.DataSource = MyReader;
                Grd_DeleteRule.DataBind();
                Pnl_DelRul.Visible = true;
                Grd_DeleteRule.Columns[0].Visible = false;
                Grd_DeleteRule.Columns[2].Visible = false;
            }
            else
            {
                Pnl_DelRul.Visible = false;
            }
        }

        private void LoadPromotionCriteriatoDrpList()
        {
            ListItem li;
            Drp_RuleType.Items.Clear();
            if(MyUser.HaveModule(3))
            {
                li = new ListItem("Exam Rule", "1");
                Drp_RuleType.Items.Add(li);
            }
            if (MyUser.HaveModule(21))
            {
                li = new ListItem("Attendance Rule", "2");
                Drp_RuleType.Items.Add(li);
            }
        }

        protected void Drp_RuleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Drp_RuleType.SelectedValue == "1")
            {
                Pnl_Exam.Visible = true;
                Pnl_Attendance.Visible = false;
            }
            else
            {
                Pnl_Exam.Visible = false;
                Pnl_Attendance.Visible = true;
            }
        }

        protected void Rdo_ExamType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void LoadGrid()
        {
            GridExams.Columns[1].Visible = true;
            GridExams.Columns[4].Visible = true;
            string sql = "";
            if (Rdo_ExamType.SelectedValue == "1")
            {
                sql = "select distinct tblexamcombmaster.Id as ExamId, tblexamcombmaster.ExamName,'Combined' as Period,0 as PeriodId from tblexamcombmaster inner join tblexamcombmap on tblexamcombmap.CombinedId = tblexamcombmaster.Id union select distinct tblexammaster.Id as ExamId, tblexammaster.ExamName , tblperiod.Period,tblperiod.Id as PeriodId from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster ON tblexammaster.Id= tblclassexam.ExamId inner join tblstudentmark on tblstudentmark.ExamSchId= tblexamschedule.Id inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId  where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "  AND tblexammaster.`Status`=1";
            }
            else if (Rdo_ExamType.SelectedValue == "2")
            {
                sql = "select distinct tblexamcombmaster.Id as ExamId, tblexamcombmaster.ExamName,'Combined' as Period , 0 as PeriodId from tblexamcombmaster inner join tblexamcombmap on tblexamcombmap.CombinedId = tblexamcombmaster.Id";
            }
            else
            {
                sql = "select distinct tblexammaster.Id as ExamId, tblexammaster.ExamName , tblperiod.Period ,tblperiod.Id as PeriodId from tblexamschedule inner join tblclassexam on tblexamschedule.ClassExamId= tblclassexam.Id inner join tblexammaster ON tblexammaster.Id= tblclassexam.ExamId inner join tblstudentmark on tblstudentmark.ExamSchId= tblexamschedule.Id inner join tblperiod on tblperiod.Id = tblexamschedule.PeriodId  where tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "  AND tblexammaster.`Status`=1";
            }
            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                this.GridArea.Visible = true;
                GridExams.DataSource = MyReader;
                GridExams.DataBind();
                GridExams.Columns[1].Visible = false;
                GridExams.Columns[4].Visible = false;
            }
            else
            {
                this.GridArea.Visible = false;
                GridExams.DataSource = null;
                GridExams.DataBind();
               
            }
        }

        //protected void GridExams_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            string strScript = "SelectDeSelectHeader(" + ((CheckBox)e.Row.Cells[0].FindControl("chkSelect")).ClientID + ");";
        //            ((CheckBox)e.Row.Cells[0].FindControl("chkSelect")).Attributes.Add("onclick", strScript);
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        //report error
        //    }
        //}

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            string Message = "";
            int Combined=0;
            if ((Drp_RuleType.SelectedValue == "1") && (ValidPercentage(out Message)) && (NewRuleName(Txt_RulName.Text.Trim(), out Message)))
            {
                CheckBox Chk_Select;
                TextBox Txt_Percentage;
                MyConfiMang.CreateTansationDb();
                try
                {
                    int MasterId = MyConfiMang.SavePromotionMaster(Txt_RulName.Text.Trim(), Drp_RuleType.SelectedValue);

                    foreach (GridViewRow gv in GridExams.Rows)
                    {
                        Chk_Select = (CheckBox)gv.FindControl("chkSelect");
                        if (Chk_Select.Checked)
                        {
                            if (Drp_RuleType.SelectedValue == "1")
                            {
                                Txt_Percentage = (TextBox)gv.FindControl("Txt_Percentage");
                                if (gv.Cells[3].Text.ToString() == "Combined")
                                {
                                    Combined = 1;
                                }
                                else
                                {
                                    Combined = 0;
                                }
                                MyConfiMang.SaveExamPromotionRule(MasterId, gv.Cells[1].Text.ToString(), gv.Cells[2].Text.ToString(), Txt_Percentage.Text.Trim(), Combined, gv.Cells[4].Text.ToString());
                            }
                            else if (Drp_RuleType.SelectedValue == "2")
                            {
                                // MyConfiMang.SaveAttendancePromotionRule();
                            }
                        }
                    }
                    MyConfiMang.EndSucessTansationDb();
                    LoadRuleToGrid();
                    Lbl_msg.Text = "Rule saved";
                    this.MPE_MessageBox.Show();
                    Txt_RulName.Text = "";
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Promotion Rule Creation", Drp_RuleType.SelectedItem.Text + " - " + Txt_RulName.Text.Trim() + " is Created ", 1);

                }
                catch (Exception exc)
                {
                    MyConfiMang.EndFailTansationDb();
                    Lbl_ExamMsg.Text = exc.Message;
                }
            }
            else
            {
                Lbl_msg.Text = Message;
                this.MPE_MessageBox.Show();
            }
        }

        private bool ValidAttPercentage(out string Message)
        {
            bool Valid = true;
            Message = "";
            if(Txt_AttPerc.Text.Trim()=="")
            {
                Valid = false;
                Message = "Percentage cannot be empty";
            }
            else if (double.Parse(Txt_AttPerc.Text.Trim())>100)
            {
                Valid = false;
                Message = "Percentage cannot be more than 100";
            }
            return Valid;
        }

        private bool ValidPercentage(out string Message)
        {
           CheckBox Chk_Select;
           //TextBox Txt_Percentage;
           bool Valid = true,Checked=false;
           Message = "";
            //double Per=0;
            //double Percent = 0;
           foreach (GridViewRow gv in GridExams.Rows)
           {
               Chk_Select = (CheckBox)gv.FindControl("chkSelect");
               if (Chk_Select.Checked)
               {
                   Checked = true;
                   //Txt_Percentage = (TextBox)gv.FindControl("Txt_Percentage");
                   //if(Txt_Percentage.Text !="")
                   //    double.TryParse(Txt_Percentage.Text,out Per);
                   //Percent = Percent + Per;
               }

           }
           if (!Checked)
           {
               Valid = false;
               Message = "Check any Exams";
           }
           
           return Valid;
        }

        protected void Btn_AttSave_Click(object sender, EventArgs e)
        {
            string Message="";
            if(Txt_RulName.Text.Trim() == "")
            {
                Lbl_msg.Text = "Rule name cannot be empty";
                this.MPE_MessageBox.Show();
            }
            else if ((Drp_RuleType.SelectedValue == "2") && (ValidAttPercentage(out Message)) && (NewRuleName(Txt_RulName.Text.Trim(), out Message)))
            {
                int MasterId = MyConfiMang.SavePromotionMaster(Txt_RulName.Text.Trim(), Drp_RuleType.SelectedValue);
                MyConfiMang.SaveAttendancePromotionRule(MasterId, Txt_AttPerc.Text.Trim());
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Promotion Rule Creation", Txt_RulName.Text.Trim()+" is Created ", 1);

                Lbl_msg.Text = "Rule saved";
                this.MPE_MessageBox.Show();
                Txt_RulName.Text = "";
                Txt_AttPerc.Text = "";
                LoadRuleToGrid();
            }
            else
            {
                Lbl_msg.Text = Message;
                this.MPE_MessageBox.Show();
            }

        }

        private bool NewRuleName(string _Name , out string _Message)
        {
            bool Valid = true;
            _Message = "";
            string sql = "select Name from tblpromotionrule_master where Name='" + _Name+"'";
            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _Message = "Rule Name already exists";
                Valid = false;
            }
            return Valid;
        }

        protected void Grd_DeleteRule_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton l = (LinkButton)e.Row.FindControl("lnk_Del");
                l.Attributes.Add("onclick", "javascript:return " +
                     "confirm('Are you sure you want to delete the Rule " +
                     DataBinder.Eval(e.Row.DataItem, "Name") + " ')");
            }
            
        }
    
        protected void DeleteRules(object sender, GridViewDeleteEventArgs e)
        {
           string  RuleId = Grd_DeleteRule.Rows[e.RowIndex].Cells[0].Text.Trim();
           if (MyConfiMang.RuleNotMappedToClass(RuleId))
           {
               MyConfiMang.DeleteRule(RuleId);
               MyUser.m_DbLog.LogToDb(MyUser.UserName, "Rule Deletion","Rule "+ Grd_DeleteRule.Rows[e.RowIndex].Cells[1].Text.Trim() + " Deleted", 1);

               LoadRuleToGrid();
               Lbl_msg.Text = "The rule is deleted";
               this.MPE_MessageBox.Show();

           }
           else
           {
               Lbl_msg.Text = "Cannot delete rule. It is mapped to some class";
               this.MPE_MessageBox.Show();
           }
          
        }

        protected void MapClasses(object sender, GridViewEditEventArgs e)
        {
            string RuleID = Grd_DeleteRule.Rows[e.NewEditIndex].Cells[0].Text;
            Hdn_RuleId.Value = RuleID;
            string RuleType = Grd_DeleteRule.Rows[e.NewEditIndex].Cells[2].Text;
            UncheckAllRuls();
            CheckRuleMappedClasses(RuleID);
            MPE_MapRule.Show();
        }

        private void UncheckAllRuls()
        {
            for (int i = 0; i < ChkBox_AllClass.Items.Count; i++)
            {
                ChkBox_AllClass.Items[i].Selected = false;
            }
        }

        private void CheckRuleMappedClasses(string _RuleID)
        {
            string sql = "select ClassId from tblclasspromotionrulemap where RuleId="+_RuleID;
            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    for (int i = 0; i < ChkBox_AllClass.Items.Count; i++)
                    {
                        if (ChkBox_AllClass.Items[i].Value == MyReader.GetValue(0).ToString())
                        {
                            ChkBox_AllClass.Items[i].Selected = true;
                        }                        
                    }
                }
            }
        }

        private void AddSubjectToChkList()
        {
            ChkBox_AllClass.Items.Clear();
            MyDataSet = MyUser.MyAssociatedClass();
            if (MyDataSet != null && MyDataSet.Tables!=null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                ListItem li;
                foreach(DataRow Dr_Class in MyDataSet.Tables[0].Rows)
                {
                    li = new ListItem(Dr_Class[1].ToString(), Dr_Class[0].ToString());
                    ChkBox_AllClass.Items.Add(li);
                }
            }
        }

        protected void Btn_MapClass_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ChkBox_AllClass.Items.Count; i++)
            {
                if ((ChkBox_AllClass.Items[i].Selected) && (!PromoRuleMapedToCLass(Hdn_RuleId.Value, ChkBox_AllClass.Items[i].Value)))
                {
                    string sql = "insert into tblclasspromotionrulemap(RuleId,ClassId) values (" + Hdn_RuleId.Value + "," + ChkBox_AllClass.Items[i].Value + ")";
                    MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Class-Rule Map Creation","Rule Mapped for "+ChkBox_AllClass.Items[i].Text, 1);

                }
                else if (!ChkBox_AllClass.Items[i].Selected)
                {
                    string sql = "delete from tblclasspromotionrulemap where RuleId =" + Hdn_RuleId.Value + " and ClassId= " + ChkBox_AllClass.Items[i].Value;
                    MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
                    sql = "delete from tblpromotionlist where tblpromotionlist.ClassId=" + ChkBox_AllClass.Items[i].Value;
                    MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
                }
            }
        }

        private bool PromoRuleMapedToCLass(string _RuleID, string _ClassId)
        {
            bool Valid = false;
            string sql = "select RuleId from tblclasspromotionrulemap where RuleId=" + _RuleID + " and ClassId="+_ClassId;
            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Valid = true;
            }
            return Valid;
        }

        protected void BtnClassRule_Click(object sender, EventArgs e)
        {
            LoadClassRules(Drp_RlClass.SelectedValue);
        }

        private void LoadClassRules(string _ClassId)
        {
            Grid_ClassRulw.Columns[0].Visible = true;
            string sql = "select distinct tblpromotionrule_master.RuleId,tblpromotionrule_master.Name from tblclasspromotionrulemap inner join tblpromotionrule_master on tblpromotionrule_master.RuleId = tblclasspromotionrulemap.RuleId where tblclasspromotionrulemap.ClassId='" + _ClassId+"'";
            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Grid_ClassRulw.DataSource = MyReader;
                Grid_ClassRulw.DataBind();
                Grid_ClassRulw.Columns[0].Visible = false;
                Pnl_ClassRule.Visible = true;
            }
            else
            {
                Grid_ClassRulw.DataSource = null;
                Grid_ClassRulw.DataBind();
                Pnl_ClassRule.Visible = false;
            }
            
        }

        protected void Grid_ClassRulw_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton l = (LinkButton)e.Row.FindControl("lnk_Remove");
                l.Attributes.Add("onclick", "javascript:return " +
                     "confirm('Are you sure you want to remove the Rule " +
                     DataBinder.Eval(e.Row.DataItem, "Name") + " ')");
            }            
        }

        protected void RemoveRuleMapping(object sender, GridViewDeleteEventArgs e)
        {
            string RuleID = Grid_ClassRulw.Rows[e.RowIndex].Cells[0].Text;
            string sql = "delete from tblclasspromotionrulemap where RuleId =" + RuleID + " and ClassId= " + Drp_RlClass.SelectedValue;
            MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            sql = "delete from tblpromotionlist where tblpromotionlist.ClassId=" + Drp_RlClass.SelectedValue;
            MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Class-Rule Map Deletion", Grid_ClassRulw.Rows[e.RowIndex].Cells[1].Text + " is Removed From " + Drp_RlClass.SelectedItem.Text, 1);

            LoadClassRules(Drp_RlClass.SelectedValue);

        }
    }
}
