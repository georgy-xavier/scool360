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
public partial class FeeDetails : System.Web.UI.Page
{
    private FeeManage MyFeeMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    private DataSet MydataSet;
    private int MasterBatchId;
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
        else
        {
            if (Rdo_Batch.SelectedValue == "0")
                MasterBatchId = MyUser.CurrentBatchId;
            else
                MasterBatchId = MyUser.CurrentBatchId + 1;
            if (!IsPostBack)
            {
                string _MenuStr;
                _MenuStr = MyFeeMang.GetSubFeeMangMenuString(MyUser.UserRoleId, int.Parse(Session["FeeId"].ToString()));
                this.SubFeeMenu.InnerHtml = _MenuStr;
                LoadDetails();
                if (MyUser.HaveActionRignt(15))
                {
                    LoadFeeScheduleDetails();
                }
                else
                {
                    Pnl_feedetailarea.Visible = false;
                }
                if (MyUser.HaveActionRignt(75))
                {
                    LoadFeeRulesDetails();
                }
                else
                {
                    pnl_FeeRule.Visible = false;
                }
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
                //some initlization

            }
        }

    }
    

    private void LoadFeeRulesDetails()
    {


        LoadGrd_ViewRule();

    }

    private void LoadGrd_ViewRule()
    {
        pnl_FeeRule.Visible = true;
        string sql = "select tblrules.RuleName  , tblclass.ClassName from tblrules inner join tblruleclassmap on tblruleclassmap.RuleId = tblrules.Id inner join tblclass on tblruleclassmap.classId = tblclass.Id where tblruleclassmap.feeTypeId=" + int.Parse(Session["FeeId"].ToString()) + " order by tblclass.Standard,tblclass.ClassName desc ";
        MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
            Grd_VewRuleDetails.DataSource = MydataSet;
            Grd_VewRuleDetails.DataBind();

        }
        else
        {
            pnl_FeeRule.Visible = false;
        }


    }


    

    private void LoadFeeScheduleDetails()
    {
        if (AddClassToDropDownClass())
        {
            this.Feeschtable.InnerHtml = MyFeeMang.GetFeeScheduleDetailsTableString(int.Parse(Drp_className.SelectedValue.ToString()), int.Parse(Session["FeeId"].ToString()),MyUser.CurrentBatchId,MasterBatchId);
        }
        else
        {
            Pnl_feedetailarea.Visible = false;
        }
    }
    private bool AddClassToDropDownClass()
    {
        bool _valid;
        Drp_className.Items.Clear();

        MydataSet = MyUser.MyAssociatedClass();
        if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr in MydataSet.Tables[0].Rows)
            {

                ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                Drp_className.Items.Add(li);

            }
            _valid = true;

        }
        else
        {
            ListItem li = new ListItem("No Class Present", "-1");
            Drp_className.Items.Add(li);
            
            _valid = false;
        }

        Drp_className.SelectedIndex = 0;
        return _valid;
        
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

    protected void Drp_className_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.Feeschtable.InnerHtml = MyFeeMang.GetFeeScheduleDetailsTableString(int.Parse(Drp_className.SelectedValue.ToString()), int.Parse(Session["FeeId"].ToString()), MyUser.CurrentBatchId,MasterBatchId);
    }

    protected void Rdo_Batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.Feeschtable.InnerHtml = MyFeeMang.GetFeeScheduleDetailsTableString(int.Parse(Drp_className.SelectedValue.ToString()), int.Parse(Session["FeeId"].ToString()), MyUser.CurrentBatchId, MasterBatchId);
    }
}
