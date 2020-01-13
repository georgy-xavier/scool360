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
using System.Drawing;
using System.IO;

namespace WinEr
{
    public partial class PrinterSetUp : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
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
            else if (!MyUser.HaveActionRignt(45))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadBillTypes();
                    LoadDefaultValues();
                }
            }
        }
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


       
        private void LoadBillTypes()
        {

            Drp_BillType.Items.Clear();
            string sql = "select id,name from tblfeebilltype order by id";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_BillType.Items.Add(li);
                }
            }
            MyReader.Close();
        }

        private void LoadDefaultValues()
        {
            DataSet mydataset = new DataSet();
            string sql = "SELECT Value FROM tblconfiguration WHERE id in (4,11,12,21,22,23)";
            mydataset = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            
                txt_billprefix.Text = mydataset.Tables[0].Rows[0][0].ToString();
                if (mydataset.Tables[0].Rows[1][0].ToString() == "1")
                {
                    chk_Clrcheque.Checked = true;
                }
                if (mydataset.Tables[0].Rows[2][0].ToString() == "1")
                {
                    chk_Clrdd.Checked = true;
                }
                Drp_BillType.SelectedValue = mydataset.Tables[0].Rows[3][0].ToString();
                if (mydataset.Tables[0].Rows[4][0].ToString() == "1")
                {
                    Chk_SchAllowforNextBatch.Checked = true;
                }
                if (mydataset.Tables[0].Rows[5][0].ToString() == "1")
                {
                    Chk_AutoSettelement.Checked = true;
                }
        }

        protected void Btn_BillPrinter_Click(object sender, EventArgs e)
        {

            string billprfx = "",sql = "";
            int ClrDD = 0, ClrChq = 0, BillType=0;
            int NextYearSch = 0;
            int AutoSettle = 0;
            int.TryParse(Drp_BillType.SelectedValue.ToString(), out BillType); 
            billprfx = txt_billprefix.Text;
            if (chk_Clrcheque.Checked)
            {
                ClrChq = 1;
            }
            if (chk_Clrdd.Checked)
            {
                ClrDD = 1;
            }
            if (Chk_SchAllowforNextBatch.Checked)
            {
                NextYearSch = 1;
            }
            if (Chk_AutoSettelement.Checked)
            {
                AutoSettle = 1;
                MyFeeMang.ISAdvanceAutoCancel = "1";
            }
            
            sql = "UPDATE tblconfiguration SET Value='" + billprfx + "' WHERE id=4";
            MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            sql = "UPDATE tblconfiguration SET Value='" + ClrChq + "' WHERE id=11";
            MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            sql = "UPDATE tblconfiguration SET Value='" + ClrDD + "' WHERE id=12";
            MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            sql = "UPDATE tblconfiguration SET Value='" + BillType + "' WHERE id=21";
            MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            sql = "UPDATE tblconfiguration SET Value='" + NextYearSch + "' WHERE id=22";
            MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            sql = "UPDATE tblconfiguration SET Value='" + AutoSettle + "' WHERE id=23";
            MyFeeMang.m_MysqlDb.ExecuteQuery(sql);

            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Fee Configuration", "Fee settings have been updated",2);
            WC_MessageBox.ShowMssage("Bill settings are updated..");
        }

        protected void BtnCncl_Click(object sender, EventArgs e)
        {
            LoadDefaultValues();
        }

        
       
    }
}
