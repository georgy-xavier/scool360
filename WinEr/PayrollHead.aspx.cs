using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;
using System.Data.Odbc;
namespace WinEr.Payroll
{
     
    public partial class PayrollHead : System.Web.UI.Page
    {
        private WinBase.Payroll Mypay;
        private KnowinUser MyUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            //if (Session["StudId"] == null)
            //{
            //    Response.Redirect("PayrollHead.aspx");
            //}
            MyUser = (KnowinUser)Session["UserObj"];
            Mypay = MyUser.GetPayrollObj();
            if (Mypay == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(800))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {


                if (!IsPostBack)
                {
                    clear();
                    Session["SortExpression"] = null;
                    Session["SortDirection"] = null;
                    Rdb_TypeEarn.Checked = true;
                    RdbYes.Checked = true;
                    Btn_Save.Visible = false;
                    Btn_Cancel.Visible = false;
                    Btn_Add.Visible = true;
                    LoadGrid();
                    //some initlization

                }
            }
        }
       
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            int Decrease = 0;
            if (Txt_Head.Text.Trim() == "")
            {
                WC_MessageBox.ShowMssage("Please Enter Head Name");
            }
            else
            {

                string _HeadName = Txt_Head.Text.Trim();
                string _Comment = Txt_Comment.Text.Trim();
                string _Type = "";
                if (Rdb_TypeEarn.Checked)
                {
                    _Type = "Earnings";
                }
                else
                {
                    _Type = "Deductions";
                }
                if (RdbYes.Checked)
                {
                    Decrease = 1;
                }
                else
                {
                    Decrease = 0;
                }
                if (Mypay.IsPresentHead(_HeadName))
                {
                    WC_MessageBox.ShowMssage("This Head Name is Already Present");
                }
                else
                {

                    Mypay.AddHead(_HeadName, _Type, _Comment, Decrease,MyUser.UserName);

                    LoadGrid();
                    Txt_Comment.Text = "";
                    Txt_Head.Text = "";
                    Rdb_TypeEarn.Checked = true;
                    Rdb_TypeDed.Checked = false;
                    WC_MessageBox.ShowMssage("Successfully created");
                }

            }
        }

        private void LoadGrid()
        {
            DataSet MyHead = Mypay.GetHeads();
            if (MyHead != null && MyHead.Tables != null && MyHead.Tables[0].Rows.Count > 0)
            {
                Grd_PayHead.Columns[0].Visible = true;
                Grd_PayHead.Columns[3].Visible = true;
                Grd_PayHead.DataSource = MyHead;
                Grd_PayHead.DataBind();
                Grd_PayHead.Columns[0].Visible = false;
                Grd_PayHead.Columns[3].Visible = false;
                Pnl_PayHead.Visible = true;

            }
            else
            {
                Grd_PayHead.DataSource = null;
                Grd_PayHead.DataBind();
            }
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            if (Txt_Head.Text.Trim() == "")
            {
                WC_MessageBox.ShowMssage("Please Enter Head Name");
            }
            else
            {
                string _HeadName = Txt_Head.Text.Trim();
                string _Comment = Txt_Comment.Text.Trim();
                string _Type = "";
                int DecType = 0;
                if (Rdb_TypeEarn.Checked)
                {
                    _Type = "Earnings";
                }
                else
                {
                    _Type = "Deductions";
                }

                if (RdbYes.Checked)
                {
                    DecType = 1;
                }
                else
                {
                    DecType = 0;
                }
                int _Id = int.Parse(Grd_PayHead.Rows[Grd_PayHead.SelectedIndex].Cells[0].Text);
                if (Mypay.IsUpdatePresentHead(_HeadName, _Id))
                {
                    WC_MessageBox.ShowMssage("This Head Name is Already Present");
                }
                else
                {
                    Mypay.SaveHead(_Id, _HeadName, _Type, _Comment, DecType,MyUser.UserName);
                    LoadGrid();
                    clear();
                    Btn_Save.Visible = false;
                    Btn_Add.Visible = true;
                    Btn_Cancel.Visible = false;
                }
            }
        }
        protected void Grd_PayHead_Selectedindexchanged(object sender, EventArgs e)
        {
            Btn_Add.Visible = false;
            Btn_Save.Visible = true;
            Btn_Cancel.Visible = true;
            Grd_PayHead.Columns[0].Visible = true;
            Grd_PayHead.Columns[3].Visible = true;
            Txt_Head.Text = Grd_PayHead.Rows[Grd_PayHead.SelectedIndex].Cells[1].Text;
            Txt_Comment.Text = Grd_PayHead.Rows[Grd_PayHead.SelectedIndex].Cells[4].Text.Replace("&nbsp;","");
            int _DecType = int.Parse(Grd_PayHead.Rows[Grd_PayHead.SelectedIndex].Cells[3].Text.ToString());
            string _TypeId = Grd_PayHead.Rows[Grd_PayHead.SelectedIndex].Cells[2].Text;
            if (_TypeId == "Earnings")
            {
                Rdb_TypeEarn.Checked = true;
                Rdb_TypeDed.Checked = false;
            }
            else 
            {
                Rdb_TypeDed.Checked = true;
                Rdb_TypeEarn.Checked = false;
            }
            if (_DecType == 1)
            {
                RdbYes.Checked = true;
                RdbNo.Checked = false;
            }
            else
            {
                RdbYes.Checked = false;
                RdbNo.Checked = true;
            }
            //OdbcDataReader MyPayReader = null;
            //MyPayReader = Mypay.Fill(_Id);

            Grd_PayHead.Columns[0].Visible = false;
            Grd_PayHead.Columns[3].Visible = false;
        }
        protected void Grd_PayHead_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton l = (LinkButton)e.Row.FindControl("LinkButton1");
                l.Attributes.Add("onclick", "javascript:return " +
                     "confirm('Are you sure you want to delete this Record ')");
            }
        }
        protected void Grd_PayHead_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int _HeadID = int.Parse(Grd_PayHead.DataKeys[e.RowIndex].Value.ToString());
            string Headname = Grd_PayHead.Rows[e.RowIndex].Cells[1].Text.ToString();
            if (Mypay.CheckHeadFixed(_HeadID))
            {
                WC_MessageBox.ShowMssage("This Head is assigned to some employees, hence cannot be Deleted");
            }

            else if (Mypay.CheckHeadFixedForCategory(_HeadID))
            {
                WC_MessageBox.ShowMssage("This Head is assigned to some category, hence cannot be Deleted");

            }
            else
            {

                Mypay.DeleteHead(_HeadID);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Delete payrollhead", "payrollhead " + Headname + " is deleted", 1);
                LoadGrid();
                clear();
                Btn_Save.Visible = false;
                Btn_Add.Visible = true;
            }

           

        }

      

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            clear();
            Btn_Save.Visible = false;
            Btn_Add.Visible = true;
            LoadGrid();
            Btn_Cancel.Visible = false;
            RdbYes.Checked = true;
            RdbNo.Checked = false;
        }

        protected void clear()
        {
            Txt_Comment.Text = "";
            Txt_Head.Text = "";
            Rdb_TypeEarn.Checked = true;
            Rdb_TypeDed.Checked = false;
        }
    }
}
