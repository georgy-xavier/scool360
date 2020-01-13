using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.Odbc;
namespace WinEr
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
       // private DataSet MydataSet;
        private int BatchId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (Session["FeeId"] == null)
            {
                Response.Redirect("ManageFeeAccount.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyFeeMang = MyUser.GetFeeObj();
            if (MyFeeMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(38))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (Rdo_Batch.SelectedValue == "0")
                    BatchId = MyUser.CurrentBatchId;
                else
                    BatchId = MyUser.CurrentBatchId + 1;
                if (!IsPostBack)
                {
                    string _MenuStr;
                    _MenuStr = MyFeeMang.GetSubFeeMangMenuString(MyUser.UserRoleId, int.Parse(Session["FeeId"].ToString()));
                    this.SubFeeMenu.InnerHtml = _MenuStr;
                    if (MyFeeMang.HasNextBatchSchedule())
                    {
                        Label_NextBatch.Visible = true;
                        Rdo_Batch.Visible = true;
                    }
                    else
                    {
                        Label_NextBatch.Visible = false;
                        Rdo_Batch.Visible = false;
                    }
                    LoadDetails();
                    AddPeriodToDrp();
                    AddClassToChkBox(); 
                    //some initlization

                }
            }

        }
       
        private void AddClassToChkBox()
        {
            ChkBox_Class.Items.Clear();
            string sql = "SELECT tblclass.Id, tblclass.ClassName from tblclass where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") AND  tblclass.Id NOT IN ( SELECT tblfeeschedule.ClassId from tblfeeschedule where tblfeeschedule.BatchId=" + BatchId + " AND tblfeeschedule.FeeId=" + Session["FeeId"].ToString() + " AND tblfeeschedule.PeriodId=" + Drp_Perod1.SelectedValue.ToString() + ")  order by tblclass.Standard, tblclass.ClassName";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_Schdule.Enabled = true;
                while (MyReader.Read())
                {
                    
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), int.Parse(MyReader.GetValue(0).ToString()).ToString());
                    if (!MyFeeMang.HaveStudentInClass(int.Parse(MyReader.GetValue(0).ToString()), MyUser.CurrentBatchId))
                    {
                        Btn_Schdule.Enabled = true;
                        li.Text = li.Text + " (No Student)";
                    }
                    else
                    {
                        Btn_Schdule.Enabled = true;
                    }
                    ChkBox_Class.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Class Present", "-1");
                Btn_Schdule.Enabled = false;
                li.Enabled = false;
                ChkBox_Class.Items.Add(li);
              
            }

            Lnk_btn_select.Text = "Select All";
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
        private void AddPeriodToDrp()
        {
            Drp_Perod1.Items.Clear();
            string sql = "SELECT  tblperiod.Id, tblperiod.Period from tblperiod inner join tblfeeaccount on tblfeeaccount.FrequencyId = tblperiod.FrequencyId where tblfeeaccount.Id=" + +int.Parse(Session["FeeId"].ToString()) + " ORDER BY tblperiod.Order";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), int.Parse(MyReader.GetValue(0).ToString()).ToString());
                    Drp_Perod1.Items.Add(li);
                }
            }
        }
        protected void Btn_Schdule_Click(object sender, EventArgs e)
        {
            Btn_Schdule.Enabled = false;
             string Feemessage="";
             bool _flag = false;
            if (Txt_amount.Text.Trim() == "" || Txt_dudate.Text.Trim() == "" || Txt_lastdate.Text.Trim() == "")
            {
                Lbl_msg.Text = "One or more fields are Empty";
                
                Btn_Schdule.Enabled = true;
            }
           
            else
            {
                //DateTime _duedate = DateTime.Parse(Txt_dudate.Text.ToString());
                //DateTime _lastdate = DateTime.Parse(Txt_lastdate.Text.ToString());
                DateTime _duedate = MyUser.GetDareFromText(Txt_dudate.Text.ToString());
                DateTime _lastdate = MyUser.GetDareFromText(Txt_lastdate.Text.ToString());
                if (_lastdate < _duedate)
                {
                    Lbl_msg.Text = "Last date cannot be less than due date";
                   
                    Btn_Schdule.Enabled = true;
                }
                else
                {
                    for (int i = 0; i < ChkBox_Class.Items.Count; i++)
                    {
                        if (ChkBox_Class.Items[i].Selected)
                        {
                            _flag = true;
                            if (MyFeeMang.HaveStudentInClass(int.Parse(ChkBox_Class.Items[i].Value.ToString()), MyUser.CurrentBatchId))
                            {
                                if (MyFeeMang.IsFeeScheduled(int.Parse(ChkBox_Class.Items[i].Value.ToString()), int.Parse(Drp_Perod1.SelectedValue.ToString()), BatchId, int.Parse(Session["FeeId"].ToString())))
                                {
                                    Feemessage = Feemessage + ChkBox_Class.Items[i].Text + " ";
                                }
                                else
                                {
                                    if (MyFeeMang.CheckForRuleApplicableToClassAndFee1(int.Parse(ChkBox_Class.Items[i].Value.ToString()), int.Parse(Session["FeeId"].ToString())))
                                    {

                                        MyFeeMang.ScheduleClassFeeAccordingTotheRule(double.Parse(Txt_amount.Text.ToString()), int.Parse(Session["FeeId"].ToString()), int.Parse(ChkBox_Class.Items[i].Value.ToString()), BatchId, int.Parse(Drp_Perod1.SelectedValue.ToString()), _duedate, _lastdate,MyUser.CurrentBatchId);
                                        Lbl_msg.Text = "Fee is scheduled";

                                        MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Fee Scheduling", Lbl_FeeName.Text + " is Scheduled for " + ChkBox_Class.Items[i].Text, 1,1);


                                    }
                                    else
                                    {
                                        MyFeeMang.ScheduleClassFee(int.Parse(ChkBox_Class.Items[i].Value.ToString()), int.Parse(Drp_Perod1.SelectedValue.ToString()), _duedate, _lastdate, double.Parse(Txt_amount.Text.ToString()), BatchId, int.Parse(Session["FeeId"].ToString()), MyUser.CurrentBatchId);
                                        Lbl_msg.Text = "Fee is scheduled";

                                        MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Fee Scheduling", Lbl_FeeName.Text + " is Scheduled for " + ChkBox_Class.Items[i].Text, 1,1);

                                        
                                    }

                                }

                            }
                            else
                            {
                                MyFeeMang.insertintoscheduletbl(double.Parse(Txt_amount.Text.ToString()), int.Parse(Session["FeeId"].ToString()), int.Parse(ChkBox_Class.Items[i].Value.ToString()), BatchId, int.Parse(Drp_Perod1.SelectedValue.ToString()), _duedate, _lastdate);
                                Lbl_msg.Text = "Fee Scheduled without student";
                                MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Fee Scheduling", Lbl_FeeName.Text + " is Scheduled without student for " + ChkBox_Class.Items[i].Text, 1,1);


                            }
                        }
                        
                    }
                    if (Feemessage != "")
                    {
                        Lbl_msg.Text = "The fee is already scheduled for " + Feemessage + ". ";
                        
                        Clear();
                    }
                    AddClassToChkBox();
                    if (_flag == false)
                    {
                        Lbl_msg.Text = "Class Not Selected.Please Select the Class ";
                        
                    }

                    Clear();
                }

            }
            this.MPE_MessageBox.Show();
           

        }

        private void Clear()
        {
            Txt_amount.Text = "";
            Txt_lastdate.Text = "";
            Txt_dudate.Text = "";
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageFeeAccount.aspx");
        }

        protected void Lnk_btn_select_Click(object sender, EventArgs e)
        {
          
                 if (Lnk_btn_select.Text == "Select All")
                 {
                     for (int i = 0; i < ChkBox_Class.Items.Count; i++)
                     {
                         if (ChkBox_Class.Items[i].Enabled == true)
                         {
                             ChkBox_Class.Items[i].Selected = true;


                         }
                     }
                     Lnk_btn_select.Text = "None";

                 }
                 else
                 {
                     for (int i = 0; i < ChkBox_Class.Items.Count; i++)
                     {
                         if (ChkBox_Class.Items[i].Enabled == true)
                         {
                             ChkBox_Class.Items[i].Selected = false;


                         }
                     }
                     Lnk_btn_select.Text = "Select All";

                 }
        }

        protected void Drp_Perod1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddClassToChkBox(); 

        }

        protected void Rdo_Batch_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            AddClassToChkBox(); 
        }
    }
}
