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
    public partial class WebForm2 : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        private static int Selected_Id=0;
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
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadFees();
                }
            }
        }
       
        protected void Grd_ExamList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowState == DataControlRowState.Alternate)
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='silver';this.style.cursor='hand'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='White';");
                }
                else
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='silver';this.style.cursor='hand'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='White';");
                }
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Grd_FeeList, "Select$" + e.Row.RowIndex);
                LinkButton Status = (LinkButton)e.Row.FindControl("lnkView");
                if (Status.Text.Trim().Equals("Not Assign"))
                {
                    Status.ForeColor = System.Drawing.Color.Orange;
                }
                else
                {
                    Status.ForeColor = System.Drawing.Color.Green;
                }
            }
        }

        protected void LoadFees()
        {
            Grd_FeeList.Columns[1].Visible = true;
            string sql = "SELECT tblfeeaccount.Id,tblfeeaccount.AccountName, tblfeefrequencytype.FreequencyName, tblfeeasso.Name, tblfeetype.Name as FeeType , tblfeeaccount.Desciptrion,tbl_feesgrouphead.Name as Header FROM tblfeeaccount inner join  tblfeefrequencytype ON tblfeefrequencytype.Id = tblfeeaccount.FrequencyId inner join tblfeeasso on tblfeeasso.Id= tblfeeaccount.AssociatedId inner join tblfeetype on tblfeetype.Id = tblfeeaccount.`Type` inner join tbl_feesgrouphead on tbl_feesgrouphead.Id = tblfeeaccount.Header_Id where tblfeeaccount.Status=1";
            MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                if (!Is_Online_Payment_Enable())
                {
                    Grd_FeeList.Columns[7].Visible = true;
                }
                Grd_FeeList.DataSource = MydataSet;
                Grd_FeeList.DataBind();
                Lbl_Note.Text = "";
                Grd_FeeList.Columns[1].Visible = false;
                if (!Is_Online_Payment_Enable())
                {
                    Grd_FeeList.Columns[7].Visible = false;
                }
            }
            else
            {
                Grd_FeeList.DataSource = null;
                Grd_FeeList.DataBind();
                Lbl_Note.Text = "No Fees found";
            }
        }

        private bool Is_Online_Payment_Enable()
        {
            bool enable = false;
            string sql = "";
            sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='Online_Payment'";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    if (int.Parse(MyReader.GetValue(0).ToString()) == 1)
                    {
                        enable = true;
                    }
                }
            }
            return enable;
        }

        protected void Grd_ExamList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_FeeList.PageIndex = e.NewPageIndex;
            LoadFees();
        }

        protected void Grd_FeeList_Header_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "select")
            {
                int Selected_Id = 0;
                int index = Convert.ToInt32(e.CommandArgument);
                int.TryParse(Grd_FeeList.Rows[index].Cells[1].Text, out Selected_Id);
                Load_Heaser_Groups();
                Load_Edit_Details(Selected_Id);
                Lbl_Edt_Msg.Text = "";
                MPE_FeesGroupPopUp.Show();

            }
            if (e.CommandName == "View")
            {
                int Selected_Id = 0;
                int index = Convert.ToInt32(e.CommandArgument);
                int.TryParse(Grd_FeeList.Rows[index].Cells[1].Text, out Selected_Id);
                if (!MyFeeMang.JoiningFee(Selected_Id))
                {
                    Session["FeeId"] = Selected_Id;
                    Response.Redirect("FeeDetails.aspx");
                }
                else
                {
                    Session["FeeId"] = null;
                    if (MyUser.HaveActionRignt(125))
                        Response.Redirect("ManageJoiningFee.aspx?FeeId=" + Selected_Id + "");
                    else
                        WC_MessageBox.ShowMssage("You do not have the right to access this option");
                }
            }
        }
        private void Load_Edit_Details(int Fees_Id)
        {
            DataSet MyDataSet = new DataSet();
            string sql = "select Id,AccountName,Header_Id from tblfeeaccount where Id=" + Fees_Id + "";
            MyDataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyDataSet != null && MyDataSet.Tables[0] != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {
                    Lbl_Fees.Text = dr["AccountName"].ToString();
                    Drplist_Header.SelectedValue = dr["Header_Id"].ToString();
                }
            }
        }
        private void Load_Heaser_Groups()
        {
            DataSet MyDataSet = new DataSet();
            string sql = "select Id,Name,Address from tbl_feesgrouphead";
            MyDataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyDataSet != null && MyDataSet.Tables[0] != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                Drplist_Header.Items.Clear();
                Drplist_Header.DataSource = MyDataSet;
                Drplist_Header.DataTextField = "Name";
                Drplist_Header.DataValueField = "Id";
                Drplist_Header.DataBind();
                Btn_Update.Enabled = true;

            }
            else
            {
                Drplist_Header.Items.Clear();
                ListItem li = new ListItem("No Header Exist", "-1");
                Drplist_Header.Items.Add(li);
                Btn_Update.Enabled = false;
            }
        }
        protected void Btn_Update_Click(object sender, EventArgs e)
        {
            try
            {
                int Header_Id=0;
                int.TryParse(Drplist_Header.SelectedValue,out Header_Id);
                string sql = "Update tblfeeaccount set Header_Id=" + Header_Id + " where Id=" + Selected_Id + " ";
                MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Fees Group", "fee Group " + Drplist_Header.SelectedItem.Text + " mapped updated", 1);
                LoadFees();
                Lbl_Edt_Msg.Text = "Group Fees Header Mapped Successfully";

            }
            catch (Exception et)
            {
                Lbl_Edt_Msg.Text = et.Message;
            }
            MPE_FeesGroupPopUp.Show();
        }
        protected void Btn_Clear_Click(object sender, EventArgs e)
        {
            MPE_FeesGroupPopUp.Hide();
        }
    }
}