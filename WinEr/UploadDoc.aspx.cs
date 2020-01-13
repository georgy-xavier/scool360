using System;
using System.Data.Odbc;
using System.Net;
using System.IO;
using System.Configuration;
using System.Data;
using WinBase;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using AjaxControlToolkit;
using System.Drawing;
using WinEr;


namespace WinEr
{
    public partial class UploadDoc : System.Web.UI.Page
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
                    LoadDocType();
                    // SubjectTypeLoading();
                   
                }
            }
        }
        protected void Btn_UploadDoc_Click(object sender, EventArgs e)
        {
            objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];

            
            string docType = Drp_Doctype.SelectedItem.Text;

            
            string empId = Session["StudId"].ToString();
            int SchoolId = WinerUtlity.GetSchoolId(objSchool);
            int docId = 0;
            string _message = "";
            string storeLocation = "";
            MysqlClass _MysqlDb = new MysqlClass(ConfigurationSettings.AppSettings["DMSConnectionInfo"]);
            using (OdbcConnection _MyODBCConn = new OdbcConnection(ConfigurationSettings.AppSettings["DMSConnectionInfo"]))
            {
                //string mop = "";
                _MyODBCConn.Open();
                string sql = "select max(UploadDocId) from insertdoc ";
                MydataSet = _MysqlDb.ExecuteQueryReturnDataSet(sql);
                if(MydataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in MydataSet.Tables[0].Rows)
                    {
                        if(dr[0].ToString() == "")
                        {
                            docId = 1;
                        }
                        else
                         docId = int.Parse(dr[0].ToString()) + 1;
                    }
                }

                System.IO.Stream fs = FileUp_Student.PostedFile.InputStream;
                System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
               



                string fileName = FileUp_Student.FileName;
               




                if (FileUp_Student.FileBytes.Length <= 5242880)
                {
                    storeLocation = "DB";
                    string m_Str_sql1 = "insert into insertdoc (UploadDocId,DocName,DocType,SchoolID,EmpID,EmpCode,Resume,ResumeName,StoreLocation) values (" + docId + ",'" + fileName + "','" + docType + "'," + SchoolId + ",'" + empId + "','" + empId + "','" + base64String + "','" + fileName + "','" + storeLocation + "')";
                    MydataSet = _MysqlDb.ExecuteQueryReturnDataSet(m_Str_sql1);
                    _message = "Document Uploaded";
                }
                else
                {
                    storeLocation = "FileExplorer";
                 
                    FileUp_Student.SaveAs(MyUser.FilePath + "\\UpImage\\" + empId + "_" + SchoolId + "_"+ fileName);
                    string sql1 = " insert into insertdoc (UploadDocId, DocName, DocType, SchoolID, EmpID, EmpCode, Resume, ResumeName, StoreLocation) values(" + docId + ", '" + fileName + "', '" + docType + "', " + SchoolId + ", '" + empId + "', '" + empId + "', '" + storeLocation + "', '" + fileName + "', '" + storeLocation + "')";
                    MydataSet = _MysqlDb.ExecuteQueryReturnDataSet(sql1);
                    _message = "Document Uploaded";
                }


            }
            Lbl_msg.Text = _message;
            MPE_MessageBox.Show();
        }
        private void LoadDocType()
        {
            objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];


            MysqlClass _MysqlDb = new MysqlClass(ConfigurationSettings.AppSettings["DMSConnectionInfo"]);
            using (OdbcConnection _MyODBCConn = new OdbcConnection(ConfigurationSettings.AppSettings["DMSConnectionInfo"]))
            {

                _MyODBCConn.Open();
                {
                    int SchoolId = WinerUtlity.GetSchoolId(objSchool);
                    Drp_Doctype.Items.Clear();
                    string sql = "SELECT Id,DocumentType FROM uploaddoc where SchoolId=" + SchoolId + "";

                    MyReader = _MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        while (MyReader.Read())
                        {
                            ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                            Drp_Doctype.Items.Add(li);


                        }
                    }
                }
            }


        }




    }
}