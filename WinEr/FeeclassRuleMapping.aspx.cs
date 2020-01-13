using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using System.Xml;
using WinBase;
using System.IO;
using AjaxControlToolkit;

namespace WinEr
{
    public partial class FeeclassRuleMapping : System.Web.UI.Page
    {
        private ExamManage MyExamMang;
        private KnowinUser MyUser;
        private FeeManage MyFeeMang;
        private DataSet MydataSet;
        private OdbcDataReader MyReader = null;
        private OdbcDataReader MyReader1 = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (Session["FeeId"] == null)
            {
                Response.Redirect("FeeclassRuleMapping.aspx");
            }
           
            MyUser = (KnowinUser)Session["UserObj"];
            MyExamMang = MyUser.GetExamObj();
            MyFeeMang = MyUser.GetFeeObj();
            if (MyExamMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(74))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {


                if (!IsPostBack)
                {
                    string _ExamMenuStr;
                   
                    _ExamMenuStr = MyFeeMang.GetSubFeeMangMenuString(MyUser.UserRoleId ,int.Parse(Session["FeeId"].ToString()));
                    this.SubExammngMenu.InnerHtml = _ExamMenuStr;
                    LoadDetails();
                    //some initlization
                    addvaluesToDropDownRule();
                    AddClassToDropDownClass();
                    
                }

            }
            
        }

