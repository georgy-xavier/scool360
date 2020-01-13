using System;
using System.Data.Odbc;
using System.Net;

using System.IO;
using System.Configuration;
using System.Data;
using WinBase;

namespace WinEr
{
    public partial class CreateDocType : System.Web.UI.Page
    {

        private ConfigManager MyConfigMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        private SchoolClass objSchool = null;

        protected void Page_init(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyConfigMang = MyUser.GetConfigObj();
            if (MyConfigMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(14))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    // SubjectTypeLoading();

                }
            }
        }
      
        protected void Btn_CreateDocType_Click(object sender, EventArgs e)
        {
            objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
            string _message = "";
            int SchoolId = WinerUtlity.GetSchoolId(objSchool);
            string Status = "active";
            string CreatedBy = MyUser.UserName;
            DateTime CreatedAt = DateTime.Now;
            string UpdatedBy = MyUser.UserName;
            DateTime UpdatedAt = DateTime.Now;
            string docType = Txt_DocType.Text;
            string Fieldvalidation = Drp_Mandatory.SelectedValue.ToString();
            string docName = "";
            string EmpId = "";

            MysqlClass _MysqlDb = new MysqlClass(ConfigurationSettings.AppSettings["DMSConnectionInfo"]);
            using (OdbcConnection _MyODBCConn = new OdbcConnection(ConfigurationSettings.AppSettings["DMSConnectionInfo"]))
            {
                string mop = "";
                _MyODBCConn.Open();
                string m_Str_sql1 = "insert into uploaddoc (SchoolId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,DocumentName,DocumentType,EmpId,Status,FieldValidation) values (" + SchoolId + ",'" + CreatedBy + "','" + CreatedAt.ToString("s") + "','" + UpdatedBy + "','" + UpdatedAt.ToString("s") + "','" + docName + "','" + docType + "' ,'" + EmpId + "','" + Status + "','" + Fieldvalidation + "')";
                MydataSet = _MysqlDb.ExecuteQueryReturnDataSet(m_Str_sql1);
                _message = "Document Type Created";
            }
            Lbl_msg.Text = _message;
            MPE_MessageBox.Show();
        }




    }
}