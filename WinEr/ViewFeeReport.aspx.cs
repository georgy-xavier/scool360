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
    public partial class ViewFeeReport : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        //private DataSet MydataSet;
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
            else if (!MyUser.HaveActionRignt(21))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    if (LoadBatchDrp())
                    {
                        LoadFeeRoport();
                    }


                }
            }

        }

        private void LoadFeeRoport()
        {

            this.Feeschtable.InnerHtml = MyFeeMang.GetFeeReportTableString(int.Parse(Drp_BatchName.SelectedValue.ToString()));
        
            
        }

        private bool LoadBatchDrp()
        {
            bool _Retval = false;
            Drp_BatchName.Items.Clear();
            string sql = "SELECT Id,BatchName FROM tblbatch where Created=1 AND Id In (select tblfeeschedule.BatchId from tblfeeschedule union select Distinct(tblview_feebill.BatchId) AS `Id` from tblview_feebill) AND Id<" + MyUser.CurrentBatchId + " ORDER by tblbatch.Id";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_BatchName.Items.Add(li);
                }
               
            }
            ListItem _li = new ListItem(MyUser.CurrentBatchName, MyUser.CurrentBatchId.ToString());
            Drp_BatchName.Items.Add(_li);
            Drp_BatchName.SelectedValue = MyUser.CurrentBatchId.ToString();
            _Retval = true;
            return _Retval;
            
        }

        protected void Drp_BatchName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFeeRoport();
        }

        protected void ImgBtn_ExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            string _Table = "<table><tr><td>Batch : " + Drp_BatchName.SelectedItem.Text + "    Created Date:" + DateTime.Now.ToString() + "</td></tr><tr><td>" + this.Feeschtable.InnerHtml + "</td></tr></table>";
            string FileName = Drp_BatchName.SelectedItem.Text+ "FeeStatement";

            if (!WinEr.ExcelUtility.ExportBuiltStringToExcel(FileName,_Table, FileName))
            {
                WC_MessageBox.ShowMssage("Unable to export to Excel or try again later");
                

            }
        }
    }
}