        private void LoadDetails()
        {
            string sql = "SELECT tblfeeaccount.AccountName, tblfeefrequencytype.FreequencyName, tblfeeasso.Name from tblfeeaccount inner join tblfeefrequencytype on tblfeefrequencytype.Id= tblfeeaccount.FrequencyId inner join tblfeeasso on tblfeeasso.Id = tblfeeaccount.AssociatedId where tblfeeaccount.Id=" + int.Parse(Session["FeeId"].ToString());
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Lbl_FeeName.Text = MyReader.GetValue(0).ToString();
                Lbl_Freq.Text = MyReader.GetValue(1).ToString();
                Lbl_asso.Text = MyReader.GetValue(2).ToString();

            }
            MyReader.Close();
        }

        private void AddClassToDropDownClass()
        {
            Drp_class.Items.Clear();
            ListItem li = new ListItem("None");
            Drp_class.Items.Add(li);
            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {

                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_class.Items.Add(li);

                }

            }
            else
            {
                 li = new ListItem("No Class Present", "-1");
                Drp_class.Items.Add(li);

            }

            Drp_class.SelectedIndex = 0;
            

        }

        private void addvaluesToDropDownRule()
        {
            Drp_Rules.Items.Clear();
            ListItem li = new ListItem("None");
            Drp_Rules.Items.Add(li);
            string sql = "select tblrules.Id , tblrules.RuleName from tblrules";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Rules.Items.Add(li);

                }

            }
        }

        protected void SelectINdexChange_drpRules(object sender, EventArgs e)
        {
            if (Drp_Rules.SelectedItem.ToString() != "None")
            {

                if (Drp_class.SelectedItem.ToString() != "None")
                {
                    // loaddataToGrdAccoringToClass(int.Parse(Drp_class.SelectedValue.ToString()));
                    if (Grd_RuledEntry.Rows.Count > 0)
                    {
                        if (!CkeckForDatealreadyEnterToGrd(Drp_class.SelectedItem.ToString(), Drp_Rules.SelectedItem.ToString()))
                        {
                            Grd_RuledEntry.DataSource = AddPreviousData();
                            Grd_RuledEntry.DataBind();
                        }
                        else
                        {
                            Lbl_msg.Text = "This Rule Is Already Selected for this Class";
                            this.MPE_MessageBox.Show();

                        }

                    }
                    else
                    {
                        Grd_RuledEntry.DataSource = AddData();
                        Grd_RuledEntry.DataBind();
                        //addclassname();
                    }
                    Btn_save.Visible = true;
                    Btn_cancel.Visible = true;
                }
                else
                {
                    Lbl_msg.Text = "Select the Class";
                    this.MPE_MessageBox.Show();
                }
            }

            else
            {
                Lbl_msg.Text = "Select the Rule";
                this.MPE_MessageBox.Show();
            }
        }

        protected void SelectIndexChange_DrpClass(object sender, EventArgs e)
        {
            Drp_Rules.Text = "None";
            Grd_RuledEntry.DataSource=null;
           // removegrdDate();
            if (Drp_class.Text != "None")
            {
                string sql = "select tblrules.RuleName  from tblrules inner join tblruleclassmap on tblrules.Id= tblruleclassmap.RuleId inner join tblclass on tblclass.Id = tblruleclassmap.classId where  tblruleclassmap.classId=" + int.Parse(Drp_class.SelectedValue.ToString()) + " and tblruleclassmap.feeTypeId=" + int.Parse(Session["FeeId"].ToString());
                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        if (!CkeckForDatealreadyEnterToGrd(Drp_class.SelectedItem.ToString(), MyReader.GetValue(0).ToString()))
                        {
                            loaddataToGrdAccoringToClass(Drp_class.SelectedValue.ToString());
                        }
                    }
                }
            }
        }

        private void loaddataToGrdAccoringToClass(string _ClassId)
        {
            Grd_RuledEntry.DataSource = null;
            
            
                if (Grd_RuledEntry.Rows.Count > 0)
                {
                    Grd_RuledEntry.DataSource = AddPreviousDatafromTbl(Drp_class.SelectedValue.ToString());
                    Grd_RuledEntry.DataBind();
                }
                else
                {
                    Grd_RuledEntry.DataSource = AddDataFromTbl(Drp_class.SelectedValue.ToString());
                    Grd_RuledEntry.DataBind();
                }
            
        }

        private object AddDataFromTbl(string _classId)
        {
            DataSet _feedataset = new DataSet();
            DataTable dt;
            DataRow dr;
            _feedataset.Tables.Add(new DataTable("feeRule"));
            dt = _feedataset.Tables["feeRule"];
            dt.Columns.Add("Rule name");
            dt.Columns.Add("Class");
            dt.Columns.Add("Amount");
           

            string sql = "select tblrules.Id ,tblrules.RuleName , tblclass.ClassName , tblrules.Amount from tblrules inner join tblruleclassmap on tblrules.Id= tblruleclassmap.RuleId inner join tblclass on tblclass.Id = tblruleclassmap.classId where  tblruleclassmap.classId=" + _classId + " and tblruleclassmap.feeTypeId=" + int.Parse(Session["FeeId"].ToString());
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    dr = _feedataset.Tables["feeRule"].NewRow();
                    dr["Rule name"] = MyReader.GetValue(1).ToString();
                    if (float.Parse(MyReader.GetValue(3).ToString()) < 0)
                    {
                        dr["Amount"] = -1 * float.Parse(MyReader.GetValue(3).ToString());

                    }
                    else
                    {
                        dr["Amount"] = MyReader.GetValue(3).ToString();
                    }
                    
                    dr["Class"] = MyReader.GetValue(2).ToString();
                    _feedataset.Tables["feeRule"].Rows.Add(dr);
                }
            }
           
           
            return _feedataset;
        }

        private object AddPreviousDatafromTbl(string _classId)
        {
            DataSet _feedataset = new DataSet();
            DataTable dt;
            DataRow dr;
            _feedataset.Tables.Add(new DataTable("feeRule"));
            dt = _feedataset.Tables["feeRule"];
            //dt.Columns.Add("Class Id");
            //dt.Columns.Add("Class Name");
            dt.Columns.Add("Rule name");
            dt.Columns.Add("Class");
            dt.Columns.Add("Amount");
            foreach (GridViewRow gv in Grd_RuledEntry.Rows)
            {
                dr = _feedataset.Tables["feeRule"].NewRow();
                dr["Rule name"] = gv.Cells[1].Text.ToString();
                dr["Class"] = gv.Cells[2].Text.ToString();
                dr["Amount"] = gv.Cells[3].Text.ToString();
                _feedataset.Tables["feeRule"].Rows.Add(dr);
            }
           
            string sql = "select tblrules.Id ,tblrules.RuleName , tblclass.ClassName , tblrules.Amount from tblrules inner join tblruleclassmap on tblrules.Id= tblruleclassmap.RuleId inner join tblclass on tblclass.Id = tblruleclassmap.classId where  tblruleclassmap.classId=" + _classId + " and tblruleclassmap.feeTypeId=" + int.Parse(Session["FeeId"].ToString());
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    dr = _feedataset.Tables["feeRule"].NewRow();
                    dr["Rule name"] = MyReader.GetValue(1).ToString();
                    if (float.Parse(MyReader.GetValue(3).ToString()) < 0)
                    {
                        dr["Amount"] = -1 * float.Parse(MyReader.GetValue(3).ToString());

                    }
                    else
                    {
                        dr["Amount"] = MyReader.GetValue(3).ToString();
                    }
                    dr["Class"] = MyReader.GetValue(2).ToString();
                    _feedataset.Tables["feeRule"].Rows.Add(dr);
                }
            }
           
            return _feedataset;
        }

        

       

        private object AddData()
        {
            DataSet _feedataset = new DataSet();
            DataTable dt;
            DataRow dr;
            _feedataset.Tables.Add(new DataTable("feeRule"));
            dt = _feedataset.Tables["feeRule"];           
            dt.Columns.Add("Rule name");
            dt.Columns.Add("Class");
            dt.Columns.Add("Amount");
            dr = _feedataset.Tables["feeRule"].NewRow();

            int _ruleId = int.Parse(Drp_Rules.SelectedValue);
           
            string sql = "select tblrules.RuleName, tblrules.Amount  from   tblrules where  tblrules.Id=" + _ruleId;            
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                dr["Rule name"] = MyReader.GetValue(0).ToString();
                if (float.Parse(MyReader.GetValue(1).ToString()) < 0)
                {
                    dr["Amount"] = -1 * float.Parse(MyReader.GetValue(1).ToString());

                }
                else
                {
                    dr["Amount"] = MyReader.GetValue(1).ToString();
                }
                
                
            }
            dr["Class"] = Drp_class.SelectedItem.Text;
            _feedataset.Tables["feeRule"].Rows.Add(dr);
            return _feedataset;
        }

        private object AddPreviousData()
        {
            DataSet _feedataset = new DataSet();
            DataTable dt;
            DataRow dr;
                                            
                _feedataset.Tables.Add(new DataTable("feeRule"));
                dt = _feedataset.Tables["feeRule"];                
                dt.Columns.Add("Rule name");
                dt.Columns.Add("Class");
                dt.Columns.Add("Amount");
                foreach (GridViewRow gv in Grd_RuledEntry.Rows)
                {
                    dr = _feedataset.Tables["feeRule"].NewRow();
                    dr["Rule name"] = gv.Cells[1].Text.ToString();
                    dr["Class"] = gv.Cells[2].Text.ToString();                   
                    dr["Amount"] = gv.Cells[3].Text.ToString();
                    
                    _feedataset.Tables["feeRule"].Rows.Add(dr);
                }
                dr = _feedataset.Tables["feeRule"].NewRow();
                int _ruleId = int.Parse(Drp_Rules.SelectedValue);

                string sql = "select tblrules.RuleName, tblrules.Amount  from   tblrules where  tblrules.Id=" + _ruleId;
                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    dr["Rule name"] = MyReader.GetValue(0).ToString();
                    if (float.Parse(MyReader.GetValue(1).ToString()) < 0)
                    {
                        dr["Amount"] = -1 * float.Parse(MyReader.GetValue(1).ToString());

                    }
                    else
                    {
                        dr["Amount"] = MyReader.GetValue(1).ToString();
                    }
                }
                dr["Class"] = Drp_class.SelectedItem.Text;
                _feedataset.Tables["feeRule"].Rows.Add(dr);
                
           
            return _feedataset;
        }

        private bool CkeckForDatealreadyEnterToGrd(string _class, string _RuleName)
        {            
            bool _flag = false;

            
            Grd_RuledEntry.Columns.Clear();
            Grd_RuledEntry.DataSource = null;
            Grd_RuledEntry.DataBind();
            foreach(GridViewRow gv  in Grd_RuledEntry.Rows)
            {

                if (gv.Cells[1].Text == _RuleName && gv.Cells[2].Text == _class)
                {
                    _flag = true;
                }
            }



            return _flag;
        }

        private void RemoveRow(GridViewRow _gv)
        {
            DataSet _feedataset = new DataSet();
            DataTable dt;
            DataRow dr;
            _feedataset.Tables.Add(new DataTable("feeRule"));
            dt = _feedataset.Tables["feeRule"];
            //dt.Columns.Add("Class Id");
            //dt.Columns.Add("Class Name");          
            dt.Columns.Add("Rule name");
            dt.Columns.Add("Class");
            dt.Columns.Add("Amount");
            foreach (GridViewRow gv in Grd_RuledEntry.Rows)
            {
                if (_gv != gv)
                {
                    dr = _feedataset.Tables["feeRule"].NewRow();
                    //dr["Class Id"] = int.Parse(gv.Cells[1].Text.ToString());
                    //dr["Class Name"] = gv.Cells[2].Text.ToString();
                    dr["Rule name"] = gv.Cells[1].Text.ToString();
                    dr["Class"] = gv.Cells[2].Text.ToString();
                    dr["Amount"] = double.Parse(gv.Cells[3].Text.ToString());
                    
                    _feedataset.Tables["feeRule"].Rows.Add(dr);
                }
            }
            Grd_RuledEntry.DataSource = _feedataset;
            Grd_RuledEntry.DataBind();
            if (Grd_RuledEntry.Rows.Count == 0)
            {
                Btn_save.Visible = false;
                Btn_cancel.Visible = false;  
            }
            //AddSubjectToEditList();
        }

        

        protected void Grd_RuledEntry_SelectedIndexChanged1(object sender, EventArgs e)
        {
            this.MPE_IncidentPopUp.Show();
            
        }

        private void deleteEntryFromFeeclassmap(int _feeclassmapId)
        {
            string sql = "delete from tblruleclassmap where tblruleclassmap.Id=" + _feeclassmapId;
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
        }

        private bool checkIfRuleIsScheduled(int _RuleId, int _ClassId, int _FeeId)
        {
            bool _flage = false;            
            
                
                string sql = "select tblfeestudent.Id from tblfeeschedule  inner join tblfeestudent on tblfeestudent.SchId= tblfeeschedule.Id where tblfeeschedule.ClassId=" + _ClassId + "  and tblfeeschedule.FeeId=" + _FeeId + " and tblfeeschedule.BatchId=" + MyUser.CurrentBatchId;
                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    _flage = true;
                }
           
            return _flage;
        }

        private bool checkedIfThisPerticularEntryInTheRuleTbl(int _RuleId, int _ClassId, out int _feeclassmapId)
        {
            bool _flag = false;
            _feeclassmapId = 0;
            string sql = "select tblruleclassmap.Id from tblruleclassmap where tblruleclassmap.classId=" + _ClassId + " and tblruleclassmap.feeTypeId=" + int.Parse(Session["FeeId"].ToString()) + " and tblruleclassmap.RuleId=" + _RuleId;
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _feeclassmapId = int.Parse(MyReader.GetValue(0).ToString());
                _flag = true;
            }
            return _flag;
        }

        protected void Btn_save_Click(object sender, EventArgs e)
        {
            int _RuleId;
            int _ClassId;
            string _msg="";
            string _rule = "";
            string _class = "";
            double _amnt = 0;
            if (Grd_RuledEntry.Rows.Count > 0)
            {
                foreach (GridViewRow gv in Grd_RuledEntry.Rows)
                {
                    if(gv.Cells[0].Text=="")
                    {
                        _rule = gv.Cells[1].Text;
                        _class = gv.Cells[2].Text;
                        _amnt = double.Parse(gv.Cells[3].Text.ToString());
                    }
                    else
                    {
                        _rule = gv.Cells[0].Text;
                        _class = gv.Cells[1].Text;
                        _amnt = double.Parse(gv.Cells[2].Text.ToString());
                    }

                    getRuleIdAndClassId(_rule, _class, out _RuleId, out _ClassId);
                    if (!MyFeeMang.CheckRuleAndClassEntryToTblRuleclassmap(_RuleId, _ClassId, int.Parse(Session["FeeId"].ToString())))
                    {
                        MyFeeMang.AddDateToTblRuleClassMap(_RuleId, _ClassId, _amnt, int.Parse(Session["FeeId"].ToString()));

                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Assign Rule To Class", "One  Rule  " + _rule + " is assigned to Class  " +_class, 1);
                    }
                    else
                    {
                        _msg = _msg + _rule + "  rule is assignend to " + _class + " class ,";
                    }
                    removegrdDate();
                }
                if (_msg == "")
                {
                    Lbl_msg.Text = "Rule Is saved";
                    this.MPE_MessageBox.Show();
                }
                else
                {
                    Lbl_msg.Text = _msg + " and Remaining is saved";
                    this.MPE_MessageBox.Show();
                    
                }

            }

        }

        private void removegrdDate()
        {
            foreach (GridViewRow gv in Grd_RuledEntry.Rows)
            {
                RemoveRow(gv);
            }


        }

        private void getRuleIdAndClassId(string _Rule, string _Class, out int _RuleId, out int _ClassId)
        {
            _RuleId = 0;
            _ClassId = 0;
            string sql = "select tblrules.Id from tblrules where tblrules.RuleName='" + _Rule + "'";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _RuleId =int .Parse(MyReader.GetValue(0).ToString());

            }
            sql = "select tblclass.Id from tblclass where tblclass.ClassName ='" + _Class + " '";
            MyReader1 = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                if(MyReader1.HasRows)
                {
                    _ClassId=int.Parse(MyReader1.GetValue(0).ToString());
                }
        }

        /*  protected void Grd_RuledEntry_RowDataBound(object sender, GridViewRowEventArgs e)
         {
             //Lbl_FailureNote.Text = "";
             if (e.Row.RowType == DataControlRowType.DataRow)
             {
                 LinkButton l = (LinkButton)e.Row.FindControl("LinkButton1");

                 l.Attributes.Add("onclick", "javascript:return " +
                      "confirm('Deleting the group " +
                      DataBinder.Eval(e.Row.DataItem, "GroupName") + " will remove all its members and data')");

             }
         }

        private object addPrevioursData()
         {
             DataSet _Daydataset = new DataSet();
             DataTable dt;
             DataRow dr;
             int i;
             _Daydataset.Tables.Add(new DataTable("feeclassRule"));
             dt = _Daydataset.Tables["feeclassRule"];
             // dt.Columns.Add("Count");
             dt.Columns.Add("Rulename");
             dt.Columns.Add("Amount");
             i = 0;
            
                 foreach (GridViewRow gv in Grd_RuledEntry.Rows)
                 {
                     i++;
                     dr = _Daydataset.Tables["feeclassRule"].NewRow();
                     //dr["Count"] = i;
                     dr["Rulename"] = gv.Cells[0].Text.ToString();
                     dr["Amount"] = gv.Cells[3].Text.ToString();
                     _Daydataset.Tables["feeclassRule"].Rows.Add(dr);


                     int _ruleId = int.Parse(Drp_Rules.SelectedValue.ToString());
                     string sql = "select tblrules.RuleName, tblrules.Amount  from   tblrules where  tblrules.Id=" + _ruleId;
                     MyReader1 = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                     if (MyReader1.HasRows)
                     {
                         dr = dt.NewRow();
                         dr["Rulename"] = MyReader1.GetValue(0).ToString();
                         dr["Amount"] = MyReader1.GetValue(1).ToString();
                         _Daydataset.Tables["feeclassRule"].Rows.Add(dr);
                     }

                 }
            
              protected void Btn_PopUpApprove_Click(object sender, EventArgs e)
         {
             if (Txt_IncidentId.Text.Trim() != "")
             {
                
                 if (MyIncedent.OnLIineApprovel(int.Parse(Txt_IncidentId.Text), out message))
                 {
                     MyIncedent.UpdateStatus(int.Parse(Txt_IncidentId.Text.Trim()));
                     FillGrid();
                     Lbl_msg.Text = "Incident is approved";
                     this.MPE_MessageBox.Show();
                 }else
                 {
                     Lbl_msg.Text = message;
                     this.MPE_MessageBox.Show();
                     FillGrid();
                 }
             }
             else
             {
                 Lbl_msg.Text = "Try again";
                 this.MPE_MessageBox.Show();
             }
         }
             return _Daydataset;

         }*/

        //private void addclassname()
        //{
        //    foreach (GridViewRow gv in Grd_RuledEntry.Rows)
        //    {


        //        Label Lbl_class = (Label)gv.FindControl("Lbl_class");


        //        Lbl_class.Text = Drp_class.SelectedItem.ToString();
        //        //Lbl_Batch.Text = MySearchMang.GetExamBatch(int.Parse(gv.Cells[1].Text.ToString()));

        //    }
        //}

       /* private object AddDateToGrid()
        {
            DataSet _Daydataset = new DataSet();
            DataTable dt;
            DataRow dr;
            int i;
            _Daydataset.Tables.Add(new DataTable("Holiday"));
            dt = _Daydataset.Tables["Holiday"];
            dt.Columns.Add("Rule Name");
            dt.Columns.Add("Amount");
            int _ruleId = int.Parse(Drp_Rules.SelectedValue.ToString());
            string sql = "select tblrules.RuleName, tblrules.Amount  from   tblrules where  tblrules.Id=" + _ruleId;
            MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                i = 0;
                foreach (GridViewRow gv in Grd_RuledEntry.Rows)
                {
                    i++;
                    dr = _Daydataset.Tables["Holiday"].NewRow();
                    //dr["Count"] = i;
                    dr["Date"] = gv.Cells[0].Text.ToString();
                    dr["month"] = gv.Cells[3].Text.ToString();
                    _Daydataset.Tables["Holiday"].Rows.Add(dr);

                }
            }
        }
        */
        protected void Btn_PopUpApprove_Click(object sender, EventArgs e)
        {
            int _RuleId;
            int _ClassId, _feeclassmapId;
            string _RuleName = Grd_RuledEntry.SelectedRow.Cells[1].Text;
            string _classname = Grd_RuledEntry.SelectedRow.Cells[2].Text;

            getRuleIdAndClassId(_RuleName, _classname, out _RuleId, out _ClassId);
            if (!checkIfRuleIsScheduled(_RuleId, _ClassId, int.Parse(Session["FeeId"].ToString())))
            {
                if (checkedIfThisPerticularEntryInTheRuleTbl(_RuleId, _ClassId, out _feeclassmapId))
                {
                    foreach (GridViewRow gv in Grd_RuledEntry.Rows)
                    {
                        if (gv.Cells[1].Text == _RuleName && gv.Cells[2].Text == _classname)
                        {
                            RemoveRow(gv);
                        }
                    }
                    deleteEntryFromFeeclassmap(_feeclassmapId);
                    Lbl_msg.Text = _RuleName + " this Rule is Removed from " + _classname + "  Class";
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Remove Rule From Class", "One  Rule  " + _RuleName + " is removed from Class  " + _classname, 1);
                    this.MPE_MessageBox.Show();
                }
            }
            else
            {
                Lbl_msg.Text = "Fee has already been scheduled.";
                this.MPE_MessageBox.Show();
            }


        }
    }
}
