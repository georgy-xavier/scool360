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
public partial class RemoveFee : System.Web.UI.Page
{
    private FeeManage MyFeeMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    //private DataSet MydataSet;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_init(object sender, EventArgs e)
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
        else if (!MyUser.HaveActionRignt(39))
        {
            Response.Redirect("RoleErr.htm");
        }
        else
        {
            if (!IsPostBack)
            {
                string _MenuStr;
                _MenuStr = MyFeeMang.GetSubFeeMangMenuString(MyUser.UserRoleId, int.Parse(Session["FeeId"].ToString()));
                this.SubFeeMenu.InnerHtml = _MenuStr;
                LoadDetails();
               
                //some initlization

            }
        }
    }

    private void LoadDetails()
    {

        string sql = "SELECT tblfeeaccount.AccountName, tblfeeaccount.Desciptrion, tblfeefrequencytype.FreequencyName, tblfeeasso.Name from tblfeeaccount inner join tblfeefrequencytype on tblfeefrequencytype.Id= tblfeeaccount.FrequencyId inner join tblfeeasso on tblfeeasso.Id = tblfeeaccount.AssociatedId where tblfeeaccount.Id=" + int.Parse(Session["FeeId"].ToString());
        MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            MyReader.Read();
            Txt_feename.Text = MyReader.GetValue(0).ToString();
            Txt_Desc.Text = MyReader.GetValue(1).ToString();  
            Lbl_FeeName.Text = MyReader.GetValue(0).ToString();
            Lbl_Freq.Text = MyReader.GetValue(2).ToString();
            Lbl_asso.Text = MyReader.GetValue(3).ToString();
            
        }
        MyReader.Close();
    }
    protected void Btn_Finish_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageFeeAccount.aspx");
       
    }
    protected void Btn_remove_Click(object sender, EventArgs e)
    {
        string _msg = "";
        if (MyFeeMang.RemoveFeeAccount(int.Parse(Session["FeeId"].ToString()), MyUser.CurrentBatchId, out _msg))
        {
            Lbl_feedeleted.Text = "Fee Deleted.";
            
            Session["FeeId"] = null;
            this.MPE_LastMessage.Show();
            MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Delete Fee", "Delete the Fee  " + Txt_feename.Text + "  From the software", 1,1);
        }
        else
        {
            Lbl_msg.Text = _msg;
            this.MPE_MessageBox.Show();

        }
        

    }

    protected void Btn_edit_Click(object sender, EventArgs e)
    {
            UpdateFeedetails();
    }

    private void UpdateFeedetails()
    {
        Lbl_msg.Text = "";

        if (Txt_feename.Text.Trim() == "")
        {
            Lbl_msg.Text = "Fee Name cannot be empty...";

            
        }
        else if (!MyFeeMang.IsValidFeeName1(Txt_feename.Text.ToString() ,Lbl_FeeName.Text.ToString()))
        {
            Lbl_msg.Text = "FeeName Already Present Try New FeeName... ";

        }
        else
        {

            MyFeeMang.UpdateFeeDetails(int.Parse(Session["FeeId"].ToString()),Txt_feename.Text, Txt_Desc.Text);
            Lbl_FeeName.Text = Txt_feename.Text;
            Lbl_msg.Text = "Fee is Updated..... ";
            MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Update Fee", "Update the Fee  " + Txt_feename.Text + " details", 1,1);
           
        }
        this.MPE_MessageBox.Show();
    }

   
}
