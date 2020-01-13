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
    public partial class ManageJoiningFee : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyFeeMang = MyUser.GetFeeObj();
            if (MyFeeMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(125))
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (Request.QueryString["FeeId"] == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    
                    LoadTopDetails();
                    LoadGrid();
                }
            }

        }

        private void LoadGrid()
        {
            Grd_FeeList.Columns[0].Visible = true;
            string sql = "select Id,Name from tblstandard";
            MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables.Count > 0)
            {
                Grd_FeeList.DataSource = MydataSet;
                Grd_FeeList.DataBind();
                LoadAmount();
            }
            else
            {
                Grd_FeeList.DataSource = null;
                Grd_FeeList.DataBind();
            }
            Grd_FeeList.Columns[0].Visible = false;
        }

        private void LoadAmount()
        {
            foreach (GridViewRow gv in Grd_FeeList.Rows)
            {
                TextBox Txt_FeeAmt = (TextBox)gv.FindControl("Txt_FeeAmount");
                Txt_FeeAmt.Text = MyFeeMang.GetAmountForClass(int.Parse(gv.Cells[0].Text), int.Parse(Request.QueryString["FeeId"].ToString()));
            }
        }

        private void LoadTopDetails()
        {
            string sql = "SELECT tblfeeaccount.AccountName, tblfeefrequencytype.FreequencyName, tblfeeasso.Name from tblfeeaccount inner join tblfeefrequencytype on tblfeefrequencytype.Id= tblfeeaccount.FrequencyId inner join tblfeeasso on tblfeeasso.Id = tblfeeaccount.AssociatedId where tblfeeaccount.Id=" + int.Parse(Request.QueryString["FeeId"].ToString());
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

        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            Lbl_Message.Text = "";
            if (Txt_Amount.Text.Trim() != "")
            {
                foreach (GridViewRow gv in Grd_FeeList.Rows)
                {
                    TextBox Txt_FeeAmt = (TextBox)gv.FindControl("Txt_FeeAmount");
                    Txt_FeeAmt.Text = Txt_Amount.Text;
                }
            }
        }

        protected void Byn_Save_Click(object sender, EventArgs e)
        {
            bool valid = true;
            foreach (GridViewRow gv in Grd_FeeList.Rows)
            {
                TextBox Txt_FeeAmt = (TextBox)gv.FindControl("Txt_FeeAmount");
                if (Txt_FeeAmt.Text.Trim()== "")
                Txt_FeeAmt.Text = "0";
                if (!MyFeeMang.GetSaveAmount(double.Parse(Txt_FeeAmt.Text.Trim()), int.Parse(gv.Cells[0].Text), int.Parse(Request.QueryString["FeeId"].ToString())))
                {
                    valid = false;
                } 
            }
            if (valid)
            {
                Lbl_Message.Text = "Fee scheduled Successfully";
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Schedule Joining Fee", "Fee "+Lbl_FeeName.Text+" is Scheduled ", 1);

            }
        }

        protected void Btn_magok_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageFeeAccount.aspx");
        }

        protected void Btn_DeleteFee_Click(object sender, EventArgs e)
        {
            string _Messsage = "";
            int FeeId = int.Parse(Request.QueryString["FeeId"].ToString());
            if (!MyFeeMang.DeleteJoiningFee(FeeId, out _Messsage))
            {
                Lbl_Message.Text = _Messsage;

            }
            else
            {
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Delete Joining Fee", "Fee " + Lbl_FeeName.Text + " is Deleted ", 1);
                Lbl_msg.Text = _Messsage;
                MPE_MessageBox.Show();
            }
        }
    }
}
