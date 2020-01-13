using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;

namespace WinEr
{
    public partial class RuleConfiguration : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private ConfigManager MyConfiMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private FeeManage myfeemager = null;
        string sql;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyConfiMang = MyUser.GetConfigObj();
            myfeemager = MyUser.GetFeeObj();
            MyFeeMang = MyUser.GetFeeObj();
            if (MyConfiMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(73))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                   loaddrp_category();
                   load_rules();
                }
            }

        }

        private void load_rules()
        {
            Lbl_gridmsg.Text = "";
            DataSet Mydataset = GetDataset();
            if (Mydataset != null && Mydataset.Tables[0].Rows.Count > 0)
            {
                GridViewRuleDetails.Columns[0].Visible = true;
                GridViewRuleDetails.DataSource = Mydataset;
                GridViewRuleDetails.DataBind();
                GridViewRuleDetails.Columns[0].Visible = false;
            }
            else
            {
                Lbl_gridmsg.Text = "No rules found";
                GridViewRuleDetails.DataSource = null;
                GridViewRuleDetails.DataBind();
            }

            
        }

        private DataSet GetDataset()
        {
            DataSet Ruledataset = new DataSet();
            DataTable _table;

            Ruledataset.Tables.Add(new DataTable("datas"));
            _table = Ruledataset.Tables["datas"];
            _table.Columns.Add("Id");
            _table.Columns.Add("Rule Name");
            _table.Columns.Add("Type");
            _table.Columns.Add("Amount");
            _table.Columns.Add("Assign Mode");
            _table.Columns.Add("Category");

            string sql = "SELECT tblrules.Id,tblrules.RuleName,tblrules.Amounttype,tblrules.Amount,tblrules.AssigMode,tblruleitem.name,tblrules.FieldValue FROM tblrules INNER JOIN tblruleitem ON  tblruleitem.Id=tblrules.tblruleitemId";
            MyReader = myfeemager.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    DataRow _row = _table.NewRow();
                    _row["Id"] = MyReader.GetValue(0).ToString();
                    _row["Rule Name"] = MyReader.GetValue(1).ToString();
                    string _type = "Fixed";
                    if (MyReader.GetValue(2).ToString() == "2")
                    {
                        _type = "Percentage";
                    }
                    _row["Type"] = _type;
                    _row["Amount"] = MyReader.GetValue(3).ToString();
                    _row["Assign Mode"] = MyReader.GetValue(4).ToString();
                    _row["Category"] = MyReader.GetValue(5).ToString();

                    Ruledataset.Tables["datas"].Rows.Add(_row);
                }
            }
            return Ruledataset;
        }

        private void loaddrp_category()
        {
            ListItem li = new ListItem("None", "-1");
            Drp_category.Items.Add(li);
            sql = "SELECT tblruleitem.Id,  tblruleitem.name from tblruleitem";
            MyReader = myfeemager.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                   li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_category.Items.Add(li);

                }
                

            }
        }

            
        protected void Drpcategory_select(object sender, EventArgs e)
        {
            string _id;
            string _tablename;
            string _tblcol;
            string _colVale;
            int _havemaster;
            _id = Drp_category.SelectedValue.ToString();
            if (_id == "-1")
            {
                Txt_subcategory.Visible = false;
                Drp_subcategory.Visible = false;

            }
            sql = "SELECT tblruleitem.havemaster ,tblruleitem.mastertblname,tblruleitem.MasterDiplyName,tblruleitem.valuecol from tblruleitem where tblruleitem.id=" + _id;
            MyReader = myfeemager.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _havemaster =int.Parse(MyReader.GetValue(0).ToString());
                if (_havemaster == 1)
                {
                    Txt_subcategory.Visible = false;
                    Drp_subcategory.Visible = true;
                    Drp_subcategory.Items.Clear() ;
                    _tablename = MyReader.GetValue(1).ToString();
                    _tblcol = MyReader.GetValue(2).ToString();
                    _colVale = MyReader.GetValue(3).ToString();
                    sql = "select DISTINCT(" + _tablename + "." + _colVale + " )," + _tablename + "." + _tblcol + " from  " + _tablename;
                    MyReader = myfeemager.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {                        
                        while (MyReader.Read())
                        {
                           ListItem li1 = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                           Drp_subcategory.Items.Add(li1);

                        }
                    }
                    
                }
                else
                {
                    Drp_subcategory.Visible = false;
                    Txt_subcategory.Visible = true;
                }
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
           
            int _TypeId;
            float _amount;
            int _ruleitemId;
            string _assgmode;
            string _category;
            int  _havemaster;
            string _FieldValue="";
            if (checkAllFeildRFilled()==true)
            {
                _category = Drp_category.SelectedItem.ToString();
                _assgmode = Drp_assigmentMode.SelectedItem.ToString();
                if (!myfeemager.CheckPresenceOfRuleName(txt_RuleName.Text.ToString().ToLower()))
                {
                    if (myfeemager.Ckeck4AssgedValueIsCorrect(_assgmode, _category) == true)
                    {
                        if (Drp_Type.SelectedItem.ToString() == "Fixed")
                        {
                            _TypeId = 1;
                        }
                        else
                        {
                            _TypeId = 2;
                        }
                        if (float.TryParse(Txt_value.Text.ToString(), out _amount))
                        {

                            if (Rdo_addsub.SelectedValue == "2")
                            {
                                _amount = -1 * _amount;
                            }


                            _ruleitemId = myfeemager.GetRuleItemId(_category, out _havemaster);
                            if (_havemaster == 1)
                            {
                                _FieldValue = Drp_subcategory.SelectedValue.ToString();
                                myfeemager.AddToRuleTbl(txt_RuleName.Text.ToString().ToLower(), _TypeId, _amount, _ruleitemId, _assgmode, _FieldValue);
                                WC_MessageBox.ShowMssage("Rule Created");
                                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Created One Fee Rule", "Fee Rule is created with name " + txt_RuleName.Text, 1);

                                clearall();

                            }
                            else
                            {
                                if (Txt_subcategory.Text != "")
                                {
                                    _FieldValue = Txt_subcategory.Text.ToString();
                                    myfeemager.AddToRuleTbl(txt_RuleName.Text.ToString().ToLower(), _TypeId, _amount, _ruleitemId, _assgmode, _FieldValue);
                                    WC_MessageBox.ShowMssage("Rule Created");
                                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Created One Fee Rule", "Fee Rule is created with name " + txt_RuleName.Text, 1);

                                    clearall();

                                }
                                else
                                {
                                    WC_MessageBox .ShowMssage("Please Enter Value Category Sub Text box");
                                   
                                    Txt_subcategory.Focus();

                                }
                            }

                            //myfeemager.AddToRuleTbl(txt_RuleName.Text.ToString().ToLower(), _TypeId, _amount, _ruleitemId, _assgmode, _FieldValue);
                            //WC_MessageBox .ShowMssage("Rule Created");
                            //MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create One Fee Rule", "Fee Rule is created with name " + txt_RuleName.Text, 1);
                            
                            //clearall();

                        }
                        else
                        {

                             WC_MessageBox .ShowMssage("Plase Check the value field");
                            
                            Txt_value.Text = "";
                            Txt_value.Focus();
                        }
                    }
                    else
                    {
                         WC_MessageBox .ShowMssage("Assigment can be only Equal in this case");
                        
                    }
                }
                else
                {
                     WC_MessageBox .ShowMssage("Rule name Already exist");
                    txt_RuleName.Text = "";
                    txt_RuleName.Focus();
                    
                }

            }

            load_rules();

        }

        private void clearall()
        {
            txt_RuleName.Text = "";
            Txt_subcategory.Text = "";
            Txt_value.Text = "";
            Txt_subcategory.Visible = false;
            Drp_subcategory.Visible = false;
            Drp_assigmentMode.ClearSelection();
            Drp_category.ClearSelection();
            Drp_Type.ClearSelection();
            txt_RuleName.Focus();
            
            
        }

     

        private bool checkAllFeildRFilled()
        {
            bool _flag = false;
            if (txt_RuleName.Text != "" && Txt_value.Text != "" && Drp_category.Text != "None" && Drp_Type.Text != "None" && Drp_assigmentMode.Text != "None")
            {
                _flag = true;
            }
            else
            {
                 WC_MessageBox .ShowMssage("Some Fields are Empty");
              
            }

            return _flag;
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            clearall();
        }





        private void RemoveRuleEntry(int _Id)
        {
            string _msg = "";
            if (IsRuleDeletionPossible(_Id, out _msg))
            {
                DeleteRules(_Id);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Delete Fee Rule", "Fee Rule deleted ", 1);
                load_rules();
            }
            else
            {
                WC_MessageBox.ShowMssage(_msg);
            }
        }

        private void DeleteRules(int _Id)
        {
            string sql = "DELETE FROM tblrules WHERE Id=" + _Id;
            myfeemager.m_MysqlDb.ExecuteQuery(sql);
        }

        private bool IsRuleDeletionPossible(int _Id, out string _msg)
        {
            bool _valid = true;
            _msg = "";
            if (IsRuleMapped(_Id))
            {
                _msg = "You cannot delete this rule. Because rule has been mapped to some fees.";
                _valid = false;
            }
            return _valid;
        }

        private bool IsRuleMapped(int _Id)
        {
            bool _valid = false;
            string sql = "SELECT COUNT(tblruleclassmap.Id) FROM tblruleclassmap WHERE tblruleclassmap.RuleId=" + _Id;
             MyReader = myfeemager.m_MysqlDb.ExecuteQuery(sql);
             if (MyReader.HasRows)
             {
                 int _count = 0;
                 int.TryParse(MyReader.GetValue(0).ToString(), out _count);
                 if (_count > 0)
                 {
                     _valid = true;
                 }
             }
             return _valid;
        }

        protected void GridViewRuleDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _Id = 0;
            if (e.CommandName == "AddToCart")
            {
                // Retrieve the row index stored in the 
                // CommandArgument property.
                int index = Convert.ToInt32(e.CommandArgument);

                // Retrieve the row that contains the button 
                // from the Rows collection.
                GridViewRow row = GridViewRuleDetails.Rows[index];

                // Add code here to add the item to the shopping cart.

                string Id_str = row.Cells[0].Text.ToString();
               
                int.TryParse(Id_str, out _Id);
                
            }
            if (_Id > 0)
            {
                RemoveRuleEntry(_Id);
            }
        }

    }
}
