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
    public partial class WebForm10 : System.Web.UI.Page
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
            else if (!MyUser.HaveActionRignt(1))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                  
                   
                    AddValueToDropDownFrequency();                    
                    AddValueToAssoTo();
                    AddFeeDetailsTOGrid();
                    LoadFeeType();
                    LoadOtherFeeList();
                }
            }

        }

        private void LoadFeeType()
        {
            //Rdo_Feetype
            Rdo_Feetype.Items.Clear();
            string sql = "SELECT Id,Name FROM tblfeetype";
            if(!HasjoinigFee())
            {
                sql = sql + " where Name NOT LIKE '%Joining Fee'";
            }
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Rdo_Feetype.Items.Add(li);

                }
                Rdo_Feetype.SelectedIndex = 0;
            }
        }

        private bool HasjoinigFee()
        {
             bool valid = false;
             string sql = "select ActionId from tblmoduleactionmap where ActionId=127";
             MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
             if (MyReader.HasRows)
             {
                 valid = true;
             }
             return valid;
        }

        
       

        private void AddFeeDetailsTOGrid()
        {


            string sql = "SELECT tblfeeaccount.AccountName As 'Fee Name', tblfeefrequencytype.FreequencyName AS 'Feequency Type', tblfeeasso.Name AS 'Associated To',tblfeetype.Name AS 'Fee Type'  FROM tblfeeaccount inner join  tblfeefrequencytype ON tblfeefrequencytype.Id = tblfeeaccount.FrequencyId inner join tblfeeasso on tblfeeasso.Id= tblfeeaccount.AssociatedId inner join tblfeetype on tblfeetype.Id = tblfeeaccount.`Type` where tblfeeaccount.Status=1";
            MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                GridViewFeeDetails.DataSource = MydataSet;
                GridViewFeeDetails.DataBind();

            }
            else
            {
                GridViewFeeDetails.DataSource = null;
                GridViewFeeDetails.DataBind();


            }

        }

        private void AddValueToAssoTo()
        {
            Drp_Asso.Items.Clear();
            string sql = "SELECT Id,Name FROM tblfeeasso ";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Asso.Items.Add(li);

                }
                Drp_Asso.SelectedIndex = 0;
            }
        }

        private void AddValueToDropDownFrequency()
        {
            Drp_Frequency.Items.Clear();
            string sql = "SELECT `Id`, `FreequencyName` FROM tblfeefrequencytype ";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Frequency.Items.Add(li);

                }
                Drp_Frequency.SelectedIndex = 0;
            }
        }

        protected void LinkDetails_Click(object sender, EventArgs e)
        {
            if (LinkDetails.Text != "Hide Fees")
            {

                Panel2.Visible = true;
                LinkDetails.Text = "Hide Fees";
            }
            else
            {
                LinkDetails.Text = "Show All Fees";
                Panel2.Visible = false;
            }
        }

        protected void Btn_Create_Click(object sender, EventArgs e)
        {
          string _msg= "";
            

            if (Txt_FeeName.Text.Trim() == "")
            {
                _msg = "Fee Name cannot be empty...";
               

            }
            else if (!MyFeeMang.IsValidFeeName(Txt_FeeName.Text.ToString()))
            {
                _msg = "FeeName Already Present Try New FeeName... ";

            }
            else
            {
                                               

                MyFeeMang.CreateFee(Txt_FeeName.Text, Txt_Desc.Text, int.Parse(Drp_Frequency.SelectedValue.ToString()), int.Parse(Drp_Asso.SelectedValue.ToString()), MyUser.UserId , int.Parse(Rdo_Feetype.SelectedValue));
                ClearAll();
                _msg = "Fee is created..... ";
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create Fee Account", "A new fee account "+ Txt_FeeName.Text.ToString() +"  is created", 1);
                AddFeeDetailsTOGrid();
            }
            WC_MessageBox.ShowMssage(_msg);
           
        }

        private void ClearAll()
        {
            Txt_Desc.Text = "";
            Txt_FeeName.Text = "";

        }

        protected void Btn_cancel_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        protected void Rdo_Feetype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Rdo_Feetype.SelectedValue == "2")//if joiing Fee
            {
                Drp_Frequency.Visible = false;
                Lbl_Frequency.Visible = false;
                Drp_Asso.Visible = false;
                Lbl_Asso.Visible = false;
            }
            else
            {
                Drp_Frequency.Visible = true;
                Drp_Asso.Visible = true;
                Lbl_Frequency.Visible = true;
                Lbl_Asso.Visible = true;
            }
        }

        # region  OtherFee


        protected void Btn_SaveOthr_Click(object sender, EventArgs e)
        {
            string _Message = "";
            if (ValidData(out _Message))
            {
                MyFeeMang.SaveOtherFee(Txt_Name.Text.Trim(),Txt_Description.Text.Trim(),0);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create Fee Account", "A new fee account " + Txt_Name.Text.ToString() + "  is created", 1);
                WC_MessageBox.ShowMssage("Fee created successfully");
                Clear();
                LoadOtherFeeList();
            }
            else
                WC_MessageBox.ShowMssage(_Message);
        }

        private void LoadOtherFeeList()
        {
            Lbl_OtherFeeMessage.Text = "";
            DataSet myotherFees = MyFeeMang.GetOtherfeeList();
            Grd_OtherFeeList.Columns[0].Visible = true;
            if (myotherFees != null && myotherFees.Tables != null && myotherFees.Tables[0].Rows.Count > 0)
            {
                Grd_OtherFeeList.DataSource = myotherFees;
                Grd_OtherFeeList.DataBind();
            }
            else
            {
                Grd_OtherFeeList.DataSource = null;
                Grd_OtherFeeList.DataBind();
                Lbl_OtherFeeMessage.Text = "No fee found";
            }
            Grd_OtherFeeList.Columns[0].Visible = false;

        }

        private void Clear()
        {
            Txt_Name.Text = "";
            Txt_Description.Text = "";
        }

        private bool ValidData(out string _Message)
        {
            _Message = "";
            bool _Valid = true;
            if (MyFeeMang.OtherFeeExists(Txt_Name.Text.Trim()))
            {
                _Message = "The fee "+Txt_Name.Text.Trim()+" exists already";
                return false;
            }
            return _Valid;
        }

        # endregion

    }
}
